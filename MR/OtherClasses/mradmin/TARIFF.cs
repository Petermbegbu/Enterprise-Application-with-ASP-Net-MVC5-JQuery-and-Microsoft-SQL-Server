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
    public class TARIFF
    {
        public string REFERENCE { get; set; }
        public string NAME { get; set; }
        public string STATMT_DES { get; set; }
        public string CATEGORY { get; set; }
        public string SUBCATEGORY { get; set; }
        public decimal AMOUNT { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal SEC_LEVEL { get; set; }
        public decimal CATP { get; set; }
        public decimal CATC { get; set; }
        public decimal CATV { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool DIFFCHARGE { get; set; }
        public bool AVAILABLE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }

        public static TARIFF GetTARIFF(string xreference)
        {
            TARIFF tariff = new TARIFF();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "TARIFF_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", xreference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    tariff.REFERENCE = reader["reference"].ToString();
                    tariff.NAME = reader["name"].ToString();
                    tariff.STATMT_DES = reader["statmt_des"].ToString();
                    tariff.CATEGORY = reader["category"].ToString();
                    tariff.SUBCATEGORY = reader["subcategory"].ToString();
                    tariff.AMOUNT = (Decimal)reader["amount"];
                    tariff.TRANS_DATE = (DateTime)reader["trans_date"];
                    tariff.SEC_LEVEL = (Decimal)reader["sec_level"];
                    tariff.CATP = (Decimal)reader["catp"];
                    tariff.CATC = (Decimal)reader["catc"];
                    tariff.CATV = (Decimal)reader["catv"];
                    tariff.POSTED = (bool)reader["posted"];
                    tariff.POST_DATE = (DateTime)reader["post_date"];
                    tariff.DIFFCHARGE = (bool)reader["diffcharge"];
                    tariff.AVAILABLE = (bool)reader["available"];
                    tariff.OPERATOR = reader["operator"].ToString();
                    tariff.DTIME = (DateTime)reader["dtime"];
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
                MessageBox.Show("" + ex, "Get Tariff Details ", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    connection.Close();
                    return null;
            }
            finally
            {
                connection.Close();
            }
            return tariff;
        }
        /// <summary>
        /// get all reference and description only
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static DataTable GetTariffAll()
        {

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection(); 
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "Tariff_Getlist"; //"spGetPatient";
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