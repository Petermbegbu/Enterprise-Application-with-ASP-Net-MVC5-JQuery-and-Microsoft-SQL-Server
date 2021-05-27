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
    public class BIRTHS
    {
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public string GROUPHEAD { get; set; }
        public string GHGROUPCODE { get; set; }
        public string PARENTGROUPCODE { get; set; }
        public string PARENT { get; set; }
        public string FATHER { get; set; }
        public string MOTHER { get; set; }
        public string NAME { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public string BIRTHTIME { get; set; }
        public string TYPEOFDELI { get; set; }
        public decimal WEIGHT { get; set; }
        public string APGARSCORE { get; set; }
        public string SEX { get; set; }
        public decimal LENGHT { get; set; }
        public string NURSE { get; set; }
        public string DOCTOR { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string PHOTOLOC { get; set; }
        public string GROUPHTYPE { get; set; }
        public string REMARKS { get; set; }
        public string BIRTHTYPE { get; set; }
        public bool BORNHERE { get; set; }
        public string HOSPITAL { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string GESTATION { get; set; }
        public decimal HEADCIRCUMF { get; set; }
        public int RECID { get; set; }

        public static BIRTHS GetBIRTHS(string reference) // GroupCodeID, string PatientID)
        {
            BIRTHS births = new BIRTHS();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BIRTHS_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
           // selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    births.PATIENTNO = reader["patientno"].ToString(); 
                    births.GROUPCODE = reader["groupcode"].ToString();
                    births.GROUPHEAD = reader["grouphead"].ToString();
                    births.GHGROUPCODE = reader["ghgroupcode"].ToString();
                    births.PARENTGROUPCODE = reader["parentgroupcode"].ToString();
                    births.PARENT = reader["parent"].ToString();
                    births.FATHER = reader["father"].ToString();
                    births.MOTHER = reader["mother"].ToString();
                    births.NAME = reader["name"].ToString();
                    births.BIRTHDATE = (DateTime)reader["birthdate"];
                    births.BIRTHTIME = reader["birthtime"].ToString();
                    births.TYPEOFDELI = reader["typeofdeli"].ToString();
                    births.WEIGHT  = (Decimal)reader["weight"];
                    births.APGARSCORE = reader["apgarscore"].ToString();
                    births.SEX = reader["sex"].ToString();
                    births.LENGHT = (Decimal)reader["lenght"];
                    births.NURSE= reader["nurse"].ToString();
                    births.DOCTOR = reader["doctor"].ToString();
                    births.POSTED = (bool)reader["posted"];
                    births.POST_DATE = (DateTime)reader["post_date"];
                    births.PHOTOLOC = reader["photoloc"].ToString();
                    births.GROUPHTYPE = reader["grouphtype"].ToString();
                    births.REMARKS = reader["remarks"].ToString();
                    births.BIRTHTYPE = reader["birthtype"].ToString();
                    births.BORNHERE  = (bool)reader["bornhere"];
                    births.HOSPITAL = reader["hospital"].ToString();
                    births.OPERATOR = reader["operator"].ToString();
                    births.DTIME = (DateTime)reader["dtime"];
                    births.GESTATION = reader["gestation"].ToString();
                    births.HEADCIRCUMF = (Decimal)reader["headcircumf"];
                    births.RECID = (Int32)reader["RECID"];               
                                                                             
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
                MessageBox.Show("" + ex, "Get Birth Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return births;
        }
        public static DataTable GetBirthsAll()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BIRTHS_Getlist";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

          //  selectCommand.Parameters.AddWithValue("@Facility", xfacility);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
    }
}