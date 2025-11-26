using System;
using System.Data;
using POSales;

namespace POSales.TestCode
{
    public class SalesTransactionTest
    {
        public void CompletesSalesProcess()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("=== Complete Sales Transaction Test ===\n");

            Console.WriteLine("Step 1: User Login");
            string cashier = "cashier01";
            string password = db.getPassword(cashier);

            if(password != null)
            {
                Console.WriteLine("User '" + cashier + "' logged in successfully\n");
            }
            else
            {
                Console.WriteLine("Login failed\n");
                return;
            }

            Console.WriteLine("Step 2: Scan product");
            string pcode = "P0Q1";
            DataTable product = db.getTable("SELECT * FROM tbProduct WHERE pcode='" + pcode + "'");

            if(product.Rows.Count == 0)
            {
                Console.WriteLine("Product not found\n");
                return;
            }

            double price = double.Parse(product.Rows[0]["price"].ToString());
            int stock = int.Parse(product.Rows[0]["qty"].ToString());
            string pdesc = product.Rows[0]["pdesc"].ToString();

            Console.WriteLine("Product: " + pdesc);
            Console.WriteLine("Price: RW" + price);
            Console.WriteLine("Available: " + stock + " units\n");

            Console.WriteLine("Step 3: Customer purchase");
            int buyQty = 2;
            Console.WriteLine("Customer buying: " + buyQty + " units");

            if(buyQty > stock)
            {
                Console.WriteLine("Insufficient stock!\n");
                return;
            }

            Console.WriteLine("\nStep 4: Calculate total");
            double subtotal = price * buyQty;
            double discountRate = 5.0;
            double discount = subtotal * (discountRate / 100);
            double total = subtotal - discount;

            Console.WriteLine("Subtotal: RW" + subtotal.ToString("F2"));
            Console.WriteLine("Discount (" + discountRate + "%): -RW" + discount.ToString("F2"));
            Console.WriteLine("Total: RW" + total.ToString("F2") + "\n");

            Console.WriteLine("Step 5: Process payment");
            string transNo = "SALE" + DateTime.Now.Ticks;

            string insertSale = "INSERT INTO tbCart (transno, pcode, price, qty, disc, total, sdate, status, cashier) " +
                               "VALUES ('" + transNo + "', '" + pcode + "', " + price + ", " + buyQty + ", " +
                               discount + ", " + total + ", '" + DateTime.Now + "', 'Sold', '" + cashier + "')";
            db.ExecuteQuery(insertSale);

            Console.WriteLine("Transaction recorded: " + transNo + "\n");

            Console.WriteLine("Step 6: Update inventory");
            string reduceStock = "UPDATE tbProduct SET qty = qty - " + buyQty + " WHERE pcode='" + pcode + "'";
            db.ExecuteQuery(reduceStock);

            Console.WriteLine("Stock reduced by " + buyQty + " units");

            Console.WriteLine("\n=== Transaction Complete ===");
            Console.WriteLine("Receipt #: " + transNo);
            Console.WriteLine("Amount Paid: RW" + total.ToString("F2"));
        }

        public void ViewDailySales()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("\n\nDaily Sales Report");
            Console.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy") + "\n");

            string today = DateTime.Now.ToString("yyyy-MM-dd");

            DataTable sales = db.getTable("SELECT * FROM tbCart WHERE CAST(sdate AS DATE) = '" + today + "' AND status='Sold'");

            Console.WriteLine("Total transactions: " + sales.Rows.Count);

            double totalRevenue = 0;
            foreach(DataRow s in sales.Rows)
            {
                totalRevenue += double.Parse(s["total"].ToString());
            }

            Console.WriteLine("Total revenue: RW" + totalRevenue.ToString("F2"));
        }

        public void ProcessRefund()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("\n\nRefund Process Test");
            string transNo = "SALE123456";

            Console.WriteLine("Processing refund for: " + transNo);

            DataTable trans = db.getTable("SELECT * FROM tbCart WHERE transno='" + transNo + "'");

            if(trans.Rows.Count > 0)
            {
                string pcode = trans.Rows[0]["pcode"].ToString();
                int qty = int.Parse(trans.Rows[0]["qty"].ToString());
                string returnStock = "UPDATE tbProduct SET qty = qty + " + qty + " WHERE pcode='" + pcode + "'";
                db.ExecuteQuery(returnStock);
                string updateStatus = "UPDATE tbCart SET status='Refund' WHERE transno='" + transNo + "'";
                db.ExecuteQuery(updateStatus);
                Console.WriteLine("Refund processed");
                Console.WriteLine("Stock returned: " + qty + " units");
            }
            else
            {
                Console.WriteLine("Transaction not found");
            }
        }

        public static void Main(string[] args)
        {
            SalesTransactionTest test = new SalesTransactionTest();
            
            Console.WriteLine("=== SALES TRANSACTION TEST ===");
            test.CompletesSalesProcess();
            test.ViewDailySales();
            test.ProcessRefund();
        }
    }
}
