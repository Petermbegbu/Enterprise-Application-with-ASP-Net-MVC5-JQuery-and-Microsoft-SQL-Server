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
    public class ATMPROFILE
    {
        public string NAME { get; set; }
        public string CURRENCY { get; set; }
        public string COMPANY { get; set; }
        public decimal BATCHNO { get; set; }
        public string DOCUMENT { get; set; }
        public string DEBIT { get; set; }
        public string CREDIT { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }


        public static DataTable GetATMPROFILE()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ATMPROFILE_GetList"; //"spGetPatient";
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