#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using mradmin.BissClass;
using mradmin.DataAccess;
using mradmin.Forms;

using msfunc;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class PreviousPregnancies
    {
        //DataTable anc03;
        //string reference;
        //string groupcode, patientno, msgeventtracker;
        //int recno;

        MR_DATA.MR_DATAvm vm;

        public PreviousPregnancies(MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;
        }


        //public PreviousPregnancies(string xreference, string xgroupcode, string xpatientno, string patientname)
        //{
        //    //InitializeComponent();
        //    //nmrnumberalive.Value = numbalive;
        //    //nmrtotalchildren.Value = prevpregnancies;
        //    //groupcode = xgroupcode;
        //    //patientno = xpatientno;
        //    //reference = xreference;

        //    //if (!string.IsNullOrWhiteSpace(patientno))
        //    //{
        //    //    this.Text = this.Text + " - " + patientname;
        //    //    BtnSave.Enabled = true;
        //    //}
        //}

        //private void PreviousPregnancies_Load(object sender, EventArgs e)
        //{
        //    displaydetails();
        //}

        //private void msgBoxHandler(object sender, EventArgs e)
        //{
        //    Form msgForm = sender as Form;
        //    if (msgForm != null)
        //    {
        //        if (msgeventtracker == "SD" && msgForm.DialogResult == DialogResult.Yes) //TO SAVE
        //        {
        //            savedetails();
        //            btnClose.PerformClick();

        //        }
        //        else if (msgeventtracker == "RI" && recno >= 0 && msgForm.DialogResult == DialogResult.Yes)
        //        {
        //            if (ANC03.DeleteANC03(groupcode, patientno, dataGridView1.Rows[recno].Cells[1].Value.ToString()))
        //            {
        //                dataGridView1.Rows.RemoveAt(recno);
        //                MessageBox.Show("Deleted...", msgBoxHandler);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Record was not Deleted...", msgBoxHandler);
        //            }
        //            recno = -1;
        //        }
        //        else
        //        {
        //            // this.txtpatientno.Text = "";
        //            // this.txtpatientno.Focus();
        //            return;
        //        }
        //    }

        //}

        //void displaydetails()
        //{
        //    anc03 = ANC03.GetANC03(groupcode, patientno);
        //    dataGridView1.Rows.Clear();
        //    DataGridViewRow row = new DataGridViewRow();
        //    for (int i = 0; i < anc03.Rows.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            nmrtotalchildren.Value = Convert.ToInt32(anc03.Rows[i]["PREV_PREG_TOTAL"]);
        //            nmrnumberalive.Value = Convert.ToInt32(anc03.Rows[i]["NOALIVE"]);
        //        }

        //        recno = i;

        //        dataGridView1.Rows.Add();
        //        row = dataGridView1.Rows[i];
        //        row.Cells[0].Value = i + 1;
        //        row.Cells[1].Value = anc03.Rows[i]["MTHOFBIRTH"].ToString();
        //        row.Cells[2].Value = anc03.Rows[i]["DURATIONOFPREG"].ToString();
        //        row.Cells[3].Value = anc03.Rows[i]["BIRTHWT"].ToString();
        //        row.Cells[4].Value = anc03.Rows[i]["SEX"].ToString();
        //        row.Cells[5].Value = anc03.Rows[i]["PLACEOFBIRTH"].ToString();
        //        row.Cells[6].Value = anc03.Rows[i]["PREG_LABOUR"].ToString();
        //        row.Cells[7].Value = anc03.Rows[i]["AGEATDEATH"].ToString();
        //        row.Cells[8].Value = anc03.Rows[i]["CAUSEOFDEATH"].ToString();
        //        row.Cells[9].Value = "OLDREC";

        //    }

        //}

        public MR_DATA.REPORTS savedetails(IEnumerable<MR_DATA.ANC03> tableList, MR_DATA.REPORTS dataList)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            connection.Open();

            foreach(var eachRow in tableList)
            {
                if(eachRow.MTHOFBIRTH == null) {
                    continue;
                }
                
                SqlCommand insertCommand = new SqlCommand();
                if (eachRow.REFERENCE != null && eachRow.REFERENCE == "OLDREC")
                {
                    insertCommand.CommandText = "ANC03_update";
                }
                else
                {
                    insertCommand.CommandText = "ANC03_Add";
                }

              
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@reference", dataList.txtreference);
                insertCommand.Parameters.AddWithValue("@groupcode", dataList.txtgroupcode);
                insertCommand.Parameters.AddWithValue("@patientno", dataList.txtpatientno);
                insertCommand.Parameters.AddWithValue("@posted", false);
                insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
                insertCommand.Parameters.AddWithValue("@prev_preg_total", dataList.total);
                insertCommand.Parameters.AddWithValue("@noalive", dataList.noOfChild);
                insertCommand.Parameters.AddWithValue("@dateofbirth", DateTime.Now.Date);
                insertCommand.Parameters.AddWithValue("@durationofpreg", eachRow.DURATIONOFPREG == null ? "" : eachRow.DURATIONOFPREG);
                insertCommand.Parameters.AddWithValue("@birthwt", eachRow.BIRTHWT == null ? "" : eachRow.BIRTHWT);
                insertCommand.Parameters.AddWithValue("@placeofbirth", eachRow.PLACEOFBIRTH == null ? "" : eachRow.PLACEOFBIRTH);
                insertCommand.Parameters.AddWithValue("@preg_labour", eachRow.PREG_LABOUR == null ? "" : eachRow.PREG_LABOUR);
                insertCommand.Parameters.AddWithValue("@ageatdeath", eachRow.AGEATDEATH == null ? "" : eachRow.AGEATDEATH);
                insertCommand.Parameters.AddWithValue("@causeofdeath", eachRow.CAUSEOFDEATH == null ? "" : eachRow.CAUSEOFDEATH);
                insertCommand.Parameters.AddWithValue("@mthofbirth", eachRow.MTHOFBIRTH == null ? "" : eachRow.MTHOFBIRTH);
                insertCommand.Parameters.AddWithValue("@sex", eachRow.SEX == null ? "" : eachRow.SEX);

                insertCommand.ExecuteNonQuery();
            }

            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
                
            //}

            connection.Close();

            vm.REPORTS.alertMessage = "Submitted";

            return vm.REPORTS;
        }

        //private void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    msgeventtracker = "SD";
        //    DialogResult result = MessageBox.Show("Confirm to Save Details...", "Previous Pregnancies Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.No)
        //        return;
        //    savedetails();
        //    btnClose.PerformClick();

        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //   recno = -1;
        //   if (e.ColumnIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
        //   {
        //       recno = e.RowIndex;
        //       btnDelete.Enabled = true;
        //   }
        //   else
        //   {
        //       btnDelete.Enabled = true;
        //   }
        //}

        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (recno == -1)
        //    {
        //        return;
        //    }
        //    msgeventtracker = "RI";
        //    DialogResult result = MessageBox.Show("Confirm to Delete item...", "Previous Pregnancies Details",
        //        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (ANC03.DeleteANC03(groupcode, patientno, dataGridView1.Rows[recno].Cells[1].Value.ToString()))
        //    {
        //        dataGridView1.Rows.RemoveAt(recno);
        //        result = MessageBox.Show("Deleted...");
        //    }
        //    else
        //    {
        //        result = MessageBox.Show("Record was not Deleted...");
        //    }
        //    recno = -1;
        //}

    }
}