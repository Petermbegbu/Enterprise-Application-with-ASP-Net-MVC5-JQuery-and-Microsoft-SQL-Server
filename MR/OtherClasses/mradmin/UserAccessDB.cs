using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace SalesControlSystem
{
    public static class UserAccessDB
    {
        public static void SaveRecord(UserAccess access)
        {
           
            SqlConnection conn = SalesDB.GetConnection();

            SqlCommand insertCommand = new SqlCommand("spInsertOrUpdateUser", conn);
            insertCommand.CommandType = CommandType.StoredProcedure;
           
            insertCommand.Parameters.AddWithValue("@Operator", access.Operator);
            insertCommand.Parameters.AddWithValue("@Count", access.Count);
            insertCommand.Parameters.AddWithValue("@Fm1", access.FM1); insertCommand.Parameters.AddWithValue("@Fm2", access.FM2);
            insertCommand.Parameters.AddWithValue("@Fm3", access.FM3); insertCommand.Parameters.AddWithValue("@Fm4", access.FM4);
            insertCommand.Parameters.AddWithValue("@Fm5", access.FM5); insertCommand.Parameters.AddWithValue("@Fm6", access.FM6);
            insertCommand.Parameters.AddWithValue("@Fm7", access.FM7); insertCommand.Parameters.AddWithValue("@Fm8", access.FM8);
            insertCommand.Parameters.AddWithValue("@Fm9", access.FM9); insertCommand.Parameters.AddWithValue("@Fm10", access.FM10);
            insertCommand.Parameters.AddWithValue("@Fm11", access.FM11); insertCommand.Parameters.AddWithValue("@Fm12", access.FM12);
            insertCommand.Parameters.AddWithValue("@Fm13", access.FM13); 

            insertCommand.Parameters.AddWithValue("@Rm1", access.RM1); insertCommand.Parameters.AddWithValue("@Rm2", access.RM2);
            insertCommand.Parameters.AddWithValue("@Rm3", access.RM3); insertCommand.Parameters.AddWithValue("@Rm4", access.RM4);
            insertCommand.Parameters.AddWithValue("@Rm5", access.RM5); insertCommand.Parameters.AddWithValue("@Rm6", access.RM6);
            insertCommand.Parameters.AddWithValue("@Rm7", access.RM7); insertCommand.Parameters.AddWithValue("@Rm8", access.RM8);
            insertCommand.Parameters.AddWithValue("@Rm9", access.RM9); insertCommand.Parameters.AddWithValue("@Rm10", access.RM10);
            insertCommand.Parameters.AddWithValue("@Rm11", access.RM11); insertCommand.Parameters.AddWithValue("@Rm12", access.RM12);
            insertCommand.Parameters.AddWithValue("@Rm13", access.RM13); insertCommand.Parameters.AddWithValue("@Rm14", access.RM14);
            insertCommand.Parameters.AddWithValue("@Rm15", access.RM15); insertCommand.Parameters.AddWithValue("@Rm16", access.RM16);
           //insertCommand.Parameters.AddWithValue("@Rm20", access.RM20);


            insertCommand.Parameters.AddWithValue("@Um1", access.RM1); insertCommand.Parameters.AddWithValue("@Um2", access.RM2);
            insertCommand.Parameters.AddWithValue("@Um3", access.RM3); insertCommand.Parameters.AddWithValue("@Um4", access.RM4);
            insertCommand.Parameters.AddWithValue("@Um5", access.RM5); insertCommand.Parameters.AddWithValue("@UM6", access.RM6);
            insertCommand.Parameters.AddWithValue("@Um7", access.RM7); insertCommand.Parameters.AddWithValue("@Um8", access.RM8);
            insertCommand.Parameters.AddWithValue("@Um9", access.RM9); 

            
            
            insertCommand.Parameters.AddWithValue("@CanDelete", access.CanDelete);
            insertCommand.Parameters.AddWithValue("@CanAlter", access.CanAlter);
            insertCommand.Parameters.AddWithValue("@CanAdd", access.CanAdd);
            insertCommand.Parameters.AddWithValue("@AccessType", access.AccessType);
            insertCommand.Parameters.AddWithValue("@HistYear", access.HistoryYear);

            try
            {
                conn.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        
        public static UserAccess GetUserInfo(string user)
        {
            SqlConnection conn = SalesDB.GetConnection();
            SqlCommand selectCommand = new SqlCommand("spGetUserInfo", conn);
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Operator", user);

            UserAccess access = new UserAccess();
            try
            {
                conn.Open();
                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    access.FM1 = Convert.ToBoolean(reader["FM1"]); access.FM2 = (bool)reader["FM2"];
                    access.FM3 = (bool)reader["FM3"]; access.FM4 = (bool)reader["FM4"];
                    access.FM5 = (bool)reader["FM5"]; access.FM6 = (bool)reader["FM6"];
                    access.FM7 = (bool)reader["FM7"]; access.FM8 = (bool)reader["FM8"];
                    access.FM9 = (bool)reader["FM9"]; access.FM10 = (bool)reader["FM10"];
                    access.FM11 = (bool)reader["FM11"]; access.FM12 = (bool)reader["FM12"];
                    access.FM13 = (bool)reader["FM13"]; 

                    access.RM1 = (bool)reader["RM1"]; access.RM2 = (bool)reader["RM2"];
                    access.RM3 = (bool)reader["RM3"]; access.RM4 = (bool)reader["RM4"];
                    access.RM5 = (bool)reader["RM5"]; access.RM6 = (bool)reader["RM6"];
                    access.RM7 = (bool)reader["RM7"]; access.RM8 = (bool)reader["RM8"];
                    access.RM9 = (bool)reader["RM9"]; access.RM2 = (bool)reader["RM9"];
                    access.RM10 = (bool)reader["RM10"]; access.RM11 = (bool)reader["RM11"];
                    access.RM12 = (bool)reader["RM12"]; access.RM13 = (bool)reader["RM13"];
                    access.RM14 = (bool)reader["RM14"]; access.RM15 = (bool)reader["RM15"];
                    access.RM16 = (bool)reader["RM16"];// access.RM17 = (bool)reader["RM17"];
                    //access.RM18 = (bool)reader["RM18"]; access.RM19 = (bool)reader["RM19"];
                    ////access.RM20 = (bool)reader["RM20"];

                    access.UM1 = (bool)reader["UM1"]; access.UM2 = (bool)reader["UM2"];
                    access.UM3 = (bool)reader["UM3"]; access.UM4 = (bool)reader["UM4"];
                    access.UM5 = (bool)reader["UM5"]; access.UM6 = (bool)reader["UM6"];
                    access.UM7 = (bool)reader["UM7"]; access.UM8 = (bool)reader["UM8"];
                    access.UM9 = (bool)reader["UM9"]; //access.UM10 = (bool)reader["UM10"];
                   // access.UM11 = (bool)reader["UM11"]; access.UM12 = Convert.ToBoolean(reader["UM12"]);

                    access.Operator = reader["OPERATOR"].ToString(); access.HistoryYear = Convert.ToInt32(reader["HISTYEAR"]);
                     access.CanDelete = (bool)reader["CANDELETE"];
                    access.CanAlter = (bool)reader["CANALTER"]; 
                    access.AccessType = (Decimal)reader["ACCESSTYPE"];
                    access.CanAdd = Convert.ToBoolean(reader["CANADD"]);

                    return access;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public static void DeleteUser(string userName)
        {
            SqlConnection conn = SalesDB.GetConnection();
            string deleteStatement = "DELETE FROM ARSTLEV WHERE OPERATOR = @Operator";
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, conn);
            deleteCommand.Parameters.AddWithValue("@Operator", userName);

            try
            {
                conn.Open();
                deleteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}