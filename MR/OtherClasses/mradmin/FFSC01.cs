using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using msfunc;

namespace mradmin.DataAccess
{
    public class FFSC01
    {
        public string PERIOD { get; set; }
        public string REFERENCE { get; set; }
        public string PATIENTNO { get; set; }
        public string GROUPCODE { get; set; }
        public string PATNAME { get; set; }
        public string GROUPHEAD { get; set; }
        public string GRPHEADNAME { get; set; }
        public string STAFFNO { get; set; }
        public string PLANTYPE { get; set; }
        public string SEX { get; set; }
        public string NOTEDATE { get; set; }
        public string ADM_DATE { get; set; }
        public string DISCHARGE { get; set; }
        public string AUTHORITYCODE { get; set; }
        public string DIAGNOSIS { get; set; }
        public decimal ACCOMMODATION { get; set; }
        public decimal ACC_DAYS { get; set; }
        public decimal FEED_DAYS { get; set; }
        public decimal FEEDAMT { get; set; }
        public string LAB { get; set; }
        public string XRAY { get; set; }
        public decimal CONSULTATION { get; set; }
        public string AGE { get; set; }
        public string TRANSTYPE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal LABAMT { get; set; }
        public decimal XRAYAMT { get; set; }
        public string OTHERS { get; set; }
        public decimal OTHERSAMT { get; set; }
        public string ENROLLENO { get; set; }
        public DateTime TREATMENTDATE { get; set; }
        public string PHONE { get; set; }

        public static FFSC01 GetFFSC01(string reference)
        {
            FFSC01 ffsc01 = new FFSC01();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "FFSC01_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    ffsc01.PERIOD = reader["period"].ToString();
                    ffsc01.REFERENCE = reader["reference"].ToString();
                    ffsc01.GROUPCODE = reader["groupcode"].ToString();
                    ffsc01.PATIENTNO = reader["patientno"].ToString();
                    ffsc01.PATNAME = reader["patname"].ToString();
                    ffsc01.GROUPHEAD = reader["grouphead"].ToString();
                    ffsc01.GRPHEADNAME = reader["grpheadname"].ToString();
                    ffsc01.STAFFNO = reader["staffno"].ToString();
                    ffsc01.PLANTYPE = reader["plantype"].ToString();
                    ffsc01.SEX = reader["sex"].ToString();
                    ffsc01.NOTEDATE = reader["notedate"].ToString();
                    ffsc01.ADM_DATE = reader["adm_date"].ToString();
                    ffsc01.DISCHARGE = reader["discharge"].ToString();
                    ffsc01.AUTHORITYCODE = reader["authoritycode"].ToString();
                    ffsc01.DIAGNOSIS = reader["diagnosis"].ToString();
                    ffsc01.ACCOMMODATION = (Decimal)reader["accommodation"];
                    ffsc01.ACC_DAYS = (Decimal)reader["acc_days"];
                    ffsc01.FEED_DAYS = (Decimal)reader["feed_days"];
                    ffsc01.FEEDAMT = (Decimal)reader["feedamt"];
                    ffsc01.LAB = reader["lab"].ToString();
                    ffsc01.XRAY = reader["xray"].ToString();
                    ffsc01.CONSULTATION = (Decimal)reader["consultation"];
                    ffsc01.AGE = reader["age"].ToString();
                    ffsc01.TRANSTYPE = reader["transtype"].ToString();
                    ffsc01.TRANS_DATE = (DateTime)reader["trans_date"];
                    ffsc01.LABAMT = (Decimal)reader["labamt"];
                    ffsc01.OTHERS = reader["others"].ToString();
                    ffsc01.OTHERSAMT = (Decimal)reader["othersamt"];
                    ffsc01.ENROLLENO = reader["enrolleno"].ToString();
                    ffsc01.TREATMENTDATE = (DateTime)reader["treatmentdate"];
                    ffsc01.PHONE = reader["phone"].ToString();
                }
                else
                {
                    connection.Close();
                    return null;

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show("" + ex, "Get Fee for Service Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return ffsc01;
        }
    }
}
//       // insert procedure //
//    void savepatientdetails()
//            {
//                    insertCommand.Parameters.AddWithValue("@period",row.Cells[0].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@reference",row.Cells[1].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@groupcode",row.Cells[2].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@patientno",row.Cells[3].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@patname",row.Cells[4].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@grouphead",row.Cells[5].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@grpheadname",row.Cells[6].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@staffno",row.Cells[7].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@plantype",row.Cells[8].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@sex",row.Cells[9].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@notedate",row.Cells[10].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@adm_date",row.Cells[11].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@discharge",row.Cells[12].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@authoritycode",row.Cells[13].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@diagnosis",row.Cells[14].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@accommodation",row.Cells[15].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@acc_days",row.Cells[16].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@feed_days",row.Cells[17].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@feedamt",row.Cells[18].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@lab",row.Cells[19].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@xray",row.Cells[20].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@consultation",row.Cells[21].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@age",row.Cells[22].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@transtype",row.Cells[23].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@trans_date",row.Cells[24].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@labamt",row.Cells[25].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@others",row.Cells[26].Value.ToString());
//                    insertCommand.Parameters.AddWithValue("@othersamt",row.Cells[27].Value.ToString());
                                          
//}
 


