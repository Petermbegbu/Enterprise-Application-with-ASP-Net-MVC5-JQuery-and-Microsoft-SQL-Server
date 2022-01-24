//#region Using

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Data.SqlClient;

//using mradmin.BissClass;
//using mradmin.DataAccess;
//using mradmin.Forms;
////using mrmenu1.Forms;

//using msfunc;


//using Gizmox.WebGUI.Common;
//using Gizmox.WebGUI.Forms;

//#endregion

//namespace OtherClasses.FILE
//{
//    public partial class Ante_Natal_Records
//    {
//        ANC01 anc01 = new ANC01();
//        ANC02 anc02 = new ANC02();
//        ANC03 acnc03 = new ANC03();
//        ANC03A anc03a = new ANC03A();
//        ANC04 anc04 = new ANC04();
//        ANC05 anc05 = new ANC05();
//        ANC06 anc06 = new ANC06();
//        DataTable anc06dt;
//        ANC07 anc07 = new ANC07();
//        ANC07A anc07a = new ANC07A();
//        //ANC07B anc07b = new ANC07B();
//        DataTable anc07b;
//        ANC07C anc07c = new ANC07C();
//        ANC07D anc07d = new ANC07D();
//        ANC08 anc08 = new ANC08();
//        ANC09 anc09 = new ANC09();
//        ANCREG ancreg = new ANCREG();
//        Vstata vstata = new Vstata();
//        Mrattend mrattend = new Mrattend();
//        APGARSCORE apgarscore = new APGARSCORE();
//        billchaindtl billchain = new billchaindtl();
//        //patientinfo patients = new patientinfo();
//        //Customer customers = new Customer();
//        MedHist medhist = new MedHist();

//        DataTable patoccupation = Dataaccess.GetAnytable("", "CODES",
//            "select type_code,name from DesignationCodes order by name", false),
//            dtdoctors = Dataaccess.GetAnytable("", "MR",
//                "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' and status = 'A' order by name", true);
//        string lookupsource, AnyCode, Anycode1, mcrossref, msection, mdoctor = "", woperator = "", manccode;
//        bool cashpaying, anc02newRecord, anc03anewRecord, anc04newRecord, anc06newRecord, anc07newRecord,
//            anc07cnewRecord, anc07anewRecord, apgarscorenewRecord, mdocson; //, recordactive; //, mcanadd, mcandelete, mcanalter;
//        int recno, currentPage5visitrecno, mdoc_seclevel, lvitemselect;
//        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start; // (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

//        public Ante_Natal_Records()
//        {
//            //InitializeComponent();
//            //isfromdoc = fromDoc;

//            mcrossref = "";

//            getcontrolsetting();

//            //mcrossref = reference;
//            //AnyCode = reference;
//            //mdoctor = xdoctor;

//            //txtConsultReference.Text = string.IsNullOrWhiteSpace(reference) ? "" : reference;
//        }

//        //private void Ante_Natal_Records_Load(object sender, EventArgs e)
//        //{
//        //    //msection = Session["section"].ToString(); 
//        //    //woperator = Session["operator"].ToString(); // msmrfunc.mrGlobals.WOPERATOR;
//        //    // if (!isfromdoc)
//        //    //mdoctor = Session["doctor"].ToString(); 
//        //    //initcomboboxes();
//        //    //msection = msmrfunc.mrGlobals.msection;
//        //    //dataGridViewPg7.Rows[0].Cells[11].Value = "Remove";
//        //    //toolTip1.SetToolTip(tabControl1.TabPages. tabPage1., "ANTE-NATAL REGISTRATION");  //  .ToolTip = "Click this tab for more information.";
//        //    //recordactive = true;
//        //}

//        void getcontrolsetting()
//        {
//            DataTable dt = Dataaccess.GetAnytable("", "MR",
//                "select docson,pvtcode from mrcontrol where recid < 4 order by recid", false);

//            mdocson = (bool)dt.Rows[0]["docson"];
//            manccode = dt.Rows[2]["pvtcode"].ToString().Substring(0, 5);

//            dt = Dataaccess.GetAnytable("", "MR",
//                "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

//            mdoc_seclevel = (Int32)dt.Rows[0]["wseclevel"];

//            /*mcanadd = (bool)dt.Rows[0]["canadd"];
//			mcanalter = (bool)dt.Rows[0]["canalter"];
//			mcandelete = (bool)dt.Rows[0]["candelete"];*/
//        }

//        //private void BtnExit_Click(object sender, EventArgs e)
//        //{
//        //	this.Close();
//        //}

//        //private void tabControl1_Click(object sender, EventArgs e)
//        //{
//        //    TabControl tpc = sender as TabControl;

//        //    if (tpc.SelectedTab.Name == "tabPage1")
//        //        this.Text = "ANTE-NATAL RECORDS - PERSONAL INFORMATION";
//        //    else if (tpc.SelectedTab.Name == "tabPage2")
//        //        this.Text = "PREVIOUS MEDICAL HISTORY";
//        //    else if (tpc.SelectedTab.Name == "tabPage3")
//        //        this.Text = "GENETIC SCREENING / TERATOLOGY COUNSELING";
//        //    else if (tpc.SelectedTab.Name == "tabPage4")
//        //        this.Text = "INITIAL PHYSICAL EXAMINATION";
//        //    else if (tpc.SelectedTab.Name == "tabPage5")
//        //        this.Text = "PRENATAL VISITS";
//        //    else if (tpc.SelectedTab.Name == "tabPage6")
//        //        this.Text = "DELIVERY RECORD";
//        //    else if (tpc.SelectedTab.Name == "tabPage7")
//        //        this.Text = "PERINEUM / IMMEDIATE POSTNATAL OBSERVATIONS";
//        //    else if (tpc.SelectedTab.Name == "tabPage8")
//        //        this.Text = "OPERATIVE DELIVERY RECORD";
//        //    else if (tpc.SelectedTab.Name == "tabPage9")
//        //        this.Text = "BABY'S RECORD / PHYSICAL EXAMINATION";
//        //    else if (tpc.SelectedTab.Name == "tabPage10")
//        //        this.Text = "PREVIOUS MEDICAL HISTORY";
//        //    else if (tpc.SelectedTab.Name == "tabPage11")
//        //        this.Text = "ALLERGIES, SP. INSTRUCTIONS, HMO/NHIS PLAN DETAIL,PHOTO";
//        //    else if (tpc.SelectedTab.Name == "tabPage12")
//        //        this.Text = "XRAY/SCAN IMAGES";
//        //}

//        //private void initcomboboxes()
//        //{
//        //    //get patient occupation
//        //    pg1_combOccupationHB.DataSource = patoccupation;
//        //    pg1_combOccupationHB.ValueMember = "Type_code";
//        //    pg1_combOccupationHB.DisplayMember = "name";

//        //    pg1_combOccupatonWf.DataSource = Dataaccess.GetAnytable("", "CODES",
//        //        "select type_code,name from DesignationCodes order by name", false);
//        //    pg1_combOccupatonWf.ValueMember = "Type_code";
//        //    pg1_combOccupatonWf.DisplayMember = "name";

//        //    pg1_combDoctor.DataSource = dtdoctors;
//        //    pg1_combDoctor.ValueMember = "reference";
//        //    pg1_combDoctor.DisplayMember = "name";
//        //}

//        //private void btngroupcode_Click(object sender, EventArgs e)
//        //{
//        //    Button btn = sender as Button;
//        //    if (btn.Name == "pg1_btngroupcodeHB")
//        //    {
//        //        pg1_txtgroupcodeHB.Text = "";
//        //        lookupsource = "g";
//        //        msmrfunc.mrGlobals.crequired = "g";
//        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
//        //    }
//        //    else if (btn.Name == "pg1_btnpatientnoHB")
//        //    {
//        //        pg1_txtpatientnoHB.Text = "";
//        //        lookupsource = "L";
//        //        msmrfunc.mrGlobals.crequired = "L";
//        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
//        //    }
//        //    else if (btn.Name == "btnAttendance")
//        //    {
//        //        this.txtConsultReference.Text = "";
//        //        lookupsource = "I";
//        //        msmrfunc.mrGlobals.lookupCriteria = chkTodaysConsult.Checked ? "C" : "";
//        //        msmrfunc.mrGlobals.crequired = "I";
//        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED DAILY ATTENDANCE";
//        //    }
//        //    else if (btn.Name == "pg1_btnANCReferemce")
//        //    {
//        //        pg1_txtANCReference.Text = "";
//        //        lookupsource = "R";
//        //        msmrfunc.mrGlobals.crequired = "ANC";
//        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED ANTE-NATAL RECORDS";
//        //    }
//        //    frmselcode FrmSelCode = new frmselcode();
//        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
//        //    FrmSelCode.ShowDialog();
//        //}

//        //void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
//        //{
//        //    frmselcode FrmSelcode = sender as frmselcode;
//        //    if (lookupsource == "I") //daiy attendance
//        //    {
//        //        this.txtConsultReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
//        //        this.txtConsultReference.Select();
//        //    }
//        //    else if (lookupsource == "R") //ANTE-NATAL
//        //    {
//        //        pg1_txtANCReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
//        //        pg1_txtANCReference.Select();
//        //    }
//        //    else if (lookupsource == "g") //ANTE-NATAL
//        //    {
//        //        pg1_txtgroupcodeHB.Text = AnyCode = msmrfunc.mrGlobals.anycode;
//        //        pg1_txtpatientnoHB.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
//        //        pg1_txtpatientnoHB.Select();
//        //    }
//        //    else if (lookupsource == "L") //ANTE-NATAL
//        //    {
//        //        pg1_txtpatientnoHB.Text = AnyCode = msmrfunc.mrGlobals.anycode;
//        //        pg1_txtpatientnoHB.Select();
//        //    }
//        //}

//        //page1
//        //private void btnreload_Click(object sender, EventArgs e)
//        //{
//        //    //get list of ANC patients
//        //    DataTable dt = msmrfunc.getLinkDetails(txtConsultReference.Text, 0, 0m, 0m, manccode, true, msection, 9, "", mdoctor);

//        //    if (dt.Rows.Count > 0)
//        //    {
//        //        frmGetlinkinfo linkinfo = new frmGetlinkinfo(dt);
//        //        linkinfo.ShowDialog();
//        //        txtConsultReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
//        //        txtConsultReference.Focus();
//        //    }
//        //}

//        //private void txtConsultReference_Enter(object sender, EventArgs e)
//        //{
//        //    clearControls();
//        //    chkRecordDeliveryDate.Visible = dtDeliveryDatepg1.Visible = false;
//        //    pg1_txtDeliveryDate.Visible = true;
//        //    if (!string.IsNullOrWhiteSpace(AnyCode)) //&& recordactive ) //no lookup
//        //    {
//        //        txtConsultReference.Text = AnyCode;
//        //    }
//        //}

//        //private void txtreference_Enter(object sender, EventArgs e)
//        //{
//        //    clearControls();
//        //    if (string.IsNullOrWhiteSpace(AnyCode)) //no lookup
//        //    {
//        //        //get list of ANC patients
//        //        frmlinkinfo FrmLinkinfo = new frmlinkinfo(txtConsultReference.Text,0,0m,0m,manccode,true,msection,9,"",mdoctor);
//        //        FrmLinkinfo.Closed += new EventHandler(FrmLinkinfo_Closed);
//        //        FrmLinkinfo.ShowDialog();
//        //    }
//        //    else
//        //    {
//        //        txtConsultReference.Text = AnyCode;
//        //    }
//        //} //Consultation reference

//        //void FrmLinkinfo_Closed(object sender, EventArgs e)
//        //{
//        //    frmlinkinfo FrmLinkinfo_Closed = sender as frmlinkinfo;

//        //    this.txtConsultReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
//        //    //this.txtreference.Focus();
//        //    txtConsultReference.Select();
//        //    recordactive = false;
//        //}

//        //private void txtConsultReference_LostFocus(object sender, EventArgs e)
//        //{
//        //    if (string.IsNullOrWhiteSpace(txtConsultReference.Text))
//        //        return;

//        //    if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtConsultReference.Text.Trim()))  //no lookup value obtained
//        //    {
//        //        this.txtConsultReference.Text = bissclass.autonumconfig(this.txtConsultReference.Text, true, "C", "999999999");
//        //    }

//        //    mrattend = Mrattend.GetMrattend(this.txtConsultReference.Text);

//        //    if (mrattend == null)
//        //    {
//        //        DialogResult result = MessageBox.Show("Unable to Link Consultation Reference in Daily Attendance Register...");
//        //        this.txtConsultReference.Text = "";
//        //        //this.btnclose.Focus();
//        //        AnyCode = "";
//        //        btnreload.PerformClick();
//        //        //txtConsultReference.Focus();
//        //        //recordactive = true;
//        //        return;
//        //    }

//        //    pg1_txtgroupcode.Text = mrattend.GROUPCODE;
//        //    pg1_txtpatientno.Text = mrattend.PATIENTNO;
//        //    pg1_TxtName.Text = mrattend.NAME;
//        //    //  string ancrtnstring = "", returnedAncRef = "";
//        //    //dttrans_date.Value = mrattend.TRANS_DATE;
//        //    //scan ancreg for current anc registeration
//        //    //   bool xval = ANCREG.GetANCREG(mrattend.PATIENTNO, mrattend.GROUPCODE, ref ancrtnstring, ref returnedAncRef );
//        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select reference from ancreg where groupcode = '" + mrattend.GROUPCODE + "' and patientno = '" + mrattend.PATIENTNO + "' AND DEL_DATE <= '" + dtmin_date + "' ", false);
//        //    // if (!xval)
//        //    if (dt.Rows.Count < 1)
//        //    {
//        //        DialogResult result = MessageBox.Show("This Patient -> " + mrattend.NAME.Trim() + " [ " + mrattend.PATIENTNO.Trim() + " ]" + "\r\n  does not have a Current ANC Registration Profile... her PATIENTNO may have " + "\r\n been changed by Record Officers between the Original ANC Registration and this Attendance..." + "\r\n Please Check and try again !!!", "ANTE-NATAL REGISTRATION");
//        //        AnyCode = txtConsultReference.Text = "";
//        //        //recordactive = true;
//        //        // txtConsultReference.Select();
//        //        btnreload.PerformClick();
//        //        return;
//        //    }
//        //    //scan anc01 for current registration
//        //    string xreference = dt.Rows[0]["reference"].ToString(); // returnedAncRef; // ancrtnstring.Substring(0, 9);
//        //    anc01 = ANC01.GetANC01(xreference);
//        //    if (anc01 == null)
//        //    {
//        //        DialogResult result = MessageBox.Show("This Patient -> " + mrattend.NAME.Trim() + " [ " + mrattend.PATIENTNO.Trim() + " ]" + "\r\n ANC REFERENCE :" + xreference + " is not on ANC Registration Profiles... her PATIENTNO may have " + "\r\n been changed by Record Officers between the Original ANC Registration and this Attendance..." + "\r\n Please Check and try again !!!", "ANTE-NATAL REGISTRATION");
//        //        //recordactive = true;
//        //        // txtConsultReference.Select();
//        //        btnreload.PerformClick();
//        //        return;
//        //    }
//        //    billchain = billchaindtl.Getbillchain(anc01.PATIENTNO, anc01.GROUPCODE);
//        //    if (billchain == null)
//        //    {
//        //        DialogResult result = MessageBox.Show("Error Reading Patients Details in Master File... \r\n Pls Check Patients Registration Details!");
//        //        return;
//        //    }
//        //    pg1_txtANCReference.Text = xreference;
//        //    pg1_txtANCReference.Enabled = false;
//        //    displayANCPages();

//        //}

//        //private void txtreference_Leave(object sender, EventArgs e)
//        //{
//        //    msgeventtracker = "";
//        //    if (string.IsNullOrWhiteSpace(pg1_txtANCReference.Text))
//        //        return;
//        //    //check if in attendance records
//        //    if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtConsultReference.Text.Trim()))  //no lookup value obtained
//        //    {
//        //        this.txtConsultReference.Text = bissclass.autonumconfig(this.txtConsultReference.Text, true, "C", "999999999");
//        //    }
//        //    mrattend = Mrattend.GetMrattend(this.txtConsultReference.Text);
//        //    if (mrattend == null)
//        //    {
//        //       DialogResult result = MessageBox.Show("Unable to Link Consultation Reference in Daily Attendance Register... ");
//        //        this.txtConsultReference.Text = " ";
//        //        //this.btnclose.Focus();
//        //        AnyCode = "";
//        //       // txtConsultReference.Focus();
//        //        return;
//        //    }
//        //    pg1_txtgroupcode.Text = mrattend.GROUPCODE;
//        //    pg1_txtpatientno.Text = mrattend.PATIENTNO;
//        //    pg1_TxtName.Text = mrattend.NAME;
//        //    string rtnstring = "";
//        //    //dttrans_date.Value = mrattend.TRANS_DATE;
//        //    //scan ancreg for current anc registeration
//        //    bool xval = ANCREG.GetANCREG(mrattend.PATIENTNO, mrattend.GROUPCODE, ref rtnstring);
//        //    if (!xval)
//        //    {
//        //        DialogResult result = MessageBox.Show("This Patient -> " + mrattend.NAME.Trim() + " [ " + mrattend.PATIENTNO.Trim() + " ]" +"\n"+
//        //        " does not have a Current ANC Registration Profile... her PATIENTNO may have " + "\n" +
//        //        "been changed by Record Officers between the Original ANC Registration and this Attendance..." + "\n" +
//        //        "Please Check and try again !!!", "ANTE-NATAL REGISTRATION");
//        //        AnyCode = txtConsultReference.Text = "";
//        //       // txtConsultReference.Select();
//        //        return;
//        //    }
//        //    //scan anc01 for current registration
//        //    string xreference = rtnstring.Substring(0, 9);
//        //    anc01 = ANC01.GetANC01(xreference);
//        //    if (anc01 == null)
//        //    {
//        //        DialogResult result = MessageBox.Show("This Patient -> " + mrattend.NAME.Trim() + " [ " + mrattend.PATIENTNO.Trim() + " ]" + "\n" +
//        //        "ANC REFERENCE :" + xreference + " is not on ANC Registration Profiles... her PATIENTNO may have " + "\n" +
//        //        "been changed by Record Officers between the Original ANC Registration and this Attendance..." + "\n" +
//        //        "Please Check and try again !!!", "ANTE-NATAL REGISTRATION", msgBoxHandler);
//        //       // txtConsultReference.Select();
//        //        return;
//        //    }
//        //    pg1_txtANCReference.Text = xreference;
//        //    pg1_txtANCReference.Enabled = false;
//        //    displaydetailsPage1();
//        //    displaydetailsPage2();
//        //    displaypage3();
//        //    displaydetailsPage4();
//        //    displaydetailsPage5();
//        //    displaydetailsPage6();
//        //    displaydetailsPage7();
//        //    displaydetailsPage8();
//        //    if (anc01.DEL_DATE > dtmin_date)
//        //    {
//        //        DialogResult result1 = MessageBox.Show("This Record is closed... Patient delivered on " + anc01.DEL_DATE.ToShortDateString(), "ANC RECORD");
//        //    }
//        //    else
//        //    {
//        //        btnSave.Enabled = true;
//        //    }
//        //    //display previous medical history
//        //    displayPrevMedHistory();
//        //    displaySpecialInstructions();
//        //    //images
//        //}
//        //private void pg1_txtreference_Leave(object sender, EventArgs e)
//        //{
//        //    if (string.IsNullOrWhiteSpace(pg1_txtANCReference.Text))
//        //    {
//        //        txtConsultReference.Focus();
//        //        return;
//        //    }
//        //    anc01 = ANC01.GetANC01(pg1_txtANCReference.Text);
//        //    if (anc01 == null)
//        //    {
//        //        DialogResult result = MessageBox.Show("Invalid ANC Registration Number... ", "ANC RECORD", msgBoxHandler);
//        //        pg1_txtANCReference.Focus();
//        //        return;
//        //    }
//        //    //pg1_txtgroupcode.Text = anc01.GROUPCODE;
//        //    //pg1_PatientPhoto.Text = anc01.PATIENTNO;
//        //    //pg1_TxtName.Text = anc01.NAME;

