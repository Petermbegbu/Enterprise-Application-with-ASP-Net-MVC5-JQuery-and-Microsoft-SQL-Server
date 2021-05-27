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
    public class custclass
    {
        public string REFERENCE { get; set; }
        public string NAME { get; set; }
        public decimal PERCENTAGE { get; set; }
        public bool DEFINEDRGS { get; set; }
        public bool DRGRESTRICTIVE { get; set; }
        public bool DEFINEPROC { get; set; }
        public bool PROCRESTRICTIVE { get; set; }
        public bool DRGINCLUSIVE { get; set; }
        public bool PROCINCLUSIVE { get; set; }
        public decimal CREDITLIMIT { get; set; }



        public static DataTable GetCUSTCLASS() //string GroupCodeID, string PatientID)
        {
            custclass custcls = new custclass();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "CUSTCLASS_GetList"; //"spGetPatient";
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
 /*        //   selectCommand.Parameters.AddWithValue("@GroupCodeID", GroupCodeID);
         //   selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    custcls.REFERENCE = reader["reference"].ToString();
                    custcls.NAME = reader["name"].ToString();
                    custcls.PERCENTAGE = (Decimal)reader["percentage"];
                    custcls.DEFINEDRGS = (bool)reader["definedrgs"];
                    custcls.DRGRESTRICTIVE = (bool)reader["drgestrictive"];
                    custcls.DEFINEPROC = (bool)reader["defineproc"];
                    custcls.PROCRESTRICTIVE = (bool)reader["procrestricitive"];
                    custcls.DRGINCLUSIVE = (bool)reader["drginclusive"];
                    custcls.PROCINCLUSIVE = (bool)reader["procinclusive"];
                    custcls.CREDITLIMIT = (Decimal)reader["creditlimit"];


                }
                else
                {
                    connection.Close();
                    return null;

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show("" + ex, "Get Patient Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return custcls;
        }

        */
