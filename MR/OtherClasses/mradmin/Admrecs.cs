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
    public class Admrecs
    {
        public string REFERENCE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public string FACILITY { get; set; }
        public string UNIT { get; set; }
        public string ROOM { get; set; }
        public string BED { get; set; }
        public decimal RATE { get; set; }
        public DateTime ADM_DATE { get; set; }
        public string TIME { get; set; }
        public string DOCTOR { get; set; }
        public string DIAGNOSIS { get; set; }
        public string DISCHARGE { get; set; }
        public string DISCH_TIME { get; set; }
        public string DISCH_DOCT { get; set; }
        public bool BILLED { get; set; }
        public DateTime DATE_BILLE { get; set; }
        public string REMARKS { get; set; }
        public string REASON { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public string GROUPHEAD { get; set; }
        public string GROUPHTYPE { get; set; }
        public string GROUPCODE { get; set; }
        public decimal ACAMT { get; set; }
        public string GHGROUPCODE { get; set; }
        public decimal DAILYFEEDING { get; set; }
        public decimal DAILYPNC { get; set; }
        public decimal PAYMENTS { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string DIAGNOSIS_ALL { get; set; }
/// <summary>
/// Send blank xreference to check if patient being admitted is already on Admission on difference Reference
/// The checkdischarge parameters - true or false to check if patient is discharged
/// Pass xreference to check to existence of admission reference
/// </summary>
/// <param name="GroupCodeID"></param>
/// <param name="PatientID"></param>
/// <param name="xreference"></param>
/// <param name="checkdischarge"></param>
/// <returns></returns>
        public static Admrecs GetADMRECS(string xreference)
        {
            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            Admrecs admrecs = new Admrecs();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = (!string.IsNullOrWhiteSpace(xreference)) ? "ADMRECS_GetByRef" :  "ADMRECS_Get" ; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", xreference);
 
            try
            {
                connection.Open();
                SqlDataReader reader = (!string.IsNullOrWhiteSpace(xreference)) ? selectCommand.ExecuteReader( CommandBehavior.SingleRow) :
                    selectCommand.ExecuteReader();

                if (reader.Read())
                {
                        admrecs.REFERENCE = reader["reference"].ToString();
                        admrecs.PATIENTNO = reader["patientno"].ToString();
                        admrecs.NAME = reader["name"].ToString();
                        admrecs.FACILITY = reader["facility"].ToString();
                        admrecs.UNIT = reader["unit"].ToString();
                        admrecs.ROOM = reader["room"].ToString();
                        admrecs.BED = reader["bed"].ToString();
                        admrecs.RATE = (Decimal)reader["rate"];
                        admrecs.ADM_DATE = (DateTime)reader["adm_date"];
                        admrecs.TIME = reader["time"].ToString();
                        admrecs.DOCTOR = reader["doctor"].ToString();
                        admrecs.DIAGNOSIS = reader["diagnosis"].ToString();
                        admrecs.DISCHARGE = reader["discharge"].ToString();
                        admrecs.DISCH_TIME = reader["disch_time"].ToString();
                        admrecs.DISCH_DOCT = reader["disch_doct"].ToString();
                        admrecs.BILLED = (bool)reader["billed"];
                        admrecs.DATE_BILLE = (DateTime)reader["date_bille"];
                        admrecs.REMARKS = reader["remarks"].ToString();
                        admrecs.REASON = reader["reason"].ToString();
                        admrecs.POSTED = (bool)reader["posted"];
                        admrecs.POST_DATE = (DateTime)reader["post_date"];
                        admrecs.GROUPHEAD = reader["grouphead"].ToString();
                        admrecs.GROUPHTYPE = reader["grouphtype"].ToString();
                        admrecs.GROUPCODE = reader["groupcode"].ToString();
                        admrecs.ACAMT = (Decimal)reader["acamt"];
                        admrecs.GHGROUPCODE = reader["GHGROUPCODE"].ToString();
                        admrecs.DAILYFEEDING = (Decimal)reader["dailyfeeding"];
                        admrecs.DAILYPNC = (Decimal)reader["dailypnc"];
                        admrecs.PAYMENTS = (Decimal)reader["payments"];
                        admrecs.OPERATOR = reader["operator"].ToString();
                        admrecs.DTIME = (DateTime)reader["dtime"];
                        admrecs.DIAGNOSIS_ALL = reader["DIAGNOSIS_ALL"].ToString();
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
                MessageBox.Show("" + ex, "Get In-Patient Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return admrecs;
        }
        public static void UpdateAdmrecAmounts(string xreference, decimal bill, decimal paymt)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "Admrecs_UpdateaAmount";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;
 
            insertCommand.Parameters.AddWithValue("@Reference", xreference);
            insertCommand.Parameters.AddWithValue("@ACAMT",bill);
            insertCommand.Parameters.AddWithValue("@PAYMENTS",paymt );
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
 
                MessageBox.Show("SQL access" + ex, "Admission Amt Update", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
        public static void UpdateDischarge(string xreference, string dischargedate, string dischargetime, string dischargedoc, bool billed, DateTime datebilled, string xremarks)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "Admrecs_Discharge";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Reference", xreference);
            insertCommand.Parameters.AddWithValue("@discharge", dischargedate);
            insertCommand.Parameters.AddWithValue("@disch_time", dischargetime);
            insertCommand.Parameters.AddWithValue("@disch_doct", dischargedoc);
            insertCommand.Parameters.AddWithValue("@billed", billed);
            insertCommand.Parameters.AddWithValue("@date_bille", datebilled);
            insertCommand.Parameters.AddWithValue("@remarks", xremarks);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                MessageBox.Show("SQL access" + ex, "Admissions Discharge Updaet", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}