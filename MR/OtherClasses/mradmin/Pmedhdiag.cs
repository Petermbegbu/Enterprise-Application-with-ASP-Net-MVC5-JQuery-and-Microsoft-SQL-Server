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
    public class Pmedhdiag
    {
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string PROVISIONAL { get; set; }
        public string FINAL { get; set; }
        public string TRANSTYPE { get; set; }
        public string REFERENCE { get; set; }
    

    public static Pmedhdiag GetPMEDHDIAG(string GroupCodeID, string PatientID, DateTime xtrans_date)
        {
            Pmedhdiag pmedh = new Pmedhdiag();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "PMEDHDIAG_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@GroupCode", GroupCodeID);
            selectCommand.Parameters.AddWithValue("@Patientno", PatientID);
            selectCommand.Parameters.AddWithValue("@Trans_date", xtrans_date);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    
                    pmedh.PATIENTNO = reader["patientno"].ToString();
                    pmedh.GROUPCODE = reader["groupcode"].ToString();
                    pmedh.TRANS_DATE = (DateTime)reader["trans_date"];
                    pmedh.PROVISIONAL = reader["provisional"].ToString();
                    pmedh.FINAL = reader["final"].ToString();
                    pmedh.TRANSTYPE = reader["transtype"].ToString();
                    pmedh.REFERENCE = reader["reference"].ToString();
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
                MessageBox.Show("" + ex, "Get Patient Diagnosis Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return pmedh;
        }
    }
}