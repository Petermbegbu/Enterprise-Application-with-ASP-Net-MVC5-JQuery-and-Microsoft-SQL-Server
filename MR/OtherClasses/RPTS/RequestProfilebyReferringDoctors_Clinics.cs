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
    public partial class RequestProfilebyReferringDoctors_Clinics : Form
    {
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt;
        DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "select name, type_code from servicecentrecodes", false),
            dtadjusttype = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM AdjustmentCodes order by name", false);
        public RequestProfilebyReferringDoctors_Clinics()
        {
            InitializeComponent();
        }
        private void RequestProfilebyReferringDoctors_Clinics_Load(object sender, EventArgs e)
        {

        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnReferrer")
            {
                txtReferrer.Text = "";
                lookupsource = "C";
                msmrfunc.mrGlobals.crequired = "C";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED REFERRERS";
            }
            else if (btn.Name == "btnTariff") //at registration
                {
                    txtProcedure.Text = "";
                    lookupsource = "T";
                    msmrfunc.mrGlobals.crequired = "T";
                    msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR DEFINED TARIFFS";
                }
            frmselcode FrmSelCode = new frmselcode();
            FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
            FrmSelCode.ShowDialog();
        }
        void FrmSelCode_Closed(object sender, EventArgs e)
        {
            frmselcode FrmSelcode = sender as frmselcode;
            if (lookupsource == "C") //REFERRERS
            {
                txtReferrer.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtReferrer.Select();
            }
            else if (lookupsource == "T")
            {
                txtProcedure.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtProcedure.Focus();
            }
            msmrfunc.mrGlobals.anycode = "";
            return;
        }
        private void txtgrouphead_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReferrer.Text))
                return;
            DialogResult result;
            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME,referrer FROM CUSTOMER WHERE CUSTNO = '" + txtReferrer.Text + "'", false);
            if (dtcustomer.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid Corporate Clients Reference...");
                txtReferrer.Text = "";
                return;
            }
            txtName.Text = dtcustomer.Rows[0]["name"].ToString();
            if (!Convert.ToBoolean( dtcustomer.Rows[0]["referrer"]))
            {
                result = MessageBox.Show("This Client is not defined as a Referrer...");
                txtReferrer.Text = "";
                return;
            }
        }
        void getData()
        {
            string xstring = "";
            if (!string.IsNullOrWhiteSpace(txtReferrer.Text))
                xstring = "select mrattend.reference, mrattend.referrer, char(50) AS ghname, suspense.groupcode, suspense.ghgroupcode, suspense.patientno, suspense.name, suspense.description, suspense.amount,  suspense.posted, suspense.facility, char(50) extdesc from mrattend INNER JOIN suspense on mrattend.reference = suspense.reference where mrattend.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and mrattend.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59:999' and mrattend.referrer = '" + txtReferrer.Text + "' order by referrer";
            else
                xstring = "select mrattend.reference, mrattend.referrer, char(50) AS ghname, suspense.groupcode, suspense.ghgroupcode, suspense.patientno, suspense.name, suspense.description, suspense.amount, suspense.posted, suspense.facility, char(50) extdesc from mrattend INNER JOIN suspense on mrattend.reference = suspense.reference where  mrattend.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and mrattend.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59:999' order by referrer";

            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            DataTable dtCustomers = Dataaccess.GetAnytable("", "MR", "SELECT NAME, custno FROM CUSTOMER WHERE referrrer = '1'",false);
            //GET MISC RECORDS
            DataTable dtbilladj = Dataaccess.GetAnytable("", "MR", "SELECT reference, comments, name, amount, grouphead, ttype, adjust FROM bill_adj WHERE trans_date >= '" + dtDateFrom.Value.Date + "' and trans_date <= '" + dtDateto.Value.Date + "'", false);
            DataRow xrow = null,newrow = null;
            bool foundit = false;
            foreach (DataRow row in dtbilladj.Rows ) //bill adjust
            {
                foundit = false;
                foreach (DataRow row1 in sdt.Rows) //transactons
                {
                    if (row["name"].ToString().Trim() == row1["name"].ToString().Trim() && row["grouphead"].ToString().Trim() == row1["grouphead"].ToString().Trim())
                    {
                        foundit = true;
                        xrow = row1;
                        break;
                    }
                }
                if (foundit)
                {
                    newrow = sdt.NewRow();
                    newrow["reference"] = row["reference"].ToString();
                    newrow["referrer"] = xrow["referrer"].ToString();
                    newrow["ghname"] = xrow["name"].ToString();
                    newrow["groupcode"] = xrow["groupcode"].ToString();
                    newrow["ghgroupcode"] = xrow["ghgroupcode"].ToString();
                    newrow["patientno"] = xrow["patientno"].ToString();
                    newrow["name"] = row["name"].ToString();
                    newrow["description"] = bissclass.combodisplayitemCodeName("type_code", row["adjust"].ToString(), dtadjusttype, "name")+ " - " +row["comments"].ToString();
                    if (xrow["ttype"].ToString() == "C")
                        newrow["amount"] = -(decimal)row["amount"];
                    else
                        newrow["amount"] = (decimal)row["amount"];
                    newrow["posted"] = true;
                    newrow["facility"] = xrow["facility"].ToString();
                    sdt.Rows.Add(newrow);
                }
            }
            string savedref, savedname; savedref = savedname = "";
            foreach (DataRow row in sdt.Rows)
            {
                if (string.IsNullOrWhiteSpace(row["referrer"].ToString()) || chkExcludeRequests.Checked && !(bool)row["posted"])
                {
                    row.Delete();
                    continue;
                }
                if (row["referrer"].ToString().Trim() != savedref )
                {
                    savedref = row["referrer"].ToString().Trim();
                    savedname = bissclass.combodisplayitemCodeName("custno", row["referrer"].ToString(), dtCustomers, "name");
                    row["ghname"] = savedname;
                }
                if ( chkGrpbyServiceCentre.Checked )
                    row["extdesc"] = bissclass.combodisplayitemCodeName("type_code", row["facility"].ToString(), dtfacility, "name");
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            if (dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateto.Value < dtDateFrom.Value || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateto.Value.Date > DateTime.Now.Date)
            {
                DialogResult result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            Session["sql"] = "";
            if (sdt != null)
            {
                sdt.Rows.Clear();
                ds.Tables.Clear();
                ds.Clear();
            }
            getData();
            Session["rdlcfile"] = chkGrpbyServiceCentre.Checked ? "ReferrerListingGrpd.rdlc" : "ReferrerListing.rdlc";
            string mrptheader = "REQUEST PROFILE BY REFERRING CLIENTS ";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader + " FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString(), "", "", "", "REFERRERS", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader + " FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString(), "", "", "", "REFERRERS", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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