//        //    displaydetailsPage1();
//        //    displaydetailsPage2();
//        //    displaypage3();
//        //    displaydetailsPage4();
//        //    displaydetailsPage5();
//        //    if (anc01.DEL_DATE > dtmin_date)
//        //    {
//        //        DialogResult result1 = MessageBox.Show("This Record is closed... Patient delivered on " + anc01.DEL_DATE.ToShortDateString(), "ANC RECORD");
//        //    }
//        //    else
//        //    {
//        //        btnSave.Enabled = true;
//        //    }
//        //    pg1_combOccupatonWf.Focus();
//        //    return;
//        //}

//        //private void pg1_txtANCReference_LostFocus(object sender, EventArgs e)
//        //{
//        //    if (string.IsNullOrWhiteSpace(pg1_txtANCReference.Text))
//        //    {
//        //        //recordactive = true;
//        //        //txtConsultReference.Select();
//        //        btnreload.PerformClick();
//        //        return;
//        //    }

//        //    anc01 = ANC01.GetANC01(pg1_txtANCReference.Text);
//        //    if (anc01 == null)
//        //    {
//        //        DialogResult result = MessageBox.Show("Invalid ANC Registration Number... ", "ANC RECORD");
//        //        pg1_txtANCReference.Focus();
//        //        return;
//        //    }
//        //    billchain = billchaindtl.Getbillchain(anc01.PATIENTNO, anc01.GROUPCODE);
//        //    if (billchain == null)
//        //    {
//        //        DialogResult result = MessageBox.Show("Error Reading Patients Details in Master File... \r\n Pls Check Patients Registration Details!");
//        //        return;
//        //    }

//        //    displayANCPages();
//        //    //displaydetailsPage1();
//        //    //displaydetailsPage2();
//        //    //displaypage3();
//        //    //displaydetailsPage4();
//        //    //displaydetailsPage5();
//        //    //if (anc01.DEL_DATE > dtmin_date)
//        //    //{
//        //    //    DialogResult result1 = MessageBox.Show("This Record is closed... Patient delivered on " + anc01.DEL_DATE.ToLongDateString(), "ANC RECORD");
//        //    //}
//        //    //else
//        //    //{
//        //    //    btnSave.Enabled = true;
//        //    //}
//        //    pg1_combOccupatonWf.Select();
//        //    // return;
//        //}

//        //void displayANCPages()
//        //{
//        //    displaydetailsPage1();
//        //    displaydetailsPage2();
//        //    displaypage3();
//        //    displaydetailsPage4();
//        //    displaydetailsPage5();
//        //    displaydetailsPage6();
//        //    displaydetailsPage7();
//        //    displaydetailsPage8();
//        //    if (anc01.DEL_DATE > dtmin_date)
//        //    {
//        //        DialogResult result1 = MessageBox.Show("This Record is closed... Patient delivered on " + anc01.DEL_DATE.ToLongDateString(), "ANC RECORD");
//        //        btnSave.Enabled = btnSubmitPage2.Enabled = btnSubmitPage3.Enabled = btnSubmitPage6.Enabled = btnSubmitPage7.Enabled = btnSubmitPage8.Enabled = btnSubmitPg4.Enabled = pg5_btnSave.Enabled = Pg9_btnSubmit.Enabled = false;
//        //        pg1_txtDeliveryDate.Text = anc01.DEL_DATE.ToShortDateString();
//        //    }
//        //    else
//        //    {
//        //        btnSave.Enabled = true;
//        //    }
//        //    //display previous medical history
//        //    displayPrevMedHistory(true);
//        //    displaySpecialInstructions();
//        //}

//        //private void dtDOBpg1_Leave(object sender, EventArgs e)
//        //{
//        //    if (Convert.ToInt32(DateTime.Now.Date.Year) - Convert.ToInt32(dtDOBpg1.Value.Date.Year) < 15)
//        //    {
//        //        DialogResult result = MessageBox.Show("Invalid Date of Birth Specification for this Patient...", "Patient Age is Less than 15");
//        //        // dtDOBpg1.Focus();
//        //        dtDOBpg1.Select();
//        //        return;
//        //    }

//        //    pg1_TxtAge.Text = (DateTime.Now.Date.Year - dtDOBpg1.Value.Date.Year).ToString();
//        //}

//        //private void dtLMPpg1_Leave(object sender, EventArgs e)
//        //{
//        //    if (dtLMPpg1.Value > DateTime.Now.Date)
//        //    {
//        //        DialogResult result = MessageBox.Show("Invalid LMP Date...", "ANC Record");
//        //        dtLMPpg1.Select();
//        //        return;
//        //    }

//        //    decimal xdays = Convert.ToDecimal(DateTime.Now.Date.Subtract(dtLMPpg1.Value.Date).TotalDays);
//        //    decimal xega = xdays / 7;
//        //    dtEDDpg1.Value = dtLMPpg1.Value.Date.AddDays(281);
//        //    pg1_txtGestationPeriod.Text = Math.Round(xega, 0).ToString() + " Wk(s)";
//        //}

//        //private void pg1_txtpatientnoHB_Leave(object sender, EventArgs e)
//        //{
//        //    if (string.IsNullOrWhiteSpace(pg1_txtpatientnoHB.Text))
//        //    {
//        //        AnyCode = "";
//        //        return;
//        //    }
//        //    else
//        //    {
//        //        if (string.IsNullOrWhiteSpace(AnyCode))  //no lookup value obtained
//        //        {
//        //            pg1_txtpatientnoHB.Text = bissclass.autonumconfig(pg1_txtpatientnoHB.Text, true, "", "9999999");
//        //        }
//        //        //check if patientno exists
//        //        billchaindtl bchainHB = billchaindtl.Getbillchain(pg1_txtpatientnoHB.Text, pg1_btngroupcodeHB.Text);
//        //        if (bchainHB == null)
//        //        {
//        //            DialogResult result = MessageBox.Show("Invalid Patient Number... ");
//        //            pg1_txtpatientnoHB.Text = " ";
//        //            pg1_txtgroupcodeHB.Select();
//        //            return;
//        //        }
//        //        else
//        //        {
//        //            pg1_txtHusbandName.Text = bchainHB.NAME;
//        //            txthusbankphoneEmail.Text = bchainHB.PHONE.Trim() + " : " + bchainHB.EMAIL.Trim();
//        //        }
//        //        pg1_txtHusbandName.Select();
//        //        return;
//        //    }
//        //}

//        //page 2
//        //private void pg2_BtnOthers_Click(object sender, EventArgs e)
//        //{
//        //    frmMedHistoryContinue medhistcont = new frmMedHistoryContinue(anc02, pg1_txtANCReference.Text, billchain);
//        //    medhistcont.Show();
//        //}

//        //private void BtnPreviousPreg_Click(object sender, EventArgs e)
//        //{
//        //    PreviousPregnancies frmPreviousPregnancies = new PreviousPregnancies(pg1_txtANCReference.Text, pg1_txtgroupcode.Text, pg1_txtpatientno.Text, anc01.NAME);
//        //    frmPreviousPregnancies.Show();
//        //    //    pg1_txtpatientno.Text, Convert.ToInt32( anc02.PREV_PREG_TOTAL),Convert.ToInt32( anc02.NOALIVE),anc01.NAME );
//        //}

//        //private void pg2_btnSocialFamilyHistory_Click(object sender, EventArgs e)
//        //{
//        //    frmSocialnFamilyHistory familyhist = new frmSocialnFamilyHistory(anc02, pg1_txtANCReference.Text, pg1_txtgroupcode.Text, pg1_txtpatientno.Text);
//        //    familyhist.Show();
//        //}

//        private void BtnLabTest_Click(object sender, EventArgs e)
//        {
//            //  FrmPre_Natal_Records preNatal=new FrmPre_Natal_Records();
//            // preNatal.Show();
//        }
//        private void BtnTetanus_Click(object sender, EventArgs e)
//        {
//            // FrmMedical_History_Cont medicahMedicalHistoryCont=new FrmMedical_History_Cont();
//            // medicahMedicalHistoryCont.Show();
//        }
//        private void BtnEdu_Click(object sender, EventArgs e)
//        {
//            //FrmEducation_Counseling educationCounseling=new FrmEducation_Counseling();
//            //educationCounseling.Show();
//            if (anc01 == null || anc01.PATIENTNO == null || string.IsNullOrWhiteSpace(anc01.PATIENTNO))
//                return;
//            frmEducationCounselling anceducation = new frmEducationCounselling(anc01, anc01.REFERENCE, anc01.GROUPCODE, anc01.PATIENTNO);
//            anceducation.Show();
//        }
//        private void button7_Click(object sender, EventArgs e)
//        {
//            // FrmRequest request=new FrmRequest();
//            // request.Show();
//        }
//        private void BtnEduCounsel_Click(object sender, EventArgs e)
//        {
//            // FrmEducation_Counseling counseling=new FrmEducation_Counseling();
//            // counseling.Show();
//        }
//        private void button10_Click(object sender, EventArgs e)
//        {
//            //FrmPostNatalVisit postNatalVisit=new FrmPostNatalVisit();
//            //postNatalVisit.Show();
//        }
//        private void button9_Click(object sender, EventArgs e)
//        {
//            //FrmDailyAppointments dailyAppointments=new FrmDailyAppointments();
//            //dailyAppointments.Show();
//        }
//        //private void pg1_txtreference_Enter(object sender, EventArgs e)
//        //{
//        //    //clearControls();
//        //    chkRecordDeliveryDate.Visible = dtDeliveryDatepg1.Visible = false;
//        //    pg1_txtDeliveryDate.Visible = true;

//        //}

//        //void clearControls()
//        //{
//        //    //Page 1
//        //    //PanSpecialInstructionsAlert.Visible =
//        //    flashBox1.Visible = SpecialInstructionsAlert.Visible = pg1_chkhighrisk.Checked =
//        //        pg1_chklowrisk.Checked = pg1_chkmedium.Checked = false;
//        //    pg1_PatientPhoto.Image = "";
//        //    pg1_combBloodGroup.Text = pg1_combDoctor.Text = pg1_combGenotype.Text = pg1_combHusbandBloodGroup.Text =
//        //        pg1_CombHusbandgenotype.Text = pg1_combOccupationHB.Text = pg1_combOccupatonWf.Text =
//        //        pg1_TxtAddress.Text = pg1_TxtAge.Text = pg1_txtAllergies.Text = pg1_TxtContrapceptive.Text =
//        //        pg1_txtDeliveryDate.Text = pg1_txtEDD.Text = pg1_txtEmployerHB.Text = pg1_txtGestationPeriod.Text =
//        //        pg1_txtgravida.Text = pg1_txtgroupcode.Text = pg1_txtgroupcodeHB.Text = pg1_txtHusbandName.Text = pg1_txtInstructions.Text =
//        //        pg1_txtLMP.Text = pg1_Txtmenarche.Text = pg1_TxtName.Text = pg1_txtpatientno.Text = pg1_txtpatientnoHB.Text =
//        //        pg1_txtANCReference.Text = pg1_TxtRegularity.Text = pg1_txtTribe.Text = "";

//        //    //page2
//        //    pg2_ChkDiabetes.Checked = pg2_ChkHypertension.Checked = pg2_ChkHeartDisease.Checked = pg2_ChkSickleCellDisease.Checked =
//        //        pg2_ChkpulmonaryTbasthma.Checked = pg2_ChkKidneyDisease.Checked = pg2_Hepatitis.Checked = pg2_chkNeurologic.Checked =
//        //        pg2_ChkThyroid.Checked = pg2_ChkPhychiatric.Checked = false;

//        //    pg2_TxtDiabetes.Text = pg2_TxtHypertention.Text = pg2_txtHeartDisease.Text = pg2_txtSickleCellDisease.Text =
//        //        pg2_txtPulmonary.Text = pg2_TxtKidney.Text = pg2_txtHepatitis.Text = pg2_txtNeurologic.Text = pg2_txtThyroid.Text =
//        //        pg2_TxtPhychiatric.Text = "";
//        //    //page3
//        //    pg3_ChkpatientsAge.Checked = pg3_ChkSickleCell.Checked = pg3_ChkDownSyndrome.Checked = pg3_ChkChromosonalAbnormalities.Checked =
//        //        pg3_ChkCongenitalHEartDisease.Checked = pg3_chkotherinheritedgenetic.Checked = pg3_chkmaternalMetabolic.Checked =
//        //        pg3_chkNeuraltube.Checked = pg3_chkHistoryofrecrrentprgloss.Checked = pg3_chkVitamins.Checked = false;
//        //    //page4
//        //    heentAbnormal.Checked = heentNormal.Checked = FundiNormal.Checked = FundiAbnormal.Checked = TeethNormal.Checked =
//        //        TeethAbnormal.Checked = ThyroidNormal.Checked = ThyroidAbnormal.Checked = BreastNormal.Checked =
//        //        BreastAbnormal.Checked = LungsNormal.Checked = LungsAbnormal.Checked = HeartNormal.Checked = HeartAbnormal.Checked =
//        //        AbdomenNormal.Checked = AbdomenAbnormal.Checked = ExtremitiesNormal.Checked = ExtremitiesAbnormal.Checked =
//        //        SkinNormal.Checked = SkinAbnormal.Checked = LymphNormal.Checked = LymphAbnormal.Checked = pg4_optVulvaNormal.Checked =
//        //        pg4_optVulvaCondyloma.Checked = pg4_OptVulvaLesions.Checked = pg4_optVaginaNormal.Checked = pg4_OptDischargeVagina.Checked =
//        //        pg4_OptCervixNormal.Checked = Pg7_chkCervicalTear.Checked = false;

//        //    pg4_optFibroidYes.Checked = pg4_optFibroidNo.Checked = pg4_OPtabnormalAdnexa.Checked = pg4_optHaemorrhoidsYes.Checked =
//        //        pg4_optHaemorrhoidsoptNo.Checked = false;

//        //    pg4_txtuterussize.Text = pg4_TxtComments.Text = pg4_TxtPlan.Text = pg4_TxtInterviewdoneBy.Text = "";
//        //    //page5
//        //    dataGridView1.Rows.Clear();
//        //    //page6
//        //    pg6_BabyDeliveredTIme.Text = pg6_dtruptureTime.Text = pg6_PushingCommencedTIme.Text = pg6_txtAttendingPaediatrician.Text =
//        //        pg6_TxttstCommentsIndications.Text = pg6_txtConsultantOb.Text = pg6_txtDeliverySuite.Text = pg6_txtOxydosageAndTime.Text =
//        //        pg6_TxtEstimatedBloodloss.Text = pg6_txtFetalNumber.Text = pg6_TxtFirstStage.Text = pg6_txtFurtherAction.Text =
//        //        pg6_txtGestationAge.Text = pg6_TxtIndicationLaborOnset.Text = pg6_TxtMeasuredBloodLoss.Text = pg6_TxtMemRuptIndication.Text =
//        //        pg6_txtParity.Text = pg6_txtPushingCommencedTIme.Text = pg6_txtRuptMemDuration.Text = pg6_TxtSecondStage.Text =
//        //        pg6_TxtThirdStage.Text = pg6_TxtTotalBloodloss.Text = "";
//        //    pg6_chktstActive.Checked = pg6_chkArtificial.Checked = pg6_ChkAugumented.Checked = pg6_ChkAugumented.Checked =
//        //        pg6_chkPRCombinedES.Checked = pg6_chkPREntonox.Checked = pg6_chkPREpidural.Checked = pg6_chkOxyErgometrine.Checked =
//        //        pg6_ChkInduced.Checked = pg6_chktstmanualremoval.Checked = pg6_chkMemInduced.Checked = pg6_chkPRNarcotics.Checked =
//        //        pg6_chkNoneLaborOnset.Checked = pg6_chkOxytocin.Checked = pg6_chkPRNone.Checked = pg6_chkPRPurdendal.Checked =
//        //        pg6_chkPRSpinal.Checked = pg6_chkSpontaneous.Checked = pg6_optcompletePlacenta.Checked = pg6_optIncompletePlacenta.Checked = false;
//        //}

//        //void displaydetailsPage1()
//        //{
//        //    pg1_txtANCReference.Text = anc01.REFERENCE;
//        //    pg1_txtpatientno.Text = anc01.PATIENTNO;
//        //    pg1_txtgroupcode.Text = anc01.GROUPCODE;
//        //    pg1_TxtName.Text = anc01.NAME;
//        //    pg1_TxtAddress.Text = billchain.RESIDENCE; //anc01.ADDRESS;
//        //    pg1_chklowrisk.Checked = anc01.BOOKINGTAG == 1 ? true : false; //"LOW RISK" 
//        //    pg1_chkhighrisk.Checked = anc01.BOOKINGTAG == 3 ? true : false; //"HIGH RISK" 
//        //    pg1_chkmedium.Checked = anc01.BOOKINGTAG == 2 ? true : false; //"MEDIUM RISK"
//        //    pg1_combHusbandBloodGroup.Text = anc01.HUSBANDBG;
//        //    pg1_txtgravida.Text = anc01.BOOKINGCATEGORY;
//        //    string tagstring = anc01.BOOKINGTAG == 1 ? "LOW RISK" : anc01.BOOKINGTAG == 2 ? "MEDIUM RISK" : anc01.BOOKINGTAG == 3 ? "HIGH RISK" : "";
//        //    toolTip1.SetToolTip(TxtCAT, "Booking Category - " + tagstring);
//        //    if (anc01.BOOKINGTAG == 1)
//        //        TxtCAT.BackColor = System.Drawing.Color.LightGray;
//        //    else if (anc01.BOOKINGTAG == 2)
//        //        TxtCAT.BackColor = System.Drawing.Color.LightGreen;
//        //    else if (anc01.BOOKINGTAG == 3)
//        //        TxtCAT.BackColor = System.Drawing.Color.Red;

//        //    this.TxtCAT.BackColor = pg1_chkhighrisk.Checked ? System.Drawing.Color.Red :
//        //        pg1_chklowrisk.Checked ? System.Drawing.Color.LightGray : System.Drawing.Color.LightGreen;
//        //    toolTip1.SetToolTip(this.TxtCAT, anc01.BOOKINGTAG == 1 ? "LOW RISK" : anc01.BOOKINGTAG == 2 ? "MEDIUM RISK" : anc01.BOOKINGTAG == 3 ? "HIGH RISK" : "");

//        //    dtLMPpg1.Visible = dtEDDpg1.Visible = true;
//        //    pg1_txtLMP.Visible = pg1_txtEDD.Visible = false;

//        //    dtLMPpg1.Value = anc01.LMP;
//        //    dtEDDpg1.Value = anc01.EDD;
//        //    dtDeliveryDatepg1.Value = anc01.DEL_DATE;


//        //    pg1_combBloodGroup.Text = anc01.BLOODGROUP;
//        //    if (dtDeliveryDatepg1.Value.Date <= dtmin_date)
//        //    {
//        //        pg1_txtDeliveryDate.Visible = false;
//        //        chkRecordDeliveryDate.Visible = true;
//        //        pg1_txtDeliveryDate.Text = "";
//        //    }
//        //    else
//        //        dtDeliveryDatepg1.Visible = true;
//        //    /*  if (dtLMPpg1.Value <= dtmin_date)
//        //          pg1_txtLMP.Visible = true;
//        //      if (dtEDDpg1.Value <= dtmin_date)
//        //      {
//        //          pg1_txtEDD.Visible = true;
//        //      }*/
//        //    //header information
//        //    TxtAge.Text = (DateTime.Now.Date.Year - billchain.BIRTHDATE.Year).ToString(); // anc01.AGE.ToString();

