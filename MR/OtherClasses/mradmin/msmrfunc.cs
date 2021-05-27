using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using mradmin.DataAccess;
using System.Web;
using msfunc;
using msfunc.Forms;


//bool rememberUserName = LoginUser.RememberMeSet;

namespace mradmin.BissClass
{
	public class msmrfunc
	{
        public static class mrGlobals
		{
			// public const string Prefix = "ID_"; // cannot change
			// public static int Total = 5; // can change because not const


			/*used like so, from master page or anywhere:

			string strStuff = MyGlobals.Prefix + "something";
			textBox1.Text = "total of " + MyGlobals.Total.ToString();
			public static class pubvar */
			public static bool loginok, isphlebotomy;
			public static DateTime mta_start, mpassdt, datefrom, dateto;
			public static bool mfilemode, allsecure, mcandelete, allow_autonomous_clinic, mcentralmpass,
				mcanalter, mcanadd, mpauto, mshieldprice, mattendlink, mtoreload, mgsort, mseclink, isbranch, iserror = false;

			public static string _mmpass, mpath, wpassword, mlocstate, msection, _MPASS, anycode2 = "", anycode1 = "", anycode = "", mbr_cc, search_text, WOPERATOR, global_clinic_code, crequired, frmcaption, mloccountry, msgeventtracker, user_name, pictSelected, waitwindowtext, localcur, lookupCriteria, auto_search_string, soundpath, posAddendum;
			public static int mcuryear, mlengnumb, uhistyr, mlastperiod, mpyear, nwseclevel;
			public static Array tda_;
			//prescription management & service centre request
			public static string mpatientno, mreference, mfacility, rtnstring, mdoctor,
				inp2medhist, mgrouphead, patname, calltype, rtnstringNotes, mgroupcode, admflag, ancRtnNotes, rtnDocdiag, diaRtnNote;
			public static DateTime mtrans_date, dtmin_date = BissClass.msmrfunc.mrGlobals.mta_start;
			public static bool minpatient, mtth,isanc,cashpaying;
			public static string[] requestalerta_ = new string[5];
			public static string[] taggedFromSuspensea_ = new string[99];
			public static int requestalerta_count = 0,returnvalue = 0,recid;
			public static decimal percentageDiscountToApply = 0m;
		}

       // private static void msgBoxHandler(object sender, EventArgs e)
       // {
       //     Form msgForm = sender as Form;
       //     if (msgForm != null)
       //     {
       //         if ((msmrfunc.mrGlobals.msgeventtracker == "SD" || msmrfunc.mrGlobals.msgeventtracker == "PA") &&
       //                 msgForm.DialogResult == DialogResult.Yes)
       //             msmrfunc.mrGlobals.msgeventtracker = "OK";
       //         else if ((msmrfunc.mrGlobals.msgeventtracker == "SD" || msmrfunc.mrGlobals.msgeventtracker == "PA") &&
       //                 msgForm.DialogResult == DialogResult.No)
       //         {
       //             msmrfunc.mrGlobals.msgeventtracker = "TERMINATE";
       //         }
       //         return;
       //         /*   if (msgForm.DialogResult == DialogResult.OK) //Gizmox.WebGui.Forms.DialogResult.Yes)
				   //{
					  // MessageBox.Show("Yes was chosen...Click OK to Continue");
				   //}
				   //else
				   //{
					  // return; // MessageBox.Show("No was chosen...Click OK to Continue");
				   //} */
       //     }
       // }


  //      private static bool procdefine_chk(bool restrictive, bool inclusive, bool preauthorization, bool xhmo,bool foundit,string xdesc)
		//{
		//	string xpattype = (xhmo) ? "HMO Plan Type" : "Billing Category";
		//   // bool hmoprice_found = false;
		//	if (foundit && inclusive)
		//	{
		//		//do nothing
		//	  //  hmoprice_found = true;
		//	}
		//	else if (restrictive) //whether found or not
		//	{
		//		DialogResult result = MessageBox.Show(xdesc.Trim() + " is not on approved list for this Patient's " + xpattype +
		//			"... - RESTRICTIVE !!! '\n' PLEASE SELECT ALTERNATIVE DRUG...", "Drugs Approved List",msgBoxHandler);
		//			return false;
		//	}
		//	else
		//	{
		//		msmrfunc.mrGlobals.msgeventtracker = "SD";
		//		DialogResult result = MessageBox.Show(xdesc.Trim() + " is not on approved list for this Patient's " + xpattype + "... Continue ?", "Procedure Approved List",
  // MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, msgBoxHandler);
		//	   // if (msmrfunc.mrGlobals.msgeventtracker == "TERMINATE")
		//		if (result == DialogResult.No )
		//			return false;
		//	}

		//	if (preauthorization)
		//	{
		//		msmrfunc.mrGlobals.msgeventtracker = "PA";
		//		DialogResult result = MessageBox.Show("Selected Service Requires Pre-Authorization...CONTINUE ? ", "PRE-AUTHORIZATION ALERT!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, msgBoxHandler);
		//	  //  if (msmrfunc.mrGlobals.msgeventtracker == "TERMINATE")
		//		if (result == DialogResult.No )
		//			return false;
		//	}
		//	return true;
		//}

        public static string GETGroupheadname(string ghgroupcode, string grouphead, string grouphtype)
        {
            string xgrphead = "";
            DataTable grphead;
            if (grouphtype == "P")
                grphead = Dataaccess.GetAnytable("", "MR", "select name from patient where groupcode = '" + ghgroupcode + "' and patientno = '" + grouphead + "'", false);
            else
                grphead = Dataaccess.GetAnytable("", "MR", "select name, hmo from customer where custno = '" + grouphead + "'", false);
            if (grphead.Rows.Count > 0)
            {
                xgrphead = grphead.Rows[0]["name"].ToString();
                //24.08.2017 had to insert next line to solve reporting problem in OPDAttendandStatistics
                if (grouphtype == "C")
                    msmrfunc.mrGlobals.anycode = (bool)grphead.Rows[0]["hmo"] ? "YES" : "NO";
            }
            return xgrphead;
        }

        public static decimal getOpeningBalance(string ghgroupcode, string grouphead, string currencyid, string customertype, DateTime datefrom, DateTime dateto, ref decimal totdebit, ref decimal totcredit, ref decimal totadjust)
        {
            decimal acctbal;
            totcredit = totdebit = acctbal = totadjust = 0m;
            int xperiod, xyear;
            xperiod = msmrfunc.mrGlobals.mlastperiod >= 12 ? 1 : msmrfunc.mrGlobals.mlastperiod + 1;
            xyear = xperiod == 1 && msmrfunc.mrGlobals.mlastperiod != 0 ? msmrfunc.mrGlobals.mpyear + 1 : msmrfunc.mrGlobals.mpyear;
            DateTime start_opening = bissclass.ConvertStringToDateTime("01", xperiod.ToString(), xyear.ToString());
            if (datefrom < start_opening)
            {
                xperiod = datefrom.Month;
                xyear = datefrom.Year;
                start_opening = datefrom;
            }

            DataTable dtcust;
            string xfile;
            if (customertype == "C")
                xfile = string.IsNullOrWhiteSpace(currencyid) ? "customer" : "fccustom";
            else
                xfile = string.IsNullOrWhiteSpace(currencyid) ? "patient" : "fcpatient";
            string balbf = "balbf" + xperiod.ToString();
            string xselstr = customertype == "C" ? " custno = '" + grouphead + "'" : " GHGROUPCODE = '" + ghgroupcode + "' and grouphead = '" + grouphead + "'";

            if (string.IsNullOrWhiteSpace(currencyid))
                dtcust = Dataaccess.GetAnytable("", "MR", "select posted, balbf, " + balbf + " from " + xfile + "  where " + xselstr, false);
            else
                dtcust = Dataaccess.GetAnytable("", "MR", "select posted, balbf, " + balbf + " from " + xfile + "  where " + xselstr + "' and currency = '" + currencyid + "'", false);
            if (dtcust.Rows.Count < 1) //not seen
                return 0m;
            //string period = xperiod.ToString();
            //get actual op bal
            acctbal = (bool)dtcust.Rows[0]["posted"] ? (decimal)dtcust.Rows[0][balbf] : (decimal)dtcust.Rows[0]["balbf"];
            if (acctbal >= 1) //debit balance
                totdebit = acctbal;
            else
                totcredit = Math.Abs(acctbal);
            //get values from billing, payment and bill_adj
            // if ((bool)dtcust.Rows[0]["posted"])
            acctbal = getTransactionDbCrAdjSummary(ghgroupcode, grouphead, currencyid, customertype, start_opening, datefrom.AddDays(-1), ref totdebit, ref totcredit, ref totadjust);
            // else
            return acctbal;
        }

