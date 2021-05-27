using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using msfunc;
//using System.Windows.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

namespace mradmin.DataAccess
{
    public class STKCHARG
    {
        public string ITEM { get; set; }
        public string PATCATEG { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PERCENTCHARGE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string PATCATEGDESC { get; set; }
        /// <summary>
        /// Retrieves for differential charge for a billing category
        /// </summary>
        /// <param name="stockcode"></param>
        /// <param name="patientbillcategory"></param>
        /// <returns></returns>
        public static STKCHARG GetSTKCHARG(string stockcode, string patientbillcategory)
        {
            STKCHARG stkch = new STKCHARG();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "STKCHARG_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Item", stockcode);
            selectCommand.Parameters.AddWithValue("@Patcateg", patientbillcategory);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    stkch.ITEM = reader["item"].ToString();
                    stkch.PATCATEG = reader["patcateg"].ToString();
                    stkch.AMOUNT = (Decimal)reader["amount"];
                    stkch.PERCENTCHARGE = (Decimal)reader["percentcharge"];
                    stkch.POSTED = (bool)reader["posted"];
                    stkch.POST_DATE = (DateTime)reader["post_date"];
                    stkch.PATCATEGDESC = reader["patcategdesc"].ToString();
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
                MessageBox.Show("" + ex, "Get Stock Differential Billing Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return stkch;
        }
        /// <summary>
        /// Retrieves all differential charges details
        /// </summary>
        /// <param name="stockcode"></param>
        /// <returns></returns>
        public static DataTable GetSTKCHARG(string stockcode)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "STKCHARG_GetByStkItem"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Item", stockcode);
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static bool DeleteItem(string stockcode, string patientbillcategory)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "STKCHARG_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Item", stockcode);
            deleteStatement.Parameters.AddWithValue("@Patcateg", patientbillcategory);
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
                MessageBox.Show(" " + ex, "Delete Differential Tariff Item", MessageBoxButtons.OK,
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