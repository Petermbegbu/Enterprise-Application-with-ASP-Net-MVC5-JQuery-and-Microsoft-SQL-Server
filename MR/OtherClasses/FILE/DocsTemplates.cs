#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    public partial class DocsTemplates
    {
        DataTable dt1, dt2, dt3;
        string woperator;
        int recno;
        DataGridViewRow gdgv;
        MR_DATA.MR_DATAvm vm;

        public DocsTemplates(MR_DATA.MR_DATAvm VM2, string xoperator)
        {
            //InitializeComponent();
            vm = VM2;
            woperator = xoperator;
        }

        //private void DocsTemplates_Load(object sender, EventArgs e)
        //{
        //    txtDoctor.Text = woperator;
        //    dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable();
        //    dt1.Columns.Add(new DataColumn("details", typeof(string)));
        //    dt2.Columns.Add(new DataColumn("details", typeof(string)));
        //    dt3.Columns.Add(new DataColumn("details", typeof(string)));
        //    LoadDetails(0);
        //}

        //void LoadDetails(int screenid)
        //{
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select * from mrb20", false);
        //    if (dt.Rows.Count < 1)
        //        return;

        //    DataGridViewRow dgv;
        //    DataRow dr;
        //    int xc = 0;

        //    if (screenid == 1 || screenid == 0)
        //    {
        //        dataGridView1.Rows.Clear();
        //        dt1.Clear();
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            if (!string.IsNullOrWhiteSpace(row["type"].ToString()))
        //                continue;
        //            dataGridView1.Rows.Add();
        //            dgv = dataGridView1.Rows[xc];
        //            dgv.Cells[0].Value = row["shortcut"].ToString();
        //            dgv.Cells[1].Value = row["description"].ToString();
        //            dgv.Cells[3].Value = row["RECID"].ToString();
        //            dr = dt1.NewRow();
        //            dt1.Rows.Add(dr);
        //            dr["details"] = row["FORMATTED"].ToString();
        //            xc++;
        //        }
        //    }

        //    if (screenid == 2 || screenid == 0)
        //    {
        //        xc = 0;
        //        dataGridView2.Rows.Clear();
        //        dt2.Clear();
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            if (string.IsNullOrWhiteSpace(row["type"].ToString()) || row["type"].ToString().Trim() != txtDoctor.Text.Trim())
        //                continue;
        //            dataGridView2.Rows.Add();
        //            dgv = dataGridView2.Rows[xc];
        //            dgv.Cells[0].Value = row["shortcut"].ToString();
        //            dgv.Cells[1].Value = row["description"].ToString();
        //            dgv.Cells[3].Value = row["RECID"].ToString();
        //            dr = dt2.NewRow();
        //            dt2.Rows.Add(dr);
        //            dr["details"] = row["FORMATTED"].ToString();
        //            xc++;
        //        }
        //    }

        //    if (screenid == 3 || screenid == 0)
        //    {
        //        xc = 0;
        //        dataGridView3.Rows.Clear();
        //        dt3.Clear();
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            if (string.IsNullOrWhiteSpace(row["type"].ToString()) || row["type"].ToString().Trim() != cboGroup.Text.Trim())
        //                continue;
        //            dataGridView3.Rows.Add();
        //            dgv = dataGridView3.Rows[xc];
        //            dgv.Cells[0].Value = row["shortcut"].ToString();
        //            dgv.Cells[1].Value = row["description"].ToString();
        //            dgv.Cells[3].Value = row["RECID"].ToString();
        //            dr = dt3.NewRow();
        //            dt3.Rows.Add(dr);
        //            dr["details"] = row["FORMATTED"].ToString();
        //            xc++;
        //        }
        //    }

        //    if (screenid == 0)
        //        LoadGroup();
        //}

        //void LoadGroup()
        //{
        //    //load template groupings
        //    cboGroup.DataSource = Dataaccess.GetAnytable("", "MR", "select * from TEMPLATEGRP", true);
        //    cboGroup.DisplayMember = "DESCRIPTION";
        //    cboGroup.ValueMember = "DESCRIPTION";
        //}

        //public MR_DATA.REPORTS btnSubmitEnterprise_Click()
        //{
        //    //Button btn = sender as Button;
        //    //int xc = 3;

        //    //if (btn.Name == "btnSubmitEnterprise")
        //    //    xc = 1;
        //    //else if (btn.Name == "btnSubmitInd")
        //    //    xc = 2;

        //    SaveDetails(screenId, tempDoctor, tableList);
        //    //LoadDetails(screenId);

        //    return vm.REPORTS;
        //}

        public MR_DATA.REPORTS SaveDetails(int scrnid, string tempDoctor, string tempAreaOfSpec, IEnumerable<MR_DATA.MRB20> tableList)
        {
            //DialogResult result = MessageBox.Show("Confirm To Save Details", "Template Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            var mrb20Data = ErpFunc.RsGet<MR_DATA.MRB20>("MR_DATA", "select * from mrb20");

            bool newitem = true;
            int xcount = 0;
            string xdetails = "";
            //DataGridViewRow dgv;

            SqlConnection connection = new SqlConnection();
            connection = Dataaccess.mrConnection();
            connection.Open();

            //int dg_count = scrnid == 1 ? dataGridView1.Rows.Count : scrnid == 2 ? dataGridView2.Rows.Count : dataGridView3.Rows.Count;

            foreach(var row in tableList)
            {
                if (string.IsNullOrWhiteSpace(row.SHORTCUT) || row.TYPE != "Updated")
                    continue;

                newitem = row.RECID.ToString() == "0" ? true : false;

                foreach(var mrb20 in mrb20Data)
                {
                    if(mrb20.SHORTCUT.Trim() == row.SHORTCUT.Trim())
                    {
                        xdetails = mrb20.FORMATTED;
                    }
                }

                //xdetails = scrnid == 1 ? dt1.Rows[i]["details"].ToString().Trim() : scrnid == 2 ? 
                //    dt2.Rows[i]["details"].ToString().Trim() : dt1.Rows[3]["details"].ToString().Trim();

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = newitem ? "mrb20_Add" : "mrb20_Update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@shortcut", row.SHORTCUT);
                insertCommand.Parameters.AddWithValue("@description", row.DESCRIPTION);
                insertCommand.Parameters.AddWithValue("@formatted", xdetails);
                insertCommand.Parameters.AddWithValue("@type", scrnid == 1 ? "" : scrnid == 2 ? tempDoctor : tempAreaOfSpec);
                insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
                insertCommand.Parameters.AddWithValue("@posted", false);
                insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@operator", woperator);
                insertCommand.Parameters.AddWithValue("@dttime", DateTime.Now);

                insertCommand.ExecuteNonQuery();
                xcount++;
                //dgv.Cells[2].Value = "";

            }

            //for (int i = 0; i < dg_count; i++)
            //{
            //    //dgv = scrnid == 1 ? dataGridView1.Rows[i] : scrnid == 2 ? dataGridView2.Rows[i] : dataGridView3.Rows[i];

            //    if (dgv.Cells[0].Value == null || string.IsNullOrWhiteSpace(dgv.Cells[0].Value.ToString()) || dgv.Cells[2].FormattedValue.ToString() != "Updated")
            //        continue;

            //    newitem = dgv.Cells[3].FormattedValue.ToString() == "0" ? true : false;
            //    xdetails = scrnid == 1 ? dt1.Rows[i]["details"].ToString().Trim() : scrnid == 2 ? dt2.Rows[i]["details"].ToString().Trim() : dt1.Rows[3]["details"].ToString().Trim();
            //    SqlCommand insertCommand = new SqlCommand();
            //    insertCommand.CommandText = newitem ? "mrb20_Add" : "mrb20_Update";
            //    insertCommand.Connection = connection;
            //    insertCommand.CommandType = CommandType.StoredProcedure;

            //    insertCommand.Parameters.AddWithValue("@shortcut", dgv.Cells[0].FormattedValue.ToString());
            //    insertCommand.Parameters.AddWithValue("@description", dgv.Cells[1].FormattedValue.ToString());
            //    insertCommand.Parameters.AddWithValue("@formatted", xdetails);
            //    insertCommand.Parameters.AddWithValue("@type", scrnid == 1 ? "" : scrnid == 2 ? txtDoctor.Text.Trim() : cboGroup.Text.Trim());
            //    insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
            //    insertCommand.Parameters.AddWithValue("@posted", false);
            //    insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
            //    insertCommand.Parameters.AddWithValue("@operator", woperator);
            //    insertCommand.Parameters.AddWithValue("@dttime", DateTime.Now);

            //    insertCommand.ExecuteNonQuery();
            //    xcount++;
            //    dgv.Cells[2].Value = "";
            //}

            connection.Close();

            vm.REPORTS.alertMessage = "Completed..." + xcount.ToString();

            return vm.REPORTS;
        }

        ////private void btnDefineGroup_Click(object sender, EventArgs e)
        ////{
        ////    TemplateGrpDetails grpdtl = new TemplateGrpDetails();
        ////    grpdtl.ShowDialog();
        ////    LoadGroup();
        ////}

        ////private void btnCloseSp_Click(object sender, EventArgs e)
        ////{
        ////    this.Close();
        ////}

        ////private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        ////{
        ////    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        ////    {
        ////        DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        ////        if (e.ColumnIndex == dataGridView1.Columns[0].Index)
        ////            cell.ToolTipText = "Define Shortcut to Consult Template. Click Once - Select for Editting. Double Click - Select for use on Consult Platform.";
        ////        else if (e.ColumnIndex == dataGridView1.Columns[1].Index)
        ////            cell.ToolTipText = "Description of Shortcut to Consult Template";
        ////    }
        ////}

        ////private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        ////{
        ////    if (e.RowIndex >= 0 && e.ColumnIndex == 0 && dataGridView1.Rows[e.RowIndex] != null)
        ////    {
        ////        recno = e.RowIndex;
        ////        dataGridView1.Rows[e.RowIndex].Cells[2].Value = "Updated";

        ////        if (e.RowIndex >= dt1.Rows.Count)
        ////            AddRowToTable(1);

        ////        processSelect(dataGridView1.Rows[e.RowIndex], dt1.Rows[e.RowIndex], 1, txtFormatEnterprise);
        ////    }
        ////}

        ////private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        ////{
        ////    if (e.RowIndex >= 0 && e.ColumnIndex == 0 && dataGridView2.Rows[e.RowIndex] != null)
        ////    {
        ////        recno = e.RowIndex;
        ////        dataGridView2.Rows[e.RowIndex].Cells[2].Value = "Updated";
        ////        if (e.RowIndex >= dt2.Rows.Count)
        ////            AddRowToTable(2);
        ////        processSelect(dataGridView2.Rows[e.RowIndex], dt2.Rows[e.RowIndex], 2, txtFormatInd);
        ////    }
        ////}

        ////private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        ////{
        ////    if (e.RowIndex >= 0 && e.ColumnIndex == 0 && dataGridView3.Rows[e.RowIndex] != null)
        ////    {
        ////        recno = e.RowIndex;
        ////        dataGridView3.Rows[e.RowIndex].Cells[2].Value = "Updated";
        ////        if (e.RowIndex >= dt3.Rows.Count)
        ////            AddRowToTable(3);
        ////        processSelect(dataGridView3.Rows[e.RowIndex], dt3.Rows[e.RowIndex], 3, txtFormatSpec);
        ////    }
        ////}

        ////void processSelect(DataGridViewRow xdgv, DataRow row, int selecttype, TextBox txt)
        ////{
        ////    if (xdgv.Cells[0].Value != null && !string.IsNullOrWhiteSpace(xdgv.Cells[0].Value.ToString()))
        ////    {
        ////        gdgv = xdgv;
        ////        txt.Text = row["details"].ToString();
        ////    }
        ////}

        ////private void btnDeleteSp_Click(object sender, EventArgs e)
        ////{
        ////    Button btn = sender as Button;
        ////    string xstr = btn.Name == "btnDeleteEntrprise" ? "Enterprise" : btn.Name == "btnDeleteInd" ? "Individual" : "Specialized";
        ////    DataGridViewRow dgv;
        ////    dgv = xstr == "Enterprise" ? dataGridView1.Rows[recno] : xstr == "Individual" ? dataGridView2.Rows[recno] : dataGridView1.Rows[recno];
        ////    DialogResult result = MessageBox.Show("Confirm To Delete " + xstr + " - " + dgv.Cells[0].FormattedValue.ToString() + " [" + (recno + 1).ToString() + "] Template ShortCut...", "Template Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        ////    if (result == DialogResult.No)
        ////        return;

        ////    // string xtable = "";
        ////    if (xstr == "Enterprise")
        ////    {
        ////        dataGridView1.Rows.RemoveAt(recno);
        ////        // dgv = dataGridView1.Rows[recno];
        ////        if (dt1.Rows.Count >= recno)
        ////            dt1.Rows.RemoveAt(recno);
        ////    }
        ////    else if (xstr == "Individual")
        ////    {
        ////        dataGridView2.Rows.RemoveAt(recno);
        ////        //dgv = dataGridView2.Rows[recno];
        ////        if (dt2.Rows.Count >= recno)
        ////            dt2.Rows.RemoveAt(recno);
        ////    }
        ////    else
        ////    {
        ////        dataGridView3.Rows.RemoveAt(recno);
        ////        //dgv = dataGridView3.Rows[recno];
        ////        if (dt3.Rows.Count >= recno)
        ////            dt3.Rows.RemoveAt(recno);
        ////        //row = dt3.Rows.Count >= recno ? dt3.Rows[recno] : null;
        ////    }
        ////    if (dgv.Cells[3].Value != null && Convert.ToInt32(dgv.Cells[3].Value) > 0)
        ////    {
        ////        xstr = "delete from MRB20 where recid = '" + dgv.Cells[3].Value.ToString() + "'";
        ////        bissclass.UpdateRecords(xstr, "MR");
        ////    }
        ////    MessageBox.Show("Record Removed...");
        ////}

        ////private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        ////{
        ////    DataGridView dgv = sender as DataGridView;
        ////    if (dgv.Rows[e.RowIndex].Cells[3].FormattedValue.ToString() == "0")
        ////    {
        ////        MessageBox.Show("Selected Format has not been Saved...");
        ////        return;
        ////    }
        ////    gdgv = dgv.Rows[e.RowIndex];
        ////    Session["RtnFormat"] = dgv.Name == "dataGridView1" ? txtFormatEnterprise.Text.Trim() : dgv.Name == "dataGridView2" ? txtFormatInd.Text.Trim() : txtFormatSpec.Text.Trim();
        ////    btnCloseSp.PerformClick();
        ////}

        ////private void btnSelectEnterprise_Click(object sender, EventArgs e)
        ////{
        ////    if (gdgv != null)
        ////    {
        ////        Session["RtnFormat"] = gdgv.DataGridView.Name == "dataGridView1" ? txtFormatEnterprise.Text.Trim() : gdgv.DataGridView.Name == "dataGridView2" ? txtFormatInd.Text.Trim() : txtFormatSpec.Text.Trim();
        ////        btnCloseSp.PerformClick();
        ////    }
        ////}

        ////private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        ////{
        ////    if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < 2)
        ////    {
        ////        DataGridView xdgv = sender as DataGridView;
        ////        recno = e.RowIndex;
        ////        DataGridViewRow dgv = xdgv.Name == "dataGridView1" ? dataGridView1.Rows[recno] : xdgv.Name == "dataGridView2" ? dataGridView2.Rows[recno] : dataGridView3.Rows[recno];
        ////        dgv.Cells[2].Value = "Updated";
        ////    }
        ////}

        ////private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        ////{
        ////    //do nothing
        ////}

        ////void AddRowToTable(int tabid)
        ////{
        ////    DataRow dr = null;
        ////    if (tabid == 1)
        ////    {
        ////        dr = dt1.NewRow();
        ////        dt1.Rows.Add(dr);
        ////    }
        ////    else if (tabid == 2)
        ////    {
        ////        dr = dt2.NewRow();
        ////        dt2.Rows.Add(dr);
        ////    }
        ////    else
        ////    {
        ////        dr = dt3.NewRow();
        ////        dt3.Rows.Add(dr);
        ////    }
        ////    dr["details"] = "";
        ////}

        ////private void txtFormatEnterprise_LostFocus(object sender, EventArgs e)
        ////{
        ////    //update dt details
        ////    TextBox txt = sender as TextBox;
        ////    if (txt.Name == "txtFormatEnterprise")
        ////        dt1.Rows[recno]["details"] = txtFormatEnterprise.Text.Trim();
        ////    else if (txt.Name == "txtFormatInd")
        ////        dt2.Rows[recno]["details"] = txtFormatInd.Text.Trim();
        ////    else
        ////        dt3.Rows[recno]["details"] = txtFormatSpec.Text.Trim();
        ////}

        ////private void cboGroup_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    LoadDetails(3);
        ////}

        private void cboGroup_LostFocus(object sender, EventArgs e)
        {

        }
    }
}