        public static decimal getTransactionDbCrAdjSummary(string ghgroupcode, string grouphead, string currencyid, string customertype, DateTime datefrom, DateTime dateto, ref decimal totdb, ref decimal totcr, ref decimal totadj)
        {
            string xfile;
            if (customertype == "C")
                xfile = string.IsNullOrWhiteSpace(currencyid) ? "customer" : "fccustom";
            else
                xfile = string.IsNullOrWhiteSpace(currencyid) ? "patient" : "fcpatient";
            string xselstr = customertype == "C" ? " grouphead = '" + grouphead + "'" : " GHGROUPCODE = '" + ghgroupcode + "' and grouphead = '" + grouphead + "'";

            //get values from billing, payment and bill_adj
            DataTable dt;
            if (string.IsNullOrWhiteSpace(currencyid))
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS debit from billing where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto.ToShortDateString() + " 23:59:59.999' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);
            else
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS debit from billing where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto.ToShortDateString() + " 23:59:59.999' and currency = '" + currencyid + "' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);

            totdb += dt.Rows.Count > 0 ? (decimal)dt.Rows[0]["DEBIT"] : 0m;
            //payments
            if (string.IsNullOrWhiteSpace(currencyid))
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS credit from paydetail where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto.ToShortDateString() + " 23:59:59.999' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);
            else
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS credit from paydetail where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto.ToShortDateString() + " 23:59:59.999' and currency = '" + currencyid + "' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);

            totcr += dt.Rows.Count > 0 ? (decimal)dt.Rows[0]["credit"] : 0m;
            //adjustments
            xselstr = customertype == "C" ? " grouphead = '" + grouphead + "'" : " ghgroupcode = '" + ghgroupcode + "' and grouphead = '" + grouphead + "'";
            if (string.IsNullOrWhiteSpace(currencyid))
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS debit from bill_adj where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto.ToShortDateString() + " 23:59:59.999' and ttype = 'D' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);
            else
                dt = Dataaccess.GetAnytable("", "MR", "select groupcode, grouphead, SUM(amount) AS debit from bill_adj where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto.ToShortDateString() + " 23:59:59.999' and ttype = 'D' and currency = '" + currencyid + "' and " + xselstr + " GROUP BY GROUPCODE, GROUPHEAD", false);

            totdb += dt.Rows.Count > 0 ? (decimal)dt.Rows[0]["debit"] : 0m;
            //credit leg
            if (string.IsNullOrWhiteSpace(currencyid))
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS credit from bill_adj where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto.ToShortDateString() + " 23:59:59.999' and ttype = 'C' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);
            else
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS credit from bill_adj where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto.ToShortDateString() + " 23:59:59.999' and ttype = 'C' and currency = '" + currencyid + "' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);

            totcr += dt.Rows.Count > 0 ? (decimal)dt.Rows[0]["credit"] : 0m;

            return totdb - totcr;
        }

        public static string GetGroupInvno(string ghgroupcode, string grouphead, DateTime transdate)
        {
            decimal xref = 0;
            string rtnval = "";
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select reference from horizbref where ghgroupcode = '" + ghgroupcode + "' and customer = '" + grouphead + "' and trans_date = '" + transdate.ToShortDateString() + "'", false);
            if (dt.Rows.Count < 1)
            {
                dt = Dataaccess.GetAnytable("", "MR", "select enqno from mrcontrol where recid = '6'", false);
                xref = Convert.ToDecimal(dt.Rows[0]["enqno"]) + 1;
                string updatestring = "update mrcontrol set enqno = '" + xref + "' where recid = '6'";
                bissclass.UpdateRecords(updatestring, "MR");
                rtnval = bissclass.autonumconfig(xref.ToString(), true, "", "999999999");
                SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = "horizbref_Add";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@ghgroupcode", ghgroupcode);
                insertCommand.Parameters.AddWithValue("@customer", grouphead);
                insertCommand.Parameters.AddWithValue("@trans_date", transdate);
                insertCommand.Parameters.AddWithValue("@reference", rtnval);
                insertCommand.Parameters.AddWithValue("@amount", 0m);

                connection.Open();
                insertCommand.ExecuteNonQuery();

            }
            else
                rtnval = dt.Rows[0]["reference"].ToString();
            return rtnval;
        }













        //private static void sortRecords(ref DataTable mthlydt, DataTable transdt, int mth, string rectype)
        //{
        //    bool foundit = false;
        //    DataRow drow = null;
        //    foreach (DataRow row in transdt.Rows)
        //    {
        //        foundit = false;
        //        foreach (DataRow row2 in mthlydt.Rows)
        //        {
        //            if (rectype == "CLIENTS" ? row2["reference"].ToString().Trim() == row["ghgroupcode"].ToString().Trim() + row["grouphead"].ToString().Trim() : rectype == "PROCESS" ? row2["name"].ToString().Trim() == row["description"].ToString().Trim() : rectype == "ATTENDANCE" ? row2["name"].ToString().Trim() == row["name"].ToString().Trim() : row2["reference"].ToString().Trim() == row["facility"].ToString().Trim()) //&& row2["name"].ToString().Trim() == row["description"].ToString().Trim())
        //            {
        //                drow = row2;
        //                foundit = true;
        //                break;
        //            }
        //        }
        //        if (!foundit)
        //        {
        //            drow = mthlydt.NewRow();
        //            drow["reference"] = rectype == "CLIENTS" ? row["ghgroupcode"].ToString().Trim() + row["grouphead"].ToString().Trim() : rectype == "PROCESS" ? row["description"].ToString().Trim() : rectype == "ATTENDANCE" ? row["name"].ToString().Trim() : row["facility"].ToString();
        //            if (rectype == "PROCESS")
        //                drow["name"] = row["description"].ToString();
        //            else if (rectype == "ATTENDANCE")
        //                drow["name"] = row["name"].ToString();
        //            else if (rectype == "FACILITY")
        //            {
        //                if (!string.IsNullOrWhiteSpace(row["facility"].ToString()))
        //                    drow["name"] = bissclass.seeksay("select name from servicecentrecodes where type_code = '" + row["facility"].ToString() + "'", "CODES", "name");
        //            }
        //            else
        //                drow["name"] = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), row["grouphtype"].ToString());
        //            drow["amt" + mth.ToString()] = 0m;
        //            drow["total"] = 0m;
        //            mthlydt.Rows.Add(drow);
        //        }
        //        decimal xamt;
        //        if (rectype == "ATTENDANCE")
        //            xamt = Convert.ToDecimal(row["amount"]);
        //        else
        //            xamt = (decimal)row["amount"];
        //        //drow["amt" + mth.ToString()] = (decimal)row["amount"];
        //        //drow["total"] = (decimal)drow["total"] + (decimal)row["amount"];
        //        drow["amt" + mth.ToString()] = xamt;
        //        drow["total"] = (decimal)drow["total"] + xamt;
        //    }
        //}
        //public static decimal gettardiffcalc(string xprocess, decimal xpercent, decimal xamount, string xpatcateg) //, 
        //                                                                                                           //DateTime xdate, string xtime, Array hola_, string xdiag)
        //{
        //    // DateTime tdate;
        //    // bool foundit = false;
        //    decimal xamt = 0, xp = 0;
        //    //xamount := xpercent := 0
        //    //sdow="M" - Monday to Friday "W" -weekend
        //    SqlConnection cs2 = Dataaccess.mrConnection();
        //    string selcommand2 = "SELECT amount,pmark FROM MRB15 WHERE rtrim(REFERENCE) = '" + xprocess.Trim() + "' AND patcat = '" + xpatcateg + "'";
        //    SqlCommand selectCommand2 = new SqlCommand(selcommand2, cs2);

        //    cs2.Open();
        //    SqlDataReader reader = selectCommand2.ExecuteReader();

        //    while (reader.Read())
        //    {

        //        xamt = (decimal)reader["amount"];
        //        xp = (decimal)reader["pmark"];
        //        break;
        //    }

        //    reader.Close();

        //    cs2.Close();

        //    if (xp != 0 && xamt != 0)
        //    { xamount += (xp * xamount) / 100; }
        //    else
        //    { xamount = xamt; }

        //    return xamount;
        //}
        //private static string gethmonhisacct(DataTable glupdate, string xacct, string xval, int xrow, string groupcode, string billservictype, string billdesc, string paytype)
        //{
        //    // **bilvouc.N-nhis H-hmo xservicetype = (chkhmocapitation.Checked) ? "H" : (chknhiscapitation.Checked) ? "N" : (chkretainership.Checked) ? "R" : "";
        //    if (xval == "B")
        //    {
        //        if (groupcode == "NHIS" || billservictype == "N")
        //        {
        //            if (billservictype == "N")
        //                return (xacct == "D") ? glupdate.Rows[xrow]["nhiscap_bdebit"].ToString() : glupdate.Rows[xrow]["nhiscap_bcredit"].ToString();
        //            else
        //                return (xacct == "D") ? glupdate.Rows[xrow]["nhisclm_bdebit"].ToString() : glupdate.Rows[xrow]["nhisclm_bcredit"].ToString();
        //        }
        //        else
        //        {
        //            if (billservictype == "H")
        //                return xacct == "D" ? glupdate.Rows[xrow]["hmocap_bdebit"].ToString() : glupdate.Rows[xrow]["hmocap_bcredit"].ToString();
        //            else
        //                return xacct == "D" ? glupdate.Rows[xrow]["hmoclm_bdebit"].ToString() : glupdate.Rows[xrow]["hmoclm_bcredit"].ToString();
        //        }
        //    }
        //    else if (xval == "P" || xval == "CASH" || xval == "CHEQUE") //payment segment
        //    {
        //        if (billdesc.Contains("NHIS") || billservictype == "N") //nhis pmt
        //        {
        //            if (paytype == "CASH")
        //                return (billservictype == "N" && xacct == "D") ? glupdate.Rows[xrow]["nhiscap_cashdebit"].ToString() :
        //                   (billservictype == "N" && xacct == "C") ? glupdate.Rows[xrow]["nhiscap_cashcredit"].ToString() :
        //                   (xacct == "D") ? glupdate.Rows[xrow]["hmoclm_cashdebit"].ToString() : glupdate.Rows[xrow]["nhisclm_cashcredit"].ToString();
        //            else
        //                return (billservictype == "N" && xacct == "D") ? glupdate.Rows[xrow]["nhiscap_chqdebit"].ToString() :
        //                   (billservictype == "N" && xacct == "C") ? glupdate.Rows[xrow]["nhiscap_chqcredit"].ToString() :
        //                   (xacct == "D") ? glupdate.Rows[xrow]["nhisclm_chqdebit"].ToString() : glupdate.Rows[xrow]["nhisclm_chqcredit"].ToString();
        //        }
        //        else //hmo pmt
        //        {
        //            if (paytype == "CASH")
        //                return (billservictype == "H" && xacct == "D") ? glupdate.Rows[xrow]["hmocap_cashdebit"].ToString() :
        //                   (billservictype == "H" && xacct == "C") ? glupdate.Rows[xrow]["hmocap_cashcredit"].ToString() :
        //                   (xacct == "D") ? glupdate.Rows[xrow]["hmoclm_cashdebit"].ToString() : glupdate.Rows[xrow]["hmoclm_cashcredit"].ToString();
        //            else
        //                return (billservictype == "H" && xacct == "D") ? glupdate.Rows[xrow]["hmocap_chqdebit"].ToString() :
        //                   (billservictype == "H" && xacct == "C") ? glupdate.Rows[xrow]["hmocap_chqcredit"].ToString() :
        //                   (xacct == "D") ? glupdate.Rows[xrow]["hmoclm_chqdebit"].ToString() : glupdate.Rows[xrow]["hmoclm_chqcredit"].ToString();
        //        }
        //    }
        //    else //if (xval == "A")
        //    {

        //        if (billdesc.Contains("NHIS"))
        //            return xacct == "D" ? glupdate.Rows[xrow]["nhisclmdebit"].ToString() : glupdate.Rows[xrow]["nhisclmcredit"].ToString();
        //        else
        //            return xacct == "D" ? glupdate.Rows[xrow]["HMOclmdebit"].ToString() : glupdate.Rows[xrow]["HMOclmcredit"].ToString();
        //    }

        //}

        //    public static void ResizeArray<T>(ref T[,] original, int newCoNum, int newRoNum)
        //    {
        //        var newArray = new T[newCoNum, newRoNum];
        //        int columnCount = original.GetLength(1);
        //        int columnCount2 = newRoNum;
        //        int columns = original.GetUpperBound(0);
        //        for (int co = 0; co <= columns; co++)
        //            Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
        //        original = newArray;
        //    }
        //    public static DataTable getmrsetup(string facility)
        //    {
        //        string selectStatement;
        //        SqlConnection cs = Dataaccess.mrConnection();
        //        selectStatement = string.IsNullOrWhiteSpace(facility) ? "SELECT * FROM mrsetup" :
        //            "SELECT * FROM mrsetup WHERE FACILITY = '" + facility + "'";

        //        SqlCommand selectCommand = new SqlCommand(selectStatement, cs);
        //        SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
        //        DataTable dt = new DataTable();


        //        ds.Fill(dt);
        //        cs.Close();
        //        return dt;
        //    }

        //    public static Decimal getcontrol_lastnumber(string xfield, int xrow, bool xupdate, decimal currentVal, bool toreset)
        //    {
        //        DataTable dt = Dataaccess.GetAnytable("mrcontrol", "MR", "SELECT " + xfield + " FROM MRcontrol WHERE RECID = " + xrow, false);
        //        Decimal xval = (Decimal)dt.Rows[0][xfield];
        //        xval++;
        //        if (xupdate) //we add 1 to whatever that is retrieved; currentval is previous retrieval+1
        //        {
        //            string updatestring = "update mrcontrol set " + xfield + " = '" + xval + "' where recid = '" + xrow + "'";
        //            bissclass.UpdateRecords(updatestring, "MR");
        //        }
        //        return xval;
        //    }
        //    public static decimal getFeefromtariff(string xprocess, string xpatcateg, ref string xdesc, ref string facility)
        //    {
        //        decimal xamt = 0, xamt1 = 0;
        //        bool diffcharge = false;
        //        SqlConnection cs2 = Dataaccess.mrConnection();
        //        string selcommand2 = "SELECT statmt_des,amount,diffcharge,category FROM TARIFF WHERE rtrim(REFERENCE) = '" + xprocess.Trim() + "'";
        //        SqlCommand selectCommand2 = new SqlCommand(selcommand2, cs2);
        //        try
        //        {
        //            cs2.Open();
        //            SqlDataReader reader = selectCommand2.ExecuteReader();

        //            while (reader.Read())
        //            {

        //                xamt = (decimal)reader["amount"];
        //                diffcharge = (bool)reader["diffcharge"];
        //                xdesc = reader["statmt_des"].ToString();
        //                facility = reader["category"].ToString();

        //                break;
        //            }

        //            reader.Close();
        //        }
        //        catch (SqlException ex)
        //        {
        //            MessageBox.Show("" + ex);
        //        }
        //        finally
        //        {
        //            cs2.Close();
        //        }
        //        if (diffcharge)
        //        {

        //            xamt1 = msmrfunc.gettardiffcalc(xprocess, 0, xamt, xpatcateg); //, 
        //                                                                           //mrattend.TRANS_DATE, DateTime.Now.TimeOfDay, @hola_, " ");
        //        }
        //        return xamt1 == 0 ? xamt : xamt1;
        //    }
        //    public static DialogResult InputBox(string title, string promptText, ref string value)
        //    {
        //        Form form = new Form();
        //        Label label = new Label();
        //        TextBox textBox = new TextBox();
        //        Button buttonOk = new Button();
        //        Button buttonCancel = new Button();

        //        form.Text = title;
        //        label.Text = promptText;
        //        textBox.Text = ""; // value;

        //        buttonOk.Text = "OK";
        //        buttonCancel.Text = "Cancel";
        //        buttonOk.DialogResult = DialogResult.OK;
        //        buttonCancel.DialogResult = DialogResult.Cancel;

        //        label.SetBounds(9, 20, 372, 13);
        //        textBox.SetBounds(12, 36, 372, 20);
        //        buttonOk.SetBounds(228, 72, 75, 23);
        //        buttonCancel.SetBounds(309, 72, 75, 23);

        //        label.AutoSize = true;
        //        textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
        //        buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        //        buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

        //        form.ClientSize = new System.Drawing.Size(396, 107); // Size(396, 107);
        //        form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
        //        form.ClientSize = new System.Drawing.Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
        //        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        //        form.StartPosition = FormStartPosition.CenterScreen;
        //        form.MinimizeBox = false;
        //        form.MaximizeBox = false;
        //        form.AcceptButton = buttonOk;
        //        form.CancelButton = buttonCancel;
        //        //frmselcode FrmSelCode = new frmselcode();
        //        // FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);

        //        DialogResult dialogResult = form.ShowDialog();
        //        value = textBox.Text;
        //        return dialogResult;
        //        /*usage:
        //*string value = "Document 1";
        //if (Tmp.InputBox("New document", "New document name:", ref value) == DialogResult.OK)
        //{
        //  myDocument.Name = value;
        //}*/
        //    }

        //    public static bool setupinfo(string xfacility, ref string rptextension, ref string xreqrpt,
        //             ref string xbillcode, ref string xemailphone, ref string xinvrpt, ref bool xinvbatch, ref string xhdjustify, ref bool autorptheader)
        //    {
        //        bool foundit = false;
        //        // DataTable mrdt = msmrfunc.getmrsetup(xfacility); //MRSETUP
        //        string selstring = string.IsNullOrWhiteSpace(xfacility) ? "select * from mrsetup" : "select rptext, reqrpt, billcode, reports, utilities, obackupend, leftjustifyheader, billcode, autorptheader from mrsetup where rtrim(facility) = '" + xfacility.Trim() + "'";
        //        DataTable mrdt = Dataaccess.GetAnytable("", "MR", selstring, false);

        //        if (mrdt.Rows.Count > 0 && xfacility != "")
        //        {
        //            foreach (DataRow row in mrdt.Rows)
        //            {
        //                foundit = true;
        //                rptextension = row["rptext"].ToString();
        //                xreqrpt = row["reqrpt"].ToString();
        //                xbillcode = row["billcode"].ToString();
        //                xemailphone = row["reports"].ToString().Trim();
        //                xinvrpt = row["utilities"].ToString().Trim();
        //                xinvbatch = Convert.ToBoolean(row["obackupend"]);
        //                xhdjustify = row["leftjustifyheader"].ToString();
        //                //  facilitydesc = row["facilityname"].ToString();
        //                //  grpbilldesc = row["billcodedesc"].ToString();
        //                autorptheader = Convert.ToBoolean(row["autorptheader"]);
        //                /*   if (string.IsNullOrWhiteSpace(grpbilldesc) && !string.IsNullOrWhiteSpace(xbillcode))
        //                       grpbilldesc = bissclass.seeksay("select name from tariff where rtrim(reference) = '" + xbillcode.Trim() + "'", "MR", "NAME");*/
        //                // if (xfacility != "")
        //                break;
        //            }
        //        }
        //        if (!foundit)
        //        {
        //            DialogResult result = MessageBox.Show("Unable to Load Setup Info for This Service Centre...Pls Contact Your Systems Administrator !!!", "SYSTEMS SETUP ERROR ...", msgBoxHandler);
        //            return false;
        //        }
        //        return true;
        //    }
        //    public static DataTable loaduserprofile(string xsys, bool globalaccess)
        //    {
        //        SqlConnection cs;
        //        string selectStatement;
        //        if (globalaccess)
        //        {
        //            cs = Dataaccess.codeConnection();
        //            selectStatement = "SELECT * FROM GLOBALSYS";
        //        }
        //        else
        //        {
        //            cs = (xsys == "MRSTLEV") ? Dataaccess.mrConnection() : (xsys == "SCSTLEV") ? Dataaccess.stkConnection() : (xsys == "APSTLEV") ?
        //                Dataaccess.apConnection() : (xsys == "ARSTLEV") ? Dataaccess.arConnection() : (xsys == "FASTLEV") ? Dataaccess.faConnection() :
        //                (xsys == "GL01") ? Dataaccess.glConnection() : Dataaccess.hrConnection();
        //            selectStatement = "SELECT * FROM " + xsys;
        //        }
        //        SqlCommand selectCommand = new SqlCommand(selectStatement, cs);
        //        SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
        //        DataTable dt = new DataTable();


        //        ds.Fill(dt);
        //        cs.Close();
        //        return dt;
        //    }
        //    public static void getdebitcredit_acct(string glupdatetype, DataTable glupdate, ref string debitacct, ref string creditacct, string grouphtype, bool corporatGHishmo, string BillsorPay_BP, string facility, string groupcode, string billservictype, string billdesc, string paytype)
        //    {
        //        // if (glupdatetype == "3") 29.09.2017 - there are no multiple interface option anymore.
        //        // {
        //        // DataTable glupdate = GLINTAB3.GetGLINTAB3(); //it will cost too much time to do this for every record.
        //        //I move to calling program - 31-12-2013
        //        string xfacility = "", adjust = "";
        //        for (int i = 0; i < glupdate.Rows.Count; i++)
        //        {
        //            if (BillsorPay_BP == "B") //bills
        //            {
        //                xfacility = glupdate.Rows[i]["facility"].ToString().Trim();
        //                if (string.IsNullOrWhiteSpace(facility) && xfacility == "OTHER" || facility.Trim() == xfacility)
        //                {

        //                    if (grouphtype == "C" && corporatGHishmo)
        //                    {
        //                        debitacct = gethmonhisacct(glupdate, "D", "B", i, groupcode, billservictype, billdesc, paytype);
        //                        creditacct = gethmonhisacct(glupdate, "C", "B", i, groupcode, billservictype, billdesc, paytype);
        //                    }
        //                    else
        //                    {
        //                        if (grouphtype == "C")
        //                        {
        //                            debitacct = glupdate.Rows[i]["copbdebit"].ToString();
        //                            creditacct = glupdate.Rows[i]["copbcredit"].ToString();
        //                        }
        //                        else
        //                        {
        //                            debitacct = glupdate.Rows[i]["pvtbdebit"].ToString();
        //                            creditacct = glupdate.Rows[i]["pvtbcredit"].ToString();
        //                        }
        //                    }
        //                    break; //if found
        //                }
        //            }
        //            else if (BillsorPay_BP == "P")//PAYMENTS
        //            {
        //                if (grouphtype == "C") //corporate
        //                {
        //                    if (paytype.Contains("CASH"))
        //                    {
        //                        debitacct = (corporatGHishmo) ? gethmonhisacct(glupdate, "D", "CASH", i, groupcode, billservictype, billdesc, paytype) : glupdate.Rows[i]["copcashdebit"].ToString();
        //                        creditacct = (corporatGHishmo) ? gethmonhisacct(glupdate, "C", "CASH", i, groupcode, billservictype, billdesc, paytype) : glupdate.Rows[i]["copcashcredit"].ToString();
        //                    }
        //                    else //cheque/BANK/CREDIT CARD
        //                    {
        //                        debitacct = (corporatGHishmo) ? gethmonhisacct(glupdate, "D", "CHEQUE", i, groupcode, billservictype, billdesc, paytype) : glupdate.Rows[i]["copchqdebit"].ToString();
        //                        creditacct = (corporatGHishmo) ? gethmonhisacct(glupdate, "C", "CHEQUE", i, groupcode, billservictype, billdesc, paytype) : glupdate.Rows[i]["copchqcredit"].ToString();

        //                    }
        //                }
        //                else //PVT/facility
        //                {
        //                    if (paytype != "CASH") //cheque/BANK/CREDIT CARD
        //                    {
        //                        debitacct = glupdate.Rows[i]["pvtchqdebit"].ToString();
        //                        creditacct = glupdate.Rows[i]["pvtchqcredit"].ToString();
        //                    }
        //                    else
        //                    {
        //                        debitacct = glupdate.Rows[i]["pvtcashdebit"].ToString();
        //                        creditacct = glupdate.Rows[i]["pvtcashcredit"].ToString();
        //                    }
        //                }

        //            }
        //            else if (BillsorPay_BP == "A")//ADJUSTMENT
        //            {
        //                adjust = glupdate.Rows[i]["adjust"].ToString();
        //                if (string.IsNullOrWhiteSpace(facility) && adjust == "OTHER" || facility == adjust)
        //                {
        //                    if (grouphtype == "C" && corporatGHishmo)
        //                    {
        //                        debitacct = gethmonhisacct(glupdate, "D", "A", i, groupcode, billservictype, billdesc, paytype);
        //                        creditacct = gethmonhisacct(glupdate, "C", "A", i, groupcode, billservictype, billdesc, paytype);
        //                    }
        //                    else
        //                    {
        //                        if (grouphtype == "C")
        //                        {
        //                            debitacct = glupdate.Rows[i]["CODEBIT"].ToString();
        //                            creditacct = glupdate.Rows[i]["COCREDIT"].ToString();
        //                        }
        //                        else
        //                        {
        //                            debitacct = glupdate.Rows[i]["FCPVTDEBIT"].ToString();
        //                            creditacct = glupdate.Rows[i]["FCPVTCREDIT"].ToString();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        // }
        //    }
        //    public static decimal hmonhistariffcheck(string calltype, string grouphead, string plantype, string patientno,
        //        string procedure, ref bool preauthorization, ref bool iscapitated, ref bool tocontinue, string proc_desc)
        //    {
        //        //  PleaseWaitForm pleaseWait = new PleaseWaitForm();
        //        //check for hmo and special discount
        //        bool restrictive, inclusive, foundit = false; //, fee_for_service = false;
        //        decimal rtnamount = 0m;
        //        //bool hmofound = false;
        //        Hmodetail hmodetail = new Hmodetail();
        //        hmodetail = Hmodetail.GetHMODETAIL(grouphead, plantype);
        //        if (hmodetail == null)
        //        {
        //            return 0m;
        //        }
        //        else
        //        {
        //            inclusive = hmodetail.PROCINCLUSIVE;
        //            restrictive = hmodetail.PROCRESTRICTIVE;

        //            if (hmodetail.CAPAMT == 0m) //all services are fee for service no capitation but the hmo could have its own tariff so we check
        //            {
        //                // fee_for_service = true;
        //            }
        //            HMOSERVPROC hmoserv = new HMOSERVPROC();
        //            hmoserv = HMOSERVPROC.GetHMOSERVPROC(grouphead, plantype, procedure);
        //            if (hmoserv != null)
        //            {
        //                //  bissclass.sysGlobals.waitwindowtext = "FOUND HMO PROCEDURE PRICE LIST...";
        //                //  pleaseWait.Show();
        //                //txtamount.Text = hmoserv.AMOUNT.ToString();
        //                rtnamount = hmoserv.AMOUNT;
        //                iscapitated = (hmoserv.CAPITATED) ? true : false;
        //                preauthorization = hmoserv.AUTHORIZATIONREQUIRED;
        //                //amtsave = Convert.ToDecimal(txtamount.Text);
        //                foundit = true;
        //            }
        //            tocontinue = procdefine_chk(restrictive, inclusive, preauthorization, true, foundit, proc_desc);
        //        }
        //        return rtnamount;
        //    }
        //    public static decimal othercorpClientTariffCheck(string patientno, string patcateg, ref bool preauthorization,
        //   ref bool tocontinue, string proc_desc, string procedure)
        //    {
        //        bool restrictive, inclusive, foundit = false;
        //        decimal rtnamount = 0m;
        //        DataTable CustClass = custclass.GetCUSTCLASS();
        //        if (CustClass.Rows.Count < 1)
        //        {
        //            return 0m;
        //        }
        //        foreach (DataRow row in CustClass.Rows)
        //        {
        //            if (row["reference"].ToString() == patcateg && Convert.ToBoolean(row["DEFINEPROC"]) == true)
        //            {
        //                //check drgdefine on current item
        //                inclusive = Convert.ToBoolean(row["PROCINCLUSIVE"]);
        //                restrictive = Convert.ToBoolean(row["PROCRESTRICTIVE"]);
        //                //preauthorization = false;

        //                PROCPROFILE procprofile = PROCPROFILE.GetPROCPROFILE(patcateg, procedure);
        //                if (procprofile != null)
        //                {
        //                    foundit = true;
        //                    rtnamount = procprofile.AMOUNT;
        //                    //iscapitated = (procprofile.CAPITATED) ? true : false;
        //                    preauthorization = procprofile.AUTHORIZATIONREQUIRED;
        //                    //amtsave = Convert.ToDecimal(txtamount.Text);
        //                }
        //                tocontinue = procdefine_chk(restrictive, inclusive, preauthorization, false, foundit, proc_desc);
        //                break;
        //            }
        //        }
        //        return rtnamount;
        //    }
        //    public static decimal stockitemValidate(string stkcode, ref decimal qtyavailable, ref bool tocontinue, ref bool preauthorization, ref bool iscapitated, string stkdesc, ref string txtdose, ref string unitid, ref decimal cost, ref decimal strength, ref decimal stkper, ref decimal packqty, int autoremind_period, ref decimal purcost, string xstore, ref bool xwithConsumables)
        //    {
        //        if (string.IsNullOrWhiteSpace(stkcode))
        //        {
        //            DialogResult result = MessageBox.Show("Valid Stock Definition must be selected...", "Stock Master Record");
        //            tocontinue = false;
        //            return 0m;
        //        }
        //        string xstoresel = "";
        //        //   PleaseWaitForm pleaseWait = new PleaseWaitForm();
        //        if (!string.IsNullOrWhiteSpace(xstore))
        //            xstoresel = " and store = '" + xstore + "'";
        //        DataTable stock = Dataaccess.GetAnytable("", "SMS", "select stock_qty, unit, sell, cost, strength, per, packqty, status, expirydate, withprescription from stock where item = '" + stkcode + "'" + xstoresel, false);
        //        if (stock.Rows.Count < 1)
        //        {
        //            DialogResult result = MessageBox.Show("Undefined Stock Item....", "Stock Definitions");
        //            tocontinue = false;
        //            return 0m;
        //        }
        //        bool xreturn = false;
        //        DateTime xpirydate = msmrfunc.mrGlobals.dtmin_date;
        //        txtdose = "";
        //        qtyavailable = 0m;
        //        foreach (DataRow row in stock.Rows)
        //        {
        //            qtyavailable += Convert.ToDecimal(row["stock_qty"]);
        //            unitid = row["unit"].ToString().Trim();
        //            cost = Convert.ToDecimal(row["sell"]);
        //            strength = Convert.ToDecimal(row["strength"]);
        //            stkper = Convert.ToDecimal(row["per"]);
        //            packqty = Convert.ToDecimal(row["packqty"]);
        //            purcost = Convert.ToDecimal(row["cost"]);
        //            if (Convert.ToBoolean(row["withprescription"]))
        //                xwithConsumables = (bool)row["withprescription"];

        //            if (row["status"].ToString() == "D")
        //            {
        //                DialogResult result = MessageBox.Show("This Item is no longer in use... has been flagged domant !", "Stock Master File");
        //                xreturn = true;
        //                break;
        //            }
        //            //CHECK FOR EXPIRY DATE
        //            xpirydate = Convert.ToDateTime(row["expirydate"]);
        //        }
        //        if (xpirydate.Year > msmrfunc.mrGlobals.mta_start.Year)
        //            bissclass.expdtremind(xpirydate, autoremind_period, msmrfunc.mrGlobals.mta_start);
        //        if (xreturn)
        //        {
        //            tocontinue = false;
        //            return 0m;
        //        }
        //        return cost;
        //    }
        //    public static decimal CheckCustClassforStkDefined(string patcateg, ref bool preauthorization, ref bool iscapitated,
        //        ref decimal cost, ref bool tocontinue, string stkcode, string stkdesc)
        //    {
        //        decimal rtnamount = 0m;
        //        bool restrictive, inclusive;
        //        bool foundit = false;
        //        DataTable CustClass = custclass.GetCUSTCLASS();
        //        if (!string.IsNullOrWhiteSpace(patcateg)) //check custclass for drgs define flag
        //        {
        //            // foreach (DataTable row in CustClass.Rows)
        //            for (int i = 0; i < CustClass.Rows.Count; i++)
        //            {
        //                if (CustClass.Rows[i]["reference"].ToString() == patcateg && Convert.ToBoolean(CustClass.Rows[i]["definedrgs"]) == true)
        //                {
        //                    //check drgdefine on current item
        //                    inclusive = Convert.ToBoolean(CustClass.Rows[i]["drginclusive"]);
        //                    restrictive = Convert.ToBoolean(CustClass.Rows[i]["drgrestrictive"]);
        //                    preauthorization = false;

        //                    dgprofile Drgprofile = dgprofile.GetDGPROFILE(patcateg, stkcode);
        //                    if (Drgprofile != null)
        //                    {
        //                        foundit = true;
        //                        cost = rtnamount = Drgprofile.AMOUNT;
        //                        iscapitated = (Drgprofile.CAPITATED) ? true : false;
        //                        preauthorization = Drgprofile.AUTHORIZATIONREQUIRED;
        //                    }
        //                    tocontinue = procdefine_chk(restrictive, inclusive, preauthorization, false, foundit, stkdesc);
        //                    break;
        //                }
        //            }
        //            //return rtnamount;
        //        }
        //        return rtnamount;
        //    }
        //    public static decimal CheckCorpPatientStkDefined(string grouphead, string grouphtype, string plantype, string groupcode, bool nhisgentariff, bool inpatient, ref decimal cost, bool fee_for_service, string stkcode, ref bool preauthorization, ref bool iscapitated, ref bool tocontinue, string stkdesc)
        //    {
        //        //  PleaseWaitForm pleaseWait = new PleaseWaitForm();
        //        bool foundit = false;
        //        decimal rtnamount = cost, savedNHISamt = 0m; ;
        //        bool restrictive = false, inclusive = false;
        //        if (grouphtype == "C" && !string.IsNullOrWhiteSpace(plantype)) //check hmo drg definitions
        //        {
        //            if (groupcode == "NHIS" && nhisgentariff && !inpatient) //outpatient
        //            {
        //                //31-05-2013 : It defaults to General Tariff for Out-Patients - Giwa Hospital
        //                foundit = true;
        //                //  hmostkpricefound = true;
        //                rtnamount = savedNHISamt = cost; // .Value = savedstksellamount;
        //            }
        //            //check hmodetail if item is define and amout to bill
        //            Hmodetail hmodetail = new Hmodetail();
        //            hmodetail = Hmodetail.GetHMODETAIL(grouphead, plantype);
        //            if (hmodetail != null)
        //            {
        //                inclusive = hmodetail.DRGINCLUSIVE;
        //                restrictive = hmodetail.DRGRESTRICTIVE;
        //                if (hmodetail.CAPAMT == 0m) //all services are fee for service no capitation but the hmo could have its own tariff so we check
        //                {
        //                    fee_for_service = true;
        //                }
        //            }
        //            HMOSERVIC hmoservic = new HMOSERVIC();
        //            hmoservic = HMOSERVIC.GetHMOSERVIC(grouphead, plantype, stkcode);
        //            if (hmoservic != null)
        //            {
        //                // hmostkpricefound = true;
        //                //  bissclass.sysGlobals.waitwindowtext = "FOUND HMO DRUG PRICE LIST...";
        //                //  pleaseWait.Show();
        //                foundit = true;
        //                rtnamount = hmoservic.AMOUNT;
        //                iscapitated = (hmoservic.CAPITATED) ? true : false;
        //                preauthorization = hmoservic.AUTHORIZATIONREQUIRED;

        //            }
        //            tocontinue = procdefine_chk(restrictive, inclusive, preauthorization, true, foundit, stkdesc);
        //            if (groupcode == "NHIS" && nhisgentariff && !inpatient && savedNHISamt > 0)
        //                rtnamount = savedNHISamt;
        //        }
        //        return rtnamount;
        //    }
        //    public static decimal CheckStkCharge(string patcateg, ref decimal cost, ref bool tocontinue, string stkcode)
        //    {
        //        //WE MUST NOW CHECK STKCHARGE FOR DIFFERENTIAL CHARGE - 10/12/2001, 22-11-2013
        //        //  if ( !hmostkpricefound )
        //        //  {
        //        DataTable CustClass = custclass.GetCUSTCLASS();
        //        decimal rtnamount = cost;
        //        STKCHARG stkcharge = new STKCHARG();
        //        stkcharge = STKCHARG.GetSTKCHARG(stkcode, patcateg);
        //        if (stkcharge != null)
        //            rtnamount = stkcharge.AMOUNT;
        //        else
        //        {
        //            for (int i = 0; i < CustClass.Rows.Count; i++)
        //            {
        //                if (CustClass.Rows[i]["reference"].ToString() == patcateg && Convert.ToDecimal(CustClass.Rows[i]["percentage"]) != 0m)
        //                {
        //                    decimal xpercentage = Convert.ToDecimal(CustClass.Rows[i]["percentage"]);
        //                    rtnamount = ((cost * xpercentage) / 100) + cost;
        //                }
        //            }
        //        }
        //        return rtnamount;
        //    }
        //    public static decimal applyDefineddiscountValue(decimal xamount)
        //    {
        //        //check for discount percentage in patient or customer and apply
        //        // PleaseWaitForm pleaseWait = new PleaseWaitForm();
        //        if (msmrfunc.mrGlobals.percentageDiscountToApply != 0m)
        //        {
        //            MessageBox.Show("Discounted :" + msmrfunc.mrGlobals.percentageDiscountToApply.ToString() + "% on " + xamount.ToString(), "SPECIAL DISCOUNT");
        //            // pleaseWait.Show();
        //            decimal xdisc = (xamount * msmrfunc.mrGlobals.percentageDiscountToApply) / 100;
        //            xamount = xamount - xdisc;
        //            //amtsave = Convert.ToDecimal(txtamount.Text);
        //        }
        //        return xamount;
        //    }
        //    public static bool updateglobaldate(DateTime trans_date)
        //    {
        //        SqlConnection connection = new SqlConnection(); connection = Dataaccess.codeConnection();
        //        SqlCommand updateCommand = new SqlCommand();
        //        updateCommand.Connection = connection;
        //        try
        //        {
        //            connection.Open();
        //            updateCommand.CommandText = "Update ctrolxl SET last_date = '" + trans_date + "' WHERE RECID = 1";
        //            updateCommand.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            MessageBox.Show(" " + ex, "ERP Systems Date Error", MessageBoxButtons.OK, MessageBoxIcon.Information,
        //                MessageBoxDefaultButton.Button1);
        //            return false;
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }
        //        return true;
        //    }
        //    public static void processComparative(ref DataTable MonthlyFigTable, ref DataTable aggregateTable, DateTime datefrom, DateTime dateto, string recordtype, string customertype)
        //    {
        //        MonthlyFigTable = new DataTable(); //table to will be passed to report dataset 
        //        MonthlyFigTable.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
        //        MonthlyFigTable.Columns.Add(new DataColumn("NAME", typeof(string)));
        //        for (int i = 1; i < 13; i++) //creates amt1 - amt13 : 1-12 mthly fig.
        //        {
        //            MonthlyFigTable.Columns.Add(new DataColumn("AMT" + i.ToString(), typeof(decimal)));
        //        }
        //        MonthlyFigTable.Columns.Add(new DataColumn("TOTAL", typeof(decimal)));
        //        MonthlyFigTable.Columns.Add(new DataColumn("AVERAGE", typeof(decimal)));
        //        aggregateTable = new DataTable(); // to hold aggregate calculations
        //        for (int i = 1; i < 13; i++)
        //        {
        //            aggregateTable.Columns.Add(new DataColumn("AMT" + i.ToString(), typeof(decimal)));
        //        }

