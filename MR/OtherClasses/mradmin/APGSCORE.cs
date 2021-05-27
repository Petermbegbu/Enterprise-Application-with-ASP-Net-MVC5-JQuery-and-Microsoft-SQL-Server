using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using msfunc;
using mradmin.BissClass;


using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

namespace mradmin.DataAccess
{
    public class APGARSCORE
    {
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public string HEART_RATE_1 { get; set; }
        public string RESPIRATIONEFF_1 { get; set; }
        public string MUSCLE_TONE_1 { get; set; }
        public string REFLEX_1 { get; set; }
        public string COLOR_1 { get; set; }
        public string TOTAL_1 { get; set; }
        public string HEART_RATE_5 { get; set; }
        public string RESPIRATIONEFF_5 { get; set; }
        public string MUSCLE_TONE_5 { get; set; }
        public string REFLEX_5 { get; set; }
        public string COLOR_5 { get; set; }
        public string TOTAL_5 { get; set; }
        public string HEART_RATE_OTH { get; set; }
        public string RESPIRATIONEFF_OTH { get; set; }
        public string MUSCLE_TONE_OTH { get; set; }
        public string REFLEX_OTH { get; set; }
        public string COLOR_OTH { get; set; }
        public string TOTAL_OTH { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string REFERENCE { get; set; }


        public static APGARSCORE GetAPGARSCORE(string Reference)
        {
            APGARSCORE apgscore = new APGARSCORE();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "APGARSCORE_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    apgscore.REFERENCE = reader["reference"].ToString();
                    apgscore.GROUPCODE = reader["groupcode"].ToString();
                    apgscore.HEART_RATE_1 = reader["heart_rate_1"].ToString();
                    apgscore.RESPIRATIONEFF_1 = reader["respirationeff_1"].ToString();
                    apgscore.MUSCLE_TONE_1 = reader["muscle_tone_1"].ToString();
                    apgscore.REFLEX_1 = reader["reflex_1"].ToString();
                    apgscore.COLOR_1 = reader["color_1"].ToString();
                    apgscore.TOTAL_1 = reader["total_1"].ToString();
                    apgscore.HEART_RATE_5 = reader["heart_rate_5"].ToString();
                    apgscore.MUSCLE_TONE_5 = reader["muscle_tone_5"].ToString();
                    apgscore.REFLEX_5 = reader["reflex_5"].ToString();
                    apgscore.COLOR_5 = reader["color_5"].ToString();
                    apgscore.TOTAL_5 = reader["total_5"].ToString();
                    apgscore.HEART_RATE_OTH = reader["heart_rate_oth"].ToString();
                    apgscore.RESPIRATIONEFF_OTH = reader["respirationeff_oth"].ToString();
                    apgscore.MUSCLE_TONE_OTH = reader["muscle_tone_oth"].ToString();
                    apgscore.REFLEX_OTH = reader["reflex_oth"].ToString();
                    apgscore.COLOR_OTH = reader["color_oth"].ToString();
                    apgscore.TOTAL_OTH = reader["total_oth"].ToString();
                    apgscore.POSTED = (bool)reader["posted"];
                    apgscore.POST_DATE = (DateTime)reader["post_date"];
                    apgscore.OPERATOR = reader["operstor"].ToString();
                    apgscore.DTIME = (DateTime)reader["dtime"];
                    apgscore.REFERENCE = reader["reference"].ToString();




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
                MessageBox.Show("" + ex, "Get APGSCORE Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return apgscore;

        }


    }
}