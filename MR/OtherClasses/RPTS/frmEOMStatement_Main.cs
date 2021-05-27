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
using MSMR.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmEOMStatement_main : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, sysmodule = bissclass.getRptfooter(),pat_type;
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtcprptdef, dtcurrency = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM currencycodes order by name", true),
            dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES", false);
        public frmEOMStatement_main()
        {
            InitializeComponent();
        }
        private void frmEOMStatement_main_Load(object sender, EventArgs e)
        {
            pat_type = "C";
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            cboCurrency.DataSource = dtcurrency;
            cboCurrency.ValueMember = "type_code";
            cboCurrency.DisplayMember = "Name";

        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btngroupcode")
            {
                this.txtgroupcode.Text = "";
                lookupsource = "g";
                msmrfunc.mrGlobals.crequired = "g";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btnPatientno")
            {
                txtpatientno.Text = "";
                lookupsource = "L";
                msmrfunc.mrGlobals.crequired = "L";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btnCorporate")
            {
                txtgrouphead.Text = "";
                lookupsource = "C";
                msmrfunc.mrGlobals.crequired = "C";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORP. CLIENTS";
            }
            else if (btn.Name == "btnGrpReference")
            {
                txtGrpRef.Text = "";
                lookupsource = "EOM";
                msmrfunc.mrGlobals.crequired = "EOMSTATMT";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR SAVED E-O-M Statement List Defintion";
            }
            else if (btn.Name == "btnReference")
            {
                txtSortBills.Text = "";
                lookupsource = "BILL";
                msmrfunc.mrGlobals.crequired = "BILL";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR SAVED PATIENT BILLS";
            }
            else if (btn.Name == "btnAdjustRef")
            {
                txtAdjRef.Text = "";
                lookupsource = "BADJ";
                msmrfunc.mrGlobals.crequired = "BADJ";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR SAVED BILLS ADJUSTMENTS";
            }
            frmselcode FrmSelCode = new frmselcode();
            FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
            FrmSelCode.ShowDialog();
        }
        void FrmSelCode_Closed(object sender, EventArgs e)
        {
            frmselcode FrmSelcode = sender as frmselcode;
            if (lookupsource == "g") //groupcodee
            {
                this.txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
                this.txtgroupcode.Focus();
            }
            else if (lookupsource == "L") //patientno
            {
                txtpatientno.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Select();
            }
            else if (lookupsource == "C") //CORPORATE
            {
                txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtgrouphead.Select();
            }
            else if (lookupsource == "EOM") //GROUP REFERENCE
            {
                txtGrpRef.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtGrpRef.Focus();
            }
            else if (lookupsource == "BILL") //GROUP REFERENCE
            {
                txtSortBills.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtSortBills.Focus();
            }
            else if (lookupsource == "BADJ") //GROUP REFERENCE
            {
                txtAdjRef.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtAdjRef.Focus();
            }
            return;
        }
        private void txtPatientno_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpatientno.Text))
                return;
            DialogResult result;
            if (string.IsNullOrWhiteSpace(txtGrpRef.Text) && chkMultiPrint.Checked)
            {
                result = MessageBox.Show("You must Enter A Group Reference to Continue...", "Group Definition");
                txtGrpRef.Focus();
                txtpatientno.Text = "";
                return;
            }
            if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
            {
                txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
            }
            //check if patientno exists
            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME, groupcode, patientno, contact, address1, balbf FROM PATIENT WHERE GROUPCODE = '"+txtgroupcode.Text+"' and patientno = '"+txtpatientno.Text+"'", false);
            if (dtcustomer.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid Patient Reference...");
                txtpatientno.Text = txtgroupcode.Text = "";
                return;
            }
            if (!(bool)dtcustomer.Rows[0]["isgrouphead"])
            {
                result = MessageBox.Show("This Patient is not A Grouphead...");
                txtpatientno.Text = txtgroupcode.Text = "";
                return;
            }
            txtName.Text = dtcustomer.Rows[0]["name"].ToString();
            chkInclude.Enabled = chkExclude.Enabled = true;
            if (chkMultiPrint.Checked)
            {
                listAdd(dtcustomer, "P");
                txtpatientno.Text = "";
                txtpatientno.Focus();
            }
            else
            {
                listView1.Items.Clear();
                txtGrpRef.Text = txtGrpDescription.Text = "SINGLE";
                listAdd(dtcustomer, "P");
            }
            btnSubmit.Enabled = true;
        }
        private void txtgrouphead_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtgrouphead.Text))
                return;
            DialogResult result;
            if (string.IsNullOrWhiteSpace(txtGrpRef.Text) && chkMultiPrint.Checked)
            {
                result = MessageBox.Show("You must Enter A Group Reference to Continue...", "Group Definition");
                txtGrpRef.Focus();
                txtgrouphead.Text = "";
                return;
            }

            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME, custno, contact, address1, balbf FROM CUSTOMER WHERE CUSTNO = '" + txtgrouphead.Text + "'", false);
            if (dtcustomer.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid Corporate Clients Reference...");
                txtgrouphead.Text = "";
                return;
            }
            txtName.Text = dtcustomer.Rows[0]["name"].ToString();
            chkInclude.Enabled = chkExclude.Enabled = true;
            if (chkMultiPrint.Checked)
            {
                listAdd(dtcustomer, "C");
                txtgrouphead.Text = "";
                txtgrouphead.Focus();
            }
            else
            {
                listView1.Items.Clear();
                txtGrpRef.Text = txtGrpDescription.Text = "SINGLE";
                listAdd(dtcustomer, "C");
            }
            btnSubmit.Enabled = true;
        }
        void listAdd( DataTable xdt, string xrectype)
        {
            if (txtGrpRef.Text.Trim() == "SINGLE")
            {
                listView1.Items.Clear(); //must check for previous defintion with single as reference. Only a record allowed
                dtcprptdef = Dataaccess.GetAnytable("", "MR", "SELECT * FROM cprptdef where reference = '" + txtGrpRef.Text + "'", false);

                if (dtcprptdef.Rows.Count > 0)
                {
                    LoadListview1();
                }
            }
            //check if name on list
            ListViewItem itm = null;
            DataRow row = null;
            bool foundit = false;
            for (int i = 0; i < listView1.Items.Count; i++)
			{
                itm = listView1.Items[i];
                row = xdt.Rows.Count < 2 ? xdt.Rows[0] : xdt.Rows[i];
                if ( xrectype == "C" && itm.SubItems[1].ToString().Trim() == row["custno"].ToString().Trim() || xrectype == "P" && itm.SubItems[1].ToString().Trim() == row["patientno"].ToString().Trim() && itm.SubItems[2].ToString().Trim() == row["groupcode"].ToString().Trim() )
                {
                    foundit = true;
                    break;
                }
            }
            if (foundit )
            {
                if (txtGrpRef.Text.Trim() != "SINGLE")
                {
                    DialogResult result = MessageBox.Show("This Client is already on this List...");
                }
                itm.SubItems[0].Text = row["name"].ToString();
                itm.SubItems[1].Text = xrectype == "C" ? row["custno"].ToString() : row["patientno"].ToString();
                itm.SubItems[2].Text = xrectype == "C" ? "" : row["groupcode"].ToString();
                itm.SubItems[3].Text = row["contact"].ToString();
                itm.SubItems[4].Text = row["address1"].ToString();
                itm.SubItems[5].Text = row["balbf"].ToString();
                itm.SubItems[6].Text = txtGrpRef.Text;
                itm.SubItems[7].Text = xrectype;
               // itm.SubItems[8].Text = row["recid"].ToString();
                return;
            }
            row = xdt.Rows[0];
            string[] lrow = {row["name"].ToString(), xrectype == "C" ? row["custno"].ToString() : row["patientno"].ToString(), xrectype == "C" ? "" : row["groupcode"].ToString(), row["contact"].ToString(), row["address1"].ToString(), row["balbf"].ToString(), txtGrpRef.Text, xrectype };
            ListViewItem item;
            item = new ListViewItem(lrow);
            listView1.Items.Add(item);
            return;
        }
        private void txtGrpRef_LostFocus(object sender, EventArgs e)
        {
           // string xrectype = chkPVTFamilyPatients.Checked ? "P" : "C";
            listView1.Items.Clear();
            DataTable dtcprptdef = Dataaccess.GetAnytable("", "MR", "SELECT * FROM cprptdef where reference = '" + txtGrpRef.Text + "'", false);
            
            if (dtcprptdef.Rows.Count > 0)
            {
                LoadListview1();
            }
            //else
            //    newheader = true;
            btnSubmit.Enabled = true;
            txtGrpDescription.Focus();
        }
        void LoadListview1()
        {
            if (dtcprptdef == null)
                return;
            foreach (DataRow drow in dtcprptdef.Rows)
            {
                if (drow["rectype"].ToString() != pat_type || txtGrpRef.Text == "SINGLE" && drow["grouphead"].ToString().Trim() != txtgrouphead.Text.Trim() )
                    continue;

                string[] row = { drow["name"].ToString(), drow["grouphead"].ToString(), drow["groupcode"].ToString(), drow["contact"].ToString(), drow["address1"].ToString(), drow["balbf"].ToString(), txtGrpRef.Text, pat_type, drow["recid"].ToString() }; /* , drow["headertext"].ToString(), drow["footertext"].ToString(), drow["header"].ToString(), drow["adjustment"].ToString(), drow["balance"].ToString(), drow["bftext"].ToString(), drow["pymtext"].ToString(), drow["adjusttext"].ToString(), drow["cur_billtext"].ToString(), drow["baltext"].ToString(), drow["rpthdtext"].ToString() };*/

                ListViewItem itm;
                itm = new ListViewItem(row);
                listView1.Items.Add(itm);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void chkCorporateClients_Click(object sender, EventArgs e)
        {
            RadioButton rbn = sender as RadioButton;

            if (rbn.Checked && (rbn.Name == "chkCorporateClients" || rbn.Name == "chkHMONHISCapitation" ))
            {
                pat_type = "C";
                txtgroupcode.Enabled = txtpatientno.Enabled = btngroupcode.Enabled = btngrouphead.Enabled = false;
                txtgrouphead.Enabled = btnCorporate.Enabled = true;
            }
            else if (rbn.Name == "chkPVTFamilyPatients" && rbn.Checked )
            {
                pat_type = "P";
                txtgrouphead.Enabled = btnCorporate.Enabled = false;
                txtgroupcode.Enabled = txtpatientno.Enabled = btngroupcode.Enabled = btngrouphead.Enabled = true;
            }
            else if (rbn.Name == "chkInclude" && chkInclude.Checked)
            {
                lblinclude.Text = "Sort Bills - Refs. to Include";
                this.toolTip1.SetToolTip(this.txtSortBills, "Enter References of Bills To Include in the Report");
                chkInclusivetoAnotherMonth.Enabled = true;
            }
            else if (rbn.Name == "chkExclude" && chkInclude.Checked)
            {
                lblinclude.Text = "Sort Bills - Refs. to Exclude";
                this.toolTip1.SetToolTip(this.txtSortBills, "Enter References of Bills To Exclude in the Report");
                chkInclusivetoAnotherMonth.Enabled = false;
            }
        }
        private void txtSortBills_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSortBills.Text))
                return;
            DialogResult result;
            DataTable xdt = Dataaccess.GetAnytable("", "MR", "select trans_date from billing where reference = '" + txtSortBills.Text + "'", false);
            if (xdt.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid Bill Reference...");
                txtSortBills.Text = "";
                return;
            }
            for (int i = 0; i < listView3.Items.Count; i++)
            {
                if (listView3.Items[i].ToString().Trim() == txtSortBills.Text.Trim())
                {
                    result = MessageBox.Show("This Bill Reference has been added before...");
                    txtSortBills.Text = "";
                    return;
                }
            }
            if (chkInclude.Checked && Convert.ToDateTime( xdt.Rows[0]["trans_date"]) >= dtDateFrom.Value.Date && Convert.ToDateTime( xdt.Rows[0]["trans_date"]) <= dtDateto.Value.Date)
            {
                result = MessageBox.Show("Date of BIll Selected 'INCLUSIVELY' cannot be in the same Period Specified...");
                txtSortBills.Text = "";
                return;
            }
            string[] row = { txtSortBills.Text, chkExclude.Checked ? "Include" : "Exclude" };
            AddToList(3, row);
        }
        void AddToList(int listnumb, string[] xrow)
        {
            ListViewItem itm;
            itm = new ListViewItem(xrow);
            if (listnumb == 3)
            {
                listView3.Items.Add(itm);
                txtSortBills.Text = "";
            }
            else
            {
                listView2.Items.Add(itm);
                txtAdjRef.Text = "";
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnClearList2")
            {
                if (listView2.Items.Count > 0)
                    listView2.Items.Clear();
            }
            else if (btn.Name == "btnClearList3" )
            {
                if (listView3.Items.Count > 0)
                    listView3.Items.Clear();
            }
        }
        private void txtAdjRef_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSortBills.Text))
                return;
            DialogResult result;
            DataTable xdt = Dataaccess.GetAnytable("", "MR", "select trans_date from bill_adj where reference = '" + txtAdjRef.Text + "'", false);
            if (xdt.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid Adjust Reference...");
                txtAdjRef.Text = "";
                return;
            }
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                if (listView2.Items[i].ToString().Trim() == txtAdjRef.Text.Trim())
                {
                    result = MessageBox.Show("This Adjust Reference has been added before...");
                    txtAdjRef.Text = "";
                    return;
                }
            }
            if (chkInclude.Checked && Convert.ToDateTime(xdt.Rows[0]["trans_date"]) >= dtDateFrom.Value.Date && Convert.ToDateTime(xdt.Rows[0]["trans_date"]) <= dtDateto.Value.Date)
            {
                result = MessageBox.Show("Date of Adjust Record Selected 'INCLUSIVELY' cannot be in the same Period Specified...");
                txtSortBills.Text = "";
                return;
            }
            string[] row = { txtAdjRef.Text, chkExclude.Checked ? "Include" : "Exclude" };
            AddToList(2, row);
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count < 1)
                return;
            DialogResult result = MessageBox.Show("Confirm to Save Current selections...", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            btnSubmit.Enabled = false;
            SaveCurrentSelections();
            MessageBox.Show("Completed...", "Save Selections in List");
        }
        void SaveCurrentSelections()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();

            connection.Open();
            int xrecid = 0;
            string updatestring = "update cprptdef set name = @name, grouphead = @grouphead, groupcode = @groupcode, contact = @contact, address1 = @address1 where RECID = '";
            string selstring = "";
            foreach (ListViewItem lv in listView1.Items )
            {
                xrecid = string.IsNullOrWhiteSpace(lv.SubItems[8].ToString()) ? 0 : Convert.ToInt32(lv.SubItems[8].ToString());
                if (xrecid > 0)
                    selstring = updatestring + xrecid + "'";
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = xrecid < 1 ? "cprptdef_Add" : selstring;
                insertCommand.Connection = connection;
                if (xrecid < 1)
                    insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@NAME", lv.SubItems[0].ToString());
                insertCommand.Parameters.AddWithValue("@GROUPHEAD", lv.SubItems[1].ToString());
                insertCommand.Parameters.AddWithValue("@GROUPCODE", lv.SubItems[2].ToString());
                insertCommand.Parameters.AddWithValue("@CONTACT", lv.SubItems[3].ToString());
                insertCommand.Parameters.AddWithValue("@ADDRESS1", lv.SubItems[4].ToString());
                if (xrecid < 1)
                {
                    insertCommand.Parameters.AddWithValue("@REFERENCE", txtGrpRef.Text);
                    insertCommand.Parameters.AddWithValue("@cur_bills", 0m);
                    insertCommand.Parameters.AddWithValue("@balbf", 0m);
                    insertCommand.Parameters.AddWithValue("@payments", 0m);
                    insertCommand.Parameters.AddWithValue("@db_notes", 0m);
                    insertCommand.Parameters.AddWithValue("@cr_notes", 0m);
                    insertCommand.Parameters.AddWithValue("@headertext", "");
                    insertCommand.Parameters.AddWithValue("@footertext", "");
                    insertCommand.Parameters.AddWithValue("@header", false); 
                    insertCommand.Parameters.AddWithValue("@adjustment", 0m);
                    insertCommand.Parameters.AddWithValue("@balance", 0m);
                    insertCommand.Parameters.AddWithValue("@bftext", "");
                    insertCommand.Parameters.AddWithValue("@pymtext", "");
                    insertCommand.Parameters.AddWithValue("@adjusttext", "");
                    insertCommand.Parameters.AddWithValue("@cur_billtext", "");
                    insertCommand.Parameters.AddWithValue("@baltext", "");
                    insertCommand.Parameters.AddWithValue("@rpthdtext", "");
                    insertCommand.Parameters.AddWithValue("@rectype", pat_type);
                    insertCommand.Parameters.AddWithValue("@description", txtGrpDescription.Text);
                }
               // if (xrecid > 0)
                //    insertCommand.Parameters.AddWithValue("@recid", Convert.ToInt32(lv.SubItems[8].ToString()));

                insertCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
        private void chkIncludeBillings_Click(object sender, EventArgs e)
        {
            if (!chkIncludeBillings.Checked)
                return;
            DialogResult result;
            if (string.IsNullOrWhiteSpace(txtGrpRef.Text))
            {
                result = MessageBox.Show("No Group Reference Specified... Confirm to AUTO Generate Report List \r\n from Customer Databases... Note: This will generate ONLY for Clients on Monthly Billing Circle ","End of Month Statement/Letter", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
                string xfile = pat_type == "P" ? "patient" : "customer";
                string rtnstring = "",selstring = "";
                if (chkHMONHISCapitation.Checked && pat_type == "C")
                    rtnstring += "customer.hmo = '1'";
                if (pat_type == "P")
                    selstring = "SELECT NAME, PATIENTNO, GROUPCODE, CONTACT, ADDRESS1, BALBF FROM patient where bill_cir = 'M'";
                else
                    selstring = "SELECT NAME, CUSTNO, CONTACT, ADDRESS1, BALBF FROM customer where bill_cir = 'M'";
                DataTable xdt = Dataaccess.GetAnytable("", "MR", selstring, false);
                //LoadListview1();
                foreach (DataRow row in xdt.Rows)
                {
                    string[] lrow = { row["name"].ToString(), pat_type == "C" ? row["custno"].ToString() : row["patientno"].ToString(), pat_type == "C" ? "" : row["groupcode"].ToString(), row["contact"].ToString(), row["address1"].ToString(), row["balbf"].ToString(), txtGrpRef.Text, pat_type };
                    ListViewItem item;
                    item = new ListViewItem(lrow);
                    listView1.Items.Add(item);
                }
            }
        }
        void LoadBills()
        {
            DialogResult result = MessageBox.Show("Confirm to Load Bills for Defined Specification... ", "End of Month Statement/Letter", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            string selstring, tmpstring; selstring = tmpstring = "";
            string bstr = "", pstr = "", astr = "", includestring = "";
            if (chkCorporateClients.Checked)
            {
                bstr = " AND BILLING.TRANSTYPE = 'C'";
                pstr = " AND PAYDETAIL.TRANSTYPE = 'C'";
                astr = " AND BILL_ADJ.TRANSTYPE = 'C'";
            }
            if (chkPVTFamilyPatients.Checked)
            {
                bstr += " AND BILLING.TRANSTYPE = 'P'";
                pstr += " AND PAYDETAIL.TRANSTYPE = 'P'";
                astr += " AND BILL_ADJ.TRANSTYPE = 'P'";
            }
            decimal balance = 0m;
            decimal db, cr, adj; db = cr = adj = 0m;
            string xgrpstring = "",xinvref = "";
            DataTable tsdt;
            DataRow dtlrow;
            if (chkIncludeBillings.Checked)
            {
                tmpstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.ITEMNO, BILLING.DESCRIPTION, BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.GROUPHEAD AS CUSTOMER, BILLING.GROUPCODE,  BILLING.EXTDESC, BILLING.ACCOUNTTYPE " + includestring + " FROM BILLING WHERE BILLING.TRANS_DATE >= '" + dtDateFrom.Value.Date + "' AND BILLING.TRANS_DATE <= '" + dtDateto.Value.Date + "' " + bstr+" and accounttype NOT LIKE '[NHR]'";

                foreach (DataRow row in dtcprptdef.Rows)
                {
                    xgrpstring = pat_type == "P" ? " and groupcode = '" + row["GHGROUPCODE"].ToString() + "' and grouphead = '" + row["grouphead"].ToString() + "'" : " and grouphead = '" + row["grouphead"].ToString() + "'";
                    selstring = tmpstring + xgrpstring;
                    if (chkIncludeGroupInvNo.Checked) //Group invoice number in report
                       xinvref = msmrfunc.GetGroupInvno(row["groupcode"].ToString(), row["grouphead"].ToString(), dtDateFrom.Value.Date);

                    //remove bills for clients not on listview
                    tsdt = Dataaccess.GetAnytable("", "MR", selstring, false);
                    foreach (DataRow brow in tsdt.Rows)
                    {
                        if (listView3.Items.Count > 0 && listcheck(listView3, brow))
                        {
                            brow.Delete();
                            continue;
                        }

                        dtlrow = createnewRow(brow, false);
                        dtlrow["CONTACT"] = row["contact"].ToString();
                        dtlrow["ADDRESS"] = row["address1"].ToString();
                        dtlrow["ghNAME"] = row["name"].ToString();
                        if (chkIncludeGroupInvNo.Checked) //Group invoice number in report
                            row["invref"] = xinvref;
                        if (chkPrintService.Checked && !string.IsNullOrWhiteSpace(brow["extdesc"].ToString()))
                        {
                            dtlrow = createnewRow(brow, true);
                            dtlrow["CONTACT"] = row["contact"].ToString();
                            dtlrow["ADDRESS"] = row["address1"].ToString();
                            dtlrow["ghNAME"] = row["name"].ToString();
                            if (chkIncludeGroupInvNo.Checked) //Group invoice number in report
                                row["invref"] = xinvref;
                        }
                    }
                }
                //check for inclusion from other billing period
                foreach (ListViewItem lvi in listView3.Items)
                {
                    if (lvi.SubItems[1].ToString().Trim() == "Exclude")
                       continue;
                    tmpstring = "SELECT BILLING.REFERENCE, BILLING.PATIENTNO, BILLING.NAME, BILLING.DESCRIPTION, BILLING.AMOUNT, BILLING.TRANS_DATE, BILLING.GROUPHEAD AS CUSTOMER, BILLING.GROUPCODE, BILLING.EXTDESC, BILLING.ACCOUNTTYPE " + includestring + " FROM BILLING WHERE rtrim(BILLING.REFERENCE) = '" + lvi.SubItems[0].ToString().Trim() + "' "; //ADD TO sdt TABLE HERE

                    tsdt = Dataaccess.GetAnytable("", "MR", tmpstring, false);
                    foreach (DataRow brow in tsdt.Rows)
                    {
                        dtlrow = createnewRow(brow, false);
                        if (chkPrintService.Checked)
                        {
                            dtlrow = createnewRow(brow, true);
                        }
                    }
                }
            }
            else
            {
                decimal payment, debits, credits, adjs;
                foreach (DataRow row in dtcprptdef.Rows)
                {
                    payment = debits = credits = adjs = 0m;
                    balance = msmrfunc.getOpeningBalance(row["GROUPCODE"].ToString(), row["grouphead"].ToString(), "", pat_type, dtDateFrom.Value.Date, dtDateto.Value.Date,ref db,ref cr,ref adj );
                    decimal xb = msmrfunc.getTransactionDbCrAdjSummary(row["GROUPCODE"].ToString(), row["grouphead"].ToString(), "", pat_type, dtDateFrom.Value.Date, dtDateto.Value.Date, ref debits, ref credits, ref adjs);
                    if (balance > 0 || debits > 0 || credits > 0)
                    {
                        //dtlrow = createnewRow(row);
                        row["cur_bills"] = debits;
                        row["balbf"] = balance;
                        row["payments"] = credits;
                        row["db_notes"] = adjs < 1 ? 0m : adjs;
                        row["cr_notes"] = adjs < 1 ? adjs : 0m;
                        row["adjustment"] = adjs;
                        row["balance"] = ((debits + (decimal)row["db_notes"] + balance) - (credits + (decimal)row["cr_notes"])); // ((debits + (decimal)row["db_notes"] + balance >= 1 ? balance : 0m) - (credits + (decimal)row["cr_notes"] + balance < 1 ? balance : 0m));
                        //((cur_bills+db_notes+iif(balbf>=1,balbf,0))-(payments+cr_notes+iif(balbf < 1,balbf,0)))
                    }
                    //how do we get remote data ? 16.07.2017 for capitolhill
                }
            }
         /*   if (chkIncludeGroupInvNo.Checked) //Group invoice number in report - Lookup() not working
            {
                foreach (DataRow row in dtcprptdef.Rows)
                {
                    row["reference"] = msmrfunc.GetGroupInvno(row["groupcode"].ToString(), row["grouphead"].ToString(), dtDateFrom.Value.Date);
                }
            }*/
        }
        bool listcheck(ListView lv, DataRow xrow)
        {
            bool foundit = false;
            foreach (ListViewItem lvi in lv.Items )
            {
                if (lvi.SubItems[0].ToString().Trim() == xrow["reference"].ToString().Trim() && lvi.SubItems[0].ToString().Trim() == "Exclude")
                {
                    foundit = true;
                    break;
                }
            }
            return foundit;
        }
        void createStatementHolder()
        {
            sdt = new DataTable(); //table to will be passed to report dataset 
          //  sdt.Columns.Add(new DataColumn("GROUPHEAD", typeof(string)));
            sdt.Columns.Add(new DataColumn("GROUPCODE", typeof(string)));
            sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
      /*      sdt.Columns.Add(new DataColumn("CONTACT", typeof(string)));
            sdt.Columns.Add(new DataColumn("ADDRESS1", typeof(string)));
            sdt.Columns.Add(new DataColumn("headertext", typeof(string)));
            sdt.Columns.Add(new DataColumn("footertext", typeof(string)));
            sdt.Columns.Add(new DataColumn("rpthdtext", typeof(string)));
            if (chkIncludeBillings.Checked)
            {*/
                sdt.Columns.Add(new DataColumn("reference", typeof(string)));
                sdt.Columns.Add(new DataColumn("Description", typeof(string)));
                sdt.Columns.Add(new DataColumn("Amount", typeof(Decimal)));
                sdt.Columns.Add(new DataColumn("PATIENTNO", typeof(string)));
             //   sdt.Columns.Add(new DataColumn("ITEMNO", typeof(string)));
                sdt.Columns.Add(new DataColumn("TRANS_DATE", typeof(DateTime)));
                sdt.Columns.Add(new DataColumn("CUSTOMER", typeof(string)));
                sdt.Columns.Add(new DataColumn("EXTDESC", typeof(string)));
                sdt.Columns.Add(new DataColumn("accounttype", typeof(string)));
                sdt.Columns.Add(new DataColumn("GHNAME", typeof(string)));
                sdt.Columns.Add(new DataColumn("CONTACT", typeof(string)));
                sdt.Columns.Add(new DataColumn("ADDRESS", typeof(string)));
                sdt.Columns.Add(new DataColumn("INVREF", typeof(string)));
           /*     sdt.Columns.Add(new DataColumn("ttype", typeof(string)));
                sdt.Columns.Add(new DataColumn("transtype", typeof(Decimal)));
            }
            else
            {
                sdt.Columns.Add(new DataColumn("cur_bills", typeof(Decimal)));
                sdt.Columns.Add(new DataColumn("balbf", typeof(Decimal)));
                sdt.Columns.Add(new DataColumn("payments", typeof(Decimal)));
                sdt.Columns.Add(new DataColumn("db_notes", typeof(Decimal)));
                sdt.Columns.Add(new DataColumn("cr_notes", typeof(Decimal)));
                sdt.Columns.Add(new DataColumn("adjustment", typeof(string)));
                sdt.Columns.Add(new DataColumn("balance", typeof(Decimal)));
                sdt.Columns.Add(new DataColumn("bftext", typeof(string)));
                sdt.Columns.Add(new DataColumn("pymtext", typeof(string)));
                sdt.Columns.Add(new DataColumn("adjusttext", typeof(string)));
                sdt.Columns.Add(new DataColumn("cur_billtext", typeof(string)));
                sdt.Columns.Add(new DataColumn("baltext", typeof(string)));
                sdt.Columns.Add(new DataColumn("rectype", typeof(string)));
        //    }*/
            
        }
        DataRow createnewRow(DataRow drow, bool extended)
        {
            DataRow dr;

            dr = sdt.NewRow();
         //   dr["GROUPHEAD"] = drow["grouphead"].ToString();
            //dr["GHGROUPCODE"] = drow["GHGROUPCODE"].ToString();
            dr["GROUPCODE"] = drow["groupcode"].ToString();
            dr["NAME"] = drow["name"].ToString();
/*            if (chkIncludeBillings.Checked)
            {*/
                dr["reference"] = drow["reference"].ToString();
                if (extended)
                {
                    dr["Description"] = drow["extdesc"].ToString();
                    dr["Amount"] = 0m;
                }
                else
                {
                    dr["Description"] = drow["description"].ToString();
                    dr["Amount"] = (decimal)drow["amount"];
                }
                dr["PATIENTNO"] = drow["patientno"].ToString();
                dr["TRANS_DATE"] = (DateTime)drow["trans_date"];
                dr["CUSTOMER"] = drow["customer"].ToString();
                dr["EXTDESC"] = ""; // drow["extdesc"].ToString();
                dr["accounttype"] = drow["accounttype"].ToString();
                if (chkSeparateOPD.Checked) //Separate OPD/In-Patient Bills
                {
                    if (dr["reference"].ToString().Substring(0, 1) == "A" &&
                        string.IsNullOrWhiteSpace(dr["accounttype"].ToString()))
                        dr["accounttype"] = "P";
                    else if (string.IsNullOrWhiteSpace(dr["accounttype"].ToString()))
                        dr["accounttype"] = "O";
                    if (dr["accounttype"].ToString().Substring(0, 1) == "I")
                        dr["accounttype"] = "P";
                }

       /*     }
            dr["cur_bills"] = 0m;
            dr["balbf"] = 0m;
            dr["payments"] = 0m;
            dr["db_notes"] = 0m;
            dr["cr_notes"] = 0m;
            dr["adjustment"] = 0m;
            dr["balance"] = 0m;
            dr["bftext"] = drow["bftext"].ToString();
            dr["pymtext"] = drow["pymtext"].ToString();
            dr["adjusttext"] = drow["adjusttext"].ToString();
            dr["cur_billtext"] = drow["cur_billtext"].ToString();
            dr["baltext"] = drow["baltext"].ToString();
            dr["rectype"] = pat_type;*/

            sdt.Rows.Add(dr);
            return dr;
            //sep_opdinpbills
        }
        private void btnLoadBills_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count < 1)
            {
                DialogResult result = MessageBox.Show("Group Reference must be selected...");
                return;
            }
            if (dtcprptdef == null)
                dtcprptdef = Dataaccess.GetAnytable("","MR","select * from cprptdef where recid < 1",false);
            else
                dtcprptdef.Rows.Clear();
            if (chkIncludeBillings.Checked)
                createStatementHolder();
            //LOAD listview1 to dtcprptdef
            DataRow row = null;
            ListViewItem itm = null;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                row = dtcprptdef.NewRow();
                itm = listView1.Items[i];
                row["name"] = itm.SubItems[0].ToString();
                row["grouphead"] = itm.SubItems[1].ToString();
                row["groupcode"] = itm.SubItems[2].ToString();
                row["contact"] = itm.SubItems[3].ToString();
                row["address1"] = itm.SubItems[4].ToString();
                row["balbf"] = Convert.ToDecimal( itm.SubItems[5].ToString());
                row["reference"] = itm.SubItems[6].ToString();
                row["rectype"] = itm.SubItems[7].ToString();
                row["headertext"] = itm.SubItems[8].ToString();
                row["footertext"] = itm.SubItems[9].ToString();
                row["header"] = false;
                row["bftext"] = itm.SubItems[11].ToString();
                row["pymtext"] = itm.SubItems[12].ToString();
                row["adjusttext"] = itm.SubItems[13].ToString();
                row["cur_billtext"] = itm.SubItems[14].ToString();
                row["baltext"] = itm.SubItems[15].ToString();
                row["rpthdtext"] = itm.SubItems[16].ToString();
                row["cur_bills"] = 0m;
                row["balbf"] = 0m;
                row["payments"] = 0m;
                row["db_notes"] = 0m;
                row["cr_notes"] = 0m;
                row["adjustment"] = 0m;
                row["balance"] = 0m;
                row["description"] = txtGrpDescription.Text;

                dtcprptdef.Rows.Add(row);
            }
            LoadBills();
            if (btnSubmit.Enabled ) //&& MessageBox.Show("Confirm to Save Current Definition...") == DialogResult.Yes)
            {
                SaveCurrentSelections();
            }
           // btnCustomizeMail.PerformClick();
        }
        private void btnCustomizeMail_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (dtDateFrom.Value.Date < msmrfunc.mrGlobals.mta_start || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateto.Value.Date < dtDateFrom.Value.Date || dtDateto.Value.Date > DateTime.Now.Date )
            {
                result = MessageBox.Show("Invalid Date Specification...");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGrpRef.Text) || dtcprptdef == null )
            {
                result = MessageBox.Show("No Valid Selections made...");
                return;
            }
            lblprompt.Text = "Loading Bills...";
            btnLoadBills.PerformClick();
            lblprompt.Text = "";
            string xperiod = dtDateFrom.Value.ToString("MMMM") + ", " + dtDateFrom.Value.Year.ToString();

            frmEOMStatement_Summary withsumm = new frmEOMStatement_Summary(chkIncludeBillings.Checked ? true : false, dtcprptdef, sdt, txtGrpRef.Text, xperiod, chkSeparateOPD.Checked ? true : false );
                withsumm.Show();
        }

        private void dtDateto_Click(object sender, EventArgs e)
        {
            dtDateto.Value = dtDateFrom.Value.AddDays(30);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int xitem = listView1.SelectedIndex;
            DialogResult result = MessageBox.Show("Confirm to Delete Current selection..."+listView1.Items[xitem].SubItems[0].Text.Trim(), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            if (Convert.ToInt32(listView1.Items[xitem].SubItems[8].Text.Trim()) > 0)
            {
                string updatestr = "delete from cprptdef where recid = '" + listView1.Items[xitem].SubItems[8].ToString().Trim() + "'";
                bissclass.UpdateRecords(updatestr, "MR");
            }
            MessageBox.Show("Record Deleted...");
            listView1.Items[xitem].Remove();
        }

    }
}