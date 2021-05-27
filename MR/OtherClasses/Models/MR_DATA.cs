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

namespace OtherClasses.Models
{
    public class MR_DATA
    {
            public class REPORTS
            {
                public bool REPORT_BY_DATE { get; set; }
                public bool PRINT { get; set; }
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
                public bool chkReportGroupFamily { get; set; }
                public bool chkIncludeGroupInvNo { get; set; }
                public bool chkIncludeBf { get; set; }
                public bool chkPrintServiceDetailsonBills { get; set; }
                public bool chkADVCorporate { get; set; }
                public bool chkADVIncludePVTFC { get; set; }
                public bool chkADVunpaidmiscbills { get; set; }
                public bool chkPMTCashierChronological { get; set; }
                public bool chkIncludeProcessHMONHIS { get; set; }
                public bool chkCummulativeSumm { get; set; }
                public bool chkExcludeRequests { get; set; }
                public bool chkGrpbyServiceCentre { get; set; }
                public bool chkSegmented { get; set; }
                public bool chkIncludeOnHold { get; set; }
                public bool chkCurrtAdmRev { get; set; }
                public bool chkReportSum { get; set; }
                public bool chkQueryTimeofDay { get; set; }
                public bool chkByDateRange { get; set; }
                public bool chkHMO { get; set; }
                public bool chkAuditProfile { get; set; }
                public bool chkApplyFilter { get; set; }
                public bool chkBasicMedicProfile { get; set; }

                public string ActRslt { get; set; }
                public string REPORT_TYPE1 { get; set; }
                public string REPORT_TYPE2 { get; set; }
                public string REPORT_TYPE3 { get; set; }
                public string REPORT_TYPE4 { get; set; }
                public string REPORT_TYPE5 { get; set; }

                public string Searchdesc { get; set; }
                public string SearchName { get; set; }
                public string txtTimeTo { get; set; }
                public string txtTimeFrom { get; set; }
                public string cboTypeofBirth { get; set; }
                public string cboDeliveryType { get; set; }
                public string cboResidence { get; set; }
                public decimal nmrMinBalance { get; set; }
                public int nmr90days { get; set; }
                public int nmr60days { get; set; }
                public int nmr30days { get; set; }
                public int nmrAmountFrom { get; set; }
                public int nmrAmountTo { get; set; }
                public int nmrPayRefFrom { get; set; }
                public int nmrPayRefTo { get; set; }
                public decimal nmrAgeFrom { get; set; }
                public decimal nmrAgeTo { get; set; }

                public DateTime? TRANS_DATE1 { get; set; }
                public DateTime? TRANS_DATE2 { get; set; }

                public string cboPVTNameFrom { get; set; }
                public string cboPVTNameTo { get; set; }
                public string RptPath { get; set; }
                public string PdfPath { get; set; }
                public string SessionSQL { get; set; }
                public string SessionOCP { get; set; }
                public string SessionXSORT { get; set; }
                public string SessionRDLC { get; set; }
                public string SessionCustomer { get; set; }
                public string SessionCustomertype { get; set; }
                public string SessionIncludebf { get; set; }
                public string SessionInv_dtl { get; set; }
                public string SessionMhead { get; set; }
                public string SessionHeaderleftjustify { get; set; }
                public string SessionInvfooter { get; set; }
                public string SessionWaitonly { get; set; }
                public string SessionPayments { get; set; }
                public decimal SessionBalbf { get; set; }
                public string SessionIsbf { get; set; }
                public string SessionIspayment { get; set; }
                public string SessionIsadjust { get; set; }
                public string SessionIscur_bal { get; set; }
                public string SessionIsbalance { get; set; }
                public string SessionMdate { get; set; }
                public string SessionOff { get; set; }
                public string SessionOncall { get; set; }
                public string SessionNight { get; set; }
                public string SessionAfternoon { get; set; }
                public string SessionMorning { get; set; }
                public string SessionBis { get; set; }
                public string SessionDs { get; set; }
                public string[] SessionDatea_ { get; set; }
                public string[] SessionAddress_ { get; set; }
                public string[] SessionDd { get; set; }
                public string[] SessionMorninga_ { get; set; }
                public string[] SessionAfternoona_ { get; set; }
                public string[] SessionNighta_ { get; set; }
                public string[] SessionOncalla_ { get; set; }
                public string[] SessionOffa_ { get; set; }
                public string[] SessionDsa_ { get; set; }
                public string[] SessionBsa_ { get; set; }

                public ReportParameter[] RptParams { get; set; }
                public ReportDataSource RptDataSrc { get; set; }
                public ReportViewer GeneratedReport { get; set; }
            


                //NEW PROPS
                public string edtspinstructions { get; set; }
                public string edtallergies { get; set; }
                public bool btnchainedmedhistory { get; set; }
                public bool btnFamilyGroup { get; set; }
                public bool cmbdelete { get; set; }
                public bool cmdgrpmember { get; set; }
                public bool chkbillregistration { get; set; }
                public bool chkgetdependants { get; set; }
                public bool cmbsave { get; set; }
                public string cboAge { get; set; }
                public string lblbillonaccount { get; set; }
                public string txtgroupcode { get; set; }
                public string txtpatientno { get; set; }
                public string txtaddress1 { get; set; }
                public DateTime dtbirthdate { get; set; }
                public string cbogender { get; set; }
                public string cbomaritalstatus { get; set; }
                public DateTime dtregistered { get; set; }
                public string txtcontactperson { get; set; }
                public string txtghgroupcode { get; set; }
                public string cbotype { get; set; }
                public string txtcreditlimit { get; set; }
                public string combillcycle { get; set; }
                public decimal nmrBalbf { get; set; }
                public string lblBalbfDbCr { get; set; }
                public string TXTPATIENTNAME { get; set; }
                public string txtbillspayable { get; set; }
                public string cbotitle { get; set; }
                public string Combillspayable { get; set; }
                public decimal txtdiscount { get; set; }
                public string comhmoservgrp { get; set; }
                public string txtcurrency { get; set; }
                public string txtclinic { get; set; }
                public string txtsurname { get; set; }
                public string txtothername { get; set; }
                public string txthomephone { get; set; }
                public string txtworkphone { get; set; }
                public string txtemployer { get; set; }
                public string txtemployeraddress { get; set; }
                public string cboemploystate { get; set; }
                public string cbooccupation { get; set; }
                public string cbobloodgroup { get; set; }
                public string cbogenotype { get; set; }
                public string txtnextofkin { get; set; }
                public string txtkinaddress1 { get; set; }
                public string cbokinstate { get; set; }
                public string txtkinphone { get; set; }
                public string txtrelationship { get; set; }
                public string txtemail { get; set; }
                public string cboReligion { get; set; }
                public string cboTribe { get; set; }
                public string cboLGA { get; set; }
                public string pictureBox1 { get; set; }
                public decimal nmrcurcredit { get; set; }
                public decimal nmrcurdebit { get; set; }
                public decimal nmrbalance { get; set; }
                public string lblDbCr { get; set; }
                public bool nmrBalbfReadOnly { get; set; }
                public string lblbillspayable { get; set; }
                public bool newrec { get; set; }
                public string newrecString { get; set; }
                public bool mcanalter { get; set; }
                public string alertMessage { get; set; }
                public string imageFile { get; set; }
                public string categ_save { get; set; }
                public string lblStaffNumber { get; set; }
                public string txtbillonacct { get; set; }
                public string txtstaffno { get; set; }
                public string txtbranch { get; set; }
                public string doctor { get; set; }
                public string txtdepartment { get; set; }
                public string txtreference { get; set; }
                public string txtconsultamt { get; set; }
                public decimal nmrattendancetoday { get; set; }
                public string txtgrouphead { get; set; }
                public string combFacility { get; set; }
                public decimal mlastno { get; set; }
                public string mpatientno { get; set; }
                public string mgroupcode { get; set; }
                public decimal nmrPayable { get; set; }
                public string mgrouphead { get; set; }
                public string mghgroupcode { get; set; }
                public string mreference { get; set; }
                public string msection { get; set; }
                public string mcusttype { get; set; }
                public string mgrouphtype { get; set; }
                public DateTime transDate { get; set; }
                public decimal? cost { get; set; }
                public decimal? sell { get; set; }









        }



        public class ADMDETAI
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TIME { get; set; }

                public string MASTPROCESS { get; set; }

                public string PROCESS { get; set; }

                public string STK_ITEM { get; set; }

                public string DESCRIPTION { get; set; }

                public string UNIT { get; set; }

                public decimal? QTY { get; set; }

                public decimal? AMOUNT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_TIME { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string DOCTOR { get; set; }

                public string FACILITY { get; set; }

                public string STORE { get; set; }





                #endregion Instance Properties
}

            public class ADMRECS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public string FACILITY { get; set; }

                public string UNIT { get; set; }

                public string ROOM { get; set; }

                public string BED { get; set; }

                public decimal? RATE { get; set; }

                public DateTime? ADM_DATE { get; set; }

                public string TIME { get; set; }

                public string DOCTOR { get; set; }

                public string DIAGNOSIS { get; set; }

                public string DISCHARGE { get; set; }

                public string DISCH_TIME { get; set; }

                public string DISCH_DOCT { get; set; }

                public bool? BILLED { get; set; }

                public DateTime? DATE_BILLE { get; set; }

                public string REMARKS { get; set; }

                public string REASON { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GROUPHTYPE { get; set; }

                public string GROUPCODE { get; set; }

                public decimal? ACAMT { get; set; }

                public string GHGROUPCODE { get; set; }

                public decimal? DAILYFEEDING { get; set; }

                public decimal? DAILYPNC { get; set; }

                public decimal? PAYMENTS { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string DIAGNOSIS_ALL { get; set; }





                #endregion Instance Properties
}
            public class ADMSPACE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string FACILITY { get; set; }

                public string NAME { get; set; }

                public string ROOM { get; set; }

                public string BED { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? RATE { get; set; }

                public string OCCUPANT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? BOOKED { get; set; }

                public DateTime? BOOKEDDATE { get; set; }

                public decimal? NURSINGCARE { get; set; }

                public decimal? adm_deposit { get; set; }





                #endregion Instance Properties
}
            public class ANC01
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPHEAD { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string NAME { get; set; }

                public DateTime? LMP { get; set; }

                public DateTime? EDD { get; set; }

                public string BLOODGROUP { get; set; }

                public DateTime? DEL_DATE { get; set; }

                public decimal? CUMMCHARGE { get; set; }

                public decimal? PAYMENTS { get; set; }

                public decimal? CHARGE { get; set; }

                public DateTime? LASTATTEND { get; set; }

                public DateTime? NEXTVISIT { get; set; }

                public decimal? CUMMATTEND { get; set; }

                public string DRUG1 { get; set; }

                public string DRUG2 { get; set; }

                public string DRUG3 { get; set; }

                public string DRUG4 { get; set; }

                public string DRUG5 { get; set; }

                public string DRUG6 { get; set; }

                public string DRUG7 { get; set; }

                public string DRUG8 { get; set; }

                public string DRUG9 { get; set; }

                public string DRUG10 { get; set; }

                public bool? ALLDRUGS { get; set; }

                public string SERVICE1 { get; set; }

                public string SERVICE2 { get; set; }

                public string SERVICE3 { get; set; }

                public string SERVICE4 { get; set; }

                public string SERVICE5 { get; set; }

                public string SERVICE6 { get; set; }

                public string SERVICE7 { get; set; }

                public string SERVICE8 { get; set; }

                public string SERVICE9 { get; set; }

                public string SERVICE10 { get; set; }

                public bool? ALLSERVICE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public DateTime? REG_DATE { get; set; }

                public string REG_TIME { get; set; }

                public string DOCTOR { get; set; }

                public string DURATIONOFPREGNANCY { get; set; }

                public decimal? AGE { get; set; }

                public string TRIBE { get; set; }

                public string RELIGION { get; set; }

                public string ADDRESS { get; set; }

                public string OCCUPATION { get; set; }

                public decimal? LEVELOFEDUCATION { get; set; }

                public string HUSBANDNAME { get; set; }

                public string HUSBANDOCCUPATION { get; set; }

                public string HUSBANDEMPLOYER { get; set; }

                public decimal? HUSBANDLEVELOFEDUCATION { get; set; }

                public string HUSBANDPHONE { get; set; }

                public string HUSBANDGC { get; set; }

                public string HUSBANDPATNO { get; set; }

                public string HUSBANDBG { get; set; }

                public string BOOKINGCATEGORY { get; set; }

                public decimal? BOOKINGTAG { get; set; }

                public string GHGROUPCODE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string SPNOTES { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public string GENOTYPE { get; set; }

                public string MENS_REGULARITY { get; set; }

                public string CONTRACEPTIVEUSE { get; set; }

                public string RISKFACTOR { get; set; }

                public string HUSBANDGENOTYPE { get; set; }

                public string MENARCHE { get; set; }





                #endregion Instance Properties
}
            public class ANC02
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPHEAD { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string DIABETES { get; set; }

                public string HYPERTENSION { get; set; }

                public string HEART_DISEASE { get; set; }

                public string SICKLE_CELL { get; set; }

                public string PULMONARY { get; set; }

                public string KIDNEYDISEASE { get; set; }

                public string HEPATITIS { get; set; }

                public decimal? PREV_PREG_TOTAL { get; set; }

                public decimal? NOALIVE { get; set; }

                public string NEUROLOGIC { get; set; }

                public string THYROID { get; set; }

                public string PSYCHIATRIC { get; set; }

                public string DEPRESSION { get; set; }

                public string VARICOSITIES { get; set; }

                public string D_RH_SENSITIZATION { get; set; }

                public string BLOOD_TRANSFUSIONS { get; set; }

                public string HIV { get; set; }

                public string BREAST_LUMPS { get; set; }

                public string GYNESURGERIES { get; set; }

                public string DRUG_ALLERGIES { get; set; }

                public string OPERATIONS { get; set; }

                public string ANAESTHETIC { get; set; }

                public string PAPSMEAR { get; set; }

                public string INFERTILITY { get; set; }

                public string OTHERS { get; set; }

                public string ALCOHOL { get; set; }

                public string SMOKING { get; set; }

                public string SOCIALDETAILS { get; set; }

                public string FAM_HYPERTENSION { get; set; }

                public string FAM_DIABETES { get; set; }

                public string FAM_SICKLE_CELL { get; set; }

                public string FAM_GENETIC { get; set; }

                public string FAM_OTHERS { get; set; }

                public string FAMILYDETAILS { get; set; }

                public string AP_PROGUANIL { get; set; }

                public string AP_PYRIMETHAMINE1 { get; set; }

                public string AP_PYRIMETHAMINE2 { get; set; }

                public string AP_PYRIMETHAMINE3 { get; set; }

                public string AP_OTHERS { get; set; }

                public string TETANUS1 { get; set; }

                public string TETANUS2 { get; set; }

                public string TETANUS3 { get; set; }

                public string RECREATIONDRGS { get; set; }

                public string TWINNING { get; set; }





                #endregion Instance Properties
}
            public class ANC03
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? PREV_PREG_TOTAL { get; set; }

                public decimal? NOALIVE { get; set; }

                public DateTime? DATEOFBIRTH { get; set; }

                public string DURATIONOFPREG { get; set; }

                public string BIRTHWT { get; set; }

                public string PLACEOFBIRTH { get; set; }

                public string PREG_LABOUR { get; set; }

                public string AGEATDEATH { get; set; }

                public string CAUSEOFDEATH { get; set; }

                public string MTHOFBIRTH { get; set; }

                public string SEX { get; set; }





                #endregion Instance Properties
}
            public class ANC03A
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? AGEATEDD { get; set; }

                public bool? SICKLECELL { get; set; }

                public bool? DOWNS { get; set; }

                public bool? CHROMOSOMAL { get; set; }

                public bool? HEARTDISEASE { get; set; }

                public bool? METABOLIC { get; set; }

                public bool? TUBEDEFECT { get; set; }

                public bool? STILLBIRTH { get; set; }

                public bool? MEDICATIONS { get; set; }

                public bool? TB { get; set; }

                public bool? HERPES { get; set; }

                public bool? VIRALILLNESS { get; set; }

                public bool? STI { get; set; }

                public bool? HEPATITISB { get; set; }

                public string INDEXPREG { get; set; }

                public string MEDICATIONDETL { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }





                #endregion Instance Properties
}
            public class ANC04
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string HEIGHT { get; set; }

                public string WEIGHT { get; set; }

                public string TEMP { get; set; }

                public string BP { get; set; }

                public decimal? HEENT { get; set; }

                public decimal? FUNDI { get; set; }

                public decimal? TEETH { get; set; }

                public decimal? THYROID { get; set; }

                public decimal? BREASTS { get; set; }

                public decimal? LUNGS { get; set; }

                public decimal? HEART { get; set; }

                public decimal? ABDOMEN { get; set; }

                public decimal? EXTREMITIES { get; set; }

                public decimal? SKIN { get; set; }

                public decimal? LYMPHNODES { get; set; }

                public decimal? VULVA { get; set; }

                public decimal? VAGINA { get; set; }

                public decimal? CERVIX { get; set; }

                public string UTERINESIZE { get; set; }

                public decimal? FIBROIDS { get; set; }

                public decimal? ADNEXA { get; set; }

                public decimal? HAEMORRHOIDS { get; set; }

                public string COMMENTS { get; set; }

                public string DELPLAN { get; set; }

                public string INTERVIEWER { get; set; }

                public string PULSE { get; set; }

                public string RESPIRATION { get; set; }





                #endregion Instance Properties
}
            public class ANC05
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string URINE { get; set; }

                public string HB_LEVEL { get; set; }

                public string BLOOD_GENOTYPE { get; set; }

                public string BP { get; set; }

                public string BLOOD_GP_P_RH { get; set; }

                public string XRAY { get; set; }

                public string BREASTS_NIPPLES { get; set; }

                public string HISTORYOFPRESENT_PREG { get; set; }

                public string PHYSICALEXAM { get; set; }

                public string RESPIRATORYSYSTEM { get; set; }

                public string CARDIO_VASCULAR { get; set; }

                public string HEIGHT { get; set; }

                public string SPLEEN { get; set; }

                public string WEIGHT { get; set; }

                public string LIVER { get; set; }

                public string VAGINAL_EXAM { get; set; }

                public string COMMENTS { get; set; }

                public string ABDOMEN { get; set; }

                public string SPECIALINSTRUCTIONS { get; set; }

                public string DOCTOR { get; set; }

                public string XRAYSCAN { get; set; }





