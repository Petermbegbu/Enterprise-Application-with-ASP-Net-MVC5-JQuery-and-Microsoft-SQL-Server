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

//using System.Windows.Forms.Design;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using Gizmox.WebGUI.Common.Resources;
using OtherClasses.Models;


#endregion

namespace OtherClasses.FILE
{
    public partial class frmGroupMembersdt
    {
        billchaindtl bchain;
        MR_DATA.MR_DATAvm vm;
        OpenFileDialog dialog = new OpenFileDialog();
        ContextMenu blah = new ContextMenu();
        patientinfo patients;
        Customer customer;

        public string errorProp { get; set; }

        string PicSelected, mloccountry, mgrouphtype = "", woperator = "", msection;
        bool misrereg, misreregpvt, misreregall, mgenregistration, autogenreg, must_patphoto, mpauto, valuefrmgrouphead = false;
        decimal mlastno, mduration; //, savedsurname, savedoname isgprestrict

        bool newrec = true, mcanadd, mcandelete, mcanalter;
        string AnyCode, Anycode1, lookupsource, mpatcateg, mHMOCODE, msgeventtracker, spnotes, mednotes;

        //AnyCode = Anycode1 = lookupsource = mpatcateg = mHMOCODE = spnotes = mednotes = "";
        public frmGroupMembersdt(MR_DATA.MR_DATAvm VM2, string woperato)
        {
            vm = VM2;
            bchain = new billchaindtl();
            patients = new patientinfo();
            customer = new Customer();

            //InitializeComponent();
            getcontrolsettings();

            AnyCode = Anycode1 = lookupsource = mpatcateg = mHMOCODE = spnotes = mednotes = "";

            woperator = woperato;

            //if (groupheaddetail.patientno != null)
            //{
            //    txtsurname.Text = groupheaddetail.surname;
            //    txtresidence.Text = groupheaddetail.address1.Trim();
            //    txtghgroupcode.Text = groupheaddetail.groupcode;
            //    txtgrouphead.Text = groupheaddetail.grouphead;
            //    valuefrmgrouphead = true;
            //    woperator = xoperator;
            //}
            //else
            //{
            //    msection = Session["section"].ToString();
            //    woperator = Session["operator"].ToString();
            //}
        }

        //private void frmGroupMembersdt_Load(object sender, EventArgs e)
        //{
        //    this.txtgroupcode.ContextMenu = blah;
        //    initcomboboxes();
        //}

        //string lcFile = "";
        //  private void initcomboboxes()
        //  {
        //      /*         this.comhmoservgrp.DataSource = selcode.getcombolist("hp");
        //comhmoservgrp.ValueMember = "HMOSERVTYPE";
        ////Setting Combo Box DisplayMember Property
        //comhmoservgrp.DisplayMember = "HMOSERVTYPE + ' : ' +Description ";*/

        //      //get currency
        //      this.cboCurrency.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from currencycodes", true);
        //      cboCurrency.ValueMember = "Type_code";
        //      cboCurrency.DisplayMember = "name";

        //      //get clinic
        //      this.cboClinic.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from servicecentrecodes", true);
        //      cboClinic.ValueMember = "Type_code";
        //      cboClinic.DisplayMember = "name";

        //  }

        private void getcontrolsettings()
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select Loccountry, PAUTO, cashpoint, filemode, dactive, Last_no, installed, attendlink from mrcontrol order by recid", false);

            mloccountry = dt.Rows[0]["Loccountry"].ToString();
            mpauto = (bool)dt.Rows[0]["PAUTO"];

            misrereg = (bool)dt.Rows[1]["cashpoint"];
            misreregpvt = (bool)dt.Rows[1]["filemode"];
            misreregall = (bool)dt.Rows[1]["dactive"];

            mduration = (decimal)dt.Rows[2]["Last_no"];

            must_patphoto = (bool)dt.Rows[4]["installed"];

            autogenreg = (bool)dt.Rows[5]["attendlink"];

            //if (!misrereg)
            //{
            //    txtnewexpiry.Visible = lblExpiry.Visible = false;
            //}

            //dt = Dataaccess.GetAnytable("", "MR", "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            //mcanadd = (bool)dt.Rows[0]["canadd"];
            //mcanalter = (bool)dt.Rows[0]["canalter"];
            //mcandelete = (bool)dt.Rows[0]["candelete"];
        }

        //private void msgBoxHandler(object sender, EventArgs e)
        //{
        //    Form msgForm = sender as Form;
        //    if (msgForm != null)
        //    {
        //        if (msgeventtracker == "PN" && msgForm.DialogResult == DialogResult.Yes) //Gizmox.WebGui.Forms.DialogResult.Yes)
        //        {
        //            this.ClearControls("N"); //its new
        //                                     //this.cmbsave.Enabled = msmrfunc.GlobalAccessCheck("A");
        //            this.btnsave.Enabled = true;
        //            newrec = true;
        //            return;
        //        }
        //        else if (msgeventtracker == "DT")
        //        {
        //            this.dtreg_date.Focus();
        //            return;
        //        }
        //        else if (msgeventtracker == "GH")
        //        {
        //            txtgrouphead.Focus();
        //            return;
        //        }
        //        else if (msgeventtracker == "PS" && msgForm.DialogResult == DialogResult.Yes) //TO SAVE
        //        {
        //            this.savepatientdetails();

        //            this.txtpatientno.Focus();
        //            return;

        //        }
        //        else if (msgeventtracker == "PD" && msgForm.DialogResult == DialogResult.Yes) //TO DELETE
        //        {
        //            //patientinfo.DeletePatient(txtpatientno.Text);
        //            billchaindtl.DeleteBillchain(txtpatientno.Text, txtgroupcode.Text);
        //            this.ClearControls("D");
        //            this.txtpatientno.Text = "";
        //            this.txtgroupcode.Focus();
        //            return;
        //        }
        //        else if (msgeventtracker == "GP" && msgForm.DialogResult != DialogResult.Cancel) //GET PICTURE
        //        {
        //            //get the name of the file
        //            string lcFile = dialog.FileName;
        //            byte[] Bytes = { };

        //            // this.pictureBox1.BackgroundImage = new BissClass.DynamicStreamResourceHandle(Bytes, "image/jpeg");
        //        }
        //        else
        //        {
        //            this.txtpatientno.Text = "";
        //            return; // this.txtpatientno.Focus();
        //        }
        //    }
        //}

