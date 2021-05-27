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
    public class patientinfo
    {
        /*      private string patientno,groupcode,patstatus,address1,address2,patstate,sex,m_status,contact,ghgroupcode,pattype,bill_cir,
                  name,grouphead,grouphtype,title,patcateg,remark,hmoservtype,currency,clinic,surname,othername,homephone,
                  workphone,employer,emp_name,emp_addr,emp_state,cont_designation, 
                  pr_doc,refer_dr,nationality,occupation,religion,bloodgroup,genotype,nextofkin,nok_adr1,nok_adr2, 
                  nok_state,nok_phone,nok_relationship,rhd,email; 
              private DateTime birthdate,reg_date,trans_date,post_date,laststatmt;
              private decimal cr_limit,upcur_db,upcur_cr,cur_db,cur_cr,balbf,debit1,credit1,balbf1,
                  debit2,credit2,balbf2,debit3,credit3,balbf3,debit4,credit4,balbf4,debit5,credit5,balbf5,debit6,credit6,
                  balbf6,debit7,credit7,balbf7,debit8,credit8,balbf8,debit9,credit9,balbf9,debit10,credit10,balbf10, 
                  debit11,credit11,balbf11,debit12,credit12,balbf12,sec_level,discount;
              private bool posted,billregistration,isgrouphead;*/
        public patientinfo() { }

        public string patientno { get; set; }
        public string groupcode { get; set; }
        public string patstatus { get; set; }
        public string address1 { get; set; }
        public string patstate { get; set; }
        public DateTime birthdate { get; set; }
        public string sex { get; set; }
        public string m_status { get; set; }
        public DateTime reg_date { get; set; }
        public string contact { get; set; }
        public string ghgroupcode { get; set; }
        public string pattype { get; set; }
        public decimal cr_limit { get; set; }
        public string bill_cir { get; set; }
        public decimal upcur_db { get; set; }
        public decimal upcur_cr { get; set; }
        public decimal cur_db { get; set; }
        public decimal cur_cr { get; set; }
        public decimal balbf { get; set; }
        public decimal balbf1 { get; set; }
        public decimal credit1 { get; set; }
        public decimal debit1 { get; set; }
        public decimal balbf2 { get; set; }
        public decimal credit2 { get; set; }
        public decimal debit2 { get; set; }
        public decimal balbf3 { get; set; }
        public decimal credit3 { get; set; }
        public decimal debit3 { get; set; }
        public decimal balbf4 { get; set; }
        public decimal credit4 { get; set; }
        public decimal debit4 { get; set; }
        public decimal balbf5 { get; set; }
        public decimal credit5 { get; set; }
        public decimal debit5 { get; set; }
        public decimal balbf6 { get; set; }
        public decimal credit6 { get; set; }
        public decimal debit6 { get; set; }
        public decimal balbf7 { get; set; }
        public decimal credit7 { get; set; }
        public decimal debit7 { get; set; }
        public decimal balbf8 { get; set; }
        public decimal credit8 { get; set; }
        public decimal debit8 { get; set; }
        public decimal balbf9 { get; set; }
        public decimal credit9 { get; set; }
        public decimal debit9 { get; set; }
        public decimal balbf10 { get; set; }
        public decimal credit10 { get; set; }
        public decimal debit10 { get; set; }
        public decimal balbf11 { get; set; }
        public decimal credit11 { get; set; }
        public decimal debit11 { get; set; }
        public decimal balbf12 { get; set; }
        public decimal credit12 { get; set; }
        public decimal debit12 { get; set; }
        public DateTime trans_date { get; set; }
        public Boolean posted { get; set; }
        public DateTime post_date { get; set; }
        public string name { get; set; }
        public string grouphead { get; set; }
        public string grouphtype { get; set; }
        public Boolean isgrouphead { get; set; }
        public DateTime laststatmt { get; set; }
        public string title { get; set; }
        public string patcateg { get; set; }
        public string remark { get; set; }
        public decimal discount { get; set; }
        public string hmoservtype { get; set; }
        public string currency { get; set; }
        public Boolean billregistration { get; set; }
        public string clinic { get; set; }
        public string surname { get; set; }
        public string othername { get; set; }
        public string homephone { get; set; }
        public string workphone { get; set; }
        public string employer { get; set; }
        public string emp_name { get; set; }
        public string emp_addr { get; set; }
        public string emp_state { get; set; }
        public string cont_designation { get; set; }
        public string pr_doc { get; set; }
        public string refer_dr { get; set; }
        public string nationality { get; set; }
        public string occupation { get; set; }
        public string religion { get; set; }
        public string bloodgroup { get; set; }
        public string genotype { get; set; }
        public string nextofkin { get; set; }
        public string nok_adr1 { get; set; }
        public string nok_adr2 { get; set; }
        public string nok_state { get; set; }
        public string nok_phone { get; set; }
        public string nok_relationship { get; set; }
        public string rhd { get; set; }
        public string email { get; set; }
        public string piclocation { get; set; }


        public static patientinfo GetPatient(string PatientNo,string groupcode)
         {
            patientinfo patients = new patientinfo();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "PATIENT_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@PatientNo", PatientNo);
            selectCommand.Parameters.AddWithValue("@GroupCode", groupcode);
 
            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {

                    patients.groupcode = reader["groupcode"].ToString();
                    patients.patientno = reader["patientno"].ToString();
                    patients.patstatus = reader["patstatus"].ToString();
                    patients.address1 = reader["address1"].ToString();
                    patients.patstate = reader["patstate"].ToString();
                    patients.birthdate = (DateTime)reader["birthdate"];
                    patients.sex = reader["sex"].ToString();
                    patients.m_status = reader["m_status"].ToString();
                    patients.reg_date = (DateTime)reader["reg_date"];
                    patients.contact = reader["contact"].ToString();
                    patients.ghgroupcode = reader["ghgroupcode"].ToString();
                    patients.pattype = reader["pattype"].ToString();
                    patients.cr_limit = (Decimal)reader["cr_limit"];
                    patients.bill_cir = reader["bill_cir"].ToString();
                    patients.upcur_db = (Decimal)reader["upcur_db"];
                    patients.upcur_cr = (Decimal)reader["upcur_cr"];
                    patients.cur_db = (Decimal)reader["cur_db"];
                    patients.cur_cr = (Decimal)reader["cur_cr"];
                    patients.balbf = (Decimal)reader["balbf"];
                    patients.trans_date = (DateTime)reader["trans_date"];
                    patients.posted = (bool)reader["posted"];
                   // patients.post_date = (DateTime)reader["post_date"];
                    patients.name = reader["name"].ToString();
                    patients.grouphead = reader["grouphead"].ToString();
                    patients.grouphtype = reader["grouphtype"].ToString();
                    patients.isgrouphead = (bool)reader["isgrouphead"];

                 //   patients.laststatmt = (DateTime)reader["laststatmt"];
                    patients.title = reader["title"].ToString();
                    patients.patcateg = reader["patcateg"].ToString();
                    patients.remark = reader["remark"].ToString();
                    patients.discount = (Decimal)reader["discount"];
                    patients.hmoservtype = reader["hmoservtype"].ToString();
                    patients.currency = reader["currency"].ToString();
                    patients.billregistration = (bool)reader["billregistration"];
                    patients.clinic = reader["clinic"].ToString();
                    patients.surname = reader["surname"].ToString();
                    patients.othername = reader["othername"].ToString();
                    patients.homephone = reader["homephone"].ToString();
                    patients.workphone = reader["workphone"].ToString();
                    patients.employer = reader["employer"].ToString();
                    patients.emp_name = reader["emp_name"].ToString();
                    patients.emp_addr = reader["emp_addr"].ToString();
                    patients.emp_state = reader["emp_state"].ToString();
                    patients.cont_designation = reader["cont_designation"].ToString();
                    patients.pr_doc = reader["pr_doc"].ToString();
                    patients.refer_dr = reader["refer_dr"].ToString();
                    patients.nationality = reader["nationality"].ToString();
                    patients.occupation = reader["occupation"].ToString();
                    patients.religion = reader["religion"].ToString();
                    patients.bloodgroup = reader["bloodgroup"].ToString();
                    patients.genotype = reader["genotype"].ToString();
                    patients.nextofkin = reader["nextofkin"].ToString();
                    patients.nok_adr1 = reader["nok_adr1"].ToString();
                    patients.nok_state = reader["nok_state"].ToString();
                    patients.nok_phone = reader["nok_phone"].ToString();
                    patients.nok_relationship = reader["nok_relationship"].ToString();
               //     patients.rhd = reader["rhd"].ToString();
                    patients.email = reader["email"].ToString();
                    patients.piclocation = reader["piclocation"].ToString();

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

            return patients;
        }

/*        public static bool AddPatients(string GroupCodeID, string PatientID )
        {
            patientinfo patients = new patientinfo();
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "Patient_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Patientno", patients.patientno);
            insertCommand.Parameters.AddWithValue("@groupcode", patients.groupcode);
            insertCommand.Parameters.AddWithValue("@patstatus", patients.patstatus);
            insertCommand.Parameters.AddWithValue("@Address1", patients.address1);
            insertCommand.Parameters.AddWithValue("@Address2", patients.address2);
            insertCommand.Parameters.AddWithValue("@Patstate", patients.patstate);
            insertCommand.Parameters.AddWithValue("@Birthdate", patients.birthdate);
            insertCommand.Parameters.AddWithValue("@Sex", patients.sex);
            insertCommand.Parameters.AddWithValue("@M_status", patients.m_status);
            insertCommand.Parameters.AddWithValue("@Reg_date", patients.reg_date);
            insertCommand.Parameters.AddWithValue("@contact", patients.contact);
            insertCommand.Parameters.AddWithValue("@ghgroupcode", patients.ghgroupcode);
            insertCommand.Parameters.AddWithValue("@pattype", patients.pattype);
            insertCommand.Parameters.AddWithValue("@cr_limit", patients.cr_limit);
            insertCommand.Parameters.AddWithValue("@bill_cir", patients.cr_limit);
            insertCommand.Parameters.AddWithValue("@Birthdate", patients.birthdate);
            insertCommand.Parameters.AddWithValue("@cur_db", patients.cur_db);
            insertCommand.Parameters.AddWithValue("@cur_cr", patients.cur_cr);
            insertCommand.Parameters.AddWithValue("@balbf", patients.balbf);
            insertCommand.Parameters.AddWithValue("@trans_date", patients.trans_date);
            insertCommand.Parameters.AddWithValue("@name", patients.name);
            insertCommand.Parameters.AddWithValue("@grouphead", patients.grouphead);
            insertCommand.Parameters.AddWithValue("@grouphtype", patients.grouphtype);
            insertCommand.Parameters.AddWithValue("@isgrouphead", patients.isgrouphead);
            insertCommand.Parameters.AddWithValue("@title", patients.title);
            insertCommand.Parameters.AddWithValue("@patcateg", patients.patcateg);
            insertCommand.Parameters.AddWithValue("@remark", patients.remark);
            insertCommand.Parameters.AddWithValue("@discount", patients.discount);
            insertCommand.Parameters.AddWithValue("@hmoservtype", patients.hmoservtype);
            insertCommand.Parameters.AddWithValue("@currency", patients.currency);
            insertCommand.Parameters.AddWithValue("@billregistration", patients.billregistration);
            insertCommand.Parameters.AddWithValue("@clinic", patients.clinic);
            insertCommand.Parameters.AddWithValue("@surname", patients.surname);
            insertCommand.Parameters.AddWithValue("@othername", patients.othername);
            insertCommand.Parameters.AddWithValue("@homephone", patients.homephone);
            insertCommand.Parameters.AddWithValue("@workphone", patients.workphone);
            insertCommand.Parameters.AddWithValue("@employer", patients.employer);
            insertCommand.Parameters.AddWithValue("@emp_name", patients.emp_name);
            insertCommand.Parameters.AddWithValue("@emp_addr", patients.emp_addr);
            insertCommand.Parameters.AddWithValue("@emp_state", patients.emp_state);
            insertCommand.Parameters.AddWithValue("@cont_designation", patients.cont_designation);
            insertCommand.Parameters.AddWithValue("@pr_doc", patients.pr_doc);
            insertCommand.Parameters.AddWithValue("@refer_dr", patients.refer_dr);
            insertCommand.Parameters.AddWithValue("@nationality", patients.nationality);
            insertCommand.Parameters.AddWithValue("@occupation", patients.occupation);
            insertCommand.Parameters.AddWithValue("@religion", patients.religion);
            insertCommand.Parameters.AddWithValue("@bloodgroup", patients.bloodgroup);
            insertCommand.Parameters.AddWithValue("@genotype", patients.genotype);
            insertCommand.Parameters.AddWithValue("@nextofkin", patients.nextofkin);
            insertCommand.Parameters.AddWithValue("@nok_adr1", patients.nok_adr1);
            insertCommand.Parameters.AddWithValue("@nok_adr2", patients.nok_adr2);
            insertCommand.Parameters.AddWithValue("@nok_state", patients.nok_state);
            insertCommand.Parameters.AddWithValue("@nok_phone", patients.nok_phone);
            insertCommand.Parameters.AddWithValue("@nok_relationship", patients.nok_relationship);
            insertCommand.Parameters.AddWithValue("@rhd", patients.rhd);
            insertCommand.Parameters.AddWithValue("@email", patients.email);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                return true;

            }
            catch (SqlException ex)
            {
                // throw ex;
                MessageBox.Show("Unable to Open SQL Server Database Table" + ex, "Add Customer Details", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }
        } */
        public static bool UpdatePatients(string PatientID,string groupcode)
        {

            patientinfo patients = new patientinfo();
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand updateCommand = new SqlCommand();
            updateCommand.CommandText = "dbo.PATIENT_Update";
            updateCommand.Connection = connection;
            updateCommand.CommandType = CommandType.StoredProcedure;

            updateCommand.Parameters.AddWithValue("@Patientno", patients.patientno);
            updateCommand.Parameters.AddWithValue("@groupcode", patients.groupcode);
            updateCommand.Parameters.AddWithValue("@patstatus", patients.patstatus);
            updateCommand.Parameters.AddWithValue("@Address1", patients.address1);
            updateCommand.Parameters.AddWithValue("@Patstate", patients.patstate);
            updateCommand.Parameters.AddWithValue("@Birthdate", patients.birthdate);
            updateCommand.Parameters.AddWithValue("@Sex", patients.sex);
            updateCommand.Parameters.AddWithValue("@M_status", patients.m_status);
            updateCommand.Parameters.AddWithValue("@Reg_date", patients.reg_date);
            updateCommand.Parameters.AddWithValue("@contact", patients.contact);
            updateCommand.Parameters.AddWithValue("@ghgroupcode", patients.ghgroupcode);
            updateCommand.Parameters.AddWithValue("@pattype", patients.pattype);
            updateCommand.Parameters.AddWithValue("@cr_limit", patients.cr_limit);
            updateCommand.Parameters.AddWithValue("@bill_cir", patients.cr_limit);
            updateCommand.Parameters.AddWithValue("@Birthdate", patients.birthdate);
            updateCommand.Parameters.AddWithValue("@cur_db", patients.cur_db);
            updateCommand.Parameters.AddWithValue("@cur_cr", patients.cur_cr);
            updateCommand.Parameters.AddWithValue("@balbf", patients.balbf);
            updateCommand.Parameters.AddWithValue("@trans_date", patients.trans_date);
            updateCommand.Parameters.AddWithValue("@name", patients.name);
            updateCommand.Parameters.AddWithValue("@grouphead", patients.grouphead);
            updateCommand.Parameters.AddWithValue("@grouphtype", patients.grouphtype);
            updateCommand.Parameters.AddWithValue("@isgrouphead", patients.isgrouphead);
            updateCommand.Parameters.AddWithValue("@title", patients.title);
            updateCommand.Parameters.AddWithValue("@patcateg", patients.patcateg);
            updateCommand.Parameters.AddWithValue("@remark", patients.remark);
            updateCommand.Parameters.AddWithValue("@discount", patients.discount);
            updateCommand.Parameters.AddWithValue("@hmoservtype", patients.hmoservtype);
            updateCommand.Parameters.AddWithValue("@currency", patients.currency);
            updateCommand.Parameters.AddWithValue("@billregistration", patients.billregistration);
            updateCommand.Parameters.AddWithValue("@clinic", patients.clinic);
            updateCommand.Parameters.AddWithValue("@surname", patients.surname);
            updateCommand.Parameters.AddWithValue("@othername", patients.othername);
            updateCommand.Parameters.AddWithValue("@homephone", patients.homephone);
            updateCommand.Parameters.AddWithValue("@workphone", patients.workphone);
            updateCommand.Parameters.AddWithValue("@employer", patients.employer);
            updateCommand.Parameters.AddWithValue("@emp_name", patients.emp_name);
            updateCommand.Parameters.AddWithValue("@emp_addr", patients.emp_addr);
            updateCommand.Parameters.AddWithValue("@emp_state", patients.emp_state);
            updateCommand.Parameters.AddWithValue("@cont_designation", patients.cont_designation);
            updateCommand.Parameters.AddWithValue("@pr_doc", patients.pr_doc);
            updateCommand.Parameters.AddWithValue("@refer_dr", patients.refer_dr);
            updateCommand.Parameters.AddWithValue("@nationality", patients.nationality);
            updateCommand.Parameters.AddWithValue("@occupation", patients.occupation);
            updateCommand.Parameters.AddWithValue("@religion", patients.religion);
            updateCommand.Parameters.AddWithValue("@bloodgroup", patients.bloodgroup);
            updateCommand.Parameters.AddWithValue("@genotype", patients.genotype);
            updateCommand.Parameters.AddWithValue("@nextofkin", patients.nextofkin);
            updateCommand.Parameters.AddWithValue("@nok_adr1", patients.nok_adr1);
            updateCommand.Parameters.AddWithValue("@nok_adr2", patients.nok_adr2);
            updateCommand.Parameters.AddWithValue("@nok_state", patients.nok_state);
            updateCommand.Parameters.AddWithValue("@nok_phone", patients.nok_phone);
            updateCommand.Parameters.AddWithValue("@nok_relationship", patients.nok_relationship);
            updateCommand.Parameters.AddWithValue("@rhd", patients.rhd);
            updateCommand.Parameters.AddWithValue("@email", patients.email);

            try
            {
                connection.Open();
                updateCommand.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }


        public static bool DeletePatient(string PatientID,string groupcode)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
           /* string deleteStatement =
                "DELETE FROM Patient " +
                "WHERE GroupCode = Patientno = @PatientID"; 
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection); */
            SqlCommand deleteStatement = new SqlCommand();
            deleteStatement.CommandText = "PATIENT_Delete";
            deleteStatement.Connection = connection;
            deleteStatement.CommandType = CommandType.StoredProcedure;

            deleteStatement.Parameters.AddWithValue("@Patientno", PatientID);
            deleteStatement.Parameters.AddWithValue("@Groupcode", groupcode);
            try
            {
                connection.Open();
                int count = deleteStatement.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch //(SqlException ex)
            {
                //throw ex;

                MessageBox.Show("Unable to Open SQL Server Database Table", "Delete Patient Record", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            finally
            {
                connection.Close();
            }

        }


    }
}