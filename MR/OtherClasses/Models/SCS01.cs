using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OtherClasses.Models
{
    public class SCS01
    {
        
            public class associateditem
            {
                public int recid { get; set; }
                public string store { get; set; }
                public string item { get; set; }
                public Nullable<decimal> qty { get; set; }
            }
            public class COMPANY
            {
                public int RECID { get; set; }
                public string COMPANY1 { get; set; }
                public string NAME { get; set; }
                public string STREET { get; set; }
                public string BOX { get; set; }
                public string CITY { get; set; }
                public string STATE { get; set; }
                public string TAX_NO { get; set; }
                public string REG_NO { get; set; }
                public string BRANCH { get; set; }
                public Nullable<int> FINYEAR { get; set; }
                public Nullable<short> FINMONTH { get; set; }
                public Nullable<decimal> BUDGET { get; set; }
                public Nullable<decimal> ACCRUAL { get; set; }
                public Nullable<decimal> BUDGET_AMT { get; set; }
                public Nullable<decimal> ACTUAL_AMT { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
            }
            public class COST
            {
                public int RECID { get; set; }
                public string COSTS { get; set; }
                public string CENTER { get; set; }
            }
            public class DRUGSLIP
            {
                public int RECID { get; set; }
                public string DRUG { get; set; }
                public string LAB1 { get; set; }
                public string LAB2 { get; set; }
                public string LAB3 { get; set; }
                public string LAB4 { get; set; }
                public string LAB5 { get; set; }
                public string LAB6 { get; set; }
                public string LAB7 { get; set; }
                public string LAB8 { get; set; }
                public string STFREETEXT { get; set; }
            }
            public class FIFO
            {
                public int RECID { get; set; }
                public string STORE { get; set; }
                public string ITEM { get; set; }
                public Nullable<decimal> COST { get; set; }
                public Nullable<decimal> SELL { get; set; }
                public Nullable<decimal> QTY { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> TRANS_DATE { get; set; }
                public Nullable<decimal> BALANCE { get; set; }
                public Nullable<decimal> U_SIZE { get; set; }
                public Nullable<System.DateTime> EXPIRYDATE { get; set; }
                public Nullable<decimal> WHSELL { get; set; }
            }
            public class GLINT
            {
                public int RECID { get; set; }
                public string REFERENCE { get; set; }
                public string NAME { get; set; }
                public string STORE { get; set; }
                public string DEBITACT { get; set; }
                public string CREDITACT { get; set; }
                public Nullable<decimal> BATCHNO { get; set; }
                public string DOC_ID { get; set; }
                public string CATEGORY { get; set; }
                public string CURRENCY { get; set; }
                public Nullable<decimal> OPENINGSTK { get; set; }
                public Nullable<decimal> PERIOD1 { get; set; }
                public Nullable<decimal> PERIOD2 { get; set; }
                public Nullable<decimal> PERIOD3 { get; set; }
                public Nullable<decimal> PERIOD4 { get; set; }
                public Nullable<decimal> PERIOD5 { get; set; }
                public Nullable<decimal> PERIOD6 { get; set; }
                public Nullable<decimal> PERIOD7 { get; set; }
                public Nullable<decimal> PERIOD8 { get; set; }
                public Nullable<decimal> PERIOD9 { get; set; }
                public Nullable<decimal> PERIOD10 { get; set; }
                public Nullable<decimal> PERIOD11 { get; set; }
                public Nullable<decimal> PERIOD12 { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
            }
            public class GLUPDATE
            {
                public int RECID { get; set; }
                public string COMPANY { get; set; }
                public Nullable<int> FINYEAR { get; set; }
                public string ACCOUNTNO { get; set; }
                public string CURRENCY { get; set; }
                public Nullable<System.DateTime> TRANS_DATE { get; set; }
                public string DESCRIPTN { get; set; }
                public Nullable<decimal> OPENINGSTK { get; set; }
                public Nullable<decimal> PERIOD1 { get; set; }
                public Nullable<decimal> PERIOD2 { get; set; }
                public Nullable<decimal> PERIOD3 { get; set; }
                public Nullable<decimal> PERIOD4 { get; set; }
                public Nullable<decimal> PERIOD5 { get; set; }
                public Nullable<decimal> PERIOD6 { get; set; }
                public Nullable<decimal> PERIOD7 { get; set; }
                public Nullable<decimal> PERIOD8 { get; set; }
                public Nullable<decimal> PERIOD9 { get; set; }
                public Nullable<decimal> PERIOD10 { get; set; }
                public Nullable<decimal> PERIOD11 { get; set; }
                public Nullable<decimal> PERIOD12 { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
            }
            public class ISS_ADDRESS
            {
                public int RECID { get; set; }
                public string name { get; set; }
                public string address { get; set; }
                public string issuedby { get; set; }
            }
            public class ISSUE
            {
                public int RECID { get; set; }
                public string REF_NO { get; set; }
                public string STORE { get; set; }
                public string ITEM { get; set; }
                public string TYPE { get; set; }
                public string BIN { get; set; }
                public Nullable<decimal> ISSUE_QTY { get; set; }
                public Nullable<decimal> COST { get; set; }
                public Nullable<decimal> SELL { get; set; }
                public string PURPOSE { get; set; }
                public string OFFICER { get; set; }
                public string ISSUEDTO { get; set; }
                public string COSTS { get; set; }
                public string RETURNED { get; set; }
                public Nullable<decimal> QTY_RETURN { get; set; }
                public Nullable<System.DateTime> RETURN_DAT { get; set; }
                public string RET_OFFICE { get; set; }
                public Nullable<System.DateTime> TRANS_DATE { get; set; }
                public Nullable<decimal> STOCK_BAL { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
                public Nullable<decimal> CUMRETURN { get; set; }
                public Nullable<decimal> ITEMNO { get; set; }
                public Nullable<System.DateTime> EXPIRYDATE { get; set; }
                public Nullable<decimal> WHSELL { get; set; }
                public string OPERATOR { get; set; }
                public Nullable<System.DateTime> DTIME { get; set; }
                public Nullable<decimal> U_SIZE { get; set; }
                public Nullable<decimal> FCCOST { get; set; }
                public Nullable<decimal> FCSELL { get; set; }
                public string CURRENCY { get; set; }
                public Nullable<bool> GENFIXEDASSETS { get; set; }
                public string APPROVALREF { get; set; }
                public string DESCRIPTION { get; set; }
                public string UNIT { get; set; }
                public string REQ_UNIT { get; set; }
                public Nullable<decimal> REQ_QTY { get; set; }
                public string RECTYPE { get; set; }
                public string BATCHNO { get; set; }
                public Nullable<System.DateTime> BATCHDATE { get; set; }
                public Nullable<bool> RESTOCK { get; set; }
            }
            public class ISSUES_PSN
            {
                public int RECID { get; set; }
                public string REFERENCE { get; set; }
                public string PRODUCT { get; set; }
                public string STORE { get; set; }
                public string PSN { get; set; }
                public string VENDOR_NO { get; set; }
                public Nullable<System.DateTime> TRANS_DATE { get; set; }
                public string ISSUEDTO { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
                public string OPERATOR { get; set; }
                public Nullable<System.DateTime> OP_DATE { get; set; }
                public Nullable<bool> STATUSNEW { get; set; }
                public Nullable<short> ITEMNO { get; set; }
            }
            public class ISSUES_WSN
            {
                public int RECID { get; set; }
                public string REFERENCE { get; set; }
                public string HEADERTXT { get; set; }
                public string FOOTERTXT { get; set; }
                public Nullable<System.DateTime> TRANS_DATE { get; set; }
                public string ISSUEDTO { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
                public string OPERATOR { get; set; }
                public Nullable<System.DateTime> OP_DATE { get; set; }
                public string TITLEHEADER1 { get; set; }
                public string TITLEHEADER2 { get; set; }
                public string SUBHEADER { get; set; }
                public string COSTCENTRE { get; set; }
            }
            public class LINK
            {
                public int RECID { get; set; }
                public string NAME { get; set; }
                public Nullable<System.DateTime> TRANS_DATE { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
                public string FRSECTION { get; set; }
                public string TIMESENT { get; set; }
                public string TOSECTION { get; set; }
                public Nullable<System.DateTime> DATEREC { get; set; }
                public string TIMEREC { get; set; }
                public string REFERENCE { get; set; }
                public string OPERATOR { get; set; }
                public string SECNAME { get; set; }
                public string COMMENTS { get; set; }
            }
            public class ORDERDET
            {




                #region Instance Properties

                public string ORDER_NO { get; set; }

                public string ITEM { get; set; }

                public decimal? ORDER_QTY { get; set; }

                public string NAME { get; set; }

                public decimal? COST { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? PENDING { get; set; }

                public decimal? REC_QTY { get; set; }

                public DateTime? REC_DATE { get; set; }





                #endregion Instance Properties
}
            public class ORDERS
            {




                #region Instance Properties

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

                public decimal? ORDERQTY { get; set; }

                public decimal? RECEVQTY { get; set; }

                public decimal? RTNQTY { get; set; }

                public DateTime? CANCELED { get; set; }

                public string COSTCENTRE { get; set; }

                public string REMARK { get; set; }





                #endregion Instance Properties
}
            public class PASSTAB
            {




                #region Instance Properties

                public string OPERATOR { get; set; }

                public string MODULE { get; set; }

                public string FUNTION { get; set; }

                public string RECORD { get; set; }

                public string RECTYPE { get; set; }

                public DateTime? TRANSDATE { get; set; }

                public string TRANSTIME { get; set; }

                public string TERMINAL { get; set; }





                #endregion Instance Properties
}
            public class PRICE
            {
                public int RECID { get; set; }
                public string STORE { get; set; }
                public string ITEM { get; set; }
                public string TYPE { get; set; }
                public string BIN { get; set; }
                public Nullable<System.DateTime> TRANS_DATE { get; set; }
                public Nullable<decimal> COST { get; set; }
                public Nullable<decimal> SELL { get; set; }
                public Nullable<decimal> STOCK_QTY { get; set; }
                public Nullable<decimal> NEW_COST { get; set; }
                public Nullable<decimal> NEW_SELL { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public string UNIT { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
                public Nullable<bool> APPLYDATE { get; set; }
                public Nullable<decimal> PREVPRICE { get; set; }
                public Nullable<System.DateTime> STARTDATE { get; set; }
                public Nullable<System.DateTime> ENDDATE { get; set; }
                public Nullable<decimal> WHSELL { get; set; }
                public string OPERATOR { get; set; }
                public Nullable<System.DateTime> DTIME { get; set; }
                public Nullable<decimal> NEW_WHSELL { get; set; }
            }
            public class prodbnd
            {
                public int RECID { get; set; }
                public string REFERENCE { get; set; }
                public string product { get; set; }
                public string store { get; set; }
                public Nullable<decimal> qty { get; set; }
                public string batchno { get; set; }
                public string vendor_no { get; set; }
                public Nullable<System.DateTime> receiveddate { get; set; }
                public Nullable<System.DateTime> issueddate { get; set; }
                public string issuedto { get; set; }
                public Nullable<bool> posted { get; set; }
                public Nullable<System.DateTime> post_date { get; set; }
                public string @operator { get; set; }
                public Nullable<System.DateTime> op_date { get; set; }
                public Nullable<bool> statusnew { get; set; }
                public string name { get; set; }
                public Nullable<decimal> receipt { get; set; }
                public Nullable<decimal> issues { get; set; }
                public Nullable<System.DateTime> expirydate { get; set; }
                public string type { get; set; }
            }
            public class RECEIPT
            {
                public int RECID { get; set; }
                public string REFERENCE { get; set; }
                public string ORDER_NO { get; set; }
                public Nullable<System.DateTime> TRANS_DATE { get; set; }
                public string STORE { get; set; }
                public string ITEM { get; set; }
                public Nullable<decimal> REC_QTY { get; set; }
                public Nullable<decimal> STOCK_BAL { get; set; }
                public string OFFICER { get; set; }
                public string RETURNED { get; set; }
                public string VENDOR_NO { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
                public string REC_OFFICE { get; set; }
                public Nullable<decimal> RET_QTY { get; set; }
                public Nullable<System.DateTime> RET_DATE { get; set; }
                public string RETOFFICE { get; set; }
                public string RETOFFICER { get; set; }
                public Nullable<decimal> ITEMNO { get; set; }
                public Nullable<decimal> CUMRETURN { get; set; }
                public Nullable<decimal> COST { get; set; }
                public Nullable<decimal> MISCCOST { get; set; }
                public string VATYPE { get; set; }
                public string RECTYPE { get; set; }
                public Nullable<decimal> U_SIZE { get; set; }
                public Nullable<System.DateTime> GRNDATE { get; set; }
                public Nullable<System.DateTime> EXPIRYDATE { get; set; }
                public Nullable<decimal> SELL { get; set; }
                public string REMARKS { get; set; }
                public Nullable<decimal> WHSELL { get; set; }
                public string OPERATOR { get; set; }
                public Nullable<System.DateTime> DTIME { get; set; }
                public Nullable<decimal> FCCOST { get; set; }
                public Nullable<decimal> FCSELL { get; set; }
                public string CURRENCY { get; set; }
                public string STATUS { get; set; }
                public Nullable<decimal> TONNAGE { get; set; }
                public string COSTCENTRE { get; set; }
                public string CLAGENT { get; set; }
                public string APPROVALREF { get; set; }
                public string UNIT { get; set; }
                public string DESCRIPTION { get; set; }
                public string REC_UNIT { get; set; }
                public Nullable<decimal> RECQTY1 { get; set; }
                public string BATCHNO { get; set; }
                public Nullable<System.DateTime> BATCHDATE { get; set; }
                public string TYPE { get; set; }
                public string BIN { get; set; }
                public string SUPPLIER { get; set; }
            }
            public class SCCOST
            {
                public int RECID { get; set; }
                public string STORE { get; set; }
                public string CATEGORY { get; set; }
                public Nullable<System.DateTime> EFF_DATE { get; set; }
                public Nullable<decimal> TRANSP_P { get; set; }
                public Nullable<decimal> TRANSP_A { get; set; }
                public Nullable<decimal> STORAGE_P { get; set; }
                public Nullable<decimal> STORAGE_A { get; set; }
                public Nullable<decimal> ADMIN_P { get; set; }
                public Nullable<decimal> ADMIN_A { get; set; }
                public Nullable<decimal> OTHERS_P { get; set; }
                public Nullable<decimal> OTHERS_A { get; set; }
                public Nullable<decimal> PROFIT_P { get; set; }
                public Nullable<decimal> PROFIT_A { get; set; }
                public Nullable<bool> POSTED { get; set; }
                public Nullable<System.DateTime> POST_DATE { get; set; }
                public Nullable<decimal> WSTRANSP_P { get; set; }
                public Nullable<decimal> WSTRANSP_A { get; set; }
                public Nullable<decimal> WSSTORAGE_P { get; set; }
                public Nullable<decimal> WSSTORAGE_A { get; set; }
                public Nullable<decimal> WSADMIN_P { get; set; }
                public Nullable<decimal> WSADMIN_A { get; set; }
                public Nullable<decimal> WSOTHERS_P { get; set; }
                public Nullable<decimal> WSOTHERS_A { get; set; }
                public Nullable<decimal> WSPROFIT_P { get; set; }
                public Nullable<decimal> WSPROFIT_A { get; set; }
                public string ITEM { get; set; }
                public Nullable<bool> P_MARKUP { get; set; }
            }
            public class SCSETUP
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

                public string ADDRESS8 { get; set; }

                public string INVNOTES { get; set; }

                public string CLREMARKS1 { get; set; }

                public string CLREMARKS2 { get; set; }

                public string BRANCH { get; set; }

                public bool? APPLYTOINV { get; set; }

                public string SLOGAN { get; set; }

                public string CLOGO { get; set; }





                #endregion Instance Properties
}
            public class SCSTLEV
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

                public bool? STOREKEEPER { get; set; }

                public bool? CANDELETE { get; set; }

                public bool? CANALTER { get; set; }

                public bool? CANADD { get; set; }

                public bool? MRP { get; set; }

                public bool? AC_COSTPR { get; set; }

                public decimal? HISTYEAR { get; set; }

                public decimal? ACCESSTYPE { get; set; }

                public string SWA1 { get; set; }

                public string SWA2 { get; set; }

                public string SWA3 { get; set; }

                public string SWA4 { get; set; }

                public string SWA5 { get; set; }

                public bool? REQ_AUTHORIZATION { get; set; }

                public bool? ISS_AUTHORIZATION { get; set; }

                public bool? SWAINCLUSIVE { get; set; }

                public bool? FILEMENU { get; set; }

                public bool? REPORTMENU { get; set; }

                public bool? PRODUCTION { get; set; }

                public string BRANCH { get; set; }





                #endregion Instance Properties
}
            public class SMCONTROL
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STATION { get; set; }

                public string NAME { get; set; }

                public bool? INSTALLED { get; set; }

                public int? CURYEAR { get; set; }

                public string STATE { get; set; }

                public string REGUSER { get; set; }

                public DateTime? TA_START { get; set; }

                public bool? TA_POST { get; set; }

                public DateTime? LAST_DATE { get; set; }

                public bool? TP_START { get; set; }

                public bool? TP_ENDED { get; set; }

                public DateTime? TP_DATE { get; set; }

                public Int16? TP_PERIOD { get; set; }

                public int? PYEAR { get; set; }

                public bool? TR_START { get; set; }

                public bool? TR_ENDED { get; set; }

                public DateTime? TR_DATE { get; set; }

                public bool? P_START { get; set; }

                public bool? P_ENDED { get; set; }

                public DateTime? P_DATE { get; set; }

                public bool? CD_START { get; set; }

                public bool? CD_ENDED { get; set; }

                public bool? POSTING { get; set; }

                public decimal? DELREC { get; set; }

                public string MPASS { get; set; }

                public DateTime? MPASSDT { get; set; }

                public bool? FSH { get; set; }

                public decimal? ENQNO { get; set; }

                public decimal? LAST_NO { get; set; }

                public bool? PAUTO { get; set; }

                public bool? CAUTO { get; set; }

                public decimal? CHARGNO { get; set; }

                public bool? ECGAUTO { get; set; }

                public decimal? ECGNO { get; set; }

                public string SERIAL { get; set; }

                public bool? PAYAUTO { get; set; }

                public decimal? PAYNO { get; set; }

                public bool? ADJAUTO { get; set; }

                public decimal? ADJNO { get; set; }

                public DateTime? XRDATE { get; set; }

                public bool? CENTRALMPA { get; set; }

                public bool? FESTLEVPAS { get; set; }

                public bool? GH_PAT { get; set; }

                public bool? PRODUCTION { get; set; }

                public bool? DS { get; set; }

                public bool? ALLOWEXP { get; set; }

                public bool? AUTOUPDATE { get; set; }

                public bool? FILEMODE { get; set; }

                public string VALUAMETH { get; set; }

                public decimal? ISSNO { get; set; }

                public bool? ISSAUTO { get; set; }

                public decimal? RECNO { get; set; }

                public bool? RECAUTO { get; set; }

                public string LOCALCUR { get; set; }

                public string LOCSTATE { get; set; }

                public string LOCCOUNTRY { get; set; }

                public string CUR_SIGN { get; set; }

                public bool? APDUPDATE { get; set; }

                public bool? PRICECHANG { get; set; }

                public bool? SECLINK { get; set; }

                public bool? AUTOCOUNT { get; set; }

                public decimal? AUTOCOUNTN { get; set; }

                public bool? AUTOORDER { get; set; }

                public decimal? AUTOORDERN { get; set; }

                public string CRPTPATH { get; set; }

                public string CFRMPATH { get; set; }

                public bool? CHANGESELL { get; set; }

                public bool? MULTISTORE { get; set; }

                public bool? GLINTERFAC { get; set; }

                public decimal? PLANPERIOD { get; set; }

                public string USEAVERAGE { get; set; }

                public decimal? LEADTIME { get; set; }

                public bool? FIFOBYCOST { get; set; }

                public bool? FIFOBYEXPD { get; set; }

                public bool? FIFOBYBATCH { get; set; }

                public int? REC_COUNT { get; set; }





                #endregion Instance Properties
}
            public class STK01
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string PRODUCT { get; set; }

                public string DESCRIPTN { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? QTY { get; set; }

                public string UNIT { get; set; }

                public string EXTDESC { get; set; }

                public string STORE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CUSTOMER { get; set; }

                public decimal? QTYCOLLECTED { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string NAME { get; set; }

                public string ADDRESS1 { get; set; }

                public string ADDRESS2 { get; set; }

                public DateTime? INV_DATE { get; set; }

                public decimal? COLLCOUNT { get; set; }





                #endregion Instance Properties
}
            public class STK02
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string PRODUCT { get; set; }

                public string DESCRIPTN { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? BALANCE { get; set; }

                public string UNIT { get; set; }

                public string XDESC { get; set; }

                public string STORE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CUSTOMER { get; set; }

                public decimal? QTYCOLLECTED { get; set; }

                public string ISSUEDBY { get; set; }

                public string DRIVERSNAME { get; set; }

                public string VEHICLENO { get; set; }

                public string COLLECTEDBY { get; set; }

                public string AUTHORISEDBY { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string APPROVAL2 { get; set; }





                #endregion Instance Properties
}
            public class STK03
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string PRODUCT { get; set; }

                public string DESCRIPTN { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? OLDBALANCE { get; set; }

                public decimal? NEWBALANCE { get; set; }

                public string UNIT { get; set; }

                public string XDESC { get; set; }

                public string STORE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CUSTOMER { get; set; }

                public decimal? OLDQTY { get; set; }

                public decimal? NEWQTY { get; set; }

                public string ISSUEDBY { get; set; }

                public string DRIVERSNAME { get; set; }

                public string VEHICLENO { get; set; }

                public string COLLECTEDBY { get; set; }

                public string AUTHORISEDBY { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string LOADINGREF { get; set; }





                #endregion Instance Properties
}
            public class STKCENTRE
            {




                #region Instance Properties

                public string REFERENCE { get; set; }

                public string DESCRIPTN { get; set; }

                public string RHEADER { get; set; }

                public string ACCT1 { get; set; }

                public string COLHEAD1 { get; set; }

                public string ACCT2 { get; set; }

                public string COLHEAD2 { get; set; }

                public string ACCT3 { get; set; }

                public string COLHEAD3 { get; set; }

                public string ACCT4 { get; set; }

                public string COLHEAD4 { get; set; }

                public string ACCT5 { get; set; }

                public string COLHEAD5 { get; set; }

                public string ACCT6 { get; set; }

                public string COLHEAD6 { get; set; }

                public string ACCT7 { get; set; }

                public string COLHEAD7 { get; set; }

                public string ACCT8 { get; set; }

                public string COLHEAD8 { get; set; }

                public string ACCT9 { get; set; }

                public string COLHEAD9 { get; set; }

                public string ACCT10 { get; set; }

                public string COLHEAD10 { get; set; }

                public string ACCT11 { get; set; }

                public string COLHEAD11 { get; set; }

                public string ACCT12 { get; set; }

                public string COLHEAD12 { get; set; }

                public string ACCT13 { get; set; }

                public string COLHEAD13 { get; set; }

                public string ACCT14 { get; set; }

                public string COLHEAD14 { get; set; }

                public string ACCT15 { get; set; }

                public string COLHEAD15 { get; set; }





                #endregion Instance Properties
}
            public class STKINF
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string TYPE { get; set; }

                public string ITEM { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string COMMENTS { get; set; }





                #endregion Instance Properties
}
            public class STKINT
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STORE { get; set; }

                public string ITEM { get; set; }

                public string TYPE { get; set; }

                public string PURPOSE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string DESCRIPTION { get; set; }





                #endregion Instance Properties
}

            public class StkRequest
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string FROMSTORE { get; set; }

                public string TOSTORE { get; set; }

                public string item { get; set; }

                public string name { get; set; }

                public DateTime? request_date { get; set; }

                public decimal? qtyrequired { get; set; }

                public string unitm { get; set; }

                public decimal? qtyavailable { get; set; }

                public decimal? qtysupplied { get; set; }

                public DateTime? supplydate { get; set; }

                //public string operator { get; set; }

                public DateTime? op_dtime { get; set; }

                public bool? posted { get; set; }

                public DateTime? post_date { get; set; }

                public string requeststaff { get; set; }

                public string supplyreference { get; set; }

                public decimal? qtyav_reqstore { get; set; }





                #endregion Instance Properties
}
            public class StkTAKE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STORE { get; set; }

                public string ITEM { get; set; }

                public string BIN { get; set; }

                public string TYPE { get; set; }

                public DateTime? LAST_STOCK { get; set; }

                public decimal? PHYSICAL { get; set; }

                public decimal? STOCK_QTY { get; set; }

                public decimal? COST { get; set; }

                public decimal? SELL { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string REF_NO { get; set; }

                public DateTime? NEXT_STOCK { get; set; }

                public string OFFICER { get; set; }

                public bool? POSTED { get; set; }

                public string NAME { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string UNIT { get; set; }

                public DateTime? EXPIRYDATE { get; set; }

                public decimal? WHSELL { get; set; }

                public string STATUS { get; set; }

                public decimal? FCCOST { get; set; }

                public decimal? FCSELL { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public decimal? RTLSACHET { get; set; }

                public decimal? TAB_CAPSALE { get; set; }





                #endregion Instance Properties
}
            public class stock
            {
                #region Instance Properties

                public int RECID { get; set; }

                public string store { get; set; }

                public string type { get; set; }

                public string item { get; set; }

                public string part { get; set; }

                public string name { get; set; }

                public string unit { get; set; }

                public string bin { get; set; }

                public decimal? cost { get; set; }

                public decimal? sell { get; set; }

                public decimal? stock_qty { get; set; }

                public decimal? committed_usage { get; set; }

                public decimal? safety { get; set; }

                public decimal? maximum { get; set; }

                public decimal? reorder_lv { get; set; }

                public decimal? reorder_qt { get; set; }

                public int? reorder_cy { get; set; }

                public DateTime? last_order { get; set; }

                public DateTime? next_order { get; set; }

                public decimal? order_qty { get; set; }

                public decimal? qty_pendin { get; set; }

                public int? stock_freq { get; set; }

                public DateTime? last_stock { get; set; }

                public DateTime? next_stock { get; set; }

                public decimal? out_sales { get; set; }

                public decimal? out_purch { get; set; }

                public decimal? ytd_sales { get; set; }

                public decimal? ytd_receip { get; set; }

                public decimal? ytd_issues { get; set; }

                public decimal? ytd_return { get; set; }

                public decimal? ytd_orders { get; set; }

                public decimal? ytd_loss { get; set; }

                public bool? ord_pend { get; set; }

                public DateTime? post_date { get; set; }

                public bool? posted { get; set; }

                public DateTime? trans_date { get; set; }

                public string vendor_no1 { get; set; }

                public string vendor_no2 { get; set; }

                public string vendor_no3 { get; set; }

                public string vendor_no4 { get; set; }

                public string vendor_no5 { get; set; }

                public string pack { get; set; }

                public decimal? packqty { get; set; }

                public decimal? totalpack { get; set; }

                public decimal? opening { get; set; }

                public bool? isfifo { get; set; }

                public string rectype { get; set; }

                public decimal? u_size { get; set; }

                public bool? flexwt { get; set; }

                public decimal? qfifo { get; set; }

                public decimal? sellunit { get; set; }

                public string selldesc { get; set; }

                public DateTime? expirydate { get; set; }

                public decimal? sellval { get; set; }

                public decimal? costval { get; set; }

                public string generic { get; set; }

                public string imagefile { get; set; }

                public decimal? whsell { get; set; }

                public DateTime? dtime { get; set; }

                public decimal? fccost { get; set; }

                public decimal? fcsell { get; set; }

                public string currency { get; set; }

                public string comments { get; set; }

                public string aimagefile { get; set; }

                public string status { get; set; }

                public string barcode { get; set; }

                public decimal? strength { get; set; }

                public decimal? per { get; set; }

                public string woperator { get; set; }

                public decimal? rtlsachet { get; set; }

                public decimal? packsellunit { get; set; }

                public decimal? tab_capsale { get; set; }

                public decimal? sachetqty { get; set; }





                #endregion Instance Properties
            }

            public class STORE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STORECODE { get; set; }

                public string NAME { get; set; }

                public string ADDRESS1 { get; set; }

                public string STATE { get; set; }

                public string MANAGER { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string COMPANY { get; set; }

                public string storetype { get; set; }

                public string BRANCH { get; set; }





                #endregion Instance Properties
}
            public class TECHSTOCK
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STORE { get; set; }

                public string TYPE { get; set; }

                public string ITEM { get; set; }

                public string PART1 { get; set; }

                public string PART2 { get; set; }

                public decimal? COST { get; set; }

                public decimal? SELL { get; set; }

                public decimal? STOCK_QTY { get; set; }

                public decimal? COMMITTED { get; set; }

                public decimal? SAFETY { get; set; }

                public decimal? MAXIMUM { get; set; }

                public decimal? REORDER_LV { get; set; }

                public decimal? REORDER_QT { get; set; }

                public decimal? REORDER_CY { get; set; }

                public DateTime? LAST_ORDER { get; set; }

                public DateTime? NEXT_ORDER { get; set; }

                public decimal? ORDER_QTY { get; set; }

                public decimal? QTY_PENDIN { get; set; }

                public decimal? STOCK_FREQ { get; set; }

                public DateTime? LAST_STOCK { get; set; }

                public DateTime? NEXT_STOCK { get; set; }

                public decimal? OUT_SALES { get; set; }

                public decimal? OUT_PURCH { get; set; }

                public decimal? YTD_SALES { get; set; }

                public decimal? YTD_RECEIP { get; set; }

                public decimal? YTD_ISSUES { get; set; }

                public decimal? YTD_RETURN { get; set; }

                public decimal? YTD_ORDERS { get; set; }

                public decimal? YTD_LOSS { get; set; }

                public bool? ORD_PEND { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string VENDOR_NO1 { get; set; }

                public string VENDOR_NO2 { get; set; }

                public string VENDOR_NO3 { get; set; }

                public string VENDOR_NO4 { get; set; }

                public string VENDOR_NO5 { get; set; }

                public string PACK { get; set; }

                public decimal? PACKQTY { get; set; }

                public string RECTYPE { get; set; }

                public decimal? U_SIZE { get; set; }

                public bool? FLEXWT { get; set; }

                public DateTime? EXPIRYDATE { get; set; }

                public string GENERIC { get; set; }

                public string IMAGEFILE { get; set; }

                public decimal? WHSELL { get; set; }

                public DateTime? DTIME { get; set; }

                public string CONFORMITY { get; set; }

                public string OWNERSHIP { get; set; }

                public string EFFECTIVITY { get; set; }

                public string ATACHAPTER { get; set; }

                public int? STATUSNEW { get; set; }

                public int? STATUSOLD { get; set; }





                #endregion Instance Properties
}
            public class TRANSMAS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TRANSTYPE { get; set; }

                public string STORE { get; set; }

                public string ITEM { get; set; }

                public string DESCRIPT { get; set; }

                public decimal? TRANS_QTY { get; set; }

                public decimal? STOCK_BAL { get; set; }

                public decimal? COST { get; set; }

                public decimal? SELL { get; set; }

                public bool? POSTED { get; set; }

                public string BIN { get; set; }

                public string TYPE { get; set; }

                public string UNIT { get; set; }

                public string RECTYPE { get; set; }

                public decimal? U_SIZE { get; set; }

                public decimal? COSTVAL { get; set; }

                public decimal? SELLVAL { get; set; }

                public decimal? WHSELL { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public decimal? FCCOST { get; set; }

                public decimal? FCSELL { get; set; }

                public string CURRENCY { get; set; }

                public string STATUS { get; set; }

                public string PURPOSE { get; set; }

                public string APPROVALREF { get; set; }





                #endregion Instance Properties
}
            public class VENDORS
            {




                #region Instance Properties

                public int RECID { get; set; }

                //public string VENDORS { get; set; }

                public string NAME { get; set; }

                public string ADDRESS1 { get; set; }

                public string STATE { get; set; }

                public string CONTACT { get; set; }

                public DateTime? ORDER_DATE { get; set; }

                public decimal? ORDER_PEND { get; set; }

                public decimal? ALL_ORDERS { get; set; }

                public decimal? ORDERS_YTD { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? POSTED { get; set; }

                public string COUNTRY { get; set; }

                public string PHONE { get; set; }

                public DateTime? DATE_REG { get; set; }

                public string EMAIL { get; set; }





                #endregion Instance Properties
}
        
            public class SCS01vm
            {
                public associateditem associateditem { get; set; }
                public COMPANY COMPANY { get; set; }
                public COST COST { get; set; }
                public DRUGSLIP DRUGSLIP { get; set; }
                public FIFO FIFO { get; set; }
                public GLINT GLINT { get; set; }
                public GLUPDATE GLUPDATE { get; set; }
                public ISS_ADDRESS ISS_ADDRESS { get; set; }
                public ISSUE ISSUE { get; set; }
                public ISSUES_PSN ISSUES_PSN { get; set; }
                public ISSUES_WSN ISSUES_WSN { get; set; }
                public LINK LINK { get; set; }
                public ORDERDET ORDERDET { get; set; }
                public ORDERS ORDERS { get; set; }
                public PASSTAB PASSTAB { get; set; }
                public PRICE PRICE { get; set; }
                public prodbnd prodbnd { get; set; }
                public RECEIPT RECEIPT { get; set; }
                public SCCOST SCCOST { get; set; }
                public SCSETUP SCSETUP { get; set; }
                public SCSTLEV SCSTLEV { get; set; }
                public SMCONTROL SMCONTROL { get; set; }
                public STK01 STK01 { get; set; }
                public STK02 STK02 { get; set; }
                public STK03 STK03 { get; set; }
                public STKCENTRE STKCENTRE { get; set; }
                public STKINF STKINF { get; set; }
                public STKINT STKINT { get; set; }
                public StkRequest StkRequest { get; set; }
                public StkTAKE StkTAKE { get; set; }
                public stock stock { get; set; }
                public STORE STORE { get; set; }
                public TECHSTOCK TECHSTOCK { get; set; }
                public TRANSMAS TRANSMAS { get; set; }
                public VENDORS VENDORS { get; set; }



                public IEnumerable<associateditem> associateditems { get; set; }
                public IEnumerable<COMPANY> COMPANYs { get; set; }
                public IEnumerable<COST> COSTs { get; set; }
                public IEnumerable<DRUGSLIP> DRUGSLIPs { get; set; }
                public IEnumerable<FIFO> FIFOs { get; set; }
                public IEnumerable<GLINT> GLINTs { get; set; }
                public IEnumerable<GLUPDATE> GLUPDATEs { get; set; }
                public IEnumerable<ISS_ADDRESS> ISS_ADDRESSs { get; set; }
                public IEnumerable<ISSUE> ISSUEs { get; set; }
                public IEnumerable<ISSUES_PSN> ISSUES_PSNs { get; set; }
                public IEnumerable<ISSUES_WSN> ISSUES_WSNs { get; set; }
                public IEnumerable<LINK> LINKs { get; set; }
                public IEnumerable<ORDERDET> ORDERDETs { get; set; }
                public IEnumerable<ORDERS> ORDERSs { get; set; }
                public IEnumerable<PASSTAB> PASSTABs { get; set; }
                public IEnumerable<PRICE> PRICEs { get; set; }
                public IEnumerable<prodbnd> prodbnds { get; set; }
                public IEnumerable<RECEIPT> RECEIPTs { get; set; }
                public IEnumerable<SCCOST> SCCOSTs { get; set; }
                public IEnumerable<SCSETUP> SCSETUPs { get; set; }
                public IEnumerable<SCSTLEV> SCSTLEVs { get; set; }
                public IEnumerable<SMCONTROL> SMCONTROLs { get; set; }
                public IEnumerable<STK01> STK01s { get; set; }
                public IEnumerable<STK02> STK02s { get; set; }
                public IEnumerable<STK03> STK03s { get; set; }
                public IEnumerable<STKCENTRE> STKCENTREs { get; set; }
                public IEnumerable<STKINF> STKINFs { get; set; }
                public IEnumerable<STKINT> STKINTs { get; set; }
                public IEnumerable<StkRequest> StkRequests { get; set; }
                public IEnumerable<StkTAKE> StkTAKEs { get; set; }
                public IEnumerable<stock> stocks { get; set; }
                public IEnumerable<STORE> STOREs { get; set; }
                public IEnumerable<TECHSTOCK> TECHSTOCKs { get; set; }
                public IEnumerable<TRANSMAS> TRANSMASs { get; set; }
                public IEnumerable<VENDORS> VENDORSs { get; set; }

                public HP_DATA.HP_DATAvm HP_DATAvm { get; set; }
                public APS01.APS01vm APS01vm { get; set; }
                public FAS01.FAS01vm FAS01vm { get; set; }
                public GLS01.GLS01vm GLS01vm { get; set; }
                public MR_DATA.MR_DATAvm MR_DATAvm { get; set; }
                public PAYPER.PAYPERvm PAYPERvm { get; set; }
                public AR_DATA.AR_DATAvm ARvm { get; set; }
                public SYSCODETABS.SYSCODETABSvm SYSCODETABSvm { get; set; }
        }
        
    }
}