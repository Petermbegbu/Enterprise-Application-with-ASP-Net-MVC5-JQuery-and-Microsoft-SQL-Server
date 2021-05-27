#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WebForms;

using OtherClasses.Models;
using OtherClasses;

using System.IO;
using System.Drawing.Printing;
//using Spire.Pdf;
//using Spire.License;

using msfunc;
using mradmin.BissClass;

using System.Security;
using System.Security.Permissions;
using System.Management;


#endregion

namespace MSMR.Forms
{
    public partial class frmReportViewer
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        string rptheader, username = string.IsNullOrWhiteSpace(msfunc.bissclass.sysGlobals.user_name) ? "ADI SYSTEMS LIMITED" :
            msfunc.bissclass.sysGlobals.user_name, rptfooter,rptcriteria, slogan, woperator = "", logo,rpttype, mcurrency,
            currencyname, programid, mprog, mreference;
        string[] addressa_ = new string[7], mischargetxta_ = new string[6], toaddressa_ = new string[3], currencya_ = new string[2];
        decimal invamount;
        DateTime datefrom, dateto, dtmin_date = msmrfunc.mrGlobals.mta_start;
         DataSet ds = new DataSet();
        bool toprint,isdataset;


        MR_DATA.REPORTS fDta;

        public frmReportViewer(string xcaption,string xrptheader,string xrptfooter, string xrptcriteria, string xcompanycode,
            string rpttype_INV_DN_CN_POS, string invref, decimal xinvamount,string xcurrency,string xlocalcur,string xcurname,
            DataSet xds, bool xdataset,int xprogramid,DateTime xdatefrom,DateTime xdateto,string xprog,bool xtoprint,
            string rptsize, string xoperator, MR_DATA.REPORTS fDtaa)
        {
            fDta = fDtaa;

            rptheader = xrptheader; rptfooter = xrptfooter == null ? bissclass.getRptfooter() : xrptfooter; rptcriteria = xrptcriteria;
            invamount = xinvamount;
            rpttype = rpttype_INV_DN_CN_POS; mcurrency = xcurrency == "" ? xlocalcur : mcurrency; currencyname = xcurname;
            programid = xprogramid.ToString();
            toprint = xtoprint; isdataset = xdataset; mreference = invref;
            datefrom = xdatefrom; dateto = xdateto; mprog = xprog; woperator = xoperator;
            m_currentPageIndex = 0;
            //if (rptsize == "W") //wide report
            //    this.reportViewer1.Size = new System.Drawing.Size(1003, 531);
            //else
            //    this.reportViewer1.Location = new System.Drawing.Point(103, 0); //center reportviewer
            if (xds != null)
                ds = xds;
           // rdlcfile = xrdlcfile; sql = xsql;
            if (xrptcriteria != "")
                rptcriteria = "Report Criteria : " + rptcriteria;
            if (rpttype == "INV" || rpttype == "DN" || rpttype == "CN" || rpttype == "STATMT" || rpttype == "POS" ||
                rpttype == "RECEIPTSTD" || rpttype == "INVRESULT" || rpttype == "DN" || rpttype == "BILLREF" ||
                rpttype == "ADMISSIONS" || rpttype == "EOMSUMMARY" || rpttype == "EOMSTATMTWBILL" ||
                rpttype == "MONTHLYBILLSUMM" || rpttype == "MRPT" || rpttype == "FFSERVICECLAIMS")
                getInvoiceHeader(xcompanycode == "" ? "01" : xcompanycode, invref);
            currencya_[0] = currencya_[1] = "";
            mcurrency = mcurrency == "" ? bissclass.sysGlobals.mlocalcur : mcurrency;
            if (mcurrency != "")
            {
                DataTable dtcur = Dataaccess.GetAnytable("CURTABLE", "CODES", "select curmain,curunit from curtable where ccode = '" +
                    mcurrency + "'", false);
                if (dtcur.Rows.Count > 0)
                {
                    currencya_[0] = dtcur.Rows[0]["curmain"].ToString().Trim();
                    currencya_[1] = dtcur.Rows[0]["curunit"].ToString().Trim();
                }
            }
        }

