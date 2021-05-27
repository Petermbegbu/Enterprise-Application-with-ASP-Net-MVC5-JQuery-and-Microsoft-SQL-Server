using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OtherClasses.Models
{
    public class PAYPER
    {
        
            public class BANKS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string BANK { get; set; }

                public string NAME { get; set; }

                public string ADDRESS_1 { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string BRANCH { get; set; }

                public string STATE { get; set; }

                public string COACCTYP { get; set; }

                public string COACCNO { get; set; }

                public string AUTHSIG1 { get; set; }

                public string AUTHSIG2 { get; set; }

                public string HEADERTEXT { get; set; }

                public string FOOTERTEXT { get; set; }





                #endregion Instance Properties
}
            public class COMPANY
            {




                #region Instance Properties

                public int RECID { get; set; }

                //public string COMPANY { get; set; }

                public string NAME { get; set; }

                public string STREET { get; set; }

                public string BOX { get; set; }

                public string CITY { get; set; }

                public string STATE { get; set; }

                public string TAX_NO { get; set; }

                public string REG_NO { get; set; }

                public string BRANCH { get; set; }

                public decimal? PPYEAR { get; set; }

                public decimal? PPMONTH { get; set; }

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

                public decimal? LEAVEPAY { get; set; }

                public string SHORTNAME { get; set; }





                #endregion Instance Properties
}
            public class COMTAB
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string CODETYPE { get; set; }

                public string CODE { get; set; }

                public string COLDESC { get; set; }





                #endregion Instance Properties
}
            public class COSTS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CENTER { get; set; }

                public string NAME { get; set; }

                public decimal? BUDGET_AMT { get; set; }

                public decimal? ACCRU_AMT { get; set; }

                public decimal? BUDGET_FIG { get; set; }

                public decimal? ACCRU_FIG { get; set; }

                public decimal? CUR_YEAR { get; set; }

                public decimal? UPD_MONTH { get; set; }

                public string GL_NO { get; set; }

                public decimal? OT_LIMIT { get; set; }

                public string DEPARTMENT { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class CUSTOMER
            {




                #region Instance Properties

                public int recid { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string CATEGORY { get; set; }

                public string ADDRESS1 { get; set; }

                public string ZONE { get; set; }

                public string STATE { get; set; }

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

                public string STATUS { get; set; }

                public string BANK { get; set; }





                #endregion Instance Properties
}
            public class DEDTAB
            {




                #region Instance Properties

                public int recid { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string CODETYPE { get; set; }

                public string CODE { get; set; }

                public string COLDESC { get; set; }





                #endregion Instance Properties
}
            public class DEPT
            {




                #region Instance Properties

                public int recid { get; set; }

                public string DEPARTMENT { get; set; }

                public string NAME { get; set; }

                public string STAFF_NO { get; set; }

                public string OPERATION { get; set; }

                public decimal? BUDGETED { get; set; }

                public decimal? ACTUAL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class EATAB
            {




                #region Instance Properties

                public int recid { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string CODETYPE { get; set; }

                public string CODE { get; set; }

                public string COLDESC { get; set; }





                #endregion Instance Properties
}
            public class EQUALOPT
            {




                #region Instance Properties

                public int recid { get; set; }

                public string REFERENCE { get; set; }

                public string APPREF { get; set; }

                public string POSITION { get; set; }

                public string NAME { get; set; }

                public string DEPARTMENT { get; set; }

                public string WHITE { get; set; }

                public string BLACK_BRITISH { get; set; }

                public string ASIAN_BRITISH_ASIAN { get; set; }

                public string MIXEDRACE { get; set; }

                public string CHINESE_OTHERS_SPECIFIED { get; set; }

                public string NATIONALITY { get; set; }

                public string REGION { get; set; }

                public string GENDER { get; set; }

                public string AGE_GROUP { get; set; }

                public string RELIGION { get; set; }

                public string DISABILITYSTATUS { get; set; }

                public string DISABILITY { get; set; }

                public string NOTES { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class EXECHORI
            {




                #region Instance Properties

                public int recid { get; set; }

                public string REFERENCE { get; set; }

                public string DESCRIPTN { get; set; }

                public decimal? HORIZ_HD_TOTAL { get; set; }

                public string HEADER1 { get; set; }

                public string HEADER2 { get; set; }

                public string HEADER3 { get; set; }

                public string HEADER4 { get; set; }

                public string HEADER5 { get; set; }

                public string HEADER6 { get; set; }





                #endregion Instance Properties
}
            public class EXECSSHD
            {




                #region Instance Properties

                public int recid { get; set; }

                public string REFERENCE { get; set; }

                public string HEADING { get; set; }

                public string TYPE { get; set; }

                public string GROUPCODE { get; set; }

                public decimal? GRPTOTAL { get; set; }

                public string GRPCODE1 { get; set; }

                public string GRPDESC1 { get; set; }

                public string GRPCODE2 { get; set; }

                public string GRPDESC2 { get; set; }

                public string GRPCODE3 { get; set; }

                public string GRPDESC3 { get; set; }

                public string GRPCODE4 { get; set; }

                public string GRPDESC4 { get; set; }

                public string GRPCODE5 { get; set; }

                public string GRPDESC5 { get; set; }

                public string GRPCODE6 { get; set; }

                public string GRPDESC6 { get; set; }

                public string GRPCODE7 { get; set; }

                public string GRPDESC7 { get; set; }

                public string GRPCODE8 { get; set; }

                public string GRPDESC8 { get; set; }

                public string GRPCODE9 { get; set; }

                public string GRPDESC9 { get; set; }

                public string GRPCODE10 { get; set; }

                public string GRPDESC10 { get; set; }





                #endregion Instance Properties
}
            public class GLINT
            {




                #region Instance Properties

                public int recid { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string CURRENCY { get; set; }

                public string COMPANY { get; set; }

                public decimal? BATCHNO { get; set; }

                public string DOCUMENT { get; set; }

                public string INTERFACEOPTION { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class GLINTDET
            {




                #region Instance Properties

                public int recid { get; set; }

                public string REFERENCE { get; set; }

                public string ADJUST { get; set; }

                public string DESCRIPTN { get; set; }

                public string DEBITACCT { get; set; }

                public string CREDITACCT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CENTRE { get; set; }

                public string DEPARTMENT { get; set; }

                public string NAME { get; set; }





                #endregion Instance Properties
}
            public class GLUPDATE
            {




                #region Instance Properties

                public int recid { get; set; }

                public string JVNO { get; set; }

                public decimal? PERIOD { get; set; }

                public decimal? PPYEAR { get; set; }

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

                public string TYPE { get; set; }

                public string TGROUPTYPE { get; set; }

                public string TBTYPE { get; set; }





                #endregion Instance Properties
}
            public class INTGRID
            {




                #region Instance Properties

                public int recid { get; set; }

                public string APPREF { get; set; }

                public string INTVNO { get; set; }

                public DateTime? INTVDATE { get; set; }

                public string INTVGRP { get; set; }

                public string INTVTYPE { get; set; }

                public string INTVTIME { get; set; }

                public string NOTES { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class PAY01
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public string DEPARTMENT { get; set; }

                public string PERIOD { get; set; }

                public DateTime? PPDATE { get; set; }

                public string COMPANY { get; set; }

                public string CENTER { get; set; }

                public string PAY_METHOD { get; set; }

                public string RUN_CODE { get; set; }

                public string BRANCH { get; set; }

                public bool? TAXEXEMP { get; set; }

                public decimal? FIXAMT { get; set; }

                public decimal? SHIFT_ALLW { get; set; }

                public string ACCOUNT_NO { get; set; }

                public string ACCT_TYPE { get; set; }

                public string BANK { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public bool? UNION_MEMB { get; set; }

                public decimal? NORMAL_HRS { get; set; }

                public decimal? OT_HRS { get; set; }

                public decimal? BASIC_SAL { get; set; }

                public decimal? OVERTIME { get; set; }

                public string NPF_NO { get; set; }

                public string APF_NO { get; set; }

                public string E_CODE1 { get; set; }

                public decimal? E_AMT1 { get; set; }

                public string E_CODE2 { get; set; }

                public decimal? E_AMT2 { get; set; }

                public string E_CODE3 { get; set; }

                public decimal? E_AMT3 { get; set; }

                public string E_CODE4 { get; set; }

                public decimal? E_AMT4 { get; set; }

                public string E_CODE5 { get; set; }

                public decimal? E_AMT5 { get; set; }

                public decimal? E_TOTAL { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? HOURLY { get; set; }

                public decimal? OT_RATE { get; set; }

                public decimal? DOUBLE_RAT { get; set; }

                public string A_CODE1 { get; set; }

                public decimal? A_AMT1 { get; set; }

                public string A_CODE2 { get; set; }

                public decimal? A_AMT2 { get; set; }

                public string A_CODE3 { get; set; }

                public decimal? A_AMT3 { get; set; }

                public string A_CODE4 { get; set; }

                public decimal? A_AMT4 { get; set; }

                public string A_CODE5 { get; set; }

                public decimal? A_AMT5 { get; set; }

                public decimal? A_TOTAL { get; set; }

                public decimal? EMPLOY_NPF { get; set; }

                public decimal? EMPLOY_APF { get; set; }

                public decimal? TAX_AMT { get; set; }

                public decimal? REFUND { get; set; }

                public decimal? NPF_AMT { get; set; }

                public decimal? PENSION { get; set; }

                public decimal? UNION_DUE { get; set; }

                public string D_CODE1 { get; set; }

                public decimal? D_AMT1 { get; set; }

                public string D_CODE2 { get; set; }

                public decimal? D_AMT2 { get; set; }

                public string D_CODE3 { get; set; }

                public decimal? D_AMT3 { get; set; }

                public string D_CODE4 { get; set; }

                public decimal? D_AMT4 { get; set; }

                public string D_CODE5 { get; set; }

                public decimal? D_AMT5 { get; set; }

                public string LOAN1 { get; set; }

                public decimal? LOAN1_AMT { get; set; }

                public string LOAN2 { get; set; }

                public decimal? LOAN2_AMT { get; set; }

                public string LOAN3 { get; set; }

                public decimal? LOAN3_AMT { get; set; }

                public string LOAN4 { get; set; }

                public decimal? LOAN4_AMT { get; set; }

                public string LOAN5 { get; set; }

                public decimal? LOAN5_AMT { get; set; }

                public decimal? D_TOTAL { get; set; }

                public string PAY_POINT { get; set; }

                public decimal? GROSS_PAY { get; set; }

                public decimal? NET_PAY { get; set; }

                public decimal? GROSS_YTD { get; set; }

                public decimal? NET_YTD { get; set; }

                public decimal? PENS_YTD { get; set; }

                public decimal? NPF_YTD { get; set; }

                public decimal? EMP_APF_YT { get; set; }

                public decimal? EMP_NPF_YT { get; set; }

                public decimal? EARN_YTD { get; set; }

                public decimal? FREEPAY { get; set; }

                public decimal? TAX_YTD { get; set; }

                public string BLANK { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string EMP_STATUS { get; set; }

                public string STAT_DESC { get; set; }

                public bool? MID_MONTH { get; set; }

                public decimal? DAYS_MON { get; set; }

                public decimal? DAYS_WRK { get; set; }

                public string LEAVING_DT { get; set; }

                public decimal? ENT_AMT { get; set; }

                public decimal? TAXABLE { get; set; }

                public bool? ACTIVEREC { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public string OLDCO { get; set; }

                public string CATEGORY { get; set; }

                public decimal? OTHER_PAY2 { get; set; }

                public decimal? BAS_SAL2 { get; set; }

                public decimal? PENS2_YTD { get; set; }

                public decimal? TAX2_YTD { get; set; }

                public decimal? TAX2 { get; set; }

                public decimal? GROSS2_YTD { get; set; }

                public decimal? EARN2_YTD { get; set; }

                public decimal? NET2_YTD { get; set; }

                public decimal? NET_SAL { get; set; }

                public decimal? PENSION2 { get; set; }

                public decimal? KOADD { get; set; }

                public decimal? KODED { get; set; }

                public string A_CODE6 { get; set; }

                public decimal? A_AMT6 { get; set; }

                public string A_CODE7 { get; set; }

                public decimal? A_AMT7 { get; set; }

                public string A_CODE8 { get; set; }

                public decimal? A_AMT8 { get; set; }

                public string A_CODE9 { get; set; }

                public decimal? A_AMT9 { get; set; }

                public string A_CODE10 { get; set; }

                public decimal? A_AMT10 { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? RUNCODE { get; set; }





                #endregion Instance Properties
}
            public class PAY02
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string NAME { get; set; }

                public string SURNAME { get; set; }

                public string FIRSTNAME { get; set; }

                public string SECONDNAME { get; set; }

                public string INITIALS { get; set; }

                public string TITLE { get; set; }

                public string SEX { get; set; }

                public string CIVIL { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public decimal? ANNUAL_SAL { get; set; }

                public decimal? BASIC_SAL { get; set; }

                public string CATEGORY { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public decimal? SHIFT_ALLW { get; set; }

                public bool? TAXEXEMP { get; set; }

                public decimal? FIXAMT { get; set; }

                public string PAY_METHOD { get; set; }

                public decimal? HOUR_RATE { get; set; }

                public string CENTER { get; set; }

                public string PAY_POINT { get; set; }

                public string DEPARTMENT { get; set; }

                public string BRANCH { get; set; }

                public string BANK { get; set; }

                public string ACCOUNT_NO { get; set; }

                public string ACCT_TYPE { get; set; }

                public decimal? TAX_YEAR { get; set; }

                public decimal? FREEPAY { get; set; }

                public string TAX_REF_NO { get; set; }

                public string NPF_NO { get; set; }

                public string APF_NO { get; set; }

                public decimal? EMPLOY_APF { get; set; }

                public decimal? EMPLOY_NPF { get; set; }

                public decimal? GROSS_YTD { get; set; }

                public decimal? EARN_YTD { get; set; }

                public decimal? NET_YTD { get; set; }

                public decimal? NPF_YTD { get; set; }

                public decimal? PENS_YTD { get; set; }

                public decimal? TAX_YTD { get; set; }

                public decimal? EMP_APF_YT { get; set; }

                public decimal? EMP_NPF_YT { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? HOURLY { get; set; }

                public decimal? OT_RATE { get; set; }

                public decimal? DOUBLE_RAT { get; set; }

                public string RUN_CODE { get; set; }

                public bool? MID_MONTH { get; set; }

                public decimal? NPF_AMT { get; set; }

                public decimal? PENSION { get; set; }

                public bool? UNION_MEMB { get; set; }

                public DateTime? DATE { get; set; }

                public string TIME { get; set; }

                public bool? STOP_CODE { get; set; }

                public DateTime? STOP_DATE { get; set; }

                public string EMP_STATUS { get; set; }

                public string STAT_DESC { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string BLANK { get; set; }

                public decimal? DAYS_MON { get; set; }

                public decimal? DAYS_WRK { get; set; }

                public string LEAVING_DT { get; set; }

                public decimal? ENT_AMT { get; set; }

                public decimal? TAXABLE { get; set; }

                public bool? ACTIVEREC { get; set; }

                public string GLCODE { get; set; }

                public string OLDCO { get; set; }

                public decimal? TAX2 { get; set; }

                public decimal? PENSION2 { get; set; }

                public decimal? BAS_SAL2 { get; set; }

                public decimal? GROSS2_YTD { get; set; }

                public decimal? TAX2_YTD { get; set; }

                public decimal? PENS2_YTD { get; set; }

                public bool? SECTI_POST { get; set; }

                public decimal? EARN2_YTD { get; set; }

                public decimal? NET2_YTD { get; set; }

                public decimal? NET_SAL { get; set; }

                public decimal? OTHER_PAY2 { get; set; }

                public string PFM { get; set; }

                public string CURRENCY { get; set; }

                public decimal? RUNCODE { get; set; }





                #endregion Instance Properties
}
            public class PAY03
            {




                #region Instance Properties

                public int recid { get; set; }

                public string CODE { get; set; }

                public string NAME { get; set; }

                public decimal? EMPLOYEE { get; set; }

                public decimal? EMPLOYER { get; set; }

                public decimal? LESS_AMT { get; set; }

                public decimal? CURRENT_YR { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string ONGROSS { get; set; }





                #endregion Instance Properties
}
            public class PAY04
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public string DEPARTMENT { get; set; }

                public string PERIOD { get; set; }

                public DateTime? PPDATE { get; set; }

                public string COMPANY { get; set; }

                public string CENTER { get; set; }

                public string PAY_METHOD { get; set; }

                public string RUN_CODE { get; set; }

                public string BRANCH { get; set; }

                public bool? TAXEXEMP { get; set; }

                public decimal? FIXAMT { get; set; }

                public decimal? SHIFT_ALLW { get; set; }

                public string ACCOUNT_NO { get; set; }

                public string ACCT_TYPE { get; set; }

                public string BANK { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public bool? UNION_MEMB { get; set; }

                public decimal? NORMAL_HRS { get; set; }

                public decimal? OT_HRS { get; set; }

                public decimal? BASIC_SAL { get; set; }

                public decimal? OVERTIME { get; set; }

                public string NPF_NO { get; set; }

                public string APF_NO { get; set; }

                public string E_CODE1 { get; set; }

                public decimal? E_AMT1 { get; set; }

                public string E_CODE2 { get; set; }

                public decimal? E_AMT2 { get; set; }

                public string E_CODE3 { get; set; }

                public decimal? E_AMT3 { get; set; }

                public string E_CODE4 { get; set; }

                public decimal? E_AMT4 { get; set; }

                public string E_CODE5 { get; set; }

                public decimal? E_AMT5 { get; set; }

                public decimal? E_TOTAL { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? HOURLY { get; set; }

                public decimal? OT_RATE { get; set; }

                public decimal? DOUBLE_RAT { get; set; }

                public string A_CODE1 { get; set; }

                public decimal? A_AMT1 { get; set; }

                public string A_CODE2 { get; set; }

                public decimal? A_AMT2 { get; set; }

                public string A_CODE3 { get; set; }

                public decimal? A_AMT3 { get; set; }

                public string A_CODE4 { get; set; }

                public decimal? A_AMT4 { get; set; }

                public string A_CODE5 { get; set; }

                public decimal? A_AMT5 { get; set; }

                public decimal? A_TOTAL { get; set; }

                public decimal? EMPLOY_NPF { get; set; }

                public decimal? EMPLOY_APF { get; set; }

                public decimal? TAX_AMT { get; set; }

                public decimal? REFUND { get; set; }

                public decimal? NPF_AMT { get; set; }

                public decimal? PENSION { get; set; }

                public decimal? UNION_DUE { get; set; }

                public string D_CODE1 { get; set; }

                public decimal? D_AMT1 { get; set; }

                public string D_CODE2 { get; set; }

                public decimal? D_AMT2 { get; set; }

                public string D_CODE3 { get; set; }

                public decimal? D_AMT3 { get; set; }

                public string D_CODE4 { get; set; }

                public decimal? D_AMT4 { get; set; }

                public string D_CODE5 { get; set; }

                public decimal? D_AMT5 { get; set; }

                public string LOAN1 { get; set; }

                public decimal? LOAN1_AMT { get; set; }

                public string LOAN2 { get; set; }

                public decimal? LOAN2_AMT { get; set; }

                public string LOAN3 { get; set; }

                public decimal? LOAN3_AMT { get; set; }

                public string LOAN4 { get; set; }

                public decimal? LOAN4_AMT { get; set; }

                public string LOAN5 { get; set; }

                public decimal? LOAN5_AMT { get; set; }

                public decimal? D_TOTAL { get; set; }

                public string PAY_POINT { get; set; }

                public decimal? GROSS_PAY { get; set; }

                public decimal? NET_PAY { get; set; }

                public decimal? GROSS_YTD { get; set; }

                public decimal? NET_YTD { get; set; }

                public decimal? PENS_YTD { get; set; }

                public decimal? NPF_YTD { get; set; }

                public decimal? EMP_APF_YT { get; set; }

                public decimal? EMP_NPF_YT { get; set; }

                public decimal? EARN_YTD { get; set; }

                public decimal? FREEPAY { get; set; }

                public decimal? TAX_YTD { get; set; }

                public string BLANK { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string EMP_STATUS { get; set; }

                public string STAT_DESC { get; set; }

                public bool? MID_MONTH { get; set; }

                public decimal? DAYS_MON { get; set; }

                public decimal? DAYS_WRK { get; set; }

                public string LEAVING_DT { get; set; }

                public decimal? ENT_AMT { get; set; }

                public decimal? TAXABLE { get; set; }

                public bool? ACTIVEREC { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public string OLDCO { get; set; }

                public string CATEGORY { get; set; }

                public decimal? OTHER_PAY2 { get; set; }

                public decimal? BAS_SAL2 { get; set; }

                public decimal? PENS2_YTD { get; set; }

                public decimal? TAX2_YTD { get; set; }

                public decimal? TAX2 { get; set; }

                public decimal? GROSS2_YTD { get; set; }

                public decimal? EARN2_YTD { get; set; }

                public decimal? NET2_YTD { get; set; }

                public decimal? NET_SAL { get; set; }

                public decimal? PENSION2 { get; set; }

                public decimal? KOADD { get; set; }

                public decimal? KODED { get; set; }

                public string A_CODE6 { get; set; }

                public decimal? A_AMT6 { get; set; }

                public string A_CODE7 { get; set; }

                public decimal? A_AMT7 { get; set; }

                public string A_CODE8 { get; set; }

                public decimal? A_AMT8 { get; set; }

                public string A_CODE9 { get; set; }

                public decimal? A_AMT9 { get; set; }

                public string A_CODE10 { get; set; }

                public decimal? A_AMT10 { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? RUNCODE { get; set; }





                #endregion Instance Properties
}
            public class PAY04HIST
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public string DEPARTMENT { get; set; }

                public string PERIOD { get; set; }

                public DateTime? PPDATE { get; set; }

                public string COMPANY { get; set; }

                public string CENTER { get; set; }

                public string PAY_METHOD { get; set; }

                public string RUN_CODE { get; set; }

                public string BRANCH { get; set; }

                public bool? TAXEXEMP { get; set; }

                public decimal? FIXAMT { get; set; }

                public decimal? SHIFT_ALLW { get; set; }

                public string ACCOUNT_NO { get; set; }

                public string ACCT_TYPE { get; set; }

                public string BANK { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public bool? UNION_MEMB { get; set; }

                public decimal? NORMAL_HRS { get; set; }

                public decimal? OT_HRS { get; set; }

                public decimal? BASIC_SAL { get; set; }

                public decimal? OVERTIME { get; set; }

                public string NPF_NO { get; set; }

                public string APF_NO { get; set; }

                public string E_CODE1 { get; set; }

                public decimal? E_AMT1 { get; set; }

                public string E_CODE2 { get; set; }

                public decimal? E_AMT2 { get; set; }

                public string E_CODE3 { get; set; }

                public decimal? E_AMT3 { get; set; }

                public string E_CODE4 { get; set; }

                public decimal? E_AMT4 { get; set; }

                public string E_CODE5 { get; set; }

                public decimal? E_AMT5 { get; set; }

                public decimal? E_TOTAL { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? HOURLY { get; set; }

                public decimal? OT_RATE { get; set; }

                public decimal? DOUBLE_RAT { get; set; }

                public string A_CODE1 { get; set; }

                public decimal? A_AMT1 { get; set; }

                public string A_CODE2 { get; set; }

                public decimal? A_AMT2 { get; set; }

                public string A_CODE3 { get; set; }

                public decimal? A_AMT3 { get; set; }

                public string A_CODE4 { get; set; }

                public decimal? A_AMT4 { get; set; }

                public string A_CODE5 { get; set; }

                public decimal? A_AMT5 { get; set; }

                public decimal? A_TOTAL { get; set; }

                public decimal? EMPLOY_NPF { get; set; }

                public decimal? EMPLOY_APF { get; set; }

                public decimal? TAX_AMT { get; set; }

                public decimal? REFUND { get; set; }

                public decimal? NPF_AMT { get; set; }

                public decimal? PENSION { get; set; }

                public decimal? UNION_DUE { get; set; }

                public string D_CODE1 { get; set; }

                public decimal? D_AMT1 { get; set; }

                public string D_CODE2 { get; set; }

                public decimal? D_AMT2 { get; set; }

                public string D_CODE3 { get; set; }

                public decimal? D_AMT3 { get; set; }

                public string D_CODE4 { get; set; }

                public decimal? D_AMT4 { get; set; }

                public string D_CODE5 { get; set; }

                public decimal? D_AMT5 { get; set; }

                public string LOAN1 { get; set; }

                public decimal? LOAN1_AMT { get; set; }

                public string LOAN2 { get; set; }

                public decimal? LOAN2_AMT { get; set; }

                public string LOAN3 { get; set; }

                public decimal? LOAN3_AMT { get; set; }

                public string LOAN4 { get; set; }

                public decimal? LOAN4_AMT { get; set; }

                public string LOAN5 { get; set; }

                public decimal? LOAN5_AMT { get; set; }

                public decimal? D_TOTAL { get; set; }

                public string PAY_POINT { get; set; }

                public decimal? GROSS_PAY { get; set; }

                public decimal? NET_PAY { get; set; }

                public decimal? GROSS_YTD { get; set; }

                public decimal? NET_YTD { get; set; }

                public decimal? PENS_YTD { get; set; }

                public decimal? NPF_YTD { get; set; }

                public decimal? EMP_APF_YT { get; set; }

                public decimal? EMP_NPF_YT { get; set; }

                public decimal? EARN_YTD { get; set; }

                public decimal? FREEPAY { get; set; }

                public decimal? TAX_YTD { get; set; }

                public string BLANK { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string EMP_STATUS { get; set; }

                public string STAT_DESC { get; set; }

                public bool? MID_MONTH { get; set; }

                public decimal? DAYS_MON { get; set; }

                public decimal? DAYS_WRK { get; set; }

                public string LEAVING_DT { get; set; }

                public decimal? ENT_AMT { get; set; }

                public decimal? TAXABLE { get; set; }

                public bool? ACTIVEREC { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public string OLDCO { get; set; }

                public string CATEGORY { get; set; }

                public decimal? OTHER_PAY2 { get; set; }

                public decimal? BAS_SAL2 { get; set; }

                public decimal? PENS2_YTD { get; set; }

                public decimal? TAX2_YTD { get; set; }

                public decimal? TAX2 { get; set; }

                public decimal? GROSS2_YTD { get; set; }

                public decimal? EARN2_YTD { get; set; }

                public decimal? NET2_YTD { get; set; }

                public decimal? NET_SAL { get; set; }

                public decimal? PENSION2 { get; set; }

                public decimal? KOADD { get; set; }

                public decimal? KODED { get; set; }

                public string A_CODE6 { get; set; }

                public decimal? A_AMT6 { get; set; }

                public string A_CODE7 { get; set; }

                public decimal? A_AMT7 { get; set; }

                public string A_CODE8 { get; set; }

                public decimal? A_AMT8 { get; set; }

                public string A_CODE9 { get; set; }

                public decimal? A_AMT9 { get; set; }

                public string A_CODE10 { get; set; }

                public decimal? A_AMT10 { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? RUNCODE { get; set; }





                #endregion Instance Properties
}
            public class PAY05
            {




                #region Instance Properties

                public int recid { get; set; }

                public string COMPANY { get; set; }

                public string STAFF_NO { get; set; }

                public string BRANCH { get; set; }

                public string DEPARTMENT { get; set; }

                public string PERIOD { get; set; }

                public decimal? E_TOTAL { get; set; }

                public decimal? EARN_YTD { get; set; }

                public decimal? FREEPAY { get; set; }

                public decimal? TAX_YTD { get; set; }

                public decimal? TAX_AMT { get; set; }

                public decimal? REFUND { get; set; }

                public DateTime? TRANS_DATE { get; set; }





                #endregion Instance Properties
}
            public class PAY05HIST
            {




                #region Instance Properties

                public int recid { get; set; }

                public string COMPANY { get; set; }

                public string STAFF_NO { get; set; }

                public string BRANCH { get; set; }

                public string DEPARTMENT { get; set; }

                public string PERIOD { get; set; }

                public decimal? E_TOTAL { get; set; }

                public decimal? EARN_YTD { get; set; }

                public decimal? FREEPAY { get; set; }

                public decimal? TAX_YTD { get; set; }

                public decimal? TAX_AMT { get; set; }

                public decimal? REFUND { get; set; }

                public DateTime? TRANS_DATE { get; set; }





                #endregion Instance Properties
}
            public class PAY06
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public string DED_CODE { get; set; }

                public string DED_NAME { get; set; }

                public decimal? STAFF_AMT { get; set; }

                public string CONT_CODE { get; set; }

                public decimal? EMP_AMT { get; set; }

                public DateTime? START_DATE { get; set; }

                public decimal? FREQUENCY { get; set; }

                public decimal? STAFF_CUM { get; set; }

                public decimal? EMP_CUM { get; set; }

                public decimal? LASTPAY { get; set; }

                public DateTime? END_DATE { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? LAST_UPD { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CENTER { get; set; }

                public string COMPANY { get; set; }

                public string DEPARTMENT { get; set; }

                public string CURRENCY { get; set; }





                #endregion Instance Properties
}
            public class PAY07
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string BRANCH { get; set; }

                public string TYPE { get; set; }

                public string GROUP_CODE { get; set; }

                public string ADJUST { get; set; }

                public string REF_NO { get; set; }

                public decimal? LAST_MONTH { get; set; }

                public decimal? AMOUNT { get; set; }

                public decimal? AMOUNT_YTD { get; set; }

                public decimal? BD_AMT { get; set; }

                public string CENTER { get; set; }

                public string DEPARTMENT { get; set; }

                public bool? CHANGED { get; set; }

                public string TAXABLE { get; set; }

                public decimal? FREQUENCY { get; set; }

                public decimal? LAST_UPD { get; set; }

                public string EFF_DATE { get; set; }

                public string STOP_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public string TIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? THISMONTH { get; set; }

                public string ADJUSTED { get; set; }

                public bool? MIDMONTH { get; set; }

                public string CONT_CODE { get; set; }

                public string CURRENCY { get; set; }





                #endregion Instance Properties
}
            public class PAY08
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public string LOAN { get; set; }

                public string NAME { get; set; }

                public DateTime? GRANT_DATE { get; set; }

                public decimal? PRINCIPAL { get; set; }

                public decimal? INTEREST { get; set; }

                public decimal? INSTALL { get; set; }

                public decimal? FIRST_INST { get; set; }

                public decimal? START_MON { get; set; }

                public decimal? LAST_MONTH { get; set; }

                public decimal? MONTHLY_PR { get; set; }

                public decimal? MONTHLY_IN { get; set; }

                public decimal? MONTHLY_AM { get; set; }

                public decimal? YTD_PAID { get; set; }

                public decimal? TOTAL_INST { get; set; }

                public decimal? CLEARD_AMT { get; set; }

                public decimal? LAST { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CENTER { get; set; }

                public string COMPANY { get; set; }

                public bool? CLEARED { get; set; }

                public DateTime? CLEAR_DATE { get; set; }

                public string LN_STATUS { get; set; }

                public string DEPARTMENT { get; set; }

                public string BRANCH { get; set; }

                public decimal? INT_PERCEN { get; set; }

                public bool? MIDMONTH { get; set; }

                public decimal? ACTUAL_PR { get; set; }

                public decimal? TOTAL_INT { get; set; }

                public string CURRENCY { get; set; }





                #endregion Instance Properties
}
            public class PAY09
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string BRANCH { get; set; }

                public DateTime? WORK_DATE { get; set; }

                public string TIME_TYPE { get; set; }

                public decimal? HOURS { get; set; }

                public string JOB_CODE { get; set; }

                public decimal? RATE { get; set; }

                public string REF_NO { get; set; }

                public decimal? AMOUNT { get; set; }

                public bool? UPDATED { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public decimal? TIN { get; set; }

                public decimal? TOUT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CENTER { get; set; }

                public string DEPARTMENT { get; set; }





                #endregion Instance Properties
}
            public class PAY10
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string BRANCH { get; set; }

                public DateTime? WORK_DATE { get; set; }

                public string TIME_TYPE { get; set; }

                public decimal? HOURS { get; set; }

                public string JOB_CODE { get; set; }

                public decimal? RATE { get; set; }

                public string REF_NO { get; set; }

                public decimal? AMOUNT { get; set; }

                public bool? UPDATED { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public decimal? TIN { get; set; }

                public decimal? TOUT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CENTER { get; set; }

                public string DEPARTMENT { get; set; }





                #endregion Instance Properties
}
            public class PAY11
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public DateTime? LAST_PAYDT { get; set; }

                public decimal? PAIDTODATE { get; set; }

                public decimal? LASTPAYMENT { get; set; }

                public decimal? INSTALL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CENTER { get; set; }

                public string COMPANY { get; set; }

                public string DEPARTMENT { get; set; }

                public string BRANCH { get; set; }

                public decimal? INSTALLTYPE { get; set; }





                #endregion Instance Properties
}
            public class PAY11A
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? PAIDTODATE { get; set; }

                public decimal? BASICSAL { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? LEAVEPAY { get; set; }

                public decimal? GROSSPAY { get; set; }

                public decimal? GRATUITY { get; set; }

                public decimal? LOANDEDUCT { get; set; }

                public decimal? COOPLOAN { get; set; }

                public decimal? PAYABLE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CENTER { get; set; }

                public string COMPANY { get; set; }

                public string DEPARTMENT { get; set; }

                public string BRANCH { get; set; }

                public string PRD_SERVED { get; set; }





                #endregion Instance Properties
}
            public class PAY12
            {




                #region Instance Properties

                public int recid { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class PAYITEM
            {




                #region Instance Properties

                public int recid { get; set; }

                public string TYPE { get; set; }

                public string PAYITEMS { get; set; }

                public decimal? MULTIPLIER { get; set; }

                public string DESCRIPTN { get; set; }

                public string PAYVALUE { get; set; }





                #endregion Instance Properties
}
            public class PAYITEMA
            {




                #region Instance Properties

                public int recid { get; set; }

                public string TYPE { get; set; }

                public string PAYITEMS { get; set; }

                public decimal? MULTIPLIER { get; set; }

                public string DESCRIPTN { get; set; }

                public string PAYVALUE { get; set; }





                #endregion Instance Properties
}
            public class PAYTAXITEMS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PAYTYPE { get; set; }

                public string PAYITEMS { get; set; }

                public decimal? MULTIPLIER { get; set; }

                public string PAYITEMDESC { get; set; }

                public string PAYVALUE { get; set; }





                #endregion Instance Properties
}
            public class PPCONTROL
            {




                #region Instance Properties

                public int recid { get; set; }

                public string SERIAL_NO { get; set; }

                public string PPUSER { get; set; }

                public DateTime? INSTAL_DAT { get; set; }

                public DateTime? TA_START { get; set; }

                public DateTime? LAST_UPDAT { get; set; }

                public decimal? PAY_MONTH { get; set; }

                public decimal? PAY_YEAR { get; set; }

                public DateTime? BACKUP_DAT { get; set; }

                public decimal? CURRENT_YR { get; set; }

                public decimal? CURRENT_MT { get; set; }

                public DateTime? TODAYS_DAT { get; set; }

                public DateTime? LAST_DATE { get; set; }

                public bool? PERSONEL { get; set; }

                public bool? PAYROLL { get; set; }

                public string SERIAL { get; set; }

                public decimal? LEVEL_CODE { get; set; }

                public bool? P_STARTED { get; set; }

                public bool? P_ENDED { get; set; }

                public bool? PA_STARTED { get; set; }

                public bool? PA_ENDED { get; set; }

                public string PATHNAME { get; set; }

                public decimal? LAST_NO { get; set; }

                public bool? MIC_CHECK { get; set; }

                public bool? MIC_UPDATE { get; set; }

                public bool? LEFT_STAFF { get; set; }

                public bool? GRADED { get; set; }

                public bool? OWNTAB { get; set; }

                public bool? AUTOPA { get; set; }

                public decimal? PALLOW { get; set; }

                public decimal? PAPERC { get; set; }

                public bool? GENNOS { get; set; }

                public bool? RENTALL { get; set; }

                public decimal? RENTPER { get; set; }

                public bool? CLUB { get; set; }

                public bool? CASH { get; set; }

                public bool? INV { get; set; }

                public bool? OCE { get; set; }

                public bool? DD { get; set; }

                public bool? GL { get; set; }

                public bool? ANN { get; set; }

                public bool? RETCONT { get; set; }

                public bool? GRASTEP { get; set; }

                public bool? DCE { get; set; }

                public bool? BD { get; set; }

                public string MPASS { get; set; }

                public DateTime? MPASSDT { get; set; }

                public bool? CALCPERCEN { get; set; }

                public decimal? TAXPERCENT { get; set; }

                public bool? ROUNDUP { get; set; }

                public bool? CLOSEDGRAD { get; set; }

                public bool? UDUEINCOMP { get; set; }

                public decimal? DWKHOUR { get; set; }

                public decimal? LUNCHBREAK { get; set; }

                public bool? OVCALCANN { get; set; }

                public decimal? OVTAX { get; set; }

                public string OTALLOWCOD { get; set; }

                public bool? DEDUCTABSE { get; set; }

                public string ABDED1 { get; set; }

                public string ABDED2 { get; set; }

                public string ABDED3 { get; set; }

                public string ABDED4 { get; set; }

                public string ABDED5 { get; set; }

                public string ABDEDFR1 { get; set; }

                public string ABDEDFR2 { get; set; }

                public string ABDEDFR3 { get; set; }

                public string ABDEDFR4 { get; set; }

                public string ABDEDFR5 { get; set; }

                public string RADD { get; set; }

                public string RDED { get; set; }

                public decimal? MINTAXRATE { get; set; }

                public bool? APPLYMINTA { get; set; }

                public decimal? VALDIVIDE { get; set; }

                public bool? FILEMODE { get; set; }

                public bool? NETPOH { get; set; }

                public bool? ALLSECURE { get; set; }

                public bool? YTDONSLIP { get; set; }

                public decimal? PAYRCOUNT { get; set; }

                public string CRPTPATH { get; set; }

                public string CFRMPATH { get; set; }

                public string LOCSTATE { get; set; }

                public string LOCALCUR { get; set; }

                public string CUR_SIGN { get; set; }

                public string LOCCOUNTRY { get; set; }





                #endregion Instance Properties
}
            public class PPSETUP
            {




                #region Instance Properties

                public int recid { get; set; }

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

                public decimal? M_DUTYFRM { get; set; }

                public decimal? M_DUTYTO { get; set; }

                public decimal? A_DUTYFRM { get; set; }

                public decimal? A_DUTYTO { get; set; }

                public decimal? N_DUTYFRM { get; set; }

                public decimal? N_DUTYTO { get; set; }





                #endregion Instance Properties
}
            public class PPSTLEV
            {




                #region Instance Properties

                public int recid { get; set; }

                public string OPERATOR { get; set; }

                public string USERTYPE { get; set; }

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

                public bool? FM21 { get; set; }

                public bool? FM22 { get; set; }

                public bool? FM23 { get; set; }

                public bool? FM24 { get; set; }

                public bool? FM25 { get; set; }

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

                public bool? RM21 { get; set; }

                public bool? RM22 { get; set; }

                public bool? RM23 { get; set; }

                public bool? RM24 { get; set; }

                public bool? RM25 { get; set; }

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

                public bool? CANADD { get; set; }

                public bool? CANDELETE { get; set; }

                public bool? CANALTER { get; set; }

                public decimal? HISTYEAR { get; set; }

                public decimal? RUNCODE { get; set; }

                public bool? FILEMENU { get; set; }

                public bool? REPORTMENU { get; set; }

                public string BRANCH { get; set; }





                #endregion Instance Properties
}
            public class PREEMPTAB
            {




                #region Instance Properties

                public int recid { get; set; }

                public string APPREF { get; set; }

                public DateTime? APPDATE { get; set; }

                public string SURNAME { get; set; }

                public string FIRSTNAME { get; set; }

                public string MIDNAME { get; set; }

                public string GENDER { get; set; }

                public string POSTAPP { get; set; }

                public DateTime? BDATE { get; set; }

                public string AGEGRP { get; set; }

                public string MSTATUS { get; set; }

                public string NATION { get; set; }

                public string STATE { get; set; }

                public string TRIBE { get; set; }

                public string RELIGION { get; set; }

                public string HOMEADD1 { get; set; }

                public string HOMEADD2 { get; set; }

                public string EMAIL { get; set; }

                public string PHONE { get; set; }

                public string DISABLED { get; set; }

                public string HIGHSTQUA { get; set; }

                public string LASTEDU1 { get; set; }

                public string LASTEDU2 { get; set; }

                public string STAFF_NO { get; set; }

                public string DESIGNATION { get; set; }

                public string DEPARTMENT { get; set; }

                public string BRANCH { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? INTVCOUNT { get; set; }

                public string REGION { get; set; }

                public decimal? EXPERIENCE { get; set; }

                public string DISCIPLINE { get; set; }





                #endregion Instance Properties
}
            public class PREINTPA
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string APPREF { get; set; }

                public string INTVNO { get; set; }

                public string NAME { get; set; }

                public string OCCUPATION { get; set; }

                public string DEPARTMENT { get; set; }

                public string COMPANY { get; set; }

                public string NOTES { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class RPTDEFHD
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string DESCRIPTN { get; set; }

                public decimal? TOTEA { get; set; }

                public decimal? TOTD { get; set; }

                public decimal? TOTC { get; set; }

                public string EAADDENDUM { get; set; }

                public string DEDADDENDU { get; set; }

                public string COMADDENDU { get; set; }





                #endregion Instance Properties
}
            public class STF001
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string SURNAME { get; set; }

                public string FIRSTNAME { get; set; }

                public string SECONDNAME { get; set; }

                public string INITIALS { get; set; }

                public string TITLE { get; set; }

                public string SEX { get; set; }

                public string CATEGORY { get; set; }

                public string CIVIL { get; set; }

                public DateTime? BIRTH_DATE { get; set; }

                public string STATE { get; set; }

                public string NATION { get; set; }

                public DateTime? JOIN_DATE { get; set; }

                public string PLACE_ENG { get; set; }

                public string EMP_STATUS { get; set; }

                public DateTime? RESUMPTION { get; set; }

                public string JOIN_DESIG { get; set; }

                public string JOB_STATUS { get; set; }

                public string CONFIRM { get; set; }

                public string CUR_JOB { get; set; }

                public string CUR_DESIG { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public string ORG_GRADE { get; set; }

                public DateTime? INC_DUE { get; set; }

                public DateTime? LAST_INC { get; set; }

                public string LOCATION { get; set; }

                public decimal? ANNUAL_SAL { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public string START_GRAD { get; set; }

                public string START_STEP { get; set; }

                public decimal? START_SAL { get; set; }

                public string CENTER { get; set; }

                public string APPT_REF { get; set; }

                public string DEPARTMENT { get; set; }

                public string BRANCH { get; set; }

                public string LEAVING_CD { get; set; }

                public string LANGUAGE { get; set; }

                public string LEAVING_DT { get; set; }

                public string LEAVING_RF { get; set; }

                public string ADMIN_GRAD { get; set; }

                public string REVIEWTYPE { get; set; }

                public string NPF_NO { get; set; }

                public string APF_NO { get; set; }

                public decimal? LEAVE_DAYS { get; set; }

                public decimal? UPD_MONTH { get; set; }

                public DateTime? LAST_LEAVE { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATE { get; set; }

                public string ETHNIC { get; set; }

                public string HOME_TOWN { get; set; }

                public string HOME_ADDR1 { get; set; }

                public string HOME_PHONE { get; set; }

                public string RES_ADDR1 { get; set; }

                public string RES_PHONE { get; set; }

                public string NEXT_KIN { get; set; }

                public string RELATION { get; set; }

                public string KIN_ADDR1 { get; set; }

                public string KIN_PHONE { get; set; }

                public string HUS_WIFE { get; set; }

                public string H_W_ADDR1 { get; set; }

                public string H_W_PHONE { get; set; }

                public string LAST_EDUC { get; set; }

                public string HIGHEST_QL { get; set; }

                public string SPECIAL_QL { get; set; }

                public string MEDICAL_DT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string STATUS { get; set; }

                public string STAT_DESC { get; set; }

                public string OLD_STAFNO { get; set; }

                public string REINST_REF { get; set; }

                public string REINST_DT { get; set; }

                public bool? ACTIVEREC { get; set; }

                public string OLDCO { get; set; }

                public string NAME { get; set; }

                public decimal? ALLOCATION { get; set; }

                public decimal? SPENT { get; set; }

                public string PHOTOLOC { get; set; }

                public string REMARKS { get; set; }

                public string PHOTODATE { get; set; }

                public string EMAILADDRESS { get; set; }

                public bool? UNIFORMED { get; set; }

                public string RELIGION { get; set; }

                public string CLOCKID { get; set; }

                public string PFM { get; set; }

                public string CURRENCY { get; set; }

                public string MAIDENNAME { get; set; }

                public decimal? RUNCODE { get; set; }

                public string MEDNOTES { get; set; }





                #endregion Instance Properties
}
            public class STF001A
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public string GENDER { get; set; }

                public DateTime? BIRTH_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class STF001B
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public string GENDER { get; set; }

                public DateTime? BIRTH_DATE { get; set; }

                public string NOTES { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class STF001C
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public string GENDER { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TIME_IN { get; set; }

                public string TIME_OUT { get; set; }

                public string NOTES { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string DEPT { get; set; }

                public string BRANCH { get; set; }

                public string DEPARTMENT { get; set; }

                public string BRANCHCODE { get; set; }

                public string DUTYSHIFT { get; set; }

                public DateTime? DATE_OUT { get; set; }





                #endregion Instance Properties
}
            public class STF002
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public DateTime? DATE { get; set; }

                public string REFNAME { get; set; }

                public string ADDRESS1 { get; set; }

                public string ADDRESS2 { get; set; }

                public string ADDRESS3 { get; set; }

                public string TELNO { get; set; }

                public string REMARKS { get; set; }





                #endregion Instance Properties
}
            public class STF003
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string COMPANY { get; set; }

                public string ORG_GRADE { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public string DESCRIPTN { get; set; }

                public decimal? ANNUAL_SAL { get; set; }

                public decimal? MIN_SAL { get; set; }

                public decimal? MAX_SAL { get; set; }

                public decimal? HOURLY { get; set; }

                public decimal? LEAVE_DAYS { get; set; }

                public decimal? OT_RATE { get; set; }

                public decimal? DOUBLE_RAT { get; set; }

                public decimal? HOUS_PER { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? TRANS_PER { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? ENT_PER { get; set; }

                public decimal? ENT_AMT { get; set; }

                public decimal? UNION_DUE { get; set; }

                public decimal? STAFF_PER { get; set; }

                public decimal? STAFF_PEN { get; set; }

                public decimal? EMPL_PER { get; set; }

                public decimal? EMPL_PEN { get; set; }

                public decimal? BUDGETED { get; set; }

                public string CATEGORY { get; set; }

                public decimal? ACTUAL { get; set; }

                public decimal? SECURITY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? UNIONPER { get; set; }

                public decimal? UNIONAMT { get; set; }

                public decimal? LEAVEPAY_PER { get; set; }

                public decimal? LEAVEPAY_AMT { get; set; }





                #endregion Instance Properties
}
            public class STF004
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public string TITLE { get; set; }

                public DateTime? START_DATE { get; set; }

                public DateTime? END_DATE { get; set; }

                public string RESULT { get; set; }

                public string COURSE_LOC { get; set; }

                public string ORGANISER { get; set; }

                public decimal? C_COST { get; set; }

                public decimal? O_COST { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string RESOURCEPERSONS { get; set; }





                #endregion Instance Properties
}
            public class STF005
            {




                #region Instance Properties

                public int? RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public string REASON { get; set; }

                public string DEED { get; set; }

                public string INVEST_RES { get; set; }

                public string TYPE { get; set; }

                public DateTime? DATE { get; set; }

                public decimal? DAYS { get; set; }

                public string REMARKS { get; set; }





                #endregion Instance Properties
}
            public class STF006
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string QUALIFY { get; set; }

                public string SUBJECTS { get; set; }

                public DateTime? Q_DATE { get; set; }

                public string GRADE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public string INSTITUTION { get; set; }





                #endregion Instance Properties
}
            public class STF007
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? START_DATE { get; set; }

                public string REF_NO { get; set; }

                public DateTime? END_DATE { get; set; }

                public string COMP_NAME { get; set; }

                public decimal? LAST_SALRY { get; set; }

                public string JOB_TITLE { get; set; }

                public string REASON { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string JOB_DESC { get; set; }





                #endregion Instance Properties
}
            public class STF008
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string STAFF_NAME { get; set; }

                public string ASSET_REF { get; set; }

                public string ASSET_CODE { get; set; }

                public string ALLOC_REF { get; set; }

                public DateTime? DATE_ALLOC { get; set; }

                public DateTime? DATE_RELES { get; set; }

                public DateTime? PUR_DATE { get; set; }

                public string ALLC_STAFF { get; set; }

                public string REG_NO { get; set; }

                public string ENG_NO { get; set; }

                public string ASSET_DESC { get; set; }

                public string ADDRESS { get; set; }

                public string STREET { get; set; }

                public string CITY { get; set; }

                public string BOX { get; set; }

                public string STATE { get; set; }

                public string VEH_TYPE { get; set; }

                public string MODEL_YEAR { get; set; }

                public DateTime? REG_DATE { get; set; }

                public DateTime? INS_DUE { get; set; }

                public DateTime? REG_DUE { get; set; }

                public DateTime? ENDT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string EMAILBOX { get; set; }





                #endregion Instance Properties
}
            public class STF009
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public string DEPARTMENT { get; set; }

                public string RDEPART { get; set; }

                public string BRANCH { get; set; }

                public string RBRANCH { get; set; }

                public string CATEGORY { get; set; }

                public string RCATEGORY { get; set; }

                public string EMP_STATUS { get; set; }

                public string RSTATUS { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public string RSAL_GRADE { get; set; }

                public string RSAL_STEP { get; set; }

                public decimal? ANNUAL_SAL { get; set; }

                public decimal? RANNUALSAL { get; set; }

                public decimal? RHOUSING { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? RTRANSPORT { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? HOURLY { get; set; }

                public decimal? OT_RATE { get; set; }

                public decimal? DOUBLE_RAT { get; set; }

                public string CUR_JOB { get; set; }

                public string CUR_DESIG { get; set; }

                public string RDESIG { get; set; }

                public string ORG_GRADE { get; set; }

                public string CENTER { get; set; }

                public string RCENTER { get; set; }

                public string LOCATION { get; set; }

                public string PAY_POINT { get; set; }

                public string REVIEWTYPE { get; set; }

                public string REMARKS { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string ENT_TIME { get; set; }

                public string ADMIN_GRAD { get; set; }

                public bool? SAL_CHANG { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? DAYS_MON { get; set; }

                public decimal? DAYS_WRK { get; set; }

                public string LEAVING_DT { get; set; }

                public string LEAVING_RF { get; set; }

                public string LEAVING_CD { get; set; }

                public string STAT_DESC { get; set; }

                public string COMPANY { get; set; }

                public string CODE { get; set; }

                public string SURNAME { get; set; }

                public string CIVIL { get; set; }

                public string OLDCO { get; set; }

                public decimal? BAS_SAL2 { get; set; }

                public decimal? TAX2 { get; set; }

                public decimal? PENSION2 { get; set; }

                public string FIRSTNAME { get; set; }

                public string SECONDNAME { get; set; }

                public string EXITINTVCOMMENT { get; set; }

                public string ACTIONREQUIRED { get; set; }

                public string INTERVIEWER { get; set; }

                public string HOD { get; set; }

                public string HROFFICER { get; set; }

                public string MAIDENNAME { get; set; }





                #endregion Instance Properties
}
            public class STF009A
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string DESCRIPTN { get; set; }

                public string TYPE { get; set; }





                #endregion Instance Properties
}
            public class STF009B
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string QUESTIONS { get; set; }

                public string REFERENCE { get; set; }

                public string RESPONSE { get; set; }





                #endregion Instance Properties
}
            public class STF009C
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public DateTime? LEAVINGDATE { get; set; }

                public string REPORTTO { get; set; }

                public string TITLE { get; set; }

                public string INTERVIEWER { get; set; }

                public string COMMENT { get; set; }

                public string ACTIONS { get; set; }

                public string REVIEWEDBY { get; set; }

                public DateTime? REVIEWDATE { get; set; }

                public string HOD { get; set; }

                public DateTime? HODDATE { get; set; }

                public string DHR { get; set; }

                public DateTime? DHRDATE { get; set; }





                #endregion Instance Properties
}
            public class STF010
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public string DEPARTMENT { get; set; }

                public string RDEPART { get; set; }

                public string BRANCH { get; set; }

                public string RBRANCH { get; set; }

                public string CATEGORY { get; set; }

                public string RCATEGORY { get; set; }

                public string EMP_STATUS { get; set; }

                public string RSTATUS { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public string RSAL_GRADE { get; set; }

                public string RSAL_STEP { get; set; }

                public decimal? ANNUAL_SAL { get; set; }

                public decimal? RANNUALSAL { get; set; }

                public decimal? RHOUSING { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? RTRANSPORT { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? HOURLY { get; set; }

                public decimal? OT_RATE { get; set; }

                public decimal? DOUBLE_RAT { get; set; }

                public string CUR_JOB { get; set; }

                public string CUR_DESIG { get; set; }

                public string RDESIG { get; set; }

                public string ORG_GRADE { get; set; }

                public string CENTER { get; set; }

                public string RCENTER { get; set; }

                public string LOCATION { get; set; }

                public string PAY_POINT { get; set; }

                public string REVIEWTYPE { get; set; }

                public string REMARKS { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string ENT_TIME { get; set; }

                public string ADMIN_GRAD { get; set; }

                public bool? SAL_CHANG { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? DAYS_MON { get; set; }

                public decimal? DAYS_WRK { get; set; }

                public string LEAVING_DT { get; set; }

                public string LEAVING_RF { get; set; }

                public string LEAVING_CD { get; set; }

                public string STAT_DESC { get; set; }

                public string COMPANY { get; set; }

                public string CODE { get; set; }

                public string SURNAME { get; set; }

                public string CIVIL { get; set; }

                public string OLDCO { get; set; }

                public decimal? BAS_SAL2 { get; set; }

                public decimal? TAX2 { get; set; }

                public decimal? PENSION2 { get; set; }

                public string FIRSTNAME { get; set; }

                public string SECONDNAME { get; set; }

                public string EXITINTVCOMMENT { get; set; }

                public string ACTIONREQUIRED { get; set; }

                public string INTERVIEWER { get; set; }

                public string HOD { get; set; }

                public string HROFFICER { get; set; }

                public string MAIDENNAME { get; set; }





                #endregion Instance Properties
}
            public class STF010HIST
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public string DEPARTMENT { get; set; }

                public string RDEPART { get; set; }

                public string BRANCH { get; set; }

                public string RBRANCH { get; set; }

                public string CATEGORY { get; set; }

                public string RCATEGORY { get; set; }

                public string EMP_STATUS { get; set; }

                public string RSTATUS { get; set; }

                public DateTime? EFF_DATE { get; set; }

                public string SAL_GRADE { get; set; }

                public string SAL_STEP { get; set; }

                public string RSAL_GRADE { get; set; }

                public string RSAL_STEP { get; set; }

                public decimal? ANNUAL_SAL { get; set; }

                public decimal? RANNUALSAL { get; set; }

                public decimal? RHOUSING { get; set; }

                public decimal? HOUSING { get; set; }

                public decimal? RTRANSPORT { get; set; }

                public decimal? TRANSPORT { get; set; }

                public decimal? HOURLY { get; set; }

                public decimal? OT_RATE { get; set; }

                public decimal? DOUBLE_RAT { get; set; }

                public string CUR_JOB { get; set; }

                public string CUR_DESIG { get; set; }

                public string RDESIG { get; set; }

                public string ORG_GRADE { get; set; }

                public string CENTER { get; set; }

                public string RCENTER { get; set; }

                public string LOCATION { get; set; }

                public string PAY_POINT { get; set; }

                public string REVIEWTYPE { get; set; }

                public string REMARKS { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string ENT_TIME { get; set; }

                public string ADMIN_GRAD { get; set; }

                public bool? SAL_CHANG { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? DAYS_MON { get; set; }

                public decimal? DAYS_WRK { get; set; }

                public string LEAVING_DT { get; set; }

                public string LEAVING_RF { get; set; }

                public string LEAVING_CD { get; set; }

                public string STAT_DESC { get; set; }

                public string COMPANY { get; set; }

                public string CODE { get; set; }

                public string SURNAME { get; set; }

                public string CIVIL { get; set; }

                public string OLDCO { get; set; }

                public decimal? BAS_SAL2 { get; set; }

                public decimal? TAX2 { get; set; }

                public decimal? PENSION2 { get; set; }

                public string FIRSTNAME { get; set; }

                public string SECONDNAME { get; set; }

                public string EXITINTVCOMMENT { get; set; }

                public string ACTIONREQUIRED { get; set; }

                public string INTERVIEWER { get; set; }

                public string HOD { get; set; }

                public string HROFFICER { get; set; }

                public string MAIDENNAME { get; set; }





                #endregion Instance Properties
}
            public class STF011
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? AMOUNT { get; set; }

                public string CENTER { get; set; }

                public string HOSPITAL { get; set; }

                public string ILLNESS { get; set; }

                public decimal? SICK_LEAVE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string COMPANY { get; set; }

                public string DEPARTMENT { get; set; }

                public string BRANCH { get; set; }





                #endregion Instance Properties
}
            public class STF012
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string DEPARTMENT { get; set; }

                public decimal? PAYYEAR { get; set; }

                public string TYPE { get; set; }

                public DateTime? START_DATE { get; set; }

                public DateTime? END_DATE { get; set; }

                public string LEAVE_CODE { get; set; }

                public DateTime? EXPECT_DATE { get; set; }

                public string REF_NO { get; set; }

                public decimal? ALLOWANCE { get; set; }

                public decimal? YTD_LEAVE { get; set; }

                public DateTime? SCH_DATE { get; set; }

                public decimal? NO_OF_DAYS { get; set; }

                public decimal? UNUSED_DAY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public string TIME { get; set; }

                public string REMARKS { get; set; }

                public string AUTHORIZEDBY { get; set; }

                public DateTime? AUTHORIZEDDATE { get; set; }





                #endregion Instance Properties
}
            public class STF012A
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string DEPARTMENT { get; set; }

                public decimal? PAYYEAR { get; set; }

                public string TYPE { get; set; }

                public DateTime? START_DATE { get; set; }

                public DateTime? END_DATE { get; set; }

                public string LEAVE_CODE { get; set; }

                public DateTime? EXPCT_DATE { get; set; }

                public string REF_NO { get; set; }

                public decimal? ALLOWANCE { get; set; }

                public decimal? YTD_LEAVE { get; set; }

                public DateTime? SCH_DATE { get; set; }

                public decimal? NO_OF_DAYS { get; set; }

                public decimal? UNUSED_DAY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }

                public string TIME { get; set; }

                public string REMARKS { get; set; }

                public string AUTHORIZEDBY { get; set; }

                public DateTime? AUTHORIZEDDATE { get; set; }

                public string LEAVETYPE { get; set; }





                #endregion Instance Properties
}
            public class STF013
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public DateTime? START_DATE { get; set; }

                public DateTime? END_DATE { get; set; }

                public string REASON { get; set; }

                public string REASON_DES { get; set; }

                public bool? ALLOWED { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class STF014
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string REF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string REASON { get; set; }

                public string REASON_DES { get; set; }

                public string REMARKS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string DEPARTMENT { get; set; }

                public string SECTION { get; set; }

                public string INV_REPORT { get; set; }

                public string HEARING_RESULT { get; set; }

                public string APPEALDETAIL { get; set; }

                public string FINALRESULT { get; set; }

                public bool? APPEALED { get; set; }

                public string COMPANY { get; set; }

                public string BRANCH { get; set; }

                public string SURNAME { get; set; }

                public string FIRSTNAME { get; set; }

                public string SECONDNAME { get; set; }

                public string CATEGORY { get; set; }

                public string CUR_DESIG { get; set; }

                public string CENTER { get; set; }

                public string CIVIL { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPTIME { get; set; }

                public string TYPE { get; set; }





                #endregion Instance Properties
}
            public class STF015
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string ALLERGIES { get; set; }

                public string BLOODTYPE { get; set; }

                public string BLOODGROUP { get; set; }

                public string REGULARMEDICATION { get; set; }

                public string HOMEBASEAIRPORT { get; set; }

                public string PPCOUNTRY { get; set; }

                public string PPNUMBER { get; set; }

                public string PPISSUEDATE { get; set; }

                public string PPEXPIRY { get; set; }

                public string PPISSUEPLACE { get; set; }

                public string ACNAME { get; set; }

                public string ACADDRESS1 { get; set; }

                public string ACRELATIONSHIP { get; set; }

                public string ACEMAIL { get; set; }

                public string ACTEL { get; set; }

                public string PAYBANK { get; set; }

                public string PAYBANKBRANCH { get; set; }

                public string PAYBANKACCTNO { get; set; }

                public string PAYBANKTYPE { get; set; }

                public string DRVLICNUMBER { get; set; }

                public string DRVLICISSUEDATE { get; set; }

                public string DRVLICEXPIRYDATE { get; set; }

                public string DRVLICCOUNTRY { get; set; }

                public string IDCARDISSUEDATE { get; set; }

                public string IDCARDEXPIRYDATE { get; set; }

                public string IDCARDFUNCTION { get; set; }

                public string IDCARDDISPLAYNAME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string PAYBANKTITLE { get; set; }

                public string PAYBANKSWIFTCODE { get; set; }

                public string PAYBANKIBAN { get; set; }





                #endregion Instance Properties
}
            public class STF016
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string PREVIOUSEMPLOYER1 { get; set; }

                public string TITLE1 { get; set; }

                public string NAME1 { get; set; }

                public string JOBTITLE1 { get; set; }

                public string COMPANY1 { get; set; }

                public string ADDRESS1 { get; set; }

                public string POSTCODE1 { get; set; }

                public string PHONE1 { get; set; }

                public string EMAIL1 { get; set; }

                public string FAX1 { get; set; }

                public string PREVIOUSEMPLOYER2 { get; set; }

                public string TITLE2 { get; set; }

                public string NAME2 { get; set; }

                public string JOBTITLE2 { get; set; }

                public string COMPANY2 { get; set; }

                public string ADDRESS2 { get; set; }

                public string POSTCODE2 { get; set; }

                public string PHONE2 { get; set; }

                public string EMAIL2 { get; set; }

                public string FAX2 { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? INTERNAL1 { get; set; }

                public bool? INTERNAL2 { get; set; }

                public DateTime? SENTDATE1 { get; set; }

                public DateTime? SENTDATE2 { get; set; }

                public string SENDOFFICER1 { get; set; }

                public string SENDOFFICER2 { get; set; }

                public DateTime? RETURNDATE1 { get; set; }

                public DateTime? RETURNDATE2 { get; set; }

                public string RECVDOFFICER1 { get; set; }

                public string RECVDOFFICER2 { get; set; }

                public string COMMENT1 { get; set; }

                public string COMMENT2 { get; set; }





                #endregion Instance Properties
}
            public class STF017
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string DEPARTMENT { get; set; }

                public string SECTION { get; set; }

                public string APPRAISALDETAIL { get; set; }

                public string TRAININGNEED { get; set; }

                public bool? FUTUREAP_DATE { get; set; }

                public string DATECONDUCTED { get; set; }

                public string SURNAME { get; set; }

                public string FIRSTNAME { get; set; }

                public string SECONDNAME { get; set; }

                public string CATEGORY { get; set; }

                public string CUR_DESIG { get; set; }

                public string CENTER { get; set; }

                public string CIVIL { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPTIME { get; set; }

                public string TYPE { get; set; }





                #endregion Instance Properties
}
            public class STF018
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string DEPENDANT { get; set; }

                public string SURNAME { get; set; }

                public string FIRSTNAME { get; set; }

                public string SECONDNAME { get; set; }

                public DateTime? DATEOFBIRTH { get; set; }

                public string GENDER { get; set; }

                public string BLOODGROUP { get; set; }

                public string GENOTYPE { get; set; }

                public string RELATIONSHIP { get; set; }

                public string OCCUPATION { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPTIME { get; set; }





                #endregion Instance Properties
}
            public class STF019
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string EMPLOYEECOMMENT { get; set; }

                public string MANAGER { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPTIME { get; set; }





                #endregion Instance Properties
}
            public class STF019A
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string QUESTIONS { get; set; }

                public string RESPONSE { get; set; }





                #endregion Instance Properties
}
            public class STF020
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string ITEMSRETURNED { get; set; }

                public decimal? QTY { get; set; }

                public string RECEIVEDBY { get; set; }

                public DateTime? RECEIPTDATE { get; set; }

                public string HRCOMMENT { get; set; }





                #endregion Instance Properties
}
            public class STF021
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string LOCATION { get; set; }

                public string CAUSEOFACCIDENT { get; set; }

                public string NATUREOFINJURY { get; set; }

                public string VISITDOCTOR { get; set; }

                public string HOSPITALISED { get; set; }

                public string HOSPITALNAME { get; set; }

                public string ADDRESS { get; set; }

                public string ACTIONSTAKEN { get; set; }

                public string OTHERCOMMENTS { get; set; }

                public string MANAGER { get; set; }

                public string HROFFICER { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class STFRCODE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public decimal? CODE { get; set; }

                public string NAME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class TAX
            {




                #region Instance Properties

                public int RECID { get; set; }

                public decimal? MIN_LIMIT { get; set; }

                public decimal? MAX_LIMIT { get; set; }

                public decimal? FIX_AMOUNT { get; set; }

                public decimal? PERCENTAGE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class TAXES
            {




                #region Instance Properties

                public int RECID { get; set; }

                public decimal? MIN_LIMIT { get; set; }

                public decimal? MAX_LIMIT { get; set; }

                public decimal? FIX_AMOUNT { get; set; }

                public decimal? PERCENTAGE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class TAXHIST
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string COMPANY { get; set; }

                public string STAFF_NO { get; set; }

                public string BRANCH { get; set; }

                public string DEPARTMENT { get; set; }

                public string PERIOD { get; set; }

                public decimal? E_TOTAL { get; set; }

                public decimal? EARN_YTD { get; set; }

                public decimal? FREEPAY { get; set; }

                public decimal? TAX_YTD { get; set; }

                public decimal? TAX_AMT { get; set; }

                public decimal? REFUND { get; set; }

                public DateTime? PPDATE { get; set; }





                #endregion Instance Properties
}
            public class TIMECARD
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string BRANCH { get; set; }

                public DateTime? WORK_DATE { get; set; }

                public string TIME_TYPE { get; set; }

                public decimal? HOURS { get; set; }

                public string JOB_CODE { get; set; }

                public decimal? RATE { get; set; }

                public string REF_NO { get; set; }

                public decimal? AMOUNT { get; set; }

                public bool? UPDATED { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }





                #endregion Instance Properties
}
            public class TIMECUM
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string BRANCH { get; set; }

                public DateTime? WORK_DATE { get; set; }

                public string TIME_TYPE { get; set; }

                public decimal? HOURS { get; set; }

                public string JOB_CODE { get; set; }

                public string COSTS { get; set; }

                public decimal? MULTIPLY { get; set; }

                public decimal? RATE { get; set; }

                public string REF_NO { get; set; }

                public bool? UPDATED { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }





                #endregion Instance Properties
}
            public class TIMEHIST
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string BRANCH { get; set; }

                public DateTime? WORK_DATE { get; set; }

                public string TIME_TYPE { get; set; }

                public decimal? HOURS { get; set; }

                public string JOB_CODE { get; set; }

                public decimal? RATE { get; set; }

                public string REF_NO { get; set; }

                public decimal? AMOUNT { get; set; }

                public bool? UPDATED { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }





                #endregion Instance Properties
}
            public class TIMEP
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string COMPANY { get; set; }

                public string BRANCH { get; set; }

                public DateTime? WORK_DATE { get; set; }

                public string TIME_TYPE { get; set; }

                public decimal? HOURS { get; set; }

                public string JOB_CODE { get; set; }

                public decimal? RATE { get; set; }

                public string REF_NO { get; set; }

                public decimal? AMOUNT { get; set; }

                public bool? UPDATED { get; set; }

                public DateTime? ENT_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPDATETIME { get; set; }





                #endregion Instance Properties
}
        
            public class PAYPERvm
            {
                public BANKS BANKS { get; set; }
                public COMPANY COMPANY { get; set; }
                public COMTAB COMTAB { get; set; }
                public COSTS COSTS { get; set; }
                public CUSTOMER CUSTOMER { get; set; }
                public DEDTAB DEDTAB { get; set; }
                public EATAB EATAB { get; set; }
                public EQUALOPT EQUALOPT { get; set; }
                public EXECHORI EXECHORI { get; set; }
                public GLINT GLINT { get; set; }
                public GLINTDET GLINTDET { get; set; }
                public GLUPDATE GLUPDATE { get; set; }
                public INTGRID INTGRID { get; set; }
                public PAY01 PAY01 { get; set; }
                public PAY02 PAY02 { get; set; }
                public PAY03 PAY03 { get; set; }
                public PAY04 PAY04 { get; set; }
                public PAY04HIST PAY04HIST { get; set; }
                public PAY05 PAY05 { get; set; }
                public PAY05HIST PAY05HIST { get; set; }
                public PAY06 PAY06 { get; set; }
                public PAY07 PAY07 { get; set; }
                public PAY08 PAY08 { get; set; }
                public PAY09 PAY09 { get; set; }
                public PAY10 PAY10 { get; set; }
                public PAY11 PAY11 { get; set; }
                public PAY11A PAY11A { get; set; }
                public PAY12 PAY12 { get; set; }
                public PAYITEM PAYITEM { get; set; }
                public PAYITEMA PAYITEMA { get; set; }
                public PAYTAXITEMS PAYTAXITEMS { get; set; }
                public PPCONTROL PPCONTROL { get; set; }
                public PPSETUP PPSETUP { get; set; }
                public PPSTLEV PPSTLEV { get; set; }
                public PREEMPTAB PREEMPTAB { get; set; }
                public PREINTPA PREINTPA { get; set; }
                public RPTDEFHD RPTDEFHD { get; set; }
                public STF001 STF001 { get; set; }
                public STF001A STF001A { get; set; }
                public STF001B STF001B { get; set; }
                public STF001C STF001C { get; set; }
                public STF002 STF002 { get; set; }
                public STF003 STF003 { get; set; }
                public STF004 STF004 { get; set; }
                public STF005 STF005 { get; set; }
                public STF006 STF006 { get; set; }
                public STF007 STF007 { get; set; }
                public STF008 STF008 { get; set; }
                public STF009 STF009 { get; set; }
                public STF009A STF009A { get; set; }
                public STF009B STF009B { get; set; }
                public STF009C STF009C { get; set; }
                public STF010 STF010 { get; set; }
                public STF010HIST STF010HIST { get; set; }
                public STF011 STF011 { get; set; }
                public STF012 STF012 { get; set; }
                public STF012A STF012A { get; set; }
                public STF013 STF013 { get; set; }
                public STF014 STF014 { get; set; }
                public STF015 STF015 { get; set; }
                public STF016 STF016 { get; set; }
                public STF017 STF017 { get; set; }
                public STF018 STF018 { get; set; }
                public STF019 STF019 { get; set; }
                public STF019A STF019A { get; set; }
                public STF020 STF020 { get; set; }
                public STF021 STF021 { get; set; }
                public STFRCODE STFRCODE { get; set; }
                public TAX TAX { get; set; }
                public TAXES TAXES { get; set; }
                public TAXHIST TAXHIST { get; set; }
                public TIMECARD TIMECARD { get; set; }
                public TIMECUM TIMECUM { get; set; }
                public TIMEHIST TIMEHIST { get; set; }
                public TIMEP TIMEP { get; set; }

















                public IEnumerable<BANKS> BANKSS { get; set; }
                public IEnumerable<COMPANY> COMPANYS { get; set; }
                public IEnumerable<COMTAB> COMTABS { get; set; }
                public IEnumerable<COSTS> COSTSS { get; set; }
                public IEnumerable<CUSTOMER> CUSTOMERS { get; set; }
                public IEnumerable<DEDTAB> DEDTABS { get; set; }
                public IEnumerable<EATAB> EATABS { get; set; }
                public IEnumerable<EQUALOPT> EQUALOPTS { get; set; }
                public IEnumerable<EXECHORI> EXECHORIS { get; set; }
                public IEnumerable<GLINT> GLINTS { get; set; }
                public IEnumerable<GLINTDET> GLINTDETS { get; set; }
                public IEnumerable<GLUPDATE> GLUPDATES { get; set; }
                public IEnumerable<INTGRID> INTGRIDS { get; set; }
                public IEnumerable<PAY01> PAY01S { get; set; }
                public IEnumerable<PAY02> PAY02S { get; set; }
                public IEnumerable<PAY03> PAY03S { get; set; }
                public IEnumerable<PAY04> PAY04S { get; set; }
                public IEnumerable<PAY04HIST> PAY04HISTS { get; set; }
                public IEnumerable<PAY05> PAY05S { get; set; }
                public IEnumerable<PAY05HIST> PAY05HISTS { get; set; }
                public IEnumerable<PAY06> PAY06S { get; set; }
                public IEnumerable<PAY07> PAY07S { get; set; }
                public IEnumerable<PAY08> PAY08S { get; set; }
                public IEnumerable<PAY09> PAY09S { get; set; }
                public IEnumerable<PAY10> PAY10S { get; set; }
                public IEnumerable<PAY11> PAY11S { get; set; }
                public IEnumerable<PAY11A> PAY11AS { get; set; }
                public IEnumerable<PAY12> PAY12S { get; set; }
                public IEnumerable<PAYITEM> PAYITEMS { get; set; }
                public IEnumerable<PAYITEMA> PAYITEMAS { get; set; }
                public IEnumerable<PAYTAXITEMS> PAYTAXITEMSS { get; set; }
                public IEnumerable<PPCONTROL> PPCONTROLS { get; set; }
                public IEnumerable<PPSETUP> PPSETUPS { get; set; }
                public IEnumerable<PPSTLEV> PPSTLEVS { get; set; }
                public IEnumerable<PREEMPTAB> PREEMPTABS { get; set; }
                public IEnumerable<PREINTPA> PREINTPAS { get; set; }
                public IEnumerable<RPTDEFHD> RPTDEFHDS { get; set; }
                public IEnumerable<STF001> STF001S { get; set; }
                public IEnumerable<STF001A> STF001AS { get; set; }
                public IEnumerable<STF001B> STF001BS { get; set; }
                public IEnumerable<STF001C> STF001CS { get; set; }
                public IEnumerable<STF002> STF002SS { get; set; }
                public IEnumerable<STF003> STF003SS { get; set; }
                public IEnumerable<STF004> STF004SS { get; set; }
                public IEnumerable<STF005> STF005SS { get; set; }
                public IEnumerable<STF006> STF006SS { get; set; }
                public IEnumerable<STF007> STF007S { get; set; }
                public IEnumerable<STF008> STF008S { get; set; }
                public IEnumerable<STF009> STF009S { get; set; }
                public IEnumerable<STF009A> STF009AS { get; set; }
                public IEnumerable<STF009B> STF009BS { get; set; }
                public IEnumerable<STF009C> STF009CS { get; set; }
                public IEnumerable<STF010> STF010S { get; set; }
                public IEnumerable<STF010HIST> STF010HISTS { get; set; }
                public IEnumerable<STF011> STF011S { get; set; }
                public IEnumerable<STF012> STF012S { get; set; }
                public IEnumerable<STF012A> STF012AS { get; set; }
                public IEnumerable<STF013> STF013SS { get; set; }
                public IEnumerable<STF014> STF014SS { get; set; }
                public IEnumerable<STF015> STF015SS { get; set; }
                public IEnumerable<STF016> STF016SS { get; set; }
                public IEnumerable<STF017> STF017S { get; set; }
                public IEnumerable<STF018> STF018S { get; set; }
                public IEnumerable<STF019> STF019S { get; set; }
                public IEnumerable<STF019A> STF019AS { get; set; }
                public IEnumerable<STF020> STF020S { get; set; }
                public IEnumerable<STF021> STF021S { get; set; }
                public IEnumerable<STFRCODE> STFRCODES { get; set; }
                public IEnumerable<TAX> TAXS { get; set; }
                public IEnumerable<TAXES> TAXESS { get; set; }
                public IEnumerable<TAXHIST> TAXHISTS { get; set; }
                public IEnumerable<TIMECARD> TIMECARDS { get; set; }
                public IEnumerable<TIMECUM> TIMECUMS { get; set; }
                public IEnumerable<TIMEHIST> TIMEHISTS { get; set; }
                public IEnumerable<TIMEP> TIMEPS { get; set; }























            }

        
    }
}