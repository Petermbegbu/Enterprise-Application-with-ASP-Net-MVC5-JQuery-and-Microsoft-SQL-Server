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
    public class Medhrec
    {
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public string CLINIC { get; set; }
        public DateTime DATE1 { get; set; }
        public DateTime DATE2 { get; set; }
        public DateTime DATE3 { get; set; }
        public DateTime DATE4 { get; set; }
        public DateTime DATE5 { get; set; }
        public DateTime DATE6 { get; set; }
        public DateTime DATE7 { get; set; }
        public string CLINIC1 { get; set; }
        public string CLINIC2 { get; set; }
        public string CLINIC3 { get; set; }
        public string CLINIC4 { get; set; }
        public string CLINIC5 { get; set; }
        public string CLINIC6 { get; set; }
        public string CLINIC7 { get; set; }
        public bool FOLLOWUP { get; set; }


        public static Medhrec GetMEDHREC(string groupcode, string PatientID)
        {
            Medhrec medhrecs = new Medhrec();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "MEDHREC_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@PatientNo", PatientID);
            selectCommand.Parameters.AddWithValue("@Groupcode", groupcode);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    medhrecs.PATIENTNO = reader["patientno"].ToString();
                    medhrecs.GROUPCODE = reader["groupcode"].ToString();
                    medhrecs.CLINIC = reader["clinic"].ToString();
                    medhrecs.DATE1 = (DateTime)reader["date1"];
                    medhrecs.DATE2 = (DateTime)reader["date2"];
                    medhrecs.DATE3 = (DateTime)reader["date3"];
                    medhrecs.DATE4 = (DateTime)reader["date4"];
                    medhrecs.DATE5 = (DateTime)reader["date5"];
                    medhrecs.DATE6 = (DateTime)reader["date6"];
                    medhrecs.DATE7 = (DateTime)reader["date7"];
                    medhrecs.CLINIC1 = reader["clinic1"].ToString();
                    medhrecs.CLINIC2 = reader["clinic2"].ToString();
                    medhrecs.CLINIC3 = reader["clinic3"].ToString();
                    medhrecs.CLINIC4 = reader["clinic4"].ToString();
                    medhrecs.CLINIC5 = reader["clinic5"].ToString();
                    medhrecs.CLINIC6 = reader["clinic6"].ToString();
                    medhrecs.CLINIC7 = reader["clinic7"].ToString();
                    medhrecs.FOLLOWUP = (bool)reader["followup"];
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
                MessageBox.Show("" + ex, "Get Patient Medhrec Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return medhrecs;
        }


    }

}