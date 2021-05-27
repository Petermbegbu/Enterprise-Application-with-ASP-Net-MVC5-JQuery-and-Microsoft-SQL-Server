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
    public partial class frmDeathRecords : Form
    {
        string mrptheader, rptcriteria, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt,dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES  order by name", true);
        public frmDeathRecords()
        {
            InitializeComponent();
        }
        private void frmDeathRecords_Load(object sender, EventArgs e)
        {
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            cboDiag.DataSource = dtdiag;
            cboDiag.ValueMember = "Type_code";
            cboDiag.DisplayMember = "name";
        }
        void getData()
        {
            rptcriteria = "";
            string selstring = " WHERE deathdate >= '" + dtDateFrom.Value.ToShortDateString() + "' and deathdate <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            if (!string.IsNullOrWhiteSpace(cboDiag.Text) )
            {
                selstring += " AND diag LIKE '%" + cboDiag.Text+ "'";
                rptcriteria += "Diagnosis : "+cboDiag.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtDescSearch.Text))
            {
                selstring += " AND REMARKS LIKE '%" + txtDescSearch.Text.Trim() + "%' ";
            }
            if (!chkBoth.Checked)
            {
                string sex = chkMale.Checked ? "MALE" : "FEMALE";
                selstring += " AND sex = '"+sex+"'";
                rptcriteria += rptcriteria == "" ? "" : "; Sex : " + sex;
            }
            if (nmrAgeFrom.Value > 0 && nmrAgeTo.Value > 0)
            {
                selstring += " AND DOB - DEATHDATE >= '" + nmrAgeFrom.Value + "' AND DOB - DEATHDATE <= '" + nmrAgeTo.Value + "'";
            }
            string xstr = "select * from deaths "+selstring;
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
            getData();
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Recorded Deaths for Specified Conditions...");
                return;
            }
            ds.Tables.Add(sdt);
            Session["sql"] = "";
            if (chkDeathDateChronological.Checked)
                Session["rdlcfile"] = "DeathListing_ByDate.rdlc";
            else 
                Session["rdlcfile"] = "DeathListing_Alpha.rdlc";

            mrptheader = "DEATH RECORDS";
                mrptheader += chkDeathDateChronological.Checked ? "CHRONOLOGICAL BY DATE" : "ALPHABETICALLY BY NAME";

            mrptheader += " FOR "+dtDateFrom.Value.ToShortDateString()+" TO : "+dtDateto.Value.ToShortDateString();
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, "", rptcriteria, "", "BIRTHLIST", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
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