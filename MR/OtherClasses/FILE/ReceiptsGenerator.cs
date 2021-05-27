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

using mradmin.Forms;
using mradmin.DataAccess;
using mradmin.BissClass;
using MSMR.Forms;
using MSMR;
//using System.Net;

//using Gizmox.WebGUI.Common;
//using Gizmox.WebGUI.Forms;
using System.Net.Http;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class ReceiptsGenerator
    {
        bool isdataset;
        string lookupsource, AnyCode, mrptheader, rptcriteria, groupcode, patientno, paydesc,
            rptfooter, sysmodule = bissclass.getRptfooter(), woperator;
        DateTime datefrom, dateto;
        DataSet ds = new DataSet();
        bool isinvoice, disallowbackdate;
        public static string mycontent = null;
        MR_DATA.MR_DATAvm vm;

        public ReceiptsGenerator(string woperato, MR_DATA.MR_DATAvm VM2)
        {
            //InitializeComponent();

            //txtReference.Text = receiptnumber;
            vm = VM2;
            woperator = woperato;
            isdataset = vm.REPORTS.chkAuditProfile;
            isinvoice = false;
            DateTime datefrom = Convert.ToDateTime(vm.REPORTS.REPORT_TYPE5);
            DateTime dateto = Convert.ToDateTime(vm.REPORTS.REPORT_TYPE4);

            
            //disallowbackdate = xdisallowbackdate;
        }

        //private void ReceiptsGenerator_Load(object sender, EventArgs e)
        //{
        //    txtReference.Select();
        //}

        //private void btnReference_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn.Name == "btnReference")
        //    {
        //        lookupsource = "PAY";
        //        msmrfunc.mrGlobals.crequired = "PAY";
        //        msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR SAVED PAYMENT RECORDS";
        //    }
        //    frmselcode FrmSelCode = new frmselcode();
        //    FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
        //    FrmSelCode.ShowDialog();
        //}

        //void FrmSelCode_Closed(object sender, EventArgs e)
        //{
        //    frmselcode FrmSelcode = sender as frmselcode;
        //    if (lookupsource == "PAY")
        //    {
        //        txtReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
        //        txtReference.Focus();
        //    }
        //}

        //private void btnPreview_Click(object sender, EventArgs e)
        //{
        //    printprocess(false);
        //}

        public MR_DATA.REPORTS printprocess()
        {
            bool isprint = false;

            //if (string.IsNullOrWhiteSpace(txtName.Text))
            //{
            //    DialogResult result = MessageBox.Show("Payment Record/Customer must be Retrieved...");
            //    txtReference.Focus();
            //    return;
            //}

            //if (disallowbackdate && datefrom < DateTime.Now.Date)
            //{
            //    MessageBox.Show("Can't print Receipt for Transaction Date Less Than Today...", "Transaction Date Control Setting");
            //    return;
            //}

            if (vm.REPORTS.txtaddress1.Trim() == "true")
            {
                ds.Clear();
                ds.Tables.Clear();
                DataTable dtpay2 = Dataaccess.GetAnytable("", "MR", "SELECT * FROM PAYDETAIL WHERE rtrim(REFERENCE) = '" + vm.REPORTS.txtreference.Trim() + "'", false);

                if (dtpay2.Rows.Count < 1)
                {
                    vm.REPORTS.alertMessage = "Invalid Payment Reference...";
                    return vm.REPORTS;
                }

                ds.Tables.Add(dtpay2);
            }



            if (vm.REPORTS.REPORT_TYPE1 == "chkRecptSMS")
            {
                ReceiptSMS();
            }
            else
            {
                vm.REPORTS.SessionRDLC = vm.REPORTS.REPORT_TYPE1 == "chkRecptPOS" ? "POSReceiptnew.rdlc" : "receipt_std_two.rdlc";
                vm.REPORTS.SessionSQL = "";
                mrptheader = "RECEIPT GENERATOR ";
                string xrpttype = vm.REPORTS.REPORT_TYPE1 == "chkRecptPOS" ? "POS" : "RECEIPTSTD";
                rptfooter = rptcriteria = "";

                frmReportViewer receipt = new frmReportViewer(mrptheader, mrptheader, rptfooter,
                   rptcriteria, "", xrpttype, vm.REPORTS.txtreference, 0m, "", "", "", ds, isdataset, 0,
                   datefrom, dateto, "", isprint, "", woperator, vm.REPORTS);

                vm.REPORTS = receipt.Show(vm.REPORTS.SessionRDLC, vm.REPORTS.SessionSQL, vm.REPORTS.PRINT);
               
                //if (!isprint)
                //{
                //    frmReportViewer receipt = new frmReportViewer(mrptheader, mrptheader, rptfooter, rptcriteria, "", xrpttype, txtReference.Text, 0m, "", "", "", ds, isdataset, 0, datefrom, dateto, "", isprint, "", woperator);
                //    receipt.Show();
                //}
                //else
                //{
                //    MRrptConversion.GeneralRpt(mrptheader, mrptheader, rptfooter, "", "", xrpttype, txtReference.Text, 0M, "", "", "", ds, 0, datefrom, dateto, "", isprint, true, "", woperator);
                //}
            }

            return vm.REPORTS;
        }

        //private void btnPrint_Click(object sender, EventArgs e)
        //{
        //    printprocess(true);
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void txtReference_Leave(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtReference.Text ))
        //    {
        //        return;
        //    }

        //    ds.Clear();
        //    ds.Tables.Clear();
        //    DataTable dtpay = Dataaccess.GetAnytable("", "MR", "SELECT * FROM PAYDETAIL WHERE rtrim(REFERENCE) = '" + txtReference.Text.Trim() + "'", false);

        //    if (dtpay.Rows.Count < 1)
        //    {
        //        DialogResult result = MessageBox.Show("Invalid Payment Reference...");
        //        txtReference.Text = "";
        //        return;
        //    }

        //    groupcode = dtpay.Rows[0]["groupcode"].ToString();
        //    patientno = dtpay.Rows[0]["patientno"].ToString();
        //    paydesc = dtpay.Rows[0]["description"].ToString();
        //    txtName.Text = dtpay.Rows[0]["name"].ToString();
        //    nmrAmount.Value = (decimal)dtpay.Rows[0]["amount"];
        //    datefrom = dateto = (DateTime)dtpay.Rows[0]["trans_date"];
        //    ds.Tables.Add(dtpay);
        //    isdataset = true;
        //}

        //private void chkSMS_Click(object sender, EventArgs e)
        //{
        //    if (chkSMS.Checked)
        //        btnPrint.Text = "Send";

        //}

        void ReceiptSMS()
        {
            //DialogResult result;
            DataTable dt = Dataaccess.GetAnytable("", "CODES", "select name,user_name,mpass,state from ctrolxl where recid = '7' ", false);
            DataRow row = dt.Rows[0];

            if (string.IsNullOrWhiteSpace(row["name"].ToString()) || string.IsNullOrWhiteSpace(row["mpass"].ToString()))
            {
                vm.REPORTS.alertMessage = "SMS Router not properly configured...";
                return;
            }

            DataTable dtp = Dataaccess.GetAnytable("", "MR", "select phone from billchain where groupcode = '" + groupcode + "' and patientno = '" + patientno + "'", false);

            if (string.IsNullOrWhiteSpace(dtp.Rows[0]["phone"].ToString()))
            {
                vm.REPORTS.alertMessage = "No recorded phone number for this patient...";
                return;
            }

            string smssender, senderpasswd, sender;
            smssender = row["name"].ToString().Trim();
            senderpasswd = Dataaccess.DecryptString(row["mpass"].ToString());
            sender = row["user_name"].ToString();
            string xref = dtp.Rows[0]["phone"].ToString(), to, msg;
            POPREAD popread = new POPREAD("Defined Phone Number(s)", "Edit", ref xref, false, false, "", "", "", false, "", "");
            //popread.ShowDialog();

            if (string.IsNullOrWhiteSpace(bissclass.sysGlobals.anycode))
            {
                vm.REPORTS.alertMessage = "No recorded phone number for this patient...";
                return;
            }

            to = bissclass.sysGlobals.anycode.Trim();
            //  WebClient client = new WebClient();

            //, user, password, send;
            // dtp.Rows[0]["phone"].ToString().Trim();
            /*  msg = "Payment Notification:\r\n Hospital # : " + groupcode.Trim() + ":" + patientno.Trim() +" ["+txtName.Text.Trim()+"]\r\n"+
                  "Desc: " + paydesc + "\r\n" +
                  "Amount Paid : " + nmrAmount.Value.ToString("N2") + "\r\n" +
                  "Date : " + datefrom.ToLongDateString() + "\r\n" +
                  "Thank you.";*/
            msg = string.Format("Payment Notification: \n" +
                "Hospital # : {0}  : {1} [ {2} ] \n" +
               "Desc: {3} \n" +
               "Amount Paid : {4} \n" +
               "Date : {5} \n" +
               "Thank you.", groupcode.Trim(), patientno.Trim(), vm.REPORTS.TXTPATIENTNAME.Trim(), paydesc, vm.REPORTS.nmrMinBalance.ToString("N2"), datefrom.ToLongDateString()).Replace("\n", Environment.NewLine);
           
            //msg = "TEST RECEIPT MSG";
            vm.REPORTS.alertMessage = msg + "\r\n TO PHONE(s) : " + to;

            //if (result == DialogResult.Cancel)
            //    return;

            //  user = smssender; // txtUsername.Text;
            //  password = senderpasswd; // txtPassword.Text;
            //   send = sender;
            /*    string baseurl = string.Format("http://login.betasms.com/customer/api/?username={0}&password={1}&message=Payment Notification: \nHospital # : {3}  : {4} [ {5} ]\nDesc: {6} \nAmount Paid : {7}\nDate : {8} \nThank you.&sender={9}&mobiles={2}", user, password, to, groupcode.Trim(), patientno.Trim(), txtName.Text.Trim(), paydesc, nmrAmount.Value.ToString("N2"), datefrom.ToLongDateString().Replace("\n", Environment.NewLine), send);

                client.OpenRead(baseurl);
                //Dataaccess.writeArchivedSMS(txtSender.Text, to, DateTime.Now, msg, 0);
                MessageBox.Show("Message Sent Successfully");*/

            vm.REPORTS.TXTPATIENTNAME = "Sending....Pls Wait!";

            PostRequest("http://login.betasms.com/customer/api/", to, msg, smssender, senderpasswd, sender);

            /*           if (mycontent == "\t1701")
                           result = MessageBox.Show("Message Sent Successfully");
                       else if (mycontent == "\t1705")
                           result = MessageBox.Show("Invalid Recipients or URL");
                       else if (mycontent == "\t1702")
                           result = MessageBox.Show("Invalid Username or Password");
                       else if (mycontent == "\t1025" || mycontent == "\t1704")
                           result = MessageBox.Show("You don't have Sufficient Credit to Perform this Transaction");
                       else if (mycontent == "\t1706")
                           result = MessageBox.Show("Internal Server Error");*/
        }//to, msg, user, password, send

        async void PostRequest(string url, string to, string msg, string user, string password, string sender)
        {

            IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("username", user), // "info@adisystems-ng.com"),
                new KeyValuePair<string, string>("password", password), // "okota@165"),
                new KeyValuePair<string, string>("message", msg), // "Payment Notification: \n Hospital #: KUPA Hospital\nDesc: Malaria Drug\nAmount:#500"),
                new KeyValuePair<string, string>("sender", sender), // "ADISYS"),
                new KeyValuePair<string, string>("mobiles", to) // "07034834761"),
            };
            HttpContent content = new FormUrlEncodedContent(queries);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(url, content))
                {
                    using (HttpContent outCont = response.Content)
                    {
                        mycontent = await outCont.ReadAsStringAsync();
                        displayresult(mycontent);
                    }
                }
            }
        }

        void displayresult(string xresult)
        {
            // DialogResult result;
            //txtName.ForeColor = Color.Red;
            if (xresult == "\t1701")
                //MessageBox.Show("Message Sent Successfully");
                vm.REPORTS.alertMessage = "Message Sent Successfully"; //for txtName.Text
            else if (xresult == "\t1705")
                //MessageBox.Show("Invalid Recipients or URL");
                vm.REPORTS.alertMessage = "Invalid Recipients or URL";
            else if (xresult == "\t1702")
                vm.REPORTS.alertMessage = "Invalid Username or Password";
            else if (xresult == "\t1025" || mycontent == "\t1704")
                vm.REPORTS.alertMessage = "You don't have Sufficient Credit to Perform this Transaction";
            else if (xresult == "\t1706")
                vm.REPORTS.alertMessage = "Internal Server Error";
        }

        //private void txtReference_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        //{
        //    txtReference_Leave(null, null);
        //}
    }
}