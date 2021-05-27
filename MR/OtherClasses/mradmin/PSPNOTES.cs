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
    public class PSPNOTES
    {
       public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string SPNOTES { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string MEDNOTES { get; set; }
     public static PSPNOTES GetPSPNOTES(string GroupCodeID, string PatientID)
        {
            PSPNOTES pspnotes = new PSPNOTES();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "PSPNOTES_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@GroupCode", GroupCodeID);
            selectCommand.Parameters.AddWithValue("@Patientno", PatientID);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    pspnotes.GROUPCODE = reader["groupcode"].ToString();
                    pspnotes.PATIENTNO = reader["patientno"].ToString();
                    pspnotes.SPNOTES = reader["spnotes"].ToString();
                    pspnotes.POSTED = (bool)reader["posted"];
                    pspnotes.POST_DATE = (DateTime)reader["post_date"];
                    pspnotes.MEDNOTES = reader["mednotes"].ToString();                                                                  
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
                MessageBox.Show("" + ex, "Get Allergies/Special Instructions Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }
            return pspnotes;
        }
    }
}