        //public void ClearControls(string xtype)
        //{
        //    // DateTime xd;
        //    // DateTime.TryParse("01/01/0000", out xd);
        //    lookupsource = mpatcateg = mHMOCODE = spnotes = mednotes = "";
        //    this.dtbirthdate.Value = this.txtnewexpiry.Value = this.dtreg_date.Value = DateTime.Now;
        //    txtname.Text = txtothernames.Text =
        //        this.txtbranch.Text = this.txtdepartment.Text = this.txtphone.Text = this.txtemail.Text =
        //        txtsurname.Text = txtstaffno.Text = txtresidence.Text = txtbillonacct.Text = "";
        //    this.spndependants.Value = 0;
        //    pictureBox1.Image = "";
        //    this.cboGender.SelectedIndex = this.cboRelationship.SelectedIndex = this.cboClinic.SelectedIndex = cboMaritalstatus.SelectedIndex = this.cboCurrency.SelectedIndex = cboTitle.SelectedIndex = -1;
        //    if (xtype == "A") //all fields
        //    {
        //        lblbillonaccount.Text = lblbranch.Text = lblgrouphead.Text = "";
        //        txtbillonacct.Text = txtgroupcode.Text = txtghgroupcode.Text = txtgrouphead.Text = txtpatientno.Text = txtname.Text = "";
        //        this.txtstaffno.Text = this.cboHmoservgrp.Text = this.txtresidence.Text = txtsurname.Text = txtothernames.Text = lblgrouphead.Text = lblcurrentgrouphead.Text = "";
        //    }
        //    else if (xtype == "D") //deleted
        //    {
        //        txtpatientno.Text = txtname.Text = this.txtstaffno.Text = "";
        //        this.cboHmoservgrp.SelectedIndex = -1;
        //    }
        //    else if (xtype == "N") //new record
        //        txtname.Text = "";
        //}

        //void combostyleset(ComboBoxStyle xval)
        //{
        //    // xval = "Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown";
        //    this.cboHmoservgrp.DropDownStyle = xval;
        //    this.cboMaritalstatus.DropDownStyle = xval;
        //    this.cboGender.DropDownStyle = xval;
        //    this.cboCurrency.DropDownStyle = xval;
        //    this.cboClinic.DropDownStyle = xval;
        //    return;
        //}

        //private void btnghgroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = (Button)sender;
        //    //   this.textBox1.Text = btn.Text;
        //    if (btn.Name == "btnghgroupcode")
        //    {
        //        this.txtghgroupcode.Text = "";
        //        lookupsource = "pg";
        //        msmrfunc.mrGlobals.crequired = "pp"; //patient grouphead groupcode
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PRIVATE/FAMILY GROUPHEADS";
        //    }

        //    else if (btn.Name == "btngroupcode")
        //    {
        //        this.txtgroupcode.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "g"; //billchain groupcode
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //        msmrfunc.mrGlobals.lookupCriteria = string.IsNullOrWhiteSpace(txtgroupcode.Text) ? "" : txtgroupcode.Text; // mgrouphtype == "P" ? patients.patientno : customer.Custno;
        //    }
        //    else if (btn.Name == "btnpatientlookup")
        //    {
        //        this.txtpatientno.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L"; //billchain member
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //    }
        //    else if (btn.Name == "btnbillonaccount")
        //    {
        //        this.txtbillonacct.Text = "";
        //        lookupsource = "bonact";
        //        msmrfunc.mrGlobals.crequired = "g"; //billchain member
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //    }
        //    else if (btn.Name == "btnPrincipal")
        //    {
        //        this.txtPrinciapl.Text = "";
        //        lookupsource = "PRINCIPAL";
        //        msmrfunc.mrGlobals.crequired = "PRINCIPAL"; //billchain member
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED (STAFF) PATIENTS OF " + customer.Name;
        //        msmrfunc.mrGlobals.lookupCriteria = customer.Custno;
        //    }
        //    else
        //    {
        //        this.txtgrouphead.Text = "";
        //        lookupsource = "P";
        //        msmrfunc.mrGlobals.crequired = mgrouphtype == "C" ? "C" : "PP";
        //        msmrfunc.mrGlobals.frmcaption = (mgrouphtype == "C") ? "LOOKUP FOR REGISTERED CORPORATE CLIENTS" : "LOOKUP FOR REGISTERED PATIENTS";
        //    }

        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();

        //}


        //p = txtghgroupcode; g = txtgroupcode; L = txtpatientno.Text ; bonact = txtbillonacct
        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;

        //    if (lookupsource == "g") //groupcodee
        //    {
        //        this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //        msmrfunc.mrGlobals.anycode = msmrfunc.mrGlobals.anycode1 = msmrfunc.mrGlobals.lookupCriteria = "";
        //        this.txtgroupcode.Focus();
        //        return;
        //    }

        //    else if (lookupsource == "L") //patientno
        //    {
        //        if (string.IsNullOrWhiteSpace(txtgroupcode.Text))
        //            this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode1;
        //        msmrfunc.mrGlobals.lookupCriteria = "";
        //        this.txtpatientno.Focus();
        //        return;
        //    }
        //    else if (lookupsource == "pg") //grouphead code
        //    {
        //        this.txtghgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtgrouphead.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //        msmrfunc.mrGlobals.anycode = msmrfunc.mrGlobals.anycode1 = "";
        //        this.txtghgroupcode.Focus();
        //        return;
        //    }
        //    else if (lookupsource == "bonact") //BILL ON ACCOUNT code
        //    {
        //        this.txtbillonacct.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        msmrfunc.mrGlobals.anycode = msmrfunc.mrGlobals.lookupCriteria = "";
        //        this.txtbillonacct.Focus();
        //        return;
        //    }
        //    else if (lookupsource == "PRINCIPAL")
        //    {
        //        this.txtPrinciapl.Text = AnyCode = msmrfunc.mrGlobals.anycode1;
        //        msmrfunc.mrGlobals.anycode = msmrfunc.mrGlobals.lookupCriteria = "";
        //        this.txtPrinciapl.Focus();
        //        return;
        //    }
        //    else if (lookupsource == "P") //GROUPHEAD
        //    {
        //        this.txtgrouphead.Text = mgrouphtype == "C" ? msmrfunc.mrGlobals.anycode : msmrfunc.mrGlobals.anycode1;
        //        //    msmrfunc.mrGlobals.anycode = "";
        //        this.txtgrouphead.Focus();
        //        //   this.txtgrouphead.Text = msmrfunc.mrGlobals.anycode1;
        //        return;
        //    }

