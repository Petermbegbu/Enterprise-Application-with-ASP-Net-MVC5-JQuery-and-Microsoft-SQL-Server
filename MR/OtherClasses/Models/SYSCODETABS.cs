using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

using OtherClasses;
using System.Data.SqlClient;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data;
using msfunc;
using GLS;
using SCS.DataAccess;
using HPL.BissClass;

namespace OtherClasses.Models
{
    public class SYSCODETABS
    {
        public class GDropDown
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public static List<CurrencyCodes> Currency()
            {
                var dropDownValues = ErpFunc.RsGet<CurrencyCodes>("SYSCODETABS",
                    "SELECT type_code, Name from currencyCodes order by name");
                return dropDownValues;
            }
            public static List<GDropDown> Periods()
            {
                var dropDownValues = new List<GDropDown>
            {
                new GDropDown {Id=0, Name="" }
            };
                return dropDownValues;
            }
            public static List<GDropDown> PaymentModes(int i = 0)
            {
                List<GDropDown> dropDownValues = new List<GDropDown>();
                if (i == 0)
                {
                    dropDownValues = new List<GDropDown>
                    {
                        new GDropDown {Id=0, Name="" },
                        new GDropDown {Id=1, Name="C\\ CASH" },
                        new GDropDown {Id=2, Name="Q\\ CHEQUE" },
                        new GDropDown {Id=3, Name="B\\ BANK TELLER" },
                        new GDropDown {Id=4, Name="R\\ CREDIT CARD/ATM" },
                        new GDropDown {Id=5, Name="D\\ DIRECT DEBIT" }
                    };
                }
                else if (i == 1)
                {
                    dropDownValues = new List<GDropDown>
                    {
                        new GDropDown {Id=0, Name="" },
                        new GDropDown {Id=1, Name="CASH" },
                        new GDropDown {Id=2, Name="R\\CREDIT CARD\\POS" },
                        new GDropDown {Id=3, Name="Q\\ CHEQUE" },
                        new GDropDown {Id=4, Name="BANK TELLER" },
                        new GDropDown {Id=5, Name="MULTIPLE-PAY-OPTION" }
                    };
                }
                return dropDownValues;
            }
            public static List<GDropDown> PaymentTypes()
            {
                var dropDownValues = new List<GDropDown>
                {
                    new GDropDown {Id=0, Name="" },
                    new GDropDown {Id=1, Name="C\\ DOWN CHARGE" },
                    new GDropDown {Id=2, Name="D\\ INITIAL DEPOSIT" },
                    new GDropDown {Id=3, Name="I\\ ON INSTALLMENTS" },
                    new GDropDown {Id=4, Name="A\\ ON ACCOUNT ADD TO DEPOSIT"}
                };
                return dropDownValues;
            }
            public static string PtName(int Id)
            {
                string name = "";
                var aa = GDropDown.PaymentTypes();
                foreach(var a in aa)
                {
                    if (a.Id == Id) { name = a.Name; continue; }
                }
                return name;
            }
            public static List<GDropDown> TransType()
            {
                List<GDropDown> dropDownValues = new List<GDropDown>();
                
                dropDownValues = new List<GDropDown>
                {
                    new GDropDown {Id=0, Name="" },
                    new GDropDown {Id=1, Name="A-CASH SALES INVOICE" },
                    new GDropDown {Id=2, Name="R-CREDIT INVOICE" },
                    new GDropDown {Id=3, Name="M-SAMPLE INVOICE" },
                    new GDropDown {Id=4, Name="P-PROFFESSIONAL INVOICE" },
                    new GDropDown {Id=5, Name="V-REVERSAL" },
                    new GDropDown {Id=6, Name="O-LOYALTY REWARD" }
                };
                
                return dropDownValues;
            }
        }
        public class ERPmiscl
        {
            public string FillUpTable { get; set; }
            public string FillUpTable2 { get; set; }
            public string FillUpControls { get; set; }
            public bool NewRec { get; set; }
            public string exMessage { get; set; }
            public string exType { get; set; }
            public string iinvalid { get; set; }
            public decimal retDecimal { get; set; }
            public decimal retDecimal2 { get; set; }
            public bool retBool { get; set; }
            public int retInt { get; set; }
            public string retString { get; set; }
            public DateTime retDate { get; set; }
            public bool PSReversePMT { get; set; }
            public bool mispos_active { get; set; }
            public bool POSReceipt4Wholesale { get; set; }
            public DateTime dtmin_date { get; set; }
            public bool chkPrint { get; set; }
            public string [,] dgv { get; set; }
            public string[,] dgv2 { get; set; }
            public string objTrans { get; set; }
        }

        public class AdjustmentCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





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

            public string PURPOSE { get; set; }

