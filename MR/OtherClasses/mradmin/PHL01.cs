using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using msfunc;

namespace mradmin.DataAccess
{
    public class PHL01
    {
       public string REFERENCE { get; set; }
        public string NAME { get; set; }
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public string SEX { get; set; }
        public string ADDRESS1 { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public string DOCTOR { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string BILLSELF { get; set; }
        public string FACILITY { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string CROSSREF { get; set; }
        public decimal AGE { get; set; }
        public string GROUPHTYPE { get; set; }
        public string GROUPHEAD { get; set; }
        public string GHGROUPCODE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string SAMPLEBY { get; set; }
        public DateTime SAMPLEDATE { get; set; }
        public string OTHERS { get; set; }
        public string REQPROFILE { get; set; }
        public string DEFAULTSTRING { get; set; }
        public string TEXTAGE { get; set; }
    

    public static PHL01 GetPHL01(string REFERENCE,string facility)
        {
            PHL01 phl01 = new PHL01();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "PHL01_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", REFERENCE);
            selectCommand.Parameters.AddWithValue("@Facility", facility);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    phl01.REFERENCE = reader["reference"].ToString();
                    phl01.NAME = reader["name"].ToString();
                    phl01.PATIENTNO = reader["patientno"].ToString();
                    phl01.GROUPCODE = reader["groupcode"].ToString();
                    phl01.SEX = reader["sex"].ToString();
                    phl01.ADDRESS1 = reader["address1"].ToString();
                    phl01.BIRTHDATE = (DateTime)reader["birthdate"];
                    phl01.DOCTOR = reader["doctor"].ToString();
                    phl01.TRANS_DATE = (DateTime)reader["trans_date"];
                    phl01.BILLSELF = reader["billself"].ToString();
                    phl01.FACILITY = reader["facility"].ToString();
                    phl01.POSTED = (bool)reader["posted"];
                    phl01.POST_DATE = (DateTime)reader["post_date"];
                    phl01.CROSSREF = reader["crossref"].ToString();
                    phl01.AGE = (Decimal)reader["age"];
                    phl01.GROUPHTYPE  = reader["grouphtype"].ToString(); 
                    phl01.GROUPHEAD  = reader["grouphead"].ToString();
                    phl01.GHGROUPCODE  = reader["GHGROUPCODE"].ToString();
                    phl01.DTIME = (DateTime)reader["dtime"];
                    phl01.SAMPLEBY = reader["sampleby"].ToString();
                    phl01.SAMPLEDATE = (DateTime)reader["sampledate"];
                    phl01.OTHERS = reader["others"].ToString();
                    phl01.REQPROFILE = reader["reqprofile"].ToString();
                    phl01.DEFAULTSTRING = reader["defaultstring"].ToString();
                    phl01.TEXTAGE = reader["textage"].ToString();
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
                MessageBox.Show("" + ex, "Get Phlebo Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return phl01;
        }
    }
}