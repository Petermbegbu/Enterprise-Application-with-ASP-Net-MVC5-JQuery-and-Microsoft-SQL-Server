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
    public class MRSTLEV
    {
        public string OPERATOR { get; set; }
        public string PASSWORD { get; set; }
        public bool FM1 { get; set; }
        public bool FM2 { get; set; }
        public bool FM3 { get; set; }
        public bool FM4 { get; set; }
        public bool FM5 { get; set; }
        public bool FM6 { get; set; }
        public bool FM7 { get; set; }
        public bool FM8 { get; set; }
        public bool FM9 { get; set; }
        public bool FM10 { get; set; }
        public bool FM11 { get; set; }
        public bool FM12 { get; set; }
        public bool FM13 { get; set; }
        public bool FM14 { get; set; }
        public bool FM15 { get; set; }
        public bool FM16 { get; set; }
        public bool FM17 { get; set; }
        public bool FM18 { get; set; }
        public bool FM19 { get; set; }
        public bool FM20 { get; set; }
        public bool FM21 { get; set; }
        public bool FM22 { get; set; }
        public bool FM23 { get; set; }
        public bool RM1 { get; set; }
        public bool RM2 { get; set; }
        public bool RM3 { get; set; }
        public bool RM4 { get; set; }
        public bool RM5 { get; set; }
        public bool RM6 { get; set; }
        public bool RM7 { get; set; }
        public bool RM8 { get; set; }
        public bool RM9 { get; set; }
        public bool RM10 { get; set; }
        public bool RM11 { get; set; }
        public bool RM12 { get; set; }
        public bool RM13 { get; set; }
        public bool RM14 { get; set; }
        public bool RM15 { get; set; }
        public bool RM16 { get; set; }
        public bool RM17 { get; set; }
        public bool RM18 { get; set; }
        public bool UM1 { get; set; }
        public bool UM2 { get; set; }
        public bool UM3 { get; set; }
        public bool UM4 { get; set; }
        public bool UM5 { get; set; }
        public bool UM6 { get; set; }
        public bool UM7 { get; set; }
        public bool UM8 { get; set; }
        public bool UM9 { get; set; }
        public bool UM10 { get; set; }
        public bool UM11 { get; set; }
        public bool UM12 { get; set; }
        public int WSECLEVEL { get; set; }
        public DateTime PASSDATE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string INITIAL { get; set; }
        public string SECTION { get; set; }
        public bool CANDELETE { get; set; }
        public bool CANALTER { get; set; }
        public bool CANADD { get; set; }
        public bool SHIELDPRICE { get; set; }
        public bool CHANGEPRESC { get; set; }
        public int HISTYEAR { get; set; }
        public string FACILITY { get; set; }

        public static DataTable GetListMRSTLEV()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "MRSTLEV_GetList"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static DataTable GetMRSTLEV(string xoperator)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "MRSTLEV_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@OPERATOR", xoperator);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static void updatePassword(string xoperator,string xpassword)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "mrstlev_UpdatePd";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Operator", xoperator);
            insertCommand.Parameters.AddWithValue("@Password", xpassword);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                MessageBox.Show("SQL access" + ex, "Update Password", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}