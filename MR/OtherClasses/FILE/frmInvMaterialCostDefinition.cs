#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using mradmin.BissClass;
using mradmin.DataAccess;
using mradmin.Forms;
using SCS.DataAccess;
using msfunc;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmInvMaterialCostDefinition
    {
        Stock stock = new Stock();
        DataTable stores = Dataaccess.GetAnytable("", "SMS", "SELECT storecode, name from store order by name", true), dtunit = Dataaccess.GetAnytable("StkUnitofMeasure", "CODES", "SELECT type_code,Name from StkUnitofMeasure order by name", true),
            dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES ORDER BY NAME", true), dtDesc, dtCopy,
            dttariff = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE,NAME FROM TARIFF", false);
        string woperator, lookupsource, anycode, savedfacility;
        int recno;
        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
        //DataSet ds;
        //DataTable sdt;

        MR_DATA.MR_DATAvm vm;

        public frmInvMaterialCostDefinition(MR_DATA.MR_DATAvm VM2, string woperato)
        {
            //InitializeComponent();
            vm = VM2;
            woperator = woperato;
        }

        //private void frmInvMaterialCostDefinition_Load(object sender, EventArgs e)
        //{
        //    initcomboboxes();
        //    //worked = false;
        //}

        //private void initcomboboxes()
        //{
        //    //get description
        //    this.cbofacility.DataSource = dtfacility;
        //    cbofacility.ValueMember = "Type_code";
        //    cbofacility.DisplayMember = "name";

        //    cboStore.DataSource = stores;
        //    cboStore.ValueMember = "storecode";
        //    cboStore.DisplayMember = "name";

        //    // initcomboUnits(1);
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btndesc")
        //    {
        //        this.cboDesc.Text = ""; // procedure = "";
        //        lookupsource = "SD";
        //        msmrfunc.mrGlobals.crequired = "SD"; //SERVICE DESCRIPTIONS
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR [ALL] DEFINED INVESTIGATIONS/PROCEDURES";
        //    }

        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;

        //    if (lookupsource == "SD") //service desc
        //    {
        //        this.cboDesc.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        //lblprocedure.Text = 
        //        string procedure = msmrfunc.mrGlobals.anycode1;

        //        bissclass.displaycombo(cbofacility, dtfacility, msmrfunc.mrGlobals.anycode2, "name");
        //        lblfacility.Text = msmrfunc.mrGlobals.anycode2;
        //        msmrfunc.mrGlobals.anycode2 = "";

        //        if (string.IsNullOrWhiteSpace(savedfacility) || savedfacility != lblfacility.Text)
        //        {
        //            combolist();
        //        }
        //        savedfacility = lblfacility.Text;
        //        bissclass.displaycombo(cboDesc, dtDesc, procedure, "name");
        //        this.cboDesc.Focus();
        //    }
        //    else if (lookupsource == "STKDETAIL")
        //    {
        //        anycode = msmrfunc.mrGlobals.anycode1;
        //        dataGridView1.Rows[recno].Cells[4].Value = cboStore.SelectedValue.ToString();
        //        dataGridView1.Rows[recno].Cells[1].Value = anycode;
        //        if (string.IsNullOrWhiteSpace(anycode))
        //            return;
        //        if (!validate_stock(cboStore.SelectedValue.ToString(), anycode))
        //        {
        //            dataGridView1.Rows[recno].Cells[4].Value = "";
        //            dataGridView1.Rows[recno].Cells[1].Value = "";
        //            dataGridView1.Rows[recno].Cells[1].Selected = true;
        //            return;
        //        }
        //    }

        //}

        //private void cbofacility_LostFocus(object sender, EventArgs e)
        //{
        //    if (cbofacility.SelectedValue == null)
        //        return;

        //    combolist(); //gets list of defined procedures for this facility
        //    cboDesc.Focus();
        //}

        //void combolist()
        //{
        //    string selectstring = "SELECT NAME, REFERENCE FROM tariff WHERE category = '" + cbofacility.SelectedValue.ToString() + "' ORDER BY NAME";

        //    dtDesc = Dataaccess.GetAnytable("", "MR", selectstring, true);

        //    cboDesc.DataSource = dtDesc;
        //    cboDesc.ValueMember = "reference";
        //    cboDesc.DisplayMember = "name";
        //    return;
        //}

        //private void cboDesc_LostFocus(object sender, EventArgs e)
        //{
        //    if (cbofacility.SelectedValue == null)
        //        return;
        //    if (string.IsNullOrWhiteSpace(cboDesc.Text))
        //        return;

        //    btnCopy.Enabled = btnAppend.Enabled = false;
        //    //newrec = true;

        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT * FROM MRB19 WHERE FACILITY = '" + cbofacility.SelectedValue.ToString() + "' and process = '" + cboDesc.SelectedValue.ToString() + "'", false);

        //    dataGridView1.Rows.Clear();
        //    DataGridViewRow dgv;
        //    DataRow row;

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        row = dt.Rows[i];
        //        dataGridView1.Rows.Add();
        //        dgv = dataGridView1.Rows[i];
        //        dgv.Cells[0].Value = (i + 1).ToString();
        //        dgv.Cells[1].Value = row["item"].ToString();
        //        dgv.Cells[3].Value = row["description"].ToString();
        //        dgv.Cells[4].Value = row["store"].ToString();
        //        dgv.Cells[5].Value = row["qty"].ToString();
        //        dgv.Cells[6].Value = row["unit"].ToString();
        //        dgv.Cells[7].Value = Convert.ToDecimal(row["cost"]).ToString("N2");
        //        dgv.Cells[8].Value = Convert.ToDecimal(row["sell"]).ToString("N2");
        //        dgv.Cells[9].Value = row["numboftest"].ToString();
        //        dgv.Cells[10].Value = row["testcount"].ToString();
        //        dgv.Cells[11].Value = Convert.ToBoolean(row["ondigital"]) ? true : false;
        //        dgv.Cells[12].Value = Convert.ToBoolean(row["selectable"]) ? true : false;
        //        dgv.Cells[14].Value = row["RECID"].ToString();
        //    }

        //    if (dt.Rows.Count > 0)
        //    {
        //        //newrec = false;
        //    }
        //    else if (dt.Rows.Count > 0)
        //        btnCopy.Enabled = true;

        //    cboStore.Focus();
        //    return;
        //}

        //private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        if (e.ColumnIndex == 1 && string.IsNullOrWhiteSpace(cboStore.Text))
        //        {
        //            MessageBox.Show("Issuing Store must be specified...", "");
        //            cboStore.Focus();
        //            return;
        //        }
        //        btnRemove.Enabled = true;
        //        recno = e.RowIndex;
        //    }
        //}

        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //        if (e.ColumnIndex == 1)
        //            cell.ToolTipText = "Enter product Code as Defined or Click the Lookup Button (...) To Select Products";
        //        else if (e.ColumnIndex == 2)
        //            cell.ToolTipText = "Lookup Button on Defined Products";
        //        else if (e.ColumnIndex == 5)
        //            cell.ToolTipText = "Enter Quantity of Material Deductable for this Test";
        //        else if (e.ColumnIndex == 6)
        //            cell.ToolTipText = "Select item's Unit of Measure";
        //        else if (e.ColumnIndex == 9)
        //            cell.ToolTipText = "Number of This Test required for a deduction to be made from stock";
        //        else if (e.ColumnIndex == 11)
        //            cell.ToolTipText = "Check, if Test is done on Digital Machine";
        //        else if (e.ColumnIndex == 12)
        //            cell.ToolTipText = "Check if material is selectable (Optional) on Reporting";
        //    }
        //}

        //private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridViewRow dgv = new DataGridViewRow();
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        dgv = dataGridView1.Rows[e.RowIndex];
        //        if (string.IsNullOrWhiteSpace(dgv.Cells[1].FormattedValue.ToString())) //there must be a stock selection
        //            return;
        //        if (dgv.Cells[1].Value != null)
        //        {
        //            btnRemove.Enabled = true;
        //            recno = e.RowIndex;
        //            if (e.ColumnIndex == 1) //&& dgv.Cells[1].Value != null) //STOCK CODE
        //            {
        //                //check stock master file
        //                string xstore, xitem;
        //                xstore = cboStore.SelectedValue.ToString();
        //                dgv.Cells[4].Value = xstore;
        //                xitem = dgv.Cells[1].Value.ToString();
        //                if (!validate_stock(xstore, xitem))
        //                {
        //                    dgv.Cells[1].Value = dgv.Cells[4].Value = "";
        //                    dgv.Cells[1].Selected = true;
        //                    return;
        //                }
        //            }
        //        }
        //        if (dgv.Cells[5].Value != null && Convert.ToDecimal(dgv.Cells[5].Value) > 0)
        //            dgv.Cells[13].Value = "UPDATED";
        //    }
        //}

        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
        //    {
        //        if (e.ColumnIndex == 2)
        //        {
        //            recno = e.RowIndex;
        //            lookupsource = "STKDETAIL";
        //            msmrfunc.mrGlobals.crequired = "s";
        //            msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED STOCK ITEMS IN ALL STORES";

        //            frmselcode FrmSelCode = new frmselcode();
        //            FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //            FrmSelCode.ShowDialog();
        //        }

        //    }
        //}

        //private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        //{
        //    //do nothing
        //}

        //========================================================
        //bool validate_stock(string xstore, string xitem)
        //{
        //    // if (xstore != dataGridView1.Rows[])
        //    stock = Stock.GetStock(xstore, xitem, false);
        //    if (stock == null)
        //    {
        //        DialogResult result = MessageBox.Show("Selected Stock is not registered in " +
        //            xstore, "Stock Master File", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return false;
        //    }
        //    if (!stock.Posted)
        //    {
        //        DialogResult result = MessageBox.Show("Selected Stock : " + stock.Name.Trim() +
        //            " Definition has not been confirmed in " + stock.Store, "STOCK MASTER RECORD");
        //        return false;
        //    }
        //    if (stock.Status == "D")
        //    {
        //        DialogResult result = MessageBox.Show("This Item has been flagged domant... NO UPDATE ALLOWED !", "Stock Master File");
        //        return false;
        //    }

        //    dataGridView1.Rows[recno].Cells[3].Value = stock.Name;
        //    dataGridView1.Rows[recno].Cells[6].Value = stock.Unit;
        //    dataGridView1.Rows[recno].Cells[7].Value = stock.Cost;
        //    dataGridView1.Rows[recno].Cells[8].Value = stock.Sell;
        //    return true;
        //}

        //private void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    if (!bissclass.IsPresent(cboStore, "Default Store", false) || !bissclass.IsPresent(cboDesc, "Procedure", false) || !bissclass.IsPresent(cbofacility, "Facility", false))
        //        return;
        //    DialogResult reply = MessageBox.Show("Confirm To Save Records..", "Inv. Material Cost Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (reply == DialogResult.No)
        //        return;

        //    savedetails();
        //}

        public MR_DATA.REPORTS savedetails(string facility, string procedure)
        {
            // int xcount = 0, xrecid = 0;
            bool newitem = false, worked = false;
            DataGridViewRow dgv;

            try
            {
                SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                connection.Open();

                foreach (var tableRow in vm.REPORTSS)
                {
                    //dgv = dataGridView1.Rows[i];
                    if (tableRow.txtstaffno == null || tableRow.cboAge == null || string.IsNullOrWhiteSpace(tableRow.cboAge) || tableRow.txtgroupcode != "UPDATED")
                        continue;

                    newitem = tableRow.txtpatientno == null || tableRow.txtpatientno == "0" ? true : false;

                    SqlCommand insertCommand = new SqlCommand();
                    insertCommand.CommandText = newitem ? "mrb19_Add" : "mrb19_Update";
                    insertCommand.Connection = connection;
                    insertCommand.CommandType = CommandType.StoredProcedure;

                    insertCommand.Parameters.AddWithValue("FACILITY", facility);
                    insertCommand.Parameters.AddWithValue("PROCESS", procedure);
                    insertCommand.Parameters.AddWithValue("ITEM", tableRow.cbotype);
                    insertCommand.Parameters.AddWithValue("DESCRIPTION", tableRow.edtallergies);
                    insertCommand.Parameters.AddWithValue("STORE", tableRow.txtbranch);
                    insertCommand.Parameters.AddWithValue("QTY", Convert.ToDecimal(tableRow.cboAge));
                    insertCommand.Parameters.AddWithValue("UNIT", tableRow.txtaddress1);
                    insertCommand.Parameters.AddWithValue("COST", tableRow.txtcurrency == null ? 0m : Convert.ToDecimal(tableRow.txtcurrency));
                    insertCommand.Parameters.AddWithValue("SELL", tableRow.txtclinic == null ? 0m : Convert.ToDecimal(tableRow.txtclinic));
                    insertCommand.Parameters.AddWithValue("NUMBOFTEST", tableRow.cbogenotype == null ? 0m : Convert.ToDecimal(tableRow.cbogenotype));
                    insertCommand.Parameters.AddWithValue("TESTCOUNT", tableRow.txtkinphone == null ? 0m : Convert.ToDecimal(tableRow.txtkinphone));
                    insertCommand.Parameters.AddWithValue("ONDIGITAL", Convert.ToBoolean(tableRow.cmbsave) ? true : false);
                    insertCommand.Parameters.AddWithValue("SELECTABLE", Convert.ToBoolean(tableRow.cmdgrpmember) ? true : false);
                    insertCommand.Parameters.AddWithValue("OPERATOR", woperator);
                    insertCommand.Parameters.AddWithValue("dtime", DateTime.Now);


                    if (!newitem)
                        insertCommand.Parameters.AddWithValue("@recid", Convert.ToInt32(tableRow.txtpatientno));

                    insertCommand.ExecuteNonQuery();
                    //  xcount++;
                }

                connection.Close();
                worked = true;
            }

            catch (SqlException ex)
            {
                vm.REPORTS.alertMessage = ex.Message + "/n" + ex.GetType().ToString();
                return vm.REPORTS;
            }
            catch (Exception ex)
            {
                vm.REPORTS.ActRslt = ex.Message + "/n" + ex.GetType().ToString();
                return vm.REPORTS;
            }

            string xstr = worked ? "Record Submitted Successfully..." : "No Record Saved!";

            vm.REPORTS.alertMessage = xstr;

            //if (worked)
            //    cboDesc.Focus();

            return vm.REPORTS;
        }

        //private void btnCopy_Click(object sender, EventArgs e)
        //{
        //    DialogResult reply = MessageBox.Show("Confirm to copy details of this defintion..", "Material Definition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (reply == DialogResult.No)
        //        return;

        //    dtCopy = Dataaccess.GetAnytable("", "MR", "SELECT * FROM MRB19 WHERE FACILITY = '" + cbofacility.SelectedValue.ToString() + "' and process = '" + cboDesc.SelectedValue.ToString() + "'", false);
        //    btnCopy.Enabled = false;

        //}

        public MR_DATA.REPORTS btnAppend_Click(string facility, string procedure, DataTable dtCopy)
        {
            //if (dtCopy.Rows.Count < 1)
            //{
            //    MessageBox.Show("No Definition Copied...");
            //    return;
            //}

            //DialogResult reply = MessageBox.Show("Confirm to Append copied details to this defintion..", "Material Definition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (reply == DialogResult.No)
            //    return;

            //btnAppend.Enabled = false;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();

            connection.Open();

            foreach (DataRow row in dtCopy.Rows)
            {
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = "mrb19_Add";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("FACILITY", facility);
                insertCommand.Parameters.AddWithValue("PROCESS", procedure);
                insertCommand.Parameters.AddWithValue("ITEM", row["item"].ToString());
                insertCommand.Parameters.AddWithValue("DESCRIPTION", row["description"].ToString());
                insertCommand.Parameters.AddWithValue("STORE", row["store"].ToString());
                insertCommand.Parameters.AddWithValue("QTY", Convert.ToDecimal(row["qty"]));
                insertCommand.Parameters.AddWithValue("UNIT", row["unit"].ToString());
                insertCommand.Parameters.AddWithValue("COST", Convert.ToDecimal(row["cost"]));
                insertCommand.Parameters.AddWithValue("SELL", Convert.ToDecimal(row["sell"]));
                insertCommand.Parameters.AddWithValue("NUMBOFTEST", Convert.ToDecimal(row["numboftest"]));
                insertCommand.Parameters.AddWithValue("TESTCOUNT", 0m);
                insertCommand.Parameters.AddWithValue("ONDIGITAL", Convert.ToBoolean(row["ondigital"]));
                insertCommand.Parameters.AddWithValue("SELECTABLE", Convert.ToBoolean(row["selectable"]));
                insertCommand.Parameters.AddWithValue("OPERATOR", woperator);
                insertCommand.Parameters.AddWithValue("dtime", DateTime.Now);

                insertCommand.ExecuteNonQuery();
                //  xcount++;
            }

            connection.Close();
            //worked = true;

            vm.REPORTS.alertMessage = "Appended Succesfully..";

            return vm.REPORTS;
        }

        //private void btnRemove_Click(object sender, EventArgs e)
        //{
        //    //if (dataGridView1.Rows.Count < 1)
        //    //    return;
        //    //DialogResult reply = MessageBox.Show("Confirm To Delete Record ..." + recno.ToString(), "Inv. Material Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    //if (reply == DialogResult.No)
        //    //    return;

        //    if (dataGridView1.Rows[recno].Cells[14].FormattedValue.ToString() != "" && Convert.ToInt32(dataGridView1.Rows[recno].Cells[14].Value) > 0)
        //    {
        //        string updstr = "delete from mrb19 where recid = '" + dataGridView1.Rows[recno].Cells[14].Value.ToString() + "'";
        //        bissclass.UpdateRecords(updstr, "MR");
        //    }

        //    dataGridView1.Rows.RemoveAt(recno);
        //    MessageBox.Show("Record Deleted...", "Issues/Requisition Item");
        //    renumberview();
        //}

        //void renumberview()
        //{
        //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
        //    {
        //        dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
        //    }
        //}

        //private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    int xc = e.RowIndex;
        //    dataGridView1.Rows[xc].Cells[0].Value = (e.RowIndex + 1).ToString();
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        public MR_DATA.REPORTS btnPrint_Click()
        {
            //if (string.IsNullOrWhiteSpace(cbofacility.Text))
            //{
            //    MessageBox.Show("Facility / Service Centre must be identified...", "Material Utilization Report");
            //    return;
            //}

            //DialogResult result = MessageBox.Show("Send to Printer <YES>, to Screen <NO>, Cancel To Ignore", "POS Receipt Re-Print", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.Cancel)
            //    return;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT * FROM MRB19 WHERE FACILITY = '" + vm.SYSCODETABSvm.ServiceCentreCodes.name + "' order by facility", false);

            if (dt.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "No data...";
                return vm.REPORTS;
            }

            dt.Columns.Add(new DataColumn("facilityname", typeof(string)));
            dt.Columns.Add(new DataColumn("processdesc", typeof(string)));
            string facilityname = "", xfacility = "";

            foreach (DataRow row in dt.Rows)
            {
                if (row["facility"].ToString().Trim() != xfacility)
                {
                    facilityname = bissclass.combodisplayitemCodeName("type_code", row["facility"].ToString(), dtfacility, "name");
                    xfacility = row["facility"].ToString().Trim();
                }
                row["facilityname"] = facilityname;
                row["processdesc"] = bissclass.combodisplayitemCodeName("reference", row["process"].ToString(), dttariff, "name");
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            vm.REPORTS.SessionSQL = "";
            //bool isprint = result == DialogResult.Yes ? true : false;
            vm.REPORTS.SessionRDLC = "InvMatDefinitionListing.rdlc";
            string mrptheader = "MATERIAL COSTING DEFINITION ";


            //if (!isprint)
            //{

            frmReportViewer paedreports = new frmReportViewer(mrptheader, "INVESTIGATION MATERIAL DEFINITION LIST", 
                "", "", "", "TARIFFLIST", "", 0m, "", "", "", ds, true, 0, DateTime.Now.Date, DateTime.Now.Date,
                "", false, "", "", vm.REPORTS);

            vm.REPORTS = paedreports.Show(vm.REPORTS.SessionRDLC, vm.REPORTS.SessionSQL, vm.REPORTS.PRINT);

            //}
            //else
            //{
            //    MRrptConversion.GeneralRpt(this.Text, "INVESTIGATION MATERIAL DEFINITION LIST", "", "", "", "TARIFFLIST", "", 0m, "", "", "", ds, 0, DateTime.Now.Date, DateTime.Now.Date, "", isprint, true, "", "");
            //}
            return vm.REPORTS;

        }
    }
}