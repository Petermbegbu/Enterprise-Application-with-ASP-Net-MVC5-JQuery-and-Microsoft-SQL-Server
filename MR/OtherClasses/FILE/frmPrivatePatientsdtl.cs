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
using mradmin;
using mradmin.Forms;
using mradmin.DataAccess;
using mradmin.BissClass;
using OtherClasses.Models;
//using Gizmox.WebGUI.Common.Interfaces;

//using Gizmox.WebGUI.Common;
//using Gizmox.WebGUI.Forms;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmPrivatePatientsdtl
    {
        MR_DATA.MR_DATAvm vm;

        public string errorProp { get; set; }

        patientinfo patients; 
        Customer customer;
        billchaindtl bchain;

        //PleaseWaitForm pleaseWait = new PleaseWaitForm();
        DataTable dtstate = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from statecodes order by name", true), dtcountry = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from countrycodes order by name", true), dtcustclass = Dataaccess.GetAnytable("", "MR", "SELECT reference, name from custclass order by name", true),
        dtdocs = Dataaccess.GetAnytable("", "MR", "select reference,name from doctors where rectype = 'D' order by name", true),
        dtreferrers = Dataaccess.GetAnytable("", "MR", "select custno, name from customer where referrer = '1' order by name", true);
        string PicSelected, mlocstate, mloccountry, mgrouphtype, woperator = "", categ_save, billcy_save, mregcode, mfacility, mstart_time;
        bool misreregall, autogenreg, mpauto, misrereg, misreregpvt, must_patphoto, autogenregforALL, mcanadd, mcandelete, mcanalter;
        // int mlastnumb = 1;  //, savedsurname, savedoname;
        decimal mduration, mlastno; // mlastnumb
        public bool newrec = true;
        string AnyCode = "", Anycode1 = "", lookupsource = "";

        //int mlastnumb = 1;
        //ContextMenu blah = new ContextMenu();


        public frmPrivatePatientsdtl(MR_DATA.MR_DATAvm VM2, string woperato)
        {
            vm = VM2;
            patients = new patientinfo();
            customer = new Customer();
            bchain = new billchaindtl();
            
            //InitializeComponent();
            woperator = woperato; //Session["operator"].ToString(); //====
            getcontrolsettings();
        }

        //private void frmPrivatePatientsdtl_Load(object sender, EventArgs e)
        //{
        //	this.txtgroupcode.ContextMenu = blah;
        //	initcomboboxes();
        //}

        void getcontrolsettings()
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select Loccountry, locstate, PAUTO, cashpoint, filemode, dactive, Last_no, installed, attendlink, regcode,dischtime,autogreg from mrcontrol order by recid", false); //msmrfunc.getcontrolsetup("MR");

            mloccountry = dt.Rows[0]["Loccountry"].ToString();
            mlocstate = dt.Rows[0]["locstate"].ToString();
            mpauto = (bool)dt.Rows[0]["PAUTO"];
            mregcode = dt.Rows[0]["regcode"].ToString();
            autogenreg = (bool)dt.Rows[0]["autogreg"];

            misrereg = (bool)dt.Rows[1]["cashpoint"];
            misreregpvt = (bool)dt.Rows[1]["filemode"];
            misreregall = (bool)dt.Rows[1]["dactive"];

            mduration = (Decimal)dt.Rows[2]["Last_no"];
            mfacility = dt.Rows[3]["dischtime"].ToString();
            must_patphoto = (bool)dt.Rows[4]["installed"];

            autogenregforALL = (bool)dt.Rows[5]["attendlink"];
            //if (!misrereg)
            //{
            //    //dtexpirydate.Visible = lblExpirydate.Visible = false;
            //    return true;
            //}
            dt = Dataaccess.GetAnytable("", "MR", "select CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];

            //return false;
        }

        //private void cmbclose_Click(object sender, EventArgs e)
        //{
        //   // pleaseWait.Close(); //  .Hide();
        //	this.Dispose();
        //	this.Close();
        //}

        //private void txtothername_LostFocus(object sender, EventArgs e)
        //{
        //    string xtitle = (string.IsNullOrWhiteSpace(cbotitle.Text)) ? "" : cbotitle.Text.Trim();
        //    this.TXTPATIENTNAME.Text = this.txtsurname.Text.Trim() + ", " + this.txtothername.Text.Trim() +
        //        " (" + xtitle + ")";
        //}

        //private void txtothername_TextChanged(object sender, EventArgs e)
        //{

        //}

        //private void txtgroupcode_Enter(object sender, EventArgs e)
        //{
        //    //  pleaseWait.Hide();
        //}

        //private void txtgroupcode_Validating(object sender, CancelEventArgs e)
        //{
        //    string gc = this.txtgroupcode.Text;
        //    if (!msmrfunc.checkGroupCode(gc)) //== false)
        //    {
        //        this.txtgroupcode.Focus();
        //    }
        //}

   //     private void txtpatientno_GotFocus(object sender, EventArgs e)
   //     {
   //         //we should get last patient number if auto
   //         //  if (mpauto)
   //         /*    if (string.IsNullOrWhiteSpace(AnyCode))
			//{
			//	if (mpauto)
			//	{
			//		mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 1);
			//		txtpatientno.Text = mlastno + 1.ToString();
			//	}
			//} */
   //     }

        //private void txtpatientno_Enter(object sender, EventArgs e)
        //{
        //    //we should get last patient number if auto
        //    //  pleaseWait.Hide();

        //    if (string.IsNullOrWhiteSpace(AnyCode) && mpauto) //no lookup
        //    {
        //        mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 2, false, mlastno, false);
        //        txtpatientno.Text = mlastno.ToString();
        //        txtpatientno.Focus();
        //    }
        //    categ_save = billcy_save = "";
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "G") //groupcodee
        //    {
        //        this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //        this.txtgroupcode.Focus();
        //    }

        //    else if (lookupsource == "P") //patientno
        //    {
        //        //this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode1;
        //        this.txtpatientno.Focus();
        //    }
        //    else if (lookupsource == "GHGC") //grouphead code
        //    {
        //        this.txtghgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtbillspayable.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //        this.txtghgroupcode.Focus();
        //    }
        //    else
        //    {
        //        //this.txtghgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtbillspayable.Text = msmrfunc.mrGlobals.anycode;
        //        this.txtbillspayable.Focus();
        //    }
        //}

        //public MR_DATA.MR_DATAvm txtpatientno_LostFocus(string patientNo, string groupCode, bool getAcctInfo)
        //{
        //    vm.REPORTS = new MR_DATA.REPORTS();
        //    mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 2, false, mlastno, false); //New addition

        //    //ClearControls();
        //    //if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    //    return;
        //    // cmbsave.Enabled = cmbdelete.Enabled = cmdgrpmember.Enabled = false;
        //    vm.REPORTS.newrec = true;
        //    mstart_time = DateTime.Now.ToLongTimeString();
        //    PicSelected = "";
        //    //DialogResult result;

        //    if (string.IsNullOrWhiteSpace(AnyCode) && mpauto) //no lookup value obtained
        //    {
        //        //if (patientNo.Trim() != "MISC" && !bissclass.IsInt32(patientNo, "Patient Number"))
        //        //{
        //        //    //txtpatientno.Text = "";
        //        //    //txtgroupcode.Focus();

        //        //    vm.REPORTS.ActRslt = "";

        //        //    return vm;
        //        //}

        //        if (patientNo.Trim() != "MISC" && bissclass.IsDigitsOnly(patientNo.Trim()) && Convert.ToDecimal(patientNo) > mlastno)
        //        {
        //            vm.REPORTS.ActRslt = "Patient Number is out of Sequence...";

        //            //txtpatientno.Text = "";
        //            // txtgroupcode.Focus();
        //            return vm;
        //        }

        //        if (patientNo.Trim() != "MISC")
        //        {
        //            patientNo = bissclass.autonumconfig(patientNo, true, "", "9999999");
        //        }

        //    }

        //    //check if patientno exists
        //    patients = patientinfo.GetPatient(patientNo, groupCode);
        //    if (patients == null) //new defintion
        //    {
        //        //CHECK BILL CHAIN
        //        DataTable dt = Dataaccess.GetAnytable("", "MR", "select name, patientno, groupcode from billchain where patientno = '" + patientNo + "'", false);
        //        if (dt.Rows.Count > 0)
        //        {
        //            vm.REPORTS.ActRslt = "This Reference is already used for " + dt.Rows[0]["name"].ToString().Trim() + " GroupCode : " + dt.Rows[0]["groupcode"].ToString().Trim() + "Patient Number :" + patientNo;

        //            AnyCode = Anycode1 = patientNo = "";
        //            return vm;
        //        }

        //        //bchain = billchaindtl.Getbillchain(this.txtpatientno.Text, txtgroupcode.Text);
        //        //if (bchain != null)
        //        //{
        //        //    result = MessageBox.Show("This Reference is already used for " + bchain.NAME.Trim() + " GroupCode : " + bchain.GROUPCODE, "Patient Number :" + txtpatientno.Text);
        //        //    AnyCode = Anycode1 = txtpatientno.Text ="";
        //        //    txtpatientno.Select();
        //        //    return;
        //        //}

        //        //bissclass.displaycombo(cbopistate, dtstate, mlocstate, "type_code");
        //        //bissclass.displaycombo(cbonationality, dtcountry, mloccountry, "type_code");

        //        //  bissclass.sysGlobals.waitwindowtext = "NEW PATIENT REGISTRATION";
        //        //   this.cmbsave.Enabled = msmrfunc.mrGlobals.mcanadd ? true : false;
        //        // Display form modelessly
        //        //  pleaseWait.Show();
        //        //ShowWaitingBox waiting = new ShowWaitingBox("Patient Regisration","NEW GROUPHEAD/PRIVATE CASH RECORDS...");
        //        //waiting.Start();
        //        //do something that takes a while
        //        //waiting.Stop();
        //        //DialogResult result = MessageBox.Show("New Patient Records ?" + " Confirm to Add...", "New Patient Records", MessageBoxButtons.YesNo,
        //        //  MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, msgBoxHandler);
        //    }
        //    else
        //    {
        //        DisplayPatients(getAcctInfo, patients);
        //        bchain = billchaindtl.Getbillchain(patientNo, groupCode);
        //        if (bchain != null)
        //        {
        //            vm.REPORTS.edtspinstructions = bchain.SPNOTES;
        //            vm.REPORTS.edtallergies = bchain.MEDNOTES;
        //        }

        //        vm.REPORTS.btnchainedmedhistory = true;
        //        vm.REPORTS.btnFamilyGroup = true;

        //        //btnchainedmedhistory.Enabled = btnFamilyGroup.Enabled = true;

        //        if (patients.posted == false)
        //        {
        //            vm.REPORTS.cmbdelete = mcandelete ? true : false;
        //        }

        //        if (patients.isgrouphead == true)
        //        {
        //            vm.REPORTS.cmdgrpmember = true;
        //            vm.REPORTS.chkbillregistration = true;

        //            //this.cmdgrpmember.Enabled = true;
        //            //this.chkbillregistration.Visible = true;
        //        }

        //        //this.cmbsave.Enabled = msmrfunc.GlobalAccessCheck("M");
        //        //this.cmbsave.Enabled = msmrfunc.mrGlobals.mcanalter ? true : false;
        //        vm.REPORTS.newrec = false;
        //    }

        //    // this.tabPage1.Focus();
        //    AnyCode = Anycode1 = "";

        //    vm.REPORTS.cmbsave = true;
        //    //cmbsave.Enabled = true;

        //    //waiting.Stop();
        //    //this.cbotype.Select();

        //    return vm;
        //}

        //private void btngroupcode_Click_1(object sender, EventArgs e)
        //{
        //    this.txtgroupcode.Text = "";
        //    lookupsource = "G";
        //    msmrfunc.mrGlobals.crequired = "p";
        //    msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //    msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //private void lookupbtn_Click(object sender, EventArgs e)
        //{
        //    txtpatientno.Text = "";
        //    lookupsource = "P";
        //    msmrfunc.mrGlobals.crequired = "P";
        //    msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //    msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //private void DisplayPatients(bool getAcctInfo, patientinfo patients)
        //{
            
        //    vm.REPORTS = new MR_DATA.REPORTS();

        //    //bissclass.sysGlobals.waitwindowtext = "RECORD EXIST ...";
        //    // Display form modelessly
        //    //pleaseWait.Show();

        //    //change comboboxstyle to allow display of field value
        //    //this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown);

        //    vm.REPORTS.txtgroupcode = patients.groupcode;
        //    vm.REPORTS.txtpatientno = patients.patientno;

        //    //bissclass.displaycombo(cbopistate, dtstate, patients.patstate, "type_code");######

        //    //this.commaritalstatus.Text = patients.patstatus;
        //    vm.REPORTS.txtaddress1 = patients.address1;
        //    //  this.txtaddress2.Text = patients.address2;
        //    vm.REPORTS.dtbirthdate = patients.birthdate;
        //    vm.REPORTS.cbogender = patients.sex;
        //    vm.REPORTS.cbomaritalstatus = patients.m_status;
        //    vm.REPORTS.dtregistered = patients.reg_date;
        //    vm.REPORTS.txtcontactperson = patients.contact;
        //    vm.REPORTS.txtghgroupcode = patients.ghgroupcode;
        //    vm.REPORTS.cbotype = patients.pattype == "P" ? "Private" : patients.pattype == "F" ? "Family" : patients.pattype == "C" ? "Corporate" : "";

        //    vm.REPORTS.txtcreditlimit = patients.cr_limit.ToString();
        //    //vm.REPORTS.combillcycle = (patients.bill_cir == "C") ? "CASH" : (patients.bill_cir == "M") ? "Monlthy" :
        //    //    (patients.bill_cir == "Q") ? "Quartely" : (patients.bill_cir == "H") ? "H>On Hold" : "S> On Payment";

        //    //if (!patients.posted)
        //    //{
        //    //    this.nmrcurdebit.Value = patients.balbf >= 1 ? patients.balbf : 0m;
        //    //    this.nmrcurcredit.Value = patients.balbf < 1 ? Math.Abs(patients.balbf) : 0m;
        //    //}
        //    vm.REPORTS.nmrBalbf = patients.balbf;
        //    vm.REPORTS.lblBalbfDbCr = patients.balbf < 1 ? "CR" : "DB";
        //    //this.nmrcurdebit.Value = patients.balbf; // patients.cur_db;
        //    //this.nmrcurcredit.Value = patients.cur_cr;
        //    // this.nmrbalance.Value = patients.balbf;
        //    //patients.trans_date = (DateTime)reader["trans_date"];
        //    //patients.posted = (bool)reader["posted"];
        //    //patients.post_date = (DateTime)reader["post_date"];
        //    vm.REPORTS.TXTPATIENTNAME = patients.name;
        //    vm.REPORTS.txtbillspayable = patients.grouphead;
        //    //patients.grouphtype = reader["grouphtype"].ToString();
        //    //patients.isgrouphead = (bool)reader["isgrouphead"];
        //    //patients.laststatmt = (DateTime)reader["laststatmt"];
        //    vm.REPORTS.cbotitle = patients.title;
        //    vm.REPORTS.Combillspayable = (patients.grouphead == patients.patientno) ? "SELF" :
        //        (patients.grouphtype == "P") ? "P\\ANOTHER PATIENT" : "CORPORATE CLIENT";
        //    //  this.combillcategory.Text =  patients.patcateg;

        //    //bissclass.displaycombo(combillcategory, dtcustclass, patients.patcateg, "name");#######

        //    //this.patients.remark = reader["remark"].ToString();
        //    vm.REPORTS.txtdiscount = patients.discount;
        //    vm.REPORTS.comhmoservgrp = patients.hmoservtype;
        //    vm.REPORTS.txtcurrency = patients.currency;
        //    vm.REPORTS.chkbillregistration = (patients.billregistration == true) ? true : false;
        //    vm.REPORTS.txtclinic = patients.clinic;
        //    vm.REPORTS.txtsurname = patients.surname;
        //    vm.REPORTS.txtothername = patients.othername;
        //    vm.REPORTS.txthomephone = patients.homephone;
        //    vm.REPORTS.txtworkphone = patients.workphone;
        //    vm.REPORTS.txtemployer = patients.employer;
        //    vm.REPORTS.txtemployer = patients.emp_name;
        //    vm.REPORTS.txtemployeraddress = patients.emp_addr;
        //    vm.REPORTS.cboemploystate = patients.emp_state;
        //    //this.patients.cont_designation = reader["cont_designation"].ToString();
        //    //bissclass.displaycombo(cboprimarydoc, dtdocs, patients.pr_doc, "reference");########
        //    //this.cboreferringdoc.Text = patients.refer_dr;
        //    //bissclass.displaycombo(cboreferringdoc, dtreferrers, patients.refer_dr, "custno");########
        //    //this.cbonationality.Text = patients.nationality;
        //    //bissclass.displaycombo(cbonationality, dtcountry, patients.nationality, "name");########
        //    vm.REPORTS.cbooccupation = patients.occupation;
        //    //patients.religion = reader["religion"].ToString();
        //    vm.REPORTS.cbobloodgroup = patients.bloodgroup;
        //    vm.REPORTS.cbogenotype = patients.genotype;
        //    vm.REPORTS.txtnextofkin = patients.nextofkin;
        //    vm.REPORTS.txtkinaddress1 = patients.nok_adr1;
        //    vm.REPORTS.cbokinstate = patients.nok_state;
        //    vm.REPORTS.txtkinphone = patients.nok_phone;
        //    vm.REPORTS.txtrelationship = patients.nok_relationship;
        //    //patients.rhd = reader["rhd"].ToString();
        //    vm.REPORTS.txtemail = patients.email;
        //    mgrouphtype = patients.grouphtype;
        //    PicSelected = patients.piclocation;
        //    vm.REPORTS.categ_save = patients.patcateg;
        //    vm.REPORTS.combillcycle = patients.bill_cir;
        //    vm.REPORTS.cboReligion = patients.religion == "C" ? "CHRISTIANITY" : patients.religion == "M" ? "MUSLIM" : "OTHERS";
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select lga, tribe FROM PATDETAIL where groupcode = '" + patients.groupcode + "' and patientno = '" + patients.patientno + "'", false);
        //    if (dt.Rows.Count > 0)
        //    {
        //        vm.REPORTS.cboTribe = dt.Rows[0]["tribe"].ToString();
        //        vm.REPORTS.cboLGA = dt.Rows[0]["lga"].ToString();
        //    }

        //    //if (!string.IsNullOrWhiteSpace(PicSelected))
        //    //{
        //    //    pictureBox1.Image = WebGUIGatway.getpicture(PicSelected);
        //    //}

        //    //revert to its original format
        //    //this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList);

        //    if (getAcctInfo)
        //    {
        //        GetAcctInfo(patients);
        //    }

        //    vm.REPORTS.ActRslt = "RECORD EXIST ...";
        //}

        //void GetAcctInfo(patientinfo patients)
        //{
           
        //    decimal db, cr, adj; db = cr = adj = 0m;
        //    vm.REPORTS.nmrBalbf = msmrfunc.getOpeningBalance(patients.groupcode, patients.patientno, "", "P", DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);
        //    vm.REPORTS.nmrcurcredit = cr;
        //    if (patients.balbf > 0)
        //        vm.REPORTS.nmrcurdebit = db - patients.balbf;
        //    else if (cr > 0)
        //        vm.REPORTS.nmrcurcredit = cr - Math.Abs(patients.balbf);
        //    //  nmrCurDebit.Value = db;
        //    vm.REPORTS.nmrBalbf = patients.balbf;
        //    vm.REPORTS.lblBalbfDbCr = patients.balbf < 1 ? "CR" : "DB";
        //    vm.REPORTS.nmrbalance = vm.REPORTS.nmrBalbf + vm.REPORTS.nmrcurdebit - vm.REPORTS.nmrcurcredit;
        //    vm.REPORTS.lblDbCr = vm.REPORTS.nmrbalance < 1 ? "CR" : "DB";

        //    if (patients.posted)
        //    {
        //        vm.REPORTS.nmrBalbfReadOnly = true;
        //    }

        //}

        #region
        void savepatientdetails()
        {
            //  patientinfo patients = new patientinfo();
            patients = patientinfo.GetPatient(vm.REPORTS.txtpatientno.Trim(), vm.REPORTS.txtgroupcode.Trim());

            newrec = patients == null ? true : false;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "Patient_Add" : "Patient_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", vm.REPORTS.txtpatientno.Trim()); //patients.patientno);
            insertCommand.Parameters.AddWithValue("@groupcode", vm.REPORTS.txtgroupcode.Trim()); // patients.groupcode);
            insertCommand.Parameters.AddWithValue("@patstatus", newrec ? "A" : patients.patstatus);  //patients.patstatus);
            insertCommand.Parameters.AddWithValue("@Address1", vm.REPORTS.txtaddress1); //patients.address1);
            insertCommand.Parameters.AddWithValue("@Patstate", string.IsNullOrWhiteSpace(vm.SYSCODETABSvm.StateCodes.name) ? "" : vm.SYSCODETABSvm.StateCodes.name.ToString()); //patients.patstate);
            insertCommand.Parameters.AddWithValue("@Birthdate", vm.REPORTS.dtbirthdate); //patients.birthdate);
            insertCommand.Parameters.AddWithValue("@Sex", vm.REPORTS.cbogender); //patients.sex);
            insertCommand.Parameters.AddWithValue("@M_status", vm.REPORTS.cbomaritalstatus == null ? "" : vm.REPORTS.cbomaritalstatus.ToString()); //patients.m_status);
            insertCommand.Parameters.AddWithValue("@Reg_date", vm.REPORTS.dtregistered); //patients.reg_date);
            insertCommand.Parameters.AddWithValue("@contact", vm.REPORTS.txtcontactperson); //patients.contact);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", patients.ghgroupcode); //patients.ghgroupcode); vm.REPORTS.txtghgroupcode
            insertCommand.Parameters.AddWithValue("@pattype", vm.REPORTS.cbotype.ToString());  //patients.pattype);
            insertCommand.Parameters.AddWithValue("@cr_limit", vm.REPORTS.txtcreditlimit); //patients.cr_limit);
            insertCommand.Parameters.AddWithValue("@bill_cir", vm.REPORTS.combillcycle);
            insertCommand.Parameters.AddWithValue("@cur_db", vm.REPORTS.nmrcurdebit); //patients.cur_db);
            insertCommand.Parameters.AddWithValue("@cur_cr", vm.REPORTS.nmrcurcredit); //patients.cur_cr);
            insertCommand.Parameters.AddWithValue("@balbf", vm.REPORTS.nmrBalbf);
            insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now); //patients.trans_date);
            insertCommand.Parameters.AddWithValue("@name", vm.REPORTS.TXTPATIENTNAME.Trim()); //patients.name);
            insertCommand.Parameters.AddWithValue("@grouphead", patients.grouphead); //patients.grouphead); vm.REPORTS.txtbillspayable == null ? "" : vm.REPORTS.txtbillspayable
            insertCommand.Parameters.AddWithValue("@grouphtype", (mgrouphtype == "S") ? "P" : mgrouphtype); //  patients.grouphtype);
            insertCommand.Parameters.AddWithValue("@isgrouphead", (vm.REPORTS.txtpatientno == patients.grouphead) ? true : false);
            insertCommand.Parameters.AddWithValue("@title", vm.REPORTS.cbotitle); //patients.title);
            insertCommand.Parameters.AddWithValue("@patcateg", vm.CUSTCLASS.NAME.ToString()); //patients.patcateg);
            //insertCommand.Parameters.AddWithValue("@remark", edtcomments.Text.Trim());
            insertCommand.Parameters.AddWithValue("@discount", vm.REPORTS.txtdiscount); //patients.discount);
            insertCommand.Parameters.AddWithValue("@hmoservtype", vm.REPORTS.comhmoservgrp == null ? "" : vm.REPORTS.comhmoservgrp.ToString()); //patients.hmoservtype);
            insertCommand.Parameters.AddWithValue("@currency", vm.SYSCODETABSvm.CurrencyCodes.name); //patients.currency);
            insertCommand.Parameters.AddWithValue("@billregistration", (vm.REPORTS.chkbillregistration) ? true : false);
            insertCommand.Parameters.AddWithValue("@clinic", vm.SYSCODETABSvm.ServiceCentreCodes.name); //patients.clinic);
            insertCommand.Parameters.AddWithValue("@surname", vm.REPORTS.txtsurname.Trim()); //patients.surname);
            insertCommand.Parameters.AddWithValue("@othername", vm.REPORTS.txtothername); //patients.othername);
            insertCommand.Parameters.AddWithValue("@homephone", vm.REPORTS.txthomephone); //patients.homephone);
            insertCommand.Parameters.AddWithValue("@workphone", vm.REPORTS.txtworkphone); //patients.workphone);
            insertCommand.Parameters.AddWithValue("@employer", vm.REPORTS.txtemployer); //patients.employer);
            insertCommand.Parameters.AddWithValue("@emp_name", vm.REPORTS.txtemployer); // patients.emp_name);
            insertCommand.Parameters.AddWithValue("@emp_addr", vm.REPORTS.txtemployeraddress); //patients.emp_addr);
            insertCommand.Parameters.AddWithValue("@emp_state", string.IsNullOrWhiteSpace(vm.REPORTS.cboemploystate) ? "" : vm.REPORTS.cboemploystate.ToString()); //patients.emp_state)#######;
            insertCommand.Parameters.AddWithValue("@cont_designation", string.IsNullOrWhiteSpace(vm.SYSCODETABSvm.DesignationCodes.name) ? "" : vm.SYSCODETABSvm.DesignationCodes.name.ToString());
            insertCommand.Parameters.AddWithValue("@pr_doc", string.IsNullOrWhiteSpace(vm.DOCTORS.NAME) ? "" : vm.DOCTORS.NAME.ToString());
            insertCommand.Parameters.AddWithValue("@refer_dr", string.IsNullOrWhiteSpace(vm.CUSTOMER.NAME) ? "" : vm.CUSTOMER.NAME.ToString());//for cboreferringdoc
            insertCommand.Parameters.AddWithValue("@nationality", string.IsNullOrWhiteSpace(vm.SYSCODETABSvm.CountryCodes.name) ? "" : vm.SYSCODETABSvm.CountryCodes.name.ToString());
            insertCommand.Parameters.AddWithValue("@occupation", string.IsNullOrWhiteSpace(vm.SYSCODETABSvm.DesignationCodes.name) ? "" : vm.SYSCODETABSvm.DesignationCodes.name.ToString());
            insertCommand.Parameters.AddWithValue("@bloodgroup", vm.REPORTS.cbobloodgroup); //patients.bloodgroup);
            insertCommand.Parameters.AddWithValue("@genotype", vm.REPORTS.cbogenotype == null ? "" : vm.REPORTS.cbogenotype.ToString()); //patients.genotype);
            insertCommand.Parameters.AddWithValue("@nextofkin", vm.REPORTS.txtnextofkin); //patients.nextofkin);
            insertCommand.Parameters.AddWithValue("@nok_adr1", vm.REPORTS.txtkinaddress1); //patients.nok_adr1);
            insertCommand.Parameters.AddWithValue("@nok_state", string.IsNullOrWhiteSpace(vm.REPORTS.cbokinstate) ? "" : vm.REPORTS.cbokinstate.ToString());//#########
            insertCommand.Parameters.AddWithValue("@nok_phone", vm.REPORTS.txtkinphone); //patients.nok_phone);
            insertCommand.Parameters.AddWithValue("@nok_relationship", vm.REPORTS.txtrelationship); //patients.nok_relationship);
            insertCommand.Parameters.AddWithValue("@email", vm.REPORTS.txtemail); //patients.email);
            insertCommand.Parameters.AddWithValue("@piclocation", PicSelected); //patients.email);

            insertCommand.Parameters.AddWithValue("@UPCUR_DB", (newrec || !patients.posted) ? 0 : patients.upcur_db);
            insertCommand.Parameters.AddWithValue("@UPCUR_CR", (newrec || !patients.posted) ? 0 : patients.upcur_cr);
            insertCommand.Parameters.AddWithValue("@DEBIT1", (newrec || !patients.posted) ? 0 : patients.debit1);
            insertCommand.Parameters.AddWithValue("@CREDIT1", (newrec || !patients.posted) ? 0 : patients.credit1);
            insertCommand.Parameters.AddWithValue("@BALBF1", (newrec || !patients.posted) ? 0 : patients.balbf1);
            insertCommand.Parameters.AddWithValue("@DEBIT2", (newrec || !patients.posted) ? 0 : patients.debit2);
            insertCommand.Parameters.AddWithValue("@CREDIT2", (newrec || !patients.posted) ? 0 : patients.credit2);
            insertCommand.Parameters.AddWithValue("@BALBF2", (newrec || !patients.posted) ? 0 : patients.balbf2);
            insertCommand.Parameters.AddWithValue("@DEBIT3", (newrec || !patients.posted) ? 0 : patients.debit3);
            insertCommand.Parameters.AddWithValue("@CREDIT3", (newrec || !patients.posted) ? 0 : patients.credit3);
            insertCommand.Parameters.AddWithValue("@BALBF3", (newrec || !patients.posted) ? 0 : patients.balbf3);
            insertCommand.Parameters.AddWithValue("@DEBIT4", (newrec || !patients.posted) ? 0 : patients.debit4);
            insertCommand.Parameters.AddWithValue("@CREDIT4", (newrec || !patients.posted) ? 0 : patients.credit4);
            insertCommand.Parameters.AddWithValue("@BALBF4", (newrec || !patients.posted) ? 0 : patients.balbf4);
            insertCommand.Parameters.AddWithValue("@DEBIT5", (newrec || !patients.posted) ? 0 : patients.debit5);
            insertCommand.Parameters.AddWithValue("@CREDIT5", (newrec || !patients.posted) ? 0 : patients.credit5);
            insertCommand.Parameters.AddWithValue("@BALBF5", (newrec || !patients.posted) ? 0 : patients.balbf5);
            insertCommand.Parameters.AddWithValue("@DEBIT6", (newrec || !patients.posted) ? 0 : patients.debit6);
            insertCommand.Parameters.AddWithValue("@CREDIT6", (newrec || !patients.posted) ? 0 : patients.credit6);
            insertCommand.Parameters.AddWithValue("@BALBF6", (newrec || !patients.posted) ? 0 : patients.balbf6);
            insertCommand.Parameters.AddWithValue("@DEBIT7", (newrec || !patients.posted) ? 0 : patients.debit7);
            insertCommand.Parameters.AddWithValue("@CREDIT7", (newrec || !patients.posted) ? 0 : patients.credit7);
            insertCommand.Parameters.AddWithValue("@BALBF7", (newrec || !patients.posted) ? 0 : patients.balbf7);
            insertCommand.Parameters.AddWithValue("@DEBIT8", (newrec || !patients.posted) ? 0 : patients.debit8);
            insertCommand.Parameters.AddWithValue("@CREDIT8", (newrec || !patients.posted) ? 0 : patients.credit8);
            insertCommand.Parameters.AddWithValue("@BALBF8", (newrec || !patients.posted) ? 0 : patients.balbf8);
            insertCommand.Parameters.AddWithValue("@DEBIT9", (newrec || !patients.posted) ? 0 : patients.debit9);
            insertCommand.Parameters.AddWithValue("@CREDIT9", (newrec || !patients.posted) ? 0 : patients.credit9);
            insertCommand.Parameters.AddWithValue("@BALBF9", (newrec || !patients.posted) ? 0 : patients.balbf9);
            insertCommand.Parameters.AddWithValue("@DEBIT10", (newrec || !patients.posted) ? 0 : patients.debit10);
            insertCommand.Parameters.AddWithValue("@CREDIT10", (newrec || !patients.posted) ? 0 : patients.credit10);
            insertCommand.Parameters.AddWithValue("@BALBF10", (newrec || !patients.posted) ? 0 : patients.balbf10);
            insertCommand.Parameters.AddWithValue("@DEBIT11", (newrec || !patients.posted) ? 0 : patients.debit11);
            insertCommand.Parameters.AddWithValue("@CREDIT11", (newrec || !patients.posted) ? 0 : patients.credit11);
            insertCommand.Parameters.AddWithValue("@BALBF11", (newrec || !patients.posted) ? 0 : patients.balbf11);
            insertCommand.Parameters.AddWithValue("@DEBIT12", (newrec || !patients.posted) ? 0 : patients.debit12);
            insertCommand.Parameters.AddWithValue("@CREDIT12", (newrec || !patients.posted) ? 0 : patients.credit12);
            insertCommand.Parameters.AddWithValue("@BALBF12", (newrec || !patients.posted) ? 0 : patients.balbf12);
            insertCommand.Parameters.AddWithValue("@SEC_LEVEL", 0);
            insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@LASTSTATMT", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@RELIGION", string.IsNullOrWhiteSpace(vm.REPORTS.cboReligion) ? "" : vm.REPORTS.cboReligion.Substring(0, 1));
            insertCommand.Parameters.AddWithValue("@RHD", " ");
            insertCommand.Parameters.AddWithValue("@POSTED", newrec ? false : patients.posted);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                errorProp = "Unable to Open SQL Server Database Table " + ex.Message ;
                //MessageBox.Show("Unable to Open SQL Server Database Table" + ex, "Add Patient Details", MessageBoxButtons.OK,
                //    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
            if (newrec && autogenreg && vm.REPORTS.dtregistered == DateTime.Now.Date) //Current Reg. generate registration fee
            {
                //unflag mautogreg at initial implimentation registration
                //IF patient.isgrouphea OR mgenregistration
                if (vm.REPORTS.txtpatientno.Trim() == vm.REPORTS.txtbillspayable.Trim() || autogenregforALL)
                    GenerateRegistrationFee();
            }
            if (!newrec && vm.CUSTCLASS.NAME != patients.patcateg.Trim()) //Adjusting GroupMembers BIlling Category..
            {
                // string xcat = combillcategory.SelectedItem.ToString().Substring(0, 1);
                string updatestring = "update billchain set patcateg = '" + vm.CUSTCLASS.NAME.ToString() + "' where ghgroupcode = '" + vm.REPORTS.txtgroupcode.Trim() + "' and grouphead = '" + vm.REPORTS.txtpatientno.Trim() + "'";
                bissclass.UpdateRecords(updatestring, "MR");
            }
            if (newrec && vm.REPORTS.dtregistered == DateTime.Now.Date)
                LINK3.WriteLINK3(vm.REPORTS.txtgroupcode.Trim(), vm.REPORTS.txtpatientno.Trim(), DateTime.Now, vm.REPORTS.TXTPATIENTNAME, "REGISTRATION", "", DateTime.Now.ToLongTimeString(), mstart_time, "1", mfacility, mstart_time, woperator);
        }

        void savebillchainetails()
        {
            bchain = billchaindtl.Getbillchain(vm.REPORTS.txtpatientno.Trim(), vm.REPORTS.txtgroupcode.Trim());

            //UPDATE BILLCHAIN
            //billchaindtl bchain = new billchaindtl();

            string xphone = "";
            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txthomephone))
                xphone = vm.REPORTS.txthomephone;
            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtworkphone))
            {
                xphone = string.IsNullOrWhiteSpace(xphone) ? "" : ", ";
                xphone += vm.REPORTS.txtworkphone;
            }


            newrec = (bchain == null || bchain.NAME == null) ? true : false;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "Billchain_Add" : "Billchain_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", vm.REPORTS.txtpatientno.Trim());
            insertCommand.Parameters.AddWithValue("@groupcode", vm.REPORTS.txtgroupcode.Trim());
            insertCommand.Parameters.AddWithValue("@status", (newrec) ? "A" : bchain.STATUS);
            insertCommand.Parameters.AddWithValue("@Birthdate", vm.REPORTS.dtbirthdate);
            insertCommand.Parameters.AddWithValue("@Sex", vm.REPORTS.cbogender == null ? "" : vm.REPORTS.cbogender.ToString());
            insertCommand.Parameters.AddWithValue("@M_status", vm.REPORTS.cbomaritalstatus == null ? "" : vm.REPORTS.cbomaritalstatus.ToString().Substring(0, 1));
            insertCommand.Parameters.AddWithValue("@Reg_date", vm.REPORTS.dtregistered);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", vm.REPORTS.txtghgroupcode);
            insertCommand.Parameters.AddWithValue("@phone", xphone);
            insertCommand.Parameters.AddWithValue("@name", vm.REPORTS.TXTPATIENTNAME.Trim());
            insertCommand.Parameters.AddWithValue("@grouphead", vm.REPORTS.txtbillspayable == null? "" : vm.REPORTS.txtbillspayable);
            insertCommand.Parameters.AddWithValue("@grouphtype", (mgrouphtype == "S") ? "P" : mgrouphtype);
            insertCommand.Parameters.AddWithValue("@patcateg", vm.CUSTCLASS.NAME.ToString());
            insertCommand.Parameters.AddWithValue("@hmoservtype", vm.REPORTS.comhmoservgrp == null ? "" : vm.REPORTS.comhmoservgrp.ToString());
            insertCommand.Parameters.AddWithValue("@currency", vm.SYSCODETABSvm.CurrencyCodes.name);
            insertCommand.Parameters.AddWithValue("@residence", vm.REPORTS.txtaddress1);
            insertCommand.Parameters.AddWithValue("@clinic", vm.SYSCODETABSvm.ServiceCentreCodes.name);
            insertCommand.Parameters.AddWithValue("@email", vm.REPORTS.txtemail);
            insertCommand.Parameters.AddWithValue("@operator", woperator);
            insertCommand.Parameters.AddWithValue("@dtime", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@hmocode", vm.REPORTS.comhmoservgrp);
            insertCommand.Parameters.AddWithValue("@expirydate", (newrec) ? DateTime.Now : bchain.EXPIRYDATE);

            insertCommand.Parameters.AddWithValue("@posted", (newrec) ? false : bchain.POSTED);
            insertCommand.Parameters.AddWithValue("@post_date", (newrec) ? DateTime.Now : bchain.POST_DATE);
            insertCommand.Parameters.AddWithValue("@staffno", (newrec) ? " " : bchain.STAFFNO);
            insertCommand.Parameters.AddWithValue("@relationsh", (newrec) ? " " : bchain.RELATIONSH);
            insertCommand.Parameters.AddWithValue("@section", (newrec) ? " " : bchain.SECTION);
            insertCommand.Parameters.AddWithValue("@department", (newrec) ? " " : bchain.DEPARTMENT);
            insertCommand.Parameters.AddWithValue("@cur_db", (newrec) ? 0 : bchain.CUR_DB);
            insertCommand.Parameters.AddWithValue("@cumvisits", (newrec) ? 0 : bchain.CUMVISITS);
            insertCommand.Parameters.AddWithValue("@billonacct", (newrec) ? "" : bchain.BILLONACCT);
            insertCommand.Parameters.AddWithValue("@astnotes", (newrec) ? false : bchain.ASTNOTES);
            insertCommand.Parameters.AddWithValue("@piclocation", PicSelected);
            insertCommand.Parameters.AddWithValue("@surname", vm.REPORTS.txtsurname.Trim());
            insertCommand.Parameters.AddWithValue("@othernames", vm.REPORTS.txtothername);
            insertCommand.Parameters.AddWithValue("@title", vm.REPORTS.cbotitle == null ? "" : vm.REPORTS.cbotitle.ToString());
            insertCommand.Parameters.AddWithValue("@SPNOTES", vm.REPORTS.edtspinstructions);
            insertCommand.Parameters.AddWithValue("@MEDNOTES", vm.REPORTS.edtallergies);
            insertCommand.Parameters.AddWithValue("@MEDHISTORYCHAINED", newrec ? false : bchain.MEDHISTORYCHAINED);
            insertCommand.Parameters.AddWithValue("PATIENTNO_PRINCIPAL", "");

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                //MessageBox.Show("SQL access" + ex, "BILLCHAIN UPDATE", MessageBoxButtons.OK,
                //    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                errorProp = "SQL access " + ex.Message;
                return;
            }
            finally
            {
                connection.Close();
            }
        }

        void GenerateRegistrationFee()
        {
            //get current alternate billing reference
            decimal xno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, true, 0, false);
            string billref = bissclass.autonumconfig(xno.ToString(), true, "", "999999999");
            //get tariff desc and amount
            decimal amount = 0m, amtsave = 0;
            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT name, amount, diffcharge from tariff where rtrim(reference) = '" + mregcode.Trim() + "'", false);
            if (dt.Rows.Count < 1)
                return;
            amount = amtsave = (decimal)dt.Rows[0]["amount"];
            if ((bool)dt.Rows[0]["diffcharge"])
                amount = msmrfunc.gettardiffcalc(mregcode, 0, amount, vm.REPORTS.combillcycle.Substring(0, 1));
            if (amount == 0)
                amount = amtsave;
            //write details
            Billings.writeBILLS(true, billref, 1m, mregcode, dt.Rows[0]["name"].ToString(), mgrouphtype == "S" ? "P" : mgrouphtype, amount, DateTime.Now.Date, vm.REPORTS.TXTPATIENTNAME, vm.REPORTS.txtbillspayable, mfacility, vm.REPORTS.txtgroupcode.Trim(), vm.REPORTS.txtpatientno.Trim(), "D", vm.REPORTS.txtghgroupcode, woperator, DateTime.Now, "", "", 0m, 0, "", "", false, "", "", 0m, "", 0m, "O", false, 0);
        }
        #endregion

        //public void ClearControls()
        //{
        //    this.txtsurname.Text = this.edtallergies.Text = this.edtcomments.Text = edtspinstructions.Text = this.comhmoservgrp.Text = this.txtothername.Text = this.TXTPATIENTNAME.Text = txtbillspayable.Text = this.txtghgroupcode.Text = this.txtclinic.Text = "";
        //    this.nmrcurdebit.Value = this.nmrbalance.Value = this.nmrcurcredit.Value = 0;
        //    pictureBox1.Image = null;
        //    txtaddress1.Text = txtcontactperson.Text = txtcurrency.Text = txtemployer.Text = txtemployeraddress.Text = cboemploystate.Text = txthomephone.Text = txtkinphone.Text = cbokinstate.Text = txtnextofkin.Text = cbooccupation.Text = txtothername.Text = cboprimarydoc.Text = cboreferringdoc.Text = txtrelationship.Text = txtworkphone.Text = txtworkphone.Text = cbobloodgroup.Text = cbopistate.Text = cbogenotype.Text = txtemail.Text = txtkinaddress1.Text = lblbillspayable.Text = "";
        //    dtbirthdate.Value = dtregistered.Value = DateTime.Now.Date;
        //    combillcategory.SelectedIndex = combillcycle.SelectedIndex = cbogender.SelectedIndex = cbotitle.SelectedIndex = cbotype.SelectedIndex = cbomaritalstatus.SelectedIndex = this.Combillspayable.SelectedIndex = cbonationality.SelectedIndex = -1;

        //}

        //private void txtgroupcode_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == Gizmox.WebGUI.Forms.MouseButtons.Right)
        //    {
        //        msmrfunc.mrGlobals.crequired = "C";
        //        lookupsource = "G"; //on groupcode
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR CORPORATE CLIENTS GROUPCODE";
        //        frmselcode FrmSelCode = new frmselcode();
        //        FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //        FrmSelCode.ShowDialog();

        //        //MessageBox.Show("you got it!");
        //    }

        //}

        /*
                    GOTO 2
                    singlepvtfc_src = autopgroup
                    misrereg = cashpoint
                    misreregpvt = filemode
                    misreregall = dactive GOTO 3  mduration = last_no   GOTO 6 autogenreg = attendlink   GOTO 5   must_patphoto = installed 
                    if (mpauto .OR. mpflex)
                        goto 2 
                        mlastno = last_no+1
                    endif */

        //private void txtpatientno_Validating(object sender, CancelEventArgs e)
        //{
        //    /*          if (string.IsNullOrWhiteSpace(this.txtpatientno.Text) || string.IsNullOrWhiteSpace(this.txtgroupcode.Text))
        //              {
        //                  return;
        //              }
        //              //configure value entered
        //             if (string.IsNullOrWhiteSpace(AnyCode) && mpauto) //no lookup value obtained
        //              {
        //                  if (Convert.ToInt32(this.txtpatientno.Text) > mlastno)
        //                  {
        //                      MessageBox.Show("Patient Number is out of Seguence...");
        //                      txtpatientno.Focus();
        //                  }
        //                  this.txtpatientno.Text = msmrfunc.autonumconfig(this.txtpatientno.Text, true, "", "9999999");
        //              } */
        //}

        //private void Combillspayable_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(Combillspayable.Text) && Combillspayable.SelectedItem != null)
        //    {
        //        Combillspayable.Text = Combillspayable.SelectedItem.ToString();
        //    }
        //    Combillspayable_LostFocus(null, null);

        //}

        //private void Combillspayable_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(Combillspayable.Text))
        //        return;

        //    txtghgroupcode.Enabled = txtbillspayable.Enabled = true;

        //    mgrouphtype = Combillspayable.Text.Substring(0, 1);

        //    comhmoservgrp.Enabled = false;

        //    if (mgrouphtype == "S")
        //    {
        //        txtghgroupcode.Text = txtgroupcode.Text;
        //        txtbillspayable.Text = txtpatientno.Text;
        //        txtghgroupcode.Enabled = txtbillspayable.Enabled = false;
        //        combillcategory.Focus();
        //    }
        //    else if (mgrouphtype == "P")
        //        txtghgroupcode.Focus();
        //    else
        //        comhmoservgrp.Enabled = true;
        //    txtbillspayable.Focus();
        //}

        //private void txtcreditlimit_GotFocus(object sender, EventArgs e)
        //{
        //    // if ( string.IsNullOrWhiteSpace(this.txtcreditlimit.Text) && !string.IsNullOrWhiteSpace(mgrouphtype))
        //    //  this.txtcreditlimit.Text = custclass.creditlimit.tostring();


        //}

        //private void dtregistered_Validating(object sender, CancelEventArgs e)
        //{
        //    if (this.dtregistered.Value < this.dtbirthdate.Value || this.dtregistered.Value > DateTime.Now)
        //    {
        //        MessageBox.Show("Invalid Date - Must be between date of Birth and today...");
        //        this.dtregistered.Select();
        //    }
        //}

        //private void initcomboboxes()
        //{
        //    this.cbopistate.DataSource = dtstate;
        //    //get patient state codes
        //    cbopistate.ValueMember = "Type_code";
        //    //Setting Combo Box DisplayMember Property
        //    cbopistate.DisplayMember = "Name";
        //    //combpistate.Items.Insert(0, "");
        //    //combpistate.SelectedIndex = 0;
        //    //get country codes
        //    this.cbonationality.DataSource = dtcountry;
        //    cbonationality.ValueMember = "Type_code";
        //    cbonationality.DisplayMember = "Name";
        //    //txtnationality.Items.Insert(0, "");
        //    //txtnationality.SelectedIndex = 0;
        //    //get patient occupation
        //    this.cbooccupation.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from designationcodes order by name", true);
        //    cbooccupation.ValueMember = "Type_code";
        //    cbooccupation.DisplayMember = "Name";
        //    //txtoccupation.Items.Insert(0, "");
        //    //txtoccupation.SelectedIndex = 0;
        //    //get next of kin state
        //    this.cbokinstate.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from statecodes order by name", true);
        //    cbokinstate.ValueMember = "Type_code";
        //    cbokinstate.DisplayMember = "Name";
        //    //txtkinstate.Items.Insert(0, "");
        //    //txtkinstate.SelectedIndex = 0;
        //    //get employer state
        //    this.cboemploystate.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from statecodes order by name", true);
        //    cboemploystate.ValueMember = "Type_code";
        //    cboemploystate.DisplayMember = "Name";
        //    //txtemploystate.Items.Insert(0, "");
        //    //txtemploystate.SelectedIndex = 0;
        //    //get primary doc
        //    this.cboprimarydoc.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT reference, name from doctors where rectype = 'D' order by name", true); //medical staff details - doctors
        //    cboprimarydoc.ValueMember = "Reference";
        //    cboprimarydoc.DisplayMember = "Name";
        //    //get referring doc
        //    this.cboreferringdoc.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT custno, name from customer where referrer = '1' order by name", true);
        //    cboreferringdoc.ValueMember = "CustNo";
        //    cboreferringdoc.DisplayMember = "Name";
        //    //txtprimarydoc.Items.Insert(0, "");
        //    //txtprimarydoc.SelectedIndex = 0;
        //    //get billing class
        //    this.combillcategory.DataSource = dtcustclass; // Dataaccess.GetAnytable("", "MR", "SELECT reference, name from custclass order by name", true);
        //    combillcategory.ValueMember = "Reference";
        //    combillcategory.DisplayMember = "Name";
        //    //combillcategory.Items.Insert(0, "");
        //    //combillcategory.SelectedIndex = 0;
        //    //get currency
        //    this.txtcurrency.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from currencycodes order by name", true);
        //    txtcurrency.ValueMember = "Type_code";
        //    txtcurrency.DisplayMember = "Name";
        //    //txtcurrency.Items.Insert(0, "");
        //    //txtcurrency.SelectedIndex = 0;
        //    //get clinic
        //    this.txtclinic.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from servicecentrecodes order by name", true);
        //    txtclinic.ValueMember = "Type_code";
        //    txtclinic.DisplayMember = "Name";
        //    //txtclinic.Items.Insert(0, "");
        //    //txtclinic.SelectedIndex = 0;

        //    this.cboTribe.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT DISTINCT tribe from patdetail order by tribe", true);
        //    this.cboTribe.ValueMember = "tribe";
        //    this.cboTribe.DisplayMember = "tribe";

        //    cboLGA.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT DISTINCT lga from patdetail order by lga", true);
        //    this.cboLGA.ValueMember = "lga";
        //    this.cboLGA.DisplayMember = "lga";

        //    cboreferringdoc.DataSource = dtreferrers;
        //    cboreferringdoc.ValueMember = "custno";
        //    cboreferringdoc.DisplayMember = "Name";
        //}

        //private void edtspinstructions_LostFocus(object sender, EventArgs e)
        //{
        //    this.cmbsave.Focus();
        //}

        //private void txtgroupcode_LostFocus(object sender, EventArgs e)
        //{
        //    if (txtgroupcode.Text.Trim() == "PVT")
        //        this.cbotype.SelectedItem = "Private";
        //    else if (txtgroupcode.Text.Trim() == "FC")
        //        this.cbotype.SelectedItem = "Family";
        //    {
        //        this.cbotype.Show();
        //        AnyCode = Anycode1;
        //        Anycode1 = "";
        //    }
        //}

        //private void comtitle_LostFocus(object sender, EventArgs e)
        //{
        //    if (this.cbotitle.SelectedItem == null)
        //        this.cbogender.SelectedItem = "";
        //    else if (this.cbotitle.SelectedItem.ToString().Trim() == "Mrs." || this.cbotitle.SelectedItem.ToString().Trim() == "Miss" || this.cbotitle.SelectedItem.ToString().Trim() == "Ms")
        //        this.cbogender.SelectedItem = "FEMALE";
        //    else if (this.cbotitle.SelectedItem.ToString().Trim() == "Mr." || this.cbotitle.SelectedItem.ToString().Trim() == "Master" || this.cbotitle.SelectedItem.ToString().Trim() == "Alhaji")
        //        this.cbogender.SelectedItem = "MALE";

        //    { this.cbogender.Show(); }
        //    txtsurname.Focus();
        //}

        /*       private void txtcreditlimit_KeyPress(object sender, KeyPressEventArgs e)
               {

                   if (Char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                   {
                       //e.Handled = true;

                   }
                   else
                   {
                       MessageBox.Show("Textbox must be numeric only!");
                   }
               }*/

        #region
        public MR_DATA.MR_DATAvm cmbsave_Click()
        {
            patients = patientinfo.GetPatient(vm.REPORTS.txtpatientno.Trim(), vm.REPORTS.txtgroupcode.Trim());

            newrec = patients == null ? true : false;
            
            //DialogResult
            if (newrec && !mcanadd || !newrec && !mcanalter)
            {
                string xstr = newrec ? "To New Record Creation." : "To Alteration of Existing Record.";
                vm.REPORTS.ActRslt = "ACCESS DENIED..." + xstr + "  See your Systems Administator!";

                return vm;
            }

            //validate fields
            //if (!bissclass.IsPresent(this.txtgroupcode, "Patients Groupcode", false) ||
            //    !bissclass.IsPresent(this.txtpatientno, "Patient Number", false) ||
            //    !bissclass.IsPresent(this.txtsurname, "Patients Surname", false) ||
            //    !bissclass.IsPresent(this.txtothername, "Patients OtherName", false) ||
            //    !bissclass.IsPresent(this.cbotype, "Patient's Group Type", false) ||
            //    !bissclass.IsPresent(this.Combillspayable, "Bills Payable By", false) ||
            //    !bissclass.IsPresent(this.dtregistered, "Date Registered", false) ||
            //    !bissclass.IsPresent(txtbillspayable, " Who Pays the BIll", false) ||
            //    !bissclass.IsPresent(this.TXTPATIENTNAME, "Patient's Full Name", false))
            //{
            //    cmbsave.Enabled = true;
            //    return;
            //}

            //if (combillcycle.Text == "" || combillcategory.SelectedValue == null)
            //{
            //    MessageBox.Show("Patient Billing Circle and Category are required...");
            //    return;
            //}

            //!bissclass.IsPresent(combillcategory,"Patient Billing Category", false )
            /*     if (string.IsNullOrWhiteSpace(combillcycle.Text) || string.IsNullOrWhiteSpace(combillcategory.Text))
                 {
                     result = MessageBox.Show("Patient Billing Circle and Category are required...");
                     cmbsave.Enabled = true;
                     return;
                 }*/

            //if (must_patphoto && string.IsNullOrWhiteSpace(PicSelected))
            //{
            //    result = MessageBox.Show("Patient Photo Must be Captured and attached....RECORD NOT SAVED !");
            //    cmbsave.Enabled = true;
            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(txthomephone.Text) && string.IsNullOrWhiteSpace(txtworkphone.Text))
            //{
            //    result = MessageBox.Show("This Patient's Work or Home Phone Number must be registered");
            //    cmbsave.Enabled = true;
            //    return;
            //}

            //save records
            //this.StorePatientsControls();
            //  pleaseWait.Hide();

            //result = MessageBox.Show("Confirm to Save...", "Patient Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;


            //if (mpauto && newrec && bissclass.IsDigitsOnly(vm.REPORTS.txtpatientno) && Convert.ToDecimal(vm.REPORTS.txtpatientno) == mlastno && vm.REPORTS.txtpatientno.Trim() != "MISC")
            //{
            //    decimal lastnosave = mlastno;
            //    mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 2, true, mlastno, false);
            //    if (mlastno != lastnosave)
            //        vm.REPORTS.txtpatientno = bissclass.autonumconfig(mlastno.ToString(), true, "", "9999999");
            //}

            savepatientdetails();
            savebillchainetails();
            //check and update group further details
            savepatientFurtherdetails();

            vm.REPORTS.ActRslt = "Successfull";

            //ClearControls();
            //tabControl1.SelectedTab = tabPage1;
            //txtpatientno.Text = AnyCode = "";
            ////   cmbsave.Enabled = false;
            //txtpatientno.Select();


            //cmbsave.Enabled = true;

            return vm;

        }

        void savepatientFurtherdetails()
        {
            bool xnew = true;
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select NOK_RELATIONSHIP, PREVIOUSMEDNOTES from patdetail where groupcode = '" + vm.REPORTS.txtgroupcode + "' and patientno = '" + vm.REPORTS.txtpatientno + "'", false);
            if (dt.Rows.Count > 0)
                xnew = false;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = xnew ? "Patdetail_Add" : "Patdetail_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", vm.REPORTS.txtpatientno);
            insertCommand.Parameters.AddWithValue("@groupcode", vm.REPORTS.txtgroupcode);
            insertCommand.Parameters.AddWithValue("@nationality", vm.SYSCODETABSvm.CountryCodes.name == null ? "" : vm.SYSCODETABSvm.CountryCodes.name.ToString());
            insertCommand.Parameters.AddWithValue("@occupation", vm.SYSCODETABSvm.DesignationCodes.name);
            insertCommand.Parameters.AddWithValue("@religion", vm.REPORTS.cboReligion);
            insertCommand.Parameters.AddWithValue("@bloodgroup", vm.REPORTS.cbobloodgroup);
            insertCommand.Parameters.AddWithValue("@genotype", vm.REPORTS.cbogenotype);
            insertCommand.Parameters.AddWithValue("@nextofkin", vm.REPORTS.txtnextofkin);
            insertCommand.Parameters.AddWithValue("@nok_Adr1", vm.REPORTS.txtkinaddress1);
            insertCommand.Parameters.AddWithValue("@nok_Phone", vm.REPORTS.txtkinphone);
            //insertCommand.Parameters.AddWithValue("@lga", cboLGA.Text);
            //insertCommand.Parameters.AddWithValue("@tribe", cboTribe.Text);
            insertCommand.Parameters.AddWithValue("@NOK_RELATIONSHIP", xnew ? "" : dt.Rows[0]["NOK_RELATIONSHIP"].ToString());
            insertCommand.Parameters.AddWithValue("@PREVIOUSMEDNOTES", xnew ? "" : dt.Rows[0]["PREVIOUSMEDNOTES"].ToString());

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                //MessageBox.Show("Unable to Open SQL Server Database Table" + ex, "Add Patient Further Details", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                errorProp = "Unable to Open SQL Server Database Table" + ex.Message;
                return;
            }
            finally
            {
                connection.Close();
            }

        }
        #endregion

        //private void btnghgroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btnghgroupcode")
        //    {
        //        this.txtghgroupcode.Text = "";
        //        lookupsource = "GHGC";
        //        msmrfunc.mrGlobals.crequired = "pp";
        //    }
        //    else if (btn.Name == "btngrouphead")
        //    {
        //        this.txtbillspayable.Text = "";
        //        lookupsource = "GH";
        //        msmrfunc.mrGlobals.crequired = mgrouphtype == "C" ? "C" : "PP";
        //    }
        //    msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED GROUPHEADS";
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //public MR_DATA.MR_DATAvm txtbillspayable_Validating(string groupCode2, string code, string mgrouphtype)
        //{
        //    if (string.IsNullOrWhiteSpace(code) || mgrouphtype == "P" && string.IsNullOrWhiteSpace(groupCode2))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        Customer customer = new Customer();
        //        patientinfo patgrphead = new patientinfo();
        //        if (mgrouphtype == "P")
        //            patgrphead = patientinfo.GetPatient(code, groupCode2);
        //        else
        //            customer = Customer.GetCustomer(code);

        //        if (mgrouphtype == "P" && patgrphead == null || mgrouphtype == "C" && customer == null)
        //        {
        //            vm.REPORTS.ActRslt = "Invalid GroupHead Specification as responsible for Bills";
        //        }
        //        else
        //        {
        //            // this.DisplayPatients();
        //            vm.REPORTS.lblbillspayable = (mgrouphtype == "P") ? patgrphead.name : customer.Name;
        //            if (mgrouphtype == "P" && !patgrphead.isgrouphead)
        //            {
        //                vm.REPORTS.ActRslt = "Specified Patient is not a registered GroupHead...";
        //            }
        //        }
        //    }

        //    return vm;
        //}

        //void combostyleset(ComboBoxStyle xval)
        //{
        //    // xval = "Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown";
        //    this.cbopistate.DropDownStyle = xval;
        //    this.cbomaritalstatus.DropDownStyle = xval;
        //    this.cbogender.DropDownStyle = xval;
        //    this.combillcycle.DropDownStyle = xval;
        //    this.txtcurrency.DropDownStyle = xval;
        //    this.combillcategory.DropDownStyle = xval;
        //    this.comhmoservgrp.DropDownStyle = xval;
        //    this.txtclinic.DropDownStyle = xval;
        //    this.cboprimarydoc.DropDownStyle = xval;
        //    this.cboreferringdoc.DropDownStyle = xval;
        //    this.cbonationality.DropDownStyle = xval;
        //    this.cbooccupation.DropDownStyle = xval;
        //    this.cbokinstate.DropDownStyle = xval;
        //    this.cboemploystate.DropDownStyle = xval;
        //    return;
        //}

        public MR_DATA.MR_DATAvm cmbdelete_Click(string patientNo, string groupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            patientinfo patients = new patientinfo();

            //check if patientno exists
            patients = patientinfo.GetPatient(patientNo, groupCode); //New addition to the code

            if (patients.posted)
            {
                vm.REPORTS.ActRslt = "Record Can't be Deleted... Its Posted!";
                return vm;
            }

            if (patients.isgrouphead)
            {
                DataTable dt = Dataaccess.GetAnytable("", "MR", "select name from billchain where ghgroupcode = '" + patients.groupcode + "' AND GROUPHEAD = '" + patients.patientno + "'", false);
                if (dt.Rows.Count > 1)
                {
                    vm.REPORTS.ActRslt = "This Record cannot be deleted... There are Records that are Grouped on it";
                    return vm;
                }
            }

            //DialogResult result = ("Confirm Delete...", "Patient Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            patientinfo.DeletePatient(patientNo, groupCode);
            billchaindtl.DeleteBillchain(patientNo, groupCode);

            //this.ClearControls();
            //this.txtpatientno.Text = txtgroupcode.Text = "";
            vm.REPORTS.alertMessage = "Record Deleted...";
            // this.txtgroupcode.Focus();

            return vm;
        }


        //private void nmrcurdebit_Enter(object sender, EventArgs e)
        //{
        //    nmrcurdebit.Select(0, nmrcurdebit.Text.Length);
        //}
        //private void nmrcurcredit_Enter(object sender, EventArgs e)
        //{
        //    nmrcurcredit.Select(0, nmrcurcredit.Text.Length);
        //}
        //private void txtdiscount_Enter(object sender, EventArgs e)
        //{
        //    txtdiscount.Select(0, txtdiscount.Text.Length);
        //}

        #region
        //private void btngetpicture_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog1 = new OpenFileDialog();
        //    openFileDialog1.Title = "Open bitmap or jpeg.";
        //    openFileDialog1.Filter = "jpg files (*.jpg);*.jpg;*.* | bmp files (*.bmp); *.bmp";
        //    openFileDialog1.Closed += openFileDialog1_Closed;
        //    openFileDialog1.ShowDialog();
        //}

        //void openFileDialog1_Closed(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog1 = sender as OpenFileDialog;
        //    if (openFileDialog1.Files.Count > 0)
        //    {
        //        Gizmox.WebGUI.Common.Resources.HttpPostedFileHandle file = (Gizmox.WebGUI.Common.Resources.HttpPostedFileHandle)openFileDialog1.Files[0];
        //        string filename = System.IO.Path.GetFileName(file.PostedFileName);
        //        string savepath = VWGContext.Current.Server.MapPath("~/Resources/Images/" + filename);

        //        PicSelected = savepath;
        //        Gizmox.WebGUI.Common.Resources.ImageResourceHandle imageResourceHandlePic1 = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle();


        //        // if (clsMain.FileExists(savepath)) clsMain.FileDelete(savepath);
        //        file.SaveAs(savepath);
        //        file.Close();
        //        file.Dispose();
        //        imageResourceHandlePic1.File = filename;
        //        pictureBox1.Image = imageResourceHandlePic1;
        //    }
        //}


        //private void txtcreditlimit_Enter(object sender, EventArgs e)
        //{
        //    txtcreditlimit.Select(0, txtcreditlimit.Text.Length);
        //}
        //private void cmdgrpmember_Click(object sender, EventArgs e)
        //{
        //    frmGroupMembersdt groupmebers = new frmGroupMembersdt(patients, woperator);
        //    groupmebers.Show();
        //}

        //private void btnchainedmedhistory_Click(object sender, EventArgs e)
        //{
        //    if (patients == null || string.IsNullOrWhiteSpace(patients.patientno))
        //        return;
        //    frmChainMedicalHistory chainedmedhist = new frmChainMedicalHistory(patients.groupcode, patients.patientno, patients.name);
        //    chainedmedhist.Show();
        //}
        #endregion


        //public MR_DATA.MR_DATAvm txtbillspayable_LostFocus(string groupCode2, string code, string mgrouphtype)
        //{
        //    vm.REPORTS = new MR_DATA.REPORTS();

        //    if (string.IsNullOrWhiteSpace(code) || mgrouphtype == "P" && string.IsNullOrWhiteSpace(groupCode2))
        //    {
        //        vm.REPORTS.chkbyacctofficers = true;

        //        return vm ;
        //    }
        //    else
        //    {
        //        vm.REPORTS.chkbyacctofficers = false;

        //        Customer customer = new Customer();
        //        patientinfo patgrphead = new patientinfo();
        //        if (mgrouphtype == "P")
        //            patgrphead = patientinfo.GetPatient(code, groupCode2);
        //        else
        //            customer = Customer.GetCustomer(code);

        //        if (mgrouphtype == "P" && patgrphead == null || mgrouphtype == "C" && customer == null)
        //        {
        //            vm.REPORTS.ActRslt = "Invalid GroupHead Specification as responsible for Bills";
        //            //txtbillspayable.Text = txtghgroupcode.Text = "";
        //        }
        //        else
        //        {
        //            // this.DisplayPatients();
        //            vm.REPORTS.lblbillspayable = (mgrouphtype == "P") ? patgrphead.name : customer.Name;
        //            if (mgrouphtype == "P" && !patgrphead.isgrouphead)
        //            {
        //                vm.REPORTS.ActRslt = "Specified Patient is not a registered GroupHead...";
        //                //txtbillspayable.Text = txtghgroupcode.Text = "";
        //            }
        //        }
        //        if (mgrouphtype == "C" && !customer.ISGROUPHEAD)
        //        {
        //            vm.REPORTS.alertMessage = "This Client is not a grouphead...CHECK CORPORATE CIENTS REGISTARTION";
        //            //txtbillspayable.Text = "";
        //            return vm;
        //        }
        //    }

        //    return vm;
        //}


        //private void cbomaritalstatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ComboBox cbo = new ComboBox();
        //    if (string.IsNullOrWhiteSpace(cbo.Text) && cbo.SelectedItem != null)
        //        cbo.Text = cbo.SelectedItem.ToString();
        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtgroupcode_LostFocus(null, null);
        //}

        //private void txtpatientno_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtpatientno_LostFocus(null, null);
        //}

        //private void txtsurname_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtothername_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtothername_LostFocus(null, null);
        //}

        //private void txtbillspayable_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtbillspayable_LostFocus(null, null);
        //}

  #region
        //private void btnFamilyGroup_Click(object sender, EventArgs e)
        //{
        //    if (newrec || patients.name == null || string.IsNullOrWhiteSpace(patients.name))
        //        return;
        //    frmFamilyGrouping fgrp = new frmFamilyGrouping(patients.ghgroupcode, patients.grouphead);
        //    fgrp.Show();
        //}

        //private void chkGetAcctInfo_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtgroupcode.Text) || patients == null || patients.patientno == null)
        //        return;
        //    if (chkGetAcctInfo.Checked)
        //        GetAcctInfo();
        //}
  #endregion


        //private void nmrBalbf_Enter(object sender, EventArgs e)
        //{
        //    NumericUpDown nmr = sender as NumericUpDown;
        //    nmr.Select(0, nmr.Text.Length);
        //}

        //private void nmrBalbf_ValueChanged(object sender, EventArgs e)
        //{
        //    lblBalbfDbCr.Text = nmrBalbf.Value < 1 ? "CR" : "DB";
        //}


    }
}





