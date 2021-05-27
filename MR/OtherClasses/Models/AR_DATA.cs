using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OtherClasses.Models
{
    public class AR_DATA

    {
        public class REPORTS
        {
            public bool REPORT_BY_DATE { get; set; }
            public bool chkbyacctofficers { get; set; }
            public bool chkSortByOperator { get; set; }
            public bool chkBroughtForward { get; set; }
            public bool chkStaffProfiling { get; set; }
            public bool chkDomantAccts { get; set; }
            public bool chkLoyaltyCustomers { get; set; }
            public bool chkReportCustomerName { get; set; }
            public bool chkByBranch { get; set; }
            public bool chkReportBankColumn { get; set; }
            public bool chkReportbyAgent { get; set; }
            public bool chkComparativereport { get; set; }
            public bool chkIncludePayments { get; set; }

            public string ActRslt { get; set; }
            public string REPORT_TYPE1 { get; set; }
            public string REPORT_TYPE2 { get; set; }
            public string REPORT_TYPE3 { get; set; }

            public string cboSearchdesc { get; set; }
            public decimal nmrMinBalance { get; set; }
            public int nmr90days { get; set; }
            public int nmr60days { get; set; }
            public int nmr30days { get; set; }

            public DateTime? TRANS_DATE1 { get; set; }
            public DateTime? TRANS_DATE2 { get; set; }

            public string RptPath { get; set; }
            public string PdfPath { get; set; }
            public string SessionSQL { get; set; }
            public string SessionOCP { get; set; }
            public string SessionXSORT { get; set; }
            public string SessionRDLC { get; set; }
            public ReportParameter[] RptParams { get; set; }
            public ReportDataSource RptDataSrc { get; set; }
            public ReportViewer GeneratedReport { get; set; }

            //public AGENTS AGENTS { get; set; }
            //public SYSCODETABS.BranchCodes BranchCodes { get; set; }
            //public SYSCODETABS.CurrencyCodes CurrencyCodes { get; set; }
            //public SCS01.STORE STORE { get; set; }
            //public COMPANY COMPANY { get; set; }
            //public LOCATION LOCATION { get; set; }
            //public ESTABLISHMENT ESTABLISHMENT { get; set; }
            //public IEnumerable<AGENTS> AGENTSs { get; set; }
            //public IEnumerable<SYSCODETABS.BranchCodes> BranchCodess { get; set; }
            //public IEnumerable<SYSCODETABS.CurrencyCodes> CurrencyCodess { get; set; }
            //public IEnumerable<SCS01.STORE> STOREs { get; set; }
            //public IEnumerable<COMPANY> COMPANYs { get; set; }
            //public IEnumerable<LOCATION> LOCATIONs { get; set; }
            //public IEnumerable<ESTABLISHMENT> ESTABLISHMENTs { get; set; }
        }

        public class AgedAccounts
        {


            #region Instance Properties
            [Key]
            public int? SN { get; set; }

            public string Reference { get; set; }

            public string name { get; set; }

            public string lblcurrent { get; set; }

            public decimal? valcurrent { get; set; }

            public string lbl30days { get; set; }

            public decimal? val30days { get; set; }

            public string lbl60days { get; set; }

            public decimal? val60days { get; set; }

            public string lbl90days { get; set; }

            public decimal? val90days { get; set; }

            public decimal? total_amt { get; set; }

            public string dbcr { get; set; }



            #endregion Instance Properties
}
        public class AGENTS
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string ADDRESS1 { get; set; }

            public string STATECODE { get; set; }

            public string PHONE { get; set; }

            public DateTime? REGISTERED { get; set; }

            public DateTime? EXPIRYDATE { get; set; }

            public string ASTATUS { get; set; }

            public string CLASS { get; set; }

            public bool? POSTED { get; set; }

            public string EMAIL { get; set; }

            public string EPHOTO1 { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string ATYPE { get; set; }



            #endregion Instance Properties
}

        public class ARCONTROL
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string STATION { get; set; }

            public string NAME { get; set; }

            public bool? INSTALLED { get; set; }

            public string STATECODE { get; set; }

            public string REGUSER { get; set; }

            public DateTime? TA_START { get; set; }

            public bool? TA_POST { get; set; }

            public DateTime? LAST_DATE { get; set; }

            public bool? TP_START { get; set; }

            public bool? TP_ENDED { get; set; }

            public DateTime? TP_DATE { get; set; }

            public decimal? TP_PERIOD { get; set; }

            public decimal? PYEAR { get; set; }

            public bool? TR_START { get; set; }

            public bool? TR_ENDED { get; set; }

            public DateTime? TR_DATE { get; set; }

            public bool? P_START { get; set; }

            public bool? P_ENDED { get; set; }

            public DateTime? P_DATE { get; set; }

            public bool? CD_START { get; set; }

            public bool? CD_ENDED { get; set; }

            public decimal? POSTING { get; set; }

            public decimal? DELREC { get; set; }

            public string MPASS { get; set; }

            public DateTime? MPASSDT { get; set; }

            public bool? FSH { get; set; }

            public decimal? LAST_NO { get; set; }

            public bool? PAUTO { get; set; }

            public bool? CAUTO { get; set; }

            public decimal? CHARGNO { get; set; }

            public bool? ECGAUTO { get; set; }

            public decimal? ECGNO { get; set; }

            public string SERIAL { get; set; }

            public bool? PAYAUTO { get; set; }

            public decimal? PAYNO { get; set; }

            public bool? XRAUTO { get; set; }

            public decimal? XRAYNO { get; set; }

            public bool? ADJAUTO { get; set; }

            public decimal? ADJNO { get; set; }

            public DateTime? XRDATE { get; set; }

            public bool? CENTRALMPA { get; set; }

            public bool? FESTLEVPAS { get; set; }

            public bool? PRODUCTION { get; set; }

            public bool? DS { get; set; }

            public decimal? CRNO { get; set; }

            public decimal? DBNO { get; set; }

            public bool? FILEMODE { get; set; }

            public bool? BONDACTIVE { get; set; }

            public string LOCALCUR { get; set; }

            public string CRPTPATH { get; set; }

            public string CFRMPATH { get; set; }

            public string LOCSTATE { get; set; }

            public string LOCCOUNTRY { get; set; }

            public string BRANCH { get; set; }

            public string COMPANY { get; set; }

            public string CUR_SIGN { get; set; }

            public bool? PAYFLEX { get; set; }

            public string INV_FOOT_N { get; set; }

            public string INV_REMARK { get; set; }

            public bool? GLINTENABL { get; set; }

            public string GLCOMPANY { get; set; }

            public decimal? INV_BATCH { get; set; }

            public string INV_JVNO { get; set; }

            public decimal? PAY_BATCH { get; set; }

            public string PAY_JVNO { get; set; }

            public bool? POS_ACTIVE { get; set; }

            public string PAYDEBIT { get; set; }

            public string PAYCREDIT { get; set; }

            public string INVLINKCOD { get; set; }

            public string POS_CUS { get; set; }

            public string POS_STORE { get; set; }

            public bool? DNCNSOURCE { get; set; }

            public bool? STRICTPASS { get; set; }

            public string MISCLINK { get; set; }

            public bool? POSPRINTER { get; set; }

            public string PRPOLICY { get; set; }

            public bool? AUTOORDER { get; set; }

            public decimal? AUTOORDERN { get; set; }



            #endregion Instance Properties
}

        public class ARSETUP
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string COMPANY { get; set; }

            public string NAME { get; set; }

            public string ADDRESS1 { get; set; }

            public string ADDRESS2 { get; set; }

            public string ADDRESS3 { get; set; }

            public string ADDRESS4 { get; set; }

            public string ADDRESS5 { get; set; }

            public string ADDRESS6 { get; set; }

            public string ADDRESS7 { get; set; }

            public string INVNOTES { get; set; }

            public string CLREMARKS1 { get; set; }

            public string CLREMARKS2 { get; set; }

            public string BRANCH { get; set; }

            public bool? APPLYTOINV { get; set; }

            public string SLOGAN { get; set; }

            public string CLOGO { get; set; }

            public string CLREMARKS3 { get; set; }



            #endregion Instance Properties
}
        
        public class arstatemt
        {


            #region Instance Properties
            [Key]
            public string CUSTOMER { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public DateTime? DATERECEVD { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? ITEMNO { get; set; }

            public string DESCRIPTN { get; set; }

            public string TRANSTYPE { get; set; }

            public decimal? QTY { get; set; }

            public string UNIT { get; set; }

            public decimal? UNITPRICE { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public decimal? DEBIT { get; set; }

            public decimal? CREDIT { get; set; }

            public decimal? BALANCE { get; set; }

            public string DBCR { get; set; }

            public string STORE { get; set; }



            #endregion Instance Properties
}

        public class ARSTLEV
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string OPERATOR { get; set; }

            public string PASSWORD { get; set; }

            public bool? FM0 { get; set; }

            public bool? FM1 { get; set; }

            public bool? FM2 { get; set; }

            public bool? FM3 { get; set; }

            public bool? FM4 { get; set; }

            public bool? FM5 { get; set; }

            public bool? FM6 { get; set; }

            public bool? FM7 { get; set; }

            public bool? FM8 { get; set; }

            public bool? FM9 { get; set; }

            public bool? FM10 { get; set; }

            public bool? FM11 { get; set; }

            public bool? FM12 { get; set; }

            public bool? FM13 { get; set; }

            public bool? FM14 { get; set; }

            public bool? FM15 { get; set; }

            public bool? FM16 { get; set; }

            public bool? FM17 { get; set; }

            public bool? FM18 { get; set; }

            public bool? FM19 { get; set; }

            public bool? FM20 { get; set; }

            public bool? RM0 { get; set; }

            public bool? RM1 { get; set; }

            public bool? RM2 { get; set; }

            public bool? RM3 { get; set; }

            public bool? RM4 { get; set; }

            public bool? RM5 { get; set; }

            public bool? RM6 { get; set; }

            public bool? RM7 { get; set; }

            public bool? RM8 { get; set; }

            public bool? RM9 { get; set; }

            public bool? RM10 { get; set; }

            public bool? RM11 { get; set; }

            public bool? RM12 { get; set; }

            public bool? RM13 { get; set; }

            public bool? RM14 { get; set; }

            public bool? RM15 { get; set; }

            public bool? RM16 { get; set; }

            public bool? RM17 { get; set; }

            public bool? RM18 { get; set; }

            public bool? RM19 { get; set; }

            public bool? RM20 { get; set; }

            public bool? UM0 { get; set; }

            public bool? UM1 { get; set; }

            public bool? UM2 { get; set; }

            public bool? UM3 { get; set; }

            public bool? UM4 { get; set; }

            public bool? UM5 { get; set; }

            public bool? UM6 { get; set; }

            public bool? UM7 { get; set; }

            public bool? UM8 { get; set; }

            public bool? UM9 { get; set; }

            public bool? UM10 { get; set; }

            public bool? UM11 { get; set; }

            public bool? UM12 { get; set; }

            public bool? UM13 { get; set; }

            public bool? UM14 { get; set; }

            public bool? UM15 { get; set; }

            public bool? UM16 { get; set; }

            public bool? UM17 { get; set; }

            public bool? UM18 { get; set; }

            public bool? UM19 { get; set; }

            public bool? UM20 { get; set; }

            public decimal? WSECLEVEL { get; set; }

            public DateTime? PASSDATE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string INITIAL { get; set; }

            public bool? CANDELETE { get; set; }

            public bool? CANALTER { get; set; }

            public bool? CANADD { get; set; }

            public decimal? HISTYEAR { get; set; }

            public decimal? ACCESSTYPE { get; set; }

            public string SWA1 { get; set; }

            public string SWA2 { get; set; }

            public string SWA3 { get; set; }

            public string SWA4 { get; set; }

            public string SWA5 { get; set; }

            public bool? swainclusive { get; set; }

            public bool? FILEMENU { get; set; }

            public bool? REPORTMENU { get; set; }

            public bool? POSCASHIERONLY { get; set; }

            public bool? POSSALESONLY { get; set; }

            public string BRANCH { get; set; }



            #endregion Instance Properties
}

        public class ATMPROFILE
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string NAME { get; set; }

            public string CURRENCY { get; set; }

            public string COMPANY { get; set; }

            public decimal? BATCHNO { get; set; }

            public string DOCUMENT { get; set; }

            public string DEBIT { get; set; }

            public string CREDIT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public decimal? POS_CHARGE { get; set; }

            public string EXPENSE_ACCT { get; set; }



            #endregion Instance Properties
}

        public class CHDETAILS
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string INVOICENO { get; set; }

            public decimal? PT_VALUE { get; set; }

            public decimal? INV_VALUE { get; set; }

            public decimal? POINT { get; set; }

            public decimal? POINT_AVAI { get; set; }

            public decimal? PAYMENT { get; set; }

            public DateTime? TRANS_DATE { get; set; }
        }

        public class CHITEMS
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public DateTime? BIRTHDATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? POINTS_TD { get; set; }

            public decimal? POINTS_UTL { get; set; }

            public decimal? VALUE_TD { get; set; }

            public decimal? PAY_TD { get; set; }

            public decimal? CUR_VALUE { get; set; }
        }

        public class COMPANY
        {

            public int RECID { get; set; }

            //public string COMPANY { get; set; }

            public string NAME { get; set; }

            public string SHORTNAME { get; set; }

            public string STREET { get; set; }

            public string BOX { get; set; }

            public string CITY { get; set; }

            public string CSTATE { get; set; }

            public string TAX_NO { get; set; }

            public string REG_NO { get; set; }

            public decimal? ACCTYEAR { get; set; }

            public decimal? ACCTMONTH { get; set; }

            public decimal? BUDGET { get; set; }

            public decimal? ACCRUAL { get; set; }

            public decimal? BUDGET_AMT { get; set; }

            public decimal? ACTUAL_AMT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string BIGNAME { get; set; }

            public bool? GRADE { get; set; }

            public decimal? MIDAMOUNT { get; set; }

            public string NSITFNO { get; set; }

            public decimal? LEAVEPAY { get; set; }

            public string BRANCH { get; set; }
        }

        public class CPPF
        {

            public int RECID { get; set; }

            public string ITEM { get; set; }

            public string DESCRIPTN { get; set; }

            public string CUSTCLASS { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? RETAIL { get; set; }

            public decimal? WHOLESALE { get; set; }

            public decimal? PERCENTAGE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }
        }

        public class CUSTCLASS
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public decimal? PERCENTAGE { get; set; }

            public decimal? CREDITLIMIT { get; set; }
        }

        public class CUSTOMER
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string CATEGORY { get; set; }

            public string ADDRESS1 { get; set; }

            public string ZONE { get; set; }

            public string STATECODE { get; set; }

            public string COUNTRY { get; set; }

            public string PHONE { get; set; }

            public decimal? CR_LIMIT { get; set; }

            public decimal? AV_CREDIT { get; set; }

            public decimal? CUR_DB { get; set; }

            public decimal? CUR_CR { get; set; }

            public decimal? UPCUR_DB { get; set; }

            public decimal? UPCUR_CR { get; set; }

            public string ACCOUNTNO { get; set; }

            public DateTime? LASTSTATMT { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public DateTime? DATE_REG { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string CONTACT { get; set; }

            public string EMAIL { get; set; }

            public string FAX { get; set; }

            public decimal? MIN_ORD_AM { get; set; }

            public decimal? MAX_ORD_AM { get; set; }

            public decimal? DISCOUNT { get; set; }

            public decimal? BALBF { get; set; }

            public decimal? DEBIT1 { get; set; }

            public decimal? CREDIT1 { get; set; }

            public decimal? BALBF1 { get; set; }

            public decimal? DEBIT2 { get; set; }

            public decimal? CREDIT2 { get; set; }

            public decimal? BALBF2 { get; set; }

            public decimal? DEBIT3 { get; set; }

            public decimal? CREDIT3 { get; set; }

            public decimal? BALBF3 { get; set; }

            public decimal? DEBIT4 { get; set; }

            public decimal? CREDIT4 { get; set; }

            public decimal? BALBF4 { get; set; }

            public decimal? DEBIT5 { get; set; }

            public decimal? CREDIT5 { get; set; }

            public decimal? BALBF5 { get; set; }

            public decimal? DEBIT6 { get; set; }

            public decimal? CREDIT6 { get; set; }

            public decimal? BALBF6 { get; set; }

            public decimal? DEBIT7 { get; set; }

            public decimal? CREDIT7 { get; set; }

            public decimal? BALBF7 { get; set; }

            public decimal? DEBIT8 { get; set; }

            public decimal? CREDIT8 { get; set; }

            public decimal? BALBF8 { get; set; }

            public decimal? DEBIT9 { get; set; }

            public decimal? CREDIT9 { get; set; }

            public decimal? BALBF9 { get; set; }

            public decimal? DEBIT10 { get; set; }

            public decimal? CREDIT10 { get; set; }

            public decimal? BALBF10 { get; set; }

            public decimal? DEBIT11 { get; set; }

            public decimal? CREDIT11 { get; set; }

            public decimal? BALBF11 { get; set; }

            public decimal? DEBIT12 { get; set; }

            public decimal? CREDIT12 { get; set; }

            public decimal? BALBF12 { get; set; }

            public bool? CARDHOLDER { get; set; }

            public bool? MULTIACCTS { get; set; }

            public bool? WHOLESALE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string GLINKCODE { get; set; }

            public string DBACCOUNT { get; set; }

            public string CRACCOUNT { get; set; }

            public string CSTATUS { get; set; }

            public string AGENT { get; set; }

            public bool? NONCREDITACCT { get; set; }

            public bool? TRACKPRICE { get; set; }

            public string PRICECATEG { get; set; }

            public string rewardtype { get; set; }

            public string GLPAYDEBITACCT { get; set; }

            public string VENDORBANKNAME { get; set; }
        }

        public class DCARDDETA
        {

            public int RECID { get; set; }

            public string CUSTOMER { get; set; }

            public string TITLE { get; set; }

            public string FIRSTNAME { get; set; }

            public string LASTNAME { get; set; }

            public string ADDRESS1 { get; set; }

            public string CITY { get; set; }

            public string STATECODE { get; set; }

            public string GENDER { get; set; }

            public string COUNTRY { get; set; }

            public string PHONE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public DateTime? DATE_REG { get; set; }

            public string EMAIL { get; set; }

            public string FAX { get; set; }

            public string TTYPE { get; set; }

            public DateTime? BIRTHDATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? opdtime { get; set; }
        }

        public class DCARDTRANS
        {

            public int RECID { get; set; }

            public string CARDNO { get; set; }

            public decimal? VALUE { get; set; }

            public string PAYDETAIL { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public DateTime? STARTDATE { get; set; }

            public DateTime? ENDDATE { get; set; }

            public decimal? USAGEVALUE { get; set; }

            public DateTime? LASTUSAGEDATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DDTIME { get; set; }

            public string STORE { get; set; }

            public string ITEM { get; set; }

            public string CUSTOMER { get; set; }

            public string RECEIPTREF { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public decimal? DISCOUNT { get; set; }

            public decimal? MONTHLYPERIOD { get; set; }

            public string DISC_CODE { get; set; }

            public string NAME { get; set; }

            public string AGENT { get; set; }
        }

        public class FCCUSTOM
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string CURRENCY { get; set; }

            public decimal? CUR_DB { get; set; }

            public decimal? CUR_CR { get; set; }

            public string ACCOUNTNO { get; set; }

            public DateTime? LASTSTATMT { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public DateTime? DATE_REG { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? BALBF { get; set; }

            public decimal? DEBIT1 { get; set; }

            public decimal? CREDIT1 { get; set; }

            public decimal? BALBF1 { get; set; }

            public decimal? DEBIT2 { get; set; }

            public decimal? CREDIT2 { get; set; }

            public decimal? BALBF2 { get; set; }

            public decimal? DEBIT3 { get; set; }

            public decimal? CREDIT3 { get; set; }

            public decimal? BALBF3 { get; set; }

            public decimal? DEBIT4 { get; set; }

            public decimal? CREDIT4 { get; set; }

            public decimal? BALBF4 { get; set; }

            public decimal? DEBIT5 { get; set; }

            public decimal? CREDIT5 { get; set; }

            public decimal? BALBF5 { get; set; }

            public decimal? DEBIT6 { get; set; }

            public decimal? CREDIT6 { get; set; }

            public decimal? BALBF6 { get; set; }

            public decimal? DEBIT7 { get; set; }

            public decimal? CREDIT7 { get; set; }

            public decimal? BALBF7 { get; set; }

            public decimal? DEBIT8 { get; set; }

            public decimal? CREDIT8 { get; set; }

            public decimal? BALBF8 { get; set; }

            public decimal? DEBIT9 { get; set; }

            public decimal? CREDIT9 { get; set; }

            public decimal? BALBF9 { get; set; }

            public decimal? DEBIT10 { get; set; }

            public decimal? CREDIT10 { get; set; }

            public decimal? BALBF10 { get; set; }

            public decimal? DEBIT11 { get; set; }

            public decimal? CREDIT11 { get; set; }

            public decimal? BALBF11 { get; set; }

            public decimal? DEBIT12 { get; set; }

            public decimal? CREDIT12 { get; set; }

            public decimal? BALBF12 { get; set; }
        }

        public class GLINT
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string DEBITACT { get; set; }

            public string CREDITACT { get; set; }

            public decimal? BATCHNO { get; set; }

            public string DOCUMENT { get; set; }

            public string ADJUST1 { get; set; }

            public string ADJDBACT1 { get; set; }

            public string ADJCRACT1 { get; set; }

            public string ADJUST2 { get; set; }

            public string ADJDBACT2 { get; set; }

            public string ADJCRACT2 { get; set; }

            public string ADJUST3 { get; set; }

            public string ADJDBACT3 { get; set; }

            public string ADJCRACT3 { get; set; }

            public string ADJUST4 { get; set; }

            public string ADJDBACT4 { get; set; }

            public string ADJCRACT4 { get; set; }

            public string ADJUST5 { get; set; }

            public string ADJDBACT5 { get; set; }

            public string ADJCRACT5 { get; set; }

            public string ADJUST6 { get; set; }

            public string ADJDBACT6 { get; set; }

            public string ADJCRACT6 { get; set; }

            public string ADJUST7 { get; set; }

            public string ADJDBACT7 { get; set; }

            public string ADJCRACT7 { get; set; }

            public string ADJUST8 { get; set; }

            public string ADJDBACT8 { get; set; }

            public string ADJCRACT8 { get; set; }

            public string ADJUST9 { get; set; }

            public string ADJDBACT9 { get; set; }

            public string ADJCRACT9 { get; set; }

            public string ADJUST10 { get; set; }

            public string ADJDBACT10 { get; set; }

            public string ADJCRACT10 { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string COMPANY { get; set; }

            public string CURRENCY { get; set; }

            public string STORE { get; set; }
        }

        public class GLUPDATE
        {

            public int RECID { get; set; }

            public string JVNO { get; set; }

            public decimal? PERIOD { get; set; }

            public decimal? ACCTYEAR { get; set; }

            public string COMPANY { get; set; }

            public string CHEQUENO { get; set; }

            public decimal? BATCHNO { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string DESCRIPTN { get; set; }

            public string ACCOUNTNO { get; set; }

            public string TRANSTYPE { get; set; }

            public decimal? ITEMNO { get; set; }

            public decimal? AMOUNT { get; set; }

            public string CURRENCY { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string ACCTTYPE { get; set; }

            public string TGROUPTYPE { get; set; }

            public string TBTYPE { get; set; }
        }
        public class LCLIENTS
        {

            public int RECID { get; set; }

            public int? SN { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string CATEGORY { get; set; }

            public string ADDRESS1 { get; set; }

            public string ADDRESS2 { get; set; }

            public string ZONE { get; set; }

            public string STATECODE { get; set; }

            public string COUNTRY { get; set; }

            public string PHONE { get; set; }

            public decimal? CR_LIMIT { get; set; }

            public decimal? AV_CREDIT { get; set; }

            public decimal? CUR_DB { get; set; }

            public decimal? CUR_CR { get; set; }

            public decimal? UPCUR_DB { get; set; }

            public decimal? UPCUR_CR { get; set; }

            public string ACCOUNTNO { get; set; }

            public DateTime? LASTSTATMT { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public DateTime? DATE_REG { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string CONTACT { get; set; }

            public string EMAIL { get; set; }

            public string FAX { get; set; }

            public decimal? MIN_ORD_AM { get; set; }

            public decimal? MAX_ORD_AM { get; set; }

            public decimal? DISCOUNT { get; set; }

            public decimal? BALBF { get; set; }

            public decimal? DEBIT1 { get; set; }

            public decimal? CREDIT1 { get; set; }

            public decimal? BALBF1 { get; set; }

            public decimal? DEBIT2 { get; set; }

            public decimal? CREDIT2 { get; set; }

            public decimal? BALBF2 { get; set; }

            public decimal? DEBIT3 { get; set; }

            public decimal? CREDIT3 { get; set; }

            public decimal? BALBF3 { get; set; }

            public decimal? DEBIT4 { get; set; }

            public decimal? CREDIT4 { get; set; }

            public decimal? BALBF4 { get; set; }

            public decimal? DEBIT5 { get; set; }

            public decimal? CREDIT5 { get; set; }

            public decimal? BALBF5 { get; set; }

            public decimal? DEBIT6 { get; set; }

            public decimal? CREDIT6 { get; set; }

            public decimal? BALBF6 { get; set; }

            public decimal? DEBIT7 { get; set; }

            public decimal? CREDIT7 { get; set; }

            public decimal? BALBF7 { get; set; }

            public decimal? DEBIT8 { get; set; }

            public decimal? CREDIT8 { get; set; }

            public decimal? BALBF8 { get; set; }

            public decimal? DEBIT9 { get; set; }

            public decimal? CREDIT9 { get; set; }

            public decimal? BALBF9 { get; set; }

            public decimal? DEBIT10 { get; set; }

            public decimal? CREDIT10 { get; set; }

            public decimal? BALBF10 { get; set; }

            public decimal? DEBIT11 { get; set; }

            public decimal? CREDIT11 { get; set; }

            public decimal? BALBF11 { get; set; }

            public decimal? DEBIT12 { get; set; }

            public decimal? CREDIT12 { get; set; }

            public decimal? BALBF12 { get; set; }

            public bool? CARDHOLDER { get; set; }

            public bool? MULTIACCTS { get; set; }

            public bool? WHOLESALE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string GLINKCODE { get; set; }

            public string DBACCOUNT { get; set; }

            public string CRACCOUNT { get; set; }

            public string CSTATUS { get; set; }

            public string AGENT { get; set; }

            public bool? NONCREDITACCT { get; set; }

            public bool? TRACKPRICE { get; set; }

            public string PRICECATEG { get; set; }

            public string rewardtype { get; set; }
        }

        public class LINK
        {

            public int RECID { get; set; }

            public string customer { get; set; }

            public string custname { get; set; }

            public DateTime? trans_date { get; set; }

            public bool? posted { get; set; }

            public DateTime? post_date { get; set; }

            public string frsection { get; set; }

            public string timesent { get; set; }

            public string tosection { get; set; }

            public DateTime? daterec { get; set; }

            public string timerec { get; set; }

            public string inv_reference { get; set; }

            public decimal? cumbill { get; set; }

            public decimal? cumpay { get; set; }

            //public string operator { get; set; }

            public string branch { get; set; }

            public bool? linkok { get; set; }

            public string store { get; set; }
        }
        public class LINK1
        {

            public int RECID { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? DEBIT { get; set; }

            public decimal? CREDIT { get; set; }

            public string CDATETIME { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public decimal? ITEMNO { get; set; }

            public string TIME_IN { get; set; }

            public string RECEIVER { get; set; }

            public decimal? DIFF { get; set; }
        }

        public class Loyaltycardtrans
        {

            public int RECID { get; set; }

            public string CARDNO { get; set; }

            public string CARDTYPE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? USAGEVALUE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DDTIME { get; set; }

            public string CUSTOMER { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public decimal? DISCOUNT { get; set; }

            public decimal? POINTS { get; set; }

            public string NAME { get; set; }

            public string REFERENCE { get; set; }
        }

        public class MATDETAIL
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public decimal? ITEMNO { get; set; }

            public string PRODUCT { get; set; }

            public string DESCRIPTN { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? FCAMOUNT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public decimal? QTY { get; set; }

            public string UNIT { get; set; }

            public decimal? SELLUNIT { get; set; }

            public string TTYPE { get; set; }

            public string XTDESC { get; set; }

            public string STORE { get; set; }

            public string GLINTCODE { get; set; }

            public decimal? UNITPRICE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string CUSTOMER { get; set; }

            public string GLDBACCT { get; set; }

            public string GLCRACCT { get; set; }

            public string INV_TYPE { get; set; }
        }

        public class MATRANS
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public string STAFF_NO { get; set; }

            public string SALESPERS { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public decimal? TOTAMOUNT { get; set; }

            public decimal? INVAMOUNT { get; set; }

            public decimal? TOTFCAMOUN { get; set; }

            public decimal? EXRATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string TRANSTYPE { get; set; }

            public string INV_TYPE { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string PAYREF { get; set; }

            public string CURRENCY { get; set; }

            public decimal? MISCCR { get; set; }

            public decimal? MISCDR { get; set; }

            public decimal? PAYMENT { get; set; }

            public string NOTE { get; set; }

            public DateTime? DUE_DATE { get; set; }

            public bool? MULTIACCTS { get; set; }
        }

        public class MISCCHG
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string ADJUST { get; set; }

            public string NAME { get; set; }

            public decimal? PERCENTAGE { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? PAYMENT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string DESCRIPTN { get; set; }

            public string TTYPE { get; set; }

            public decimal? VALUE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string RECTYPE { get; set; }

            public string DEBITACCT { get; set; }

            public string CREDITACCT { get; set; }

            public string PAYREF { get; set; }

            public string CURRENCY { get; set; }

            public decimal? FCAMOUNT { get; set; }

            public decimal? FCVALUE { get; set; }

            public string CUSTOMER { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }
        }

        public class MISCCHTA
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public decimal? PERCENTAGE { get; set; }

            public decimal? AMOUNT { get; set; }

            public string TTYPE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string APPLYVOL { get; set; }

            public string CATEGORY { get; set; }

            public bool? DISC_CARD { get; set; }

            public decimal? STARTDAY { get; set; }

            public decimal? ENDDAY { get; set; }

            public bool? POSAUTO { get; set; }

            public decimal? POSRATE { get; set; }

            public decimal? POSAMOUNT { get; set; }

            public bool? INCLUSIVE { get; set; }
        }

        public class ORDERS
        {

            public int RECID { get; set; }

            public string ORDER_NO { get; set; }

            public DateTime? ORDER_DATE { get; set; }

            public string STORE { get; set; }

            public string OFFICER { get; set; }

            public string VENDOR_NO { get; set; }

            public DateTime? EXP_DATE { get; set; }

            public bool? RECEIVED { get; set; }

            public bool? PENDING { get; set; }

            public string REC_OFFICE { get; set; }

            public DateTime? ENT_DATE { get; set; }

            public DateTime? UPD_DATE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string COSTCENTRE { get; set; }
        }

        public class PASSTAB
        {

            public int RECID { get; set; }

            public string OPERATOR { get; set; }

            public string MODULE { get; set; }

            public string FUNTION { get; set; }

            public string RECORD { get; set; }

            public string TTYPE { get; set; }

            public DateTime? TDATE { get; set; }

            public string TERMINAL { get; set; }
        }

        public class PI_VCTDETAIL
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public decimal? ITEMNO { get; set; }

            public string PRODUCT { get; set; }

            public string DESCRIPTN { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? FCAMOUNT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public decimal? QTY { get; set; }

            public string UNIT { get; set; }

            public decimal? SELLUNIT { get; set; }

            public string TTYPE { get; set; }

            public string XTDESC { get; set; }

            public string STORE { get; set; }

            public string GLINTCODE { get; set; }

            public decimal? UNITPRICE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string CUSTOMER { get; set; }

            public string GLDACCT { get; set; }

            public string GLCRACCT { get; set; }

            public string INV_TYPE { get; set; }
        }

        public class PI_VCTRANS
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public string STAFF_NO { get; set; }

            public string SALESPERS { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public decimal? TOTAMOUNT { get; set; }

            public decimal? INVAMOUNT { get; set; }

            public decimal? TOTFCAMOUNT { get; set; }

            public decimal? EXRATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string TRANSTYPE { get; set; }

            public string INV_TYPE { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string PAYREF { get; set; }

            public string CURRENCY { get; set; }

            public decimal? MISCCR { get; set; }

            public decimal? MISCDR { get; set; }

            public decimal? PAYMENT { get; set; }

            public string NOTE { get; set; }

            public DateTime? DUE_DATE { get; set; }

            public bool? MULTIACCTS { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string VENDORACCT { get; set; }

            public string ORDER_NO { get; set; }

            public string MEPBREFERENCE { get; set; }

            public string DEPARTMENT { get; set; }

            public string COSTCENTRE { get; set; }

            public string AGENT { get; set; }

            public bool? RESTOCK { get; set; }
        }

        public class POSPAYBANK
        {

            public int RECID { get; set; }

            public string accountno { get; set; }

            public string currency { get; set; }

            public string company { get; set; }

            public string name { get; set; }
        }

        public class POSSTORES
        {

            public int RECID { get; set; }

            public string STORECODE { get; set; }

            public string NAME { get; set; }
        }

        public class PRODSN
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string PRODUCT { get; set; }

            public string STORE { get; set; }

            public decimal? ITEMNO { get; set; }

            public string PSN { get; set; }

            public string VENDOR_NO { get; set; }

            public DateTime? RECEIVEDDATE { get; set; }

            public DateTime? ISSUEDDATE { get; set; }

            public string ISSUEDTO { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OP_DATE { get; set; }

            public bool? STATUSNEW { get; set; }

            public string NAME { get; set; }
        }

        public class PRODUCT
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string PTYPE { get; set; }

            public string STOCKBIN { get; set; }

            public string STORE { get; set; }

            public string NAME { get; set; }

            public string PACK { get; set; }

            public decimal? PACKQTY { get; set; }

            public decimal? TOTALPACK { get; set; }

            public string UNIT { get; set; }

            public decimal? STOCK_QTY { get; set; }

            public decimal? GIFT_RATE { get; set; }

            public decimal? DISC_LIMIT { get; set; }

            public decimal? COST { get; set; }

            public decimal? SELL { get; set; }

            public DateTime? LAST_UPD { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string MISC_CH1 { get; set; }

            public string MISC_CH2 { get; set; }

            public string MISC_CH3 { get; set; }

            public string MISC_CH4 { get; set; }

            public string MISC_CH5 { get; set; }

            public string BIN { get; set; }

            public decimal? U_SIZE { get; set; }

            public decimal? SELLUNIT { get; set; }

            public string SELLDESC { get; set; }
        }

        public class PRODWD
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string PRODUCT { get; set; }

            public string STORE { get; set; }

            public decimal? ITEMNO { get; set; }

            public DateTime? WARRANTYSTART { get; set; }

            public DateTime? WARRANTYEND { get; set; }

            public string VENDOR_NO { get; set; }

            public string BRIEFDESC { get; set; }

            public string DETAILS { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OP_DATE { get; set; }

            public string NAME { get; set; }

            public decimal? QTY { get; set; }

            public DateTime? RECEIVEDDATE { get; set; }
        }

        public class PRREVIEW
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string PRODUCT { get; set; }

            public DateTime? EFF_DATE { get; set; }

            public string TTYPE { get; set; }

            public string NAME { get; set; }

            public string UNIT { get; set; }

            public decimal? GIFT_RATE { get; set; }

            public decimal? DISC_LIMIT { get; set; }

            public decimal? COST { get; set; }

            public decimal? SELL { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string MISC_CH1 { get; set; }

            public string MISC_CH2 { get; set; }

            public string MISC_CH3 { get; set; }

            public string MISC_CH4 { get; set; }

            public string MISC_CH5 { get; set; }
        }

        public class rewarddetails
        {

            public int RECID { get; set; }

            public string customer { get; set; }

            public string rewardtype { get; set; }

            public decimal? usagevaluetodate { get; set; }

            public decimal? pointstodate { get; set; }

            public decimal? rewardstodate { get; set; }

            public decimal? claimstodate { get; set; }

            public decimal? currentusagevalue { get; set; }

            public decimal? currentpoints { get; set; }

            public DateTime? start_date { get; set; }

            public decimal? currentreward { get; set; }

            public string lasttransaction { get; set; }

            public DateTime? lastdate { get; set; }

            public decimal? pendingclaims { get; set; }

            public decimal? valueforcard { get; set; }

            public string valueforcarddate { get; set; }
        }

        public class rewardsetup
        {

            public int RECID { get; set; }

            public string reference { get; set; }

            public string description { get; set; }

            public decimal? amttoapoint { get; set; }

            public decimal? valueofapoint { get; set; }

            public decimal? rewardpoint { get; set; }

            public decimal? valueofreward { get; set; }

            public decimal? rewardtimeframe { get; set; }

            public bool? posted { get; set; }

            public DateTime? post_date { get; set; }

            //public string operator { get; set; }

        public DateTime? opdtime { get; set; }

        public decimal? valueforcard { get; set; }
    }
        public class salessummary
        {

            public int RECID { get; set; }

            public int? SN { get; set; }

            public string NAME { get; set; }

            public string REFERENCE { get; set; }

            public decimal? OPBALANCE { get; set; }

            public decimal? DBNOTE { get; set; }

            public decimal? CRNOTE { get; set; }

            public decimal? INVAMT { get; set; }

            public decimal? VAT { get; set; }

            public decimal? DISCOUNT { get; set; }

            public decimal? NETSALES { get; set; }

            public decimal? PAYMENTS { get; set; }

            public decimal? BALANCE { get; set; }

            public string DBCR { get; set; }

            public int? readopbal { get; set; }

            public string CATEGORY { get; set; }
        }

        public class sellpricecontrolrange
        {

            public int RECID { get; set; }

            public decimal? min_amt { get; set; }

            public decimal? max_amt { get; set; }

            public decimal? rate { get; set; }
        }

        public class TC01
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? FCAMOUNT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string STATION { get; set; }

            public decimal? FUTUREAMOUNT { get; set; }

            public decimal? FUTUREITEMNO { get; set; }

            public decimal? FURUTEFCAMOUNT { get; set; }

            public decimal? ITEMNO { get; set; }
        }

        public class TC02
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string DESCRIPTN { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? FCAMOUNT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string TICKETTYPE { get; set; }

            public string XDESC { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string TICKETNO { get; set; }

            public string CLASS { get; set; }

            public string CATEGORY { get; set; }

            public decimal? ACTUAL { get; set; }

            public bool? UTILIZED { get; set; }

            public DateTime? U_DATE { get; set; }

            public string U_ROUTE { get; set; }

            public string U_FLIGHT { get; set; }

            public string U_DEPARTURE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OPTIME { get; set; }

            public string STATION { get; set; }

            public DateTime? BOOKING { get; set; }
        }

        public class TC03
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string DESCRIPTN { get; set; }

            public decimal? EXECUTIVE { get; set; }

            public decimal? BUSINESS { get; set; }

            public decimal? ECONOMY { get; set; }

            public decimal? CHILD { get; set; }

            public decimal? INFANT { get; set; }

            public decimal? REBATE { get; set; }

            public decimal? CAPACITY { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string GLINTCODE { get; set; }

            public string TAX1 { get; set; }

            public decimal? TAX1PCNT { get; set; }

            public decimal? TAX1AMT { get; set; }

            public string TAX2 { get; set; }

            public decimal? TAX2PCNT { get; set; }

            public decimal? TAX2AMT { get; set; }

            public string TAX3 { get; set; }

            public decimal? TAX3PCT { get; set; }

            public decimal? TAX3AMT { get; set; }

            public string TAX4 { get; set; }

            public decimal? TAX4PCNT { get; set; }

            public decimal? TAX4AMT { get; set; }

            public string TAX5 { get; set; }

            public decimal? TAX5PCNT { get; set; }

            public decimal? TAX5AMT { get; set; }
        }

        public class TC04
        {

            public int RECID { get; set; }

            public string DESCRIPTN { get; set; }

            public string STATION { get; set; }

            public string BRANCH { get; set; }

            public string NAME { get; set; }

            public string GLINTCODE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }
        }

        public class TC05
        {

            public int RECID { get; set; }

            public string DESCRIPTN { get; set; }

            public string STATION { get; set; }

            public string BRANCH { get; set; }

            public string NAME { get; set; }

            public string GLDBACCT { get; set; }

            public string GLCRACCT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }
        }

        public class TC06
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string PRODUCT { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? UNITPRICE { get; set; }
        }

        public class VCPAYINV
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public decimal? INVAMOUNT { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string PAYREF { get; set; }

            public string CURRENCY { get; set; }

            public decimal? PAYMENT { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }
        }

        public class VCTDETAIL
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public decimal? ITEMNO { get; set; }

            public string PRODUCT { get; set; }

            public string DESCRIPTN { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? FCAMOUNT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public decimal? QTY { get; set; }

            public string UNIT { get; set; }

            public decimal? SELLUNIT { get; set; }

            public string TTYPE { get; set; }

            public string XTDESC { get; set; }

            public string STORE { get; set; }

            public string GLINTCODE { get; set; }

            public decimal? UNITPRICE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string CUSTOMER { get; set; }

            public string GLDBACCT { get; set; }

            public string GLCRACCT { get; set; }

            public string INV_TYPE { get; set; }
        }

        public class VCTPAY
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public string STAFF_NO { get; set; }

            public string SALESPERS { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public decimal? TOTAMOUNT { get; set; }

            public decimal? TOTFCAMOUNT { get; set; }

            public decimal? EXRATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string TRANSTYPE { get; set; }

            public string INV_TYPE { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string CURRENCY { get; set; }

            public string NOTE { get; set; }

            public string INV_NO { get; set; }

            public bool? POSTDATED { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string MEPREFERENCE { get; set; }
        }

        public class VCTPAYDE
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public decimal? ITEMNO { get; set; }

            public string DESCRIPTN { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? FCAMOUNT { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string TRANSTYPE { get; set; }

            public string GLINTCODE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public bool? POSTDATED { get; set; }

            public DateTime? DTIME { get; set; }

            public string CUSTOMER { get; set; }

            public string GLDBACCT { get; set; }

            public string GLCRACCT { get; set; }

            public string XTDESC { get; set; }

            public string TTYPE { get; set; }

            public DateTime? DATERECEVD { get; set; }

            public string PAYTYPE { get; set; }
        }

        public class VCTRANS
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public string STAFF_NO { get; set; }

            public string SALESPERS { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public decimal? TOTAMOUNT { get; set; }

            public decimal? INVAMOUNT { get; set; }

            public decimal? TOTFCAMOUNT { get; set; }

            public decimal? EXRATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string TRANSTYPE { get; set; }

            public string INV_TYPE { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string PAYREF { get; set; }

            public string CURRENCY { get; set; }

            public decimal? MISCCR { get; set; }

            public decimal? MISCDR { get; set; }

            public decimal? PAYMENT { get; set; }

            public string NOTE { get; set; }

            public DateTime? DUE_DATE { get; set; }

            public bool? MULTIACCTS { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string VENDORACCT { get; set; }

            public string ORDER_NO { get; set; }

            public string MEPBREFERENCE { get; set; }

            public string DEPARTMENT { get; set; }

            public string COSTCENTRE { get; set; }

            public string AGENT { get; set; }

            public bool? RESTOCK { get; set; }
        }

        public class VOLDISC
        {

            public int RECID { get; set; }

            public decimal? MIN_AMT { get; set; }

            public decimal? MAX_AMT { get; set; }

            public decimal? RATE { get; set; }

            public string CATEGORY { get; set; }

            public string REFERENCE { get; set; }
        }


        public class AR_DATAvm
        {
            public AgedAccounts AgedAccounts { get; set; }
            public AGENTS AGENTS { get; set; }
            public ARCONTROL ARCONTROL { get; set; }
            public ARSETUP ARSETUP { get; set; }
            public arstatemt arstatemt { get; set; }
            public ARSTLEV ARSTLEV { get; set; }
            public ATMPROFILE ATMPROFILE { get; set; }
            public CHDETAILS CHDETAILS { get; set; }
            public CHITEMS CHITEMS { get; set; }
            public COMPANY COMPANY { get; set; }
            public CPPF CPPF { get; set; }
            public CUSTCLASS CUSTCLASS { get; set; }
            public CUSTOMER CUSTOMER { get; set; }
            public DCARDDETA DCARDDETA { get; set; }
            public DCARDTRANS DCARDTRANS { get; set; }
            public FCCUSTOM FCCUSTOM { get; set; }
            public GLINT GLINT { get; set; }
            public GLUPDATE GLUPDATE { get; set; }
            public LCLIENTS LCLIENTS { get; set; }
            public LINK LINK { get; set; }
            public LINK1 LINK1 { get; set; }
            public Loyaltycardtrans Loyaltycardtrans { get; set; }
            public MATDETAIL MATDETAIL { get; set; }
            public MATRANS MATRANS { get; set; }
            public MISCCHG MISCCHG { get; set; }
            public MISCCHTA MISCCHTA { get; set; }
            public ORDERS ORDERS { get; set; }
            public PASSTAB PASSTAB { get; set; }
            public PI_VCTDETAIL PI_VCTDETAIL { get; set; }
            public PI_VCTRANS PI_VCTRANS { get; set; }
            public POSPAYBANK POSPAYBANK { get; set; }
            public POSSTORES POSSTORES { get; set; }
            public PRODSN PRODSN { get; set; }
            public PRODUCT PRODUCT { get; set; }
            public PRODWD PRODWD { get; set; }
            public PRREVIEW PRREVIEW { get; set; }
            public rewarddetails rewarddetails { get; set; }
            public rewardsetup rewardsetup { get; set; }
            public salessummary salessummary { get; set; }
            public sellpricecontrolrange sellpricecontrolrange { get; set; }
            public TC01 TC01 { get; set; }
            public TC02 TC02 { get; set; }
            public TC03 TC03 { get; set; }
            public TC04 TC04 { get; set; }
            public TC05 TC05 { get; set; }
            public TC06 TC06 { get; set; }
            public VCPAYINV VCPAYINV { get; set; }
            public VCTDETAIL VCTDETAIL { get; set; }
            public VCTPAY VCTPAY { get; set; }
            public VCTPAYDE VCTPAYDE { get; set; }
            public VCTRANS VCTRANS { get; set; }
            public VOLDISC VOLDISC { get; set; }
            public REPORTS REPORTS { get; set; }


            public IEnumerable<AgedAccounts> AgedAccountss { get; set; }
            public IEnumerable<AGENTS> AGENTSs { get; set; }
            public IEnumerable<ARCONTROL> ARCONTROLs { get; set; }
            public IEnumerable<ARSETUP> ARSETUPs { get; set; }
            public IEnumerable<arstatemt> arstatemts { get; set; }
            public IEnumerable<ARSTLEV> ARSTLEVs { get; set; }
            public IEnumerable<ATMPROFILE> ATMPROFILEs { get; set; }
            public IEnumerable<CHDETAILS> CHDETAILSs { get; set; }
            public IEnumerable<CHITEMS> CHITEMSs { get; set; }
            public IEnumerable<COMPANY> COMPANYs { get; set; }
            public IEnumerable<CPPF> CPPFs { get; set; }
            public IEnumerable<CUSTCLASS> CUSTCLASSs { get; set; }
            public IEnumerable<CUSTOMER> CUSTOMERs { get; set; }
            public IEnumerable<DCARDDETA> DCARDDETAs { get; set; }
            public IEnumerable<DCARDTRANS> DCARDTRANSs { get; set; }
            public IEnumerable<FCCUSTOM> FCCUSTOMs { get; set; }
            public IEnumerable<GLINT> GLINTs { get; set; }
            public IEnumerable<GLUPDATE> GLUPDATEs { get; set; }
            public IEnumerable<LCLIENTS> LCLIENTSs { get; set; }
            public IEnumerable<LINK> LINKs { get; set; }
            public IEnumerable<LINK1> LINK1s { get; set; }
            public IEnumerable<Loyaltycardtrans> Loyaltycardtranss { get; set; }
            public IEnumerable<MATDETAIL> MATDETAILs { get; set; }
            public IEnumerable<MATRANS> MATRANSs { get; set; }
            public IEnumerable<MISCCHG> MISCCHGs { get; set; }
            public IEnumerable<MISCCHTA> MISCCHTAs { get; set; }
            public IEnumerable<ORDERS> ORDERSs { get; set; }
            public IEnumerable<PASSTAB> PASSTABs { get; set; }
            public IEnumerable<PI_VCTDETAIL> PI_VCTDETAILs { get; set; }
            public IEnumerable<PI_VCTRANS> PI_VCTRANSs { get; set; }
            public IEnumerable<POSPAYBANK> POSPAYBANKs { get; set; }
            public IEnumerable<POSSTORES> POSSTORESs { get; set; }
            public IEnumerable<PRODSN> PRODSNs { get; set; }
            public IEnumerable<PRODUCT> PRODUCTs { get; set; }
            public IEnumerable<PRODWD> PRODWDs { get; set; }
            public IEnumerable<PRREVIEW> PRREVIEWs { get; set; }
            public IEnumerable<rewarddetails> rewarddetailss { get; set; }
            public IEnumerable<rewardsetup> rewardsetups { get; set; }
            public IEnumerable<salessummary> salessummarys { get; set; }
            public IEnumerable<sellpricecontrolrange> sellpricecontrolranges { get; set; }
            public IEnumerable<TC01> TC01s { get; set; }
            public IEnumerable<TC02> TC02s { get; set; }
            public IEnumerable<TC03> TC03s { get; set; }
            public IEnumerable<TC04> TC04s { get; set; }
            public IEnumerable<TC05> TC05s { get; set; }
            public IEnumerable<TC06> TC06s { get; set; }
            public IEnumerable<VCPAYINV> VCPAYINVs { get; set; }
            public IEnumerable<VCTDETAIL> VCTDETAILs { get; set; }
            public IEnumerable<VCTPAY> VCTPAYs { get; set; }
            public IEnumerable<VCTPAYDE> VCTPAYDEs { get; set; }
            public IEnumerable<VCTRANS> VCTRANSs { get; set; }
            public IEnumerable<VOLDISC> VOLDISCs { get; set; }


            public HP_DATA.HP_DATAvm HP_DATAvm { get; set; }
            public APS01.APS01vm APS01vm { get; set; }
            public FAS01.FAS01vm FAS01vm { get; set; }
            public GLS01.GLS01vm GLS01vm { get; set; }
            public MR_DATA.MR_DATAvm MR_DATAvm { get; set; }
            public PAYPER.PAYPERvm PAYPERvm { get; set; }
            public SCS01.SCS01vm SCS01vm { get; set; }
            public SYSCODETABS.SYSCODETABSvm SYSCODETABSvm { get; set; }
        }
    }
}