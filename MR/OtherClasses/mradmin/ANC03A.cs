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
    public class ANC03A
    {
        public string REFERENCE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public bool AGEATEDD { get; set; }
        public bool SICKLECELL { get; set; }
        public bool DOWNS { get; set; }
        public bool CHROMOSOMAL { get; set; }
        public bool HEARTDISEASE { get; set; }
        public bool METABOLIC { get; set; }
        public bool TUBEDEFECT { get; set; }
        public bool STILLBIRTH { get; set; }
        public bool MEDICATIONS { get; set; }
        public bool TB { get; set; }
        public bool HERPES { get; set; }
        public bool VIRALILLNESS { get; set; }
        public bool STI { get; set; }
        public bool HEPATITISB { get; set; }
        public string INDEXPREG { get; set; }
        public string MEDICATIONDETL { get; set; }


        public static ANC03A GetANC03A(string Groupcode, string patientno)
        {
            ANC03A anc03a = new ANC03A();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC03A_Get";
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
                    anc03a.REFERENCE = reader["reference"].ToString();
                    anc03a.POSTED = (bool)reader["posted"];
                    anc03a.POST_DATE = (DateTime)reader["post_date"];
                    anc03a.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc03a.AGEATEDD = (bool)reader["ageatedd"];
                    anc03a.SICKLECELL = (bool)reader["sicklecell"];
                    anc03a.DOWNS = (bool)reader["downs"];
                    anc03a.CHROMOSOMAL = (bool)reader["chromosomal"];
                    anc03a.HEARTDISEASE = (bool)reader["heartdisease"];
                    anc03a.METABOLIC = (bool)reader["metabolic"];
                    anc03a.TUBEDEFECT = (bool)reader["tubedefect"];
                    anc03a.STILLBIRTH = (bool)reader["stillbirth"];
                    anc03a.MEDICATIONS = (bool)reader["medications"];
                    anc03a.TB = (bool)reader["tb"];
                    anc03a.HERPES = (bool)reader["herpes"];
                    anc03a.VIRALILLNESS = (bool)reader["viralillness"];
                    anc03a.STI = (bool)reader["sti"];
                    anc03a.HEPATITISB = (bool)reader["hepatitisb"];
                    anc03a.INDEXPREG = reader["indexpreg"].ToString();
                    anc03a.MEDICATIONDETL = reader["medicationdetl"].ToString();


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
                MessageBox.Show("" + ex, "Get ANC03A Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc03a;
        }
    }
}