        //        string recselect = customertype == "C" ? "transtype = 'C' " : customertype == "P" ? "transtype = 'P' and ghgroupcode = 'PVT'" : "transtype = 'P' and ghgroupcode = 'FC'"; // like [CP ] ";
        //        string selstring = "";
        //        if (recordtype == "CLIENTS")
        //            selstring = "select Sum(amount) as amount, ghgroupcode, grouphead, grouphtype from billing where " + recselect;
        //        else if (recordtype == "PROCESS")
        //            selstring = "select Sum(amount) as amount, description from billing ";
        //        else if (recordtype == "ATTENDANCE")
        //            selstring = "select Count(name) as amount from mrattend where " + recselect;
        //        else
        //            selstring = "select Sum(amount) as amount, facility from billing ";
        //        string xsel = "";
        //        DataTable dt; //, tmpdt;
        //        if (recordtype == "ATTENDANCE") //pvt,fc,special service,corporate,hmo,nhis,staff,
        //        {
        //            for (int i = 0; i < 7; i++)
        //            {
        //                switch (i)
        //                {
        //                    case 0:
        //                        selstring = "select Count(name) as amount, char(50) as name from mrattend where groupcode = 'PVT'";
        //                        break;
        //                    case 1:
        //                        selstring = "select Count(name) as amount, char(50) as name from mrattend where groupcode = 'FC'";
        //                        break;
        //                    case 2:
        //                        selstring = "select Count(name) as amount, char(50) as name from mrattend where left(reference,1) = 'S'";
        //                        break;
        //                    case 3:
        //                        selstring = "select Count(mrattend.name) as amount, char(50) as name from mrattend LEFT JOIN CUSTOMER on mrattend.grouphead = customer.custno where mrattend.grouphtype = 'C' and customer.hmo = '0'";
        //                        break;
        //                    case 4:
        //                        selstring = "select Count(mrattend.name) as amount, char(50) as name from mrattend LEFT JOIN CUSTOMER on mrattend.grouphead = customer.custno where mrattend.grouphtype = 'C' and customer.hmo = '1' and mrattend.groupcode != 'NHIS'";
        //                        break;
        //                    case 5:
        //                        selstring = "select Count(mrattend.name) as amount, char(50) as name from mrattend WHERE mrattend.groupcode = 'NHIS'";
        //                        break;
        //                    case 6: //customertype is Hospital Code on Customer File
        //                        selstring = "select Count(mrattend.name) as amount, char(50) as name from mrattend where GROUPHEAD = '" + customertype + "'";
        //                        break;
        //                }
        //                for (int ix = 1; ix < 13; ix++)
        //                {
        //                    xsel = selstring + " and MONTH(mrattend.trans_date) = '" + ix.ToString() + "' and year(mrattend.trans_date) = '" + datefrom.Year.ToString() + "'";
        //                    dt = Dataaccess.GetAnytable("", "MR", xsel, false);
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        dt.Rows[0]["name"] = i == 1 ? "PRIVATE PATIENTS" : i == 2 ? "FAMILY PATIENTS" : i == 3 ? "SPECIAL SERVICE PATIENTS" : i == 4 ? "CORPORATE PATIENTS" : i == 5 ? "HMO PATIENTS" : i == 6 ? "NHIS PATIENTS" : "STAFF/OTHERS";
        //                        sortRecords(ref MonthlyFigTable, dt, ix, recordtype);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 1; i < 13; i++)
        //            {
        //                if (recordtype == "CLIENTS")
        //                    xsel = selstring + " and MONTH(trans_date) = '" + i.ToString() + "' and year(trans_date) = '" + datefrom.Year.ToString() + "' group by grouphead, ghgroupcode, grouphtype";
        //                else if (recordtype == "PROCESS")
        //                    xsel = selstring + " where MONTH(trans_date) = '" + i.ToString() + "' and year(trans_date) = '" + datefrom.Year.ToString() + "' group by description";
        //                else  //BY FACILITY 
        //                    xsel = selstring + " where MONTH(trans_date) = '" + i.ToString() + "' and year(trans_date) = '" + datefrom.Year.ToString() + "' group by facility";

