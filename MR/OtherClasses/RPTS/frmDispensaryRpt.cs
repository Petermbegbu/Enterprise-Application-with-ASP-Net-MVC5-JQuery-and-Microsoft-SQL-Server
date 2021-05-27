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
using SCS.BissClass;


using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
	public partial class frmDispensaryRpt : Form
	{
		billchaindtl bchain = new billchaindtl();
	   // Admrecs admrecs = new Admrecs();
		string lookupsource, AnyCode, mrptheader, sysmodule = bissclass.getRptfooter();
		DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
		DataSet ds = new DataSet();
		DataTable Tsdt, sdt, dtcostcentre = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM COSTCENTRECODES order by name", true), dtstore = Dataaccess.GetAnytable("", "SMS", "select storecode, name from store order by name", true), dtcust, dtdocs = Dataaccess.GetAnytable("", "MR", "select reference,name from doctors", false);
		decimal gtotal;
		public frmDispensaryRpt()
		{
			InitializeComponent();
		}
		private void frmDispensaryRpt_Load(object sender, EventArgs e)
		{
			initcomboboxes("S");
		}
		private void initcomboboxes(string xtype)
		{
			cboDispensingUnit.DataSource = xtype == "S" ? dtstore : dtcostcentre;
			cboDispensingUnit.ValueMember = xtype == "S" ? "storecode" : "type_code";
			cboDispensingUnit.DisplayMember = "name";
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
			else if (btn.Name == "btngrouphead")
			{
				txtgrouphead.Text = "";
				lookupsource = "C";
				msmrfunc.mrGlobals.crequired = "C";
				msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORP. CLIENTS";
			}
			else if (btn.Name == "btnStock")
			{
				txtStock.Text = "";
				lookupsource = "s";
				msmrfunc.mrGlobals.crequired = "s";
				msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED STOCK ITEMS IN ALL STORES";
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
			else if (lookupsource == "C") //patientno
			{
				txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
				txtgrouphead.Select();
			}
			else if (lookupsource == "s")
			{
				txtStock.Text = AnyCode = msmrfunc.mrGlobals.anycode1;
				txtStock.Focus();
				return;
			}
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
			lblname.Text = bchain.NAME;
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
			lblname.Text = dtcustomer.Rows[0]["name"].ToString();
		}
		private void chkStore_Click(object sender, EventArgs e)
		{
			RadioButton rbn = sender as RadioButton;
			if (rbn.Name == "chkStore")
				initcomboboxes("S");
			else
				initcomboboxes("C");
		}
		private void txtStock_LostFocus(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtStock.Text))
				return;

			DataTable stock = Dataaccess.GetAnytable("", "SMS", "select name from stock where item = '"+txtStock.Text+"'", false);

			if (stock.Rows.Count < 1)
			{
				DialogResult result = MessageBox.Show("Undefined Stock Item....", "Stock Definitions");
				txtStock.Text = "";
				return;
			}
			lblstock.Text = stock.Rows[0]["name"].ToString();
			txtDescSearch.Text = "";
			txtDescSearch.Enabled = false;
		}
		void createSummary()
		{
			Tsdt = new DataTable(); //table to will be passed to report dataset 
			Tsdt.Columns.Add(new DataColumn("trans_date", typeof(DateTime)));
			Tsdt.Columns.Add(new DataColumn("NAME", typeof(string)));
			Tsdt.Columns.Add(new DataColumn("groupcode", typeof(string)));
			Tsdt.Columns.Add(new DataColumn("patientno", typeof(string)));
			Tsdt.Columns.Add(new DataColumn("nhis", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("phis", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("company", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("pvt", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("fc", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("staff", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("cost", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("TOTALAMT", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("CLOSINGSTK", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("BALBF", typeof(decimal)));
			Tsdt.Columns.Add(new DataColumn("Percentage", typeof(decimal)));
		}
		bool getcustHMO(string xgrouphead, ref string billcircle)
		{
			bool foundit = false;
			foreach (DataRow row in dtcust.Rows )
			{
				if (row["custno"].ToString().Trim() == xgrouphead )
				{
					billcircle = row["BILL_CIR"].ToString();
					foundit = true;
					break;
				}
		 
			}
			return foundit;
		}
		DataRow createnewRow(DataRow drow)
		{
			bool foundit = false;
			DataRow dr = null;

			foreach (DataRow row in Tsdt.Rows)
			{
				if (chkDailyprescritnrpt.Checked && drow["GROUPCODE"].ToString() == row["groupcode"].ToString() && drow["patientno"].ToString() == row["patientno"].ToString() && Convert.ToDateTime(drow["trans_date"]).Date == Convert.ToDateTime(row["trans_date"]).Date || chkStockUtilSumm.Checked && row["name"].ToString().Trim() == drow["stk_desc"].ToString().Trim()) // && Convert.ToDateTime(drow["trans_date"]).Date == Convert.ToDateTime(row["trans_date"]).Date )
				{
					foundit = true;
					dr = row;
					break;
				}
			}
			if (!foundit)
			{
				dr = Tsdt.NewRow();
				dr["groupcode"] = chkDailyprescritnrpt.Checked ? drow["GROUPCODE"].ToString() : drow["stk_item"].ToString();
				dr["patientno"] = chkDailyprescritnrpt.Checked ? drow["patientno"].ToString() : drow["store"].ToString();
				dr["name"] = chkDailyprescritnrpt.Checked ? drow["name"].ToString() : drow["stk_desc"].ToString();
				dr["trans_date"] = (DateTime) drow["trans_date"];
				dr["nhis"] = 0;
				dr["phis"] = 0;
				dr["company"] = 0;
				dr["pvt"] = 0;
				dr["fc"] = 0;
				dr["staff"] = 0;
				dr["cost"] = 0;
				dr["TOTALAMT"] = 0;
				dr["BALBF"] = 0;
				dr["CLOSINGSTK"] = 0;
				dr["percentage"] = 0m;
				Tsdt.Rows.Add(dr);
			}
			decimal xamt = chkQtyDispensed.Checked ? Convert.ToDecimal(drow["CUMGV"]) : Convert.ToDecimal(drow["cost"]);
			string xtype = drow["grouphtype"].ToString();
			if (drow["groupcode"].ToString().Trim() == "PVT" ) //|| !string.IsNullOrWhiteSpace(drow["groupcode"].ToString()))
				dr["pvt"] = (decimal)dr["pvt"] + xamt;
			else if (drow["groupcode"].ToString().Trim() == "FC")
				dr["fc"] = (decimal)dr["fc"] + xamt;
			else if (drow["groupcode"].ToString().Trim() == "NHIS")
				dr["NHIS"] = (decimal)dr["NHIS"] + xamt;
			else if (drow["groupcode"].ToString().Trim() == "PHIS" )
				dr["phis"] = (decimal)dr["phis"] + xamt;
			else if (bissclass.sysGlobals.user_name.Substring(0,7) == "CAPITOL" && drow["groupcode"].ToString().Trim() == "CHC")
				dr["staff"] = (decimal)dr["staff"] + xamt;
			else if (drow["grouphtype"].ToString() == "C")
			{
				string xbc = "";
				bool hmo = getcustHMO(drow["grouphead"].ToString().Trim(), ref xbc);
				if (hmo)
					dr["phis"] = (decimal)dr["phis"] + xamt;
				else if (xbc == "H")
					dr["staff"] = (decimal)dr["staff"] + xamt;
				else
					dr["company"] = (decimal)dr["company"] + xamt;
			}
			dr["totalamt"] = (decimal)dr["totalamt"] + xamt;
			dr["cost"] = (decimal)dr["cost"] + Convert.ToDecimal(drow["unitpurvalue"]);
			gtotal += (decimal)dr["totalamt"]; // +xamt;

			return dr;
		}
		void getData()
		{
			if (chkDailyprescritnrpt.Checked || chkStockUtilSumm.Checked )
			{
				dtcust = Dataaccess.GetAnytable("","MR","SELECT hmo, custno, bill_cir from customer",false);
			}
			bool lhistory = false;
			if (dtDateFrom.Value.Year < msmrfunc.mrGlobals.mpyear)
				lhistory = true;

			string rptfile = lhistory ? "Dispensehist" : "Dispensa";
			string selstring = " WHERE trans_date >= '"+dtDateFrom.Value.ToShortDateString()+"' and trans_date <= '"+dtDateto.Value.ToShortDateString()+" 23:59:59.999'";
 
			if (!string.IsNullOrWhiteSpace(txtStock.Text))
				selstring += " and stk_item = '"+txtStock.Text+"'";
			else if (!string.IsNullOrWhiteSpace(txtDescSearch.Text))
				selstring += " AND STK_DESC LIKE '%" + txtDescSearch.Text.Trim() + "%' ";
			if (!string.IsNullOrWhiteSpace(cboDispensingUnit.Text))
				selstring += " and store = '"+cboDispensingUnit.SelectedValue.ToString()+"'";
			if (chkUndispensed.Checked )
				selstring += " and qty_gv = '0'";
			if (chkDispensed.Checked)
				selstring += " and qty_gv >= '1'";
			if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
				selstring += " AND GROUPHEAD = '" + txtgrouphead.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
				selstring += " AND GROUPCODE = '" + txtgroupcode.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
				selstring += " AND PATIENTNO = '" + txtpatientno.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtNameSearch.Text))
				selstring += " AND NAME LIKE '%" + txtNameSearch.Text.Trim() + "%' ";

			string xstr = "SELECT PATIENTNO, GROUPCODE, TRANS_DATE, STK_ITEM, STK_DESC, STORE, QTY_PR, QTY_GV, CUMGV, DOSE, INTERVAL, DURATION, UNIT, COST, NURSE, CHAR(50) AS DOCTOR, DOCTOR AS XDOC, NAME, TYPE, GROUPHEAD, GHGROUPCODE, GROUPHTYPE, OPERATOR, OP_TIME, UNITCOST, RX, CDOSE, SP_INST, CINTERVAL, CDURATION, REFERENCE, unitpurvalue, CHAR(50) AS STORENAME, CHAR(50) AS GHNAME, STKBAL, TIME from dispensa "+selstring;
			sdt = Dataaccess.GetAnytable("", "MR", xstr, false);
			string store, storename,docsave,docname; store = storename = docsave = docname = "";
			gtotal = 0m;
			foreach (DataRow row in sdt.Rows )
			{
				if (chkDailyprescritnrpt.Checked || chkStockUtilSumm.Checked)
					createnewRow(row);
				else
				{
					if (row["store"].ToString().Trim() != store)
					{
						storename = bissclass.combodisplayitemCodeName("storecode", row["store"].ToString(), dtstore, "name");
						store = row["store"].ToString().Trim();
					}
					row["storename"] = storename;
				}
				if (docsave != row["xdoc"].ToString().Trim())
				{
					if (row["xdoc"].ToString().Trim().Length < 5)
						docname = bissclass.combodisplayitemCodeName("reference", row["xdoc"].ToString(), dtdocs, "name");
					else
						docname = row["xdoc"].ToString().Trim();

					docsave = row["xdoc"].ToString().Trim();
				}
				row["doctor"] = docname;

			}
			//get opening and closing bal for qty dispsensed option
			if (chkStockUtilSumm.Checked)
			{
				//closing = bf+receipt-issues; how do we get receipts?2.12.2018
				//I think we should get a more accurate figure from purchases
				//patientno = store; groupcode = item
				DataTable dt;
				decimal cost = 0m, sell = 0m, qty = 0m, receipt = 0m;
				foreach (DataRow row in Tsdt.Rows )
				{
					receipt = 0m;
					//get receipt from purchases
					dt = Dataaccess.GetAnytable("", "AP", "select sum(qty) as qty, sum(amount) as amount, product from vctdetail WHERE trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999' and rtrim(product) = '" + row["groupcode"].ToString().Trim() + "' group by product", false);
					if (dt.Rows.Count > 0) { receipt = chkQtyDispensed.Checked ? (decimal)dt.Rows[0]["qty"] : (decimal)dt.Rows[0]["amount"]; }

					scsfunc.GetStockStatusFromTransmas(row["patientno"].ToString(), row["groupcode"].ToString(), dtDateFrom.Value.Date, ref cost, ref sell, ref qty, dtDateFrom.Value.Date);
					row["balbf"] = chkQtyDispensed.Checked ? qty : cost;
					row["cost"] = receipt;
					receipt += chkQtyDispensed.Checked ? qty : cost;
					row["closingstk"] = receipt - (decimal)row["totalamt"]; //totalamt = issues
					row["percentage"] = ((decimal)row["totalamt"] * 100) / gtotal;
					if (row["name"].ToString().Trim() == "PARACETAMOL TABLET")
					{
						MessageBox.Show("total :" + row["totalamt"].ToString() + " Gotal : "+gtotal.ToString()+" % " + row["percentage"].ToString());
					}
				}
			}
 
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		void printprocess(bool isprint)
		{
			DialogResult result;
			if (dtDateFrom.Value.Date > DateTime.Now.Date || dtDateFrom.Value.Date < msmrfunc.mrGlobals.mta_start || dtDateto.Value.Date < dtDateFrom.Value.Date )
			{
				result = MessageBox.Show("Invalid Date Specification");
				return;
			}
			ds = new DataSet();
			Tsdt = new DataTable();
			if (chkDailyprescritnrpt.Checked || chkStockUtilSumm.Checked)
				createSummary();


			getData();
			if (sdt.Rows.Count < 1)
			{
				result = MessageBox.Show("No Data for Specified Conditions...");
				return;
			}
			if (chkDailyprescritnrpt.Checked || chkStockUtilSumm.Checked)
				ds.Tables.Add(Tsdt);
			else
				ds.Tables.Add(sdt);
			Session["sql"] = "";
			if (chkByDatePatient.Checked)
				Session["rdlcfile"] = "Dispenary_DatePatient.rdlc";
			else if (chkbyStockItemPatient.Checked)
				Session["rdlcfile"] = "Dispenary_StkItemPatient.rdlc";
			else if (chkValueofPrescription.Checked)
				Session["rdlcfile"] = "Dispenary_DailySumStkValue.rdlc";
			else
				Session["rdlcfile"] = "Dispenary_DailySumStkItem.rdlc";
			//else
			//    Session["rdlcfile"] = "Dispenary_DailySumPat.rdlc";

			if (chkStockUtilSumm.Checked)
				mrptheader = "DISPENSARY SUMMARY/FREQUENCY RATE BY STOCK ITEM";
			else if (chkDailyprescritnrpt.Checked)
				mrptheader = "DAILY DISPENSARY SUMMARY BY PATIENT";
			else
			{
				string xstr = chkByDatePatient.Checked ? "DATE/PATIENT" : "STOCK ITEM/PATIENT";
				mrptheader = "DISPENSARY REPORT BY " + xstr;
			}

			mrptheader += " FOR "+dtDateFrom.Value.ToShortDateString()+" TO : "+dtDateto.Value.ToShortDateString();
			if (!isprint)
			{
				frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, "", "", "", "DISPENSARYRPT", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

				//if (isprint)
				//    paedreports.work();
				//else
				paedreports.Show();
			}
			else
			{
				MRrptConversion.GeneralRpt(this.Text, mrptheader, "", "", "", "DISPENSARYRPT", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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

		private void chkStockUtilSumm_CheckedChanged(object sender, EventArgs e)
		{
			if (chkStockUtilSumm.Checked || chkDailyprescritnrpt.Checked )
				panel_QtyValue.Visible = true;
			else
				panel_QtyValue.Visible = false;
		}

	}
}