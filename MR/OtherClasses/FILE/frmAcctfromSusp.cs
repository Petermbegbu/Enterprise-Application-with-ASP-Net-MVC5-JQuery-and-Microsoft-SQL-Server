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

using mradmin.Forms;
using mradmin.DataAccess;
using mradmin.BissClass;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmAcctfromSusp
    {
        DataTable attaged;
        public frmAcctfromSusp(DataTable dtattaged, string patientprofile)
        {
            //InitializeComponent();
            //attaged = dtattaged;
            //this.Text = this.Text + " - " + patientprofile;
            //paintHeader();
        }

        private void frmAcctfromSusp_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < 15; i++) //reinit array to hold tagged procedures
            {
                msmrfunc.mrGlobals.taggedFromSuspensea_[i] = "NO";
            }
            //loadgrid(attaged);
        }

        //void loadgrid(DataTable suspense)
        //{
        //    // DataTable dt = Dataaccess.GetAnytable("", "MR", "SELECT NAME, TYPE_CODE FROM SERVICECENTRECODES", false);
        //    decimal xduration;
        //    string xDuration;
        //    string[] arr = new string[12];
        //    ListViewItem itm;
        //    //txtcurrenttotal.Text = "0";
        //    listView1.Items.Clear();
        //    for (int i = 0; i < suspense.Rows.Count; i++)
        //    {
        //        // txtcurrenttotal.Text = (Convert.ToDecimal(txtcurrenttotal.Text) + Convert.ToDecimal(suspense.Rows[i]["amount"])).ToString();
        //        xduration = Convert.ToDecimal(suspense.Rows[i]["duration"]);
        //        xDuration = (xduration >= 1m) ? " x " + xduration.ToString() : "";

        //        arr[0] = suspense.Rows[i]["DESCRIPTION"].ToString() + xDuration;
        //        arr[1] = suspense.Rows[i]["amount"].ToString();
        //        arr[2] = suspense.Rows[i]["facility"].ToString();
        //        arr[3] = suspense.Rows[i]["process"].ToString();
        //        arr[4] = suspense.Rows[i]["trans_date"].ToString();
        //        arr[5] = "NO";
        //        arr[6] = suspense.Rows[i]["notes"].ToString().Trim();
        //        arr[7] = suspense.Rows[i]["billprocess"].ToString();
        //     //   arr[8] = suspense.Rows[i]["facilityname"].ToString();
        //        arr[9] = (Convert.ToBoolean(suspense.Rows[i]["groupeditem"])) ? "YES" : "NO"; 
        //        arr[10] = (Convert.ToBoolean(suspense.Rows[i]["grpbillbyservtype"])) ? "YES" : "NO";
        //        arr[11] = suspense.Rows[i]["itemno"].ToString();

        //        itm = new ListViewItem(arr);
        //        listView1.Items.Add(itm);
        //    }
        //    nmrTagged.Value = 0;
        //}

        //private void btnselect_Click(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < listView1.Items.Count; i++)
        //    {
        //        if (listView1.Items[i].Selected)
        //        {
        //            if (listView1.Items[i].SubItems[5].ToString().Trim() == "YES")
        //            {
        //                nmrTagged.Value = nmrTagged.Value - 1;
        //                listView1.Items[i].SubItems[5].Text = "NO";
        //                msmrfunc.mrGlobals.taggedFromSuspensea_[i] = "NO";
        //            }
        //            else
        //            {
        //                nmrTagged.Value++;
        //                listView1.Items[i].SubItems[5].Text = "YES";
        //                msmrfunc.mrGlobals.taggedFromSuspensea_[i] = "YES";
        //            }
        //            listView1.Show();
        //        }
        //    }
        //}

        //private void btnclose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}


    }
}
 