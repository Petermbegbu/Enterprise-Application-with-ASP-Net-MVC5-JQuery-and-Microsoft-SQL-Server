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
    public partial class frmOPDAttendance_BillSummary : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtcurrency = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM currencycodes order by name", true);
        public frmOPDAttendance_BillSummary()
        {
            InitializeComponent();
        }

        private void frmOPDAttendance_BillSummary_Load(object sender, EventArgs e)
        {
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            cboCurrency.DataSource = dtcurrency;
            cboCurrency.ValueMember = "type_code";
            cboCurrency.DisplayMember = "Name";
        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btngroupcode")
            {
                this.txtgroupcode.Text = "";
                lookupsource = "g";
                msmrfunc.mrGlobals.crequired = "g";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btnPatientno")
            {
                txtpatientno.Text = "";
                lookupsource = "L";
                msmrfunc.mrGlobals.crequired = "L";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btnCorporate")
            {
                txtgrouphead.Text = "";
                lookupsource = "C";
                msmrfunc.mrGlobals.crequired = "C";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORP. CLIENTS";
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
                this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
                this.txtgroupcode.Focus();
            }
            else if (lookupsource == "L") //patientno
            {
                txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Select();
            }
            else if (lookupsource == "C") //CORPORATE
            {
                txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtgrouphead.Select();
            }
            return;
        }
        private void txtPatientno_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpatientno.Text))
                return;
            if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
            {
                txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
            }
            //check if patientno exists
            bchain = billchaindtl.Getbillchain(txtpatientno.Text, txtgroupcode.Text);
            if (bchain == null)
            {
                DialogResult result = MessageBox.Show("Invalid Patient Number... ");
                txtpatientno.Text = " ";
                txtgroupcode.Select();
                return;
            }
            txtName.Text = bchain.NAME;
        }
        private void txtgrouphead_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtgrouphead.Text))
                return;
            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + txtgrouphead.Text + "'", false);
            if (dtcustomer.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("Invalid Corporate Clients Reference...");
                txtgrouphead.Text = "";
                return;
            }
            txtName.Text = dtcustomer.Rows[0]["name"].ToString();
        }
        void createSummary()
        {
            sdt = new DataTable(); //table to will be passed to report dataset 
            sdt.Columns.Add(new DataColumn("GROUPHTYPE", typeof(string)));
            sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
            sdt.Columns.Add(new DataColumn("PATIENTNO", typeof(string)));
            sdt.Columns.Add(new DataColumn("TRANS_DATE", typeof(DateTime)));
            sdt.Columns.Add(new DataColumn("VISITS", typeof(int)));
            sdt.Columns.Add(new DataColumn("AMOUNT", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("GHNAME", typeof(string)));
        }
        DataRow createnewRow(DataTable tdt, DataRow drow)
        {
            bool foundit = false;
            DataRow dr = null;

            foreach (DataRow row in sdt.Rows )
            {
                if (row["patientno"].ToString().Trim() == drow["patientno"].ToString().Trim() && row["name"].ToString().Trim() == drow["name"].ToString().Trim())
                {
                    if (row["trans_date"].ToString() != drow["trans_date"].ToString())
                        row["visits"] = (Int32)row["visits"] + 1;
                    row["amount"] = (decimal)row["amount"] + (decimal)drow["amount"];
                    foundit = true;
                    dr = row;
                    break;
                }
            }
            if (!foundit)
            {
                dr = sdt.NewRow();
                dr["grouphtype"] = drow["transtype"].ToString();
                dr["patientno"] = drow["patientno"].ToString();
                dr["NAME"] = drow["name"].ToString();
                dr["VISITS"] = 1;
                dr["AMOUNT"] = (decimal)drow["amount"];
                dr["GHNAME"] = "";
                sdt.Rows.Add(dr);
            }
            return dr;
        }

        void getData()
        {
            string bstr = "";
            bstr = " WHERE BILLING.TRANS_DATE >= '" + dtDateFrom.Value.Date + "' AND BILLING.TRANS_DATE <= '" + dtDateto.Value.Date + "' and ttype = 'D' ";
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
                bstr += " AND BILLING.GROUPHEAD = '" + txtgrouphead.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                bstr += " AND BILLING.GROUPCODE = '" + txtgroupcode.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
                bstr += " AND BILLING.PATIENTNO = '" + txtpatientno.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtNameSearch.Text))
                bstr += " AND NAME LIKE '%" + txtNameSearch.Text.Trim() + "%' ";
            if (chkCorporate.Checked)
                bstr += " transtype = 'C' ";
            if (chkPVTFamily.Checked)
                bstr += " transtype = 'P' ";
            //if (chkBoth.Checked)
            //    bstr += " transtype LIKE '[CP]' ";

            string xstring = "";
            xstring = "SELECT PATIENTNO, NAME, TRANS_DATE, TRANSTYPE, AMOUNT, GROUPHEAD, GROUPCODE, TTYPE, GHGROUPCODE FROM BILLING " + bstr+" order by grouphead, name";

            DataTable tsdt = Dataaccess.GetAnytable("", "MR", xstring, false);
          //  DataTable cdt = Dataaccess.GetAnytable("", "MR", "select name, custno from customer", false),
           //     pdt = Dataaccess.GetAnytable("", "MR", "select name, groupcode, patiento from patient", false);
            string savedname = "", savedgh = "";
            DataRow row,dtlr;
            //foreach (DataRow row in sdt.Rows)
            for (int i = 0; i < tsdt.Rows.Count; i++)
            {
                row = tsdt.Rows[i];
                dtlr = createnewRow(tsdt, row);

                if (row["grouphead"].ToString() != savedgh)
                {
                    savedgh = row["grouphead"].ToString();
                    msmrfunc.mrGlobals.anycode = "";
                    if (row["transtype"].ToString() == "C")
                    {
                        savedname = msmrfunc.GETGroupheadname(row["GHGROUPCODE"].ToString(), row["grouphead"].ToString(), row["transtype"].ToString());
                        dtlr["ghname"] = savedname;
                    }
                }
                if (row["transtype"].ToString() == "P" ) //&& row["grouphead"].ToString() == row["patientno"].ToString())
                    dtlr["ghname"] = "PRIVATE/FAMILY PATIENTS";
                else
                {
                    dtlr["ghname"] = savedname;
                    if (row["transtype"].ToString() == "C" && msmrfunc.mrGlobals.anycode == "YES")
                        dtlr["grouphtype"] = "H";
                }
            }
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            if (dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateto.Value < dtDateFrom.Value || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateto.Value.Date > DateTime.Now.Date )
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
            else
                createSummary();
            Session["rdlcfile"] = "opdAttendStatistics.rdlc";
            getData();
            ds.Tables.Add(sdt);
            string mrptheader = "PATIENTS ATTENDANCE/BILLING STATISTICS FOR PERIOD " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString();
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer("PATIENTS ATTENDANCE/BILLING STATISTICS", mrptheader, "", "", "", "OPDATTEND", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt("PATIENTS ATTENDANCE/BILLING STATISTICS", mrptheader, "", "", "", "OPDATTEND", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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