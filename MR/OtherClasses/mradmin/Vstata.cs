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
    public class Vstata
    {
        public string GROUPCODE { get; set; }
        public string PATIENTNO { get; set; }
        public string REFERENCE { get; set; }
        public string GENDER { get; set; }
        public string WLBS { get; set; }
        public string WSTONE { get; set; }
        public decimal WEIGHT { get; set; }
        public string HFT { get; set; }
        public string HIN { get; set; }
        public decimal HIGHT { get; set; }
        public string BPSITTING { get; set; }
        public string BPSTANDING { get; set; }
        public string PULSE { get; set; }
        public string TEMP { get; set; }
        public string RESPIRATIO { get; set; }
        public decimal BMP { get; set; }
        public string OTHERS { get; set; }
        public string CLINIC { get; set; }
        public string DOCTOR { get; set; }
        public string TIME { get; set; }
        public bool POSTED { get; set; }
        public DateTime POST_DATE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string HAIRCOLOR { get; set; }
        public string HAIRTYPE { get; set; }
        public string EYECOLOR { get; set; }
        public string COMPLEXION { get; set; }
        public string RACIALGRP { get; set; }
        public string ETHNICITY { get; set; }
        public string RELIGION { get; set; }
        public string BLOODGRP { get; set; }
        public string COMPLAINT { get; set; }
        public decimal HEADCIRCUMF { get; set; }
   

     public static Vstata GetVSTATA(string xreference )
        {
            if (string.IsNullOrWhiteSpace(xreference))
                return null;
            Vstata vstata = new Vstata();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.mrConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "VSTATA_Get"; //"spGetPatient";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@reference", xreference);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    vstata.GROUPCODE = reader["groupcode"].ToString();
                    vstata.PATIENTNO = reader["patientno"].ToString();
                    vstata.REFERENCE = reader["reference"].ToString();
                    vstata.GENDER = reader["gender"].ToString();
                    vstata.WLBS = reader["wlbs"].ToString();
                    vstata.WSTONE = reader["wstone"].ToString();
                    vstata.WEIGHT = (Decimal)reader["weight"];
                    vstata.HFT = reader["hft"].ToString();
                    vstata.HIN = reader["hin"].ToString();
                    vstata.HIGHT = (Decimal)reader["hight"];
                    vstata.BPSITTING = reader["bpsitting"].ToString();
                    vstata.BPSTANDING = reader["bpstanding"].ToString();
                    vstata.PULSE = reader["pulse"].ToString();
                    vstata.TEMP = reader["temp"].ToString();
                    vstata.RESPIRATIO = reader["respiratio"].ToString();
                    vstata.BMP = (Decimal)reader["bmp"];
                    vstata.OTHERS = reader["others"].ToString();
                    vstata.CLINIC = reader["clinic"].ToString();
                    vstata.DOCTOR = reader["doctor"].ToString();
                    vstata.TIME = reader["time"].ToString();
                    vstata.POSTED = (bool)reader["posted"];
                    vstata.POST_DATE = (DateTime)reader["post_date"];
                    vstata.TRANS_DATE = (DateTime)reader["trans_date"];
                    vstata.HAIRCOLOR = reader["haircolor"].ToString();
                    vstata.HAIRTYPE = reader["hairtype"].ToString();
                    vstata.EYECOLOR = reader["eyecolor"].ToString();
                    vstata.COMPLEXION = reader["complexion"].ToString();
                    vstata.RACIALGRP = reader["racialgrp"].ToString();
                    vstata.ETHNICITY = reader["ethnicity"].ToString();
                    vstata.RELIGION = reader["religion"].ToString();
                    vstata.BLOODGRP = reader["bloodgrp"].ToString();
                    vstata.COMPLAINT = reader["complaint"].ToString();
                    vstata.HEADCIRCUMF = (Decimal)reader["headcircumf"];                 
                                                                            
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
                MessageBox.Show("" + ex, "Get Patient OPD Vitals ", MessageBoxButtons.OK,
   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }

            return vstata;
        }
    
    }
}