        //}

        private void button1_Click(object sender, EventArgs e)
        {
            //WebGUIGatway.GETpictureNew();
            //PicSelected = msmrfunc.mrGlobals.pictSelected;
            //if (!string.IsNullOrWhiteSpace(PicSelected))
            //{ pictureBox1.Image = WebGUIGatway.getpicture(PicSelected); }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Open bitmap or jpeg.";
            openFileDialog1.Filter = "jpg files (*.jpg);*.jpg;*.* | bmp files (*.bmp); *.bmp";
            openFileDialog1.Closed += openFileDialog1_Closed;
            openFileDialog1.ShowDialog();
        }

        void openFileDialog1_Closed(object sender, EventArgs e)
        {
            //string xp = "c:\\MSSQLSV\\PHOTO\\";
            OpenFileDialog openFileDialog1 = sender as OpenFileDialog;
            if (openFileDialog1.Files.Count > 0)
            {
                HttpPostedFileHandle file = (HttpPostedFileHandle)openFileDialog1.Files[0];
                string filename = System.IO.Path.GetFileName(file.PostedFileName);
                string savepath = VWGContext.Current.Server.MapPath("~/Resources/Images/" + filename);
                // string savepath = xp + filename; // VWGContext.Current.Server.MapPath(xp + filename);
                PicSelected = savepath;
                ImageResourceHandle imageResourceHandlePic1 = new ImageResourceHandle();


                // if (clsMain.FileExists(savepath)) clsMain.FileDelete(savepath);
                file.SaveAs(savepath);
                file.Close();
                file.Dispose();
                imageResourceHandlePic1.File = filename;
                //pictureBox1.Image = imageResourceHandlePic1;
            }
        }

        // //string lcFile = "";
        // msgeventtracker = "GP";
        // //Create the OpenFileDialog (note that the FileDialog class is an abstract and cannot be used directly)
        //// OpenFileDialog dialog = new OpenFileDialog();

        // //Specify the default filter to use for displaying files
        // //	ofd.Filter = "All Files (*.*)|*.*|All Graphic Files (*.bmp;*.dib;*.jpg;*.cur;ani;*.ico;*.gif)|*.bmp;*.dib;*.jpg;*.cur;*.ani;*.ico;*.gif|Bitmap (*.bmp;*.dib)|*.bmp;*.dib|Cursor (*.cur)|*.cur|Animated Cursor (*.ani)|*.ani|Icon (*.ico)|*.ico|JPEG (*.jpg)|*.jpg|GIF (*.gif)|*.gif";
        // dialog.Filter = "bmp files (*.bmp)|*.bmp|jpg files (*.jpg*)|*.jpg*|All files (*.*)|*.*";
        // dialog.FilterIndex = 2;
        // dialog.RestoreDirectory = true;

        // //Show the dialog and if the user selects a file return the file name
        // dialog.ShowDialog(msgBoxHandler);// != DialogResult.Cancel)
        // //{
        //     //get the name of the file
        //     //lcFile = dialog.FileName;
        //     // this.pictureBox1.Image = lcFile;
        //     // this.pictureBox1.Image = new ImageResourceHandle(lcFile);
        //     //MessageBox.Show(lcFile + " File Selected!");

        // //}


        // /*
        //OpenFileDialog dialog = new OpenFileDialog();
        ////dialog.InitialDirectory = "c:\\";
        //dialog.Filter = "bmp files (*.bmp)|*.bmp|jpg files (*.jpg*)|*.jpg*|All files (*.*)|*.*";
        //dialog.FilterIndex = 2;
        //dialog.RestoreDirectory = true;

        //string xfile = dialog.ShowDialog().ToString();
        //MessageBox.Show(xfile+" File Selected!");


        //  this.pictureBox1.Image = dialog.FileName;

        //if (dialog.ShowDialog() == DialogResult.OK)
        //{
        //    MessageBox.Show(dialog.FileName, "My Application", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //}
        //else+
        //    MessageBox.Show("No File Selected!");
        // */
        // /*  OpenFileDialog dialog = new OpenFileDialog();            
        //   if (dialog.ShowDialog()==DialogResult.OK){
        //       MessageBox.Show(dialog.FileName,"My Application", MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        //       string s; 
        //       s=".bmp";
        //       if (dialog.FileName.Substring(dialog.FileName.LastIndexOf('.')).Equals(s))
        //       {
        //          this.pictureBox1.Load(dialog.FileName);
        //           BitmapFile = new Bitmap(dialog.FileName.ToString());
        //       }
        //       else {
        //           MessageBox.Show("Not a BMP file!");
        //       }*/

        // /*     this.pictureBox1.Image = Image.FromFile
        //      (System.Environment.GetFolderPath
        //      (System.Environment.SpecialFolder.Personal)
        //      + @"\Image.gif");
        //     */
        //}

        //private void btnclose_Click(object sender, EventArgs e)
        //{
        //    // pleaseWait.Close(); //  .Hide();
        //    this.Dispose();
        //    this.Close();
        //}

        //private void dtreg_date_Validating(object sender, CancelEventArgs e)
        //{
        //    if (this.dtreg_date.Value < this.dtbirthdate.Value || this.dtreg_date.Value > DateTime.Now)
        //    {
        //        msgeventtracker = "DT";
        //        MessageBox.Show("Invalid Date - Must be between date of Birth and today...", msgBoxHandler);
        //        //this.dtreg_date.Focus();
        //    }
        //}

        //private void comgrouphtype_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(comgrouphtype.Text))
        //        return;

        //    //if ( this.comgrouphtype.SelectedItem == "CORPORATE" )
        //    //{
        //    if (!valuefrmgrouphead && !chkRememberDetails.Checked)
        //    {
        //        ClearControls("A");
        //    }
        //    // Char xgrouphtype = this.comgrouphtype.SelectedItem.ToString()[0];
        //    mgrouphtype = comgrouphtype.Text.Substring(0, 1); // xgrouphtype.ToString();
        //    txtghgroupcode.Enabled = mgrouphtype == "P" ? true : false;
        //    btnghgroupcode.Enabled = mgrouphtype == "P" ? true : false;

        //    if (mgrouphtype == "P")
        //    {
        //        cboHmoservgrp.Enabled = false;
        //        btnpvtacct.Enabled = false;
        //        txtghgroupcode.Focus();
        //    }
        //    else
        //    {
        //        cboHmoservgrp.Enabled = true;
        //        txtgrouphead.Focus();
        //    }
        //    valuefrmgrouphead = false;
        //    // }
        //}

