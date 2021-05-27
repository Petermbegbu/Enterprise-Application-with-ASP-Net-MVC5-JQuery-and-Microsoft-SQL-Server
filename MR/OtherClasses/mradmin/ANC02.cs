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
    public class ANC02
    {
        public string REFERENCE { get; set; }
        public string PATIENTNO { get; set; }
        public string GROUPHEAD { get; set; }
        public string GROUPCODE { get; set; }
        public string GROUPHTYPE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string DIABETES { get; set; }
        public string HYPERTENSION { get; set; }
        public string HEART_DISEASE { get; set; }
        public string SICKLE_CELL { get; set; }
        public string PULMONARY { get; set; }
        public string KIDNEYDISEASE { get; set; }
        public string HEPATITIS { get; set; }
        public decimal PREV_PREG_TOTAL { get; set; }
        public decimal NOALIVE { get; set; }
        public string NEUROLOGIC { get; set; }
        public string THYROID { get; set; }
        public string PSYCHIATRIC { get; set; }
        public string DEPRESSION { get; set; }
        public string VARICOSITIES { get; set; }
        public string D_RH_SENSITIZATION { get; set; }
        public string BLOOD_TRANSFUSIONS { get; set; }
        public string HIV { get; set; }
        public string BREAST_LUMPS { get; set; }
        public string GYNESURGERIES { get; set; }
        public string DRUG_ALLERGIES { get; set; }
        public string OPERATIONS { get; set; }
        public string ANAESTHETIC { get; set; }
        public string PAPSMEAR { get; set; }
        public string INFERTILITY { get; set; }
        public string OTHERS { get; set; }
        public string ALCOHOL { get; set; }
        public string SMOKING { get; set; }
        public string SOCIALDETAILS { get; set; }
        public string FAM_HYPERTENSION { get; set; }
        public string FAM_DIABETES { get; set; }
        public string FAM_SICKLE_CELL { get; set; }
        public string FAM_GENETIC { get; set; }
        public string FAM_OTHERS { get; set; }
        public string FAMILYDETAILS { get; set; }
        public string AP_PROGUANIL { get; set; }
        public string AP_PYRIMETHAMINE1 { get; set; }
        public string AP_PYRIMETHAMINE2 { get; set; }
        public string AP_PYRIMETHAMINE3 { get; set; }
        public string AP_OTHERS { get; set; }
        public string TETANUS1 { get; set; }
        public string TETANUS2 { get; set; }
        public string TETANUS3 { get; set; }
        public string RECREATIONDRGS { get; set; }
        public string TWINNING { get; set; }

        public static ANC02 GetANC02(string Groupcode, string patientno)
        {
            ANC02 anc02 = new ANC02();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC02_Get"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Groupcode", Groupcode);
            selectCommand.Parameters.AddWithValue("@Patientno", patientno);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc02.REFERENCE = reader["reference"].ToString();
                    anc02.PATIENTNO = reader["patientno"].ToString();
                    anc02.GROUPHEAD = reader["grouphead"].ToString();
                    anc02.GROUPCODE = reader["groupcode"].ToString();
                    anc02.GROUPHTYPE = reader["grouphtype"].ToString();
                    anc02.POSTED = (bool)reader["posted"];
                    anc02.POST_DATE = (DateTime)reader["post_date"];
                    anc02.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc02.DIABETES = reader["diabetes"].ToString();
                    anc02.HYPERTENSION = reader["hypertension"].ToString();
                    anc02.HEART_DISEASE = reader["heart_disease"].ToString();
                    anc02.SICKLE_CELL = reader["sickle_cell"].ToString();
                    anc02.PULMONARY = reader["pulmonary"].ToString();
                    anc02.KIDNEYDISEASE = reader["kidneydisease"].ToString();
                    anc02.HEPATITIS = reader["hepatitis"].ToString();
                    anc02.PREV_PREG_TOTAL = (Decimal)reader["prev_preg_total"];
                    anc02.NOALIVE = (Decimal)reader["noalive"];
                    anc02.NEUROLOGIC = reader["neurologic"].ToString();
                    anc02.THYROID = reader["thyroid"].ToString();
                    anc02.PSYCHIATRIC = reader["psychiatric"].ToString();
                    anc02.DEPRESSION = reader["depression"].ToString();
                    anc02.VARICOSITIES = reader["varicosities"].ToString();
                    anc02.D_RH_SENSITIZATION = reader["d_rh_sensitization"].ToString();
                    anc02.BLOOD_TRANSFUSIONS = reader["blood_transfusions"].ToString();
                    anc02.HIV = reader["hiv"].ToString();
                    anc02.BREAST_LUMPS = reader["breast_lumps"].ToString();
                    anc02.GYNESURGERIES = reader["gynesurgeries"].ToString();
                    anc02.DRUG_ALLERGIES = reader["drug_allergies"].ToString();
                    anc02.OPERATIONS = reader["operations"].ToString();
                    anc02.ANAESTHETIC = reader["anaesthetic"].ToString();
                    anc02.PAPSMEAR = reader["papsmear"].ToString();
                    anc02.INFERTILITY = reader["infertility"].ToString();
                    anc02.OTHERS = reader["others"].ToString();
                    anc02.ALCOHOL = reader["alcohol"].ToString();
                    anc02.SMOKING = reader["smoking"].ToString();
                    anc02.SOCIALDETAILS = reader["socialdetails"].ToString();
                    anc02.FAM_HYPERTENSION = reader["fam_hypertension"].ToString();
                    anc02.FAM_DIABETES = reader["fam_diabetes"].ToString();
                    anc02.FAM_SICKLE_CELL = reader["fam_sickle_cell"].ToString();
                    anc02.FAM_GENETIC = reader["fam_genetic"].ToString();
                    anc02.FAM_OTHERS = reader["fam_others"].ToString();
                    anc02.FAMILYDETAILS = reader["familydetails"].ToString();
                    anc02.AP_PROGUANIL = reader["ap_proguanil"].ToString();
                    anc02.AP_PYRIMETHAMINE1 = reader["ap_pyrimethamine1"].ToString();
                    anc02.AP_PYRIMETHAMINE2 = reader["ap_pyrimethamine2"].ToString();
                    anc02.AP_PYRIMETHAMINE3 = reader["ap_pyrimethamine3"].ToString();
                    anc02.AP_OTHERS = reader["ap_others"].ToString();
                    anc02.TETANUS1 = reader["tetanus1"].ToString();
                    anc02.TETANUS2 = reader["tetanus2"].ToString();
                    anc02.TETANUS3 = reader["tetanus3"].ToString();
                    anc02.RECREATIONDRGS = reader["recreationdrgs"].ToString();
                    anc02.TWINNING = reader["twinning"].ToString();

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
                MessageBox.Show("" + ex, "Get ANC02 Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc02;
        }

    }
}