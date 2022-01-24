#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;

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
    public partial class frmImageAcquisition
    {
        billchaindtl bchain = new billchaindtl();
        Customer customers = new Customer();
        patientinfo patients = new patientinfo();

        DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), medhpic,
            dtselected = Dataaccess.GetAnytable("", "MR",
                "SELECT medhpic.groupcode+':'+medhpic.patientno+' '+convert(char,medhpic.trans_date) AS reference, medhpic.groupcode,medhpic.patientno,medhpic.trans_date,billchain.name from medhpic LEFT JOIN BILLCHAIN on medhpic.groupcode = billchain.groupcode and medhpic.patientno = billchain.patientno order by name", true);

        string picselected, lookupsource, AnyCode, Anycode1, woperator, txtImageLocation;
        int picsCounter, pdfCounter, recid;
        bool ispdf, newrec;
        string[,] imagenotes = new string[10, 4]; string[] pdfNotes = new string[10];
        //imagenotes 0-notes,1-facility,2-recid,3-imagefile

        MR_DATA.MR_DATAvm vm;

        public frmImageAcquisition(string woperato, MR_DATA.MR_DATAvm VM2)
        {
            //InitializeComponent();
            woperator = woperato;
            vm = VM2;
        }

        //private void frmImageAcquisition_Load(object sender, EventArgs e)
        //{
        //    //initcomboboxes();
        //    picsCounter = pdfCounter = 0;
        //    for (int i = 0; i < 10; i++)
        //    {
        //        imagenotes[i, 0] = imagenotes[i, 1] = imagenotes[i, 2] = imagenotes[i, 3] = "";
        //        pdfNotes[i] = "";
        //    }
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select authsign from mrsetup where recid = '10'", false);
        //    txtImageLocation = dt.Rows[0]["authsign"].ToString();
        //    /*  if (string.IsNullOrWhiteSpace(txtImageLocation))
        //      {
        //          DialogResult result = MessageBox.Show("Image Location has not been defined in Systems setup...", "Systems Setup Error");
        //          btnClose.PerformClick();
        //      }*/
        //}

        //private void initcomboboxes()
        //{
        //	//get clinic
        //	combfacility.DataSource = dtfacility;
        //	combfacility.ValueMember = "Type_code";
        //	combfacility.DisplayMember = "name";
        //	//referring Docs
        //	cboQueryPrevDef.DataSource = dtselected; // Dataaccess.GetAnytable("", "MR", "SELECT groupcode+':'+patientno+' '+convert(char,trans_date) AS reference from medhpic order by reference", true);
        //	cboQueryPrevDef.ValueMember = "REFERENCE";
        //	cboQueryPrevDef.DisplayMember = "reference";
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btngroupcode")
        //    {
        //        txtgroupcode.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "g";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnpatientlookup")
        //    {
        //        txtPatientNo.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    msmrfunc.mrGlobals.lookupCriteria = "";
        //    if (lookupsource == "g") //groupcodee
        //    {
        //        txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtPatientNo.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //        txtgroupcode.Select();
        //    }
        //    else if (lookupsource == "L") //patientno
        //    {
        //        txtPatientNo.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtPatientNo.Select();
        //    }
        //    return;
        //}

        //private void cboQueryPrevDef_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(cboQueryPrevDef.Text))
        //        return;
        //    loadpreviousDefinitions();
        //}

        //void loadpreviousDefinitions()
        //{
        //    newrec = true;
        //    if (!string.IsNullOrWhiteSpace(cboQueryPrevDef.Text))
        //    {
        //        int xsel = cboQueryPrevDef.SelectedIndex;
        //        DataRow rowsel = dtselected.Rows[xsel];
        //        txtgroupcode.Text = rowsel["groupcode"].ToString();
        //        txtPatientNo.Text = rowsel["patientno"].ToString();
        //        dtTrans_date.Value = Convert.ToDateTime(rowsel["trans_date"]).Date;
        //    }

        //    medhpic = Dataaccess.GetAnytable("", "MR", "select * from medhpic where groupcode = '" + txtgroupcode.Text + "' and patientno = '" + txtPatientNo.Text + "' and trans_date = '" + dtTrans_date.Value.ToShortDateString() + "'", false);
        //    if (medhpic.Rows.Count < 1)
        //        return;
        //    newrec = false;
        //    recid = Convert.ToInt32(medhpic.Rows[0]["recid"]);
        //    DataRow row = medhpic.Rows[0];
        //    for (int i = 0; i < 10; i++) //images
        //    {
        //        imagenotes[i, 0] = row["note" + (i + 1).ToString()].ToString();
        //        imagenotes[i, 1] = row["facility" + (i + 1).ToString()].ToString();
        //        imagenotes[i, 2] = row["recid"].ToString();
        //        imagenotes[i, 3] = row["pic" + (i + 1).ToString()].ToString();
        //        if (!string.IsNullOrWhiteSpace(imagenotes[i, 3]))
        //            picsCounter++;
        //    }
        //    if (!string.IsNullOrWhiteSpace(row["pic1"].ToString()))
        //        pictureBox1.Image = WebGUIGatway.getpicture(row["pic1"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic2"].ToString()))
        //        pictureBox2.Image = WebGUIGatway.getpicture(row["pic2"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic3"].ToString()))
        //        pictureBox3.Image = WebGUIGatway.getpicture(row["pic3"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic4"].ToString()))
        //        pictureBox4.Image = WebGUIGatway.getpicture(row["pic4"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic5"].ToString()))
        //        pictureBox5.Image = WebGUIGatway.getpicture(row["pic5"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic6"].ToString()))
        //        pictureBox6.Image = WebGUIGatway.getpicture(row["pic6"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic7"].ToString()))
        //        pictureBox7.Image = WebGUIGatway.getpicture(row["pic7"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic8"].ToString()))
        //        pictureBox8.Image = WebGUIGatway.getpicture(row["pic8"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic9"].ToString()))
        //        pictureBox9.Image = WebGUIGatway.getpicture(row["pic9"].ToString().Trim());
        //    if (!string.IsNullOrWhiteSpace(row["pic10"].ToString()))
        //        pictureBox10.Image = WebGUIGatway.getpicture(row["pic10"].ToString().Trim());

        //    txtImage1.Text = row["pic1"].ToString().Trim();
        //    txtImage2.Text = row["pic2"].ToString().Trim();
        //    txtImage3.Text = row["pic3"].ToString().Trim();
        //    txtImage4.Text = row["pic4"].ToString().Trim();
        //    txtImage5.Text = row["pic5"].ToString().Trim();
        //    txtImage6.Text = row["pic6"].ToString().Trim();
        //    txtImage7.Text = row["pic7"].ToString().Trim();
        //    txtImage8.Text = row["pic8"].ToString().Trim();
        //    txtImage9.Text = row["pic9"].ToString().Trim();
        //    txtImage10.Text = row["pic10"].ToString().Trim();

        //    for (int i = 0; i < 9; i++) //pdf
        //    {
        //        pdfNotes[i] = row["pdffile" + (i + 1).ToString()].ToString().Trim();
        //        if (!string.IsNullOrWhiteSpace(pdfNotes[i]))
        //            pdfCounter++;
        //    }
        //    txtPDF1.Text = row["pdffile1"].ToString().Trim();
        //    txtPDF2.Text = row["pdffile2"].ToString().Trim();
        //    txtPDF3.Text = row["pdffile3"].ToString().Trim();
        //    txtPDF4.Text = row["pdffile4"].ToString().Trim();
        //    txtPDF5.Text = row["pdffile5"].ToString().Trim();
        //    txtPDF6.Text = row["pdffile6"].ToString().Trim();
        //    txtPDF7.Text = row["pdffile7"].ToString().Trim();
        //    txtPDF8.Text = row["pdffile8"].ToString().Trim();
        //    txtPDF9.Text = row["pdffile9"].ToString().Trim();

        //}

        //private void txtPatientNo_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtPatientNo.Text))
        //    {
        //        AnyCode = Anycode1 = "";
        //        return;
        //    }

        //    if (string.IsNullOrWhiteSpace(AnyCode))  //no lookup value obtained
        //    {
        //        txtPatientNo.Text = bissclass.autonumconfig(txtPatientNo.Text, true, "", "9999999");
        //    }

        //    //check if patientno exists
        //    bchain = billchaindtl.Getbillchain(txtPatientNo.Text, txtgroupcode.Text);
        //    if (bchain == null)
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Patient Number... ");
        //        txtPatientNo.Text = " ";
        //        return;
        //    }

        //    txtName.Text = bchain.NAME;
        //    txtGrouphead.Text = msmrfunc.GETGroupheadname(bchain.GHGROUPCODE, bchain.GROUPHEAD, bchain.GROUPHTYPE);
        //}

        //     private void btnLoad_Click(object sender, EventArgs e)
        //     {
        //         Button btn = sender as Button;
        //         if (string.IsNullOrWhiteSpace(txtgroupcode.Text) || string.IsNullOrWhiteSpace(txtPatientNo.Text) || string.IsNullOrWhiteSpace(combfacility.Text))
        //         {
        //             DialogResult result = MessageBox.Show("Valid Patient Number, Service Centre/Facility must be specified...");
        //             return;
        //         }
        //         ispdf = btn.Name == "btnLoadPDF" ? true : false;
        //         if (!ispdf && picsCounter > 10 || ispdf && pdfCounter > 9)
        //         {
        //             DialogResult result = MessageBox.Show("Maximum Image Loadable per Treatment Date is 10");
        //             return;
        //         }
        //         var codecs = ImageCodecInfo.GetImageEncoders();
        //         var codecFilter = "Image Files|";
        //         foreach (var codec in codecs)
        //         {
        //             codecFilter += codec.FilenameExtension + ";";
        //         }
        //         // dialog.Filter = codecFilter;

        //         OpenFileDialog openFileDialog1 = new OpenFileDialog();
        //         openFileDialog1.SupportMultiDottedExtensions = true;
        //         /*openFileDialog1.Title = ispdf ? "Open PDF Files" : "Open bitmap or jpeg.";
        //openFileDialog1.Filter = ispdf ? "PDF files |*.pdf" : codecFilter;*/
        //         openFileDialog1.Title = "Open bitmap or jpeg.";
        //         //openFileDialog1.Filter = ispdf ? "PDF files |*.pdf" : codecFilter;

        //         //"BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|"
        //         //+ "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";

        //         //"BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";

        //         //"JPG (*.jpg,*.jpeg)|*.jpg;*.jpeg|TIFF (*.tif,*.tiff)|*.tif;*.tiff| BMP (*.bmp,*.BMP); *.bmp;*.BMP";
        //         //"BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff| All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";

        //         //"jpg files (*.jpg); *.jpg; *.* | bmp files (*.bmp); *.bmp";
        //         openFileDialog1.Closed += openFileDialog1_Closed;
        //         openFileDialog1.ShowDialog();
        //     }

        //void openFileDialog1_Closed(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog1 = sender as OpenFileDialog;
        //    if (openFileDialog1.Files.Count > 0)
        //    {
        //        Gizmox.WebGUI.Common.Resources.HttpPostedFileHandle file = (Gizmox.WebGUI.Common.Resources.HttpPostedFileHandle)openFileDialog1.Files[0];
        //        string filename = System.IO.Path.GetFileName(file.PostedFileName);
        //        string savepath = VWGContext.Current.Server.MapPath("~/Resources/Images/" + filename);
        //        // string savepath = txtImageLocation + "\\" + filename; // VWGContext.Current.Server.MapPath("~/Resources/Images/" + filename);

        //        picselected = savepath;
        //        Gizmox.WebGUI.Common.Resources.ImageResourceHandle imageResourceHandlePic1 = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle();

        //        // if (clsMain.FileExists(savepath)) clsMain.FileDelete(savepath);
        //        file.SaveAs(savepath);
        //        file.Close();
        //        file.Dispose();
        //        imageResourceHandlePic1.File = filename;
        //        if (!ispdf)
        //        {
        //            //string xpic = "pics"+picsCounter.ToString().Trim();
        //            //PictureBox pics = new PictureBox();
        //            picsCounter++;
        //            imagenotes[picsCounter - 1, 3] = savepath;
        //            if (picsCounter == 1)
        //            {
        //                pictureBox1.Image = pictureBox1.Image = WebGUIGatway.getpicture(picselected);
        //                txtImage1.Text = savepath;
        //            }
        //            if (picsCounter == 2)
        //            {
        //                pictureBox2.Image = imageResourceHandlePic1;
        //                txtImage2.Text = savepath;
        //            }
        //            if (picsCounter == 3)
        //            {
        //                pictureBox3.Image = imageResourceHandlePic1;
        //                txtImage3.Text = savepath;
        //            }
        //            if (picsCounter == 4)
        //            {
        //                pictureBox4.Image = imageResourceHandlePic1;
        //                txtImage4.Text = savepath;
        //            }
        //            if (picsCounter == 5)
        //            {
        //                pictureBox5.Image = imageResourceHandlePic1;
        //                txtImage5.Text = savepath;
        //            }
        //            if (picsCounter == 6)
        //            {
        //                pictureBox6.Image = pictureBox6.Image = WebGUIGatway.getpicture(picselected);
        //                txtImage6.Text = savepath;
        //            }
        //            if (picsCounter == 7)
        //            {
        //                pictureBox7.Image = pictureBox7.Image = WebGUIGatway.getpicture(picselected);
        //                txtImage7.Text = savepath;
        //            }
        //            if (picsCounter == 8)
        //            {
        //                pictureBox8.Image = pictureBox8.Image = WebGUIGatway.getpicture(picselected);
        //                txtImage8.Text = savepath;
        //            }
        //            if (picsCounter == 9)
        //            {
        //                pictureBox9.Image = pictureBox9.Image = WebGUIGatway.getpicture(picselected);
        //                txtImage9.Text = savepath;
        //            }
        //            if (picsCounter == 10)
        //            {
        //                pictureBox10.Image = pictureBox10.Image = WebGUIGatway.getpicture(picselected);
        //                txtImage10.Text = savepath;
        //            }
        //        }
        //        else
        //        {
        //            pdfCounter++;
        //            pdfNotes[pdfCounter - 1] = savepath;
        //            if (pdfCounter == 1)
        //                txtPDF1.Text = savepath;
        //            if (pdfCounter == 2)
        //                txtPDF2.Text = savepath;
        //            if (pdfCounter == 3)
        //                txtPDF3.Text = savepath;
        //            if (pdfCounter == 4)
        //                txtPDF4.Text = savepath;
        //            if (pdfCounter == 5)
        //                txtPDF5.Text = savepath;
        //            if (pdfCounter == 6)
        //                txtPDF6.Text = savepath;
        //            if (pdfCounter == 7)
        //                txtPDF7.Text = savepath;
        //            if (pdfCounter == 8)
        //                txtPDF8.Text = savepath;
        //            if (pdfCounter == 9)
        //                txtPDF9.Text = savepath;
        //        }
        //        btnSave.Enabled = true;
        //    }
        //}

        //private void btnClear_Click(object sender, EventArgs e)
        //{
        //    DialogResult result = MessageBox.Show("Confirm to Clear ALL Selections...", "Image Acquisition", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.Yes)
        //    {
        //        txtImage1.Text = txtImage2.Text = txtImage3.Text = txtImage4.Text = txtImage5.Text = txtPDF1.Text = txtPDF2.Text = txtPDF3.Text = txtPDF4.Text = txtPDF5.Text = txtImage6.Text = txtImage7.Text = txtImage8.Text = txtImage9.Text = txtImage10.Text = txtPDF6.Text = txtPDF7.Text = txtPDF8.Text = txtPDF9.Text = "";
        //        pictureBox1.Image = pictureBox2.Image = pictureBox3.Image = pictureBox4.Image = pictureBox5.Image = pictureBox6.Image = pictureBox7.Image = pictureBox8.Image = pictureBox9.Image = pictureBox10.Image = pictureBox12.Image = "";
        //        txtSelectedImage.Text = txtSelectedImagedate.Text = txtNotes.Text = "";

        //    }
        //}

        public MR_DATA.REPORTS btnSave_Click()
        {
            //if (!bissclass.IsPresent(combfacility, "Facility/Service Centre", true) ||
            //    !bissclass.IsPresent(txtPatientNo, "Patient Number", false))
            //    return;

            //DialogResult result = MessageBox.Show("Confirm to Submit ALL Selections...", "Image Acquisition", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            
            DataTable medhpic = Dataaccess.GetAnytable("", "MR",
               "select * from medhpic where groupcode = '" + vm.REPORTS.txtgroupcode + "' and patientno = '" + 
               vm.REPORTS.txtpatientno + "' and trans_date = '" + vm.REPORTS.txtTimeFrom + "'", false);

            newrec = false;

            if (medhpic.Rows.Count < 1)
                newrec = true; 

            DataRow row = medhpic.Rows[0];

            for (int i = 0; i < 10; i++) //images
            {
                imagenotes[i, 0] = row["note" + (i + 1).ToString()].ToString();
                imagenotes[i, 1] = row["facility" + (i + 1).ToString()].ToString();
                imagenotes[i, 2] = row["recid"].ToString();
                imagenotes[i, 3] = row["pic" + (i + 1).ToString()].ToString();
            }

            for (int i = 0; i < 9; i++) //pdf
            {
                pdfNotes[i] = row["pdffile" + (i + 1).ToString()].ToString().Trim();
            }

            var trandate = Convert.ToDateTime(vm.REPORTS.txtTimeFrom);

            //btnSave.Enabled = false;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "Medhpic_Add" : "Medhpic_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@PATIENTNO", vm.REPORTS.txtpatientno);
            insertCommand.Parameters.AddWithValue("@TRANS_DATE", trandate.Date);
            insertCommand.Parameters.AddWithValue("@POSTED", false);
            insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@GROUPCODE", vm.REPORTS.txtgroupcode);
            insertCommand.Parameters.AddWithValue("@PIC1", vm.REPORTS.txtImage1 == null ? "" : vm.REPORTS.txtImage1);
            insertCommand.Parameters.AddWithValue("@NOTE1", imagenotes[0, 0]);
            insertCommand.Parameters.AddWithValue("@PIC2", vm.REPORTS.txtImage2 == null ? "" : vm.REPORTS.txtImage2);
            insertCommand.Parameters.AddWithValue("@NOTE2", imagenotes[1, 0]);
            insertCommand.Parameters.AddWithValue("@PIC3", vm.REPORTS.txtImage3 == null ? "" : vm.REPORTS.txtImage3);
            insertCommand.Parameters.AddWithValue("@NOTE3", imagenotes[2, 0]);
            insertCommand.Parameters.AddWithValue("@PIC4", vm.REPORTS.txtImage4 == null ? "" : vm.REPORTS.txtImage4);
            insertCommand.Parameters.AddWithValue("@NOTE4", imagenotes[3, 0]);
            insertCommand.Parameters.AddWithValue("@PIC5", vm.REPORTS.txtImage5 == null ? "" : vm.REPORTS.txtImage5);
            insertCommand.Parameters.AddWithValue("@NOTE5", imagenotes[4, 0]);
            insertCommand.Parameters.AddWithValue("@PIC6", vm.REPORTS.txtImage6 == null ? "" : vm.REPORTS.txtImage6);
            insertCommand.Parameters.AddWithValue("@NOTE6", imagenotes[5, 0]);
            insertCommand.Parameters.AddWithValue("@PIC7", vm.REPORTS.txtImage7 == null ? "" : vm.REPORTS.txtImage7);
            insertCommand.Parameters.AddWithValue("@NOTE7", imagenotes[6, 0]);
            insertCommand.Parameters.AddWithValue("@PIC8", vm.REPORTS.txtImage8 == null ? "" : vm.REPORTS.txtImage8);
            insertCommand.Parameters.AddWithValue("@NOTE8", imagenotes[7, 0]);
            insertCommand.Parameters.AddWithValue("@PIC9", vm.REPORTS.txtImage9 == null ? "" : vm.REPORTS.txtImage9);
            insertCommand.Parameters.AddWithValue("@NOTE9", imagenotes[8, 0]);
            insertCommand.Parameters.AddWithValue("@PIC10", vm.REPORTS.txtImage10 == null ? "" : vm.REPORTS.txtImage10);
            insertCommand.Parameters.AddWithValue("@NOTE10", imagenotes[9, 0]);
            insertCommand.Parameters.AddWithValue("@TOTPIC", 0);
            insertCommand.Parameters.AddWithValue("@FACILITY1", imagenotes[0, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY2", imagenotes[1, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY3", imagenotes[2, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY4", imagenotes[3, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY5", imagenotes[4, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY6", imagenotes[5, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY7", imagenotes[6, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY8", imagenotes[7, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY9", imagenotes[8, 1]);
            insertCommand.Parameters.AddWithValue("@FACILITY10", imagenotes[9, 1]);
            insertCommand.Parameters.AddWithValue("@OPERATOR", woperator);
            insertCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@pdffile1", vm.REPORTS.txtPDF1 == null ? "" : vm.REPORTS.txtPDF1);
            insertCommand.Parameters.AddWithValue("@pdffile2", vm.REPORTS.txtPDF2 == null ? "" : vm.REPORTS.txtPDF2);
            insertCommand.Parameters.AddWithValue("@pdffile3", vm.REPORTS.txtPDF3 == null ? "" : vm.REPORTS.txtPDF3);
            insertCommand.Parameters.AddWithValue("@pdffile4", vm.REPORTS.txtPDF4 == null ? "" : vm.REPORTS.txtPDF4);
            insertCommand.Parameters.AddWithValue("@pdffile5", vm.REPORTS.txtPDF5 == null ? "" : vm.REPORTS.txtPDF5);
            insertCommand.Parameters.AddWithValue("@pdffile6", vm.REPORTS.txtPDF6 == null ? "" : vm.REPORTS.txtPDF6);
            insertCommand.Parameters.AddWithValue("@pdffile7", vm.REPORTS.txtPDF7 == null ? "" : vm.REPORTS.txtPDF7);
            insertCommand.Parameters.AddWithValue("@pdffile8", vm.REPORTS.txtPDF8 == null ? "" : vm.REPORTS.txtPDF8);
            insertCommand.Parameters.AddWithValue("@pdffile9", vm.REPORTS.txtPDF9 == null ? "" : vm.REPORTS.txtPDF9);
            insertCommand.Parameters.AddWithValue("@pdffile10", "");

            if (!newrec)
                insertCommand.Parameters.AddWithValue("@RECID", recid);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                vm.REPORTS.alertMessage = "Image Acquisition " + ex;
                return vm.REPORTS;
            }
            finally
            {
                connection.Close();
                vm.REPORTS.alertMessage = "Done...";
            }

            return vm.REPORTS;
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void pictureBox1_Click(object sender, EventArgs e)
        //{
        //    PictureBox pbx = sender as PictureBox;
        //    if (pbx.Name == "pictureBox1" && string.IsNullOrWhiteSpace(txtImage1.Text) ||
        //        pbx.Name == "pictureBox2" && string.IsNullOrWhiteSpace(txtImage2.Text) ||
        //        pbx.Name == "pictureBox3" && string.IsNullOrWhiteSpace(txtImage3.Text) ||
        //        pbx.Name == "pictureBox4" && string.IsNullOrWhiteSpace(txtImage4.Text) ||
        //        pbx.Name == "pictureBox5" && string.IsNullOrWhiteSpace(txtImage5.Text) ||
        //        pbx.Name == "pictureBox6" && string.IsNullOrWhiteSpace(txtImage6.Text) ||
        //        pbx.Name == "pictureBox7" && string.IsNullOrWhiteSpace(txtImage7.Text) ||
        //        pbx.Name == "pictureBox8" && string.IsNullOrWhiteSpace(txtImage8.Text) ||
        //        pbx.Name == "pictureBox9" && string.IsNullOrWhiteSpace(txtImage9.Text) ||
        //        pbx.Name == "pictureBox10" && string.IsNullOrWhiteSpace(txtImage10.Text))
        //        return;

        //    if (pbx.Name == "pictureBox1")
        //        nmrImageCount.Value = 1m;
        //    if (pbx.Name == "pictureBox2")
        //        nmrImageCount.Value = 2m;
        //    if (pbx.Name == "pictureBox3")
        //        nmrImageCount.Value = 3m;
        //    if (pbx.Name == "pictureBox4")
        //        nmrImageCount.Value = 4m;
        //    if (pbx.Name == "pictureBox5")
        //        nmrImageCount.Value = 5m;
        //    if (pbx.Name == "pictureBox6")
        //        nmrImageCount.Value = 6m;
        //    if (pbx.Name == "pictureBox7")
        //        nmrImageCount.Value = 7m;
        //    if (pbx.Name == "pictureBox8")
        //        nmrImageCount.Value = 8m;
        //    if (pbx.Name == "pictureBox9")
        //        nmrImageCount.Value = 9m;
        //    if (pbx.Name == "pictureBox10")
        //        nmrImageCount.Value = 10m;

        //    int xval = Convert.ToInt32(nmrImageCount.Value) - 1;
        //    //imagenotes 0-notes,1-facility,2-recid,3-imagefile
        //    txtSelectedImage.Text = imagenotes[xval, 3];
        //    txtNotes.Text = imagenotes[xval, 0];
        //    if (!string.IsNullOrWhiteSpace(imagenotes[xval, 1]))
        //        combfacility.Text = bissclass.combodisplayitemCodeName("type_code", imagenotes[xval, 1].ToString(), dtfacility, "name");
        //    if (!string.IsNullOrWhiteSpace(txtSelectedImage.Text))
        //        pictureBox12.Image = WebGUIGatway.getpicture(imagenotes[xval, 3]);
        //}

        private void txtImage1_Click(object sender, EventArgs e)
        {
            //TextBox tbx = sender as TextBox;
            //if (tbx.Name == "txtImage1" && string.IsNullOrWhiteSpace(txtImage1.Text) ||
            //    tbx.Name == "txtImage2" && string.IsNullOrWhiteSpace(txtImage2.Text) ||
            //    tbx.Name == "txtImage3" && string.IsNullOrWhiteSpace(txtImage3.Text) ||
            //    tbx.Name == "txtImage4" && string.IsNullOrWhiteSpace(txtImage4.Text) ||
            //    tbx.Name == "txtImage5" && string.IsNullOrWhiteSpace(txtImage5.Text))
            //    return;

            //if (tbx.Name == "txtImage1" )
            //    nmrImageCount.Value = 1m;
            //if (tbx.Name == "txtImage2" )
            //    nmrImageCount.Value = 2m;
            //if (tbx.Name == "txtImage3" )
            //    nmrImageCount.Value = 3m;
            //if (tbx.Name == "txtImage4" )
            //    nmrImageCount.Value = 4m;
            //if (tbx.Name == "txtImage5" )
            //    nmrImageCount.Value = 5m;
            //int xval = Convert.ToInt32(nmrImageCount.Value) - 1;
            //txtSelectedImage.Text = tbx.Text; // "txtImage" + nmrImageCount.Value.ToString().Trim();
            //txtNotes.Text = imagenotes[xval, 0];
            //if (!string.IsNullOrWhiteSpace(imagenotes[xval, 1]) )
            //    combfacility.Text = bissclass.combodisplayitemCodeName("type_code", imagenotes[xval, 1].ToString(), dtfacility, "name");
            //if (!string.IsNullOrWhiteSpace(txtSelectedImage.Text))
            //    pictureBox1.Image = WebGUIGatway.getpicture(txtSelectedImage.Text.Trim());
        }

        //private void txtPDF1_Click(object sender, EventArgs e)
        //{
        //    TextBox tbx = sender as TextBox;
        //    if (tbx.Name == "txtPDF1" && string.IsNullOrWhiteSpace(txtPDF1.Text) ||
        //        tbx.Name == "txtPDF2" && string.IsNullOrWhiteSpace(txtPDF2.Text) ||
        //        tbx.Name == "txtPDF3" && string.IsNullOrWhiteSpace(txtPDF3.Text) ||
        //        tbx.Name == "txtPDF4" && string.IsNullOrWhiteSpace(txtPDF4.Text) ||
        //        tbx.Name == "txtPDF5" && string.IsNullOrWhiteSpace(txtPDF5.Text) ||
        //        tbx.Name == "txtPDF6" && string.IsNullOrWhiteSpace(txtPDF6.Text) ||
        //        tbx.Name == "txtPDF7" && string.IsNullOrWhiteSpace(txtPDF7.Text) ||
        //        tbx.Name == "txtPDF8" && string.IsNullOrWhiteSpace(txtPDF8.Text) ||
        //        tbx.Name == "txtPDF9" && string.IsNullOrWhiteSpace(txtPDF9.Text))
        //        return;

        //    if (tbx.Name == "txtPDF1")
        //        nmrPDFCount.Value = 1m;
        //    if (tbx.Name == "txtPDF2")
        //        nmrPDFCount.Value = 2m;
        //    if (tbx.Name == "txtPDF3")
        //        nmrPDFCount.Value = 3m;
        //    if (tbx.Name == "txtPDF4")
        //        nmrPDFCount.Value = 4m;
        //    if (tbx.Name == "txtPDF5")
        //        nmrPDFCount.Value = 5m;
        //    if (tbx.Name == "txtPDF6")
        //        nmrPDFCount.Value = 6m;
        //    if (tbx.Name == "txtPDF7")
        //        nmrPDFCount.Value = 7m;
        //    if (tbx.Name == "txtPDF8")
        //        nmrPDFCount.Value = 8m;
        //    if (tbx.Name == "txtPDF9")
        //        nmrPDFCount.Value = 9m;
        //    int xval = Convert.ToInt32(nmrPDFCount.Value) - 1;
        //    txtSelectedImage.Text = pdfNotes[xval]; // tbx.Text; 
        //                                            //   txtNotes.Text = pdfNotes[xval];
        //                                            // combfacility.Text = bissclass.combodisplayitemCodeName("type_code", imagenotes[xval, 1].ToString(), dtfacility, "name");
        //    if (!string.IsNullOrWhiteSpace(txtSelectedImage.Text))
        //        pictureBox12.Image = WebGUIGatway.getpicture(pdfNotes[xval]);
        //}

        //private void chkFullScreen_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(txtSelectedImage.Text))
        //    {
        //        frmImageView imagev = new frmImageView(txtSelectedImage.Text);
        //        imagev.Show();
        //    }
        //    //Process.Start(txtSelectedImage.Text);
        //}

        //private void dtTrans_date_LostFocus(object sender, EventArgs e)
        //{
        //    //we must check if there are recordings for this date
        //    picsCounter = pdfCounter = 0;
        //    cboQueryPrevDef.Text = "";
        //    loadpreviousDefinitions();
        //}

        private void combfacility_LostFocus(object sender, EventArgs e)
        {
            //loadpreviousDefinitions();
        }

    }
}