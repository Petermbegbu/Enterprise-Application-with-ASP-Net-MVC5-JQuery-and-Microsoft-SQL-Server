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
    public class HMOSERVIC
    {
        public string CUSTNO { get; set; }
        public string HMOSERVTYPE { get; set; }
        public string DRUGS { get; set; }
        public decimal AMOUNT { get; set; }
        public string OTHERS { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool CAPITATED { get; set; }
        public bool AUTHORIZATIONREQUIRED { get; set; }


        public static HMOSERVIC GetHMOSERVIC(string xcustno, string hmoservtype, string xdrug)
        {
            HMOSERVIC hmoservic = new HMOSERVIC();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "HMOSERVIC_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Custno", xcustno);
            selectCommand.Parameters.AddWithValue("@Hmoservtype", hmoservtype );
            selectCommand.Parameters.AddWithValue("@Drugs", xdrug );
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    hmoservic.CUSTNO = reader["custno"].ToString();
                    hmoservic.HMOSERVTYPE = reader["hmoservtype"].ToString();
                    hmoservic.DRUGS = reader["drugs"].ToString();
                    hmoservic.AMOUNT = (Decimal)reader["amount"];
                    hmoservic.OTHERS = reader["others"].ToString();
                    hmoservic.POSTED = (bool)reader["posted"];
                    hmoservic.POST_DATE = (DateTime)reader["post_date"];
                    hmoservic.CAPITATED = (bool)reader["capitated"];
                    hmoservic.AUTHORIZATIONREQUIRED = (bool)reader["authorizationrequired"];


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
                MessageBox.Show("" + ex, "Get HMO Drug Charge ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return hmoservic;
        }

    }
}