        //private void txtgroupcode_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right) //Gizmox.WebGUI.Forms.
        //    {
        //        if (comgrouphtype.Text.Substring(0, 1) == "P")
        //        {
        //            return;
        //        }
        //        txtgroupcode.Text = "";
        //        msmrfunc.mrGlobals.crequired = "C";
        //        lookupsource = "g"; //on groupcode
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR CORPORATE CLIENTS GROUPCODE";
        //        frmselcode FrmSelCode = new frmselcode();
        //        FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //        FrmSelCode.ShowDialog();
        //    }

        //}

        //private void txtghgroupcode_LostFocus(object sender, EventArgs e)
        //{
        //    AnyCode = "";
        //}

        //private void txtgrouphead_LostFocus(object sender, EventArgs e)
        //{
        //    DialogResult result;
        //    if (string.IsNullOrWhiteSpace(txtgrouphead.Text) || mgrouphtype == "P" && string.IsNullOrWhiteSpace(txtghgroupcode.Text))
        //        return;

        //    customer = new Customer();
        //    patients = new patientinfo();
        //    if (mgrouphtype == "P")
        //        patients = patientinfo.GetPatient(this.txtgrouphead.Text, txtghgroupcode.Text);
        //    else
        //        customer = Customer.GetCustomer(this.txtgrouphead.Text);

        //    if (mgrouphtype == "P" && patients == null || mgrouphtype == "C" && customer == null)
        //    {
        //        msgeventtracker = "GH";
        //        result = MessageBox.Show("Invalid GroupHead Specification as responsible for Bills");
        //        return;
        //    }
        //    else
        //    {
        //        // this.DisplayPatients();
        //        this.lblgrouphead.Text = (mgrouphtype == "P") ? patients.name : customer.Name;
        //        if (mgrouphtype == "P" && !patients.isgrouphead)
        //        {
        //            msgeventtracker = "GH";
        //            result = MessageBox.Show("Specified Patient is not a registered GroupHead...");
        //            // txtgrouphead.Select();
        //            return;
        //        }
        //    }
        //    if (mgrouphtype == "C" && !customer.ISGROUPHEAD)
        //    {
        //        MessageBox.Show("This Client is not a grouphead...CHECK CORPORATE CIENTS REGISTARTION");
        //        txtgrouphead.Text = "";
        //        return;
        //    }
        //    mpatcateg = (mgrouphtype == "P") ? patients.patcateg : customer.Patcateg;
        //    mHMOCODE = (mgrouphtype == "C" && customer.HMO) ? customer.Custno : " ";
        //    if (mgrouphtype == "C" && customer.HMO)
        //        lblStaffNumber.Text = "Enrollee Number..";
        //    else
        //        lblStaffNumber.Text = "Company Staff No.";
        //    //lblgrouphead.Text = (mgrouphtype == "P") ? patients.name : customer.Name;
        //    txtgroupcode.Text = (mgrouphtype == "P") ? this.txtghgroupcode.Text : txtgrouphead.Text;
        //    mgenregistration = (mgrouphtype == "P") ? patients.billregistration : customer.Billregistration;

        //    ttpf01b.SetToolTip(txtbranch, (mgrouphtype == "C" && txtgrouphead.Text.Trim() == "PAN") ? "Enter Staff Grade Level" : " ");
        //    btngroupcode.Visible = mgrouphtype == "C" ? true : false;
        //    //if (mgrouphtype == "C" && "NHIS".IndexOf(txtgrouphead.Text.Trim()) >= 0)
        //    //    if (mgrouphtype == "C" && txtgrouphead.Text.Contains("NHIS"))
        //    //     txtgroupcode.Text = "NHIS";

        //    if (mgrouphtype == "C" && customer.HMO)
        //    {
        //        txtgroupcode.Text = txtgrouphead.Text.Contains("NHIS") ? "NHIS" : " ";
        //        // get plan types into combo 
        //        this.cboHmoservgrp.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT HMOSERVTYPE,DESCRIPTION FROM HMODETAIL WHERE custno = '" + customer.Custno + "'", true); // selcode.getHMOplantypecombolist("hp", customer.Custno);
        //        cboHmoservgrp.ValueMember = "HMOSERVTYPE";
        //        //Setting Combo Box DisplayMember Property
        //        cboHmoservgrp.DisplayMember = "HMOSERVTYPE + ' : ' +Description ";
        //    }
        //    else if (comgrouphtype.Text.Trim() == "PRIVATE")
        //        txtgroupcode.Text = txtghgroupcode.Text;
        //    else
        //        txtgroupcode.Text = txtgrouphead.Text;

        //    AnyCode = Anycode1 = "";
        //    txtgroupcode.Focus();

        //}

        //private void txtgroupcode_Enter(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtgrouphead.Text))
        //    {
        //        DialogResult result = MessageBox.Show("Group head must be specified...");
        //        //txtgrouphead.Select();
        //        return;
        //    }
        //}

        //private void txtgroupcode_LostFocus(object sender, EventArgs e)
        //{
        //    string gc = this.txtgroupcode.Text;
        //    if (!msmrfunc.checkGroupCode(gc)) //== false)
        //    {
        //        // this.txtgroupcode.Select();
        //        txtgroupcode.Text = "";
        //        return;
        //    }
        //    lblbranch.Text = (txtgroupcode.Text.Trim() == "NHIS") ? "Comp./Ministry" : "Branch/Section";
        //}

        //private void txtpatientno_Enter(object sender, EventArgs e)
        //{
        //    // pleaseWait.Hide();
        //    if (!chkRememberDetails.Checked)
        //        ClearControls("P");
        //    if (string.IsNullOrWhiteSpace(AnyCode)) //no lookup
        //    {
        //        if (mpauto)
        //        {
        //            mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 2, false, mlastno, false);
        //            txtpatientno.Text = mlastno.ToString();
        //            txtpatientno.Focus();
        //        }
        //    }
        //}

        //private void txtpatientno_LostFocus(object sender, EventArgs e)
        //{
        //    //if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    //{
        //    //    AnyCode = "";
        //    //    return;
        //    //}

        //    //if (!chkRememberDetails.Checked)
        //    //    ClearControls("");

