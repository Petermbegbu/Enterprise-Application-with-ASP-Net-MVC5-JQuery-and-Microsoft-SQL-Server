using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using msfunc;
using mradmin.BissClass;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

namespace mradmin.DataAccess
    
{
    public class ANC07B
    {
        public string REFERENCE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string TEMP { get; set; }
        public string PR { get; set; }
        public string BP { get; set; }
        public string SP02 { get; set; }
        public string UTERUS { get; set; }
        public string LOCIA { get; set; }
        public string WOUNDS { get; set; }
        public string PERINEUM { get; set; }
        public string URINE { get; set; }
        public string STAFFSIGN { get; set; }



        public static DataTable GetANC07B(string Reference)
        {
            ANC07B anc07b = new ANC07B();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC07B_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }


    }
}
