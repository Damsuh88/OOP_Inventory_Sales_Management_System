using System;
using System.Data;
using POSales;

namespace POSales.TestCode
{
    public class ProductManagementTest
    {
        public void AddProductTest()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("Product Management Test\n");
            Console.WriteLine("Step 1: Adding new product");

            string sql = "INSERT INTO tbProduct (pcode, barcode, pdesc, bid, cid, price, qty, reorder) " + 
                        "VALUES ('TESTPRODB1', '9876543210', 'Test Chocolate Bar', 1, 2, 5.50, 100, 20)";

            db.ExecuteQuery(sql);
            Console.WriteLine("Product added successfully\n");
        }

        public void ViewProductTest()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("Step 2: Checking product details");

            DataTable prod = db.getTable("SELECT * FROM tbProduct WHERE pcode='TESTPRODB1'");

            if(prod.Rows.Count > 0)
            {
                DataRow row = prod.Rows[0];
                Console.WriteLine("Product Code: " + row["pcode"]);
                Console.WriteLine("Description: " + row["pdesc"]);
                Console.WriteLine("Price: RW" + row["price"]);
                Console.WriteLine("Stock: " + row["qty"] + " units");
                Console.WriteLine("Reorder Level: " + row["reorder"] + "\n");
            }
        }

        public void UpdateProductTest()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("Step 3: Updating product price");

            string update = "UPDATE tbProduct SET price = 6.00 WHERE pcode='TESTPRODB1'";
            db.ExecuteQuery(update);

            Console.WriteLine("Price updated to RW6.00\n");
        }

        public void SearchProductTest()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("Step 4: Searching products");

            string keyword = "chocolate";
            DataTable results = db.getTable("SELECT * FROM tbProduct WHERE pdesc LIKE '%" + keyword + "%'");

            Console.WriteLine("Found " + results.Rows.Count + " products matching '" + keyword + "'");

            foreach(DataRow r in results.Rows)
            {
                Console.WriteLine("-- " + r["pdesc"] + " (RW" + r["price"] + ")");
            }
            Console.WriteLine();
        }

        public void DeleteProductTest()
        {
            DBConnect db = new DBConnect();

            Console.WriteLine("Step 5: Removing test product");

            db.ExecuteQuery("DELETE FROM tbProduct WHERE pcode='TESTPRODB1'");

            Console.WriteLine("Test product deleted\n");
        }

        public static void Main(string[] args)
        {
            ProductManagementTest test = new ProductManagementTest();
            
            Console.WriteLine("=== PRODUCT MANAGEMENT TEST ===");
            test.AddProductTest();
            test.ViewProductTest();
            test.UpdateProductTest();
            test.SearchProductTest();
            test.DeleteProductTest();
        }
    }
}
