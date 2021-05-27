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
    public partial class frmInvProcRequest
    {
        billchaindtl bchain = new billchaindtl();
        Customer customers = new Customer();
        patientinfo patients = new patientinfo();
        Mrattend mrattend = new Mrattend();
        // PleaseWaitForm pleaseWait = new PleaseWaitForm();
        Billings billv = new Billings();
        billchaindtl BillOnAcct = new billchaindtl();
        DataTable CustClass = custclass.GetCUSTCLASS();
        DataTable suspense; //= new SUSPENSE();

        string paediatriccode, mmisc_patient_code, allow_docsto_sc, billcode, procedure;
        string AnyCode, mbill_cir, mpatcateg, lookupsource, mgrouphtype, Notes, calltype, mreference, mgroupcode, mpatientno, mgrouphead, mghgroupcode, rtnstring, admspacedtl, savedgroup = "", mdoctor, msection, savedfacility, woperator;
        bool cashpaying, mbillatrequest, foundit, newrec, hmoprice_found, fee_for_service, iscapitated, groupeditem, isposted, inpatient, in_patient_alertsend, isbillonaccount, mcanadd, mcandelete, mcanalter;
        decimal counter, countersave, amtsave, medhistupdateallowed, grpprocedure_amount, savedoldamt, mlastno;
        int recno;
        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
        DataTable dtrequestalert, dttempfacility, dttariff = Dataaccess.GetAnytable("", "MR", "select reference,name from tariff order by name", true), dtreferrers = Dataaccess.GetAnytable("", "MR", "select CUSTNO, name from customer where referrer = '1' order by name", true), dtfacility = Dataaccess.GetAnytable("", "CODES", "select type_code, name from servicecentrecodes order by name", true), dtDesc;
        bool chkinpatient;

        MR_DATA.MR_DATAvm vm;

        public frmInvProcRequest(MR_DATA.MR_DATAvm VM2, string woperato)
        {
            //InitializeComponent();

            //calltype = xcalltype; mreference = reference; mgroupcode = groupcode; mpatientno = xpatientno; mgrouphtype = grouphtype;
            //mgrouphead = grouphead; mghgroupcode = ghgroupcode; inpatient = xinpatient; rtnstring = xrtnstring; admspacedtl = xadmspacedtl;

            //dtregistered.Value = trans_date;
            //cboName.Text = patientname;

            inpatient = false;
            chkinpatient = false;
            vm = VM2;

            calltype = vm.REPORTS.Searchdesc;
            mreference = vm.REPORTS.txtreference;
            woperator = woperato;

            getcontrolsettings();
            //initcomboboxes();
            //initheader();
            initAlertDataTable();
        }

        private void getcontrolsettings()
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT seclink, glbatchno, automedbil, name, attendlink from mrcontrol order by recid", false);

            //msmrfunc.getcontrolsetup("MR");

            allow_docsto_sc = dt.Rows[2]["seclink"].ToString();
            medhistupdateallowed = (Decimal)dt.Rows[2]["glbatchno"];

            mbillatrequest = (bool)dt.Rows[3]["automedbil"];

            paediatriccode = dt.Rows[7]["name"].ToString().Substring(0, 5);

            mmisc_patient_code = dt.Rows[6]["name"].ToString();

            //local settings

            //if (inpatient)
            //{
            //    in_patient_alertsend = (bool)dt.Rows[7]["attendlink"];

            //    chkinpatient.Checked = true;
            //    chkinpatient.Enabled = false;
            //    this.Text = "In-Patient Investigation/Procedure Request Management";
            //}

            //if (bissclass.sysGlobals.ismeddiag)
            //{
            //    btnConvert.Visible = chkinpatient.Visible = false;
            //}

            //dtregistered.MinDate = dtmin_date;
        }


        //private void frmInvProcRequest_Load(object sender, EventArgs e)
        //{
        //	// getcontrolsettings();
        //	if (string.IsNullOrWhiteSpace(mreference))
        //	{
        //		DialogResult result = MessageBox.Show("This Request has NO Consultation/Request Reference... Cannot be Tracked!!!", "NO CONSULTATION REFERENCE");
        //		//btnclose.PerformClick();
        //		return;
        //	}

        //	//getcontrolsettings();
        //	//initcomboboxes();
        //	//initheader();
        //	//initAlertDataTable();

        //	//Session["opdstring"] = "";
        //	//dtregistered.Value = DateTime.Now;
        //}


        //private void chkgetdependants_Click(object sender, EventArgs e)
        //{
        //    if (chkgetdependants.Checked && !string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        chkgetdependants.Checked = false;
        //        getdependants GetDependants = new getdependants("REGISTERED DEPENDANTS OF :" + bchain.NAME + " [" + bchain.GROUPCODE.Trim() + ":" + bchain.PATIENTNO.Trim() + "]", txtpatientno.Text, bchain.GROUPHEAD);
        //        GetDependants.Closed += new EventHandler(GetDependants_Closed);
        //        GetDependants.ShowDialog();
        //    }
        //}


        //void GetDependants_Closed(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //    this.txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
        //    this.txtpatientno.Focus();
        //}


        //private void initcomboboxes()
        //{
        //    this.Text = (calltype == "S") ? this.Text : "SERVICES AT DESIGNATED CENTRES - Select Facility/Service items";
        //    //get clinic
        //    this.cboFacility.DataSource = dtfacility; // Dataaccess.GetAnytable("", "CODES", "select type_code, name from servicecentrecodes order by name", true);
        //    cboFacility.ValueMember = "Type_code";
        //    cboFacility.DisplayMember = "name";
        //    //group procedure
        //    this.cboGrpprocess.DataSource = Dataaccess.GetAnytable("", "MR", "select reference, name from grpprocedure order by name", true);
        //    cboGrpprocess.ValueMember = "Reference";
        //    cboGrpprocess.DisplayMember = "Name";
        //    //referring Docs
        //    this.cboReferrer.DataSource = dtreferrers;
        //    cboReferrer.ValueMember = "Custno";
        //    cboReferrer.DisplayMember = "Name";
        //}


    //    void initheader()
    //    {
    //        if (calltype != "S")
    //        {
    //            bchain = billchaindtl.Getbillchain(mpatientno, mgroupcode);
    //            this.panel2.Enabled = true;
    //            chkinpatient.Visible = true;
    //            //ThisForm.Check1.Enabled = .t. - Query Procedure/Test Definitions for this Patient
    //            this.txtgroupcode.Text = bchain.GROUPCODE;
    //            txtpatientno.Text = bchain.PATIENTNO;
    //            cboName.Text = bchain.NAME;

    //            if (bchain.GROUPHTYPE == "P")
    //            {
    //                patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);
    //                if (patients == null)
    //                {
    //                    DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS");
    //                    btnclose.PerformClick();
    //                    return;
    //                }
    //                cboBillspayable.Text = bchain.PATIENTNO == bchain.GROUPHEAD ? "SELF" : "P\\Another Patient";

    //                // SELF P\Another Patient Corporate Client
    //            }
    //            else
    //            {
    //                customers = Customer.GetCustomer(bchain.GROUPHEAD);
    //                if (customers == null)
    //                {
    //                    DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS");
    //                    btnclose.PerformClick();
    //                    return;
    //                }
    //                cboBillspayable.Text = "Corporate Client";
    //            }
    //            mbill_cir = (bchain.GROUPHTYPE == "C") ? customers.Bill_cir : patients.bill_cir;
    //            txtgroupheadname.Text = (bchain.GROUPHTYPE == "P" && bchain.GROUPHEAD == patients.patientno) ?
    //                "< SELF >" : (bchain.GROUPHTYPE == "C") ? customers.Name : patients.name;
    //            cashpaying = (mbill_cir == "C") ? true : false;
    //            mpatcateg = bchain.PATCATEG;
    //            txtgrouphead.Text = bchain.GROUPHEAD;
    //            txtghgroupcode.Text = bchain.GHGROUPCODE;
    //            /*
    //IF !thisform.dispcurrency()
    //    RETURN .f.
    //ENDIF */
    //            cboBillspayable.SelectedValue = (bchain.GROUPHTYPE == "P" && bchain.GROUPHEAD == patients.patientno) ?
    //                "SELF" : (bchain.GROUPHTYPE == "C") ? "CORPORATE" : "P'\'Another Patient";
    //            panel1.Enabled = txtpatientno.Enabled = txtgroupcode.Enabled = cboBillspayable.Enabled = btngroupcode.Enabled = chkgetdependants.Enabled = false;
    //        }
    //        else
    //        {
    //            if (string.IsNullOrWhiteSpace(mmisc_patient_code))
    //            {
    //                DialogResult result = MessageBox.Show("Misc. Patient Account Code Not Defined in Systems Setup... PLS SEE YOUR SYSTEMS ADMINISTRATOR !!!", "Systems Setup Error...");
    //                btnclose.PerformClick();
    //                return;
    //            }
    //            //  combolist("N"); //loads name from suspense
    //            //combolist("A"); //loads address from suspense.  It should be disabled if it slowed down system 28-11-2013
    //            this.panel2.Enabled = true;
    //        }
    //        //check for previous requests
    //        DisplayPrevDefinitions();
    //    }


        //void DisplayPrevDefinitions()
        //{
        //    decimal xduration = 0m;
        //    string xDuration;
        //    counter = 0m;
        //    string[] arr = new string[17];
        //    ListViewItem itm;
        //    nmrCurrentTotal.Value = 0m;
        //    DataRow row = null;
        //    suspense = SUSPENSE.GetSUSPENSE(mreference, "A");
        //    if (suspense != null)
        //    {
        //        listView1.Items.Clear();
        //        //foreach (DataTable row in suspense.Rows)
        //        //xt = mrdt.Rows[0]["rptext"].ToString();
        //        string xtranstype;
        //        for (int i = 0; i < suspense.Rows.Count; i++)
        //        {
        //            counter++;
        //            row = suspense.Rows[i];
        //            nmrCurrentTotal.Value += Convert.ToDecimal(row["amount"]);
        //            xduration = Convert.ToDecimal(row["duration"]);
        //            xDuration = (xduration >= 1m) ? " x " + xduration.ToString() : "";

        //            arr[0] = row["itemno"].ToString();
        //            arr[1] = row["facility"].ToString();
        //            arr[2] = row["DESCRIPTION"].ToString() + xDuration;
        //            arr[3] = Convert.ToDecimal(row["amount"]).ToString("N2");
        //            arr[4] = row["process"].ToString();
        //            arr[5] = row["billprocess"].ToString();
        //            arr[6] = row["notes"].ToString().Trim();
        //            arr[7] = row["trans_date"].ToString();
        //            arr[8] = (Convert.ToBoolean(row["CAPITATED"])) ? "YES" : "NO";
        //            arr[9] = (Convert.ToBoolean(row["groupeditem"])) ? "YES" : "NO";
        //            arr[10] = row["duration"].ToString();
        //            arr[11] = (Convert.ToBoolean(row["posted"])) ? "YES" : "NO"; //done or note
        //            arr[12] = "NO"; // indicates for new request or not... needed during delete
        //            arr[13] = row["facility"].ToString();
        //            arr[14] = (Convert.ToBoolean(row["grpbillbyservtype"])) ? "YES" : "NO";
        //            arr[16] = row["recid"].ToString();
        //            if (i == 0)
        //            {
        //                xtranstype = row["transtype"].ToString();
        //                txtgroupcode.Text = row["groupcode"].ToString();
        //                txtpatientno.Text = row["patientno"].ToString();
        //                cboAge.Text = row["age"].ToString();
        //                cboSex.Text = row["sex"].ToString();
        //                cboName.Text = row["name"].ToString();
        //                txtAddress.Text = row["address1"].ToString();
        //                txtEmail.Text = row["email"].ToString();
        //                txtPhone.Text = row["phone"].ToString();
        //                txtghgroupcode.Text = row["ghgroupcode"].ToString();
        //                txtgrouphead.Text = row["grouphead"].ToString();
        //                bissclass.displaycombo(cboReferrer, dtreferrers, row["DOCTOR"].ToString(), "name");
        //                //  dtregistered.Value = (DateTime)row["trans_date"];
        //                cboBillspayable.Text = (xtranstype == "P" && txtgrouphead.Text.Trim() == mmisc_patient_code.Trim()) ? "SELF" : (xtranstype == "P" && txtgrouphead.Text.Trim() != mmisc_patient_code.Trim()) ? "ANOTHER PATIENT" : "CORPORATE CLIENT";
        //                Notes = arr[6];
        //            }
        //            itm = new ListViewItem(arr);
        //            listView1.Items.Add(itm);
        //        }
        //    }
        //}

        //void combolist(string xrequired)
        //{
        //    string selectstring = "";
        //    if (xrequired == "N")
        //        selectstring = "SELECT DISTINCT name FROM suspense ORDER BY NAME";
        //    else if (xrequired == "D") //&& !string.IsNullOrWhiteSpace(combclinic.Text) )
        //        selectstring = "SELECT NAME, REFERENCE FROM tariff WHERE rtrim(category) = '" + lblfacility.Text.Trim() + "' ORDER BY NAME";
        //    else
        //    {
        //        selectstring = "SELECT DISTINCT address1 FROM suspense ORDER BY ADDRESS1";
        //    }
        //    dtDesc = Dataaccess.GetAnytable("", "MR", selectstring, true);

        //    // DataRow row = dt.NewRow();
        //    // dt.Rows.InsertAt(row, 0);
        //    switch (xrequired)
        //    {
        //        case "N":
        //            cboName.DataSource = dtDesc;
        //            cboName.DisplayMember = "Name";
        //            break;
        //        case "D":
        //            cboDesc.DataSource = dtDesc;
        //            cboDesc.ValueMember = "reference";
        //            cboDesc.DisplayMember = "name";
        //            break;
        //            //case "A":
        //            //    cboAddress.DataSource = dt;
        //            //    cboAddress.DisplayMember = "address1";
        //            //    break;
        //    }
        //    return;
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
        //    else if (btn.Name == "btnpatientlookup")
        //    {
        //        this.txtpatientno.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnghgroupcode")
        //    {
        //        this.txtghgroupcode.Text = "";
        //        lookupsource = "p";
        //        msmrfunc.mrGlobals.crequired = "p";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED GROUPHEAD";
        //    }
        //    else if (btn.Name == "btngrouphead")
        //    {
        //        this.btngrouphead.Text = "";
        //        lookupsource = "GH";
        //        msmrfunc.mrGlobals.crequired = mgrouphtype == "P" ? "P" : "C";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED GROUPHEAD";
        //    }
        //    else if (btn.Name == "btndesc")
        //    {
        //        this.cboDesc.Text = procedure = "";
        //        lookupsource = "SD";
        //        msmrfunc.mrGlobals.crequired = "SD"; //SERVICE DESCRIPTIONS
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR [ALL] DEFINED INVESTIGATIONS/PROCEDURES";
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
        //        this.txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
        //        this.txtgroupcode.Focus();
        //    }

        //    else if (lookupsource == "L") //patientno
        //    {
        //        this.txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Focus();
        //    }
        //    else if (lookupsource == "p") //grouphead groupcode
        //    {
        //        this.txtghgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtgrouphead.Text = msmrfunc.mrGlobals.anycode1;
        //        this.txtghgroupcode.Focus();
        //    }
        //    else if (lookupsource == "GH") //grouphead 
        //    {
        //        this.txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtgrouphead.Focus();
        //    }
        //    else if (lookupsource == "SD") //service desc
        //    {
        //        // cboDesc.DropDownStyle = Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown;
        //        this.cboDesc.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        lblprocedure.Text = procedure = msmrfunc.mrGlobals.anycode1;
        //        // cboDesc.SelectedValue = procedure;
        //        // cboDesc.DropDownStyle = Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList;
        //        bissclass.displaycombo(cboFacility, dtfacility, msmrfunc.mrGlobals.anycode2, "name");
        //        lblfacility.Text = msmrfunc.mrGlobals.anycode2;
        //        msmrfunc.mrGlobals.anycode2 = "";

        //        if (string.IsNullOrWhiteSpace(savedfacility) || savedfacility != lblfacility.Text)
        //        {
        //            processFacilitySetting();
        //            combolist("D");
        //        }
        //        savedfacility = lblfacility.Text;
        //        bissclass.displaycombo(cboDesc, dtDesc, procedure, "name");
        //        this.cboDesc.Focus();
        //    }
        //}

        //private void txtpatientno_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        AnyCode = "";
        //        cboSex.Focus();
        //        return;
        //    }
        //    else
        //    {

        //        if (string.IsNullOrWhiteSpace(AnyCode))  //no lookup value obtained
        //        {
        //            this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");
        //        }

        //        //check if patientno exists
        //        bchain = billchaindtl.Getbillchain(this.txtpatientno.Text, txtgroupcode.Text);
        //        if (bchain == null)
        //        {
        //            DialogResult result = MessageBox.Show("Invalid Patient Number... ");
        //            txtpatientno.Text = txtgroupcode.Text = " ";
        //            txtgroupcode.Focus();
        //            return;
        //        }
        //        else
        //        {
        //            mgrouphtype = bchain.GROUPHTYPE;
        //            this.DisplayDetails();

        //        }
        //        cboBillspayable.Focus();
        //        return;
        //    }
        //}

        //private void DisplayDetails()
        //{
        //    if (mgrouphtype == "P")
        //    {
        //        patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE); // this.txtpatientno.Text,txtgroupcode.Text );
        //        if (patients == null)
        //        {
        //            DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS");
        //            txtgroupcode.Text = txtpatientno.Text = "";
        //            this.txtgroupcode.Select();
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        customers = Customer.GetCustomer(bchain.GROUPHEAD);
        //        if (customers == null)
        //        {
        //            DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS");
        //            txtgroupcode.Text = txtpatientno.Text = "";
        //            this.txtgroupcode.Select();
        //            return;
        //        }
        //        this.txtgroupcode.Text = bchain.GROUPCODE;
        //        this.cboName.Text = bchain.NAME;
        //        mbill_cir = (mgrouphtype == "C") ? customers.Bill_cir : patients.bill_cir;
        //        this.txtgroupheadname.Text = (mgrouphtype == "P" && bchain.GROUPHEAD == patients.patientno) ?
        //            "< SELF >" : (mgrouphtype == "C") ? customers.Name : patients.name;
        //    }
        //    //if (mgrouphtype == "P" && bchain.PATIENTNO == patients.patientno)
        //    //{
        //    //    txtAddress.Text = patients.address1.Trim();

        //    //    //rtxtaddress.Text = rtxtaddress.Text + patients.address2.Trim() + "\n";
        //    //}
        //    //else
        //    cboName.Text = bchain.NAME;
        //    txtAddress.Text = bchain.RESIDENCE;
        //    txtPhone.Text = bchain.PHONE;
        //    txtEmail.Text = bchain.EMAIL;


        //    if (bchain.STATUS == "C")
        //    {
        //        DialogResult result = MessageBox.Show("PATIENT NOT VALID FOR TRANSACTION - < Cancelled >");
        //        txtgroupcode.Text = txtpatientno.Text = "";
        //        this.txtgroupcode.Select();
        //        return;
        //    }
        //    if (bchain.STATUS == "S")
        //    {
        //        DialogResult result = MessageBox.Show("PATIENT NOT VALID FOR TRANSACTION - < Suspended > " +
        //         " CONFIRM TO CONTINUE...", "PATIENT STATUS", MessageBoxButtons.YesNo,
        //                   MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.No)
        //        {
        //            txtgroupcode.Text = txtpatientno.Text = "";
        //            this.txtgroupcode.Select();
        //            return;

        //        }
        //    }
        //    if (msection == "1" && mgrouphtype == "C" && customers.HMO && bchain.HMOSERVTYPE == "")
        //    {
        //        DialogResult result = MessageBox.Show("HMO Plan Type not specified in Patient Registration Details...Incomplete HMO Patient Registration");
        //        txtgroupcode.Text = txtpatientno.Text = "";
        //        this.txtgroupcode.Select();
        //        return;

        //    }

        //    cboAge.Text = bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now);
        //    cboSex.SelectedItem = bchain.SEX.Substring(0, 1) == "F" ? "FEMALE" : "MALE";
        //    chkgetdependants.Enabled = true;
        //}


        //private void cboBillspayable_LostFocus(object sender, EventArgs e)
        //{
        //    //txtghgroupcode.Text = txtgrouphead.Text = txtgroupheadname.Text = "";

        //    if (cboBillspayable.SelectedItem != null)
        //    {
        //        // this.txtghgroupcode.Enabled = true;
        //        // this.txtbillspayable.Enabled = true;

        //        Char xgrouphtype = this.cboBillspayable.SelectedItem.ToString()[0];
        //        mgrouphtype = xgrouphtype.ToString();
        //    }

        //    txtghgroupcode.Enabled = txtgrouphead.Enabled = false;

        //    if (mgrouphtype == "S")
        //    {
        //        mgrouphtype = "P";
        //        cashpaying = true;
        //        txtghgroupcode.Enabled = txtgrouphead.Enabled = true;
        //        if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //        {
        //            //wait window nowait "You must specify Miscellaneous Patients Account Code for this Transaction..."
        //            this.txtghgroupcode.Text = "PVT      ";
        //            txtgrouphead.Text = mmisc_patient_code;
        //            txtgrouphead_LostFocus(null, null);
        //            //txtghgroupcode.Focus();
        //            return;
        //        }
        //        else if (bchain.GROUPHEAD == txtpatientno.Text)
        //        {
        //            this.txtghgroupcode.Text = txtgroupcode.Text;
        //            txtgrouphead.Text = txtpatientno.Text;
        //            txtgroupheadname.Text = cboName.Text;
        //            cboReferrer.Focus();
        //            return;
        //        }
        //        else
        //        {
        //            this.txtghgroupcode.Text = "PVT      ";
        //            txtgrouphead.Text = mmisc_patient_code;
        //            txtghgroupcode.Focus();
        //            return;
        //        }
        //    }
        //    else if (mgrouphtype == "P")
        //    {
        //        txtghgroupcode.Enabled = txtgrouphead.Enabled = true;
        //        this.txtghgroupcode.Focus();
        //    }
        //    else if (mgrouphtype == "C" && !string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        txtghgroupcode.Text = ""; txtgrouphead.Text = bchain.GROUPHEAD;
        //        txtgrouphead.Enabled = true;
        //        txtgrouphead_LostFocus(null, null);
        //    }
        //    else
        //    {
        //        txtghgroupcode.Text = "";
        //        txtgrouphead.Enabled = true;
        //        txtgrouphead.Focus();
        //    }

        //}

        //private void txtgrouphead_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtgrouphead.Text) || mgrouphtype == "P" && string.IsNullOrWhiteSpace(txtghgroupcode.Text))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        Customer customer = new Customer();
        //        patientinfo patgrphead = new patientinfo();
        //        if (mgrouphtype == "P")
        //            patgrphead = patientinfo.GetPatient(txtgrouphead.Text, txtghgroupcode.Text);
        //        else
        //            customer = Customer.GetCustomer(txtgrouphead.Text);

        //        dtregistered.Value = DateTime.Now;
        //        if (mgrouphtype == "P" && patgrphead == null || mgrouphtype == "C" && customer == null)
        //        {
        //            DialogResult result = MessageBox.Show("Invalid GroupHead Specification as responsible for Bills");
        //            txtgrouphead.Text = "";
        //            txtgrouphead.Select();
        //        }
        //        else
        //        {
        //            // this.DisplayPatients();
        //            if (mgrouphtype == "P")
        //            {
        //                cashpaying = (patgrphead.bill_cir == "C") ? true : false;
        //                mpatcateg = patgrphead.patcateg;
        //            }
        //            else
        //            {
        //                cashpaying = (customer.Bill_cir == "C") ? true : false;
        //                mpatcateg = customer.Patcateg;
        //            }
        //            txtgroupheadname.Text = (mgrouphtype == "P") ? patgrphead.name : customer.Name;
        //            if (mgrouphtype == "P" && !patgrphead.isgrouphead)
        //            {
        //                DialogResult result = MessageBox.Show("Specified Patient is not a registered GroupHead...");
        //                txtgrouphead.Text = "";
        //                txtgrouphead.Select();
        //            }

        //        }
        //    }
        //}

        //private void dtregistered_Leave(object sender, EventArgs e)
        //{
        //    if (dtregistered.Value.Date > DateTime.Now.Date)
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Date Specification...", "Investigation Request Date");
        //        dtregistered.Value = DateTime.Now.Date;
        //        dtregistered.Focus();
        //        return;
        //    }
        //    else if (!bissclass.checkperiod(dtregistered.Value.Date, msmrfunc.mrGlobals.mlastperiod, msmrfunc.mrGlobals.mpyear, dtmin_date))
        //    {
        //        dtregistered.Value = DateTime.Now;
        //        dtregistered.Focus();
        //        return;
        //    }
        //    else
        //    {
        //        cboFacility.Focus();
        //        return;
        //    }
        //}

        //  private void cboFacility_LostFocus(object sender, EventArgs e)
        //  {

        //      if (cboFacility.SelectedValue == null)
        //          return;
        //      //16.03.2018
        //      /*   lblfacility.Text = cboFacility.SelectedValue.ToString();
        //if (!msmrfunc.setupinfo(cboFacility.SelectedValue.ToString(), ref xt, ref xreqrpt, ref billcode, ref emailphone, ref invrpt, ref invbatch, ref hjustify, ref autorptheader))
        //{
        // cboFacility.Focus();
        // return;
        //}*/
        //      if (processFacilitySetting())
        //      {
        //          combolist("D"); //gets list of defined procedures for this facility
        //          cboDesc.Focus();
        //      }
        //  }

        //bool processFacilitySetting()
        //{
        //    string xt = "", xreqrpt = "", emailphone = "", invrpt = "", hjustify = "";
        //    bool invbatch = false, autorptheader = false;
        //    lblfacility.Text = cboFacility.SelectedValue.ToString();
        //    if (!msmrfunc.setupinfo(cboFacility.SelectedValue.ToString(), ref xt, ref xreqrpt, ref billcode, ref emailphone, ref invrpt, ref invbatch, ref hjustify, ref autorptheader))
        //    {
        //        cboDesc.Text = ""; nmrAmount.Value = 0;
        //        cboFacility.Focus();
        //        return false;
        //    }
        //    return true;
        //}

        //private void combodesc_Enter(object sender, EventArgs e)
        //{
        //    //procedure = "";
        //}


        //    private void cboDesc_LostFocus(object sender, EventArgs e)
        //    {
        //        if (string.IsNullOrWhiteSpace(cboFacility.SelectedValue.ToString()))
        //            return;

        //        string xdesc = "", xfacility = Notes = "";
        //        // combodesc.SelectedValue = "";

        //        if (string.IsNullOrWhiteSpace(cboDesc.Text))
        //            return;

        //        procedure = cboDesc.SelectedValue.ToString();
        //        //procedure = (string.IsNullOrWhiteSpace(procedure) && !string.IsNullOrWhiteSpace(cboDesc.Text)) ? cboDesc.SelectedValue.ToString() : procedure;

        //        lblprocedure.Text = procedure;
        //        DialogResult result;
        //        if (string.IsNullOrWhiteSpace(procedure))
        //        {
        //            result = MessageBox.Show("Valid Service Description must be selected from available list...", "Investigation Request Item");
        //            cboDesc.Text = "";
        //            cboDesc.Select();
        //            return;
        //        }
        //        else
        //        {
        //            nmrAmount.Value = msmrfunc.getFeefromtariff(procedure, mpatcateg, ref xdesc, ref xfacility);
        //            amtsave = nmrAmount.Value;
        //        }

        //        foundit = isposted = false;
        //        newrec = btnAdd.Enabled = true;
        //        //we must scan through listview to check if stock had been selected - we edit
        //        if (listView1.Items.Count > 0)
        //        {
        //            for (int i = 0; i < listView1.Items.Count; i++)
        //            {
        //                if (listView1.Items[i].SubItems[4].Text.Trim() == procedure.Trim())
        //                {
        //                    string xd;
        //                    foundit = true;
        //                    recno = i;
        //                    nmrduration.Value = Convert.ToDecimal((xd = listView1.Items[recno].SubItems[10].ToString()));
        //                    nmrAmount.Value = Convert.ToDecimal(listView1.Items[recno].SubItems[3].ToString());
        //                    Notes = listView1.Items[recno].SubItems[6].ToString();
        //                    isposted = listView1.Items[recno].SubItems[11].ToString().Trim() == "YES" ? true : false;

        //                    break;
        //                }
        //            }
        //            if (foundit)
        //            {
        //                ServiceDuplicateOptions serviceuplicate = new ServiceDuplicateOptions();
        //                serviceuplicate.Closed += new EventHandler(serviceuplicate_Closed);
        //                serviceuplicate.ShowDialog();
        //            }
        //        }
        //        //check for hmo and special discount
        //        hmoprice_found = false;
        //        bool restrictive, inclusive, preauthorization;
        //        if (calltype != "S" && bchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(bchain.HMOSERVTYPE))
        //        {
        //            hmoserv_check(bchain.GROUPHEAD, bchain.HMOSERVTYPE);
        //        }
        //        if (!hmoprice_found && !string.IsNullOrWhiteSpace(txtpatientno.Text) && string.IsNullOrWhiteSpace(bchain.HMOSERVTYPE) && !string.IsNullOrWhiteSpace(mpatcateg))
        //        {
        //            foreach (DataRow row in CustClass.Rows)
        //            {
        //                if (row["reference"].ToString() == bchain.PATCATEG && Convert.ToBoolean(row["DEFINEPROC"]) == true)
        //                {
        //                    //check drgdefine on current item
        //                    inclusive = Convert.ToBoolean(row["PROCINCLUSIVE"]);
        //                    restrictive = Convert.ToBoolean(row["PROCRESTRICTIVE"]);
        //                    preauthorization = false;

        //                    PROCPROFILE procprofile = PROCPROFILE.GetPROCPROFILE(bchain.PATCATEG, procedure);
        //                    if (procprofile != null)
        //                    {
        //                        foundit = true;
        //                        nmrAmount.Value = procprofile.AMOUNT;
        //                        iscapitated = (procprofile.CAPITATED) ? true : false;
        //                        preauthorization = procprofile.AUTHORIZATIONREQUIRED;
        //                        amtsave = nmrAmount.Value;
        //                    }
        //                    procdefine_chk(restrictive, inclusive, preauthorization, false);
        //                    break;
        //                }
        //            }
        //        }

        //        if (!hmoprice_found && !string.IsNullOrWhiteSpace(txtpatientno.Text))
        //        {
        //            //check for discount percentage in patient or customer and apply
        //            if (msmrfunc.mrGlobals.percentageDiscountToApply != 0m)
        //            {
        //                // msmrfunc.mrGlobals.waitwindowtext = "Discounted :" + msmrfunc.mrGlobals.percentageDiscountToApply.ToString() +"% on " + nmrAmount.Value;
        //                //  pleaseWait.Show();
        //                decimal xdisc = (nmrAmount.Value * msmrfunc.mrGlobals.percentageDiscountToApply) / 100;
        //                nmrAmount.Value = nmrAmount.Value - xdisc;
        //                amtsave = nmrAmount.Value;
        //            }
        //        }
        //        /*	IF m.fxtype = 2 &&direct foreign currency bills - PENDING 30-11-2013
        //	thisform.convertcurrency()
        //ENDIF */
        //        chknotes.Enabled = true;
        //        nmrAmount.Focus();
        //    }

        //   void serviceuplicate_Closed(object sender, EventArgs e)
        //   {
        //       /*	 
        //            1 - ADD
        //2 - Amend
        //3 - Delete
        //4 - Ignor 
        //       */

        //       int rtnval = msmrfunc.mrGlobals.returnvalue;
        //       if (rtnval < 1 || rtnval > 3)
        //       {
        //           nmrAmount.Value = 0m;
        //           cboDesc.Text = procedure = "";
        //           cboDesc.Focus();
        //           return;
        //       }
        //       else if (rtnval == 1) //ADD 
        //       {
        //           newrec = true;
        //           nmrAmount.Focus();
        //           return;
        //       }
        //       else if (rtnval == 2) //AMend
        //       {
        //           newrec = false;
        //           string co = listView1.Items[recno].SubItems[0].ToString();
        //           countersave = Convert.ToDecimal(co); //listView1.Items[recno].SubItems[0]);
        //           savedoldamt = nmrAmount.Value;
        //           if (isposted || !mcanalter)
        //           {
        //               string xstr = isposted ? "This Process has been done... Can't be amended!" : "Access To Amend existing request Denied... Administrative Restriction!";
        //               DialogResult result = MessageBox.Show(xstr, "Investivation/Procedure Request Management");
        //               cboDesc.Text = Notes = "";
        //               nmrAmount.Value = 0m;
        //               chknotes.Checked = false;
        //               nmrduration.Value = 0;
        //               cboDesc.Focus();
        //               return;
        //           }
        //           // txtcurrenttotal.Text = (Convert.ToDecimal(txtcurrenttotal.Text) - Convert.ToDecimal(txtamount.Text)).ToString("9,999,999.99");

        //       }
        //       else if (rtnval == 3) //Delete
        //       {
        //           if (listView1.Items[recno].SubItems[11].Text == "YES" || !mcandelete) //posted
        //           {
        //               string xstr = isposted ? "This Process has been done... Can't be Deleted!" : "Access To Delete existing request Denied... Administrative Restriction!";
        //               DialogResult result = MessageBox.Show(xstr, "Investivation/Procedure Request Management");
        //               cboDesc.Text = Notes = "";
        //               chknotes.Checked = false;
        //               nmrduration.Value = nmrAmount.Value = 0;
        //               cboDesc.Focus();
        //               return;
        //           }
        //           DialogResult result1 = MessageBox.Show("Pls Confirm to Delete this item..." + cboDesc.Text.Trim(), "INVESTIGATIONS/PROCEDURE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //           if (result1 == DialogResult.Yes)
        //           {
        //               nmrCurrentTotal.Value -= nmrAmount.Value;

        //               if (listView1.Items[recno].SubItems[12].ToString().Trim() == "NO") //not new request - saved to database before now
        //               {
        //                   string xitem;
        //                   string xprocess = (string.IsNullOrWhiteSpace(listView1.Items[recno].SubItems[5].ToString())) ?
        //                       listView1.Items[recno].SubItems[4].ToString() : listView1.Items[recno].SubItems[5].ToString();
        //                   decimal itemcount = Convert.ToDecimal(xitem = listView1.Items[recno].SubItems[0].ToString());
        //                   if (SUSPENSE.DeleteSuspense(Convert.ToInt32(listView1.Items[recno].SubItems[16].ToString())))
        //                   {
        //                       //DataTable dt = Billings.GetBILLING(msmrfunc.mrGlobals.mreference,xprocess);
        //                       Billings.updateBILLS(mreference, itemcount, xprocess, nmrCurrentTotal.Value, woperator, DateTime.Now.Date, dtregistered.Value.Date);
        //                   }
        //               }
        //               counter--;
        //               listView1.Items[recno].Remove();
        //               listView1.Show();
        //               renumberview();
        //               nmrAmount.Value = 0;
        //               cboDesc.Text = Notes = "";
        //               chknotes.Checked = false;
        //               nmrduration.Value = 0;
        //               cboDesc.Focus();
        //               return;
        //           }
        //       }

        //   }

        //void hmoserv_check(string xcustomer, string xplantype)
        //{
        //    bool restrictive, inclusive, preauthorization = false;
        //    //bool hmofound = false;
        //    foundit = true; fee_for_service = false;
        //    Hmodetail hmodetail = new Hmodetail();
        //    hmodetail = Hmodetail.GetHMODETAIL(xcustomer, xplantype);
        //    if (hmodetail == null)
        //    {
        //        foundit = false;
        //        return;
        //    }
        //    else
        //    {
        //        inclusive = hmodetail.PROCINCLUSIVE;
        //        restrictive = hmodetail.PROCRESTRICTIVE;

        //        if (hmodetail.CAPAMT == 0m) //all services are fee for service no capitation but the hmo could have its own tariff so we check
        //        {
        //            fee_for_service = true;
        //        }
        //        HMOSERVPROC hmoserv = new HMOSERVPROC();
        //        hmoserv = HMOSERVPROC.GetHMOSERVPROC(xcustomer, xplantype, procedure);
        //        if (hmoserv != null)
        //        {
        //            /* msmrfunc.mrGlobals.waitwindowtext = "FOUND HMO PROCEDURE PRICE LIST...";
        //             pleaseWait.Show();*/
        //            foundit = true;
        //            hmoprice_found = true;
        //            nmrAmount.Value = hmoserv.AMOUNT;
        //            iscapitated = (hmoserv.CAPITATED) ? true : false;
        //            preauthorization = hmoserv.AUTHORIZATIONREQUIRED;
        //            amtsave = nmrAmount.Value;
        //        }
        //        procdefine_chk(restrictive, inclusive, preauthorization, true);
        //    }
        //    return;
        //}

        //void procdefine_chk(bool restrictive, bool inclusive, bool preauthorization, bool xhmo)
        //{
        //    string xpattype = (xhmo) ? "HMO Plan Type" : "Billing Category";
        //    hmoprice_found = false;
        //    if (foundit && inclusive)
        //    {
        //        //do nothing
        //        hmoprice_found = true;
        //    }
        //    else if (restrictive) //whether found or not
        //    {
        //        DialogResult result = MessageBox.Show(cboDesc.Text.Trim() + " is not on approved list for this Patient's " + xpattype +
        //            "... - RESTRICTIVE !!! '\r\n' PLEASE SELECT ALTERNATIVE PROCEDURE...", "Procedure Approved List");
        //        cboDesc.Text = "";
        //        cboDesc.Focus();
        //        return;
        //    }
        //    else
        //    {
        //        DialogResult result = MessageBox.Show(cboDesc.Text.Trim() + " is not on approved list for this Patient's " + xpattype + "... Continue ?", "Procedure Approved List", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.No)
        //        {
        //            cboDesc.Text = "";
        //            cboDesc.Focus();
        //            return;
        //        }
        //    }
        //    if (preauthorization)
        //    {
        //        DialogResult result = MessageBox.Show("Selected Service Requires Pre-Authorization...CONTINUE ? ", "PRE-AUTHORIZATION ALERT!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.No)
        //        {
        //            cboDesc.Text = "";
        //            cboDesc.Focus();
        //            return;
        //        }
        //        //init new row for request alert and add description of request
        //        int xrow = dtrequestalert.Rows.Count < 1 ? 0 : dtrequestalert.Rows.Count + 1;
        //        DataRow dr = dtrequestalert.NewRow();

        //        dr["name"] = cboDesc.Text;
        //    }

        //}

        void initAlertDataTable()
        {
            dtrequestalert = new DataTable();
            dtrequestalert.Columns.Add(new DataColumn("name", typeof(string)));
        }

        //private void combogrpprocess_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(cboGrpprocess.Text))
        //        return;

        //    string xfacility;
        //    groupeditem = newrec = true;
        //    DialogResult result;
        //    if (!string.IsNullOrWhiteSpace(savedgroup) && cboGrpprocess.Text.Trim() != savedgroup)
        //    {
        //        result = MessageBox.Show("A group had been selected before... Two different service groups can't be selected together !", "GROUPED INVESTIGATIONS/PROCEDURES");
        //        cboFacility.Focus();
        //        return;
        //    }
        //    DataTable grpprocess = Dataaccess.GetAnytable("", "MR", "select name, amount,  grpbillbyservtype from GRPPROCEDURE where rtrim(reference) = '" + cboGrpprocess.SelectedValue.ToString().Trim() + "'", false);
        //    if (grpprocess.Rows.Count < 1)
        //    {
        //        result = MessageBox.Show("No Record for selected Group Definition...");
        //        cboGrpprocess.Text = "";
        //        cboFacility.Focus();
        //        return;
        //    }
        //    bool grpbillbyservtype = Convert.ToBoolean(grpprocess.Rows[0]["grpbillbyservtype"].ToString());
        //    grpprocedure_amount = Convert.ToDecimal(grpprocess.Rows[0]["amount"]);
        //    if (nmrCurrentTotal.Value > 0 && string.IsNullOrWhiteSpace(savedgroup) && !grpbillbyservtype)
        //    {
        //        result = MessageBox.Show("A grouped billed item(s) cannot be added to an existing selection !", "GROUPED INVESTIGATIONS/PROCEDURES");
        //        cboFacility.Focus();
        //        return;
        //    }
        //    DialogResult result1 = MessageBox.Show("Confirm to Load " + cboGrpprocess.Text.Trim() + "...", "GROUPED INVESTIGATIONS/PROCEDURES", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result1 == DialogResult.No)
        //    {
        //        cboGrpprocess.Text = "";
        //        cboFacility.Focus();
        //        //  cboGrpprocess.Focus();
        //        return;
        //    }
        //    //load and display details to listview
        //    //createfacilitygroup(); //to hold facility summary 
        //    int xitemcount = 0;
        //    bool xfoundit = false, invbatch = false, autorptheader = false;
        //    savedgroup = cboGrpprocess.Text.Trim();
        //    nmrCurrentTotal.Value += grpprocedure_amount;
        //    DataTable grpdetail = Dataaccess.GetAnytable("", "MR", "select facility, process, description, amount from mrb15a where rtrim(reference) = '" + cboGrpprocess.SelectedValue.ToString().Trim() + "' order by facility", false);
        //    //MRB15A.GetMRB15A(combogrpprocess.SelectedValue.ToString());
        //    string xt = "", xreqrpt = "", emailphone = "", invrpt = "", hjustify = xfacility = "";
        //    //there may need to create temprorary table to hold facility,billcode

        //    foreach (DataRow row in grpdetail.Rows)
        //    {
        //        if (row["facility"].ToString().Trim() != xfacility)
        //            msmrfunc.setupinfo(row["facility"].ToString().Trim(), ref xt, ref xreqrpt, ref billcode, ref emailphone, ref invrpt, ref invbatch, ref hjustify, ref autorptheader);

        //        xfacility = row["facility"].ToString().Trim();
        //        xfoundit = false;
        //        //CHECK IF ITEM HAD BEEN SELECTED
        //        for (int i = 0; i < listView1.Items.Count; i++)
        //        {
        //            if (listView1.Items[i].SubItems[4].ToString().Trim() == row["process"].ToString().Trim())
        //            {
        //                xfoundit = true;
        //                break;
        //            }
        //        }
        //        if (!xfoundit) //add to grid
        //        {
        //            if (grpprocedure_amount == 0)
        //            {
        //                nmrCurrentTotal.Value += Convert.ToDecimal(row["amount"]);
        //            }

        //            xitemcount++;
        //            string[] addrow = { xitemcount.ToString(), xfacility, row["DESCRIPTION"].ToString(), Convert.ToDecimal(row["amount"]).ToString("N2"), row["process"].ToString(), string.IsNullOrWhiteSpace(billcode) ? row["process"].ToString() : billcode, "", DateTime.Now.ToString(), "NO", "YES", "0", "NO", "YES", xfacility, (grpbillbyservtype) ? "YES" : "NO", "0" };

        //            /*       string[] row = { counter.ToString(), cboFacility.Text, cboDesc.Text, nmrAmount.Value.ToString("N2"), procedure, billcode, Notes, (newrec) ? DateTime.Now.ToString() : dtregistered.Value.ToString(), (iscapitated) ? "YES" : "NO", (groupeditem) ? "YES" : "NO", nmrduration.Value.ToString(), newrec || !isposted ? "NO" : "YES", (newrec) || recid == "" ? "YES" : "NO", lblfacility.Text, "NO", "", recid };*/

        //            var listViewItem = new ListViewItem(addrow);
        //            listView1.Items.Add(listViewItem);
        //            //  }
        //        }
        //    }
        //    btnsave.Enabled = true;
        //    btnsave.Focus();
        //}

        //private void chknotes_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(cboDesc.Text) || string.IsNullOrWhiteSpace(cboFacility.Text))
        //        return;
        //    //string xcaption,string xtype,ref string xcomments)
        //    msmrfunc.mrGlobals.rtnstringNotes = Notes;
        //    NOTES notes = new NOTES("Special Notes/Instructions for " + cboName.Text.Trim() + " On: " +
        //        cboDesc.Text.Trim() + " - " + cboFacility.Text.Trim(), "N", Notes, false);
        //    notes.Closed += new EventHandler(notes_Closed);
        //    notes.ShowDialog();
        //}

        //void notes_Closed(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    Notes = msmrfunc.mrGlobals.rtnstringNotes;
        //    if (string.IsNullOrWhiteSpace(Notes))
        //    {
        //        chknotes.Checked = false;
        //    }
        //    return;
        //}

        //private void nmrduration_Leave(object sender, EventArgs e)
        //{
        //    //if (nmrduration.Value >= 1)
        //    //    nmrAmount.Value += (amtsave * nmrduration.Value);
        //}

        //private void nmrAmount_LostFocus(object sender, EventArgs e)
        //{
        //    //if (nmrAmount.Value < 1)
        //    //{
        //    //    cboDesc.Focus();
        //    //    return;
        //    //}
        //    //if (nmrAmount.Value < amtsave)
        //    //{
        //    //    DialogResult result = MessageBox.Show("Specified value is less than control value... CONTINUE ?", "Service Control Value", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    //    if (result == DialogResult.No)
        //    //    {
        //    //        cboDesc.Focus();
        //    //        return;
        //    //    }
        //    //}
        //}

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    DialogResult result;
        //    if (nmrAmount.Value < 1 || string.IsNullOrWhiteSpace(cboDesc.Text))
        //    {
        //        string xstr = nmrAmount.Value < 1 ? "Negative Charge ? " : "Request Description is Empty...";
        //        result = MessageBox.Show(xstr);
        //        cboDesc.Select();
        //        return;
        //    }
        //    if (nmrAmount.Value < amtsave)
        //    {
        //        result = MessageBox.Show("Specified value is less than control value... ", "Service Control Value", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
        //        cboDesc.Focus();
        //        return;
        //    }

        //    //save item counter, if old record and restore after init
        //    if (nmrduration.Value > 0)
        //        nmrAmount.Value = nmrAmount.Value * nmrduration.Value;

        //    decimal newcounter = counter;
        //    counter = (newrec) ? ++counter : countersave;
        //    string recid = "";
        //    if (!newrec)
        //    {
        //        recid = string.IsNullOrWhiteSpace(listView1.Items[recno].SubItems[16].ToString()) ? "" : listView1.Items[recno].SubItems[16].ToString();
        //        listView1.Items[recno].Remove();
        //        //counter = newcounter DISPLAY AMOUNT WITH COMMA
        //        nmrCurrentTotal.Value -= savedoldamt;
        //    }
        //    nmrCurrentTotal.Value += nmrAmount.Value;

        //    string[] row = { counter.ToString(), cboFacility.Text, cboDesc.Text, nmrAmount.Value.ToString("N2"), procedure, billcode, Notes, (newrec) ? DateTime.Now.ToString() : dtregistered.Value.ToString(), (iscapitated) ? "YES" : "NO", (groupeditem) ? "YES" : "NO", nmrduration.Value.ToString(), newrec || !isposted ? "NO" : "YES", (newrec) || recid == "" ? "YES" : "NO", lblfacility.Text, "NO", "", recid };
        //    ListViewItem itm;
        //    itm = new ListViewItem(row);
        //    listView1.Items.Add(itm);
        //    if (!newrec)
        //    {
        //        renumberview();
        //    }
        //    //SAMUEL 08178143959,07037984916
        //    //       var listViewItem = new ListViewItem(row);
        //    //      listView1.Items.Add(listViewItem);
        //    nmrduration.Value = nmrAmount.Value = 0;
        //    cboDesc.Text = procedure = Notes = "";
        //    chknotes.Checked = false;
        //    btnAdd.Enabled = false;
        //    btnsave.Enabled = true;
        //    cboDesc.Focus();
        //    return;
        //}

        public MR_DATA.MR_DATAvm btnsave_Click(IEnumerable<MR_DATA.PPDRESSINGDTL> tableList)
        {
            //DialogResult result;
            //if (!mcanalter || !mcanadd)
            //{
            //    result = MessageBox.Show("Access Denied...", "Investigation/Procedure Management");
            //    return;
            //}

            //if (listView1.Items.Count == 0)
            //{
            //    result = MessageBox.Show("No Selection Made...", "Tagged Services");
            //    cboFacility.Focus();
            //    return;
            //}

            //if (!bissclass.IsPresent(txtgrouphead, "Who Pays the Bill", false) ||
            //    !bissclass.IsPresent(cboName, "Patient Name", false) ||
            //    !bissclass.IsPresent(txtgroupheadname, "Who Pays the Bill", false))
            //{
            //    return;
            //}

            //if (calltype == "S" && string.IsNullOrWhiteSpace(txtAddress.Text) && string.IsNullOrWhiteSpace(txtPhone.Text) && string.IsNullOrWhiteSpace(txtEmail.Text))
            //{
            //    result = MessageBox.Show("Check Patient's Contact information - Phone/Email/Address", "Incomplete Data");
            //    return;
            //}

            if (medhistupdateallowed > 0 && Convert.ToDecimal(DateTime.Now.Date.Subtract(vm.REPORTS.dtregistered).TotalDays) >= medhistupdateallowed)
            {
                vm.REPORTS.alertMessage = "Patients' Medical History Notes Update RESTRICTED... Pls Consult your Systems Administrator !!!";
                return vm;
            }

            //result = MessageBox.Show("Confirm to Submit Selections to Service Centres...", "Investigation/Procedure Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            //if (result == DialogResult.No)
            //    return;
            //btnsave.Enabled = false;

            savedetails(tableList);

            return vm;
        }

        #region
        void savedetails(IEnumerable<MR_DATA.PPDRESSINGDTL> tableList)
        {
            string[] servpointa_ = new string[10]; //to hold service centre codes/desc for ALERT/grp billing
            for (int ise = 0; ise < 10; ise++)
            {
                servpointa_[ise] = "";
            }

            // string[] servdesca_ = new string[10];
            string[,] billitemsa_ = new string[10, 7];
            string[,] inpatientupdatea_ = new string[10, 6];
            string xservicetype = "";

            //lblprompt.Text = "Saving/Adjusting Records - Please Wait !!!";

            //   msmrfunc.mrGlobals.waitwindowtext = "Saving/Adjusting Records - Please Wait !!!";
            //30-06-2013  there is need to prevent duplicate Special service number before updating

            if (calltype == "S" && vm.REPORTS.newrecString == "true") //update control counter
            {
                updatemrattend();
            }

            //   pleaseWait.Show();
            string xstring = (chkinpatient) ? "Inpatient" : "Out-Patient";

            if (mbillatrequest && calltype != "S" && !string.IsNullOrWhiteSpace(bchain.BILLONACCT))
            {
                BillOnAcct = billchaindtl.Getbillchain(bchain.BILLONACCT, bchain.GROUPCODE);
                isbillonaccount = (BillOnAcct != null) ? true : false;
            }

            string xfacility = "";
            int arr_count = 0;
            //int billcount = 0;
            // bool newbillitem = false;
            int xele = 0;
            //return string to medical history
            string xtype = (chkinpatient) ? "In-patient" : "OPD";
            rtnstring = "==> " + xtype + "  Inv/Proc Request Details - " +
            DateTime.Now.ToString("dd-MM-yyyy @ HH:mmtt ") + " - "; //AMPM
            rtnstring += (!string.IsNullOrWhiteSpace(mdoctor)) ? mdoctor : (chkinpatient) ? " REVIEWS " : msection == "3" ? " Nurses Desk" : msection == "1" ? " Front Desk" : "";
            rtnstring += "\r\n";

            //extract service centre for billing and alert profile for newrequest 

            foreach (var tableRow in tableList)
            {
                if (xfacility != "" && ASCAN(servpointa_, tableRow.REFERENCE, ref xele)) // servpointa_.Containsxfacility)  )
                    continue;

                if (tableRow.RECTYPE.Trim() == "YES") //newrequest
                {
                    xfacility = tableRow.REFERENCE;

                    servpointa_[arr_count] = xfacility;
                    billitemsa_[arr_count, 2] = tableRow.FACILITY; //process desc
                    billitemsa_[arr_count, 3] = tableRow.GHGROUPCODE; //billcode
                    billitemsa_[arr_count, 4] = tableRow.GROUPCODE; //PROCESS
                    arr_count++;
                }
            }

            //for (int i = 0; i < listView1.Items.Count; i++)
            //{
            //    if (xfacility != "" && ASCAN(servpointa_, listView1.Items[i].SubItems[13].ToString(), ref xele)) // servpointa_.Containsxfacility)  )
            //        continue;
            //    if (listView1.Items[i].SubItems[12].ToString().Trim() == "YES") //newrequest
            //    {
            //        xfacility = listView1.Items[i].SubItems[13].ToString();

            //        servpointa_[arr_count] = xfacility;
            //        billitemsa_[arr_count, 2] = listView1.Items[i].SubItems[1].ToString(); //process desc
            //        billitemsa_[arr_count, 3] = listView1.Items[i].SubItems[5].ToString(); //billcode
            //        billitemsa_[arr_count, 4] = listView1.Items[i].SubItems[4].ToString(); //PROCESS
            //        arr_count++;
            //    }
            //}

            //int xlen = comborequestalert.Text.Length;
            //check for alert profile and xtract service centre code from listview
            //   if ( xlen > 0 && comborequestalert.Text.Substring(0, 1) == "U" || bissclass.sysGlobals.ismeddiag || inpatient && in_patient_alertsend)
            //   {

            string xalert = (!string.IsNullOrWhiteSpace(vm.REPORTS.REPORT_TYPE3)) ? vm.REPORTS.REPORT_TYPE3.Trim() : "";
            rtnstring += xalert == "" ? "" : "*** REQUEST ALERT !!! ***" + xalert + "\r\n";
            //write to mrb21 - alert inbox to service centres
            string xnotes = "";

            for (int i = 0; i < servpointa_.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(servpointa_[i]))
                {
                    xnotes = (inpatient) ? "From > Wd/Rm : " + admspacedtl : string.IsNullOrWhiteSpace(vm.REPORTS.REPORT_TYPE3) ? "" : vm.REPORTS.REPORT_TYPE3.Trim();
                    xnotes += "\r\n";
                    xnotes += billitemsa_[i, 2];

                    MRB21.Writemrb21Details(vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, DateTime.Now, vm.REPORTS.TXTPATIENTNAME, servpointa_[i], woperator, xnotes + xalert, mreference, msection, "6", woperator, mdoctor, inpatient ? "I" : "O");
                }
            }

            //   }
            // string tmpamt;

            arr_count = 0;
            int inpCounter = 0;

            //   int xl = billitemsa_.Length / 7;
            for (int i = 0; i < 10; i++)
            {
                billitemsa_[i, 0] = billitemsa_[i, 1] = billitemsa_[i, 2] = billitemsa_[i, 3] = billitemsa_[i, 4] = billitemsa_[i, 5] = billitemsa_[i, 6] = "";
            }

            //for billing
            /*   for (int i = 0; i < listView1.Items.Count; i++)
               {
                   if (xfacility != "" && servpointa_.Contains(xfacility)  )
                       continue;

                   xfacility = listView1.Items[i].SubItems[13].ToString();

                   servpointa_[arr_count] = xfacility;
                   arr_count++;
               }*/

            bool foundit;
            //ListViewItem lv = new ListViewItem();

            foreach (var tableRow in tableList)
            {
                //lv = listView1.Items[i];
                //  tmpcol = 0;

                foundit = false;
                for (int ia = 0; ia < 10; ia++)
                {
                    //check with billcode
                    if (!string.IsNullOrWhiteSpace(tableRow.GHGROUPCODE) && tableRow.GHGROUPCODE.Trim() == billitemsa_[ia, 3].Trim())
                    {
                        foundit = true;
                        xele = ia;
                        break;
                    }
                    else if (string.IsNullOrWhiteSpace(tableRow.GHGROUPCODE) && tableRow.BILLPROCESS.Trim() == billitemsa_[ia, 2].Trim() || string.IsNullOrWhiteSpace(tableRow.GHGROUPCODE) && !string.IsNullOrWhiteSpace(tableRow.TRANSTYPE) && tableRow.TRANSTYPE.Trim() == billitemsa_[ia, 2].Trim())
                    {
                        foundit = true;
                        xele = ia;
                        break;
                    }
                }
                if (foundit)
                {
                    billitemsa_[xele, 5] = (Convert.ToDecimal(billitemsa_[xele, 5]) + Convert.ToDecimal(tableRow.AMOUNT)).ToString();
                    billitemsa_[xele, 6] = billitemsa_[xele, 6] + "\r\n" + "   ..." + tableRow.BILLPROCESS.Trim() + " (" + tableRow.AMOUNT + ")";
                }
                else
                {
                    xele = arr_count;
                    billitemsa_[xele, 1] = tableRow.REFERENCE; //facility code
                    billitemsa_[xele, 2] = !string.IsNullOrWhiteSpace(tableRow.GHGROUPCODE) ?
                        bissclass.combodisplayitemCodeName("reference", tableRow.GHGROUPCODE, dttariff, "name") : string.IsNullOrWhiteSpace(tableRow.TRANSTYPE) ? tableRow.BILLPROCESS : tableRow.TRANSTYPE; //description
                    billitemsa_[xele, 3] = tableRow.GHGROUPCODE; //tarbillitem
                    billitemsa_[xele, 4] = tableRow.GROUPCODE; //process
                    billitemsa_[xele, 5] = tableRow.AMOUNT.ToString(); //amount
                    billitemsa_[xele, 6] = "   ..." + tableRow.BILLPROCESS.Trim() + " (" + tableRow.AMOUNT + ")";
                }

                if (!foundit)
                    arr_count++;
                
            }

            //for (int i = 0; i < listView1.Items.Count; i++)  //GET DETAILS FOR BILLLING FILE
            //{
            //    lv = listView1.Items[i];
            //    //  tmpcol = 0;
            //    foundit = false;
            //    for (int ia = 0; ia < 10; ia++)
            //    {
            //        //check with billcode
            //        if (!string.IsNullOrWhiteSpace(lv.SubItems[5].ToString()) && lv.SubItems[5].ToString().Trim() == billitemsa_[ia, 3].Trim())
            //        {
            //            foundit = true;
            //            xele = ia;
            //            break;
            //        }
            //        else if (string.IsNullOrWhiteSpace(lv.SubItems[5].ToString()) && lv.SubItems[2].ToString().Trim() == billitemsa_[ia, 2].Trim() || string.IsNullOrWhiteSpace(lv.SubItems[5].ToString()) && !string.IsNullOrWhiteSpace(lv.SubItems[15].ToString()) && lv.SubItems[15].ToString().Trim() == billitemsa_[ia, 2].Trim())
            //        {
            //            foundit = true;
            //            xele = ia;
            //            break;
            //        }
            //    }
            //    if (foundit)
            //    {
            //        billitemsa_[xele, 5] = (Convert.ToDecimal(billitemsa_[xele, 5]) + Convert.ToDecimal(lv.SubItems[3].ToString())).ToString();
            //        billitemsa_[xele, 6] = billitemsa_[xele, 6] + "\r\n" + "   ..." + lv.SubItems[2].ToString().Trim() + " (" + lv.SubItems[3].ToString().Trim() + ")";
            //    }
            //    else
            //    {
            //        xele = arr_count;
            //        billitemsa_[xele, 1] = lv.SubItems[13].ToString(); //facility code
            //        billitemsa_[xele, 2] = !string.IsNullOrWhiteSpace(lv.SubItems[5].ToString()) ?
            //            bissclass.combodisplayitemCodeName("reference", lv.SubItems[5].ToString(), dttariff, "name") : string.IsNullOrWhiteSpace(lv.SubItems[15].ToString()) ? lv.SubItems[2].ToString() : lv.SubItems[15].ToString(); //description
            //        billitemsa_[xele, 3] = lv.SubItems[5].ToString(); //tarbillitem
            //        billitemsa_[xele, 4] = lv.SubItems[4].ToString(); //process
            //        billitemsa_[xele, 5] = lv.SubItems[3].ToString(); //amount
            //        billitemsa_[xele, 6] = "   ..." + lv.SubItems[2].ToString().Trim() + " (" + lv.SubItems[3].ToString().Trim() + ")";
            //    }
            //    if (!foundit)
            //        arr_count++;
            //    /*   if (!string.IsNullOrWhiteSpace(lv.SubItems[5].ToString()) &&
            //           bissclass.multiAscan(billitemsa_, lv.SubItems[5].ToString(), 3, "", 2, ref tmpcol)) //billcode
            //           xele = tmpcol; //arr_count;
            //       else if (string.IsNullOrWhiteSpace(lv.SubItems[5].ToString()) &&
            //           bissclass.multiAscan(billitemsa_, lv.SubItems[4].ToString(), 4, "", 2, ref tmpcol))
            //           xele = tmpcol; //process direct bill

            //       tmpamt = lv.SubItems[3].ToString();
            //       billitemsa_[xele, 5] = (string.IsNullOrWhiteSpace(billitemsa_[xele, 5])) ? tmpamt :
            //           (Convert.ToDecimal(billitemsa_[xele, 5]) + Convert.ToDecimal(tmpamt)).ToString(); 

            //       billitemsa_[arr_count, 1] = servpointa_[xele]; //facility code
            //       billitemsa_[arr_count, 2] = !string.IsNullOrWhiteSpace(lv.SubItems[5].ToString()) ? bissclass.combodisplayitemCodeName("reference",lv.SubItems[5].ToString(),dttariff,"name") :  string.IsNullOrWhiteSpace(lv.SubItems[15].ToString()) ? lv.SubItems[2].ToString() : lv.SubItems[15].ToString(); //description
            //       billitemsa_[arr_count, 3] = lv.SubItems[5].ToString(); //tarbillitem
            //       billitemsa_[arr_count, 4] = lv.SubItems[4].ToString(); //process
            //       //init variable for xtended description - re-init whether done b4 now or not
            //       if (!string.IsNullOrWhiteSpace(billitemsa_[arr_count, 3]) && !inpatient) // &&
            //              // lv.SubItems[11].ToString().Trim() != "YES")  //writes to xtended desc IF NOT POSTED
            //       {
            //           billitemsa_[arr_count, 6] = (string.IsNullOrWhiteSpace(billitemsa_[arr_count, 6])) ?
            //               lv.SubItems[2].ToString().Trim() + " (" + tmpamt + ")" : billitemsa_[arr_count, 6] + "\r\n" + lv.SubItems[2].ToString().Trim() + " (" + tmpamt + ")";
            //       }
            //       arr_count++; */
            //}

            //   try
            //   {

            SqlConnection connection = new SqlConnection();
            connection = Dataaccess.mrConnection();

            string xtranstype = (vm.REPORTS.txtbillspayable.Trim() == "S" || vm.REPORTS.txtbillspayable.Trim() == "P" ||
                vm.REPORTS.txtbillspayable.Trim() == "S" && !string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno)) ? "P" : "C";
            
            // string xdesc = "";
            xservicetype = (mreference.Substring(0, 1) != "S" && mgrouphtype == "C" &&
                !string.IsNullOrWhiteSpace(bchain.HMOSERVTYPE) || xtranstype == "C" && fee_for_service) ? "C" : "";

            connection.Open();

            foreach (var tableRow in tableList)
            {
                //lv = listView1.Items[i];

                if (tableRow.POSTED == true || string.IsNullOrWhiteSpace(tableRow.BILLPROCESS)) // test done before now/empty desc
                    continue;

                SqlCommand insertCommand = new SqlCommand();
                if (tableRow.RECTYPE.Trim() == "NO") //old rec NOT NEW REQUEST
                    insertCommand.CommandText = "SUSPENSE_update";
                else
                {
                    insertCommand.CommandText = "SUSPENSE_Add";
                    rtnstring += "\r\n" +
                    tableRow.DESCRIPTION.Trim() + ". " +
                    tableRow.BILLPROCESS + " (" + tableRow.AMOUNT.ToString().Trim() + ") -> " + tableRow.FACILITY.Trim();
                }

                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@Reference", mreference);
                insertCommand.Parameters.AddWithValue("@Name", vm.REPORTS.TXTPATIENTNAME);
                insertCommand.Parameters.AddWithValue("@itemno", Convert.ToDecimal(tableRow.DESCRIPTION));
                insertCommand.Parameters.AddWithValue("@DESCRIPTION", tableRow.BILLPROCESS);
                insertCommand.Parameters.AddWithValue("@rectype", "D");
                insertCommand.Parameters.AddWithValue("@process", tableRow.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@Groupcode", vm.REPORTS.txtgroupcode);
                insertCommand.Parameters.AddWithValue("@Patientno", vm.REPORTS.txtpatientno);
                insertCommand.Parameters.AddWithValue("@Grouphead", vm.REPORTS.txtgrouphead);
                insertCommand.Parameters.AddWithValue("@transtype", xtranstype);
                insertCommand.Parameters.AddWithValue("@Doctor", mdoctor);
                insertCommand.Parameters.AddWithValue("@facility", tableRow.REFERENCE);
                insertCommand.Parameters.AddWithValue("@posted", tableRow.POSTED);
                insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@trans_date", Convert.ToDateTime(tableRow.TRANS_DATE));
                insertCommand.Parameters.AddWithValue("@amount", Convert.ToDecimal(tableRow.AMOUNT));
                insertCommand.Parameters.AddWithValue("@ghgroupcode", vm.REPORTS.txtghgroupcode);
                insertCommand.Parameters.AddWithValue("@Title", (inpatient) ? "Inpatient" : "");
                insertCommand.Parameters.AddWithValue("@Address1", vm.REPORTS.txtaddress1);
                insertCommand.Parameters.AddWithValue("@currency", "");
                insertCommand.Parameters.AddWithValue("@exrate", 0m);
                insertCommand.Parameters.AddWithValue("@fcamount", 0M);
                insertCommand.Parameters.AddWithValue("@duration", Convert.ToDecimal(tableRow.DURATION));
                insertCommand.Parameters.AddWithValue("@billprocess", tableRow.GHGROUPCODE);
                insertCommand.Parameters.AddWithValue("@notes", tableRow.NOTES);
                insertCommand.Parameters.AddWithValue("@servicetype", xservicetype);
                insertCommand.Parameters.AddWithValue("@capitated", tableRow.CAPITATED);
                insertCommand.Parameters.AddWithValue("@groupeditem", tableRow.GROUPEDITEM);
                insertCommand.Parameters.AddWithValue("@grpbillbyservtype", tableRow.GRPBILLBYSERVTYPE);
                insertCommand.Parameters.AddWithValue("@Age", vm.REPORTS.cboAge);
                insertCommand.Parameters.AddWithValue("@sex", vm.REPORTS.cbogender);
                insertCommand.Parameters.AddWithValue("@PHONE", vm.REPORTS.txthomephone);
                insertCommand.Parameters.AddWithValue("@EMAIL", vm.REPORTS.txtemail);

                if (tableRow.RECTYPE == "NO") //old rec NOT NEW REQUEST
                    insertCommand.Parameters.AddWithValue("@recid", Convert.ToInt32(tableRow.RECID));

                insertCommand.ExecuteNonQuery();

                if (inpatient && mbillatrequest && tableRow.RECTYPE != "NO")
                {
                    //update array for inpatient service update
                    inpatientupdatea_[inpCounter, 0] = tableRow.REFERENCE; //facility 
                    inpatientupdatea_[inpCounter, 1] = tableRow.BILLPROCESS; //desc
                    inpatientupdatea_[inpCounter, 2] = tableRow.AMOUNT.ToString(); // amt
                    inpatientupdatea_[inpCounter, 3] = tableRow.GROUPCODE; //process
                    inpatientupdatea_[inpCounter, 4] = tableRow.GHGROUPCODE; //tarbillitem
                                                                                  // inpatientupdatea_[inpCounter, 5] = lv.SubItems[12].ToString().Trim(); //new or old
                    inpatientupdatea_[inpCounter, 5] = tableRow.FACILITY; //facilityname name
                    inpCounter++;
                }

                /*		IF mseclink AND !EMPTY(patientno) AND !EMPTY(billchai.currency) AND billchai.currency # mlocalcur
                        **convert value in suspense
                    replace fcamount WITH mfa_[x,8],exrate with curtable.rate,;   PENDING 2-12-2013
                    currency WITH billchai.currency
                    ENDIF
                        IF calltype = "S" - conversion notes: 2-12-2013
                        replace suspense.address1 WITH ThisForm.Container1.Container2.combsex.Value+ThisForm.Container1.Container2.txtage.Value
                    ENDIF
                */

            }

            connection.Close();

            /*           }
                       catch (SqlException ex)
                       {
                           MessageBox.Show(ex.Message, ex.GetType().ToString());
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(ex.Message, ex.GetType().ToString());
                       }*/

            if (inpatient)
            {
                vm.REPORTS.mcanalter = false; //for CashPaying

                //write to admission service update
                //bool xnewrecitem;

                int xarraylen = inpatientupdatea_.GetUpperBound(0);
                for (int i = 0; i < xarraylen; i++)
                {
                    if (!string.IsNullOrWhiteSpace(inpatientupdatea_[i, 1])) //DESCRIPTION
                    {
                        // xnewrecitem = (inpatientupdatea_[i, 5] == "YES") ? true : false;
                        ADMDETAI.writeAdmdetails(true, mreference, vm.REPORTS.dtregistered, vm.REPORTS.dtregistered.ToString("HH:ss"), 
                            inpatientupdatea_[i, 3], inpatientupdatea_[i, 4], "", inpatientupdatea_[i, 1], "", 0m, Convert.ToDecimal(inpatientupdatea_[i, 2]), 
                            false, dtmin_date, woperator, DateTime.Now, vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, mdoctor, inpatientupdatea_[i, 0], 0, "");
                    }
                }
            }
            else if (mbillatrequest) // && !inpatient)
            {
                int recid = 0;
                bool newbill = true;
                decimal xitem = 0, xfditem = 0; //,oldamt = 0m;
                DataTable refbill; // = Billings.GetBILLING(mreference);
                                   //newbill = (refbill != null) ? true : false;
                string xprocess;
                //   for (int i = 0; i < servpointa_.Length; i++)
                refbill = Dataaccess.GetAnytable("", "MR", "SELECT itemno, process, amount, recid from billing where reference = '" + mreference + "' order by itemno", false); // Billings.GetBILLING(mreference, xprocess);
                if (refbill.Rows.Count > 0)
                    xitem = (decimal)refbill.Rows.Count - 1;

                for (int i = 0; i < 10; i++)
                {
                    if (string.IsNullOrWhiteSpace(billitemsa_[i, 5])) //amount
                        continue;
                    newbill = true;
                    xprocess = (string.IsNullOrWhiteSpace(billitemsa_[i, 3])) ? billitemsa_[i, 4] : billitemsa_[i, 3]; //billcode / process
                                                                                                                       /*  refbill = Dataaccess.GetAnytable("", "MR", "SELECT itemno, process, amount, recid from billing where reference = '" + mreference + "' order by itemno", false); // Billings.GetBILLING(mreference, xprocess);*/
                    if (refbill.Rows.Count > 0)
                    {
                        xfditem = 0;
                        //check if xprocess is part of the old bill
                        foreach (DataRow row in refbill.Rows)
                        {
                            if (row["process"].ToString().Trim() == xprocess.Trim())
                            {
                                newbill = false;
                                xfditem = (decimal)row["itemno"];
                                recid = (Int32)row["recid"];
                                break;
                            }
                        }
                        if (xfditem < 1)
                            xitem++;
                    }
                    else
                        xitem++;

                    /*     if (!newbill || xitem > 0)
                             xitem = Billings.getBillNextItems(mreference, xprocess, true, ref oldamt, ref recid);
                         else
                             xitem++;*/

                    writeBILLS(newbill, mreference, !newbill ? xfditem : xitem, xprocess,
                        billitemsa_[i, 2], mgrouphtype, Convert.ToDecimal(billitemsa_[i, 5]), 
                        vm.REPORTS.dtregistered.Date, vm.REPORTS.TXTPATIENTNAME, vm.REPORTS.txtgrouphead, 
                        billitemsa_[i, 1], vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, "D", 
                        vm.REPORTS.txtghgroupcode, woperator, DateTime.Now, billitemsa_[i, 6], "", 
                        0m, 0, "", mdoctor, false, "", xservicetype, 0m, "", 0m, "O", false, 
                        newbill ? 0 : recid);
                }
                //alert to cash office if cash paying
            }

            //    pleaseWait.Hide();

            if (vm.REPORTS.mcanalter) //if cashpaying is true;
            {
                LINK.WriteLINK(0, vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, vm.REPORTS.TXTPATIENTNAME, "2", mreference, vm.REPORTS.nmrbalance, 0m, vm.REPORTS.REPORT_TYPE1, false, mdoctor, false, 0, "", msection, woperator);
            }
            else //send alert to service centres
            {
                //int xarraylen = servpointa_.GetUpperBound(0);
                //int xeln = servpointa_.Length;
                for (int i = 0; i < servpointa_.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(servpointa_[i]))
                        continue;

                    WriteLINK(0, vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, vm.REPORTS.TXTPATIENTNAME, (servpointa_[i] == paediatriccode) ? "9" : "6", mreference, vm.REPORTS.nmrbalance, 0m, servpointa_[i], true, mdoctor, false, 0, "", msection, woperator);
                    /*   we separate request for this group of patient at this point
						 whereas that of the cash paying patients will be done at pay point (cashier)*/
                }
            }

            if (calltype == "S" && !string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno)) //update medhist history file
            {
                MedHist medhist = MedHist.GetMEDHIST(vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, "", false, true, vm.REPORTS.dtregistered.Date, "DESC");
                bool newhist = (medhist == null) ? true : false;
                string xcomments = "";
                if (newhist)
                    xcomments = rtnstring;
                else
                {
                    xcomments = medhist.COMMENTS.Trim() + "\r\n" + rtnstring.Trim();
                }
                MedHist.updatemedhistcomments(vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, vm.REPORTS.dtregistered, xcomments, newhist, vm.REPORTS.REPORT_TYPE2, vm.REPORTS.TXTPATIENTNAME, vm.REPORTS.txtghgroupcode, vm.REPORTS.txtgrouphead, "");
            }
            //else
            //{
            //    Session["opdstring"] = rtnstring;
            //}

            //btnclose.PerformClick();
        }

        public bool WriteLINK(int recrec, string GroupCodeID, string PatientID, string patname, string xtosection, string xreference, decimal xcumbil, decimal xcumpay, string xclinic, bool xlinkok, string xdoctor, bool xclusive, int xprocfunc, string xflag, string xfrmsection, string woperator)
        {
            DateTime dtmin_date = DateTime.Now;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "LINK_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@groupcode", GroupCodeID);
            insertCommand.Parameters.AddWithValue("@patientno", PatientID);
            insertCommand.Parameters.AddWithValue("@name", patname);
            insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
            insertCommand.Parameters.AddWithValue("@posted", false);
            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date.Date);
            insertCommand.Parameters.AddWithValue("@frsection", xfrmsection);
            insertCommand.Parameters.AddWithValue("@timesent", DateTime.Now.ToLongTimeString());
            insertCommand.Parameters.AddWithValue("@tosection", xtosection);
            insertCommand.Parameters.AddWithValue("@daterec", "");
            insertCommand.Parameters.AddWithValue("@timerec", "");
            insertCommand.Parameters.AddWithValue("@reference", xreference);
            insertCommand.Parameters.AddWithValue("@cumbil", xcumbil);
            insertCommand.Parameters.AddWithValue("@cumpay", xcumpay);
            insertCommand.Parameters.AddWithValue("@operator", woperator);
            insertCommand.Parameters.AddWithValue("@facility", xclinic);
            insertCommand.Parameters.AddWithValue("@linkok", xlinkok);
            insertCommand.Parameters.AddWithValue("@procfunc", xprocfunc);
            insertCommand.Parameters.AddWithValue("@doctor", xdoctor);
            insertCommand.Parameters.AddWithValue("@sendexcl", xclusive);
            insertCommand.Parameters.AddWithValue("@transflag", xflag);


            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                vm.REPORTS.alertMessage = "Unable to Open SQL Server Database Table" + ex;
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }


        private static void writeBILLS(bool xnewrec, string xreference, decimal xitem, string xprocess, string xdescription,
           string xgrouphtype, decimal xamount, DateTime xdate, string xname, string xgrouphead, string xfacility, string xgroupcode, string xpatientno, string debitcredit_CD, string xghgroupcod, string xoperator, DateTime xop_time, string xextdesc, string xcurrency, decimal xexrate, int xfxtype, string xdiag, string xdoctor, bool xposted, string xpayref, string xservicetyp, decimal xpayment, string xpaytype, decimal xfcamount, string in_outpatient, bool receipted, int recid)
        {
            DateTime dtmin_date = DateTime.Now; //msmrfunc.mrGlobals.mta_start;
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


        bool ASCAN(string[] xarray, string searchstr, ref int rtnindex)
        {
            int xindex = -1;
            for (int i = 0; i < xarray.Length; i++)
            {
                if (xarray[i].Trim() == searchstr.Trim())
                {
                    xindex = i;
                    break;
                }
            }
            rtnindex = xindex;
            return xindex == -1 ? false : true;
        }

        /*
        IF m.mbillatrequest AND EMPTY(ThisForm.chkinpatient.Value) AND !isdelayedconsultcheck  AND !mfa_[x,10] OR ;
            m.mbillatrequest AND EMPTY(ThisForm.chkinpatient.Value) AND ;
                EMPTY(ThisForm.chkdelayedconsult.Value) AND !mfa_[x,10]
            IF !mfa_[x,11] OR mfa_[x,11] AND Grpprocedure.grpbillbyservtype &&= groupeditem 22-01-2013/25-01-2013
                thisform.raisebill(m.xbillonacctname,mfa_[x,6])
            ENDIF
        ENDIF
    **	WAIT WINDOW 'here...5'
        &&26-01-2013 if not billed at request time, and not billed by groupbillbyservicetype, service/bill is packaged.
        &&we must generate the bill here. system will not be able to know what amount to bill when results are being sent.
        IF !EMPTY(ThisForm.comroutinedrg.DisplayValue) AND !Grpprocedure.grpbillbyservtype AND mfa_[x,11]
             &&grouped item but not bill by service type and not bill at request time
             &&we add a flag in suspense to tell the service centre that bill had been raised !(grpbillbyservtype)
             thisform.raisebill(m.xbillonacctname,mfa_[x,6])
        ENDIF

        IF m.mbillatrequest AND ThisForm.chkinpatient.Value=1 AND xnewrequest &&AND LEFT(xmreference,1)='A' it can be 'C' 16/5/2012
            thisform.admupdate(mfa_[x,6])
        ENDIF
    **	WAIT WINDOW 'here...6'
        IF !EMPTY(mfa_[x,7])
            rtnstring = rtnstring+CHR(13)+;
            ALLTRIM(mfa_[x,7])
        ENDIF
    ENDIF
    SELECT SUSPENSE
NEXT



        if (!string.IsNullOrWhiteSpace(phamalterstr)) //update to medhist is Pharmacist alter prescription
        {
            MedHist.updatemedhistcomments(phbchain.GROUPCODE, phbchain.PATIENTNO,msmrfunc.mrGlobals.mtrans_date,phamalterstr);
        }
        if (chkinpatient.Checked == false & new string[] { "1", "2", "3", "4", "5","8", "9" }.Contains(msmrfunc.mrGlobals.msection))
            if (mdrgmarkup != 0m) //outpatient
            {
                txtcurrentTotal.Text = (Convert.ToDecimal(txtcurrentTotal.Text) + mdrgmarkup).ToString();
            }
            if (msmrfunc.mrGlobals.msection != "8")
            {
                LINK.WriteLINK(0, phbchain.GROUPCODE, phbchain.PATIENTNO, phbchain.NAME, "8", msmrfunc.mrGlobals.mreference,
                    Convert.ToDecimal(txtcurrentTotal.Text), 0, msmrfunc.mrGlobals.mfacility,
                    (msmrfunc.mrGlobals.cashpaying || phbchain.GROUPCODE == "NHIS" && isnhischarge) ? false : true,
                    msmrfunc.mrGlobals.mdoctor, false, 0, "", msmrfunc.mrGlobals.msection, woperator);
                //writemonitor(xmgroupcode,xmpatientno,date(),xmreference,"PRESCRIPTION",xmpatientname,TIME(),"",'P')                
            }

        //write details to billing
            {
                mdescription = "";
                msmrfunc.getFeefromtariff(mdrugcode, "", ref mdescription);
                mdescription = (string.IsNullOrWhiteSpace(mdescription)) ? "DRUGS" : mdescription;
                nhisdescription = "NHIS 10% DRUG CHARGE";
                bool isbillonaccount = false ;
                if (!string.IsNullOrWhiteSpace(phbchain.BILLONACCT))
                {
                    BillOnAcct = billchaindtl.Getbillchain(phbchain.BILLONACCT);
                    isbillonaccount = (BillOnAcct != null) ? true : false;
                }

                string servicetyp = "";
                decimal nhisdrgbill = 0m;
                if (phbchain.GROUPHTYPE == "C" && fee_for_service || phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge)
                {
                    servicetyp = "C";  //claim 17/02/2011
                }

                if (phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge)
                {
                    nhisdrgbill = Math.Round(Convert.ToDecimal(txtcurrentTotal.Text) * .10m);
                }
                decimal mitem = Billings.getBillNextItems(msmrfunc.mrGlobals.mreference,mdrugcode, true);

                Billings.writeBILLS( (mitem >= 1) ? false : true, msmrfunc.mrGlobals.mreference, mitem, mdrugcode, mdescription,
                    phbchain.GROUPHTYPE, (phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge) ? nhisdrgbill :
                    Convert.ToDecimal( txtcurrentTotal.Text), msmrfunc.mrGlobals.mtrans_date,
                    (isbillonaccount) ? BillOnAcct.NAME : phbchain.NAME, phbchain.GROUPHTYPE,
                    (string.IsNullOrWhiteSpace(xmfacility)) ? msmrfunc.mrGlobals.mfacility : xmfacility,phbchain.GROUPCODE,
                    (isbillonaccount) ? BillOnAcct.PATIENTNO : phbchain.PATIENTNO, "D",
                    phbchain.GHGROUPCODE, woperator, DateTime.Now,mxdesc,"", 0m, 0, 0m, "", msmrfunc.mrGlobals.mdoctor, false, "",
                    servicetyp, 0m, "", 0m,"O",false);

                /*   /if !EMPTY(billchai.currency) AND billchai.currency # mlocalcur
                     IF m.fxtype=1 OR EMPTY(ThisForm.txtfxtotal.value)
                         replace fcamount WITH amount,amount WITH ROUND(fcamount/curtable.rate,2)
                     ELSE
                         replace fcamount WITH ThisForm.txtfxtotal.value*curtable.rate,amount WITH ThisForm.txtfxtotal.value
                     ENDIF
                     replace exrate with curtable.rate,currency WITH billchai.currency
                 ENDIF - PENDING 25-11-2013

                 IF m.mdrugsonbill &&write details to mrb25
                    thisform.writedetails(billvouc.reference,billvouc.itemno)
                    ENDIF

                if (phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge)
                {
                    //11/06/2013 - I think we shuld write balance to capbills for proper accountability of cost of NHIS service
                    // "b" servicetyp is sent for capbills
                    Billings.writeBILLS( (mitem >= 1) ? false : true, msmrfunc.mrGlobals.mreference, mitem, mdrugcode, 
                    "Capitated Balance of NHIS DrugBill",
                    phbchain.GROUPHTYPE, Convert.ToDecimal( txtcurrentTotal.Text)-nhisdrgbill, msmrfunc.mrGlobals.mtrans_date,
                    (isbillonaccount) ? BillOnAcct.NAME : phbchain.NAME, phbchain.GROUPHTYPE,
                    (string.IsNullOrWhiteSpace(xmfacility)) ? msmrfunc.mrGlobals.mfacility : xmfacility,phbchain.GROUPCODE,
                    (isbillonaccount) ? BillOnAcct.PATIENTNO : phbchain.PATIENTNO, "D",
                    phbchain.GHGROUPCODE, woperator, DateTime.Now,mxdesc,"", 0m, 0, 0m, "", msmrfunc.mrGlobals.mdoctor, false, "",
                    "b", 0m, "", 0m,"O",false);
                }

                if (msmrfunc.mrGlobals.cashpaying || phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge)
                {
                    LINK.WriteLINK(0, phbchain.GROUPCODE, phbchain.PATIENTNO, phbchain.NAME, "2", msmrfunc.mrGlobals.mreference,
                    (phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge) ? nhisdrgbill : Convert.ToDecimal(txtcurrentTotal.Text), 0, msmrfunc.mrGlobals.mfacility,
                     false, msmrfunc.mrGlobals.mdoctor, false, 0, "", msmrfunc.mrGlobals.msection, woperator);
                }

                btnclose.PerformClick();
            }

        }*/

        void updatemrattend()
        {
            decimal mlastno = msmrfunc.getcontrol_lastnumber("CHARGNO", 5, true, Convert.ToDecimal(mreference.Substring(1, 8)), false);
            //        if (mlastno != lastnosave)

            mreference = bissclass.autonumconfig(mlastno.ToString(), true, "S", "999999999");

            /*  decimal lastnosave = mlastno;
              mlastno = msmrfunc.getcontrol_lastnumber("CHARGNO", 5, true, mlastno, false);
              if (mlastno != lastnosave)
              {
                  mreference = bissclass.autonumconfig(mlastno.ToString(), true, "S", "999999999");
              }*/
            //update daily attendance
            //? comboclinic.text may have been selected without any procedure

            Mrattend.mrattendWrite(true, mreference, vm.REPORTS.txtgroupcode, vm.REPORTS.txtpatientno, vm.REPORTS.TXTPATIENTNAME, vm.REPORTS.dtregistered, vm.REPORTS.REPORT_TYPE1, vm.REPORTS.txtgrouphead, vm.REPORTS.txtbillspayable, false, false, vm.REPORTS.txtghgroupcode, "", "", "", woperator, DateTime.Now, "S", false, string.IsNullOrWhiteSpace(vm.REPORTS.REPORT_TYPE2) ? "" : vm.REPORTS.REPORT_TYPE2, "");
        }

        //private void btnclose_Click(object sender, EventArgs e)
        //{
        //    if (btnsave.Enabled && listView1.Items.Count > 0)
        //    {
        //        DialogResult result = MessageBox.Show("Changes to Procedure Request Details have not been saved! Confirm to Close without saving", "Inv/Procecure Requests", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
        //        if (result == DialogResult.No)
        //            return;
        //    }
        //    this.Close();
        //}

        //void renumberview()
        //{
        //    for (int i = 0; i < listView1.Items.Count; i++)
        //    {
        //        listView1.Items[i].SubItems[0].Text = (i + 1).ToString();
        //    }
        //}

        private void createfacilitygroup()
        {
            // DataRow dr = sdt.NewRow();
            dttempfacility = new DataTable();
            dttempfacility.Columns.Add(new DataColumn("facility", typeof(string)));
            dttempfacility.Columns.Add(new DataColumn("billprocess", typeof(string)));
            dttempfacility.Columns.Add(new DataColumn("packqty", typeof(decimal)));

        }

        DataRow createnewRow_facility(string facility, decimal amount)
        {
            DataRow dr = dttempfacility.NewRow();

            dr["facility"] = facility;
            dr["amount"] = amount;
            dttempfacility.Rows.Add(dr);
            return dr;
        }

        //private void listView1_DoubleClick(object sender, EventArgs e)=======================
        //{
        //    recno = listView1.SelectedIndex;
        //    ListViewItem lv = listView1.Items[recno];
        //    this.cboDesc.Text = AnyCode = lv.SubItems[2].ToString();
        //    lblprocedure.Text = procedure = lv.SubItems[4].ToString();

        //    bissclass.displaycombo(cboFacility, dtfacility, lv.SubItems[13].ToString(), "name");
        //    lblfacility.Text = lv.SubItems[13].ToString();

        //    if (string.IsNullOrWhiteSpace(savedfacility) || savedfacility != lblfacility.Text)
        //    {
        //        processFacilitySetting();
        //        combolist("D");
        //    }

        //    savedfacility = lblfacility.Text;
        //    bissclass.displaycombo(cboDesc, dtDesc, procedure, "name");
        //    this.cboDesc.Focus();
        //}

        //private void btnClearSelections_Click(object sender, EventArgs e)
        //{
        //    if (listView1.Items.Count < 1)
        //        return;
        //    DialogResult result = MessageBox.Show("Confirm to Remove Selections... ", "Inv/Proc Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    if (result == DialogResult.No)
        //        return;
        //    listView1.Items.Clear();
        //    DisplayPrevDefinitions();
        //}

        //private void chkinpatient_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkinpatient.Checked && !inpatient)
        //    {
        //        DialogResult result = MessageBox.Show("OPD -> In-Patient Request Conversion ? \r\n This Consult will be converted on Submission of Details \r\n on Main Consult Platform...CONFIRM", "Investigation/Procedure Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //        if (result == DialogResult.No)
        //        {
        //            chkinpatient.Checked = false;
        //            return;
        //        }
        //        Session["Inpatient"] = "Y";
        //        inpatient = true;

        //    }
        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtpatientno_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtpatientno_LostFocus(null, null);
        //}

        //private void cboName_KeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    if (objArgs.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(cboName.Text))
        //    {
        //        SelectNextControl(ActiveControl, true, true, true, true);
        //    }
        //}

        //private void cboBillspayable_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    cboBillspayable_LostFocus(null, null);
        //}

        //private void txtgrouphead_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtgrouphead_LostFocus(null, null);
        //}

        //private void cboDesc_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(cboDesc.Text))
        //        cboDesc_LostFocus(null, null);
        //}

        //private void nmrduration_KeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    if (objArgs.KeyCode == Keys.Enter)
        //    {
        //        SelectNextControl(ActiveControl, true, true, true, true);
        //    }
        //}

        //private void cboGrpprocess_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(cboGrpprocess.Text))
        //        combogrpprocess_LostFocus(null, null);

        //}

        //private void btnConvert_Click(object sender, EventArgs e)===============================
        //{
        //    if (listView1.Items.Count < 1)
        //    {
        //        MessageBox.Show("No Request to convert...");
        //        return;
        //    }
        //    DialogResult result = MessageBox.Show("Confirm to Convert This Request to In-Patient \r\n NOTE : Patient must have been admitted and Admission Reference Available... CONTINUE ?", "Inv/Procedure Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    if (result == DialogResult.No)
        //        return;
        //    string xval = "";
        //    POPREAD popread = new POPREAD("OPD Requests -> In-Patient", "Enter Admission Reference...", ref xval, false, false, "", "", "", false, "", "");
        //    popread.ShowDialog();
        //    xval = bissclass.sysGlobals.anycode;
        //    if (string.IsNullOrWhiteSpace(xval))
        //        return;
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select name, groupcode, patientno from admrecs where reference = '" + xval + "'", false);
        //    if (dt.Rows.Count < 1)
        //    {
        //        MessageBox.Show("Invalid Admission Reference...");
        //        return;
        //    }
        //    DataRow row = dt.Rows[0];
        //    if (row["groupcode"].ToString() != bchain.GROUPCODE || row["patientno"].ToString() != bchain.PATIENTNO)
        //    {
        //        MessageBox.Show("Specified Reference belongs to another Patient - \r\n " + row["name"].ToString() + " " + row["groupcode"].ToString() + ":" + row["patientno"].ToString());
        //        return;
        //    }
        //    string updstr = "";
        //    foreach (ListViewItem itm in listView1.Items)
        //    {
        //        ADMDETAI.writeAdmdetails(true, mreference, dtregistered.Value, dtregistered.Value.ToString("HH:ss"), itm.SubItems[4].Text, itm.SubItems[5].Text, "", itm.SubItems[2].Text, "", 0m, Convert.ToDecimal(itm.SubItems[3].Text), false, dtmin_date, woperator, DateTime.Now, txtgroupcode.Text, txtpatientno.Text, mdoctor, itm.SubItems[1].Text, 0, "");

        //        updstr = "delete from billing where reference = '" + mreference + "' and process = '" + itm.SubItems[5].Text + "'";
        //        bissclass.UpdateRecords(updstr, "MR");
        //    }
        //    listView1.Items.Clear();
        //    MessageBox.Show("Completed...", "OPD To In-Patient Request");
        //}

        #endregion
    }
}