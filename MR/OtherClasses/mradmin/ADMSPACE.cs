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
    public class ADMSPACE
    {
        public string FACILITY { get; set; }
        public string NAME { get; set; }
        public string ROOM { get; set; }
        public string BED { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal RATE { get; set; }
        public string OCCUPANT { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POSTED_DATE { get; set; }
        public bool BOOKED { get; set; }
        public DateTime BOOKEDATE { get; set; }
        public static DataTable GetADMSpace(string xfacility)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ADMSPACE_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Facility", xfacility);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static bool DeleteAdmSpace(string xfacility, string room, string bed)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "admspace_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Facility", xfacility);
            deleteStatement.Parameters.AddWithValue("@Room", room);
            deleteStatement.Parameters.AddWithValue("@Bed", bed);
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

                MessageBox.Show(" " + ex, "Delete Patient Admission Space", MessageBoxButtons.OK,
    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// PASS blank space on string occupant to Release Bed Space
        /// </summary>
        /// <param name="xfacility"></param>
        /// <param name="occupant"></param>
        /// <param name="room"></param>
        /// <param name="bed"></param>
        /// <param name="transdate"></param>
        /// <param name="updatetype"></param>
        public static void UpdateAdmOccupant(string xfacility, string occupant, string room,string bed,DateTime transdate)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "Admspace_UpdateOccupant";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Facility", xfacility);
            insertCommand.Parameters.AddWithValue("@occupant", occupant);
            insertCommand.Parameters.AddWithValue("@Room", room);
            insertCommand.Parameters.AddWithValue("@Bed", bed);
            insertCommand.Parameters.AddWithValue("@trans_date", transdate);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                MessageBox.Show("SQL access" + ex, "admspace UPDATE", MessageBoxButtons.OK,
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