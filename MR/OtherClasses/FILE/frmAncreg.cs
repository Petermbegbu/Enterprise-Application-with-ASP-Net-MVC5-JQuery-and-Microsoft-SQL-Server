#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using mradmin;
using mradmin.DataAccess;
using mradmin.BissClass;
using mradmin.Forms;
using msfunc;
using msfunc.Forms;
using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmAncreg
    {
        ANCREG ancreg = new ANCREG();
        billchaindtl bchain = new billchaindtl();
        Customer customers = new Customer();
        patientinfo patients = new patientinfo();
        MedHist medhist = new MedHist();
        ANC01 anc01 = new ANC01();

        // PleaseWaitForm pleaseWait = new PleaseWaitForm();

        string AnyCode, Anycode1, mancregbillcode, manccode, mancbcode, msgeventtracker,
            mgrouphtype, mgrouphead, lookupsource, mbill_cir, msection, woperator;
        decimal mlastno;
        bool newrec, mautogenreg, mcanadd, mcandelete, mcanalter;
        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start.Year < 2000 ? bissclass.ConvertStringToDateTime("01", "01", "2011") : msmrfunc.mrGlobals.mta_start;
        //DateTime blankdate = new DateTime(0000, 00, 00);
        string[] drga_ = new string[10], procedure_ = new string[10];
        int drgcounter, invcounter;

        MR_DATA.MR_DATAvm vm;

        public frmAncreg(string woperato, string section)
        {
            vm = new MR_DATA.MR_DATAvm();

            msection = section; // msmrfunc.mrGlobals.msection;
            woperator = woperato; // msmrfunc.mrGlobals.WOPERATOR;

            //InitializeComponent();
            getcontrolsettings();

        }

        //private void frmAncreg_Load(object sender, EventArgs e)
        //{
        //    getcontrolsettings();

        //    initcomboboxes();
        //}

        private void getcontrolsettings()
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select pvtcode, autogreg from mrcontrol order by recid", false); // msmrfunc.getcontrolsetup("MR");

            mancbcode = dt.Rows[1]["pvtcode"].ToString();
            manccode = dt.Rows[2]["pvtcode"].ToString();

            mautogenreg = (bool)dt.Rows[7]["autogreg"];

            mancregbillcode = dt.Rows[7]["pvtcode"].ToString();

            dt = Dataaccess.GetAnytable("", "MR", "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];
        }

        //private void initcomboboxes()
        //{
        //    //get stock
        //    combdrugs.DataSource = Dataaccess.GetAnytable("", "SMS", "SELECT DISTINCT name, item from stock", true);
        //    combdrugs.ValueMember = "Item";
        //    combdrugs.DisplayMember = "Name";

        //    //group procedure
        //    combprocedures.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT reference,name from Tariff", true);
        //    combprocedures.ValueMember = "Reference";
        //    combprocedures.DisplayMember = "Name";

        //    reinitArray();
        //}

        //void reinitArray()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        drga_[i] = "";
        //        procedure_[i] = "";
        //    }
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
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
        //    }
        //    else if (btn.Name == "btnpatientlookup")
        //    {
        //        this.txtpatientno.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else
        //    {
        //        this.txtreference.Text = "";
        //        lookupsource = "R";
        //        msmrfunc.mrGlobals.crequired = "N";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR ANTE-NATAL CLINIC REGISTRATIONS";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "g") //groupcodee
        //    {
        //        this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //        this.txtgroupcode.Focus();
        //    }

        //    else if (lookupsource == "L") //patientno
        //    {
        //        this.txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Focus();
        //    }
        //    else if (lookupsource == "R") //daiy attendance
        //    {
        //        this.txtreference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtreference.Focus();
        //    }
        //}

        //private void txtreference_Enter(object sender, EventArgs e)
        //{
        //    //  ClearControls("R");
        //    // pleaseWait.Hide();

        //    if (string.IsNullOrWhiteSpace(AnyCode)) //no lookup
        //    {
        //        txtreference.Text = "";
        //        mlastno = msmrfunc.getcontrol_lastnumber("ATTNO", 2, false, mlastno, false) + 1;
        //        {
        //            txtreference.Text = mlastno.ToString();
        //        }
        //    }

        //    drgcounter = invcounter = 0;
        //    AnyCode = "";
        //}

        //private void txtreference_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtreference.Text))
        //    {
        //        mlastno = msmrfunc.getcontrol_lastnumber("ATTNO", 2, false, mlastno, false) + 1;
        //        txtreference.Text = mlastno.ToString();
        //    }
        //}


        //private void txtreference_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtreference.Text))
        //        return;
        //    DialogResult result;

        //    if (bissclass.IsDigitsOnly(txtreference.Text.Trim()))
        //    {
        //        if (Convert.ToDecimal(this.txtreference.Text) > mlastno)
        //        {
        //            result = MessageBox.Show("ANC Registeration Reference is out of Sequence...");
        //            txtreference.Text = "";
        //            return;
        //        }
        //        this.txtreference.Text = bissclass.autonumconfig(this.txtreference.Text, true, " ", "999999999");
        //    }
        //    newrec = true;
        //    //check if reference exist
        //    this.ClearControls("P");
        //    AnyCode = Anycode1 = "";
        //    msgeventtracker = "RN";
        //    ancreg = ANCREG.GetANCREG(txtreference.Text);
        //    if (ancreg == null) //new defintion
        //    {
        //        // msmrfunc.mrGlobals.waitwindowtext = "NEW ANC RECORD ...";
        //        //  MessageBox.Show("NEW ANC RECORD ...");
        //        txtedd.Visible = txtlmp.Visible = false;
        //        // Display form modelessly
        //        //  pleaseWait.Show();
        //    }
        //    else
        //    {
        //        string xposted = (ancreg.POSTED) ? " and Posted Can't be amended !" : "";
        //        //msmrfunc.mrGlobals.waitwindowtext = "Record Exists"+xposted;
        //        newrec = false;
        //        // pleaseWait.Show();
        //        mgrouphtype = ancreg.GROUPHTYPE;
        //        txtpatientno.Text = ancreg.PATIENTNO;
        //        txtgroupcode.Text = ancreg.GROUPCODE;
        //        mgrouphead = ancreg.GROUPHEAD;
        //        dtdateregistered.Value = ancreg.REG_DATE;
        //        bchain = billchaindtl.Getbillchain(this.txtpatientno.Text, txtgroupcode.Text);
        //        dtlmp.Value = ancreg.LMP;
        //        dtedd.Value = ancreg.EDD;
        //        if (ancreg.LMP <= dtmin_date)
        //        {
        //            txtedd.Visible = txtlmp.Visible = true;
        //            //dtlmp.Text = ""; // blankdate.ToString();
        //            //dtedd.Text = ""; // blankdate.ToString();.

        //        }
        //        if (ancreg.DEL_DATE <= dtmin_date)
        //        {
        //            anc01 = ANC01.GetANC01(ancreg.REFERENCE);
        //            if (anc01 != null && anc01.DEL_DATE != ancreg.DEL_DATE)
        //            {
        //                string updatestr = "update ancreg set del_date = '" + anc01.DEL_DATE + "' where reference = '" + txtreference.Text + "'";
        //                bissclass.UpdateRecords(updatestr, "MR");
        //                MessageBox.Show("Delivery Date Adjusted...");
        //            }
        //        }
        //        chkvisitconsultcharge.Checked = (ancreg.EVERYVISITCONSULT) ? true : false;
        //        nmrConsultAmt.Value = ancreg.CONSULTAMT;
        //        txtdeliverydate.Text = (ancreg.DEL_DATE <= dtmin_date) ? "" : ancreg.DEL_DATE.ToString();
        //        DisplayDetails("S");
        //        if (ancreg.DEL_DATE > dtmin_date)
        //        {
        //            result = MessageBox.Show("This Record is closed... Patient delivered on " + ancreg.DEL_DATE.ToString(), "ANC RECORD");

        //            return;
        //        }
        //        result = MessageBox.Show("Record Exists" + xposted);
        //        if (ancreg.POSTED)
        //        {
        //            ClearControls("R");
        //            txtreference.Focus();
        //            return;
        //        }
        //        btnDelete.Enabled = true;
        //    }
        //    btnSave.Enabled = true;
        //    txtgroupcode.Focus();
        //}


        //private void txtpatientno_LostFocus(object sender, EventArgs e)
        //{
        //    //if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    //{
        //    //    AnyCode = "";
        //    //    return;
        //    //}

        //    //DialogResult result;

        //    newrec = true;

        //    //if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))
        //    //    this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");
        //    //AnyCode = "";

        //    //check if patientno exists
        //    bchain = billchaindtl.Getbillchain(this.txtpatientno.Text, txtgroupcode.Text);
        //    if (bchain == null)
        //    {
        //        result = MessageBox.Show("Invalid Patient Number... ");
        //        txtpatientno.Text = " ";
        //        txtgroupcode.Focus();
        //        return;
        //    }
        //    else
        //    {
        //        mgrouphtype = bchain.GROUPHTYPE;
        //        mgrouphead = bchain.GROUPHEAD;
        //        if (!DisplayDetails(""))
        //        {
        //            txtgroupcode.Text = "";
        //            txtgroupcode.Focus();
        //            return;
        //        }
        //    }

        //    //check for active anc registration
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT del_date, reference, REG_DATE, reg_time from ancreg where groupcode = '" + bchain.GROUPCODE + "' and patientno = '" + bchain.PATIENTNO + "'", false);
        //    bool foundit = false;
        //    string rtntext = "";
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (Convert.ToDateTime(dt.Rows[i]["del_date"]).Date <= dtmin_date && dt.Rows[i].ToString() != txtreference.Text)
        //            {
        //                foundit = true;
        //                rtntext = dt.Rows[i]["reference"].ToString().Trim() + " of " +
        //                    Convert.ToDateTime(dt.Rows[i]["REG_DATE"]).ToString("dd-MM-yyyy") + " @ " + dt.Rows[i]["reg_time"].ToString().Substring(0, 5);
        //                break;
        //            }
        //        }
        //        if (foundit)
        //        {
        //            result = MessageBox.Show("An Active ANC Record found for This Patient...Check ANC Ref : " + rtntext + " ...", "Duplicate ANC Registration");
        //            ClearControls("R");
        //            txtreference.Focus();
        //            return;
        //        }
        //    }

        //    btnSave.Enabled = true;
        //    dtdateregistered.Focus();
        //}


        //private bool DisplayDetails(string xtype)
        //{
        //    DialogResult result;
        //    if (mgrouphtype == "P")
        //    {
        //        patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);
        //        if (patients == null)
        //        {
        //            result = MessageBox.Show("Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS", "GroupHead Informaton");
        //            txtpatientno.Text = "";
        //            this.txtgroupcode.Focus();
        //            return false;
        //        }

        //    }
        //    else
        //    {
        //        customers = Customer.GetCustomer(bchain.GROUPHEAD);
        //        if (customers == null)
        //        {
        //            result = MessageBox.Show("Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS", "GroupHead Informaton");
        //            txtpatientno.Text = "";
        //            this.txtgroupcode.Focus();
        //            return false;
        //        }
        //    }

        //    this.txtgroupcode.Text = bchain.GROUPCODE;
        //    this.txtname.Text = bchain.NAME;
        //    mbill_cir = (mgrouphtype == "C") ? customers.Bill_cir : patients.bill_cir;
        //    this.txtgroupheadname.Text = (mgrouphtype == "P" && bchain.GROUPHEAD == bchain.PATIENTNO) ?
        //        "< SELF >" : (mgrouphtype == "C") ? customers.Name : patients.name;

        //    if (mgrouphtype == "P" && bchain.PATIENTNO == patients.patientno)
        //    {
        //        rtxtaddress.Text = patients.address1.Trim() + "\r\n";
        //    }
        //    else
        //        rtxtaddress.Text = bchain.RESIDENCE;

        //    if (bchain.SECTION != "")
        //        rtxtaddress.Text = rtxtaddress.Text + bchain.SECTION + "\r\n";
        //    if (bchain.DEPARTMENT != "")
        //        rtxtaddress.Text = rtxtaddress.Text + bchain.DEPARTMENT.Trim() + "\r\n";
        //    if (bchain.STAFFNO != "")
        //        rtxtaddress.Text = rtxtaddress.Text + "[Staff # :" + bchain.STAFFNO.Trim() + " ]";
        //    if (xtype == "S") //display from existing reference
        //        return true;
        //    if (bchain.SEX.Substring(0, 1) != "F" || bchain.RELATIONSH == "C" || DateTime.Now.Year - bchain.BIRTHDATE.Year < 18 || bchain.M_STATUS == "S")
        //    {
        //        if (bchain.SEX.Substring(0, 1) != "F")
        //        {
        //            result = MessageBox.Show("Patient Cannot be registered for ANTE-NATAL - Registered as a Male...", "Patient's Record Inconsistency");
        //            return false;
        //        }
        //        else
        //        {
        //            result = MessageBox.Show("Patient Cannot be registered for ANTE-NATAL : \r\n Check Age or Marital Status on Registeration... Continue with ANC Registration anyhow ?", "Patient's Record Inconsistency", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            if (result == DialogResult.No)
        //                return false;
        //        }
        //    }

        //    if (bchain.STATUS == "C")
        //    {
        //        result = MessageBox.Show("PATIENT NOT VALID FOR TRANSACTION - < Cancelled >");
        //        txtpatientno.Text = "";
        //        txtgroupcode.Focus();
        //        return false;
        //    }
        //    if (bchain.STATUS == "S")
        //    {
        //        result = MessageBox.Show("PATIENT NOT VALID FOR TRANSACTION - < Suspended > " +
        //         " CONFIRM TO CONTINUE...", "PATIENT STATUS", MessageBoxButtons.YesNo,
        //                   MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.No)
        //        {
        //            ClearControls("R");
        //            txtreference.Select();
        //            return false;
        //        }
        //        frmOverwrite overwrite = new frmOverwrite("Overwrite  Suspended Registration", "mrstlev", "MR");
        //        overwrite.ShowDialog();
        //        if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode)) //anycode global var is returned
        //            return false;
        //        //update overwrite profile
        //        msmrfunc.updateOverwrite(txtreference.Text, "Overwrite Suspended Registration at ANC Booking", bchain, bchain.GROUPHTYPE == "P" ? patients.cr_limit : customers.Cr_limit, 0m);
        //    }

        //    return true;
        //}



        /*      if (xtype == "S") //drgs and procedure selections for existing record 11-01-2014
              {
                  string[] arr = new string[3];
                  ListViewItem itm;
                  string drg,serv;
                  for (int i = 1; i < 11; i++)
                  {
                      drg = "ancreg.drug" + i.ToString().Trim();
                      serv = "ancreg.service" + i.ToString().Trim();

                      arr[0] = i.ToString();
                      arr[1] = drg;

                      itm = new ListViewItem(arr);
                      listView1.Items.Add(itm); 

              }
              //&&CHECK FOR EXPIRY DATE IF RE-REGISTRATION IS ENABLED
                 if ( misrereg &&  EMPTY(bchain.EXPIRYDATE ? && bchain.expirydate <= DateTime.Now() )
                    MessageBox.Show( "PATIENT NOT VALID FOR TRANSACTION - Registration Expired on "+bchain.EXPIRYDATE.ToString(),
                         "Patient's Re-Registration Check" );
                         txtgroupcode.Focus();
                    return;*/


        //private void dtdateregistered_Leave(object sender, EventArgs e)
        //{
        //    msgeventtracker = "RD";
        //    if (dtdateregistered.Value.Date > DateTime.Now.Date)
        //    {
        //        MessageBox.Show("Registration Date cannot be greater than Today...", "ANC Registration Date", msgBoxHandler);
        //        dtdateregistered.Value = DateTime.Now.Date;
        //    }
        //}

        //public void ClearControls(string xtype)
        //{

        //    this.txtname.Text = this.rtxtaddress.Text =
        //        txtgroupheadname.Text = combprocedures.Text = combdrugs.Text = txtdeliverydate.Text = "";
        //    listView_drgs.Items.Clear();
        //    listView_procedures.Items.Clear();
        //    //  chkalldrgandservices.Checked = chkalldrugs.Checked = chkspecify.Checked = chkspecify.Checked =
        //    //      chkallservices.Checked = chkvisitconsultcharge.Checked = false;
        //    chkvisitconsultcharge.Checked = false;
        //    dtlmp.Value = dtedd.Value = DateTime.Now.Date;
        //    nmrConsultAmt.Value = 0m;
        //    //this.nmbalance.Value = this.nmrcredit.Value = this.nmrdebit.Value = 0;
        //    //this.combclinic.SelectedItem = -1;
        //    if (xtype == "R")
        //    {
        //        txtreference.Text = txtgroupcode.Text = txtpatientno.Text = "";
        //        txtreference.Focus();
        //        return;
        //    }
        //    if (xtype == "P")
        //    {
        //        txtgroupcode.Text = txtpatientno.Text = "";
        //    }

        //}


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
        //                mlastno = msmrfunc.getcontrol_lastnumber("ATTNO", 2, true, mlastno, false);
        //                if (mlastno != lastnosave)
        //                    this.txtreference.Text = bissclass.autonumconfig(mlastno.ToString(), true, "C", "999999999");
        //            }
        //            savedetails();
        //            ClearControls("R");
        //            this.txtreference.Focus();
        //            return;

        //        }
        //        else if (msgeventtracker == "g")
        //        {
        //            txtpatientno.Text = "";
        //            this.txtgroupcode.Focus();
        //            return;
        //        }
        //        else if (msgeventtracker == "RD")
        //        {
        //            dtdateregistered.Focus();
        //            return;
        //        }
        //        else if (msgeventtracker == "ARD" && msgForm.DialogResult == DialogResult.Yes) //TO DELETE
        //        {
        //            ANCREG.DeleteANCREG(txtreference.Text);
        //            this.ClearControls("R");
        //            txtreference.Focus();
        //            return;
        //        }
        //        else if (msgeventtracker == "R")
        //        {
        //            txtreference.Focus();
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


        public MR_DATA.REPORTS btnSave(MR_DATA.BILLCHAIN dataObject, IEnumerable<MR_DATA.DISPSERV> stockList, 
            IEnumerable<MR_DATA.HMODETAIL> procedureList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            decimal mlastno = Convert.ToDecimal(dataObject.PHONE);

            //msgeventtracker = "R";

            // pleaseWait.Hide();
            if (!string.IsNullOrWhiteSpace(dataObject.CURRENCY) && Convert.ToDateTime(dataObject.CURRENCY) <= Convert.ToDateTime(dataObject.OPERATOR) )
            {
                vm.REPORTS.alertMessage = "Invalid date specification on Delivery Date! Pls Check ";
                return vm.REPORTS;
            }

            //if (!bissclass.IsPresent(this.txtgroupcode, "Patients Groupcode", false) ||
            //    !bissclass.IsPresent(this.txtpatientno, "Patient Number", false) ||
            //    !bissclass.IsPresent(this.txtname, "Patients Name", false) ||
            //    !bissclass.IsPresent(this.txtgroupheadname, "Bills Payable By", false) ||
            //    !bissclass.IsPresent(this.txtreference, "Reference", false))
            //    return;

            newrec = dataObject.STATUS == "true" ? true : false;

            if (newrec && !mcanadd)
            {
                vm.REPORTS.alertMessage = "ACCESS DENIED To New Record Creation.  See your Systems Administator!";
                return vm.REPORTS;
            }


            //msgeventtracker = "PS";
            //   if (dtlmp.Value.Date >= DateTime.Now.Date)
            //   {

            vm.REPORTS.REPORT_TYPE1 = @String.Format("{0:yyyy-MM-dd}", dtmin_date);  //for dtlmp.Value and dtedd.Value

            //vm.REPORTS.REPORT_TYPE1 = dtmin_date; // blankdate; for dtlmp.Value and dtedd.Value;
            //dtedd.Value = dtmin_date; //  blankdate; for dtedd.Value
            //   }
            //convert selected drugs and selected to arrays for update

            bool chkSpecify = dataObject.POSTED == true ? true : false;

            if (chkSpecify)
            {
                int i = 0;
                foreach (var list1 in stockList)
                {
                    drga_[i] = list1.REFERENCE;

                    i++;
                }

                //for (int i = 0; i < listView_drgs.Items.Count; i++)
                //{
                //    drga_[i] = listView_drgs.Items[i].SubItems[2].ToString();
                //}

                i = 0;
                foreach (var list2 in procedureList)
                {
                    procedure_[i] = list2.CUSTNO;

                    i++;
                }

                //for (int i = 0; i < listView_procedures.Items.Count; i++)
                //{
                //    procedure_[i] = listView_procedures.Items[i].SubItems[2].ToString();
                //}
            }

            //save records
            // pleaseWait.Hide();

            //DialogResult result = MessageBox.Show("Confirm to Save...", "ANC Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            vm.REPORTS.cmbsave = false; //for procedure_[i]

            if (newrec && bissclass.IsDigitsOnly(dataObject.STAFFNO.Trim()) && Convert.ToDecimal(dataObject.STAFFNO.Trim()) == mlastno)
            {
                decimal lastnosave = mlastno;
                mlastno = getcontrol_lastnumber("ATTNO", 2, true, mlastno, false) + 1;
                if (mlastno != lastnosave)
                    vm.REPORTS.txtreference = bissclass.autonumconfig(mlastno.ToString(), true, "", "999999999");
            }

            savedetails(dataObject);

            return vm.REPORTS;
        }

        private static Decimal getcontrol_lastnumber(string xfield, int xrow, bool xupdate, decimal currentVal, bool toreset)
        {
            DataTable dt = Dataaccess.GetAnytable("mrcontrol", "MR", "SELECT " + xfield + " FROM MRcontrol WHERE RECID = " + xrow, false);
            Decimal xval = (Decimal)dt.Rows[0][xfield];
            xval++;

            if (xupdate) //we add 1 to whatever that is retrieved; currentval is previous retrieval+1
            {
                string updatestring = "update mrcontrol set " + xfield + " = '" + xval + "' where recid = '" + xrow + "'";
                bissclass.UpdateRecords(updatestring, "MR");
            }

            return xval;
        }


        void savedetails(MR_DATA.BILLCHAIN dataObject)
        {
            ancreg = ANCREG.GetANCREG(dataObject.STAFFNO);
                 
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "ANCREG_Add" : "ANCREG_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Reference", dataObject.STAFFNO);
            insertCommand.Parameters.AddWithValue("@patientno", dataObject.PATIENTNO);
            insertCommand.Parameters.AddWithValue("@name", dataObject.NAME);
            insertCommand.Parameters.AddWithValue("@grouphead", dataObject.DEPARTMENT);
            insertCommand.Parameters.AddWithValue("@groupcode", dataObject.GROUPCODE);
            insertCommand.Parameters.AddWithValue("@grouphtype", dataObject.SECTION);
            insertCommand.Parameters.AddWithValue("@lmp", dtmin_date.Date);
            insertCommand.Parameters.AddWithValue("@edd", dtmin_date.Date);
            insertCommand.Parameters.AddWithValue("@del_date", (newrec) ? dtmin_date.Date : Convert.ToDateTime(dataObject.CURRENCY));
            insertCommand.Parameters.AddWithValue("@cummcharge", (newrec) ? 0m : ancreg.CUMMCHARGE);
            insertCommand.Parameters.AddWithValue("@payments", (newrec) ? 0m : ancreg.PAYMENTS);
            insertCommand.Parameters.AddWithValue("@charge", (newrec) ? 0m : ancreg.CHARGE);
            insertCommand.Parameters.AddWithValue("@lastattend", (newrec) ? DateTime.Now.Date : ancreg.LASTATTEND);
            insertCommand.Parameters.AddWithValue("@CUMMATTEND", 0);
            insertCommand.Parameters.AddWithValue("@drug1", drga_[0]);
            insertCommand.Parameters.AddWithValue("@drug2", drga_[1]);
            insertCommand.Parameters.AddWithValue("@drug3", drga_[2]);
            insertCommand.Parameters.AddWithValue("@drug4", drga_[3]);
            insertCommand.Parameters.AddWithValue("@drug5", drga_[4]);
            insertCommand.Parameters.AddWithValue("@drug6", drga_[5]);
            insertCommand.Parameters.AddWithValue("@drug7", drga_[6]);
            insertCommand.Parameters.AddWithValue("@drug8", drga_[7]);
            insertCommand.Parameters.AddWithValue("@drug9", drga_[8]);
            insertCommand.Parameters.AddWithValue("@drug10", drga_[9]);
            insertCommand.Parameters.AddWithValue("@alldrugs", (dataObject.RELATIONSH == "true") ? true : false);
            insertCommand.Parameters.AddWithValue("@service1", procedure_[0]);
            insertCommand.Parameters.AddWithValue("@service2", procedure_[1]);
            insertCommand.Parameters.AddWithValue("@service3", procedure_[2]);
            insertCommand.Parameters.AddWithValue("@service4", procedure_[3]);
            insertCommand.Parameters.AddWithValue("@service5", procedure_[4]);
            insertCommand.Parameters.AddWithValue("@service6", procedure_[5]);
            insertCommand.Parameters.AddWithValue("@service7", procedure_[6]);
            insertCommand.Parameters.AddWithValue("@service8", procedure_[7]);
            insertCommand.Parameters.AddWithValue("@service9", procedure_[8]);
            insertCommand.Parameters.AddWithValue("@service10", procedure_[9]);
            insertCommand.Parameters.AddWithValue("@allservice", (dataObject.HMOCODE == "true") ? true : false);
            insertCommand.Parameters.AddWithValue("@posted", false);
            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
            insertCommand.Parameters.AddWithValue("@trans_date", (newrec) ? DateTime.Now.Date : ancreg.TRANS_DATE);
            insertCommand.Parameters.AddWithValue("@reg_date", Convert.ToDateTime(dataObject.OPERATOR));
            insertCommand.Parameters.AddWithValue("@reg_time", newrec ? DateTime.Now.ToLongTimeString() : ancreg.REG_TIME);
            insertCommand.Parameters.AddWithValue("@anchistory", (newrec) ? "" : ancreg.ANCHISTORY);
            insertCommand.Parameters.AddWithValue("@antenatalnotes", (newrec) ? "" : ancreg.ANTENATALNOTES);
            insertCommand.Parameters.AddWithValue("@deliverynotes", (newrec) ? "" : ancreg.DELIVERYNOTES);
            insertCommand.Parameters.AddWithValue("@postnatalnotes", (newrec) ? "" : ancreg.POSTNATALNOTES);
            insertCommand.Parameters.AddWithValue("@operator", woperator);
            insertCommand.Parameters.AddWithValue("@dtime", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@everyvisitconsult", (dataObject.RESIDENCE == "true") ? true : false);
            insertCommand.Parameters.AddWithValue("@consultamt", dataObject.GHGROUPCODE);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", bchain.GHGROUPCODE);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // throw ex;
                vm.REPORTS.alertMessage = "SQL access" + ex;
                return;
            }
            finally
            {
                connection.Close();
            }

            //update anc01
            ancreg = ANCREG.GetANCREG(dataObject.STAFFNO); //we need to pass this to anc01
            bool ancnew;
            anc01 = ANC01.GetANC01(dataObject.STAFFNO);
            ancnew = (anc01 == null) ? true : false;
            anc01Write(ancnew, ancreg, dataObject.STAFFNO);

            if (newrec)
            {
                //update medhist
                string datestr = Convert.ToDateTime(dataObject.OPERATOR) == DateTime.Now.Date ? DateTime.Now.ToString("dd-MM-yyyy @ HH:mmt") : Convert.ToDateTime(dataObject.OPERATOR).ToShortDateString();
                string xstr = (Convert.ToDateTime(dataObject.HMOSERVTYPE) <= dtmin_date.Date) ? "" : " : LMP : " + dataObject.HMOSERVTYPE;
                string xstr1 = (Convert.ToDateTime(dataObject.BILLONACCT) <= dtmin_date.Date) ? "" : " EDD : " + dataObject.BILLONACCT + "\r\n";
                string rtnstring = "==> Ante-Natal Registration - " + dataObject.STAFFNO.Trim() +
                    " Date Regd : " + datestr + xstr + " " + xstr1;

                MedHist medhist = MedHist.GetMEDHIST(dataObject.GROUPCODE, dataObject.PATIENTNO, "", false, true, Convert.ToDateTime(dataObject.OPERATOR), "DESC");
                bool newhist = (medhist == null) ? true : false;
                string xcomments = "";

                if (newhist)
                    xcomments = rtnstring;
                else
                {
                    xcomments = medhist.COMMENTS.Trim() + "\r\n" + rtnstring.Trim();
                }

                MedHist.updatemedhistcomments(dataObject.GROUPCODE, dataObject.PATIENTNO, Convert.ToDateTime(dataObject.OPERATOR), xcomments, newhist, dataObject.STAFFNO, dataObject.NAME, bchain.GHGROUPCODE, bchain.GROUPHEAD, "");

                if (mautogenreg && (Convert.ToDateTime(dataObject.OPERATOR) == DateTime.Now.Date ||
                     (Convert.ToInt32(DateTime.Now.Date - Convert.ToDateTime(dataObject.OPERATOR))) < 7))
                {
                    generate_RegFee(dataObject);
                }
            }

            vm.REPORTS.alertMessage = "Record Submitted Successfuly...";

            //reinitArray();
            //ClearControls("R");
            //vm.REPORTS.txtreference = "";
            //txtreference.Focus();
            return;
        }


        private static bool anc01Write(bool newrec, ANCREG ancreg, string reference)
        {
            //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start.Year < 2000 ? bissclass.ConvertStringToDateTime("01", "01", "2011") : msmrfunc.mrGlobals.mta_start;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "ANC01_Addfrmreg" : "ANC01_Updatefrmreg";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", reference);
            insertCommand.Parameters.AddWithValue("@patientno", ancreg.PATIENTNO);
            insertCommand.Parameters.AddWithValue("@grouphead", ancreg.GROUPHEAD);
            insertCommand.Parameters.AddWithValue("@groupcode", ancreg.GROUPCODE);
            insertCommand.Parameters.AddWithValue("@grouphtype", ancreg.GROUPHTYPE);
            insertCommand.Parameters.AddWithValue("@name", ancreg.NAME);
            insertCommand.Parameters.AddWithValue("@lmp", dtmin_date); // ancreg.LMP );
            insertCommand.Parameters.AddWithValue("@edd", dtmin_date); //  ancreg.EDD ); 
            insertCommand.Parameters.AddWithValue("@BLOODGROUP", "");
            insertCommand.Parameters.AddWithValue("@DEL_DATE", dtmin_date); //for dtmin_date
            insertCommand.Parameters.AddWithValue("@CUMMCHARGE", 0m);
            insertCommand.Parameters.AddWithValue("@PAYMENTS", 0m);
            insertCommand.Parameters.AddWithValue("@CHARGE", 0m);
            insertCommand.Parameters.AddWithValue("@LASTATTEND", dtmin_date); 
            insertCommand.Parameters.AddWithValue("@NEXTVISIT", dtmin_date);  
            insertCommand.Parameters.AddWithValue("@CUMMATTEND", 0m);
            insertCommand.Parameters.AddWithValue("@drug1", ancreg.DRUG1);
            insertCommand.Parameters.AddWithValue("@drug2", ancreg.DRUG2);
            insertCommand.Parameters.AddWithValue("@drug3", ancreg.DRUG3);
            insertCommand.Parameters.AddWithValue("@drug4", ancreg.DRUG4);
            insertCommand.Parameters.AddWithValue("@drug5", ancreg.DRUG5);
            insertCommand.Parameters.AddWithValue("@drug6", ancreg.DRUG6);
            insertCommand.Parameters.AddWithValue("@drug7", ancreg.DRUG7);
            insertCommand.Parameters.AddWithValue("@drug8", ancreg.DRUG8);
            insertCommand.Parameters.AddWithValue("@drug9", ancreg.DRUG9);
            insertCommand.Parameters.AddWithValue("@drug10", ancreg.DRUG10);
            insertCommand.Parameters.AddWithValue("@alldrugs", ancreg.ALLDRUGS);
            insertCommand.Parameters.AddWithValue("@service1", ancreg.SERVICE1);
            insertCommand.Parameters.AddWithValue("@service2", ancreg.SERVICE2);
            insertCommand.Parameters.AddWithValue("@service3", ancreg.SERVICE3);
            insertCommand.Parameters.AddWithValue("@service4", ancreg.SERVICE4);
            insertCommand.Parameters.AddWithValue("@service5", ancreg.SERVICE5);
            insertCommand.Parameters.AddWithValue("@service6", ancreg.SERVICE6);
            insertCommand.Parameters.AddWithValue("@service7", ancreg.SERVICE7);
            insertCommand.Parameters.AddWithValue("@service8", ancreg.SERVICE8);
            insertCommand.Parameters.AddWithValue("@service9", ancreg.SERVICE9);
            insertCommand.Parameters.AddWithValue("@service10", ancreg.SERVICE10);
            insertCommand.Parameters.AddWithValue("@allservice", ancreg.ALLSERVICE);
            insertCommand.Parameters.AddWithValue("@posted", false);
            insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@TRANS_DATE", DateTime.Now.Date);
            insertCommand.Parameters.AddWithValue("@REG_DATE", ancreg.TRANS_DATE);
            insertCommand.Parameters.AddWithValue("@REG_TIME", ancreg.REG_TIME);
            insertCommand.Parameters.AddWithValue("@DOCTOR", "");

            insertCommand.Parameters.AddWithValue("@DURATIONOFPREGNANCY", "");
            insertCommand.Parameters.AddWithValue("@AGE", 0m);
            insertCommand.Parameters.AddWithValue("@TRIBE", "");
            insertCommand.Parameters.AddWithValue("@RELIGION", "");
            insertCommand.Parameters.AddWithValue("@ADDRESS", "");
            insertCommand.Parameters.AddWithValue("@OCCUPATION", "");
            insertCommand.Parameters.AddWithValue("@LEVELOFEDUCATION", 0m);
            insertCommand.Parameters.AddWithValue("@HUSBANDNAME", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDOCCUPATION", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDEMPLOYER", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDLEVELOFEDUCATION", 0m);
            insertCommand.Parameters.AddWithValue("@HUSBANDPHONE", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDGC", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDPATNO", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDBG", "");
            insertCommand.Parameters.AddWithValue("@BOOKINGCATEGORY", "");
            insertCommand.Parameters.AddWithValue("@BOOKINGTAG", 0m);
            insertCommand.Parameters.AddWithValue("@GHGROUPCODE", ancreg.GHGROUPCODE);
            insertCommand.Parameters.AddWithValue("@OPERATOR", ancreg.OPERATOR);
            insertCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@SPNOTES", "");
            insertCommand.Parameters.AddWithValue("@BIRTHDATE", dtmin_date);
            insertCommand.Parameters.AddWithValue("@GENOTYPE", "");
            insertCommand.Parameters.AddWithValue("@MENS_REGULARITY", "");
            insertCommand.Parameters.AddWithValue("@CONTRACEPTIVEUSE", "");
            insertCommand.Parameters.AddWithValue("@RISKFACTOR", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDGENOTYPE", "");
            insertCommand.Parameters.AddWithValue("@MENARCHE", "");

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL access" + ex, "ANTE-NATAL UPDATE", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        void generate_RegFee(MR_DATA.BILLCHAIN dataObject)
        {
            string xdesc = "", facility = "";
            decimal alt_lastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, true, 0m, false) + 1;
            string xreference = bissclass.autonumconfig(alt_lastno.ToString(), true, "", "999999999");
            // string mitem = Billings.getBillNextItems(txtreference.Text, mconscode, true);
            decimal mamount = msmrfunc.getFeefromtariff(mancregbillcode, bchain.PATCATEG, ref xdesc, ref facility);

            Billings.writeBILLS(newrec, dataObject.STAFFNO, 1m, mancregbillcode, xdesc, dataObject.SECTION, mamount, Convert.ToDateTime(dataObject.OPERATOR), bchain.NAME, (dataObject.SECTION == "C") ? customers.Custno : patients.patientno, facility,
                dataObject.GROUPCODE, dataObject.PATIENTNO, "D", bchain.GHGROUPCODE, woperator, DateTime.Now, "", "", 0m, 0, "", "", false, "", "", 0m, "", 0m, "O", false, 0);
            return;
        }

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    if ((string.IsNullOrWhiteSpace(combdrugs.Text) || string.IsNullOrWhiteSpace(combdrugs.SelectedValue.ToString())) &&
        //        (string.IsNullOrWhiteSpace(combprocedures.Text) || string.IsNullOrWhiteSpace(combprocedures.SelectedValue.ToString())))
        //    {
        //        return;
        //    }
                
        //    Button btn = sender as Button;
        //    if (btn.Name == "btnAdd")
        //    {
        //        drgcounter++;
        //        string[] rowd = { drgcounter.ToString(), combdrugs.Text, combdrugs.SelectedValue.ToString() };
        //        ListViewItem itm;
        //        itm = new ListViewItem(rowd);
        //        listView_drgs.Items.Add(itm);
        //    }
        //    else
        //    {
        //        invcounter++;
        //        string[] rowp = { invcounter.ToString(), combprocedures.Text, combprocedures.SelectedValue.ToString() };
        //        ListViewItem itm;
        //        itm = new ListViewItem(rowp);
        //        { listView_procedures.Items.Add(itm); }
        //    }
        //    combdrugs.Text = combprocedures.Text = "";
        //    return;
        //}

        //private void chkspecify_Click(object sender, EventArgs e)
        //{
        //    combprocedures.Enabled = combdrugs.Enabled = btnAdd.Enabled = btnInv.Enabled = true;
        //}


        //private void chkvisitconsultcharge_CheckStateChanged(object sender, EventArgs e)
        //{
        //    if (chkvisitconsultcharge.Checked)
        //        nmrConsultAmt.Enabled = true;
        //    else
        //        nmrConsultAmt.Enabled = false;
        //}


        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (ancreg.POSTED)
        //    {
        //        MessageBox.Show("Record Can't be Deleted...");
        //        return;
        //    }
        //    if (!mcandelete)
        //    {
        //        MessageBox.Show("ACCESS DENIED...To Delete existing Record.  See your Systems Administator!");
        //        return;
        //    }
        //    msgeventtracker = "ARD";
        //    DialogResult result = MessageBox.Show("Confirm to Delete Record", "Ante-Natal Registeration", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, msgBoxHandler);
        //    if (result == DialogResult.No)
        //        return;
        //    ANCREG.DeleteANCREG(txtreference.Text);
        //    this.ClearControls("R");
        //    result = MessageBox.Show("Record Deleted...");
        //    txtreference.Focus();
        //    return;
        //}

        //private void chkvisitconsultcharge_Click(object sender, EventArgs e)
        //{
        //    if (chkvisitconsultcharge.Checked)
        //        nmrConsultAmt.Enabled = true;
        //    else
        //        nmrConsultAmt.Enabled = false;
        //}

        //private void txtreference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
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



        /*IF EMPTY(ThisForm.REG_DATE1.Value) OR EMPTY(ThisForm.REFERENCE1.Value) OR EMPTY(ThisForm.REFERENCE1.Value)
	=MESSAGEBOX("Registration No./Date must be specified...",0,"ANC Registration")
	RETURN
ENDIF

if messagebox( "Confirm to Save Records...",4+32+0,"Ante-Natal Registration")==6
	this.enabled = .f.
	select ancreg
	*if empty(reference)
		replace grouphead WITH billchai.grouphead,;
		grouphtype WITH grouphtype,operator WITH woperator,dtime WITH DATETIME()
		if empty(ancreg.reference)
			select control
			replace attno with attno+1
			IF (mautogenreg .and. (Ancreg.reg_date = DATE() .or. DATE()-Ancreg.reg_date < 7))
				thisform.Genreg_fee()
				SELECT ancreg
			ENDIF
		ENDIF
		select ancreg
		replace reference with ThisForm.REFERENCE1.value
		tableupdate()
*	replace operator WITH woperator,dtime WITH DATETIME()
*	TABLEUPDATE()
	SELECT medhist
	if !seek( ancreg.groupcode+ancreg.patientno+dtos(Ancreg.reg_date) )
		append blank
		replace trans_date WITH Ancreg.reg_date,patientno WITH ancreg.patientno,;
		groupcode WITH ancreg.groupcode,name WITH ancreg.name
		replace comments with alltrim(comments)+chr(13)+;
		"==> Ante-Natal Registration - "+TAMPM(Ancreg.reg_time)+;
		" Date Regd : "+dtoc(Ancreg.reg_date)+IIF(!EMPTY(Ancreg.lmp)," : LMP : "+DTOC(Ancreg.lmp)+;
		" EDD : "+DTOC(Ancreg.edd),"")+chr(13)
			&&WE MUST UPDATE ANC DATABASES
			*USE ANC!ANC01 IN 0 SHARED TAG REFERENCE ALIAS ANC01
	endif
		*OPENFILE('ANC01.DAT',.F.)
		*SET ORDER TO TAG REFERENCE
	SELECT anc01
	IF !SEEK(ANCREG.REFERENCE)
		SELECT ANCREG
		SCATTER TO TEMPA_
		SELECT ANC01
		APPEND BLANK
		GATHER FROM TEMPA_
		replace reg_date WITH ancreg.reg_date,reg_time WITH ancreg.reg_time
		TABLEUPDATE()
	ENDIF
	SELECT ANCREG
	
*	This.Parent.cmdadd.enabled = .t.
*	This.Parent.cmdadd.setfocus
	thisform.varstore("B",.f.)
	ThisForm.REFERENCE1.setfocus
endif
*/
    }
}