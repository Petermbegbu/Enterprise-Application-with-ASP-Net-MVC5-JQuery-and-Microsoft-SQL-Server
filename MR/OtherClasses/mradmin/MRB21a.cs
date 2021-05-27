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
    public class MRB21a
    {
        public string NAME { get; set; }
        public string SENDER { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string NOTES { get; set; }
        public string RECEIVED { get; set; }
        public string SENDSECTION { get; set; }
        public bool POSTED { get; set; }
        public string TOSECTION { get; set; }
        public decimal REPLAYAT { get; set; }
        public decimal REPLAYDAYS { get; set; }
        public bool resultalert { get; set; }

        public static MRB21a GetMRB21A(string GroupCodeID, string PatientID)
        {
            MRB21a mrb21a = new MRB21a();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "MRB21A_Get"; //"spGetPatient";
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
                    mrb21a.NAME = reader["name"].ToString();
                    mrb21a.SENDER = reader["sender"].ToString();
                    mrb21a.TRANS_DATE = (DateTime)reader["trans_date"];
                    mrb21a.NOTES = reader["notes"].ToString();
                    mrb21a.RECEIVED = reader["received"].ToString();
                    mrb21a.SENDSECTION = reader["sendsection"].ToString();
                    mrb21a.POSTED = (bool)reader["posted"];
                    mrb21a.TOSECTION = reader["tosection"].ToString();
                    mrb21a.REPLAYAT = (Decimal)reader["replayat"];
                    mrb21a.REPLAYDAYS = (Decimal)reader["replaydays"];
                    mrb21a.resultalert = (bool)reader["resultalert"];

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
                MessageBox.Show("" + ex, "Get Alert Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return mrb21a;
        }
        public static bool WriteAlertDetails(string xname, string xsender, DateTime xtrans_date, string xnotes, string xreceived, string xsendsection, bool xposted, string xtosection, decimal xreplayat, decimal xreplaydays, bool isresultalrt)
        {
            //DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "MRB21A_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@name", xname.ToString());
            insertCommand.Parameters.AddWithValue("@sender", xsender.ToString());
            insertCommand.Parameters.AddWithValue("@trans_date", xtrans_date );
            insertCommand.Parameters.AddWithValue("@notes", xnotes.ToString());
            insertCommand.Parameters.AddWithValue("@posted", false);
            insertCommand.Parameters.AddWithValue("@received", xreceived);
            insertCommand.Parameters.AddWithValue("@sendsection", xsendsection.ToString());
            insertCommand.Parameters.AddWithValue("@tosection", xtosection.ToString());
            insertCommand.Parameters.AddWithValue("@replayat", xreplayat);
            insertCommand.Parameters.AddWithValue("@replaydays", xreplaydays);
            insertCommand.Parameters.AddWithValue("@resultalert", isresultalrt);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show(" " + ex, "Add Alert Details", MessageBoxButtons.OK,
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