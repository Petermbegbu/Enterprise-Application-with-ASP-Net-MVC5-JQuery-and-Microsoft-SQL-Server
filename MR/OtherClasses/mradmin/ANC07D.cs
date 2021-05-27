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
    public class ANC07D
    {
        public string REFERENCE { get; set; }
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string BP { get; set; }
        public string PR { get; set; }
        public string URINALYSIS { get; set; }
        public string COMMENTS { get; set; }
        public DateTime NEXTVISIT { get; set; }
        public string DOCTOR { get; set; }
        public decimal NNV { get; set; }
        public string ATTENDREF { get; set; }


        public static ANC07D GetANC07D(string Reference)
        {
            ANC07D anc07d = new ANC07D();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC07D_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@reference", Reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc07d.REFERENCE = reader["reference"].ToString();
                    anc07d.GROUPCODE = reader["groupcode"].ToString();
                    anc07d.PATIENTNO = reader["patientno"].ToString();
                    anc07d.POSTED = (bool)reader["posted"];
                    anc07d.POST_DATE = (DateTime)reader["post_date"];
                    anc07d.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc07d.BP = reader["bp"].ToString();
                    anc07d.PR = reader["pr"].ToString();
                    anc07d.URINALYSIS = reader["urinalysis"].ToString();
                    anc07d.COMMENTS = reader["comments"].ToString();
                    anc07d.NEXTVISIT = (DateTime)reader["nextvisit"];
                    anc07d.DOCTOR = reader["doctor"].ToString();
                    anc07d.NNV = (Decimal)reader["nnv"];
                    anc07d.ATTENDREF = reader["attendref"].ToString();



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
                MessageBox.Show("" + ex, "Get ANC07D Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc07d;
        }
    }
}