        //                dt = Dataaccess.GetAnytable("", "MR", xsel, false);
        //                //   if (i > 1)
        //                sortRecords(ref MonthlyFigTable, dt, i, recordtype);
        //                // else
        //                //     MonthlyFigTable = dt;
        //            }
        //        }
        //        //calculate aggregate
        //        DataRow arow;
        //        if (aggregateTable.Rows.Count < 1)
        //        {
        //            arow = aggregateTable.NewRow();
        //            aggregateTable.Rows.Add(arow);
        //        }
        //        else
        //            aggregateTable.Clear();
        //        for (int i = 0; i < aggregateTable.Rows.Count; i++)
        //        {
        //            arow = aggregateTable.Rows[i];
        //            for (int ix = 1; ix < 13; ix++)
        //            {
        //                arow["amt" + ix.ToString()] = 0m;
        //            }
        //        }
        //        arow = aggregateTable.Rows[0];
        //        decimal gtotal = 0m, d1, d2;
        //        //string xd1,xd2;
        //        //string xname = "";
        //        foreach (DataRow row in MonthlyFigTable.Rows)
        //        {
        //            //  xname = row["name"].ToString();
        //            for (int i = 1; i < 13; i++) //add up all monthly figures to 12
        //            {
        //                // xd1 = arow["amt" + i.ToString()].ToString();
        //                // xd2 = row["amt" + i.ToString()].ToString();
        //                d1 = (decimal)arow["amt" + i.ToString()];
        //                d2 = string.IsNullOrWhiteSpace(row["amt" + i.ToString()].ToString()) ? 0m : (decimal)row["amt" + i.ToString()];
        //                arow["amt" + i.ToString()] = (decimal)arow["amt" + i.ToString()] + d2;
        //                //   arow["amt" + i.ToString()] = (decimal)arow["amt" + i.ToString()] + (decimal)row["amt" + i.ToString()];
        //            }
        //            gtotal += (decimal)row["total"];
        //        }
        //        //find aggregate
        //        for (int i = 1; i < 13; i++) //add up all monthly figures to 12
        //        {
        //            //(ma_[xcount]/gamt)*100
        //            arow["amt" + i.ToString()] = ((decimal)arow["amt" + i.ToString()] / gtotal) * 100;
        //        }
        //        //	replace amt14 WITH (amt13/gamt)*100
        //        //average
        //        foreach (DataRow row in MonthlyFigTable.Rows)
        //        {
        //            row["average"] = ((decimal)row["total"] / gtotal) * 100;
        //        }
        //    }
        //    private static DataTable GenerateRecords(string xrecordtype, string sel, string xselstring, ref DataTable xMonthlyFigTable, DateTime xdatefrom)
        //    {
        //        DataTable xdt = new DataTable();
        //        for (int i = 1; i < 13; i++)
        //        {
        //            if (xrecordtype == "CLIENTS")
        //                sel = xselstring + " and MONTH(trans_date) = '" + i.ToString() + "' and year(trans_date) = '" + xdatefrom.Year.ToString() + "' group by grouphead, ghgroupcode, grouphtype";
        //            else if (xrecordtype == "PROCESS")
        //                sel = xselstring + " where MONTH(trans_date) = '" + i.ToString() + "' and year(trans_date) = '" + xdatefrom.Year.ToString() + "' group by description";
        //            else  //BY FACILITY 
        //                sel = xselstring + " where MONTH(trans_date) = '" + i.ToString() + "' and year(trans_date) = '" + xdatefrom.Year.ToString() + "' group by facility";

