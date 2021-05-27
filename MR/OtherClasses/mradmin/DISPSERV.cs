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
    public class DISPSERV
    {
        public string REFERENCE { get; set; }
        public string DESCRIPTION { get; set; }
    


     public static DataTable GetDISPSERV()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "DISPSERV_GetList"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
    }
}