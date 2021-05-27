#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SCS.DataAccess;
using msfunc;
using mradmin.BissClass;
using mradmin.DataAccess;
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmStkitemsonbill
    {
        Stock stock = new Stock();
        decimal itemsave;
        int recno;
        string lookupsource, AnyCode, mreference;

        //public frmStkitemsonbill(string xreference)
        //{
        //    //InitializeComponent();
        //    mreference = xreference;

        //    msmrfunc.mrGlobals.anycode1 = nmrCummTotal.Value < 1 ? "" : nmrCummTotal.Value.ToString();
        //}

        //private void frmStkitemsonbill_Load(object sender, EventArgs e)
        //{
        //    itemsave = 0m;
        //    DataTable dtstk = Dataaccess.GetAnytable("", "MR", "select * from mrb25 where reference = '" + mreference + "'", false);
        //    DataGridViewRow dgv;
        //    int xc = 0;
        //    foreach (DataRow row in dtstk.Rows )
        //    {
        //        dataGridView1.Rows.Add();
        //        dgv = dataGridView1.Rows[xc];
        //        itemsave++;
        //        dgv.Cells[0].Value = itemsave.ToString();
        //        dgv.Cells[1].Value = row["stk_item"].ToString();
        //        dgv.Cells[3].Value = row["stk_dec"].ToString();
        //        dgv.Cells[4].Value = row["qty"].ToString();
        //        dgv.Cells[5].Value = row["unit"].ToString();
        //        dgv.Cells[6].Value = row["unitcost"].ToString();
        //        dgv.Cells[7].Value = row["cost"].ToString();
        //        dgv.Cells[8].Value = row["recid"].ToString();
        //        nmrCummTotal.Value += (decimal)row["cost"];
        //        xc++;
        //    }

        //    msmrfunc.mrGlobals.anycode1 = nmrCummTotal.Value < 1 ? "" : nmrCummTotal.Value.ToString();
        //}

        //void initgridcombos(int xrow)
        //{
        //    DataGridViewButtonCell btcol1 = (DataGridViewButtonCell)(dataGridView1.Rows[xrow].Cells[1]);
        //    btcol1.UseColumnTextForButtonValue = true; 

        //}

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (itemsave > 0)
            //{
            //    int xrow = Convert.ToInt32(itemsave - 1);
            //    if (dataGridView1.Rows[xrow].Cells[2].Value == null ||
            //        dataGridView1.Rows[xrow].Cells[5].Value == null ||
            //        Convert.ToDecimal(dataGridView1.Rows[xrow].Cells[5].Value) < 1)
            //    {
            //        DialogResult result = MessageBox.Show("Generated record space has not been fully utilized...", "New record Add");
            //        return;
            //    }
            //}
            //DataGridViewRow row = new DataGridViewRow();
            //dataGridView1.Rows.Add();
            //itemsave++;
            //int xrec = Convert.ToInt32(itemsave - 1);
            //dataGridView1.Rows[xrec].Cells[0].Value = itemsave;
            //initgridcombos(xrec);
            //recno = xrec;
            //return;
        }

        //private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
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
        //            cell.ToolTipText = "Select Stock Code or Click the Lookup Button (...) for Lookup on Defined Products";
        //        else if (e.ColumnIndex == 2) 
        //            cell.ToolTipText = "Lookup Button on Defined Stock Items";
        //        else if (e.ColumnIndex == 4)
        //            cell.ToolTipText = "Enter Quantity To Bill";
        //        else if (e.ColumnIndex == 5)
        //            cell.ToolTipText = "Product Unit of Measure"; //Qty of this product the Customer requested for";
        //        else if (e.ColumnIndex == 6)
        //            cell.ToolTipText = "Unit Cost of Product";
        //    }
        //}

        //private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridViewRow dgv = new DataGridViewRow();
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        dgv = dataGridView1.Rows[e.RowIndex];
        //        if (dgv.Cells[1].Value != null && dgv.Cells[1].FormattedValue.ToString() != "")
        //        {
        //            DialogResult result;
        //            btnRemove.Enabled = true;
        //            recno = e.RowIndex;

        //            stock = Stock.GetStock("", dgv.Cells[1].FormattedValue.ToString(), true);
        //            if (stock == null)
        //            {
        //                result = MessageBox.Show("Selected Stock is not registered...", "Stock Master File");
        //                dgv.Cells[1].Value = "";
        //                return;
        //            }

        //            //09.06.2019 - how about the hmo or nhis tariff definiton for this product?
        //            dgv.Cells[3].Value = stock.Name;
        //            dgv.Cells[5].Value = stock.Unit;
        //            dgv.Cells[6].Value = stock.Sell;
        //        }
        //        else if ((e.ColumnIndex == 4 || e.ColumnIndex == 6) && dgv.Cells[4].Value != null & Convert.ToDecimal(dgv.Cells[4].Value) > 0m && dgv.Cells[6].Value != null && Convert.ToDecimal(dgv.Cells[6].Value) > 0m ) 
        //        {
        //            dgv.Cells[7].Value = Convert.ToDecimal(dgv.Cells[4].Value) * Convert.ToDecimal(dgv.Cells[6].Value);
        //            btnSubmit.Enabled = true;
        //        }
        //    }
        //}

        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == 2)
        //    {
        //        recno = e.RowIndex;
        //        lookupsource = "STK";
        //        msmrfunc.mrGlobals.crequired = "s"; //Stock
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED STOCK ITEMS IN ALL STORES";

        //        frmselcode FrmSelCode = new frmselcode();
        //        FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //        FrmSelCode.ShowDialog();
        //    }
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e) 
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    msmrfunc.mrGlobals.lookupCriteria = "";
        //    if (lookupsource == "STK") //stock
        //    {
        //        dataGridView1.Rows[recno].Cells[1].Value = AnyCode = msmrfunc.mrGlobals.anycode1;
        //        dataGridView1.Rows[recno].Cells[1].Selected = true;
        //    }
        //    return;
        //}

        //private void btnRemove_Click(object sender, EventArgs e)
        //{
        //    if (dataGridView1.Rows.Count < 1)
        //        return;

        //    DialogResult result;
        //    //if (dataGridView1.Rows[recno].Cells[13].FormattedValue.ToString() == "POSTED")
        //    //{
        //    //    result = MessageBox.Show("This Record can't be Removed...Its Confirmed !", "");
        //    //    return;
        //    //}

        //    result = MessageBox.Show("Delete Record..." + dataGridView1.Rows[recno].Cells[3].FormattedValue.ToString() + ".?", "Delete Stock Item From Table", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (result == DialogResult.No)
        //        return;
        //    {
        //        if (Convert.ToDecimal(dataGridView1.Rows[recno].Cells[8].Value) > 0)
        //        {
        //            string updatestring = "DELETE from mrb25 WHERE RECID = '" + dataGridView1.Rows[recno].Cells[8].Value.ToString() + "' ";

        //            if (bissclass.UpdateRecords(updatestring, "MR"))
        //            {
        //                MessageBox.Show("Record Deleted...", "Stock Item on Bill");
        //            }
        //        }

        //        dataGridView1.Rows.RemoveAt(recno);
        //    }
        //}

        //private void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    DialogResult result = MessageBox.Show("Confirm to Save Records.....", "Stock Items on Bills", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //    if (result == DialogResult.No)
        //        return;
        //    /*     PARACETAMOL TABLET (18 @ 10.00 : 180.00)
        //            COARTEM 80/480MG TAB (24 @ 400.00 : 9600.00)*/

        //    string[,] itema_ = new string[10, 7];

        //    for (int i = 0; i < 10; i++)
        //    {
        //        for (int ia = 0; ia < 7; ia++)
        //        {
        //            itema_[i, ia] = "";
        //        }

        //    }

        //    DataGridViewRow row;
        //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
        //    {
        //        row = dataGridView1.Rows[i];
        //        if (row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[3].FormattedValue.ToString()) || Convert.ToDecimal(row.Cells[4].Value) < 1 || Convert.ToDecimal(row.Cells[6].Value) < 1 || Convert.ToDecimal(row.Cells[7].Value) < 1)
        //            continue;
        //        itema_[i, 0] = row.Cells[0].Value.ToString();
        //        itema_[i, 1] = row.Cells[1].FormattedValue.ToString();
        //        itema_[i, 2] = row.Cells[3].FormattedValue.ToString();
        //        itema_[i, 3] = row.Cells[4].Value.ToString();
        //        itema_[i, 4] = row.Cells[5].FormattedValue.ToString();
        //        itema_[i, 5] = row.Cells[6].Value.ToString();
        //        itema_[i, 6] = row.Cells[7].Value.ToString();
        //        if (i > 10)
        //            break;
        //    }

        //    Session["stkitems"] = (Array)itema_;
        //    //  Session["totamt"] = (Decimal)nmrCummTotal.Value;
        //    msmrfunc.mrGlobals.anycode1 = nmrCummTotal.Value < 1 ? "" : nmrCummTotal.Value.ToString();
        //    btnClose.PerformClick();
        //}

        //private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    //int xrow = e.RowIndex;
        //    /*    if (itemsave > 0)
        //        {
        //            xrow = Convert.ToInt32(itemsave - 1);
        //        }*/
        //    DataGridViewRow dgv = dataGridView1.Rows[e.RowIndex];
        //    //  dataGridView1.Rows.Add();
        //    itemsave++;
        //    // int xrec = Convert.ToInt32(itemsave - 1);
        //    dgv.Cells[0].Value = itemsave.ToString();
        //    // initgridcombos(e.RowIndex);
        //    recno = e.RowIndex;
        //    return;
        //}

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //do nothing
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}