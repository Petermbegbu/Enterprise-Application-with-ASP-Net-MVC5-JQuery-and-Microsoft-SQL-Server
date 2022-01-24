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
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmInpPrescriptiondtls
    {
        DataTable disp;
        string mreference, mgroupcode, mpatientno, lookupsource, AnyCode, datestr;
        bool isduenext;

        public frmInpPrescriptiondtls()
        {
            //InitializeComponent();
            //pharmacystkscreenid = 0;
            //isfromduenext = false;

            //string xstr = isfromduenext ? " - UNCHARTED PRESCRIPTIONS" : "";
            //this.Text += "Adm. Ref.:" + mreference.Trim() + " [" + name.Trim() + "]" + xstr;
            //isduenext = isfromduenext;
            //if (isduenext)
            //    panel_header.Enabled = chkUnprocessed.Checked = chkUnprocessed.Visible = false;
            //if (pharmacystkscreenid == 2 || mreference == "")
            //    btnAdd.Visible = true;
            //txtreferenceSU.Text = mreference;
        }

        //private void frmInpPrescriptiondtls_Load(object sender, EventArgs e)
        //{
        //    datestr = "";
        //    if (mreference != "")
        //        LoadDetails();
        //}

        //void LoadDetails()
        //{
        //    // mreference = "";
        //    if (isduenext)
        //        datestr = " posted = '0'";
        //    else
        //    {
        //        datestr = " trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
        //        if (chkUnprocessed.Checked)
        //            datestr += " and (phtransferred = '0' OR qty_pr != cumgv)";
        //    }
        //    if (!string.IsNullOrWhiteSpace(txtreferenceSU.Text))
        //        datestr += " and reference = '" + txtreferenceSU.Text + "'";
        //    //            if (mreference != "")
        //    disp = Dataaccess.GetAnytable("", "MR", "select trans_date, itemno, stk_item, stk_desc, qty_pr, cumgv, unit, cdose, cinterval, cduration, unitcost, cost, rx, doctor, stkbal, recid, posted, qty_gv, phtransferred, phqtytransferred, name, groupcode, patientno, unitpurvalue from inpdispensa where " + datestr + " order by trans_date", false);
        //    /*            else
        //                {
        //                    string xstr = mpatientno == "" ? " WHERE " : " where groupcode = '" + mgroupcode + "' and patientno = '" + mpatientno + "' and ";
        //                    disp = Dataaccess.GetAnytable("", "MR", "select trans_date, itemno, stk_item, stk_desc, qty_pr, cumgv, unit, cdose, cinterval, cduration, unitcost, cost, rx, doctor, stkbal, recid, posted, qty_gv, phtransferred, name, groupcode, patientno from inpdispensa " + xstr + " posted = '0' order by name, trans_date", false);
        //                }*/
        //    listView1.Items.Clear();
        //    if (disp.Rows.Count < 1)
        //    {
        //        MessageBox.Show("No Record...");
        //        if (string.IsNullOrWhiteSpace(txtreferenceSU.Text))
        //        {
        //            txtreferenceSU.Text = mreference;
        //            txtreferenceSU.Focus();
        //        }
        //        return;
        //    }
        //    int xrow = 0;
        //    string[] arr = new string[18];
        //    ListViewItem itm;
        //    foreach (DataRow row in disp.Rows)
        //    {
        //        arr[0] = row["trans_date"].ToString();
        //        arr[1] = row["itemno"].ToString();
        //        arr[2] = row["stk_desc"].ToString();
        //        arr[3] = row["qty_pr"].ToString();
        //        arr[4] = row["CUMGV"].ToString();
        //        arr[5] = row["phqtytransferred"].ToString();
        //        arr[6] = (Convert.ToDecimal(row["qty_pr"]) - Convert.ToDecimal(row["phqtytransferred"])).ToString();
        //        arr[7] = row["unit"].ToString();
        //        arr[8] = row["cdose"].ToString().Trim() + " x " + row["cinterval"].ToString().Trim() + " x " +
        //                    row["cduration"].ToString().Trim() + " : " + row["rx"].ToString().Trim();
        //        arr[9] = row["doctor"].ToString();
        //        arr[10] = row["unitcost"].ToString();
        //        arr[11] = row["cost"].ToString();
        //        arr[12] = row["stkbal"].ToString();
        //        arr[13] = row["stk_item"].ToString();
        //        arr[14] = row["qty_gv"].ToString();
        //        arr[15] = row["recid"].ToString();
        //        arr[16] = (bool)row["phtransferred"] ? "YES" : "NO";
        //        arr[17] = string.IsNullOrWhiteSpace(row["name"].ToString()) ? row["groupcode"].ToString().Trim() + ":" + row["patientno"].ToString() : row["name"].ToString();
        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);
        //        listView1.Items[xrow].UseItemStyleForSubItems = false;
        //        if (arr[16] == "YES")
        //        {
        //            bool isnil = Convert.ToDecimal(row["qty_pr"]) == Convert.ToDecimal(row["phqtytransferred"]) ? true : false;
        //            for (int i = 0; i < 17; i++) { listView1.Items[xrow].SubItems[i].BackColor = isnil ? Color.Red : Color.Yellow; }
        //        }
        //        xrow++;
        //    }
        //    if (string.IsNullOrWhiteSpace(txtreferenceSU.Text))
        //        btnAdd.Visible = false;
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    string xstring = chkCurrntAdmSu.Checked ? "[CURRENT ADMISSIONS]" : "[ALL]";
        //    if (btn.Name == "btnreferenceSu") //at SERVICE UPDATE
        //    {
        //        txtNameSu.Text = "";
        //        lookupsource = "ASU";
        //        msmrfunc.mrGlobals.crequired = "A";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED ADMISSIONS " + xstring;
        //        msmrfunc.mrGlobals.lookupCriteria = chkCurrntAdmSu.Checked ? "C" : "";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    msmrfunc.mrGlobals.lookupCriteria = "";
        //    if (lookupsource == "ASU") //AT SERVICE UPDATE
        //    {
        //        txtreferenceSU.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtreferenceSU.Focus();
        //    }
        //    return;
        //}

        //private void txtreferenceSU_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtreferenceSU.Text))
        //        return;
        //    if (string.IsNullOrWhiteSpace(AnyCode) && txtreferenceSU.Text.Substring(0, 1) != "A")
        //    {
        //        /* MessageBox.Show("Reference is not in the right Format...", "ADMISSION REFERENCE", msgBoxHandler);
        //         txtreferenceSU.Text = "";
        //         txtreferenceSU.Focus();
        //         return; */
        //        if (bissclass.IsDigitsOnly(txtreferenceSU.Text.Trim()))
        //            this.txtreferenceSU.Text = bissclass.autonumconfig(this.txtreferenceSU.Text, true, "A", "999999999");
        //    }
        //    //check if reference exist
        //    //string admreference,string groupcode,string patientno, string name
        //    AnyCode = "";
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select name, reference, groupcode, patientno, adm_date from admrecs where reference = '" + txtreferenceSU.Text + "'", false);
        //    if (dt.Rows.Count < 1)
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Admission Reference...", "ADMISSION DETAILS");
        //        txtreferenceSU.Text = "";
        //        return;
        //    }
        //    DataRow row = dt.Rows[0];
        //    txtNameSu.Text = row["name"].ToString();
        //    mreference = row["reference"].ToString();
        //    mgroupcode = row["groupcode"].ToString();
        //    mpatientno = row["patientno"].ToString();
        //    dtDateFrom.Value = Convert.ToDateTime(row["adm_date"]).Date;
        //    dtDateto.Value = DateTime.Now.Date;

        //}

        //private void btnLoad_Click(object sender, EventArgs e)
        //{
        //    //if (string.IsNullOrWhiteSpace(txtreferenceSU.Text))
        //    //  return;
        //    if (string.IsNullOrWhiteSpace(txtreferenceSU.Text))
        //        this.Text = "IN-PATIENT PRESCRIPTION DETAILS - Adm. Ref.:" + mreference.Trim() + " [" + txtNameSu.Text.Trim() + "]";
        //    else
        //        this.Text = "IN-PATIENT PRESCRIPTION DETAILS - ALL";

        //    LoadDetails();
        //}

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    if (listView1.Items.Count < 1)
        //        return;
        //    DataTable dtrtn = new DataTable();
        //    if (!chkUnprocessed.Checked)
        //    {
        //        dtrtn = disp.Clone();
        //        foreach (DataRow row in disp.Rows)
        //        {
        //            foreach (ListViewItem itm in listView1.SelectedItems)
        //            {
        //                if (row["stk_item"].ToString().Trim() == itm.SubItems[13].Text.Trim())
        //                {
        //                    dtrtn.ImportRow(row);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    Session["dispdtl"] = chkUnprocessed.Checked ? disp : dtrtn;
        //    btnClose.PerformClick();
        //}

    }
}