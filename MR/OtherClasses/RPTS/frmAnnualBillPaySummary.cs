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
	public partial class frmAnnualBillPaySummary : Form
	{
		billchaindtl bchain = new billchaindtl();
		string lookupsource, AnyCode, mrptheader, sysmodule = bissclass.getRptfooter();
		DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
		DataSet ds = new DataSet();
		DataTable sdt, dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtdocs = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE,NAME FROM DOCTORS WHERE RECTYPE = 'D'", true), dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES", false);
		string mprog;
		bool iscorporate;
		public frmAnnualBillPaySummary(string reportID)
		{
			InitializeComponent();
			mprog = reportID;
			if (mprog == "DAILYACTIVITIES")
			{
				this.Text = "SUMMARY OF DAILY ACTIVITIES (BILLS) REPORT";
				panel_DA.Visible = lblDailyActiviies.Visible = true;
			}
		}

		private void frmAnnualBillPaySummary_Load(object sender, EventArgs e)
		{
			
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
			else if (btn.Name == "btnpatientno")
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
		void createRptTable()
		{
			sdt = new DataTable(); //table to will be passed to report dataset 
			sdt.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
			sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
			sdt.Columns.Add(new DataColumn("AMT1", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT2", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT3", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT4", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT5", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT6", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT7", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT8", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT9", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT10", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT11", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("AMT12", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY1", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY2", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY3", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY4", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY5", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY6", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY7", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY8", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY9", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY10", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY11", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAY12", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL1", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL2", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL3", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL4", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL5", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL6", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL7", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL8", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL9", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL10", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL11", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BAL12", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("TOTAL", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAYTOTAL", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BALTOTAL", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BALBF", typeof(decimal)));
		}
		DataRow createnewRow(DataRow drow, bool itspay, string xname, int xperiod )
		{
			decimal db, cr,adj;
			db = cr = adj = 0;
			bool foundit = false;
		  //  int balprd = xperiod + 1;
			string xref = string.IsNullOrWhiteSpace(drow["ghgroupcode"].ToString()) ? drow["grouphead"].ToString() : drow["GHGROUPCODE"].ToString() + drow["GROUPHEAD"].ToString();
			DataRow dr = null;
			foreach (DataRow row in sdt.Rows )
			{
				if ( row["reference"].ToString().Trim() == xref.Trim() )
				{
					foundit = true;
					dr = row;
					break;
				}
			}
			if (!foundit)
			{
				string xghtype = string.IsNullOrWhiteSpace(drow["GHGROUPCODE"].ToString()) ? "C" : "P";
			 //   db = cr = adj = 0;
				dr = sdt.NewRow();
				dr["REFERENCE"] = xref;
				dr["NAME"] = xname;
				dr["BALBF"] = msmrfunc.getOpeningBalance(drow["GHGROUPCODE"].ToString(), drow["GROUPHEAD"].ToString(), "", xghtype, dtDatefrom.Value.Date, dtDatefrom.Value.Date, ref db, ref cr, ref adj);
				for (int i = 1; i < 13; i++)
				{
					dr["amt" + i.ToString()] = 0m;
					dr["pay" + i.ToString()] = 0m;
					dr["bal" + i.ToString()] = 0m;
				}
				dr["TOTAL"] = 0m; dr["paytotal"] = 0m; dr["baltotal"] = 0m;
				sdt.Rows.Add(dr);
			}
			for (int i = 1; i < 13; i++)
			{
				if (xperiod == i) //monthly of transactions, update figures
				{
					if (itspay)
					{
						dr["pay" + i.ToString()] = (decimal)dr["pay" + i.ToString()] + (decimal)drow["amount"];
						dr["paytotal"] = (decimal)dr["paytotal"] + (decimal)drow["amount"];
					}
					else
					{
						dr["amt" + i.ToString()] = (decimal)dr["amt" + i.ToString()] + (decimal)drow["amount"];
						dr["total"] = (decimal)dr["total"] + (decimal)drow["amount"];
					}
					//db = (decimal)dr["balbf"]; cr = 0;
					//for (int ia = 1; ia < balprd; ia++) //to get cummulative balance up to period
					//{
					//    db += (decimal)dr["amt" + ia.ToString()];
					//    cr += (decimal)dr["pay" + ia.ToString()];
					//    dr["bal" + ia.ToString()] = db - cr;
					//}
				  // // dr["bal" + xperiod.ToString()] = (db + (decimal)dr["balbf"]) - cr;
				  // // dr["bal" + xperiod.ToString()] = db - cr;
				}
				
			}
			return dr;
		}
		void createRptTableDA()
		{
			sdt = new DataTable(); //table to will be passed to report dataset 
			sdt.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
			sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
			sdt.Columns.Add(new DataColumn("DRGOPD", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("DRGINP", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("LAB", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("XRSCAN", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("ECG", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("OTHERS", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("TOTAL", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("PAYMENTS", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("BALTOTAL", typeof(decimal)));

		}
		DataRow createnewRowDA(DataRow drow, decimal amt, string xname, string service)
		{
			bool foundit = false; string xref = "";
			if (chkSummary.Checked)
				xref = string.IsNullOrWhiteSpace(drow["ghgroupcode"].ToString()) ? drow["grouphead"].ToString() : drow["GHGROUPCODE"].ToString().Trim() + drow["GROUPHEAD"].ToString().Trim();
			else
				xref = drow["NAME"].ToString();
			DataRow dr = null;
			foreach (DataRow row in sdt.Rows)
			{
				if (chkSummary.Checked ? row["reference"].ToString().Trim() == xref.Trim() : row["name"].ToString().Trim() == xref.Trim() )
				{
					foundit = true;
					dr = row;
					break;
				}
			}
			if (!foundit)
			{
				string xghtype = string.IsNullOrWhiteSpace(drow["GHGROUPCODE"].ToString()) ? "C" : "P";
				//   db = cr = adj = 0;
				dr = sdt.NewRow();
				dr["REFERENCE"] = xref;
				dr["NAME"] = chkSummary.Checked ? xname : drow["name"].ToString();
			  //  dr["GHNAME"] = xname;
				dr["drgopd"] = 0m;
				dr["DRGINP"] = 0m;
				dr["LAB"] = 0m;
				dr["XRSCAN"] = 0m;
				dr["ECG"] = 0m;
				dr["OTHERS"] = 0m;
				dr["PAYMENTS"] = 0m;
				dr["TOTAL"] = 0m; dr["baltotal"] = 0m;
				sdt.Rows.Add(dr);
			}
			if (service == "DRGOPD")
				dr["drgopd"] = amt;
			else if (service == "DRGINP")
				dr["DRGINP"] = amt;
			else if (service == "LAB")
				dr["LAB"] = amt;
			else if (service == "XRSCAN")
				dr["XRSCAN"] = amt;
			else if (service == "ECG")
				dr["ECG"] = amt;
			else if (service == "OTHERS")
				dr["OTHERS"] = amt;
			else if (service == "PAYMENTS")
				dr["PAYMENTS"] = amt;

			if (service == "PAYMENTS")
				dr["PAYMENTS"] = (decimal)dr["PAYMENTS"] + amt;
			else
				dr["total"] = (decimal)dr["total"] + amt;

			dr["baltotal"] = (decimal)dr["total"] - (decimal)dr["payments"];
			return dr;
		}

		void getData()
		{
			bool lhistory = false;
			if (dtDatefrom.Value.Year < msmrfunc.mrGlobals.mpyear)
				lhistory = true;
			DataTable tmpsdt;
			string rptfile = lhistory ? "billhist" : "billing";

			string selstring = ""; // WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'"; // and accounttype NOT LIKE '[NHR]' and ttype <> 'C'";
			if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
				selstring += " AND GROUPHEAD = '" + txtgrouphead.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
				selstring += " AND GHGROUPCODE = '" + txtgroupcode.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
				selstring += " AND GROUPHEAD = '" + txtpatientno.Text + "'";
			//EXTRACT ALL CLIENTS ON MONTHLY_BILL_CIRCLE to a datatable otherwise i will be required to go forth and come to sql server database for each patient record 03.09.2017
			iscorporate = false;
			if (chkCorporate.Checked || chkHMO.Checked || chkNHIS.Checked )
			{
				iscorporate = true;
				selstring += " AND transtype = 'C'";
				if (chkFamily.Checked)
					selstring += ""; // OR transtype = 'P'"; - gets all types
			   // dtt = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO, name FROM CUSTOMER", false);
			}
			else if (chkFamily.Checked || chkPrivate.Checked )
			{
				selstring += chkPrivate.Checked ? " AND transtype = 'P' and ghgroupcode = 'PVT'" : " AND transtype = 'P' and ghgroupcode = 'FC'";
			 //   dtt = Dataaccess.GetAnytable("", "MR", "SELECT groupcode+patientno AS custno, name FROM patient WHERE ISGROUPHEAD = '1'", false);
			}
			if (mprog == "DAILYACTIVITIES")
			{
				ProcessDailyActivities();
				return;
			}
			DataTable dtt = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO, name, hmo FROM CUSTOMER", false);

			int startmth = dtDatefrom.Value.Month, endmth = dtDateto.Value.Month+1;
			//billing segment
			string strhd = "select ghgroupcode,grouphead, sum(amount) as Amount from billing ",strhd1 = "",xstr = "",custname = "";
			for (int i = startmth; i < endmth; i++)
			{
				strhd1 = " WHERE month(trans_date) = '" + i + "' and year(trans_date) = '" + dtDateto.Value.Year+ "'";
				xstr = strhd + strhd1 + selstring+" GROUP BY GHGROUPCODE, GROUPHEAD";
				tmpsdt = Dataaccess.GetAnytable("", "MR", xstr, false);
			
				foreach (DataRow row in tmpsdt.Rows )
				{
					if (iscorporate && !GrpSelectCheck(row, dtt))
						continue;

					if ( chkCorporate.Checked)
						custname = bissclass.combodisplayitemCodeName("custno",row["grouphead"].ToString(), dtt,"name"); //msmrfunc.GETGroupheadname("", row["grouphead"].ToString(), "C"); //
					else
						custname = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P"); // bissclass.combodisplayitemCodeName("custno", row["ghgroupcode"].ToString() + row["grouphead"].ToString(), dtt, "name");
					createnewRow(row, false, custname, i);
				}
			}
			//payment segment
			strhd = "select ghgroupcode,grouphead, sum(amount) as Amount from paydetail ";
			for (int i = startmth; i < endmth; i++)
			{
				strhd1 = " WHERE month(trans_date) = '" + i + "' and year(trans_date) = '" + dtDateto.Value.Year + "'";
				xstr = strhd + strhd1 + selstring + " GROUP BY GHGROUPCODE, GROUPHEAD";
				tmpsdt = Dataaccess.GetAnytable("", "MR", xstr, false);

				foreach (DataRow row in tmpsdt.Rows)
				{
					if (iscorporate && !GrpSelectCheck(row, dtt))
						continue;

					if (chkCorporate.Checked)
						custname = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dtt, "name");  //msmrfunc.GETGroupheadname("", row["grouphead"].ToString(), "C"); //
					else
						custname = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P"); // bissclass.combodisplayitemCodeName("custno", row["ghgroupcode"].ToString() + row["grouphead"].ToString(), dtt, "name");
					createnewRow(row, true, custname, i);
				}
			}
			//adjust segment - Debit
			strhd = "select ghgroupcode,grouphead, sum(amount) as Amount from bill_adj";
			for (int i = startmth; i < endmth; i++)
			{
				strhd1 = " WHERE month(trans_date) = '" + i + "' and year(trans_date) = '" + dtDateto.Value.Year + "'";
				xstr = strhd + strhd1 + selstring + " and ttype = 'D' GROUP BY GHGROUPCODE, GROUPHEAD";
				tmpsdt = Dataaccess.GetAnytable("", "MR", xstr, false);

				foreach (DataRow row in tmpsdt.Rows)
				{
					if (iscorporate && !GrpSelectCheck(row, dtt))
						continue;

					if (chkCorporate.Checked)
						custname = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dtt, "name"); //msmrfunc.GETGroupheadname("", row["grouphead"].ToString(), "C"); //
					else
						custname = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P"); // bissclass.combodisplayitemCodeName("custno", row["ghgroupcode"].ToString() + row["grouphead"].ToString(), dtt, "name");

					createnewRow(row, false, custname, i);
				}
			}
			//adjust segment - Crdit
			strhd = "select ghgroupcode,grouphead, sum(amount) as Amount from bill_adj";
			for (int i = startmth; i < endmth; i++)
			{
				strhd1 = " WHERE month(trans_date) = '" + i + "' and year(trans_date) = '" + dtDateto.Value.Year + "'";
				xstr = strhd + strhd1 + selstring + " and ttype = 'C' GROUP BY GHGROUPCODE, GROUPHEAD";
				tmpsdt = Dataaccess.GetAnytable("", "MR", xstr, false);

				foreach (DataRow row in tmpsdt.Rows)
				{
					if (iscorporate && !GrpSelectCheck(row, dtt))
						continue;

					if (chkCorporate.Checked)
						custname = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dtt, "name"); //msmrfunc.GETGroupheadname("", row["grouphead"].ToString(), "C"); //
					else
						custname = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P"); // bissclass.combodisplayitemCodeName("custno", row["ghgroupcode"].ToString() + row["grouphead"].ToString(), dtt, "name");

					createnewRow(row, true, custname, i);
				}
			}
			//GET BALANCES for each period for each client
			int balprd = dtDateto.Value.Month + 1, statpd = dtDatefrom.Value.Month;
			decimal db, cr; 
			foreach (DataRow dr in sdt.Rows )
			{
				db = (decimal)dr["balbf"]; cr = 0;
				for (int x = statpd; x < balprd; x++) //to get cummulative balance up to period
				{
					db += (decimal)dr["amt" + x.ToString()];
					cr += (decimal)dr["pay" + x.ToString()];
					dr["bal" + x.ToString()] = db - cr;
				}
			}

		}
		bool GrpSelectCheck(DataRow xr, DataTable xdt)
		{
			bool foundit = false;
			if (iscorporate && string.IsNullOrWhiteSpace(xr["ghgroupcode"].ToString()))
			{
				foreach (DataRow row in xdt.Rows )
				{
					if (row["custno"].ToString() == xr["grouphead"].ToString())
					{
						foundit = chkCorporate.Checked && !(bool)row["hmo"] ? true : chkHMO.Checked && (bool)row["hmo"] && !row["name"].ToString().Contains("NHIS") ? true : chkNHIS.Checked && (bool)row["hmo"] && row["name"].ToString().Contains("NHIS") ? true : false;
						break;
					}
				}
			}
			return foundit;
		}
		void ProcessDailyActivities()
		{
			//get value from dispensa and inpdispensa
		  //  string grphtype = chkCorporate.Checked ? " and grouphtype = 'C'" : chkPVTFamily.Checked ? " and grouphtype = 'P'" : "";
			createRptTableDA();
			string selstring = ""; 
			if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
				selstring += " AND GROUPHEAD = '" + txtgrouphead.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
				selstring += " AND GHGROUPCODE = '" + txtgroupcode.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
				selstring += " AND GROUPHEAD = '" + txtpatientno.Text + "'";
			//EXTRACT ALL CLIENTS ON MONTHLY_BILL_CIRCLE to a datatable otherwise i will be required to go forth and come to sql server database for each patient record 03.09.2017
			string selstr = "";
			DataTable dtt = new DataTable();
			iscorporate = false;
			if (chkCorporate.Checked || chkHMO.Checked || chkNHIS.Checked)
			{
				iscorporate = true;
				selstr = " AND grouphtype = 'C'";
				if (chkFamily.Checked)
					chkFamily.Checked = false; // selstr = ""; // OR transtype = 'P'"; - gets all types
				dtt = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO, name, hmo FROM CUSTOMER", false);
			}
			else if (chkFamily.Checked || chkPrivate.Checked)
			{
				selstr = chkPrivate.Checked ? " AND grouphtype = 'P' and groupcode = 'PVT'" : " AND grouphtype = 'P' and groupcode = 'FC'";
				dtt = Dataaccess.GetAnytable("", "MR", "SELECT groupcode+patientno AS custno, name FROM patient WHERE ISGROUPHEAD = '1'"+selstr, false);
			}
			selstring += selstr;
			selstr = "";
			if (chkSummary.Checked)
				selstr = "select grouphtype, ghgroupcode,grouphead, sum(cost) as Amount from dispensa WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'" + selstring + " GROUP BY grouphtype, GHGROUPCODE, GROUPHEAD";
			else
				selstr = "select grouphtype, name, sum(cost) as Amount from dispensa WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'" + selstring + " GROUP BY grouphtype, name";
			GetDADetails(selstr, "DRGOPD", dtt );
			if (chkSummary.Checked)
				selstr = "select grouphtype, ghgroupcode,grouphead, sum(cost) as Amount from inpdispensa WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'" + selstring + " GROUP BY grouphtype,  GHGROUPCODE, GROUPHEAD";
			else
				selstr = "select grouphtype, name, sum(cost) as Amount from inpdispensa WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'" + selstring + " GROUP BY grouphtype, name";
			GetDADetails(selstr, "DRGINP", dtt);
			if (chkSummary.Checked)
				selstr = "select facility, grouphtype, ghgroupcode,grouphead, SUM(Amount) AS AMOUNT from LABDET WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'" + selstring + " GROUP BY FACILITY, grouphtype, GHGROUPCODE, GROUPHEAD";
			else
				selstr = "select facility, grouphtype, name, SUM(Amount) AS AMOUNT from LABDET WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'" + selstring + " GROUP BY FACILITY, grouphtype, name";

			GetDADetails(selstr, "LAB", dtt);
			//string replacestr = camt.Replace("XYZ", "abc");
			selstring = selstring.Replace("grouphtype", "transtype");
			if (chkSummary.Checked)
				selstr = "select transtype, ghgroupcode, grouphead, sum(amount) as Amount from paydetail WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'" + selstring + " GROUP BY transtype, GHGROUPCODE, GROUPHEAD";
			else
				selstr = "select transtype, name, sum(amount) as Amount from paydetail WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'" + selstring + " GROUP BY transtype, name";
			GetDADetails(selstr, "PAYMENTS", dtt);
		}
		void GetDADetails(string strhd, string servicetype, DataTable grphd)
		{
			DataTable dt = Dataaccess.GetAnytable("", "MR", strhd, false);
			string custname = "",ghsave = "";
		   // dt.Columns.Add(new DataColumn("facdesc", typeof(string)));
			string xtype = "";
			foreach (DataRow row in dt.Rows)
			{
				xtype = servicetype == "PAYMENTS" ? row["transtype"].ToString() : row["grouphtype"].ToString();

				if (!iscorporate && xtype == "C" || !chkFamily.Checked && xtype == "P" || iscorporate && !filterCorporate(grphd, row["grouphead"].ToString()))
						continue;
 
				if (servicetype == "LAB")
				{
					servicetype = row["facility"].ToString().Contains("LAB") ? "LAB" : new string[] { "XRAY", "XR" }.Contains(row["facility"].ToString()) ? "XRSCAN" : row["facility"].ToString().Contains("ECG") ? "ECG" : "OTHERS";
				}
				if (chkSummary.Checked )
				{
					if (ghsave != row["grouphead"].ToString().Trim())
					{
						if (!iscorporate)
							custname = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P");
						  //  custname = bissclass.combodisplayitemCodeName("custno", row["ghgroupcode"].ToString() + row["grouphead"].ToString(), grphd, "name");
						else
							//custname = msmrfunc.GETGroupheadname("", row["grouphead"].ToString(), "C");
							custname = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), grphd, "name");
							
						ghsave = row["grouphead"].ToString().Trim();
					}
				}
				createnewRowDA(row, (decimal)row["amount"], custname, servicetype);
			}

		}
		bool filterCorporate(DataTable xdt, string gh)
		{
			bool rtnvalue = false;
			foreach (DataRow row in xdt.Rows )
			{
				if (row["custno"].ToString().Trim() == gh.Trim())
				{
					if (chkCorporate.Checked && (chkHMO.Checked || chkNHIS.Checked)) // Convert.ToBoolean(row["hmo"]))
					{
						rtnvalue = true;
						break;
					}
					if (Convert.ToBoolean(row["hmo"]) && chkHMO.Checked && !gh.Contains("NHIS")) // || chkNHIS.Checked))
					{
						rtnvalue = true;
						break;
					}
					if (Convert.ToBoolean(row["hmo"]) && chkNHIS.Checked && gh.Contains("NHIS"))
					{
						rtnvalue = true;
						break;
					}
					if (chkCorporate.Checked && !Convert.ToBoolean(row["hmo"]))
					{
						rtnvalue = true;
						break;
					}
			  /*      if (chkNHIS.Checked && !gh.Contains("NHIS") && (!chkHMO.Checked || !chkCorporate.Checked))
					{
						break;
					}

					
					else if (chkCorporate.Checked && !Convert.ToBoolean(row["hmo"]))
					{
						rtnvalue = true;
						break;
					}
					else */
						break;
				}
			}
			return rtnvalue;
		}
		void printprocess(bool isprint)
		{
			DialogResult result;
			if (dtDatefrom.Value.Date > DateTime.Now.Date || dtDatefrom.Value.Date < msmrfunc.mrGlobals.mta_start || dtDateto.Value.Date < dtDatefrom.Value.Date || dtDatefrom.Value.Year != dtDateto.Value.Year)
			{
				result = MessageBox.Show("Invalid Date Specification");
				return;
			}
	  //      sdt = new DataTable();
			ds = new DataSet();
			//if (sdt != null)
			//{
			//    sdt.DataSet.Reset();
			//    ds.Tables.Clear();
			//    ds.Clear();
			//}
			createRptTable();


			getData();
			if (sdt == null || sdt.Rows.Count < 1)
			{
				result = MessageBox.Show("No Data for Specified Conditions...");
				return;
			}
			ds.Tables.Add(sdt);

			Session["sql"] = "";
			if (mprog == "DAILYACTIVITIES")
			{
				Session["rdlcfile"] = "SummaryOfDailyActivities.rdlc";

				mrptheader = "SUMMARY OF DAILY ACTIVITIES (BILLS/PMTS) FOR  " + dtDatefrom.Value.ToShortDateString() + "  TO :  " + dtDateto.Value.ToShortDateString();
			}
			else
			{
				Session["rdlcfile"] = "AnnualBillSummary.rdlc";

				mrptheader = "MONTHLY BILLS/PAY/BAL SUMMARY FOR  " + dtDatefrom.Value.ToShortDateString() + "  TO :  " + dtDateto.Value.ToShortDateString();
			}
			if (!isprint)
			{
				frmReportViewer paedreports = new frmReportViewer(mprog == "DAILYACTIVITIES" ? "SUMMARY OF DAILY ACTIVITIES (BILLS/PMTS)" : "MONTHLY BILLS/PAY/BAL SUMMARY REPORTS", mrptheader, "", "", "", "ANNUALSUMMARY", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "W", "");

				if (isprint)
					paedreports.work();
				else
					paedreports.Show();
			}
			else
			{
				MRrptConversion.GeneralRpt(mprog == "DAILYACTIVITIES" ? "SUMMARY OF DAILY ACTIVITIES (BILLS/PMTS)" : "MONTHLY BILLS/PAY/BAL SUMMARY REPORTS", mrptheader, "", "", "", "ANNUALSUMMARY", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "W", "");
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
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
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