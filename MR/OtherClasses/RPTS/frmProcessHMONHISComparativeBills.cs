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
    public partial class frmProcessHMONHISComparativeBills : Form
    {
        Customer customer = new Customer();
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter(), woperator;
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, bills;
        public frmProcessHMONHISComparativeBills(string xoperator)
        {
            InitializeComponent();
            woperator = xoperator;
        }
        private void frmProcessHMONHISComparativeBills_Load(object sender, EventArgs e)
        {
            
        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnCustomer")
            {
                txtCustomer.Text = "";
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
            if (lookupsource == "C") //CORPORATE
            {
                txtCustomer.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtCustomer.Select();
            }
            return;
        }
        private void txtgrouphead_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomer.Text))
                return;
            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + txtCustomer.Text + "'", false);
            if (dtcustomer.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("Invalid Corporate Clients Reference...");
                txtCustomer.Text = "";
                return;
            }
            txtName.Text = dtcustomer.Rows[0]["name"].ToString();
        }

        void getData()
        {
            string bstr = "", pstr = "";
            bstr = " WHERE CAPBILLS.TRANS_DATE >= '" + dtDateFrom.Value.Date + "' AND CAPBILLS.TRANS_DATE <= '" + dtDateto.Value.Date + "'";
            pstr = " WHERE PAYDETAIL.TRANS_DATE >= '" + dtDateFrom.Value.Date + "' AND PAYDETAIL.TRANS_DATE <= '" + dtDateto.Value.Date + "'";
            if (!string.IsNullOrWhiteSpace(txtCustomer.Text ))
            {
                pstr += " AND PAYDETAIL.GROUPHEAD = '" + txtCustomer.Text + "'";
                bstr += " AND CAPBILLS.GROUPHEAD = '" + txtCustomer.Text + "'";
            }
 
            pstr += " AND ttype = 'C AND servicetyp LIKE '[HN]'";
            string xstring = "";
                xstring = "SELECT CAPBILLS.REFERENCE, CAPBILLS.PATIENTNO, CAPBILLS.NAME, CAPBILLS.ITEMNO, CAPBILLS.DESCRIPTION, CAPBILLS.AMOUNT, CAPBILLS.TRANS_DATE, CAPBILLS.TRANSTYPE, CAPBILLS.GROUPHEAD, CAPBILLS.SERVICETYP, CAPBILLS.GROUPCODE, CAPBILLS.TTYPE, CAPBILLS.GHGROUPCODE, CAPBILLS.EXTDESC, CAPBILLS.ACCOUNTTYPE, CHAR(50) AS COMPNAME FROM CAPBILLS " + bstr + " UNION SELECT PAYDETAIL.AMOUNT, PAYDETAIL.REFERENCE, PAYDETAIL.DESCRIPTION, PAYDETAIL.TRANS_DATE, PAYDETAIL.TRANSTYPE, PAYDETAIL.GROUPHEAD, PAYDETAIL.SERVICETYP, PAYDETAIL.GROUPCODE, PAYDETAIL.TTYPE, PAYDETAIL.GHGROUPCODE FROM PAYDETAIL " + pstr;

            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            DataTable dd = Dataaccess.GetAnytable("", "MR", "select name, custno from customer", false);
            foreach (DataRow row in sdt.Rows)
            {
                row["company"] = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dd, "name");
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            if (string.IsNullOrWhiteSpace(txtCustomer.Text))
            {
                DialogResult result = MessageBox.Show("You must Select a Company or HMO for this Report...", "Report Query Criteria Alert !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    return;
            }
            Session["sql"] = "";
            getData();
            string mrptheader = "HMO-NHIS COMPARATIVE CAPITATION/PAYMENTS FOR TRANSACTION PERIOD " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString();
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader, "", "", "", "BILLS", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader, "", "", "", "BILLS", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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

        private void txtCustomer_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomer.Text))
                return;
            DialogResult result;
            customer = Customer.GetCustomer(txtCustomer.Text);
            if (customer == null)
            {
                result = MessageBox.Show("Invalid Customer Code....", "Customer Definition");
                txtCustomer.Text = "";
                return;
            }
            txtGrpName.Text = customer.Name;

            //if (customer.Custstatus == "D")
            //{
            //    result = MessageBox.Show("This Customer's record has been flagged domant... NO UPDATE ALLOWED !", "Customer Master File");
            //    txtCustomer.Text = "";
            //    return;
            //}
            if (!customer.HMO)
            {
                result = MessageBox.Show("This Customer's is not Registered as an HMO !", "Customer Master File");
                txtCustomer.Text = "";
                return;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirm to Load Bills for Specified HMO/Dates...", "TAG HMO Bills", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
                return;

            LoadBills();
        }
        void LoadBills()
        {
            string rtnstring = "select * from CAPBILLS where grouphead = '" + txtCustomer.Text + "' and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            if (!string.IsNullOrWhiteSpace(txtName.Text))
                rtnstring += " and name like '%" + txtName.Text.Trim() + "%'";

            rtnstring += " order by trans_date, name";
            dataGridView1.Rows.Clear();
            nmrTotal.Value = 0;
            DataRow row;
            DataGridViewRow grow;
            bills = Dataaccess.GetAnytable("", "MR", rtnstring, false);
            if (bills.Rows.Count < 1)
            {
                MessageBox.Show("No Data...");
                return;
            }
            for (int i = 0; i < bills.Rows.Count; i++)
            {
                row = bills.Rows[i];
                dataGridView1.Rows.Add();
                grow = dataGridView1.Rows[i];
                grow.Cells[0].Value = Convert.ToDateTime(row["trans_date"]).ToShortDateString();
                grow.Cells[1].Value = row["REFERENCE"].ToString();
                grow.Cells[2].Value = row["itemno"].ToString();
                grow.Cells[3].Value = Convert.ToDecimal(row["amount"]).ToString("N2");
                grow.Cells[4].Value = Convert.ToBoolean(row["RECEIPTED"]) ? true : false;
                grow.Cells[5].Value = row["description"].ToString();
                grow.Cells[6].Value = row["GROUPCODE"].ToString();
                grow.Cells[7].Value = row["PATIENTNO"].ToString();
                grow.Cells[8].Value = row["NAME"].ToString();
                grow.Cells[9].Value = row["diag"].ToString();
                grow.Cells[10].Value = row["facility"].ToString();
                grow.Cells[11].Value = ""; //updated
                grow.Cells[12].Value = row["recid"].ToString();
                grow.Cells[13].Value = i.ToString();

                nmrTotal.Value += Convert.ToDecimal(row["amount"]);
            }
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //=MESSAGEBOX("Value must be <C>laims,<H>MO/<N>HIS Mthly Capitation or leave blank for Capitation...",0+64+0,"Bill Type...")
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == 4)
                    cell.ToolTipText = "Check (Click) To unFlag This Bill and make Active to further Amendment...";
            }
        }
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == 2)
            //   initVal = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim();
        }
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgv = new DataGridViewRow();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && (e.ColumnIndex < 7 || e.ColumnIndex == 11))
            {
                dgv = dataGridView1.Rows[e.RowIndex];
                if (e.ColumnIndex == 4 && !string.IsNullOrWhiteSpace(dgv.Cells[4].FormattedValue.ToString()))
                {
                    dgv.Cells[11].Value = "UPDATED";
                }
             
            }
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //do nothing
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirm to Apply Changes made...", "HMO BILL Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
                return;
            int xr = 0, ix;
            //string savedstring; // = "update CAPBILLS set servicetype = '", updatestring;
            DataGridViewRow dgv;
            //foreach (DataGridViewRow row in dataGridView1.Rows)

            DataRow row;
            string updstr = "";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dgv = dataGridView1.Rows[i];
                if (dgv.Cells[0].FormattedValue.ToString() == "" || dgv.Cells[11].Value.ToString().Trim() != "UPDATED")
                    continue;
                ix = Convert.ToInt32(dgv.Cells[13].Value);
                row = bills.Rows[ix];
                Billings.writeBILLS(true, row["reference"].ToString(), Convert.ToDecimal(row["itemno"]), row["process"].ToString(), row["description"].ToString(), row["transtype"].ToString(), Convert.ToDecimal(row["amount"]), Convert.ToDateTime(row["trans_date"]), row["name"].ToString(), row["grouphead"].ToString(), row["facility"].ToString(), row["groupcode"].ToString(), row["patientno"].ToString(), row["ttype"].ToString(), row["ghgroupcode"].ToString(), woperator, DateTime.Now, row["EXTDESC"].ToString(), row["currency"].ToString(), Convert.ToDecimal(row["exrate"]), 0, row["diag"].ToString(), row["doctor"].ToString(), false, "", row["servicetype"].ToString(), 0m, "", 0m, row["reference"].ToString().Substring(0, 1) == "A" ? "I" : "O", false, 0);
                xr++;
                //delete from capbills
                updstr = "delete from capbills where recid = '" + row["recid"].ToString() + "'";
                bissclass.UpdateRecords(updstr, "MR");
                dgv.Cells[11].Value = dgv.Cells[0].Value = "";

                // dgv.Cells[11].Value = "";
            }
            MessageBox.Show("Completed... " + xr.ToString() + " Processed !");

        }
    }
}