//        //    txtbloodgroup.Text = anc01.BLOODGROUP;
//        //    TxtGT.Text = anc01.GENOTYPE;
//        //    txtEDDpg1Header.Text = (anc01.EDD.Year <= dtmin_date.Year) ? "" : anc01.EDD.ToShortDateString();
//        //    TxtPatient.Text = anc01.NAME;
//        //    pg1_TxtAge.Text = anc01.AGE.ToString();

//        //    //   billchain = billchaindtl.Getbillchain(pg1_txtpatientno.Text,pg1_txtgroupcode.Text );

//        //    if (billchain.PATIENTNO == billchain.GROUPHEAD || billchain.GROUPHTYPE == "P")
//        //        cashpaying = true;
//        //    else
//        //    {
//        //        cashpaying = false;
//        //    }
//        //    if (dtDOBpg1.Value <= dtmin_date.Date || dtDOBpg1.Value == DateTime.Now.Date)
//        //    {
//        //        dtDOBpg1.Value = billchain.BIRTHDATE;
//        //        pg1_TxtAge.Text = (DateTime.Now.Date.Year - billchain.BIRTHDATE.Year).ToString();
//        //    }

//        //    dtDateofBookingpg1.Value = anc01.REG_DATE;
//        //    dtTimeofBookpg1.Text = anc01.REG_TIME;
//        //    // pg1_txtTimeofBook.Text = anc01.REG_TIME;
//        //    // pg1_combDoctor.Text = bissclass.combodisplayitemCodeName("reference", anc01.DOCTOR, dtdoctors, "name");
//        //    // pg1_combDoctor.SelectedValue = anc01.DOCTOR;
//        //    bissclass.displaycombo(pg1_combDoctor, dtdoctors, anc01.DOCTOR, "name");
//        //    pg1_txtGestationPeriod.Text = anc01.DURATIONOFPREGNANCY;

//        //    pg1_txtTribe.Text = anc01.TRIBE;
//        //    // anc01.RELIGION = reader["religion"].ToString();
//        //    //   pg1_TxtAddress.Text = anc01.ADDRESS;
//        //    pg1_combOccupatonWf.Text = anc01.OCCUPATION;
//        //    OptNone.Checked = anc01.LEVELOFEDUCATION == 0 ? true : false;
//        //    optPrimary.Checked = anc01.LEVELOFEDUCATION == 1 ? true : false;
//        //    OptSecondary.Checked = anc01.LEVELOFEDUCATION == 2 ? true : false;
//        //    OptTertiary.Checked = anc01.LEVELOFEDUCATION == 3 ? true : false;
//        //    pg1_txtHusbandName.Text = anc01.HUSBANDNAME;
//        //    pg1_combOccupationHB.Text = anc01.HUSBANDOCCUPATION;
//        //    pg1_txtEmployerHB.Text = anc01.HUSBANDEMPLOYER;
//        //    optHusbandNone.Checked = anc01.HUSBANDLEVELOFEDUCATION == 0 ? true : false;
//        //    optHusbandPrimary.Checked = anc01.HUSBANDLEVELOFEDUCATION == 1 ? true : false;
//        //    optHusbandSecondary.Checked = anc01.HUSBANDLEVELOFEDUCATION == 2 ? true : false;
//        //    optHusbandTertiary.Checked = anc01.HUSBANDLEVELOFEDUCATION == 3 ? true : false;

//        //    txthusbankphoneEmail.Text = anc01.HUSBANDPHONE;
//        //    pg1_txtgroupcodeHB.Text = anc01.HUSBANDGC;
//        //    pg1_txtpatientnoHB.Text = anc01.HUSBANDPATNO;
//        //    pg1_combHusbandBloodGroup.Text = anc01.HUSBANDBG;

//        //    // anc01.BOOKINGTAG = (Decimal)reader["bookingtag"];
//        //    // anc01.GHGROUPCOD = reader["ghgroupcod"].ToString();
//        //    // anc01.OPERATOR = reader["operator"].ToString();
//        //    //  anc01.DTIME = (DateTime)reader["dtime"];
//        //    pg1_txtInstructions.Text = anc01.SPNOTES;
//        //    //  dtDOBpg1.Value = anc01.BIRTHDATE;
//        //    pg1_combGenotype.Text = anc01.GENOTYPE;
//        //    pg1_TxtRegularity.Text = anc01.MENS_REGULARITY;
//        //    pg1_TxtContrapceptive.Text = anc01.CONTRACEPTIVEUSE;
//        //    pg1_txtAllergies.Text = anc01.RISKFACTOR;
//        //    pg1_CombHusbandgenotype.Text = anc01.HUSBANDGENOTYPE;
//        //    pg1_Txtmenarche.Text = anc01.MENARCHE;
//        //    if (Convert.ToDecimal(pg1_TxtAge.Text) < 15)
//        //    {
//        //        DialogResult result = MessageBox.Show("Please check this Patients Age......CONTINUE ?", "Patient Age is Less than 15", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
//        //        if (result == DialogResult.No)
//        //        {
//        //            dtDOBpg1.Select();
//        //            return;
//        //        }
//        //    }
//        //}

//        //void displaydetailsPage2()
//        //{
//        //    anc02 = ANC02.GetANC02(anc01.GROUPCODE, anc01.PATIENTNO);
//        //    if (anc02 == null)
//        //    {
//        //        anc02newRecord = true;
//        //        return;
//        //    }
//        //    anc02newRecord = false;

//        //    pg2_ChkDiabetes.Checked = string.IsNullOrWhiteSpace(anc02.DIABETES) ? false : true;
//        //    pg2_ChkHypertension.Checked = string.IsNullOrWhiteSpace(anc02.HYPERTENSION) ? false : true;
//        //    pg2_ChkHeartDisease.Checked = string.IsNullOrWhiteSpace(anc02.HEART_DISEASE) ? false : true;
//        //    pg2_ChkSickleCellDisease.Checked = string.IsNullOrWhiteSpace(anc02.SICKLE_CELL) ? false : true;
//        //    pg2_ChkpulmonaryTbasthma.Checked = string.IsNullOrWhiteSpace(anc02.PULMONARY) ? false : true;
//        //    pg2_ChkKidneyDisease.Checked = string.IsNullOrWhiteSpace(anc02.KIDNEYDISEASE) ? false : true;
//        //    pg2_Hepatitis.Checked = string.IsNullOrWhiteSpace(anc02.HEPATITIS) ? false : true;
//        //    pg2_chkNeurologic.Checked = string.IsNullOrWhiteSpace(anc02.NEUROLOGIC) ? false : true;
//        //    pg2_ChkThyroid.Checked = string.IsNullOrWhiteSpace(anc02.THYROID) ? false : true;
//        //    pg2_ChkPhychiatric.Checked = string.IsNullOrWhiteSpace(anc02.PSYCHIATRIC) ? false : true;

//        //    pg2_TxtDiabetes.Text = anc02.DIABETES;
//        //    pg2_TxtHypertention.Text = anc02.HYPERTENSION;
//        //    pg2_txtHeartDisease.Text = anc02.HEART_DISEASE;
//        //    pg2_txtSickleCellDisease.Text = anc02.SICKLE_CELL;
//        //    pg2_txtPulmonary.Text = anc02.PULMONARY;
//        //    pg2_TxtKidney.Text = anc02.KIDNEYDISEASE;
//        //    pg2_txtHepatitis.Text = anc02.HEPATITIS;
//        //    pg2_txtNeurologic.Text = anc02.NEUROLOGIC;
//        //    pg2_txtThyroid.Text = anc02.THYROID;
//        //    pg2_TxtPhychiatric.Text = anc02.PSYCHIATRIC;

//        //}

//        //void displaypage3()
//        //{
//        //    anc03a = ANC03A.GetANC03A(anc01.GROUPCODE, anc01.PATIENTNO);
//        //    if (anc03a == null)
//        //    {
//        //        anc03anewRecord = true;
//        //        return;
//        //    }
//        //    anc03anewRecord = false;
//        //    pg3_ChkpatientsAge.Checked = (anc03a.AGEATEDD) ? true : false;
//        //    pg3_ChkSickleCell.Checked = (anc03a.SICKLECELL) ? true : false;
//        //    pg3_ChkDownSyndrome.Checked = (anc03a.DOWNS) ? true : false;
//        //    pg3_ChkChromosonalAbnormalities.Checked = (anc03a.CHROMOSOMAL) ? true : false;
//        //    pg3_ChkCongenitalHEartDisease.Checked = (anc03a.HEARTDISEASE) ? true : false;
//        //    pg3_chkotherinheritedgenetic.Checked = (anc03a.TUBEDEFECT) ? true : false; //create field .chromosomal
//        //    pg3_chkmaternalMetabolic.Checked = (anc03a.METABOLIC) ? true : false;
//        //    pg3_chkNeuraltube.Checked = (anc03a.TUBEDEFECT) ? true : false;
//        //    pg3_chkHistoryofrecrrentprgloss.Checked = (anc03a.STILLBIRTH) ? true : false;
//        //    pg3_chkVitamins.Checked = (anc03a.MEDICATIONS) ? true : false;

//        //    pg3_chkTB.Checked = (anc03a.TB) ? true : false;
//        //    pg3_chkherpes.Checked = (anc03a.HERPES) ? true : false;
//        //    pg3_chkviralillnes.Checked = (anc03a.VIRALILLNESS) ? true : false;
//        //    pg3_chkSTD.Checked = (anc03a.STI) ? true : false;
//        //    pg3_ChkHepatitis.Checked = (anc03a.HEPATITISB) ? true : false;

//        //    pg3_txtIndexpregnancy.Text = anc03a.INDEXPREG;
//        //    pg3_txtMedicationDtl.Text = anc03a.MEDICATIONDETL;
//        //}

//        //void displaydetailsPage4()
//        //{
//        //    anc04 = ANC04.GetANC04(pg1_txtANCReference.Text);
//        //    if (anc04 == null)
//        //    {
//        //        anc04newRecord = true;
//        //        if (vstata == null)
//        //            return;
//        //        TxtWeight.Text = vstata.WEIGHT.ToString() + " kg";
//        //        //	urine WITH vstata.others,;
//        //        txtRespiration.Text = vstata.RESPIRATIO;
//        //        txtPulse.Text = vstata.PULSE;
//        //        TxtHeight.Text = vstata.HIGHT.ToString() + " mtr";
//        //        TxtBloodPressure.Text = vstata.BPSITTING;
//        //        return;
//        //    }
//        //    anc04newRecord = false;
//        //    dteAssesmentDate.Value = anc04.TRANS_DATE;
//        //    TxtHeight.Text = anc04.HEIGHT;
//        //    TxtWeight.Text = anc04.WEIGHT;
//        //    TxtBloodPressure.Text = anc04.BP;

//        //    heentAbnormal.Checked = anc04.HEENT == 2 ? true : false;
//        //    heentNormal.Checked = anc04.HEENT == 1 ? true : false;
//        //    FundiNormal.Checked = anc04.FUNDI == 1 ? true : false;
//        //    FundiAbnormal.Checked = anc04.FUNDI == 2 ? true : false;
//        //    TeethNormal.Checked = anc04.TEETH == 1 ? true : false;
//        //    TeethAbnormal.Checked = anc04.TEETH == 2 ? true : false;
//        //    ThyroidNormal.Checked = anc04.THYROID == 1 ? true : false;
//        //    ThyroidAbnormal.Checked = anc04.THYROID == 2 ? true : false;
//        //    BreastNormal.Checked = anc04.BREASTS == 1 ? true : false;
//        //    BreastAbnormal.Checked = anc04.BREASTS == 2 ? true : false;
//        //    LungsNormal.Checked = anc04.LUNGS == 1 ? true : false;
//        //    LungsAbnormal.Checked = anc04.LUNGS == 2 ? true : false;
//        //    HeartNormal.Checked = anc04.HEART == 1 ? true : false;
//        //    HeartAbnormal.Checked = anc04.HEART == 2 ? true : false;
//        //    AbdomenNormal.Checked = anc04.ABDOMEN == 1 ? true : false;
//        //    AbdomenAbnormal.Checked = anc04.ABDOMEN == 2 ? true : false;
//        //    ExtremitiesNormal.Checked = anc04.EXTREMITIES == 1 ? true : false;
//        //    ExtremitiesAbnormal.Checked = anc04.EXTREMITIES == 2 ? true : false;
//        //    SkinNormal.Checked = anc04.SKIN == 1 ? true : false;
//        //    SkinAbnormal.Checked = anc04.SKIN == 2 ? true : false;
//        //    LymphNormal.Checked = anc04.LYMPHNODES == 1 ? true : false;
//        //    LymphAbnormal.Checked = anc04.LYMPHNODES == 2 ? true : false;
//        //    pg4_optVulvaNormal.Checked = anc04.VULVA == 1 ? true : false;
//        //    pg4_optVulvaCondyloma.Checked = anc04.VULVA == 2 ? true : false;
//        //    pg4_OptVulvaLesions.Checked = anc04.VULVA == 3 ? true : false;
//        //    pg4_optVaginaNormal.Checked = anc04.VAGINA == 1 ? true : false;
//        //    pg4_OptDischargeVagina.Checked = anc04.VAGINA == 2 ? true : false;
//        //    pg4_OptCervixNormal.Checked = anc04.CERVIX == 1 ? true : false;
//        //    pg4_optInflamationNormal.Checked = anc04.CERVIX == 2 ? true : false;
//        //    pg4_OptLesionNormal.Checked = anc04.CERVIX == 3 ? true : false;

//        //    pg4_txtuterussize.Text = anc04.UTERINESIZE;
//        //    pg4_optFibroidYes.Checked = anc04.FIBROIDS == 1 ? true : false;
//        //    pg4_optFibroidNo.Checked = anc04.FIBROIDS == 2 ? true : false;
//        //    pg4_optnormalAnexa.Checked = anc04.ADNEXA == 1 ? true : false;
//        //    pg4_OPtabnormalAdnexa.Checked = anc04.ADNEXA == 2 ? true : false;
//        //    pg4_optHaemorrhoidsYes.Checked = anc04.HAEMORRHOIDS == 1 ? true : false;
//        //    pg4_optHaemorrhoidsoptNo.Checked = anc04.HAEMORRHOIDS == 2 ? true : false;

//        //    pg4_TxtComments.Text = anc04.COMMENTS;
//        //    pg4_TxtPlan.Text = anc04.DELPLAN;

//        //    pg4_TxtInterviewdoneBy.Text = anc04.INTERVIEWER;

//        //}

//        //void displaydetailsPage5()
//        //{
//        //    anc06newRecord = false;
//        //    DataGridViewRow dgv; // = new DataGridViewRow();
//        //    DataRow row;
//        //    recno = 0;
//        //    medhist = MedHist.GetMEDHIST(anc01.GROUPCODE, anc01.PATIENTNO, "", false, true, DateTime.Now.Date, "DESC");
//        //    anc06dt = ANC06.GetANC06(pg1_txtANCReference.Text);
//        //    for (int i = 0; i < anc06dt.Rows.Count; i++)
//        //    {
//        //        row = anc06dt.Rows[i];
//        //        dataGridView1.Rows.Add();
//        //        dgv = dataGridView1.Rows[i];
//        //        dgv.Cells[0].Value = row["TRANS_DATE"].ToString();
//        //        dgv.Cells[1].Value = row["GESTATIONALAGE"].ToString();
//        //        dgv.Cells[2].Value = row["HIGHT_OF_FUNDUS"].ToString();
//        //        dgv.Cells[3].Value = row["PRESENTATION_POSITION"].ToString();
//        //        dgv.Cells[4].Value = row["RELATION_OF_PP_TOBRIM"].ToString();
//        //        dgv.Cells[5].Value = row["FOETAL_HEART"].ToString();
//        //        dgv.Cells[6].Value = row["URINE"].ToString();
//        //        dgv.Cells[7].Value = row["BLOOD_PRESSURE"].ToString();
//        //        dgv.Cells[8].Value = row["WEIGHT"].ToString();
//        //        dgv.Cells[9].Value = row["REMARKS_TREATMENT"].ToString();
//        //        //dgv.Cells[10].Value = button
//        //        dgv.Cells[11].Value = row["nnv"].ToString();
//        //        dgv.Cells[12].Value = Convert.ToDateTime(row["NEXTVISIT"]) <= dtmin_date ? "" : Convert.ToDateTime(row["NEXTVISIT"]).ToString("dd/MM/yyyy");
//        //        dgv.Cells[13].Value = row["DOCTOR"].ToString();
//        //        recno = i;
//        //    }
//        //    anc06 = ANC06.GetANC06(pg1_txtANCReference.Text, DateTime.Now.Date);
//        //    if (anc06 == null)
//        //    {
//        //        anc06newRecord = true;
//        //        vstata = Vstata.GetVSTATA(txtConsultReference.Text);
//        //        // dataGridView1.Rows.Count - 1;
//        //        dataGridView1.Rows.Add();
//        //        recno = dataGridView1.Rows.Count - 1;
//        //        dataGridView1.Rows[recno].Cells[0].Value = DateTime.Now.ToShortDateString();
//        //        if (vstata != null)
//        //        {
//        //            dataGridView1.Rows[recno].Cells[6].Value = vstata.OTHERS.ToString();
//        //            dataGridView1.Rows[recno].Cells[7].Value = vstata.BPSITTING.ToString();
//        //            dataGridView1.Rows[recno].Cells[8].Value = vstata.WEIGHT.ToString("N2") + "kg";
//        //            dataGridView1.Rows[recno].Cells[13].Value = vstata.DOCTOR.ToString();
//        //        }
//        //        dataGridView1.Rows[recno].Cells[9].Value = medhist != null ? medhist.COMMENTS.ToString() : "";
//        //    }
//        //}

//        //void displaydetailsPage6()
//        //{
//        //    recno = 0;
//        //    anc07 = ANC07.GetANC07(anc01.REFERENCE);
//        //    if (anc07 == null)
//        //    {
//        //        anc07newRecord = true;
//        //        return;
//        //    }
//        //    anc07newRecord = false;
//        //    pg6_txtConsultantOb.Text = anc07.CONSOBS;
//        //    pg6_txtAttendingPaediatrician.Text = anc07.ATTENDPAED;
//        //    pg6_dtpDeliverydate.Value = anc07.DELDATE;
//        //    pg6_txtDeliverySuite.Text = anc07.DELSUITE;
//        //    pg6_txtBabyDeliveredTIme.Text = anc07.DELTIME;
//        //    pg6_txtParity.Text = anc07.PARITY;
//        //    pg6_txtFetalNumber.Text = anc07.FETALNUMBER;
//        //    pg6_txtGestationAge.Text = anc07.GESTAGE;
//        //    pg6_chkNoneLaborOnset.Checked = anc07.LO_NONE ? true : false;
//        //    pg6_chkSpontaneous.Checked = anc07.LO_SPONTANEOUS ? true : false;
//        //    pg6_ChkInduced.Checked = anc07.LO_INDUCED ? true : false;
//        //    pg6_ChkAugumented.Checked = anc07.LO_AUGUMENTD ? true : false;
//        //    pg6_TxtIndicationLaborOnset.Text = anc07.INDICANTIONS;
//        //    pg6_chkArtificial.Checked = anc07.ROM_ACTIFICIAL;
//        //    pg6_chkMemInduced.Checked = anc07.ROM_INDUCED ? true : false;
//        //    pg6_TxtMemRuptIndication.Text = anc07.ROM_INDICATIONS;
//        //    pg6_dtruptureDate.Value = anc07.ROMDATE;
//        //    pg6_txtRuptMemDuration.Text = anc07.ROM_DURATION;
//        //    pg6_txtdtruptureTime.Text = anc07.ROMTIME;
//        //    pg6_chkPRNone.Checked = anc07.PR_NONE ? true : false;
//        //    pg6_chkPRNarcotics.Checked = anc07.PR_NARCOTICS ? true : false;
//        //    pg6_chkPRPurdendal.Checked = anc07.PR_PRUDENDAL ? true : false;
//        //    pg6_chkPREntonox.Checked = anc07.PR_ENTONOX ? true : false;
//        //    pg6_chkPREpidural.Checked = anc07.PR_EPIDURAL ? true : false;
//        //    pg6_chkPRSpinal.Checked = anc07.PR_SPINAL ? true : false;
//        //    pg6_chkPRCombinedES.Checked = anc07.PR_COMBINED ? true : false;
//        //    pg6_chktstActive.Checked = anc07.TSTAGEM_A ? true : false;
//        //    pg6_chktstmanualremoval.Checked = anc07.TSTAGEM_M ? true : false;
//        //    pg6_TxttstCommentsIndications.Text = anc07.TSTAGEMGTNOTES;

