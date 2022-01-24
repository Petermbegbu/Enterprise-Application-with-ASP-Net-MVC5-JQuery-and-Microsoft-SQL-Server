using mradmin.BissClass;
using mradmin.DataAccess;
using msfunc;
using msfunc.Forms;
using OtherClasses;
using OtherClasses.FILE;
using OtherClasses.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medical_Records.Controllers
{

    public class FileController : Controller
    {
        MR_DATA.MR_DATAvm vm = new MR_DATA.MR_DATAvm();
        
        // GET: File
        public ActionResult a1()
        {
            return View();
        }

        public ActionResult PatientInformation()
        {
            return View();
        }

        public ActionResult NewPatientsGroupPrivate()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (Session["error"] != null)
            {
                vm.REPORTS.REPORT_TYPE1 = (string)Session["error"];
                Session["error"] = null;
            }
            else if (Session["patientSave"] != null)
            {
                vm = (MR_DATA.MR_DATAvm)Session["patientSave"];
                if (vm.REPORTS.ActRslt == "Successfull")
                {
                    vm.REPORTS.REPORT_TYPE1 = "Record Saved Successfully...";
                }
                else
                {
                    vm.REPORTS.REPORT_TYPE1 = "";
                }

                Session["patientSave"] = null;
            }
            else
            {
                vm.REPORTS.REPORT_TYPE1 = "";
            }

            vm.CUSTOMERS = ErpFunc.RsGet<MR_DATA.CUSTOMER>("MR_DATA", "SELECT CUSTNO, NAME FROM CUSTOMER WHERE REFERRER = '1' order by NAME");
            vm.DOCTORSS = ErpFunc.RsGet<MR_DATA.DOCTORS>("MR_DATA", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' order by NAME");
            vm.CUSTCLASSS = ErpFunc.RsGet<MR_DATA.CUSTCLASS>("MR_DATA", "SELECT REFERENCE, NAME FROM CUSTCLASS order by NAME");

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                StateCodess = ErpFunc.RsGet<SYSCODETABS.StateCodes>("SYSCODETABS", "SELECT type_code, name from statecodes order by name"),
                CountryCodess = ErpFunc.RsGet<SYSCODETABS.CountryCodes>("SYSCODETABS", "SELECT type_code, name from countrycodes order by name"),
                DesignationCodess = ErpFunc.RsGet<SYSCODETABS.DesignationCodes>("SYSCODETABS", "SELECT type_code, name from designationcodes order by name"),
                CurrencyCodess = ErpFunc.RsGet<SYSCODETABS.CurrencyCodes>("SYSCODETABS", "SELECT type_code, name from currencycodes order by name"),
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            return View(vm);
        }

        public ActionResult PatientSave(MR_DATA.MR_DATAvm VM1)
        {
            vm = VM1;

            frmPrivatePatientsdtl formObj = new frmPrivatePatientsdtl(vm, Request.Cookies["mrName"].Value);
            Session["patientSave"] = formObj.cmbsave_Click();
            Session["error"] = formObj.errorProp;
            
            return RedirectToAction("NewPatientsGroupPrivate");
        }

        public ActionResult GroupMembersInformation()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (Session["groupMemberError"] != null)
            {
                vm.REPORTS.REPORT_TYPE1 = (string)Session["groupMemberError"];
                Session["groupMemberError"] = null;
            }
            else if (Session["groupMemberSave"] != null)
            {
                vm = (MR_DATA.MR_DATAvm)Session["groupMemberSave"];
                if (vm.REPORTS.alertMessage == "Successfull")
                {
                    vm.REPORTS.REPORT_TYPE1 = "Record Saved Successfull...";
                }
                else
                {
                    vm.REPORTS.REPORT_TYPE1 = "";
                }

                Session["groupMemberSave"] = null;
            }
            else
            {
                vm.REPORTS.REPORT_TYPE1 = "";
            }

            vm.DOCTORSS = ErpFunc.RsGet<MR_DATA.DOCTORS>("MR_DATA", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' order by NAME");
            vm.CUSTCLASSS = ErpFunc.RsGet<MR_DATA.CUSTCLASS>("MR_DATA", "SELECT REFERENCE, NAME FROM CUSTCLASS order by NAME");
            vm.PATDETAILS = ErpFunc.RsGet<MR_DATA.PATDETAIL>("MR_DATA", "SELECT DISTINCT TRIBE FROM PATDETAIL order by TRIBE");
            
            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                StateCodess = ErpFunc.RsGet<SYSCODETABS.StateCodes>("SYSCODETABS", "SELECT type_code, name from statecodes order by name"),
                CountryCodess = ErpFunc.RsGet<SYSCODETABS.CountryCodes>("SYSCODETABS", "SELECT type_code, name from countrycodes order by name"),
                DesignationCodess = ErpFunc.RsGet<SYSCODETABS.DesignationCodes>("SYSCODETABS", "SELECT type_code, name from designationcodes order by name"),
                CurrencyCodess = ErpFunc.RsGet<SYSCODETABS.CurrencyCodes>("SYSCODETABS", "SELECT type_code, name from currencycodes order by name"),
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            return View(vm);
        }

        public ActionResult GroupMemberSubmit(MR_DATA.MR_DATAvm VM1)
        {
            vm = VM1;

            frmGroupMembersdt formObj = new frmGroupMembersdt(vm, Request.Cookies["mrName"].Value);
            Session["groupMemberSave"] = formObj.btnsave_Click();
            Session["groupMemberError"] = formObj.errorProp;

            return RedirectToAction("GroupMembersInformation");
        }

      
        public ActionResult FamilyMembersGroupings ()
        {
            return View();
        }

       
        public ActionResult DailyAttendanceRecords()
        {
            vm.REPORTS = new MR_DATA.REPORTS();
            
            if (Session["dailyMembSave"] != null)
            {
                vm = (MR_DATA.MR_DATAvm)Session["dailyMembSave"];

                Session["dailyMembSave"] = null;
            }
            else
            {
                vm.REPORTS.REPORT_TYPE2 = "";
                vm.REPORTS.ActRslt = "";
                vm.REPORTS.alertMessage = "";

                DataTable dt = Dataaccess.GetAnytable("", "MR", "select docson, conscode, consbycli, autogcons, cashpoint, filemode, dactive, installed, seclink, Last_no, pvtcode, attendlink, fsh, regconspay, last_no, name,  attdate, attno from mrcontrol order by recid", false);

                var dat = dt.Rows[3]["attdate"];
                if (Convert.ToDateTime(dt.Rows[3]["attdate"]) != DateTime.Now.Date) //new day, restart counter
                {
                    string updatestring = "update mrcontrol set attdate = '" +
                        string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date)  + "', attno = '0' where recid = '4'";

                    //ErpFunc.RwAlter("MR_DATA", updatestring);
                    bissclass.UpdateRecords(updatestring, "MR");
                    vm.REPORTS.nmrattendancetoday = 0;
                }
                else
                {
                    vm.REPORTS.nmrattendancetoday = (decimal)dt.Rows[3]["attno"];
                }

            }

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            vm.CUSTOMERS = ErpFunc.RsGet<MR_DATA.CUSTOMER>("MR_DATA", "SELECT CUSTNO, NAME FROM CUSTOMER WHERE referrer = '1' order by NAME");
            vm.GRPPROCEDURES = ErpFunc.RsGet<MR_DATA.GRPPROCEDURE>("MR_DATA", "select REFERENCE, NAME from grpprocedure order by name");

            return View(vm);
        }

        public ActionResult DailyAttendanceSubmit(MR_DATA.MR_DATAvm VM1)
        {
            vm = VM1;

            frmDailyAttendance formObj = new frmDailyAttendance(vm, Request.Cookies["mrName"].Value);
            Session["dailyMembSave"] = formObj.btnsave_Click();
            //Session["groupMemberError"] = formObj.errorProp;

            return RedirectToAction("DailyAttendanceRecords");
        }

        public ActionResult AnteNatalBookings_Registration()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.TARIFFS = ErpFunc.RsGet<MR_DATA.TARIFF>("MR_DATA", "SELECT REFERENCE, NAME from Tariff");
            
            vm.SCS01vm = new SCS01.SCS01vm
            {
                stocks = ErpFunc.RsGet<SCS01.stock>("SCS01", "SELECT DISTINCT name, item from stock")
            };
            
            return View(vm);
        }


        public ActionResult OPDvitalsNursesDesk()
        {
            vm.REPORTS = new MR_DATA.REPORTS();            

            if (Session["vitalSignSave"] != null)
            {
                vm = (MR_DATA.MR_DATAvm)Session["vitalSignSave"];

                Session["vitalSignSave"] = null;
            }
            else
            {
                vm.REPORTS.alertMessage = "";
            }

            if(vm.REPORTS.alertMessage == null)
            {
                vm.REPORTS.alertMessage = "";
            }

            Vstata vstata;
            if (vm.REPORTS.chkBroughtForward)
            {
                vstata = Vstata.GetVSTATA(vm.REPORTS.txtreference);

                vm.REPORTS.REPORT_TYPE5 = vstata.OTHERS;
                vm.REPORTS.txtgroupcode = vstata.GROUPCODE;
                vm.REPORTS.txtpatientno = vstata.PATIENTNO;
                vm.REPORTS.txtreference = vstata.REFERENCE;
            }

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            vm.DOCTORSS = ErpFunc.RsGet<MR_DATA.DOCTORS>("MR_DATA", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' order by NAME");
            vm.CUSTOMERS = ErpFunc.RsGet<MR_DATA.CUSTOMER>("MR_DATA", "SELECT CUSTNO, NAME FROM CUSTOMER WHERE referrer = '1' order by NAME");
            vm.GRPPROCEDURES = ErpFunc.RsGet<MR_DATA.GRPPROCEDURE>("MR_DATA", "select REFERENCE, NAME from grpprocedure order by name");

            return View(vm);
        }

        public ActionResult VitalSignsSubmit(MR_DATA.MR_DATAvm VM1)
        {
            vm = VM1;

            opdvitals formObj = new opdvitals(vm, Request.Cookies["mrName"].Value);
            Session["vitalSignSave"] = formObj.btnsave_Click();

            return RedirectToAction("OPDvitalsNursesDesk");
        }

        public ActionResult HMOAuthourizationCode()
        {
            vm.DOCTORSS = ErpFunc.RsGet<MR_DATA.DOCTORS>("MR_DATA", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' order by NAME");

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            return View(vm);
        }

        public ActionResult Waiting_AttendanceListByClinic()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (Session["Waiting_AttendanceListByClinic"] != null)
            {
                vm.REPORTS = (MR_DATA.REPORTS)Session["Waiting_AttendanceListByClinic"];
                Session["Waiting_AttendanceListByClinic"] = null;
            }

            vm.DOCTORSS = ErpFunc.RsGet<MR_DATA.DOCTORS>("MR_DATA", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' order by NAME");
            vm.CUSTOMERS = ErpFunc.RsGet<MR_DATA.CUSTOMER>("MR_DATA", "SELECT CUSTNO, NAME FROM CUSTOMER order by NAME");

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            return View(vm);
        }
        
        public ActionResult AttendListByClinicSubmit(MR_DATA.MR_DATAvm VM1)
        {
            vm = VM1;

            frmAttendanceList formObj = new frmAttendanceList(vm);
            Session["vitalSignSave"] = formObj.printprocess();

            return RedirectToAction("Waiting_AttendanceListByClinic");
        }

        public ActionResult DailyAppointments_Schedule()
        {
            return View();
        }

        public ActionResult PatientChargesMaintenance()
        {
            vm.DOCTORSS = ErpFunc.RsGet<MR_DATA.DOCTORS>("MR_DATA", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' and STATUS = 'A' order by NAME");
            vm.TARIFFS = ErpFunc.RsGet<MR_DATA.TARIFF>("MR_DATA", "SELECT REFERENCE, NAME FROM TARIFF  ORDER BY NAME");

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name"),
                DiagnosisCodess = ErpFunc.RsGet<SYSCODETABS.DiagnosisCodes>("SYSCODETABS", "SELECT type_code, name FROM DIAGNOSISCODES ORDER BY name")
            };

            vm.GRPPROCEDURES = ErpFunc.RsGet<MR_DATA.GRPPROCEDURE>("MR_DATA", "select REFERENCE, NAME from grpprocedure order by name");

            return View(vm);
        }


        
        public ActionResult PatientPaymentMaintenance()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.REPORT_TYPE1 = ""; //for service type
            vm.REPORTS.msection = "2";

            vm.ATMPROFILES = ErpFunc.RsGet<MR_DATA.ATMPROFILE>("MR_DATA", "SELECT NAME FROM ATMPROFILE order by NAME");

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            return View(vm);
        }


        public ActionResult ReceiptGenerator()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if(Session["ReceiptGenerator"] != null)
            {
                vm.REPORTS = (MR_DATA.REPORTS)Session["ReceiptGenerator"];
                Session["ReceiptGenerator"] = null;
            }

            //vm.REPORTS.txtreference = (string)Session["mreference"]; //for mreference
            //vm.REPORTS.chkIncludeBf = false; //for isinvoice
            //vm.REPORTS.SessionDs = Request.Cookies["mrName"].Value; //for woperator
            //vm.REPORTS.chkgetdependants = (bool)Session["disallowbackdate"];

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select pauto, activeglint, glbatchno, gldocument, glcompany, cashpoint, attendlink, dischtime, name, DTFORMAT from mrcontrol order by recid", false);
            vm.REPORTS.chkgetdependants = (bool)dt.Rows[2]["cashpoint"];

            if(vm.REPORTS.alertMessage == null)
                vm.REPORTS.alertMessage = ""; //This is to initialize value field in the view sent from the ReportViewer Action

            return View(vm);
        }

        public ActionResult ReportViewer(MR_DATA.MR_DATAvm VM1, string formTitle)
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if(formTitle == "ReceiptGenerator"){
                vm = VM1;
                string woperator = Request.Cookies["mrName"].Value;

                ReceiptsGenerator formObj = new ReceiptsGenerator(woperator, vm);
                vm.REPORTS = formObj.printprocess();

                Session["ReceiptGenerator"] = vm.REPORTS;
            } else if (formTitle == "Waiting_AttendanceListByClinic") {
                vm = VM1;
                string woperator = Request.Cookies["mrName"].Value;

                frmAttendanceList formObj = new frmAttendanceList(vm);
                vm.REPORTS = formObj.printprocess();

                Session["Waiting_AttendanceListByClinic"] = vm.REPORTS;

            } else if (formTitle == "SampleLabelGenerator") {
                vm = VM1;
                string woperator = Request.Cookies["mrName"].Value;

                SampleLabelGenerator formObj = new SampleLabelGenerator(woperator, vm);
                vm.REPORTS = formObj.printprocess();

                Session["SampleLabelGenerator"] = vm.REPORTS;

            } else if (formTitle == "MaterialCostDefinition") {
                vm = VM1;
                string woperator = Request.Cookies["mrName"].Value;

                frmInvMaterialCostDefinition formObj = new frmInvMaterialCostDefinition(vm, woperator);
                vm.REPORTS = formObj.btnPrint_Click();

                Session["MaterialCostDefinition"] = vm.REPORTS;

            } 


            return RedirectToAction(formTitle);
        }

        public ActionResult AdmissionDeposit_PaymentRecord()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.REPORT_TYPE1 = "Admissions"; //for service type
            vm.REPORTS.msection = "2";

            vm.ATMPROFILES = ErpFunc.RsGet<MR_DATA.ATMPROFILE>("MR_DATA", "SELECT NAME FROM ATMPROFILE order by NAME");

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            return View(vm);
        }

        public ActionResult OtherServicesIncome_Receivables()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.REPORT_TYPE1 = "OtherServiceIncome"; //for service type
            vm.REPORTS.msection = "2";

            vm.ATMPROFILES = ErpFunc.RsGet<MR_DATA.ATMPROFILE>("MR_DATA", "SELECT NAME FROM ATMPROFILE order by NAME");

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name")
            };

            return View(vm);
        }

        public ActionResult Move_ProcessHMO_NHISCapitatedBills()
        {
            return View();
        }

        public ActionResult BillAdjustmentEntry()
        {
            return View();
        }

        public ActionResult MedStaffInfoMaintenance()
        {
            return View();
        }

        public ActionResult DiagnosisCodeDefinition()
        {
            return View();
        }

        public ActionResult ServCentreCodeDefinition()
        {
            return View();
        }

        public ActionResult CostCentreCodeDefinition()
        {
            return View();
        }

        public ActionResult PatientAdmissionDischargeReasonCode()
        {
            return View();
        }

        public ActionResult Bill_PaymentAdjustCodeDef()
        {
            return View();
        }

        public ActionResult CustomerBillingClassDetails()
        {
            return View();
        }

        public ActionResult DC_ATMBankTellerConfigForCashier()
        {
            return View();
        }

        public ActionResult DoctorsConsultingRoomDef()
        {
            return View();
        }

        public ActionResult Tariff_ChargeMasterMaintenance()
        {
            return View();
        }

        public ActionResult Stock_DrugChargesMaintenance()
        {
            return View();
        }

        public ActionResult RoutineDrugsDefinition()
        {
            return View();
        }

        public ActionResult SpecialConsultationChargeProfile()
        {
            return View();
        }

        public ActionResult Grpd_MultipleProcedureDef()
        {
            return View();
        }

        public ActionResult CorpClients_RefDocDetails()
        {
            return View();
        }

        public ActionResult HMODetails()
        {
            return View();
        }

        public ActionResult TagHMO_NHISBillsCapitation_Claims()
        {
            return View();
        }

        public ActionResult AdmissionsManagement()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", 
                    "SELECT type_code, name from servicecentrecodes order by name"),

                DiagnosisCodess = ErpFunc.RsGet<SYSCODETABS.DiagnosisCodes>("SYSCODETABS",
                    "SELECT type_code, name FROM DIAGNOSISCODES order by name"),

                BranchCodess = ErpFunc.RsGet<SYSCODETABS.BranchCodes>("SYSCODETABS",
                    "SELECT type_code, name FROM branchcodes order by name"),

                mrPatDischReasonss = ErpFunc.RsGet<SYSCODETABS.mrPatDischReasons>("SYSCODETABS",
                    "SELECT TYPE_CODE, NAME FROM mrpatdischreasons order by name"),

                CostCentreCodess = ErpFunc.RsGet<SYSCODETABS.CostCentreCodes>("SYSCODETABS",
                    "SELECT type_code, name FROM COSTCENTRECODES ORDER BY name"),
            };

            vm.SCS01vm = new SCS01.SCS01vm
            {
                STOREs = ErpFunc.RsGet<SCS01.STORE>("SCS01", "select storecode, name from store order by name"),
                stocks = ErpFunc.RsGet<SCS01.stock>("SCS01", "SELECT DISTINCT NAME, ITEM FROM STOCK order by name"),
            };

            vm.DOCTORSS = ErpFunc.RsGet<MR_DATA.DOCTORS>("MR_DATA",
                "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' and STATUS = 'A' order by NAME");

            vm.TARIFFS = ErpFunc.RsGet<MR_DATA.TARIFF>("MR_DATA",
                "SELECT REFERENCE, NAME FROM TARIFF order by name");

            vm.DISPSERVS = ErpFunc.RsGet<MR_DATA.DISPSERV>("MR_DATA",
                "SELECT REFERENCE, DESCRIPTION FROM dispserv order by description");

            vm.TEMPLATEGRPS = ErpFunc.RsGet<MR_DATA.TEMPLATEGRP>("MR_DATA", "select * from TEMPLATEGRP");

            vm.CUSTOMERS = ErpFunc.RsGet<MR_DATA.CUSTOMER>("MR_DATA", 
                "SELECT CUSTNO, NAME FROM CUSTOMER WHERE referrer = '1' order by NAME");

            vm.GRPPROCEDURES = ErpFunc.RsGet<MR_DATA.GRPPROCEDURE>("MR_DATA", 
                "select REFERENCE, NAME from grpprocedure order by name");

            vm.ROUTDRGSS = ErpFunc.RsGet<MR_DATA.ROUTDRGS>("MR_DATA",
                "select DISTINCT reference FROM routdrgs ORDER BY reference");

            vm.REPORTS.REPORT_TYPE1 = Request.Cookies["mrName"].Value;
            vm.REPORTS.msection = "A";

            return View(vm);
        }

        public ActionResult AnteNatalRecords()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.REPORTS.REPORT_TYPE1 = Request.Cookies["mrName"].Value; //operator

            DataTable udt = Dataaccess.GetAnytable("", "MR", 
                "select * from mrstlev where operator = '" + vm.REPORTS.REPORT_TYPE1 + "'", false);

            if (udt.Rows.Count < 1 || string.IsNullOrWhiteSpace(udt.Rows[0]["section"].ToString()))
            {
                vm.REPORTS.ActRslt = "Your profile to this Application has not been properly defined..." +
                    "Pls See Your Systems Administrator!";

                vm.REPORTS.msection = "";
            } else {
                vm.REPORTS.msection = msmrfunc.mrGlobals.msection = udt.Rows[0]["section"].ToString().Substring(0, 1);
            }

            string xdoc = "";
            if (new string[] { "1", "4", "3", "9" }.Contains(vm.REPORTS.msection)) //msection.Contains("429"))  //doctors and nurses
            {
                if (vm.REPORTS.msection == "4")
                {
                    xdoc = bissclass.sysGlobals.anycode;
                    if (string.IsNullOrWhiteSpace(xdoc))
                    {
                        vm.REPORTS.alertMessage = "Access Denied...";
                        //return;
                    }
                }
            }

            vm.REPORTS.doctor = xdoc;

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                DiagnosisCodess = ErpFunc.RsGet<SYSCODETABS.DiagnosisCodes>("SYSCODETABS",
                   "select type_code,name from DesignationCodes order by name"),
            };

            vm.DOCTORSS = ErpFunc.RsGet<MR_DATA.DOCTORS>("MR_DATA",
                "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' and STATUS = 'A' order by NAME");

            return View(vm);
        }

        public ActionResult PaediatricsClinic()
        {
            return View();
        }

        public ActionResult PaediatricsClinicMgt()
        {
            return View();
        }

        public ActionResult PaediatricsMilestones()
        {
            return View();
        }

        public ActionResult MediaReportWriter()
        {
            return View();
        }

        public ActionResult SampleCollectDetails()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (Session["SampleCollectDetails"] != null) {
                vm.REPORTS = (MR_DATA.REPORTS)Session["SampleCollectDetails"];
                Session["SampleCollectDetails"] = null;
            } else {
                vm.REPORTS.alertMessage = "";
            }

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", 
                    "SELECT type_code, name from servicecentrecodes order by name")
            };

            vm.REPORTS.msection = "6";

            return View(vm);
        }

        public ActionResult SampleCollectDetailsSubmit(MR_DATA.MR_DATAvm VM1)
        {
            vm = VM1;
            string woperato = Request.Cookies["mrName"].Value;

            samplecollectiondetails formObj = new samplecollectiondetails(vm, woperato);
            Session["SampleCollectDetails"] = formObj.btnsave_Click();

            return RedirectToAction("SampleCollectDetails");
        }
        
        public ActionResult SampleLabelGenerator()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (Session["SampleLabelGenerator"] != null)
            {
                vm.REPORTS = (MR_DATA.REPORTS)Session["SampleLabelGenerator"];
                Session["SampleLabelGenerator"] = null;
            }

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", 
                    "SELECT type_code, name from servicecentrecodes order by name")
            };

            return View(vm);
        }

        public ActionResult InvestigationsMgtReports()
        {
            return View();
        }

        public ActionResult ImageAcquisitions()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name"),
            };

            vm.MEDHPICPROPS = ErpFunc.RsGet<MR_DATA.MEDHPIC>("MR_DATA",
                "SELECT medhpic.groupcode+':'+medhpic.patientno+':'+convert(char,medhpic.trans_date) AS facility1, medhpic.groupcode,medhpic.patientno,medhpic.trans_date,billchain.name from medhpic LEFT JOIN BILLCHAIN on medhpic.groupcode = billchain.groupcode and medhpic.patientno = billchain.patientno order by name");

            return View(vm);
        }


        public ActionResult MaterialCostDefinition()
        {
            vm.REPORTS = new MR_DATA.REPORTS();

            if (Session["MaterialCostDefinition"] != null)
            {
                vm.REPORTS = (MR_DATA.REPORTS)Session["MaterialCostDefinition"];
                Session["MaterialCostDefinition"] = null;
            }

            vm.SYSCODETABSvm = new SYSCODETABS.SYSCODETABSvm
            {
                ServiceCentreCodess = ErpFunc.RsGet<SYSCODETABS.ServiceCentreCodes>("SYSCODETABS", "SELECT type_code, name from servicecentrecodes order by name"),
            };

            vm.SCS01vm = new SCS01.SCS01vm
            {
                STOREs = ErpFunc.RsGet<SCS01.STORE>("SCS01", "SELECT STORECODE, NAME FROM STORE ORDER BY NAME")
            };

            if(vm.REPORTS.alertMessage == null)
            {
                vm.REPORTS.alertMessage = "";
            }
            
            return View(vm);
        }

        public ActionResult PrescriptionsMgt()
        {
            return View();
        }

        public ActionResult PharmacyStkMgt()
        {
            return View();
        }

        public ActionResult PendingPrescrDetails()
        {
            return View();
        }

        public ActionResult WoundDressingDetails()
        {
            return View();
        }

        public ActionResult Deaths_MorbidAnatomy()
        {
            return View();
        }

        public ActionResult MedicalStaffDutyRoaster()
        {
            return View();
        }

        public ActionResult NursingNotes()
        {
            return View();
        }

        public ActionResult ChangeStatusPvt_Corp()
        {
            return View();
        }

        public ActionResult SyncPatientsName()
        {
            return View();
        }

        public ActionResult ChangeGroupHead()
        {
            return View();
        }






    }
}