        //    DialogResult result;
        //    newrec = true;
        //    if (string.IsNullOrWhiteSpace(AnyCode)) // && mpauto) //no lookup value obtained
        //    {
        //        decimal xnum = bissclass.GetNumberFromString(txtpatientno.Text);
        //        // if ( bissclass.IsDigitsOnly(txtpatientno.Text.Trim()) && Convert.ToDecimal(this.txtpatientno.Text.Trim()) > mlastno)

        //        if (xnum > mlastno)
        //        {
        //            msgeventtracker = "P";
        //            result = MessageBox.Show("Patient Number is out of Sequence...");
        //            txtpatientno.Text = mlastno.ToString();
        //            txtpatientno.Focus();
        //            return;
        //        }

        //        if (txtpatientno.Text.Trim().Length < 7)
        //        {
        //            if (!bissclass.IsDigitsOnly(txtpatientno.Text.Trim().Substring(txtpatientno.Text.Trim().Length - 1)))
        //                txtpatientno.Text = string.Concat(Enumerable.Repeat("0", 7 - txtpatientno.Text.Trim().Length)) + txtpatientno.Text.Trim();
        //            else
        //                this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");
        //        }
        //    }
        //    //check if patientno exists
        //    bchain = billchaindtl.Getbillchain(this.txtpatientno.Text, txtgroupcode.Text);
        //    if (bchain == null) //new defintion
        //    {
        //        // DialogResult result = MessageBox.Show("New Patient Records ?" + " Confirm to Add...", "New Patient Records", MessageBoxButtons.YesNo,
        //        //  MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, msgBoxHandler);
        //        //    msmrfunc.mrGlobals.waitwindowtext = "NEW PATIENT REGISTRATION";
        //        // Display form modelessly
        //        //    pleaseWait.Show();
        //        DataTable dt = Dataaccess.GetAnytable("", "MR", "select name, patientno, groupcode from billchain where patientno = '" + txtpatientno.Text + "'", false);
        //        if (dt.Rows.Count > 0)
        //        {
        //            MessageBox.Show("This Reference is already used for " + dt.Rows[0]["name"].ToString().Trim() + " GroupCode : " + dt.Rows[0]["groupcode"].ToString().Trim(), "Patient Number :" + txtpatientno.Text);
        //            AnyCode = Anycode1 = txtpatientno.Text = "";
        //            txtgroupcode.Select();
        //            // txtpatientno.Select();
        //            return;

        //        }
        //        btnsave.Enabled = mcanadd ? true : false;
        //        if (mgrouphtype == "P")
        //        {
        //            txtresidence.Text = patients.address1;
        //            txtphone.Text = patients.homephone;
        //            txtemail.Text = patients.email;
        //            txtsurname.Text = patients.surname;

        //        }
        //    }
        //    else
        //    {
        //        // msmrfunc.mrGlobals.waitwindowtext = "RECORD EXIST ...";
        //        result = MessageBox.Show("RECORD EXIST ...");
        //        // Display form modelessly
        //        //  pleaseWait.Show();
        //        newrec = false;
        //        this.btnchainedmedhistory.Enabled = true;
        //        btnsave.Enabled = mcanalter ? true : false;
        //        this.DisplayPatients();
        //        if (!bchain.POSTED)
        //            btndelete.Enabled = mcandelete ? true : false;

        //        if (bchain.GROUPHTYPE == "C")
        //            this.btnpvtacct.Enabled = true;
        //        if (string.IsNullOrWhiteSpace(bchain.SURNAME))
        //        {
        //            txtsurname.Text = bchain.NAME;
        //            txtothernames.Text = bchain.NAME;
        //            result = MessageBox.Show("Please edit content of SURNAME and OTHERNAMES columnes...", "Patient Names");
        //        }
        //    }
        //    AnyCode = "";
        //    cboTitle.Select();
        //}

        //private void txtbillonacct_Leave(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtbillonacct.Text))
        //    {
        //        AnyCode = "";
        //        return;
        //    }

        //    DialogResult result;
        //    if (string.IsNullOrWhiteSpace(AnyCode)) // && mpauto) //no lookup value obtained
        //    {
        //        this.txtbillonacct.Text = bissclass.autonumconfig(this.txtbillonacct.Text, true, "", "9999999");
        //    }

        //    //check if patientno exists
        //    billchaindtl tmpchain = billchaindtl.Getbillchain(this.txtbillonacct.Text, txtgroupcode.Text);
        //    if (tmpchain == null || tmpchain.GROUPHEAD.Trim() != this.txtgrouphead.Text.Trim())  // wrong value
        //    {
        //        result = MessageBox.Show("Invalid Patient No. or inconsistent Grouping...BILL ON ACCOUNT !");
        //        this.txtbillonacct.Text = "";
        //        this.txtbillonacct.Select();
        //        return;
        //    }
        //    else
        //    { lblbillonaccount.Text = tmpchain.NAME; }

        //    AnyCode = "";

        //}

        //private void txtothernames_Leave(object sender, EventArgs e)
        //{
        //    string xtitle = (string.IsNullOrWhiteSpace(cboTitle.Text)) ? "" : cboTitle.Text.Trim();
        //    txtname.Text = txtsurname.Text.Trim() + ", " + txtothernames.Text.Trim() + " (" + xtitle + ")";
        //    dtbirthdate.Select();
        //}

        //private void dtbirthdate_Leave(object sender, EventArgs e)
        //{
        //    if (dtbirthdate.Value > DateTime.Now)
        //    {
        //        DialogResult result = MessageBox.Show("Date of Birth cannot be greater than today...");
        //        dtbirthdate.Value = DateTime.Now.Date;
        //        dtbirthdate.Select();
        //        return;
        //    }
        //}

        //private void dtreg_date_Leave(object sender, EventArgs e)
        //{
        //    if (dtreg_date.Value > DateTime.Now)
        //    {
        //        DialogResult result = MessageBox.Show("Date Registered cannot be greater than today...");
        //        dtreg_date.Focus();
        //        return;
        //    }
        //}

        //private void txtnewexpiry_Leave(object sender, EventArgs e)
        //{
        //    if (txtnewexpiry.Value < dtreg_date.Value)
        //    {
        //        DialogResult result = MessageBox.Show("Expiry Date cannot be less than Date Registered...");
        //        dtreg_date.Select();
        //        return;
        //    }
        //}

