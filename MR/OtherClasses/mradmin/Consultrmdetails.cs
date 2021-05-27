using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using msfunc;

namespace mradmin.DataAccess
{
    public class Consultrmdetails
    {
        public string NAME { get; set; }
        public string LOCATION { get; set; }
        public string DOCTOR { get; set; }
        public DateTime LOGIN_DATETIME { get; set; }
        public DateTime LOGOUT_DATETIME { get; set; }
        public decimal FREQUENCY { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool OCCUPIED { get; set; }

        public static DataTable GetCONSULTRMDETAILS(DateTime xdatefrom, DateTime xdateto)       
        {
            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            Consultrmdetails consrm = new Consultrmdetails();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = (xdatefrom == dtmin_date ) ? "CONSULTRMDETAILS_Get" : "CONSULTRMDETAILS_GetList"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            //   selectCommand.Parameters.AddWithValue("@GroupCodeID", GroupCodeID);
            //   selectCommand.Parameters.AddWithValue("@PatientID", PatientID);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }

        public static bool UpdateConsultRMdetails(string xNAME, string xLOCATION, string xDOCTOR,
            DateTime xLOGIN_DATETIME, DateTime xLOGOUT_DATETIME, decimal xFREQUENCY, bool xPOSTED, DateTime xPOST_DATE,
            bool xOCCUPIED,bool toadd )
        {
            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (toadd) ? "CONSULTRMDETAILS_Add" : "CONSULTRMDETAILS_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@name", xNAME);
            insertCommand.Parameters.AddWithValue("@location", xLOCATION);
            insertCommand.Parameters.AddWithValue("@Doctor", xDOCTOR);
            insertCommand.Parameters.AddWithValue("@login_datetime", xLOGIN_DATETIME);
            insertCommand.Parameters.AddWithValue("@logout_datetime", xLOGOUT_DATETIME);
            insertCommand.Parameters.AddWithValue("@frequency", xFREQUENCY);
            insertCommand.Parameters.AddWithValue("@posted", xPOSTED);
            insertCommand.Parameters.AddWithValue("@post_date", xPOST_DATE);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show(" " + ex, "Consult Rooms Details", MessageBoxButtons.OK,
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