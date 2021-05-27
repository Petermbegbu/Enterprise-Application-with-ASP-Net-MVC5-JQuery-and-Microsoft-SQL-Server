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

using mradmin.BissClass;
using mradmin.DataAccess;
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmBillTransfer
    {
        billchaindtl bchain = new billchaindtl();
        Customer customers = new Customer();
        patientinfo patients = new patientinfo();
        string lookupsource, anycode, anycode1, woperator;
        DataTable bills;

        public frmBillTransfer(decimal currentamt, DataTable dt, string xoperator)
        {
            //InitializeComponent();
            //nmrCurrentValue.Value = nmrValueToTransfer.Value = currentamt;

            bills = dt;
            woperator = xoperator;
        }

        //private void frmBillTransfer_Load(object sender, EventArgs e)
        //{

        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btngroupcode")
        //    {
        //        txtgroupcode.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "g";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnpatientlookup")
        //    {
        //        txtPatientno.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnpayreference")
        //    {
        //        combPmtNo.Text = "";
        //        lookupsource = "PAY";
        //        msmrfunc.mrGlobals.crequired = "PAY";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR SAVED PAYMENT RECORDS";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "g") //groupcodee
        //    {
        //        txtgroupcode.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        txtPatientno.Text = anycode1 = msmrfunc.mrGlobals.anycode1;
        //        txtgroupcode.Focus();
        //    }
        //    else if (lookupsource == "L") //patientno
        //    {
        //        txtgroupcode.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        txtPatientno.Text = anycode1 = msmrfunc.mrGlobals.anycode1;
        //        txtPatientno.Focus();
        //    }
        //    else if (lookupsource == "PAY")
        //    {
        //        combPmtNo.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        combPmtNo.Focus();
        //    }
        //}

        //private void txtPatientno_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtPatientno.Text))
        //        return;

        //    if (string.IsNullOrWhiteSpace(anycode))
        //    {
        //        txtPatientno.Text = bissclass.autonumconfig(txtPatientno.Text, true, "", "9999999");
        //    }

        //    bchain = billchaindtl.Getbillchain(txtPatientno.Text, txtgroupcode.Text);
        //    if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO))
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Patient Number... ");
        //        txtPatientno.Text = "";
        //        return;
        //    }
        //    bool xfoundit = true;
        //    if (bchain.GROUPHTYPE == "P")
        //    {
        //        patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);
        //        if (patients == null)
        //        {
        //            xfoundit = false;
        //        }
        //    }
        //    else
        //    {
        //        customers = Customer.GetCustomer(bchain.GROUPHEAD);
        //        if (customers == null)
        //            xfoundit = false;
        //    }
        //    if (!xfoundit)
        //    {
        //        DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS");
        //        txtPatientno.Text = "";
        //        return;
        //    }
        //    txtName.Text = bchain.NAME;
        //    txtGrouphead.Text = (bchain.GROUPHTYPE == "P" && bchain.GROUPHEAD == patients.patientno) ?
        //        "< SELF >" : bchain.GROUPHTYPE == "C" ? customers.Name : patients.name;
        //    anycode = anycode1 = "";
        //    btnTransfer.Focus();
        //}

        //private void btnTransfer_Click(object sender, EventArgs e)
        //{
        //    if (nmrValueToTransfer.Value < 1 || nmrValueToTransfer.Value > nmrCurrentValue.Value || chkBills.Checked == false && chkPayments.Checked == false)
        //    {
        //        MessageBox.Show("Check Value to Transfer...");
        //        return;
        //    }

        //    string xstr = chkTransfer.Checked ? "Confirm to Transfer Bills..." : "Confirm to Move Bills...",
        //        xstr2 = chkTransfer.Checked ? "Transfer" : "Move";
        //    DialogResult result = MessageBox.Show(xstr, "Bills/Pmts " + xstr2, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    if (result == DialogResult.No)
        //        return;

        //    string updstr = "";
        //    if (chkTransfer.Checked)
        //    {
        //        decimal mlastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, true, 0, false) + 1;
        //        string xref = bissclass.autonumconfig(mlastno.ToString(), true, "", "999999999");
        //        Billings.writeBILLS(true, xref, 1, bills.Rows[0]["process"].ToString(), "Bills Transferred from :" + bills.Rows[0]["reference"].ToString(), bchain.GROUPHTYPE, nmrValueToTransfer.Value, (DateTime)bills.Rows[0]["trans_date"], bchain.NAME, bchain.GROUPHEAD, bills.Rows[0]["facility"].ToString(), txtgroupcode.Text, txtPatientno.Text, "D", bchain.GHGROUPCODE, woperator, DateTime.Now, "", "", 0m, 0, bills.Rows[0]["diag"].ToString(), bills.Rows[0]["doctor"].ToString(), false, "", "", 0m, "", 0m, "O", false, 0);

        //        mlastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, true, 0, false) + 1;
        //        xref = bissclass.autonumconfig(mlastno.ToString(), true, "", "999999999");
        //        Billings.writeBILLS(true, xref, 1, bills.Rows[0]["process"].ToString(), "Transferred Bills To : :" + txtgroupcode.Text.Trim() + ":" + txtPatientno.Text.Trim(), bchain.GROUPHTYPE, -nmrValueToTransfer.Value, (DateTime)bills.Rows[0]["trans_date"], bills.Rows[0]["name"].ToString(), bills.Rows[0]["grouphead"].ToString(), bills.Rows[0]["facility"].ToString(), bills.Rows[0]["groupcode"].ToString(), bills.Rows[0]["patientno"].ToString(), "D", bills.Rows[0]["ghgroupcode"].ToString(), woperator, DateTime.Now, "", "", 0m, 0, bills.Rows[0]["diag"].ToString(), bills.Rows[0]["doctor"].ToString(), false, "", "", 0m, "", 0m, "O", false, 0);
        //    }
        //    else
        //    {
        //        if (chkBills.Checked)
        //        {
        //            updstr = "update billing set groupcode = '" + txtgroupcode.Text + "', patientno = '" + txtPatientno.Text + "', ghgroupcode = '" + bchain.GHGROUPCODE + "', grouphead = '" + bchain.GROUPHEAD + "', name = '" + bchain.NAME + "' where reference = '" + bills.Rows[0]["reference"].ToString() + "'";
        //            bissclass.UpdateRecords(updstr, "MR");
        //        }
        //        if (chkPayments.Checked && !string.IsNullOrWhiteSpace(combPmtNo.Text) && nmrPayments.Value > 0)
        //        {
        //            updstr = "update paydetail set groupcode = '" + txtgroupcode.Text + "', ghgroupcode = '" + bchain.GHGROUPCODE + "', grouphead = '" + bchain.GROUPHEAD + "', name = '" + bchain.NAME + "' where reference = '" + combPmtNo.Text + "'";
        //            bissclass.UpdateRecords(updstr, "MR");
        //        }
        //    }
        //    MessageBox.Show("Completed...", "Bills Transfer");
        //    btnClose.PerformClick();
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void combPmtNo_LostFocus(object sender, EventArgs e)
        //{
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select amount, name from paydetail where reference = '" + combPmtNo.Text + "'", false);

        //    if (dt.Rows.Count < 1)
        //    {
        //        MessageBox.Show("Invalid Payment Reference...");
        //        combPmtNo.Text = "";
        //        return;
        //    }
        //    nmrPayments.Value = Convert.ToDecimal(dt.Rows[0]["amount"]);
        //}
    }

}