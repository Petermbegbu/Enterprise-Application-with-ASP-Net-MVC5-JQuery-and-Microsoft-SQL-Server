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
    public class BILL_ADJ
    {
        public string REFERENCE { get; set; }
        public string CUSTOMER { get; set; }
        public string NAME { get; set; }
        public string CUSTTYPE { get; set; }
        public string ADJUST { get; set; }
        public string TYPE { get; set; }
        public decimal AMOUNT { get; set; }
        public string COMMENTS { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal SEC_LEVEL { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string GROUPCODE { get; set; }
        public string FACILITY { get; set; }
        public string CURRENCY { get; set; }
        public decimal EXRATE { get; set; }
        public decimal FCAMOUNT { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string ADJUSTDESC { get; set; }
        public string DEBITACCT { get; set; }
        public string CREDITACCT { get; set; }

        public static BILL_ADJ GetBILL_ADJ(string GroupCodeID, string PatientID)
        {
            BILL_ADJ bill = new BILL_ADJ();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BILL_ADJ_Get"; //"spGetPatient";
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

                    bill.REFERENCE = reader["reference"].ToString();
                    bill.CUSTOMER = reader["grouphead"].ToString();
                    bill.NAME = reader["name"].ToString();
                    bill.CUSTTYPE = reader["transtype"].ToString();
                    bill.ADJUST = reader["adjust"].ToString();
                    bill.TYPE = reader["ttype"].ToString();
                    bill.AMOUNT = (Decimal)reader["amount"];
                    bill.COMMENTS = reader["comments"].ToString();
                    bill.TRANS_DATE = (DateTime)reader["trans_date"];
                    bill.SEC_LEVEL = (Decimal)reader["sec_level"];
                    bill.POSTED = (bool)reader["posted"];
                    bill.POST_DATE = (DateTime)reader["post_date"];
                    bill.GROUPCODE = reader["ghgroupcode"].ToString();
                    bill.FACILITY = reader["facility"].ToString();
                    bill.CURRENCY = reader["currency"].ToString();
                    bill.EXRATE = (Decimal)reader["exrate"];
                    bill.FCAMOUNT = (Decimal)reader["fcamount"];
                    bill.OPERATOR = reader["operator"].ToString();
                    bill.DTIME = (DateTime)reader["dtime"];
                    bill.DEBITACCT = reader["debitacct"].ToString();
                    bill.CREDITACCT = reader["creditacct"].ToString();
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
            return bill;
        }
        /// <summary>
        /// get by adjust reference
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static BILL_ADJ GetBILL_ADJ(string reference)
        {
            BILL_ADJ bill = new BILL_ADJ();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BILL_ADJ_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    bill.REFERENCE = reader["reference"].ToString();
                    bill.CUSTOMER = reader["grouphead"].ToString();
                    bill.NAME = reader["name"].ToString();
                    bill.CUSTTYPE = reader["transtype"].ToString();
                    bill.ADJUST = reader["adjust"].ToString();
                    bill.TYPE = reader["ttype"].ToString();
                    bill.AMOUNT = (Decimal)reader["amount"];
                    bill.COMMENTS = reader["comments"].ToString();
                    bill.TRANS_DATE = (DateTime)reader["trans_date"];
                    bill.SEC_LEVEL = (Decimal)reader["sec_level"];
                    bill.POSTED = (bool)reader["posted"];
                    bill.POST_DATE = (DateTime)reader["post_date"];
                    bill.GROUPCODE = reader["ghgroupcode"].ToString();
                    bill.FACILITY = reader["facility"].ToString();
                    bill.CURRENCY = reader["currency"].ToString();
                    bill.EXRATE = (Decimal)reader["exrate"];
                    bill.FCAMOUNT = (Decimal)reader["fcamount"];
                    bill.OPERATOR = reader["operator"].ToString();
                    bill.DTIME = (DateTime)reader["dtime"];
                    bill.DEBITACCT = reader["debitacct"].ToString();
                    bill.CREDITACCT = reader["creditacct"].ToString();

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
                MessageBox.Show("" + ex, "Get Patient Details ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }
            return bill;
        }
        public static DataTable GetAdjustdetails(string grouphead, string misc_name, string patientno, string sorttype,
             DateTime datefrom, DateTime dateto)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BILL_ADJ_GetDetails";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@grouphead", grouphead);
            selectCommand.Parameters.AddWithValue("@name", misc_name);
            selectCommand.Parameters.AddWithValue("@Sorttype", sorttype);
            selectCommand.Parameters.AddWithValue("@Datefrom", datefrom);
            selectCommand.Parameters.AddWithValue("@Dateto", dateto);
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static Decimal GetAdjustOpBal(string grouphead, string misc_name, string patientno, string sorttype, DateTime transdate,
            bool inclusive)
        {
            Billings billing = new Billings();
            decimal xamt = 0m,rtnamt = 0m;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BILL_ADJ_GetOpBal"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@grouphead", grouphead);
            selectCommand.Parameters.AddWithValue("@name", misc_name);
            selectCommand.Parameters.AddWithValue("@Sorttype", sorttype);
            selectCommand.Parameters.AddWithValue("@transdate", transdate );
            selectCommand.Parameters.AddWithValue("@Inclusive", inclusive);

            try
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
            }
            return rtnamt;
        } 


    }
}