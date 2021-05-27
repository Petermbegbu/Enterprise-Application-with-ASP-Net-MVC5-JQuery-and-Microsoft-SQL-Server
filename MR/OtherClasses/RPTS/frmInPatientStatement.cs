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
    public partial class frmInPatientStatement : Form
    {
        billchaindtl bchain = new billchaindtl();
        Admrecs admrecs = new Admrecs();
        string lookupsource, AnyCode, mrptheader, rptcriteria, rptfooter = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtdocs = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE,NAME FROM DOCTORS WHERE RECTYPE = 'D' ORDER BY NAME", true), admDaily, dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES", false),
            dttariff = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE,NAME FROM tariff", false);
        string[] datea_ = new string[7];
        public frmInPatientStatement(string xreference)
        {
            InitializeComponent();
            txtReference.Text = xreference;
        }
        private void frmInPatientStatement_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtReference.Text))
                txtReference.Focus();
        }
        private void chkSummary_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSummary.Checked)
                panel_SummaryOptions.Enabled = true;
            else
                panel_SummaryOptions.Enabled = false;
                
        }
        void createAdmdaily()
        {
            admDaily = new DataTable(); //table to will be passed to report dataset 
            admDaily.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
            admDaily.Columns.Add(new DataColumn("MASTPROCESS", typeof(string)));
            admDaily.Columns.Add(new DataColumn("MASTPROCESSDESC", typeof(string)));
            admDaily.Columns.Add(new DataColumn("DESCRIPTION", typeof(string)));   
            admDaily.Columns.Add(new DataColumn("BALBF", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("DAY1", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("QTY1", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("AMT1", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("DAY2", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("QTY2", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("AMT2", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("DAY3", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("QTY3", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("AMT3", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("DAY4", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("QTY4", typeof(decimal)));
            admDaily.Columns.Add(new DataColumn("AMT4", typeof(decimal))); 
            admDaily.Columns.Add(new DataColumn("DAY5", typeof(decimal)));  
            admDaily.Columns.Add(new DataColumn("QTY5", typeof(decimal)));  
            admDaily.Columns.Add(new DataColumn("AMT5", typeof(decimal))); 
            admDaily.Columns.Add(new DataColumn("DAY6", typeof(decimal))); 
            admDaily.Columns.Add(new DataColumn("QTY6", typeof(decimal))); 
            admDaily.Columns.Add(new DataColumn("AMT6", typeof(decimal))); 
            admDaily.Columns.Add(new DataColumn("DAY7", typeof(decimal))); 
            admDaily.Columns.Add(new DataColumn("QTY7", typeof(decimal))); 
            admDaily.Columns.Add(new DataColumn("AMT7", typeof(decimal))); 
            admDaily.Columns.Add(new DataColumn("TOTAL", typeof(decimal))); 
        }
        DataRow createnewRow(DataRow drow)
        {
            bool foundit = false;
            DataRow dr = null;
            if (chkDetail.Checked)
            {
                foreach (DataRow row in admDaily.Rows )
                {
                    if (row["DESCRIPTION"].ToString().Trim() == drow["description"].ToString().Trim() && row["mastprocess"].ToString() == drow["mastprocess"].ToString())
                    {
                        dr = row;
                        foundit = true;
                        break;
                    }
                }
            }
            if (!foundit)
                dr = admDaily.NewRow();

            if (chkDetail.Checked)
            {
                if (!foundit)
                    dr["REFERENCE"] = drow["reference"].ToString();
            }
            else
                dr["REFERENCE"] = "";
            dr["MASTPROCESS"] = drow["mastprocess"].ToString();
            if (!string.IsNullOrWhiteSpace(drow["mastprocess"].ToString()) && !foundit)
                dr["MASTPROCESSDESC"] = bissclass.combodisplayitemCodeName("reference", drow["mastprocess"].ToString(), dttariff, "name");
            /*else if (!chkDetail.Checked)
                dr["MASTPROCESSDESC"] = "";*/
            if (chkDetail.Checked || chkincludegroupheadings.Checked)
                dr["DESCRIPTION"] = drow["description"].ToString();
            /*else
                dr["DESCRIPTION"] = "";*/
            if (!foundit)
            {
                dr["BALBF"] = 0;
                dr["QTY1"] = 0;
                dr["AMT1"] = 0;
                dr["QTY2"] = 0;
                dr["AMT2"] = 0;
                dr["QTY3"] = 0;
                dr["AMT3"] = 0;
                dr["QTY4"] = 0;
                dr["AMT4"] = 0;
                dr["QTY5"] = 0;
                dr["AMT5"] = 0;
                dr["QTY6"] = 0;
                dr["AMT6"] = 0;
                dr["QTY7"] = 0;
                dr["AMT7"] = 0;
                dr["TOTAL"] = 0;

                admDaily.Rows.Add(dr);
            }
            return dr;
        }
        private void txtReference_GotFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AnyCode))
            {
                lblname.Text = txtgroupcode.Text = txtpatientno.Text = txtgrouphead.Text = "";
                dtDateFrom.Value = admrecs.ADM_DATE = DateTime.Now.Date;
                txtgroupcode.Enabled = txtpatientno.Enabled = txtgrouphead.Enabled = dtDateFrom.Enabled = panel_SummaryOptions.Enabled = cboFacility.Enabled = true;
            }
        }
        private void txtReference_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReference.Text))
                return;
            if (string.IsNullOrWhiteSpace(AnyCode) && txtReference.Text.Substring(0, 1) != "A")
            {
                txtReference.Text = bissclass.autonumconfig(txtReference.Text, true, "A", "999999999");
            }
            //check if reference exist
            AnyCode = "";
            admrecs = Admrecs.GetADMRECS(txtReference.Text);
            if (admrecs == null)
            {
                MessageBox.Show("Invalid Admission Reference...", "ADMISSION DETAILS");
                return;
            }
            lblname.Text = admrecs.NAME;
            txtgroupcode.Text = admrecs.GROUPCODE;
            txtpatientno.Text = admrecs.PATIENTNO;
            txtgrouphead.Text = admrecs.GROUPHEAD;
            dtDateFrom.Value = admrecs.ADM_DATE;
            dtDateto.Value = string.IsNullOrWhiteSpace( admrecs.DISCHARGE) ? DateTime.Now.Date : Convert.ToDateTime(admrecs.DISCHARGE);
            txtgroupcode.Enabled = txtpatientno.Enabled = txtgrouphead.Enabled = panel_SummaryOptions.Enabled = cboFacility.Enabled = false;
            if (dtDateFrom.Value.Subtract(dtDateFrom.Value).Days > 7)
                dtDateFrom.Enabled = dtDateto.Enabled = true;
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
            else if (btn.Name == "btnReference") //aDMISSIONS
            {
                string xstring = chkCurrtAdmRev.Checked ? "[CURRENT ADMISSIONS]" : "[ALL]";
                txtReference.Text = "";
                lookupsource = "A";
                msmrfunc.mrGlobals.crequired = "A";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED ADMISSIONS " + xstring;
                msmrfunc.mrGlobals.lookupCriteria = chkCurrtAdmRev.Checked ? "C" : "";
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
            else if (lookupsource == "C") //patientno
            {
                txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtgrouphead.Select();
            }
            else if (lookupsource == "A") //AT REG
            {
                txtReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtReference.Focus();
            }

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
            lblname.Text = bchain.NAME;
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
            lblname.Text = dtcustomer.Rows[0]["name"].ToString();
        }
        void getData()
        {
            bool lhistory = false;
            if (dtDateFrom.Value.Year < msmrfunc.mrGlobals.mpyear)
                lhistory = true;

            string rptfile = lhistory ? "attendhist" : "mrattend";
            string selstring = "";
            if (string.IsNullOrWhiteSpace(txtReference.Text) && !chkSummaryforAll.Checked || chkSummaryforAll.Checked && dtDateFrom.Value.Date != dtDateto.Value.Date )
                selstring = " WHERE adm_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and adm_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            if (!string.IsNullOrWhiteSpace(txtReference.Text))
                selstring += " where ADMRECS.reference = '" + txtReference.Text + "'";
            else
            {

                if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
                {
                    selstring += selstring == "" ? " where " : " AND ";
                    selstring += " ADMRECS.GROUPHEAD = '" + txtgrouphead.Text + "'";
                    rptcriteria += " For : " + lblname.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                {
                    selstring += selstring == "" ? " where " : " AND ";
                    selstring += " ADMRECS.GROUPCODE = '" + txtgroupcode.Text + "'";
                    rptcriteria += " Group : " + lblname.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
                {
                    selstring += selstring == "" ? " where " : " AND ";
                    selstring += " ADMRECS.PATIENTNO = '" + txtpatientno.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(cboFacility.Text))
                {
                    selstring += selstring == "" ? " where " : " AND ";
                    selstring += " trim(ADMRECS.facility) = '" + cboFacility.SelectedValue.ToString().Trim() + "'";
                    rptcriteria += " Facility : " + cboFacility.Text.Trim();
                }
            }
            if (chkSummaryforAll.Checked)
            {
                GetSummary(selstring);
                return;
            }
            //billchain.residence AS ADDRESS, billchain.staffno from admrecs LEFT JOIN BILLCHAIN ON ADMRECS.GROUPCODE = BILLCHAIN.GROUPCODE AND ADMRECS.PATIENTNO = ADMRECS.PATIENTNO "
            string xstr = "select ADMRECS.REFERENCE, ADMRECS.PATIENTNO, ADMRECS.NAME, ADMRECS.FACILITY, ADMRECS.UNIT, ADMRECS.ROOM, ADMRECS.BED, ADMRECS.RATE, ADMRECS.ADM_DATE, ADMRECS.TIME, ADMRECS.DOCTOR, ADMRECS.DIAGNOSIS, ADMRECS.DISCHARGE, ADMRECS.DISCH_TIME, ADMRECS.DISCH_DOCT, ADMRECS.BILLED, ADMRECS.DATE_BILLE, ADMRECS.REMARKS, ADMRECS.REASON, ADMRECS.GROUPHEAD, ADMRECS.GROUPHTYPE, ADMRECS.GROUPCODE, ADMRECS.ACAMT, ADMRECS.GHGROUPCODE, ADMRECS.DAILYFEEDING, ADMRECS.DAILYPNC, ADMRECS.PAYMENTS, ADMRECS.DIAGNOSIS_ALL, CHAR(50) AS STAYPERIOD, billchain.residence AS ADDRESS, billchain.staffno, billchain.hmocode from admrecs LEFT JOIN BILLCHAIN ON ADMRECS.GROUPCODE = BILLCHAIN.GROUPCODE AND ADMRECS.PATIENTNO = BILLCHAIN.PATIENTNO " + selstring + " ORDER BY REFERENCE";
            string dtl = " TRANS_DATE, REFERENCE, MASTPROCESS, DESCRIPTION, UNIT, char(50) AS MASTERPROCESSDESC ";
            sdt = Dataaccess.GetAnytable("", "MR", xstr, false);
            DataTable dtt;
            string tmpstr;
            DateTime md;
            DataRow dtlrow;
            int xdateval; //,xcount = 1;
            for (int i = 0; i < 7; i++)
			{
			    datea_[i] = "";
			}
            foreach (DataRow row in sdt.Rows)
            {
                md = dtDateFrom.Value.Date;
                xdateval = Convert.ToInt32( dtDateto.Value.Date.Subtract( dtDateFrom.Value.Date).TotalDays)+1;
             //   xcount = 1;
                if (chkDetail.Checked)
                {
                    for (int i = 0; i < xdateval; i++)
                    {
                        if (i < 7 && md <= dtDateto.Value.Date)
                        {
                            datea_[i] = md.ToShortDateString();
                          //  xcount++;
                        }
                        md = md.AddDays(1);
                    }
                }
                row["STAYPERIOD"] = string.IsNullOrWhiteSpace(row["discharge"].ToString()) ? DateTime.Now.Date.Subtract((DateTime)row["adm_date"]).TotalDays.ToString() : Convert.ToDateTime(row["discharge"]).Subtract((DateTime)row["adm_date"]).TotalDays.ToString();
                if (row["grouphtype"].ToString() == "C")
                {
                    dtt = Dataaccess.GetAnytable("", "MR", "select name, address1, custno from customer where custno = '" + row["grouphead"].ToString()+"'", false);
                    if (dtt.Rows.Count > 0)
                        row["address"] = dtt.Rows[0]["name"].ToString().Trim() + "; " + dtt.Rows[0]["address1"].ToString().Trim() + " [" + dtt.Rows[0]["custno"].ToString().Trim() + "]";
                }
                if (chkSummary.Checked && chkincludegroupheadings.Checked)
                    tmpstr = "SELECT MASTPROCESS, DESCRIPTION, Sum(AMOUNT) AS AMOUNT, SUM(QTY) AS QTY, char(50) AS MASTERPROCESSDESC from admdetai where reference = '" + row["reference"].ToString() + "' GROUP BY MASTPROCESS, DESCRIPTION";
                else if (chkSummary.Checked && !chkincludegroupheadings.Checked)
                    tmpstr = "select MASTPROCESS, Sum(AMOUNT) AS AMOUNT, SUM(QTY) AS QTY, char(50) AS MASTERPROCESSDESC from admdetai where reference = '" + row["reference"].ToString() + "' GROUP BY MASTPROCESS";
                else
                    tmpstr = "select amount, qty, "+dtl+" from admdetai where reference = '" + row["reference"].ToString()+"' and description != ''";
                dtt = Dataaccess.GetAnytable("", "MR", tmpstr, false);
                foreach (DataRow drow in dtt.Rows )
                {
                    dtlrow = createnewRow(drow);
                    if (chkDetail.Checked)
                    {
                        if (Convert.ToDateTime(drow["trans_date"]) < dtDateFrom.Value.Date)
                        {
                            dtlrow["balbf"] = Convert.ToDecimal(dtlrow["balbf"]) + (decimal)drow["amount"];
                            dtlrow["total"] = Convert.ToDecimal(dtlrow["balbf"]);
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (datea_[i] == "")
                                    continue;
                                if (datea_[i] == Convert.ToDateTime(drow["trans_date"]).ToShortDateString())
                                {
                                    dtlrow["amt" + (i + 1).ToString()] = (decimal)drow["amount"] +
                                        (decimal)dtlrow["amt" + (i + 1).ToString()];
                                    dtlrow["qty" + (i + 1).ToString()] = (decimal)drow["qty"] + 
                                        (decimal)dtlrow["qty" + (i + 1).ToString()];
                                    dtlrow["total"] = (decimal)dtlrow["total"] + (decimal)drow["amount"];
                                    break;
                                }
                            }
                        }
                    }
                    else
                        dtlrow["total"] = (decimal)drow["amount"];
                 }
            }
        }
        void GetSummary(string xstr)
        {
            rptcriteria = "";
            xstr = "";
          //  if (dtDateFrom.Value.Date != dtDateto.Value.Date)
          //  {
                xstr = " where  adm_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and adm_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
          //  }
            if (chkDebitBal.Checked)
            {
               // xstr += xstr == "" ? " where " : " AND ";
                xstr += " and admrecs.acamt - admrecs.payments >= '1' ";
            }
            if (chkCreditBal.Checked)
            {
               // xstr += xstr == "" ? " where " : " AND ";
                xstr += " and admrecs.payments - admrecs.acamt >= '1' ";
            }
            if (chkDischarged.Checked)
            {
               // xstr += xstr == "" ? " where " : " AND ";
                xstr += " and admrecs.discharge != '' ";
            }
            if (chkUndischarged.Checked)
            {
               // xstr += xstr == "" ? " where " : " AND ";
                xstr += " and admrecs.discharge = '' ";
            }
            if (chkPrivatePatients.Checked)
            {
              //  xstr += xstr == "" ? " where " : " AND ";
                xstr += " and admrecs.grouphtype = 'P' ";
            }
            else if (chkCorporate.Checked)
            {
              //  xstr += xstr == "" ? " where " : " AND ";
                xstr += " and admrecs.grouphtype = 'C' ";
            }
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
            {
                xstr += " and ADMRECS.GROUPHEAD = '" + txtgrouphead.Text + "'";
                rptcriteria += " For : " + lblname.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
            {
                xstr += " and ADMRECS.GROUPCODE = '" + txtgroupcode.Text + "'";
                rptcriteria += " Group : " + txtgroupcode.Text;
            }
            if (!string.IsNullOrWhiteSpace(cboFacility.Text))
            {
                xstr += " and ADMRECS.facility = '" + cboFacility.SelectedValue.ToString().Trim() + "'";
                rptcriteria += " Facility : " + cboFacility.Text.Trim();
            }
            string selstring = "SELECT Admrecs.reference, Admrecs.patientno, Admrecs.name, Admrecs.facility, Admrecs.unit, Admrecs.room, Admrecs.bed, Admrecs.rate, Admrecs.adm_date, Admrecs.time, Admrecs.discharge, Admrecs.disch_time, Admrecs.date_bille, Admrecs.grouphtype, Admrecs.groupcode, Admrecs.acamt,Admrecs.GHGROUPCODE, Admrecs.payments, billchain.residence, billchain.staffno from admrecs LEFT JOIN BILLCHAIN ON ADMRECS.GROUPCODE = BILLCHAIN.GROUPCODE AND ADMRECS.PATIENTNO = BILLCHAIN.PATIENTNO "+xstr +" order by name";

            sdt = Dataaccess.GetAnytable("", "MR", selstring, false);
            //DataTable dtt;

            //foreach (DataRow row in sdt.Rows )
            //{
            //    dtt = Dataaccess.GetAnytable("","MR","select Sum(amount) AS amount from admdetai where reference = '"+row["reference"].ToString()+"' GROUP BY REFERENCE",false);
            //    if (dtt.Rows.Count > 0)
            //        row["acamt"] = (decimal)dtt.Rows[0]["amount"];
            //}
         //   rptcriteria = ""; //Report Criteria : ";
            if (chkPrivatePatients.Checked)
                rptcriteria += " - PRIVATE PATIENTS ; ";
            if (chkCorporate.Checked)
                rptcriteria += " - CORPORATE PATIENTS ; ";
            if (chkDebitBal.Checked)
                rptcriteria += " - DEBIT BALANCES ; ";
            if (chkCreditBal.Checked)
                rptcriteria += " - CREDIT BALANCES ; ";
            if (chkUndischarged.Checked)
                rptcriteria += " - UN-DISCHARGED PATIENTS ; ";
            if (chkDischarged.Checked)
                rptcriteria += " - DISCHARGED PATIENTS ; ";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (string.IsNullOrWhiteSpace(txtReference.Text))
            {
                if (dtDateFrom.Value.Date > DateTime.Now.Date || dtDateFrom.Value.Date < msmrfunc.mrGlobals.mta_start || dtDateto.Value.Date < dtDateFrom.Value.Date || dtDateFrom.Value.Year != dtDateto.Value.Year)
                {
                    result = MessageBox.Show("Invalid Date Specification");
                    return;
                }
            }
            if (chkDetail.Checked && dtDateto.Value.Subtract(dtDateFrom.Value).Days > 7)
            {
                result = MessageBox.Show("Daily Admissions Ledger cannot handle more than 7 days at a time...");
                dtDateto.Value = dtDateFrom.Value.AddDays(6);
            }
            sdt = new DataTable();
            ds = new DataSet();
            createAdmdaily();

            getData();
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data for Specified Conditions...");
                return;
            }
            if (chkSummaryforAll.Checked)
                ds.Tables.Add(sdt);
            else
            {
                ds.Tables.Add(admDaily);
                ds.Tables.Add(sdt);
            }
            Session["sql"] = "";
            Session["datea_"] = (string[])datea_;
            string xrptsize = "";
            if (chkSummaryforAll.Checked)
            {
                Session["rdlcfile"] = "AdmSummaryRpt.rdlc";
                xrptsize = "W";
            }
            else if (chkSummary.Checked && !chkincludegroupheadings.Checked)
                Session["rdlcfile"] = "AdmSummary.rdlc";
            else if (chkSummary.Checked && chkincludegroupheadings.Checked)
                Session["rdlcfile"] = "AdmSummaryWGroup.rdlc";
            else
            {
                Session["rdlcfile"] = "AdmDailyRpt.rdlc";
                xrptsize = "W";
            }

            mrptheader = chkSummary.Checked ? "IN-PATIENT SUMMARY STATEMENT" : "IN-PATIENT DAILY SERVICE LEDGER";
             string sumhead = chkSummaryforAll.Checked ? rptcriteria : "";
             if (!isprint)
             {
                 frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader + sumhead, rptfooter, rptcriteria, "", "ADMISSIONS", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, xrptsize, "");

                 if (isprint)
                     paedreports.work();
                 else
                     paedreports.Show();
             }
             else
             {
                 MRrptConversion.GeneralRpt(mrptheader, mrptheader + sumhead, rptfooter, rptcriteria, "", "ADMISSIONS", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, xrptsize, "");
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