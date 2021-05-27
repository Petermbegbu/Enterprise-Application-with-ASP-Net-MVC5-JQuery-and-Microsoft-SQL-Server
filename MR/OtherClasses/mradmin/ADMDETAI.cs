﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using msfunc;

namespace mradmin.DataAccess
{
    public class ADMDETAI
    {
        public string REFERENCE { get; set; }
        public DateTime DATE { get; set; }
        public string TIME { get; set; }
        public string MASTPROCESS { get; set; }
        public string PROCESS { get; set; }
        public string STK_ITEM { get; set; }
        public string DESCRIPTION { get; set; }
        public string UNIT { get; set; }
        public decimal QTY { get; set; }
        public decimal AMOUNT { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime OP_TIME { get; set; }
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string DOCTOR { get; set; }
        public string FACILITY { get; set; }
        public string STORE { get; set; }

        public static DataTable GetADMDETAI(string reference)
        {
           // ADMDETAI adetai = new ADMDETAI();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ADMDETAI_Get"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        /// <summary>
        /// Check and get details of process if generated for the day
        /// necessary for autogenerated services - accomodation etc.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static DataTable GetADMDETAI(string Reference, string process, DateTime transdate)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ADMETAI_GetDayProcess";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            selectCommand.Parameters.AddWithValue("@process", process);
            selectCommand.Parameters.AddWithValue("@trans_date", transdate);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }

        public static bool DeleteAdmdetail(int RECID) // string Reference, string process, DateTime transdate)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "Admdetai_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@RECID", RECID);
       /*     deleteStatement.Parameters.AddWithValue("@process", process);
            deleteStatement.Parameters.AddWithValue("@trans_date", transdate);*/
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

                MessageBox.Show(" " + ex, "Delete Admission Bills", MessageBoxButtons.OK,
    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static void writeAdmdetails(bool xnewrec, string xreference,DateTime xdate, string xtime, string xprocess, string masterprocess,string stkitemcode,  string xdescription,string unit,decimal qty, decimal xamount, bool posted, DateTime postdate, string xoperator, DateTime opdatetime,string groupcode,string patientno,string doctor, string facility, int recid, string xstore)
        {
           // DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (xnewrec) ? "Admdetai_Add" : "Admdetai_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;
             
            insertCommand.Parameters.AddWithValue("@reference",xreference );
            insertCommand.Parameters.AddWithValue("@trans_date",xdate );
            insertCommand.Parameters.AddWithValue("@time",xtime );
            insertCommand.Parameters.AddWithValue("@mastprocess",masterprocess );
            insertCommand.Parameters.AddWithValue("@process",xprocess );
            insertCommand.Parameters.AddWithValue("@stk_item",stkitemcode );
            insertCommand.Parameters.AddWithValue("@description",xdescription );
            insertCommand.Parameters.AddWithValue("@unit",unit );
            insertCommand.Parameters.AddWithValue("@qty",qty );
            insertCommand.Parameters.AddWithValue("@amount",xamount );
            insertCommand.Parameters.AddWithValue("@posted",posted );
            insertCommand.Parameters.AddWithValue("@post_date",postdate );
            insertCommand.Parameters.AddWithValue("@operator",xoperator);
            insertCommand.Parameters.AddWithValue("@op_time",opdatetime );
            insertCommand.Parameters.AddWithValue("@groupcode",groupcode);
            insertCommand.Parameters.AddWithValue("@patientno",patientno );
            insertCommand.Parameters.AddWithValue("@doctor", doctor );
            insertCommand.Parameters.AddWithValue("@facility", facility);
            insertCommand.Parameters.AddWithValue("@store", xstore);
            if (!xnewrec)
                insertCommand.Parameters.AddWithValue("@recid", recid);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("SQL access" + ex, "BILLINGS UPDATE", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }

        }
        /// <summary>
        /// get discharge bills sum(amount) and grouped by mastprocess
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="dischargebills"></param>
        /// <returns></returns>
        public static DataTable GetADMDETAI(string reference,bool dischargebills)
        {
            // ADMDETAI adetai = new ADMDETAI();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ADMDETAI_GetforDischargeBillUpdate";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }

/*            selectCommand.Parameters.AddWithValue("@Reference", reference);

            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    adetai.REFERENCE = reader["reference"].ToString();
                    adetai.DATE = (DateTime)reader["date"];
                    adetai.TIME = reader["time"].ToString();
                    adetai.MASTPROCESS = reader["mastprocess"].ToString();
                    adetai.PROCESS = reader["process"].ToString();
                    adetai.STK_ITEM = reader["stk_item"].ToString();
                    adetai.DESCRIPTION = reader["description"].ToString();
                    adetai.UNIT = reader["unit"].ToString();
                    adetai.QTY = (Decimal)reader["qty"];
                    adetai.AMOUNT = (Decimal)reader["amount"];
                    adetai.POSTED = (bool)reader["posted"];
                    adetai.POST_DATE = (DateTime)reader["post_date"];
                    adetai.OPERATOR = reader["operator"].ToString();
                    adetai.OP_TIME = (DateTime)reader["op_time"];
                    adetai.GROUPCODE = reader["groupcode"].ToString();
                    adetai.PATIENTNO = reader["patientno"].ToString();
                    adetai.DOCTOR = reader["doctor"].ToString();



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

            return adetai;
        }
*/

    }
}