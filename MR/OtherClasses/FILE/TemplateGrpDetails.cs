#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
//using System.Diagnostics;

using msfunc;
//using msfunc.Forms;

//using mradmin.BissClass;
//using mradmin.DataAccess;
//using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class TemplateGrpDetails
    {
        MR_DATA.MR_DATAvm vm;

        public TemplateGrpDetails(MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;

            //InitializeComponent();
        }

        //private void TemplateGrpDetails_Load(object sender, EventArgs e)
        //{
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select * from TEMPLATEGRP", false);
        //    if (dt.Rows.Count < 1)
        //        return;
        //    DataGridViewRow dgv;
        //    int xc = 0;
        //    foreach (DataRow row in dt.Rows )
        //    {
        //        dataGridView1.Rows.Add();
        //        dgv = dataGridView1.Rows[xc];
        //        dgv.Cells[0].Value = row["DESCRIPTION"].ToString();
        //        dgv.Cells[1].Value = " ";
        //        dgv.Cells[2].Value = row["RECID"].ToString();
        //        xc++;
        //    }
        //}

        public MR_DATA.REPORTS btnSubmit_Click(IEnumerable<MR_DATA.MRB20> tableList)
        {
            //if (dataGridView1.Rows.Count < 1)
            //    return;

            //DialogResult result = MessageBox.Show("Confirm To Save Details", "Template Grouping", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            bool newitem = true;
            int xcount = 0;
            DataGridViewRow dgv;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();

            connection.Open();

            foreach(var row in tableList)
            {
                //dgv = dataGridView1.Rows[i];
                if (string.IsNullOrWhiteSpace(row.DESCRIPTION) || row.RECID.ToString() != null && row.RECID.ToString() != "0" && row.TYPE != "Updated")
                    continue;

                newitem = row.TYPE == null || row.RECID.ToString() != null && row.RECID.ToString() != "0" ? true : false;
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = newitem ? "templategrp_Add" : "templategrp_Update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@DESCRIPTION ", row.DESCRIPTION);

                insertCommand.ExecuteNonQuery();
                xcount++;
            }

            connection.Close();

            vm.REPORTS.alertMessage = "Completed..." + xcount.ToString();

            return vm.REPORTS;
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}