        //            xdt = Dataaccess.GetAnytable("", "MR", sel, false);
        //            //   if (i > 1)
        //            sortRecords(ref xMonthlyFigTable, xdt, i, xrecordtype);
        //            // else
        //            //     MonthlyFigTable = dt;
        //        }
        //        return xdt;
        //    }
        //    public static decimal getSpecialistConsultCharge(billchaindtl billchain, string clinic)
        //    {
        //        Medhrec medhrec = Medhrec.GetMEDHREC(billchain.GROUPCODE, billchain.PATIENTNO);
        //        int xlastday = 0;
        //        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
        //        if (medhrec != null)
        //        {
        //            if (medhrec.DATE4 > dtmin_date.Date && medhrec.CLINIC4.Trim() != "" && medhrec.CLINIC4.Trim() == clinic) //Last visit b4 this one.
        //                xlastday = DateTime.Now.Date.Subtract(medhrec.DATE4.AddDays(1)).Days;
        //            else if (medhrec.DATE3 > dtmin_date.Date && medhrec.CLINIC3 != "" && medhrec.CLINIC4.Trim() == clinic)
        //                xlastday = DateTime.Now.Date.Subtract(medhrec.DATE3.AddDays(1)).Days;
        //            else if (medhrec.DATE2 > dtmin_date.Date && medhrec.CLINIC2 != "" && medhrec.CLINIC2.Trim() == clinic)
        //                xlastday = DateTime.Now.Date.Subtract(medhrec.DATE2.AddDays(1)).Days;
        //            else if (medhrec.DATE1 > dtmin_date.Date && medhrec.CLINIC1 != "" && medhrec.CLINIC1.Trim() == clinic)
        //                xlastday = DateTime.Now.Date.Subtract(medhrec.DATE1.AddDays(1)).Days;

        //        }
        //        //get specialist consultation details from spcprofile
        //        decimal xamt = 0m;
        //        bool xdiffcharge = false;
        //        SqlConnection cs = Dataaccess.mrConnection();
        //        string selcommand = "SELECT facility,followupdays,followupamt,amount,diffcharge FROM SPCPROFILE WHERE FACILITY = '" + clinic + "'";
        //        SqlCommand selectCommand = new SqlCommand(selcommand, cs);
        //        try
        //        {
        //            cs.Open();
        //            SqlDataReader reader = selectCommand.ExecuteReader();

        //            while (reader.Read())
        //            {

        //                xamt = (xlastday > 0 && xlastday < (Int32)reader["followupdays"]) ? (decimal)reader["followupamt"] : (decimal)reader["amount"];
        //                xdiffcharge = (bool)reader["diffcharge"];
        //                break;
        //            }

        //            reader.Close();
        //        }
        //        catch (SqlException ex)
        //        {
        //            MessageBox.Show("" + ex);
        //        }
        //        finally
        //        {
        //            cs.Close();
        //        }
        //        if (billchain.GROUPHTYPE == "P" || !xdiffcharge)
        //            return xamt;
        //        //corporate search

        //        string selcommand2 = "SELECT facility,followupdays,followupamt,amount,customer FROM SPCDETAIL WHERE FACILITY = '" + clinic + "' and customer = '" + billchain.GROUPHEAD + "'";
        //        DataTable dt = Dataaccess.GetAnytable("", "MR", selcommand2, false);
        //        if (dt.Rows.Count < 1)
        //            return xamt;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            xamt = (xlastday > 0 && xlastday < (Int32)row["followupdays"]) ? (decimal)row["followupamt"] : (decimal)row["amount"];
        //            break;
        //        }
        //        return xamt;
        //    }
        //    public static void updateOverwrite(string reference, string xnotes, billchaindtl bchain, decimal creditlimit, decimal balance)
        //    {
        //        SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
        //        SqlCommand insertCommand = new SqlCommand();
        //        insertCommand.CommandText = "ovprofile_Add";
        //        insertCommand.Connection = connection;
        //        insertCommand.CommandType = CommandType.StoredProcedure;

        //        insertCommand.Parameters.AddWithValue("@REFERENCE", reference);
        //        insertCommand.Parameters.AddWithValue("@GROUPCODE", bchain.GROUPCODE);
        //        insertCommand.Parameters.AddWithValue("@PATIENTNO", bchain.PATIENTNO);
        //        insertCommand.Parameters.AddWithValue("@NAME", bchain.NAME);
        //        insertCommand.Parameters.AddWithValue("@GROUPHEAD", bchain.GROUPHEAD);
        //        insertCommand.Parameters.AddWithValue("@GROUPHTYPE", bchain.GROUPHTYPE);
        //        insertCommand.Parameters.AddWithValue("@POSTED", false);
        //        insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
        //        insertCommand.Parameters.AddWithValue("@GHGROUPCODE", bchain.GHGROUPCODE);
        //        insertCommand.Parameters.AddWithValue("@OPERATOR", bissclass.sysGlobals.anycode);
        //        insertCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);
        //        insertCommand.Parameters.AddWithValue("@CRLIMIT", creditlimit);
        //        insertCommand.Parameters.AddWithValue("@BALANCE", balance);
        //        insertCommand.Parameters.AddWithValue("@overwritenote", xnotes);

