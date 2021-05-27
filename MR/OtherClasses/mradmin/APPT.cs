using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using msfunc;
//using ANC.DataAccess;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

namespace mradmin.DataAccess
{
    public class APPT
    {
        public string USERID { get; set; }
        public DateTime DATE { get; set; }
        public string TIME { get; set; }
        public string ENDTIME { get; set; }
        public string BRIEF { get; set; }
        public string COMMENTS { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string FACILITY { get; set; }
        public string ROOM { get; set; }
        public string BED { get; set; }
        public string PROCESS { get; set; }
        public bool ISNOTE { get; set; }
        public string PURPOSE { get; set; }


        public static DataTable GetAPPT(DateTime apptmtdate)
        {
           // APPT appt = new APPT();

            SqlConnection connection = new SqlConnection(); connection =  Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "APPT_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@DATE", apptmtdate);
            //selectCommand.Parameters.AddWithValue("@PatientID", PatientID);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static bool writeApptment(bool newrec, DateTime date, string time, string endtime, string facility, string room, string bed, string process, bool isnote, string[] schedulea_ )
        {
           // DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.codeConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "APPT_Add" : "APPT_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@userid", schedulea_[0] );
            insertCommand.Parameters.AddWithValue("@date",date );
            insertCommand.Parameters.AddWithValue("@time",time );
            insertCommand.Parameters.AddWithValue("@endtime",endtime );
            insertCommand.Parameters.AddWithValue("@brief", schedulea_[2] );
            insertCommand.Parameters.AddWithValue("@comments", schedulea_[1]);
            insertCommand.Parameters.AddWithValue("@posted",false );
            insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@facility",facility );
            insertCommand.Parameters.AddWithValue("@room",room );
            insertCommand.Parameters.AddWithValue("@bed",bed );
            insertCommand.Parameters.AddWithValue("@process",process );
            insertCommand.Parameters.AddWithValue("@isnote",isnote );
            insertCommand.Parameters.AddWithValue("@USERID_PHONE", schedulea_[3]);
        	insertCommand.Parameters.AddWithValue("@USERID_EMAIL", schedulea_[4]);
	        insertCommand.Parameters.AddWithValue("@BENEFICIARY", schedulea_[5]);
	        insertCommand.Parameters.AddWithValue("@BENEFICIARY_PHONE", schedulea_[6]);
            insertCommand.Parameters.AddWithValue("@BENEFICIARY_EMAIL", schedulea_[7]);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL access" + ex, "APPTMT UPDATE", MessageBoxButtons.OK,
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