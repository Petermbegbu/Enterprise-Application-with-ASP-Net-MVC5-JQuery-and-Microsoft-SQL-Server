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
    public class HMOSERVPROC
    {
       public string CUSTNO { get; set; }
        public string HMOSERVTYPE { get; set; }
        public string PROCESS { get; set; }
        public decimal AMOUNT { get; set; }
        public string OTHERS { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool CAPITATED { get; set; }
        public bool AUTHORIZATIONREQUIRED { get; set; }


        public static HMOSERVPROC GetHMOSERVPROC(string xcustno, string hmoservtype, string process)
        {
            HMOSERVPROC hmoserv = new HMOSERVPROC();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "HMOSERVPROC_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Custno", xcustno);
            selectCommand.Parameters.AddWithValue("@Hmoservtype", hmoservtype);
            selectCommand.Parameters.AddWithValue("@process", process);
           
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    hmoserv.CUSTNO = reader["custno"].ToString();
                    hmoserv.HMOSERVTYPE = reader["hmoservtype"].ToString();
                    hmoserv.PROCESS = reader["process"].ToString();
                    hmoserv.AMOUNT = (Decimal)reader["amount"];
                    hmoserv.OTHERS = reader["others"].ToString();
                    hmoserv.POSTED = (bool)reader["posted"];
                    hmoserv.POST_DATE = (DateTime)reader["post_date"];
                    hmoserv.CAPITATED = (bool)reader["capitated"];
                    hmoserv.AUTHORIZATIONREQUIRED = (bool)reader["authorizationrequired"];


                }
                reader.Close();
            
                connection.Close();
                
            return hmoserv;
        }
    }
}