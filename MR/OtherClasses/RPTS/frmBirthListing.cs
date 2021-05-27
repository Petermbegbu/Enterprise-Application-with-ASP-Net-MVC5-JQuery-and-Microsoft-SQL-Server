#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Data.SqlClient;

using msfunc;
//using msfunc.Forms;

//
//using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmBirthListing : Form
    {
        string mrptheader, rptcriteria; //, rptfooter, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt;
        public frmBirthListing()
        {
            InitializeComponent();
        }

        private void frmBirthListing_Load(object sender, EventArgs e)
        {

        }
        void getData()
        {
            rptcriteria = "";
            string selstring = " WHERE birthdate >= '"+dtDateFrom.Value.ToShortDateString()+"' and birthdate <= '"+dtDateto.Value.ToShortDateString()+" 23:59:59.999'";
            if (!string.IsNullOrWhiteSpace(cboTypeofBirth.Text) && cboTypeofBirth.Text.Substring(0, 1) != "A")
            {
                selstring += " AND LEFT(birthtype,1) = '" + cboTypeofBirth.Text.Substring(0, 1) + "'";
                rptcriteria += "Type of Birth : "+cboTypeofBirth.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(cboDeliveryType.Text))
            {
                selstring += " AND typeofdeli = '" + cboDeliveryType.Text.Substring(0,1) + "'";
                rptcriteria += rptcriteria == "" ? "" : "; Delivery Type : " + cboDeliveryType.Text.Trim();
            }
            if (!chkBoth.Checked)
            {
                string sex = chkMale.Checked ? "MALE" : "FEMALE";
                selstring += " AND sex = '"+sex+"'";
                rptcriteria += rptcriteria == "" ? "" : "; Sex : " + sex == "MALE" ? "< MALE >" : "< FEMALE >";
            }
            string xstr = "select * from births "+selstring;
            sdt = Dataaccess.GetAnytable("", "MR", xstr, false);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (dtDateFrom.Value.Date > DateTime.Now.Date || dtDateFrom.Value.Date < msmrfunc.mrGlobals.mta_start || dtDateto.Value.Date < dtDateFrom.Value.Date )
            {
                result = MessageBox.Show("Invalid Date Specification");
                return;
            }
            sdt = new DataTable();
            ds = new DataSet();
            //if (sdt != null)
            //{
            //    sdt.Rows.Clear();
            //    ds.Tables.Clear();
            //    ds.Clear();
            //}
            getData();
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Record of Births for Specified Conditions...");
                return;
            }
            ds.Tables.Add(sdt);
            Session["sql"] = "";
            if (chkBirthDateChronological.Checked)
                Session["rdlcfile"] = "BirthListing_Date.rdlc";
            else 
                Session["rdlcfile"] = "BirthListing_Alpha.rdlc";

            mrptheader = "BIRTH LISTING";
                mrptheader += chkBirthDateChronological.Checked ? "CHRONOLOGICAL BY DATE" : "ALPHABETICALLY BY CHILD'S NAME";

            mrptheader += " FOR "+dtDateFrom.Value.ToShortDateString()+" TO : "+dtDateto.Value.ToShortDateString();
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, "", rptcriteria, "", "BIRTHLIST", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(this.Text, mrptheader, "", rptcriteria, "", "BIRTHLIST", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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
    }
}