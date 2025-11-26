using System;
using System.Data;
using POSales;

namespace POSales.TestCode
{
    public class SupplierManagementTest
    {
        public void AddSupplierTest()
        {
            DBConnect db = new DBConnect();
            Console.WriteLine("Supplier Management Test\n");
            Console.WriteLine("Adding new supplier...");
            string insert = "INSERT INTO tbSupplier (supplier, address, contactperson, telephone, email, fax) " + 
                          "VALUES ('Kedai Runcit Sdn Bhd', 'No 45 Jalan Ipoh', 'Ahmad bin Ali', " +
                          "'012-3456789', 'ahmad@kedairuncit.com', '05-5551234')";
            db.ExecuteQuery(insert);
            Console.WriteLine("Supplier added!\n");
        }

        public void ViewSuppliersTest()
        {
            DBConnect db = new DBConnect();
            Console.WriteLine("Current suppliers list:");
            DataTable suppliers = db.getTable("SELECT * FROM tbSupplier");
            int num = 1;
            foreach(DataRow s in suppliers.Rows)
            {
                Console.WriteLine(num + ". " + s["supplier"]);
                Console.WriteLine("   Contact: " + s["contactperson"] + " (" + s["telephone"] + ")");
                Console.WriteLine("   Email: " + s["email"]);
                num++;
            }
            Console.WriteLine();
        }

        public void UpdateSupplierTest()
        {
            DBConnect db = new DBConnect();
            Console.WriteLine("Updating supplier contact info...");
            string update = "UPDATE tbSupplier SET telephone='013-9998888', " + 
                          "email='ahmad.new@kedairuncit.com' " +
                          "WHERE supplier='Kedai Runcit Sdn Bhd'";
            db.ExecuteQuery(update);
            Console.WriteLine("Contact info updated\n");
        }

        public void DeleteSupplierTest()
        {
            DBConnect db = new DBConnect();
            Console.WriteLine("Removing test supplier...");
            db.ExecuteQuery("DELETE FROM tbSupplier WHERE supplier='Kedai Runcit Sdn Bhd'");
            Console.WriteLine("Supplier removed from system");
        }

        public static void Main(string[] args)
        {
            SupplierManagementTest test = new SupplierManagementTest();
            
            Console.WriteLine("=== SUPPLIER MANAGEMENT TEST ===");
            test.AddSupplierTest();
            test.ViewSuppliersTest();
            test.UpdateSupplierTest();
            test.DeleteSupplierTest();
        }
    }
}
