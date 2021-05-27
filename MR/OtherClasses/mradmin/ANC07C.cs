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
    public class ANC07C
    {
        public string REFERENCE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string BWEIGHT { get; set; }
        public string SEX { get; set; }
        public decimal BIRTHTYPE { get; set; }
        public string GESTAGE { get; set; }
        public string MODEOFRESUSC { get; set; }
        public string DRUGS { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string EXAMTIME { get; set; }
        public string HR { get; set; }
        public string RR { get; set; }
        public string TEMP { get; set; }
        public string OFC { get; set; }
        public string LENGTH { get; set; }
        public string PALOR { get; set; }
        public string CYANOSIS { get; set; }
        public string JAUNDICE { get; set; }
        public string R_DISTRESS { get; set; }
        public decimal APPEARANCE { get; set; }
        public string APPEARANCENOTE { get; set; }
        public decimal HEAD { get; set; }
        public string HEADNOTE { get; set; }
        public decimal EARS { get; set; }
        public string EARSNOTE { get; set; }
        public decimal EYES { get; set; }
        public string EYESNOTE { get; set; }
        public decimal NOSE { get; set; }
        public string NOSENOTE { get; set; }
        public decimal MOUTH { get; set; }
        public string MOUTHNOTE { get; set; }
        public decimal RESPSYSTEM { get; set; }
        public string RESPSYSTEMNOTE { get; set; }
        public decimal CARDIOSYSTEM { get; set; }
        public string CARDIOSYSTEMNOTE { get; set; }
        public decimal ABDOMEN { get; set; }
        public string ABDOMENNOTE { get; set; }
        public decimal FEMORALS { get; set; }
        public string FEMORALSNOTE { get; set; }
        public decimal ANUS { get; set; }
        public string ANUSNOTE { get; set; }
        public decimal GENITALIA { get; set; }
        public string GENITALIANOTE { get; set; }
        public decimal EXTEMITES { get; set; }
        public string EXTREMITESNOTE { get; set; }
        public decimal HIPS { get; set; }
        public string HIPSNOTE { get; set; }
        public decimal CNS { get; set; }
        public string CNSNOTE { get; set; }
        public string OTHERFINDINGS { get; set; }
        public string COMMENTS { get; set; }
        public string STAFFSIGN { get; set; }
        public bool MEDHISTUPDATED { get; set; }




        public static ANC07C GetANC07C(string Reference)
        {
            ANC07C anc07c = new ANC07C();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC07C_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc07c.REFERENCE = reader["reference"].ToString();
                    anc07c.POSTED = (bool)reader["posted"];
                    anc07c.POST_DATE = (DateTime)reader["post_date"];
                    anc07c.BWEIGHT = reader["bweight"].ToString();
                    anc07c.SEX = reader["sex "].ToString();
                    anc07c.BIRTHTYPE = (Decimal)reader["birthtype"];
                    anc07c.GESTAGE = reader["gestage"].ToString();
                    anc07c.MODEOFRESUSC = reader["modeofresusc"].ToString();
                    anc07c.DRUGS = reader["drugs"].ToString();
                    anc07c.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc07c.EXAMTIME = reader["examtime"].ToString();
                    anc07c.HR = reader["hr"].ToString();
                    anc07c.RR = reader["rr"].ToString();
                    anc07c.TEMP = reader["temp"].ToString();
                    anc07c.OFC = reader["ofc"].ToString();
                    anc07c.LENGTH = reader["length"].ToString();
                    anc07c.PALOR = reader["palor"].ToString();
                    anc07c.CYANOSIS = reader["cyanosis"].ToString();
                    anc07c.JAUNDICE = reader["jaundice"].ToString();
                    anc07c.R_DISTRESS = reader["drugs"].ToString();
                    anc07c.APPEARANCE = (Decimal)reader["appearance"];
                    anc07c.APPEARANCENOTE = reader["appearancenote"].ToString();
                    anc07c.HEAD = (Decimal)reader["head"];
                    anc07c.HEADNOTE = reader["headnote"].ToString();
                    anc07c.EARS = (Decimal)reader["ears"];
                    anc07c.EARSNOTE = reader["earsnote"].ToString();
                    anc07c.EYES = (Decimal)reader["eyes"];
                    anc07c.EYESNOTE = reader["eyesnote"].ToString();
                    anc07c.NOSE = (Decimal)reader["nose"];
                    anc07c.NOSENOTE = reader["nosenote"].ToString();
                    anc07c.MOUTH = (Decimal)reader["mouth"];
                    anc07c.MOUTHNOTE = reader["mouthnote"].ToString();
                    anc07c.RESPSYSTEM = (Decimal)reader["respsystem"];
                    anc07c.RESPSYSTEMNOTE = reader["respsystemnote"].ToString();
                    anc07c.CARDIOSYSTEM = (Decimal)reader["cardiosystem"];
                    anc07c.CARDIOSYSTEMNOTE = reader["cardiosystemnote"].ToString();
                    anc07c.ABDOMEN = (Decimal)reader["abdomen"];
                    anc07c.ABDOMENNOTE = reader["abdomennote"].ToString();
                    anc07c.FEMORALS = (Decimal)reader["femorals"];
                    anc07c.FEMORALSNOTE = reader["femoralsnote"].ToString();
                    anc07c.ANUS = (Decimal)reader["anus"];
                    anc07c.ANUSNOTE = reader["anusnote"].ToString();
                    anc07c.GENITALIA = (Decimal)reader["genitalia"];
                    anc07c.GENITALIANOTE = reader["genitalianote"].ToString();
                    anc07c.EXTEMITES = (Decimal)reader["extemites"];
                    anc07c.EXTREMITESNOTE = reader["extremitesnote"].ToString();
                    anc07c.HIPS = (Decimal)reader["hips"];
                    anc07c.HIPSNOTE = reader["hipsnote"].ToString();
                    anc07c.CNS = (Decimal)reader["cns"];
                    anc07c.CNSNOTE = reader["hipsnote"].ToString();
                    anc07c.OTHERFINDINGS = reader["otherfindings"].ToString();
                    anc07c.COMMENTS = reader["comments"].ToString();
                    anc07c.STAFFSIGN = reader["staffsign"].ToString();
                    anc07c.MEDHISTUPDATED = (bool)reader["medhistupdated"];

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
                MessageBox.Show("" + ex, "Get ANC07C Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc07c;
        }
    }
 }