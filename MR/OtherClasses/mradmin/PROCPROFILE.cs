using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
//using System.Windows.Forms;
using msfunc;

namespace mradmin.DataAccess
{
    public class PROCPROFILE
    {
        public string REFERENCE { get; set; }
        public string PROCESS { get; set; }
        public decimal AMOUNT { get; set; }
        public string OTHERS { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool CAPITATED { get; set; }
        public bool AUTHORIZATIONREQUIRED { get; set; }
        //SCAN FOR procprofile.reference=custclass.reference AND procprofile.procedure = xitem    

        public static PROCPROFILE GetPROCPROFILE(string patcategory, string procedure)
        {
            PROCPROFILE procprof = new PROCPROFILE();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "PROCPROFILE_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", patcategory);
            selectCommand.Parameters.AddWithValue("@process", procedure);

            connection.Open();

            SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
            if (reader.Read())
            {

                procprof.REFERENCE = reader["reference"].ToString();
                procprof.PROCESS = reader["process"].ToString();
                procprof.AMOUNT = (Decimal)reader["amount"];
                procprof.OTHERS = reader["others"].ToString();
                procprof.POSTED = (bool)reader["posted"];
                procprof.POST_DATE = (DateTime)reader["post_date"];
                procprof.CAPITATED = (bool)reader["capitated"];
                procprof.AUTHORIZATIONREQUIRED = (bool)reader["authorizationrequired"];
            }
            reader.Close();

            connection.Close();

            return procprof;


        }
    }
}