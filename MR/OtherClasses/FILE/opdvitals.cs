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
using MSMR.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class opdvitals
    {
        MR_DATA.MR_DATAvm vm;

        Vstata vstata = new Vstata();
        Mrattend mrattend = new Mrattend();
        billchaindtl bchain = new billchaindtl();
        DateTime dtmin_date = DateTime.Now;

        //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start; // (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        DataTable dtclinic = Dataaccess.GetAnytable("", "CODES", "select type_code,name from servicecentrecodes order by name", true),
        dtdocs = Dataaccess.GetAnytable("", "MR", "select reference,name from doctors where rectype = 'D' order by name", true);
        bool newrec, mdocson, mcanadd, mcandelete, mcanalter;
        string AnyCode = "", msection, woperator, mancclinic;
        string start_time;
        int sendrec = 0;

        public opdvitals(MR_DATA.MR_DATAvm VM2, string woperato)
        {
            vm = VM2;

            //msection = Session["section"].ToString(); // msmrfunc.mrGlobals.msection;
            msection = "1";
            woperator = woperato; // msmrfunc.mrGlobals.WOPERATOR;

            //InitializeComponent();

            getcontrolsettings();

            newrec = (vm.REPORTS.newrecString == "true") ? true : false;

            mrattend = Mrattend.GetMrattend(vm.REPORTS.txtreference);
            bchain = billchaindtl.Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);
            vstata = Vstata.GetVSTATA(vm.REPORTS.txtreference);
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    initcomboboxes();
        //}

        //private void initcomboboxes()
        //{

        //    //get primary doc
        //    this.combdocs.DataSource = dtdocs;
        //    combdocs.ValueMember = "Reference";
        //    combdocs.DisplayMember = "Name";
        //    //get clinic
        //    this.combclinic.DataSource = dtclinic;
        //    combclinic.ValueMember = "Type_code";
        //    combclinic.DisplayMember = "Name";

        //}

        private void getcontrolsettings()
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select docson, pvtcode from mrcontrol order by recid", false);

            mdocson = (bool)dt.Rows[0]["docson"];
            mancclinic = dt.Rows[2]["pvtcode"].ToString();
            //disable family clinic if not installed
            dt = Dataaccess.GetAnytable("", "CODES", "select installed from ctrolxl where recid = '3'", false);

            //if (!(bool)dt.Rows[0]["installed"])
            //    btnFamilyHealth.Visible = false;

            dt = Dataaccess.GetAnytable("", "MR", "select CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];
        }

        //private void btnAttendance_Click(object sender, EventArgs e)
        //{
        //    msmrfunc.mrGlobals.lookupCriteria = chkTodaysConsult.Checked ? "C" : "";
        //    this.txtreference.Text = "";
        //    msmrfunc.mrGlobals.crequired = "I";
        //    msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED DAILY ATTENDANCE";
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //// g - groupcode; L - patientno; I - daily attendance
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    // if (lookupsource == "I") //daiy attendance
        //    {
        //        this.txtreference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtreference.Focus();
        //    }
        //}

        //private void txtreference_Enter(object sender, EventArgs e)
        //{
        //    newrec = true;
        //    ClearControls();
        //    if (string.IsNullOrWhiteSpace(AnyCode)) //no lookup
        //    {
        //        //DataTable linkdat = msmrfunc.getLinkDetails("", 0, 0m, 0m, "", true, msection, 1, "", "");
        //        //if (linkdat.Rows.Count < 1)
        //        //{
        //        //    DialogResult result = MessageBox.Show("No Patient Awaiting Service...","Nurses Desk");
        //        //}
        //        //button1.PerformClick();
        //        //btnReload.Select();
        //        ////get list of patients for vitals
        //        //frmlinkinfo FrmLinkinfo = new frmlinkinfo("",0,0m,0m,"",true,msection,1,"","");
        //        //FrmLinkinfo.Closed += new EventHandler(FrmLinkinfo_Closed);
        //        //FrmLinkinfo.ShowDialog();
        //    }
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //get list of patients for vitals
        //    DataTable dt = msmrfunc.getLinkDetails("", 0, 0m, 0m, "", true, msection, 0, "", "");
        //    if (dt.Rows.Count > 0)
        //    {
        //        frmGetlinkinfo linkinfo = new frmGetlinkinfo(dt);
        //        linkinfo.ShowDialog();
        //        txtreference.Text = msmrfunc.mrGlobals.anycode;
        //        txtreference.Focus();
        //        /*    frmlinkinfo FrmLinkinfo = new frmlinkinfo("", 0, 0m, 0m, "", true, msection, 0, "", "");
        //            FrmLinkinfo.Closed += new EventHandler(FrmLinkinfo_Closed);
        //            FrmLinkinfo.Show();*/
        //    }
        //}

        //void FrmLinkinfo_Closed(object sender, EventArgs e)

        //{
        //    frmlinkinfo FrmLinkinfo_Closed = sender as frmlinkinfo;
        //    if (string.IsNullOrWhiteSpace(msmrfunc.mrGlobals.anycode))
        //        return;

        //    this.txtreference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //    this.txtreference.Focus();

        //}

        //private void txtreference_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtreference.Text))
        //    {
        //        AnyCode = "";
        //        //txtreference.Focus();
        //        return;
        //    }

        //    start_time = DateTime.Now.ToLongTimeString();
        //    newrec = true;
        //    if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtreference.Text))  //no lookup value obtained
        //    {
        //        this.txtreference.Text = bissclass.autonumconfig(this.txtreference.Text, true, "C", "999999999");
        //    }
        //    //check if in attendance records
        //    mrattend = Mrattend.GetMrattend(this.txtreference.Text);
        //    if (mrattend == null)
        //    {
        //        MessageBox.Show("Invalid Consultation Reference in Daily Attendance Register... ");
        //        this.txtreference.Text = " ";
        //        this.btnclose.Focus();
        //        return;
        //    }
        //    //CHECK NUMBER FORMAT
        //    if (txtreference.Text.Substring(0, 1) != "C")
        //    {
        //        MessageBox.Show("Not a valid Consultation Reference in Daily Attendance Register... ");
        //        this.txtreference.Text = " ";
        //        btnReload.PerformClick();
        //        return;
        //    }
        //    //patient profile
        //    bchain = billchaindtl.Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);
        //    if (bchain == null)
        //    {
        //        MessageBox.Show("Error in Patient Master File for this Consultaton Reference...'\r\n' Pls Check and Try and Again !",
        //            "Patient Master File Informaton");
        //        txtreference.Focus();
        //        return;
        //    }
        //    ClearControls();
        //    lblname.Text = mrattend.NAME;
        //    lbltrans_date.Text = mrattend.DTIME.ToString("dd-MM-yyyy @ HH:mm");
        //    dtHistoryDatefrom.Value = dtHistoryDateto.Value = mrattend.TRANS_DATE.Date; // vstata.TRANS_DATE;
        //    if (bchain.GROUPHTYPE == "C")
        //        lblgrouphead.Text = "< CORPORATE > " + bchain.GROUPHEAD;
        //    else
        //        lblgrouphead.Text = "< PRIVATE > " + bchain.GHGROUPCODE + ": " + bchain.GROUPHEAD;
        //    txtedtprofile.Text = "GENDER : [ " + bchain.SEX + " ] ;    AGE : ";
        //    string xx = (bchain.BIRTHDATE != dtmin_date.Date) ? bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now.Date) :
        //    (bchain.RELATIONSH == "C") ? "Minor" : (bchain.RELATIONSH == "S" || bchain.RELATIONSH == "W" ||
        //    bchain.RELATIONSH == "H") ? "< Adult >" : "...";
        //    string xx1 = "     M_STATUS : < " + bchain.M_STATUS + " > ";
        //    txtedtprofile.Text = txtedtprofile.Text + xx + "; " + xx1;
        //    //  this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown);
        //    // combclinic.Text = bissclass.combodisplayitemCodeName("type_code", mrattend.CLINIC, dtclinic, "name");
        //    bissclass.displaycombo(combclinic, dtclinic, mrattend.CLINIC, "type_code");
        //    //  this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList);
        //    if (!string.IsNullOrWhiteSpace(combclinic.Text) && combclinic.SelectedValue.ToString().Trim() == mancclinic.Trim())
        //        chkToClinicExclusive.Checked = true;
        //    else
        //        chkToClinicExclusive.Checked = false;


        //    if (!string.IsNullOrWhiteSpace(bchain.PICLOCATION))
        //        pictureBox1.Image = WebGUIGatway.getpicture(bchain.PICLOCATION);

        //    vstata = Vstata.GetVSTATA(this.txtreference.Text);
        //    if (vstata != null)
        //        this.DisplayDetails();
        //    else
        //    {
        //        DataTable oldvitas = Dataaccess.GetAnytable("", "MR", "select weight, hight, bpsitting, bpstanding, temp, pulse, respiratio, others, bmp,trans_date, time from vstata where groupcode = '" + mrattend.GROUPCODE + "' and patientno = '" + mrattend.PATIENTNO + "' and weight > '0' and hight > '0'", false);
        //        DataRow xrow = null;
        //        string xstring = "LastVitals : ";
        //        if (oldvitas.Rows.Count > 0)
        //        {
        //            xrow = oldvitas.Rows[oldvitas.Rows.Count - 1];
        //            xstring += Convert.ToDateTime(xrow["trans_date"]).ToShortDateString() + " @ " + xrow["time"].ToString() + " = ";
        //            xstring += !string.IsNullOrWhiteSpace(xrow["weight"].ToString()) ? " wt-" + xrow["weight"].ToString().Trim() + " : " : "";
        //            xstring += !string.IsNullOrWhiteSpace(xrow["hight"].ToString()) ? "ht-" + xrow["hight"].ToString().Trim() + " : " : "";
        //            xstring += !string.IsNullOrWhiteSpace(xrow["bpsitting"].ToString()) ? "BPs-" + xrow["bpsitting"].ToString().Trim() + " : " : "";
        //            xstring += !string.IsNullOrWhiteSpace(xrow["bpstanding"].ToString()) ? "BPstn-" + xrow["bpstanding"].ToString().Trim() + " : " : "";
        //            xstring += !string.IsNullOrWhiteSpace(xrow["temp"].ToString()) ? "tmp-" + xrow["temp"].ToString().Trim() + " : " : "";
        //            xstring += !string.IsNullOrWhiteSpace(xrow["pulse"].ToString()) ? "puls-" + xrow["pulse"].ToString().Trim() + " : " : "";
        //            xstring += !string.IsNullOrWhiteSpace(xrow["respiratio"].ToString()) ? "resp-" + xrow["respiratio"].ToString().Trim() + " : " : "";
        //            xstring += !string.IsNullOrWhiteSpace(xrow["others"].ToString()) ? "ots-" + xrow["others"].ToString().Trim() + " : " : "";
        //            nmrbmi.Text = xrow["bmp"].ToString();

        //            nmrweight.Value = Convert.ToDecimal(xrow["weight"]);
        //            nmrhight.Value = Convert.ToDecimal(xrow["hight"]);
        //            txtprev_vitals.Text = xstring;
        //            txtprev_vitals.Visible = true;

        //        }
        //        if (mrattend.ATTENDTYPE == "I")
        //        {
        //            txtothers.Text = "...FOR INJECTION...";
        //            DataTable injcard = Dataaccess.GetAnytable("", "MR", "select description from injcard where groupcode = '" + mrattend.GROUPCODE + "' and patientno = '" + mrattend.PATIENTNO + "' and timegiven = ''", false);
        //            if (injcard.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in injcard.Rows)
        //                {
        //                    txtothers.Text += "\r\n";
        //                    txtothers.Text += row["description"].ToString().Trim();
        //                }
        //            }
        //        }
        //        else if (mrattend.ATTENDTYPE == "E")
        //            txtothers.Text = "...FOR MEDICAL...";
        //        else if (mrattend.ATTENDTYPE == "F")
        //            txtothers.Text = "...FOR FOLLOW-UP VISIT...";
        //        else if (mrattend.ATTENDTYPE == "S")
        //            txtothers.Text = "...FOR SPECIALIST CONSULT...";
        //        else if (mrattend.ATTENDTYPE == "D")
        //        {
        //            txtothers.Text = "...FOR DRESSING...";
        //            DialogResult result = MessageBox.Show("Nurses must make Procedure Request for DRESSING to Appropriate Service Centre for Costing...");
        //        }
        //        else if (mrattend.ATTENDTYPE == "G")
        //            txtothers.Text = "...EMERGENCY...";
        //        else if (mrattend.ATTENDTYPE == "R")
        //            txtothers.Text = "...FOR DRUG REFILL...";
        //        //get medical history for last ?? days

        //    }

        //    nmrhight.Focus();
        //    AnyCode = "";
        //    btnsave.Enabled = true;
        //}

        //private void nmrhight_LostFocus(object sender, EventArgs e)
        //{
        //    decimal x;
        //    if (nmrhight.Value >= 1)
        //    // 1m = 3.28084ft ; 1m = 129.16693in
        //    // 1kg = 2.20462 ibs ; 1stone = 13ibs(pounds)
        //    //x = round( (this.value*3.28),2)
        //    {
        //        x = nmrhight.Value * 3.28M;
        //        this.nmrhightft.Text = x.ToString();
        //        //cstring = x.ToString().PadRight(2); //alltrim(str(x,14,2))
        //        //this.nmrhightin.Text = x.ToString().PadRight(2); // This.parent.nmrhightin.value = int(val((substr( cstring, AT('.',cstring)+1 ))))
        //    }
        //    if (nmrweight.Value > 0 && nmrhight.Value > 0)
        //        //nmrbmi.Text = Math.Sqrt( Convert.ToDouble( nmrhight.Value / nmrweight.Value ) ).ToString(); // sqrt( this.value/This.parent.nmrweight.value)
        //        nmrbmi.Text = Math.Round((nmrweight.Value / (nmrhight.Value * nmrhight.Value)), 2).ToString(); // sqrt( this.value/This.parent.nmrweight.value)
        //    //This.parent.nmrbmi.value = this.value/(This.parent.nmrhight.value*This.parent.nmrhight.value)
        //    nmrweight.Focus();
        //}

        //private void nmrweight_LostFocus(object sender, EventArgs e)
        //{
        //    decimal x;
        //    if (nmrweight.Value >= 1)
        //    // 1m = 3.28084ft ; 1m = 129.16693in
        //    // 1kg = 2.20462 ibs ; 1stone = 13ibs(pounds)
        //    //x = round( (this.value*3.28),2)
        //    {
        //        x = Math.Round(nmrweight.Value * 2.20M, 2);
        //        //  nmrweightibs.Text = x.ToString();
        //        nmrweightst.Text = Math.Round(x / 14, 0).ToString();
        //    }
        //    if (nmrhight.Value >= 1)
        //        // nmrbmi.Text = Math.Sqrt( Convert.ToDouble( nmrweight.Value /( nmrhight.Value * nmrweight.Value) ) ).ToString(); // sqrt( this.value/This.parent.nmrweight.value)
        //        nmrbmi.Text = Math.Round((nmrweight.Value / (nmrhight.Value * nmrhight.Value)), 2).ToString();
        //}

        //public void ClearControls()
        //{

        //    lblname.Text = lbltrans_date.Text = txtbpsitting.Text = txtbpstanding.Text = txtchiefcomplaint.Text = txtheadcircum.Text = txtPrev_records.Text = txtothers.Text = txtpulse.Text = txtrespiration.Text = txttemp.Text = nmrbmi.Text = nmrhightft.Text = nmrhightin.Text = nmrweightibs.Text = nmrweightst.Text = lblgrouphead.Text = combclinic.Text = nmrbmi.Text = combclinic.Text = combdocs.Text = txtedtprofile.Text = txtprev_vitals.Text = combrequestalert.Text = txtheadcircum.Text = "";
        //    nmrhight.Value = nmrweight.Value = 0;
        //    chkexclusive.Checked = false;
        //    pictureBox1.Image = "";

        //}

        //private void DisplayDetails()
        //{
        //    newrec = false;
        //    //change comboboxstyle to allow display of field value
        //    this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown);

        //    lblname.Text = mrattend.NAME;
        //    lbltrans_date.Text = mrattend.TRANS_DATE.ToString();
        //    txtbpsitting.Text = vstata.BPSITTING;
        //    txtbpstanding.Text = vstata.BPSTANDING;
        //    txtchiefcomplaint.Text = vstata.COMPLAINT;
        //    txtheadcircum.Text = vstata.HEADCIRCUMF.ToString();
        //    txtothers.Text = vstata.OTHERS;
        //    txtpulse.Text = vstata.PULSE;
        //    txtrespiration.Text = vstata.RESPIRATIO;
        //    txttemp.Text = vstata.TEMP;
        //    nmrbmi.Text = vstata.BMP.ToString();
        //    nmrhightft.Text = vstata.HFT.ToString();
        //    nmrhightin.Text = vstata.HIN;
        //    nmrweightibs.Text = vstata.WLBS;
        //    nmrweightst.Text = vstata.WSTONE;
        //    nmrhight.Value = vstata.HIGHT;
        //    nmrweight.Value = vstata.WEIGHT;
        //    /*mpatientno = vstata.PATIENTNO;
        //    mgroupcode = vstata.GROUPCODE;*/
        //    combclinic.Text = bissclass.combodisplayitemCodeName("type_code", vstata.CLINIC, dtclinic, "name");
        //    combclinic.SelectedValue = vstata.CLINIC;
        //    combdocs.Text = vstata.DOCTOR;
        //    combdocs.SelectedValue = vstata.DOCTOR;
        //    newrec = false;
        //    //revert to its original format
        //    this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList);
        //}

        public MR_DATA.MR_DATAvm btnsave_Click()
        {
            //if (!bissclass.IsPresent(this.txtreference, "Consultation Reference", false) ||
            //    !bissclass.IsPresent(this.lblname, "Patient Name", false) ||
            //    !bissclass.IsPresent(this.lbltrans_date, "Date Registered", false) ||
            //    !bissclass.IsPresent(this.combclinic, "Clinic", true) ||
            //    !bissclass.IsPresent(this.combdocs, "Consulting Doctor", true))
            //{
            //    return;
            //}

            //DialogResult result;

            //if (string.IsNullOrWhiteSpace(combclinic.Text))
            //{
            //    result = MessageBox.Show("Clinic/Facility must be selected...");
            //    return;
            //}

            ////save records
            //result = MessageBox.Show("Confirm to Save...", "Patient Vital Signs", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            //this.btnsave.Enabled = false;
           
            savepatientdetails();

            string updatestring = "";
            if (newrec) //update attendance counter and Link alert
            {
                string xflag, xtosection;
                if (mrattend.ATTENDTYPE == "D" || mrattend.ATTENDTYPE == "I")
                {
                    xflag = mrattend.ATTENDTYPE;
                    xtosection = "8"; //default to pharmacy
                    //combrequestalert.Text = "";
                }
                else
                {
                    xflag = (vm.REPORTS.chkbyacctofficers) ? "C" : "";
                    xtosection = mdocson ? "4" : "5";
                }

                WriteLINK(sendrec, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.NAME, xtosection, vm.REPORTS.txtreference, 0, 0, vm.SYSCODETABSvm.ServiceCentreCodes.name, false, vm.DOCTORS.NAME, (vm.REPORTS.chkByBranch) ? true : false, 9, xflag, msection, woperator);

                //update mrattend
                updatestring = "update mrattend set vtaken = @vtaken,clinic = @clinic,doctor = @doctor, sendexcl = @sendexcl where reference = '" + mrattend.REFERENCE + "'";

                SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = updatestring;
                insertCommand.Connection = connection;
                //insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@vtaken", true);
                insertCommand.Parameters.AddWithValue("@clinic", vm.SYSCODETABSvm.ServiceCentreCodes.name);
                insertCommand.Parameters.AddWithValue("@doctor", vm.DOCTORS.NAME);
                insertCommand.Parameters.AddWithValue("@sendexcl", vm.REPORTS.chkByBranch ? true : false);
                try
                {
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    vm.REPORTS.ActRslt = " " + ex; 
                    return vm;
                }
                finally
                {
                    connection.Close();
                }
            }

            //get medhist and update record
            string xcomment = "";
            MedHist medhist = MedHist.GetMEDHIST(mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.REFERENCE, false, true, mrattend.TRANS_DATE, "DESC");
            bool newmedhist = true;

            if (medhist != null)
            {
                newmedhist = false;
                xcomment = medhist.COMMENTS.Trim() + "\r\n";
            }

            string xx = Convert.ToDecimal(vm.REPORTS.txtsurname) > 0 ? "WEIGHT:" + vm.REPORTS.txtsurname.ToString() + "(kg) \r\n" : "";
            xx += Convert.ToDecimal(vm.REPORTS.txthomephone) > 0 ? "HEIGHT:" + vm.REPORTS.txthomephone.ToString() + " (m) [BMI :" + vm.REPORTS.lblBalbfDbCr.ToString() + "] \r\n" : "";
            xx += !string.IsNullOrWhiteSpace(vm.REPORTS.REPORT_TYPE3) ? "BP/s:" + vm.REPORTS.REPORT_TYPE3 + "\r\n" : "";
            xx += !string.IsNullOrWhiteSpace(vm.REPORTS.REPORT_TYPE4) ? "BP/stn:" + vm.REPORTS.REPORT_TYPE4 + "\r\n" : "";
            xx += !string.IsNullOrWhiteSpace(vm.REPORTS.combillcycle) ? "TEMP:" + vm.REPORTS.combillcycle + "\r\n" : "";
            xx += !string.IsNullOrWhiteSpace(vm.REPORTS.txtstaffno) ? "PULSE:" + vm.REPORTS.txtstaffno + "\r\n" : "";
            xx += !string.IsNullOrWhiteSpace(vm.REPORTS.categ_save) ? "RESPIRATION:" + vm.REPORTS.categ_save + "\r\n" : "";
            xx += true && !string.IsNullOrWhiteSpace(vm.REPORTS.txtdepartment) ? "Head Circumference :" + vm.REPORTS.txtdepartment + "\r\n" : "";
            xx += !string.IsNullOrWhiteSpace(vm.REPORTS.txtconsultamt) ? "Chief Complaint: " + vm.REPORTS.txtconsultamt + "\r\n" : "";
            xx += !string.IsNullOrWhiteSpace(vm.REPORTS.REPORT_TYPE5) ? "OTHERS:" + vm.REPORTS.REPORT_TYPE5 + "\r\n" : "";
            if (xcomment != "")
            {
                xcomment += string.Concat(Enumerable.Repeat("-", 144));
                xcomment += "\r\n";
            }
            xcomment += xx;
            MedHist.updatemedhistcomments(mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.TRANS_DATE, xcomment, newmedhist, mrattend.REFERENCE, mrattend.NAME, mrattend.GHGROUPCODE, mrattend.GROUPHEAD, "");

            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno) && vm.REPORTS.txtpatientno.Substring(0, 1) != "R")
            {
                xcomment += xcomment + "\r\n";
                xcomment += "*** NursesDesk To Docs ALERT !!! ***" + vm.REPORTS.txtpatientno.Trim();

                MedHist.updatemedhistcomments(mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.TRANS_DATE, xcomment, newmedhist, mrattend.REFERENCE, mrattend.NAME, mrattend.GHGROUPCODE, mrattend.GROUPHEAD, "");

                MRB21.Writemrb21Details(mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.TRANS_DATE, mrattend.NAME, vm.SYSCODETABSvm.ServiceCentreCodes.name.ToString(), woperator, vm.REPORTS.txtpatientno.Trim(), vm.REPORTS.txtreference, "3", "4", woperator, vm.DOCTORS.NAME.ToString(), "O");
            }

            //must update link DATEREC AND TIMEREC
            /* updatestring = "UPDATE LINK SET LINKOK = '1', DATEREC = '" + DateTime.Now.ToShortDateString() + "', TIMEREC = '" + DateTime.Now.ToLongTimeString() + "' WHERE reference = '" + mrattend.REFERENCE + "' and tosection = '3' ";
            bissclass.UpdateRecords(updatestring, "MR");*/
            LINK.ClearLink(vm.REPORTS.txtreference, "3", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), DateTime.Now.ToLongTimeString());
            // LINK.updateLinkOk(mrattend.REFERENCE, "3", DateTime.Now.Date, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), true, true);

            if (mrattend.ATTENDTYPE == "D" || mrattend.ATTENDTYPE == "I" || mrattend.ATTENDTYPE == "R") //for DRESSING/injection/refill
            {
                //flag to indicate this block was executed;
                vm.REPORTS.chkBroughtForward = true;
                vm.REPORTS.TXTPATIENTNAME = vm.REPORTS.TXTPATIENTNAME;
                vm.REPORTS.SessionOCP = woperator;

                //send alert to Dressing and Injection 
                //vstata = Vstata.GetVSTATA(vm.REPORTS.txtreference);
                //frmInjectionAlert injectionalert = new frmInjectionAlert(vstata.OTHERS, vstata.GROUPCODE, vstata.PATIENTNO, lblname.Text, vstata.REFERENCE, woperator, "");
                //injectionalert.ShowDialog();
            }

            start_time = DateTime.Now.ToLongTimeString();

            WriteLINK3(mrattend.GROUPCODE, mrattend.PATIENTNO, DateTime.Now, mrattend.NAME, "VITAL SIGNS                   ", vm.REPORTS.txtreference, DateTime.Now.ToLongTimeString(), start_time, "3", vm.SYSCODETABSvm.ServiceCentreCodes.name, start_time, woperator);
            
            //ClearControls();
            //btnReload.PerformClick();

            return vm;
        }


        private bool WriteLINK(int recrec, string GroupCodeID, string PatientID, string patname, string xtosection, string xreference, decimal xcumbil, decimal xcumpay, string xclinic, bool xlinkok, string xdoctor, bool xclusive, int xprocfunc, string xflag, string xfrmsection, string woperator)
        {
            //DateTime dtmin_date = BissClass.msmrfunc.mrGlobals.mta_start;
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
                MessageBox.Show("Unable to Open SQL Server Database Table" + ex, "Add Customer Details", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        private bool WriteLINK3(string GroupCodeID, string PatientID, DateTime xdate, string patname, string xsection, string xreference, string xtimesent, string xselected, string xrectype, string xfacility,
            string xtimein, string xoperator)
        {
            //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
            //DateTime dtmin_date = DateTime.Now;
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
                MessageBox.Show(" " + ex, "Add Attendance Monitor Details", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return true;

        }

        void savepatientdetails()
        {
            //DateTime dtmin_date = DateTime.Now;

            // patientinfo patients = new patientinfo();
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "VSTATA_Add" : "VSTATA_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", bchain.PATIENTNO);
            insertCommand.Parameters.AddWithValue("@groupcode", bchain.GROUPCODE);
            insertCommand.Parameters.AddWithValue("@reference", vm.REPORTS.txtreference);
            insertCommand.Parameters.AddWithValue("@gender", bchain.SEX);
            insertCommand.Parameters.AddWithValue("@wlbs", vm.REPORTS.txtcontactperson);
            insertCommand.Parameters.AddWithValue("@wstone", vm.REPORTS.txtghgroupcode);
            insertCommand.Parameters.AddWithValue("@weight", vm.REPORTS.txtsurname);
            insertCommand.Parameters.AddWithValue("@hft", vm.REPORTS.cbotitle);
            insertCommand.Parameters.AddWithValue("@hin", "");
            insertCommand.Parameters.AddWithValue("@hight", vm.REPORTS.txthomephone);
            insertCommand.Parameters.AddWithValue("@bpsitting", vm.REPORTS.REPORT_TYPE3);
            insertCommand.Parameters.AddWithValue("@bpstanding", vm.REPORTS.REPORT_TYPE4);
            insertCommand.Parameters.AddWithValue("@pulse", vm.REPORTS.txtstaffno);
            insertCommand.Parameters.AddWithValue("@temp", vm.REPORTS.combillcycle);
            insertCommand.Parameters.AddWithValue("@respiratio", vm.REPORTS.categ_save);
            insertCommand.Parameters.AddWithValue("@bmp", string.IsNullOrWhiteSpace(vm.REPORTS.lblBalbfDbCr) ? 0m : Convert.ToDecimal(vm.REPORTS.lblBalbfDbCr));
            insertCommand.Parameters.AddWithValue("@others", vm.REPORTS.REPORT_TYPE5);
            insertCommand.Parameters.AddWithValue("@clinic", vm.SYSCODETABSvm.ServiceCentreCodes.name);
            insertCommand.Parameters.AddWithValue("@doctor", vm.DOCTORS.NAME);
            insertCommand.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
            insertCommand.Parameters.AddWithValue("@posted", (newrec) ? false : vstata.POSTED);
            insertCommand.Parameters.AddWithValue("@post_date", (newrec) ? dtmin_date : vstata.POST_DATE);
            insertCommand.Parameters.AddWithValue("@trans_date", (newrec) ? DateTime.Now : vstata.TRANS_DATE);
            insertCommand.Parameters.AddWithValue("@haircolor", (newrec) ? "" : vstata.HAIRCOLOR);
            insertCommand.Parameters.AddWithValue("@hairtype", (newrec) ? "" : vstata.HAIRTYPE);
            insertCommand.Parameters.AddWithValue("@eyecolor", (newrec) ? "" : vstata.EYECOLOR);
            insertCommand.Parameters.AddWithValue("@complexion", (newrec) ? "" : vstata.COMPLEXION);
            insertCommand.Parameters.AddWithValue("@racialgrp", (newrec) ? "" : vstata.RACIALGRP);
            insertCommand.Parameters.AddWithValue("@ethnicity", (newrec) ? "" : vstata.ETHNICITY);
            insertCommand.Parameters.AddWithValue("@religion", (newrec) ? "" : vstata.RELIGION);
            insertCommand.Parameters.AddWithValue("@bloodgrp", (newrec) ? "" : vstata.BLOODGRP);
            insertCommand.Parameters.AddWithValue("@complaint", vm.REPORTS.txtconsultamt);
            insertCommand.Parameters.AddWithValue("@headcircumf", (vm.REPORTS.txtdepartment == "") ? 0m : Convert.ToDecimal(vm.REPORTS.txtdepartment));
            insertCommand.Parameters.AddWithValue("@operator", woperator);
            insertCommand.Parameters.AddWithValue("@opdttime", DateTime.Now);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;
            }
            catch (SqlException ex)
            {
                // throw ex;
                vm.REPORTS.alertMessage = " " + ex;
                return;
            }
            finally
            {
                connection.Close();
                //update medhist
            }
            // btnReload.PerformClick();
        }

        //void combostyleset(ComboBoxStyle xval)
        //{
        //    // xval = "Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown";
        //    this.combclinic.DropDownStyle = xval;
        //    this.combdocs.DropDownStyle = xval;
        //    return;
        //}

        //private void btnclose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void nmrhight_Enter(object sender, EventArgs e)
        //{
        //    nmrhight.Select(0, nmrhight.Text.Length);
        //}

        //private void nmrweight_Enter(object sender, EventArgs e)
        //{
        //    nmrweight.Select(0, nmrweight.Text.Length);
        //}


        //==================================================================================================================

        //private void btnprofile_Click(object sender, EventArgs e)
        //{
        //    if (mrattend == null || string.IsNullOrWhiteSpace(mrattend.PATIENTNO))
        //    {
        //        DialogResult result = MessageBox.Show("A Patient Details must be in Focus As Reference to this process..");
        //        return;
        //    }

        //    string xstr = "";
        //    frmInvProcRequest invrequest = new frmInvProcRequest("P", mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.GROUPHTYPE, DateTime.Now, mrattend.NAME, mrattend.GROUPHEAD, mrattend.GHGROUPCODE, false, ref xstr, "", 0m, "", msection, mcanadd, mcanalter, mcandelete, woperator);
        //    invrequest.ShowDialog();
        //}

        //private void btndocsprofile_Click(object sender, EventArgs e)
        //{
        //    string xstr = "SELECT REFERENCE, GROUPCODE, PATIENTNO, NAME, TRANS_DATE, CLINIC, GROUPHEAD, GROUPHTYPE, VTAKEN, GHGROUPCODE, DOCTOR, DOC_TIME, ATTENDTYPE, CHAR(50) AS DOCSNAME, CHAR(50) AS ghname from mrattend where CONVERT(date, trans_date) = '" + DateTime.Now.ToShortDateString() + "' and doc_time  = '' and LEFT(reference,1) = 'C' order by doctor,trans_date";
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", xstr, false);
        //    if (dt.Rows.Count < 1)
        //    {
        //        DialogResult result = MessageBox.Show("No Patient Waiting... ");
        //        return;
        //    }
        //    DataTable dtcust = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO,NAME FROM CUSTOMER", false);
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        if (!string.IsNullOrWhiteSpace(row["doctor"].ToString())) // chkOnDocsPatProfile.Checked)
        //            row["docsname"] = bissclass.combodisplayitemCodeName("reference", row["doctor"].ToString(), dtdocs, "name");
        //        else
        //            row["docsname"] = "< Unspecified >";
        //        if (row["patientno"].ToString() == row["grouphead"].ToString())
        //            row["ghname"] = "< S E L F >";
        //        else if (row["grouphtype"].ToString() == "P")
        //            row["ghname"] = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P");
        //        else
        //            row["ghname"] = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dtcust, "name");
        //    }
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(dt);
        //    Session["rdlcfile"] = "Attendance_ByDocs.rdlc";
        //    Session["sql"] = "";
        //    Session["waitonly"] = "Y";
        //    string mrptheader = "WAITING LIST FOR " + DateTime.Now.ToLongDateString();
        //    frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, "", "", "", "WAITINGLIST", "", 0m, "", "", "", ds, true, 0, DateTime.Now, DateTime.Now, "", false, "", woperator);
        //    paedreports.Show();
        //}

        //private void btnBPHistory_Click(object sender, EventArgs e)
        //{
        //    if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO))
        //        return;
        //    frmBPHistory bphist = new frmBPHistory(bchain.GROUPCODE, bchain.PATIENTNO, bchain.NAME);
        //    bphist.Show();
        //}

        //private void btnGetHistory_Click(object sender, EventArgs e)
        //{
        //    txtPrev_records.Text = MedHist.GetMEDHISTCaseNotes(bchain.GROUPCODE, bchain.PATIENTNO, false, true, dtHistoryDatefrom.Value.Date, dtHistoryDateto.Value.Date, bchain, "DESC");
        //    if (string.IsNullOrWhiteSpace(txtPrev_records.Text))
        //    {
        //        MessageBox.Show("No Data");
        //    }
        //}

        //private void btnFamilyHealth_Click(object sender, EventArgs e)
        //{
        //    if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO))
        //    {
        //        MessageBox.Show("A Patient profile must be Selected...");
        //        return;
        //    }
        //    frmFH_Main commhealth = new frmFH_Main(bchain, woperator);
        //    commhealth.Show();
        //}

        ////private void txtreference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        ////{
        ////    txtreference_LostFocus(null, null);
        ////}

        ////private void nmrhight_KeyDown(object objSender, KeyEventArgs objArgs)
        ////{
        ////    if (objArgs.KeyCode == Keys.Enter)
        ////        nmrhight_LostFocus(null, null);
        ////    /*           {
        ////                   SelectNextControl(ActiveControl, true, true, true, true);
        ////               }*/
        ////}

        ////private void nmrweight_KeyDown(object objSender, KeyEventArgs objArgs)
        ////{
        ////    if (objArgs.KeyCode == Keys.Enter)
        ////    {
        ////        nmrweight_LostFocus(null, null);
        ////        SelectNextControl(ActiveControl, true, true, true, true);
        ////    }
        ////}

        //private void txtbpsitting_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void combclinic_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //-------------------------------------------------------------------------------------------
    }
}