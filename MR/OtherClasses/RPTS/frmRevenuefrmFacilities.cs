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
    public partial class frmRevenuefrmFacilities : Form
    {
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtcurrency = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM currencycodes order by name", true), dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dttariff = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE, NAME FROM TARIFF  order by name", true), dtcust = Dataaccess.GetAnytable("", "MR", "SELECT custno, hmo FROM customer", false );
        int mprog;
        public frmRevenuefrmFacilities(int xprog)
        {
            InitializeComponent();
            mprog = xprog;
            if (mprog == 1)
                lblFacility.Visible = cboFacility.Visible = true;
            else
            {
                lblProcedure.Visible = cboProcedure.Visible = true;
                this.Text = "ACCOUNTS SUMMARY BY PROCEDURE (TARIFF ITEM)";
            }
        }
        private void frmRevenuefrmFacilities_Load(object sender, EventArgs e)
        {
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            cboCurrency.DataSource = dtcurrency;
            cboCurrency.ValueMember = "type_code";
            cboCurrency.DisplayMember = "Name";

            cboFacility.DataSource = dtfacility;
            cboFacility.ValueMember = "Type_code";
            cboFacility.DisplayMember = "name";
            
            cboProcedure.DataSource = dttariff;
            cboProcedure.ValueMember = "Reference";
            cboProcedure.DisplayMember = "Name";
        }
        void createSummary()
        {
            sdt = new DataTable(); //table to will be passed to report dataset 
            sdt.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
            sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
            sdt.Columns.Add(new DataColumn("FREQUENCY", typeof(int)));
            sdt.Columns.Add(new DataColumn("PVTFAMILY", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("CORPORATE", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("HMO", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("NHIS", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("TOTALAMT", typeof(decimal)));
        }
        DataRow createnewRow(DataRow drow)
        {
            bool foundit = false, ishmo = false;
            DataRow dr = null;
            string xreference = "";

            foreach (DataRow row in sdt.Rows)
            {
                if (mprog == 1)
                    xreference = string.IsNullOrWhiteSpace( drow["FACILITY"].ToString().Trim()) ? "UNSPECIFIED" : drow["FACILITY"].ToString().Trim();
                else
                    xreference = string.IsNullOrWhiteSpace(drow["PROCESS"].ToString().Trim()) ? "UNSPECIFIED" : drow["PROCESS"].ToString().Trim();
                if (row["reference"].ToString() == xreference )
                {
                    dr = row;
                    foundit = true;
                    break;
                }
            }
            if (!foundit)
            {
                dr = sdt.NewRow();
                dr["reference"] = xreference; 
                dr["name"] = xreference == "UNSPECIFIED" ? "< UNSPECIFIED >" : bissclass.combodisplayitemCodeName(mprog == 1 ?  "type_code" : "reference", mprog == 1 ? drow["facility"].ToString() : drow["process"].ToString(), mprog == 1 ? dtfacility : dttariff, "name");
                dr["FREQUENCY"] = 0;
                dr["PVTFAMILY"] = 0;
                dr["CORPORATE"] = 0;
                dr["HMO"] = 0;
                dr["NHIS"] = 0;
                dr["TOTALAMT"] = 0;
                sdt.Rows.Add(dr);
            }
            dr["FREQUENCY"] = (int)dr["frequency"] + 1;
            if (drow["transtype"].ToString() == "P")
                dr["PVTFAMILY"] = (decimal)dr["pvtfamily"] + (decimal)drow["amount"];
            else if (drow["groupcode"].ToString().Trim() == "NHIS")
                dr["NHIS"] = (decimal)dr["NHIS"] + (decimal)drow["amount"];
            else 
            {
                for (int i = 0; i < dtcust.Rows.Count; i++)
                {
                    if (dtcust.Rows[i]["custno"].ToString().Trim() == drow["grouphead"].ToString().Trim() )
                    {
                        ishmo = (bool)dtcust.Rows[i]["hmo"];
                        break;
                    }
                }
                if (ishmo)
                    dr["HMO"] = (decimal)dr["hmo"] + (decimal)drow["amount"];
                else
                    dr["CORPORATE"] = (decimal)dr["corporate"] + (decimal)drow["amount"];
            }
            dr["TOTALAMT"] = (decimal)dr["pvtfamily"] + (decimal)dr["nhis"] + (decimal)dr["hmo"] + (decimal)dr["corporate"];
            return dr;
        }
        void getData()
        {
            string bstr = "", pstr = "";
            bstr = " WHERE BILLING.TRANS_DATE >= '" + dtDateFrom.Value.ToShortDateString() + "' and BILLING.TRANS_DATE <= '" + dtDateto.Value.ToShortDateString()+ " 23:59:59:999'";
            pstr = " WHERE PAYDETAIL.TRANS_DATE >= '" + dtDateFrom.Value.ToShortDateString() + "' and PAYDETAIL.TRANS_DATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59:999'";
            if (mprog == 1 && !string.IsNullOrWhiteSpace(cboFacility.Text))
                bstr += " and facility = '"+cboFacility.SelectedValue.ToString()+"'";
            else if (mprog == 2 && !string.IsNullOrWhiteSpace(cboProcedure.Text))
                bstr += " and process = '"+cboProcedure.SelectedValue.ToString();
            if (!chkIncludeProcessHMONHIS.Checked)
                bstr += " and (left(description,10) != 'NHIS MTHLY' or left(description,9) != 'HMO MTHLY')";
            if (!string.IsNullOrWhiteSpace(txtDescSearch.Text))
                bstr += " AND DESCRIPTION LIKE '%" + txtDescSearch.Text.Trim() + "%' ";
            if (!string.IsNullOrWhiteSpace(cboCurrency.Text))
                bstr += " and currency = '" + cboCurrency.SelectedValue.ToString() + "'";
            bstr += " and ttype = 'D'";
            //start_opening = bissclass.ConvertStringToDateTime("01", msmrfunc.mrGlobals.mlastperiod == 12 ? "01" : msmrfunc.mrGlobals.mlastperiod + 1.ToString(), msmrfunc.mrGlobals.mlastperiod == 12 ? msmrfunc.mrGlobals.mpyear + 1.ToString() : msmrfunc.mrGlobals.mpyear.ToString());
            Session["PAYMENTS"] = "0";
            string xstring = "";
            xstring = "SELECT BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD, BILLING.TTYPE, BILLING.GHGROUPCODE, billing.process, billing.facility, billing.groupcode FROM BILLING " + bstr;

            DataTable tsdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            foreach (DataRow row in tsdt.Rows)
            {
                createnewRow(row);
            }
            if (chkIncludePmts.Checked)
            {
                xstring = "SELECT SUM(AMOUNT) AS AMOUNT FROM PAYDETAIL " + pstr;
                tsdt = Dataaccess.GetAnytable("", "MR", xstring, false);
                Session["PAYMENTS"] = tsdt.Rows[0]["amount"].ToString();
            }
            ds.Tables.Add(sdt);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateto.Value < dtDateFrom.Value || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateto.Value.Date > DateTime.Now.Date)
            {
                result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            if (dtDateFrom.Value.Year != dtDateto.Value.Year)
            {
                result = MessageBox.Show("Date specification for must be within the same yaer...");
                return;
            }
            Session["sql"] = "";
            if (chkMthlyComparative.Checked)
            {
                if (ds != null)
                {
                    ds.Tables.Clear();
                    ds.Clear();
                }
                string xrpttype = mprog == 1 ? "FACILITY" : "PROCESS";
                DataTable dtmthlyfig = new DataTable(), dtaggregate = new DataTable();
                msmrfunc.processComparative(ref dtmthlyfig, ref dtaggregate, dtDateFrom.Value, dtDateto.Value, xrpttype, "");
                ds.Tables.Add(dtmthlyfig);
                ds.Tables.Add(dtaggregate);
            }
            else
            {
                if (sdt == null)
                    createSummary();
                else
                {
                    sdt.Rows.Clear();
                    ds.Tables.Clear();
                    ds.Clear();
                }
                getData();
            }
            string papertype = chkCummulativeSumm.Checked ? "" : "W";
            string mrptheader = mprog == 1 ? "REVENUE FROM FACILITIES/SERVICE CENTRES "  : this.Text;
            Session["rdlcfile"] = chkCummulativeSumm.Checked ? "RevenueByFacilities.rdlc" : "ComparativeRpt.rdlc";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader + " FOR : " + dtDateFrom.Value.ToShortDateString() + " TO " + dtDateto.Value.ToShortDateString(), mrptheader, "", "", "", "REVENUEFRMFACILITIES", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, papertype, "");
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader + " FOR : " + dtDateFrom.Value.ToShortDateString() + " TO " + dtDateto.Value.ToShortDateString(), mrptheader, "", "", "", "REVENUEFRMFACILITIES", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, papertype, "");
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

        private void chkMthlyComparative_Click(object sender, EventArgs e)
        {
            string xyear = dtDateFrom.Value.Year.ToString();
            dtDateFrom.Value = bissclass.ConvertStringToDateTime("01", "01", xyear);
            dtDateto.Value = bissclass.ConvertStringToDateTime("31", "12 ", xyear);
        }
        //void processComparative(ref DataTable MonthlyFigTable, ref DataTable aggregateTable, DateTime datefrom, DateTime dateto, string recordtype, string customertype)
        //{
        //    string recselect = customertype == "C" ? "transtype = 'C' " : customertype == "P" ? "transtype = 'P'" : "transtype like [CP ] ";
        //    string selstring = "";
        //    if (recordtype == "CLIENTS")
        //        selstring = "select Sum(amount) as amount, ghgroupcode, grouphead, grouphtype from billing where " + recselect + " group by grouphead, ghgroupcode, grouphtype";
        //    else if (recordtype == "PROCESS")
        //        selstring = "select Sum(amount) as amount, process, description from billing group by process, description";

        //    DataTable dt;
        //    for (int i = 1; i < 13; i++)
        //    {
        //        dt = Dataaccess.GetAnytable("", "MR", selstring + " and MONTH(trans_date) = '" + i.ToString() + "' ", false);
        //        if (i > 1)
        //            sortRecords(ref MonthlyFigTable, dt, i, recordtype );
        //        else
        //            MonthlyFigTable = dt;
        //    }
        //    //calculate aggregate
        //    DataRow arow;
        //    if (aggregateTable == null)
        //        arow = aggregateTable.NewRow();
        //    else
        //        aggregateTable.Clear();
        //    arow = aggregateTable.Rows[0];
        //    decimal gtotal = 0m;
        //    foreach (DataRow row in MonthlyFigTable.Rows)
        //    {
        //        for (int i = 1; i < 13; i++) //add up all monthly figures to 12
        //        {
        //            arow["amt" + (i + 1).ToString()] = (decimal)arow["amt" + (i + 1).ToString()] + (decimal)row["amt" + (i + 1).ToString()];
        //        }
        //        gtotal += (decimal)row["total"];
        //    }
        //    //find aggregate
        //    for (int i = 1; i < 13; i++) //add up all monthly figures to 12
        //    {
        //        //(ma_[xcount]/gamt)*100
        //        arow["amt" + (i + 1).ToString()] = ((decimal)arow["amt" + (i + 1).ToString()] / gtotal) * 100;
        //    }
        //}
        //void sortRecords(ref DataTable mdt, DataTable xtdt, int mth, string rectype)
        //{
        //    bool foundit = false;
        //    DataRow drow = null;
        //    foreach (DataRow row in xtdt.Rows )
        //    {
        //        foundit = false;
        //        foreach (DataRow row2 in mdt.Rows )
        //        {
        //            if (rectype == "CLIENTS" ? row2["reference"].ToString().Trim() == row["ghgroupcode"].ToString().Trim() + row["grouphead"].ToString().Trim() : row2["reference"].ToString().Trim() == row["process"].ToString().Trim() && row2["name"].ToString().Trim() == row["description"].ToString().Trim())
        //            {
        //                drow = row2;
        //                foundit = true;
        //                break;
        //            }
        //        }
        //        if (!foundit)
        //        {
        //            drow = mdt.NewRow();
        //            drow["reference"] = rectype == "CLIENTS" ? row["ghgroupcode"].ToString().Trim() + row["grouphead"].ToString().Trim() : row["process"].ToString().Trim();
        //            if (rectype == "PROCESS")
        //                drow["name"] = row["description"].ToString();
        //            else
        //                drow["name"] = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), row["grouphtype"].ToString());
        //        }
        //        drow["amt"+mth.ToString()] = (decimal)row["amount"];
        //        drow["total"] = (decimal)drow["total"] + (decimal)row["amount"];
        //    }
        //}
    }
}