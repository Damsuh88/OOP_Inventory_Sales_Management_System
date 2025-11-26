using System;
using System.Data;
using POSales;

namespace POSales.TestCode
{
    public class DatabaseConnectionTest
    {
        public void TestConnection()
        {
            DBConnect db = new DBConnect();
            Console.WriteLine("Testing database connection...\n");
            DataTable products = db.getTable("SELECT * FROM tbProduct");
            if(products.Rows.Count > 0)
            {
                Console.WriteLine("Connection successful!");
                Console.WriteLine("Found " + products.Rows.Count + " products in database\n");
                Console.WriteLine("Sample products:");
                for(int i = 0; i < 3 && i < products.Rows.Count; i++)
                {
                    Console.WriteLine((i+1) + ". " + products.Rows[i]["pdesc"] + " - RW" + products.Rows[i]["price"]);
                }
            }
            else
            {
                Console.WriteLine("Connection works but no products found");
            }
        }

        public void TestUserAuthentication()
        {
            DBConnect db = new DBConnect();
            Console.WriteLine("\nTesting user authentication...");
            string testUser = "admin";
            string pwd = db.getPassword(testUser);
            if(pwd != null && pwd != "")
            {
                Console.WriteLine("User '" + testUser + "' found in system");
                Console.WriteLine("Authentication check passed");
            }
            else
            {
                Console.WriteLine("User not found or password empty");
            }
        }

        public void TestDataExtraction()
        {
            DBConnect db = new DBConnect();
            Console.WriteLine("\nTesting data extraction...");
            double totalSales = db.ExtractData("SELECT SUM(total) FROM tbCart WHERE status='Sold'");
            Console.WriteLine("Total sales amount: RW" + totalSales.ToString("F2"));
        }

        public static void Main(string[] args)
        {
            DatabaseConnectionTest test = new DatabaseConnectionTest();
            
            Console.WriteLine("=== DATABASE CONNECTION TEST ===");
            test.TestConnection();
            test.TestUserAuthentication();
            test.TestDataExtraction();
        }
    }
}
