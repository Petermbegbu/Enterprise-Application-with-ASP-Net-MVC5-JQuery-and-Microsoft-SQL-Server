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
    public class INVLINK
    {
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string TIMESENT { get; set; }
        public DateTime DATEREC { get; set; }
        public string TIMEREC { get; set; }
        public string REFERENCE { get; set; }
        public string OPERATOR { get; set; }
        public string FACILITY { get; set; }
        public string DOCTOR { get; set; }
        public string CROSSREF { get; set; }
        public string GHGROUPCODE { get; set; }
        public string GROUPHEAD { get; set; }



        public static int GetINVLINK(string xdoctor, DateTime xtrans_date)
        {
            INVLINK invlink = new INVLINK();
            int xcount = 0;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "INVLINK_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@DOCTOR", xdoctor);
            selectCommand.Parameters.AddWithValue("@trans_date", xtrans_date);
            try
            {
                connection.Open();
 
                SqlDataReader reader = selectCommand.ExecuteReader(); //CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    xcount++;

               /*     invlink.GROUPCODE = reader["groupcode"].ToString();
                    invlink.PATIENTNO = reader["patientno"].ToString();
                    invlink.NAME = reader["name"].ToString();
                    invlink.TRANS_DATE = (DateTime)reader["trans_date"]; 
                    invlink.POSTED = (bool)reader["posted"];
                    invlink.POST_DATE = (DateTime)reader["post_date"];
                    invlink.TIMESENT = reader["timesent"].ToString();
                    invlink.DATEREC = (DateTime)reader["daterec"];
                    invlink.TIMEREC = reader["timerec"].ToString();
                    invlink.REFERENCE = reader["reference"].ToString();
                    invlink.OPERATOR = reader["operator"].ToString();
                    invlink.FACILITY = reader["facility"].ToString();
                    invlink.DOCTOR = reader["doctor"].ToString();
                    invlink.CROSSREF = reader["crossref"].ToString();
                    invlink.GHGROUPCODE = reader["GHGROUPCODE"].ToString();
                    invlink.GROUPHEAD = reader["grouphead"].ToString();

                    */


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
                MessageBox.Show("" + ex, "Get Inv Result Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return 0;
            }
            finally
            {
                connection.Close();
            }

            return xcount;
        }
        public static bool WriteInvLink(DateTime xtrans_date,bool xposted,DateTime xpost_date,DateTime xdaterec,string xtimerec,
            string xfaciity,string xdoctor,string xcrossref)

        {
            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "INVLINK_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
            insertCommand.Parameters.AddWithValue("@posted", xposted);
            insertCommand.Parameters.AddWithValue("@post_date", xpost_date );
            insertCommand.Parameters.AddWithValue("@daterec", xdaterec);
            insertCommand.Parameters.AddWithValue("@timerec", xtimerec);
            insertCommand.Parameters.AddWithValue("@facility", xfaciity);
            insertCommand.Parameters.AddWithValue("@doctor", xdoctor.ToString());
            insertCommand.Parameters.AddWithValue("@crossref", xcrossref);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(" " + ex, "Update Inv Alert Link", MessageBoxButtons.OK,
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