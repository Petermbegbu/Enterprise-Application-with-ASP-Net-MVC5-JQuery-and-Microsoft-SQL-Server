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
//using msfunc.Forms;

//using mradmin.Forms;
//using mradmin.DataAccess;
//using mradmin.BissClass;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmGroupFurtherDetails
    {
        MR_DATA.MR_DATAvm vm;

        DataTable dtcountry = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from countrycodes order by name", true), dtPatdetails;
        //bool newrec;
        //string patientno, groupcode;

        public frmGroupFurtherDetails(MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;

            dtPatdetails = Dataaccess.GetAnytable("", "MR", "SELECT * FROM PATDETAIL WHERE GROUPCODE = '" + vm.PATDETAIL.GROUPCODE + "' and patientno = '" + vm.PATDETAIL.PATIENTNO + "'", false);

            //InitializeComponent();

            //groupcode = xgroupcode;
            //patientno = xpatientno;
        }

        //private void frmGroupFurtherDetails_Load(object sender, EventArgs e)
        //{
        //    //initcomboboxes();

        //    //newrec = true;

        //    //GetPreviousRecord();
        //}

        //private void initcomboboxes()
        //{
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select Loccountry, locstate from mrcontrol where recid = '1'", false); //msmrfunc.getcontrolsetup("MR");

        //    string mloccountry = dt.Rows[0]["Loccountry"].ToString();
        //    cboNationality.DataSource = dtcountry;
        //    cboNationality.ValueMember = "Type_code";
        //    cboNationality.DisplayMember = "Name";

        //    this.cboOccupation.DataSource = Dataaccess.GetAnytable("", "CODES", "SELECT type_code, name from designationcodes order by name", true);
        //    this.cboOccupation.ValueMember = "Type_code";
        //    this.cboOccupation.DisplayMember = "Name";

        //    this.cboTribe.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT DISTINCT tribe from patdetail order by tribe", true);
        //    this.cboTribe.ValueMember = "tribe";
        //    this.cboTribe.DisplayMember = "tribe";

        //    cboLGA.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT DISTINCT lga from patdetail order by lga", true);
        //    this.cboLGA.ValueMember = "lga";
        //    this.cboLGA.DisplayMember = "lga";

        //    bissclass.displaycombo(cboNationality, dtcountry, mloccountry, "name");
        //}


        //void GetPreviousRecord()
        //{
        //    dtPatdetails = Dataaccess.GetAnytable("", "MR", "SELECT * FROM PATDETAIL WHERE GROUPCODE = '" + groupcode + "' and patientno = '" + patientno + "'", false);
        //    if (dtPatdetails.Rows.Count < 1)
        //        return;
        //    newrec = false;
        //    DataRow row = dtPatdetails.Rows[0];
        //    bissclass.displaycombo(cboNationality, dtcountry, row["nationality"].ToString(), "name");
        //    cboOccupation.Text = row["occupation"].ToString();
        //    cboReligion.Text = row["religion"].ToString();
        //    cboLGA.Text = row["lga"].ToString();
        //    cboTribe.Text = row["tribe"].ToString();
        //    cbobloodgroup.Text = row["bloodgroup"].ToString();
        //    cbogenotype.Text = row["genotype"].ToString();
        //    row["occupation"].ToString();
        //    txtNextofKin.Text = row["nextofkin"].ToString();
        //    txtNOK_Address.Text = row["nok_AdR1"].ToString();
        //    txtNOK_Phone.Text = row["nok_Phone"].ToString();
        //}


        //private void cboNationality_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtNextofKin_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    DialogResult result = MessageBox.Show("Confirm to Save Details...", "Patient Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

        //    if (result == DialogResult.No)
        //        return;
        //    savepatientdetails();
        //}


        public MR_DATA.REPORTS savepatientdetails(string newrec)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = newrec == "true" ? "Patdetail_Add" : "Patdetail_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", vm.PATDETAIL.PATIENTNO);
            insertCommand.Parameters.AddWithValue("@groupcode", vm.PATDETAIL.GROUPCODE);
            insertCommand.Parameters.AddWithValue("@nationality", vm.PATDETAIL.NATIONALITY == null ? "" : vm.PATDETAIL.NATIONALITY);
            insertCommand.Parameters.AddWithValue("@occupation", vm.PATDETAIL.OCCUPATION);
            insertCommand.Parameters.AddWithValue("@religion", vm.PATDETAIL.RELIGION);
            insertCommand.Parameters.AddWithValue("@bloodgroup", vm.PATDETAIL.BLOODGROUP);
            insertCommand.Parameters.AddWithValue("@genotype", vm.PATDETAIL.GENOTYPE);
            insertCommand.Parameters.AddWithValue("@nextofkin", vm.PATDETAIL.NEXTOFKIN);
            insertCommand.Parameters.AddWithValue("@nok_Adr1", vm.PATDETAIL.NOK_ADR1);
            insertCommand.Parameters.AddWithValue("@nok_Phone", vm.PATDETAIL.NOK_PHONE);
            insertCommand.Parameters.AddWithValue("@lga", vm.PATDETAIL.LGA);
            insertCommand.Parameters.AddWithValue("@tribe", vm.PATDETAIL.TRIBE);
            insertCommand.Parameters.AddWithValue("@NOK_RELATIONSHIP", newrec == "true" ? "" : dtPatdetails.Rows[0]["NOK_RELATIONSHIP"].ToString());
            insertCommand.Parameters.AddWithValue("@PREVIOUSMEDNOTES", newrec == "true" ? "" : dtPatdetails.Rows[0]["PREVIOUSMEDNOTES"].ToString());

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // throw ex;
                vm.REPORTS.alertMessage = "Unable to Open SQL Server Database Table" + ex;

                return vm.REPORTS;
            }
            finally
            {
                connection.Close();
            }

            vm.REPORTS.alertMessage = "Details submitted successfully...";

            //btnClose.PerformClick();

            return vm.REPORTS;
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}