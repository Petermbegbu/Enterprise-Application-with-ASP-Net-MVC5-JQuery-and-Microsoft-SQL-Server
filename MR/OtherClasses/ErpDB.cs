using System;
using System.Collections.Generic;
using System.Reflection;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace OtherClasses
{
    public class ErpDB
    {
        public static List<T> ConvertDtToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        private static string DBSqlCStr(string dbName)
        {
            string cString = "";
            if (dbName == "AR_DATA") { cString = ConfigurationManager.ConnectionStrings["arConnectionString"].ConnectionString; }
            else if (dbName == "SCS01") { cString = ConfigurationManager.ConnectionStrings["SCS01ConnectionString"].ConnectionString; }
            else if (dbName == "SYSCODETABS") { cString = ConfigurationManager.ConnectionStrings["codesConnectionString"].ConnectionString; }
            else if (dbName == "GLS01") { cString = ConfigurationManager.ConnectionStrings["glConnectionString"].ConnectionString; }
            else if (dbName == "APS01") { cString = ConfigurationManager.ConnectionStrings["apConnectionString"].ConnectionString; }
            else if (dbName == "FAS01") { cString = ConfigurationManager.ConnectionStrings["faConnectionString"].ConnectionString; }
            else if (dbName == "HP_DATA") { cString = ConfigurationManager.ConnectionStrings["hplConnectionString"].ConnectionString; }
            else if (dbName == "PAYPER") { cString = ConfigurationManager.ConnectionStrings["ppConnectionString"].ConnectionString; }
            else if (dbName == "HOMS") { cString = ConfigurationManager.ConnectionStrings["hmConnectionString"].ConnectionString; }
            else if (dbName == "MR_DATA") { cString = ConfigurationManager.ConnectionStrings["MR_DATAConnectionString"].ConnectionString; }
            return cString;
        }


        public static string CLGet(string ConStr, string SqlSelectStatement)
        {
            //RETURNS THE DATA IN A SINGLE CELL

            SqlConnection connect1 = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork1 = new SqlCommand(SqlSelectStatement, connect1);

            connect1.Open();
            string cellData = Convert.ToString(doWork1.ExecuteScalar()).Trim();
            connect1.Close();
            return cellData;
        }
        public static string CLGet(string ConStr, string SqlSelectStatement, params string[] SqlParameters)
        {
            //RETURNS THE DATA IN A SINGLE CELL

            SqlConnection connect1 = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork1 = new SqlCommand(SqlSelectStatement, connect1);
            for (int i = 1; i <= SqlParameters.Length; i++) { doWork1.Parameters.AddWithValue("@p" + i, SqlParameters[--i]); i++; }

            connect1.Open();
            string cellData = Convert.ToString(doWork1.ExecuteScalar()).Trim();
            connect1.Close();
            return cellData;
        }


        public static DataTable RsGetDT(/* AR_DATA, SCS01, SYSCODETABS, GLS01, AP01, FAS01, HP_DATA, PAYPER, HOMS, MR_DATA */
            string ConStr, string SqlSelectStatement)
        {
            //RETURNS THE NO OF ROW DATA FOUND IN A DATATABLE

            SqlConnection connect1 = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork1 = new SqlCommand(SqlSelectStatement, connect1);

            connect1.Open();

            SqlDataReader reader1 = doWork1.ExecuteReader();
            DataTable retDT = new DataTable();

            if (reader1.HasRows)
            {
                retDT.Load(reader1);
                connect1.Close();
            }
            else { connect1.Close(); }

            return retDT;
        }
        public static DataTable RsGetDT(/* AR_DATA, SCS01, SYSCODETABS, GLS01, AP01, FAS01, HP_DATA, PAYPER, HOMS, MR_DATA */
            string ConStr, string SqlSelectStatement, params string[] SqlParameters)
        {
            //RETURNS THE NO OF ROW DATA FOUND IN A DATATABLE

            SqlConnection connect1 = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork1 = new SqlCommand(SqlSelectStatement, connect1);
            for (int i = 1; i <= SqlParameters.Length; i++) { doWork1.Parameters.AddWithValue("@p" + i, SqlParameters[--i]); i++; }

            connect1.Open();

            SqlDataReader reader1 = doWork1.ExecuteReader();
            DataTable retDT = new DataTable();

            if (reader1.HasRows)
            {
                retDT.Load(reader1);
                connect1.Close();
            }
            else { connect1.Close(); }

            return retDT;
        }


        public static List<T> RsGet<T>(/* AR_DATA, SCS01, SYSCODETABS, GLS01, AP01, FAS01, HP_DATA, PAYPER, HOMS, MR_DATA */
            string ConStr, string SqlSelectStatement)
        {
            //RETURNS THE NO OF ROW DATA FOUND IN A DATATABLE

            SqlConnection connect1 = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork1 = new SqlCommand(SqlSelectStatement, connect1);

            connect1.Open();

            SqlDataReader reader1 = doWork1.ExecuteReader();
            DataTable retDT = new DataTable();

            if (reader1.HasRows)
            {
                retDT.Load(reader1);
                connect1.Close();
            }
            else { connect1.Close(); }

            return ConvertDtToList<T>(retDT);
        }
        public static List<T> RsGet<T>(/* AR_DATA, SCS01, SYSCODETABS, GLS01, AP01, FAS01, HP_DATA, PAYPER, HOMS, MR_DATA */
            string ConStr, string SqlSelectStatement, params string[] SqlParameters)
        {
            //RETURNS THE NO OF ROW DATA FOUND IN A DATATABLE

            SqlConnection connect1 = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork1 = new SqlCommand(SqlSelectStatement, connect1);
            for (int i = 1; i <= SqlParameters.Length; i++) { doWork1.Parameters.AddWithValue("@p" + i, SqlParameters[--i]); i++; }

            connect1.Open();

            SqlDataReader reader1 = doWork1.ExecuteReader();
            DataTable retDT = new DataTable();

            if (reader1.HasRows)
            {
                retDT.Load(reader1);
                connect1.Close();
            }
            else { connect1.Close(); }

            return ConvertDtToList<T>(retDT);
        }


        public static dynamic RGet<T>(/* AR_DATA, SCS01, SYSCODETABS, GLS01, AP01, FAS01, HP_DATA, PAYPER, HOMS, MR_DATA */
            string ConStr, string SqlSelectStatement)
        {
            //RETURNS THE NO OF ROW DATA FOUND IN A DATATABLE

            SqlConnection connect1 = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork1 = new SqlCommand(SqlSelectStatement, connect1);

            connect1.Open();

            SqlDataReader reader1 = doWork1.ExecuteReader();
            DataTable retDT = new DataTable();

            if (reader1.HasRows)
            {
                retDT.Load(reader1);
                connect1.Close();
            }
            else { connect1.Close(); }

            var rett = new object();
            foreach (var a in ConvertDtToList<T>(retDT)) { rett = a; continue; }

            return rett;
        }
        public static dynamic RGet<T>(/* AR_DATA, SCS01, SYSCODETABS, GLS01, AP01, FAS01, HP_DATA, PAYPER, HOMS, MR_DATA */
            string ConStr, string SqlSelectStatement, params string[] SqlParameters)
        {
            //RETURNS THE NO OF ROW DATA FOUND IN A DATATABLE

            SqlConnection connect1 = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork1 = new SqlCommand(SqlSelectStatement, connect1);
            for (int i = 1; i <= SqlParameters.Length; i++) { doWork1.Parameters.AddWithValue("@p" + i, SqlParameters[--i]); i++; }

            connect1.Open();

            SqlDataReader reader1 = doWork1.ExecuteReader();
            DataTable retDT = new DataTable();

            if (reader1.HasRows)
            {
                retDT.Load(reader1);
                connect1.Close();
            }
            else { connect1.Close(); }

            var rett = new object();
            foreach (var a in ConvertDtToList<T>(retDT)) { rett = a; continue; }

            return rett;
        }


        public static int RwAlter(string ConStr, string SqlAlterStatement)
        {
            //ALTERS THE DATA IN DBs & TABLEs AND ALSO RETURNS THE NUMBER OF ROWS AFFECTED

            SqlConnection connectStr = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork = new SqlCommand(SqlAlterStatement, connectStr);
            connectStr.Open();
            int rwsAltrd = doWork.ExecuteNonQuery();
            connectStr.Close();
            return rwsAltrd;
        }
        public static int RwAlter(string ConStr, string SqlAlterStatement, params string[] SqlParameters)
        {
            //ALTERS THE DATA IN DBs & TABLEs AND ALSO RETURNS THE NUMBER OF ROWS AFFECTED
            //MULTIPLE AND SINGLE PARAMETER VALUES ARE TO BE PASSED IN A STRING FORMAT
            //eg MYDalter(dSqlConnectionString, dSqlAlterStatement,"joshua ola peter..this is a single parameter value")=> 1 parameter passing
            //eg MYDalter(dSqlConnectionString, dSqlAlterStatement,"joshua ola peter..this is a single parameter value","d second parameter value","etc")=> 3 parameter passing
            //NOTE: THE PARAMETER IN dSqlAlterStatement MUST BE NAMED USING @p1, @p2, @p3 ... AND SO ON

            SqlConnection connectStr = new SqlConnection(DBSqlCStr(ConStr));
            SqlCommand doWork = new SqlCommand(SqlAlterStatement, connectStr);
            for (int i = 1; i <= SqlParameters.Length; i++) { doWork.Parameters.AddWithValue("@p" + i, SqlParameters[--i]); i++; }

            connectStr.Open();
            int rwsAltrd = doWork.ExecuteNonQuery();
            connectStr.Close();
            return rwsAltrd;
        }

        public static string SavePDF(ReportViewer viewer)
        {
            string pdfName = DateTime.Today.DayOfYear + "" +
                             DateTime.Today.Year + "-" +
                             DateTime.Now.TimeOfDay.ToString().Replace(":", "").Replace(".", "") + ".pdf";

            string devfo = "<DeviceInfo>" +
              "  <OutputFormat>PDF</OutputFormat>" +
              "  <PageWidth>8.5in</PageWidth>" +
              "  <PageHeight>11in</PageHeight>" +
              "  <MarginTop>0.25in</MarginTop>" +
              "  <MarginLeft>0.25in</MarginLeft>" +
              "  <MarginRight>0.25in</MarginRight>" +
              "  <MarginBottom>0.25in</MarginBottom>" +
              "</DeviceInfo>";

            byte[] Bytes = viewer.LocalReport.Render(format: "PDF", deviceInfo: devfo);
            string iisPath = ConfigurationManager.AppSettings["PDFFolder"];

            using (FileStream stream = new FileStream(iisPath + pdfName, FileMode.Create))
            {
                stream.Write(Bytes, 0, Bytes.Length);
            }
            return "/ReportPDFs/" + pdfName;
        }

    }
}