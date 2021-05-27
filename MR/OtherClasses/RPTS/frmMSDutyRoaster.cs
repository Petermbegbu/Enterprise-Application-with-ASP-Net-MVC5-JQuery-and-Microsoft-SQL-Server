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

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

#endregion

namespace MR.RPTS
{
	public partial class frmMSDutyRoaster : Form
	{
		DataTable dtfacility = Dataaccess.GetAnytable("", "CODES", "SELECT TYPE_CODE, NAME FROM SERVICECENTRECODES order by name", true), dtGRP = Dataaccess.GetAnytable("", "MR", "SELECT name FROM msdutyrgrp order by name", true), dtdutyr, dtNames = Dataaccess.GetAnytable("", "MR", "SELECT name FROM doctors order by name", true);
		int recno, itemsave,mcol,eom;
		DateTime mdate;
		string savedDuty = "", lookupsource, woperator;
		public frmMSDutyRoaster(string xoperator)
		{
			InitializeComponent();
			woperator = xoperator;
		}

		private void frmMSDutyRoaster_Load(object sender, EventArgs e)
		{
			nmrYear.Value = DateTime.Now.Year;
			cboMonth.SelectedIndex = DateTime.Now.Month-1; //.ToString("MMMM");
			initcomboboxes();
			for (int i = 3; i < 34; i++)
			{
				dataGridView1.Columns[i].HeaderText = "  ";
			}
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
		//void initgridcombos(int xrow)
		//{
		//    DataGridViewComboBoxCell cboName = (DataGridViewComboBoxCell)(dataGridView1.Rows[xrow].Cells[1]);
		//    cboName.DataSource = dtNames;
		//    cboName.ValueMember = "name";
		//    cboName.DisplayMember = "Name";
		//    cboName.AutoComplete = true;
		//}
		private void cboMonth_LostFocus(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(cboMonth.Text))
				return;
			string selectmonthindex = (cboMonth.SelectedIndex + 1).ToString();
			mdate = bissclass.ConvertStringToDateTime("01", selectmonthindex, nmrYear.Value.ToString());
			eom = bissclass.monthend(mdate.Month, mdate.Year);
			dataGridView1.Rows.Clear();
			msDayofWeek(); //display day in selected month
		}
		void msDayofWeek()
		{
			int m, y;
			m = cboMonth.SelectedIndex+1;
			y = Convert.ToInt32( nmrYear.Value);
			DateTime md = mdate;
		  //  emonth = bissclass.monthend(m, y);
			//clear previous day id
			for (int i = 3; i <  34; i++)
			{
				dataGridView1.Columns[i].HeaderText = "     ";
				dataGridView1.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
				dataGridView1.Columns[i].HeaderCell.Style.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
			}
			/*        DataGridViewColumn dataGridViewColumn = dataGridView1.Columns[0];
		dataGridViewColumn.HeaderCell.Style.BackColor = Color.Magenta;
		dataGridViewColumn.HeaderCell.Style.ForeColor = Color.Yellow;
			 this.dataGridViewMain.Columns[i].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 14F, FontStyle.Bold);
			 myDataGrid.Columns[0].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);*/
			//create for selected month/year and change color for Weekend
		   // string xspace = "";
			for (int i = 0; i < eom; i++)
			{
			  //  xspace = i < 10 ? "0" : "";
				dataGridView1.Columns[i + 3].HeaderText = (i + 1).ToString() + "-" + md.DayOfWeek.ToString().Substring(0, 2).Trim();
				if (md.DayOfWeek.ToString().Substring(0, 1) == "S")
				{
					dataGridView1.Columns[i + 3].HeaderCell.Style.ForeColor = Color.Red;
					dataGridView1.Columns[i + 3].HeaderCell.Style.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
				}
				md = md.AddDays(1);
			}
		}
		private void cboGroup_LostFocus(object sender, EventArgs e)
		{
			if (nmrYear.Value < 2000 || string.IsNullOrWhiteSpace(cboMonth.Text) || string.IsNullOrWhiteSpace(cboGroup.Text) )
				return;
			if (string.IsNullOrWhiteSpace(dataGridView1.Columns[3].HeaderText))
				msDayofWeek();
			string xfacility = string.IsNullOrWhiteSpace(cboFacility.Text) ? "" : cboFacility.SelectedValue.ToString();
			itemsave = mcol = recno = 0;
			int m, y, emonth;
			m = cboMonth.SelectedIndex+1;
			y = Convert.ToInt32(nmrYear.Value);
			DateTime md = mdate; // bissclass.ConvertStringToDateTime("01", m.ToString(), y.ToString());
			emonth = bissclass.monthend(m, y);
			dtdutyr = Dataaccess.GetAnytable("", "MR", "SELECT * FROM MSDUTYR where facility = '" + xfacility + "' and rgroup = '" + cboGroup.Text + "' and rmonth = '" + (cboMonth.SelectedIndex+1) + "' and ryear = '" + nmrYear.Value + "' order by staff_no", false);
			if (dtdutyr.Rows.Count < 1)
				return;
			dataGridView1.Rows.Clear();
			chkSecureDefinitions.Enabled = true;
			DataGridViewRow drow = null;
			recno = itemsave = 0;
			foreach (DataRow row in dtdutyr.Rows)
			{
				dataGridView1.Rows.Add();
				drow = dataGridView1.Rows[recno];
			  //  initgridcombos(recno);
				recno++;
				drow.Cells[0].Value = row["staff_no"].ToString();
				drow.Cells[1].Value = row["name"].ToString();
				drow.Cells[34].Value = row["RECID"].ToString();
				drow.Cells[35].Value = (bool)row["posted"] ? "POSTED" : "";
				md = mdate;
				for (int i = 0; i < eom; i++)
				{
					drow.Cells[i + 3].Value = row["day" + (i + 1).ToString()].ToString();
					if (md.DayOfWeek.ToString().Substring(0, 1) == "S")
					{
					   // drow.Cells[1 + 1].Style.ForeColor = Color.Red;
					  //  dataGridView1.Columns[i + 3].HeaderCell.Style.ForeColor = Color.Red;
					  //  dataGridView1.Columns[i + 3].HeaderCell.Style.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
						drow.Cells[i + 3].Style.ForeColor = Color.Red;
					}
					md = md.AddDays(1);
				}
				itemsave++;
			}
		}
		bool msDutyCheck(string xval)
		{
			if (string.IsNullOrWhiteSpace(xval))
				return false;
			if (!new string[] { "M", "A", "N", "C", "O", "P", "L", "D", "B" }.Contains(xval))
			{
				DialogResult result = MessageBox.Show("Duty Options are : M>orning;  A>fternoon;  N>ight;  O>Duty Off;  P>Public Holiday; L>Annual Leave; D>Day Shift; B>BI-Shift; C>On Call ", "Duty Roaster Optons");
				return false;
			}
			lblDuty.Text = xval == "M" ? "Morning " : xval == "A" ? "Afternoon" : xval == "N" ? "Night" : xval == "O" ? "Duty/Night Off" : xval == "P" ? "Public Holiday" : xval == "L" ? "Annual Leave" : xval == "D" ? "Day Shift" : xval == "B" ? "BI-Shift" : "On Call ";
			return true;
		}
		private void btnAdd_Click(object sender, EventArgs e)
		{
			DialogResult result;
			if (string.IsNullOrWhiteSpace(cboFacility.Text) && string.IsNullOrWhiteSpace(cboGroup.Text))
			{
				result = MessageBox.Show("Facility or Group must be specified...");
				return;
			}
			if (itemsave > 0)
			{
				int xrow = itemsave - 1;
				if (dataGridView1.Rows[xrow].Cells[1].Value == null ||
					string.IsNullOrWhiteSpace( dataGridView1.Rows[xrow].Cells[1].FormattedValue.ToString()))
				{
					result = MessageBox.Show("Generated record space has not been fully utilized...", "New record Add");
					return;
				}
			}
			DataGridViewRow row = new DataGridViewRow();
			dataGridView1.Rows.Add();
			itemsave++;
			int xrec = itemsave - 1;
			dataGridView1.Rows[xrec].Cells[0].Value = bissclass.autonumconfig(itemsave.ToString(),true,"","9999");
		  //  initgridcombos(xrec);
			dataGridView1.Rows[xrec].Cells[2].Value = "...";
			dataGridView1.Rows[xrec].Cells[1].Selected = true;
			recno = xrec;
			return;
		}
		private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex > 2 && e.ColumnIndex  <= eom + 3)
			{
				DataGridViewRow dgv = new DataGridViewRow();
				DateTime md;
				dgv = dataGridView1.Rows[e.RowIndex];
			 //   nmrAutoCounter.Enabled = false;
				md = mdate.AddDays(e.ColumnIndex - 3);
				if (md.DayOfWeek.ToString().Substring(0, 1) == "S")
				{
					dgv.Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
					// dgv.Cells[e.ColumnIndex].Style.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
				}
			  //  nmrAutoCounter.Enabled = true;
			}
		}
		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
				if (e.ColumnIndex == 1)
					cell.ToolTipText = "You can Type Name of Staff Freely or Click the Lookup Button (...) To Select Name";
				else if (e.ColumnIndex >= 3)
					cell.ToolTipText = "Click to repeat previous Duty ID...or type and Press the TAB key";
				else if (e.ColumnIndex >= 2)
					cell.ToolTipText = "Lookup Button to Staff Names...";
			}
			/*
				DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
				if (!pantransfer.Visible && e.ColumnIndex == dataGridView1.Columns[1].Index)
					cell.ToolTipText = "Select Store/Warehouse";
				else if (e.ColumnIndex == dataGridView1.Columns[2].Index)
					cell.ToolTipText = "Select Product or Click the Lookup Button (...) for Lookup on Defined Products";
				else if (e.ColumnIndex == dataGridView1.Columns[3].Index)
					cell.ToolTipText = "Lookup Button on Defined Products";
				else if (e.ColumnIndex == dataGridView1.Columns[5].Index)
					cell.ToolTipText = "Enter Quantity Required";
				else if (e.ColumnIndex == dataGridView1.Columns[6].Index)
					cell.ToolTipText = "Select Product Unit of Measure"; //Qty of this product the Customer requested for";
				else if (e.ColumnIndex == dataGridView1.Columns[7].Index)
					cell.ToolTipText = "Actual Quanity to Issue/Transfer";
				else if (e.ColumnIndex == dataGridView1.Columns[21].Index && chkrestock.Checked)
					cell.ToolTipText = "'YES' to restock 'NO' No re-stock";
			}*/
		}
		private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && (e.ColumnIndex > 0 && e.ColumnIndex <= (eom+3)))
			{
				DataGridViewRow dgv = new DataGridViewRow();
				recno = e.RowIndex;
				mcol = e.ColumnIndex;
				DateTime md = mdate;
				dgv = dataGridView1.Rows[e.RowIndex];
		   //     nmrAutoCounter.Enabled = false;
				if (e.ColumnIndex == 1 && dgv.Cells[1].Value != null && !string.IsNullOrWhiteSpace( dgv.Cells[1].Value.ToString()))
				{
					btnDelete.Enabled = true;
					//check for duplicate name
						if (CheckNameDuplicate(dgv.Cells[1].Value.ToString(), e.RowIndex))
					{
						dgv.Cells[1].Value = "";
						return;
					}
						dgv.Cells[35].Value = "UPDATED";
				}
				if (e.ColumnIndex > 2 && e.ColumnIndex <= eom+3)
				{
					if (!msDutyCheck(dgv.Cells[e.ColumnIndex].FormattedValue.ToString()))
					{
						dgv.Cells[e.ColumnIndex].Value = "";
						dgv.Cells[e.ColumnIndex].Selected = true;
						return;
					}
					savedDuty = dgv.Cells[e.ColumnIndex].FormattedValue.ToString().Trim(); //stored to be displayed when m-click
					dgv.Cells[35].Value = "UPDATED";
				}
				//md = mdate.AddDays( (e.ColumnIndex - 1));
				//if (md.DayOfWeek.ToString().Substring(0, 1) == "S")
				//{
				//    //dgv.Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
				//    dgv.Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
				//   // dgv.Cells[e.ColumnIndex].Style.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
				//}
			//    nmrAutoCounter.Enabled = true;

			}
		}
		bool CheckNameDuplicate(string xname, int xrow)
		{
			bool foundit = false;
			DataGridViewRow dgv;
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				dgv = dataGridView1.Rows[i];
				if (xrow == i || dgv.Cells[1].Value == null || string.IsNullOrWhiteSpace(dgv.Cells[1].Value.ToString()))
					continue;

				if (dgv.Cells[1].Value.ToString().Trim() == xname )
				{
					DialogResult result = MessageBox.Show("This name is already on the list...", "Duplicate");
					foundit = true;
					break;
				}
			}
			return foundit;
		}
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == 2)
			{
				recno = e.RowIndex;
				lookupsource = "STAFF";
				msmrfunc.mrGlobals.lookupCriteria = "C";
				msmrfunc.mrGlobals.crequired = "STAFF";
				msmrfunc.mrGlobals.frmcaption = "LOOKUP FOR REGISTERED MEDICAL STAFF";

				frmselcode FrmSelCode = new frmselcode();
				FrmSelCode.Closed += new EventHandler(FrmSelCode_Closed);
				FrmSelCode.ShowDialog();
			}
			else if (e.RowIndex >= 0 && e.ColumnIndex >= 3 && e.ColumnIndex <= eom+3 )
			{
				DataGridViewRow dgv = new DataGridViewRow();
				dgv = dataGridView1.Rows[e.RowIndex];
				if (!string.IsNullOrWhiteSpace(savedDuty) && string.IsNullOrWhiteSpace(dgv.Cells[e.ColumnIndex].FormattedValue.ToString()) && e.ColumnIndex < eom+3 )
					dgv.Cells[e.ColumnIndex].Value = savedDuty;
			}
		}
		void FrmSelCode_Closed(object sender, EventArgs e)
		{
			frmselcode FrmSelcode = sender as frmselcode;
			if (lookupsource == "STAFF")
			{
				dataGridView1.Rows[recno].Cells[1].Value = msmrfunc.mrGlobals.anycode1;
				//CHECK IF SELECTED
				if (CheckNameDuplicate(dataGridView1.Rows[recno].Cells[1].Value.ToString(), recno ))
				{
					dataGridView1.Rows[recno].Cells[1].Value = "";
					return;
				}
				dataGridView1.Rows[recno].Cells[35].Value = "UPDATED";
			}
			return;
		}
		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count < 1)
				return;
			if (dataGridView1.Rows[recno].Cells[35].Value != null &&
				dataGridView1.Rows[recno].Cells[35].Value.ToString() == "POSTED")
			{
				DialogResult resultremove = MessageBox.Show("This Record can't be Removed...Its Confirmed !", "");
				return;
			}
			DialogResult result = MessageBox.Show("Delete Record..?", dataGridView1.Rows[recno].Cells[1].FormattedValue.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.No)
				return;

			if (Convert.ToDecimal(dataGridView1.Rows[recno].Cells[34].Value) > 0)
			{
				string updatestring = "DELETE from msdutyr WHERE RECID = '" + dataGridView1.Rows[recno].Cells[34].Value.ToString() + "' ";

				if (bissclass.UpdateRecords(updatestring, "MR"))
				{
					MessageBox.Show("Record Removed...");
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
				dataGridView1.Rows[i].Cells[0].Value = bissclass.autonumconfig((i+1).ToString(), true, "", "9999"); 
				itemsave = i;
			}
			itemsave++;
		}
		void generate_multiple(int xcol, string xvalue)
		{
			int m, y;
			DateTime md = mdate;
			m = cboMonth.SelectedIndex;
			y = Convert.ToInt32( nmrYear.Value);
			int xday = xcol - 2;
		  //  md = md.AddDays(xday);
			int endMultiple = xday + Convert.ToInt32( nmrAutoCounter.Value-1) <= eom ? xday + Convert.ToInt32( nmrAutoCounter.Value-1) : 0;
			if (endMultiple < 1)
				return;
			//em++;
		   // int colcount = xcol;
			md = md.AddDays(xday);
			DataGridViewRow dgv = dataGridView1.Rows[recno];
			for (int i = xday; i <= endMultiple; i++)
			{
				if ((i + 3) > 31)
					break;
				dgv.Cells[i+3].Value = xvalue;
				if (md.DayOfWeek.ToString().Substring(0, 1) == "S")
					dgv.Cells[i+3].Style.ForeColor = Color.Red;
				md = md.AddDays(1);
			   // colcount++;
			}
			nmrAutoCounter.Value = 0m;
			if (endMultiple < eom )
				dataGridView1.Rows[recno].Cells[endMultiple+1].Selected = true;
		}

		private void btnClearDefinitions_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count < 1)
				return;
			DialogResult result = MessageBox.Show("Confirm to Clear All Selections on Screen...","MS Duty Roaster", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.No)
				return;
			dataGridView1.Rows.Clear();
			chkSecureDefinitions.Enabled = btnDelete.Enabled = btnSubmit.Enabled = false;
		}

		private void chkSecureDefinitions_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count < 1)
				return;
			DialogResult result;
			bool iserror = false;
			foreach (DataGridViewRow row in dataGridView1.Rows )
			{
				if (string.IsNullOrWhiteSpace( row.Cells[33].FormattedValue.ToString()))
				{
					result = MessageBox.Show("THERE ARE UN-SAVED RECORD(S) IN THIS GROUP...\r\n All Records must be Submitted before 'SECURING'.  Can't Secure what has not been saved!!!", "MS Duty Roaster");
					iserror = true;
					break;
				}
			}
			if (iserror)
				return;
			result = MessageBox.Show("Confirm to Save Secure All Definitions...\r\n If Secure, further amendment will not be allowed on this Roster \r\n And this Request Cannot be reversed...!\r\n CONTINUE...?", "MS Duty Roaster", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.No)
				return;
			string updatestring = "update msdutyr set posted = @posted, post_date = @post_date, operator = @operator, dtime = @dtime were recid = '",xupd = "";
			SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
			connection.Open();
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				if (row.Cells[34].FormattedValue.ToString() == "POSTED")
					continue;
				xupd = updatestring + row.Cells[34].FormattedValue.ToString().Trim() + "'";

				SqlCommand insertCommand = new SqlCommand();
				insertCommand.CommandText = xupd;
				insertCommand.Connection = connection;

				insertCommand.Parameters.AddWithValue("@posted", true);
				insertCommand.Parameters.AddWithValue("@post_date", DateTime.Now);
				insertCommand.Parameters.AddWithValue("@operator", woperator);
				insertCommand.Parameters.AddWithValue("@dtime", DateTime.Now);

				insertCommand.ExecuteNonQuery();
			}
			connection.Close();
			MessageBox.Show("Secure Completed...", "MS Duty Roaster");
			chkSecureDefinitions.Enabled = false;
		}

		private void btnDefineGroup_Click(object sender, EventArgs e)
		{
			frmMSDutyrGroup grpdef = new frmMSDutyrGroup();
			grpdef.ShowDialog();

			dtGRP = Dataaccess.GetAnytable("", "MR", "SELECT name FROM msdutyrgrp order by name", true);
			cboGroup.DataSource = dtGRP;
			cboGroup.ValueMember = "NAME";
			cboGroup.DisplayMember = "NAME";
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count < 1)
				return;
			DialogResult result;
			if (string.IsNullOrWhiteSpace(cboFacility.Text) && string.IsNullOrWhiteSpace(cboGroup.Text))
			{
				result = MessageBox.Show("Facility or Group must be specified...");
				return;
			}
			result = MessageBox.Show("Confirm to Save All Definitions...", "MS Duty Roaster", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.No)
				return;
			bool newrec;
			SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
			connection.Open();
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				if (row == null || string.IsNullOrWhiteSpace(row.Cells[1].FormattedValue.ToString()) || row.Cells[35].FormattedValue.ToString().Trim() == "POSTED" || row.Cells[35].FormattedValue.ToString().Trim() != "UPDATED")
					continue;
			   // newrec = Convert.ToInt32(row.Cells[33].FormattedValue.ToString().Trim()) > 0 ? true : false;
				newrec = row.Cells[34].Value == null || Convert.ToInt32( row.Cells[34].Value) < 1 ? true : false;

				SqlCommand insertCommand = new SqlCommand();
				insertCommand.CommandText = newrec ? "msdutyr_Add" : "msdutyr_Update";
				insertCommand.Connection = connection;
				insertCommand.CommandType = CommandType.StoredProcedure;

				insertCommand.Parameters.AddWithValue("@STAFF_NO", row.Cells[0].Value.ToString());
				insertCommand.Parameters.AddWithValue("@NAME", row.Cells[1].Value.ToString());
				insertCommand.Parameters.AddWithValue("@FACILITY", string.IsNullOrWhiteSpace(cboFacility.SelectedValue.ToString()) ? "" : cboFacility.SelectedValue.ToString());
				insertCommand.Parameters.AddWithValue("@RMONTH", Convert.ToDecimal( cboMonth.SelectedIndex+1));
				insertCommand.Parameters.AddWithValue("@RYEAR", nmrYear.Value);
				insertCommand.Parameters.AddWithValue("@RGROUP", cboGroup.Text);
				insertCommand.Parameters.AddWithValue("@POSTED", false);
				insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
				insertCommand.Parameters.AddWithValue("@OPERATOR", woperator);
				insertCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);

				for (int i = 3; i < 34; i++)
				{
					insertCommand.Parameters.AddWithValue("@DAY"+(i-2).ToString(), row.Cells[i].Value != null ? row.Cells[i].Value.ToString() : "");
				}
				if (!newrec)
					insertCommand.Parameters.AddWithValue("@RECID", Convert.ToInt32( row.Cells[34].Value));

				insertCommand.ExecuteNonQuery();
			}
			connection.Close();
			MessageBox.Show("Completed...", "MS Duty Roster Update");
			cboGroup_LostFocus(null, null); //reload
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void chkDuplicatePrevRecord_CheckedChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(savedDuty) || nmrAutoCounter.Value < 1)
			{
				MessageBox.Show("Last Duty and Auto Counter must be specified...");
				chkDuplicatePrevRecord.Checked = false;
				return;
			}
			chkDuplicatePrevRecord.Checked = false;
			generate_multiple(mcol, savedDuty);
		}
		private void btnPrint_Click(object sender, EventArgs e)
		{
			frmMsdutyr_Print dutyrprint = new frmMsdutyr_Print(nmrYear.Value, DateTime.Now.Month.ToString(), cboFacility.Text, cboGroup.Text);
			dutyrprint.Show();
		}

		private void btnRenumber_Click(object sender, EventArgs e)
		{
			renumberview();
		}

		private void nmrYear_LostFocus(object sender, EventArgs e)
		{
			cboMonth.Focus();
		}

		private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			//DO NOTHING
		}

		private void cboGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(cboGroup.SelectedItem.ToString()))
				return;
			cboGroup_LostFocus(null, null);
		}

		private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMonth.SelectedItem == null)
				return;
			cboMonth_LostFocus(null, null);
		}



	}
}