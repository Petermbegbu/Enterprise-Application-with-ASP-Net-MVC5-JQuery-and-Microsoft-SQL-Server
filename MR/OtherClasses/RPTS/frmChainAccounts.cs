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
    public partial class frmChainAccounts : Form
    {
        string rptcriteria, rptfooter = bissclass.getRptfooter();
        DataSet ds = new DataSet();
        DataTable sdt = new DataTable();
        string lookupsource, anycode;
        public frmChainAccounts()
        {
            InitializeComponent();
        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btngroupcode")
            {
                txtGroupcode.Text = "";
                lookupsource = "g";
                msmrfunc.mrGlobals.crequired = "g";
                msmrfunc.mrGlobals.lookupCriteria = txtGroupcode.Text;
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btngrouphead")
            {
                txtGrouphead.Text = "";
                lookupsource = chkCorporate.Checked ? "C" : "L";
                msmrfunc.mrGlobals.crequired = chkCorporate.Checked ? "C" : "L";
                msmrfunc.mrGlobals.lookupCriteria = chkCorporate.Checked ? "" : txtGroupcode.Text;
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORPORATE CLIENTS";
            }
            frmselcode FrmSelCode = new frmselcode();
            FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
            FrmSelCode.ShowDialog();
        }
        void FrmSelCode_Closed(object sender, EventArgs e)
        {
            frmselcode FrmSelcode = sender as frmselcode;
            if (lookupsource == "g") //groupcodee
            {
                txtGroupcode.Text = anycode = msmrfunc.mrGlobals.anycode;
                txtGrouphead.Text = msmrfunc.mrGlobals.anycode1;
                txtGroupcode.Focus();
            }
            else if (lookupsource == "L" || lookupsource == "C") 
            {
                txtGrouphead.Text = anycode = msmrfunc.mrGlobals.anycode;
                txtGrouphead.Select();
            }

        }
        void getData()
        {
            string selstring = " name != ''",addselect = "";
            if (chkByDateRange.Checked)
            {
                selstring += " AND reg_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and reg_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                rptcriteria = "For Reg. Date : " + dtDateFrom.Value.ToShortDateString() + " and " + dtDateto.Value.ToShortDateString();
            }
            if (chkCorporate.Checked) // chkGroupcode.Checked)
            {
               // selstring = string.IsNullOrWhiteSpace(selstring) ? "" : " AND ";
                selstring += " AND Grouphtype = 'C'";
                rptcriteria += "Group = Corporate";
            }
            else if (chkPVTFamily.Checked)
            {
                //selstring = string.IsNullOrWhiteSpace(selstring) ? "" : " AND ";
                selstring += " AND Grouphtype != 'C'";
                rptcriteria += "Group = Private/Families";
            }
            if (!string.IsNullOrWhiteSpace(txtGrouphead.Text))
            {
                //selstring = string.IsNullOrWhiteSpace(selstring) ? "" : " AND ";
                selstring += " AND Grouphead = '" + txtGrouphead.Text + "'";
                rptcriteria += "GroupHead = " + txtGrouphead.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtGroupcode.Text)) // chkGroupcode.Checked)
            {
                //selstring = string.IsNullOrWhiteSpace(selstring) ? "" : " AND ";
                selstring += " AND groupcode = '" + txtGroupcode.Text + "'";
                rptcriteria += "GroupCode = " + txtGroupcode.Text;
            }
            if (chkHMOonly.Checked)
            {
                //selstring = string.IsNullOrWhiteSpace(selstring) ? "" : " AND ";
                selstring = " HMOSERVTYPE != ''";
                if (chkActiverecs.Checked)
                    selstring += " and status = 'A'";
                else if (chkInactiverecs.Checked)
                    selstring += " and status != 'A'";
                string xstr = chkActiverecs.Checked ? "Active Records " : chkInactiverecs.Checked ? "Inactive Records" : "All Records";
                rptcriteria = "HMO Patients - " + xstr;
            }
            if (!string.IsNullOrWhiteSpace(txtNameSearch.Text))
                selstring += " AND NAME LIKE '%" + txtNameSearch.Text.Trim() + "%' ";
            if (chkAuditProfile.Checked )
                 addselect = ", [PICLOCATION] ";
            if (!string.IsNullOrWhiteSpace(selstring))
                selstring = " WHERE " + selstring;

            string xstring = "SELECT [GROUPCODE], [PATIENTNO], [GROUPHEAD], [NAME], [REG_DATE], [GROUPHTYPE], [STAFFNO],[RELATIONSH], [SECTION], [DEPARTMENT], [STATUS], [SEX], [M_STATUS], [RESIDENCE], [GHGROUPCODE], [HMOSERVTYPE],[BILLONACCT], [PHONE], [EMAIL], [PATIENTNO_PRINCIPAL], [M_STATUS], [BIRTHDATE], CHAR(50) AS GHNAME, PATCATEG" + addselect + " from BILLCHAIN " + selstring;

            if (chkPatientName.Checked)
            {
                if (chkGrouphead.Checked)
                    xstring += " ORDER BY GROUPHEAD, NAME";
                else if (chkGroupcode.Checked)
                    xstring += " ORDER BY GROUPCODE, NAME";
                else
                    xstring += " ORDER BY NAME";
            }
            else if (chkPatientNumberUngrouped.Checked)
                xstring += " ORDER BY patientno";
            else
                xstring += " ORDER BY GHGROUPCODE, GROUPHEAD, PATIENTNO";

            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            string ghsaved, gh, ghgc; ghsaved = gh = ghgc = "";
           // if (chkGrouphead.Checked)
            if (chkPatientNumberGrouped.Checked)
            {
                foreach (DataRow row in sdt.Rows )
                {
                    if (row["ghgroupcode"].ToString() == ghgc && row["grouphead"].ToString() == gh)
                        row["ghname"] = ghsaved;
                    else
                    {
                        ghsaved = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString().Trim(), row["grouphtype"].ToString());
                        row["ghname"] = ghsaved;
                        gh = row["grouphead"].ToString(); ghgc = row["ghgroupcode"].ToString();
                    }
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
            if (chkByDateRange.Checked && dtDateFrom.Value > DateTime.Now.Date || dtDateto.Value < dtDateFrom.Value)
            {
                result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            if (chkAuditProfile.Checked && string.IsNullOrWhiteSpace(txtGrouphead.Text))
            {
                result = MessageBox.Show("Groupcode must be specified for Audit Report Otpion ...");
                return;
            }
            if (chkPatientNumberUngrouped.Checked)
                chkGroupcode.Checked = chkGrouphead.Checked = chkCorporate.Checked = chkPVTFamily.Checked = false;
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
            //if (chkGrouphead.Checked )
            if (chkPatientNumberGrouped.Checked)
                Session["rdlcfile"] = "GrpChainByGpHead.rdlc";
            else if (chkAuditProfile.Checked)
                Session["rdlcfile"] = "GrpChainAudit.rdlc";
            else
                Session["rdlcfile"] = "GrpChainList.rdlc";
            string mrptheader = "GROUP/CHAIN ACCOUNT LISTING";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader, rptfooter, rptcriteria, "", "CHAINACCT", "", 0m, "", "", "", ds, true, 0, dtDateFrom.Value, dtDateto.Value, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader, rptfooter, rptcriteria, "", "CHAINACCT", "", 0m, "", "", "", ds, 0, dtDateFrom.Value, dtDateto.Value, "", isprint, true, "", "");
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

        private void chkPVTFamily_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkPVTFamily.Checked)
            //    txtGroupcode.Enabled = btngroupcode.Enabled = true;
            //else
            //    txtGroupcode.Enabled = btngroupcode.Enabled = false;
        }

        private void txtGrouphead_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGrouphead.Text))
                return;
            string xfile = chkPVTFamily.Checked ? "patient where groupcode = '"+txtGroupcode.Text+"' and patientno = '"+txtGrouphead.Text+"'" : "customer where custno = '"+txtGrouphead.Text+"'";
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select name from " + xfile, false);
            if (dt.Rows.Count < 1)
            {
                string xstr = chkPVTFamily.Checked ? "patient" : "customer";
                MessageBox.Show("Invalid "+xstr+" Reference...");
                txtGrouphead.Text = txtGroupcode.Text = "";
                return;
            }
        }

        private void chkHMOonly_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHMOonly.Checked)
                chkAllrecs.Checked = true;
            else
                chkAllrecs.Checked = false;
        }

        private void chkGroupcode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbn = sender as RadioButton;
            if (rbn.Name == "chkGroupcode" && chkGroupcode.Checked)
            {
                txtGroupcode.Enabled = btngroupcode.Enabled = true;
                txtGrouphead.Enabled = btngrouphead.Enabled = false;
            }
            else
            {
                txtGroupcode.Enabled = btngroupcode.Enabled = false;
                txtGrouphead.Enabled = btngrouphead.Enabled = true;
            }
        }
    }
}