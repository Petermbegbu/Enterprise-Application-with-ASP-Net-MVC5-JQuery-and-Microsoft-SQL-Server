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
    public class LINK
    {
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string FRSECTION { get; set; }
        public string TIMESENT { get; set; }
        public string TOSECTION { get; set; }
        public string DATEREC { get; set; }
        public string TIMEREC { get; set; }
        public string REFERENCE { get; set; }
        public decimal CUMBIL { get; set; }
        public decimal CUMPAY { get; set; }
        public string OPERATOR { get; set; }
        public string FACILITY { get; set; }
        public bool LINKOK { get; set; }
        public decimal PROCFUNC { get; set; }
        public string DOCTOR { get; set; }
        public bool SENDEXCL { get; set; }
        public string TRANSFLAG { get; set; }


        public static LINK GetLINK(string GroupCodeID, string PatientID)
        {
            LINK link = new LINK();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "LINK_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@GroupCodeID", GroupCodeID);
            selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    link.GROUPCODE = reader["groupcode"].ToString();
                    link.PATIENTNO = reader["patientno"].ToString();
                    link.NAME = reader["name"].ToString();
                    link.TRANS_DATE = (DateTime)reader["trans_date"];
                    link.POSTED = (bool)reader["posted"];
                    link.POST_DATE = (DateTime)reader["post_date"];
                    link.FRSECTION = reader["frsection"].ToString();
                    link.TIMESENT = reader["timesent"].ToString();
                    link.TOSECTION = reader["tosection"].ToString();
                    link.DATEREC = reader["daterec"].ToString();
                    link.TIMEREC = reader["timerec"].ToString();
                    link.REFERENCE = reader["reference"].ToString();
                    link.CUMBIL = (Decimal)reader["cumbil"];
                    link.CUMPAY = (Decimal)reader["cumpay"];
                    link.OPERATOR = reader["operator"].ToString();
                    link.FACILITY = reader["facility"].ToString();
                    link.LINKOK = (bool)reader["linkok"];
                    link.PROCFUNC = (Decimal)reader["procfunc"];
                    link.DOCTOR = reader["doctor"].ToString();
                    link.SENDEXCL = (bool)reader["sendexcl"];
                    link.TRANSFLAG = reader["transflag"].ToString();


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
                MessageBox.Show("" + ex, "Get Link Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return link;
        }
        public static bool WriteLINK(int recrec, string GroupCodeID, string PatientID, string patname, string xtosection, string xreference, decimal xcumbil, decimal xcumpay, string xclinic, bool xlinkok, string xdoctor, bool xclusive, int xprocfunc, string xflag, string xfrmsection,string woperator)
        {
            DateTime dtmin_date = BissClass.msmrfunc.mrGlobals.mta_start; 
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "LINK_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@groupcode", GroupCodeID);
            insertCommand.Parameters.AddWithValue("@patientno", PatientID);
            insertCommand.Parameters.AddWithValue("@name", patname);
            insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
            insertCommand.Parameters.AddWithValue("@posted", false);
            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date.Date );
            insertCommand.Parameters.AddWithValue("@frsection", xfrmsection);
            insertCommand.Parameters.AddWithValue("@timesent", DateTime.Now.ToLongTimeString());
            insertCommand.Parameters.AddWithValue("@tosection", xtosection);
            insertCommand.Parameters.AddWithValue("@daterec", "" );
            insertCommand.Parameters.AddWithValue("@timerec", "");
            insertCommand.Parameters.AddWithValue("@reference", xreference);
            insertCommand.Parameters.AddWithValue("@cumbil", xcumbil);
            insertCommand.Parameters.AddWithValue("@cumpay", xcumpay);
            insertCommand.Parameters.AddWithValue("@operator", woperator );
            insertCommand.Parameters.AddWithValue("@facility", xclinic);
            insertCommand.Parameters.AddWithValue("@linkok", xlinkok);
            insertCommand.Parameters.AddWithValue("@procfunc", xprocfunc);
            insertCommand.Parameters.AddWithValue("@doctor", xdoctor);
            insertCommand.Parameters.AddWithValue("@sendexcl", xclusive);
            insertCommand.Parameters.AddWithValue("@transflag", xflag);


            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("Unable to Open SQL Server Database Table" + ex, "Add Customer Details", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        /// <summary>
        /// Updates LinkOk for pharmacy on cash paying patients
        /// and Date/Time Received
        /// </summary>
        /// <param name="xreference"></param>
        /// <param name="tosection"></param>
        /// <param name="transdate"></param>
        public static void updateLinkOk(string xreference, string tosection, DateTime transdate, string daterec, string timerec, bool islinkupdate, bool linkok, int recid)
        {
            //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;  (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "Link_UpdateLinkOk";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Trans_date", transdate);
            insertCommand.Parameters.AddWithValue("@Tosection", tosection);
            insertCommand.Parameters.AddWithValue("@Daterec", daterec);
            insertCommand.Parameters.AddWithValue("@Timerec", timerec);
            insertCommand.Parameters.AddWithValue("@Reference", xreference);
            insertCommand.Parameters.AddWithValue("@LinkOk", linkok);
            insertCommand.Parameters.AddWithValue("@Linkupdate", islinkupdate);
            insertCommand.Parameters.AddWithValue("@recid", recid );
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL access" + ex, "LINK UPDATE", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// get all link records for a particular reference
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static DataTable GetLINK(string reference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "LINK_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
            //selectCommand.Parameters.AddWithValue("@PatientID", PatientID);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static void ClearLink(string xreference, string tosection, string daterec, string timerec)
        {
            //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;  (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "Link_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Tosection", tosection);
            insertCommand.Parameters.AddWithValue("@Daterec", daterec);
            insertCommand.Parameters.AddWithValue("@Timerec", timerec);
            insertCommand.Parameters.AddWithValue("@Reference", xreference);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL access" + ex, "LINK UPDATE", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}