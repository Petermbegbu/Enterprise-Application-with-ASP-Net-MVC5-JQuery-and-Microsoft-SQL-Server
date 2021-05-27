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
    public class BSDETAIL
    {
        public string REFERENCE { get; set; }
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public bool SIGNEDBILL { get; set; }
        public string GROUPHEAD { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public decimal AMOUNT { get; set; }
        public string AUTH_CODE { get; set; }
        public DateTime AUTH_DATE { get; set; }

        public static BSDETAIL GetBSDETAIL(string xreference)
        {
            BSDETAIL bsdet = new BSDETAIL();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BSDETAIL_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", xreference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    bsdet.REFERENCE = reader["reference"].ToString();
                    bsdet.GROUPCODE = reader["groupcode"].ToString();
                    bsdet.PATIENTNO = reader["patientno"].ToString();
                    bsdet.NAME = reader["name"].ToString();
                    bsdet.TRANS_DATE = (DateTime)reader["trans_date"];
                    bsdet.SIGNEDBILL = (bool)reader["signedbill"];
                    bsdet.GROUPHEAD = reader["grouphead"].ToString();
                    bsdet.OPERATOR = reader["operator"].ToString();
                    bsdet.DTIME = (DateTime)reader["dtime"];
                    bsdet.AMOUNT = (Decimal)reader["amount"];
                    bsdet.AUTH_CODE = reader["auth_code"].ToString();
                    bsdet.AUTH_DATE = (DateTime)reader["auth_date"];
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
                MessageBox.Show("" + ex, "Get Bill Signing Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }
            return bsdet;
        }
    }
}