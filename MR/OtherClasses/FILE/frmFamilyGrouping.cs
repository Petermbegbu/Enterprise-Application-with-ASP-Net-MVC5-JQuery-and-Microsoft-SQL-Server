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

using mradmin.Forms;
using mradmin.DataAccess;
using mradmin.BissClass;

using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmFamilyGrouping
    {
        MR_DATA.MR_DATAvm vm;
        billchaindtl bchain;
        Customer customers;
        patientinfo patients;

        string lookupsource, anycode, PicSelected;

        public frmFamilyGrouping(MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;
            bchain = new billchaindtl();
            patients = new patientinfo();
            customers = new Customer();

            //InitializeComponent();

            //txtgroupcode.Text = xghgroupcode;
            //txtpatientno.Text = xgrouphead;
        }

        //private void frmFamilyGrouping_Load(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        txtpatientno_LostFocus(null, null);
        //        txtMemberNo.Focus();
        //    }
        //    else
        //        txtgroupcode.Focus();
        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtgroupcode_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtgroupcode.Text))
        //        return;
        //    txtpatientno.Focus();
        //}

        //private void txtpatientno_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtpatientno_LostFocus(null, null);
        //}

        //private void txtpatientno_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        anycode = "";
        //        txtgroupcode.Select();
        //        return;
        //    }

        //    if (string.IsNullOrWhiteSpace(anycode))  //no lookup value obtained
        //        this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");

        //    //check if patientno exists
        //    patients = patientinfo.GetPatient(this.txtpatientno.Text, txtgroupcode.Text);
        //    if (patients == null || string.IsNullOrWhiteSpace(patients.name) || !patients.isgrouphead)
        //    {
        //        MessageBox.Show("Invalid Record in Family Group Head...");
        //        txtpatientno.Text = "";
        //        return;
        //    }

        //    TXTPATIENTNAME.Text = patients.name;
        //    PicSelected = patients.piclocation;
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT name, groupcode, patientno, residence, sex, BIRTHDATE from billchain where grouphead = '" + txtpatientno.Text + "'", false);
        //    nmrTotalinGrp.Value = dt.Rows.Count;
        //    int xcount = 0;
        //    listView1.Items.Clear();
        //    string[] arr = new string[8];
        //    ListViewItem itm;
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        xcount++;
        //        arr[0] = xcount.ToString();
        //        arr[1] = row["name"].ToString();
        //        arr[2] = row["groupcode"].ToString();
        //        arr[3] = row["patientno"].ToString();
        //        arr[4] = row["residence"].ToString();
        //        arr[5] = row["sex"].ToString();
        //        arr[6] = Convert.ToDateTime(row["BIRTHDATE"]).ToShortDateString();
        //        arr[7] = "";

        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);
        //    }
        //    if (!string.IsNullOrWhiteSpace(PicSelected))
        //    {
        //        pictureBox2.Image = WebGUIGatway.getpicture(PicSelected);
        //    }

        //    //  bchain = billchaindtl.Getbillchain(this.txtpatientno.Text, txtgroupcode.Text);

        //}


        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    //           Button btn = (Button)sender;

        //    if (btn.Name == "btnMembergrpcode")
        //    {
        //        this.txtMemberGrpCode.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "g";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //    }
        //    else if (btn.Name == "btnpatientlookup")
        //    {
        //        this.txtMemberNo.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //    }
        //    else if (btn.Name == "btngroupcode")
        //    {
        //        this.txtgroupcode.Text = "";
        //        lookupsource = "G";
        //        msmrfunc.mrGlobals.crequired = "p";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnpatientno")
        //    {
        //        txtpatientno.Text = "";
        //        lookupsource = "P";
        //        msmrfunc.mrGlobals.crequired = "P";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
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
        //        this.txtMemberGrpCode.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        this.txtMemberNo.Text = anycode = msmrfunc.mrGlobals.anycode1;
        //        this.txtMemberNo.Select();
        //    }

        //    else if (lookupsource == "L") //patientno
        //    {
        //        this.txtMemberNo.Text = anycode = msmrfunc.mrGlobals.anycode1;
        //        this.txtMemberNo.Select();
        //    }
        //    else if (lookupsource == "G") //groupcodee
        //    {
        //        this.txtgroupcode.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Text = anycode = msmrfunc.mrGlobals.anycode1;
        //        this.txtgroupcode.Focus();
        //    }

        //    else if (lookupsource == "P") //patientno
        //    {
        //        this.txtpatientno.Text = anycode = msmrfunc.mrGlobals.anycode1;
        //        this.txtpatientno.Focus();
        //    }
        //}

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

        //private void txtMemberNo_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtMemberGrpCode.Text))
        //    {
        //        anycode = "";
        //        return;
        //    }

        //    if (string.IsNullOrWhiteSpace(anycode))  //no lookup value obtained
        //        this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");

        //    //check if patientno exists
        //    bchain = billchaindtl.Getbillchain(this.txtMemberNo.Text, txtMemberGrpCode.Text);
        //    if (bchain == null)
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Patient Number... ");
        //        txtMemberNo.Text = " ";
        //        return;
        //    }
        //    anycode = "";
        //    txtname.Text = bchain.NAME;
        //    if (!string.IsNullOrWhiteSpace(bchain.PICLOCATION))
        //        pictureBox1.Image = WebGUIGatway.getpicture(bchain.PICLOCATION);
        //    //check if patient is already selected...
        //    foreach (ListViewItem itm in listView1.Items)
        //    {
        //        if (itm.SubItems[2].Text.Trim() == bchain.GROUPCODE.Trim() && itm.SubItems[3].Text.Trim() == bchain.PATIENTNO.Trim())
        //        {
        //            MessageBox.Show("This Patient is already a Member of this Family...");
        //            txtMemberNo.Text = "";
        //            return;
        //        }
        //    }

        //}

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    if (!bissclass.IsPresent(txtpatientno, "Group head", false) || !bissclass.IsPresent(txtgroupcode, "Group Head Groupcoee", false) || !bissclass.IsPresent(txtMemberNo, "Patient Number", false) || !bissclass.IsPresent(txtMemberGrpCode, "Patient Groupcode", false) || !bissclass.IsPresent(txtname, "Patient Name", false) || !bissclass.IsPresent(TXTPATIENTNAME, "Group Head Name", false))
        //        return;

        //    foreach (ListViewItem itm in listView1.Items)
        //    {
        //        if (itm.SubItems[2].Text.Trim() == txtMemberGrpCode.Text.Trim() && itm.SubItems[3].Text.Trim() == txtMemberNo.Text.Trim())
        //        {
        //            MessageBox.Show("This Patient is already a Member of this Family...");
        //            txtMemberNo.Text = "";
        //            return;
        //        }
        //    }
        //    string[] row = { listView1.Items.Count.ToString(), bchain.NAME, bchain.GROUPCODE, bchain.PATIENTNO, bchain.RESIDENCE, bchain.SEX, bchain.BIRTHDATE.ToShortDateString(), "YES" };
        //    ListViewItem xitm;
        //    xitm = new ListViewItem(row);
        //    listView1.Items.Add(xitm);
        //    btnSubmit.Enabled = true;
        //}

        //public MR_DATA.MR_DATAvm btnSubmit()
        //{
        //    //if (listView1.Items.Count < 1 || string.IsNullOrWhiteSpace(txtpatientno.Text) || string.IsNullOrWhiteSpace(txtgroupcode.Text))
        //    //    return;

        //    //DialogResult result = MessageBox.Show("Confirm to Save Records...", "Family Grouping", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

        //    //if (result == DialogResult.No)
        //    //    return;

        //    patients = patientinfo.GetPatient(vm.REPORTS.txtpatientno, vm.REPORTS.txtgroupcode);

        //    int xc = 0;
        //    string updstr = "update billchain set grouphead = '", selstr = "";
        //    patientinfo pattmp;

        //    foreach (ListViewItem itm in listView1.Items)
        //    {
        //        if (txtgroupcode.Text.Trim() == itm.SubItems[2].Text.Trim() && txtpatientno.Text.Trim() == itm.SubItems[3].Text.Trim() || itm.SubItems[7].Text.Trim() != "YES")
        //            continue;
        //        xc++;
        //        selstr = updstr + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //        bissclass.UpdateRecords(selstr, "MR");

        //        pattmp = patientinfo.GetPatient(itm.SubItems[3].Text, itm.SubItems[2].Text);
        //        if (pattmp != null)
        //            patientinfo.DeletePatient(itm.SubItems[3].Text, itm.SubItems[2].Text);
        //        if (chkMovetransactions.Checked)
        //        {
        //            //bills
        //            selstr = "update billing set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', transtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //payments
        //            selstr = "update paydetail set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', transtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //daily attendance
        //            selstr = "update mrattend set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //medhist
        //            selstr = "update medhist set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //pharmacy
        //            selstr = "update dispensa set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //inpatient drugs
        //            selstr = "update inpdispensa set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //Investigations
        //            selstr = "update labdet set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //Admissions
        //            selstr = "update admrecs set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //ante natal
        //            selstr = "update ancreg set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //anc01
        //            selstr = "update anc01 set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //            //anc02
        //            selstr = "update anc02 set grouphead = '" + patients.patientno + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + itm.SubItems[2].Text + "' and patientno = '" + itm.SubItems[3].Text + "'";
        //            bissclass.UpdateRecords(selstr, "MR");
        //        }
        //        itm.SubItems[7].Text = "";
        //    }
        //    chkReGroup.Checked = false;
        //    MessageBox.Show("Completed..." + xc.ToString() + " Updated...");

        //    return vm;
        //}

        //private void chkReGroup_Click(object sender, EventArgs e)
        //{
        //    if (listView1.Items.Count < 1 || !chkReGroup.Checked)
        //        return;
        //    foreach (ListViewItem itm in listView1.Items)
        //    {
        //        itm.SubItems[7].Text = "YES";
        //    }
        //}

    }
}