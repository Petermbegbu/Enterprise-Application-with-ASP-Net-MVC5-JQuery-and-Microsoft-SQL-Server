#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using mradmin.BissClass;
using mradmin.DataAccess;
using mradmin.Forms;

using msfunc;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace ANC.Forms
{
    public partial class frmSocialnFamilyHistory
    {
        ANC02 anc02 = new ANC02();
        string reference;
        string groupcode, patientno;

        public frmSocialnFamilyHistory(ANC02 Anc02, string Reference, string Groupcode, string Patientno)
        {
            //InitializeComponent();
            anc02 = Anc02;
            reference = Reference;
            groupcode = Groupcode;
            patientno = Patientno;
        }

        private void frmSocialnFamilyHistory_Load(object sender, EventArgs e)
        {
            displaydetails();
        }
        void displaydetails()
        {
            anc02 = ANC02.GetANC02(groupcode, patientno);
            if (anc02 == null)
                return;

            txtAlcolhol.Text = anc02.ALCOHOL;
            txttobacco.Text = anc02.SMOKING;
            txtRecreationalDrugs.Text = anc02.RECREATIONDRGS; //anc02.SOCIALDETAILS;
            txtHypertension.Text = anc02.FAM_HYPERTENSION;
            txtDiabetes.Text = anc02.FAM_DIABETES;
            txtSicklecell.Text = anc02.FAM_SICKLE_CELL;
            txtGenetic.Text = anc02.FAM_GENETIC;
            txtTwinning.Text = anc02.TWINNING;
            txtOthers.Text = anc02.FAM_OTHERS; //anc02.FAMILYDETAILS;

            chkalcolhol.Checked = string.IsNullOrWhiteSpace(anc02.ALCOHOL) ? false : true;
            chkTobacco.Checked = string.IsNullOrWhiteSpace(anc02.SMOKING) ? false : true;
            ChkRecreationalD.Checked = string.IsNullOrWhiteSpace(anc02.RECREATIONDRGS) ? false : true;
            ChkHypertension.Checked = string.IsNullOrWhiteSpace(anc02.FAM_HYPERTENSION) ? false : true;
            ChkDiabetes.Checked = string.IsNullOrWhiteSpace(anc02.FAM_DIABETES) ? false : true;
            ChkSickleCell.Checked = string.IsNullOrWhiteSpace(anc02.FAM_SICKLE_CELL) ? false : true;
            ChkGenetic.Checked = string.IsNullOrWhiteSpace(anc02.FAM_GENETIC) ? false : true;
            ChkTwinning.Checked = string.IsNullOrWhiteSpace(anc02.TWINNING) ? false : true;
            ChkOthers.Checked = string.IsNullOrWhiteSpace(anc02.FAM_OTHERS) ? false : true; //anc02.FAMILYDETAILS;

        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
           // msgeventtracker = "SD";
            DialogResult result = MessageBox.Show("Confirm to Save Details...", "Patient Social/Family Medical History", MessageBoxButtons.YesNo,
              MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
                return;
            if (anc02 == null)
                writePage2();

            savedetails();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void writePage2()
        {

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "ANC02_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", reference);
            insertCommand.Parameters.AddWithValue("@patientno", patientno);
            insertCommand.Parameters.AddWithValue("@GROUPHEAD", "");
            insertCommand.Parameters.AddWithValue("@groupcode", groupcode);
            insertCommand.Parameters.AddWithValue("@GROUPHTYPE", "");
            insertCommand.Parameters.AddWithValue("POSTED", false);
            insertCommand.Parameters.AddWithValue("POST_DATE", DateTime.Now);
            insertCommand.Parameters.AddWithValue("TRANS_DATE", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@diabetes", "");
            insertCommand.Parameters.AddWithValue("@HYPERTENSION", "");
            insertCommand.Parameters.AddWithValue("@HEART_DISEASE", "");
            insertCommand.Parameters.AddWithValue("@SICKLE_CELL", "");
            insertCommand.Parameters.AddWithValue("@PULMONARY", "");
            insertCommand.Parameters.AddWithValue("@KIDNEYDISEASE", "");
            insertCommand.Parameters.AddWithValue("@HEPATITIS", "");
            insertCommand.Parameters.AddWithValue("@NEUROLOGIC", "");
            insertCommand.Parameters.AddWithValue("@THYROID", "");
            insertCommand.Parameters.AddWithValue("@PSYCHIATRIC", "");
            //end of page1
            insertCommand.Parameters.AddWithValue("@PREV_PREG_TOTAL", "");
            insertCommand.Parameters.AddWithValue("@NOALIVE", "");
            insertCommand.Parameters.AddWithValue("@DEPRESSION", "");
            insertCommand.Parameters.AddWithValue("@VARICOSITIES", "");
            insertCommand.Parameters.AddWithValue("@D_RH_SENSITIZATION", "");
            insertCommand.Parameters.AddWithValue("@BLOOD_TRANSFUSIONS", "");
            insertCommand.Parameters.AddWithValue("@HIV", "");
            insertCommand.Parameters.AddWithValue("@BREAST_LUMPS", "");
            insertCommand.Parameters.AddWithValue("@GYNESURGERIES", "");
            insertCommand.Parameters.AddWithValue("@DRUG_ALLERGIES", "");
            insertCommand.Parameters.AddWithValue("@OPERATIONS", "");
            insertCommand.Parameters.AddWithValue("@ANAESTHETIC", "");
            insertCommand.Parameters.AddWithValue("@PAPSMEAR", "");
            insertCommand.Parameters.AddWithValue("@INFERTILITY", "");
            insertCommand.Parameters.AddWithValue("@OTHERS", "");
            insertCommand.Parameters.AddWithValue("@ALCOHOL", "");
            insertCommand.Parameters.AddWithValue("@SMOKING", "");
            insertCommand.Parameters.AddWithValue("@SOCIALDETAILS", "");
            insertCommand.Parameters.AddWithValue("@FAM_HYPERTENSION", "");
            insertCommand.Parameters.AddWithValue("@FAM_DIABETES", "");
            insertCommand.Parameters.AddWithValue("@FAM_SICKLE_CELL", "");
            insertCommand.Parameters.AddWithValue("@FAM_GENETIC", "");
            insertCommand.Parameters.AddWithValue("@FAM_OTHERS", "");
            insertCommand.Parameters.AddWithValue("@FAMILYDETAILS", "");
            insertCommand.Parameters.AddWithValue("@AP_PROGUANIL", "");
            insertCommand.Parameters.AddWithValue("@AP_PYRIMETHAMINE1", "");
            insertCommand.Parameters.AddWithValue("@AP_PYRIMETHAMINE2", "");
            insertCommand.Parameters.AddWithValue("@AP_PYRIMETHAMINE3", "");
            insertCommand.Parameters.AddWithValue("@AP_OTHERS", "");
            insertCommand.Parameters.AddWithValue("@TETANUS1", "");
            insertCommand.Parameters.AddWithValue("@TETANUS2", "");
            insertCommand.Parameters.AddWithValue("@TETANUS3", "");
            insertCommand.Parameters.AddWithValue("@RECREATIONDRGS", "");
            insertCommand.Parameters.AddWithValue("@TWINNING", "");

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
              //  MessageBox.Show("Records Saved successfully...", "MEDICAL HISTORY PAGE 1 ");

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("Update ANC03A " + ex, "ANC Details", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
        void savedetails()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "ANC02_UpdateSFHistory";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", reference);
            insertCommand.Parameters.AddWithValue("@patientno", patientno);
            insertCommand.Parameters.AddWithValue("@groupcode", groupcode);

            insertCommand.Parameters.AddWithValue("@alcohol", txtAlcolhol.Text);
            insertCommand.Parameters.AddWithValue("@smoking", txttobacco.Text);
            insertCommand.Parameters.AddWithValue("@socialdetails", "");
            insertCommand.Parameters.AddWithValue("@fam_hypertension", txtHypertension.Text);
            insertCommand.Parameters.AddWithValue("@fam_diabetes", txtDiabetes.Text);
            insertCommand.Parameters.AddWithValue("@fam_sickle_cell", txtSicklecell.Text);
            insertCommand.Parameters.AddWithValue("@fam_genetic", txtGenetic.Text);
            insertCommand.Parameters.AddWithValue("@fam_others", txtOthers.Text);
            insertCommand.Parameters.AddWithValue("@familydetails", "");
            insertCommand.Parameters.AddWithValue("@recreationdrgs", txtRecreationalDrugs.Text);
            insertCommand.Parameters.AddWithValue("@twinning", txtTwinning.Text);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Records Saved successfully...", "Patient Social/Family Medical History");
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("Update ANC02 - Others " + ex, "", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}