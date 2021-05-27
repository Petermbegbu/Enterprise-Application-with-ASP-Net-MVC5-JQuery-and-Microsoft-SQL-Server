#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;

using msfunc;
using msfunc.Forms;


using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;


using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmEOMStatement_WBill : Form
    {
        DataTable sdt, bills;
        DataSet ds;
        public frmEOMStatement_WBill(DataTable rpttable, DataTable xbills, string grpreference, bool sep_opdinpbills, string period)
        {
            InitializeComponent();
            txtgrpreference.Text = grpreference;
            txttitleheader.Text = "MEDICAL BILLS FOR THE MONTH OF " + period;
            sdt = rpttable;
            bills = xbills;
            Session["sep_opdinpbills"] = sep_opdinpbills ? "Y" : "N";
            if (sep_opdinpbills)
                chkPrintCummulativeTotal.Visible = true;
        }
        private void frmEOMStatement_WBill_Load(object sender, EventArgs e)
        {
            
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {//=Lookup(Fields!ProductID.Value, Fields!ID.Value, Fields!Name.Value, "Product") 
            DialogResult result;
            if (string.IsNullOrWhiteSpace(txtheader.Text) || string.IsNullOrWhiteSpace(txtfooter.Text))
            {
                result = MessageBox.Show("Header and Footer of letter must be specified...", "End of Month Statement");
                return;
            }
            if (bills.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data for Specified Conditions...");
                return;
            }
            ds.Tables.Add(sdt);
            ds.Tables.Add(bills);

            Session["sql"] = "";
            if (Session["sep_opdinpbills"].ToString() == "Y")
                Session["rdlcfile"] = "EOMStatement_WBopdinpDtl.rdlc";
            else
                Session["rdlcfile"] = "EOMStatement_WBDtl.rdlc";

            string mrptheader = "EOM Statement with Bills";
            frmReportViewer paedreports = new frmReportViewer(mrptheader, txttitleheader.Text, "", "", "", "EOMSTATMTWBILL", "", 0m, "", "", "", ds, true, 0, dtMailingDate.Value.Date, dtMailingDate.Value.Date, "", isprint);

            if (isprint)
                paedreports.work();
            else
                paedreports.Show();
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            printprocess(false);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            printprocess(true);
        }
    }
}