        //        connection.Open();
        //        insertCommand.ExecuteNonQuery();

        //        connection.Close();

        //    }
        //    public static bool checklinkOK(string section, string reference)
        //    {
        //        DataTable dt = Dataaccess.GetAnytable("", "MR", "select linkok from link where reference = '" + reference + "' and tosection = '" + section + "'", false);
        //        if (dt.Rows.Count < 1)
        //            return false;
        //        return (bool)dt.Rows[0]["linkok"];

        //    }
        //    public static DataTable getLinkDetails(string reference, int recno, decimal cumbil, decimal cumpay, string facility, bool linkok, string msection, int procedure, string admflag, string doctor)
        //    {
        //        msmrfunc.mrGlobals.anycode = "";
        //        string global_clinic_code = msmrfunc.mrGlobals.global_clinic_code.Trim();
        //        int nwseclevel = msmrfunc.mrGlobals.nwseclevel;
        //        bool isphlebotomy = msmrfunc.mrGlobals.isphlebotomy;
        //        facility = facility.Trim();
        //        string selcommand = "SELECT NAME, GROUPCODE, PATIENTNO, TIMESENT, REFERENCE, OPERATOR, FACILITY, PROCFUNC, SENDEXCL, DOCTOR, LINKOK, tosection,recid,cumbil,transflag FROM LINK WHERE DATEREC = '' AND TRANS_DATE = '" + DateTime.Now.Date + "' AND TOSECTION LIKE '[" + msection + "]' AND REFERENCE != '' ORDER BY TIMESENT";
        //        //EMPTY TIMEREC AND
        //        //removed 17.01.2018 from dtlink loop - string.IsNullOrWhiteSpace(global_clinic_code) &&
        //        //       !string.IsNullOrWhiteSpace(xfacility) && !global_clinic_check(xfacility) ||
        //        DataRow row = null;
        //        string xfacility, xname, xdoctor, xtosection; int procfunc; bool xsendexcl, xlinkok;
        //        DataTable dtlink = Dataaccess.GetAnytable("", "MR", selcommand, false);
        //        DataTable dtrtn = new DataTable();
        //        if (dtlink.Rows.Count > 0) //create return table
        //        {
        //            //dtrtn = new DataTable();
        //            dtrtn = dtlink.Clone();

        //        }
        //        for (int i = 0; i < dtlink.Rows.Count; i++)
        //        {
        //            row = dtlink.Rows[i];
        //            xname = row["Name"].ToString();
        //            xfacility = row["facility"].ToString().Trim();
        //            xdoctor = row["doctor"].ToString().Trim();
        //            procfunc = Convert.ToInt32(row["procfunc"]);
        //            xsendexcl = Convert.ToBoolean(row["sendexcl"]);
        //            xtosection = row["tosection"].ToString().Trim();
        //            xlinkok = Convert.ToBoolean(row["linkok"]);
        //            if (xtosection != "2" && !string.IsNullOrWhiteSpace(facility) && xfacility != facility ||
        //                !string.IsNullOrWhiteSpace(global_clinic_code) && xfacility != global_clinic_code || xtosection != "2" && procedure > 0 && procfunc != procedure || string.IsNullOrWhiteSpace(xname) || xsendexcl && !string.IsNullOrWhiteSpace(doctor) && xdoctor != doctor ||
        //                msection == "6" && xtosection == "6" && nwseclevel == 1 && xlinkok ||
        //                msection == "6" && xtosection == "6" && nwseclevel != 1 && isphlebotomy && !xlinkok || !string.IsNullOrWhiteSpace(admflag) && admflag != row["transflag"].ToString().Trim()) //sample not taken
        //            {
        //                // dtlink.Rows.Remove(row);
        //                continue;
        //            }
        //            //dtTableNew.ImportRow(drtableOld);
        //            dtrtn.ImportRow(row);
        //        }
        //        // if (dtlink.Rows.Count < 1)
        //        if (dtrtn.Rows.Count < 1)
        //        {
        //            string xlocation =
        //                (msection == "3") ? "Nurses Station/Desk" : (msection == "1") ? "Registration Desk" :
        //                (msection == "2") ? "Cashier/Payment Desk" : (msection == "4") ? "Consultation(Doctors)" :
        //                (msection == "5") ? "Ward/Process Desk" : (msection == "6") ? "Lab/Xray/Scan/Theatre" :
        //                (msection == "7") ? "Billing Office" : (msection == "8") ? "Pharmacy" :
        //                (msection == "9") ? "Paediatrics" : (msection == "A") ? "Admissions" : "";
        //            DialogResult result = MessageBox.Show("No Patient Awaiting Service . . . ", xlocation);
        //        }
        //        //return dtlink;
        //        return dtrtn;
        //    }
        //    public static void SendSMS(string profile, DataTable phonenumbers)
        //    {
        //        DialogResult result;
        //        DataTable dt = Dataaccess.GetAnytable("", "CODES", "select name,user_name,mpass,state from ctrolxl where recid = '7' ", false);
        //        DataRow row = dt.Rows[0];
        //        if (string.IsNullOrWhiteSpace(row["name"].ToString()) || string.IsNullOrWhiteSpace(row["mpass"].ToString()))
        //        {
        //            result = MessageBox.Show("SMS Router not properly configured...");
        //            return;
        //        }
        //        string bulksmsAcctName, senderpasswd, Defaultsender;
        //        bulksmsAcctName = row["name"].ToString().Trim();
        //        senderpasswd = Dataaccess.DecryptString(row["mpass"].ToString());
        //        Defaultsender = row["user_name"].ToString();
        //        //  string to, msg;

        //        /*     msg = string.Format("Payment Notification: \n" +
        //                 "Hospital # : {0}  : {1} [ {2} ] \n" +
        //                "Desc: {3} \n" +
        //                "Amount Paid : {4} \n" +
        //                "Date : {5} \n" +
        //                "Thank you.", groupcode.Trim(), patientno.Trim(), txtName.Text.Trim(), paydesc, nmrAmount.Value.ToString("N2"), datefrom.ToLongDateString()).Replace("\n", Environment.NewLine);
        //             //msg = "TEST RECEIPT MSG";
        //             txtName.Text = "Sending....Pls Wait!";
        //             PostRequest("http://login.betasms.com/customer/api/", to, msg, smssender, senderpasswd, sender);


        // */
        //    }
        //    public static DataRow TimerExecute(string xsection, string xoperator, int filecheck)
        //    {
        //        bool foundit = false;
        //        DataRow returnRow = null;
        //        DataTable adt;
        //        if (filecheck == 1)
        //        {
        //            adt = Dataaccess.GetAnytable("", "MR", "SELECT * from mrb21 where posted = '0' and tosection = '" + xsection + "' and CONVERT(DATE,trans_date) = '" + DateTime.Now.Date + "'", false);
        //            if (adt.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in adt.Rows)
        //                {
        //                    // IntemedsAlerts lt = new IntemedsAlerts(adt, "ALERT", "MRB21", msmrfunc.mrGlobals.msection, false);
        //                    //  lt.ShowDialog();
        //                    returnRow = row;
        //                    foundit = true;
        //                    break;
        //                }
        //                //GROUPCODE, PATIENTNO, NAME, SENDER, TRANS_DATE, FACILITY, NOTES, RECEIVED, OPERATOR, REFERENCE, SENDSECTION, DOCTOR, TOSECTION
        //                //sms
        //            }
        //            if (foundit)
        //                return returnRow;
        //        }
        //        else
        //        {
        //            adt = Dataaccess.GetAnytable("", "MR", "SELECT * from mrb21a where posted = '0' and rtrim(name) = '" + xoperator.Trim() + "'", false);
        //            if (adt.Rows.Count > 0)
        //            {

        //                foreach (DataRow row in adt.Rows)
        //                {
        //                    if ((decimal)row["replaydays"] > 0 && Convert.ToDateTime(row["received"]).AddDays(Convert.ToDouble(row["replaydays"])) < DateTime.Now.Date || (decimal)row["replayat"] > 0 && Convert.ToInt32(row["replayat"]) != DateTime.Now.TimeOfDay.Hours)
        //                        continue;
        //                    returnRow = row;
        //                    foundit = true;
        //                    break;
        //                }
        //            }
        //        }
        //        return returnRow;
        //    }






        //public static bool checkGroupCode(string customerID)
        //{
        //	bool rtnval = true;
        //	if (customerID.Trim() == "PVT" || customerID.Trim() == "FC" || string.IsNullOrWhiteSpace(customerID))
        //	{
        //		return true;
        //	}
        //	else
        //	{
        //		Customer customer = new Customer();
        //		SqlConnection connection = Dataaccess.mrConnection();
        //		string selectStatement =
        //		"SELECT Custno, Name, Address1 " +
        //		"FROM Customer " +
        //		"WHERE Custno = @customerID";
        //		SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
        //		selectCommand.Parameters.AddWithValue("@customerID", customerID);
        //		try
        //		{
        //			connection.Open();
        //			SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
        //			if (reader.Read())
        //			{
        //				customer.Custno = reader["custno"].ToString();
        //				customer.Name = reader["Name"].ToString();
        //				// customer.Address1 = reader["Address1"].ToString();

        //			}
        //			else
        //			{
        //				MessageBox.Show("Undefined Patient Groupcode in Corporate Clients' File...", "Patient Group Validation",
        //					MessageBoxButtons.OK, MessageBoxIcon.Information);
        //				rtnval = false;
        //			}
        //			reader.Close();
        //		}
        //		catch (SqlException ex)
        //		{
        //			//throw ex;
        //			MessageBox.Show(ex.Message, ex.GetType().ToString(), true);
        //			// MessageBox.Show("Unable to Connect to Database File...", "Patient Group Validation");
        //			rtnval = false;
        //		}
        //		finally
        //		{
        //			connection.Close();
        //		}
        //	}
        //	return rtnval;
        //}

















        /// <summary>
        /// Contains global variables for project.
        /// </summary>

        /*    public static void ResizeArray<T>(
				ref T[,] array, int padLeft, int padRight, int padTop, int padBottom)
			{
				int ow = array.GetLength(0);
				int oh = array.GetLength(1);
				int nw = ow + padLeft + padRight;
				int nh = oh + padTop + padBottom;

				int x0 = padLeft;
				int y0 = padTop;
				int x1 = x0 + ow - 1;
				int y1 = y0 + oh - 1;
				int u0 = -x0;
				int v0 = -y0;

				if (x0 < 0) x0 = 0;
				if (y0 < 0) y0 = 0;
				if (x1 >= nw) x1 = nw - 1;
				if (y1 >= nh) y1 = nh - 1;

				T[,] nArr = new T[nw, nh];
				for (int y = y0; y <= y1; y++)
				{
					for (int x = x0; x <= x1; x++)
					{
						nArr[x, y] = array[u0 + x, v0 + y];
					}
				}
				array = nArr;
			}
			 */

        //public static DataTable getcontrolsetup(string xsys)
        //{
        //    string selectStatement="";
        //    SqlConnection cs = new SqlConnection();
        //    if ( xsys == "MR" )
        //    {
        //        cs = Dataaccess.mrConnection();
        //        selectStatement = "SELECT * FROM mrcontrol";
        //    }
        //    else if (xsys == "SMS" )
        //    {
        //        cs = Dataaccess.stkConnection();
        //        selectStatement = "SELECT * FROM control";
        //    }
        //    else if (xsys == "FA")
        //    {
        //    }
        //    else if (xsys == "CTROL")
        //    {
        //        cs = Dataaccess.codeConnection();
        //        selectStatement = "SELECT * FROM ctrolxl";
        //    }
        //    SqlCommand selectCommand = new SqlCommand(selectStatement, cs);
        //    SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
        //    DataTable dt = new DataTable();

