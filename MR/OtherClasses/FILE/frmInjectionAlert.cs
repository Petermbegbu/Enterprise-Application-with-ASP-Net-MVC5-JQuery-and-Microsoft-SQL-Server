#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using msfunc;
using msfunc.Forms;

using mradmin.BissClass;
using mradmin.DataAccess;
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmInjectionAlert
    {
        MR_DATA.MR_DATAvm vm;

        string mgroupcode, mpatientno, mname, mreference, woperator;
        bool frmfrontdesk;

        public frmInjectionAlert(MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;
            mgroupcode = vm.REPORTS.txtgroupcode;
            mpatientno = vm.REPORTS.txtpatientno;
            mname = vm.REPORTS.TXTPATIENTNAME;
            mreference = vm.REPORTS.txtreference;
            woperator = vm.REPORTS.txtemployer;

            vm.REPORTS = new MR_DATA.REPORTS();

            //InitializeComponent();
            //txtNotes.Text = notes;
            //mgroupcode = groupcode;
            //mpatientno = patientno;
            //mname = name;
            //mreference = reference;
            //woperator = xoperator;

            string xcaption = "";

            if (xcaption != "")
            {
                //this.Text = xcaption;
                frmfrontdesk = true;
            }
        }

        public MR_DATA.MR_DATAvm btnSend_Click()
        {
            //DialogResult result;
            //if (!chkDressing.Checked && !chkInjection.Checked)
            //{
            //    result = MessageBox.Show("Destination must be selected for your Alert !");
            //    return;
            //}

            //result = MessageBox.Show("Confirm to Send Alert Notice...", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            //if (result == DialogResult.No)
            //    return;

            //write to med history
            string xstr = "";
            string alertto = vm.REPORTS.chkADVCorporate ? " Pharmacy/Dispensary " : " Treatment Room "; //chkPharmacy.Checked
            alertto += "\r\n";
            xstr = vm.REPORTS.chkbillregistration ? "INJECTIONS" : "WOUND DRESSING"; //chkInjection.Checked
            alertto += "FOR > "+xstr;
            string xx = "*** NursesDesk ALERT to :"+alertto+"\r\n @ "+DateTime.Now.ToShortTimeString();

            string xcomment = "";
            MedHist medhist = MedHist.GetMEDHIST(mgroupcode, mpatientno, mreference, false, true, DateTime.Now.Date, "DESC");
            bool newmedhist = true;

            if (medhist != null)
            {
                newmedhist = false;
                xcomment = medhist.COMMENTS.Trim() + "\r\n";
            }
            if (xcomment != "")
            {
                xcomment += string.Concat(Enumerable.Repeat("-", 144));
                xcomment += "\r\n";
            }

            xcomment += alertto;
            MedHist.updatemedhistcomments(mgroupcode, mpatientno, DateTime.Now.Date, xcomment, newmedhist, mreference,mname,"","","");
            if (vm.REPORTS.chkBroughtForward) //chkOPDNurses.Checked
            {
                MRB21.Writemrb21Details(mgroupcode, mpatientno, DateTime.Now.Date, mname, newmedhist ? "" : medhist.CLINIC, frmfrontdesk ? "FRONT DESK" : "OPD NURSES", vm.REPORTS.edtspinstructions.Trim(), mreference, frmfrontdesk ? "1" : "3", "3", woperator, "", "O"); //all nurses
                MRB21.Writemrb21Details(mgroupcode, mpatientno, DateTime.Now.Date, mname, newmedhist ? "" : medhist.CLINIC, frmfrontdesk ? "FRONT DESK" : "OPD NURSES", vm.REPORTS.edtspinstructions.Trim(), mreference, frmfrontdesk ? "1" : "3", "A", woperator, "", "O"); // all ward nurses
            }
            if (vm.REPORTS.chkADVCorporate)
                MRB21.Writemrb21Details(mgroupcode, mpatientno, DateTime.Now.Date, mname, newmedhist ? "" : medhist.CLINIC, frmfrontdesk ? "FRONT DESK" : "OPD NURSES", vm.REPORTS.edtspinstructions.Trim(), mreference, frmfrontdesk ? "1" : "3", "8", woperator, "", "O");


            //btnCancel.PerformClick();
            vm.REPORTS.chkDomantAccts = true;
            return vm;
        }

        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}