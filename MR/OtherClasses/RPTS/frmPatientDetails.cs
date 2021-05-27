#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

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
    public partial class frmPatientDetails : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, mrptheader, sysmodule = bissclass.getRptfooter(), mrpttype;
       // DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtcountry = Dataaccess.GetAnytable("", "CODES", "select type_code, name from countrycodes order by name", true);
        public frmPatientDetails()
        {
            InitializeComponent();
        }
        private void frmPatientDetails_Load(object sender, EventArgs e)
        {
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            cboFacility.DataSource = dtfacility;
            cboFacility.ValueMember = "Type_code";
            cboFacility.DisplayMember = "name";

            cboNationality.DataSource = dtcountry;
            cboNationality.ValueMember = "Type_code";
            cboNationality.DisplayMember = "name";
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
        }
        private void txtgroupcode_LostFocus(object sender, EventArgs e)
        {
            txtpatientno.Focus();
        }
        private void txtPatientno_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpatientno.Text))
                return;
            if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
            {
                txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
            }
            DialogResult result;
            //check if patientno exists
            bchain = billchaindtl.Getbillchain(txtpatientno.Text, txtgroupcode.Text);
            if (bchain == null)
            {
                result = MessageBox.Show("Invalid Patient Number... ");
                txtpatientno.Text = " ";
                txtgroupcode.Select();
                return;
            }
            lblname.Text = bchain.NAME;
            btnPreview.Focus();
            return;
        }
        void getData()
        {
            string rtnstring = " WHERE billchain.name != ''";
            if (chkReportByDate.Checked)
                rtnstring += " and billchain.reg_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and billchain.reg_date <= '" + dtDateto.Value.ToShortDateString() + "  23:59:59.999'";
            if (chkCorporate.Checked)
                rtnstring += " and billchain.grouphtype = 'C'";
            if (chkPVTFC.Checked)
                rtnstring += " and billchain.grouphtype = 'P'";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                rtnstring += " and billchain.groupcode = '" + txtgroupcode.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
                rtnstring += " and billchain.patientno = '" + txtpatientno.Text + "'";
            if (!string.IsNullOrWhiteSpace(cboFacility.Text))
                rtnstring += " and clinic = '" + cboFacility.SelectedValue.ToString() + "'";
            if (chkApplyFilter.Checked)
            {
                if (nmrAgeFrom.Value > 0 && nmrAgeTo.Value > nmrAgeFrom.Value)
                    rtnstring += " and year(getDate()) - year(billchain.BIRTHDATE) >= '" + nmrAgeFrom.Value + "' and year(getDate()) - year(billchain.BIRTHDATE) <= '" + nmrAgeTo.Value + "'";
                if (!string.IsNullOrWhiteSpace(cboGender.Text))
                    rtnstring += " and left(billchain.sex,1) = '" + cboGender.Text.Substring(0, 1) + "'";
                if (!string.IsNullOrWhiteSpace(cboMaritalStat.Text))
                    rtnstring += " and left(billchain.m_status,1) = '" + cboMaritalStat.Text.Substring(0, 1) + "'";
                if (!string.IsNullOrWhiteSpace(cboResidence.Text))
                    rtnstring += " and billchain.residence LIKE '%" + cboResidence.Text.Trim() + "%'";
            }
            if (chkGroupByNationality.Checked && !string.IsNullOrWhiteSpace(cboNationality.Text))
                rtnstring += " and patient.NATIONALITY = '" + cboNationality.SelectedValue.ToString() + "'";

            mrpttype = "PATIENTDETAILS";
            string xstring = "";
            if (chkGroupByNationality.Checked)
                if (chkNationalitySummary.Checked)
                    /*SELECT distributor_id, count(*) AS total, sum(case when level = 'exec' then 1 else 0 end) AS ExecCount,
                    sum(case when level = 'personal' then 1 else 0 end) AS PersonalCount FROM yourtable GROUP BY distributor_id
                    xstring = "SELECT patient.NATIONALITY, COUNT(*) as TOTPAT, SUM(case when billchain.SEX = 'MALE' then 1 else 0 end) AS totmale, SUM(case when billchain.SEX = 'FEMALE' then 1 else 0 end) AS totfemale from billchain LEFT OUTER JOIN PATIENT ON billchain.groupcode = patient.groupcode and billchain.patientno = patient.patientno group by nationality";*/
                    xstring = "SELECT patient.NATIONALITY, COUNT(*) as TOTPAT, SUM(case when patient.SEX = 'MALE' then 1 else 0 end) AS totmale, SUM(case when patient.SEX = 'FEMALE' then 1 else 0 end) AS totfemale from PATIENT LEFT JOIN billchain on patient.groupcode = billchain.groupcode and patient.patientno = billchain.patientno" + rtnstring + " group by nationality";
                else
                    xstring = "select billchain.GROUPCODE, billchain.PATIENTNO, billchain.GROUPHEAD, billchain.NAME, billchain.REG_DATE, billchain.GROUPHTYPE, billchain.STAFFNO, billchain.RELATIONSH, billchain.SEX, billchain.M_STATUS, billchain.BIRTHDATE, billchain.CUMVISITS, billchain.HMOCODE, billchain.PATCATEG, billchain.RESIDENCE, billchain.GHGROUPCODE, billchain.HMOSERVTYPE, billchain.BILLONACCT, billchain.CURRENCY, billchain.CLINIC, billchain.PHONE, billchain.EMAIL, billchain.SURNAME, billchain.OTHERNAMES, billchain.TITLE, billchain.MEDHISTORYCHAINED, billchain.PATIENTNO_PRINCIPAL, patient.NATIONALITY, char(100) as NATION from billchain LEFT JOIN PATIENT ON billchain.groupcode = patient.groupcode and billchain.patientno = patient.patientno" + rtnstring;
            else
                xstring = "select billchain.GROUPCODE, billchain.PATIENTNO, billchain.GROUPHEAD, billchain.NAME, billchain.REG_DATE, billchain.GROUPHTYPE, billchain.STAFFNO, billchain.RELATIONSH, billchain.SEX, billchain.M_STATUS, billchain.BIRTHDATE, billchain.CUMVISITS, billchain.HMOCODE, billchain.PATCATEG, billchain.RESIDENCE, billchain.GHGROUPCODE, billchain.HMOSERVTYPE, billchain.BILLONACCT, billchain.CURRENCY, billchain.CLINIC, billchain.PHONE, billchain.EMAIL, billchain.SURNAME, billchain.OTHERNAMES, billchain.TITLE, billchain.MEDHISTORYCHAINED, billchain.PATIENTNO_PRINCIPAL from billchain" + rtnstring;
            if (chkReportByDate.Checked)
                rtnstring += " ORDER BY REG_DATE";
            else if (chkSortPatientNoUngrouped.Checked)
                rtnstring += " ORDER BY PATIENTNO";
            else if (chkSortReferenceOnGrp.Checked)
                rtnstring += " ORDER BY GROUPCODE, PATIENTNO";
            else
                rtnstring += " ORDER BY NAME";

            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            if(chkGroupByNationality.Checked)
            {
                if (chkNationalitySummary.Checked)
                {
                    sdt.Columns.Add(new DataColumn("NATION", typeof(string)));
                   // sdt.Columns.Add(new DataColumn("name", typeof(string)));
                   // sdt.Columns.Add(new DataColumn("description", typeof(string)));
                }
                foreach (DataRow row in sdt.Rows )
                {
                    if (string.IsNullOrWhiteSpace(row["nationality"].ToString()))
                        row["nation"] = "< NOT SPECIFIED >";
                    else
                        row["nation"] = bissclass.combodisplayitemCodeName("type_code",row["nationality"].ToString(),dtcountry,"name");
                    
                }
            }
            //CHECKA AND GET REMOTE DATA
          /*  if (chkGetRemoteData.Checked)
            {
                frmGetRemoteData getrd = new frmGetRemoteData(sdt, xstring, "MR");
                getrd.ShowDialog();
                if (Session["remotedata"] != null)
                    sdt = (DataTable)Session["remotedata"];
            }*/
            ds.Tables.Add(sdt);
 
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            printprocess(false);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printprocess(true );
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (chkReportByDate.Checked && (dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateto.Value < dtDateFrom.Value || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateto.Value.Date > DateTime.Now.Date))
            {
                result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            string rptfooter, rptcriteria; rptfooter = rptcriteria = "";
            Session["sql"] = "";
            ds = new DataSet();
            sdt = new DataTable();
            getData();
            if (sdt.Rows.Count < 1)
            {
                MessageBox.Show("No Data for specified conditions...");
                return;
            }
            mrptheader = chkGroupByNationality.Checked ? "PATIENTS BY NATIONALITIES" : "PATIENT LISTING";
            if (chkGroupByNationality.Checked && chkNationalitySummary.Checked)
                mrptheader += " (SUMMARY)";
            else if (chkSortAlpha.Checked)
                mrptheader += " - ALPHABETICAL";
            else
                mrptheader += " - BY REGISTRATION NUMBER";
            if (chkRptDetail.Checked)
                mrptheader += " [ DETAILS ]";
            if (chkReportByDate.Checked)
                mrptheader += " For Registeration Dates : " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString();

            if (chkGroupByNationality.Checked)
                Session["rdlcfile"] = chkNationalitySummary.Checked ? "PatientDetailsByNationSummary.rdlc" : "PatientDetailsByNationality.rdlc";
            else if (chkBasicMedicProfile.Checked)
                Session["rdlcfile"] = "PatientDetailsMedicProfile.rdlc";
            else if (chkRptSummary.Checked)
                Session["rdlcfile"] = "PatientDetailsSummary.rdlc";
            else
                Session["rdlcfile"] = "PatientDetails.rdlc";
            

            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, rptfooter, rptcriteria, "", mrpttype, "", 0m, "", "", "", ds, true, 0, dtDateFrom.Value.Date, dtDateto.Value.Date, "", isprint, "", "");
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(this.Text, mrptheader, rptfooter, rptcriteria, "", mrpttype, "", 0m, "", "", "", ds, 0, dtDateFrom.Value.Date, dtDateto.Value.Date, "", isprint, true, "", "");
            }


        }

        private void chkReportByDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReportByDate.Checked)
                panel_ByDate.Enabled = true;
            else
                panel_ByDate.Enabled = false;

        }


    }
}