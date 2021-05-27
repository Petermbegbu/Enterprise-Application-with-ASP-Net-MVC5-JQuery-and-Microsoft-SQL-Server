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
using mradmin.BissClass;
using mradmin.Forms;
using mradmin.DataAccess;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion


namespace OtherClasses.FILE
{
    public partial class hmoauthorizationcode
    {
        string lookupsource, AnyCode, woperator, calenderText;
        Mrattend mrattend = new Mrattend();
        Customer customers = new Customer();
        HmoAuthorizations hmoautho = new HmoAuthorizations();
        DataTable dtclinic = Dataaccess.GetAnytable("", "CODES", "select type_code,name from servicecentrecodes order by name", true),
                    dtdocs = Dataaccess.GetAnytable("", "MR", "select reference,name from doctors where rectype = 'D' order by name", true);
        bool newrec;

        MR_DATA.MR_DATAvm vm;


        public hmoauthorizationcode(MR_DATA.MR_DATAvm VM2, string woperato)
        {
            vm = VM2;

            woperator = woperato;
            //InitializeComponent();
        }

        //private void btnclose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void hmoauthorizationcode_Load(object sender, EventArgs e)
        //{
        //    initcomboboxes();
        //}

        //private void initcomboboxes()
        //{
        //    //get primary doc
        //    cboprimarydoc.DataSource = dtdocs;
        //    cboprimarydoc.ValueMember = "Reference";
        //    cboprimarydoc.DisplayMember = "Name";

        //    //get clinic
        //    cboClinic.DataSource = dtclinic;
        //    cboClinic.ValueMember = "Type_code";
        //    cboClinic.DisplayMember = "Name";

        //    cboReferredClinic.DataSource = Dataaccess.GetAnytable("", "CODES", "select type_code,name from servicecentrecodes order by name", true);
        //    cboReferredClinic.ValueMember = "Type_code";
        //    cboReferredClinic.DisplayMember = "Name";
        //}

        //void ClearControls()
        //{
        //    txtreference.Text = txtgroupcode.Text = txthmocode.Text = txtothers.Text = txtpatientno.Text = cboClinic.Text = cboprimarydoc.Text = cboReferredClinic.Text = lblgroupheadname.Text = lblpatientname.Text = txtDateReceived.Text = "";
        //    dtReferraldate.Value = DateTime.Now;
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;

        //    if (btn.Name == "btnAttendance")
        //    {
        //        this.txtreference.Text = "";
        //        lookupsource = "I";
        //        msmrfunc.mrGlobals.lookupCriteria = chkTodaysConsult.Checked ? "C" : "";
        //        msmrfunc.mrGlobals.crequired = "I";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED DAILY ATTENDANCE";
        //    }

        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "I") //daiy attendance
        //    {
        //        msmrfunc.mrGlobals.lookupCriteria = "";
        //        this.txtreference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtreference.Focus();
        //    }
        //}


        //private void txtreference_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtreference.Text))
        //        return;

        //    mrattend = Mrattend.GetMrattend(this.txtreference.Text);

        //    if (mrattend == null)
        //    {
        //        DialogResult result = MessageBox.Show("Unable to Link Consultation Reference in Daily Attendance Register... ");
        //        this.txtreference.Text = " ";
        //        return;
        //    }

        //    txtgroupcode.Text = mrattend.GROUPCODE;
        //    txtpatientno.Text = mrattend.PATIENTNO;
        //    lblpatientname.Text = mrattend.NAME;

        //    customers = Customer.GetCustomer(mrattend.GROUPHEAD);

        //    if (customers == null || !customers.HMO)
        //    {
        //        DialogResult result = MessageBox.Show("The Record is not an HMO Account...");
        //        txtreference.Text = "";
        //        return;
        //    }

        //    newrec = true;
        //    txthmocode.Text = mrattend.AUTHORIZEDCODE;
        //    lblgroupheadname.Text = customers.Name;
        //    hmoautho = HmoAuthorizations.GetHMOAUTHORIZATIONS(mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO);

        //    if (hmoautho != null)
        //    {
        //        newrec = false;
        //        txtothers.Text = hmoautho.REQUESTDETAILS;
        //        txthmocode.Text = hmoautho.AUTHORIZEDCODE;
        //        dtReferraldate.Value = hmoautho.REFERRALDATE;
        //        bissclass.displaycombo(cboClinic, dtclinic, hmoautho.REFERRALCLINIC, "type_code");
        //        bissclass.displaycombo(cboReferredClinic, dtclinic, hmoautho.REFERREDTOCLINIC, "type_code");
        //        bissclass.displaycombo(cboprimarydoc, dtdocs, hmoautho.REFERRAL, "reference");
        //        txtDateReceived.Text = hmoautho.DATERECEIVED;
        //    }

