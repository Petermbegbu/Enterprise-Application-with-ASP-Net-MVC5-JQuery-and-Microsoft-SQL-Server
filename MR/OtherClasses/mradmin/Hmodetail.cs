using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
//using System.Windows.Forms;
using msfunc;

namespace mradmin.DataAccess
{
    public class Hmodetail
    {
        public string CUSTNO { get; set; }
        public string HMOSERVTYPE { get; set; }
        public string DESCRIPTION { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime DATE_REG { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public decimal LASTNUMB { get; set; }
        public string REMARK { get; set; }
        public decimal CAPAMT { get; set; }
        public decimal ACCFEEDING { get; set; }
        public decimal VISALLOWED { get; set; }
        public decimal CUMVISITS { get; set; }
        public decimal TOTBENEFIC { get; set; }
        public decimal FAMUNIT { get; set; }
        public bool SERV_ITEMS { get; set; }
        public bool DRGRESTRICTIVE { get; set; }
        public bool PROCRESTRICTIVE { get; set; }
        public bool DRGINCLUSIVE { get; set; }
        public bool PROCINCLUSIVE { get; set; }
        public decimal CONSULTATION { get; set; }
        public decimal ADMISSIONS { get; set; }
        public decimal FEEDING { get; set; }
        public int RECID { get; set; }

        public static Hmodetail GetHMODETAIL(string xcustno, string xplantype)
        {
            Hmodetail hmodetail = new Hmodetail();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "HMODETAIL_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@custno", xcustno);
            selectCommand.Parameters.AddWithValue("@hmoservtype", xplantype);
            
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    hmodetail.CUSTNO = reader["custno"].ToString();
                    hmodetail.HMOSERVTYPE = reader["hmoservtype"].ToString();
                    hmodetail.DESCRIPTION = reader["description"].ToString();
                    hmodetail.POSTED = (bool)reader["posted"];
                    hmodetail.POST_DATE = (DateTime)reader["post_date"];
                    hmodetail.DATE_REG = (DateTime)reader["date_reg"];
                    hmodetail.TRANS_DATE = (DateTime)reader["trans_date"];
                    hmodetail.LASTNUMB = (Decimal)reader["lastnumb"];
                    hmodetail.REMARK = reader["remark"].ToString();
                    hmodetail.CAPAMT = (Decimal)reader["capamt"];
                    hmodetail.ACCFEEDING = (Decimal)reader["accfeeding"];
                    hmodetail.VISALLOWED = (Decimal)reader["visallowed"];
                    hmodetail.CUMVISITS = (Decimal)reader["cumvisits"];
                    hmodetail.TOTBENEFIC = (Decimal)reader["totbenefic"];
                    hmodetail.FAMUNIT = (Decimal)reader["famunit"];
                    hmodetail.SERV_ITEMS = (bool)reader["serv_items"];
                    hmodetail.DRGRESTRICTIVE = (bool)reader["drgrestrictive"];
                    hmodetail.PROCRESTRICTIVE = (bool)reader["procrestrictive"];
                    hmodetail.DRGINCLUSIVE = (bool)reader["drginclusive"];
                    hmodetail.PROCINCLUSIVE = (bool)reader["procinclusive"];
                    hmodetail.CONSULTATION = (Decimal)reader["consultation"];
                    hmodetail.ADMISSIONS = (Decimal)reader["admissions"];
                    hmodetail.FEEDING = (Decimal)reader["feeding"];
                    hmodetail.RECID = (Int32)reader["RECID"];

                }

                reader.Close();

                connection.Close();
            
            return hmodetail;
        }
        public static DataTable GetHMODETAIL(string xcustno)
        {
            Hmodetail hmodetail = new Hmodetail();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "HMODETAIL_Getlist"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@custno", xcustno);

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
    }
}