//        //    pg6_dtpOnset.Value = anc07.LABONSETDT;
//        //    pg6_dtpFullyDilated.Value = anc07.LABFDDT;
//        //    pg6_dtpPushingCommenced.Value = anc07.LABPCDT;
//        //    pg6_dtpHeadDelivered.Value = anc07.LABHDDT;
//        //    pg6_dtpBabyDelivered.Value = anc07.LABBDDT;
//        //    pg6_dtpEndofThirdStage.Value = anc07.LABEOTSDT;
//        //    pg6_dtpTwinDel.Value = anc07.LABT2DDT;

//        //    pg6_txtOnsetTime.Text = anc07.LABPNSETTIME;
//        //    pg6_txtFullDilatedTime.Text = anc07.LABFDTIME;
//        //    pg6_txtPushingCommencedTIme.Text = anc07.LABPCTIME;
//        //    pg6_txtHeadDeliveredTIme.Text = anc07.LABHDTIME;
//        //    pg6_txtBabyDeliveredTIme.Text = anc07.LABBDTIME;
//        //    pg6_txtEndofThirdStageTime.Text = anc07.LABEOTSTIME;
//        //    pg6_txtTwinDeliveredTime.Text = anc07.LAB2DTIME;
//        //    //if (pg6_dtpDeliverydate.Value <= dtmin_date)
//        //    //{
//        //    //    txtdtpDeliveryTime.Visible = pg1_txtDeliveryDate.Visible = true;
//        //    //}
//        //    if (anc07.LABONSETDT <= dtmin_date.Date)
//        //    {
//        //        pg6_txtdtpOnset.Visible = true;
//        //        pg6_txtdtpFullyDilated.Visible = true;
//        //        pg6_txtdtpPushingCommenced.Visible = true;
//        //        pg6_txtdtpHeadDelivered.Visible = true;
//        //        pg6_txtdtpBabyDelivered.Visible = true;
//        //        pg6_txtdtpEndofThirdStage.Visible = true;
//        //        pg6_txtdtpTwinDel.Visible = true;

//        //        pg6_txtOnsetTime.Visible = true;
//        //        pg6_txtFullDilatedTime.Visible = true;
//        //        pg6_txtPushingCommencedTIme.Visible = true;
//        //        pg6_txtHeadDeliveredTIme.Visible = true;
//        //        pg6_txtBabyDeliveredTIme.Visible = true;
//        //        pg6_txtEndofThirdStageTime.Visible = true;
//        //        pg6_txtTwinDeliveredTime.Visible = true;
//        //    }

//        //    pg6_TxtFirstStage.Text = anc07.FSTSTAGEHRMIN;
//        //    pg6_TxtSecondStage.Text = anc07.SSTSTAGEHRMIN;
//        //    pg6_TxtThirdStage.Text = anc07.TSTSTAGEHRMIN;
//        //    Pg6_LaborDuration.Text = anc07.LABDURATION;

//        //    pg6_chkOxytocin.Checked = anc07.OXYTOCICS ? true : false;
//        //    pg6_chkOxyErgometrine.Checked = anc07.EGOMETRINE ? true : false;
//        //    pg6_txtOxydosageAndTime.Text = anc07.OXYTOCICSDTM;

//        //    TxtCordNoVessels.Text = anc07.CORD;
//        //    optMembApparenntlyComplete.Checked = anc07.MEMBRANES == 1 ? true : false;
//        //    optMembIncomplete.Checked = anc07.MEMBRANES == 2 ? true : false;

//        //    pg6_TxtMeasuredBloodLoss.Text = anc07.BLMEASURE;
//        //    pg6_TxtEstimatedBloodloss.Text = anc07.BLESTIMATES;
//        //    pg6_TxtTotalBloodloss.Text = anc07.BLTOTAL;

//        //    pg6_txtFurtherAction.Text = anc07.FURTHERACTN;
//        //    pg6_optcompletePlacenta.Checked = anc07.PLACENTA == 1 ? true : false;
//        //    pg6_optIncompletePlacenta.Checked = anc07.PLACENTA == 2 ? true : false;
//        //    /*         

//        //  anc07.LABDURATION;  anc07.TRAUMA_NONE;  anc07.CERVICAL_TEAR;
//        //  anc07.PERINEAL_TEAR;
//        //  anc07.TEARDEGREE;
//        //  anc07.EPISIOTOMY;
//        //  anc07.INDI4EPISIOTOMY;
//        //  anc07.REPREQ;
//        //  anc07.MOTHERAGREE;
//        //  anc07.ANAESTHUSED;
//        //  anc07.STAFF;
//        //  anc07.TRDTTIME;
//        //  anc07.BAB1BY;
//        //  anc07.BAB2BY;
//        //  anc07.BAB3BY;
//        //  anc07.COMMENTS;*/


//        //}

//        //void displaydetailsPage7()
//        //{
//        //    recno = 0;
//        //    anc07 = ANC07.GetANC07(anc01.REFERENCE);
//        //    if (anc07 == null)
//        //    {
//        //        anc07newRecord = true;
//        //        return;
//        //    }
//        //    anc07newRecord = false;
//        //    //      anc07.LABDURATION;
//        //    Pg7_ChkNonIdentified.Checked = anc07.TRAUMA_NONE ? true : false;
//        //    Pg7_chkCervicalTear.Checked = anc07.CERVICAL_TEAR ? true : false;
//        //    Pg7_ChkPerinealTear.Checked = anc07.PERINEAL_TEAR ? true : false;
//        //    Pg7_optFirstDeg.Checked = anc07.TEARDEGREE == 1 ? true : false;
//        //    Pg7_optSecondDeg.Checked = anc07.TEARDEGREE == 2 ? true : false;
//        //    Pg7_optThirdDeg.Checked = anc07.TEARDEGREE == 3 ? true : false;
//        //    Pg7_optFourthDeg.Checked = anc07.TEARDEGREE == 4 ? true : false;
//        //    Pg7_chkEpisotomy.Checked = anc07.EPISIOTOMY ? true : false;
//        //    Pg7_txtindicationforepisotomy.Text = anc07.INDI4EPISIOTOMY;
//        //    Pg7_OptRepairYes.Checked = anc07.REPREQ == 1 ? true : false;
//        //    Pg7_optRepairNo.Checked = anc07.REPREQ == 2 ? true : false;
//        //    Pg7_optMothRYes.Checked = anc07.MOTHERAGREE == 1 ? true : false;
//        //    Pg7_optMotherRNo.Checked = anc07.MOTHERAGREE == 2 ? true : false;
//        //    Pg7_txtAnaestheticUsed.Text = anc07.ANAESTHUSED;
//        //    Pg7_txtSignature.Text = anc07.STAFF;
//        //    Pg7_AnaextheticDate.Value = anc07.TRDTTIME.Date;
//        //    Pg7_AnaestheticTime.Text = anc07.TRDTTIME.ToString("HH:mm:ss");
//        //    Pg7_TxtBaby1delivered.Text = anc07.BAB1BY;
//        //    Pg7_TxtBaby2delivered.Text = anc07.BAB2BY;
//        //    Pg7_TxtBaby3delivered.Text = anc07.BAB3BY;
//        //    Pg7_commentsActions.Text = anc07.COMMENTS;
//        //    //the datagrid values
//        //    anc07b = Dataaccess.GetAnytable("", "MR", "select * from ANC07B where reference = '" + anc01.REFERENCE + "'", false);
//        //    dataGridViewPg7.Rows.Clear();
//        //    for (int i = 0; i < anc07b.Rows.Count; i++)
//        //    {
//        //        recno = i;
//        //        dataGridViewPg7.Rows.Add();

//        //        dataGridViewPg7.Rows[i].Cells[0].Value = anc07b.Rows[i]["trans_date"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[1].Value = anc07b.Rows[i]["temp"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[2].Value = anc07b.Rows[i]["pr"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[3].Value = anc07b.Rows[i]["bp"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[4].Value = anc07b.Rows[i]["sp02"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[5].Value = anc07b.Rows[i]["uterus"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[6].Value = anc07b.Rows[i]["locia"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[7].Value = anc07b.Rows[i]["wounds"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[8].Value = anc07b.Rows[i]["perineum"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[9].Value = anc07b.Rows[i]["urine"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[10].Value = anc07b.Rows[i]["staffsign"].ToString();
//        //        dataGridViewPg7.Rows[i].Cells[12].Value = "OLDREC";
//        //    }
//        //}

//        //void displaydetailsPage8()
//        //{
//        //    recno = 0;
//        //    anc07a = ANC07A.GetANC07A(anc01.REFERENCE);
//        //    if (anc07a == null)
//        //    {
//        //        anc07anewRecord = true;
//        //        return;
//        //    }
//        //    anc07anewRecord = false;

//        //    Pg8_TxtGesAge.Text = anc07a.GESTAGE;
//        //    Pg8_TxtParity.Text = anc07a.PARITY;
//        //    Pg8_trans_date.Value = anc07a.TRANS_DATE;
//        //    Pg8_txtProcess.Text = anc07a.PROCESS;
//        //    Pg8_txtIndications.Text = anc07a.INDICATIONS;
//        //    Pg8_txtStaffPresent.Text = anc07a.STAFFPRESENT;
//        //    Pg8_TxtSurgeon.Text = anc07a.SURGEON;
//        //    Pg8_txtAssistant.Text = anc07a.ASSISTANT;
//        //    Pg8_txtPaediatricians.Text = anc07a.PAEDIATRICIAN;
//        //    Pg8_txtMidwives.Text = anc07a.MIDWIVES;
//        //    Pg8_txtAnaesthetist.Text = anc07a.ANAESTHETIST;
//        //    Pg8_txtOthers.Text = anc07a.OTHERS;
//        //    Pg8_txtAnaethesia.Text = anc07a.ANAESTHESIA;
//        //    Pg8_txtFindings.Text = anc07a.FINDINGS;
//        //    Pg8_txtProcedure.Text = anc07a.PROCEDURENOTE;
//        //    Pg8_txtMother.Text = anc07a.MOTHER;
//        //    Pg8_txtBaby.Text = anc07a.BABY;
//        //    Pg8_txtStaffSign.Text = anc07a.STAFFSIGN;
//        //}

//        void diaplaydetailsPage9()
//        {
//            recno = 0;
//            anc07c = ANC07C.GetANC07C(anc01.REFERENCE);
//            if (anc07c == null)
//            {
//                anc07cnewRecord = true;
//                return;
//            }
//            anc07cnewRecord = false;
//            pg9_txtBirthWeight.Text = anc07c.BWEIGHT;
//            pg9_combSex.Text = anc07c.SEX;
//            pg9_chkSingleton.Checked = anc07c.BIRTHTYPE == 1 ? true : false;
//            pg9_chkMultiple.Checked = anc07c.BIRTHTYPE == 2 ? true : false;
//            pg9_txtGestaAge.Text = anc07c.GESTAGE;
//            pg9_txtModeofResuscitation.Text = anc07c.MODEOFRESUSC;
//            pg9_txtDrugs.Text = anc07c.DRUGS;
//            pg9_dtPhysicalExamDate.Value = anc07c.TRANS_DATE;
//            if (anc07c.TRANS_DATE > dtmin_date)
//            {
//                pg9_dtPhysicalExamDate.Visible = true;
//                pg9_txtPhysicalExamDate.Visible = false;
//            }
//            pg9_txtTime.Text = anc07c.EXAMTIME;
//            pg9_txtHR.Text = anc07c.HR;
//            pg9_txtRR.Text = anc07c.RR;
//            pg9_txtTemp.Text = anc07c.TEMP;
//            pg9_txtOFC.Text = anc07c.OFC;
//            pg9_txtLength.Text = anc07c.LENGTH;
//            pg9_txtPalor.Text = anc07c.PALOR;
//            pg9_txtCyanosis.Text = anc07c.CYANOSIS;
//            pg9_txtJaudice.Text = anc07c.JAUNDICE;
//            pg9_txtRespDistress.Text = anc07c.R_DISTRESS;
//            pg9_txtDrugs.Text = anc07c.DRUGS;
//            pg9_chkApperanceAbNormal.Checked = anc07c.APPEARANCE == 2m ? true : false;
//            pg9_chkApperanceNormal.Checked = anc07c.APPEARANCE == 1m ? true : false;
//            pg9_chkApperanceDetails.Text = anc07c.APPEARANCENOTE;
//            pg9_chkHeadNormal.Checked = anc07c.HEAD == 1m ? true : false;
//            pg9_chkHeadAbNormal.Checked = anc07c.HEAD == 2m ? true : false;
//            pg9_chkHeadDetails.Text = anc07c.HEADNOTE;
//            pg9_chkEarsNormal.Checked = anc07c.EARS == 1m ? true : false;
//            pg9_chkEarsAbNormal.Checked = anc07c.EARS == 2m ? true : false;
//            pg9_chkEarsDetails.Text = anc07c.EARSNOTE;
//            pg9_chkEyesNormal.Checked = anc07c.EYES == 1m ? true : false;
//            pg9_chkEyesAbNormal.Checked = anc07c.EYES == 2m ? true : false;
//            pg9_chkEyesDetails.Text = anc07c.EYESNOTE;
//            pg9_chkNoseNormal.Checked = anc07c.NOSE == 1m ? true : false;
//            pg9_chkNoseAbNormal.Checked = anc07c.EYES == 2m ? true : false;
//            pg9_chkNoseDetails.Text = anc07c.NOSENOTE;
//            pg9_chkMouthNormal.Checked = anc07c.MOUTH == 1m ? true : false;
//            pg9_chkMouthAbNormal.Checked = anc07c.MOUTH == 2m ? true : false;
//            pg9_chkMouthDetails.Text = anc07c.MOUTHNOTE;
//            pg9_chkRespNormal.Checked = anc07c.RESPSYSTEM == 1m ? true : false;
//            pg9_chkRespAbNormal.Checked = anc07c.RESPSYSTEM == 2m ? true : false;
//            pg9_chkRespDetails.Text = anc07c.RESPSYSTEMNOTE;
//            pg9_chkCadiovascularNormal.Checked = anc07c.CARDIOSYSTEM == 1m ? true : false;
//            pg9_chkCadiovascularAbNormal.Checked = anc07c.CARDIOSYSTEM == 2m ? true : false;
//            pg9_chkCadiovascularDetails.Text = anc07c.CARDIOSYSTEMNOTE;
//            pg9_chkAbdomenNormal.Checked = anc07c.ABDOMEN == 1m ? true : false;
//            pg9_chkAbdomenAbNormal.Checked = anc07c.ABDOMEN == 2m ? true : false;
//            pg9_chkAbdomenDetails.Text = anc07c.ABDOMENNOTE;
//            pg9_chkFemoralsNormal.Checked = anc07c.FEMORALS == 1m ? true : false;
//            pg9_chkFemoralsAbNormal.Checked = anc07c.FEMORALS == 2m ? true : false;
//            pg9_chkFemoralsDetails.Text = anc07c.FEMORALSNOTE;
//            pg9_chkAnusNormal.Checked = anc07c.ANUS == 1m ? true : false;
//            pg9_chkAnusAbNormal.Checked = anc07c.ANUS == 2m ? true : false;
//            pg9_chkAnusDetails.Text = anc07c.ANUSNOTE;
//            pg9_chkGenitaliaNormal.Checked = anc07c.GENITALIA == 1m ? true : false;
//            pg9_chkGenitaliaAbNormal.Checked = anc07c.GENITALIA == 2m ? true : false;
//            pg9_chkGenitaliaDetails.Text = anc07c.GENITALIANOTE;
//            pg9_chkExtremitesNormal.Checked = anc07c.EXTEMITES == 1m ? true : false;
//            pg9_chkExtremitesAbNormal.Checked = anc07c.EXTEMITES == 2m ? true : false;
//            pg9_chkExtremitesDetails.Text = anc07c.EXTREMITESNOTE;
//            pg9_chkHipsNormal.Checked = anc07c.HIPS == 1m ? true : false;
//            pg9_chkHipsAbNormal.Checked = anc07c.HIPS == 2m ? true : false;
//            pg9_chkHipsDetails.Text = anc07c.HIPSNOTE;
//            pg9_chkCentralNervousNormal.Checked = anc07c.CNS == 1m ? true : false;
//            pg9_chkCentralNervousAbNormal.Checked = anc07c.CNS == 2m ? true : false;
//            pg9_chkCentralNervousDetails.Text = anc07c.CNSNOTE;
//            pg9_txtOtherFindings.Text = anc07c.OTHERFINDINGS;
//            pg9_txtProblemsandComments.Text = anc07c.COMMENTS;
//            Pg8_txtStaffSign.Text = anc07c.STAFFSIGN;
//            //anc07c.MEDHISTUPDATED
//            //apgar score
//            apgarscore = APGARSCORE.GetAPGARSCORE(anc01.REFERENCE);
//            if (apgarscore == null)
//            {
//                apgarscorenewRecord = true;
//                return;
//            }
//            apgarscorenewRecord = false;

//            pg9_txtHeartRate_1.Text = apgarscore.HEART_RATE_1;
//            pg9_txtRespEffort_1.Text = apgarscore.RESPIRATIONEFF_1;
//            pg9_txtMuscleTone_1.Text = apgarscore.MUSCLE_TONE_1;
//            pg9_txtReflex_1.Text = apgarscore.REFLEX_1;
//            pg9_txtColor_1.Text = apgarscore.COLOR_1;
//            pg9_txtTotal_1.Text = apgarscore.TOTAL_1;
//            pg9_txtHeartRate_5.Text = apgarscore.HEART_RATE_5;
//            pg9_txtMuscleTone_5.Text = apgarscore.MUSCLE_TONE_5;
//            pg9_txtReflex_5.Text = apgarscore.REFLEX_5;
//            pg9_txtColor_5.Text = apgarscore.COLOR_5;
//            pg9_txtTotal_5.Text = apgarscore.TOTAL_5;
//            pg9_txtHeartRate_oth.Text = apgarscore.HEART_RATE_OTH;
//            pg9_txtRespEffort_oth.Text = apgarscore.RESPIRATIONEFF_OTH;
//            pg9_txtMuscleTone_oth.Text = apgarscore.MUSCLE_TONE_OTH;
//            pg9_txtReflex_oth.Text = apgarscore.REFLEX_OTH;
//            pg9_txtColor_oth.Text = apgarscore.COLOR_OTH;
//            pg9_txtTotal_oth.Text = apgarscore.TOTAL_OTH;