/*        {
            try
            {
                if (openFileDialog1.Files.Count > 0)
                {
                    Gizmox.WebGUI.Common.Resources.HttpPostedFileHandle file = (Gizmox.WebGUI.Common.Resources.HttpPostedFileHandle)openFileDialog1.Files[0];
                    string filename = System.IO.Path.GetFileName(file.PostedFileName);
                    string savepath = VWGContext.Current.Server.MapPath("~/Resources/Images/" + filename);

                    Gizmox.WebGUI.Common.Resources.IconResourceHandle iconResourceHandlePic1 = new Gizmox.WebGUI.Common.Resources.IconResourceHandle();


                    if (clsMain.FileExists(savepath)) clsMain.FileDelete(savepath);
                    file.SaveAs(savepath);
                    file.Close();
                    file.Dispose();
                    iconResourceHandlePic1.File = filename;
                    pic.Image = iconResourceHandlePic1;
                }
            }
            catch (Exception) { }
            finally { }
        }

                        Docspopread dpopread = new Docspopread(woperator);
            dpopread.Closed += new EventHandler(dpopread_Closed);
            dpopread.ShowDialog();
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.Title = "Open bitmap or jpeg.";
        dlg.Filter = "jpg files (*.jpg);*.jpg;*.* | bmp files (*.bmp); *.bmp";
        dlg.Closed += new EventHandler(dlg_Closed);
        dlg.ShowDialog();
      //  if (result == Gizmox.WebGUI.Forms.DialogResult.OK)
        //if ( dlg.ShowDialog() == DialogResult.OK )
        {
            pictureBox1.Image = new Bitmap(dlg.OpenFile());
        }
        dlg.Dispose();
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Title = "Open bitmap or jpeg.";
        openFileDialog1.Filter = "jpg files (*.jpg);*.jpg;*.* | bmp files (*.bmp); *.bmp";

        openFileDialog1.Closed += new EventHandler(openFileDialog1_Closed);
        openFileDialog1.ShowDialog();
        //pictureBox1.Image = new Gizmox.WebGUI.Common.Resources.UrlResourceHandle(openFileDialog1.File.ToString());

        //openFileDialog1.ShowDialog();
        //try
       // {
           // if (openFileDialog1.Files.Count > 0)
            //if (openFileDialog1.DialogResult.ToString() == "OK")
            {

//                   Gizmox.WebGUI.Common.Resources.ImageResourceHandle rh = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle();
//                 pictureBox1.Image = openFileDialog1.FileName; // PicSelected;
 //               pictureBox1.Show();
                if (clsMain.FileExists(savepath)) clsMain.FileDelete(savepath);
                file.SaveAs(savepath);
                file.Close();
                file.Dispose();
                iconResourceHandlePic1.File = filename;
                pic.Image = iconResourceHandlePic1;
            }
        //}
       // catch (Exception) { }
       // finally { } */
