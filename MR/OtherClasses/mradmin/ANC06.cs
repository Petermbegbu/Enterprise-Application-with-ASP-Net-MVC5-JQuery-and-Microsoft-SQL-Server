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
    public class ANC06
    {
        public string REFERENCE { get; set; }
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string HIGHT_OF_FUNDUS { get; set; }
        public string PRESENTATION_POSITION { get; set; }
        public string RELATION_OF_PP_TOBRIM { get; set; }
        public string FOETAL_HEART { get; set; }
        public string URINE { get; set; }
        public string BLOOD_PRESSURE { get; set; }
        public string WEIGHT { get; set; }
        public string HB { get; set; }
        public string OEDEMA { get; set; }
        public string REMARKS_TREATMENT { get; set; }
        public DateTime NEXTVISIT { get; set; }
        public string DOCTOR { get; set; }
        public decimal NNV { get; set; }
        public string ATTENDREF { get; set; }
        public string GESTATIONALAGE { get; set; }


        public static ANC06 GetANC06(string Reference, DateTime transdate)
        {
            ANC06 anc06 = new ANC06();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC06_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            selectCommand.Parameters.AddWithValue("@Trans_date", transdate);
            selectCommand.Parameters.AddWithValue("@ISAll", false );

            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc06.REFERENCE = reader["reference"].ToString();
                    anc06.GROUPCODE = reader["groupcode"].ToString();
                    anc06.PATIENTNO = reader["patientno"].ToString();
                    anc06.POSTED = (bool)reader["posted"];
                    anc06.POST_DATE = (DateTime)reader["post_date"];
                    anc06.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc06.HIGHT_OF_FUNDUS = reader["hight_of_fundus"].ToString();
                    anc06.RELATION_OF_PP_TOBRIM = reader["relation_of_pp_tobrim"].ToString();
                    anc06.FOETAL_HEART = reader["foetal_heart"].ToString();
                    anc06.URINE = reader["urine"].ToString();
                    anc06.BLOOD_PRESSURE = reader["blood_pressure"].ToString();
                    anc06.WEIGHT = reader["weight"].ToString();
                    anc06.HB = reader["hb"].ToString();
                    anc06.OEDEMA = reader["oedema"].ToString();
                    anc06.REMARKS_TREATMENT = reader["remarks_treatment"].ToString();
                    anc06.NEXTVISIT = (DateTime)reader["nextvisit"];
                    anc06.DOCTOR = reader["doctor"].ToString();
                    anc06.NNV = (Decimal)reader["nnv"];
                    anc06.ATTENDREF = reader["attendref"].ToString();
                    anc06.GESTATIONALAGE = reader["gestationalage"].ToString();

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
                MessageBox.Show("" + ex, "Get ANC06 Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc06;
        }
        /// <summary>
        /// Returns datatable on Reference
        /// </summary>
        /// <param name="Reference"></param>
        /// <param name="transdate"></param>
        /// <returns></returns>
        public static DataTable GetANC06(string Reference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC06_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            selectCommand.Parameters.AddWithValue("@Trans_date", DateTime.Now.Date ); //not useful
            selectCommand.Parameters.AddWithValue("@ISAll", true);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
    }
}