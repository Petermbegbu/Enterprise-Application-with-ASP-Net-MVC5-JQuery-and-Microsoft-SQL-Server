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
	public partial class frmAttendanceList : Form
	{
		billchaindtl bchain = new billchaindtl();
		string lookupsource, AnyCode, mrptheader, rptfooter = bissclass.getRptfooter();
		DateTime datefrom = DateTime.Now.Date, dateto = DateTime.Now.Date;
		DataSet ds = new DataSet();
        DataTable sdt, dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtdocs = Dataaccess.GetAnytable("", "MR", "SELECT REFERENCE,NAME FROM DOCTORS WHERE RECTYPE = 'D'", true), dtcust = Dataaccess.GetAnytable("", "MR", "SELECT CUSTNO,NAME FROM CUSTOMER order by name", true);
		public frmAttendanceList()
		{
			InitializeComponent();
		}
		private void frmAttendanceList_Load(object sender, EventArgs e)
		{
			initcomboboxes();
		}
		private void initcomboboxes()
		{
			cboFacility.DataSource = dtfacility;
			cboFacility.ValueMember = "Type_code";
			cboFacility.DisplayMember = "name";

			combDoc.DataSource = dtdocs;//medical staff details - doctors
			combDoc.ValueMember = "Reference";
			combDoc.DisplayMember = "Name";

			cboHospitalAccount.DataSource = dtcust;
			cboHospitalAccount.ValueMember = "custno";
			cboHospitalAccount.DisplayMember = "name";
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
			else if (btn.Name == "btngrouphead")
			{
				txtgrouphead.Text = "";
				lookupsource = "C";
				msmrfunc.mrGlobals.crequired = "C";
				msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED CORP. CLIENTS";
			}
			else if (btn.Name == "btnReference")
			{
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
				MessageBox.Show("Invalid Patient Number... ");
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
				MessageBox.Show("Invalid Corporate Clients Reference...");
				txtgrouphead.Text = "";
				return;
			}
			lblname.Text = dtcustomer.Rows[0]["name"].ToString();
		}
		void getData()
		{
			bool lhistory = false;
			if (dtDateFrom.Value.Year < msmrfunc.mrGlobals.mpyear)
				lhistory = true;

			string rptfile = lhistory ? "attendhist" : "mrattend";

			string selstring = " WHERE mrattend.trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and mrattend.trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
		   // string selstring = " WHERE mrattend.trans_date between '" + dtDateFrom.Value.ToShortDateString() + "' and  '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'"; //'2011/02/25' and '2011/02/27'"
			if (!string.IsNullOrWhiteSpace(txtgrouphead.Text))
				selstring += " AND mrattend.GROUPHEAD = '" + txtgrouphead.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
				selstring += " AND mrattend.GROUPCODE = '" + txtgroupcode.Text + "'";
			if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
				selstring += " AND mrattend.PATIENTNO = '" + txtpatientno.Text + "'";
			if (!string.IsNullOrWhiteSpace(cboFacility.Text))
				selstring += " AND mrattend.clinic = '" + cboFacility.SelectedValue.ToString() + "'";
			if (chkWaitingList.Checked )
				selstring += " and mrattend.trans_date = '" + DateTime.Now.Date + "' and mrattend.doc_time = ''";
			if (chkExceptions.Checked)
				selstring += " and mrattend.billed = 'X'";
			if (chkNotSeenByDoc.Checked)
				selstring += " and mrattend.doc_time = ''";
			if (chkNHISonly.Checked)
				selstring += " and rtrim(mrattend.groupcode) = 'NHIS'";
			else if (chkSpecialServicePatients.Checked)
				selstring += " and left(mrattend.reference,1) = 'S'";
			else if (chkPVTFC.Checked)
				selstring += " and (left(mrattend.groupcode) = 'PVT' || rtrim(mrattend.groupcode) = 'FC')";
			else if (chkCorporates.Checked)
				selstring += " and (mrattend.groupcode) = '' && rtrim(mrattend.groupcode) != 'NHIS')";
			else if (chkHMO.Checked)
				selstring += " and (mrattend.groupcode) = '' && rtrim(mrattend.groupcode) != 'NHIS')";

			if (chkSortByAttendanceType.Checked && !chkALL.Checked )
			{
				if (chkMedicalTreatmt.Checked)
					selstring += " and mrattend.attendtype = 'M'";
				else if (chkMedicalExam.Checked)
					selstring += " and mrattend.attendtype = 'E'";
				else if (chkDressingInj.Checked)
					selstring += " and mrattend.attendtype = 'D'";
				else if (chkOnAppointmt.Checked)
					selstring += " and mrattend.attendtype = 'O'";
				else if (chkSpecialistConsult.Checked)
					selstring += " and mrattend.attendtype = 'S'";
				else if (chkOnFollowup.Checked)
					selstring += " and mrattend.attendtype = 'F'";
				else if (chkDrugRefill.Checked)
					selstring += " and mrattend.attendtype = 'R'";
			}
			if (!string.IsNullOrWhiteSpace(txtNameSearch.Text))
				selstring += " AND mrattend.NAME LIKE '%" + txtNameSearch.Text.Trim() + "%' ";
			if (chkSortByDocs.Checked && !string.IsNullOrWhiteSpace(combDoc.Text))
				selstring += " and mrattend.doctor = '" + combDoc.SelectedValue.ToString() + "'";
 
			string xstr = ", char(50) AS facility, CHAR(50) AS GHNAME ";
			if (chkDtlwithPhone.Checked)
				xstr += ", CHAR(50) AS address, char(50) AS email, char(50) AS phone ";
		   // if (chkOnDocsPatProfile.Checked)
			//    xstr += ", CHAR(50) AS docsname ";
			if (chkMonthlyComparative.Checked)
			{

				DataTable dtmthlyfig = new DataTable(), dtaggregate = new DataTable();
				msmrfunc.processComparative(ref dtmthlyfig, ref dtaggregate, dtDateFrom.Value, dtDateto.Value, "ATTENDANCE", cboHospitalAccount.SelectedValue.ToString());
				ds.Tables.Add(dtmthlyfig);
				ds.Tables.Add(dtaggregate);

				Session["rdlcfile"] = "ComparativeRpt.rdlc";
				return;
			}
			string xstring;
			//if (chkSortByDocs.Checked)
			//    xstring = "SELECT mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.NAME, mrattend.TRANS_DATE, mrattend.CLINIC, mrattend.BILLED, mrattend.GROUPHEAD, mrattend.GROUPHTYPE, mrattend.VTAKEN, mrattend.GHGROUPCODE, medhist.DOCTOR, mrattend.DOC_TIME, mrattend.DIAGNOSIS, mrattend.ATTENDTYPE, CHAR(50) AS DOCSNAME, mrattend.REFERRER,mrattend.AUTHORIZEDCODE, billchain.section, billchain.department, billchain.PATIENTNO_PRINCIPAL, billchain.sex" + xstr + " from " + rptfile + " LEFT JOIN BILLCHAIN ON MRATTEND.GROUPCODE = BILLCHAIN.GROUPCODE AND MRATTEND.PATIENTNO = BILLCHAIN.PATIENTNO LEFT JOIN medhist on mrattend.reference = medhist.reference" + selstring;
			if (chkDtlwithPhone.Checked)
				xstring = "SELECT mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.NAME, mrattend.TRANS_DATE, mrattend.CLINIC, mrattend.BILLED, mrattend.GROUPHEAD, mrattend.GROUPHTYPE, mrattend.VTAKEN, mrattend.GHGROUPCODE, mrattend.DOCTOR, mrattend.DOC_TIME, mrattend.DIAGNOSIS, mrattend.ATTENDTYPE, CHAR(50) AS DOCSNAME, customer.name AS REFERRER, mrattend.AUTHORIZEDCODE" + xstr + " from " + rptfile + " LEFT JOIN CUSTOMER ON mrattend.referrer = customer.custno " + selstring;
			else
				xstring = "SELECT mrattend.REFERENCE, mrattend.GROUPCODE, mrattend.PATIENTNO, mrattend.NAME, mrattend.TRANS_DATE, mrattend.CLINIC, mrattend.BILLED, mrattend.GROUPHEAD, mrattend.GROUPHTYPE, mrattend.VTAKEN, mrattend.GHGROUPCODE, mrattend.DOCTOR, mrattend.DOC_TIME, mrattend.DIAGNOSIS, mrattend.ATTENDTYPE, CHAR(50) AS DOCSNAME, mrattend.REFERRER,mrattend.AUTHORIZEDCODE, billchain.section, billchain.department, billchain.PATIENTNO_PRINCIPAL, billchain.sex" + xstr + " from " + rptfile +" LEFT JOIN BILLCHAIN ON MRATTEND.GROUPCODE = BILLCHAIN.GROUPCODE AND MRATTEND.PATIENTNO = BILLCHAIN.PATIENTNO "+ selstring;

			if (chkNHISonly.Checked || chkHMO.Checked || chkCorporates.Checked )
				xstring += " order by grouphead,trans_date";
			else
				xstring += " order by trans_date";
			sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
			//CHECKA AND GET REMOTE DATA
		  /*  if (chkGetRemoteData.Checked)
			{
				frmGetRemoteData getrd = new frmGetRemoteData(sdt, xstring, "MR");
				getrd.ShowDialog();
				if (Session["remotedata"] != null)
					sdt = (DataTable)Session["remotedata"];
			}*/
			DataTable dtt;
			string tmpstr;
			foreach (DataRow row in sdt.Rows)
			{
				row["facility"] = bissclass.combodisplayitemCodeName("type_code",row["clinic"].ToString(), dtfacility, "name");
				if (chkDtlwithPhone.Checked)
				{
					tmpstr = row["reference"].ToString().Substring(0,1) == "S" ? "select address1, email, phone from suspense where reference = '"+row["reference"].ToString() : "select residence As address1, email, phone from billchain where groupcode = '"+row["groupcode"].ToString()+"' and patientno = '"+row["patientno"].ToString();
					dtt = Dataaccess.GetAnytable("","MR",tmpstr,false);
					row["address"] = dtt.Rows[0]["address1"].ToString();
					row["email"] = dtt.Rows[0]["email"].ToString();
					row["phone"] = dtt.Rows[0]["phone"].ToString();
				}
				else if (chkNHISonly.Checked || chkCorporates.Checked || chkHMO.Checked )
				{
					row["facility"] = row["section"].ToString().Trim() + ", " + row["department"].ToString().Trim() + "[" + row["sex"].ToString() + "][" + row["PATIENTNO_PRINCIPAL"].ToString().Trim() + "]";
				}
				if (!string.IsNullOrWhiteSpace(row["doctor"].ToString())) // chkOnDocsPatProfile.Checked)
					row["docsname"] = bissclass.combodisplayitemCodeName("reference", row["doctor"].ToString(), dtdocs, "name");
				else
					row["docsname"] = "< Unspecified >";
				if (row["patientno"].ToString() == row["grouphead"].ToString())
					row["ghname"] = "< S E L F >";
				else if (row["grouphtype"].ToString() == "P")
					row["ghname"] = msmrfunc.GETGroupheadname(row["ghgroupcode"].ToString(), row["grouphead"].ToString(), "P");
				else
					row["ghname"] = bissclass.combodisplayitemCodeName("custno", row["grouphead"].ToString(), dtcust, "name");
			}
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		void printprocess(bool isprint)
		{
			//DialogResult result;
			if (dtDateFrom.Value.Date > DateTime.Now.Date || dtDateFrom.Value.Date < msmrfunc.mrGlobals.mta_start || dtDateto.Value.Date < dtDateFrom.Value.Date || dtDateFrom.Value.Year != dtDateto.Value.Year )
			{
				MessageBox.Show("Invalid Date Specification");
				return;
			}
			/*if (chkMonthlyComparative.Checked && dtDateFrom.Value == dtDateto.Value)
			{
				DialogResult result = MessageBox.Show("Monthly Comparative summary report should be \r\n for more than 1 day or 1 month...CONTINUE?", "Comparative Report Date Range", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (result == DialogResult.No )
					return;
			}*/
			if (chkMonthlyComparative.Checked && string.IsNullOrWhiteSpace(cboHospitalAccount.Text))
			{
				MessageBox.Show("Hospital Account for Staff Registrations must be Selected...", "Monthly Comparative Summary Report");
				return;
			}
		   /* if (sdt != null)
			{
				sdt.Rows.Clear();
				ds.Tables.Clear();
				ds.Clear();
			}*/
			sdt = new DataTable();
			ds = new DataSet();
			getData();
            string xperiod = " FOR ";
            if (chkWaitingList.Checked)
                xperiod += bissclass.DateWord(DateTime.Now.Date);
            else if (dtDateFrom.Value == dtDateto.Value)
                xperiod += bissclass.DateWord(dtDateFrom.Value.Date);
            else
            {
                xperiod += dtDateFrom.Value.Date.ToString("MMMM") + " " + dtDateFrom.Value.Day.ToString();
                string xyear = "";
                if (dtDateFrom.Value.Month == dtDateto.Value.Month)
                    xyear = " - " + dtDateto.Value.Day.ToString() + ",  " + dtDateFrom.Value.Year.ToString();
                else if (dtDateFrom.Value.Month != dtDateto.Value.Month && dtDateFrom.Value.Year == dtDateto.Value.Year)
                    xyear = " - " + dtDateto.Value.Date.ToString("MMMM") + " " + dtDateto.Value.Day.ToString() + ",  " + dtDateFrom.Value.Year.ToString();
                else
                    xyear = ", " + dtDateFrom.Value.Year.ToString() + " - " + dtDateto.Value.Date.ToString("MMMM") + " " + dtDateto.Value.Day.ToString() + ",  " + dtDateto.Value.Year.ToString();
                xperiod += xyear;
            }
            datefrom = dtDateFrom.Value.Date; dateto = dtDateto.Value.Date;
            Session["sql"] = "";
            if (chkMonthlyComparative.Checked)
            {
                mrptheader = "MONTHLY COMPARATIVE SUMMARY REPORT";
                if (datefrom.Month != 1 || dateto.Month != 12)
                {
                    datefrom = Convert.ToDateTime("01/01/" + datefrom.Year.ToString());
                    dateto = Convert.ToDateTime("31/12/" + datefrom.Year.ToString());
                }
            }
            else 
            {
                if (sdt.Rows.Count < 1)
                {
                    MessageBox.Show("No Data for Specified Conditions...");
                    return;
                }
                ds.Tables.Add(sdt);

                if (chkGroupbyClinic.Checked || !string.IsNullOrWhiteSpace(cboFacility.Text))
                    Session["rdlcfile"] = "Attendance_ByClinicGh.rdlc"; //"Attendance_ByClinic.rdlc";
                else if (chkSortByAttendanceType.Checked)
                    Session["rdlcfile"] = "Attendance_VisitType.rdlc";
                else if (chkSortByDocs.Checked)
                    Session["rdlcfile"] = "Attendance_ByDocs.rdlc";
                else if (chkDtlwithPhone.Checked)
                    Session["rdlcfile"] = "Attendance_MiscDtl.rdlc";
                else if (chkNHISonly.Checked)
                    Session["rdlcfile"] = "Attendance_NHIS.rdlc";
                else
                    Session["rdlcfile"] = "Attendance_Serial.rdlc";
                Session["waitonly"] = chkWaitingList.Checked ? "Y" : "N";
                if (chkWaitingList.Checked)
                    mrptheader = "WAITING LIST"; // FOR " + bissclass.DateWord(DateTime.Now.Date);
                else if (chkExceptions.Checked)
                    mrptheader = "SERVICE EXCEPTIONS LISTING";
                else
                    mrptheader = "ATTENDANCE LIST";


                if (chkSpecialServicePatients.Checked)
                    xperiod += " - SPECIAL SERVICE ONLY";
                else if (chkNHISonly.Checked)
                    xperiod += " - NHIS ONLY";
                else if (chkPVTFC.Checked)
                    xperiod += " - EXCLUDES NHIS & SP.SERVICE";
 
            }
			if (!isprint)
			{
				frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader + xperiod, rptfooter, "", "", chkMonthlyComparative.Checked ? "SUMMARYOFACCTS" : "ATTENDANCE", "", 0m, "", "", "", ds, true, 0, datefrom, dateto, "", isprint, "", "");

				//if (isprint)
				//    paedreports.work();
				//else
				paedreports.Show();
			}
			else
			{
				MRrptConversion.GeneralRpt(mrptheader, mrptheader + xperiod, rptfooter, "", "", chkMonthlyComparative.Checked ? "SUMMARYOFACCTS" : "ATTENDANCE", "", 0m, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", "");
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

		private void chkMonthlyComparative_CheckedChanged(object sender, EventArgs e)
		{
			if (chkMonthlyComparative.Checked)
				panel_MonthlyCummative.Visible = true;
			else
				panel_MonthlyCummative.Visible = false;

		}
	}
}