        public MR_DATA.REPORTS Show(string rdlc, string sql, bool print=false)
        {
            toprint = print;

            if (ds == null || rpttype == "INV" || rpttype == "UNVINV" || rpttype == "PMT")
            {
                ds = Dataaccess.GetDataToDataset("", "MR", sql, false);
            }
            ReportParameter[] param = new ReportParameter[11];

            if (rpttype == "POS")
            {
                param = new ReportParameter[6];
                param[0] = new ReportParameter("company", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("addressa_", addressa_);
                param[3] = new ReportParameter("pos_payreference", mreference);
                param[4] = new ReportParameter("woperator", woperator);
                param[5] = new ReportParameter("POSAddendum", msmrfunc.mrGlobals.posAddendum);
                //param[6] = new ReportParameter("clogo", "file:\\" + @logo);
            }
            else if (rpttype == "RECEIPTSTD")
            {
                param = new ReportParameter[6];
                param[0] = new ReportParameter("company", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("addressa_", addressa_);
                param[3] = new ReportParameter("clogo", "file:\\" + @logo);
                param[4] = new ReportParameter("woperator", woperator);
                param[5] = new ReportParameter("currencya_", currencya_);
            }
            else if (rpttype == "PRESCRIPTIONSLIP")
            {
                param = new ReportParameter[2];
                param[0] = new ReportParameter("company", username);
                param[1] = new ReportParameter("woperator", woperator);
            }
            else if (rpttype == "SAMPLELABLE")
            {
                param = new ReportParameter[3];
                param[0] = new ReportParameter("company", username);
                param[1] = new ReportParameter("woperator", woperator);
                param[2] = new ReportParameter("inv_dtl", fDta.SessionInv_dtl);
            }
            else if (rpttype == "INVRESULT")
            {
                //15.06.2019 : toprint - represent ON LETTER HEAD  - REFERENCE - Patient Photo
                if (rdlc == "InvestigationresultPIC.rdlc")
                    param = new ReportParameter[11];
                else
                    param = new ReportParameter[10];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("addressa_", addressa_);
                param[3] = new ReportParameter("woperator", woperator);
                param[4] = new ReportParameter("rptheader", rptheader);
                param[5] = new ReportParameter("mcurrency", "");
                param[6] = new ReportParameter("clogo", toprint && mreference != "" ? "file:\\" + @mreference : "file:\\" + @logo);
                param[7] = new ReportParameter("mhead", fDta.SessionMhead);
                param[8] = new ReportParameter("headerleftjustify", fDta.SessionHeaderleftjustify);
                param[9] = new ReportParameter("invfooter", fDta.SessionInvfooter);
                if (rdlc == "InvestigationresultPIC.rdlc")
                    param[10] = new ReportParameter("patientpic", "file:\\" + @mreference);
            }
            else if (rpttype == "BALANCEDUE" || rpttype == "OPDATTEND" || rpttype == "TARIFFLIST" || rpttype == "ADMSPACE" ||
                rpttype == "ADMRECS" || rpttype == "ANNUALSUMMARY" || rpttype == "STAFF" || rpttype == "ATTENDMONITOR" ||
                rpttype == "BIRTHLIST" || rpttype == "PATIENTDETAILS" || rpttype == "SURGERYDETL" || rpttype == "ENCOUNTER" ||
                rpttype == "PAED_ADMS" || rpttype == "RecordList" || rpttype == "PHONENUMBERS")
            {
                param = new ReportParameter[3];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("rptfooter", rptfooter);
            }
            else if (rpttype.Substring(0, 4) == "PAED")
            {
                param = new ReportParameter[4];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("rptfooter", rptfooter);
                param[3] = new ReportParameter("minDate", dtmin_date.ToShortDateString());
            }
            else if (rpttype == "ATTENDANCE" || rpttype == "WAITINGLIST")
            {
                param = new ReportParameter[4];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("rptfooter", rptfooter);
                param[3] = new ReportParameter("waitonly", fDta.SessionWaitonly);
            }
            else if (rpttype.Substring(0, 3) == "ANC")
            {
                if (rpttype == "ANCACTIVEREG")
                {
                    param = new ReportParameter[5];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter);
                    param[3] = new ReportParameter("recordtype", programid == "2" ? "A" : "N");
                    param[4] = new ReportParameter("startdate", dtmin_date.ToShortDateString());
                }
                else
                {
                    param = new ReportParameter[4];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter);
                    param[3] = new ReportParameter("startdate", dtmin_date.ToShortDateString());
                }
            }
            else if (rpttype == "PMT" || rpttype == "PMTBANKCOLUMN")
            {
                param = new ReportParameter[6];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("currencyname", currencyname);
                param[2] = new ReportParameter("rptheader", rptheader);
                param[3] = new ReportParameter("rptfooter", rptfooter);
                param[4] = new ReportParameter("mcurrency", mcurrency);
                param[5] = new ReportParameter("mprog", mprog);
            }
            else if (rpttype == "MEDHIST" || rpttype == "ADJUSTMENTS" || rpttype == "CLSTATISTICS" || rpttype == "SUMMARYOFACCTS" ||
                rpttype == "CORPCLIENTS" || rpttype == "CHAINACCT" || rdlc == "ComparativeRpt.rdlc" || rpttype == "INVDETAILS" ||
                rpttype == "DISPENSARYRPT" || rpttype == "AGEDACCTS")
            {
                param = new ReportParameter[4];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("rptfooter", rptfooter);
                param[3] = new ReportParameter("rptcriteria", rptcriteria);
            }
            else if (rpttype == "REVENUEFRMFACILITIES")
            {
                param = new ReportParameter[5];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("rptfooter", rptfooter);
                param[3] = new ReportParameter("rptcriteria", rptcriteria);
                param[4] = new ReportParameter("payments", fDta.SessionPayments);

            }
            else if (rpttype == "DN")
            {
                param = new ReportParameter[8];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("rptheader", rptheader);
                param[3] = new ReportParameter("clogo", "file:\\" + @logo);
                param[4] = new ReportParameter("addressa_", addressa_);
                param[5] = new ReportParameter("toaddressa_", toaddressa_);
                param[6] = new ReportParameter("woperator", woperator);
                param[7] = new ReportParameter("currencyname", "");
            }
            else if (rpttype == "STATMT")
            {
                param = new ReportParameter[11];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("rptheader", rptheader);
                param[3] = new ReportParameter("rptfooter", rptfooter);
                param[4] = new ReportParameter("currencyname", currencyname);
                param[5] = new ReportParameter("invref", mreference);
                param[6] = new ReportParameter("clogo", "file:\\" + @logo);
                param[7] = new ReportParameter("addressa_", addressa_);
                param[8] = new ReportParameter("Toaddressa_", toaddressa_);
                param[9] = new ReportParameter("balbf", fDta.SessionBalbf.ToString());
                param[10] = new ReportParameter("includebf", fDta.SessionIncludebf);
            }
            else if (rpttype == "BILLREF")
            {
                param = new ReportParameter[9];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("rptheader", rptheader);
                param[3] = new ReportParameter("rptfooter", rptfooter);
                param[4] = new ReportParameter("currencyname", currencyname);
                param[5] = new ReportParameter("invref", "");
                param[6] = new ReportParameter("clogo", "file:\\" + @logo);
                param[7] = new ReportParameter("addressa_", addressa_);
                param[8] = new ReportParameter("Toaddressa_", toaddressa_);
            }
            else if (rpttype == "BILLCHRONOLOGICAL" || rpttype == "BILLS_ALL")
            {
                param = new ReportParameter[5];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("currencyname", currencyname);
                param[3] = new ReportParameter("rptfooter", rptfooter);
                param[4] = new ReportParameter("includebf", fDta.SessionIncludebf);
            }
            else if (rpttype == "ADMISSIONS")
            {
                if (rdlc == "AdmSummaryRpt.rdlc")
                {
                    param = new ReportParameter[4];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter);
                    param[3] = new ReportParameter("rptcriteria", rptcriteria);
                }
                else
                {
                    param = new ReportParameter[5];
                    param[0] = new ReportParameter("username", username);
                    param[1] = new ReportParameter("rptheader", rptheader);
                    param[2] = new ReportParameter("rptfooter", rptfooter);

                    if (rdlc == "AdmDailyRpt.rdlc")
                        param[3] = new ReportParameter("datea_", (string[])fDta.SessionDatea_);
                    else
                        param[3] = new ReportParameter("clogo", "file:\\" + @logo);
                    param[4] = new ReportParameter("currencya_", currencya_);
                }

            }
            else if (rpttype == "EOMSUMMARY")
            {
                param = new ReportParameter[12];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("currencyname", currencyname);
                param[3] = new ReportParameter("isbf", fDta.SessionIsbf);
                param[4] = new ReportParameter("ispayment", fDta.SessionIspayment);
                param[5] = new ReportParameter("isadjust", fDta.SessionIsadjust);
                param[6] = new ReportParameter("iscur_bal", fDta.SessionIscur_bal);
                param[7] = new ReportParameter("isbalance", fDta.SessionIsbalance);
                param[8] = new ReportParameter("mdate", fDta.SessionMdate);
                param[9] = new ReportParameter("clogo", "file:\\" + @logo);
                param[10] = new ReportParameter("addressa_", addressa_);
                param[11] = new ReportParameter("currencya_", currencya_);

            }
            else if (rpttype == "EOMSTATMTWBILL")
            {
                param = new ReportParameter[7];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("currencyname", currencyname);
                param[3] = new ReportParameter("mdate", fDta.SessionMdate);
                param[4] = new ReportParameter("clogo", "file:\\" + @logo);
                param[5] = new ReportParameter("addressa_", addressa_);
                param[6] = new ReportParameter("currencya_", currencya_);
            }
            else if (rpttype == "MRPT")
            {
                param = new ReportParameter[4];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("clogo", "file:\\" + @logo);
                param[3] = new ReportParameter("addressa_", addressa_);
            }
            else if (rpttype == "MONTHLYBILLSUMM")
            {
                param = new ReportParameter[6];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("currencyname", currencyname);
                param[3] = new ReportParameter("rptheader", rptheader);
                param[4] = new ReportParameter("clogo", "file:\\" + @logo);
                param[5] = new ReportParameter("addressa_", addressa_);
            }
            else if (rpttype == "FFSERVICECLAIMS")
            {
                param = new ReportParameter[6];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("rptheader", rptheader);
                param[3] = new ReportParameter("clogo", "file:\\" + @logo);
                param[4] = new ReportParameter("addressa_", addressa_);
                param[5] = new ReportParameter("currencya_", currencya_);
            }
            else if (rpttype == "REFERRERS")
            {
                param = new ReportParameter[3];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("slogan", slogan);
                param[2] = new ReportParameter("rptheader", rptheader);
            }
            else if (programid == "3" || programid == "4")
            {
                param = new ReportParameter[9];
                param[0] = new ReportParameter("rptheader", rptheader);
                param[1] = new ReportParameter("rptfooter", rptfooter);
                param[2] = new ReportParameter("addressa_", (string[])fDta.SessionAddress_);
                param[3] = new ReportParameter("username", username);
            }
            else if (rpttype == "BALDUE" || rpttype == "UNVINV") //CUSTOMER BALANCES DUE
            {
                param = new ReportParameter[6];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("rptfooter", rptfooter);
                param[3] = new ReportParameter("rptcriteria", rptcriteria);
                param[4] = new ReportParameter("datefrom", datefrom.ToShortDateString());
                param[5] = new ReportParameter("mprog", mprog);
            }
            else if (rpttype == "MRSTOCKINFO")
            {
                param = new ReportParameter[8];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("rptfooter", rptfooter);
                param[3] = new ReportParameter("rptcriteria", rptcriteria);
                param[4] = new ReportParameter("AdmReference", mreference);
                param[5] = new ReportParameter("Duration", dateto.Subtract(datefrom).TotalDays.ToString() + " Days");
                param[6] = new ReportParameter("DateRange", datefrom.ToShortDateString() + " TO : " + dateto.ToShortDateString());
                param[7] = new ReportParameter("patname", woperator);
            }

