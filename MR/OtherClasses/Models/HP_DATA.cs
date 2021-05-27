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
    public class HP_DATA
    {
        public class REPORTS
        {
            public string CUSTOMER_REFERENCE { get; set; }
            public string NAME { get; set; }
            public DateTime? TRANS_DATE1 { get; set; }
            public DateTime? TRANS_DATE2 { get; set; }
            public bool REPORT_BY_DATE { get; set; }
            public string REPORT_TYPE { get; set; }
            public string PdfPath { get; set; }

            public ReportParameter[] RptParams { get; set; }
            public ReportDataSource RptDataSrc { get; set; }
            public string RptPath { get; set; }

            public ReportViewer GeneratedReport { get; set; }
            public AGENTS AGENTS { get; set; }
            public LOCATION LOCATION { get; set; }
            public ESTABLISHMENT ESTABLISHMENT { get; set; }
            public SYSCODETABS.BranchCodes BranchCodes { get; set; }

            public IEnumerable<AGENTS> AGENTSs { get; set; }
            public IEnumerable<LOCATION> LOCATIONs { get; set; }
            public IEnumerable<ESTABLISHMENT> ESTABLISHMENTs { get; set; }
            public IEnumerable<SYSCODETABS.BranchCodes> BranchCodess { get; set; }
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

            public string OPERATOR { get; set; }

            public DateTime? OPTIME { get; set; }





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

        public class BRANCHPROFILE
        {

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string STATECODE { get; set; }

            public string CONTACT { get; set; }

            public string CONTACTPHONE { get; set; }

            public string CONTACTEMAIL { get; set; }

            public decimal? PERCENTAGEMARKUP { get; set; }

            public decimal? MARKUPAMOUNT { get; set; }

            public decimal? MARKUPONCOST { get; set; }

            public string INCOMEACCT { get; set; }

            public string DEBTORSACCT { get; set; }

            public string INCACCTNAME { get; set; }

            public string DEBTACCTNAME { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OPDTIME { get; set; }

            public string HPLDEPOSITACCT { get; set; }

            public string HLPDEPACCTNAME { get; set; }

            public string STATENAME { get; set; }

            public decimal? DEPOSITPERCENT { get; set; }

            public decimal? SUPPLYPERCENT { get; set; }

            public string CASHRECEIPTCONTROL { get; set; }

            public string CHEQUERECEIPTCONTROL { get; set; }

            public string CASHRECPTNAME { get; set; }

            public string CHEQUERECPTNAME { get; set; }

            public string DIRECTTRANSFER { get; set; }

            public string DIRECTTRNSNAME { get; set; }

            public string INCCOMMACCT { get; set; }

            public string INCCOMMACCTNAME { get; set; }

            public decimal? PERCENTAGEONCASH { get; set; }

            public decimal? PERCENTAGEONHP { get; set; }

            public string ICACCTHP { get; set; }

            public string ICACCTHPNAME { get; set; }

            public string COSTOFSALES { get; set; }

            public string COSTOFSALESNAME { get; set; }

            public string STOCKACCT { get; set; }

            public string STOCKACCTNAME { get; set; }

            public decimal? MARKUPAMTONVALUE { get; set; }

            public decimal? MARKUPAMTONCOST { get; set; }

            public decimal? INITIALDEPOSITAMT { get; set; }

            public decimal? MTHLYINSTALLPAY { get; set; }

            public decimal? INTERESTONCASHAMT { get; set; }

            public decimal? INTERESTONPHAMT { get; set; }

            public decimal? DOWNCHARGEPERCENT { get; set; }

            public decimal? DOWNCHARGEAMT { get; set; }

            public string DOWNCHARGEACCT { get; set; }

            public string DOWNCHARGEACCTNAME { get; set; }

            public string INTSUSPENSEACCT { get; set; }

            public string INTSUSPENSEACCTNAME { get; set; }

            public string PMTSUSPENSELIABACCT { get; set; }

}

        public class CUSTOMER
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string SURNAME { get; set; }

            public string OTHERNAMES { get; set; }

            public string CATEGORY { get; set; }

            public string ADDRESS1 { get; set; }

            public string PARASTATAL { get; set; }

            public string DEPARTMENT { get; set; }

            public string STATECODE { get; set; }

            public string LOCALGOVTAREA { get; set; }

            public string COUNTRY { get; set; }

            public string PHONE { get; set; }

            public string GENDER { get; set; }

            public string CUSTOMERTYPE { get; set; }

            public string TITLE { get; set; }

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

            public string EMAIL { get; set; }

            public string FAX { get; set; }

            public decimal? BALBF { get; set; }

            public bool? CARDHOLDER { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string GLINKCODE { get; set; }

            public string DBACCOUNT { get; set; }

            public string CRACCOUNT { get; set; }

            public string CSTATUS { get; set; }

            public string AGENT { get; set; }

            public string PRICECATEG { get; set; }

            public string PHOTOLOCATION { get; set; }

            public string LOCATION { get; set; }

            public string CONTACT { get; set; }

            public string STAFFID { get; set; }

            public string BIRTHDATE { get; set; }



            #endregion Instance Properties
}

        public class DOWNCHARGE
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public decimal? MINIMUM { get; set; }

            public decimal? MAXIMUM { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? RATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OPDTIME { get; set; }



            #endregion Instance Properties
}

        public class ESTABLISHMENT
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string SHORTNAME { get; set; }

            public string ADDRESS1 { get; set; }

            public string STATECODE { get; set; }

            public string GROUPID { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OPDTIME { get; set; }

            public string CONTACT { get; set; }

            public string CONTACTPHONE { get; set; }

            public string CONTACTEMAIL { get; set; }

            public decimal? ENROLLEECOUNT { get; set; }

            public string NUMBPREFIX { get; set; }



            #endregion Instance Properties
}

        public class FCCUSTOM
        {


            #region Instance Properties

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

            public bool? CARDHOLDER { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string GLINKCODE { get; set; }

            public string DBACCOUNT { get; set; }

            public string CRACCOUNT { get; set; }

            public string CSTATUS { get; set; }

            public string AGENT { get; set; }



            #endregion Instance Properties
}

        public class glint
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string name { get; set; }

            public string debitacct { get; set; }

            public string creditacct { get; set; }



            #endregion Instance Properties
}
        
        public class HPCREVIEW
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public string HPWITNESS { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public decimal? TOTAMOUNT { get; set; }

            public decimal? INVAMOUNT { get; set; }

            public decimal? TOTFCAMOUN { get; set; }

            public decimal? EXRATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string TRANSTYPE { get; set; }

            public decimal? TENURE { get; set; }

            public decimal? DEDUCTIONRATE { get; set; }

            public DateTime? STARTDATE { get; set; }

            public DateTime? ENDDATE { get; set; }

            public decimal? LASTPAYMENT { get; set; }

            public DateTime? LASTPAYDATE { get; set; }

            public decimal? DELIVERYAMOUNT { get; set; }

            public decimal? TOTALPAYMT { get; set; }

            public decimal? QTYSUPPLIED { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string LASTPAYREF { get; set; }

            public string CURRENCY { get; set; }

            public decimal? MISCCR { get; set; }

            public decimal? MISCDR { get; set; }

            public string NOTE { get; set; }

            public DateTime? DUE_DATE { get; set; }

            public bool? MULTIACCTS { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string AGENT { get; set; }

            public decimal? INITIALDEPOSIT { get; set; }

            public decimal? TOTALINTEREST { get; set; }

            public bool? GROUPED { get; set; }

            public string GROUPID { get; set; }

            public bool? INTUPDATED { get; set; }

            public decimal? INTERESTRATE { get; set; }

            public decimal? SUPPLYRATE { get; set; }

            public string SUPPLYRATETYPE { get; set; }

            public decimal? DOWNCHARGE { get; set; }

            public string DOWNCHARGETYPE { get; set; }

            public decimal? DOWNCHARGEPAYABLE { get; set; }

            public decimal? DOWNCHARGEPAID { get; set; }

            public string LOCATION { get; set; }

            public DateTime? REVIEWDATE { get; set; }

            public string REVUSER { get; set; }

            public string REVIEWTYPE { get; set; }



            #endregion Instance Properties
}

        public class HPCREVIEWDTL
        {


            #region Instance Properties

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

            public decimal? QTYDELIVERED { get; set; }

            public DateTime? LASTDELIVERYDATE { get; set; }

            public string DELIVERYREFERENCE { get; set; }

            public decimal? UNITCOST { get; set; }



            #endregion Instance Properties
}

        public class HPLCONTROL
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

        public class HPLSETUP
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



            #endregion Instance Properties
}

        public class HPLSTLEV
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

            public bool? filemenu { get; set; }

            public bool? reportmenu { get; set; }

            public string BRANCH { get; set; }

            public string section { get; set; }



            #endregion Instance Properties

            //public static bool UExists(System.Web.HttpCookie cooks)//UE means UserExists
            //{
            //    bool ret = false;
            //    HPLSTLEV vv;
            //    using (var HPdb = new MR.DAL.HP_DATAContext())
            //    {
            //        vv = HPdb.HPLSTLEV.SqlQuery("SELECT * FROM HPLSTLEV WHERE PASSWORD = @cId ",
            //            new SqlParameter("@cId", cooks.Value)).FirstOrDefault();
            //    }
            //    if (vv != null) { ret = true; }

            //    return ret;
            //}
        }

        public class LOCATION
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string CONTACT { get; set; }

            public string PHONE { get; set; }

            public string EMAIL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OPDTIME { get; set; }



            #endregion Instance Properties
}

        public class MISCCHG
        {


            #region Instance Properties

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



            #endregion Instance Properties
}

        public class MISCCHTA
        {


            #region Instance Properties

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



            #endregion Instance Properties
}

        public class PAYMENTSREF
        {
            public string Id { get; set; }
        }

        public class PAYSCHEDULE
    {


        #region Instance Properties

        public int RECID { get; set; }

        public string REFERENCE { get; set; }

        public DateTime? DUEDATE { get; set; }

        public decimal? AMOUNTDUE { get; set; }

        public decimal? AMOUNTPAID { get; set; }

        public string DATEPAID { get; set; }

        public string PAYREFERENCE { get; set; }

        public bool? POSTED { get; set; }

        public DateTime? POST_DATE { get; set; }

        public string OPERATOR { get; set; }

        public DateTime? OPDTIME { get; set; }

        public decimal? PERIOD { get; set; }



        #endregion Instance Properties
}

        public class REFEREES
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string CUSTOMER { get; set; }

            public string TITLE { get; set; }

            public string NAME { get; set; }

            public string JOBTITLE { get; set; }

            public string COMPANY { get; set; }

            public string ADDRESS1 { get; set; }

            public string PHONE { get; set; }

            public string EMAIL { get; set; }

            public string FAX { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public bool? INTERNAL1 { get; set; }

            public bool? INTERNAL2 { get; set; }

            public string COMMENT { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OPDTIME { get; set; }

            public string GENDER { get; set; }

            public string PHOTOLOCATION { get; set; }

            public string RELATIONSHIP { get; set; }



            #endregion Instance Properties
}

        public class SECTION
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string CONTACT { get; set; }

            public string PHONE { get; set; }

            public string EMAIL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OPDTIME { get; set; }



            #endregion Instance Properties
}

        public class SUPPLIES
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string HPLREFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public string DELIVERYADR1 { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public decimal? TOTAMOUNT { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string NOTE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string PREPAREDBY { get; set; }

            public string SUPPLIEDBY { get; set; }

            public string DRIVERLICENCE { get; set; }

            public string LORRYNO { get; set; }

            public string APPROVEDBY { get; set; }

            public string RECEIVEDBY { get; set; }

            public DateTime? RECEIVEDDATE { get; set; }



            #endregion Instance Properties
}
        public class SUPPLYREF
        {
            public string Id { get; set; }
        }

        public class TENOR
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string DESCRIPTN { get; set; }

            public decimal? DURATION { get; set; }

            public decimal? PERCENTAGEMARKUP { get; set; }

            public decimal? AMOUNTMARKUP { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OPDTIME { get; set; }



            #endregion Instance Properties
}

        public class VCTDETAIL
        {


            #region Instance Properties

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

            public decimal? QTYDELIVERED { get; set; }

            public string LASTDELIVERYDATE { get; set; }

            public string DELIVERYREFERENCE { get; set; }

            public decimal? UNITCOST { get; set; }



            #endregion Instance Properties
}

        public class VCTPAY
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public decimal? TOTAMOUNT { get; set; }

            public decimal? TOTFCAMOUN { get; set; }

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

            public string GROUPID { get; set; }

            public string LOCATION { get; set; }



            #endregion Instance Properties
}

        public class VCTPAYDE
        {


            #region Instance Properties

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

            public string CUSTOMER { get; set; }

            public string GLDBACCT { get; set; }

            public string GLCRACCT { get; set; }

            public string XTDESC { get; set; }

            public string TTYPE { get; set; }

            public DateTime? DATERECEVD { get; set; }



            #endregion Instance Properties
}

        public class VCTRANS
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string COMPANY { get; set; }

            public string BRANCH { get; set; }

            public string CUSTOMER { get; set; }

            public string NAME { get; set; }

            public string HPWITNESS { get; set; }

            public decimal? TOTITEMNO { get; set; }

            public decimal? TOTAMOUNT { get; set; }

            public decimal? INVAMOUNT { get; set; }

            public decimal? TOTFCAMOUNT { get; set; }

            public decimal? EXRATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string TRANSTYPE { get; set; }

            public decimal? TENURE { get; set; }

            public decimal? DEDUCTIONRATE { get; set; }

            public DateTime? STARTDATE { get; set; }

            public DateTime? ENDDATE { get; set; }

            public decimal? LASTPAYMENT { get; set; }

            public DateTime? LASTPAYDATE { get; set; }

            public decimal? DELIVERYAMOUNT { get; set; }

            public decimal? TOTALPAYMT { get; set; }

            public decimal? QTYSUPPLIED { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string LASTPAYREF { get; set; }

            public string CURRENCY { get; set; }

            public decimal? MISCCR { get; set; }

            public decimal? MISCDR { get; set; }

            public string NOTE { get; set; }

            public DateTime? DUE_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? DTIME { get; set; }

            public string AGENT { get; set; }

            public decimal? INITIALDEPOSIT { get; set; }

            public decimal? TOTALINTEREST { get; set; }

            public bool? GROUPED { get; set; }

            public string GROUPID { get; set; }

            public bool? INTUPDATED { get; set; }

            public decimal? INTERESTRATE { get; set; }

            public decimal? SUPPLYRATE { get; set; }

            public string SUPPLYRATETYPE { get; set; }

            public decimal? DOWNCHARGE { get; set; }

            public string DOWNCHARGETYPE { get; set; }

            public decimal? DOWNCHARGEPAYABLE { get; set; }

            public decimal? DOWNCHARGEPAID { get; set; }

            public string LOCATION { get; set; }

            public bool? REVIEWED { get; set; }

            public string CSTATUS { get; set; }

            public DateTime? STATUS_DATE { get; set; }



            #endregion Instance Properties
}
        

        public class HP_DATAvm
        {
            public AGENTS AGENTS { get; set; }
            public ATMPROFILE ATMPROFILE { get; set; }
            public BRANCHPROFILE BRANCHPROFILE { get; set; }
            public CUSTOMER CUSTOMER { get; set; }
            public DOWNCHARGE DOWNCHARGE { get; set; }
            public ESTABLISHMENT ESTABLISHMENT { get; set; }
            public FCCUSTOM FCCUSTOM { get; set; }
            public glint glint { get; set; }
            public HPCREVIEW HPCREVIEW { get; set; }
            public HPCREVIEWDTL HPCREVIEWDTL { get; set; }
            public HPLCONTROL HPLCONTROL { get; set; }
            public HPLSETUP HPLSETUP { get; set; }
            public HPLSTLEV HPLSTLEV { get; set; }
            public LOCATION LOCATION { get; set; }
            public MISCCHG MISCCHG { get; set; }
            public MISCCHTA MISCCHTA { get; set; }
            public PAYSCHEDULE PAYSCHEDULE { get; set; }
            public REFEREES REFEREES { get; set; }
            public SECTION SECTION { get; set; }
            public SUPPLIES SUPPLIES { get; set; }
            public SUPPLYREF SUPPLYREF { get; set; }
            public TENOR TENOR { get; set; }
            public VCTDETAIL VCTDETAIL { get; set; }
            public VCTPAY VCTPAY { get; set; }
            public VCTPAYDE VCTPAYDE { get; set; }
            public VCTRANS VCTRANS { get; set; }
            
            public IEnumerable<AGENTS> AGENTSs { get; set; }
            public IEnumerable<ATMPROFILE> ATMPROFILEs { get; set; }
            public IEnumerable<BRANCHPROFILE> BRANCHPROFILEs { get; set; }
            public IEnumerable<CUSTOMER> CUSTOMERs { get; set; }
            public IEnumerable<DOWNCHARGE> DOWNCHARGEs { get; set; }
            public IEnumerable<ESTABLISHMENT> ESTABLISHMENTs { get; set; }
            public IEnumerable<FCCUSTOM> FCCUSTOMs { get; set; }
            public IEnumerable<glint> glints { get; set; }
            public IEnumerable<HPCREVIEW> HPCREVIEWs { get; set; }
            public IEnumerable<HPCREVIEWDTL> HPCREVIEWDTLs { get; set; }
            public IEnumerable<HPLCONTROL> HPLCONTROLs { get; set; }
            public IEnumerable<HPLSETUP> HPLSETUPs { get; set; }
            public IEnumerable<HPLSTLEV> HPLSTLEVs { get; set; }
            public IEnumerable<LOCATION> LOCATIONs { get; set; }
            public IEnumerable<MISCCHG> MISCCHGs { get; set; }
            public IEnumerable<MISCCHTA> MISCCHTAs { get; set; }
            public IEnumerable<PAYSCHEDULE> PAYSCHEDULEs { get; set; }
            public IEnumerable<REFEREES> REFEREESs { get; set; }
            public IEnumerable<SECTION> SECTIONs { get; set; }
            public IEnumerable<SUPPLIES> SUPPLIESs { get; set; }
            public IEnumerable<TENOR> TENORs { get; set; }
            public IEnumerable<VCTDETAIL> VCTDETAILs { get; set; }
            public IEnumerable<VCTPAY> VCTPAYs { get; set; }
            public IEnumerable<VCTPAYDE> VCTPAYDEs { get; set; }
            public IEnumerable<VCTRANS> VCTRANSs { get; set; }
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