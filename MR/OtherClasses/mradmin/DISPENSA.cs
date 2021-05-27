using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using msfunc;

namespace mradmin.DataAccess
{
    public class DISPENSA
    {
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public DateTime DATE { get; set; }
        public string STK_ITEM { get; set; }
        public string STK_DESC { get; set; }
        public string STORE { get; set; }
        public decimal QTY_PR { get; set; }
        public decimal QTY_GV { get; set; }
        public decimal CUMGV { get; set; }
        public decimal DOSE { get; set; }
        public decimal INTERVAL { get; set; }
        public decimal DURATION { get; set; }
        public string UNIT { get; set; }
        public decimal COST { get; set; }
        public string NURSE { get; set; }
        public string DOCTOR { get; set; }
        public string DIAG { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public decimal ITEMNO { get; set; }
        public string NAME { get; set; }
        public decimal STKBAL { get; set; }
        public string TIME { get; set; }
        public string TYPE { get; set; }
        public string GROUPHEAD { get; set; }
        public string GHGROUPCODE { get; set; }
        public string GROUPHTYPE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime OP_TIME { get; set; }
        public decimal UNITCOST { get; set; }
        public string RX { get; set; }
        public string SP_INST { get; set; }
        public string CDOSE { get; set; }
        public string CINTERVAL { get; set; }
        public string CDURATION { get; set; }
        public bool CAPITATED { get; set; }
        public string REFERENCE { get; set; }
        public decimal UNITPURVALUE { get; set; }

        public static DataTable GetDISPENSA(string GroupCodeID, string PatientID, DateTime xtrans_date,bool xinpatient)
        {
            DISPENSA dispen = new DISPENSA();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = (xinpatient) ? "INPDISPENSA_GetPM" : "DISPENSA_GetPM"; //for Presc Management
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@GroupCode", GroupCodeID);
            selectCommand.Parameters.AddWithValue("@Patientno", PatientID);
            selectCommand.Parameters.AddWithValue("@trans_date", xtrans_date);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
        /// <summary>
        /// type = S for store C for cost centre I for impatient transfer
        /// </summary>
        /// <param name="newrec"></param>
        /// <param name="itemno"></param>
        /// <param name="stkitem"></param>
        /// <param name="stkdesc"></param>
        /// <param name="unitcost"></param>
        /// <param name="qtypr"></param>
        /// <param name="qty_gv"></param>
        /// <param name="cumgv"></param>
        /// <param name="cost"></param>
        /// <param name="cdose"></param>
        /// <param name="cinterval"></param>
        /// <param name="cduration"></param>
        /// <param name="rx"></param>
        /// <param name="unit"></param>
        /// <param name="spinst"></param>
        /// <param name="duration"></param>
        /// <param name="dose"></param>
        /// <param name="capited"></param>
        /// <param name="Reference"></param>
        /// <param name="name"></param>
        /// <param name="groupcode"></param>
        /// <param name="patientno"></param>
        /// <param name="doctor"></param>
        /// <param name="trans_date"></param>
        /// <param name="store"></param>
        /// <param name="givenby"></param>
        /// <param name="diagnosis"></param>
        /// <param name="posted"></param>
        /// <param name="postdate"></param>
        /// <param name="stkbal"></param>
        /// <param name="time"></param>
        /// <param name="type"></param>
        /// <param name="grouphead"></param>
        /// <param name="groupheadcode"></param>
        /// <param name="grouphtype"></param>
        /// <param name="xoperator"></param>
        /// <param name="optime"></param>
        public static void WriteDispensa(bool newrec, decimal itemno, string stkitem, string stkdesc, decimal unitcost, decimal qtypr, decimal qty_gv,decimal cumgv, decimal cost,string cdose,string cinterval,string cduration,string rx,string unit,string spinst,decimal duration, decimal dose,decimal capited, string Reference, string name, string groupcode,string patientno, string doctor, DateTime trans_date, string store, string givenby, string diagnosis, bool posted,DateTime postdate, decimal stkbal, string time, string type, string grouphead, string groupheadcode, string grouphtype, string xoperator, DateTime optime, decimal unitpurvalue, decimal xinterval, int recid)
        {
          //  DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "Dispensa_Add" : "Dispensa_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", patientno);
            insertCommand.Parameters.AddWithValue("@Groupcode", groupcode);
            insertCommand.Parameters.AddWithValue("@trans_date", trans_date);
            insertCommand.Parameters.AddWithValue("@stk_item", stkitem);
            insertCommand.Parameters.AddWithValue("@stk_desc", stkdesc);
            insertCommand.Parameters.AddWithValue("@store", store);
            insertCommand.Parameters.AddWithValue("@qty_pr", qtypr);
            insertCommand.Parameters.AddWithValue("@qty_gv", qty_gv);
            insertCommand.Parameters.AddWithValue("@cumgv", cumgv);
            insertCommand.Parameters.AddWithValue("@dose", dose);
            insertCommand.Parameters.AddWithValue("@INTERVAL", xinterval);
            insertCommand.Parameters.AddWithValue("@duration", duration);
            insertCommand.Parameters.AddWithValue("@unit", unit);
            insertCommand.Parameters.AddWithValue("@cost", cost);
            insertCommand.Parameters.AddWithValue("@nurse", givenby);
            insertCommand.Parameters.AddWithValue("@Doctor", doctor);
            insertCommand.Parameters.AddWithValue("@diag", diagnosis);
            insertCommand.Parameters.AddWithValue("@posted", posted);
            insertCommand.Parameters.AddWithValue("@post_date", postdate);
            insertCommand.Parameters.AddWithValue("@itemno", itemno);
            insertCommand.Parameters.AddWithValue("@Name", name);
            insertCommand.Parameters.AddWithValue("@stkbal", stkbal);
            insertCommand.Parameters.AddWithValue("@time", time);
            insertCommand.Parameters.AddWithValue("@TYPE", type);
            insertCommand.Parameters.AddWithValue("@grouphead", grouphead);
            insertCommand.Parameters.AddWithValue("@GHGROUPCODE", groupheadcode);
            insertCommand.Parameters.AddWithValue("@grouphtype", grouphtype);
            insertCommand.Parameters.AddWithValue("@operator", xoperator);
            insertCommand.Parameters.AddWithValue("@op_time", optime);
            insertCommand.Parameters.AddWithValue("@unitcost", unitcost);
            insertCommand.Parameters.AddWithValue("@rx", rx);
            insertCommand.Parameters.AddWithValue("@sp_inst", spinst);
            insertCommand.Parameters.AddWithValue("@cdose", cdose);  
            insertCommand.Parameters.AddWithValue("@cinterval", cinterval);
            insertCommand.Parameters.AddWithValue("@cduration", cduration);
            insertCommand.Parameters.AddWithValue("@capitated", capited);
            insertCommand.Parameters.AddWithValue("@Reference", Reference);
            insertCommand.Parameters.AddWithValue("@unitpurvalue", unitpurvalue);
            if (!newrec)
                insertCommand.Parameters.AddWithValue("@RECID", recid);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                 MessageBox.Show("SQL access" + ex, "Prescriptions Update", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool DeletePrescription( bool xinpatient, int recid)
        {
            SqlConnection connection = Dataaccess.mrConnection();
            SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.CommandText = (xinpatient) ?  "INPDISPENSA_Delete" : "DISPENSA_Delete" ;
            deleteCommand.Connection = connection;
            deleteCommand.CommandType = CommandType.StoredProcedure;

            deleteCommand.Parameters.AddWithValue("@recid", recid);
            try
            {
                connection.Open();
                int count = deleteCommand.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException ex)
            {
                //throw ex;

                MessageBox.Show("Unable to Open SQL Server Database Table" + ex);
                return false;
            }
            finally
            {
                connection.Close();
            }

        }

    }
}