//        }

//        //void displayPrevMedHistory(bool xcurrent) //page 10
//        //{
//        //    //   DataTable dt = MedHist.GetHISTByPatient(anc01.GROUPCODE,anc01.PATIENTNO,"D", anc01.LASTATTEND.Date,  DateTime.Now.Date, true);
//        //    string medrecs = MedHist.GetMEDHISTCaseNotes(anc01.GROUPCODE, anc01.PATIENTNO, xcurrent ? false : true, xcurrent ? true : false, anc01.LASTATTEND.Date, DateTime.Now.Date, billchain, "DESC");
//        //    //   PrevMedHistoryNotes.Text = "";
//        //    // DataTable disp;
//        //    //   DateTime xdate;
//        //    //  for (int i = 0; i < dt.Rows.Count; i++)
//        //    //  {
//        //    PrevMedHistoryNotes.Text = medrecs; // PrevMedHistoryNotes.Text + medhist.COMMENTS.Trim();
//        //                                        //xdate = Convert.ToDateTime(dt.Rows[i]["trans_date"]);
//        //                                        //get prescriptons
//        //                                        //disp = DISPENSA.GetDISPENSA(anc01.GROUPCODE,anc01.PATIENTNO, medhist.TRANS_DATE, false);
//        //                                        //if (disp.Rows.Count > 0)
//        //                                        //{
//        //                                        //    PrevMedHistoryNotes.Text += "\r\n" +
//        //                                        //    "S/N Drugs Details                  Unit Presc'd    Given         Cost  D/ I /D \r\n" +
//        //                                        //    "---------------------------------------------------------------------------------------";
//        //                                        //    for (int xd = 0; xd < disp.Rows.Count; xd++)
//        //                                        //    {
//        //                                        //        PrevMedHistoryNotes.Text += "\r\n"+
//        //                                        //        disp.Rows[xd]["itemno"].ToString().Trim() + "  " + disp.Rows[xd]["stk_desc"].ToString() + " " + disp.Rows[xd]["unit"].ToString() +
//        //                                        //        " " + disp.Rows[xd]["qty_pr"].ToString() + "  " + disp.Rows[xd]["qty_gv"].ToString() + "         " +
//        //                                        //        disp.Rows[xd]["cost"].ToString() + " " + disp.Rows[xd]["cdose"].ToString().Trim() + "x" +
//        //                                        //        disp.Rows[xd]["cinterval"].ToString().Trim() + "x" +
//        //                                        //        disp.Rows[xd]["cduration"].ToString().Trim() + "  (" + disp.Rows[xd]["sp_inst"].ToString().Trim();
//        //                                        //    }
//        //                                        //    PrevMedHistoryNotes.Text += "\r\n";
//        //                                        //}
//        //                                        //PrevMedHistoryNotes.Text = PrevMedHistoryNotes.Text + string.Concat(Enumerable.Repeat("_", 80));
//        //                                        //  }
//        //}

//        //void displaySpecialInstructions()
//        //{
//        //    //retrieve special medical notes - allergies etc.
//        //    edtallergies.Text = edtspinstructions.Text = txtHmoNhisPlanType.Text = txtHMOPlanType.Text =
//        //        txtHMOPlanType.Text = "";
//        //    // PSPNOTES pspnotes = PSPNOTES.GetPSPNOTES(anc01.GROUPCODE,anc01.PATIENTNO);
//        //    //  if ( pspnotes != null)
//        //    //  {
//        //    edtallergies.Text = billchain.MEDNOTES.Trim();
//        //    edtspinstructions.Text = billchain.SPNOTES.Trim();
//        //    //  }
//        //    if (billchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(billchain.HMOSERVTYPE))
//        //    {
//        //        txtHMOPlanType.Text = billchain.HMOSERVTYPE;
//        //        DataTable HMODETAILS = Hmodetail.GetHMODETAIL(billchain.GROUPHEAD);
//        //        for (int i = 0; i < HMODETAILS.Rows.Count; i++)
//        //        {
//        //            if (HMODETAILS.Rows[i]["HMOSERVTYPE"].ToString().Trim() == billchain.HMOSERVTYPE.Trim())
//        //            {
//        //                txtHmoNhisPlanType.Text = HMODETAILS.Rows[i]["HMOSERVTYPE"].ToString().Trim();
//        //                break;
//        //            }
//        //        }
//        //    }
//        //}

//        void writePage1()
//        {
//            //check for delivery date update
//            if (chkRecordDeliveryDate.Visible && dtDeliveryDatepg1.Value.Date > dtmin_date)
//            {
//                string updstr = "update ancreg set del_date = '" + dtDeliveryDatepg1.Value.ToShortDateString() + "', operator = '" + woperator + "', dtime = '" + DateTime.Now + "' where reference = '" + anc01.REFERENCE + "'";
//                bissclass.UpdateRecords(updstr, "MR");
//            }

//            anc01.BOOKINGCATEGORY = pg1_txtgravida.Text;
//            anc01.HUSBANDBG = pg1_combHusbandBloodGroup.Text;

//            anc01.LMP = dtLMPpg1.Value.Date;
//            anc01.EDD = dtEDDpg1.Value.Date;
//            anc01.BLOODGROUP = pg1_combBloodGroup.Text;
//            anc01.DEL_DATE = dtDeliveryDatepg1.Value.Date; //if delivery date > dtmindate, we update ancreg 10.05.2014
//                                                           //header information
//            anc01.BLOODGROUP = txtbloodgroup.Text;
//            anc01.GENOTYPE = TxtGT.Text;
//            //txtEDDpg1Header.Text = (anc01.EDD.Year <= dtmin_date.Year) ? "" : anc01.EDD.ToShortDateString();
//            anc01.EDD = dtEDDpg1.Value.Date;

//            anc01.DOCTOR = string.IsNullOrWhiteSpace(pg1_combDoctor.Text) ? "" : pg1_combDoctor.SelectedValue.ToString();
//            anc01.DURATIONOFPREGNANCY = pg1_txtGestationPeriod.Text;
//            anc01.AGE = Convert.ToDecimal(pg1_TxtAge.Text);
//            anc01.TRIBE = pg1_txtTribe.Text;

//            anc01.ADDRESS = pg1_TxtAddress.Text;
//            anc01.OCCUPATION = pg1_combOccupatonWf.Text;
//            anc01.LEVELOFEDUCATION = optPrimary.Checked ? 1 : OptSecondary.Checked ? 2 :
//                OptTertiary.Checked ? 3 : 0;
//            anc01.HUSBANDNAME = pg1_txtHusbandName.Text;
//            anc01.HUSBANDOCCUPATION = pg1_combOccupationHB.Text;
//            anc01.HUSBANDEMPLOYER = pg1_txtEmployerHB.Text;
//            anc01.HUSBANDLEVELOFEDUCATION = optHusbandPrimary.Checked ? 1 : optHusbandSecondary.Checked ? 2 :
//                optHusbandTertiary.Checked ? 3 : 0;

//            anc01.HUSBANDPHONE = txthusbankphoneEmail.Text;
//            anc01.HUSBANDGC = pg1_txtgroupcodeHB.Text;
//            anc01.HUSBANDPATNO = pg1_txtpatientnoHB.Text;
//            anc01.HUSBANDBG = pg1_combHusbandBloodGroup.Text;

//            anc01.BOOKINGTAG = pg1_chklowrisk.Checked ? 1 : pg1_chkhighrisk.Checked ? 3 : pg1_chkmedium.Checked ? 2 : 0;
//            anc01.GHGROUPCODE = billchain.GHGROUPCODE;
//            anc01.OPERATOR = woperator;
//            anc01.DTIME = DateTime.Now;
//            anc01.SPNOTES = pg1_txtInstructions.Text;
//            anc01.BIRTHDATE = dtDOBpg1.Value.Date;
//            anc01.GENOTYPE = pg1_combGenotype.Text;
//            anc01.MENS_REGULARITY = pg1_TxtRegularity.Text;
//            anc01.CONTRACEPTIVEUSE = pg1_TxtContrapceptive.Text;
//            anc01.RISKFACTOR = pg1_txtAllergies.Text;
//            anc01.HUSBANDGENOTYPE = pg1_CombHusbandgenotype.Text;
//            anc01.MENARCHE = pg1_Txtmenarche.Text;
//            ANC01.anc01WriteANC(false, anc01, anc01.REFERENCE);

//        }
//        void writePage2()
//        {

//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = anc02newRecord ? "ANC02_Add" : "ANC02_Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;

//            insertCommand.Parameters.AddWithValue("@reference", "");
//            insertCommand.Parameters.AddWithValue("@patientno", billchain.PATIENTNO);
//            insertCommand.Parameters.AddWithValue("@GROUPHEAD", billchain.GROUPHEAD);
//            insertCommand.Parameters.AddWithValue("@groupcode", billchain.GROUPCODE);
//            insertCommand.Parameters.AddWithValue("@GROUPHTYPE", billchain.GROUPHTYPE);
//            insertCommand.Parameters.AddWithValue("POSTED", anc02newRecord ? false : anc02.POSTED);
//            insertCommand.Parameters.AddWithValue("POST_DATE", anc02newRecord ? DateTime.Now : anc02.POST_DATE);
//            insertCommand.Parameters.AddWithValue("TRANS_DATE", DateTime.Now);
//            insertCommand.Parameters.AddWithValue("@diabetes", pg2_TxtDiabetes.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@HYPERTENSION", pg2_TxtHypertention.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@HEART_DISEASE", pg2_txtHeartDisease.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@SICKLE_CELL", pg2_txtSickleCellDisease.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@PULMONARY", pg2_txtPulmonary.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@KIDNEYDISEASE", pg2_TxtKidney.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@HEPATITIS", pg2_txtHepatitis.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@NEUROLOGIC", pg2_txtNeurologic.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@THYROID", pg2_txtThyroid.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@PSYCHIATRIC", pg2_TxtPhychiatric.Text.Trim());
//            //end of page1
//            insertCommand.Parameters.AddWithValue("@PREV_PREG_TOTAL", anc02newRecord ? 0 : anc02.PREV_PREG_TOTAL);
//            insertCommand.Parameters.AddWithValue("@NOALIVE", anc02newRecord ? 0 : anc02.NOALIVE);
//            insertCommand.Parameters.AddWithValue("@DEPRESSION", anc02newRecord ? "" : anc02.DEPRESSION);
//            insertCommand.Parameters.AddWithValue("@VARICOSITIES", anc02newRecord ? "" : anc02.VARICOSITIES);
//            insertCommand.Parameters.AddWithValue("@D_RH_SENSITIZATION", anc02newRecord ? "" : anc02.D_RH_SENSITIZATION);
//            insertCommand.Parameters.AddWithValue("@BLOOD_TRANSFUSIONS", anc02newRecord ? "" : anc02.BLOOD_TRANSFUSIONS);
//            insertCommand.Parameters.AddWithValue("@HIV", anc02newRecord ? "" : anc02.HIV);
//            insertCommand.Parameters.AddWithValue("@BREAST_LUMPS", anc02newRecord ? "" : anc02.BREAST_LUMPS);
//            insertCommand.Parameters.AddWithValue("@GYNESURGERIES", anc02newRecord ? "" : anc02.GYNESURGERIES);
//            insertCommand.Parameters.AddWithValue("@DRUG_ALLERGIES", anc02newRecord ? "" : anc02.DRUG_ALLERGIES);
//            insertCommand.Parameters.AddWithValue("@OPERATIONS", anc02newRecord ? "" : anc02.OPERATIONS);
//            insertCommand.Parameters.AddWithValue("@ANAESTHETIC", anc02newRecord ? "" : anc02.ANAESTHETIC);
//            insertCommand.Parameters.AddWithValue("@PAPSMEAR", anc02newRecord ? "" : anc02.PAPSMEAR);
//            insertCommand.Parameters.AddWithValue("@INFERTILITY", anc02newRecord ? "" : anc02.INFERTILITY);
//            insertCommand.Parameters.AddWithValue("@OTHERS", anc02newRecord ? "" : anc02.OTHERS);
//            insertCommand.Parameters.AddWithValue("@ALCOHOL", anc02newRecord ? "" : anc02.ALCOHOL);
//            insertCommand.Parameters.AddWithValue("@SMOKING", anc02newRecord ? "" : anc02.SMOKING);
//            insertCommand.Parameters.AddWithValue("@SOCIALDETAILS", anc02newRecord ? "" : anc02.SOCIALDETAILS);
//            insertCommand.Parameters.AddWithValue("@FAM_HYPERTENSION", anc02newRecord ? "" : anc02.FAM_HYPERTENSION);
//            insertCommand.Parameters.AddWithValue("@FAM_DIABETES", anc02newRecord ? "" : anc02.FAM_DIABETES);
//            insertCommand.Parameters.AddWithValue("@FAM_SICKLE_CELL", anc02newRecord ? "" : anc02.FAM_SICKLE_CELL);
//            insertCommand.Parameters.AddWithValue("@FAM_GENETIC", anc02newRecord ? "" : anc02.FAM_GENETIC);
//            insertCommand.Parameters.AddWithValue("@FAM_OTHERS", anc02newRecord ? "" : anc02.FAM_OTHERS);
//            insertCommand.Parameters.AddWithValue("@FAMILYDETAILS", anc02newRecord ? "" : anc02.FAMILYDETAILS);
//            insertCommand.Parameters.AddWithValue("@AP_PROGUANIL", anc02newRecord ? "" : anc02.AP_PROGUANIL);
//            insertCommand.Parameters.AddWithValue("@AP_PYRIMETHAMINE1", anc02newRecord ? "" : anc02.AP_PYRIMETHAMINE1);
//            insertCommand.Parameters.AddWithValue("@AP_PYRIMETHAMINE2", anc02newRecord ? "" : anc02.AP_PYRIMETHAMINE2);
//            insertCommand.Parameters.AddWithValue("@AP_PYRIMETHAMINE3", anc02newRecord ? "" : anc02.AP_PYRIMETHAMINE3);
//            insertCommand.Parameters.AddWithValue("@AP_OTHERS", anc02newRecord ? "" : anc02.AP_OTHERS);
//            insertCommand.Parameters.AddWithValue("@TETANUS1", anc02newRecord ? "" : anc02.TETANUS1);
//            insertCommand.Parameters.AddWithValue("@TETANUS2", anc02newRecord ? "" : anc02.TETANUS2);
//            insertCommand.Parameters.AddWithValue("@TETANUS3", anc02newRecord ? "" : anc02.TETANUS3);
//            insertCommand.Parameters.AddWithValue("@RECREATIONDRGS", anc02newRecord ? "" : anc02.RECREATIONDRGS);
//            insertCommand.Parameters.AddWithValue("@TWINNING", anc02newRecord ? "" : anc02.TWINNING);

//            try
//            {
//                connection.Open();
//                insertCommand.ExecuteNonQuery();
//                MessageBox.Show("Records Saved successfully...", "MEDICAL HISTORY PAGE 1 ");

//            }
//            catch (SqlException ex)
//            {
//                // throw ex;
//                MessageBox.Show("Update ANC03A " + ex, "ANC Details", MessageBoxButtons.OK,
//                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                return;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }
//        void writePage3()
//        {
//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = anc03anewRecord ? "ANC03A_Add" : "ANC03A_Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;

//            insertCommand.Parameters.AddWithValue("@reference", pg1_txtANCReference.Text);
//            insertCommand.Parameters.AddWithValue("@patientno", pg1_txtpatientno.Text);
//            insertCommand.Parameters.AddWithValue("@groupcode", pg1_txtgroupcode.Text);
//            insertCommand.Parameters.AddWithValue("@posted", false);
//            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
//            insertCommand.Parameters.AddWithValue("@ageatedd", pg3_ChkpatientsAge.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@sicklecell", pg3_ChkSickleCell.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@downs", pg3_ChkDownSyndrome.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@chromosomal", pg3_ChkChromosonalAbnormalities.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@heartdisease", pg3_ChkCongenitalHEartDisease.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@metabolic", pg3_chkmaternalMetabolic.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@tubedefect", pg3_chkotherinheritedgenetic.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@stillbirth", pg3_chkHistoryofrecrrentprgloss.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@medications", pg3_chkVitamins.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@tb", pg3_chkTB.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@herpes", pg3_chkherpes.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@viralillness", pg3_chkviralillnes.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@sti", pg3_chkSTD.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@hepatitisb", pg3_ChkHepatitis.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@indexpreg", pg3_txtIndexpregnancy.Text.Trim());
//            insertCommand.Parameters.AddWithValue("@MEDICATIONDETL", pg3_txtMedicationDtl.Text);
//            insertCommand.Parameters.AddWithValue("@TRANS_DATE", DateTime.Now.Date);

//            try
//            {
//                connection.Open();
//                insertCommand.ExecuteNonQuery();
//                MessageBox.Show("Records Saved successfully...", "GENERIC SCREENING / TERATOLOGY COUNSELING ");

//            }
//            catch (SqlException ex)
//            {
//                // throw ex;
//                MessageBox.Show("Update ANC03A " + ex, "ANC Details", MessageBoxButtons.OK,
//                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                return;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }
//        void writePage4()
//        {
//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = anc04newRecord ? "ANC04_Add" : "ANC04_Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;
//            /*replace weight WITH STR(vstata.weight,6,2)+'kg',;
//								urine WITH vstata.others,;
//								height WITH STR(vstata.hight,6,2)+'mtr',;
//								bp WITH Vstata.bpsitting*/
//            insertCommand.Parameters.AddWithValue("@reference", pg1_txtANCReference.Text);
//            insertCommand.Parameters.AddWithValue("@posted", false);
//            insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
//            insertCommand.Parameters.AddWithValue("@TRANS_DATE", dteAssesmentDate.Value.Date);
//            insertCommand.Parameters.AddWithValue("@HEIGHT", TxtHeight.Text);
//            insertCommand.Parameters.AddWithValue("@WEIGHT", TxtWeight.Text);
//            insertCommand.Parameters.AddWithValue("@BP", vstata.BPSITTING);
//            insertCommand.Parameters.AddWithValue("@TEMP", txtPulse.Text);
//            insertCommand.Parameters.AddWithValue("@PULSE", txtPulse.Text);
//            insertCommand.Parameters.AddWithValue("@RESPIRATION", txtRespiration.Text);
//            insertCommand.Parameters.AddWithValue("@HEENT", heentAbnormal.Checked ? 2 : heentNormal.Checked ? 1 : 0);
//            insertCommand.Parameters.AddWithValue("@FUNDI", FundiNormal.Checked ? 1 : FundiAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@TEETH", TeethNormal.Checked ? 1 : TeethAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@THYROID", ThyroidNormal.Checked ? 1 : ThyroidAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@BREASTS", BreastNormal.Checked ? 1 : BreastAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@LUNGS", LungsNormal.Checked ? 1 : LungsAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@HEART", HeartNormal.Checked ? 1 : HeartAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@ABDOMEN", AbdomenNormal.Checked ? 1 : AbdomenAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@EXTREMITIES", ExtremitiesNormal.Checked ? 1 : ExtremitiesAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@SKIN", SkinNormal.Checked ? 1 : SkinAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@LYMPHNODES", LymphNormal.Checked ? 1 : LymphAbnormal.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@VULVA", pg4_optVulvaNormal.Checked ? 1 : pg4_optVulvaCondyloma.Checked ? 2 :
//                pg4_OptVulvaLesions.Checked ? 3 : 0);
//            insertCommand.Parameters.AddWithValue("@VAGINA", pg4_optVaginaNormal.Checked ? 1 : pg4_OptDischargeVagina.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@CERVIX", pg4_OptCervixNormal.Checked ? 1 : pg4_optInflamationNormal.Checked ? 2 : pg4_OptLesionNormal.Checked ? 3 : 0);
//            insertCommand.Parameters.AddWithValue("@UTERINESIZE", pg4_txtuterussize.Text);
//            insertCommand.Parameters.AddWithValue("@FIBROIDS", pg4_optFibroidYes.Checked ? 1 : pg4_optFibroidNo.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@ADNEXA", pg4_optnormalAnexa.Checked ? 1 : pg4_OPtabnormalAdnexa.Checked ? 2 : 0);
//            insertCommand.Parameters.AddWithValue("@HAEMORRHOIDS", pg4_optHaemorrhoidsYes.Checked ? 1 : pg4_optHaemorrhoidsoptNo.Checked ? 2 : 0);

