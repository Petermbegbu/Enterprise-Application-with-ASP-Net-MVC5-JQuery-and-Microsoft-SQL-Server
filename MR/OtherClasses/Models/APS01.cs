using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace OtherClasses.Models
{
    public class APS01
    {
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
            public DateTime? DATERECVED { get; set; }
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
        public class APCONTROL
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
        public class APSETUP
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

            public string CLREMARKS { get; set; }



            #endregion Instance Properties
        }
        public class APSTLEV
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string OPERATOR { get; set; }

            public string PASSWORD { get; set; }

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
        public class COMPANY
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string COMPANYY { get; set; }

            public string NAME { get; set; }

            public string SHORTNAME { get; set; }

            public string STREET { get; set; }

            public string BOX { get; set; }

            public string CITY { get; set; }

            public string CSTATE { get; set; }

            public string TAX_NO { get; set; }

            public string REG_NO { get; set; }

            public string BRANCH { get; set; }

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



            #endregion Instance Properties
        }
        public class CUSTOMER
        {


            #region Instance Properties

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

            public string REWARDTYPE { get; set; }

            public string GLPAYDEBITACCT { get; set; }

            public string VENDORBANKNAME { get; set; }



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



            #endregion Instance Properties
        }
        public class GLINT
        {


            #region Instance Properties

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



            #endregion Instance Properties
        }
        public class GLUPDATE
        {


            #region Instance Properties

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



            #endregion Instance Properties
        }
        public class LINK1
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? DEBIT { get; set; }

            public decimal? CREDIT { get; set; }

            public string CTIME { get; set; }

            public DateTime? CDATE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public decimal? ITEMNO { get; set; }

            public string TTIME { get; set; }

            public string TIME_IN { get; set; }

            public string RECEIVER { get; set; }

            public decimal? DIFF { get; set; }



            #endregion Instance Properties
        }
        public class MATDETAIL
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

            public DateTime? DATERECEVD { get; set; }



            #endregion Instance Properties
        }
        public class MATRANS
        {


            #region Instance Properties

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

            public string VENDORACCT { get; set; }

            public string ORDER_NO { get; set; }

            public string MEPBREFERENCE { get; set; }

            public string DEPARTMENT { get; set; }

            public string COSTCENTRE { get; set; }



            #endregion Instance Properties
        }
        public class MEPBADJ
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string MEPBREFERENCE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public decimal? BUDGETAMT { get; set; }

            public decimal? COMMITTED { get; set; }

            public decimal? ACTUAL { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? TRANSACTONS { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OP_DATE { get; set; }



            #endregion Instance Properties
        }
        public class MEPBUDGET
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public DateTime? STARTDATE { get; set; }

            public DateTime? ENDDATE { get; set; }

            public decimal? BUDGETAMT { get; set; }

            public decimal? BCOMMITTED { get; set; }

            public decimal? ACTUAL { get; set; }

            public decimal? ADJUSTMENTS { get; set; }

            public decimal? TRANSACTONS { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string OPERATOR { get; set; }

            public DateTime? OP_DATE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string BUDGETREFERENCE { get; set; }

            public string BTYPE { get; set; }

            public string BSTATUS { get; set; }

            public decimal? BALANCE { get; set; }



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

            public string CUSTOMER { get; set; }



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
        public class ORDERS
        {


            #region Instance Properties

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



            #endregion Instance Properties
        }
        public class PASSTAB
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string OPERATOR { get; set; }

            public string MODULE { get; set; }

            public string FUNTION { get; set; }

            public string RECORD { get; set; }

            public string TTYPE { get; set; }

            public DateTime? TDATE { get; set; }

            public string TERMINAL { get; set; }



            #endregion Instance Properties
        }
        public class PD1
        {


            #region Instance Properties

            public int DiagnosisID { get; set; }

            public int PatientID { get; set; }

            public string DiagnosisDate { get; set; }

            public string CodeNumber { get; set; }

            public string DiagnosisType { get; set; }

            public string DiagnosisTitle { get; set; }

            public string Diagnosis { get; set; }

            public string DiagnosisBy { get; set; }



            #endregion Instance Properties
        }
        public class PRODSN
        {


            #region Instance Properties

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



            #endregion Instance Properties
        }
        public class PRODUCT
        {


            #region Instance Properties

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



            #endregion Instance Properties
        }
        public class PRODWD
        {


            #region Instance Properties

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



            #endregion Instance Properties
        }
        public class PRREVIEW
        {


            #region Instance Properties

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



            #endregion Instance Properties
        }
        public class VCPAYINV
        {


            #region Instance Properties

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

            public string DTIME { get; set; }

            public string CUSTOMER { get; set; }

            public string GLDBACCT { get; set; }

            public string GLCRACCT { get; set; }

            public string XTDESC { get; set; }

            public string TTYPE { get; set; }

            public DateTime? DATERECEVD { get; set; }



            #endregion Instance Properties
        }
        public class VOLDISC
        {


            #region Instance Properties

            public int RECID { get; set; }

            public decimal? MIN_AMT { get; set; }

            public decimal? MAX_AMT { get; set; }

            public decimal? RATE { get; set; }

            public string CATEGORY { get; set; }

            public string REFERENCE { get; set; }



            #endregion Instance Properties
        }
        public class VPROFILE
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string NAME { get; set; }

            public string CATEGORY { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public DateTime? DATE_REG { get; set; }

            public DateTime? TRANS_DATE { get; set; }



            #endregion Instance Properties
        }
        public class VPROFILEDETAIL
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string REFERENCE { get; set; }

            public string CATEGORY { get; set; }

            public string PRODUCT { get; set; }

            public decimal? PRICE { get; set; }

            public string VENDPROFILE { get; set; }

            public string PRODDESC { get; set; }

            public decimal? SEC_LEVEL { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public DateTime? DATE_REG { get; set; }

            public DateTime? TRANS_DATE { get; set; }



            #endregion Instance Properties
        }
        
        public class APS01vm
        {
            public SYSCODETABS.SYSCODETABSvm SYSCODETABSvm { get; set; }
            public VCTDETAIL VCTDETAIL { get; set; }
            public VCTRANS VCTRANS { get; set; }
            public AGENTS AGENTS { get; set; }
            public APCONTROL APCONTROL { get; set; }
            public APSETUP APSETUP { get; set; }
            public APSTLEV APSTLEV { get; set; }
            public ATMPROFILE ATMPROFILE { get; set; }
            public COMPANY COMPANY { get; set; }
            public CUSTOMER CUSTOMER { get; set; }
            public FCCUSTOM FCCUSTOM { get; set; }
            public GLINT GLINT { get; set; }
            public GLUPDATE GLUPDATE { get; set; }
            public LINK1 LINK1 { get; set; }
            public MATDETAIL MATDETAIL { get; set; }
            public MATRANS MATRANS { get; set; }
            public MEPBADJ MEPBADJ { get; set; }
            public MEPBUDGET MEPBUDGET { get; set; }
            public MISCCHG MISCCHG { get; set; }
            public MISCCHTA MISCCHTA { get; set; }
            public ORDERS ORDERS { get; set; }
            public PASSTAB PASSTAB { get; set; }
            public PD1 PD1 { get; set; }
            public PRODSN PRODSN { get; set; }
            public PRODUCT PRODUCT { get; set; }
            public PRODWD PRODWD { get; set; }
            public PRREVIEW PRREVIEW { get; set; }
            public VCPAYINV VCPAYINV { get; set; }
            public VCTPAY VCTPAY { get; set; }
            public VCTPAYDE VCTPAYDE { get; set; }
            public VOLDISC VOLDISC { get; set; }
            public VPROFILE VPROFILE { get; set; }
            public VPROFILEDETAIL VPROFILEDETAIL { get; set; }
            public IEnumerable<VCTDETAIL> VCTDETAILs { get; set; }
            public IEnumerable<VCTRANS> VCTRANSs { get; set; }
            public IEnumerable<AGENTS> AGENTSs { get; set; }
            public IEnumerable<APCONTROL> APCONTROLs { get; set; }
            public IEnumerable<APSETUP> APSETUPs { get; set; }
            public IEnumerable<APSTLEV> APSTLEVs { get; set; }
            public IEnumerable<ATMPROFILE> ATMPROFILEs { get; set; }
            public IEnumerable<COMPANY> COMPANYs { get; set; }
            public IEnumerable<CUSTOMER> CUSTOMERs { get; set; }
            public IEnumerable<FCCUSTOM> FCCUSTOMs { get; set; }
            public IEnumerable<GLINT> GLINTs { get; set; }
            public IEnumerable<GLUPDATE> GLUPDATEs { get; set; }
            public IEnumerable<LINK1> LINK1s { get; set; }
            public IEnumerable<MATDETAIL> MATDETAILs { get; set; }
            public IEnumerable<MATRANS> MATRANSs { get; set; }
            public IEnumerable<MEPBADJ> MEPBADJs { get; set; }
            public IEnumerable<MEPBUDGET> MEPBUDGETs { get; set; }
            public IEnumerable<MISCCHG> MISCCHGs { get; set; }
            public IEnumerable<MISCCHTA> MISCCHTAs { get; set; }
            public IEnumerable<ORDERS> ORDERSs { get; set; }
            public IEnumerable<PASSTAB> PASSTABs { get; set; }
            public IEnumerable<PD1> PD1s { get; set; }
            public IEnumerable<PRODSN> PRODSNs { get; set; }
            public IEnumerable<PRODUCT> PRODUCTs { get; set; }
            public IEnumerable<PRODWD> PRODWDs { get; set; }
            public IEnumerable<PRREVIEW> PRREVIEWs { get; set; }
            public IEnumerable<VCPAYINV> VCPAYINVs { get; set; }
            public IEnumerable<VCTPAY> VCTPAYs { get; set; }
            public IEnumerable<VCTPAYDE> VCTPAYDEs { get; set; }
            public IEnumerable<VOLDISC> VOLDISCs { get; set; }
            public IEnumerable<VPROFILE> VPROFILEs { get; set; }
            public IEnumerable<VPROFILEDETAIL> VPROFILEDETAILs { get; set; }
        }
    }
}