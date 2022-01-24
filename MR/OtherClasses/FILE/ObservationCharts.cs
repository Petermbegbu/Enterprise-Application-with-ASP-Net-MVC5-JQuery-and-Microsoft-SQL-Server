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
using mradmin.BissClass;
using mradmin.DataAccess;
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class ObservationCharts
    {
        DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name ", true),
            dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES order by name", true);

        string anycode, lookupsource, woperator;
        Admrecs admrecs = new Admrecs();
        //DataTable dtfacility, dtdiag;
        billchaindtl bchain;
        int recno1, recno2, recno3, colid;
        MR_DATA.MR_DATAvm vm;

        //  string calenderText;

        public ObservationCharts(MR_DATA.MR_DATAvm VM2, string xoperator)
        {
            //InitializeComponent();
            //dtfacility = facility;
            //dtdiag = diag;
            //txtreference.Text = reference;
            //admrecs = admissionsrec;
            //monthCalendar1.TodayDate = DateTime.Now.Date;
            //monthCalendar1.Value = DateTime.Now.Date;

            vm = VM2;
            woperator = xoperator;
        }

        //public ObservationCharts(DataTable facility, DataTable diag, string reference, Admrecs admissionsrec, string xoperator)
        //{
        //    //InitializeComponent();
        //    dtfacility = facility;
        //    dtdiag = diag;
        //    txtreference.Text = reference;
        //    admrecs = admissionsrec;
        //    monthCalendar1.TodayDate = DateTime.Now.Date;
        //    monthCalendar1.Value = DateTime.Now.Date;
        //    woperator = xoperator;
        //}

        //private void ObservationCharts_Load(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(txtreference.Text) && !string.IsNullOrWhiteSpace(admrecs.REFERENCE))
        //    {
        //        displayadmrecs();
        //        edtprofile.Focus();
        //    }
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    string xstring = chkCurrntAdmSu.Checked ? "[CURRENT ADMISSIONS]" : "[ALL]";
        //    if (btn.Name == "btnreferenceSu")
        //    {
        //        txtreference.Text = "";
        //        lookupsource = "A";
        //        msmrfunc.mrGlobals.crequired = "A";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED ADMISSIONS " + xstring;
        //        msmrfunc.mrGlobals.lookupCriteria = chkCurrntAdmSu.Checked ? "C" : "";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    msmrfunc.mrGlobals.lookupCriteria = "";
        //    if (lookupsource == "A")
        //    {
        //        txtreference.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        txtreference.Focus();
        //    }
        //    return;
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void btnGraph_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(cboChartOptions.Text))
        //    {
        //        DialogResult result = MessageBox.Show("Graph Option must be selected...");
        //        return;
        //    }
        //    Session["name"] = txtName.Text.Trim();
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select BPSITTING, PULSE, TEMP, TRANS_DATE from MRB22 where groupcode = '" + txtGroupcode.Text + "' and PATIENTNO = '" + txtPatientno.Text + "'", false);
        //    Form chart;
        //    if (cboChartOptions.SelectedItem.ToString() == "BP")
        //        chart = new Chart_BP(dt);
        //    else if (cboChartOptions.SelectedItem.ToString() == "PULSE")
        //        chart = new Chart_pulse(dt);
        //    else
        //        chart = new Chart_Temp(dt);
        //    chart.Show();

        //}

        //private void txtreferenceSU_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtreference.Text))
        //        return;

        //    if (string.IsNullOrWhiteSpace(anycode) && txtreference.Text.Substring(0, 1) != "A")
        //    {
        //        if (bissclass.IsDigitsOnly(txtreference.Text.Trim()))
        //            this.txtreference.Text = bissclass.autonumconfig(this.txtreference.Text, true, "A", "999999999");
        //    }

        //    //check if reference exist
        //    anycode = "";
        //    admrecs = Admrecs.GetADMRECS(txtreference.Text);
        //    if (admrecs == null) //new defintion
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Admission Reference...", "ADMISSION DETAILS");
        //        txtreference.Text = "";
        //        txtreference.Focus();
        //        return;
        //    }
        //    if (!displayadmrecs())
        //    {
        //        txtreference.Text = "";
        //        txtreference.Focus();
        //    }
        //}

        //bool displayadmrecs()
        //{
        //    txtGroupcode.Text = admrecs.GROUPCODE;
        //    txtPatientno.Text = admrecs.PATIENTNO;
        //    txtName.Text = admrecs.NAME;
        //    lblfaciitySu.Text = bissclass.combodisplayitemCodeName("type_code", admrecs.FACILITY, dtfacility, "name"); 
        //    txtWardRm.Text = admrecs.ROOM;
        //    txtbed.Text = admrecs.BED;
        //    txtDiagnosis.Text = bissclass.combodisplayitemCodeName("type_code", admrecs.DIAGNOSIS, dtdiag, "name");
        //    if (txtDiagnosis.Text.Trim() != admrecs.DIAGNOSIS_ALL && !string.IsNullOrWhiteSpace(admrecs.DIAGNOSIS_ALL))
        //        txtDiagnosis.Text = admrecs.DIAGNOSIS_ALL;
        //    txtAdm_date.Text = admrecs.ADM_DATE.ToShortDateString();
        //    txtdischarge_date.Text = admrecs.DISCHARGE;
        //    DateTime xdischarge = string.IsNullOrWhiteSpace(admrecs.DISCHARGE) ? DateTime.Now.Date : Convert.ToDateTime(admrecs.DISCHARGE);
        //        lblLenghtofStay.Text = (xdischarge - admrecs.ADM_DATE).Days.ToString() + " day(s)";

        //    bchain = billchaindtl.Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);
        //    txtAddress.Text = bchain.RESIDENCE;
        //    edtprofile.Text = patientprofile();

        //    txtgrouphead.Text = msmrfunc.GETGroupheadname(bchain.GHGROUPCODE, bchain.GROUPHEAD, bchain.GROUPHTYPE);
        //    if (txtgrouphead.Text.Trim() == "Abort")
        //    {
        //        return false;
        //    }
        //    displaydetails();
        //    if (admrecs.DISCHARGE != "")
        //    {
        //        DialogResult result = MessageBox.Show("This Patient has been discharged on " + admrecs.DISCHARGE, admrecs.NAME);
        //        return false;
        //    }
        //    return true;
        //}

        string patientprofile()
        {
            //edtprofileRev.Text = 
            string xtext =
            "[ " + bchain.SEX + " ] ;    AGE : ";
            string xx = (bchain.BIRTHDATE.Year > 1920) ? bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now.Date) :
            (bchain.RELATIONSH == "C") ? "Minor" : (bchain.RELATIONSH == "S" || bchain.RELATIONSH == "W" ||
            bchain.RELATIONSH == "H") ? "< Adult >" : "...";
            string xx1 = "     M_STATUS : < " + bchain.M_STATUS + " > ";
            xtext = xtext + xx + "; " + xx1;
            return xtext;
        }

        //void displaydetails()
        //{
        //    /*	SELECT mrb22 //bp
        //     	SELECT mrb23 //fluid
        //        SELECT mrb24 //diabetics*/
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select * from mrb22 where reference = '" + txtreference.Text + "'", false);
        //    DataGridViewRow dgv;
        //    DataRow row;
        //   // string xdate;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        dataGridView1.Rows.Add();
        //        dgv = dataGridView1.Rows[i];
        //        row = dt.Rows[i];
        //     //   xdate = Convert.ToDateTime(row["trans_date"]).ToShortDateString()+" "+row["time"].ToString().Trim();
        //     //   xdate = xdate.Replace("PM", ":00");
        //     //   xdate = xdate.Replace("AM", ":00");
        //        dgv.Cells[0].Value = Convert.ToDateTime(row["trans_date"]).ToShortDateString(); // row["trans_date"].ToString();
        //        dgv.Cells[2].Value = row["time"].ToString().Trim();
        //        dgv.Cells[3].Value = row["temp"].ToString();
        //        dgv.Cells[4].Value = row["pulse"].ToString();
        //        dgv.Cells[5].Value = row["respiratio"].ToString();
        //        dgv.Cells[6].Value = row["bpsitting"].ToString();
        //        dgv.Cells[7].Value = row["sp02"].ToString();
        //        dgv.Cells[8].Value = row["remark"].ToString();
        //        dgv.Cells[9].Value = (bool)row["posted"] ? true : false;
        //        dgv.Cells[10].Value = row["recid"].ToString();
        //        dgv.Cells[11].Value = (bool)row["posted"] ? "YES" : "NO";
        //        if ((bool)row["posted"])
        //            dgv.ReadOnly = true;
        //    }
        //    dt = Dataaccess.GetAnytable("", "MR", "select * from mrb23 where reference = '" + txtreference.Text + "'", false);
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        dataGridView2.Rows.Add();
        //        dgv = dataGridView2.Rows[i];
        //        row = dt.Rows[i];
        //        dgv.Cells[0].Value = Convert.ToDateTime(row["trans_date"]).ToShortDateString();
        //        dgv.Cells[2].Value = row["time"].ToString().Trim();
        //        dgv.Cells[3].Value = row["natureoffluid"].ToString();
        //        dgv.Cells[4].Value = row["oral"].ToString();
        //        dgv.Cells[5].Value = row["rectral"].ToString();
        //        dgv.Cells[6].Value = row["iv"].ToString();
        //        dgv.Cells[7].Value = row["otherroutes"].ToString();
        //        dgv.Cells[8].Value = row["inflo_total"].ToString();
        //        dgv.Cells[9].Value = row["inflo_Remarks"].ToString();
        //        dgv.Cells[10].Value = row["urine"].ToString();
        //        dgv.Cells[11].Value = row["vomitus"].ToString();
        //        dgv.Cells[12].Value = row["tube"].ToString();
        //        dgv.Cells[13].Value = row["others"].ToString();
        //        dgv.Cells[14].Value = row["out_total"].ToString();
        //        dgv.Cells[15].Value = row["balance"].ToString();
        //        dgv.Cells[16].Value = row["chloride"].ToString();
        //        dgv.Cells[17].Value = row["Outflo_Remarks"].ToString();
        //        dgv.Cells[18].Value = (bool)row["posted"] ? true : false;
        //        dgv.Cells[19].Value = row["recid"].ToString();
        //        dgv.Cells[21].Value = (bool)row["posted"] ? "YES" : "NO";
        //        if ((bool)row["posted"])
        //            dgv.ReadOnly = true;
        //    }
        //    dt = Dataaccess.GetAnytable("", "MR", "select * from mrb24 where reference = '" + txtreference.Text + "'", false);
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        dataGridView3.Rows.Add();
        //        dgv = dataGridView3.Rows[i];
        //        row = dt.Rows[i];
        //        dgv.Cells[0].Value = Convert.ToDateTime(row["trans_date"]).ToShortDateString();
        //        dgv.Cells[2].Value = row["time"].ToString().Trim();
        //        dgv.Cells[3].Value = row["rbs"].ToString();
        //        dgv.Cells[4].Value = row["fbs"].ToString();
        //        dgv.Cells[5].Value = row["urinalysis"].ToString();
        //        dgv.Cells[6].Value = row["acetone"].ToString();
        //        dgv.Cells[7].Value = row["drgadmin"].ToString();
        //        dgv.Cells[8].Value = row["remark"].ToString();
        //        dgv.Cells[9].Value = row["operator"].ToString();
        //        dgv.Cells[10].Value = (bool)row["posted"] ? true : false;
        //        dgv.Cells[11].Value = row["recid"].ToString();
        //        dgv.Cells[12].Value = (bool)row["posted"] ? "YES" : "NO";
        //        if ((bool)row["posted"])
        //            dgv.ReadOnly = true;
        //    }
        //}

        public MR_DATA.REPORTS btnSubmit_bp_Click(IEnumerable<MR_DATA.REPORTS> tableList, string obAdmReference)
        {
            //DialogResult result;
            //if (dataGridView1.Rows.Count < 1)
            //    return;

            //result = MessageBox.Show("Confirm to Submit Records...", "Intensive/Observation Chart Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            admrecs = Admrecs.GetADMRECS(obAdmReference);
            bchain = billchaindtl.Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);

            bool xnew = false;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            connection.Open();

            foreach (var row in tableList)
            {
                if (row.cbogender == null || row.txtgrouphead == "YES" || row.doctor != "Y")
                    continue;

                xnew = row.cboReligion == "0" || row.cboReligion.ToString() == "" ? true : false;
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = xnew ? "mrb22_Add" : "mrb22_Update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@GROUPCODE", bchain.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@PATIENTNO", bchain.PATIENTNO);
                insertCommand.Parameters.AddWithValue("@REFERENCE", obAdmReference);
                insertCommand.Parameters.AddWithValue("@GENDER", bchain.SEX);
                insertCommand.Parameters.AddWithValue("@WLBS", "");
                insertCommand.Parameters.AddWithValue("@WSTONE", "");
                insertCommand.Parameters.AddWithValue("@WEIGHT", 0m);
                insertCommand.Parameters.AddWithValue("@HFT", "");
                insertCommand.Parameters.AddWithValue("@HIN", "");
                insertCommand.Parameters.AddWithValue("@HIGHT", 0m);
                insertCommand.Parameters.AddWithValue("@BPSITTING", row.txtbranch);
                insertCommand.Parameters.AddWithValue("@BPSTANDING", "");
                insertCommand.Parameters.AddWithValue("@PULSE", row.txtclinic);
                insertCommand.Parameters.AddWithValue("@TEMP", row.cboTribe);
                insertCommand.Parameters.AddWithValue("@RESPIRATIO", row.txtcurrency);
                insertCommand.Parameters.AddWithValue("@BMP", 0m);
                insertCommand.Parameters.AddWithValue("@REMARK", row.txtemployer);
                insertCommand.Parameters.AddWithValue("@CLINIC", "");
                insertCommand.Parameters.AddWithValue("@SP02", row.txtothername);
                insertCommand.Parameters.AddWithValue("@TIME", row.cbotitle);
                insertCommand.Parameters.AddWithValue("@POSTED", row.mcanalter ? true : false);
                insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@TRANS_DATE", Convert.ToDateTime(row.cbogender));
                insertCommand.Parameters.AddWithValue("@HAIRCOLOR", "");
                insertCommand.Parameters.AddWithValue("@HAIRTYPE", "");
                insertCommand.Parameters.AddWithValue("@EYECOLOR", "");
                insertCommand.Parameters.AddWithValue("@COMPLEXION", "");
                insertCommand.Parameters.AddWithValue("@RACIALGRP", "");
                insertCommand.Parameters.AddWithValue("@ETHNICITY", "");
                insertCommand.Parameters.AddWithValue("@RELIGION", "");
                insertCommand.Parameters.AddWithValue("@BLOODGRP", "");
                if (!xnew)
                    insertCommand.Parameters.AddWithValue("@RECID", Convert.ToInt32(row.cboReligion));

                // connection.Open();
                insertCommand.ExecuteNonQuery();
                //dgv.Cells[11].Value = "";
                //  btnSubmit_bp.Enabled = false;
                // txtreference.Text = "";
            }

            connection.Close();
            vm.REPORTS.alertMessage = "Records Submitted Successfully...";

            return vm.REPORTS;
        }

        public MR_DATA.REPORTS btnSubmit_fluid_Click(IEnumerable<MR_DATA.REPORTS> tableList, string obAdmReference)
        {
            // outtotal -ALLTRIM(STR(VAL(mrb23.urine)+VAL(mrb23.vomitus)+VAL(mrb23.tube)+VAL(mrb23.others)))
            //balance - ALLTRIM(STR(VAL(mrb23.inflo_total)-VAL(mrb23.out_total)))
            //DialogResult result;
            //if (dataGridView2.Rows.Count < 1)
            //    return;

            //result = MessageBox.Show("Confirm to Submit Records...", "Fuid Chart Details", MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            admrecs = Admrecs.GetADMRECS(obAdmReference);
            bchain = billchaindtl.Getbillchain(admrecs.PATIENTNO, admrecs.GROUPCODE);

            bool xnew = false;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            connection.Open();

            foreach (var row in tableList)
            {
                if (row.cbogender == null || row.txtkinaddress1 == "YES" || row.txtdepartment != "Y")
                    continue;

                xnew = row.cbogenotype == "" || row.cbogenotype == "0" ? true : false;

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = xnew ? "mrb23_Add" : "mrb23_update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@GROUPCODE", bchain.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@PATIENTNO", bchain.PATIENTNO);
                insertCommand.Parameters.AddWithValue("@REFERENCE", obAdmReference);
                insertCommand.Parameters.AddWithValue("@GENDER", bchain.SEX);
                insertCommand.Parameters.AddWithValue("@ORAL", row.txtclinic);
                insertCommand.Parameters.AddWithValue("@IV", row.txtbranch);
                insertCommand.Parameters.AddWithValue("@N_GASTRIC", "");
                insertCommand.Parameters.AddWithValue("@URINE", row.cboReligion);
                insertCommand.Parameters.AddWithValue("@VOMITUS", row.txtgrouphead);
                insertCommand.Parameters.AddWithValue("@OTHERS", row.txtworkphone);
                insertCommand.Parameters.AddWithValue("@TIME", row.cbotitle);
                insertCommand.Parameters.AddWithValue("@POSTED", row.mcanalter ? true : false);
                insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@TRANS_DATE", Convert.ToDateTime(row.cbogender));
                insertCommand.Parameters.AddWithValue("@NATUREOFFLUID", row.cboTribe);
                insertCommand.Parameters.AddWithValue("@RECTRAL", row.txtcurrency);
                insertCommand.Parameters.AddWithValue("@OTHERROUTES", row.txtothername);
                insertCommand.Parameters.AddWithValue("@INFLO_TOTAL", row.txtemployer);
                insertCommand.Parameters.AddWithValue("@TUBE", row.combillcycle);
                insertCommand.Parameters.AddWithValue("@OUT_TOTAL", row.doctor);
                insertCommand.Parameters.AddWithValue("@CHLORIDE", row.cbokinstate);
                insertCommand.Parameters.AddWithValue("@BALANCE", row.cbobloodgroup);
                insertCommand.Parameters.AddWithValue("@INFLO_REMARKS", row.txtconsultamt);
                insertCommand.Parameters.AddWithValue("@OUTFLO_REMARKS", row.txtkinphone);

                if (!xnew)
                    insertCommand.Parameters.AddWithValue("@RECID", Convert.ToInt32(row.cbogenotype));

                // connection.Open();
                insertCommand.ExecuteNonQuery();
                //dgv.Cells[21].Value = "";
                //btnSubmit_fluid.Enabled = false;
            }

            connection.Close();

            vm.REPORTS.alertMessage = "Records Submitted Successfully...";

            return vm.REPORTS;
        }

        public MR_DATA.REPORTS btnSubmit_Db_Click(IEnumerable<MR_DATA.REPORTS> tableList, string obAdmReference)
        {
            //DialogResult result;
            //if (dataGridView2.Rows.Count < 1)
            //    return;
            //result = MessageBox.Show("Confirm to Submit Records...", "Diabetic Chart Details", MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;

            bool xnew = false;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            connection.Open();

            foreach (var row in tableList)
            {
                if (row.cbogender == null || row.txtgrouphead == "YES" || row.doctor != "Y")
                    continue;

                xnew = row.cboReligion == "0" || row.cboReligion == "" ? true : false;

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = xnew ? "mrb24_Add" : "mrb24_update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@GROUPCODE", bchain.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@PATIENTNO", bchain.PATIENTNO);
                insertCommand.Parameters.AddWithValue("@REFERENCE", obAdmReference);
                insertCommand.Parameters.AddWithValue("@GENDER", bchain.SEX);
                insertCommand.Parameters.AddWithValue("@RBS", row.cboTribe);
                insertCommand.Parameters.AddWithValue("@FBS", row.txtclinic);
                insertCommand.Parameters.AddWithValue("@URINALYSIS", row.txtcurrency);
                insertCommand.Parameters.AddWithValue("@ACETONE", row.txtbranch);
                insertCommand.Parameters.AddWithValue("@DRGADMIN", row.txtothername);
                insertCommand.Parameters.AddWithValue("@REMARK", row.txtemployer);
                insertCommand.Parameters.AddWithValue("@OPERATOR", woperator);
                insertCommand.Parameters.AddWithValue("@DTTIME", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@POSTED", row.mcanalter ? true : false);
                insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@TRANS_DATE", Convert.ToDateTime(row.cbogender));
                insertCommand.Parameters.AddWithValue("@TIME", row.cbotitle);

                if (!xnew)
                    insertCommand.Parameters.AddWithValue("@RECID", Convert.ToInt32(row.cboReligion));

                // connection.Open();
                insertCommand.ExecuteNonQuery();

                //btnSubmit_Db.Enabled = false;
                //dgv.Cells[12].Value = "";
            }

            connection.Close();

            vm.REPORTS.alertMessage = "Records Submitted Successfully...";

            return vm.REPORTS;
        }

        //private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridViewRow dgv = new DataGridViewRow();
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        dgv = dataGridView1.Rows[e.RowIndex];
        //        if (dgv.Cells[0].Value != null)
        //        {
        //            btnFluidDelete.Enabled = true;
        //            recno3 = e.RowIndex;
        //            dgv.Cells[12].Value = "Y";
        //        }
        //    }
        //}

        //private void dataGridView2_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridView sdg = sender as DataGridView;
        //    DataGridViewRow dgv = new DataGridViewRow();
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        dgv = dataGridView2.Rows[e.RowIndex];
        //        if (sdg.Name == "dataGridView2" && dgv.Cells[e.ColumnIndex].FormattedValue.ToString() != "")
        //        {
        //            if (e.ColumnIndex > 3 && e.ColumnIndex < 8 || e.ColumnIndex > 9 && e.ColumnIndex < 14)
        //            {
        //                int xvalue = bissclass.GetNumberFromString(dgv.Cells[e.ColumnIndex].ToString()); //, "Fluid (in Mls)"))
        //                if (xvalue == 0)
        //                    return;
        //                if (e.ColumnIndex < 7)
        //                {
        //                    decimal xv = 0m;
        //                    for (int i = 4; i < 8; i++)
        //                    {
        //                        if (dgv.Cells[i].FormattedValue.ToString() != "")
        //                            xv += Convert.ToDecimal(dgv.Cells[i].Value);
        //                    }
        //                    dgv.Cells[8].Value = xv.ToString("N2");
        //                }
        //                else
        //                {
        //                    decimal xv = 0m;
        //                    for (int i = 10; i < 14; i++)
        //                    {
        //                        if (dgv.Cells[i].FormattedValue.ToString() != "")
        //                            xv += Convert.ToDecimal(dgv.Cells[i].Value);
        //                    }
        //                    dgv.Cells[14].Value = xv.ToString("N2");
        //                }
        //                decimal inf, otf; inf = otf = 0m;
        //                if (dgv.Cells[8].FormattedValue.ToString() != "")
        //                    inf = Convert.ToDecimal(dgv.Cells[8].Value);
        //                if (dgv.Cells[14].FormattedValue.ToString() != "")
        //                    otf = Convert.ToDecimal(dgv.Cells[14].Value);

        //                dgv.Cells[15].Value = (inf - otf).ToString("N2");
        //            }
        //        }

        //        if (dgv.Cells[0].Value != null)
        //        {
        //            btnFluidDelete.Enabled = true;
        //            recno2 = e.RowIndex;
        //            dgv.Cells[21].Value = "Y";
        //        }
        //    }
        //}

        //private void dataGridView3_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridViewRow dgv = new DataGridViewRow();
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        dgv = dataGridView3.Rows[e.RowIndex];
        //        if (dgv.Cells[0].Value != null)
        //        {
        //            btnFluidDelete.Enabled = true;
        //            recno3 = e.RowIndex;
        //            dgv.Cells[13].Value = "Y";
        //        }
        //    }
        //}

        //private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridView sdg = sender as DataGridView;
        //    DataGridViewRow dgv = new DataGridViewRow();
        //    if (e.RowIndex >= 0 && e.ColumnIndex == 0) // datetime
        //    {
        //        if (sdg.Name == "dataGridView1")
        //            dgv = dataGridView1.Rows[e.RowIndex];
        //        else if (sdg.Name == "dataGridView2") // datetime
        //            dgv = dataGridView2.Rows[e.RowIndex];
        //        else
        //            dgv = dataGridView3.Rows[e.RowIndex];

        //        if (dgv.Cells[0].Value != null && string.IsNullOrWhiteSpace(dgv.Cells[0].Value.ToString()))
        //            dgv.Cells[0].Value = DateTime.Now;
        //    }
        //}

        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        DataGridView sdg = sender as DataGridView;
        //        if (sdg.Name == "dataGridView1" && e.ColumnIndex == 1)
        //        {
        //            colid = 1;
        //            recno1 = e.RowIndex;
        //            panelCalender.Location = new System.Drawing.Point(143, 177);
        //            panelCalender.Visible = true;
        //        }
        //        else if (sdg.Name == "dataGridView2" && e.ColumnIndex == 1)
        //        {
        //            colid = 2;
        //            recno2 = e.RowIndex;
        //            panelCalender.Location = new System.Drawing.Point(143, 177);
        //            panelCalender.Visible = true;
        //        }
        //        else if (sdg.Name == "dataGridView3" && e.ColumnIndex == 1)
        //        {
        //            colid = 3;
        //            recno3 = e.RowIndex;
        //            panelCalender.Location = new System.Drawing.Point(143, 177);
        //            panelCalender.Visible = true;
        //        }
        //    }
        //}

        //private void monthCalendar1_DateSelected(object sender, EventArgs e)
        //{
        //    //var monthCalendar = sender as MonthCalendar;
        //    Button btn = sender as Button;
        //    if (btn.Name == "btnDateSelect")
        //    {
        //        if (colid == 1)
        //            dataGridView1.Rows[recno1].Cells[0].Value = monthCalendar1.SelectionStart.ToShortDateString();
        //        else if (colid == 2)
        //            dataGridView2.Rows[recno2].Cells[0].Value = monthCalendar1.SelectionStart.ToShortDateString();
        //        else if (colid == 3)
        //            dataGridView3.Rows[recno3].Cells[0].Value = monthCalendar1.SelectionStart.ToShortDateString();

        //        panelCalender.Visible = false;
        //    }
        //    else if (btn.Name == "btnIgnorDateSelect")
        //    {
        //        panelCalender.Visible = false;
        //    }
        //}



    }
}