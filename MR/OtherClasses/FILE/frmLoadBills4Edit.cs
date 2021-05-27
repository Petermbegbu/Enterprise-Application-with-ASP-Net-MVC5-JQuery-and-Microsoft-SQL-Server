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

using mradmin.Forms;
using mradmin.DataAccess;
using mradmin.BissClass;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace OtherClasses.FILE
{

    public partial class frmLoadBills4Edit
    {
        billchaindtl bchain = new billchaindtl();
        string woperator, lookupsource, AnyCode;
        bool fcgroup;
        public frmLoadBills4Edit(string xoperator)
        {
            //InitializeComponent();
            woperator = xoperator;
        }

        //private void frmLoadBills4Edit_Load(object sender, EventArgs e)
        //{
        //    DialogResult result = MessageBox.Show("Some Data element will be vulnerable on this process \r\n\r\n you are trying to run. \r\n\r\n You must therfore take full responsibility for your actions ! CONTINUE...?", "C A U T I O N ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.No)
        //    {
        //        btnClose.PerformClick();
        //        return;
        //    }

        //    string xval = "";
        //    POPREAD popread = new POPREAD("Restricted Access to Bills", "Enter Access Code", ref xval, true, false, "", "", "", false, "", "");
        //    popread.ShowDialog();
        //    xval = bissclass.sysGlobals.anycode;
        //    if (xval != "ALLbILLS")
        //    {
        //        MessageBox.Show("Invalid Access Code...");
        //        btnClose.PerformClick();
        //        return;
        //    }

        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btngroupcode")
        //    {
        //        this.txtgroupcode.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "g";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnPatientno")
        //    {
        //        txtpatientno.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btngrouphead")
        //    {
        //        txtgrouphead.Text = "";
        //        lookupsource = "C";
        //        msmrfunc.mrGlobals.crequired = "C";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORPORATE CLIENTS";
        //    }

        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "g") //groupcodee
        //    {
        //        this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
        //        this.txtgroupcode.Focus();
        //    }
        //    else if (lookupsource == "L") //patientno
        //    {
        //        txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtpatientno.Select();
        //    }
        //    else if (lookupsource == "C") //patientno
        //    {
        //        txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtgrouphead.Select();
        //    }

        //}

        //private void txtgroupcode_LostFocus(object sender, EventArgs e)
        //{
        //    txtpatientno.Focus();
        //}

        //private void txtPatientno_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtpatientno.Text))
        //        return;

        //    if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
        //    {
        //        txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
        //    }

        //    DialogResult result;
        //    //check if patientno exists
        //    bchain = billchaindtl.Getbillchain(txtpatientno.Text, txtgroupcode.Text);
        //    if (bchain == null)
        //    {
        //        result = MessageBox.Show("Invalid Patient Number... ");
        //        txtpatientno.Text = " ";
        //        txtgroupcode.Select();
        //        return;
        //    }
        //    lblname.Text = bchain.NAME;
        //    fcgroup = false;
        //    if (bchain.GROUPHEAD == bchain.PATIENTNO)
        //    {
        //        result = MessageBox.Show("Confirm to Load for All members of this Group/Family...", "Private/Grouphead Report Option", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.Yes)
        //            fcgroup = true;
        //    }
        //    dtDateFrom.Focus();
        //    return;
        //}

        //private void txtgrouphead_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtgrouphead.Text))
        //        return;

        //    DialogResult result;
        //    DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + txtgrouphead.Text + "'", false);
        //    if (dtcustomer.Rows.Count < 1)
        //    {
        //        result = MessageBox.Show("Invalid Corporate Clients Reference...");
        //        txtgrouphead.Text = "";
        //        return;
        //    }
        //    lblname.Text = dtcustomer.Rows[0]["name"].ToString();
        //    dtDateFrom.Focus();
        //}

        //private void btnLoad_Click(object sender, EventArgs e)
        //{
        //    DialogResult result;
        //    if (string.IsNullOrWhiteSpace(txtgrouphead.Text) && string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        result = MessageBox.Show("A Grouphead or Patient must be selected...");
        //        return;
        //    }
        //    if (dtDateFrom.Value.Month != dtDateto.Value.Month || dtDateFrom.Value.Year != dtDateto.Value.Year)
        //    {
        //        result = MessageBox.Show("Transaction Dates must be within SAME month and Year...", "Date Specification Error");
        //        return;
        //    }

        //    result = MessageBox.Show("Confirm to Load Bills...", "Patient Billing Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

        //    if (result == DialogResult.No)
        //        return;

        //    string rtnstring = " where trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999' AND posted = '0'";
        //    if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
        //        rtnstring += " and grouphead = '" + txtgrouphead.Text + "'";
        //    if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
        //    {
        //        if (fcgroup)
        //            rtnstring += " and ghgroupcode = '" + txtgroupcode.Text + "' and  grouphead = '" + txtpatientno.Text + "'";
        //        else
        //            rtnstring += " and groupcode = '" + txtgroupcode.Text + "' and patientno = '" + txtpatientno.Text + "'";
        //    }

        //    string selstr = "SELECT reference, patientno, name, itemno, diag, process, description, facility, amount, trans_date, grouphead, servicetype, groupcode, operator, ghgroupcode, op_time, accounttype, recid, RECEIPTED from billing" + rtnstring + " order by grouphead, ghgroupcode, name";

        //    DataTable dt = Dataaccess.GetAnytable("", "MR", selstr, false);
        //    if (dt.Rows.Count < 1)
        //    {
        //        result = MessageBox.Show("No Data...");
        //        return;
        //    }
        //    dataGridView1.Rows.Clear();
        //    int xc = 0;
        //    DataGridViewRow dgv;
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        dataGridView1.Rows.Add();
        //        dgv = dataGridView1.Rows[xc];

        //        dgv.Cells[0].Value = row["servicetype"].ToString();
        //        dgv.Cells[1].Value = row["reference"].ToString();
        //        dgv.Cells[2].Value = row["itemno"].ToString();
        //        dgv.Cells[3].Value = row["name"].ToString();
        //        dgv.Cells[4].Value = row["diag"].ToString();
        //        dgv.Cells[5].Value = row["facility"].ToString();
        //        dgv.Cells[6].Value = row["process"].ToString();
        //        dgv.Cells[7].Value = row["description"].ToString();
        //        dgv.Cells[8].Value = Convert.ToDecimal(row["amount"]).ToString("N2");
        //        dgv.Cells[9].Value = Convert.ToDateTime(row["trans_date"]).ToShortDateString();
        //        dgv.Cells[10].Value = Convert.ToBoolean(row["RECEIPTED"]) ? true : false;
        //        dgv.Cells[11].Value = row["groupcode"].ToString();
        //        dgv.Cells[12].Value = row["patientno"].ToString();
        //        dgv.Cells[13].Value = row["grouphead"].ToString();
        //        dgv.Cells[14].Value = row["accounttype"].ToString();
        //        dgv.Cells[15].Value = row["operator"].ToString();
        //        dgv.Cells[16].Value = row["op_time"].ToString();
        //        dgv.Cells[17].Value = row["ghgroupcode"].ToString();
        //        dgv.Cells[18].Value = row["recid"].ToString();
        //        dgv.Cells[19].Value = "";

        //        xc++;

        //        if (Convert.ToBoolean(row["RECEIPTED"]))
        //            dgv.ReadOnly = true;
        //    }

        //    btnApply.Enabled = true;

        //}

        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //        if (e.ColumnIndex == 0)
        //            cell.ToolTipText = "Enter <C>laims,<H>MO/<N>HIS Mthly Capitation or leave blank for capitation/others...";
        //        else if (e.ColumnIndex == 10)
        //            cell.ToolTipText = "Check (Click) To Flag This Bill processed and Locked to further Amendment...";
        //    }
        //}

        //private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridViewRow dgv = new DataGridViewRow();
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        dgv = dataGridView1.Rows[e.RowIndex];
        //        if (e.ColumnIndex == 0 && !string.IsNullOrWhiteSpace(dgv.Cells[0].FormattedValue.ToString()) && !new string[] { "C", "H", "N" }.Contains(dgv.Cells[0].Value.ToString()))
        //        {
        //            MessageBox.Show("Enter <C>laims,<H>MO/<N>HIS Mthly Capitation or leave blank for capitation/others...");
        //            return;
        //        }
        //        else if (e.ColumnIndex == 10)
        //        {
        //            //DialogResult result = MessageBox.Show("Confirm to Flag this bill(s) Processed and Locked", "Patient Billing Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            //if (result == DialogResult.Yes)
        //            //{
        //            bool xchecked = Convert.ToBoolean(dgv.Cells[10].Value);
        //            string xref = dgv.Cells[1].Value.ToString().Trim();
        //            foreach (DataGridViewRow xxdgv in dataGridView1.Rows)
        //            {
        //                if (xxdgv.Cells[1].Value.ToString() == xref)
        //                    xxdgv.Cells[10].Value = xchecked;
        //            }
        //        }
        //        dgv.Cells[19].Value = "Updated";
        //    }
        //}

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //do nothing
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void btnApply_Click(object sender, EventArgs e)
        //{
        //    if (dataGridView1.Rows.Count < 1)
        //        return;

        //    DialogResult result = MessageBox.Show("Confirm to Apply Changes...", "Patient Billing Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.No)
        //        return;
        //    //   btnApply.Enabled = false;

        //    int xcount = 0, xlocked = 0;
        //    string updstr = "", xref = "";
        //    foreach (DataGridViewRow dgv in dataGridView1.Rows)
        //    {
        //        if (dgv.Cells[0].Value == null || dgv.Cells[19].FormattedValue.ToString().Trim() != "Updated")
        //            continue;
        //        xlocked = Convert.ToBoolean(dgv.Cells[10].Value) ? 1 : 0;
        //        updstr = "update billing set servicetype = '" + dgv.Cells[0].Value.ToString() + "', reference = '" + dgv.Cells[1].Value.ToString() + "', itemno = '" + Convert.ToDecimal(dgv.Cells[2].Value) + "', amount = '" + Convert.ToDecimal(dgv.Cells[8].Value) + "', trans_date = '" + Convert.ToDateTime(dgv.Cells[9].Value) + "', accounttype = '" + dgv.Cells[14].Value.ToString() + "', operator = '" + woperator + "', op_time = '" + DateTime.Now + "', RECEIPTED = '" + xlocked + "' where recid = '" + Convert.ToInt32(dgv.Cells[18].Value) + "'";
        //        bissclass.UpdateRecords(updstr, "MR");
        //        xcount++;
        //    }
        //    if (xcount > 0) //we need to add up total bill by reference
        //    {
        //        DataTable dt = Dataaccess.GetAnytable("", "MR", "select reference, sum(amount) as amount from billing where grouphead = '" + txtgrouphead.Text + "' and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999' AND posted = '0' GROUP BY REFERENCE", false);
        //        xref = "";
        //        decimal xamt = 0m;
        //        foreach (DataGridViewRow dgv in dataGridView1.Rows)
        //        {
        //            if (dgv.Cells[0].Value == null || dgv.Cells[19].FormattedValue.ToString().Trim() != "Updated")
        //                continue;
        //            xlocked = Convert.ToBoolean(dgv.Cells[10].Value) ? 1 : 0;
        //            if (xlocked == 1 && dgv.Cells[1].Value.ToString().Trim() != xref)
        //            {
        //                xref = dgv.Cells[1].Value.ToString().Trim();
        //                xamt = getAMT(xref, dt);
        //                updstr = "update medhist set billed = 'Y', billref = '" + xref + "', amount = '" + xamt + "' where reference = '" + xref + "'";
        //                bissclass.UpdateRecords(updstr, "MR");
        //            }

        //            dgv.Cells[19].Value = "";
        //        }
        //    }

        //    MessageBox.Show("Update Completed Successfully... " + xcount + "  Record(s).");

        //}

        decimal getAMT(string xreference, DataTable xdt)
        {
            decimal rtnamt = 0m;
            foreach (DataRow row in xdt.Rows)
            {
                if (row["reference"].ToString() == xreference)
                {
                    rtnamt = Convert.ToDecimal(row["amount"]);
                    break;
                }

            }
            return rtnamt;
        }

        //private void txtgrouphead_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtgrouphead_LostFocus(null, null);
        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtgroupcode_LostFocus(null, null);
        //}

        //private void txtpatientno_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtPatientno_LostFocus(null, null);
        //}

        //private void dtDateFrom_KeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    if (objArgs.KeyCode == Keys.Enter)
        //    {
        //        SelectNextControl(ActiveControl, true, true, true, true);
        //    }
        //}
    }
}