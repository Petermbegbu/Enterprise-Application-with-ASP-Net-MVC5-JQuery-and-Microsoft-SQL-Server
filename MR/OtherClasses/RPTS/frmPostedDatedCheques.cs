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
    public partial class frmPostedDatedCheques : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtcurrency = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM currencycodes order by name", true);
        public frmPostedDatedCheques()
        {
            InitializeComponent();
        }

        private void frmPostedDatedCheques_Load(object sender, EventArgs e)
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
            else if (btn.Name == "btnpatientno")
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

        void getData()
        {
            string pstr = "";
            pstr = " WHERE PAYDETAIL.TRANS_DATE >= '" + dtDateFrom.Value.Date + "' AND PAYDETAIL.TRANS_DATE <= '" + dtDateto.Value.Date + "'";
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
                pstr += " AND PAYDETAIL.GROUPHEAD = '" + txtgrouphead.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                pstr += " AND PAYDETAIL.GROUPCODE = '" + txtgroupcode.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
                pstr += " AND PAYDETAIL.PATIENTNO = '" + txtpatientno.Text + "'";

            if (!string.IsNullOrWhiteSpace(txtDescSearch.Text))
                pstr += " AND DESCRIPTION LIKE '%" + txtDescSearch.Text.Trim() + "%' ";
            if (!string.IsNullOrWhiteSpace(txtNameSearch.Text))
                pstr += " AND NAME LIKE '%" + txtNameSearch.Text.Trim() + "%' ";

            string xstring = "";
            xstring = "SELECT [REFERENCE], [PATIENTNO], [NAME], [ITEMNO], [DESCRIPTION], [AMOUNT], [TRANS_DATE], [TRANSTYPE], [GROUPHEAD], [SERVICETYPE], [GROUPCODE], [TTYPE], [GHGROUPCODE], [PAYTYPE], [OPERATOR], [OP_TIME], [ACCOUNTTYPE], [CURRENCY], [EXRATE], [FCAMOUNT], [EXTDESC], [DATERECEIVED], [CROSSREF], [PVTDEPOSIT], char(50) as ghname FROM PAYDETAIL " + pstr;

            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            DataTable dd = Dataaccess.GetAnytable("", "MR", "select name, custno from customer", false);
            foreach (DataRow row in sdt.Rows)
            {
                if (row["trans_date"].ToString() == "P")
                {
                    if (row["grouphead"].ToString() == row["patientno"].ToString())
                        row["ghname"] = row["name"].ToString();
                    else
                        row["ghname"] = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P");
                }
                else
                    row["ghname"] = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dd, "name");
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (dtDateFrom.Value <= DateTime.Now.Date || dtDateto.Value < dtDateFrom.Value )
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
            Session["rdlcfile"] = "PostDatedPayments.rdlc";
            string mrptheader = "POST-DATED CHEQUES";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader + " FOR : " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString(), "", "", "", "BILLS", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader + " FOR : " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString(), "", "", "", "BILLS", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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