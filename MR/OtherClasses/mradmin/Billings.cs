using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using msfunc;
using mradmin.BissClass;

namespace mradmin.DataAccess
{
    public class Billings
    { 
        public string REFERENCE { get; set; }
        public string PATIENTNO { get; set; }
        public string NAME { get; set; }
        public decimal ITEMNO { get; set; }
        public string DIAG { get; set; }
        public string PROCESS { get; set; }
        public string DESCRIPTON { get; set; }
        public string DOCTOR { get; set; }
        public string FACILITY { get; set; }
        public decimal AMOUNT { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal SEC_LEVEL { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public bool RECEIPTED { get; set; }
        public string TRANSTYPE { get; set; }
        public string PAYREF { get; set; }
        public string GROUPHEAD { get; set; }
        public string SERVICETYP { get; set; }
        public decimal PAYMENT { get; set; }
        public string GROUPCODE { get; set; }
        public string TTYPE { get; set; }
        public string GHGROUPCODE { get; set; }
        public string PAYTYPE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime OP_TIME { get; set; }
        public string CURRENCY { get; set; }
        public decimal EXRATE { get; set; }
        public decimal FCAMOUNT { get; set; }
        public string EXTDESC { get; set; }
        public string ACCOUNTTYPE { get; set; }

        public static DataTable GetBILLING(string reference)
        {
           // Billings billing = new Billings();
            
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BILLING_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
            //selectCommand.Parameters.AddWithValue("@PatientID", PatientID);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        public static DataTable GetBILLING(string reference,string process)
        {
           // Billings billing = new Billings();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BILLING_GetRefProcess"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
            selectCommand.Parameters.AddWithValue("@process", process);
            //selectCommand.Parameters.AddWithValue("@PatientID", PatientID);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        /// <summary>
        /// Gets Billing Details by date range - datefrom to dtaeto
        /// sorttype - G-grouphead N-patient name(misc_name) or P-by patient number
        /// this class includes calls to the payment and bill_adjust records
        /// </summary>
        public static DataTable GetBILLINGdetails(string grouphead, string misc_name, string patientno, string sorttype,
            DateTime datefrom, DateTime dateto)
        {
           // Billings billing = new Billings();
 
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BILLING_GetDetails"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@grouphead", grouphead);
            selectCommand.Parameters.AddWithValue("@name", misc_name);
            selectCommand.Parameters.AddWithValue("@Sorttype", sorttype);
            selectCommand.Parameters.AddWithValue("@Datefrom", datefrom );
            selectCommand.Parameters.AddWithValue("@Dateto", dateto);
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        /// <summary>
        /// Gets Account Balance up to specified date.
        /// The 'inclusive' bool flag includes transactions for specified date, if TRUE, otherwise transaction before specified date
        /// sorttype - G-grouphead N-patient name(misc_name) or P-by patient number
        /// this class includes calls to the payment and bill_adjust records
        /// </summary>
        public static Decimal GetBILLINGOpBal(string grouphead, string misc_name, string patientno, string sorttype, DateTime transdate,bool inclusive)
        {
            //Billings billing = new Billings();
            decimal xamt = 0m,rtnamt = 0m;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "BILLING_GetOpBal"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@grouphead", grouphead);
            selectCommand.Parameters.AddWithValue("@name", misc_name);
            selectCommand.Parameters.AddWithValue("@patientno", patientno);
            selectCommand.Parameters.AddWithValue("@Sorttype", sorttype);
            selectCommand.Parameters.AddWithValue("@transdate", transdate );
            selectCommand.Parameters.AddWithValue("@Inclusive", inclusive);
            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
 /*           return dt;
        }
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader();  //CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    xamt = (Decimal)reader["amount"];
                    if (reader["ttype"].ToString() == "D")
                        rtnamt += xamt;
                    else
                        rtnamt = rtnamt - xamt;
 
                    //billing.AMOUNT = (Decimal)reader["amount"]; 
                    //billing.TRANS_DATE = (DateTime)reader["trans_date"];
                    //billing.TTYPE  = reader["ttype"].ToString();
                                                                                                                                      
                }
                else
                {
                    connection.Close();
                    return 0m;

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show("" + ex, "Get Patient Bills ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return 0m;
            }
            finally
            {
                connection.Close();
            }*/
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                    xamt = (Decimal)dt.Rows[i]["amount"];
                    if (dt.Rows[i]["ttype"].ToString() == "D")
                        rtnamt += xamt;
                    else
                        rtnamt = rtnamt - xamt;             
            }
            //GET PAY RECORDS 24-12-2013
            xamt = PAYDETAIL.GetPAYOpBal(grouphead,misc_name,patientno,sorttype,transdate,inclusive );
            rtnamt = rtnamt - xamt;
            //get adjust - credit/debit records
            xamt = BILL_ADJ.GetAdjustOpBal(grouphead,misc_name,patientno,sorttype,transdate,inclusive );
            rtnamt = rtnamt - xamt;
            return rtnamt;
        } 
    /// <summary>
    /// Delete all bills on a particular reference.
    /// </summary>
    public static bool DeleteBills(string Reference)
    {
        SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
        SqlCommand deleteStatement = new SqlCommand();
        deleteStatement.CommandText = "Billing_Delete";
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

            MessageBox.Show(" "+ex, "Delete Patient Bills", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return false;
        }
        finally
        {
            connection.Close();
        }

    }
    /// <summary>
    /// Delete bill using reference, process and patientid.
    /// </summary>
    public static bool DeleteBills(string Reference, string process,string patientid,string patientname)
    {
        SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
        SqlCommand deleteStatement = new SqlCommand();
        deleteStatement.CommandText = "Billing_Delete";
        deleteStatement.Connection = connection;
        deleteStatement.CommandType = CommandType.StoredProcedure;

        deleteStatement.Parameters.AddWithValue("@Reference", Reference);
        deleteStatement.Parameters.AddWithValue("@process", process);
        deleteStatement.Parameters.AddWithValue("@patientid", patientid);
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

            MessageBox.Show(" "+ex, "Delete Patient Bills", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return false;
        }
        finally
        {
            connection.Close();
        }

    }
    /// <summary>
    /// Delete bill using reference and itemmo.
    /// </summary>
    public static bool DeleteBills(string Reference, string itemcount)
    {
        SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
        SqlCommand deleteStatement = new SqlCommand();
        deleteStatement.CommandText = "Billing_Delete";
        deleteStatement.Connection = connection;
        deleteStatement.CommandType = CommandType.StoredProcedure;

        deleteStatement.Parameters.AddWithValue("@Reference", Reference);
        deleteStatement.Parameters.AddWithValue("@Itemno", itemcount);
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

            MessageBox.Show(" " + ex, "Delete Patient Bills", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return false;
        }
        finally
        {
            connection.Close();
        }

    }
    public static void writeBILLS(bool xnewrec, string xreference, decimal xitem, string xprocess, string xdescription,
            string xgrouphtype, decimal xamount, DateTime xdate, string xname, string xgrouphead, string xfacility, string xgroupcode, string xpatientno, string debitcredit_CD, string xghgroupcod, string xoperator, DateTime xop_time, string xextdesc, string xcurrency, decimal xexrate, int xfxtype, string xdiag,string xdoctor,bool xposted,string xpayref,string xservicetyp, decimal xpayment, string xpaytype, decimal xfcamount, string in_outpatient, bool receipted, int recid)
    {
        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
        SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
        SqlCommand insertCommand = new SqlCommand();
        insertCommand.CommandText = (xservicetyp == "b") ? "capbills_Add" : (xnewrec) ? "Billing_Add" : "Billing_Update";
        insertCommand.Connection = connection;
        insertCommand.CommandType = CommandType.StoredProcedure;
        
        insertCommand.Parameters.AddWithValue("@Reference", xreference);
        insertCommand.Parameters.AddWithValue("@patientno",xpatientno);
        insertCommand.Parameters.AddWithValue("@name",xname );
        insertCommand.Parameters.AddWithValue("@Itemno", xitem);
        insertCommand.Parameters.AddWithValue("@diag",xdiag);
        insertCommand.Parameters.AddWithValue("@process",xprocess );
        insertCommand.Parameters.AddWithValue("@description",xdescription );
        insertCommand.Parameters.AddWithValue("@doctor",xdoctor );
        insertCommand.Parameters.AddWithValue("@facility",xfacility );
        insertCommand.Parameters.AddWithValue("@amount",xamount );
        insertCommand.Parameters.AddWithValue("@trans_date",xdate );
        insertCommand.Parameters.AddWithValue("@sec_level",0m);
        insertCommand.Parameters.AddWithValue("@posted",(xnewrec) ? false : xposted );
        insertCommand.Parameters.AddWithValue("@post_date",dtmin_date );
        insertCommand.Parameters.AddWithValue("@receipted",receipted );
        insertCommand.Parameters.AddWithValue("@transtype",xgrouphtype  );
        insertCommand.Parameters.AddWithValue("@payref",xpayref );
        insertCommand.Parameters.AddWithValue("@grouphead",xgrouphead );
        insertCommand.Parameters.AddWithValue("@servicetype", xservicetyp);
        insertCommand.Parameters.AddWithValue("@payment",xpayment );
        insertCommand.Parameters.AddWithValue("@groupcode",xgroupcode );
        insertCommand.Parameters.AddWithValue("@ttype", debitcredit_CD);
        insertCommand.Parameters.AddWithValue("@ghgroupcode",xghgroupcod );
        insertCommand.Parameters.AddWithValue("@operator",xoperator );
        insertCommand.Parameters.AddWithValue("@op_time",xop_time );
        insertCommand.Parameters.AddWithValue("@currency",xcurrency );
        insertCommand.Parameters.AddWithValue("@exrate",xexrate );
        insertCommand.Parameters.AddWithValue("@fcamount",xfcamount );
        insertCommand.Parameters.AddWithValue("@extdesc",xextdesc );
        insertCommand.Parameters.AddWithValue("@Accounttype", xservicetyp); // in_outpatient);
        if (!xnewrec)
            insertCommand.Parameters.AddWithValue("@RECID", recid);

        try
        {
            connection.Open();
            insertCommand.ExecuteNonQuery();
            //return true;

        }
        catch (SqlException ex)
        {
            // throw ex;
            MessageBox.Show("SQL access" + ex, "BILLINGS UPDATE", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return;
        }
        finally
        {
            connection.Close();
        }

    }
    public static void updateBILLS(string xreference, decimal xitem, string xprocess,decimal xamount,string xoperator, DateTime xop_time, DateTime transdate)
    {
        DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
        SqlCommand insertCommand = new SqlCommand();
        insertCommand.CommandText = "Billing_UpdateAmt";
        insertCommand.Connection = connection;
        insertCommand.CommandType = CommandType.StoredProcedure;

        insertCommand.Parameters.AddWithValue("@Reference", xreference);
        insertCommand.Parameters.AddWithValue("@Itemno", xitem);
        insertCommand.Parameters.AddWithValue("@process", xprocess);
        insertCommand.Parameters.AddWithValue("@transdate", transdate );
        insertCommand.Parameters.AddWithValue("@amount", xamount);
        insertCommand.Parameters.AddWithValue("@operator", xoperator);
        insertCommand.Parameters.AddWithValue("@op_time", xop_time);


        try
        {
            connection.Open();
            insertCommand.ExecuteNonQuery();
            //return true;

        }
        catch (SqlException ex)
        {
            // throw ex;
            MessageBox.Show("SQL access" + ex, "BILLINGS UPDATE", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return;
        }
        finally
        {
            connection.Close();
        }

    }            
    public static decimal getBillNextItems( string xreference,string xprocess,bool getitemcount, ref decimal xoldamt,ref int recid)
    {
        int xitem = 1;
        string selcommand2;

        if (getitemcount)
            selcommand2 = "SELECT itemno, process, amount, recid FROM Billing WHERE rtrim(reference) = '" + xreference.Trim()+"' order by itemno"; 
        else
            selcommand2 = "SELECT reference,itemno,process,description,amount,recid FROM Billing WHERE rtrim(reference) = '" + xreference.Trim()+"' AND rtrim(process) = '"+xprocess.Trim()+"' order by itemno" ; 
        DataTable dtbills = Dataaccess.GetAnytable("","MR", selcommand2,false);
        if (dtbills.Rows.Count > 0)
        {
            int xc = dtbills.Rows.Count - 1;
            xoldamt = Convert.ToDecimal(dtbills.Rows[xc]["amount"]);
            xitem = Convert.ToInt32(dtbills.Rows[xc]["itemno"])+1;
            recid = Convert.ToInt32(dtbills.Rows[xc]["recid"]);
		}
        return xitem;
    }


/*
    void savepatientdetails()
                  {
    
                    insertCommand.Parameters.AddWithValue("@reference",);
                    insertCommand.Parameters.AddWithValue("@patientno",);
                    insertCommand.Parameters.AddWithValue("@name",);
                    insertCommand.Parameters.AddWithValue("@itemno",);
                    insertCommand.Parameters.AddWithValue("@diag",);
                    insertCommand.Parameters.AddWithValue("@process",);
                    insertCommand.Parameters.AddWithValue("@description",);
                    insertCommand.Parameters.AddWithValue("@doctor",);
                    insertCommand.Parameters.AddWithValue("@facility",);
                    insertCommand.Parameters.AddWithValue("@amount",);
                    insertCommand.Parameters.AddWithValue("@trans_date",);
                    insertCommand.Parameters.AddWithValue("@sec_level",);
                    insertCommand.Parameters.AddWithValue("@posted",);
                    insertCommand.Parameters.AddWithValue("@post_date",);
                    insertCommand.Parameters.AddWithValue("@receipted",);
                    insertCommand.Parameters.AddWithValue("@transtype",);
                    insertCommand.Parameters.AddWithValue("@payref",);
                    insertCommand.Parameters.AddWithValue("@grouphead",);
                    insertCommand.Parameters.AddWithValue("@servicetyp",);
                    insertCommand.Parameters.AddWithValue("@payment",);
                    insertCommand.Parameters.AddWithValue("@groupcode",);
                    insertCommand.Parameters.AddWithValue("@ttype",);
                    insertCommand.Parameters.AddWithValue("@GHGROUPCODE",);
                    insertCommand.Parameters.AddWithValue("@paytype",);
                    insertCommand.Parameters.AddWithValue("@operator",);
                    insertCommand.Parameters.AddWithValue("@op_time",);
                    insertCommand.Parameters.AddWithValue("@currency",);
                    insertCommand.Parameters.AddWithValue("@exrate",);
                    insertCommand.Parameters.AddWithValue("@fcamount",);
                    insertCommand.Parameters.AddWithValue("@extdesc",);     
}
      try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("Unable to Open SQL Server Database Table" + ex, "Add Customer Details", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
 }
        finally
                {
                connection.Close(); */
    }
}