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
    public partial class CustomerBalancesDueReport : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date, start_opening;
        DataSet ds = new DataSet();
        DataTable sdt, dtcurrency = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM currencycodes order by name", true);
        public CustomerBalancesDueReport()
        {
            InitializeComponent();
        }

        private void CustomerBalancesDueReport_Load(object sender, EventArgs e)
        {
        
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
                msmrfunc.mrGlobals.crequired = "p";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED GROUPHEADS/PRIVATE PATIENTS";
            }
            else if (btn.Name == "btnPatientno")
            {
                txtpatientno.Text = "";
                lookupsource = "L";
                msmrfunc.mrGlobals.crequired = "P";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED GROUPHEADS/PRIVATE PATIENTS"; 
            }
            else if (btn.Name == "btngrouphead")
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
        bool getData()
        {

            string bstr = "", pstr = "", astr = "";
            bstr = " WHERE BILLING.TRANS_DATE >= '" + start_opening + "' and BILLING.TRANS_DATE <= '" + dtDateto.Value.Date + "'";
            pstr = " WHERE PAYDETAIL.TRANS_DATE >= '" + start_opening + "' and PAYDETAIL.TRANS_DATE <= '" + dtDateto.Value.Date + "'";
            astr = "WHERE BILL_ADJ.TRANS_DATE >= '" + start_opening + "' and BILL_ADJ.TRANS_DATE <= '" + dtDateto.Value.Date + "'";
            if (chkCorporate.Checked)
            {
                bstr = " AND BILLING.TRANSTYPE != 'C'";
                pstr = " AND PAYDETAIL.TRANSTYPE != 'C'";
                astr = " AND BILL_ADJ.TRANSTYPE != 'C'";
            }
            if (chkFamily.Checked)
            {
                bstr += " AND BILLING.TRANSTYPE != 'P'";
                pstr += " AND PAYDETAIL.TRANSTYPE != 'P'";
                astr += " AND BILL_ADJ.TRANSTYPE != 'P'";
            }
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
            {
                bstr += " AND BILLING.GROUPHEAD = '" + txtgrouphead.Text + "'";
                pstr += " AND PAYDETAIL.GROUPHEAD = '" + txtgrouphead.Text + "'";
                astr += " AND BILL_ADJ.GROUPHEAD = '" + txtgrouphead.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
            {
                bstr += " AND BILLING.GROUPCODE = '" + txtgroupcode.Text + "'";
                pstr += " AND PAYDETAIL.GROUPCODE = '" + txtgroupcode.Text + "'";
                pstr += " AND BILL_ADJ.GROUPCODE = '" + txtgroupcode.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
            {
                bstr += " AND BILLING.PATIENTNO = '" + txtpatientno.Text + "'";
                pstr += " AND PAYDETAIL.PATIENTNO = '" + txtpatientno.Text + "'";
                astr += " AND BILL_ADJ.GROUPHEAD = '" + txtpatientno.Text + "'";
            }
            
            start_opening = bissclass.ConvertStringToDateTime("01", msmrfunc.mrGlobals.mlastperiod == 12 ? "01" : msmrfunc.mrGlobals.mlastperiod + 1.ToString(), msmrfunc.mrGlobals.mlastperiod == 12 ? msmrfunc.mrGlobals.mpyear + 1.ToString() : msmrfunc.mrGlobals.mpyear.ToString());

            string xstring = "",xstr = "";
            if (chkCorporate.Checked)
                xstring = "select customer.custno AS grouphead, char(9) as ghgroupcode, customer.name, customer.balbf, address1, phone, email, posted from customer ";
            else if (chkFamily.Checked || chkPrivate.Checked)
            {
                xstr = chkFamily.Checked ? " and rtrim(patient.groupcode) = 'FC'" : " and rtrim(patient.groupcode) = 'PVT'";
                if (chkPrivate.Checked && !string.IsNullOrWhiteSpace(cboPVTNameFrom.Text))
                {
                    xstr += " and (left(patient.name,1) >= '" + cboPVTNameFrom.Text.Trim() + "' and patient.name <= '" + cboPVTNameTo.Text.Trim() + "')";
                }
                xstring = "select patient.groupcode AS ghgroupcode, patient.patientno As grouphead, patient.name, patient.balbf, address1, HOMEPHONE as phone, email, posted from patient where patient.isgrouphead = '1'" + xstr;
            }
            //else
            //    xstring = "select customer.custno AS grouphead, char(9) as ghgroupcode, customer.name, customer.balbf, address1, phone, email from customer UNION select patient.groupcode AS ghgroupcode, patient.patientno As grouphead, patient.name, patient.balbf, address1, phone, email from patient where patient.isgrouphead = '1'";
            if (chkAlphabetical.Checked)
                xstring += "ORDER BY NAME";
            else
                xstring += "ORDER BY GHGROUPCODE, GROUPHEAD";
            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            if (chkPrivate.Checked && sdt.Rows.Count > 1000 && (cboPVTNameFrom.Text.Trim() != cboPVTNameTo.Text.Trim() || string.IsNullOrWhiteSpace(cboPVTNameFrom.Text))
)            {
                MessageBox.Show("Private Patient Accounts are too many for Selected Criteria " + sdt.Rows.Count.ToString() + " Seen");
                return false;
            }
            DataTable ttd;
            xstr = "select Sum(amount) AS AMOUNT, GHGROUPCODE, grouphead from ";
            string xselstr = "";
            decimal db = 0, cr = 0, adj = 0;

            foreach (DataRow row in sdt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(txtgrouphead.Text) && row["grouphead"].ToString() != txtgrouphead.Text || !string.IsNullOrWhiteSpace(txtgroupcode.Text) && row["ghgroupcode"].ToString() != txtgroupcode.Text)
                    continue;
                if (!(bool)row["posted"])
                    db = Convert.ToDecimal(row["balbf"]);
                else
                    db = msmrfunc.getOpeningBalance(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "", string.IsNullOrWhiteSpace(row["ghgroupcode"].ToString()) ? "C" : "P", start_opening, dtDateto.Value.Date, ref db, ref cr, ref adj);
                cr = 0m;
                if (string.IsNullOrWhiteSpace(row["ghgroupcode"].ToString()))
                    xselstr = " where grouphead = '" + row["grouphead"].ToString() + "' and trans_date >= '" + start_opening.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + "' group by ghgroupcode, grouphead";
                else
                    xselstr = " where GHGROUPCODE = '" + row["ghgroupcode"].ToString() + "' and grouphead = '" + row["grouphead"].ToString() + "' and trans_date >= '" + start_opening.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + "' group by GHGROUPCODE, grouphead";

                ttd = Dataaccess.GetAnytable("", "MR", xstr+" billing "+xselstr, false);
                if (ttd.Rows.Count > 0)
                    db += (decimal)ttd.Rows[0]["amount"];
                ttd = Dataaccess.GetAnytable("", "MR", xstr + " paydetail " + xselstr, false);
                if (ttd.Rows.Count > 0)
                    cr = (decimal)ttd.Rows[0]["amount"];
                ttd = Dataaccess.GetAnytable("", "MR", xstr + " bill_adj " + xselstr, false);
                if (ttd.Rows.Count > 0)
                    db += (decimal)ttd.Rows[0]["amount"];
                row["balbf"] = db - cr;
            }
            //we must scan to check min. value
            if (nmrMinAmount.Value > 0 || !chkPrivate.Checked) //clone sdt and insert into new
            {
                DataTable newdt = sdt.Clone();
                foreach (DataRow row in sdt.Rows)
                {
                    if (nmrMinAmount.Value > 0 && Math.Abs(Convert.ToDecimal(row["balbf"])) < nmrMinAmount.Value || chkCreditBal.Checked && Convert.ToDecimal(row["balbf"]) >= 1 || chkDebitBal.Checked && Convert.ToDecimal(row["balbf"]) < 1)
                        continue;
                    newdt.ImportRow(row);
                }
                ds.Tables.Add(newdt);

            }
            else
                ds.Tables.Add(sdt);
            return true;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            if (dtDateto.Value > DateTime.Now.Date || dtDateto.Value < msmrfunc.mrGlobals.mta_start)
            {
                DialogResult result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            if (chkPrivate.Checked && (string.IsNullOrWhiteSpace(cboPVTNameFrom.Text) || string.IsNullOrWhiteSpace(cboPVTNameTo.Text)))
            {
                MessageBox.Show("Name Filter SHOULD be selected for Private Patients Reports");
                //return;
            }
            Session["sql"] = "";
            sdt = new DataTable();
            ds = new DataSet();

            if (!getData())
                return;
            Session["rdlcfile"] = "balancesdue_Alpha.rdlc"; // : "balancesdue_Ref";
            string mrptheader = "CUSTOMERS BALANCES DUE REPORT AS AT " + dtDateto.Value.ToShortDateString();
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer("BALANCES DUE REPORT", mrptheader, "", "", "", "BALANCEDUE", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                //if (isprint)
                //    paedreports.work();
                //else
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt("BALANCES DUE REPORT", mrptheader, "", "", "", "BALANCEDUE", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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

        private void chkPrivate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPrivate.Checked)
                panel_PVTFilter.Visible = true;
            else
                panel_PVTFilter.Visible = false;
        }
        
       
    }
}