            public string USERID_PHONE { get; set; }

            public string USERID_EMAIL { get; set; }

            public string BENEFICIARY { get; set; }

            public string BENEFICIARY_PHONE { get; set; }

            public string BENEFICIARY_EMAIL { get; set; }





            #endregion Instance Properties
}
        public class ARCHIVEDSMS
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string SENDER { get; set; }

            public string RECEIVER { get; set; }

            public DateTime? DTSEND { get; set; }

            public string DETAILS { get; set; }

            public int? SECURITYLEVEL { get; set; }





            #endregion Instance Properties
}
        public class AssetCategory
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class AssetlocationCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class BinLocationCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class BranchCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class ClassificationCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class CostCentreCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class CountryCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class ctrolxl
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string station { get; set; }

            public string country { get; set; }

            public string name { get; set; }

            public bool? installed { get; set; }

            public bool? allowexp { get; set; }

            public bool? gsort { get; set; }

            public string state { get; set; }

            public string user_name { get; set; }

            public bool? ta_post { get; set; }

            public DateTime? last_date { get; set; }

            public bool? tp_start { get; set; }

            public bool? tp_ended { get; set; }

            public DateTime? tp_date { get; set; }

            public decimal? tp_period { get; set; }

            public decimal? pyear { get; set; }

            public bool? tr_start { get; set; }

            public bool? tr_ended { get; set; }

            public DateTime? tr_date { get; set; }

            public bool? p_start { get; set; }

            public bool? p_ended { get; set; }

            public DateTime? p_date { get; set; }

            public bool? cd_start { get; set; }

            public bool? cd_ended { get; set; }

            public decimal? posting { get; set; }

            public decimal? delrec { get; set; }

            public string mpass { get; set; }

            public DateTime? mpassdate { get; set; }

            public bool? fsh { get; set; }

            public int? enqno { get; set; }

            public string serial { get; set; }

            public bool? production { get; set; }

            public string ds { get; set; }

            public string appver { get; set; }

            public string RVERSION { get; set; }

            public string BCC { get; set; }

            public bool? reload { get; set; }

            public string email { get; set; }

            public string phone { get; set; }

            public bool? filemode { get; set; }

            public int? REC_COUNT { get; set; }

            public string localcur { get; set; }





            #endregion Instance Properties
}
        public class CurrencyCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class CURTABLE
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string ccode { get; set; }

            public string curmain { get; set; }

            public string curunit { get; set; }

            public string cursign { get; set; }

            public decimal? rate { get; set; }

            public DateTime? trans_date { get; set; }





            #endregion Instance Properties
}
        public class DeductionCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class DesignationCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class DiagnosisCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class EarningCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class Ftdtctr
        {


            #region Instance Properties

            public int RECID { get; set; }

            public DateTime? lastdate { get; set; }

            public bool? smssys { get; set; }

            public bool? arsys { get; set; }

            public bool? apsys { get; set; }

            public bool? fasys { get; set; }

            public bool? glsys { get; set; }

            public bool? mrsys { get; set; }

            public bool? hrsys { get; set; }

            public decimal? torrec { get; set; }

            public DateTime? tfdate { get; set; }

            public string tfuser { get; set; }

            public string branch { get; set; }

            public DateTime? startdate { get; set; }





            #endregion Instance Properties
}
        public class globalsys
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string OPERATOR { get; set; }

            public string password { get; set; }

            public bool? medical { get; set; }

            public bool? stock { get; set; }

            public bool? ap { get; set; }

            public bool? ar { get; set; }

            public bool? fa { get; set; }

            public bool? mspp { get; set; }

            public bool? gl { get; set; }

            public bool? enterprise { get; set; }

            public bool? lease { get; set; }

            public bool? hom { get; set; }

            public DateTime? PASSDATE { get; set; }

            public bool? createusers { get; set; }

            public string username { get; set; }

            public DateTime? dttime { get; set; }

            public bool? singledatabase { get; set; }

            public string branch { get; set; }

            public string department { get; set; }

            public decimal? wseclevel { get; set; }





            #endregion Instance Properties
}
        public class GReasonsCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class HrAssetCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class HrReasonsCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class les01
        {

            #region Instance Properties

            #endregion Instance Properties
}
        public class LeaveCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class LoanCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class MNALOGS
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string NAME { get; set; }

            public string SENDER { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string NOTES { get; set; }

            public string RECEIVED { get; set; }

            public string SENDSDEPT { get; set; }

            public string SENDSBRANCH { get; set; }

            public bool? POSTED { get; set; }

            public string TODEPT { get; set; }

            public string TOBRANCH { get; set; }

            public decimal? REPLAYAT { get; set; }

            public decimal? REPLAYDAYS { get; set; }

            public bool? resultalert { get; set; }





            #endregion Instance Properties
}
        public class mrPatDischReasons
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string TYPE_CODE { get; set; }

            public string NAME { get; set; }

            public string FLDATTRIB { get; set; }





            #endregion Instance Properties
}
        public class msinpset
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string INSTDRIVE { get; set; }

            public string dpath { get; set; }

            public bool? medical { get; set; }

            public bool? mf2 { get; set; }

            public bool? mf3 { get; set; }

            public bool? mf4 { get; set; }

            public bool? gl { get; set; }

            public bool? stock { get; set; }

            public bool? payper { get; set; }

            public bool? fa { get; set; }

            public bool? ar { get; set; }

            public bool? ap { get; set; }

            public bool? mis { get; set; }

            public bool? icis { get; set; }

            public string ddirectory { get; set; }

            public bool? sh_call { get; set; }

            public string appath { get; set; }

            public bool? mf1 { get; set; }

            public bool? isd { get; set; }

            public bool? hom { get; set; }

            public string appver { get; set; }

            public bool? sma { get; set; }

            public bool? cps { get; set; }

            public bool? meddiag { get; set; }

            public bool? phpos { get; set; }

            public bool? leasesys { get; set; }

            public bool? enterprise { get; set; }





            #endregion Instance Properties
}
        public class NonTaxableCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class PayPointCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class ProductCategoryCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }

            public string codeprefix { get; set; }

            public decimal? codecounter { get; set; }





            #endregion Instance Properties
}
        public class QualificationCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class REQUESTAUTHORIZATIONS
        {


            #region Instance Properties

            public int? RECID { get; set; }

            public string REFERENCE { get; set; }

            public DateTime? TRANS_DATE { get; set; }

            public string REQUESTFROM { get; set; }

            public string SENTTO { get; set; }

            public string DETAILS { get; set; }

            public string AUTHORIZEDBY { get; set; }

            public string DATETIME_AUTHORIZED { get; set; }

            public bool? REQUEST_GRANTED { get; set; }

            public string REQUEST_LIFESPAN { get; set; }





            #endregion Instance Properties
}
        public class SalesZoneCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class ServiceCentreCodes
        {
            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class StateCodes
        {
            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }
}
        public class StatusCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class StkCategoryCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }

            public string codeprefix { get; set; }

            public decimal? counter { get; set; }





            #endregion Instance Properties
}

        public class StkGenericCodes
        {



            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
        }
        public class StkUnitofMeasure
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}

        public class StkUsageCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}

        public class tables
        {


            #region Instance Properties

            public string type { get; set; }

            public string code { get; set; }

            public string type_code { get; set; }

            public string lookup { get; set; }

            public string typedesc { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        public class TradeZoneCodes
        {


            #region Instance Properties

            public int RECID { get; set; }

            public string type_code { get; set; }

            public string name { get; set; }

            public string fldattrib { get; set; }





            #endregion Instance Properties
}
        

        public class SYSCODETABSvm
        {
            [NotMapped]
            public ERPmiscl ERPmiscl { get; set; }
            public GDropDown GPaymentType { get; set; }
            public GDropDown GPeriod { get; set; }
            public GDropDown GPaymentMode { get; set; }
            public GDropDown GTransType { get; set; }

            public AdjustmentCodes AdjustmentCodes { get; set; }
            public APPT APPT { get; set; }
            public ARCHIVEDSMS ARCHIVEDSMS { get; set; }
            public AssetCategory AssetCategory { get; set; }
            public AssetlocationCodes AssetlocationCodes { get; set; }
            public BinLocationCodes BinLocationCodes { get; set; }
            public BranchCodes BranchCodes { get; set; }
            public ClassificationCodes ClassificationCodes { get; set; }
            public CostCentreCodes CostCentreCodes { get; set; }
            public CountryCodes CountryCodes { get; set; }
            public ctrolxl ctrolxl { get; set; }
            public CurrencyCodes CurrencyCodes { get; set; }
            public CURTABLE CURTABLE { get; set; }
            public DeductionCodes DeductionCodes { get; set; }
            public DesignationCodes DesignationCodes { get; set; }
            public DiagnosisCodes DiagnosisCodes { get; set; }
            public EarningCodes EarningCodes { get; set; }
            public Ftdtctr Ftdtctr { get; set; }
            public globalsys globalsys { get; set; }
            public GReasonsCodes GReasonsCodes { get; set; }
            public HrAssetCodes HrAssetCodes { get; set; }
            public HrReasonsCodes HrReasonsCodes { get; set; }
            public les01 les01 { get; set; }
            public LeaveCodes LeaveCodes { get; set; }
            public LoanCodes LoanCodes { get; set; }
            public MNALOGS MNALOGS { get; set; }
            public mrPatDischReasons mrPatDischReasons { get; set; }
            public msinpset msinpset { get; set; }
            public NonTaxableCodes NonTaxableCodes { get; set; }
            public PayPointCodes PayPointCodes { get; set; }
            public ProductCategoryCodes ProductCategoryCodes { get; set; }
            public QualificationCodes QualificationCodes { get; set; }
            public REQUESTAUTHORIZATIONS REQUESTAUTHORIZATIONS { get; set; }
            public SalesZoneCodes SalesZoneCodes { get; set; }
            public ServiceCentreCodes ServiceCentreCodes { get; set; }
            public StateCodes StateCodes { get; set; }
            public StkCategoryCodes StkCategoryCodes { get; set; }
            public StkGenericCodes StkGenericCodes { get; set; }
            public StkUnitofMeasure StkUnitofMeasure { get; set; }
            public StkUsageCodes StkUsageCodes { get; set; }
            public tables tables { get; set; }
            public TradeZoneCodes TradeZoneCodes { get; set; }


            public IEnumerable<GDropDown> GPaymentTypes { get; set; }
            public IEnumerable<GDropDown> GPeriods { get; set; }
            public IEnumerable<GDropDown> GPaymentModes { get; set; }
            public IEnumerable<GDropDown> GTransTypes { get; set; }

            public IEnumerable<AdjustmentCodes> AdjustmentCodess { get; set; }
            public IEnumerable<APPT> APPTs { get; set; }
            public IEnumerable<ARCHIVEDSMS> ARCHIVEDSMSs { get; set; }
            public IEnumerable<AssetCategory> AssetCategorys { get; set; }
            public IEnumerable<AssetlocationCodes> AssetlocationCodess { get; set; }
            public IEnumerable<BinLocationCodes> BinLocationCodess { get; set; }
            public IEnumerable<BranchCodes> BranchCodess { get; set; }
            public IEnumerable<ClassificationCodes> ClassificationCodess { get; set; }
            public IEnumerable<CostCentreCodes> CostCentreCodess { get; set; }
            public IEnumerable<CountryCodes> CountryCodess { get; set; }
            public IEnumerable<ctrolxl> ctrolxls { get; set; }
            public IEnumerable<CurrencyCodes> CurrencyCodess { get; set; }
            public IEnumerable<CURTABLE> CURTABLEs { get; set; }
            public IEnumerable<DeductionCodes> DeductionCodess { get; set; }
            public IEnumerable<DesignationCodes> DesignationCodess { get; set; }
            public IEnumerable<DiagnosisCodes> DiagnosisCodess { get; set; }
            public IEnumerable<EarningCodes> EarningCodess { get; set; }
            public IEnumerable<Ftdtctr> Ftdtctrs { get; set; }
            public IEnumerable<globalsys> globalsyss { get; set; }
            public IEnumerable<GReasonsCodes> GReasonsCodess { get; set; }
            public IEnumerable<HrAssetCodes> HrAssetCodess { get; set; }
            public IEnumerable<HrReasonsCodes> HrReasonsCodess { get; set; }
            public IEnumerable<les01> les01s { get; set; }
            public IEnumerable<LeaveCodes> LeaveCodess { get; set; }
            public IEnumerable<LoanCodes> LoanCodess { get; set; }
            public IEnumerable<MNALOGS> MNALOGSs { get; set; }
            public IEnumerable<mrPatDischReasons> mrPatDischReasonss { get; set; }
            public IEnumerable<msinpset> msinpsets { get; set; }
            public IEnumerable<NonTaxableCodes> NonTaxableCodess { get; set; }
            public IEnumerable<PayPointCodes> PayPointCodess { get; set; }
            public IEnumerable<ProductCategoryCodes> ProductCategoryCodess { get; set; }
            public IEnumerable<QualificationCodes> QualificationCodess { get; set; }
            public IEnumerable<REQUESTAUTHORIZATIONS> REQUESTAUTHORIZATIONSs { get; set; }
            public IEnumerable<SalesZoneCodes> SalesZoneCodess { get; set; }
            public IEnumerable<ServiceCentreCodes> ServiceCentreCodess { get; set; }
            public IEnumerable<StateCodes> StateCodess { get; set; }
            public IEnumerable<StkCategoryCodes> StkCategoryCodess { get; set; }
            public IEnumerable<StkGenericCodes> StkGenericCodess { get; set; }
            public IEnumerable<StkUnitofMeasure> StkUnitofMeasures { get; set; }
            public IEnumerable<StkUsageCodes> StkUsageCodess { get; set; }
            public IEnumerable<tables> tabless { get; set; }
            public IEnumerable<TradeZoneCodes> TradeZoneCodess { get; set; }
        }
        
    }

}
