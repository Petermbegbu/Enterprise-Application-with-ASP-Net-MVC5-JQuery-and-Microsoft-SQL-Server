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

using mradmin.Forms;
using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class SampleLabelGenerator
    {
        bool isdataset;
        string lookupsource, AnyCode, mrptheader, sysmodule = bissclass.getRptfooter(), woperator;
        DateTime datefrom, dateto;
        DataSet ds = new DataSet();
        MR_DATA.MR_DATAvm vm;

        public SampleLabelGenerator(string woperato, MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;

            //InitializeComponent();
            //txtReference.Text = ServiceReference;
            woperator = woperato;
            isdataset = vm.REPORTS.chkAuditProfile;
            datefrom = Convert.ToDateTime(vm.REPORTS.REPORT_TYPE5);
            dateto = Convert.ToDateTime(vm.REPORTS.REPORT_TYPE4);

            //cboTestLabel.Items.Clear();

            //for (int i = 0; i < tagged.Rows.Count; i++)
            //{
            //    if (taggedFrmSuspa_[i] == "YES") //tagged
            //        cboTestLabel.Items.Add(tagged.Rows[i]["description"].ToString().Trim());
            //}

            //combfacility.DataSource = facilities;
            //combfacility.ValueMember = "type_code";
            //combfacility.DisplayMember = "name";

            //combfacility.SelectedValue = selectedfacility;
            //combfacility.Text = bissclass.combodisplayitemCodeName("type_code", selectedfacility, facilities, "name");

            // cboTestLabel.Show();
        }

        //private void SampleLabelGenerator_Load(object sender, EventArgs e)
        //{
        //    txtReference.Select();
        //}

        //private void btnReference_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btnReference")
        //    {
        //        this.txtReference.Text = "";
        //        lookupsource = "SL";
        //        msmrfunc.mrGlobals.crequired = "SL";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED SAMPLE DETAILS";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "SL")
        //    {
        //        txtReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtReference.Focus();
        //    }
        //}

        //private void btnPreview_Click(object sender, EventArgs e)
        //{
        //    printprocess(false);
        //}

        public MR_DATA.REPORTS printprocess()
        {
            bool isprint = false;

            //if (string.IsNullOrWhiteSpace(txtName.Text))
            //{
            //    DialogResult result = MessageBox.Show("Payment Record/Customer must be Retrieved...");
            //    txtReference.Focus();
            //    return;
            //}
            vm.REPORTS.SessionRDLC = "SampleLabel.rdlc";
            vm.REPORTS.SessionSQL = "";
            //vm.REPORTS.SessionInv_dtl = cboTestLabel.Text.Trim();
            mrptheader = "SAMPLE LABLE GENERATOR ";
            string xrpttype = "SAMPLELABLE";

           
            frmReportViewer receipt = new frmReportViewer(mrptheader, mrptheader, "", "", "", xrpttype, 
                vm.REPORTS.txtreference, 0m, "", "", "", ds, isdataset, 0, datefrom, dateto, "", isprint, 
                "", woperator, vm.REPORTS);

            vm.REPORTS = receipt.Show(vm.REPORTS.SessionRDLC, vm.REPORTS.SessionSQL, vm.REPORTS.PRINT);
            
            //else
            //{
            //    MRrptConversion.GeneralRpt(mrptheader, mrptheader, "", "", "", xrpttype, txtReference.Text, 0M, "", "", "", ds, 0, datefrom, dateto, "", true, true, "", woperator);
            //}

            return vm.REPORTS;
        }

        //private void btnPrint_Click(object sender, EventArgs e)
        //{
        //    printprocess(true);
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void txtReference_Leave(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtReference.Text) || combfacility.SelectedValue == null)
        //        return;
        //    ds.Clear();
        //    ds.Tables.Clear();
        //    DataTable dtsample = Dataaccess.GetAnytable("", "MR", "SELECT * FROM phl01 WHERE rtrim(REFERENCE) = '" + txtReference.Text.Trim() + "' and facility = '" + combfacility.SelectedValue.ToString() + "'", false);
        //    if (dtsample.Rows.Count < 1)
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Reference...");
        //        txtReference.Text = "";
        //        return;
        //    }
        //    txtName.Text = dtsample.Rows[0]["name"].ToString();
        //    datefrom = dateto = (DateTime)dtsample.Rows[0]["trans_date"];
        //    ds.Tables.Add(dtsample);
        //    isdataset = true;
        //}

        //private void txtReference_Enter(object sender, EventArgs e)
        //{
        //    if (combfacility.SelectedValue == null)
        //    {
        //        DialogResult resut = MessageBox.Show("Service Centre must be selected...");
        //        combfacility.Select();
        //        return;
        //    }
        //}



    }
}