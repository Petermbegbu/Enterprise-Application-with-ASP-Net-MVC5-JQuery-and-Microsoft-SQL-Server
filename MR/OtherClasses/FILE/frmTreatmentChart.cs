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
using SCS.DataAccess;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmTreatmentChart
    {
        DataTable chart, phs = Dataaccess.GetAnytable("", "SMS", "SELECT station from smcontrol where recid = '2'", false),
            stkitems, nursestable = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RTRIM(RECTYPE) = 'N' order by name", true),
            store = Dataaccess.GetAnytable("", "SMS", "SELECT STORECODE, NAME FROM STORE order by name", true), dtInpPrescriptn;

        string pharmacystore;
        int recno;
        Admrecs Admrecs;
        bool medhistwritten, iscostcentrestores;
        MR_DATA.MR_DATAvm vm;

        public frmTreatmentChart(MR_DATA.MR_DATAvm VM2)
        {
            //InitializeComponent();
            //txtreference.Text = admrec.REFERENCE;
            //txtgroupcode.Text = admrec.GROUPCODE;
            //txtpatientno.Text = admrec.PATIENTNO;
            //txtname.Text = admrec.NAME;
            //txtdiagnosis.Text = admrec.DIAGNOSIS;
            //txtadm_dateSu.Text = admrec.ADM_DATE.ToString();
            //txtbedSU.Text = admrec.BED;
            //txtWardRmSu.Text = admrec.ROOM;
            //lblfaciitySu.Text = admrec.FACILITY;
            //Admrecs = admrec;

            vm = VM2;

            pharmacystore = phs.Rows[0]["station"].ToString();
            //if (string.IsNullOrWhiteSpace(pharmacystore))
            //{
            //    DialogResult result = MessageBox.Show("Pharmacy Store Not Defined in Stock Database...\r\n Can't process requests on this platform!", "Control Data Definition Error");
            //    //btnClose.PerformClick();
            //}

            stkitems = Dataaccess.GetAnytable("", "SMS",
                "SELECT DISTINCT NAME, ITEM FROM STOCK where status != 'D' and store = '" + pharmacystore + "' order by name", true);

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select facilauto from mrcontrol where recid = '5'", false);
            iscostcentrestores = (bool)dt.Rows[0]["facilauto"];
        }

        //private void frmTreatmentChart_Load(object sender, EventArgs e)
        //{
        //    pharmacystore = phs.Rows[0]["station"].ToString();
        //    if (string.IsNullOrWhiteSpace(pharmacystore))
        //    {
        //        DialogResult result = MessageBox.Show("Pharmacy Store Not Defined in Stock Database...\r\n Can't process requests on this platform!", "Control Data Definition Error");
        //        //btnClose.PerformClick();
        //    }
        //    stkitems = Dataaccess.GetAnytable("", "SMS", "SELECT DISTINCT NAME, ITEM FROM STOCK where status != 'D' and store = '" + pharmacystore + "' order by name", true);

        //    //initcomboboxes(0);
        //    //initbillprocess();
        //    //displaydetails();

        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select facilauto from mrcontrol where recid = '5'", false);
        //    iscostcentrestores = (bool)dt.Rows[0]["facilauto"];
        //}

        //private void chkCostCentre_Click(object sender, EventArgs e)
        //{
        //    if (chkCostcentre.Checked) //populate cmbunitsu with cost centre details "P"
        //    {
        //        combCostCentre.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM COSTCENTRECODES order by name", true);
        //        combCostCentre.ValueMember = "Type_code";
        //        combCostCentre.DisplayMember = "name";
        //    }
        //    else
        //    {
        //        combCostCentre.DataSource = store;
        //        combCostCentre.ValueMember = "storecode";
        //        combCostCentre.DisplayMember = "name";
        //    }
        //}

        //void initbillprocess()
        //{
        //    combbillprocess.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT reference, description FROM DISPSERV order by description", true);
        //    combbillprocess.ValueMember = "reference";
        //    combbillprocess.DisplayMember = "description";
        //}

        //private void initcomboboxes(int xrec)
        //{
        //    //combDtyNurse.ValueMember = "Reference";
        //    //combDtyNurse.DisplayMember = "Name";
        //    //STOCK
        //    DataGridViewComboBoxCell CombDrugs = (DataGridViewComboBoxCell)(dataGridView1.Rows[xrec].Cells[3]);
        //    CombDrugs.DataSource = stkitems; // selcode.getcombolist("stk", "SMS");
        //    CombDrugs.ValueMember = "Item";
        //    CombDrugs.DisplayMember = "Name";
        //    CombDrugs.AutoComplete = true;
        //    DataGridViewComboBoxCell CombNurses = (DataGridViewComboBoxCell)(dataGridView1.Rows[xrec].Cells[7]);
        //    CombNurses.DataSource = nursestable;
        //    CombNurses.ValueMember = "Reference";
        //    CombNurses.DisplayMember = "Name";
        //    CombNurses.AutoComplete = true;
        //}

        //void displaydetails()
        //{
        //    dataGridView1.Rows.Clear();
        //    chart = DUENEXT.GetDUENEXT(Admrecs.REFERENCE);
        //    DataRow row = null;
        //    for (int i = 0; i < chart.Rows.Count; i++)
        //    {
        //        row = chart.Rows[i];
        //        dataGridView1.Rows.Add();
        //        initgridcombos(i);
        //        dataGridView1.Rows[i].Cells[0].Value = Convert.ToDateTime(row["Trans_Date"]).Date;
        //        dataGridView1.Rows[i].Cells[1].Value = row["Duetime"].ToString();
        //        dataGridView1.Rows[i].Cells[2].Value = row["Timegiven"].ToString();
        //        dataGridView1.Rows[i].Cells[3].Value = row["stk_desc"].ToString();
        //        dataGridView1.Rows[i].Cells[4].Value = row["Dose"].ToString();
        //        dataGridView1.Rows[i].Cells[5].Value = row["Unit"].ToString();
        //        dataGridView1.Rows[i].Cells[6].Value = row["Methodadm"].ToString();
        //        dataGridView1.Rows[i].Cells[7].Value = row["Nurse"].ToString();
        //        dataGridView1.Rows[i].Cells[8].Value = row["Remarks"].ToString();
        //        dataGridView1.Rows[i].Cells[9].Value = Convert.ToDecimal(row["billqty"]);
        //        dataGridView1.Rows[i].Cells[10].Value = row["Billqtyunit"].ToString();
        //        dataGridView1.Rows[i].Cells[11].Value = Convert.ToDecimal(row["unitcost"]);
        //        dataGridView1.Rows[i].Cells[12].Value = Convert.ToDecimal(row["cost"]);
        //        dataGridView1.Rows[i].Cells[13].Value = Convert.ToBoolean(row["Posted"]) ? true : false;
        //        dataGridView1.Rows[i].Cells[14].Value = row["stk_item"].ToString();
        //        dataGridView1.Rows[i].Cells[15].Value = row["stkbal"].ToString();
        //        dataGridView1.Rows[i].Cells[16].Value = Convert.ToDecimal(row["packqty"]);
        //        dataGridView1.Rows[i].Cells[17].Value = Convert.ToDecimal(row["packcost"]);
        //        dataGridView1.Rows[i].Cells[18].Value = Convert.ToBoolean(row["Posted"]) ? "POSTED" : "";
        //        dataGridView1.Rows[i].Cells[19].Value = row["recid"].ToString();

        //    }
        //}

        //private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == 0)
        //    {
        //        DataGridViewRow dgv = dataGridView1.Rows[e.RowIndex];
        //        if (string.IsNullOrWhiteSpace(dgv.Cells[0].FormattedValue.ToString()))
        //            dgv.Cells[0].Value = DateTime.Now.Date;
        //    }
        //}

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == 13)
        //    {
        //        DialogResult result;
        //        DataGridViewRow dgv = dataGridView1.Rows[e.RowIndex];
        //        if (string.IsNullOrWhiteSpace(txtreference.Text) || string.IsNullOrWhiteSpace(combbillprocess.Text) || string.IsNullOrWhiteSpace(dgv.Cells[3].FormattedValue.ToString()) || string.IsNullOrWhiteSpace(combCostCentre.Text))
        //        {
        //            result = MessageBox.Show("Unable to Save Record... Vital Fields are empty !\r\n Check Store/Cost Centre/ product description/bill process code... ", "Admission Service Update");
        //            return;
        //        }

        //        if (Convert.ToDecimal(dgv.Cells[9].Value) == 0 && Convert.ToDecimal(dgv.Cells[12].Value) != 0)
        //        {
        //            result = MessageBox.Show("Bill Quantity is Empty and there is value on Cost of Drug...CONTINUE ?", "Admission Service Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            if (result == DialogResult.No)
        //                return;
        //        }

        //        result = MessageBox.Show("Confirm to Update Service Record...", "Service Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.No)
        //            return;

        //        //TOTAL COST AND BILL QTY GREATER THAN 0, CHECK AND UPDATE ADMDETAIL
        //        if (Convert.ToDecimal(dgv.Cells[12].Value) >= 1 && Convert.ToDecimal(dgv.Cells[9].Value) >= 1)
        //        {
        //            if (!string.IsNullOrWhiteSpace(dgv.Cells[14].FormattedValue.ToString())) //DRUGCODE
        //            {
        //                string xfile = chkCostcentre.Checked ? "stockmas" : "stock";
        //                DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT stock_qty,cost,type from " + xfile + " where item = '" + dgv.Cells[16].FormattedValue.ToString() + "' and store = '" + combCostCentre.SelectedValue.ToString() + "'", false);
        //                if (dt.Rows.Count < 1)
        //                {
        //                    result = MessageBox.Show(dgv.Cells[3].FormattedValue.ToString() + " is NOT defined in " + combCostCentre.Text.Trim() + " \r\n This item can't be processed... Check Selections and Try Again!", "Service Update");
        //                    return;
        //                }
        //                if ((decimal)dt.Rows[0]["stock_qty"] < Convert.ToDecimal(dgv.Cells[9].Value))
        //                {
        //                    result = MessageBox.Show("There is not enough qty in stock to service request for " + dgv.Cells[3].FormattedValue.ToString() + " ... CONTINUE ?", "SHORTAGE ALERT !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //                    if (result == DialogResult.No)
        //                        return;
        //                }
        //                //update transmas if store checked
        //                if (chkStore.Checked)
        //                    Stock.writeTransmast(Convert.ToDateTime(dgv.Cells[0].Value), combCostCentre.SelectedValue.ToString(), dgv.Cells[16].Value.ToString(), "ISSUE - " + txtname.Text.Trim(), "I", Convert.ToDecimal(dgv.Cells[9].Value), txtreference.Text, msmrfunc.mrGlobals.WOPERATOR, DateTime.Now, combbillprocess.SelectedValue.ToString(), "");
        //                else
        //                {
        //                    string updatestring = "update stockmas set stock_qty = stock_qty - '" + Convert.ToDecimal(dgv.Cells[9].Value) + "' where store = '" + combCostCentre.SelectedValue.ToString() + "' and item = '" + dgv.Cells[14].Value.ToString() + "'";
        //                    bissclass.UpdateRecords(updatestring, "SMS");

        //                    SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
        //                    SqlCommand selectCommand = new SqlCommand();
        //                    selectCommand.CommandText = "STKTRANS_Add";
        //                    selectCommand.Connection = connection;
        //                    selectCommand.CommandType = CommandType.StoredProcedure;
        //                    DataRow row = dt.Rows[0];
        //                    connection.Open();
        //                    selectCommand.Parameters.AddWithValue("@Reference", txtreference.Text);
        //                    selectCommand.Parameters.AddWithValue("@TRANS_DATE", Convert.ToDateTime(dgv.Cells[0].Value));
        //                    selectCommand.Parameters.AddWithValue("@TRANSTYPE", "I");
        //                    selectCommand.Parameters.AddWithValue("@STORE", combCostCentre.SelectedValue.ToString());
        //                    selectCommand.Parameters.AddWithValue("@ITEM", dgv.Cells[14].Value.ToString());
        //                    selectCommand.Parameters.AddWithValue("@DESCRIPTION", dgv.Cells[3].Value.ToString());
        //                    selectCommand.Parameters.AddWithValue("@TRANS_QTY", Convert.ToDecimal(dgv.Cells[9].Value));
        //                    selectCommand.Parameters.AddWithValue("@STOCK_BAL,", (decimal)row["STOCK_QTY"] - Convert.ToDecimal(dgv.Cells[9].Value));
        //                    selectCommand.Parameters.AddWithValue("@COST", (decimal)row["cost"]);
        //                    selectCommand.Parameters.AddWithValue("@SELL", Convert.ToDecimal(dgv.Cells[11].Value));
        //                    selectCommand.Parameters.AddWithValue("@POSTED", true);
        //                    selectCommand.Parameters.AddWithValue("@BIN", "");
        //                    selectCommand.Parameters.AddWithValue("@TYPE", row["type"].ToString());
        //                    selectCommand.Parameters.AddWithValue("@UNIT", dgv.Cells[10].Value.ToString());
        //                    selectCommand.Parameters.AddWithValue("@RECTYPE", "I");
        //                    selectCommand.Parameters.AddWithValue("@U_SIZE", 0m);
        //                    selectCommand.Parameters.AddWithValue("@COSTVAL", 0m);
        //                    selectCommand.Parameters.AddWithValue("@SELLVAL", 0m);
        //                    selectCommand.Parameters.AddWithValue("@WHSELL", 0m);
        //                    selectCommand.Parameters.AddWithValue("@OPERATOR", msmrfunc.mrGlobals.WOPERATOR);
        //                    selectCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);

        //                    selectCommand.ExecuteNonQuery();

        //                    connection.Close();
        //                }
        //            }
        //            ADMDETAI.writeAdmdetails(true, txtreference.Text, Convert.ToDateTime(dgv.Cells[0].Value), dgv.Cells[2].Value.ToString(), combbillprocess.SelectedValue.ToString(), combbillprocess.SelectedValue.ToString(), dgv.Cells[14].Value.ToString(), dgv.Cells[3].Value.ToString(), dgv.Cells[10].Value.ToString(), Convert.ToDecimal(dgv.Cells[9].Value), Convert.ToDecimal(dgv.Cells[12].Value), false, DateTime.Now, msmrfunc.mrGlobals.WOPERATOR, DateTime.Now, Admrecs.GROUPCODE, Admrecs.PATIENTNO, "", "", 0);

        //            Admrecs.UpdateAdmrecAmounts(txtreference.Text, Convert.ToDecimal(dgv.Cells[12].Value), 0m);
        //        }
        //        string billat = Convert.ToDecimal(dgv.Cells[9].Value) == 0 ? "0" : Convert.ToDecimal(dgv.Cells[9].Value) + " " + dgv.Cells[10].Value.ToString().Trim() + " = " + dgv.Cells[12].Value.ToString() + " BY " + msmrfunc.mrGlobals.WOPERATOR;
        //        string xcomments = dgv.Cells[3].Value.ToString().Trim() + " Givern : " + dgv.Cells[2].Value.ToString() + " Dose : " + dgv.Cells[4].Value.ToString() + " " + dgv.Cells[5].Value.ToString().Trim() + "\r\n Billed at " + billat;
        //        //update med history file
        //        MedHist medhist = MedHist.GetMEDHIST(Admrecs.GROUPCODE, Admrecs.PATIENTNO, "", false, true, Convert.ToDateTime(dgv.Cells[0].Value));
        //        bool newhist = (medhist == null) ? true : false;
        //        if (!newhist)
        //        {
        //            xcomments = medhist.COMMENTS.Trim() + "\r\n" + xcomments.Trim();
        //        }
        //        if (!medhistwritten)
        //        {
        //            xcomments = "==> IN-PATIENT PRESCRIPTIONS (Given):" + DateTime.Now.ToString("dd-MM-yyyy @ HH:mm:sst") + " : " + msmrfunc.mrGlobals.WOPERATOR + "\r\n" + xcomments;
        //            medhistwritten = true;
        //        }

        //        MedHist.updatemedhistcomments(Admrecs.GROUPCODE, Admrecs.PATIENTNO, Convert.ToDateTime(dgv.Cells[0].Value), xcomments, newhist, Admrecs.REFERENCE, Admrecs.NAME, Admrecs.GHGROUPCODE, Admrecs.GROUPHEAD, "");

        //        MessageBox.Show("Completed...", "Updated");
        //    }
        //}

        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //        if (e.ColumnIndex == this.dataGridView1.Columns[0].Index)
        //            cell.ToolTipText = "Treatment Date"; //Select Immunization Code as Defined in Tariff File on Immunization Category";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[1].Index)
        //            cell.ToolTipText = "You must specify Duetime in 24 HR Format to allow for auto-duenext Alert";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[2].Index)
        //            cell.ToolTipText = "Time Giving - you can type freelly";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[3].Index)
        //            cell.ToolTipText = "You can click the down arrow to select Drug, if not sent by Doctors";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[4].Index)
        //            cell.ToolTipText = "Dose  -  Qty To Be Given or Given";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[5].Index)
        //            cell.ToolTipText = "Options > 'Mls','Tabs','Amp','Caps','Vial','Synge','Btl','Drops','mg,'Grams' : Type freelly";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[6].Index)
        //            cell.ToolTipText = "Method of Administration";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[7].Index)
        //            cell.ToolTipText = "Staff Nurse that administered the drug/injection";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[8].Index)
        //            cell.ToolTipText = "";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[9].Index)
        //            cell.ToolTipText = "Qty to Bill Patient and Deduct from Store. Enter 0 for DON'T BILL.";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[10].Index)
        //            cell.ToolTipText = "Unit of measure.  You can click the down arrow to select.";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[11].Index)
        //            cell.ToolTipText = "UnitCost of the qty requied";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[12].Index)
        //            cell.ToolTipText = "Total cost of required qty";
        //        else if (e.ColumnIndex == this.dataGridView1.Columns[13].Index)
        //            cell.ToolTipText = "Click to Save Record; update Admission Service Records and update Stock - ONLY WHEN GIVEN";
        //        // else if (e.ColumnIndex == this.dataGridView1.Columns[14].Index)
        //        //     cell.ToolTipText = "Click to Remove all definitions on this line";
        //    }
        //}

        //private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        DataGridViewRow dgv = dataGridView1.Rows[e.RowIndex];
        //        recno = e.RowIndex;

        //        if (e.ColumnIndex == 9) //on bill qty
        //        {
        //            if (Convert.ToDecimal(dgv.Cells[9].Value) > 0 && Convert.ToDecimal(dgv.Cells[11].Value) > 0)
        //                dgv.Cells[12].Value = Convert.ToDecimal(dgv.Cells[9].Value) * Convert.ToDecimal(dgv.Cells[11].Value);
        //        }
        //        else if (e.ColumnIndex == 2 && !string.IsNullOrWhiteSpace(dgv.Cells[2].FormattedValue.ToString()) && Convert.ToDateTime(dgv.Cells[0].FormattedValue.ToString()) == DateTime.Now.Date && Convert.ToDouble(dgv.Cells[1].FormattedValue.ToString()) < Convert.ToDouble(DateTime.Now.ToShortTimeString()))
        //        {
        //            DialogResult result = MessageBox.Show("This Prescription is not due yet...CONTINUE ?", "Dispensing Chart", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //            if (result == DialogResult.No)
        //                dgv.Cells[2].Value = "";
        //        }
        //        else if (string.IsNullOrWhiteSpace(dgv.Cells[2].FormattedValue.ToString()) && dgv.Cells[19].FormattedValue.ToString().Trim() != "POSTED" && msmrfunc.mrGlobals.mcandelete)
        //            btnRemove.Enabled = true;

        //    }
        //}

        //private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    initgridcombos(e.RowIndex);
        //    /*   DataGridViewComboBoxCell CombDrugs = (DataGridViewComboBoxCell)(dataGridView1.Rows[e.RowIndex].Cells[3]);
        //       CombDrugs.DataSource = stkitems; // selcode.getcombolist("stk", "SMS");
        //       CombDrugs.ValueMember = "Item";
        //       CombDrugs.DisplayMember = "Name";
        //       CombDrugs.AutoComplete = true;
        //       DataGridViewComboBoxCell CombNurses = (DataGridViewComboBoxCell)(dataGridView1.Rows[0].Cells[7]);
        //       CombNurses.ValueMember = "Reference";
        //       CombNurses.DisplayMember = "Name";
        //       CombNurses.AutoComplete = true;*/

        //    //  dataGridView1.Rows[e.RowIndex].Cells[13].Value = "Remove";
        //    //  dataGridView1.Rows[e.RowIndex].Cells[17].Value = "NEWREC";
        //}

        //void initgridcombos(int xrow)
        //{
        //    DataGridViewComboBoxCell CombDrugs = (DataGridViewComboBoxCell)(dataGridView1.Rows[xrow].Cells[3]);
        //    CombDrugs.DataSource = stkitems; // selcode.getcombolist("stk", "SMS");
        //    CombDrugs.ValueMember = "Item";
        //    CombDrugs.DisplayMember = "Name";
        //    CombDrugs.AutoComplete = true;
        //    DataGridViewComboBoxCell CombNurses = (DataGridViewComboBoxCell)(dataGridView1.Rows[xrow].Cells[7]);
        //    CombNurses.DataSource = nursestable;
        //    CombNurses.ValueMember = "Reference";
        //    CombNurses.DisplayMember = "Name";
        //    CombNurses.AutoComplete = true;

        //    //    dataGridView1.Rows[xrow].Cells[13].Value = "Remove";
        //}

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    if (recno > 0)
        //    {
        //        int xrow = recno - 1;
        //        if (dataGridView1.Rows[xrow].Cells[1].Value == null || dataGridView1.Rows[xrow].Cells[4].Value == null)
        //        {
        //            DialogResult result = MessageBox.Show("Generated record space has not been fully utilized...", "New record Add");
        //            return;
        //        }
        //    }

        //    DataGridViewRow row = new DataGridViewRow();
        //    dataGridView1.Rows.Add();
        //    recno++;
        //    initgridcombos(recno - 1);
        //    return;
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        public MR_DATA.REPORTS btnSubmit_Click(IEnumerable<MR_DATA.REPORTS> tableList)
        {
            //DialogResult result = MessageBox.Show("Confirm to Submit Records...";

            //if (result == DialogResult.No)
            //    return;

            //Admrecs admrec = new Admrecs();

            //check if reference exist
            Admrecs = Admrecs.GetADMRECS(vm.REPORTS.txtreference);

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            //DataGridViewRow gv = null;
            connection.Open();

            foreach(var row in tableList)
            {
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = row.cmbsave ? "duenext_Update" : "duenext_Add" ;
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@patientno", Admrecs.PATIENTNO);
                insertCommand.Parameters.AddWithValue("@groupcode", Admrecs.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@trans_date", Convert.ToDateTime(row.txtclinic));
                insertCommand.Parameters.AddWithValue("@stk_item", row.doctor);
                insertCommand.Parameters.AddWithValue("@stk_desc", row.txtsurname);
                insertCommand.Parameters.AddWithValue("@store", vm.REPORTS.comhmoservgrp);
                insertCommand.Parameters.AddWithValue("@dose", Convert.ToDecimal(row.edtallergies));
                insertCommand.Parameters.AddWithValue("@unit", row.cbotype);
                insertCommand.Parameters.AddWithValue("@cost", Convert.ToDecimal(row.cboTribe));
                insertCommand.Parameters.AddWithValue("@nurse", row.txtcreditlimit);
                insertCommand.Parameters.AddWithValue("@posted", row.cmbsave ? true : false);
                insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@itemno", Convert.ToDecimal(row.txtstaffno));
                insertCommand.Parameters.AddWithValue("@stkbal", Convert.ToDecimal(row.txtbranch));
                insertCommand.Parameters.AddWithValue("@timegiven", row.combFacility);
                insertCommand.Parameters.AddWithValue("@type", "");
                insertCommand.Parameters.AddWithValue("@duetime", row.cbotitle);
                insertCommand.Parameters.AddWithValue("@methodadm", row.cbogender);
                insertCommand.Parameters.AddWithValue("@remarks", row.categ_save);
                //insertCommand.Parameters.AddWithValue("@adminref", Admrecs.REFERENCE);
                insertCommand.Parameters.AddWithValue("@packqty", Convert.ToDecimal(row.cbogenotype));
                insertCommand.Parameters.AddWithValue("@packcost", Convert.ToDecimal(row.txtworkphone));
                insertCommand.Parameters.AddWithValue("@unitcost", Convert.ToDecimal(row.cbokinstate));
                insertCommand.Parameters.AddWithValue("@billqty", Convert.ToDecimal(row.combillcycle));
                insertCommand.Parameters.AddWithValue("@billqtyunit", row.txtcurrency);

                insertCommand.ExecuteNonQuery();
            }

     
            connection.Close();
            vm.REPORTS.alertMessage = "Completed...";

            return vm.REPORTS;
        }

        //private void btnRemove_Click(object sender, EventArgs e)
        //{
        //    DataGridViewRow dgv = dataGridView1.Rows[recno];
        //    DialogResult result;

        //    if (dgv.Cells[19].FormattedValue.ToString().Trim() == "POSTED")
        //    {
        //        result = MessageBox.Show("This Record can't be deleted... Its Confirmed!");
        //        return;
        //    }
        //    result = MessageBox.Show("Delete Record..?", "In-Patient's Treatment Chart", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (result == DialogResult.No)
        //        return;
        //    if (Convert.ToDecimal(dataGridView1.Rows[recno].Cells[15].Value) > 0)
        //    {
        //        string updatestring = "DELETE from duenext WHERE RECID = '" + dataGridView1.Rows[recno].Cells[19].Value.ToString().Trim() + "' ";
        //        bissclass.UpdateRecords(updatestring, "MR");
        //    }

        //    dataGridView1.Rows.RemoveAt(recno);
        //    result = MessageBox.Show("Record Deleted...", "In-Patient's Treatment Chart Item");
        //    recno = 0;
        //}

        //private void btnDueNext_Click(object sender, EventArgs e)
        //{
        //    string xduetime = "duenext.trans_date = '" + DateTime.Now.ToShortDateString() + "' and left(duenext.duetime,2) = '" + DateTime.Now.Hour + "' and timegiven = ''";
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT duenext.groupcode, duenext.patientno, duenext.stk_desc+' '+duenext.dose+' '+duenext.unit AS description, duenext.duetime, duenext.reference, admrecs.name, admrecs.FACILITY+' Rm'+rtrim(admrecs.ROOM)+' '+admrecs.BED AS facility from duenext left join admrecs on duenext.reference = '" + Admrecs.REFERENCE + "' where '" + xduetime + "'", false);

        //    if (dt.Rows.Count < 1)
        //        return;

        //    frmDueNextRpt duenext = new frmDueNextRpt(dt);
        //    duenext.Show();

        //}

        //private void btnApply_Click(object sender, EventArgs e)
        //{
        //}

        //private void nmrTime2_ValueChanged(object sender, EventArgs e)
        //{
        //    DialogResult result;
        //    bool iserror = false;
        //    NumericUpDown nmr = sender as NumericUpDown;
        //    if (nmr.Value < 1)
        //        return;
        //    if (nmr.Name == "nmrTF1" && nmrTF2.Value <= nmrTF1.Value)
        //        iserror = true;
        //    if (nmr.Name == "nmrTF3" && nmrTF3.Value <= nmrTF2.Value)
        //        iserror = true;
        //    if (nmr.Name == "nmrTF4" && nmrTF4.Value <= nmrTF3.Value)
        //        iserror = true;
        //    if (iserror)
        //    {
        //        result = MessageBox.Show("Invalid time Specification... Time Must be insequence");
        //        nmr.Value = 0;
        //    }
        //}

        //private void btnCloseInpPrescription_Click(object sender, EventArgs e)
        //{
        //    panel_InpPrescription.Visible = false;
        //}

        //private void btnInpPresAdd_Click(object sender, EventArgs e)
        //{
        //    if (listView1.SelectedItems.Count < 1)
        //        return;

        //    ListViewItem lv = listView1.Items[listView1.SelectedIndex];
        //    DialogResult result;

        //    if (iscostcentrestores && lv.SubItems[14].ToString().Trim() == "NO" && lv.SubItems[4].ToString().Trim() == "0")
        //    {
        //        result = MessageBox.Show("This Item has not been transferred from Pharmacy... CAN'T ADD TO CHART UNTIL PHARMACY HAS WORKED ON IT. TKS!!!", "Inpatient Dispenssing Chart");
        //        return;
        //    }

        //    panel_TF.Visible = true;

        //    /*start_save = xstart
        //        rcount = 0
        //        xduration = IIF(EMPTY(tmpinpDispensa.duration),1,tmpinpDispensa.duration)*/
            
        //    txtprescriptions.Visible = true;
        //    DataRow row = dtInpPrescriptn.Rows[listView1.SelectedIndex];

        //    txtprescriptions.Text = lv.SubItems[2].Text.Trim() + "\r\n" + row["cdose"].ToString() + " x " +
        //        row["cinterval"].ToString() + " For " + row["cduration"].ToString() + " Sp.Instr : " +
        //        row["rx"].ToString() + " : " + row["sp_inst"].ToString();
        //}

        //private void btnInpatientPrescription_Click(object sender, EventArgs e)
        //{
        //    dtInpPrescriptn = Dataaccess.GetAnytable("", "MR", "select trans_date, itemno, stk_item, stk_desc, qty_pr, cumgv, unit, dose, duration, cdose, cinterval, cduration, unitcost, cost, rx, doctor, stkbal, recid, posted, qty_gv, phtransferred, sp_inst, interval from inpdispensa where reference = '" + txtreference.Text + "' and posted = '0' order by trans_date", false);
        //    string[] arr = new string[15];
        //    listView1.Items.Clear();
        //    ListViewItem itm;

        //    foreach (DataRow row in dtInpPrescriptn.Rows)
        //    {
        //        arr[0] = row["trans_date"].ToString();
        //        arr[1] = row["itemno"].ToString();
        //        arr[2] = row["stk_desc"].ToString();
        //        arr[3] = row["qty_pr"].ToString();
        //        arr[4] = row["CUMGV"].ToString();
        //        arr[5] = row["unit"].ToString();
        //        arr[6] = row["cdose"].ToString().Trim() + " x " + row["cinterval"].ToString().Trim() + " x " +
        //        row["cduration"].ToString().Trim() + " : " + row["rx"].ToString().Trim();
        //        arr[7] = row["doctor"].ToString();
        //        arr[8] = row["unitcost"].ToString();
        //        arr[9] = row["cost"].ToString();
        //        arr[10] = row["stkbal"].ToString();
        //        arr[11] = row["stk_item"].ToString();
        //        arr[12] = row["qty_gv"].ToString();
        //        arr[13] = row["recid"].ToString();
        //        arr[14] = (bool)row["phtransferred"] ? "YES" : "NO";
        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);
        //    }

        //    panel_InpPrescription.Visible = true;
        //}

        //private void btnIgnoreTF_Click(object sender, EventArgs e)
        //{
        //    panel_TF.Visible = false;
        //}

        //private void btnApplyTF_Click(object sender, EventArgs e)
        //{
        //    DialogResult result;
        //    result = MessageBox.Show("Confirm to Apply Prescription details to Treatment Chart", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.No)
        //        return;
        //    DataRow row = dtInpPrescriptn.Rows[listView1.SelectedIndex];
        //    int xduration = Convert.ToInt32(row["duration"]);
        //    int xinterval = Convert.ToInt32(row["interval"]);
        //    xduration = xduration < 1 ? 1 : xduration;
        //    xinterval = xinterval < 1 ? 24 : xinterval;
        //    int xtimes = 0, dgr = 0; //,xcount;

        //    for (int i = 0; i < xduration; i++)
        //    {
        //        xtimes = 24 / xinterval;
        //        // xcount = 1;
        //        for (int i1 = 0; i1 < xtimes; i1++)
        //        {
        //            dataGridView1.Rows.Add();
        //            initgridcombos(i);
        //            //   dgr = dataGridView1.NewRowIndex;
        //            string xxstr = row["stk_desc"].ToString().Trim();
        //            dataGridView1.Rows[dgr].Cells[0].Value = i < 1 ? Convert.ToDateTime(row["Trans_Date"]).Date : Convert.ToDateTime(row["Trans_Date"]).Date.AddDays(i);
        //            dataGridView1.Rows[dgr].Cells[1].Value = i1 == 0 ? nmrTF1.Value.ToString() + ".00" : i1 == 1 ? nmrTF2.Value.ToString() + ".00" : i1 == 2 ? nmrTF3.Value.ToString() + ".00" : nmrTF1.Value.ToString() + ".00"; // row["Duetime"].ToString();
        //            dataGridView1.Rows[dgr].Cells[2].Value = ""; // row["Timegiven"].ToString();
        //            dataGridView1.Rows[dgr].Cells[3].Value = row["stk_desc"].ToString();
        //            dataGridView1.Rows[dgr].Cells[4].Value = row["Dose"].ToString();
        //            dataGridView1.Rows[dgr].Cells[5].Value = row["Unit"].ToString();
        //            dataGridView1.Rows[dgr].Cells[6].Value = ""; // row["Methodadm"].ToString();
        //            dataGridView1.Rows[dgr].Cells[7].Value = ""; // row["Nurse"].ToString();
        //            dataGridView1.Rows[dgr].Cells[8].Value = ""; // row["Remarks"].ToString();
        //            dataGridView1.Rows[dgr].Cells[9].Value = "0"; // Convert.ToDecimal(row["billqty"]);
        //            dataGridView1.Rows[dgr].Cells[10].Value = "0"; // row["Billqtyunit"].ToString();
        //            dataGridView1.Rows[dgr].Cells[11].Value = Convert.ToDecimal(row["unitcost"]);
        //            dataGridView1.Rows[dgr].Cells[12].Value = Convert.ToDecimal(row["cost"]);
        //            dataGridView1.Rows[dgr].Cells[13].Value = false; //Convert.ToBoolean( row["Posted"]) ? "true : false;
        //            dataGridView1.Rows[dgr].Cells[14].Value = row["stk_item"].ToString();
        //            dataGridView1.Rows[dgr].Cells[15].Value = row["stkbal"].ToString(); // row["recid"].ToString();
        //            dataGridView1.Rows[dgr].Cells[16].Value = 0;
        //            dataGridView1.Rows[dgr].Cells[17].Value = 0; // Convert.ToDecimal(row["packqty"]);
        //            dataGridView1.Rows[dgr].Cells[18].Value = ""; // Convert.ToDecimal(row["packcost"]);
        //            dataGridView1.Rows[dgr].Cells[19].Value = 0; // Convert.ToBoolean(row["Posted"]) ? "POSTED" : "";
        //            dgr++;
        //        }
        //    }
        //    panel_TF.Visible = false;
        //}

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //do nothing
        }
    }
}