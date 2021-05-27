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
using mradmin.Forms;
using mradmin.DataAccess;
using mradmin.BissClass;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmPrivateAcct
    {
        MR_DATA.MR_DATAvm vm;
        patientinfo patients = new patientinfo();
        billchaindtl billchain;
        string woperator;

        public frmPrivateAcct(MR_DATA.MR_DATAvm VM2, string xoperator)
        {
            vm = VM2;
            woperator = xoperator;
            billchain = new billchaindtl();

            billchain = billchaindtl.Getbillchain(vm.BILLCHAIN.SECTION, vm.BILLCHAIN.GROUPCODE);

            //InitializeComponent();

            //txtgroupcode.Text = "PVT      ";
            //txtpatientno.Text = bchain.PATIENTNO.Trim() + "P"; 
            //txtbillspayable.Text = txtpatientno.Text;
            //txtType.Text = "PRIVATE";
            //txtghgroupcode.Text = "PVT";
            //txtsurname.Text = bchain.SURNAME;
            //txtothername.Text = bchain.OTHERNAMES;
            //txtaddress1.Text = bchain.RESIDENCE;
            //combillcycle.Text = "CASH";
            //dtregistered.Value = DateTime.Now.Date;
            //comgender.Text = bchain.SEX;
            //commaritalstatus.Text = bchain.M_STATUS;
            //dtbirthdate.Value = bchain.BIRTHDATE;
            //txtpiclocation.Text = bchain.PICLOCATION;
            //TXTPATIENTNAME.Text = bchain.NAME;

            //billchain = bchain;
        }

        //private void frmPrivateAcct_Load(object sender, EventArgs e)
        //{
        //    //check if record has been created before
        //    patients = patientinfo.GetPatient(txtpatientno.Text, txtgroupcode.Text);
        //    if (patients != null)
        //    {
        //        DialogResult result = MessageBox.Show("This Private Account has been created once...", "PRIVATE ACCOUNT CREATION");
        //        btnClose.PerformClick();
        //        return;
        //    }
        //    //  woperator = msmrfunc.mrGlobals.WOPERATOR;
        //    initcomboboxes();
        //}

        //void initcomboboxes()
        //{
        //    this.combillcategory.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT reference, name from custclass order by name", true);
        //    combillcategory.ValueMember = "Reference";
        //    combillcategory.DisplayMember = "Name";
        //}

        //private void btnCreate_Click(object sender, EventArgs e)
        //{
        //    //validate fields
        //    if (!bissclass.IsPresent(this.txtsurname, "Patients Surname Name", false) ||
        //        !bissclass.IsPresent(this.txtothername, "Patients OtherName", false) ||
        //        !bissclass.IsPresent(this.combillcategory, "Patient Bill Category", false) ||
        //        !bissclass.IsPresent(this.combillcycle, "Patient Bill Cycle", false) ||
        //        !bissclass.IsPresent(this.Combillspayable, "Bills Payable By", false))
        //        return;

        //    btnCreate.Enabled = false;

        //    DialogResult result = MessageBox.Show("Confirm to Create Private Account for this Patient ?", "PRIVATE ACCOUNT CREATION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.Yes)
        //    {
        //        btnCreate.Enabled = false;
        //        savedetails();
        //    }

        //}

        public MR_DATA.REPORTS savedetails()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "Patient_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", vm.BILLCHAIN.PATIENTNO.Trim()); //patients.patientno);
            insertCommand.Parameters.AddWithValue("@groupcode", vm.BILLCHAIN.GROUPCODE.Trim()); // patients.groupcode);
            insertCommand.Parameters.AddWithValue("@patstatus", "A");  //patients.patstatus);
            insertCommand.Parameters.AddWithValue("@Address1", vm.BILLCHAIN.RESIDENCE); //patients.address1);
            insertCommand.Parameters.AddWithValue("@Patstate", ""); //patients.patstate);
            insertCommand.Parameters.AddWithValue("@Birthdate", vm.BILLCHAIN.BIRTHDATE); //patients.birthdate);
            insertCommand.Parameters.AddWithValue("@Sex", vm.BILLCHAIN.SEX); //patients.sex);
            insertCommand.Parameters.AddWithValue("@M_status", vm.BILLCHAIN.STATUS); //patients.m_status);
            insertCommand.Parameters.AddWithValue("@Reg_date", vm.BILLCHAIN.REG_DATE); //patients.reg_date);
            insertCommand.Parameters.AddWithValue("@contact", ""); //patients.contact);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", vm.BILLCHAIN.GHGROUPCODE); //patients.ghgroupcode);
            insertCommand.Parameters.AddWithValue("@pattype", vm.BILLCHAIN.GROUPHTYPE);
            insertCommand.Parameters.AddWithValue("@cr_limit", 0m);
            insertCommand.Parameters.AddWithValue("@bill_cir", vm.BILLCHAIN.HMOSERVTYPE); //for combillcycle;
            insertCommand.Parameters.AddWithValue("@UPCUR_DB", 0m);
            insertCommand.Parameters.AddWithValue("@UPCUR_CR", 0m);
            insertCommand.Parameters.AddWithValue("@cur_db", 0m);
            insertCommand.Parameters.AddWithValue("@cur_cr", 0m);
            insertCommand.Parameters.AddWithValue("@balbf", 0m);
            insertCommand.Parameters.AddWithValue("@SEC_LEVEL", 0m);
            insertCommand.Parameters.AddWithValue("@trans_date", DateTime.Now.Date);
            insertCommand.Parameters.AddWithValue("@POSTED", false);
            insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@name", vm.BILLCHAIN.NAME.Trim()); //patients.name);
            insertCommand.Parameters.AddWithValue("@grouphead", vm.BILLCHAIN.MEDNOTES); //patients.grouphead);
            insertCommand.Parameters.AddWithValue("@grouphtype", "P");
            insertCommand.Parameters.AddWithValue("@isgrouphead", true);
            insertCommand.Parameters.AddWithValue("@LASTSTATMT", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@title", vm.BILLCHAIN.TITLE);
            insertCommand.Parameters.AddWithValue("@patcateg", vm.BILLCHAIN.BILLONACCT);
            insertCommand.Parameters.AddWithValue("@remark", "");
            insertCommand.Parameters.AddWithValue("@discount", 0m); //patients.discount);
            insertCommand.Parameters.AddWithValue("@hmoservtype", ""); //patients.hmoservtype);
            insertCommand.Parameters.AddWithValue("@currency", ""); //patients.currency);
            insertCommand.Parameters.AddWithValue("@billregistration", false);
            insertCommand.Parameters.AddWithValue("@clinic", ""); //patients.clinic);
            insertCommand.Parameters.AddWithValue("@surname", vm.BILLCHAIN.SURNAME); //patients.surname);
            insertCommand.Parameters.AddWithValue("@othername", vm.BILLCHAIN.OTHERNAMES); //patients.othername);
            insertCommand.Parameters.AddWithValue("@homephone", ""); //patients.homephone);
            insertCommand.Parameters.AddWithValue("@workphone", ""); //patients.workphone);
            insertCommand.Parameters.AddWithValue("@employer", ""); //patients.employer);
            insertCommand.Parameters.AddWithValue("@emp_name", ""); // patients.emp_name);
            insertCommand.Parameters.AddWithValue("@emp_addr", ""); //patients.emp_addr);
            insertCommand.Parameters.AddWithValue("@emp_state", ""); //patients.emp_state);
            insertCommand.Parameters.AddWithValue("@cont_designation", "");
            insertCommand.Parameters.AddWithValue("@pr_doc", ""); //patients.pr_doc);
            insertCommand.Parameters.AddWithValue("@refer_dr", ""); //patients.refer_dr);
            insertCommand.Parameters.AddWithValue("@nationality", ""); //patients.nationality);
            insertCommand.Parameters.AddWithValue("@occupation", ""); //patients.occupation);
            insertCommand.Parameters.AddWithValue("@RELIGION", " ");
            insertCommand.Parameters.AddWithValue("@bloodgroup", vm.BILLCHAIN.PATCATEG); //patients.bloodgroup);
            insertCommand.Parameters.AddWithValue("@genotype", vm.BILLCHAIN.HMOCODE); //patients.genotype);
            insertCommand.Parameters.AddWithValue("@nextofkin", ""); //patients.nextofkin);
            insertCommand.Parameters.AddWithValue("@nok_adr1", ""); //patients.nok_adr1);
            insertCommand.Parameters.AddWithValue("@nok_state", ""); //patients.nok_state);
            insertCommand.Parameters.AddWithValue("@nok_phone", ""); //patients.nok_phone);
            insertCommand.Parameters.AddWithValue("@nok_relationship", ""); //patients.nok_relationship);
            insertCommand.Parameters.AddWithValue("@RHD", " ");
            insertCommand.Parameters.AddWithValue("@email", ""); //patients.email);
            insertCommand.Parameters.AddWithValue("@piclocation", "");
            insertCommand.Parameters.AddWithValue("@DEBIT1", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT1", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF1", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT2", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT2", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF2", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT3", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT3", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF3", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT4", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT4", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF4", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT5", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT5", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF5", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT6", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT6", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF6", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT7", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT7", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF7", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT8", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT8", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF8", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT9", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT9", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF9", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT10", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT10", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF10", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT11", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT11", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF11", 0m);
            insertCommand.Parameters.AddWithValue("@DEBIT12", 0m);
            insertCommand.Parameters.AddWithValue("@CREDIT12", 0m);
            insertCommand.Parameters.AddWithValue("@BALBF12", 0m);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

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

            savebillchainetails();

            return vm.REPORTS;
        }

        bool savebillchainetails()
        {
            //UPDATE BILLCHAIN
            billchaindtl bchain = new billchaindtl();
            bchain = billchaindtl.Getbillchain(vm.BILLCHAIN.SECTION, vm.BILLCHAIN.GROUPCODE);

            bool newrec = (bchain == null) ? true : false;

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "Billchain_Add" : "Billchain_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", vm.BILLCHAIN.PATIENTNO.Trim());
            insertCommand.Parameters.AddWithValue("@groupcode", vm.BILLCHAIN.GROUPCODE.Trim());
            insertCommand.Parameters.AddWithValue("@status", (newrec) ? "A" : bchain.STATUS);
            insertCommand.Parameters.AddWithValue("@Birthdate", vm.BILLCHAIN.BIRTHDATE);
            insertCommand.Parameters.AddWithValue("@Sex", vm.BILLCHAIN.SEX);
            insertCommand.Parameters.AddWithValue("@M_status", vm.BILLCHAIN.STATUS);
            insertCommand.Parameters.AddWithValue("@Reg_date", vm.BILLCHAIN.REG_DATE);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", vm.BILLCHAIN.GHGROUPCODE);
            insertCommand.Parameters.AddWithValue("@phone", billchain.PHONE);
            insertCommand.Parameters.AddWithValue("@name", vm.BILLCHAIN.NAME.Trim());
            insertCommand.Parameters.AddWithValue("@grouphead", vm.BILLCHAIN.MEDNOTES);
            insertCommand.Parameters.AddWithValue("@grouphtype", "P");
            insertCommand.Parameters.AddWithValue("@patcateg", vm.BILLCHAIN.BILLONACCT);
            insertCommand.Parameters.AddWithValue("@hmoservtype", "");
            insertCommand.Parameters.AddWithValue("@currency", "");
            insertCommand.Parameters.AddWithValue("@residence", vm.BILLCHAIN.RESIDENCE + " " + vm.BILLCHAIN.STAFFNO);
            insertCommand.Parameters.AddWithValue("@clinic", "");
            insertCommand.Parameters.AddWithValue("@email", billchain.EMAIL);
            insertCommand.Parameters.AddWithValue("@operator", woperator);
            insertCommand.Parameters.AddWithValue("@dtime", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@hmocode", "");
            insertCommand.Parameters.AddWithValue("@expirydate", DateTime.Now.Date);

            insertCommand.Parameters.AddWithValue("@posted", false);
            insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@staffno", " ");
            insertCommand.Parameters.AddWithValue("@relationsh", " ");
            insertCommand.Parameters.AddWithValue("@section", " ");
            insertCommand.Parameters.AddWithValue("@department", " ");
            insertCommand.Parameters.AddWithValue("@cur_db", 0m);
            insertCommand.Parameters.AddWithValue("@cumvisits", 0m);
            insertCommand.Parameters.AddWithValue("@billonacct", "");
            insertCommand.Parameters.AddWithValue("@astnotes", false);
            insertCommand.Parameters.AddWithValue("@piclocation", ""); //billchain.PICLOCATION
            insertCommand.Parameters.AddWithValue("@surname", vm.BILLCHAIN.SURNAME);
            insertCommand.Parameters.AddWithValue("@othernames", vm.BILLCHAIN.OTHERNAMES);
            insertCommand.Parameters.AddWithValue("@title", vm.BILLCHAIN.TITLE);
            insertCommand.Parameters.AddWithValue("@SPNOTES", billchain.SPNOTES);
            insertCommand.Parameters.AddWithValue("@MEDNOTES", billchain.MEDNOTES);
            insertCommand.Parameters.AddWithValue("@MEDHISTORYCHAINED", billchain.MEDHISTORYCHAINED);
            insertCommand.Parameters.AddWithValue("PATIENTNO_PRINCIPAL", "");

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // throw ex;
                vm.REPORTS.alertMessage = "SQL access" + ex; 
                return false;
            }
            finally
            {
                connection.Close();
            }

            vm.REPORTS.ActRslt = "Private Account Created!";

            return true;
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void txtothername_LostFocus(object sender, EventArgs e)
        //{
        //    string xtitle = (string.IsNullOrWhiteSpace(comtitle.Text)) ? "" : comtitle.Text.Trim();
        //    TXTPATIENTNAME.Text = txtsurname.Text.Trim() + ", " + txtothername.Text.Trim() + " (" + xtitle + ")";
        //}
    }
}