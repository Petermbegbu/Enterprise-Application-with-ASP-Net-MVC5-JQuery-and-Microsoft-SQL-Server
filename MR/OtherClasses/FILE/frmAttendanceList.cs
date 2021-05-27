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

using mradmin.Forms;
using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmAttendanceList
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, mrptheader, rptfooter = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtdocs = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE,NAME FROM DOCTORS WHERE RECTYPE = 'D'", true), dtcust = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO,NAME FROM CUSTOMER order by name", true);

        MR_DATA.MR_DATAvm vm;

        public frmAttendanceList(MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;

            //InitializeComponent();
        }

        //private void frmAttendanceList_Load(object sender, EventArgs e)
        //{
        //	initcomboboxes();
        //}

        //private void initcomboboxes()
        //{
        //	cboFacility.DataSource = dtfacility;
        //	cboFacility.ValueMember = "Type_code";
        //	cboFacility.DisplayMember = "name";

        //	combDoc.DataSource = dtdocs; //medical staff details - doctors
        //	combDoc.ValueMember = "Reference";
        //	combDoc.DisplayMember = "Name";

        //	cboHospitalAccount.DataSource = dtcust;
        //	cboHospitalAccount.ValueMember = "custno";
        //	cboHospitalAccount.DisplayMember = "name";
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btngroupcode")
        //    {
        //        this.txtgroupcode.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "g";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnPatientno")
        //    {
        //        txtpatientno.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btngrouphead")
        //    {
        //        txtgrouphead.Text = "";
        //        lookupsource = "C";
        //        msmrfunc.mrGlobals.crequired = "C";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORP. CLIENTS";
        //    }
        //    else if (btn.Name == "btnReference")
        //    {
        //        lookupsource = "R";
        //        msmrfunc.mrGlobals.crequired = "BILL";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED BILLS";
        //    }

        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "g") //groupcodee
        //    {
        //        this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
        //        this.txtgroupcode.Focus();
        //    }
        //    else if (lookupsource == "L") //patientno
        //    {
        //        txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtpatientno.Select();
        //    }
        //    else if (lookupsource == "C") //patientno
        //    {
        //        txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtgrouphead.Select();
        //    }

        //}

        //private void txtPatientno_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //        return;

        //    if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
        //    {
        //        txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
        //    }

        //    //check if patientno exists
        //    bchain = billchaindtl.Getbillchain(txtpatientno.Text, txtgroupcode.Text);
        //    if (bchain == null)
        //    {
        //        MessageBox.Show("Invalid Patient Number... ");
        //        txtpatientno.Text = " ";
        //        txtgroupcode.Select();
        //        return;
        //    }
        //    lblname.Text = bchain.NAME;
        //}

        //private void txtgrouphead_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtgrouphead.Text))
        //        return;
        //    DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + txtgrouphead.Text + "'", false);
        //    if (dtcustomer.Rows.Count < 1)
        //    {
        //        MessageBox.Show("Invalid Corporate Clients Reference...");
        //        txtgrouphead.Text = "";
        //        return;
        //    }
        //    lblname.Text = dtcustomer.Rows[0]["name"].ToString();
        //}


        void getData()
        {
            string dateFrom = string.Format("{0:yyyy-MM-dd}", vm.REPORTS.txtTimeFrom);
            string dateTo = string.Format("{0:yyyy-MM-dd}", vm.REPORTS.txtTimeTo);

            //DateTime dateFrom = Convert.ToDateTime(vm.REPORTS.txtTimeFrom);
            //DateTime dateTo = Convert.ToDateTime(vm.REPORTS.txtTimeTo);

            bool lhistory = false;
            if (Convert.ToDateTime(dateFrom).Year < msmrfunc.mrGlobals.mpyear)
                lhistory = true;

            string rptfile = lhistory ? "attendhist" : "mrattend";

            string selstring = " WHERE mrattend.trans_date >= '" + dateFrom + "' and mrattend.trans_date <= '" + dateTo + " 23:59:59.999'";
            // string selstring = " WHERE mrattend.trans_date between '" + dateFrom.ToShortDateString() + "' and  '" + dateTo.ToShortDateString() + " 23:59:59.999'"; //'2011/02/25' and '2011/02/27'"
            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtgrouphead))
                selstring += " AND mrattend.GROUPHEAD = '" + vm.REPORTS.txtgrouphead + "'";
            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtgroupcode))
                selstring += " AND mrattend.GROUPCODE = '" + vm.REPORTS.txtgroupcode + "'";
            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno))
                selstring += " AND mrattend.PATIENTNO = '" + vm.REPORTS.txtpatientno + "'";
            if (!string.IsNullOrWhiteSpace(vm.SYSCODETABSvm.ServiceCentreCodes.name))
                selstring += " AND mrattend.clinic = '" + vm.SYSCODETABSvm.ServiceCentreCodes.name + "'";
            if (vm.REPORTS.chkSortByOperator)
                selstring += " and mrattend.trans_date = '" + DateTime.Now.Date + "' and mrattend.doc_time = ''";
            if (vm.REPORTS.chkBroughtForward)
                selstring += " and mrattend.billed = 'X'";
            if (vm.REPORTS.chkDomantAccts)
                selstring += " and mrattend.doc_time = ''";
            if (vm.REPORTS.REPORT_TYPE1 == "chkNHISonly")
                selstring += " and rtrim(mrattend.groupcode) = 'NHIS'";
            else if (vm.REPORTS.REPORT_TYPE1 == "chkSpecialServicePatients")
                selstring += " and left(mrattend.reference,1) = 'S'";
            else if (vm.REPORTS.REPORT_TYPE1 == "chkPVTFC")
                selstring += " and (left(mrattend.groupcode) = 'PVT' || rtrim(mrattend.groupcode) = 'FC')";
            else if (vm.REPORTS.REPORT_TYPE1 == "chkCorporates")
                selstring += " and (mrattend.groupcode) = '' && rtrim(mrattend.groupcode) != 'NHIS')";
            else if (vm.REPORTS.REPORT_TYPE1 == "chkHMO")
                selstring += " and (mrattend.groupcode) = '' && rtrim(mrattend.groupcode) != 'NHIS')";

            if (vm.REPORTS.chkReportBankColumn && vm.REPORTS.REPORT_TYPE2 != "chkALL")
            {
                if (vm.REPORTS.REPORT_TYPE2 == "chkMedicalTreatmt")
                    selstring += " and mrattend.attendtype = 'M'";
                else if (vm.REPORTS.REPORT_TYPE2 == "chkMedicalExam")
                    selstring += " and mrattend.attendtype = 'E'";
                else if (vm.REPORTS.REPORT_TYPE2 == "chkDressingInj")
                    selstring += " and mrattend.attendtype = 'D'";
                else if (vm.REPORTS.REPORT_TYPE2 == "chkOnAppointmt")
                    selstring += " and mrattend.attendtype = 'O'";
                else if (vm.REPORTS.REPORT_TYPE2 == "chkSpecialistConsult")
                    selstring += " and mrattend.attendtype = 'S'";
                else if (vm.REPORTS.REPORT_TYPE2 == "chkOnFollowup")
                    selstring += " and mrattend.attendtype = 'F'";
                else if (vm.REPORTS.REPORT_TYPE2 == "chkDrugRefill")
                    selstring += " and mrattend.attendtype = 'R'";
            }

            if (!string.IsNullOrWhiteSpace(vm.REPORTS.SearchName))
                selstring += " AND mrattend.NAME LIKE '%" + vm.REPORTS.SearchName.Trim() + "%' ";
            if (vm.REPORTS.chkByBranch && !string.IsNullOrWhiteSpace(vm.DOCTORS.NAME))
                selstring += " and mrattend.doctor = '" + vm.DOCTORS.NAME + "'";

            string xstr = ", char(50) AS facility, CHAR(50) AS GHNAME ";
            if (vm.REPORTS.chkStaffProfiling)
                xstr += ", CHAR(50) AS address, char(50) AS email, char(50) AS phone ";
            // if (chkOnDocsPatProfile.Checked)
            //    xstr += ", CHAR(50) AS docsname ";

            if (vm.REPORTS.chkLoyaltyCustomers)
            {

                DataTable dtmthlyfig = new DataTable(), dtaggregate = new DataTable();
                msmrfunc.processComparative(ref dtmthlyfig, ref dtaggregate, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), "ATTENDANCE", vm.CUSTOMER.NAME);
                ds.Tables.Add(dtmthlyfig);
                ds.Tables.Add(dtaggregate);

                vm.REPORTS.SessionRDLC = "ComparativeRpt.rdlc";
                return;
            }
            string xstring;
            //if (chkSortByDocs.Checked)
            //    xstring = "SELECT mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.NAME, mrattend.TRANS_DATE, mrattend.CLINIC, mrattend.BILLED, mrattend.GROUPHEAD, mrattend.GROUPHTYPE, mrattend.VTAKEN, mrattend.GHGROUPCODE, medhist.DOCTOR, mrattend.DOC_TIME, mrattend.DIAGNOSIS, mrattend.ATTENDTYPE, CHAR(50) AS DOCSNAME, mrattend.REFERRER,mrattend.AUTHORIZEDCODE, billchain.section, billchain.department, billchain.PATIENTNO_PRINCIPAL, billchain.sex" + xstr + " from " + rptfile + " LEFT JOIN BILLCHAIN ON MRATTEND.GROUPCODE = BILLCHAIN.GROUPCODE AND MRATTEND.PATIENTNO = BILLCHAIN.PATIENTNO LEFT JOIN medhist on mrattend.reference = medhist.reference" + selstring;
            if (vm.REPORTS.chkStaffProfiling)
                xstring = "SELECT mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.NAME, mrattend.TRANS_DATE, mrattend.CLINIC, mrattend.BILLED, mrattend.GROUPHEAD, mrattend.GROUPHTYPE, mrattend.VTAKEN, mrattend.GHGROUPCODE, mrattend.DOCTOR, mrattend.DOC_TIME, mrattend.DIAGNOSIS, mrattend.ATTENDTYPE, CHAR(50) AS DOCSNAME, customer.name AS REFERRER, mrattend.AUTHORIZEDCODE" + xstr + " from " + rptfile + " LEFT JOIN CUSTOMER ON mrattend.referrer = customer.custno " + selstring;
            else
                xstring = "SELECT mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.NAME, mrattend.TRANS_DATE, mrattend.CLINIC, mrattend.BILLED, mrattend.GROUPHEAD, mrattend.GROUPHTYPE, mrattend.VTAKEN, mrattend.GHGROUPCODE, mrattend.DOCTOR, mrattend.DOC_TIME, mrattend.DIAGNOSIS, mrattend.ATTENDTYPE, CHAR(50) AS DOCSNAME, mrattend.REFERRER,mrattend.AUTHORIZEDCODE, billchain.section, billchain.department, billchain.PATIENTNO_PRINCIPAL, billchain.sex" + xstr + " from " + rptfile + " LEFT JOIN BILLCHAIN ON MRATTEND.GROUPCODE = BILLCHAIN.GROUPCODE AND MRATTEND.PATIENTNO = BILLCHAIN.PATIENTNO " + selstring;

            if (vm.REPORTS.REPORT_TYPE1 == "chkNHISonly" || vm.REPORTS.REPORT_TYPE1 == "chkHMO" || vm.REPORTS.REPORT_TYPE1 == "chkCorporates")
                xstring += " order by grouphead, trans_date";
            else
                xstring += " order by trans_date";
            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);


            //"SELECT mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.NAME, mrattend.TRANS_DATE, 
            //mrattend.CLINIC, mrattend.BILLED, mrattend.GROUPHEAD, mrattend.GROUPHTYPE, mrattend.VTAKEN, mrattend.GHGROUPCODE, 
            //mrattend.DOCTOR, mrattend.DOC_TIME, mrattend.DIAGNOSIS, mrattend.ATTENDTYPE, CHAR(50) AS DOCSNAME, 
            //mrattend.REFERRER,mrattend.AUTHORIZEDCODE, billchain.section, billchain.department, billchain.PATIENTNO_PRINCIPAL,
            //billchain.sex, char(50) AS facility, CHAR(50) AS GHNAME  from mrattend LEFT JOIN BILLCHAIN ON MRATTEND.GROUPCODE =
            //BILLCHAIN.GROUPCODE AND MRATTEND.PATIENTNO = BILLCHAIN.PATIENTNO  WHERE mrattend.trans_date >= '15/04/2021' and 
            //mrattend.trans_date <= '15/04/2021 23:59:59.999' order by trans_date"


            //CHECKA AND GET REMOTE DATA
            /*  if (chkGetRemoteData.Checked)
              {
                  frmGetRemoteData getrd = new frmGetRemoteData(sdt, xstring, "MR");
                  getrd.ShowDialog();
                  if (Session["remotedata"] != null)
                      sdt = (DataTable)Session["remotedata"];
              }*/
            DataTable dtt;
            string tmpstr;
            foreach (DataRow row in sdt.Rows)
            {
                row["facility"] = bissclass.combodisplayitemCodeName("type_code", row["clinic"].ToString(), dtfacility, "name");
                if (vm.REPORTS.chkStaffProfiling)
                {
                    tmpstr = row["reference"].ToString().Substring(0, 1) == "S" ? "select address1, email, phone from suspense where reference = '" + row["reference"].ToString() : "select residence As address1, email, phone from billchain where groupcode = '" + row["groupcode"].ToString() + "' and patientno = '" + row["patientno"].ToString();
                    dtt = Dataaccess.GetAnytable("", "MR", tmpstr, false);
                    row["address"] = dtt.Rows[0]["address1"].ToString();
                    row["email"] = dtt.Rows[0]["email"].ToString();
                    row["phone"] = dtt.Rows[0]["phone"].ToString();
                }
                else if (vm.REPORTS.REPORT_TYPE1 == "chkNHISonly" || vm.REPORTS.REPORT_TYPE1 == "chkCorporates" || vm.REPORTS.REPORT_TYPE1 == "chkHMO")
                {
                    row["facility"] = row["section"].ToString().Trim() + ", " + row["department"].ToString().Trim() + "[" + row["sex"].ToString() + "][" + row["PATIENTNO_PRINCIPAL"].ToString().Trim() + "]";
                }
                if (!string.IsNullOrWhiteSpace(row["doctor"].ToString())) // chkOnDocsPatProfile.Checked)
                    row["docsname"] = bissclass.combodisplayitemCodeName("reference", row["doctor"].ToString(), dtdocs, "name");
                else
                    row["docsname"] = "< Unspecified >";
                if (row["patientno"].ToString() == row["grouphead"].ToString())
                    row["ghname"] = "< S E L F >";
                else if (row["grouphtype"].ToString() == "P")
                    row["ghname"] = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P");
                else
                    row["ghname"] = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dtcust, "name");
            }
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        public MR_DATA.REPORTS printprocess()
        {
            bool isprint = false;
            DateTime dateFrom = Convert.ToDateTime(vm.REPORTS.txtTimeFrom);
            DateTime dateTo = Convert.ToDateTime(vm.REPORTS.txtTimeTo);

            //DialogResult result;
            if (dateFrom.Date > DateTime.Now.Date || dateFrom.Date < msmrfunc.mrGlobals.mta_start || dateTo.Date < dateFrom.Date || dateFrom.Year != dateTo.Year)
            {
                vm.REPORTS.alertMessage = "Invalid Date Specification";

                return vm.REPORTS;
            }

            /*if (chkMonthlyComparative.Checked && dateFrom == dateTo)
			{
				DialogResult result = MessageBox.Show("Monthly Comparative summary report should be \r\n for more than 1 day or 1 month...CONTINUE?", "Comparative Report Date Range", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (result == DialogResult.No )
					return;
			}*/

            if (vm.REPORTS.chkLoyaltyCustomers && string.IsNullOrWhiteSpace(vm.CUSTOMER.NAME))
            {
                vm.REPORTS.alertMessage = "Hospital Account for Staff Registrations must be Selected...";

                return vm.REPORTS;
            }

            /* if (sdt != null)
             {
                 sdt.Rows.Clear();
                 ds.Tables.Clear();
                 ds.Clear();
             }*/

            sdt = new DataTable();
            ds = new DataSet();

            getData();

            string xperiod = " FOR ";
            if (vm.REPORTS.chkSortByOperator)
                xperiod += bissclass.DateWord(DateTime.Now.Date);
            else if (dateFrom == dateTo)
                xperiod += bissclass.DateWord(dateFrom.Date);
            else
            {
                xperiod += dateFrom.Date.ToString("MMMM") + " " + dateFrom.Day.ToString();
                string xyear = "";
                if (dateFrom.Month == dateTo.Month)
                    xyear = " - " + dateTo.Day.ToString() + ",  " + dateFrom.Year.ToString();
                else if (dateFrom.Month != dateTo.Month && dateFrom.Year == dateTo.Year)
                    xyear = " - " + dateTo.Date.ToString("MMMM") + " " + dateTo.Day.ToString() + ",  " + dateFrom.Year.ToString();
                else
                    xyear = ", " + dateFrom.Year.ToString() + " - " + dateTo.Date.ToString("MMMM") + " " + dateTo.Day.ToString() + ",  " + dateTo.Year.ToString();
                xperiod += xyear;
            }

            datefrom = dateFrom.Date; dateto = dateTo.Date;

            vm.REPORTS.SessionSQL = "";

            if (vm.REPORTS.chkLoyaltyCustomers)
            {
                mrptheader = "MONTHLY COMPARATIVE SUMMARY REPORT";
                if (datefrom.Month != 1 || dateto.Month != 12)
                {
                    datefrom = Convert.ToDateTime("01/01/" + datefrom.Year.ToString());
                    dateto = Convert.ToDateTime("31/12/" + datefrom.Year.ToString());
                }
            }
            else
            {
                if (sdt.Rows.Count < 1)
                {
                    vm.REPORTS.alertMessage = "No Data for Specified Conditions...";

                    return vm.REPORTS;
                }

                ds.Tables.Add(sdt);

                if (vm.REPORTS.chkbyacctofficers || !string.IsNullOrWhiteSpace(vm.SYSCODETABSvm.ServiceCentreCodes.name))
                    vm.REPORTS.SessionRDLC = "Attendance_ByClinicGh.rdlc"; //"Attendance_ByClinic.rdlc";
                else if (vm.REPORTS.chkReportBankColumn)
                    vm.REPORTS.SessionRDLC = "Attendance_VisitType.rdlc";
                else if (vm.REPORTS.chkByBranch)
                    vm.REPORTS.SessionRDLC = "Attendance_ByDocs.rdlc";
                else if (vm.REPORTS.chkStaffProfiling)
                    vm.REPORTS.SessionRDLC = "Attendance_MiscDtl.rdlc";
                else if (vm.REPORTS.REPORT_TYPE1 == "chkNHISonly")
                    vm.REPORTS.SessionRDLC = "Attendance_NHIS.rdlc";
                else
                    vm.REPORTS.SessionRDLC = "Attendance_Serial.rdlc";
                vm.REPORTS.SessionWaitonly = vm.REPORTS.chkSortByOperator ? "Y" : "N";
                if (vm.REPORTS.chkSortByOperator)
                    mrptheader = "WAITING LIST"; // FOR " + bissclass.DateWord(DateTime.Now.Date);
                else if (vm.REPORTS.chkBroughtForward)
                    mrptheader = "SERVICE EXCEPTIONS LISTING";
                else
                    mrptheader = "ATTENDANCE LIST";


                if (vm.REPORTS.REPORT_TYPE1 == "chkSpecialServicePatients")
                    xperiod += " - SPECIAL SERVICE ONLY";
                else if (vm.REPORTS.REPORT_TYPE1 == "chkNHISonly")
                    xperiod += " - NHIS ONLY";
                else if (vm.REPORTS.REPORT_TYPE1 == "chkPVTFC")
                    xperiod += " - EXCLUDES NHIS & SP.SERVICE";

            }

           
            frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader + xperiod, rptfooter, "", "", 
                vm.REPORTS.chkLoyaltyCustomers ? "SUMMARYOFACCTS" : "ATTENDANCE", "", 
                0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "", vm.REPORTS);

            //if (isprint)
            //    paedreports.work();
            //else

            vm.REPORTS = paedreports.Show(vm.REPORTS.SessionRDLC, vm.REPORTS.SessionSQL, vm.REPORTS.PRINT);

            return vm.REPORTS;

            //else
            //{
            //    MRrptConversion.GeneralRpt(mrptheader, mrptheader + xperiod, rptfooter, "", "", vm.REPORTS.chkLoyaltyCustomers ? "SUMMARYOFACCTS" : "ATTENDANCE", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
            //}
        }

        //private void btnPreview_Click(object sender, EventArgs e)
        //{
        //    printprocess(false);
        //}

        //private void btnPrint_Click(object sender, EventArgs e)
        //{
        //    printprocess(true);
        //}

        //private void chkMonthlyComparative_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkMonthlyComparative.Checked)
        //        panel_MonthlyCummative.Visible = true;
        //    else
        //        panel_MonthlyCummative.Visible = false;

        //}

    }
}