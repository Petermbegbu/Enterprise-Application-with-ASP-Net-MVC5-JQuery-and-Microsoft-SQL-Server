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
    public partial class frmMedicalHistReport : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode, Anycode1, mrptheader, sysmodule = bissclass.getRptfooter(), woperator,msection;
        DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
        DataSet ds = new DataSet();
        DataTable sdt, dtdocs = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE, NAME FROM DOCTORS WHERE RECTYPE = 'D' order by name", true), dtdiag = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM DIAGNOSISCODES  order by name", true);
        int UserSecurityLevel;
        public frmMedicalHistReport(string xoperator)
        {
            InitializeComponent();
            woperator = xoperator;
        }
        private void frmMedicalHistReport_Load(object sender, EventArgs e)
        {
           // woperator = Session["operator"].ToString();
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select WSECLEVEL, section from mrstlev where operator = '" + woperator + "'", false);
            msection = ""; UserSecurityLevel = 0;
            if (dt.Rows.Count > 0)
            {
                UserSecurityLevel = (Int32)dt.Rows[0]["WSECLEVEL"];
                msection = dt.Rows[0]["section"].ToString().Substring(0, 1);
            }
            initcomboboxes();
        }
        private void initcomboboxes()
        {
            //get doc
            cboDoc.DataSource = dtdocs;//medical staff details - doctors
            cboDoc.ValueMember = "Reference";
            cboDoc.DisplayMember = "Name";
        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btngroupcode")
            {
                txtgroupcode.Text = "";
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
            else if (btn.Name == "btngrouphead")
            {
                txtgrouphead.Text = "";
                lookupsource = "C";
                msmrfunc.mrGlobals.crequired = "C";
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORP. CLIENTS";
            }
            frmselcode FrmSelCode = new frmselcode();
            FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
            FrmSelCode.ShowDialog();
        }
        void FrmSelCode_Closed(object sender, EventArgs e) // g - groupcode; L - patientno; I - daily attendance
        {
            frmselcode FrmSelcode = sender as frmselcode;
            if (lookupsource == "g") //groupcodee
            {
                txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
                txtgroupcode.Focus();
            }
            else if (lookupsource == "L") //patientno
            {
                txtgroupcode.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Text = Anycode1 = msmrfunc.mrGlobals.anycode1;
                txtpatientno.Focus();
            }
            else if (lookupsource == "C") 
            {
                txtgrouphead.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtgrouphead.Select();
            }
        }
        private void txtPatientno_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpatientno.Text))
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
            lblname.Text = bchain.NAME;
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
            lblname.Text = dtcustomer.Rows[0]["name"].ToString();
        }
        void getData()
        {
            lblPrompt.Text = "Loading Report Data....";
            bool lhistory = false;
            if ( dtDatefrom.Value.Year < msmrfunc.mrGlobals.mpyear)
                lhistory = true;

            string rptfile = lhistory ? "MedhistArchieved" : "Medhis";
            string selstring = " WHERE medhist.trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and medhist.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";

            if (!string.IsNullOrWhiteSpace(txtDescSearch.Text))
                selstring += " AND medhist.comments LIKE '%" + txtDescSearch.Text.Trim() + "%' ";
            if (!string.IsNullOrWhiteSpace(cboDoc.Text))
                selstring += " and medhist.doctor = '" + cboDoc.SelectedValue.ToString().Trim() + "' or medhist.doctor1 = '" + cboDoc.SelectedValue.ToString().Trim() + "' or medhist.doctor2 = '" + cboDoc.SelectedValue.ToString().Trim() + "' or medhist.doctor3 = '" + cboDoc.SelectedValue.ToString().Trim() + "' or medhist.doctor4 = '" + cboDoc.SelectedValue.ToString().Trim() + "' ";
            if (chkQueryTimeofDay.Checked && !string.IsNullOrWhiteSpace( txtTimeFrom.Text) && !string.IsNullOrWhiteSpace(txtTimeTo.Text))
                selstring += " and medhist.CTIME >= '" + txtTimeFrom.Text + "' and medhist.CTIME <= '" + txtTimeTo.Text + "' or medhist.DOCS1_DATETIME_TXT >= '" + txtTimeFrom.Text + "' and medhist.DOCS1_DATETIME_TXT <= '" + txtTimeTo.Text + "' or medhist.DOCS2_DATETIME_TXT >= '" + txtTimeFrom.Text + "' and medhist.DOCS2_DATETIME_TXT <= '" + txtTimeTo.Text + "' or medhist.DOCS3_DATETIME_TXT >= '" + txtTimeFrom.Text + "' and medhist.DOCS3_DATETIME_TXT <= '" + txtTimeTo.Text + "' or medhist.DOCS4_DATETIME_TXT >= '" + txtTimeFrom.Text + "' and medhist.DOCS4_DATETIME_TXT <= '" + txtTimeTo.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
                selstring += " AND billchain.GROUPHEAD = '" + txtgrouphead.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                selstring += " AND medhist.GROUPCODE = '" + txtgroupcode.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
                selstring += " AND medhist.PATIENTNO = '" + txtpatientno.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtNameSearch.Text))
                selstring += " AND medhist.NAME LIKE '%" + txtNameSearch.Text.Trim() + "%' ";
            
            if (!string.IsNullOrWhiteSpace(cboGender.Text))
                selstring += " and billchain.sex = '" + cboGender.Text.Trim() + "'";

            if (chkAlphabetical.Checked)
                selstring += " ORDER BY NAME";
            else if (chkTransDate.Checked)
                selstring += " ORDER BY TRANS_DATE";
            else
                selstring += " ORDER BY GROUPHEAD, TRANS_DATE";

            //string xstr = "select medhist.groupcode, medhist.patientno, medhist.name, medhist.ctime, medhist.comments, medhist.doctor, doctors.name AS docname, medhist.bp_stn, billchain.HMOSERVTYPE, billchain.hmocode, billchain.sex, billchain.staffno, billchain.birthdate, char(50) AS age, medhist.trans_date, dispensa.itemno, dispensa.stk_desc, dispensa.unit, dispensa.qty_pr, dispensa.qty_gv, dispensa.cost, dispensa.cdose, dispensa.cinterval, dispensa.cduration, dispensa.rx from medhist LEFT JOIN billchain on medhist.groupcode = billchain.groupcode and medhist.patientno = billchain.patientno LEFT JOIN doctors on medhist.doctor = doctors.reference LEFT JOIN dispensa on medhist.groupcode = dispensa.groupcode and medhist.patientno = dispensa.patientno and medhist.trans_date = dispensa.trans_date " + selstring;
            string xstr = "select medhist.groupcode, medhist.patientno, medhist.name, medhist.ctime, medhist.comments, medhist.doctor, medhist.billref, medhist.billed, medhist.amount, char(50) AS docname, medhist.bp_stn, billchain.HMOSERVTYPE, billchain.hmocode, billchain.sex, billchain.staffno, billchain.grouphead, billchain.birthdate, billchain.PHONE, char(50) AS age, medhist.trans_date, medhist.enchrypted, medhist.protected from medhist LEFT JOIN billchain on medhist.groupcode = billchain.groupcode and medhist.patientno = billchain.patientno" + selstring; //LEFT JOIN doctors on medhist.doctor = doctors.reference 
            sdt = Dataaccess.GetAnytable("", "MR", xstr, false);
            string xdrugs = "", xr, tmpstring = "";
            DataTable dtdrugs;
            string xstrd = "select dispensa.itemno, dispensa.stk_desc, dispensa.unit, dispensa.qty_pr, dispensa.qty_gv, dispensa.cost, dispensa.cdose, dispensa.cinterval, dispensa.cduration, dose, interval, duration, dispensa.rx, Dispensa.sp_inst from dispensa ";
         /*   if (!string.IsNullOrWhiteSpace(txtgrouphead.Text) )
            {
                for (int i = 0; i < sdt.Rows.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(txtgrouphead.Text) && sdt.Rows[i]["grouphead"].ToString().Trim() != txtgrouphead.Text.Trim())
                        sdt.Rows[i].Delete();
                }
                DataRow[] rows;
                rows = sdt.Select("grouphead != '"+txtgrouphead.Text+"'");
                foreach (DataRow row in rows )
                {
                    sdt.Rows.Remove(row);
                }

            }*/

            foreach (DataRow row in sdt.Rows )
            {
                if (Convert.ToDecimal(row["protected"]) > 0 && (msection == "4" && UserSecurityLevel < Convert.ToDecimal(row["protected"]) || msection != "4"))
                {
                    string xsr = Convert.ToBoolean(row["enchrypted"]) ? "and ENCHRYPTED " : "";
                    row["comments"] = " *** PROTECTED " + xsr + "PATIENT MEDICAL HISTORY ***" + "\r\n\r\n";
                    continue;
                }
                if (nmrAgefrom.Value > 0 && nmrAgeto.Value >= nmrAgefrom.Value)
                {
                    int xyear = DateTime.Now.Year - Convert.ToDateTime(row["birthdate"]).Year;
                    if (Math.Truncate(nmrAgefrom.Value) > 0 || Math.Truncate(nmrAgeto.Value) > 0)
                    {
                        //for Paediatrics, we need to get actual age
                        string paedAge = bissclass.agecalc(Convert.ToDateTime(row["birthdate"]), DateTime.Now.Date);
                        //if (xdays < 30) xrtn = Convert.ToInt32(xdays).ToString() + "d";
                        //xrtn = xY.ToString() + "y/" + Convert.ToInt32(xM).ToString() + "m/" + Convert.ToInt32(xD).ToString() + "d";
                    }
                    if (nmrAgefrom.Value < xyear || nmrAgeto.Value > xyear)
                        continue;

                }
                if (!string.IsNullOrWhiteSpace(row["doctor"].ToString()))
                    row["docname"] = bissclass.combodisplayitemCodeName("reference", row["doctor"].ToString(), dtdocs, "name");
                else
                    row["docname"] = "";
                xdrugs = "";
                if (!string.IsNullOrWhiteSpace( row["birthdate"].ToString()))
                    row["age"] = bissclass.agecalc((DateTime)row["birthdate"], DateTime.Now.Date);
                dtdrugs = Dataaccess.GetAnytable("","MR",xstrd+"where groupcode = '"+row["groupcode"].ToString()+"' and patientno = '"+row["patientno"].ToString()+"' and trans_date = '"+(DateTime)row["trans_date"]+"' order by itemno",false);
                if (dtdrugs.Rows.Count > 0)
                {
                    xdrugs = string.Format("S/N", 3) + " " + string.Format("DRUG DESCRIPTION / Unit", 35) + "   " + string.Format("Qty_Pr", 6) + "   " + string.Format("Qty_Gv", 6) + "   " + string.Format("Cost", 10) + "   " + string.Format("D / I /D", 20) + "\r\n";
                    foreach (DataRow drow in dtdrugs.Rows )
                	{
                        xr = string.IsNullOrWhiteSpace( drow["rx"].ToString()) ? "" : "\r\n"+ string.Format( drow["rx"].ToString());
                        /*xdrugs += string.Format(drow["itemno"].ToString(), 3) + " " + string.Format(drow["stk_desc"].ToString(), 30) + " " + string.Format(drow["unit"].ToString(), 5) + " " + string.Format(drow["qty_pr"].ToString(), 7) + " " + string.Format(drow["qty_gv"].ToString(), 7) + " " + string.Format(drow["cost"].ToString(), 10) + " " + string.Format(drow["cdose"].ToString(), 10) + " " + string.Format(drow["cinterval"].ToString(), 20) + " " + string.Format(drow["cduration"].ToString(), 10) + "   "+drow["rx"].ToString() + "\r\n";
                        if (!string.IsNullOrWhiteSpace(drow["sp_inst"].ToString()))
                            xdrugs += string.Format(drow["sp_inst"].ToString())+"\r\n";*/
                        tmpstring = drow["cdose"].ToString() == "" ? drow["dose"].ToString() + "x" + drow["interval"].ToString() + 'x' + drow["duration"].ToString() : drow["cdose"].ToString() + "x" + drow["cinterval"].ToString() + 'x' + drow["cduration"].ToString();
                        xdrugs += drow["itemno"].ToString() + ".... " + drow["stk_desc"].ToString() + "..... " + drow["unit"].ToString() + ".... " + drow["qty_pr"].ToString() + "......  " + drow["qty_gv"].ToString() + ".......... " + drow["cost"].ToString() + " " + tmpstring + "  (" + drow["cost"].ToString() + ")" + "\r\n";
                        if (!string.IsNullOrWhiteSpace(drow["sp_inst"].ToString()))
                            xdrugs += string.Format(drow["sp_inst"].ToString()) + "\r\n";
                    }
                    row["comments"] = row["comments"].ToString().Trim() + "\r\n"+xdrugs;
                }
            }
            ds.Tables.Add(sdt);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (dtDatefrom.Value.Date > DateTime.Now.Date || dtDateto.Value.Date < dtDatefrom.Value.Date )
            {
                result = MessageBox.Show("Invalid Date Specification");
                return;
            }
            if (dtDateto.Value.Subtract(dtDatefrom.Value).Days > 31 && (string.IsNullOrWhiteSpace(txtpatientno.Text) && string.IsNullOrWhiteSpace(txtgroupcode.Text)) && string.IsNullOrWhiteSpace(txtNameSearch.Text) && string.IsNullOrWhiteSpace(txtDescSearch.Text)  )
            {
                 result = MessageBox.Show("Invalid Date Specification, Global Medical History Report must be within one month");
                 return;
            }

            sdt = new DataTable();
            ds = new DataSet();
            //if (sdt != null)
            //{
            //    sdt.Rows.Clear();
            //    ds.Tables.Clear();
            //    ds.Clear();
            //}

            getData();
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data for Specified Conditions...");
                return;
            }
            Session["sql"] = "";
            Session["rdlcfile"] = "MedicalHistReport.rdlc";

            mrptheader += "MEDICAL HISTORY REPORT FOR " + dtDatefrom.Value.ToShortDateString() + " TO : " + dtDateto.Value.ToShortDateString();
            //rptfooter, rptcriteria
            lblPrompt.Text = "";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, "", "", "", "MEDHIST", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", woperator);

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(this.Text, mrptheader, "", "", "", "MEDHIST", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", woperator);
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
 
    }
}