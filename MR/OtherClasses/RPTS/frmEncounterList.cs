#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using msfunc;
//using msfunc.Forms;


//using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmEncounterList : Form
    {
        string lookupsource, AnyCode, mrptheader, sysmodule = bissclass.getRptfooter(), mrpttype;
        DataSet ds = new DataSet();
        DataTable sdt;
        public frmEncounterList()
        {
            InitializeComponent();
        }
        private void frmEncounterList_Load(object sender, EventArgs e)
        {

        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn.Name == "btngrouphead" || btn.Name == "btngroupcode")
            {
                if (btn.Name == "btngrouphead")
                {
                    txtgrouphead.Text = "";
                    lookupsource = "C";
                }
                else
                {
                    txtgroupcode.Text = "";
                    lookupsource = "g";
                }
                
                msmrfunc.mrGlobals.crequired = "C";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORPORATE CLIENTS";
            }

            frmselcode FrmSelCode = new frmselcode();
            FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
            FrmSelCode.ShowDialog();
        }
        void FrmSelCode_Closed(object sender, EventArgs e)
        {
            frmselcode FrmSelcode = sender as frmselcode;
            if (lookupsource == "C") //patientno
            {
                txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtgrouphead.Select();
            }
            else
            {
                txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtgroupcode.Select();
            }
        }
        private void txtgrouphead_LostFocus(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrWhiteSpace(txt.Text))
                return;
            DialogResult result;
            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + txt.Text + "'", false);
            if (dtcustomer.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid Corporate Clients Reference...");
                txt.Text = "";
                if (txt.Name == "txtgroupcode")
                    txtgroupcode.Focus();
                else
                    txtgrouphead.Focus();
                return;
            }
            lblName.Text = dtcustomer.Rows[0]["name"].ToString();
            dtDateFrom.Focus();
        }
        void GetData()
        {
            /*			replace groupcode WITH mrattend.groupcode,patientno WITH mrattend.patientno,;
			name WITH mrattend.name,residence WITH mrattend.diagnosis,grouphead WITH mrattend.grouphead,;
			reg_date WITH mrattend.date,;
			staffno WITH seeksay(mrattend.groupcode+mrattend.patientno,'billchai','staffno')
			select mrattend*/
            string rtnstring = " where mrattend.TRANS_DATE >= '" + dtDateFrom.Value.ToShortDateString() + "' AND mrattend.TRANS_DATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
                rtnstring += " and mrattend.grouphead = '" + txtgrouphead.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                rtnstring += " and mrattend.groupcode = '" + txtgroupcode.Text + "'";
            string selstring = "select mrattend.groupcode, mrattend.patientno, mrattend.name, mrattend.diagnosis, mrattend.grouphead, mrattend.trans_date, billchain.staffno as AUTHORIZEDCODE, pmedhdiag.provisional, pmedhdiag.final, char(50) as ghname from mrattend LEFT JOIN billchain on mrattend.groupcode = billchain.groupcode and mrattend.patientno = billchain.patientno LEFT JOIN pmedhdiag on mrattend.reference = pmedhdiag.reference" + rtnstring;
            selstring += " order by grouphead, trans_date, name";
            sdt = Dataaccess.GetAnytable("", "MR", selstring, false);
            string savedgh = "",ghname = "";
            foreach (DataRow row in sdt.Rows )
            {
                if (row["grouphead"].ToString().Trim() != savedgh )
                {
                    ghname = msmrfunc.GETGroupheadname("", row["grouphead"].ToString(), "C");
                    savedgh = row["grouphead"].ToString().Trim();
                }
                row["ghname"] = ghname;
                row["diagnosis"] = string.IsNullOrWhiteSpace(row["final"].ToString()) ? row["provisional"].ToString() : row["final"].ToString();
            }

        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateto.Value < dtDateFrom.Value || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateto.Value.Date > DateTime.Now.Date)
            {
                result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtgroupcode.Text) && string.IsNullOrWhiteSpace(txtgrouphead.Text))
            {
                result = MessageBox.Show("A Customer Code or Group Code must be specified...", "HMO/NHIS Encounter Form");
                return;
            }
            string rptfooter, rptcriteria; rptfooter = rptcriteria = "";
            //   fcgroup = false;
            Session["sql"] = "";

            sdt = new DataTable();
            ds = new DataSet();

            GetData();

            if (sdt.Rows.Count < 1)
            {
                MessageBox.Show("No Record...");
                return;
            }
            ds.Tables.Add(sdt);
            Session["rdlcfile"] = "EncounterForm.rdlc";
            mrpttype = "ENCOUNTER";
            string xstr = dtDateFrom.Value == dtDateto.Value ? dtDateFrom.Value.ToLongDateString() : dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString();
            mrptheader += dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString();

            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, rptfooter, rptcriteria, "", mrpttype, "", 0m, "", "", "", ds, true, 0, dtDateFrom.Value.Date, dtDateto.Value.Date, "", isprint, "", "");
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(this.Text, mrptheader, rptfooter, rptcriteria, "", mrpttype, "", 0m, "", "", "", ds, 0, dtDateFrom.Value.Date, dtDateto.Value.Date, "", isprint, true, "", "");
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            printprocess(false );
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printprocess(true);
        }
    }
}