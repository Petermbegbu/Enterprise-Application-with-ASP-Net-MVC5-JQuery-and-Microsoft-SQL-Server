using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using msfunc;
using mradmin.BissClass;

namespace mradmin.DataAccess
{
    
    public class ANCREG
    {
        public string REFERENCE { get; set; }
        public string PATIENTNO { get; set; }
        public string GROUPHEAD { get; set; }
        public string GHGROUPCODE { get; set; }
        public string GROUPCODE { get; set; }
        public string GROUPHTYPE { get; set; }
        public string NAME { get; set; }
        public DateTime LMP { get; set; }
        public DateTime EDD { get; set; }
        public DateTime DEL_DATE { get; set; }
        public decimal CUMMCHARGE { get; set; }
        public decimal PAYMENTS { get; set; }
        public decimal CHARGE { get; set; }
        public DateTime LASTATTEND { get; set; }
        public decimal CUMMATTEND { get; set; }
        public string DRUG1 { get; set; }
        public string DRUG2 { get; set; }
        public string DRUG3 { get; set; }
        public string DRUG4 { get; set; }
        public string DRUG5 { get; set; }
        public string DRUG6 { get; set; }
        public string DRUG7 { get; set; }
        public string DRUG8 { get; set; }
        public string DRUG9 { get; set; }
        public string DRUG10 { get; set; }
        public bool ALLDRUGS { get; set; }
        public string SERVICE1 { get; set; }
        public string SERVICE2 { get; set; }
        public string SERVICE3 { get; set; }
        public string SERVICE4 { get; set; }
        public string SERVICE5 { get; set; }
        public string SERVICE6 { get; set; }
        public string SERVICE7 { get; set; }
        public string SERVICE8 { get; set; }
        public string SERVICE9 { get; set; }
        public string SERVICE10 { get; set; }
        public bool ALLSERVICE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public DateTime REG_DATE { get; set; }
        public string REG_TIME { get; set; }
        public string ANCHISTORY { get; set; }
        public string ANTENATALNOTES { get; set; }
        public string DELIVERYNOTES { get; set; }
        public string POSTNATALNOTES { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public bool EVERYVISITCONSULT { get; set; }
        public decimal CONSULTAMT { get; set; }
  /// <summary>
  /// Returns ancreg details by ANC Registration Reference
  /// </summary>
  /// <param name="xreference"></param>
  /// <returns></returns>
        public static ANCREG GetANCREG(string xreference) //,string GroupCode, string PatientID)
        {
            ANCREG ancreg = new ANCREG();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANCREG_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

          //  selectCommand.Parameters.AddWithValue("@GroupCodeID", GroupCodeID);
          //  selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            selectCommand.Parameters.AddWithValue("@Reference", xreference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    ancreg.REFERENCE = reader["reference"].ToString();
                    ancreg.PATIENTNO = reader["patientno"].ToString();
                    ancreg.GROUPHEAD = reader["grouphead"].ToString();
                    ancreg.GHGROUPCODE = reader["ghgroupcode"].ToString();
                    ancreg.GROUPCODE = reader["groupcode"].ToString();
                    ancreg.GROUPHTYPE = reader["grouphtype"].ToString();
                    ancreg.NAME = reader["name"].ToString();
                    ancreg.LMP = (DateTime)reader["lmp"];
                    ancreg.EDD = (DateTime)reader["edd"];
                    ancreg.DEL_DATE = (DateTime)reader["del_date"];
                    ancreg.CUMMCHARGE = (Decimal)reader["cummcharge"];
                    ancreg.PAYMENTS = (Decimal)reader["payments"];
                    ancreg.CHARGE = (Decimal)reader["charge"];
                    ancreg.LASTATTEND = (DateTime)reader["lastattend"];
                    ancreg.CUMMATTEND = (reader["cummattend"] == DBNull.Value) ? 0m : Convert.ToDecimal(reader["cummattend"]);
                    ancreg.DRUG1 = reader["drug1"].ToString();
                    ancreg.DRUG2 = reader["drug2"].ToString();
                    ancreg.DRUG3 = reader["drug3"].ToString();
                    ancreg.DRUG4 = reader["drug4"].ToString();
                    ancreg.DRUG5 = reader["drug5"].ToString();
                    ancreg.DRUG6 = reader["drug6"].ToString();
                    ancreg.DRUG7 = reader["drug7"].ToString();
                    ancreg.DRUG8 = reader["drug8"].ToString();
                    ancreg.DRUG9 = reader["drug9"].ToString();
                    ancreg.DRUG10 = reader["drug10"].ToString();
                    ancreg.ALLDRUGS = (bool)reader["alldrugs"];
                    ancreg.SERVICE1 = reader["service1"].ToString();
                    ancreg.SERVICE2 = reader["service2"].ToString();
                    ancreg.SERVICE3 = reader["service3"].ToString();
                    ancreg.SERVICE4 = reader["service4"].ToString();
                    ancreg.SERVICE5 = reader["service5"].ToString();
                    ancreg.SERVICE6 = reader["service6"].ToString();
                    ancreg.SERVICE7 = reader["service7"].ToString();
                    ancreg.SERVICE8 = reader["service8"].ToString();
                    ancreg.SERVICE9 = reader["service9"].ToString();
                    ancreg.SERVICE10 = reader["service10"].ToString();
                    ancreg.ALLSERVICE = (bool)reader["allservice"];
                    ancreg.POSTED = (bool)reader["posted"];
                    ancreg.POST_DATE = (DateTime)reader["post_date"];
                    ancreg.TRANS_DATE = (DateTime)reader["trans_date"];
                    ancreg.REG_DATE = (DateTime)reader["reg_date"];
                    ancreg.REG_TIME = reader["reg_time"].ToString();
                    ancreg.ANCHISTORY = reader["anchistory"].ToString();
                    ancreg.ANTENATALNOTES = reader["antenatalnotes"].ToString();
                    ancreg.DELIVERYNOTES = reader["deliverynotes"].ToString();
                    ancreg.POSTNATALNOTES = reader["postnatalnotes"].ToString();
                    ancreg.OPERATOR = reader["operator"].ToString();
                    ancreg.DTIME = (DateTime)reader["dtime"];
                    ancreg.EVERYVISITCONSULT = (bool)reader["everyvisitconsult"];
                    ancreg.CONSULTAMT = (Decimal)reader["consultamt"];
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
                MessageBox.Show("" + ex, "Get ANC Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }
            return ancreg;
        }
        /// <summary>
        /// Checks for Current ANC registration for this patient using Del_Date - 
        /// At daily attendance and Doctors Consulting Platforms. TRANSTYPE param: D-check for Drugs allowed : 
        /// S-services or procedure allowed. RtnText carries LMP,EDD, etc. rtnreference = row.Columns["reference"].ToString()
        /// </summary>
        /// <param name="patientno"></param>
        /// <param name="transtype"></param>
        /// <param name="compareString"></param>
        /// <param name="amount"></param>
        /// <param name="rtntext"></param>
        /// <param name="everyvisitconsult"></param>
        /// <param name="everyvisitconsultamt"></param>
        /// <param name="rtnreference"></param>
        /// <returns></returns>
        public static bool GetANCREG(string patientno,string groupcode,string transtype,string compareString,ref decimal amount,ref string rtntext, ref bool everyvisitconsult,ref decimal everyvisitconsultamt,ref string rtnreference) //string GroupCodeID, string PatientID)
        {
            // xgc,xpat,xflag,xval,xamt,xctext,xconsultaton,xconsultamt)
            bool rtnval = false;
            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start.Year < 2000 ? bissclass.ConvertStringToDateTime("01", "01", "2000") : msmrfunc.mrGlobals.mta_start;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANCREG_GetByPatient"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Groupcode", groupcode);
            selectCommand.Parameters.AddWithValue("@Patientno", patientno);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            if (dt.Rows.Count > 0)
            {
                string drg,serv;
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToDateTime(row["del_date"]) <= dtmin_date)
                    {
                        rtnval = true;
                        everyvisitconsult = Convert.ToBoolean(row["everyvisitconsult"]);
                        everyvisitconsultamt = Convert.ToDecimal(row["consultamt"]);
                        rtntext = "  LMP : " + Convert.ToDateTime(row["LMP"]) + " []  EDD :" + Convert.ToDateTime(row["edd"]);
                        rtnreference = row["reference"].ToString();
                        if (transtype == "D" || transtype == "S") //drug or services
                            compareString = compareString.Trim();
                        if (transtype == "D" && Convert.ToBoolean(row["alldrugs"]) || transtype == "S"
                            && Convert.ToBoolean(row["allservice"])) //patient is exempted from paying
                            amount = 0m;
                        else
                        {
                            for (int i = 1; i < 11; i++)
                            {
                                drg = "drug" + i.ToString().Trim();
                                serv = "service" + i.ToString().Trim();
                                if (transtype == "S" && compareString.Trim() == row[serv].ToString() ||
                                    transtype == "D" && compareString.Trim() == row[drg].ToString())
                                {
                                    amount = 0m;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return rtnval;
        }
        /// <summary>
        /// Checks for all registration for specified patient and scan for empty del_date.  This is to prevent multiple ANC registeration.
        /// Returns True if found with empty delivery date
        /// </summary>
        /// <param name="patientno"></param>
        /// <param name="rtntext"></param>
        /// <returns></returns>
        public static bool GetANCREG(string patientno, string groupcode, ref string rtntext, ref string rtnancref)
        {
            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANCREG_GetByPatient"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Groupcode", groupcode);
            selectCommand.Parameters.AddWithValue("@Patientno", patientno);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            bool xfoundit = false;
            if (dt.Rows.Count > 0 )
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(dt.Rows[i]["del_date"]).Date <= dtmin_date)
                    {
                        xfoundit = true;
                        rtntext =  dt.Rows[i]["reference"].ToString().Trim()+" of "+
                            Convert.ToDateTime(dt.Rows[i]["REG_DATE"]).ToString("dd-MM-yyyy @ HH:ss");
                        rtnancref = dt.Rows[i]["reference"].ToString();
                    }
                }
            }
            return xfoundit;
        }

        public static bool DeleteANCREG(string Reference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "Ancreg_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Reference", Reference);
            try
            {
                connection.Open();
                int count = deleteStatement.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch //(SqlException ex)
            {
                //throw ex;

                MessageBox.Show("Unable to Open SQL Server Database Table", "Delete ANC DETAILS", MessageBoxButtons.OK,
    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }

        }

    }
}