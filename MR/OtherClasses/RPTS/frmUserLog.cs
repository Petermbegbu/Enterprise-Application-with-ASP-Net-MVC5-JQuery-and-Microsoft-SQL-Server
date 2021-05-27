#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using msfunc;
using mradmin.BissClass;


using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmUserLog : Form
    {
        string lookupsource, AnyCode;
        public frmUserLog()
        {
            InitializeComponent();
        }

        private void frmUserLog_Load(object sender, EventArgs e)
        {

        }
        private void btngroupcode_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btngroupcode")
            {
                this.txtgroupcode.Text = "";
                lookupsource = "g";
                msmrfunc.mrGlobals.crequired = "g";
                msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btnPatientno")
            {
                txtpatientno.Text = "";
                lookupsource = "L";
                msmrfunc.mrGlobals.crequired = "L";
                msmrfunc.mrGlobals.lookupCriteria = txtgroupcode.Text;
                msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED PATIENTS";
            }
            else if (btn.Name == "btnReference")
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
            else if (lookupsource == "R") //reference
            {
                txtReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtReference.Select();
            }

        }
        private void txtgroupcode_LostFocus(object sender, EventArgs e)
        {
            txtpatientno.Focus();
        }
        private void txtPatientno_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpatientno.Text))
                return;
            if (string.IsNullOrWhiteSpace(AnyCode) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
            {
                txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
            }
            DialogResult result;
            //check if patientno exists
            DataTable dt = Dataaccess.GetAnytable("", "MR", "select name, operator, dtime from billchain where groupcode = '" + txtgroupcode.Text + "' and patientno = '" + txtpatientno.Text + "'", false);
            if (dt.Rows.Count < 1)
            {
                result = MessageBox.Show("Invalid Patient Number... ");
                txtpatientno.Text = " ";
                txtgroupcode.Select();
                return;
            }
            lblName.Name = dt.Rows[0]["name"].ToString();
            dtDateFrom.Focus();
            return;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (!bissclass.IsPresent(cboTransaction, "Transaction File...", false))
                return;
            if (string.IsNullOrWhiteSpace(txtDescSearch.Text) && string.IsNullOrWhiteSpace(txtgroupcode.Text) && string.IsNullOrWhiteSpace(txtpatientno.Text) && string.IsNullOrWhiteSpace(txtReference.Text) && !chkByTransdate.Checked)
            {
                MessageBox.Show("Query Criteria must be specified...","Audit Trail");
                return;
            }
            string xstr = "", selstr = "";
            string xoption = cboTransaction.Text.Substring(0, 5).ToUpper();
            xstr = " where operator != ''";
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
                xstr += " and groupcode = '" + txtgroupcode.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
                xstr += " and patientno = '" + txtpatientno.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtReference.Text))
                xstr += " and reference = '" + txtReference.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtDescSearch.Text))
            {
                if (cboTransaction.Text == "Admissions Service Update")
                    xstr += " and DESCRIPTION LIKE '%" + txtDescSearch.Text.Trim() + "%' ";
                else
                    xstr += " and NAME LIKE '%" + txtDescSearch.Text.Trim() + "%' ";
            }
            if (!string.IsNullOrWhiteSpace(txtReference.Text))
                xstr += " and reference = '" + txtReference.Text + "'";
         /*   if (chkByTransdate.Checked)
                xstr += " and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";*/
            if (xoption == "PATIE") //REGISTRATION
            {
                if (chkByTransdate.Checked)
                    xstr += " and convert(date, dtime) >= '" + dtDateFrom.Value.ToShortDateString() + "' and convert(date, dtime) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from billchain" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select groupcode, patientno, name, reg_date, operator, dtime from billchain" + xstr;
            }
            else if (xoption == "DAILY")
            {
                if (chkByTransdate.Checked)
                    xstr += " and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from mrattend" + xstr+" GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, trans_date, operator, dtime from mrattend" + xstr;

            }
            else if (xoption == "BILLI" || xoption == "PAYME" || xoption == "BILLS") //BILLS/PMT/ADJMT
            {
                string xfile = xoption == "BILLI" ? "billing" : xoption == "PAYME" ? "Paydetail" : "bill_adj";
                if (chkByTransdate.Checked)
                    xstr += " and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from " + xfile + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, DESCRIPTION, trans_date, amount, operator, OP_TIME from " + xfile + xstr+" order by reference, name";
            }
            else if (xoption == "TARIF")
            {
                if (chkByTransdate.Checked)
                    xstr += " and CONVERT(date, dtime) >= '" + dtDateFrom.Value.ToShortDateString() + "' and CONVERT(date, dtime) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from TARIFF" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, trans_date, operator, dtime from TARIFF" + xstr;

            }
            else if (xoption == "CORPO")
            {
                if (chkByTransdate.Checked)
                    xstr += " and convert(date, dtime) >= '" + dtDateFrom.Value.ToShortDateString() + "' and convert(date, dtime) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from CUSTOMER" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, DATE_REG, operator, dtime from CUSTOMER" + xstr;

            }
            else if (xoption == "LAB/X")
            {
                if (chkByTransdate.Checked)
                    xstr += " and TRANS_DATE >= '" + dtDateFrom.Value.ToShortDateString() + "' and TRANS_DATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from LABDET" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, TRANS_DATE, operator, dtime from LABDET" + xstr;

            }
            else if (xoption == "BIRTH")
            {
                if (chkByTransdate.Checked)
                    xstr += " and BIRTHDATE >= '" + dtDateFrom.Value.ToShortDateString() + "' and BIRTHDATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from BIRTHS" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, BIRTHDATE, operator, dtime from BIRTHS" + xstr;

            }
            else if (xoption == "DEATH")
            {
                if (chkByTransdate.Checked)
                    xstr += " and DEATHDATE >= '" + dtDateFrom.Value.ToShortDateString() + "' and DEATHDATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from DEATH" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, DEATHDATE, operator, dtime from DEATH" + xstr;

            }
            else if (xoption == "IMMUN")
            {
                if (chkByTransdate.Checked)
                    xstr += " and CONVERT(DATE, DATETAKEN) >= '" + dtDateFrom.Value.ToShortDateString() + "' and CONVERT(DATE, DATETAKEN) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from IMUNES" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, DATETAKEN, operator, dtime from IMUNES" + xstr;

            }
            else if (cboTransaction.Text == "Admissions Registration")
            {
                if (chkByTransdate.Checked)
                    xstr += " and ADM_DATE >= '" + dtDateFrom.Value.ToShortDateString() + "' and ADM_DATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from admrecs" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, ADM_DATE, operator, dtime from admrecs" + xstr;

            }
            else if (cboTransaction.Text == "Admissions Service Update")
            {
                if (chkByTransdate.Checked)
                    xstr += " and convert(date, op_time) >= '" + dtDateFrom.Value.ToShortDateString() + "' and convert(date, op_time) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from admdetai" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, description, TRANS_DATE, operator, op_time from admdetai" + xstr;

            }
            else if (cboTransaction.Text == "Admissions Discharge Record")
            {
                if (chkByTransdate.Checked)
                    xstr += " and DISCHARGE != '' and (convert(date, discharge) >= '" + dtDateFrom.Value.ToShortDateString() + "' and convert(date, discharge) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999')";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from admrecs" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, discharge, DISCH_DOCT, operator, dtime from admrecs" + xstr;

            }
            else if (xoption == "PHARM")
            {
                if (chkByTransdate.Checked)
                    xstr += " and convert(date, op_time) >= '" + dtDateFrom.Value.ToShortDateString() + "' and convert(date, op_time) <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999')";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from dispensa" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, trans_date, operator, op_time from dispensa" + xstr;

            }
            else if (xoption == "ANTE-")
            {
                if (chkByTransdate.Checked)
                    xstr += " and REG_DATE >= '" + dtDateFrom.Value.ToShortDateString() + "' and REG_DATE <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999')";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from ANCREG" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, REG_DATE, operator, Dtime from ANCREG" + xstr;
            }
            else if (xoption == "OVERW" ) //OVERRIGHT
            {
                if (chkByTransdate.Checked)
                    xstr += " and trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
                if (chkUserProfiling.Checked)
                    selstr = "SELECT operator, COUNT(operator) AS Records from OVPROFILE" + xstr + " GROUP BY OPERATOR order by operator";
                else
                    selstr = "select reference, name, OVERWRITENOTE, amount, operator, OP_TIME from OVPROFILE" + xstr + " order by reference, name";
            }
            DataTable dt = Dataaccess.GetAnytable("", "MR", selstr, false);
            listView1.Items.Clear();
            listView1.Columns.Clear();
          //  listView1 = new ListView();
            bissclass.ListViewFill(dt, listView1);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboTransaction.Text))
                return;
            if (cboTransaction.SelectedIndex == 1)
            {
                txtReference.Text = "";
                txtReference.Enabled = false;

            }
            else
                txtReference.Enabled = true;

        }
    }
}
/*IF LEFT(ThisForm.combfile.DisplayValue,1)='6' OR ;
	LEFT(ThisForm.combfile.DisplayValue,1)='7' &&customer/tariff
	STORE "" TO ThisForm.txtgroupcode.Value,ThisForm.txtfacility.Value,ThisForm.txtreference.Value
ENDIF

DO CASE 
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='1'  &&patient reg
		SELECT name,groupcode,patientno,reg_date,;	
		operator,dtime as 'date_time' FROM msmr01!billchai ;
		HAVING reg_date >= datefrom AND reg_date <= dateto ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='2' &&daily attendance
		SELECT name,groupcode,patientno,date,time,;
		operator,dtime as 'date_time' ;
		FROM msmr01!mrattend ;
		HAVING date >= datefrom AND date <= dateto ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='3' &&billing
		SELECT name,desc,trans_date,amount,grouphead,;
		operator,op_time as 'date_time';
		FROM msmr01!billvouc ;
		HAVING trans_date >= datefrom AND trans_date <= dateto AND ttype=="D" ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='4' &&payment
		SELECT name,desc,trans_date,amount,grouphead,;
		operator,op_time as 'date_time';
		FROM msmr01!billvouc ;
		HAVING trans_date >= datefrom AND trans_date <= dateto AND ttype=="C" ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='5' &&bill adjust
		SELECT name,reference,adjust,type,amount,trans_date,operator,dtime as 'date_time';
		FROM msmr01!bill_adj ;
		HAVING trans_date >= datefrom AND trans_date <= dateto ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='6' &&tariff
		SELECT name,reference,amount,operator,dtime as 'date_time';
		FROM msmr01!tariff ;
		HAVING IIF(EMPTY(trans_date),TTOD(DTIME) >= datefrom AND TTOD(DTIME) <= dateto, ;
		trans_date >= datefrom AND trans_date <= dateto) ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='7' &&customer
		SELECT name,custno,date_reg,operator,dtime as 'date_time';
		FROM msmr01!customer ;
		HAVING date_reg >= datefrom AND date_reg <= dateto ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='8' &&lab/xray etc
		&&16-02-2013 field provided for userlog has been converted to something else
		=MESSAGEBOX("This module is under reconstruction...Pls try later",0,"")
*!*			SELECT name,reference,patientno,groupcode,trans_date,amount,;
*!*			operator,dtime as 'date_time';
*!*			FROM msmr01!labdet ;
*!*			HAVING trans_date >= datefrom AND trans_date <= dateto AND facility==mfacility ;
*!*			ORDER BY name into CURSOR userlog readwrite
		RETURN

	CASE LEFT(ThisForm.combfile.DisplayValue,1)='9'  &&births
		SELECT name,patientno,groupcode,mother,birthdate,birthtime,;
		operator,dtime as 'date_time';
		FROM msmr01!births ;
		HAVING TTOD(DTIME) >= datefrom AND TTOD(DTIME) <= dateto ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='A' &&admissions Registration
		SELECT reference,name,room,bed,rate,adm_date,;
		operator,dtime as 'date_time';
		FROM msmr01!admrecs ;
		HAVING TTOD(DTIME) >= datefrom AND TTOD(DTIME) <= dateto ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='B' &&admissions Service update
		SELECT reference,date,time,process,desc,qty,amount,operator,op_time as 'date_time';
		FROM msmr01!admdetai ;
		HAVING TTOD(op_time) >= datefrom AND TTOD(op_time) <= dateto ;
		ORDER BY reference into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='C' &&dispensary
		SELECT name,stk_desc,store,qty_pr,qty_gv,date,;
		operator,op_time as 'date_time';
		FROM msmr01!dispensa ;
		HAVING TTOD(op_time) >= datefrom AND TTOD(op_time) <= dateto ;
		ORDER BY name into CURSOR userlog readwrite
	CASE LEFT(ThisForm.combfile.DisplayValue,1)='D' &&ante-natal
		SELECT reference,name,reg_date,groupcode,patientno,;
		operator,dtime as 'date_time';
		FROM msmr01!ancreg ;
		HAVING TTOD(DTIME) >= datefrom AND TTOD(DTIME) <= dateto ;
		ORDER BY name into CURSOR userlog readwrite
*/