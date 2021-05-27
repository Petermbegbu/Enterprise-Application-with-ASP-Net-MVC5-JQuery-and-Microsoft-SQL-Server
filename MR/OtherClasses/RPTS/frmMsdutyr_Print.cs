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
    public partial class frmMsdutyr_Print : Form
    {
        DataTable sdt, dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtGRP = Dataaccess.GetAnytable("", "MR", "SELECT name FROM msdutyrgrp order by name", true);
        string mrptheader, rptcriteria = "",  rptfooter = "", sysmodule = bissclass.getRptfooter();
        DataSet ds = new DataSet();
       // int[] dd = new int[31];
        int[] morninga_ = new int[31], afternoona_ = new int[31], nighta_ = new int[31], oncalla_ = new int[31], offa_ = new int[31], dsa_ = new int[31], bsa_ = new int[31];
        int morning, afternoon, night, oncall, off, dss, bis;
        //declare dd[31],daycounta_[5,31]
        public frmMsdutyr_Print(decimal year,string month, string facility, string group)
        {
            InitializeComponent();
            if (year < 1)
            {
                nmrYear.Value = DateTime.Now.Year;
                cboMonth.SelectedItem = DateTime.Now.ToString("MMMM");
            }
            else
            {
                nmrYear.Value = year;
                int xitem = Convert.ToInt32(month)-1;
                cboMonth.SelectedItem = cboMonth.Items[xitem];
               /* bissclass.displaycombo(cboFacility, dtfacility, facility, "type_code");
                cboGroup.SelectedItem = group;*/
            }

        }
        private void frmMsdutyr_Print_Load(object sender, EventArgs e)
        {
            initcomboboxes();
        }
        void initcomboboxes()
        {
            cboFacility.DataSource = dtfacility;
            cboFacility.ValueMember = "Type_code";
            cboFacility.DisplayMember = "NAME";

            cboGroup.DataSource = dtGRP;
            cboGroup.ValueMember = "NAME";
            cboGroup.DisplayMember = "NAME";
        }
        void getData()
        {
            for (int i = 0; i < 31; i++) //arrays to hold summary
            {
                morninga_[i] = afternoona_[i] = nighta_[i] = oncalla_[i] = offa_[i] = dsa_[i] = bsa_[i] = 0;
            }
            int xmonth = cboMonth.SelectedIndex + 1;
            string selstring = "where ryear = '"+nmrYear.Value+"' and rmonth = '"+xmonth+"'";
            if (!string.IsNullOrWhiteSpace(cboFacility.Text))
                selstring += " and facility = '" + cboFacility.SelectedValue.ToString() + "'";
            if (!string.IsNullOrWhiteSpace(cboGroup.Text))
                selstring += " and rtrim(rgroup) = '" + cboGroup.Text.Trim() + "'";
            string xstr = "select STAFF_NO, NAME, FACILITY, RMONTH, RYEAR, RGROUP, DAY1, DAY2, DAY3, DAY4, DAY5, DAY6, DAY7, DAY8, DAY9, DAY10, DAY11, DAY12, DAY13, DAY14, DAY15, DAY16, DAY17, DAY18, DAY19, DAY20, DAY21, DAY22, DAY23, DAY24, DAY25, DAY26, DAY27, DAY28, DAY29, DAY30, DAY31, char(50) as facilityname, 0 AS toton, 0 AS totoff from msdutyr " + selstring;
            if (string.IsNullOrWhiteSpace(cboGroup.Text))
                selstring += "order by rgroup";
            else if (chkAlphabetic.Checked)
                selstring += "order by name";
            else
                selstring += "order by staff_no";

            sdt = Dataaccess.GetAnytable("", "MR", xstr, false);
            //M>orning;  A>fternoon;  N>ight;  O>Duty Off;  P>Public Holiday; L>Annual Leave; C>On Call "
            morning = afternoon = night = oncall = off = 0;
            string shift = "";
            foreach (DataRow row in sdt.Rows)
            {
                if (string.IsNullOrWhiteSpace(cboFacility.Text))
                    row["facilityname"] = bissclass.combodisplayitemCodeName("type_code", row["facility"].ToString(), dtfacility, "name");
                else
                    row["facilityname"] = cboFacility.Text;
                for (int i = 0; i < 31; i++)
                {
                    shift = row["day" + (i + 1).ToString()].ToString();
                    if (shift == "M")
                    {
                        morninga_[i]++;
                        morning++;
                        row["toton"] = (Int32)row["toton"] + 1;
                    }
                    else if (shift == "A")
                    {
                        afternoona_[i]++;
                        row["toton"] = (Int32)row["toton"] + 1;
                        afternoon++;
                    }
                    else if (shift == "N")
                    {
                        nighta_[i]++;
                        row["toton"] = (Int32)row["toton"] + 1;
                        night++;
                    }
                    else if (shift == "C")
                    {
                        oncalla_[i]++;
                        row["toton"] = (Int32)row["toton"] + 1;
                        oncall++;
                    }
                    else if (shift == "O")
                    {
                        offa_[i]++;
                        row["totoff"] = (Int32)row["totoff"] + 1;
                        off++;
                    }
                    else if (shift == "P")
                    {
                        offa_[i]++;
                        row["totoff"] = (Int32)row["totoff"] + 1;
                        off++;
                    }
                    else if (shift == "L")
                    {
                        offa_[i]++;
                        row["totoff"] = (Int32)row["totoff"] + 1;
                        off++;
                    }
                    else if (shift == "D")
                    {
                        dsa_[i]++;
                        row["toton"] = (Int32)row["toton"] + 1;
                        dss++;
                    }
                    else if (shift == "B")
                    {
                         bsa_[i]++;
                        row["toton"] = (Int32)row["toton"] + 1;
                        bis++;
                    }
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void printprocess(bool isprint)
        {
            DialogResult result;
            if (nmrYear.Value < 2000 || string.IsNullOrWhiteSpace(cboMonth.Text))
            {
                result = MessageBox.Show("Invalid Date Specification - Year and Month must be selected...");
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
            if (sdt.Rows.Count < 1)
            {
                result = MessageBox.Show("No Data for Specified Conditions...");
                return;
            }
            //convert int arrarys to string to be passed to report
            string[] dda = new string[31], morningaa_ = new string[31], afternoonaa_ = new string[31], nightaa_ = new string[31], oncallaa_ = new string[31], offaa_ = new string[31], dsaa_ = new string[31], bsaa_ = new string[31];
 
            DateTime md = bissclass.ConvertStringToDateTime("01", (cboMonth.SelectedIndex + 1).ToString(), nmrYear.Value.ToString());
            for (int i = 0; i < 31; i++)
            {
                dda[i] = md.DayOfWeek.ToString().Substring(0, 2).Trim();
                morningaa_[i] = morninga_[i].ToString();
                afternoonaa_[i] = afternoona_[i].ToString();
                nightaa_[i] = nighta_[i].ToString();
                oncallaa_[i] = oncalla_[i].ToString();
                offaa_[i] = offa_[i].ToString();
                dsaa_[i] = dsa_[i].ToString();
                bsaa_[i] = bsa_[i].ToString();
                md = md.AddDays(1);
            }
            md = bissclass.ConvertStringToDateTime("01", (cboMonth.SelectedIndex + 1).ToString(), nmrYear.Value.ToString());


            ds.Tables.Add(sdt);
            Session["sql"] = "";
            Session["dd"] = (string[])dda;
            Session["morninga_"] = (string[])morningaa_;
            Session["afternoona_"] = (string[])afternoonaa_;
            Session["nighta_"] = (string[])nightaa_;
            Session["oncalla_"] = (string[])oncallaa_;
            Session["offa_"] = (string[])offaa_;
            Session["morning"] = morning.ToString();
            Session["afternoon"] = afternoon.ToString();
            Session["night"] = night.ToString();
            Session["oncall"] = oncall.ToString();
            Session["off"] = off.ToString();
            Session["dsa_"] = (string[])dsaa_;
            Session["bsa_"] = (string[])bsaa_;
            Session["ds"] = dss.ToString();
            Session["bis"] = bis.ToString();
            if (!String.IsNullOrWhiteSpace(cboGroup.Text))
                Session["rdlcfile"] = "DutyRoster_Alpha.rdlc";
            else
                Session["rdlcfile"] = "DutyRoster_ALL.rdlc";
            mrptheader = "DUTY ROSTER FOR " + cboMonth.Text + ",   " + nmrYear.Value.ToString();
            mrptheader += chkBySeniority.Checked ? " - BY STAFF NUMBER" : " - ALPHABETICAL BY NAME";
            if (!isprint)
            {
                frmReportViewer paedreports = new frmReportViewer(this.Text, mrptheader, rptfooter, rptcriteria, "", "MSDUTYR", "", 0m, "", "", "", ds, true, 0, DateTime.Now.Date, DateTime.Now.Date, "", isprint, "W", "");

                if (isprint)
                    paedreports.work();
                else
                    paedreports.Show();
            }
            else
            {
                MRrptConversion.GeneralRpt(this.Text, mrptheader, rptfooter, rptcriteria, "", "MSDUTYR", "", 0m, "", "", "", ds, 0, DateTime.Now.Date, DateTime.Now.Date, "", isprint, true, "W", "");
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