using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
//using System.Windows.Forms;
using msfunc;

namespace mradmin.DataAccess
{
    public class billchaindtl
    {
          public billchaindtl() { }

            public string GROUPCODE { get; set; }
            public string PATIENTNO { get; set; }
            public string GROUPHEAD { get; set; }
            public string NAME { get; set; }
            public DateTime REG_DATE { get; set; }
            public bool POSTED { get; set; }
            public DateTime POST_DATE { get; set; }
            public string GROUPHTYPE { get; set; }
            public string STAFFNO { get; set; }
            public string RELATIONSH { get; set; }
            public string SECTION { get; set; }
            public string DEPARTMENT { get; set; }
            public decimal CUR_DB { get; set; }
            public string STATUS { get; set; }
            public string SEX { get; set; }
            public string M_STATUS { get; set; }
            public DateTime BIRTHDATE { get; set; }
            public decimal CUMVISITS { get; set; }
            public string HMOCODE { get; set; }
            public string PATCATEG { get; set; }
            public string RESIDENCE { get; set; }
            public string GHGROUPCODE { get; set; }
            public string HMOSERVTYPE { get; set; }
            public string BILLONACCT { get; set; }
            public string CURRENCY { get; set; }
            public string OPERATOR { get; set; }
            public DateTime DTIME { get; set; }
            public DateTime EXPIRYDATE { get; set; }
            public bool ASTNOTES { get; set; }
            public string CLINIC { get; set; }
            public string PHONE { get; set; }
            public string EMAIL { get; set; }
            public string PICLOCATION { get; set; }
            public string SURNAME { get; set; }
            public string OTHERNAMES { get; set; }
            public string TITLE { get; set; }
            public string SPNOTES { get; set; }
            public string MEDNOTES { get; set; }
            public bool MEDHISTORYCHAINED { get; set; }
            public string PATIENTNO_PRINCIPAL { get; set; }
        /// <summary>
        /// pass empty groupcode to check for patient number only
        /// </summary>
        /// <param name="PatientID"></param>
        /// <param name="groupcode"></param>
        /// <returns></returns>
        public static billchaindtl Getbillchain(string PatientID, string groupcode)
        {
            billchaindtl bchain = new billchaindtl();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = string.IsNullOrWhiteSpace(groupcode) ? "BILLCHAIN_checkpatno" : "BILLCHAIN_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@GroupCode", groupcode);
            selectCommand.Parameters.AddWithValue("@PatientNo", PatientID);

            connection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
            if (reader.Read())
            {
                bchain.GROUPCODE = reader["groupcode"].ToString();
                bchain.PATIENTNO = reader["patientno"].ToString();
                bchain.GROUPHEAD = reader["grouphead"].ToString();
                bchain.NAME = reader["name"].ToString();
                bchain.REG_DATE = (DateTime)reader["reg_date"];
                bchain.POSTED = (bool)reader["posted"];
                bchain.POST_DATE = (DateTime)reader["post_date"];
                bchain.GROUPHTYPE = reader["grouphtype"].ToString();
                bchain.STAFFNO = reader["staffno"].ToString();
                bchain.RELATIONSH = reader["relationsh"].ToString();
                bchain.SECTION = reader["section"].ToString();
                bchain.DEPARTMENT = reader["department"].ToString();
                bchain.CUR_DB = (Decimal)reader["cur_db"];
                bchain.STATUS = reader["status"].ToString();
                bchain.SEX = reader["sex"].ToString();
                bchain.M_STATUS = reader["m_status"].ToString();
                bchain.BIRTHDATE = (DateTime)reader["birthdate"];
                bchain.CUMVISITS = (Decimal)reader["cumvisits"];
                bchain.HMOCODE = reader["hmocode"].ToString();
                bchain.PATCATEG = reader["patcateg"].ToString();
                bchain.RESIDENCE = reader["residence"].ToString();
                bchain.GHGROUPCODE = reader["ghgroupcode"].ToString();
                bchain.HMOSERVTYPE = reader["hmoservtype"].ToString();
                bchain.BILLONACCT = reader["billonacct"].ToString();
                bchain.CURRENCY = reader["currency"].ToString();
                bchain.OPERATOR = reader["operator"].ToString();
                bchain.DTIME = (DateTime)reader["dtime"];
                bchain.EXPIRYDATE = (DateTime)reader["expirydate"];
                bchain.ASTNOTES = (bool)reader["astnotes"];
                bchain.CLINIC = reader["clinic"].ToString();
                bchain.PHONE = reader["phone"].ToString();
                bchain.EMAIL = reader["email"].ToString();
                bchain.PICLOCATION = reader["piclocation"].ToString();
                bchain.SURNAME = reader["surname"].ToString();
                bchain.OTHERNAMES = reader["othernames"].ToString();
                bchain.TITLE = reader["title"].ToString();
                bchain.SPNOTES = reader["spnotes"].ToString();
                bchain.MEDNOTES = reader["mednotes"].ToString();
                bchain.MEDHISTORYCHAINED = (bool)reader["medhistorychained"];
                bchain.PATIENTNO_PRINCIPAL = reader["patientno_principal"].ToString();
            }
            else { bchain = null; }            
            reader.Close();
            connection.Close();

            return bchain;
        }
      public static bool DeleteBillchain(string PatientID,string groupcode)
      {
           SqlConnection connection = Dataaccess.mrConnection();
           SqlCommand deleteCommand = new SqlCommand();
           deleteCommand.CommandText = "BILLCHAIN_Delete"; //"spGetPatient";
           deleteCommand.Connection = connection;
           deleteCommand.CommandType = CommandType.StoredProcedure;

           deleteCommand.Parameters.AddWithValue("@PatientNo", PatientID);
           deleteCommand.Parameters.AddWithValue("@Groupcode", groupcode);

            connection.Open();
              int count = deleteCommand.ExecuteNonQuery();
            connection.Close();

            if (count > 0)
                  return true;
              else
                  return false;
            
      }
 
    }
}