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
    public class MRB15
    {
        public string REFERENCE { get; set; }
        public string SDOW { get; set; }
        public string START { get; set; }
        public string TEND { get; set; }
        public string PATCAT { get; set; }
        public decimal PMARK { get; set; }
        public decimal AMOUNT { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string DIAG { get; set; }


        public static DataTable GetMRB15(string xreference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "MRB15_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", xreference);
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static bool DeleteItem(string xreference,int xitemno)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "MRB15_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Reference", xreference);
            deleteStatement.Parameters.AddWithValue("@Itemno", xitemno);
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