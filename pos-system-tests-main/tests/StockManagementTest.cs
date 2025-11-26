using System;
using System.Data;
using POSales;

namespace POSales.TestCode
{
    public class StockManagementTest
    {
        public void CheckCurrentStock()
        {
            DBConnect db = new DBConnect();
            string productCode = "P0Q1";

            Console.WriteLine("Stock Management Test\n");
            Console.WriteLine("Checking current stock for " + productCode);

            DataTable prod = db.getTable("SELECT * FROM tbProduct WHERE pcode='" + productCode + "'");

            if(prod.Rows.Count > 0)
            {
                int currentQty = int.Parse(prod.Rows[0]["qty"].ToString());
                string desc = prod.Rows[0]["pdesc"].ToString();

                Console.WriteLine("Product: " + desc);
                Console.WriteLine("Current stock: " + currentQty + " units\n");
            }
        }

        public void RecordStockIn()
        {
            DBConnect db = new DBConnect();
            string pcode = "P0Q1";
            int addQty = 50;

            Console.WriteLine("Recording stock in...");

            string refNo = "ST" + DateTime.Now.ToString("yyyyMMddHHmmss");

            string insertStockIn = "INSERT INTO tbStockIn (refno, pcode, sdate, stockinby, supplierid, qty) " +
                                "VALUES ('" + refNo + "', '" + pcode + "', '" + DateTime.Now + "', " +
                                "'TestAdmin', 1, " + addQty + ")";

            db.ExecuteQuery(insertStockIn);

            Console.WriteLine("Stock in recorded");
            Console.WriteLine("Reference: " + refNo);
            Console.WriteLine("Quantity: " + addQty + " units\n");
        }

        public void UpdateProductQuantity()
        {
            DBConnect db = new DBConnect();
            string pcode = "P0Q1";
            int addQty = 50;

            Console.WriteLine("Updating product quantity in system...");

            string updateQty = "UPDATE tbProduct SET qty = qty + " + addQty + " WHERE pcode='" + pcode + "'";
            db.ExecuteQuery(updateQty);

            Console.WriteLine("Quantity updated (+50 units)\n");
        }

        public void CheckLowStock()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("Checking for low stock items...");

            DataTable lowStock = db.getTable("SELECT * FROM tbProduct WHERE qty <= reorder");

            if(lowStock.Rows.Count > 0)
            {
                Console.WriteLine("Found " + lowStock.Rows.Count + " items below reorder level:\n");

                foreach(DataRow item in lowStock.Rows)
                {
                    Console.WriteLine("-- " + item["pdesc"]);
                    Console.WriteLine("   Current: " + item["qty"] + " | Reorder: " + item["reorder"]);
                }
            }
            else
            {
                Console.WriteLine("All products have sufficient stock");
            }
        }

        public void ViewStockHistory()
        {
            DBConnect db = new DBConnect();
            string pcode = "P001";

            Console.WriteLine("\nViewing stock in history for " + pcode);

            DataTable history = db.getTable("SELECT * FROM tbStockIn WHERE pcode='" + pcode + "' " +
                                          "ORDER BY sdate DESC");

            Console.WriteLine("Recent stock in records:");

            int count = 0;
            foreach(DataRow r in history.Rows)
            {
                if(count >= 5) break;

                Console.WriteLine("Date: " + r["sdate"] + " | Qty: " + r["qty"] + " | Ref: " + r["refno"]);
                count++;
            }
        }

        public static void Main(string[] args)
        {
            StockManagementTest test = new StockManagementTest();
            
            Console.WriteLine("=== STOCK MANAGEMENT TEST ===");
            test.CheckCurrentStock();
            test.RecordStockIn();
            test.UpdateProductQuantity();
            test.CheckLowStock();
            test.ViewStockHistory();
        }
    }
}
