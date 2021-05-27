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
    public class SyscodesandTables
    {
       public char type_code { get; set; }
       public char name { get; set; }
       public char fldattrib { get; set; }


       public static DataTable getsyscode(string xrequired)
       {
           SqlConnection cs = Dataaccess.codeConnection();
           SqlCommand selectCommand = new SqlCommand(GetCommandtable(xrequired), cs);

           SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
           DataTable dt = new DataTable();

           ds.Fill(dt);
           cs.Close();
         //  DataRow row = dt.NewRow();
         //  dt.Rows.InsertAt(row, 0);

           return dt;
       }
       private static string GetCommandtable(string xtype)
       {
 
            return "SELECT type_code,name as Description,fldattrib FROM " + xtype;
       }
       public static bool DeleteSystemsCode(string syscode,string xtype)
       {

           SqlConnection connection = msmr.Dataaccess.codeConnection();
           SqlCommand deleteCommand = new SqlCommand();
           deleteCommand.CommandText = "SystemCodes_Delete";
           deleteCommand.Connection = connection;
           deleteCommand.CommandType = CommandType.StoredProcedure;

           deleteCommand.Parameters.AddWithValue("@Typecode", syscode);
           deleteCommand.Parameters.AddWithValue("@Control", xtype);           
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
               //throw ex;

               MessageBox.Show(""+ex, "Delete Systems Code "+syscode, MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
               return false;
           }
           finally
           {
               connection.Close();
           }
       }
       public static DataTable getglobalaccess() //string xoperator)
       {
           SqlConnection connection = msmr.Dataaccess.codeConnection();
           SqlCommand selectCommand = new SqlCommand();
           selectCommand.CommandText = "GlobalSys_Get";
           selectCommand.Connection = connection;
           selectCommand.CommandType = CommandType.StoredProcedure;

           //selectCommand.Parameters.AddWithValue("@OPERATOR", xoperator);
           
           SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
           DataTable dt = new DataTable();

           ds.Fill(dt);
           connection.Close();
           DataRow row = dt.NewRow();
           dt.Rows.InsertAt(row, 0);
           return dt;
       }
       public static bool DeleteGlobalAccess(string xoperator)
       {
           SqlConnection connection = msmr.Dataaccess.codeConnection();
           SqlCommand deleteCommand = new SqlCommand();
           deleteCommand.CommandText = "GlobalSys_Delete";
           deleteCommand.Connection = connection;
           deleteCommand.CommandType = CommandType.StoredProcedure;

           deleteCommand.Parameters.AddWithValue("@OPERATOR", xoperator);
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
               //throw ex;

               MessageBox.Show("" + ex, "Delete Acess to Systems ", MessageBoxButtons.OK,
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