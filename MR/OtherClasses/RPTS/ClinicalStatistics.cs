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
	public partial class ClinicalStatistics : Form
	{
		DataSet ds = new DataSet();
		DataTable sdt, tsdt, dtdocs, dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES  order by name", true), dtchain = Dataaccess.GetAnytable("", "MR", "SELECT GROUPCODE, PATIENTNO, SEX, GROUPHEAD FROM BILLCHAIN", false), dtcust = Dataaccess.GetAnytable("", "MR", "SELECT custno, name, hmo FROM customer order by name", true ), dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", false);
		decimal xtotal, newmale, newfemale, newcomp, newpvt, male, female, comp, pvt, nhmo, newhmo, nhis, newnhis;
		string mname, AnyCode,lookupsource;
		public ClinicalStatistics()
		{
			InitializeComponent();
		}

		private void ClinicalStatistics_Load(object sender, EventArgs e)
		{
			initcomboboxes();
		}
		private void initcomboboxes()
		{
			//diagnosis
			cboDiag.DataSource = dtdiag;
			cboDiag.ValueMember = "Type_code";
			cboDiag.DisplayMember = "name";

			cboCorpClient.DataSource = dtcust;
			cboCorpClient.ValueMember = "custno";
			cboCorpClient.DisplayMember = "name";
		}
		private void txtpatientno_LostFocus(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtpatientno.Text))
				return;
			DialogResult result;
			if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text))  //no lookup value obtained
				this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");

			//check if patientno exists
			DataTable dtpat = Dataaccess.GetAnytable("", "MR", "SELECT name from patient where groupcode = '" + txtgroupcode.Text + "' and patientno = '" + txtpatientno.Text + "'", false);
			if (dtpat.Rows.Count < 1)
			{
				result = MessageBox.Show("Invalid Patient Number... ");
				txtgroupcode.Text = txtpatientno.Text = lblprompt.Text = "";
				return;
			}
			lblprompt.Text = dtpat.Rows[0]["name"].ToString();
			if (!(bool)dtpat.Rows[0]["isgrouphead"])
			{
				result = MessageBox.Show("Selected Patient is not a Grouphead or Head of Family... ");
				txtgroupcode.Text = txtpatientno.Text = lblprompt.Text = "";
				return;
			}
		}
		private void btngroupcode_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			//           Button btn = (Button)sender;

			if (btn.Name == "btngroupcode")
			{
				this.txtgroupcode.Text = "";
				lookupsource = "g";
				msmrfunc.mrGlobals.crequired = "g";
				msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
			}
			else if (btn.Name == "btnpatientlookup")
			{
				this.txtpatientno.Text = "";
				lookupsource = "L";
				msmrfunc.mrGlobals.crequired = "L";
				msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
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
				this.txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
				this.txtpatientno.Select();
			}

			else if (lookupsource == "P") //patientno
			{
				this.txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode;
				this.txtpatientno.Select();
			}

		}
		private void chkPrescriptionStatistics_Click(object sender, EventArgs e)
		{
			if (chkPrescriptionStatistics.Checked)
			{
				lblDiagDesc.Text = "Stock Query Description";
				this.toolTip1.SetToolTip(this.txtQueryDecs, "You can specify more than one Product/Drug  Separated by Comman \",\"");
			}
			else
			{
				lblDiagDesc.Text = "Disease/Diagnosis Query Desc";
				this.toolTip1.SetToolTip(this.txtQueryDecs, "You can specify more than one Disease/Diag.  Separated by Comman \",\"");
			}

		}
		void createSummary()
		{
			sdt = new DataTable(); //table will be passed to report dataset 
			sdt.Columns.Add(new DataColumn("reference", typeof(string)));
			sdt.Columns.Add(new DataColumn("GRPDESC", typeof(string)));
			sdt.Columns.Add(new DataColumn("GRPTYPE", typeof(string)));
			sdt.Columns.Add(new DataColumn("DESCRIPTION", typeof(string)));
			sdt.Columns.Add(new DataColumn("amt1", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt2", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt3", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt4", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt5", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt6", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt7", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt8", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt9", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt10", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt11", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt12", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt13", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt14", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt15", typeof(decimal)));
			sdt.Columns.Add(new DataColumn("amt16", typeof(decimal)));
		}
		DataRow createnewRow(string xfile) //DataRow drow)
		{
		   // bool foundit = false, ishmo = false;
			DataRow dr = null;
			if (xfile == "SDT")
				dr = sdt.NewRow();
			else
				dr = tsdt.NewRow();
			dr["reference"] = "";
			dr["GRPDESC"] = "";
			dr["GRPTYPE"] = "";
			dr["DESCRIPTION"] = "";
			dr["amt1"] = 0;
			dr["amt2"] = 0;
			dr["amt3"] = 0;
			dr["amt4"] = 0;
			dr["amt5"] = 0;
			dr["amt6"] = 0;
			dr["amt7"] = 0;
			dr["amt8"] = 0;
			dr["amt9"] = 0;
			dr["amt10"] = 0;
			dr["amt11"] = 0;
			dr["amt12"] = 0;
			dr["amt13"] = 0;
			dr["amt14"] = 0;
			dr["amt15"] = 0;
			dr["amt16"] = 0;
			if (xfile == "SDT")
				sdt.Rows.Add(dr);
			else
				tsdt.Rows.Add(dr);
			return dr;
		}
 /*           foreach (DataRow row in sdt.Rows)
			{
				if (mprog == 1)
					xreference = string.IsNullOrWhiteSpace(drow["FACILITY"].ToString().Trim()) ? "UNSPECIFIED" : drow["FACILITY"].ToString().Trim();
				else
					xreference = string.IsNullOrWhiteSpace(drow["PROCESS"].ToString().Trim()) ? "UNSPECIFIED" : drow["PROCESS"].ToString().Trim();
				if (row["reference"].ToString() == xreference)
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
				dr["name"] = xreference == "UNSPECIFIED" ? "< UNSPECIFIED >" : bissclass.combodisplayitemCodeName(mprog == 1 ? "type_code" : "reference", drow["facility"].ToString(), mprog == 1 ? dtfacility : dttariff, "name");
				dr["FREQUENCY"] = 0;
				dr["PVTFAMILY"] = 0;
				dr["CORPORATE"] = 0;
				dr["HMO"] = 0;
				dr["NHIS"] = 0;
				dr["TOTALAMT"] = 0;
			}
			dr["FREQUENCY"] = (decimal)dr["frequency"] + 1;
			if (drow["transtype"].ToString() == "P")
				dr["PVTFAMILY"] = (decimal)dr["pvtfamily"] + (decimal)dr["amount"];
			else if (drow["groupcode"].ToString().Trim() == "NHIS")
				dr["NHIS"] = (decimal)dr["NHIS"] + (decimal)drow["amount"];
			else
			{
				for (int i = 0; i < dtcust.Rows.Count; i++)
				{
					if (dtcust.Rows[i]["custno"].ToString().Trim() == drow["grouphead"])
					{
						ishmo = (bool)dtcust.Rows[i]["ishmo"];
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
		}*/
		void outpatient()
		{
			lblprompt.Text = "Scanning for Out-Patient Visits...";
			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = nhis = newnhis = 0;
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT mrattend.trans_date, mrattend.groupcode, mrattend.patientno, mrattend.grouphtype, mrattend.grouphead, billchain.sex from mrattend left join billchain ON mrattend.groupcode = billchain.groupcode and mrattend.patientno = billchain.patientno where mrattend.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and mrattend.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
			getPatientProfile(tsdt, 1);
	  /*      foreach (DataRow row in tsdt.Rows)
			{
				getPatientProfile(row,1);
			}*/
			newRegistration();
		}
		void newRegistration()
		{   
			lblprompt.Text = "Scanning for New Registration...";
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT billchain.reg_date, billchain.groupcode, billchain.patientno, billchain.grouphtype, billchain.grouphead, billchain.sex from billchain where reg_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and reg_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
			getPatientProfile(tsdt, 2);
		/*    foreach (DataRow row in tsdt.Rows)
			{
				getPatientProfile(row,2);
			}*/
			update_td("OUTPATIENT", "1", "NEW REGISTRATIONS", 1, newmale, newfemale, newmale + newfemale, newcomp, newhmo, newnhis, newpvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);
			update_td("OUTPATIENT", "1", "ATTENDANCE", 2, male, female, male + female, comp, nhmo, nhis, pvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);

			DataRow drow = createnewRow("SDT");
			drow["GRPDESC"] = "OUTPATIENT";
			drow["GRPTYPE"] = "1";
			drow["DESCRIPTION"] = "AGGREGATE IN %";

			drow["amt1"] = (male / (male + female)) * 100;
			drow["amt2"] = (female / (male + female)) * 100;
			drow["amt4"] = (comp / (male + female)) * 100;
			drow["amt5"] = (nhmo / (male + female)) * 100;
			drow["amt6"] = (nhis / (male + female)) * 100;
			drow["amt7"] = (pvt / (male + female)) * 100;
		}
		void update_td(string xgrpdesc,string xgrptype,string xdesc,int grp, decimal xamt1, decimal xamt2, decimal xamt3, decimal xamt4, decimal xamt5, decimal xamt6, decimal xamt7, decimal xamt8, decimal xamt9, decimal xamt10, decimal xamt11, decimal xamt12, decimal xamt13, decimal xamt14, decimal xamt15, decimal xamt16)
		{
			string xclinic = "";
			bool foundit = false;
			DataRow drow = null;
			if ( xgrptype == "4" || xgrptype == "5" || xgrptype == "6" || xgrptype == "7" || xgrptype == "8") //delivery,clinic,labxray,groupattend,docsPat,diseasequery
			{
				foreach (DataRow row in sdt.Rows)
				{
					if (xgrptype == "4" && xdesc.Trim() == row["reference"].ToString().Trim() || xgrptype == "7" && row["grptype"].ToString().Trim() == xgrptype && row["reference"].ToString().Trim() == xdesc.Trim() || ((xgrptype == "5" || xgrptype == "6") && row["grptype"].ToString() == xgrptype && row["description"].ToString().Trim() == xdesc.Trim()) || xgrptype == "8" && row["grptype"].ToString() == xgrptype && row["description"].ToString().Trim() == xdesc.Trim()) // row["description"].ToString().Trim().Contains(xdesc.Trim()))
					{
						foundit = true;
						drow = row;
						break;
					}
				}
				if (!foundit && xgrptype == "4" || xgrptype == "7" ) //we must get clinic name from facility
				{
					bool ffoundit = false;
					if (xgrptype == "4")
					{
						foreach (DataRow frow in dtfacility.Rows)
						{
							if (frow["type_code"].ToString().Trim() == xdesc.Trim())
							{
								xclinic = frow["name"].ToString();
								ffoundit = true;
								break;
							}
						}
					}
					else
					{
						foreach (DataRow frow in dtdocs.Rows)
						{
							if (frow["reference"].ToString().Trim() == xdesc.Trim())
							{
								xclinic = frow["name"].ToString();
								ffoundit = true;
								break;
							}
						}

					}
					if (!ffoundit)
					{
						xclinic = xdesc;
					}
				}
			}
			if (!foundit)
			{
				drow = createnewRow("SDT");
				if (xgrptype == "4" || xgrptype == "7")
				{
					drow["reference"] = xdesc;
					drow["DESCRIPTION"] = xclinic;
				}
				else
				{
					drow["DESCRIPTION"] = xdesc;
				}
				drow["GRPDESC"] = xgrpdesc;
				drow["GRPTYPE"] = xgrptype;
			}
			drow["amt1"] = (decimal)drow["amt1"] + xamt1; // male;
			drow["amt2"] = (decimal)drow["amt2"] + xamt2; //female;
			drow["amt3"] = (decimal)drow["amt3"] + xamt3; //; //male + female;
			drow["amt4"] = (decimal)drow["amt4"] + xamt4; //comp;
			drow["amt5"] = (decimal)drow["amt5"] + xamt5; //nhmo;
			drow["amt6"] = (decimal)drow["amt6"] + xamt6; //pvt;
			drow["amt7"] = (decimal)drow["amt7"] + xamt7; //comp;
			drow["amt8"] = (decimal)drow["amt8"] + xamt8; //nhmo;
			drow["amt9"] = (decimal)drow["amt9"] + xamt9; //pvt;
			drow["amt10"] = (decimal)drow["amt10"] + xamt10; //pvt;
			if (xgrptype == "8")
			{
				drow["amt11"] = (decimal)drow["amt11"] + xamt11; 
				drow["amt12"] = (decimal)drow["amt12"] + xamt12; 
				drow["amt13"] = (decimal)drow["amt13"] + xamt13; 
				drow["amt14"] = (decimal)drow["amt14"] + xamt14; 
				drow["amt15"] = (decimal)drow["amt15"] + xamt15; 
				drow["amt16"] = (decimal)drow["amt16"] + xamt16; 
			}
			else
			{
				drow["amt11"] = (decimal)drow["amt11"] + xamt5; //nhmo;
				drow["amt12"] = (decimal)drow["amt12"] + xamt6; //pvt;
				drow["amt13"] = (decimal)drow["amt13"] + xamt7; //comp;
				drow["amt14"] = (decimal)drow["amt14"] + xamt8; //nhmo;
				drow["amt15"] = (decimal)drow["amt15"] + xamt9; //pvt;
				drow["amt16"] = (decimal)drow["amt16"] + xamt9; //pvt;
			}
		}
	  //  void getPatientProfile(DataRow drow, int grpid)
		void getPatientProfile(DataTable pdt, int grpid)
		{
			foreach (DataRow row in pdt.Rows) // dtchain.Rows)
			{
				if (row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0,1) == "M")
				{
					if (grpid == 1)
						male++;
					else
						newmale++;
				}
				else
				{
					if (grpid == 1)
						female++;
					else
						newfemale++;
				}

				if (row["grouphtype"].ToString() == "C") //corporate
				{
					foreach (DataRow crow in dtcust.Rows)
					{
						if (crow["name"].ToString() == "" || crow["hmo"] == DBNull.Value)
							continue;
						if (row["grouphead"].ToString().Trim() == crow["custno"].ToString().Trim())
						{
							mname = crow["name"].ToString(); //needed in grp 7 - group attendance
							if ( Convert.ToBoolean(crow["hmo"]))
							{
								if (grpid == 1)
								{
									if (row["groupcode"].ToString().Trim() == "NHIS")
										nhis++;
									else
										nhmo++;
								}
								else
								{
									if (row["groupcode"].ToString().Trim() == "NHIS")
										newnhis++;
									else
										newhmo++;
								}
							}
							else
							{
								if (grpid == 1)
									comp++;
								else
									newcomp++;
							}
							break;
						}
					}
				}
				else
				{
					if (grpid == 1)
						pvt++;
					else
						newpvt++;
				}
			   // break;
			}
		}
		void getPatientProfileBYROW(DataRow row, int grpid)
		{
		  //  foreach (DataRow row in pdt.Rows) // dtchain.Rows)
		   // {
				if (row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "M")
				{
					male++;
				}
				else
				{
					female++;
				}

				if (row["grouphtype"].ToString() == "C") //corporate
				{
					foreach (DataRow crow in dtcust.Rows)
					{
						if (crow["name"].ToString() == "" || crow["hmo"] == DBNull.Value)
							continue;
						if (row["grouphead"].ToString().Trim() == crow["custno"].ToString().Trim())
						{
							mname = crow["name"].ToString(); //needed in grp 7 - group attendance
							if (Convert.ToBoolean(crow["hmo"]))
							{
								if (row["groupcode"].ToString().Trim() == "NHIS")
									nhis++;
								else
									nhmo++;
							}
							else
							{
								comp++;
							}
							break;
						}
					}
				}
				else
				{
					pvt++;
				}
				// break;
		  //  }
		}
		void inpatient()
		{
			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = nhis = newnhis = 0;
			lblprompt.Text = "Scanning In-Patient Records...";
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT admrecs.adm_date, admrecs.groupcode, admrecs.patientno, admrecs.grouphtype, admrecs.grouphead, billchain.sex from admrecs left join billchain on admrecs.groupcode = billchain.groupcode and  admrecs.patientno = billchain.patientno where admrecs.adm_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and admrecs.adm_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
			lblprompt.Text = "Scanning In-Patient Records..."+tsdt.Rows.Count.ToString();
			getPatientProfile(tsdt, 1);
			/*foreach (DataRow row in tsdt.Rows)
			{
				getPatientProfile(row, 1);
			}*/
			//get discharge records for the period
		  //  xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = 0;
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT admrecs.discharge, admrecs.groupcode, admrecs.patientno, admrecs.grouphtype, admrecs.grouphead, billchain.sex from admrecs left join billchain on admrecs.groupcode = billchain.groupcode and admrecs.patientno = billchain.patientno where admrecs.discharge != '' AND CONVERT(DATE, admrecs.discharge) >= '" + dtDateFrom.Value.ToShortDateString() + "' and CONVERT(DATE, admrecs.discharge) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
			lblprompt.Text = "Scanning In-Patient (Discharge) Records..." + tsdt.Rows.Count.ToString();
			getPatientProfile(tsdt, 2);
			/*foreach (DataRow row in tsdt.Rows)
			{
				getPatientProfile(row, 2);
			}*/
			update_td("IN-PATIENTS", "2", "ADMISSION", 1, newmale, newfemale, newmale + newfemale, newcomp, newhmo, newnhis, newpvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);
			update_td("IN-PATIENTS", "2", "DISCHARGE", 2, male, female, male + female, comp, nhmo, nhis, pvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);

			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = nhis = newnhis = 0;
			//get death records for the period
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT deaths.deathdate, deaths.groupcode, deaths.patientno, billchain.grouphtype, billchain.sex, billchain.grouphead from deaths left join billchain on deaths.groupcode = billchain.groupcode and deaths.patientno = billchain.patientno where deaths.deathdate >= '" + dtDateFrom.Value.ToShortDateString() + "' and deaths.deathdate <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
			lblprompt.Text = "Scanning Death Records..." + tsdt.Rows.Count.ToString();
			getPatientProfile(tsdt, 2);
/*            foreach (DataRow row in tsdt.Rows)
			{
				getPatientProfile(row, 1);
			}*/
			update_td("IN-PATIENTS", "2", "DEATHS", 1, male, female, male + female, comp, nhmo, nhis, pvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);
		}
		void Delivery()
		{
			decimal n_male, n_female, n_co, n_pvt, c_male, c_female, c_co, c_pvt, s_male, s_female, s_co, s_pvt, v_male, v_female, v_co, v_pvt, f_male, f_female, f_co, f_pvt, xtotal, n_hmo, v_hmo, c_hmo, s_hmo, f_hmo, n_nhis, v_nhis, c_nhis, s_nhis, f_nhis;
			n_male = n_female = n_co = n_pvt = c_male = c_female = c_co = c_pvt = s_male = s_female = s_co = s_pvt = v_male = v_female = v_co = v_pvt = f_male = f_female = f_co = f_pvt = xtotal = n_hmo = v_hmo = c_hmo = s_hmo = f_hmo = n_nhis = v_nhis = c_nhis = s_nhis = f_nhis = 0;
			lblprompt.Text = "Scanning Birth Records...";
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT births.birthdate, births.groupcode, births.patientno, births.grouphtype, births.sex, births.grouphtype, births.grouphead, births.TYPEOFDELI from births where birthdate >= '" + dtDateFrom.Value.ToShortDateString() + "' and birthdate <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
			lblprompt.Text = "Scanning In-Patient Records..." + tsdt.Rows.Count.ToString();
			foreach (DataRow row in tsdt.Rows)
			{
				//Normal birth
				n_male += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "M" && row["typeofdeli"].ToString() == "N" ? 1 : 0;
				n_female += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "F" && row["typeofdeli"].ToString() == "N" ? 1 : 0;
				n_pvt += row["grouphtype"].ToString() == "P" && row["typeofdeli"].ToString() == "N" ? 1 : 0;
                //Normal Caesarian Still Vacuum Forsep
				if (row["grouphtype"].ToString() == "C")
				{
					for (int i = 0; i < dtcust.Rows.Count; i++)
					{
						if (row["grouphead"].ToString().Trim() == dtcust.Rows[i]["custno"].ToString().Trim())
						{
							if ((bool)dtcust.Rows[i]["hmo"])
							{
								if (row["groupcode"].ToString().Trim() == "NHIS")
								{
									c_nhis += row["typeofdeli"].ToString() == "C" ? 1 : 0;
									n_nhis += row["typeofdeli"].ToString() == "N" ? 1 : 0;
									s_nhis += row["typeofdeli"].ToString() == "S" ? 1 : 0;
									v_nhis += row["typeofdeli"].ToString() == "V" ? 1 : 0;
									f_nhis += row["typeofdeli"].ToString() == "F" ? 1 : 0;
								}
								else
								{
									c_hmo += row["typeofdeli"].ToString() == "C" ? 1 : 0;
									n_hmo += row["typeofdeli"].ToString() == "N" ? 1 : 0;
									s_hmo += row["typeofdeli"].ToString() == "S" ? 1 : 0;
									v_hmo += row["typeofdeli"].ToString() == "V" ? 1 : 0;
									f_hmo += row["typeofdeli"].ToString() == "F" ? 1 : 0;
								}
							}
							else
							{
								n_co += row["typeofdeli"].ToString() == "N" ? 1 : 0;
								c_co += row["typeofdeli"].ToString() == "C" ? 1 : 0;
								s_co += row["typeofdeli"].ToString() == "S" ? 1 : 0;
								v_co += row["typeofdeli"].ToString() == "V" ? 1 : 0;
								f_co += row["typeofdeli"].ToString() == "F" ? 1 : 0;
							}
							break;
						}
					}
					//cs
					c_male += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "M" && row["typeofdeli"].ToString() == "C" ? 1 : 0;
					c_female += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "F" && row["typeofdeli"].ToString() == "C" ? 1 : 0;
					c_pvt += row["grouphtype"].ToString() == "P" && row["typeofdeli"].ToString() == "C" ? 1 : 0;
					//still
					s_male += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "M" && row["typeofdeli"].ToString() == "S" ? 1 : 0;
					s_female += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "F" && row["typeofdeli"].ToString() == "S" ? 1 : 0;
					s_pvt += row["grouphtype"].ToString() == "P" && row["typeofdeli"].ToString() == "S" ? 1 : 0;
					//vacuum
					v_male += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "M" && row["typeofdeli"].ToString() == "V" ? 1 : 0;
					v_female += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "F" && row["typeofdeli"].ToString() == "V" ? 1 : 0;
					v_pvt += row["grouphtype"].ToString() == "P" && row["typeofdeli"].ToString() == "V" ? 1 : 0;
					//forsep
					f_male += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "M" && row["typeofdeli"].ToString() == "F" ? 1 : 0;
					f_female += row["sex"].ToString().Length > 0 && row["sex"].ToString().Substring(0, 1) == "F" && row["typeofdeli"].ToString() == "F" ? 1 : 0;
					f_pvt += row["grouphtype"].ToString() == "P" && row["typeofdeli"].ToString() == "F" ? 1 : 0;
				}
			}
			update_td("DELIVERIES", "3", "NORMAL", 0, n_male, n_female, n_male + n_female, n_co, n_hmo, n_nhis, n_pvt, n_male + n_female < 1 ? 0m : (n_male / tsdt.Rows.Count) * 100, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);

			update_td("DELIVERIES", "3", "CAESARIAN", 0, c_male, c_female, c_male + c_female, c_co, c_hmo, c_nhis, c_pvt, c_male + c_female < 1 ? 0m : ((c_male + c_female) / tsdt.Rows.Count) * 100, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);

			update_td("DELIVERIES", "3", "Still Birth", 0, s_male, s_female, s_male + s_female, s_co, s_hmo, s_nhis, s_pvt, s_male + s_female < 1 ? 0m : ((s_male + s_female) / tsdt.Rows.Count) * 100, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);

			update_td("DELIVERIES", "3", "VACUUM", 0, v_male, v_female, v_male + v_female, v_co, v_hmo, v_hmo, v_pvt, v_male + v_female < 1 ? 0m : ((v_male + v_female) / tsdt.Rows.Count) * 100, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);

			update_td("DELIVERIES", "3", "FORSEP", 0, f_male, f_female, f_male + f_female, f_co, f_hmo, f_nhis, f_pvt, f_male + f_female < 1 ? 0m : ((f_male + f_female) / tsdt.Rows.Count) * 100, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);

		  //  }
 
		}
		void Clinics()
		{
			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = nhis = newnhis = 0;
			//get clinics visit records for the period
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT mrattend.trans_date, mrattend.groupcode, mrattend.patientno, mrattend.grouphtype, mrattend.grouphead, mrattend.clinic, billchain.sex from mrattend left join billchain on mrattend.groupcode = billchain.groupcode and mrattend.patientno = billchain.patientno where trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
 
			foreach (DataRow trow in tsdt.Rows)
			{
			   getPatientProfileBYROW(trow, 4); 
			   update_td("CONSULTATION CLINICS", "4", trow["clinic"].ToString(), 1, male, female, male + female, comp, nhmo, nhis, pvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);
				male = female = male = female = comp = nhmo = pvt = nhis = 0;
				xtotal++;
			}
			//calcaulate amt8 PERCENTAGE
			foreach (DataRow row in sdt.Rows )
			{
				if (row["grptype"].ToString() == "4")
				{
					row["amt8"] = (decimal)row["amt3"] < 1 ? 0m : ((decimal)row["amt3"] / xtotal) * 100;
				}
			}
		}
		void Laboratory(string xtype)
		{
			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = 0;
			string mfacility = "";
			DataTable dt;
			if (xtype == "L")
			{
				dt = Dataaccess.GetAnytable("", "MR", "select pvtcode from mrcontrol where recid = '7'", false);
				mfacility = dt.Rows[0]["pvtcode"].ToString().Trim();
			}
			else
			{
				dt = Dataaccess.GetAnytable("", "MR", "select mpass from mrcontrol where recid = '4'", false);
				mfacility = dt.Rows[0]["mpass"].ToString().Trim();
			}
			if (string.IsNullOrWhiteSpace(mfacility))
			{
				string xstr = xtype == "L" ? "LAB  " : "XRAY ";
				DialogResult result = MessageBox.Show("Setup Error for " + xstr + "Centre : Go to Page 2 of Systems Setup and Define", "Special Centre Setup Error");
				return;
			}
			string selstr = "select labdet.sex, labdet.grouphead, labdet.grouphtype, labdet.grouphead, labdet.trans_date, labdet.groupcode, labdet.patientno, labdet.facility, labdet.reference, labtrans.process, labtrans.description from labdet INNER JOIN labtrans ON labdet.facility = labtrans.facility and labdet.reference = labtrans.reference where labdet.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and labdet.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.99' and labdet.facility = '"+mfacility+"'";
			 tsdt = Dataaccess.GetAnytable("", "MR", selstr, false);
			// getPatientProfile(tsdt, 5);
			 foreach (DataRow row in tsdt.Rows)
			 {
				 getPatientProfileBYROW(row, 1);
				 update_td(xtype == "L" ? "LABORATORY" : "RADIOLOGY", "5", row["description"].ToString(), 1, male, female, male + female, comp, nhmo, nhis, pvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);
				 male = female = comp = nhmo = pvt = nhis = 0;
				 xtotal++;
			 }
			 //calcaulate amt8 PERCENTAGE
			 foreach (DataRow row in sdt.Rows)
			 {
				 if (row["grptype"].ToString() == "5")
				 {
					 row["amt8"] = (decimal)row["amt3"] < 1 || xtotal < 1 ? 0m : ((decimal)row["amt3"] / xtotal) * 100;
				 }
			 }
		}
		void GroupAttendance()
		{
			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = nhis = newnhis = 0;
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT mrattend.trans_date, mrattend.groupcode, mrattend.patientno, mrattend.grouphtype, mrattend.grouphead, billchain.sex from mrattend left join billchain on mrattend.groupcode = billchain.groupcode and mrattend.patientno = billchain.patientno where mrattend.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and mrattend.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
		   // getPatientProfile(tsdt, 6);
			foreach (DataRow row in tsdt.Rows)
			{
				getPatientProfileBYROW(row, 1);
				update_td("GROUP ATTENDANCE RECORD", "6", row["grouphtype"].ToString() == "P" ? "PRIVATE/FAMILY" : mname, 1, male, female, male + female, comp, nhmo, nhis, pvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);
				male = female = comp = nhmo = pvt = nhis = 0;
				xtotal++;
			}
			//calcaulate amt8
			foreach (DataRow row in sdt.Rows)
			{
				if (row["grptype"].ToString() == "6")
				{
					row["amt8"] = (decimal)row["amt3"] < 1 ? 0m : ((decimal)row["amt3"] / xtotal) * 100;
				}
			}
		}
		void DocPatientProfile()
		{
			dtdocs = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D'", false);
			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = nhis = newnhis = 0;
			tsdt = Dataaccess.GetAnytable("", "MR", "SELECT medhist.trans_date, medhist.groupcode, medhist.patientno, medhist.doctor, billchain.grouphtype, billchain.grouphead, billchain.sex from medhist left join billchain on medhist.groupcode = billchain.groupcode and medhist.patientno = billchain.patientno where medhist.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and medhist.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'", false);
		 //   getPatientProfile(tsdt, 7);
			foreach (DataRow row in tsdt.Rows)
			{
				if (string.IsNullOrWhiteSpace(row["doctor"].ToString()) || string.IsNullOrWhiteSpace(row["sex"].ToString() ))
					continue;
				getPatientProfileBYROW(row, 1);
				update_td("DOC-PATIENT PROFILE", "7", row["doctor"].ToString(), 1, male, female, male + female, comp, nhmo, nhis, pvt, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m);
				male = female = comp = nhmo = pvt = nhis = 0;
				xtotal++;
			}
			//calcaulate amt8 PERCENTAGE
			foreach (DataRow row in sdt.Rows)
			{
				if (row["grptype"].ToString() == "7")
				{
					row["amt8"] = (decimal)row["amt3"] < 1 ? 0m : ((decimal)row["amt3"] / xtotal) * 100;
				}
			}
		}
		private void chkDiseaseDiag_Click(object sender, EventArgs e)
		{
			if (chkDiseaseDiag.Checked)
				panel_AgeGroup.Visible = chkReportSum.Visible = true;
			else
				panel_AgeGroup.Visible = chkReportSum.Visible = false;
		}
		void DiseaseQuery()
		{
			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = nhis = newnhis = 0;
			decimal amt1, amt2, amt3, amt4, amt5, amt6, amt7, amt8, amt9, amt10, amt11, amt12, amt13, amt14, amt15, amt16,
				male1, male2, male3, male4, male5, male6, male7, male8, male9, female1, female2, female3, female4, female5, female6, female7, female8, female9, inpat, opd; male1 = male2 = male3 = male4 = male5 = male6 = male7 = male8 = male9 = female1 = female2 = female3 = female4 = female5 = female6 = female7 = female8 = female9 = inpat = opd = 0m;
			amt1 = amt2 = amt3 = amt4 = amt5 = amt6 = amt7 = amt8 = amt9 = amt10 = amt11 = amt12 = amt13 = amt14 = amt15 = amt16 = 0m;
			lblprompt.Text = "Scanning Diagnosis (Out/In) Patient Records...";
			string selstr = "SELECT Pmedhdiag.groupcode,Pmedhdiag.patientno,Pmedhdiag.trans_date,Pmedhdiag.provisional, Pmedhdiag.final AS diagnosis,Pmedhdiag.transtype,Pmedhdiag.reference,'O' AS rectype, billchain.birthdate, billchain.residence AS address1, billchain.sex, billchain.name, billchain.grouphead, billchain.grouphtype from pmedhdiag LEFT JOIN billchain ON pmedhdiag.groupcode = billchain.groupcode and pmedhdiag.patientno = billchain.patientno WHERE pmedhdiag.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and pmedhdiag.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999' UNION select admrecs.groupcode, admrecs.patientno, admrecs.adm_date as trans_date, char(1) AS provisional, diagnosis_all AS diagnosis, admrecs.grouphtype AS transtype, admrecs.reference, 'I' AS rectype, billchain.birthdate, billchain.residence AS address1, billchain.sex, billchain.name, billchain.grouphead, billchain.grouphtype from admrecs LEFT JOIN billchain ON admrecs.groupcode = billchain.groupcode and admrecs.patientno = billchain.patientno WHERE admrecs.adm_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and adm_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";

			DataTable xdt = Dataaccess.GetAnytable("", "MR", selstr, false);
			lblprompt.Text = "Scanning Diagnosis (Out/In) Patient Records..."+xdt.Rows.Count.ToString();
			decimal xage = 0m;
			int xday, xmth, xyear;
			//string querydesc = !string.IsNullOrWhiteSpace(txtQueryDecs.Text) ? txtQueryDecs.Text.Trim().Split(' ') : "";
			foreach (DataRow row in xdt.Rows)
			{
				if (row["birthdate"] == DBNull.Value || Convert.ToDateTime(row["birthdate"]).Year < 1920) //invalid value
					continue;
				xage = Convert.ToDecimal(DateTime.Now.Date.Subtract((DateTime)row["birthdate"]).Days / 365);
				if (nmrAgeFrom.Value >= 1 && xage < 1 || nmrAgeFrom.Value > 0 && xage < nmrAgeFrom.Value || nmrAgeTo.Value > 0 && xage > nmrAgeTo.Value || chk0to5yrs.Checked && xage > 5)
					continue;
				//merge provisional and final diagnosis to avoid duplication
				row["diagnosis"] = row["diagnosis"].ToString().Trim() + "" + row["provisional"].ToString().Trim();

				//we must check for specified query
				if (string.IsNullOrWhiteSpace(row["diagnosis"].ToString()) || !string.IsNullOrWhiteSpace(txtQueryDecs.Text) && !txtQueryDecs.Text.Trim().Contains(row["diagnosis"].ToString().Trim()) || row["sex"].ToString().Length < 1) 
					continue;

				if (!string.IsNullOrWhiteSpace(cboGender.Text) && row["sex"].ToString().Substring(0,1) != cboGender.Text.Substring(0,1) || !string.IsNullOrWhiteSpace(cboResidence.Text) && !row["residence"].ToString().Trim().Contains(cboResidence.Text.Trim()))
					continue;

				if (!string.IsNullOrWhiteSpace(txtpatientno.Text) && row["grouphead"].ToString() != txtpatientno.Text)
					continue;


				if (!chkReportSum.Checked) //detail
					UpdatediseaseDtl(row,"D");
				else
				{
					//get day and month
					xday = xmth = xyear = 0;
					xyear = DateTime.Now.Year - Convert.ToDateTime(row["birthdate"]).Year;
					int totdays = Convert.ToInt32(DateTime.Now.Date.Subtract((DateTime)row["birthdate"]).TotalDays);
					xmth = totdays / 30;
					xday = totdays - (xmth * 30);
					if (chk0to5yrs.Checked)
					{
						if (xyear < 1 && xday < 29 && xmth == 0)
						{
							if (row["sex"].ToString().Substring(0,1) == "M")
							{
								male++;
								amt1++;
							}
							else
							{
								female++;
								amt4++;
							}
						}
						else if (xday > 28 && xmth < 12 && xyear == 0)
						{
							if (row["sex"].ToString().Substring(0, 1) == "M")
							{
								male++;
								amt2++;
							}
							else
							{
								female++;
								amt5++;
							}
						}
						else //if (xmth > 11 )
						{
							if (row["sex"].ToString().Substring(0, 1) == "M")
							{
								male++;
								amt3++;
							}
							else
							{
								female++;
								amt6++;
							}
						}
					}
					else //0-65years
					{
						if (xyear < 15)
						{
							if (row["sex"].ToString().Substring(0, 1) == "M")
							{
								male++;
								amt1++;
							}
							else
							{
								female++;
								amt2++;
							}
						  //  xtotal++;
						}
						else if (xyear > 14 && xyear < 26)
						{
							if (row["sex"].ToString().Substring(0, 1) == "M")
							{
								male++;
								amt3++;
							}
							else
							{
								female++;
								amt4++;
							}
						}
						else if (xyear > 25 && xyear < 40)
						{
							if (row["sex"].ToString().Substring(0, 1) == "M")
							{
								male++;
								amt5++;
							}
							else
							{
								female++;
								amt6++;
							}
						}
						else if (xyear > 39 && xyear < 60)
						{
							if (row["sex"].ToString().Substring(0, 1) == "M")
							{
								male++;
								amt7++;
							}
							else
							{
								female++;
								amt8++;
							}
						}
						else if (xyear > 59)
						{
							if (row["sex"].ToString().Substring(0, 1) == "M")
							{
								male++;
								amt9++;
							}
							else
							{
								female++;
								amt10++;
							}
						}
					}
					if (row["grouphtype"].ToString() == "P")
					{
						pvt++;
						if (row["sex"].ToString().Substring(0, 1) == "M")
							amt15++;
						else
							amt16++;
					}
					else
					{
						if (row["sex"].ToString().Substring(0, 1) == "M")
							amt13++;
						else
							amt14++;
					}
					if (row["rectype"].ToString() == "I")
						inpat++;
					else
						opd++;
					if (chk0to5yrs.Checked)
						update_td("DISEASE/DIAGNOSIS REPORT", "8", row["diagnosis"].ToString(), 0, amt1, amt2, amt3, amt4, amt5, amt6, male, female, amt9, amt10, amt11, amt12, amt13, amt4, amt15, amt16);
					else
						update_td("DISEASE/DIAGNOSIS REPORT", "8", row["diagnosis"].ToString(), 0, amt1, amt2, amt3, amt4, amt5, amt6, amt7, amt8, amt9, amt10, male, female, amt13, amt14, amt15, amt16);

					male1 = male2 = male3 = male4 = male5 = female1 = female2 = female3 = female4 = female5 = comp = pvt = inpat = opd = nhis = 0m;
					amt1 = amt2 = amt3 = amt4 = amt5 = amt6 = amt7 = amt8 = amt9 = amt10 = amt11 = amt12 = amt13 = amt14 = amt15 = amt16 = 0m;
					xtotal++;
				}
				//there is need to sort,filter  and merge diagnosis
				//create temp table to hold consolidated/merged records
				if (chkReportSum.Checked)
				{ 
				   tsdt = new DataTable();
				   tsdt = sdt.Clone();
				   // tsdt = xdt.Copy(); //dtrtn.ImportRow(row);
				 //   xdt.Columns.Add(new DataColumn("diag", typeof(string)));
					bool foundit = false;
					DataRow rtnrow = null;
					foreach (DataRow xdr in sdt.Rows)
					{
						foreach (DataRow mrow in tsdt.Rows)
						{
							if (xdr["DESCRIPTION"].ToString().Trim() == xdr["DESCRIPTION"].ToString().Trim())
							{
								foundit = true;
								rtnrow = mrow;
								break;
							}
						}
						if (!foundit)
						{
							rtnrow = createnewRow("");
							rtnrow["reference"] = xdr["reference"].ToString();
							rtnrow["GRPDESC"] = xdr["GRPDESC"].ToString();
							rtnrow["GRPTYPE"] = xdr["GRPTYPE"].ToString();
							rtnrow["DESCRIPTION"] = xdr["DESCRIPTION"].ToString();
						}

						rtnrow["amt1"] = (decimal)rtnrow["amt1"] + (decimal)xdr["amt1"];
						rtnrow["amt2"] = (decimal)rtnrow["amt2"] + (decimal)xdr["amt2"];
						rtnrow["amt3"] = (decimal)rtnrow["amt3"] + (decimal)xdr["amt3"];
						rtnrow["amt4"] = (decimal)rtnrow["amt4"] + (decimal)xdr["amt4"];
						rtnrow["amt5"] = (decimal)rtnrow["amt5"] + (decimal)xdr["amt5"];
						rtnrow["amt6"] = (decimal)rtnrow["amt6"] + (decimal)xdr["amt6"];
						rtnrow["amt7"] = (decimal)rtnrow["amt7"] + (decimal)xdr["amt7"];
						rtnrow["amt8"] = (decimal)rtnrow["amt8"] + (decimal)xdr["amt8"];
						rtnrow["amt9"] = (decimal)rtnrow["amt9"] + (decimal)xdr["amt9"];
						rtnrow["amt10"] = (decimal)rtnrow["amt10"] + (decimal)xdr["amt10"];
						rtnrow["amt11"] = (decimal)rtnrow["amt11"] + (decimal)xdr["amt11"];
						rtnrow["amt12"] = (decimal)rtnrow["amt12"] + (decimal)xdr["amt12"];
						rtnrow["amt13"] = (decimal)rtnrow["amt13"] + (decimal)xdr["amt13"];
						rtnrow["amt14"] = (decimal)rtnrow["amt14"] + (decimal)xdr["amt14"];
						rtnrow["amt15"] = (decimal)rtnrow["amt15"] + (decimal)xdr["amt15"];
						rtnrow["amt16"] = (decimal)rtnrow["amt16"] + (decimal)xdr["amt16"];
					}
				}
			}
			if (chkReportSum.Checked)
			{
				bool is60 = chk0to60yrs.Checked ? true : false;
				foreach (DataRow row in sdt.Rows)
				{
					if (row["grptype"].ToString().Trim() == "8")
					{
					  //  male += (decimal)row["amt7"];
					  //  female += (decimal)row["amt8"];
						amt1 += (decimal)row["amt1"];
						amt2 += (decimal)row["amt2"];
						amt3 += (decimal)row["amt3"];
						amt4 += (decimal)row["amt4"];
						amt5 += (decimal)row["amt5"];
						amt6 += (decimal)row["amt6"];
						if (chk0to60yrs.Checked)
						{
							amt7 += (decimal)row["amt7"];
							amt8 += (decimal)row["amt8"];
							amt9 += (decimal)row["amt9"];
							amt10 += (decimal)row["amt10"];
						}
						else
						{
							amt7 += (decimal)row["amt7"];
							amt8 += (decimal)row["amt8"];
						}
					}
				}
				update_td("DISEASE/DIAGNOSIS REPORT", "8", "AGGREGATE IN %", 0, (amt1 / xtotal) * 100, (amt2 / xtotal) * 100, (amt3 / xtotal) * 100, (amt4 / xtotal) * 100, (amt5 / xtotal) * 100, (amt6 / xtotal) * 100, is60 ? (amt7 / xtotal) * 100 : male, is60 ? (amt8 / xtotal) * 100 : female, is60 ? (amt9 / xtotal) * 100: 0m, is60 ? (amt10 / xtotal) * 100 : 0m, is60 ? male : 0m, is60 ? female : 0m, 0m, 0m, 0m, 0m);
			}
		}
		void createDiseaseSummary()
		{
			sdt = new DataTable(); //table will be passed to report dataset 
			sdt.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
			sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
			sdt.Columns.Add(new DataColumn("DESCRIPTION", typeof(string)));
			sdt.Columns.Add(new DataColumn("TRANS_DATE", typeof(DateTime )));
			sdt.Columns.Add(new DataColumn("ADDRESS", typeof(string)));
			sdt.Columns.Add(new DataColumn("AGE", typeof(string)));
		}
		void UpdatediseaseDtl(DataRow drow, string xtype)
		{
			DataRow dr = null;
			dr = sdt.NewRow();
			dr["reference"] = drow["groupcode"].ToString().Trim() + " " + drow["patientno"].ToString();
			dr["name"] = drow["name"].ToString();
			dr["Description"] = xtype == "D" ? drow["diagnosis"].ToString() : drow["stk_desc"].ToString();
			dr["trans_date"] = (DateTime)drow["trans_date"];
			dr["address"] = drow["address1"].ToString();
			dr["age"] = bissclass.agecalc((DateTime)drow["birthdate"], DateTime.Now.Date) + " [" + drow["sex"].ToString().Substring(0, 1);
			sdt.Rows.Add(dr);
		   // return dr;
		}
		void Pharmacology()
		{
			xtotal = newmale = newfemale = newcomp = newpvt = male = female = comp = pvt = nhmo = newhmo = nhis = newnhis = 0;
			decimal amt1, amt2, amt3, amt4, amt5, amt6, amt7, amt8, amt9, amt10, amt11, amt12, amt13, amt14, amt15, amt16, male1, male2, male3, male4, male5, male6, male7, male8, male9, female1, female2, female3, female4, female5, female6, female7, female8, female9, inpat, opd; male1 = male2 = male3 = male4 = male5 = male6 = male7 = male8 = male9 = female1 = female2 = female3 = female4 = female5 = female6 = female7 = female8 = female9 = inpat = opd = 0m;
			amt1 = amt2 = amt3 = amt4 = amt5 = amt6 = amt7 = amt8 = amt9 = amt10 = amt11 = amt12 = amt13 = amt14 = amt15 = amt16 = 0m;
			lblprompt.Text = "Scanning Pharmacy/Dispenary Records...";
			string selstr = "SELECT Dispensa.name, Dispensa.groupcode, Dispensa.patientno, Dispensa.grouphtype, Dispensa.grouphead, Dispensa.trans_date, Dispensa.stk_desc, Dispensa.qty_pr, Dispensa.cum_gv, Dispensa.nurse, Dispensa.cost, Dispensa.reference, billchain.birthdate, billchain.residence AS address1, billchain.sex, billchain.grouphead FROM Dispensa LEFT JOIN billchain ON Dispensa.groupcode = billchain.groupcode and Dispensa.patientno = billchain.patientno WHERE Dispensa.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and Dispensa.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";

			tsdt = Dataaccess.GetAnytable("", "MR", selstr, false);
			lblprompt.Text = "Scanning Pharmacy/Dispenary Records..." + tsdt.Rows.Count.ToString();
			decimal xage = 0m;
			int xday, xmth, xyear;
			foreach (DataRow row in tsdt.Rows)
			{
				if (!string.IsNullOrWhiteSpace(txtQueryDecs.Text) && !txtQueryDecs.Text.Trim().Contains(row["stk_desc"].ToString().Trim())) //we must check for pecified query
					continue;

				if (!string.IsNullOrWhiteSpace(cboGender.Text) && row["sex"].ToString().Substring(0, 1) != cboGender.Text.Substring(0, 1) || !string.IsNullOrWhiteSpace(cboResidence.Text) && !row["residence"].ToString().Trim().Contains(cboResidence.Text.Trim()))
					continue;

				if (!string.IsNullOrWhiteSpace(txtpatientno.Text) && row["grouphead"].ToString() != txtpatientno.Text)
					continue;
				xage = Convert.ToDecimal(DateTime.Now.Date.Subtract((DateTime)row["birthdate"]).Days / 365);
				if (nmrAgeFrom.Value >= 1 && xage < 1 || nmrAgeFrom.Value > 0 && xage < nmrAgeFrom.Value || nmrAgeTo.Value > 0 && xage > nmrAgeTo.Value || chk0to5yrs.Checked && xage > 5)
					continue;

				if (!chkReportSum.Checked) //detail
					UpdatediseaseDtl(row,"P");
				else
				{
					//get day and month
					xday = xmth = xyear = 0;
					xyear = DateTime.Now.Year - Convert.ToDateTime(row["birthdate"]).Year;
					int totdays = Convert.ToInt32(DateTime.Now.Date.Subtract((DateTime)row["birthdate"]).TotalDays);
					xmth = totdays / 30;
					xday = totdays - (xmth * 30);

					if (xyear < 15)
					{
						if (row["sex"].ToString() == "MALE")
						{
							amt1++;
							male++;
						}
						else
						{
							amt2++;
							female++;
						}
					 //   xtotal++;
					}
					else if (xyear > 14 && xyear < 26)
					{
						if (row["sex"].ToString() == "MALE")
						{
							amt3++;
							male++;
						}
						else
						{
							amt4++;
							female++;
						}
					  //  xtotal++;
					}
					else if (xyear > 25 && xyear < 40)
					{
						if (row["sex"].ToString() == "MALE")
						{
							amt5++;
							male++;
						}
						else
						{
							amt6++;
							female++;
						}
					  //  xtotal++;
					}
					else if (xyear > 39 && xyear < 60)
					{
						if (row["sex"].ToString() == "MALE")
						{
							amt7++;
							male++;
						}
						else
						{
							amt8++;
							female++;
						}
					  //  xtotal++;
					}
					else if (xyear > 59)
					{
						if (row["sex"].ToString() == "MALE")
						{
							amt9++;
							male++;
						}
						else
						{
							amt10++;
							female++;
						}
					  //  xtotal++;
					}
					if (row["grouphtype"].ToString() == "P")
					{
						pvt++;
						if (row["sex"].ToString() == "MALE")
							amt15++;
						else
							amt16++;
					}
					else
					{
						//foreach (DataRow crow in dtcust.Rows)
						//{
						//    if (row["grouphead"].ToString() == crow["custno"].ToString())
						//    {
						//        if ((bool)crow["hmo"])
						//            nhmo++;
						//        else
						//            comp++;
						//        break;
						//    }
						//}
						if (row["sex"].ToString() == "MALE")
							amt13++;
						else
							amt14++;
						if (row["reference"].ToString().Length > 0 && row["reference"].ToString().Substring(0, 1) == "A")
							inpat++;
						else
							opd++;
					}
					update_td("PRESCRIPTION STATISTICS", "9", row["diagnosis"].ToString(), 0, amt1, amt2, amt3, amt4, amt5, amt6, amt7, amt8, amt9, amt10, male, female, amt12, amt13, inpat, opd);

					male1 = male2 = male3 = male4 = male5 = female1 = female2 = female3 = female4 = female5 = comp = pvt = inpat = opd = 0m;
					amt1 = amt2 = amt3 = amt4 = amt5 = amt6 = amt7 = amt8 = amt9 = amt10 = amt11 = amt12 = amt13 = amt14 = amt15 = amt16 = 0m;
					xtotal++;
				}
			}
			if (!chkReportSum.Checked)
			{
				foreach (DataRow row in sdt.Rows)
				{
					if (row["grptype"].ToString().Trim() == "9")
					{
						male += (decimal)row["male"];
						female += (decimal)row["female"];
						amt1 += (decimal)row["amt1"];
						amt2 += (decimal)row["amt2"];
						amt3 += (decimal)row["amt3"];
						amt4 += (decimal)row["amt4"];
						amt5 += (decimal)row["amt5"];
						amt6 += (decimal)row["amt6"];
						amt7 += (decimal)row["amt7"];
						amt8 += (decimal)row["amt8"];
						amt9 += (decimal)row["amt9"];
						amt10 += (decimal)row["amt10"];
					}
				}
				update_td("PRESCRIPTION STATISTICS", "9", "AGGREGATE IN %", 0, (amt1 / xtotal) * 100, (amt2 / xtotal) * 100, (amt3 / xtotal) * 100, (amt4 / xtotal) * 100, (amt5 / xtotal) * 100, (amt6 / xtotal) * 100, (amt7 / xtotal) * 100, (amt8 / xtotal) * 100, (amt9 / xtotal) * 100, (amt10 / xtotal) * 100, male, female, 0m, 0m, 0m, 0m);
			}

			/*

		append blank
			replace descriptn with 'TOTAL',amt1 with agra_[1,1],;
			amt2 with agra_[1,2],amt3 with agra_[1,3],;
			amt4 with agra_[1,4],amt5 with agra_[1,5],amt6 with agra_[1,6],gtype with '8',;
			grpdesc with 'PRESCRIPTION STATISTICS'
		append blank
			replace descriptn with 'AGGREGATE IN %',amt1 with (agra_[1,1]/xtotal)*100,;
				amt2 with (agra_[1,2]/xtotal)*100,amt3 with (agra_[1,3]/xtotal)*100,;
				amt4 with (agra_[1,4]/xtotal)*100,amt5 with (agra_[1,5]/xtotal)*100,;
				amt6 with (agra_[1,6]/xtotal)*100,gtype with '8',grpdesc with 'PRESCRIPTION STATISTICS'
		append blank
			replace descriptn with 'SUMMARY - Frequenty',amt1 with male,;
			amt2 with female,amt3 with xinpatient,amt4 with xoutpatient,amt5 with xoutpatient+xinpatient,gtype with '9',;
			grpdesc with 'S U M M A R Y '
			
		append blank
			replace descriptn with 'SUMMARY - Cost',;
			amt3 with mval_inpat,amt4 with mval_opd,amt5 with mval_opd+mval_inpat,gtype with '9',;
			grpdesc with 'S U M M A R Y '
*/
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		void printprocess(bool isprint)
		{
			if (dtDateto.Value > DateTime.Now.Date || dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateto.Value < dtDateFrom.Value )
			{
				DialogResult result = MessageBox.Show("Invalid Date specification...");
				return;
			}
			Session["sql"] = "";
			if (chkDiseaseDiag.Checked && !chkReportSum.Checked) // detail report
				createDiseaseSummary();
			else
				createSummary();
			ds = new DataSet();
			/*if (sdt != null)
			{
				sdt.Rows.Clear();
				ds.Tables.Clear();
				ds.Clear();
			}*/
			if (chkAll.Checked)
			{
				outpatient();
				inpatient();
				Delivery();
				Clinics();
				Laboratory("L");
				Laboratory("X");
				GroupAttendance();
				DocPatientProfile();
				ds.Tables.Add(sdt);
				Session["rdlcfile"] = "clinicalstatistics.rdlc";
			}
			else if (chkOutPatient.Checked)
				outpatient();
			else if (chkInPatient.Checked)
				inpatient();
			else if (chkDelivery.Checked)
				Delivery();
			else if (chkConsultClinics.Checked)
				Clinics();
			else if (chkLaboratory.Checked)
				Laboratory("L");
			else if (chkRadiology.Checked)
				Laboratory("X");
			else if (chkGroupAttendance.Checked)
				GroupAttendance();
			else if (chkDocPatProfile.Checked)
				DocPatientProfile();
			else if (chkDiseaseDiag.Checked)
			{
				DiseaseQuery();
				if (chkReportSum.Checked)
					ds.Tables.Add(tsdt);
				else
					ds.Tables.Add(sdt);

				if (chkReportSum.Checked && chk0to60yrs.Checked)
					Session["rdlcfile"] = "clStat_Disease60Summ.rdlc";
				else if (chkReportSum.Checked && chk0to5yrs.Checked)
					Session["rdlcfile"] = "clStat_Disease5Summ.rdlc";
				else
					Session["rdlcfile"] = "clStat_DiseaseDtl.rdlc";
			}
			else if (chkPrescriptionStatistics.Checked)
			{
				Pharmacology();
				ds.Tables.Add(sdt);
				if (chkReportSum.Checked)
					Session["rdlcfile"] = "clStat_Disease60Summ.rdlc";
				else
					Session["rdlcfile"] = "clStat_DiseaseDtl.rdlc";
			}
			if (chkOutPatient.Checked || chkInPatient.Checked || chkDelivery.Checked || chkConsultClinics.Checked ||chkLaboratory.Checked || chkRadiology.Checked || chkGroupAttendance.Checked || chkDocPatProfile.Checked)
			{
				ds.Tables.Add(sdt);
				Session["rdlcfile"] = "clinicalstatistics.rdlc";
			}
			string mrptheader = "CLINICAL STATISTICS FOR " + dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString();
			if (!isprint)
			{
				frmReportViewer paedreports = new frmReportViewer("CLINICAL STATISTICS", mrptheader, "", "", "", "CLSTATISTICS", "", 0m, "", "", "", ds, true, 0, dtDateFrom.Value.Date, dtDateto.Value.Date, "", isprint, "W", "");

				//if (isprint)
				//    paedreports.work();
				//else
				paedreports.Show();
			}
			else
			{
				MRrptConversion.GeneralRpt("CLINICAL STATISTICS", mrptheader, "", "", "", "CLSTATISTICS", "", 0m, "", "", "", ds, 0, dtDateFrom.Value.Date, dtDateto.Value.Date, "", true, true, "W", "");
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



 /*			replace descriptn with 'AGGREGATE IN %',amt1 with (agra_[1,1]/agra_[1,9])*100,; &&xtotal)*100
				amt2 with (agra_[1,2]/agra_[1,9])*100,amt3 with (agra_[1,3]/agra_[1,9])*100,;
				amt4 with (agra_[1,4]/agra_[1,9])*100,amt5 with (agra_[1,5]/agra_[1,9])*100,;
				amt6 with (agra_[1,6]/agra_[1,9])*100,amt7 with (agra_[1,7]/agra_[1,9])*100,;
				amt8 with (agra_[1,8]/agra_[1,9])*100,gtype with '9',grpdesc with 'DISEASE/DIAGNOSIS REPORT'
*/

	}
}