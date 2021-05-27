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
    public class ANC09
    {
        public bool HIV { get; set; }
        public string HIV_TEXT { get; set; }
        public bool RISKFACTOR { get; set; }
        public string RISKFA_TEXT { get; set; }
        public bool PRENATAL { get; set; }
        public string PRENAT_TEXT { get; set; }
        public bool NUTRITION { get; set; }
        public string NUTRI_TEXT { get; set; }
        public bool SEXUAL { get; set; }
        public string SEXU_TEXT { get; set; }
        public bool EXERCISE { get; set; }
        public string EXERC_TEXT { get; set; }
        public bool ENVIRONMENTAL { get; set; }
        public string ENVIRO_TEXT { get; set; }
        public bool TOBACCO { get; set; }
        public string TOBACC_TEXT { get; set; }
        public bool ALCOHOL { get; set; }
        public string ALCOHO_TEXT { get; set; }
        public bool ILLICITDRUGS { get; set; }
        public string IL_DRG_TEXT { get; set; }
        public bool MEDICATIONS { get; set; }
        public string MEDIC_TEXT { get; set; }
        public bool ULTRASOUND { get; set; }
        public string ULTRAS_TEXT { get; set; }
        public bool DOMESTIC { get; set; }
        public string DOMES_TEXT { get; set; }
        public bool SEATBELT { get; set; }
        public string SEATBELT_TEXT { get; set; }
        public bool CHILDBIRTH { get; set; }
        public string CHILDB_TEXT { get; set; }
        public bool PRETERM { get; set; }
        public string PRETER_TEXT { get; set; }
        public bool ABNORMAL { get; set; }
        public string AB_LAB_TEXT { get; set; }
        public bool POSTPARTUM_FAM_PLAN { get; set; }
        public string PPARTUM_FAM_PLANNING { get; set; }
        public bool ANAESTHESIA { get; set; }
        public string ANAES_TEXT { get; set; }
        public bool FETAL { get; set; }
        public string FETAL_TEXT { get; set; }
        public bool LABOURSIGNS { get; set; }
        public string LABOU_TEXT { get; set; }
        public bool VBAC { get; set; }
        public string VBAC_TEXT { get; set; }
        public bool PIH { get; set; }
        public string PIH_TEXT { get; set; }
        public bool POSTTERM { get; set; }
        public string POSTT_TEXT { get; set; }
        public bool CIRCUMCISION { get; set; }
        public string CIRUMC_TEXT { get; set; }
        public bool BREASTFEED { get; set; }
        public string BREAST_TEXT { get; set; }
        public bool POSPARTUM_DEPRESSION { get; set; }
        public string PPDEPR_TEXT { get; set; }
        public bool NEWBORN { get; set; }
        public string NEWBOR_TEXT { get; set; }
        public string REFERENCE { get; set; }
        public bool MEDHISTUPDATED { get; set; }
        public int RECID { get; set; }


        public static ANC09 GetANC09(string Reference)
        {
            ANC09 anc09 = new ANC09();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC09_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc09.HIV = (bool)reader["hiv"];
                    anc09.HIV_TEXT = reader["hiv_text"].ToString();
                    anc09.RISKFACTOR = (bool)reader["ristfactor"];
                    anc09.RISKFA_TEXT = reader["riskfa_text"].ToString();
                    anc09.PRENATAL = (bool)reader["prenatal"];
                    anc09.PRENAT_TEXT = reader["prenat_text"].ToString();
                    anc09.NUTRITION = (bool)reader["nutrition"];
                    anc09.NUTRI_TEXT = reader["nutir_text"].ToString();
                    anc09.SEXUAL = (bool)reader["sexual"];
                    anc09.SEXU_TEXT = reader["sexu_text"].ToString();
                    anc09.EXERCISE = (bool)reader["exercise"];
                    anc09.EXERC_TEXT = reader["exerc_text"].ToString();
                    anc09.ENVIRONMENTAL = (bool)reader["enviromental"];
                    anc09.ENVIRO_TEXT = reader["enviro_text"].ToString();
                    anc09.TOBACCO = (bool)reader["tobacco"];
                    anc09.TOBACC_TEXT = reader["tobacc_text"].ToString();
                    anc09.ALCOHOL = (bool)reader["alcohol"];
                    anc09.ALCOHO_TEXT = reader["alcoho_text"].ToString();
                    anc09.ILLICITDRUGS = (bool)reader["illicitdrugs"];
                    anc09.IL_DRG_TEXT = reader["il_drg_text"].ToString();
                    anc09.MEDICATIONS = (bool)reader["medications"];
                    anc09.MEDIC_TEXT = reader["medic_text"].ToString();
                    anc09.ULTRASOUND = (bool)reader["ultrasound"];
                    anc09.ULTRAS_TEXT = reader["ultras_text"].ToString();
                    anc09.DOMESTIC = (bool)reader["domestic"];
                    anc09.DOMES_TEXT = reader["domes_text"].ToString();
                    anc09.SEATBELT = (bool)reader["seatbelt"];
                    anc09.SEATBELT_TEXT = reader["seabelt_text"].ToString();
                    anc09.CHILDBIRTH = (bool)reader["childbirth"];
                    anc09.CHILDB_TEXT = reader["childb_text"].ToString();
                    anc09.PRETERM = (bool)reader["preterm"];
                    anc09.PRETER_TEXT = reader["preter_text"].ToString();
                    anc09.ABNORMAL = (bool)reader["abnormal"];
                    anc09.AB_LAB_TEXT = reader["ab_lab_text"].ToString();
                    anc09.POSTPARTUM_FAM_PLAN = (bool)reader["postpartum"];
                    anc09.PPARTUM_FAM_PLANNING = reader["ppartum_te"].ToString();
                    anc09.ANAESTHESIA = (bool)reader["anaesthesia"];
                    anc09.ANAES_TEXT = reader["anaes_text"].ToString();
                    anc09.FETAL = (bool)reader["fetal"];
                    anc09.FETAL_TEXT = reader["fetal_text"].ToString();
                    anc09.LABOURSIGNS = (bool)reader["laboursign"];
                    anc09.LABOU_TEXT = reader["labou_text"].ToString();
                    anc09.VBAC = (bool)reader["vbac"];
                    anc09.VBAC_TEXT = reader["vbac_text"].ToString();
                    anc09.PIH = (bool)reader["pih"];
                    anc09.PIH_TEXT = reader["pih_text"].ToString();
                    anc09.POSTTERM = (bool)reader["postterm"];
                    anc09.POSTT_TEXT = reader["postt_text"].ToString();
                    anc09.CIRCUMCISION = (bool)reader["circumcision"];
                    anc09.CIRUMC_TEXT = reader["cirumc_text"].ToString();
                    anc09.BREASTFEED = (bool)reader["breastfeed"];
                    anc09.BREAST_TEXT = reader["breast_text"].ToString();
                    anc09.POSPARTUM_DEPRESSION = (bool)reader["pospartum"];
                    anc09.PPDEPR_TEXT = reader["ppdepr_text"].ToString();
                    anc09.NEWBORN = (bool)reader["newborn"];
                    anc09.NEWBOR_TEXT = reader["newbor_text"].ToString();
                    anc09.REFERENCE = reader["reference"].ToString();
                    anc09.MEDHISTUPDATED = (bool)reader["medhistupdate"];
                    anc09.RECID = (Int32)reader["recid"];

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
                MessageBox.Show("" + ex, "Get ANC09 Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc09;
        }


    }
}