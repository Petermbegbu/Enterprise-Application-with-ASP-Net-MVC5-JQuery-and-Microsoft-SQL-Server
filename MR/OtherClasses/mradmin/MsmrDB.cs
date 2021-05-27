using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web;

namespace msmr
{
    public static class Dataaccess
    {
        /*  public static SqlConnection mrGetConnection()
           {
              SqlConnection conn = new SqlConnection();
               conn.ConnectionString = ConfigurationManager.ConnectionStrings["mrDBConnector"].ConnectionString;

                return conn;
         * 
         * SqlConnection sqlConnection = 
    new SqlConnection(@"Data Source=SERVER_NAME;Initial Catalog=TABLE_NAME;Integrated Security=True");
   sqlConnection.Open();
   SqlCommand sqlCommand = thisConnection.CreateCommand();
   sqlCommand.CommandText = "SELECT MAX(Salary) Salary FROM STUFF";
   SqlDataReader reader = thisCommand.ExecuteReader();
   reader.Read();
   Int32 maxSalary = (Int32)reader["Salary"];
         * 
         * 
          } */
        public static SqlConnection mrConnection()
        {
            //string mrConnectionString = "Data Source=localhost\\norbert-pc;Initial Catalog=MR_DATA;" +
             //   "Integrated Security=True";
            string mrConnectionString = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=MR_DATA;Data Source=localhost";
            SqlConnection mrconnection = new SqlConnection(mrConnectionString);
            return mrconnection;
            //Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=MR_DATA;Data Source=localhost
            //SqlConnection conn = new SqlConnection();
            //conn.ConnectionString = ConfigurationManager.ConnectionStrings["mrconnect"].ConnectionString;

            //return conn;

        }
          
       public static SqlConnection codeConnection()
       {

           string codeConnectionString = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=SYSCODETABS;Data Source=localhost";
           SqlConnection codeconnection = new SqlConnection(codeConnectionString);
           return codeconnection;
           //string codeConnectionString = "Data Source=localhost\\norbert-pc;Initial Catalog=SYSCODETABS;" +
           //   "Integrated Security=True";

           //SqlConnection codeconnection = new SqlConnection(codeConnectionString);

           //return codeconnection;
         /*  SqlConnection conn = new SqlConnection();
           conn.ConnectionString = ConfigurationManager.ConnectionStrings["codDBConnector"].ConnectionString;

           return conn; */
       }

       public static SqlConnection hrConnection()
       {
/*           SqlConnection hrconnection = new SqlConnection();
           hrconnection.ConnectionString = ConfigurationManager.ConnectionStrings["hrDBConnector"].ConnectionString;*/
           string hrConnectionString = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=PAYPER;Data Source=localhost";
           SqlConnection hrConnection = new SqlConnection(hrConnectionString);

           return hrConnection;
       }
       public static SqlConnection stkConnection()
       {
         //  SqlConnection stkConnection = new SqlConnection();
         // stkConnection.ConnectionString = ConfigurationManager.ConnectionStrings["stkDBConnector"].ConnectionString;
           string stkConnectionString = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=SCS01;Data Source=localhost";
           SqlConnection stkConnection = new SqlConnection(stkConnectionString);
           return stkConnection;

       }
       public static SqlConnection glConnection()
       {
          /* SqlConnection glConnection = new SqlConnection();
           glConnection.ConnectionString = ConfigurationManager.ConnectionStrings["glsconnect"].ConnectionString;

           return glConnection; */
           string glconnection = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=GLS01;Data Source=localhost";
           SqlConnection glConnection = new SqlConnection(glconnection);
           return glConnection;
       }
       public static SqlConnection faConnection()
       {
         /*  SqlConnection faConnection = new SqlConnection();
           faConnection.ConnectionString = ConfigurationManager.ConnectionStrings["faDBConnector"].ConnectionString;*/
           string faconnection = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=FAS01;Data Source=localhost";
           SqlConnection faConnection = new SqlConnection(faconnection);

           return faConnection;
       }
       public static SqlConnection apConnection()
       {
         /*  SqlConnection apConnection = new SqlConnection();
           apConnection.ConnectionString = ConfigurationManager.ConnectionStrings["apDBConnector"].ConnectionString;*/
           string apconnection = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=APS01;Data Source=localhost";
           SqlConnection apConnection = new SqlConnection(apconnection);


           return apConnection;
       }
       public static SqlConnection arConnection()
       {
          /* SqlConnection arConnection = new SqlConnection();
           arConnection.ConnectionString = ConfigurationManager.ConnectionStrings["arDBConnector"].ConnectionString;*/
           string arconnection = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=ARS01;Data Source=localhost";
           SqlConnection arConnection = new SqlConnection(arconnection);

           return arConnection;
       }

    }
}