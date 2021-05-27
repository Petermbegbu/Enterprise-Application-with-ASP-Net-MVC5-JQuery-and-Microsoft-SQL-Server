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
    public class LINK3
    {
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string SECTION { get; set; }
        public string TIMESENT { get; set; }
        public string TIMEIN { get; set; }
        public string TIMESELECTED { get; set; }
        public string REFERENCE { get; set; }
        public string OPERATOR { get; set; }
        public string FACILITY { get; set; }
        public string TIMESPENT { get; set; }
        public string RECTYPE { get; set; }



        public static LINK3 GetLINK3(string GroupCodeID, string PatientID)
        {
            LINK3 link3 = new LINK3();
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "LINK3_Get"; //"spGetPatient";
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
                    link3.GROUPCODE = reader["groupcode"].ToString();
                    link3.PATIENTNO = reader["patientno"].ToString();
                    link3.NAME = reader["name"].ToString();
                    link3.TRANS_DATE = (DateTime)reader["trans_date"];
                    link3.POSTED = (bool)reader["posted"];
                    link3.POST_DATE = (DateTime)reader["post_date"];
                    link3.SECTION = reader["section"].ToString();
                    link3.TIMESENT = reader["timesent"].ToString();
                    link3.TIMEIN = reader["timein"].ToString();
                    link3.TIMESELECTED = reader["timeselected"].ToString();
                    link3.REFERENCE = reader["reference"].ToString();
                    link3.OPERATOR = reader["operator"].ToString();
                    link3.FACILITY = reader["facility"].ToString();
                    link3.TIMESPENT = reader["timespent"].ToString();
                    link3.RECTYPE = reader["rectype"].ToString();

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
                MessageBox.Show("" + ex, "Get Attendance Monitor Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return link3;
        }

        public static bool WriteLINK3(string GroupCodeID, string PatientID,DateTime xdate, string patname, string xsection, string xreference, string xtimesent, string xselected, string xrectype,string xfacility,
            string xtimein, string xoperator)

        {
            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "LINK3_Add";
            insertCommand.Connection = connection; //00:00:00
            insertCommand.CommandType = CommandType.StoredProcedure;
            int hours = Convert.ToInt32(xtimein.Substring(0,2)), minutes = Convert.ToInt32(xtimein.Substring(3,2)),
                seconds = Convert.ToInt32(xtimein.Substring(6,2));
            TimeSpan T1 = new TimeSpan(hours, minutes, seconds);
        /*    string timenow = DateTime.Now.ToLongTimeString();
            string spent = DateTime.Now.Subtract(T1).ToShortTimeString();*/
            string spent1 = DateTime.Now.Subtract(T1).ToLongTimeString();
            insertCommand.Parameters.AddWithValue("@groupcode",GroupCodeID ); 
            insertCommand.Parameters.AddWithValue("@patientno",PatientID );
            insertCommand.Parameters.AddWithValue("@name",patname );
            insertCommand.Parameters.AddWithValue("@trans_date",xdate );
            insertCommand.Parameters.AddWithValue("@posted",false );
            insertCommand.Parameters.AddWithValue("@post_date",dtmin_date ); 
            insertCommand.Parameters.AddWithValue("@section",xsection ); 
            insertCommand.Parameters.AddWithValue("@timesent",xtimesent ); 
            insertCommand.Parameters.AddWithValue("@timein",xtimein );
            insertCommand.Parameters.AddWithValue("@timeselected",xselected ); 
            insertCommand.Parameters.AddWithValue("@reference",xreference );
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@facility",xfacility );
            insertCommand.Parameters.AddWithValue("@timespent", spent1);
            insertCommand.Parameters.AddWithValue("@rectype",xrectype );


            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show(" " + ex, "Add Attendance Monitor Details", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;

        }

    }
}