            else if (rpttype == "MSDUTYR")
            {
                param = new ReportParameter[18];
                param[0] = new ReportParameter("username", username);
                param[1] = new ReportParameter("rptheader", rptheader);
                param[2] = new ReportParameter("rptfooter", rptfooter);
                param[3] = new ReportParameter("dda_", (string[])fDta.SessionDd);
                param[4] = new ReportParameter("morninga_", (string[])fDta.SessionMorninga_);
                param[5] = new ReportParameter("afternoona_", (string[])fDta.SessionAfternoona_);
                param[6] = new ReportParameter("nighta_", (string[])fDta.SessionNighta_);
                param[7] = new ReportParameter("oncalla_", (string[])fDta.SessionOncalla_);
                param[8] = new ReportParameter("offa_", (string[])fDta.SessionOffa_);
                param[9] = new ReportParameter("morning", fDta.SessionMorning);
                param[10] = new ReportParameter("afternoon", fDta.SessionAfternoon);
                param[11] = new ReportParameter("night", fDta.SessionNight);
                param[12] = new ReportParameter("oncall", fDta.SessionOncall);
                param[13] = new ReportParameter("off", fDta.SessionOff);
                param[14] = new ReportParameter("dsa_", (string[])fDta.SessionDsa_);
                param[15] = new ReportParameter("bsa_", (string[])fDta.SessionBsa_);
                param[16] = new ReportParameter("ds", fDta.SessionDs);
                param[17] = new ReportParameter("bis", fDta.SessionBis);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", ds.Tables[0]);
            ReportDataSource rds1 = new ReportDataSource();
            if (rpttype == "ADMISSIONS" && rdlc != "AdmSummaryRpt.rdlc" || rpttype == "EOMSTATMTWBILL" ||
                rdlc == "ComparativeRpt.rdlc" || rpttype == "FFSERVICECLAIMS")
            {
                rds1 = new ReportDataSource("DataSet2", ds.Tables[1]);
            }



            ReportViewer rptViewer = new ReportViewer();
            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.LocalReport.EnableExternalImages = true;
            rptViewer.SizeToReportContent = true;

            rptViewer.LocalReport.ReportPath = @"RDLCs/" + rdlc;
            rptViewer.LocalReport.DataSources.Add(rds);

            if (rpttype == "ADMISSIONS" && rdlc != "AdmSummaryRpt.rdlc" || rpttype == "EOMSTATMTWBILL" ||
                rdlc == "ComparativeRpt.rdlc" || rpttype == "FFSERVICECLAIMS")
            {
                rptViewer.LocalReport.DataSources.Add(rds1);
            }

            rptViewer.LocalReport.SetParameters(param);

            return
                new MR_DATA.REPORTS
                {
                    GeneratedReport = rptViewer,
                    PdfPath = toprint ? ErpFunc.SavePDF(rptViewer) : "",
                    PRINT = toprint
                };
        }

