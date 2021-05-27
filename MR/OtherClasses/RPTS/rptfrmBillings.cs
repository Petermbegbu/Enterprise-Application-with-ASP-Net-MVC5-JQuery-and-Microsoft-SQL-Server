#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Data.SqlClient;

using Microsoft.Reporting.WebForms;
using OtherClasses.Models;
using OtherClasses;

using msfunc;
using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

#endregion

namespace MR.RPTS
{
    public partial class rptfrmBillings
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, mrptheader, sysmodule = bissclass.getRptfooter(), mrpttype;
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt,
        dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true),
        dtcurrency = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM currencycodes  order by name", true), bftab;
        bool fcgroup;
        int ansd;

        MR_DATA.MR_DATAvm fDta;

        public rptfrmBillings(int xoption, MR_DATA.MR_DATAvm fDtaa)
        {
            fDta = fDtaa;
            ansd = xoption;//either of 1-11
            fcgroup = fDta.REPORTS.chkReportGroupFamily;
            fDta.BILLCHAIN.GROUPHEAD = fDta.CUSTOMER.CUSTNO;

            if (!string.IsNullOrWhiteSpace(fDta.BILLCHAIN.PATIENTNO))
            {
                fDta.BILLCHAIN.PATIENTNO = bissclass.autonumconfig(fDta.BILLCHAIN.PATIENTNO, true, "", "9999999");
            }

            if (!string.IsNullOrWhiteSpace(fDta.BILLING.REFERENCE))
            {
                string xstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.ITEMNO, " +
                    "BILLING.DESCRIPTION, BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD," +
                    " BILLING.SERVICETYPE, BILLING.GROUPCODE, BILLING.TTYPE, BILLING.GHGROUPCODE, BILLING.EXTDESC," +
                    " BILLING.ACCOUNTTYPE, CHAR(50) AS GHNAME, 0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT," +
                    "DIAG,PROCESS,DOCTOR,FACILITY FROM BILLING WHERE REFERENCE = '" + fDta.BILLING.REFERENCE + "'";

                sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
                if (sdt.Rows.Count >= 1)
                {
                    DataRow row = sdt.Rows[0];
                    fDta.REPORTS.SessionRDLC = "BillingrptbyReference.rdlc";
                    mrptheader = "";
                    mrpttype = "BILLREF";
                    foreach (DataRow drow in sdt.Rows)
                    {
                        if (fDta.REPORTS.REPORT_TYPE3 == "chkPrintServiceDetailsonBills" &&
                            !string.IsNullOrWhiteSpace(drow["EXTDESC"].ToString()))
                            drow["description"] = drow["description"].ToString().Trim() + "\r\n" + drow["EXTDESC"].ToString();

                        if (drow["ttype"].ToString() == "D")
                            drow["debit"] = (decimal)drow["amount"];
                        else
                            drow["credit"] = (decimal)drow["amount"];
                    }
                    ds.Tables.Add(sdt);
                }
            }
        }
        
        void getData()
        {
            string paytype = "";
            if (fDta.REPORTS.ActRslt=="chkCashPayments")
                paytype = "CASH";
            else if (fDta.REPORTS.ActRslt == "chkChequePayments")
            {
                if (fDta.REPORTS.REPORT_TYPE2 == "chkFO_BankTeller")
                    paytype = "BANK TELLER";
                else if (fDta.REPORTS.REPORT_TYPE2 == "chkFO_Cheque")
                    paytype = "CHEQUE";
                else if (fDta.REPORTS.REPORT_TYPE2 == "chkFO_DirectCredit")
                    paytype = "DIRECT CREDIT";
                else if (fDta.REPORTS.REPORT_TYPE2=="chkFO_POS")
                    paytype = "POS/CREDIT CARD";
                else
                    paytype = "ALL";
            }
            mrpttype = "BILLS_ALL";
            fDta.REPORTS.SessionRDLC = "rptTransactionsDbCrBal.rdlc";
            string bstr = "", pstr = "", astr = "", includestring = "";
            if (fDta.REPORTS.chkADVCorporate && !fDta.REPORTS.chkADVIncludePVTFC)
            {
                bstr = " AND BILLING.TRANSTYPE = 'C'";
                pstr = " AND PAYDETAIL.TRANSTYPE = 'C'";
                astr = " AND BILL_ADJ.TRANSTYPE = 'C'";
            }
            if (fDta.REPORTS.chkADVIncludePVTFC && !fDta.REPORTS.chkADVCorporate)
            {
                bstr += " AND BILLING.TRANSTYPE = 'P'";
                pstr += " AND PAYDETAIL.TRANSTYPE = 'P'";
                astr += " AND BILL_ADJ.TRANSTYPE = 'P'";
            }
            if (fDta.REPORTS.chkADVunpaidmiscbills)
            {
                bstr += " AND BILLING.GROUPCODE = '' AND BILLING.PATIENTNO = ''";
                pstr += ""; // AND PAYDETAIL.GROUPHEAD = '' AND BILLING.PAYDETAIL = ''";
            }
            if (!string.IsNullOrWhiteSpace(fDta.BILLCHAIN.GROUPHEAD))
            {
                bstr += " AND BILLING.GROUPHEAD = '" + fDta.BILLCHAIN.GROUPHEAD + "'";
                pstr += " AND PAYDETAIL.GROUPHEAD = '" + fDta.BILLCHAIN.GROUPHEAD + "'";
                astr += " AND BILL_ADJ.GROUPHEAD = '" + fDta.BILLCHAIN.GROUPHEAD + "'";
                if (ansd > 3)
                {
                    fDta.REPORTS.SessionRDLC = "BillingStatement.rdlc";
                    mrpttype = "STATMT";
                }
            }
            if (!string.IsNullOrWhiteSpace(fDta.BILLCHAIN.GROUPCODE))
            {
                if (fcgroup)
                    bstr += " AND BILLING.GHGROUPCODE = '" + fDta.BILLCHAIN.GROUPCODE + "'";
                else
                    bstr += " AND BILLING.GROUPCODE = '" + fDta.BILLCHAIN.GROUPCODE + "'";
                pstr += " AND PAYDETAIL.GHGROUPCODE = '" + fDta.BILLCHAIN.GROUPCODE + "'";
                astr += " AND BILL_ADJ.GHGROUPCODE = '" + fDta.BILLCHAIN.GROUPCODE + "'";
            }
            if (!string.IsNullOrWhiteSpace(fDta.BILLCHAIN.PATIENTNO))
            {
                if (fcgroup)
                    bstr += " AND BILLING.GROUPHEAD = '" + fDta.BILLCHAIN.PATIENTNO + "'";
                else
                    bstr += " AND BILLING.PATIENTNO = '" + fDta.BILLCHAIN.PATIENTNO + "'";

                pstr += " AND PAYDETAIL.GROUPHEAD = '" + fDta.BILLCHAIN.PATIENTNO + "'";
                astr += " AND BILL_ADJ.GROUPHEAD = '" + fDta.BILLCHAIN.PATIENTNO + "'";
                fDta.REPORTS.SessionRDLC = "BillingStatement.rdlc";
                mrpttype = "STATMT";
            }
            if (fDta.REPORTS.REPORT_TYPE4=="chkPrintHMOClaimsandFFS")
                bstr += " AND BILLING.SERVICETYPE = 'C'";
            //if ()

            // if (chkPrintServiceDetailsonBills.Checked)
            //  includestring = ", RTRIM(mrb25.stk_desc)+' ('+rtrim(STR(mrb25.mrb25.qty))+' @ '+rTRIM(STR(mrb25.unitcost)) as grpdesc LEFT JOIN MRB25 ON BILLING.REFERENCE = MRB25.REFERENCE ";
            string xstring = "";
            if (fDta.REPORTS.ActRslt=="chkAllBillsandPayments")
            {
                string bqstr = "", pqstr = "", aqstr = "";
                if (!string.IsNullOrWhiteSpace(fDta.REPORTS.Searchdesc))
                {
                    bqstr = " AND BILLING.DESCRIPTION LIKE '%" + fDta.REPORTS.Searchdesc.Trim() + "%' ";
                    pqstr = " AND PAYDETAIL.DESCRIPTION LIKE '%" + fDta.REPORTS.Searchdesc.Trim() + "%' ";
                }
                if (!string.IsNullOrWhiteSpace(fDta.REPORTS.SearchName))
                {
                    bqstr += " AND BILLING.NAME LIKE '%" + fDta.REPORTS.SearchName.Trim() + "%' "; //bills
                    pqstr += " AND PAYDETAIL.NAME LIKE '%" + fDta.REPORTS.SearchName.Trim() + "%' "; //payments
                    aqstr += " AND BILL_ADJ.NAME LIKE '%" + fDta.REPORTS.SearchName.Trim() + "%' "; //adjustments
                }
                //xstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.ITEMNO, BILLING.DESCRIPTION, BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD, BILLING.SERVICETYP, BILLING.GROUPCODE, BILLING.TTYPE, BILLING.GHGROUPCODE, BILLING.EXTDESC, BILLING.ACCOUNTTYPE, PAYDETAIL.AMOUNT, PAYDETAIL.REFERENCE, PAYDETAIL.DESCRIPTION FROM BILLING LEFT JOIN PAYDETAIL ON BILLING.GHGROUPCODE = PAYDETAIL.GHGROUPCODE AND BILLING.GROUPHEAD = PAYDETAIL.GROUPHEAD WHERE BILLING.TRANS_DATE >= '" + dtDateFrom.Value.Date + "' AND BILLING.TRANS_DATE <= '" + dtDateto.Value.Date + "'";

                if (fDta.REPORTS.REPORT_TYPE3=="chkPrintSummary")
                {
                    sdt = new DataTable();
                    sdt.Columns.Add(new DataColumn("reference", typeof(string)));
                    sdt.Columns.Add(new DataColumn("name", typeof(string)));
                    sdt.Columns.Add(new DataColumn("description", typeof(string)));
                    sdt.Columns.Add(new DataColumn("paytype", typeof(string)));
                    sdt.Columns.Add(new DataColumn("balbf", typeof(decimal)));
                    sdt.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                    sdt.Columns.Add(new DataColumn("Credit", typeof(decimal)));
                    xstring = "select ghgroupcode, grouphead, grouphtype, sum(amount) as amount FROM BILLING WHERE TRANS_DATE >= '" +
                        fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + bstr +
                        " group by grouphtype, ghgroupcode, grouphead";
                    SortPrintSummary(xstring, "BILLS");

                    xstring = "select grouphtype, ghgroupcode, grouphead, paytype, sum(amount) as amount FROM PAYDETAIL WHERE "+
                        "TRANS_DATE >= '" + fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + pstr +
                        " group by grouphtype, ghgroupcode, grouphead, paytype";
                    SortPrintSummary(xstring, "PAY");

                    xstring = "select grouphtype, ghgroupcode, grouphead, transtype, sum(amount) as amount FROM FROM BILL_ADJ "+
                        "WHERE TRANS_DATE >= '" + fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + pstr +
                        " group by grouphtype, ghgroupcode, grouphead, transtype";
                    SortPrintSummary(xstring, "ADJ");

                    if (fDta.REPORTS.REPORT_TYPE1=="chkSortByGrpDateName" && mrpttype == "BILLS_ALL")
                        fDta.REPORTS.SessionRDLC = "rptTransactionsDCBNEW.rdlc";

                    ds.Tables.Add(sdt);
                    return;
                }
                else
                    xstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.ITEMNO, BILLING.DESCRIPTION, "+
                        "BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD, BILLING.SERVICETYPE, "+
                        "BILLING.GROUPCODE, BILLING.TTYPE, BILLING.GHGROUPCODE, BILLING.EXTDESC, BILLING.ACCOUNTTYPE, "+
                        "CHAR(50) AS GHNAME, 0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT FROM BILLING WHERE "+
                        "BILLING.TRANS_DATE >= '" + fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND BILLING.TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + bstr + bqstr +
                        " UNION ALL SELECT PAYDETAIL.REFERENCE, PAYDETAIL.PATIENTNO, PAYDETAIL.NAME, "+
                        "PAYDETAIL.ITEMNO, PAYDETAIL.DESCRIPTION, PAYDETAIL.AMOUNT, PAYDETAIL.TRANS_DATE, "+
                        "PAYDETAIL.TRANSTYPE, PAYDETAIL.GROUPHEAD, PAYDETAIL.SERVICETYPE, PAYDETAIL.TTYPE, CHAR(9) AS GROUPCODE,"+
                        "  PAYDETAIL.GHGROUPCODE, PAYDETAIL.EXTDESC, PAYDETAIL.ACCOUNTTYPE, CHAR(50) AS GHNAME, "+
                        "0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT FROM PAYDETAIL WHERE PAYDETAIL.TRANS_DATE >= '" +
                        fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND PAYDETAIL.TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + pstr + pqstr +
                        " UNION ALL SELECT BILL_ADJ.REFERENCE, CHAR(5) AS PATIENTNO, BILL_ADJ.NAME, 0 AS ITEMNO,"+
                        " RTRIM(BILL_ADJ.ADJUST)+' '+BILL_ADJ.COMMENTS AS DESCRIPTION, BILL_ADJ.AMOUNT, BILL_ADJ.TRANS_DATE, "+
                        "BILL_ADJ.TRANSTYPE, BILL_ADJ.GROUPHEAD, CHAR(1) AS SERVICETYPE, CHAR(9) AS GROUPCODE, BILL_ADJ.TTYPE, "+
                        "BILL_ADJ.GHGROUPCODE, CHAR(5) AS EXTDESC, CHAR(5) AS ACCOUNTTYPE, CHAR(50) AS GHNAME, 0.00 AS BALBF, "+
                        "0.00 AS DEBIT, 0.00 AS CREDIT FROM BILL_ADJ WHERE BILL_ADJ.TRANS_DATE >= '" +
                        fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND BILL_ADJ.TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() +" 23:59:59.999'" + astr + aqstr;

                if (fDta.REPORTS.REPORT_TYPE1=="chkSortByGrpDateName" && mrpttype == "BILLS_ALL")
                {
                    fDta.REPORTS.SessionRDLC = "rptTransactionsDCBNEW.rdlc";
                    xstring += "ORDER BY TRANSTYPE, GROUPHEAD, NAME, TRANS_DATE";
                }
                else
                    xstring += "ORDER BY GHGROUPCODE,GROUPHEAD,NAME,TRANS_DATE";

            }
            else if (fDta.REPORTS.ActRslt=="chkCashPayments")
                xstring = "SELECT REFERENCE, PATIENTNO, NAME, ITEMNO, DESCRIPTION, DOCTOR, FACILITY, AMOUNT, TRANS_DATE, "+
                    "TRANSTYPE, GROUPHEAD, SERVICETYPE, GROUPCODE, TTYPE, GHGROUPCODE, PAYTYPE, OPERATOR, OP_TIME, ACCOUNTTYPE, "+
                    "CURRENCY, EXRATE, FCAMOUNT, EXTDESC, DATERECEIVED, CROSSREF, PVTDEPOSIT, OPERATOR FROM PAYDETAIL WHERE "+
                    "PAYTYPE = 'CASH' AND TRANS_DATE >= '" +
                    fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                    fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + pstr;
            else if (fDta.REPORTS.ActRslt == "chkChequePayments")
            {
                if (paytype == "ALL")
                    xstring = "SELECT REFERENCE, PATIENTNO, NAME, ITEMNO, DESCRIPTION, DOCTOR, FACILITY, AMOUNT, TRANS_DATE, TRANSTYPE, "+
                        "GROUPHEAD, SERVICETYPE, GROUPCODE, TTYPE, GHGROUPCODE, PAYTYPE, OPERATOR, OP_TIME, ACCOUNTTYPE,"+
                        " CURRENCY, EXRATE, FCAMOUNT, EXTDESC, DATERECEIVED, CROSSREF, PVTDEPOSIT, OPERATOR FROM PAYDETAIL"+
                        " WHERE PAYTYPE != 'CASH' AND TRANS_DATE >= '" +
                        fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + pstr;
                else
                    xstring = "SELECT REFERENCE, PATIENTNO, NAME, ITEMNO, DESCRIPTION, DOCTOR, FACILITY, AMOUNT, TRANS_DATE, "+
                        "TRANSTYPE, GROUPHEAD, SERVICETYPE, GROUPCODE, TTYPE, GHGROUPCODE, PAYTYPE, OPERATOR, OP_TIME,"+
                        " ACCOUNTTYPE, CURRENCY, EXRATE, FCAMOUNT, EXTDESC, DATERECEIVED, CROSSREF, PVTDEPOSIT,"+
                        "OPERATOR FROM PAYDETAIL WHERE PAYTYPE = '" + paytype + "' AND TRANS_DATE >= '" +
                        fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + pstr;
            }
            else if (fDta.REPORTS.ActRslt == "chkAllPayments")
                xstring = "SELECT REFERENCE, PATIENTNO, NAME, ITEMNO, DESCRIPTION, DOCTOR, FACILITY, AMOUNT, TRANS_DATE, TRANSTYPE, "+
                    "GROUPHEAD, SERVICETYPE, GROUPCODE, TTYPE, GHGROUPCODE, PAYTYPE, OPERATOR, OP_TIME, ACCOUNTTYPE, "+
                    "CURRENCY, EXRATE, FCAMOUNT, EXTDESC, DATERECEIVED, CROSSREF, PVTDEPOSIT, OPERATOR FROM PAYDETAIL WHERE "+
                    "TRANS_DATE >= '" +
                    fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                    fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + pstr;
            else if (fDta.REPORTS.ActRslt == "chkBillsOnly")
            {
                mrpttype = "";
                xstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.ITEMNO, BILLING.DESCRIPTION, "+
                    "BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD, BILLING.SERVICETYPE, "+
                    "BILLING.GROUPCODE, BILLING.TTYPE, BILLING.FACILITY, BILLING.GHGROUPCODE, BILLING.EXTDESC, "+
                    "BILLING.ACCOUNTTYPE " + includestring + " FROM BILLING WHERE BILLING.TRANS_DATE >= '" +
                    fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND BILLING.TRANS_DATE <= '" +
                    fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + bstr;
                if (!string.IsNullOrWhiteSpace(fDta.SYSCODETABSvm.ServiceCentreCodes.type_code))
                    xstring += " AND BILLING.FACILITY = '" + fDta.SYSCODETABSvm.ServiceCentreCodes.type_code + "'";
            }
            else if (fDta.REPORTS.ActRslt=="chkQueryByExample")
            {
                if (fDta.REPORTS.REPORT_TYPE5=="chkInBills")
                    xstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.ITEMNO, BILLING.DESCRIPTION, "+
                        "BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD, BILLING.SERVICETYPE, "+
                        "BILLING.GROUPCODE, BILLING.TTYPE, BILLING.GHGROUPCODE, BILLING.EXTDESC, BILLING.ACCOUNTTYPE, 0.00 "+
                        "AS DEBIT, 0.00 AS CREDIT FROM BILLING WHERE BILLING.TRANS_DATE >= '" +
                        fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND BILLING.TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + bstr;
                else
                    xstring = "SELECT REFERENCE, PATIENTNO, NAME, ITEMNO, DESCRIPTION, DOCTOR, FACILITY, AMOUNT, TRANS_DATE, "+
                        "TRANSTYPE, GROUPHEAD, SERVICETYPE, GROUPCODE, TTYPE, GHGROUPCODE, PAYTYPE, OPERATOR, OP_TIME, "+
                        "ACCOUNTTYPE, CURRENCY, EXRATE, FCAMOUNT, EXTDESC, DATERECEIVED, CROSSREF, PVTDEPOSIT, "+
                        "0.00 AS DEBIT, 0.00 AS CREDIT FROM PAYDETAIL WHERE TRANS_DATE >= '" +
                        fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                        fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999'" + pstr;
                if (!string.IsNullOrWhiteSpace(fDta.REPORTS.Searchdesc))
                    xstring += " AND DESCRIPTION LIKE '%" + fDta.REPORTS.Searchdesc.Trim() + "%' ";
                if (!string.IsNullOrWhiteSpace(fDta.REPORTS.SearchName))
                    xstring += " AND NAME LIKE '%" + fDta.REPORTS.SearchName.Trim() + "%' ";
                if (fDta.REPORTS.nmrAmountFrom > 0 && fDta.REPORTS.nmrAmountTo > 0)
                    xstring += " AND AMOUNT >= '" + fDta.REPORTS.nmrAmountFrom + "' AND AMOUNT <= '" + fDta.REPORTS.nmrAmountTo + "'";
            }
            else if (fDta.REPORTS.ActRslt == "chkNegativeAmount")
            {
                string xfile = fDta.REPORTS.REPORT_TYPE5 == "chkInBills" ? "BILLING" : "PAYDETAIL";
                string qstr = xfile == "BILLING" ? bstr : pstr;
                xstring = "SELECT REFERENCE, PATIENTNO, NAME, ITEMNO, DESCRIPTION, AMOUNT, TRANS_DATE, TRANSTYPE, GROUPHEAD, "+
                    "SERVICETYPE, GROUPCODE, TTYPE, GHGROUPCODE, EXTDESC, ACCOUNTTYPE FROM " + xfile + " WHERE TRANS_DATE >= '" +
                    fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + "' AND TRANS_DATE <= '" +
                    fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString() + " 23:59:59.999' AND AMOUNT < 1" + qstr;

            }
            if (fDta.REPORTS.ActRslt=="chkCashPayments" || fDta.REPORTS.ActRslt == "chkChequePayments" ||
                fDta.REPORTS.ActRslt == "chkAllPayments")
            {
                if (fDta.REPORTS.nmrPayRefFrom > 0 && fDta.REPORTS.nmrPayRefTo >= fDta.REPORTS.nmrPayRefFrom)
                {
                    //WHERE CAST(RIGHT( rtrim(REFERENCE),6) AS int ) >= 2 AND CAST(RIGHT( rtrim(REFERENCE),6) AS int ) < 6
                    if (fDta.REPORTS.nmrPayRefFrom > 0m && fDta.REPORTS.nmrPayRefTo > 0m)
                    {
                        int payfrom = Convert.ToInt32(fDta.REPORTS.nmrPayRefFrom), payto = Convert.ToInt32(fDta.REPORTS.nmrPayRefTo);
                        xstring += " AND CAST( rtrim(REFERENCE) AS int ) >= '" + payfrom + "' AND CAST( rtrim(REFERENCE) AS int )"+
                            " <= '" + payto + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(fDta.BILLING.OPERATOR))
                    xstring += " AND OPERATOR LIKE '%" + fDta.BILLING.OPERATOR.Trim() + "%' ";
            }
            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            //CHECKA AND GET REMOTE DATA
            /* if (chkGetRemoteData.Checked)
             {
                 frmGetRemoteData getrd = new frmGetRemoteData(sdt, xstring, "MR");
                 getrd.ShowDialog();
                 if (Session["remotedata"] != null)
                     sdt = (DataTable)Session["remotedata"];
             }*/
            //can we select UNIQUE from sdt all groupheads for b/f purpose, if b/f is required by user ? 5.7.2017
            // DataRow row = null;
            //we should just create temp table to hold bf and pass as 2nd table to dataset
            fDta.REPORTS.SessionBalbf = 0m;
            string xselwhere = "";
            bool foundit = false;
            decimal db, cr, adj; db = cr = adj = 0m;
            DataRow dr = null;
            foreach (DataRow row in sdt.Rows)
            {
                if (fDta.REPORTS.REPORT_TYPE3=="chkPrintServiceDetailsonBills" && !string.IsNullOrWhiteSpace(row["EXTDESC"].ToString()))
                    row["description"] = row["description"].ToString().Trim() + "\r\n" + row["EXTDESC"].ToString();


                foreach (DataRow drow in bftab.Rows)
                {
                    if (row["groupcode"].ToString().Trim() + row["grouphead"].ToString().Trim() == drow["reference"].ToString().Trim())
                    {
                        foundit = true;
                        dr = drow;
                        break;
                    }
                }
                if (!foundit)
                {
                    xselwhere = row["transtype"].ToString() == "C" ? "customer where custno = '" + row["grouphead"].ToString() + "'" :
                        "patient where ghgroupcode = '" + row["ghgroupcode"].ToString() + "' and grouphead = '" +
                        row["grouphead"].ToString() + "'";
                    dr = bftab.NewRow();
                    bftab.Rows.Add(dr);
                    dr["balbf"] = 0m;
                    dr["name"] = "";
                    if (fDta.REPORTS.ActRslt=="chkAllBillsandPayments")
                    {
                        dr["name"] = bissclass.seeksay("select name from " + xselwhere, "MR", "NAME");
                        if (fDta.REPORTS.chkIncludeBf) // chkADVBroughtFwd.Checked)
                            dr["balbf"] = msmrfunc.getOpeningBalance(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "",
                                row["transtype"].ToString(), fDta.REPORTS.TRANS_DATE1.Value.Date,
                                fDta.REPORTS.TRANS_DATE2.Value.Date, ref db, ref cr, ref adj);
                        if (mrpttype == "STATMT")
                        {
                            fDta.REPORTS.SessionBalbf = (decimal)dr["balbf"];
                            // break;
                        }
                    }
                }
                if (mrpttype != "STATMT" && fDta.REPORTS.ActRslt=="chkAllBillsandPayments")
                {
                    row["GHNAME"] = dr["name"].ToString();
                    row["balbf"] = (decimal)dr["balbf"];
                }
                if (mrpttype == "STATMT" || fDta.REPORTS.ActRslt=="chkAllBillsandPayments")
                {
                    if (row["ttype"].ToString() == "D")
                        row["debit"] = (decimal)row["amount"];
                    else
                        row["credit"] = (decimal)row["amount"];
                }
            }
            ds.Tables.Add(sdt);
            //if (chkAllBillsandPayments.Checked)
            //    ds.Tables.Add(bftab);

        }
        void createBFTab()
        {
            bftab = new DataTable();
            // sdt.Columns.Add(new DataColumn("recid", typeof(int)));
            bftab.Columns.Add(new DataColumn("reference", typeof(string)));
            bftab.Columns.Add(new DataColumn("balbf", typeof(decimal)));
            bftab.Columns.Add(new DataColumn("name", typeof(string)));
        }
        void SortPrintSummary(string selstr, string xtype)
        {
            bool foundit = false;
            decimal db, cr, adj; db = cr = adj = 0m; DataRow dr = null;
            DataTable dt = Dataaccess.GetAnytable("", "MR", selstr, false);
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataRow sdtrow in sdt.Rows)
                {
                    if (xtype == "BILLS" || xtype == "ADJ" ? row["name"].ToString() == sdtrow["name"].ToString() &&
                        sdtrow["reference"].ToString() == row["grouphead"].ToString() : row["name"].ToString() ==
                        sdtrow["name"].ToString() && sdtrow["reference"].ToString() == row["grouphead"].ToString() &&
                        sdtrow["paytype"].ToString() == row["paytype"].ToString())
                    {
                        foundit = true;
                        break;
                    }
                }
                if (!foundit)
                {
                    dr = sdt.NewRow();
                    sdt.Rows.Add(dr);
                    dr["balbf"] = msmrfunc.getOpeningBalance(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "",
                        row["transtype"].ToString(), fDta.REPORTS.TRANS_DATE1.Value.Date,
                        fDta.REPORTS.TRANS_DATE2.Value.Date, ref db, ref cr, ref adj);
                    dr["reference"] = row["grouphead"].ToString();
                    dr["paytype"] = xtype == "PAY" ? row["paytype"].ToString() : "";
                    dr["debit"] = 0m; dr["credit"] = 0m;
                    if (xtype == "PAY")
                        dr["description"] = "PAYMENTS - " + row["paytype"].ToString();
                    else
                        dr["description"] = xtype == "BILLS";

                    dr["name"] = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(),
                        row["grouphtype"].ToString());

                }
                if (xtype == "BILLS")
                    dr["debit"] = (decimal)dr["debit"] + (decimal)dr["amount"];
                else if (xtype == "PAY")
                    dr["credit"] = (decimal)dr["credit"] + (decimal)dr["amount"];
                else if (xtype == "ADJ")
                {
                    if (row["transtype"].ToString() == "D")
                        dr["debit"] = (decimal)dr["debit"] + (decimal)dr["amount"];
                    else
                        dr["credit"] = (decimal)dr["credit"] + (decimal)dr["amount"];
                    dr["description"] = row["transtype"].ToString() == "D" ? "DEBIT ADJUSTMENTS" : "CREDIT ADJUSTMENTS";
                }
            }


        }
        public MR_DATA.REPORTS printprocess(bool isprint=false)
        {
            string rptfooter, rptcriteria; rptfooter = rptcriteria = "";
            //   fcgroup = false;
            fDta.REPORTS.SessionSQL = "";
            if (string.IsNullOrWhiteSpace(fDta.BILLING.REFERENCE))
            {
                //if (sdt != null)
                //{
                //    sdt.Rows.Clear();
                //    if (bftab != null && bftab.Rows.Count > 0)
                //        bftab.Clear();
                //    ds.Tables.Clear();
                //    ds.Clear();
                //}
                //else
                //    createBFTab();
                sdt = new DataTable();
                ds = new DataSet();
                createBFTab();

                getData();
                if (fDta.REPORTS.ActRslt=="chkCashPayments" || fDta.REPORTS.ActRslt == "chkChequePayments" ||
                    fDta.REPORTS.ActRslt == "chkAllPayments" || fDta.REPORTS.ActRslt == "chkBillsOnly" ||
                    fDta.REPORTS.ActRslt == "chkNegativeAmount")
                {
                    fDta.REPORTS.SessionRDLC = fDta.REPORTS.chkPMTCashierChronological ?
                        "PaymtsChronological.rdlc" : "BillingsChronological.rdlc";
                    mrpttype = "BILLCHRONOLOGICAL";
                }
                string xstr = string.IsNullOrWhiteSpace(fDta.BILLING.OPERATOR) ? "" : "[ BY : " +
                    fDta.BILLING.OPERATOR.Trim() + " ]";
                if (fDta.REPORTS.ActRslt=="chkCashPayments")
                    mrptheader = "CASH PAYMENTS " + xstr + " FOR : ";
                else if (fDta.REPORTS.ActRslt == "chkChequePayments")
                {
                    rptcriteria = fDta.REPORTS.REPORT_TYPE2=="chkFO_BankTeller" ? "BANK TELLER; " :
                        fDta.REPORTS.REPORT_TYPE2 == "chkFO_Cheque" ? "CHEQUES; " :
                        fDta.REPORTS.REPORT_TYPE2 == "chkFO_DirectCredit" ? "DIRECT CREDIT; " :
                        fDta.REPORTS.REPORT_TYPE2 == "chkFO_POS" ? "POS/CREDIT CARD " : "";
                    rptcriteria = !string.IsNullOrWhiteSpace(rptcriteria) ? "[ " + rptcriteria + "]" : "";
                    mrptheader = "CHEQUE PAYMENTS " + rptcriteria + xstr + " FOR ";
                }
                else if (fDta.REPORTS.ActRslt == "chkAllPayments")
                    mrptheader = "ALL PAYMENTS " + xstr + " FOR ";
            }

            mrptheader += fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + " TO : " +
                fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString();
            if (!string.IsNullOrWhiteSpace(fDta.BILLING.REFERENCE) ||
                fDta.REPORTS.ActRslt=="chkAllBillsandPayments")
            {
                //string xstr = dtDateFrom.Value == dtDateto.Value ? dtDateFrom.Value.Day.ToString() + bissclass.GetdateDaySuffix(dtDateFrom.Value.Day) + " " + dtDateFrom.Value.ToString("MMMM") + ", " + dtDateFrom.Value.Year.ToString() : dtDateFrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString();
                string xstr = fDta.REPORTS.TRANS_DATE1.Value ==
                    fDta.REPORTS.TRANS_DATE2.Value ? fDta.REPORTS.TRANS_DATE1.Value.ToLongDateString() :
                    fDta.REPORTS.TRANS_DATE1.Value.ToShortDateString() + " TO : " + fDta.REPORTS.TRANS_DATE2.Value.ToShortDateString();
                string xs = fDta.REPORTS.ActRslt == "chkAllBillsandPayments" ? "ALL BILLS AND PAYMENTS" : "BILLS";
                mrptheader = xs + " FOR : " + xstr;
            }
            string invref = "";
            if (mrpttype == "STATMT" && fDta.REPORTS.chkIncludeGroupInvNo)
            {
                string xgh = string.IsNullOrWhiteSpace(fDta.BILLCHAIN.GROUPHEAD) ?
                    fDta.BILLCHAIN.PATIENTNO : fDta.BILLCHAIN.GROUPHEAD;
                invref = msmrfunc.GetGroupInvno(fDta.BILLCHAIN.GROUPCODE, xgh, fDta.REPORTS.TRANS_DATE1.Value.Date);
            }
            if (mrpttype == "STATMT" || mrpttype == "BILLREF")
            {
                if (mrpttype == "STATMT")
                {
                    if (fDta.REPORTS.REPORT_TYPE1=="chkSortByGrpDateName")
                        fDta.REPORTS.SessionRDLC = "BillingStatementByDate.rdlc";
                    else
                        fDta.REPORTS.SessionRDLC = "BillingStatement.rdlc";
                }
                fDta.REPORTS.SessionCustomer = !string.IsNullOrWhiteSpace(fDta.BILLCHAIN.GROUPHEAD)
                    ? fDta.BILLCHAIN.GROUPHEAD : !string.IsNullOrWhiteSpace(fDta.BILLCHAIN.PATIENTNO) ? bchain.GROUPHEAD : "";
                fDta.REPORTS.SessionCustomertype = !string.IsNullOrWhiteSpace(fDta.BILLCHAIN.GROUPHEAD)
                    ? "C" : bchain != null ? bchain.GROUPHTYPE : "P";
            }
            if (!string.IsNullOrWhiteSpace(fDta.BILLING.REFERENCE))
            {
                fDta.REPORTS.SessionCustomertype = sdt.Rows[0]["TRANSTYPE"].ToString();
                fDta.REPORTS.SessionCustomer = sdt.Rows[0]["GROUPHEAD"].ToString();
            }
            fDta.REPORTS.SessionIncludebf = fDta.REPORTS.chkIncludeBf ? "YES" : "NO";
            if (fDta.REPORTS.REPORT_TYPE1=="chkSortByGrpDateName" && mrpttype == "BILLS_ALL")
                fDta.REPORTS.SessionRDLC = "rptTransactionsDCBNEW.rdlc";

            frmReportViewer paedreports = new frmReportViewer("", mrptheader, rptfooter, rptcriteria, "", mrpttype, invref, 0m,
                "", "", "", ds, true, 0, fDta.REPORTS.TRANS_DATE1.Value.Date, fDta.REPORTS.TRANS_DATE2.Value.Date, "", isprint, "", "",
                fDta.REPORTS);
            return paedreports.Show(fDta.REPORTS.SessionRDLC, fDta.REPORTS.SessionSQL);
        }

        /*
        private void chkInpatientStatement_Click(object sender, EventArgs e)
        {
            frmInPatientStatement inpatientstatement = new frmInPatientStatement("");
            inpatientstatement.Show();
        }

        private void chkOPDAttentBillsStat_Click(object sender, EventArgs e)
        {
            frmOPDAttendance_BillSummary opdattensumm = new frmOPDAttendance_BillSummary();
            opdattensumm.Show();
        }

        private void chkHorizontalBills_Click(object sender, EventArgs e)
        {
            frmHorizontalBills horizontalbills = new frmHorizontalBills();
            horizontalbills.Show();
        }

        private void chkEOMStatement_Click(object sender, EventArgs e)
        {
            frmEOMStatement_main eomstatement = new frmEOMStatement_main();
            eomstatement.Show();
        }

        private void txtReference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            txtReference_LostFocus(null, null);
        }

        private void txtgrouphead_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            txtgrouphead_LostFocus(null, null);
        }

        private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            txtgroupcode_LostFocus(null, null);
        }

        private void txtpatientno_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            txtPatientno_LostFocus(null, null);
        }
        */
    }
}