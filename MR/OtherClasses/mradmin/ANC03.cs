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
    public class ANC03
    {
        public string REFERENCE { get; set; }
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal PREV_PREG_TOTAL { get; set; }
        public decimal NOALIVE { get; set; }
        public DateTime DATEOFBIRTH { get; set; }
        public string DURATIONOFPREG { get; set; }
        public string BIRTHWT { get; set; }
        public string PLACEOFBIRTH { get; set; }
        public string PREG_LABOUR { get; set; }
        public string AGEATDEATH { get; set; }
        public string CAUSEOFDEATH { get; set; }
        public string MTHOFBIRTH { get; set; }
        public string SEX { get; set; }


        public static DataTable GetANC03(string groupcode, string patietno)
        {
            // ANC03 anc03 = new ANC03();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC03_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@groupcode", groupcode);
            selectCommand.Parameters.AddWithValue("@patientno", patietno);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static bool DeleteANC03(string groupcode, string patietno, string monthofbirth)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "ANC03_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@groupcode", groupcode);
            deleteStatement.Parameters.AddWithValue("@patientno", patietno);
            deleteStatement.Parameters.AddWithValue("@mthofbirth", monthofbirth);
            try
            {
                connection.Open();
                int count = deleteStatement.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException ex)
            {
                MessageBox.Show(" " + ex, "Delete Previous Pregnancy Details", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return false;
            }
            finally
            {
                connection.Close();
            }

        }
/*
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc03.REFERENCE = reader["reference"].ToString();
                    anc03.GROUPCODE = reader["groupcode"].ToString();
                    anc03.PATIENTNO = reader["patientno"].ToString();
                    anc03.POSTED = (bool)reader["posted"];
                    anc03.POST_DATE = (DateTime)reader["post_date"];
                    anc03.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc03.PREV_PREG_TOTAL = (Decimal)reader["prev_preg_total"];
                    anc03.NOALIVE = (Decimal)reader["noalive"];
                    anc03.DATEOFBIRTH = (DateTime)reader["dateofbirth"];
                    anc03.DURATIONOFPREG = reader["durationofpreg"].ToString();
                    anc03.BIRTHWT = reader["birthwt"].ToString();
                    anc03.PLACEOFBIRTH = reader["placeofbirth"].ToString();
                    anc03.PREG_LABOUR = reader["preg_labour"].ToString();
                    anc03.AGEATDEATH = reader["ageatdeath"].ToString();
                    anc03.CAUSEOFDEATH = reader["causeofdeath"].ToString();
                    anc03.MTHOFBIRTH = reader["mthofbirth"].ToString();
                    anc03.SEX = reader["sex"].ToString();

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
                MessageBox.Show("" + ex, "Get Patient Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc03;
        }
            */

    }
}