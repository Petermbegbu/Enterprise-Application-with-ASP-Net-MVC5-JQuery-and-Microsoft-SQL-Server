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
//using MRMENU2.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmBillings
    {
        MR_DATA.MR_DATAvm vm;

        billchaindtl bchain = new billchaindtl();
        Customer customers = new Customer();
        patientinfo patients = new patientinfo();
        Mrattend mrattend = new Mrattend();
        PleaseWaitForm pleaseWait = new PleaseWaitForm();
        Billings billv = new Billings();
        DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true),
            dtdocs = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' and STATUS = 'A' order by name", true),
            dttariff = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE, NAME FROM TARIFF  order by name", true),
            dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES  order by name", true), bills;
        Admrecs admrec = new Admrecs();
        string lookupsource, AnyCode, Anycode1, msection, mbill_cir,
                mgroupcode, mpatientno, mname, mgrouphtype, procedure, woperator, mreference;
        int itemno, recno;
        bool newrec, isposted, mcanadd, mcandelete, mcanalter;
        decimal amtsave, savedoldamt, mlastno;


        public frmBillings(string woperato, string section, MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;

            newrec = vm.REPORTS.newrec;
            mlastno = vm.REPORTS.mlastno;
            
            //msection = Session["section"].ToString(); // msmrfunc.mrGlobals.msection;
            msection = section;
            woperator = woperato; // msmrfunc.mrGlobals.WOPERATOR;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select wseclevel, CANDELETE, CANALTER, CANADD, section from mrstlev where operator = '" + woperator + "'", false);

            mcanadd = (bool)dt.Rows[0]["canadd"];
            mcanalter = (bool)dt.Rows[0]["canalter"];
            mcandelete = (bool)dt.Rows[0]["candelete"];
            if (dt.Rows[0]["section"].ToString().Length > 1)
                msection = dt.Rows[0]["section"].ToString().Substring(0, 1);

        }

        //private void frmBillings_Load(object sender, EventArgs e)
        //{
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", "select wseclevel, CANDELETE, CANALTER, CANADD, section from mrstlev where operator = '" + woperator + "'", false);

        //    mcanadd = (bool)dt.Rows[0]["canadd"];
        //    mcanalter = (bool)dt.Rows[0]["canalter"];
        //    mcandelete = (bool)dt.Rows[0]["candelete"];
        //    if (dt.Rows[0]["section"].ToString().Length > 1)
        //        msection = dt.Rows[0]["section"].ToString().Substring(0, 1);

        //    //initcomboboxes();
        //}

        //private void initcomboboxes()
        //{
        //    //get doc
        //    combDoc.DataSource = dtdocs;//medical staff details - doctors
        //    combDoc.ValueMember = "Reference";
        //    combDoc.DisplayMember = "Name";
        //    //get clinic
        //    combFacility.DataSource = dtfacility;
        //    combFacility.ValueMember = "Type_code";
        //    combFacility.DisplayMember = "name";
        //    //procedure
        //    combProc.DataSource = dttariff;
        //    combProc.ValueMember = "Reference";
        //    combProc.DisplayMember = "Name";
        //    //diagnosis
        //    combDiag.DataSource = dtdiag;
        //    combDiag.ValueMember = "Type_code";
        //    combDiag.DisplayMember = "name";
        //}

        //private void combConsAdmNo_Enter(object sender, EventArgs e)
        //{
        //    ClearControls("R");
        //    /*    
        //        if (string.IsNullOrWhiteSpace(AnyCode) &&
        //                new string[] { "7", "B", "C" }.Contains(msection)) //no lookup and msection $ "7BC" && bills
        //        {
        //            //get list of patients for final documentation of visit at billing
        //            DataTable dt = msmrfunc.getLinkDetails("",0,0m,0m,"",true,msection,0,"","");
        //            if (dt.Rows.Count > 0)
        //            {
        //                frmGetlinkinfo linkinfo = new frmGetlinkinfo(dt);
        //                linkinfo.ShowDialog();
        //                combConsAdmNo.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //                combConsAdmNo.Focus();
        //            }
        //        }
        //     */
        //}


        //private void combConsAdmNo_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(combConsAdmNo.Text))
        //        return;

        //    bool isopd = (chkopdrecords.Checked) ? true : false;

        //    if (string.IsNullOrWhiteSpace(AnyCode) && !string.IsNullOrWhiteSpace(combConsAdmNo.Text) &&
        //        (Char.IsDigit(combConsAdmNo.Text[0])))  //no lookup value obtained
        //    {
        //        combConsAdmNo.Text = bissclass.autonumconfig(combConsAdmNo.Text, true, (isopd) ? "C" : "A", "999999999");
        //    }

        //    itemno = 0;
        //    combRefNo.Text = combConsAdmNo.Text;

        //    if (!displaydetails(combConsAdmNo.Text))
        //    {
        //        combRefNo.Text = combConsAdmNo.Text = "";
        //        combConsAdmNo.Select();
        //        return;
        //    }

        //    combTransType.Focus();
        //}


        //void FrmLinkinfo_Closed(object sender, EventArgs e)
        //{
        //    frmlinkinfo FrmLinkinfo_Closed = sender as frmlinkinfo;
        //    {
        //        combConsAdmNo.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        combConsAdmNo.Focus();
        //    }
        //}

        //private void combPtNo_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(combPtNo.Text))
        //    {
        //        AnyCode = "";
        //        // txtgroupcode.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrWhiteSpace(AnyCode))  //no lookup value obtained
        //    {
        //        combPtNo.Text = bissclass.autonumconfig(combPtNo.Text, true, "", "9999999");
        //    }

        //    if (!DisplayPatDetails())
        //    {
        //        txtgroupcode.Text = combPtNo.Text = "";
        //        // txtgroupcode.Focus();
        //        return;
        //    }

        //    AnyCode = Anycode1 = "";
        //    combTransType.Focus();
        //    //btnsave.Enabled = true;
        //}

        //bool DisplayPatDetails()
        //{
        //    nmrCurrentTotal.Value = 0;
        //    //check if patientno exists
        //    bchain = billchaindtl.Getbillchain(combPtNo.Text, txtgroupcode.Text);
        //    if (bchain == null || string.IsNullOrWhiteSpace(bchain.PATIENTNO))
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Patient Number... ");
        //        return false;
        //    }
        //    mgrouphtype = bchain.GROUPHTYPE;
        //    bool xfoundit = true;
        //    if (mgrouphtype == "P")
        //    {
        //        patients = patientinfo.GetPatient(bchain.GROUPHEAD, bchain.GHGROUPCODE);
        //        if (patients == null)
        //            xfoundit = false;
        //    }
        //    else
        //    {
        //        customers = Customer.GetCustomer(bchain.GROUPHEAD);
        //        if (customers == null)
        //            xfoundit = false;
        //    }
        //    if (!xfoundit)
        //    {
        //        DialogResult result = MessageBox.Show("Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS");
        //        return false;
        //    }
        //    //     txtgroupheadName.Text = (string.IsNullOrWhiteSpace(mpatientno) || xreference.Substring(0, 1) == "S") ?
        //    //         txtgroupheadName.Text : (mgrouphtype == "P") ? patients.name : customers.Name;
        //    //     txtghgroupcode.Text = (mgrouphtype == "P") ? patients.groupcode : "";

        //    txtgroupcode.Text = bchain.GROUPCODE;
        //    combName.Text = bchain.NAME;
        //    mbill_cir = (mgrouphtype == "C") ? customers.Bill_cir : patients.bill_cir;
        //    txtgroupheadName.Text = (mgrouphtype == "P" && bchain.GROUPHEAD == patients.patientno) ?
        //        "< SELF >" : (mgrouphtype == "C") ? customers.Name : patients.name;
        //    txtghgroupcode.Text = (mgrouphtype == "P") ? patients.groupcode : "";
        //    combClientCd.Text = bchain.GROUPHEAD;

        //    if (mgrouphtype == "P" && bchain.PATIENTNO == patients.patientno)
        //    {
        //        txtAddress.Text = patients.address1.Trim() + "\r\n";
        //    }
        //    else
        //        txtAddress.Text = bchain.RESIDENCE;

        //    if (bchain.SECTION != "")
        //        txtAddress.Text = txtAddress.Text + bchain.SECTION + "\r\n";
        //    if (bchain.DEPARTMENT != "")
        //        txtAddress.Text = txtAddress.Text + bchain.DEPARTMENT.Trim() + "\r\n";
        //    if (bchain.STAFFNO != "")
        //        txtAddress.Text = txtAddress.Text + "[Staff # :" + bchain.STAFFNO.Trim() + " ]";
        //    combClientCd.Enabled = txtghgroupcode.Enabled = combName.Enabled = false;
        //    if (bchain.STATUS == "C" || bchain.STATUS == "S")
        //    {
        //        DialogResult result = MessageBox.Show("PATIENT NOT VALID FOR TRANSACTION - < Cancelled or Suspended >");
        //        return false;
        //    }
        //    return true;
        //}

        //bool displaydetails(string xreference)
        //{
        //    // msgeventtracker = "GH";
        //    string xtranstype = xreference.Substring(0, 1);

        //    if (xtranstype == "A") //admissions
        //    {
        //        admrec = Admrecs.GetADMRECS(xreference);
        //        if (admrec == null)
        //        {
        //            DialogResult result = MessageBox.Show("Invalid Admissions Reference... ", "IN-PATIENT PAYMENT");
        //            return false;
        //        }
        //    }
        //    else if (xtranstype == "C" || xtranstype == "S")
        //    {
        //        mrattend = Mrattend.GetMrattend(xreference);
        //        if (mrattend == null)
        //        {
        //            DialogResult result = MessageBox.Show("Unable to Link this Consultation Reference in Daily Attendance Register... ");
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Number Format for Consultation/Admission Reference...");
        //        return false;
        //    }

        //    txtghgroupcode.Text = (xtranstype == "A") ? admrec.GHGROUPCODE : mrattend.GHGROUPCODE;
        //    combClientCd.Text = (xtranstype == "A") ? admrec.GROUPHEAD : mrattend.GROUPHEAD;
        //    combName.Text = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;
        //    dttrans_date.Value = (xtranstype == "A") ? admrec.ADM_DATE : mrattend.TRANS_DATE;
        //    mgrouphtype = (xtranstype == "A") ? admrec.GROUPHTYPE : mrattend.GROUPHTYPE;
        //    mgroupcode = (xtranstype == "A") ? admrec.GROUPCODE : mrattend.GROUPCODE;
        //    mpatientno = (xtranstype == "A") ? admrec.PATIENTNO : mrattend.PATIENTNO;
        //    mname = (xtranstype == "A") ? admrec.NAME : mrattend.NAME;
        //    combFacility.Text = (xtranstype == "A") ? admrec.FACILITY : mrattend.CLINIC;
        //    combPtNo.Text = mpatientno;
        //    txtgroupcode.Text = mgroupcode;

        //    if (!DisplayPatDetails())
        //    {
        //        return false;
        //    }
        //    /*           bool xfoundit = true;
        //               if (mcusttype == "P")
        //               {
        //                   patients = patientinfo.GetPatient(combClientCd.Text);
        //                   if (patients == null)
        //                       xfoundit = false;
        //               }
        //               else
        //               {
        //                   customers = Customer.GetCustomer(combClientCd.Text);
        //                   if (customers == null)
        //                       xfoundit = false;
        //               }
        //               if (!xfoundit)
        //               {
        //                   MessageBox.Show("Unable to Link Patient's Account Information...'Check RESPONSIBLE FOR BILLS", msgBoxHandler);
        //               }
        //               txtgroupheadName.Text = (string.IsNullOrWhiteSpace(mpatientno) || xreference.Substring(0, 1) == "S") ?
        //                   txtgroupheadName.Text : (mcusttype == "P") ? patients.name : customers.Name;
        //               txtghgroupcode.Text = (mcusttype == "P") ? patients.groupcode : ""; */
        //    AnyCode = Anycode1 = "";
        //    displaybillinfo(xreference);
        //    return true;
        //}

        //private void combRefNo_Enter(object sender, EventArgs e)
        //{
        //    //combRefNo.Focus();
        //    if (string.IsNullOrWhiteSpace(AnyCode) && string.IsNullOrWhiteSpace(combConsAdmNo.Text)) //no lookup
        //    {
        //        mlastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, false, mlastno, false) + 1;
        //        combRefNo.Text = mlastno.ToString();
        //    }
        //    //dttrans_date.Value = DateTime.Now.Date;
        //}

        //private void combRefNo_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(combRefNo.Text))
        //    {
        //        txtgroupcode.Focus();
        //        return;
        //    }
        //    // msgeventtracker = "BREFF";
        //    AnyCode = Anycode1 = "";

        //    if (!bissclass.IsDigitsOnly(combRefNo.Text)) //combRefNo.Text.Substring(0, 1) == "A" || combRefNo.Text.Substring(0, 1) == "C" || combRefNo.Text.Substring(0, 1) == "S")
        //    {
        //        //do nothing
        //    }
        //    else if (bissclass.IsDigitsOnly(combRefNo.Text) && Convert.ToInt32(combRefNo.Text) > mlastno)
        //    {
        //        DialogResult result = MessageBox.Show("Bill Reference is out of Seguence...");
        //        combRefNo.Text = "";
        //        combRefNo.Select();
        //        return;
        //    }
        //    else
        //    {
        //        combRefNo.Text = bissclass.autonumconfig(combRefNo.Text, true, "", "999999999");
        //    }

        //    btnAdd.Enabled = true;
        //    mreference = combRefNo.Text;
        //    displaybillinfo(combRefNo.Text);
        //    nmrItmCnt.Focus();
        //}

        //void ClearControls(string xtype)
        //{
        //    txtgroupcode.Text = combPtNo.Text = combClientCd.Text = combName.Text = mgroupcode = mpatientno = mname = combFacility.Text = combDiag.Text = combDoc.Text = txtAddress.Text = txtgroupheadName.Text = combCurrency.Text = combRefNo.Text = txtghgroupcode.Text = txtCredLmt.Text = txtCurrntBal.Text = combTransType.Text = lblbillonaccount.Text =
        //        combProc.Text = txtOtherChrg.Text = "";
        //    dttrans_date.Value = DateTime.Now.Date;
        //    nmrCurrentTotal.Value = nmrAmount.Value = 0m;
        //    listView1.Items.Clear();
        //    if (xtype != "R")
        //        combConsAdmNo.Text = "";
        //}

        //private void btngroupcode_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btngroupcode")
        //    {
        //        txtgroupcode.Text = "";
        //        lookupsource = "g";
        //        msmrfunc.mrGlobals.crequired = "g";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnpatientlookup")
        //    {
        //        combPtNo.Text = "";
        //        lookupsource = "L";
        //        msmrfunc.mrGlobals.crequired = "L";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
        //    }
        //    else if (btn.Name == "btnAttendance")
        //    {
        //        string xrec = (chkopdrecords.Checked) ? "RECORDED DAILY ATTENDANCE" : "ADMISSION RECORDS";
        //        combRefNo.Text = "";
        //        lookupsource = (chkopdrecords.Checked) ? "I" : "A";
        //        msmrfunc.mrGlobals.crequired = (chkopdrecords.Checked) ? "I" : "A";
        //        msmrfunc.mrGlobals.lookupCriteria = chkopdrecords.Checked && chkTodaysConsult.Checked ? "C" : "";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR " + xrec;
        //    }
        //    else if (btn.Name == "btnbillreference")
        //    {
        //        combRefNo.Text = "";
        //        lookupsource = "B";
        //        msmrfunc.mrGlobals.crequired = "BILL";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR SAVED BILLS";
        //    }
        //    else if (btn.Name == "btndesc")
        //    {
        //        combProc.Text = procedure = "";
        //        lookupsource = "SD";
        //        msmrfunc.mrGlobals.crequired = "SD"; //SERVICE DESCRIPTIONS
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR [ALL] DEFINED INVESTIGATIONS/PROCEDURES";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "g") //groupcodee
        //    {
        //        txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        combPtNo.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //        txtgroupcode.Focus();
        //    }
        //    else if (lookupsource == "L") //patientno
        //    {
        //        txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        combPtNo.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
        //        combPtNo.Focus();
        //    }
        //    else if (lookupsource == "I" || lookupsource == "A") //daiy attendance/admrecs
        //    {
        //        msmrfunc.mrGlobals.lookupCriteria = "";
        //        combConsAdmNo.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        combConsAdmNo.Focus();
        //    }
        //    else if (lookupsource == "B") //BILLS
        //    {
        //        combRefNo.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        combRefNo.Focus();
        //    }
        //    else if (lookupsource == "SD") //service desc
        //    {
        //        combProc.DropDownStyle = Gizmox.WebGUI.Forms.ComboBoxStyle.DropDown;
        //        combProc.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        procedure = msmrfunc.mrGlobals.anycode1;
        //        combProc.DropDownStyle = Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList;
        //        combProc.Focus();
        //    }
        //}

        //void displaybillinfo(string xreference)
        //{
        //    string xname = "", xpatientno = "";
        //    itemno = 0;
        //    nmrCurrentTotal.Value = 0m;
        //    string[] arr = new string[13];
        //    ListViewItem itm;
        //    listView1.Items.Clear();
        //    bills = Billings.GetBILLING(xreference);
        //    if (bills.Rows.Count < 1)
        //        return;
        //    mreference = xreference;
        //    //  for (int i = 0; i < bills.Rows.Count; i++)
        //    foreach (DataRow row in bills.Rows)
        //    {
        //        itemno++;
        //        nmrCurrentTotal.Value += Convert.ToDecimal(row["amount"]);
        //        xname = row["name"].ToString();
        //        xpatientno = row["patientno"].ToString();

        //        arr[0] = itemno.ToString(); // row["itemno"].ToString();
        //        arr[1] = row["facility"].ToString();
        //        arr[2] = row["DESCRIPTION"].ToString();
        //        arr[3] = row["amount"].ToString();
        //        arr[4] = row["process"].ToString();
        //        arr[5] = Convert.ToDateTime(row["trans_date"]).ToShortDateString();
        //        arr[6] = row["diag"].ToString().Trim();
        //        arr[7] = row["doctor"].ToString();
        //        arr[8] = row["accounttype"].ToString(); //Outpatient ; Inpatient
        //        arr[9] = row["servicetype"].ToString();     //Claim/Fee for service or blank for capitation(HMO-NHIS) 
        //        arr[10] = "NO"; // new or old record
        //        arr[11] = (Convert.ToBoolean(row["posted"])) ? "YES" : "NO"; //posted or not
        //        arr[12] = row["recid"].ToString();
        //        //   itemno++;
        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);
        //        savedoldamt = Convert.ToDecimal(arr[3]);
        //    }
        //    if (combName.Text.Trim() != xname.Trim() || combPtNo.Text.Trim() != xpatientno.Trim())
        //    {
        //        DialogResult result = MessageBox.Show("Invoice item not for this Patient...Check " + xpatientno + " CONTINUE ?", "Invoice Matching Error...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.No)
        //        {
        //            ClearControls("R");
        //            txtgroupcode.Select();
        //            return;
        //        }
        //        if (!string.IsNullOrWhiteSpace(combPtNo.Text) && xpatientno != combPtNo.Text)
        //        {
        //            DialogResult result1 = MessageBox.Show("Confirm to Change Ownership of Bill...", "Bill Ownership Conflict",
        //                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            if (result1 == DialogResult.No)
        //            {
        //                txtgroupcode.Select();
        //                return;
        //            }
        //        }
        //    }
        //}

        //void renumberview()
        //{
        //    for (int i = 0; i < listView1.Items.Count; i++)
        //    {
        //        listView1.Items[i].SubItems[0].Text = (i + 1).ToString();
        //    }
        //}

        //private void nmrItmCnt_Enter(object sender, EventArgs e)
        //{
        //    nmrItmCnt.Value = itemno + 1;
        //    nmrItmCnt.Select(0, nmrItmCnt.Text.Length);
        //}

        //private void nmrItmCnt_LostFocus(object sender, EventArgs e)
        //{
        //    // msgeventtracker = "BI";
        //    if (nmrItmCnt.Value > itemno + 1)
        //    {
        //        DialogResult result = MessageBox.Show("Value is out of Sequence...", "Biling Item Count");
        //    }

        //    if (nmrItmCnt.Value < 1)
        //    {
        //        combRefNo.Focus();
        //        return;
        //    }

        //    bool foundit = false;
        //    newrec = true;
        //    if (listView1.Items.Count >= 1) //check if record exist
        //    {
        //        for (int i = 0; i < listView1.Items.Count; i++)
        //        {
        //            if (listView1.Items[i].SubItems[0].Text.Trim() == nmrItmCnt.Value.ToString())
        //            {
        //                foundit = true;
        //                recno = i;
        //                newrec = false;
        //                bissclass.displaycombo(combFacility, dtfacility, listView1.Items[i].SubItems[1].ToString(), "type_code");
        //                txtOtherChrg.Text = listView1.Items[i].SubItems[2].ToString();
        //                nmrAmount.Value = Convert.ToDecimal(listView1.Items[i].SubItems[3].ToString());
        //                bissclass.displaycombo(combProc, dttariff, listView1.Items[i].SubItems[4].ToString(), "reference");
        //                dttrans_date.Value = Convert.ToDateTime(listView1.Items[i].SubItems[5].ToString());
        //                bissclass.displaycombo(combDiag, dtdiag, listView1.Items[i].SubItems[6].ToString(), "type_code");
        //                bissclass.displaycombo(combDoc, dtdocs, listView1.Items[i].SubItems[7].ToString(), "reference");
        //                isposted = listView1.Items[i].SubItems[11].ToString().Trim() == "YES" ? true : false;
        //                break;
        //            }
        //        }
        //        if (foundit)
        //        {
        //            string msg = combRefNo.Text.Substring(0, 1) == "S" && bchain.GROUPHTYPE != "C" ? "\r\n\r\n It's Special Service Bill... Can't be modified" : isposted ? "Its Confirmed... Cannot be Amended" : "";
        //            DialogResult result = MessageBox.Show("Record Exist...." + msg, "BILLS");
        //            if (isposted)
        //            {
        //                nmrItmCnt.Select();
        //                return;
        //            }
        //            else if (combRefNo.Text.Substring(0, 1) == "S" && bchain.GROUPHTYPE == "P")
        //            {
        //                combRefNo.Text = "";
        //                txtgroupcode.Focus();
        //                return;
        //            }
        //        }
        //    }
        //    btnAdd.Enabled = true;
        //    dttrans_date.Focus();
        //}

        //private void combProc_LostFocus(object sender, EventArgs e)
        //{
        //    // msgeventtracker = "SD";
        //    string xdesc = "";
        //    if (string.IsNullOrWhiteSpace(combProc.Text))
        //        return;

        //    procedure = (string.IsNullOrWhiteSpace(procedure)) && !string.IsNullOrWhiteSpace(combProc.Text) ? combProc.SelectedValue.ToString() : procedure;
        //    txtOtherChrg.Text = procedure;
        //    string facility = "";

        //    if (string.IsNullOrWhiteSpace(procedure))
        //        return;

        //    nmrAmount.Value = amtsave = msmrfunc.getFeefromtariff(procedure, bchain.PATCATEG, ref xdesc, ref facility);

        //    if (string.IsNullOrWhiteSpace(xdesc)) //not found in tariff file
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Tariff Reference", "TARIFF OF CHARGES");
        //    }
        //    txtOtherChrg.Text = xdesc;

        //    if (combFacility.SelectedValue == null)
        //    {
        //        combFacility.SelectedValue = facility;
        //        combFacility.Text = bissclass.combodisplayitemCodeName("type_code", facility, dtfacility, "name");
        //    }
        //    if (chkStkItmForDrgBill.Checked && combProc.Text.Trim().Contains("DRUG"))
        //    {
        //        frmStkitemsonbill stkitems = new frmStkitemsonbill(combRefNo.Text);
        //        stkitems.ShowDialog();
        //        Array itema_;
        //        if (!string.IsNullOrWhiteSpace(msmrfunc.mrGlobals.anycode1))
        //        {
        //            nmrAmount.Value = Convert.ToDecimal(msmrfunc.mrGlobals.anycode1);
        //            itema_ = (Array)Session["stkitems"];
        //            txtOtherChrg.Enabled = false;
        //            nmrAmount.ReadOnly = true;
        //            btnAdd.Focus();
        //        }
        //        else
        //        {
        //            txtOtherChrg.Enabled = true;
        //            nmrAmount.ReadOnly = false;
        //        }
        //    }
        //    nmrAmount.Focus();
        //}

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    if (nmrAmount.Value == 0)
        //        return;
        //    DialogResult result;
        //    if (nmrAmount.Value < 1)
        //    {
        //        result = MessageBox.Show("You have specified a negative value... CONFIRM TO CONTINUE...", "EXCEPTIONAL VALUE ALERT !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //        if (result == DialogResult.No)
        //        {
        //            nmrAmount.Value = 0;
        //            return;
        //        }
        //    }

        //    if (amtsave > 0 && nmrAmount.Value < amtsave)
        //    {
        //        if (bchain.GROUPHTYPE == "P" || msection != "7") //billing
        //        {
        //            result = MessageBox.Show("Specified Amount is less than control value from Service Tariff...", "BILLING AMOUNT");
        //            return;
        //        }
        //    }

        //    if (string.IsNullOrWhiteSpace(txtOtherChrg.Text))
        //    {
        //        result = MessageBox.Show("Invalid Record... Check amount/transaction desc etc.");
        //        return;
        //    }

        //    if (!bissclass.IsPresent(combName, "Patients Name", false) ||
        //        !bissclass.IsPresent(this.combTransType, "Transaction Type", true) ||
        //        !bissclass.IsPresent(this.combClientCd, "Who Pays the Bill", false) ||
        //        !bissclass.IsPresent(this.combRefNo, "Transaction Reference", false) ||
        //        !bissclass.IsPresent(this.combFacility, "Facility/Service Centre Id", true) ||
        //        !bissclass.IsPresent(this.txtOtherChrg, "Description of Service", false) ||
        //        !bissclass.IsPresent(this.dttrans_date, "Transaction Date", false))
        //    {
        //        return;
        //    }

        //    string servicetype = "";
        //    if (!newrec)
        //    {
        //        servicetype = listView1.Items[recno].SubItems[9].ToString();
        //        nmrCurrentTotal.Value = nmrCurrentTotal.Value - savedoldamt;

        //        //counter = newcounter DISPLAY AMOUNT WITH COMMA
        //        ListViewItem xitm = listView1.Items[recno];
        //        xitm.SubItems[1].Text = string.IsNullOrWhiteSpace(combFacility.Text) ? "" : combFacility.SelectedValue.ToString();
        //        xitm.SubItems[2].Text = txtOtherChrg.Text;
        //        xitm.SubItems[3].Text = nmrAmount.Value.ToString("N2");
        //        xitm.SubItems[4].Text = string.IsNullOrWhiteSpace(combProc.Text) ? "" : combProc.SelectedValue.ToString();
        //        xitm.SubItems[5].Text = dttrans_date.Value.ToShortDateString();
        //        xitm.SubItems[6].Text = string.IsNullOrWhiteSpace(combDiag.Text) ? "" : combDiag.SelectedValue.ToString();
        //        xitm.SubItems[7].Text = string.IsNullOrWhiteSpace(combDoc.Text) ? "" : combDoc.SelectedValue.ToString();
        //        nmrCurrentTotal.Value += nmrAmount.Value;
        //    }
        //    else
        //    {
        //        itemno++;
        //        //9.8.2017 we must array items from stkitemonbill to listview for this item
        //        nmrCurrentTotal.Value += nmrAmount.Value;
        //        string[] row = { nmrItmCnt.Value.ToString(), string.IsNullOrWhiteSpace(combFacility.Text) ? "" : combFacility.SelectedValue.ToString(), txtOtherChrg.Text, nmrAmount.Value.ToString("N2"), procedure, dttrans_date.Value.ToShortDateString(), string.IsNullOrWhiteSpace(combDiag.Text) ? "" : combDiag.SelectedValue.ToString(), string.IsNullOrWhiteSpace(combDoc.Text) ? "" : combDoc.SelectedValue.ToString(), combTransType.Text, (newrec) ? "" : servicetype, "YES", "NO", "0" };
        //        ListViewItem itm;
        //        itm = new ListViewItem(row);
        //        listView1.Items.Add(itm);
        //    }

        //    //   DateTime xdate = Convert.ToDateTime(listView1.Items[0].SubItems[5].ToString().Trim());
        //    /* if (!newrec)
        //     {
        //         renumberview();
        //     }*/
        //    btnAdd.Enabled = false;
        //    nmrItmCnt.Focus();
        //    return;
        //}

        public MR_DATA.REPORTS btnSave_Click(IEnumerable<MR_DATA.PPDRESSINGDTL> tableList)
        {
            //DialogResult result;
            //if (listView1.Items.Count == 0)
            //{
            //    result = MessageBox.Show("No Records generated...", "Patient Billing Details");
            //    combRefNo.Focus();
            //    return;
            //}

            if (newrec && !mcanadd || !newrec && !mcanalter)
            {
                string xstr = vm.REPORTS.newrec ? "To New Record Creation" : "To Record Alteration";
                vm.REPORTS.alertMessage = "ACCESS DENIED... " + xstr + "...  See your Systems Administator!";
                return vm.REPORTS;
            } 

            //  msgeventtracker = "RS";
            //result = MessageBox.Show("Confirm to Submit ALL Records...", "Patient Billing Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.Yes)

            savedetails(tableList);

            return vm.REPORTS;
        }

        private void writeBILLS(bool xnewrec, string xreference, decimal xitem, string xprocess, string xdescription,
            string xgrouphtype, decimal xamount, DateTime xdate, string xname, string xgrouphead, string xfacility, string xgroupcode, string xpatientno, string debitcredit_CD, string xghgroupcod, string xoperator, DateTime xop_time, string xextdesc, string xcurrency, decimal xexrate, int xfxtype, string xdiag, string xdoctor, bool xposted, string xpayref, string xservicetyp, decimal xpayment, string xpaytype, decimal xfcamount, string in_outpatient, bool receipted, int recid)
        {
            //DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;

            DateTime dtmin_date = DateTime.Now;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (xservicetyp == "b") ? "capbills_Add" : (xnewrec) ? "Billing_Add" : "Billing_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Reference", xreference);
            insertCommand.Parameters.AddWithValue("@patientno", xpatientno);
            insertCommand.Parameters.AddWithValue("@name", xname);
            insertCommand.Parameters.AddWithValue("@Itemno", xitem);
            insertCommand.Parameters.AddWithValue("@diag", xdiag);
            insertCommand.Parameters.AddWithValue("@process", xprocess);
            insertCommand.Parameters.AddWithValue("@description", xdescription);
            insertCommand.Parameters.AddWithValue("@doctor", xdoctor);
            insertCommand.Parameters.AddWithValue("@facility", xfacility);
            insertCommand.Parameters.AddWithValue("@amount", xamount);
            insertCommand.Parameters.AddWithValue("@trans_date", xdate);
            insertCommand.Parameters.AddWithValue("@sec_level", 0m);
            insertCommand.Parameters.AddWithValue("@posted", (xnewrec) ? false : xposted);
            insertCommand.Parameters.AddWithValue("@post_date", dtmin_date);
            insertCommand.Parameters.AddWithValue("@receipted", receipted);
            insertCommand.Parameters.AddWithValue("@transtype", xgrouphtype);
            insertCommand.Parameters.AddWithValue("@payref", xpayref);
            insertCommand.Parameters.AddWithValue("@grouphead", xgrouphead);
            insertCommand.Parameters.AddWithValue("@servicetype", xservicetyp);
            insertCommand.Parameters.AddWithValue("@payment", xpayment);
            insertCommand.Parameters.AddWithValue("@groupcode", xgroupcode);
            insertCommand.Parameters.AddWithValue("@ttype", debitcredit_CD);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", xghgroupcod);
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@op_time", xop_time);
            insertCommand.Parameters.AddWithValue("@currency", xcurrency);
            insertCommand.Parameters.AddWithValue("@exrate", xexrate);
            insertCommand.Parameters.AddWithValue("@fcamount", xfcamount);
            insertCommand.Parameters.AddWithValue("@extdesc", xextdesc);
            insertCommand.Parameters.AddWithValue("@Accounttype", xservicetyp); // in_outpatient);
            if (!xnewrec)
                insertCommand.Parameters.AddWithValue("@RECID", recid);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                //return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                vm.REPORTS.alertMessage = "SQL access" + ex;

                //MessageBox.Show("SQL access" + ex, "BILLINGS UPDATE", MessageBoxButtons.OK,
                //    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                return;
            }
            finally
            {
                connection.Close();
            }

        }

        void savedetails(IEnumerable<MR_DATA.PPDRESSINGDTL> tableList)
        {
            if (newrec && bissclass.IsDigitsOnly(vm.REPORTS.txtreference.Trim())) // (combRefNo.Text.Substring(0, 1) == "A" || combRefNo.Text.Substring(0, 1) == "C")) //update ref. counter and get a new value, if necessary.
            {
                decimal lastnosave = mlastno;
                vm.REPORTS.mlastno = msmrfunc.getcontrol_lastnumber("ALTERNATENO", 1, true, mlastno, false);

                if (vm.REPORTS.mlastno != lastnosave)
                    vm.REPORTS.txtreference = bissclass.autonumconfig(vm.REPORTS.mlastno.ToString(), true, "", "999999999");

            }

            mreference = vm.REPORTS.txtreference;
            //foreach  (int i = 0; i < listView1.Items.Count; i++)  //GET DETAILS FOR BILLLING FILE

            foreach(var eachRow in tableList)
            {
                writeBILLS(
                    eachRow.GROUPHEAD == "YES" ? true : false, vm.REPORTS.txtreference, 
                    Convert.ToDecimal(eachRow.TITLE), eachRow.PROCESS, eachRow.DESCRIPTION, 
                    mgrouphtype, Convert.ToDecimal(eachRow.GROUPCODE), 
                    Convert.ToDateTime(eachRow.ADDRESS1).Date, vm.REPORTS.TXTPATIENTNAME, 
                    vm.REPORTS.cbotitle, eachRow.FACILITY, vm.REPORTS.txtgroupcode, 
                    vm.REPORTS.txtpatientno, "D", vm.REPORTS.txtghgroupcode, woperator, 
                    DateTime.Now, "", "", 0m, 0, eachRow.NOTES, eachRow.DOCTOR, false, "", 
                    eachRow.SERVICETYPE, 0m, "", 0m, vm.REPORTS.cbotype, false, 
                    Convert.ToInt32(eachRow.RECID)
                );
            }

            vm.REPORTS.alertMessage = "Saved Succesfullly";

            //foreach (ListViewItem itm in listView1.Items)
            //{
            //    Billings.writeBILLS(itm.SubItems[10].ToString().Trim() == "YES" ? true : false, vm.REPORTS.txtreference,
            //        Convert.ToDecimal(itm.SubItems[0].ToString().Trim()), itm.SubItems[4].ToString(), itm.SubItems[2].ToString(), mgrouphtype, Convert.ToDecimal(itm.SubItems[3].ToString().Trim()), Convert.ToDateTime(itm.SubItems[5].ToString()).Date, combName.Text, combClientCd.Text, itm.SubItems[1].ToString(), txtgroupcode.Text, combPtNo.Text, "D", txtghgroupcode.Text, woperator, DateTime.Now, "", "", 0m, 0, itm.SubItems[6].ToString(), itm.SubItems[7].ToString(), false, "", itm.SubItems[9].ToString(), 0m, "", 0m, combTransType.Text, false, Convert.ToInt32(itm.SubItems[12].ToString().Trim()));
            //}

            //combConsAdmNo.Focus();
            return;
        }

        //private void btnExit_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void combTransType_LostFocus(object sender, EventArgs e)
        //{
        //    if (combTransType.Text.Trim() == "REVERSAL")
        //    {
        //        if (!processReversal())
        //        {
        //            combTransType.Text = "";
        //            //combTransType.Select();
        //            return;
        //        }
        //    }

        //    //combRefNo.Focus();

        //}

        //bool processReversal()
        //{
        //    /* if (!reversal_access) // REVERSAL/debit/credit note
        //     {
        //         DialogResult result = MessageBox.Show("Access Denied... See your Systems Administrator! ", "Billing Reversal");
        //         return false;
        //     }*/
        //    // msgeventtracker = "REVERSAL";
        //    DialogResult result1 = MessageBox.Show("Transactions Reversal...\r\n" + " ...Overwrite required !  CONTINUE... ?", "Invoice reversal", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    if (result1 == DialogResult.No)
        //        return false;
        //    if (getOverwrite("R"))
        //        return true;
        //    return false;
        //}

        //private bool getOverwrite(string xtype)
        //{
        //    frmOverwrite overwrite = new frmOverwrite(xtype == "R" ? "OVERWRITE TO PATIENTS BILLS REVERSAL" :
        //        "Overwrite to Date Control", "MRSTLEV", "MR");
        //    overwrite.Closed += new EventHandler(Frmoverwrite_Closed);
        //    overwrite.ShowDialog();
        //    if (string.IsNullOrWhiteSpace(AnyCode))
        //        return false;
        //    return true;
        //}

        //void Frmoverwrite_Closed(object sender, EventArgs e)
        //{
        //    frmOverwrite overwrite_Closed = sender as frmOverwrite;

        //    AnyCode = bissclass.sysGlobals.anycode;
        //    return;
        //}

        //private void combProc_Enter(object sender, EventArgs e)
        //{
        //    procedure = "";
        //    if (chkStkItmForDrgBill.Checked)
        //    {
        //        DialogResult result = MessageBox.Show("You must specify DRUGS/INJECTION HERE to Load Drugs Details...");
        //    }
        //}

        //private void nmrAmount_Enter(object sender, EventArgs e)
        //{
        //    NumericUpDown nmr = sender as NumericUpDown;
        //    nmr.Select(0, nmr.Text.Length);
        //    //nmrAmount.Select(0, nmrAmount.Text.Length);
        //}

        //private void combName_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtgroupcode.Text) && string.IsNullOrWhiteSpace(combPtNo.Text) && !string.IsNullOrWhiteSpace(combName.Text))
        //        combClientCd.Enabled = txtghgroupcode.Enabled = true;
        //}

        //private void btnPrintBills_Click(object sender, EventArgs e)
        //{
        //    rptfrmBillings bills = new rptfrmBillings(5, mreference, "", "");
        //    bills.Show();
        //}

        //private void chkopdrecords_Click(object sender, EventArgs e)
        //{
        //    if (!chkopdrecords.Checked)
        //        chkTodaysConsult.Visible = false;
        //    else
        //        chkTodaysConsult.Visible = true;
        //}

        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (nmrItmCnt.Value < 1 || Convert.ToInt32(nmrItmCnt.Value) > listView1.Items.Count)
        //        return;
        //    DialogResult result;
        //    //if (!mcandelete || bchain.GROUPHTYPE == "P")
        //    //{
        //    //    string xstr = !mcandelete ? "Delete Privilege" : "Private Bill - Use Reversal or Transfer Option";
        //    //    result = MessageBox.Show("Access Denied..." + xstr);
        //    //  //  return;
        //    //}
        //    result = MessageBox.Show("Confirm To Delete Bill...?", "Delete Bill", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    if (result == DialogResult.No)
        //        return;
        //    // int xitm = Convert.ToInt32(nmrItmCnt.Value);
        //    if (Convert.ToInt32(listView1.Items[recno].SubItems[12].Text.Trim()) > 0)
        //    {
        //        //string updatestr = "delete from billing where reference = '" + combRefNo.Text + "' and itemno = '" + listView1.Items[recno].SubItems[0].ToString() + "' and rtrim(DESCRIPTION) = '" + listView1.Items[recno].SubItems[2].ToString().Trim() + "'";
        //        string updatestr = "delete from billing where recid = '" + listView1.Items[recno].SubItems[12].ToString().Trim() + "'";
        //        bissclass.UpdateRecords(updatestr, "MR");
        //    }

        //    MessageBox.Show("Record Deleted...");
        //    renumberview();
        //    listView1.Items[recno].Remove();

        //}

        //===================================================================
        //private void chkTrnsfBill_Click(object sender, EventArgs e)
        //{
        //    if (nmrCurrentTotal.Value < 1 || bills.Rows.Count < 1 || msection != "7")
        //    {
        //        string xs = msection != "7" ? "A Non-Billing Staff Cannot Transfer Bills..." : "No Value To Transfer...";
        //        MessageBox.Show(xs);
        //        return;
        //    }

        //    frmBillTransfer billtransfer = new frmBillTransfer(nmrCurrentTotal.Value, bills, woperator);
        //    billtransfer.Show();
        //}

        //private void btnAllBills_Click(object sender, EventArgs e)
        //{
        //    frmLoadBills4Edit loadbills = new frmLoadBills4Edit(woperator);
        //    loadbills.Show();
        //}

        //=============================================

        //private void combProc_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    /*if (combProc.SelectedItem == null || string.IsNullOrWhiteSpace(combProc.Text))
        //        return;
        //    combProc_LostFocus(null, null);*/
        //}

        //private void nmrAmount_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 13) // (Char)13 );
        //        btnAdd.Focus();
        //}

        //private void combConsAdmNo_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    //e..SuppressKeyPress = true;
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void txtgroupcode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    SelectNextControl(ActiveControl, true, true, true, true);
        //}

        //private void combPtNo_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    combPtNo_LostFocus(null, null);
        //}

        //private void combName_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    combName_LostFocus(null, null);
        //}

        //private void combRefNo_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    combRefNo_LostFocus(null, null);
        //}

        //private void combConsAdmNo_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    combConsAdmNo_LostFocus(null, null);
        //}
    }
}

