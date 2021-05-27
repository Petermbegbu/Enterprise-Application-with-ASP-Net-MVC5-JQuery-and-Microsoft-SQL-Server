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
    public class HmoAuthorizations
    {
        public string REFERENCE { get; set; }
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string REFERRAL { get; set; }
        public string REFERRALCLINIC { get; set; }
        public DateTime REFERRALDATE { get; set; }
        public string REFERREDTODOC { get; set; }
        public string REFERREDTOCLINIC { get; set; }
        public DateTime REQUESTCOMMENCED { get; set; }
        public string REQUESTDETAILS { get; set; }
        public string GROUPHEAD { get; set; }
        public string GROUPHTYPE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string GHGROUPCODE { get; set; }
        public string DIAGNOSIS { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string AUTHORIZEDCODE { get; set; }
        public string DATERECEIVED { get; set; }


        public static HmoAuthorizations GetHMOAUTHORIZATIONS(string xreference, string GroupCodeID, string PatientID)
        {
            HmoAuthorizations hmoautho = new HmoAuthorizations();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "HMOAUTHORIZATIONS_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", xreference);
          /*  selectCommand.Parameters.AddWithValue("@GroupCode", GroupCodeID);
            selectCommand.Parameters.AddWithValue("@Patient", PatientID);*/
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    hmoautho.REFERENCE = reader["reference"].ToString();
                    hmoautho.GROUPCODE = reader["groupcode"].ToString();
                    hmoautho.PATIENTNO = reader["patientno"].ToString();
                    hmoautho.NAME = reader["name"].ToString();
                    hmoautho.TRANS_DATE = (DateTime)reader["trans_date"];
                    hmoautho.REFERRAL = reader["referral"].ToString();
                    hmoautho.REFERRALCLINIC = reader["referralclinic"].ToString();
                    hmoautho.REFERRALDATE = (DateTime)reader["referraldate"];
                    hmoautho.REFERREDTODOC = reader["referredtodoc"].ToString();
                    hmoautho.REQUESTCOMMENCED = (DateTime)reader["requestcommenced"];
                    hmoautho.REQUESTDETAILS = reader["requestdetails"].ToString();
                    hmoautho.GROUPHEAD = reader["grouphead"].ToString();
                    hmoautho.GROUPHTYPE = reader["grouphtype"].ToString();
                    hmoautho.POSTED = (bool)reader["posted"];
                    hmoautho.POST_DATE = (DateTime)reader["post_date"];
                    hmoautho.GHGROUPCODE = reader["GHGROUPCODE"].ToString();
                    hmoautho.DIAGNOSIS = reader["diagnosis"].ToString();
                    hmoautho.OPERATOR = reader["operator"].ToString();
                    hmoautho.DTIME = (DateTime)reader["dtime"];
                    hmoautho.AUTHORIZEDCODE = reader["authorizedcode"].ToString();
                    hmoautho.DATERECEIVED = reader["datereceived"].ToString();
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
                MessageBox.Show("" + ex, "Get HMO Authorization Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return hmoautho;
        }


    }
}