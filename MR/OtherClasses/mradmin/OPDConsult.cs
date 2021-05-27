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
    public class OPDConsult
    {
        public DateTime TRANS_DATE { get; set; }
        public bool POSTED { get; set; }
        public string DOCTOR { get; set; }
        public string REFERENCE { get; set; }

        public static int GetOpdConsult(string xdoctor, DateTime currentdate,bool XPOSTED)
        {
            int xcount = 0;
            OPDConsult opdconsult = new OPDConsult();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "OPDCONSULT_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Doctor", xdoctor);
            selectCommand.Parameters.AddWithValue("@Trans_date", currentdate);
            selectCommand.Parameters.AddWithValue("@POSTED", XPOSTED);

            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(); //CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    opdconsult.DOCTOR = reader["doctor"].ToString();
                    opdconsult.REFERENCE = reader["reference"].ToString();
                    opdconsult.TRANS_DATE = (DateTime)reader["trans_date"];
                    opdconsult.POSTED = (bool)reader["posted"];
                    xcount++;
                }
                else
                {
                    connection.Close();
                    return xcount;

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show("" + ex, "Get OPD Consult Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return xcount;
            }
            finally
            {
                connection.Close();
            }

            return xcount;
        }
        public static bool WriteOpdConsult(string xdoctor, string xreference,bool xposted,DateTime xtrans_date)
        {
            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "OPDCONSULT_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@doctor", xdoctor.ToString());
            insertCommand.Parameters.AddWithValue("@reference", xreference.ToString());
            insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
            insertCommand.Parameters.AddWithValue("@posted", xposted);
 
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
 
            }
            catch (SqlException ex)
            {
                 MessageBox.Show(" " + ex, "Add OPD Consult Details", MessageBoxButtons.OK,
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