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
    public class IMUNES
    {
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public string PARENT { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public string PROCESS { get; set; }
        public DateTime DATEDUE { get; set; }
        public DateTime DATETAKEN { get; set; }
        public decimal WEIGHT { get; set; }
        public decimal HEIGHT { get; set; }
        public string TEMP { get; set; }
        public string REMARKS { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public decimal AGE { get; set; }
        public decimal AMOUNT { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string NEXTPROCEDURE { get; set; }
        public decimal NEXTDUEDAY { get; set; }
        public DateTime NEXTDUEDATE { get; set; }
        public string REACTIONS { get; set; }
        public string SITEOFADMIN { get; set; }
        public string ROUTEOFADMIN { get; set; }
        public string SIGNATURE { get; set; }
        public string BATCHNO { get; set; }
        public string AGEGROUP { get; set; }
        public decimal HEADCIRCUMF { get; set; }
        public string NEXTDUE { get; set; }
        public string PROCESSDESC { get; set; }

        public static DataTable GetImmunizationDetials(string groupcode,string patientno)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "IMUNES_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Groupcode", groupcode);
            selectCommand.Parameters.AddWithValue("@Patientno", patientno);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        /// <summary>
        /// Delete bill using reference, process and patientid.
        /// </summary>
        public static bool DeleteImmunizationItem(string groupcode, string patientno, string process)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "IMMUNES_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Groupcode", groupcode);
            deleteStatement.Parameters.AddWithValue("@Patientno", patientno);
            deleteStatement.Parameters.AddWithValue("@Process", process);
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

                MessageBox.Show(" " + ex, "Delete Immunization Item", MessageBoxButtons.OK,
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