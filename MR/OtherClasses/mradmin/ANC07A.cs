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
    public class ANC07A
    {
        public string REFERENCE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string GESTAGE { get; set; }
        public string PARITY { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string PROCESS { get; set; }
        public string INDICATIONS { get; set; }
        public string STAFFPRESENT { get; set; }
        public string SURGEON { get; set; }
        public string ASSISTANT { get; set; }
        public string PAEDIATRICIAN { get; set; }
        public string MIDWIVES { get; set; }
        public string ANAESTHETIST { get; set; }
        public string OTHERS { get; set; }
        public string ANAESTHESIA { get; set; }
        public string FINDINGS { get; set; }
        public string PROCEDURENOTE { get; set; }
        public string MOTHER { get; set; }
        public string BABY { get; set; }
        public string STAFFSIGN { get; set; }


        public static ANC07A GetANC07A(string Reference)
        {
            ANC07A anc07a = new ANC07A();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC07A_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc07a.REFERENCE = reader["reference"].ToString();
                    anc07a.POSTED = (bool)reader["posted"];
                    anc07a.POST_DATE  = (DateTime)reader["post_date"];
                    anc07a.GESTAGE = reader["gestage"].ToString();
                    anc07a.PARITY = reader["parity"].ToString();
                    anc07a.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc07a.PROCESS = reader["process"].ToString();
                    anc07a.INDICATIONS = reader["indications"].ToString();
                    anc07a.STAFFPRESENT = reader["staffpresent"].ToString();
                    anc07a.SURGEON = reader["surgeon"].ToString();
                    anc07a.ASSISTANT = reader["assistant"].ToString();
                    anc07a.PAEDIATRICIAN = reader["paediatrician"].ToString();
                    anc07a.MIDWIVES = reader["midwives"].ToString();
                    anc07a.ANAESTHETIST = reader["anaesthetist"].ToString();
                    anc07a.OTHERS = reader["others"].ToString();
                    anc07a.ANAESTHESIA = reader["anaesthesia"].ToString();
                    anc07a.FINDINGS = reader["findings"].ToString();
                    anc07a.PROCEDURENOTE = reader["procedurenote"].ToString();
                    anc07a.MOTHER = reader["mother"].ToString();
                    anc07a.BABY = reader["baby"].ToString();
                    anc07a.STAFFSIGN = reader["staffsign"].ToString();
                                        
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
                MessageBox.Show("" + ex, "Get ANC07A Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc07a;
        }


    }
}