                #endregion Instance Properties
}
            public class ANC06
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string HIGHT_OF_FUNDUS { get; set; }

                public string PRESENTATION_POSITION { get; set; }

                public string RELATION_OF_PP_TOBRIM { get; set; }

                public string FOETAL_HEART { get; set; }

                public string URINE { get; set; }

                public string BLOOD_PRESSURE { get; set; }

                public string WEIGHT { get; set; }

                public string HB { get; set; }

                public string OEDEMA { get; set; }

                public string REMARKS_TREATMENT { get; set; }

                public DateTime? NEXTVISIT { get; set; }

                public string DOCTOR { get; set; }

                public decimal? NNV { get; set; }

                public string ATTENDREF { get; set; }

                public string GESTATIONALAGE { get; set; }





                #endregion Instance Properties
}
            public class ANC07
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? ADMITTED { get; set; }

                public string CONSOBS { get; set; }

                public string ATTENDPAED { get; set; }

                public string DELSUITE { get; set; }

                public DateTime? DELDATE { get; set; }

                public string DELTIME { get; set; }

                public string PARITY { get; set; }

                public string GESTAGE { get; set; }

                public string FETALNUMBER { get; set; }

                public bool? LO_NONE { get; set; }

                public bool? LO_SPONTANEOUS { get; set; }

                public bool? LO_INDUCED { get; set; }

                public bool? LO_AUGUMENTD { get; set; }

                public string INDICANTIONS { get; set; }

                public DateTime? ROMDATE { get; set; }

                public string ROMTIME { get; set; }

                public bool? ROM_INDUCED { get; set; }

                public bool? ROM_ACTIFICIAL { get; set; }

                public string ROM_INDICATIONS { get; set; }

                public string ROM_DURATION { get; set; }

                public bool? PR_NONE { get; set; }

                public bool? PR_NARCOTICS { get; set; }

                public bool? PR_PRUDENDAL { get; set; }

                public bool? PR_ENTONOX { get; set; }

                public bool? PR_EPIDURAL { get; set; }

                public bool? PR_SPINAL { get; set; }

                public bool? PR_COMBINED { get; set; }

                public DateTime? LABONSETDT { get; set; }

                public string LABPNSETTIME { get; set; }

                public DateTime? LABFDDT { get; set; }

                public string LABFDTIME { get; set; }

                public DateTime? LABPCDT { get; set; }

                public string LABPCTIME { get; set; }

                public DateTime? LABHDDT { get; set; }

                public string LABHDTIME { get; set; }

                public DateTime? LABBDDT { get; set; }

                public string LABBDTIME { get; set; }

                public DateTime? LABEOTSDT { get; set; }

                public string LABEOTSTIME { get; set; }

                public DateTime? LABT2DDT { get; set; }

                public string LAB2DTIME { get; set; }

                public string FSTSTAGEHRMIN { get; set; }

                public string SSTSTAGEHRMIN { get; set; }

                public string TSTSTAGEHRMIN { get; set; }

                public string LABDURATION { get; set; }

                public bool? TSTAGEM_A { get; set; }

                public bool? TSTAGEM_M { get; set; }

                public string TSTAGEMGTNOTES { get; set; }

                public bool? OXYTOCICS { get; set; }

                public bool? EGOMETRINE { get; set; }

                public string OXYTOCICSDTM { get; set; }

                public string CORD { get; set; }

                public decimal? MEMBRANES { get; set; }

                public decimal? PLACENTA { get; set; }

                public string BLMEASURE { get; set; }

                public string BLESTIMATES { get; set; }

                public string BLTOTAL { get; set; }

                public string FURTHERACTN { get; set; }

                public bool? TRAUMA_NONE { get; set; }

                public bool? CERVICAL_TEAR { get; set; }

                public bool? PERINEAL_TEAR { get; set; }

                public decimal? TEARDEGREE { get; set; }

                public bool? EPISIOTOMY { get; set; }

                public string INDI4EPISIOTOMY { get; set; }

                public decimal? REPREQ { get; set; }

                public decimal? MOTHERAGREE { get; set; }

                public string ANAESTHUSED { get; set; }

                public string STAFF { get; set; }

                public DateTime? TRDTTIME { get; set; }

                public string BAB1BY { get; set; }

                public string BAB2BY { get; set; }

                public string BAB3BY { get; set; }

                public string COMMENTS { get; set; }





                #endregion Instance Properties
}
            public class ANC07A
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GESTAGE { get; set; }

                public string PARITY { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string PROCESS { get; set; }

                public string INDICATIONS { get; set; }

                public string STAFFPRESENT { get; set; }

                public string SURGEON { get; set; }

                public string ASSISTANT { get; set; }

                public string PAEDIATRICIAN { get; set; }

                public string MIDWIVES { get; set; }

                public string ANAESTHETIST { get; set; }

                public string OTHERS { get; set; }

                public string ANAESTHESIA { get; set; }

                public string FINDINGS { get; set; }

                public string PROCEDURENOTE { get; set; }

                public string MOTHER { get; set; }

                public string BABY { get; set; }

                public string STAFFSIGN { get; set; }





                #endregion Instance Properties
}
            public class ANC07B
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TEMP { get; set; }

                public string PR { get; set; }

                public string BP { get; set; }

                public string SP02 { get; set; }

                public string UTERUS { get; set; }

                public string LOCIA { get; set; }

                public string WOUNDS { get; set; }

                public string PERINEUM { get; set; }

                public string URINE { get; set; }

                public string STAFFSIGN { get; set; }





                #endregion Instance Properties
}
            public class ANC07C
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string BWEIGHT { get; set; }

                public string SEX { get; set; }

                public decimal? BIRTHTYPE { get; set; }

                public string GESTAGE { get; set; }

                public string MODEOFRESUSC { get; set; }

                public string DRUGS { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string EXAMTIME { get; set; }

                public string HR { get; set; }

                public string RR { get; set; }

                public string TEMP { get; set; }

                public string OFC { get; set; }

                public string LENGTH { get; set; }

                public string PALOR { get; set; }

                public string CYANOSIS { get; set; }

                public string JAUNDICE { get; set; }

                public string R_DISTRESS { get; set; }

                public decimal? APPEARANCE { get; set; }

                public string APPEARANCENOTE { get; set; }

                public decimal? HEAD { get; set; }

                public string HEADNOTE { get; set; }

                public decimal? EARS { get; set; }

                public string EARSNOTE { get; set; }

                public decimal? EYES { get; set; }

                public string EYESNOTE { get; set; }

                public decimal? NOSE { get; set; }

                public string NOSENOTE { get; set; }

                public decimal? MOUTH { get; set; }

                public string MOUTHNOTE { get; set; }

                public decimal? RESPSYSTEM { get; set; }

                public string RESPSYSTEMNOTE { get; set; }

                public decimal? CARDIOSYSTEM { get; set; }

                public string CARDIOSYSTEMNOTE { get; set; }

                public decimal? ABDOMEN { get; set; }

                public string ABDOMENNOTE { get; set; }

                public decimal? FEMORALS { get; set; }

                public string FEMORALSNOTE { get; set; }

                public decimal? ANUS { get; set; }

                public string ANUSNOTE { get; set; }

                public decimal? GENITALIA { get; set; }

                public string GENITALIANOTE { get; set; }

                public decimal? EXTEMITES { get; set; }

                public string EXTREMITESNOTE { get; set; }

                public decimal? HIPS { get; set; }

                public string HIPSNOTE { get; set; }

                public decimal? CNS { get; set; }

                public string CNSNOTE { get; set; }

                public string OTHERFINDINGS { get; set; }

                public string COMMENTS { get; set; }

                public string STAFFSIGN { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class ANC07D
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string BP { get; set; }

                public string PR { get; set; }

                public string URINALYSIS { get; set; }

                public string COMMENTS { get; set; }

                public DateTime? NEXTVISIT { get; set; }

                public string DOCTOR { get; set; }

                public decimal? NNV { get; set; }

                public string ATTENDREF { get; set; }





                #endregion Instance Properties
}
            public class ANC08
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string FACILITY { get; set; }

                public string PROCESS { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string RESULT { get; set; }





                #endregion Instance Properties
}
            public class ANC09
            {




                #region Instance Properties

                public int RECID { get; set; }

                public bool? HIV { get; set; }

                public string HIV_TEXT { get; set; }

                public bool? RISKFACTOR { get; set; }

                public string RISKFA_TEXT { get; set; }

                public bool? PRENATAL { get; set; }

                public string PRENAT_TEXT { get; set; }

                public bool? NUTRITION { get; set; }

                public string NUTRI_TEXT { get; set; }

                public bool? SEXUAL { get; set; }

                public string SEXU_TEXT { get; set; }

                public bool? EXERCISE { get; set; }

                public string EXERC_TEXT { get; set; }

                public bool? ENVIRONMENTAL { get; set; }

                public string ENVIRO_TEXT { get; set; }

                public bool? TOBACCO { get; set; }

                public string TOBACC_TEXT { get; set; }

                public bool? ALCOHOL { get; set; }

                public string ALCOHO_TEXT { get; set; }

                public bool? ILLICITDRUGS { get; set; }

                public string IL_DRG_TEXT { get; set; }

                public bool? MEDICATIONS { get; set; }

                public string MEDIC_TEXT { get; set; }

                public bool? ULTRASOUND { get; set; }

                public string ULTRAS_TEXT { get; set; }

                public bool? DOMESTIC { get; set; }

                public string DOMES_TEXT { get; set; }

                public bool? SEATBELT { get; set; }

                public string SEATBELT_TEXT { get; set; }

                public bool? CHILDBIRTH { get; set; }

                public string CHILDB_TEXT { get; set; }

                public bool? PRETERM { get; set; }

                public string PRETER_TEXT { get; set; }

                public bool? ABNORMAL { get; set; }

                public string AB_LAB_TEXT { get; set; }

                public bool? POSTPARTUM_FAM_PLAN { get; set; }

                public string PPARTUM_FAM_PLANNING { get; set; }

                public bool? ANAESTHESIA { get; set; }

                public string ANAES_TEXT { get; set; }

                public bool? FETAL { get; set; }

                public string FETAL_TEXT { get; set; }

                public bool? LABOURSIGNS { get; set; }

                public string LABOU_TEXT { get; set; }

                public bool? VBAC { get; set; }

                public string VBAC_TEXT { get; set; }

                public bool? PIH { get; set; }

                public string PIH_TEXT { get; set; }

                public bool? POSTTERM { get; set; }

                public string POSTT_TEXT { get; set; }

                public bool? CIRCUMCISION { get; set; }

                public string CIRUMC_TEXT { get; set; }

                public bool? BREASTFEED { get; set; }

                public string BREAST_TEXT { get; set; }

                public bool? POSPARTUM_DEPRESSION { get; set; }

                public string PPDEPR_TEXT { get; set; }

                public bool? NEWBORN { get; set; }

                public string NEWBOR_TEXT { get; set; }

                public string REFERENCE { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class ANCEXEMP
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string DRUGS1 { get; set; }

                public string DRUGS2 { get; set; }

                public string DRUGS3 { get; set; }

                public string DRUGS4 { get; set; }

                public string DRUGS5 { get; set; }

                public string DRUGS6 { get; set; }

                public string DRUGS7 { get; set; }

                public string DRUGS8 { get; set; }

                public string DRUGS9 { get; set; }

                public string DRUGS10 { get; set; }

                public bool? ALLDRUGS { get; set; }

                public string SERVICE1 { get; set; }

                public string SERVICE2 { get; set; }

                public string SERVICE3 { get; set; }

                public string SERVICE4 { get; set; }

                public string SERVICE5 { get; set; }

                public string SERVICE6 { get; set; }

                public string SERVICE7 { get; set; }

                public string SERVICE8 { get; set; }

                public string SERVICE9 { get; set; }

                public string SERVICE10 { get; set; }

                public bool? ALLSERVICE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public DateTime? REG_DATE { get; set; }

                public string REG_TIME { get; set; }





                #endregion Instance Properties
}
            public class ANCREG
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPHEAD { get; set; }

                public string GHGROUPCODE { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string NAME { get; set; }

                public DateTime? LMP { get; set; }

                public DateTime? EDD { get; set; }

                public DateTime? DEL_DATE { get; set; }

                public decimal? CUMMCHARGE { get; set; }

                public decimal? PAYMENTS { get; set; }

                public decimal? CHARGE { get; set; }

                public DateTime? LASTATTEND { get; set; }

                public int? CUMMATTEND { get; set; }

                public string DRUG1 { get; set; }

                public string DRUG2 { get; set; }

                public string DRUG3 { get; set; }

                public string DRUG4 { get; set; }

                public string DRUG5 { get; set; }

                public string DRUG6 { get; set; }

                public string DRUG7 { get; set; }

                public string DRUG8 { get; set; }

                public string DRUG9 { get; set; }

                public string DRUG10 { get; set; }

                public bool? ALLDRUGS { get; set; }

                public string SERVICE1 { get; set; }

                public string SERVICE2 { get; set; }

                public string SERVICE3 { get; set; }

                public string SERVICE4 { get; set; }

                public string SERVICE5 { get; set; }

                public string SERVICE6 { get; set; }

                public string SERVICE7 { get; set; }

                public string SERVICE8 { get; set; }

                public string SERVICE9 { get; set; }

                public string SERVICE10 { get; set; }

                public bool? ALLSERVICE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public DateTime? REG_DATE { get; set; }

                public string REG_TIME { get; set; }

                public string ANCHISTORY { get; set; }

                public string ANTENATALNOTES { get; set; }

                public string DELIVERYNOTES { get; set; }

                public string POSTNATALNOTES { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public bool? EVERYVISITCONSULT { get; set; }

                public decimal? CONSULTAMT { get; set; }





                #endregion Instance Properties
}
            public class APGARSCORE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string HEART_RATE_1 { get; set; }

                public string RESPIRATIONEFF_1 { get; set; }

                public string MUSCLE_TONE_1 { get; set; }

                public string REFLEX_1 { get; set; }

                public string COLOR_1 { get; set; }

                public string TOTAL_1 { get; set; }

                public string HEART_RATE_5 { get; set; }

                public string RESPIRATIONEFF_5 { get; set; }

                public string MUSCLE_TONE_5 { get; set; }

                public string REFLEX_5 { get; set; }

                public string COLOR_5 { get; set; }

                public string TOTAL_5 { get; set; }

                public string HEART_RATE_OTH { get; set; }

                public string RESPIRATIONEFF_OTH { get; set; }

                public string MUSCLE_TONE_OTH { get; set; }

                public string REFLEX_OTH { get; set; }

                public string COLOR_OTH { get; set; }

                public string TOTAL_OTH { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string REFERENCE { get; set; }





                #endregion Instance Properties
}
            public class APPT
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string USERID { get; set; }

                public DateTime? DATE { get; set; }

                public string TIME { get; set; }

                public string ENDTIME { get; set; }

                public string BRIEF { get; set; }

                public string COMMENTS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string FACILITY { get; set; }

                public string ROOM { get; set; }

                public string BED { get; set; }

                public string PROCESS { get; set; }

                public bool? ISNOTE { get; set; }

                public string USERID_PHONE { get; set; }

                public string USERID_EMAIL { get; set; }

                public string BENEFICIARY { get; set; }

                public string BENEFICIARY_PHONE { get; set; }

                public string BENEFICIARY_EMAIL { get; set; }





                #endregion Instance Properties
}
            public class ASTNOTES
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string GENDER { get; set; }

                public string WLBS { get; set; }

                public string WSTONE { get; set; }

                public decimal? WEIGHT { get; set; }

                public string HFT { get; set; }

                public string HIN { get; set; }

                public decimal? HIGHT { get; set; }

                public string BP { get; set; }

                public string PULSE { get; set; }

                public string TEMP { get; set; }

                public string RESPIRATIO { get; set; }

                public decimal? BMP { get; set; }

                public string CLINIC { get; set; }

                public string DOCTOR { get; set; }

                public string TIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string HAIR { get; set; }

                public string EYE { get; set; }

                public string SKIN { get; set; }

                public string RACIALGRP { get; set; }

                public string ETHNICITY { get; set; }

                public string RELIGION { get; set; }

                public string EARS { get; set; }

                public string NOSE { get; set; }

                public string HEAD_NECK { get; set; }

                public string MOUTH { get; set; }

                public string THROAT { get; set; }

                public string LUNGS { get; set; }

                public string DIAGNOSIS { get; set; }

                public string CC { get; set; }

                public string INTERVALHISORY { get; set; }

                public string SPACERUSER { get; set; }

                public string PEAKFLOW { get; set; }

                public string O2SAT { get; set; }

                public bool? CIGARETTEXPO { get; set; }

                public string CURRENTMED { get; set; }

                public string HOSPITALEDVISIT { get; set; }

                public string EXERCISETOLERANCE { get; set; }

                public bool? DRUG_ALLERGY { get; set; }

                public bool? ENVIRON_ALLERGY { get; set; }

                public bool? PETS_ALLERGY { get; set; }

                public string DELPLAN { get; set; }

                public DateTime? FOLLOWUPVISIT { get; set; }

                public string REFEREAL { get; set; }

                public string DAYSYMPTOMS { get; set; }

                public string NIGHTSYMPTOMS { get; set; }

                public string BAGONISTUSE { get; set; }

                public string SEVERITY { get; set; }

                public string TREATMT_CIR { get; set; }

                public string PLANGIVEN { get; set; }

                public bool? TREATMENT { get; set; }

                public bool? ASTHMACLASS { get; set; }

                public bool? INHALER { get; set; }

                public bool? SMOKING { get; set; }

                public bool? MDIUSE { get; set; }





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

                public decimal? POS_FIXAMT { get; set; }

                public decimal? POST_FIXAMT_RANGE { get; set; }





                #endregion Instance Properties
}
            public class ATTDFRM
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHEAD { get; set; }

                public DateTime? EXPIRYDATE { get; set; }

                public DateTime? DATESUBMIT { get; set; }

                public DateTime? LASTUSED { get; set; }

                public decimal? FREQUENCY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class BILL_ADJ
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPHEAD { get; set; }

                public string NAME { get; set; }

                public string TRANSTYPE { get; set; }

                public string ADJUST { get; set; }

                public string TTYPE { get; set; }

                public decimal? AMOUNT { get; set; }

                public string COMMENTS { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string FACILITY { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class BILLAUX
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string ATTENDANCE { get; set; }

                public string GROUPCODE { get; set; }





                #endregion Instance Properties
}

            public class BILLCHAIN
            {

                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }
                
                public string PATIENTNO { get; set; }

                public string GROUPHEAD { get; set; }

                public string NAME { get; set; }

                public DateTime? REG_DATE { get; set; }

                public bool? POSTED { get; set; }
                
                public DateTime? POST_DATE { get; set; }
                
                public string GROUPHTYPE { get; set; }

                public string STAFFNO { get; set; }

                public string RELATIONSH { get; set; }

                public string SECTION { get; set; }

                public string DEPARTMENT { get; set; }

                public decimal? CUR_DB { get; set; }

                public string STATUS { get; set; }

                public string SEX { get; set; }

                public string M_STATUS { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public decimal? CUMVISITS { get; set; }

                public string HMOCODE { get; set; }

                public string PATCATEG { get; set; }

                public string RESIDENCE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string HMOSERVTYPE { get; set; }

                public string BILLONACCT { get; set; }

                public string CURRENCY { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public DateTime? EXPIRYDATE { get; set; }

                public bool? ASTNOTES { get; set; }

                public string CLINIC { get; set; }

                public string PHONE { get; set; }

                public string EMAIL { get; set; }

                public string PICLOCATION { get; set; }

                public string SURNAME { get; set; }

                public string OTHERNAMES { get; set; }

                public string TITLE { get; set; }

                public string SPNOTES { get; set; }

                public string MEDNOTES { get; set; }

                public bool? MEDHISTORYCHAINED { get; set; }

                public string PATIENTNO_PRINCIPAL { get; set; }
            
                #endregion Instance Properties
            }

            public class BILLING
            {
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public decimal? ITEMNO { get; set; }

                public string DIAG { get; set; }

                public string PROCESS { get; set; }

                public string DESCRIPTION { get; set; }

                public string DOCTOR { get; set; }

                public string FACILITY { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? RECEIPTED { get; set; }

                public string TRANSTYPE { get; set; }

                public string PAYREF { get; set; }

                public string GROUPHEAD { get; set; }

                public string SERVICETYPE { get; set; }

                public decimal? PAYMENT { get; set; }

                public string GROUPCODE { get; set; }

                public string TTYPE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_TIME { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public string EXTDESC { get; set; }

                public string ACCOUNTTYPE { get; set; }





                #endregion Instance Properties
}
            public class BILLSET
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPNAME { get; set; }

                public string TEXTDETAIL { get; set; }

                public string CLIENT1 { get; set; }

                public string FILTYPE1 { get; set; }

                public string GC1 { get; set; }

                public string CLIENT2 { get; set; }

                public string FILTYPE2 { get; set; }

                public string GC2 { get; set; }

                public string CLIENT3 { get; set; }

                public string FILTYPE3 { get; set; }

                public string GC3 { get; set; }

                public string CLIENT4 { get; set; }

                public string FILTYPE4 { get; set; }

                public string GC4 { get; set; }

                public string CLIENT5 { get; set; }

                public string FILTYPE5 { get; set; }

                public string GC5 { get; set; }

                public string CLIENT6 { get; set; }

                public string FILTYPE6 { get; set; }

                public string GC6 { get; set; }

                public string CLIENT7 { get; set; }

                public string FILTYPE7 { get; set; }

                public string GC7 { get; set; }

                public string CLIENT8 { get; set; }

                public string FILTYPE8 { get; set; }

                public string GC8 { get; set; }

                public string CLIENT9 { get; set; }

                public string FILTYPE9 { get; set; }

                public string GC9 { get; set; }

                public string CLIENT10 { get; set; }

                public string FILTYPE10 { get; set; }

                public string GC10 { get; set; }

                public string CLIENT11 { get; set; }

                public string FILTYPE11 { get; set; }

                public string GC11 { get; set; }

                public string CLIENT12 { get; set; }

                public string FILTYPE12 { get; set; }

                public string GC12 { get; set; }

                public string CLIENT13 { get; set; }

                public string FILTYPE13 { get; set; }

                public string GC13 { get; set; }

                public string CLIENT14 { get; set; }

                public string FILTYPE14 { get; set; }

                public string GC14 { get; set; }

                public string CLIENT15 { get; set; }

                public string FILTYPE15 { get; set; }

                public string GC15 { get; set; }





                #endregion Instance Properties
}
            public class BILLVOUC
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public decimal? ITEMNO { get; set; }

                public string DIAG { get; set; }

                public string PROCESS { get; set; }

                public string DESCRIPTION { get; set; }

                public string DOCTOR { get; set; }

                public string FACILITY { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? RECEIPTED { get; set; }

                public string TRANSTYPE { get; set; }

                public string PAYREF { get; set; }

                public string GROUPHEAD { get; set; }

                public string SERVICETYP { get; set; }

                public decimal? PAYMENT { get; set; }

                public string GROUPCODE { get; set; }

                public string TTYPE { get; set; }

                public string GHGROUPCOD { get; set; }

                public string PAYTYPE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_TIME { get; set; }

                public string ACCOUNTTYPE { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public string EXTDESC { get; set; }





                #endregion Instance Properties
}
            public class BIRTHS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GHGROUPCODE { get; set; }

                public string PARENT { get; set; }

                public string FATHER { get; set; }

                public string MOTHER { get; set; }

                public string NAME { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public string BIRTHTIME { get; set; }

                public string TYPEOFDELI { get; set; }

                public decimal? WEIGHT { get; set; }

                public string APGARSCORE { get; set; }

                public string SEX { get; set; }

                public decimal? LENGHT { get; set; }

                public string NURSE { get; set; }

                public string DOCTOR { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string PHOTOLOC { get; set; }

                public string GROUPHTYPE { get; set; }

                public string REMARKS { get; set; }

                public string BIRTHTYPE { get; set; }

                public bool? BORNHERE { get; set; }

                public string HOSPITAL { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string GESTATION { get; set; }

                public decimal? HEADCIRCUMF { get; set; }

                public string Parentgroupcode { get; set; }





                #endregion Instance Properties
}
            public class BSDETAIL
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? SIGNEDBILL { get; set; }

                public string GROUPHEAD { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public decimal? AMOUNT { get; set; }

                public string AUTH_CODE { get; set; }

                public DateTime? AUTH_DATE { get; set; }





                #endregion Instance Properties
}
            public class CAPBILLCHAIN
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPHEAD { get; set; }

                public string NAME { get; set; }

                public DateTime? REG_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string STAFFNO { get; set; }

                public string RELATIONSH { get; set; }

                public string DEPT { get; set; }

                public string SECTION { get; set; }

                public decimal? CUR_DB { get; set; }

                public string STATUS { get; set; }

                public string SEX { get; set; }

                public string M_STATUS { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public decimal? CUMVISITS { get; set; }

                public string HMOCODE { get; set; }

                public string PATCATEG { get; set; }

                public string RESIDENCE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string HMOSERVTYPE { get; set; }





                #endregion Instance Properties
}
            public class CAPBILLS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public decimal? ITEMNO { get; set; }

                public string DIAG { get; set; }

                public string PROCESS { get; set; }

                public string DESCRIPTION { get; set; }

                public string DOCTOR { get; set; }

                public string FACILITY { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? RECEIPTED { get; set; }

                public string TRANSTYPE { get; set; }

                public string PAYREF { get; set; }

                public string GROUPHEAD { get; set; }

                public string SERVICETYPE { get; set; }

                public decimal? PAYMENT { get; set; }

                public string GROUPCODE { get; set; }

                public string TTYPE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string PAYTYPE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_TIME { get; set; }

                public string ACCOUNTTYPE { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public string EXTDESC { get; set; }





                #endregion Instance Properties
}
            public class cdmdetails
            {

                #region Instance Properties

                public int RECID { get; set; }

                public string groupcode { get; set; }

                public string patientno { get; set; }

                public string name { get; set; }

                public string diagnosis { get; set; }

                public string drugs { get; set; }

                public decimal? frequency { get; set; }

                public decimal? qty { get; set; }

                public DateTime? lastcollection { get; set; }

                public string dispensedby { get; set; }

                public DateTime? disp_datetime { get; set; }

                public string lastvisitdoc { get; set; }

                public DateTime? visit_datetime { get; set; }

                public string comments { get; set; }

                public string clinic { get; set; }

                public string grouphead { get; set; }





                #endregion Instance Properties
            }

            public class CONSULTRMDETAILS
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string NAME { get; set; }

                public string LOCATION { get; set; }

                public string DOCTOR { get; set; }

                public DateTime? LOGIN_DATETIME { get; set; }

                public DateTime? LOGOUT_DATETIME { get; set; }

                public decimal? FREQUENCY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
            }

            public class CONSULTROOMS
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string LOCATION { get; set; }

                public string LASTDOC { get; set; }

                public string LOGIN_DATETIME { get; set; }

                public string LOGOUT_DATETIME { get; set; }

                public decimal? FREQUENCY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? OCCUPIED { get; set; }

                public string DOCNAME { get; set; }

                public decimal? ACTIVELOGS { get; set; }





                #endregion Instance Properties
            }
            
            public class CPRPTDEF
            {

                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GROUPCODE { get; set; }

                public string NAME { get; set; }

                public string CONTACT { get; set; }

                public string ADDRESS1 { get; set; }

                public decimal? cur_bills { get; set; }

                public decimal? balbf { get; set; }

                public decimal? payments { get; set; }

                public decimal? db_notes { get; set; }

                public decimal? cr_notes { get; set; }

                public string headertext { get; set; }

                public string footertext { get; set; }

                public bool? header { get; set; }

                public decimal? adjustment { get; set; }

                public decimal? balance { get; set; }

                public string bftext { get; set; }

                public string pymtext { get; set; }

                public string adjusttext { get; set; }

                public string cur_billtext { get; set; }

                public string baltext { get; set; }

                public string rpthdtext { get; set; }

                public string rectype { get; set; }

                public string DESCRIPTION { get; set; }





                #endregion Instance Properties
            }

            public class CUSTCLASS
            {

                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public decimal? PERCENTAGE { get; set; }

                public bool? DEFINEDRGS { get; set; }

                public bool? DRGRESTRICTIVE { get; set; }

                public bool? DEFINEPROC { get; set; }

                public bool? PROCRESTRICTIVE { get; set; }

                public bool? DRGINCLUSIVE { get; set; }

                public bool? PROCINCLUSIVE { get; set; }

                public decimal? CREDITLIMIT { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class CUSTOMER
            {

                #region Instance Properties

                public int RECID { get; set; }

                public string CUSTNO { get; set; }

                public string NAME { get; set; }

                public string CATEGORY { get; set; }

                public string ADDRESS1 { get; set; }

                public string STATECODE { get; set; }

                public string COUNTRY { get; set; }

                public string PHONE { get; set; }

                public decimal? CR_LIMIT { get; set; }

                public string BILL_CIR { get; set; }

                public decimal? UPCUR_DB { get; set; }

                public decimal? UPCUR_CR { get; set; }

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

                public string CONTACT { get; set; }

                public string CUSTSTATUS { get; set; }

                public decimal? LASTNUMB { get; set; }

                public string REMARK { get; set; }

                public bool? ONHIS { get; set; }

                public string CAPTYPE { get; set; }

                public decimal? CUMVISITS { get; set; }

                public decimal? TOTBENEFIC { get; set; }

                public string HMOCODE { get; set; }

                public string PATCATEG { get; set; }

                public string EMAIL { get; set; }

                public decimal? MIN_ORD_AM { get; set; }

                public decimal? MAX_ORD_AM { get; set; }

                public decimal? DISCOUNT { get; set; }

                public decimal? HMOMARKUP { get; set; }

                public bool? HMO { get; set; }

                public string PAYMTNOTE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public bool? TRACKFORM { get; set; }

                public bool? LINKADMSP { get; set; }

                public bool? TRACKATTNDFORM { get; set; }

                public bool? BILLSBYGC { get; set; }

                public string CURRENCY { get; set; }

                public bool? QAALERT { get; set; }

                public bool? BILLREGISTRATION { get; set; }

                public decimal? GRPCREDITLIMIT { get; set; }

                public decimal? GRPCREDITTYPE { get; set; }

                public bool? TOSIGNBILL { get; set; }

                public decimal? CONSULTATION { get; set; }

                public decimal? ADMISSIONS { get; set; }

                public decimal? FEEDING { get; set; }

                public bool? NONCAPITATION { get; set; }

                public string BANK_BRANCH { get; set; }

                public bool? REFERRER { get; set; }





                #endregion Instance Properties
}
            public class DEATHS
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string NAME { get; set; }

                public DateTime? DEATHDATE { get; set; }

                public string DEATHTIME { get; set; }

                public string DIAG { get; set; }

                public string REMARKS { get; set; }

                public string DOCTOR { get; set; }

                public string NURSE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? DOB { get; set; }

                public string SEX { get; set; }

                public string FACILITY { get; set; }





                #endregion Instance Properties
}
            public class DEPT
            {




                #region Instance Properties

                public int RECID { get; set; }

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
            public class DGPROFILE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string DRUGS { get; set; }

                public decimal? AMOUNT { get; set; }

                public string OTHERS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? CAPITATED { get; set; }

                public bool? AUTHORIZATIONREQUIRED { get; set; }

                public string NAME { get; set; }





                #endregion Instance Properties
}
            public class DISPENSA
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string STK_ITEM { get; set; }

                public string STK_DESC { get; set; }

                public string STORE { get; set; }

                public decimal? QTY_PR { get; set; }

                public decimal? QTY_GV { get; set; }

                public decimal? CUMGV { get; set; }

                public decimal? DOSE { get; set; }

                public decimal? INTERVAL { get; set; }

                public decimal? DURATION { get; set; }

                public string UNIT { get; set; }

                public decimal? COST { get; set; }

                public string NURSE { get; set; }

                public string DOCTOR { get; set; }

                public string DIAG { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string NAME { get; set; }

                public decimal? STKBAL { get; set; }

                public string TIME { get; set; }

                public string TYPE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GHGROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_TIME { get; set; }

                public decimal? UNITCOST { get; set; }

                public string RX { get; set; }

                public string SP_INST { get; set; }

                public string CDOSE { get; set; }

                public string CINTERVAL { get; set; }

                public string CDURATION { get; set; }

                public bool? CAPITATED { get; set; }

                public string REFERENCE { get; set; }

                public decimal? unitpurvalue { get; set; }





                #endregion Instance Properties
}
            public class DISPSERV
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string DESCRIPTION { get; set; }
            
                #endregion Instance Properties
}
            public class DOCTORS
            {


                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string SURNAME { get; set; }

                public string OTHERS { get; set; }

                public string NAME { get; set; }

                public DateTime? PROF_REGD { get; set; }

                public string REG_NO { get; set; }

                public string RECTYPE { get; set; }

                public string STAFF_NO { get; set; }

                public string ADD_DATA { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? CONSCHARGE { get; set; }

                public bool? UNRESTRICTED { get; set; }

                public string LOGINDATE { get; set; }

                public string LOGOUTDATE { get; set; }

                public bool? LOGINACTIVE { get; set; }

                public string FACILITY { get; set; }

                public string CONSROOM { get; set; }

                public bool? EXCLUSIVEDATA { get; set; }

                public bool? GLOBALACCESS { get; set; }

                public string PHONE { get; set; }

                public string EMAIL { get; set; }





                #endregion Instance Properties
}
            public class DRUGSLIP
            {


            

                #region Instance Properties

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

                public bool? BFREETEXT { get; set; }

                public string LFREETEXT { get; set; }





                #endregion Instance Properties
}
            public class DUENEXT
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string STK_ITEM { get; set; }

                public string STK_DESC { get; set; }

                public string STORE { get; set; }

                public decimal? DOSE { get; set; }

                public string UNIT { get; set; }

                public decimal? COST { get; set; }

                public string NURSE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? ITEMNO { get; set; }

                public decimal? STKBAL { get; set; }

                public string TIMEGIVEN { get; set; }

                public string TYPE { get; set; }

                public string DUETIME { get; set; }

                public string METHODADM { get; set; }

                public string REMARKS { get; set; }

                public string REFERENCE { get; set; }

                public decimal? PACKQTY { get; set; }

                public decimal? PACKCOST { get; set; }

                public decimal? UNITCOST { get; set; }

                public decimal? BILLQTY { get; set; }

                public string BILLQTYUNIT { get; set; }





                #endregion Instance Properties
}

            public class DUTYRTAB
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string DESCRIPTION { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }


                #endregion Instance Properties
}

            public class ECG
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string HEARTRATE { get; set; }

                public string RHYTHM { get; set; }

                public string F_AXIS { get; set; }

                public string H_POSITION { get; set; }

                public string PR_INTERVA { get; set; }

                public string QRS_INTERV { get; set; }

                public string QT { get; set; }

                public string P_WAVE { get; set; }

                public string QRS_WAVE { get; set; }

                public string VOLTAGE { get; set; }

                public string ST_SEGMENT { get; set; }

                public string T_WAVE { get; set; }

                public string EF_TOLERAN { get; set; }

                public string BP_STAGE1 { get; set; }

                public string BP_MMHG_1 { get; set; }

                public string BP_SYMP1 { get; set; }

                public string BP_STAGE2 { get; set; }

                public string BP_MMHG_2 { get; set; }

                public string BP_SYMP2 { get; set; }

                public string BP_STAGE3 { get; set; }

                public string BP_MMHG_3 { get; set; }

                public string BP_SYMP3 { get; set; }

                public string BP_STAGE4 { get; set; }

                public string BP_MMHG_4 { get; set; }

                public string BP_SYMP4 { get; set; }

                public string BP_STAGE5 { get; set; }

                public string BP_MMHG_5 { get; set; }

                public string BP_SYMP5 { get; set; }

                public string M_HRATE { get; set; }

                public string O_CHANGES { get; set; }

                public string IMPRESSION { get; set; }

                public string CONCLUSION { get; set; }

                public string SYMP_REF { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string NAME { get; set; }

                public string RESTCOMENT { get; set; }

                public string EXERCOMENT { get; set; }

                public int? AGE { get; set; }

                public string HROTATION { get; set; }

                public string BILL_NO { get; set; }

                public string BILLSELF { get; set; }

                public string BILLPATIEN { get; set; }

                public string CROSSREF { get; set; }

                public string BILLCUST { get; set; }





                #endregion Instance Properties
}
            public class FCCUSTOM
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CUSTNO { get; set; }

                public string CURRENCY { get; set; }

                public decimal? CUR_DB { get; set; }

                public decimal? CUR_CR { get; set; }

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
            public class FCPATIENT
            {
                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string CURRENCY { get; set; }

                public decimal? CR_LIMIT { get; set; }

                public decimal? CUR_DB { get; set; }

                public decimal? CUR_CR { get; set; }

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

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class FCSTKCHARG
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string ITEM { get; set; }

                public string PATCATEG { get; set; }

                public decimal? AMOUNT { get; set; }

                public decimal? PERCENTAGE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CURRENCY { get; set; }





                #endregion Instance Properties
}
            public class FCTARIFF
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string STATMT_DES { get; set; }

                public string CATEGORY { get; set; }

                public string TYPE { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public decimal? CATP { get; set; }

                public decimal? CATC { get; set; }

                public decimal? CATV { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? DIFFCHARGE { get; set; }

                public bool? AVAILABLE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string CURRENCY { get; set; }





                #endregion Instance Properties
}
            public class FFSC01
            {
            

                #region Instance Properties

                public int RECID { get; set; }

                public string PERIOD { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string PATNAME { get; set; }

                public string GROUPHEAD { get; set; }

                public string GRPHEADNAME { get; set; }

                public string STAFFNO { get; set; }

                public string PLANTYPE { get; set; }

                public string SEX { get; set; }

                public string NOTEDATE { get; set; }

                public string ADM_DATE { get; set; }

                public string DISCHARGE { get; set; }

                public string AUTHORITYCODE { get; set; }

                public string DIAGNOSIS { get; set; }

                public decimal? ACCOMMODATION { get; set; }

                public decimal? ACC_DAYS { get; set; }

                public decimal? FEED_DAYS { get; set; }

                public decimal? FEEDAMT { get; set; }

                public string LAB { get; set; }

                public string XRAY { get; set; }

                public decimal? CONSULTATION { get; set; }

                public string AGE { get; set; }

                public string TRANSTYPE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? LABAMT { get; set; }

                public decimal? XRAYAMT { get; set; }

                public string OTHERS { get; set; }

                public decimal? OTHERSAMT { get; set; }

                public string ENROLLENO { get; set; }

                public DateTime? treatmentdate { get; set; }

                public string PHONE { get; set; }





                #endregion Instance Properties
}
            public class FFSC01B
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PERIOD { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? QTY { get; set; }

                public decimal? AMOUNT { get; set; }





                #endregion Instance Properties
}
            public class FFSC02
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PERIOD { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string PATNAME { get; set; }

                public string GROUPHEAD { get; set; }

                public string GRPHEADNAME { get; set; }

                public string STAFFNO { get; set; }

                public string PLANTYPE { get; set; }

                public string SEX { get; set; }

                public DateTime? NOTEDATE { get; set; }

                public string AUTHORITYCODE { get; set; }

                public string BODY_MASS { get; set; }

                public string FASTINGBLOOD { get; set; }

                public DateTime? DT_LAST_MB { get; set; }

                public string LAST_VALUE { get; set; }

                public string COMPLICATIONS { get; set; }

                public string ATTEND_SPECIALIST { get; set; }

                public string OTHER_INVS { get; set; }

                public string DETAILS_OF_THERAPY { get; set; }

                public DateTime? NEXT_APPTMT { get; set; }

                public string COMPANY { get; set; }

                public string AGE { get; set; }





                #endregion Instance Properties
}
            public class FFSC03
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PERIOD { get; set; }

                public string TRANS_DATE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string PATNAME { get; set; }

                public string GROUPHEAD { get; set; }

                public string GRPHEADNAME { get; set; }

                public string STAFFNO { get; set; }

                public string PLANTYPE { get; set; }

                public string SEX { get; set; }

                public DateTime? NOTEDATE { get; set; }

                public string AUTHORITYCODE { get; set; }

                public string BP { get; set; }

                public string BODY_MASS { get; set; }

                public string COMPLICATIONS { get; set; }

                public string ATTEND_SPECIALIST { get; set; }

                public string OTHER_INVS { get; set; }

                public string DETAILS_OF_THERAPY { get; set; }

                public DateTime? NEXT_APPTMT { get; set; }

                public string COMPANY { get; set; }

                public string AGE { get; set; }





                #endregion Instance Properties
}
            public class FFSC04
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PERIOD { get; set; }

                public string TRANS_DATE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string PATNAME { get; set; }

                public string GROUPHEAD { get; set; }

                public string GRPHEADNAME { get; set; }

                public string STAFFNO { get; set; }

                public string PLANTYPE { get; set; }

                public string SEX { get; set; }

                public DateTime? NOTEDATE { get; set; }

                public string AUTHORITYCODE { get; set; }

                public string CASETYPE { get; set; }

                public string BLOODCHECK { get; set; }

                public string PARASITE { get; set; }

                public string ADMITTED { get; set; }

                public string DRUGS_GIVEN { get; set; }

                public string COMPANY { get; set; }

                public string AGE { get; set; }





                #endregion Instance Properties
}
            public class FFSC05
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PERIOD { get; set; }

                public string TRANS_DATE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string PATNAME { get; set; }

                public string GROUPHEAD { get; set; }

                public string GRPHEADNAME { get; set; }

                public string STAFFNO { get; set; }

                public string PLANTYPE { get; set; }

                public string SEX { get; set; }

                public DateTime? NOTEDATE { get; set; }

                public string AUTHORITYCODE { get; set; }

                public string COMPANY { get; set; }

                public string DIAG { get; set; }

                public string SERVICE { get; set; }

                public string REASONFORCLAIM { get; set; }

                public bool? DENTALREG { get; set; }

                public bool? SCALING { get; set; }

                public bool? FILLED { get; set; }

                public bool? EXTRACTED { get; set; }

                public bool? XRAY { get; set; }

                public bool? OPTICALREG { get; set; }

                public bool? GLASSES { get; set; }

                public bool? ANCREG { get; set; }

                public bool? DELIVERY { get; set; }

                public bool? C_S { get; set; }

                public decimal? AMOUNT_CLAIMED { get; set; }

                public string AGE { get; set; }

                public string TRANSTYPE { get; set; }





                #endregion Instance Properties
}
            public class FFSC06
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PERIOD { get; set; }

                public string DTYPE { get; set; }

                public string TRANS_DATE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string PATNAME { get; set; }

                public string GROUPHEAD { get; set; }

                public string GRPHEADNAME { get; set; }

                public string STAFFNO { get; set; }

                public string PLANTYPE { get; set; }

                public string SEX { get; set; }

                public DateTime? NOTEDATE { get; set; }

                public string AUTHORITYCODE { get; set; }

                public string COMPANY { get; set; }

                public string DIAG { get; set; }

                public DateTime? ADM_DATE { get; set; }

                public DateTime? DISCHARGE { get; set; }

                public string SERVICE { get; set; }

                public string SURGERY { get; set; }

                public string CONDITIONONDISCHARGE { get; set; }

                public string AGE { get; set; }





                #endregion Instance Properties
}
            public class ffsformprofiler
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string custno { get; set; }

                public string name { get; set; }

                public string address1 { get; set; }

                public string phone { get; set; }

                public string email { get; set; }

                public string formheader { get; set; }

                public string formlogo { get; set; }





                #endregion Instance Properties
}
            public class GLINT
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CURRENCY { get; set; }

                public string COMPANY { get; set; }

                public decimal? BATCHNO { get; set; }

                public string DOCUMENTID { get; set; }

                public string PVTCASHDEBIT { get; set; }

                public string PVTCASHCREDIT { get; set; }

                public string PVTCHQDEBIT { get; set; }

                public string PVTCHQCREDIT { get; set; }

                public string COPCASHDEBIT { get; set; }

                public string COPCASHCREDIT { get; set; }

                public string COPCHQDEBIT { get; set; }

                public string COPCHQCREDIT { get; set; }

                public string HMOCLM_CASHDEBIT { get; set; }

                public string HMOCLM_CASHCREDIT { get; set; }

                public string HMOCLM_CHQDEBIT { get; set; }

                public string HMOCLM_CHQCREDIT { get; set; }

                public string HMOCAP_CASHDEBIT { get; set; }

                public string HMOCAP_CASHCREDIT { get; set; }

                public string HMOCAP_CHQDEBIT { get; set; }

                public string HMOCAP_CHQCREDIT { get; set; }

                public string NHISCLM_CASHDEBIT { get; set; }

                public string NHISCLM_CASHCREDIT { get; set; }

                public string NHISCLM_CHQDEBIT { get; set; }

                public string NHISCLM_CHQCREDIT { get; set; }

                public string NHISCAP_CASHDEBIT { get; set; }

                public string NHISCAP_CASHCREDIT { get; set; }

                public string NHISCAP_CHQDEBIT { get; set; }

                public string NHISCAP_CHQCREDIT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class GLINTAB1
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string DOCUMENT { get; set; }

                public decimal? BATCHNO { get; set; }

                public string CURRENCY { get; set; }

                public string COMPANY { get; set; }

                public string PVTBDEBIT { get; set; }

                public string PVTBCREDIT { get; set; }

                public string PVTCASHCREDIT { get; set; }

                public string PVTCASHDEBIT { get; set; }

                public string PVTCHQDEBIT { get; set; }

                public string PVTCHQCREDIT { get; set; }

                public string FCBDEBIT { get; set; }

                public string FCBCREDIT { get; set; }

                public string FCCASHDEBIT { get; set; }

                public string FCCASHCREDIT { get; set; }

                public string FCCHQCREDIT { get; set; }

                public string FCCHQDEBIT { get; set; }

                public string COPBDEBIT { get; set; }

                public string COPBCREDIT { get; set; }

                public string COPCASHDEBIT { get; set; }

                public string COPCASHCREDIT { get; set; }

                public string COPCHQDEBIT { get; set; }

                public string COPCHQCREDIT { get; set; }

                public string SPSBDEBIT { get; set; }

                public string SPSBCREDIT { get; set; }

                public string SPSCASHDEBIT { get; set; }

                public string SPSCASHCREDIT { get; set; }

                public string SPSCHQDEBIT { get; set; }

                public string SPSCHQCREDIT { get; set; }

                public string OTHBDEBIT { get; set; }

                public string OTHBCREDIT { get; set; }

                public string OTHCASHDEBIT { get; set; }

                public string OTHCASHCREDIT { get; set; }

                public string OTHCHQDEBIT { get; set; }

                public string OTHCHQCREDIT { get; set; }

                public string HMOCLM_BDEBIT { get; set; }

                public string HMOCLM_BCREDIT { get; set; }

                public string HMOCLM_CASHDEBIT { get; set; }

                public string HMOCLM_CASHCREDIT { get; set; }

                public string HMOCLM_CHQDEBIT { get; set; }

                public string HMOCLM_CHQCREDIT { get; set; }

                public string HMOCAP_BDEBIT { get; set; }

                public string HMOCAP_BCREDIT { get; set; }

                public string HMOCAP_CASHDEBIT { get; set; }

                public string HMOCAP_CASHCREDIT { get; set; }

                public string NHISCLM_BDEBIT { get; set; }

                public string NHISCLM_BCREDIT { get; set; }

                public string NHISCLM_CASHDEBIT { get; set; }

                public string NHISCLM_CASHCREDIT { get; set; }

                public string NHISCLM_CHQDEBIT { get; set; }

                public string NHISCLM_CHQCREDIT { get; set; }

                public string NHISCAP_BDEBIT { get; set; }

                public string NHISCAP_BCREDIT { get; set; }

                public string NHISCAP_CASHDEBIT { get; set; }

                public string NHISCAP_CASHCREDIT { get; set; }

                public string NHISCAP_CHQDEBIT { get; set; }

                public string NHISCAP_CHQCREDIT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class GLINTAB2
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string ADJUST { get; set; }

                public string NAME { get; set; }

                public string TYPE { get; set; }

                public string FCPVTDEBIT { get; set; }

                public string FCPVTCREDIT { get; set; }

                public string CODEBIT { get; set; }

                public string COCREDIT { get; set; }

                public string HMOCLMDEBIT { get; set; }

                public string HMOCLMCREDIT { get; set; }

                public string HMOCAPDEBIT { get; set; }

                public string HMOCAPCREDIT { get; set; }

                public string NHISCLMDEBIT { get; set; }

                public string NHISCLMCREDIT { get; set; }

                public string NHISCAPDEBIT { get; set; }

                public string NHISCAPCREDIT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class GLINTAB3
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string FACILITY { get; set; }

                public string NAME { get; set; }

                public string PVTBDEBIT { get; set; }

                public string PVTBCREDIT { get; set; }

                public string COPBCREDIT { get; set; }

                public string COPBDEBIT { get; set; }

                public string HMOCLM_BDEBIT { get; set; }

                public string HMOCLM_BCREDIT { get; set; }

                public string HMOCAP_BDEBIT { get; set; }

                public string HMOCAP_BCREDIT { get; set; }

                public string NHISCLM_BDEBIT { get; set; }

                public string NHISCLM_BCREDIT { get; set; }

                public string NHISCAP_BDEBIT { get; set; }

                public string NHISCAP_BCREDIT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class GLINTAB4
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PROCESS { get; set; }

                public string NAME { get; set; }

                public string CURRENCY { get; set; }

                public string COMPANY { get; set; }

                public decimal? BATCHNO { get; set; }

                public string DOCUMENT { get; set; }

                public string PVTBDEBIT { get; set; }

                public string PVTBCREDIT { get; set; }

                public string PVTCASHDEBIT { get; set; }

                public string PVTCASHCREDIT { get; set; }

                public string PVTCHQDEBIT { get; set; }

                public string PVTCHQCREDIT { get; set; }

                public string COPBDEBIT { get; set; }

                public string COPBCREDIT { get; set; }

                public string COPCASHDEBIT { get; set; }

                public string COPCASHCREDIT { get; set; }

                public string COPCHQDEBIT { get; set; }

                public string COPCHQCREDIT { get; set; }

                public string HMOCLM_BDEBIT { get; set; }

                public string HMOCLM_BCREDIT { get; set; }

                public string HMOCLM_CASHDEBIT { get; set; }

                public string HMOCLM_CASHCREDIT { get; set; }

                public string HMOCLM_CHQDEBIT { get; set; }

                public string HMOCLM_CHQCREDIT { get; set; }

                public string HMOCAP_BDEBIT { get; set; }

                public string HMOCAP_BCREDIT { get; set; }

                public string HMOCAP_CASHDEBIT { get; set; }

                public string HMOCAP_CASHCREDIT { get; set; }

                public string HMOCAP_CHQDEBIT { get; set; }

                public string HMOCAP_CHQCREDIT { get; set; }

                public string NHISCLM_BDEBIT { get; set; }

                public string NHISCLM_BCREDIT { get; set; }

                public string NHISCLM_CASHDEBIT { get; set; }

                public string NHISCLM_CASHCREDIT { get; set; }

                public string NHISCLM_CHQDEBIT { get; set; }

                public string NHISCLM_CHQCREDIT { get; set; }

                public string NHISCAP_BDEBIT { get; set; }

                public string NHISCAP_BCREDIT { get; set; }

                public string NHISCAP_CASHDEBIT { get; set; }

                public string NHISCAP_CASHCREDIT { get; set; }

                public string NHISCAP_CHQDEBIT { get; set; }

                public string NHISCAP_CHQCREDIT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class GRPPROCEDURE
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string FACILITY { get; set; }

                public string NAME { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public bool? grpbillbyservtype { get; set; }





                #endregion Instance Properties
}
            public class HMOAUTHORIZATIONS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string REFERRAL { get; set; }

                public string REFERRALCLINIC { get; set; }

                public DateTime? REFERRALDATE { get; set; }

                public string REFERREDTODOC { get; set; }

                public string REFERREDTOCLINIC { get; set; }

                public DateTime? REQUESTCOMMENCED { get; set; }

                public string REQUESTDETAILS { get; set; }

                public string GROUPHEAD { get; set; }

                public string GROUPHTYPE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string DIAGNOSIS { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string AUTHORIZEDCODE { get; set; }

                public string DATERECEIVED { get; set; }





                #endregion Instance Properties
}
            public class HMODETAIL
            {

                #region Instance Properties

                public int RECID { get; set; }

                public string CUSTNO { get; set; }

                public string HMOSERVTYPE { get; set; }

                public string DESCRIPTION { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? DATE_REG { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? LASTNUMB { get; set; }

                public string REMARK { get; set; }

                public decimal? CAPAMT { get; set; }

                public decimal? ACCFEEDING { get; set; }

                public decimal? VISALLOWED { get; set; }

                public decimal? CUMVISITS { get; set; }

                public decimal? TOTBENEFIC { get; set; }

                public decimal? FAMUNIT { get; set; }

                public bool? SERV_ITEMS { get; set; }

                public bool? DRGRESTRICTIVE { get; set; }

                public bool? PROCRESTRICTIVE { get; set; }

                public bool? DRGINCLUSIVE { get; set; }

                public bool? PROCINCLUSIVE { get; set; }

                public decimal? CONSULTATION { get; set; }

                public decimal? ADMISSIONS { get; set; }

                public decimal? FEEDING { get; set; }





                #endregion Instance Properties
}
            public class HMOSERVIC
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CUSTNO { get; set; }

                public string HMOSERVTYPE { get; set; }

                public string DRUGS { get; set; }

                public decimal? AMOUNT { get; set; }

                public string OTHERS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? CAPITATED { get; set; }

                public bool? AUTHORIZATIONREQUIRED { get; set; }

                public string NAME { get; set; }





                #endregion Instance Properties
}
            public class HMOSERVPROC
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CUSTNO { get; set; }

                public string HMOSERVTYPE { get; set; }

                public string PROCESS { get; set; }

                public decimal? AMOUNT { get; set; }

                public string OTHERS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? CAPITATED { get; set; }

                public bool? AUTHORIZATIONREQUIRED { get; set; }

                public string NAME { get; set; }





                #endregion Instance Properties
}
            public class HORIZBREF
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CUSTOMER { get; set; }

                public string GHGROUPCODE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string REFERENCE { get; set; }

                public decimal? AMOUNT { get; set; }





                #endregion Instance Properties
}
            public class IBR001
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string REF_SYMPTO { get; set; }

                public string PROCESS { get; set; }

                public string DESCRIPTIO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string RADRPT { get; set; }

                public string FACILITY { get; set; }

                public decimal? AMOUNT { get; set; }

                public string TESTBY { get; set; }

                public DateTime? TESTDATE { get; set; }

                public string DOCTOR { get; set; }





                #endregion Instance Properties
}
            public class ICD10
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CHAPTER { get; set; }

                public string REFERENCE { get; set; }

                public string CATEGORY { get; set; }

                public string NAME { get; set; }





                #endregion Instance Properties
}
            public class ICPC2
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string CODETYPE { get; set; }





                #endregion Instance Properties
}
            public class IMMI01
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CHILD_AGE { get; set; }

                public bool? SHY { get; set; }

                public bool? MOTHER_LEA { get; set; }

                public bool? IMITAT_PEO { get; set; }

                public bool? SPEC_PREFE { get; set; }

                public bool? TEST_PAREN { get; set; }

                public bool? RESP_BEHAV { get; set; }

                public bool? FEARFUL { get; set; }

                public bool? PREFER_MOT { get; set; }

                public bool? REPEAT_SOU { get; set; }

                public bool? FINGER_FEE { get; set; }

                public bool? EXTEND_ARM { get; set; }

                public bool? EXPLOR_OBJ { get; set; }

                public bool? HIDDEN_OBJ { get; set; }

                public bool? LOOK_PICTU { get; set; }

                public bool? IMITAT_GES { get; set; }

                public bool? USE_OBJECT { get; set; }

                public bool? ATTEN_SPEE { get; set; }

                public bool? VERB_REQUE { get; set; }

                public bool? RESPOND_NO { get; set; }

                public bool? SIMP_GESTU { get; set; }

                public bool? BABBLES { get; set; }

                public bool? DADA_MAMA { get; set; }

                public bool? EXCLAM_OH { get; set; }

                public bool? IMITAT_WOR { get; set; }

                public string COMMENTS { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class IMMI02
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CHILD_AGE { get; set; }

                public bool? IMITAT_BEH { get; set; }

                public bool? MORE_AWARE { get; set; }

                public bool? MORE_EXCIT { get; set; }

                public bool? DEM_INDEPE { get; set; }

                public bool? DEFIA_BEHA { get; set; }

                public bool? ANXIETY_IN { get; set; }

                public bool? FIND_OBJEC { get; set; }

                public bool? SORT_SHAPE { get; set; }

                public bool? MAKE_BELIE { get; set; }

                public bool? POINT_OBJE { get; set; }

                public bool? RECOG_NAME { get; set; }

                public bool? SINGL_WORD { get; set; }

                public bool? SIMP_PHRAS { get; set; }

                public bool? WORD_SENTE { get; set; }

                public bool? SIMPL_INST { get; set; }

                public bool? REPEA_WORD { get; set; }

                public bool? WALK_ALONE { get; set; }

                public bool? PULLS_TOYS { get; set; }

                public bool? LARGE_TOYS { get; set; }

                public bool? BEGIN_RUN { get; set; }

                public bool? STAND_TIPT { get; set; }

                public bool? KICKS_BALL { get; set; }

                public bool? CLIMB_FURN { get; set; }

                public bool? WALKS_UP { get; set; }

                public bool? SCRIBBLES { get; set; }

                public bool? TURN_OVER { get; set; }

                public bool? BULD_TOWER { get; set; }

                public bool? ONE_HAND { get; set; }

                public string COMMENTS { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class IMMI03
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CHILD_AGE { get; set; }

                public bool? IMITATESADULTS { get; set; }

                public bool? SPONTANEOUSLY { get; set; }

                public bool? CANTAKE { get; set; }

                public bool? UNDERSTANDS { get; set; }

                public bool? EXPRESSES { get; set; }

                public bool? EMOTIONS { get; set; }

                public bool? PARENTS { get; set; }

                public bool? OBJECTS { get; set; }

                public bool? MAKES { get; set; }

                public bool? MATCHES { get; set; }

                public bool? PLAYS { get; set; }

                public bool? SORTS { get; set; }

                public bool? COMPLETES { get; set; }

                public bool? CONCEPTS { get; set; }

                public bool? FOLLOWS { get; set; }

                public bool? RECOGNIZES { get; set; }

                public bool? SENTENCES { get; set; }

                public bool? PLACEMENT { get; set; }

                public bool? WORDS { get; set; }

                public bool? SAYNAME { get; set; }

                public bool? PRONOUS { get; set; }

                public bool? STRANGERS { get; set; }

                public bool? CLIMBS { get; set; }

                public bool? WALKS { get; set; }

                public bool? KICKS { get; set; }

                public bool? RUNS { get; set; }

                public bool? PEDALS { get; set; }

                public bool? BENDS { get; set; }

                public bool? CIRCULAR { get; set; }

                public bool? TURNSBOOKS { get; set; }

                public bool? BUILDS { get; set; }

                public bool? HOLDS { get; set; }

                public bool? SCREWS { get; set; }

                public bool? TURNSROTATING { get; set; }

                public string COMMENTS { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class IMMI03M
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CHILD_AGE { get; set; }

                public bool? SOCIA_SMIL { get; set; }

                public bool? PLAY_PEOPL { get; set; }

                public bool? EXPRESSIVE { get; set; }

                public bool? IMITATE_MO { get; set; }

                public bool? RAISE_HEAD { get; set; }

                public bool? SUPP_BODY { get; set; }

                public bool? STRECH_LEG { get; set; }

                public bool? OPEN_SHUT { get; set; }

                public bool? PUSH_DOWN { get; set; }

                public bool? HAND2MOUTH { get; set; }

                public bool? TAKE_SWIPE { get; set; }

                public bool? GRASP_SHAK { get; set; }

                public bool? WATCH_FACE { get; set; }

                public bool? FOLLOW_OBJ { get; set; }

                public bool? RECOG_FAMI { get; set; }

                public bool? HAND_EYES { get; set; }

                public bool? SMIL_SOUND { get; set; }

                public bool? BEGIN_BABB { get; set; }

                public bool? IMIT_SOUND { get; set; }

                public bool? TURNS_HEAD { get; set; }

                public string COMMENTS { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class IMMI04
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CHILD_AGE { get; set; }

                public bool? INTERESTED { get; set; }

                public bool? COOPERATES { get; set; }

                public bool? PLAYSMOM { get; set; }

                public bool? INVENTIVE { get; set; }

                public bool? DRESSES { get; set; }

                public bool? NEGOTIATES { get; set; }

                public bool? INDEPENDENT { get; set; }

                public bool? IMAGINES { get; set; }

                public bool? VIEWSSELF { get; set; }

                public bool? FANTASY { get; set; }

                public bool? CORRECTLY { get; set; }

                public bool? COUNTING { get; set; }

                public bool? SOLVEPROBLEMS { get; set; }

                public bool? CLEARERSENSE { get; set; }

                public bool? COMMANDS { get; set; }

                public bool? RECALLS { get; set; }

                public bool? DIFFERENT { get; set; }

                public bool? ENGAGES { get; set; }

                public bool? MASTERED { get; set; }

                public bool? SENTENCES { get; set; }

                public bool? SPEAKSCLEARLY { get; set; }

                public bool? TELLSSTORIES { get; set; }

                public bool? HOPS { get; set; }

                public bool? GOES { get; set; }

                public bool? KICKS { get; set; }

                public bool? THROWS { get; set; }

                public bool? CATCHES { get; set; }

                public bool? MOVES { get; set; }

                public bool? COPIES { get; set; }

                public bool? DRAWS { get; set; }

                public bool? USES { get; set; }

                public bool? CIRCLES { get; set; }

                public bool? COPYSOME { get; set; }

                public string COMMENTS { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class IMMI05
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CHILD_AGE { get; set; }

                public bool? PLEASEFRIENDS { get; set; }

                public bool? HERFRIENDS { get; set; }

                public bool? AGREETORULE { get; set; }

                public bool? SINGS { get; set; }

                public bool? INDEPENDENCE { get; set; }

                public bool? GENDER { get; set; }

                public bool? DISTINGUISH { get; set; }

                public bool? DEMANDING { get; set; }

                public bool? CANCOUNT { get; set; }

                public bool? COLOURS { get; set; }

                public bool? CONCEPTOFTIME { get; set; }

                public bool? EVERYDAYTHINGS { get; set; }

                public bool? STORY { get; set; }

                public bool? FIVEWORDS { get; set; }

                public bool? FUTURETENSE { get; set; }

                public bool? LONGERSTORES { get; set; }

                public bool? NAMEADDRESS { get; set; }

                public bool? STANDSLONGER { get; set; }

                public bool? SOMERSAULTS { get; set; }

                public bool? SWINGS { get; set; }

                public bool? ABLETOSKIP { get; set; }

                public bool? TRIANGLE { get; set; }

                public bool? DRAWSPERSONS { get; set; }

                public bool? PRINTS { get; set; }

                public bool? DRESSES { get; set; }

                public bool? USESPOON { get; set; }

                public bool? CARES { get; set; }

                public string COMMENTS { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class IMMI07M
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANSDATE { get; set; }

                public string CHILD_AGE { get; set; }

                public bool? SOCIA_PLAY { get; set; }

                public bool? MIRROR_IMG { get; set; }

                public bool? RESPOND_EX { get; set; }

                public bool? FINDS_OBJE { get; set; }

                public bool? EXPLOR_HAN { get; set; }

                public bool? STRUGG_OBJ { get; set; }

                public bool? RESP_NAME { get; set; }

                public bool? RESPOND_NO { get; set; }

                public bool? TELL_EMOTI { get; set; }

                public bool? RESP_SOUND { get; set; }

                public bool? JOY_DISPLE { get; set; }

                public bool? BABBLE_SOU { get; set; }

                public bool? ROLLS_BOTH { get; set; }

                public bool? SITS_WITH { get; set; }

                public bool? SUPP_WEIGH { get; set; }

                public bool? REACH_HAND { get; set; }

                public bool? TRANSF_OBJ { get; set; }

                public bool? RAKE_OBJEC { get; set; }

                public bool? COLOR_VISI { get; set; }

                public bool? DIST_VISIO { get; set; }

                public bool? TRACK_OBJE { get; set; }

                public string COMMENTS { get; set; }

                public bool? MEDHISTUPDATED { get; set; }





                #endregion Instance Properties
}
            public class IMMUNTAB
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PROCESS { get; set; }

                public decimal? DAYSFROM { get; set; }

                public decimal? DAYSTO { get; set; }

                public bool? FROMBIRTH { get; set; }

                public bool? FROMREG { get; set; }

                public decimal? AGELIMIT { get; set; }

                public string TYPE { get; set; }

                public bool? COMPULSORY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? FROMLASTDOSE { get; set; }

                public string STOCKCODE { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? BASIC_COST { get; set; }

                public bool? CORP_DIFF_CHG { get; set; }

                public bool? GLOBAL_DIFF_CHG { get; set; }

                public decimal? QTY_REQD { get; set; }

                public string NAME { get; set; }

                public string STORE { get; set; }





                #endregion Instance Properties
}
            public class IMUNES
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public string PARENT { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public string PROCESS { get; set; }

                public DateTime? DATEDUE { get; set; }

                public DateTime? DATETAKEN { get; set; }

                public decimal? WEIGHT { get; set; }

                public decimal? HEIGHT { get; set; }

                public string TEMP { get; set; }

                public string REMARKS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string AGE { get; set; }

                public decimal? AMOUNT { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string NEXTPROCEDURE { get; set; }

                public decimal? NEXTDUEDAY { get; set; }

                public DateTime? NEXTDUEDATE { get; set; }

                public string REACTIONS { get; set; }

                public string SITEOFADMIN { get; set; }

                public string ROUTEOFADMIN { get; set; }

                public string SIGNATURE { get; set; }

                public string BATCHNO { get; set; }

                public string AGEGROUP { get; set; }

                public decimal? HEADCIRCUMF { get; set; }

                public string NEXTDUE { get; set; }

                public string PROCESSDESC { get; set; }





                #endregion Instance Properties
}
            public class INJCARD
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string STKITEM { get; set; }

                public string DESCRIPTION { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPTIME { get; set; }

                public string CROSSREF { get; set; }

                public bool? MONDAY { get; set; }

                public bool? TUESDAY { get; set; }

                public bool? WEDNESDAY { get; set; }

                public bool? THURSDAY { get; set; }

                public bool? FRIDAY { get; set; }

                public bool? SATURDAY { get; set; }

                public bool? SUNDAY { get; set; }

                public DateTime? TMTDATE { get; set; }

                public string TIMEGIVEN { get; set; }





                #endregion Instance Properties
}
            public class INPDISPENSA
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string STK_ITEM { get; set; }

                public string STK_DESC { get; set; }

                public string STORE { get; set; }

                public decimal? QTY_PR { get; set; }

                public decimal? QTY_GV { get; set; }

                public decimal? CUMGV { get; set; }

                public decimal? DOSE { get; set; }

                public decimal? INTERVAL { get; set; }

                public decimal? DURATION { get; set; }

                public string UNIT { get; set; }

                public decimal? COST { get; set; }

                public string NURSE { get; set; }

                public string DOCTOR { get; set; }

                public string DIAG { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string NAME { get; set; }

                public decimal? STKBAL { get; set; }

                public string TIME { get; set; }

                public string TYPE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GHGROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_TIME { get; set; }

                public decimal? UNITCOST { get; set; }

                public string RX { get; set; }

                public string SP_INST { get; set; }

                public string CDOSE { get; set; }

                public string CINTERVAL { get; set; }

                public string CDURATION { get; set; }

                public bool? CAPITATED { get; set; }

                public string REFERENCE { get; set; }

                public decimal? unitpurvalue { get; set; }

                public bool? phtransferred { get; set; }





                #endregion Instance Properties
}
            public class INVLINK
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string TIMESENT { get; set; }

                public string DATEREC { get; set; }

                public string TIMEREC { get; set; }

                public string REFERENCE { get; set; }

                public string OPERATOR { get; set; }

                public string FACILITY { get; set; }

                public string DOCTOR { get; set; }

                public string CROSSREF { get; set; }

                public string GHGROUPCODE { get; set; }

                public string GROUPHEAD { get; set; }





                #endregion Instance Properties
}
            public class LABDET
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string SEX { get; set; }

                public string ADDRESS1 { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public string REFERAL { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string BILL_NO { get; set; }

                public string PAY_NO { get; set; }

                public string BILLSELF { get; set; }

                public string FACILITY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CROSSREF { get; set; }

                public string AGE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GHGROUPCODE { get; set; }

                public string OCCUPATION { get; set; }

                public decimal? AMOUNT { get; set; }

                public decimal? MAT_CUMAMT { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string PHONE { get; set; }

                public string EMAIL { get; set; }





                #endregion Instance Properties
}
            public class LABTRANS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string REF_SYMPTO { get; set; }

                public string PROCESS { get; set; }

                public string DESCRIPTION { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string RADRPT { get; set; }

                public string FACILITY { get; set; }

                public decimal? AMOUNT { get; set; }

                public string TESTBY { get; set; }

                public DateTime? TESTDATE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public string CURRENCY { get; set; }

                public string FILMUSED { get; set; }

                public decimal? views { get; set; }

                public bool? ondigitalequip { get; set; }





                #endregion Instance Properties
}
            public class LINK
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string FRSECTION { get; set; }

                public string TIMESENT { get; set; }

                public string TOSECTION { get; set; }

                public string DATEREC { get; set; }

                public string TIMEREC { get; set; }

                public string REFERENCE { get; set; }

                public decimal? CUMBIL { get; set; }

                public decimal? CUMPAY { get; set; }

                public string OPERATOR { get; set; }

                public string FACILITY { get; set; }

                public bool? LINKOK { get; set; }

                public decimal? PROCFUNC { get; set; }

                public string DOCTOR { get; set; }

                public bool? SENDEXCL { get; set; }

                public string TRANSFLAG { get; set; }





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
            public class LINK2
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? DEBIT { get; set; }

                public DateTime? CREDIT { get; set; }

                public string CTIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string VERIFIER { get; set; }

                public string VTIME { get; set; }

                public decimal? ITEMNO { get; set; }

                public DateTime? VDATE { get; set; }





                #endregion Instance Properties
}
            public class LINK3
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string SECTION { get; set; }

                public string TIMESENT { get; set; }

                public string TIMEIN { get; set; }

                public string TIMESELECTED { get; set; }

                public string REFERENCE { get; set; }

                public string OPERATOR { get; set; }

                public string FACILITY { get; set; }

                public string TIMESPENT { get; set; }

                public string RECTYPE { get; set; }





                #endregion Instance Properties
}
            public class LINK3HIST
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? posted { get; set; }

                public DateTime? post_date { get; set; }

                public string section { get; set; }

                public string timesent { get; set; }

                public string timein { get; set; }

                public string timeselected { get; set; }

                public string reference { get; set; }

                //public string operator { get; set; }

                public string facility { get; set; }

                public string timespent { get; set; }

                public string rectype { get; set; }





                #endregion Instance Properties
}
            public class LINKADM
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string CUSTNO { get; set; }

                public string FACILITY { get; set; }

                public string NAME { get; set; }

                public string ROOM { get; set; }

                public string BED { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? RATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class MEDHIST
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CTIME { get; set; }

                public string COMMENTS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string BILLED { get; set; }

                public string BILLREF { get; set; }

                public decimal? AMOUNT { get; set; }

                public string GROUPCODE { get; set; }

                public string WEIGHT { get; set; }

                public string HEIGHT { get; set; }

                public string BP_S { get; set; }

                public string BP_STN { get; set; }

                public string TEMP { get; set; }

                public string PULSE { get; set; }

                public string RESPIRATIO { get; set; }

                public string OTHERS { get; set; }

                public string DOCTOR { get; set; }

                public string CLINIC { get; set; }

                public decimal? PROTECTED { get; set; }

                public bool? ENCHRYPTED { get; set; }

                public string GHGROUPCODE { get; set; }

                public string GROUPHEAD { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class MEDHISTCHAIN
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string groupcode { get; set; }

                public string patientno { get; set; }

                public string name { get; set; }

                public DateTime? reg_date { get; set; }

                public bool? posted { get; set; }

                public DateTime? post_date { get; set; }

                public string grouphtype { get; set; }

                public string chainedGc { get; set; }

                public string chainedpatno { get; set; }

                public string gender { get; set; }





                #endregion Instance Properties
}
            public class MEDHISTMULTICONSULT
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string DOCTOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class MEDHPIC
            {
                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GROUPCODE { get; set; }

                public string PIC1 { get; set; }

                public string NOTE1 { get; set; }

                public string PIC2 { get; set; }

                public string NOTE2 { get; set; }

                public string PIC3 { get; set; }

                public string NOTE3 { get; set; }

                public string PIC4 { get; set; }

                public string NOTE4 { get; set; }

                public string PIC5 { get; set; }

                public string NOTE5 { get; set; }

                public int? TOTPIC { get; set; }

                public string FACILITY1 { get; set; }

                public string FACILITY2 { get; set; }

                public string FACILITY3 { get; set; }

                public string FACILITY4 { get; set; }

                public string FACILITY5 { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string pdffile1 { get; set; }

                public string pdffile2 { get; set; }

                public string pdffile3 { get; set; }

                public string pdffile4 { get; set; }

                public string pdffile5 { get; set; }





                #endregion Instance Properties
}

            public class MEDHREC
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string CLINIC { get; set; }

                public DateTime? DATE1 { get; set; }

                public DateTime? DATE2 { get; set; }

                public DateTime? DATE3 { get; set; }

                public DateTime? DATE4 { get; set; }

                public DateTime? DATE5 { get; set; }

                public DateTime? DATE6 { get; set; }

                public DateTime? DATE7 { get; set; }

                public string CLINIC1 { get; set; }

                public string CLINIC2 { get; set; }

                public string CLINIC3 { get; set; }

                public string CLINIC4 { get; set; }

                public string CLINIC5 { get; set; }

                public string CLINIC6 { get; set; }

                public string CLINIC7 { get; set; }

                public bool? FOLLOWUP { get; set; }





                #endregion Instance Properties
}
            public class MORTBILLS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PROCESS { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class MORTDETAIL
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public DateTime? REC_DATE { get; set; }

                public string REC_TIME { get; set; }

                public decimal? AGE { get; set; }

                public string SEX { get; set; }

                public string SOURCE { get; set; }

                public string FACILITY { get; set; }

                public string UNIT { get; set; }

                public string CHAMBERID { get; set; }

                public string REC_OFFICER { get; set; }

                public string REL_OFFICER { get; set; }

                public DateTime? REL_DATE { get; set; }

                public string REL_TIME { get; set; }

                public string COMMENTS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class MRATTEND
            {
           
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string CLINIC { get; set; }

                public string BILLED { get; set; }

                public string GROUPHEAD { get; set; }

                public string GROUPHTYPE { get; set; }

                public bool? VTAKEN { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string DOCTOR { get; set; }

                public string DOC_TIME { get; set; }

                public string DIAGNOSIS { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string ATTENDTYPE { get; set; }

                public bool? SENDEXCL { get; set; }

                public string REFERRER { get; set; }

                public string AUTHORIZEDCODE { get; set; }





                #endregion Instance Properties
}
            public class MORTSPACE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string FACILITY { get; set; }

                public string NAME { get; set; }

                public string UNIT { get; set; }

                public string CHAMBERID { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? RATE { get; set; }

                public string OCCUPANT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? BOOKED { get; set; }

                public DateTime? BOOKEDDATE { get; set; }





                #endregion Instance Properties
}
            public class MRB15
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string SDOW { get; set; }

                public string START { get; set; }

                public string TEND { get; set; }

                public string PATCAT { get; set; }

                public decimal? PMARK { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string DIAG { get; set; }

                public decimal? ITEMNO { get; set; }





                #endregion Instance Properties
}
            public class MRB15A
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string FACILITY { get; set; }

                public string NAME { get; set; }

                public string PROCESS { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string SERVICE { get; set; }





                #endregion Instance Properties
}
            public class MRB15FC
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string SDOW { get; set; }

                public decimal? START { get; set; }

                public decimal? TEND { get; set; }

                public string PATCAT { get; set; }

                public decimal? PMARK { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string DIAG { get; set; }

                public string CURRENCY { get; set; }





                #endregion Instance Properties
}
            public class MRB17
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public DateTime? DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TIME { get; set; }

                public string MASTPROCES { get; set; }

                public string PROCESS { get; set; }

                public string STK_ITEM { get; set; }

                public string DESCRIPTION { get; set; }

                public string FACILITY { get; set; }

                public decimal? QTY { get; set; }

                public decimal? AMOUNT { get; set; }

                public string UNIT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}


            public class MRB19
            {
                public int RECID { get; set; }

                public string FACILITY { get; set; }

                public string PROCESS { get; set; }

                public string ITEM { get; set; }

                public string DESCRIPTION { get; set; }

                public string STORE { get; set; }

                public decimal? QTY { get; set; }

                public string UNIT { get; set; }

                public decimal? COST { get; set; }

                public decimal? SELL { get; set; }

                public decimal? NUMBOFTEST { get; set; }

                public decimal? TESTCOUNT { get; set; }

                public bool? ONDIGITAL { get; set; }

                public bool? SELECTABLE { get; set; }

                public string OPERATOR { get; set; }

                public string DTIME { get; set; }

            }


            public class MRB20
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string SHORTCUT { get; set; }

                public string DESCRIPTION { get; set; }

                public string FORMATTED { get; set; }

                public string TYPE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTTIME { get; set; }





                #endregion Instance Properties
}
            public class MRB21
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public string SENDER { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string FACILITY { get; set; }

                public string NOTES { get; set; }

                public string RECEIVED { get; set; }

                public string OPERATOR { get; set; }

                public string REFERENCE { get; set; }

                public string SENDSECTION { get; set; }

                public string DOCTOR { get; set; }

                public bool? POSTED { get; set; }

                public string TOSECTION { get; set; }





                #endregion Instance Properties
}
            public class MRB21A
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string NAME { get; set; }

                public string SENDER { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string NOTES { get; set; }

                public string RECEIVED { get; set; }

                public string SENDSECTION { get; set; }

                public bool? POSTED { get; set; }

                public string TOSECTION { get; set; }

                public decimal? REPLAYAT { get; set; }

                public decimal? REPLAYDAYS { get; set; }

                public bool? resultalert { get; set; }





                #endregion Instance Properties
}
            public class MRB22
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string REFERENCE { get; set; }

                public string GENDER { get; set; }

                public string WLBS { get; set; }

                public string WSTONE { get; set; }

                public decimal? WEIGHT { get; set; }

                public string HFT { get; set; }

                public string HIN { get; set; }

                public decimal? HIGHT { get; set; }

                public string BPSITTING { get; set; }

                public string BPSTANDING { get; set; }

                public string PULSE { get; set; }

                public string TEMP { get; set; }

                public string RESPIRATIO { get; set; }

                public decimal? BMP { get; set; }

                public string REMARK { get; set; }

                public string CLINIC { get; set; }

                public string SP02 { get; set; }

                public string TIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string HAIRCOLOR { get; set; }

                public string HAIRTYPE { get; set; }

                public string EYECOLOR { get; set; }

                public string COMPLEXION { get; set; }

                public string RACIALGRP { get; set; }

                public string ETHNICITY { get; set; }

                public string RELIGION { get; set; }

                public string BLOODGRP { get; set; }





                #endregion Instance Properties
}
            public class MRB23
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string REFERENCE { get; set; }

                public string GENDER { get; set; }

                public string ORAL { get; set; }

                public string IV { get; set; }

                public string N_GASTRIC { get; set; }

                public string URINE { get; set; }

                public string VOMITUS { get; set; }

                public string OTHERS { get; set; }

                public string TIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string NATUREOFFLUID { get; set; }

                public string RECTRAL { get; set; }

                public string OTHERROUTES { get; set; }

                public string INFLO_TOTAL { get; set; }

                public string TUBE { get; set; }

                public string OUT_TOTAL { get; set; }

                public string CHLORIDE { get; set; }





                #endregion Instance Properties
}
            public class MRB24
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string REFERENCE { get; set; }

                public string GENDER { get; set; }

                public string RBS { get; set; }

                public string URINALYSIS { get; set; }

                public string ACETONE { get; set; }

                public string DRGADMIN { get; set; }

                public string REMARK { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTTIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TIME { get; set; }





                #endregion Instance Properties
}
            public class MRB25
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string STK_ITEM { get; set; }

                public string STK_DESC { get; set; }

                public decimal? QTY { get; set; }

                public string UNIT { get; set; }

                public decimal? ITEMNO { get; set; }

                public decimal? UNITCOST { get; set; }

                public decimal? COST { get; set; }

                public string REFERENCE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public string CURRENCY { get; set; }





                #endregion Instance Properties
}
            public class MRCONTROL
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string NAME { get; set; }

                public bool? INSTALLED { get; set; }

                public bool? ATTENDLINK { get; set; }

                public bool? DIAGONBILL { get; set; }

                public bool? OTHERCHARG { get; set; }

                public string PVTCODE { get; set; }

                public string APPUSER { get; set; }

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

                public bool? FACILAUTO { get; set; }

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

                public bool? PUBCORP { get; set; }

                public bool? PAYFLEX { get; set; }

                public bool? DIFFCHARGE { get; set; }

                public bool? MF1 { get; set; }

                public bool? RELOAD { get; set; }

                public DateTime? STATPERIOD { get; set; }

                public DateTime? ATTDATE { get; set; }

                public decimal? ATTNO { get; set; }

                public bool? GSORT { get; set; }

                public decimal? ADMIT { get; set; }

                public DateTime? ADMITDATE { get; set; }

                public string DISCHTIME { get; set; }

                public string CPREFIX { get; set; }

                public string SPREFIX { get; set; }

                public bool? AUTOGREG { get; set; }

                public bool? AUTOGCONS { get; set; }

                public decimal? DURACONSUL { get; set; }

                public string REGCODE { get; set; }

                public string CONSCODE { get; set; }

                public bool? CONSBYCLI { get; set; }

                public bool? BEFROMADM { get; set; }

                public bool? AUTORECEIP { get; set; }

                public bool? AUTOPHABIL { get; set; }

                public bool? AUTOMEDBIL { get; set; }

                public string FCCODE { get; set; }

                public decimal? LENGNUMB { get; set; }

                public bool? AUTOPGROUP { get; set; }

                public decimal? FCLASTNUMB { get; set; }

                public decimal? PVTLASTNUM { get; set; }

                public bool? SECLINK { get; set; }

                public bool? DOCSON { get; set; }

                public bool? FILEMODE { get; set; }

                public bool? DACTIVE { get; set; }

                public DateTime? LASTPROCES { get; set; }

                public bool? CASHPOINT { get; set; }

                public decimal? REGCONSPAY { get; set; }

                public string LOCALCUR { get; set; }

                public string LOCSTATE { get; set; }

                public string LOCCOUNTRY { get; set; }

                public string CUR_SIGN { get; set; }

                public string DTFORMAT { get; set; }

                public decimal? PAEDIACONS { get; set; }

                public bool? GLINTENABL { get; set; }

                public string ACTIVEGLINT { get; set; }

                public string GLCOMPANY { get; set; }

                public decimal? GLBATCHNO { get; set; }

                public string GLDOCUMENT { get; set; }

                public decimal? ALTERNATENO { get; set; }

                public int? REC_COUNT { get; set; }





                #endregion Instance Properties
}
            public class MRPT01
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string ADDRESS1 { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GHGROUPCODE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string SALUTATION { get; set; }

                public string RPTHEAD1 { get; set; }

                public string RPTHEAD2 { get; set; }

                public string DETAILS { get; set; }





                #endregion Instance Properties
}
            public class MRSETUP
            {




                #region Instance Properties

                public int RECID { get; set; }

                public decimal? OLDCHARGES { get; set; }

                public decimal? INACTIVEPA { get; set; }

                public bool? UPDATEPHON { get; set; }

                public decimal? ECGLEFT { get; set; }

                public decimal? ECGRIGHT { get; set; }

                public decimal? ECGPAPER { get; set; }

                public decimal? ECGLINESPA { get; set; }

                public bool? OBACKUPSTA { get; set; }

                public bool? OBACKUPEND { get; set; }

                public DateTime? OBACKDATE { get; set; }

                public string ECGCODE { get; set; }

                public string XRAYCODE { get; set; }

                public string SETDETAIL { get; set; }

                public string CREDITS { get; set; }

                public string AUTHSIGN { get; set; }

                public string FILES { get; set; }

                public string REPORTS { get; set; }

                public string UTILITIES { get; set; }

                public string FACILITY { get; set; }

                public string REQRPT { get; set; }

                public string RPTEXT { get; set; }

                public string DRUGCODE { get; set; }

                public string BILLCODE { get; set; }

                public string INVRPTCODE { get; set; }

                public bool? LEFTJUSTIFYHEADER { get; set; }

                public bool? frontdeskalert { get; set; }

                public bool? autorptheader { get; set; }





                #endregion Instance Properties
}
            public class MRSTLEV
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

                public bool? FM21 { get; set; }

                public bool? FM22 { get; set; }

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

                public int? WSECLEVEL { get; set; }

                public DateTime? PASSDATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string INITIAL { get; set; }

                public string SECTION { get; set; }

                public bool? CANDELETE { get; set; }

                public bool? CANALTER { get; set; }

                public bool? CANADD { get; set; }

                public bool? SHIELDPRICE { get; set; }

                public bool? CHANGEPRESC { get; set; }

                public int? HISTYEAR { get; set; }

                public string FACILITY { get; set; }

                public bool? FILEMENU { get; set; }

                public bool? REPORTMENU { get; set; }

                public bool? RM18 { get; set; }

                public bool? RM19 { get; set; }

                public bool? RM20 { get; set; }

                public bool? UM12 { get; set; }

                public bool? UM13 { get; set; }

                public bool? UM14 { get; set; }

                public bool? UM15 { get; set; }

                public bool? UM16 { get; set; }

                public bool? UM17 { get; set; }

                public bool? UM18 { get; set; }

                public bool? UM19 { get; set; }

                public bool? UM20 { get; set; }

                public string BRANCH { get; set; }





                #endregion Instance Properties
}
            public class MSDUTYR
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STAFF_NO { get; set; }

                public string NAME { get; set; }

                public string FACILITY { get; set; }

                public decimal? RMONTH { get; set; }

                public decimal? RYEAR { get; set; }

                public string RGROUP { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string DAY1 { get; set; }

                public string DAY2 { get; set; }

                public string DAY3 { get; set; }

                public string DAY4 { get; set; }

                public string DAY5 { get; set; }

                public string DAY6 { get; set; }

                public string DAY7 { get; set; }

                public string DAY8 { get; set; }

                public string DAY9 { get; set; }

                public string DAY10 { get; set; }

                public string DAY11 { get; set; }

                public string DAY12 { get; set; }

                public string DAY13 { get; set; }

                public string DAY14 { get; set; }

                public string DAY15 { get; set; }

                public string DAY16 { get; set; }

                public string DAY17 { get; set; }

                public string DAY18 { get; set; }

                public string DAY19 { get; set; }

                public string DAY20 { get; set; }

                public string DAY21 { get; set; }

                public string DAY22 { get; set; }

                public string DAY23 { get; set; }

                public string DAY24 { get; set; }

                public string DAY25 { get; set; }

                public string DAY26 { get; set; }

                public string DAY27 { get; set; }

                public string DAY28 { get; set; }

                public string DAY29 { get; set; }

                public string DAY30 { get; set; }

                public string DAY31 { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class MSDUTYRGRP
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string NAME { get; set; }





                #endregion Instance Properties
}
            public class NNOTES
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string UNITID { get; set; }

                public string STAFFID { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string NTIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string COMMENTS { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class OPDCONSULT
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string DOCTOR { get; set; }

                public string REFERENCE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime TRANS_DATE { get; set; }





                #endregion Instance Properties
}
            public class OVPROFILE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public string GROUPHEAD { get; set; }

                public string GROUPHTYPE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public decimal? CRLIMIT { get; set; }

                public decimal? BALANCE { get; set; }

                public string OVERWRITENOTE { get; set; }





                #endregion Instance Properties
}
            public class PASSTAB
            {

                #region Instance Properties

                public int RECID { get; set; }

                public string OPERATOR { get; set; }

                public string MODULE { get; set; }

                public string PROCESS { get; set; }

                public string PROCESS_RECORD { get; set; }

                public string PROCESS_TYPE { get; set; }

                public DateTime? DTIME { get; set; }

                public string PROCESS_TERMINAL { get; set; }





                #endregion Instance Properties
}
           
        public class PATDETAIL
        {
            public int RECID { get; set; }

            public string PATIENTNO { get; set; }

            public string GROUPCODE { get; set; }

            public string NATIONALITY { get; set; }

            public string OCCUPATION { get; set; }

            public string RELIGION { get; set; }

            public string BLOODGROUP { get; set; }

            public string GENOTYPE { get; set; }

            public string NEXTOFKIN { get; set; }

            public string NOK_ADR1 { get; set; }

            public string NOK_PHONE { get; set; }

            public string NOK_RELATIONSHIP { get; set; }

            public string TRIBE { get; set; }

            public string LGA { get; set; }

            public string PREVIOUSMEDNOTES { get; set; }

        }


        public class PATIENT
        {
            public int RECID { get; set; }

            public string PATIENTNO { get; set; }

            public string GROUPCODE { get; set; }

            public string PATSTATUS { get; set; }

            public string ADDRESS1 { get; set; }

            public string PATSTATE { get; set; }

            public DateTime? BIRTHDATE { get; set; }

            public string SEX { get; set; }

            public string M_STATUS { get; set; }

            public DateTime? REG_DATE { get; set; }

            public string CONTACT { get; set; }

            public string GHGROUPCODE { get; set; }

            public string PATTYPE { get; set; }

            public decimal? CR_LIMIT { get; set; }

            public string BILL_CIR { get; set; }

            public decimal? UPCUR_DB { get; set; }

            public decimal? UPCUR_CR { get; set; }

            public decimal? CUR_DB { get; set; }

            public decimal? CUR_CR { get; set; }

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

            public decimal? SEC_LEVEL { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public bool? POSTED { get; set; }

            public DateTime? POST_DATE { get; set; }

            public string NAME { get; set; }

            public string GROUPHEAD { get; set; }

            public string GROUPHTYPE { get; set; }

            public bool? ISGROUPHEAD { get; set; }

            public DateTime? LASTSTATMT { get; set; }

            public string TITLE { get; set; }

            public string PATCATEG { get; set; }

            public string REMARK { get; set; }

            public decimal? DISCOUNT { get; set; }

            public string HMOSERVTYPE { get; set; }

            public string CURRENCY { get; set; }

            public bool? BILLREGISTRATION { get; set; }

            public string CLINIC { get; set; }

            public string SURNAME { get; set; }

            public string OTHERNAME { get; set; }

            public string HOMEPHONE { get; set; }

            public string WORKPHONE { get; set; }

            public string EMPLOYER { get; set; }

            public string EMP_NAME { get; set; }

            public string EMP_ADDR { get; set; }

            public string EMP_STATE { get; set; }

            public string CONT_DESIGNATION { get; set; }

            public string PR_DOC { get; set; }

            public string REFER_DR { get; set; }

            public string NATIONALITY { get; set; }

            public string OCCUPATION { get; set; }

            public string RELIGION { get; set; }

            public string BLOODGROUP { get; set; }

            public string GENOTYPE { get; set; }

            public string NEXTOFKIN { get; set; }

            public string NOK_ADR1 { get; set; }

            public string NOK_STATE { get; set; }

            public string NOK_PHONE { get; set; }

            public string NOK_RELATIONSHIP { get; set; }

            public string RHD { get; set; }

            public string EMAIL { get; set; }

            public string PICLOCATION { get; set; }

        }



        public class PATPIC
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string PHOTOLOC { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class PATREREG
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHEAD { get; set; }

                public DateTime? EXPIRYDATE { get; set; }

                public DateTime? NEWEXPIRY { get; set; }

                public DateTime? DATEPAID { get; set; }

                public string RECEIPTNO { get; set; }

                public string GROUPHTYPE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string BILLREFERENCE { get; set; }

                public decimal? AMOUNT { get; set; }

                public string NAME { get; set; }

                public string GHGROUPCOD { get; set; }





                #endregion Instance Properties
}
            public class PAYDETAIL
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public decimal? ITEMNO { get; set; }

                public string DESCRIPTION { get; set; }

                public string DOCTOR { get; set; }

                public string FACILITY { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? RECEIPTED { get; set; }

                public string TRANSTYPE { get; set; }

                public string GROUPHEAD { get; set; }

                public string SERVICETYPE { get; set; }

                public string GROUPCODE { get; set; }

                public string TTYPE { get; set; }

                public string GHGROUPCODE { get; set; }

                public string PAYTYPE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_TIME { get; set; }

                public string ACCOUNTTYPE { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public string EXTDESC { get; set; }

                public DateTime? DATERECEIVED { get; set; }

                public string CROSSREF { get; set; }

                public bool? PVTDEPOSIT { get; set; }





                #endregion Instance Properties
}
            public class PDEVELOP
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public DateTime? DATETAKEN { get; set; }

                public string GROSSMOTOR { get; set; }

                public string FINEMOTOR { get; set; }

                public string LANGUAGE { get; set; }

                public string PERSONAL { get; set; }

                public bool? POSTED { get; set; }

                public string POST_DATE { get; set; }

                public string AGE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string COMMENTS { get; set; }





                #endregion Instance Properties
}
            public class PGROWTH
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public DateTime? DATETAKEN { get; set; }

                public decimal? WEIGHT { get; set; }

                public decimal? HEIGHT { get; set; }

                public string TEMP { get; set; }

                public string REMARKS { get; set; }

                public bool? POSTED { get; set; }

                public string POST_DATE { get; set; }

                public string AGE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string OFC { get; set; }

                public string NUTRITION { get; set; }

                public string COMMENTS { get; set; }





                #endregion Instance Properties
}
            public class PHL01
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string SEX { get; set; }

                public string ADDRESS1 { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public string DOCTOR { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string BILLSELF { get; set; }

                public string FACILITY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string CROSSREF { get; set; }

                public decimal? AGE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GHGROUPCODE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string SAMPLEBY { get; set; }

                public DateTime? SAMPLEDATE { get; set; }

                public string OTHERS { get; set; }

                public string REQPROFILE { get; set; }

                public string DEFAULTSTRING { get; set; }

                public string TEXTAGE { get; set; }





                #endregion Instance Properties
}
            public class PMEDH01
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string DISEASE { get; set; }

                public string DETAILS { get; set; }

                public string DISEASECODE { get; set; }





                #endregion Instance Properties
}
            public class PMEDH02
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? ALCOHOL { get; set; }

                public bool? SMOKING { get; set; }

                public bool? SOC_OTHERS { get; set; }

                public string SOC_ALCOH_DTL { get; set; }

                public string SOC_SMOK_DTL { get; set; }

                public string SOC_OTH_DTL { get; set; }

                public bool? FAM_HYPERTENSION { get; set; }

                public string FAM_HYP_DTL { get; set; }

                public bool? FAM_DIABETES { get; set; }

                public string FAM_DIAB_DTL { get; set; }

                public bool? FAM_SICKLE_CELL { get; set; }

                public string FAM_SICKL_DTL { get; set; }

                public bool? FAM_GENETIC { get; set; }

                public string FAM_GENET_DTL { get; set; }

                public bool? FAM_OTHERS { get; set; }

                public string FAMILYDETAILS { get; set; }

                public string AP_PROGUANIL { get; set; }

                public string AP_PYRIMETHAMINE1 { get; set; }

                public string AP_PYRIMETHAMINE2 { get; set; }

                public string AP_PYRIMETHAMINE3 { get; set; }

                public string AP_OTHERS { get; set; }

                public string TETANUS1 { get; set; }

                public string TETANUS2 { get; set; }

                public string TETANUS3 { get; set; }

                public string HISTORY { get; set; }

                public string GEN_CONDITION { get; set; }

                public string RESPIRATORY { get; set; }

                public string CARDIO_VASCULAR { get; set; }

                public string ABDOMEN { get; set; }

                public string SPLEEN { get; set; }

                public string LIVER { get; set; }

                public string COMMENTS { get; set; }

                public string SPINSTRUCTIONS { get; set; }

                public string HEIGHT { get; set; }

                public string WEIGHT { get; set; }

                public string BP { get; set; }

                public string URINE { get; set; }

                public string HPLEVEL { get; set; }

                public string GENOTYPE { get; set; }

                public string P_RH { get; set; }

                public string DOCTOR { get; set; }





                #endregion Instance Properties
}
            public class PMEDH04
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string FACILITY { get; set; }

                public string PROCESS { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string RESULT { get; set; }





                #endregion Instance Properties
}
            public class PMEDHDIAG
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string PROVISIONAL { get; set; }

                public string FINAL { get; set; }

                public string TRANSTYPE { get; set; }

                public string REFERENCE { get; set; }





                #endregion Instance Properties
}
            public class PP001
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string SURNAME { get; set; }

                public string FORENAME { get; set; }

                public string NAME { get; set; }

                public string CONSOBS { get; set; }

                public string HOMEADRESS { get; set; }

                public string OFFICEADDRESS { get; set; }

                public string HOMEPHONE { get; set; }

                public string OFFICEPHONE { get; set; }

                public string PASTMEDHIST { get; set; }

                public string MEDICATION { get; set; }

                public string SEROLOGY { get; set; }

                public string PARITY { get; set; }

                public decimal? PREVIOUSPREG { get; set; }

                public decimal? NUMBERALIVE { get; set; }

                public DateTime? LMP { get; set; }

                public DateTime? EDD { get; set; }

                public string DURATIONFRM { get; set; }

                public string TEMP { get; set; }

                public string ANTIBIOTIC { get; set; }

                public string ANTIIBIOTICTIME { get; set; }

                public string NARCOTIC { get; set; }

                public string DELIVERYTYPE { get; set; }

                public string DELIVERYNOTES { get; set; }

                public string BLOODGROUP { get; set; }

                public DateTime? DEL_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public DateTime? REG_DATE { get; set; }

                public string REG_TIME { get; set; }

                public string DOCTOR { get; set; }

                public string DURATIONOFPREGNANCY { get; set; }

                public decimal? AGE { get; set; }

                public string OCCUPATION { get; set; }

                public string HUSBANDNAME { get; set; }

                public string HUSBANDOCCUPATION { get; set; }

                public string HUSBANDGC { get; set; }

                public string HUSBANDBG { get; set; }

                public string GHGROUPCOD { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string SPNOTES { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public string GENOTYPE { get; set; }

                public string MOTHERGC { get; set; }

                public string MOTHERPATNO { get; set; }

                public string COMMENTS { get; set; }





                #endregion Instance Properties
}
            public class PP002
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string NAME { get; set; }

                public DateTime? BIRTHDATE { get; set; }

                public string BIRTHTIME { get; set; }

                public decimal? WEIGHT { get; set; }

                public string APGARSCORE { get; set; }

                public string SEX { get; set; }

                public decimal? LENGHT { get; set; }

                public string NURSE { get; set; }

                public string DOCTOR { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string REMARKS { get; set; }

                public string BIRTHTYPE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string GESTATION { get; set; }

                public decimal? HEADCIRCUMF { get; set; }

                public string FOETALDISTRESS { get; set; }

                public decimal? FIRSTBREATH { get; set; }





                #endregion Instance Properties
}
            public class PP003
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string DURATIONFRM { get; set; }

                public string TEMP { get; set; }

                public string ANTIBIOTIC { get; set; }

                public string ANTIBIOTICTIME { get; set; }

                public string NARCOTIC { get; set; }

                public string NARCOTICTIME { get; set; }

                public bool? SPONTANEOUS { get; set; }

                public bool? FORCEPS { get; set; }

                public bool? VACUUM { get; set; }

                public bool? BREECH { get; set; }

                public bool? ELECTIVE { get; set; }

                public bool? EMERGENCY { get; set; }

                public bool? GENANAESTHETIC { get; set; }

                public bool? SPINAL { get; set; }

                public bool? BBA { get; set; }

                public string DELIVERYNOTES { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class PP004
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string EXAMAT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TIME { get; set; }

                public decimal? ANAEMIA { get; set; }

                public decimal? JAUNDICE { get; set; }

                public decimal? HEAD { get; set; }

                public decimal? FACIES { get; set; }

                public decimal? REDREFLEX { get; set; }

                public decimal? EYES { get; set; }

                public decimal? EARS { get; set; }

                public decimal? NOSE { get; set; }

                public decimal? MOUTH { get; set; }

                public decimal? PALATE { get; set; }

                public decimal? NECK { get; set; }

                public decimal? SKIN { get; set; }

                public decimal? UMBILICUS { get; set; }

                public decimal? CVSFP { get; set; }

                public decimal? HEARTSOUNDS { get; set; }

                public decimal? MURMURS { get; set; }

                public decimal? CHESTAUS { get; set; }

                public decimal? ABDOMEN { get; set; }

                public decimal? SPLEEN { get; set; }

                public decimal? LIVER { get; set; }

                public decimal? KIDNEYS { get; set; }

                public decimal? GENITALIA { get; set; }

                public decimal? ANUS { get; set; }

                public decimal? SPINE { get; set; }

                public decimal? ARMS { get; set; }

                public decimal? HANDS { get; set; }

                public decimal? LEGS { get; set; }

                public decimal? FEET { get; set; }

                public decimal? HIPS { get; set; }

                public decimal? POSTURE { get; set; }

                public decimal? MUSCLETONE { get; set; }

                public decimal? GRASP { get; set; }

                public decimal? MORO { get; set; }

                public decimal? CRY { get; set; }

                public string EXAMINER { get; set; }

                public string COMMENTS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class PP005
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string WEIGHT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TIME { get; set; }

                public string FEEDING { get; set; }

                public decimal? ANAEMIA { get; set; }

                public decimal? JAUNDICE { get; set; }

                public decimal? HEAD { get; set; }

                public decimal? FACIES { get; set; }

                public decimal? REDREFLEX { get; set; }

                public decimal? EYES { get; set; }

                public decimal? EARS { get; set; }

                public decimal? NOSE { get; set; }

                public decimal? MOUTH { get; set; }

                public decimal? PALATE { get; set; }

                public decimal? NECK { get; set; }

                public decimal? SKIN { get; set; }

                public decimal? UMBILICUS { get; set; }

                public decimal? CVSFP { get; set; }

                public decimal? HEARTSOUNDS { get; set; }

                public decimal? MURMURS { get; set; }

                public decimal? CHESTAUS { get; set; }

                public decimal? ABDOMEN { get; set; }

                public decimal? SPLEEN { get; set; }

                public decimal? LIVER { get; set; }

                public decimal? KIDNEYS { get; set; }

                public decimal? GENITALIA { get; set; }

                public decimal? ANUS { get; set; }

                public decimal? SPINE { get; set; }

                public decimal? ARMS { get; set; }

                public decimal? HANDS { get; set; }

                public decimal? LEGS { get; set; }

                public decimal? FEET { get; set; }

                public decimal? HIPS { get; set; }

                public decimal? POSTURE { get; set; }

                public decimal? MUSCLETONE { get; set; }

                public decimal? GRASP { get; set; }

                public decimal? MORO { get; set; }

                public decimal? CRY { get; set; }

                public string EXAMINER { get; set; }

                public string COMMENTS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class PPDETAIL
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPCODE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string STK_ITEM { get; set; }

                public string STK_DESC { get; set; }

                public string STORE { get; set; }

                public decimal? QTY_PR { get; set; }

                public decimal? QTY_GV { get; set; }

                public decimal? CUMGV { get; set; }

                public decimal? DOSE { get; set; }

                public decimal? INTERVAL { get; set; }

                public decimal? DURATION { get; set; }

                public string UNIT { get; set; }

                public decimal? COST { get; set; }

                public string NURSE { get; set; }

                public string DOCTOR { get; set; }

                public string DIAG { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? ITEMNO { get; set; }

                public string NAME { get; set; }

                public decimal? STKBAL { get; set; }

                public string TIME { get; set; }

                public string TYPE { get; set; }

                public string GROUPHEAD { get; set; }

                public string GHGROUPCODE { get; set; }

                public string GROUPHTYPE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OP_TIME { get; set; }

                public decimal? UNITCOST { get; set; }

                public string RX { get; set; }

                public string SP_INST { get; set; }

                public string REFERENCE { get; set; }





                #endregion Instance Properties
}
            public class PPDRESSING
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string PROCESS { get; set; }

                public string DESCRIPTION { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? OPTIME { get; set; }

                public string CROSSREF { get; set; }

                public bool? MONDAY { get; set; }

                public bool? TUESDAY { get; set; }

                public bool? WEDNESDAY { get; set; }

                public bool? THURSDAY { get; set; }

                public bool? FRIDAY { get; set; }

                public bool? SATURDAY { get; set; }

                public bool? SUNDAY { get; set; }

                public DateTime? TMTDATE { get; set; }

                public string TIMEGIVEN { get; set; }

                public decimal? ITEMNO { get; set; }





                #endregion Instance Properties
}
            public class PPDRESSINGDTL
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public decimal? ITEMNO { get; set; }

                public string DESCRIPTION { get; set; }

                public string RECTYPE { get; set; }

                public string PROCESS { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPHEAD { get; set; }

                public string TRANSTYPE { get; set; }

                public string DOCTOR { get; set; }

                public string FACILITY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? AMOUNT { get; set; }

                public string GHGROUPCODE { get; set; }

                public string TITLE { get; set; }

                public string ADDRESS1 { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public decimal? DURATION { get; set; }

                public string BILLPROCESS { get; set; }

                public string NOTES { get; set; }

                public string SERVICETYPE { get; set; }

                public bool? CAPITATED { get; set; }

                public bool? GROUPEDITEM { get; set; }

                public bool? GRPBILLBYSERVTYPE { get; set; }

                public string AGE { get; set; }

                public string SEX { get; set; }

                public string PHONE { get; set; }

                public string EMAIL { get; set; }





                #endregion Instance Properties
}
            public class PROCPROFILE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string PROCESS { get; set; }

                public decimal? AMOUNT { get; set; }

                public string OTHERS { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? CAPITATED { get; set; }

                public bool? AUTHORIZATIONREQUIRED { get; set; }

                public string NAME { get; set; }





                #endregion Instance Properties
}
            public class PSPNOTES
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string SPNOTES { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string MEDNOTES { get; set; }





                #endregion Instance Properties
}
            public class PUBHOLIDAYS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public DateTime PDATE { get; set; }

                public string DESCRIPTION { get; set; }





                #endregion Instance Properties
}
            public class PVRPTDEF
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string NAME { get; set; }

                public string ADDRESS1 { get; set; }

                public string ADDRESS2 { get; set; }

                public decimal? CUR_BILLS { get; set; }

                public decimal? BALBF { get; set; }

                public decimal? PAYMENTS { get; set; }

                public decimal? DB_NOTES { get; set; }

                public decimal? CR_NOTES { get; set; }





                #endregion Instance Properties
}
            public class REFERRALS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string FACILITY { get; set; }

                public decimal? PERCENTAGE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTTIME { get; set; }





                #endregion Instance Properties
}
            public class REMOTEDATA
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string LOCATION { get; set; }

                public string SERVERNAME { get; set; }

                public string SERVER_IPS { get; set; }

                public string DATAFOLDER { get; set; }

                public DateTime? START_DATE { get; set; }

                public string LAST_TRANSFERS { get; set; }





                #endregion Instance Properties
}
            public class ROUTDRGS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string DRUGS { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? QTY { get; set; }

                public string UNIT { get; set; }

                public decimal? COST { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? GLOBAL_DIFF_CHG { get; set; }

                public bool? CORP_DIFF_CHG { get; set; }

                public decimal? DOSE { get; set; }

                public decimal? INTERVAL { get; set; }

                public decimal? DURATION { get; set; }

                public string CDOSE { get; set; }

                public string CINTERVAL { get; set; }

                public string CDURATION { get; set; }

                public string WHENHOW { get; set; }





                #endregion Instance Properties
}
            public class RPTDEF
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string DESCRIPTION { get; set; }

                public string ITEM1 { get; set; }

                public string ITEM2 { get; set; }

                public string ITEM3 { get; set; }

                public string ITEM4 { get; set; }

                public string ITEM5 { get; set; }

                public string ITEM6 { get; set; }

                public string ITEM7 { get; set; }

                public string ITEM8 { get; set; }

                public string ITEM9 { get; set; }

                public string ITEM10 { get; set; }

                public string ITEM11 { get; set; }

                public string ITEM12 { get; set; }

                public string ITEM13 { get; set; }

                public string ITEM14 { get; set; }





                #endregion Instance Properties
}
            public class SMSTEXTPROFILE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string PROFILEID { get; set; }

                public string email_text { get; set; }

                public string sms_text { get; set; }

                public bool? email_apply { get; set; }

                public bool? sms_apply { get; set; }

                //public string operator { get; set; }

                public DateTime? dtime { get; set; }

                public int? reminderday { get; set; }





                #endregion Instance Properties
}
            public class SPCDETAIL
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string FACILITY { get; set; }

                public string CUSTOMER { get; set; }

                public string NAME { get; set; }

                public decimal? AMOUNT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public decimal? followupamt { get; set; }

                public int? followupdays { get; set; }

                public bool? samemonth { get; set; }





                #endregion Instance Properties
}
            public class SPCPROFILE
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string FACILITY { get; set; }

                public string NAME { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public bool? CORPORATE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? DIFFCHARGE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public decimal? followupamt { get; set; }

                public int? followupdays { get; set; }

                public bool? samemonth { get; set; }





                #endregion Instance Properties
}
            public class STKCHARG
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string ITEM { get; set; }

                public string PATCATEG { get; set; }

                public decimal? AMOUNT { get; set; }

                public decimal? PERCENTCHARGE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string PATCATEGDESC { get; set; }





                #endregion Instance Properties
}
            public class STKREQUESTS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string COSTCENTRE { get; set; }

                public string STORE { get; set; }

                public string ITEM { get; set; }

                public string DESCRIPTION { get; set; }

                public decimal? TRANS_QTY { get; set; }

                public decimal? STOCK_BAL { get; set; }

                public decimal? COST { get; set; }

                public decimal? SELL { get; set; }

                public bool? POSTED { get; set; }

                public string REQUESTBY { get; set; }

                public string TYPE { get; set; }

                public string UNIT { get; set; }

                public string REQUESTTO { get; set; }

                public decimal? ACTUAL { get; set; }

                public string GIVENBY { get; set; }

                public string DATEGIVEN { get; set; }

                public decimal? WHSELL { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }

                public string ADM_REFERENCE { get; set; }

                public string PURPOSE { get; set; }





                #endregion Instance Properties
}
            public class STKTRANS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TRANSTYPE { get; set; }

                public string STORE { get; set; }

                public string ITEM { get; set; }

                public string DESCRIPTION { get; set; }

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





                #endregion Instance Properties
}

            public class STOCKMAS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string STORE { get; set; }

                public string ITEM { get; set; }

                public string TYPE { get; set; }

                public string NAME { get; set; }

                public decimal? STOCK_QTY { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string OFFICER { get; set; }

                public decimal? COST { get; set; }

                public decimal? SELL { get; set; }

                public string UNIT { get; set; }

                public decimal? SELLUNIT { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public string SELLDESC { get; set; }

                public string CROSSREF { get; set; }





                #endregion Instance Properties
}
            public class SURGERYDETAILS
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string FACILITY { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string TIME { get; set; }

                public string TOPERATION { get; set; }

                public string TYPE { get; set; }

                public string SEXOFBABY { get; set; }

                public decimal? BWEIGHT { get; set; }

                public string SURGEON { get; set; }

                public string ASSTSURGEON { get; set; }

                public string ANEASTETIST { get; set; }

                public string ANEASTESIA { get; set; }

                public string BRIEFREMARK { get; set; }

                public string ALIFEDEATH { get; set; }

                public string NURSE { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }





                #endregion Instance Properties
}
            public class SUSPENSE
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public decimal? ITEMNO { get; set; }

                public string DESCRIPTION { get; set; }

                public string RECTYPE { get; set; }

                public string PROCESS { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string GROUPHEAD { get; set; }

                public string TRANSTYPE { get; set; }

                public string DOCTOR { get; set; }

                public string FACILITY { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? AMOUNT { get; set; }

                public string GHGROUPCODE { get; set; }

                public string TITLE { get; set; }

                public string ADDRESS1 { get; set; }

                public string CURRENCY { get; set; }

                public decimal? EXRATE { get; set; }

                public decimal? FCAMOUNT { get; set; }

                public decimal? DURATION { get; set; }

                public string BILLPROCESS { get; set; }

                public string NOTES { get; set; }

                public string SERVICETYPE { get; set; }

                public bool? CAPITATED { get; set; }

                public bool? GROUPEDITEM { get; set; }

                public bool? GRPBILLBYSERVTYPE { get; set; }

                public string AGE { get; set; }

                public string SEX { get; set; }

                public string PHONE { get; set; }

                public string EMAIL { get; set; }





                #endregion Instance Properties
}
            public class TARIFF
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string REFERENCE { get; set; }

                public string NAME { get; set; }

                public string STATMT_DES { get; set; }

                public string CATEGORY { get; set; }

                public string SUBCATEGORY { get; set; }

                public decimal? AMOUNT { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public decimal? SEC_LEVEL { get; set; }

                public decimal? CATP { get; set; }

                public decimal? CATC { get; set; }

                public decimal? CATV { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public bool? DIFFCHARGE { get; set; }

                public bool? AVAILABLE { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? DTIME { get; set; }





                #endregion Instance Properties
}
            public class TEMPLATEGRP
            {




                #region Instance Properties

                public int RECID { get; set; }

                public string DESCRIPTION { get; set; }





                #endregion Instance Properties
}
            public class VSTATA
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string GROUPCODE { get; set; }

                public string PATIENTNO { get; set; }

                public string REFERENCE { get; set; }

                public string GENDER { get; set; }

                public string WLBS { get; set; }

                public string WSTONE { get; set; }

                public decimal? WEIGHT { get; set; }

                public string HFT { get; set; }

                public string HIN { get; set; }

                public decimal? HIGHT { get; set; }

                public string BPSITTING { get; set; }

                public string BPSTANDING { get; set; }

                public string PULSE { get; set; }

                public string TEMP { get; set; }

                public string RESPIRATIO { get; set; }

                public decimal? BMP { get; set; }

                public string OTHERS { get; set; }

                public string CLINIC { get; set; }

                public string DOCTOR { get; set; }

                public string TIME { get; set; }

                public bool? POSTED { get; set; }

                public DateTime? POST_DATE { get; set; }

                public DateTime? TRANS_DATE { get; set; }

                public string HAIRCOLOR { get; set; }

                public string HAIRTYPE { get; set; }

                public string EYECOLOR { get; set; }

                public string COMPLEXION { get; set; }

                public string RACIALGRP { get; set; }

                public string ETHNICITY { get; set; }

                public string RELIGION { get; set; }

                public string BLOODGRP { get; set; }

                public string COMPLAINT { get; set; }

                public decimal? HEADCIRCUMF { get; set; }

                public string OPERATOR { get; set; }

                public DateTime? opdttime { get; set; }





                #endregion Instance Properties
}

            public class WORDLST1
            {

                #region Instance Properties

                public int RECID { get; set; }

                public string WORD { get; set; }





                #endregion Instance Properties
}

            public class WORDLST2
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string WORD { get; set; }





                #endregion Instance Properties
}
            public class WORDLST3
            {
            
                #region Instance Properties

                public int RECID { get; set; }

                public string WORD { get; set; }





                #endregion Instance Properties
}

        
            public class MR_DATAvm
            {
                public ADMDETAI ADMDETAI { get; set; }
                public ADMRECS ADMRECS { get; set; }
                public ADMSPACE ADMSPACE { get; set; }
                public ANC01 ANC01 { get; set; }
                public ANC02 ANC02 { get; set; }
                public ANC03 ANC03 { get; set; }
                public ANC03A ANC03A { get; set; }
                public ANC04 ANC04 { get; set; }
                public ANC05 ANC05 { get; set; }
                public ANC06 ANC06 { get; set; }
                public ANC07 ANC07 { get; set; }
                public ANC07A ANC07A { get; set; }
                public ANC07B ANC07B { get; set; }
                public ANC07C ANC07C { get; set; }
                public ANC07D ANC07D { get; set; }
                public ANC08 ANC08 { get; set; }
                public ANC09 ANC09 { get; set; }
                public ANCEXEMP ANCEXEMP { get; set; }
                public ANCREG ANCREG { get; set; }
                public APGARSCORE APGARSCORE { get; set; }
                public APPT APPT { get; set; }
                public ASTNOTES ASTNOTES { get; set; }
                public ATMPROFILE ATMPROFILE { get; set; }
                public ATTDFRM ATTDFRM { get; set; }
                public BILL_ADJ BILL_ADJ { get; set; }
                public BILLAUX BILLAUX { get; set; }
                public BILLCHAIN BILLCHAIN { get; set; }
                public BILLING BILLING { get; set; }
                public BILLSET BILLSET { get; set; }
                public BILLVOUC BILLVOUC { get; set; }
                public BIRTHS BIRTHS { get; set; }
                public BSDETAIL BSDETAIL { get; set; }
                public CAPBILLCHAIN CAPBILLCHAIN { get; set; }
                public CAPBILLS CAPBILLS { get; set; }
                public cdmdetails cdmdetails { get; set; }
                public CONSULTRMDETAILS CONSULTRMDETAILS { get; set; }
                public CONSULTROOMS CONSULTROOMS { get; set; }
                public CPRPTDEF CPRPTDEF { get; set; }
                public CUSTCLASS CUSTCLASS { get; set; }
                public CUSTOMER CUSTOMER { get; set; }
                public DEATHS DEATHS { get; set; }
                public DEPT DEPT { get; set; }
                public DGPROFILE DGPROFILE { get; set; }
                public DISPENSA DISPENSA { get; set; }
                public DISPSERV DISPSERV { get; set; }
                public DOCTORS DOCTORS { get; set; }
                public DUENEXT DUENEXT { get; set; }
                public DUTYRTAB DUTYRTAB { get; set; }
                public ECG ECG { get; set; }
                public FCCUSTOM FCCUSTOM { get; set; }
                public FCPATIENT FCPATIENT { get; set; }
                public FCSTKCHARG FCSTKCHARG { get; set; }
                public FCTARIFF FCTARIFF { get; set; }
                public FFSC01 FFSC01 { get; set; }
                public FFSC01B FFSC01B { get; set; }
                public FFSC02 FFSC02 { get; set; }
                public FFSC03 FFSC03 { get; set; }
                public FFSC04 FFSC04 { get; set; }
                public FFSC05 FFSC05 { get; set; }
                public FFSC06 FFSC06 { get; set; }
                public ffsformprofiler ffsformprofiler { get; set; }
                public GLINT GLINT { get; set; }
                public GLINTAB1 GLINTAB1 { get; set; }
                public GLINTAB2 GLINTAB2 { get; set; }
                public GLINTAB3 GLINTAB3 { get; set; }
                public GLINTAB4 GLINTAB4 { get; set; }
                public GRPPROCEDURE GRPPROCEDURE { get; set; }
                public HMOAUTHORIZATIONS HMOAUTHORIZATIONS { get; set; }
                public HMODETAIL HMODETAIL { get; set; }
                public HMOSERVIC HMOSERVIC { get; set; }
                public IBR001 IBR001 { get; set; }
                public ICD10 ICD10 { get; set; }
                public ICPC2 ICPC2 { get; set; }
                public IMMI01 IMMI01 { get; set; }
                public IMMI02 IMMI02 { get; set; }
                public IMMI03 IMMI03 { get; set; }
                public IMMI03M IMMI03M { get; set; }
                public IMMI04 IMMI04 { get; set; }
                public IMMI05 IMMI05 { get; set; }
                public IMMI07M IMMI07M { get; set; }
                public IMMUNTAB IMMUNTAB { get; set; }
                public IMUNES IMUNES { get; set; }
                public INJCARD INJCARD { get; set; }
                public INPDISPENSA INPDISPENSA { get; set; }
                public INVLINK INVLINK { get; set; }
                public LABDET LABDET { get; set; }
                public LABTRANS LABTRANS { get; set; }
                public LINK LINK { get; set; }
                public LINK1 LINK1 { get; set; }
                public LINK2 LINK2 { get; set; }
                public LINK3 LINK3 { get; set; }
                public LINK3HIST LINK3HIST { get; set; }
                public LINKADM LINKADM { get; set; }
                public MEDHIST MEDHIST { get; set; }
                public MEDHISTCHAIN MEDHISTCHAIN { get; set; }
                public MEDHISTMULTICONSULT MEDHISTMULTICONSULT { get; set; }
                public MEDHPIC MEDHPICPROP { get; set; }
                public MEDHREC MEDHREC { get; set; }
                public MORTBILLS MORTBILLS { get; set; }
                public MORTDETAIL MORTDETAIL { get; set; }
                public MORTSPACE MORTSPACE { get; set; }
                public MRATTEND MRATTEND { get; set; }
                public MRB15 MRB15 { get; set; }
                public MRB15A MRB15A { get; set; }
                public MRB15FC MRB15FC { get; set; }
                public MRB17 MRB17 { get; set; }
                public MRB19 MRB19 { get; set; }
                public MRB20 MRB20 { get; set; }
                public MRB21 MRB21 { get; set; }
                public MRB21A MRB21A { get; set; }
                public MRB22 MRB22 { get; set; }
                public MRB23 MRB23 { get; set; }
                public MRB24 MRB24 { get; set; }
                public MRB25 MRB25 { get; set; }
                public MRCONTROL MRCONTROL { get; set; }
                public MRPT01 MRPT01 { get; set; }
                public MRSETUP MRSETUP { get; set; }
                public MRSTLEV MRSTLEV { get; set; }
                public MSDUTYR MSDUTYR { get; set; }
                public MSDUTYRGRP MSDUTYRGRP { get; set; }
                public NNOTES NNOTES { get; set; }
                public OPDCONSULT OPDCONSULT { get; set; }
                public OVPROFILE OVPROFILE { get; set; }
                public PASSTAB PASSTAB { get; set; }
                public PATDETAIL PATDETAIL { get; set; }
                public PATIENT PATIENT { get; set; }
                public PATPIC PATPIC { get; set; }
                public PATREREG PATREREG { get; set; }
                public PAYDETAIL PAYDETAIL { get; set; }
                public PDEVELOP PDEVELOP { get; set; }
                public PGROWTH PGROWTH { get; set; }
                public PHL01 PHL01 { get; set; }
                public PMEDH01 PMEDH01 { get; set; }
                public PMEDH02 PMEDH02 { get; set; }
                public PMEDH04 PMEDH04 { get; set; }
                public PMEDHDIAG PMEDHDIAG { get; set; }
                public PP001 PP001 { get; set; }
                public PP002 PP002 { get; set; }
                public PP003 PP003 { get; set; }
                public PP004 PP004 { get; set; }
                public PP005 PP005 { get; set; }
                public PPDETAIL PPDETAIL { get; set; }
                public PPDRESSING PPDRESSING { get; set; }
                public PPDRESSINGDTL PPDRESSINGDTL { get; set; }
                public PROCPROFILE PROCPROFILE { get; set; }
                public PSPNOTES PSPNOTES { get; set; }
                public PUBHOLIDAYS PUBHOLIDAYS { get; set; }
                public ROUTDRGS ROUTDRGS { get; set; }
                public RPTDEF RPTDEF { get; set; }
                public SMSTEXTPROFILE SMSTEXTPROFILE { get; set; }
                public SPCDETAIL SPCDETAIL { get; set; }
                public SPCPROFILE SPCPROFILE { get; set; }
                public STKCHARG STKCHARG { get; set; }
                public STKREQUESTS STKREQUESTS { get; set; }
                public STKTRANS STKTRANS { get; set; }
                public STOCKMAS STOCKMAS { get; set; }
                public SURGERYDETAILS SURGERYDETAILS { get; set; }
                public SUSPENSE SUSPENSE { get; set; }
                public TARIFF TARIFF { get; set; }
                public TEMPLATEGRP TEMPLATEGRP { get; set; }
                public VSTATA VSTATA { get; set; }
                public WORDLST1 WORDLST1 { get; set; }
                public WORDLST2 WORDLST2 { get; set; }
                public WORDLST3 WORDLST3 { get; set; }
                public REPORTS REPORTS { get; set; }




                public IEnumerable<REPORTS> REPORTSS { get; set; }
                public IEnumerable<ADMDETAI> ADMDETAIS { get; set; }
                public IEnumerable<ADMRECS> ADMRECSS { get; set; }
                public IEnumerable<ADMSPACE> ADMSPACES { get; set; }
                public IEnumerable<ANC01> ANC01S { get; set; }
                public IEnumerable<ANC02> ANC02S { get; set; }
                public IEnumerable<ANC03> ANC03S { get; set; }
                public IEnumerable<ANC03A> ANC03AS { get; set; }
                public IEnumerable<ANC04> ANC04S { get; set; }
                public IEnumerable<ANC05> ANC05S { get; set; }
                public IEnumerable<ANC06> ANC06S { get; set; }
                public IEnumerable<ANC07> ANC07S { get; set; }
                public IEnumerable<ANC07A> ANC07AS { get; set; }
                public IEnumerable<ANC07B> ANC07BS { get; set; }
                public IEnumerable<ANC07C> ANC07CS { get; set; }
                public IEnumerable<ANC07D> ANC07DS { get; set; }
                public IEnumerable<ANC08> ANC08S { get; set; }
                public IEnumerable<ANC09> ANC09S { get; set; }
                public IEnumerable<ANCEXEMP> ANCEXEMPS { get; set; }
                public IEnumerable<ANCREG> ANCREGS { get; set; }
                public IEnumerable<APGARSCORE> APGARSCORES { get; set; }
                public IEnumerable<APPT> APPTS { get; set; }
                public IEnumerable<ASTNOTES> ASTNOTESS { get; set; }
                public IEnumerable<ATMPROFILE> ATMPROFILES { get; set; }
                public IEnumerable<ATTDFRM> ATTDFRMS { get; set; }
                public IEnumerable<BILL_ADJ> BILL_ADJS { get; set; }
                public IEnumerable<BILLAUX> BILLAUXS { get; set; }
                public IEnumerable<BILLCHAIN> BILLCHAINS { get; set; }
                public IEnumerable<BILLING> BILLINGS { get; set; }
                public IEnumerable<BILLSET> BILLSETS { get; set; }
                public IEnumerable<BILLVOUC> BILLVOUCS { get; set; }
                public IEnumerable<BIRTHS> BIRTHSS { get; set; }
                public IEnumerable<BSDETAIL> BSDETAILS { get; set; }
                public IEnumerable<CAPBILLCHAIN> CAPBILLCHAINS { get; set; }
                public IEnumerable<CAPBILLS> CAPBILLSS { get; set; }
                public IEnumerable<cdmdetails> cdmdetailsS { get; set; }
                public IEnumerable<CONSULTRMDETAILS> CONSULTRMDETAILSS { get; set; }
                public IEnumerable<CONSULTROOMS> CONSULTROOMSS { get; set; }
                public IEnumerable<CPRPTDEF> CPRPTDEFS { get; set; }
                public IEnumerable<CUSTCLASS> CUSTCLASSS { get; set; }
                public IEnumerable<CUSTOMER> CUSTOMERS { get; set; }
                public IEnumerable<DEATHS> DEATHSS { get; set; }
                public IEnumerable<DEPT> DEPTS { get; set; }
                public IEnumerable<DGPROFILE> DGPROFILES { get; set; }
                public IEnumerable<DISPENSA> DISPENSAS { get; set; }
                public IEnumerable<DISPSERV> DISPSERVS { get; set; }
                public IEnumerable<DOCTORS> DOCTORSS { get; set; }
                public IEnumerable<DUENEXT> DUENEXTS { get; set; }
                public IEnumerable<DUTYRTAB> DUTYRTABS { get; set; }
                public IEnumerable<ECG> ECGS { get; set; }
                public IEnumerable<FCCUSTOM> FCCUSTOMS { get; set; }
                public IEnumerable<FCPATIENT> FCPATIENTS { get; set; }
                public IEnumerable<FCSTKCHARG> FCSTKCHARGS { get; set; }
                public IEnumerable<FCTARIFF> FCTARIFFS { get; set; }
                public IEnumerable<FFSC01> FFSC01S { get; set; }
                public IEnumerable<FFSC01B> FFSC01BS { get; set; }
                public IEnumerable<FFSC02> FFSC02S { get; set; }
                public IEnumerable<FFSC03> FFSC03S { get; set; }
                public IEnumerable<FFSC04> FFSC04S { get; set; }
                public IEnumerable<FFSC05> FFSC05S { get; set; }
                public IEnumerable<FFSC06> FFSC06S { get; set; }
                public IEnumerable<ffsformprofiler> ffsformprofilerS { get; set; }
                public IEnumerable<GLINT> GLINTS { get; set; }
                public IEnumerable<GLINTAB1> GLINTAB1S { get; set; }
                public IEnumerable<GLINTAB2> GLINTAB2S { get; set; }
                public IEnumerable<GLINTAB3> GLINTAB3S { get; set; }
                public IEnumerable<GLINTAB4> GLINTAB4S { get; set; }
                public IEnumerable<GRPPROCEDURE> GRPPROCEDURES { get; set; }
                public IEnumerable<HMOAUTHORIZATIONS> HMOAUTHORIZATIONSS { get; set; }
                public IEnumerable<HMODETAIL> HMODETAILS { get; set; }
                public IEnumerable<HMOSERVIC> HMOSERVICS { get; set; }
                public IEnumerable<IBR001> IBR001S { get; set; }
                public IEnumerable<ICD10> ICD10S { get; set; }
                public IEnumerable<ICPC2> ICPC2S { get; set; }
                public IEnumerable<IMMI01> IMMI01S { get; set; }
                public IEnumerable<IMMI02> IMMI02S { get; set; }
                public IEnumerable<IMMI03> IMMI03S { get; set; }
                public IEnumerable<IMMI03M> IMMI03MS { get; set; }
                public IEnumerable<IMMI04> IMMI04S { get; set; }
                public IEnumerable<IMMI05> IMMI05S { get; set; }
                public IEnumerable<IMMI07M> IMMI07MS { get; set; }
                public IEnumerable<IMMUNTAB> IMMUNTABS { get; set; }
                public IEnumerable<IMUNES> IMUNESS { get; set; }
                public IEnumerable<INJCARD> INJCARDS { get; set; }
                public IEnumerable<INPDISPENSA> INPDISPENSAS { get; set; }
                public IEnumerable<INVLINK> INVLINKS { get; set; }
                public IEnumerable<LABDET> LABDETS { get; set; }
                public IEnumerable<LABTRANS> LABTRANSS { get; set; }
                public IEnumerable<LINK> LINKS { get; set; }
                public IEnumerable<LINK1> LINK1S { get; set; }
                public IEnumerable<LINK2> LINK2S { get; set; }
                public IEnumerable<LINK3> LINK3S { get; set; }
                public IEnumerable<LINK3HIST> LINK3HISTS { get; set; }
                public IEnumerable<LINKADM> LINKADMS { get; set; }
                public IEnumerable<MEDHIST> MEDHISTS { get; set; }
                public IEnumerable<MEDHISTCHAIN> MEDHISTCHAINS { get; set; }
                public IEnumerable<MEDHISTMULTICONSULT> MEDHISTMULTICONSULTS { get; set; }
                public IEnumerable<MEDHPIC> MEDHPICPROPS { get; set; }
                public IEnumerable<MEDHREC> MEDHRECS { get; set; }
                public IEnumerable<MORTBILLS> MORTBILLSS { get; set; }
                public IEnumerable<MORTDETAIL> MORTDETAILS { get; set; }
                public IEnumerable<MORTSPACE> MORTSPACES { get; set; }
                public IEnumerable<MRATTEND> MRATTENDS { get; set; }
                public IEnumerable<MRB15> MRB15S { get; set; }
                public IEnumerable<MRB15A> MRB15AS { get; set; }
                public IEnumerable<MRB15FC> MRB15FCS { get; set; }
                public IEnumerable<MRB17> MRB17S { get; set; }
                public IEnumerable<MRB19> MRB19S { get; set; }
                public IEnumerable<MRB20> MRB20S { get; set; }
                public IEnumerable<MRB21> MRB21S { get; set; }
                public IEnumerable<MRB21A> MRB21AS { get; set; }
                public IEnumerable<MRB22> MRB22S { get; set; }
                public IEnumerable<MRB23> MRB23S { get; set; }
                public IEnumerable<MRB24> MRB24S { get; set; }
                public IEnumerable<MRB25> MRB25S { get; set; }
                public IEnumerable<MRCONTROL> MRCONTROLS { get; set; }
                public IEnumerable<MRPT01> MRPT01S { get; set; }
                public IEnumerable<MRSETUP> MRSETUPS { get; set; }
                public IEnumerable<MRSTLEV> MRSTLEVS { get; set; }
                public IEnumerable<MSDUTYR> MSDUTYRS { get; set; }
                public IEnumerable<MSDUTYRGRP> MSDUTYRGRPS { get; set; }
                public IEnumerable<NNOTES> NNOTESS { get; set; }
                public IEnumerable<OPDCONSULT> OPDCONSULTS { get; set; }
                public IEnumerable<OVPROFILE> OVPROFILES { get; set; }
                public IEnumerable<PASSTAB> PASSTABS { get; set; }
                public IEnumerable<PATDETAIL> PATDETAILS { get; set; }
                public IEnumerable<PATIENT> PATIENTS { get; set; }
                public IEnumerable<PATPIC> PATPICSS { get; set; }
                public IEnumerable<PATREREG> PATREREGS { get; set; }
                public IEnumerable<PAYDETAIL> PAYDETAILS { get; set; }
                public IEnumerable<PDEVELOP> PDEVELOPS { get; set; }
                public IEnumerable<PGROWTH> PGROWTHS { get; set; }
                public IEnumerable<PHL01> PHL01S { get; set; }
                public IEnumerable<PMEDH01> PMEDH01S { get; set; }
                public IEnumerable<PMEDH02> PMEDH02S { get; set; }
                public IEnumerable<PMEDH04> PMEDH04S { get; set; }
                public IEnumerable<PMEDHDIAG> PMEDHDIAGS { get; set; }
                public IEnumerable<PP001> PP001S { get; set; }
                public IEnumerable<PP002> PP002S { get; set; }
                public IEnumerable<PP003> PP003S { get; set; }
                public IEnumerable<PP004> PP004S { get; set; }
                public IEnumerable<PP005> PP005S { get; set; }
                public IEnumerable<PPDETAIL> PPDETAILS { get; set; }
                public IEnumerable<PPDRESSING> PPDRESSINGS { get; set; }
                public IEnumerable<PPDRESSINGDTL> PPDRESSINGDTLS { get; set; }
                public IEnumerable<PROCPROFILE> PROCPROFILES { get; set; }
                public IEnumerable<PSPNOTES> PSPNOTESS { get; set; }
                public IEnumerable<PUBHOLIDAYS> PUBHOLIDAYSS { get; set; }
                public IEnumerable<ROUTDRGS> ROUTDRGSS { get; set; }
                public IEnumerable<RPTDEF> RPTDEFS { get; set; }
                public IEnumerable<SMSTEXTPROFILE> SMSTEXTPROFILES { get; set; }
                public IEnumerable<SPCDETAIL> SPCDETAILS { get; set; }
                public IEnumerable<SPCPROFILE> SPCPROFILES { get; set; }
                public IEnumerable<STKCHARG> STKCHARGS { get; set; }
                public IEnumerable<STKREQUESTS> STKREQUESTSS { get; set; }
                public IEnumerable<STKTRANS> STKTRANSS { get; set; }
                public IEnumerable<STOCKMAS> STOCKMASS { get; set; }
                public IEnumerable<SURGERYDETAILS> SURGERYDETAILSS { get; set; }
                public IEnumerable<SUSPENSE> SUSPENSES { get; set; }
                public IEnumerable<TARIFF> TARIFFS { get; set; }
                public IEnumerable<TEMPLATEGRP> TEMPLATEGRPS { get; set; }
                public IEnumerable<VSTATA> VSTATAS { get; set; }
                public IEnumerable<WORDLST1> WORDLST1S { get; set; }
                public IEnumerable<WORDLST2> WORDLST2S { get; set; }
                public IEnumerable<WORDLST3> WORDLST3S { get; set; }





            public HP_DATA.HP_DATAvm HP_DATAvm { get; set; }
            public APS01.APS01vm APS01vm { get; set; }
            public FAS01.FAS01vm FAS01vm { get; set; }
            public GLS01.GLS01vm GLS01vm { get; set; }
            public AR_DATA.AR_DATAvm AR_DATAvm { get; set; }
            public PAYPER.PAYPERvm PAYPERvm { get; set; }
            public SCS01.SCS01vm SCS01vm { get; set; }
            public SYSCODETABS.SYSCODETABSvm SYSCODETABSvm { get; set; }
        }










































        
    }
}