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


using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmEOMStatement_Summary : Form
    {
        DataTable sdt = new DataTable(), bills = new DataTable();
        DataSet ds = new DataSet();
        bool rptwithbills,ftime;
        public frmEOMStatement_Summary(bool withbills, DataTable xtable, DataTable xbills, string grpreference, string period, bool sep_opdinpbills)
        {
            InitializeComponent();
            sdt = xtable;
            txtgrpreference.Text = grpreference;
            txttitleheader.Text = "MEDICAL BILLS FOR THE MONTH OF " + period;
            rptwithbills = withbills;
            if (withbills)
            {
                bills = xbills;
                Session["sep_opdinpbills"] = sep_opdinpbills ? "Y" : "N";
                panel_WithBills.Visible = true;
                if (sep_opdinpbills)
                    chkPrintCummulativeTotal.Visible = true;
            }
            else
                panel_Summary.Visible = true;
        }
        private void frmEOMStatement_Summary_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtgrpreference.Text))
                return;
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select bftext, pymtext, adjusttext, cur_billtext, baltext, rpthdtext, footertext, headertext from cprptdef where ltrim(rtrim(reference)) =  '" + txtgrpreference.Text.Trim() + "'", false);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                if (!rptwithbills)
                {
                    txtbalbf.Text = row["bftext"].ToString();
                    txtPayments.Text = row["pymtext"].ToString();
                    txtadjustment.Text = row["adjusttext"].ToString();
                    txtcur_bills.Text = row["cur_billtext"].ToString();
                    txtbalance.Text = row["baltext"].ToString();
                }
               // txttitleheader.Text = row["rpthdtext"].ToString();
                txtheader.Text = row["headertext"].ToString();
                txtfooter.Text = row["footertext"].ToString();
               
                lblGroupRefCopy.Visible = txtRefernceCopy.Visible = btnGrpReference.Visible = false;
                txttitleheader.Focus();
            }
            if (dt.Rows.Count < 1 || string.IsNullOrWhiteSpace(txtcur_bills.Text))
            {
                txtbalbf.Text = "Balance brought forward";
                txtPayments.Text = "Payment During the month";
                txtadjustment.Text = "Adjustments (DN/CN)";
                txtcur_bills.Text = "CURRENT BILLS";
                txtbalance.Text = "Total amount payable";
            }
            ftime = true;
        }
        private void btnCopy_Click(object sender, EventArgs e)
        {
            lblGroupRefCopy.Visible = txtRefernceCopy.Visible = btnGrpReference.Visible = true;
            txtRefernceCopy.Focus();
        }
        private void txtRefernceCopy_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRefernceCopy.Text))
                return;
            DialogResult result;
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select bftext, pymtext, adjusttext, curbaltext, baltext, rpthdtext, footertext, headertext from cprptdef where ltrimg(rtrim(reference)) =  '" + txtRefernceCopy.Text.Trim() + "'", false);
            if (dt.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid group reference...", "End of Month Statement");
                txtRefernceCopy.Text = "";
                return;
            }
            result = MessageBox.Show("Confirm to Copy Details from " + txtRefernceCopy.Text.Trim() + "...", "End of Month Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
                return;
            DataRow row = dt.Rows[0];

            if (!rptwithbills)
            {
                txtbalbf.Text = row["bftext"].ToString();
                txtPayments.Text = row["pymtext"].ToString();
                txtadjustment.Text = row["adjusttext"].ToString();
                txtcur_bills.Text = row["curbilltext"].ToString();
                txtbalance.Text = row["baltext"].ToString();
            }

            //txttitleheader.Text = row["rpthdtext"].ToString();
            txtfooter.Text = row["footertext"].ToString();
            txtheader.Text = row["headertext"].ToString();
            lblGroupRefCopy.Visible = txtRefernceCopy.Visible = false;
            txttitleheader.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (string.IsNullOrWhiteSpace(txtheader.Text) || string.IsNullOrWhiteSpace(txtfooter.Text))
            {
                result = MessageBox.Show("Header and Footer of letter must be specified...","End of Month Statement");
                return;
            }
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data for Specified Conditions...");
                return;
            }
            if (rptwithbills && bills.Rows.Count < 1)
            {
                result = MessageBox.Show("No billing record generated for Specified Conditions...");
                return;
            }
            if (!ftime)
                ds.Tables.Clear();
          //  ds.Tables.Clear();
          //  sdt = new DataTable();
          //   ds = new DataSet();
 
            //we must save users input here for future use
            if (chkUpdated.Checked)
                saveDetails();

           // sdt = savedsdt;
            if (rptwithbills)
            {
              //  bills = savedbills;
                ds.Tables.Add(bills);
                ds.Tables.Add(sdt);
            }
            else
                ds.Tables.Add(sdt);

            Session["sql"] = "";
            if (rptwithbills)
            {
                if (Session["sep_opdinpbills"].ToString() == "Y")
                    Session["rdlcfile"] = chkPrintLetterHead.Checked ? "EOMStatement_WBopdinpDtlLH.rdlc" : "EOMStatement_WBopdinpDtl.rdlc";
                else
                    Session["rdlcfile"] = chkPrintLetterHead.Checked ? "EOMStatementWBDtlNEWLH.rdlc" : "EOMStatementWBDtlNEW.rdlc";
            }
            else
            {
                Session["rdlcfile"] = chkPrintLetterHead.Checked ? "EOMStatement_SummaryLH.rdlc" : "EOMStatement_Summary.rdlc";
                Session["isbf"] = chkbalbf.Checked ? "Y" : "N";
                Session["ispayment"] = chkPayments.Checked ? "Y" : "N";
                Session["isadjust"] = chkAdjustments.Checked ? "Y" : "N";
                Session["iscur_bal"] = chkCurBills.Checked ? "Y" : "N";
                Session["isbalance"] = chkBalance.Checked ? "Y" : "N";
            }
            Session["mdate"] = dtMailDate.Value.ToLongDateString();
            string mrptheader = rptwithbills ? "EOM Statement with Bills" : "EOM Statement with Summary Accounts";
            ftime = false;
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader, "", "", "", rptwithbills ? "EOMSTATMTWBILL" : "EOMSUMMARY", "", 0m, "", "", "", ds, true, 0, dtMailDate.Value.Date, dtMailDate.Value.Date, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader, "", "", "", rptwithbills ? "EOMSTATMTWBILL" : "EOMSUMMARY", "", 0m, "", "", "", ds, 0, dtMailDate.Value.Date, dtMailDate.Value.Date, "", isprint, true, "", "");
            }


 
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            printprocess(false);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            printprocess(true);
        }
        void saveDetails()
        {
            string updatestring = "";
            if (rptwithbills)
                updatestring = "update cprptdef set rpthdtext = @rpthdtext, headertext = @headertext, footertext = @footertext where reference = '" + txtgrpreference.Text + "'";
            else
                updatestring = "update cprptdef set bftext = @bftext, pymtext = @pymtext, adjusttext = @adjusttext, cur_billtext = @cur_billtext, baltext = @baltext, rpthdtext = @rpthdtext, headertext = @headertext, footertext = @footertext where reference = '" + txtgrpreference.Text + "'";

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = updatestring;
            insertCommand.Connection = connection;
            //  insertCommand.CommandType = CommandType.StoredProcedure;
            connection.Open();

            if (!rptwithbills)
            {
                insertCommand.Parameters.AddWithValue("@bftext", txtbalbf.Text);
                insertCommand.Parameters.AddWithValue("@pymtext", txtPayments.Text);
                insertCommand.Parameters.AddWithValue("@adjusttext", txtadjustment.Text);
                insertCommand.Parameters.AddWithValue("@cur_billtext", txtcur_bills.Text);
                insertCommand.Parameters.AddWithValue("@baltext", txtbalance.Text);
            }
            insertCommand.Parameters.AddWithValue("@rpthdtext", txttitleheader.Text);
            insertCommand.Parameters.AddWithValue("@headertext", txtheader.Text);
            insertCommand.Parameters.AddWithValue("@footertext", txtfooter.Text);

            insertCommand.ExecuteNonQuery();

            connection.Close();
            foreach (DataRow row in sdt.Rows )
            {
                if (!rptwithbills)
                {
                    row["bftext"] = txtbalbf.Text;
                    row["pymtext"] = txtPayments.Text;
                    row["adjusttext"] = txtadjustment.Text;
                    row["cur_billtext"] = txtcur_bills.Text;
                    row["baltext"] = txtbalance.Text;
                }
                row["rpthdtext"] = txttitleheader.Text;
                row["headertext"] = txtheader.Text;
                row["footertext"] = txtfooter.Text;
            }
        }

        private void txttitleheader_TextChanged(object sender, EventArgs e)
        {
            chkUpdated.Checked = true;
        }

        private void frmEOMStatement_Summary_FormClosing(object sender, FormClosingEventArgs e)
        {
            ds.Tables.Clear();
            ds = new DataSet();
            bills = new DataTable();
            sdt = new DataTable();
        }

    }
}