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
    public class SPCDETAIL
    {
       public string REFERENCE { get; set; }
       public string FACILITY { get; set; }
       public string CUSTOMER { get; set; }
       public string NAME { get; set; }
       public decimal AMOUNT { get; set; }
       public bool POSTED { get; set; }
       public DateTime POST_DATE { get; set; }

       public static DataTable GetSPCDETAIL(string xfacility) //, string xcustomer)
        {

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "SPCDETAIL_Get"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Facility", xfacility);
            //selectCommand.Parameters.AddWithValue("@Customer", xcustomer);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
       public static bool DeleteItem(string xfacility,string xcustomer)
       {
           SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
           SqlCommand deleteStatement = new SqlCommand();
           deleteStatement.CommandText = "SPCDETAIL_Delete";
           deleteStatement.Connection = connection;
           deleteStatement.CommandType = CommandType.StoredProcedure;

           deleteStatement.Parameters.AddWithValue("@Facility", xfacility);
           deleteStatement.Parameters.AddWithValue("@Customer", xcustomer);
           try
           {
               connection.Open();
               int count = deleteStatement.ExecuteNonQuery();
               connection.Close();
               if (count > 0)
                   return true;
               else
                   return false;

           }
           catch (SqlException ex)
           {
               MessageBox.Show(" " + ex, "Delete Company on Specialist Consult Details", MessageBoxButtons.OK,
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