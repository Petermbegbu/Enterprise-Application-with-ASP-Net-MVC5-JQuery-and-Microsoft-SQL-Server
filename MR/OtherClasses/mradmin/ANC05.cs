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
    public class ANC05
    {
        public string REFERENCE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string URINE { get; set; }
        public string HB_LEVEL { get; set; }
        public string BLOOD_GENOTYPE { get; set; }
        public string BP { get; set; }
        public string BLOOD_GP_P_RH { get; set; }
        public string XRAY { get; set; }
        public string BREASTS_NIPPLES { get; set; }
        public string HISTORYOFPRESENT_PREG { get; set; }
        public string PHYSICALEXAM { get; set; }
        public string RESPIRATORYSYSTEM { get; set; }
        public string CARDIO_VASCULAR { get; set; }
        public string HEIGHT { get; set; }
        public string SPLEEN { get; set; }
        public string WEIGHT { get; set; }
        public string LIVER { get; set; }
        public string VAGINAL_EXAM { get; set; }
        public string COMMENTS { get; set; }
        public string ABDOMEN { get; set; }
        public string SPECIALINSTRUCTIONS { get; set; }
        public string DOCTOR { get; set; }
        public string XRAYSCAN { get; set; }


        public static ANC05 GetANC05(string Reference)
        {
            ANC05 anc05 = new ANC05();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC05_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);

            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc05.REFERENCE = reader["reference"].ToString();
                    anc05.POSTED = (bool)reader["posted"];
                    anc05.POST_DATE = (DateTime)reader["post_date"];
                    anc05.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc05.URINE = reader["urine"].ToString();
                    anc05.HB_LEVEL = reader["hb_level"].ToString();
                    anc05.BLOOD_GENOTYPE = reader["blood_genotype"].ToString();
                    anc05.BP = reader["bp"].ToString();
                    anc05.BLOOD_GP_P_RH = reader["blood_gp_p_rh"].ToString();
                    anc05.XRAY = reader["xray"].ToString();
                    anc05.BREASTS_NIPPLES = reader["breasts_nipples"].ToString();
                    anc05.HISTORYOFPRESENT_PREG = reader["historyofpresent_preg"].ToString();
                    anc05.PHYSICALEXAM = reader["physicalexam"].ToString();
                    anc05.RESPIRATORYSYSTEM = reader["respiratorysystem"].ToString();
                    anc05.CARDIO_VASCULAR = reader["cardio_vascular"].ToString();
                    anc05.HEIGHT = reader["height"].ToString();
                    anc05.SPLEEN = reader["spleen"].ToString();
                    anc05.WEIGHT = reader["weight"].ToString();
                    anc05.LIVER = reader["liver"].ToString();
                    anc05.VAGINAL_EXAM = reader["vaginal_exam"].ToString();
                    anc05.COMMENTS = reader["comments"].ToString();
                    anc05.ABDOMEN = reader["abdomen"].ToString();
                    anc05.SPECIALINSTRUCTIONS = reader["specialinstructions"].ToString();
                    anc05.DOCTOR = reader["doctor"].ToString();
                    anc05.XRAYSCAN = reader["xrayscan"].ToString();



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
                MessageBox.Show("" + ex, "Get ANC05 Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc05;
        }


    }
}