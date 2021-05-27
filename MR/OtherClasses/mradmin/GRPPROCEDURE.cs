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
    public class GRPPROCEDURE
    {
        public string REFERENCE { get; set; }
        public string FACILITY { get; set; }
        public string NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public bool grpbillbyservtype {get; set;}


        public static DataTable GetGRPPROCEDURE(string reference)
        {
            //GRPPROCEDURE grpproc = new GRPPROCEDURE();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "GRPPROCEDURE_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@reference", reference);
            //selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        /// <summary>
        /// Load all definitons
        /// </summary>
        /// <returns></returns>
        public static DataTable GetGRPPROCEDURE()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "GRPPROCEDURE_Getlist"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static bool DeleteItem(string xreference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "GRPPROCEDURE_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Reference", xreference);
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
                MessageBox.Show(" " + ex, "Delete Grp Procedure Details", MessageBoxButtons.OK,
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