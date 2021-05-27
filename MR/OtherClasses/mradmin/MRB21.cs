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
    public class MRB21
    {
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public string SENDER { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string FACILITY { get; set; }
        public string NOTES { get; set; }
        public string RECEIVED { get; set; }
        public string OPERATOR { get; set; }
        public string REFERENCE { get; set; }
        public string SENDSECTION { get; set; }
        public string DOCTOR { get; set; }
        public bool POSTED { get; set; }
        public string TOSECTION { get; set; }

        public static MRB21 GetMRB21(string GroupCodeID, string PatientID)
        {
            MRB21 mrb21 = new MRB21();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "MRB21_Get"; //"spGetPatient";
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
                    mrb21.GROUPCODE = reader["groupcode"].ToString();
                    mrb21.PATIENTNO = reader["patientno"].ToString();
                    mrb21.NAME = reader["name"].ToString();
                    mrb21.SENDER = reader["sender"].ToString();
                    mrb21.TRANS_DATE = (DateTime)reader["trans_date"];
                    mrb21.FACILITY = reader["facility"].ToString();
                    mrb21.NOTES = reader["notes"].ToString();
                    mrb21.RECEIVED = reader["received"].ToString();
                    mrb21.OPERATOR = reader["operator"].ToString();
                    mrb21.REFERENCE = reader["reference"].ToString();
                    mrb21.SENDSECTION = reader["sendsection"].ToString();
                    mrb21.DOCTOR = reader["doctor"].ToString();
                    mrb21.POSTED = (bool)reader["posted"];
                    mrb21.TOSECTION = reader["tosection"].ToString();

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

            return mrb21;
        }
        public static bool Writemrb21Details(string groupcode, string patientno, DateTime xtrans_date, string xname, string facility, string sender, string notes,string reference,string sendsection,string tosection,string xoperator,string xdoctor, string xttype)
        {
           // DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "MRB21_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

                    insertCommand.Parameters.AddWithValue("@groupcode",groupcode ); 
                    insertCommand.Parameters.AddWithValue("@patientno",patientno  );
                    insertCommand.Parameters.AddWithValue("@name",xname );
                    insertCommand.Parameters.AddWithValue("@sender",sender );
                    insertCommand.Parameters.AddWithValue("@trans_date",xtrans_date );
                    insertCommand.Parameters.AddWithValue("@facility",facility ); 
                    insertCommand.Parameters.AddWithValue("@notes",notes ); 
                    insertCommand.Parameters.AddWithValue("@received", "" );
                    insertCommand.Parameters.AddWithValue("@operator",xoperator );
                    insertCommand.Parameters.AddWithValue("@reference",reference );
                    insertCommand.Parameters.AddWithValue("@sendsection",sendsection ); 
                    insertCommand.Parameters.AddWithValue("@doctor",xdoctor);
                    insertCommand.Parameters.AddWithValue("@posted",false );
                    insertCommand.Parameters.AddWithValue("@tosection",tosection );
                    insertCommand.Parameters.AddWithValue("@ttype", xttype ); 

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();

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