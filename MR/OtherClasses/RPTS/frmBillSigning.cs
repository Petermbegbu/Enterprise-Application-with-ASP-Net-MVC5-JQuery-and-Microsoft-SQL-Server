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
using MSMR.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmBillSigning : Form
    {
        DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME,CUSTNO FROM CUSTOMER", false);
        public frmBillSigning()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirm to Load Bills for Specified Dates...","Patients To Sign Bills", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
               return;

            string rtnstring = "WHERE trans_date >= '"+dtDateFrom.Value.Date +"' and trans_date <= '"+dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            if (chkTaggedBills.Checked)
                rtnstring += " and signedbill = 1 ";
            else if (chkUntaggedBills.Checked )
                rtnstring += " and signedbill = 0 ";
            DataRow row;
            DataGridViewRow grow;
            DataTable dtbsd = Dataaccess.GetAnytable("", "MR", "select * from BSDETAIL " + rtnstring, false);
            for (int i = 0; i < dtbsd.Rows.Count; i++)
            {
                row = dtbsd.Rows[i];
                dataGridView1.Rows.Add();
                grow = dataGridView1.Rows[i];
                grow.Cells[0].Value = i + 1;
                grow.Cells[1].Value = row["REFERENCE"].ToString();
                grow.Cells[2].Value = row["NAME"].ToString();
                grow.Cells[3].Value = row["GROUPCODE"].ToString();
                grow.Cells[4].Value = row["PATIENTNO"].ToString();
                grow.Cells[5].Value = Convert.ToBoolean(row["SIGNEDBILL"]);
                grow.Cells[6].Value = row["AMOUNT"].ToString();
                grow.Cells[7].Value = bissclass.combodisplayitemCodeName("custno", row["GROUPHEAD"].ToString(), dtcustomer, "name");
                grow.Cells[8].Value = Convert.ToDateTime( row["TRANS_DATE"]).ToString("dd/MM/yyyy");
                grow.Cells[9].Value = bissclass.sysGlobals.WOPERATOR;
                grow.Cells[10].Value = DateTime.Now.ToString();
                grow.Cells[11].Value = row["AUTH_CODE"].ToString();
                grow.Cells[12].Value = Convert.ToDateTime(row["AUTH_DATE"]) < msmrfunc.mrGlobals.mta_start ? "" : Convert.ToDateTime(row["TRANS_DATE"]).ToString("dd/MM/yyyy");
                grow.Cells[13].Value = row["recid"].ToString();
                if (Convert.ToBoolean(row["signedbill"]))
                    dataGridView1.Rows[i].Cells[6].ReadOnly = dataGridView1.Rows[i].Cells[7].ReadOnly = true;
            }
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == dataGridView1.Columns[5].Index)
                    cell.ToolTipText = "Check if Patient has signed his/her bill";
                else if (e.ColumnIndex == dataGridView1.Columns[6].Index)
                    cell.ToolTipText = "Enter Amount Patient signed for...";
            }
        }
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgv = new DataGridViewRow();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgv = dataGridView1.Rows[e.RowIndex];
                if (dgv.Cells[0].Value != null)
                {
                    if (e.ColumnIndex == 6 && Convert.ToDecimal( dgv.Cells[6].Value) > 0) 
                    {
                        if (Convert.ToDecimal( dgv.Cells[6].Value) > 99999 || Convert.ToDecimal( dgv.Cells[6].Value) < 1)
                        {
                            DialogResult result = MessageBox.Show("Invalid Amount Input...");
                            dgv.Cells[6].Value = '0';
                            return;
                        }

                    }
                }
            }
        }
        decimal getbills(string billreference)
        {
            DataTable dtb = Dataaccess.GetAnytable("", "MR", "select reference, SUM(amount) AS amount from billing where reference = '" + billreference + "' group by reference", false);
            return (decimal)dtb.Rows[0]["amount"];
            //Registration bill does not always have same reference with other bills WIP
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirm to Apply Changes made...", "Patient Bill Signing Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
                return;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            connection.Open();
            string savedstring = "update bsdetail set signedbill = @signedbill, amount = @amount, operator = @operator, dtime = @dtime, auth_code = @auth_code, auth_date = @auth_date ", updatestring;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!Convert.ToBoolean(row.Cells[5].Value) || string.IsNullOrWhiteSpace(row.Cells[9].Value.ToString()) )
                    continue;

                row.Cells[5].ReadOnly = true;
                updatestring = savedstring + "WHERE RTRIM(REFERENCE) = '"+row.Cells[1].Value.ToString().Trim()+"'";

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = updatestring;
                insertCommand.Connection = connection;
 
                insertCommand.Parameters.AddWithValue("@signedbill", true);
                insertCommand.Parameters.AddWithValue("@amount", Convert.ToDecimal(row.Cells[6].Value));
                insertCommand.Parameters.AddWithValue("@operator", row.Cells[9].Value.ToString());
                insertCommand.Parameters.AddWithValue("@dtime", Convert.ToDateTime(row.Cells[10].Value));
                insertCommand.Parameters.AddWithValue("@auth_code", row.Cells[11].Value.ToString());
                insertCommand.Parameters.AddWithValue("@auth_date ", string.IsNullOrWhiteSpace(row.Cells[12].Value.ToString()) ? msmrfunc.mrGlobals.mta_start : Convert.ToDateTime(row.Cells[12].Value));

                insertCommand.ExecuteNonQuery();       
            }
            connection.Close();
            result = MessageBox.Show("Completed...");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridViewRow dgv = new DataGridViewRow();
            //if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            //{
            //    dgv = dataGridView1.Rows[e.RowIndex];
            //    if (dgv.Cells[1].Value != null && !string.IsNullOrWhiteSpace(dgv.Cells[1].Value.ToString())
            //    {

            //    }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
            {
                if (e.ColumnIndex == 5)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    DialogResult result;
                    if (!Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[5].Value))
                    {
                        row.Cells[9].Value = row.Cells[10].Value = "";
                        return;
                    }

                    if (Convert.ToDecimal(row.Cells[6].Value) < 1) //get bills
                    {
                        row.Cells[6].Value = getbills(row.Cells[1].Value.ToString());
                        if (Convert.ToDecimal(row.Cells[6].Value) < 1)
                        {
                            result = MessageBox.Show("Amount Patient Signed for must be specified...");
                            row.Cells[5].Value = "";
                            return;
                        }
                    }
                    row.Cells[9].Value = msmrfunc.mrGlobals.WOPERATOR;
                    row.Cells[10].Value = DateTime.Now;
                    btnSubmit.Enabled = true;
                }
            }
        }
    }
}