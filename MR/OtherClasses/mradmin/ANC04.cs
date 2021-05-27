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
    public class ANC04
    {
        public string REFERENCE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string HEIGHT { get; set; }
        public string WEIGHT { get; set; }
        public string TEMP { get; set; }
        public string BP { get; set; }
        public decimal HEENT { get; set; }
        public decimal FUNDI { get; set; }
        public decimal TEETH { get; set; }
        public decimal THYROID { get; set; }
        public decimal BREASTS { get; set; }
        public decimal LUNGS { get; set; }
        public decimal HEART { get; set; }
        public decimal ABDOMEN { get; set; }
        public decimal EXTREMITIES { get; set; }
        public decimal SKIN { get; set; }
        public decimal LYMPHNODES { get; set; }
        public decimal VULVA { get; set; }
        public decimal VAGINA { get; set; }
        public decimal CERVIX { get; set; }
        public string UTERINESIZE { get; set; }
        public decimal FIBROIDS { get; set; }
        public decimal ADNEXA { get; set; }
        public decimal HAEMORRHOIDS { get; set; }
        public string COMMENTS { get; set; }
        public string DELPLAN { get; set; }
        public string INTERVIEWER { get; set; }
        public string PULSE { get; set; }
        public string RESPIRATION { get; set; }



        public static ANC04 GetANC04(string Reference)
        {
            ANC04 anc04 = new ANC04();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC04_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);

            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc04.REFERENCE = reader["reference"].ToString();
                    anc04.POSTED = (bool)reader["posted"];
                    anc04.POST_DATE = (DateTime)reader["post_date"];
                    anc04.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc04.HEIGHT = reader["height"].ToString();
                    anc04.TEMP = reader["temp"].ToString();
                    anc04.BP = reader["bp"].ToString();
                    anc04.HEENT = (Decimal)reader["heent"];
                    anc04.FUNDI = (Decimal)reader["fundi"];
                    anc04.TEETH = (Decimal)reader["teeth"];
                    anc04.THYROID = (Decimal)reader["thyroid"];
                    anc04.BREASTS = (Decimal)reader["breasts"];
                    anc04.LUNGS = (Decimal)reader["lungs"];
                    anc04.HEART = (Decimal)reader["heart"];
                    anc04.ABDOMEN = (Decimal)reader["abdomen"];
                    anc04.EXTREMITIES = (Decimal)reader["extremities"];
                    anc04.SKIN = (Decimal)reader["skin"];
                    anc04.LYMPHNODES = (Decimal)reader["lymphnodes"];
                    anc04.VULVA = (Decimal)reader["vulva"];
                    anc04.VAGINA = (Decimal)reader["vagina"];
                    anc04.CERVIX = (Decimal)reader["cervix"];
                    anc04.UTERINESIZE = reader["UTERINESIZE"].ToString();
                    anc04.FIBROIDS = (Decimal)reader["fibroids"];
                    anc04.ADNEXA = (Decimal)reader["adnexa"];
                    anc04.HAEMORRHOIDS = (Decimal)reader["haemorrhoids"];
                    anc04.COMMENTS = reader["comments"].ToString();
                    anc04.DELPLAN = reader["delplan"].ToString();
                    anc04.INTERVIEWER = reader["interviewer"].ToString();
                    anc04.PULSE = reader["pulse"].ToString();
                    anc04.RESPIRATION = reader["respiration"].ToString();



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
                MessageBox.Show("" + ex, "Get ANC04 Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc04;
        }
    }


    }
