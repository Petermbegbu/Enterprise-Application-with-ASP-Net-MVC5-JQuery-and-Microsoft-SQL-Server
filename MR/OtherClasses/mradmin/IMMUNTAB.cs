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
    public class IMMUNTAB
    {
        public string PROCESS { get; set; }
        public decimal DAYSFROM { get; set; }
        public decimal DAYSTO { get; set; }
        public bool FROMBIRTH { get; set; }
        public bool FROMREG { get; set; }
        public decimal AGELIMIT { get; set; }
        public string TYPE { get; set; }
        public bool COMPULSORY { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool FROMLASTDOSE { get; set; }
        public string STOCKCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal BASIC_COST { get; set; }
        public bool CORP_DIFF_CHG { get; set; }
        public bool GLOBAL_DIFF_CHG { get; set; }
        public decimal QTY_REQD { get; set; }
        public string NAME { get; set; }
        public string STORE { get; set; }
        public int RECID { get; set; }
 
        public static DataTable GetImmuntab()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "IMMUNTAB_GetList"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;
            
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static IMMUNTAB GetImmuntabbyProcess(string process)
        {
            IMMUNTAB immuntab = new IMMUNTAB();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "IMMUNTAB_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@PROCESS", process);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    immuntab.PROCESS = reader["process"].ToString();
                    immuntab.DAYSFROM = (Decimal)reader["daysfrom"];
                    immuntab.DAYSTO = (Decimal)reader["daysto"];
                    immuntab.FROMBIRTH = (bool)reader["frombirth"];
                    immuntab.FROMREG = (bool)reader["fromreg"];
                    immuntab.AGELIMIT = (Decimal)reader["agelimit"];
                    immuntab.TYPE = reader["type"].ToString();
                    immuntab.COMPULSORY = (bool)reader["compulsory"];
                    immuntab.POSTED = (bool)reader["posted"];
                    immuntab.POST_DATE = (DateTime)reader["post_date"];
                    immuntab.FROMLASTDOSE = (bool)reader["fromlastdose"];
                    immuntab.STOCKCODE = reader["stockcode"].ToString();
                    immuntab.DESCRIPTION = reader["description"].ToString();
                    immuntab.BASIC_COST = (Decimal)reader["basic_cost"];
                    immuntab.CORP_DIFF_CHG = (bool)reader["corp_diff_chg"];
                    immuntab.GLOBAL_DIFF_CHG = (bool)reader["global_diff_chg"];
                    immuntab.QTY_REQD = (Decimal)reader["qty_reqd"];
                    immuntab.NAME = reader["name"].ToString();
                    immuntab.STORE = reader["store"].ToString();
                    immuntab.RECID = Convert.ToInt32(reader["recid"]);
                }
                else
                {
                    connection.Close();
                    return null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Get Immuntab by process ", MessageBoxButtons.OK,
                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }
            return immuntab;
        }
        public static bool DeleteImmunizationTAB(string PROCESS)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "IMMUNTAB_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@process", PROCESS);
            try
            {
                connection.Open();
                int count = deleteStatement.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException ex)
            {
                //throw ex;

                MessageBox.Show(" " + ex, "Delete Immunization Tab Item", MessageBoxButtons.OK,
    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }

        }
    }
}