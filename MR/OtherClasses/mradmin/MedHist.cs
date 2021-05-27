using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using msfunc;
using mradmin.BissClass;
//using mradmin.DataAccess;

namespace mradmin.DataAccess
{
	public class MedHist
	{
		public string REFERENCE { get; set; }
		public string PATIENTNO { get; set; }
		public string NAME { get; set; }
		public DateTime TRANS_DATE { get; set; }
		public string CTIME { get; set; } 
		public string COMMENTS { get; set; }
		public bool POSTED { get; set; }
		public DateTime POST_DATE { get; set; }
		public string BILLED { get; set; }
		public string BILLREF { get; set; }
		public decimal AMOUNT { get; set; }
		public string GROUPCODE { get; set; }
		public string WEIGHT { get; set; }
		public string HEIGHT { get; set; }
		public string BP_S { get; set; }
		public string BP_STN { get; set; }
		public string TEMP { get; set; }
		public string PULSE { get; set; }
		public string RESPIRATIO { get; set; }
		public string OTHERS { get; set; }
		public string DOCTOR { get; set; }
		public string CLINIC { get; set; }
		public decimal PROTECTED { get; set; }
		public bool ENCHRYPTED { get; set; }
		public string GHGROUPCODE { get; set; }
		public string GROUPHEAD { get; set; }
		public string OPERATOR { get; set; }
		public DateTime DTIME { get; set; }





/// <summary>
/// Returns Patient Med.Hist by Reference Only,By date on groupcode+patientno+transdat or 
/// By Patient on groupcode+patientno 
/// </summary>
/// <param name="GroupCodeID"></param>
/// <param name="PatientID"></param>
/// <param name="xreference"></param>
/// <param name="ByReference"></param>
/// <param name="ByDate"></param>
/// <param name="xtrans_date"></param>
/// <returns></returns>


