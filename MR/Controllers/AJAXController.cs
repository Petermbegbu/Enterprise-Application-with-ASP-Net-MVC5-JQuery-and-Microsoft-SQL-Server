using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using OtherClasses.Models;
using OtherClasses;
using System.Data;
using System.Data.SqlClient;
using msfunc;
using HPL.BissClass;
using OtherClasses.FILE;
using mradmin.DataAccess;
using mradmin.BissClass;
using Gizmox.WebGUI.Forms;
using msfunc.Forms;
using MSMR.Forms;
using SCS.DataAccess;
using mradmin.Forms;

namespace OtherClasses.FILE
{
    //[ValidateAntiForgeryToken]
    public class AJAXController : Controller
    {
        MR_DATA.MR_DATAvm vm = new MR_DATA.MR_DATAvm();

        // GET: AJAX
        string br_cc = bissclass.sysGlobals.mbr_cc;

        //Ante_Natal Records Start
        #region 

        public JsonResult prvPregSubmitBtnClick(IEnumerable<MR_DATA.ANC03> tableList, MR_DATA.REPORTS dataList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            //string woperato = Request.Cookies["mrName"].Value;
            //vm.REPORTS = dataList;

            PreviousPregnancies formObject = new PreviousPregnancies(vm);
            vm.REPORTS = formObject.savedetails(tableList, dataList);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult prvPregDeleteBtnClick(string pg1PatientNo, string pg1GroupCode, string mthOfBirth)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (ANC03.DeleteANC03(pg1GroupCode, pg1PatientNo, mthOfBirth))
            {
                vm.REPORTS.alertMessage = "Deleted...";
            }
            else
            {
                vm.REPORTS.alertMessage = "Record was not Deleted...";
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult prevPregOnLoad(string pg1PatientNo, string pg1GroupCode)
        {           
            vm.ANC03S = ErpFunc.RsGet<MR_DATA.ANC03>("MR_DATA", 
                "SELECT * FROM ANC03 WHERE GROUPCODE = " + pg1GroupCode + " and PATIENTNO = " + pg1PatientNo + "");

            return Json(vm.ANC03, JsonRequestBehavior.AllowGet);
        }

        public JsonResult pg1HbPatientNoFocusout(string pg1HbPatientNo, string pg1HbGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (string.IsNullOrWhiteSpace(pg1HbPatientNo))  //no lookup value obtained
            {
                vm.REPORTS.txtpatientno = bissclass.autonumconfig(pg1HbPatientNo, true, "", "9999999"); //pg1_txtpatientnoHB.Text
            }

            //check if patientno exists
            billchaindtl bchainHB = billchaindtl.Getbillchain(pg1HbPatientNo, pg1HbGroupCode);

            if (bchainHB == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";
                //pg1_txtpatientnoHB.Text = " ";
                //pg1_txtgroupcodeHB.Select();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.REPORTS.TXTPATIENTNAME = bchainHB.NAME; //pg1_txtHusbandName.Text
                vm.REPORTS.txtemail = bchainHB.PHONE.Trim() + " : " + bchainHB.EMAIL.Trim(); //txthusbankphoneEmail.Text
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult pg1ANCreferenceFocusout(string pg1ConsultRef, string pg1ANCreference, string pg1DOB)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            ANC01 anc01 = new ANC01();
            billchaindtl billchain = new billchaindtl();

            anc01 = ANC01.GetANC01(pg1ANCreference);

            if (anc01 == null)
            {
                vm.REPORTS.alertMessage = "Invalid ANC Registration Number... ";
                //pg1_txtANCReference.Focus();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            billchain = billchaindtl.Getbillchain(anc01.PATIENTNO, anc01.GROUPCODE);
            if (billchain == null)
            {
                vm.REPORTS.alertMessage = "Error Reading Patients Details in Master File... \r\n Pls Check Patients Registration Details!";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            displayANCPages(anc01, billchain, pg1ConsultRef, pg1ANCreference, pg1DOB);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult pg1LMPFocusout(string pg1LMP)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DateTime lmp = Convert.ToDateTime(pg1LMP);

            if (lmp.Date > DateTime.Now.Date)
            {
                vm.REPORTS.alertMessage = "Invalid LMP Date...";
                //dtLMPpg1.Select();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            decimal xdays = Convert.ToDecimal(DateTime.Now.Date.Subtract(lmp.Date).TotalDays);
            decimal xega = xdays / 7;
            DateTime edd = lmp.Date.AddDays(281); //dtEDDpg1.Value
            vm.REPORTS.REPORT_TYPE1 = edd.ToString();

            vm.REPORTS.txtGestationAge = Math.Round(xega, 0).ToString() + " Wk(s)"; //pg1_txtGestationPeriod.Text

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void displaySpecialInstructions(billchaindtl billchain)
        {
            //retrieve special medical notes - allergies etc.
            //edtallergies.Text = edtspinstructions.Text = txtHmoNhisPlanType.Text = txtHMOPlanType.Text =
            //    txtHMOPlanType.Text = "";
            vm.REPORTS.SessionMhead = "";
            // PSPNOTES pspnotes = PSPNOTES.GetPSPNOTES(anc01.GROUPCODE,anc01.PATIENTNO);
            //  if ( pspnotes != null)
            //  {

            vm.REPORTS.edtallergies = billchain.MEDNOTES.Trim(); //edtallergies.Text
            vm.REPORTS.edtspinstructions = billchain.SPNOTES.Trim(); //edtspinstructions.Text

            //  }
            if (billchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(billchain.HMOSERVTYPE))
            {
                vm.REPORTS.diagnosis = billchain.HMOSERVTYPE; //txtHMOPlanType.Text
                DataTable HMODETAILS = Hmodetail.GetHMODETAIL(billchain.GROUPHEAD);

                for (int i = 0; i < HMODETAILS.Rows.Count; i++)
                {
                    if (HMODETAILS.Rows[i]["HMOSERVTYPE"].ToString().Trim() == billchain.HMOSERVTYPE.Trim())
                    {
                        vm.REPORTS.diagnosis = HMODETAILS.Rows[i]["HMOSERVTYPE"].ToString().Trim(); //txtHmoNhisPlanType.Text
                        break;
                    }
                }
            }
        }

        void displayPrevMedHistory(bool xcurrent, ANC01 anc01, billchaindtl billchain) //page 10
        {
          
            //DataTable dt = MedHist.GetHISTByPatient(anc01.GROUPCODE,anc01.PATIENTNO,"D", anc01.LASTATTEND.Date,  DateTime.Now.Date, true);
            string medrecs = GetMEDHISTCaseNotes(anc01.GROUPCODE, anc01.PATIENTNO, xcurrent ? false : true, 
                xcurrent ? true : false, anc01.LASTATTEND.Date, DateTime.Now.Date, billchain, "DESC");
            
            vm.REPORTS.txtMedHistory = medrecs; // PrevMedHistoryNotes.Text + medhist.COMMENTS.Trim();
                                               
        }

        void displaydetailsPage8(ANC01 anc01)
        {
            ANC07A anc07a = new ANC07A();

            //recno = 0;
            anc07a = ANC07A.GetANC07A(anc01.REFERENCE);

            if (anc07a == null)
            {
                vm.REPORTS.chkExcludeRequests = true; //anc07anewRecord
                return;
            }

            vm.REPORTS.chkExcludeRequests = false; //anc07anewRecord

            vm.REPORTS.txtGestage = anc07a.GESTAGE; //Pg8_TxtGesAge.Text
            vm.REPORTS.txtParity2 = anc07a.PARITY; //Pg8_TxtParity.Text
            DateTime transDate = anc07a.TRANS_DATE; //Pg8_trans_date.Value
            vm.REPORTS.REPORT_TYPE2 = string.Format("{0:yyyy-MM-dd}", transDate);
            vm.REPORTS.txtProcess = anc07a.PROCESS; //Pg8_txtProcess.Text
            vm.REPORTS.txtIndications = anc07a.INDICATIONS; //Pg8_txtIndications.Text
            vm.REPORTS.txtStaffPresent = anc07a.STAFFPRESENT; //Pg8_txtStaffPresent.Text
            vm.REPORTS.txtSurgeon = anc07a.SURGEON; //Pg8_TxtSurgeon.Text
            vm.REPORTS.txtAssistant = anc07a.ASSISTANT; //Pg8_txtAssistant.Text
            vm.REPORTS.txtPaediatricians = anc07a.PAEDIATRICIAN; //Pg8_txtPaediatricians.Text
            vm.REPORTS.txtMidwives = anc07a.MIDWIVES; //Pg8_txtMidwives.Text
            vm.REPORTS.txtAnaesthetist = anc07a.ANAESTHETIST; //Pg8_txtAnaesthetist.Text
            vm.REPORTS.txtOthers = anc07a.OTHERS; //Pg8_txtOthers.Text
            vm.REPORTS.txtAnaethesia = anc07a.ANAESTHESIA; //Pg8_txtAnaethesia.Text
            vm.REPORTS.txtFindings = anc07a.FINDINGS; //Pg8_txtFindings.Text
            vm.REPORTS.txtProcedure = anc07a.PROCEDURENOTE; //Pg8_txtProcedure.Text
            vm.REPORTS.txtMother = anc07a.MOTHER; //Pg8_txtMother.Text
            vm.REPORTS.txtBaby = anc07a.BABY; //Pg8_txtBaby.Text
            vm.REPORTS.txtStaffSign = anc07a.STAFFSIGN; //Pg8_txtStaffSign.Text

        }

        void displaydetailsPage7(ANC01 anc01)
        {
            ANC07 anc07 = new ANC07();

            //recno = 0;

            anc07 = ANC07.GetANC07(anc01.REFERENCE);
            if (anc07 == null)
            {
                vm.REPORTS.chkCummulativeSumm = true; //anc07newRecord
                return;
            }
            vm.REPORTS.chkCummulativeSumm = false; //anc07newRecord

            //anc07.LABDURATION;
            vm.REPORTS.chkNonIdentified = anc07.TRAUMA_NONE ? true : false; //Pg7_ChkNonIdentified.Checked
            vm.REPORTS.chkPerinealTear = anc07.PERINEAL_TEAR ? true : false; //Pg7_ChkPerinealTear.Checked
            vm.REPORTS.chkOptFirstDeg = anc07.TEARDEGREE == 1 ? true : false; //Pg7_optFirstDeg.Checked
            vm.REPORTS.chkOptSecondDeg = anc07.TEARDEGREE == 2 ? true : false; //Pg7_optSecondDeg.Checked
            vm.REPORTS.chkOptThirdDeg = anc07.TEARDEGREE == 3 ? true : false; //Pg7_optThirdDeg.Checked
            vm.REPORTS.chkOptFourthDeg = anc07.TEARDEGREE == 4 ? true : false; //Pg7_optFourthDeg.Checked
            vm.REPORTS.chkEpisotomy = anc07.EPISIOTOMY ? true : false; //Pg7_chkEpisotomy.Checked
            vm.REPORTS.txtindforepisotomy = anc07.INDI4EPISIOTOMY; //Pg7_txtindicationforepisotomy.Text
            vm.REPORTS.chkOptRepairYes = anc07.REPREQ == 1 ? true : false; //Pg7_OptRepairYes.Checked
            vm.REPORTS.chkOptRepairNo = anc07.REPREQ == 2 ? true : false; //Pg7_optRepairNo.Checked 
            vm.REPORTS.chkOptMothRYes = anc07.MOTHERAGREE == 1 ? true : false; //Pg7_optMothRYes.Checked
            vm.REPORTS.chkOptMotherRNo = anc07.MOTHERAGREE == 2 ? true : false; //Pg7_optMotherRNo.Checked
            vm.REPORTS.txtAnaestheticUsed = anc07.ANAESTHUSED; //Pg7_txtAnaestheticUsed.Text
            vm.REPORTS.txtSignature = anc07.STAFF; //Pg7_txtSignature.Text
            DateTime trdttime = anc07.TRDTTIME.Date; //Pg7_AnaextheticDate.Value
            vm.REPORTS.txtAnaextheticDate = string.Format("{0:yyyy-MM-dd}", trdttime);
            vm.REPORTS.txtAnaestheticTime = anc07.TRDTTIME.ToString("HH:mm:ss"); //Pg7_AnaestheticTime.Text
            vm.REPORTS.txtBaby1delivered = anc07.BAB1BY; //Pg7_TxtBaby1delivered.Text
            vm.REPORTS.txtBaby2delivered = anc07.BAB2BY; //Pg7_TxtBaby2delivered.Text
            vm.REPORTS.txtBaby3delivered = anc07.BAB3BY; //Pg7_TxtBaby3delivered.Text
            vm.REPORTS.txtcommentsActions = anc07.COMMENTS; //Pg7_commentsActions.Text

            //the datagrid values
            vm.ANC07BS = ErpFunc.RsGet<MR_DATA.ANC07B>("MR_DATA",
                "select * from ANC07B where reference = '" + anc01.REFERENCE + "'");

        }

        void displaydetailsPage6(ANC01 anc01)
        {
            //recno = 0;
            ANC07 anc07 = new ANC07();

            anc07 = ANC07.GetANC07(anc01.REFERENCE);

            if (anc07 == null)
            {
                vm.REPORTS.chkADVunpaidmiscbills = true; //anc07newRecord
                return;
            }

            vm.REPORTS.chkADVunpaidmiscbills = false; //anc07newRecord

            vm.REPORTS.txtConsultant = anc07.CONSOBS; //pg6_txtConsultantOb.Text
            vm.REPORTS.txtAttendingPaed = anc07.ATTENDPAED; //pg6_txtAttendingPaediatrician.Text
            DateTime dtpDeliverydate = anc07.DELDATE; //pg6_dtpDeliverydate.Value 
            vm.REPORTS.txtDeliverydate = string.Format("{0:yyyy-MM-dd}", dtpDeliverydate);

            vm.REPORTS.txtDeliverySuite = anc07.DELSUITE; //pg6_txtDeliverySuite.Text
            vm.REPORTS.txtDeliveredTime = anc07.DELTIME; //pg6_txtBabyDeliveredTIme.Text
            vm.REPORTS.txtParity = anc07.PARITY; //pg6_txtParity.Text
            vm.REPORTS.txtFetalNumber = anc07.FETALNUMBER; //pg6_txtFetalNumber.Text
            vm.REPORTS.txtGestationAge = anc07.GESTAGE; //pg6_txtGestationAge.Text
            vm.REPORTS.chkNoneLaborOnset = anc07.LO_NONE ? true : false; //pg6_chkNoneLaborOnset.Checked
            vm.REPORTS.chkSpontaneous = anc07.LO_SPONTANEOUS ? true : false; //pg6_chkSpontaneous.Checked
            vm.REPORTS.chkInduced = anc07.LO_INDUCED ? true : false; //pg6_ChkInduced.Checked
            vm.REPORTS.chkAugumented = anc07.LO_AUGUMENTD ? true : false; //pg6_ChkAugumented.Checked
            vm.REPORTS.txtIndicationLabor = anc07.INDICANTIONS; //pg6_TxtIndicationLaborOnset.Text
            vm.REPORTS.chkArtificial = anc07.ROM_ACTIFICIAL; //pg6_chkArtificial.Checked
            vm.REPORTS.chkMemInduced = anc07.ROM_INDUCED ? true : false; //pg6_chkMemInduced.Checked
            vm.REPORTS.txtMemRuptIndication = anc07.ROM_INDICATIONS; //pg6_TxtMemRuptIndication.Text
            DateTime dtruptureDate = anc07.ROMDATE; //pg6_dtruptureDate.Value
            vm.REPORTS.txtRuptureDate = string.Format("{0:yyyy-MM-dd}", dtruptureDate);
            vm.REPORTS.txtRuptMemDuration = anc07.ROM_DURATION; //pg6_txtRuptMemDuration.Text
            vm.REPORTS.txtdtruptureTime = anc07.ROMTIME; //pg6_txtdtruptureTime.Text
            vm.REPORTS.chkPRNone = anc07.PR_NONE ? true : false; //pg6_chkPRNone.Checked
            vm.REPORTS.chkPRNarcotics = anc07.PR_NARCOTICS ? true : false; //pg6_chkPRNarcotics.Checked
            vm.REPORTS.chkPRPurdendal = anc07.PR_PRUDENDAL ? true : false; //pg6_chkPRPurdendal.Checked
            vm.REPORTS.chkPREntonox = anc07.PR_ENTONOX ? true : false; //pg6_chkPREntonox.Checked
            vm.REPORTS.chkPREpidural = anc07.PR_EPIDURAL ? true : false; //pg6_chkPREpidural.Checked
            vm.REPORTS.chkPRSpinal = anc07.PR_SPINAL ? true : false; //pg6_chkPRSpinal.Checked
            vm.REPORTS.chkPRCombinedES = anc07.PR_COMBINED ? true : false; //pg6_chkPRCombinedES.Checked
            vm.REPORTS.chktstActive = anc07.TSTAGEM_A ? true : false; //pg6_chktstActive.Checked
            vm.REPORTS.chktstmanualremoval = anc07.TSTAGEM_M ? true : false; //pg6_chktstmanualremoval.Checked
            vm.REPORTS.txttstComments = anc07.TSTAGEMGTNOTES; //pg6_TxttstCommentsIndications.Text

            DateTime dtpOnset = anc07.LABONSETDT; //pg6_dtpOnset.Value
            vm.REPORTS.txtOnsetDate = string.Format("{0:yyyy-MM-dd}", dtpOnset);
            DateTime dtpFullyDilated = anc07.LABFDDT; //pg6_dtpFullyDilated.Value
            vm.REPORTS.txtFullyDilatedDate = string.Format("{0:yyyy-MM-dd}", dtpFullyDilated);
            DateTime dtpPushingCommenced = anc07.LABPCDT; //pg6_dtpPushingCommenced.Value
            vm.REPORTS.txtPushingComDate = string.Format("{0:yyyy-MM-dd}", dtpPushingCommenced);
            DateTime dtpHeadDelivered = anc07.LABHDDT; //pg6_dtpHeadDelivered.Value
            vm.REPORTS.txtHeadDelDate = string.Format("{0:yyyy-MM-dd}", dtpHeadDelivered);
            DateTime dtpBabyDelivered = anc07.LABBDDT; //pg6_dtpBabyDelivered.Value
            vm.REPORTS.txtBabyDelDate = string.Format("{0:yyyy-MM-dd}", dtpBabyDelivered);
            DateTime dtpEndofThirdStage = anc07.LABEOTSDT; //pg6_dtpEndofThirdStage.Value
            vm.REPORTS.txtEndof3StageDate = string.Format("{0:yyyy-MM-dd}", dtpEndofThirdStage);
            DateTime dtpTwinDel = anc07.LABT2DDT; //pg6_dtpTwinDel.Value
            vm.REPORTS.txtTwinDelDate = string.Format("{0:yyyy-MM-dd}", dtpTwinDel);

            vm.REPORTS.txtOnsetTime = anc07.LABPNSETTIME; //pg6_txtOnsetTime.Text
            vm.REPORTS.txtFullDilatedTime = anc07.LABFDTIME; //pg6_txtFullDilatedTime.Text
            vm.REPORTS.txtPushingCommencedTIme = anc07.LABPCTIME; //pg6_txtPushingCommencedTIme.Text
            vm.REPORTS.txtHeadDeliveredTIme = anc07.LABHDTIME; //pg6_txtHeadDeliveredTIme.Text
            vm.REPORTS.txtBabyDeliveredTIme = anc07.LABBDTIME; //pg6_txtBabyDeliveredTIme.Text
            vm.REPORTS.txtEndofThirdStageTime = anc07.LABEOTSTIME; //pg6_txtEndofThirdStageTime.Text
            vm.REPORTS.txtTwinDeliveredTime = anc07.LAB2DTIME; //pg6_txtTwinDeliveredTime.Text

            //if (anc07.LABONSETDT <= dtmin_date.Date)
            //{
            //     = true; //pg6_txtdtpOnset.Visible
            //     = true; //pg6_txtdtpFullyDilated.Visible
            //     = true; //pg6_txtdtpPushingCommenced.Visible
            //     = true; //pg6_txtdtpHeadDelivered.Visible
            //     = true; //pg6_txtdtpBabyDelivered.Visible
            //     = true; //pg6_txtdtpEndofThirdStage.Visible
            //     = true; //pg6_txtdtpTwinDel.Visible

            //     = true; //pg6_txtOnsetTime.Visible
            //     = true; //pg6_txtFullDilatedTime.Visible
            //     = true; //pg6_txtPushingCommencedTIme.Visible
            //     = true; //pg6_txtHeadDeliveredTIme.Visible
            //     = true; //pg6_txtBabyDeliveredTIme.Visible
            //     = true; //pg6_txtEndofThirdStageTime.Visible
            //     = true; //pg6_txtTwinDeliveredTime.Visible
            //}

            vm.REPORTS.txtFirstStage = anc07.FSTSTAGEHRMIN; //pg6_TxtFirstStage.Text
            vm.REPORTS.txtSecondStage = anc07.SSTSTAGEHRMIN; //pg6_TxtSecondStage.Text
            vm.REPORTS.txtThirdStage = anc07.TSTSTAGEHRMIN; //pg6_TxtThirdStage.Text
            vm.REPORTS.txtLaborDuration = anc07.LABDURATION; //Pg6_LaborDuration.Text

            vm.REPORTS.chkOxytocin = anc07.OXYTOCICS ? true : false; //pg6_chkOxytocin.Checked
            vm.REPORTS.chkOxyErgometrine = anc07.EGOMETRINE ? true : false; //pg6_chkOxyErgometrine.Checked
            vm.REPORTS.txtOxydosageAndTime = anc07.OXYTOCICSDTM; //pg6_txtOxydosageAndTime.Text

            vm.REPORTS.txtCordNoVessels = anc07.CORD; //TxtCordNoVessels.Text
            vm.REPORTS.chkOptMembApparenntly = anc07.MEMBRANES == 1 ? true : false; //optMembApparenntlyComplete.Checked
            vm.REPORTS.chkOptMembIncomplete = anc07.MEMBRANES == 2 ? true : false; //optMembIncomplete.Checked

            vm.REPORTS.txtMeasuredBloodLoss = anc07.BLMEASURE; //pg6_TxtMeasuredBloodLoss.Text
            vm.REPORTS.txtEstimatedBloodloss = anc07.BLESTIMATES; //pg6_TxtEstimatedBloodloss.Text
            vm.REPORTS.txtTotalBloodloss = anc07.BLTOTAL; //pg6_TxtTotalBloodloss.Text

            vm.REPORTS.txtFurtherAction = anc07.FURTHERACTN; //pg6_txtFurtherAction.Text
            vm.REPORTS.chkOptcompletePlacenta = anc07.PLACENTA == 1 ? true : false; //pg6_optcompletePlacenta.Checked
            vm.REPORTS.chkOptIncompletePlacenta = anc07.PLACENTA == 2 ? true : false; //pg6_optIncompletePlacenta.Checked

        }

        void displaydetailsPage5(ANC01 anc01, Vstata vstata, DataTable anc06dt, ANC06 anc06)
        {
            MedHist medhist = new MedHist();

            vm.REPORTS.chkAuditProfile = false; //anc06newRecord = false;
            //DataGridViewRow dgv; // = new DataGridViewRow();
            //DataRow row;
            //recno = 0;

            medhist = MedHist.GetMEDHIST(anc01.GROUPCODE, anc01.PATIENTNO, "", false, true, DateTime.Now.Date, "DESC");

            vm.ANC06S = ErpFunc.ConvertDtToList<MR_DATA.ANC06>(anc06dt);
            
            
            //anc06 = ANC06.GetANC06(pg1_txtANCReference.Text, DateTime.Now.Date);

            if (anc06 == null)
            {
                vm.REPORTS.chkAuditProfile = true; //anc06newRecord
                
                //dataGridView1.Rows[recno].Cells[0].Value = DateTime.Now.ToShortDateString();

                if (vstata != null)
                {
                    vm.REPORTS.txtothername = vstata.OTHERS.ToString(); //dataGridView1.Rows[recno].Cells[6].Value
                    vm.REPORTS.txthomephone = vstata.BPSITTING.ToString(); //dataGridView1.Rows[recno].Cells[7].Value
                    vm.REPORTS.txtworkphone = vstata.WEIGHT.ToString("N2") + "kg"; //dataGridView1.Rows[recno].Cells[8].Value
                    vm.REPORTS.doctor = vstata.DOCTOR.ToString(); //dataGridView1.Rows[recno].Cells[13].Value
                }

                vm.REPORTS.comments = medhist != null ? medhist.COMMENTS.ToString() : ""; //dataGridView1.Rows[recno].Cells[9].Value
            }
        }

        void displaydetailsPage4(ANC04 anc04, Vstata vstata)
        {
            if (anc04 == null)
            {
                vm.REPORTS.chkADVIncludePVTFC = true; //anc04newRecord

                if (vstata == null)
                    return;

                if (anc04 == null)
                    return;

                vm.REPORTS.txtWeight = vstata.WEIGHT.ToString() + " kg"; //TxtWeight.Text
                //	urine WITH vstata.others,;
                vm.REPORTS.combillcycle = vstata.RESPIRATIO; //txtRespiration.Text
                vm.REPORTS.SessionIsbf = vstata.PULSE; //txtPulse.Text
                vm.REPORTS.txtHeight = vstata.HIGHT.ToString() + " mtr"; //TxtHeight.Text
                vm.REPORTS.txtghgroupcode = vstata.BPSITTING; //TxtBloodPressure.Text
                return; 
            }

            vm.REPORTS.chkADVIncludePVTFC = false; //anc04newRecord
            DateTime AssesmentDate = anc04.TRANS_DATE; //dteAssesmentDate.Value
            vm.REPORTS.assesmentDate = string.Format("{0:yyyy-MM-dd}", AssesmentDate);
            vm.REPORTS.txtHeight = anc04.HEIGHT; //TxtHeight.Text 
            vm.REPORTS.txtWeight = anc04.WEIGHT; //TxtWeight.Text 
            vm.REPORTS.txtghgroupcode = anc04.BP; //TxtBloodPressure.Text

            vm.REPORTS.chkHeentAbnormal = anc04.HEENT == 2 ? true : false; //heentAbnormal.Checked
            vm.REPORTS.chkHeentNormal = anc04.HEENT == 1 ? true : false; //heentNormal.Checked
            vm.REPORTS.chkFundiNormal = anc04.FUNDI == 1 ? true : false; //FundiNormal.Checked
            vm.REPORTS.chkFundiAbnormal = anc04.FUNDI == 2 ? true : false; //FundiAbnormal.Checked
            vm.REPORTS.chkTeethNormal = anc04.TEETH == 1 ? true : false; //TeethNormal.Checked
            vm.REPORTS.chkTeethAbnormal = anc04.TEETH == 2 ? true : false; //TeethAbnormal.Checked
            vm.REPORTS.chkThyroidNormal = anc04.THYROID == 1 ? true : false; //ThyroidNormal.Checked
            vm.REPORTS.chkThyroidAbnormal = anc04.THYROID == 2 ? true : false; //ThyroidAbnormal.Checked
            vm.REPORTS.chkBreastNormal = anc04.BREASTS == 1 ? true : false; //BreastNormal.Checked
            vm.REPORTS.chkBreastAbnormal = anc04.BREASTS == 2 ? true : false; //BreastAbnormal.Checked 
            vm.REPORTS.chkLungsNormal = anc04.LUNGS == 1 ? true : false; //LungsNormal.Checked
            vm.REPORTS.chkLungsAbnormal = anc04.LUNGS == 2 ? true : false; //LungsAbnormal.Checked 
            vm.REPORTS.chkHeartNormal = anc04.HEART == 1 ? true : false; //HeartNormal.Checked
            vm.REPORTS.chkHeartAbnormal = anc04.HEART == 2 ? true : false; //HeartAbnormal.Checked
            vm.REPORTS.chkAbdomenNormal = anc04.ABDOMEN == 1 ? true : false; //AbdomenNormal.Checked
            vm.REPORTS.chkAbdomenAbnormal = anc04.ABDOMEN == 2 ? true : false; //AbdomenAbnormal.Checked
            vm.REPORTS.chkExtremitiesNormal = anc04.EXTREMITIES == 1 ? true : false; //ExtremitiesNormal.Checked
            vm.REPORTS.chkExtremitiesAbnormal = anc04.EXTREMITIES == 2 ? true : false; //ExtremitiesAbnormal.Checked
            vm.REPORTS.chkSkinNormal = anc04.SKIN == 1 ? true : false; //SkinNormal.Checked
            vm.REPORTS.chkSkinAbnormal = anc04.SKIN == 2 ? true : false; //SkinAbnormal.Checked
            vm.REPORTS.chkLymphNormal = anc04.LYMPHNODES == 1 ? true : false; //LymphNormal.Checked
            vm.REPORTS.chkLymphAbnormal = anc04.LYMPHNODES == 2 ? true : false; //LymphAbnormal.Checked
            vm.REPORTS.chkOptVulvaNormal = anc04.VULVA == 1 ? true : false; //pg4_optVulvaNormal.Checked
            vm.REPORTS.chkOptVulvaCondyloma = anc04.VULVA == 2 ? true : false; // pg4_optVulvaCondyloma.Checked
            vm.REPORTS.chkOptVulvaLesions = anc04.VULVA == 3 ? true : false; //pg4_OptVulvaLesions.Checked
            vm.REPORTS.chkOptVaginaNormal = anc04.VAGINA == 1 ? true : false; //pg4_optVaginaNormal.Checked
            vm.REPORTS.chkOptDischargeVagina = anc04.VAGINA == 2 ? true : false; //pg4_OptDischargeVagina.Checked
            vm.REPORTS.chkOptCervixNormal = anc04.CERVIX == 1 ? true : false; //pg4_OptCervixNormal.Checked
            vm.REPORTS.chkOptInflamationNormal = anc04.CERVIX == 2 ? true : false; //pg4_optInflamationNormal.Checked
            vm.REPORTS.chkOptLesionNormal = anc04.CERVIX == 3 ? true : false; //pg4_OptLesionNormal.Checked

            vm.REPORTS.RptPath = anc04.UTERINESIZE; //pg4_txtuterussize.Text
            vm.REPORTS.chkOptFibroidYes = anc04.FIBROIDS == 1 ? true : false; //pg4_optFibroidYes.Checked
            vm.REPORTS.chkOptFibroidNo = anc04.FIBROIDS == 2 ? true : false; //pg4_optFibroidNo.Checked
            vm.REPORTS.chkOptnormalAnexa = anc04.ADNEXA == 1 ? true : false; //pg4_optnormalAnexa.Checked
            vm.REPORTS.chkOPtabnormalAdnexa = anc04.ADNEXA == 2 ? true : false; //pg4_OPtabnormalAdnexa.Checked
            vm.REPORTS.chkOptHaemorrhoidsYes = anc04.HAEMORRHOIDS == 1 ? true : false; //pg4_optHaemorrhoidsYes.Checked
            vm.REPORTS.chkOptHaemorrhoidsoptNo = anc04.HAEMORRHOIDS == 2 ? true : false; //pg4_optHaemorrhoidsoptNo.Checked

            vm.REPORTS.cboPVTNameTo = anc04.COMMENTS; //pg4_TxtComments.Text
            vm.REPORTS.SessionSQL = anc04.DELPLAN; //pg4_TxtPlan.Text

            vm.REPORTS.SessionOCP = anc04.INTERVIEWER; //pg4_TxtInterviewdoneBy.Text
        }

        void displaypage3(ANC01 anc01)
        {
            ANC03A anc03a = new ANC03A();

            anc03a = ANC03A.GetANC03A(anc01.GROUPCODE, anc01.PATIENTNO);

            if (anc03a == null)
            {
                vm.REPORTS.PRINT = true; //anc03anewRecord
                return;
            }

            vm.REPORTS.PRINT = false; //anc03anewRecord

            vm.REPORTS.chkbyacctofficers = (anc03a.AGEATEDD) ? true : false; //pg3_ChkpatientsAge.Checked
            vm.REPORTS.chkSortByOperator = (anc03a.SICKLECELL) ? true : false; //pg3_ChkSickleCell.Checked
            vm.REPORTS.chkBroughtForward = (anc03a.DOWNS) ? true : false; //pg3_ChkDownSyndrome.Checked
            vm.REPORTS.chkStaffProfiling = (anc03a.CHROMOSOMAL) ? true : false; //pg3_ChkChromosonalAbnormalities.Checked
            vm.REPORTS.chkDomantAccts = (anc03a.HEARTDISEASE) ? true : false; //pg3_ChkCongenitalHEartDisease.Checked
            vm.REPORTS.chkLoyaltyCustomers = (anc03a.TUBEDEFECT) ? true : false; //create field .chromosomal, pg3_chkotherinheritedgenetic.Checked
            vm.REPORTS.chkReportCustomerName = (anc03a.METABOLIC) ? true : false; //pg3_chkmaternalMetabolic.Checked
            vm.REPORTS.chkByBranch = (anc03a.TUBEDEFECT) ? true : false; //pg3_chkNeuraltube.Checked
            vm.REPORTS.chkReportBankColumn = (anc03a.STILLBIRTH) ? true : false; //pg3_chkHistoryofrecrrentprgloss.Checked
            vm.REPORTS.chkReportbyAgent = (anc03a.MEDICATIONS) ? true : false; //pg3_chkVitamins.Checked;

            vm.REPORTS.chkComparativereport = (anc03a.TB) ? true : false; //pg3_chkTB.Checked
            vm.REPORTS.chkIncludePayments = (anc03a.HERPES) ? true : false; //pg3_chkherpes.Checked
            vm.REPORTS.chkReportGroupFamily = (anc03a.VIRALILLNESS) ? true : false; //pg3_chkviralillnes.Checked
            vm.REPORTS.chkIncludeBf = (anc03a.STI) ? true : false; //pg3_chkSTD.Checked
            vm.REPORTS.chkADVCorporate = (anc03a.HEPATITISB) ? true : false; //pg3_ChkHepatitis.Checked

            vm.REPORTS.Searchdesc = anc03a.INDEXPREG; //pg3_txtIndexpregnancy.Text
            vm.REPORTS.SearchName = anc03a.MEDICATIONDETL; //pg3_txtMedicationDtl.Text
        }

        void displaydetailsPage2(ANC01 anc01)
        {
            ANC02 anc02 = new ANC02();

            anc02 = ANC02.GetANC02(anc01.GROUPCODE, anc01.PATIENTNO);

            if (anc02 == null)
            {
                vm.REPORTS.REPORT_BY_DATE = true; //anc02newRecord
                return;
            }

            vm.REPORTS.REPORT_BY_DATE = false; //anc02newRecord

            vm.REPORTS.chkDiabetes = string.IsNullOrWhiteSpace(anc02.DIABETES) ? false : true; //pg2_ChkDiabetes.Checked
            vm.REPORTS.ChkHypertension = string.IsNullOrWhiteSpace(anc02.HYPERTENSION) ? false : true; //pg2_ChkHypertension.Checked
            vm.REPORTS.chkHeartDisease = string.IsNullOrWhiteSpace(anc02.HEART_DISEASE) ? false : true; //pg2_ChkHeartDisease.Checked
            vm.REPORTS.chkSickleCellDisease = string.IsNullOrWhiteSpace(anc02.SICKLE_CELL) ? false : true; //pg2_ChkSickleCellDisease.Checked
            vm.REPORTS.chkpulmonaryTbasthma = string.IsNullOrWhiteSpace(anc02.PULMONARY) ? false : true; //pg2_ChkpulmonaryTbasthma.Checked
            vm.REPORTS.chkKidneyDisease = string.IsNullOrWhiteSpace(anc02.KIDNEYDISEASE) ? false : true; //pg2_ChkKidneyDisease.Checked
            vm.REPORTS.chkHepatitis = string.IsNullOrWhiteSpace(anc02.HEPATITIS) ? false : true; //pg2_Hepatitis.Checked
            vm.REPORTS.chkNeurologic = string.IsNullOrWhiteSpace(anc02.NEUROLOGIC) ? false : true; //pg2_chkNeurologic.Checked
            vm.REPORTS.chkThyroid = string.IsNullOrWhiteSpace(anc02.THYROID) ? false : true; //pg2_ChkThyroid.Checked
            vm.REPORTS.chkPhychiatric = string.IsNullOrWhiteSpace(anc02.PSYCHIATRIC) ? false : true; //pg2_ChkPhychiatric.Checked

            vm.REPORTS.txtDiabetes = anc02.DIABETES; //pg2_TxtDiabetes.Text
            vm.REPORTS.txtHypertention = anc02.HYPERTENSION; //pg2_TxtHypertention.Text
            vm.REPORTS.txtHeartDisease = anc02.HEART_DISEASE; //pg2_txtHeartDisease.Text
            vm.REPORTS.txtSickleCell = anc02.SICKLE_CELL; //pg2_txtSickleCellDisease.Text
            vm.REPORTS.txtPulmonary = anc02.PULMONARY; // pg2_txtPulmonary.Text
            vm.REPORTS.txtKidney = anc02.KIDNEYDISEASE; //pg2_TxtKidney.Text
            vm.REPORTS.txtHepatitis = anc02.HEPATITIS; //pg2_txtHepatitis.Text
            vm.REPORTS.txtNeurologic = anc02.NEUROLOGIC; //pg2_txtNeurologic.Text
            vm.REPORTS.txtThyroid = anc02.THYROID; //pg2_txtThyroid.Text
            vm.REPORTS.txtPhychiatric = anc02.PSYCHIATRIC; //pg2_TxtPhychiatric.Text

        }

        void displaydetailsPage1(ANC01 anc01, billchaindtl billchain, string pg1DOB)
        {
            DateTime dtmin_date = DateTime.Now;
            DateTime DOB = Convert.ToDateTime(pg1DOB);

            vm.REPORTS.cbotype = anc01.REFERENCE; //pg1_txtANCReference.Text
            vm.REPORTS.txtpatientno = anc01.PATIENTNO; //pg1_txtpatientno.Text
            vm.REPORTS.txtgroupcode = anc01.GROUPCODE; //pg1_txtgroupcode.Text
            vm.REPORTS.TXTPATIENTNAME = anc01.NAME; //pg1_TxtName.Text
            vm.REPORTS.txtaddress1 = billchain.RESIDENCE; //anc01.ADDRESS;
            vm.REPORTS.chkLowRisk = anc01.BOOKINGTAG == 1 ? true : false; //"LOW RISK" 
            vm.REPORTS.chkHighRisk = anc01.BOOKINGTAG == 3 ? true : false; //"HIGH RISK" 
            vm.REPORTS.chkMediumRisk = anc01.BOOKINGTAG == 2 ? true : false; //"MEDIUM RISK"
            vm.REPORTS.cbobloodgroup = anc01.HUSBANDBG; //pg1_combHusbandBloodGroup.Text
            vm.REPORTS.txtbranch = anc01.BOOKINGCATEGORY; //pg1_txtgravida.Text
            
            DateTime lmp = anc01.LMP; //dtLMPpg1.Value
            vm.REPORTS.lmpDate = string.Format("{0:yyyy-MM-dd}", lmp);
            DateTime edd = anc01.EDD; //dtEDDpg1.Value
            vm.REPORTS.eddDate = edd.ToShortDateString();
            DateTime deliveryDate = anc01.DEL_DATE; //dtDeliveryDatepg1.Value
            vm.REPORTS.cboReligion = deliveryDate.ToShortDateString();

            vm.REPORTS.SessionCustomer = anc01.BLOODGROUP; //pg1_combBloodGroup.Text

            if (deliveryDate.Date <= dtmin_date)
            {
                vm.REPORTS.chkReportbyAgent = false; //pg1_txtDeliveryDate.Visible
                vm.REPORTS.chkIncludePayments = true; //chkRecordDeliveryDate.Visible
                vm.REPORTS.cboReligion = ""; //pg1_txtDeliveryDate.Text
            }
            else
                vm.REPORTS.chkReportbyAgent = true; //dtDeliveryDatepg1.Visible

            //header information
            vm.REPORTS.cboAge = (DateTime.Now.Date.Year - billchain.BIRTHDATE.Year).ToString(); // anc01.AGE.ToString(); TxtAge.Text

            vm.REPORTS.SessionCustomer = anc01.BLOODGROUP; //txtbloodgroup.Text
            vm.REPORTS.cbogenotype = anc01.GENOTYPE; //TxtGT.Text
            vm.REPORTS.txtBed = (anc01.EDD.Year <= dtmin_date.Year) ? "" : anc01.EDD.ToShortDateString(); //txtEDDpg1Header.Text
            vm.REPORTS.patientName = anc01.NAME; //TxtPatient.Text
            vm.REPORTS.cboAge = anc01.AGE.ToString(); //pg1_TxtAge.Text

            if (billchain.PATIENTNO == billchain.GROUPHEAD || billchain.GROUPHTYPE == "P")
                vm.REPORTS.cashpaying = true; //cashpaying
            else
            {
                vm.REPORTS.cashpaying = false; //cashpaying
            }

            if (DOB <= dtmin_date.Date || DOB == DateTime.Now.Date)
            {
                DateTime dtDOB = billchain.BIRTHDATE; //dtDOBpg1.Value;
                vm.REPORTS.txtDateOfBirth = string.Format("{0:yyyy-MM-dd}", dtDOB);
                //vm.REPORTS.txtworkphone = dtDOB.ToString();
                vm.REPORTS.cboAge = (DateTime.Now.Date.Year - billchain.BIRTHDATE.Year).ToString(); //pg1_TxtAge.Text
            }

            DateTime dateOfBooking = anc01.REG_DATE; //dtDateofBookingpg1.Value
            vm.REPORTS.combFacility = string.Format("{0:yyyy-MM-dd}", dateOfBooking);

            vm.REPORTS.txtTimeTo = anc01.REG_TIME; //dtTimeofBookpg1.Text

            //bissclass.displaycombo(pg1_combDoctor, dtdoctors, anc01.DOCTOR, "name");
            vm.REPORTS.cbokinstate = anc01.DURATIONOFPREGNANCY; //pg1_txtGestationPeriod.Text

            vm.REPORTS.cboTribe = anc01.TRIBE; //pg1_txtTribe.Text

            vm.REPORTS.cbooccupation = anc01.OCCUPATION; //pg1_combOccupatonWf.Text
            vm.REPORTS.chkBasicMedicProfile = anc01.LEVELOFEDUCATION == 0 ? true : false; //OptNone.Checked
            vm.REPORTS.chkApplyFilter = anc01.LEVELOFEDUCATION == 1 ? true : false; //optPrimary.Checked
            vm.REPORTS.chkSecondary = anc01.LEVELOFEDUCATION == 2 ? true : false; //OptSecondary.Checked
            vm.REPORTS.chkHMO = anc01.LEVELOFEDUCATION == 3 ? true : false; //OptTertiary.Checked
            vm.REPORTS.txtHusbandName = anc01.HUSBANDNAME; //pg1_txtHusbandName.Text
            vm.REPORTS.txtsurname = anc01.HUSBANDOCCUPATION; //pg1_combOccupationHB.Text
            vm.REPORTS.txtemployer = anc01.HUSBANDEMPLOYER;
            vm.REPORTS.chkByDateRange = anc01.HUSBANDLEVELOFEDUCATION == 0 ? true : false; //optHusbandNone.Checked
            vm.REPORTS.chkQueryTimeofDay = anc01.HUSBANDLEVELOFEDUCATION == 1 ? true : false; //optHusbandPrimary.Checked
            vm.REPORTS.chkReportSum = anc01.HUSBANDLEVELOFEDUCATION == 2 ? true : false; //optHusbandSecondary.Checked
            vm.REPORTS.chkCurrtAdmRev = anc01.HUSBANDLEVELOFEDUCATION == 3 ? true : false; //optHusbandTertiary.Checked

            vm.REPORTS.txtemail = anc01.HUSBANDPHONE; //txthusbankphoneEmail.Text
            vm.REPORTS.txtstaffno = anc01.HUSBANDGC; //pg1_txtgroupcodeHB.Text
            vm.REPORTS.txtdepartment = anc01.HUSBANDPATNO; //pg1_txtpatientnoHB.Text
            vm.REPORTS.txtHbBloodGrp = anc01.HUSBANDBG; //pg1_combHusbandBloodGroup.Text

            vm.REPORTS.txtSpInstruct = anc01.SPNOTES; //pg1_txtInstructions.Text
            //  dtDOBpg1.Value = anc01.BIRTHDATE;
            vm.REPORTS.cbogenotype = anc01.GENOTYPE; // pg1_combGenotype.Text 
            vm.REPORTS.msection = anc01.MENS_REGULARITY; //pg1_TxtRegularity.Text
            vm.REPORTS.mcusttype = anc01.CONTRACEPTIVEUSE; //pg1_TxtContrapceptive.Text
            vm.REPORTS.mgroupcode = anc01.RISKFACTOR; //pg1_txtAllergies.Text
            vm.REPORTS.mgrouphtype = anc01.HUSBANDGENOTYPE; //pg1_CombHusbandgenotype.Text
            vm.REPORTS.txtgrouphead = anc01.MENARCHE; //pg1_Txtmenarche.Text

            if (Convert.ToDecimal(vm.REPORTS.cboAge) < 15)
            {
                vm.REPORTS.ActRslt = "Please check this Patients Age...... CONTINUE ?";

                //if (result == DialogResult.No)
                //{
                //    dtDOBpg1.Select(); 
                //    return;
                //}
            }
        }

        void displayANCPages(ANC01 anc01, billchaindtl billchain, string pg1ConsultRef, string pg1ANCreference, string pg1DOB)
        {
            ANC04 anc04 = new ANC04();
            ANC06 anc06 = new ANC06();
            Vstata vstata = new Vstata();
            DataTable anc06dt;
            DateTime dtmin_date = DateTime.Now;

            vstata = Vstata.GetVSTATA(pg1ConsultRef);
            anc04 = ANC04.GetANC04(pg1ANCreference);
            anc06dt = ANC06.GetANC06(pg1ANCreference);
            anc06 = ANC06.GetANC06(pg1ANCreference, DateTime.Now.Date);

            displaydetailsPage1(anc01, billchain, pg1DOB);
            displaydetailsPage2(anc01);
            displaypage3(anc01);
            displaydetailsPage4(anc04, vstata);
            displaydetailsPage5(anc01, vstata, anc06dt, anc06);
            displaydetailsPage6(anc01);
            displaydetailsPage7(anc01);
            displaydetailsPage8(anc01);

            if (anc01.DEL_DATE > dtmin_date)
            {
                vm.REPORTS.alertMessage = "This Record is closed... Patient delivered on " + anc01.DEL_DATE.ToLongDateString();

                //btnSave.Enabled = btnSubmitPage2.Enabled = btnSubmitPage3.Enabled = btnSubmitPage6.Enabled = 
                //    btnSubmitPage7.Enabled = btnSubmitPage8.Enabled = btnSubmitPg4.Enabled = pg5_btnSave.Enabled = 
                //    Pg9_btnSubmit.Enabled = false;

                vm.REPORTS.REPORT_TYPE3 = anc01.DEL_DATE.ToShortDateString(); //pg1_txtDeliveryDate.Text 
            }
            else
            {
                //btnSave.Enabled = true;
            }

            //display previous medical history
            displayPrevMedHistory(true, anc01, billchain);
            displaySpecialInstructions(billchain);
        }

        public JsonResult pg1ConsultRefFocusout(string pg1ConsultRef, string pg1DOB, string pg1ANCreference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Mrattend mrattend = new Mrattend();
            billchaindtl billchain = new billchaindtl();
            ANC01 anc01 = new ANC01();
            DateTime dtmin_date = DateTime.Now;

            if (bissclass.IsDigitsOnly(pg1ConsultRef))  //no lookup value obtained
            {
                vm.REPORTS.txtreference = bissclass.autonumconfig(pg1ConsultRef, true, "C", "999999999");
            }

            mrattend = Mrattend.GetMrattend(pg1ConsultRef);

            if (mrattend == null)
            {
                vm.REPORTS.newrecString = "Unable to Link Consultation Reference in Daily Attendance Register...";
                //vm.REPORTS.txtreference = "";
                
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.txtgroupcode = mrattend.GROUPCODE;
            vm.REPORTS.txtpatientno = mrattend.PATIENTNO;
            vm.REPORTS.TXTPATIENTNAME = mrattend.NAME;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select reference from ancreg where groupcode = '" +
                mrattend.GROUPCODE + "' and patientno = '" + mrattend.PATIENTNO + "' AND DEL_DATE <= '" + 
                dtmin_date + "' ", false);

            // if (!xval)
            if (dt.Rows.Count < 1)
            {
                vm.REPORTS.newrecString = "This Patient -> " + mrattend.NAME.Trim() + " [ " + mrattend.PATIENTNO.Trim() + 
                    " ]" + "\r\n  does not have a Current ANC Registration Profile... her PATIENTNO may have " + 
                    "\r\n been changed by Record Officers between the Original ANC Registration and this Attendance..." +
                    "\r\n Please Check and try again !!!";

                vm.REPORTS.txtreference = "";
               
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            //scan anc01 for current registration
            string xreference = dt.Rows[0]["reference"].ToString(); // returnedAncRef; // ancrtnstring.Substring(0, 9);
            anc01 = ANC01.GetANC01(xreference);

            if (anc01 == null)
            {
                vm.REPORTS.newrecString = "This Patient -> " + mrattend.NAME.Trim() + " [ " +
                    mrattend.PATIENTNO.Trim() + " ]" + "\r\n ANC REFERENCE :" + xreference + 
                    " is not on ANC Registration Profiles... her PATIENTNO may have " + 
                    "\r\n been changed by Record Officers between the Original ANC Registration and this Attendance..." +
                    "\r\n Please Check and try again !!!";

                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            billchain = billchaindtl.Getbillchain(anc01.PATIENTNO, anc01.GROUPCODE);
            if (billchain == null)
            {
                vm.REPORTS.alertMessage = "Error Reading Patients Details in Master File... \r\n Pls Check Patients Registration Details!";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.cbotype = xreference; // pg1_txtANCReference.Text
                                             //pg1_txtANCReference.Enabled = false;

            displayANCPages(anc01, billchain, pg1ConsultRef, pg1ANCreference, pg1DOB);
            //displayANCPages(anc01, billchain, pg1DOB);
            
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadListBtnClick(string pg1ConsultRef, string msection, string mdoctor)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dt1 = Dataaccess.GetAnytable("", "MR",
                "select docson,pvtcode from mrcontrol where recid < 4 order by recid", false);
            
            string manccode = dt1.Rows[2]["pvtcode"].ToString().Substring(0, 5);

            DataTable dt = getLinkDetails(pg1ConsultRef, 0, 0m, 0m, manccode, true, msection, 9, "", mdoctor);

            if (dt.Rows.Count > 0)
            {
                vm.REPORTS.txtreference = msmrfunc.mrGlobals.anycode; //txtConsultReference.Text
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //Ante_Natal Records End



        //Admissions Management Start
        #region

        //Discharge
        public JsonResult dSubmitBtnClick(MR_DATA.REPORTS dataList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;
            vm.REPORTS = dataList;

            Admissions formObject = new Admissions(vm, woperato);
            vm.REPORTS = formObject.btnSubmitD_Click();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void calc_prof_fees(bool iswrite, string dAdmReference, decimal dAccumCharge)
        {
            // string mgldocument,mdtformat;
            string masterprocessbillcode, billcode;
            decimal percentagemarkup = 0m;

            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();
            Customer customers = new Customer();

            //check if reference exist
            admrecs = Admrecs.GetADMRECS(dAdmReference);
            bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);
            customers = Customer.GetCustomer(bchain.GROUPHEAD);

            DataTable dt = Dataaccess.GetAnytable("", "MR", 
                "SELECT gldocument, fccode, paediacons, dtformat from mrcontrol order by recid", false);

            DateTime dtmin_date = DateTime.Now;
            string woperator = Request.Cookies["mrName"].Value;

            masterprocessbillcode = dt.Rows[6]["gldocument"].ToString().Length > 6 ? 
                dt.Rows[6]["gldocument"].ToString().Substring(0, 7) : "";// master process
                                                                                                                                              //17/09/2011 - 12.15AM, 6-3-2014 - CHECK PATIENT GROUP AND % TO ADD
            if (bchain.GROUPHTYPE == "P")
                percentagemarkup = (decimal)dt.Rows[6]["paediacons"];

            billcode = dt.Rows[6]["dtformat"].ToString().Length > 6 ? dt.Rows[6]["dtformat"].ToString().Substring(0, 7) : "";
            
            //  encryptedaccesscode = dt.Rows[1]["fccode"].ToString();
            //check if specific corporate has defined %
            if (bchain.GROUPHTYPE == "C" && customers.MAX_ORD_AM > 0)
                percentagemarkup = customers.MAX_ORD_AM;
            else if (bchain.GROUPHTYPE == "C" && string.IsNullOrWhiteSpace(bchain.HMOSERVTYPE))
                percentagemarkup = (decimal)dt.Rows[0]["paediacons"];
            else if (bchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(bchain.HMOSERVTYPE))
                percentagemarkup = (decimal)dt.Rows[1]["paediacons"];

            decimal xamt = dAccumCharge;
            vm.REPORTS.nmrbalance = (xamt * percentagemarkup) / 100; // nmrProfCharge_D.Value
            vm.REPORTS.lblbillonaccount = percentagemarkup.ToString("N2") + " %"; //lblproffees.Text

            if (!iswrite)
                return;

            string mdesc = "", xfacility = ""; // bissclass.seeksay("select name from tariff where reference = '" + billcode + "'", "MR", "NAME");

            msmrfunc.getFeefromtariff(billcode, bchain.PATCATEG, ref mdesc, ref xfacility);
            //write to admdetail
            ADMDETAI.writeAdmdetails(true, dAdmReference, DateTime.Now.Date, DateTime.Now.ToShortTimeString(), 
                billcode, masterprocessbillcode, "", mdesc, "", 1m, vm.REPORTS.nmrbalance, false, dtmin_date, 
                woperator, DateTime.Now, bchain.GROUPCODE, bchain.PATIENTNO, "", xfacility, 0, "");

            Admrecs.UpdateAdmrecAmounts(dAdmReference, vm.REPORTS.nmrbalance, 0m);
        }

        public JsonResult dDateFocusout(string dAdmReference, decimal dAccumCharge)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dt = Dataaccess.GetAnytable("", "MR",
               "select fccode, facilauto, facilauto, facilauto, glintenabl, dactive, pvtcode, installed, serial, ta_post from mrcontrol order by recid", false);

            bool mcalc_prof_fees = (bool)dt.Rows[6]["glintenabl"];

            if (mcalc_prof_fees)
                calc_prof_fees(false, dAdmReference, dAccumCharge);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult dAdmReferenceFocusout(string dAdmReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name ", true),
                dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES order by name", true);

            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();

            //check if reference exist
            admrecs = Admrecs.GetADMRECS(dAdmReference);

            if (dAdmReference.Substring(0, 1) != "A" && bissclass.IsDigitsOnly(dAdmReference.Trim()))
            {
                vm.REPORTS.txtreference = bissclass.autonumconfig(dAdmReference, true, "A", "999999999");
            }

            if (admrecs == null)
            {
                vm.REPORTS.alertMessage = "Invalid Admission Reference...";
                //txtReference_D.Text = "";
                //txtReference_D.Select();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.REPORTS.txtreference = dAdmReference;
                vm.REPORTS.mgrouphtype = admrecs.GROUPHTYPE;
                vm.REPORTS.txtpatientno = admrecs.PATIENTNO;
                vm.REPORTS.txtgroupcode = admrecs.GROUPCODE;
                vm.REPORTS.combFacility = bissclass.combodisplayitemCodeName("type_code", admrecs.FACILITY, dtfacility, "name");
                vm.REPORTS.TXTPATIENTNAME = admrecs.NAME;
                vm.REPORTS.txtRoom = admrecs.ROOM;
                vm.REPORTS.txtBed = admrecs.BED;
                vm.REPORTS.diagnosis = bissclass.combodisplayitemCodeName("type_code", admrecs.DIAGNOSIS, dtdiag, "name") + " " + admrecs.DIAGNOSIS_ALL.Trim();
                vm.REPORTS.REPORT_TYPE1 = admrecs.ADM_DATE.ToShortDateString(); //txtadm_dateD.Text
                vm.REPORTS.REPORT_TYPE2 = admrecs.DISCHARGE;
                DateTime xdischarge = string.IsNullOrWhiteSpace(admrecs.DISCHARGE) ? DateTime.Now.Date : Convert.ToDateTime(admrecs.DISCHARGE);
                vm.REPORTS.cbotitle = xdischarge.Subtract(admrecs.ADM_DATE).TotalDays.ToString() + " day(s)";
                vm.REPORTS.mgrouphead = admrecs.GROUPHEAD;
                //  nmrAccumCharge_D.Value = admrecs.ACAMT;
                vm.REPORTS.txtdiscount = getAccummulatedCharge(admrecs.REFERENCE, admrecs.ACAMT);
                vm.REPORTS.cbotype = "YES"; //cboRaiseBill_D.SelectedItem
                bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);
                vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
                vm.REPORTS.txtsurname = patientprofile(bchain);
                vm.REPORTS.txtgrouphead = getgrouphead(bchain.GROUPHEAD, bchain.GHGROUPCODE, bchain.GROUPHTYPE);

                if (vm.REPORTS.txtgrouphead.Trim() == "Abort")
                {
                    //txtReference_D.Focus();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                //displayPatientPicture(this.pictureBox_Discharge, bchain.PICLOCATION);

                if (!string.IsNullOrWhiteSpace(admrecs.DISCHARGE))
                {
                    vm.REPORTS.ActRslt = "This Patient has been discharged on " + admrecs.DISCHARGE + ", " + admrecs.NAME;
                    //txtReference_D.Text = "";
                    //txtReference_D.Select();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                //dttrans_dateD.Focus();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

        }


        //Change Bed / Room
        public JsonResult cbSubmitBtnClick(MR_DATA.REPORTS dataList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;
            vm.REPORTS = dataList;

            Admissions formObject = new Admissions(vm, woperato);
            vm.REPORTS = formObject.btnSubmitCB_Click();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult cbAdmReferenceFocusout(string cbAdmReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name ", true),
            dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES order by name", true);

            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();

            admrecs = Admrecs.GetADMRECS(cbAdmReference);

            if (cbAdmReference.Substring(0, 1) != "A")
            {
                vm.REPORTS.txtreference = bissclass.autonumconfig(cbAdmReference, true, "A", "999999999");
            }

            //check if reference exist
            //AnyCode = Anycode1 = "";

            if (admrecs == null) //new defintion
            {
                vm.REPORTS.alertMessage = "Invalid Admission Reference...";
                //vm.REPORTS.txtreference = "";
                //txtReference_BC.Select();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.REPORTS.mgrouphtype = admrecs.GROUPHTYPE;
                vm.REPORTS.txtpatientno = admrecs.PATIENTNO;
                vm.REPORTS.txtgroupcode = admrecs.GROUPCODE;
                vm.REPORTS.combFacility = bissclass.combodisplayitemCodeName("type_code", admrecs.FACILITY, dtfacility, "name");
                vm.REPORTS.TXTPATIENTNAME = admrecs.NAME;
                vm.REPORTS.txtstaffno = admrecs.RATE.ToString(); //txtoldRateCB.Text
                vm.REPORTS.txtRoom = admrecs.ROOM;
                vm.REPORTS.txtBed = admrecs.BED;
                vm.REPORTS.diagnosis = bissclass.combodisplayitemCodeName("type_code", admrecs.DIAGNOSIS, dtdiag, "name") + " " + admrecs.DIAGNOSIS_ALL.Trim();
                vm.REPORTS.REPORT_TYPE1 = admrecs.ADM_DATE.ToShortDateString(); //txtadm_dateCB.Text
                vm.REPORTS.REPORT_TYPE2 = admrecs.DISCHARGE;
                DateTime xdischarge = string.IsNullOrWhiteSpace(admrecs.DISCHARGE) ? DateTime.Now.Date : Convert.ToDateTime(admrecs.DISCHARGE);
                vm.REPORTS.REPORT_TYPE3 = xdischarge.Subtract(admrecs.ADM_DATE).TotalDays.ToString() + " day(s)"; //lbllenghtofStay_CB.Text
                vm.REPORTS.mgrouphead = admrecs.GROUPHEAD;
                bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);
                vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
                vm.REPORTS.txtsurname = patientprofile(bchain);
                vm.REPORTS.txtgrouphead = getgrouphead(bchain.GROUPHEAD, bchain.GHGROUPCODE, bchain.GROUPHTYPE); //txtgroupheadchangebed.Text

                if (vm.REPORTS.txtgrouphead.Trim() == "Abort")
                {
                    //txtReference_BC.Focus();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                //displayPatientPicture(this.pictureBox_CB, bchain.PICLOCATION);

                if (admrecs.DISCHARGE != "")
                {
                    vm.REPORTS.alertMessage = "This Patient has been discharged on " + admrecs.DISCHARGE + "," + admrecs.NAME;
                    //vm.REPORTS.txtreference = "";
                    //txtReference_BC.Select();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.txtdiscount = getAccummulatedCharge(admrecs.REFERENCE, admrecs.ACAMT); //nmrCummAmt_BC.Value
                //combfacilityCB.Focus();

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult cbFacilityFocusout(string cbFacility)
        {
            vm.ADMSPACES = ErpFunc.RsGet<MR_DATA.ADMSPACE>("MR_DATA",
                "SELECT * FROM ADMSPACE WHERE FACILITY='" + cbFacility + "' ORDER BY ROOM");

            return Json(vm.ADMSPACES, JsonRequestBehavior.AllowGet);
        }


        //Service Items
        public JsonResult siDeleteBtnClick(string headerDesc, string tariffCode, string status)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            DataTable dt = Dataaccess.GetAnytable("", "MR",
              "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperato + "'", false);

            int mdoc_seclevel = (Int32)dt.Rows[0]["wseclevel"];

            if (mdoc_seclevel < 9)
            {
                vm.REPORTS.alertMessage = "Access Denied... See your Systems Administrator!";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            if (status == "OldRec")
            {
                string updstr = "delete from Dispserv where rtrim(reference) = '" + tariffCode + "'";
                bissclass.UpdateRecords(updstr, "MR");
            }

            vm.REPORTS.ActRslt = "Deleted Successfully...";
            //listViewSI.Items[recno].Remove();
            //btnDeleteSI.Enabled = false;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult siSubmitBtnClick(IEnumerable<MR_DATA.REPORTS> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            DataTable dt = Dataaccess.GetAnytable("", "MR",
              "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperato + "'", false);

            int mdoc_seclevel = (Int32)dt.Rows[0]["wseclevel"];

            if (mdoc_seclevel < 9)
            {
                vm.REPORTS.alertMessage = "Access Denied... See your Systems Administrator!";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            Admissions formObject = new Admissions(vm, woperato);
            vm = formObject.btnsubmitSI_Click(tableList);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }



        //Review
        public JsonResult prescHistoryBtnClick(string revGroupCode, string revPatientNo, string revTreatmentDate)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            billchaindtl phbchain = new billchaindtl();
            phbchain = Getbillchain(revPatientNo, revGroupCode);

            vm.REPORTS.txtgroupcode = phbchain.GROUPCODE;
            vm.REPORTS.txtpatientno = phbchain.PATIENTNO;
            vm.REPORTS.TXTPATIENTNAME = phbchain.NAME;

            var xdate = Convert.ToDateTime(revTreatmentDate);
            var datefrom = xdate.AddDays(-90); //datefrom
            var dateto = xdate; //dateto

            vm.REPORTS.REPORT_TYPE1 = datefrom.ToShortDateString();
            vm.REPORTS.REPORT_TYPE2 = xdate.ToShortDateString();
            

            vm.DISPENSAS = ErpFunc.RsGet<MR_DATA.DISPENSA>("MR_DATA",
               "SELECT dispensa.STK_DESC,dispensa.TRANS_DATE,  dispensa.QTY_PR, dispensa.CUMGV, dispensa.CDOSE, " +
                "dispensa.CINTERVAL,dispensa.CDURATION, dispensa.DOCTOR, dispensa.NURSE, dispensa.TYPE, dispensa.RX, " +
                " dispensa.SP_INST AS RX from dispensa where dispensa.groupcode = '" + vm.REPORTS.txtgroupcode +
                "' and dispensa.patientno = '" + vm.REPORTS.txtpatientno + "' and dispensa.trans_date >= '" +
                datefrom.ToShortDateString() + "' and dispensa.trans_date <= '" + dateto.ToShortDateString() +
                " 23:59:59:999' UNION SELECT inpdispensa.STK_DESC, inpdispensa.TRANS_DATE, inpdispensa.QTY_PR, " +
                "inpdispensa.CUMGV, inpdispensa.CDOSE, inpdispensa.CINTERVAL,inpdispensa.CDURATION, inpdispensa.DOCTOR, " +
                "inpdispensa.NURSE, 'I' AS TYPE, inpdispensa.RX, inpdispensa.SP_INST AS RX from inpdispensa where " +
                "inpdispensa.groupcode = '" + vm.REPORTS.txtgroupcode + "' and inpdispensa.patientno = '" +
                vm.REPORTS.txtpatientno + "' and inpdispensa.trans_date >= '" + datefrom.ToShortDateString() +
                "' and inpdispensa.trans_date <= '" + dateto.ToShortDateString() + " 23:59:59:999' ");

            if (vm.DISPENSAS.Count() < 1)
            {
                vm.REPORTS.alertMessage = "No Prescription Record for the period...";
                return Json(vm, JsonRequestBehavior.AllowGet);
                //btnClose.PerformClick();
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult prescConvertOPDBtnClick(IEnumerable<MR_DATA.DISPENSA> tableList, string revGroupCode, 
            string revPatientNo, string revTreatmentDate, string mdoctor, string prescSpecInstrn)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            billchaindtl phbchain = new billchaindtl();
            phbchain = Getbillchain(revPatientNo, revGroupCode);

            PrescriptionsNew formObject = new PrescriptionsNew(vm, woperato);
            vm.REPORTS = formObject.btnConvert_Click(tableList, phbchain, revTreatmentDate, mdoctor, prescSpecInstrn);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }
          
        public JsonResult prescRoutineDrugsFocusout(string prescRoutineDrugs, string revGroupCode, string revPatientNo, 
            bool inpatient)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            billchaindtl phbchain = new billchaindtl();
            Customer customer = new Customer();

            phbchain = Getbillchain(revPatientNo, revGroupCode);
            customer = Customer.GetCustomer(phbchain.GROUPHEAD);

            vm.ROUTDRGSS = ErpFunc.RsGet<MR_DATA.ROUTDRGS>("MR_DATA",
              "select reference, description, qty, unit, cost, drugs, GLOBAL_DIFF_CHG, CORP_DIFF_CHG, DOSE, " +
              " INTERVAL, DURATION, CDOSE, CINTERVAL, CDURATION, WHENHOW FROM routdrgs where reference = '" +
              prescRoutineDrugs + "' order by description");

            //string[] arr = new string[21];
            //ListViewItem itm;
            decimal xcost = 0m;
            bool preauthorization = false, tocontue = false, capitated = false;
            //int xrow = listView1.Items.Count;

            foreach (var row in vm.ROUTDRGSS)
            {
                if (((bool)row.GLOBAL_DIFF_CHG || (bool)row.CORP_DIFF_CHG)) //&& phbchain.GROUPHTYPE == "C")
                {
                    if (!string.IsNullOrWhiteSpace(phbchain.HMOSERVTYPE)) //hmo patient
                    {
                        xcost = msmrfunc.CheckCorpPatientStkDefined(phbchain.GROUPHEAD, phbchain.GROUPHTYPE, 
                            phbchain.HMOSERVTYPE, phbchain.GROUPCODE, false, inpatient, ref xcost, customer.noncapitation, 
                            row.DRUGS, ref preauthorization, ref capitated, ref tocontue, row.DESCRIPTION);
                    }
                    else
                    {
                        xcost = msmrfunc.othercorpClientTariffCheck(phbchain.PATIENTNO, phbchain.PATCATEG,
                          ref preauthorization, ref tocontue, row.DESCRIPTION, row.DRUGS);
                    }
                }
                else
                {
                    xcost = (decimal)row.COST;
                }

            }

            vm.REPORTS.nmrbalance = xcost;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult prescClearSelectBtnClick(string msection, string revAdmReference, string revTreatmentDate, 
            string revGroupCode, string revPatientNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            billchaindtl phbchain = new billchaindtl();
           
            phbchain = Getbillchain(revPatientNo, revGroupCode);


            loadprevDefinitions(msection, revAdmReference, phbchain, revTreatmentDate);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult prescWriteSaveBtnClick(MR_DATA.REPORTS dataObject, IEnumerable<MR_DATA.DISPENSA> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            billchaindtl phbchain = new billchaindtl();
            phbchain = Getbillchain(dataObject.txtpatientno, dataObject.txtgroupcode);

            PrescriptionsNew formObject = new PrescriptionsNew(vm, woperato);
            vm.REPORTS = formObject.btnsave_Click(dataObject, tableList, phbchain);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult prescDubDeleteBtnClick(bool prescChkInpatient, string recID)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            string xfile = prescChkInpatient ? "inpdispensa" : "dispensa";
            string updstr = "delete from " + xfile + " where recid = '" + recID + "'";

            bissclass.UpdateRecords(updstr, "MR");

            //we need to adjust bill
            vm.REPORTS.chkADVCorporate = true; //btnsave.Enabled;

            vm.REPORTS.alertMessage = "You must click the Submit Button To Adjust Bills and other records accordingly...";

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void anc_check(ref decimal xcost, billchaindtl phbchain, string mstkcode)
        {
            string xref = "", ct = "";
            bool xv = false;
            decimal xvamt = 0;

            ANCREG.GetANCREG(phbchain.PATIENTNO, phbchain.GROUPCODE, "D", mstkcode, ref xcost, 
                ref ct, ref xv, ref xvamt, ref xref);

            vm.REPORTS.txtdiscount = xcost; //for nmrCost.Value
        }

        void LoadListviewDtl(decimal prescUnitCost, decimal prescQtyReqd, bool prescChkInpatient, string prescPatientNo, 
            string prescGroupCode, string txtdose, string prescInterval, string prescDuration, string prescUnitId,
            string prescNewRec, string unitIDsave, bool isanc, bool iscapitated, string mstkcode, string msection)
        {
            billchaindtl phbchain = new billchaindtl();
            phbchain = billchaindtl.Getbillchain(prescPatientNo, prescGroupCode);

            DataTable dt = Dataaccess.GetAnytable("", "MR",
                "SELECT glbatchno, ecgno, dischtime, othercharg, installed, pvtcode, dischtime, serial, fsh, FESTLEVPAS from mrcontrol order by recid", false);

            decimal mdrgmarkup = (Decimal)dt.Rows[1]["ecgno"];
            bool markupdrgbillonPercentage = Convert.ToBoolean(dt.Rows[1]["fsh"]);

            bool newrec = prescNewRec == "true" ? true : false;
            decimal xcost;
            vm.REPORTS.txtdiscount = prescQtyReqd * prescUnitCost; //for nmrCost.Value

            //17.05.2019 - check and add markup if enabled
            if (phbchain.GROUPHTYPE == "P" && !prescChkInpatient)
            {
                if (mdrgmarkup != 0m) //outpatient
                {
                    if (markupdrgbillonPercentage)
                        vm.REPORTS.txtdiscount += ((vm.REPORTS.txtdiscount * mdrgmarkup) / 100); //nmrCost.Value
                    else
                        vm.REPORTS.txtdiscount += mdrgmarkup; //nmrCost.Value
                }
            }

            if (string.IsNullOrWhiteSpace(txtdose) && string.IsNullOrWhiteSpace(prescInterval) && string.IsNullOrWhiteSpace(prescDuration))
            {
            }
            else
            {
                //03/08/2011 - 8.57 - We must attempt to put a check here against irresponsible consulting
                string xlabel = prescUnitId.Length > 2 ? prescUnitId.Substring(0, 3).ToUpper() : prescUnitId.Trim().ToUpper();

                if ((xlabel == "BTL" || xlabel == "BOT" || xlabel == "VIA" || xlabel == "SYR" || xlabel == "INJ" || 
                    xlabel == "AMP" || unitIDsave == "Cream") && prescQtyReqd > 2)
                {
                    //result = Messa "Please Check Your Prescription criterial - Quantity Required Inconsistency !!!", "QUANTITY REQUIRED ALERT!!!");
                    //if (result == DialogResult.No)
                    //{
                    //    return;
                    //}
                    //result = MessageBox.Show("Would you Prescribe " + nmrQtyReqd.Value.ToString() + " of " + txtDescription.Text.Trim() + " at once...?", "Prescriptions Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    //if (result == DialogResult.No)
                    //{
                    //    return;
                    //}
                }
            }
          
            xcost = vm.REPORTS.txtdiscount;
            if (isanc)
            {
                anc_check(ref xcost, phbchain, mstkcode);
                vm.REPORTS.txtdiscount = xcost;
            }

            if (iscapitated)
                vm.REPORTS.nmrBalbf = (prescQtyReqd * prescUnitCost); // for nmrCapitedCost.Value
            else
                vm.REPORTS.nmrcurcredit += vm.REPORTS.txtdiscount; //for nmrCurrentTotal.Value

            vm.REPORTS.chkReportSum = iscapitated;

            //save item counter, if old record and restore after init
            string uid = prescUnitId;

            //decimal newcounter = counter;

            if (!newrec)
            {
                //We update the row;
                vm.REPORTS.chkgetdependants = true;                
            }
            else
            {
                //We add a row;
                vm.REPORTS.chkIncludeBf = true;
            }

            if (newrec && msection == "8") //for medhist note
            {
                vm.REPORTS.chkIncludeOnHold = true;
            }

            //  lv_written = true;
            //anycode = "";
            vm.REPORTS.chkApplyFilter = true; //for btnsave.Enabled
            //ClearControls("");
            //txtStkCode.Focus();

            return;
        }

        public JsonResult prescLoadListView(decimal prescUnitCost, decimal prescQtyReqd, bool prescChkInpatient, string prescPatientNo,
            string prescGroupCode, string txtdose, string prescInterval, string prescDuration, string prescUnitId, string prescNewRec,
            string unitIDsave, bool isanc, bool iscapitated, string mstkcode, string msection)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            LoadListviewDtl(prescUnitCost, prescQtyReqd, prescChkInpatient, prescPatientNo, prescGroupCode, txtdose, prescInterval,
                prescDuration, prescUnitId, prescNewRec, unitIDsave, isanc, iscapitated, mstkcode, msection);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult prescDurationFocusout(int prescDuration, string prescDurationText, string prescIntervalText,
            decimal minterval, decimal mduration, decimal mdose, bool isconvertable, decimal packqty, decimal prescDose)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.nmrPayable = mduration;
            vm.REPORTS.nmrcurcredit = mdose;


            decimal xqtysave, xdose, x;

            if (prescDuration == 10) //OTHERS - specify
            {
                prescDurationText = bissclass.GetNumberFromString(prescDurationText.Trim()).ToString() + " days"; //comboDuration.Text
            }

            //int xindex = -1;
            //if (comboDuration.Text.Trim().Length == 1 && comboDuration.SelectedIndex == -1)
            //{
            //    comboDuration.Text = GetComboText(comboDuration, comboDuration.Text.Trim(), ref xindex);
            //    comboDuration.SelectedIndex = xindex;
            //}

            if (prescDurationText != null)
            {
                vm.REPORTS.nmrPayable = prescDuration < 6 ? Convert.ToDecimal(prescDurationText.Substring(0, 1)) : 
                    prescDuration < 8 ? Convert.ToDecimal(prescDurationText.Substring(0, 2)) : 
                    bissclass.GetNumberFromString(prescDurationText);
            }
            else
                vm.REPORTS.nmrPayable = bissclass.GetNumberFromString(prescDurationText);

            if (prescIntervalText == null)
                vm.REPORTS.txtdiscount = mdose; //nmrQtyReqd.Value
            else
            {
                xdose = vm.REPORTS.nmrcurcredit = prescDose;

                if (isconvertable)
                {
                    //xdose = Stkstrength / stkper;
                    xdose = vm.REPORTS.nmrcurcredit / xdose;
                }
                x = minterval < 1 ? 1 : 24m / minterval;
                vm.REPORTS.txtdiscount = (x * vm.REPORTS.nmrPayable * xdose);//nmrQtyReqd.Value

                if (vm.REPORTS.txtdiscount < 1m)
                    vm.REPORTS.txtdiscount = 1m;

                xqtysave = vm.REPORTS.txtdiscount;

                if (packqty > 0)
                {
                    vm.REPORTS.txtdiscount = Math.Round((vm.REPORTS.txtdiscount / packqty)); //nmrQtyReqd.Value

                    if (xqtysave < packqty)
                        vm.REPORTS.txtdiscount = 1m;
                }
            }

            if (vm.REPORTS.txtdiscount > 99m) //nmrQtyReqd.Value
            {
                vm.REPORTS.alertMessage = "System is Unable to calculate actual requirement from your Selections...'\n'" + 
                    "Pls Specify Qty Required Manually";

                vm.REPORTS.txtdiscount = 0; //nmrQtyReqd.Value
                //nmrQtyReqd.Focus();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void doconvertable(decimal Stkstrength, decimal stkper, decimal prescDose)
        {
            decimal xdose1;
            vm.REPORTS.chkbillregistration = true; //for isconvertable
            xdose1 = Stkstrength / stkper;
            //xdose1 = Convert.ToDecimal(comboDoseLiquid.Text) / xdose1;
            xdose1 = prescDose / xdose1;

            msmrfunc.mrGlobals.waitwindowtext = "Convertable - Strength :" + Stkstrength.ToString() +
                " mg Per " + stkper.ToString() + "ml   :=  " + xdose1.ToString() + "ml";
        }

        public JsonResult prescUnitIdFocusout(decimal Stkstrength, decimal stkper, decimal prescDose)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            doconvertable(Stkstrength, stkper, prescDose);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        decimal stockitemValidate(string stkcode, ref decimal qtyavailable, ref bool tocontinue, 
            ref bool preauthorization, ref bool iscapitated, string stkdesc, ref string txtdose, ref string unitid, 
            ref decimal cost, ref decimal strength, ref decimal stkper, ref decimal packqty, int autoremind_period,
            ref decimal purcost, string xstore, ref bool xwithConsumables)
        {
            DateTime dateNow = DateTime.Now;

            if (string.IsNullOrWhiteSpace(stkcode))
            {
                vm.REPORTS.alertMessage = "Valid Stock Definition must be selected...";
                tocontinue = false;
                return 0m;
            }

            string xstoresel = "";
            //   PleaseWaitForm pleaseWait = new PleaseWaitForm();
            if (!string.IsNullOrWhiteSpace(xstore))
                xstoresel = " and store = '" + xstore + "'";

            DataTable stock = Dataaccess.GetAnytable("", "SMS", 
                "select stock_qty, unit, sell, cost, strength, per, packqty, status, expirydate, withprescription " +
                "from stock where item = '" + stkcode + "'" + xstoresel, false);

            if (stock.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Undefined Stock Item....";
                tocontinue = false;
                return 0m;
            }

            bool xreturn = false;
            DateTime xpirydate = msmrfunc.mrGlobals.dtmin_date;
            txtdose = "";
            qtyavailable = 0m;

            foreach (DataRow row in stock.Rows)
            {
                qtyavailable += Convert.ToDecimal(row["stock_qty"]);
                unitid = row["unit"].ToString().Trim();
                cost = Convert.ToDecimal(row["sell"]);
                strength = Convert.ToDecimal(row["strength"]);
                stkper = Convert.ToDecimal(row["per"]);
                packqty = Convert.ToDecimal(row["packqty"]);
                purcost = Convert.ToDecimal(row["cost"]);

                if (Convert.ToBoolean(row["withprescription"]))
                    xwithConsumables = (bool)row["withprescription"];

                vm.REPORTS.chkSegmented = xwithConsumables;

                if (row["status"].ToString() == "D")
                {
                    vm.REPORTS.alertMessage = "This Item is no longer in use... has been flagged domant !";
                    xreturn = true;
                    break;
                }

                //CHECK FOR EXPIRY DATE
                xpirydate = Convert.ToDateTime(row["expirydate"]);
            }

            if (xpirydate.Year < dateNow.Year)
            {
                vm.REPORTS.alertMessage = "This Item Expired " + xpirydate;
            }
            //bissclass.expdtremind(xpirydate, autoremind_period, msmrfunc.mrGlobals.mta_start);

            if (xreturn)
            {
                tocontinue = false;
                return 0m;
            }

            return cost;
        }

        void GetItemDtls(string prescStockDisc, bool prescChkInpatient, string dtlastattend, string prescGroupCode, 
            string prescPatientNo, int reqAlertCount, IEnumerable<MR_DATA.REPORTS> tableList)
        {
            DateTime xdtlastattend;
            billchaindtl phbchain = Getbillchain(prescPatientNo, prescGroupCode);

            string[] requestalert = new string[5];

            DataTable dtGrpDrgChargRate = Dataaccess.GetAnytable("", "MR", "select reference,rate,oncost from GROUPDRGCHARGE", false);
            DataTable dt = Dataaccess.GetAnytable("", "MR",
                "SELECT glbatchno, ecgno, dischtime, othercharg, installed, pvtcode, dischtime, serial, fsh, FESTLEVPAS from mrcontrol order by recid", false);

            decimal presctnIntValidation = (decimal)dt.Rows[2]["glbatchno"];
            bool nhisgentariff = (bool)dt.Rows[5]["installed"]; //use gen tariff to nhis outpatient consult

            bool iscapitated = false, fee_for_service = false, preauthorization = false, alertforConsumables = false;
            bool tocontinue = true;
            bool foundit;

            if (string.IsNullOrWhiteSpace(dtlastattend)){
                xdtlastattend = DateTime.Now.Date;
            } else {
                xdtlastattend = Convert.ToDateTime(dtlastattend);
            }

            // lv_written = false;
            decimal Stkstrength = 0, stkper = 0;
            decimal qtyavailable = 0m, cost = 0m, rtnCost = 0m, purcost = 0m, packqty = 0m;
            string txtdose = "", unitid = "", pharmacyStore = "";
            int autoremind_period = 0;

            if (string.IsNullOrWhiteSpace(prescStockDisc) || string.IsNullOrWhiteSpace(vm.REPORTS.txtbranch))
                return;

            //anycode = "";
            string mstkcode = prescStockDisc; // cboDescription.SelectedValue.ToString();

            rtnCost = stockitemValidate(mstkcode, ref qtyavailable, ref tocontinue, ref preauthorization,
                ref iscapitated, vm.REPORTS.txtbranch, ref txtdose, ref unitid, ref cost, ref Stkstrength, ref stkper,
                ref packqty, autoremind_period, ref purcost, pharmacyStore, ref alertforConsumables);

            vm.REPORTS.nmrAgeFrom = Stkstrength;
            vm.REPORTS.nmrAgeTo = stkper;
            vm.REPORTS.nmrcurdebit = packqty;
            vm.REPORTS.txtbillonacct = txtdose;
            vm.REPORTS.chkCurrtAdmRev = iscapitated;
            vm.REPORTS.txtcurrency = mstkcode;

            if (!tocontinue)
            {
                //btnstock.Focus();
                vm.REPORTS.txtreference = ""; //txtStkCode.Text = "";
                vm.REPORTS.txtbranch = "";  //cboDescription.Text = "";

                //txtStkCode.Focus();
                return;
            }

            vm.REPORTS.lblbillspayable = packqty < 1 ? "1" : packqty.ToString(); //lblQtyPack.Text
            vm.REPORTS.lblbillspayable += "/pU";
            vm.REPORTS.nmrBalbf = qtyavailable; //nmrQtyavailable.Value
            vm.REPORTS.cbotype = unitid; //lblunitid.Text
            vm.REPORTS.txtdiscount = cost; //nmrunitcost.Value
            vm.REPORTS.nmrcurcredit = purcost; //nmrUnitPurchaseValue.Value
            decimal grpRate = 0m;

            //30-04-2021 - GIWA wants to check if patient got this drug in the past x period for opd only
            if (!prescChkInpatient && presctnIntValidation > 0 && xdtlastattend != DateTime.Now.Date && 
                DateTime.Now.Date.Subtract(xdtlastattend).Days <= presctnIntValidation && !msmrfunc.CheckHistoryPresciption(phbchain.GROUPCODE, 
                phbchain.PATIENTNO, mstkcode, presctnIntValidation))
            {
                vm.REPORTS.txtreference = ""; //txtStkCode.Text = "";
                vm.REPORTS.txtbranch = "";  //cboDescription.Text = "";
                // txtStkCode.Focus();
                return;
            }

            //27.04.2021 - implemented drug cost by billing category for corporate - GIWA
            if (phbchain.GROUPHTYPE == "C" && string.IsNullOrWhiteSpace(phbchain.HMOSERVTYPE)) //coproate
            {
                rtnCost = grpRate = msmrfunc.CheckGrpDrugChargePercentage(dtGrpDrgChargRate, phbchain.PATCATEG, vm.REPORTS.nmrcurcredit, vm.REPORTS.txtdiscount);
            }
            else if (phbchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(phbchain.HMOSERVTYPE)) //check hmo/nhis drgs define 
            {
                //cost = rtnamt;
                //31.10.2016 MUST CHECK RTNAMT - QTY AVAIL OR COST ?

                rtnCost = msmrfunc.CheckCorpPatientStkDefined(phbchain.GROUPHEAD, phbchain.GROUPHTYPE, phbchain.HMOSERVTYPE,
                    phbchain.GROUPCODE, nhisgentariff, (prescChkInpatient) ? true : false, ref cost, fee_for_service, mstkcode,
                    ref preauthorization, ref iscapitated, ref tocontinue, vm.REPORTS.txtbranch);

                vm.REPORTS.chkCurrtAdmRev = iscapitated;

                if (!tocontinue)
                {
                    vm.REPORTS.txtreference = ""; //txtStkCode.Text = "";
                    vm.REPORTS.txtbranch = "";  //cboDescription.Text = "";

                    //txtStkCode.Focus();
                    return;
                }
            }

            if (grpRate < 1 && rtnCost < 1m && phbchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(phbchain.PATCATEG))
            {
                rtnCost = msmrfunc.CheckCustClassforStkDefined(phbchain.PATCATEG, ref preauthorization, ref iscapitated, 
                    ref cost, ref tocontinue, mstkcode, vm.REPORTS.txtbranch);

                vm.REPORTS.chkCurrtAdmRev = iscapitated;

                if (!tocontinue)
                {
                    vm.REPORTS.txtreference = ""; //txtStkCode.Text = "";
                    vm.REPORTS.txtbranch = "";  //cboDescription.Text = "";

                    //txtStkCode.Focus();
                    return;
                }
            }

            if (grpRate < 1 && rtnCost < 1m) //stock differential tarif defined
            {
                rtnCost = msmrfunc.CheckStkCharge(phbchain.PATCATEG, ref cost, ref tocontinue, mstkcode);

                if (!tocontinue)
                {
                    vm.REPORTS.txtreference = ""; //txtStkCode.Text = "";
                    vm.REPORTS.txtbranch = "";  //cboDescription.Text = "";

                    //txtStkCode.Focus();
                    return;
                }
            }

            //anycode = txtStkCode.Text;
            vm.REPORTS.nmrMinBalance = rtnCost; //savedstksellamount
            vm.REPORTS.txtdiscount = rtnCost; //nmrunitcost.Value
            // the following segment is necessary to be able to know the doseage type to enable - Tab/Liquid - 04/06/2010

            if (string.IsNullOrWhiteSpace(vm.REPORTS.cbotype))
            {
                vm.REPORTS.ActRslt = vm.REPORTS.txtbranch.Trim() + " - Unable to Determine Drug Type; CLICK YES for SYRUP/LIQUID; NO for OTHERS";

                //if (result == DialogResult.Yes)
                //{
                //    vm.REPORTS.cbotype = "Tabs";
                //    // return;
                //}
                //else if (result == DialogResult.No)
                //{
                //    vm.REPORTS.cbotype = "Btl";
                //    // return;
                //}
            }

            foundit = false;

            vm.REPORTS.newrec = true;

            //we must scan through wgeta_ array to check if stock had been selected - we edit
            if (tableList != null)
            {
                foreach(var row in tableList)
                {
                    if (string.IsNullOrWhiteSpace(row.txtclinic)) {
                        continue;
                    }

                    if (row.txtclinic.Trim() == mstkcode.Trim())
                    {
                        foundit = true;
                        vm.REPORTS.txtstaffno = row.txtstaffno;
                        break;
                    }
                }

                if (foundit)
                {
                    //ServiceDuplicateOptions serviceuplicate = new ServiceDuplicateOptions();
                    //serviceuplicate.Closed += new EventHandler(serviceuplicate_Closed);
                    //serviceuplicate.ShowDialog();

                    vm.REPORTS.chkHMO = true;
                }
            }
            
            if (preauthorization && reqAlertCount < 5)
            {
                requestalert[reqAlertCount] = vm.REPORTS.txtbranch.Trim();
                reqAlertCount++;
            }

            vm.REPORTS.nmrAmountTo = reqAlertCount;
            return;
        }

        public JsonResult prescStockDiscFocusout(string prescStockDisc, bool prescChkInpatient, string dtlastattend,
            string prescGroupCode, string prescPatientNo, int reqAlertCount, IEnumerable<MR_DATA.REPORTS> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dt = Dataaccess.GetAnytable("", "SMS", 
                "select name from stock where item = '" + prescStockDisc + "'", false);

            if (dt.Rows.Count < 1)
            {
                msmrfunc.mrGlobals.auto_search_string = prescStockDisc;
                //btnstock.PerformClick();
            }
            else
            {
                vm.REPORTS.txtbranch = dt.Rows[0]["name"].ToString(); //txtDescription.Text
            }

            GetItemDtls(prescStockDisc, prescChkInpatient, dtlastattend, prescGroupCode, prescPatientNo, reqAlertCount, tableList);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        private void prescGetcontrolsettings()
        {
            string mdrugsonbill, nhisfac_code, nhisbillcode, mfacility, mdrugcode = "", pharmacyStore;
            Decimal fxtype, autoremind_period;
            bool nhisgentariff, mallowexp, isnhischarge, markupdrgbillonPercentage;
            decimal mdrgmarkup = 0m, medhistupdateallowed, presctnIntValidation;
            string[] requestalert = new string[5];
            bool inpatient = true;
            bool mtth = false;

            DataTable dt = Dataaccess.GetAnytable("", "MR", 
                "SELECT glbatchno, ecgno, dischtime, othercharg, installed, pvtcode, dischtime, serial, fsh, FESTLEVPAS from mrcontrol order by recid", false);

            // mdrugcode = dt.Rows[0]["mdrugcode"].ToString();
            pharmacyStore = dt.Rows[0]["serial"].ToString().Trim();
            //if (string.IsNullOrWhiteSpace(pharmacyStore))
            //    dtstock = Dataaccess.GetAnytable("", "SMS", "select DISTINCT name, item from stock order by name ", true);
            //else
            //    dtstock = Dataaccess.GetAnytable("", "SMS", "select name, item from stock where rtrim(store) = '" + pharmacyStore + "' order by name ", true);

            fxtype = (Decimal)dt.Rows[1]["glbatchno"];
            mdrgmarkup = (Decimal)dt.Rows[1]["ecgno"];
            markupdrgbillonPercentage = Convert.ToBoolean(dt.Rows[1]["fsh"]);

            mfacility = dt.Rows[2]["dischtime"].ToString();
            medhistupdateallowed = (Decimal)dt.Rows[2]["glbatchno"];

            mdrugsonbill = dt.Rows[3]["othercharg"].ToString();

            nhisgentariff = (bool)dt.Rows[5]["installed"]; //use gen tariff to nhis outpatient consult

            nhisfac_code = dt.Rows[8]["pvtcode"].ToString();
            nhisbillcode = dt.Rows[8]["dischtime"].ToString().Substring(0, 7);
            isnhischarge = (bool)dt.Rows[8]["othercharg"];
            presctnIntValidation = (decimal)dt.Rows[2]["glbatchno"];

            DataTable dtsms = Dataaccess.GetAnytable("", "SMS", 
                "SELECT allowexp, enqno from smcontrol where recid = '1'", false);

            mallowexp = (bool)dtsms.Rows[0]["allowexp"];
            autoremind_period = (Decimal)dtsms.Rows[0]["enqno"];

            if (inpatient)
            {
                vm.REPORTS.chkbillregistration = true; //chkinpatient.Checked
                vm.REPORTS.chkbyacctofficers = false; // chkinpatient.Enabled = btnConvert.Enabled = false;
                vm.REPORTS.cbotitle = "In-Patient Prescriptions Management"; //this.Text
                vm.REPORTS.chkByDateRange = (mtth) ? true : false; //chktth.Checked
                //btninpatprescdtl.Enabled = true;
            }

            DataTable dtmrsetup = Dataaccess.GetAnytable("", "MR", "Select drugcode from mrsetup order by facility", false);
            mdrugcode = dtmrsetup.Rows[0]["drugcode"].ToString();
        }

        void loadprevDefinitions(string msection, string revAdmReference, billchaindtl phbchain, string revTreatmentDate)
        {
            DateTime mtrans_date = Convert.ToDateTime(revTreatmentDate);

            if (msection == "4" || msection == "8") //DOCS AND PHARMACY ONLY
            {
                //ok
            }
            else
            {
                //msgeventtracker = "EXIT";
                vm.REPORTS.alertMessage = "Further Access Denied....";
                //btnclose.PerformClick();
                return;
            }

            if (string.IsNullOrWhiteSpace(revAdmReference))
            {
                //msgeventtracker = "EXIT";
                vm.REPORTS.alertMessage = "This Request has NO Consultation Reference... Cannot be Tracked!!!";
                vm.REPORTS.chkApplyFilter = false; //btnsave.Enabled
                //btnclose.PerformClick();
                return;
            }
           
            DataTable dt;

            if (vm.REPORTS.chkbillregistration){
                dt = DISPENSA.GetDISPENSA(phbchain.GROUPCODE, phbchain.PATIENTNO, mtrans_date, true);
            }
            else {
                dt = DISPENSA.GetDISPENSA(phbchain.GROUPCODE, phbchain.PATIENTNO, mtrans_date, false);
            }

            if (dt.Rows.Count < 1)
                return;

            vm.DISPENSAS = ErpFunc.ConvertDtToList<MR_DATA.DISPENSA>(dt);
        }


        public JsonResult revPrescriptionBtnClick(string revGroupCode, string revPatientNo, string msection,
            string revAdmReference, string revTreatmentDate)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Customer customer = new Customer();
            billchaindtl phbchain = new billchaindtl();

            Session["Inpatient"] = "N";
            prescGetcontrolsettings();
            //initcomboboxes();

            phbchain = Getbillchain(revPatientNo, revGroupCode);

            loadprevDefinitions(msection, revAdmReference, phbchain, revTreatmentDate);

            if (phbchain.GROUPHTYPE == "C")
                customer = Customer.GetCustomer(phbchain.GROUPHEAD);

            Session["inp2medhist"] = "";
            //onNmrDose = alertforConsumables = false;
            //get lastattendance date

            DateTime dtlastattend = DateTime.Now.Date;
            Medhrec medhrec = Medhrec.GetMEDHREC(phbchain.GROUPCODE, phbchain.PATIENTNO);

            if (medhrec != null)
                dtlastattend = medhrec.DATE5.Date; //.ToShortDateString() + "  @ " + medhrec.DATE5.ToShortTimeString();

            if (phbchain.GROUPHTYPE == "C") //&& customer.HMO ) ALL CORPORATE
                vm.REPORTS.chkADVCorporate = true; //chkPrivateAcct.Enabled

            vm.REPORTS.dtregistered = dtlastattend;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult diabSubmitBtnClick(IEnumerable<MR_DATA.REPORTS> tableList, string obAdmReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            ObservationCharts formObject = new ObservationCharts(vm, woperato);
            vm.REPORTS = formObject.btnSubmit_Db_Click(tableList, obAdmReference);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult fluidSubmitBtnClick(IEnumerable<MR_DATA.REPORTS> tableList, string obAdmReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            ObservationCharts formObject = new ObservationCharts(vm, woperato);
            vm.REPORTS = formObject.btnSubmit_fluid_Click(tableList, obAdmReference);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult intenSubmitBtnClick(IEnumerable<MR_DATA.REPORTS> tableList, string obAdmReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            ObservationCharts formObject = new ObservationCharts(vm, woperato);
            vm.REPORTS = formObject.btnSubmit_bp_Click(tableList, obAdmReference);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult obAdmReferenceFocusout(string obAdmReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrec = new Admrecs();

            if (obAdmReference.Substring(0, 1) != "A")
            {
                if (bissclass.IsDigitsOnly(obAdmReference.Trim()))
                {
                    obAdmReference = bissclass.autonumconfig(obAdmReference, true, "A", "999999999");
                    vm.REPORTS.txtreference = obAdmReference;
                }
            }

            //check if reference exist
            //anycode = "";
            admrec = Admrecs.GetADMRECS(obAdmReference);

            if (admrec == null) //new defintion
            {
                vm.REPORTS.alertMessage = "Invalid Admission Reference...";
                //txtreference.Text = "";
                //txtreference.Focus();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            if (!displayadmrecs(admrec, obAdmReference))
            {
                vm.REPORTS.txtreference = "";
                //txtreference.Focus();
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        void obChartDisplaydetails(string revAdmReference)
        {
            vm.MRB22S = ErpFunc.RsGet<MR_DATA.MRB22>("MR_DATA",
               "select * from mrb22 where reference = '" + revAdmReference + "'");

            vm.MRB23S = ErpFunc.RsGet<MR_DATA.MRB23>("MR_DATA",
               "select * from mrb23 where reference = '" + revAdmReference + "'");

            vm.MRB24S = ErpFunc.RsGet<MR_DATA.MRB24>("MR_DATA",
               "select * from mrb24 where reference = '" + revAdmReference + "'");
        }

        bool displayadmrecs(Admrecs admrecs, string revAdmReference)
        {
            billchaindtl bchain = new billchaindtl();

            DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name ", true),
                dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES order by name", true);

            vm.REPORTS.txtgroupcode = admrecs.GROUPCODE;
            vm.REPORTS.txtpatientno = admrecs.PATIENTNO;
            vm.REPORTS.TXTPATIENTNAME = admrecs.NAME;
            vm.REPORTS.combFacility = bissclass.combodisplayitemCodeName("type_code", admrecs.FACILITY, dtfacility, "name"); //lblfaciitySu.Text
            vm.REPORTS.txtRoom = admrecs.ROOM;
            vm.REPORTS.txtBed = admrecs.BED;
            vm.REPORTS.diagnosis = bissclass.combodisplayitemCodeName("type_code", admrecs.DIAGNOSIS, dtdiag, "name");

            if (vm.REPORTS.diagnosis.Trim() != admrecs.DIAGNOSIS_ALL && !string.IsNullOrWhiteSpace(admrecs.DIAGNOSIS_ALL))
                vm.REPORTS.diagnosis = admrecs.DIAGNOSIS_ALL;

            vm.REPORTS.REPORT_TYPE1 = admrecs.ADM_DATE.ToShortDateString();
            vm.REPORTS.REPORT_TYPE2 = admrecs.DISCHARGE;
            DateTime xdischarge = string.IsNullOrWhiteSpace(admrecs.DISCHARGE) ? DateTime.Now.Date : Convert.ToDateTime(admrecs.DISCHARGE);
            vm.REPORTS.lblStaffNumber = (xdischarge - admrecs.ADM_DATE).Days.ToString() + " day(s)";

            bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);
            vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
            vm.REPORTS.txtstaffno = patientprofile(bchain);

            vm.REPORTS.txtgrouphead = msmrfunc.GETGroupheadname(bchain.GHGROUPCODE, bchain.GROUPHEAD, bchain.GROUPHTYPE);

            if (vm.REPORTS.txtgrouphead.Trim() == "Abort")
            {
                return false;
            }

            obChartDisplaydetails(revAdmReference);

            if (admrecs.DISCHARGE != "")
            {
                vm.REPORTS.ActRslt = "This Patient has been discharged on " + admrecs.DISCHARGE + "," + admrecs.NAME;
                return false;
            }

            return true;
        }

        public JsonResult observChartOnLoad(string revAdmReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrec = new Admrecs();
            billchaindtl bchain = new billchaindtl();

            //check if reference exist
            admrec = Admrecs.GetADMRECS(revAdmReference);

            if (string.IsNullOrWhiteSpace(revAdmReference) || admrec == null)
            {
                vm.REPORTS.alertMessage = "Patient Record must be Selected...";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(revAdmReference) && !string.IsNullOrWhiteSpace(admrec.REFERENCE))
            {
                displayadmrecs(admrec, revAdmReference);
                //edtprofile.Focus();
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult defSubmitBtnClicked(IEnumerable<MR_DATA.MRB20> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            TemplateGrpDetails formObject = new TemplateGrpDetails(vm);
            vm.REPORTS = formObject.btnSubmit_Click(tableList);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult defineGrpDetailsOnLoad()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.TEMPLATEGRPS = ErpFunc.RsGet<MR_DATA.TEMPLATEGRP>("MR_DATA", "select * from TEMPLATEGRP");

            if (vm.TEMPLATEGRPS.Count() < 1)
            {
                return Json(vm.TEMPLATEGRPS, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.TEMPLATEGRPS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult templateDeleteBtnClick(int tableRecID)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            string xstr = "delete from MRB20 where recid = '" + tableRecID.ToString() + "'";
            bissclass.UpdateRecords(xstr, "MR");
            vm.REPORTS.alertMessage = "Item deleted from database..";

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult templateSubmitBtnClick(int screenId, string tempDoctor, string tempAreaOfSpec, IEnumerable<MR_DATA.MRB20> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;

            DocsTemplates formObject = new DocsTemplates(vm, woperato);
            vm.REPORTS = formObject.SaveDetails(screenId, tempDoctor, tempAreaOfSpec, tableList);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult tempOnLoad()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.MRB20S = ErpFunc.RsGet<MR_DATA.MRB20>("MR_DATA", "select * from mrb20");

            if(vm.MRB20S.Count() < 1)
            {
                return Json(vm.MRB20S, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.MRB20S, JsonRequestBehavior.AllowGet);
        }

        public JsonResult revGetHistoryBtnClick(string revHistDateFrom, string revHistDateTo, bool revChkQueryBackup, 
            string revAdmReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();

            admrecs = Admrecs.GetADMRECS(revAdmReference);
            bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);
            
            var historyDateTo = Convert.ToDateTime(revHistDateTo);
            var historyDateFrom = Convert.ToDateTime(revHistDateFrom);

            if ((historyDateTo - historyDateFrom).TotalDays > 190)
            {
                vm.REPORTS.alertMessage = "Medical History Query Date Range is too wide...";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            medhistryRev(false, admrecs, bchain, historyDateFrom, historyDateTo, revChkQueryBackup);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult revSaveBtnClick(MR_DATA.REPORTS dataList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperato = Request.Cookies["mrName"].Value;
            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();

            vm.REPORTS = dataList;

            admrecs = Admrecs.GetADMRECS(vm.REPORTS.txtreference);
            bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);

            if (string.IsNullOrWhiteSpace(vm.REPORTS.txtreference) || admrecs == null || admrecs.PATIENTNO == null)
            {
                vm.REPORTS.alertMessage = "Patient Record must be Selected...";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
              
            Admissions formObject = new Admissions(vm, woperato);
            vm.REPORTS = formObject.saveRevDetails(admrecs, bchain);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void ExitReviewDate(DateTime treatmentDate, string revAdmReference, Admrecs admrecs, billchaindtl bchain,
            string msection, string woperator, string revDiagnosis, string revDateOfAdm)
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR",
               "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            int mdoc_seclevel = (Int32)dt.Rows[0]["wseclevel"];

            string xaccesscode = "", retrieved_note = "";

            if (treatmentDate.Date > DateTime.Now.Date || treatmentDate.Date < admrecs.ADM_DATE)
            {
                vm.REPORTS.alertMessage = "Invalid Date Specification...";
                //vm.REPORTS.REPORT_TYPE1 = string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date); //dtTreatmentDateRev.Value
                //dtTreatmentDateRev.Focus();
                return;
            }

            if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO) || string.IsNullOrWhiteSpace(revAdmReference))
                return;

            vm.REPORTS.newrec = true; //newrecRev
            vm.REPORTS.REPORT_TYPE2 = DateTime.Now.ToShortTimeString(); // txtTimeTrtmtRev.Text

            //medhistryRev(false);
            MedHist medhistRev = MedHist.GetMEDHIST(bchain.GROUPCODE, bchain.PATIENTNO, revAdmReference,
                false, true, treatmentDate.Date, "DESC");

            if (medhistRev != null)
            {
                vm.REPORTS.newrec = false;

                if (medhistRev.PROTECTED > 0 && mdoc_seclevel < medhistRev.PROTECTED)
                {
                    vm.REPORTS.ActRslt = "Access to Secured Medical History Records Denied...";
                    //txtreferenceRev.Focus();
                    return;
                }

                if (medhistRev.PROTECTED > 0 && medhistRev.ENCHRYPTED)
                {
                    POPREAD popread = new POPREAD("Medical History Enchrypted Access Code", 
                        "This Medical History is Enchrypted... Enter Access Code :", ref xaccesscode,
                        false, false, "", "", "", false, "", "");
                    //popread.Closed += new EventHandler(popread_Closed);
                    //popread.ShowDialog();
                }

                vm.REPORTS.edtallergies = retrieved_note = medhistRev.COMMENTS.Trim(); // txtcurrentrecs.Text
                vm.REPORTS.edtallergies = vm.REPORTS.edtallergies + "\r\n";
                vm.REPORTS.chkADVCorporate = (medhistRev.PROTECTED > 0) ? true : false; // chkProtectRev.Checked
                vm.REPORTS.chkApplyFilter = (medhistRev.ENCHRYPTED) ? true : false; //chkEncryptRev.Checked
            }
            else if (msection == "4")
            {
                vm.REPORTS.edtallergies = "==> Admission Update BY : " + woperator.Trim() + " @ " + 
                    DateTime.Now.ToString("HH:mmtt ") + " Date Admitted : " + revDateOfAdm + " : Diag : " +
                    revDiagnosis + "\r\n\r\n";
            }

            if (msection == "4")
                vm.REPORTS.chkBroughtForward = true; // btnPrescriptionRev.Enabled = btnInvProcRev.Enabled = btnTTHRev.Enabled = true;
            else
            {
                //txtcurrentrecs.ReadOnly = true;
                vm.REPORTS.chkByBranch = true; //btnSaveRev.Enabled = true;
            }

            //timer_Adm.Enabled = timer_Doc.Enabled = false;
            vm.REPORTS.txtclinic = vm.REPORTS.edtallergies; //savedCaseNote
            //txtcurrentrecs.Focus();
            return;
        }

        public JsonResult revTreatmentDateFocusout(string revTreatmentDate, string revAdmReference, string msection,
            string woperator, string revDiagnosis, string revDateOfAdm)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();

            admrecs = Admrecs.GetADMRECS(revAdmReference);
            bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);

            DateTime treatmentDate = Convert.ToDateTime(revTreatmentDate);

            ExitReviewDate(treatmentDate, revAdmReference, admrecs, bchain, msection, woperator, revDiagnosis, revDateOfAdm);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void ImageInfo(Admrecs admrecs)
        {
            vm.MEDHPICPROPS = ErpFunc.RsGet<MR_DATA.MEDHPIC>("MR_DATA",
                "select TRANS_DATE,PIC1,NOTE1, PIC2, NOTE2, PIC3, NOTE3, PIC4, NOTE4, PIC5, NOTE5, TOTPIC, FACILITY1, " +
                "FACILITY2, FACILITY3, FACILITY4, FACILITY5, pdffile1, pdffile2, pdffile3, pdffile4, pdffile5 from medhpic where ltrim(rtrim(groupcode)) = '" +
                admrecs.GROUPCODE.Trim() + "' and ltrim(rtrim(patientno)) = '" + admrecs.PATIENTNO.Trim() + "'");

        }

        DataTable GetChainedPatNo(billchaindtl bc)
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", 
                "select chainedgc,chainedpatno from MEDHISTCHAIN where groupcode = '" + bc.GROUPCODE + 
                "' and patientno = '" + bc.PATIENTNO + "' ORDER BY chainedpatno", false);

            return dt;
        }

        string GetPrescriptnDetails(string xgc, string xpat, DateTime xdate)
        {
            bool ftime = true;
            string tmpstring = "SELECT sp_inst, itemno, stk_desc, unit, qty_pr, qty_gv, cost, cdose, dose, interval, duration, cinterval, cduration, rx from dispensa where ltrim(rtrim(groupcode)) = '" + xgc.Trim() + "' and ltrim(rtrim(patientno)) = '" + xpat + "' AND TRANS_DATE >= '" + xdate.ToShortDateString() + "' and trans_date <= '" + xdate.ToShortDateString() + " 23:59:59:999' ";
            DataTable dtdrugs = Dataaccess.GetAnytable("", "MR", tmpstring, false);

            string rxspecialinstructions = "", xdtl = "";
            foreach (DataRow row in dtdrugs.Rows)
            {
                if (ftime)
                {
                    xdtl += "\r\n Prescriptions : \r\n";
                    //"S/N Drugs Details.......................... Unit Presc'd......  Given........ Cost...... D/ I /D" + "\r\n"; //   ..ForeColor = System.Drawing.Color.DarkGray;  //+'\n';

                    rxspecialinstructions = row["sp_inst"].ToString();
                }
                ftime = false;
                // tmpstring = row["cdose"].ToString() == "" ? row["dose"].ToString() + "x" + row["interval"].ToString() + 'x' + row["duration"].ToString() : row["cdose"].ToString() + "x" + row["cinterval"].ToString() + 'x' + row["cduration"].ToString();
                //xdtl += row["itemno"].ToString() + ".... " + row["stk_desc"].ToString() + "..... " + row["unit"].ToString() + ".... " + row["qty_pr"].ToString() + "......  " + row["qty_gv"].ToString() + ".......... " + row["cost"].ToString() + " " + tmpstring + "  (" + row["cost"].ToString() + ")" + "\r\n";
                tmpstring = row["cdose"].ToString() == "" ? row["dose"].ToString() + " x " + row["interval"].ToString() + " x " + row["duration"].ToString() : row["cdose"].ToString() + " x " + row["cinterval"].ToString() + 'x' + row["cduration"].ToString();
                xdtl += row["itemno"].ToString() + ". " + row["qty_pr"].ToString() + " " + row["unit"].ToString() + " " + row["stk_desc"].ToString() + "   Gv[" + row["qty_gv"].ToString() + "] @ " + row["cost"].ToString() + "  " + tmpstring + "\r\n"; //"  (" + row["cost"].ToString() + ")" + 
            }
            if (rxspecialinstructions != "")
                xdtl += rxspecialinstructions + "\r\n";
            return xdtl;
        }

        string GetChainedRecords(billchaindtl bc, DataTable xdtchained, DateTime xdate)
        {
            string chainedselect = "";
            string dtl = "";
            DataRow row;
            DataTable chainedmedhist = new DataTable();
            for (int i = 0; i < xdtchained.Rows.Count; i++)
            {
                row = xdtchained.Rows[i];
                chainedselect += "select comments, protected, enchrypted, trans_date, name, ctime, groupcode, patientno, doctor from medhist where groupcode = '" + 
                    row["chainedgc"].ToString() + "' and patientno = '" + row["chainedpatno"].ToString() + "'"; // and trans_date = '" + xdate.ToShortDateString() + "'";
                                                                                                                                                                                                                                                                //28.01.2020 querry by date limits ability of program to return med hist of chained accounts
                if (xdtchained.Rows.Count > i + 1)
                    chainedselect += " UNION ";
            }
            if (chainedselect == "")
                return dtl;

            chainedselect += " order by trans_date"; // DESC";
            chainedmedhist = Dataaccess.GetAnytable("", "MR", chainedselect, false);
            if (chainedmedhist.Rows.Count < 1)
                return dtl;
            //	bool ftime = true;
            string xname = "";
            foreach (DataRow xrow in chainedmedhist.Rows)
            {
                if (xrow["name"].ToString() != xname)
                {
                    dtl += "\r\n";
                    dtl += " *** ==>> CHAINED MED.HISTORY FROM > " + xrow["groupcode"].ToString().Trim() + ":" + xrow["patientno"].ToString() + " - " + xrow["name"].ToString().Trim() + "  S T A R T  \r\n";
                    xname = xrow["name"].ToString();
                }
                dtl += " *** DATE : " + Convert.ToDateTime(xrow["trans_date"]).ToShortDateString() + " ***";

                dtl += "\r\n";
                //	ftime = false;
                dtl += xrow["COMMENTS"].ToString().Trim() + "\r\n";
                dtl += GetPrescriptnDetails(xrow["GROUPCODE"].ToString(), xrow["PATIENTNO"].ToString(), Convert.ToDateTime(xrow["trans_date"]).Date);
                dtl += "\r\n\r\n";
            }
            dtl += " *** END-OF-CHAINED MED. HISTORY"; // FOR : " + xdate.ToShortDateString();

            return dtl;
        }

        public string GetMEDHISTCaseNotes(string GroupCodeID, string PatientID, bool ByPatient, bool ByDate, DateTime xdatefrom, DateTime xdateto, billchaindtl bchain, string xsortorder)
        {
            MedHist medhist = new MedHist();
            string details = "";
            DataTable dtchained = new DataTable();
            //  DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = (ByDate) ? "MEDHIST_GetByDateCaseNote" : "MEDHIST_GetByPatientCaseNote";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Groupcode", GroupCodeID);
            selectCommand.Parameters.AddWithValue("@Patientno", PatientID);
            selectCommand.Parameters.AddWithValue("@datefrom", xdatefrom);
            selectCommand.Parameters.AddWithValue("@dateto", xdateto);
            selectCommand.Parameters.AddWithValue("@sortorder", xsortorder);
            try
            {
                string tmpstring = "";
                bool ftime = true;
                //   DataTable dtdrugs = new DataTable();
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();

                //   List<Trigger> TriggerValues = new List<Trigger>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        medhist.PATIENTNO = reader["patientno"].ToString();
                        medhist.NAME = reader["name"].ToString();
                        medhist.TRANS_DATE = (DateTime)reader["trans_date"];
                        medhist.CTIME = reader["ctime"].ToString();
                        medhist.COMMENTS = reader["comments"].ToString();
                        medhist.GROUPCODE = reader["groupcode"].ToString();
                        medhist.DOCTOR = (reader["doctor"] == DBNull.Value) ? "" : reader["doctor"].ToString();
                        medhist.PROTECTED = (Decimal)reader["protected"];
                        medhist.ENCHRYPTED = (bool)reader["ENCHRYPTED"];
                        //update variable
                        details += medhist.TRANS_DATE.ToShortDateString() + " : @ " + medhist.CTIME + " :- " + medhist.DOCTOR.Trim() + "  (" + bissclass.seeksay("select name from doctors where rtrim(reference) = '" + medhist.DOCTOR.Trim() + "'", "MR", "name") + ") \r\n";
                        //17-08-2013 for chained med history
                        /*     if (bchain.MEDHISTORYCHAINED && dtrow["patientno"].ToString() != bchain.PATIENTNO)
							 {
								 txtPrev_records.Text += " ==>> CHAINED MED.HISTORY FROM " + dtrow["groupcode"].ToString().Trim() + ":" + dtrow["patientno"].ToString() + " - " + dtrow["name"].ToString() + "\n";
							 }*/
                        //check for protect and enchryption - 14/10/2011,21.09.2016
                        if (medhist.PROTECTED > 0 && msmrfunc.mrGlobals.nwseclevel < medhist.PROTECTED)
                        {
                            tmpstring = medhist.ENCHRYPTED ? "and ENCHRYPTED " : "";
                            details += " *** PROTECTED " + tmpstring + " PATIENT MEDICAL HISTORY ***" + "\r\n";
                            tmpstring += Enumerable.Repeat("-", 144);
                            tmpstring += "\r\n";
                        }
                        else
                        {
                            details += medhist.COMMENTS.Trim() + "\r\n";

                            //get drugs from prescription
                            details += GetPrescriptnDetails(medhist.GROUPCODE, medhist.PATIENTNO, medhist.TRANS_DATE);

                            //check for chained medic history for this date
                            if (bchain != null && bchain.MEDHISTORYCHAINED && ftime) //16.03.2019
                            {
                                // if (ftime) //get chained recoreds
                                // {
                                dtchained = GetChainedPatNo(bchain); //gets chained patient numbers
                                details += GetChainedRecords(bchain, dtchained, medhist.TRANS_DATE); //gets medi recs 
                                                                                                     // }
                                ftime = false;
                            }
                        }
                        details += string.Concat(Enumerable.Repeat("-", 144));
                        details += "\r\n";
                    }
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                //throw ex;
   //             MessageBox.Show("" + ex, "Patient Medical Details ", MessageBoxButtons.OK,
   //MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return details;
        }

        void medhistryRev(bool allrecords, Admrecs admrecs, billchaindtl bchain, DateTime dateFrom, 
            DateTime dateTo, bool revChkQueryBackup)
        {
            DateTime startdate, enddate;

            if (allrecords)
            {
                startdate = admrecs.ADM_DATE.AddDays(-30);
                enddate = DateTime.Now.Date;// (DateTime.Now.Date - admrecs.ADM_DATE).TotalDays > 7 ? DateTime.Now.Date.AddDays(-7) : admrecs.ADM_DATE;
            }
            else
            {
                startdate = dateFrom.Date;
                enddate = dateTo.Date;
            }

            DateTime historyDateFrom = startdate; //dtHistoryDatefrom.Value
            DateTime historyDateTo = enddate; //dtHistoryDateto.Value

            vm.REPORTS.REPORT_TYPE3 = string.Format("{0:yyyy-MM-dd}", historyDateFrom);
            vm.REPORTS.REPORT_TYPE4 = string.Format("{0:yyyy-MM-dd}", historyDateTo);

            if (revChkQueryBackup)
            {
                //this.txtPreviousRecRev.BackColor = System.Drawing.Color.White;
                //this.txtPreviousRecRev.ForeColor = System.Drawing.Color.Blue;
                vm.REPORTS.REPORT_TYPE5 = MedHist.GetMEDHISTBKUPCaseNotes(bchain.GROUPCODE, bchain.PATIENTNO, 
                    historyDateFrom, historyDateTo); //txtPreviousRecRev.Text
                //LightGray 
            }
            else
            {
                //this.txtPreviousRecRev.BackColor = System.Drawing.Color.LightGray;
                //this.txtPreviousRecRev.ForeColor = System.Drawing.Color.Black;
                vm.REPORTS.REPORT_TYPE5 = GetMEDHISTCaseNotes(bchain.GROUPCODE, bchain.PATIENTNO, 
                    false, true, startdate, enddate, bchain, "DESC"); //txtPreviousRecRev.Text
            }
            return;
        }

        public billchaindtl Getbillchain(string PatientID, string groupcode)
        {
            billchaindtl bchain = new billchaindtl();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = string.IsNullOrWhiteSpace(groupcode) ? "BILLCHAIN_checkpatno" : "BILLCHAIN_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@GroupCode", groupcode);
            selectCommand.Parameters.AddWithValue("@PatientNo", PatientID);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    bchain.GROUPCODE = reader["groupcode"].ToString();
                    bchain.PATIENTNO = reader["patientno"].ToString();
                    bchain.GROUPHEAD = reader["grouphead"].ToString();
                    bchain.NAME = reader["name"].ToString();
                    bchain.REG_DATE = (DateTime)reader["reg_date"];
                    bchain.POSTED = (bool)reader["posted"];
                    bchain.POST_DATE = (DateTime)reader["post_date"];
                    bchain.GROUPHTYPE = reader["grouphtype"].ToString();
                    bchain.STAFFNO = reader["staffno"].ToString();
                    bchain.RELATIONSH = reader["relationsh"].ToString();
                    bchain.SECTION = reader["section"].ToString();
                    bchain.DEPARTMENT = reader["department"].ToString();
                    bchain.CUR_DB = (Decimal)reader["cur_db"];
                    bchain.STATUS = reader["status"].ToString();
                    bchain.SEX = reader["sex"].ToString();
                    bchain.M_STATUS = reader["m_status"].ToString();
                    bchain.BIRTHDATE = (DateTime)reader["birthdate"];
                    bchain.CUMVISITS = (Decimal)reader["cumvisits"];
                    bchain.HMOCODE = reader["hmocode"].ToString();
                    bchain.PATCATEG = reader["patcateg"].ToString();
                    bchain.RESIDENCE = reader["residence"].ToString();
                    bchain.GHGROUPCODE = reader["ghgroupcode"].ToString();
                    bchain.HMOSERVTYPE = reader["hmoservtype"].ToString();
                    bchain.BILLONACCT = reader["billonacct"].ToString();
                    bchain.CURRENCY = reader["currency"].ToString();
                    bchain.OPERATOR = reader["operator"].ToString();
                    bchain.DTIME = (DateTime)reader["dtime"];
                    bchain.EXPIRYDATE = (DateTime)reader["expirydate"];
                    bchain.ASTNOTES = (bool)reader["astnotes"];
                    bchain.CLINIC = reader["clinic"].ToString();
                    bchain.PHONE = reader["phone"].ToString();
                    bchain.EMAIL = reader["email"].ToString();
                    bchain.PICLOCATION = reader["piclocation"].ToString();
                    bchain.SURNAME = reader["surname"].ToString();
                    bchain.OTHERNAMES = reader["othernames"].ToString();
                    bchain.TITLE = reader["title"].ToString();
                    bchain.SPNOTES = reader["spnotes"].ToString();
                    bchain.MEDNOTES = reader["mednotes"].ToString();
                    bchain.MEDHISTORYCHAINED = (bool)reader["medhistorychained"];
                    bchain.PATIENTNO_PRINCIPAL = reader["patientno_principal"].ToString();
                }
                else
                {
                    connection.Close();
                    return null;

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //throw ex;
                //             MessageBox.Show("" + ex, "Get Patient Details ", MessageBoxButtons.OK,
                //MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return bchain;
        }


        public JsonResult revAdmReferenceFocusout(string revAdmReference, string woperator, string msection, 
            string revHistDateFrom, string revHistDateTo, bool revChkQueryBackup)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();
            DataTable dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES order by name", true);
            DataTable dt = Dataaccess.GetAnytable("", "MR",
               "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            int mdoc_seclevel = (Int32)dt.Rows[0]["wseclevel"];

            DateTime dateFrom = Convert.ToDateTime(revHistDateFrom);
            DateTime dateTo = Convert.ToDateTime(revHistDateTo);

            if (revAdmReference.Substring(0, 1) != "A")
            {
                vm.REPORTS.txtreference = bissclass.autonumconfig(revAdmReference, true, "A", "999999999");
            }

            //check if reference exist
            //AnyCode = Anycode1 = "";
            admrecs = Admrecs.GetADMRECS(revAdmReference);

            if (admrecs == null) //new defintion
            {
                vm.REPORTS.alertMessage = "Invalid Admission Reference...";
                vm.REPORTS.txtreference = "";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.mreference = vm.REPORTS.txtreference;
            vm.REPORTS.mgrouphtype = admrecs.GROUPHTYPE;
            vm.REPORTS.txtpatientno = admrecs.PATIENTNO;
            vm.REPORTS.txtgroupcode = admrecs.GROUPCODE;
            vm.REPORTS.lblbillspayable = admrecs.FACILITY; // lblfacilityRev.Text
            vm.REPORTS.TXTPATIENTNAME = admrecs.NAME;
            vm.REPORTS.txtRoom = admrecs.ROOM;
            vm.REPORTS.txtBed = admrecs.BED;
            vm.REPORTS.diagnosis = bissclass.combodisplayitemCodeName("type_code", admrecs.DIAGNOSIS, dtdiag, "name");

            if (vm.REPORTS.diagnosis.Trim() != admrecs.DIAGNOSIS_ALL && !string.IsNullOrWhiteSpace(admrecs.DIAGNOSIS_ALL))
                vm.REPORTS.diagnosis = admrecs.DIAGNOSIS_ALL.Trim();

            vm.REPORTS.REPORT_TYPE1 = admrecs.ADM_DATE.ToShortDateString(); //txtadm_dateRev.Text
            vm.REPORTS.REPORT_TYPE2 = admrecs.DISCHARGE; //txtDischargeDateRev.Text
            DateTime xdischarge = string.IsNullOrWhiteSpace(admrecs.DISCHARGE) ? DateTime.Now.Date : Convert.ToDateTime(admrecs.DISCHARGE);
            vm.REPORTS.lblStaffNumber = xdischarge.Subtract(admrecs.ADM_DATE).TotalDays.ToString() + " day(s)"; //lblLenghtofStay_Rev.Text
            vm.REPORTS.mgrouphead = admrecs.GROUPHEAD;
            
            bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);

            if (bchain == null)
            {
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
            vm.REPORTS.txtsurname = patientprofile(bchain); //edtprofileRev.Text
            vm.REPORTS.txtgrouphead = getgrouphead(bchain.GROUPHEAD, bchain.GHGROUPCODE, bchain.GROUPHTYPE);

            if (vm.REPORTS.txtgrouphead.Trim() == "Abort")
            {
                //txtreferenceRev.Focus();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            //displayPatientPicture(this.pictureBox_Rev, bchain.PICLOCATION);
            if (admrecs.DISCHARGE != "")
            {
                vm.REPORTS.ActRslt = "This Patient has been discharged on " + admrecs.DISCHARGE + "," + admrecs.NAME;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.REPORTS.chkADVCorporate = true; //btnSaveRev.Enabled
                vm.REPORTS.chkApplyFilter = true; // btnObservationChart.Enabled = btnFluidChatRev.Enabled = btnDiabeticChatRev.Enabled = btnInPrescRev.Enabled = true;

                if (msection == "4" && mdoc_seclevel >= 5)
                    vm.REPORTS.chkByBranch = true; // chkProtectRev.Enabled = chkEncryptRev.Enabled = true;

                medhistryRev(true, admrecs, bchain, dateFrom, dateTo, revChkQueryBackup);
                ImageInfo(admrecs);

                vm.REPORTS.txtdiscount = getAccummulatedCharge(revAdmReference, admrecs.ACAMT); //nmrCummAmt_Rev.Value
                //dtTreatmentDateRev.Focus();
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        //Service Update
        public JsonResult xSaveNoteBtnClick(string xSpecInstText, string xMedNoteText, string xmgroupcode, string xmpatientno)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            frmSpInstnMednotes formObject = new frmSpInstnMednotes(vm);
            vm.REPORTS = formObject.btnSubmit_Click(xSpecInstText, xMedNoteText, xmgroupcode, xmpatientno);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult trtmentInpPrescAddBtnClicked(string phProcess, string given)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            bool iscostcentrestores;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select facilauto from mrcontrol where recid = '5'", false);

            iscostcentrestores = (bool)dt.Rows[0]["facilauto"];

            if (iscostcentrestores && phProcess == "NO" && given == "0")
            {
                vm.REPORTS.alertMessage = "This Item has not been transferred from Pharmacy... CAN'T ADD TO CHART UNTIL PHARMACY HAS WORKED ON IT. TKS!!!";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.chkADVCorporate = true; //panel_TF.Visible

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult trtmentInpPrescBtnClicked(string trtmentReference)
        {
            vm.INPDISPENSAS = ErpFunc.RsGet<MR_DATA.INPDISPENSA>("MR_DATA",
                "select trans_date, itemno, stk_item, stk_desc, qty_pr, cumgv, unit, dose, duration, cdose, cinterval, " + 
                "cduration, unitcost, cost, rx, doctor, stkbal, recid, posted, qty_gv, phtransferred, sp_inst, interval " + 
                "from inpdispensa where reference = '" + trtmentReference + "' and posted = '0' order by trans_date");

            return Json(vm.INPDISPENSAS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult trtmentRemoveBtnClick(decimal stkBalance, string recId)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (stkBalance > 0)
            {
                string updatestring = "DELETE from duenext WHERE RECID = '" + recId + "' ";

                bissclass.UpdateRecords(updatestring, "MR");
            }

            //dataGridView1.Rows.RemoveAt(recno);
            vm.REPORTS.alertMessage = "Record Deleted...";
            //recno = 0;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult trtmentSubmitBtnClick(IEnumerable<MR_DATA.REPORTS> tableList, string trtmentReference, string trtmentUnit)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            vm.REPORTS.txtreference = trtmentReference;
            vm.REPORTS.comhmoservgrp = trtmentUnit; 
            //string woperato = Request.Cookies["mrName"].Value;

            frmTreatmentChart formObject = new frmTreatmentChart(vm);
            vm.REPORTS = formObject.btnSubmit_Click(tableList);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void writeAdmdetails(bool xnewrec, string xreference, DateTime xdate, string xtime, string xprocess, 
            string masterprocess, string stkitemcode, string xdescription, string unit, decimal qty, decimal xamount, 
            bool posted, DateTime postdate, string xoperator, DateTime opdatetime, string groupcode, string patientno, 
            string doctor, string facility, int recid, string xstore)
        {
            // DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (xnewrec) ? "Admdetai_Add" : "Admdetai_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", xreference);
            insertCommand.Parameters.AddWithValue("@trans_date", xdate);
            insertCommand.Parameters.AddWithValue("@time", xtime);
            insertCommand.Parameters.AddWithValue("@mastprocess", masterprocess);
            insertCommand.Parameters.AddWithValue("@process", xprocess);
            insertCommand.Parameters.AddWithValue("@stk_item", stkitemcode);
            insertCommand.Parameters.AddWithValue("@description", xdescription);
            insertCommand.Parameters.AddWithValue("@unit", unit);
            insertCommand.Parameters.AddWithValue("@qty", qty);
            insertCommand.Parameters.AddWithValue("@amount", xamount);
            insertCommand.Parameters.AddWithValue("@posted", posted);
            insertCommand.Parameters.AddWithValue("@post_date", postdate);
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@op_time", opdatetime);
            insertCommand.Parameters.AddWithValue("@groupcode", groupcode);
            insertCommand.Parameters.AddWithValue("@patientno", patientno);
            insertCommand.Parameters.AddWithValue("@doctor", doctor);
            insertCommand.Parameters.AddWithValue("@facility", facility);
            insertCommand.Parameters.AddWithValue("@store", xstore);
            if (!xnewrec)
                insertCommand.Parameters.AddWithValue("@recid", recid);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                vm.REPORTS.alertMessage = "SQL access" + ex;
                return;
            }
            finally
            {
                connection.Close();
            }

        }

        void writeTransmast(DateTime trans_date, string store, string stkcode, string transdescription, string updatetype,
            decimal updateqty, string reference, bool toadd, string xoperator, DateTime operatordttime, string purpose)
        {
            //get stock information
            Stock stock = new Stock();
            stock = Stock.GetStock(store, stkcode, false);
            decimal stkbalance = toadd ? stock.StockQty + updateqty : stock.StockQty - updateqty;

            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.stkConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "TRANSMAS_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Ref_No", reference);
            insertCommand.Parameters.AddWithValue("@Trans_Date", trans_date);
            insertCommand.Parameters.AddWithValue("@TransType", updatetype);
            insertCommand.Parameters.AddWithValue("@Store", store);
            insertCommand.Parameters.AddWithValue("@Item", stkcode);
            insertCommand.Parameters.AddWithValue("@Descript", transdescription);
            insertCommand.Parameters.AddWithValue("@Trans_Qty", updateqty);
            insertCommand.Parameters.AddWithValue("@Stock_Bal", stkbalance);
            insertCommand.Parameters.AddWithValue("@Cost", stock.Cost);
            insertCommand.Parameters.AddWithValue("@Sell", stock.Sell);
            insertCommand.Parameters.AddWithValue("@Posted", false);
            insertCommand.Parameters.AddWithValue("@Bin", stock.Bin);
            insertCommand.Parameters.AddWithValue("@Type", stock.Type);
            insertCommand.Parameters.AddWithValue("@RecType", stock.RecType);
            insertCommand.Parameters.AddWithValue("@Unit", stock.Unit);
            insertCommand.Parameters.AddWithValue("@Costval", stock.costval);
            insertCommand.Parameters.AddWithValue("@Whsell", stock.Whsell);
            insertCommand.Parameters.AddWithValue("@Operator", xoperator);
            insertCommand.Parameters.AddWithValue("@dtime", operatordttime);
            insertCommand.Parameters.AddWithValue("@usize", stock.Usize);
            insertCommand.Parameters.AddWithValue("@Sellval", stock.Sellval);
            insertCommand.Parameters.AddWithValue("@FcCost", stock.Fccost);
            insertCommand.Parameters.AddWithValue("@Fcsell", stock.Fcsell);
            insertCommand.Parameters.AddWithValue("@Currency", stock.Currency);
            insertCommand.Parameters.AddWithValue("@Status", stock.Status);
            insertCommand.Parameters.AddWithValue("@purpose", purpose);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                vm.REPORTS.alertMessage = "" + ex;
                return;
            }
            finally
            {
                connection.Close();
            }
        }

        public JsonResult trtmentTbodyClick(MR_DATA.DUENEXT rowList, bool trtmentChkStore, string trtmentUnit, 
            string trtmentName, string trtmentReference, string trtmentProcBillCode, bool medhistwritten)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            Admrecs admrec = new Admrecs();

            //check if reference exist
            admrec = Admrecs.GetADMRECS(trtmentReference);

            //TOTAL COST AND BILL QTY GREATER THAN 0, CHECK AND UPDATE ADMDETAIL
            if (Convert.ToDecimal(rowList.COST) >= 1 && Convert.ToDecimal(rowList.BILLQTY) >= 1)
            {
                if (!string.IsNullOrWhiteSpace(rowList.STK_ITEM)) //DRUGCODE
                {
                    string xfile = trtmentChkStore ? "stockmas" : "stock";
                    DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT stock_qty,cost,type from " + xfile + " where item = '" + 
                        rowList.PACKQTY.ToString() + "' and store = '" + trtmentUnit + "'", false);

                    if (dt.Rows.Count < 1)
                    {
                        vm.REPORTS.alertMessage = rowList.STK_DESC + " is NOT defined in " + trtmentUnit.Trim() + 
                            " \r\n This item can't be processed... Check Selections and Try Again!";
                        return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                    }

                    if ((decimal)dt.Rows[0]["stock_qty"] < Convert.ToDecimal(rowList.BILLQTY))
                    {
                        vm.REPORTS.alertMessage = "There is not enough qty in stock to service request for " + rowList.STK_DESC;
                        return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                    }

                    //update transmas if store checked
                    if (trtmentChkStore)
                    {
                        writeTransmast(Convert.ToDateTime(rowList.TRANS_DATE), trtmentUnit, rowList.STK_ITEM, rowList.STK_DESC,
                            "I", Convert.ToDecimal(rowList.BILLQTY), trtmentReference, true, msmrfunc.mrGlobals.WOPERATOR, 
                            DateTime.Now, trtmentProcBillCode);
                    }
                    else
                    {
                        string updatestring = "update stockmas set stock_qty = stock_qty - '" + Convert.ToDecimal(rowList.BILLQTY) + 
                            "' where store = '" + trtmentUnit + "' and item = '" + rowList.STK_ITEM + "'";
                        bissclass.UpdateRecords(updatestring, "SMS");

                        SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                        SqlCommand selectCommand = new SqlCommand();
                        selectCommand.CommandText = "STKTRANS_Add";
                        selectCommand.Connection = connection;
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        DataRow row = dt.Rows[0];
                        connection.Open();
                        selectCommand.Parameters.AddWithValue("@Reference", trtmentReference);
                        selectCommand.Parameters.AddWithValue("@TRANS_DATE", Convert.ToDateTime(rowList.TRANS_DATE));
                        selectCommand.Parameters.AddWithValue("@TRANSTYPE", "I");
                        selectCommand.Parameters.AddWithValue("@STORE", trtmentUnit);
                        selectCommand.Parameters.AddWithValue("@ITEM", rowList.STK_ITEM);
                        selectCommand.Parameters.AddWithValue("@DESCRIPTION", rowList.STK_DESC);
                        selectCommand.Parameters.AddWithValue("@TRANS_QTY", Convert.ToDecimal(rowList.BILLQTY));
                        selectCommand.Parameters.AddWithValue("@STOCK_BAL,", (decimal)row["STOCK_QTY"] - Convert.ToDecimal(rowList.BILLQTY));
                        selectCommand.Parameters.AddWithValue("@COST", (decimal)row["cost"]);
                        selectCommand.Parameters.AddWithValue("@SELL", Convert.ToDecimal(rowList.UNITCOST));
                        selectCommand.Parameters.AddWithValue("@POSTED", true);
                        selectCommand.Parameters.AddWithValue("@BIN", "");
                        selectCommand.Parameters.AddWithValue("@TYPE", row["type"].ToString());
                        selectCommand.Parameters.AddWithValue("@UNIT", rowList.BILLQTYUNIT);
                        selectCommand.Parameters.AddWithValue("@RECTYPE", "I");
                        selectCommand.Parameters.AddWithValue("@U_SIZE", 0m);
                        selectCommand.Parameters.AddWithValue("@COSTVAL", 0m);
                        selectCommand.Parameters.AddWithValue("@SELLVAL", 0m);
                        selectCommand.Parameters.AddWithValue("@WHSELL", 0m);
                        selectCommand.Parameters.AddWithValue("@OPERATOR", msmrfunc.mrGlobals.WOPERATOR);
                        selectCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);

                        selectCommand.ExecuteNonQuery();

                        connection.Close();
                    }
                }

                writeAdmdetails(true, trtmentReference, Convert.ToDateTime(rowList.TRANS_DATE), rowList.TIMEGIVEN, 
                    trtmentProcBillCode, trtmentProcBillCode, rowList.STK_ITEM, rowList.STK_DESC, rowList.BILLQTYUNIT, 
                    Convert.ToDecimal(rowList.BILLQTY), Convert.ToDecimal(rowList.COST), false, DateTime.Now,
                    msmrfunc.mrGlobals.WOPERATOR, DateTime.Now, admrec.GROUPCODE, admrec.PATIENTNO, "", "", 0, trtmentUnit);

                Admrecs.UpdateAdmrecAmounts(trtmentReference, Convert.ToDecimal(rowList.COST), 0m);
            }

            string billat = Convert.ToDecimal(rowList.BILLQTY) == 0 ? "0" : Convert.ToDecimal(rowList.BILLQTY) + " " + 
                rowList.BILLQTYUNIT + " = " + rowList.COST.ToString() + " BY " + msmrfunc.mrGlobals.WOPERATOR;

            string xcomments = rowList.STK_DESC.Trim() + " Givern : " + rowList.TIMEGIVEN + " Dose : " + rowList.DOSE.ToString() + 
                " " + rowList.UNIT + "\r\n Billed at " + billat;

            //update med history file
            MedHist medhist = MedHist.GetMEDHIST(admrec.GROUPCODE, admrec.PATIENTNO, "", false, true,
                Convert.ToDateTime(rowList.TRANS_DATE), "DESC");

            bool newhist = (medhist == null) ? true : false;

            if (!newhist)
            {
                xcomments = medhist.COMMENTS.Trim() + "\r\n" + xcomments.Trim();
            }

            if (!medhistwritten)
            {
                xcomments = "==> IN-PATIENT PRESCRIPTIONS (Given):" + DateTime.Now.ToString("dd-MM-yyyy @ HH:mm:sst") + 
                    " : " + msmrfunc.mrGlobals.WOPERATOR + "\r\n" + xcomments;

                vm.REPORTS.chkAuditProfile = true; //for medhistwritten
            }

            MedHist.updatemedhistcomments(admrec.GROUPCODE, admrec.PATIENTNO, Convert.ToDateTime(rowList.TRANS_DATE), 
                xcomments, newhist, admrec.REFERENCE, admrec.NAME, admrec.GHGROUPCODE, admrec.GROUPHEAD, "");

            vm.REPORTS.ActRslt = "Completed...";

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult suTreatmentChtBtnClick(string suAdminReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrec = new Admrecs();

            //check if reference exist
            admrec = Admrecs.GetADMRECS(suAdminReference);

            if (admrec == null)
            {
                vm.REPORTS.alertMessage = "A patient admission record must be specified...";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }


            vm.REPORTS.txtreference = admrec.REFERENCE;
            vm.REPORTS.txtgroupcode = admrec.GROUPCODE;
            vm.REPORTS.txtpatientno = admrec.PATIENTNO;
            vm.REPORTS.TXTPATIENTNAME = admrec.NAME;
            vm.REPORTS.diagnosis = admrec.DIAGNOSIS;
            vm.REPORTS.REPORT_TYPE1 = admrec.ADM_DATE.ToString(); 
            vm.REPORTS.txtBed = admrec.BED;
            vm.REPORTS.txtRoom = admrec.ROOM;
            vm.REPORTS.combFacility = admrec.FACILITY;

            vm.DUENEXTS = ErpFunc.RsGet<MR_DATA.DUENEXT>("MR_DATA",
                "SELECT * FROM DUENEXT WHERE REFERENCE = '"+ suAdminReference + "'");


            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        void AutogenerateCharge(string xtype, DataRow xrow, DataRow xrow1, string acc_code)
        {
            DataTable dispserv = DISPSERV.GetDISPSERV();

            string woperator = Request.Cookies["mrName"].Value;
            string mbillcode = "";
            decimal xamt = 0m;

            if (xtype == "A") //accommodation
            {
                mbillcode = acc_code;
                xamt = (decimal)xrow["rate"];
            }
            else if (xtype == "F")
            {
                mbillcode = xrow1["regcode"].ToString();
                xamt = (decimal)xrow["dailyfeeding"];
            }
            else
            {
                mbillcode = xrow1["conscode"].ToString();
                xamt = (decimal)xrow["dailypnc"];
            }

            //check, just in case nurses had raised bill manually before now
            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT reference from admdetai where reference = '" + xrow["reference"].ToString() + 
                "' and trans_date = '" + DateTime.Now.ToShortDateString() + "' and process = '" + mbillcode + "'", false);

            if (dt.Rows.Count > 0)
                return;

            string masterprocess, xds; masterprocess = xds = "";

            foreach (DataRow row in dispserv.Rows)
            {
                if (xtype == "A" && row["description"].ToString().Contains("ACCOMMODATION") ||
                    xtype == "F" && row["description"].ToString().Contains("FEEDING") ||
                    xtype == "P" && row["description"].ToString().Contains("NURSING") ||
                    xtype == "P" && row["description"].ToString().Contains("PROF"))
                {
                    xds = row["description"].ToString();
                    masterprocess = row["reference"].ToString();
                    break;
                }
            }

            dt = Dataaccess.GetAnytable("", "MR", "select name from tariff where reference = '" + mbillcode + "'", false);

            if (dt.Rows.Count > 0)
                xds = dt.Rows[0]["name"].ToString();

            ADMDETAI.writeAdmdetails(true, xrow["reference"].ToString(), DateTime.Now.Date, DateTime.Now.ToLongTimeString(), mbillcode, 
                masterprocess, "", xds, "", 0m, xamt, false, DateTime.Now, woperator, DateTime.Now, xrow["groupcode"].ToString(), 
                xrow["patientno"].ToString(), "", xrow["facility"].ToString(), 0, "");

            /* xds = "update admrecs set posted = '1', acamt = acamt + '" + xamt + "', date_bille = '"+DateTime.Now.Date+"' where reference = '" + xrow["reference"].ToString();
             bissclass.UpdateRecords(xds, "MR");*/
            Admrecs.UpdateAdmrecAmounts(xrow["reference"].ToString(), xamt, 0m);

        }

        void TimerProcess()
        {
            DataTable dt1 = Dataaccess.GetAnytable("", "MR",
              "select fccode, facilauto, facilauto, facilauto, glintenabl, dactive, pvtcode, installed, serial, ta_post from mrcontrol order by recid", false);

            bool mautoaccommodation = (bool)dt1.Rows[1]["facilauto"];
            bool mautofeeding = (bool)dt1.Rows[2]["facilauto"];
            bool mautopnc = (bool)dt1.Rows[3]["facilauto"];
            string acc_code = dt1.Rows[3]["pvtcode"].ToString();

            var dateNow = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

            if (mautoaccommodation || mautofeeding || mautopnc)
            {
                DataTable dt2 = Dataaccess.GetAnytable("", "MR", 
                    "select reference, rate, dailyfeeding, dailypnc, groupcode, patientno, facility from admrecs where discharge" + 
                    " = '' and date_bille < '" + dateNow + "'", false);

                //DataTable dt2 = Dataaccess.GetAnytable("", "MR",
                //   "select reference, rate, dailyfeeding, dailypnc, groupcode, patientno, facility from admrecs where discharge" +
                //   " = '' and date_bille < '" + DateTime.Now.ToShortDateString() + "'", false);


                if (dt2.Rows.Count < 1)
                    return;

                //timer_Adm.Enabled = false;
                // int xcount = 0;

                DataTable dtc = Dataaccess.GetAnytable("", "MR", "SELECT REGCODE, conscode from mrcontrol where recid = '8'", false);

                foreach (DataRow row in dt2.Rows)
                {
                    if (mautoaccommodation && (decimal)row["rate"] > 0)
                        AutogenerateCharge("A", row, dtc.Rows[0], acc_code);
                    if (mautofeeding && (decimal)row["dailyfeeding"] > 0)
                        AutogenerateCharge("F", row, dtc.Rows[0], acc_code);
                    if (mautopnc && (decimal)row["dailypnc"] > 0)
                        AutogenerateCharge("P", row, dtc.Rows[0], acc_code);

                    string updatestr = "update admrecs set date_bille = '" + DateTime.Now.Date + "' where reference = '" + row["REFERENCE"].ToString() + "'";
                    bissclass.UpdateRecords(updatestr, "MR");
                }

                vm.REPORTS.alertMessage = "Completed for " + dt2.Rows.Count.ToString() + " In-Patient(s) ";

                //timer_Adm.Enabled = true;
            }
            return;
        }

        public JsonResult suAutoGenerateBtnClick(string inpPrescAdmRef)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            TimerProcess();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void LoadDetails(string suAdminReference, bool isduenext, string inpPrescDateFrom, 
            string inpPrescDateTo, bool chkUnProcessed)
        {
            string datestr;
            DateTime dateFrom = Convert.ToDateTime(inpPrescDateFrom);
            DateTime dateTo = Convert.ToDateTime(inpPrescDateTo);

            // mreference = "";
            if (isduenext)
                datestr = " posted = '0'";
            else
            {
                datestr = " trans_date >= '" + dateFrom.ToShortDateString() + "' and trans_date <= '" +
                    dateTo.ToShortDateString() + " 23:59:59.999'";

                if (chkUnProcessed)
                    datestr += " and (phtransferred = '0' OR qty_pr != cumgv)";
            }

            if (!string.IsNullOrWhiteSpace(suAdminReference))
                datestr += " and reference = '" + suAdminReference + "'";

            vm.INPDISPENSAS = ErpFunc.RsGet<MR_DATA.INPDISPENSA>("MR_DATA",
                "select trans_date, itemno, stk_item, stk_desc, qty_pr, cumgv, unit, cdose, cinterval, cduration, " +
                "unitcost, cost, rx, doctor, stkbal, recid, posted, qty_gv, phtransferred, phqtytransferred, name, " +
                "groupcode, patientno, unitpurvalue from inpdispensa where " + datestr + " order by trans_date");

        }

        public JsonResult inpPrescLoadBtnClick(string inpPrescAdmRef, bool isduenext, string inpPrescDateFrom,
            string inpPrescDateTo, bool chkUnProcessed)
        {
            LoadDetails(inpPrescAdmRef, isduenext, inpPrescDateFrom, inpPrescDateTo, chkUnProcessed);

            return Json(vm.INPDISPENSAS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult inpPrescAdmRefFocusout(string inpPrescAdmRef)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (inpPrescAdmRef.Substring(0, 1) != "A")
            {
                if (bissclass.IsDigitsOnly(inpPrescAdmRef.Trim()))
                    vm.REPORTS.txtreference = bissclass.autonumconfig(inpPrescAdmRef, true, "A", "999999999");
            }

            //check if reference exist
            //string admreference,string groupcode,string patientno, string name
            //AnyCode = "";

            DataTable dt = Dataaccess.GetAnytable("", "MR", 
                "select name, reference, groupcode, patientno, adm_date from admrecs where reference = '" + 
                inpPrescAdmRef + "'", false);

            if (dt.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Invalid Admission Reference...";
                vm.REPORTS.txtreference = "";
                return Json(vm.INPDISPENSAS, JsonRequestBehavior.AllowGet);
            }

            DataRow row = dt.Rows[0];
            vm.REPORTS.TXTPATIENTNAME = row["name"].ToString(); //txtNameSu.Text
            vm.REPORTS.mreference = row["reference"].ToString();
            //mgroupcode = row["groupcode"].ToString();
            //mpatientno = row["patientno"].ToString();
            DateTime dateFrom = Convert.ToDateTime(row["adm_date"]).Date;
            DateTime dateTo = DateTime.Now.Date;

            vm.REPORTS.REPORT_TYPE1 = string.Format("{0:yyyy-MM-dd}", dateFrom);
            vm.REPORTS.REPORT_TYPE2 = string.Format("{0:yyyy-MM-dd}", dateTo);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult inpPrescLoadDetails(string xAdminReference, bool isduenext, string inpPrescDateFrom, 
            string inpPrescDateTo, bool chkUnProcessed)
        {
            LoadDetails(xAdminReference, isduenext, inpPrescDateFrom, inpPrescDateTo, chkUnProcessed);

            return Json(vm.INPDISPENSAS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult suStockItemFocusout(string suStockItem, string suUnitID, string suUnitIDText, string suStockItemText,
            bool suIsDelete, string suDateGiven, decimal SUamtsave, IEnumerable<MR_DATA.REPORTS> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            decimal qtyavailable = 0m, cost = 0m, strength = 0m, stkper = 0m, packqty = 0m, pur_cost = 0m, amtsave = SUamtsave;
            bool tocontinue = true, iscapitated = false, preauthorization = false;
            string txtdose = "", unitid = "", xdesc = "", sutracker = "stock";
            int autoremind_period = 0;
            bool isdeleted = suIsDelete;
            string woperator = Request.Cookies["mrName"].Value;
            DateTime transDate = Convert.ToDateTime(suDateGiven);

            DataTable dt = Dataaccess.GetAnytable("", "MR",
                "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            int mdoc_seclevel = (Int32)dt.Rows[0]["wseclevel"];

            if (string.IsNullOrWhiteSpace(suUnitID))
            {
                vm.REPORTS.alertMessage = "Store / Dispensing Unit must be selected...";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            int xv = SUDuplicateCheck(xdesc, sutracker, isdeleted, transDate, mdoc_seclevel, tableList);

            if (xv < 1 || isdeleted)
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);

            if (suStockItem != null) //valid stock definition was selected - CHECK FOR VALID SELECTION
            {
                bool nothing = false;
                decimal rtnamt = stockitemValidate(suStockItem, ref qtyavailable, ref tocontinue, 
                    ref preauthorization, ref iscapitated, suStockItemText, ref txtdose, ref unitid, ref cost, 
                    ref strength, ref stkper, ref packqty, autoremind_period, ref pur_cost, suUnitID, ref nothing);

                if (!tocontinue)
                {
                    vm.REPORTS.cbotitle = ""; //combStkItemSu.Text
                    vm.REPORTS.txtdiscount = 0m; //nmrAmountSu.Value
                    // combStkItemSu.Focus();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                if (rtnamt > 0)
                {
                    amtsave = rtnamt;
                    vm.REPORTS.txtdiscount = rtnamt; //nmrAmountSu.Value
                }
                else //we check for special discount for patient
                {
                    amtsave = cost; //cost of drug on general tariff
                }

                if (amtsave > 0)
                    rtnamt = msmrfunc.applyDefineddiscountValue(amtsave);

                if (preauthorization)
                {
                    vm.REPORTS.SessionBis = "Confirm to Initiate Pre-Authorization Request Alerts to Sections...";

                    //if (result == DialogResult.Yes)
                    //{
                    //    preauthorizationAlert();
                    //    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                    //}
                }

                vm.REPORTS.nmrPayable = amtsave;

                //nmrQtySu.Focus();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult inPatientPresBtnClick(string xAdminReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrecs = new Admrecs();
            admrecs = Admrecs.GetADMRECS(xAdminReference);

            if (string.IsNullOrWhiteSpace(xAdminReference) || admrecs == null)
            {
                vm.REPORTS.alertMessage = "Patient Record must be Selected...";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            
            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult xMedicalNotesClick(string xReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrecs = new Admrecs();
            admrecs = Admrecs.GetADMRECS(xReference);

            if (string.IsNullOrWhiteSpace(xReference) || admrecs == null)
            {
                vm.REPORTS.alertMessage = "Patient Record must be Selected...";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.TXTPATIENTNAME = admrecs.NAME;
            vm.REPORTS.txtgroupcode = admrecs.GROUPCODE;
            vm.REPORTS.txtpatientno = admrecs.PATIENTNO;

            DataTable dtnotes = Dataaccess.GetAnytable("", "MR",
                "select SPNOTES, mednotes from billchain where groupcode = '" + vm.REPORTS.txtgroupcode +
                "' and patientno = '" + vm.REPORTS.txtpatientno + "'", false);

            if (dtnotes.Rows.Count < 1)
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);

            vm.REPORTS.edtallergies = dtnotes.Rows[0]["mednotes"].ToString().Trim(); //txtMednotes
            vm.REPORTS.edtspinstructions = dtnotes.Rows[0]["spnotes"].ToString().Trim(); //txtSpInstructions

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult suSubmitBtnClick(IEnumerable<MR_DATA.REPORTS> tableList, string suAdminReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            vm.REPORTS.txtreference = suAdminReference;

            string woperato = Request.Cookies["mrName"].Value;

            Admissions formObject = new Admissions(vm, woperato);
            vm.REPORTS = formObject.btnSubmitSU_Click(tableList);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult suReturnDelete(string recID)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            int xrecid = -1;
            if (!string.IsNullOrWhiteSpace(recID))
                xrecid = Convert.ToInt32(recID);

            if (xrecid != -1)
            {
                string updstr = "delete from admdetai where recid = '" + xrecid + "'";
                bissclass.UpdateRecords(updstr, "MR");
            }

            vm.REPORTS.alertMessage = "Record Deleted...";
            vm.REPORTS.cmbdelete = false;
            //rtnval = 0;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        int SUDuplicateCheck(string xdesc, string sutracker, bool isdeleted, DateTime transDate, int mdoc_seclevel,
            IEnumerable<MR_DATA.REPORTS> tableList)
        {
            int rtnval = 99;
            DateTime xdate;
            vm.REPORTS.newrec = true;
            string recid = "";

            if(tableList != null)
            {
                foreach (var row in tableList)
                {
                    xdate = Convert.ToDateTime(row.txtclinic);

                    if (xdate == transDate.Date && row.edtallergies.Trim() == xdesc.Trim())
                    {
                        vm.REPORTS.nmr30days = Convert.ToInt32(row.txtstaffno); //recno
                        recid = row.txtbranch;

                        if (sutracker == "stock")
                            vm.REPORTS.txtconsultamt = row.edtallergies; //combStkItemSu.Text
                        else
                            vm.REPORTS.txtothername = row.edtallergies; //combProcedureSu.Text

                        vm.REPORTS.nmrcurdebit = Convert.ToDecimal(row.cbotype); //nmrQtySu.Value
                        vm.REPORTS.txtemployer = row.cbotype; //combUnitSu.Text
                        vm.REPORTS.nmrMinBalance = Convert.ToDecimal(row.txtcreditlimit); //nmrAmountSu.Value

                        ServiceDuplicateOptions serviceduplicate = new ServiceDuplicateOptions();
                        //serviceduplicate.ShowDialog();

                        rtnval = msmrfunc.mrGlobals.returnvalue;

                        if (rtnval < 1 || rtnval > 3)
                        {
                            //combProcessSu.Focus();
                        }
                        else if (rtnval == 1) //ADD 
                            vm.REPORTS.newrec = true;
                        else if (rtnval == 2)  // AMend
                            vm.REPORTS.newrec = false;
                        else if (rtnval == 3) //Delete
                        {
                            if (mdoc_seclevel < 7)
                            {
                                vm.REPORTS.ActRslt = "You are not allowed to delete a charted Service on this Platform\r\n\r\n Please See Systmes Administrator or Your Sectional Head";
                                rtnval = 0;
                                //break;
                            }
                            else
                                isdeleted = true;
                        }
                        break;
                    }
                }
            }

            if (isdeleted)
            {
                vm.REPORTS.SessionDs = "Confirm to Delete Record..." + recid;

                //if (result == DialogResult.Yes)
                //{
                //    int xrecid = -1;
                //    if (!string.IsNullOrWhiteSpace(listView3.Items[recno].SubItems[14].Text))
                //        xrecid = Convert.ToInt32(listView3.Items[recno].SubItems[14].Text);
                //    if (xrecid != -1)
                //    {
                //        string updstr = "delete from admdetai where recid = '" + xrecid + "'";
                //        bissclass.UpdateRecords(updstr, "MR");
                //    }
                //    listView3.Items[recno].Remove();
                //    MessageBox.Show("Record Deleted...");
                //    isdeleted = false;
                //    rtnval = 0;
                //}
            }

            return rtnval;
        }

        public JsonResult suProcedureFocusout(string suProcedure, string suAdminReference, string suDateGiven, 
            string suStockItem, IEnumerable<MR_DATA.REPORTS> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();
            string woperator = Request.Cookies["mrName"].Value;

            DateTime transDate = Convert.ToDateTime(suDateGiven);
            string sutracker = "procedure";
            string procedure = suProcedure;
            bool iscapitated, tocontinue;
            bool preauthorization = false;
            string xdesc = "", xfacility = "";
            decimal rtnamt = 0m;
            bool isdeleted = false;

            admrecs = Admrecs.GetADMRECS(suAdminReference);

            if(admrecs == null)
            {
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);
            rtnamt = msmrfunc.getFeefromtariff(procedure, bchain.PATCATEG, ref xdesc, ref xfacility);

            DataTable dt = Dataaccess.GetAnytable("", "MR",
                "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            int mdoc_seclevel = (Int32)dt.Rows[0]["wseclevel"];

            decimal amtsave = rtnamt;
            vm.REPORTS.nmrMinBalance = amtsave; //nmrAmountSu.Value
            iscapitated = false;
            tocontinue = true;
            vm.REPORTS.txtconsultamt = suStockItem;

            int xv = SUDuplicateCheck(xdesc, sutracker, isdeleted, transDate, mdoc_seclevel, tableList);
            vm.REPORTS.cmbdelete = isdeleted;

            if (xv < 1 || isdeleted)
                return Json(vm, JsonRequestBehavior.AllowGet);

            if (xv == 1)
            {
                vm.REPORTS.nmrMinBalance = amtsave; //nmrAmountSu.Value
                vm.REPORTS.nmrcurdebit = 0m; //nmrQtySu.Value
            }

            //check for hmo/nhis tariff
            if (bchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(bchain.HMOSERVTYPE))
            {
                rtnamt = msmrfunc.hmonhistariffcheck("C", bchain.GROUPHEAD, bchain.HMOSERVTYPE, bchain.PATIENTNO, 
                    procedure, ref preauthorization, ref iscapitated, ref tocontinue, vm.REPORTS.txtconsultamt);

                if (!tocontinue)
                {
                    vm.REPORTS.txtothername = procedure = ""; //combProcedureSu.Text
                    vm.REPORTS.nmrMinBalance = 0m; // nmrAmountSu.Value
                    //combProcedureSu.Focus();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                if (rtnamt > 0)
                {
                    amtsave = rtnamt;
                    vm.REPORTS.nmrMinBalance = rtnamt;
                }
            }

            if (bchain.GROUPHTYPE == "C")
            {
                //we check corporate clients tariff for amount to charge
                rtnamt = msmrfunc.othercorpClientTariffCheck(bchain.PATIENTNO, bchain.PATCATEG, ref preauthorization, 
                    ref tocontinue, vm.REPORTS.txtconsultamt, procedure);

                if (!tocontinue)
                {
                    vm.REPORTS.txtothername = procedure = ""; //combProcedureSu.Text
                    vm.REPORTS.nmrMinBalance = 0m;
                    //combProcedureSu.Focus();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }
                if (rtnamt > 0)
                {
                    amtsave = rtnamt;
                    vm.REPORTS.nmrMinBalance = rtnamt;
                }
            }

            if (rtnamt > 0) //we check for special discount for patient
            {
                rtnamt = msmrfunc.applyDefineddiscountValue(amtsave);
            }

            if (preauthorization)
            {
                vm.REPORTS.SessionBis = "Confirm to Initiate Pre-Authorization Request Alerts to Sections...";

                //if (result == DialogResult.Yes)
                //{
                //    preauthorizationAlert();
                //    return Json(vm, JsonRequestBehavior.AllowGet);
                //}
            }

            //nmrQtySu.Focus();
            vm.REPORTS.nmrPayable = amtsave;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void displayserivesSU(string suAdminReference)
        {
            vm.ADMDETAIS = ErpFunc.RsGet<MR_DATA.ADMDETAI>("MR_DATA",
                "SELECT * FROM ADMDETAI WHERE REFERENCE='" + suAdminReference + "'");
        }

        decimal getAccummulatedCharge(string xreference, decimal oldamt)
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select sum(amount) as amount from admdetai where reference = '" + xreference + "'", false);

            if (dt.Rows.Count < 1)
                return 0m;
            
            string amount = dt.Rows[0]["amount"].ToString();

            if (amount.Trim() == "")
                amount = "0";

            if (Convert.ToDecimal(amount) != oldamt)
            {
                string updstr = "update admrecs set acamt = '" + Convert.ToDecimal(dt.Rows[0]["amount"]) + "' where reference = '" + xreference + "'";
                bissclass.UpdateRecords(updstr, "MR");
            }

            return Convert.ToDecimal(amount);
        }

        public JsonResult suAdminReferenceFocusout(string suAdminReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();
            DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name ", true),
            dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES order by name", true);

            if (suAdminReference.Substring(0, 1) != "A")
            {
                if (bissclass.IsDigitsOnly(suAdminReference))
                    vm.REPORTS.txtreference = bissclass.autonumconfig(suAdminReference, true, "A", "999999999");
            }

            //check if reference exist
            //AnyCode = Anycode1 = "";
            admrecs = Admrecs.GetADMRECS(suAdminReference);

            if (admrecs == null) //new defintion
            {
                vm.REPORTS.alertMessage = "Invalid Admission Reference...";
                vm.REPORTS.txtreference = "";
                //txtreferenceSU.Select();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.REPORTS.mgrouphtype = admrecs.GROUPHTYPE;
                vm.REPORTS.txtpatientno = admrecs.PATIENTNO;
                vm.REPORTS.txtgroupcode = admrecs.GROUPCODE;
                vm.REPORTS.combFacility = bissclass.combodisplayitemCodeName("type_code", admrecs.FACILITY, dtfacility, "name"); // admrecs.FACILITY;
                vm.REPORTS.TXTPATIENTNAME = admrecs.NAME;
                vm.REPORTS.txtRoom = admrecs.ROOM;
                vm.REPORTS.txtBed = admrecs.BED;
                vm.REPORTS.diagnosis = bissclass.combodisplayitemCodeName("type_code", admrecs.DIAGNOSIS, dtdiag, "name");

                if (vm.REPORTS.diagnosis.Trim() != admrecs.DIAGNOSIS_ALL && !string.IsNullOrWhiteSpace(admrecs.DIAGNOSIS_ALL))
                    vm.REPORTS.diagnosis = admrecs.DIAGNOSIS_ALL;

                vm.REPORTS.REPORT_TYPE1 = admrecs.ADM_DATE.ToShortDateString(); //for txtadm_dateSu.Text
                vm.REPORTS.REPORT_TYPE2 = admrecs.DISCHARGE;
                DateTime xdischarge = string.IsNullOrWhiteSpace(admrecs.DISCHARGE) ? DateTime.Now.Date : Convert.ToDateTime(admrecs.DISCHARGE);
                vm.REPORTS.REPORT_TYPE3 = (xdischarge - admrecs.ADM_DATE).Days.ToString() + " day(s)";
                vm.REPORTS.mgrouphead = admrecs.GROUPHEAD;
                bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);
                vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
                vm.REPORTS.txtstaffno = patientprofile(bchain); //edtprofileSu.Text
                vm.REPORTS.txtgrouphead = getgrouphead(bchain.GROUPHEAD, bchain.GHGROUPCODE, bchain.GROUPHTYPE);

                if (vm.REPORTS.txtgrouphead.Trim() == "Abort")
                {
                    //txtreferenceSU.Focus();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                //displayPatientPicture(this.pictureBox_SU, bchain.PICLOCATION);
                displayserivesSU(suAdminReference);
                vm.REPORTS.mreference = vm.REPORTS.txtreference;

                if (admrecs.DISCHARGE != "") {
                    vm.REPORTS.ActRslt = "This Patient has been discharged on " + admrecs.DISCHARGE + "," + admrecs.NAME;
                }
                else {
                    vm.REPORTS.chkADVCorporate = true; //for btnTreatmtCht.Enabled
                }

                getAccummulatedCharge(admrecs.REFERENCE, admrecs.ACAMT);
                // if (nmrCurrentTotalSU.Value != admrecs.ACAMT)
                //     Admrecs.UpdateAdmrecAmounts(admrecs.REFERENCE, nmrCurrentTotalSU.Value, 0m);
                vm.REPORTS.chkApplyFilter = true; //for btnInPtPrescSu.Enabled
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        //Registration
        public JsonResult regLoadListBtnClick()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dt = getLinkDetails("", 0, 0m, 0m, "", false, "A", 9, "A", "");

            if (dt.Rows.Count > 0)
            {
                frmGetlinkinfo linkinfo = new frmGetlinkinfo(dt);
                //linkinfo.ShowDialog();

                vm.REPORTS.txtreference = msmrfunc.mrGlobals.anycode; //mcrossref
                //Anycode1 = AnyCode = "";
                //txtreferenceReg.Focus();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult regSubmitBtnClick(MR_DATA.REPORTS dataObject)
        {
            vm.REPORTS = dataObject;

            string woperato = Request.Cookies["mrName"].Value;

            Admissions formObject = new Admissions(vm, woperato);
            vm.REPORTS = formObject.btnSaveReg_Click();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void displaydetails_SpaceDef(string regFacility)
        {
            vm.ADMSPACES = ErpFunc.RsGet<MR_DATA.ADMSPACE>("MR_DATA",
               "SELECT * FROM ADMSPACE WHERE FACILITY='" + regFacility + "' ORDER BY ROOM");
        }

        public JsonResult regFacilityOnChange(string regFacility)
        {
            displaydetails_SpaceDef(regFacility);

            return Json(vm.ADMSPACES, JsonRequestBehavior.AllowGet);
        }

        void checkOutPatientBillnPay(billchaindtl bchain)
        {
            //15/05/2012 - we check if patient has bills from outpatient consulting for today
            DataTable tmpbills = Dataaccess.GetAnytable("", "MR", "select billing.reference, billing.amount, paydetail.reference as Payreference, paydetail.amount as PayAmount from billing LEFT JOIN paydetail on billing.groupcode = paydetail.groupcode and billing.patientno = paydetail.patientno where billing.groupcode = '" + 
                bchain.GROUPCODE + "' and billing.patientno = '" + bchain.PATIENTNO + "' and billing.trans_date = '" + DateTime.Now.ToShortDateString() + "'", false);

            if (tmpbills.Rows.Count < 1)
            {
                tmpbills = Dataaccess.GetAnytable("", "MR", "select INPDISPENSA.reference from INPDISPENSA where INPDISPENSA.groupcode = '" + bchain.GROUPCODE + 
                    "' and INPDISPENSA.patientno = '" + bchain.PATIENTNO + "' and INPDISPENSA.trans_date = '" + DateTime.Now.ToShortDateString() + "'", false);

                if (tmpbills.Rows.Count < 1)
                {
                    tmpbills = Dataaccess.GetAnytable("", "MR", "select suspense.reference from suspense where suspense.groupcode = '" + bchain.GROUPCODE + 
                        "' and suspense.patientno = '" + bchain.PATIENTNO + "' and suspense.trans_date = '" + DateTime.Now.ToShortDateString() + "'", false);

                    if (tmpbills.Rows.Count > 0){
                        vm.REPORTS.chkApplyFilter = true; //chkConvertbills.Visible
                    }
                }
            }

            if (tmpbills.Rows.Count > 0)
            {
                vm.REPORTS.chkApplyFilter = true; //chkConvertbills.Visible
                vm.REPORTS.ActRslt = "Out-Patient Bills/Payments/Request(s) found for this Patient on this Visit...\r\n CONVERSION ON RECORD SAVE ! ";
            }
            return;
            // combFacilityreg.Focus();
        }

        public JsonResult regPatientNoFocusout(string regPatientNo, string regGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();
            Customer customers = new Customer();

            //check if patientno exists
            bchain = Getbillchain(regPatientNo, regGroupCode);
            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number...";
                //txtPatNoReg.Text = "";
                //txtgroupcode_Reg.Focus();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //displayPatientPicture(this.pictureBox_Reg, bchain.PICLOCATION);
                //we check if this patient is already admitted on different reference and not discharged
                //Admrecs tmpadmrecs = Admrecs.GetADMRECS(bchain.GROUPCODE, bchain.PATIENTNO, " ", true);
                DataTable tmpadmrecs = Dataaccess.GetAnytable("", "MR", "select reference, adm_date from admrecs where groupcode = '" + 
                    bchain.GROUPCODE + "' and patientno = '" + bchain.PATIENTNO + "' and discharge = ''", false);

                if (tmpadmrecs.Rows.Count > 0)
                {
                    vm.REPORTS.alertMessage = "This Patient is already on Admission... CHECK Admission Ref. : " + tmpadmrecs.Rows[0]["REFERENCE"].ToString().Trim();
                    //txtPatNoReg.Text = "";
                    //pictureBox_Reg.Image = null;
                    //txtPatNoReg.Select();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.mgrouphtype = bchain.GROUPHTYPE;
                //DisplayDetailsReg();
                vm.REPORTS.txtsurname = patientprofile(bchain); //edtprofileReg.Text
                vm.REPORTS.txtgrouphead = getgrouphead(bchain.GROUPHEAD, bchain.GHGROUPCODE, bchain.GROUPHTYPE);

                if (vm.REPORTS.txtgrouphead.Trim() == "Abort")
                {
                    vm.REPORTS.chkSegmented = true;
                    //txtreferenceReg.Focus();
                    //btnreload.PerformClick();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
                //15/05/2012 - we check if patient has bills from outpatient consulting for today
                checkOutPatientBillnPay(bchain);
               
                //we get defined specific bed space/feeding tariff, if corporate
                if (bchain.GROUPHTYPE == "C")
                {
                    vm.REPORTS.nmrPayable = 0m; //for nmrFeedingReg.Value;
                    vm.REPORTS.txtdiscount = 0m; //for nmrDailyRateReg.Value

                    if (customers.HMO)
                    {
                        Hmodetail hmodetail = Hmodetail.GetHMODETAIL(bchain.GROUPHEAD, bchain.HMOSERVTYPE);
                        if (hmodetail != null)
                        {
                            vm.REPORTS.nmrPayable = hmodetail.FEEDING; //nmrFeedingReg.Value
                            vm.REPORTS.txtdiscount = hmodetail.ACCFEEDING; //nmrDailyRateReg.Value
                        }
                    }

                    vm.REPORTS.nmrPayable = vm.REPORTS.nmrPayable < 1 ? customers.Feeding : vm.REPORTS.nmrPayable;
                    vm.REPORTS.txtdiscount = vm.REPORTS.txtdiscount < 1 ? customers.Admissions : vm.REPORTS.txtdiscount;
                }
            }
            //combFacilityreg.Focus();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        string patientprofile(billchaindtl bchain)
        {
            //edtprofileRev.Text = 
            string xtext = "[ " + bchain.SEX + " ] ;    AGE : ";
            string xx = (bchain.BIRTHDATE.Year > 1920) ? bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now.Date) :
                (bchain.RELATIONSH == "C") ? "Minor" : (bchain.RELATIONSH == "S" || bchain.RELATIONSH == "W" ||
                bchain.RELATIONSH == "H") ? "< Adult >" : "...";
            string xx1 = "     M_STATUS : < " + bchain.M_STATUS + " > ";
            xtext = xtext + xx + "; " + xx1;
            return xtext;
        }

        void DisplayDetailsReg(Admrecs admrecs, billchaindtl bchain)
        {
            vm.REPORTS.mgrouphtype = admrecs.GROUPHTYPE;
            vm.REPORTS.mgrouphead = admrecs.GROUPHEAD;

            vm.REPORTS.txtpatientno = admrecs.PATIENTNO;
            vm.REPORTS.txtgroupcode = admrecs.GROUPCODE;
            vm.REPORTS.TXTPATIENTNAME = admrecs.NAME;
            //combFacilityreg.Text = bissclass.combodisplayitemCodeName("type_code", admrecs.FACILITY, dtfacility, "name");
            vm.REPORTS.combFacility = admrecs.FACILITY; //= facilitysave
            vm.REPORTS.REPORT_TYPE1 = admrecs.ROOM;  //= roomsave
            vm.REPORTS.REPORT_TYPE2 = admrecs.BED;  // = bedsave
            DateTime dateReg = admrecs.ADM_DATE; //= madmdate 
            vm.REPORTS.REPORT_TYPE3 = string.Format("{0:yyyy-MM-dd}", dateReg);
            vm.REPORTS.txtTimeFrom = admrecs.TIME;
            vm.REPORTS.doctor = admrecs.DOCTOR;
            //bissclass.displaycombo(combDocReg, dtdocs, admrecs.DOCTOR, "reference");
            //bissclass.displaycombo(combDiagReg, dtdiag, admrecs.DIAGNOSIS, "type_code");
            //bissclass.displaycombo(combCostCtrReg, dtbranch, admrecs.UNIT, "type_code");

            vm.REPORTS.txtbranch = admrecs.UNIT;
            vm.REPORTS.txtdiscount = admrecs.RATE; //nmrDailyRateReg.Value
            //oldrate = admrecs.RATE;
            vm.REPORTS.nmrbalance = admrecs.DAILYPNC; //nmrPncReg.Value
            vm.REPORTS.nmrPayable = admrecs.DAILYFEEDING;
            vm.REPORTS.txtsurname = patientprofile(bchain); //edtprofileReg.Text
            vm.REPORTS.REPORT_TYPE4 = admrecs.DIAGNOSIS_ALL; //diagnosis
            //displayPatientPicture(this.pictureBox_Reg, bchain.PICLOCATION);
        }

        public JsonResult regReferenceFocusout(string regReference, decimal mlastno)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Admrecs admrecs = new Admrecs();
            billchaindtl bchain = new billchaindtl();
            string woperator = Request.Cookies["mrName"].Value;

            DataTable dt = Dataaccess.GetAnytable("", "MR",
               "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            bool mcandelete = (bool)dt.Rows[0]["candelete"];
            bool mcanadd = (bool)dt.Rows[0]["canadd"];
            bool mcanalter = (bool)dt.Rows[0]["canalter"];

            string AnyCode = msmrfunc.mrGlobals.anycode;

            if (string.IsNullOrWhiteSpace(AnyCode) || regReference.Substring(0, 1) != "A")
            {
                if (bissclass.IsDigitsOnly(regReference) && Convert.ToDecimal(regReference) > mlastno)
                {
                    vm.REPORTS.alertMessage = "Admissions Reference is out of Sequence...";
                    vm.REPORTS.chkAuditProfile = true;

                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                if (bissclass.IsDigitsOnly(regReference))
                    vm.REPORTS.txtreference = bissclass.autonumconfig(regReference, true, "A", "999999999");
            }

            vm.REPORTS.newrec = true;
            AnyCode = "";
            admrecs = Admrecs.GetADMRECS(regReference);

            if (admrecs == null) //new defintion
            {
                vm.REPORTS.txtTimeFrom = DateTime.Now.ToShortTimeString();
                vm.REPORTS.chkADVCorporate = true;  //chk_Reg_AdmNoticeToDoc.Checked
            }
            else
            {
                vm.REPORTS.newrec = false;

                string xvalue = getgrouphead(admrecs.GROUPHEAD, admrecs.GHGROUPCODE, admrecs.GROUPHTYPE);
                if (xvalue == "Abort")
                {
                    vm.REPORTS.chkAuditProfile = true;
                    //txtreferenceReg.Focus();
                    //btnreload.PerformClick();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.txtgrouphead = xvalue;

                bchain = Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);

                DisplayDetailsReg(admrecs, bchain);

                if (admrecs.DISCHARGE != "")
                {
                    vm.REPORTS.alertMessage = "This Patient has been discharged on " + admrecs.DISCHARGE + "," + admrecs.NAME;
                    vm.REPORTS.chkAuditProfile = true;
                    vm.REPORTS.txtreference = "";
                    //txtreferenceReg.Select();
                    //btnreload.PerformClick();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                if (admrecs.POSTED || admrecs.ACAMT > 0)
                {
                    vm.REPORTS.ActRslt = "Record is Posted... LIMITED UPDATE ALLOWED!";
                    //txtPatNoReg.Enabled = txtgroupcode_Reg.Enabled = combFacilityreg.Enabled = combRmWdReg.Enabled = combBedReg.Enabled = dtdateregReg.Enabled = false;
                    //lblLimitedUpdate1.Visible = lblLimitedUpdate2.Visible = lblLimitedUpdate3.Visible = lblLimitedUpdate4.Visible = lblLimitedUpdate5.Visible = lblLimitedUpdate6.Visible = true;
                }
                else
                {
                    vm.REPORTS.txtstaffno = "Record Exists";
                    vm.REPORTS.chkApplyFilter = mcandelete ? true : false; //btnDeleteReg.Enabled
                }
            }

            if (vm.REPORTS.newrec && mcanadd)
                vm.REPORTS.chkbillregistration = true; //btnSaveReg.Enabled
            else if (!vm.REPORTS.newrec && mcanalter)
                vm.REPORTS.chkbillregistration = true; //btnSaveReg.Enabled

            if (vm.REPORTS.newrec)
                vm.REPORTS.chkByBranch = true; //txtgroupcode_Reg.Focus();
            else if (admrecs.POSTED || admrecs.ACAMT > 0)
                vm.REPORTS.chkCurrtAdmRev = true; //combDiagReg.Focus();

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void LINKDISPLAYDETAILS(string mcrossref)
        {
            Mrattend mrattend = new Mrattend();
            bool payok = false;

            DataTable dt1 = Dataaccess.GetAnytable("", "MR",
                "select fccode, facilauto, facilauto, facilauto, glintenabl, dactive, pvtcode, installed, serial, ta_post from mrcontrol order by recid", false);

            bool mdactive = (bool)dt1.Rows[3]["dactive"];

            if (!string.IsNullOrWhiteSpace(mcrossref))
            {
                mrattend = Mrattend.GetMrattend(mcrossref);
                if (mrattend == null)
                {
                    vm.REPORTS.alertMessage = "Unable to Link Selected Consultation Reference in Daily Attendance Register... ";
                    //txtreferenceReg.Text = "";
                    //txtreferenceReg.Focus();
                    return;
                }

                vm.REPORTS.txtgroupcode = mrattend.GROUPCODE;
                vm.REPORTS.txtpatientno = mrattend.PATIENTNO;
                DateTime dateReg = mrattend.TRANS_DATE;
                vm.REPORTS.REPORT_TYPE1 = string.Format("{0:yyyy-MM-dd}", dateReg);
                vm.REPORTS.TXTPATIENTNAME = mrattend.NAME;
                //bissclass.displaycombo(combDiagReg, dtdiag, mrattend.DIAGNOSIS, "type_code");
                //bissclass.displaycombo(combDocReg, dtdocs, mrattend.DOCTOR, "reference");
                vm.REPORTS.mgroupcode = mrattend.GROUPCODE;
                vm.REPORTS.mpatientno = mrattend.PATIENTNO;
                vm.REPORTS.mgrouphead = mrattend.GROUPHEAD;
                vm.REPORTS.mghgroupcode = mrattend.GHGROUPCODE;
                vm.REPORTS.mgrouphtype = mrattend.GROUPHTYPE;

                //get diagnosis
                DataTable dt = Dataaccess.GetAnytable("", "MR", "select provisional, final from pmedhdiag where reference = '" + mrattend.REFERENCE + "'", false);

                if (dt.Rows.Count > 0)
                {
                     vm.REPORTS.REPORT_TYPE2 = dt.Rows[0]["provisional"].ToString() + ", " + 
                        dt.Rows[0]["final"].ToString(); //txtPrimaryDiagnosis.Text
                }

                if (vm.REPORTS.mgrouphead == vm.REPORTS.mpatientno || vm.REPORTS.mgrouphtype == "P")
                    vm.REPORTS.cashpaying = true;

                if (vm.REPORTS.cashpaying)
                    payok = msmrfunc.checklinkOK("A", mcrossref);

                if (vm.REPORTS.cashpaying && mdactive && !payok)
                {
                    vm.REPORTS.alertMessage = "No Admission Deposit found for this Referal...";
                    vm.REPORTS.chkAuditProfile = true;
                    //txtreferenceReg.Text = "";
                    // txtreferenceReg.Focus();
                    //btnreload.PerformClick();
                    return;
                }
            }
        }

        public JsonResult regReferenceFocus(string regReference, string regmcrossref)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            decimal mlastno = 0;

            string mcrossref = regmcrossref;

            if (!string.IsNullOrWhiteSpace(mcrossref)) {
                LINKDISPLAYDETAILS(mcrossref);
            }

            if (string.IsNullOrWhiteSpace(regReference))
            {
                vm.REPORTS.mlastno = msmrfunc.getcontrol_lastnumber("ADMIT", 8, false, mlastno, false);
                vm.REPORTS.txtreference = vm.REPORTS.mlastno.ToString();
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        //Space Definition
        public JsonResult spacDefRelBedSpBtnClicked(string tableRecID)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            string updstr = "update admspace set occupant = '' where recid = '" + tableRecID + "'";
            bissclass.UpdateRecords(updstr, "MR");
            vm.REPORTS.alertMessage = "Record Updated...";

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        bool DeleteAdmSpace(string xfacility, string room, string bed)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "admspace_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Facility", xfacility);
            deleteStatement.Parameters.AddWithValue("@Room", room);
            deleteStatement.Parameters.AddWithValue("@Bed", bed);

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
                vm.REPORTS.alertMessage = "" + ex;
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public JsonResult spacDefDeleteBtnClicked(string spacDefFacility, string spacDefRoom, string spacDefBedNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (DeleteAdmSpace(spacDefFacility, spacDefRoom, spacDefBedNo))
            {
                vm.REPORTS.alertMessage = "Deleted Successfully";
            };

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult spacDefSubmitBtnClicked(IEnumerable<MR_DATA.REPORTS> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTSS = tableList;

            string woperato = Request.Cookies["mrName"].Value;

            Admissions formObject = new Admissions(vm, woperato);

            vm.REPORTS = formObject.savedetails();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult spacDefFacilityFocusout(string spacDefFacility)
        {
            vm.ADMSPACES = ErpFunc.RsGet<MR_DATA.ADMSPACE>("MR_DATA", 
                "SELECT * FROM ADMSPACE WHERE FACILITY='"+ spacDefFacility +"' ORDER BY ROOM");

            return Json(vm.ADMSPACES, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //Admissions Management End


        //Image Acquisition Start
        #region        

        public JsonResult imageAcqSubmitClicked(MR_DATA.REPORTS dataObject)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS = dataObject;

            string woperato = Request.Cookies["mrName"].Value;

            frmImageAcquisition formObject = new frmImageAcquisition(woperato, vm);

            vm.REPORTS = formObject.btnSave_Click();


            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult imageOkBtnClicked(HttpPostedFileBase file, bool isPDF)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (file == null)
            {
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
            string extension = System.IO.Path.GetExtension(file.FileName);
            string fullFileName = fileName + DateTime.Now.ToString("yymmssff") + extension; //Date is added to the name to make each file to be unique;
            string path = System.IO.Path.Combine(Server.MapPath("~/Resources/Images/"), fullFileName);
            file.SaveAs(path);

            vm.REPORTS.pictureBox1 = fullFileName;
            vm.REPORTS.RptPath = path;
           
            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult imageAcqPatientNoFocusOut(string groupCode, string patientNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();

            //check if patientno exists
            bchain = Getbillchain(patientNo, groupCode);

            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
            vm.REPORTS.txtgrouphead = msmrfunc.GETGroupheadname(bchain.GHGROUPCODE, bchain.GROUPHEAD, bchain.GROUPHTYPE);
        
            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void loadpreviousDefinitions(string groupCode, string patientNo, string transDate)
        {
            vm.REPORTS.newrec = true;

            DataTable medhpic = Dataaccess.GetAnytable("", "MR",
                "select * from medhpic where groupcode = '" + groupCode + "' and patientno = '" + patientNo +
                "' and trans_date = '" + transDate + "'", false);

            if (medhpic.Rows.Count < 1)
                return;

            vm.REPORTS.newrec = false;
            int recid = Convert.ToInt32(medhpic.Rows[0]["recid"]);
            DataRow row = medhpic.Rows[0];

          
            if (!string.IsNullOrWhiteSpace(row["pic1"].ToString()))
                vm.MEDHPICPROP.PIC1 = System.IO.Path.GetFileName(row["pic1"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic2"].ToString()))
                vm.MEDHPICPROP.PIC2 = System.IO.Path.GetFileName(row["pic2"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic3"].ToString()))
                vm.MEDHPICPROP.PIC3 = System.IO.Path.GetFileName(row["pic3"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic4"].ToString()))
                vm.MEDHPICPROP.PIC4 = System.IO.Path.GetFileName(row["pic4"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic5"].ToString()))
                vm.MEDHPICPROP.PIC5 = System.IO.Path.GetFileName(row["pic5"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic6"].ToString()))
                vm.MEDHPICPROP.PIC6 = System.IO.Path.GetFileName(row["pic6"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic7"].ToString()))
                vm.MEDHPICPROP.PIC7 = System.IO.Path.GetFileName(row["pic7"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic8"].ToString()))
                vm.MEDHPICPROP.PIC8 = System.IO.Path.GetFileName(row["pic8"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic9"].ToString()))
                vm.MEDHPICPROP.PIC9 = System.IO.Path.GetFileName(row["pic9"].ToString().Trim());
            if (!string.IsNullOrWhiteSpace(row["pic10"].ToString()))
                vm.MEDHPICPROP.PIC10 = System.IO.Path.GetFileName(row["pic10"].ToString().Trim());


            vm.MEDHPICPROP.pdffile1 = row["pdffile1"].ToString().Trim();
            vm.MEDHPICPROP.pdffile2 = row["pdffile2"].ToString().Trim();
            vm.MEDHPICPROP.pdffile3 = row["pdffile3"].ToString().Trim();
            vm.MEDHPICPROP.pdffile4 = row["pdffile4"].ToString().Trim();
            vm.MEDHPICPROP.pdffile5 = row["pdffile5"].ToString().Trim();
            vm.MEDHPICPROP.pdffile6 = row["pdffile6"].ToString().Trim();
            vm.MEDHPICPROP.pdffile7 = row["pdffile7"].ToString().Trim();
            vm.MEDHPICPROP.pdffile8 = row["pdffile8"].ToString().Trim();
            vm.MEDHPICPROP.pdffile9 = row["pdffile9"].ToString().Trim();

        }

        public JsonResult PrevDefinitionFocusOut(string groupCode, string patientNo, string transDate)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            vm.MEDHPICPROP = new MR_DATA.MEDHPIC();

            loadpreviousDefinitions(groupCode, patientNo, transDate);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //Image Acquisition End


        //Material Definition Costing Start
        #region
        
        public JsonResult validateStock(string code, string store)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            //Stock stock = new Stock();

            //stock = Stock.GetStock(store, code, false);
            vm.SCS01vm = new SCS01.SCS01vm
            {
                stock = ErpFunc.RGet<SCS01.stock>("SCS01", "SELECT * FROM stock WHERE store='" + store +
                "' AND item='" + code + "' ORDER BY NAME")
            };

            if (vm.SCS01vm.stock == null)
            {
                vm.REPORTS.alertMessage = "Selected Stock is not registered in " + store;
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            if (vm.SCS01vm.stock.posted != true)
            {
                vm.REPORTS.alertMessage = "Selected Stock : " + vm.SCS01vm.stock.name.Trim() + 
                    " Definition has not been confirmed in " + vm.SCS01vm.stock.store;

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            if (vm.SCS01vm.stock.status == "D")
            {
                vm.REPORTS.alertMessage = "This Item has been flagged domant... NO UPDATE ALLOWED !";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.txtreference = vm.SCS01vm.stock.item;
            vm.REPORTS.txtaddress1 = vm.SCS01vm.stock.store;
            vm.REPORTS.TXTPATIENTNAME = vm.SCS01vm.stock.name; //dataGridView1.Rows[recno].Cells[3].Value = stock.Name;
            vm.REPORTS.cbotype = vm.SCS01vm.stock.unit;        //dataGridView1.Rows[recno].Cells[6].Value = stock.Unit;
            vm.REPORTS.cost = vm.SCS01vm.stock.cost;     //dataGridView1.Rows[recno].Cells[7].Value = stock.Cost;
            vm.REPORTS.sell = vm.SCS01vm.stock.sell;     //dataGridView1.Rows[recno].Cells[8].Value = stock.Sell;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult McostingRemoveBtn(string recID)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (recID != "" && Convert.ToInt32(recID) > 0)
            {
                string updstr = "delete from mrb19 where recid = '" + recID + "'";
                bissclass.UpdateRecords(updstr, "MR");
            }

            vm.REPORTS.alertMessage = "Record Deleted...";

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult appendBtnClicked(string facility, string procedure)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DataTable dtCopy = (DataTable)Session["dtCopy"];

            if (dtCopy == null)
            {
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (dtCopy.Rows.Count < 1)
                {
                    vm.REPORTS.alertMessage = "No Definition Copied...";
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }
            }

            string woperato = Request.Cookies["mrName"].Value;

            frmInvMaterialCostDefinition formObject = new frmInvMaterialCostDefinition(vm, woperato);

            vm.REPORTS = formObject.btnAppend_Click(facility, procedure, dtCopy);

            //Clear Session
            Session["dtCopy"] = null;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult copyBtnClicked(string facility, string procedure)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            Session["dtCopy"] = Dataaccess.GetAnytable("", "MR", "SELECT * FROM MRB19 WHERE FACILITY = '" + facility + 
                "' and process = '" + procedure + "'", false);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult matCostingSubmit(IEnumerable<MR_DATA.REPORTS> tableList, string facility, string procedure)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTSS = tableList;
            
            string woperato = Request.Cookies["mrName"].Value;

            frmInvMaterialCostDefinition formObject = new frmInvMaterialCostDefinition(vm, woperato);

            vm.REPORTS = formObject.savedetails(facility, procedure);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MCostingProcedureFocusOut(string facility, string procedure)
        {
            vm.MRB19S = ErpFunc.RsGet<MR_DATA.MRB19>("MR_DATA",
                "SELECT * FROM MRB19 WHERE FACILITY = '" + facility + "' and process = '" + procedure + "'");

            return Json(vm.MRB19S, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MCostingFacilityFocusOut(string facility)
        {
            string selectstring = "SELECT NAME, REFERENCE FROM tariff WHERE category = '" + facility + "' ORDER BY NAME";

            vm.TARIFFS = ErpFunc.RsGet<MR_DATA.TARIFF>("MR_DATA", selectstring);
            
            return Json(vm.TARIFFS, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
        //Material Definition Costing End

        
        //SampleCollectDetails start
        #region
        public JsonResult sampleReferenceFocusout(string facility, string sampleReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            //ds.Clear();
            //ds.Tables.Clear();
            DataTable dtsample = Dataaccess.GetAnytable("", "MR", "SELECT * FROM phl01 WHERE rtrim(REFERENCE) = '" + sampleReference.Trim() + "' and facility = '" + facility + "'", false);

            if (dtsample.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Invalid Reference...";
                //txtReference.Text = "";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.TXTPATIENTNAME = dtsample.Rows[0]["name"].ToString();
            var transDate = (DateTime)dtsample.Rows[0]["trans_date"]; 
            vm.REPORTS.REPORT_TYPE2 = string.Format("{0:yyyy-MM-dd}", transDate);
            //datefrom = dateto = (DateTime)dtsample.Rows[0]["trans_date"];
            //ds.Tables.Add(dtsample);
            vm.REPORTS.chkADVCorporate = true; //for isdataset

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult sampleCollectionOnLoad()
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperator = Request.Cookies["mrName"].Value;
            int nwseclevel;

            DataTable dt = Dataaccess.GetAnytable("", "MR", 
                "select wseclevel, candelete, canalter, canadd from mrstlev where operator = '" + woperator + "'", false);

            nwseclevel = (int)dt.Rows[0]["wseclevel"];
            vm.REPORTS.chkADVCorporate = (bool)dt.Rows[0]["canadd"]; //for mcanadd

            if (nwseclevel != 1)
            {
                vm.REPORTS.alertMessage = "Limited Access - " + woperator.Trim() + " not defined for Phlebotomy. . .";
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult serviceRefFocusout( string serviceRef, string facility)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.txtreference = bissclass.autonumconfig(serviceRef, true, "", "999999999");

            PHL01 phlebo = PHL01.GetPHL01(serviceRef, facility);

            if (phlebo != null)
            {
                vm.REPORTS.newrec = false;

                vm.REPORTS.txtaddress1 = phlebo.ADDRESS1;
                vm.REPORTS.cboAge = phlebo.AGE.ToString();
                vm.REPORTS.REPORT_TYPE1 = phlebo.CROSSREF;
                vm.REPORTS.edtallergies = phlebo.DEFAULTSTRING;
                vm.REPORTS.TXTPATIENTNAME = phlebo.NAME;

                vm.REPORTS.txtgroupcode = phlebo.GROUPCODE;
                vm.REPORTS.edtspinstructions = phlebo.OTHERS;
                vm.REPORTS.txtpatientno = phlebo.PATIENTNO;
                //txtGroupheadName.Text = phlebo.
                vm.REPORTS.REPORT_TYPE2 = phlebo.DOCTOR;
                vm.REPORTS.REPORT_TYPE4 = phlebo.SAMPLEBY;
                vm.REPORTS.cbogender = phlebo.SEX;
                DateTime collectionDate = phlebo.SAMPLEDATE; //for dtcollectiondate.Value
                vm.REPORTS.REPORT_TYPE3 = String.Format("{0:yyyy-MM-dd}", collectionDate);

                vm.REPORTS.alertMessage = "RECORD EXIST - Limited Update allowed!";
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult serviceRefFocus( int servicerecno, decimal mlastno)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.mlastno = msmrfunc.getcontrol_lastnumber("XRAYNO", servicerecno, false, mlastno, false) + 1;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult othersNoteClick(bool chkBlood, bool chkSputum, bool chkStool, bool chkUrine, bool chkSwab, bool chkSemen,
            bool chkHair, string otherText, string sampleCollectedBy, string collectionDateTime, string defaults)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DateTime collectionDate = Convert.ToDateTime(collectionDateTime);

            string xchecked = chkBlood ? "Blood; " : chkSputum ? "Sputum; " : chkStool ? "Stool; " : chkUrine ? "Urine; " :
               chkSwab ? "Swab; " : chkSemen ? "Semen; " : chkHair ? "Hair; " : !string.IsNullOrWhiteSpace(otherText) ?
               "Others - (" + otherText.Trim() + ")" : "";

            vm.REPORTS.edtspinstructions = "=> Sample Taken : " + xchecked + " - by " + sampleCollectedBy + "  @  " +
                collectionDate.Date + " " + collectionDate.ToShortTimeString();

            if (string.IsNullOrWhiteSpace(defaults))
                vm.REPORTS.edtallergies = vm.REPORTS.edtspinstructions;

            if (xchecked != "")
                vm.REPORTS.chkADVCorporate = true;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult defaultClick(bool chkBlood, bool chkSputum, bool chkStool, bool chkUrine, bool chkSwab, bool chkSemen,
            bool chkHair, string otherText, string sampleCollectedBy, string collectionDateTime, string othersNote)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DateTime collectionDate = Convert.ToDateTime(collectionDateTime);

            string xchecked = chkBlood ? "Blood; " : chkSputum ? "Sputum; " : chkStool ? "Stool; " : chkUrine ? "Urine; " :
               chkSwab ? "Swab; " : chkSemen ? "Semen; " : chkHair ? "Hair; " : !string.IsNullOrWhiteSpace(otherText) ?
               "Others - (" + otherText.Trim() + ")" : "";

            vm.REPORTS.edtallergies = "=> Sample Taken : " + xchecked + " - by " + sampleCollectedBy + "  @  " +
                collectionDate.Date + " " + collectionDate.ToShortTimeString();

            if (string.IsNullOrWhiteSpace(othersNote))
                vm.REPORTS.edtspinstructions = vm.REPORTS.edtallergies;

            if (xchecked != "")
                vm.REPORTS.chkADVCorporate = true;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult sampleTypeBlockFocusout(bool chkBlood, bool chkSputum, bool chkStool, bool chkUrine, bool chkSwab, bool chkSemen,
            bool chkHair, string otherText, string sampleCollectedBy, string collectionDateTime)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DateTime collectionDate = Convert.ToDateTime(collectionDateTime);

            string xchecked = chkBlood ? "Blood; " : chkSputum ? "Sputum; " : chkStool ? "Stool; " : chkUrine ? "Urine; " :
                chkSwab ? "Swab; " : chkSemen ? "Semen; " : chkHair ? "Hair; " : !string.IsNullOrWhiteSpace(otherText) ?
                "Others - (" + otherText.Trim() + ")" : "";

            vm.REPORTS.edtallergies = "=> Sample Taken : " + xchecked + " - by " + sampleCollectedBy + "  @  " +
                collectionDate.Date + " " + collectionDate.ToShortTimeString();

            vm.REPORTS.edtspinstructions = vm.REPORTS.edtallergies;

            //btnsave.Enabled = true;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult frmAcctfromSuspClosed(string crossRef, string sampleCollectedBy, string patientNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string[] taggedFrmSuspensea_ = new string[10];
            DataTable servicetagged = SUSPENSE.GetSUSPENSE(crossRef, "U");

            taggedFrmSuspensea_ = msmrfunc.mrGlobals.taggedFromSuspensea_;

            for (int i = 0; i < servicetagged.Rows.Count; i++)
            {
                if (msmrfunc.mrGlobals.taggedFromSuspensea_[i] == "YES") //tagged
                    vm.REPORTS.edtallergies += servicetagged.Rows[i]["description"].ToString().Trim() + ","; //for txtrequestprofiles.Text
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        string getgrouphead(string xgrouphead, string xghgroupcode, string xtype)
        {
            Customer customers = new Customer();
            patientinfo patients = new patientinfo();

            string xreturnvalue = "";
            if (xtype == "P")
            {
                patients = patientinfo.GetPatient(xgrouphead, xghgroupcode);
                if (patients == null)
                {
                    //msgeventtracker = "g";
                    vm.REPORTS.ActRslt = "Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS";
                    xreturnvalue = "Abort";
                }
            }
            else
            {
                customers = Customer.GetCustomer(xgrouphead);
                if (customers == null)
                {
                    vm.REPORTS.ActRslt = "Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS";
                    xreturnvalue = "Abort";
                }
            }

            if (xreturnvalue != "Abort")
            {
                xreturnvalue = (xtype == "P" && xgrouphead == patients.patientno) ?
                    "< SELF >" : (xtype == "C") ? customers.Name : patients.name;
            }

            return xreturnvalue;
        }

        public JsonResult returnCrossRefFocusout(string crossRef, string sampleCollectedBy, string patientNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string woperator = Request.Cookies["mrName"].Value;
            DataTable servicetagged = SUSPENSE.GetSUSPENSE(crossRef, "U");

            if (servicetagged.Rows.Count > 0 && string.IsNullOrWhiteSpace(patientNo))
            {
                vm.REPORTS.cbogender = servicetagged.Rows[0]["sex"].ToString();
                vm.REPORTS.cboAge = servicetagged.Rows[0]["age"].ToString();
                vm.REPORTS.txtaddress1 = servicetagged.Rows[0]["address"].ToString();

                vm.REPORTS.chkbyacctofficers = true; //flag for txtgroupcode.Enabled = txtpatientno.Enabled = false;
            }

            if (string.IsNullOrWhiteSpace(sampleCollectedBy))
            {
                vm.REPORTS.REPORT_TYPE4 = woperator; //for txtsamplecollectedBy.Text
                //dtcollectiondate.Value = DateTime.Now;
            }


            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        bool displaypatdetailsSample(string xreference, string sampleCollectedBy)
        {
            Admrecs admrec = new Admrecs();
            Mrattend mrattend = new Mrattend();
            billchaindtl bchain = new billchaindtl();
            Customer customers = new Customer();
            string woperator = Request.Cookies["mrName"].Value;
            DataTable servicetagged = SUSPENSE.GetSUSPENSE(xreference, "U");

            bool foundit = true;
            string xtranstype = xreference.Substring(0, 1);

            if (xtranstype == "A") //admissions
            {
                admrec = Admrecs.GetADMRECS(xreference);

                if (admrec == null)
                {
                    foundit = false;
                    vm.REPORTS.alertMessage = "Invalid Admissions Reference... ";
                    return false;
                }

                vm.REPORTS.chkApplyFilter = true;  //for chkInpatient.Checked
            }
            else if (xtranstype == "C" || xtranstype == "S")
            {
                mrattend = Mrattend.GetMrattend(xreference);
                if (mrattend == null)
                {
                    foundit = false;
                    vm.REPORTS.alertMessage = "Unable to Link this Consultation Reference in Daily Attendance Register... ";
                    return false;
                }
                //dtTrans_date.Value = mrattend.TRANS_DATE;
            }
            else
            {
                foundit = false;
                vm.REPORTS.alertMessage = "Invalid Number Format for Consultation/Admission Reference...";
                return false;
            }

            if (!foundit)
            {
                //txtcrossref.Select();
                return false;
            }

            vm.REPORTS.mreference = xreference;
            vm.REPORTS.mgrouphtype = (xtranstype == "A") ? admrec.GROUPHTYPE : mrattend.GROUPHTYPE;
            vm.REPORTS.TXTPATIENTNAME = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;
            vm.REPORTS.txtgroupcode = (xtranstype == "A") ? admrec.GROUPCODE : mrattend.GROUPCODE;
            vm.REPORTS.txtpatientno = (xtranstype == "A") ? admrec.PATIENTNO : mrattend.PATIENTNO;
            vm.REPORTS.REPORT_TYPE2 = (xtranstype != "A") ? mrattend.REFERRER : admrec.DOCTOR; //for  txtreferringdoctor.Text
            //txtBills.Text = labdet.AMOUNT.ToString("N2");
            //Combillspayable.Text = (string.IsNullOrWhiteSpace(txtpatientno.Text) || txtpatientno.Text == txtgrouphead.Text) ? "SELF" : mgrouphtype == "P" ? "Another Patient" : "Corporate Client";
            vm.REPORTS.txtghgroupcode = (xtranstype == "A") ? admrec.GHGROUPCODE : mrattend.GHGROUPCODE;
            vm.REPORTS.txtgrouphead = (xtranstype == "A") ? admrec.GROUPHEAD : mrattend.GROUPHEAD;
            vm.REPORTS.chkApplyFilter = xtranstype == "A" ? true : false; //for chkInpatient.Checked

            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno))
            {
                vm.REPORTS.txtothername = getgrouphead(vm.REPORTS.txtgrouphead, vm.REPORTS.txtghgroupcode, vm.REPORTS.mgrouphtype); //for txtGroupheadName.Text

                if (vm.REPORTS.txtgrouphead.Trim() == "Abort")
                {
                    //txtReference.Select();
                    return false;
                }
            }
            else
            {
                vm.REPORTS.txtothername = "< SELF >";
            }

            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno))
            {
                bchain = Getbillchain(vm.REPORTS.txtpatientno, vm.REPORTS.txtgroupcode);

                if (bchain == null)
                {
                    vm.REPORTS.alertMessage = "Critical Error in Patient Master File... Call Technical Support";
                    return false;
                }

                vm.REPORTS.cbogender = bchain.SEX;
                vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
                vm.REPORTS.cboAge = bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now.Date);
                //PicSelected = bchain.PICLOCATION;

                //if (!string.IsNullOrWhiteSpace(PicSelected))
                //{
                //    pictureBox1.Image = WebGUIGatway.getpicture(PicSelected);
                //}
            }

            if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno) && bchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(bchain.HMOSERVTYPE))
            {
                //for customer.hmo  - 30-09-2012
                HmoAuthorizations hmoauthorization = HmoAuthorizations.GetHMOAUTHORIZATIONS(xreference, bchain.GROUPCODE, bchain.PATIENTNO);

                if (hmoauthorization != null && string.IsNullOrWhiteSpace(hmoauthorization.AUTHORIZEDCODE))
                {
                    vm.REPORTS.alertMessage = "There is a pending Treatment Authorization Request on this patient for this Reference : " + xreference.Trim();
                    return false;
                }
            }

            if (xreference.Substring(0, 1) != "A" && !string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno) && bchain.GROUPHTYPE == "C" &&
                customers.Tosignbill) //13-04-2013 check if outpatient and patient is to sing bill - 
            {
                BSDETAIL bsdetail = BSDETAIL.GetBSDETAIL(xreference);

                if (bsdetail != null && !bsdetail.SIGNEDBILL)
                {
                    vm.REPORTS.alertMessage = "This Patient is required TO SIGN BILL before service on this Reference : " + xreference.Trim() + " can be received...";
                    return false;
                }
            }

            //check for investigations request in suspense and get details to acctfromsusp form for tagging
            //  displayallrequests();

            if (servicetagged.Rows.Count > 0)
            {
                vm.REPORTS.chkLoyaltyCustomers = true; //A flag;

                //frmAcctfromSusp FrmacctFromsusp = new frmAcctfromSusp(servicetagged, txtcrossref.Text.Trim() + 
                //    "  : " + txtfullname.Text.Trim() + " : " + txtgroupcode.Text.Trim() + "-" + txtpatientno.Text.Trim());

                //FrmacctFromsusp.Closed += new EventHandler(FrmacctFromsusp_Closed);
                //FrmacctFromsusp.ShowDialog();
            }
            else
            {
                vm.REPORTS.chkComparativereport = true;
                return true;
                //vm.REPORTS.alertMessage = "No Selected or Pending Service on this Reference..." + "\n" + "for This Service Centre... Pls Confirm to Continue... ";
                
                //if (result == DialogResult.No)
                //{
                //    return false;
                //}
            }

            if (servicetagged.Rows.Count > 0 && string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno))
            {
                vm.REPORTS.cbogender = servicetagged.Rows[0]["sex"].ToString();
                vm.REPORTS.cboAge = servicetagged.Rows[0]["age"].ToString();
                vm.REPORTS.txtaddress1 = servicetagged.Rows[0]["address"].ToString();

                vm.REPORTS.chkbyacctofficers = true; //flag for txtgroupcode.Enabled = txtpatientno.Enabled = false;
            }

            if (string.IsNullOrWhiteSpace(sampleCollectedBy))
            {
                vm.REPORTS.REPORT_TYPE4 = woperator; //for txtsamplecollectedBy.Text
                //dtcollectiondate.Value = DateTime.Now;
            }

            return true;
        }

        public JsonResult crossRefFocusout(string crossRef, string sampleCollectedBy)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            //suspense = SUSPENSE.GetSUSPENSE(crossRef, "A");

            if (!displaypatdetailsSample(crossRef, sampleCollectedBy))
            {
                vm.REPORTS.REPORT_TYPE1 = "";   //txtcrossref.Text = "";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult crossRefFocus(string facility, string msection)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (new string[] { "3", "4", "6" }.Contains(msection)) //nurses/docs/lab/xray/ecg/thearthre
            {
                DataTable dt = getLinkDetails("", 0, 0m, 0m, facility, true, msection, 0, "", "");

                if (dt.Rows.Count > 0)
                {
                    vm.REPORTS.chkADVCorporate = true;

                    //frmGetlinkinfo linkinfo = new frmGetlinkinfo(dt);
                    //linkinfo.ShowDialog();
                    //txtcrossref.Text = Anycode = msmrfunc.mrGlobals.anycode;
                    //txtcrossref.Select();
                }

            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult facilityFocusout(string facility)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DataTable mrcontrol = Dataaccess.GetAnytable("", "MR", "select * from mrcontrol order by recid", false);

            bool foundit = false;
            int servicerecno = 0;

            for (int i = 0; i < mrcontrol.Rows.Count; i++)
            {
                if (mrcontrol.Rows[i]["MPASS"].ToString().Trim() == facility.Trim())
                {
                    foundit = true;
                    servicerecno = i;
                    break;
                }
            }

            if (!foundit)
            {
                vm.REPORTS.alertMessage = "This Service Centre has not been properly configured...";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            servicerecno = 5;
            vm.REPORTS.nmrAmountTo = servicerecno;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        #endregion
        //SampleCollectDetails end



        //PatientPaymentMaintenance Start
        #region

        void linkdetails(string anycode, bool isadmissions, string mpatientno, string mgroupcode, string mname, 
            string consultRefNo, string msection, string mfacility1)
        {
            if (isadmissions)
            {
                return;
            }

            DataTable servicetagged = SUSPENSE.GetSUSPENSE(consultRefNo, "U");
            bool ismeddiag = bissclass.sysGlobals.ismeddiag;

            string woperator = Request.Cookies["mrName"].Value;
            string updatestring = "";
            int xcount = (servicetagged.Rows.Count > 0) ? servicetagged.Rows.Count : 1;
            string[] tempa_ = new string[xcount];
            string xfacility;
            string start_time = DateTime.Now.ToString("HH:mm:ss");
            string mfacility = mfacility1;

            for (int i = 0; i < servicetagged.Rows.Count; i++)
            {
                if (msmrfunc.mrGlobals.taggedFromSuspensea_[i] == "YES") //tagged
                {
                    xfacility = servicetagged.Rows[i]["facility"].ToString();
                    if (i > 0 && tempa_.Contains(xfacility))
                        continue;
                    tempa_[i] = xfacility;
                }
            }

            if (servicetagged.Rows.Count < 1)
            {
                tempa_[0] = mfacility;
            }

            for (int i = 0; i < tempa_.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(tempa_[i]))
                {
                    LINK.WriteLINK(0, mgroupcode, mpatientno, mname, anycode.Substring(0, 1), consultRefNo, 
                        0m, 0m, tempa_[i], true, "", false, 0, "", msection, woperator);

                    LINK3.WriteLINK3(mgroupcode, mpatientno, DateTime.Now, mname, "INV/PROC - " + tempa_[i],
                        consultRefNo, DateTime.Now.ToLongTimeString(), start_time, "6", tempa_[i], 
                        start_time, woperator); //attendance monitor
                                                                                                                                                                                                                           /*       IF !EMPTY(tempa_[x])
                                                                                                                                                                                                                           writemonitor(mgroupcode,mpatientno,date(),mcrossref,"INV/PROC - "+tempa_[x],mname,TIME(),"","I",tempa_[x]) */
                }
            }

            if ((anycode.Substring(0, 1) == "6" || anycode.Substring(0, 1) == "8") && !ismeddiag) //cashier selected pharmacy/lab/xray/scan
            {
                //update link ok
                updatestring = "UPDATE LINK SET LINKOK = '1' WHERE reference = '" + consultRefNo + "' and linkok = '0' and (tosection = '8' or tosection = '6') ";
                bissclass.UpdateRecords(updatestring, "MR");
                // LINK.updateLinkOk(combConsAdmNo.Text, "8", dtValueDate.Value, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), true, true);
            }

            //update received date in link
            //multiple records are usually sent by docs and PCP TO CASHIER
            updatestring = "UPDATE LINK SET LINKOK = '1', DATEREC = '" + DateTime.Now.ToShortDateString() + "', TIMEREC = '" + DateTime.Now.ToLongTimeString() + "' WHERE reference = '" + consultRefNo + "' and tosection = '2' "; //and linkok = '0'
            bissclass.UpdateRecords(updatestring, "MR");
            // LINK.updateLinkOk(combConsAdmNo.Text, "2", DateTime.Now.Date, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("HH:mm:ss"), true, true);
        }


        public JsonResult paylinkinfoSubmit(string xselect, bool isadmissions, string mpatientno, string mgroupcode,
            string mname, string consultRefNo, string msection, string mfacility1)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            string anycode = xselect;

            //    mfacility = msmrfunc.mrGlobals.mfacility;
            //    mrecid = msmrfunc.mrGlobals.recid;

            linkdetails(anycode, isadmissions, mpatientno, mgroupcode, mname, consultRefNo, msection, mfacility1);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult receiptRefNoFocusout(string receiptRefNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            //ds.Clear();
            //ds.Tables.Clear();

            DataTable dtpay = Dataaccess.GetAnytable("", "MR", "SELECT * FROM PAYDETAIL WHERE rtrim(REFERENCE) = '" + receiptRefNo.Trim() + "'", false);

            if (dtpay.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Invalid Payment Reference...";
                //txtReference.Text = "";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.txtgroupcode = dtpay.Rows[0]["groupcode"].ToString();
            vm.REPORTS.txtpatientno = dtpay.Rows[0]["patientno"].ToString();
            vm.REPORTS.REPORT_TYPE1 = dtpay.Rows[0]["description"].ToString(); //for paydesc
            vm.REPORTS.TXTPATIENTNAME = dtpay.Rows[0]["name"].ToString();
            vm.REPORTS.txtdiscount = (decimal)dtpay.Rows[0]["amount"]; //for nmrAmount.Value
            var transDate = (DateTime)dtpay.Rows[0]["trans_date"]; //datefrom = dateto
            vm.REPORTS.REPORT_TYPE2 = string.Format("{0:yyyy-MM-dd}", transDate);

            vm.REPORTS.chkADVCorporate = true; //for isdataset

            //ds.Tables.Add(dtpay);
            vm.REPORTS.chkbillregistration = true; //dtpay > 0 flag

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ledgerBtnClicked(string consultRefNo, string mreference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select trans_date, groupcode, patientno, grouphead, name from mrattend where reference = '" + consultRefNo + "'", false);

            if (dt.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Unable to Link Reference : " + mreference + " to Patients Transacitons...";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            DataRow row = dt.Rows[0];
            if (row["patientno"].ToString() != row["grouphead"].ToString())
            {
                vm.REPORTS.alertMessage = "Patient is not an account holder (Grouphead)... Can't print Ledger";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            string xstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.ITEMNO, BILLING.DESCRIPTION, BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD, BILLING.SERVICETYPE, BILLING.GROUPCODE, BILLING.TTYPE, BILLING.GHGROUPCODE, BILLING.EXTDESC, BILLING.ACCOUNTTYPE, CHAR(50) AS GHNAME, 0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT FROM BILLING WHERE BILLING.TRANS_DATE >= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + "' AND BILLING.TRANS_DATE <= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + " 23:59:59.999' and BILLING.groupcode = '" + row["groupcode"].ToString() + "' and BILLING.patientno = '" + row["patientno"].ToString() + "' UNION ALL SELECT PAYDETAIL.REFERENCE, PAYDETAIL.PATIENTNO, PAYDETAIL.NAME, PAYDETAIL.ITEMNO, PAYDETAIL.DESCRIPTION, PAYDETAIL.AMOUNT, PAYDETAIL.TRANS_DATE, PAYDETAIL.TRANSTYPE, PAYDETAIL.GROUPHEAD, PAYDETAIL.SERVICETYPE, PAYDETAIL.GROUPCODE, PAYDETAIL.TTYPE, PAYDETAIL.GHGROUPCODE, PAYDETAIL.EXTDESC, PAYDETAIL.ACCOUNTTYPE, CHAR(50) AS GHNAME, 0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT FROM PAYDETAIL WHERE PAYDETAIL.TRANS_DATE >= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + "' AND PAYDETAIL.TRANS_DATE <= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + " 23:59:59.999' and PAYDETAIL.groupcode = '" + row["groupcode"].ToString() + "' and PAYDETAIL.patientno = '" + row["patientno"].ToString() + "' UNION ALL SELECT BILL_ADJ.REFERENCE, CHAR(5) AS PATIENTNO, BILL_ADJ.NAME, 0 AS ITEMNO, RTRIM(BILL_ADJ.ADJUST)+' '+BILL_ADJ.COMMENTS AS DESCRIPTION, BILL_ADJ.AMOUNT, BILL_ADJ.TRANS_DATE, BILL_ADJ.TRANSTYPE, BILL_ADJ.GROUPHEAD, CHAR(1) AS SERVICETYPE, CHAR(9) AS GROUPCODE, BILL_ADJ.TTYPE, BILL_ADJ.GHGROUPCODE, CHAR(5) AS EXTDESC, CHAR(5) AS ACCOUNTTYPE, CHAR(50) AS GHNAME, 0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT FROM BILL_ADJ WHERE BILL_ADJ.TRANS_DATE >= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + "' AND BILL_ADJ.TRANS_DATE <= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + " 23:59:59.999' and BILL_ADJ.ghgroupcode = '" + row["groupcode"].ToString() + "' and BILL_ADJ.GROUPHEAD = '" + row["patientno"].ToString() + "'";

            //DialogResult result = MessageBox.Show("OUTPUT TO SCREEN<yes>, TO PRINTER<no>, CANCEL", "Patient's Ledger On POS Format", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            //if (result == DialogResult.Cancel)
            //    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);

            //bool isprint = result == DialogResult.No ? true : false;

            bool isprint = true;

            DataSet ds = new DataSet();
            dt = Dataaccess.GetAnytable("", "MR", xstring, false);

            foreach (DataRow xr in dt.Rows)
            {
                if (xr["ttype"].ToString() == "D")
                    xr["debit"] = Convert.ToDecimal(xr["amount"]);
                else
                    xr["credit"] = Convert.ToDecimal(xr["amount"]);
            }

            ds.Tables.Add(dt);
            Session["rdlcFile"] = "POSStatement.rdlc";
            Session["sql"] = "";
            string mrptheader = "POS Statement/RECEIPT GENERATOR ";
            string rptfooter = "", rptcriteria = "";

            if (!isprint)
            {
                //MSMR.Forms.frmReportViewer receipt = new MSMR.Forms.frmReportViewer(mrptheader, mrptheader, rptfooter, rptcriteria, "", "POS", mreference, 0m, "", "", "", ds, true, 0, Convert.ToDateTime(row["trans_date"]), Convert.ToDateTime(row["trans_date"]), "", isprint, "", woperator);
                //receipt.Show();
            }
            else
            {
                //MSMR.MRrptConversion.GeneralRpt(mrptheader, mrptheader, rptfooter, "", "", "POS", mreference, 0M, "", "", "", ds, 0, Convert.ToDateTime(row["trans_date"]), Convert.ToDateTime(row["trans_date"]), "", isprint, true, "", woperator);
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        private DataTable getLinkDetails(string reference, int recno, decimal cumbil, decimal cumpay, string facility, 
            bool linkok, string msection, int procedure, string admflag, string doctor)
        {
            msmrfunc.mrGlobals.anycode = "";
            string global_clinic_code = msmrfunc.mrGlobals.global_clinic_code;
            int nwseclevel = msmrfunc.mrGlobals.nwseclevel;
            bool isphlebotomy = msmrfunc.mrGlobals.isphlebotomy;
            facility = facility.Trim();

            string date = string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);

            string selcommand = "SELECT NAME, GROUPCODE, PATIENTNO, TIMESENT, REFERENCE, OPERATOR, FACILITY, " +
            "PROCFUNC, SENDEXCL, DOCTOR, LINKOK, tosection,recid,cumbil,transflag FROM LINK WHERE DATEREC = '' AND TRANS_DATE = '" +
            date + "' AND TOSECTION LIKE '[" + msection + "]' AND REFERENCE != '' ORDER BY TIMESENT";
            
            //EMPTY TIMEREC AND
            //removed 17.01.2018 from dtlink loop - string.IsNullOrWhiteSpace(global_clinic_code) &&
            //       !string.IsNullOrWhiteSpace(xfacility) && !global_clinic_check(xfacility) ||

            DataRow row = null;
            string xfacility, xname, xdoctor, xtosection; int procfunc; bool xsendexcl, xlinkok;
            DataTable dtlink = Dataaccess.GetAnytable("", "MR", selcommand, false);
            DataTable dtrtn = new DataTable();

            if (dtlink.Rows.Count > 0) //create return table
            {
                //dtrtn = new DataTable();
                dtrtn = dtlink.Clone();
            }

            for (int i = 0; i < dtlink.Rows.Count; i++)
            {
                row = dtlink.Rows[i];
                xname = row["Name"].ToString();
                xfacility = row["facility"].ToString().Trim();
                xdoctor = row["doctor"].ToString().Trim();
                procfunc = Convert.ToInt32(row["procfunc"]);
                xsendexcl = Convert.ToBoolean(row["sendexcl"]);
                xtosection = row["tosection"].ToString().Trim();
                xlinkok = Convert.ToBoolean(row["linkok"]);

                if (xtosection != "2" && !string.IsNullOrWhiteSpace(facility) && xfacility != facility ||
                    !string.IsNullOrWhiteSpace(global_clinic_code) && xfacility != global_clinic_code || xtosection != "2" && procedure > 0 && procfunc != procedure || string.IsNullOrWhiteSpace(xname) || xsendexcl && !string.IsNullOrWhiteSpace(doctor) && xdoctor != doctor ||
                    msection == "6" && xtosection == "6" && nwseclevel == 1 && xlinkok ||
                    msection == "6" && xtosection == "6" && nwseclevel != 1 && isphlebotomy && !xlinkok || !string.IsNullOrWhiteSpace(admflag) && admflag != row["transflag"].ToString().Trim()) //sample not taken
                {
                    // dtlink.Rows.Remove(row);
                    continue;
                }
                //dtTableNew.ImportRow(drtableOld);
                dtrtn.ImportRow(row);
            }

            // if (dtlink.Rows.Count < 1)
            if (dtrtn.Rows.Count < 1)
            {
                string xlocation =
                    (msection == "3") ? "Nurses Station/Desk" : (msection == "1") ? "Registration Desk" :
                    (msection == "2") ? "Cashier/Payment Desk" : (msection == "4") ? "Consultation(Doctors)" :
                    (msection == "5") ? "Ward/Process Desk" : (msection == "6") ? "Lab/Xray/Scan/Theatre" :
                    (msection == "7") ? "Billing Office" : (msection == "8") ? "Pharmacy" :
                    (msection == "9") ? "Paediatrics" : (msection == "A") ? "Admissions" : "";

                vm.REPORTS.alertMessage = "No Patient Awaiting Service . . . ";
            }
            
            //return dtlink;
            return dtrtn;
        }


        public JsonResult reLoadBtnClicked(string consultRefNo, string msection)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            //anycode = msmrfunc.mrGlobals.anycode = mfacility = msmrfunc.mrGlobals.mfacility = admdepositflag = msmrfunc.mrGlobals.admflag = "";
            //mrecid = msmrfunc.mrGlobals.recid = 0;
           
            //DataTable dt = msmrfunc.getLinkDetails(combConsAdmNo.Text, 0, 0m, 0m, "", true, msection, 2, "", "");
            DataTable dt = getLinkDetails(consultRefNo, 0, 0m, 0m, "", true, "12", 2, "", "");

            if (dt.Rows.Count > 0)
            {
                //frmGetlinkinfo linkinfo = new frmGetlinkinfo(dt);
                
                //linkinfo.ShowDialog();
                //combConsAdmNo.Text = anycode = msmrfunc.mrGlobals.anycode;
                //admdepositflag = msmrfunc.mrGlobals.admflag;
                //mrecid = msmrfunc.mrGlobals.recid;
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult paymentDeleteBtnClicked(bool newrec, string paymentNo, string groupCode, string mpatientno)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DataTable paydetail = PAYDETAIL.GetPAYDETAIL(groupCode, mpatientno, true, paymentNo);

            if (paydetail.Rows.Count > 0)
            {
                if (!newrec && (bool)paydetail.Rows[0]["POSTED"])
                {
                    vm.REPORTS.alertMessage = "Record Can't be Deleted...";
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }
            }
                
            // msgeventtracker = "PD";
            //DialogResult result = MessageBox.Show("Confirm to Delete Record", "Payment Details", MessageBoxButtons.YesNo,
            //   MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.Yes)
            //{
            //    PAYDETAIL.DeletePay(paymentNo);
            //    //combPmtNo.Focus();
            //    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            //}

            PAYDETAIL.DeletePay(paymentNo);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult PaymentSubmitClicked(MR_DATA.REPORTS dataObject)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS = dataObject;
            vm.REPORTS.dtbirthdate = Convert.ToDateTime(vm.REPORTS.REPORT_TYPE1);
            vm.REPORTS.dtregistered = Convert.ToDateTime(vm.REPORTS.REPORT_TYPE2);

            string woperato = Request.Cookies["mrName"].Value;

            TransactionsPayments formObject = new TransactionsPayments(woperato, vm);

            vm.REPORTS = formObject.btnSave_Click();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult receivedDateFocusout(string receivedDate)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DateTime dtmin_date = DateTime.Now;
            DateTime receivedDate1 = Convert.ToDateTime(receivedDate);

            DataTable dtcontrol = Dataaccess.GetAnytable("", "MR", "SELECT locstate,localcur, ta_start, pyear, tp_period, seclink, installed from mrcontrol order by recid", false);
            msmrfunc.mrGlobals.mlastperiod = Convert.ToInt32(dtcontrol.Rows[0]["tp_period"]);
            msmrfunc.mrGlobals.mpyear = Convert.ToInt32(dtcontrol.Rows[0]["pyear"]);

            if(receivedDate1.Date >= DateTime.Now.Date)
            {
                if (receivedDate1.Date > DateTime.Now.Date ||
                    !bissclass.checkperiod(receivedDate1.Date, msmrfunc.mrGlobals.mlastperiod, msmrfunc.mrGlobals.mpyear, dtmin_date.Date))
                {
                    vm.REPORTS.REPORT_TYPE5 = string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult valueDateFocusout(string valueDate, string paymentMode )
        {
            vm.REPORTS = new MR_DATA .REPORTS();

            DateTime dtmin_date = DateTime.Now;
            DateTime valueDate1 = Convert.ToDateTime(valueDate);

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select pauto, activeglint, glbatchno, gldocument, glcompany, cashpoint, attendlink, dischtime, name, DTFORMAT from mrcontrol order by recid", false);
            bool disallowbackdate = (bool)dt.Rows[2]["cashpoint"];
            //Session["disallowbackdate"] = disallowbackdate;

            DataTable dtcontrol = Dataaccess.GetAnytable("", "MR", "SELECT locstate,localcur, ta_start, pyear, tp_period, seclink, installed from mrcontrol order by recid", false);
            msmrfunc.mrGlobals.mlastperiod = Convert.ToInt32(dtcontrol.Rows[0]["tp_period"]);
            msmrfunc.mrGlobals.mpyear = Convert.ToInt32(dtcontrol.Rows[0]["pyear"]);

            //vm.MRCONTROLS = ErpFunc.RsGet<MR_DATA.MRCONTROL>("MR_DATA", "SELECT locstate,localcur, ta_start, pyear, tp_period, seclink, installed from mrcontrol order by recid");

            //foreach(var mrcontrol in vm.MRCONTROLS)
            //{
            //    msmrfunc.mrGlobals.mlastperiod = (int)mrcontrol.TP_PERIOD;
            //    msmrfunc.mrGlobals.mpyear = (int)mrcontrol.PYEAR;
            //    break;
            //}

            vm.REPORTS.chkApplyFilter = disallowbackdate;

            if (valueDate1.Date < DateTime.Now.Date && !disallowbackdate)
            {
                // msgeventtracker = "OVERWRITE";
                vm.REPORTS.alertMessage = "Transactions not allowed on/or before  :  " + DateTime.Now.Date.AddDays(-1).ToString() + "\n\n ...Overwrite Authorization required !  CONTINUE... ?";

                //return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);

                //frmOverwrite overwrite = new frmOverwrite("Overwrite to Date Control", "MRSTLEV", "MR");
                //overwrite.ShowDialog();
                //if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode)) //not successful
                //{
                //    combConsAdmNo.Text = anycode = "";
                //    combConsAdmNo.Focus();
                //    return;
                //}
            }

            if (valueDate1.Date <= DateTime.Now.Date && !bissclass.checkperiod(valueDate1.Date, msmrfunc.mrGlobals.mlastperiod, msmrfunc.mrGlobals.mpyear, dtmin_date.Date))
            {
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else if (paymentMode.Trim() == "CHEQUE" && valueDate1 > DateTime.Now.Date)
            {
                vm.REPORTS.ActRslt = "Post-Dated Cheque ? Confirm to Continue...";
            }
            else
            {
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }


            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult bankFocusout()
        {
            DataTable atmprofile = ATMPROFILE.GetATMPROFILE();
            vm.ATMPROFILES = ErpFunc.ConvertDtToList<MR_DATA.ATMPROFILE>(atmprofile);

            return Json(vm.ATMPROFILES, JsonRequestBehavior.AllowGet);
        }


        public JsonResult amountFocused( string consultRefNo, bool newrec, string groupCode, 
            string paymentNo, string mpatientno)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Mrattend mrattend = new Mrattend();
            Admrecs admrec = new Admrecs();

            admrec = Admrecs.GetADMRECS(consultRefNo);
            mrattend = Mrattend.GetMrattend(consultRefNo);

            DataTable paydetail = PAYDETAIL.GetPAYDETAIL(groupCode, mpatientno, true, paymentNo);

            //nmrAmount.Select(0, nmrAmount.Text.Length);

            //   nmrAmount.Value = lblpayabledbcr.Text == "CR" ? 0 : nmrPayable.Value;
            //if (string.IsNullOrWhiteSpace(combConsAdmNo.Text) && mcusttype == "C")
            //    pancapitations.Visible = true;

            if (!newrec)
            {
                if (paydetail.Rows.Count > 0)
                    vm.REPORTS.txtdiscount = (decimal)paydetail.Rows[0]["amount"];
            }

            //if (lbladmissiondeposit.Visible && nmrAdmissionDeposit.Value > 0m)
            //{
            //    //  nmrAmount.Value += nmrAdmissionDeposit.Value;
            //    txtDetails.Text = "Admissions Deposit";
            //}

            if (!string.IsNullOrWhiteSpace(consultRefNo) && consultRefNo.Trim().Length > 0)
            {
                if (consultRefNo.Substring(0, 1) == "A")
                    vm.REPORTS.TXTPATIENTNAME = admrec.NAME.Trim() + " : " + admrec.PATIENTNO.Trim() + ":" + admrec.GROUPCODE.Trim() + ":" + admrec.REFERENCE.Trim();
                else
                    vm.REPORTS.TXTPATIENTNAME = mrattend.NAME.Trim() + " : " + mrattend.PATIENTNO.Trim() + ":" + mrattend.GROUPCODE.Trim() + ":" + mrattend.REFERENCE.Trim();
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult clientCodeFocusOut(string clientCode, string groupCode, bool chkGetBal)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            Customer customers = new Customer();
            patientinfo patients = new patientinfo();
            patientinfo patgrphead = new patientinfo();

            patients = patientinfo.GetPatient(clientCode, groupCode);

            if (!string.IsNullOrWhiteSpace(groupCode))
            {
                patgrphead = patientinfo.GetPatient(clientCode, groupCode);
                vm.REPORTS.mcusttype = "P";
            }
            else
            {
                customers = Customer.GetCustomer(clientCode);
                vm.REPORTS.mcusttype = "C";
            }

            if (vm.REPORTS.mcusttype == "P" && patgrphead == null || vm.REPORTS.mcusttype == "C" && customers == null)
            {
                vm.REPORTS.alertMessage = "Invalid GroupHead Specification as responsible for Bills";
                
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            // this.DisplayPatients();
            vm.REPORTS.TXTPATIENTNAME = (vm.REPORTS.mcusttype == "P") ? patgrphead.name : customers.Name;
            vm.REPORTS.mgrouphead = (vm.REPORTS.mcusttype == "P") ? patients.patientno : customers.Custno;

            if (vm.REPORTS.mcusttype == "P" && !patgrphead.isgrouphead)
            {
                vm.REPORTS.alertMessage = "Specified Patient is not a registered GroupHead...";
                
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.mgroupcode = "";
            vm.REPORTS.mpatientno = "";

            if (vm.REPORTS.mcusttype == "P")
            {
                if (chkGetBal) {
                    vm.REPORTS.chkByBranch = true;
                    vm.REPORTS.txtgroupcode = groupCode;
                    vm.REPORTS.REPORT_TYPE1 = clientCode;

                    calc_op_bal();

                }// 12-12-2013

                vm.REPORTS.mgroupcode = patgrphead.groupcode;
                vm.REPORTS.mpatientno = patgrphead.patientno;

                //panPVTDeposit.Visible = true;
                vm.REPORTS.chkAuditProfile = true;

                //if (!string.IsNullOrWhiteSpace(patgrphead.piclocation))
                //{
                //    pictureBox1.Visible = true;
                //    pictureBox1.Image = WebGUIGatway.getpicture(patgrphead.piclocation);
                //}
            }
            else
            {
                vm.REPORTS.chkApplyFilter = true; //for pancapitations.Visible
                //pictureBox1.Visible = false;
            }

            vm.REPORTS.chkADVCorporate = true;  //for chknone.Checked

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult paymentNoFocusout(string paymentNo, decimal mlastno, string groupCode,
            string mpatientno, string msection)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            string mbr_cc = bissclass.sysGlobals.mbr_cc;
            string woperator = Request.Cookies["mrName"].Value;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select pauto, activeglint, glbatchno, gldocument, glcompany, cashpoint, attendlink, dischtime, name, DTFORMAT from mrcontrol order by recid", false);
            bool mpayauto = (bool)dt.Rows[0]["pauto"];

            dt = Dataaccess.GetAnytable("", "MR", "select CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);
            bool mcanalter = (bool)dt.Rows[0]["canalter"];

            if (mpayauto && bissclass.IsDigitsOnly(paymentNo.Trim()) && Convert.ToInt32(paymentNo) > mlastno)
            {
                vm.REPORTS.alertMessage = "Payment Reference is out of Sequence...";
                //combPmtNo.Text = "";
                //combPmtNo.Select();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            if (mpayauto && bissclass.IsDigitsOnly(paymentNo.Trim()))
            {
                vm.REPORTS.REPORT_TYPE1 = bissclass.autonumconfig(paymentNo, true, (string.IsNullOrWhiteSpace(mbr_cc)) ? "" : mbr_cc, "999999999"); //for combPmtNo.Text
            }

            if(vm.REPORTS.REPORT_TYPE1 != null)
                vm.REPORTS.mreference = vm.REPORTS.REPORT_TYPE1;
            
            //anycode = anycode1 = "";
            vm.REPORTS.newrec = true;

            DataTable paydetail = PAYDETAIL.GetPAYDETAIL(groupCode, mpatientno, true, paymentNo);

            
            if (paydetail.Rows.Count > 0)
            {
                vm.REPORTS.cmdgrpmember = true; //A flag
                vm.REPORTS.newrec = false;
                vm.REPORTS.txtdiscount = (decimal)paydetail.Rows[0]["amount"]; //for amount
                vm.REPORTS.TXTPATIENTNAME = paydetail.Rows[0]["name"].ToString();
                vm.REPORTS.REPORT_TYPE2 = paydetail.Rows[0]["paytype"].ToString(); //for combPmtMode.Text
                vm.REPORTS.REPORT_TYPE3 = paydetail.Rows[0]["DESCRIPTION"].ToString(); //for txtDetails.Text
                var valueDate = (DateTime)paydetail.Rows[0]["TRANS_DATE"];
                var receivedDate = (DateTime)paydetail.Rows[0]["DATERECEIVED"];
                vm.REPORTS.REPORT_TYPE4 = string.Format("{0:yyyy-MM-dd}", valueDate);
                vm.REPORTS.REPORT_TYPE5 = string.Format("{0:yyyy-MM-dd}", receivedDate);

                vm.REPORTS.mreference = paydetail.Rows[0]["reference"].ToString();

                if (msection == "7" || msection == "C") //billing
                {
                    //ok
                }
                else
                {
                    vm.REPORTS.alertMessage = "Record Exist...and Further access Denied !";
                    //combPmtNo.Text = "";
                    // combPmtNo.Select();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                if ((bool)paydetail.Rows[0]["posted"])
                {
                    vm.REPORTS.alertMessage = "Record Exist ... AND IT CANNOT BE AMENED !";
                    //combPmtNo.Text = "";
                    // combPmtNo.Select();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                if (msection == "C" && paydetail.Rows[0]["operator"].ToString().Trim() != woperator)
                {
                    vm.REPORTS.alertMessage = "Further Access Denied... CURRENT USER AND RECORD SIGNATURE CONFLICT!!!";
                    //combPmtNo.Text = "";
                    // combPmtNo.Select();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.cmbdelete = mcanalter ? true : false;
            }
       
            Session["mreference"] = vm.REPORTS.mreference;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult paymentNoFocused( string consultRefNo )
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            bool mpayauto;
            string anycode = "";
            DataTable dtcontrol;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select pauto, activeglint, glbatchno, gldocument, glcompany, cashpoint, attendlink, dischtime, name, DTFORMAT from mrcontrol order by recid", false);
            mpayauto = (bool)dt.Rows[0]["pauto"];

            //panPVTDeposit.Visible = false;

            if (string.IsNullOrWhiteSpace(anycode) && mpayauto) //no lookup
            {
                dtcontrol = Dataaccess.GetAnytable("", "MR", "SELECT payno from mrcontrol where recid = '4'", false);
                vm.REPORTS.mlastno = (decimal)dtcontrol.Rows[0]["payno"] + 1; // msmrfunc.getcontrol_lastnumber("PAYNO", 3, false, mlastno, false) + 1;
                //combPmtNo.Text = mlastno.ToString();
            }

            if (string.IsNullOrWhiteSpace(consultRefNo))
            {
                //combGpCd.Text = combClientCd.Text = txtName.Text = mcusttype = mfacility = combBillRef.Text = "";
                //nmrCurrentCredit.Value = nmrCurrentDebit.Value = nmrPayable.Value = nmrBalanceBF.Value = 0m;

                vm.REPORTS.REPORT_TYPE1 = "";
                vm.REPORTS.nmrPayable = 0;
            }

            //btnDelete.Enabled = btnSave.Enabled = pancapitations.Visible = false;
            vm.REPORTS.cmbsave = false;

            //start_time = DateTime.Now.ToString("HH:mm:ss");
            //dtValueDate.Value = dtRecvdDate.Value = DateTime.Now.Date;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void get_currenttrans()
        {
            //listviewHeader.Visible = true;
            //listView1.Visible = true;
            //// listviewHeaderDetails();

            //string[] arr = new string[5];
            //ListViewItem itm;
            //// nmrPayable.Value = 0m;
            //decimal mcurdb = 0m, mcurcr = 0m;
            //int itemcount = 0;
            //listView1.Items.Clear();

            //show table
            vm.REPORTS.chkReportbyAgent = true;

            DataTable dt = Billings.GetBILLINGdetails(vm.REPORTS.mgrouphead, vm.REPORTS.txtothername, vm.REPORTS.mpatientno, 
                (vm.REPORTS.mgrouphead.Trim() == "MISC") ? "N" : (!string.IsNullOrWhiteSpace(vm.REPORTS.mpatientno)) ?
                "P" : "G", vm.REPORTS.TRANS_DATE1.Value.Date, vm.REPORTS.TRANS_DATE1.Value.Date, vm.REPORTS.txtgroupcode);

            if (dt.Rows.Count > 0)
            {
                vm.BILLINGS = ErpFunc.ConvertDtToList<MR_DATA.BILLING>(dt);
            }


            DataTable dtp = PAYDETAIL.GetPAYMENTdetails(vm.REPORTS.mgrouphead, vm.REPORTS.txtothername, vm.REPORTS.mpatientno, (vm.REPORTS.mgrouphead.Trim() == "MISC") ? "N" :
                (!string.IsNullOrWhiteSpace(vm.REPORTS.mpatientno)) ? "P" : "G", vm.REPORTS.TRANS_DATE1.Value.Date, vm.REPORTS.TRANS_DATE1.Value.Date);

            if (dtp.Rows.Count > 0)
            {
                vm.PAYDETAILS = ErpFunc.ConvertDtToList<MR_DATA.PAYDETAIL>(dtp);
            }

            
            //adjust
            DataTable dta = BILL_ADJ.GetAdjustdetails(vm.REPORTS.mgrouphead, vm.REPORTS.txtothername, vm.REPORTS.mpatientno, (vm.REPORTS.mgrouphead.Trim() == "MISC") ? "N" :
                (!string.IsNullOrWhiteSpace(vm.REPORTS.mpatientno)) ? "P" : "G", vm.REPORTS.TRANS_DATE1.Value.Date, vm.REPORTS.TRANS_DATE1.Value.Date);

            if (dta.Rows.Count > 0)
            {
                vm.BILL_ADJS = ErpFunc.ConvertDtToList<MR_DATA.BILL_ADJ>(dta);
            }

            
            //nmrCurrentCredit.Value = mcurcr;
            //nmrCurrentDebit.Value = mcurdb;
            //nmrPayable.Value = (nmrBalanceBF.Value + nmrCurrentDebit.Value) - nmrCurrentCredit.Value;
            //nmrAmount.Value = nmrPayable.Value < 1 ? 0 : nmrPayable.Value;
        }

        void calc_op_bal()
        {
            decimal db, cr, adj, bal; db = cr = adj = bal = 0m;
            bal = getOpeningBalance(vm.REPORTS.txtgroupcode, vm.REPORTS.REPORT_TYPE1, "", 
                string.IsNullOrWhiteSpace(vm.REPORTS.txtgroupcode) ? "C" : "P", 
                DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);

            db = cr = adj = 0m;

            decimal xamt = getTransactionDbCrAdjSummary(vm.REPORTS.txtgroupcode, vm.REPORTS.REPORT_TYPE1, 
                "", string.IsNullOrWhiteSpace(vm.REPORTS.txtgroupcode) ? "C" : "P", DateTime.Now.Date, 
                DateTime.Now.Date, ref db, ref cr, ref adj);

            vm.REPORTS.nmrbalance = Math.Abs(bal);
            vm.REPORTS.lblBalbfDbCr = (bal < 1) ? "CR" : "DB";
            vm.REPORTS.nmrcurcredit = cr;
            vm.REPORTS.nmrcurdebit = db;

            if (adj < 1)
                vm.REPORTS.nmrcurcredit += Math.Abs(adj);
            else
                vm.REPORTS.nmrcurdebit += adj;

            if (bal < 1)
                vm.REPORTS.nmrcurcredit += Math.Abs(bal);
            else
                vm.REPORTS.nmrcurdebit += bal;
            
            vm.REPORTS.nmrPayable = Math.Abs(vm.REPORTS.nmrcurdebit - vm.REPORTS.nmrcurcredit);
            vm.REPORTS.lblbillspayable = vm.REPORTS.nmrcurdebit - vm.REPORTS.nmrcurcredit < 1 ? "CR" : "DB";

            if (vm.REPORTS.lblbillspayable == "CR")
                vm.REPORTS.nmrAmountTo = 0;

        }

        public JsonResult loadgrid(string consultRefNo)
        {
            DataTable servicetagged = SUSPENSE.GetSUSPENSE(consultRefNo, "U");

            vm.SUSPENSES = ErpFunc.ConvertDtToList<MR_DATA.SUSPENSE>(servicetagged);

            return Json(vm.SUSPENSES, JsonRequestBehavior.AllowGet);
        }


        public JsonResult consultRefNoFocusout(string consultRefNo, string clientCode, string groupCode,
            bool isadmissions, string name, bool chkGetBal)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Mrattend mrattend = new Mrattend();
            Admrecs admrec = new Admrecs();
            patientinfo patients = new patientinfo();
            Customer customers = new Customer();
            DataTable servicetagged;

            vm.REPORTS.txtreference = consultRefNo;
            vm.REPORTS.txtgroupcode = groupCode;
            vm.REPORTS.REPORT_TYPE1 = clientCode;
            vm.REPORTS.TXTPATIENTNAME = name;

            string msection = "2";

            if (string.IsNullOrWhiteSpace(vm.REPORTS.txtreference))
            {
                //check sections and allows  msection $ "1293" && 9-PAEDIATRICS CAN COLLECT MONEY:REG./CA/PAED/ND
                if (new string[] { "1", "2", "3", "9", "C" }.Contains(msection)) //msection $ "1293"
                {
                }//ok
                else
                {
                    //msgeventtracker = "EXIT";
                    vm.REPORTS.alertMessage = "No Access to further details here... TKS";
                    //btnExit.PerformClick();
                    return Json(vm, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //combGpCd.Enabled = combClientCd.Enabled = txtName.Enabled = combBillRef.Enabled = false;
                vm.REPORTS.chkDomantAccts = true; //if true is returned, disable; 

                // msgeventtracker = "REF";
                string anycode = "";

                string start_time = DateTime.Now.ToString("HH:mm:ss");
                string xtranstype = "";
                bool xfoundit = true;

                //check if in attendance records
                if (string.IsNullOrWhiteSpace(anycode) && !string.IsNullOrWhiteSpace(vm.REPORTS.txtreference) &&
                    (Char.IsDigit(vm.REPORTS.txtreference[0])))  //no lookup value obtained
                {
                    vm.REPORTS.txtreference = bissclass.autonumconfig(vm.REPORTS.txtreference, true, "C", "999999999"); //for combConsAdmNo.Text
                }

                xtranstype = vm.REPORTS.txtreference.Substring(0, 1);

                if (xtranstype == "A") //admissions
                {
                    admrec = Admrecs.GetADMRECS(vm.REPORTS.txtreference);

                    if (admrec == null)
                    {
                        vm.REPORTS.alertMessage = "Invalid Admissions Reference... ";

                        return Json(vm, JsonRequestBehavior.AllowGet);
                    }
                }
                else //if (xtranstype == "C" || xtranstype == "S" || xtranstype == "I") //CONSULT/SP.SERVICE/IMMUNIZATN
                {
                    mrattend = Mrattend.GetMrattend(vm.REPORTS.txtreference);

                    if (mrattend == null)
                    {
                        vm.REPORTS.alertMessage = "Unable to Link this Consultation Reference in Daily Attendance Register... ";

                        return Json(vm, JsonRequestBehavior.AllowGet);
                    }
                }

                vm.REPORTS.txtgroupcode = (xtranstype == "A") ? admrec.GHGROUPCODE : mrattend.GHGROUPCODE;   //for combGpCd.Text
                vm.REPORTS.REPORT_TYPE1 = (xtranstype == "A") ? admrec.GROUPHEAD : mrattend.GROUPHEAD;  //for combClientCd.Text
                vm.REPORTS.TXTPATIENTNAME = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;
                vm.REPORTS.TRANS_DATE1 = (xtranstype == "A") ? admrec.ADM_DATE : mrattend.TRANS_DATE;
                vm.REPORTS.REPORT_TYPE2 = string.Format("{0:yyyy-MM-dd}", vm.REPORTS.TRANS_DATE1); 
                vm.REPORTS.cbotype = (xtranstype == "A") ? admrec.GROUPHTYPE : mrattend.GROUPHTYPE;  //for mcusttype
                vm.REPORTS.mgroupcode = (xtranstype == "A") ? admrec.GROUPCODE : mrattend.GROUPCODE; //for mgroupcode
                vm.REPORTS.mpatientno = (xtranstype == "A") ? admrec.PATIENTNO : mrattend.PATIENTNO; //for mpatientno
                vm.REPORTS.txtothername = (xtranstype == "A") ? admrec.NAME : mrattend.NAME; //for mname

                if (vm.REPORTS.cbotype == "P")
                {
                    patients = patientinfo.GetPatient(vm.REPORTS.REPORT_TYPE1, vm.REPORTS.txtgroupcode);
                    if (patients == null)
                        xfoundit = false;
                }
                else
                {
                    customers = Customer.GetCustomer(vm.REPORTS.REPORT_TYPE1);
                    if (customers == null)
                        xfoundit = false;
                }

                if (!xfoundit)
                {
                    vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information... 'Check RESPONSIBLE FOR BILLS";

                    return Json(vm, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.TXTPATIENTNAME = (string.IsNullOrWhiteSpace(vm.REPORTS.mpatientno) || vm.REPORTS.txtreference.Substring(0, 1) == "S" || vm.REPORTS.mgroupcode == "NHIS") ? vm.REPORTS.TXTPATIENTNAME : (vm.REPORTS.cbotype == "P") ? patients.name : customers.Name;
                vm.REPORTS.mgrouphead = (vm.REPORTS.cbotype == "P") ? patients.patientno : customers.Custno;  //for mgrouphead
                vm.REPORTS.mghgroupcode = (vm.REPORTS.cbotype == "P") ? patients.groupcode : ""; //for mghgroupcode

                //if (vm.REPORTS.cbotype == "P" && !string.IsNullOrWhiteSpace(patients.piclocation))
                //{
                //    pictureBox1.Visible = true;
                //    pictureBox1.Image = WebGUIGatway.getpicture(patients.piclocation);
                //}
                //else if (vm.REPORTS.mgroupcode.Trim() == "NHIS") //get piicture from billchian
                //{
                //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select piclocation from billchain where groupcode = '" + mrattend.GROUPCODE + "' and patientno = '" + mrattend.PATIENTNO + "'", false);
                //    if (dt.Rows.Count > 0)
                //    {
                //        pictureBox1.Visible = true;
                //        pictureBox1.Image = WebGUIGatway.getpicture(dt.Rows[0]["piclocation"].ToString());
                //    }
                //}

                vm.REPORTS.nmrPayable = 0;
                vm.REPORTS.nmrAmountTo = 0; 

                if (!isadmissions)
                {
                    //check for investigations request in suspense and get details to acctfromsusp form for tagging
                    servicetagged = SUSPENSE.GetSUSPENSE(consultRefNo, "U");

                    if (servicetagged.Rows.Count > 0)
                    {
                        vm.REPORTS.chkLoyaltyCustomers = true; //A flag;

                        //frmAcctfromSusp FrmacctFromsusp = new frmAcctfromSusp(servicetagged, combConsAdmNo.Text.Trim() + "  : " + mname.Trim() + " : " + combGpCd.Text.Trim() + "-" + combClientCd.Text.Trim());
                        //FrmacctFromsusp.Closed += new EventHandler(FrmacctFromsusp_Closed);
                        //FrmacctFromsusp.ShowDialog();
                    }

                    if (chkGetBal && vm.REPORTS.cbotype == "P" && !string.IsNullOrWhiteSpace(vm.REPORTS.mpatientno))
                    {
                        calc_op_bal();
                    }

                    get_currenttrans();
                }

                //if (admdepositflag == "A")
                //{
                //    decimal xm = nmrAmount.Value;
                //    lbladmissiondeposit.Visible = nmrAdmissionDeposit.Visible = true;
                //    nmrAdmissionDeposit.Value = Convert.ToDecimal(bissclass.seeksay("select cumbil from link where recid = '" + mrecid + "'", "MR", "cumbil"));
                //    nmrAmount.Value += nmrAdmissionDeposit.Value;
                //}

                vm.REPORTS.chkHMO = true; //for combPmtNo.Focus();
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult onLoad(bool isotherserviceIncome)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select pauto, activeglint, glbatchno, gldocument, glcompany, cashpoint, attendlink, dischtime, name, DTFORMAT from mrcontrol order by recid", false);
            string NHISAcct = dt.Rows[2]["DTFORMAT"].ToString();
            string  miscpatientAcct = dt.Rows[6]["name"].ToString().Substring(0, 9);

            if (isotherserviceIncome && string.IsNullOrWhiteSpace(miscpatientAcct))
            {
                vm.REPORTS.alertMessage = "Misc. Transactions/Patient Account Code Not Defined in Systems Setup";
                //btnExit.PerformClick();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(NHISAcct))
            {
                vm.REPORTS.alertMessage = "NHIS Group Payments Account Code Not Defined in Systems Setup";
                //btnExit.PerformClick();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        #endregion
        //PatientPaymentMaintenance End



        //Billing Start
        #region
        public JsonResult BTpaymentNoFocusout(string BTpaymentNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select amount, name from paydetail where reference = '" + BTpaymentNo + "'", false);

            if (dt.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Invalid Payment Reference...";
                vm.REPORTS.txtreference = ""; //for payment No

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.nmrMinBalance = Convert.ToDecimal(dt.Rows[0]["amount"]); //for nmrPayments.Value

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        private void writeBILLS(bool xnewrec, string xreference, decimal xitem, 
            string xprocess, string xdescription, string xgrouphtype, decimal xamount, 
            DateTime xdate, string xname, string xgrouphead, string xfacility, 
            string xgroupcode, string xpatientno, string debitcredit_CD, string xghgroupcod, 
            string xoperator, DateTime xop_time, string xextdesc,string xcurrency, 
            decimal xexrate, int xfxtype, string xdiag, string xdoctor, bool xposted,
            string xpayref, string xservicetyp, decimal xpayment, string xpaytype, 
            decimal xfcamount, string in_outpatient, bool receipted, int recid)
        {
            //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
            DateTime dtmin_date = DateTime.Now;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (xservicetyp == "b") ? "capbills_Add" : (xnewrec) ? "Billing_Add" : "Billing_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Reference", xreference);
            insertCommand.Parameters.AddWithValue("@patientno", xpatientno);
            insertCommand.Parameters.AddWithValue("@name", xname);
            insertCommand.Parameters.AddWithValue("@Itemno", xitem);
            insertCommand.Parameters.AddWithValue("@diag", xdiag);
            insertCommand.Parameters.AddWithValue("@process", xprocess);
            insertCommand.Parameters.AddWithValue("@description", xdescription);
            insertCommand.Parameters.AddWithValue("@doctor", xdoctor);
            insertCommand.Parameters.AddWithValue("@facility", xfacility);
            insertCommand.Parameters.AddWithValue("@amount", xamount);
            insertCommand.Parameters.AddWithValue("@trans_date", xdate);
            insertCommand.Parameters.AddWithValue("@sec_level", 0m);
            insertCommand.Parameters.AddWithValue("@posted", (xnewrec) ? false : xposted);
            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
            insertCommand.Parameters.AddWithValue("@receipted", receipted);
            insertCommand.Parameters.AddWithValue("@transtype", xgrouphtype);
            insertCommand.Parameters.AddWithValue("@payref", xpayref);
            insertCommand.Parameters.AddWithValue("@grouphead", xgrouphead);
            insertCommand.Parameters.AddWithValue("@servicetype", xservicetyp);
            insertCommand.Parameters.AddWithValue("@payment", xpayment);
            insertCommand.Parameters.AddWithValue("@groupcode", xgroupcode);
            insertCommand.Parameters.AddWithValue("@ttype", debitcredit_CD);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", xghgroupcod);
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@op_time", xop_time);
            insertCommand.Parameters.AddWithValue("@currency", xcurrency);
            insertCommand.Parameters.AddWithValue("@exrate", xexrate);
            insertCommand.Parameters.AddWithValue("@fcamount", xfcamount);
            insertCommand.Parameters.AddWithValue("@extdesc", xextdesc);
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
                vm.REPORTS.ActRslt = "SQL access" + ex;
                return;
            }
            finally
            {
                connection.Close();
            }

        }

        public JsonResult BTtransferBtnClicked(bool chkTransferVal, string BTpatientNo, 
            string BTgroupCode, decimal valToTransfer, bool chkBills, bool chkPayments, 
            int BTpaymentVal, string BTpaymentNo, string consultAdmRef)
        {
            string woperator = Request.Cookies["mrName"].Value;
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();

            DataTable bills = Billings.GetBILLING(consultAdmRef);

            bchain = Getbillchain(BTpatientNo, BTgroupCode);

            string updstr = "";
            if (chkTransferVal)
            {
                decimal mlastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, true, 0, false) + 1;
                string xref = bissclass.autonumconfig(mlastno.ToString(), true, "", "999999999");

                writeBILLS(
                    true, xref, 1, bills.Rows[0]["process"].ToString(), 
                    "Bills Transferred from :" + bills.Rows[0]["reference"].ToString(), 
                    bchain.GROUPHTYPE, valToTransfer, (DateTime)bills.Rows[0]["trans_date"], 
                    bchain.NAME, bchain.GROUPHEAD, bills.Rows[0]["facility"].ToString(),
                    BTgroupCode, BTpatientNo, "D", bchain.GHGROUPCODE, woperator, 
                    DateTime.Now, "", "", 0m, 0, bills.Rows[0]["diag"].ToString(), 
                    bills.Rows[0]["doctor"].ToString(), false, "", "", 0m, "", 0m, "O", false, 0
                );

                mlastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, true, 0, false) + 1;
                xref = bissclass.autonumconfig(mlastno.ToString(), true, "", "999999999");

                writeBILLS(
                    true, xref, 1, bills.Rows[0]["process"].ToString(), 
                    "Transferred Bills To : :" + BTgroupCode.Trim() + ":" + 
                    BTpatientNo.Trim(), bchain.GROUPHTYPE, -valToTransfer, 
                    (DateTime)bills.Rows[0]["trans_date"], bills.Rows[0]["name"].ToString(), 
                    bills.Rows[0]["grouphead"].ToString(), bills.Rows[0]["facility"].ToString(), 
                    bills.Rows[0]["groupcode"].ToString(), bills.Rows[0]["patientno"].ToString(), 
                    "D", bills.Rows[0]["ghgroupcode"].ToString(), woperator, DateTime.Now, 
                    "", "", 0m, 0, bills.Rows[0]["diag"].ToString(), bills.Rows[0]["doctor"].ToString(), 
                    false, "", "", 0m, "", 0m, "O", false, 0
                );
            }
            else
            {
                if (chkBills)
                {
                    updstr = "update billing set groupcode = '" + BTgroupCode + 
                        "', patientno = '" + BTpatientNo + "', ghgroupcode = '" + 
                        bchain.GHGROUPCODE + "', grouphead = '" + bchain.GROUPHEAD +
                        "', name = '" + bchain.NAME + "' where reference = '" + 
                        bills.Rows[0]["reference"].ToString() + "'";

                    bissclass.UpdateRecords(updstr, "MR");
                }

                if (chkPayments && !string.IsNullOrWhiteSpace(BTpaymentNo) && BTpaymentVal > 0)
                {
                    updstr = "update paydetail set groupcode = '" + BTgroupCode + 
                        "', ghgroupcode = '" + bchain.GHGROUPCODE + "', grouphead = '" + 
                        bchain.GROUPHEAD + "', name = '" + bchain.NAME + 
                        "' where reference = '" + BTpaymentNo + "'";

                    bissclass.UpdateRecords(updstr, "MR");
                }
            }

            vm.REPORTS.alertMessage = "Completed...";

            //btnClose.PerformClick();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BTpatientNoFocusout(string BTpatientNo, string BTgroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            billchaindtl bchain = new billchaindtl();
            patientinfo patients = new patientinfo();
            Customer customers = new Customer();

            bchain = Getbillchain(BTpatientNo, BTgroupCode);

            if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO))
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";
                vm.REPORTS.txtpatientno = "";

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            bool xfoundit = true;
            if (bchain.GROUPHTYPE == "P")
            {
                patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);

                if (patients == null){
                    xfoundit = false;
                }
            }
            else
            {
                customers = Customer.GetCustomer(bchain.GROUPHEAD);
                if (customers == null)
                    xfoundit = false;
            }
            if (!xfoundit)
            {
                vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS'";
                vm.REPORTS.txtpatientno = "";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
            vm.REPORTS.txtgrouphead = (bchain.GROUPHTYPE == "P" && bchain.GROUPHEAD == patients.patientno) ?
                "< SELF >" : bchain.GROUPHTYPE == "C" ? customers.Name : patients.name;
            
            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult loadRecsBtnClicked(string transDateString, string dateToString, string ldBillsGroupHead, 
            string ldBillsPatientNo, string ldBillsGroupCode, string hiddenFcgroup)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            DateTime dtDateFrom = Convert.ToDateTime(transDateString);
            DateTime dtDateto = Convert.ToDateTime(dateToString);

            bool fcgroup = hiddenFcgroup == "true" ? true : false;

            string rtnstring = " where trans_date >= '" + dtDateFrom.ToShortDateString() + "' and trans_date <= '" + dtDateto.ToShortDateString() + " 23:59:59.999' AND posted = '0'";

            if (!string.IsNullOrWhiteSpace(ldBillsGroupHead))
                rtnstring += " and grouphead = '" + ldBillsGroupHead + "'";
            if (!string.IsNullOrWhiteSpace(ldBillsPatientNo))
            {
                if (fcgroup)
                    rtnstring += " and ghgroupcode = '" + ldBillsGroupCode + "' and  grouphead = '" + ldBillsPatientNo + "'";
                else
                    rtnstring += " and groupcode = '" + ldBillsGroupCode + "' and patientno = '" + ldBillsPatientNo + "'";
            }

            string selstr = "SELECT reference, patientno, name, itemno, diag, process, description, facility, amount, trans_date, grouphead, servicetype, groupcode, operator, ghgroupcode, op_time, accounttype, recid, RECEIPTED from billing" + rtnstring + " order by grouphead, ghgroupcode, name";

            DataTable dt = Dataaccess.GetAnytable("", "MR", selstr, false);

            vm.BILLINGS = ErpFunc.ConvertDtToList<MR_DATA.BILLING>(dt);

            if (vm.BILLINGS.Count() < 1 || vm.BILLINGS == null)
            {
                vm.REPORTS.alertMessage = "No Data...";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ldBillsGroupHeadFocusout(string ldBillsGroupHead, string ldBillsGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + ldBillsGroupHead + "'", false);

            if (dtcustomer.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Invalid Corporate Clients Reference...";
                //vm.REPORTS.txtgrouphead = "";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.TXTPATIENTNAME = dtcustomer.Rows[0]["name"].ToString();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ldBillsPatientNoFocusout(string ldBillsPatientNo, string ldBillsGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();

            //if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
            //{
            //    txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
            //}

            //check if patientno exists
            bchain = Getbillchain(ldBillsPatientNo, ldBillsGroupCode);

            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";
                vm.REPORTS.txtpatientno = "";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
            vm.REPORTS.chkbyacctofficers = false;   //for fcgroup

            if (bchain.GROUPHEAD == bchain.PATIENTNO)
            {
                vm.REPORTS.ActRslt = "Confirm to Load for All members of this Group/Family...";
                //if (result == DialogResult.Yes)
                //    fcgroup = true;

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult accessCodeSubmitClicked(string accessCodeText)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            
            if(accessCodeText != "ALLbILLS") {
                vm.REPORTS.alertMessage = "Invalid Code";
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult stkItmSubmitBtnClicked(string recID)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            string[,] itema_ = new string[10, 7];

            for (int i = 0; i < 10; i++)
            {
                for (int ia = 0; ia < 7; ia++)
                {
                    itema_[i, ia] = "";
                }

            }

            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    row = dataGridView1.Rows[i];
            //    if (row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[3].FormattedValue.ToString()) || 
            //      Convert.ToDecimal(row.Cells[4].Value) < 1 || Convert.ToDecimal(row.Cells[6].Value) < 1 || Convert.ToDecimal(row.Cells[7].Value) < 1)
            //        continue;
            //    itema_[i, 0] = row.Cells[0].Value.ToString();
            //    itema_[i, 1] = row.Cells[1].FormattedValue.ToString();
            //    itema_[i, 2] = row.Cells[3].FormattedValue.ToString();
            //    itema_[i, 3] = row.Cells[4].Value.ToString();
            //    itema_[i, 4] = row.Cells[5].FormattedValue.ToString();
            //    itema_[i, 5] = row.Cells[6].Value.ToString();
            //    itema_[i, 6] = row.Cells[7].Value.ToString();
            //    if (i > 10)
            //        break;
            //}

            //Session["stkitems"] = (Array)itema_;
            ////  Session["totamt"] = (Decimal)nmrCummTotal.Value;
            //msmrfunc.mrGlobals.anycode1 = nmrCummTotal.Value < 1 ? "" : nmrCummTotal.Value.ToString();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult stkItmRemoveBtnClicked(string recID)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (Convert.ToDecimal(recID) > 0)
            {
                string updatestring = "DELETE from mrb25 WHERE RECID = '" + recID + "' ";

                if (bissclass.UpdateRecords(updatestring, "MR"))
                {
                    vm.REPORTS.alertMessage = "Record Deleted...";
                }
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult stkItemsOnBillingOnLoad(string referenceNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.MRB25S = ErpFunc.RsGet<MR_DATA.MRB25>("MR_DATA", "select * from mrb25 where reference = '" + referenceNo + "'");
            
            return Json(vm.MRB25S, JsonRequestBehavior.AllowGet);
        }

        public JsonResult billingSaveBtnClicked(MR_DATA.REPORTS dataObject, IEnumerable<MR_DATA.PPDRESSINGDTL> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS = dataObject;

            string woperato = Request.Cookies["mrName"].Value;
            string msection = "7";

            frmBillings formObject = new frmBillings(woperato, msection, vm);

            vm.REPORTS = formObject.btnSave_Click(tableList);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult transTypeFocusout()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

           

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteBtnClicked(int recID)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (recID > 0)
            {
                //string updatestr = "delete from billing where reference = '" + combRefNo.Text + "' and itemno = '" + listView1.Items[recno].SubItems[0].ToString() + "' and rtrim(DESCRIPTION) = '" + listView1.Items[recno].SubItems[2].ToString().Trim() + "'";
                string updatestr = "delete from billing where recid = '" + recID.ToString().Trim() + "'";
                bissclass.UpdateRecords(updatestr, "MR");
            }

            vm.REPORTS.alertMessage = "Record Deleted...";

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult procedureFocusout(string procedure, string patientNo, string groupCode, 
            string facility, string procedureText, bool chkStkItmForDrgBill)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();

            string xdesc = "";
            string facility2 = "";

            bchain = Getbillchain(patientNo, groupCode);

            string patcateg = bchain == null ? "" : bchain.PATCATEG;

            vm.REPORTS.txtdiscount = msmrfunc.getFeefromtariff(procedure, patcateg, ref xdesc, ref facility2);  //nmrAmount.Value

            if (string.IsNullOrWhiteSpace(xdesc)) //not found in tariff file
            {
                vm.REPORTS.alertMessage = "Invalid Tariff Reference";
            }

            vm.REPORTS.REPORT_TYPE1 = xdesc; //txtOtherChrg.Text

            if (facility == null)
            {
                vm.REPORTS.combFacility = facility2;
                //combFacility.Text = bissclass.combodisplayitemCodeName("type_code", facility2, dtfacility, "name");
            }

            if (chkStkItmForDrgBill && procedureText.Trim().Contains("DRUG"))
            {
                //frmStkitemsonbill stkitems = new frmStkitemsonbill(combRefNo.Text);
                //stkitems.ShowDialog();
                vm.REPORTS.chkAuditProfile = true; //if true, display modal

                Array itema_;

                if (!string.IsNullOrWhiteSpace(msmrfunc.mrGlobals.anycode1))
                {
                    vm.REPORTS.txtdiscount = Convert.ToDecimal(msmrfunc.mrGlobals.anycode1);
                    itema_ = (Array)Session["stkitems"];
                    vm.REPORTS.chkADVCorporate = false; //txtOtherChrg.Enabled
                    vm.REPORTS.chkApplyFilter = true; //nmrAmount.ReadOnly
                    //btnAdd.Focus();
                }
                else
                {
                    vm.REPORTS.chkADVCorporate = true; //txtOtherChrg.Enabled
                    vm.REPORTS.chkApplyFilter = false; //nmrAmount.ReadOnly
                }
            }

            //nmrAmount.Focus();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }



        public JsonResult itemCountFocusout(IEnumerable<MR_DATA.BILLING> tableList, int itemCount,
            string referenceNo, string patientNo, string groupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();

            bchain = Getbillchain(patientNo, groupCode);
            bool isPosted = false;
            bool foundit = false;
            vm.REPORTS.newrec = true;

            if (tableList.Count() >= 1)
            {
                foreach (var eachRow in tableList)
                {
                    if (eachRow.RECID == itemCount)
                    {
                        var transdate = Convert.ToDateTime(eachRow.OPERATOR);

                        foundit = true;
                        vm.REPORTS.newrec = false;
                        vm.REPORTS.nmrPayRefTo = itemCount;
                        vm.REPORTS.combFacility = eachRow.FACILITY;
                        vm.REPORTS.cbotitle = eachRow.DESCRIPTION; //for other charges;
                        vm.REPORTS.REPORT_TYPE1 = eachRow.AMOUNT.ToString(); //for amount
                        vm.REPORTS.REPORT_TYPE2 = eachRow.PROCESS; //procedure
                        vm.REPORTS.REPORT_TYPE3 = string.Format("{0:yyyy-MM-dd}", transdate); //TransDate
                        vm.REPORTS.REPORT_TYPE4 = eachRow.DIAG;
                        vm.REPORTS.REPORT_TYPE5 = eachRow.DOCTOR;
                        isPosted = eachRow.POSTED == true ? true : false; //converting from bool? to bool

                        break;
                    }
                }

                if (foundit)
                {
                    string msg = referenceNo.Substring(0, 1) == "S" && bchain.GROUPHTYPE != "C" ? "\r\n\r\n It's Special Service Bill... Can't be modified" : isPosted ? "Its Confirmed... Cannot be Amended" : "";
                    vm.REPORTS.alertMessage = "Record Exist...." + msg;

                    if (isPosted)
                    {
                        return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                    }
                    else if (referenceNo.Substring(0, 1) == "S" && bchain.GROUPHTYPE == "P")
                    {
                        vm.REPORTS.txtreference = "";
                        //txtgroupcode.Focus();
                        return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            vm.REPORTS.cmbsave = true; //btnAdd.Enabled = true;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BILLINGreferenceNoFocusout(string referenceNo, int mlastno)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            
            if (bissclass.IsDigitsOnly(referenceNo) && Convert.ToInt32(referenceNo) > mlastno)
            {
                vm.REPORTS.alertMessage = "Bill Reference is out of Sequence...";
                vm.REPORTS.txtreference = "";
                //combRefNo.Select();

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.REPORTS.txtreference = bissclass.autonumconfig(referenceNo, true, "", "999999999");
            }

            vm.REPORTS.cmbsave = true; //btnAdd.Enabled

            displaybillinfo(vm.REPORTS.txtreference);

            //nmrItmCnt.Focus();

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BILLINGreferenceNoFocus(string consultAdmRef, string referenceNo, string mlastnoView)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Mrattend mrattend = new Mrattend();

            mrattend = Mrattend.GetMrattend(referenceNo);

            if(mrattend == null)
            {
                decimal mlastno = Convert.ToDecimal(mlastnoView.Trim());

                if (string.IsNullOrWhiteSpace(consultAdmRef)) //no lookup
                {
                    mlastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, false, mlastno, false) + 1;
                    vm.REPORTS.txtreference = mlastno.ToString();  //combRefNo.Text

                    vm.REPORTS.nmrBalbf = mlastno;
                }
            }


            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BILLINGpatientNoFocusout(string patientNo, string groupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            //if (string.IsNullOrWhiteSpace(AnyCode))  //no lookup value obtained
            //{
            //    combPtNo.Text = bissclass.autonumconfig(combPtNo.Text, true, "", "9999999");
            //}

            if (!DisplayPatDetails(patientNo, groupCode))
            {
                vm.REPORTS.txtgroupcode = vm.REPORTS.txtpatientno = "";

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        void displaybillinfo(string xreference)
        {
            vm.REPORTS.txtdiscount = 0; //nmrCurrentTotal.Value

            DataTable bills = Billings.GetBILLING(xreference);

            vm.BILLINGS = ErpFunc.ConvertDtToList<MR_DATA.BILLING>(bills);

            if (vm.BILLINGS.Count() < 1 || vm.BILLINGS == null)
                return;
        }

        bool DisplayPatDetails(string patientNo, string groupCode)
        {
            billchaindtl bchain = new billchaindtl();
            patientinfo patients = new patientinfo();
            Customer customers = new Customer();

            vm.REPORTS.nmrBalbf = 0;  //nmrCurrentTotal.Value

            //check if patientno exists
            bchain = Getbillchain(patientNo, groupCode);

            if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO))
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";

                return false;
            }

            string mgrouphtype = bchain.GROUPHTYPE;
            bool xfoundit = true;

            if (mgrouphtype == "P")
            {
                patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);

                if (patients == null)
                    xfoundit = false;
            }
            else
            {
                customers = Customer.GetCustomer(bchain.GROUPHEAD);
                if (customers == null)
                    xfoundit = false;
            }
            if (!xfoundit)
            {
                vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information... 'Check RESPONSIBLE FOR BILLS";

                return false;
            }
            
            vm.REPORTS.txtgroupcode = bchain.GROUPCODE;
            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
            vm.REPORTS.combillcycle = (mgrouphtype == "C") ? customers.Bill_cir : patients.bill_cir; //mbill_cir
            vm.REPORTS.txtothername = (mgrouphtype == "P" && bchain.GROUPHEAD == patients.patientno) ?
                "< SELF >" : (mgrouphtype == "C") ? customers.Name : patients.name;
            vm.REPORTS.txtghgroupcode = (mgrouphtype == "P") ? patients.groupcode : "";
            vm.REPORTS.cbotitle = bchain.GROUPHEAD; //combClientCd.Text

            if (mgrouphtype == "P" && bchain.PATIENTNO == patients.patientno)
            {
                vm.REPORTS.txtaddress1 = patients.address1.Trim() + "\r\n";
            }
            else
                vm.REPORTS.txtaddress1 = bchain.RESIDENCE;

            if (bchain.SECTION != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + bchain.SECTION + "\r\n";
            if (bchain.DEPARTMENT != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + bchain.DEPARTMENT.Trim() + "\r\n";
            if (bchain.STAFFNO != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + "[Staff # :" + bchain.STAFFNO.Trim() + " ]";
                vm.REPORTS.cmbsave = true;
                //combClientCd.Enabled = txtghgroupcode.Enabled = combName.Enabled = false;

            if (bchain.STATUS == "C" || bchain.STATUS == "S")
            {
                vm.REPORTS.alertMessage = "PATIENT NOT VALID FOR TRANSACTION - < Cancelled or Suspended >";

                return false;
            }

            return true;
        }

        bool displaydetails(string xreference)
        {
            Admrecs admrec = new Admrecs();
            Mrattend mrattend = new Mrattend();

            // msgeventtracker = "GH";
            string xtranstype = xreference.Substring(0, 1);

            if (xtranstype == "A") //admissions
            {
                admrec = Admrecs.GetADMRECS(xreference);

                if (admrec == null)
                {
                    vm.REPORTS.alertMessage = "Invalid Admissions Reference... ";

                    return false;
                }
            }
            else if (xtranstype == "C" || xtranstype == "S")
            {
                mrattend = Mrattend.GetMrattend(xreference);

                if (mrattend == null)
                {
                    vm.REPORTS.alertMessage = "Unable to Link this Consultation Reference in Daily Attendance Register... ";

                    return false;
                }
            }
            else
            {
                vm.REPORTS.alertMessage = "Invalid Number Format for Consultation/Admission Reference...";

                return false;
            }

            vm.REPORTS.txtghgroupcode = (xtranstype == "A") ? admrec.GHGROUPCODE : mrattend.GHGROUPCODE;
            vm.REPORTS.cbotitle = (xtranstype == "A") ? admrec.GROUPHEAD : mrattend.GROUPHEAD; //combClientCd.Text
            vm.REPORTS.TXTPATIENTNAME = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;

            DateTime dttrans_date = (xtranstype == "A") ? admrec.ADM_DATE : mrattend.TRANS_DATE;
            vm.REPORTS.REPORT_TYPE1 = @String.Format("{0:yyyy-MM-dd}", dttrans_date);  //for REFERRALDATE

            vm.REPORTS.cbotype = (xtranstype == "A") ? admrec.GROUPHTYPE : mrattend.GROUPHTYPE;
            string mgroupcode = (xtranstype == "A") ? admrec.GROUPCODE : mrattend.GROUPCODE;
            string mpatientno = (xtranstype == "A") ? admrec.PATIENTNO : mrattend.PATIENTNO;
            //mname = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;

            vm.REPORTS.combFacility = (xtranstype == "A") ? admrec.FACILITY : mrattend.CLINIC;
            vm.REPORTS.txtpatientno = mpatientno;
            vm.REPORTS.txtgroupcode = mgroupcode;

            if (!DisplayPatDetails(mpatientno, mgroupcode))
            {
                return false;
            }
            
            //AnyCode = Anycode1 = "";
            displaybillinfo(xreference);

            return true;
        }

        public JsonResult consultAdmRefFocusout(string consultAdmRef, bool chkOPDrecords)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            bool isopd = (chkOPDrecords) ? true : false;

            if (!string.IsNullOrWhiteSpace(consultAdmRef) && (Char.IsDigit(consultAdmRef[0])))  //no lookup value obtained
            {
                vm.REPORTS.txtstaffno = bissclass.autonumconfig(consultAdmRef, true, (isopd) ? "C" : "A", "999999999");
                //combConsAdmNo
            }

            vm.REPORTS.nmrAmountTo = 0; //itemno
            vm.REPORTS.txtreference = consultAdmRef; //for  combRefNo.Text

            if (!displaydetails(consultAdmRef))
            {
                vm.REPORTS.txtreference = vm.REPORTS.txtstaffno = "";
                //combConsAdmNo.Select();

                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            //combTransType.Focus();

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        #endregion
        //Billing End


        //AttendanceListByClinic Start
        #region

        public JsonResult AttendListgroupHeadFocusout(string groupHead)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + groupHead + "'", false);

            if (dtcustomer.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Invalid Corporate Clients Reference...";

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.TXTPATIENTNAME = dtcustomer.Rows[0]["name"].ToString();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AttendListPatientNoFocusout(string patientNo, string groupCode)
        {
            billchaindtl bchain = new billchaindtl();
            vm.REPORTS = new MR_DATA.REPORTS();

            //if (bissclass.IsDigitsOnly(patientNo.Trim()))  //no lookup value obtained
            //{
            //    vm.REPORTS.txtpatientno = bissclass.autonumconfig(patientNo, true, "", "9999999");
            //}

            //check if patientno exists
            bchain = Getbillchain(patientNo, groupCode);

            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        #endregion
        //AttendanceListByClinic End



        //HMOauthorizationCode Start
        #region
        public JsonResult HMOauthSubmit(MR_DATA.REPORTS dataObject)
        {
            vm.REPORTS = dataObject;

            hmoauthorizationcode formObj = new hmoauthorizationcode(vm, Request.Cookies["mrName"].Value);
            vm.REPORTS = formObj.btnSubmit_Click();

            
            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult HMOreferenceFocusout(string reference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Mrattend mrattend = new Mrattend();
            Customer customers = new Customer();
            HmoAuthorizations hmoautho = new HmoAuthorizations();

            mrattend = Mrattend.GetMrattend(reference);

            if (mrattend == null)
            {
                vm.REPORTS.alertMessage = "Unable to Link Consultation Reference in Daily Attendance Register... ";
                //this.txtreference.Text = " ";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            customers = Customer.GetCustomer(mrattend.GROUPHEAD);

            vm.REPORTS.txtgroupcode = mrattend.GROUPCODE;
            vm.REPORTS.txtpatientno = mrattend.PATIENTNO;
            vm.REPORTS.TXTPATIENTNAME = mrattend.NAME;


            if (customers == null || !customers.HMO)
            {
                vm.REPORTS.alertMessage = "The Record is not an HMO Account...";
                //txtreference.Text = "";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.newrec = true;
            vm.REPORTS.REPORT_TYPE1 = mrattend.AUTHORIZEDCODE;  //for txthmocode.Text
            vm.REPORTS.REPORT_TYPE4 = customers.Name;  //for lblgroupheadname.Text

            hmoautho = HmoAuthorizations.GetHMOAUTHORIZATIONS(mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO);

            if (hmoautho != null)
            {
                vm.REPORTS.newrec = false;
                vm.REPORTS.txtothername = hmoautho.REQUESTDETAILS;  //for txtothers.Text
                vm.REPORTS.REPORT_TYPE1 = hmoautho.AUTHORIZEDCODE;
                DateTime referralDate = hmoautho.REFERRALDATE;
                vm.REPORTS.REPORT_TYPE2 = @String.Format("{0:yyyy-MM-dd}", referralDate);  //for REFERRALDATE

                //bissclass.displaycombo(cboClinic, dtclinic, hmoautho.REFERRALCLINIC, "type_code");
                //bissclass.displaycombo(cboReferredClinic, dtclinic, hmoautho.REFERREDTOCLINIC, "type_code");
                //bissclass.displaycombo(cboprimarydoc, dtdocs, hmoautho.REFERRAL, "reference");

                string dateReceived = hmoautho.DATERECEIVED;
                vm.REPORTS.REPORT_TYPE3 = @String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateReceived));  //for REFERRALDATE
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //HMOauthorizationCode End



        //Ante-Natal Registration start 
        #region
        public JsonResult ANCsubmitBtnClicked(MR_DATA.BILLCHAIN dataObject, IEnumerable<MR_DATA.DISPSERV> stockList,
            IEnumerable<MR_DATA.HMODETAIL> procedureList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            string woperato = Request.Cookies["mrName"].Value;
            string msection = "1";

            frmAncreg formObject = new frmAncreg(woperato, msection);
            vm.REPORTS = formObject.btnSave(dataObject, stockList, procedureList);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }
            

        public JsonResult ANCdeleteClicked(string referenceNo) {
            vm.REPORTS = new MR_DATA.REPORTS();

            string woperato = Request.Cookies["mrName"].Value;
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperato + "'", false);

            ANCREG ancreg = new ANCREG();
            ancreg = ANCREG.GetANCREG(referenceNo);

            bool mcandelete = (bool)dt.Rows[0]["candelete"];

            if (ancreg.POSTED)
            {
                vm.REPORTS.alertMessage = "Record Can't be Deleted...";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            if (!mcandelete)
            {
                vm.REPORTS.alertMessage = "ACCESS DENIED...To Delete existing Record.  See your Systems Administrator!";
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
                       
            ANCREG.DeleteANCREG(referenceNo);

            //this.ClearControls("R");

            vm.REPORTS.alertMessage = "Deleted";
            //txtreference.Focus();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult dateRegFocusout(string dateRegistered)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (Convert.ToDateTime(dateRegistered) > DateTime.Now.Date)
            {
                vm.REPORTS.alertMessage = "Registration Date cannot be greater than Today...";

                vm.REPORTS.REPORT_TYPE1 = @String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);  //for dtedd.Value
            }

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ANChospitalNoFocusout(string referenceNo, string hospitalNo, string groupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start.Year < 2000 ?
                bissclass.ConvertStringToDateTime("01", "01", "2011") :
                msmrfunc.mrGlobals.mta_start;

            billchaindtl bchain = new billchaindtl();

            vm.REPORTS.newrec = true;
            
            //check if patientno exists
            bchain = Getbillchain(hospitalNo, groupCode);

            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";
                vm.REPORTS.txtpatientno = "";

                //txtgroupcode.Focus();
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.REPORTS.cbotype = bchain.GROUPHTYPE;
                vm.REPORTS.txtgrouphead = bchain.GROUPHEAD;

                if (!ANCDisplayDetails(referenceNo, bchain, ""))
                {
                    vm.REPORTS.txtgroupcode = "";
                    //txtgroupcode.Focus();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }
            }

            //check for active anc registration
            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT del_date, reference, REG_DATE, reg_time from ancreg where groupcode = '" + bchain.GROUPCODE + "' and patientno = '" + bchain.PATIENTNO + "'", false);
            bool foundit = false;
            string rtntext = "";

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(dt.Rows[i]["del_date"]).Date <= dtmin_date && dt.Rows[i].ToString() != referenceNo)
                    {
                        foundit = true;
                        rtntext = dt.Rows[i]["reference"].ToString().Trim() + " of " +
                            Convert.ToDateTime(dt.Rows[i]["REG_DATE"]).ToString("dd-MM-yyyy") + " @ " + dt.Rows[i]["reg_time"].ToString().Substring(0, 5);
                        break;
                    }
                }
                if (foundit)
                {
                    vm.REPORTS.alertMessage = "An Active ANC Record found for This Patient...Check ANC Ref : " + rtntext + " ...";
                    //ClearControls("R");
                    //txtreference.Focus();

                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }
            }

            vm.REPORTS.cmbsave = true;

            //btnSave.Enabled = true;
            //dtdateregistered.Focus();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ANCreferenceOnFocus()
        {
            decimal mlastno = 0;

            vm.REPORTS = new MR_DATA.REPORTS();

            mlastno = msmrfunc.getcontrol_lastnumber("ATTNO", 2, false, mlastno, false) + 1;

            vm.REPORTS.txtreference = mlastno.ToString();
            vm.REPORTS.nmrBalbf = mlastno;

            //vm.REPORTS.nmr30days = 0;   //for drgcounter
            //vm.REPORTS.nmr60days = 0;   //for invcounter

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        private bool ANCDisplayDetails(string referenceNo, billchaindtl bchain, string xtype)
        {
            //DialogResult result;
            patientinfo patients = new patientinfo();
            Customer customers = new Customer();
            string mbill_cir = "";
            string mgrouphtype = vm.REPORTS.cbotype;
            var woperator = Request.Cookies["mrName"].Value;

            if (bchain == null)
            {
                return false;
            }

            if (vm.REPORTS.cbotype == "P")
            {
                
                patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);

                if (patients == null)
                {
                    vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information... Check RESPONSIBLE FOR BILLS";
                    vm.REPORTS.txtpatientno = "";
                    //this.txtgroupcode.Focus();
                    return false;
                }
            }
            else
            {
                if (bchain != null)
                {
                    customers = Customer.GetCustomer(bchain.GROUPHEAD);
                }

                if (customers == null)
                {
                    vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information... Check RESPONSIBLE FOR BILLS";
                    vm.REPORTS.txtpatientno = "";
                    //this.txtgroupcode.Focus();
                    return false;
                }
            }

            vm.REPORTS.txtgroupcode = bchain.GROUPCODE;
            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;

            mbill_cir = (mgrouphtype == "C") ? customers.Bill_cir : patients.bill_cir;

            vm.REPORTS.txtothername = (mgrouphtype == "P" && bchain.GROUPHEAD == bchain.PATIENTNO) ?
                "< SELF >" : (mgrouphtype == "C") ? customers.Name : patients.name; //for txtgroupheadname

            if (mgrouphtype == "P" && bchain.PATIENTNO == patients.patientno)
            {
                vm.REPORTS.txtaddress1 = patients.address1.Trim() + "\r\n";
            }
            else
                vm.REPORTS.txtaddress1 = bchain.RESIDENCE;

            if (bchain.SECTION != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + bchain.SECTION + "\r\n";
            if (bchain.DEPARTMENT != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + bchain.DEPARTMENT.Trim() + "\r\n";
            if (bchain.STAFFNO != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + "[Staff # :" + bchain.STAFFNO.Trim() + " ]";

            if (xtype == "S") //display from existing reference
                return true;

            if (bchain.SEX.Substring(0, 1) != "F" || bchain.RELATIONSH == "C" || DateTime.Now.Year - bchain.BIRTHDATE.Year < 18 || bchain.M_STATUS == "S")
            {
                if (bchain.SEX.Substring(0, 1) != "F")
                {
                    vm.REPORTS.alertMessage = "Patient Cannot be registered for ANTE-NATAL - Registered as a Male...";
                    return false;
                }
                else
                {
                    vm.REPORTS.alertMessage = "Patient Cannot be registered for ANTE-NATAL : \r\n Check Age or Marital Status on Registeration...";
                    //if (result == DialogResult.No)
                    //    return false;
                    return false;
                }
            }

            if (bchain.STATUS == "C")
            {
                vm.REPORTS.alertMessage = "PATIENT NOT VALID FOR TRANSACTION - < Cancelled >";
                vm.REPORTS.txtpatientno = "";
                //txtgroupcode.Focus();
                return false;
            }
            if (bchain.STATUS == "S")
            {
                vm.REPORTS.alertMessage = "PATIENT NOT VALID FOR TRANSACTION - < Suspended > ";

                //if (result == DialogResult.No)
                //{
                //    ClearControls("R");
                //    txtreference.Select();
                //    return false;
                //}

                frmOverwrite overwrite = new frmOverwrite("Overwrite  Suspended Registration", "mrstlev", "MR");
                overwrite.ShowDialog();

                if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode)) //anycode global var is returned
                    return false;
                
                //update overwrite profile
                msmrfunc.updateOverwrite(referenceNo, "Overwrite Suspended Registration at ANC Booking", bchain,
                    bchain.GROUPHTYPE == "P" ? patients.cr_limit : customers.Cr_limit, 0m, woperator);
            }

            return true;
        }


        public JsonResult ANCreferenceOnFocusOut(string referenceNo, string mlastno, string hospitalNo,
            string groupCode)
        {
            ANCREG ancreg = new ANCREG();
            ANC01 anc01 = new ANC01();
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();
            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start.Year < 2000 ? 
                bissclass.ConvertStringToDateTime("01", "01", "2011") : 
                msmrfunc.mrGlobals.mta_start;
            string xposted = "";
            //string mgrouphtype = "";
            //string mgrouphead = "";

            if (bissclass.IsDigitsOnly(referenceNo.Trim()))
            {
                if (Convert.ToDecimal(referenceNo) > Convert.ToDecimal(mlastno))
                {
                    vm.REPORTS.alertMessage = "ANC Registeration Reference is out of Sequence...";
                    vm.REPORTS.txtreference = "";

                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.txtreference = bissclass.autonumconfig(referenceNo, true, " ", "999999999");
            };

            vm.REPORTS.newrec = true;

            //check if reference exist
            //this.ClearControls("P");
            //AnyCode = Anycode1 = "";
            //msgeventtracker = "RN";

            ancreg = ANCREG.GetANCREG(vm.REPORTS.txtreference);

            if (ancreg == null) //new defintion
            {
                // msmrfunc.mrGlobals.waitwindowtext = "NEW ANC RECORD ...";
                //  MessageBox.Show("NEW ANC RECORD ...");

                vm.REPORTS.chkSortByOperator = false;  //For txtedd.Visible and txtlmp.Visible

                // Display form modelessly
                //  pleaseWait.Show();

            }
            else
            {
                xposted = (ancreg.POSTED) ? " and Posted Can't be amended !" : "";

                //msmrfunc.mrGlobals.waitwindowtext = "Record Exists"+xposted;
                vm.REPORTS.newrec = false;
                // pleaseWait.Show();
                vm.REPORTS.cbotype = ancreg.GROUPHTYPE; //for mgrouphtype
                vm.REPORTS.txtpatientno = ancreg.PATIENTNO;
                vm.REPORTS.txtgroupcode = ancreg.GROUPCODE;
                vm.REPORTS.txtgrouphead = ancreg.GROUPHEAD;  // for mgrouphead
                vm.REPORTS.REPORT_TYPE3 = @String.Format("{0:yyyy-MM-dd}", ancreg.REG_DATE);

                bchain = Getbillchain(hospitalNo, groupCode);
                vm.REPORTS.REPORT_TYPE1 = @String.Format("{0:yyyy-MM-dd}", ancreg.LMP); //for dtlmp.Value 
                vm.REPORTS.REPORT_TYPE2 = @String.Format("{0:yyyy-MM-dd}", ancreg.EDD);  //for dtedd.Value

                if (ancreg.LMP <= dtmin_date)
                {
                    vm.REPORTS.chkSortByOperator = true;  //For txtedd.Visible and txtlmp.Visible
                    //dtlmp.Text = ""; // blankdate.ToString();
                    //dtedd.Text = ""; // blankdate.ToString();
                }

                if (ancreg.DEL_DATE <= dtmin_date)
                {
                    anc01 = ANC01.GetANC01(ancreg.REFERENCE);
                    if (anc01 != null && anc01.DEL_DATE != ancreg.DEL_DATE)
                    {
                        string updatestr = "update ancreg set del_date = '" + anc01.DEL_DATE + "' where reference = '" + vm.REPORTS.txtreference + "'";
                        bissclass.UpdateRecords(updatestr, "MR");

                        vm.REPORTS.ActRslt = "Delivery Date Adjusted...";
                    }
                }

                vm.REPORTS.chkBroughtForward = (ancreg.EVERYVISITCONSULT) ? true : false; //for  chkvisitconsultcharge.Checked
                vm.REPORTS.nmrMinBalance = ancreg.CONSULTAMT;
                vm.REPORTS.REPORT_TYPE4 = (ancreg.DEL_DATE <= dtmin_date) ? "" : ancreg.DEL_DATE.ToShortDateString();  //for txtdeliverydate.Text
                
                if (vm.REPORTS.REPORT_TYPE4 == "") {
                    vm.REPORTS.REPORT_TYPE4 = DateTime.Now.ToString();
                } else {
                    vm.REPORTS.REPORT_TYPE4 = @String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(vm.REPORTS.REPORT_TYPE4));
                }


                bool result = ANCDisplayDetails(vm.REPORTS.txtreference, bchain, "S");

                if(result == false) {
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                if (ancreg.DEL_DATE > dtmin_date)
                {
                    vm.REPORTS.alertMessage = "This Record is closed... Patient delivered on " + ancreg.DEL_DATE.ToString() ;

                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.alertMessage = "Record Exists" + xposted;

                if (ancreg.POSTED)
                {
                    vm.REPORTS.categ_save = "true";

                    //ClearControls("R");
                    //txtreference.Focus();
                    return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.cmbdelete = true;

                //btnDelete.Enabled = true;
            }

            vm.REPORTS.cmbsave = true;

            //btnSave.Enabled = true;

            //txtgroupcode.Focus();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //Ante-Natal Registration end



        //OPD VITALS START

        #region
        public JsonResult BPHLoadBtnClicked(string consultReference, string dateRangeBPH, string dateToBPH)
        {
            billchaindtl bchain = new billchaindtl();
            Mrattend mrattend = new Mrattend();

            mrattend = Mrattend.GetMrattend(consultReference);

            bchain = Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);

            if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO))
            {
                vm.REPORTS.alertMessage = "No Record";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            vm.VSTATAS = ErpFunc.RsGet<MR_DATA.VSTATA>("MR_DATA",
                "select * from vstata where groupcode = '" + bchain.GROUPCODE + "' and patientno = '" + bchain.PATIENTNO + "' and trans_date >= '" + dateRangeBPH + "' and trans_date <= '" + dateToBPH + " 23:59:59:999'");

            return Json(vm, JsonRequestBehavior.AllowGet);

        }


        public JsonResult BPHbtnClicked(string consultReference)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();
            Mrattend mrattend = new Mrattend();

            mrattend = Mrattend.GetMrattend(consultReference);

            bchain = Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);

            if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO))
                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);

            vm.REPORTS.txtothername = "Bp History for : " + bchain.NAME + " [" + bchain.GROUPCODE + ":" + bchain.PATIENTNO + "]";
            vm.REPORTS.REPORT_TYPE1 = @String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date.AddDays(-30));

            //frmBPHistory formObject = new frmBPHistory(vm.REPORTS);
            //vm.REPORTS = formObject.btnLoad_Click();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);

        }

        public string patientProfileBtnClicked()
        {
            var woperator = Request.Cookies["mrName"].Value;
            vm.REPORTS = new MR_DATA.REPORTS();

            string xstr = "SELECT REFERENCE, GROUPCODE, PATIENTNO, NAME, TRANS_DATE, CLINIC, GROUPHEAD, GROUPHTYPE, VTAKEN, GHGROUPCODE, DOCTOR, DOC_TIME, ATTENDTYPE, CHAR(50) AS DOCSNAME, CHAR(50) AS ghname from mrattend where CONVERT(date, trans_date) = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and doc_time  = '' and LEFT(reference,1) = 'C' order by doctor,trans_date";

            DataTable dtdocs = Dataaccess.GetAnytable("", "MR", "select reference,name from doctors where rectype = 'D' order by name", true);

            //DataTable dt = Dataaccess.GetAnytable("", "MR", xstr, false);

            //DataTable dt = ErpFunc.RsGetDT("MR_DATA", xstr);
            DataTable dt = Dataaccess.GetAnytable("", "MR", xstr, false);

            if (dt.Rows.Count < 1)
            {
                return "False**No Patient Waiting... ";
            }

            DataTable dtcust = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO,NAME FROM CUSTOMER", false);

            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row["doctor"].ToString())) // chkOnDocsPatProfile.Checked)
                    row["docsname"] = bissclass.combodisplayitemCodeName("reference", row["doctor"].ToString(), dtdocs, "name");
                else
                    row["docsname"] = "< Unspecified >";
                if (row["patientno"].ToString() == row["grouphead"].ToString())
                    row["ghname"] = "< S E L F >";
                else if (row["grouphtype"].ToString() == "P")
                    row["ghname"] = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P");
                else {
                    row["ghname"] = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dtcust, "name");
                }
            
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            vm.REPORTS.SessionRDLC = "Attendance_ByDocs.rdlc";
            vm.REPORTS.SessionSQL = "";
            vm.REPORTS.SessionWaitonly = "Y";

            string mrptheader = "WAITING LIST FOR " + DateTime.Now.ToLongDateString();

            frmReportViewer paedreports = new frmReportViewer("", mrptheader, "",
                "", "", "WAITINGLIST", "", 0m, "", "", "", ds, true, 0, DateTime.Now,
                DateTime.Now, "", false, "", woperator, vm.REPORTS);

            vm.REPORTS = paedreports.Show(vm.REPORTS.SessionRDLC,vm.REPORTS.SessionSQL, true);

            return "True**"+vm.REPORTS.PdfPath;
        }

        public JsonResult getHistoryBtnClicked(string consultReference, string dateFrom, string dateTo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();
            Mrattend mrattend = new Mrattend();

            mrattend = Mrattend.GetMrattend(consultReference);

            bchain = Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);

            var historyDateFrom = Convert.ToDateTime(dateFrom);
            var historyDateTo = Convert.ToDateTime(dateTo);

            vm.REPORTS.edtspinstructions = MedHist.GetMEDHISTCaseNotes(bchain.GROUPCODE, bchain.PATIENTNO, false, true, historyDateFrom.Date, historyDateTo.Date, bchain, "DESC");
            //if (string.IsNullOrWhiteSpace(vm.REPORTS.edtspinstructions))
            //{
            //    vm.REPORTS.alertMessage = "No Data";
            //}

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        void DisplayPrevDefinitions(Mrattend mrattend)
        {
            decimal xduration = 0m;
            string xDuration;
            decimal counter = 0m;
            //string[] arr = new string[17];
            vm.REPORTS.nmrMinBalance = 0m; //for nmrCurrentTotal.Value

            //DataRow row = null;
            DataTable suspense = SUSPENSE.GetSUSPENSE(mrattend.REFERENCE, "A");

            //Convert DataTable to List
            vm.SUSPENSES = ErpFunc.ConvertDtToList<MR_DATA.SUSPENSE>(suspense);

            //if (suspense != null)
            //{
            //    //listView1.Items.Clear();
            //    //foreach (DataTable row in suspense.Rows)
            //    //xt = mrdt.Rows[0]["rptext"].ToString();

            //    string xtranstype;
            //    for (int i = 0; i < suspense.Rows.Count; i++)
            //    {
            //        counter++;
            //        row = suspense.Rows[i];
            //        vm.REPORTS.nmrMinBalance += Convert.ToDecimal(row["amount"]);
            //        xduration = Convert.ToDecimal(row["duration"]); 
            //        xDuration = (xduration >= 1m) ? " x " + xduration.ToString() : "";

            //        arr[0] = row["itemno"].ToString();
            //        arr[1] = row["facility"].ToString();
            //        arr[2] = row["DESCRIPTION"].ToString() + xDuration;
            //        arr[3] = Convert.ToDecimal(row["amount"]).ToString("N2");
            //        arr[4] = row["process"].ToString();
            //        arr[5] = row["billprocess"].ToString();
            //        arr[6] = row["notes"].ToString().Trim();
            //        arr[7] = row["trans_date"].ToString();
            //        arr[8] = (Convert.ToBoolean(row["CAPITATED"])) ? "YES" : "NO";
            //        arr[9] = (Convert.ToBoolean(row["groupeditem"])) ? "YES" : "NO";
            //        arr[10] = row["duration"].ToString();
            //        arr[11] = (Convert.ToBoolean(row["posted"])) ? "YES" : "NO"; //done or note
            //        arr[12] = "NO"; // indicates for new request or not... needed during delete
            //        arr[13] = row["facility"].ToString();
            //        arr[14] = (Convert.ToBoolean(row["grpbillbyservtype"])) ? "YES" : "NO";
            //        arr[16] = row["recid"].ToString();

            //        if (i == 0)
            //        {
            //            xtranstype = row["transtype"].ToString();
            //            txtgroupcode.Text = row["groupcode"].ToString();
            //            txtpatientno.Text = row["patientno"].ToString();
            //            cboAge.Text = row["age"].ToString();
            //            cboSex.Text = row["sex"].ToString();
            //            cboName.Text = row["name"].ToString();
            //            txtAddress.Text = row["address1"].ToString();
            //            txtEmail.Text = row["email"].ToString();
            //            txtPhone.Text = row["phone"].ToString();
            //            txtghgroupcode.Text = row["ghgroupcode"].ToString();
            //            txtgrouphead.Text = row["grouphead"].ToString();
            //            bissclass.displaycombo(cboReferrer, dtreferrers, row["DOCTOR"].ToString(), "name");
            //            //  dtregistered.Value = (DateTime)row["trans_date"];
            //            cboBillspayable.Text = (xtranstype == "P" && txtgrouphead.Text.Trim() == mmisc_patient_code.Trim()) ? "SELF" : (xtranstype == "P" && txtgrouphead.Text.Trim() != mmisc_patient_code.Trim()) ? "ANOTHER PATIENT" : "CORPORATE CLIENT";
            //            Notes = arr[6];
            //        }

            //        itm = new ListViewItem(arr);
            //        listView1.Items.Add(itm);
            //    }
            //}
        }


        MR_DATA.MR_DATAvm initheader(string calltype, Mrattend mrattend)
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT seclink, glbatchno, automedbil, name, attendlink from mrcontrol order by recid", false);
            string mmisc_patient_code = dt.Rows[6]["name"].ToString();

            patientinfo patients = new patientinfo();
            billchaindtl bchain = new billchaindtl();
            Customer customers = new Customer();

            if (calltype != "S")
            {
                bchain = Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);

                //this.panel2.Enabled = true;
                //chkinpatient.Visible = true;

                //ThisForm.Check1.Enabled = .t. - Query Procedure/Test Definitions for this Patient
                vm.REPORTS.txtgroupcode = bchain.GROUPCODE;
                vm.REPORTS.txtpatientno = bchain.PATIENTNO;
                vm.REPORTS.TXTPATIENTNAME = bchain.NAME;

                patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);
                customers = Customer.GetCustomer(bchain.GROUPHEAD);

                if (bchain.GROUPHTYPE == "P")
                {
                    if (patients == null)
                    {
                        vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS'";
                        //btnclose.PerformClick();
                        return vm;
                    }
                    vm.REPORTS.cbotype = bchain.PATIENTNO == bchain.GROUPHEAD ? "S" : "P";

                    // SELF P\Another Patient Corporate Client
                }
                else
                {
                    if (customers == null)
                    {
                        vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information... 'Check RESPONSIBLE FOR BILLS'";
                        //btnclose.PerformClick();
                        return vm;
                    }

                    vm.REPORTS.cbotype = "C";
                }

                string mbill_cir = (bchain.GROUPHTYPE == "C") ? customers.Bill_cir : patients.bill_cir;

                vm.REPORTS.txtothername = (bchain.GROUPHTYPE == "P" && bchain.GROUPHEAD == patients.patientno) ?
                    "< SELF >" : (bchain.GROUPHTYPE == "C") ? customers.Name : patients.name;

                vm.REPORTS.mcanalter = (mbill_cir == "C") ? true : false;
                //mpatcateg = bchain.PATCATEG;
                vm.REPORTS.txtgrouphead = bchain.GROUPHEAD;
                vm.REPORTS.txtghgroupcode = bchain.GHGROUPCODE;

                vm.REPORTS.cbotype = (bchain.GROUPHTYPE == "P" && bchain.GROUPHEAD == patients.patientno) ?
                    "S" : (bchain.GROUPHTYPE == "C") ? "C" : "P";

                //panel1.Enabled = txtpatientno.Enabled = txtgroupcode.Enabled = cboBillspayable.Enabled = btngroupcode.Enabled = chkgetdependants.Enabled = false;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(mmisc_patient_code))
                {
                    vm.REPORTS.alertMessage = "Misc. Patient Account Code Not Defined in Systems Setup... PLS SEE YOUR SYSTEMS ADMINISTRATOR !!!";
                    //btnclose.PerformClick();
                    return vm;
                }

                //  combolist("N"); //loads name from suspense
                //combolist("A"); //loads address from suspense.  It should be disabled if it slowed down system 28-11-2013
                //this.panel2.Enabled = true;
            }

            //check for previous requests
            DisplayPrevDefinitions(mrattend);

            return vm;
        }


        public JsonResult precedureReqClicked(string consultReference, string calltype)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Mrattend mrattend = new Mrattend();
            mrattend = Mrattend.GetMrattend(consultReference);

            if (mrattend == null || string.IsNullOrWhiteSpace(mrattend.PATIENTNO))
            {
                vm.REPORTS.alertMessage = "A Patient Details must be in Focus As Reference to this process..";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            vm = initheader(calltype, mrattend);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult sendInjectionAlert(string injectionNote, string txtgroupcode, string txtpatientno, string TXTPATIENTNAME,
            string txtreference, string woperator, bool chkPharmacy, bool chkInjection, bool chkOPDNurses)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.edtspinstructions = injectionNote;
            vm.REPORTS.txtgroupcode = txtgroupcode;
            vm.REPORTS.txtpatientno = txtpatientno;
            vm.REPORTS.TXTPATIENTNAME = TXTPATIENTNAME;
            vm.REPORTS.txtreference = txtreference;
            vm.REPORTS.txtemployer = woperator;
            vm.REPORTS.chkADVCorporate = chkPharmacy;
            vm.REPORTS.chkbillregistration = chkInjection;
            vm.REPORTS.chkBroughtForward = chkOPDNurses;

            frmInjectionAlert formObject = new frmInjectionAlert(vm);
            vm = formObject.btnSend_Click();

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult consultReferenceFocusOut(string consultReference, string clinic)
        {
            billchaindtl bchain = new billchaindtl();
            Mrattend mrattend = new Mrattend();
            Vstata vstata = new Vstata();
            vm.REPORTS = new MR_DATA.REPORTS();
            string start_time = DateTime.Now.ToLongTimeString();
            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start; // (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select docson, pvtcode from mrcontrol order by recid", false);
            string mancclinic = dt.Rows[2]["pvtcode"].ToString();

            //if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtreference.Text))  //no lookup value obtained
            //{
            //    this.txtreference.Text = bissclass.autonumconfig(this.txtreference.Text, true, "C", "999999999");
            //}

            vm.REPORTS.newrec = true;

            //check if in attendance records
            mrattend = Mrattend.GetMrattend(consultReference);

            if (mrattend == null)
            {
                vm.REPORTS.alertMessage = "Invalid Consultation Reference in Daily Attendance Register... ";
                //this.txtreference.Text = " ";
                //this.btnclose.Focus();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            //CHECK NUMBER FORMAT
            if (consultReference.Substring(0, 1) != "C")
            {
                vm.REPORTS.alertMessage = "Not a valid Consultation Reference in Daily Attendance Register... ";
                //this.txtreference.Text = " ";
                //btnReload.PerformClick();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            //patient profile
            bchain = Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);
            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Error in Patient Master File for this Consultaton Reference...'\r\n' Pls Check and Try and Again !";
                //txtreference.Focus();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            //ClearControls();

            vm.REPORTS.TXTPATIENTNAME = mrattend.NAME;
            vm.REPORTS.REPORT_TYPE1 = mrattend.DTIME.ToString("dd-MM-yyyy @ HH:mm");  //for lbltrans_date
            vm.REPORTS.edtallergies = mrattend.TRANS_DATE.Date.ToString("yyyy-MM-dd"); // vstata.TRANS_DATE; for dateFrom and dateTo

            if (bchain.GROUPHTYPE == "C")
                vm.REPORTS.REPORT_TYPE2 = "< CORPORATE > " + bchain.GROUPHEAD; //for lblgrouphead.Text;
            else
                vm.REPORTS.REPORT_TYPE2 = "< PRIVATE > " + bchain.GHGROUPCODE + ": " + bchain.GROUPHEAD;

            vm.REPORTS.cbomaritalstatus = "GENDER : [ " + bchain.SEX + " ] ;    AGE : "; //for txtedtprofile.Text

            string xx = (bchain.BIRTHDATE != dtmin_date.Date) ? bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now.Date) :
            (bchain.RELATIONSH == "C") ? "Minor" : (bchain.RELATIONSH == "S" || bchain.RELATIONSH == "W" ||
            bchain.RELATIONSH == "H") ? "< Adult >" : "...";

            string xx1 = "     M_STATUS : < " + bchain.M_STATUS + " > ";

            vm.REPORTS.cbomaritalstatus = vm.REPORTS.cbomaritalstatus + xx + "; " + xx1;

            // combclinic.Text = bissclass.combodisplayitemCodeName("type_code", mrattend.CLINIC, dtclinic, "name");
            // bissclass.displaycombo(combclinic, dtclinic, mrattend.CLINIC, "type_code");
            // this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList);

            if (!string.IsNullOrWhiteSpace(clinic) && clinic.ToString().Trim() == mancclinic.Trim())
                vm.REPORTS.chkbyacctofficers = true; //for chkToClinicExclusive
            else
                vm.REPORTS.chkbyacctofficers = false; //for chkToClinicExclusive

            //if (!string.IsNullOrWhiteSpace(bchain.PICLOCATION))
            //    pictureBox1.Image = WebGUIGatway.getpicture(bchain.PICLOCATION);

            vstata = Vstata.GetVSTATA(consultReference);

            if (vstata != null)
            {
                vm.REPORTS.newrec = false;

                ////change comboboxstyle to allow display of field value
                //this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown);

                vm.REPORTS.TXTPATIENTNAME = mrattend.NAME;
                vm.REPORTS.REPORT_TYPE1 = mrattend.TRANS_DATE.ToString(); //lbltrans_date.Text
                vm.REPORTS.REPORT_TYPE3 = vstata.BPSITTING;
                vm.REPORTS.REPORT_TYPE4 = vstata.BPSTANDING;
                vm.REPORTS.txtconsultamt = vstata.COMPLAINT;
                vm.REPORTS.txtdepartment = vstata.HEADCIRCUMF.ToString();
                vm.REPORTS.REPORT_TYPE5 = vstata.OTHERS;
                vm.REPORTS.txtstaffno = vstata.PULSE;
                vm.REPORTS.categ_save = vstata.RESPIRATIO;
                vm.REPORTS.combillcycle = vstata.TEMP;
                vm.REPORTS.lblBalbfDbCr = vstata.BMP.ToString(); //for BMI
                vm.REPORTS.cbotitle = vstata.HFT.ToString(); //for height in ft
                vm.REPORTS.cbotype = vstata.HIN; //for height in inch
                vm.REPORTS.txtcontactperson = vstata.WLBS; //for weight in lbs
                vm.REPORTS.txtghgroupcode = vstata.WSTONE; //for weight in st
                vm.REPORTS.txtdiscount = vstata.HIGHT; //for height
                vm.REPORTS.nmrbalance = vstata.WEIGHT; //for weight in kg
                /*mpatientno = vstata.PATIENTNO;
                mgroupcode = vstata.GROUPCODE;*/
                //combclinic.Text = bissclass.combodisplayitemCodeName("type_code", vstata.CLINIC, dtclinic, "name");
                vm.REPORTS.txtclinic = vstata.CLINIC; //combclinic.SelectedValue
                //combdocs.Text = vstata.DOCTOR;
                vm.REPORTS.cboTribe = vstata.DOCTOR; //combdocs.SelectedValue

                ////revert to its original format
                //this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList);
            }
            else
            {
                DataTable oldvitas = Dataaccess.GetAnytable("", "MR", "select weight, hight, bpsitting, bpstanding, temp, pulse, respiratio, others, bmp,trans_date, time from vstata where groupcode = '" + mrattend.GROUPCODE + "' and patientno = '" + mrattend.PATIENTNO + "' and weight > '0' and hight > '0'", false);
                DataRow xrow = null;
                string xstring = "LastVitals : ";
                if (oldvitas.Rows.Count > 0)
                {
                    xrow = oldvitas.Rows[oldvitas.Rows.Count - 1];
                    xstring += Convert.ToDateTime(xrow["trans_date"]).ToShortDateString() + " @ " + xrow["time"].ToString() + " = ";
                    xstring += !string.IsNullOrWhiteSpace(xrow["weight"].ToString()) ? " wt-" + xrow["weight"].ToString().Trim() + " : " : "";
                    xstring += !string.IsNullOrWhiteSpace(xrow["hight"].ToString()) ? "ht-" + xrow["hight"].ToString().Trim() + " : " : "";
                    xstring += !string.IsNullOrWhiteSpace(xrow["bpsitting"].ToString()) ? "BPs-" + xrow["bpsitting"].ToString().Trim() + " : " : "";
                    xstring += !string.IsNullOrWhiteSpace(xrow["bpstanding"].ToString()) ? "BPstn-" + xrow["bpstanding"].ToString().Trim() + " : " : "";
                    xstring += !string.IsNullOrWhiteSpace(xrow["temp"].ToString()) ? "tmp-" + xrow["temp"].ToString().Trim() + " : " : "";
                    xstring += !string.IsNullOrWhiteSpace(xrow["pulse"].ToString()) ? "puls-" + xrow["pulse"].ToString().Trim() + " : " : "";
                    xstring += !string.IsNullOrWhiteSpace(xrow["respiratio"].ToString()) ? "resp-" + xrow["respiratio"].ToString().Trim() + " : " : "";
                    xstring += !string.IsNullOrWhiteSpace(xrow["others"].ToString()) ? "ots-" + xrow["others"].ToString().Trim() + " : " : "";
                    vm.REPORTS.lblBalbfDbCr = xrow["bmp"].ToString();

                    vm.REPORTS.nmrbalance = Convert.ToDecimal(xrow["weight"]);
                    vm.REPORTS.txtdiscount = Convert.ToDecimal(xrow["hight"]);
                    vm.REPORTS.Searchdesc = xstring;  //for txtprev_vitals.Text
                    //txtprev_vitals.Visible = true;

                }

                if (mrattend.ATTENDTYPE == "I")
                {
                    vm.REPORTS.REPORT_TYPE5 = "...FOR INJECTION..."; //for others
                    DataTable injcard = Dataaccess.GetAnytable("", "MR", "select description from injcard where groupcode = '" + mrattend.GROUPCODE + "' and patientno = '" + mrattend.PATIENTNO + "' and timegiven = ''", false);
                    if (injcard.Rows.Count > 0)
                    {
                        foreach (DataRow row in injcard.Rows)
                        {
                            vm.REPORTS.REPORT_TYPE5 += "\r\n";
                            vm.REPORTS.REPORT_TYPE5 += row["description"].ToString().Trim();
                        }
                    }
                }
                else if (mrattend.ATTENDTYPE == "E")
                    vm.REPORTS.REPORT_TYPE5 = "...FOR MEDICAL...";
                else if (mrattend.ATTENDTYPE == "F")
                    vm.REPORTS.REPORT_TYPE5 = "...FOR FOLLOW-UP VISIT...";
                else if (mrattend.ATTENDTYPE == "S")
                    vm.REPORTS.REPORT_TYPE5 = "...FOR SPECIALIST CONSULT...";
                else if (mrattend.ATTENDTYPE == "D")
                {
                    vm.REPORTS.REPORT_TYPE5 = "...FOR DRESSING...";
                    vm.REPORTS.ActRslt = "Nurses must make Procedure Request for DRESSING to Appropriate Service Centre for Costing...";
                }
                else if (mrattend.ATTENDTYPE == "G")
                    vm.REPORTS.REPORT_TYPE5 = "...EMERGENCY...";
                else if (mrattend.ATTENDTYPE == "R")
                    vm.REPORTS.REPORT_TYPE5 = "...FOR DRUG REFILL...";
                //get medical history for last ?? days
            }

            //nmrhight.Focus();
            //AnyCode = "";

            //btnsave.Enabled = true;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //OPD VITALS END

        
        #region
        //For Special Service Patient
        public JsonResult SSPsubmitBtnClicked(string SSPtransDate, string hiddenNewRec, string SSPhospitalNo, string SSPpatGroupCode, 
            string SSPfullName, string SSPgrouphead, string SSPfacility, string SSPghgroupcode, string SSPreferrer, string SSPbillsPayableBy, 
            string SSPrequestAlert, string SSPaddress, string SSPage, string SSPgender, string SSPphone, string SSPemail, string referenceNo,
            decimal SSPcurrentTotal, string hiddenCashPaying, string calltype, IEnumerable<MR_DATA.PPDRESSINGDTL> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.dtregistered = Convert.ToDateTime(SSPtransDate);
            vm.REPORTS.newrecString = hiddenNewRec;
            vm.REPORTS.txtpatientno = SSPhospitalNo;
            vm.REPORTS.txtgroupcode = SSPpatGroupCode;
            vm.REPORTS.TXTPATIENTNAME = SSPfullName;
            vm.REPORTS.txtgrouphead = SSPgrouphead; // for code
            vm.REPORTS.REPORT_TYPE1 = SSPfacility;
            vm.REPORTS.txtghgroupcode = SSPghgroupcode;
            vm.REPORTS.REPORT_TYPE2 = SSPreferrer;
            vm.REPORTS.txtbillspayable = SSPbillsPayableBy;
            vm.REPORTS.REPORT_TYPE3 = SSPrequestAlert;
            vm.REPORTS.txtaddress1 = SSPaddress;
            vm.REPORTS.cboAge = SSPage;
            vm.REPORTS.cbogender = SSPgender;
            vm.REPORTS.txthomephone = SSPphone;
            vm.REPORTS.txtemail = SSPemail;
            vm.REPORTS.txtreference = referenceNo;
            vm.REPORTS.nmrbalance = SSPcurrentTotal;
            vm.REPORTS.mcanalter = (hiddenCashPaying == "true") ? true : false;
            vm.REPORTS.Searchdesc = calltype;


            frmInvProcRequest formObject = new frmInvProcRequest(vm, Request.Cookies["mrName"].Value);
            vm = formObject.btnsave_Click(tableList);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult grpProcedSecondCall (string grpProcedure)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            //string xfacility;

            DataTable grpprocess = Dataaccess.GetAnytable("", "MR", "select name, amount,  grpbillbyservtype from GRPPROCEDURE where rtrim(reference) = '" + grpProcedure.Trim() + "'", false);
            vm.REPORTS.nmrBalbf = Convert.ToDecimal(grpprocess.Rows[0]["amount"]); //for grpprocedure_amount
            vm.REPORTS.chkbillregistration = Convert.ToBoolean(grpprocess.Rows[0]["grpbillbyservtype"].ToString()); //for grpbillbyservtype

            //DataTable grpdetail = Dataaccess.GetAnytable("", "MR", "select facility, process, description, amount from MRB15A where rtrim(reference) = '" + grpProcedure.Trim() + "' order by facility", false);
            vm.MRB15AS = ErpFunc.RsGet<MR_DATA.MRB15A>("MR_DATA", "select facility, process, description, amount from MRB15A where rtrim(reference) = '" + grpProcedure.Trim() + "' order by facility");

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult grpProcedureFocusOut(string grpProcedure, string saveGroup, decimal SSPcurrentTotal, string grpProcedureText)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.cmdgrpmember = true;  //for groupeditem
            vm.REPORTS.newrec = true; 

            if (!string.IsNullOrWhiteSpace(saveGroup) && grpProcedure.Trim() != saveGroup)
            {
                vm.REPORTS.alertMessage = "A group had been selected before... Two different service groups can't be selected together !";
                //cboFacility.Focus();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            DataTable grpprocess = Dataaccess.GetAnytable("", "MR", "select name, amount,  grpbillbyservtype from GRPPROCEDURE where rtrim(reference) = '" + grpProcedure.Trim() + "'", false);
            if (grpprocess.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "No Record for selected Group Definition...";
                //cboGrpprocess.Text = "";
                //cboFacility.Focus();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            bool grpbillbyservtype = Convert.ToBoolean(grpprocess.Rows[0]["grpbillbyservtype"].ToString());

            if (SSPcurrentTotal > 0 && string.IsNullOrWhiteSpace(saveGroup) && !grpbillbyservtype)
            {
                vm.REPORTS.alertMessage = "A grouped billed item(s) cannot be added to an existing selection !";
                //cboFacility.Focus();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.ActRslt = "Confirm to Load " + grpProcedureText.Trim() + "...";

            //if (result1 == DialogResult.No)
            //{
            //    cboGrpprocess.Text = "";
            //    cboFacility.Focus();
            //    //  cboGrpprocess.Focus();
            //    return;
            //}

            //load and display details to listview
            //createfacilitygroup(); //to hold facility summary 

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        void procdefine_chk(bool restrictive, bool inclusive, bool preauthorization, bool xhmo, bool foundit, bool hmoprice_found)
        {
            string xpattype = (xhmo) ? "HMO Plan Type" : "Billing Category";
            hmoprice_found = false;

            if (foundit && inclusive)
            {
                //do nothing
                hmoprice_found = true;
            }
            else if (restrictive) //whether found or not
            {
                vm.REPORTS.alertMessage = "is not on approved list for this Patient's " + xpattype +
                    "... - RESTRICTIVE !!! '\r\n' PLEASE SELECT ALTERNATIVE PROCEDURE...";
                //cboDesc.Text = "";
                //cboDesc.Focus();
                return;
            }
            else
            {
                vm.REPORTS.alertMessage = "is not on approved list for this Patient's " + xpattype;
                //if (result == DialogResult.No)
                //{
                //    cboDesc.Text = "";
                //    cboDesc.Focus();
                //    return;
                //}
                return;
            }

            if (preauthorization)
            {
                vm.REPORTS.alertMessage = "Selected Service Requires Pre-Authorization...";
                //if (result == DialogResult.No)
                //{
                //    cboDesc.Text = "";
                //    cboDesc.Focus();
                //    return;
                //}

                ////init new row for request alert and add description of request
                //int xrow = dtrequestalert.Rows.Count < 1 ? 0 : dtrequestalert.Rows.Count + 1;
                //DataRow dr = dtrequestalert.NewRow();

                //dr["name"] = cboDesc.Text;

                return;
            }

        }

        void hmoserv_check(string xcustomer, string xplantype, string procedure, bool hmoprice_found, bool restrictive, bool inclusive, bool preauthorization, bool foundit)
        {
            bool fee_for_service = false;

            Hmodetail hmodetail = new Hmodetail();
            hmodetail = Hmodetail.GetHMODETAIL(xcustomer, xplantype);

            if (hmodetail == null)
            {
                foundit = false;
                return;
            }
            else
            {
                inclusive = hmodetail.PROCINCLUSIVE;
                restrictive = hmodetail.PROCRESTRICTIVE;

                if (hmodetail.CAPAMT == 0m) //all services are fee for service no capitation but the hmo could have its own tariff so we check
                {
                    fee_for_service = true;
                }

                HMOSERVPROC hmoserv = new HMOSERVPROC();
                hmoserv = HMOSERVPROC.GetHMOSERVPROC(xcustomer, xplantype, procedure);

                if (hmoserv != null)
                {
                    /* msmrfunc.mrGlobals.waitwindowtext = "FOUND HMO PROCEDURE PRICE LIST...";
                     pleaseWait.Show();*/
                    foundit = true;
                    hmoprice_found = true;
                    vm.REPORTS.nmrbalance = hmoserv.AMOUNT; //for nmramount
                    vm.REPORTS.mcanalter = (hmoserv.CAPITATED) ? true : false; //for iscapitated
                    preauthorization = hmoserv.AUTHORIZATIONREQUIRED;
                    vm.REPORTS.nmrcurdebit = vm.REPORTS.nmrbalance; //for amtsave;
                }

                procdefine_chk(restrictive, inclusive, preauthorization, true, foundit, hmoprice_found);
            }

            return;
        }

        public JsonResult SSPservRequiredFocusOut(string SSPservRequired, string SSPfacility, string SSPhospitalNo, string SSPpatGroupCode,
            string SSPbillsPayableBy)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();
            DataTable CustClass = custclass.GetCUSTCLASS();

            string xdesc = "";
            string xfacility = "";
            //string Notes = "";
            string procedure;
            string mpatcateg = "";
            string groupType = "";
            string groupHead = "";
            string hmoServType = "";


            bchain = Getbillchain(SSPhospitalNo, SSPpatGroupCode);

            if (bchain != null)
            {
                mpatcateg = bchain.PATCATEG;
                groupType = bchain.GROUPHTYPE;
                groupHead = bchain.GROUPHEAD;
                hmoServType = bchain.HMOSERVTYPE;
            }

            procedure = SSPservRequired;

            vm.REPORTS.nmrbalance = msmrfunc.getFeefromtariff(procedure, mpatcateg, ref xdesc, ref xfacility);  //nmrAmount.Value

            //check for hmo and special discount
            bool hmoprice_found = false;
            bool restrictive = false;
            bool inclusive = false;
            bool preauthorization = false;
            bool foundit = false;

            if (SSPbillsPayableBy != "S" && groupType == "C" && !string.IsNullOrWhiteSpace(hmoServType))
            {
                hmoserv_check(groupHead, hmoServType, procedure, hmoprice_found, restrictive, inclusive, preauthorization, foundit);
            }

            if (!hmoprice_found && !string.IsNullOrWhiteSpace(SSPhospitalNo) && string.IsNullOrWhiteSpace(hmoServType) && !string.IsNullOrWhiteSpace(mpatcateg))
            {
                foreach (DataRow row in CustClass.Rows)
                {
                    if (row["reference"].ToString() == mpatcateg && Convert.ToBoolean(row["DEFINEPROC"]) == true)
                    {
                        //check drgdefine on current item
                        inclusive = Convert.ToBoolean(row["PROCINCLUSIVE"]);
                        restrictive = Convert.ToBoolean(row["PROCRESTRICTIVE"]);
                        preauthorization = false;

                        PROCPROFILE procprofile = PROCPROFILE.GetPROCPROFILE(mpatcateg, procedure);
                        if (procprofile != null)
                        {
                            foundit = true;
                            vm.REPORTS.nmrbalance = procprofile.AMOUNT;  //for nmrAmount.Value
                            vm.REPORTS.mcanalter = (procprofile.CAPITATED) ? true : false;  //for iscapitated
                            preauthorization = procprofile.AUTHORIZATIONREQUIRED;
                            vm.REPORTS.nmrcurdebit = vm.REPORTS.nmrbalance;  //amtsave
                        }

                        procdefine_chk(restrictive, inclusive, preauthorization, false, foundit, hmoprice_found);
                        break;
                    }
                }
            }

            if (!hmoprice_found && !string.IsNullOrWhiteSpace(SSPhospitalNo))
            {
                //check for discount percentage in patient or customer and apply
                if (msmrfunc.mrGlobals.percentageDiscountToApply != 0m)
                {
                    // msmrfunc.mrGlobals.waitwindowtext = "Discounted :" + msmrfunc.mrGlobals.percentageDiscountToApply.ToString() +"% on " + nmrAmount.Value;
                    //  pleaseWait.Show();
                    decimal xdisc = (vm.REPORTS.nmrbalance * msmrfunc.mrGlobals.percentageDiscountToApply) / 100;
                    vm.REPORTS.nmrbalance = vm.REPORTS.nmrbalance - xdisc;  //for nmrAmount.Value 
                    vm.REPORTS.nmrcurdebit = vm.REPORTS.nmrbalance;  //for amtsave
                }
            }

            vm.REPORTS.REPORT_BY_DATE = true; //for chknotes.Enabled
            //nmrAmount.Focus();

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        void combolist(string xrequired)
        {
            if (xrequired == "N")
                vm.SUSPENSES = ErpFunc.RsGet<MR_DATA.SUSPENSE>("MR_DATA", "SELECT DISTINCT name FROM suspense ORDER BY NAME");
            //selectstring = "SELECT DISTINCT name FROM suspense ORDER BY NAME";
            else if (xrequired == "D") //&& !string.IsNullOrWhiteSpace(combclinic.Text) )
                vm.TARIFFS = ErpFunc.RsGet<MR_DATA.TARIFF>("MR_DATA", "SELECT NAME, REFERENCE FROM tariff WHERE rtrim(category) = '" + vm.REPORTS.lblStaffNumber.Trim() + "' ORDER BY NAME");
            //selectstring = "SELECT NAME, REFERENCE FROM tariff WHERE rtrim(category) = '" + vm.REPORTS.lblStaffNumber.Trim() + "' ORDER BY NAME";
            else
            {
                vm.SUSPENSES = ErpFunc.RsGet<MR_DATA.SUSPENSE>("MR_DATA", "SELECT DISTINCT address1 FROM suspense ORDER BY ADDRESS1");
                //selectstring = "SELECT DISTINCT address1 FROM suspense ORDER BY ADDRESS1";
            }

            return;
        }

        bool processFacilitySetting(string SSPfacility)
        {

            vm.REPORTS.lblStaffNumber = SSPfacility; //for lblfacility

            vm.MRSETUP.BILLCODE = ErpFunc.CLGet("MR_DATA",
               "SELECT billcode from mrsetup where rtrim(facility) = '" + SSPfacility.Trim() + "'");

            if (string.IsNullOrWhiteSpace(vm.MRSETUP.BILLCODE))
            {
                return false;
            }

            //if (!msmrfunc.setupinfo(SSPfacility, ref xt, ref xreqrpt, ref billcode, ref emailphone, ref invrpt, ref invbatch, ref hjustify, ref autorptheader))
            //{
            //    //cboDesc.Text = ""; nmrAmount.Value = 0;
            //    //cboFacility.Focus();
            //    return false;
            //}

            return true;
        }

        public JsonResult SSPfacilityFocusOut(string SSPfacility)
        {
            vm.MRSETUP = new MR_DATA.MRSETUP();
            vm.REPORTS = new MR_DATA.REPORTS();

            if (processFacilitySetting(SSPfacility))
            {
                combolist("D"); //gets list of defined procedures for this facility
                //cboDesc.Focus();
            }
            else
            {
                vm.REPORTS.alertMessage = "Unable to Load Setup Info for This Service Centre...  Pls Contact Your Systems Administrator !!!";
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SSPgroupheadFocusOut(string SSPgrouphead, string SSPghgroupcode, string mgrouphtype)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Customer customer = new Customer();
            patientinfo patgrphead = new patientinfo();

            if (mgrouphtype == "P")
                patgrphead = patientinfo.GetPatient(SSPgrouphead, SSPghgroupcode);
            else
                customer = Customer.GetCustomer(SSPgrouphead);

            //dtregistered.Value = DateTime.Now;

            if (mgrouphtype == "P" && patgrphead == null || mgrouphtype == "C" && customer == null)
            {
                vm.REPORTS.alertMessage = "Invalid GroupHead Specification as responsible for Bills";
                //txtgrouphead.Text = "";
                //txtgrouphead.Select();
            }
            else
            {
                if (mgrouphtype == "P")
                {
                    if (patgrphead != null)
                    {
                        vm.REPORTS.mcanalter = (patgrphead.bill_cir == "C") ? true : false; //for CashPaying
                        //mpatcateg = patgrphead.patcateg;
                    }
                }
                else
                {
                    if (customer != null)
                    {
                        vm.REPORTS.mcanalter = (customer.Bill_cir == "C") ? true : false; //for CashPaying
                        //mpatcateg = customer.Patcateg;
                    }
                }

                if (mgrouphtype == "P")
                {
                    vm.REPORTS.txtothername = patgrphead.name;
                }
                else {
                    if (customer != null)
                    {
                        vm.REPORTS.txtothername = customer.Name;
                    }
                }

                //vm.REPORTS.txtothername = (mgrouphtype == "P") ? patgrphead.name : customer.Name;

                if (mgrouphtype == "P" && !patgrphead.isgrouphead)
                {
                    vm.REPORTS.alertMessage = "Specified Patient is not a registered GroupHead...";
                    //txtgrouphead.Text = "";
                    //txtgrouphead.Select();
                }

            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SSPbillsPayableByFocusOut(string SSPbillsPayableBy, string SSPhospitalNo, string SSPpatGroupCode)
        {
            string mgrouphtype = "";
            string mmisc_patient_code;

            vm.REPORTS = new MR_DATA.REPORTS();

            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT seclink, glbatchno, automedbil, name, attendlink from mrcontrol order by recid", false);
            mmisc_patient_code = dt.Rows[6]["name"].ToString();

            billchaindtl bchain = new billchaindtl();

            //check if patientno exists
            bchain = Getbillchain(SSPhospitalNo, SSPpatGroupCode);

            if (SSPbillsPayableBy != null && SSPbillsPayableBy.Trim() != "")
            {
                Char xgrouphtype = SSPbillsPayableBy[0];
                mgrouphtype = xgrouphtype.ToString();
            }


            if (mgrouphtype == "S")
            {
                mgrouphtype = "P";
                vm.REPORTS.mcanalter = true; // for CashPaying

                if (string.IsNullOrWhiteSpace(SSPhospitalNo))
                {
                    //wait window nowait "You must specify Miscellaneous Patients Account Code for this Transaction..."
                    vm.REPORTS.txtghgroupcode = "PVT      ";
                    vm.REPORTS.txtgrouphead = mmisc_patient_code;
                    //txtgrouphead_LostFocus(null, null);
                    return Json(vm, JsonRequestBehavior.AllowGet);
                }
                else if (bchain.GROUPHEAD == SSPhospitalNo)
                {
                    //this.txtghgroupcode.Text = txtgroupcode.Text;
                    vm.REPORTS.txtgrouphead = SSPhospitalNo;
                    vm.REPORTS.txtothername = bchain.NAME; //cboName.Text;

                    //cboReferrer.Focus();
                    return Json(vm, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    vm.REPORTS.txtghgroupcode = "PVT      ";
                    vm.REPORTS.txtgrouphead = mmisc_patient_code;
                    //txtghgroupcode.Focus();
                    return Json(vm, JsonRequestBehavior.AllowGet);
                }
            }
            else if (mgrouphtype == "P")
            {
                //txtghgroupcode.Enabled = txtgrouphead.Enabled = true;
                //this.txtghgroupcode.Focus();
            }
            else if (mgrouphtype == "C" && !string.IsNullOrWhiteSpace(SSPhospitalNo))
            {
                vm.REPORTS.txtghgroupcode = "";
                vm.REPORTS.txtgrouphead = bchain.GROUPHEAD;
                //txtgrouphead.Enabled = true;
                //txtgrouphead_LostFocus(null, null);
            }
            else
            {
                vm.REPORTS.txtghgroupcode = "";
                //txtgrouphead.Enabled = true;
                //txtgrouphead.Focus();
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        private void SSPDisplayDetails(billchaindtl bchain, string mgrouphtype)
        {
            patientinfo patients = new patientinfo();
            Customer customers = new Customer();
            string msection = "1";
            string mbill_cir;

            if (mgrouphtype == "P")
            {
                patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE); // this.txtpatientno.Text, txtgroupcode.Text );

                if (patients == null)
                {
                    vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information...  'Check RESPONSIBLE FOR BILLS' ";
                    //txtgroupcode.Text = txtpatientno.Text = "";
                    //this.txtgroupcode.Select();
                    return;
                }
            }
            else
            {
                customers = Customer.GetCustomer(bchain.GROUPHEAD);
                if (customers == null)
                {
                    vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information...  'Check RESPONSIBLE FOR BILLS' ";
                    //txtgroupcode.Text = txtpatientno.Text = "";
                    //this.txtgroupcode.Select();
                    return;
                }

                vm.REPORTS.txtgroupcode = bchain.GROUPCODE;
                vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
                mbill_cir = (mgrouphtype == "C") ? customers.Bill_cir : patients.bill_cir;
                vm.REPORTS.txtothername = (mgrouphtype == "P" && bchain.GROUPHEAD == patients.patientno) ? "< SELF >" : (mgrouphtype == "C") ? customers.Name : patients.name;
            }

            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
            vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
            vm.REPORTS.txthomephone = bchain.PHONE;
            vm.REPORTS.txtemail = bchain.EMAIL;

            if (bchain.STATUS == "C")
            {
                vm.REPORTS.alertMessage = "PATIENT NOT VALID FOR TRANSACTION - < Cancelled >";
                //txtgroupcode.Text = txtpatientno.Text = "";
                //this.txtgroupcode.Select();
                return;
            }

            if (bchain.STATUS == "S")
            {
                vm.REPORTS.alertMessage = "PATIENT NOT VALID FOR TRANSACTION - < Suspended > ";

                return;

                //if (result == DialogResult.No)
                //{
                //    txtgroupcode.Text = txtpatientno.Text = "";
                //    this.txtgroupcode.Select();
                //    return;
                //}
            }

            if (msection == "1" && mgrouphtype == "C" && customers.HMO && bchain.HMOSERVTYPE == "")
            {
                vm.REPORTS.alertMessage = "HMO Plan Type not specified in Patient Registration Details... Incomplete HMO Patient Registration";
                //txtgroupcode.Text = txtpatientno.Text = "";
                //this.txtgroupcode.Select();
                return;
            }

            vm.REPORTS.cboAge = bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now);
            vm.REPORTS.cbogender = bchain.SEX.Substring(0, 1) == "F" ? "FEMALE" : "MALE";
            vm.REPORTS.chkgetdependants = true;
        }

        public JsonResult SSPhospitalNoFocusOut(string SSPhospitalNo, string SSPpatGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            billchaindtl bchain = new billchaindtl();

            string mgrouphtype = "";

            //check if patientno exists
            bchain = Getbillchain(SSPhospitalNo, SSPpatGroupCode);

            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";

                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            else
            {
                mgrouphtype = bchain.GROUPHTYPE;

                SSPDisplayDetails(bchain, mgrouphtype);
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region
        //Daily Attendance Records Start

        public JsonResult ClinicFocusOut(string clinic, bool chkMedicalExams, bool chkDressing, bool chkInjections,
                bool chkFollowUp, bool chkEmergency, bool chkSpecConsult, bool chkDrugRefill, string hospitalCardNo, string PatGroupCode)
        {
            Customer customers = new Customer();
            billchaindtl bchain = new billchaindtl();
            ANCREG ancreg = new ANCREG();
            Medhrec medhrec = new Medhrec();
            vm.REPORTS = new MR_DATA.REPORTS();

            //DataTable dt = msmrfunc.getcontrolsetup("MR");
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select docson, conscode, consbycli, autogcons, cashpoint, filemode, dactive, installed, seclink, Last_no, pvtcode, attendlink, fsh, regconspay, last_no, name,  attdate, attno from mrcontrol order by recid", false);

            bool cons_fee_at_docs = (bool)dt.Rows[1]["installed"];
            bool mconsbyclinic = (bool)dt.Rows[0]["consbycli"];
            bool skipConsult4NewReg = (bool)dt.Rows[0]["consbycli"];
            bool monthlyconsultflag = (bool)dt.Rows[2]["attendlink"];
            bool mautogcons = (bool)dt.Rows[0]["autogcons"];
            string mimmunizationclinic = dt.Rows[7]["name"].ToString().Substring(0, 5);  //.Substring(0, 5);
            string mancclinic = dt.Rows[2]["pvtcode"].ToString();
            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;


            bchain = Getbillchain(hospitalCardNo, PatGroupCode);
            customers = Customer.GetCustomer(bchain.GROUPHEAD);
            medhrec = Medhrec.GetMEDHREC(bchain.GROUPCODE, bchain.PATIENTNO);


            //we check from here if patient is ANC Registered
            bool topaycons;

            topaycons = true;
            if (chkMedicalExams || chkDressing || chkInjections || chkFollowUp || chkDrugRefill)
            //medical exams,dressing,injection/followup/Drug Refill/disabled auto consult generate
            {
                topaycons = false;
                vm.REPORTS.cmbsave = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            else if (cons_fee_at_docs || clinic.Trim() == mimmunizationclinic.Trim())
            {
                topaycons = false;
                vm.REPORTS.cmbsave = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            else if (chkSpecConsult) //specialist
            {
                topaycons = true;
                vm.REPORTS.cmbsave = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            if (skipConsult4NewReg && bchain.REG_DATE.Date == DateTime.Now.Date)
            {
                topaycons = false;
                vm.REPORTS.cmbsave = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            bool ancrecord = false;
            if (clinic.Trim() == mancclinic.Trim())
            {
                string xrtnancref = "";
                string rtnstring = "";
                ancrecord = ANCREG.GetANCREG(hospitalCardNo, PatGroupCode, ref rtnstring, ref xrtnancref);
                if (!ancrecord) //problem
                {
                    vm.REPORTS.alertMessage = "This Patient is not on ANC Registration Profiles... \r\n If you are sure she had been registered, her GROUPCODE or PATIENTNO may have \r\n been changed by Record Officers between the Original ANC Registration and this Attendance...\r\n Please Check and Rectify !!! \r\n YOU MUST NOT ATTEMPT RE-REGISTRATION ON THE ANTE-NATAL REGISTER TO AVOID LOSS OF EXISTING \r\n ANTE-NATAL NOTES/RECORDS ON PREVIOUS REGISTRATION \r\n, ANTE-NATAL REGISTRATION";
                    //this.ClearControls("R");
                    //txtreference.Focus();
                    return Json(vm, JsonRequestBehavior.AllowGet);
                }
                if (ancreg.EVERYVISITCONSULT)
                {
                    vm.REPORTS.txtconsultamt = ancreg.CONSULTAMT.ToString(); //txtconsultamt.Text
                    topaycons = true;
                }
                else
                {
                    vm.REPORTS.txtconsultamt = ancreg.CONSULTAMT.ToString();
                    topaycons = false;
                }
                vm.REPORTS.cmbsave = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            if (!mautogcons || bchain.GROUPHTYPE == "C" && customers.Consultation < 1 && !chkSpecConsult)
            {
                topaycons = false;
                vm.REPORTS.cmbsave = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }

            //check for monthy consultation flag for all patients
            if (monthlyconsultflag)
            {
                //no consultaton. date5 is checked here because update to date4 has not taken place, date4 works if atdocsconsult 8/6/2012
                if (medhrec == null) //new
                    topaycons = true;
                else
                    topaycons = (DateTime.Now.Year == medhrec.DATE5.Year && DateTime.Now.Month == medhrec.DATE5.Month) ? false : true;

                vm.REPORTS.cmbsave = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            if (!ancrecord && (medhrec == null || clinic.Trim() == "" && mconsbyclinic))
                topaycons = true;

            else if (medhrec != null && medhrec.DATE7 >= dtmin_date.Date && medhrec.DATE7 == DateTime.Now.Date && medhrec.CLINIC7.Trim() == clinic.Trim()) //follow up
            {
                vm.REPORTS.chkStaffProfiling = true; //For chkFollowup.Checked
                topaycons = false;
            }

            vm.REPORTS.cmbsave = true;
            vm.REPORTS.cmdgrpmember = topaycons;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DailyAttendDeleteClicked(string referenceNo)
        {
            DataTable dbillv = Billings.GetBILLING(referenceNo);

            if (dbillv.Rows.Count > 0)
            {
                vm.REPORTS.ActRslt = "This Attendance Record cannot be deleted... Bills on this Accounts will be compromised!";
            }
            else
            {
                Mrattend.DeleteMrattend(referenceNo);
                //this.ClearControls("R");
                //txtreference.Select();
                vm.REPORTS.alertMessage = "Delete Succesful!!";
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        void checkmultipleattendance(string hospitalCardNo, string PatGroupCode)
        {
            string dateNow = DateTime.Now.ToString("yyyy/MM/dd");

            string selectStatement = "SELECT reference, name, clinic, operator, dtime FROM MRATTEND WHERE " +
               "patientno = '" + hospitalCardNo + "' AND groupcode = '" + PatGroupCode + "' AND CONVERT(DATE,TRANS_DATE) = '" + dateNow + "'";

            DataTable dt = Dataaccess.GetAnytable("", "MR", selectStatement, false);

            if (dt.Rows.Count < 1)
                return;

            vm.REPORTS.REPORT_TYPE2 = "Multiple Attendance Registration Detected for this patient for Today! @ " + dt.Rows[0]["dtime"].ToString() + "...!!! \r\n CREATE A DUPLICATE ATTENDANCE RECORD FOR TODAY ?";

            //vm.REPORTS.REPORT_TYPE3 = "ARE YOU REALLY SURE YOU WANT TO DISTORT CLINIC VISIT STATISTICS FOR TODAY...?";

            return;
        }

        private bool DisplayDetails(billchaindtl bchain)
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select docson, conscode, consbycli, autogcons, cashpoint, filemode, dactive, installed, seclink, Last_no, pvtcode, attendlink, fsh, regconspay, last_no, name,  attdate, attno from mrcontrol order by recid", false);
            bool enforcecreditlimit;

            patientinfo patients = new patientinfo();
            Customer customer = new Customer();

            string mremark;
            bool cashpaying;
            string mgrouphtype = bchain.GROUPHTYPE;

            customer = Customer.GetCustomer(bchain.GROUPHEAD);

            enforcecreditlimit = (bool)dt.Rows[2]["dactive"];

            // msgeventtracker = "g";
            //DialogResult result;

            if (mgrouphtype == "P")
            {
                patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);
                if (patients == null)
                {
                    //msgeventtracker = "g";
                    vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS (PVT/FC)";
                    return false;
                }
                cashpaying = patients.bill_cir == "C" ? true : false;
            }
            else
            {
                customer = Customer.GetCustomer(bchain.GROUPHEAD);
                if (customer == null)
                {
                    vm.REPORTS.alertMessage = "Unable to Link Patient's Account Information...Check RESPONSIBLE FOR BILLS (Corporate)";
                    return false;
                }
                cashpaying = customer.Bill_cir == "C" ? true : false;
            }

            vm.REPORTS.txtgroupcode = bchain.GROUPCODE;
            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;

            mremark = (mgrouphtype == "C") ? customer.Remark : patients.remark;
            vm.REPORTS.txtothername = (mgrouphtype == "P" && bchain.GROUPHEAD == bchain.PATIENTNO) ? "< SELF >" : (mgrouphtype == "C") ? customer.Name : patients.name;

            if (mgrouphtype == "P" && bchain.PATIENTNO == bchain.PATIENTNO)
                vm.REPORTS.txtaddress1 = patients.address1.Trim();
            else
                vm.REPORTS.txtaddress1 = bchain.RESIDENCE;

            if (bchain.SECTION.Trim() != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + "\r\n" + bchain.SECTION;
            if (bchain.DEPARTMENT.Trim() != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + "\r\n" + bchain.DEPARTMENT.Trim();
            if (bchain.STAFFNO.Trim() != "")
                vm.REPORTS.txtaddress1 = vm.REPORTS.txtaddress1 + "\r\n [Staff # :" + bchain.STAFFNO.Trim() + ":" + bchain.HMOCODE.Trim() + " ]";

            //PicSelected = bchain.PICLOCATION;
            //if (!string.IsNullOrWhiteSpace(PicSelected))
            //{
            //    pictureBox1.Image = WebGUIGatway.getpicture(PicSelected);
            //}

            vm.REPORTS.edtspinstructions = " ";

            //01.09.2019 - check for Group Head Status
            if (bchain.GROUPHTYPE == "C" && customer.Custstatus == "D" || bchain.GROUPHTYPE == "P" && (patients.patstatus == "C" || patients.patstatus == "S"))
            {
                vm.REPORTS.alertMessage = "This Patient Group Head is no longer Active for Service in this Facility...\r\n\r\n PLEASE CONTACT ACCOUNTS DEPARTMENT!";
                return false;
            }

            if (mgrouphtype == "C" && customer.Trackattndform)
            {
                //this.chkattendanceform.Visible = true;
                vm.REPORTS.alertMessage = "This patient is required to fill and sign attendance form at Front Desk ...";
            }

            //string xtosection = "3";

            if (!string.IsNullOrWhiteSpace(mremark + bchain.MEDNOTES + bchain.SPNOTES))
                vm.REPORTS.edtspinstructions = "" + mremark + "\r\n-------------------------------------------------\r\n" + bchain.SPNOTES + "\r\n-------------------------------------------------\r\n" + bchain.MEDNOTES + "";


            if (bchain.STATUS == "C")
            {
                vm.REPORTS.ActRslt = "PATIENT NOT VALID FOR TRANSACTION - < Cancelled >";
                return false;
            }
            if (bchain.STATUS == "S")
            {
                // msgeventtracker = "RN";
                vm.REPORTS.ActRslt = "PATIENT NOT VALID FOR TRANSACTION - < Suspended > ";       // " CONFIRM TO CONTINUE...";

                return false;

                //if (result == DialogResult.No)
                //{
                //    ClearControls("R");
                //    txtreference.Select();
                //    return false;
                //}

                //frmOverwrite overwrite = new frmOverwrite("Overwrite Suspended Registration", "mrstlev", "MR");
                //overwrite.ShowDialog();
                //if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode)) //anycode global var is returned
                //    return false;
                ////update overwrite profile
                //msmrfunc.updateOverwrite(txtreference.Text, "Overwrite  Suspended Registration", bchain, bchain.GROUPHTYPE == "P" ? patients.cr_limit : customer.Cr_limit, nmbalance.Value);
            }

            //if (must_patphoto && (pictureBox1.Image == null || this.pictureBox1.Image.ToString() == ""))
            //{
            //    result = MessageBox.Show("Patient Photo is missing...", "Control Setup Requirement");
            //    return false;
            //}

            if (mgrouphtype == "C" && customer.HMO && bchain.HMOSERVTYPE == "")
            {
                // msgeventtracker = "g";
                vm.REPORTS.ActRslt = "HMO Plan Type not specified in Patient Registration Details... Incomplete HMO Patient Registration";
                return false;
            }

            if (bchain.GROUPHTYPE == "P") //get current balance
            {
                //if (enforcecreditlimit)
                //{
                //    nmrCreditLimit.Visible = lblCreditLimit.Visible = true;
                //    nmrCreditLimit.Value = patients.cr_limit;
                //}

                decimal db, cr, bal, adj; db = cr = bal = adj = 0m;
                bal = getOpeningBalance(bchain.GHGROUPCODE, bchain.GROUPHEAD, "", bchain.GROUPHTYPE, DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);
                db = cr = adj = 0m;
                decimal xamt = getTransactionDbCrAdjSummary(bchain.GHGROUPCODE, bchain.GROUPHEAD, "", bchain.GROUPHTYPE, DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);

                vm.REPORTS.nmrcurcredit = cr; //nmrcredit.Value 
                vm.REPORTS.nmrcurdebit = db; //nmrdebit.Value

                if (bal < 1)
                    vm.REPORTS.nmrcurcredit += Math.Abs(bal);
                else
                    vm.REPORTS.nmrcurdebit += bal;

                if (adj < 1)
                    vm.REPORTS.nmrcurcredit += Math.Abs(adj);
                else
                    vm.REPORTS.nmrcurdebit += adj;

                xamt = vm.REPORTS.nmrcurdebit - vm.REPORTS.nmrcurcredit;
                vm.REPORTS.nmrbalance = Math.Abs(xamt);
                vm.REPORTS.lblBalbfDbCr = (xamt < 1) ? "CR" : "DB";

                ////check for credit limit
                if (enforcecreditlimit && patients.cr_limit > 0 && vm.REPORTS.nmrbalance > patients.cr_limit)
                {
                    vm.REPORTS.ActRslt = "Transactions not allowed for this Patient... CREDIT LIMIT EXCEEDED ! \r\n\r\n";

                    return false;

                    //if (result == DialogResult.No)
                    //    return false;
                    //frmOverwrite overwrite = new frmOverwrite("Overwrite To Credit Limit Control", "mrstlev", "MR");
                    //overwrite.ShowDialog();
                    //if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode)) //anycode global var is returned
                    //    return false;
                    ////update overwrite profile
                    //msmrfunc.updateOverwrite(txtreference.Text, "Overwrite To Credit Limit Control", bchain, bchain.GROUPHTYPE == "P" ? patients.cr_limit : customers.Cr_limit, nmbalance.Value);
                }
            }

            vm.REPORTS.REPORT_TYPE4 = bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now); //lblage.Text
            return true;
        }

        private decimal getOpeningBalance(string ghgroupcode, string grouphead, string currencyid, string customertype, DateTime datefrom, DateTime dateto, ref decimal totdebit, ref decimal totcredit, ref decimal totadjust)
        {
            decimal acctbal;
            totcredit = totdebit = acctbal = totadjust = 0m;
            int xperiod, xyear;
            xperiod = msmrfunc.mrGlobals.mlastperiod >= 12 ? 1 : msmrfunc.mrGlobals.mlastperiod + 1;
            xyear = xperiod == 1 && msmrfunc.mrGlobals.mlastperiod != 0 ? msmrfunc.mrGlobals.mpyear + 1 : msmrfunc.mrGlobals.mpyear;
            DateTime start_opening = bissclass.ConvertStringToDateTime("01", xperiod.ToString(), xyear.ToString());
            if (datefrom < start_opening)
            {
                xperiod = datefrom.Month;
                xyear = datefrom.Year;
                start_opening = datefrom;
            }

            DataTable dtcust;
            string xfile;
            if (customertype == "C")
                xfile = string.IsNullOrWhiteSpace(currencyid) ? "customer" : "fccustom";
            else
                xfile = string.IsNullOrWhiteSpace(currencyid) ? "patient" : "fcpatient";
            string balbf = "balbf" + xperiod.ToString();
            string xselstr = customertype == "C" ? " custno = '" + grouphead + "'" : " GHGROUPCODE = '" + ghgroupcode + "' and grouphead = '" + grouphead + "'";

            if (string.IsNullOrWhiteSpace(currencyid))
                dtcust = Dataaccess.GetAnytable("", "MR", "select posted, balbf, " + balbf + " from " + xfile + "  where " + xselstr, false);
            else
                dtcust = Dataaccess.GetAnytable("", "MR", "select posted, balbf, " + balbf + " from " + xfile + "  where " + xselstr + "' and currency = '" + currencyid + "'", false);
            if (dtcust.Rows.Count < 1) //not seen
                return 0m;
            //string period = xperiod.ToString();
            //get actual op bal
            acctbal = (bool)dtcust.Rows[0]["posted"] ? (decimal)dtcust.Rows[0][balbf] : (decimal)dtcust.Rows[0]["balbf"];
            if (acctbal >= 1) //debit balance
                totdebit = acctbal;
            else
                totcredit = Math.Abs(acctbal);
            //get values from billing, payment and bill_adj
            // if ((bool)dtcust.Rows[0]["posted"])
            acctbal = getTransactionDbCrAdjSummary(ghgroupcode, grouphead, currencyid, customertype, start_opening, datefrom.AddDays(-1), ref totdebit, ref totcredit, ref totadjust);
            // else
            return acctbal;
        }

        private decimal getTransactionDbCrAdjSummary(string ghgroupcode, string grouphead, string currencyid, string customertype, DateTime datefro, DateTime datet, ref decimal totdb, ref decimal totcr, ref decimal totadj)
        {

            string xfile;
            if (customertype == "C")
                xfile = string.IsNullOrWhiteSpace(currencyid) ? "customer" : "fccustom";
            else
                xfile = string.IsNullOrWhiteSpace(currencyid) ? "patient" : "fcpatient";
            string xselstr = customertype == "C" ? " grouphead = '" + grouphead + "'" : " GHGROUPCODE = '" + ghgroupcode + "' and grouphead = '" + grouphead + "'";

            //get values from billing, payment and bill_adj
            DataTable dt;

            string dateto = datet.ToString("yyyy/MM/dd");
            string datefrom = datefro.ToString("yyyy/MM/dd");


            if (string.IsNullOrWhiteSpace(currencyid))
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS debit from billing where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto + "' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);
            else
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS debit from billing where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto + "' and currency = '" + currencyid + "' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);

            totdb += dt.Rows.Count > 0 ? (decimal)dt.Rows[0]["DEBIT"] : 0m;
            //payments
            if (string.IsNullOrWhiteSpace(currencyid))
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS credit from paydetail where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto + " 23:59:59.999' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);
            else
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS credit from paydetail where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto + " 23:59:59.999' and currency = '" + currencyid + "' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);

            totcr += dt.Rows.Count > 0 ? (decimal)dt.Rows[0]["credit"] : 0m;
            //adjustments
            xselstr = customertype == "C" ? " grouphead = '" + grouphead + "'" : " ghgroupcode = '" + ghgroupcode + "' and grouphead = '" + grouphead + "'";
            if (string.IsNullOrWhiteSpace(currencyid))
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS debit from bill_adj where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto + " 23:59:59.999' and ttype = 'D' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);
            else
                dt = Dataaccess.GetAnytable("", "MR", "select groupcode, grouphead, SUM(amount) AS debit from bill_adj where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto + " 23:59:59.999' and ttype = 'D' and currency = '" + currencyid + "' and " + xselstr + " GROUP BY GROUPCODE, GROUPHEAD", false);

            totdb += dt.Rows.Count > 0 ? (decimal)dt.Rows[0]["debit"] : 0m;
            //credit leg
            if (string.IsNullOrWhiteSpace(currencyid))
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS credit from bill_adj where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto + " 23:59:59.999' and ttype = 'C' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);
            else
                dt = Dataaccess.GetAnytable("", "MR", "select ghgroupcode, grouphead, SUM(amount) AS credit from bill_adj where trans_date >= '" + datefrom + "' and trans_date <= '" + dateto + " 23:59:59.999' and ttype = 'C' and currency = '" + currencyid + "' and " + xselstr + " GROUP BY GHGROUPCODE, GROUPHEAD", false);

            totcr += dt.Rows.Count > 0 ? (decimal)dt.Rows[0]["credit"] : 0m;

            return totdb - totcr;
        }

        public JsonResult DailyAttendHospitalNoFocusOut(string hospitalCardNo, string PatGroupCode)
        {
            billchaindtl bchain = new billchaindtl();
            Customer customers = new Customer();
            vm.REPORTS = new MR_DATA.REPORTS();
            Medhrec medhrec = new Medhrec();
            string mgrouphtype = "";

            //check if patientno exists
            bchain = Getbillchain(hospitalCardNo, PatGroupCode);
            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";  //"Invalid Patient Number... "

                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            else
            {
                mgrouphtype = bchain.GROUPHTYPE;

                if (!DisplayDetails(bchain))
                {
                    vm.REPORTS.REPORT_TYPE5 = "empty";
                    //txtpatientno.Text = txtgroupcode.Text = "";
                    //txtgroupcode.Focus();

                    return Json(vm, JsonRequestBehavior.AllowGet);
                }
            }

            //vm.REPORTS.txtclinic = "true";   //combclinic.Enabled
            //vm.REPORTS.btnFamilyGroup = true; // btnledger.Enabled

            //GET lAST ATTENDANCE DATE
            medhrec = Medhrec.GetMEDHREC(bchain.GROUPCODE, bchain.PATIENTNO);

            if (medhrec != null)
                vm.REPORTS.REPORT_TYPE1 = medhrec.DATE5.ToShortDateString() + "  @ " + medhrec.DATE5.ToShortTimeString(); // dtlastattend.Text

            checkmultipleattendance(hospitalCardNo, PatGroupCode);

            customers = Customer.GetCustomer(bchain.GROUPHEAD);

            if (customers != null)
            {
                if (bchain.GROUPHTYPE == "C" && customers.HMO) //&& !string.IsNullOrWhiteSpace(bchain.STAFFNO))
                {
                    vm.REPORTS.chkReportbyAgent = true;  //panel_EnrolleeDetails.Visible
                    vm.REPORTS.lblStaffNumber = bchain.STAFFNO; //lblEnrolleeNumber.Text
                    vm.REPORTS.lblbillonaccount = bchain.HMOSERVTYPE; //lblPlantype.Text
                    vm.REPORTS.txthomephone = bchain.PHONE; //lblEnrolleePhone.Text
                }
            }

            //chkMedicalTreatment.Focus();

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ReferenceFocusOut(string referenceNo, string serviceType, string PatGroupCode)
        {
            //Create instances
            vm.REPORTS = new MR_DATA.REPORTS();
            Mrattend mrattend = new Mrattend();
            billchaindtl bchain = new billchaindtl();

            //Initialize variables
            string woperato = Request.Cookies["mrName"].Value;
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperato + "'", false);
            bool mcanalter = (bool)dt.Rows[0]["canalter"];
            string mgrouphtype = "";

            //check if reference exist
            mrattend = Mrattend.GetMrattend(referenceNo);

            if(mrattend == null)
            {
                if (PatGroupCode.Trim() == "" && !string.IsNullOrWhiteSpace(referenceNo) && !bissclass.IsDigitsOnly(referenceNo)) //, "Daily Attendance Reference"))
                {
                    vm.REPORTS.txtreference = "";
                    return Json(vm, JsonRequestBehavior.AllowGet);
                }

                vm.REPORTS.newrec = true;
                vm.REPORTS.txtreference = bissclass.autonumconfig(referenceNo, true, (serviceType == "S") ? "S" : "C", "999999999");

            } else {

                vm.REPORTS.txtreference = referenceNo;
                //msmrfunc.mrGlobals.waitwindowtext = "Record Exists";
                vm.REPORTS.newrec = false;

                mgrouphtype = mrattend.GROUPHTYPE;  //mgrouphtype
                vm.REPORTS.txtpatientno = mrattend.PATIENTNO;
                vm.REPORTS.txtgroupcode = mrattend.GROUPCODE;
                vm.REPORTS.txtbillspayable = mrattend.GROUPHEAD;
                vm.REPORTS.txtghgroupcode = mrattend.GHGROUPCODE;

                if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtpatientno))
                {
                    bchain = Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);

                    DisplayDetails(bchain);

                    if (mrattend.POSTED)
                    {
                        // msgeventtracker = "RN";
                        vm.REPORTS.alertMessage = "Record Exist... and it's Posted...";
                        //ClearControls("R");
                        //txtreference.Select();
                        return Json(vm, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        vm.REPORTS.cmbdelete = mcanalter ? true : false;
                    }
                }
                vm.REPORTS.categ_save = "Record Exist...";
            }
            

            if (serviceType == "S") // special service patient
            {
                string msection = "1";

                if (msection != "1")
                {
                    vm.REPORTS.alertMessage = "User Defined Function Conflict...NOT A FRONT DESK OFFICER!";

                    return Json(vm, JsonRequestBehavior.AllowGet);
                }

            }
            //vm.REPORTS.TRANS_DATE1 = DateTime.Now; //dttrans_date.Value

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReferenceFocused(string serviceType, string referenceNo)
        {
            Mrattend mrattend = new Mrattend();

            decimal mlastno = 0;

            //check if reference exist
            mrattend = Mrattend.GetMrattend(referenceNo);

            if (mrattend == null)
            {
                if (serviceType == "O") //outpatient
                    mlastno = msmrfunc.getcontrol_lastnumber("CHARGNO", 3, false, mlastno, false);
                else
                    mlastno = msmrfunc.getcontrol_lastnumber("CHARGNO", 5, false, mlastno, false);
            } else
            {
                return Json(referenceNo, JsonRequestBehavior.AllowGet);
            }

            return Json(mlastno, JsonRequestBehavior.AllowGet);
        }
        //Daily Attendance Records End

        #endregion


        #region
        //Family Members Grouping Start
        public JsonResult FamGroupOnSubmit(IEnumerable<MR_DATA.BILLCHAIN> tableList, string patientNo, string groupCode, bool chkMoveTransactions)
        {
            patientinfo patients = new patientinfo();

            vm.REPORTS = new MR_DATA.REPORTS();
            patients = patientinfo.GetPatient(patientNo, groupCode);

            int xc = 0;
            string updstr = "update billchain set grouphead = '", selstr = "";
            patientinfo pattmp;

            foreach (var row in tableList)
            {
                if (groupCode.Trim() == row.GROUPCODE.Trim() && patientNo.Trim() == row.PATIENTNO.Trim() || row.STATUS != "YES")
                    continue;

                xc++;
                selstr = updstr + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                bissclass.UpdateRecords(selstr, "MR");

                pattmp = patientinfo.GetPatient(row.PATIENTNO, row.GROUPCODE);

                if (pattmp != null)
                    patientinfo.DeletePatient(row.PATIENTNO, row.GROUPCODE);

                if (chkMoveTransactions)
                {
                    //bills
                    selstr = "update billing set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', transtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //payments
                    selstr = "update paydetail set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', transtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //daily attendance
                    selstr = "update mrattend set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //medhist
                    selstr = "update medhist set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //pharmacy
                    selstr = "update dispensa set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //inpatient drugs
                    selstr = "update inpdispensa set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //Investigations
                    selstr = "update labdet set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //Admissions
                    selstr = "update admrecs set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //ante natal
                    selstr = "update ancreg set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //anc01
                    selstr = "update anc01 set grouphead = '" + patients.patientno + "', ghgroupcode = '" + patients.groupcode + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                    //anc02
                    selstr = "update anc02 set grouphead = '" + patients.patientno + "', grouphtype = '" + patients.grouphtype + "' where groupcode = '" + row.GROUPCODE + "' and patientno = '" + row.PATIENTNO + "'";
                    bissclass.UpdateRecords(selstr, "MR");
                }

                vm.REPORTS.txtgroupcode = row.GROUPCODE;
                vm.REPORTS.txtpatientno = row.PATIENTNO;
                //itm.SubItems[7].Text = "";
            }

            //chkReGroup.Checked = false;

            vm.REPORTS.alertMessage = "Completed..." + xc.ToString() + " Updated...";


            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddBtnClicked(string patientGroupCode, string hospitalNo)
        {
            vm.BILLCHAIN = new MR_DATA.BILLCHAIN();
            billchaindtl bchain = new billchaindtl();

            //check if patientno exists
            bchain = Getbillchain(hospitalNo, patientGroupCode);

            vm.BILLCHAIN.NAME = bchain.NAME;
            vm.BILLCHAIN.GROUPCODE = bchain.GROUPCODE;
            vm.BILLCHAIN.PATIENTNO = bchain.PATIENTNO;
            vm.BILLCHAIN.RESIDENCE = bchain.RESIDENCE;
            vm.BILLCHAIN.SEX = bchain.SEX;
            vm.BILLCHAIN.BIRTHDATE = bchain.BIRTHDATE;
            vm.BILLCHAIN.STATUS = "YES";

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FamMemHospitalNoFocusOut(string patientGroupCode, string hospitalNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();

            //check if patientno exists
            bchain = Getbillchain(hospitalNo, patientGroupCode);
            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";
                //txtMemberNo.Text = " ";
            }
            else
            {
                vm.REPORTS.txtothername = bchain.NAME;
            }

            //if (!string.IsNullOrWhiteSpace(bchain.PICLOCATION))
            //    pictureBox1.Image = WebGUIGatway.getpicture(bchain.PICLOCATION);

            //check if patient is already selected...
            //foreach (ListViewItem itm in listView1.Items)
            //{
            //    if (itm.SubItems[2].Text.Trim() == bchain.GROUPCODE.Trim() && itm.SubItems[3].Text.Trim() == bchain.PATIENTNO.Trim())
            //    {
            //        MessageBox.Show("This Patient is already a Member of this Family...");
            //        txtMemberNo.Text = "";
            //        return;
            //    }
            //}

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FamMembPatientFocusOut(string patientNo, string groupCode)
        {
            patientinfo patients = new patientinfo();
            vm.REPORTS = new MR_DATA.REPORTS();

            //check if patientno exists
            patients = patientinfo.GetPatient(patientNo, groupCode);
            if (patients == null || string.IsNullOrWhiteSpace(patients.name) || !patients.isgrouphead)
            {
                vm.REPORTS.alertMessage = "Invalid Record in Family Group Head...";
                //txtpatientno.Text = "";
            }
            else
            {
                vm.REPORTS.TXTPATIENTNAME = patients.name;
                //PicSelected = patients.piclocation;
                vm.BILLCHAINS = ErpFunc.RsGet<MR_DATA.BILLCHAIN>("MR_DATA",
                    "SELECT NAME, GROUPCODE, PATIENTNO, RESIDENCE, SEX, BIRTHDATE FROM BILLCHAIN WHERE GROUPHEAD = '" + patientNo + "'");

                //vm.PAYSCHEDULEs = ErpFunc.ConvertDtToList<HP_DATA.PAYSCHEDULE>(payschedule);//Converting to IEnumerable

            }


            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        //Family Members Grouping End

        #endregion


        #region
        //Group Members Information Start
        public JsonResult furthDetSubmitBtnClicked(MR_DATA.PATDETAIL dataList, string newrec)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            vm.PATDETAIL = dataList;

            frmGroupFurtherDetails formObj = new frmGroupFurtherDetails(vm);

            vm.REPORTS = formObj.savepatientdetails(newrec);

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult furtherDetBtnClicked(string patientGroupCode, string hospitalNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.PATDETAIL = ErpFunc.RGet<MR_DATA.PATDETAIL>("MR_DATA", "SELECT * FROM PATDETAIL WHERE GROUPCODE = '" + patientGroupCode + "' and patientno = '" + hospitalNo + "'");

            if (vm.PATDETAIL == null)
            {
                vm.REPORTS.newrec = true;

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.newrec = false;
            
            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult createAcctBtnClicked(MR_DATA.BILLCHAIN dataList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            vm.BILLCHAIN = dataList;

            frmPrivateAcct formObj = new frmPrivateAcct(vm, Request.Cookies["mrName"].Value);

            vm.REPORTS = formObj.savedetails();

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult privateAcctBtnClicked(string hospitalNo, string patientGroupCode, 
            string privAcctGroupCode, string privAcctPatientNo)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();
            patientinfo patients = new patientinfo();

            bchain = Getbillchain(hospitalNo, patientGroupCode);

            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "No Record";

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            //patients = patientinfo.GetPatient(privAcctPatientNo, privAcctGroupCode);
            vm.PATIENT = ErpFunc.RGet<MR_DATA.PATIENT>("MR_DATA", "SELECT * FROM PATIENT WHERE GROUPCODE = '" + privAcctGroupCode + "' and PATIENTNO = '" + privAcctPatientNo + "'");

            if (vm.PATIENT != null)
            {
                vm.REPORTS.alertMessage = "This Private Account has been created once...";

                return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
            }

            vm.REPORTS.txtgroupcode = "PVT      ";
            vm.REPORTS.txtpatientno = bchain.PATIENTNO.Trim() + "P";
            vm.REPORTS.txtbillspayable = vm.REPORTS.txtpatientno;
            vm.REPORTS.cbotype = "Private";
            vm.REPORTS.txtghgroupcode = "PVT";
            vm.REPORTS.txtsurname = bchain.SURNAME;
            vm.REPORTS.txtothername = bchain.OTHERNAMES;
            vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
            vm.REPORTS.combillcycle = "C";
            vm.REPORTS.REPORT_TYPE1 = string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);
            vm.REPORTS.cbogender = bchain.SEX;
            vm.REPORTS.cbomaritalstatus = bchain.M_STATUS;
            vm.REPORTS.REPORT_TYPE2 = string.Format("{0:yyyy-MM-dd}", bchain.BIRTHDATE);
            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
            //txtpiclocation.Text = bchain.PICLOCATION;

            

            return Json(vm.REPORTS, JsonRequestBehavior.AllowGet);
        }


        public JsonResult principalFocusOut(string principal)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            //if (string.IsNullOrWhiteSpace(AnyCode)) // && mpauto) //no lookup value obtained
            //{
            //    if (bissclass.IsDigitsOnly(txtPrinciapl.Text.Trim()))
            //        this.txtpatientno.Text = bissclass.autonumconfig(this.txtPrinciapl.Text, true, "", "9999999");
            //}

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select name from billchain where patientno = '" + principal + "'", false);

            if (dt.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number as Principal...";
                //txtPrinciapl.Text = AnyCode = "";
                //return ;
            }
            else
            {
                vm.REPORTS.lblbillonaccount = dt.Rows[0]["name"].ToString();
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult billOnAcctFocusOut(string billOnAcct, string patientGroupCode, string billsPayableBy)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            //check if patientno exists
            billchaindtl tmpchain = Getbillchain(billOnAcct, patientGroupCode);

            if (tmpchain == null || tmpchain.GROUPHEAD.Trim() != billsPayableBy.Trim())  // wrong value
            {
                vm.REPORTS.alertMessage = "Invalid Patient No. or inconsistent Grouping...BILL ON ACCOUNT !";
                //this.txtbillonacct.Text = "";
            }
            else
            {
                vm.REPORTS.lblbillonaccount = tmpchain.NAME;
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        private void grpMembDisplayPatients(string patientNo, string patientGroupCode, string groupType, string billsPayableBy, string groupCode, billchaindtl bchain)
        {
            if (string.IsNullOrWhiteSpace(billsPayableBy))
            {
                vm.REPORTS.txtbillspayable = bchain.GROUPHEAD;
                if (bchain.GROUPHTYPE == "P")
                    vm.REPORTS.txtghgroupcode = bchain.GHGROUPCODE;

                vm.REPORTS.cbotype = bchain.GROUPHTYPE == "P" ? "P" : "C";
            }

            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
            vm.REPORTS.txtbillonacct = bchain.BILLONACCT;
            vm.REPORTS.dtbirthdate = bchain.BIRTHDATE;
            vm.REPORTS.cbogender = bchain.SEX;
            vm.REPORTS.cbomaritalstatus = bchain.M_STATUS;
            vm.REPORTS.txtstaffno = bchain.STAFFNO;
            vm.REPORTS.txtrelationship = bchain.RELATIONSH == "S" ? "STAFF" : bchain.RELATIONSH == "C" ? "CHILD" : bchain.RELATIONSH == "W" ? "WIFE" : bchain.RELATIONSH == "H" ? "HUSBAND" : "N/A"; // bchain.RELATIONSH;
            vm.REPORTS.comhmoservgrp = bchain.HMOSERVTYPE;
            vm.REPORTS.nmrbalance = bchain.CUMVISITS; //For spndependants
            vm.REPORTS.txtbranch = bchain.SECTION;
            vm.REPORTS.txtdepartment = bchain.DEPARTMENT;
            vm.REPORTS.txthomephone = bchain.PHONE;
            vm.REPORTS.txtemail = bchain.EMAIL;
            vm.REPORTS.txtaddress1 = bchain.RESIDENCE;
            vm.REPORTS.dtregistered = bchain.REG_DATE;
            vm.REPORTS.TRANS_DATE1 = bchain.EXPIRYDATE;
            vm.REPORTS.txtcurrency = bchain.CURRENCY;
            vm.REPORTS.txtclinic = bchain.CLINIC;
            vm.REPORTS.txtsurname = string.IsNullOrWhiteSpace(bchain.SURNAME) ? bchain.NAME : bchain.SURNAME;
            vm.REPORTS.txtothername = bchain.OTHERNAMES;
            vm.REPORTS.cbotitle = bchain.TITLE;
            //pictureBox1.Image = bchain.PICLOCATION;
            vm.REPORTS.newrecString = bchain.HMOCODE;
            //  mgrouphtype = bchain.GROUPHTYPE;
            vm.REPORTS.edtspinstructions = bchain.SPNOTES;
            vm.REPORTS.edtallergies = bchain.MEDNOTES;
            //vm.REPORTS.pictureBox1 = bchain.PICLOCATION;

            //if (!string.IsNullOrWhiteSpace(vm.REPORTS.pictureBox1))
            //{ pictureBox1.Image = WebGUIGatway.getpicture(PicSelected); }

            //revert to its original format
            //this.combostyleset(Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList);
            string xghname = msmrfunc.GETGroupheadname(bchain.GHGROUPCODE, bchain.GROUPHEAD, bchain.GROUPHTYPE);

            if (bchain.GROUPHEAD.Trim() != billsPayableBy.Trim())
                vm.REPORTS.lblBalbfDbCr = "Current Grouphead : " + xghname; //lblcurrentgrouphead
            else
            {
                vm.REPORTS.lblBalbfDbCr = ""; //lblcurrentgrouphead
                vm.REPORTS.lblDbCr = xghname;  //lblgrouphead
            }
        }

        private MR_DATA.MR_DATAvm patientNoFocusOutGrpMemb(string patientNo, string patientGroupCode, string groupType, string billsPayableBy, string groupCode)
        {
            string mloccountry, woperator = "";
            bool misrereg, misreregpvt, misreregall, autogenreg, must_patphoto, mpauto = false;
            decimal mlastno, mduration = 0;

            bool mcanadd, mcandelete, mcanalter;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select Loccountry, PAUTO, cashpoint, filemode, dactive, Last_no, installed, attendlink from mrcontrol order by recid", false);

            mloccountry = dt.Rows[0]["Loccountry"].ToString();
            mpauto = (bool)dt.Rows[0]["PAUTO"];
            misrereg = (bool)dt.Rows[1]["cashpoint"];
            misreregpvt = (bool)dt.Rows[1]["filemode"];
            misreregall = (bool)dt.Rows[1]["dactive"];
            mduration = (decimal)dt.Rows[2]["Last_no"];
            must_patphoto = (bool)dt.Rows[4]["installed"];
            autogenreg = (bool)dt.Rows[5]["attendlink"];

            woperator = Request.Cookies["mrName"].Value;

            dt = Dataaccess.GetAnytable("", "MR", "select wseclevel, CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];

            billchaindtl bchain = new billchaindtl();
            patientinfo patients = new patientinfo();

            decimal mlastno1 = 0;

            mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 2, false, mlastno1, false);

            decimal xnum = bissclass.GetNumberFromString(patientNo);

            if (xnum > mlastno)
            {
                //msgeventtracker = "P";
                vm.REPORTS.alertMessage = "Patient Number is out of Sequence...";
                vm.REPORTS.txtpatientno = mlastno.ToString();
                //txtpatientno.Focus();
                return vm;
            }

            if (patientNo.Trim().Length < 7)
            {
                if (!bissclass.IsDigitsOnly(patientNo.Trim().Substring(patientNo.Trim().Length - 1)))
                    vm.REPORTS.txtpatientno = string.Concat(Enumerable.Repeat("0", 7 - patientNo.Trim().Length)) + patientNo.Trim();
                else
                    vm.REPORTS.txtpatientno = bissclass.autonumconfig(patientNo, true, "", "9999999");
            }

            //check if patientno exists
            bchain = Getbillchain(patientNo, patientGroupCode);
            patients = patientinfo.GetPatient(billsPayableBy, groupCode);
            if (bchain == null) //new defintion
            {
                DataTable dt2 = Dataaccess.GetAnytable("", "MR", "select name, patientno, groupcode from billchain where patientno = '" + patientNo + "'", false);
                if (dt2.Rows.Count > 0)
                {
                    vm.REPORTS.alertMessage = "This Reference is already used for " + dt2.Rows[0]["name"].ToString().Trim() + " GroupCode : " + dt2.Rows[0]["groupcode"].ToString().Trim() + "Patient Number :" + patientNo;

                    return vm;
                }

                vm.REPORTS.cmbsave = mcanadd ? true : false;
                //btnsave.Enabled = mcanadd ? true : false;

                if (groupType == "P")
                {
                    vm.REPORTS.txtaddress1 = patients.address1;
                    vm.REPORTS.txthomephone = patients.homephone;
                    vm.REPORTS.txtemail = patients.email;
                    vm.REPORTS.txtsurname = patients.surname;
                }
            }
            else
            {
                // msmrfunc.mrGlobals.waitwindowtext = "RECORD EXIST ...";
                vm.REPORTS.ActRslt = "RECORD EXIST ...";

                vm.REPORTS.btnchainedmedhistory = true;

                vm.REPORTS.cmbsave = mcanalter ? true : false;
                //btnsave.Enabled = mcanalter ? true : false;

                grpMembDisplayPatients(patientNo, patientGroupCode, groupType, billsPayableBy, groupCode, bchain);

                if (!bchain.POSTED)
                {
                    vm.REPORTS.cmbdelete = mcandelete ? true : false;
                    //btndelete.Enabled = mcandelete ? true : false;
                }

                if (bchain.GROUPHTYPE == "C")
                {
                    vm.REPORTS.cmdgrpmember = true;
                    //this.btnpvtacct.Enabled = true;
                }

                if (string.IsNullOrWhiteSpace(bchain.SURNAME))
                {
                    vm.REPORTS.txtsurname = bchain.NAME;
                    vm.REPORTS.txtothername = bchain.NAME;
                    vm.REPORTS.alertMessage = "Please edit content of SURNAME and OTHERNAMES columnes... Patient Names";
                }
            }

            return vm;
        }

        public JsonResult grpMembPatientFocusOut(string patientNo, string patientGroupCode, string groupType, string billsPayableBy, string groupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm = patientNoFocusOutGrpMemb(patientNo, patientGroupCode, groupType, billsPayableBy, groupCode);

            //Converting the date to the right format
            vm.REPORTS.REPORT_TYPE1 = string.Format("{0:yyyy-MM-dd}", vm.REPORTS.dtregistered);
            vm.REPORTS.REPORT_TYPE2 = string.Format("{0:yyyy-MM-dd}", vm.REPORTS.dtbirthdate);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public MR_DATA.MR_DATAvm txtgrouphead_LostFocus(string billsPayableBy, string groupCode, string groupType, string patientGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            Customer customer = new Customer();
            patientinfo patients = new patientinfo();

            string mpatcateg, mHMOCODE;


            if (groupType == "P")
                patients = patientinfo.GetPatient(billsPayableBy, groupCode);
            else
                customer = Customer.GetCustomer(billsPayableBy);

            if (groupType == "P" && patients == null || groupType == "C" && customer == null)
            {
                //msgeventtracker = "GH";
                vm.REPORTS.alertMessage = "Invalid GroupHead Specification as responsible for Bills";
                return vm;
            }
            else
            {
                // this.DisplayPatients();
                if ((groupType == "P"))
                {
                    vm.REPORTS.lblbillspayable = patients.name;
                }
                else
                {
                    if (customer != null)
                    {
                        vm.REPORTS.lblbillspayable = customer.Name;
                    }
                }

                //vm.REPORTS.lblbillspayable = (groupType == "P") ? patients.name : customer.Name;

                if (groupType == "P" && !patients.isgrouphead)
                {
                    //msgeventtracker = "GH";
                    vm.REPORTS.alertMessage = "Specified Patient is not a registered GroupHead...";
                    // txtgrouphead.Select();
                    return vm;
                }
            }
            if (groupType == "C" && !customer.ISGROUPHEAD)
            {
                vm.REPORTS.alertMessage = "This Client is not a grouphead...CHECK CORPORATE CIENTS REGISTARTION";
                //txtgrouphead.Text = "";
                return vm;
            }

            if (groupType == "P")
            {
                mpatcateg = patients.patcateg;
            }
            else {
                if (customer != null)
                {
                    mpatcateg = customer.Patcateg;

                    mHMOCODE = (groupType == "C" && customer.HMO) ? customer.Custno : " ";

                    if (groupType == "C" && customer.HMO)
                        vm.REPORTS.lblStaffNumber = "Enrollee Number..";
                    else
                        vm.REPORTS.lblStaffNumber = "Company Staff No.";

                }
            }

            //lblgrouphead.Text = (mgrouphtype == "P") ? patients.name : customer.Name;
            vm.REPORTS.txtgroupcode = (groupType == "P") ? groupCode : billsPayableBy;
            //mgenregistration = (groupType == "P") ? patients.billregistration : customer.Billregistration;

            //ttpf01b.SetToolTip(txtbranch, (groupType == "C" && billsPayableBy.Trim() == "PAN") ? "Enter Staff Grade Level" : " ");
            //btngroupcode.Visible = groupType == "C" ? true : false;
            //if (mgrouphtype == "C" && "NHIS".IndexOf(txtgrouphead.Text.Trim()) >= 0)
            //    if (mgrouphtype == "C" && txtgrouphead.Text.Contains("NHIS"))
            //     txtgroupcode.Text = "NHIS";


            return vm;
        }

        public JsonResult billsPayableByFocusOut(string billsPayableBy, string groupCode, string groupType, string patientGroupCode) // == groupHead FocusOut
        {
            vm = txtgrouphead_LostFocus(billsPayableBy, groupCode, groupType, patientGroupCode);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult groupMembersDelete(string billsPayableBy, string patientGroupCode)
        {
            frmGroupMembersdt formObj = new frmGroupMembersdt(vm, Request.Cookies["mrName"].Value);

            vm = formObj.btndelete_Click(billsPayableBy, patientGroupCode);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        //Group Members Information End
        #endregion


        //Chained Medical History Start
        public JsonResult chnMedclHistryBtnClicked(string patientNo, string groupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.MEDHISTCHAINS = ErpFunc.RsGet<MR_DATA.MEDHISTCHAIN>("MR_DATA", "SELECT * FROM MEDHISTCHAIN WHERE GROUPCODE = '" + groupCode + "' and patientno = '" + patientNo + "' order by name");

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult chnMedHistorySubmit(string patientNo, string groupCode, IEnumerable<MR_DATA.BILLCHAIN> tableList)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.REPORT_TYPE1 = patientNo;
            vm.REPORTS.REPORT_TYPE2 = groupCode;

            frmChainMedicalHistory formObj = new frmChainMedicalHistory(vm);
            vm = formObj.btnSubmit_Click(tableList);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult chnMedAddBtnClicked(string chnMedPatientNo, string chnMedGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            billchaindtl bchain = new billchaindtl();

            bchain = Getbillchain(chnMedPatientNo, chnMedGroupCode);

            vm.REPORTS.TXTPATIENTNAME = bchain.NAME;
            vm.REPORTS.REPORT_TYPE1 = bchain.REG_DATE.ToShortDateString();
            vm.REPORTS.cbogender = bchain.SEX;
            vm.REPORTS.cbotype = bchain.GROUPHTYPE;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult chnMedPatientNoFocusOut(string chnMedPatientNo, string chnMedGroupCode)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            billchaindtl bchain = new billchaindtl();

            //recno = 0;

            //if (string.IsNullOrWhiteSpace(AnyCode))  //no lookup value obtained
            //{
            //    this.txtpatientno.Text = bissclass.autonumconfig(this.txtpatientno.Text, true, "", "9999999");
            //}

            //check if patientno exists
            bchain = Getbillchain(chnMedPatientNo, chnMedGroupCode);

            if (bchain == null)
            {
                vm.REPORTS.alertMessage = "Invalid Patient Number... ";

                vm.REPORTS.txtpatientno = bissclass.autonumconfig(chnMedPatientNo, true, "", "9999999");
              
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.REPORTS.txtothername = bchain.NAME;  //for lblname.Text
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        //Chained Medical History End


        #region       
        //Patient Information Start 

        void GetAcctInfo(patientinfo patients)
        {

            decimal db, cr, adj; db = cr = adj = 0m;
            vm.REPORTS.nmrBalbf = getOpeningBalance(patients.groupcode, patients.patientno, "", "P", DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);
            vm.REPORTS.nmrcurcredit = cr;
            if (patients.balbf > 0)
                vm.REPORTS.nmrcurdebit = db - patients.balbf;
            else if (cr > 0)
                vm.REPORTS.nmrcurcredit = cr - Math.Abs(patients.balbf);
            //  nmrCurDebit.Value = db;
            vm.REPORTS.nmrBalbf = patients.balbf;
            vm.REPORTS.lblBalbfDbCr = patients.balbf < 1 ? "CR" : "DB";
            vm.REPORTS.nmrbalance = vm.REPORTS.nmrBalbf + vm.REPORTS.nmrcurdebit - vm.REPORTS.nmrcurcredit;
            vm.REPORTS.lblDbCr = vm.REPORTS.nmrbalance < 1 ? "CR" : "DB";

            if (patients.posted)
            {
                vm.REPORTS.nmrBalbfReadOnly = true;
            }

        }

        private void DisplayPatients(bool getAcctInfo, patientinfo patients)
        {
            string PicSelected, mgrouphtype = "";

            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.txtgroupcode = patients.groupcode;
            vm.REPORTS.txtpatientno = patients.patientno;

            //bissclass.displaycombo(cbopistate, dtstate, patients.patstate, "type_code");######

            //this.commaritalstatus.Text = patients.patstatus;
            vm.REPORTS.txtaddress1 = patients.address1;
            //  this.txtaddress2.Text = patients.address2;
            vm.REPORTS.dtbirthdate = patients.birthdate;
            vm.REPORTS.cbogender = patients.sex;
            vm.REPORTS.cbomaritalstatus = patients.m_status;
            vm.REPORTS.dtregistered = patients.reg_date;
            vm.REPORTS.txtcontactperson = patients.contact;
            vm.REPORTS.txtghgroupcode = patients.ghgroupcode;
            vm.REPORTS.cbotype = patients.pattype == "P" ? "Private" : patients.pattype == "F" ? "Family" : patients.pattype == "C" ? "Corporate" : "";

            vm.REPORTS.txtcreditlimit = patients.cr_limit.ToString();

            vm.REPORTS.nmrBalbf = patients.balbf;
            vm.REPORTS.lblBalbfDbCr = patients.balbf < 1 ? "CR" : "DB";

            vm.REPORTS.TXTPATIENTNAME = patients.name;
            vm.REPORTS.txtbillspayable = patients.grouphead;

            vm.REPORTS.cbotitle = patients.title;
            vm.REPORTS.Combillspayable = (patients.grouphead == patients.patientno) ? "SELF" :
                (patients.grouphtype == "P") ? "P\\ANOTHER PATIENT" : "CORPORATE CLIENT";

            vm.REPORTS.txtdiscount = patients.discount;
            vm.REPORTS.comhmoservgrp = patients.hmoservtype;
            vm.REPORTS.txtcurrency = patients.currency;
            vm.REPORTS.chkbillregistration = (patients.billregistration == true) ? true : false;
            vm.REPORTS.txtclinic = patients.clinic;
            vm.REPORTS.txtsurname = patients.surname;
            vm.REPORTS.txtothername = patients.othername;
            vm.REPORTS.txthomephone = patients.homephone;
            vm.REPORTS.txtworkphone = patients.workphone;
            vm.REPORTS.txtemployer = patients.employer;
            vm.REPORTS.txtemployer = patients.emp_name;
            vm.REPORTS.txtemployeraddress = patients.emp_addr;
            vm.REPORTS.cboemploystate = patients.emp_state;

            vm.REPORTS.cbooccupation = patients.occupation;
            //patients.religion = reader["religion"].ToString();
            vm.REPORTS.cbobloodgroup = patients.bloodgroup;
            vm.REPORTS.cbogenotype = patients.genotype;
            vm.REPORTS.txtnextofkin = patients.nextofkin;
            vm.REPORTS.txtkinaddress1 = patients.nok_adr1;
            vm.REPORTS.cbokinstate = patients.nok_state;
            vm.REPORTS.txtkinphone = patients.nok_phone;
            vm.REPORTS.txtrelationship = patients.nok_relationship;
            //patients.rhd = reader["rhd"].ToString();
            vm.REPORTS.txtemail = patients.email;
            mgrouphtype = patients.grouphtype;
            PicSelected = patients.piclocation;
            vm.REPORTS.categ_save = patients.patcateg;
            vm.REPORTS.combillcycle = patients.bill_cir;
            vm.REPORTS.cboReligion = patients.religion == "C" ? "CHRISTIANITY" : patients.religion == "M" ? "MUSLIM" : "OTHERS";
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select lga, tribe FROM PATDETAIL where groupcode = '" + patients.groupcode + "' and patientno = '" + patients.patientno + "'", false);
            if (dt.Rows.Count > 0)
            {
                vm.REPORTS.cboTribe = dt.Rows[0]["tribe"].ToString();
                vm.REPORTS.cboLGA = dt.Rows[0]["lga"].ToString();
            }

            if (getAcctInfo)
            {
                GetAcctInfo(patients);
            }

            vm.REPORTS.ActRslt = "RECORD EXIST ...";
        }

        public MR_DATA.MR_DATAvm txtpatientno_LostFocus(string patientNo, string groupCode, bool getAcctInfo)
        {
            bool misreregall, autogenreg, mpauto, misrereg, misreregpvt, must_patphoto, autogenregforALL, mcanadd, mcandelete, mcanalter;
            string mlocstate, mloccountry, woperator, mregcode, mfacility, mstart_time, PicSelected = "";
            string AnyCode, Anycode1 = "";
            decimal mduration, mlastno = 0; // mlastnumb

            billchaindtl bchain = new billchaindtl();
            patientinfo patients = new patientinfo();

            //getControlSettings code
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select Loccountry, locstate, PAUTO, cashpoint, filemode, dactive, Last_no, installed, attendlink, regcode,dischtime,autogreg from mrcontrol order by recid", false); //msmrfunc.getcontrolsetup("MR");

            woperator = Request.Cookies["mrName"].Value;

            mloccountry = dt.Rows[0]["Loccountry"].ToString();
            mlocstate = dt.Rows[0]["locstate"].ToString();
            mpauto = (bool)dt.Rows[0]["PAUTO"];
            mregcode = dt.Rows[0]["regcode"].ToString();
            autogenreg = (bool)dt.Rows[0]["autogreg"];

            misrereg = (bool)dt.Rows[1]["cashpoint"];
            misreregpvt = (bool)dt.Rows[1]["filemode"];
            misreregall = (bool)dt.Rows[1]["dactive"];

            mduration = (Decimal)dt.Rows[2]["Last_no"];
            mfacility = dt.Rows[3]["dischtime"].ToString();
            must_patphoto = (bool)dt.Rows[4]["installed"];

            autogenregforALL = (bool)dt.Rows[5]["attendlink"];

            dt = Dataaccess.GetAnytable("", "MR", "select CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];


            vm.REPORTS = new MR_DATA.REPORTS();
            mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 2, false, mlastno, false); //New addition

            //vm.REPORTS.newrec = true;
            mstart_time = DateTime.Now.ToLongTimeString();
            PicSelected = "";


            if (patientNo.Trim() != "MISC" && bissclass.IsDigitsOnly(patientNo.Trim()) && Convert.ToDecimal(patientNo) > mlastno)
            {
                vm.REPORTS.ActRslt = "Patient Number is out of Sequence...";

                return vm;
            }

            if (patientNo.Trim() != "MISC")
            {
                vm.REPORTS.txtpatientno = bissclass.autonumconfig(patientNo, true, "", "9999999");
            }

            //check if patientno exists
            patients = patientinfo.GetPatient(patientNo, groupCode);
            if (patients == null) //new defintion
            {
                //CHECK BILL CHAIN
                DataTable dt2 = Dataaccess.GetAnytable("", "MR", "select name, patientno, groupcode from billchain where patientno = '" + patientNo + "'", false);
                if (dt2.Rows.Count > 0)
                {
                    vm.REPORTS.ActRslt = "This Reference is already used for " + dt2.Rows[0]["name"].ToString().Trim() + " GroupCode : " + dt2.Rows[0]["groupcode"].ToString().Trim() + "Patient Number :" + patientNo;

                    AnyCode = Anycode1 = patientNo = "";
                    return vm;
                }

            }
            else
            {
                DisplayPatients(getAcctInfo, patients);
                bchain = Getbillchain(patientNo, groupCode);
                if (bchain != null)
                {
                    vm.REPORTS.edtspinstructions = bchain.SPNOTES;
                    vm.REPORTS.edtallergies = bchain.MEDNOTES;
                }

                vm.REPORTS.btnchainedmedhistory = true;
                vm.REPORTS.btnFamilyGroup = true;

                if (patients.posted == false)
                {
                    vm.REPORTS.cmbdelete = mcandelete ? true : false;
                }

                if (patients.isgrouphead == true)
                {
                    vm.REPORTS.cmdgrpmember = true;
                    vm.REPORTS.chkbillregistration = true;
                }

                //vm.REPORTS.newrec = false;
            }

            AnyCode = Anycode1 = "";

            vm.REPORTS.cmbsave = true;

            return vm;
        }

        public JsonResult patientFocusOut(string patientNo, string groupCode, bool getAcctInfo)
        {

            vm = txtpatientno_LostFocus(patientNo, groupCode, getAcctInfo);

            //Converting the date to the right format
            vm.REPORTS.REPORT_TYPE1 = string.Format("{0:yyyy-MM-dd}", vm.REPORTS.dtregistered);
            vm.REPORTS.REPORT_TYPE2 = string.Format("{0:yyyy-MM-dd}", vm.REPORTS.dtbirthdate);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult patientOnDelete(string patientNo, string groupCode )
        {
            frmPrivatePatientsdtl formObj = new frmPrivatePatientsdtl(vm, Request.Cookies["mrName"].Value);
            vm = formObj.cmbdelete_Click(patientNo, groupCode);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public MR_DATA.MR_DATAvm txtbillspayable_LostFocus(string groupCode2, string code, string mgrouphtype)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (string.IsNullOrWhiteSpace(code) || mgrouphtype == "P" && string.IsNullOrWhiteSpace(groupCode2))
            {
                vm.REPORTS.chkbyacctofficers = true;

                return vm;
            }
            else
            {
                vm.REPORTS.chkbyacctofficers = false;

                Customer customer = new Customer();
                patientinfo patgrphead = new patientinfo();
                if (mgrouphtype == "P")
                    patgrphead = patientinfo.GetPatient(code, groupCode2);
                else
                    customer = Customer.GetCustomer(code);

                if (mgrouphtype == "P" && patgrphead == null || mgrouphtype == "C" && customer == null)
                {
                    vm.REPORTS.ActRslt = "Invalid GroupHead Specification as responsible for Bills";
                    //txtbillspayable.Text = txtghgroupcode.Text = "";
                }
                else
                {
                    // this.DisplayPatients();
                    vm.REPORTS.lblbillspayable = (mgrouphtype == "P") ? patgrphead.name : customer.Name;
                    if (mgrouphtype == "P" && !patgrphead.isgrouphead)
                    {
                        vm.REPORTS.ActRslt = "Specified Patient is not a registered GroupHead...";
                        //txtbillspayable.Text = txtghgroupcode.Text = "";
                    }
                }
                if (customer != null)
                {
                    if (mgrouphtype == "C" && customer.ISGROUPHEAD == false)
                    {
                        vm.REPORTS.alertMessage = "This Client is not a grouphead...CHECK CORPORATE CIENTS REGISTARTION";
                        //txtbillspayable.Text = "";
                        return vm;
                    }
                }
            }

            return vm;
        }

        //code focusout
        public JsonResult codeFocusOut(string groupCode2, string code, string mgrouphtype)
        {
            vm = txtbillspayable_LostFocus(groupCode2, code, mgrouphtype);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult patientOnFocus()
        {
            decimal mlastno = 0;
            mlastno = msmrfunc.getcontrol_lastnumber("LAST_NO", 2, false, mlastno, false);

            return Json(mlastno, JsonRequestBehavior.AllowGet);
        }
        //For Patient Information End 
        #endregion

        public JsonResult RptSuspenseLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.SUSPENSES = ErpFunc.RsGet<MR_DATA.SUSPENSE>("MR_DATA",
                "Select NAME, REFERENCE, TRANS_DATE, FACILITY, GROUPCODE, PATIENTNO, PHONE " +
                "from SUSPENSE WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from SUSPENSE WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }



        public JsonResult RptAdmLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            if (srchVal.Length > 2 && srchVal[2] == "True")
            {
                vm.ADMRECSS = ErpFunc.RsGet<MR_DATA.ADMRECS>("MR_DATA",
                                "Select NAME, GROUPCODE, PATIENTNO, REFERENCE, ADM_DATE, DISCHARGE, ROOM, BED, GROUPHEAD, GHGROUPCODE, ACAMT, PAYMENTS " +
                                "from ADMRECS WHERE discharge='' and NAME LIKE '%" +
                                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                                " ROWS FETCH FIRST " + rows + " ROWS ONLY");

                foreach (var aa in vm.ADMRECSS)
                {
                    aa.GROUPHEAD = aa.GHGROUPCODE + " " + aa.GROUPHEAD;
                    aa.NAME += " : (" + aa.PATIENTNO + "/" + aa.GROUPCODE + ")";
                    aa.ROOM += "/" + aa.BED;
                }

                var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from ADMRECS WHERE "+
                    " discharge='' and NAME LIKE '%" + srchVal[0] + "%'"));

                if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

                SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
                {
                    ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
                };

                vm.SYSCODETABSvm = bb;
            }
            else if(srchVal.Length == 2 || srchVal.Length > 2 && srchVal[2]=="False")
            {
                vm.ADMRECSS = ErpFunc.RsGet<MR_DATA.ADMRECS>("MR_DATA",
                                "Select NAME, GROUPCODE, PATIENTNO, REFERENCE, ADM_DATE, DISCHARGE, ROOM, BED, GROUPHEAD, GHGROUPCODE, ACAMT, PAYMENTS " +
                                "from ADMRECS WHERE NAME LIKE '%" +
                                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                                " ROWS FETCH FIRST " + rows + " ROWS ONLY");

                foreach (var aa in vm.ADMRECSS)
                {
                    aa.GROUPHEAD = aa.GHGROUPCODE + " " + aa.GROUPHEAD;
                    aa.NAME += " : (" + aa.PATIENTNO + "/" + aa.GROUPCODE + ")";
                    aa.ROOM += "/" + aa.BED;
                }

                var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from ADMRECS WHERE NAME" +
                    " LIKE '%" + srchVal[0] + "%'"));

                if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

                SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
                {
                    ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
                };

                vm.SYSCODETABSvm = bb;
            }
            

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string RptAdmExists(string Id)
        {
            string retVal = "False";
            try
            {
                string c = ErpFunc.CLGet("MR_DATA", "SELECT REFERENCE " +
                    " FROM ADMRECS WHERE REFERENCE = @p1", Id.Trim());
                if (!string.IsNullOrWhiteSpace(c))
                {
                    retVal = "True";
                }
            }
            catch { }

            return retVal;
        }

        public JsonResult RptAdmFillUp(string Id)
        {
            var vm = new MR_DATA.MR_DATAvm
            {
                ADMRECS = ErpFunc.RGet<MR_DATA.ADMRECS>("MR_DATA",
                "Select NAME, GROUPCODE, PATIENTNO, REFERENCE, ADM_DATE, DISCHARGE, ROOM, BED, GROUPHEAD, GHGROUPCODE, ACAMT, PAYMENTS" +
                " FROM ADMRECS WHERE REFERENCE = @p1", Id.Trim())
            };
            vm.BILLCHAIN = new MR_DATA.BILLCHAIN { GROUPCODE = vm.ADMRECS.GROUPCODE, PATIENTNO = vm.ADMRECS.PATIENTNO };
            vm.CUSTOMER = new MR_DATA.CUSTOMER { CUSTNO = vm.ADMRECS.GROUPHEAD };
            vm.REPORTS = new MR_DATA.REPORTS
            {
                TRANS_DATE1 = Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", vm.ADMRECS.ADM_DATE)),
                TRANS_DATE2 = Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", vm.ADMRECS.DISCHARGE))
            };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptBillsLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.BILLINGS = ErpFunc.RsGet<MR_DATA.BILLING>("MR_DATA",
                "Select NAME, REFERENCE, TRANS_DATE, ITEMNO, AMOUNT, GROUPHEAD, GHGROUPCODE " +
                "from BILLING WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");

            foreach (var aa in vm.BILLINGS) { aa.GROUPHEAD = aa.GHGROUPCODE.Trim() + ":" + aa.GROUPHEAD.Trim(); }

            var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from BILLING WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string RptBillsExists(string Id)
        {
            string retVal = "False";
            try
            {
                string c = ErpFunc.CLGet("MR_DATA", "SELECT REFERENCE " +
                    " FROM BILLING WHERE REFERENCE = @p1", Id.Trim());
                if (!string.IsNullOrWhiteSpace(c))
                {
                    retVal = "True";
                }
            }
            catch { }

            return retVal;
        }
        public JsonResult RptBillsFillUp(string Id)
        {
            var vm = new MR_DATA.MR_DATAvm
            {
                BILLING = ErpFunc.RGet<MR_DATA.BILLING>("MR_DATA",
                "Select REFERENCE, AMOUNT, NAME, GHGROUPCODE, PATIENTNO, TRANS_DATE FROM BILLING WHERE REFERENCE = @p1", Id.Trim())
            };
            vm.BILLCHAIN = new MR_DATA.BILLCHAIN { GROUPCODE = vm.BILLING.GHGROUPCODE, PATIENTNO = vm.BILLING.PATIENTNO };

            vm.REPORTS = new MR_DATA.REPORTS
            {
                TRANS_DATE1 = Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", vm.BILLING.TRANS_DATE)),
                TRANS_DATE2 = Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", vm.BILLING.TRANS_DATE))
            };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptCustomerLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.CUSTOMERS = ErpFunc.RsGet<MR_DATA.CUSTOMER>("MR_DATA",
                "Select NAME, CUSTNO, ADDRESS1 " +
                "from CUSTOMER WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from CUSTOMER WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptPhl01LookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.PHL01S = ErpFunc.RsGet<MR_DATA.PHL01>("MR_DATA",
                "Select NAME, REFERENCE, GROUPCODE, PATIENTNO, FACILITY, ADDRESS1, SEX " +
                "from PHL01 WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from PHL01 WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptPatientLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.PATIENTS = ErpFunc.RsGet<MR_DATA.PATIENT>("MR_DATA",
                "Select NAME, GROUPCODE, PATIENTNO, ADDRESS1, SEX, HOMEPHONE " +
                "from PATIENT WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from CUSTOMER WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptPaydetailLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.PAYDETAILS = ErpFunc.RsGet<MR_DATA.PAYDETAIL>("MR_DATA",
                "Select NAME, REFERENCE, TRANS_DATE, ITEMNO, AMOUNT, GROUPHEAD " +
                "FROM PAYDETAIL WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from PAYDETAIL WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string RptCustomerExists(string Id)
        {
            string retVal = "False";
            try
            {
                string c = ErpFunc.CLGet("MR_DATA", "SELECT CUSTNO " +
                    " FROM CUSTOMER WHERE CUSTNO = @p1", Id.Trim());
                if (!string.IsNullOrWhiteSpace(c))
                {
                    retVal = "True";
                }
            }
            catch { }

            return retVal;
        }

        public JsonResult RptCustomerFillUp(string Id)
        {
            var vm = new MR_DATA.MR_DATAvm
            {
                CUSTOMER = ErpFunc.RGet<MR_DATA.CUSTOMER>("MR_DATA",
                "Select CUSTNO, NAME FROM CUSTOMER WHERE CUSTNO = @p1", Id.Trim())
            };
            vm.BILLCHAIN = new MR_DATA.BILLCHAIN { NAME = vm.CUSTOMER.NAME };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptANC01LookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.ANC01S = srchVal.Length > 2 ?
                ErpFunc.RsGet<MR_DATA.ANC01>("MR_DATA",
                "Select NAME, REFERENCE, REG_DATE, DEL_DATE, GROUPCODE, PATIENTNO " +
                "from ANC01 WHERE NAME LIKE '%" +
                srchVal[0] + "%' AND GROUPCODE='" + srchVal[2] + "' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY")
                :
                ErpFunc.RsGet<MR_DATA.ANC01>("MR_DATA",
                "Select NAME, REFERENCE, REG_DATE, DEL_DATE, GROUPCODE, PATIENTNO " +
                "from ANC01 WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = srchVal.Length > 2 ?
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from ANC01 WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%' AND GROUPCODE='" + srchVal[2] + "' "))
                :
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from ANC01 WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RptANCREGLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.ANCREGS = srchVal.Length > 2 ?
                ErpFunc.RsGet<MR_DATA.ANCREG>("MR_DATA",
                "Select NAME, REFERENCE, REG_DATE, DEL_DATE, GROUPCODE, PATIENTNO " +
                "from ANCREG WHERE NAME LIKE '%" +
                srchVal[0] + "%' AND GROUPCODE='" + srchVal[2] + "' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY")
                :
                ErpFunc.RsGet<MR_DATA.ANCREG>("MR_DATA",
                "Select NAME, REFERENCE, REG_DATE, DEL_DATE, GROUPCODE, PATIENTNO " +
                "from ANCREG WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = srchVal.Length > 2 ?
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from ANCREG WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%' AND GROUPCODE='" + srchVal[2] + "' "))
                :
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from ANCREG WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptBillChainLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.BILLCHAINS = srchVal.Length > 2 ?
                ErpFunc.RsGet<MR_DATA.BILLCHAIN>("MR_DATA",
                "Select NAME, GROUPCODE, PATIENTNO, RESIDENCE, GROUPHEAD, SEX, PHONE " +
                "from BILLCHAIN WHERE NAME LIKE '%" +
                srchVal[0] + "%' AND GROUPCODE='" + srchVal[2] + "' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY")
                :
            ErpFunc.RsGet<MR_DATA.BILLCHAIN>("MR_DATA",
                "Select NAME, GROUPCODE, PATIENTNO, RESIDENCE, GROUPHEAD, SEX, PHONE " +
                "from BILLCHAIN WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = srchVal.Length > 2 ?
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from BILLCHAIN WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%' AND GROUPCODE='" + srchVal[2] + "' "))
                :
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from BILLCHAIN WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptMRATTENDLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.MRATTENDS = srchVal.Length > 2 ?
                ErpFunc.RsGet<MR_DATA.MRATTEND>("MR_DATA",
                "Select NAME, REFERENCE, TRANS_DATE, CLINIC, GROUPCODE, PATIENTNO " +
                "from MRATTEND WHERE NAME LIKE '%" +
                srchVal[0] + "%' AND GROUPCODE='" + srchVal[2] + "' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY")
                :
            ErpFunc.RsGet<MR_DATA.MRATTEND>("MR_DATA",
                "Select NAME, REFERENCE, TRANS_DATE, CLINIC, GROUPCODE, PATIENTNO " +
                "from MRATTEND WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");

            var max = srchVal.Length > 2 ?
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from MRATTEND WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%' AND GROUPCODE='" + srchVal[2] + "' "))
                :
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from MRATTEND WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptGrpCodeLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.BILLCHAINS = srchVal.Length > 2 ? ErpFunc.RsGet<MR_DATA.BILLCHAIN>("MR_DATA",
                "Select NAME, GROUPCODE, PATIENTNO, RESIDENCE, GROUPHEAD, SEX, PHONE " +
                "from BILLCHAIN WHERE NAME LIKE '%" +
                srchVal[0] + "%' AND GROUPCODE = '" + srchVal[2] + "' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY")
                :
                ErpFunc.RsGet<MR_DATA.BILLCHAIN>("MR_DATA",
                "Select NAME, GROUPCODE, PATIENTNO, RESIDENCE, GROUPHEAD, SEX, PHONE " +
                "from BILLCHAIN WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");


            var max = srchVal.Length > 2 ? Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from BILLCHAIN WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%' AND GROUPCODE = '" + srchVal[2] + "' "))
                :
                Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from BILLCHAIN WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string RptGrpCodeExists(string Id)
        {
            string retVal = "False";
            try
            {
                string c = ErpFunc.CLGet("MR_DATA", "SELECT GROUPCODE " +
                    " FROM BILLCHAIN WHERE GROUPCODE = @p1", Id.Trim());
                if (!string.IsNullOrWhiteSpace(c))
                {
                    retVal = "True";
                }
            }
            catch { }

            return retVal;
        }

        public string RptPatNoExists(string Id)
        {
            string retVal = "False";
            try
            {
                string[] Idd = Id.Split('`');
                MR_DATA.BILLCHAIN c = ErpFunc.RGet<MR_DATA.BILLCHAIN>("MR_DATA", "SELECT PATIENTNO, GROUPHEAD" +
                    " FROM BILLCHAIN WHERE GROUPCODE = @p1 AND PATIENTNO = @p2", Idd[0].Trim(), Idd[1].Trim());
                if (c != null && !string.IsNullOrWhiteSpace(c.PATIENTNO))
                {
                    retVal = c.GROUPHEAD.Trim() == c.PATIENTNO.Trim() ? "True-yes" : "True-no";
                }
            }
            catch { }

            return retVal;
        }

        public JsonResult RptStockLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new SCS01.SCS01vm { };

            vm.stocks = ErpFunc.RsGet<SCS01.stock>("SCS01",
                "Select * from stock WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");

            var max = Convert.ToInt32(ErpFunc.CLGet("SCS01", "Select COUNT(*) from stock WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RptTariffLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new MR_DATA.MR_DATAvm { };

            vm.TARIFFS = ErpFunc.RsGet<MR_DATA.TARIFF>("MR_DATA",
                "Select NAME, REFERENCE, AMOUNT, DIFFCHARGE, CATEGORY " +
                "from TARIFF WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");

            foreach (var a in vm.TARIFFS)
            {
                a.OPERATOR = a.DIFFCHARGE == true ? "YES" : "NO";
            }

            var max = Convert.ToInt32(ErpFunc.CLGet("MR_DATA", "Select COUNT(*) from TARIFF WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string RptTariffExists(string Id)
        {
            string retVal = "False";
            try
            {
                string c = ErpFunc.CLGet("MR_DATA", "SELECT REFERENCE " +
                    " FROM TARIFF WHERE REFERENCE = @p1", Id.Trim());
                if (!string.IsNullOrWhiteSpace(c))
                {
                    retVal = "True";
                }
            }
            catch { }

            return retVal;
        }

        public JsonResult RptTariffFillUp(string Id)
        {
            var vm = new MR_DATA.MR_DATAvm
            {
                CUSTOMER = ErpFunc.RGet<MR_DATA.CUSTOMER>("MR_DATA",
                "Select CUSTNO, NAME FROM CUSTOMER WHERE CUSTNO = @p1", Id.Trim())
            };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string RptProductExists(string Id)
        {
            string c = null;
            try
            {
                if (Id.IndexOf("~") != -1)
                {
                    string[] Idd = Id.Split('~');
                    c = ErpFunc.CLGet("SCS01", "SELECT item FROM stock WHERE item = @p1 AND STORE = @p2",
                        Idd[0].Trim(), Idd[1].Trim());
                }
                else
                {
                    c = ErpFunc.CLGet("SCS01", "SELECT item FROM stock WHERE item = @p1",
                        Id.Trim());
                }
            }
            catch { }


            return string.IsNullOrWhiteSpace(c) ? "False" : "True";
        }

        public JsonResult RptProductFillUp(string Id)
        {
            SCS01.SCS01vm s = new SCS01.SCS01vm();

            if (Id.IndexOf("~") != -1)
            {
                string[] Idd = Id.Split('~');

                s.stock = ErpFunc.RGet<SCS01.stock>("SCS01",
                "Select name, item, stock_qty, store, expirydate, sell, whsell, " +
                "unit, rtlsachet, tab_capsale, packsellunit, sachetqty, sellunit " +
                "from stock WHERE item = @p1 AND store = @p2", Idd[0].Trim(), Idd[1].Trim());
            }
            else
            {
                s.stock = ErpFunc.RGet<SCS01.stock>("SCS01",
                "Select name, item, stock_qty, store, expirydate, sell, whsell, " +
                "unit, rtlsachet, tab_capsale, packsellunit, sachetqty, sellunit " +
                "from stock WHERE item = @p1", Id.Trim());
            }

            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new SCS01.SCS01vm { };

            vm.stocks = srchVal.Length > 2 ?
                ErpFunc.RsGet<SCS01.stock>("SCS01",
                "Select name, item, stock_qty, RECID, store, generic, expirydate, sell, whsell, " +
                "unit, rtlsachet, tab_capsale, packsellunit, sachetqty, sellunit, cost, status " +
                "from stock WHERE name LIKE '%" +
                srchVal[0] + "%' AND store = '" + srchVal[2] + "' ORDER BY name ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY")
                :
                ErpFunc.RsGet<SCS01.stock>("SCS01",
                "Select name, item, stock_qty, RECID, store, generic, expirydate, sell, whsell, " +
                "unit, rtlsachet, tab_capsale, packsellunit, sachetqty, sellunit, cost, status " +
                "from stock WHERE name LIKE '%" +
                srchVal[0] + "%' ORDER BY name ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");



            var max = srchVal.Length > 2 ?
                Convert.ToInt64(ErpFunc.CLGet("SCS01",
                "Select COUNT(*) from stock WHERE name LIKE '%" + srchVal[0] + "%' AND store = '" + srchVal[2] + "'"))
                :
                Convert.ToInt64(ErpFunc.CLGet("SCS01",
                "Select COUNT(*) from stock WHERE name LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl
                {
                    FillUpTable = srchVal[1] + "++++" + max.ToString()
                }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RptInvoiceLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new AR_DATA.AR_DATAvm { };

            var inv = ErpFunc.RsGet<AR_DATA.VCTRANS>("AR_DATA",
                "Select NAME, REFERENCE, TRANS_DATE, TOTITEMNO, INVAMOUNT, PAYMENT, PAYREF " +
                "from VCTRANS WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");
            vm.VCTRANSs = inv;

            var max = Convert.ToInt32(ErpFunc.CLGet("AR_DATA", "Select COUNT(*) from VCTRANS WHERE NAME" +
                " LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string RptInvoiceExists(string Id)
        {
            string retVal = "False";
            try
            {
                string c = ErpFunc.CLGet("AR_DATA", "SELECT REFERENCE " +
                    " FROM VCTRANS WHERE REFERENCE = @p1", Id.Trim());
                if (!string.IsNullOrWhiteSpace(c))
                {
                    retVal = "True";
                }
            }
            catch { }

            return retVal;
        }

        public JsonResult RptInvoiceFillUp(string Id)
        {
            var vm = new AR_DATA.AR_DATAvm
            {
                VCTRANS = ErpFunc.RGet<AR_DATA.VCTRANS>("AR_DATA",
                "Select REFERENCE, NAME, INVAMOUNT FROM VCTRANS WHERE REFERENCE = @p1", Id.Trim())
            };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string RptCSARecInconsistency(string Id)
        {
            string status = "";
            try
            {
                if (Id == "Proceed1")
                {
                    DataTable dt = Dataaccess.GetAnytable("", "AR", "select reference, customer, name from vctrans where " +
                        "len(customer) = 9 order by customer", false), dtP = new DataTable();

                    dtP = Dataaccess.GetAnytable("", "AR", "select reference, customer, name from vctpay where " +
                        "len(customer) = 9 order by customer", false);
                    int xc = dt.Rows.Count < 1 ? dtP.Rows.Count : dt.Rows.Count;
                    if (xc < 1)
                    {
                        status = "1++++Records Status!\n\nNo Errors Detected...";
                    }
                    else {
                        status = "2++++" +
                         xc.ToString() + " Records Found and will be fixed...\n\n Click OK To Continue";
                    }
                }
                else if (Id == "Proceed2")
                {
                    DataTable dt = Dataaccess.GetAnytable("", "AR", "select reference, customer, name from vctrans where " +
                        "len(customer) = 9 order by customer", false), dtP = new DataTable();
                    dtP = Dataaccess.GetAnytable("", "AR", "select reference, customer, name from vctpay where " +
                        "len(customer) = 9 order by customer", false);

                    DataTable dtcust = Dataaccess.GetAnytable("", "AR", "select reference, name from customer", false);
                    string oldcustomer = "";
                    string xrtnRef = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["customer"].ToString().Trim() != oldcustomer)
                        {
                            oldcustomer = row["customer"].ToString().Trim();

                            xrtnRef = bissclass.combodisplayitemCodeName("name", row["name"].ToString(), dtcust,
                                "reference").Trim();
                        }
                        if (xrtnRef != "" && xrtnRef != row["customer"].ToString().Trim()) //wrong reference
                        {
                            bissclass.UpdateRecords("update vctrans set customer = '" + xrtnRef + "' where reference = '"
                                + row["reference"].ToString() + "'", "AR");
                            bissclass.UpdateRecords("update vctdetail set customer = '" + xrtnRef + "' where reference = '"
                                + row["reference"].ToString() + "'", "AR");
                        }

                    }
                    oldcustomer = "";
                    xrtnRef = "";
                    foreach (DataRow row in dtP.Rows)
                    {
                        if (row["customer"].ToString().Trim() != oldcustomer)
                        {
                            oldcustomer = row["customer"].ToString().Trim();

                            xrtnRef = bissclass.combodisplayitemCodeName("name", row["name"].ToString(), dtcust,
                                "reference").Trim();
                        }
                        if (xrtnRef != "" && xrtnRef != row["customer"].ToString().Trim()) //wrong reference
                        {
                            bissclass.UpdateRecords("update vctpay set customer = '" + xrtnRef + "' where reference = '" +
                                row["reference"].ToString() + "'", "AR");
                            bissclass.UpdateRecords("update vctpayde set customer = '" + xrtnRef + "' where reference = '" +
                                row["reference"].ToString() + "'", "AR");
                        }

                    }
                    status = "Records Fixed Successfully!\n\nThank You...";
                }
            }
            catch (Exception ex)
            {
                status = "AN ERROR OCCURED\n\n" + ex.Message.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ")
                    + "\n\n" + ex.Source + "\n\n" + ex.StackTrace;
            }

            return status;
        }

        public string RptAccessCode(string Id)
        {
            string retVal = "False";
            if (Id == "ARaccessUtils") { retVal = "True"; }
            return retVal;
        }

        public string CustomerExists(string Id)
        {
            string retVal = "False";
            try
            {
                AR_DATA.CUSTOMER c = ErpFunc.RGet<AR_DATA.CUSTOMER>("AR_DATA",
                    "SELECT * FROM CUSTOMER WHERE CSTATUS!='D' AND REFERENCE = @p1",
                    Id.Trim());

                if (c != null)
                {
                    retVal = "True";

                    if (c.CR_LIMIT > 0)
                    {
                        decimal xavcr = (Convert.ToDecimal(c.CR_LIMIT) -
                            ((Convert.ToDecimal(c.CUR_DB) + Convert.ToDecimal(c.UPCUR_DB)) - (Convert.ToDecimal(c.CUR_CR)
                            + Convert.ToDecimal(c.UPCUR_DB))));
                        if (xavcr < 1m)
                        {
                            retVal = "NO++++ has exceeded defined Credit Limit by :" + xavcr.ToString("N2");
                        }
                        else
                        {
                            retVal = "YES++++ has an available Credit of : " + xavcr.ToString("N2");
                        }
                    }
                }
            }
            catch { }

            return retVal;
        }

        public JsonResult CustomerFillUp(string Id)
        {
            var vm = new AR_DATA.AR_DATAvm
            {
                CUSTOMER = ErpFunc.RGet<AR_DATA.CUSTOMER>("AR_DATA",
                "Select NAME, REFERENCE, ADDRESS1, STATECODE, COUNTRY from CUSTOMER WHERE REFERENCE = @p1", Id.Trim())
            };
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            srchVal[0] = srchVal[0].Trim();
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new AR_DATA.AR_DATAvm { };

            var custmr = ErpFunc.RsGet<AR_DATA.CUSTOMER>("AR_DATA",
                "Select NAME, REFERENCE, ADDRESS1, EMAIL, PHONE " +
                "from CUSTOMER WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME ASC OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");
            vm.CUSTOMERs = custmr;

            var max = Convert.ToInt32(ErpFunc.CLGet("AR_DATA", "Select COUNT(*) from CUSTOMER WHERE NAME LIKE '%" + srchVal[0] + "%'"));

            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = srchVal[1] + "++++" + max.ToString() }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PSCustomerLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new AR_DATA.AR_DATAvm { };

            var custmr = ErpFunc.RsGet<AR_DATA.CUSTOMER>("AR_DATA",
                "Select NAME, REFERENCE, ADDRESS1, EMAIL, PHONE, STATECODE, COUNTRY, WHOLESALE " +
                "from CUSTOMER WHERE NAME LIKE '%" +
                srchVal[0] + "%' ORDER BY NAME OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY");
            vm.CUSTOMERs = custmr;

            var max = Convert.ToInt64(ErpFunc.CLGet("AR_DATA",
                "Select COUNT(*) from CUSTOMER WHERE NAME LIKE '%" + srchVal[0] + "%'"));
            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl
                {
                    FillUpTable = srchVal[1] + "++++" + max.ToString()
                }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PSInvoiceLookUp(string Id)
        {
            int rows = 17;
            string[] srchVal = Id.Split('~');
            int offset = Convert.ToInt16(srchVal[1]); offset--; offset *= rows;

            var vm = new AR_DATA.AR_DATAvm { };

            string cref = ErpFunc.CLGet("AR_DATA", "SELECT REFERENCE FROM CUSTOMER WHERE NAME = @p1", srchVal[0]);

            var custmr = ErpFunc.RsGet<AR_DATA.VCTRANS>("AR_DATA",
                "Select NAME, REFERENCE, TRANS_DATE, TOTITEMNO, INVAMOUNT, PAYMENT, PAYREF " +
                "from VCTRANS WHERE CUSTOMER = @p1 AND POSTED != 1 ORDER BY TRANS_DATE OFFSET " + offset +
                " ROWS FETCH FIRST " + rows + " ROWS ONLY", cref);
            vm.VCTRANSs = custmr;

            var max = Convert.ToInt64(ErpFunc.CLGet("AR_DATA",
                "Select COUNT(*) from VCTRANS WHERE CUSTOMER = @p1 AND POSTED != 1", cref));
            if (max % rows == 0) { max /= rows; } else { max /= rows; max++; }

            SYSCODETABS.SYSCODETABSvm bb = new SYSCODETABS.SYSCODETABSvm
            {
                ERPmiscl = new SYSCODETABS.ERPmiscl
                {
                    FillUpTable = srchVal[1] + "++++" + max.ToString()
                }
            };

            vm.SYSCODETABSvm = bb;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPSTableData(string Id)
        {
            var prod = ErpFunc.RsGet<AR_DATA.VCTDETAIL>("AR_DATA", "SELECT * FROM VCTDETAIL WHERE REFERENCE=@p1 " +
                " ORDER BY RECID", Id);
            var tax = ErpFunc.RsGet<AR_DATA.MISCCHG>("AR_DATA", "SELECT * FROM MISCCHG WHERE REFERENCE=@p1 " +
                " ORDER BY RECID", Id);
            string a = " ";
            int i = 0;

            foreach (var aa in prod)
            {
                i++;
                a += "<tr>" +
                    "<td class='all'><span class='serialNo'>" + i +
                    "</span> <a href='#' class='fa fa-trash deleteRow'></a></td>" +
                    "<td class='all'>" + aa.DESCRIPTN + "</td>" +
                    "<td class='all'>" + aa.QTY + "</td>" +
                    "<td class='all'>" + aa.UNIT + "</td>" +
                    "<td class='all'>" + aa.UNITPRICE + "</td>" +
                    "<td class='all'>" + aa.AMOUNT + "</td>" +
                    "<td class='all'>" + aa.PRODUCT + "</td>" +
                    "<td class='all'>" + aa.STORE + "</td>" +
                    "<td class='all'>" + aa.GLINTCODE + "</td>" +
                    "<td class='all'>OldRec</td>" +
                    "<td class='all'>" + aa.SELLUNIT + "</td>" +
                    "<td class='all'>" + aa.RECID + "</td>" +
                    "</tr>";
            }
            a += " ++++ "; i = 0;
            foreach (var bb in tax)
            {
                i++;
                a += "<tr>" +
                    "<td class='all'>" + i + "</td>" +
                    "<td class='all'>" + bb.ADJUST + "</td>" +
                    "<td class='all'>" + bb.DESCRIPTN + "</td>" +
                    "<td class='all'>" + bb.PERCENTAGE + "</td>" +
                    "<td class='all'>" + bb.AMOUNT + "</td>" +
                    "<td class='all'>" + bb.NAME + "</td>" +
                    "<td class='all'>" + bb.TTYPE + "</td>" +
                    "<td class='all'>" + bb.VALUE + "</td>" +
                    "<td class='all'>" + bb.TRANS_DATE + "</td>" +
                    "<td class='all'>" + bb.DEBITACCT + " </td>" +
                    "<td class='all'>" + bb.CREDITACCT + " </td>" +
                    "<td class='all'>OldRec</td>" +
                    "</tr>";
            }

            var vm = new AR_DATA.AR_DATAvm
            {
                SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
                {
                    ERPmiscl = new SYSCODETABS.ERPmiscl { FillUpTable = a }
                }
            };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public string PSInvoiceExists(string Id)
        {
            string[] aa = Id.Split('~');
            string retVal = "False++++ ";
            try
            {
                AR_DATA.VCTRANS c = ErpFunc.RGet<AR_DATA.VCTRANS>("AR_DATA", "SELECT CUSTOMER, POSTED" +
                    " FROM VCTRANS WHERE REFERENCE = @p1",
                    aa[0].Trim());
                if (c != null)
                {
                    if (c.CUSTOMER.Trim() != aa[1].Trim()) { retVal = "Not++++" + c.CUSTOMER; }
                    else { retVal = "True++++" + c.POSTED; }
                }
            }
            catch { }

            return retVal;
        }

    }
}