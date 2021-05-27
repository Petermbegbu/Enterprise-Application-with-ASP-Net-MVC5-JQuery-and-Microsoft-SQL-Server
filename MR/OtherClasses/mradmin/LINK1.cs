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
    public class LINK1
    {
        public string OPERATOR { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal DEBIT { get; set; }
        public decimal CREDIT { get; set; }
        public string CTIME { get; set; }
        public DateTime CDATE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public decimal ITEMNO { get; set; }
        public string TTIME { get; set; }
        public string TIME_IN { get; set; }
        public string RECEIVER { get; set; }
        public decimal DIFF { get; set; }

        public static LINK1 GetLINK1(string xoperator,DateTime xdate,bool fromcashoffice)
        {
            LINK1 link1 = new LINK1();
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "LINK1_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@OPERATOR", xoperator);
            selectCommand.Parameters.AddWithValue("@TRANS_DATE", xdate);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    link1.OPERATOR = reader["operator"].ToString();
                    link1.TRANS_DATE = (DateTime)reader["trans_date"];
                    link1.DEBIT = (Decimal)reader["debit"];
                    link1.ITEMNO = (Decimal)reader["itemno"];
                    if (!fromcashoffice)
                    {
                        link1.CREDIT = (Decimal)reader["credit"];
                        link1.CTIME = reader["ctime"].ToString();
                        link1.CDATE = (DateTime)reader["cdate"];
                        link1.POSTED = (bool)reader["posted"];
                        link1.POST_DATE = (DateTime)reader["post_date"];
                        
                        link1.TTIME = reader["ttime"].ToString();
                        link1.TIME_IN = reader["time_in"].ToString();
                        link1.RECEIVER = reader["receiver"].ToString();
                        link1.DIFF = (Decimal)reader["diff"];
                    }
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
                MessageBox.Show("" + ex, "Get CASHIERS Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return link1;
        }
        public static bool LINK1Write(bool newrec, string xoperator, DateTime xdate, decimal debitamt, decimal itemno)
        {
            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "LINK1_Add" : "LINK1_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@trans_date", xdate);
            insertCommand.Parameters.AddWithValue("@debit", debitamt);
            insertCommand.Parameters.AddWithValue("@itemno", itemno);
            insertCommand.Parameters.AddWithValue("@ttime",DateTime.Now.ToShortTimeString());
            if (newrec)
            {
                insertCommand.Parameters.AddWithValue("@time_in", DateTime.Now.ToShortTimeString());
            }
            //insertCommand.Parameters.AddWithValue("@credit",);
            //insertCommand.Parameters.AddWithValue("@ctime",);
            //insertCommand.Parameters.AddWithValue("@cdate",);  
            //insertCommand.Parameters.AddWithValue("@posted",);
            //insertCommand.Parameters.AddWithValue("@post_date",); 

            /*           
                        
                        insertCommand.Parameters.AddWithValue("@receiver",); 
                        insertCommand.Parameters.AddWithValue("@diff",); */

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("" + ex, "Add Customer Details", MessageBoxButtons.OK,
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