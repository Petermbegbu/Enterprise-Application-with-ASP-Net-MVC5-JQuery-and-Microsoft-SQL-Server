#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ANC.Forms;
using paediatrics.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmClinicalAdminReports : Form
    {
        int optn;
        string woperator;
        public frmClinicalAdminReports(int rptoption, string xoperator)
        {
            InitializeComponent();
            optn = rptoption;
            woperator = xoperator;
        }
        private void frmClinicalAdminReports_Load(object sender, EventArgs e)
        {
            if (optn == 1)
                btnAdmSpace.PerformClick();
            else if (optn == 2)
                btnAdmRecords.PerformClick();
            else if (optn == 3)
                btnDischargeRecords.PerformClick();
            /*else if (optn == 4) //bed occupanty
                btnAdmRecords.PerformClick();*/
            else if (optn == 5)
                btnBirthListing.PerformClick();
            else if (optn == 6)
                btnDeathList.PerformClick();
            else if (optn == 7)
                btnPharmacyReports.PerformClick();
            else if (optn == 8)
                btnMSDutyRoaster.PerformClick();
            else if (optn == 9)
                btnClinicalStatistics.PerformClick();
            else if (optn == 10)
                btnAttenMonitor.PerformClick();
            else if (optn == 11)
                btnANCReg.PerformClick();
            else if (optn == 12)
                btnPaediatrics.PerformClick();
            else if (optn == 13)
                btnOverwriteprofile.PerformClick();
            else if (optn == 14)
                btnDocsLoginProfile.PerformClick();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            frmAdmspace admspace = new frmAdmspace();
            admspace.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAdmissionRecords admrecs = new frmAdmissionRecords(1);
            admrecs.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmAdmissionRecords admrecs = new frmAdmissionRecords(2);
            admrecs.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmBirthListing birthlist = new frmBirthListing();
            birthlist.Show();
        }

        private void btnDeathList_Click(object sender, EventArgs e)
        {
            frmDeathRecords deathrecs = new frmDeathRecords();
            deathrecs.Show();
        }

        private void btnPharmacyReports_Click(object sender, EventArgs e)
        {
            frmDispensaryRpt dispenaryrpt = new frmDispensaryRpt();
            dispenaryrpt.Show();
        }

        private void btnMSDutyRoaster_Click(object sender, EventArgs e)
        {
            frmMSDutyRoaster dutyroaster = new frmMSDutyRoaster(woperator );
            dutyroaster.Show();
        }

        private void btnClinicalStatistics_Click(object sender, EventArgs e)
        {
            ClinicalStatistics clinicals = new ClinicalStatistics();
            clinicals.Show();
        }

        private void btnANCReg_Click(object sender, EventArgs e)
        {
            rptFormsANC ancrpts = new rptFormsANC("", "", "","");
            ancrpts.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            rptFormPaediatrics paedrpts = new rptFormPaediatrics("", "", "");
            paedrpts.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAttenMonitor_Click(object sender, EventArgs e)
        {
            frmAttendanceMonitor attendmonitor = new frmAttendanceMonitor();
            attendmonitor.Show();
        }

        private void btnOverwriteprofile_Click(object sender, EventArgs e)
        {
            frmOverwriteProfiles overrights = new frmOverwriteProfiles();
            overrights.Show();
        }

 
    }
}