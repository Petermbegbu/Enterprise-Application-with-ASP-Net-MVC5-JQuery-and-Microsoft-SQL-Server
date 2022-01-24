#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using msfunc;
using mradmin.DataAccess;
using mradmin.BissClass;

using Gizmox.WebGUI.Common;
using OtherClasses.Models;
//using Gizmox.WebGUI.Forms;

#endregion

namespace OtherClasses.FILE
{
    public partial class frmSpInstnMednotes
    {
        MR_DATA.MR_DATAvm vm;

        //string mgroupcode, mpatientno;
        public frmSpInstnMednotes(MR_DATA.MR_DATAvm VM2)
        {
            vm = VM2;

            //InitializeComponent();
            //this.Text += " [" + name.Trim() + "]";
            //mgroupcode = groupcode; mpatientno = patientno;
        }

        //private void frmSpInstnMednotes_Load(object sender, EventArgs e)
        //{
        //    LoadDetails();
        //}

        //void LoadDetails()
        //{
        //    DataTable dtnotes = Dataaccess.GetAnytable("", "MR", "select SPNOTES, mednotes from billchain where groupcode = '" + mgroupcode + "' and patientno = '" + mpatientno + "'", false);

        //    if (dtnotes.Rows.Count < 1)
        //        return;
        //    txtMednotes.Text = dtnotes.Rows[0]["mednotes"].ToString().Trim();
        //    txtSpInstructions.Text = dtnotes.Rows[0]["spnotes"].ToString().Trim();
        //}

        public MR_DATA.REPORTS btnSubmit_Click(string xSpecInstText, string xMedNoteText, string xmgroupcode, string xmpatientno)
        {
            //if (string.IsNullOrWhiteSpace(txtSpInstructions.Text) && string.IsNullOrWhiteSpace(txtMednotes.Text))
            //    return;

            //DialogResult result = MessageBox.Show("Confirm To Apply Changes...", "Out-Patient To In-patient Bills Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            //if (result == DialogResult.No)
            //    return;

            string updatestring = "update billchain set spnotes = '" + xSpecInstText.Trim() + "', mednotes = '" +
                xMedNoteText.Trim() + "' where groupcode = '" + xmgroupcode + "' and patientno = '" + xmpatientno + "'";

            bissclass.UpdateRecords(updatestring, "MR");
            vm.REPORTS.alertMessage = "Done...";

            return vm.REPORTS;
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}