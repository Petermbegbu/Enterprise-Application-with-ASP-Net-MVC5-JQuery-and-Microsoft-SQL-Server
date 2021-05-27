#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using msfunc;


using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class rptfrmCorporateClients : Form
    {
        DataTable dtcustclass = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from ClassificationCodes order by name", true);
        string rptcriteria, rptfooter = bissclass.getRptfooter();
        DataSet ds = new DataSet();
        DataTable sdt = new DataTable();
        public rptfrmCorporateClients()
        {
            InitializeComponent();
        }

        private void frmCorporateClients_Load(object sender, EventArgs e)
        {
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            cboClassification.DataSource = dtcustclass;
            cboClassification.ValueMember = "type_code";
            cboClassification.DisplayMember = "Name";
        }
        void getData()
        {
            string selstring = "";
            if (!string.IsNullOrWhiteSpace(cboClassification.Text))
            {
                selstring = " cagetory = '" + cboClassification.SelectedValue.ToString() + "'";
                rptcriteria = "Group = " + cboClassification.Text.Trim();
            }
            if ( chkHMO.Checked)
            {
                selstring = string.IsNullOrWhiteSpace(selstring) ? "" : " AND ";
                selstring += "hmo = '1'";
                rptcriteria = string.IsNullOrWhiteSpace(rptcriteria) ? "" : "; For HMO/NHIS";
            }
            if (chkByDateRange.Checked)
            {
                selstring = string.IsNullOrWhiteSpace(selstring) ? "" : " AND ";
                selstring += "date_reg >= '" + dtDateFrom.Value.ToShortDateString() + "' and date_reg <= '" + dtDateto.Value.ToShortDateString() + "'";
                rptcriteria = string.IsNullOrWhiteSpace(rptcriteria) ? "" : "; For Reg. Dates "+dtDateFrom.Value.ToShortDateString()+" To : "+dtDateto.Value.ToShortDateString();
            }
            if (!string.IsNullOrWhiteSpace(selstring))
                selstring = " WHERE " + selstring;

            string xstring = "SELECT [CUSTNO], [NAME], [CATEGORY], [ADDRESS1], [STATECODE], [COUNTRY], [PHONE], [CR_LIMIT], [DATE_REG], [EMAIL], [CONTACT] from customer " + selstring;

            if (chkName.Checked)
                xstring += "ORDER BY NAME";
            else
                xstring += "ORDER BY CUSTNO";

            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (chkByDateRange.Checked && dtDateFrom.Value > DateTime.Now.Date || dtDateto.Value < dtDateFrom.Value)
            {
                result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            Session["sql"] = "";
            if (sdt.DataSet != null)
                sdt.DataSet.Reset();
            getData();
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data for specified conditions...");
                return;
            }
            ds.Tables.Add(sdt);
            Session["rdlcfile"] = "CustomerList.rdlc";
            string mrptheader = "CORPORATE CLIENTS LISTING";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader, rptfooter, rptcriteria, "", "CORPCLIENTS", "", 0m, "", "", "", ds, true, 0, dtDateFrom.Value, dtDateto.Value, "", isprint,"","");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader, rptfooter, rptcriteria, "", "CORPCLIENTS", "", 0m, "", "", "", ds, 0, dtDateFrom.Value, dtDateto.Value, "", isprint, true,"","");
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

        private void chkByDateRange_Click(object sender, EventArgs e)
        {
            if (chkByDateRange.Checked)
                panel_DateRange.Visible = true;
            else
                panel_DateRange.Visible = false;
        }
    }
}