//            insertCommand.Parameters.AddWithValue("@COMMENTS", pg4_TxtComments.Text);
//            insertCommand.Parameters.AddWithValue("@DELPLAN", pg4_TxtPlan.Text);

//            insertCommand.Parameters.AddWithValue("@INTERVIEWER", pg4_TxtInterviewdoneBy.Text);


//            try
//            {
//                connection.Open();
//                insertCommand.ExecuteNonQuery();
//                MessageBox.Show("Records Saved successfully...", "INITIAL PHYSICAL EXAMS BY DOCTOR ");

//            }
//            catch (SqlException ex)
//            {
//                // throw ex;
//                MessageBox.Show("Update ANC03A " + ex, "ANC Details", MessageBoxButtons.OK,
//                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                return;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }
//        void writePage5()
//        {
//            DataGridViewRow dgv = dataGridView1.Rows[currentPage5visitrecno];
//            if (dataGridView1.Rows[currentPage5visitrecno].Cells[12].Value == null)
//            {
//                DialogResult result = MessageBox.Show("Next Visit Date Not specified...CONTINUE ?", "ANC Details", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                if (result == DialogResult.No)
//                    return;
//            }
//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = anc06newRecord ? "ANC06_Add" : "ANC06_Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;

//            insertCommand.Parameters.AddWithValue("@reference", pg1_txtANCReference.Text);
//            insertCommand.Parameters.AddWithValue("@groupcode", anc01.GROUPCODE);
//            insertCommand.Parameters.AddWithValue("@patientno", anc01.PATIENTNO);
//            insertCommand.Parameters.AddWithValue("@posted", false);
//            insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
//            insertCommand.Parameters.AddWithValue("@trans_date", Convert.ToDateTime(dgv.Cells[0].Value));
//            insertCommand.Parameters.AddWithValue("@hight_of_fundus", dgv.Cells[2].Value == null ? "" : dgv.Cells[2].Value.ToString());
//            insertCommand.Parameters.AddWithValue("@PRESENTATION_POSITION", dgv.Cells[3].Value == null ? "" : dgv.Cells[3].Value.ToString());
//            insertCommand.Parameters.AddWithValue("@relation_of_pp_tobrim", dgv.Cells[4].Value == null ? "" : dgv.Cells[4].Value.ToString());
//            insertCommand.Parameters.AddWithValue("@foetal_heart", dgv.Cells[5].Value == null ? "" : dgv.Cells[5].Value.ToString());
//            insertCommand.Parameters.AddWithValue("@urine", dgv.Cells[6].Value == null ? "" : dgv.Cells[6].Value.ToString());
//            insertCommand.Parameters.AddWithValue("@blood_pressure", dgv.Cells[7].Value.ToString());
//            insertCommand.Parameters.AddWithValue("@weight", dgv.Cells[8].Value.ToString());
//            insertCommand.Parameters.AddWithValue("@hb", "");
//            insertCommand.Parameters.AddWithValue("@oedema", "");
//            insertCommand.Parameters.AddWithValue("@remarks_treatment", dgv.Cells[9].Value.ToString().Trim());
//            insertCommand.Parameters.AddWithValue("@nextvisit", dgv.Cells[12].Value == null || string.IsNullOrWhiteSpace(dgv.Cells[12].Value.ToString()) ? dtmin_date : Convert.ToDateTime(dgv.Cells[12].Value));
//            insertCommand.Parameters.AddWithValue("@doctor", mdoctor);
//            insertCommand.Parameters.AddWithValue("@nnv", dgv.Cells[12].Value == null ? 0m : Convert.ToDecimal(dgv.Cells[11].Value));
//            insertCommand.Parameters.AddWithValue("@attendref", txtConsultReference.Text);
//            insertCommand.Parameters.AddWithValue("@gestationalage", dgv.Cells[1].Value == null ? "" : dgv.Cells[1].Value.ToString().Trim());
//            /*         try
//                     {*/
//            connection.Open();
//            insertCommand.ExecuteNonQuery();
//            MessageBox.Show("Records Saved successfully...", "ANTE-NATA VISIT RECORDS ");
//            /*          }
//                      catch (SqlException ex)
//                      {
//                          // throw ex;
//                          MessageBox.Show("Update ANC06 " + ex, "ANC Details", MessageBoxButtons.OK,
//                              MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                          return;
//                      }
//                      finally
//                      {*/
//            connection.Close();
//            writePage5Medhist();
//            //}
//        }
//        void writePage6()
//        {
//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = anc04newRecord ? "ANC07_Add" : "ANC07_Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;

//            insertCommand.Parameters.AddWithValue("@Anc07Group", "1");
//            insertCommand.Parameters.AddWithValue("@reference", pg1_txtANCReference.Text);
//            insertCommand.Parameters.AddWithValue("@posted", false);
//            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
//            insertCommand.Parameters.AddWithValue("@admitted", false);
//            insertCommand.Parameters.AddWithValue("@consobs", pg6_txtConsultantOb.Text);

//            insertCommand.Parameters.AddWithValue("@attendpaed", pg6_txtAttendingPaediatrician.Text);
//            insertCommand.Parameters.AddWithValue("@deldate", pg6_dtpDeliverydate.Value);
//            insertCommand.Parameters.AddWithValue("@deltime", pg6_txtBabyDeliveredTIme.Text);
//            insertCommand.Parameters.AddWithValue("@DELSUITE", pg6_txtDeliverySuite.Text);
//            insertCommand.Parameters.AddWithValue("@parity", pg6_txtParity.Text);
//            insertCommand.Parameters.AddWithValue("@gestage", pg6_txtGestationAge.Text);
//            insertCommand.Parameters.AddWithValue("@ fetalnumber", pg6_txtFetalNumber.Text);
//            insertCommand.Parameters.AddWithValue("@lo_none", pg6_chkNoneLaborOnset.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@lo_spontaneous", pg6_chkSpontaneous.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@lo_induced", pg6_ChkInduced.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@lo_augumentd", pg6_ChkAugumented.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@indicantions", pg6_TxtIndicationLaborOnset.Text);
//            insertCommand.Parameters.AddWithValue("@romdate", pg6_dtruptureDate.Value);
//            insertCommand.Parameters.AddWithValue("@romtime", pg6_dtruptureTime.Text);
//            insertCommand.Parameters.AddWithValue("@rom_induced", pg6_chkMemInduced.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@rom_indications", pg6_TxtMemRuptIndication.Text);
//            insertCommand.Parameters.AddWithValue("@rom_duration", pg6_txtRuptMemDuration.Text);
//            insertCommand.Parameters.AddWithValue("@ROM_ACTIFICIAL", pg6_chkArtificial.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@pr_none", pg6_chkPRNone.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@pr_narcotics", pg6_chkPRNarcotics.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@pr_prudendal", pg6_chkPRPurdendal.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@pr_entonox", pg6_chkPREntonox.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@pr_epidural", pg6_chkPREpidural.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@pr_spinal", pg6_chkPRSpinal.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@PR_COMBINED", pg6_chkPRCombinedES.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@labonsetdt", pg6_dtpOnset.Value);
//            insertCommand.Parameters.AddWithValue("@labpnsettime", pg6_OnsetTime.Text);
//            insertCommand.Parameters.AddWithValue("@labfddt", pg6_dtpFullyDilated.Value);
//            insertCommand.Parameters.AddWithValue("@labfdtime", pg6_FullDilatedTime.Text);
//            insertCommand.Parameters.AddWithValue("@labpcdt", pg6_dtpPushingCommenced.Value);
//            insertCommand.Parameters.AddWithValue("@labpctime", pg6_PushingCommencedTIme.Text);
//            insertCommand.Parameters.AddWithValue("@labhddt", pg6_dtpHeadDelivered.Value);
//            insertCommand.Parameters.AddWithValue("@labhdtime", pg6_HeadDeliveredTIme.Text);
//            insertCommand.Parameters.AddWithValue("@labbddt", pg6_dtpBabyDelivered.Value);
//            insertCommand.Parameters.AddWithValue("@labbdtime", pg6_BabyDeliveredTIme.Text);
//            insertCommand.Parameters.AddWithValue("@labeotsdt", pg6_dtpEndofThirdStage.Value);
//            insertCommand.Parameters.AddWithValue("@labeotstime", pg6_EndofThirdStageTime.Text);
//            insertCommand.Parameters.AddWithValue("@labt2ddt", pg6_dtpTwinDel.Value);
//            insertCommand.Parameters.AddWithValue("@lab2dtime", pg6_TwinDeliveredTime.Text);
//            insertCommand.Parameters.AddWithValue("@fststagehirmin", pg6_TxtFirstStage.Text);
//            insertCommand.Parameters.AddWithValue("@sststagehrmin", pg6_TxtSecondStage.Text);
//            insertCommand.Parameters.AddWithValue("@tststagehrmin", pg6_TxtThirdStage.Text);
//            insertCommand.Parameters.AddWithValue("@labduration", Pg6_LaborDuration.Text);

//            insertCommand.Parameters.AddWithValue("stagem_a", pg6_chktstActive.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@tstagem_m", pg6_chktstmanualremoval.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@tstagemgtnotes", pg6_TxttstCommentsIndications.Text);
//            insertCommand.Parameters.AddWithValue("@oxytocics", pg6_chkOxytocin.Checked);
//            insertCommand.Parameters.AddWithValue("@egometrine", pg6_chkOxyErgometrine.Checked);
//            insertCommand.Parameters.AddWithValue("@oxytocicsdim", pg6_txtOxydosageAndTime.Text);
//            insertCommand.Parameters.AddWithValue("@cord", TxtCordNoVessels.Text);
//            insertCommand.Parameters.AddWithValue("@membranes", optMembApparenntlyComplete.Checked ? 1m :
//                optMembIncomplete.Checked ? 2m : 0);
//            insertCommand.Parameters.AddWithValue("@placenta", pg6_optIncompletePlacenta.Checked ? 2 :
//                pg6_optcompletePlacenta.Checked ? 1 : 0);
//            insertCommand.Parameters.AddWithValue("@blmeasure", pg6_TxtMeasuredBloodLoss.Text);
//            insertCommand.Parameters.AddWithValue("@blestimates", pg6_TxtEstimatedBloodloss.Text);
//            insertCommand.Parameters.AddWithValue("@bltotal", pg6_TxtTotalBloodloss.Text);
//            insertCommand.Parameters.AddWithValue("@furtheractn", pg6_txtFurtherAction.Text);

//            try
//            {
//                connection.Open();
//                insertCommand.ExecuteNonQuery();
//                MessageBox.Show("Records Saved successfully...", "DELIVERY RECORDS ");

//            }
//            catch (SqlException ex)
//            {
//                // throw ex;
//                MessageBox.Show("Update ANC07 " + ex, "DELIVERY RECORDS ", MessageBoxButtons.OK,
//                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                return;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }
//        void writePage7()
//        {
//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = anc07newRecord ? "ANC07_Add" : "ANC07_Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;

//            insertCommand.Parameters.AddWithValue("@Anc07Group", "2");
//            insertCommand.Parameters.AddWithValue("@reference", pg1_txtANCReference.Text);

//            insertCommand.Parameters.AddWithValue("@TRAUMA_NONE", Pg7_ChkNonIdentified.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@CERVICAL_TEAR", Pg7_chkCervicalTear.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@PERINEAL_TEAR", Pg7_ChkPerinealTear.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@TEARDEGREE", Pg7_optFirstDeg.Checked ? 1m :
//                Pg7_optSecondDeg.Checked ? 2m : Pg7_optThirdDeg.Checked ? 3m : Pg7_optFourthDeg.Checked ? 4m : 0);
//            insertCommand.Parameters.AddWithValue("@EPISIOTOMY", Pg7_chkEpisotomy.Checked ? true : false);
//            insertCommand.Parameters.AddWithValue("@INDI4EPISIOTOMY", Pg7_txtindicationforepisotomy.Text);
//            insertCommand.Parameters.AddWithValue("@REPREQ", Pg7_OptRepairYes.Checked ? 1m : Pg7_optRepairNo.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@MOTHERAGREE", Pg7_optMothRYes.Checked ? 1m : Pg7_optMotherRNo.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@ANAESTHUSED", Pg7_txtAnaestheticUsed.Text);
//            insertCommand.Parameters.AddWithValue("@STAFF", Pg7_txtSignature.Text);
//            insertCommand.Parameters.AddWithValue("@TRDTTIME", Pg7_AnaextheticDate.Value.Date);
//            insertCommand.Parameters.AddWithValue("@TRDTTIME", Pg7_AnaestheticTime.Text);
//            insertCommand.Parameters.AddWithValue("@BAB1BY", Pg7_TxtBaby1delivered.Text);
//            insertCommand.Parameters.AddWithValue("@BAB2BY", Pg7_TxtBaby2delivered.Text);
//            insertCommand.Parameters.AddWithValue("@BAB3BY", Pg7_TxtBaby3delivered.Text);
//            insertCommand.Parameters.AddWithValue("@COMMENTS", Pg7_commentsActions.Text);

//            try
//            {
//                connection.Open();
//                insertCommand.ExecuteNonQuery();
//                MessageBox.Show("Records Saved successfully...", "DELIVERY RECORDS ");

//            }
//            catch (SqlException ex)
//            {
//                // throw ex;
//                MessageBox.Show("Update ANC07 " + ex, "DELIVERY RECORDS ", MessageBoxButtons.OK,
//                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                return;
//            }
//            finally
//            {
//                connection.Close();
//            }
//            writePage7b();
//        }
//        void writePage7b()
//        {
//            SqlConnection connection = new SqlConnection(); connection = msmr.Dataaccess.mrConnection();
//            connection.Open();
//            DataGridViewRow dgv;
//            for (int i = 0; i < dataGridViewPg7.Rows.Count - 1; i++)
//            {
//                if (dataGridViewPg7.Rows[i].Cells[0].Value == null)
//                    continue;
//                dgv = dataGridView1.Rows[i];
//                SqlCommand insertCommand = new SqlCommand();
//                if (dgv.Cells[12].Value != null && dgv.Cells[12].Value.ToString().Trim() == "OLDREC")
//                {
//                    insertCommand.CommandText = "ANC07b_update";
//                }
//                else
//                {
//                    insertCommand.CommandText = "ANC07b_Add";
//                }
//                insertCommand.Connection = connection;
//                insertCommand.CommandType = CommandType.StoredProcedure;

//                insertCommand.Parameters.AddWithValue("@reference", anc01.REFERENCE);
//                insertCommand.Parameters.AddWithValue("@posted", false);
//                insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
//                insertCommand.Parameters.AddWithValue("@trans_date", Convert.ToDateTime(dgv.Cells[0].Value));
//                insertCommand.Parameters.AddWithValue("@temp", dgv.Cells[1].Value == null ? "" : dgv.Cells[1].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@pr", dgv.Cells[2].Value == null ? "" : dgv.Cells[2].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@bp", dgv.Cells[3].Value == null ? "" : dgv.Cells[3].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@sp02", dgv.Cells[4].Value == null ? "" : dgv.Cells[4].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@uterus", dgv.Cells[5].Value == null ? "" : dgv.Cells[5].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@locia", dgv.Cells[6].Value == null ? "" : dgv.Cells[6].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@wounds", dgv.Cells[7].Value == null ? "" : dgv.Cells[7].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@perineum", dgv.Cells[8].Value == null ? "" : dgv.Cells[8].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@urine", dgv.Cells[9].Value == null ? "" : dgv.Cells[9].Value.ToString());
//                insertCommand.Parameters.AddWithValue("@staffsign", dgv.Cells[10].Value == null ? "" : dgv.Cells[10].Value.ToString());

//                insertCommand.ExecuteNonQuery();
//            }
//            connection.Close();
//            return;
//        }
//        void writePage8()
//        {
//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = anc07anewRecord ? "ANC07a_Add" : "ANC07a_Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;

//            insertCommand.Parameters.AddWithValue("@reference", anc01.REFERENCE);
//            insertCommand.Parameters.AddWithValue("@posted", false);
//            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
//            insertCommand.Parameters.AddWithValue("@gestage", Pg8_TxtGesAge.Text);
//            insertCommand.Parameters.AddWithValue("@parity", Pg8_TxtParity.Text);
//            insertCommand.Parameters.AddWithValue("@trans_date", Pg8_trans_date.Value);
//            insertCommand.Parameters.AddWithValue("@process", Pg8_txtProcess.Text);
//            insertCommand.Parameters.AddWithValue("@indications", Pg8_txtIndications.Text);
//            insertCommand.Parameters.AddWithValue("@staffpresent", Pg8_txtStaffPresent.Text);
//            insertCommand.Parameters.AddWithValue("@surgeon", Pg8_TxtSurgeon.Text);
//            insertCommand.Parameters.AddWithValue("@assistant", Pg8_txtAssistant.Text);
//            insertCommand.Parameters.AddWithValue("@paediatrician", Pg8_txtPaediatricians.Text);
//            insertCommand.Parameters.AddWithValue("@midwives", Pg8_txtMidwives.Text);
//            insertCommand.Parameters.AddWithValue("@naesthetist", Pg8_txtAnaesthetist.Text);
//            insertCommand.Parameters.AddWithValue("@others", Pg8_txtOthers.Text);
//            insertCommand.Parameters.AddWithValue("@anaesthesia", Pg8_txtAnaethesia.Text);
//            insertCommand.Parameters.AddWithValue("@findings", Pg8_txtFindings.Text);
//            insertCommand.Parameters.AddWithValue("@procedurenote", Pg8_txtProcedure.Text);
//            insertCommand.Parameters.AddWithValue("@mother", Pg8_txtMother.Text);
//            insertCommand.Parameters.AddWithValue("@baby", Pg8_txtBaby.Text);
//            insertCommand.Parameters.AddWithValue("@staffsign", Pg8_txtStaffSign.Text);

//            try
//            {
//                connection.Open();
//                insertCommand.ExecuteNonQuery();
//                MessageBox.Show("Records Saved successfully...", "OPERATIVE DELIVERY RECORD ");

//            }
//            catch (SqlException ex)
//            {
//                // throw ex;
//                MessageBox.Show("Update ANC07a " + ex, "OPERATIVE DELIVERY RECORD ", MessageBoxButtons.OK,
//                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                return;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }
//        void writePage9()
//        {
//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = anc07cnewRecord ? "ANC07c_Add" : "ANC07c.Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;

