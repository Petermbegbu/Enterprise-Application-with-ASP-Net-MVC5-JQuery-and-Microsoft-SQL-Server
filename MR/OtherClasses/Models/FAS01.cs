using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OtherClasses.Models
{
    public class FAS01
    {
        
            public class ADEP
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string ASSETNO { get; set; }

                public decimal? VALUE { get; set; }

                public string DEPR_METH { get; set; }

                public decimal? DEPR_AMT { get; set; }

                public decimal? DEPR_TD { get; set; }

                public decimal? DEPR_YTD { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public decimal? DEPR_RATE { get; set; }

                public string DEP_ACCNO { get; set; }

                public string PAL_ACCNO { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string ASSETTYPE { get; set; }





                #endregion Instance Properties
}
            public class ADF
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string ASSETNO { get; set; }

                public string NAME { get; set; }

                public decimal? DISP_VAL { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string REFERENCE { get; set; }

                public string RECEIVER { get; set; }

                public string REC_ADDRESS { get; set; }

                public string CONTACT { get; set; }

                public string PHONE { get; set; }

                public string FAX { get; set; }

                public DateTime? PUR_DATE { get; set; }

                public decimal? COST { get; set; }

                public decimal? VALUE { get; set; }

                public decimal? LIFE_SPAN { get; set; }

                public decimal? CUR_YEAR { get; set; }

                public decimal? RESIDUAL { get; set; }

                public decimal? MAINT_TD { get; set; }

                public decimal? MAINT_YTD { get; set; }

                public decimal? DEPR_TD { get; set; }

                public decimal? DEPR_YTD { get; set; }

                public decimal? ADJ_TD { get; set; }

                public decimal? ADJ_YTD { get; set; }

                public decimal? PREM_TD { get; set; }

                public decimal? PREM_YTD { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class AHIST
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CODE { get; set; }

                public string ASSETNO { get; set; }

                public string ASTYPE { get; set; }

                public string REFERENCE { get; set; }

                public string ASSTATUS { get; set; }

                public string STAT_DESC { get; set; }

                public string PUR_ORDNO { get; set; }

                public DateTime? PUR_DATE { get; set; }

                public string LOC_CODE { get; set; }

                public string LOC_NAME { get; set; }

                public string STAFF_NO { get; set; }

                public string SA_NAME { get; set; }

                public decimal? COST { get; set; }

                public decimal? VALUE { get; set; }

                public decimal? PRE_VALUE { get; set; }

                public DateTime? VAL_DATE { get; set; }

                public string COMP_CODE { get; set; }

                public string COMP_NAME { get; set; }

                public string ADDRESS1 { get; set; }

                public string ADDRESS2 { get; set; }

                public string CITY { get; set; }

                public string ASSTATE { get; set; }

                public string BRANCH { get; set; }

                public string REG_NO { get; set; }

                public string TAX_REF { get; set; }

                public decimal? CUR_YEAR { get; set; }

                public decimal? CUR_MONTH { get; set; }

                public string SERIAL_NO { get; set; }

                public string MODEL { get; set; }

                public decimal? YEAR_MANU { get; set; }

                public string VENDOR { get; set; }

                public string INSURA { get; set; }

                public string ASS_ACNO { get; set; }

                public decimal? DEPR_RATE { get; set; }

                public decimal? DEPR_YTD { get; set; }

                public decimal? DEPR_TD { get; set; }

                public decimal? DEP_TD { get; set; }

                public decimal? DEP_YTD { get; set; }

                public decimal? MAINT_TD { get; set; }

                public decimal? MAINT_YTD { get; set; }

                public decimal? ADJ_TD { get; set; }

                public decimal? ADJ_YTD { get; set; }

                public DateTime? DEP_DATE { get; set; }

                public DateTime? FVAL_DATE { get; set; }

                public decimal? PR_VALUE { get; set; }

                public decimal? LIFE_SPAN { get; set; }

                public decimal? RESIDUAL { get; set; }

                public string ASS_DESCR { get; set; }

                public string DEP_MTH { get; set; }

                public DateTime? L_DDATE { get; set; }

                public decimal? AC_YR_ST { get; set; }

                public decimal? NYR { get; set; }

                public decimal? BALANCE { get; set; }

                public string CODE_DESC { get; set; }

                public decimal? PERIOD { get; set; }

                public DateTime? EXPIR { get; set; }

                public decimal? YEAR_NO { get; set; }

                public decimal? NYEAR { get; set; }

                public decimal? PREM_YTD { get; set; }

                public decimal? PREM_TD { get; set; }

                public decimal? INS_VALUE { get; set; }





                #endregion Instance Properties
}
            public class AINS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string ASSETNO { get; set; }

                public string NAME { get; set; }

                public string LOCATION { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string REFERENCE { get; set; }

                public string REMARKS { get; set; }

                public string ASSTATUS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string INSPECTOR { get; set; }





                #endregion Instance Properties
}
            public class AINSPREM
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string ASSETNO { get; set; }

                public string NAME { get; set; }

                public string INSURANCE { get; set; }

                public decimal? AMOUNT { get; set; }

                public string PAYMODE { get; set; }

                public string DETAIL { get; set; }

                public DateTime? PSTART { get; set; }

                public DateTime? PEND { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? INSVALUE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string INSCLASS { get; set; }

                public string INSPAYINT { get; set; }

                public decimal? PREMIUM { get; set; }





                #endregion Instance Properties
}
            public class AMAINT
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string ASSETNO { get; set; }

                public string NAME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TRANSCODE { get; set; }

                public decimal? AMOUNT { get; set; }

                public decimal? ITEMNO { get; set; }

                public string MAINTDESC { get; set; }

                public string GLDEBIT { get; set; }

                public string GLCREDIT { get; set; }

                public string MAINTTYPE { get; set; }





                #endregion Instance Properties
}
            public class ASLOG
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string ASSETNO { get; set; }

                public string NAME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TRANSCODE { get; set; }

                public decimal? AMOUNT { get; set; }

                public decimal? ITEMNO { get; set; }

                public string DESTINATION { get; set; }

                public string STARTTIME { get; set; }

                public string ENDTIME { get; set; }

                public DateTime? ENDDATE { get; set; }

                public string DRIVER { get; set; }

                public decimal? USAGE { get; set; }





                #endregion Instance Properties
}
            public class ASSETS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string ASSTATUS { get; set; }

                public string NAME { get; set; }

                public string CATEGORY { get; set; }

                public string PUR_ORDNO { get; set; }

                public DateTime? PUR_DATE { get; set; }

                public string LOCATION { get; set; }

                public string AKEEPER { get; set; }

                public decimal? COST { get; set; }

                public decimal? VALUE { get; set; }

                public decimal? INIT_VAL { get; set; }

                public DateTime? INIT_VALDA { get; set; }

                public DateTime? LAST_VALDA { get; set; }

                public string COMPANY { get; set; }

                public string BRANCH { get; set; }

                public string SERIALNO { get; set; }

                public string MODEL { get; set; }

                public decimal? YEAR_MANU { get; set; }

                public string VENDOR { get; set; }

                public string INSURA { get; set; }

                public string ACCOUNTNO { get; set; }

                public decimal? DEPR_RATE { get; set; }

                public decimal? DEPR_YTD { get; set; }

                public decimal? DEPR_TD { get; set; }

                public string DEPR_MTH { get; set; }

                public decimal? MAINT_TD { get; set; }

                public decimal? MAINT_YTD { get; set; }

                public decimal? ADJ_TD { get; set; }

                public decimal? ADJ_YTD { get; set; }

                public decimal? LIFE_SPAN { get; set; }

                public decimal? RESIDUAL { get; set; }

                public string L_DDATE { get; set; }

                public DateTime? DEPR_STAT { get; set; }

                public decimal? YEAR_NO { get; set; }

                public decimal? DEPR_AMT { get; set; }

                public decimal? PREM_YTD { get; set; }

                public decimal? PREM_TD { get; set; }

                public decimal? INSVALUE { get; set; }

                public DateTime? INSPAYSTAT { get; set; }

                public string INSPAYINT { get; set; }

                public DateTime? LASTINSPAY { get; set; }

                public DateTime? NEXTDUE { get; set; }

                public string INSCLASS { get; set; }

                public decimal? PREMIUM { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? LDEPR_AMT { get; set; }

                public DateTime? REG_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string PUR_RECPT { get; set; }

                public string DEPRINT { get; set; }

                public DateTime? LASTINSPEC { get; set; }

                public string POLICYNO { get; set; }

                public string ENGINENO { get; set; }

                public string CHASSISNO { get; set; }

                public decimal? CUR_DEPR_P { get; set; }

                public decimal? CUR_DEPR_Y { get; set; }

                public string COMMENTS { get; set; }

                public string AIMAGEFILE { get; set; }

                public string PALACCTNO { get; set; }

                public string BSACCTNO { get; set; }

                public decimal? CUR_DEPR_Y_NO { get; set; }

                public decimal? MAINT_INTERVAL { get; set; }

                public string MAINT_TAG { get; set; }

                public DateTime? LASTMAINTAINED { get; set; }

                public decimal? CURRENTUSAGE { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? LASTLOGDATE { get; set; }

                public string MAINT_STAFF { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class ASSNUMFMT
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string ASSETTYPE { get; set; }

                public string ASDESCRIPTION { get; set; }

                public string PREFIX { get; set; }

                public decimal? ITEMNO { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_DATE { get; set; }

                public decimal? DEPR_RATE { get; set; }

                public string DEPR_ACC { get; set; }

                public string DEPR_PAL { get; set; }

                public decimal? DEPR_AMOUNT { get; set; }





                #endregion Instance Properties
}
            public class ATF
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string ASSETNO { get; set; }

                public string NAME { get; set; }

                public string REFERENCE { get; set; }

                public string NEWASSETNO { get; set; }

                public string ASSTATUS { get; set; }

                public string OLDLOCATIO { get; set; }

                public string NEWLOCATIO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string NEWCOMP { get; set; }

                public string OLDCOMP { get; set; }

                public string NEWBRANCH { get; set; }

                public string OLDBRANCH { get; set; }

                public decimal? COST { get; set; }

                public decimal? VALUE { get; set; }

                public decimal? PREM_TD { get; set; }

                public decimal? DEPR_TD { get; set; }

                public decimal? MAINT_TD { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class AVAL
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string ASSETNO { get; set; }

                public string NAME { get; set; }

                public string CATEGORY { get; set; }

                public DateTime? PUR_DATE { get; set; }

                public string LOCATION { get; set; }

                public decimal? VALUE { get; set; }

                public DateTime? LAST_VALDA { get; set; }

                public string COMPANY { get; set; }

                public decimal? DEPR_RATE { get; set; }

                public decimal? DEPR_YTD { get; set; }

                public decimal? DEPR_TD { get; set; }

                public decimal? MAINT_TD { get; set; }

                public decimal? MAINT_YTD { get; set; }

                public decimal? ADJ_TD { get; set; }

                public decimal? ADJ_YTD { get; set; }

                public decimal? LIFE_SPAN { get; set; }

                public decimal? RESIDUAL { get; set; }

                public string DEPR_MTH { get; set; }

                public DateTime? L_DDATE { get; set; }

                public DateTime? DEPR_STAT { get; set; }

                public decimal? PREM_YTD { get; set; }

                public decimal? PREM_TD { get; set; }

                public decimal? INS_VALUE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? LDEPR_AMT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string DEPRINT { get; set; }

                public decimal? PERCENTAGE { get; set; }

                public decimal? VALAMOUNT { get; set; }

                public decimal? NEWVALUE { get; set; }

                public string APPLYGLOB { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public string NDEPR_MTH { get; set; }

                public decimal? NLIFE_SPAN { get; set; }

                public decimal? NRESIDUAL { get; set; }

                public DateTime? NDEPR_STAT { get; set; }

                public decimal? NDEPR_RATE { get; set; }

                public decimal? NDEPR_TD { get; set; }

                public decimal? NDEPR_YTD { get; set; }

                public decimal? YEAR_NO { get; set; }

                public decimal? NLDEPR_AMT { get; set; }

                public decimal? FIRSTSALESVAL { get; set; }





                #endregion Instance Properties
}
            public class COMPANY
            {




                #region Instance Properties

                public int RECID { get; set; }

                //public string COMPANY { get; set; }

                public string NAME { get; set; }

                public string SHORTNAME { get; set; }

                public string STREET { get; set; }

                public string BOX { get; set; }

                public string CITY { get; set; }

                public string CO_STATE { get; set; }

                public string TAX_NO { get; set; }

                public string REG_NO { get; set; }

                public string BRANCH { get; set; }

                public decimal? ASYEAR { get; set; }

                public decimal? ASMONTH { get; set; }

                public DateTime? TP_DATE { get; set; }

                public decimal? BUDGET { get; set; }

                public decimal? ACCRUAL { get; set; }

                public decimal? STAFF { get; set; }

                public decimal? BUDGET_AMT { get; set; }

                public decimal? ACTUAL_AMT { get; set; }

                public decimal? UNION_AMT { get; set; }

                public decimal? UNION_PERC { get; set; }

                public decimal? MID_MON { get; set; }

                public decimal? STAFF_CUM { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string BIGNAME { get; set; }

                public bool? GRADE { get; set; }

                public decimal? MIDAMOUNT { get; set; }

                public string NSITFNO { get; set; }





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

                public string ADDRESS2 { get; set; }

                public string ZONE { get; set; }

                public string CUSSTATE { get; set; }

                public string COUNTRY { get; set; }

                public string PHONE { get; set; }

                public decimal? CUR_DB { get; set; }

                public decimal? CUR_CR { get; set; }

                public string ACCOUNTNO { get; set; }

                public DateTime? LASTSTATMT { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? DATE_REG { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CONTACT { get; set; }

                public string FAX { get; set; }

                public decimal? BALBF { get; set; }

                public string EMAIL { get; set; }

                public string AGENT { get; set; }





                #endregion Instance Properties
}
            public class FACONTROL
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STATION { get; set; }

                public string NAME { get; set; }

                public bool? INSTALLED { get; set; }

                public decimal? BENO { get; set; }

                public decimal? RELEASENO { get; set; }

                public bool? BERESET { get; set; }

                public string USER_STATE { get; set; }

                public string ASUSER { get; set; }

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

                public bool? XRAUTO { get; set; }

                public decimal? XRAYNO { get; set; }

                public bool? ADJAUTO { get; set; }

                public decimal? ADJNO { get; set; }

                public DateTime? XRDATE { get; set; }

                public bool? CENTRALMPA { get; set; }

                public DateTime? MSDUTYRDAT { get; set; }

                public bool? FESTLEVPAS { get; set; }

                public bool? FILEMODE { get; set; }

                public string CRPTPATH { get; set; }

                public string CFRMPATH { get; set; }

                public decimal? GLBATCH { get; set; }

                public string GLJVNO { get; set; }

                public bool? GLINTENABL { get; set; }

                public string GLCOMPANY { get; set; }

                public string LOCSTATE { get; set; }

                public string LOCCOUNTRY { get; set; }

                public string BRANCH { get; set; }

                public string COMPANY { get; set; }

                public string LOCALCUR { get; set; }

                public string CUR_SIGN { get; set; }

                public string NUMFMT { get; set; }





                #endregion Instance Properties
}
            public class FADDESC
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string NAME { get; set; }

                public string CATEGORY { get; set; }

                public string ASSETSACCTNO { get; set; }

                public string LIABILITYACCTNO { get; set; }

                public decimal? LIFE_SPAN { get; set; }

                public decimal? RESIDUAL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string DEPRINT { get; set; }

                public string COMMENTS { get; set; }

                public string AIMAGEFILE { get; set; }

                public string PALACCTNO { get; set; }

                public string BSACCTNO { get; set; }

                public decimal? MAINT_INTERVAL { get; set; }

                public string MAINT_TAG { get; set; }

                public string DEPR_MTH { get; set; }





                #endregion Instance Properties
}
            public class FASTLEV
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

                public bool? UM1 { get; set; }

                public bool? UM2 { get; set; }

                public bool? UM3 { get; set; }

                public bool? UM4 { get; set; }

                public bool? UM5 { get; set; }

                public bool? UM6 { get; set; }

                public bool? UM7 { get; set; }

                public bool? UM8 { get; set; }

                public bool? UM9 { get; set; }

                public decimal? WSECLEVEL { get; set; }

                public DateTime? PASSDATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string INITIAL { get; set; }

                public bool? CANDELETE { get; set; }

                public bool? CANALTER { get; set; }

                public bool? CANADD { get; set; }

                public decimal? HISTYEAR { get; set; }

                public bool? FM15 { get; set; }

                public bool? FM16 { get; set; }

                public bool? FM17 { get; set; }

                public bool? FM18 { get; set; }

                public bool? FM19 { get; set; }

                public bool? FM20 { get; set; }

                public bool? RM16 { get; set; }

                public bool? RM17 { get; set; }

                public bool? RM18 { get; set; }

                public bool? RM19 { get; set; }

                public bool? RM20 { get; set; }

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

                public bool? FILEMENU { get; set; }

                public bool? REPORTMENU { get; set; }

                public string BRANCH { get; set; }





                #endregion Instance Properties
}




            public class FAS01vm
            {
                public ADEP ADEP { get; set; }
                public ADF ADF { get; set; }
                public AHIST AHIST { get; set; }
                public AINS AINS { get; set; }
                public AINSPREM AINSPREM { get; set; }
                public AMAINT AMAINT { get; set; }
                public ASLOG ASLOG { get; set; }
                public ASSETS ASSETS { get; set; }
                public ASSNUMFMT ASSNUMFMT { get; set; }
                public ATF ATF { get; set; }
                public AVAL AVAL { get; set; }
                public COMPANY COMPANY { get; set; }
                public CUSTOMER CUSTOMER { get; set; }
                public FACONTROL FACONTROL { get; set; }
                public FADDESC FADDESC { get; set; }
                public FASTLEV FASTLEV { get; set; }



                public IEnumerable<ADEP> ADEPS { get; set; }
                public IEnumerable<ADF> ADFS { get; set; }
                public IEnumerable<AHIST> AHISTS { get; set; }
                public IEnumerable<AINS> AINSS { get; set; }
                public IEnumerable<AINSPREM> AINSPREMS { get; set; }
                public IEnumerable<AMAINT> AMAINTS { get; set; }
                public IEnumerable<ASLOG> ASLOGS { get; set; }
                public IEnumerable<ASSETS> ASSETSS { get; set; }
                public IEnumerable<ASSNUMFMT> ASSNUMFMTS { get; set; }
                public IEnumerable<ATF> ATFS { get; set; }
                public IEnumerable<AVAL> AVALS { get; set; }
                public IEnumerable<COMPANY> COMPANYS { get; set; }
                public IEnumerable<CUSTOMER> CUSTOMERS { get; set; }
                public IEnumerable<FACONTROL> FACONTROLS { get; set; }
                public IEnumerable<FADDESC> FADDESCS { get; set; }
                public IEnumerable<FASTLEV> FASTLEVS { get; set; }














            }





    
    }
}