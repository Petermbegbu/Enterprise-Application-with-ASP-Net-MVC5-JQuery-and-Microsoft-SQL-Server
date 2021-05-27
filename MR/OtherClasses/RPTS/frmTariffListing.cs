#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

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
    public partial class frmTariffListing : Form
    {
        DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true),
            dtcustclass = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE,NAME FROM CUSTCLASS order by name", true);
        string mrptheader, rptcriteria, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt;
        public frmTariffListing()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void frmTariffListing_Load(object sender, EventArgs e)
        {
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            cboBillingCagegory.DataSource = dtcustclass;
            cboBillingCagegory.ValueMember = "reference";
            cboBillingCagegory.DisplayMember = "Name";

            cboTariffCategory.DataSource = dtfacility;
            cboTariffCategory.ValueMember = "Type_code";
            cboTariffCategory.DisplayMember = "name";
        }
        void getData()
        {
            rptcriteria = "";
            string selstring = "";
            if (!string.IsNullOrWhiteSpace(cboTariffCategory.Text) )
            {
                selstring = " WHERE category = '" + cboTariffCategory.SelectedValue.ToString() + "'";
                rptcriteria += "Tariff Category : " + cboTariffCategory.Text.Trim();
            }
            string xstr = "select tariff.reference,tariff.name,tariff.subcategory,tariff.category,tariff.amount, char(50) as categorydesc from tariff " + selstring;
            if (chkAlphabetical.Checked)
                xstr += "order by name";
            else if (chkCategory.Checked)
                xstr += "order by category, name";
            else if (chkReference.Checked)
                xstr += "order by reference";
            sdt = Dataaccess.GetAnytable("", "MR", xstr, false);
            string savedcat = "";
            foreach (DataRow row in sdt.Rows )
            {
                if (row["category"].ToString() != savedcat)
                {
                    row["categorydesc"] = bissclass.combodisplayitemCodeName("type_code", row["category"].ToString(), dtfacility, "name");
                    savedcat = row["category"].ToString();
                }
            }
            
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (sdt != null)
            {
                sdt.Rows.Clear();
                ds.Tables.Clear();
                ds.Clear();
            }
            getData();
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data for Specified Conditions...");
                return;
            }
            ds.Tables.Add(sdt);
            Session["sql"] = "";
            if (chkCategory.Checked)
                Session["rdlcfile"] = "TariffList_Grp.rdlc";
            else
                Session["rdlcfile"] = "TariffListing.rdlc";

            mrptheader = "TARIFF LISTING - ";
            mrptheader += chkMedicalservices.Checked ? "MEDICAL SERVICES" : "STOCK ITEMS";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, "", rptcriteria, "", "TARIFFLIST", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(this.Text, mrptheader, "", rptcriteria, "", "TARIFFLIST", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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

        private void chkCategory_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCategory.Checked)
                panel_Category.Visible = true;
            else
                panel_Category.Visible = false;
        }
    }
}