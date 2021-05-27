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
    public class DOCTORS
    {
        public string REFERENCE { get; set; }
        public string SURNAME { get; set; }
        public string OTHERS { get; set; }
        public string NAME { get; set; }
        public DateTime PROF_REGD { get; set; }
        public string REG_NO { get; set; }
        public string RECTYPE { get; set; }
        public string STAFF_NO { get; set; }
        public string ADD_DATA { get; set; }
        public decimal SEC_LEVEL { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public decimal CONSCHARGE { get; set; }
        public bool UNRESTRICTED { get; set; }
        public string LOGINDATE { get; set; }
        public string LOGOUTDATE { get; set; }
        public bool LOGINACTIVE { get; set; }
        public string FACILITY { get; set; }
        public string CONSROOM { get; set; }
        public bool EXCLUSIVEDATA { get; set; }
        public bool GLOBALACCESS { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string STATUS { get; set; }
        public static DOCTORS GetDOCTORS(string xreference)
        {
            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            DOCTORS doc = new DOCTORS();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "DOCTORS_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", xreference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    doc.REFERENCE = reader["reference"].ToString();
                    doc.SURNAME = reader["surname"].ToString();
                    doc.OTHERS = reader["others"].ToString();
                    if (reader["prof_regd"] != DBNull.Value)
                        doc.PROF_REGD = (DateTime)reader["prof_regd"];
                    else
                    {
                        doc.PROF_REGD = DateTime.Now.Date;
                    }
                    doc.NAME = reader["name"].ToString();
                    doc.REG_NO = reader["reg_no"].ToString();
                    doc.RECTYPE = reader["rectype"].ToString();
                    doc.STAFF_NO = reader["staff_no"].ToString();
                    doc.ADD_DATA = reader["add_data"].ToString();
                    doc.SEC_LEVEL = (Decimal)reader["sec_level"];
                    doc.POSTED = (bool)reader["posted"];
                    if (reader["post_date"] != DBNull.Value)
                        doc.POST_DATE = (DateTime)reader["post_date"];
                    doc.CONSCHARGE = (Decimal)reader["conscharge"];
                    doc.UNRESTRICTED = (bool)reader["unrestricted"];
                    doc.LOGINDATE = reader["logindate"].ToString();
                    doc.LOGOUTDATE = reader["logoutdate"].ToString();
                    doc.LOGINACTIVE = (bool)reader["loginactive"];
                    doc.FACILITY = reader["facility"].ToString();
                    doc.CONSROOM = reader["consroom"].ToString();
                    doc.EXCLUSIVEDATA = (bool)reader["exclusivedata"];
                    doc.GLOBALACCESS = (bool)reader["globalaccess"];
                    doc.PHONE = reader["phone"].ToString();
                    doc.EMAIL = reader["email"].ToString();
                    doc.STATUS = reader["status"].ToString();
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
                MessageBox.Show("" + ex, "Get Docs Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return doc;
        }
        public static bool DeleteRecord(string reference)
        {

            SqlConnection connection = msmr.Dataaccess.codeConnection();
            SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.CommandText = "DOCTORS_Delete";
            deleteCommand.Connection = connection;
            deleteCommand.CommandType = CommandType.StoredProcedure;

            deleteCommand.Parameters.AddWithValue("@Reference", reference);
            try
            {
                connection.Open();
                int count = deleteCommand.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException ex)
            {
                //throw ex;

                MessageBox.Show("" + ex, "Delete Med.Staff Record", MessageBoxButtons.OK,
 MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }

        }

    }
}