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
    public class Cdmdetails
    {
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public decimal DRUGS { get; set; }
        public string DIAGNOSIS { get; set; }
        public decimal FREQUENCY { get; set; }
        public decimal QTY { get; set; }
        public DateTime LASTCOLLECTION { get; set; }
        public string DISPENSEDBY { get; set; }
        public DateTime DISP_DATETIME { get; set; }
        public string LASTVISITDOC { get; set; }
        public DateTime VISIT_DATETIME { get; set; }
        public string COMMENTS { get; set; }
        public string CLINIC { get; set; }
        public string GROUPHEAD { get; set; }

        public static Cdmdetails Getcdmdetails(string xgroupcode, string patientid)
        {
            Cdmdetails cdmdetails = new Cdmdetails();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "CDMDETAILS_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Groupcode", xgroupcode);
            selectCommand.Parameters.AddWithValue("@Patientno", patientid);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    cdmdetails.GROUPHEAD = reader["grouphead"].ToString();
                    cdmdetails.PATIENTNO = reader["patientno"].ToString();
                    cdmdetails.NAME = reader["name"].ToString();
                    cdmdetails.DRUGS = (Decimal)reader["drugs"];
                    cdmdetails.DIAGNOSIS = reader["diagnosis"].ToString();
                    cdmdetails.FREQUENCY = (Decimal)reader["frequency"];
                    cdmdetails.QTY = (Decimal)reader["qty"];
                    cdmdetails.LASTCOLLECTION = (DateTime)reader["lastcollection"];
                    cdmdetails.DISPENSEDBY  = reader["dispensedby"].ToString();
                    cdmdetails.DISP_DATETIME = (DateTime)reader["disp_datetime"];
                    cdmdetails.LASTVISITDOC = reader["lastvisitdoc"].ToString();
                    cdmdetails.VISIT_DATETIME = (DateTime)reader["visit_datetime"];
                    cdmdetails.COMMENTS = reader["posted"].ToString();
                    cdmdetails.CLINIC  = reader["clinic"].ToString();
                    cdmdetails.GROUPHEAD = reader["grouphead"].ToString();

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
                MessageBox.Show("" + ex, "Chronic Disease Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return cdmdetails;
        }

    }
}