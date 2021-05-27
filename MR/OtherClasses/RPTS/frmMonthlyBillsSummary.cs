
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
using MSMR;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmMonthlyBillsSummary : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, mrptheader, sysmodule = bissclass.getRptfooter();
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtdocs = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE,NAME FROM DOCTORS WHERE RECTYPE = 'D'", true),
            dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES", false);
        bool fcgroup;
        public frmMonthlyBillsSummary()
        {
            InitializeComponent();
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
            else if (btn.Name == "btnpatientno" )
            {
                txtpatientno.Text = "";
                lookupsource = "L";
                msmrfunc.mrGlobals.crequired = "L";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btngrouphead")
            {
                txtgrouphead.Text = "";
                lookupsource = "C";
                msmrfunc.mrGlobals.crequired = "C";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORP. CLIENTS";
            }
            else if (btn.Name == "btnBillReference")
            {
                txtReference.Text = "";
                lookupsource = "R";
                msmrfunc.mrGlobals.crequired = "BILL";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR RECORDED BILLS";
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
            else if (lookupsource == "C") //patientno
            {
                txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtgrouphead.Select();
            }
            else if (lookupsource == "R") 
            {
                txtReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtReference.Select();
            }

        }
        private void txtPatientno_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace( txtpatientno.Text))
                return;
            if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
            {
                txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
            }
            //check if patientno exists
            bchain = billchaindtl.Getbillchain(txtpatientno.Text, txtgroupcode.Text);
            if (bchain == null)
            {
                DialogResult result = MessageBox.Show("Invalid Patient Number... ");
                txtpatientno.Text = " ";
                txtgroupcode.Select();
                return;
            }
            txtName.Text = bchain.NAME;
            chkFamily.Checked = true;
            fcgroup = false;
            if (bchain.PATIENTNO == bchain.GROUPHEAD)
                fcgroup = true;
        }
        private void txtgrouphead_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtgrouphead.Text))
                return;
            DataTable dtcustomer = Dataaccess.GetAnytable("", "MR", "SELECT NAME FROM CUSTOMER WHERE CUSTNO = '" + txtgrouphead.Text + "'", false);
            if (dtcustomer.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("Invalid Corporate Clients Reference...");
                txtgrouphead.Text = "";
                return;
            }
            txtName.Text = dtcustomer.Rows[0]["name"].ToString();

        }
        private void txtReference_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReference.Text))
                return;
            DialogResult result;
             DataTable xdt = Dataaccess.GetAnytable("", "MR", "select name from billing where reference = '"+txtReference.Text+"'", false);
            if (xdt.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid Bill Reference...");
                txtReference.Text = "";
                return;
            }
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].ToString().Trim() == txtReference.Text.Trim())
                {
                    result = MessageBox.Show("This Bill Reference has been added before...");
                    txtReference.Text = "";
                    return;
                }
            }
            listBox1.Items.Add(txtReference.Text);
            txtReference.Text = "";
        }
        void createZenithFormat()
        {
            sdt = new DataTable(); //table to will be passed to report dataset 
            if (chkZenithFormat.Checked)
            {
                sdt.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
                sdt.Columns.Add(new DataColumn("STAFFNO", typeof(string)));
                sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
                sdt.Columns.Add(new DataColumn("BENEFICIARIES", typeof(string)));
                sdt.Columns.Add(new DataColumn("TRANS_DATE", typeof(DateTime)));
                sdt.Columns.Add(new DataColumn("AMOUNT", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("TOTALAMT", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("HOSPITAL", typeof(string)));
                sdt.Columns.Add(new DataColumn("ghgroupcode", typeof(string)));
                sdt.Columns.Add(new DataColumn("grouphead", typeof(string)));
                sdt.Columns.Add(new DataColumn("relationship", typeof(string)));
                sdt.Columns.Add(new DataColumn("patientno", typeof(string)));
                sdt.Columns.Add(new DataColumn("patientno_principal", typeof(string)));
            }
            else
            {
                sdt.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
                sdt.Columns.Add(new DataColumn("STAFFNO", typeof(string)));
                sdt.Columns.Add(new DataColumn("NAME", typeof(string)));
                sdt.Columns.Add(new DataColumn("TRANS_DATE", typeof(DateTime)));
                sdt.Columns.Add(new DataColumn("AMOUNT", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("TOTALAMT", typeof(decimal)));
                sdt.Columns.Add(new DataColumn("ghgroupcode", typeof(string)));
                sdt.Columns.Add(new DataColumn("grouphead", typeof(string)));
                sdt.Columns.Add(new DataColumn("relationship", typeof(string)));
                sdt.Columns.Add(new DataColumn("patientno", typeof(string)));
            }
            sdt.Columns.Add(new DataColumn("GHNAME", typeof(string)));
            sdt.Columns.Add(new DataColumn("CONTACT", typeof(string)));
            sdt.Columns.Add(new DataColumn("ADDRESS", typeof(string)));
            sdt.Columns.Add(new DataColumn("INVREF", typeof(string)));
            sdt.Columns.Add(new DataColumn("description", typeof(string)));
            sdt.Columns.Add(new DataColumn("RECID", typeof(decimal)));
            sdt.Columns.Add(new DataColumn("ITEMNO", typeof(decimal)));
        }
        DataRow createnewRow(DataRow drow, bool extended)
        {
            DataRow dr;

            dr = sdt.NewRow();
            dr["REFERENCE"] = drow["reference"].ToString();
            dr["STAFFNO"] = "";
            dr["NAME"] = drow["name"].ToString();
            dr["TRANS_DATE"] = (DateTime)drow["trans_date"];
            if (extended)
            {
                dr["AMOUNT"] = 0m;
                dr["description"] = drow["extdesc"].ToString();
            }
            else
            {
                dr["AMOUNT"] = (decimal)drow["amount"];
                dr["description"] = drow["description"].ToString();
            }
            dr["ghgroupcode"] = drow["ghgroupcode"].ToString();
            dr["grouphead"] = drow["grouphead"].ToString();
            dr["patientno"] = drow["patientno"].ToString();
            if (chkZenithFormat.Checked)
            {
                dr["BENEFICIARIES"] = "";
                dr["TOTALAMT"] = 0;
                dr["HOSPITAL"] = "";
                dr["relationship"] = "";
                dr["patientno_principal"] = "";
            }
            dr["recid"] = drow["recid"];
            sdt.Rows.Add(dr);
            return dr;
        }

        void getData()
        {
            bool lhistory = false;
            if (dtDatefrom.Value.Year < msmrfunc.mrGlobals.mpyear)
                lhistory = true;
            DataTable dtt,tmpsdt,dttz = new DataTable();
            string rptfile = lhistory ? "billhist" : "billing";
            string xstr = "";
            //accounttype $ "NHR" OR ttype = "C"
            string selstring = " WHERE trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999' and accounttype NOT LIKE '[NHR]' and ttype <> 'C'";
            if (!string.IsNullOrWhiteSpace(txtReference.Text))
                selstring += " AND reference = '" + txtReference.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
                selstring += " AND GROUPHEAD = '" + txtgrouphead.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
            {
                xstr = fcgroup ? " AND GHGROUPCODE = '" + txtgroupcode.Text + "'" : " AND GROUPCODE = '" + txtgroupcode.Text + "'";
                selstring += xstr; // " AND GHGROUPCODE = '" + txtgroupcode.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
            {
                xstr = fcgroup ? " AND GROUPHEAD = '" + txtpatientno.Text + "'" : " AND PATIENTNO = '" + txtpatientno.Text + "'";
                selstring += xstr;
                // selstring += " AND PATIENTNO = '" + txtpatientno.Text + "'";
                //EXTRACT ALL CLIENTS ON MONTHLY_BILL_CIRCLE to a datatable otherwise i will be required to go forth and come to sql server database for each patient record 03.09.2017

            }
            if (chkCorporate.Checked)
            {
                selstring += " AND transtype = 'C'";
                dtt = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO, contact, address1, name FROM CUSTOMER WHERE bill_cir = 'M'", false);
            }
            else if (chkFamily.Checked)
            {
                selstring += " AND transtype = 'P' AND GHGROUPCODE = 'FC'";
                dtt = Dataaccess.GetAnytable("", "MR", "SELECT groupcode+patientno AS custno, contact, address1, name FROM patient WHERE ISGROUPHEAD = '1' AND GROUPCODE = 'FC'", false);
            }
            else
            {
                selstring += " AND transtype = 'P' AND GHGROUPCODE = 'PVT'";
                dtt = Dataaccess.GetAnytable("", "MR", "SELECT groupcode+patientno AS custno, contact, address1, name FROM patient WHERE ISGROUPHEAD = '1' AND GHGROUPCODE = 'PVT'", false);
                //dtt = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO, contact, address1, name FROM CUSTOMER WHERE bill_cir = 'M' UNION SELECT groupcode+patientno AS custno, contact, address1, name FROM patient WHERE bill_cir = 'M' AND ISGROUPHEAD = '1'", false);
            }

            if (chkZenithFormat.Checked)
                dttz = Dataaccess.GetAnytable("", "MR", "SELECT groupcode, patientno, staffno, relationsh, patientno_principal, NAME from billchain where grouphead = '" + txtgrouphead.Text + "'", false);

            xstr = "select * from billing "+selstring+" ORDER BY GROUPHEAD";
            tmpsdt = Dataaccess.GetAnytable("", "MR", xstr, false);

            DataTable GRPREF = Dataaccess.GetAnytable("", "MR", "select count(reference) as refcount, reference from billing " + selstring + " GROUP by reference", false);
            DataRow dtlrow;
            string principal = "",xval = "";
            bool foundit = false;
            string xgrphead, xinvref, grpheadname, address, contact,name; xgrphead = xinvref = grpheadname = address = contact = name = "";
            string refsave = "";
            decimal xc = 1;
            foreach (DataRow row in tmpsdt.Rows)
            {
                //check to ensure that corporate/pvt/family account is monthly
                xval = row["transtype"].ToString() == "C" ? row["grouphead"].ToString() : row["ghgroupcode"].ToString() + row["grouphead"].ToString();
                if (refsave != row["reference"].ToString().Trim())
                {
                    xc = ReferenceCheck(row["reference"].ToString().Trim(), GRPREF);
                    refsave = row["reference"].ToString().Trim();
                }
                row["recid"] = xc;
                if (xval != xgrphead)
                {
                    xgrphead = xval; // row["transtype"].ToString() == "C" ? row["grouphead"].ToString() : row["ghgroupcode"].ToString() + row["grouphead"].ToString();
                    if (!CHKECKBILL_CIRCLE(dtt, xgrphead, ref address, ref contact,ref grpheadname))
                        continue;
                    if (chkIncludeGroupInvNo.Checked) //Group invoice number in report
                        xinvref = msmrfunc.GetGroupInvno(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), dtDatefrom.Value.Date);
                }
                if (listBox1.Items.Count > 0) //check for include and exclude bills
                {
                    foundit = false;
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        if (listBox1.Items[i].ToString().Trim() == row["reference"].ToString().Trim())
                        {
                            foundit = true;
                            break;
                        }
                    }
                    if (chkInclude.Checked && !foundit || chkExclude.Checked && foundit)
                        continue;
                }
                dtlrow = createnewRow(row,false);
                dtlrow["CONTACT"] = contact;
                dtlrow["ADDRESS"] = address;
                dtlrow["ghNAME"] = grpheadname;
                if (chkIncludeGroupInvNo.Checked ) //Group invoice number in report
                    dtlrow["invref"] = xinvref;
                if (chkPrintService.Checked && !string.IsNullOrWhiteSpace(row["extdesc"].ToString()))
                {
                    dtlrow = createnewRow(row, true);
                    dtlrow["CONTACT"] = contact;
                    dtlrow["ADDRESS"] = address;
                    dtlrow["ghNAME"] = name;
                    if (chkIncludeGroupInvNo.Checked ) //Group invoice number in report
                        dtlrow["invref"] = xinvref;
                }
                if (chkZenithFormat.Checked)
                {
                    bool itsfound = false; principal = "";
                    foreach (DataRow zdtl in dttz.Rows)
                    {
                        if (zdtl["groupcode"].ToString() == row["groupcode"].ToString() && zdtl["patientno"].ToString() == row["patientno"].ToString())
                        {
                            itsfound = true;
                            dtlrow["staffno"] = zdtl["staffno"].ToString();
                            dtlrow["relationship"] = zdtl["relationsh"].ToString();
                            if (zdtl["relationsh"].ToString() != "S")
                            {
                                dtlrow["beneficiaries"] = zdtl["name"].ToString();
                                principal = zdtl["patientno_principal"].ToString();
                            }
                        }
                    }
                    if (itsfound && !string.IsNullOrWhiteSpace(principal)) //get name of principal
                    {
                        foreach (DataRow zdtl in dttz.Rows)
                        {
                            if (zdtl["patientno"].ToString() == principal )
                                dtlrow["patientno_principal"] = zdtl["name"].ToString();
                        }
                    }
                }
              //  xc++;
            }
        }
        decimal ReferenceCheck(string xref, DataTable xdt)
        {
            decimal rtnvalue = 0;
            foreach (DataRow row in xdt.Rows )
            {
                if (row["reference"].ToString().Trim() == xref)
                {
                    rtnvalue = Convert.ToDecimal(row["refcount"]);
                    break;
                }
            }
            return rtnvalue;
        }
        bool CHKECKBILL_CIRCLE(DataTable xdt, string xvalue, ref string xaddress, ref string xcontact, ref string xname)
        {
            bool foundit = false;
            foreach (DataRow row in xdt.Rows )
            {
                if (row["custno"].ToString() == xvalue )
                {
                    foundit = true;
                    xaddress = row["address1"].ToString();
                    xcontact = row["contact"].ToString();
                    xname = row["name"].ToString();
                    break;
                }
            }
            return foundit;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (dtDatefrom.Value.Date > DateTime.Now.Date || dtDatefrom.Value.Date < msmrfunc.mrGlobals.mta_start || dtDateto.Value.Date < dtDatefrom.Value.Date || dtDatefrom.Value.Year != dtDateto.Value.Year)
            {
                result = MessageBox.Show("Invalid Date Specification");
                return;
            }
            if (chkZenithFormat.Checked && string.IsNullOrWhiteSpace(txtgrouphead.Text))
            {
                result = MessageBox.Show("A Corporate Clients ID must be specified \r\n for Zenith Bill Report Format...");
                return;
            }
            sdt = new DataTable();
            ds = new DataSet();
       /*     if (sdt != null )
            {
                sdt.DataSet.Reset();
                ds.Tables.Clear();
                ds.Clear();
            }*/
            createZenithFormat();
               

            getData();
            if (sdt == null || sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data for Specified Conditions...");
                return;
            }
            ds.Tables.Add(sdt);

            Session["sql"] = "";
            if (chkZenithFormat.Checked)
                Session["rdlcfile"] = "MonthlyBillSummZenithFmt.rdlc";
            else if (chkOpdInpat.Checked)
                Session["rdlcfile"] = "MonthlyBillSummOPDinpt.rdlc";
            else
                Session["rdlcfile"] = "MonthlyBillSummOthers.rdlc";
            mrptheader = "FOR  "+dtDatefrom.Value.ToShortDateString()+"  TO :  "+dtDateto.Value.ToShortDateString();

            Session["customer"] = !string.IsNullOrWhiteSpace(txtgrouphead.Text) ? txtgrouphead.Text : !string.IsNullOrWhiteSpace(txtpatientno.Text) ? bchain.GROUPHEAD : "";
            Session["customertype"] = !string.IsNullOrWhiteSpace(txtgrouphead.Text) ? "C" : bchain != null ? bchain.GROUPHTYPE : "P";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer("MONTHLY BILLS SUMMARY REPORTS", mrptheader, "", "", "", "MONTHLYBILLSUMM", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt("MONTHLY BILLS SUMMARY REPORTS", mrptheader, "", "", "", "MONTHLYBILLSUMM", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
            }
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            printprocess(false);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            printprocess(true);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1)
                return;
            DialogResult result = MessageBox.Show("Confirm to Clear ALL selections...","", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            listBox1.Items.Clear();

        }

    }
}