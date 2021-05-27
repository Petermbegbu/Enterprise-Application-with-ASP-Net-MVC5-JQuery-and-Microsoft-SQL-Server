using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using msfunc;
using mradmin.BissClass;

//using System.Windows.Forms;

using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;

namespace mradmin.DataAccess
{
    public class ANC01
    {
        public string REFERENCE { get; set; }
        public string PATIENTNO { get; set; }
        public string GROUPHEAD { get; set; }
        public string GROUPCODE { get; set; }
        public string GROUPHTYPE { get; set; }
        public string NAME { get; set; }
        public DateTime LMP { get; set; }
        public DateTime EDD { get; set; }
        public string BLOODGROUP { get; set; }
        public DateTime DEL_DATE { get; set; }
        public decimal CUMMCHARGE { get; set; }
        public decimal PAYMENTS { get; set; }
        public decimal CHARGE { get; set; }
        public DateTime LASTATTEND { get; set; }
        public DateTime NEXTVISIT { get; set; }
        public decimal CUMMATTEND { get; set; }
        public string DRUG1 { get; set; }
        public string DRUG2 { get; set; }
        public string DRUG3 { get; set; }
        public string DRUG4 { get; set; }
        public string DRUG5 { get; set; }
        public string DRUG6 { get; set; }
        public string DRUG7 { get; set; }
        public string DRUG8 { get; set; }
        public string DRUG9 { get; set; }
        public string DRUG10 { get; set; }
        public bool ALLDRUGS { get; set; }
        public string SERVICE1 { get; set; }
        public string SERVICE2 { get; set; }
        public string SERVICE3 { get; set; }
        public string SERVICE4 { get; set; }
        public string SERVICE5 { get; set; }
        public string SERVICE6 { get; set; }
        public string SERVICE7 { get; set; }
        public string SERVICE8 { get; set; }
        public string SERVICE9 { get; set; }
        public string SERVICE10 { get; set; }
        public bool ALLSERVICE { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public DateTime REG_DATE { get; set; }
        public string REG_TIME { get; set; }
        public string DOCTOR { get; set; }
        public string DURATIONOFPREGNANCY { get; set; }
        public decimal AGE { get; set; }
        public string TRIBE { get; set; }
        public string RELIGION { get; set; }
        public string ADDRESS { get; set; }
        public string OCCUPATION { get; set; }
        public decimal LEVELOFEDUCATION { get; set; }
        public string HUSBANDNAME { get; set; }
        public string HUSBANDOCCUPATION { get; set; }
        public string HUSBANDEMPLOYER { get; set; }
        public decimal HUSBANDLEVELOFEDUCATION { get; set; }
        public string HUSBANDPHONE { get; set; }
        public string HUSBANDGC { get; set; }
        public string HUSBANDPATNO { get; set; }
        public string HUSBANDBG { get; set; }
        public string BOOKINGCATEGORY { get; set; }
        public decimal BOOKINGTAG { get; set; }
        public string GHGROUPCODE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public string SPNOTES { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public string GENOTYPE { get; set; }
        public string MENS_REGULARITY { get; set; }
        public string CONTRACEPTIVEUSE { get; set; }
        public string RISKFACTOR { get; set; }
        public string HUSBANDGENOTYPE { get; set; }
        public string MENARCHE { get; set; }

        public static ANC01 GetANC01(string reference)
        {
            ANC01 ac01 = new ANC01();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "ANC01_Get"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Reference", reference);
            //selectCommand.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    ac01.REFERENCE = reader["reference"].ToString();
                    ac01.PATIENTNO = reader["patientno"].ToString();
                    ac01.GROUPHEAD = reader["grouphead"].ToString();
                    ac01.GROUPCODE = reader["groupcode"].ToString();
                    ac01.GROUPHTYPE = reader["grouphtype"].ToString();
                    ac01.NAME = reader["name"].ToString();
                    ac01.LMP = Convert.ToDateTime(reader["lmp"]);
                    ac01.EDD = Convert.ToDateTime(reader["edd"]);
                    ac01.BLOODGROUP = reader["bloodgroup"].ToString();
                    ac01.DEL_DATE = (DateTime)reader["del_date"];
                    ac01.CUMMCHARGE = (Decimal)reader["cummcharge"];
                 //   ac01.PAYMENTS = Convert.ToDecimal(reader["payment"]);
                 //   ac01.CHARGE = Convert.ToDecimal(reader["charge"]);
                    ac01.LASTATTEND = (DateTime)reader["lastattend"];
                    ac01.NEXTVISIT = (DateTime)reader["nextvisit"];
                    ac01.CUMMATTEND = Convert.ToDecimal(reader["cummattend"]);
                    ac01.DRUG1 = reader["drug1"].ToString();
                    ac01.DRUG2 = reader["drug2"].ToString();
                    ac01.DRUG3 = reader["drug3"].ToString();
                    ac01.DRUG4 = reader["drug4"].ToString();
                    ac01.DRUG5 = reader["drug5"].ToString();
                    ac01.DRUG6 = reader["drug6"].ToString();
                    ac01.DRUG7 = reader["drug7"].ToString();
                    ac01.DRUG8 = reader["drug8"].ToString();
                    ac01.DRUG9 = reader["drug9"].ToString();
                    ac01.DRUG10 = reader["drug10"].ToString();
                    ac01.ALLDRUGS = (bool)reader["alldrugs"];
                    ac01.SERVICE1 = reader["service1"].ToString();
                    ac01.SERVICE2 = reader["service2"].ToString();
                    ac01.SERVICE3 = reader["service3"].ToString();
                    ac01.SERVICE4 = reader["service4"].ToString();
                    ac01.SERVICE5 = reader["service5"].ToString();
                    ac01.SERVICE6 = reader["service6"].ToString();
                    ac01.SERVICE7 = reader["service7"].ToString();
                    ac01.SERVICE8 = reader["service8"].ToString();
                    ac01.SERVICE9 = reader["service9"].ToString();
                    ac01.SERVICE10 = reader["service10"].ToString();
                    ac01.ALLSERVICE = (bool)reader["allservice"];
                    ac01.POSTED = (bool)reader["posted"];
                    ac01.POST_DATE = (DateTime)reader["post_date"];
                    ac01.TRANS_DATE = (DateTime)reader["trans_date"];
                    ac01.REG_DATE = (DateTime)reader["reg_date"];
                    ac01.REG_TIME = reader["reg_time"].ToString();
                    ac01.DOCTOR = reader["doctor"].ToString();
                    ac01.DURATIONOFPREGNANCY = reader["durationofpregnancy"].ToString();
                    ac01.AGE = (Decimal)reader["age"];
                    ac01.TRIBE = reader["tribe"].ToString();
                    ac01.RELIGION = reader["religion"].ToString();
                    ac01.ADDRESS = reader["address"].ToString();
                    ac01.OCCUPATION = reader["occupation"].ToString();
                    ac01.LEVELOFEDUCATION = (Decimal)reader["levelofeducation"];
                    ac01.HUSBANDNAME = reader["husbandname"].ToString();
                    ac01.HUSBANDOCCUPATION = reader["husbandoccupation"].ToString();
                    ac01.HUSBANDEMPLOYER = reader["husbandemployer"].ToString();
                    ac01.HUSBANDLEVELOFEDUCATION = (Decimal)reader["husbandlevelofeducation"];
                    ac01.HUSBANDPHONE = reader["husbandphone"].ToString();
                    ac01.HUSBANDGC = reader["husbandgc"].ToString();
                    ac01.HUSBANDPATNO = reader["husbandpatno"].ToString();
                    ac01.HUSBANDBG = reader["husbandbg"].ToString();
                    ac01.BOOKINGCATEGORY = reader["bookingcategory"].ToString();
                    ac01.BOOKINGTAG = (Decimal)reader["bookingtag"];
                    ac01.GHGROUPCODE = reader["GHGROUPCODE"].ToString();
                    ac01.OPERATOR = reader["operator"].ToString();
                    ac01.DTIME = (DateTime)reader["dtime"];
                    ac01.SPNOTES = reader["spnotes"].ToString();
                    ac01.BIRTHDATE = (DateTime)reader["birthdate"];
                    ac01.GENOTYPE = reader["genotype"].ToString();
                    ac01.MENS_REGULARITY = reader["mens_regularity"].ToString();
                    ac01.CONTRACEPTIVEUSE = reader["contraceptiveuse"].ToString();
                    ac01.RISKFACTOR = reader["riskfactor"].ToString();
                    ac01.HUSBANDGENOTYPE = reader["husbandgenotype"].ToString();
                    ac01.MENARCHE = reader["menarche"].ToString();
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
                MessageBox.Show("" + ex, "Get Patient Details ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return ac01;
        }
        /// <summary>
        /// update from anc registration to anc01
        /// </summary>
        /// <param name="newrec"></param>
        /// <param name="ancreg"></param>
        /// <param name="anc01"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static bool anc01Write(bool newrec, ANCREG ancreg, string reference)
        {
            DateTime dtmin_date = msmrfunc.mrGlobals.mta_start;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "ANC01_Addfrmreg" : "ANC01_Updatefrmreg";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@reference", reference);
            insertCommand.Parameters.AddWithValue("@patientno", ancreg.PATIENTNO);
            insertCommand.Parameters.AddWithValue("@grouphead", ancreg.GROUPHEAD);
            insertCommand.Parameters.AddWithValue("@groupcode", ancreg.GROUPCODE);
            insertCommand.Parameters.AddWithValue("@grouphtype", ancreg.GROUPHTYPE);
            insertCommand.Parameters.AddWithValue("@name", ancreg.NAME);
            insertCommand.Parameters.AddWithValue("@lmp", dtmin_date); // ancreg.LMP );
            insertCommand.Parameters.AddWithValue("@edd", dtmin_date); //  ancreg.EDD );
            insertCommand.Parameters.AddWithValue("@BLOODGROUP","");
            insertCommand.Parameters.AddWithValue("@DEL_DATE",  dtmin_date);
            insertCommand.Parameters.AddWithValue("@CUMMCHARGE", 0m);
            insertCommand.Parameters.AddWithValue("@PAYMENTS", 0m);
            insertCommand.Parameters.AddWithValue("@CHARGE", 0m);
            insertCommand.Parameters.AddWithValue("@LASTATTEND", dtmin_date);
            insertCommand.Parameters.AddWithValue("@NEXTVISIT", dtmin_date);
            insertCommand.Parameters.AddWithValue("@CUMMATTEND", 0m);
            insertCommand.Parameters.AddWithValue("@drug1", ancreg.DRUG1);
            insertCommand.Parameters.AddWithValue("@drug2", ancreg.DRUG2);
            insertCommand.Parameters.AddWithValue("@drug3", ancreg.DRUG3);
            insertCommand.Parameters.AddWithValue("@drug4", ancreg.DRUG4);
            insertCommand.Parameters.AddWithValue("@drug5", ancreg.DRUG5);
            insertCommand.Parameters.AddWithValue("@drug6", ancreg.DRUG6);
            insertCommand.Parameters.AddWithValue("@drug7", ancreg.DRUG7);
            insertCommand.Parameters.AddWithValue("@drug8", ancreg.DRUG8);
            insertCommand.Parameters.AddWithValue("@drug9", ancreg.DRUG9);
            insertCommand.Parameters.AddWithValue("@drug10", ancreg.DRUG10);
            insertCommand.Parameters.AddWithValue("@alldrugs", ancreg.ALLDRUGS);
            insertCommand.Parameters.AddWithValue("@service1", ancreg.SERVICE1);
            insertCommand.Parameters.AddWithValue("@service2", ancreg.SERVICE2);
            insertCommand.Parameters.AddWithValue("@service3", ancreg.SERVICE3);
            insertCommand.Parameters.AddWithValue("@service4", ancreg.SERVICE4);
            insertCommand.Parameters.AddWithValue("@service5", ancreg.SERVICE5);
            insertCommand.Parameters.AddWithValue("@service6", ancreg.SERVICE6);
            insertCommand.Parameters.AddWithValue("@service7", ancreg.SERVICE7);
            insertCommand.Parameters.AddWithValue("@service8", ancreg.SERVICE8);
            insertCommand.Parameters.AddWithValue("@service9", ancreg.SERVICE9);
            insertCommand.Parameters.AddWithValue("@service10", ancreg.SERVICE10);
            insertCommand.Parameters.AddWithValue("@allservice", ancreg.ALLSERVICE);
            insertCommand.Parameters.AddWithValue("@posted", false);
            insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@TRANS_DATE", DateTime.Now.Date);
            insertCommand.Parameters.AddWithValue("@REG_DATE", ancreg.TRANS_DATE);
            insertCommand.Parameters.AddWithValue("@REG_TIME", ancreg.REG_TIME);
            insertCommand.Parameters.AddWithValue("@DOCTOR", "");

            insertCommand.Parameters.AddWithValue("@DURATIONOFPREGNANCY", "");
            insertCommand.Parameters.AddWithValue("@AGE", 0m);
            insertCommand.Parameters.AddWithValue("@TRIBE", "");
            insertCommand.Parameters.AddWithValue("@RELIGION", "");
            insertCommand.Parameters.AddWithValue("@ADDRESS","");
            insertCommand.Parameters.AddWithValue("@OCCUPATION", "");
            insertCommand.Parameters.AddWithValue("@LEVELOFEDUCATION", 0m);
            insertCommand.Parameters.AddWithValue("@HUSBANDNAME", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDOCCUPATION", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDEMPLOYER", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDLEVELOFEDUCATION", 0m);
            insertCommand.Parameters.AddWithValue("@HUSBANDPHONE", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDGC", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDPATNO", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDBG", "");
            insertCommand.Parameters.AddWithValue("@BOOKINGCATEGORY", "");
            insertCommand.Parameters.AddWithValue("@BOOKINGTAG", 0m);
            insertCommand.Parameters.AddWithValue("@GHGROUPCODE", ancreg.GHGROUPCODE);
            insertCommand.Parameters.AddWithValue("@OPERATOR", ancreg.OPERATOR);
            insertCommand.Parameters.AddWithValue("@DTIME", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@SPNOTES", "");
            insertCommand.Parameters.AddWithValue("@BIRTHDATE", dtmin_date);
            insertCommand.Parameters.AddWithValue("@GENOTYPE", "");
            insertCommand.Parameters.AddWithValue("@MENS_REGULARITY", "");
            insertCommand.Parameters.AddWithValue("@CONTRACEPTIVEUSE", "");
            insertCommand.Parameters.AddWithValue("@RISKFACTOR", "");
            insertCommand.Parameters.AddWithValue("@HUSBANDGENOTYPE", "");
            insertCommand.Parameters.AddWithValue("@MENARCHE", "");

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL access" + ex, "ANTE-NATAL UPDATE", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        /// <summary>
        /// Update from Ante-Natal Clinic Module
        /// </summary>
        /// <param name="newrec"></param>
        /// <param name="ancreg"></param>
        /// <param name="anc01"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static bool anc01WriteANC(bool newrec, ANC01 anc01, string reference)
        {
            DateTime dtmin_date = msmrfunc.mrGlobals.dtmin_date;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "ANC01_Add" : "ANC01_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

                insertCommand.Parameters.AddWithValue("@reference", reference);
                insertCommand.Parameters.AddWithValue("@patientno", anc01.PATIENTNO);
                insertCommand.Parameters.AddWithValue("@grouphead", anc01.GROUPHEAD);
                insertCommand.Parameters.AddWithValue("@groupcode", anc01.GROUPCODE);
                insertCommand.Parameters.AddWithValue("@grouphtype", anc01.GROUPHTYPE);
                insertCommand.Parameters.AddWithValue("@name", anc01.NAME);
                insertCommand.Parameters.AddWithValue("@lmp", anc01.LMP);
                insertCommand.Parameters.AddWithValue("@edd", anc01.EDD);
                insertCommand.Parameters.AddWithValue("@CUMMCHARGE", anc01.CUMMCHARGE);
                insertCommand.Parameters.AddWithValue("@PAYMENTS", anc01.PAYMENTS);
                insertCommand.Parameters.AddWithValue("@CHARGE", anc01.CHARGE);
                insertCommand.Parameters.AddWithValue("@LASTATTEND", anc01.LASTATTEND);
                insertCommand.Parameters.AddWithValue("@NEXTVISIT", anc01.NEXTVISIT);
                insertCommand.Parameters.AddWithValue("@CUMMATTEND", anc01.CUMMATTEND);
                insertCommand.Parameters.AddWithValue("@drug1", anc01.DRUG1);
                insertCommand.Parameters.AddWithValue("@drug2", anc01.DRUG2);
                insertCommand.Parameters.AddWithValue("@drug3", anc01.DRUG3);
                insertCommand.Parameters.AddWithValue("@drug4", anc01.DRUG4);
                insertCommand.Parameters.AddWithValue("@drug5", anc01.DRUG5);
                insertCommand.Parameters.AddWithValue("@drug6", anc01.DRUG6);
                insertCommand.Parameters.AddWithValue("@drug7", anc01.DRUG7);
                insertCommand.Parameters.AddWithValue("@drug8", anc01.DRUG8);
                insertCommand.Parameters.AddWithValue("@drug9", anc01.DRUG9);
                insertCommand.Parameters.AddWithValue("@drug10", anc01.DRUG10);
                insertCommand.Parameters.AddWithValue("@alldrugs", anc01.ALLDRUGS);
                insertCommand.Parameters.AddWithValue("@service1", anc01.SERVICE1);
                insertCommand.Parameters.AddWithValue("@service2", anc01.SERVICE2);
                insertCommand.Parameters.AddWithValue("@service3", anc01.SERVICE3);
                insertCommand.Parameters.AddWithValue("@service4", anc01.SERVICE4);
                insertCommand.Parameters.AddWithValue("@service5", anc01.SERVICE5);
                insertCommand.Parameters.AddWithValue("@service6", anc01.SERVICE6);
                insertCommand.Parameters.AddWithValue("@service7", anc01.SERVICE7);
                insertCommand.Parameters.AddWithValue("@service8", anc01.SERVICE8);
                insertCommand.Parameters.AddWithValue("@service9", anc01.SERVICE9);
                insertCommand.Parameters.AddWithValue("@service10", anc01.SERVICE10);
                insertCommand.Parameters.AddWithValue("@allservice", anc01.ALLSERVICE);
                insertCommand.Parameters.AddWithValue("@posted", anc01.POSTED );
                insertCommand.Parameters.AddWithValue("@post_date", anc01.POST_DATE );
                insertCommand.Parameters.AddWithValue("@trans_date", anc01.TRANS_DATE);
                insertCommand.Parameters.AddWithValue("@reg_date", anc01.REG_DATE);
                insertCommand.Parameters.AddWithValue("@reg_time", anc01.REG_TIME);
                insertCommand.Parameters.AddWithValue("@doctor", anc01.DOCTOR);
                insertCommand.Parameters.AddWithValue("@durationofpregnancy", anc01.DURATIONOFPREGNANCY);
                insertCommand.Parameters.AddWithValue("@age", anc01.AGE);
                insertCommand.Parameters.AddWithValue("@tribe", anc01.TRIBE);
                insertCommand.Parameters.AddWithValue("@religion", anc01.RELIGION);
                insertCommand.Parameters.AddWithValue("@address", anc01.ADDRESS);
                insertCommand.Parameters.AddWithValue("@occupation", anc01.OCCUPATION);
                insertCommand.Parameters.AddWithValue("@levelofeducation", anc01.LEVELOFEDUCATION);
                insertCommand.Parameters.AddWithValue("@husbandname", anc01.HUSBANDNAME);
                insertCommand.Parameters.AddWithValue("@husbandoccupation", anc01.HUSBANDOCCUPATION);
                insertCommand.Parameters.AddWithValue("@husbandemployer", anc01.HUSBANDEMPLOYER);
                insertCommand.Parameters.AddWithValue("@husbandlevelofeducation", anc01.HUSBANDLEVELOFEDUCATION);
                insertCommand.Parameters.AddWithValue("@husbandphone", anc01.HUSBANDPHONE);
                insertCommand.Parameters.AddWithValue("@husbandgc", anc01.HUSBANDGC);
                insertCommand.Parameters.AddWithValue("@husbandpatno", anc01.HUSBANDPATNO);
                insertCommand.Parameters.AddWithValue("@husbandbg", anc01.HUSBANDBG);
                insertCommand.Parameters.AddWithValue("@bookingcategory", anc01.BOOKINGCATEGORY);
                insertCommand.Parameters.AddWithValue("@bookingtag", anc01.BOOKINGTAG);
                insertCommand.Parameters.AddWithValue("@GHGROUPCODE", anc01.GHGROUPCODE);
                insertCommand.Parameters.AddWithValue("@operator", anc01.OPERATOR);
                insertCommand.Parameters.AddWithValue("@dtime", anc01.DTIME);
                insertCommand.Parameters.AddWithValue("@spnotes", anc01.SPNOTES);
                insertCommand.Parameters.AddWithValue("@birthdate", anc01.BIRTHDATE);
                insertCommand.Parameters.AddWithValue("@genotype", anc01.GENOTYPE);
                insertCommand.Parameters.AddWithValue("@mens_regularity", anc01.MENS_REGULARITY);
                insertCommand.Parameters.AddWithValue("@contraceptiveuse", anc01.CONTRACEPTIVEUSE);
                insertCommand.Parameters.AddWithValue("@riskfactor", anc01.RISKFACTOR);
                insertCommand.Parameters.AddWithValue("@husbandgenotype", anc01.HUSBANDGENOTYPE);
                insertCommand.Parameters.AddWithValue("@menarche", anc01.MENARCHE);
                insertCommand.Parameters.AddWithValue("@bloodgroup", anc01.BLOODGROUP);
                insertCommand.Parameters.AddWithValue("@del_date", anc01.DEL_DATE);
                 
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL access" + ex, "ANTE-NATAL UPDATE", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
    }
}