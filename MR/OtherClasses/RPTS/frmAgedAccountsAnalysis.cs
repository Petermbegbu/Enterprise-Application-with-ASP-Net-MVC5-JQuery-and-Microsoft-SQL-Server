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
    public partial class frmAgedAccountsAnalysis : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date, start_opening;
        DataSet ds = new DataSet();
        DataTable sdt, dtcurrency = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM currencycodes order by name", true);
        int mval90days, mval60days, mval30days;
        public frmAgedAccountsAnalysis()
        {
            InitializeComponent();
        }

        private void frmAgedAccountsAnalysis_Load(object sender, EventArgs e)
        {
            txtCurrent.Text = "Current Balance";
            txt30days.Text = "30 DAYS";
            txt60days.Text = "60 DAYS";
            txt90days.Text = "90 DAYS";

            nmrCurrent.Value = 29;
            nmr30days.Value = 30;
            nmr60days.Value = 60;
            nmr90days.Value = 90;

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
            txtName.Text = bchain.NAME;
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
            txtName.Text = dtcustomer.Rows[0]["name"].ToString();
        }
        void createSummary()
        {
            sdt = new DataTable(); //table to will be passed to report dataset 
            sdt.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
            sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
            sdt.Columns.Add(new DataColumn("VALCURRENT", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("VAL30DAYS", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("VAL60DAYS", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("VAL90DAYS", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("total_amt", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("credit", typeof(decimal)));
        }
        DataRow createnewRow(DataRow drow)
        {
            bool foundit = false;
            decimal db, cr, adj; db = cr = adj = 0m;
            decimal opbal = 0m;
            DataRow dr = null;

            foreach (DataRow row in sdt.Rows)
            {
                if (drow["grouphead"].ToString().Trim() == row["reference"].ToString().Trim())
                {
                    foundit = true;
                    dr = row;
                    break;
                }
            }
            if (!foundit)
            {
                string transtype = chkCorporate.Checked ? "C" : "P";
                dr = sdt.NewRow();
                dr["reference"] = drow["grouphead"].ToString();
                dr["name"] = msmrfunc.GETGroupheadname("", drow["grouphead"].ToString().Trim(), transtype);
                opbal = msmrfunc.getOpeningBalance("", drow["grouphead"].ToString().Trim(), "", transtype, start_opening, dtDateto.Value.Date,ref db,ref cr,ref adj );
                dr["VALCURRENT"] = 0m;
                dr["VAL30DAYS"] = 0m;
                dr["VAL60DAYS"] = 0m;
                dr["VAL90DAYS"] = 0m;
                dr["total_amt"] = 0m;
                dr["credit"] = 0m;
                sdt.Rows.Add(dr);
            }
            decimal xamt = Convert.ToDecimal(drow["amount"]);
            string xtype = drow["ttype"].ToString();
            if (xtype == "C")
            {
                dr["credit"] = Convert.ToDecimal(dr["credit"]) + xamt;
                dr["total_amt"] = Convert.ToDecimal(dr["total_amt"]) - xamt;
                return dr;
            }
            if (dtDateto.Value.Date.Subtract((DateTime)drow["trans_date"]).Days >= mval90days)
                dr["val90days"] = Convert.ToDecimal(dr["val90days"]) + xamt;
            else if (dtDateto.Value.Date.Subtract((DateTime)drow["trans_date"]).Days >= mval60days)
                dr["val60days"] = Convert.ToDecimal(dr["val60days"]) + xamt;
            else if (dtDateto.Value.Date.Subtract((DateTime)drow["trans_date"]).Days >= mval30days)
                dr["val30days"] = Convert.ToDecimal(dr["val30days"]) + xamt;
            else
                dr["valcurrent"] = Convert.ToDecimal(dr["valcurrent"]) + xamt;
            dr["total_amt"] = Convert.ToDecimal(dr["val90days"]) + Convert.ToDecimal(dr["val60days"]) + Convert.ToDecimal(dr["val30days"]) + Convert.ToDecimal(dr["valcurrent"]);

            return dr;
        }
/*           if (dtDateto.Value.Date.Subtract((DateTime)drow["trans_date"]).Days >= mval90days)
                dr["val90days"] = xtype == "D" ? Convert.ToDecimal(dr["val90days"]) + xamt : Convert.ToDecimal(dr["val90days"]) - xamt;
            else if (dtDateto.Value.Date.Subtract((DateTime)drow["trans_date"]).Days >= mval60days)
                dr["val60days"] = xtype == "D" ? Convert.ToDecimal(dr["val60days"]) + xamt : Convert.ToDecimal(dr["val60days"]) - xamt;
            else if (dtDateto.Value.Date.Subtract((DateTime)drow["trans_date"]).Days >= mval30days)
                dr["val30days"] = xtype == "D" ? Convert.ToDecimal(dr["val30days"]) + xamt : Convert.ToDecimal(dr["val30days"]) - xamt;
            else
                dr["valcurrent"] = xtype == "D" ? Convert.ToDecimal(dr["valcurrent"]) + xamt : Convert.ToDecimal(dr["valcurrent"]) - xamt;
            dr["total_amt"] = Convert.ToDecimal(dr["val90days"]) + Convert.ToDecimal(dr["val60days"]) + Convert.ToDecimal(dr["val30days"]) + Convert.ToDecimal(dr["valcurrent"]);*/
        void getData()
        {
            mval90days = Convert.ToInt32(nmr90days.Value);  mval60days = Convert.ToInt32(nmr60days.Value);
            mval30days = Convert.ToInt32(nmr30days.Value);

            string bstr = "", pstr = "", astr = "";
            bstr = " WHERE BILLING.TRANS_DATE >= '" + start_opening.ToShortDateString() + "' and BILLING.TRANS_DATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            pstr = " WHERE PAYDETAIL.TRANS_DATE >= '" + start_opening.ToShortDateString() + "' and PAYDETAIL.TRANS_DATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            astr = "WHERE BILL_ADJ.TRANS_DATE >= '" + start_opening.ToShortDateString() + "' and BILL_ADJ.TRANS_DATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            if (chkCorporate.Checked)
            {
                bstr += " AND BILLING.TRANSTYPE = 'C'";
                pstr += " AND PAYDETAIL.TRANSTYPE = 'C'";
                astr += " AND BILL_ADJ.TRANSTYPE = 'C'";
            }
            if (chkPVTFamily.Checked)
            {
                bstr += " AND BILLING.TRANSTYPE = 'P'";
                pstr += " AND PAYDETAIL.TRANSTYPE = 'P'";
                astr += " AND BILL_ADJ.TRANSTYPE = 'P'";
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

            start_opening = bissclass.ConvertStringToDateTime("01", msmrfunc.mrGlobals.mlastperiod == 12 ? "01" : msmrfunc.mrGlobals.mlastperiod+1.ToString(),  msmrfunc.mrGlobals.mlastperiod == 12 ? msmrfunc.mrGlobals.mpyear + 1.ToString() : msmrfunc.mrGlobals.mpyear.ToString());

           /* string xstring = "SELECT BILLING.REFERENCE, BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD, BILLING.TTYPE, BILLING.GHGROUPCODE FROM BILLING " + bstr + " UNION SELECT PAYDETAIL.REFERENCE, PAYDETAIL.AMOUNT, PAYDETAIL.TRANS_DATE, PAYDETAIL.TRANSTYPE, PAYDETAIL.GROUPHEAD, PAYDETAIL.TTYPE, PAYDETAIL.GHGROUPCODE FROM PAYDETAIL " + pstr + " UNION SELECT BILL_ADJ.REFERENCE, BILL_ADJ.AMOUNT, BILL_ADJ.TRANS_DATE, BILL_ADJ.TRANSTYPE, BILL_ADJ.GROUPHEAD,  BILL_ADJ.TTYPE,  BILL_ADJ.GHGROUPCODE FROM BILL_ADJ " + astr;*/
            string xstring = "SELECT SUM(BILLING.AMOUNT) AS AMOUNT, BILLING.TRANS_DATE, BILLING.GROUPHEAD, BILLING.TTYPE FROM BILLING " + bstr + " GROUP BY GROUPHEAD, TRANS_DATE, TTYPE UNION SELECT SUM(PAYDETAIL.AMOUNT) AS AMOUNT, PAYDETAIL.TRANS_DATE, PAYDETAIL.GROUPHEAD, PAYDETAIL.TTYPE FROM PAYDETAIL " + pstr + " GROUP BY GROUPHEAD, TRANS_DATE, TTYPE UNION SELECT SUM(BILL_ADJ.AMOUNT) AS AMOUNT, BILL_ADJ.TRANS_DATE, BILL_ADJ.GROUPHEAD,  BILL_ADJ.TTYPE FROM BILL_ADJ " + astr+" GROUP BY GROUPHEAD, TRANS_DATE, TTYPE";

            DataTable tsdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            foreach (DataRow row in tsdt.Rows)
            {
                createnewRow(row);
            }
            //we must scan to remove credit balance
            DataTable fsdt = new DataTable();
            fsdt = sdt.Clone();

            decimal xdebit,xcredit; xdebit = xcredit = 0m;
            foreach (DataRow row in sdt.Rows )
	        {
                if (string.IsNullOrWhiteSpace(row["name"].ToString()) || string.IsNullOrWhiteSpace(row["reference"].ToString()))
                    continue;
                xcredit = (decimal)row["credit"];
		        if ((decimal)row["val90days"] > 0 && xcredit > 0)
                {
                    xdebit = (decimal)row["val90days"];
                    SortValue(ref xdebit, ref xcredit );
                    row["val90days"] = (decimal)row["val90days"] - xdebit;
                }
                if ((decimal)row["val60days"] > 0 && xcredit > 0)
                {
                    xdebit = (decimal)row["val60days"];
                    SortValue(ref xdebit, ref xcredit );
                    row["val60days"] = (decimal)row["val60days"] - xdebit;
                }
                if ((decimal)row["val30days"] > 0 && xcredit > 0)
                {
                    xdebit = (decimal)row["val30days"];
                    SortValue(ref xdebit, ref xcredit );
                    row["val30days"] = (decimal)row["val30days"] - xdebit;
                }
                if ((decimal)row["valcurrent"] > 0 && xcredit > 0)
                {
                    xdebit = (decimal)row["valcurrent"];
                    SortValue(ref xdebit, ref xcredit );
                    row["valcurrent"] = (decimal)row["valcurrent"] - xdebit;
                }
                row["total_amt"] = (decimal)row["val90days"] + (decimal)row["val60days"] + (decimal)row["val30days"] +
                    (decimal)row["valcurrent"];
                if ((decimal)row["total_amt"] >= 1)
                    fsdt.ImportRow(row);
	        }


 /*           foreach (DataRow row in sdt.Rows)
            {
                if ((decimal)row["total_amt"] >= 1 && !string.IsNullOrWhiteSpace(row["name"].ToString()))
                {
                    if ((decimal)row["valcurrent"] < 1 && (decimal)row["valcurrent"] != 0)
                    {
                        row[]
                        row["valcurrent"] = 0m;
                    }
                    if ((decimal)row["val30days"] < 1)
                        row["val30days"] = 0m;
                    if ((decimal)row["val60days"] < 1)
                        row["val60days"] = 0m;

                    if (Convert.ToDecimal(row["val90days"]) >= 1)
                        row["total_amt"] = Convert.ToDecimal(row["val90days"]);
                    if (Convert.ToDecimal(row["val60days"]) >= 1)
                        row["total_amt"] = Convert.ToDecimal(row["total_amt"]) + Convert.ToDecimal(row["val60days"]);
                    if (Convert.ToDecimal(row["val30days"]) >= 1)
                        row["total_amt"] = Convert.ToDecimal(row["total_amt"]) + Convert.ToDecimal(row["val30days"]);

                    if ((decimal)row["total_amt"] >= 1)
                        fsdt.ImportRow(row);
                }
            }*/
            ds.Tables.Add(fsdt);
        }
        void SortValue(ref decimal xcum, ref decimal xpay)
        {
            if (xcum > xpay)
            {
                xcum = xcum - xpay;
                xpay = 0;
            }
            else
            {
                xpay = xpay - xcum;
                xcum = 0;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            if (dtDateto.Value > DateTime.Now.Date || dtDateto.Value < msmrfunc.mrGlobals.mta_start )
            {
                DialogResult result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            Session["sql"] = "";
            Session["rdlcfile"] = "CustomersAgedAccount.rdlc";

            createSummary();
           // sdt = new DataTable();
            ds = new DataSet();
            getData();
            //Session["current"] = txtCurrent.Text.Trim();
            //Session["lbl30days"] = txt30days.Text.Trim();
            //Session["lbl60days"] = txt60days.Text.Trim();
            //Session["lbl90days"] = txt90days.Text.Trim();
            string mrptheader = "CUSTOMERS AGED ACCOUNTS ANALYSIS AS AT " + dtDateto.Value.ToShortDateString();
            string rptfooter, rptcriteria; rptfooter = sysmodule; rptcriteria = "";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer("AGED ACCOUNTS ANALYSIS", mrptheader, rptfooter, rptcriteria, "", "AGEDACCTS", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "W", "");

                //if (isprint)
                //    paedreports.work();
                //else
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt("AGED ACCOUNTS ANALYSIS", mrptheader, rptfooter, rptcriteria, "", "AGEDACCTS", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "W", "");
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
        
    }
}