        //private void DisplayPatients()
        //{
        //    //change comboboxstyle to allow display of field value
        //    this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown);
        //    if (string.IsNullOrWhiteSpace(txtgrouphead.Text))
        //    {
        //        txtgrouphead.Text = bchain.GROUPHEAD;
        //        if (bchain.GROUPHTYPE == "P")
        //            txtghgroupcode.Text = bchain.GHGROUPCODE;

        //        comgrouphtype.Text = bchain.GROUPHTYPE == "P" ? "PRIVATE" : "CORPORATE";

        //    }
        //    this.txtname.Text = bchain.NAME;
        //    this.txtbillonacct.Text = bchain.BILLONACCT;
        //    this.dtbirthdate.Value = bchain.BIRTHDATE;
        //    this.cboGender.Text = bchain.SEX;
        //    this.cboMaritalstatus.Text = bchain.M_STATUS;
        //    this.txtstaffno.Text = bchain.STAFFNO;
        //    this.cboRelationship.Text = bchain.RELATIONSH == "S" ? "STAFF" : bchain.RELATIONSH == "C" ? "CHILD" : bchain.RELATIONSH == "W" ? "WIFE" : bchain.RELATIONSH == "H" ? "HUSBAND" : "N/A"; // bchain.RELATIONSH;
        //    this.cboHmoservgrp.Text = bchain.HMOSERVTYPE;
        //    this.spndependants.Value = bchain.CUMVISITS;
        //    this.txtbranch.Text = bchain.SECTION;
        //    this.txtdepartment.Text = bchain.DEPARTMENT;
        //    this.txtphone.Text = bchain.PHONE;
        //    this.txtemail.Text = bchain.EMAIL;
        //    this.txtresidence.Text = bchain.RESIDENCE;
        //    this.dtreg_date.Value = bchain.REG_DATE;
        //    this.txtnewexpiry.Value = bchain.EXPIRYDATE;
        //    this.cboCurrency.Text = bchain.CURRENCY;
        //    this.cboClinic.Text = bchain.CLINIC;
        //    txtsurname.Text = string.IsNullOrWhiteSpace(bchain.SURNAME) ? bchain.NAME : bchain.SURNAME;
        //    txtothernames.Text = bchain.OTHERNAMES;
        //    cboTitle.Text = bchain.TITLE;
        //    //pictureBox1.Image = bchain.PICLOCATION;
        //    mHMOCODE = bchain.HMOCODE;
        //    //  mgrouphtype = bchain.GROUPHTYPE;
        //    spnotes = bchain.SPNOTES;
        //    mednotes = bchain.MEDNOTES;
        //    PicSelected = bchain.PICLOCATION;
        //    if (!string.IsNullOrWhiteSpace(PicSelected))
        //    { pictureBox1.Image = WebGUIGatway.getpicture(PicSelected); }
        //    //string filename = System.IO.Path.GetFileName(PicSelected);
        //    //ImageResourceHandle imageResourceHandlePic1 = new ImageResourceHandle(filename);
        //    //pictureBox1.Image = imageResourceHandlePic1;

        //    //revert to its original format
        //    this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList);
        //    string xghname = msmrfunc.GETGroupheadname(bchain.GHGROUPCODE, bchain.GROUPHEAD, bchain.GROUPHTYPE);
        //    if (bchain.GROUPHEAD.Trim() != txtgrouphead.Text.Trim())
        //        lblcurrentgrouphead.Text = "Current Grouphead : " + xghname;
        //    else
        //    {
        //        lblcurrentgrouphead.Text = "";
        //        lblgrouphead.Text = xghname;
        //    }
        //}

        public MR_DATA.MR_DATAvm btnsave_Click()
        {
            //validate fields
            //msmrfunc.mrGlobals.iserror = false;

            //if (!bissclass.IsPresent(this.txtgroupcode, "Patients Groupcode", false) ||
            //    !bissclass.IsPresent(this.txtpatientno, "Patient Number", false) ||
            //    !bissclass.IsPresent(txtsurname, "Patients Surname", false) ||
            //    !bissclass.IsPresent(txtothernames, "Patient's other names", false) ||
            //    mgrouphtype == "C" && !bissclass.IsPresent(this.txtstaffno, "Patient staff / Enrollee Number", false) ||
            //    mgrouphtype == "C" && customer.HMO && !bissclass.IsPresent(cboHmoservgrp, "Patient HMO PlanType", false) ||
            //    !bissclass.IsPresent(this.comgrouphtype, "Patient's Group Type", false) &&
            //    !bissclass.IsPresent(this.txtgrouphead, "Bills Payable By", false) ||
            //    !bissclass.IsPresent(cboRelationship, "Staff Relationship Id", false) ||
            //    !bissclass.IsPresent(this.txtname, "Patient's Full Name", false))
            //    return;

            //DialogResult result;
            //if (dtbirthdate.Value.Date > DateTime.Now.Date)
            //{
            //    MessageBox.Show("Date of Birth Error... Pls Check!");
            //    return;
            //}

            //if (txtname.Text.Substring(0, 1) == ",")
            //{
            //    MessageBox.Show("Patient Name Error... Pls Check! " + txtname.Text);
            //    return;
            //}

            //msgeventtracker = "PS";
            //if (newrec && !mcanadd)
            //{
            //    MessageBox.Show("ACCESS DENIED...To New Record Creation. See your Systems Administrator!");
            //    btnclose.Select();
            //    return;
            //}

            //if (must_patphoto && string.IsNullOrWhiteSpace(PicSelected))
            //{
            //    MessageBox.Show("Patient Photo Must be Captured and attached... RECORD NOT SAVED !");
            //    cboGetPicture.Select();
            //    return;
            //}

            if (mgrouphtype == "C" && vm.REPORTS.txtrelationship.Trim() != "STAFF" && customer.BILLSBYGC && string.IsNullOrWhiteSpace(vm.REPORTS.REPORT_TYPE4))
            {
                vm.REPORTS.alertMessage = "Printical (Staff of company Card Number) Must be specified... RECORD NOT SAVED !";
                return vm;
            }

            //if (mgrouphtype == "C" && customer.HMO && string.IsNullOrWhiteSpace(cboHmoservgrp.Text))
            //{
            //    MessageBox.Show("Patient's HMO PlanType must be specified...");
            //    return;
            //}

            //    save records

            //    pleaseWait.Hide();

            //vm.REPORTS.ActRslt = "Confirm to Save...", "Patient Records", MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, msgBoxHandler);

            //if (result == DialogResult.No)
            //    return;

            //decimal xn = Convert.ToDecimal(txtpatientno.Text.Trim());

            patients = patientinfo.GetPatient(vm.REPORTS.txtbillspayable, vm.REPORTS.txtgroupcode);

            newrec = patients == null ? true : false;

            if (newrec && bissclass.IsDigitsOnly(vm.REPORTS.txtpatientno.Trim()) && Convert.ToDecimal(vm.REPORTS.txtpatientno.Trim().Trim()) >= mlastno)
            {
                decimal lastnosave = mlastno;
                mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 2, true, mlastno, false);
                if (mlastno != lastnosave)
                    vm.REPORTS.REPORT_TYPE3 = bissclass.autonumconfig(mlastno.ToString(), true, "", "9999999"); //for patient no.
            }