//            insertCommand.Parameters.AddWithValue("@reference", pg1_txtANCReference.Text);
//            //     insertCommand.Parameters.AddWithValue("@groupcode", anc01.GROUPCODE);
//            //    insertCommand.Parameters.AddWithValue("@patientno", anc01.PATIENTNO);
//            insertCommand.Parameters.AddWithValue("@posted", false);
//            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
//            insertCommand.Parameters.AddWithValue("@bweight", pg9_txtBirthWeight.Text);
//            insertCommand.Parameters.AddWithValue("@sex", pg9_combSex.Text);
//            insertCommand.Parameters.AddWithValue("@birthtype", pg9_chkSingleton.Checked ? 1m : pg9_chkMultiple.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@gestage", pg9_txtGestaAge.Text);
//            insertCommand.Parameters.AddWithValue("@modeofresusc", pg9_txtModeofResuscitation.Text);
//            insertCommand.Parameters.AddWithValue("@drugs", pg9_txtDrugs.Text);
//            insertCommand.Parameters.AddWithValue("@trans_date", pg9_dtPhysicalExamDate.Value);
//            insertCommand.Parameters.AddWithValue("@examtime", pg9_txtTime.Text);
//            insertCommand.Parameters.AddWithValue("@hr", pg9_txtHR.Text);
//            insertCommand.Parameters.AddWithValue("@rr", pg9_txtRR.Text);
//            insertCommand.Parameters.AddWithValue("@temp", pg9_txtTemp.Text);
//            insertCommand.Parameters.AddWithValue("@ofc", pg9_txtOFC.Text);
//            insertCommand.Parameters.AddWithValue("@length", pg9_txtLength.Text);
//            insertCommand.Parameters.AddWithValue("@palor", pg9_txtPalor.Text);
//            insertCommand.Parameters.AddWithValue("@cyanosis", pg9_txtCyanosis.Text);
//            insertCommand.Parameters.AddWithValue("@JAUNDICE", pg9_txtJaudice.Text);
//            insertCommand.Parameters.AddWithValue("@R_DISTRESS", pg9_txtRespDistress.Text);
//            insertCommand.Parameters.AddWithValue("@drugs", pg9_txtDrugs.Text);
//            insertCommand.Parameters.AddWithValue("@appearance", pg9_chkApperanceAbNormal.Checked ? 1m :
//                pg9_chkApperanceNormal.Checked ? 1m : 0m);
//            insertCommand.Parameters.AddWithValue("@appearancenote", pg9_chkApperanceDetails.Text);
//            insertCommand.Parameters.AddWithValue("@head", pg9_chkHeadNormal.Checked ? 1m : pg9_chkHeadAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@headnote", pg9_chkHeadDetails.Text);

//            insertCommand.Parameters.AddWithValue("@ears", pg9_chkEarsNormal.Checked ? 1m : pg9_chkEarsAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@earsnote", pg9_chkEarsDetails.Text);
//            insertCommand.Parameters.AddWithValue("@eyes", pg9_chkEyesNormal.Checked ? 1m : pg9_chkEyesAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@eyesnote", pg9_chkEyesDetails.Text);
//            insertCommand.Parameters.AddWithValue("@nose", pg9_chkNoseNormal.Checked ? 1m : pg9_chkNoseAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@nosenote", pg9_chkNoseDetails.Text);
//            insertCommand.Parameters.AddWithValue("@mouth", pg9_chkMouthNormal.Checked ? 1m : pg9_chkMouthAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@mouthnote", pg9_chkMouthDetails.Text);
//            insertCommand.Parameters.AddWithValue("@respsystem", pg9_chkRespNormal.Checked ? 1m : pg9_chkRespAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@respsystemnote", pg9_chkRespDetails.Text);
//            insertCommand.Parameters.AddWithValue("@cardiosystem", pg9_chkCadiovascularNormal.Checked ? 1m :
//               pg9_chkCadiovascularAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@cardiosystemnote", pg9_chkCadiovascularDetails.Text);
//            insertCommand.Parameters.AddWithValue("@abdomen", pg9_chkAbdomenNormal.Checked ? 1m : pg9_chkAbdomenAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@abdomennote", pg9_chkAbdomenDetails.Text);
//            insertCommand.Parameters.AddWithValue("@femorals", pg9_chkFemoralsNormal.Checked ? 1m : pg9_chkFemoralsAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@femoralsnote", pg9_chkFemoralsDetails.Text);
//            insertCommand.Parameters.AddWithValue("@anus", pg9_chkAnusNormal.Checked ? 1m : pg9_chkAnusAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@anusnote", pg9_chkAnusDetails.Text);
//            insertCommand.Parameters.AddWithValue("@genitalia", pg9_chkGenitaliaNormal.Checked ? 1m : pg9_chkGenitaliaAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@genitalianote", pg9_chkGenitaliaDetails.Text);
//            insertCommand.Parameters.AddWithValue("@extemites", pg9_chkExtremitesNormal.Checked ? 1m : pg9_chkExtremitesAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@extremitesnote", pg9_chkExtremitesDetails.Text);
//            insertCommand.Parameters.AddWithValue("@hips", pg9_chkHipsNormal.Checked ? 1m : pg9_chkHipsAbNormal.Checked ? 2m : 0m);
//            insertCommand.Parameters.AddWithValue("@hipsnote", pg9_chkHipsDetails.Text);
//            insertCommand.Parameters.AddWithValue("@cns", pg9_chkCentralNervousNormal.Checked ? 1m : pg9_chkCentralNervousAbNormal.Checked ? 2m : 0);
//            insertCommand.Parameters.AddWithValue("@hipsnote", pg9_chkCentralNervousDetails.Text);
//            insertCommand.Parameters.AddWithValue("@otherfindings", pg9_txtOtherFindings.Text);
//            insertCommand.Parameters.AddWithValue("@comments", pg9_txtProblemsandComments.Text);
//            insertCommand.Parameters.AddWithValue("@staffsign", Pg8_txtStaffSign.Text);
//            insertCommand.Parameters.AddWithValue("@medhistupdated", false);
//            try
//            {
//                connection.Open();
//                insertCommand.ExecuteNonQuery();
//                MessageBox.Show("Records Saved successfully...", "BABY'S RECORD");

//            }
//            catch (SqlException ex)
//            {
//                // throw ex;
//                MessageBox.Show("Update ANC07a " + ex, "BABY'S RECORD", MessageBoxButtons.OK,
//                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                return;
//            }
//            finally
//            {
//                connection.Close();
//            }
//            updateApgarScore();
//        }
//        void updateApgarScore()
//        {
//            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//            SqlCommand insertCommand = new SqlCommand();
//            insertCommand.CommandText = apgarscorenewRecord ? "APGARSCORE_Add" : "APGARSCORE.Update";
//            insertCommand.Connection = connection;
//            insertCommand.CommandType = CommandType.StoredProcedure;

//            insertCommand.Parameters.AddWithValue("@reference", pg1_txtANCReference.Text);
//            insertCommand.Parameters.AddWithValue("@groupcode", anc01.GROUPCODE);
//            insertCommand.Parameters.AddWithValue("@patientno", anc01.PATIENTNO);
//            insertCommand.Parameters.AddWithValue("@posted", false);
//            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
//            insertCommand.Parameters.AddWithValue("@heart_rate_1", pg9_txtHeartRate_1.Text);
//            insertCommand.Parameters.AddWithValue("@respirationeff_1", pg9_txtRespEffort_1.Text);
//            insertCommand.Parameters.AddWithValue("@muscle_tone_1", pg9_txtMuscleTone_1.Text);
//            insertCommand.Parameters.AddWithValue("@reflex_1", pg9_txtReflex_1.Text);
//            insertCommand.Parameters.AddWithValue("@color_1", pg9_txtColor_1.Text);
//            insertCommand.Parameters.AddWithValue("@total_1", pg9_txtTotal_1.Text);
//            insertCommand.Parameters.AddWithValue("@heart_rate_5", pg9_txtHeartRate_5.Text);
//            insertCommand.Parameters.AddWithValue("@muscle_tone_5", pg9_txtMuscleTone_5.Text);
//            insertCommand.Parameters.AddWithValue("@reflex_5", pg9_txtReflex_5.Text);
//            insertCommand.Parameters.AddWithValue("@color_5", pg9_txtColor_5.Text);
//            insertCommand.Parameters.AddWithValue("@total_5", pg9_txtTotal_5.Text);
//            insertCommand.Parameters.AddWithValue("@heart_rate_oth", pg9_txtHeartRate_oth.Text);
//            insertCommand.Parameters.AddWithValue("@respirationeff_oth", pg9_txtRespEffort_oth.Text);
//            insertCommand.Parameters.AddWithValue("@muscle_tone_oth", pg9_txtMuscleTone_oth.Text);
//            insertCommand.Parameters.AddWithValue("@reflex_oth", pg9_txtReflex_oth.Text);
//            insertCommand.Parameters.AddWithValue("@color_oth", pg9_txtColor_oth.Text);
//            insertCommand.Parameters.AddWithValue("@total_oth", pg9_txtTotal_oth.Text);
//            insertCommand.Parameters.AddWithValue("@operator", woperator);
//            insertCommand.Parameters.AddWithValue("@dtime", DateTime.Now);

//            try
//            {
//                connection.Open();
//                insertCommand.ExecuteNonQuery();
//                MessageBox.Show("Records Saved successfully...", "APGARSCORE RECORD");

//            }
//            catch (SqlException ex)
//            {
//                // throw ex;
//                MessageBox.Show("Update " + ex, "APGARSCORE RECORD", MessageBoxButtons.OK,
//                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                return;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }
//        void writePage5Medhist()
//        {
//            //update medhist
//            DateTime xdate = Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[0].Value).Date;
//            string rtnstring = "==> Ante-Natal Records - " + DateTime.Now.ToString("dd-MM-yyyy @ HH:mm:ss");
//            MedHist medhist = MedHist.GetMEDHIST(anc01.GROUPCODE, anc01.PATIENTNO, "", false, true, xdate, "DESC");
//            bool newhist = (medhist == null) ? true : false;
//            string xcomments = "";
//            if (newhist)
//                xcomments = rtnstring + "\r\n";
//            else
//            {
//                xcomments = medhist.COMMENTS.Trim() + "\r\n" + rtnstring.Trim();
//            }
//            xcomments += dataGridView1.Rows[currentPage5visitrecno].Cells[9].Value != null ?
//                dataGridView1.Rows[currentPage5visitrecno].Cells[9].Value.ToString().Trim() : "";
//            MedHist.updatemedhistcomments(anc01.GROUPCODE, anc01.PATIENTNO, xdate, xcomments, newhist, txtConsultReference.Text, pg1_TxtName.Text, anc01.GHGROUPCODE, anc01.GROUPHEAD, mdoctor);

//            //update global appointment register if current transaction and Anc06.nextvisit > date()
//            if (Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[0].Value) == DateTime.Now.Date &&
//                Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[12].Value) > DateTime.Now.Date)
//            {
//                string docemail, docphone, docname; docemail = docphone = docname = "";
//                DataTable dt = Dataaccess.GetAnytable("", "MR", "select email, phone, name from doctors where reference = '" + mdoctor + "'", false);
//                if (dt.Rows.Count > 0)
//                {
//                    docemail = dt.Rows[0]["email"].ToString();
//                    docphone = dt.Rows[0]["phone"].ToString();
//                    docname = dt.Rows[0]["name"].ToString();
//                }
//                string[] schpara_ = new string[8]; schpara_[0] = docname == "" ? dataGridView1.Rows[currentPage5visitrecno].Cells[13].Value.ToString().Trim() : docname; schpara_[1] = billchain.NAME + "\r\n" + billchain.GROUPCODE.Trim() + ":" + billchain.PATIENTNO.Trim() + " Ante-Natal Care/Visit"; schpara_[2] = "FOLLOW-UP";
//                schpara_[3] = docphone; schpara_[4] = docemail; schpara_[5] = billchain.NAME; schpara_[6] = billchain.PHONE;
//                schpara_[7] = billchain.EMAIL;

//                string T = Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[12].Value.ToString()).ToShortDateString();
//                T = T + " " + DateTime.Now.ToLongTimeString();
//                DateTime ts = Convert.ToDateTime(T);

//                APPT.writeApptment(true, ts, DateTime.Now.ToLongTimeString(), ts.AddMinutes(30).ToLongDateString(), manccode, "", "", "", false, schpara_);

//            }
//            if (mdoc_seclevel >= 5)
//            {
//                updateMedhistProtect();
//                pg4_chkProtect.Checked = pg4_chkEncrypt.Checked = false;
//            }
//            updatelastvisit();
//            if (msection == "4" && Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[0].Value) == DateTime.Now.Date)
//            {
//                LINK.WriteLINK(0, anc01.GROUPCODE, anc01.PATIENTNO, anc01.NAME, "7", txtConsultReference.Text, 0m, 0m, mrattend.CLINIC, true, mdoctor, false, 0, "", "4", woperator);
//            }
//            //CLEAR LINK
//            string updatestring = "UPDATE LINK SET DATEREC = '" + DateTime.Now.ToShortDateString() + "', TIMEREC = '" + DateTime.Now.ToLongTimeString() + "' WHERE reference = '" + txtConsultReference.Text + "' and tosection = '4' ";
//            bissclass.UpdateRecords(updatestring, "MR");
//            //22.08.2019 update backup file. To hold all medhist saved. Med Hist Saved (20.08.2019) for a patient at Harmony vanished.
//            MedHist.UpdateMedHistBackup(anc01.GROUPCODE, anc01.PATIENTNO, xdate, xcomments, woperator, txtConsultReference.Text);
//        }
//        void updatelastvisit()
//        {
//            string reference = pg1_txtANCReference.Text;
//            DateTime xdate = Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[0].Value), xnextvisit = dataGridView1.Rows[currentPage5visitrecno].Cells[12].Value == null || string.IsNullOrWhiteSpace(dataGridView1.Rows[currentPage5visitrecno].Cells[12].Value.ToString()) ? dtmin_date : Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[12].Value);
//            string xstr = "UPDATE Anc01 SET lastattend = '" + xdate + "', nextvisit = '" + xnextvisit + "' WHERE Reference = '" + reference + "'";
//            bissclass.UpdateRecords(xstr, "MR");
//            /*      try
//                  {

//                      SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//                      connection.Open();
//                      SqlCommand updateCommand = new SqlCommand("UPDATE Anc01 SET lastattend = @lastattend WHERE Reference=@reference", connection);

//                      updateCommand.Connection = connection;
//                      updateCommand.Parameters.AddWithValue("@Reference", reference);
//                      updateCommand.Parameters.AddWithValue("@Lastattend", xdate);

//                      int rows = updateCommand.ExecuteNonQuery();
//                      connection.Close();
//                  }
//                  catch (SqlException ex)
//                  {
//                      MessageBox.Show("Update ANC01 Last Visit " + ex, "ANC Details", MessageBoxButtons.OK,
//                          MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                      return;
//                  }*/
//        }
//        void updateMedhistProtect()
//        {
//            string xgc = anc01.GROUPCODE, xpat = anc01.PATIENTNO;
//            DateTime xdate = Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[0].Value);
//            int protectlevel = pg4_chkProtect.Checked ? mdoc_seclevel : 0;
//            bool enchrypted = pg4_chkEncrypt.Checked ? true : false;
//            string xstr = "UPDATE Medhist SET protected = '" + protectlevel + "', enchrypted = '" + enchrypted + "' WHERE GROUPCODE = '" + xgc + "' and Patientno = '" + xpat + "' AND TRANS_DATE = '" + xdate + "'";
//            bissclass.UpdateRecords(xstr, "MR");
//            /*    try
//                {
//                    SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
//                    connection.Open();
//                    SqlCommand updateCommand = new SqlCommand("UPDATE Medhist SET Trans_date = @xdate,protected = @protectleve,"+
//                        "enchrypted = @enchrypted WHERE Patientno=@xpat AND Groupcode=@xgc", connection);

//                    updateCommand.Connection = connection;
//                    updateCommand.Parameters.AddWithValue("@Groupcode", xgc);
//                    updateCommand.Parameters.AddWithValue("@Patientno", xpat);
//                    updateCommand.Parameters.AddWithValue("@Trans_date", xdate);
//                    updateCommand.Parameters.AddWithValue("@Protected", protectlevel);
//                    updateCommand.Parameters.AddWithValue("@enchrypted", enchrypted);

//                    int rows = updateCommand.ExecuteNonQuery();
//                    connection.Close();
//                }
//                catch (SqlException ex)
//                {
//                    MessageBox.Show("Update ANC-Medhist Update " + ex, "ANC Details", MessageBoxButtons.OK,
//                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//                    return;
//                }*/
//        }
//        private void btnSubmitPage3_Click(object sender, EventArgs e)
//        {
//            string xheader = "";
//            string btnselected = "";
//            Button btn = sender as Button;
//            if (btn.Name == "btnSave") //Page 1
//            {
//                btnselected = "SP1";
//                xheader = "PATIENT ANTE-NATAL RECORDS";
//            }
//            if (btn.Name == "btnSubmitPage2")
//            {
//                btnselected = "SP2";
//                xheader = "PATIENT MEDICAL HISTORY";
//            }
//            else if (btn.Name == "btnSubmitPage3")
//            {
//                btnselected = "SP3";
//                xheader = "GENERIC SCREENING / TERATOLOGY COUNSELING";
//            }
//            else if (btn.Name == "btnSubmitPg4")
//            {
//                btnselected = "SP4";
//                xheader = "INITIAL PHYSICAL EXAMINATION BY DOCTOR";
//            }
//            else if (btn.Name == "pg5_btnSave")
//            {
//                btnselected = "SP5";
//                xheader = "ANTE-NATAL CLINIC VISIT RECORDS";
//            }
//            else if (btn.Name == "pg5_btnSave")
//            {
//                btnselected = "SP6";
//                xheader = "DELIVERY RECORD";
//            }
//            else if (btn.Name == "btnSubmitPage7")
//            {
//                btnselected = "SP7";
//                xheader = "PERINEUM / IMMEDIATE POSTNATAL OBSERVATIONS";
//            }
//            else if (btn.Name == "btnSubmitPage8")
//            {
//                btnselected = "SP8";
//                xheader = "OPERATIVE DELIVERY RECORD";
//            }
//            if (btn.Name == "Pg9_btnSubmit")
//            {
//                btnselected = "SP9";
//                xheader = "BABY'S RECORDS";
//            }
//            DialogResult result = MessageBox.Show("Confirm to Save Details...", xheader,
//                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
//            if (result == DialogResult.No)
//                return;
//            switch (btnselected)
//            {
//                case "SP1":
//                    writePage1();
//                    break;
//                case "SP2":
//                    writePage2();
//                    break;
//                case "SP3":
//                    writePage3();
//                    break;
//                case "SP4":
//                    writePage4();
//                    break;
//                case "SP5":
//                    writePage5();
//                    break;
//                case "SP6":
//                    writePage6();
//                    break;
//                case "SP7":
//                    writePage7();
//                    break;
//                case "SP8":
//                    writePage8();
//                    break;
//                case "SP9":
//                    writePage9();
//                    break;
//            }
//        }

//        //page5 treatment date on page 5 must be clicked to active notes,prescription,request buttons
//        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
//        {
//            //treatment date on page 5 must be clicked to active notes,prescription,request buttons
//            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
//            {
//                if (e.ColumnIndex == this.dataGridView1.Columns[0].Index && dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
//                {
//                    currentPage5visitrecno = e.RowIndex;
//                    pg4_btnPrescription.Enabled = true;
//                    pg4_btnInvestigations.Enabled = true;
//                    pg5_btnSave.Enabled = true;

