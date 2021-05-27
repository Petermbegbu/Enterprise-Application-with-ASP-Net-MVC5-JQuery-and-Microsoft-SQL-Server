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
    public class LABDET
    {
       public string REFERENCE { get; set; }
        public string NAME { get; set; }
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public string SEX { get; set; }
        public string ADDRESS1 { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public string REFERAL { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string BILL_NO { get; set; }
        public string PAY_NO { get; set; }
        public string BILLSELF { get; set; }
        public string FACILITY { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string CROSSREF { get; set; }
        public string AGE { get; set; }
        public string GROUPHTYPE { get; set; }
        public string GROUPHEAD { get; set; }
        public string GHGROUPCODE { get; set; }
        public string OCCUPATION { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal MAT_CUMAMT { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public int RECID { get; set; }
    

        public static LABDET GetLABDET(string REFERENCE,string facility)
        {
            LABDET labdet = new LABDET();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "LABDET_Get"; //"spGetPatient";
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
                    labdet.REFERENCE = reader["reference"].ToString();
                    labdet.NAME = reader["name"].ToString();
                    labdet.PATIENTNO = reader["patientno"].ToString();
                    labdet.GROUPCODE = reader["groupcode"].ToString();
                    labdet.SEX = reader["sex"].ToString();
                    labdet.ADDRESS1 = reader["address1"].ToString();
                    labdet.BIRTHDATE = (DateTime)reader["birthdate"];
                    labdet.REFERAL = reader["referal"].ToString();
                    labdet.TRANS_DATE = (DateTime)reader["trans_date"];
                    labdet.BILL_NO = reader["bill_no"].ToString();
                    labdet.PAY_NO = reader["pay_no"].ToString();
                    labdet.BILLSELF = reader["billself"].ToString();
                    labdet.FACILITY = reader["facility"].ToString();
                    labdet.POSTED = (bool)reader["posted"];
                    labdet.POST_DATE = (DateTime)reader["post_date"];
                    labdet.CROSSREF = reader["crossref"].ToString();
                    labdet.AGE  = reader["age"].ToString();
                    labdet.GROUPHTYPE  = reader["grouphtype"].ToString();
                    labdet.GROUPHEAD  = reader["grouphead"].ToString();
                    labdet.GHGROUPCODE  = reader["GHGROUPCODE"].ToString();
                    labdet.OCCUPATION = reader["occupation"].ToString();
                    labdet.AMOUNT = (Decimal)reader["amount"];
                    labdet.MAT_CUMAMT = (Decimal)reader["mat_cumamt"];
                    labdet.OPERATOR  = reader["operator"].ToString();
                    labdet.DTIME = (DateTime)reader["dtime"];
                    labdet.PHONE  = reader["phone"].ToString();
                    labdet.EMAIL  = reader["email"].ToString();
                    labdet.RECID = (Int32)reader["recid"];
                                                                  
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
                MessageBox.Show("" + ex, "Get Investigation Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }
            return labdet;
        }



    }
}