            savepatientdetails();
            vm.REPORTS.cmbsave = false;
            //txtpatientno.Focus();

            return vm;
        }

        void savepatientdetails()
        {

            bchain = billchaindtl.Getbillchain(vm.REPORTS.txtbillspayable.Trim(), vm.REPORTS.txtgroupcode.Trim());

            newrec = (bchain == null || bchain.NAME == null) ? true : false;

            // patientinfo patients = new patientinfo();
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "Billchain_Add" : "Billchain_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("GROUPCODE", vm.REPORTS.txtgroupcode);
            insertCommand.Parameters.AddWithValue("PATIENTNO", vm.REPORTS.txtpatientno);
            insertCommand.Parameters.AddWithValue("GROUPHEAD", vm.REPORTS.txtbillspayable);
            insertCommand.Parameters.AddWithValue("NAME", vm.REPORTS.TXTPATIENTNAME);
            insertCommand.Parameters.AddWithValue("REG_DATE", vm.REPORTS.dtregistered);
            insertCommand.Parameters.AddWithValue("POSTED", newrec ? false : bchain.POSTED);
            insertCommand.Parameters.AddWithValue("POST_DATE", newrec ? DateTime.Now : bchain.POST_DATE);
            insertCommand.Parameters.AddWithValue("GROUPHTYPE", mgrouphtype);
            insertCommand.Parameters.AddWithValue("STAFFNO", vm.REPORTS.txtstaffno);
            insertCommand.Parameters.AddWithValue("RELATIONSH", vm.REPORTS.txtrelationship);
            insertCommand.Parameters.AddWithValue("SECTION", vm.REPORTS.txtbranch);
            insertCommand.Parameters.AddWithValue("DEPARTMENT", vm.REPORTS.txtdepartment);
            insertCommand.Parameters.AddWithValue("CUR_DB", newrec ? 0m : bchain.CUR_DB);
            insertCommand.Parameters.AddWithValue("STATUS", newrec ? "A" : bchain.STATUS);
            insertCommand.Parameters.AddWithValue("SEX", vm.REPORTS.cbogender);
            insertCommand.Parameters.AddWithValue("M_STATUS", vm.REPORTS.cbomaritalstatus);
            insertCommand.Parameters.AddWithValue("BIRTHDATE", vm.REPORTS.dtbirthdate);
            insertCommand.Parameters.AddWithValue("CUMVISITS", newrec ? 0 : bchain.CUMVISITS);
            insertCommand.Parameters.AddWithValue("HMOCODE", mgrouphtype == "C" && customer.HMO ? customer.Custno : "");
            insertCommand.Parameters.AddWithValue("PATCATEG", mpatcateg);
            insertCommand.Parameters.AddWithValue("RESIDENCE", vm.REPORTS.txtaddress1);
            insertCommand.Parameters.AddWithValue("GHGROUPCODE", vm.REPORTS.txtghgroupcode);
            insertCommand.Parameters.AddWithValue("HMOSERVTYPE", vm.REPORTS.comhmoservgrp);
            insertCommand.Parameters.AddWithValue("BILLONACCT", vm.REPORTS.txtbillonacct); 
            insertCommand.Parameters.AddWithValue("CURRENCY", vm.SYSCODETABSvm.CurrencyCodes.name);
            insertCommand.Parameters.AddWithValue("OPERATOR", woperator);
            insertCommand.Parameters.AddWithValue("DTIME", DateTime.Now);
            //insertCommand.Parameters.AddWithValue("EXPIRYDATE", txtnewexpiry.Value);
            insertCommand.Parameters.AddWithValue("ASTNOTES", 0);
            insertCommand.Parameters.AddWithValue("CLINIC", vm.SYSCODETABSvm.ServiceCentreCodes.name);
            insertCommand.Parameters.AddWithValue("PHONE", vm.REPORTS.txthomephone);
            insertCommand.Parameters.AddWithValue("EMAIL", vm.REPORTS.txtemail);
            insertCommand.Parameters.AddWithValue("PICLOCATION", PicSelected);
            insertCommand.Parameters.AddWithValue("SURNAME", vm.REPORTS.txtsurname);
            insertCommand.Parameters.AddWithValue("OTHERNAMES", vm.REPORTS.txtothername);
            insertCommand.Parameters.AddWithValue("TITLE", vm.REPORTS.cbotitle);
            insertCommand.Parameters.AddWithValue("SPNOTES", spnotes); // newrec ? "" : bchain.SPNOTES);
            insertCommand.Parameters.AddWithValue("MEDNOTES", mednotes); //  newrec ? "" : bchain.MEDNOTES);
            insertCommand.Parameters.AddWithValue("MEDHISTORYCHAINED", newrec ? false : bchain.MEDHISTORYCHAINED);
            insertCommand.Parameters.AddWithValue("PATIENTNO_PRINCIPAL", vm.REPORTS.REPORT_TYPE4);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                vm.REPORTS.alertMessage = "Successfull";
                //txtpatientno.Text = AnyCode = "";
            }
            catch (SqlException ex)
            {
                errorProp = "" + ex.Message + "Add Group Details";
                return;
            }
            finally
            {
                connection.Close();
            }
        }

        public MR_DATA.MR_DATAvm btndelete_Click(string billsPayableBy, string patientGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            billchaindtl bchain = billchaindtl.Getbillchain(billsPayableBy, patientGroupCode);

            if (bchain.POSTED)
            {
                vm.REPORTS.alertMessage = "Record Can't be Deleted...";
                return vm;
            }

            //msgeventtracker = "PD";
            //DialogResult result = MessageBox.Show("Confirm to Delete Record", "GroupMembers Registeration", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            if (bchain.PATIENTNO == bchain.GROUPHEAD && bchain.GROUPCODE == bchain.GHGROUPCODE)
            {
                DataTable dt = Dataaccess.GetAnytable("", "MR", "select name from billchain where ghgroupcode = '" + bchain.GHGROUPCODE + "' AND GROUPHEAD = '" + bchain.GROUPHEAD + "'", false);
                if (dt.Rows.Count > 1)
                {
                    vm.REPORTS.alertMessage = "This Record cannot be deleted... There are Records that are Grouped on it";
                    return vm;
                }
                string updstr = "delete from patient where groupcode = '" + bchain.GROUPCODE + "' and patientno = '" + bchain.PATIENTNO + "'";
                bissclass.UpdateRecords(updstr, "MR");
            }
            billchaindtl.DeleteBillchain(billsPayableBy, patientGroupCode);

            vm.REPORTS.alertMessage = "Record Deleted Successfully...";

            //this.ClearControls("D");
            //this.txtpatientno.Text = "";
            //this.txtgroupcode.Focus();
            return vm;
        }

        void getAutoCompleteDetails(string xfile, string xfield1, string xfield2) //, 
        {
            SqlConnection cs = Dataaccess.codeConnection();
            SqlCommand selectCommand = new SqlCommand("SELECT CONCAT(surname,othernames,title) as NameswitTitle FROM patient", cs);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(selectCommand);
            da.Fill(ds, "PatientList");
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            int i = 0; for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            { col.Add(ds.Tables[0].Rows[i]["NamewithTitle"].ToString()); }
            // col.Add(ds.Tables[0].Rows[i]["phone_no"].ToString()); } 
            // System.Windows.Forms.AutoCompleteMode.SuggestAppend

            //             txtname.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //              txtname.AutoCompleteCustomSource = col; 
            //               txtname.AutoCompleteMode = AutoCompleteMode.Suggest;
            cs.Close();

        }

        //private void btnpvtacct_Click(object sender, EventArgs e)
        //{
        //    if (bchain == null)
        //        return;
        //    frmPrivateAcct pvtacct = new frmPrivateAcct(bchain, woperator);
        //    pvtacct.ShowDialog();
        //}

        //private void btnchainedmedhistory_Click(object sender, EventArgs e)
        //{
        //    frmChainMedicalHistory chainmedhist = new frmChainMedicalHistory(bchain.GROUPCODE, bchain.PATIENTNO, bchain.NAME);
        //    chainmedhist.ShowDialog();
        //}

        //private void comrelationship_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(cboRelationship.Text) && cboRelationship.SelectedItem != null)
        //        cboRelationship.Text = cboRelationship.SelectedItem.ToString();

        //    if (cboRelationship.Text.Trim() != "STAFF")
        //        btnPrincipal.Enabled = txtPrinciapl.Enabled = true;
        //    else
        //        btnPrincipal.Enabled = txtPrinciapl.Enabled = false;

        //}

        //private void btnspnotes_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    string caption = btn.Name == "btnMednotes" ? "MEDICAL NOTES - Allergies, Special Instructions, etc." : "SPECIAL INSTRUCTIONS / ADMINISTRATIVE NOTES For this Patient";
        //    string xnote = "";
        //    if (btn.Name == "btnspnotes" && newrec)
        //        xnote = bchain.SPNOTES;
        //    else if (btn.Name == "btnMednotes" && newrec)
        //        xnote = bchain.MEDNOTES;

        //    NOTES notes = new NOTES(caption, "", xnote, false);
        //    notes.ShowDialog();
        //    if (btn.Name == "btnspnotes")
        //        spnotes = string.IsNullOrWhiteSpace(msmrfunc.mrGlobals.rtnstringNotes) ? spnotes : msmrfunc.mrGlobals.rtnstringNotes;
        //    else
        //        mednotes = string.IsNullOrWhiteSpace(msmrfunc.mrGlobals.rtnstringNotes) ? mednotes : msmrfunc.mrGlobals.rtnstringNotes;
        //}

        //private void txtPrinciapl_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtPrinciapl.Text))
        //    {
        //        AnyCode = "";
        //        return;
        //    }
        //    DialogResult result;
        //    if (string.IsNullOrWhiteSpace(AnyCode)) // && mpauto) //no lookup value obtained
        //    {
        //        if (bissclass.IsDigitsOnly(txtPrinciapl.Text.Trim()))
        //            this.txtpatientno.Text = bissclass.autonumconfig(this.txtPrinciapl.Text, true, "", "9999999");
        //    }
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select name from billchain where patientno = '" + txtPrinciapl.Text + "'", false);
        //    if (dt.Rows.Count < 1)
        //    {
        //        result = MessageBox.Show("Invalid Patient Number as Principal...");
        //        txtPrinciapl.Text = AnyCode = "";
        //        return;
        //    }
        //    lblPrincipal.Text = dt.Rows[0]["name"].ToString();
        //}

        //private void cboClinic_LostFocus(object sender, EventArgs e)
        //{
        //    btnsave.Focus();
        //}

        //private void cboTitle_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ComboBox cbo = new ComboBox();
        //    if (string.IsNullOrWhiteSpace(cbo.Text) && cbo.SelectedItem != null)
        //        cbo.Text = cbo.SelectedItem.ToString();
        //}

        //private void txtghgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtgrouphead_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtgrouphead_LostFocus(null, null);
        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtgroupcode_LostFocus(null, null);
        //}

        //private void txtpatientno_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtpatientno_LostFocus(null, null);
        //}

        //private void txtbillonacct_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtbillonacct_Leave(null, null);
        //}

        //private void txtsurname_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtothernames_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtothernames_Leave(null, null);
        //}

        //private void txtPrinciapl_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtPrinciapl_LostFocus(null, null);
        //}

        //private void txtgrouphead_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.F2)
        //    {
        //        btngrouphead.PerformClick();
        //    }
        //}

        //private void btnFurtherDetails_Click(object sender, EventArgs e)  ################
        //{
        //    if (string.IsNullOrWhiteSpace(txtpatientno.Text) || string.IsNullOrWhiteSpace(txtgroupcode.Text) || string.IsNullOrWhiteSpace(txtname.Text))
        //    {
        //        MessageBox.Show("Patient Information must be called up...");
        //        return;
        //    }

        //    frmGroupFurtherDetails grpdetails = new frmGroupFurtherDetails(txtgroupcode.Text, txtpatientno.Text);
        //    grpdetails.Show();
        //}

    }
}