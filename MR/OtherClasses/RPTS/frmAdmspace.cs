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
    public partial class frmAdmspace : Form
    {
        DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name ", true),sdt;
        DataSet ds = new DataSet();
        string woperator;
        public frmAdmspace()
        {
            InitializeComponent();
        }

        private void frmAdmspace_Load(object sender, EventArgs e)
        {
            woperator = Session["operator"].ToString();
            initcomboboxes();
        }
        void initcomboboxes()
        {
            cboFacility.DataSource = dtfacility;
            cboFacility.ValueMember = "Type_code";
            cboFacility.DisplayMember = "NAME";
        }
        bool getData()
        {
            string bstr = "";
            if (!string.IsNullOrWhiteSpace(cboFacility.Text))
                bstr = " where facility = '" + cboFacility.SelectedValue.ToString() + "'";

            string xstring = "select FACILITY, NAME, ROOM, BED, DESCRIPTION, RATE, OCCUPANT, BOOKED, BOOKEDDATE, NURSINGCARE, adm_deposit, CHAR(50) AS accname from admspace" + bstr + " order by facility";

            DataTable sdt = Dataaccess.GetAnytable("", "MR", xstring, false);
            if (sdt.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("No Data...");
                return false;
            }
            string facilitysave = "",facname = "";
            foreach (DataRow row in sdt.Rows )
            {
                if (row["facility"].ToString().Trim() != facilitysave)
                {
                    facname = bissclass.combodisplayitemCodeName("type_code", row["facility"].ToString(), dtfacility, "name");
                    facilitysave = row["facility"].ToString();
                }
                row["accname"] = facname;
            }
            ds.Tables.Add(sdt);
            return true;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            Session["sql"] = "";
            ds = new DataSet();
            sdt = new DataTable();
            if (!getData())
                return;

            Session["rdlcfile"] = "admspaceListing.rdlc";
            string mrptheader = "ADMISSION SPACE AVAILABILITY";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(mrptheader, mrptheader + " AS AT " + DateTime.Now.ToString(), "", "", "", "ADMSPACE", "", 0m, "", "", "", ds, true, 0, DateTime.Now.Date, DateTime.Now.Date, "", isprint, "S", woperator);

                //if (isprint)
                //    paedreports.work();
                //else
                paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(mrptheader, mrptheader + " AS AT " + DateTime.Now.ToString(), "", "", "", "ADMSPACE", "", 0m, "", "", "", ds, 0, DateTime.Now.Date, DateTime.Now.Date, "", isprint, true, "", woperator);
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