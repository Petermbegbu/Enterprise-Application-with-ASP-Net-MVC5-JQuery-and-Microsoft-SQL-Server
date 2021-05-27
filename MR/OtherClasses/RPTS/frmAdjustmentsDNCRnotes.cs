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
    public partial class frmAdjustmentsDNCRnotes : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtadjusttype = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM AdjustmentCodes order by name", true);
        public frmAdjustmentsDNCRnotes()
        {
            InitializeComponent();
        }
        private void frmAdjustmentsDNCRnotes_Load(object sender, EventArgs e)
        {
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            cboAdjustType.DataSource = dtadjusttype;
            cboAdjustType.ValueMember = "type_code";
            cboAdjustType.DisplayMember = "Name";
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
            else if (btn.Name == "btnAdjustRef")
            {
                txtReference.Text = "";
                lookupsource = "ADJ";
                msmrfunc.mrGlobals.crequired = "ADJ";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED ADJUSTMENTS(DN/CR)";
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
            else if (lookupsource == "ADJ") //CORPORATE
            {
                txtReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtReference.Select();
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
        private void txtReference_LostFocus(object sender, EventArgs e)
        {
            DataTable dtadj = Dataaccess.GetAnytable("", "MR", "select name, amount, trans_date from bill_adj where reference = '" + txtReference.Text + "'", false);
            if (dtadj.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("Invalid Reference...");
                txtReference.Text = "";
                return;
            }
            txtName.Text = dtadj.Rows[0]["name"].ToString();
            nmramount.Value = (decimal)dtadj.Rows[0]["amount"];
            dtDateFrom.Value = dtDateTo.Value = (DateTime)dtadj.Rows[0]["trans_date"];
            txtgroupcode.Text = txtgrouphead.Text = txtpatientno.Text = cboAdjustType.Text = "";
        }
        void getData()
        {
            string bstr = "";
            bstr = " WHERE TRANS_DATE >= '" + dtDateFrom.Value.Date + "' AND TRANS_DATE <= '" + dtDateTo.Value.Date + "'";
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
                bstr += " AND GROUPHEAD = '" + txtgrouphead.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                bstr += " AND GHGROUPCODE = '" + txtgroupcode.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
                bstr += " AND GROUPHEAD = '" + txtpatientno.Text + "'";
            if (!string.IsNullOrWhiteSpace(cboAdjustType.Text))
                bstr += " and adjust = '" + cboAdjustType.SelectedValue.ToString() + "'";
            if (!string.IsNullOrWhiteSpace(txtReference.Text))
                bstr += " and reference = '" + txtReference.Text + "'";
 

            string xstring = "";
            //xstring = "SELECT [REFERENCE],[GROUPHEAD],[NAME],[TRANSTYPE],[ADJUST],[TTYPE],[AMOUNT],[COMMENTS],[TRANS_DATE],[GHGROUPCODE],[FACILITY],[CURRENCY],[EXRATE],[FCAMOUNT],char(50) AS ADJUSTNAME FROM BILL_ADJ " + bstr+" ORDER BY ADJUST";
            xstring = "SELECT [REFERENCE],[NAME],[TRANSTYPE],[ADJUST],[TTYPE],[AMOUNT],[COMMENTS],[TRANS_DATE],char(50) AS ADJUSTNAME FROM BILL_ADJ " + bstr + " ORDER BY ADJUST";
            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            string savedname = "", saveadj = "";

            foreach (DataRow row in sdt.Rows )
            {
                if (row["adjust"].ToString().Trim() != saveadj)
                {
                    saveadj = row["adjust"].ToString();
                    savedname = bissclass.combodisplayitemCodeName("type_code", saveadj, dtadjusttype, "name" );
                }
                row["adjustname"] = savedname;
            }
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
            if (sdt != null)
            {
                sdt.Rows.Clear();
                ds.Tables.Clear();
                ds.Clear();
            }
            getData();
            Session["rdlcfile"] = string.IsNullOrWhiteSpace(txtReference.Text) ? "AdjustmentDtl.rdlc" : "DNCRNotes.rdlc";
            string mrptheader = "ADJUSTMENTS (DN/CR) NOTES RECORDS";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader + " FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateTo.Value.ToShortDateString(), "", "", "", string.IsNullOrWhiteSpace(txtReference.Text) ? "ADJUSTMENTS" : "DN", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                //if (isprint)
                //    paedreports.work();
                //else
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader + " FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateTo.Value.ToShortDateString(), "", "", "", string.IsNullOrWhiteSpace(txtReference.Text) ? "ADJUSTMENTS" : "DN", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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