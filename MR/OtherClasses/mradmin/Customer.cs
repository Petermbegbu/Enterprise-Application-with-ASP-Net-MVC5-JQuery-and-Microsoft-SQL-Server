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
    public class Customer
    {
 //       /*  private string name, category, address1, address2, custstate, country, phone, contactPerson, email, fax, operator1,
 //                 agent, foreignCurrency;
 //         private decimal currentCredit, currentDebit, balance, averageCredit, minimum, maximum;
 //         private int pricetag, nonCreditAcct, cardHolder;
 //         private DateTime dateRegistered;*/

 //       private string custno, name, category, address1, address2, statecode, country, phone, bill_cir,
 //           accountno, contact, custstatus, remark, captype, hmocode, patcateg, email, operators, currency;
 //       private decimal cur_db, cur_cr, cr_limit, lastnumb, cumvisits,  discount, hmomarkup,
 //       consultation, admissions, feeding;
   
 //       /*,sec_level,	balbf,debit1,credit1,balbf1,debit2,credit2,	balbf2,	debit3,	credit3,balbf3,
 //       debit4,	credit4,balbf4,	debit5,credit5,balbf5,debit6,credit6,balbf6,debit7,	credit7,balbf7,	debit8,	credit8,balbf8,	debit9,	credit9,balbf9,	
 //       debit10,credit10,balbf10,debit11,
 //   credit11,balbf11,debit12,credit12,balbf12, totbenefic,min_ord_am, max_ord_am, paymtnote,, billsbygc*/

 //       private DateTime post_date, date_reg, trans_date, dtime;
 //       private bool posted, onhis, hmo, trackform, linkadmsp, trackattndform, qaalert, billregistration, tosignbill;

 //       public Customer() { }

 //       public string Custno
 //       { get { return custno; } set { custno = value; } }
 //       public string Name
 //       { get { return name; } set { name = value; } }

 //       public string Category
 //       { get { return category; } set { category = value; } }

 //       public string Address1
 //       { get { return address1; } set { address1 = value; } }

 //       public string Address2
 //       { get { return address2; } set { address2 = value; } }

 //       public string Statecode
 //       { get { return statecode; } set { statecode = value; } }

 //       public string StateCode
 //       { get { return statecode; } set { statecode = value; } }

 //       public string Country
 //       { get { return country; } set { country = value; } }

 //       public string Phone
 //       { get { return phone; } set { phone = value; } }

 //       public string Contact
 //       { get { return contact; } set { contact = value; } }

 //       public string Email
 //       { get { return email; } set { email = value; } }

 //       public string Bill_cir
 //       { get { return bill_cir; } set { bill_cir = value; } }

 //       public string Accountno
 //       { get { return accountno; } set { accountno = value; } }

 //       public string Custstatus
 //       { get { return custstatus; } set { custstatus = value; } }

 //       public string Remark
 //       { get { return remark; } set { remark = value; } }

 //       public string Captype
 //       { get { return captype; } set { captype = value; } }

 //       public string Hmocode
 //       { get { return hmocode; } set { hmocode = value; } }

 //       public string Patcateg
 //       { get { return patcateg; } set { patcateg = value; } }

 ////       public string Paymtnote
 ////       { get { return paymtnote; } set { paymtnote = value; } }

 //       public string Operators
 //       { get { return operators; } set { operators = value; } }

 //       public string Currency
 //       { get { return currency; } set { currency = value; } }

 //       public decimal Cur_db
 //       { get { return cur_db; } set { cur_db = value; } }

 //       public decimal Cur_cr
 //       { get { return cur_cr; } set { cur_cr = value; } }

 //       public decimal Cr_limit
 //       { get { return cr_limit; } set { cr_limit = value; } }

 //       public decimal Lastnumb
 //       { get { return lastnumb; } set { lastnumb = value; } }

 //       public decimal Cumvisits
 //       { get { return cumvisits; } set { cumvisits = value; } }

 //   //    public decimal Totbenefic
 //   //    { get { return totbenefic; } set { totbenefic = value; } }
 //       public decimal Discount
 //       { get { return discount; } set { discount = value; } }
 //       public decimal Hmomarkup
 //       { get { return hmomarkup; } set { hmomarkup = value; } }
 //       public decimal Consultation { get { return consultation; } set { consultation = value; } }
 //       public decimal Admissions { get { return admissions; } set { admissions = value; } }
 //       public decimal Feeding { get { return feeding; } set { feeding = value; } }
 //       public DateTime Post_date { get { return post_date; } set { post_date = value; } }
 //       public DateTime Date_reg { get { return date_reg; } set { date_reg = value; } }
 //       public DateTime Trans_date { get { return trans_date; } set { trans_date = value; } }
 //       public DateTime Dtime { get { return dtime; } set { dtime = value; } }
 //       //bool posted, onhis, hmo, trackform, linkadmsp, trackattndform, billsbygc, qaalert, billregistration, tosignbill;
 //       public bool Hmo { get { return hmo; } set { hmo = value; } }
 //       public bool Posted { get { return posted; } set { posted = value; } }
 //       public bool Trackform { get { return trackform; } set { trackform = value; } }
 //       public bool Linkadmsp { get { return linkadmsp; } set { linkadmsp = value; } }
 //       public bool Trackattndform { get { return trackattndform; } set { trackattndform = value; } }
 //       //public bool Billsbygc { get { return billsbygc; } set { billsbygc = value; } }
 //       public bool Qaalert { get { return qaalert; } set { qaalert = value; } }
 //       public bool Billregistration { get { return billregistration; } set { billregistration = value; } }
 //       public bool Tosignbill { get { return tosignbill; } set { tosignbill = value; } }
        public string Custno { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Address1 { get; set; }
        public string Statecode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public decimal Cr_limit { get; set; }
        public string Bill_cir { get; set; }
        public decimal Upcur_db { get; set; }
        public decimal Upcur_cr { get; set; }
        public decimal Cur_db { get; set; }
        public decimal Cur_cr { get; set; }
        public string Accountno { get; set; }
        public DateTime LASTSTATMT { get; set; }
        public decimal SEC_LEVEL { get; set; }
        public bool Posted { get; set; }
        public DateTime Post_date { get; set; }
        public DateTime Date_reg { get; set; }
        public DateTime Trans_date { get; set; }
        public decimal Balbf { get; set; }
        public string Contact { get; set; }
        public string Custstatus { get; set; }
        public decimal Lastnumb { get; set; }
        public string Remark { get; set; }
        public bool ONHIS { get; set; }
        public string Captype { get; set; }
        public decimal Cumvisits { get; set; }
        public decimal TOTBENEFIC { get; set; }
        public string Hmocode { get; set; }
        public string Patcateg { get; set; }
        public string Email { get; set; }
        public decimal MIN_ORD_AM { get; set; }
        public decimal MAX_ORD_AM { get; set; }
        public decimal Discount { get; set; }
        public decimal Hmomarkup { get; set; }
        public bool HMO { get; set; }
        public string PAYMTNOTE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime DTIME { get; set; }
        public bool Trackform { get; set; }
        public bool Linkadmsp { get; set; }
        public bool Trackattndform { get; set; }
        public bool BILLSBYGC { get; set; }
        public string Currency { get; set; }
        public bool Qaalert { get; set; }
        public bool Billregistration { get; set; }
        public decimal GRPCREDITLIMIT { get; set; }
        public decimal GRPCREDITTYPE { get; set; }
        public bool Tosignbill { get; set; }
        public decimal Consultation { get; set; }
        public decimal Admissions { get; set; }
        public decimal Feeding { get; set; }
        public bool REFERRER { get; set; }
        public bool noncapitation { get; set; }
        public string bank_branch { get; set; }
        public bool ISGROUPHEAD { get; set; } 

        public static Customer GetCustomer(string CustNo)
        {
            Customer customer = new Customer();
 /*
            Customer customer = new Customer();
            SqlConnection connection = Dataaccess.mrConnection();
            string selectStatement =
                "SELECT Custno,Name,Address1,Address2,Category,Statecode,Country,Phone,Contact,Email,Bill_cir,Operators " +
                    "Accountno,Custstatus,Remark,Captype,Hmocode,Patcateg,Paymtnote,Currency,Cur_db,Cur_cr,Cr_limit,Lastnumb,Discount,Consultation " +
                    "Admissions,Feeding,Date_reg,Trans_date,Posted,Trackform,Linkadmsp,Trackattndform,Billsbygc,Qaalert,Billregistration,Tosignbill " +
                    "FROM Customer " +
                    "WHERE Custno = @CustomerID";
  
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            SqlParameter referenceParm = new SqlParameter();
            referenceParm.ParameterName = "@CustomerID"; referenceParm.Value = CustomerID;

            selectCommand.Parameters.Add(referenceParm);

            //selectCommand.Parameters.AddWithValue("@customerID", customer);
  */
            //SqlConnection conn = new SqlConnection(); conn = SCS01DB.GetConnection();
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "CUSTOMER_Get";
            selectCommand.Connection = connection ;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@CustNo", CustNo);

            connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
  
                    customer.Custno = reader["custno"].ToString();
                    customer.Name = reader["Name"].ToString();
                    customer.Address1 = reader["Address1"].ToString();
                    customer.Category = reader["Category"].ToString();
                    customer.Statecode = reader["Statecode"].ToString();
                    customer.Country = reader["Country"].ToString();
                    customer.Phone = reader["Phone"].ToString();
                    customer.Contact = reader["Contact"].ToString();
                    customer.Email = reader["Email"].ToString();
                     if (reader["Bill_cir"] != DBNull.Value)
                     {
                         customer.Bill_cir = reader["Bill_cir"].ToString();
                     }
                     else
                         customer.Bill_cir = "M";

                    customer.Accountno = reader["Accountno"].ToString();
                    customer.Custstatus = reader["Custstatus"].ToString();
                    customer.Remark = reader["Remark"].ToString();
                    customer.Captype = reader["Captype"].ToString();
                    customer.Hmocode = reader["Hmocode"].ToString();
                    customer.Patcateg = reader["Patcateg"].ToString();
                   // customer.Paymtnote = reader["Paymtnote"].ToString();
                   /* if (reader["Operators"] != DBNull.Value)
                    {
                        customer.Operators = reader["Operators"].ToString();
                    }
                    else
                        customer.Operators = "";*/
                    customer.Currency = reader["Currency"].ToString();
                    customer.Cur_db = (decimal)reader["Cur_db"];
                    customer.Cur_cr = (decimal)reader["Cur_cr"];
                    customer.Cr_limit = (decimal)reader["Cr_limit"];
                    customer.Balbf = (decimal)reader["balbf"];
                    customer.Lastnumb = (decimal)reader["Lastnumb"];
                    customer.Cumvisits = (decimal)reader["Cumvisits"];
                    //customer.Totbenefic = (decimal)reader["Totbenefic"];
                    customer.Discount = (decimal)reader["Discount"];
                    if (reader["HMOMARKUP"] != DBNull.Value)
                    {
                        customer.Hmomarkup = (decimal)reader["HMOMARKUP"];
                    }
                    else
                    {
                        customer.Hmomarkup = 0;
                    }

                   if (reader["Consultation"] != DBNull.Value)
                        {
                       customer.Consultation = (decimal)reader["Consultation"];
                        }
                    else
                    {
                        customer.Consultation = 0;
                    }
                   if (reader["Admissions"] != DBNull.Value)
                       customer.Admissions = (decimal)reader["Admissions"];
                   else
                       customer.Admissions = 0;
                    if (reader["Feeding"] != DBNull.Value)
                    {
                        customer.Feeding = (decimal)reader["Feeding"];
                    }
                    else
                        customer.Feeding = 0;
                   /* if (reader["Post_date"] != DBNull.Value)
                    { customer.Post_date = (DateTime)reader["Post_date"]; }*/
                    customer.Date_reg = (DateTime)reader["Date_reg"];
                    if (reader["Trans_date"] != DBNull.Value)
                        customer.Trans_date = (DateTime)reader["Trans_date"];
                    else
                        customer.Trans_date = DateTime.Now;
                 /*   if (reader["Dtime"] != DBNull.Value)
                    {customer.Dtime = (DateTime)reader["Dtime"];}*/
                    customer.Posted = (bool)reader["Posted"];
                    customer.Trackform = (bool)reader["Trackform"];
                    customer.Linkadmsp = (bool)reader["Linkadmsp"];
                    if (reader["Trackattndform"] != DBNull.Value)
                    {
                        customer.Trackattndform = (bool)reader["Trackattndform"];
                    }
                    else
                        customer.Trackattndform = false;

                    //customer.Billsbygc = (bool)reader["Billsbygc"];
                    //customer.Qaalert = (bool)reader["Qaalert"];
                    if (reader["Qaalert"] != DBNull.Value)
                    {
                        customer.Qaalert = (bool)reader["Qaalert"];
                    }
                    else
                        customer.Qaalert = false;

                    if (reader["Billregistration"] != DBNull.Value)
                    {
                        customer.Billregistration = (bool)reader["Billregistration"];
                    }
                    else
                        customer.Billregistration = false;
                    if (reader["Tosignbill"] != DBNull.Value)
                    {
                        customer.Tosignbill = (bool)reader["Tosignbill"];
                    }
                    if (reader["HMO"] != DBNull.Value)
                    {
                        customer.HMO = (bool)reader["HMO"];
                    }
                    else
                        customer.Tosignbill = false;
                    customer.Tosignbill = (bool)reader["Tosignbill"];
                    if (reader["Referrer"] != DBNull.Value)
                        customer.REFERRER = (bool)reader["Referrer"];
                    else
                        customer.REFERRER = false;
                    customer.noncapitation = (bool)reader["noncapitation"];
                    customer.bank_branch = reader["bank_branch"].ToString();
                    customer.ISGROUPHEAD = (bool)reader["isgrouphead"];

                }
                
                reader.Close();
                connection.Close();
            
            return customer;

        }
        public static bool AddCustomer(bool newrec, Customer customer)
        {
            //Customer customer = new Customer();
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = (newrec) ? "CUSTOMER_Add" : "CUSTOMER_Update";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Custno", customer.Custno);
            insertCommand.Parameters.AddWithValue("@Name", customer.Name);
            insertCommand.Parameters.AddWithValue("@Category", customer.Category);
            insertCommand.Parameters.AddWithValue("@Address1", customer.Address1);
            insertCommand.Parameters.AddWithValue("@Statecode", customer.Statecode);
            insertCommand.Parameters.AddWithValue("@Country", customer.Country);
            insertCommand.Parameters.AddWithValue("@Phone", customer.Phone);
            insertCommand.Parameters.AddWithValue("@CR_LIMIT",  customer.Cr_limit);
            insertCommand.Parameters.AddWithValue("@Bill_cir", customer.Bill_cir);
            insertCommand.Parameters.AddWithValue("@Cur_db", customer.Cur_db);
            insertCommand.Parameters.AddWithValue("@Cur_cr", customer.Cur_cr);
            insertCommand.Parameters.AddWithValue("@Accountno", customer.Accountno);
            insertCommand.Parameters.AddWithValue("@Contact",customer.Contact);
            insertCommand.Parameters.AddWithValue("@Email", customer.Email);
            insertCommand.Parameters.AddWithValue("@Custstatus", customer.Custstatus);
            insertCommand.Parameters.AddWithValue("@Remark", customer.Remark);
            insertCommand.Parameters.AddWithValue("@Captype", customer.Captype);
            insertCommand.Parameters.AddWithValue("@Hmocode", customer.Hmocode);
            insertCommand.Parameters.AddWithValue("@Patcateg", customer.Patcateg);
            insertCommand.Parameters.AddWithValue("@Paymtnote", customer.PAYMTNOTE);
            insertCommand.Parameters.AddWithValue("@Currency", customer.Currency);
            insertCommand.Parameters.AddWithValue("@Lastnumb", customer.Lastnumb);
            insertCommand.Parameters.AddWithValue("@Discount", customer.Discount);
            insertCommand.Parameters.AddWithValue("@Consultation", customer.Consultation);
            insertCommand.Parameters.AddWithValue("@Admissions", customer.Admissions);
            insertCommand.Parameters.AddWithValue("@Feeding", customer.Feeding);
            insertCommand.Parameters.AddWithValue("@Date_reg", customer.Date_reg);
            insertCommand.Parameters.AddWithValue("@Trans_date", customer.Trans_date);
            insertCommand.Parameters.AddWithValue("@Trackform", customer.Trackform);
            insertCommand.Parameters.AddWithValue("@Linkadmsp", customer.Linkadmsp);
            insertCommand.Parameters.AddWithValue("@Trackattndform", customer.Trackattndform);
            insertCommand.Parameters.AddWithValue("@Billsbygc", customer.BILLSBYGC);
            insertCommand.Parameters.AddWithValue("@Qaalert", customer.Qaalert);
            insertCommand.Parameters.AddWithValue("@Billregistration", customer.Billregistration);
            insertCommand.Parameters.AddWithValue("@Tosignbill", customer.Tosignbill);
            insertCommand.Parameters.AddWithValue("@Referrer", customer.REFERRER);
            insertCommand.Parameters.AddWithValue("@OPERATOR", customer.OPERATOR);
            insertCommand.Parameters.AddWithValue("@DTIME", customer.DTIME);
            insertCommand.Parameters.AddWithValue("@Balbf",  customer.Balbf);
            insertCommand.Parameters.AddWithValue("@bank_branch", customer.bank_branch);
            insertCommand.Parameters.AddWithValue("@ONHIS", false);
            insertCommand.Parameters.AddWithValue("@CUMVISITS", 0m);
            insertCommand.Parameters.AddWithValue("@TOTBENEFIC", 0m);
            insertCommand.Parameters.AddWithValue("@MIN_ORD_AM", 0m);
            insertCommand.Parameters.AddWithValue("@MAX_ORD_AM", 0m);
            insertCommand.Parameters.AddWithValue("@HMOMARKUP", customer.HMO ? customer.Hmomarkup : 0m );
            insertCommand.Parameters.AddWithValue("@HMO", customer.HMO ? true : false);
            insertCommand.Parameters.AddWithValue("@GRPCREDITLIMIT", 0m);
            insertCommand.Parameters.AddWithValue("@GRPCREDITTYPE", 0m);
            insertCommand.Parameters.AddWithValue("@NONCAPITATION", customer.HMO ? customer.noncapitation : false);
            insertCommand.Parameters.AddWithValue("@ISGROUPHEAD", customer.ISGROUPHEAD ? true : false);
            if (newrec)
            {
                insertCommand.Parameters.AddWithValue("@LASTSTATMT", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@SEC_LEVEL", 0m);
                insertCommand.Parameters.AddWithValue("@POSTED", false);
                insertCommand.Parameters.AddWithValue("@POST_DATE", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@UPCUR_DB", 0m);
                insertCommand.Parameters.AddWithValue("@UPCUR_CR", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT1", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT1", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF1", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT2", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT2", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF2", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT3", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT3", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF3", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT4", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT4", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF4", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT5", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT5", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF5", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT6", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT6", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF6", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT7", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT7", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF7", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT8", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT8", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF8", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT9", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT9", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF9", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT10", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT10", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF10", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT11", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT11", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF11", 0m);
                insertCommand.Parameters.AddWithValue("@DEBIT12", 0m);
                insertCommand.Parameters.AddWithValue("@CREDIT12", 0m);
                insertCommand.Parameters.AddWithValue("@BALBF12", 0m);
            }
                connection.Open();
                insertCommand.ExecuteNonQuery();
                connection.Close();
            return true;
        }
        public static bool DeleteCustomer(string CustomerID)
        {
            //Customer customer = new Customer();
            SqlConnection connection = Dataaccess.mrConnection();
            string deleteStatement =
                "DELETE FROM Customer " +
                "WHERE Custno = @CustomerID";
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection);
            
                connection.Open();
                int count = deleteCommand.ExecuteNonQuery();
                connection.Close();

                if (count > 0)
                    return true;
                else
                    return false;


        }
        /// <summary>
        /// get list
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCustomer()
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "Customer_Getlist"; 
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter ds = new SqlDataAdapter(selectCommand);
            DataTable dt = new DataTable();

            ds.Fill(dt);
            connection.Close();
            return dt;
        }
  
    } //end of class
}
/*      public decimal DEBIT1 { get; set; }
      public decimal CREDIT1 { get; set; }
      public decimal BALBF1 { get; set; }
      public decimal DEBIT2 { get; set; }
      public decimal CREDIT2 { get; set; }
      public decimal BALBF2 { get; set; }
      public decimal DEBIT3 { get; set; }
      public decimal CREDIT3 { get; set; }
      public decimal BALBF3 { get; set; }
      public decimal DEBIT4 { get; set; }
      public decimal CREDIT4 { get; set; }
      public decimal BALBF4 { get; set; }
      public decimal DEBIT5 { get; set; }
      public decimal CREDIT5 { get; set; }
      public decimal BALBF5 { get; set; }
      public decimal DEBIT6 { get; set; }
      public decimal CREDIT6 { get; set; }
      public decimal BALBF6 { get; set; }
      public decimal DEBIT7 { get; set; }
      public decimal CREDIT7 { get; set; }
      public decimal BALBF7 { get; set; }
      public decimal DEBIT8 { get; set; }
      public decimal CREDIT8 { get; set; }
      public decimal BALBF8 { get; set; }
      public decimal DEBIT9 { get; set; }
      public decimal CREDIT9 { get; set; }
      public decimal BALBF9 { get; set; }
      public decimal DEBIT10 { get; set; }
      public decimal CREDIT10 { get; set; }
      public decimal BALBF10 { get; set; }
      public decimal DEBIT11 { get; set; }
      public decimal CREDIT11 { get; set; }
      public decimal BALBF11 { get; set; }
      public decimal DEBIT12 { get; set; }
      public decimal CREDIT12 { get; set; }
      public decimal BALBF12 { get; set; }*/



                 