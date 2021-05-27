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
    public class GLINTAB3
    {
        public string FACILITY { get; set; }
        public string NAME { get; set; }
        public string CURRENCY { get; set; }
        public string COMPANY { get; set; }
        public decimal BATCHNO { get; set; }
        public string DOCUMENT { get; set; }
        public string PVTBDEBIT { get; set; }
        public string PVTBCREDIT { get; set; }
        public string PVTCASHCREDIT { get; set; }
        public string PVTCASHDEBIT { get; set; }
        public string PVTCHQDEBIT { get; set; }
        public string PVTCHQCREDIT { get; set; }
        public string COPBCREDIT { get; set; }
        public string COPBDEBIT { get; set; }
        public string COPCASHDEBIT { get; set; }
        public string COPCASHCREDIT { get; set; }
        public string COPCHQDEBIT { get; set; }
        public string COPCHQCREDIT { get; set; }
        public string HMOCLM_BDEBIT { get; set; }
        public string HMOCLM_BCREDIT { get; set; }
        public string HMOCLM_CASHDEBIT { get; set; }
        public string HMOCLM_CASHCREDIT { get; set; }
        public string HMOCLM_CHQDEBIT { get; set; }
        public string HMOCLM_CHQCREDIT { get; set; }
        public string HMOCAP_BDEBIT { get; set; }
        public string HMOCAP_BCREDIT { get; set; }
        public string HMOCAP_CASHDEBIT { get; set; }
        public string HMOCAP_CASHCREDIT { get; set; }
        public string HMOCAP_CHQDEBIT { get; set; }
        public string HMOCAP_CHQCREDIT { get; set; }
        public string NHISCLM_BDEBIT { get; set; }
        public string NHISCLM_BCREDIT { get; set; }
        public string NHISCLM_CASHDEBIT { get; set; }
        public string NHISCLM_CASHCREDIT { get; set; }
        public string NHISCLM_CHQDEBIT { get; set; }
        public string NHISCLM_CHQCREDIT { get; set; }
        public string NHISCAP_BDEBIT { get; set; }
        public string NHISCAP_BCREDIT { get; set; }
        public string NHISCAP_CASHDEBIT { get; set; }
        public string NHISCAP_CASHCREDIT { get; set; }
        public string NHISCAP_CHQDEBIT { get; set; }
        public string NHISCAP_CHQCREDIT { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }


        public static DataTable GetGLINTAB3()
        {
           // GLINTAB3 glintab3 = new GLINTAB3();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "GLINTAB3_GetList"; //"spGetPatient";
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
 /*           selectCommand.Parameters.AddWithValue("@GroupCodeID", GroupCodeID);
            selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    glintab3.FACILITY = reader["facility"].ToString();
                    glintab3.NAME = reader["name"].ToString();
                    glintab3.CURRENCY = reader["currency"].ToString();
                    glintab3.COMPANY = reader["company"].ToString();
                    glintab3.BATCHNO = (Decimal)reader["batchno"];
                    glintab3.DOCUMENT = reader["document"].ToString();
                    glintab3.PVTBDEBIT = reader["pvtbdebit"].ToString();
                    glintab3.PVTBCREDIT = reader["pvtbcredit"].ToString();
                    glintab3.PVTCASHDEBIT = reader["pvtcashdebit"].ToString();
                    glintab3.PVTCASHCREDIT = reader["pvtcashcredit"].ToString();
                    glintab3.PVTCHQDEBIT = reader["pvtchqdebit"].ToString();
                    glintab3.PVTCHQCREDIT = reader["pvtchqcredit"].ToString();
                    glintab3.COPBDEBIT = reader["copbdebit"].ToString();
                    glintab3.COPBCREDIT = reader["copbcredit"].ToString();
                    glintab3.COPCASHDEBIT = reader["copcashdebit"].ToString();
                    glintab3.COPCASHCREDIT = reader["copcashcredit"].ToString();
                    glintab3.COPCHQDEBIT = reader["copchqdebit"].ToString();
                    glintab3.COPCHQCREDIT = reader["copchqcredit"].ToString();
                    glintab3.HMOCLM_BDEBIT = reader["hmoclm_bdebit"].ToString();
                    glintab3.HMOCLM_BCREDIT = reader["hmoclm_bcredit"].ToString();
                    glintab3.HMOCLM_CASHDEBIT = reader["hmoclm_cashdebit"].ToString();
                    glintab3.HMOCLM_CASHCREDIT = reader["hmoclm_cashcredit"].ToString();
                    glintab3.HMOCLM_CHQDEBIT = reader["hmoclm_chqdebit"].ToString();
                    glintab3.HMOCLM_CHQCREDIT = reader["hmoclm_chqcredit"].ToString();
                    glintab3.HMOCAP_BDEBIT = reader["hmocap_bdebit"].ToString();
                    glintab3.HMOCAP_BCREDIT = reader["hmocap_bcredit"].ToString();
                    glintab3.HMOCAP_CASHDEBIT = reader["hmocap_cashdebit"].ToString();
                    glintab3.HMOCAP_CASHCREDIT = reader["hmocap_cashcredit"].ToString();
                    glintab3.HMOCAP_CHQDEBIT = reader["hmocap_chqdebit"].ToString();
                    glintab3.HMOCAP_CHQCREDIT = reader["hmocap_chqcredit"].ToString();
                    glintab3.NHISCLM_BDEBIT = reader["nhisclm_bdebit"].ToString();
                    glintab3.NHISCLM_BCREDIT = reader["nhisclm_bcredit"].ToString();
                    glintab3.NHISCLM_CASHDEBIT = reader["nhisclm_cashdebit"].ToString();
                    glintab3.NHISCLM_CASHCREDIT = reader["nhisclm_cashcredit"].ToString();
                    glintab3.NHISCLM_CHQDEBIT = reader["nhisclm_chqdebit"].ToString();
                    glintab3.NHISCLM_CHQCREDIT = reader["nhisclm_chqcredit"].ToString();
                    glintab3.NHISCAP_BDEBIT = reader["nhiscap_bdebit"].ToString();
                    glintab3.NHISCAP_BCREDIT = reader["nhiscap_bcredit"].ToString();
                    glintab3.NHISCAP_CASHDEBIT = reader["nhiscap_cashdebit"].ToString();
                    glintab3.NHISCAP_CASHCREDIT = reader["nhiscap_cashcredit"].ToString();
                    glintab3.NHISCAP_CHQDEBIT = reader["nhiscap_chqdebit"].ToString();
                    glintab3.NHISCAP_CHQCREDIT = reader["nhiscap_chqcredit"].ToString();
                    glintab3.POSTED = (bool)reader["posted"];
                    glintab3.POST_DATE = (DateTime)reader["post_date"];

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

            return glintab3;
        }

    }
}*/