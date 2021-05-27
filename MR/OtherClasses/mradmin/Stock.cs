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
    public class Stock
    {
        public string Store { get; set; }
        public string Type { get; set; }
        public string Item { get; set; }
        public string Part { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Bin { get; set; }
        public decimal Cost { get; set; }
        public decimal Sell { get; set; }
        public decimal StockQty { get; set; }
        public decimal CommittedUsage { get; set; }
        public decimal Safety { get; set; }
        public decimal Maximum { get; set; }
        public decimal Reorderlv { get; set; }
        public decimal ReorderQt { get; set; }
        public int Reordercy { get; set; }
        public DateTime LastOrder { get; set; }
        public DateTime NextOrder { get; set; }
        public decimal OrderQty { get; set; }
        public decimal QtyPending{get;set;}
        public int Stockfreq { get; set; }
        public DateTime LastStock { get; set; }
        public DateTime NextStock { get; set; }
        public decimal OutSales { get; set; }
        public decimal OutPurch { get; set; }
        public decimal YtdSales { get; set; }
        public decimal Ytdreceip { get; set; }
        public decimal Ytdissues { get; set; }
        public decimal Ytdreturn { get; set; }
        public decimal Ytdorders { get; set; }
        public decimal Ytdloss { get; set; }
        public bool OdrPend { get; set; }
        public string _Operator { get; set; }
        public DateTime PostDate { get; set; }
        public bool Posted { get; set; }
        public DateTime TransDate { get; set; }
        public string Vendorno1 { get; set; }
        public string Vendorno2 { get; set; }
        public string Vendorno3 { get; set; }
        public string Vendorno4 { get; set; }
        public string Vendorno5 { get; set; }
        public string Pack { get; set; }
        public int PacketQty { get; set; }
        public int TotalPack { get; set; }
        public bool Isfifo { get; set; }
        public string RecType { get; set; }
        public decimal Usize { get; set; }
        public bool Flexwt { get; set; }
        public decimal Qfifo { get; set; }
        public string SEllUnit { get; set; }
        public string SellDesc { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Sellval { get; set; }
        public decimal costval { get; set; }
        public string Generic { get; set; }
        public string Imagefile { get; set; }
        public decimal Whsell { get; set; }
        public DateTime Dtime { get; set; }
        public decimal Fccost { get; set; }
        public decimal Fcsell { get; set; }
        public string Currency { get; set; }
        public string Comments { get; set; }
        public string Aimagefile { get; set; }
        public string Status { get; set; }
        public string Barcode { get; set; }
        public decimal Strength { get; set; }
        public decimal Per { get; set; }

        public static Stock GetStock(string xstore, string xitem,bool byitemonly)
        {
            Stock Stock = new Stock();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.stkConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = (byitemonly) ? "Stock_GetByItem" : "Stock_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            if (!byitemonly)
            {
                selectCommand.Parameters.AddWithValue("@Store", xstore);
            }
            selectCommand.Parameters.AddWithValue("@Item", xitem);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    Stock.Store = reader["store"].ToString();
                    Stock.Type = reader["type"].ToString();
                    Stock.Item = reader["item"].ToString();
                    Stock.Part = reader["part"].ToString();
                    Stock.Name = reader["name"].ToString();
                    Stock.Unit = reader["unit"].ToString();
                    Stock.Bin = reader["bin"].ToString();
                    Stock.Sell = (decimal)reader["sell"];
                    Stock.Cost = (decimal)reader["cost"];
                    Stock.StockQty = (decimal)reader["stockQty"];
                    Stock.CommittedUsage = (decimal)reader["committedusage"];
                    Stock.Safety = (decimal)reader["safety"];
                    Stock.Maximum = (decimal)reader["maximum"];
                    Stock.Reorderlv = (decimal)reader["reorderlv"];
                    Stock.ReorderQt = (decimal)reader["reorderqt"];
                    Stock.Reordercy = (int)reader["reordercy"];
                    Stock.LastOrder = (DateTime)reader["lastOrder"];
                    Stock.NextOrder = (DateTime)reader["Nextorder"];
                    Stock.OrderQty = (decimal)reader["OrderQty"];
                    Stock.QtyPending = (decimal)reader["qtyPending"];
                    Stock.Stockfreq = (int)reader["stockfreq"];
                    Stock.LastStock = (DateTime)reader["laststock"];
                    Stock.NextStock = (DateTime)reader["nextstock"];
                    Stock.OutSales = (decimal)reader["outsales"];
                    Stock.OutPurch = (decimal)reader["outpurch"];
                    Stock.YtdSales = (decimal)reader["ytdsales"];
                    Stock.Ytdreceip = (decimal)reader["ytdreceip"];
                    Stock.Ytdissues = (decimal)reader["ytdtssues"];
                    Stock.Ytdreturn = (decimal)reader["ytdreturn"];
                    Stock.Ytdorders = (decimal)reader["ytdorders"];
                    Stock.Ytdloss = (decimal)reader["ytdloss"];
                    Stock.OdrPend = (bool)reader["ordpend"];
                    Stock._Operator = reader["_operator"].ToString();
                    Stock.PostDate = (DateTime)reader["postDate"];
                    Stock.Posted = (bool)reader["posted"];
                    Stock.TransDate = (DateTime)reader["transDate"];
                    Stock.Vendorno1 = reader["tendorno1"].ToString();
                    Stock.Vendorno2 = reader["Vendorno2"].ToString();
                    Stock.Vendorno3 = reader["Vendorno3"].ToString();
                    Stock.Vendorno4 = reader["Vendorno4"].ToString();
                    Stock.Vendorno5 = reader["Vendorno5"].ToString();
                    Stock.Pack = reader["pack"].ToString();
                    Stock.PacketQty = (int)reader["packetqty"];
                    Stock.TotalPack = (int)reader["pack"];
                    Stock.Isfifo = (bool)reader["isfifo"];
                    Stock.RecType = reader["rectype"].ToString();
                    Stock.Usize = (decimal)reader["usize"];
                    Stock.Flexwt = (bool)reader["flexwt"];
                    Stock.Qfifo = (decimal)reader["qfifo"];
                    Stock.SEllUnit = reader["sEllUnit"].ToString();
                    Stock.SellDesc = reader["sellDesc"].ToString();
                    Stock.ExpiryDate = (DateTime)reader["expiryDate"];
                    Stock.Sellval = (decimal)reader["sellval"];
                    Stock.costval = (decimal)reader["imageFile"];
                    Stock.Generic = reader["generic"].ToString();
                    Stock.Imagefile = reader["imagefile"].ToString();
                    Stock.Whsell = (decimal)reader["whsell"];
                    Stock.Dtime = (DateTime)reader["dtime"];
                    Stock.Fccost = (decimal)reader["fccost"];
                    Stock.Fcsell = (decimal)reader["fcsell"];
                    Stock.Currency = reader["currency"].ToString();
                    Stock.Comments = reader["comments"].ToString();
                    Stock.Aimagefile = reader["Aimagefile"].ToString();
                    Stock.Status = reader["status"].ToString();
                    Stock.Barcode = reader["barcode"].ToString();
                    Stock.Strength = (decimal)reader["strength"];
                    Stock.Per = (decimal)reader["per"];
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
                MessageBox.Show("" + ex, "Get Stock Details ", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }
            return Stock;
        }
        public static Stock CheckStockBalance_Expiry(string xstore, string xitem)
        {
            Stock Stock = new Stock();

            SqlConnection connection = new SqlConnection(); connection = Dataaccess.stkConnection();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = "Stock_Get";
            selectCommand.Connection = connection;
            selectCommand.CommandType = CommandType.StoredProcedure;

            selectCommand.Parameters.AddWithValue("@Store", xstore);
            selectCommand.Parameters.AddWithValue("@Item", xitem);
            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
 /*                   Stock.Store = reader["store"].ToString();
                    Stock.Type = reader["type"].ToString();
                    Stock.Item = reader["item"].ToString();
                    Stock.Part = reader["part"].ToString();
                    Stock.Name = reader["name"].ToString();
                    Stock.Unit = reader["unit"].ToString();
                    Stock.Bin = reader["bin"].ToString();*/
                    Stock.Sell = (decimal)reader["sell"];
                    Stock.StockQty = (decimal)reader["stockQty"];
/*                    Stock.CommittedUsage = (decimal)reader["committedusage"];
                    Stock.Safety = (decimal)reader["safety"];
                    Stock.Maximum = (decimal)reader["maximum"];
                    Stock.Reorderlv = (decimal)reader["reorderlv"];
                    Stock.ReorderQt = (decimal)reader["reorderqt"];
                    Stock.Reordercy = (int)reader["reordercy"];
                    Stock.LastOrder = (DateTime)reader["lastOrder"];
                    Stock.NextOrder = (DateTime)reader["Nextorder"];
                    Stock.OrderQty = (decimal)reader["OrderQty"];
                    Stock.QtyPending = (decimal)reader["qtyPending"];
                    Stock.Stockfreq = (int)reader["stockfreq"];
                    Stock.LastStock = (DateTime)reader["laststock"];
                    Stock.NextStock = (DateTime)reader["nextstock"];
                    Stock.OutSales = (decimal)reader["outsales"];
                    Stock.OutPurch = (decimal)reader["outpurch"];
                    Stock.YtdSales = (decimal)reader["ytdsales"];
                    Stock.Ytdreceip = (decimal)reader["ytdreceip"];
                    Stock.Ytdissues = (decimal)reader["ytdtssues"];
                    Stock.Ytdreturn = (decimal)reader["ytdreturn"];
                    Stock.Ytdorders = (decimal)reader["ytdorders"];
                    Stock.Ytdloss = (decimal)reader["ytdloss"];
                    Stock.OdrPend = (bool)reader["ordpend"];
                    Stock._Operator = reader["_operator"].ToString();
                    Stock.PostDate = (DateTime)reader["postDate"];
                    Stock.Posted = (bool)reader["posted"];
                    Stock.TransDate = (DateTime)reader["transDate"];
                    Stock.Vendorno1 = reader["tendorno1"].ToString();
                    Stock.Vendorno2 = reader["Vendorno2"].ToString();
                    Stock.Vendorno3 = reader["Vendorno3"].ToString();
                    Stock.Vendorno4 = reader["Vendorno4"].ToString();
                    Stock.Vendorno5 = reader["Vendorno5"].ToString();
                    Stock.Pack = reader["pack"].ToString();
                    Stock.PacketQty = (int)reader["packetqty"];
                    Stock.TotalPack = (int)reader["pack"];
                    Stock.Isfifo = (bool)reader["isfifo"];
                    Stock.RecType = reader["rectype"].ToString();
                    Stock.Uize = (decimal)reader["usize"];
                    Stock.Flexwt = (bool)reader["flexwt"];
                    Stock.Qfifo = (decimal)reader["qfifo"];
                    Stock.SEllUnit = reader["sEllUnit"].ToString();
                    Stock.SellDesc = reader["sellDesc"].ToString(); */
                    Stock.ExpiryDate = (DateTime)reader["expiryDate"];
/*                    Stock.Sellval = (decimal)reader["sellval"];
                    Stock.costval = (decimal)reader["imageFile"];
                    Stock.Generic = reader["generic"].ToString();
                    Stock.Imagefile = reader["imagefile"].ToString(); */
                    Stock.Whsell = (decimal)reader["whsell"];
/*                    Stock.Dtime = (DateTime)reader["dtime"];
                    Stock.Fccost = (decimal)reader["fccost"];
                    Stock.Fcsell = (decimal)reader["fcsell"];
                    Stock.Currency = reader["currency"].ToString();
                    Stock.Comments = reader["comments"].ToString();
                    Stock.Aimagefile = reader["Aimagefile"].ToString();
                    Stock.Status = reader["status"].ToString();
                    Stock.Barcode = reader["barcode"].ToString();
                    Stock.Strength = (decimal)reader["strength"];
                    Stock.Per = (decimal)reader["per"];*/
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
                MessageBox.Show("" + ex, "Get Stock Details ", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                connection.Close();
                return null;
            }
            finally
            {
                connection.Close();
            }
            return Stock;
        }
        /// <summary>
        /// updatetype : I-issues,R-receipt,U-return,N-openingstk,C-stockcount - Rectype : P-production stock
        /// </summary>
        /// <param name="xstore"></param>
        /// <param name="xitem"></param>
        /// <returns></returns>
        public static void writeTransmast(DateTime trans_date, string store, string stkcode, string transdescription, string updatetype, decimal updateqty,
            string reference, bool toadd, string xoperator, DateTime operatordttime, string purpose)
        {
            //get stock information
            Stock stock = new Stock();
            stock = Stock.GetStock(store, stkcode, false);
            decimal stkbalance = toadd ? stock.StockQty + updateqty : stock.StockQty - updateqty;

            DateTime dtmin_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.stkConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "TRANSMAS_Add";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Ref_No", reference);
            insertCommand.Parameters.AddWithValue("@Trans_Date", trans_date);
            insertCommand.Parameters.AddWithValue("@TransType", updatetype);
            insertCommand.Parameters.AddWithValue("@Store", store);
            insertCommand.Parameters.AddWithValue("@Item", stkcode);
            insertCommand.Parameters.AddWithValue("@Descript", transdescription);
            insertCommand.Parameters.AddWithValue("@Trans_Qty", updateqty);
            insertCommand.Parameters.AddWithValue("@Stock_Bal",stkbalance);
            insertCommand.Parameters.AddWithValue("@Cost", stock.Cost);
            insertCommand.Parameters.AddWithValue("@Sell", stock.Sell);
            insertCommand.Parameters.AddWithValue("@Posted", false);
            insertCommand.Parameters.AddWithValue("@Bin", stock.Bin);
            insertCommand.Parameters.AddWithValue("@Type", stock.Type);
            insertCommand.Parameters.AddWithValue("@RecType",stock.RecType);
            insertCommand.Parameters.AddWithValue("@Unit", stock.Unit);
            insertCommand.Parameters.AddWithValue("@Costval",stock.costval);
            insertCommand.Parameters.AddWithValue("@Whsell", stock.Whsell);
            insertCommand.Parameters.AddWithValue("@Operator", xoperator);
            insertCommand.Parameters.AddWithValue("@dtime", operatordttime);
            insertCommand.Parameters.AddWithValue("@usize", stock.Usize);
            insertCommand.Parameters.AddWithValue("@Sellval",stock.Sellval);
            insertCommand.Parameters.AddWithValue("@FcCost",stock.Fccost);
            insertCommand.Parameters.AddWithValue("@Fcsell",stock.Fcsell);
            insertCommand.Parameters.AddWithValue("@Currency", stock.Currency);
            insertCommand.Parameters.AddWithValue("@Status", stock.Status);
            insertCommand.Parameters.AddWithValue("@purpose", purpose);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
             }
            catch (SqlException ex)
            {
                MessageBox.Show("" + ex, "Stock Movement Update", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
        public static void updatestkmastBal(DateTime trans_date, string store, string stkcode, decimal updateqty,bool toadd)
        {
            SqlConnection connection = new SqlConnection(); connection = Dataaccess.stkConnection();
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText = "stock_UpdateBalance";
            insertCommand.Connection = connection;
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Trans_Date", trans_date);
            insertCommand.Parameters.AddWithValue("@Store", store);
            insertCommand.Parameters.AddWithValue("@Item", stkcode);
            insertCommand.Parameters.AddWithValue("@Trans_Qty", updateqty);
            insertCommand.Parameters.AddWithValue("@toadd", toadd);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("" + ex, "Stock Master Balance Update", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            finally
            {
                connection.Close();
            }
        }
       
    }
}