		public static MedHist GetMEDHIST(string GroupCodeID, string PatientID, string xreference, bool ByReference,bool ByDate, DateTime xtrans_date, string xsortorder)
		{
			MedHist medhist = new MedHist();
			DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
			SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
			SqlCommand selectCommand = new SqlCommand();
			selectCommand.CommandText = (ByReference) ? "MEDHIST_Get" : (ByDate) ? "MEDHIST_GetByDate" : "MEDHIST_GetByPatient";
			selectCommand.Connection = connection;
			selectCommand.CommandType = CommandType.StoredProcedure;

			if (ByReference)
				selectCommand.Parameters.AddWithValue("@Reference", xreference);
			else if (ByDate)
			{
				selectCommand.Parameters.AddWithValue("@Groupcode", GroupCodeID);
				selectCommand.Parameters.AddWithValue("@Patientno", PatientID);
				selectCommand.Parameters.AddWithValue("@trans_date", xtrans_date);
			}
			else
			{
				selectCommand.Parameters.AddWithValue("@GroupCode", GroupCodeID);
				selectCommand.Parameters.AddWithValue("@Patientno", PatientID);
			}
			selectCommand.Parameters.AddWithValue("@sortorder", xsortorder);
			try
			{
				connection.Open();
				SqlDataReader reader = (ByReference || ByDate ) ? selectCommand.ExecuteReader(CommandBehavior.SingleRow) :
										selectCommand.ExecuteReader();

				if (reader.Read())
				{
					medhist.REFERENCE = reader["reference"].ToString();
					medhist.PATIENTNO = reader["patientno"].ToString();
					medhist.NAME = reader["name"].ToString();
					medhist.TRANS_DATE = (DateTime)reader["trans_date"];
					medhist.COMMENTS = reader["comments"].ToString();
					medhist.POSTED = (reader["posted"] == DBNull.Value ) ? false : (bool)reader["posted"];
					medhist.POST_DATE = (reader["posted"] == DBNull.Value ) ? dtmin_date :  (DateTime)reader["post_date"];
					medhist.BILLED = (reader["billed"] == DBNull.Value ) ? "" : reader["billed"].ToString();
					medhist.BILLREF = (reader["billref"] == DBNull.Value ) ?  "" : reader["billref"].ToString();
					medhist.AMOUNT = (reader["posted"] == DBNull.Value ) ? 0m : (Decimal)reader["amount"];
					medhist.GROUPCODE = reader["groupcode"].ToString();
					medhist.WEIGHT = (reader["weight"] == DBNull.Value ) ? "" : reader["weight"].ToString();
					medhist.HEIGHT = (reader["height"] == DBNull.Value ) ? "" : reader["height"].ToString();
					medhist.BP_S = (reader["bp_s"] == DBNull.Value ) ? "" : reader["bp_s"].ToString();
					medhist.BP_STN = (reader["bp_stn"] == DBNull.Value ) ? "" : reader["bp_stn"].ToString();
					medhist.TEMP = (reader["temp"] == DBNull.Value ) ? "" : reader["temp"].ToString();
					medhist.PULSE = (reader["pulse"] == DBNull.Value ) ? "" : reader["pulse"].ToString();
					medhist.RESPIRATIO = (reader["respiratio"] == DBNull.Value ) ? "" : reader["respiratio"].ToString();
					medhist.OTHERS = (reader["others"] == DBNull.Value ) ? "" : reader["others"].ToString();
					medhist.DOCTOR = (reader["doctor"] == DBNull.Value ) ? "" : reader["doctor"].ToString();
					medhist.PROTECTED = (reader["protected"] == DBNull.Value ) ? 0m :  (Decimal)reader["protected"];
					medhist.ENCHRYPTED = (reader["ENCHRYPTED"] == DBNull.Value ) ? false : (bool)reader["ENCHRYPTED"];
					medhist.GHGROUPCODE = (reader["ghgroupcode"] == DBNull.Value) ? "" : reader["ghgroupcode"].ToString();
					medhist.GROUPHEAD = (reader["grouphead"] == DBNull.Value) ? "" : reader["grouphead"].ToString();
					medhist.OPERATOR = (reader["operator"] == DBNull.Value) ? "" : reader["operator"].ToString();
					medhist.DTIME = (reader["dtime"] == DBNull.Value) ? dtmin_date : (DateTime)reader["dtime"];

				}
				else
				{
					connection.Close();
					return null;

				}
				reader.Close();
			}
			catch (Exception ex)
			{
				//throw ex;
				MessageBox.Show("" + ex, "Patient Medical Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				connection.Close();
				return null;
			}
			finally
			{
				connection.Close();
			}

			return medhist;
		}
		/// <summary>
		/// Returns patient history DT on ordertype "A"ascending or descing. Ideal for viewing pat.prev med history during consulting
		/// </summary>
		/// <param name="GroupCodeID"></param>
		/// <param name="PatientID"></param>
		/// <param name="ordertype"></param>
		/// <param name="xdatefrom"></param>
		/// <param name="xdateto"></param>
		/// <param name="isall"></param>
		/// <returns></returns>
		public static DataTable GetHISTByPatient(string GroupCodeID, string PatientID, string ordertype, DateTime xdatefrom, DateTime xdateto,bool isall)
		{
			MedHist medhist = new MedHist();

			string xsort = (ordertype == "A") ? " [ ASC ] " : " [ DESC ] ";
			SqlConnection cs = Dataaccess.mrConnection();
			string selcommand = "";
			if (isall)
			{
				selcommand = "SELECT REFERENCE, PATIENTNO, NAME, TRANS_DATE, COMMENTS, GROUPCODE, DOCTOR, CLINIC, PROTECTED, ENCHRYPTED FROM MEDHIST WHERE GROUPCODE = '" + GroupCodeID + "' AND PATIENTNO = '" + PatientID + "' ORDER BY TRANS_DATE" + xsort;
			}
			else
			{
				selcommand = "SELECT REFERENCE, PATIENTNO, NAME, TRANS_DATE, COMMENTS, GROUPCODE, DOCTOR, CLINIC, PROTECTED, ENCHRYPTED FROM MEDHIST WHERE GROUPCODE = '" + GroupCodeID + "' AND PATIENTNO = '" + PatientID + "' AND TRANS_DATE >= @xdatefrom AND TRANS_DATE <= @xdateto ORDER BY TRANS_DATE" + xsort;
			}

			SqlCommand selectCommand = new SqlCommand(selcommand, cs);
			SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
			DataTable dt = new DataTable();

			ds.Fill(dt);
			cs.Close();
			return dt;
		}
/*            try
			{
				cs.Open();
				SqlDataReader reader = selectCommand.ExecuteReader();
				if (reader.Read())
				{
					medhist.REFERENCE = reader["reference"].ToString();
					medhist.PATIENTNO = reader["patientno"].ToString();
					medhist.NAME = reader["name"].ToString();
					medhist.TRANS_DATE = (DateTime)reader["trans_date"];
					medhist.COMMENTS = reader["comments"].ToString();
					medhist.GROUPCODE = reader["groupcode"].ToString();
					medhist.DOCTOR = reader["doctor"].ToString();
					medhist.PROTECTED = (Decimal)reader["protected"];
					medhist.ENCHRYPTED = (bool)reader["enchryted"];

				}
				else
				{
					cs.Close();
					return null;

				}
				reader.Close();
			}
			catch (Exception ex)
			{
				//throw ex;
				MessageBox.Show("" + ex, "Patient Medical Details ", MessageBoxButtons.OK,
	MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				cs.Close();
				return null;
			}
			finally
			{
				cs.Close();
			}

			return medhist;
		}*/

		public static void updatemedhistcomments(string GroupCode, string Patientid, DateTime xtrans_date,string xcomments,bool newrec,string xreference,string xname, string xghgroupcode, string xgrouphead,string doctor)
		{
		   // MedHist medhist = MedHist.GetMEDHIST(GroupCode,Patientid,"",false,xtrans_date);

			SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
			SqlCommand insertCommand = new SqlCommand();
			insertCommand.CommandText = (newrec) ? "MEDHIST_Add" : "MEDHIST_UpdateComments";
			insertCommand.Connection = connection;
			insertCommand.CommandType = CommandType.StoredProcedure;
			if (newrec)
			{
				//get current viatas
				Vstata vstatas = Vstata.GetVSTATA(xreference); 
				insertCommand.Parameters.AddWithValue("@REFERENCE", xreference);
				insertCommand.Parameters.AddWithValue("@PATIENTNO", Patientid);
				insertCommand.Parameters.AddWithValue("@NAME", xname);
				insertCommand.Parameters.AddWithValue("@TRANS_DATE", xtrans_date);
				insertCommand.Parameters.AddWithValue("@CTIME", DateTime.Now.ToShortTimeString());
				insertCommand.Parameters.AddWithValue("@COMMENTS", xcomments);
				insertCommand.Parameters.AddWithValue("@POSTED", false);
				insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
				insertCommand.Parameters.AddWithValue("@BILLED", "");
				insertCommand.Parameters.AddWithValue("@BILLREF", "");
				insertCommand.Parameters.AddWithValue("@AMOUNT", 0m);
				insertCommand.Parameters.AddWithValue("@GROUPCODE", GroupCode);
				insertCommand.Parameters.AddWithValue("@WEIGHT", vstatas == null ? 0m : vstatas.WEIGHT);
				insertCommand.Parameters.AddWithValue("@HEIGHT", vstatas == null ? 0m : vstatas.HIGHT);
				insertCommand.Parameters.AddWithValue("@BP_S", vstatas == null ? "" : vstatas.BPSITTING);
				insertCommand.Parameters.AddWithValue("@BP_STN", vstatas == null ? "" : vstatas.BPSTANDING);
				insertCommand.Parameters.AddWithValue("@TEMP", vstatas == null ? "" : vstatas.TEMP);
				insertCommand.Parameters.AddWithValue("@PULSE", vstatas == null ? "" : vstatas.PULSE);
				insertCommand.Parameters.AddWithValue("@RESPIRATIO", vstatas == null ? "" : vstatas.RESPIRATIO);
				insertCommand.Parameters.AddWithValue("@OTHERS", vstatas == null ? "" : vstatas.OTHERS);
				insertCommand.Parameters.AddWithValue("@DOCTOR", doctor); //initial doctor
				insertCommand.Parameters.AddWithValue("@CLINIC", vstatas == null ? "" : vstatas.CLINIC);
				insertCommand.Parameters.AddWithValue("@PROTECTED", 0m);
				insertCommand.Parameters.AddWithValue("@ENCHRYPTED", false);
				insertCommand.Parameters.AddWithValue("@GHGROUPCODE", xghgroupcode);
				insertCommand.Parameters.AddWithValue("@GROUPHEAD", xgrouphead);
				insertCommand.Parameters.AddWithValue("@OPERATOR", msmrfunc.mrGlobals.WOPERATOR);
				insertCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);
			}
			else
			{
				insertCommand.Parameters.AddWithValue("@patientno", Patientid);
				insertCommand.Parameters.AddWithValue("@trans_date", xtrans_date);
				insertCommand.Parameters.AddWithValue("@comments", xcomments);
				insertCommand.Parameters.AddWithValue("@groupcode", GroupCode);
				insertCommand.Parameters.AddWithValue("@operator", msmrfunc.mrGlobals.WOPERATOR);
				insertCommand.Parameters.AddWithValue("@dtime", DateTime.Now);
				//insertCommand.Parameters.AddWithValue("@DOCTORS1",  "");
				//insertCommand.Parameters.AddWithValue("@DOCTORS2", "");
				//insertCommand.Parameters.AddWithValue("@DOCTORS3", "");
				//insertCommand.Parameters.AddWithValue("@DOCTORS4", "");
			}

			try
			{
				connection.Open();
				insertCommand.ExecuteNonQuery();

			}
			catch (SqlException ex)
			{
				// throw ex;
				MessageBox.Show(" " + ex, "Medical History Update", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				return;
			}
			finally
			{
				connection.Close();
			}
		}
		public static string GetMEDHISTCaseNotes(string GroupCodeID, string PatientID, bool ByPatient, bool ByDate, DateTime xdatefrom,DateTime xdateto, billchaindtl bchain, string xsortorder)
		{
			MedHist medhist = new MedHist();
			string details = "";
			DataTable dtchained = new DataTable();
		  //  DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
			SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
			SqlCommand selectCommand = new SqlCommand();
			selectCommand.CommandText = (ByDate) ? "MEDHIST_GetByDateCaseNote" : "MEDHIST_GetByPatientCaseNote";
			selectCommand.Connection = connection;
			selectCommand.CommandType = CommandType.StoredProcedure;


				selectCommand.Parameters.AddWithValue("@Groupcode", GroupCodeID);
				selectCommand.Parameters.AddWithValue("@Patientno", PatientID);
				selectCommand.Parameters.AddWithValue("@datefrom", xdatefrom );
				selectCommand.Parameters.AddWithValue("@dateto", xdateto);
				selectCommand.Parameters.AddWithValue("@sortorder", xsortorder);
			try
			{
				string tmpstring = "";
				bool ftime = true;
			 //   DataTable dtdrugs = new DataTable();
				connection.Open();
				SqlDataReader reader = selectCommand.ExecuteReader();

			 //   List<Trigger> TriggerValues = new List<Trigger>();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						medhist.PATIENTNO = reader["patientno"].ToString();
						medhist.NAME = reader["name"].ToString();
						medhist.TRANS_DATE = (DateTime)reader["trans_date"];
						medhist.CTIME = reader["ctime"].ToString();
						medhist.COMMENTS = reader["comments"].ToString();
						medhist.GROUPCODE = reader["groupcode"].ToString();
						medhist.DOCTOR = (reader["doctor"] == DBNull.Value) ? "" : reader["doctor"].ToString();
						medhist.PROTECTED = (Decimal)reader["protected"];
						medhist.ENCHRYPTED = (bool)reader["ENCHRYPTED"];
						//update variable
						details += medhist.TRANS_DATE.ToShortDateString() + " : @ " + medhist.CTIME + " :- " + medhist.DOCTOR.Trim() + "  (" + bissclass.seeksay("select name from doctors where rtrim(reference) = '" + medhist.DOCTOR.Trim() + "'", "MR", "name") + ") \r\n";
						//17-08-2013 for chained med history
						/*     if (bchain.MEDHISTORYCHAINED && dtrow["patientno"].ToString() != bchain.PATIENTNO)
							 {
								 txtPrev_records.Text += " ==>> CHAINED MED.HISTORY FROM " + dtrow["groupcode"].ToString().Trim() + ":" + dtrow["patientno"].ToString() + " - " + dtrow["name"].ToString() + "\n";
							 }*/
						//check for protect and enchryption - 14/10/2011,21.09.2016
						if (medhist.PROTECTED > 0 && msmrfunc.mrGlobals.nwseclevel < medhist.PROTECTED)
						{
							tmpstring = medhist.ENCHRYPTED ? "and ENCHRYPTED " : "";
							details += " *** PROTECTED " + tmpstring + " PATIENT MEDICAL HISTORY ***" + "\r\n";
							tmpstring += Enumerable.Repeat("-", 144);
							tmpstring += "\r\n";
						}
						else
						{
							details += medhist.COMMENTS.Trim() + "\r\n";

							//get drugs from prescription
							details += GetPrescriptnDetails(medhist.GROUPCODE, medhist.PATIENTNO, medhist.TRANS_DATE );

							//check for chained medic history for this date
							if (bchain != null && bchain.MEDHISTORYCHAINED && ftime ) //16.03.2019
							{
							   // if (ftime) //get chained recoreds
							   // {
									dtchained = GetChainedPatNo(bchain); //gets chained patient numbers
									details += GetChainedRecords(bchain, dtchained, medhist.TRANS_DATE); //gets medi recs 
							   // }
							   ftime = false;
							}
						}
						details += string.Concat(Enumerable.Repeat("-", 144));
						details += "\r\n";
					}
				}

				reader.Close();
				connection.Close();
			}
			catch (Exception ex)
			{
				//throw ex;
				MessageBox.Show("" + ex, "Patient Medical Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				connection.Close();
				return null;
			}
			finally
			{
				connection.Close();
			}

			return details;
		}
		static DataTable GetChainedPatNo(billchaindtl bc)
		{
			DataTable dt = Dataaccess.GetAnytable("", "MR", "select chainedgc,chainedpatno from MEDHISTCHAIN where groupcode = '" + bc.GROUPCODE + "' and patientno = '" + bc.PATIENTNO + "' ORDER BY chainedpatno", false);
			return dt;
		}
		static string GetPrescriptnDetails(string xgc, string xpat, DateTime xdate)
		{
			bool ftime = true;
			string tmpstring = "SELECT sp_inst, itemno, stk_desc, unit, qty_pr, qty_gv, cost, cdose, dose, interval, duration, cinterval, cduration, rx from dispensa where ltrim(rtrim(groupcode)) = '" + xgc.Trim() + "' and ltrim(rtrim(patientno)) = '" + xpat + "' AND TRANS_DATE >= '" + xdate.ToShortDateString() + "' and trans_date <= '" + xdate.ToShortDateString() + " 23:59:59:999' ";
			DataTable dtdrugs = Dataaccess.GetAnytable("", "MR", tmpstring, false);

			string rxspecialinstructions = "",xdtl = "";
			foreach (DataRow row in dtdrugs.Rows)
			{
				if (ftime)
				{
					xdtl += "\r\n Prescriptions : \r\n";
						//"S/N Drugs Details.......................... Unit Presc'd......  Given........ Cost...... D/ I /D" + "\r\n"; //   ..ForeColor = System.Drawing.Color.DarkGray;  //+'\n';

					rxspecialinstructions = row["sp_inst"].ToString();
				}
				ftime = false;
			   // tmpstring = row["cdose"].ToString() == "" ? row["dose"].ToString() + "x" + row["interval"].ToString() + 'x' + row["duration"].ToString() : row["cdose"].ToString() + "x" + row["cinterval"].ToString() + 'x' + row["cduration"].ToString();
				//xdtl += row["itemno"].ToString() + ".... " + row["stk_desc"].ToString() + "..... " + row["unit"].ToString() + ".... " + row["qty_pr"].ToString() + "......  " + row["qty_gv"].ToString() + ".......... " + row["cost"].ToString() + " " + tmpstring + "  (" + row["cost"].ToString() + ")" + "\r\n";
				tmpstring = row["cdose"].ToString() == "" ? row["dose"].ToString() + " x " + row["interval"].ToString() + " x " + row["duration"].ToString() : row["cdose"].ToString() + " x " + row["cinterval"].ToString() + 'x' + row["cduration"].ToString();
				xdtl += row["itemno"].ToString() + ". " + row["qty_pr"].ToString() + " " + row["unit"].ToString() + " " + row["stk_desc"].ToString() + "   Gv[" + row["qty_gv"].ToString() + "] @ " + row["cost"].ToString() + "  " + tmpstring + "\r\n"; //"  (" + row["cost"].ToString() + ")" + 
			}
			if (rxspecialinstructions != "")
				xdtl += rxspecialinstructions + "\r\n";
			return xdtl;
		}
		static string GetChainedRecords(billchaindtl bc, DataTable xdtchained, DateTime xdate)
		{
			string chainedselect = "";
			string dtl = "";
			DataRow row;
			DataTable chainedmedhist = new DataTable();
			for (int i = 0; i <  xdtchained.Rows.Count; i++)
			{
				row = xdtchained.Rows[i];
				chainedselect += "select comments, protected, enchrypted, trans_date, name, ctime, groupcode, patientno, doctor from medhist where groupcode = '" + row["chainedgc"].ToString() + "' and patientno = '" + row["chainedpatno"].ToString() + "'"; // and trans_date = '" + xdate.ToShortDateString() + "'";
				//28.01.2020 querry by date limits ability of program to return med hist of chained accounts
				if (xdtchained.Rows.Count > i + 1)
					chainedselect += " UNION ";
			}
			if (chainedselect == "")
				return dtl;

			chainedselect += " order by trans_date"; // DESC";
			chainedmedhist = Dataaccess.GetAnytable("", "MR", chainedselect, false);
			if (chainedmedhist.Rows.Count < 1)
				return dtl;
		//	bool ftime = true;
			string xname = "";
			foreach (DataRow xrow in chainedmedhist.Rows )
			{
				if (xrow["name"].ToString() != xname )
				{
					dtl += "\r\n";
					dtl += " *** ==>> CHAINED MED.HISTORY FROM > " + xrow["groupcode"].ToString().Trim() + ":" + xrow["patientno"].ToString() + " - " + xrow["name"].ToString().Trim() + "  S T A R T  \r\n";
					xname = xrow["name"].ToString();
				}
				dtl += " *** DATE : " + Convert.ToDateTime(xrow["trans_date"]).ToShortDateString() + " ***";

				dtl += "\r\n";
			//	ftime = false;
				dtl += xrow["COMMENTS"].ToString().Trim() + "\r\n";
				dtl += GetPrescriptnDetails(xrow["GROUPCODE"].ToString(), xrow["PATIENTNO"].ToString(), Convert.ToDateTime(xrow["trans_date"]).Date );
				dtl += "\r\n\r\n";
			}
			dtl += " *** END-OF-CHAINED MED. HISTORY"; // FOR : " + xdate.ToShortDateString();

			return dtl;
		}
		public static void UpdateMedHistBackup(string GroupCode, string Patientid, DateTime xtrans_date, string xcomments, string xoperator, string xreference )
		{
			SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
			SqlCommand insertCommand = new SqlCommand();
			insertCommand.CommandText = "MRB01_Add";
			insertCommand.Connection = connection;
			insertCommand.CommandType = CommandType.StoredProcedure;

			insertCommand.Parameters.AddWithValue("@reference", xreference);
			insertCommand.Parameters.AddWithValue("@patientno", Patientid);
			insertCommand.Parameters.AddWithValue("@trans_date", xtrans_date);
			insertCommand.Parameters.AddWithValue("@comments", xcomments);
			insertCommand.Parameters.AddWithValue("@groupcode", GroupCode);
			insertCommand.Parameters.AddWithValue("@operator", xoperator);
			insertCommand.Parameters.AddWithValue("@dtime", DateTime.Now);
			try
			{
				connection.Open();
				insertCommand.ExecuteNonQuery();
			}
			catch (SqlException ex)
			{
				// throw ex;
				MessageBox.Show(" " + ex, "Update Medical History backup", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				return;
			}
			finally
			{
				connection.Close();
			}
		}
		public static string GetMEDHISTBKUPCaseNotes(string GroupCodeID, string PatientID, DateTime xdatefrom, DateTime xdateto)
		{
			MedHist medhist = new MedHist();
			string details = "";
		   // DataTable dtchained = new DataTable();
			//  DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
			SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
			SqlCommand selectCommand = new SqlCommand();
			selectCommand.CommandText = "MRB01_GetByDateCaseNote";
			selectCommand.Connection = connection;
			selectCommand.CommandType = CommandType.StoredProcedure;


			selectCommand.Parameters.AddWithValue("@Groupcode", GroupCodeID);
			selectCommand.Parameters.AddWithValue("@Patientno", PatientID);
			selectCommand.Parameters.AddWithValue("@datefrom", xdatefrom);
			selectCommand.Parameters.AddWithValue("@dateto", xdateto);
			selectCommand.Parameters.AddWithValue("@sortorder", xdateto);
			try
			{
			   // string tmpstring = "";
				bool ftime = true;
				//   DataTable dtdrugs = new DataTable();
				connection.Open();
				SqlDataReader reader = selectCommand.ExecuteReader();

				//   List<Trigger> TriggerValues = new List<Trigger>();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						medhist.REFERENCE = reader["reference"].ToString();
						medhist.PATIENTNO = reader["patientno"].ToString();
						medhist.TRANS_DATE = (DateTime)reader["trans_date"];
						medhist.COMMENTS = reader["comments"].ToString();
						medhist.GROUPCODE = reader["groupcode"].ToString();
						medhist.OPERATOR = reader["operator"].ToString();
						medhist.DTIME = (DateTime)reader["dtime"];
						//update variable
						if (ftime)
						{
							details += "For Patient : " + medhist.GROUPCODE + ":" + medhist.PATIENTNO + "\r\n\r\n";
							ftime = false;
						}
						details += " By : " + medhist.OPERATOR.Trim() + " @ " + medhist.DTIME.ToString("dd/MM/yyyy hh:mm:ss") + " Ref : " + medhist.REFERENCE + " TREATMENT DATE : " + medhist.TRANS_DATE.ToShortDateString() + "\r\n";
						details += medhist.COMMENTS.Trim() + "\r\n";

							//get drugs from prescription
						 //   details += GetPrescriptnDetails(medhist.GROUPCODE, medhist.PATIENTNO, medhist.TRANS_DATE);

							//check for chained medic history for this date
 
						details += string.Concat(Enumerable.Repeat("-", 144));
						details += "\r\n";
					}
				}

				reader.Close();
				connection.Close();
			}
			catch (Exception ex)
			{
				//throw ex;
				MessageBox.Show("" + ex, "Patient Medical Backup Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				connection.Close();
				return null;
			}
			finally
			{
				connection.Close();
			}

			return details;
		}
	}
}