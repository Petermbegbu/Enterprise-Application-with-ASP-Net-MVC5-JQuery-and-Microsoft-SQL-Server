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
    public class PGROWTH
    {
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public DateTime DATETAKEN { get; set; }
        public decimal WEIGHT { get; set; }
        public decimal HEIGHT { get; set; }
        public string TEMP { get; set; }
        public string REMARKS { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string AGE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string OFC { get; set; }
        public string NUTRITION { get; set; }
        public string COMMENTS { get; set; }

        public static DataTable GetPgrowth(string groupcode,string patientno)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "PGROWTH_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static bool DeletePgrowth(string groupcode,string patientno,string age, DateTime datetaken)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "PGROWTH_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@groupcode", groupcode);
            deleteStatement.Parameters.AddWithValue("@patientno", patientno);
            deleteStatement.Parameters.AddWithValue("@age", age);
            deleteStatement.Parameters.AddWithValue("@datetaken", datetaken);
            try
            {
                connection.Open();
                int count = deleteStatement.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException ex)
            {
                //throw ex;

                MessageBox.Show(" " + ex, "Delete Child Growth Record", MessageBoxButtons.OK,
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