        //    ds.Fill(dt);
        //    cs.Close();
        //    return dt;
        //}

        /// <summary>
        /// If string facility is null, returns all records in mrsetup
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>

        //public static Decimal getcontrol_lastnumber(string xfield, int recid, bool xupdate, decimal currentVal,bool toreset)
        //{

        //    SqlConnection cs = Dataaccess.mrConnection();
        //    DataTable dt = Dataaccess.GetAnytable("mrcontrol", "MR", "select "+ xfield+" from mrcontrol where recid = '"+recid +"'", false);

        //    Decimal xval = (Decimal)dt.Rows[0][xfield]; //"Last_no"];

        //    if (xupdate) //we add 1 to whatever that is retrieved; currentval is previous retrieval+1
        //    {
        //       // xrow++;
        //        if (toreset)
        //            xval = currentVal;
        //        else
        //            xval++;

        //        //  currentVal = xval;
        //        SqlCommand updateStatement = new SqlCommand();
        //        updateStatement.CommandText = (xfield == "LAST_NO") ? "Mrcontrol_LastNo" :
        //            (xfield == "CHARGNO") ? "Mrcontrol_ChargNo" : (xfield == "PAYNO") ? "Mrcontrol_Payno" :
        //            (xfield == "ALTERNATENO") ? "Mrcontrol_Alternate" : (xfield == "ADMIT") ? "Mrcontrol_AdmissionNo" :
        //            (xfield == "ADJNO") ? "Mrcontrol_Adjno" : "Mrcontrol_Attno";
        //        updateStatement.Connection = cs;
        //        updateStatement.CommandType = CommandType.StoredProcedure;

        //        updateStatement.Parameters.AddWithValue("@" + xfield, xval);  //currentVal);
        //        updateStatement.Parameters.AddWithValue("@RECID", recid );
        //        cs.Open();
        //        updateStatement.ExecuteNonQuery();

        //    }

        //            cs.Close();
        //           return xval;
        //      }


        //public static DateTime updatecontrol_DateValue(string xdfield, int xrow, bool xupdate, DateTime dcurrentVal)
        //{

        //    SqlConnection cs = Dataaccess.mrConnection();
        //    /*       string selectStatement = "SELECT " + xdfield + " FROM mrcontrol";
        //           SqlCommand selectCommand = new SqlCommand(selectStatement, cs);
        //           SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
        //           DataTable dt = new DataTable();

        //           ds.Fill(dt);
        //           DateTime xrtnval = (DateTime)dt.Rows[xrow][xdfield]; 
        //           if (xupdate && xdfield == "ATTDATE" && xrtnval.ToShortDateString() !=  DateTime.Now.ToShortDateString() ) 
        //           { */
        //    xrow++;
        //    // xrtnval = DateTime.Now;
        //    //  currentVal = xval;
        //    SqlCommand updateStatement = new SqlCommand();
        //    updateStatement.CommandText = (xdfield == "ATTDATE") ? "Mrcontrol_AttDate" : "Mrcontrol_ChargNo";
        //    updateStatement.Connection = cs;
        //    updateStatement.CommandType = CommandType.StoredProcedure;

        //    updateStatement.Parameters.AddWithValue("@" + xdfield, dcurrentVal);  //currentVal);
        //    updateStatement.Parameters.AddWithValue("@RECID", xrow);
        //    cs.Open();
        //    updateStatement.ExecuteNonQuery();

        //    cs.Close();
        //    return dcurrentVal;
        //}


        //public static void getAutoCompleteDetails(string xfile,string xfield1, string xfield2) //, 
        //{
        /*          MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(patient_name,phone_no) as NamewithNo FROM patient", myConnection);
                  DataSet ds = new DataSet(); 
                  MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                  da.Fill(ds, "My List"); 
                  AutoCompleteStringCollection col = new AutoCompleteStringCollection();
                  int i = 0; for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                      { col.Add(ds.Tables[0].Rows[i]["NamewithNo"].ToString()); 
                      // col.Add(ds.Tables[0].Rows[i]["phone_no"].ToString()); } 
                      txtname.AutoCompleteSource = AutoCompleteSource.CustomSource;
                      txtname.AutoCompleteCustomSource = col; 
                      txtname.AutoCompleteMode = AutoCompleteMode.Suggest;
                      myConnection.Close(); */

        //   }
        /// <summary>
        /// Returns string.  Uses designed form.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="promptText"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //public static void frmInputBox(string title, string promptText, ref string value)
        //{
        //    POPREAD popread = new POPREAD(title, promptText, ref value,false );
        //    popread.Closed += new EventHandler(popread_Closed);
        //    popread.ShowDialog();
        //}
        //static void popread_Closed(object sender, EventArgs e)
        //{
        //    POPREAD popread = sender as POPREAD;
        //    return;
        //}
        //     public static bool LoadInstall()
        //     {
        //         SqlConnection cs = Dataaccess.codeConnection();
        //         string selectStatement = "SELECT * FROM msinpset";

        //         SqlCommand selectCommand = new SqlCommand(selectStatement, cs);
        //         SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
        //         DataTable dt = new DataTable();
        //         try
        //         {
        //             ds.Fill(dt);
        //             cs.Close();
        //             msmrfunc.mrGlobals.ismedical = (bool)dt.Rows[0]["medical"];
        //             msmrfunc.mrGlobals.isstock = (bool)dt.Rows[0]["stock"];
        //             msmrfunc.mrGlobals.ispp = (bool)dt.Rows[0]["stock"];
        //             msmrfunc.mrGlobals.isgl = (bool)dt.Rows[0]["stock"];
        //             msmrfunc.mrGlobals.isap = (bool)dt.Rows[0]["stock"];
        //             msmrfunc.mrGlobals.isfa = (bool)dt.Rows[0]["stock"];
        //             msmrfunc.mrGlobals.isicis = (bool)dt.Rows[0]["stock"];
        //             msmrfunc.mrGlobals.ismeddiag = (bool)dt.Rows[0]["stock"];
        //             return true;
        //         }
        //        catch (Exception ex)
        //         {
        //             //throw ex;
        //             MessageBox.Show("" + ex, "INSTALLATION Details ", MessageBoxButtons.OK,
        //MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //             cs.Close();
        //             return false;
        //         }
        //         finally
        //         {
        //             cs.Close();
        //         }
        //        //return true;
        //     }

        /// <summary>
        /// get gl debit and credit accounts, passed by reference.  Routing also calls the gethomdebitcredit acct
        /// if called from billing verification pass billservictyp for HMO - transtype is D or C - debit/credit.
        /// Its implemented for MSMR GL interface by service facility option "3" for now as at 31-12-2013
        /// </summary>
        /// <param name="glupdatetype"></param>
        /// <param name="glupdate"></param>
        /// <param name="debitacct"></param>
        /// <param name="creditacct"></param>
        /// <param name="grouphtype"></param>
        /// <param name="corporatGHishmo"></param>
        /// <param name="BillsorPay_BP"></param>
        /// <param name="facility"></param>
        /// <param name="groupcode"></param>
        /// <param name="billservictype"></param>
        /// <param name="billdesc"></param>
        /// <param name="paytype"></param>

        /// <summary>
        /// gets hmo/nhis GL accounts for bills/pay verification
        /// </summary>
        /// <param name="glupdate"></param>
        /// <param name="xacct"></param>
        /// <param name="xval"></param>
        /// <param name="xrow"></param>
        /// <param name="groupcode"></param>
        /// <param name="billservictype"></param>
        /// <param name="billdesc"></param>
        /// <param name="paytype"></param>
        /// <returns></returns>

        /// <summary>
        /// Validate procedure on HMO/NHIS tariff table
        /// </summary>
        /// <param name="calltype"></param>
        /// <param name="grouphead"></param>
        /// <param name="plantype"></param>
        /// <param name="patientno"></param>
        /// <param name="procedure"></param>
        /// <param name="preauthorization"></param>
        /// <param name="iscapitated"></param>
        /// <param name="tocontinue"></param>
        /// <param name="proc_desc"></param>
        /// <returns></returns>

        /// <summary>
        /// Validates Stock item. Get qty available in ALL stores. Ideal for Docs consult & Admissions Service Update
        /// </summary>
        /// <param name="stkcode"></param>
        /// <param name="qtyavailable"></param>
        /// <param name="tocontinue"></param>
        /// <param name="preauthorization"></param>
        /// <param name="iscapitated"></param>
        /// <param name="stkdesc"></param>
        /// <param name="txtdose"></param>
        /// <param name="unitid"></param>
        /// <param name="cost"></param>
        /// <param name="strength"></param>
        /// <param name="stkper"></param>
        /// <param name="packqty"></param>
        /// <param name="autoremind_period"></param>
        /// <returns></returns>

        /// <summary>
        /// Checks Custclass table to determine if Patient is on defined tariff for stock items - Non-HMO/NHIS Patients
        /// </summary>
        /// <param name="plantype"></param>
        /// <param name="patcateg"></param>
        /// <param name="preauthorization"></param>
        /// <param name="iscapitated"></param>
        /// <param name="cost"></param>
        /// <param name="tocontinue"></param>
        /// <param name="stkcode"></param>
        /// <param name="stkdesc"></param>
        /// <returns></returns>

        /// <summary>
        /// Checks defined stock tariff for HMO/nhis patients
        /// </summary>
        /// <param name="grouphead"></param>
        /// <param name="grouphtype"></param>
        /// <param name="plantype"></param>
        /// <param name="groupcode"></param>
        /// <param name="nhisgentariff"></param>
        /// <param name="inpatient"></param>
        /// <param name="cost"></param>
        /// <param name="fee_for_service"></param>
        /// <param name="stkcode"></param>
        /// <param name="preauthorization"></param>
        /// <param name="iscapitated"></param>
        /// <param name="tocontinue"></param>
        /// <param name="stkdesc"></param>
        /// <returns></returns>

        /// <summary>
        /// check Stkcharge table for differential charge on billing category on Drugs
        /// </summary>
        /// <param name="patcateg"></param>
        /// <param name="cost"></param>
        /// <param name="tocontinue"></param>
        /// <param name="stkcode"></param>
        /// <returns></returns>

        /// <summary>
        /// Check special discount on customers/patients and apply discount, if applicable
        /// </summary>
        /// <param name="xamount"></param>
        /// <returns></returns>

        /*      public static object FindInDimensions(this object[,] target, object searchTerm)
               {
                   object result = null;
                   var rowLowerLimit = target.GetLowerBound(0);
                   var rowUpperLimit = target.GetUpperBound(0);

                   var colLowerLimit = target.GetLowerBound(1);
                   var colUpperLimit = target.GetUpperBound(1);

                   for (int row = rowLowerLimit; row < rowUpperLimit; row++)
                   {
                       for (int col = colLowerLimit; col < colUpperLimit; col++)
                       {
                           // you could do the search here...
                       }
                   }

                   return result;
               }*/

        /// <summary>
        /// returns all records in any table - tablename must be passed and module - sys must be passed where table is contained
        /// If para selectsatment is not blank, records are returned according to statement
        /// sys options are MR,SMS,FA,AP,GL,HR,SYSCODE
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="sys"></param>
        /// <returns></returns>

        //public static ImageResourceHandle getpicture(string pictureselected)
        //{
        //    string filename = System.IO.Path.GetFileName(pictureselected);
        //    ImageResourceHandle imageResourceHandlePic1 = new ImageResourceHandle(filename);
        //    return imageResourceHandlePic1;
        //}

        /// <summary>
        /// recordtype - CLIENTS or PROCESS, customertype C or P or blank for all
        /// </summary>
        /// <param name="MonthlyFigTable"></param>
        /// <param name="aggregateTable"></param>
        /// <param name="datefrom"></param>
        /// <param name="dateto"></param>
        /// <param name="recordtype"></param>
        /// <param name="customertype"></param>

        //    async void PostRequest(string url, string to, string msg, string user, string password, string sender)
        //    {