//  }

/*     void openFileDialog1_Closed(object sender, EventArgs e)
     {
         OpenFileDialog openFileDialog1 = sender as OpenFileDialog;
         if (openFileDialog1.DialogResult.ToString() == "OK")
         {
             pictureBox1.Image = new Gizmox.WebGUI.Common.Resources.UrlResourceHandle(openFileDialog1.FileName.ToString());
             pictureBox1.Text = openFileDialog1.FileName.ToString();
             // .ImageResourceHandle(openFileDialog1.FileName.ToString());
            // string imageurl = "fullpath";
            // pictureBox1.Image = imageurl; // .BackgroundImage = openFileDialog1.FileName;
            // pictureBox1.Text = openFileDialog1.File.ToString(); // .FileName;
         }
     }

     void dlg_Closed(object sender, EventArgs e)
     {
         //throw new NotImplementedException();
         //Docspopread dpopread_Closed = sender as Docspopread;
         Gizmox.WebGUI.Common.Resources.ImageResourceHandle rh = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle();
         OpenFileDialog dlg_Closed = sender as OpenFileDialog;
         if (dlg_Closed.DialogResult.ToString() == "OK")
         {
             //pictureBox1.Image = rh.File; // dlg_Closed.File; // new Bitmap(dlg.OpenFile());
            // PicSelected = rh.File;
            // PicSelected = dlg_Closed.File.ToString();
             PicSelected =  dlg_Closed.FileName.ToString();
             pictureBox1.Image = PicSelected;
         }
     }
 }
}*/
