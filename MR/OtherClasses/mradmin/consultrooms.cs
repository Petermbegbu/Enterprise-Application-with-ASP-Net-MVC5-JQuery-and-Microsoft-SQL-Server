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
    public class consultrooms
    {
        public string REFERENCE { get; set; }
        public string NAME { get; set; }
        public string LOCATION { get; set; }
        public string LASTDOC { get; set; }
        public string LOGIN_DATETIME { get; set; }
        public string LOGOUT_DATETIME { get; set; }
        public decimal FREQUENCY { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool OCCUPIED { get; set; }
        public string DOCNAME { get; set; }
        public decimal ACTIVELOGS { get; set; }


        public static DataTable GetCONSULTROOMS()
        {
            consultrooms consrm = new consultrooms();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "CONSULTROOMS_GetList"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            //   selectCommand.Parameters.AddWithValue("@GroupCodeID", GroupCodeID);
            //   selectCommand.Parameters.AddWithValue("@PatientID", PatientID);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }

        public static consultrooms GetDocInCONSULTROOMS(string xdocname, DateTime xlogoutdate)
        {
            consultrooms consrm = new consultrooms();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "CONSULTROOMS_CheckDocActiveLogin"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@DOCNAME", xdocname);
            selectCommand.Parameters.AddWithValue("@logout_datetime", xlogoutdate);
 
           try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    consrm.REFERENCE = reader["reference"].ToString();
                    consrm.NAME = reader["name"].ToString();
                    consrm.LOCATION = reader["location"].ToString();
                    consrm.LASTDOC = reader["lastdoc"].ToString();
                    consrm.LOGIN_DATETIME = reader["login_datetime"].ToString();
                    consrm.LOGOUT_DATETIME = reader["logout_datetime"].ToString();
                    consrm.FREQUENCY = (Decimal)reader["frequency"];
                    consrm.POSTED = (bool)reader["posted"];
                    consrm.POST_DATE = (DateTime)reader["post_date"];
                    consrm.OCCUPIED = (bool)reader["occupied"];
                    consrm.DOCNAME = reader["docname"].ToString();
                    consrm.ACTIVELOGS = (Decimal)reader["activelogs"];
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
                MessageBox.Show("" + ex, "Get Consult Room Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return consrm;
        }

        public static bool UpdateConsultRooms(string xREFERENCE, string xNAME, string xLOCATION, string xLASTDOC, string  xLOGIN_DATETIME, string xLOGOUT_DATETIME,decimal xFREQUENCY, bool xPOSTED, DateTime xPOST_DATE, bool xOCCUPIED, string xDOCNAME, decimal xACTIVELOGS,bool toadd)
        {
            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (toadd) ? "CONSULTROOMS_Add" : "CONSULTROOMS_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference",xREFERENCE );
            insertCommand.Parameters.AddWithValue("@name",xNAME );
            insertCommand.Parameters.AddWithValue("@location",xLOCATION );
            insertCommand.Parameters.AddWithValue("@lastdoc",xLASTDOC );
            insertCommand.Parameters.AddWithValue("@login_datetime",xLOGIN_DATETIME );
            insertCommand.Parameters.AddWithValue("@logout_datetime",xLOGOUT_DATETIME );
            insertCommand.Parameters.AddWithValue("@frequency",xFREQUENCY );
            insertCommand.Parameters.AddWithValue("@posted",xPOSTED );
            insertCommand.Parameters.AddWithValue("@post_date",xPOST_DATE );
            insertCommand.Parameters.AddWithValue("@occupied",xOCCUPIED );
            insertCommand.Parameters.AddWithValue("@docname",xDOCNAME );
            insertCommand.Parameters.AddWithValue("@activelogs",xACTIVELOGS );
   /*         if (!toadd)
            {
                insertCommand.Parameters.AddWithValue("@singlebyname", singlebyname);
            }*/

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show(" " + ex, "Consult Rooms Details", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;

        }    
    
    }
}