using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medical_Records.Controllers
{
    public class DoctorsConsultController : Controller
    {
        // GET: DoctorsConsult
        public ActionResult MedicalHistoryConsultDetails()
        {
            return View();
        }
        public ActionResult ChainedMedHistory()
        {
            return View();
        }
        public ActionResult InvProceduresRequest()
        {
            return View();
        }
        public ActionResult MiscInvRpt()
        {
            return View();
        }
        public ActionResult PatGroupCodeLookUp()
        {
            return View();
        }
        public ActionResult PatNumberLookUp()
        {
            return View();
        }
        public ActionResult Referal()
        {
            return View();
        }
        public ActionResult ServiceCentre()
        {
            return View();
        }
    }
}