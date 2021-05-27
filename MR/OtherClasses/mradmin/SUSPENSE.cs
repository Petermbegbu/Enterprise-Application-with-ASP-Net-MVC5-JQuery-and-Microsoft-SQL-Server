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
    public class SUSPENSE
    {
        public string REFERENCE { get; set; }
        public string NAME { get; set; }
        public Decimal ITEMNO { get; set; }
        public string DESCRIPTON { get; set; }
        public string RECTYPE { get; set; }
        public string PROCESS { get; set; }
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string GROUPHEAD { get; set; }
        public string TRANSTYPE { get; set; }
        public string DOCTOR { get; set; }
        public string FACILITY { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal AMOUNT { get; set; }
        public string GHGROUPCODE { get; set; }
        public string TITLE { get; set; }
        public string ADDRESS1 { get; set; }
        public string CURRENCY { get; set; }
        public decimal EXRATE { get; set; }
        public decimal FCAMOUNT { get; set; }
        public decimal DURATION { get; set; }
        public string BILLPROCESS { get; set; }
        public string NOTES { get; set; }
        public string SERVICETYPE { get; set; }
        public bool CAPITATED { get; set; }
        /// <summary>
        /// Retrieve Investigations Request from Suspense : 
        /// Sorttype : A=ALL, P=FOR PROCESSED, U-UNPROCESSED REQUESTS
        /// </summary>
        /// <param name="xreference"></param>
        /// <param name="sorttype"></param>
        /// <returns></returns>
        public static DataTable GetSUSPENSE(string xreference,string sorttype)
        {
            SUSPENSE supense = new SUSPENSE();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "SUSPENSE_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

  //          selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            selectCommand.Parameters.AddWithValue("@Reference", xreference);
            selectCommand.Parameters.AddWithValue("@Sorttype", sorttype);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
/*

            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    supense.REFERENCE = reader["reference"].ToString();
                    supense.NAME = reader["name"].ToString();
                    supense.ITEMNO = (Decimal)reader["itemno"];
                    supense.DESCRIPTON = reader["descripton"].ToString();
                    supense.RECTYPE = reader["rectype"].ToString();
                    supense.PROCESS = reader["process"].ToString();
                    supense.GROUPCODE = reader["groupcode"].ToString();
                    supense.PATIENTNO = reader["patientno"].ToString();
                    supense.GROUPHEAD = reader["grouphead"].ToString();
                    supense.TRANSTYPE = reader["transtype"].ToString();
                    supense.DOCTOR = reader["doctor"].ToString();
                    supense.FACILITY = reader["facility"].ToString();
                    supense.POSTED = (bool)reader["posted"];
                    supense.POST_DATE = (DateTime)reader["post_date"];
                    supense.TRANS_DATE = (DateTime)reader["trans_date"];
                    supense.AMOUNT = (Decimal)reader["amount"];
                    supense.GHGROUPCODE = reader["GHGROUPCODE"].ToString();
                    supense.TITLE = reader["title"].ToString();
                    supense.ADDRESS1 = reader["address1"].ToString();
                    supense.ADDRESS2 = reader["address2"].ToString();
                    supense.CURRENCY = reader["currency"].ToString();
                    supense.EXRATE = (Decimal)reader["exrate"];
                    supense.FCAMOUNT = (Decimal)reader["fcamount"];
                    supense.DURATION = (Decimal)reader["duration"];
                    supense.BILLPROCESS = reader["billprocess"].ToString();
                    supense.NOTES = reader["notes"].ToString();
                    supense.SERVICETYPE = reader["servicetype"].ToString();
                    supense.CAPITATED = (bool)reader["capitated"];

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

            return supense; */
        }
        public static bool DeleteSuspense( int recid ) //  string reference, string process, decimal itemcount)
        {
            SqlConnection connection = Dataaccess.mrConnection();
            SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.CommandText = "SUSPENSE_Delete";
            deleteCommand.Connection = connection;
            deleteCommand.CommandType = CommandType.StoredProcedure;

            deleteCommand.Parameters.AddWithValue("@recid", recid);

        /*    deleteCommand.Parameters.AddWithValue("@Reference", reference);
            deleteCommand.Parameters.AddWithValue("@process", process);
            deleteCommand.Parameters.AddWithValue("@Itemno", itemcount);*/
            try
            {
                connection.Open();
                int count = deleteCommand.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Unable to Open SQL Server Database Table" + ex);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

    }
}