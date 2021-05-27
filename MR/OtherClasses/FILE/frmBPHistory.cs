#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using mradmin.BissClass;
using msfunc;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using OtherClasses.Models;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmBPHistory
    {
        //string groupcode, patientno;
        //DataTable dt;

        //public frmBPHistory()
        //{
        //    //InitializeComponent();
        //    //this.Text = "Bp History for : " + xname + " [" + xgroupcode.Trim() + ":" + xpatientno.Trim() + "]";

        //    //groupcode = xgroupcode; patientno = xpatientno;
        //}

        //private void frmBPHistory_Load(object sender, EventArgs e)
        //{
        //    dtDatefrom.Value = DateTime.Now.Date.AddDays(-30);

        //    btnLoad.PerformClick();
        //}

        //void displaydetails(DataTable dt)
        //{
        //    string[] arr = new string[8];
        //    ListViewItem itm;
        //    listView1.Items.Clear();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        arr[0] = row["trans_date"].ToString();
        //        arr[1] = row["BMP"].ToString();
        //        arr[2] = row["BPSITTING"].ToString();
        //        arr[3] = row["temp"].ToString();
        //        arr[4] = row["pulse"].ToString();
        //        arr[5] = row["RESPIRATIO"].ToString();
        //        arr[6] = row["operator"].ToString();
        //        arr[7] = row["opdttime"].ToString();
        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);
        //    }
        //}

        //public MR_DATA.REPORTS btnLoad_Click(object sender, EventArgs e)
        //{
        //    //lblPrompt.Text = "";
        //    dt = Dataaccess.GetAnytable("", "MR", "select * from vstata where groupcode = '" + groupcode + "' and patientno = '" + patientno + "' and trans_date >= '" + dtDatefrom.Value.ToShortDateString() + "' and trans_date <= '" + dtDateto.Value.ToShortDateString() + " 23:59:59:999'", false);
        //    if (dt.Rows.Count < 1)
        //    {
        //        //MessageBox.Show("No Record...");
        //        lblPrompt.Text = "No Record for specified Conditions...";
        //        return;
        //    }
        //    displaydetails(dt);
        //}

        //private void btnGraph_Click(object sender, EventArgs e)
        //{
        //    if (dt.Rows.Count < 1)
        //    {
        //        DialogResult result = MessageBox.Show("No Data...");
        //        return;
        //    }
        //    DataRow row = dt.Rows[0];
        //    DataTable xdt = Dataaccess.GetAnytable("","MR","SELECT NAME FROM BILLCHAIN WHERE GROUPCODE = '"+row["groupcode"].ToString()+"' AND PATIENTNO = '"+row["patientno"].ToString()+"'",false);
        //    frmCharts charts = new frmCharts("OPD", row["groupcode"].ToString(), row["patientno"].ToString(), xdt.Rows[0]["NAME"].ToString(), "",dtDatefrom.Value,dtDateto.Value );
        //    charts.Show();
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}