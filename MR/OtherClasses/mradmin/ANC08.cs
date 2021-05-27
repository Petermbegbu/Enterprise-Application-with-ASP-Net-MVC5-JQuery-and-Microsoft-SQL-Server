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
    public class ANC08
    {
        public string REFERENCE { get; set; }
        public string FACILITY { get; set; }
        public string PROCESS { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string RESULT { get; set; }


        public static ANC08 GetANC08(string Reference)
        {
            ANC08 anc08 = new ANC08();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC08_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", Reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    anc08.REFERENCE = reader["reference"].ToString();
                    anc08.FACILITY = reader["facility"].ToString();
                    anc08.PROCESS = reader["process"].ToString();
                    anc08.TRANS_DATE = (DateTime)reader["trans_date"];
                    anc08.RESULT = reader["result"].ToString();


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
                MessageBox.Show("" + ex, "Get ANC08 Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return anc08;
        }


    }
}