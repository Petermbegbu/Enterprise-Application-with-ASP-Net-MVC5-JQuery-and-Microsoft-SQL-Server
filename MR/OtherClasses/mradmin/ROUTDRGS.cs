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
    public class ROUTDRGS
    {
        public string REFERENCE { get; set; }
        public string DRUGS { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal QTY { get; set; }
        public string UNIT { get; set; }
        public decimal COST { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool GLOBAL_DIFF_CHG { get; set; }
        public bool CORP_DIFF_CHG { get; set; }
        public decimal DOSE { get; set; }
        public decimal INTERVAL { get; set; }
        public decimal DURATION { get; set; }
        public string CDOSE { get; set; }
        public string CINTERVAL { get; set; }
        public string CDURATION { get; set; }
        public string WHENHOW { get; set; }

        public static ROUTDRGS GetROUTDRGS(string reference)
        {
            ROUTDRGS routd = new ROUTDRGS();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ROUTDRGS_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    routd.REFERENCE = reader["reference"].ToString();
                    routd.DRUGS = reader["drugs"].ToString();
                    routd.DESCRIPTION = reader["description"].ToString();
                    routd.QTY = (Decimal)reader["qty"];
                    routd.UNIT = reader["unit"].ToString();
                    routd.COST = (Decimal)reader["cost"];
                    routd.POSTED = (bool)reader["posted"];
                    routd.POST_DATE = (DateTime)reader["post_date"];
                    routd.GLOBAL_DIFF_CHG = (bool)reader["global_diff_chg"];
                    routd.CORP_DIFF_CHG = (bool)reader["corp_diff_chg"];
                    routd.DOSE = (Decimal)reader["dose"];
                    routd.INTERVAL = (Decimal)reader["interval"];
                    routd.DURATION = (Decimal)reader["duration"];
                    routd.CDOSE = reader["cdose"].ToString();
                    routd.CINTERVAL = reader["cinterval"].ToString();
                    routd.CDURATION = reader["cduration"].ToString();
                    routd.WHENHOW = reader["whenhow"].ToString();

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

            return routd;
        }
        /// <summary>
        /// gets all definitions
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static DataTable GetROUTDRGS()
        {
            // Billings billing = new Billings();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ROUTDRGS_Getlist"; //"spGetPatient";
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