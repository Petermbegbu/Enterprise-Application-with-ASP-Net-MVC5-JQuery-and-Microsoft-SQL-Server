using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Configuration;

using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using System.IO;

using OtherClasses.Models;

using System.Data.SqlClient;
using System.Data;
using msfunc;
using GLS;
using SCS.DataAccess;
using HPL.BissClass;

namespace OtherClasses
{
    public class ErpFunc
    {
        public static string tX(string a)
        {
            return "throwX`" + a;
        }

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
                try
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name.ToUpper() == column.ColumnName.ToUpper())
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                    }
                }
                catch { }
            }
            return obj;
        }
        private static string DBSqlCStr(string dbName)
        {
            string cString = "";
            if (dbName== "AR_DATA") { cString = ConfigurationManager.ConnectionStrings["arConnectionString"].ConnectionString; }
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
            DataTable retDT =new DataTable();

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

            Object rett = null;
            foreach(var a in ConvertDtToList<T>(retDT)) { rett = a; continue; }

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

            Object rett = null;
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

            string folderName = "ReportPDFs";

            /*
            bool cFgPthXsts = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["PDFFolder"]) ?
                true : false;

            string iisPath = cFgPthXsts ? ConfigurationManager.AppSettings["PDFFolder"] :
                CLGet("AR_DATA", "SELECT REGUSER FROM ARCONTROL ORDER BY RECID OFFSET 2 " +
                "ROWS FETCH FIRST 1 ROWS ONLY").Trim() + "\\ ";

            using (FileStream stream = new FileStream(iisPath.Trim() + pdfName, FileMode.Create))
            {
                stream.Write(Bytes, 0, Bytes.Length);
            }

            string fName = cFgPthXsts ? null : iisPath;
            if (!cFgPthXsts)
            {
                string[] fName2 = fName.Split('\\');
                fName = "../" + fName2[fName2.Length - 2] + "/";
            }

            return cFgPthXsts ?
                "../ReportPDFs/" + pdfName
                :
                fName + pdfName;
            */

            using (FileStream stream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/" + folderName + "/" + pdfName, FileMode.Create))
            {
                stream.Write(Bytes, 0, Bytes.Length);
            }

            return "../" + folderName + "/" + pdfName;
        }

        public class HplRptViewer
        {
            //string xuser = msfunc.bissclass.sysGlobals.user_name;
            //string user = msfunc.bissclass.sysGlobals.user_name; // "Adisystems Limited";
            //string rptheader = "STOCK AVAILABILITY REPORT AT " + DateTime.Now.ToLongDateString();
            //string user = "Adisystems Limited";
            //private int m_currentPageIndex;
            //private IList<Stream> m_streams;
            string rptheader, username = string.IsNullOrWhiteSpace(msfunc.bissclass.sysGlobals.user_name) ? "ADI SYSTEMS LIMITED" : msfunc.bissclass.sysGlobals.user_name, rptfooter, rptcriteria, slogan, woperator = bissclass.sysGlobals.WOPERATOR, clremark1, clremark2, logo, rpttype, invnotes, mcurrency, currencyname, rptfile, mreference, posAddendum;
            string[] addressa_ = new string[7], mischargetxta_ = new string[6], currencya_ = new string[2];
            decimal invamount;
            DateTime datefrom, dateto;
            //DataTable dtstock = Dataaccess.GetAnytable("stock", "SMS", "select * from stock where store = 'PH'", false);
            DataSet ds = new DataSet();
            bool toprint, isdataset;

            public static HP_DATA.REPORTS RptVwr(string xcaption, string xrptheader, string xrptfooter,
                string xrptcriteria, string xcompanycode, string rpttype_INV_DN_CN_POS, string invref,
                decimal xinvamount, string xcurrency, string xlocalcur, string xcurname, DataSet xds,
                DateTime xdatefrom, DateTime xdateto, bool xtoprint, bool xisdataset, string rptsize,
                string rdlc, string sql)
            {
                var aa = new HplRptViewer();

                return aa.frmHPLReportViewer(xcaption, xrptheader, xrptfooter, xrptcriteria, xcompanycode,
                    rpttype_INV_DN_CN_POS, invref, xinvamount, xcurrency, xlocalcur, xcurname, xds, xdatefrom,
                    xdateto, xtoprint, xisdataset, rptsize, rdlc, sql);
            }

            HP_DATA.REPORTS frmHPLReportViewer(string xcaption, string xrptheader, string xrptfooter,
                string xrptcriteria, string xcompanycode, string rpttype_INV_DN_CN_POS,
                string invref, decimal xinvamount, string xcurrency, string xlocalcur,
                string xcurname, DataSet xds, DateTime xdatefrom, DateTime xdateto,
                bool xtoprint, bool xisdataset, string rptsize, string rdlc, string sql) //,string xrdlcfile,string xsql)
            {
                rptheader = xrptheader; rptfooter = xrptfooter; rptcriteria = xrptcriteria; invamount = xinvamount;
                rpttype = rpttype_INV_DN_CN_POS; mcurrency = xcurrency == "" ? xlocalcur : xcurrency; currencyname = xcurname;
                toprint = xtoprint;
                datefrom = xdatefrom; dateto = xdateto; ds = xds; isdataset = xisdataset; mreference = invref;

                // rdlcfile = xrdlcfile; sql = xsql;
                if (xrptcriteria != "")
                    rptcriteria = "Report Criteria : " + rptcriteria;
                if (rpttype == "INV" || rpttype == "DN" || rpttype == "CN" || rpttype == "STATMT" || rpttype == "MULTSTATMT" || rpttype == "POS" || rpttype == "RECEIPTSTD")
                    getInvoiceHeader(xcompanycode == "" ? "01" : xcompanycode, invref);
                currencya_[0] = currencya_[1] = "";
                mcurrency = mcurrency == "" ? bissclass.sysGlobals.mlocalcur : mcurrency;
                if (mcurrency != "")
                {
                    DataTable dtcur = Dataaccess.GetAnytable("CURTABLE", "CODES", "select curmain,curunit from curtable where ccode = '" + mcurrency + "'", false);
                    if (dtcur.Rows.Count > 0)
                    {
                        currencya_[0] = dtcur.Rows[0]["curmain"].ToString().Trim();
                        currencya_[1] = dtcur.Rows[0]["curunit"].ToString().Trim();
                    }
                }






                //METHOD SHOW -START

                var rett = new HP_DATA.REPORTS();

                rptfile = rdlc;

                rett.RptPath = @"Reports\" + rdlc; //reportViewer1.LocalReport.ReportPath

                if (!isdataset)
                {
                    ds = Dataaccess.GetDataToDataset("", "HPL", sql, false);
                }
                ReportParameter[] param = new ReportParameter[4];
                if (rpttype == "CONTRACTDETAILS" || rpttype == "CUSTOMERLIST" || rpttype == "LOCATION" || rpttype == "ESTABLISHMENT")
                {
                    //param = new ReportParameter[4];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter);
                    param[3] = new ReportParameter("rptcriteria", rptcriteria);
                }
                else if (rpttype == "POS")
                {
                    param = new ReportParameter[6];
                    param[0] = new ReportParameter("company", username);
                    param[1] = new ReportParameter("slogan", slogan);
                    param[2] = new ReportParameter("addressa_", addressa_);
                    param[3] = new ReportParameter("pos_payreference", mreference);
                    param[4] = new ReportParameter("woperator", woperator);
                    param[5] = new ReportParameter("POSAddendum", posAddendum);
                    //param[6] = new ReportParameter("clogo", "file:\\" + @logo);
                }
                else if (rpttype == "RECEIPTSTD")
                {
                    param = new ReportParameter[6];
                    param[0] = new ReportParameter("company", username);
                    param[1] = new ReportParameter("slogan", slogan);
                    param[2] = new ReportParameter("addressa_", addressa_);
                    param[3] = new ReportParameter("clogo", "file:\\" + @logo);
                    param[4] = new ReportParameter("woperator", woperator);
                    param[5] = new ReportParameter("currencya_", currencya_);
                }
                if (rpttype == "INV")
                {
                    param = new ReportParameter[11];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("slogan", slogan);
                    param[2] = new ReportParameter("addressa_", addressa_);
                    param[3] = new ReportParameter("woperator", woperator);
                    param[4] = new ReportParameter("mischargetxta_", mischargetxta_);
                    param[5] = new ReportParameter("mcurrency", "");
                    param[6] = new ReportParameter("clogo", "file:\\" + @logo);
                    param[7] = new ReportParameter("invoicetype", "INV");
                    param[8] = new ReportParameter("invoiceamt", invamount.ToString());
                    param[9] = new ReportParameter("invnotes", invnotes);
                    param[10] = new ReportParameter("currencyname", currencyname);
                }
                else if (rpttype == "PMT" || rpttype == "PMTBANKCOLUMN" || rpttype == "INV-PMT")
                {
                    param = new ReportParameter[5];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("currencyname", currencyname);
                    param[2] = new ReportParameter("rptheader", rptheader);
                    param[3] = new ReportParameter("rptfooter", rptfooter);
                    param[4] = new ReportParameter("mcurrency", mcurrency);
                }
                else if (rpttype == "POSSUMMARY")
                {
                    param = new ReportParameter[3];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter);
                }

                else if (rpttype == "MULTSTATMT")
                {
                    param = new ReportParameter[8];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("slogan", slogan);
                    param[2] = new ReportParameter("mcurrency", "");
                    param[3] = new ReportParameter("currencyname", currencyname);
                    param[4] = new ReportParameter("datefrom", datefrom.ToShortDateString());
                    param[5] = new ReportParameter("dateto", dateto.ToShortDateString());
                    param[6] = new ReportParameter("rptheader", rptheader);
                    param[7] = new ReportParameter("rptfooter", rptfooter);
                }
                else if (rpttype == "STATMT")
                {
                    param = new ReportParameter[10];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("slogan", slogan);
                    param[2] = new ReportParameter("mcurrency", "");
                    param[3] = new ReportParameter("clogo", "file:\\" + @logo);
                    param[4] = new ReportParameter("invoicetype", "STATMT");
                    param[5] = new ReportParameter("mbalbf", invamount.ToString());
                    param[6] = new ReportParameter("currencyname", currencyname);
                    param[7] = new ReportParameter("datefrom", datefrom.ToShortDateString());
                    param[8] = new ReportParameter("dateto", dateto.ToShortDateString());
                    param[9] = new ReportParameter("addressa_", addressa_);

                }
                else if (rpttype == "BALDUE" || rpttype == "UNVINV") //CUSTOMER BALANCES DUE
                {
                    param = new ReportParameter[5];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter);
                    param[3] = new ReportParameter("rptcriteria", rptcriteria);
                    param[4] = new ReportParameter("datefrom", datefrom.ToShortDateString());
                }
                else if (rpttype == "CUSTLIST" || rpttype == "MiscCharges") //CUSTOMER LISTING
                {
                    param = new ReportParameter[4];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter);
                    param[3] = new ReportParameter("rptcriteria", rptcriteria);
                }
                else if (rpttype == "PAYSCHEDULE")
                {
                    param = new ReportParameter[4];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("currencyname", currencyname);
                    param[3] = new ReportParameter("rptfooter", rptfooter);
                }
                else if (rpttype == "PAYMENTS" || rpttype == "RecordList")
                {
                    param = new ReportParameter[4];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter == null ? " " : rptfooter);
                    param[3] = new ReportParameter("rptcriteria", rptcriteria);

                }

                rett.RptParams = param; // reportViewer1.LocalReport.SetParameters(param);

                rett.RptDataSrc = new ReportDataSource("DataSet1", ds.Tables[0]);

                //METHOD SHOW -END

                return rett;
            }

            void getInvoiceHeader(string companycode, string invoice_reference)
            {
                DataTable dtsetup = Dataaccess.GetAnytable("arsetup", "HPL", "select * from HPLsetup where company = '" + companycode + "'", false);
                posAddendum = dtsetup.Rows[0]["posaddendum"].ToString();
                for (int i = 0; i < dtsetup.Rows.Count; i++)
                {
                    if (dtsetup.Rows[i]["name"].ToString() != "")
                    {
                        username = dtsetup.Rows[i]["name"].ToString().Trim();
                        slogan = dtsetup.Rows[i]["slogan"].ToString().Trim();
                        addressa_[0] = dtsetup.Rows[i]["address1"].ToString().Trim();
                        addressa_[1] = dtsetup.Rows[i]["address2"].ToString().Trim();
                        addressa_[2] = dtsetup.Rows[i]["address3"].ToString().Trim();
                        addressa_[3] = dtsetup.Rows[i]["address4"].ToString().Trim();
                        addressa_[4] = dtsetup.Rows[i]["address5"].ToString().Trim();
                        addressa_[5] = dtsetup.Rows[i]["address6"].ToString().Trim();
                        addressa_[6] = dtsetup.Rows[i]["address7"].ToString().Trim();
                        clremark1 = dtsetup.Rows[i]["clremarks1"].ToString().Trim();
                        clremark2 = dtsetup.Rows[i]["clremarks2"].ToString().Trim();
                        invnotes = dtsetup.Rows[i]["invnotes"].ToString().Trim();
                        logo = dtsetup.Rows[i]["clogo"].ToString().Trim();
                        //string pic = dtsetup.Rows[i]["clogo"].ToString().Trim();
                        //logo =  new Uri(Context.Server.MapPath(pic)).AbsoluteUri;
                        // logo = Directory.GetFiles(dtsetup.Rows[i]["clogo"].ToString().Trim());
                        //StreamReader logo = new StreamReader(Context.Server.MapPath(pic));
                        break;
                    }
                }
                if (rpttype == "INV" || rpttype == "POS")
                {
                    //get misc. charges - disc. etc.
                    DataTable dtmischarge = Dataaccess.GetAnytable("miscchg", "AR", "select descriptn,percentage,amount,ttype from miscchg where reference = '" + invoice_reference + "'", false);
                    string xp;
                    int creditcount = 0, debitcount = 0;
                    if (dtmischarge.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtmischarge.Rows.Count; i++)
                        {
                            xp = (decimal)dtmischarge.Rows[i]["percentage"] > 0m ? dtmischarge.Rows[i]["percentage"].ToString().Trim() + "% " :
                                "";
                            if (dtmischarge.Rows[i]["ttype"].ToString() == "C" &&
                                creditcount < 1)
                            {
                                creditcount++;
                                mischargetxta_[0] = xp + dtmischarge.Rows[i]["descriptn"].ToString().Trim();
                                mischargetxta_[3] = dtmischarge.Rows[i]["amount"].ToString();
                            }
                            else if (debitcount < 4)
                            {
                                debitcount++;
                                mischargetxta_[debitcount] = xp + dtmischarge.Rows[i]["descriptn"].ToString().Trim();
                                mischargetxta_[debitcount + 3] = dtmischarge.Rows[i]["amount"].ToString();
                            }
                        }
                    }

                }
                else if (rpttype == "STATMT")
                {
                    addressa_ = new string[3];
                    DataTable dt = Dataaccess.GetAnytable("", "HPL", "select name,address1 from customer where reference = '" + invoice_reference + "'", false); //invoice_reference passed as customer ref from 'statmt'
                    foreach (DataRow row in dt.Rows)
                    {
                        addressa_[0] = row["address1"].ToString().Trim();
                        addressa_[1] = ""; // row["address2"].ToString().Trim();
                        addressa_[2] = "";
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    if (mischargetxta_[i] == null)
                        mischargetxta_[i] = "";
                }
            }

        }

    }
}