        /*
        private void reportViewer1_HostedPageLoad(object sender, Gizmox.WebGUI.Forms.Hosts.AspPageEventArgs e)
        {
            System.Web.HttpContext objHttpContext = System.Web.HttpContext.Current;

            if (objHttpContext != null && objHttpContext.Request != null)
            {

                if (objHttpContext.Request.RequestType == "GET")
                {
                    show(Session["rdlcFile"].ToString(), Session["sql"].ToString());
                }
            }
        }
        */
        
        void getInvoiceHeader(string companycode,string invoice_reference)
        {
            DataTable dtsetup = Dataaccess.GetAnytable("mrsetup", "MR", "select files, setdetail from mrsetup order by recid",
                false); //company logo
            DataTable dtcontrol = Dataaccess.GetAnytable("mrcontrol", "MR", "select appuser, name from mrcontrol order by recid",
                false);
            //for (int i = 0; i < dtsetup.Rows.Count; i++)
            //{
            //    if (dtsetup.Rows[i]["name"].ToString() != "")
            //    {
            addressa_ = new string[5];
                    username = dtcontrol.Rows[0]["appuser"].ToString().Trim();
                    slogan = dtsetup.Rows[1]["files"].ToString().Trim();
                    addressa_[0] = dtcontrol.Rows[1]["appuser"].ToString().Trim();
                    addressa_[1] = dtcontrol.Rows[2]["appuser"].ToString().Trim();
                    addressa_[2] = dtcontrol.Rows[2]["name"].ToString().Trim();
                    addressa_[3] = dtcontrol.Rows[3]["name"].ToString().Trim();
                    addressa_[4] = dtcontrol.Rows[4]["name"].ToString().Trim();
                    //addressa_[2] = dtsetup.Rows[i]["address3"].ToString().Trim();
                    //addressa_[3] = dtsetup.Rows[i]["address4"].ToString().Trim();
                    //addressa_[4] = dtsetup.Rows[i]["address5"].ToString().Trim();
                    //addressa_[5] = dtsetup.Rows[i]["address6"].ToString().Trim();
                    //addressa_[6] = dtsetup.Rows[i]["address7"].ToString().Trim();
                    //clremark1 = dtsetup.Rows[i]["clremarks1"].ToString().Trim();
                    //clremark2 = dtsetup.Rows[i]["clremarks2"].ToString().Trim();
                    //invnotes = dtsetup.Rows[i]["invnotes"].ToString().Trim();

                    logo = dtsetup.Rows[4]["setdetail"].ToString().Trim();

            //        break;
            //    }
            //}

            if (rpttype == "STATMT" || rpttype == "BILLREF" ) //|| rpttype == "MONTHLYBILLSUMM")
            {
                string transtype = fDta.SessionCustomertype;
                string customer = fDta.SessionCustomer;
                string selstring = transtype == "P" ? "select name, grouphtype, grouphead, address1 from patient where patientno = '" +
                    customer + "'" : "select name,address1 from customer where custno = '" + customer + "'";
               // addressa_ = new string[3];
                DataTable dt = Dataaccess.GetAnytable("", "MR", selstring, false); //invoice_reference passed as customer ref from 'statmt'
                string adr = "";
                foreach (DataRow row in dt.Rows)
                {
                    if (transtype == "P" && row["grouphtype"].ToString() == "C")
                    {
                        adr = msmrfunc.GETGroupheadname("", row["grouphead"].ToString(), "C");
                    }
                    toaddressa_[0] = adr == "" ? row["name"].ToString().Trim() : adr;
                    toaddressa_[1] = adr == "" ? row["address1"].ToString().Trim() : "";
                    toaddressa_[2] = "";
                }
            }
            for (int i = 0; i < 6; i++)
            {
                if (mischargetxta_[i] == null)
                    mischargetxta_[i] = "";
                if ((rpttype == "STATMT" || rpttype == "BILLREF") && i < 3 && toaddressa_[i] == null )
                    toaddressa_[i] = "";
            }
        }
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);

            return stream;
        }

        /*
        private void Export()
        {
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>PDF</OutputFormat>" +
              "  <PageWidth>8.5in</PageWidth>" +
              "  <PageHeight>11in</PageHeight>" +
              "  <MarginTop>0.25in</MarginTop>" +
              "  <MarginLeft>0.25in</MarginLeft>" +
              "  <MarginRight>0.25in</MarginRight>" +
              "  <MarginBottom>0.25in</MarginBottom>" +
              "</DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            reportViewer1.LocalReport.Render("PDF", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        public void SpPrint()
        {
            PdfDocument doc = new PdfDocument(m_streams[m_currentPageIndex]);

            System.Windows.Forms.PrintDialog pd = new System.Windows.Forms.PrintDialog();

            pd.PrinterSettings.MinimumPage = 1;
            pd.PrinterSettings.MaximumPage = doc.Pages.Count;
            pd.PrinterSettings.ToPage = doc.Pages.Count;

            DialogResult result = (DialogResult)pd.ShowDialog();

            if (result == Gizmox.WebGUI.Forms.DialogResult.OK)
            {
                doc.PrintFromPage = pd.PrinterSettings.FromPage;

                doc.PrintToPage = pd.PrinterSettings.ToPage;

                doc.PrinterName = pd.PrinterSettings.PrinterName;

                System.Drawing.Printing.PrintDocument pdoc = doc.PrintDocument;
                pd.Document = pdoc;
                pdoc.Print();
            }
        }
        public void DirectPrint()
        {
            PdfDocument doc = new PdfDocument(m_streams[m_currentPageIndex]);
            System.Drawing.Printing.PrintDocument pdoc = doc.PrintDocument;
            pdoc.Print();
        }
        public void work()
        {
            show(Session["rdlcFile"].ToString(), Session["sql"].ToString());
        }
        private void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
        */

    }
}
