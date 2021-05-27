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
    public partial class frmAdmissionRecords : Form
    {
        billchaindtl bchain = new billchaindtl();
        Admrecs admrecs = new Admrecs();
        string lookupsource, AnyCode, mrptheader, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtbranch = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM branchcodeS order by name", true);
        int mprog;
        public frmAdmissionRecords(int xprog)
        {
            InitializeComponent();
            mprog = xprog;
            if (mprog == 2)
            {
                chkCurrtAdmRev.Text = "ALL Discharge Records";
                this.Text = "DISCHARGE RECORDS";
            }
        }

        private void frmAdmissionRecords_Load(object sender, EventArgs e)
        {
            initcomboboxes();

        }
        void initcomboboxes()
        {
            cboFacility.DataSource = dtfacility;
            cboFacility.ValueMember = "Type_code";
            cboFacility.DisplayMember = "NAME";

            cboBranch.DataSource = dtbranch;
            cboBranch.ValueMember = "Type_code";
            cboBranch.DisplayMember = "NAME";
        }
        private void txtReference_GotFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AnyCode))
            {
                lblname.Text = txtgroupcode.Text = txtpatientno.Text = txtgrouphead.Text = "";
                dtDateFrom.Value = admrecs.ADM_DATE = DateTime.Now.Date;
                txtgroupcode.Enabled = txtpatientno.Enabled = txtgrouphead.Enabled = dtDateFrom.Enabled = panel_Sortoptions.Enabled = cboFacility.Enabled = true;
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
            dtDateto.Value = string.IsNullOrWhiteSpace(admrecs.DISCHARGE) ? DateTime.Now.Date : Convert.ToDateTime(admrecs.DISCHARGE);
            txtgroupcode.Enabled = txtpatientno.Enabled = txtgrouphead.Enabled = dtDateFrom.Enabled = panel_Sortoptions.Enabled = cboFacility.Enabled = false;

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
        bool getData()
        {
            bool lhistory = false;
            if (dtDateFrom.Value.Year < msmrfunc.mrGlobals.mpyear)
                lhistory = true;

            string rptfile = lhistory ? "Admhist" : "Admrecs";
            string selstring = " WHERE ", xfile = rptfile+".";
            if (mprog == 1)
            {
                if (chkCurrtAdmRev.Checked)
                    selstring += xfile+"discharge = '' ";
                else if (chkChronologicalAdmDate.Checked || chkAlphaPatientName.Checked)
                    selstring += xfile + "adm_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and " + xfile + "adm_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                else
                    selstring += xfile + "discharge <> '' and CONVERT(DATE, " + xfile + "discharge) >= '" + dtDateFrom.Value.ToShortDateString() + "' and CONVERT(DATE, " + xfile + "discharge) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            }
            else
            {
                if (chkCurrtAdmRev.Checked)
                    selstring += xfile + "discharge <> ''";
                else
                    selstring += xfile + "discharge <> '' and CONVERT(DATE, " + xfile + "discharge) >= '" + dtDateFrom.Value.ToShortDateString() + "' and CONVERT(DATE, " + xfile + "discharge) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            }
            if (!string.IsNullOrWhiteSpace(txtReference.Text))
                selstring += " AND " + xfile + "reference = '" + txtReference.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
                selstring += " AND " + xfile + "GROUPHEAD = '" + txtgrouphead.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                selstring += " AND " + xfile + "GROUPCODE = '" + txtgroupcode.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
                selstring += " AND " + xfile + "PATIENTNO = '" + txtpatientno.Text + "'";
            if (!string.IsNullOrWhiteSpace(cboFacility.Text))
                selstring += " AND " + xfile + "facility = '" + cboFacility.SelectedValue.ToString() + "'";
            if (!string.IsNullOrWhiteSpace(cboBranch.Text))
                selstring += " AND " + xfile + "unit = '" + cboBranch.SelectedValue.ToString() + "'";

            string xstr = "select ADMRECS.REFERENCE, ADMRECS.PATIENTNO, ADMRECS.NAME, ADMRECS.FACILITY, ADMRECS.UNIT, ADMRECS.ROOM, ADMRECS.BED, ADMRECS.RATE, ADMRECS.ADM_DATE, ADMRECS.TIME, ADMRECS.DOCTOR, ADMRECS.DIAGNOSIS, ADMRECS.DISCHARGE, ADMRECS.DISCH_TIME, ADMRECS.DISCH_DOCT, ADMRECS.BILLED, ADMRECS.DATE_BILLE, ADMRECS.REMARKS, ADMRECS.REASON, ADMRECS.GROUPHEAD, ADMRECS.GROUPHTYPE, ADMRECS.GROUPCODE, ADMRECS.ACAMT, ADMRECS.GHGROUPCODE, ADMRECS.DAILYFEEDING, ADMRECS.DAILYPNC, ADMRECS.PAYMENTS, ADMRECS.DIAGNOSIS_ALL, CHAR(50) AS GHNAME, CHAR(50) AS ADDRESS, billchain.birthdate from admrecs left join billchain on admrecs.groupcode = billchain.groupcode and admrecs.patientno = billchain.patientno" + selstring;
            if (chkAlphaPatientName.Checked)
                xstr +=  " order by name";
            else if (chkChronologicalAdmDate.Checked)
                xstr += " order by Adm_date";
            else if (chkChronologicalAdmRef.Checked)
                xstr += " order by Reference";
            else
                xstr += " order by Discharge";
            sdt = Dataaccess.GetAnytable("", "MR", xstr, false);
            if (sdt.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("No Data for Specified Conditions...");
                return false;
            }
            string grouphead = "",ghname = "";
            foreach (DataRow row in sdt.Rows)
            {
                if (row["birthdate"] == DBNull.Value)
                    row["address"] = "";
                else
                    row["address"] = bissclass.agecalc(Convert.ToDateTime(row["birthdate"]), Convert.ToDateTime(row["adm_date"]));
                if (row["grouphtype"].ToString() == "P" && row["grouphead"].ToString() == row["patientno"].ToString())
                {
                    grouphead = "< S E L F>";
                    ghname = grouphead;
                }
                else if (row["grouphead"].ToString().Trim() != grouphead.Trim())
                {
                    ghname = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), row["grouphtype"].ToString());
                    grouphead = row["grouphead"].ToString();
                }
                row["ghname"] = ghname;
            }
            ds.Tables.Add(sdt);
            return true;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (dtDateFrom.Value.Date > DateTime.Now.Date || dtDateFrom.Value.Date < msmrfunc.mrGlobals.mta_start || dtDateto.Value.Date < dtDateFrom.Value.Date || dtDateFrom.Value.Year != dtDateto.Value.Year)
            {
                result = MessageBox.Show("Invalid Date Specification");
                return;
            }
            ds = new DataSet();
            sdt = new DataTable();
            getData();
           
            Session["sql"] = "";
           // if (chkAlphaPatientName.Checked)
            Session["rdlcfile"] = "AdmRecs_Alpha.rdlc";
            //else if (chkChronologicalAdmDate.Checked)
            //    Session["rdlcfile"] = "AdmRecs_AdmDate.rdlc";
            //else if (chkChronologicalAdmRef.Checked)
            //    Session["rdlcfile"] = "AdmRecs_AdmRef.rdlc";
            //else
            //    Session["rdlcfile"] = "AdmRecs_DischargeDate.rdlc";
          //  mrptheader = mprog == 1 ? "ADMISSION RECORDS" : "DISCHARGE RECORDS";
            if (chkCurrtAdmRev.Checked)
                mrptheader = "CURRENT ADMISSION RECORDS ";
            else
            {
                mrptheader = mprog == 1 ? "ADMISSION RECORDS " : "DISCHARGE RECORDS ";
                mrptheader += chkChronologicalAdmDate.Checked ? "CHRONOLOGICAL BY ADMISSION DATE" : chkChronologicalDischargeDate.Checked ? "CHRONOLOGICAL BY DISCHARGE DATE" : chkChronologicalAdmRef.Checked ? "CHRONOLOGICAL BY ADMISSION REFERENCE" : "ALPHABETICALLY BY PATIENT'S NAME";
            }
            mrptheader += " FOR "+dtDateFrom.Value.ToShortDateString()+" TO : "+dtDateto.Value.ToShortDateString();
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, "", "", "", "ADMRECS", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "W", "");

                //if (isprint)
                //    paedreports.work();
                //else
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(this.Text, mrptheader, "", "", "", "ADMRECS", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "W", "");
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