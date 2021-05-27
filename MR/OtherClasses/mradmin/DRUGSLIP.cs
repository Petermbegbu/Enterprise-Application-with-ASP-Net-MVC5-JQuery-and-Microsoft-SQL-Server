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
    public class DRUGSLIP
    {
        public string DRUG { get; set; }
        public string LAB1 { get; set; }
        public string LAB2 { get; set; }
        public string LAB3 { get; set; }
        public string LAB4 { get; set; }
        public string LAB5 { get; set; }
        public string LAB6 { get; set; }
        public string LAB7 { get; set; }
        public string LAB8 { get; set; }
        public string LFREETEXT { get; set; }

        public static DRUGSLIP GetDRUGSLIP(string reference)
        {
            DRUGSLIP drugslip = new DRUGSLIP();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "DRUGSLIP_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Drug",reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    drugslip.DRUG = reader["drug"].ToString();
                    drugslip.LAB1 = reader["lab1"].ToString();
                    drugslip.LAB2 = reader["lab2"].ToString();
                    drugslip.LAB3 = reader["lab3"].ToString();
                    drugslip.LAB4 = reader["lab4"].ToString();
                    drugslip.LAB5 = reader["lab5"].ToString();
                    drugslip.LAB6 = reader["lab6"].ToString();
                    drugslip.LAB7 = reader["lab7"].ToString();
                    drugslip.LAB8 = reader["lab8"].ToString();
                    drugslip.LFREETEXT = reader["lfreetext"].ToString();
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
                MessageBox.Show("" + ex, "Get DrugSlip Definition ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return drugslip;
        }
        public static bool WriteDrugSlip(string xreference,bool newrec,string lab1,string lab2,string lab3,
            string lab4,string lab5,string lab6,string lab7,string lab8,string lfreetext)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = newrec ?  "DRUGSLIP_Add" : "DRUGSLIP_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@drug",xreference);
            insertCommand.Parameters.AddWithValue("@lab1",lab1);
            insertCommand.Parameters.AddWithValue("@lab2",lab2);
            insertCommand.Parameters.AddWithValue("@lab3",lab3);
            insertCommand.Parameters.AddWithValue("@lab4",lab4);
            insertCommand.Parameters.AddWithValue("@lab5",lab5);
            insertCommand.Parameters.AddWithValue("@lab6",lab6);
            insertCommand.Parameters.AddWithValue("@lab7",lab7);
            insertCommand.Parameters.AddWithValue("@lab8", lab8);
            insertCommand.Parameters.AddWithValue("@lfreetext", lfreetext);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                MessageBox.Show("" + ex, "DRUGSLIP UPDATE", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        public static bool DeleteDefinition(string Reference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "Drugslip_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Reference", Reference);
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
                MessageBox.Show(" " + ex, "Delete Prescription Slip Definition", MessageBoxButtons.OK,
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