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
//using msfunc.Forms;
//using MSMR.Forms;
//
//using mradmin.DataAccess;
using mradmin.BissClass;
//using MSMR.Forms;
//using MSMR;
using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmCashiersLodgmt : Form
    {
        string rptfooter, sysmodule = bissclass.getRptfooter(), mcurrency = "", mlocalcur = bissclass.sysGlobals.mlocalcur, woperator;
        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
        DataSet ds = new DataSet();

        DataTable sdt;
        public frmCashiersLodgmt(string xoperator)
        {
            InitializeComponent();
            woperator = xoperator;
        }
        private void initdataSet()
        {
            ds.Clear();
            ds.Tables.Clear();
            sdt = new DataTable();
 
            sdt.Columns.Add(new DataColumn("customer", typeof(string)));
            sdt.Columns.Add(new DataColumn("name", typeof(string)));
            sdt.Columns.Add(new DataColumn("invamount", typeof(decimal)));
      //       sdt.Columns.Add(new DataColumn("dbnotes", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("payment", typeof(decimal)));
      //      sdt.Columns.Add(new DataColumn("crnotes", typeof(decimal)));
        }
        private void getselectstring()
        {
            string paytype = "";
            if (chkFO_BankTeller.Checked)
                paytype = "BANK TELLER";
            else if (chkFO_Cheque.Checked)
                paytype = "CHEQUE";
            else if (chkFO_DirectCredit.Checked)
                paytype = "DIRECT CREDIT";
            else if (chkFO_POS.Checked)
                paytype = "POS/CREDIT CARD";
            else if (chkCash.Checked)
                paytype = "CASH";
            else
                paytype = "ALL";
            rptfooter = sysmodule + "Cashier's Lodgments";
           // rptcriteria = "";
            string rptstr = "";
            if (!string.IsNullOrWhiteSpace(cboOperator.Text))
                rptstr = " and operator = '" + cboOperator.Text + "'";
         //   bissclass.sysGlobals.waitwindowtext = "Loading Invoices / Pmt Records... Pls Wait!";
         //   pleasewait.Show();
            string xstring = "";
            if (paytype == "ALL")
                xstring = "select operator, count(operator) as transtot, sum(amount) as amount, trans_date, paytype from paydetail where trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'"+rptstr+" group by operator, trans_date, paytype order by trans_date,operator";
            else
                xstring = "select operator, count(operator) as transtot, sum(amount) as amount, trans_date from paydetail where trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999' and paytype = '"+paytype+"'"+rptstr+" group by operator, trans_date order by trans_date";
            dataGridView1.Rows.Clear();
            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            DataGridViewRow dgv;
            DataRow row;
            string xstr = "";
            for (int i = 0; i < sdt.Rows.Count; i++)
            {
                dataGridView1.Rows.Add();
                row = sdt.Rows[i];
                dgv = dataGridView1.Rows[i];
                xstr = chkALL.Checked ? "  - "+row["paytype"].ToString() : "";
                dgv.Cells[0].Value = Convert.ToDateTime(row["trans_date"]).ToShortDateString();
                dgv.Cells[1].Value = row["operator"].ToString() + xstr;
                dgv.Cells[2].Value = row["transtot"].ToString();
                dgv.Cells[3].Value = Convert.ToDecimal( row["amount"]).ToString("N2");
            }
            if (!chkCash.Checked)
                return;
            //check and get lodgment details;
            xstring = "select OPERATOR, TRANS_DATE, CREDIT, CTIME, CDATE, POSTED, RECEIVER, DIFF, RECID from link1 where trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'"+xstr;
            DataTable dt = Dataaccess.GetAnytable("", "MR", xstring, false);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
                foreach (DataGridViewRow xr in dataGridView1.Rows )
                {
                    if (xr.Cells[0].Value == null || xr.Cells[1].Value == null)
                        continue;
                    if (xr.Cells[1].Value.ToString().Trim() == row["operator"].ToString().Trim() && Convert.ToDateTime(xr.Cells[0].Value) == Convert.ToDateTime(row["trans_date"]))
                    {
                        xr.Cells[4].Value = Convert.ToDecimal( row["credit"]).ToString("N2");
                        xr.Cells[5].Value = (Convert.ToDecimal( row["credit"]) - Convert.ToDecimal(xr.Cells[3].Value)).ToString("N2");
                        xr.Cells[6].Value = row["cdate"].ToString();
                        xr.Cells[7].Value = row["receiver"].ToString();
                        xr.Cells[8].Value = row["recid"].ToString();
                        xr.Cells[9].Value = Convert.ToBoolean( row["posted"]) ? "YES" : "NO";
                        break;
                    }
                }
                
            }
        //    add_value_toDatasetTableSummary(xstring);
          //  pleasewait.Hide();

            ds.Tables.Add(sdt);
        }
        void add_value_toDatasetTableSummary(string statement)
        {
            string xoldcustomer = "";
            DataRow currentrow = null, row = null;

            DataTable dt = Dataaccess.GetAnytable("", "AR", statement, false);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
                if (row["name"].ToString() != xoldcustomer) 
                {
                    currentrow = createnewRow(row["name"].ToString(), dt );
                    xoldcustomer = row["name"].ToString();
                }
                currentrow["customer"] = row["customer"].ToString();
                currentrow["name"] = row["name"].ToString();
                currentrow["invamount"] = (decimal)currentrow["invamount"] + (decimal)row["invamount"];
             //   currentrow["dbnotes"] = 0m;
                currentrow["payment"] = (decimal)currentrow["payment"] + (decimal)row["payment"];
             //   currentrow["crnotes"] = 0m;
            }
        }
        DataRow createnewRow(string name, DataTable xdt)
        {
            bool foundit = false;
            DataRow currentrow = null;
            for (int i = 0; i < sdt.Rows.Count; i++)
            {
                currentrow = sdt.Rows[i];
                if (currentrow["name"].ToString().Trim() == name.Trim())
                {
                    foundit = true;
                    break;
                }
            }
            if (foundit)
                return currentrow;

            DataRow dr = sdt.NewRow();
            dr["customer"] = "";
            dr["name"] = "";
            dr["invamount"] = 0m;
            dr["payment"] = 0m;

            sdt.Rows.Add(dr);
            return dr;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            printprocess(false);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            printprocess(true);
        }
        void printprocess(bool isprint)
        {

            if (dtDatefrom.Value < dtmin_date || dtDatefrom.Value > DateTime.Now.Date )
            {
                DialogResult result = MessageBox.Show("Invalid Date specification...");
                return;
            }
          //  initdataSet();
            string xhd = "CASH LODGMENT REPORT ";
            string rptheader = xhd + "  FOR : " + dtDatefrom.Value.ToLongDateString()+"  TO  "+dtDateto.Value.ToShortDateString();
           // getselectstring();
            if (sdt.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("No Data...");
                return;
            }
            Session["rdlcFile"] = "cashierlodgmt.rdlc";

            Session["sql"] = ""; // string.Format(xstring);
            string xcur = string.IsNullOrWhiteSpace(mcurrency) ? mlocalcur : mcurrency;
           // string currencyname = "";
         //   string rpttype = "POSSUMMARY";
    /*        if (!isprint)
            {
                frmARReportViewer reportview = new frmARReportViewer(xhd, rptheader, rptfooter, rptcriteria, "", rpttype, cboCustomer.Text, 0m, xcur, mlocalcur, currencyname, ds, 0, dtDatefrom.Value.Date, dtDatefrom.Value.Date, "AR", isprint, true, "","");
                if (isprint)
                    reportview.work();
                else
                    reportview.Show();
            }
            else
                rptConversion.GeneralRpt(xhd, rptheader, rptfooter, rptcriteria, "", rpttype, cboCustomer.Text, 0m, xcur, mlocalcur, currencyname, ds, 0, dtDatefrom.Value.Date, dtDatefrom.Value.Date, "AR", isprint, true, "", ARS.BissClass.Arfunc.arGlobals.exportrptdirectory);*/
                
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            getselectstring();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!chkCash.Checked)
            {
                MessageBox.Show("Submit Apply to Cashier's Cash Lodgment Only...");
                return;
            }
            DialogResult result = MessageBox.Show("Confirm to Submit Details....", "Cashier's Lodgements", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
                return;

            int xcount = 0;
            DataGridViewRow dgv;
            bool newitem = false;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();

            connection.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dgv = dataGridView1.Rows[i];
                if (dgv.Cells[0].Value == null || dgv.Cells[1].FormattedValue.ToString() == "" || dgv.Cells[9].FormattedValue.ToString() == "YES")
                    continue;
                newitem = dgv.Cells[8].Value == null || dgv.Cells[8].FormattedValue.ToString() == "0" ? true : false;

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = newitem ? "link1_Add" : "link1_Update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@operator", dgv.Cells[1].Value.ToString());
                insertCommand.Parameters.AddWithValue("@trans_date", Convert.ToDateTime(dgv.Cells[0].Value));
                insertCommand.Parameters.AddWithValue("@debit", Convert.ToDecimal(dgv.Cells[3].Value));
                insertCommand.Parameters.AddWithValue("@credit", Convert.ToDecimal(dgv.Cells[4].Value));
                insertCommand.Parameters.AddWithValue("@ctime", DateTime.Now.ToLongTimeString());
                insertCommand.Parameters.AddWithValue("@cdate", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@posted", dgv.Cells[4].Value != null && Convert.ToDecimal(dgv.Cells[4].Value) > 0 ? true : false);
                insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@itemno", dgv.Cells[2].Value != null ? Convert.ToDecimal(dgv.Cells[2].Value) : 0m);
                insertCommand.Parameters.AddWithValue("@ttime", DateTime.Now.ToLongTimeString());
                insertCommand.Parameters.AddWithValue("@time_in", DateTime.Now.ToLongTimeString());
                insertCommand.Parameters.AddWithValue("@receiver", woperator);
                insertCommand.Parameters.AddWithValue("@diff", Convert.ToDecimal(dgv.Cells[4].Value) - Convert.ToDecimal(dgv.Cells[3].Value));

                insertCommand.ExecuteNonQuery();
                xcount++;
            }
            connection.Close();
            MessageBox.Show("Completed..." + xcount.ToString(), "Updated");
        }
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgv = new DataGridViewRow();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgv = dataGridView1.Rows[e.RowIndex];
                if (dgv.Cells[0].Value == null || dgv.Cells[9].FormattedValue.ToString() == "YES" || dgv.Cells[4].Value == null || Convert.ToDecimal(dgv.Cells[4].Value) < 1)
                    return;

                dgv.Cells[5].Value = Convert.ToDecimal(dgv.Cells[4].Value) - Convert.ToDecimal(dgv.Cells[3].Value);
                dgv.Cells[6].Value = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm");
                dgv.Cells[7].Value = woperator;

            }
        }

        private void frmCashiersLodgmt_Load(object sender, EventArgs e)
        {
            cboOperator.DataSource = Dataaccess.GetAnytable("", "MR", "select operator from mrstlev where left(section,1) = '2'", true);
            cboOperator.ValueMember = "operator";
            cboOperator.DisplayMember = "operator";
        }
    }
}