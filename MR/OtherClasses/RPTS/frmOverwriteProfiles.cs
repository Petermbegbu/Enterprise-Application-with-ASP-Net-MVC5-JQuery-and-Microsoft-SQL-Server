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
    public partial class frmOverwriteProfiles : Form
    {
       // string sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt;
        public frmOverwriteProfiles()
        {
            InitializeComponent();
        }
        void getData()
        {
            string bstr = "";
            bstr = " WHERE CONVERT(DATE,DTIME) >= '" + dtDateFrom.Value.ToShortDateString() + "' AND CONVERT(DATE,DTIME) <= '" + dtDateTo.Value.ToShortDateString() + " 23:59:59.999'";

            if (!string.IsNullOrWhiteSpace(txtName.Text))
                bstr += " AND OVERWRITENOTE LIKE '%" + txtName.Text + "'";

            string xstring = "";
            xstring = "SELECT * FROM OVPROFILE " + bstr + " ORDER BY NAME";
            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);

            ds.Tables.Add(sdt);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            if (dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateTo.Value < dtDateFrom.Value || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateTo.Value.Date > DateTime.Now.Date)
            {
                DialogResult result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            Session["sql"] = "";
            sdt = new DataTable();
            ds = new DataSet();

            getData();
            Session["rdlcfile"] = "OverrightProfiles.rdlc";
            string mrptheader = "PATIENTS OVERWRITE PROFILE";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader + " FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateTo.Value.ToShortDateString(), "", "", "",  "ADJUSTMENTS", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                //if (isprint)
                //    paedreports.work();
                //else
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader + " FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateTo.Value.ToShortDateString(), "", "", "", "ADJUSTMENTS", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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

        private void frmOverwriteProfiles_Load(object sender, EventArgs e)
        {

        }
    }
}