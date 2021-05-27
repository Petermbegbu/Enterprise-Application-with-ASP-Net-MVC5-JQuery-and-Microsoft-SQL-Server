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
    public class Mrattend
    {
       public string REFERENCE { get; set; }
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string CLINIC { get; set; }
        public string BILLED { get; set; }
        public string GROUPHEAD { get; set; }
        public string GROUPHTYPE { get; set; }
        public bool VTAKEN { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string GHGROUPCODE { get; set; }
        public string DOCTOR { get; set; }
        public string DOC_TIME { get; set; }
        public string DIAGNOSIS { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string ATTENDTYPE { get; set; }
        public bool SENDEXCL { get; set; }
        public string REFERRER { get; set; }
        public string AUTHORIZEDCODE { get; set; }
        
    

     public static Mrattend GetMrattend(string Reference )
        {
            Mrattend mrattend = new Mrattend();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "MRATTEND_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
           // selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    mrattend.REFERENCE = reader["reference"].ToString();
                    mrattend.GROUPCODE = reader["groupcode"].ToString();
                    mrattend.PATIENTNO = reader["patientno"].ToString();
                    mrattend.NAME = reader["name"].ToString();
                    mrattend.TRANS_DATE = (DateTime)reader["trans_date"];
                    mrattend.CLINIC = reader["clinic"].ToString();
                    mrattend.BILLED = reader["billed"].ToString();
                    mrattend.GROUPHEAD = reader["grouphead"].ToString();
                    mrattend.GROUPHTYPE = reader["grouphtype"].ToString();
                    mrattend.VTAKEN = (bool)reader["vtaken"];
                    mrattend.POSTED = (bool)reader["posted"];
          //          mrattend.POST_DATE = ( (reader["Trans_date"] != DBNull.Value) ? (DateTime)reader["post_date"] : DateTime.MinValue );
                    mrattend.GHGROUPCODE = reader["GHGROUPCODE"].ToString();
                    mrattend.DOCTOR = reader["doctor"].ToString();
                    mrattend.DOC_TIME = reader["doc_time"].ToString();
                    mrattend.DIAGNOSIS = reader["diagnosis"].ToString();
                    mrattend.OPERATOR = reader["operator"].ToString();
                    mrattend.DTIME  = (DateTime)reader["dtime"];
                    mrattend.ATTENDTYPE = reader["attendtype"].ToString();
                    mrattend.SENDEXCL = (bool)reader["sendexcl"];
                    mrattend.REFERRER = reader["referrer"].ToString();
                    mrattend.AUTHORIZEDCODE = reader["authorizedcode"].ToString();
                                                                            
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
                MessageBox.Show("" + ex, "Get Daily Attendance Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return mrattend;
        }
    public static Mrattend GetMultiattend(string groupcodeid, string patientId, DateTime xtransdate )
        {
            Mrattend mrattend = new Mrattend();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            string selectStatement = "SELECT reference, name, clinic, operator, dtime FROM MRATTEND WHERE "+
                "patientno = '"+patientId+"' AND groupcode = '"+groupcodeid+"' AND trans_date = '"+DateTime.Now.ToShortDateString()+"'";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
//nd date='"+ date1.ToString("yyyy-MM-dd HH:mm:ss") +"'"
//"SELECT * FROM KSMCemp_data WHERE Nationality = @Nationality and [date] = @Exp_Date"

            selectCommand.Parameters.AddWithValue("@groupcodeid", groupcodeid);
            selectCommand.Parameters.AddWithValue("@PatientId", patientId);
            selectCommand.Parameters.AddWithValue("@xtransdate", xtransdate);
        
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    mrattend.REFERENCE = reader["reference"].ToString();
                    mrattend.NAME = reader["name"].ToString();
                    mrattend.CLINIC = reader["clinic"].ToString();
                    mrattend.OPERATOR = reader["operator"].ToString();
                    mrattend.DTIME  = (DateTime)reader["dtime"];
                                                                            
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
                MessageBox.Show("" + ex, "Get Daily Attendance Details ", MessageBoxButtons.OK,
                       MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return mrattend;
        }
        public static bool DeleteMrattend(string Reference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "Mrattend_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Reference", Reference);
            try
            {
                connection.Open();
                int count = deleteStatement.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch //(SqlException ex)
            {
                //throw ex;

                MessageBox.Show("Unable to Open SQL Server Database Table", "Delete Patient Bills", MessageBoxButtons.OK,
    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }

        }
        public static bool mrattendWrite(bool newrec, string reference, string groupcode, string patientno, string patname, DateTime transdate, string clinic, string grouphead, string grouphtype, bool vitalstaken, bool isposted, string ghgroupcode, string doctor, string docstime, string diag, string xoperator, DateTime operatordatetime, string attendtype, bool sendeclusive, string referrer, string hmoauthorizationcode)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "Mrattend_Add" : "Mrattend_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", reference);
            insertCommand.Parameters.AddWithValue("@groupcode", groupcode);
            insertCommand.Parameters.AddWithValue("@patientno", patientno);
            insertCommand.Parameters.AddWithValue("@name", patname);
            insertCommand.Parameters.AddWithValue("@trans_date", transdate);
            insertCommand.Parameters.AddWithValue("@clinic", clinic);
            insertCommand.Parameters.AddWithValue("@billed","");
            insertCommand.Parameters.AddWithValue("@grouphead", grouphead);
            insertCommand.Parameters.AddWithValue("@grouphtype", grouphtype);
            insertCommand.Parameters.AddWithValue("@vtaken", vitalstaken);
            insertCommand.Parameters.AddWithValue("@posted", isposted);
            insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@GHGROUPCODE", ghgroupcode);
            insertCommand.Parameters.AddWithValue("@doctor", doctor);
            insertCommand.Parameters.AddWithValue("@doc_time", docstime);
            insertCommand.Parameters.AddWithValue("@diagnosis", diag);
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@dtime", operatordatetime);
            insertCommand.Parameters.AddWithValue("@attendtype", attendtype);
            insertCommand.Parameters.AddWithValue("@SENDEXCL", sendeclusive);
            insertCommand.Parameters.AddWithValue("@referrer", referrer);
            insertCommand.Parameters.AddWithValue("@authorizedcode", hmoauthorizationcode);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("" + ex, "Update Daily Attendance Details", MessageBoxButtons.OK,
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