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
using mradmin.DataAccess;
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class samplecollectiondetails
    {
        //tables
        Mrattend mrattend = new Mrattend();
        billchaindtl bchain = new billchaindtl();
        MedHist medhist = new MedHist();
        Customer customers = new Customer();
        patientinfo patients = new patientinfo();
        Admrecs admrec = new Admrecs();
        PleaseWaitForm pleaseWait = new PleaseWaitForm();
        DataTable dts = msmrfunc.getmrsetup("");
        DataTable mrcontrol = Dataaccess.GetAnytable("", "MR", "select * from mrcontrol order by recid", false),
            dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES", true);
        LABDET labdet = new LABDET();
        DataTable servicetagged, suspense;

        string woperator, refprefix, Anycode, msection, mgrouphtype, PicSelected, start_time, AnyCode, lookupsource, mreference;
        bool automat_costing, strictreferal, newrec, mcanadd, mcandelete, mcanalter;
        int servicerecno, nwseclevel;
        decimal mlastno;
        string[] taggedFrmSuspensea_ = new string[10];

        MR_DATA.MR_DATAvm vm;

        public samplecollectiondetails(MR_DATA.MR_DATAvm VM2, string woperato)
        {
            //InitializeComponent();
            vm = VM2;

            bchain = billchaindtl.Getbillchain(vm.REPORTS.txtpatientno, vm.REPORTS.txtgroupcode);

            mlastno = vm.REPORTS.mlastno;
            newrec = vm.REPORTS.newrecString == "true" ? true : false;
            msection = vm.REPORTS.msection;
            woperator = woperato;

            getcontrolsettings();

            //if (nwseclevel != 1)
            //{
            //    MessageBox.Show("Limited Access - " + woperator.Trim() + " not defined for Phlebotomy. . .", "");
            //}

        }

        //private void samplecollectiondetails_Load(object sender, EventArgs e)
        //{
        //    initcomboboxes();
        //   // msection = Session["section"].ToString(); 
        //   // woperator = Session["operator"].ToString(); 
        //    getcontrolsettings();

        //    if(nwseclevel != 1)
        //    {
        //        MessageBox.Show("Limited Access - "+woperator.Trim()+" not defined for Phlebotomy. . .","");
        //    }
        //}

        private void getcontrolsettings()
        {
            // DataTable dt = Dataaccess.GetAnytable("", "MR", "select * from mrcontrol order by recid", false);

            refprefix = mrcontrol.Rows[1]["sprefix"].ToString();
            automat_costing = (bool)mrcontrol.Rows[2]["automedbil"];
            strictreferal = (bool)mrcontrol.Rows[3]["seclink"];

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            nwseclevel = (Int32)dt.Rows[0]["wseclevel"];
            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];
        }

        //private void initcomboboxes()
        //{
        //    //get clinic
        //    this.combfacility.DataSource = dtfacility;
        //    combfacility.ValueMember = "Type_code";
        //    combfacility.DisplayMember = "name";
        //    ////referring Docs
        //    //combReferringDoc.DataSource = selcode.getcombolist("c", "MR");  //selcode.getsyscodes("c");
        //    //combReferringDoc.ValueMember = "Custno";
        //    //combReferringDoc.DisplayMember = "Name";
        //}

        //void clearcontrols()
        //{
        //    txtaddress.Text = txtage.Text = txtcrossref.Text = txtdefault.Text = txtfullname.Text =
        //    txtgroupcode.Text = txtothernotes.Text = txtpatientno.Text = txtGroupheadName.Text = txtreferringdoctor.Text =
        //    txtsamplecollectedBy.Text = txtReference.Text = txtothernotes.Text = combsex.Text = "";
        //    dtcollectiondate.Value = DateTime.Now.Date;

        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;

        //    if (btn.Name == "btnAttendance")
        //    {
        //        txtcrossref.Text = "";
        //        lookupsource = "I";
        //        msmrfunc.mrGlobals.crequired = "I";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED DAILY ATTENDANCE";
        //    }
        //    else if (btn.Name == "txtReference")
        //    {
        //        this.txtReference.Text = "";
        //        lookupsource = "LR";
        //        msmrfunc.mrGlobals.crequired = "IVR";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED INVESTIGATION DETAILS";
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
        //        txtcrossref.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtcrossref.Focus();
        //    }
        //    else if (lookupsource == "LR") //daiy attendance
        //    {
        //        txtReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtReference.Focus();
        //    }
        //}


        //private void combfacility_Leave(object sender, EventArgs e)
        //{
        //    if (combfacility.SelectedValue == null)
        //        return;

        //    bool foundit = false;

        //    servicerecno = 0;
        //    for (int i = 0; i < mrcontrol.Rows.Count; i++)
        //    {
        //        if (mrcontrol.Rows[i]["MPASS"].ToString().Trim() == combfacility.SelectedValue.ToString().Trim())
        //        {
        //            foundit = true;
        //            servicerecno = i;
        //            break;
        //        }
        //    }
        //    if (!foundit)
        //    {
        //        MessageBox.Show("This Service Centre has not been properly configured...", "Service Centre/Facility Details");
        //        btnclose.PerformClick();
        //        return;
        //    }
        //    servicerecno = 5;
        //}

        //private void dtTrans_date_Enter(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(combfacility.Text))
        //    {
        //        MessageBox.Show("Facility Cannot be empty...", "Service ID");
        //        combfacility.Select();
        //        return;
        //    }
        //}

        //private void dtTrans_date_Leave(object sender, EventArgs e)
        //{
        //    if (dttrans_date.Value.Date > DateTime.Now.Date)
        //    {
        //        MessageBox.Show("Service Date cannot be greater than Today...", "Invalid date Specificication");
        //        dttrans_date.Select();
        //        return;
        //    }
        //    txtReference.Select();
        //}


        //private void txtcrossref_Enter(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(combfacility.Text))
        //    {
        //        MessageBox.Show("Facility Cannot be empty...", "Service ID");
        //        combfacility.Select();
        //        return;
        //    }

        //    if (!string.IsNullOrWhiteSpace(Anycode))
        //        txtcrossref.Text = Anycode;

        //    else if (new string[] { "3", "4", "6" }.Contains(msection)) //nurses/docs/lab/xray/ecg/thearthre
        //    {
        //        //get list of patients for investigations
        //        /* frmlinkinfo FrmLinkinfo = new frmlinkinfo("", 0, 0m, 0m, combfacility.SelectedValue.ToString(), true, msection, 0, "", "");
        //         FrmLinkinfo.Closed += new EventHandler(FrmLinkinfo_Closed);
        //         FrmLinkinfo.ShowDialog();*/

        //        DataTable dt = msmrfunc.getLinkDetails("", 0, 0m, 0m, combfacility.SelectedValue.ToString(), true, msection, 0, "", "");
        //        if (dt.Rows.Count > 0)
        //        {
        //            frmGetlinkinfo linkinfo = new frmGetlinkinfo(dt);
        //            linkinfo.ShowDialog();
        //            txtcrossref.Text = Anycode = msmrfunc.mrGlobals.anycode;
        //            txtcrossref.Select();
        //        }

        //    }
        //}


        //void FrmLinkinfo_Closed(object sender, EventArgs e)
        //{
        //    frmlinkinfo FrmLinkinfo_Closed = sender as frmlinkinfo;
        //    {
        //        txtcrossref.Text = Anycode = msmrfunc.mrGlobals.anycode;
        //        //if (strictreferal && string.IsNullOrWhiteSpace(Anycode))
        //        //{
        //        //    MessageBox.Show("Investigations/Procedures must be Referred...", "STRICT REFERAL CONTROL");
        //        //    txtReference.Select();
        //        //    return;
        //        //}
        //        txtcrossref.Select();
        //    }
        //}

        //private void txtcrossref_Leave(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(txtcrossref.Text))
        //    {
        //        suspense = SUSPENSE.GetSUSPENSE(txtcrossref.Text, "A");
        //        if (!displaypatdetails(txtcrossref.Text))
        //        {
        //            txtcrossref.Text = "";
        //            txtcrossref.Select();
        //            return;
        //        }
        //    }
        //}

        //bool displaypatdetails(string xreference)
        //{
        //    bool foundit = true;
        //    string xtranstype = xreference.Substring(0, 1);
        //    if (xtranstype == "A") //admissions
        //    {
        //        admrec = Admrecs.GetADMRECS(xreference);
        //        if (admrec == null)
        //        {
        //            foundit = false;
        //            DialogResult result = MessageBox.Show("Invalid Admissions Reference... ", "IN-PATIENT PAYMENT");
        //            return false;
        //        }
        //        chkInpatient.Checked = true;
        //    }
        //    else if (xtranstype == "C" || xtranstype == "S")
        //    {
        //        mrattend = Mrattend.GetMrattend(xreference);
        //        if (mrattend == null)
        //        {
        //            foundit = false;
        //            DialogResult result = MessageBox.Show("Unable to Link this Consultation Reference in Daily Attendance Register... ");
        //            return false;
        //        }
        //        //dtTrans_date.Value = mrattend.TRANS_DATE;
        //    }
        //    else
        //    {
        //        foundit = false;
        //        DialogResult result = MessageBox.Show("Invalid Number Format for Consultation/Admission Reference...");
        //        return false;
        //    }
        //    if (!foundit)
        //    {
        //        txtcrossref.Select();
        //        return false;
        //    }
        //    mreference = xreference;
        //    mgrouphtype = (xtranstype == "A") ? admrec.GROUPHTYPE : mrattend.GROUPHTYPE;
        //    txtfullname.Text = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;
        //    txtgroupcode.Text = (xtranstype == "A") ? admrec.GROUPCODE : mrattend.GROUPCODE;
        //    txtpatientno.Text = (xtranstype == "A") ? admrec.PATIENTNO : mrattend.PATIENTNO;
        //    txtreferringdoctor.Text = (xtranstype != "A") ? mrattend.REFERRER : admrec.DOCTOR;
        //    //txtBills.Text = labdet.AMOUNT.ToString("N2");
        //    //  Combillspayable.Text = (string.IsNullOrWhiteSpace(txtpatientno.Text) || txtpatientno.Text == txtgrouphead.Text) ? "SELF" : mgrouphtype == "P" ? "Another Patient" : "Corporate Client";
        //    txtghgroupcode.Text = (xtranstype == "A") ? admrec.GHGROUPCODE : mrattend.GHGROUPCODE;
        //    txtgrouphead.Text = (xtranstype == "A") ? admrec.GROUPHEAD : mrattend.GROUPHEAD;
        //    chkInpatient.Checked = xtranstype == "A" ? true : false;
        //    if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        txtGroupheadName.Text = getgrouphead(txtgrouphead.Text, txtghgroupcode.Text, mgrouphtype);
        //        if (txtgrouphead.Text.Trim() == "Abort")
        //        {
        //            txtReference.Select();
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        txtGroupheadName.Text = "< SELF >";
        //    }
        //    if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        bchain = billchaindtl.Getbillchain(txtpatientno.Text, txtgroupcode.Text);
        //        if (bchain == null)
        //        {
        //            DialogResult result = MessageBox.Show("Critical Error in Patient Master File... Call Technical Support", "Patient Master File");
        //            return false;
        //        }
        //        combsex.Text = bchain.SEX;
        //        txtaddress.Text = bchain.RESIDENCE;
        //        txtage.Text = bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now.Date);
        //        PicSelected = bchain.PICLOCATION;
        //        if (!string.IsNullOrWhiteSpace(PicSelected))
        //        {
        //            pictureBox1.Image = WebGUIGatway.getpicture(PicSelected);
        //        }
        //    }

        //    if (!string.IsNullOrWhiteSpace(txtpatientno.Text) && bchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(bchain.HMOSERVTYPE))
        //    {
        //        //for customer.hmo  - 30-09-2012
        //        HmoAuthorizations hmoauthorization = HmoAuthorizations.GetHMOAUTHORIZATIONS(txtcrossref.Text,
        //            bchain.GROUPCODE, bchain.PATIENTNO);
        //        if (hmoauthorization != null && string.IsNullOrWhiteSpace(hmoauthorization.AUTHORIZEDCODE))
        //        {
        //            MessageBox.Show("There is a pending Treatment Authorization Request on this patient for this Reference : " + txtcrossref.Text.Trim(), "HMO AUTHORIZATION REQUEST ALERT!!!");
        //            return false;
        //        }
        //    }
        //    if (txtcrossref.Text.Substring(0, 1) != "A" && !string.IsNullOrWhiteSpace(txtpatientno.Text) && bchain.GROUPHTYPE == "C" &&
        //        customers.Tosignbill) //13-04-2013 check if outpatient and patient is to sing bill - 
        //    {
        //        BSDETAIL bsdetail = BSDETAIL.GetBSDETAIL(txtcrossref.Text);
        //        if (bsdetail != null && !bsdetail.SIGNEDBILL)
        //        {
        //            DialogResult result = MessageBox.Show("This Patient is required TO SIGN BILL before service on this Reference : " + txtcrossref.Text.Trim() +
        //                " can be received...", "ADMINISTRATORS SETUP REQUIREMENT!!!");
        //            return false;
        //        }
        //    }
        //    //check for investigations request in suspense and get details to acctfromsusp form for tagging
        //    //  displayallrequests();
        //    servicetagged = SUSPENSE.GetSUSPENSE(txtcrossref.Text, "U");
        //    if (servicetagged.Rows.Count > 0)
        //    {
        //        frmAcctfromSusp FrmacctFromsusp = new frmAcctfromSusp(servicetagged, txtcrossref.Text.Trim() + "  : " + txtfullname.Text.Trim() + " : " + txtgroupcode.Text.Trim() + "-" + txtpatientno.Text.Trim());
        //        FrmacctFromsusp.Closed += new EventHandler(FrmacctFromsusp_Closed);
        //        FrmacctFromsusp.ShowDialog();
        //    }
        //    else
        //    {
        //        DialogResult result = MessageBox.Show("No Selected or Pending Service on this Reference..." + "\n" + "for This Service Centre... Pls Confirm to Continue... ", "",
        //            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1); //, msgBoxHandler);
        //        if (result == DialogResult.No)
        //        {
        //            return false;
        //        }
        //    }
        //    if (servicetagged.Rows.Count > 0 && string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        combsex.Text = servicetagged.Rows[0]["sex"].ToString();
        //        txtage.Text = servicetagged.Rows[0]["age"].ToString();
        //        txtaddress.Text = servicetagged.Rows[0]["address"].ToString();

        //        txtgroupcode.Enabled = txtpatientno.Enabled = false;
        //    }

        //    if (string.IsNullOrWhiteSpace(txtsamplecollectedBy.Text))
        //    {
        //        txtsamplecollectedBy.Text = woperator;
        //        dtcollectiondate.Value = DateTime.Now;
        //    }
        //    return true;
        //}

        //void FrmacctFromsusp_Closed(object sender, EventArgs e)
        //{
        //    txtrequestprofiles.Text = "";
        //    taggedFrmSuspensea_ = msmrfunc.mrGlobals.taggedFromSuspensea_;
        //    for (int i = 0; i < servicetagged.Rows.Count; i++)
        //    {
        //        if (msmrfunc.mrGlobals.taggedFromSuspensea_[i] == "YES") //tagged
        //            txtrequestprofiles.Text += servicetagged.Rows[i]["description"].ToString().Trim() + ",";
        //    }
        //}

        string getgrouphead(string xgrouphead, string xghgroupcode, string xtype)
        {
            string xreturnvalue = "";
            if (xtype == "P")
            {
                patients = patientinfo.GetPatient(xgrouphead, xghgroupcode);
                if (patients == null)
                {
                    //msgeventtracker = "g";
                    DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS", "GroupHead Informaton");
                    xreturnvalue = "Abort";
                }
            }
            else
            {
                customers = Customer.GetCustomer(xgrouphead);
                if (customers == null)
                {
                    DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS", "GroupHead Informaton");
                    xreturnvalue = "Abort";
                }
            }
            if (xreturnvalue != "Abort")
            {
                xreturnvalue = (xtype == "P" && xgrouphead == patients.patientno) ?
                    "< SELF >" : (xtype == "C") ? customers.Name : patients.name;
            }
            return xreturnvalue;
        }

        //private void panel3_Enter(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtsamplecollectedBy.Text))
        //    {
        //        DialogResult result = MessageBox.Show("Sample Collector ID and date/Time must be specified to use this segment...");
        //        txtsamplecollectedBy.Focus();
        //        return;
        //    }
        //}

        //private void panel3_LostFocus(object sender, EventArgs e)
        //{
        //    string xchecked = chkblood.Checked ? "Blood; " : chksputum.Checked ? "Sputum; " : chkstool.Checked ? "Stool; " : chkurine.Checked ? "Urine; " : chkswab.Checked ? "Swab; " : chksemen.Checked ? "Semen; " : chkhair.Checked ? "Hair; " : !string.IsNullOrWhiteSpace(txtothers.Text) ? "Others - (" + txtothers.Text.Trim() + ")" : "";
        //    txtdefault.Text = "=> Sample Taken : " + xchecked + " - by " + txtsamplecollectedBy.Text.Trim() + "  @  " + dtcollectiondate.Value.Date + " " + dtcollectiondate.Value.ToShortTimeString();
        //    txtothernotes.Text = txtdefault.Text;
        //    btnsave.Enabled = true;
        //}

        //private void txtdefault_Click(object sender, EventArgs e)
        //{
        //    string xchecked = chkblood.Checked ? "Blood; " : chksputum.Checked ? "Sputum; " : chkstool.Checked ? "Stool; " : chkurine.Checked ? "Urine; " : chkswab.Checked ? "Swab; " : chksemen.Checked ? "Semen; " : chkhair.Checked ? "Hair; " : !string.IsNullOrWhiteSpace(txtothers.Text) ? "Others - (" + txtothers.Text.Trim() + ")" : "";
        //    txtdefault.Text = "=> Sample Taken : " + xchecked + " - by " + txtsamplecollectedBy.Text.Trim() + "  @  " + dtcollectiondate.Value.Date + " " + dtcollectiondate.Value.ToShortTimeString();
        //    if (string.IsNullOrWhiteSpace(txtothernotes.Text))
        //        txtothernotes.Text = txtdefault.Text;
        //    if (xchecked != "")
        //        btnsave.Enabled = true;
        //}

        public MR_DATA.REPORTS btnsave_Click()
        {
            //if (newrec && !mcanadd)
            //{
            //    DialogResult result = MessageBox.Show("ACCESS DENIED...To New Record Creation.  See your Systems Administator!");
            //    btnclose.Select();
            //    return;
            //}

            //if (!bissclass.IsPresent(txtfullname, "Patients Name", false) ||
            //    !bissclass.IsPresent(txtGroupheadName, "'Bills Payable By'", false) ||
            //    !bissclass.IsPresent(txtcrossref, "Service Reference", false) ||
            //    !bissclass.IsPresent(combfacility, "FACILITY DETAILS", true))
            //{
            //    return;
            //}

            //DialogResult result1 = MessageBox.Show("Confirm to Save...", "Attendance Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result1 == DialogResult.No)
            //    return;

            if (newrec) //update ref. counter and get a new value, if necessary.
            {
                decimal lastnosave = mlastno;
                mlastno = msmrfunc.getcontrol_lastnumber("XRAYNO", 5, false, mlastno, false);

                if (mlastno != lastnosave)
                    vm.REPORTS.txtreference = bissclass.autonumconfig(mlastno.ToString(), true, "C", "999999999");
            }

            savedetails();

            return vm.REPORTS;
        }

        void savedetails()
        {                

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = newrec ? "PHL01_Add" : "PHL01_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", vm.REPORTS.txtreference);
            insertCommand.Parameters.AddWithValue("@name", vm.REPORTS.TXTPATIENTNAME);
            insertCommand.Parameters.AddWithValue("@patientno", vm.REPORTS.txtpatientno);
            insertCommand.Parameters.AddWithValue("@groupcode", vm.REPORTS.txtgroupcode);
            insertCommand.Parameters.AddWithValue("@sex", vm.REPORTS.cbogender);
            insertCommand.Parameters.AddWithValue("@address1", vm.REPORTS.txtaddress1);
            insertCommand.Parameters.AddWithValue("@birthdate", !string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno) ? bchain.BIRTHDATE.Date : msmrfunc.mrGlobals.mta_start);
            insertCommand.Parameters.AddWithValue("@doctor", vm.REPORTS.doctor);
            insertCommand.Parameters.AddWithValue("@trans_date", Convert.ToDateTime(vm.REPORTS.REPORT_TYPE1).Date);
            insertCommand.Parameters.AddWithValue("@billself", string.IsNullOrWhiteSpace(vm.REPORTS.doctor) || vm.REPORTS.txtpatientno.Trim() == vm.REPORTS.txtgrouphead.Trim() ? "Y" : "N");
            insertCommand.Parameters.AddWithValue("@facility", vm.SYSCODETABSvm.ServiceCentreCodes.name );
            insertCommand.Parameters.AddWithValue("@posted", true);
            insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@crossref", vm.REPORTS.REPORT_TYPE2); //for crossRef
            insertCommand.Parameters.AddWithValue("@age", 0m);
            insertCommand.Parameters.AddWithValue("@grouphtype", vm.REPORTS.mgrouphtype);
            insertCommand.Parameters.AddWithValue("@grouphead", vm.REPORTS.txtgrouphead);
            insertCommand.Parameters.AddWithValue("@GHGROUPCODE", vm.REPORTS.txtghgroupcode);
            insertCommand.Parameters.AddWithValue("@operator", woperator);
            insertCommand.Parameters.AddWithValue("@dtime", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@sampleby", vm.REPORTS.REPORT_TYPE4.Trim()); //for  txtsamplecollectedBy.Text
            insertCommand.Parameters.AddWithValue("@sampledate", vm.REPORTS.REPORT_TYPE5); //for dtcollectiondate.Value
            insertCommand.Parameters.AddWithValue("@others", vm.REPORTS.edtspinstructions.Trim()); //for txtothernotes
            insertCommand.Parameters.AddWithValue("@reqprofile", vm.REPORTS.REPORT_TYPE3.Trim()); //for txtrequestprofiles
            insertCommand.Parameters.AddWithValue("@defaultstring", vm.REPORTS.edtallergies.Trim()); //for  txtdefault.Text
            insertCommand.Parameters.AddWithValue("@textage", vm.REPORTS.cboAge);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                vm.REPORTS.alertMessage = " " + ex;
                return;
            }
            finally
            {
                connection.Close();

                if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno) && newrec)
                    updatemedhist();

                if (newrec)
                {
                    string updatestring = "UPDATE LINK SET LINKOK = '1', DATEREC = '" + DateTime.Now.ToLongDateString() + "', TIMEREC = '" + DateTime.Now.ToLongTimeString() + "' WHERE reference = '" + vm.REPORTS.REPORT_TYPE2 + "' and linkok = '0' and tosection = '2' ";
                    bissclass.UpdateRecords(updatestring, "MR");
                    // LINK.updateLinkOk(txtcrossref.Text, "6", dttrans_date.Value.Date, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), false, true);
                }
            }

            //txtReference.Focus();
            return;
        }

        void updatemedhist()
        {
            MedHist.updatemedhistcomments(bchain.GROUPCODE, bchain.PATIENTNO, Convert.ToDateTime(vm.REPORTS.REPORT_TYPE1).Date, vm.REPORTS.edtspinstructions.Trim(), newrec, vm.REPORTS.REPORT_TYPE2, vm.REPORTS.TXTPATIENTNAME, bchain.GHGROUPCODE, bchain.GROUPHEAD, vm.REPORTS.doctor.Trim());
        }

        //private void txtReference_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtReference.Text))
        //        return;

        //    newrec = true;
        //    if (string.IsNullOrWhiteSpace(txtReference.Text) || combfacility.SelectedValue == null)
        //        return;

        //    if (Convert.ToInt32(txtReference.Text) > mlastno)
        //    {

        //        DialogResult result = MessageBox.Show("Service Reference is out of Seguence...");
        //        txtReference.Text = "";
        //        txtReference.Select();
        //        return;
        //    }

        //    this.txtReference.Text = bissclass.autonumconfig(this.txtReference.Text, true, "", "999999999");
        //    PHL01 phlebo = PHL01.GetPHL01(txtReference.Text, combfacility.SelectedValue.ToString());
        //    if (phlebo != null)
        //    {
        //        newrec = false;

        //        txtaddress.Text = phlebo.ADDRESS1;
        //        txtage.Text = phlebo.AGE.ToString();
        //        txtcrossref.Text = phlebo.CROSSREF;
        //        txtdefault.Text = phlebo.DEFAULTSTRING;
        //        txtfullname.Text = phlebo.NAME;

        //        txtgroupcode.Text = phlebo.GROUPCODE;
        //        txtothernotes.Text = phlebo.OTHERS;
        //        txtpatientno.Text = phlebo.PATIENTNO;
        //        //txtGroupheadName.Text = phlebo.
        //        txtreferringdoctor.Text = phlebo.DOCTOR;
        //        txtsamplecollectedBy.Text = phlebo.SAMPLEBY;
        //        combsex.Text = phlebo.SEX;
        //        dtcollectiondate.Value = phlebo.SAMPLEDATE;
        //        DialogResult result = MessageBox.Show("RECORD EXIST - Limited Update allowed!");
        //        btnsave.Enabled = true;
        //    }
        //}

        //private void txtReference_Enter(object sender, EventArgs e)
        //{
        //    clearcontrols();
        //    newrec = true;
        //    start_time = DateTime.Now.TimeOfDay.ToString();
        //    if (string.IsNullOrWhiteSpace(AnyCode)) //no lookup
        //    {
        //        mlastno = msmrfunc.getcontrol_lastnumber("XRAYNO", servicerecno, false, mlastno, false) + 1;
        //        txtReference.Text = mlastno.ToString();
        //    }
        //}

        //private void btnclose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void btnprint_Click(object sender, EventArgs e)
        //{
        //    if (combfacility.SelectedValue == null)
        //        return;

        //    SampleLabelGenerator samplelabel = new SampleLabelGenerator(mreference, servicetagged, dtfacility, combfacility.SelectedValue.ToString(), woperator, taggedFrmSuspensea_);
        //    samplelabel.ShowDialog();
        //}

        //private void txtothernotes_Click(object sender, EventArgs e)
        //{
        //    string xchecked = chkblood.Checked ? "Blood; " : chksputum.Checked ? "Sputum; " : chkstool.Checked ? "Stool; " : chkurine.Checked ? "Urine; " : chkswab.Checked ? "Swab; " : chksemen.Checked ? "Semen; " : chkhair.Checked ? "Hair; " : !string.IsNullOrWhiteSpace(txtothers.Text) ? "Others - (" + txtothers.Text.Trim() + ")" : "";
        //    txtothernotes.Text = "=> Sample Taken : " + xchecked + " - by " + txtsamplecollectedBy.Text.Trim() + "  @  " + dtcollectiondate.Value.ToLongDateString() + " " + dtcollectiondate.Value.ToShortTimeString();
        //    if (string.IsNullOrWhiteSpace(txtdefault.Text))
        //        txtdefault.Text = txtothernotes.Text;
        //    if (xchecked != "")
        //        btnsave.Enabled = true;
        //}

        //private void dttrans_date_KeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    if (objArgs.KeyCode == Keys.Enter)
        //        dtTrans_date_Leave(null, null);
        //}

        //private void txtReference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtReference_LostFocus(null, null);
        //}

        //private void txtcrossref_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtcrossref_Leave(null, null);
        //}
    }
}