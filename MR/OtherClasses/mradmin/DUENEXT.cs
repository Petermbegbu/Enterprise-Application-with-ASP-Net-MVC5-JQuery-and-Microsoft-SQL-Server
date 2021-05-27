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
    public class DUENEXT
    {
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string STK_ITEM { get; set; }
        public string STK_DESC { get; set; }
        public string STORE { get; set; }
        public decimal DOSE { get; set; }
        public string UNIT { get; set; }
        public decimal COST { get; set; }
        public string NURSE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public decimal ITEMNO { get; set; }
        public decimal STKBAL { get; set; }
        public string TIMEGIVEN { get; set; }
        public string TYPE { get; set; }
        public string DUETIME { get; set; }
        public string METHODADM { get; set; }
        public string REMARKS { get; set; }
        public string REFERENCE { get; set; }
        public decimal PACKQTY { get; set; }
        public decimal PACKCOST { get; set; }
        public decimal UNITCOST { get; set; }
        public decimal BILLQTY{ get; set; }
        public string BILLQTYUNIT { get; set; }
    

        public static DataTable GetDUENEXT(string reference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "DUENEXT_Get"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
 
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static bool DeleteDUENEXT(string Reference)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "DUENEXT_Delete";
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
                //throw ex;

                MessageBox.Show(" " + ex, "Delete Treatment Chart Item", MessageBoxButtons.OK,
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