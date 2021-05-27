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
using mradmin.DataAccess;
using mradmin.BissClass;
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmChainMedicalHistory
    {
        billchaindtl bchain = new billchaindtl();
        //string mcgc, mcpatno, lookupsource, AnyCode, Anycode1;
        int recno = 0;

        MR_DATA.MR_DATAvm vm;

        public frmChainMedicalHistory(MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;

            //InitializeComponent();
            //this.Text = "PATIENT NUMBERS CHAINED TO "+groupcode.Trim()+" : "+patientno.Trim()+
            //    "   [ "+name.Trim()+" ]    Medical History";

            //mcgc = groupcode;
            //mcpatno = patientno;
        }

        //private void frmChainMedicalHistory_Load(object sender, EventArgs e)
        //{
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT * FROM MEDHISTCHAIN WHERE GROUPCODE = '" + mcgc + "' and patientno = '" + mcpatno + "' order by name", false);

        //    ListViewItem itm;
        //    string[] arr = new string[12];

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        arr[0] = row["chainedgc"].ToString();
        //        arr[1] = row["chainedpatno"].ToString();
        //        arr[2] = row["name"].ToString();
        //        arr[3] = row["reg_date"].ToString();
        //        arr[4] = row["gender"].ToString();
        //        arr[5] = row["grouphtype"].ToString();
        //        arr[6] = "OLDREC";
        //        arr[7] = row["recid"].ToString();
        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);

        //    }
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
        //        if (string.IsNullOrWhiteSpace(txtgroupcode.Text))
        //            this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        this.txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode1;
        //        txtpatientno.Focus();
        //    }

        //}

        //private void txtpatientno_Leave(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        AnyCode = "";
        //        txtgroupcode.Select();
        //        return;
        //    }
        //    else
        //    {
        //        recno = 0;
        //        if (string.IsNullOrWhiteSpace(AnyCode))  //no lookup value obtained
        //        {
        //            this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");
        //        }
        //        if (txtpatientno.Text.Trim() == mcpatno.Trim())
        //        {
        //            DialogResult result = MessageBox.Show("You can't Chain Records for the same Patient Number...", "ERROR!!!");
        //            txtpatientno.Text = txtgroupcode.Text = "";
        //            return;
        //        }
        //        //check if patientno exists
        //        bchain = billchaindtl.Getbillchain(this.txtpatientno.Text, txtgroupcode.Text);
        //        if (bchain == null)
        //        {
        //            MessageBox.Show("Invalid Patient Number... ");
        //            txtpatientno.Text = " ";
        //            txtgroupcode.Select();
        //            return;
        //        }
        //        else
        //        {
        //            lblname.Text = bchain.NAME;

        //        }
        //        //check f name is already on listview
        //        for (int i = 0; i < listView1.Items.Count; i++)
        //        {
        //            if (txtgroupcode.Text.Trim() == listView1.Items[i].SubItems[0].ToString().Trim() && 
        //                txtpatientno.Text.Trim() == listView1.Items[i].SubItems[1].ToString().Trim() )
        //            {
        //                recno = i;
        //                btnRemove.Enabled = true;
        //                MessageBox.Show("Record has been added once...");
        //                txtpatientno.Text = "";
        //                return;
        //            }
        //        }
        //        btnAdd.Enabled = true;
        //    }
        //}

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtgroupcode.Text) || string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        return;
        //    }
        //    if (recno == 0)
        //    {
        //        string[] row = { txtgroupcode.Text, txtpatientno.Text, bchain.NAME, bchain.REG_DATE.ToShortDateString(), bchain.SEX, bchain.GROUPHTYPE, "NEWREC" };

        //        ListViewItem itm;
        //        itm = new ListViewItem(row);
        //        listView1.Items.Add(itm);
        //    }
        //    btnSubmit.Enabled = true;
        //}


        public MR_DATA.MR_DATAvm btnSubmit_Click(IEnumerable<MR_DATA.BILLCHAIN> tableList)
        {
            //DialogResult result = MessageBox.Show("Confirm to Submit Records...", "Chained Medical History Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            savedetails(tableList);

            return vm;
        }

        void savedetails(IEnumerable<MR_DATA.BILLCHAIN> tableList)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            connection.Open();

            foreach(var tableRow in tableList)
            {
                //if (listView1.Items[i].SubItems[6].ToString().Trim() == "OLDREC")
                //    continue;

                if (tableRow.STATUS == "OLDREC")
                    continue;

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = "MEDHISTCHAIN_Add";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@GROUPCODE", vm.REPORTS.REPORT_TYPE2);
                insertCommand.Parameters.AddWithValue("@PATIENTNO", vm.REPORTS.REPORT_TYPE1);
                insertCommand.Parameters.AddWithValue("@NAME", tableRow.NAME);
                insertCommand.Parameters.AddWithValue("@REG_DATE", Convert.ToDateTime(tableRow.SECTION));
                insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@POSTED", false);
                insertCommand.Parameters.AddWithValue("@GENDER", tableRow.SEX);
                insertCommand.Parameters.AddWithValue("@GROUPHTYPE", tableRow.GROUPHTYPE);
                insertCommand.Parameters.AddWithValue("@CHAINEDGC", tableRow.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@CHAINEDPATNO", tableRow.PATIENTNO);

                insertCommand.ExecuteNonQuery();
            }

            connection.Close();

            //update billchain MEDICALHISTORYCHAIN
            string updstr = "update billchain set MEDHISTORYCHAINED = '1' where groupcode = '" + vm.REPORTS.REPORT_TYPE2 + "' and patientno = '" + vm.REPORTS.REPORT_TYPE1 + "'";
            bissclass.UpdateRecords(updstr, "MR");
           
            vm.REPORTS.alertMessage = "Record(s) saved successfully...";

            return;
        }

        //private void btnRemove_Click(object sender, EventArgs e)
        //{
        //    if (listView1.Items.Count < 1)
        //        return;
        //    DialogResult result = MessageBox.Show("Confirm to Remove..." + listView1.Items[recno].SubItems[2].ToString(), "Chained Medical History Details", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.No)
        //        return;
        //    if (Convert.ToInt32(listView1.Items[recno].SubItems[8].ToString().Trim()) > 0)
        //    {
        //        int recid = Convert.ToInt32(listView1.Items[recno].SubItems[8].ToString().Trim());
        //        string updatestring = "delete from medhistchain where recid = '" + recid + "'";
        //        bissclass.UpdateRecords(updatestring, "MR");
        //    }
        //    /*           {
        //                   SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
        //                   SqlCommand deleteStatement = new SqlCommand();
        //                   deleteStatement.CommandText = "MEDHISTCHAIN_Delete";
        //                   deleteStatement.Connection = connection;
        //                   deleteStatement.CommandType = CommandType.StoredProcedure;

        //                   deleteStatement.Parameters.AddWithValue("@groupcode",txtgroupcode.Text);
        //                   deleteStatement.Parameters.AddWithValue("@patientno", txtpatientno.Text);
        //                   deleteStatement.Parameters.AddWithValue("@chainedgc", mcgc);
        //                   deleteStatement.Parameters.AddWithValue("@chainedpatno", mcpatno);
        //                   try
        //                   {
        //                       connection.Open();
        //                       int count = deleteStatement.ExecuteNonQuery();
        //                   }
        //                   catch (SqlException ex)
        //                   {
        //                       MessageBox.Show(" " + ex, "Delete Chained Medical History Detail", MessageBoxButtons.OK,
        //                           MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                   }
        //                   finally
        //                   {
        //                       connection.Close();
        //                   }
        //               }*/
        //    listView1.Items[recno].Remove();
        //    result = MessageBox.Show("DONE...");
        //    txtpatientno.Text = txtgroupcode.Text = lblname.Text = "";
        //    btnRemove.Enabled = false;
        //    return;
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtpatientno_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtpatientno_Leave(null, null);
        //}
    }
}