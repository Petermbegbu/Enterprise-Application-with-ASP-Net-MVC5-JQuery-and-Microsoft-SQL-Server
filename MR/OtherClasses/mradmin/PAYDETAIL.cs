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
    public class PAYDETAIL
    {
        public string REFERENCE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public decimal ITEMNO { get; set; }
        public string DESCRIPTION { get; set; }
        public string DOCTOR { get; set; }
        public string FACILITY { get; set; }
        public decimal AMOUNT { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal SEC_LEVEL { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool RECEIPTED { get; set; }
        public string TRANSTYPE { get; set; }
        public string GROUPHEAD { get; set; }
        public string SERVICETYPE { get; set; }
        public string GROUPCODE { get; set; }
        public string TTYPE { get; set; }
        public string GHGROUPCODE { get; set; }
        public string PAYTYPE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime OP_TIME { get; set; }
        public string ACCOUNTTYPE { get; set; }
        public string CURRENCY { get; set; }
        public decimal EXRATE { get; set; }
        public decimal FCAMOUNT { get; set; }
        public string EXTDESC { get; set; }
        public DateTime DATERECEIVED { get; set; }
        public string CROSSREF { get; set; }

        public static DataTable GetPAYDETAIL(string GroupCodeID, string PatientID, bool byreference, string reference)
        {
            //PAYDETAIL paydet = new PAYDETAIL();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = (byreference) ? "PAYDETAIL_GetReference" : "PAYDETAIL_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            if (byreference)
            {
                selectCommand.Parameters.AddWithValue("@Reference", reference);
            }
            else
            {
                selectCommand.Parameters.AddWithValue("@GroupCodeID", GroupCodeID);
                selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            }
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static DataTable GetPAYMENTdetails(string grouphead, string misc_name, string patientno, string sorttype,
             DateTime datefrom, DateTime dateto)
        {
            // Billings billing = new Billings();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "PAYDETAIL_GetDetails";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@grouphead", grouphead);
            selectCommand.Parameters.AddWithValue("@name", misc_name);
            selectCommand.Parameters.AddWithValue("@Patientno", patientno );
            selectCommand.Parameters.AddWithValue("@Sorttype", sorttype);
            selectCommand.Parameters.AddWithValue("@Datefrom", datefrom);
            selectCommand.Parameters.AddWithValue("@Dateto", dateto);
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static Decimal GetPAYOpBal(string grouphead, string misc_name, string patientno, string sorttype, DateTime transdate,
            bool inclusive)
        {
            Billings billing = new Billings();
            decimal xdamt = 0m,xcamt = 0m,xamt=0m;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "PAYDETAIL_GetOpBal"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@grouphead", grouphead);
            selectCommand.Parameters.AddWithValue("@name", misc_name);
            selectCommand.Parameters.AddWithValue("patientno", patientno);
            selectCommand.Parameters.AddWithValue("@Sorttype", sorttype);
            selectCommand.Parameters.AddWithValue("@transdate", transdate );
            selectCommand.Parameters.AddWithValue("@Inclusive", inclusive);
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
 /*           try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader();//CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    xamt = (Decimal)reader["amount"]; 
                    if (reader["ttype"].ToString()=="C")
                        rtnamt += xamt;
                    else
                        rtnamt = rtnamt - xamt;
 
                    //billing.AMOUNT = (Decimal)reader["amount"]; 
                    //billing.TRANS_DATE = (DateTime)reader["trans_date"];
                    //billing.TTYPE  = reader["ttype"].ToString();
                                                                                                                                      
                }
                else
                {
                    connection.Close();
                    return 0m;

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show("" + ex, "Get Patient Bills ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return 0m;
            }
            finally
            {
                connection.Close();
            } */
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                xamt = (Decimal)dt.Rows[i]["amount"];
                if (dt.Rows[i]["ttype"].ToString() == "D")
                    xdamt += xamt;
                else
                    xcamt += xamt;
            }
            return Math.Abs( (xdamt - xcamt) );
        } 
        /// <summary>
        /// Delete all pay details on a particular reference.
        /// </summary>
        public static bool DeletePay(string Reference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "Paydetail_Delete";
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
            catch (SqlException ex)
            {
                //throw ex;

                MessageBox.Show(" " + ex, "Delete Patient Paymeny", MessageBoxButtons.OK,
    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}