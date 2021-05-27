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
    public class LABTRANS
    {
        public string REFERENCE { get; set; }
        public decimal ITEMNO { get; set; }
        public string REF_SYMPTO { get; set; }
        public string PROCESS { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string RADRPT { get; set; }
        public string FACILITY { get; set; }
        public decimal AMOUNT { get; set; }
        public string TESTBY { get; set; }
        public DateTime TESTDATE { get; set; }
        public decimal FCAMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public int RECID { get; set; }

     public static DataTable GetLABTRANS(string Reference,string facilitycode)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "LABTRANS_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            selectCommand.Parameters.AddWithValue("@Facility", facilitycode);
           SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }


    }
}