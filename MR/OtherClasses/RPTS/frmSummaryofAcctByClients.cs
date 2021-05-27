#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using msfunc;
using msfunc.Forms;


using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmSummaryofAcctByClients : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date, start_opening;
        DataSet ds = new DataSet();
        DataTable sdt, finalsdt, dtadjusttype = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM AdjustmentCodes order by name", true);
        public frmSummaryofAcctByClients()
        {
            InitializeComponent();
        }
        private void frmSummaryofAcctByClients_Load(object sender, EventArgs e)
        {

        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btngroupcode")
            {
                this.txtgroupcode.Text = "";
                lookupsource = "g";
                msmrfunc.mrGlobals.crequired = "g";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btnPatientno")
            {
                txtpatientno.Text = "";
                lookupsource = "L";
                msmrfunc.mrGlobals.crequired = "L";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btngrouphead")
            {
                txtgrouphead.Text = "";
                lookupsource = "C";
                msmrfunc.mrGlobals.crequired = "C";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORP. CLIENTS";
            }
            frmselcode FrmSelCode = new frmselcode();
            FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
            FrmSelCode.ShowDialog();
        }
        void FrmSelCode_Closed(object sender, EventArgs e)
        {
            frmselcode FrmSelcode = sender as frmselcode;
            if (lookupsource == "g") //groupcodee
            {
                this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
                this.txtgroupcode.Focus();
            }
            else if (lookupsource == "L") //patientno
            {
                txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Select();
            }
            else if (lookupsource == "C") //CORPORATE
            {
                txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtgrouphead.Select();
            }
            return;
        }
        private void txtPatientno_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpatientno.Text))
                return;
            if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
            {
                txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
            }
            //check if patientno exists
            bchain = billchaindtl.Getbillchain(txtpatientno.Text, txtgroupcode.Text);
            if (bchain == null)
            {
                DialogResult result = MessageBox.Show("Invalid Patient Number... ");
                txtpatientno.Text = " ";
                txtgroupcode.Select();
                return;
            }
            lblName.Text = bchain.NAME;
        }
        private void txtgrouphead_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtgrouphead.Text))
                return;
            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + txtgrouphead.Text + "'", false);
            if (dtcustomer.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("Invalid Corporate Clients Reference...");
                txtgrouphead.Text = "";
                return;
            }
            lblName.Text = dtcustomer.Rows[0]["name"].ToString();
        }
        void createSummary()
        {
            sdt = new DataTable(); //table to will be passed to report dataset 
            if (chkSegmented.Checked)
            {
                sdt.Columns.Add(new DataColumn("GROUPHEAD", typeof(string)));
                sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
                sdt.Columns.Add(new DataColumn("BALBF", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("OPDBILLS", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("ADMISSIONS", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("CASHPMTS", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("BANKCREDITS", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("DBNOTES", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("CRNOTES", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("BALANCE", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("GROUPHTYPE", typeof(string)));
            }
            else
            {
                sdt.Columns.Add(new DataColumn("GROUPHEAD", typeof(string)));
                sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
                sdt.Columns.Add(new DataColumn("BALBF", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("PAYMENTS", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("DBNOTES", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("CRNOTES", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("BALANCE", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("CUR_BILLS", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("NEWBALANCE", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("GROUPHTYPE", typeof(string)));
            }
        }
        DataRow createnewRow(DataRow drow,decimal xamount, string xtype)
        {
            bool foundit = false;
            decimal db, cr, adj; db = cr = adj = 0m;
            decimal opbal = 0m;
            DataRow dr = null;

            foreach (DataRow row in sdt.Rows)
            {
                if (drow["GHGROUPCODE"].ToString().Trim() + drow["grouphead"].ToString().Trim() == row["GROUPHEAD"].ToString().Trim())
                {
                    dr = row;
                    foundit = true;
                    break;
                }
            }
            if (!foundit)
            {
                dr = sdt.NewRow();
                dr["GROUPHEAD"] = drow["GHGROUPCODE"].ToString().Trim() + drow["grouphead"].ToString().Trim();
                dr["name"] = drow["name"].ToString();
                opbal = msmrfunc.getOpeningBalance(drow["GHGROUPCODE"].ToString().Trim(), drow["grouphead"].ToString().Trim(), "", string.IsNullOrWhiteSpace(drow["GHGROUPCODE"].ToString()) ? "C" : "P", start_opening, dtDateFrom.Value, ref db, ref cr, ref adj);
                dr["BALBF"] = opbal;
                if (xamount == 0 && opbal == 0) //no transaction no opening balance
                    return dr;
                if (chkSegmented.Checked)
                {
                    dr["OPDBILLS"] = 0;
                    dr["ADMISSIONS"] = 0;
                    dr["CASHPMTS"] = 0;
                    dr["BANKCREDITS"] = 0;
                }
                else
                {
                    dr["PAYMENTS"] = 0;
                    dr["CUR_BILLS"] = 0;
                    dr["NEWBALANCE"] = 0;
                }
                dr["DBNOTES"] = 0;
                dr["CRNOTES"] = 0;
                dr["BALANCE"] = 0;
               // dr["GROUPHTYPE"] = 0;
                dr["GROUPHTYPE"] = string.IsNullOrWhiteSpace(drow["GHGROUPCODE"].ToString()) ? "C" : "P";
                sdt.Rows.Add(dr);
            }
            if (chkSegmented.Checked)
            {
                if (xtype == "OPD")
                    dr["opdbills"] = (decimal)dr["opdbills"] + xamount;
                if (xtype == "ADM")
                    dr["ADMISSIONS"] = (decimal)dr["admissions"] + xamount;
                else if (xtype == "CASH")
                    dr["CASHPMTS"] = (decimal)dr["cashpmts"] + xamount;
                if (xtype == "BC")
                    dr["BANKCREDITS"] = (decimal)dr["bankcredits"] + xamount;
                else if (xtype == "DN")
                    dr["dbnotes"] = (decimal)dr["dbnotes"] + xamount;
                else if (xtype == "CN")
                    dr["crnotes"] = (decimal)dr["crnotes"] + xamount;
                dr["balance"] = (((decimal)dr["BALBF"] + (decimal)dr["OPDBILLS"] + (decimal)dr["ADMISSIONS"] + (decimal)dr["dbnotes"]) - ((decimal)dr["CASHPMTS"] + (decimal)dr["BANKCREDITS"] + (decimal)dr["crnotes"]));
            }
            else
            {
                if (xtype == "CURBILL")
                    dr["cur_bills"] = xamount;
                else if (xtype == "PMT")
                    dr["payments"] = xamount;
                else if (xtype == "DN")
                    dr["dbnotes"] = xamount;
                else if (xtype == "CN")
                    dr["crnotes"] = xamount;
                dr["balance"] = (((decimal)dr["BALBF"] + (decimal)dr["dbnotes"]) - ((decimal)dr["payments"] + (decimal)dr["crnotes"]));
                dr["newbalance"] = (decimal)dr["cur_bills"] + (decimal)dr["balance"];
            }
 
            return dr;
        }
        bool getData()
        {
            start_opening = bissclass.ConvertStringToDateTime("01", msmrfunc.mrGlobals.mlastperiod == 12 ? "01" : msmrfunc.mrGlobals.mlastperiod + 1.ToString(), msmrfunc.mrGlobals.mlastperiod == 12 ? msmrfunc.mrGlobals.mpyear + 1.ToString() : msmrfunc.mrGlobals.mpyear.ToString());
            string bstr = "", pstr = "", astr = "";
            bstr = " WHERE BILLING.TRANS_DATE >= '" + start_opening + "' and BILLING.TRANS_DATE <= '" + dtDateTo.Value.Date + "'";
            pstr = " WHERE PAYDETAIL.TRANS_DATE >= '" + start_opening + "' and PAYDETAIL.TRANS_DATE <= '" + dtDateTo.Value.Date + "'";
            astr = "WHERE BILL_ADJ.TRANS_DATE >= '" + start_opening + "' and BILL_ADJ.TRANS_DATE <= '" + dtDateTo.Value.Date + "'";
            if (chkCorporate.Checked)
            {
                bstr += " AND BILLING.TRANSTYPE = 'C'";
                pstr += " AND PAYDETAIL.TRANSTYPE = 'C'";
                astr += " AND BILL_ADJ.TRANSTYPE = 'C'";
            }
            else if (chkFamily.Checked)
            {
                bstr += " AND BILLING.TRANSTYPE = 'P' and BILLING.GHGROUPCODE = 'FC'";
                pstr += " AND PAYDETAIL.TRANSTYPE = 'P' and BILLING.GHGROUPCODE = 'FC'";
                astr += " AND BILL_ADJ.TRANSTYPE = 'P' and BILLING.GHGROUPCODE = 'FC'";
            }
            else if (chkPrivate.Checked)
            {
                bstr += " AND BILLING.TRANSTYPE = 'P' and RTRIM(BILLING.GHGROUPCODE) = 'PVT'";
                pstr += " AND PAYDETAIL.TRANSTYPE = 'P' and RTRIM(BILLING.GHGROUPCODE) = 'PVT'";
                astr += " AND BILL_ADJ.TRANSTYPE = 'P' and RTRIM(BILLING.GHGROUPCODE) = 'PVT'";
            }
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
            {
                bstr += " AND BILLING.GROUPHEAD = '" + txtgrouphead.Text + "'";
                pstr += " AND PAYDETAIL.GROUPHEAD = '" + txtgrouphead.Text + "'";
                astr += " AND BILL_ADJ.GROUPHEAD = '" + txtgrouphead.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
            {
                bstr += " AND BILLING.GHGROUPCODE = '" + txtgroupcode.Text + "'";
                pstr += " AND PAYDETAIL.GHGROUPCODE = '" + txtgroupcode.Text + "'";
                pstr += " AND BILL_ADJ.GHGROUPCODE = '" + txtgroupcode.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
            {
                bstr += " AND BILLING.GROUPHEAD = '" + txtpatientno.Text + "'";
                pstr += " AND PAYDETAIL.GROUPHEAD = '" + txtpatientno.Text + "'";
                astr += " AND BILL_ADJ.GROUPHEAD = '" + txtpatientno.Text + "'";
            }

            string onhold = !chkIncludeOnHold.Checked ? " WHERE bill_cir <> 'H'" : "";
            string xstring = "",xstr;
            if (chkCorporate.Checked)
                xstring = "select customer.custno AS grouphead, char(9) as ghgroupcode, customer.name, customer.balbf from customer " + onhold;
            else if (chkFamily.Checked || chkPrivate.Checked)
            {
                xstr = chkFamily.Checked ? " and rtrim(patient.groupcode) = 'FC'" : " and rtrim(patient.groupcode) = 'PVT'";
                if (chkPrivate.Checked && !string.IsNullOrWhiteSpace(cboPVTNameFrom.Text))
                {
                    xstr += " and (left(patient.name,1) >= '" + cboPVTNameFrom.Text.Trim() + "' and patient.name <= '" + cboPVTNameTo.Text.Trim() + "')";
                }
                onhold = !chkIncludeOnHold.Checked ? " and bill_cir <> 'H'" : "";
                xstring = "select patient.groupcode AS ghgroupcode, patient.patientno As grouphead, patient.name, patient.balbf from patient where patient.isgrouphead = '1'" + xstr + onhold;
            }
            //else
            //    xstring = "select customer.custno AS grouphead, char(9) as ghgroupcode, customer.name, customer.balbf from customer UNION select patient.groupcode AS ghgroupcode, patient.patientno As grouphead, patient.name, patient.balbf from patient where patient.isgrouphead = '1'";

            DataTable dtgrphead = Dataaccess.GetAnytable("", "MR", xstring, false);
            DataTable ttd;
            xstr = ""; string pxstr = "";
            if (chkSegmented.Checked)
                xstr = "select reference, amount, ghgroupcode, grouphead from ";
            else
                xstr = "select Sum(amount) AS AMOUNT, GHGROUPCODE, grouphead from ";
            string xselstr = "",xgrp = "",xttype = "";
            if (chkPrivate.Checked && dtgrphead.Rows.Count > 1000 && (cboPVTNameFrom.Text.Trim() != cboPVTNameTo.Text.Trim() || string.IsNullOrWhiteSpace(cboPVTNameFrom.Text)))
            {
                MessageBox.Show("Private Patient Accounts are too many for Selected Criteria " + dtgrphead.Rows.Count.ToString() + " Seen");
                return false;
            }
            foreach (DataRow row in dtgrphead.Rows)
            {
                xgrp = chkSegmented.Checked ? "" : " group by GHGROUPCODE, grouphead";
                xselstr = " where grouphead = '" + row["grouphead"].ToString() + "' and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateTo.Value.ToShortDateString() + " 23:59:59:999' "+xgrp;

                ttd = Dataaccess.GetAnytable("", "MR", xstr + " billing " + xselstr, false);
                if (ttd.Rows.Count > 0)
                {
                    if (chkSegmented.Checked)
                    {
                        foreach (DataRow trow in ttd.Rows )
	                    {
                            xttype = trow["reference"].ToString().Substring(0,1) == "A" ? "ADM" : "OPD";
                            createnewRow(row, (decimal)trow["amount"], xttype );
	                    }
                    }
                    else
                        createnewRow(row, (decimal)ttd.Rows[0]["amount"], "CURBILL");
                }
                else
                {
                    createnewRow(row, 0m, "CURBILL");
                }
                if (chkSegmented.Checked)
                {
                    pxstr = "select reference, amount, ghgroupcode, grouphead, paytype from paydetail where grouphead = '" + row["grouphead"].ToString() + "' and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateTo.Value.ToShortDateString() + " 23:59:59:999' ";
                    ttd = Dataaccess.GetAnytable("", "MR", pxstr, false);
                }
                else
                    ttd = Dataaccess.GetAnytable("", "MR", xstr + " paydetail " + xselstr, false);
                if (ttd.Rows.Count > 0)
                {
                    if (chkSegmented.Checked)
                    {
                        foreach (DataRow trow in ttd.Rows)
                        {
                            xttype = trow["paytype"].ToString().Substring(0, 1) == "C" ? "CASH" : "BC";
                            createnewRow(row, (decimal)trow["amount"], xttype);
                        }
                    }
                    else
                        createnewRow(row, (decimal)ttd.Rows[0]["amount"], "PMT");
                }
                if (chkSegmented.Checked)
                    xstr = "select Sum(amount) AS AMOUNT, GHGROUPCODE, grouphead from ";

                xselstr = " where grouphead = '" + row["grouphead"].ToString() + "' and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateTo.Value.ToShortDateString() + " 23:59:59:999' and ttype = 'D' group by GHGROUPCODE, grouphead";
                ttd = Dataaccess.GetAnytable("", "MR", xstr + " bill_adj " + xselstr, false);
                if (ttd.Rows.Count > 0)
                    createnewRow(row, (decimal)ttd.Rows[0]["amount"], "DN");
                xselstr = " where grouphead = '" + row["grouphead"].ToString() + "' and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateTo.Value.ToShortDateString() + " 23:59:59:999' and ttype = 'C' group by GHGROUPCODE, grouphead";
                ttd = Dataaccess.GetAnytable("", "MR", xstr + " bill_adj " + xselstr, false);
                if (ttd.Rows.Count > 0)
                    createnewRow(row, (decimal)ttd.Rows[0]["amount"], "CN");
                if (chkSegmented.Checked)
                    xstr = "select reference, amount, ghgroupcode, grouphead from ";
            }
            //we must scan to remove balances according to user specs
            finalsdt = sdt.Clone();
            foreach (DataRow row in sdt.Rows)
            {
                if (!chkSegmented.Checked && (decimal)row["BALBF"] == 0 && (decimal)row["dbnotes"] == 0 && (decimal)row["payments"] == 0 && (decimal)row["crnotes"] == 0 && (decimal)row["cur_bills"] == 0 && (decimal)row["balance"] == 0 || chkSegmented.Checked && (decimal)row["BALBF"] == 0 && (decimal)row["dbnotes"] == 0 && (decimal)row["cashpmts"] == 0 && (decimal)row["bankcredits"] == 0 && (decimal)row["crnotes"] == 0 && (decimal)row["opdbills"] == 0 && (decimal)row["admissions"] == 0 && (decimal)row["balance"] == 0)
                {
                    //row.Delete();
                    continue;
                }
                if (chkDebitBal.Checked && (decimal)row["balance"] < 1 || chkCreditBal.Checked && (decimal)row["balance"] >= 1)
                {
                    //row.Delete();
                    continue;
                }
                finalsdt.ImportRow(row);
            }
            return true;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            if (dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateTo.Value < dtDateFrom.Value || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateTo.Value.Date > DateTime.Now.Date || dtDateFrom.Value.Month != dtDateTo.Value.Month || dtDateTo.Value.Year != dtDateFrom.Value.Year )
            {
                DialogResult result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            if (chkPrivate.Checked && (string.IsNullOrWhiteSpace(cboPVTNameFrom.Text) || string.IsNullOrWhiteSpace( cboPVTNameTo.Text)))
            {
                MessageBox.Show("Name Filter SHOULD be selected for Private Patients Reports");
                //return;
            }
            Session["sql"] = "";
            if (chkMthlyComparativeSumm.Checked)
            {
               /* if (ds != null)
                {
                    ds.Tables.Clear();
                    ds.Clear();
                }*/
                ds = new DataSet();
                string xrectype = chkCorporate.Checked ? "C" : chkFamily.Checked ? "F" : "P";
                DataTable dtmthlyfig = new DataTable(), dtaggregate = new DataTable();
                msmrfunc.processComparative(ref dtmthlyfig, ref dtaggregate, dtDateFrom.Value, dtDateTo.Value, "CLIENTS", xrectype);
                ds.Tables.Add(dtmthlyfig);
                ds.Tables.Add(dtaggregate);

                Session["rdlcfile"] = "ComparativeRpt.rdlc";
            }
            else
            {
                //if (sdt == null)
                createSummary();
                ds = new DataSet();
                if (chkSegmented.Checked)
                    Session["rdlcfile"] = "SummaryOfAcctsbyClientsSgmtd.rdlc";
                else
                    Session["rdlcfile"] = "SummaryOfAcctsbyClients.rdlc";
                if (!getData())
                    return;
               // ds.Tables.Add(sdt);
                ds.Tables.Add(finalsdt);
            }
            string mrptheader = "SUMMARY OF ACCOUNTS BY CLIENTS";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, " FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateTo.Value.ToShortDateString(), "", "", "", "SUMMARYOFACCTS", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "W", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader + " FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateTo.Value.ToShortDateString(), "", "", "", "SUMMARYOFACCTS", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "W", "");
            }
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            printprocess(false);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            printprocess(true);
        }
        private void chkMthlyComparative_Click(object sender, EventArgs e)
        {
            string xyear = dtDateFrom.Value.Year.ToString();
            dtDateFrom.Value = bissclass.ConvertStringToDateTime("01", "01", xyear);
            dtDateTo.Value = bissclass.ConvertStringToDateTime("31", "12 ", xyear);
        }

        private void chkPrivate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPrivate.Checked)
                panel_PVTFilter.Visible = true;
            else
                panel_PVTFilter.Visible = false;

        }
    }
}