using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using msfunc;

using OtherClasses.Models;

//using Gizmox.WebGUI.Common;
//using Gizmox.WebGUI.Forms;

namespace HPL.BissClass
{
    public class HPLfunc
    {
        public static decimal tonnagecalc( decimal unit,decimal actual)
        {
            decimal x = 0m;
            if (unit >= 1)
            {
                x = Math.Round((1000/unit),0);
                return Math.Round(actual/x,0);
            }
            return x;
        }
        public static class hpGlobals
        {
            public static string WOPERATOR, WPASS, wpassword, crequired, frmcaption, anycode, search_text, anycode1, auto_search_string, timein, mlocalcur, MLOCSTATE;
            public static bool isstorekeeper,mproduction,masterproc,ismrp,mcanalter,mcandelete, mcanadd,mcanaccess_cp,swainclusive, mcuryear, mvaluameth, mseclink, isairline, updateok, isdcardday,ispayok;
            public static string[] cswa_ = new string[5];
            public static DateTime mta_start;
            public static decimal updateQty,maccesstype, mlastperiod, mpyear;
            public static DateTime update_date,dtstart_date;
        }
        /// <summary>
        /// Updates Control field(s) for automated numbering - xfield = the field in control file to select and update
        /// xrow - the ROW number in the control file, xupdate (bool) whether to update or false to just retrive value,
        /// currentval, applies when updating, toreset - whether to reset counter or not, necessary when value in control file has changed
        /// since the currentval was retrieved. updatestring - the string with which to update field, if xupdate
        /// </summary>
        /// <param name="xfield"></param>
        /// <param name="xrow"></param>
        /// <param name="xupdate"></param>
        /// <param name="currentVal"></param>
        /// <param name="toreset"></param>
        /// <param name="updatestring"></param>
        /// <returns></returns>
        public static Decimal getcontrol_lastnumber(string xfield, int xrow, bool xupdate, decimal currentVal, bool toreset,
            string updatestring)
        {
            DataTable dt = Dataaccess.GetAnytable("hplcontrol", "HPL", "SELECT " + xfield + " FROM hplcontrol "+
                "WHERE RECID = "+xrow, false);
            Decimal xval = (Decimal)dt.Rows[0][xfield];  //(Decimal)dt.Rows[(xrow - 1)][xfield]; //"Last_no"];
            xval++;
            if (xupdate) //we add 1 to whatever that is retrieved; currentval is previous retrieval+1
            {
               /*     if (!string.IsNullOrWhiteSpace(updatestring))
                    {
                        bissclass.UpdateRecords(updatestring, "AR");
                    }*/
                updatestring = "update hplcontrol set "+xfield+" = '" + xval + "' where recid = '"+xrow+"'";
                bissclass.UpdateRecords(updatestring, "HPL");
                   // SqlCommand updateStatement = new SqlCommand(); 
                    //updateStatement.CommandText = (xfield == "LAST_NO") ? "Mrcontrol_LastNo" :
                    //    (xfield == "CHARGNO") ? "Mrcontrol_ChargNo" : (xfield == "PAYNO") ? "Mrcontrol_Payno" :
                    //    (xfield == "ALTERNATENO") ? "Mrcontrol_Alternate" : (xfield == "ADMIT") ? "Mrcontrol_AdmissionNo" :
                    //    (xfield == "ADJNO") ? "Mrcontrol_Adjno" : "Mrcontrol_Attno";
                    //updateStatement.Connection = cs;
                    //updateStatement.CommandType = CommandType.StoredProcedure;

                    //updateStatement.Parameters.AddWithValue("@" + xfield, xval);  //currentVal);
                    //updateStatement.Parameters.AddWithValue("@REC_COUNT", xrow);
                    //cs.Open();
                    //updateStatement.ExecuteNonQuery();
            }
            return xval;
        }
        public static decimal loadvoldisc(decimal xval)
        {
            decimal rate = 0m;
            DataTable dtvd = Dataaccess.GetAnytable("voldisc", "AR", "", false);
            for (int i = 0; i < dtvd.Rows.Count; i++)
            {
                if (xval >= (decimal)dtvd.Rows[i]["min_amt"] && xval <= (decimal)dtvd.Rows[i]["max_amt"])
                {
                    rate = (decimal)dtvd.Rows[i]["rate"];
                    break;
                }
            }
            return rate;
        }
        public static SYSCODETABS.SYSCODETABSvm updatecustomer_curbal(string customercode,decimal amount,string type_DC_debitcredit)
        {
            SYSCODETABS.SYSCODETABSvm mm = new SYSCODETABS.SYSCODETABSvm() { };

            decimal currentbal = 0m;
            string updatestring = "";
            DataTable customer = Dataaccess.GetAnytable("customer", "HPL", "select cur_db,cur_cr from customer where reference = '" + customercode + "'", false);
            currentbal = type_DC_debitcredit == "D" ? (decimal)customer.Rows[0]["cur_db"] : (decimal)customer.Rows[0]["cur_cr"];

            if (type_DC_debitcredit == "D")
                updatestring = "UPDATE [HP_DATA].[dbo].[customer] SET [cur_db] = @cur_db, trans_date = @trans_date," +
                    "posted = '1' where reference = '" + customercode + "'";
            else
            {
                updatestring = "UPDATE [HP_DATA].[dbo].[customer] SET cur_db = @cur_cr, trans_date = @trans_date,"+
                    "posted = '1' where reference = '" + customercode + "'";
            }
            SqlConnection conn = new SqlConnection();
            conn = Dataaccess.hplConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = updatestring;
            insertCommand.Connection = conn;

            if (type_DC_debitcredit == "D")
            {
                insertCommand.Parameters.AddWithValue("@cur_db", currentbal+amount);
            }
            else
            {
                insertCommand.Parameters.AddWithValue("@cur_cr", currentbal+amount);
            }
        //    insertCommand.Parameters.AddWithValue("@customer", customercode);
            insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
            insertCommand.Parameters.AddWithValue("@posted", true);
            
            conn.Open();
            insertCommand.ExecuteNonQuery();
            
            conn.Close();
            
            mm.ERPmiscl.retDecimal = currentbal + amount;
            return mm;
        }
    }
}
/* [C#]

// separator bevel line
label1.AutoSize = false;
label1.Height = 2;
label1.BorderStyle = BorderStyle.Fixed3D;


 * updatestring = "INSERT INTO [AR_DATA].[dbo].[rewardsetup] ([reference],[description],[amttoapoint],[valuetoapoint],[rewardpoint]," +
                        "[valueofreward],[rewardtimeframe],[posted],[post_date],[operator],[opdtime])" +
                        "VALUES (@reference, @description, @amttoapoint, @valuetoapoint, @rewardpoint, @valueofreward, @rewardtimeframe, @posted," +
                        "@post_date, @operator, @opdtime )";*/