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
using GLS.DataAcess;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class TransactionsPayments
    {
        // PAYDETAIL paydetail = new PAYDETAIL();
        DataTable paydetail;
        Customer customers = new Customer();
        patientinfo patients = new patientinfo();
        Mrattend mrattend = new Mrattend();
        // PleaseWaitForm pleaseWait = new PleaseWaitForm();
        Billings billv = new Billings();
        // DataTable suspense;
        Admrecs admrec = new Admrecs();
        DataTable servicetagged, atmprofile = ATMPROFILE.GetATMPROFILE(), gltab3 = Dataaccess.GetAnytable("", "MR", "select * from glintab3", false), glupdate = Dataaccess.GetAnytable("", "MR", "select * from glint", false), acc05, acc05a, acc08;
        LINK1 link1 = new LINK1();

        bool isadmissions, newrec, disallowbackdate, mpayauto, ismeddiag, newcashier, isgl, isotherserviceIncome, mcanadd, mcandelete, mcanalter;
        string mfacility, anycode, anycode1, mcurrency, msection, start_time, mgroupcode, mpatientno, mname, mcusttype, mgrouphead, mghgroupcode, mgldocument, mglcompany, mimmunizationclinic, nhisdrugcode, mbr_cc, woperator, lookupsource, mactiveglint, atmcompany, atmdocument, atmExpense_Acct, atmdebit, atmcredit, miscpatientAcct, mreference, admdepositflag, NHISAcct, mservicetype;
        decimal mlastno, mglbatchno, atmbatchno, atmpos_charge;
        //debitacct, creditacct, 
        DataTable dtcontrol;
        int mrecid;
        DateTime dtmin_date = msmrfunc.mrGlobals.dtmin_date; // (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                                                             //DateTime mtransdate = DateTime.Now.Date;
                                                             //to track and return admissions deposit flag - 01/08/2012

        MR_DATA.MR_DATAvm vm;

        public TransactionsPayments(string woperato, MR_DATA.MR_DATAvm VM2)
        {
            //InitializeComponent();
            //isadmissions = servicetype == "Admissions" ? true : false;
            //isotherserviceIncome = servicetype == "OtherServiceIncome" ? true : false;

            //if (isadmissions)
            //{
            //    this.Text = "ADMISSIONS DEPOSIT/PAYMENT RECORDS";
            //    lblreference.Text = "Admission Ref.";
            //    toolTip1.SetToolTip(combConsAdmNo, "Admissions Number or Reference must be specified...");
            //    chkCurrntAdmSu.Visible = true;
            //}
            //else if (isotherserviceIncome)
            //{
            //    this.Text = "OTHER SERVICES INCOME/RECEIVABLES";
            //    txtName.ReadOnly = false;
            //    combGpCd.Text = "PVT";
            //    combClientCd.Text = miscpatientAcct;
            //    txtName.TabStop = true;

            //    pan_MiscPaymentIncome.Visible = true;
            //}
            //else
            //    chkTodaysConsult.Visible = true;
            ////  woperator = Session["operator"].ToString();  // msmrfunc.mrGlobals.WOPERATOR;

            vm = VM2;

            ismeddiag = bissclass.sysGlobals.ismeddiag;
            start_time = DateTime.Now.ToString("HH:mm:ss");
            mgroupcode = vm.REPORTS.mgroupcode;
            mpatientno = vm.REPORTS.mpatientno;
            mname = vm.REPORTS.txtothername;

            mcurrency = msmrfunc.mrGlobals.localcur;
            isgl = bissclass.sysGlobals.isgl;
            msection = "2";
            woperator = woperato;
            isadmissions = vm.REPORTS.chkIncludePayments;
            isotherserviceIncome = vm.REPORTS.chkReportbyAgent;
            vm.REPORTS.transDate = Convert.ToDateTime(vm.REPORTS.txtTimeTo);
            newrec = vm.REPORTS.newrec;

            getcontrolsettings();
        }

        //private void TransactionsPayments_Load(object sender, EventArgs e)
        //{
        //    ismeddiag = bissclass.sysGlobals.ismeddiag;
        //    getcontrolsettings();
        //    //   getcashierbalance();
        //    initcomboboxes();

        //    if (isotherserviceIncome && string.IsNullOrWhiteSpace(miscpatientAcct))
        //    {
        //        DialogResult result = MessageBox.Show("Misc. Transactions/Patient Account Code Not Defined in Systems Setup", "Systems Setup Error...");
        //        btnExit.PerformClick();
        //        return;
        //    }

        //    if (string.IsNullOrWhiteSpace(NHISAcct))
        //    {
        //        DialogResult result = MessageBox.Show("NHIS Group Payments Account Code Not Defined in Systems Setup", "Systems Setup Error...");
        //        btnExit.PerformClick();
        //        return;
        //    }
        //}

        private void getcontrolsettings()
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select pauto, activeglint, glbatchno, gldocument, glcompany, cashpoint, attendlink, dischtime, name, DTFORMAT from mrcontrol order by recid", false);

            mpayauto = (bool)dt.Rows[0]["pauto"];
            mactiveglint = dt.Rows[0]["activeglint"].ToString();
            mglbatchno = (Decimal)dt.Rows[0]["glbatchno"];
            mgldocument = dt.Rows[0]["gldocument"].ToString();
            mglcompany = dt.Rows[0]["glcompany"].ToString();

            disallowbackdate = (bool)dt.Rows[2]["cashpoint"];
            NHISAcct = dt.Rows[2]["DTFORMAT"].ToString();

            miscpatientAcct = dt.Rows[6]["name"].ToString().Substring(0, 9);

            mimmunizationclinic = dt.Rows[7]["name"].ToString().Substring(0, 5);

            nhisdrugcode = dt.Rows[8]["dischtime"].ToString().Substring(0, 7);

            mbr_cc = bissclass.sysGlobals.mbr_cc;
            dt = Dataaccess.GetAnytable("", "MR", "select CANDELETE, CANALTER, CANADD from mrstlev where operator = '" + woperator + "'", false);

            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];

        }

        //private void initcomboboxes()
        //{
        //    //get bank
        //    combbank.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM ATMPROFILE order by name", true);
        //    combbank.ValueMember = "Name";
        //    combbank.DisplayMember = "Name";

        //    if (isotherserviceIncome)
        //    {
        //        cboFacility.DataSource = Dataaccess.GetAnytable("", "CODES", "select type_code, name from ServiceCentreCodes order by name", true);
        //        cboFacility.ValueMember = "type_code";
        //        cboFacility.DisplayMember = "name";
        //    }
        //}

        //void combostyleset(ComboBoxStyle xval)
        //{
        //    // xval = "Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown";
        //    combbank.DropDownStyle = xval;
        //    combPmtMode.DropDownStyle = xval;
        //    return;
        //}

        //void getcashierbalance()
        //{
        //    link1 = LINK1.GetLINK1(woperator, DateTime.Now.Date, true);

        //    if (link1 == null)
        //        newcashier = true;
        //    else
        //    {
        //        txtdebit.Text = link1.DEBIT.ToString("N2");
        //        txtitemcount.Text = link1.ITEMNO.ToString("N0");

        //    }

        //    /*          SqlConnection cs = new SqlConnection(); cs = msmrDB.mrConnection();
        //              string selcommand = "SELECT debit,itemno FROM LINK1 WHERE OPERATOR = '" + msmrfunc.mrGlobals.WOPERATOR.Trim() +
        //              "' AND trans_date = '" + DateTime.Now.Date + "'";
        //              SqlCommand selectCommand = new SqlCommand(selcommand, cs);
        //              try
        //              {
        //                  cs.Open();
        //                  SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
        //                  if (reader.Read())
        //                  {
        //                      txtdebit.Text = Convert.ToDecimal(reader["debit"]).ToString("N2");
        //                      txtitemcount.Text = reader["itemno"].ToString();
        //                  }

        //                  reader.Close();
        //              }
        //              catch (SqlException ex)
        //              {
        //                  MessageBox.Show("" + ex, msgBoxHandler);
        //              }
        //              finally
        //              {
        //                  cs.Close();
        //              }*/
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;

        //    if (btn.Name == "btnAttendance")
        //    {
        //        string xheader = (isadmissions) ? "ADMISSION RECORDS" : "RECORDED DAILY ATTENDANCE";
        //        combConsAdmNo.Text = "";
        //        lookupsource = (isadmissions) ? "A" : "I";
        //        msmrfunc.mrGlobals.crequired = (isadmissions) ? "A" : "I";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR " + xheader;
        //        if (lookupsource == "A")
        //            msmrfunc.mrGlobals.lookupCriteria = chkCurrntAdmSu.Checked ? "C" : "";
        //        else
        //            msmrfunc.mrGlobals.lookupCriteria = chkTodaysConsult.Checked ? "C" : "";
        //    }
        //    else if (btn.Name == "btnghgroupcode")
        //    {
        //        combGpCd.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "p";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS/FAMILY GROUPHEADS";
        //    }
        //    else if (btn.Name == "btngrouphead")
        //    {
        //        combClientCd.Text = "";
        //        lookupsource = "P";
        //        msmrfunc.mrGlobals.crequired = (string.IsNullOrWhiteSpace(combGpCd.Text)) ? "C" : "L";
        //        msmrfunc.mrGlobals.frmcaption = (string.IsNullOrWhiteSpace(combGpCd.Text)) ?
        //        "LOOKUP FOR REGISTERED CORPORATE CLIENTS" : "LOOKUP FOR REGISTERED PATIENTS/FAMILY GROUPHEADS";
        //    }
        //    else if (btn.Name == "btnpayreference")
        //    {
        //        combPmtNo.Text = "";
        //        lookupsource = "PAY";
        //        msmrfunc.mrGlobals.crequired = "PAY";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR SAVED PAYMENT RECORDS";
        //    }
        //    else if (btn.Name == "btnbillreference")
        //    {
        //        combPmtNo.Text = "";
        //        lookupsource = "B";
        //        msmrfunc.mrGlobals.crequired = "B";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR SAVED BILLS";
        //    }
        //    //else
        //    //{
        //    //    string xheader = (isadmissions) ? "ADMISSION RECORDS" : "RECORDED DAILY ATTENDANCE";
        //    //    combConsAdmNo.Text = "";
        //    //    lookupsource = (isadmissions) ? "A" : "I";
        //    //    msmrfunc.mrGlobals.crequired = (isadmissions) ? "A" : "I";
        //    //    msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR " + xheader;
        //    //    msmrfunc.mrGlobals.lookupCriteria = chkCurrntAdmSu.Checked ? "C" : "";
        //    //}
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "g") //ghgroupcodee
        //    {
        //        combGpCd.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        combClientCd.Text = anycode1 = msmrfunc.mrGlobals.anycode1;
        //        combGpCd.Focus();
        //    }

        //    else if (lookupsource == "P") //patientno grouphead
        //    {
        //        combClientCd.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        combClientCd.Focus();
        //    }
        //    else if (lookupsource == "B") //BILLS
        //    {
        //        combBillRef.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        combBillRef.Focus();
        //    }
        //    else if (lookupsource == "PAY")
        //    {
        //        combPmtNo.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        combPmtNo.Focus();
        //    }
        //    else //daiy attendance
        //    {
        //        msmrfunc.mrGlobals.lookupCriteria = "";
        //        combConsAdmNo.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        combConsAdmNo.Focus();
        //    }
        //}

        //private void btnReload_Click(object sender, EventArgs e)
        //{
        //    //combConsAdmNo.Focus();
        //    //return;
        //    anycode = msmrfunc.mrGlobals.anycode = mfacility = msmrfunc.mrGlobals.mfacility = admdepositflag = msmrfunc.mrGlobals.admflag = "";
        //    mrecid = msmrfunc.mrGlobals.recid = 0;
        //    /* frmlinkinfo FrmLinkinfo = new frmlinkinfo(combConsAdmNo.Text, 0, 0m, 0m, "", true, msection, 2, "", "");
        //     FrmLinkinfo.Closed += new EventHandler(FrmLinkinfo_Closed);
        //     FrmLinkinfo.ShowDialog();*/

        //    //DataTable dt = msmrfunc.getLinkDetails(combConsAdmNo.Text, 0, 0m, 0m, "", true, msection, 2, "", "");
        //    DataTable dt = msmrfunc.getLinkDetails(combConsAdmNo.Text, 0, 0m, 0m, "", true, "12", 2, "", "");
        //    if (dt.Rows.Count > 0)
        //    {
        //        frmGetlinkinfo linkinfo = new frmGetlinkinfo(dt);
        //        linkinfo.ShowDialog();
        //        combConsAdmNo.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        admdepositflag = msmrfunc.mrGlobals.admflag;
        //        mrecid = msmrfunc.mrGlobals.recid;
        //    }
        //    combConsAdmNo.Focus();
        //}

        //void FrmLinkinfo_Closed(object sender, EventArgs e)
        //{
        //    frmlinkinfo FrmLinkinfo_Closed = sender as frmlinkinfo;
        //    {
        //        combConsAdmNo.Text = anycode = msmrfunc.mrGlobals.anycode;
        //        admdepositflag = msmrfunc.mrGlobals.admflag;
        //        mrecid = msmrfunc.mrGlobals.recid;
        //        combConsAdmNo.Focus();
        //    }

        //}

        //private void combConsAdmNo_Click(object sender, EventArgs e)
        //{
        //    combConsAdmNo.Focus();
        //}

        //private void combConsAdmNo_Enter(object sender, EventArgs e)
        //{
        //    if (isotherserviceIncome && !string.IsNullOrWhiteSpace(txtName.Text) && cboFacility.SelectedValue != null)
        //    {
        //        if (string.IsNullOrWhiteSpace(txtName.Text))
        //            txtName.Focus();
        //        else
        //            nmrAmount.Focus();
        //        return;
        //    }

        //    ClearControls("R");

        //    if (string.IsNullOrWhiteSpace(anycode) && !isadmissions &&
        //            new string[] { "1", "2", "3", "9" }.Contains(msection)) //no lookup and msection $ "1293" && 9-PAEDIATRICS CAN COLLECT MONEY:REG./CA/PAED/ND
        //    {
        //        //get list of patients for payment
        //        //  btnReload.Select();
        //        //frmlinkinfo FrmLinkinfo = new frmlinkinfo(combConsAdmNo.Text,0,0m,0m,"",true,msection,3,"","");
        //        //FrmLinkinfo.Closed += new EventHandler(FrmLinkinfo_Closed);
        //        //FrmLinkinfo.ShowDialog();
        //        // btnReload.PerformClick();
        //    }
        //}

        //private void combConsAdmNo_LostFocus(object sender, EventArgs e)
        //{
        //    combGpCd.Enabled = combClientCd.Enabled = txtName.Enabled = combBillRef.Enabled = true;

        //    if (string.IsNullOrWhiteSpace(combConsAdmNo.Text))
        //    {
        //        //check sections and allows  msection $ "1293" && 9-PAEDIATRICS CAN COLLECT MONEY:REG./CA/PAED/ND
        //        if (new string[] { "1", "2", "3", "9", "C" }.Contains(msection)) //msection $ "1293"
        //        {
        //        }//ok
        //        else
        //        {
        //            //msgeventtracker = "EXIT";
        //            DialogResult result = MessageBox.Show("No Access to further details here...TKS", "CASH OFFICE PLATFORM");
        //            btnExit.PerformClick();
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        combGpCd.Enabled = combClientCd.Enabled = txtName.Enabled = combBillRef.Enabled = false;
        //        // msgeventtracker = "REF";
        //        anycode = anycode1 = "";
        //        start_time = DateTime.Now.ToString("HH:mm:ss");
        //        string xtranstype = "";
        //        bool xfoundit = true;
        //        //check if in attendance records
        //        if (string.IsNullOrWhiteSpace(anycode) && !string.IsNullOrWhiteSpace(combConsAdmNo.Text) &&
        //            (Char.IsDigit(combConsAdmNo.Text[0])))  //no lookup value obtained
        //        {
        //            combConsAdmNo.Text = bissclass.autonumconfig(combConsAdmNo.Text, true, "C", "999999999");
        //        }
        //        xtranstype = combConsAdmNo.Text.Substring(0, 1);
        //        if (xtranstype == "A") //admissions
        //        {
        //            admrec = Admrecs.GetADMRECS(combConsAdmNo.Text);
        //            if (admrec == null)
        //            {
        //                DialogResult result = MessageBox.Show("Invalid Admissions Reference... ", "IN-PATIENT PAYMENT");
        //                combConsAdmNo.Text = "";
        //                combConsAdmNo.Select();
        //                return;
        //            }
        //        }
        //        else //if (xtranstype == "C" || xtranstype == "S" || xtranstype == "I") //CONSULT/SP.SERVICE/IMMUNIZATN
        //        {
        //            mrattend = Mrattend.GetMrattend(combConsAdmNo.Text);
        //            if (mrattend == null)
        //            {
        //                DialogResult result = MessageBox.Show("Unable to Link this Consultation Reference in Daily Attendance Register... ");
        //                combConsAdmNo.Text = "";
        //                combConsAdmNo.Select();
        //                return;
        //            }
        //        }
        //        //}
        //        //else
        //        //{
        //        //    DialogResult result = MessageBox.Show("Invalid Number Format for Consultation/Admission Reference...");
        //        //    combConsAdmNo.Text = "";
        //        //    combConsAdmNo.Select();
        //        //    return;
        //        //}

        //        combGpCd.Text = (xtranstype == "A") ? admrec.GHGROUPCODE : mrattend.GHGROUPCODE;
        //        combClientCd.Text = (xtranstype == "A") ? admrec.GROUPHEAD : mrattend.GROUPHEAD;
        //        txtName.Text = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;
        //        dttrans_date.Value = (xtranstype == "A") ? admrec.ADM_DATE : mrattend.TRANS_DATE;
        //        mcusttype = (xtranstype == "A") ? admrec.GROUPHTYPE : mrattend.GROUPHTYPE;
        //        mgroupcode = (xtranstype == "A") ? admrec.GROUPCODE : mrattend.GROUPCODE;
        //        mpatientno = (xtranstype == "A") ? admrec.PATIENTNO : mrattend.PATIENTNO;
        //        mname = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;
        //        if (mcusttype == "P")
        //        {
        //            patients = patientinfo.GetPatient(combClientCd.Text, combGpCd.Text);
        //            if (patients == null)
        //                xfoundit = false;
        //        }
        //        else
        //        {
        //            customers = Customer.GetCustomer(combClientCd.Text);
        //            if (customers == null)
        //                xfoundit = false;
        //        }
        //        if (!xfoundit)
        //        {
        //            DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS");
        //            combConsAdmNo.Text = "";
        //            combConsAdmNo.Select();
        //            return;
        //        }
        //        txtName.Text = (string.IsNullOrWhiteSpace(mpatientno) || combConsAdmNo.Text.Substring(0, 1) == "S" || mgroupcode == "NHIS") ? txtName.Text : (mcusttype == "P") ? patients.name : customers.Name;
        //        mgrouphead = (mcusttype == "P") ? patients.patientno : customers.Custno;
        //        mghgroupcode = (mcusttype == "P") ? patients.groupcode : "";
        //        if (mcusttype == "P" && !string.IsNullOrWhiteSpace(patients.piclocation))
        //        {
        //            pictureBox1.Visible = true;
        //            pictureBox1.Image = WebGUIGatway.getpicture(patients.piclocation);
        //        }
        //        else if (mgroupcode.Trim() == "NHIS") //get piicture from billchian
        //        {
        //            DataTable dt = Dataaccess.GetAnytable("", "MR", "select piclocation from billchain where groupcode = '" + mrattend.GROUPCODE + "' and patientno = '" + mrattend.PATIENTNO + "'", false);
        //            if (dt.Rows.Count > 0)
        //            {
        //                pictureBox1.Visible = true;
        //                pictureBox1.Image = WebGUIGatway.getpicture(dt.Rows[0]["piclocation"].ToString());
        //            }
        //        }
        //        nmrPayable.Value = nmrAmount.Value = 0m;
        //        if (!isadmissions)
        //        {
        //            //check for investigations request in suspense and get details to acctfromsusp form for tagging
        //            servicetagged = SUSPENSE.GetSUSPENSE(combConsAdmNo.Text, "U");
        //            if (servicetagged.Rows.Count > 0)
        //            {
        //                frmAcctfromSusp FrmacctFromsusp = new frmAcctfromSusp(servicetagged, combConsAdmNo.Text.Trim() + "  : " + mname.Trim() + " : " + combGpCd.Text.Trim() + "-" + combClientCd.Text.Trim());
        //                FrmacctFromsusp.Closed += new EventHandler(FrmacctFromsusp_Closed);
        //                FrmacctFromsusp.ShowDialog();

        //            }

        //            if (chkbalbf.Checked && mcusttype == "P" && !string.IsNullOrWhiteSpace(mpatientno))
        //            {
        //                calc_op_bal();
        //            }
        //            get_currenttrans();
        //        }
        //        //  nmrPayable.Value += nmrBalanceBF.Value;
        //        if (admdepositflag == "A")
        //        {
        //            decimal xm = nmrAmount.Value;
        //            lbladmissiondeposit.Visible = nmrAdmissionDeposit.Visible = true;
        //            nmrAdmissionDeposit.Value = Convert.ToDecimal(bissclass.seeksay("select cumbil from link where recid = '" + mrecid + "'", "MR", "cumbil"));
        //            nmrAmount.Value += nmrAdmissionDeposit.Value;
        //        }
        //        combPmtNo.Focus();
        //    }
        //    anycode = anycode1 = "";
        //}

        //void FrmacctFromsusp_Closed(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    //we need to get datable back from acct from suspense
        //    decimal xamt = 0m;
        //    for (int i = 0; i < servicetagged.Rows.Count; i++)
        //    {
        //        if (msmrfunc.mrGlobals.taggedFromSuspensea_[i] == "YES") //tagged
        //        {
        //            xamt += Convert.ToDecimal(servicetagged.Rows[i]["amount"]);
        //        }
        //    }
        //    nmrPayable.Value += xamt;
        //    vm.REPORTS.txtdiscount = nmrPayable.Value;  //for amount
        //}

        /*		&&GET PATIENT PHOTO
    IF mcusttype="P"
        select 0
        USE PATPIC.DAT SHARED
        set order to tag patientno
        if seek( THISFORM.txtgroupcode.value+thisform.txtgrouphead.value) .and. !empty(photoloc)
            ThisForm.Image1.PICTURE = photoloc
            ThisForm.Image1.Visible = .t.
        ELSE
            ThisForm.Image1.PICTURE = ''
        endif
        use	
    ELSE
        ThisForm.Image1.Visible = .f.
    ENDIF*/

        //void get_currenttrans()
        //{
        //    listviewHeader.Visible = true;
        //    listView1.Visible = true;
        //    // listviewHeaderDetails();
        //    string[] arr = new string[5];
        //    ListViewItem itm;
        //    // nmrPayable.Value = 0m;
        //    decimal mcurdb = 0m, mcurcr = 0m;
        //    int itemcount = 0;
        //    listView1.Items.Clear();
        //    DataTable dt = Billings.GetBILLINGdetails(mgrouphead, mname, mpatientno, (mgrouphead.Trim() == "MISC") ? "N" :
        //        (!string.IsNullOrWhiteSpace(mpatientno)) ? "P" : "G", dttrans_date.Value.Date, dttrans_date.Value.Date);
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            itemcount++;
        //            arr[1] = arr[2] = "";
        //            if (dt.Rows[i]["ttype"].ToString() == "D")
        //                mcurdb += Convert.ToDecimal(dt.Rows[i]["amount"]);
        //            else
        //            {
        //                mcurcr += Convert.ToDecimal(dt.Rows[i]["amount"]);
        //            }
        //            arr[0] = itemcount.ToString();
        //            arr[1] = dt.Rows[i]["DESCRIPTION"].ToString();
        //            if (dt.Rows[i]["TTYPE"].ToString() == "D")
        //                arr[2] = dt.Rows[i]["AMOUNT"].ToString();
        //            else
        //            { arr[3] = dt.Rows[i]["AMOUNT"].ToString(); }
        //            arr[4] = (mcurdb - mcurcr).ToString("N2");
        //            itm = new ListViewItem(arr);
        //            listView1.Items.Add(itm);
        //        }
        //    }
        //    DataTable dtp = PAYDETAIL.GetPAYMENTdetails(mgrouphead, mname, mpatientno, (mgrouphead.Trim() == "MISC") ? "N" :
        //        (!string.IsNullOrWhiteSpace(mpatientno)) ? "P" : "G", dttrans_date.Value.Date, dttrans_date.Value.Date);
        //    if (dtp.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtp.Rows.Count; i++)
        //        {
        //            itemcount++;
        //            arr[1] = arr[2] = "";
        //            arr[0] = itemcount.ToString();
        //            if (dtp.Rows[i]["ttype"].ToString() == "D")
        //                mcurdb += Convert.ToDecimal(dtp.Rows[i]["amount"]);
        //            else
        //            {
        //                mcurcr += Convert.ToDecimal(dtp.Rows[i]["amount"]);
        //            }
        //            arr[1] = dtp.Rows[i]["DESCRIPTION"].ToString();
        //            if (dtp.Rows[i]["TTYPE"].ToString() == "D")
        //                arr[2] = dtp.Rows[i]["AMOUNT"].ToString();
        //            else
        //            { arr[3] = dtp.Rows[i]["AMOUNT"].ToString(); }
        //            arr[4] = (mcurdb - mcurcr).ToString("N2");
        //            itm = new ListViewItem(arr);
        //            listView1.Items.Add(itm);
        //        }
        //    }
        //    //adjust
        //    DataTable dta = BILL_ADJ.GetAdjustdetails(mgrouphead, mname, mpatientno, (mgrouphead.Trim() == "MISC") ? "N" :
        //        (!string.IsNullOrWhiteSpace(mpatientno)) ? "P" : "G", dttrans_date.Value.Date, dttrans_date.Value.Date);
        //    if (dta.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dta.Rows.Count; i++)
        //        {
        //            itemcount++;
        //            if (dta.Rows[i]["type"].ToString() == "D")
        //                mcurdb += Convert.ToDecimal(dta.Rows[i]["amount"]);
        //            else
        //            {
        //                mcurcr += Convert.ToDecimal(dta.Rows[i]["amount"]);
        //            }
        //            arr[0] = itemcount.ToString();
        //            arr[1] = dta.Rows[i]["ADJUST"].ToString().Trim() + "; " + dta.Rows[i]["COMMENTS"].ToString().Trim();
        //            if (dta.Rows[i]["TYPE"].ToString() == "D")
        //                arr[2] = dta.Rows[i]["AMOUNT"].ToString();
        //            else
        //            {
        //                arr[3] = dta.Rows[i]["AMOUNT"].ToString();
        //            }

        //            arr[4] = (mcurdb - mcurcr).ToString("N2");
        //            itm = new ListViewItem(arr);
        //            listView1.Items.Add(itm);
        //        }
        //    }
        //    //txtpayable.Text = (mcurdb - mcurcr).ToString("N2");
        //    nmrCurrentCredit.Value = mcurcr;
        //    nmrCurrentDebit.Value = mcurdb;
        //    nmrPayable.Value = (nmrBalanceBF.Value + nmrCurrentDebit.Value) - nmrCurrentCredit.Value;
        //    nmrAmount.Value = nmrPayable.Value < 1 ? 0 : nmrPayable.Value;
        //}


        //private void combPmtNo_Enter(object sender, EventArgs e)
        //{
        //    combPmtNo.Focus();
        //    panPVTDeposit.Visible = false;
        //    if (string.IsNullOrWhiteSpace(anycode) && mpayauto) //no lookup
        //    {
        //        dtcontrol = Dataaccess.GetAnytable("", "MR", "SELECT payno from mrcontrol where recid = '4'", false);
        //        mlastno = (decimal)dtcontrol.Rows[0]["payno"] + 1; // msmrfunc.getcontrol_lastnumber("PAYNO", 3, false, mlastno, false) + 1;
        //        combPmtNo.Text = mlastno.ToString();
        //    }

        //    if (string.IsNullOrWhiteSpace(combConsAdmNo.Text))
        //    {
        //        combGpCd.Text = combClientCd.Text = txtName.Text = mcusttype = mfacility = combBillRef.Text = "";
        //        nmrCurrentCredit.Value = nmrCurrentDebit.Value = nmrPayable.Value = nmrBalanceBF.Value = 0m;
        //    }
        //    btnDelete.Enabled = btnSave.Enabled = pancapitations.Visible = false;
        //    start_time = DateTime.Now.ToString("HH:mm:ss");
        //    dtValueDate.Value = dtRecvdDate.Value = DateTime.Now.Date;
        //}

        //private void combPmtNo_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(combPmtNo.Text))
        //    {
        //        combConsAdmNo.Focus();
        //        return;
        //    }

        //    // msgeventtracker = "PREFF";
        //    anycode = anycode1 = "";
        //    if (mpayauto && bissclass.IsDigitsOnly(combPmtNo.Text.Trim()) && Convert.ToInt32(combPmtNo.Text) > mlastno)
        //    {
        //        DialogResult result = MessageBox.Show("Payment Reference is out of Seguence...");
        //        combPmtNo.Text = "";
        //        combPmtNo.Select();
        //        return;
        //    }
        //    if (mpayauto && bissclass.IsDigitsOnly(combPmtNo.Text.Trim()))
        //    {
        //        combPmtNo.Text = bissclass.autonumconfig(combPmtNo.Text, true, (string.IsNullOrWhiteSpace(mbr_cc)) ? "" : mbr_cc, "999999999");
        //    }
        //    mreference = combPmtNo.Text;
        //    anycode = anycode1 = "";
        //    newrec = true;
        //    paydetail = PAYDETAIL.GetPAYDETAIL(combGpCd.Text, mpatientno, true, combPmtNo.Text);
        //    if (paydetail.Rows.Count > 0)
        //    {
        //        newrec = false;
        //        nmrAmount.Value = (decimal)paydetail.Rows[0]["amount"];
        //        txtName.Text = paydetail.Rows[0]["name"].ToString();
        //        combPmtMode.Text = paydetail.Rows[0]["paytype"].ToString();
        //        txtDetails.Text = paydetail.Rows[0]["DESCRIPTION"].ToString();
        //        dtValueDate.Value = (DateTime)paydetail.Rows[0]["TRANS_DATE"];
        //        dtRecvdDate.Value = (DateTime)paydetail.Rows[0]["DATERECEIVED"];
        //        mreference = paydetail.Rows[0]["reference"].ToString();
        //        //  msgeventtracker = "REFF";
        //        if (msection == "7" || msection == "C") //billing
        //        {
        //            //ok
        //        }
        //        else
        //        {
        //            DialogResult result = MessageBox.Show("Record Exist...and Further access Denied !", "PAYMENT INFORMATION");
        //            combPmtNo.Text = "";
        //            // combPmtNo.Select();
        //            return;

        //        }
        //        if ((bool)paydetail.Rows[0]["posted"])
        //        {
        //            DialogResult result = MessageBox.Show("Record Exist ... AND IT CANNOT BE AMENED !", "PAYMENT RECORD");
        //            combPmtNo.Text = "";
        //            // combPmtNo.Select();
        //            return;
        //        }
        //        if (msection == "C" && paydetail.Rows[0]["operator"].ToString().Trim() != woperator)
        //        {
        //            DialogResult result = MessageBox.Show("Further Access Denied...CURRENT USER AND RECORD SIGNATURE CONFLICT!!!", "PAYMENT RECORDS");
        //            combPmtNo.Text = "";
        //            // combPmtNo.Select();
        //            return;
        //        }
        //        btnDelete.Enabled = mcanalter ? true : false;
        //        //  msmrfunc.mrGlobals.waitwindowtext = "Editing Old Payment Record...!!!";
        //        //  pleaseWait.Show();
        //    }
        //    //if (isotherserviceIncome)
        //    //{
        //    //    combGpCd.Text = "PVT      ";
        //    //    combClientCd.Text = miscpatientAcct;
        //    //}
        //    if (string.IsNullOrWhiteSpace(combConsAdmNo.Text))
        //        combGpCd.Focus();
        //    else
        //        nmrAmount.Focus();
        //}

        //private void combClientCd_Leave(object sender, EventArgs e)
        //{
        //    // msgeventtracker = "GH";
        //    if (string.IsNullOrWhiteSpace(combClientCd.Text))
        //    {
        //        if (isotherserviceIncome)
        //            cboFacility.Select();
        //        return;
        //    }

        //    anycode = anycode1 = "";
        //    // Customer customer = new Customer();
        //    patientinfo patgrphead = new patientinfo();
        //    if (!string.IsNullOrWhiteSpace(combGpCd.Text))
        //    {
        //        patgrphead = patientinfo.GetPatient(combClientCd.Text, combGpCd.Text);
        //        mcusttype = "P";
        //    }
        //    else
        //    {
        //        customers = Customer.GetCustomer(combClientCd.Text);
        //        mcusttype = "C";
        //    }
        //    if (mcusttype == "P" && patgrphead == null || mcusttype == "C" && customers == null)
        //    {
        //        DialogResult result = MessageBox.Show("Invalid GroupHead Specification as responsible for Bills");
        //        combClientCd.Text = "";
        //        combClientCd.Select();
        //        return;
        //    }
        //    // this.DisplayPatients();
        //    txtName.Text = (mcusttype == "P") ? patgrphead.name : customers.Name;
        //    mgrouphead = (mcusttype == "P") ? patients.patientno : customers.Custno;
        //    if (mcusttype == "P" && !patgrphead.isgrouphead)
        //    {
        //        DialogResult result = MessageBox.Show("Specified Patient is not a registered GroupHead...");
        //        combClientCd.Text = "";
        //        combClientCd.Select();
        //        return;
        //    }
        //    mgroupcode = mpatientno = "";
        //    if (mcusttype == "P")
        //    {
        //        if (chkbalbf.Checked) // 12-12-2013
        //            calc_op_bal();
        //        mgroupcode = patgrphead.groupcode;
        //        mpatientno = patgrphead.patientno;
        //        panPVTDeposit.Visible = true;
        //        if (!string.IsNullOrWhiteSpace(patgrphead.piclocation))
        //        {
        //            pictureBox1.Visible = true;
        //            pictureBox1.Image = WebGUIGatway.getpicture(patgrphead.piclocation);
        //        }
        //    }
        //    else
        //    {
        //        pancapitations.Visible = true;
        //        pictureBox1.Visible = false;
        //    }
        //    chknone.Checked = true;
        //}

        //void calc_op_bal()
        //{
        //    decimal db, cr, adj, bal; db = cr = adj = bal = 0m;
        //    bal = msmrfunc.getOpeningBalance(combGpCd.Text, combClientCd.Text, "", string.IsNullOrWhiteSpace(combGpCd.Text) ? "C" : "P", DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);
        //    //  decimal xamt = Billings.GetBILLINGOpBal(mgrouphead, mname, mpatientno, (mgrouphead == "MISC") ? "N" :
        //    //    (!string.IsNullOrWhiteSpace(mpatientno)) ? "P" : "G", DateTime.Now.Date,true);
        //    db = cr = adj = 0m;
        //    decimal xamt = msmrfunc.getTransactionDbCrAdjSummary(combGpCd.Text, combClientCd.Text, "", string.IsNullOrWhiteSpace(combGpCd.Text) ? "C" : "P", DateTime.Now.Date, DateTime.Now.Date, ref db, ref cr, ref adj);
        //    nmrBalanceBF.Value = Math.Abs(bal);
        //    lblbalbfdbcr.Text = (bal < 1) ? "CR" : "DB";
        //    nmrCurrentCredit.Value = cr;
        //    nmrCurrentDebit.Value = db;
        //    if (adj < 1)
        //        nmrCurrentCredit.Value += Math.Abs(adj);
        //    else
        //        nmrCurrentDebit.Value += adj;

        //    if (bal < 1)
        //        nmrCurrentCredit.Value += Math.Abs(bal);
        //    else
        //        nmrCurrentDebit.Value += bal;
        //    //decimal xxamt = nmrCurrentDebit.Value - nmrCurrentCredit.Value;
        //    //nmrPayable.Value = Math.Abs(xamt);
        //    //lblbalbfdbcr.Text = (xxamt < 1) ? "CR" : "DB";
        //    nmrPayable.Value = Math.Abs(nmrCurrentDebit.Value - nmrCurrentCredit.Value);
        //    lblpayabledbcr.Text = nmrCurrentDebit.Value - nmrCurrentCredit.Value < 1 ? "CR" : "DB";
        //    if (lblpayabledbcr.Text == "CR")
        //        nmrAmount.Value = 0m;

        //}

        //private void cboFacility_LostFocus(object sender, EventArgs e)
        //{
        //    txtName.Select();
        //    return;
        //}

        //private void nmrAmount_Enter(object sender, EventArgs e)
        //{
        //    //if (nmrAmount.Value == 0m)
        //    //    return;
        //    //NumericUpDown fld = sender as NumericUpDown;
        //    //fld.Select(0, fld.Text.Length);
        //    nmrAmount.Select(0, nmrAmount.Text.Length);

        //    //   nmrAmount.Value = lblpayabledbcr.Text == "CR" ? 0 : nmrPayable.Value;
        //    if (string.IsNullOrWhiteSpace(combConsAdmNo.Text) && mcusttype == "C")
        //        pancapitations.Visible = true;
        //    if (!newrec)
        //        nmrAmount.Value = (decimal)paydetail.Rows[0]["amount"];
        //    if (lbladmissiondeposit.Visible && nmrAdmissionDeposit.Value > 0m)
        //    {
        //        //  nmrAmount.Value += nmrAdmissionDeposit.Value;
        //        txtDetails.Text = "Admissions Deposit";
        //    }
        //    if (!string.IsNullOrWhiteSpace(combConsAdmNo.Text) && combConsAdmNo.Text.Trim().Length > 0)
        //    {
        //        if (combConsAdmNo.Text.Substring(0, 1) == "A")
        //            txtName.Text = admrec.NAME.Trim() + " : " + admrec.PATIENTNO.Trim() + ":" + admrec.GROUPCODE.Trim() + ":" + admrec.REFERENCE.Trim();
        //        else
        //            txtName.Text = mrattend.NAME.Trim() + " : " + mrattend.PATIENTNO.Trim() + ":" + mrattend.GROUPCODE.Trim() + ":" + mrattend.REFERENCE.Trim();
        //    }

        //}

        private void combPmtMode_LostFocus(object sender, EventArgs e)
        {
            //msgeventtracker = "PM";
            //dtRecvdDate.Enabled = true;
            //if (combPmtMode.Text.Trim() == "POS/CREDIT CARD" || combPmtMode.Text.Trim() == "BANK TELLER" ||
            //    combPmtMode.Text.Trim() == "DIRECT CREDIT") //  "RBD" Credit Card/ATm-bank teller-direct credit
            //{
            //    lbldetails.Text = "BANK";
            //    combbank.Visible = true;
            //    combbank.Focus();
            //}
            //else
            //{
            //    lbldetails.Text = "DETAILS";
            //    combbank.Visible = false;
            //    combbank.Focus();
            //}

            //if (!string.IsNullOrWhiteSpace(combPmtMode.Text) && newrec && combPmtMode.Text.Trim() == "CASH")
            //{
            //    txtDetails.Text = (string.IsNullOrWhiteSpace(txtDetails.Text) || txtDetails.Text.Length > 18 &&
            //        txtDetails.Text.Substring(0, 18) != "Admissions Deposit") ?
            //        "C A S H" : txtDetails.Text;
            //    dtRecvdDate.Enabled = false;
            //    txtDetails.Focus();
            //}
            //if (combPmtMode.SelectedItem == null || string.IsNullOrWhiteSpace(combPmtMode.SelectedItem.ToString()))
            //{
            //    MessageBox.Show("Payment Mode cannot be empty...", "NOTE :");
            //    nmrAmount.Focus();
            //    return;
            //}
            //btnSave.Enabled = true;
        }

        //private void txtDetails_LostFocus(object sender, EventArgs e)
        //{
        //    //if (isadmissions && newrec)
        //    //{
        //    //    txtDetails.Text = txtDetails.Text.Trim() + " - Admissions";
        //    //}
        //    dtValueDate.Focus();
        //    return;
        //}

        //  private void combbank_LostFocus(object sender, EventArgs e)
        //  {
        //      if (string.IsNullOrWhiteSpace(combbank.Text))
        //          return;
        //      /*              MessageBox.Show("A Bank must be selected for Paymode - CreditCard/BankTeller/DirectCredit", "PAYMENT MODE", msgBoxHandler);
        //	//combPmtMode.Focus();
        //	//combbank.Focus();
        //	//return;
        //}
        //else
        //{ */
        //      // txtDetails.Text = (combPmtMode.Text.Substring(0,1) == "R") ? "CreditCard" : (combPmtMode.Text.Substring(0,1) == "B") ? 
        //      //     "BankTeller" : "DirectCredit";
        //      //     txtDetails.Text = txtDetails.Text +" - "+txtName.Text.Trim();
        //      txtDetails.Text = combPmtMode.Text.Trim() + " - " + txtName.Text.Trim() + "\r\n : (" +
        //          combbank.Text.Trim() + ")";
        //      //extract glupdae profile
        //      for (int i = 0; i < atmprofile.Rows.Count; i++)
        //      {
        //          if (atmprofile.Rows[i]["Name"].ToString().Trim() == combbank.Text.Trim())
        //          {
        //              atmcompany = atmprofile.Rows[i]["company"].ToString();
        //              atmbatchno = (decimal)atmprofile.Rows[i]["batchno"];
        //              atmdocument = atmprofile.Rows[i]["document"].ToString();
        //              atmpos_charge = (decimal)atmprofile.Rows[i]["pos_charge"];
        //              atmExpense_Acct = atmprofile.Rows[i]["Expense_Acct"].ToString();
        //              atmdebit = atmprofile.Rows[i]["debit"].ToString();
        //              atmcredit = atmprofile.Rows[i]["credit"].ToString();
        //              break;
        //          }
        //      }
        //      lbldetails.Text = "DETAILS";
        //      combbank.Visible = false;
        //      txtDetails.Focus();
        //  }

        private void dtValueDate_Enter(object sender, EventArgs e)
        {
            //msgeventtracker = "PM";
            //if ( string.IsNullOrWhiteSpace(txtDetails.Text) ) //&& string.IsNullOrWhiteSpace(combbank.SelectedItem.ToString()))
            //{
            //    DialogResult result = MessageBox.Show("Payment mode or details cannot be empty...","PAYMENT MODE",msgBoxHandler );
            //    nmrAmount.Focus();
            //    return;
            //}
            //if ((combPmtMode.Text.Trim() == "POS/CREDIT CARD" || combPmtMode.Text.Trim() == "BANK TELLER" || combPmtMode.Text.Trim() == "DIRECT CREDIT") && string.IsNullOrWhiteSpace(combbank.Text)) //  "RBD" Credit Card/ATm-bank teller-direct credit
            //{
            //    DialogResult result = MessageBox.Show("A Bank must be selected for Pay Mode - CreditCard/BankTeller/DirectCredit","GL Interface from payment",msgBoxHandler);
            //    combbank.Select();
            //    return;
            //}
            //if (isadmissions && newrec )
            //{
            //    txtDetails.Text = txtDetails.Text.Trim()+" - Admissions";
            //}
        }

        //private void dtValueDate_LostFocus(object sender, EventArgs e)
        //{
        //    DialogResult result;
        //    if (dtValueDate.Value.Date < DateTime.Now.Date && !disallowbackdate)
        //    {
        //        // msgeventtracker = "OVERWRITE";
        //        result = MessageBox.Show("Transactions not allowed on/or before  :  " + DateTime.Now.Date.AddDays(-1).ToString() + "\n\n ...Overwrite Authorization required !  CONTINUE... ?", "TRANSACTION DATE CONTROL SETUP", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.No)
        //        {
        //            dtValueDate.Value = DateTime.Now.Date;
        //            dtValueDate.Select();
        //            return;
        //        }
        //        frmOverwrite overwrite = new frmOverwrite("Overwrite to Date Control", "MRSTLEV", "MR");
        //        overwrite.ShowDialog();
        //        if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode)) //not successful
        //        {
        //            combConsAdmNo.Text = anycode = "";
        //            combConsAdmNo.Focus();
        //            return;
        //        }
        //    }
        //    if (dtValueDate.Value.Date <= DateTime.Now.Date && !bissclass.checkperiod(dtValueDate.Value.Date, msmrfunc.mrGlobals.mlastperiod, msmrfunc.mrGlobals.mpyear, dtmin_date))
        //    {
        //        nmrAmount.Focus();
        //        return;
        //    }
        //    else if (combPmtMode.Text.Trim() == "CHEQUE" && dtValueDate.Value > DateTime.Now.Date)
        //    {
        //        result = MessageBox.Show("Post-Dated Cheque ? Confirm to Continue...", "VALUE DATE VALIDATION",
        //            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.No)
        //        {
        //            dtValueDate.Value = DateTime.Now.Date;
        //            dtValueDate.Focus();
        //            return;
        //        }
        //        dtRecvdDate.Focus();
        //    }
        //    else
        //    {
        //        btnSave.Enabled = true;
        //        btnSave.Focus();
        //        return;
        //    }

        //}

        //private void dtRecvdDate_Leave(object sender, EventArgs e)
        //{
        //    if (dtRecvdDate.Value.Date > DateTime.Now.Date ||
        //        !bissclass.checkperiod(dtRecvdDate.Value.Date, msmrfunc.mrGlobals.mlastperiod, msmrfunc.mrGlobals.mpyear, dtmin_date))
        //    {
        //        dtRecvdDate.Value = DateTime.Now.Date;
        //        dtRecvdDate.Focus();
        //        return;
        //    }
        //}

        public MR_DATA.REPORTS btnSave_Click()
        {
            //DialogResult result;
            //if (string.IsNullOrWhiteSpace(txtDetails.Text)) //&& string.IsNullOrWhiteSpace(combbank.SelectedItem.ToString()))
            //{
            //    result = MessageBox.Show("Payment mode or details cannot be empty...", "PAYMENT MODE");
            //    return;
            //}
            //if (isotherserviceIncome && (string.IsNullOrWhiteSpace(cboFacility.Text)) || string.IsNullOrWhiteSpace(txtName.Text))
            //{
            //    result = MessageBox.Show("Service Centre/Facility/NAME for this Payment must be specified...");
            //    return;
            //}
            //if ((combPmtMode.Text.Trim() == "POS/CREDIT CARD" || combPmtMode.Text.Trim() == "BANK TELLER" || combPmtMode.Text.Trim() == "DIRECT CREDIT") && string.IsNullOrWhiteSpace(combbank.Text)) //  "RBD" Credit Card/ATm-bank teller-direct credit
            //{
            //    result = MessageBox.Show("A Bank must be selected for Pay Mode - CreditCard/BankTeller/DirectCredit", "GL Interface from payment");
            //    return;
            //}
            //if (isadmissions && newrec)
            //{
            //    txtDetails.Text = txtDetails.Text.Trim() + " - Admissions";
            //}
            //if (newrec && !mcanadd || !newrec && !mcanalter)
            //{
            //    string xstring = newrec ? "NEW RECORD CREATION" : "RECORD ALTERATION";
            //    result = MessageBox.Show("ACCESS DENIED...! - SEE YOUR SYSTEMS ADMINISTRATOR", xstring);
            //    return;
            //}
            // msgeventtracker = "PM";
            //if (nmrAmount.Value != 0m && nmrAmount.Value < 1m && msection != "C") //only Accounts 13.08.2018 billing dept allowed
            //{
            //    result = MessageBox.Show("Invalid Amount Specification...", "NEGATIVE PAY AMOUNT");
            //    nmrAmount.Value = 0m;
            //    nmrAmount.Select();
            //    return;
            //}
            //if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(combPmtNo.Text))
            //{
            //    result = MessageBox.Show("Vital Fields are empty.. Cannot Save Record", "Payment Records");
            //    return;
            //}
            //if (string.IsNullOrWhiteSpace(combConsAdmNo.Text) && isadmissions)
            //{
            //    // msgeventtracker = "REF";
            //    result = MessageBox.Show("Admissions Number or Reference must be specified...", "Admissions Deposit/Payments");
            //    return;
            //}
            //if (nmrAdmissionDeposit.Visible && nmrAdmissionDeposit.Value > 0m && nmrAmount.Value < nmrAdmissionDeposit.Value)
            //{
            //    result = MessageBox.Show("Amount specified is LESS THAN Admissions Deposit Requirement...\r\n    CONTINUE ? ", "Admissions Deposit/Payments", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //    if (result == DialogResult.No)
            //        return;
            //}

            //result = MessageBox.Show("Confirm to Save...", "Payment Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.No)
            //    return;
            //btnSave.Enabled = false;

            admrec = Admrecs.GetADMRECS(vm.REPORTS.txtreference);
            mrattend = Mrattend.GetMrattend(vm.REPORTS.txtreference);

            if (vm.REPORTS.txtdiscount != 0)
            {
                if (!string.IsNullOrWhiteSpace(vm.REPORTS.txtreference))
                {
                    if (vm.REPORTS.txtreference.Substring(0, 1) == "A")
                        vm.REPORTS.edtallergies = vm.REPORTS.edtallergies + " : " + admrec.PATIENTNO.Trim() + ":" + admrec.GROUPCODE.Trim() + ":" + admrec.REFERENCE.Trim();
                    else
                    {
                        string xstr = string.IsNullOrWhiteSpace(mrattend.PATIENTNO) ? "" : mrattend.PATIENTNO.Trim() + ":" + mrattend.GROUPCODE.Trim() + ":";
                        vm.REPORTS.edtallergies = vm.REPORTS.edtallergies + " : " + xstr + mrattend.REFERENCE.Trim();
                    }
                }

                savedetails();
            }

            LINKS_UPDATE();


            if(mrattend != null)
            {
                if (vm.REPORTS.txtdiscount == 0 && !string.IsNullOrWhiteSpace(vm.REPORTS.txtreference)) //11.7.2019 - Harmony wants audit trail for 0 amt
                {
                    billchaindtl xbc = billchaindtl.Getbillchain(mrattend.PATIENTNO, mrattend.GROUPCODE);
                    msmrfunc.updateOverwrite(vm.REPORTS.txtreference, "ZERO amt to allow link flow by : " + woperator + 
                        " @ " + DateTime.Now.ToLongDateString(), xbc, 0m, 0m, woperator);
                }
            }

            //ClearControls("");
            //btnReload.PerformClick();

            if (new string[] { "1", "2", "3", "9" }.Contains(msection))
                vm.REPORTS.chkgetdependants = true; //for btnReload.PerformClick();
            else
            {
                vm.REPORTS.alertMessage = "Payment Records Submitted successfully...";
                //combConsAdmNo.Focus();
            }
            return vm.REPORTS;
        }

        void savedetails()
        {
            //  msgeventtracker = "REF";
            if (newrec && bissclass.IsDigitsOnly(vm.REPORTS.txtstaffno) && Convert.ToDecimal(vm.REPORTS.txtstaffno) >= mlastno) //update ref. counter and get a new value, if necessary.
            {
                decimal lastnosave = mlastno;
                mlastno = msmrfunc.getcontrol_lastnumber("PAYNO", 4, true, mlastno, false);
                if (mlastno != lastnosave)
                    vm.REPORTS.txtstaffno = bissclass.autonumconfig(mlastno.ToString(), true, (string.IsNullOrWhiteSpace(mbr_cc)) ? "" : mbr_cc, "999999999");

            }

            mservicetype = "";
            //if (pancapitations.Visible)
            mservicetype = (vm.REPORTS.chkHMO) ? "H" : (vm.REPORTS.chkByBranch) ? "N" : (vm.REPORTS.chkIncludeBf) ? "R" : "";

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "PAYDETAIL_Add" : "PAYDETAIL_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", vm.REPORTS.txtstaffno);
            insertCommand.Parameters.AddWithValue("@patientno", mpatientno);
            insertCommand.Parameters.AddWithValue("@name", vm.REPORTS.TXTPATIENTNAME);
            insertCommand.Parameters.AddWithValue("@itemno", 1m);
            insertCommand.Parameters.AddWithValue("@description", vm.REPORTS.edtallergies);
            insertCommand.Parameters.AddWithValue("@doctor", "");
            insertCommand.Parameters.AddWithValue("@facility", "");
            insertCommand.Parameters.AddWithValue("@amount", vm.REPORTS.txtdiscount);
            insertCommand.Parameters.AddWithValue("@trans_date", vm.REPORTS.dtbirthdate.Date);
            insertCommand.Parameters.AddWithValue("@sec_level", 0m);
            insertCommand.Parameters.AddWithValue("@posted", true);
            //(combPmtMode.Text.Trim() == "CREDIT/ATM CARD" || combPmtMode.Text.Trim() == "BANK TELLER" ||
            //combPmtMode.Text.Trim() == "DIRECT CREDIT" || combPmtMode.Text.Trim() == "CASH" ) ? true :  false );
            /*            if (isotherserviceIncome)
			{
				combGpCd.Text = "PVT      ";
				combClientCd.Text = miscpatientAcct;
			}*/
            insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@receipted", true);
            insertCommand.Parameters.AddWithValue("@transtype", mcusttype);
            insertCommand.Parameters.AddWithValue("@grouphead", mgroupcode == "NHIS" ? NHISAcct : isotherserviceIncome && string.IsNullOrWhiteSpace(vm.REPORTS.txtghgroupcode) ? miscpatientAcct : vm.REPORTS.txtghgroupcode);
            insertCommand.Parameters.AddWithValue("@servicetype", mservicetype);
            insertCommand.Parameters.AddWithValue("@groupcode", mgroupcode);
            insertCommand.Parameters.AddWithValue("@ttype", "C");
            insertCommand.Parameters.AddWithValue("@GHGROUPCODE", mgroupcode == "NHIS" ? "" : isotherserviceIncome && string.IsNullOrWhiteSpace(vm.REPORTS.txtghgroupcode) ? "PVT      " : vm.REPORTS.txtgroupcode);
            insertCommand.Parameters.AddWithValue("@paytype", vm.REPORTS.combillcycle);
            insertCommand.Parameters.AddWithValue("@operator", woperator);
            insertCommand.Parameters.AddWithValue("@op_time", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@accounttype", mservicetype);
            insertCommand.Parameters.AddWithValue("@currency", "");
            insertCommand.Parameters.AddWithValue("@exrate", 0m);
            insertCommand.Parameters.AddWithValue("@fcamount", 0m);
            insertCommand.Parameters.AddWithValue("@extdesc", "");
            insertCommand.Parameters.AddWithValue("@datereceived", vm.REPORTS.dtregistered.Date);
            insertCommand.Parameters.AddWithValue("@CROSSREF", vm.REPORTS.txtreference);
            insertCommand.Parameters.AddWithValue("@PVTDEPOSIT", mcusttype == "P" && vm.REPORTS.chkQueryTimeofDay ? true : false);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;
            }
            catch (SqlException ex)
            {
                // throw ex;
                vm.REPORTS.alertMessage = " " + ex;
                return;
            }
            finally
            {
                connection.Close();
            }
            return;
        }

        void LINKS_UPDATE()
        {
            string updatestring = "";
            vm.REPORTS.chkCummulativeSumm = false;  //for pancapitations.Visible = false;
            if ((msection == "1" || msection == "2" || msection == "3" || msection == "9") &&
                !string.IsNullOrWhiteSpace(vm.REPORTS.txtreference) && !isadmissions) //$ "1239" .and. !empty(mcrossref) AND !isadmissions
            {
                if (ismeddiag)
                {
                    anycode = "6";
                    linkdetails();
                }
                else
                {
                    vm.REPORTS.chkIncludeOnHold = true; //A flag to call paylinkinfo form

                    //*****************************************************
                    //paylinkinfo PayLinkInfo = new paylinkinfo(); //linkdetails are updated here...
                    //PayLinkInfo.Closed += new EventHandler(PayLinkInfo_Closed);
                    //PayLinkInfo.ShowDialog();
                }
            }

            //if (lbladmissiondeposit.Visible && mrecid > 0)
            if (mrecid > 0) //01/08/2012 - admission deposit
            {
                updatestring = "UPDATE LINK SET LINKOK = '1' WHERE reference = '" + vm.REPORTS.txtreference + "' and tosection = 'A'";
                bissclass.UpdateRecords(updatestring, "MR");
                //  LINK.updateLinkOk(combConsAdmNo.Text, "A", DateTime.Now.Date, DateTime.Now.ToShortDateString(), "", true, true);
            }
            //update cashier's daily holding
            if (vm.REPORTS.txtdiscount > 0m)
            {
                LINK1.LINK1Write(newcashier, woperator, DateTime.Now.Date, vm.REPORTS.txtdiscount, 1);
                // getcashierbalance();
            }

            //update admissions payment
            if (isadmissions)
            {
                Admrecs.UpdateAdmrecAmounts(vm.REPORTS.txtreference, 0m, vm.REPORTS.txtdiscount);
            }
            if (string.IsNullOrWhiteSpace(vm.REPORTS.txtreference) && newrec && (vm.REPORTS.chkByBranch || vm.REPORTS.chkHMO || vm.REPORTS.chkIncludeBf) || isotherserviceIncome)
            {
                // msgeventtracker = "CAPS";
                if (mgroupcode.Trim() != "NHIS")
                {
                    string xstr = isotherserviceIncome ? "Confirm to Generate Bill to Nill Off this Misc Payment" : "Confirm to Generate HMO/NHIS Capitation Pay-Bill or Retainership Bill";
                    vm.REPORTS.ActRslt = "" + xstr; // isotherserviceIncome ? "Auto Generation of Service Bill" : "Auto Generation of Capitation/Retainership Bill", MessageBoxButtons.YesNo,

                    generateCapitation();

                    //if (result == DialogResult.Yes)
                    //{
                    //    generateCapitation();
                    //}
                }
                else
                    generateCapitationdetails("NHIS Service Bill for " + vm.REPORTS.TXTPATIENTNAME);
            }
            if (isgl && vm.REPORTS.txtdiscount != 0m && vm.REPORTS.dtbirthdate.Date <= DateTime.Now.Date)
            {
                updategl(false);
            }
            //combConsAdmNo.Focus();
            return;
        }

        //void PayLinkInfo_Closed(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    paylinkinfo PayLinkInfo_Closed = sender as paylinkinfo;

        //    anycode = msmrfunc.mrGlobals.anycode;
        //    mfacility = msmrfunc.mrGlobals.mfacility;
        //    mrecid = msmrfunc.mrGlobals.recid;
        //    linkdetails();

        //}

        void linkdetails()
        {
            string updatestring = "";
            int xcount = (servicetagged.Rows.Count > 0) ? servicetagged.Rows.Count : 1;
            string[] tempa_ = new string[xcount];
            string xfacility;
            for (int i = 0; i < servicetagged.Rows.Count; i++)
            {
                if (msmrfunc.mrGlobals.taggedFromSuspensea_[i] == "YES") //tagged
                {
                    xfacility = servicetagged.Rows[i]["facility"].ToString();
                    if (i > 0 && tempa_.Contains(xfacility))
                        continue;
                    tempa_[i] = xfacility;
                }
            }
            if (servicetagged.Rows.Count < 1)
            {
                tempa_[0] = mfacility;
            }
            for (int i = 0; i < tempa_.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(tempa_[i]))
                {
                    LINK.WriteLINK(0, mgroupcode, mpatientno, mname, anycode.Substring(0, 1), vm.REPORTS.txtreference, 0m, 0m, tempa_[i], true, "", false, 0, "", msection, woperator);
                    LINK3.WriteLINK3(mgroupcode, mpatientno, DateTime.Now, mname, "INV/PROC - " + tempa_[i], vm.REPORTS.txtreference, DateTime.Now.ToLongTimeString(), start_time, "6", tempa_[i], start_time, woperator); //attendance monitor
                                                                                                                                                                                                                           /*       IF !EMPTY(tempa_[x])
                                                                                                                                                                                                                           writemonitor(mgroupcode,mpatientno,date(),mcrossref,"INV/PROC - "+tempa_[x],mname,TIME(),"","I",tempa_[x]) */
                }
            }

            if ((anycode.Substring(0, 1) == "6" || anycode.Substring(0, 1) == "8") && !ismeddiag) //cashier selected pharmacy/lab/xray/scan
            {
                //update link ok
                updatestring = "UPDATE LINK SET LINKOK = '1' WHERE reference = '" + vm.REPORTS.txtreference + "' and linkok = '0' and (tosection = '8' or tosection = '6') ";
                bissclass.UpdateRecords(updatestring, "MR");
                // LINK.updateLinkOk(combConsAdmNo.Text, "8", dtValueDate.Value, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), true, true);
            }

            //update received date in link
            //multiple records are usually sent by docs and PCP TO CASHIER
            updatestring = "UPDATE LINK SET LINKOK = '1', DATEREC = '" + DateTime.Now.ToShortDateString() + "', TIMEREC = '" + DateTime.Now.ToLongTimeString() + "' WHERE reference = '" + vm.REPORTS.txtreference + "' and tosection = '2' "; //and linkok = '0'
            bissclass.UpdateRecords(updatestring, "MR");
            // LINK.updateLinkOk(combConsAdmNo.Text, "2", DateTime.Now.Date, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("HH:mm:ss"), true, true);
        }

        //private void btnhideshow_Click(object sender, EventArgs e)
        //{
        //    if (pnlDailyCash.Visible)
        //        pnlDailyCash.Visible = false;
        //    else
        //        pnlDailyCash.Visible = true;
        //}

        //private void btnPrintRcpt_Click(object sender, EventArgs e)
        //{
        //    ReceiptsGenerator RcptGen = new ReceiptsGenerator(mreference, woperator, false, disallowbackdate); //combPmtNo.Text);
        //    RcptGen.ShowDialog();
        //}

        //void ClearControls(string xcontrol)
        //{
        //    lbladmissiondeposit.Visible = panPVTDeposit.Visible = nmrAdmissionDeposit.Visible = false;
        //    combPmtNo.Text = combCurrency.Text = combBillRef.Text = txtBillVal.Text = txtDetails.Text = txtName.Text = combGpCd.Text = combClientCd.Text = combPmtMode.Text = "";
        //    nmrCurrentDebit.Value = nmrPayable.Value = nmrCurrentCredit.Value = nmrBalanceBF.Value = nmrAdmissionDeposit.Value = nmrAmount.Value = 0m;
        //    combPmtMode.SelectedItem = "";
        //    listView1.Items.Clear();
        //    pictureBox1.Image = null;
        //    if (xcontrol != "R")
        //    {
        //        combConsAdmNo.Text = "";
        //    }
        //    //if (!isotherserviceIncome)
        //    //    combGpCd.Text = combClientCd.Text = "";
        //}

        //private void btnExit_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        void updategl(bool xbillsonly)
        {
            bool glnewrec;
            decimal xamt = vm.REPORTS.txtdiscount;
            string xcompany = (xbillsonly || string.IsNullOrWhiteSpace(atmcompany)) ? mglcompany : atmcompany;
            decimal xbatchno = (xbillsonly || atmbatchno < 1) ? mglbatchno : atmbatchno;
            string xdocument = (xbillsonly || string.IsNullOrWhiteSpace(atmdocument)) ? mgldocument : atmdocument;
            acc05 = Dataaccess.GetAnytable("", "GL", "select recid from acc05 where acctyear = '" + vm.REPORTS.dtbirthdate.Year + "' and period = '" + vm.REPORTS.dtbirthdate.Month + "' and batchno = '" + xbatchno + "'", false);
            //ACC05.Getacc05(xcompany, Convert.ToDecimal(dtValueDate.Value.Year), Convert.ToDecimal(dtValueDate.Value.Month), xbatchno);
            glnewrec = (acc05.Rows.Count > 0) ? false : true;
            ACC05.acc05Write(glnewrec, xcompany, Convert.ToDecimal(vm.REPORTS.dtbirthdate.Year), Convert.ToDecimal(vm.REPORTS.dtbirthdate.Month), xbatchno, xamt, xamt, xamt, xamt, vm.REPORTS.dtbirthdate.Date, 1, "C", glnewrec ? 0 : (Int32)acc05.Rows[0]["recid"]); //batch update
                                                                                                                                                                                                                                                                        //update period file and trans counter
            acc08 = Dataaccess.GetAnytable("", "GL", "select recid from acc08 where acctyear = '" + vm.REPORTS.dtbirthdate.Year + "' and period = '" + vm.REPORTS.dtbirthdate.Month + "' ", false);
            if (acc08.Rows.Count > 0)
                ACC08.acc08aWrite(false, xcompany, Convert.ToDecimal(vm.REPORTS.dtbirthdate.Year), Convert.ToDecimal(vm.REPORTS.dtbirthdate.Month), Convert.ToDecimal(vm.REPORTS.dtbirthdate.Month), vm.REPORTS.dtbirthdate.Year, "", 1m, false, DateTime.Now, vm.REPORTS.transDate.Date, 0m, vm.REPORTS.txtdiscount, (Int32)acc08.Rows[0]["recid"]);
            acc05a = Dataaccess.GetAnytable("", "GL", "select recid,TOTITEM from acc05a where acctyear = '" + vm.REPORTS.dtbirthdate.Year + "' and period = '" + vm.REPORTS.dtbirthdate.Month + "' and batchno = '" + xbatchno + "' and rtrim(jvno) = '" + xdocument.Trim() + "'", false);

            glnewrec = (acc05a.Rows.Count > 0) ? false : true;
            ACC05A.acc05aWrite(glnewrec, xcompany, Convert.ToDecimal(vm.REPORTS.dtbirthdate.Year), Convert.ToDecimal(vm.REPORTS.dtbirthdate.Month), xbatchno, xdocument, xamt, xamt, vm.REPORTS.dtbirthdate.Date, 0, "C", glnewrec ? 0 : (Int32)acc05a.Rows[0]["recid"]); //document update
                                                                                                                                                                                                                                                                          /* add record to acc02 - Atmprofile criteria - Debit Bank / Credit Control Acct
                                                                                                                                                                                                                                                                              was previously done directly in GL by account officer
                                                                                                                                                                                                                                                                              03-04-2013 POS_Charge Added to ATMprofile 'cos Hosp Bank Acct is not credited with actual payment by patient
                                                                                                                                                                                                                                                                              An expense acct for POS Charge must be credited if POS Charge Applies - R : for Credit Cared/ATM */

            decimal xitemno = (acc05a.Rows.Count > 0) ? Convert.ToDecimal(acc05a.Rows[0]["TOTITEM"]) : 0m;
            if (!xbillsonly && (vm.REPORTS.combillcycle == "POS/CREDIT CARD" || vm.REPORTS.combillcycle == "BANK TELLER" || vm.REPORTS.combillcycle == "DIRECT CREDIT")) //  "RBD" Credit Card/ATm-bank teller-direct credit
            {
                decimal xposamt = 0m;
                xitemno++;
                if (vm.REPORTS.combillcycle == "POS/CREDIT CARD" && atmpos_charge > 0m && !string.IsNullOrWhiteSpace(atmExpense_Acct)) //CreditCard/ATM
                {
                    xposamt = (xamt * atmpos_charge) / 100;
                    ACC02.ACC02Write(newrec, atmExpense_Acct, xcompany, mcurrency, xbatchno, Convert.ToDecimal(vm.REPORTS.dtbirthdate.Year), Convert.ToDecimal(vm.REPORTS.dtbirthdate.Month), xdocument, vm.REPORTS.dtbirthdate.Date, vm.REPORTS.edtallergies, "D", xposamt, xitemno, woperator, DateTime.Now, "", "", 0m);
                }
                decimal xdebitamt = xamt;
                for (int i = 0; i < 2; i++)
                {
                    // xitemno++;
                    if (xposamt > 0 && i == 0)
                    {
                        xdebitamt = xamt - xposamt; //POS/ATM charge applies. Debit to bank should be less than actual payment
                    }
                    ACC02.ACC02Write(newrec, (i == 0) ? atmdebit : atmcredit, xcompany, mcurrency, xbatchno, Convert.ToDecimal(vm.REPORTS.dtbirthdate.Year), Convert.ToDecimal(vm.REPORTS.dtbirthdate.Month), xdocument, vm.REPORTS.dtbirthdate.Date, vm.REPORTS.edtallergies, (i == 0) ? "D" : "C", (i == 0) ? xdebitamt : xamt, xitemno, woperator, DateTime.Now, "", "", 0m);
                }
            }
            string debitacct = "", creditacct = "";

            bool corporatehmo = (mcusttype == "C" && customers.HMO) ? true : false;
            msmrfunc.getdebitcredit_acct("3", xbillsonly ? gltab3 : glupdate, ref debitacct, ref creditacct, mcusttype, corporatehmo, xbillsonly ? "B" : "P", xbillsonly ? "" : mfacility, mgroupcode, mservicetype, vm.REPORTS.edtallergies, vm.REPORTS.combillcycle);
            acc05a = Dataaccess.GetAnytable("", "GL", "select recid,TOTITEM from acc05a where acctyear = '" + vm.REPORTS.dtbirthdate.Year + "' and period = '" + vm.REPORTS.dtbirthdate.Month + "' and batchno = '" + mglbatchno + "' and rtrim(jvno) = '" + mgldocument.Trim() + "'", false);

            glnewrec = (acc05a.Rows.Count > 0) ? false : true;
            ACC05A.acc05aWrite(glnewrec, xcompany, vm.REPORTS.dtbirthdate.Year, vm.REPORTS.dtbirthdate.Month, Convert.ToInt32(mglbatchno), mgldocument, xamt, xamt, vm.REPORTS.dtbirthdate.Date, 0, "C", glnewrec ? 0 : (Int32)acc05a.Rows[0]["recid"]); //document update
                                                                                                                                                                                                                                                         // xitemno++;
            xitemno = (acc05a.Rows.Count > 0) ? Convert.ToDecimal(acc05a.Rows[0]["TOTITEM"]) + 1 : 1m;
            string xn = mcusttype == "P" || mgroupcode == "NHIS" || isotherserviceIncome ? vm.REPORTS.TXTPATIENTNAME : "";
            for (int i = 0; i < 2; i++)
            {
                ACC02.ACC02Write(newrec, (i == 0) ? debitacct : creditacct, xcompany, mcurrency, mglbatchno, Convert.ToDecimal(vm.REPORTS.dtbirthdate.Year), Convert.ToDecimal(vm.REPORTS.dtbirthdate.Month), mgldocument, vm.REPORTS.dtbirthdate.Date, vm.REPORTS.edtallergies + " - " + xn, (i == 0) ? "D" : "C", xamt, xitemno, woperator, DateTime.Now, "", "", 0m);
            }
        }

        void generateCapitation()
        {
            string descriptn =
                isotherserviceIncome ? "Service Bill for " :
                (vm.REPORTS.chkHMO) ? "HMO Monthly Capitation Pay-Bill For " :
                (vm.REPORTS.chkByBranch) ? "NHIS Monthly Capitation Pay-Bill For " : "Retainership Fee (Bill) For ";
            //09/11/2008, 01-01-2014 PROVIDE FOR AUTO GENERATION OF HMO/NHIS BILL
            //msmrfunc.frmInputBox("NHIS/HMO Capitation Pay-Bill","Enter Month/Year of Capitation or Retainership - Transactn Descriptn ",
            //    ref descriptn);
            POPREAD popread = new POPREAD(descriptn, isotherserviceIncome ? "Enter Description of Pay-Bill" : "Enter Month/Year of Capitation or Retainership - Transactn Descriptn ", ref descriptn, false, false, "", "", "", false, "", "");
            popread.Closed += new EventHandler(popread_Closed);
            popread.ShowDialog();
        }

        void popread_Closed(object sender, EventArgs e)
        {
            POPREAD popread = sender as POPREAD;
            string descriptn = bissclass.sysGlobals.anycode;
            generateCapitationdetails(descriptn);
            return;
        }

        void generateCapitationdetails(string descriptn)
        {
            //   string descriptn = msmrfunc.mrGlobals.anycode;
            //get billing reference counter in control - alternate
            string xaccounttype = (vm.REPORTS.chkByBranch) ? "N" : (vm.REPORTS.chkHMO) ? "H" : "R";
            decimal alt_lastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, true, 0m, false) + 1;
            string xreference = bissclass.autonumconfig(alt_lastno.ToString(), true, "", "999999999");
            Billings.writeBILLS(true, xreference, 1m, "", descriptn, mcusttype, vm.REPORTS.txtdiscount, vm.REPORTS.dtbirthdate.Date, vm.REPORTS.TXTPATIENTNAME, isotherserviceIncome ? "MISC" : mgrouphead, "", "", "", "D", isotherserviceIncome ? "PVT" : "", woperator, DateTime.Now, "", mcurrency, 0m, 0, "", "", true, "", xaccounttype, vm.REPORTS.txtdiscount, vm.REPORTS.combillcycle, 0m, "O", false, 0);
            //15/07/2012, 01-01-2014 : we must verify this bill to avoid being moved with capited bills
            if (isgl)
            {
                updategl(true);
            }
            //DialogResult result = MessageBox.Show("Done...", isotherserviceIncome ? "Auto Generation of Misc Pay-Bill" : "Auto Generation of NHIS/HMO/Retainership Pay-Bill" );
            //chknone.Checked = true;
            //if (new string[] { "1", "2", "3", "9" }.Contains(msection))
            //    btnReload.PerformClick();
            //else
            //    combConsAdmNo.Focus();
            //return;
        }

        //private void btndelete_Click(object sender, EventArgs e)
        //{
        //    if (!newrec && (bool)paydetail.Rows[0]["POSTED"])
        //    {
        //        MessageBox.Show("Record Can't be Deleted...");
        //        return;
        //    }
        //    // msgeventtracker = "PD";
        //    DialogResult result = MessageBox.Show("Confirm to Delete Record", "Payment Details", MessageBoxButtons.YesNo,
        //       MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.Yes)
        //    {
        //        PAYDETAIL.DeletePay(combPmtNo.Text);
        //        combPmtNo.Focus();
        //        return;
        //    }
        //}

        private void txtName_Leave(object sender, EventArgs e)
        {
            mpatientno = mgroupcode = "";
            mgrouphead = "MISC";
            mcusttype = "P";
            //nmrAmount.Focus();
        }

        //private void combPmtMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (combPmtMode.SelectedIndex == -1 || string.IsNullOrWhiteSpace(combPmtMode.Text))
        //        return;
        //    // msgeventtracker = "PM";
        //    dtRecvdDate.Enabled = true;
        //    if (combPmtMode.Text.Trim() == "POS/CREDIT CARD" || combPmtMode.Text.Trim() == "BANK TELLER" ||
        //        combPmtMode.Text.Trim() == "DIRECT CREDIT") //  "RBD" Credit Card/ATm-bank teller-direct credit
        //    {
        //        lbldetails.Text = "BANK";
        //        combbank.Visible = true;
        //        combbank.Focus();
        //    }
        //    else
        //    {
        //        lbldetails.Text = "DETAILS";
        //        combbank.Visible = false;
        //        combbank.Focus();
        //    }

        //    if (!string.IsNullOrWhiteSpace(combPmtMode.Text) && newrec && combPmtMode.Text.Trim() == "CASH")
        //    {
        //        txtDetails.Text = (string.IsNullOrWhiteSpace(txtDetails.Text) || txtDetails.Text.Length > 18 &&
        //            txtDetails.Text.Substring(0, 18) != "Admissions Deposit") ?
        //            "C A S H" : txtDetails.Text;
        //        dtRecvdDate.Enabled = false;
        //        txtDetails.Focus();
        //    }
        //    //if (combPmtMode.SelectedItem == null || string.IsNullOrWhiteSpace(combPmtMode.SelectedItem.ToString()))
        //    //{
        //    //    MessageBox.Show("Payment Mode cannot be empty...", "NOTE :");
        //    //    nmrAmount.Focus();
        //    //    return;
        //    //}
        //    btnSave.Enabled = true;

        //}

        //private void combConsAdmNo_KeyDown(object objSender, KeyEventArgs objArgs)
        //{

        //}

        //private void combConsAdmNo_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar != 13)
        //        return;
        //    ComboBox cbo = sender as ComboBox;
        //    if (cbo.Name == "combConsAdmNo")
        //        combConsAdmNo_LostFocus(null, null);
        //    else if (cbo.Name == "combPmtNo")
        //        combPmtNo_LostFocus(null, null);
        //    else if (cbo.Name == "combPmtMode")
        //        combPmtMode_LostFocus(null, null);
        //    else if (cbo.Name == "combGpCd")
        //        SelectNextControl(ActiveControl, true, true, true, true);

        //}

        //private void txtName_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtName_Leave(null, null);
        //}

        //private void cboFacility_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    cboFacility_LostFocus(null, null);
        //}

        //private void dtValueDate_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.Enter)
        //        dtValueDate_LostFocus(null, null);
        //}

        //private void btnLedger_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(mreference))
        //    {
        //        MessageBox.Show("No On-line Transaction Reference...");
        //        return;
        //    }

        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select trans_date, groupcode, patientno, grouphead, name from mrattend where reference = '" + combConsAdmNo.Text + "'", false);
        //    if (dt.Rows.Count < 1)
        //    {
        //        MessageBox.Show("Unable to Link Reference : " + mreference + " to Patients Transacitons...");
        //        return;
        //    }

        //    DataRow row = dt.Rows[0];
        //    if (row["patientno"].ToString() != row["grouphead"].ToString())
        //    {
        //        MessageBox.Show("Patient is not an account holder (Grouphead)... Can't print Ledger");
        //        return;
        //    }

        //    string xstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.ITEMNO, BILLING.DESCRIPTION, BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.TRANSTYPE, BILLING.GROUPHEAD, BILLING.SERVICETYPE, BILLING.GROUPCODE, BILLING.TTYPE, BILLING.GHGROUPCODE, BILLING.EXTDESC, BILLING.ACCOUNTTYPE, CHAR(50) AS GHNAME, 0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT FROM BILLING WHERE BILLING.TRANS_DATE >= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + "' AND BILLING.TRANS_DATE <= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + " 23:59:59.999' and BILLING.groupcode = '" + row["groupcode"].ToString() + "' and BILLING.patientno = '" + row["patientno"].ToString() + "' UNION ALL SELECT PAYDETAIL.REFERENCE, PAYDETAIL.PATIENTNO, PAYDETAIL.NAME, PAYDETAIL.ITEMNO, PAYDETAIL.DESCRIPTION, PAYDETAIL.AMOUNT, PAYDETAIL.TRANS_DATE, PAYDETAIL.TRANSTYPE, PAYDETAIL.GROUPHEAD, PAYDETAIL.SERVICETYPE, PAYDETAIL.GROUPCODE, PAYDETAIL.TTYPE, PAYDETAIL.GHGROUPCODE, PAYDETAIL.EXTDESC, PAYDETAIL.ACCOUNTTYPE, CHAR(50) AS GHNAME, 0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT FROM PAYDETAIL WHERE PAYDETAIL.TRANS_DATE >= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + "' AND PAYDETAIL.TRANS_DATE <= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + " 23:59:59.999' and PAYDETAIL.groupcode = '" + row["groupcode"].ToString() + "' and PAYDETAIL.patientno = '" + row["patientno"].ToString() + "' UNION ALL SELECT BILL_ADJ.REFERENCE, CHAR(5) AS PATIENTNO, BILL_ADJ.NAME, 0 AS ITEMNO, RTRIM(BILL_ADJ.ADJUST)+' '+BILL_ADJ.COMMENTS AS DESCRIPTION, BILL_ADJ.AMOUNT, BILL_ADJ.TRANS_DATE, BILL_ADJ.TRANSTYPE, BILL_ADJ.GROUPHEAD, CHAR(1) AS SERVICETYPE, CHAR(9) AS GROUPCODE, BILL_ADJ.TTYPE, BILL_ADJ.GHGROUPCODE, CHAR(5) AS EXTDESC, CHAR(5) AS ACCOUNTTYPE, CHAR(50) AS GHNAME, 0.00 AS BALBF, 0.00 AS DEBIT, 0.00 AS CREDIT FROM BILL_ADJ WHERE BILL_ADJ.TRANS_DATE >= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + "' AND BILL_ADJ.TRANS_DATE <= '" + Convert.ToDateTime(row["trans_date"]).ToShortDateString() + " 23:59:59.999' and BILL_ADJ.ghgroupcode = '" + row["groupcode"].ToString() + "' and BILL_ADJ.GROUPHEAD = '" + row["patientno"].ToString() + "'";

        //    DialogResult result = MessageBox.Show("OUTPUT TO SCREEN<yes>, TO PRINTER<no>, CANCEL", "Patient's Ledger On POS Format", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //    if (result == DialogResult.Cancel)
        //        return;
        //    bool isprint = result == DialogResult.No ? true : false;
        //    DataSet ds = new DataSet();
        //    dt = Dataaccess.GetAnytable("", "MR", xstring, false);
        //    foreach (DataRow xr in dt.Rows)
        //    {
        //        if (xr["ttype"].ToString() == "D")
        //            xr["debit"] = Convert.ToDecimal(xr["amount"]);
        //        else
        //            xr["credit"] = Convert.ToDecimal(xr["amount"]);
        //    }
        //    ds.Tables.Add(dt);
        //    Session["rdlcFile"] = "POSStatement.rdlc";
        //    Session["sql"] = "";
        //    string mrptheader = "POS Statement/RECEIPT GENERATOR ";
        //    string rptfooter = "", rptcriteria = "";

        //    if (!isprint)
        //    {
        //        MSMR.Forms.frmReportViewer receipt = new MSMR.Forms.frmReportViewer(mrptheader, mrptheader, rptfooter, rptcriteria, "", "POS", mreference, 0m, "", "", "", ds, true, 0, Convert.ToDateTime(row["trans_date"]), Convert.ToDateTime(row["trans_date"]), "", isprint, "", woperator);
        //        receipt.Show();
        //    }
        //    else
        //    {
        //        MSMR.MRrptConversion.GeneralRpt(mrptheader, mrptheader, rptfooter, "", "", "POS", mreference, 0M, "", "", "", ds, 0, Convert.ToDateTime(row["trans_date"]), Convert.ToDateTime(row["trans_date"]), "", isprint, true, "", woperator);
        //    }

        //}


    }
}

/*    string example = "abc5:!";
char[] myChars = example.ToCharArray();
foreach (char myChr in myChars)
{
if (!char.IsLetterOrDigit(myChr))
{
MessageBox.Show("This string is not an alphanumeric string.");
break;
}
}*/