//                    /*	&&calc gestational age of preg
//IF !EMPTY(This.parent.parent.parent.parent.Page1.EDD1.Value) AND EMPTY(This.parent.parent.Column2.Text1.Value)
//xdays = DATE() - This.parent.parent.parent.parent.Page1.LMP1.Value
//xega = xdays/7
//This.parent.parent.Column2.Text1.Value = ALLTRIM(STR(INT(xega))+"wks+"+SUBSTR(STR(xega,6,4),4,1))+"d" &&STR(xega,6,1)
//ENDIF*/
//                    if (Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[0].Value) == DateTime.Now.Date && dtLMPpg1.Value > dtmin_date && (dataGridView1.Rows[e.RowIndex].Cells[1].Value == null || string.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString())))
//                    {
//                        //calc gestational age of preg
//                        decimal xdays = Convert.ToDecimal((DateTime.Now.Date - dtLMPpg1.Value).TotalDays);
//                        decimal xega = xdays / 7m;
//                        string xdaystr = xega.ToString().Length > 3 ? xega.ToString().Substring(3, 1) : "0";
//                        // string xstr = Convert.ToInt32( xega).ToString() + "wks+" + xega.ToString().Substring(3, 1) + "d";
//                        string xstr = Convert.ToInt32(xega).ToString() + "wks+" + xdaystr + "d";
//                        dataGridView1.Rows[e.RowIndex].Cells[1].Value = xstr;
//                    }
//                }
//            }
//        }
//        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
//            {
//                DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
//                if (e.ColumnIndex == this.dataGridView1.Columns[9].Index)
//                    cell.ToolTipText = "Click on Next Column (...) for Details. You must click on Date to activate this column";
//                else if (e.ColumnIndex == this.dataGridView1.Columns[10].Index)
//                    cell.ToolTipText = "Click To Load Current Case Note";
//                else if (e.ColumnIndex == this.dataGridView1.Columns[11].Index)
//                    cell.ToolTipText = "Next Vist (in weeks)";
//            }
//        }
//        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
//            {
//                if (e.ColumnIndex == 10)
//                {
//                    // DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
//                    //   if (e.ColumnIndex == this.dataGridView1.Columns[9].Index)
//                    //   {
//                    recno = e.RowIndex;
//                    string xnote = this.dataGridView1.Rows[e.RowIndex].Cells[9].Value != null ?
//                        this.dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString() : "";
//                    msmrfunc.mrGlobals.ancRtnNotes = xnote;
//                    NOTES notes = new NOTES("ANC Remarks/Treatment Notes for " + pg1_TxtName.Text.Trim(), "W", xnote, true);
//                    notes.Closed += new EventHandler(notes_Closed);
//                    notes.ShowDialog();
//                    //  }
//                }

//            }
//        }
//        void notes_Closed(object sender, EventArgs e)
//        {
//            //throw new NotImplementedException();
//            this.dataGridView1.Rows[recno].Cells[9].Value = msmrfunc.mrGlobals.ancRtnNotes;
//            return;
//        }
//        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.ColumnIndex == 11 && dataGridView1.Rows[e.RowIndex].Cells[11].Value != null)
//            {
//                //replace anc06.nextvisit WITH anc06.trans_date+(this.Value*7)
//                dataGridView1.Rows[e.RowIndex].Cells[12].Value =
//                    Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[0].Value).AddDays(
//                    Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[11].Value) * 7).ToString("dd/MM/yyyy");
//            }
//        }
//        void initglobalvariables()
//        {
//            msmrfunc.mrGlobals.mpatientno = pg1_txtpatientno.Text;
//            msmrfunc.mrGlobals.mreference = txtConsultReference.Text;
//            msmrfunc.mrGlobals.mfacility = vstata != null ? vstata.CLINIC : "";
//            msmrfunc.mrGlobals.mtrans_date = Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[0].Value);
//            msmrfunc.mrGlobals.minpatient = false;
//            //  msmrfunc.mrGlobals.mtth = false;
//            msmrfunc.mrGlobals.rtnstring = "";
//            msmrfunc.mrGlobals.percentageDiscountToApply = 0m; // (billchain.GROUPHTYPE == "P") ? patients.discount : customers.Discount;
//            msmrfunc.mrGlobals.mdoctor = dataGridView1.Rows[currentPage5visitrecno].Cells[13].Value == null ?
//                "" : dataGridView1.Rows[currentPage5visitrecno].Cells[13].Value.ToString();
//            msmrfunc.mrGlobals.inp2medhist = "";
//            msmrfunc.mrGlobals.isanc = true;
//            msmrfunc.mrGlobals.cashpaying = this.cashpaying;
//            for (int i = 0; i < 5; i++)
//            {
//                msmrfunc.mrGlobals.requestalerta_[i] = "";
//            }
//            msmrfunc.mrGlobals.requestalerta_count = 0;
//            msmrfunc.mrGlobals.returnvalue = 0;
//        }
//        private void pg4_btnPrescription_Click(object sender, EventArgs e)
//        {
//            initglobalvariables();
//            PrescriptionsNew presc = new PrescriptionsNew(anc01.GROUPCODE, anc01.PATIENTNO, mrattend.REFERENCE, mrattend.CLINIC, mrattend.TRANS_DATE, false, false, mdoctor, true, cashpaying, woperator, msection);
//            presc.Closed += new EventHandler(presc_Closed);
//            presc.Show();
//        }
//        void presc_Closed(object sender, EventArgs e)
//        {
//            // throw new NotImplementedException();
//            PrescriptionsNew presc_Closed = sender as PrescriptionsNew;
//            {
//                if (Session["inp2medhist"] != null && !string.IsNullOrWhiteSpace(Session["inp2medhist"].ToString())) //msmrfunc.mrGlobals.rtnstring))
//                {
//                    string xtext = dataGridView1.Rows[recno].Cells[9].Value.ToString();
//                    xtext += "\r\n" + Session["inp2medhist"].ToString();
//                    dataGridView1.Rows[recno].Cells[9].Value = xtext;
//                }
//                return;
//            }
//        }
//        private void pg4_btnInvestigations_Click(object sender, EventArgs e)
//        {
//            string rtnstring = "";
//            initglobalvariables();
//            frmInvProcRequest invrequest = new frmInvProcRequest("C", mrattend.REFERENCE, billchain.GROUPCODE, billchain.PATIENTNO, billchain.GROUPHTYPE, Convert.ToDateTime(dataGridView1.Rows[currentPage5visitrecno].Cells[0].Value), billchain.NAME, billchain.GROUPHEAD, billchain.GHGROUPCODE, false, ref rtnstring, "", 0, mdoctor, msection, true, true, true, woperator);
//            invrequest.Closed += new EventHandler(invrequest_Closed);
//            invrequest.Show();
//        }
//        void invrequest_Closed(object sender, EventArgs e)
//        {
//            if (Session["opdstring"] != null && !string.IsNullOrWhiteSpace(Session["opdstring"].ToString())) // msmrfunc.mrGlobals.rtnstring))
//            {
//                //edit1.Text += "\n" + msmrfunc.mrGlobals.rtnstring.Trim();
//                string xtext = dataGridView1.Rows[recno].Cells[9].Value.ToString();
//                xtext += "\r\n" + Session["opdstring"].ToString(); // msmrfunc.mrGlobals.rtnstring.Trim();
//                dataGridView1.Rows[recno].Cells[9].Value = xtext;
//            }
//            return;
//        }

//        private void btnNEXT_Click(object sender, EventArgs e)
//        {
//            // ActiveControl =  tabPage1;
//            //txtreference.Select();
//            //tabPage11.Select();
//            //tabPage11.Show();
//            //recordactive = true;
//            txtConsultReference.Text = AnyCode = Anycode1 = "";
//            tabControl1.SelectedTab = tabPage1;
//            // txtConsultReference.Select();
//            btnreload.PerformClick();
//            return;
//        } //at page5
//          //page 7
//        private void dataGridViewPg7_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
//        {
//            dataGridViewPg7.Rows[e.RowIndex].Cells[11].Value = "Remove";
//            dataGridViewPg7.Rows[e.RowIndex].Cells[12].Value = "NEWREC";
//        }
//        private void dataGridViewPg7_CellContentClick(object sender, DataGridViewCellEventArgs e)
//        {
//            // MessageBox.Show("Click Btn - cell content", msgBoxHandler); - detected
//            var senderGrid = (DataGridView)sender;

//            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
//                e.RowIndex >= 0 && e.ColumnIndex == 11) //Remove Button
//            {
//                recno = e.RowIndex;
//                dataGridViewPg7.Rows.RemoveAt(recno);
//                MessageBox.Show("Deleted...");
//                recno = -1;
//            }
//        }

//        private void pg6_txtConsultantOb_Leave(object sender, EventArgs e)
//        {
//            if (!string.IsNullOrWhiteSpace(pg6_txtConsultantOb.Text))
//            {
//                pg6_dtpDeliverydate.Visible = dtpDeliveryTime.Visible = true;
//                txtdtpDeliveryTime.Visible = pg6_txtdtpDeliverydate.Visible = false;
//            }
//        }

//        private void BtnPrint_Click(object sender, EventArgs e)
//        {
//            rptFormsANC ancreports = new rptFormsANC(pg1_txtgroupcode.Text, pg1_txtpatientno.Text, pg1_TxtName.Text, woperator);
//            ancreports.Show();
//        }

//        private void btnGetHistory_Click(object sender, EventArgs e)
//        {
//            displayPrevMedHistory(false);
//            /*     if (dtHistoryDatefrom.Value.Date < dtmin_date || dtHistoryDateto.Value.Date < dtHistoryDatefrom.Value.Date || dtHistoryDatefrom.Value.Date > DateTime.Now.Date)
//                 {
//                     DialogResult result = MessageBox.Show("Invalud Date Specification...");
//                     return;
//                 }
//                 lblPreviousRecords.Visible = true;
//                 PrevMedHistoryNotes.Text = MedHist.GetMEDHISTCaseNotes(billchain.GROUPCODE, billchain.PATIENTNO, false, true, dtHistoryDatefrom.Value.Date, dtHistoryDateto.Value.Date);
//                 lblPreviousRecords.Visible = false;*/
//        }

//        private void listView1_MouseClick(object sender, MouseEventArgs e)
//        {
//            // MessageBox.Show("Mouse Click");
//            ListViewItem lv = new ListViewItem();
//            if (listView1.SelectedItems.Count > 0)
//            {
//                lvitemselect = listView1.SelectedIndex;
//                lv = listView1.SelectedItems[0];
//                pictBox1.Image = WebGUIGatway.getpicture(lv.SubItems[3].Text.Trim());
//                pictBox2.Image = WebGUIGatway.getpicture(lv.SubItems[6].Text.Trim());
//                pictBox3.Image = WebGUIGatway.getpicture(lv.SubItems[9].Text.Trim());
//                pictBox4.Image = WebGUIGatway.getpicture(lv.SubItems[12].Text.Trim());
//                pictBox5.Image = WebGUIGatway.getpicture(lv.SubItems[15].Text.Trim());

//                txtImagenotes.Text = lv.SubItems[4].Text;

//                txtPic1.Text = lv.SubItems[3].Text.Trim();
//                txtPic2.Text = lv.SubItems[6].Text;
//                txtPic3.Text = lv.SubItems[9].Text;
//                txtPic4.Text = lv.SubItems[12].Text;
//                txtPic5.Text = lv.SubItems[15].Text;
//            }
//        }
//        private void pictBox2_Click(object sender, EventArgs e)
//        {
//            PictureBox pbox = sender as PictureBox;
//            if (pbox.Name == "pictBox2" && !string.IsNullOrWhiteSpace(txtPic2.Text))
//            {
//                pictBox1.Image = WebGUIGatway.getpicture(txtPic2.Text);
//                txtPic1.Text = txtPic2.Text;
//            }
//            else if (pbox.Name == "pictBox3" && !string.IsNullOrWhiteSpace(txtPic3.Text))
//            {
//                pictBox1.Image = WebGUIGatway.getpicture(txtPic3.Text);
//                txtPic1.Text = txtPic3.Text;
//            }
//            else if (pbox.Name == "pictBox4" && !string.IsNullOrWhiteSpace(txtPic4.Text))
//            {
//                pictBox1.Image = WebGUIGatway.getpicture(txtPic4.Text);
//                txtPic1.Text = txtPic4.Text;
//            }
//            else if (pbox.Name == "pictBox5" && !string.IsNullOrWhiteSpace(txtPic5.Text))
//            {
//                pictBox1.Image = WebGUIGatway.getpicture(txtPic5.Text);
//                txtPic1.Text = txtPic5.Text;
//            }

//            /*     if (!string.IsNullOrWhiteSpace( pbox.Image.ToString()))
//				 {
//					 string id = pbox.Name.Substring(pbox.Name.Length - 1, 1), txt = "txtPic"; // .PadRight(1)
//					// txt = txt + id + ".Text";
//					 //txtPic1.Text = txt;
//					 txtPic1.Text = txtPic2.Text;
//				 }*/
//        }

//        private void chkFullScreen_Click(object sender, EventArgs e)
//        {
//            if (!string.IsNullOrWhiteSpace(txtPic1.Text))
//            {
//                frmImageView imagev = new frmImageView(txtPic1.Text);
//                imagev.Show();
//            }
//            //if (!string.IsNullOrWhiteSpace( txtPic1.Text))
//            //   Process.Start(txtPic1.Text); //APP,USER,PASSWD,DOMAIN
//        }

//        private void chkRecordDeliveryDate_CheckedChanged(object sender, EventArgs e)
//        {
//            if (chkRecordDeliveryDate.Checked)
//            {
//                dtDeliveryDatepg1.Visible = true;
//                pg1_txtDeliveryDate.Visible = false;
//            }
//            else
//            {
//                dtDeliveryDatepg1.Visible = false;
//                pg1_txtDeliveryDate.Visible = true;
//            }
//        }

//        private void pg4_btnAdmissions_Click(object sender, EventArgs e)
//        {
//            DialogResult result;
//            if (anc01 == null || mrattend == null || anc01.PATIENTNO != mrattend.PATIENTNO || string.IsNullOrWhiteSpace(anc01.NAME) || string.IsNullOrWhiteSpace(mrattend.PATIENTNO) || anc01.DEL_DATE > dtmin_date)
//            {
//                result = MessageBox.Show("No Active patient details on Consulting Platform...\r\n OR Patient Profile on Ante-Natal not the same with Daily Attendance Record");
//                return;
//            }

//            decimal xamt = 0m;
//            bool isadmdeposit, foundit, cashpaying;
//            isadmdeposit = foundit = cashpaying = false;

//            result = MessageBox.Show("Confirm to Send Patient " + anc01.NAME + "for Admission...", "",
//                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
//            if (result == DialogResult.No)
//                return;
//            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE from admrecs where groupcode = '" + anc01.GROUPCODE + "' and patientno = '" + anc01.PATIENTNO + "' and discharge = ''", false);
//            if (dt.Rows.Count > 0)
//            {
//                MessageBox.Show("This Patient is already on Admission or \r\n there is an undischarged record for the Patient. Check : " + dt.Rows[0]["reference"].ToString());
//                return;
//            }
//            //lblprompt.Text = "Paging Admissions... Pls Wait !";
//            dt = Dataaccess.GetAnytable("", "MR", "select grouphead,grouphtype,ghgroupcode from billchain where groupcode = '" + anc01.GROUPCODE + "' and patientno = '" + anc01.PATIENTNO + "'", false);
//            if (dt.Rows.Count < 1)
//            {
//                MessageBox.Show("This Patient record is not in Patient Master File...");
//                return;
//            }
//            if (anc01.PATIENTNO == dt.Rows[0]["grouphead"].ToString() || dt.Rows[0]["grouphtype"].ToString() == "P")
//                cashpaying = true;
//            string xcomment = "";
//            //commented 09.02.2018 to return xcomment to medhist consult
//            MedHist medhist = MedHist.GetMEDHIST(anc01.GROUPCODE, anc01.PATIENTNO, "", false, true, DateTime.Now.Date, "DESC");
//            bool newmedhist = true;
//            if (medhist != null)
//            {
//                newmedhist = false;
//                xcomment = medhist.COMMENTS.Trim() + "\r\n";
//            }
//            if (xcomment != "")
//            {
//                xcomment += string.Concat(Enumerable.Repeat("-", 144));
//                xcomment += "\r\n";
//            }
//            xcomment += "==>Admissions Referral By : " + woperator + "  @ " + DateTime.Now.ToLongTimeString();
//            //commented 09.02.2018 to return xcomment to medhist consult
//            MedHist.updatemedhistcomments(anc01.GROUPCODE, anc01.PATIENTNO, DateTime.Now, xcomment, newmedhist, mrattend.REFERENCE, anc01.NAME, dt.Rows[0]["ghgroupcode"].ToString(), dt.Rows[0]["grouphead"].ToString(), mdoctor);
//            //01/08/2012 check for admission deposit setting in control.dat
//            if (cashpaying)
//            {
//                dt = Dataaccess.GetAnytable("", "MR", "select regconspay, dactive from mrcontrol order by recid", false);
//                xamt = (decimal)dt.Rows[1]["regconspay"];
//                isadmdeposit = (bool)dt.Rows[3]["dactive"];
//            }
//            if (isadmdeposit) //post alert to cashier
//            {
//                LINK.WriteLINK(0, anc01.GROUPCODE, anc01.PATIENTNO, anc01.NAME, "2", mrattend.REFERENCE, xamt, 0m, mrattend.CLINIC, false, mdoctor, false, 0, "A", "4", woperator);
//            }
//            //alert to admission nurses
//            LINK.WriteLINK(0, anc01.GROUPCODE, anc01.PATIENTNO, anc01.NAME, "A", mrattend.REFERENCE, 0m, 0m, mrattend.CLINIC, false, mdoctor, false, 9, "", "4", woperator);
//            //Send Alert to all nurses 07-03-2013 : Kupa request
//            string xnote = "*** PATIENT ADMISSION ALERT !!! *** \r\n\r\n" + anc01.NAME + "\r\n DATE : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:sst");
//            MRB21.Writemrb21Details(anc01.GROUPCODE, anc01.PATIENTNO, DateTime.Now, anc01.NAME, mrattend.CLINIC, woperator, xnote, mrattend.REFERENCE, "4", "3", woperator, mdoctor, "I");
//            // lblprompt.Text = "";
//            result = MessageBox.Show("DONE...", "Admissions Referals", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//            //msmrfunc.mrGlobals.rtnstring = xcomment;
//            //btnClose.PerformClick();

//        }

//        //private void txtConsultReference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
//        //{
//        //    txtConsultReference_LostFocus(null, null);
//        //}

//        //private void pg1_txtANCReference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
//        //{
//        //    pg1_txtANCReference_LostFocus(null, null);
//        //}

//        //private void pg1_txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
//        //{
//        //    SelectNextControl(ActiveControl, true, true, true, true);
//        //}

//        //private void dtDOBpg1_KeyDown(object objSender, KeyEventArgs objArgs)
//        //{
//        //    if (objArgs.KeyCode == Keys.Enter)
//        //    {
//        //        dtDOBpg1_Leave(null, null);
//        //        SelectNextControl(ActiveControl, true, true, true, true);
//        //    }
//        //}

//        //private void pg1_combOccupatonWf_KeyDown(object objSender, KeyEventArgs objArgs)
//        //{
//        //    if (objArgs.KeyCode == Keys.Enter)
//        //    {
//        //        SelectNextControl(ActiveControl, true, true, true, true);
//        //    }
//        //}

//        //private void dtLMPpg1_KeyDown(object objSender, KeyEventArgs objArgs)
//        //{
//        //    if (objArgs.KeyCode == Keys.Enter)
//        //    {
//        //        dtLMPpg1_Leave(null, null);
//        //        SelectNextControl(ActiveControl, true, true, true, true);
//        //    }
//        //}

//        private void btnLaborVitals_Click(object sender, EventArgs e)
//        {
//            frmLaborVitalSignChart laborvitals = new frmLaborVitalSignChart(anc01.REFERENCE, anc01, TxtAge.Text, woperator);
//            laborvitals.Show();
//        }




//        //PAGE 9

//    }
//}