        /*   IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>(){
               new KeyValuePair<string, string>("username", user), // "info@adisystems-ng.com"),
               new KeyValuePair<string, string>("password", password), // "okota@165"),
               new KeyValuePair<string, string>("message", msg), // "Payment Notification: \n Hospital #: KUPA Hospital\nDesc: Malaria Drug\nAmount:#500"),
               new KeyValuePair<string, string>("sender", sender), // "ADISYS"),
               new KeyValuePair<string, string>("mobiles", to) // "07034834761"),
           };
           HttpContent content = new FormUrlEncodedContent(queries);
           using (HttpClient client = new HttpClient())
           {
               using (HttpResponseMessage response = await client.PostAsync(url, content))
               {
                   using (HttpContent outCont = response.Content)
                   {
                       mycontent = await outCont.ReadAsStringAsync();
                     //  displayresult(mycontent);
                   }
               }
           }*/
        //   }
    }
}


/*
 Warning	3	This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.	C:\VSPWEB\mr\mradmin\BissClass\msmrfunc.cs	1567	20	mradmin

 System.Drawing.Pen myPen;
myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
System.Drawing.Graphics formGraphics = this.CreateGraphics();
formGraphics.DrawLine(myPen, 0, 0, 200, 200);
myPen.Dispose();
formGraphics.Dispose();
 * 
 * 
 private void DrawVerticalText()
{
   System.Drawing.Graphics formGraphics = this.CreateGraphics();
   string drawString = "Sample Text";
   System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
   System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
   float x = 150.0f;
   float y = 50.0f;
   System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat(StringFormatFlags.DirectionVertical);
   formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
   drawFont.Dispose();
   drawBrush.Dispose();
   formGraphics.Dispose();
}
 * 
 * for (int i = 0; i < dt.Rows.Count; i++)
		   {
			   if (i == 1)
				   msmrfunc.mrGlobals.mlocstate = dt.Columns["locstate"].ToString();
				   msmrfunc.mrGlobals.mloccountry = dt.Columns["loccountry"].ToString();
				   msmrfunc.mrGlobals.mpauto = Convert.ToBoolean(dt.Columns["pauto"]);
				   mlocste = Row]
			   // your index is in i
			   var row = dt.Rows[i];
		   }
			 mlastnumb = dt.Rows
			   //this.txtread.Text = dt.Rows.Count.ToString();
			   DataRow[] currentRows = workTable.Select(
   null, null, DataViewRowState.CurrentRows);
			   MLOCSTATE = locSTATE
Mloccountry = loccountry
mpauto = iif(pauto == "A", .T., .F.)
mpflex = iif(pauto == "F", .T., .F.)
mcauto = cauto
mlocalcur = localcur
mecgauto = ecgauto
mxrauto = xrauto
mpayauto = payauto
madjauto = adjauto
mlastperiod = tp_period
mpyear = pyear
mstatperiod = statperiod
mattendlink = attendlink
mdiagonbill = diagonbill
mothercharg = othercharg
mfacilauto = facilauto
mgsort = gsort
mdischtime = dischtime
mta_start = ta_start
19/09/96
mcprefix = cprefix
msprefix = sprefix
mautoGreg = autoGreg
mautoGcons = autoGcons
mduraconsul = duraconsul
mregcode = regcode
mconscode = conscode
mconsbycli = consbycli
mbefromadm = befromadm &&//allow billing from admission update
//mdiffcharge := diffcharge //differential charge for private and corporate patients
mautoreceipt = autoreceip
mautophabill = autophabil
mautomedbill = automedbil
//05/01/98
mautopgroup = autopgroup
mlengnumb = lengnumb
mseclink = seclink
toreload = reload
goto 4
acc_code = pvtcode &&accommodation billing code for admissions
mattendmonitor = installed &&01/06/2009
GOTO 7
allow_autonomous_clinic = installed &&10/06/2011

			}
	   }

	   function linkinfo(scrloc,xreference,xfile,xrec,xcumbil,xcumpay,xfacility,xlinkok,xsection,xprocedure,xadmflag)
local oldselect,sel,tablesused
anycode = ' '
oldselect = SELECT()
xprocedure = iif(empty(xprocedure),0,xprocedure)
sel = 1
xsection = iif(empty(xsection), msection, xsection)
&&03.05.2011
IF !USED('tables')
   tablesused = .t.
   SELECT 0
   USE apath+'\tables.dat' SHARED 
   SET ORDER to tag codes
ENDIF
if empty(xfile)
   select 0
   *openfile( "link.dat",.f.)
   USE link.dat SHARED 
   set order to tag link03
ENDIF
SELECT LINK
dimension brows2d[1,9]
STORE .F. TO BROWS2D
*WAIT WINDOW global_clinic_code
&&02/05/2011 added global_clinic_code to implememtn restriction to staff clinic, if restrictive - AVIATION in Kupa
**I think we must setup glogal array to fold all clinics and their flags to be able to identify clinics that are restrictve
set near on
seek dtos(date()) &&+space(8)
set near off
do while trans_date==date() .and. !eof()
   *WAIT WINDOW IIF(sendexcl,'YES ','NO ')+Link.doctor+IIF(TYPE('mdoctor')='U',"modc - undefined",mdoctor)
  if !Empty(daterec) .OR. !tosection $ m.xsection .OR. ;
	   !Empty(m.xfacility) .AND. facility # m.xfacility .or. ;
	   !EMPTY(m.global_clinic_code) AND facility # m.global_clinic_code OR ;
	   !empty(m.xprocedure) .and. procfunc # m.xprocedure OR ;
	   EMPTY(name) OR sendexcl AND Link.doctor # m.mdoctor OR ;
	   EMPTY(m.global_clinic_code) AND !EMPTY(facility) AND !global_clinic_check(facility)
	  Skip
	 loop
  ENDIF
  IF msection = '6' AND tosection = '6'
	  &&check if phlebotomy - sample taken
	   IF TYPE('nwseclevel')='N' AND nwseclevel=1 AND Link.linkok OR ; 
		   TYPE('nwseclevel')='N' AND nwseclevel # 1 AND isphlebotomy AND !link.linkok &&sampe not taken
		   SKIP
		   LOOP
	   ENDIF
   ENDIF
	
   *WAIT WINDOW "RECNO ->"+ALIAS()+STR(RECNO())
   if sel > 1
	   dimension brows2d[sel,9]
   endif
   brows2d[sel,1] = name
   brows2d[sel,2] = groupcode
   brows2d[sel,3] = patientno
   brows2d[sel,4] = timesent
   brows2d[sel,5] = reference
   brows2d[sel,6] = alltrim(operator)+iif(msection=='4','-> '+ALLTRIM(doctor),"")
   brows2d[sel,7] = iif(checkfileuse('tables'),seeksay(facility,'tables','lookup'),;
	   seeksay(facility,"",'lookup','tables.dat','codes'))
   brows2d[sel,8] = cumbil
   brows2d[sel,9] = recno()
   sel = sel + 1
   SKIP
ENDDO
IF tablesused
   SELECT tables
   USE
ENDIF

if !empty(brows2d[1,1])
   *WAIT WINDOWS "ABOUT TO LOAD LINK..."
  do form (frmpath+'linkinfo')
   *anycode is returned
   *with "NAME","GROUPCODE","HOSPITAL #", "TIME SENT",;
	*  "REFERENCE","CUM_BIL","SENT BY","SECTION/DEPT","REC_NO"
	   if empty(anycode)
		   if Empty(xfile)
			   use
			   select (oldselect)
		   endif
		   return .f.
	   endif
	   xreference = brows2d[ANYCODE,5]
	   xfacility = brows2d[ANYCODE,7]
	   xrec = brows2d[ANYCODE,9]
	   SELECT link
	   *if msection == "8"  commented 'cause it was restricting payment clearance to PH only
	   IF xprocedure = 8 .and. msection $ '8349' OR xprocedure=17 AND m.msection $ "3A"
		   goto xrec
		   xlinkok = linkok
	   ENDIF
	   &&20-08-2012
	   GOTO xrec
	   xadmflag = link.transflag &&returned to cashier for admissions deposit flag
	  dimension browse2d[1]
ELSE
  =messagebox("No Patient Awaiting Service . . . ",0,;
  iif(msection="3","Nurses Station/Desk",iif(msection="1","Registration Desk",;
  iif(msection="3","Cashier/Payment Desk",iif(msection="4","Consultation(Doctors)",;
  iif(msection="5","Ward/Process Desk",;
  iif(msection="6","Lab/Xray/Scan/Theatre",;
  iif(msection="7","Billing Office",;
  iif(msection="8","Pharmacy",;
  iif(msection="9","Paediatrics",;
  iif(msection="A","Admissions","")))))))))))
endif
if Empty(xfile)
  use
  select (oldselect)
endif
return iif( Empty(anycode), .F., .T.)

This code example produces the following results:

3.4 = Math.Round( 3.45, 1)
-3.4 = Math.Round(-3.45, 1)

3.4 = Math.Round( 3.45, 1, MidpointRounding.ToEven)
3.5 = Math.Round( 3.45, 1, MidpointRounding.AwayFromZero)

-3.4 = Math.Round(-3.45, 1, MidpointRounding.ToEven)
-3.5 = Math.Round(-3.45, 1, MidpointRounding.AwayFromZero)



   }

} 
}

You would refer to the static extension like this in other parts of your application code:

object[,] myArray = GetMyArray(); // gets an array[,]
myArray.FindInDimensions(someObject);  

		static void Main(string[] args)
		{
			try
			{

				Process firstProc = new Process();
				firstProc.StartInfo.FileName = "notepad.exe";
				firstProc.EnableRaisingEvents = true;

				firstProc.Start();

				firstProc.WaitForExit();
  // Example 1: Start an app by specifying an .EXE file, no arguments

			Process.Start("calc.exe");

			Console.WriteLine("Calculator started, please press RETURN key to continue...");

			Console.ReadLine();



			// Example 2: Start an app by specifying an .EXE file, with some arguments

			Process.Start("notepad.exe", 

				Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "boot.ini"));

The answer for all of these examples is the same, you can use the classes and methods in System.Diagnostics.Process to accomplish these tasks and more.

Example 1. Running a command line application, without concern for the results:

private void simpleRun_Click(object sender, System.EventArgs e){
 System.Diagnostics.Process.Start(@"C:\listfiles.bat");
}

Example 2. Retrieving the results and waiting until the process stops (running the process synchronously):

private void runSyncAndGetResults_Click(object sender, System.EventArgs e){
 System.Diagnostics.ProcessStartInfo psi =
   new System.Diagnostics.ProcessStartInfo(@"C:\listfiles.bat");
 psi.RedirectStandardOutput = true;
 psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
 psi.UseShellExecute = false;
 System.Diagnostics.Process listFiles;
 listFiles = System.Diagnostics.Process.Start(psi);
 System.IO.StreamReader myOutput = listFiles.StandardOutput;
 listFiles.WaitForExit(2000);
 if (listFiles.HasExited)
  {
  string output = myOutput.ReadToEnd();
  this.processResults.Text = output;
 }
}

 Example 3. Displaying a URL using the default browser on the user's machine:

private void launchURL_Click(object sender, System.EventArgs e){
 string targetURL = @http://www.duncanmackenzie.net;
 System.Diagnostics.Process.Start(targetURL);
 
 * NUMBER TO WORDS
 *  if ((number / 1000) > 0)
	{
		words += NumberToWords(number / 1000) + " thousand ";
		number %= 1000;
	}

	if ((number / 100) > 0)
	{
		words += NumberToWords(number / 100) + " hundred ";
		number %= 100;
	}

	if (number > 0)
	{
		if (words != "")
			words += "and ";

		var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
		var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

		if (number < 20)
			words += unitsMap[number];
		else
		{
			words += tensMap[number / 10];
			if ((number % 10) > 0)
				words += "-" + unitsMap[number % 10];
		}
	}

	return words;
}
  GET SERVER DATE
   protected void btnGetServerDateTime_Click(object sender, EventArgs e)
	{
		SqlConnection con = new SqlConnection("Server=admin1-pc;uid=sa;Password=p@ssw0rd;Database=master;");
		SqlCommand cmd = new SqlCommand("select getdate()",con);
//Open The database
con.Open();
DataSet ds = new DataSet();
 
DateTime  strDatetime;
strDatetime =(DateTime)cmd.ExecuteScalar();

Response.Write("Server Date and Time is  :  " + strDatetime.ToString());
con.Close(); 

	}
*/

