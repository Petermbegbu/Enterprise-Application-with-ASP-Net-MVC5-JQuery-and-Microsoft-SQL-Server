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
    public class ANC07
    {
        public string REFERENCE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime ADMITTED { get; set; }
        public string CONSOBS { get; set; }
        public string ATTENDPAED { get; set; }
        public string DELSUITE { get; set; }
        public DateTime DELDATE { get; set; }
        public string DELTIME { get; set; }
        public string PARITY { get; set; }
        public string GESTAGE { get; set; }
        public string FETALNUMBER { get; set; }
        public bool LO_NONE { get; set; }
        public bool LO_SPONTANEOUS { get; set; }
        public bool LO_INDUCED { get; set; }
        public bool LO_AUGUMENTD { get; set; }
        public string INDICANTIONS { get; set; }
        public DateTime ROMDATE { get; set; }
        public string ROMTIME { get; set; }
        public bool ROM_INDUCED { get; set; }
        public bool ROM_ACTIFICIAL { get; set; }
        public string ROM_INDICATIONS { get; set; }
        public string ROM_DURATION { get; set; }
        public bool PR_NONE { get; set; }
        public bool PR_NARCOTICS { get; set; }
        public bool PR_PRUDENDAL { get; set; }
        public bool PR_ENTONOX { get; set; }
        public bool PR_EPIDURAL { get; set; }
        public bool PR_SPINAL { get; set; }
        public bool PR_COMBINED { get; set; }
        public DateTime LABONSETDT { get; set; }
        public string LABPNSETTIME { get; set; }
        public DateTime LABFDDT { get; set; }
        public string LABFDTIME { get; set; }
        public DateTime LABPCDT { get; set; }
        public string LABPCTIME { get; set; }
        public DateTime LABHDDT { get; set; }
        public string LABHDTIME { get; set; }
        public DateTime LABBDDT { get; set; }
        public string LABBDTIME { get; set; }
        public DateTime LABEOTSDT { get; set; }
        public string LABEOTSTIME { get; set; }
        public DateTime LABT2DDT { get; set; }
        public string LAB2DTIME { get; set; }
        public string FSTSTAGEHRMIN { get; set; }
        public string SSTSTAGEHRMIN { get; set; }
        public string TSTSTAGEHRMIN { get; set; }
        public string LABDURATION { get; set; }
        public bool TSTAGEM_A { get; set; }
        public bool TSTAGEM_M { get; set; }
        public string TSTAGEMGTNOTES { get; set; }
        public bool OXYTOCICS { get; set; }
        public bool EGOMETRINE { get; set; }
        public string OXYTOCICSDTM { get; set; }
        public string CORD { get; set; }
        public decimal MEMBRANES { get; set; }
        public decimal PLACENTA { get; set; }
        public string BLMEASURE { get; set; }
        public string BLESTIMATES { get; set; }
        public string BLTOTAL { get; set; }
        public string FURTHERACTN { get; set; }
        public bool TRAUMA_NONE { get; set; }
        public bool CERVICAL_TEAR { get; set; }
        public bool PERINEAL_TEAR { get; set; }
        public decimal TEARDEGREE { get; set; }
        public bool EPISIOTOMY { get; set; }
        public string INDI4EPISIOTOMY { get; set; }
        public decimal REPREQ { get; set; }
        public decimal MOTHERAGREE { get; set; }
        public string ANAESTHUSED { get; set; }
        public string STAFF { get; set; }
        public DateTime TRDTTIME { get; set; }
        public string BAB1BY { get; set; }
        public string BAB2BY { get; set; }
        public string BAB3BY { get; set; }
        public string COMMENTS { get; set; }


        public static ANC07 GetANC07(string Reference)
        {
            ANC07 anc07 = new ANC07();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC07_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc07.REFERENCE = reader["reference"].ToString();
                    anc07.POSTED = (bool)reader["posted"];
                    anc07.POST_DATE = (DateTime)reader["post_date"];
                    anc07.ADMITTED = (DateTime)reader["admitted"];
                    anc07.CONSOBS = reader["consobs"].ToString();
                    anc07.ATTENDPAED = reader["attendpaed"].ToString();
                    anc07.DELDATE = (DateTime)reader["deldate"];
                    anc07.DELTIME = reader["deltime"].ToString();
                    anc07.PARITY = reader["parity"].ToString();
                    anc07.GESTAGE = reader["gestage"].ToString();
                    anc07.FETALNUMBER = reader["fetalnumber"].ToString();
                    anc07.LO_NONE = (bool)reader["lo_none"];
                    anc07.LO_SPONTANEOUS = (bool)reader["lo_spontaneous"];
                    anc07.LO_INDUCED = (bool)reader["lo_induced"];
                    anc07.LO_AUGUMENTD = (bool)reader["lo_augumentd"];
                    anc07.INDICANTIONS = reader["indicantions"].ToString();
                    anc07.ROMDATE = (DateTime)reader["romdate"];
                    anc07.ROMTIME = reader["romtime"].ToString();
                    anc07.ROM_INDUCED = (bool)reader["rom_induced"];
                    anc07.ROM_ACTIFICIAL = (bool)reader["rom_actificial"];
                    anc07.ROM_INDICATIONS = reader["rom_indications"].ToString();
                    anc07.ROM_DURATION = reader["rom_duration"].ToString();
                    anc07.PR_NONE = (bool)reader["pr_none"];
                    anc07.PR_NARCOTICS = (bool)reader["pr_narcotics"];
                    anc07.PR_PRUDENDAL = (bool)reader["pr_prudendal"];
                    anc07.PR_ENTONOX = (bool)reader["pr_entonox"];
                    anc07.PR_EPIDURAL = (bool)reader["pr_epidural"];
                    anc07.PR_SPINAL = (bool)reader["pr_spinal"];
                    anc07.PR_COMBINED = (bool)reader["PR_COMBINED"];
                    anc07.LABONSETDT = (DateTime)reader["labonsetdt"];
                    anc07.LABPNSETTIME = reader["labpnsettime"].ToString();
                    anc07.LABFDDT = (DateTime)reader["labfddt"];
                    anc07.LABFDTIME = reader["labfdtime"].ToString();
                    anc07.LABPCDT = (DateTime)reader["labpcdt"];
                    anc07.LABPCTIME = reader["labpctime"].ToString();
                    anc07.LABHDDT = (DateTime)reader["labhddt"];
                    anc07.LABHDTIME = reader["labhdtime"].ToString();
                    anc07.LABBDDT = (DateTime)reader["labbddt"];
                    anc07.LABBDTIME = reader["labbdtime"].ToString();
                    anc07.LABEOTSDT = (DateTime)reader["labeotsdt"];
                    anc07.LABEOTSTIME = reader["labeotstime"].ToString();
                    anc07.LABT2DDT = (DateTime)reader["labt2ddt"];
                    anc07.LAB2DTIME = reader["lab2dtime"].ToString();
                    anc07.FSTSTAGEHRMIN = reader["FSTSTAGEHRMIN"].ToString();
                    anc07.SSTSTAGEHRMIN = reader["sststagehrmin"].ToString();
                    anc07.TSTSTAGEHRMIN = reader["tststagehrmin"].ToString();
                    anc07.LABDURATION = reader["labduration"].ToString();
                    anc07.TSTAGEM_A = (bool)reader["tstagem_a"];
                    anc07.TSTAGEM_M = (bool)reader["tstagem_m"];
                    anc07.TSTAGEMGTNOTES = reader["tstagemgtnotes"].ToString();
                    anc07.OXYTOCICS = (bool)reader["oxytocics"];
                    anc07.EGOMETRINE = (bool)reader["egometrine"];
                    anc07.OXYTOCICSDTM = reader["OXYTOCICSDTM"].ToString();
                    anc07.CORD = reader["cord"].ToString();
                    anc07.MEMBRANES = (Decimal)reader["membranes"];
                    anc07.PLACENTA = (Decimal)reader["placenta"];
                    anc07.BLMEASURE = reader["blmeasure"].ToString();
                    anc07.BLESTIMATES = reader["blestimates"].ToString();
                    anc07.BLTOTAL = reader["bltotal"].ToString();
                    anc07.FURTHERACTN = reader["furtheractn"].ToString();
                    anc07.TRAUMA_NONE = (bool)reader["trauma_none"];
                    anc07.CERVICAL_TEAR = (bool)reader["cervical_tear"];
                    anc07.PERINEAL_TEAR = (bool)reader["perineal_tear"];
                    anc07.TEARDEGREE = (Decimal)reader["teardegree"];
                    anc07.EPISIOTOMY = (bool)reader["episiotomy"];
                    anc07.INDI4EPISIOTOMY = reader["INDI4EPISIOTOMY"].ToString();
                    anc07.REPREQ = (Decimal)reader["repreq"];
                    anc07.MOTHERAGREE = (Decimal)reader["motheragree"];
                    anc07.ANAESTHUSED = reader["anaesthused"].ToString();
                    anc07.STAFF = reader["staff"].ToString();
                    anc07.TRDTTIME = (DateTime)reader["trdttime"];
                    anc07.BAB1BY = reader["bab1by"].ToString();
                    anc07.BAB2BY = reader["bab2by"].ToString();
                    anc07.BAB3BY = reader["bab3by"].ToString();
                    anc07.COMMENTS = reader["comments"].ToString();



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
                MessageBox.Show("" + ex, "Get ANC07 Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc07;
        }


    }
}