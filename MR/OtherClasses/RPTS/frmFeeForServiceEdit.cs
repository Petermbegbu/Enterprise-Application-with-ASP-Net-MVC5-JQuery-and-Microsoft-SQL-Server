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
    public partial class frmFeeForServiceEdit : Form
    {
        billchaindtl bchain = new billchaindtl();
        FFSC01 ffsc01 = new FFSC01();
        DataTable dtconsult;
        bool Inpatient, newrec;
        string sysmodule = bissclass.getRptfooter(), calenderText;
        DataSet ds = new DataSet();
        int recno;
        decimal itemsave;
        DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
        public frmFeeForServiceEdit(billchaindtl xbchain, DataTable consult, string reference, bool inpOutPat, DateTime datefrom, DateTime dateto)
        {
            InitializeComponent();
            bchain = xbchain;
            txtReference.Text = reference;
            Inpatient = inpOutPat;
            dtconsult = consult;
            dtTrans_date.Value = dtTreatmtDate.Value = datefrom;
            if (inpOutPat)
            {
                txtDateofAdm.Text = datefrom.ToShortDateString();
                txtDateofDischarge.Text = dateto.ToShortDateString();
            }
                
        }
        private void frmFeeForServiceEdit_Load(object sender, EventArgs e)
        {
            newrec = true;
            ffsc01 = FFSC01.GetFFSC01(txtReference.Text);
            if (ffsc01 == null)
            {
                loadPatientDetails();
                btnSave.Enabled = true;
                return;
            }
            newrec = false;
            nmrAccommodation.Value = ffsc01.ACCOMMODATION;
            nmrDaysFed.Value = ffsc01.FEED_DAYS;
            nmrFeedingAmount.Value = ffsc01.FEEDAMT;
            nmrTotalStay.Value = ffsc01.ACC_DAYS;
            nmrConsultationFee.Value = ffsc01.CONSULTATION;
            nmrLaboratoryAmt.Value = ffsc01.LABAMT;
            nmrOtherProceduresAmt.Value = ffsc01.OTHERSAMT;
            nmrRadiologicalAmt.Value = ffsc01.XRAYAMT;
            txtAge.Text = ffsc01.AGE;
            txtAuthorizationCode.Text = ffsc01.AUTHORITYCODE;
            txtCompanyName.Text = ffsc01.GRPHEADNAME;
            txtDiagnosis.Text = ffsc01.DIAGNOSIS;
            txtEnrollno.Text = ffsc01.ENROLLENO;
            txtLabDetails.Text = ffsc01.LAB;
            txtName.Text = ffsc01.PATNAME;
            txtOtherProcedures.Text = ffsc01.OTHERS;
            txtPatientno.Text = ffsc01.PATIENTNO;
            txtPlantype.Text = ffsc01.PLANTYPE;
            txtRadiological.Text = ffsc01.XRAY;
            txtType.Text = ffsc01.TRANSTYPE;
            txtDateofAdm.Text = ffsc01.ADM_DATE;
            txtDateofNotification.Text = ffsc01.NOTEDATE;
            txtDateofDischarge.Text = ffsc01.ADM_DATE;
            dtTrans_date.Value = ffsc01.TRANS_DATE;
            dtTreatmtDate.Value = ffsc01.TREATMENTDATE;
            txtPhone.Text = ffsc01.PHONE;

            DataTable dt = Dataaccess.GetAnytable("", "MR", "select * from ffsc01b where reference = '" + txtReference.Text + "'", false);
            if (dt.Rows.Count < 1)
            {
                //loadPatientDetails();
                return;
            }
            DataRow row;
            DataGridViewRow dgv;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
                dataGridView1.Rows.Add();
                dgv = dataGridView1.Rows[i];
                dgv.Cells[0].Value = i + 1;
                dgv.Cells[1].Value = row["description"].ToString();
                dgv.Cells[2].Value = (decimal)row["qty"];
                dgv.Cells[3].Value = (decimal)row["amount"];
                dgv.Cells[4].Value = (Int32)row["recid"];
            }

        }
        void loadPatientDetails()
        {
            txtPatientno.Text = bchain.PATIENTNO;
            txtCompanyName.Text = msmrfunc.GETGroupheadname(bchain.GHGROUPCODE, bchain.GROUPHEAD, bchain.GROUPHTYPE);
            txtEnrollno.Text = bchain.STAFFNO+" "+bchain.HMOCODE;
            txtName.Text = bchain.NAME;
            txtPlantype.Text = bchain.HMOSERVTYPE;
            txtAge.Text = bissclass.agecalc(bchain.BIRTHDATE, DateTime.Now.Date);
            chkCorporate.Checked = true;
            txtType.Text = Inpatient ? "IN-PATIENT" : "OPD";
            txtPhone.Text = bchain.PHONE;

            //get prescription details
            string selstring = "select ITEMNO, STK_DESC, QTY_PR, UNIT, COST, CINTERVAL, CDURATION, cdose, dose, interval, duration, RECID, rx from ";
            selstring += Inpatient ? "INPDISPENSA" : "DISPENSA";
            selstring += " where reference = '"+txtReference.Text+"' order by itemno";
            DataTable dt = Dataaccess.GetAnytable("", "MR", selstring, false);
            DataRow row;
            DataGridViewRow dgv;
            string xdesc = "",xstr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
                dataGridView1.Rows.Add();
                dgv = dataGridView1.Rows[i];
                xstr = string.IsNullOrWhiteSpace(row["cdose"].ToString()) ? row["dose"].ToString()+"x"+row["interval"].ToString()+"x"+row["duration"].ToString() : row["cdose"].ToString()+","+row["cinterval"].ToString()+","+row["cduration"].ToString()+"("+row["rx"].ToString().Trim();
                xdesc = row["stk_desc"].ToString().Trim()+" ("+xstr+")";
                dgv.Cells[0].Value = i + 1;
                dgv.Cells[1].Value = xdesc;
                dgv.Cells[2].Value = (decimal)row["qty_pr"];
                dgv.Cells[3].Value = (decimal)row["cost"];
                dgv.Cells[4].Value = "0"; // (Int32)row["recid"];
                //Dispensa.cost,qty WITH qty+Dispensa.qty_pr
            }
            getInvProcedures();
        }
        void getInvProcedures()
        {
            string xraycode = "", labcode = "", mfeeding = "";
            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT xraycode from mrsetup where recid = '1'", false);
            if (dt.Rows.Count > 0)
                xraycode = dt.Rows[0]["xraycode"].ToString().Trim();
            dt = Dataaccess.GetAnytable("", "MR", "SELECT pvtcode, regcode from mrcontrol where recid > '6' and recid < '9' ", false);
            labcode = dt.Rows[0]["pvtcode"].ToString().Trim();
            mfeeding = dt.Rows[1]["regcode"].ToString().Trim();
            string strdate = !Inpatient ? "labdet.trans_date = '" + dtTreatmtDate.Value.ToShortDateString() + "'" : "labdet.trans_date >= '" + txtDateofAdm.Text + "' and labdet.trans_date <= '" + txtDateofDischarge.Text + " 23:59:59.999'";
            string selstring = "select labdet.reference, labdet.facility, labtrans.amount, labtrans.process,  labtrans.description from labdet INNER JOIN labtrans on labdet.reference = labtrans.reference and labdet.facility = labtrans.facility WHERE labdet.groupcode = '" + bchain.GROUPCODE + "' and labdet.patientno = '" + bchain.PATIENTNO + "' and " + strdate;
            dt = Dataaccess.GetAnytable("", "MR", selstring, false);
            foreach (DataRow row in dt.Rows )
            {
                if (row["facility"].ToString().Trim() == labcode )
                {
                    nmrLaboratoryAmt.Value += (decimal)row["amount"];
                    txtLabDetails.Text = txtLabDetails.Text.Trim() + ", " + row["description"].ToString();
                }
                else if (row["facility"].ToString().Trim() == xraycode)
                {
                    nmrRadiologicalAmt.Value += (decimal)row["amount"];
                    txtRadiological.Text = txtRadiological.Text.Trim() + ", " + row["description"].ToString();
                }
                else
                {
                    nmrOtherProceduresAmt.Value += (decimal)row["amount"];
                    txtOtherProcedures.Text = txtOtherProcedures.Text.Trim() + ", " + row["description"].ToString();
                }
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            btnSave.Enabled = true;
        }
        private void txtDiagnosis_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }
        private void nmrAccommodation_ValueChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (recno > 0)
            {
                int xrow = recno - 1;
                if (dataGridView1.Rows[xrow].Cells[1].Value == null || string.IsNullOrWhiteSpace(dataGridView1.Rows[xrow].Cells[1].FormattedValue.ToString()) || Convert.ToDecimal(dataGridView1.Rows[xrow].Cells[3].FormattedValue.ToString()) == 0)
                {
                    DialogResult result = MessageBox.Show("Generated record space has not been fully utilized...", "New record Add");
                    return;
                }
            }
            DataGridViewRow row = new DataGridViewRow();
            dataGridView1.Rows.Add();
            recno++;
            return;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
                return;
            DialogResult result = MessageBox.Show("Delete Record..?", "Dispensary Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            if (Convert.ToDecimal(dataGridView1.Rows[recno].Cells[4].Value) > 0)
            {
                string updatestring = "DELETE from ffsc01b WHERE RECID = '" + dataGridView1.Rows[recno].Cells[4].Value.ToString() + "' ";

                if (bissclass.UpdateRecords(updatestring, "MR"))
                {
                    MessageBox.Show("Record Deleted...");
                }
            }
            dataGridView1.Rows.RemoveAt(recno);
            renumberview();
        }
        void renumberview()
        {
            itemsave = -1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                itemsave = Convert.ToDecimal(i);
            }
            itemsave++;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirm to Save Details..?", "Fee for Service Claims Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            SaveDetails();
        }
        void SaveDetails()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = newrec ? "ffsc01_Add" : "ffsc01_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;
            connection.Open();

            insertCommand.Parameters.AddWithValue("@PERIOD", dtTrans_date.Value.Date.ToString("MMMM") + " " + dtTrans_date.Value.Year.ToString());
            insertCommand.Parameters.AddWithValue("@REFERENCE", txtReference.Text);
            insertCommand.Parameters.AddWithValue("@GROUPCODE", bchain.GROUPCODE);
            insertCommand.Parameters.AddWithValue("@PATIENTNO", bchain.PATIENTNO);
            insertCommand.Parameters.AddWithValue("@PATNAME", txtName.Text);
            insertCommand.Parameters.AddWithValue("@GROUPHEAD", bchain.GROUPHEAD);
            insertCommand.Parameters.AddWithValue("@GRPHEADNAME", txtCompanyName.Text);
            insertCommand.Parameters.AddWithValue("@STAFFNO", txtEnrollno.Text);
            insertCommand.Parameters.AddWithValue("@PLANTYPE", txtPlantype.Text);
            insertCommand.Parameters.AddWithValue("@SEX", cboSex.Text);
            insertCommand.Parameters.AddWithValue("@NOTEDATE", txtDateofNotification.Text);
            insertCommand.Parameters.AddWithValue("@ADM_DATE", txtDateofAdm.Text);
            insertCommand.Parameters.AddWithValue("@DISCHARGE", txtDateofDischarge.Text);
            insertCommand.Parameters.AddWithValue("@AUTHORITYCODE", txtAuthorizationCode.Text);
            insertCommand.Parameters.AddWithValue("@DIAGNOSIS", txtDiagnosis.Text);
            insertCommand.Parameters.AddWithValue("@ACCOMMODATION", nmrAccommodation.Value);
            insertCommand.Parameters.AddWithValue("@ACC_DAYS", nmrTotalStay.Value);
            insertCommand.Parameters.AddWithValue("@FEED_DAYS", nmrDaysFed.Value);
            insertCommand.Parameters.AddWithValue("@FEEDAMT", nmrFeedingAmount.Value);
            insertCommand.Parameters.AddWithValue("@LAB", txtLabDetails.Text);
            insertCommand.Parameters.AddWithValue("@XRAY", txtRadiological.Text);
            insertCommand.Parameters.AddWithValue("@CONSULTATION", nmrConsultationFee.Value);
            insertCommand.Parameters.AddWithValue("@AGE", txtAge.Text);
            insertCommand.Parameters.AddWithValue("@TRANSTYPE", txtType.Text);
            insertCommand.Parameters.AddWithValue("@TRANS_DATE", dtTrans_date.Value.Date);
            insertCommand.Parameters.AddWithValue("@LABAMT", nmrLaboratoryAmt.Value);
            insertCommand.Parameters.AddWithValue("@XRAYAMT", nmrRadiologicalAmt.Value);
            insertCommand.Parameters.AddWithValue("@OTHERS", txtOtherProcedures.Text);
            insertCommand.Parameters.AddWithValue("@OTHERSAMT", nmrOtherProceduresAmt.Value);
            insertCommand.Parameters.AddWithValue("@ENROLLENO", txtEnrollno.Text);
            insertCommand.Parameters.AddWithValue("@treatmentdate", dtTreatmtDate.Value.Date);
            insertCommand.Parameters.AddWithValue("@PHONE", txtPhone.Text);

            insertCommand.ExecuteNonQuery();

            connection.Close();

            saveDetails_B();
            MessageBox.Show("Details Saved successfully...");
        }
        void saveDetails_B()
        {
            int recid;
            SqlConnection connection = new SqlConnection();
            connection = Dataaccess.mrConnection();

            connection.Open();
            DataGridViewRow dgv;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value == null || string.IsNullOrWhiteSpace(dataGridView1.Rows[i].Cells[1].FormattedValue.ToString()))
                    continue;
                dgv = dataGridView1.Rows[i];
                recid = dgv.Cells[4].Value == null ? 0 : Convert.ToInt32(dgv.Cells[4].Value.ToString());
                SqlCommand insertCommand = new SqlCommand();
                insertCommand.CommandText = recid < 1 ? "ffsc01b_Add" : "ffsc01b_Update";
                insertCommand.Connection = connection;
                insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@PERIOD", dtTrans_date.Value.Month.ToString() + " " + dtTrans_date.Value.Year.ToString());
                insertCommand.Parameters.AddWithValue("@REFERENCE", txtReference.Text);
                insertCommand.Parameters.AddWithValue("@ITEMNO", Convert.ToDecimal(dgv.Cells[0].Value));
                insertCommand.Parameters.AddWithValue("@DESCRIPTION", dgv.Cells[1].FormattedValue.ToString());
                insertCommand.Parameters.AddWithValue("@QTY", dgv.Cells[2].Value == null ? 0m : Convert.ToDecimal(dgv.Cells[2].Value));
                insertCommand.Parameters.AddWithValue("@AMOUNT", dgv.Cells[3].Value == null ? 0m : Convert.ToDecimal(dgv.Cells[3].Value));

                if (recid > 0)
                {
                    insertCommand.Parameters.AddWithValue("@RECID",
                        Convert.ToInt32(dgv.Cells[4].Value));
                }

                insertCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            Session["sql"] = "";
            if (btnSave.Enabled)
                btnSave.PerformClick();

            DataTable sdt = new DataTable();
            if (sdt.DataSet != null)
            {
                sdt.DataSet.Reset();
                ds.Tables.Clear();
                ds.Clear();
            }
            DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT name,address1 FROM customer where custno = '" + bchain.GROUPHEAD + "'", false);
            string[] address_ = new string[2];
            address_[0] = dt.Rows[0]["name"].ToString();
            address_[1] = dt.Rows[0]["address1"].ToString();
            Session["address_"] = (string[])address_;
            dt = Dataaccess.GetAnytable("", "MR", "SELECT * FROM ffsc01b where reference = '" + txtReference.Text + "'", false);
            
            sdt = Dataaccess.GetAnytable("", "MR", "SELECT * FROM ffsc01 where reference = '" + txtReference.Text + "'", false);

            if (sdt.Rows.Count < 1)
            {
                MessageBox.Show("No Data...");
                return;
            }
            ds.Tables.Add(dt);
            ds.Tables.Add(sdt);
            Session["rdlcfile"] = "FFServiceClaims.rdlc";

            string mrptheader = "HMO CLAIMS FORM ";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, "HMO CLAIMS", "", "", "", "FFSERVICECLAIMS", "", 0m, "", "", "", ds, true, 0, DateTime.Now.Date, DateTime.Now.Date, "", isprint, "", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, "HMO CLAIMS", "", "", "", "FFSERVICECLAIMS", "", 0m, "", "", "", ds, 0, DateTime.Now.Date, DateTime.Now.Date, "", isprint, true, "", "");
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

        private void monthCalendar1_DateSelected(object sender, EventArgs e)
        {
            //var monthCalendar = sender as MonthCalendar;
            Button btn = sender as Button;
            if (btn.Name == "btnDateSelect")
            {
                if (calenderText == "btnDateofNotification")
                    txtDateofNotification.Text = monthCalendar1.SelectionStart.ToShortDateString();
                else if (calenderText == "btnDateofAdm")
                    txtDateofAdm.Text = monthCalendar1.SelectionStart.ToShortDateString();
                else if (calenderText == "btnDateofDischarge")
                    txtDateofDischarge.Text = monthCalendar1.SelectionStart.ToShortDateString();
                panelCalender.Visible = false;
            }
            else if (btn.Name == "btnIgnorDateSelect")
            {
                panelCalender.Visible = false;
            }
        }

        private void btnDateofNotification_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnDateofNotification")
                panelCalender.Location = new System.Drawing.Point(224, 58);
            else if (btn.Name == "btnDateofAdm")
                panelCalender.Location = new System.Drawing.Point(458, 58);
            else if (btn.Name == "btnDateofDischarge")
                panelCalender.Location = new System.Drawing.Point(694, 58);
            panelCalender.Visible = true;
            calenderText = btn.Name;
        }

    }
}
/*STORE "" TO mname,maddress1,maddress2,mhead1,mhead2,rptheader
SELECT 0
USE Ffsformprofiler.dat SHARED 
SET ORDER TO tag CUSTNO   && CUSTNO 
IF !SEEK(ThisForm.txtgrouphead.Value)
	USE
	=MESSAGEBOX("No HMO Claims Form header Profiler found...",0,"")
	RETURN
ENDIF
mname = Ffsformprofiler.name
maddress1 = Ffsformprofiler.address1
maddress2 = Ffsformprofiler.address1
rptheader = Ffsformprofiler.formheader
picselected = Ffsformprofiler.formlogo
USE*/