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

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmMSDutyrGroup : Form
    {
        DataTable dttab;
        public frmMSDutyrGroup()
        {
            InitializeComponent();
        }

        private void frmMSDutyrGroup_Load(object sender, EventArgs e)
        {
            dttab = Dataaccess.GetAnytable("", "MR", "SELECT * FROM MSDUTYRGRP", false);
            int counter = 0;
            DataGridViewRow dgv;
            foreach (DataRow row in dttab.Rows )
            {
                dataGridView1.Rows.Add();
                dgv = dataGridView1.Rows[counter];
                dgv.Cells[0].Value = row["name"].ToString();
                dgv.Cells[1].Value = row["recid"].ToString();
                counter++;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirm to Submit Records...", "MS Duty Roaster Grouping", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
                return;
            int xrec = 0;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();

            connection.Open();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value == null || string.IsNullOrWhiteSpace(row.Cells[0].FormattedValue.ToString()))
                    continue;
                xrec = Convert.ToInt32(row.Cells[1].Value);
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = xrec < 1 ? "msdutyrgrp_Add" : "msdutyrgrp_update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@name", row.Cells[0].FormattedValue.ToString());
                if (xrec > 0)
                    insertCommand.Parameters.AddWithValue("@recid", Convert.ToInt32( row.Cells[1].Value.ToString()));

                insertCommand.ExecuteNonQuery();
             }
             connection.Close();
             MessageBox.Show("Completed...", "Group Update");
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}