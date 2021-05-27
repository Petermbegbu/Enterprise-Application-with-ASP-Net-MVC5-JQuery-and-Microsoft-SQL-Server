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
    public partial class frmDailyAttendance
    {

        billchaindtl bchain;
        Customer customers;
        patientinfo patients;
        Mrattend mrattend;
        //  PleaseWaitForm pleaseWait = new PleaseWaitForm();
        Medhrec medhrec = new Medhrec();
        ANCREG ancreg = new ANCREG();
        //categ_save, start_time,global_clinic_code = "",mgenregistration,singlepvtfc_src, mlastnumb = 1, 
        DataTable dtclinics = Dataaccess.GetAnytable("", "CODES", "select type_code, name from servicecentrecodes order by name", true);
        string PicSelected, mgrouphtype, msection, start_time,
            mservice_type, spconsultcode, mancclinic, mimmunizationclinic, sub_cons_code_adult, xtosection, mremark, mconscode, conscode_save;

        bool misreregall, autogenreg, must_patphoto, mdocson, isreregistration, cons_fee_at_docs, Recs_ToCashOffice, newrec, autoANClabreq, mcanadd, mcandelete, mcanalter, opdNurseActive, enforcecreditlimit = false;
        bool allow_autonomous_clinic, topaycons, ancrecord, monthlyconsultflag, mconsbyclinic, mautogcons, misrereg, misreregpvt, cashpaying, skipConsult4NewReg = false;

        decimal mduration, init_cons_fee_adult, init_cons_duration_adult, mduraconsul, sub_cons_duration_adult, sub_cons_fee_adult, mlastno;
        string AnyCode = "", Anycode1 = "", lookupsource = "", woperator, rtnstring = "", mgrouphead, mghgroupcode = "";
        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start; //start of transaction
        DateTime dttrans_date;

        MR_DATA.MR_DATAvm vm;
        
        public frmDailyAttendance(MR_DATA.MR_DATAvm VM2, string woperato)
        {
            vm = VM2;

            bchain = new billchaindtl();
            customers = new Customer();
            patients = new patientinfo();
            mrattend = new Mrattend();

            woperator = woperato;
            msection = "1";
            getcontrolsettings();
            //initcomboboxes();
            mservice_type = "O";
            //chkOutPatient.Checked = true;
            dttrans_date = DateTime.Now;
            //txtreference.Focus();

            topaycons = (vm.REPORTS.REPORT_TYPE5 == "true") ? true : false;
        }

        private void getcontrolsettings()
        {
            //DataTable dt = msmrfunc.getcontrolsetup("MR");
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select docson, conscode, consbycli, autogcons, cashpoint, filemode, dactive, installed, seclink, Last_no, pvtcode, attendlink, fsh, regconspay, last_no, name,  attdate, attno from mrcontrol order by recid", false);

            // MessageBox.Show(dt.Rows.Count.ToString());
            mdocson = (bool)dt.Rows[0]["docson"];
            mconscode = dt.Rows[0]["conscode"].ToString();
            mconsbyclinic = (bool)dt.Rows[0]["consbycli"];
            mautogcons = (bool)dt.Rows[0]["autogcons"];

            misrereg = (bool)dt.Rows[1]["cashpoint"];
            misreregpvt = (bool)dt.Rows[1]["filemode"];
            misreregall = (bool)dt.Rows[1]["dactive"];
            spconsultcode = dt.Rows[1]["conscode"].ToString();
            cons_fee_at_docs = (bool)dt.Rows[1]["installed"];
            isreregistration = (bool)dt.Rows[1]["cashpoint"];
            Recs_ToCashOffice = (bool)dt.Rows[1]["seclink"]; //records to Cash office
            skipConsult4NewReg = (bool)dt.Rows[0]["consbycli"];

            mduration = (Decimal)dt.Rows[2]["Last_no"];
            mancclinic = dt.Rows[2]["pvtcode"].ToString();
            enforcecreditlimit = (bool)dt.Rows[2]["dactive"];
            monthlyconsultflag = (bool)dt.Rows[2]["attendlink"];

            autoANClabreq = (bool)dt.Rows[3]["fsh"];

            must_patphoto = (bool)dt.Rows[4]["installed"];
            init_cons_fee_adult = (Decimal)dt.Rows[4]["regconspay"];
            init_cons_duration_adult = mduraconsul = (Decimal)dt.Rows[4]["last_no"];

            autogenreg = (bool)dt.Rows[5]["attendlink"];
            sub_cons_fee_adult = (Decimal)dt.Rows[5]["regconspay"];
            sub_cons_duration_adult = (Decimal)dt.Rows[5]["last_no"];
            sub_cons_code_adult = dt.Rows[5]["pvtcode"].ToString();

            allow_autonomous_clinic = (bool)dt.Rows[6]["installed"];

            opdNurseActive = (bool)dt.Rows[6]["attendlink"];
            mimmunizationclinic = dt.Rows[7]["name"].ToString().Substring(0, 5);  //.Substring(0, 5);

            //if (Convert.ToDateTime(dt.Rows[3]["attdate"]) != DateTime.Now.Date) //new day, restart counter
            //{
            //    var dat = dt.Rows[3]["attdate"];
            //    string updatestring = "update mrcontrol set attdate = '" + DateTime.Now.Date + "', attno = '0' where recid = '4'";
            //    bissclass.UpdateRecords(updatestring, "MR");
            //    vm.REPORTS.nmrattendancetoday = 0;
            //}
            //else
            //{
            //    vm.REPORTS.nmrattendancetoday = (decimal)dt.Rows[3]["attno"];
            //}

            dt = Dataaccess.GetAnytable("", "MR", "select CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];

        }


        //private void button6_Click(object sender, EventArgs e)
        //{
        //    // pleaseWait.Close();
        //    this.Close();
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    //           Button btn = (Button)sender;

        //    if (btn.Name == "btngroupcode")
        //    {
        //        this.txtgroupcode.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "g";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //    }
        //    else if (btn.Name == "btnpatientlookup")
        //    {
        //        this.txtpatientno.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //    }
        //    else
        //    {
        //        this.txtreference.Text = "";
        //        lookupsource = "I";
        //        msmrfunc.mrGlobals.crequired = "I";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED DAILY ATTENDANCE";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //    void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        //    {
        //        frmselcode FrmSelcode = sender as frmselcode;
        //        if (lookupsource == "g") //groupcodee
        //        {
        //            this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //            this.txtpatientno.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //            this.txtpatientno.Select();
        //        }

        //        else if (lookupsource == "L") //patientno
        //        {
        //            this.txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode1;
        //            this.txtpatientno.Select();
        //        }
        //        else if (lookupsource == "I") //daiy attendance
        //        {
        //            this.txtreference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //            this.txtreference.Focus();
        //        }
        //        /*    else
        //{
        //	//this.txtghgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //	this.txtbillspayable.Text = msmrfunc.mrGlobals.anycode;
        //	this.txtbillspayable.Focus();

        //}*/
        //    }

        /*       private void comservtype_Leave(object sender, EventArgs e)
               {
                   if (this.comservtype.SelectedItem == null)
                   { this.comservtype.SelectedItem = "Out-Patient  Records"; }

                   {
                       //Char xgrouphtype = this.comservtype.SelectedItem.ToString()[0];
                       //mservice_type = xgrouphtype.ToString();
                       mservice_type = comservtype.SelectedItem.ToString().Substring(0,1);
                       txtreference.Focus();

                   }
               }
               */

        //private void opgserviceoptions_Enter(object sender, EventArgs e)
        //{
        //    txtreference.Text = "";
        //    chkOutPatient.Checked = chkSpecialService.Checked = false;
        //}

        //private void rBnservicetype_Click(object sender, EventArgs e)
        //{
        //    ClearControls("R");
        //    mservice_type = "O"; //comservtype.SelectedItem.ToString().Substring(0, 1);
        //    chkOutPatient.Checked = true;
        //    txtreference.Focus();
        //    // return;
        //}

        //private void radioButton1_Click(object sender, EventArgs e) //special service
        //{
        //    ClearControls("R");
        //    mservice_type = "S"; // comservtype.SelectedItem.ToString().Substring(0, 1);
        //    chkSpecialService.Checked = true;
        //    txtreference.Focus();
        //    //  return;
        //}


        //private void txtreference_Enter(object sender, EventArgs e)
        //{
        //    if (!chkOutPatient.Checked && !chkSpecialService.Checked)
        //    {
        //        /* DialogResult result = MessageBox.Show("Service Type must be Selected...Out-Patient or Special Service Records !", "Service Options");
        //         chkOutPatient.Select();*/
        //        return;
        //    }
        //    // pleaseWait.Hide();
        //    if (string.IsNullOrWhiteSpace(AnyCode)) //no lookup
        //    {
        //        txtreference.Text = "";
        //        if (mservice_type == "O") //outpatient
        //            mlastno = msmrfunc.getcontrol_lastnumber("CHARGNO", 3, false, mlastno, false);
        //        else
        //            mlastno = msmrfunc.getcontrol_lastnumber("CHARGNO", 5, false, mlastno, false);
        //        txtreference.Text = mlastno.ToString();
        //    }
        //}


        //private void txtreference_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtreference.Text))
        //    {
        //        return;
        //    }
        //    if (string.IsNullOrWhiteSpace(AnyCode) && !string.IsNullOrWhiteSpace(txtreference.Text) && !bissclass.IsDigitsOnly(txtreference.Text)) //, "Daily Attendance Reference"))
        //    {
        //        txtreference.Text = "";
        //        txtreference.Select();
        //        return;
        //    }
        //    this.txtreference.Text = bissclass.autonumconfig(this.txtreference.Text, true, (mservice_type == "S") ? "S" : "C", "999999999");
        //    newrec = true;
        //    //check if reference exist
        //    this.ClearControls("P");
        //    AnyCode = Anycode1 = "";
        //    //  msgeventtracker = "RN";
        //    mrattend = Mrattend.GetMrattend(this.txtreference.Text);
        //    if (mrattend == null) //new defintion
        //    {
        //        //msmrfunc.mrGlobals.waitwindowtext = "NEW ATTENDANCE RECORD ...";

        //        // Display form modelessly
        //        //pleaseWait.Show();
        //        if (mservice_type == "O")
        //            txtgroupcode.Focus();
        //    }
        //    else
        //    {
        //        //msmrfunc.mrGlobals.waitwindowtext = "Record Exists";
        //        newrec = false;
        //        // Display form modelessly
        //        // pleaseWait.Show();
        //        mgrouphtype = mrattend.GROUPHTYPE;
        //        txtpatientno.Text = mrattend.PATIENTNO;
        //        txtgroupcode.Text = mrattend.GROUPCODE;
        //        bissclass.displaycombo(combclinic, dtclinics, "name", "type_code");
        //        //combclinic.Text = bissclass.combodisplayitemCodeName("type_code", mrattend.CLINIC, dtclinics, "name");
        //        //combclinic.SelectedValue = mrattend.CLINIC;
        //        mgrouphead = mrattend.GROUPHEAD;
        //        mghgroupcode = mrattend.GHGROUPCODE;
        //        if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
        //        {
        //            bchain = billchaindtl.Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);
        //            this.DisplayDetails();
        //            if (mrattend.POSTED)
        //            {
        //                // msgeventtracker = "RN";
        //                DialogResult result = MessageBox.Show("Record Exist... and it's Posted...");
        //                ClearControls("R");
        //                txtreference.Select();
        //                return;

        //                //this.cmbdelete.Enabled = msmrfunc.GlobalAccessCheck("D"); 
        //                //   this.cmbdelete.Enabled = true;
        //            }
        //            else
        //                this.btndelete.Enabled = mcanalter ? true : false;
        //        }
        //        DialogResult result1 = MessageBox.Show("Record Exist...");
        //    }
        //    if (mservice_type == "S") // special service patient
        //    {
        //        if (msection != "1")
        //        {
        //            MessageBox.Show("User Defined Function Conflict...NOT A FRONT DESK OFFICER!");
        //            return;
        //        }
        //        frmInvProcRequest invrequest = new frmInvProcRequest("S", txtreference.Text, txtgroupcode.Text, txtpatientno.Text, mgrouphtype, dttrans_date.Value.Date, txtname.Text, mgrouphead, mghgroupcode, false, ref rtnstring, "", mlastno, "", msection, mcanadd, mcanalter, mcandelete, woperator);
        //        invrequest.Closed += new EventHandler(invrequest_Closed);
        //        invrequest.Show();
        //        //   txtreference.Focus();
        //    }
        //    dttrans_date.Value = DateTime.Now;
        //}

        //void invrequest_Closed(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    //comservtype.Focus();
        //    chkSpecialService.Checked = true;
        //    txtreference.Focus();
        //    return;
        //}

        //private void txtpatientno_LostFocus(object sender, EventArgs e)
        //{
        //    this.nmbalance.Value = this.nmrcredit.Value = this.nmrdebit.Value = nmrCreditLimit.Value = 0m;
        //    nmrCreditLimit.Visible = lblCreditLimit.Visible = false;

        //    if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        AnyCode = "";
        //        txtgroupcode.Select();
        //        return;
        //    }

        //    dttrans_date.Value = DateTime.Now;
        //    start_time = DateTime.Now.ToLongTimeString();
        //    newrec = true;
        //    if (string.IsNullOrWhiteSpace(AnyCode))  //no lookup value obtained
        //        this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");

        //    //check if patientno exists
        //    bchain = billchaindtl.Getbillchain(this.txtpatientno.Text, txtgroupcode.Text);
        //    if (bchain == null)
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Patient Number... ");
        //        txtpatientno.Text = " ";
        //        txtgroupcode.Select();
        //        return;
        //    }
        //    else
        //    {
        //        mgrouphtype = bchain.GROUPHTYPE;
        //        if (!DisplayDetails())
        //        {
        //            txtpatientno.Text = txtgroupcode.Text = "";
        //            txtgroupcode.Focus();
        //            return;
        //        }
        //    }

        //    combclinic.Enabled = true;
        //    btnledger.Enabled = true;
        //    // gET lAST ATTENDANCE DATE
        //    medhrec = Medhrec.GetMEDHREC(bchain.GROUPCODE, bchain.PATIENTNO);
        //    if (medhrec != null)
        //        dtlastattend.Text = medhrec.DATE5.ToShortDateString() + "  @ " + medhrec.DATE5.ToShortTimeString();

        //    checkmultipleattendance();
        //    // combclinic.Focus();
        //    AnyCode = "";
        //    //btnsave.Enabled = true;
        //    if (bchain.GROUPHTYPE == "C" && customers.HMO) //&& !string.IsNullOrWhiteSpace(bchain.STAFFNO))
        //    {
        //        panel_EnrolleeDetails.Visible = true;
        //        lblEnrolleeNumber.Text = bchain.STAFFNO;
        //        lblPlantype.Text = bchain.HMOSERVTYPE;
        //        lblEnrolleePhone.Text = bchain.PHONE;
        //    }
        //    chkMedicalTreatment.Focus();
        //}

        //  private bool DisplayDetails()
        //  {
        //      // msgeventtracker = "g";
        //      DialogResult result;
        //      if (mgrouphtype == "P")
        //      {
        //          patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);
        //          if (patients == null)
        //          {
        //              //msgeventtracker = "g";
        //              result = MessageBox.Show("Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS (PVT/FC)", "GroupHead Informaton");
        //              return false;
        //          }
        //          cashpaying = patients.bill_cir == "C" ? true : false;
        //      }
        //      else
        //      {
        //          customers = Customer.GetCustomer(bchain.GROUPHEAD);
        //          if (customers == null)
        //          {
        //              result = MessageBox.Show("Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS (Corporate)", "GroupHead Informaton");
        //              return false;
        //          }
        //          cashpaying = customers.Bill_cir == "C" ? true : false;
        //      }

        //      this.txtgroupcode.Text = bchain.GROUPCODE;
        //      this.txtname.Text = bchain.NAME;
        //      //  mbill_cir = (mgrouphtype == "C") ? customers.Bill_cir : patients.bill_cir;
        //      mremark = (mgrouphtype == "C") ? customers.Remark : patients.remark;
        //      this.txtgroupheadname.Text = (mgrouphtype == "P" && bchain.GROUPHEAD == bchain.PATIENTNO) ? "< SELF >" : (mgrouphtype == "C") ? customers.Name : patients.name;

        //      //  if (global_clinic_code != "" && global_clinic_code != 'ALL') //autonomous clinic - aviation for kupa 01/06/2011
        //      //      this.combclinic.Text = global_clinic_code;

        //      if (mgrouphtype == "P" && bchain.PATIENTNO == bchain.PATIENTNO)
        //          txtaddress.Text = patients.address1.Trim();
        //      else
        //          txtaddress.Text = bchain.RESIDENCE;

        //      if (bchain.SECTION != "")
        //          txtaddress.Text = txtaddress.Text + "\r\n" + bchain.SECTION;
        //      if (bchain.DEPARTMENT != "")
        //          txtaddress.Text = txtaddress.Text + "\r\n" + bchain.DEPARTMENT.Trim();
        //      if (bchain.STAFFNO != "")
        //          txtaddress.Text = txtaddress.Text + "\r\n [Staff # :" + bchain.STAFFNO.Trim() + ":" + bchain.HMOCODE.Trim() + " ]";

        //      PicSelected = bchain.PICLOCATION;
        //      if (!string.IsNullOrWhiteSpace(PicSelected))
        //      {
        //          pictureBox1.Image = WebGUIGatway.getpicture(PicSelected);
        //      }

        //      this.rtxtspecialinstructinos.Text = " ";
        //      //01.09.2019 - check for Group Head Status
        //      if (bchain.GROUPHTYPE == "C" && customers.Custstatus == "D" || bchain.GROUPHTYPE == "P" && (patients.patstatus == "C" || patients.patstatus == "S"))
        //      {
        //          MessageBox.Show("This Patient Group Head is no longer Active for Service in this Facility...\r\n\r\n PLEASE CONTACT ACCOUNTS DEPARTMENT!", "SERVICE RESTRICTION", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
        //          return false;
        //      }
        //      if (mgrouphtype == "C" && customers.Trackattndform)
        //      {
        //          this.chkattendanceform.Visible = true;
        //          result = MessageBox.Show("This patient is required to fill and sign attendance form at Front Desk ...", "PLEASE NOTE!!!");
        //      }
        //      xtosection = "3";
        //      //if (mbill_cir == "C" ) //cash
        //      //    xtosection = "2"; //he sees the nurse for v/sign

        //      if (!string.IsNullOrWhiteSpace(mremark + bchain.MEDNOTES + bchain.SPNOTES))
        //          this.rtxtspecialinstructinos.Text = mremark + "\r\n-------------------------------------------------\r\n" + bchain.SPNOTES + "\r\n-------------------------------------------------\r\n" + bchain.MEDNOTES;


        //      if (bchain.STATUS == "C")
        //      {
        //          result = MessageBox.Show("PATIENT NOT VALID FOR TRANSACTION - < Cancelled >");
        //          return false;
        //      }
        //      if (bchain.STATUS == "S")
        //      {
        //          // msgeventtracker = "RN";
        //          result = MessageBox.Show("PATIENT NOT VALID FOR TRANSACTION - < Suspended > " +
        //           " CONFIRM TO CONTINUE...", "PATIENT STATUS", MessageBoxButtons.YesNo,
        //                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //          if (result == DialogResult.No)
        //          {
        //              ClearControls("R");
        //              txtreference.Select();
        //              return false;
        //          }
        //          frmOverwrite overwrite = new frmOverwrite("Overwrite Suspended Registration", "mrstlev", "MR");
        //          overwrite.ShowDialog();
        //          if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode)) //anycode global var is returned
        //              return false;
        //          //update overwrite profile
        //          msmrfunc.updateOverwrite(txtreference.Text, "Overwrite  Suspended Registration", bchain, bchain.GROUPHTYPE == "P" ? patients.cr_limit : customers.Cr_limit, nmbalance.Value);
        //      }
        //      //&&CHECK FOR EXPIRY DATE IF RE-REGISTRATION IS ENABLED
        //      /*   if ( misrereg &&  EMPTY(bchain.EXPIRYDATE ? && bchain.expirydate <= DateTime.Now() )
        //MessageBox.Show( "PATIENT NOT VALID FOR TRANSACTION - Registration Expired on "+bchain.EXPIRYDATE.ToString(),
        //  "Patient's Re-Registration Check" );
        //  txtgroupcode.Focus();
        //return;*/


        //      //thisform.get_age()
        //      //&&24/06/2012 check for patient photo 
        //      if (must_patphoto && (pictureBox1.Image == null || this.pictureBox1.Image.ToString() == ""))
        //      {
        //          result = MessageBox.Show("Patient Photo is missing...", "Control Setup Requirement");
        //          return false;
        //      }
        //      if (mgrouphtype == "C" && customers.HMO && bchain.HMOSERVTYPE == "")
        //      {
        //          // msgeventtracker = "g";
        //          result = MessageBox.Show("HMO Plan Type not specified in Patient Registration Details...Incomplete HMO Patient Registration");
        //          return false;
        //      }
        //      if (bchain.GROUPHTYPE == "P") //get current balance
        //      {
        //          if (enforcecreditlimit)
        //          {
        //              nmrCreditLimit.Visible = lblCreditLimit.Visible = true;
        //              nmrCreditLimit.Value = patients.cr_limit;
        //          }

        //          decimal db, cr, bal, adj; db = cr = bal = adj = 0m;
        //          bal = msmrfunc.getOpeningBalance(bchain.GHGROUPCODE, bchain.GROUPHEAD, "", bchain.GROUPHTYPE, DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);
        //          db = cr = adj = 0m;
        //          decimal xamt = msmrfunc.getTransactionDbCrAdjSummary(bchain.GHGROUPCODE, bchain.GROUPHEAD, "", bchain.GROUPHTYPE, DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);
        //          nmrcredit.Value = cr;
        //          nmrdebit.Value = db;
        //          if (bal < 1)
        //              nmrcredit.Value += Math.Abs(bal);
        //          else
        //              nmrdebit.Value += bal;
        //          if (adj < 1)
        //              nmrcredit.Value += Math.Abs(adj);
        //          else
        //              nmrdebit.Value += adj;
        //          xamt = nmrdebit.Value - nmrcredit.Value;
        //          nmbalance.Value = Math.Abs(xamt);
        //          lblbalbfdbcr.Text = (xamt < 1) ? "CR" : "DB";

        //          //check for credit limit
        //          if (enforcecreditlimit && patients.cr_limit > 0 && nmbalance.Value > patients.cr_limit)
        //          {
        //              result = MessageBox.Show("Transactions not allowed for this Patient... CREDIT LIMIT EXCEEDED ! \r\n\r\n ...Overwrite required !  CONTINUE... ?", "CREDIT LIMIT RESTRICTION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //              if (result == DialogResult.No)
        //                  return false;
        //              frmOverwrite overwrite = new frmOverwrite("Overwrite To Credit Limit Control", "mrstlev", "MR");
        //              overwrite.ShowDialog();
        //              if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode)) //anycode global var is returned
        //                  return false;
        //              //update overwrite profile
        //              msmrfunc.updateOverwrite(txtreference.Text, "Overwrite To Credit Limit Control", bchain, bchain.GROUPHTYPE == "P" ? patients.cr_limit : customers.Cr_limit, nmbalance.Value);
        //          }
        //      }
        //      lblage.Text = bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now);
        //      // lblage.Text = agecalc(bchain.BIRTHDATE, DateTime.Now);
        //      return true;
        //  }

        public MR_DATA.MR_DATAvm btnsave_Click()
        {
            //if (!bissclass.IsPresent(this.txtgroupcode, "Patients Groupcode", false) ||
            //    !bissclass.IsPresent(this.txtpatientno, "Patient Number", false) ||
            //    !bissclass.IsPresent(this.txtname, "Patients Name", false) ||
            //    !bissclass.IsPresent(this.txtgroupheadname, "Bills Payable By", false) ||
            //    !bissclass.IsPresent(this.txtreference, "Consultation Reference", false) ||
            //    !bissclass.IsPresent(this.combclinic, "CLINIC DETAILS", false))
            //{
            //    return;
            //}

            //DialogResult result1;
            //msgeventtracker = "PS";

            bool flag = true;
            newrec = false;

            Mrattend mrattend = Mrattend.GetMrattend(vm.REPORTS.txtreference);

            if (mrattend == null){
                newrec = true;
            }

            if (newrec && !mcanadd){
                vm.REPORTS.alertMessage = "ACCESS DENIED...To New Record Creation.  See your Systems Administator!";
                return vm;
            }

            if (must_patphoto && string.IsNullOrWhiteSpace(PicSelected))
            {
                vm.REPORTS.alertMessage = "Patient Photo Must be Captured and attached....RECORD NOT SAVED !";
                return vm;
            }

            if (msection != "1")
            {
                vm.REPORTS.alertMessage = "User Defined Function Conflict...NOT A FRONT DESK OFFICER!";
                return vm;
            }

            //save records

            //pleaseWait.Hide();
            //this.btnsave.Enabled = false;
            //DialogResult result = MessageBox.Show("Confirm to Save...", "Attendance Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            //if (result == DialogResult.Yes)
            //{

            if (newrec && (bissclass.IsDigitsOnly(vm.REPORTS.txtreference.Trim()) ||
                bissclass.IsDigitsOnly(vm.REPORTS.txtreference.Substring(1, 8)))) //update ref. counter and get a new value, if necessary.
            {
                decimal xnum = Convert.ToDecimal(vm.REPORTS.txtreference.Substring(1, 8));
                if (xnum >= mlastno)
                {
                    decimal lastnosave = mlastno;
                    mlastno = msmrfunc.getcontrol_lastnumber("CHARGNO", 3, true, mlastno, false);
                    if (mlastno != lastnosave)
                        vm.REPORTS.txtreference = bissclass.autonumconfig(mlastno.ToString(), true, "C", "999999999");
                }
            }

            savepatientdetails();

            if (newrec) //update attendance counter
            {
                vm.REPORTS.nmrattendancetoday = msmrfunc.getcontrol_lastnumber("ATTNO", 4, true, vm.REPORTS.nmrattendancetoday, false);
                flag = savemedhrec_link();
            }

            if (flag)
            {
                vm.REPORTS.alertMessage = "Record Saved Successfully...";
            }
            //ClearControls("R");
            //this.txtreference.Focus();
            //}

            return vm;
        }

        void savepatientdetails()
        {
            bchain = billchaindtl.Getbillchain(vm.REPORTS.txtpatientno, vm.REPORTS.txtgroupcode);

            string xtype =
                (vm.REPORTS.REPORT_TYPE1 == "chkMedicalTreatment" ? "M" :
                   vm.REPORTS.REPORT_TYPE1 == "chkMedicalExams" ? "E" :
                   vm.REPORTS.REPORT_TYPE1 == "chkDressing" ? "D" :
                   vm.REPORTS.REPORT_TYPE1 == "chkSpecialistConsult" ? "S" :
                   vm.REPORTS.REPORT_TYPE1 == "chkEmergency" ? "G" :
                   vm.REPORTS.REPORT_TYPE1 == "chkFollowup" ? "F" :
                   vm.REPORTS.REPORT_TYPE1 == "chkInjections" ? "I" :
                   vm.REPORTS.REPORT_TYPE1 == "chkDrugRefill" ? "R" : ""
                );

            string start_time = DateTime.Now.ToLongTimeString();

            mrattendWrite(newrec, vm.REPORTS.txtreference, vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, vm.REPORTS.TXTPATIENTNAME, DateTime.Now.Date, vm.SYSCODETABSvm.ServiceCentreCodes.name, (mgrouphtype == "C") ? customers.Custno : patients.patientno, mgrouphtype, false, false, mgrouphtype == "C" ? " " : bchain.GHGROUPCODE, "", "", "", woperator, DateTime.Now, xtype, false, "", "");

            bool result = WriteLINK3(vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno,
                DateTime.Now, vm.REPORTS.TXTPATIENTNAME, "ATTENDANCE", vm.REPORTS.txtreference,
                start_time, start_time, "1A", vm.SYSCODETABSvm.ServiceCentreCodes.name,
                start_time, woperator);

            if (newrec && bchain.GROUPHTYPE == "C" && customers.Tosignbill)
            {
                SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = "bsdetail_Add";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@REFERENCE", vm.REPORTS.txtreference);
                insertCommand.Parameters.AddWithValue("@GROUPCODE", bchain.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@PATIENTNO", bchain.PATIENTNO);
                insertCommand.Parameters.AddWithValue("@NAME", bchain.NAME);
                insertCommand.Parameters.AddWithValue("@TRANS_DATE", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@SIGNEDBILL", false);
                insertCommand.Parameters.AddWithValue("@GROUPHEAD", bchain.GROUPHEAD);
                insertCommand.Parameters.AddWithValue("@OPERATOR", woperator);
                insertCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@AMOUNT", 0m);
                insertCommand.Parameters.AddWithValue("@AUTH_CODE", "");
                insertCommand.Parameters.AddWithValue("@AUTH_DATE", "");

                connection.Open();
                insertCommand.ExecuteNonQuery();
                connection.Close();
            }

            //if (new string[] { "D", "I", "R" }.Contains(xtype))
            //{
            //    string xstr = xtype == "D" ? "WOUND DRESSING" : xtype == "I" ? "INJECTION" : "DRUG REFILL";
            //    frmInjectionAlert injalrt = new frmInjectionAlert(xstr + " ALERT!!!", bchain.GROUPCODE, bchain.PATIENTNO, bchain.NAME, txtreference.Text, woperator, "INJECTION/TREATMENT ALERT FROM FRONT DESK !");
            //    injalrt.ShowDialog();
            //}
        }


        private bool mrattendWrite(bool newrec, string reference, string groupcode, string patientno, string patname, DateTime transdate, string clinic, string grouphead, string grouphtype, bool vitalstaken, bool isposted, string ghgroupcode, string doctor, string docstime, string diag, string xoperator, DateTime operatordatetime, string attendtype, bool sendeclusive, string referrer, string hmoauthorizationcode)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "Mrattend_Add" : "Mrattend_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", reference);
            insertCommand.Parameters.AddWithValue("@groupcode", groupcode);
            insertCommand.Parameters.AddWithValue("@patientno", patientno);
            insertCommand.Parameters.AddWithValue("@name", patname);
            insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@clinic", clinic);
            insertCommand.Parameters.AddWithValue("@billed", "");
            insertCommand.Parameters.AddWithValue("@grouphead", grouphead);
            insertCommand.Parameters.AddWithValue("@grouphtype", grouphtype);
            insertCommand.Parameters.AddWithValue("@vtaken", vitalstaken);
            insertCommand.Parameters.AddWithValue("@posted", isposted);
            insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@GHGROUPCODE", ghgroupcode);
            insertCommand.Parameters.AddWithValue("@doctor", doctor);
            insertCommand.Parameters.AddWithValue("@doc_time", docstime);
            insertCommand.Parameters.AddWithValue("@diagnosis", diag);
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@dtime", operatordatetime);
            insertCommand.Parameters.AddWithValue("@attendtype", attendtype);
            insertCommand.Parameters.AddWithValue("@SENDEXCL", sendeclusive);
            insertCommand.Parameters.AddWithValue("@referrer", referrer);
            insertCommand.Parameters.AddWithValue("@authorizedcode", hmoauthorizationcode);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("" + ex, "Update Daily Attendance Details", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }


        private bool WriteLINK3(string GroupCodeID, string PatientID, DateTime xdate, string patname, string xsection, string xreference,
            string xtimesent, string xselected, string xrectype, string xfacility, string xtimein, string xoperator)
        {
            //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
            DateTime dtmin_date = DateTime.Now;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "LINK3_Add";
            insertCommand.Connection = connection; //00:00:00
            insertCommand.CommandType = CommandType.StoredProcedure;
            int hours = Convert.ToInt32(xtimein.Substring(0, 2)), minutes = Convert.ToInt32(xtimein.Substring(3, 2)),
                seconds = Convert.ToInt32(xtimein.Substring(6, 2));
            TimeSpan T1 = new TimeSpan(hours, minutes, seconds);
            /*    string timenow = DateTime.Now.ToLongTimeString();
                string spent = DateTime.Now.Subtract(T1).ToShortTimeString();*/
            string spent1 = DateTime.Now.Subtract(T1).ToLongTimeString();
            insertCommand.Parameters.AddWithValue("@groupcode", GroupCodeID);
            insertCommand.Parameters.AddWithValue("@patientno", PatientID);
            insertCommand.Parameters.AddWithValue("@name", patname);
            insertCommand.Parameters.AddWithValue("@trans_date", xdate);
            insertCommand.Parameters.AddWithValue("@posted", false);
            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
            insertCommand.Parameters.AddWithValue("@section", xsection);
            insertCommand.Parameters.AddWithValue("@timesent", xtimesent);
            insertCommand.Parameters.AddWithValue("@timein", xtimein);
            insertCommand.Parameters.AddWithValue("@timeselected", xselected);
            insertCommand.Parameters.AddWithValue("@reference", xreference);
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@facility", xfacility);
            insertCommand.Parameters.AddWithValue("@timespent", spent1);
            insertCommand.Parameters.AddWithValue("@rectype", xrectype);


            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show(" " + ex, "Add Attendance Monitor Details", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;

        }


        bool savemedhrec_link()
        {
            bool flag = true;
            bchain = billchaindtl.Getbillchain(vm.REPORTS.txtpatientno, vm.REPORTS.txtgroupcode);
            DateTime dtmin_date = DateTime.Now;

            medhrec = Medhrec.GetMEDHREC(bchain.GROUPCODE, bchain.PATIENTNO);

            bool tosendlink = false;
            bool newmedhrec = (medhrec == null) ? true : false;
            if (newrec && (msection == "1" || msection == "3" || msection == "4"))
            {
                if (ancrecord && vm.SYSCODETABSvm.ServiceCentreCodes.name.Trim() == mancclinic.Trim() && autoANClabreq) //auto generate ANC lab request as defined - capitol request
                {
                    generateANCRequest(bchain);
                }

                SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = (newmedhrec) ? "Medhrec_Add" : "Medhrec_Update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@patientno", vm.REPORTS.txtpatientno);
                insertCommand.Parameters.AddWithValue("@groupcode", vm.REPORTS.txtgroupcode);
                insertCommand.Parameters.AddWithValue("@clinic", vm.SYSCODETABSvm.ServiceCentreCodes.name);
                insertCommand.Parameters.AddWithValue("@date1", (newmedhrec) ? dtmin_date : medhrec.DATE2);
                insertCommand.Parameters.AddWithValue("@date2", (newmedhrec) ? dtmin_date : medhrec.DATE3);
                insertCommand.Parameters.AddWithValue("@date3", (newmedhrec) ? dtmin_date : medhrec.DATE4);
                insertCommand.Parameters.AddWithValue("@date4", (newmedhrec) ? dtmin_date : medhrec.DATE5);
                insertCommand.Parameters.AddWithValue("@clinic1", (newmedhrec) ? "" : medhrec.CLINIC2);
                insertCommand.Parameters.AddWithValue("@clinic2", (newmedhrec) ? "" : medhrec.CLINIC3);
                insertCommand.Parameters.AddWithValue("@clinic3", (newmedhrec) ? "" : medhrec.CLINIC4);
                insertCommand.Parameters.AddWithValue("@clinic4", (newmedhrec) ? "" : medhrec.CLINIC);
                insertCommand.Parameters.AddWithValue("@date5", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@date6", (newmedhrec) ? dtmin_date : medhrec.DATE6);
                insertCommand.Parameters.AddWithValue("@followup", false);
                insertCommand.Parameters.AddWithValue("@date7", (newmedhrec) ? dtmin_date : medhrec.DATE7);
                insertCommand.Parameters.AddWithValue("@clinic7", (newmedhrec) ? "" : medhrec.CLINIC7);


                try
                {
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                    //return true;
                    flag = true;
                }
                catch (SqlException ex)
                {
                    // throw ex;
                    vm.REPORTS.alertMessage = " " + ex + "Please Update Attendance Tracker!!!   Not Saved!!!";
                    //return false;
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }

                //generate consultation fee if autogcons and topaycons
                tosendlink = (dttrans_date == DateTime.Now.Date) ? true : false; //current daily attendance

                if (mautogcons && topaycons)
                    generateConsultationFee(newmedhrec, bchain);
                if (vm.REPORTS.REPORT_TYPE1 == "chkEmergency" || vm.REPORTS.REPORT_TYPE1 == "chkInjections" || vm.REPORTS.REPORT_TYPE1 == "chkDressing" ) //Dressing/Injection :
                                                                                          //See nurses for dressing/injection How do we identify them ? 03/06/2008
                                                                                          //WE SEND TO PHARMACY -
                                                                                          // 29/05/2012 we send to Nurses  Nurses should take vitals of patient and click cancel on
                                                                                          //on Vital Screen.  They should not send patient to Doctor.
                    xtosection = "3";
                else if (vm.REPORTS.REPORT_TYPE1 == "chkDrugRefill") //drug Refill - 23-01-2013
                    xtosection = "8"; //send to pharmacy only					
                else if (Recs_ToCashOffice && mautogcons && topaycons && cashpaying)
                    xtosection = "2"; //goes to cash office
                else
                    xtosection = msection == "1" && (opdNurseActive || vm.SYSCODETABSvm.ServiceCentreCodes.name.Trim() == mancclinic.Trim()) ? "3" : (mdocson) ? "4" : "5";
            }

            //process link
            if (tosendlink)
            {
                int xfunc = opdNurseActive || vm.SYSCODETABSvm.ServiceCentreCodes.name.Trim() == mancclinic.Trim() ? 1 : 9;
                LINK.WriteLINK(0, vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, vm.REPORTS.TXTPATIENTNAME, xtosection, vm.REPORTS.txtreference, 0, 0, vm.SYSCODETABSvm.ServiceCentreCodes.name, false, "", false, (vm.SYSCODETABSvm.ServiceCentreCodes.name.Trim() == mimmunizationclinic.Trim()) ? 20 : xfunc, "", msection, woperator);
            }


            return flag;
        }

        void generateANCRequest(billchaindtl bchain)
        {
            string ancprocedureE, ancprocedureM, nameE, nameM;
            decimal amtE = 0m, amtM = 0m;
            bool monthly = false;
            nameE = nameM = "";
            bool newmedhrec = (medhrec == null) ? true : false;

            if (!newmedhrec && DateTime.Now.Date.Month == medhrec.DATE5.Date.Month && DateTime.Now.Date.Year == medhrec.DATE5.Year) //she came less than a month ago. supposed she attended another clinic ? 09-08-2014 - capitol warri
                monthly = false;
            else
                monthly = true;

            DataTable dtcontrol = Dataaccess.GetAnytable("", "MR", "select fccode from mrcontrol", false);
            ancprocedureE = dtcontrol.Rows[3]["fccode"].ToString().Trim();
            ancprocedureM = dtcontrol.Rows[5]["fccode"].ToString().Trim();

            DataTable dttariff = Dataaccess.GetAnytable("", "MR", "select name, reference, category, amount from tariff where rtrim(reference) = '" + ancprocedureE + "'", false);

            amtE = (decimal)dttariff.Rows[0]["amount"];
            nameE = dttariff.Rows[0]["name"].ToString();

            string tariffcategory = bissclass.seeksay("select name from servicecentrecodes where rtrim(type_code) = '" + dttariff.Rows[0]["category"].ToString().Trim() + "'", "CODES", "NAME");

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            connection.Open();
            for (int i = 0; i < 2; i++)
            {
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = "suspense_Add";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@reference", vm.REPORTS.txtreference);
                insertCommand.Parameters.AddWithValue("@name", vm.REPORTS.TXTPATIENTNAME);
                insertCommand.Parameters.AddWithValue("@itemno", Convert.ToDecimal(i + 1));
                insertCommand.Parameters.AddWithValue("@DESCRIPTION", dttariff.Rows[0]["name"].ToString());
                insertCommand.Parameters.AddWithValue("@rectype", "D");
                insertCommand.Parameters.AddWithValue("@process", dttariff.Rows[0]["reference"].ToString());
                insertCommand.Parameters.AddWithValue("@groupcode", bchain.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@patientno", bchain.PATIENTNO);
                insertCommand.Parameters.AddWithValue("@grouphead", bchain.GROUPHEAD);
                insertCommand.Parameters.AddWithValue("@transtype", bchain.GROUPHTYPE);
                insertCommand.Parameters.AddWithValue("@doctor", "");
                insertCommand.Parameters.AddWithValue("@facility", dttariff.Rows[0]["category"].ToString());
                insertCommand.Parameters.AddWithValue("@posted", false);
                insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
                insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
                insertCommand.Parameters.AddWithValue("@amount", (Decimal)dttariff.Rows[0]["amount"]);
                insertCommand.Parameters.AddWithValue("@ghgroupcode", bchain.GHGROUPCODE);
                insertCommand.Parameters.AddWithValue("@title", bchain.TITLE);
                insertCommand.Parameters.AddWithValue("@address1", bchain.RESIDENCE);
                insertCommand.Parameters.AddWithValue("@currency", "");
                insertCommand.Parameters.AddWithValue("@exrate", 0m);
                insertCommand.Parameters.AddWithValue("@fcamount", 0m);
                insertCommand.Parameters.AddWithValue("@duration", 0);
                insertCommand.Parameters.AddWithValue("@billprocess", "");
                insertCommand.Parameters.AddWithValue("@notes", "");
                insertCommand.Parameters.AddWithValue("@servicetype", "");
                insertCommand.Parameters.AddWithValue("@capitated", false);
                insertCommand.Parameters.AddWithValue("@groupeditem", false);
                insertCommand.Parameters.AddWithValue("@grpbillbyservtype", false);
                insertCommand.Parameters.AddWithValue("@AGE", bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now.Date));
                insertCommand.Parameters.AddWithValue("@SEX", bchain.SEX);
                insertCommand.Parameters.AddWithValue("@PHONE", "");
                insertCommand.Parameters.AddWithValue("@EMAIL", "");

                insertCommand.ExecuteNonQuery();

                if (i == 0 && !string.IsNullOrWhiteSpace(ancprocedureM))
                {
                    dttariff = Dataaccess.GetAnytable("", "MR", "select name, reference, category, amount from tariff where rtrim(reference) = '" + ancprocedureM + "'", false);
                    amtM = (decimal)dttariff.Rows[0]["amount"];
                    nameM = dttariff.Rows[0]["name"].ToString();
                }
                else
                    break;

            }

            connection.Close();
            LINK.WriteLINK(0, vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, vm.REPORTS.TXTPATIENTNAME, "6", vm.REPORTS.txtreference, 0, 0, dttariff.Rows[0]["category"].ToString(), false, "", false, 0, "", msection, woperator);
            string xstring = monthly ? " and " + nameM.Trim() : "";

            if (bchain.GROUPHTYPE == "P")
            {
                vm.REPORTS.ActRslt = "Please Inform Patient to go to Cash Office and thereafter To " + tariffcategory.Trim() + " for " + nameE.Trim() + xstring + "...";
            }
            else
            {
                vm.REPORTS.ActRslt = "Please Inform Patient to go to the " + tariffcategory.Trim() + " for " + nameE.Trim() + xstring + "...";
            }

            DataTable medhist = Dataaccess.GetAnytable("", "MR", "SELECT comments from medhist where groupcode = '" + bchain.GROUPCODE + "' and patientno = '" + bchain.PATIENTNO + "' and trans_date = '" + DateTime.Now.ToShortDateString() + "'", false);
            bool xnew = medhist.Rows.Count < 1 ? true : false;
            xstring = "=> OPD ANC Inv./Proc. Request Details - " + DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString() + "  " + woperator.Trim() + " @ FrontDesk \r\n 1. " + nameE + " (" + amtE.ToString("N2") + ") -> " + tariffcategory.Trim();
            if (monthly)
            {
                xstring += "\r\n" + "2. " + nameM + " (" + amtM.ToString("N2") + ") -> " + tariffcategory.Trim();
            }
            xstring = medhist.Rows.Count > 0 ? medhist.Rows[0]["comments"].ToString().Trim() : "";
            xstring += "\r\n" + xstring;

            MedHist.updatemedhistcomments(bchain.GROUPCODE, bchain.PATIENTNO, DateTime.Now.Date, xstring, xnew, vm.REPORTS.txtreference, bchain.NAME, bchain.GHGROUPCODE, bchain.GROUPHEAD, "");
            
            //update bills
            //    string xtd = "";

            decimal mitem = 0, oldamt = 0;
            int recid = 0;
            for (int i = 0; i < 2; i++)
            {
                if (i == 0 && amtE > 0 || i == 1 && amtM > 0)
                {
                    mitem = Billings.getBillNextItems(vm.REPORTS.txtreference, mconscode, true, ref oldamt, ref recid);

                    Billings.writeBILLS(newrec, vm.REPORTS.txtreference, mitem, mconscode, i == 0 ? nameE : nameM, mgrouphtype, i == 0 ? amtE : amtM, DateTime.Now.Date, bchain.NAME, bchain.GROUPHEAD, dttariff.Rows[0]["category"].ToString(), vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, "D", bchain.GHGROUPCODE, woperator, DateTime.Now, "", "", 0m, 0, "", "", false, "", "", 0m, "", 0m, "O", false, recid);
                }
            }
        }

        //private void msgBoxHandler(object sender, EventArgs e)
        //{
        //    Form msgForm = sender as Form;
        //    if (msgForm != null)
        //    {
        //        if (msgeventtracker == "PS" && msgForm.DialogResult == DialogResult.Yes) //TO SAVE
        //        {
        //            if (newrec) //update ref. counter and get a new value, if necessary.
        //            {
        //                decimal lastnosave = mlastno;
        //                mlastno = msmrfunc.getcontrol_lastnumber("CHARGNO", 3, true, mlastno, false);
        //                if (mlastno != lastnosave)
        //                    this.txtreference.Text = bissclass.autonumconfig(mlastno.ToString(), true, "C", "999999999");

        //            }
        //            this.savepatientdetails();
        //            if (newrec) //update attendance counter
        //            {
        //                nmrattendancetoday.Value = msmrfunc.getcontrol_lastnumber("ATTNO", 4, true, nmrattendancetoday.Value, false);
        //                savemedhrec_link();
        //            }

        //            ClearControls("R");
        //            this.txtreference.Focus();
        //            return;

        //        }
        //        else if (msgeventtracker == "g")
        //        {
        //            txtpatientno.Text = "";
        //            this.txtgroupcode.Select();
        //            return;
        //        }
        //        else if (msgeventtracker == "RN")
        //        {
        //            ClearControls("R");  //txtreference.Text = txtgroupcode.Text = txtpatientno.Text = "";
        //            txtreference.Select();
        //            return;
        //        }
        //        else if (msgeventtracker == "RD" && msgForm.DialogResult == DialogResult.Yes) //TO DELETE
        //        {
        //            Mrattend.DeleteMrattend(txtreference.Text);
        //            this.ClearControls("R");
        //            txtreference.Select();
        //            return;
        //        }
        //        else if (msgeventtracker == "MA") //Multiple Attendance
        //        {
        //            if (msgForm.DialogResult == DialogResult.No)
        //            {
        //                this.ClearControls("R");
        //                txtreference.Select();
        //            }
        //            else
        //            { combclinic.Select(); }
        //            return;
        //        }
        //        else
        //        {
        //            // this.txtpatientno.Text = "";
        //            // this.txtpatientno.Focus();
        //            return;
        //        }
        //    }

        //}

        //public void ClearControls(string xtype)
        //{

        //    this.txtname.Text = this.txtaddress.Text = dtlastattend.Text = this.rtxtspecialinstructinos.Text = lblage.Text = txtgroupheadname.Text = "";
        //    this.nmbalance.Value = this.nmrcredit.Value = this.nmrdebit.Value = nmrCreditLimit.Value = 0m;
        //    nmrCreditLimit.Visible = lblCreditLimit.Visible = false;
        //    this.combclinic.SelectedItem = -1;
        //    if (xtype == "R")
        //        txtreference.Text = txtgroupcode.Text = txtpatientno.Text = "";
        //    if (xtype == "P")
        //        txtgroupcode.Text = txtpatientno.Text = "";
        //    pictureBox1.Image = null;
        //    if (panel_EnrolleeDetails.Visible)
        //    {
        //        panel_EnrolleeDetails.Visible = false;
        //        //   lblEnrolleeNumber.Text = bchain.STAFFNO;
        //        //   lblEnrolleePhone.Text = bchain.PHONE;
        //    }
        //}


        //void checkmultipleattendance()
        //{
        //    //&&25/07/2011 - check for multiple attendance for today
        //    //msgeventtracker = "MA";
        //    // Mrattend patientattendrec = new Mrattend();
        //    // patientattendrec = Mrattend.GetMultiattend(this.txtgroupcode.Text, this.txtpatientno.Text, DateTime.Now.Date);
        //    string selectStatement = "SELECT reference, name, clinic, operator, dtime FROM MRATTEND WHERE " +
        //       "patientno = '" + txtpatientno.Text + "' AND groupcode = '" + txtgroupcode.Text + "' AND CONVERT(DATE,TRANS_DATE) = '" + DateTime.Now.ToShortDateString() + "'";
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", selectStatement, false);
        //    if (dt.Rows.Count < 1)
        //        return;
        //    DialogResult result = MessageBox.Show("Multiple Attendance Registration Detected for this patient for Today! @ " + dt.Rows[0]["dtime"].ToString() + "...!!! \r\n CREATE A DUPLICATE ATTENDANCE RECORD FOR TODAY ?", "Multiple Attendance Registraton", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.No)
        //    {
        //        this.ClearControls("R");
        //        txtreference.Text = "";
        //        txtreference.Select();
        //    }
        //    result = MessageBox.Show("ARE YOU REALLY SURE YOU WANT TO DISTORT CLINIC VISIT STATISTICS FOR TODAY...?", "MULTILE CLICK VISIT FOR TODAY", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.No)
        //    {
        //        this.ClearControls("R");
        //        txtreference.Text = "";
        //        txtreference.Select();
        //    }
        //    { combclinic.Select(); }
        //    return;
        //}

        //void checkcreditlimit()
        //{

        //}

        //    private void btndelete_Click(object sender, EventArgs e)
        //    {

        //        /*  if (patients.posted) //check if patient has right to delete
        //{
        //	MessageBox.Show("Record Can't be Delete...");
        //	return;
        //} */

        //        DataTable dbillv = Billings.GetBILLING(txtreference.Text);
        //        if (dbillv.Rows.Count > 0)
        //        {
        //            DialogResult result = MessageBox.Show("This Attendance Record cannot be deleted...Bills on this Accounts will be compromised!");
        //        }
        //        else
        //        {
        //            //  msgeventtracker = "RD";
        //            DialogResult result = MessageBox.Show("Confirm Delete...", "Patient Daily Attendance Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            if (result == DialogResult.Yes)
        //            {
        //                Mrattend.DeleteMrattend(txtreference.Text);
        //                this.ClearControls("R");
        //                txtreference.Select();
        //                return;
        //            }
        //        }
        //    }

        //private void initcomboboxes()
        //{
        //    //get clinic
        //    this.combclinic.DataSource = dtclinics; // Dataaccess.GetAnytable("", "CODES", "select type_code, name from servicecentrecodes", true);
        //    combclinic.ValueMember = "Type_code";
        //    combclinic.DisplayMember = "name";

        //}

        //private void combclinic_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(combclinic.Text))
        //        return;
        //    // decimal xanc_consult = 0;
        //    //we check from here if patient is ANC Registered
        //    topaycons = true;
        //    if (chkMedicalExams.Checked || chkDressing.Checked || chkInjections.Checked || chkFollowup.Checked || chkDrugRefill.Checked)
        //    //medical exams,dressing,injection/followup/Drug Refill/disabled auto consult generate
        //    {
        //        topaycons = false;
        //        btnsave.Enabled = true;
        //        return;
        //    }
        //    else if (cons_fee_at_docs || combclinic.SelectedValue.ToString().Trim() == mimmunizationclinic.Trim())
        //    {
        //        topaycons = false;
        //        btnsave.Enabled = true;
        //        return;
        //    }
        //    else if (chkSpecialistConsult.Checked) //specialist
        //    {
        //        topaycons = true;
        //        btnsave.Enabled = true;
        //        return;
        //    }
        //    if (skipConsult4NewReg && bchain.REG_DATE.Date == DateTime.Now.Date)
        //    {
        //        topaycons = false;
        //        btnsave.Enabled = true;
        //        return;
        //    }
        //    ancrecord = false;
        //    if (combclinic.SelectedValue.ToString().Trim() == mancclinic.Trim())
        //    {
        //        string xrtnancref = "";
        //        ancrecord = ANCREG.GetANCREG(txtpatientno.Text, txtgroupcode.Text, ref rtnstring, ref xrtnancref);
        //        if (!ancrecord) //problem
        //        {
        //            DialogResult result = MessageBox.Show(
        //                "This Patient is not on ANC Registration Profiles... \r\n If you are sure she had been registered, her GROUPCODE or PATIENTNO may have \r\n been changed by Record Officers between the Original ANC Registration and this Attendance...\r\n Please Check and Rectify !!! \r\n YOU MUST NOT ATTEMPT RE-REGISTRATION ON THE ANTE-NATAL REGISTER TO AVOID LOSS OF EXISTING \r\n ANTE-NATAL NOTES/RECORDS ON PREVIOUS REGISTRATION \r\n, ANTE-NATAL REGISTRATION");
        //            this.ClearControls("R");
        //            txtreference.Focus();
        //            return;
        //        }
        //        if (ancreg.EVERYVISITCONSULT)
        //        {
        //            txtconsultamt.Text = ancreg.CONSULTAMT.ToString();
        //            topaycons = true;
        //        }
        //        else
        //        {
        //            txtconsultamt.Text = ancreg.CONSULTAMT.ToString();
        //            topaycons = false;
        //        }
        //        btnsave.Enabled = true;
        //        return;
        //    }

        //    if (!mautogcons || bchain.GROUPHTYPE == "C" && customers.Consultation < 1 && !chkSpecialistConsult.Checked)
        //    {
        //        topaycons = false;
        //        btnsave.Enabled = true;
        //        return;
        //    }
        //    //check for monthy consultation flag for all patients
        //    if (monthlyconsultflag)
        //    {
        //        //no consultaton. date5 is checked here because update to date4 has not taken place, date4 works if atdocsconsult 8/6/2012
        //        if (medhrec == null) //new
        //            topaycons = true;
        //        else
        //            topaycons = (DateTime.Now.Year == medhrec.DATE5.Year && DateTime.Now.Month == medhrec.DATE5.Month) ? false : true;

        //        btnsave.Enabled = true;
        //        return;
        //    }
        //    if (!ancrecord && (medhrec == null || combclinic.SelectedValue.ToString().Trim() == "" && mconsbyclinic))
        //        topaycons = true;

        //    else if (medhrec != null && medhrec.DATE7 >= dtmin_date.Date && medhrec.DATE7 == DateTime.Now.Date && medhrec.CLINIC7.Trim() == combclinic.SelectedValue.ToString().Trim()) //follow up
        //    {
        //        chkFollowup.Checked = true;
        //        topaycons = false;
        //    }
        //    btnsave.Enabled = true;
        //}

        private void generateConsultationFee(bool xnewmedhrec, billchaindtl bchain)
        {
            // msmrfunc.mrGlobals.waitwindowtext = "Generating Consultation Charge... Pls Wait !!!";
            // pleaseWait.Show();
            conscode_save = mconscode;
            string mdescription = "CONSULTATION", xcons_code = mconscode, xdesc = "", facility = "";
            decimal mamount = 0, mitem = 0;
            bool isbillonaccount = false;
            billchaindtl BillOnAcct = new billchaindtl();

            patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);


            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtconsultamt)) //anc consultation charge for every visist	&&30/05/2012 capitol
                mamount = Convert.ToDecimal(vm.REPORTS.txtconsultamt);
            else if (vm.REPORTS.REPORT_TYPE1 == "chkSpecialistConsult") //specialist:we check if its by clinic and get amount accordingly.30/05/2012
            {
                mamount = msmrfunc.getSpecialistConsultCharge(bchain, vm.SYSCODETABSvm.ServiceCentreCodes.name);
                mdescription = "SPECIALIST CONSULT - " + vm.SYSCODETABSvm.ServiceCentreCodes.name.Trim();
            }
            else if (bchain.GROUPHTYPE == "C") //we check for coporate clients tariff 8/6/2012
            {
                if (!customers.HMO)
                    mamount = customers.Consultation;
                else
                    mamount = getHmoConsultFee();
            }
            else //general consult
                mamount = Calc_Cons(@xcons_code);

            if (mamount == 0) //go to general tariff for consultation fee
                mamount = msmrfunc.getFeefromtariff(mconscode, bchain.PATCATEG, ref xdesc, ref facility);

            if (mamount >= 1)  //it could be negative - not paying 
            {
                /*  we now check for special discount to the company or patient
					select iif(Billchai.grouphtype="P","patient","customer")
					if discount > 0 .and. !empty(mamount)
					*wait window nowait noclear "Discounted :"+alltrim(str(discount,6,2))+"% on "+alltrim(str(thisform.nmramount.value,12,2))
					xdisc = (mamount*discount)/100
					store mamount-xdisc to mamount
					ENDIF */
                decimal oldamt = 0m;
                int recid = 0;
                decimal discount = (mgrouphtype == "P") ? patients.discount : customers.Discount;
                if (discount != 0)
                {
                    decimal xdiscount = (discount >= 1) ? (mamount * discount) / 100 : 0;
                    mamount = mamount - xdiscount;
                }
                //now update billing file
                if (bchain.BILLONACCT != "")
                {
                    BillOnAcct = billchaindtl.Getbillchain(bchain.BILLONACCT, bchain.GROUPCODE);
                    isbillonaccount = (BillOnAcct != null) ? true : false;
                }
                mitem = Billings.getBillNextItems(vm.REPORTS.txtreference, mconscode, true, ref oldamt, ref recid);

                writeBILLS(newrec, vm.REPORTS.txtreference, mitem, mconscode, mdescription, mgrouphtype, 
                    mamount, dttrans_date, (isbillonaccount) ? BillOnAcct.NAME : bchain.NAME, 
                    (mgrouphtype == "C") ? customers.Custno : patients.patientno, vm.SYSCODETABSvm.ServiceCentreCodes.name, 
                    vm.REPORTS.txtgroupcode, (isbillonaccount) ? BillOnAcct.PATIENTNO : vm.REPORTS.txtpatientno, 
                    "D", (mgrouphtype == "C") ? " " : patients.groupcode, woperator, DateTime.Now, "", "", 0m, 0, "", 
                    "", false, "", "", 0m, "", 0m, "O", false, recid);

                //		mcumbil = amount
                // DialogResult result = MessageBox.Show()
                //check flag for FD to Cash Office
                if (Recs_ToCashOffice) //yes, they pay before Nurses Desk
                {
                    string xstr = bchain.GROUPHTYPE == "P" && patients.bill_cir == "C" ? "\r\n\r\n Pls Inform Patient to Go To Cash Office \r\n\r\n To Make Payment for Consultation" : "";
                    vm.REPORTS.REPORT_TYPE2 = "Consultation Fee Generated for " + mamount.ToString("N2") + xstr + "...";
                }
            }
            //  pleaseWait.Hide();

        }



        public static void writeBILLS(bool xnewrec, string xreference, decimal xitem, string xprocess, string xdescription,
            string xgrouphtype, decimal xamount, DateTime xdate, string xname, string xgrouphead, string xfacility, string xgroupcode, string xpatientno, string debitcredit_CD, string xghgroupcod, string xoperator, DateTime xop_time, string xextdesc, string xcurrency, decimal xexrate, int xfxtype, string xdiag, string xdoctor, bool xposted, string xpayref, string xservicetyp, decimal xpayment, string xpaytype, decimal xfcamount, string in_outpatient, bool receipted, int recid)
        {
            //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
            DateTime dtmin_date = DateTime.Now;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (xservicetyp == "b") ? "capbills_Add" : (xnewrec) ? "Billing_Add" : "Billing_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Reference", xreference);
            insertCommand.Parameters.AddWithValue("@patientno", xpatientno);
            insertCommand.Parameters.AddWithValue("@name", xname);
            insertCommand.Parameters.AddWithValue("@Itemno", xitem);
            insertCommand.Parameters.AddWithValue("@diag", xdiag);
            insertCommand.Parameters.AddWithValue("@process", xprocess);
            insertCommand.Parameters.AddWithValue("@description", xdescription);
            insertCommand.Parameters.AddWithValue("@doctor", xdoctor);
            insertCommand.Parameters.AddWithValue("@facility", xfacility);
            insertCommand.Parameters.AddWithValue("@amount", xamount);
            insertCommand.Parameters.AddWithValue("@trans_date", xdate);
            insertCommand.Parameters.AddWithValue("@sec_level", 0m);
            insertCommand.Parameters.AddWithValue("@posted", (xnewrec) ? false : xposted);
            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
            insertCommand.Parameters.AddWithValue("@receipted", receipted);
            insertCommand.Parameters.AddWithValue("@transtype", xgrouphtype);
            insertCommand.Parameters.AddWithValue("@payref", xpayref);
            insertCommand.Parameters.AddWithValue("@grouphead", xgrouphead);
            insertCommand.Parameters.AddWithValue("@servicetype", xservicetyp);
            insertCommand.Parameters.AddWithValue("@payment", xpayment);
            insertCommand.Parameters.AddWithValue("@groupcode", xgroupcode);
            insertCommand.Parameters.AddWithValue("@ttype", debitcredit_CD);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", xghgroupcod);
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@op_time", xop_time);
            insertCommand.Parameters.AddWithValue("@currency", xcurrency);
            insertCommand.Parameters.AddWithValue("@exrate", xexrate);
            insertCommand.Parameters.AddWithValue("@fcamount", xfcamount);
            insertCommand.Parameters.AddWithValue("@extdesc", xextdesc);
            insertCommand.Parameters.AddWithValue("@Accounttype", xservicetyp); // in_outpatient);
            if (!xnewrec)
                insertCommand.Parameters.AddWithValue("@RECID", recid);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("SQL access" + ex, "BILLINGS UPDATE", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }

        }

        //private decimal getmultspc_consult(bool xnewmedhrec) //string xclinic, string xgrouphead, string xgrouphtype,bool xnewmedhrec,string xpatientno)
        //{
        //    //IF billchai.grouphtype="P" &&Privae
        //    //21/01/2012 - we scan to ascertain if its followup or fresh visit. 
        //    //Patient may have seen more than one specialist ? We must scan thru the last 7 visits
        //    msmrfunc.getSpecialistConsultCharge(bchain, combclinic.SelectedValue.ToString());
        //}
        //    if (!xnewmedhrec) // new attendance  IF !EMPTY(m.lastattend)
        //    {
        //        if (medhrec.DATE4 > dtmin_date.Date && medhrec.CLINIC4.Trim() != "" && medhrec.CLINIC4.Trim() == combclinic.SelectedValue.ToString().Trim()) //Last visit b4 this one.
        //            xlastday = Convert.ToInt32(DateTime.Now.Date.Subtract(medhrec.DATE4.AddDays(1)));
        //        else if (medhrec.DATE3 > dtmin_date.Date && medhrec.CLINIC3 != "" && medhrec.CLINIC4.Trim() == combclinic.SelectedValue.ToString().Trim())
        //            xlastday = Convert.ToInt32(DateTime.Now.Date.Subtract(medhrec.DATE3.AddDays(1)));
        //        else if (medhrec.DATE2 > dtmin_date.Date && medhrec.CLINIC2 != "" && medhrec.CLINIC2.Trim() == combclinic.SelectedValue.ToString().Trim())
        //            xlastday = Convert.ToInt32(DateTime.Now.Date.Subtract(medhrec.DATE2.AddDays(1)));
        //        else if (medhrec.DATE1 > dtmin_date.Date && medhrec.CLINIC1 != "" && medhrec.CLINIC1.Trim() == combclinic.SelectedValue.ToString().Trim())
        //            xlastday = Convert.ToInt32(DateTime.Now.Date.Subtract(medhrec.DATE1.AddDays(1)));
        //    }
        //    //get specialist consultation details from spcprofile
        //    SqlConnection cs = Dataaccess.mrConnection();
        //    string selcommand = "SELECT facility,followupdays,followupamt,amount,diffcharge FROM SPCPROFILE WHERE rtrim(FACILITY) = '" + combclinic.SelectedValue.ToString().Trim()+"'";
        //    SqlCommand selectCommand = new SqlCommand(selcommand, cs);
        //    try
        //    {
        //        cs.Open();
        //        SqlDataReader reader = selectCommand.ExecuteReader();

        //        while (reader.Read())
        //        {

        //            xamt = (xlastday > 0 && xlastday < (Int32)reader["followupdays"]) ? (decimal)reader["followupamt"] : (decimal)reader["amount"];
        //            xdiffcharge = (bool)reader["diffcharge"];
        //            break;
        //        }

        //        reader.Close();
        //    }
        //    catch (SqlException ex)
        //    {
        //        MessageBox.Show("" + ex);
        //    }
        //    finally
        //    {
        //        cs.Close();
        //    }
        //    if (bchain.GROUPHTYPE == "P" || !xdiffcharge)
        //        return xamt;
        //    //corporate search
        //    SqlConnection cs2 = Dataaccess.mrConnection();
        //    string selcommand2 = "SELECT facility,followupdays,followupamt,amount,customer FROM SPCDETAIL WHERE rtrim(FACILITY) = '"+ combclinic.SelectedValue.ToString().Trim() + "' and customer = '" + bchain.GROUPHEAD+"'";
        //    SqlCommand selectCommand2 = new SqlCommand(selcommand2, cs2);
        //    try
        //    {
        //        cs.Open();
        //        SqlDataReader reader = selectCommand2.ExecuteReader();

        //        while (reader.Read())
        //        {

        //            xamt = (xlastday > 0 && xlastday < (Int32)reader["followupdays"]) ? (decimal)reader["followupamt"] : (decimal)reader["amount"];
        //            //xdiffcharge = (bool)reader["diffcharge"];
        //            break;
        //        }

        //        reader.Close();
        //    }
        //    catch (SqlException ex)
        //    {
        //        MessageBox.Show("" + ex);
        //    }
        //    finally
        //    {
        //        cs2.Close();
        //    }
        //    return xamt;

        //}

        decimal getHmoConsultFee()
        {
            decimal xamt = 0;
            //  SqlConnection cs2 = Dataaccess.mrConnection();

            string selcommand2 = "SELECT consultation,custno FROM HMODETAIL WHERE custno = '" + customers.Custno + 
                "' AND rtrim(hmoservtype) = '" + bchain.HMOSERVTYPE.Trim() + "'";

            DataTable dt = Dataaccess.GetAnytable("", "MR", selcommand2, false);

            if (dt.Rows.Count > 0)
            {
                xamt = (decimal)dt.Rows[0]["consultation"];
            }
            /*      SqlCommand selectCommand2 = new SqlCommand(selcommand2, cs2);
                  try
                  {
                      cs2.Open();
                      SqlDataReader reader = selectCommand2.ExecuteReader();

                      while (reader.Read())
                      {

                          xamt = (decimal)reader["consultation"];
                          break;
                      }

                      reader.Close();
                  }
                  catch (SqlException ex)
                  {
                      MessageBox.Show("" + ex);
                  }
                  finally
                  {
                      cs2.Close();
                  }*/
            return (xamt == 0) ? customers.Consultation : xamt;

        }

        decimal Calc_Cons(string xconscode) //30/05/2012 - charges in systems setup must be set to 0
        {
            decimal xinit_cons_duration_adult, consamt = 0m;

            /* NOTE: if consuldaton fee (initial or subsequent) is empty in setup, system goes;
				to tariff to pick consultation charge, thereby giving oppurnity;
				for graduated or descrimating charge */

            xinit_cons_duration_adult = (init_cons_duration_adult == 0m) ? 9999999m : init_cons_duration_adult;
            sub_cons_duration_adult = (sub_cons_duration_adult == 0m) ? 9999999m : sub_cons_duration_adult;

            if (medhrec == null || medhrec.DATE5 == dtmin_date.Date) //DateTime.MinValue)
            {
                if (init_cons_fee_adult == 0m)
                    xconscode = mconscode;
                else
                    consamt = init_cons_fee_adult;
            }
            else if ((medhrec.DATE5 != dtmin_date.Date && medhrec.DATE4 == dtmin_date.Date &&
                Convert.ToDecimal(DateTime.Now.Date.Subtract(medhrec.DATE5.Date).TotalDays.ToString()) > xinit_cons_duration_adult ||
                medhrec.DATE4 != dtmin_date.Date && Convert.ToDecimal(DateTime.Now.Date.Subtract(medhrec.DATE5.Date).TotalDays.ToString()) >
                sub_cons_duration_adult))
            {
                if (sub_cons_fee_adult == 0m)
                    xconscode = sub_cons_code_adult;
                else
                    consamt = sub_cons_fee_adult;
            }
            else if (mconsbyclinic && medhrec.CLINIC != "" && medhrec.CLINIC.Trim() != vm.SYSCODETABSvm.ServiceCentreCodes.name.Trim())
            {
                if (sub_cons_fee_adult == 0m)
                    xconscode = sub_cons_code_adult;
                else
                    consamt = sub_cons_fee_adult;
            }
            conscode_save = xconscode;
            return consamt;
        }

        //private void chkgetdependants_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtpatientno.Text) || string.IsNullOrWhiteSpace(txtgroupcode.Text))
        //        return;

        //    //  string xgh = (bchain.PATIENTNO.Trim() == bchain.GROUPHEAD.Trim()) ? txtname.Text.Trim() : bchain.GROUPHEAD.Trim() + " : " + txtgroupheadname.Text.Trim();
        //    chkgetdependants.Checked = false;
        //    getdependants GetDependants = new getdependants("REGISTERED DEPENDANTS OF :" + bchain.NAME + " [" + bchain.GROUPCODE.Trim() + ":" + bchain.PATIENTNO.Trim() + "]", txtpatientno.Text, bchain.GROUPHEAD);
        //    GetDependants.Closed += new EventHandler(GetDependants_Closed);
        //    GetDependants.ShowDialog();

        //}

        //void GetDependants_Closed(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //    this.txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
        //    this.txtpatientno.Select();
        //}

        //private void btnprint_Click(object sender, EventArgs e)
        //{
        //    frmAttendanceList attendlist = new frmAttendanceList();
        //    attendlist.Show();
        //}

        //private void btnledger_Click(object sender, EventArgs e)
        //{
        //    string xref = mrattend == null || mrattend.REFERENCE != txtreference.Text ? "" : txtreference.Text;
        //    rptfrmBillings rptbilling = new rptfrmBillings(5, xref, txtgroupcode.Text, txtpatientno.Text);
        //    rptbilling.Show();
        //}

        //private void combclinic_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(combclinic.Text))
        //        return;
        //    combclinic_LostFocus(null, null);
        //}

        //private void txtreference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    if (string.IsNullOrWhiteSpace(txtreference.Text))
        //        return;
        //    txtreference_LostFocus(null, null);

        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtpatientno_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtpatientno_LostFocus(null, null);
        //}

        //private void txtgroupcode_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.F2)
        //    {
        //        btngroupcode.PerformClick();
        //    }
        //}

        //private void txtpatientno_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.F2)
        //    {
        //        btnpatientlookup.PerformClick();
        //    }
        //}

        //string agecalc(DateTime xdate1Low, DateTime xdate2High)
        //{
        //    //int xx = 0;
        //    double xyear, xmonth, xdays, xx;
        //    //  string xstrpd, returnval;
        //    DateTime xdate1 = xdate1Low, xdate2 = xdate2High; ;
        //    // x = xdate2.Year - xdate1.Year; //+" Yrs"

        //    //if (xdate2.Date.Subtract(xdate1.Date).TotalDays < 365)
        //    //    xstrpd = " Mths";
        //    //else
        //    //    xstrpd = " Yrs";
        //    xdays = xdate2.Subtract(xdate1).TotalDays;
        //    xyear = xdays > 365 ? xdays / 365 : 0;
        //    decimal xY = 0, xM = 0, xD = 0;
        //    if (xyear > 0)
        //    {
        //        xY = Convert.ToInt32(Math.Floor(xyear));
        //    }
        //    string xrtn = "";
        //    if (xdays < 30)
        //        xrtn = Convert.ToInt32(xdays).ToString() + "d";
        //    else
        //    {
        //        xx = xdays % 365;
        //        xmonth = xx > 30 ? xx / 30 : 0;
        //        xM = Convert.ToInt32(Math.Floor(xmonth));
        //        xdays = xx > 0 ? xx % 30 : 0;
        //        xD = Convert.ToInt32(Math.Floor(xdays));
        //        if (xyear > 12)
        //            xrtn = xY.ToString() + "Yrs";
        //        else
        //            xrtn = xY.ToString() + "y/" + Convert.ToInt32(xM).ToString() + "m/" + Convert.ToInt32(xD).ToString() + "d";
        //    }
        //    return xrtn;
        //}

    }
}
/*
 
		}
	  set order to tag attref
	IF billchai.grouphtype='P' &&it takes too much for corporate accts
		thisform.calc_op_bal()
	ENDIF
	&&check for credit limit
	xcredit = IIF(billchai.grouphtype="P",Patient.cr_limit,Customer.cr_limit)
	IF enforcecreditlimit AND xcredit > 0 AND ThisForm.nmrbalance.Value > xcredit
		if messagebox( "Transactions not allowed for this Patient... CREDIT LIMIT EXCEEDED ! "+chr(13)+chr(13)+;
			" ...Overwrite required !  CONTINUE... ?",4+32+0,'CREDIT LIMIT RESTRICTION')=6
			SELECT 0
			openfile( 'MRstlev.dat',.f.)
			set order to tag name
			do form ar_overwrite with "Overwrite to Data Control" && to x
			if empty(anycode) &&anycode global var is returned
				use
				return .f.
			ENDIF
			openfile( 'ovprofile.dat',.f.)
			APPEND BLANK
			replace reference WITH ThisForm.txtreference.Value,groupcode WITH billchai.groupcode,;
			patientno WITH billchai.patientno,grouphtype WITH billchai.grouphtype,GHGROUPCODE WITH billchai.GHGROUPCODE,;
			grouphead WITH billchai.grouphead,crlimit WITH xcredit,balance WITH ThisForm.nmrbalance.Value,;
			operator WITH anycode,dtime WITH DATETIME(),name WITH billchai.name
			use
		else
			return .f.
		endif
	ENDIF
 
	select medhrec
	if seek(thisform.txtgroupcode.value+this.value)
		ThisForm.dtattendancedate.value = date5 &&//last attendance date
		lastattend = recno()
	endif
   * ThisForm.cmdledger.enabled = .t.
	IF !thisform.dispcurrency()
		RETURN .f.
	ENDIF
	IF billchai.grouphtype='C' AND customer.trackform 
		DO FORM attendform WITH .t. to xrtn
		IF !xrtn
			RETURN .f.
		ENDIF
	ENDIF
	IF !EMPTY(m.global_clinic_code) AND m.global_clinic_code # 'ALL' &&autonomous clinic - aviation for kupa 01/06/2011
		ThisForm.txtclinic.Value = m.global_clinic_code
	ENDIF
	*ThisForm.savekey.enabled = .T.
	IF ThisForm.chkgetdependants.Value = 1
		DO FORM getdependants WITH this.Value,billchai.name
		ThisForm.chkgetdependants.Value = 0 
		IF !EMPTY(anycode)
			this.Value = anycode
			RETURN .f.
		ENDIF
	ENDIF

			*/
