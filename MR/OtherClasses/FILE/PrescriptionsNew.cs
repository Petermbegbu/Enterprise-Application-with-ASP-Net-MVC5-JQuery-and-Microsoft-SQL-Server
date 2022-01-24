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

using mradmin.DataAccess;
using mradmin.BissClass;
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class PrescriptionsNew
    {
        //PleaseWaitForm pleaseWait = new PleaseWaitForm();
        //custclass CustClass = new custclass();
        DataTable CustClass = custclass.GetCUSTCLASS(),
           dtRoutinDrg = Dataaccess.GetAnytable("", "MR", "select DISTINCT reference FROM routdrgs ORDER BY reference", true),
           dtGrpDrgChargRate = Dataaccess.GetAnytable("", "MR", "select reference,rate,oncost from GROUPDRGCHARGE", false);
        billchaindtl phbchain = new billchaindtl();
        dgprofile Drgprofile = new dgprofile();
        billchaindtl BillOnAcct = new billchaindtl();
        Customer customer = new Customer();
        //nhisdescription,msgeventtracker,
        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start, mtrans_date, dtlastattend;
        string mdrugsonbill, nhisfac_code, nhisbillcode, phamalterstr = "", mfacility, mstkcode, txtdose, mdrugcode, 
            woperator, anycode = "", groupcode, patientno, reference, mdoctor, msection, unitIDsave, pharmacyStore;
        Decimal fxtype, autoremind_period, mdose, minterval, mduration;
        bool nhisgentariff, mallowexp, iscapitated, fee_for_service, isconvertable, isnhischarge, foundit, newrec, inpatient,
            mtth, isanc, cashpaying, onNmrDose, markupdrgbillonPercentage, alertforConsumables;
        decimal savedstksellamount, mcumgv, mdrgmarkup = 0m, medhistupdateallowed, countersave, counter, packqty, Stkstrength, stkper, presctnIntValidation;
        int recno = 0, reqAlertCount = 0;
        string[] requestalert = new string[5];

        MR_DATA.MR_DATAvm vm;

        //public PrescriptionsNew(string xgroupcode, string xpatientno, string xreference, string xfacility, 
        //DateTime xtrans_date, bool isinpatient, bool ismtth, string xdoctor, bool xisanc, bool iscashpaying, 
        //string xoperator, string xsection)
        //{
        //	//InitializeComponent();

        //	if (ismtth)
        //	{
        //		this.Text += "  (TTH)";
        //	}

        //	reference = xreference; groupcode = xgroupcode; patientno = xpatientno; woperator = xoperator; msection = xsection;
        //	mtrans_date = xtrans_date; inpatient = isinpatient; mtth = ismtth; mdoctor = xdoctor;   isanc = xisanc; cashpaying = iscashpaying;

        //   //  initcomboboxes();
        //	for (int i = 0; i < 5; i++)
        //	{
        //		requestalert[i] = "";
        //	}
        //}

        public PrescriptionsNew(MR_DATA.MR_DATAvm VM2, string xoperator)
        {
            //InitializeComponent();

            //if (ismtth)
            //{
            //    this.Text += "  (TTH)";
            //}

            //reference = xreference; groupcode = xgroupcode; patientno = xpatientno; woperator = xoperator; msection = xsection;
            //mtrans_date = xtrans_date; inpatient = isinpatient; mtth = ismtth; mdoctor = xdoctor; isanc = xisanc; cashpaying = iscashpaying;

            //  initcomboboxes();

            vm = VM2;
            woperator = xoperator;

            getcontrolsettings();

            for (int i = 0; i < 5; i++)
            {
                requestalert[i] = "";
            }
        }

        //private void PrescriptionsNew_Load(object sender, EventArgs e)
        //{
        //    Session["Inpatient"] = "N";
        //    getcontrolsettings();
        //    //initcomboboxes();

        //    phbchain = billchaindtl.Getbillchain(patientno, groupcode);
        //    loadprevDefinitions();

        //    if (phbchain.GROUPHTYPE == "C")
        //        customer = Customer.GetCustomer(phbchain.GROUPHEAD);

        //    Session["inp2medhist"] = "";
        //    onNmrDose = alertforConsumables = false;
        //    //get lastattendance date
        //    dtlastattend = DateTime.Now.Date;
        //    Medhrec medhrec = Medhrec.GetMEDHREC(phbchain.GROUPCODE, phbchain.PATIENTNO);

        //    if (medhrec != null)
        //        dtlastattend = medhrec.DATE5.Date; //.ToShortDateString() + "  @ " + medhrec.DATE5.ToShortTimeString();

        //    if (phbchain.GROUPHTYPE == "C") //&& customer.HMO ) ALL CORPORATE
        //        chkPrivateAcct.Enabled = true;
        //}

        //private void initcomboboxes()
        //{
        //	/*get stock
        //	comboDescription.DataSource = selcode.getsyscodes("stk");
        //	comboDescription.ValueMember = "Item";
        //	comboDescription.DisplayMember = "Name";*/

        //	//cboDescription.DataSource = dtstock;
        //	//cboDescription.ValueMember = "Item";
        //	//cboDescription.DisplayMember = "Name";

        //	cboRoutineDrgs.DataSource = dtRoutinDrg;
        //	cboRoutineDrgs.ValueMember = "reference";
        //	cboRoutineDrgs.DisplayMember = "reference";
        //}

        private void getcontrolsettings()
        {
            DataTable dt = Dataaccess.GetAnytable("", "MR",
                "SELECT glbatchno, ecgno, dischtime, othercharg, installed, pvtcode, dischtime, serial, fsh, FESTLEVPAS from mrcontrol order by recid", false);

            // mdrugcode = dt.Rows[0]["mdrugcode"].ToString();
            pharmacyStore = dt.Rows[0]["serial"].ToString().Trim();
            //if (string.IsNullOrWhiteSpace(pharmacyStore))
            //    dtstock = Dataaccess.GetAnytable("", "SMS", "select DISTINCT name, item from stock order by name ", true);
            //else
            //    dtstock = Dataaccess.GetAnytable("", "SMS", "select name, item from stock where rtrim(store) = '" + pharmacyStore + "' order by name ", true);

            fxtype = (Decimal)dt.Rows[1]["glbatchno"];
            mdrgmarkup = (Decimal)dt.Rows[1]["ecgno"];
            markupdrgbillonPercentage = Convert.ToBoolean(dt.Rows[1]["fsh"]);

            mfacility = dt.Rows[2]["dischtime"].ToString();
            medhistupdateallowed = (Decimal)dt.Rows[2]["glbatchno"];

            mdrugsonbill = dt.Rows[3]["othercharg"].ToString();

            nhisgentariff = (bool)dt.Rows[5]["installed"]; //use gen tariff to nhis outpatient consult

            nhisfac_code = dt.Rows[8]["pvtcode"].ToString();
            nhisbillcode = dt.Rows[8]["dischtime"].ToString().Substring(0, 7);
            isnhischarge = (bool)dt.Rows[8]["othercharg"];
            presctnIntValidation = (decimal)dt.Rows[2]["glbatchno"];

            DataTable dtsms = Dataaccess.GetAnytable("", "SMS",
                "SELECT allowexp, enqno from smcontrol where recid = '1'", false);

            mallowexp = (bool)dtsms.Rows[0]["allowexp"];
            autoremind_period = (Decimal)dtsms.Rows[0]["enqno"];

            ////local settings
            //if (inpatient)
            //{
            //    chkinpatient.Checked = true;
            //    chkinpatient.Enabled = btnConvert.Enabled = false;
            //    this.Text = "In-Patient Prescriptions Management";
            //    chktth.Checked = (mtth) ? true : false;
            //    btninpatprescdtl.Enabled = true;
            //}

            DataTable dtmrsetup = Dataaccess.GetAnytable("", "MR", "Select drugcode from mrsetup order by facility", false);
            mdrugcode = dtmrsetup.Rows[0]["drugcode"].ToString();
        }

        //private void btnstock_Click(object sender, EventArgs e)
        //{
        //    //this.txtreference.Text = "";
        //    msmrfunc.mrGlobals.lookupCriteria = pharmacyStore;
        //    // mstkcode = "";
        //    msmrfunc.mrGlobals.crequired = "s";
        //    msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED STOCK IN PHARMACY";
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    {
        //        anycode = msmrfunc.mrGlobals.anycode1;
        //        txtDescription.Text = msmrfunc.mrGlobals.anycode;
        //        //.bissclass.displaycombo(cboDescription, dtstock, anycode, "item");
        //        //   this.cboDescription.Text = bissclass.combodisplayitemCodeName("item", anycode, dtstock, "name");
        //        //   cboDescription.SelectedValue = msmrfunc.mrGlobals.anycode1; // mstkcode = msmrfunc.mrGlobals.anycode1;
        //        //. anycode = cboDescription.Text;
        //        //. this.cboDescription.Focus();
        //        txtStkCode.Text = mstkcode = anycode;
        //        txtStkCode.Focus();
        //        return;
        //    }
        //}

        //private void txtStkCode_Enter(object sender, EventArgs e)
        //{
        //    if (onNmrDose)
        //        return;
        //    if (string.IsNullOrWhiteSpace(anycode))
        //        ClearControls("");
        //}

        //private void txtStkCode_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtStkCode.Text))
        //        return;

        //    DataTable dt = Dataaccess.GetAnytable("", "SMS", "select name from stock where item = '" + txtStkCode.Text + "'", false);

        //    if (dt.Rows.Count < 1)
        //    {
        //        msmrfunc.mrGlobals.auto_search_string = txtStkCode.Text;
        //        btnstock.PerformClick();
        //    }
        //    else
        //        txtDescription.Text = dt.Rows[0]["name"].ToString();
        //    GetItemDtls();
        //}

        private void txtStkCode_LostFocus(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(txtStkCode.Text))
            //    return;
            //DataTable dt = Dataaccess.GetAnytable("", "SMS", "select name from stock where item = '" + txtStkCode.Text + "'", false);
            //if (dt.Rows.Count < 1)
            //{
            //    msmrfunc.mrGlobals.auto_search_string = txtStkCode.Text;
            //    btnstock.PerformClick();
            //}
            //else
            //    txtDescription.Text = dt.Rows[0]["name"].ToString();
            //GetItemDtls();
        }

        //void GetItemDtls()
        //{
        //    // if (onNmrDose)
        //    //     return;
        //    iscapitated = fee_for_service = false;
        //    bool tocontinue = true, preauthorization = false;
        //    // lv_written = false;
        //    Stkstrength = stkper = 0;
        //    decimal qtyavailable = 0m, cost = 0m, rtnCost = 0m, purcost = 0m;
        //    string txtdose = "", unitid = "";
        //    packqty = 0m;
        //    int autoremind_period = 0;
        //    //if (string.IsNullOrWhiteSpace(cboDescription.Text) || cboDescription.SelectedValue == null )
        //    //{
        //    //    return;
        //    //}
        //    if (string.IsNullOrWhiteSpace(txtStkCode.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
        //        return;
        //    anycode = "";
        //    mstkcode = txtStkCode.Text; // cboDescription.SelectedValue.ToString();

        //    rtnCost = msmrfunc.stockitemValidate(mstkcode, ref qtyavailable, ref tocontinue, ref preauthorization, 
        //      ref iscapitated, txtDescription.Text, ref txtdose, ref unitid, ref cost, ref Stkstrength, ref stkper, 
        //      ref packqty, autoremind_period, ref purcost, pharmacyStore, ref alertforConsumables);
        //    if (!tocontinue)
        //    {
        //        //cboDescription.Text = "";
        //        //btnstock.Focus();
        //        txtStkCode.Text = txtDescription.Text = "";
        //        txtStkCode.Focus();
        //        return;
        //    }
        //    lblQtyPack.Text = packqty < 1 ? "1" : packqty.ToString();
        //    lblQtyPack.Text += "/pU";
        //    nmrQtyavailable.Value = qtyavailable;
        //    lblunitid.Text = unitid;
        //    nmrunitcost.Value = cost;
        //    nmrUnitPurchaseValue.Value = purcost;
        //    decimal grpRate = 0m;
        //    //30-04-2021 - GIWA wants to check if patient got this drug in the past x period for opd only
        //    if (!chkinpatient.Checked && presctnIntValidation > 0 && dtlastattend != DateTime.Now.Date && 
        //      DateTime.Now.Date.Subtract(dtlastattend).Days <= presctnIntValidation && !msmrfunc.CheckHistoryPresciption(phbchain.GROUPCODE, 
        //      phbchain.PATIENTNO, mstkcode, presctnIntValidation))
        //    {
        //        txtStkCode.Text = txtDescription.Text = "";
        //        txtStkCode.Focus();
        //        return;
        //    }
        //    //27.04.2021 - implemented drug cost by billing category for corporate - GIWA
        //    if (phbchain.GROUPHTYPE == "C" && string.IsNullOrWhiteSpace(phbchain.HMOSERVTYPE)) //coproate
        //    {
        //        rtnCost = grpRate = msmrfunc.CheckGrpDrugChargePercentage(dtGrpDrgChargRate, phbchain.PATCATEG, nmrUnitPurchaseValue.Value, nmrunitcost.Value);
        //    }
        //    else if (phbchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(phbchain.HMOSERVTYPE)) //check hmo/nhis drgs define 
        //    {
        //        //cost = rtnamt;
        //        //31.10.2016 MUST CHECK RTNAMT - QTY AVAIL OR COST ?

        //        rtnCost = msmrfunc.CheckCorpPatientStkDefined(phbchain.GROUPHEAD, phbchain.GROUPHTYPE, phbchain.HMOSERVTYPE, 
        //          phbchain.GROUPCODE, nhisgentariff, (chkinpatient.Checked) ? true : false, ref cost, fee_for_service, 
        //          mstkcode, ref preauthorization, ref iscapitated, ref tocontinue, txtDescription.Text);
        //        if (!tocontinue)
        //        {
        //            txtStkCode.Text = txtDescription.Text = "";
        //            txtStkCode.Focus();
        //            return;
        //        }
        //    }
        //    if (grpRate < 1 && rtnCost < 1m && phbchain.GROUPHTYPE == "C" && !string.IsNullOrWhiteSpace(phbchain.PATCATEG))
        //    {
        //        rtnCost = msmrfunc.CheckCustClassforStkDefined(phbchain.PATCATEG, ref preauthorization, ref iscapitated, ref cost, ref tocontinue, mstkcode, txtDescription.Text);
        //        if (!tocontinue)
        //        {
        //            txtStkCode.Text = txtDescription.Text = "";
        //            txtStkCode.Focus();
        //            return;
        //        }
        //    }
        //    if (grpRate < 1 && rtnCost < 1m) //stock differential tarif defined
        //    {
        //        rtnCost = msmrfunc.CheckStkCharge(phbchain.PATCATEG, ref cost, ref tocontinue, mstkcode);
        //        if (!tocontinue)
        //        {
        //            txtStkCode.Text = txtDescription.Text = "";
        //            txtStkCode.Focus();
        //            return;
        //        }
        //    }
        //    anycode = txtStkCode.Text;
        //    savedstksellamount = nmrunitcost.Value = rtnCost;
        //    // the following segment is necessary to be able to know the doseage type to enable - Tab/Liquid - 04/06/2010
        //    if (string.IsNullOrWhiteSpace(lblunitid.Text))
        //    {
        //        DialogResult result = MessageBox.Show(txtDescription.Text.Trim() +
        //            " - Unable to Determine Drug Type; CLICK YES for SYRUP/LIQUID; NO for OTHERS", "Drugs Unit of Measure", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (result == DialogResult.Yes)
        //        {
        //            lblunitid.Text = "Tabs";
        //            // return;
        //        }
        //        else if (result == DialogResult.No)
        //        {
        //            lblunitid.Text = "Btl";
        //            // return;
        //        }
        //    }
        //    /*     string xlabel = lblunitid.Text.Trim().Length > 2 ? lblunitid.Text.Substring(0, 3).ToUpper() : lblunitid.Text.Trim().ToUpper();
        //         if (xlabel == "BTL" || xlabel == "BOT" || xlabel == "VIA" || xlabel == "SYR" || xlabel == "INJ" || xlabel == "AMP")
        //         {
        //             comboDoseTab.Visible = false;
        //             comboDoseLiquid.Visible = true;
        //         }
        //         else
        //         {
        //             comboDoseLiquid.Visible = false;
        //             comboDoseTab.Visible = true;
        //         }*/
        //    foundit = false;

        //    newrec = true;
        //    //we must scan through wgeta_ array to check if stock had been selected - we edit
        //    if (listView1.Items.Count > 0)
        //    {
        //        for (int i = 0; i < listView1.Items.Count; i++)
        //        {
        //            if (listView1.Items[i].SubItems[20].Text.Trim() == mstkcode.Trim())
        //            {
        //                foundit = true;
        //                recno = i;
        //                break;
        //            }
        //        }
        //        if (foundit)
        //        {
        //            ServiceDuplicateOptions serviceuplicate = new ServiceDuplicateOptions();
        //            serviceuplicate.Closed += new EventHandler(serviceuplicate_Closed);
        //            serviceuplicate.ShowDialog();
        //        }
        //    }
        //    /*    if (comboDoseLiquid.Visible)
        //            comboDoseLiquid.Focus();
        //        else
        //        { comboDoseTab.Focus(); }*/
        //    if (preauthorization && reqAlertCount < 5)
        //    {
        //        requestalert[reqAlertCount] = txtDescription.Text.Trim();
        //        reqAlertCount++;
        //    }
        //    return;
        //}

        //   void serviceuplicate_Closed(object sender, EventArgs e)
        //   {
        //       /*	1 - ADD
        //2 - Amend
        //3 - Delete
        //4 - Ignor */
        //       int rtnval = msmrfunc.mrGlobals.returnvalue;
        //       if (rtnval < 1 || rtnval > 3)
        //       {
        //           txtDescription.Focus();
        //           return;
        //       }
        //       else if (rtnval == 1) //ADD 
        //       {
        //           newrec = true;
        //           //  lv_written = false;
        //           nmrDose.Focus();
        //           /* if (comboDoseLiquid.Visible)
        // comboDoseLiquid.Focus();
        //else
        //{ comboDoseTab.Focus(); }*/

        //           return;
        //       }
        //       else if (rtnval == 2)  // AMend
        //       {
        //           // lv_written = false;
        //           newrec = false;
        //           countersave = Convert.ToDecimal(listView1.Items[recno].SubItems[0].ToString());
        //           nmrQtyReqd.Value = Convert.ToDecimal(listView1.Items[recno].SubItems[4].ToString());
        //           txtdose = listView1.Items[recno].SubItems[7].Text;
        //           nmrDose.Value = Convert.ToDecimal(listView1.Items[recno].SubItems[15].ToString());
        //           //   comboDoseTab.Text = (comboDoseTab.Visible) ? listView1.Items[recno].SubItems[7].Text : "";
        //           //   comboDoseLiquid.Text = (comboDoseLiquid.Visible) ? listView1.Items[recno].SubItems[7].Text : "";


        //           comboInterval.Text = listView1.Items[recno].SubItems[8].Text;
        //           comboDuration.Text = listView1.Items[recno].SubItems[9].Text;
        //           comboWhenHow.Text = listView1.Items[recno].SubItems[10].Text;

        //           txtRxInstructions.Text = listView1.Items[recno].SubItems[11].Text;
        //           mcumgv = Convert.ToDecimal(listView1.Items[recno].SubItems[12].ToString());

        //           if (mcumgv != 0)
        //           {
        //               DialogResult result = MessageBox.Show("Record Exist and Already Dispensed... Can't Be Amended", "Prescription Management");
        //               txtStkCode.Text = "";
        //               txtDescription.Text = "";
        //               txtStkCode.Focus();
        //               return;
        //           }
        //           //25.3.2018  nmrCurrentTotal.Value = nmrCurrentTotal.Value - Convert.ToDecimal(listView1.Items[recno].SubItems[6].ToString());
        //           //initialize string to med.history
        //           if (msection == "8")  //for PHARMACY NOTE IN medhist note
        //           {
        //               phamalterstr = phamalterstr + "Pharmacist Alteration - " + txtDescription.Text.Trim() + " Qty_Presd." +
        //               txtdose.Trim() + " x " + comboInterval.Text.Trim() + " x " +
        //               comboDuration.Text.Trim() + " By : " + woperator.Trim() + " " + DateTime.Now.ToString("dd-MM-yyyy @ HH:mm") + "\r\n";
        //           }

        //       }
        //       else if (rtnval == 3) //Delete
        //       {
        //           decimal mcumgv = Convert.ToDecimal(listView1.Items[recno].SubItems[12].ToString());
        //           if (mcumgv != 0)
        //           {
        //               DialogResult result = MessageBox.Show("Item Already Dispensed - use the listBox Option....", "Deleting Dispensed Drug");
        //               return;
        //           }
        //           //msgeventtracker = "DP";
        //           DialogResult result1 = MessageBox.Show("Pls Confirm to Delete this item..." + txtDescription.Text.Trim(), "PRESCRIPTIONS", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        //           if (result1 == DialogResult.Yes)
        //           {
        //               //SAVE COST OF ITEM TO BE REMOVED
        //               decimal xamt = Convert.ToDecimal(listView1.Items[recno].SubItems[6].ToString());
        //               phamalterstr = phamalterstr + "Deleted - " + txtDescription.Text.Trim() + " Qty_Prescd." +
        //                   nmrQtyReqd.Value.ToString() + " x " + txtdose.Trim() + " x " + comboInterval.Text.Trim() + " x " +
        //                   comboDuration.Text.Trim() + " Cost : " + listView1.Items[recno].SubItems[6].ToString() +
        //                   "(" + mdoctor.Trim() + ")" + " By : " + woperator.Trim() + " " +
        //                   DateTime.Now.ToString("dd-MM-yyyy @ HH:mm") + "\r\n";

        //               if (!string.IsNullOrWhiteSpace(listView1.Items[recno].SubItems[18].Text) && 
        //                   Convert.ToDecimal(listView1.Items[recno].SubItems[18].Text.Trim()) > 0) //saved to database before now
        //               {
        //                   /* decimal itemcount = Convert.ToDecimal(listView1.Items[recno].SubItems[0].ToString());
        //	 DISPENSA.DeletePrescription((chkinpatient.Checked) ? true : false, Convert.ToInt32( listView1.Items[recno].SubItems[18].ToString()));*/
        //                   string xfile = chkinpatient.Checked ? "inpdispensa" : "dispensa";
        //                   string updstr = "delete from " + xfile + " where recid = '" + listView1.Items[recno].SubItems[18].ToString().Trim() + "'";
        //                   bissclass.UpdateRecords(updstr, "MR");
        //                   //we need to adjust bill
        //                   btnsave.Enabled = true;
        //                   MessageBox.Show("You must click the Submit Button To Adjust Bills and other records accordingly...");
        //               }

        //               nmrCurrentTotal.Value = nmrCurrentTotal.Value - xamt;
        //               anycode = "";
        //               listView1.Items[recno].Remove();
        //               listView1.Show();
        //               renumberview();
        //               txtDescription.Select();
        //               return;

        //           }
        //       }

        //   }

        /*	void renumberview()
            {
                counter = 0m;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[0].Text = (i + 1).ToString();
                    counter = Convert.ToDecimal(i);
                }
                counter++;
            }
            void drgdefine_chk(bool restrictive,bool inclusive,bool preauthorization,bool xhmo ) 
            {
                string xpattype = (xhmo) ? "HMO Plan Type" : "Billing Category";
                hmostkpricefound = false;
                if ( foundit && inclusive)
                {
                    //do nothing
                    hmostkpricefound = true;
                }
                else if (restrictive) //whether found or not
                {
                    MessageBox.Show(comboDescription.Text.Trim() + " is not on approved list for this Patient's "+xpattype+"... - RESTRICTIVE !!! '\n' PLEASE SELECT ALTERNATIVE DRUG...", "Drugs Approved List");
                    comboDescription.Focus();
                    return;
                }
                else
                {
                    msgeventtracker = "DRG";
                    DialogResult result = MessageBox.Show(comboDescription.Text.Trim() + " is not on approved list for this Patient's " + xpattype + "... Continue ?", "Drugs Approved List",
       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, msgBoxHandler);
                }

                if ( preauthorization )
                {
                    msgeventtracker = "PA";
                    DialogResult result  = MessageBox.Show("Selected Service Requires Pre-Authorization...CONTINUE ? ",
                    "PRE-AUTHORIZATION ALERT!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, msgBoxHandler);
                }

            }*/

        /*       void hmoserv_check(string xcustomer, string xplantype)
               {
                   bool restrictive, inclusive, preauthorization = false;
                   //bool hmofound = false;
                   foundit = fee_for_service = false;
                   Hmodetail hmodetail = new Hmodetail();
                   hmodetail = Hmodetail.GetHMODETAIL(xcustomer,xplantype);
                   if (hmodetail == null)
                   {
                       hmostkpricefound = false;
                       return;
                   }
                   inclusive = hmodetail.DRGINCLUSIVE;
                   restrictive = hmodetail.DRGRESTRICTIVE;
                   if ( hmodetail.CAPAMT == 0m ) //all services are fee for service no capitation but the hmo could have its own tariff so we check
                   {
                       fee_for_service = true;
                   }
                   HMOSERVIC hmoservic = new HMOSERVIC();
                   hmoservic = HMOSERVIC.GetHMOSERVIC(xcustomer,xplantype,mstkcode);
                   if (hmoservic != null)
                   {
                       hmostkpricefound = true;
                       BissClass.msmrfunc.mrGlobals.waitwindowtext = "FOUND HMO DRUG PRICE LIST...";
                       pleaseWait.Show();
                       foundit = true;
                       nmrunitcost.Value = hmoservic.AMOUNT;
                       iscapitated = (hmoservic.CAPITATED) ? true : false;
                       preauthorization = hmoservic.AUTHORIZATIONREQUIRED;
                   }
                   drgdefine_chk(restrictive, inclusive, preauthorization,true );
               }*/

        //     void loadprevDefinitions()
        //     {
        //         if (msection == "4" || msection == "8") //DOCS AND PHARMACY ONLY
        //         {
        //             //ok
        //         }
        //         else
        //         {
        //             //msgeventtracker = "EXIT";
        //             DialogResult result = MessageBox.Show("Further Access Denied....", "Prescription Management");
        //             btnclose.PerformClick();
        //             return;
        //         }
        //         if (string.IsNullOrWhiteSpace(reference))
        //         {
        //             //msgeventtracker = "EXIT";
        //             DialogResult result = MessageBox.Show("This Request has NO Consultation Reference... Cannot be Tracked!!!", "NO CONSULTATION REFERENCE");
        //             btnsave.Enabled = false;
        //             btnclose.PerformClick();
        //             return;
        //         }
        //         nmrCurrentTotal.Value = 0m;
        //         string[] arr = new string[23];
        //         ListViewItem itm;
        //         DataTable dt;
        //         if (chkinpatient.Checked)
        //             dt = DISPENSA.GetDISPENSA(phbchain.GROUPCODE, phbchain.PATIENTNO, mtrans_date, true);
        //         else
        //             dt = DISPENSA.GetDISPENSA(phbchain.GROUPCODE, phbchain.PATIENTNO, mtrans_date, false);
        //         if (dt.Rows.Count < 1)
        //             return;
        //         {
        //             // listView1.DataSource = dt;
        //             /* listView1.Items[0].UseItemStyleForSubItems = false;
        //// Now you can Change the Particular Cell Property of Style

        //  listView1.Items[0].SubItems[1].BackColor = Color.Red;*/
        //             int xrow = 0;
        //             foreach (DataRow row in dt.Rows)
        //             {
        //                 nmrCurrentTotal.Value += Convert.ToDecimal(row["cost"]);
        //                 string xsp_inst = row["sp_inst"].ToString().Trim();
        //                 if (string.IsNullOrWhiteSpace(xsp_inst))
        //                 {
        //                     txtRxInstructions.Text = txtRxInstructions.Text.Trim() + xsp_inst;
        //                 }
        //                 arr[0] = row["itemno"].ToString();
        //                 arr[1] = row["stk_desc"].ToString();
        //                 arr[2] = row["unitcost"].ToString();
        //                 arr[3] = " "; //for qty available only during data entry 
        //                 arr[4] = row["qty_pr"].ToString();
        //                 arr[5] = row["unit"].ToString();
        //                 arr[6] = row["cost"].ToString();
        //                 arr[7] = row["cdose"].ToString();
        //                 arr[8] = row["cinterval"].ToString();
        //                 arr[9] = row["cduration"].ToString();
        //                 arr[10] = row["rx"].ToString();
        //                 arr[11] = row["sp_inst"].ToString();
        //                 arr[12] = row["CUMGV"].ToString();
        //                 arr[13] = row["interval"].ToString();
        //                 arr[14] = row["duration"].ToString();
        //                 arr[15] = row["dose"].ToString();
        //                 arr[16] = (Convert.ToBoolean(row["CAPITATED"])) ? "YES" : "NO";
        //                 arr[17] = Convert.ToBoolean(row["posted"]) || Convert.ToDecimal(row["cumgv"]) > 0 ? "YES" : "NO"; // indicates record was saved before.. needed during delete
        //                 arr[18] = row["recid"].ToString();
        //                 arr[19] = row["unitpurvalue"].ToString();
        //                 arr[20] = row["stk_item"].ToString();
        //                 arr[21] = "";
        //                 itm = new ListViewItem(arr);
        //                 listView1.Items.Add(itm);
        //                 listView1.Items[xrow].UseItemStyleForSubItems = false;
        //                 listView1.Items[xrow].SubItems[4].BackColor = Color.Red;
        //                 xrow++;
        //             }
        //             counter = xrow;
        //         }
        //     }

        //public void ClearControls(string xtype)
        //{

        //    comboDoseLiquid.Text = comboDoseTab.Text = comboInterval.Text = comboDuration.Text = comboUnitId.Text = comboWhenHow.Text = cboRoutineDrgs.Text = txtdose = txtRxInstructions.Text = "";
        //    recno = 0;
        //    nmrQtyReqd.Value = nmrunitcost.Value = nmrDose.Value = 0m;
        //    if (xtype == "A") //everything 
        //        nmrCurrentTotal.Value = nmrCapitedCost.Value = 0m;
        //    if (anycode == "")
        //        txtDescription.Text = txtStkCode.Text = anycode = "";
        //    return;
        //}

        private void comboDoseLiquid_SelectedValueChanged(object sender, EventArgs e)
        {
            /*   ComboBox btn = sender as ComboBox;
               if ( btn.Name == "comboDoseLiquid" && comboDoseLiquid.Text.Substring(0, 1) == "O")
                    comboDoseLiquid.Text = "";
                else if (btn.Name == "comboDoseTab" && comboDoseTab.Text.Substring(0, 1) == "O")
                    comboDoseTab.Text = "";
                else if (btn.Name == "comboUnitId" && comboUnitId.Text.Substring(0, 3) == "Oth")
                    comboUnitId.Text = "";
                else if (btn.Name == "comboUnitId" && comboUnitId.Text.Substring(0, 1) == "O")
                    comboUnitId.Text = "";
               else if (btn.Name == "comboInterval" && comboInterval.Text.Substring(0, 3) == "Oth")
                   comboInterval.Text = "";
               else if (btn.Name == "comboDuration" && comboDuration.Text.Substring(0, 1) == "O")
                   comboDuration.Text = "";
               else if (btn.Name == "comboWhenHow" && comboWhenHow.Text.Substring(0, 3) == "Oth")
               { comboWhenHow.Text = ""; }

               return;*/
        }

        //private void nmrDose_Enter(object sender, EventArgs e)
        //{
        //    NumericUpDown nmr = sender as NumericUpDown;
        //    nmr.Select(0, nmr.Text.Length);
        //    if (nmr.Name == "nmrDose")
        //    {
        //        if (string.IsNullOrWhiteSpace(txtStkCode.Text))
        //        {
        //            //cboDescription.Select();
        //            txtStkCode.Select();
        //            return;
        //        }
        //        onNmrDose = true;
        //    }
        //}

        //private void nmrDose_LostFocus(object sender, EventArgs e)
        //{
        //    if (nmrDose.Value > 0)
        //    {
        //        txtdose = nmrDose.Value.ToString();
        //        mdose = nmrDose.Value;
        //    }
        //    comboUnitId.Focus();
        //    onNmrDose = false;
        //}

        //Start

        //private void comboDoseTab_Enter(object sender, EventArgs e)
        //{
        //    //if (string.IsNullOrEmpty(comboBox1.Text)) or if (comboBox1.SelectedIndex == -1)

        //    ComboBox btn = sender as ComboBox;
        //    if (btn == null || string.IsNullOrWhiteSpace(txtDescription.Text))
        //    {
        //        //   comboDescription.Focus();
        //        return;
        //    }
        //    if (btn.Name == "comboInterval" && (string.IsNullOrEmpty(comboUnitId.Text) && !string.IsNullOrWhiteSpace(txtdose) || 
        //          comboUnitId.SelectedItem != null && !string.IsNullOrWhiteSpace(comboUnitId.SelectedItem.ToString())))
        //    {
        //        if (lblunitid.Text.ToUpper() == "BTL" || lblunitid.Text.Length > 2 && lblunitid.Text.ToUpper().Substring(0, 3) == "BOT" || 
        //          lblunitid.Text.ToUpper() == "VIAL" || lblunitid.Text.ToUpper() == "SYR" || lblunitid.Text.ToUpper() == 
        //          "INJ" || comboUnitId.SelectedItem != null && comboUnitId.Text.Contains("ml"))
        //            comboUnitId.Text = string.IsNullOrWhiteSpace(comboUnitId.Text) ? "ml" : comboUnitId.Text;
        //        else if (lblunitid.Text.ToUpper() == "TAB")
        //            comboUnitId.Text = "Tab";
        //        else if (comboUnitId.SelectedItem != null)
        //            comboUnitId.Text = comboUnitId.SelectedItem.ToString();
        //        else
        //        {
        //            string xstr = comboUnitId.SelectedItem == null ? "" : comboUnitId.SelectedItem.ToString();
        //            DialogResult result = MessageBox.Show("Pls Specifiy Unit of Measure on Unit ID Column..." + xstr, "Prescriptions Management");
        //            comboUnitId.Focus();
        //            return;
        //        }
        //    }
        //    /*   else if (btn.Name == "nmrQtyReqd")
        //       {
        //           BissClass.msmrfunc.mrGlobals.waitwindowtext = "Enter Actual Qty for Costing/Deduction from Stock...";
        //          pleaseWait.Show();
        //       }*/
        //}

        //private void comboDoseTab_LostFocus(object sender, EventArgs e)
        //{
        //    txtdose = "";
        //    if (string.IsNullOrWhiteSpace(comboDoseTab.Text) && comboDoseTab.SelectedItem != null && !string.IsNullOrWhiteSpace(comboDoseTab.SelectedItem.ToString()))
        //        comboDoseTab.Text = comboDoseTab.SelectedItem.ToString();
        //    if (string.IsNullOrWhiteSpace(comboDoseTab.Text))
        //        return;

        //    comboUnitId.Enabled = true;
        //    // if (comboDoseTab.Text.Trim().Length > 1 && comboDoseTab.Text.Contains("."))

        //    if (bissclass.IsDigitsOnly(comboDoseTab.Text.Trim()))
        //        mdose = Convert.ToDecimal(comboDoseTab.Text);
        //    else
        //        mdose = Convert.ToDecimal(bissclass.GetNumberFromString(comboDoseTab.Text));
        //    //      mdose = (comboDoseTab.Text.Trim() == "One Half") ? .5m : Convert.ToDecimal(comboDoseTab.Text);
        //    txtdose = mdose.ToString();
        //    if (bissclass.IsDigitsOnly(comboDoseTab.Text.Trim()))
        //        mdose = Convert.ToDecimal(Convert.ToDouble(comboDoseTab.Text));
        //    else
        //        mdose = Convert.ToDecimal(bissclass.GetNumberFromString(comboDoseTab.Text));
        //    txtdose = mdose.ToString();
        //}

        //private void comboDoseLiquid_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(comboDoseLiquid.Text) && comboDoseLiquid.SelectedIndex == -1)
        //        return;

        //    //  5 ml  7.5 ml 15 ml 250 ml 300 ml 500 ml 1 drop 2 drops 3 drops 4 drops 1 bag Cream
        //    //  Others - Specify
        //    unitIDsave = "";
        //    if (comboDoseLiquid.SelectedIndex != -1)
        //    {
        //        int xopt = comboDoseLiquid.SelectedIndex;
        //        switch (xopt)
        //        {
        //            case 0:
        //            case 6:
        //            case 7:
        //            case 8:
        //            case 10:
        //                mdose = Convert.ToDecimal(comboDoseLiquid.SelectedItem.ToString().Substring(0, 1));
        //                break;
        //            case 2:
        //                mdose = Convert.ToDecimal(comboDoseLiquid.SelectedItem.ToString().Substring(0, 2));
        //                break;
        //            case 1:
        //            case 3:
        //            case 4:
        //            case 5:
        //                mdose = Convert.ToDecimal(comboDoseLiquid.SelectedItem.ToString().Substring(0, 3));
        //                break;
        //            case 11: //cream
        //                mdose = 1; // bissclass.GetNumberFromString(comboDoseLiquid.Text.Trim());
        //                unitIDsave = "Cream";
        //                break;
        //            case 12:
        //                mdose = Convert.ToDecimal(bissclass.GetNumberFromString(comboDoseLiquid.Text.Trim()));
        //                break;
        //            default:
        //                mdose = Convert.ToDecimal(comboDoseLiquid.SelectedItem.ToString().Substring(0, 1));
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        //if (comboDoseLiquid.Text.Trim().Length > 1 && (comboDoseLiquid.Text.Trim().Substring(0, 2) == "0." || comboDoseLiquid.Text.Trim().Substring(0, 1) == "."))
        //        //    mdose = Convert.ToDecimal(comboDoseLiquid.Text.Trim());
        //        //else
        //        mdose = bissclass.IsDigitsOnly(comboDoseLiquid.Text.Trim()) ? Convert.ToDecimal(comboDoseLiquid.Text) : Convert.ToDecimal(bissclass.GetNumberFromString(comboDoseLiquid.Text.Trim()));
        //    }
        //    string x;
        //    //mdose = Convert.ToDecimal(comboDoseLiquid.Text.Trim());
        //    comboDoseLiquid.Text = txtdose = mdose.ToString();
        //    x = "";
        //    if (comboDoseLiquid.Text.Substring(0, 1).ToUpper() == "D")
        //    {
        //        x = (mdose == 1m) ? "drop" : "drops";
        //        // txtdose = comboDoseLiquid.Text.Substring(0, 2);
        //    }
        //    else if (new string[] { "M", "G", "MG" }.Contains(comboDoseLiquid.Text.ToUpper()))
        //    {
        //        //check for convertable
        //        if (Stkstrength != 0 && stkper != 0)
        //        {
        //            doconvertable();
        //        }
        //        x = "mg";
        //        //   txtdose = Convert.ToDecimal(comboDoseLiquid.Text.Substring(0, 2)).ToString();
        //    }
        //    else if (comboDoseLiquid.Text.Trim().Substring(0, 1).ToUpper() == "M")
        //    {
        //        x = "ml";
        //        //   txtdose = comboDoseLiquid.Text.Trim().Substring(0, 2);
        //        //    }
        //        //else
        //        //{
        //        //    txtdose = comboDoseLiquid.Text;
        //        //}

        //    }
        //    comboUnitId.Text = x;

        //}

        //End

        //void doconvertable()
        //{
        //    decimal xdose1;
        //    isconvertable = true;
        //    xdose1 = Stkstrength / stkper;
        //    //xdose1 = Convert.ToDecimal(comboDoseLiquid.Text) / xdose1;
        //    xdose1 = nmrDose.Value / xdose1;
        //    msmrfunc.mrGlobals.waitwindowtext = "Convertable - Strength :" + Stkstrength.ToString() +
        //        " mg Per " + stkper.ToString() + "ml   :=  " + xdose1.ToString() + "ml";
        //    //pleaseWait.Show();
        //}

        //private void comboUnitId_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(comboUnitId.Text))
        //        return;
        //    int xindex = -1;
        //    if (comboUnitId.Text.Trim().Length == 1 && comboUnitId.SelectedIndex == -1)
        //    {
        //        comboUnitId.Text = GetComboText(comboUnitId, comboUnitId.Text.Trim(), ref xindex);
        //        comboUnitId.SelectedIndex = xindex;
        //    }
        //    if (comboUnitId.Text.Trim() == "MG" && Stkstrength != 0 && stkper != 0)
        //        doconvertable();
        //}

        //     private void comboInterval_LostFocus(object sender, EventArgs e)
        //     {
        //         if (string.IsNullOrWhiteSpace(comboInterval.Text) && comboInterval.SelectedItem != null && !string.IsNullOrWhiteSpace(comboInterval.SelectedItem.ToString()))
        //             comboInterval.Text = comboInterval.SelectedItem.ToString();

        //         if (string.IsNullOrWhiteSpace(comboInterval.Text))
        //             return;

        //         /*1-once daily 2-twice a day 3-3times daily 4-4times daily 5-5times daily
        //	6-every 3 hrs, 7-every 4 hrs, 8-every 6 hrs, 9-every 8 hrs, 10-every 12hrs
        //	11-every other day, 12-freely as needed, 13-when necessary(prn),14-others */

        //         int xindex = -1;

        //         if (comboInterval.Text.Trim().Length == 1 && comboInterval.SelectedIndex == -1)
        //         {
        //             comboInterval.Text = GetComboText(comboInterval, comboInterval.Text.Trim(), ref xindex);
        //             comboInterval.SelectedIndex = xindex;
        //         }

        //         int xopt = comboInterval.SelectedIndex;

        //         switch (xopt)
        //         {
        //             case 0:
        //                 minterval = 24m;
        //                 break;
        //             case 1:
        //             case 9:
        //                 minterval = 12m;
        //                 break;
        //             case 2:
        //             case 5:
        //             case 8:
        //                 minterval = 8m;
        //                 break;
        //             case 3:
        //             case 7:
        //                 minterval = 6m;
        //                 break;
        //             case 4:
        //             case 6:
        //                 minterval = 4m;
        //                 break;
        //             case 10:
        //                 minterval = 48m;
        //                 break;
        //             default:
        //                 minterval = 1m;
        //                 break;
        //         }
        //         /*if ( minterval == 0 && Convert.ToDecimal(comboInterval.Text) >= 1 )
        //{
        //	minterval = Convert.ToDecimal(comboInterval.Text);
        //	comboInterval.Text = minterval.ToString().Trim() +" hourly";
        //}*/
        //     }

        //        private void comboDuration_LostFocus(object sender, EventArgs e)
        //        {
        //            if (string.IsNullOrWhiteSpace(comboDuration.Text) && comboDuration.SelectedItem != null)
        //                comboDuration.Text = comboDuration.SelectedItem.ToString();

        //            if (string.IsNullOrWhiteSpace(comboDuration.Text))
        //                return;

        //            decimal xqtysave, xdose, x;
        //            if (listView1.SelectedIndex == 10) //OTHERS - specify
        //            {
        //                comboDuration.Text = bissclass.GetNumberFromString(comboDuration.Text.Trim()).ToString() + " days";
        //            }

        //            int xindex = -1;
        //            if (comboDuration.Text.Trim().Length == 1 && comboDuration.SelectedIndex == -1)
        //            {
        //                comboDuration.Text = GetComboText(comboDuration, comboDuration.Text.Trim(), ref xindex);
        //                comboDuration.SelectedIndex = xindex;
        //            }

        //            /*1 day 2 days 3 days 4 days 5 days 7 days 14 days 21 days 28 days Others - Specify*/
        //            /* mduration = comboDuration.SelectedItem != null && comboDuration.SelectedIndex < 7 ?
        //				 Convert.ToDecimal(comboDuration.SelectedItem.ToString().Substring(0, 1)) : Convert.ToDecimal(comboDuration.SelectedItem.ToString().Substring(0, 2));*/
        //            /*1 day
        //2 days
        //3 days
        //4 days
        //5 days
        //7 days
        //14 days
        //21 days
        //28 days
        //Others - Specify - 0 BASE*/
        //            if (comboDuration.SelectedItem != null)
        //            {
        //                mduration = comboDuration.SelectedIndex < 6 ?
        //                    Convert.ToDecimal(comboDuration.SelectedItem.ToString().Substring(0, 1)) : 
        //                    comboDuration.SelectedIndex < 8 ? Convert.ToDecimal(comboDuration.SelectedItem.ToString().Substring(0, 2)) : 
        //                    bissclass.GetNumberFromString(comboDuration.Text);
        //            }
        //            else
        //                mduration = bissclass.GetNumberFromString(comboDuration.Text);
        //            if (comboInterval.SelectedItem == null)
        //                nmrQtyReqd.Value = mdose;
        //            else
        //            {
        //                xdose = mdose = nmrDose.Value;
        //                if (isconvertable)
        //                {
        //                    xdose = Stkstrength / stkper;
        //                    xdose = mdose / xdose;
        //                }
        //                x = minterval < 1 ? 1 : 24m / minterval;
        //                nmrQtyReqd.Value = (x * mduration * xdose);
        //                if (nmrQtyReqd.Value < 1m)
        //                    nmrQtyReqd.Value = 1m;
        //                xqtysave = nmrQtyReqd.Value;
        //                if (packqty > 0)
        //                {
        //                    nmrQtyReqd.Value = Math.Round((nmrQtyReqd.Value / packqty));
        //                    if (xqtysave < packqty)
        //                        nmrQtyReqd.Value = 1m;
        //                    /*  else
        //                      {
        //                          int x1;
        //                          Math.DivRem(Convert.ToInt32(xqtysave), Convert.ToInt32(packqty), out x1);
        //                          x = (x1 >= 1) ? 1m : 0m;
        //                          nmrQtyReqd.Value += x;
        //                      }*/
        //                }
        //            }
        //            if (nmrQtyReqd.Value > 99m)
        //            {
        //                DialogResult result = MessageBox.Show("System is Unable to calculate actual requirement from your Selections...'\n'" + "Pls Specify Qty Required Manually", "Prescriptions");
        //                nmrQtyReqd.Value = 0;
        //                nmrQtyReqd.Focus();
        //                return;
        //            }
        //            //else 28.09.2016
        //            //{
        //            //    nmrCost.Value = nmrQtyReqd.Value * nmrunitcost.Value;
        //            //}
        //            ///*IF !EMPTY(billchai.currency) AND m.fxtype=2 AND !EMPTY(ThisForm.txtfxunitcost.Value) PENDING 23-11-2013
        //            //    ThisForm.txtfxcost.Value = ThisForm.nmrqreqd.value*ThisForm.txtfxunitcost.Value
        //            //ENDIF*/
        //            //xcost = nmrCost.Value;
        //            //if (isanc)
        //            //{
        //            //    anc_check(ref xcost);
        //            //}
        //            //nmrCost.Value = xcost;
        //        }

        //string GetComboText(ComboBox cbo, string xstr, ref int xselindex)
        //{
        //    string xstring = xstr;
        //    for (int i = 0; i < cbo.Items.Count; i++)
        //    {
        //        if (cbo.Items[i].ToString().Substring(0, 1) == xstr)
        //        {
        //            xstring = cbo.Items[i].ToString();
        //            xselindex = i;
        //            break;
        //        }
        //    }
        //    return xstring;
        //}

        //void anc_check(ref decimal xcost)
        //{
        //    string xref = "", ct = "";
        //    bool xv = false;
        //    decimal xvamt = 0;

        //    ANCREG.GetANCREG(phbchain.PATIENTNO, phbchain.GROUPCODE, "D", mstkcode, ref xcost, ref ct, ref xv, ref xvamt, ref xref);
        //    nmrCost.Value = xcost;
        //}

        //private void nmrQtyReqd_LostFocus(object sender, EventArgs e)
        //{
        //    if (nmrQtyReqd.Value == 0)
        //        return;

        //    LoadListviewDtl();
        //    //if (nmrQtyReqd.Value == 0)
        //    //{
        //    //    DialogResult result = MessageBox.Show("Confirm to write for ZERO stock qty","Prescriptions Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    //    if (result == DialogResult.No)
        //    //    {
        //    //        comboDescription.Focus();
        //    //        return;
        //    //    }

        //    //}
        //    //decimal xcost; //, x;
        //    ////03/08/2011 - 8.57 - We must attempt to put a check here against irresponsible consulting
        //    //if ((lblunitid.Text.ToUpper() == "BTL" || lblunitid.Text.ToUpper() == "BOT" || lblunitid.Text.ToUpper() == "VIAL" ||
        //    //    lblunitid.Text.ToUpper() == "SYR" || lblunitid.Text.ToUpper() == "INJ" || lblunitid.Text.ToUpper() == "AMP")
        //    //    && nmrQtyReqd.Value > 2)
        //    //{
        //    //    DialogResult result = MessageBox.Show("Please Check Your Prescription criterial - Quantity Required Inconsistency !!!", "QUANTITY REQUIRED ALERT!!!");
        //    //    if (result == DialogResult.No)
        //    //    {
        //    //        comboDescription.Focus();
        //    //        return;
        //    //    }
        //    //    msgeventtracker = "ZQ";
        //    //    DialogResult resultzq = MessageBox.Show("Would you Prescribe " + nmrQtyReqd.Value.ToString() + 
        //          " of this item at once...?", "Prescriptions Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
        //          MessageBoxDefaultButton.Button2, msgBoxHandler);
        //    //    if (resultzq == DialogResult.No )
        //    //    {
        //    //        comboDescription.Text = "";
        //    //        comboDescription.Focus();
        //    //        return;
        //    //    }
        //    //}
        //    ///*we check if anc and drug is free; we pass "D" for drugs,txtstockcode for drugcode
        //    //    and nmrcost (xcost) for amount to charge which might be changed from glanccheck
        //    //    18/06/2011 - we check for capitated drug and keep cost */
        //    //if (iscapitated)
        //    //    nmrCapitedCost.Value = (nmrQtyReqd.Value * nmrunitcost.Value);
        //    //else
        //    //    nmrCurrentTotal.Value = (nmrQtyReqd.Value * nmrunitcost.Value);
        //    ///*	IF !EMPTY(billchai.currency) AND m.fxtype=2 AND !EMPTY(ThisForm.txtfxunitcost.Value) &&FX Direct Tariff
        //    //    ThisForm.txtfxcost.Value = this.value*ThisForm.txtfxunitcost.Value
        //    //    ThisForm.txtfxtotal.Value = ThisForm.txtfxtotal.Value + ThisForm.txtfxcost.Value   PENDING 23-11-2013 */
        //    //xcost = nmrCurrentTotal.Value;
        //    //if (isanc)
        //    //{
        //    //    anc_check(ref xcost);
        //    //}
        //}

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    LoadListviewDtl();
        //}

        //    void LoadListviewDtl()
        //    {
        //        //if (lv_written)
        //        //    return;
        //        DialogResult result;
        //        if (string.IsNullOrWhiteSpace(txtDescription.Text) || nmrQtyReqd.Value == 0)
        //        {
        //            result = MessageBox.Show("Please Check Product Description/Qty Required, \r\n\r\n It's Empty or Item Details have been Added ...");
        //            comboWhenHow.Focus();
        //            return;
        //        }

        //        //if (string.IsNullOrWhiteSpace(comboInterval.Text))
        //        //{
        //        //    result = MessageBox.Show("Internval is empty..."+comboInterval.SelectedItem.ToString() );
        //        //}
        //        //if (string.IsNullOrWhiteSpace(comboDuration.Text))
        //        //{
        //        //    result = MessageBox.Show("Duration is empty..."+comboDuration.SelectedItem.ToString() );
        //        //}
        //        //if (string.IsNullOrWhiteSpace(comboDoseTab.Text))
        //        //{
        //        //    result = MessageBox.Show("Dose Tab is empty..."+comboDoseTab.SelectedItem.ToString() );
        //        //    comboDoseTab.Text = comboDoseTab.SelectedItem.ToString();
        //        //}
        //        //check values MOVED FROM QTYREQUIRED LEAVE

        //        if (txtdose == "" && nmrDose.Value > 0)
        //            txtdose = nmrDose.Value.ToString();
        //        if (nmrunitcost.Value < 1)
        //        {
        //            result = MessageBox.Show("Please check this prescription...\r\n ZERO COST!!!  CONTINUE ??", "Prescriptions Management", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //            if (result == DialogResult.No)
        //                return;
        //        }
        //        decimal xcost;
        //        nmrCost.Value = nmrQtyReqd.Value * nmrunitcost.Value;
        //        //17.05.2019 - check and add markup if enabled
        //        if (phbchain.GROUPHTYPE == "P" && !chkinpatient.Checked)
        //        {
        //            if (mdrgmarkup != 0m) //outpatient
        //            {
        //                if (markupdrgbillonPercentage)
        //                    nmrCost.Value += ((nmrCost.Value * mdrgmarkup) / 100);
        //                else
        //                    nmrCost.Value += mdrgmarkup;
        //            }
        //        }
        //        if (string.IsNullOrWhiteSpace(txtdose) && string.IsNullOrWhiteSpace(comboInterval.Text) && string.IsNullOrWhiteSpace(comboDuration.Text))
        //        {
        //            //allow to go...
                   /*  result = MessageBox.Show("Please check this prescription...\r\n INCOOMPLETE RECORD - 
                        Cost/Qty Required/Dose/Interval/Duration \r\n ARE REQUIRED...", "Prescriptions Management", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                      return;*/
        //        }
        //        else
        //        {
        //            //03/08/2011 - 8.57 - We must attempt to put a check here against irresponsible consulting
        //            string xlabel = lblunitid.Text.Length > 2 ? lblunitid.Text.Substring(0, 3).ToUpper() : lblunitid.Text.Trim().ToUpper();
        //            if ((xlabel == "BTL" || xlabel == "BOT" || xlabel == "VIA" || xlabel == "SYR" || xlabel == "INJ" || xlabel == "AMP" || unitIDsave == "Cream") && nmrQtyReqd.Value > 2)
        //            {
        //                result = MessageBox.Show("Please Check Your Prescription criterial - Quantity Required Inconsistency !!!", "QUANTITY REQUIRED ALERT!!!");
        //                if (result == DialogResult.No)
        //                {
        //                    return;
        //                }
        //                result = MessageBox.Show("Would you Prescribe " + nmrQtyReqd.Value.ToString() + " of " + 
        //                  txtDescription.Text.Trim() + " at once...?", "Prescriptions Management", MessageBoxButtons.YesNo,
        //                  MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //                if (result == DialogResult.No)
        //                {
        //                    return;
        //                }
        //            }
        //        }
        //        /*we check if anc and drug is free; we pass "D" for drugs,txtstockcode for drugcode
        //and nmrcost (xcost) for amount to charge which might be changed from glanccheck
        //18/06/2011 - we check for capitated drug and keep cost */

        //        xcost = nmrCost.Value;
        //        if (isanc)
        //        {
        //            anc_check(ref xcost);
        //            nmrCost.Value = xcost;
        //        }
        //        if (iscapitated)
        //            nmrCapitedCost.Value = (nmrQtyReqd.Value * nmrunitcost.Value);
        //        else
        //            nmrCurrentTotal.Value += nmrCost.Value;
        //        /*	IF !EMPTY(billchai.currency) AND m.fxtype=2 AND !EMPTY(ThisForm.txtfxunitcost.Value) &&FX Direct Tariff
        //ThisForm.txtfxcost.Value = this.value*ThisForm.txtfxunitcost.Value
        //ThisForm.txtfxtotal.Value = ThisForm.txtfxtotal.Value + ThisForm.txtfxcost.Value   PENDING 23-11-2013 */

        //        //save item counter, if old record and restore after init
        //        string uid = comboUnitId.SelectedItem == null ? comboUnitId.Text : comboUnitId.SelectedItem.ToString();
        //        decimal newcounter = counter;
        //        if (!newrec)
        //        {
        //            ListViewItem itm = listView1.Items[recno];
        //            nmrCurrentTotal.Value = nmrCurrentTotal.Value - Convert.ToDecimal(itm.SubItems[6].Text);
        //            itm.SubItems[0].Text = countersave.ToString();
        //            itm.SubItems[1].Text = txtDescription.Text;
        //            itm.SubItems[2].Text = nmrunitcost.Value.ToString();
        //            itm.SubItems[3].Text = nmrQtyavailable.Value.ToString();
        //            itm.SubItems[4].Text = nmrQtyReqd.Value.ToString();
        //            itm.SubItems[5].Text = lblunitid.Text;
        //            itm.SubItems[6].Text = nmrCost.Value.ToString();
        //            itm.SubItems[7].Text = txtdose.Trim() + " " + uid; // comboUnitId.Text.Trim();
        //            itm.SubItems[8].Text = comboInterval.SelectedItem == null ? comboInterval.Text : comboInterval.SelectedItem.ToString(); // comboInterval.Text;
        //            itm.SubItems[9].Text = comboDuration.Text;
        //            itm.SubItems[10].Text = comboWhenHow.Text;
        //            itm.SubItems[11].Text = txtRxInstructions.Text.Trim();
        //            itm.SubItems[12].Text = mcumgv.ToString();
        //            itm.SubItems[13].Text = minterval.ToString();
        //            itm.SubItems[14].Text = mduration.ToString();
        //            itm.SubItems[15].Text = mdose.ToString();
        //            itm.SubItems[16].Text = iscapitated ? "YES" : "NO";
        //            itm.SubItems[17].Text = "NO";
        //            itm.SubItems[19].Text = nmrUnitPurchaseValue.Value.ToString();
        //            itm.SubItems[20].Text = mstkcode;
        //            itm.SubItems[21].Text = "YES";
        //            itm.SubItems[22].Text = alertforConsumables ? "YES" : "NO";
        //        }
        //        else
        //        {
        //            counter++;

        //            string[] row = { counter.ToString(), txtDescription.Text, nmrunitcost.Value.ToString(), nmrQtyavailable.Value.ToString(), 
        //nmrQtyReqd.Value.ToString(), lblunitid.Text, nmrCost.Value.ToString(), txtdose.Trim() + " " + 
        //            comboUnitId.Text.Trim(), comboInterval.Text, comboDuration.Text, comboWhenHow.Text,
        //            txtRxInstructions.Text.Trim(), mcumgv.ToString(), minterval.ToString(), mduration.ToString(),
        //            mdose.ToString(), (iscapitated) ? "YES" : "NO", "NO", newrec? "0" : listView1.Items[recno].SubItems[18].ToString(), 
        //            nmrUnitPurchaseValue.Value.ToString(), mstkcode, "", alertforConsumables? "YES" : "NO" };

    //            var listViewItem = new ListViewItem(row);
    //            if (!newrec)
    //            {
    //                listView1.Items[recno].Remove();
    //                counter = newcounter;
    //            }
    //            listView1.Items.Add(listViewItem);
    //        }
    //        int xrow = listView1.SelectedIndex;
    //        if (xrow != -1)
    //        {
    //            listView1.Items[xrow].UseItemStyleForSubItems = false;
    //            listView1.Items[xrow].SubItems[5].BackColor = Color.DeepPink;
    //        }
    //        if (newrec && msection == "8") //for medhist note
    //        {
    //            phamalterstr = phamalterstr + "Added - " + txtDescription.Text.Trim() + " Qty_Prd." +
    //                    nmrQtyReqd.Value.ToString().Trim() + " : " + txtdose.Trim() + " " + comboUnitId.Text.Trim() + " x " +
    //                    comboInterval.Text.Trim() + " x " + comboDuration.Text.Trim() + " Cost : " + nmrCurrentTotal.Value.ToString().Trim() +
    //                    " By : " + woperator.Trim() + " " + DateTime.Now.ToString("mm-DD-yyyy @ HH:mm:ss") + "\r\n";
    //        }
    //        //  lv_written = true;
    //        anycode = "";
    //        btnsave.Enabled = true;
    //        ClearControls("");
    //        txtStkCode.Focus();
    //        return;
    //    }

    public MR_DATA.REPORTS btnsave_Click(MR_DATA.REPORTS dataObject, IEnumerable<MR_DATA.DISPENSA> tableList, billchaindtl phbchain)
    {
            //if ( !mcanalter || !mcanadd )
            //{
            //    DialogResult result = MessageBox.Show("Access Denied...","Prescriptions Management");
            //    return;
            //}

        mtrans_date = Convert.ToDateTime(dataObject.txtclinic);
        mdoctor = dataObject.mdoctor;
        reference = dataObject.txtreference;

        if (medhistupdateallowed > 0 && Convert.ToDecimal(DateTime.Now.Date.Subtract(mtrans_date).TotalDays) >= medhistupdateallowed)
        {
            vm.REPORTS.alertMessage = "Patients' Medical History Notes Update RESTRICTED...";
            return vm.REPORTS;
        }

        ////msgeventtracker = "SP";
        //DialogResult result1 = MessageBox.Show("Confirm to Write Prescriptions...", "Prescriptions Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

        //if (result1 == DialogResult.No)
        //    return;

        vm.REPORTS.btnFamilyGroup = false; //btnsave.Enabled;

        string mdescription, mxdesc = "", inprtnstring = ""; //xbillonacctname, nhisdescription
                                                             // bool foundit, xnew;
        decimal xunitp; //curcredit,xamount = 0m,
                        //Array tempa_;
                        //   msmrfunc.mrGlobals.waitwindowtext = "Saving/Adjusting Records - Please Wait !!!";

        //ListViewItem lv = new ListViewItem();
        //   pleaseWait.Show();

        if (dataObject.chkStaffProfiling)
        {
            string xstring = (dataObject.chkBroughtForward) ? "T.T.H." : "Inpatient";
            //msmrfunc.mrGlobals.inp2medhist = 
            inprtnstring = "==> " + xstring + " Prescription Details - " + 
                    DateTime.Now.ToString("dd-MM-yyyy @ HH:mm") + "  By : " + mdoctor.Trim() + "\r\n";
        }
        try
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            connection.Open();

            foreach(var row in tableList)
            {
                if (row.POSTED == true) //dispensed before now
                    continue;

                SqlCommand insertCommand = new SqlCommand();

                if (Convert.ToInt32(row.RECID) > 0)
                {
                    insertCommand.CommandText = (dataObject.chkStaffProfiling) ? "INPDISPENSA_Update" : "DISPENSA_Update";
                }
                else
                {
                    insertCommand.CommandText = (dataObject.chkStaffProfiling) ? "INPDISPENSA_Add" : "DISPENSA_Add";
                }

                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                if (dataObject.chkStaffProfiling && (Convert.ToInt32(row.RECID) < 1 || row.DIAG == "YES"))
                {
                    //3/08/2011 - 3.45 we attempt to pass array to medhist platform for fresh inpatient prescriptions
                    //we cummulate to write to medhist msmrfunc.mrGlobals.inp2medhist
                    inprtnstring += "\r\n" + row.PATIENTNO + " " + row.STK_DESC + "  : " + row.CDOSE + "  : " +
                        row.CINTERVAL.Trim() + " FOR " + row.CDURATION;
                }

                //13.05.2019 change in TTH process

                if (dataObject.chkBroughtForward) //no need to write 
                    continue;

                //init variable for update to billing extended description
                xunitp = Convert.ToDecimal(row.COST.ToString()) / Convert.ToDecimal(row.QTY_PR.ToString());
                mxdesc += "(" + row.STK_DESC.Trim() + " = " + row.QTY_PR.ToString().Trim() + " @ " +
                    xunitp.ToString().Trim() + " : " + row.COST.ToString().Trim() + ")\r\n";

                insertCommand.Parameters.AddWithValue("@itemno", Convert.ToDecimal(row.PATIENTNO));
                insertCommand.Parameters.AddWithValue("@stk_desc", row.STK_DESC);
                insertCommand.Parameters.AddWithValue("@stk_item", row.STK_ITEM);
                insertCommand.Parameters.AddWithValue("@unitcost", Convert.ToDecimal(row.UNITCOST.ToString()));
                //    arr[4] = " "; //for qty available only during data entry
                insertCommand.Parameters.AddWithValue("@qty_pr", Convert.ToDecimal(row.QTY_PR.ToString()));
                insertCommand.Parameters.AddWithValue("@unit", row.UNIT.ToString());
                insertCommand.Parameters.AddWithValue("@cost", Convert.ToDecimal((row.COST.ToString())));
                insertCommand.Parameters.AddWithValue("@cdose", row.CDOSE.ToString());
                insertCommand.Parameters.AddWithValue("@cinterval", row.CINTERVAL);
                insertCommand.Parameters.AddWithValue("@cduration", row.CDURATION);
                insertCommand.Parameters.AddWithValue("@rx", row.RX);
                insertCommand.Parameters.AddWithValue("@sp_inst", row.SP_INST);
                // arr[13] = row.Columns["CUMGV"].ToString();
                insertCommand.Parameters.AddWithValue("@Interval", Convert.ToDecimal(row.INTERVAL.ToString()));
                insertCommand.Parameters.AddWithValue("@duration", Convert.ToDecimal(row.DURATION.ToString()));
                insertCommand.Parameters.AddWithValue("@dose", Convert.ToDecimal(row.DOSE.ToString()));
                insertCommand.Parameters.AddWithValue("@capitated", row.CAPITATED.ToString() == "true" ? true : false);
                // arr[18] = "YES"; // indicates record was saved before.. needed during delete
                insertCommand.Parameters.AddWithValue("@Reference", reference);
                insertCommand.Parameters.AddWithValue("@Name", phbchain.NAME);
                insertCommand.Parameters.AddWithValue("@Groupcode", phbchain.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@Patientno", phbchain.PATIENTNO);
                insertCommand.Parameters.AddWithValue("@Doctor", mdoctor);
                insertCommand.Parameters.AddWithValue("@trans_date", mtrans_date);

                insertCommand.Parameters.AddWithValue("@STORE", "");
                insertCommand.Parameters.AddWithValue("@QTY_GV", 0m);
                insertCommand.Parameters.AddWithValue("@CUMGV", 0m);
                insertCommand.Parameters.AddWithValue("@NURSE", "");
                insertCommand.Parameters.AddWithValue("@DIAG", "");
                insertCommand.Parameters.AddWithValue("@POSTED", false);
                insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@STKBAL", 0m);
                insertCommand.Parameters.AddWithValue("@TIME", "");
                insertCommand.Parameters.AddWithValue("@TYPE", "");
                insertCommand.Parameters.AddWithValue("@GROUPHEAD", phbchain.GROUPHEAD);
                insertCommand.Parameters.AddWithValue("@GHGROUPCODE", phbchain.GHGROUPCODE);
                insertCommand.Parameters.AddWithValue("@GROUPHTYPE", phbchain.GROUPHTYPE);
                insertCommand.Parameters.AddWithValue("@OPERATOR", woperator);
                insertCommand.Parameters.AddWithValue("@OP_TIME", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@unitpurvalue", Convert.ToDecimal(row.unitpurvalue.ToString()));
                
                if (dataObject.chkStaffProfiling)
                    insertCommand.Parameters.AddWithValue("@phtransferred", false);

                if (Convert.ToInt32(row.RECID) > 0)
                    insertCommand.Parameters.AddWithValue("@RECID", Convert.ToInt32(row.RECID.ToString().Trim()));

                insertCommand.ExecuteNonQuery();

            }

            connection.Close();

            if (dataObject.chkStaffProfiling && !string.IsNullOrWhiteSpace(dataObject.edtspinstructions))
            {
                //msmrfunc.mrGlobals.inp2medhist
                inprtnstring += "\r\n" + "Sp.Instructions -> " + dataObject.edtspinstructions + "\r\n";
            }

        }
        catch (SqlException ex)
        {
            vm.REPORTS.ActRslt = ex.Message + ", " + ex.GetType().ToString();
        }
        catch (Exception ex)
        {
                vm.REPORTS.ActRslt = ex.Message + ", " + ex.GetType().ToString();
        }

        //ADDED 26/10/2011 for TTH Alert to the Pharmacist and Admission Nurses
        if (dataObject.chkStaffProfiling && dataObject.chkBroughtForward) //we post to OPD Pharmacy
        {
            try
            {
                //ListViewItem lv4tth = new ListViewItem();
                SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                connection.Open();

                foreach(var row in tableList)
                {
                    if (row.POSTED == true) //not dispensed before now
                        continue;

                    SqlCommand insertCommand = new SqlCommand();
                    if (Convert.ToInt32(row.RECID) > 0) //saved before now
                    {
                        insertCommand.CommandText = "DISPENSA_Update";
                    }
                    else
                    {
                        insertCommand.CommandText = "DISPENSA_Add";
                    }

                    insertCommand.Connection = connection;
                    insertCommand.CommandType = CommandType.StoredProcedure;

                    insertCommand.Parameters.AddWithValue("@itemno", Convert.ToDecimal(row.PATIENTNO));
                    insertCommand.Parameters.AddWithValue("@stk_item", row.STK_ITEM);
                    insertCommand.Parameters.AddWithValue("@stk_desc", row.STK_DESC);

                    insertCommand.Parameters.AddWithValue("@unitcost", Convert.ToDecimal(row.UNITCOST));
                    //    arr[4] = " "; //for qty available only during data entry 
                    insertCommand.Parameters.AddWithValue("@qty_pr", row.QTY_PR.ToString());
                    insertCommand.Parameters.AddWithValue("@unit", row.UNIT.ToString());
                    insertCommand.Parameters.AddWithValue("@cost", row.COST.ToString());
                    insertCommand.Parameters.AddWithValue("@cdose", row.CDOSE.ToString());
                    insertCommand.Parameters.AddWithValue("@cinterval", row.CINTERVAL);
                    insertCommand.Parameters.AddWithValue("@cduration", row.CDURATION);
                    insertCommand.Parameters.AddWithValue("@rx", row.RX);
                    insertCommand.Parameters.AddWithValue("@sp_inst", row.SP_INST);
                    // arr[13] = row.Columns["CUMGV"].ToString();
                    insertCommand.Parameters.AddWithValue("@Interval", Convert.ToDecimal(row.INTERVAL.ToString()));
                    insertCommand.Parameters.AddWithValue("@duration", Convert.ToDecimal(row.DURATION.ToString()));
                    insertCommand.Parameters.AddWithValue("@dose", Convert.ToDecimal(row.DOSE.ToString()));
                    insertCommand.Parameters.AddWithValue("@capitated", row.CAPITATED == true ? true : false);
                    // arr[18] = "YES"; // indicates record was saved before.. needed during delete
                    insertCommand.Parameters.AddWithValue("@Reference", reference);
                    insertCommand.Parameters.AddWithValue("@Name", phbchain.NAME);
                    insertCommand.Parameters.AddWithValue("@Groupcode", phbchain.GROUPCODE);
                    insertCommand.Parameters.AddWithValue("@Patientno", phbchain.PATIENTNO);
                    insertCommand.Parameters.AddWithValue("@Doctor", mdoctor);
                    insertCommand.Parameters.AddWithValue("@trans_date", mtrans_date);

                    insertCommand.Parameters.AddWithValue("@STORE", "");
                    insertCommand.Parameters.AddWithValue("@QTY_GV", 0m);
                    insertCommand.Parameters.AddWithValue("@CUMGV", 0m);
                    insertCommand.Parameters.AddWithValue("@NURSE", "");
                    insertCommand.Parameters.AddWithValue("@DIAG", "");
                    insertCommand.Parameters.AddWithValue("@POSTED", false);
                    insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@STKBAL", 0m);
                    insertCommand.Parameters.AddWithValue("@TIME", "");
                    insertCommand.Parameters.AddWithValue("@TYPE", "");
                    insertCommand.Parameters.AddWithValue("@GROUPHEAD", phbchain.GROUPHEAD);
                    insertCommand.Parameters.AddWithValue("@GHGROUPCODE", phbchain.GHGROUPCODE);
                    insertCommand.Parameters.AddWithValue("@GROUPHTYPE", phbchain.GROUPHTYPE);
                    insertCommand.Parameters.AddWithValue("@OPERATOR", woperator);
                    insertCommand.Parameters.AddWithValue("@OP_TIME", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@unitpurvalue", Convert.ToDecimal(row.unitpurvalue));

                    if (Convert.ToInt32(row.RECID) > 0)
                        insertCommand.Parameters.AddWithValue("@RECID", Convert.ToInt32(row.RECID));

                    insertCommand.ExecuteNonQuery();
                }

                connection.Close();

                if (msection != "8")
                {
                    LINK.WriteLINK(0, phbchain.GROUPCODE, phbchain.PATIENTNO, phbchain.NAME, "8", reference, 
                        dataObject.nmrAmountTo, 0, mfacility, true, mdoctor, false, 8, "", msection, woperator);
                }

            }
            catch (SqlException ex)
            {
                vm.REPORTS.ActRslt = ex.Message + ", " + ex.GetType().ToString();
            }
            catch (Exception ex)
            {
                vm.REPORTS.ActRslt = ex.Message + ", " + ex.GetType().ToString();
            }
        }

        if (!string.IsNullOrWhiteSpace(phamalterstr)) //update to medhist if Pharmacist alter prescription
        {
            MedHist medhist = MedHist.GetMEDHIST(phbchain.GROUPCODE, phbchain.PATIENTNO, "", false, true, mtrans_date, "DESC");
            bool newhist = (medhist == null) ? true : false;
            string xcomments = "";

            if (newhist)
                xcomments = phamalterstr;
            else
            {
                xcomments = medhist.COMMENTS.Trim() + "\r\n" + phamalterstr.Trim();
            }

            MedHist.updatemedhistcomments(phbchain.GROUPCODE, phbchain.PATIENTNO, mtrans_date, xcomments, newhist,
                reference, phbchain.NAME, phbchain.GHGROUPCODE, phbchain.GROUPHEAD, mdoctor);
        }

        if (!dataObject.chkStaffProfiling & new string[] { "1", "2", "3", "4", "5", "8", "9" }.Contains(msection))
        {
            //if (mdrgmarkup != 0m) //outpatient - MOVED 17.05.2019 TO INDIVIDUAL STOCK ITEMS
            //{
            //    if (markupdrgbillonPercentage)
            //        nmrCurrentTotal.Value += ((nmrCurrentTotal.Value * mdrgmarkup) / 100);
            //    else
            //        nmrCurrentTotal.Value += mdrgmarkup;
            //}
            if (msection != "8")
            {
                bool linkok = cashpaying || phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge ? false : true;
                LINK.WriteLINK(0, phbchain.GROUPCODE, phbchain.PATIENTNO, phbchain.NAME, "8", reference, 
                    dataObject.nmrAmountTo, 0, mfacility, linkok, mdoctor, false, 0, "", msection, woperator);

            }

            //write details to billing

            mdescription = "";
            msmrfunc.getFeefromtariff(mdrugcode, "", ref mdescription, ref mfacility);
            mdescription = (string.IsNullOrWhiteSpace(mdescription)) ? "DRUGS" : mdescription;
            //nhisdescription = "NHIS 10% DRUG CHARGE";
            bool isbillonaccount = false;

            if (!string.IsNullOrWhiteSpace(phbchain.BILLONACCT))
            {
                BillOnAcct = billchaindtl.Getbillchain(phbchain.BILLONACCT, phbchain.GROUPCODE);
                isbillonaccount = (BillOnAcct != null) ? true : false;
            }

            string servicetyp = "";
            decimal nhisdrgbill = 0m;

            if (phbchain.GROUPHTYPE == "C" && fee_for_service || phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge)
            {
                servicetyp = "C";  //claim 17/02/2011
            }

            if (phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge)
            {
                nhisdrgbill = Math.Round(dataObject.nmrAmountTo * .10m);
            }
            //WE MUST OVERWRITE PREVIOUS BILL WITH CURRENT SELECTIONS - WE SHOULD GET FACILITY FROM HERE
            int recid = 0;
            //  decimal oldbill = 0m;
            //          decimal mitem = Billings.getBillNextItems(reference,mdrugcode, true, ref oldbill, ref recid );

            decimal xfditem = 0;
            DataTable refbill;
            refbill = Dataaccess.GetAnytable("", "MR", 
                "SELECT itemno, process, amount, recid from billing where reference = '" + reference + 
                "' order by itemno", false); // Billings.GetBILLING(mreference, xprocess);

            if (refbill.Rows.Count > 0)
            {
                xfditem = 0;
                //check if xprocess is part of the old bill
                foreach (DataRow row in refbill.Rows)
                {
                    if (row["process"].ToString().Trim() == mdrugcode.Trim())
                    {
                        xfditem = (decimal)row["itemno"];
                        recid = (Int32)row["recid"];
                        break;
                    }
                }
                if (xfditem < 1)
                    xfditem++;
            }
            else
                xfditem++;

            Billings.writeBILLS(recid > 0 ? false : true, reference, xfditem, mdrugcode, mdescription, phbchain.GROUPHTYPE,
                (phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge) ? nhisdrgbill : dataObject.nmrAmountTo, mtrans_date,
                (isbillonaccount) ? BillOnAcct.NAME : phbchain.NAME, phbchain.GROUPHEAD, mfacility, phbchain.GROUPCODE, 
                (isbillonaccount) ? BillOnAcct.PATIENTNO : phbchain.PATIENTNO, "D", phbchain.GHGROUPCODE, woperator, 
                DateTime.Now, mxdesc, "", 0m, 0, "", mdoctor, false, "", servicetyp, 0m, "", 0m, "O", false, recid);

            /*   /if !EMPTY(billchai.currency) AND billchai.currency # mlocalcur
                    IF m.fxtype=1 OR EMPTY(ThisForm.txtfxtotal.value)
                        replace fcamount WITH amount,amount WITH ROUND(fcamount/curtable.rate,2)
                    ELSE
                        replace fcamount WITH ThisForm.txtfxtotal.value*curtable.rate,amount WITH ThisForm.txtfxtotal.value
                    ENDIF
                    replace exrate with curtable.rate,currency WITH billchai.currency
                ENDIF - PENDING 25-11-2013

                IF m.mdrugsonbill &&write details to mrb25
                thisform.writedetails(billvouc.reference,billvouc.itemno)
                ENDIF
                * 
                */

            if (phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge && recid == 0)
            {
                //11/06/2013 - I think we shuld write balance to capbills for proper accountability of cost of NHIS service
                // "b" servicetyp is sent to capbills
                Billings.writeBILLS(true, reference, xfditem, mdrugcode, "Capitated Balance of NHIS DrugBill", 
                    phbchain.GROUPHTYPE, dataObject.nmrAmountTo - nhisdrgbill, mtrans_date, (isbillonaccount) ? 
                    BillOnAcct.NAME : phbchain.NAME, phbchain.GROUPHEAD, mfacility, phbchain.GROUPCODE, (isbillonaccount) ? 
                    BillOnAcct.PATIENTNO : phbchain.PATIENTNO, "D", phbchain.GHGROUPCODE, woperator, DateTime.Now, mxdesc, 
                    "", 0m, 0, "", mdoctor, false, "", "b", 0m, "", 0m, "O", false, 0);
            }

            if (cashpaying || phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge)
            {
                LINK.WriteLINK(0, phbchain.GROUPCODE, phbchain.PATIENTNO, phbchain.NAME, "2", reference, 
                    (phbchain.GROUPCODE.Trim() == "NHIS" && isnhischarge) ? nhisdrgbill : dataObject.nmrAmountTo, 0, 
                    mfacility, false, mdoctor, false, 0, "", msection, woperator);
            }
        }

        if (dataObject.chkStaffProfiling)
        {
            if (dataObject.chkBroughtForward)
            {
                //UPDATE in-patient bill automatically 12.05.2019 - shabach
                UpdateTTHDetails(tableList, phbchain);
            }
            else
            {
                //send alert to nurses Harmony - 20.06.2019
                string rtnstring = "";
                if (reference.Substring(0, 1) == "C") //opd
                {
                    rtnstring = "==> Admissions Request Details - " +
                    DateTime.Now.ToString("dd-MM-yyyy @ HH:mmtt ") + " - ";

                    rtnstring += "*** OPD ADMISSIONS NOTIFICATION !!! ***" + "\r\n";
                }
                else
                {
                    rtnstring = "==> Inpatient Prescriptions Details - " + DateTime.Now.ToString("dd-MM-yyyy @ HH:mmtt ") + " - ";

                    rtnstring += "*** REQUEST ALERT !!! ***" + "\r\n";
                }

                string xnotes = "";
                Admrecs admr = Admrecs.GetADMRECS(reference);

                if (reference.Substring(0, 1) == "C") //opd
                    xnotes = "From >  : " + mdoctor;
                else
                    xnotes = "From > Wd/Rm : " + admr.ROOM.Trim() + "/" + admr.ROOM.Trim();

                xnotes += "\r\n";

                MRB21.Writemrb21Details(groupcode, patientno, DateTime.Now, phbchain.NAME, mfacility, woperator,
                    xnotes, reference, msection, "3", woperator, woperator, "I");
            }
        }
        else
        {
            bool xfoundit = false;
            //send alert to Pharmacy Shabach - 06.05.2020, IF DRUG REQUIRES CONSUMABLE
            string xnotes = "From > Docs OPD Prescription : " + mdoctor + "\r\n DRUGS With Consumables for  : \r\n";

            foreach (var row in tableList)
            {
                if (row.GROUPCODE != null && row.GROUPCODE == "YES")
                {
                    xfoundit = true;
                    break;
                }

            }
            if (xfoundit)
            {
                MRB21.Writemrb21Details(groupcode, patientno, DateTime.Now, phbchain.NAME, mfacility, woperator, 
                    xnotes, reference, msection, "8", woperator, woperator, "O");
            }

            if (requestalert[0] != "")
            {
                    vm.REPORTS.SessionAddress_ = requestalert; //Session["REQUESTALERT"] = requestalert;
            }
        }

        vm.REPORTS.SessionMorning = inprtnstring;  //Session["inp2medhist"] = inprtnstring;
        //btnclose.PerformClick();

        return vm.REPORTS;
    }

        //private void btnclose_Click(object sender, EventArgs e)
        //{
        //    if (btnsave.Enabled)
        //    {
        //        //msgeventtracker = "BC";
        //        DialogResult result = MessageBox.Show("Changes to Prescription Details have not been saved! Confirm to Close", 
        //          "Prescriptions Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //        if (result == DialogResult.Yes)
        //        {
        //            this.Close();
        //        }
        //    }
        //    else
        //        this.Close();

        //}

        //private void cboRoutineDrgs_LostFocus(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(cboRoutineDrgs.Text))
        //        return;
        //    unitIDsave = "";
        //    DialogResult result = MessageBox.Show("Confirm to Load " + cboRoutineDrgs.Text.Trim() + "...",
        //      "Routine Drugs", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    if (result == DialogResult.No)
        //        return;

        //    DataTable dt = Dataaccess.GetAnytable("", "MR",
        //      "select reference, description, qty, unit, cost, drugs, GLOBAL_DIFF_CHG, CORP_DIFF_CHG, DOSE, " +
        //      " INTERVAL, DURATION, CDOSE, CINTERVAL, CDURATION, WHENHOW FROM routdrgs where reference = '" +
        //      cboRoutineDrgs.Text + "' order by description", false);

        //    string[] arr = new string[21];
        //    ListViewItem itm;
        //    decimal xcost = 0m;
        //    bool preauthorization = false, tocontue = false, capitated = false;
        //    int xrow = listView1.Items.Count;
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        if (((bool)row["global_diff_chg"] || (bool)row["corp_diff_chg"])) //&& phbchain.GROUPHTYPE == "C")
        //        {
        //            if (!string.IsNullOrWhiteSpace(phbchain.HMOSERVTYPE)) //hmo patient
        //                xcost = msmrfunc.CheckCorpPatientStkDefined(phbchain.GROUPHEAD, phbchain.GROUPHTYPE, phbchain.HMOSERVTYPE,
        //                  phbchain.GROUPCODE, false, inpatient, ref xcost, customer.noncapitation, row["drugs"].ToString(),
        //                  ref preauthorization, ref capitated, ref tocontue, row["description"].ToString());
        //            else
        //                xcost = msmrfunc.othercorpClientTariffCheck(phbchain.PATIENTNO, phbchain.PATCATEG,
        //                  ref preauthorization, ref tocontue, row["description"].ToString(), row["drugs"].ToString());
        //        }
        //        else
        //            xcost = (decimal)row["cost"];

        //        nmrCurrentTotal.Value += xcost;

        //        arr[0] = (xrow + 1).ToString();
        //        arr[1] = row["description"].ToString();
        //        arr[2] = xcost.ToString("N2"); // row["cost"].ToString();
        //        arr[3] = "0"; //for qty available only during data entry 
        //        arr[4] = row["qty"].ToString();
        //        arr[5] = row["unit"].ToString();
        //        arr[6] = row["cost"].ToString();
        //        arr[7] = row["cdose"].ToString();
        //        arr[8] = row["cinterval"].ToString();
        //        arr[9] = row["cduration"].ToString();
        //        arr[10] = row["whenhow"].ToString();
        //        arr[11] = ""; //special instruciton
        //        arr[12] = "0"; //cumgiven
        //        arr[13] = row["interval"].ToString();
        //        arr[14] = row["duration"].ToString();
        //        arr[15] = row["dose"].ToString();
        //        arr[16] = "NO"; // Convert.ToBoolean( row["CAPITATED"]) ) ? "YES" : "NO";
        //        arr[17] = "NO"; // indicates record was saved before.. needed during delete
        //        arr[18] = "0";
        //        arr[19] = "0"; // row["unitpurvalue"].ToString();
        //        arr[20] = row["drugs"].ToString();
        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);
        //        listView1.Items[xrow].UseItemStyleForSubItems = false;
        //        listView1.Items[xrow].SubItems[4].BackColor = Color.Red;
        //        xrow++;
        //    }
        //    dt.Clear();
        //    if (xrow > 0)
        //        btnsave.Enabled = true;

        //}

        //private void btnTIPS_Click(object sender, EventArgs e)
        //{
        //    frmRxguidline rxguidline = new frmRxguidline();
        //    rxguidline.Show();
        //}

        //private void btninpatprescdtl_Click(object sender, EventArgs e)
        //{
        //    frmPatientsPrescriptionDetails prescdtl = new frmPatientsPrescriptionDetails(phbchain.GROUPCODE,
        //        phbchain.PATIENTNO, phbchain.NAME, mtrans_date);

        //    //prescdtl.Show();
        //}

        //private void chkClearSelections_Click(object sender, EventArgs e)
        //{

        //}

        ////private void listView1_DoubleClick(object sender, EventArgs e)
        ////{
        ////    recno = listView1.SelectedIndex;
        ////    ListViewItem lv = listView1.Items[recno];
        ////    this.txtDescription.Text = lv.SubItems[1].ToString(); // bissclass.combodisplayitemCodeName("item", lv.SubItems[20].ToString(), dtstock, "name");
        ////                                                          //cboDescription.SelectedValue = lv.SubItems[20].ToString();
        ////    txtStkCode.Text = anycode = lv.SubItems[20].ToString();
        ////    txtStkCode.Focus();
        ////    // anycode = cboDescription.Text;
        ////    //this.cboDescription.Focus();
        ////    return;
        ////}

        ////private void btnClearSelection_Click(object sender, EventArgs e)
        ////{
        ////    if (listView1.Items.Count < 1)
        ////        return;
        ////    DialogResult result = MessageBox.Show("Confirm to Remove Selections... ", "Prescriptions",
        //          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        ////    if (result == DialogResult.No)
        ////        return;
        ////    counter = 0m;
        ////    listView1.Items.Clear();
        ////    loadprevDefinitions();
        ////    /*   foreach (ListViewItem lv in listView1.Items)
        ////       {
        ////           if (lv.SubItems[18].Text.Trim() == "YES")
        ////               continue;
        ////           nmrCurrentTotal.Value -= Convert.ToDecimal(lv.SubItems[6].Text);
        ////           lv.Remove();
        ////       }*/
        ////}

        ////private void chkinpatient_CheckedChanged(object sender, EventArgs e)
        ////{
        ////    if (chkinpatient.Checked && !inpatient)
        ////    {
        ////        DialogResult result = MessageBox.Show("OPD -> In-Patient Prescriptions Conversion ? \r\n This Consult will " +
        //          "be converted on Submission of Details \r\n on Main Consult Platform...CONFIRM", "Prescriptions",
        //          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        ////        if (result == DialogResult.No)
        ////        {
        ////            chkinpatient.Checked = false;
        ////            return;
        ////        }

        ////        Session["Inpatient"] = "Y";
        ////        inpatient = true;
        ////        chkinpatient.Enabled = btnConvert.Enabled = false;
        ////    }
        ////}

        public MR_DATA.REPORTS btnConvert_Click(IEnumerable<MR_DATA.DISPENSA> tableList, billchaindtl phbchain, 
            string revTreatmentDate, string mdoctor, string prescSpecInstrn)
        {
            //if (listView1.Items.Count < 1)
            //{
            //    MessageBox.Show("No Prescription for convert...");
            //    return;
            //}

            //DialogResult result = MessageBox.Show("Confirm to Convert This Precription to In-Patient \r\n NOTE :" +
            //  " Patient must have been admitted and Admission Reference Available... CONTINUE ?", "Prescriptions",
            //  MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            //if (result == DialogResult.No)
            //    return;

            mtrans_date = Convert.ToDateTime(revTreatmentDate);

            string xval = "";
            POPREAD popread = new POPREAD("OPD Prescriptions -> In-Patient", "Enter Admission Reference...", 
                ref xval, false, false, "", "", "", false, "", "");

            //popread.ShowDialog();

            xval = bissclass.sysGlobals.anycode;

            if (string.IsNullOrWhiteSpace(xval))
                return vm.REPORTS;

            DataTable dt = Dataaccess.GetAnytable("", "MR", 
                "select name, groupcode, patientno from admrecs where reference = '" + xval + "'", false);

            if (dt.Rows.Count < 1)
            {
                vm.REPORTS.alertMessage = "Invalid Admission Reference...";
                return vm.REPORTS;
            }

            DataRow row = dt.Rows[0];
            if (row["groupcode"].ToString() != groupcode || row["patientno"].ToString() != patientno)
            {
                vm.REPORTS.ActRslt = "Specified Reference belongs to another Patient - \r\n " + row["name"].ToString() +
                    " " + row["groupcode"].ToString() + ":" + row["patientno"].ToString();

                return vm.REPORTS;
            }

            string inprtnstring = "";
            //ListViewItem lv = new ListViewItem();
            bool worked = false;
            string xstring = "Inpatient";
            //msmrfunc.mrGlobals.inp2medhist = 
            inprtnstring = "==> " + xstring + " Prescription Details - " + DateTime.Now.ToString("dd-MM-yyyy @ HH:mm") + 
                "  By : " + mdoctor.Trim() + "\r\n";

            try
            {
                SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
                connection.Open();

                foreach (var row2 in tableList)
                {
                    //lv = listView1.Items[i];
                    if ((bool)row2.POSTED) //dispensed before now
                        continue;

                    SqlCommand insertCommand = new SqlCommand();

                    insertCommand.CommandText = "INPDISPENSA_Add";
                    insertCommand.Connection = connection;
                    insertCommand.CommandType = CommandType.StoredProcedure;

                    inprtnstring += "\r\n" + row2.PATIENTNO + " " + row2.STK_DESC + "  : " + row2.CDOSE.ToString() +
                    "  : " + row2.CINTERVAL + " FOR " + row2.CDURATION;

                    insertCommand.Parameters.AddWithValue("@itemno", Convert.ToDecimal(row2.PATIENTNO));
                    insertCommand.Parameters.AddWithValue("@stk_desc", row2.STK_DESC);
                    insertCommand.Parameters.AddWithValue("@stk_item", row2.STK_ITEM);
                    insertCommand.Parameters.AddWithValue("@unitcost", Convert.ToDecimal(row2.UNITCOST.ToString()));

                    insertCommand.Parameters.AddWithValue("@qty_pr", Convert.ToDecimal(row2.QTY_PR.ToString()));
                    insertCommand.Parameters.AddWithValue("@unit", row2.UNIT.ToString());
                    insertCommand.Parameters.AddWithValue("@cost", Convert.ToDecimal((row2.COST.ToString())));
                    insertCommand.Parameters.AddWithValue("@cdose", row2.CDOSE.ToString());
                    insertCommand.Parameters.AddWithValue("@cinterval", row2.CINTERVAL);
                    insertCommand.Parameters.AddWithValue("@cduration", row2.CDURATION);
                    insertCommand.Parameters.AddWithValue("@rx", row2.RX);
                    insertCommand.Parameters.AddWithValue("@sp_inst", row2.SP_INST);

                    insertCommand.Parameters.AddWithValue("@Interval", Convert.ToDecimal(row2.INTERVAL.ToString()));
                    insertCommand.Parameters.AddWithValue("@duration", Convert.ToDecimal(row2.DURATION.ToString()));
                    insertCommand.Parameters.AddWithValue("@dose", Convert.ToDecimal(row2.DOSE.ToString()));
                    insertCommand.Parameters.AddWithValue("@capitated", (bool)row2.CAPITATED);
                    insertCommand.Parameters.AddWithValue("@Reference", xval);
                    insertCommand.Parameters.AddWithValue("@Name", phbchain.NAME);
                    insertCommand.Parameters.AddWithValue("@Groupcode", phbchain.GROUPCODE);
                    insertCommand.Parameters.AddWithValue("@Patientno", phbchain.PATIENTNO);
                    insertCommand.Parameters.AddWithValue("@Doctor", mdoctor);
                    insertCommand.Parameters.AddWithValue("@trans_date", mtrans_date);

                    insertCommand.Parameters.AddWithValue("@STORE", "");
                    insertCommand.Parameters.AddWithValue("@QTY_GV", 0m);
                    insertCommand.Parameters.AddWithValue("@CUMGV", 0m);
                    insertCommand.Parameters.AddWithValue("@NURSE", "");
                    insertCommand.Parameters.AddWithValue("@DIAG", "");
                    insertCommand.Parameters.AddWithValue("@POSTED", false);
                    insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@STKBAL", 0m);
                    insertCommand.Parameters.AddWithValue("@TIME", "");
                    insertCommand.Parameters.AddWithValue("@TYPE", "");
                    insertCommand.Parameters.AddWithValue("@GROUPHEAD", phbchain.GROUPHEAD);
                    insertCommand.Parameters.AddWithValue("@GHGROUPCODE", phbchain.GHGROUPCODE);
                    insertCommand.Parameters.AddWithValue("@GROUPHTYPE", phbchain.GROUPHTYPE);
                    insertCommand.Parameters.AddWithValue("@OPERATOR", woperator);
                    insertCommand.Parameters.AddWithValue("@OP_TIME", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@unitpurvalue", Convert.ToDecimal(row2.unitpurvalue.ToString()));
                    insertCommand.Parameters.AddWithValue("@phtransferred", false);
                    /*   if ( Convert.ToInt32( lv.SubItems[18].Text.Trim()) > 0 )
                           insertCommand.Parameters.AddWithValue("@RECID", Convert.ToInt32(lv.SubItems[18].ToString().Trim()));*/

                    insertCommand.ExecuteNonQuery();

                }
                connection.Close();
                inprtnstring += "\r\n" + "Sp.Instructions -> " + prescSpecInstrn + "\r\n";
                worked = true;
            }
            catch (SqlException ex)
            {
                vm.REPORTS.ActRslt = ex.Message + ", " + ex.GetType().ToString();
            }
            catch (Exception ex)
            {
                vm.REPORTS.ActRslt = ex.Message + ", " + ex.GetType().ToString();
            }
            if (worked)
            {
                string updstr = "delete from dispensa where reference = '" + reference + "'";
                bissclass.UpdateRecords(updstr, "MR");
                //listView1.Items.Clear();
            }

            vm.REPORTS.alertMessage = "Completed...";

            return vm.REPORTS;
        }

        void UpdateTTHDetails(IEnumerable<MR_DATA.DISPENSA> tableList, billchaindtl phbchain)
        {
            // bool medhistwritten = false;
            //ListViewItem lv = new ListViewItem();

            foreach (var row in tableList)
            {
                //lv = listView1.Items[i];
                if (row.POSTED == true) //dispensed before now
                    continue;

                ADMDETAI.writeAdmdetails(true, reference, mtrans_date, DateTime.Now.ToShortTimeString(), mdrugcode, 
                    mdrugcode, row.STK_ITEM, row.STK_DESC, row.UNIT.ToString(), Convert.ToDecimal(row.QTY_PR.ToString()),
                    Convert.ToDecimal(row.COST.ToString()), true, DateTime.Now, woperator, DateTime.Now, phbchain.GROUPCODE,
                    phbchain.PATIENTNO, mdoctor, mfacility, 0, pharmacyStore);

                Admrecs.UpdateAdmrecAmounts(reference, Convert.ToDecimal(row.COST.ToString()), 0m);

                //details of prescription already sent to Review Page.
                /*     string billat = Convert.ToDecimal(lv.SubItems[6].ToString()) + " BY " + mdoctor + " (TTH)";
                     string xcomments = "Billed at " + billat;
                     //update med history file
                     string oldhist = "";
                     MedHist medhist = MedHist.GetMEDHIST(phbchain.GROUPCODE, phbchain.PATIENTNO, "", false, true, mtrans_date);
                     bool newhist = (medhist == null) ? true : false;
                     if (!newhist)
                     {
                         oldhist = medhist.COMMENTS.Trim() + "\r\n\r\n"; // +xcomments.Trim();
                     }
                     if (!medhistwritten)
                     {
                         xcomments = oldhist + "==> IN-PATIENT PRESCRIPTIONS - Given; (Updated) :" + DateTime.Now.ToString("dd-MM-yyyy @ HH:mm") + " : " + woperator + "\r\n" + xcomments;
                         medhistwritten = true;
                     }
                     else
                         xcomments = oldhist + xcomments;

                     MedHist.updatemedhistcomments(phbchain.GROUPCODE, phbchain.PATIENTNO, mtrans_date, xcomments, newhist, reference, phbchain.NAME, phbchain.GHGROUPCODE, phbchain.GROUPHEAD, "");
                     medhistwritten = true;*/
            }
        }

        ////private void txtStkCode_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        ////{
        ////    // txtStkCode_TextChanged(null, null);
        ////    if (!string.IsNullOrWhiteSpace(txtStkCode.Text) && !string.IsNullOrWhiteSpace(txtDescription.Text))
        ////        SelectNextControl(ActiveControl, true, true, true, true);
        ////    else
        ////        txtStkCode_TextChanged(null, null);

        ////}

        ////private void comboUnitId_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    comboUnitId_LostFocus(null, null);
        ////}

        ////private void comboInterval_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    comboInterval_LostFocus(null, null);
        ////}

        ////private void comboDuration_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    comboDuration_LostFocus(null, null);
        ////}

        ////private void nmrDose_KeyDown(object objSender, KeyEventArgs objArgs)
        ////{
        ////    if (objArgs.KeyCode == Keys.Enter)
        ////        nmrDose_LostFocus(null, null);

        ////}

        ////private void comboUnitId_KeyDown(object objSender, KeyEventArgs objArgs)
        ////{
        ////    if (objArgs.KeyCode == Keys.Enter)
        ////    {
        ////        comboUnitId_LostFocus(null, null);
        ////        SelectNextControl(ActiveControl, true, true, true, true);
        ////    }
        ////}

        ////private void comboInterval_KeyDown(object objSender, KeyEventArgs objArgs)
        ////{
        ////    if (objArgs.KeyCode == Keys.Enter)
        ////    {
        ////        comboInterval_LostFocus(null, null);
        ////        SelectNextControl(ActiveControl, true, true, true, true);
        ////    }
        ////}

        ////private void comboDuration_KeyDown(object objSender, KeyEventArgs objArgs)
        ////{
        ////    if (objArgs.KeyCode == Keys.Enter)
        ////    {
        ////        comboDuration_LostFocus(null, null);
        ////        SelectNextControl(ActiveControl, true, true, true, true);
        ////    }
        ////}

        //private void chkPrivateAcct_Click(object sender, EventArgs e)
        //{
        //    if (phbchain == null || phbchain.GROUPHTYPE != "C") //|| !customer.HMO )
        //        return;

        //    if (msmrfunc.InitiatePrivateConsultReference(phbchain.PATIENTNO, "Prescription Management", mfacility, mdoctor, woperator, phbchain))
        //        chkPrivateAcct.Checked = false;

        //}

        ////void renumberview()
        ////{
        ////    counter = 0;
        ////    for (int i = 0; i < listView1.Items.Count; i++)
        ////    {
        ////        listView1.Items[i].SubItems[0].Text = (i + 1).ToString();
        ////        counter = Convert.ToDecimal(i + 1);
        ////    }
        ////    counter++;
        ////}
    }
}
