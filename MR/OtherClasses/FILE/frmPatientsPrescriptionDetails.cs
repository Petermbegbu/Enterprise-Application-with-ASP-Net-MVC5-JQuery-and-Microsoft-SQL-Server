#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using msfunc;
using msfunc.Forms;

using mradmin.DataAccess;
using mradmin.BissClass;
using mradmin.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmPatientsPrescriptionDetails
    {
        string mgroupcode, mpatientno, mname;
        DateTime datefrom,dateto;

        //public frmPatientsPrescriptionDetails(string groupcode, string patientno, string name, DateTime xdate)
        //{
        //    //InitializeComponent();
        //    mgroupcode = groupcode;
        //    mpatientno = patientno;
        //    mname = name;
        //    datefrom = xdate.AddDays(-90);
        //    dateto = xdate;
        //    this.Text = "PRESCRIPTION DETAILS for :" + mname + " " + datefrom.ToShortDateString() +
        //        " - " + xdate.ToShortDateString()+" [Within 3 months uptill date of this Visit]";
        //}

        //private void frmPatientsPrescriptionDetails_Load(object sender, EventArgs e)
        //{
        //   // DataTable dtdoc = Dataaccess.GetAnytable("", "MR", "SELECT reference, name from doctors", false);
        //    //ListViewItem item1 = new ListViewItem();
        //    //item1.SubItems.Add("Color");
        //    //item1.SubItems[3].ForeColor = System.Drawing.Color.Yellow;
        //    //item1.SubItems[4].ForeColor = System.Drawing.Color.YellowGreen;
        //    //item1.UseItemStyleForSubItems = false;
        //    //listView1.Items.Add(item1);
        //    DataTable dt = Dataaccess.GetAnytable("", "MR", 
        //        "SELECT dispensa.STK_DESC,dispensa.TRANS_DATE,  dispensa.QTY_PR, dispensa.CUMGV, dispensa.CDOSE, " +
        //        "dispensa.CINTERVAL,dispensa.CDURATION, dispensa.DOCTOR, dispensa.NURSE, dispensa.TYPE, dispensa.RX, " +
        //        " dispensa.SP_INST AS RX from dispensa where dispensa.groupcode = '" + mgroupcode + 
        //        "' and dispensa.patientno = '" + mpatientno + "' and dispensa.trans_date >= '" + datefrom.ToShortDateString() +
        //        "' and dispensa.trans_date <= '" + dateto.ToShortDateString() + " 23:59:59:999' UNION SELECT " +
        //        "inpdispensa.STK_DESC, inpdispensa.TRANS_DATE, inpdispensa.QTY_PR, inpdispensa.CUMGV, inpdispensa.CDOSE, " +
        //        "inpdispensa.CINTERVAL,inpdispensa.CDURATION, inpdispensa.DOCTOR, inpdispensa.NURSE, 'I' AS TYPE, " + 
        //        "inpdispensa.RX+' '+inpdispensa.SP_INST AS RX from inpdispensa where inpdispensa.groupcode = '" + 
        //        mgroupcode + "' and inpdispensa.patientno = '" + mpatientno + "' and inpdispensa.trans_date >= '" + 
        //        datefrom.ToShortDateString() + "' and inpdispensa.trans_date <= '" + dateto.ToShortDateString() + 
        //        " 23:59:59:999' ", false);

        //    DialogResult result;
        //    if (dt.Rows.Count < 1)
        //    {
        //        result = MessageBox.Show("No Prescription Record for the period...");
        //        btnClose.PerformClick();
        //    }
        //    //bissclass.LoadListView(dt, listView1);
        //   // listView1.Items[xrow].UseItemStyleForSubItems = false;
        //   // listView1.Items[xrow].SubItems[5].BackColor = Color.Red;
        //    string[] arr = new string[12];
        //    ListViewItem itm;
        //    int xrow = 0;

        //    foreach (DataRow row in dt.Rows )
        //    {
        //        arr[0] = (xrow + 1).ToString();
        //        arr[1] = row["stk_desc"].ToString();
        //        arr[2] = Convert.ToDateTime(row["trans_date"]).ToShortDateString();
        //        arr[3] = row["qty_pr"].ToString();
        //        arr[4] = row["CUMGV"].ToString();
        //        arr[5] = row["cdose"].ToString();
        //        arr[6] = row["cinterval"].ToString();
        //        arr[7] = row["cduration"].ToString();
        //        arr[8] = row["doctor"].ToString();
        //        arr[9] = row["nurse"].ToString();
        //        arr[10] = row["type"].ToString() == "I" ? "In-Patient" : "OPD";
        //        arr[11] = row["rx"].ToString();
        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);
        //        listView1.Items[xrow].UseItemStyleForSubItems = false;
        //        listView1.Items[xrow].SubItems[3].BackColor = Color.Yellow;
        //        listView1.Items[xrow].SubItems[4].BackColor = Color.YellowGreen;
        //        xrow++;
        //    }
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}