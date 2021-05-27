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
    public partial class frmAttendanceMonitor : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource,Anycode;
        DataTable sdt,tsdt;
        DataSet ds;
        public frmAttendanceMonitor()
        {
            InitializeComponent();
        }
        void getData()
        {
            string rtnstring = " where trans_date >= '" + dtDateFrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59.999'";
            
            if (!string.IsNullOrWhiteSpace(txtgroupcode.Text))
            {
                rtnstring += " and groupcode = '" + txtgroupcode.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtpatientno.Text))
            {
                rtnstring += " and patientno = '" + txtpatientno.Text + "'";
            }
            string xstring = "SELECT * from link3 " + rtnstring+" order by trans_date, name, recid";

            string i1, i2, i3, s1, s2, s3;
            TimeSpan xhr, xmin, xsec;
            string xtimein, xh, xm, xs; xtimein = "";
            string timespt = "";
            DateTime xdate = DateTime.Now.Date;
            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            foreach (DataRow row in sdt.Rows)
            {
                if (xtimein != "" && !new string[] { "C", "I", "P" }.Contains(row["dtype"].ToString().Trim()))
                    row["timein"] = xtimein;
               // "00:00:00"
                //"01234567
                i1 = row["timein"].ToString().Substring(0, 2);
                i2 = row["timein"].ToString().Substring(3, 2);
                i3 = row["timein"].ToString().Substring(6, 2);
               /* xhr = TimeSpan.FromHours( Convert.ToDouble(row["timein"])) - TimeSpan.FromHours(Convert.ToDouble(row["timesent"]));
                xmin = TimeSpan.FromMinutes(Convert.ToDouble(row["timein"])) - TimeSpan.FromMinutes(Convert.ToDouble(row["timesent"]));
                xsec = TimeSpan.FromSeconds(Convert.ToDouble(row["timein"])) - TimeSpan.FromSeconds(Convert.ToDouble(row["timesent"]));
                timespt = xhr.ToString() + ":" + xmin + ":" + xsec.ToString();*/
                s1 = row["timesent"].ToString().Substring(0, 2);
                s2 = row["timesent"].ToString().Substring(3, 2);
                s3 = row["timesent"].ToString().Substring(6, 2);

                xhr = TimeSpan.FromHours(Convert.ToDouble(s1)) - TimeSpan.FromHours(Convert.ToDouble(i1));
                xmin = TimeSpan.FromMinutes(Convert.ToDouble(s2)) - TimeSpan.FromMinutes(Convert.ToDouble(i2));
                xsec = TimeSpan.FromMinutes(Convert.ToDouble(s3)) - TimeSpan.FromMinutes(Convert.ToDouble(i3));
                
                xh = xhr.ToString().Substring(0, 2) != "00" ? xhr.ToString().Substring(0, 2) : ""; // + "hr " : "";
                xm = xmin.ToString().Substring(3, 2) != "00" ? xmin.ToString().Substring(3, 2) : ""; // + "mins " : "";
                xs = xsec.ToString().Substring(3, 2) != "00" ? xsec.ToString().Substring(3, 2) : ""; // + "sec." : "";

                if (xm != "" && xm.Substring(0, 1) == ":") // Convert.ToDouble(xm) < 1)
                {
                    if (xh != "" && xh.Substring(1, 1) == ":")
                        xh = xh.Substring(1, 1);

                    xh = xh == "" ? "1" : (Convert.ToDouble(xh) + 1).ToString();
                    xm = xm.Substring(1, 1);
                }
                if (xs != "" && xs.Substring(0, 1) == ":") // Convert.ToDouble(xs) < 1)
                {
                    if (xm != "" && xm.Length > 1 && xm.Substring(1, 1) == ":" )
                        xm = xm.Substring(1, 1);

                    xm = xm == "" ? "1" : (Convert.ToDouble(xm) + 1).ToString();
                    xs = xs.Substring(1, 1);
                }
                if (xh != "")
                    xh += "hr ";
                if (xm != "")
                    xm += "min ";
                if (xs != "")
                    xs += "sec.";

               /* timespt = xhr.ToString().Substring(0, 2) != "00" ? xhr.ToString().Substring(0, 2) + "hr " : "" + xmin.ToString().Substring(3, 2) + "mins " + xsec.ToString().Substring(3, 2)+"sec.";*/
                timespt = xh + xm + xs;
                row["timespent"] = timespt;
            }

            ds.Tables.Add(sdt);

        }
        void createBFTab()
        {
            tsdt = new DataTable();
            tsdt.Columns.Add(new DataColumn("reference", typeof(string)));
            tsdt.Columns.Add(new DataColumn("balbf", typeof(decimal)));
            tsdt.Columns.Add(new DataColumn("name", typeof(string)));
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            printprocess(false);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printprocess(true);
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (dtDateFrom.Value < msmrfunc.mrGlobals.mta_start || dtDateto.Value < dtDateFrom.Value || dtDateFrom.Value.Date > DateTime.Now.Date || dtDateto.Value.Date > DateTime.Now.Date)
            {
                result = MessageBox.Show("Invalid Date specification...");
                return;
            }
            string rptfooter, rptcriteria; rptfooter = rptcriteria = "";
            sdt = new DataTable();
            ds = new DataSet();
            getData();
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data...");
                return;
            }
            Session["rdlcFile"] = "rptAttendanceMonitor.rdlc";
            Session["sql"] = "";
            string xstr1 = chkStatistics.Checked ? "(AVR STATISTICS) " : "";
            string xstr2 = dtDateFrom.Value.Date == dtDateto.Value.Date ? dtDateto.Value.ToLongDateString() : " FROM "+dtDateFrom.Value.ToShortDateString()+" TO : "+dtDateto.Value.ToShortDateString();
            string mrptheader = "PATIENT'S ATTENDANCE MONITOR " + xstr1 + xstr2;
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, rptfooter, rptcriteria, "", "ATTENDMONITOR", "", 0m, "", "", "", ds, true, 0, dtDateFrom.Value.Date, dtDateto.Value.Date, "", isprint, "", "");
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(this.Text, mrptheader, rptfooter, rptcriteria, "", "ATTENDMONITOR", "", 0m, "", "", "", ds, 0, dtDateFrom.Value.Date, dtDateto.Value.Date, "", isprint, true, "", "");
            }
        }
        private void btnReference_Click(object sender, EventArgs e)
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
                this.txtgroupcode.Text = Anycode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Text = msmrfunc.mrGlobals.anycode1;
                this.txtgroupcode.Focus();
            }
            else if (lookupsource == "L") //patientno
            {
                txtpatientno.Text = Anycode = msmrfunc.mrGlobals.anycode;
                txtpatientno.Select();
            }
            else if (lookupsource == "R") //patientno
            {
                txtReference.Text = Anycode = msmrfunc.mrGlobals.anycode;
                txtReference.Select();
            }
        }

        private void txtpatientno_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpatientno.Text))
                return;
            if (string.IsNullOrWhiteSpace(Anycode ) && bissclass.IsDigitsOnly(txtpatientno.Text.Trim()))  //no lookup value obtained
            {
                txtpatientno.Text = bissclass.autonumconfig(txtpatientno.Text, true, "", "9999999");
            }
            DialogResult result;
            //check if patientno exists
            bchain = billchaindtl.Getbillchain(txtpatientno.Text, txtgroupcode.Text);
            if (bchain == null)
            {
                result = MessageBox.Show("Invalid Patient Number... ");
                txtpatientno.Text = " ";
                txtgroupcode.Select();
                return;
            }
            lblname.Text = bchain.NAME;
            dtDateFrom.Focus();
            return;
        }
    }
}