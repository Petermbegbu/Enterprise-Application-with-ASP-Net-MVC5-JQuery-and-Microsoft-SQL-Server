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
    public class dgprofile
    {
        public string REFERENCE { get; set; }
        public string DRUGS { get; set; }
        public decimal AMOUNT { get; set; }
        public string OTHERS { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool CAPITATED { get; set; }
        public bool AUTHORIZATIONREQUIRED { get; set; }


        public static dgprofile GetDGPROFILE(string xpatcateg, string xdrug)
        {
            dgprofile dgprof = new dgprofile();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "DGPROFILE_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@reference", xpatcateg);
            selectCommand.Parameters.AddWithValue("@drugs", xdrug);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    dgprof.REFERENCE = reader["reference"].ToString();
                    dgprof.DRUGS = reader["drugs"].ToString();
                    dgprof.AMOUNT = (Decimal)reader["amount"];
                    dgprof.OTHERS = reader["others"].ToString();
                    dgprof.POSTED = (bool)reader["posted"];
                    dgprof.POST_DATE = (DateTime)reader["post_date"];
                    dgprof.CAPITATED = (bool)reader["capitated"];
                    dgprof.AUTHORIZATIONREQUIRED = (bool)reader["authorizationrequired"];

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
                MessageBox.Show("" + ex, "Get Patient Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return dgprof;
        }

    }
}