        //}

        public MR_DATA.REPORTS btnSubmit_Click()
        {
            //if (!bissclass.IsPresent(this.txtgroupcode, "Patients Groupcode", false) ||
            //    !bissclass.IsPresent(this.txtpatientno, "Patient Number", false) ||
            //    !bissclass.IsPresent(this.lblpatientname, "Patients Name", false) ||
            //    !bissclass.IsPresent(this.lblgroupheadname, "Responsible for Bill", false) ||
            //    !bissclass.IsPresent(this.txtothers, "Request Details", false) ||
            //    !bissclass.IsPresent(this.txtreference, "Consultation Reference", false) ||
            //    !bissclass.IsPresent(this.cboClinic, "Referring Clinic", false) ||
            //    !bissclass.IsPresent(this.cboReferredClinic, "ReferredTo Clinic", false) ||
            //    !bissclass.IsPresent(this.cboprimarydoc, "Referring Doctor", false))
            //    return;

            //DialogResult result = MessageBox.Show("Confirm to Submit Details...", "HMO AuthorizationCode Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            savedetails();

            vm.REPORTS.alertMessage = "Record Submitted...";

            //ClearControls();

            return vm.REPORTS;
        }

        void savedetails()
        {
            mrattend = Mrattend.GetMrattend(vm.REPORTS.txtreference);
            newrec = vm.REPORTS.newrecString == "true" ? true : false;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = newrec ? "HMOAUTHORIZATIONS_Add" : "HMOAUTHORIZATIONS_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@REFERENCE", vm.REPORTS.txtreference);
            insertCommand.Parameters.AddWithValue("@GROUPCODE", mrattend.GROUPCODE);
            insertCommand.Parameters.AddWithValue("@PATIENTNO", mrattend.PATIENTNO);
            insertCommand.Parameters.AddWithValue("@NAME", mrattend.NAME);
            insertCommand.Parameters.AddWithValue("@TRANS_DATE", mrattend.TRANS_DATE);
            insertCommand.Parameters.AddWithValue("@REFERRAL", vm.DOCTORS.NAME);  //for cboprimarydoc.SelectedValue.ToString()
            insertCommand.Parameters.AddWithValue("@REFERRALCLINIC", vm.REPORTS.txtclinic);
            insertCommand.Parameters.AddWithValue("@REFERRALDATE", vm.REPORTS.REPORT_TYPE1);
            insertCommand.Parameters.AddWithValue("@REFERREDTODOC", "");
            insertCommand.Parameters.AddWithValue("@REFERREDTOCLINIC", vm.REPORTS.txtstaffno);
            insertCommand.Parameters.AddWithValue("@REQUESTCOMMENCED", newrec ? DateTime.Now : hmoautho.REQUESTCOMMENCED);
            insertCommand.Parameters.AddWithValue("@REQUESTDETAILS", vm.REPORTS.edtallergies);
            insertCommand.Parameters.AddWithValue("@GROUPHEAD", mrattend.GROUPHEAD);
            insertCommand.Parameters.AddWithValue("@GROUPHTYPE", mrattend.GROUPHTYPE);
            insertCommand.Parameters.AddWithValue("@POSTED", false);
            insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@GHGROUPCODE", "");
            insertCommand.Parameters.AddWithValue("@DIAGNOSIS", mrattend.DIAGNOSIS);
            insertCommand.Parameters.AddWithValue("@OPERATOR", woperator);
            insertCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@AUTHORIZEDCODE", vm.REPORTS.txtgrouphead);
            insertCommand.Parameters.AddWithValue("@DATERECEIVED", vm.REPORTS.REPORT_TYPE2);

            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();
        }

        //private void btnMedExamDate_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btnDateReceived")
        //        panelCalender.Location = new System.Drawing.Point(437, 217);
        //    panelCalender.Visible = true;
        //    calenderText = btn.Name;
        //}

        //private void monthCalendar1_DateSelected(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btnDateSelect")
        //    {
        //        if (calenderText == "btnMedExamDate")
        //            txtDateReceived.Text = monthCalendar1.SelectionStart.ToShortDateString();
        //        panelCalender.Visible = false;
        //    }
        //    else if (btn.Name == "btnIgnorDateSelect")
        //    {
        //        panelCalender.Visible = false;
        //    }
        //}

        //private void txtreference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtreference_LostFocus(null, null);
        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}
    }
}