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

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
    public partial class frmFeeForServiceClaims : Form
    {
        billchaindtl bchain = new billchaindtl();
        string lookupsource, AnyCode;
        DataTable sdt;
        public frmFeeForServiceClaims()
        {
            InitializeComponent();
        }
        private void frmFeeForServiceClaims_Load(object sender, EventArgs e)
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
            else if (lookupsource == "R") //patientno
            {
                txtReference.Text = AnyCode = msmrfunc.mrGlobals.anycode;
                txtReference.Select();
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
            txtName.Text = bchain.NAME;
            txtgroupcode.Text = bchain.GROUPCODE;
            txtpatientno.Text = bchain.PATIENTNO;
            txtgrouphead.Text = bchain.GROUPHEAD;
            dtDateFrom.Focus();
        }
        private void txtReference_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReference.Text))
                return;
            string xfile = chkInpatient.Checked ? ", admdate, discharge from ADMRECS" : ", trans_date from MRATTEND";

            string xstring = "SELECT REFERENCE, PATIENTNO, NAME, GROUPHTYPE, GROUPHEAD, GROUPCODE, GHGROUPCODE"+xfile+" where reference = '"+txtReference.Text + "'";
            sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            if (sdt.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("Invalid Reference...");
                txtReference.Text = "";
                return;
            }

            DataRow row = sdt.Rows[0];
            txtName.Text = row["name"].ToString();
            txtgroupcode.Text = row["groupcode"].ToString();
            txtpatientno.Text = row["patientno"].ToString();
            txtgrouphead.Text = row["grouphead"].ToString();
            dtDateFrom.Value = chkInpatient.Checked ? (DateTime)row["adm_date"] : (DateTime)row["trans_date"];
            dtDateto.Value = chkInpatient.Checked && !string.IsNullOrWhiteSpace(row["discharge"].ToString()) ? Convert.ToDateTime(row["discharge"].ToString()) : !chkInpatient.Checked ? (DateTime)row["trans_date"] : DateTime.Now.Date;
            dtDateFrom.Enabled = dtDateto.Enabled = false;
            btnEdit.Enabled = true;
            bchain = billchaindtl.Getbillchain(row["patientno"].ToString(), row["groupcode"].ToString());
            if (txtReference.Text.Substring(0, 1) == "A")
                chkInpatient.Checked = true;
            else
                chkOutPatient.Checked = true;
 
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (chkClaimsALLHMO.Checked && (string.IsNullOrWhiteSpace(txtpatientno.Text) || string.IsNullOrWhiteSpace(txtReference.Text)))
            {
                result = MessageBox.Show("Consult Reference or Patient Number/Transaction Dates must be specified for this Report...", "Fee For Service Claims Details");
                return;
            }
            if (!chkClaimsALLHMO.Checked && string.IsNullOrWhiteSpace(txtgrouphead.Text))
            {
                result = MessageBox.Show("GroupHead must be specified for this report...");
                return;
            }
            if (chkClaimsALLHMO.Checked)
            {
                frmFeeForServiceEdit ffservice = new frmFeeForServiceEdit(bchain, sdt, txtReference.Text, chkOutPatient.Checked ? false : true, dtDateFrom.Value.Date, dtDateto.Value.Date );
                ffservice.Show();
            }
        }
        private void txtgrouphead_LostFocus(object sender, EventArgs e)
        {
 
            if (string.IsNullOrWhiteSpace(txtgrouphead.Text) )
                return;

            Customer customer = Customer.GetCustomer(this.txtgrouphead.Text);

            if (customer == null)
            {
                MessageBox.Show("Invalid GroupHead Specification as responsible for Bills");
                return;
            }
            if (customer.HMO)
            {
                cboHmoPlantype.DataSource = Dataaccess.GetAnytable("", "MR", "SELECT HMOSERVTYPE,DESCRIPTION FROM HMODETAIL WHERE custno = '" + customer.Custno + "'", true); // selcode.getHMOplantypecombolist("hp", customer.Custno);
                cboHmoPlantype.ValueMember = "HMOSERVTYPE";
                //Setting Combo Box DisplayMember Property
                cboHmoPlantype.DisplayMember = "HMOSERVTYPE + ' : ' +Description ";
            }
        }
    }
}