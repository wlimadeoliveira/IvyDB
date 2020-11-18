using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventar_bearbeiten.Model;

using System.Data.SQLite;

namespace Inventar_bearbeiten.Controller
{
    class SupplierController
    {


        public static void addSupplier(SupplierModel supplier)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            string query = "INSERT INTO Suppliers (Name, ClientNumber, WebAddress) VALUES(@Name,@ClientNumber, @WebAddress);";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            con.Open();
            cmd.Parameters.AddWithValue("@Name", supplier.Name);
            cmd.Parameters.AddWithValue("@ClientNumber", supplier.webAddress);
            cmd.Parameters.AddWithValue("@WebAddress", supplier.clientNumber);
            cmd.ExecuteNonQuery();
            con.Close();
        }




        public static List<string> searchSupplier(string word)
        {
            List<string> result = new List<string>();

            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT name FROM Suppliers Where name like '%" + word + "%' ";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    /* CategoryModel model = new CategoryModel()
                     {
                         CategoryID = reader[0].ToString(),
                         Description = reader[1].ToString()
                     };
                     models.Add(model);*/

                    result.Add(reader[0].ToString());
                }
            }
            con.Close();
            return result;
        }


        public static int getSupplierID(string supplierName)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT SupplierID FROM Suppliers WHERE Name = '" + supplierName + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            var supplierID = cmd.ExecuteScalar();
            con.Close();
            if (supplierID != null)
            {
                return Convert.ToInt32(supplierID);
            }
            else
            {
                return 0;
            }

        }

        public static List<SupplierModel> getSupplierList()
        {
            List<SupplierModel> supplierModels = new List<SupplierModel>();
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT * FROM Suppliers Order by name asc;";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    SupplierModel model = new SupplierModel()
                    {
                        supplierID = Convert.ToInt32(reader[0]),
                        Name = reader[1].ToString(),
                        clientNumber = reader[2].ToString(),
                        webAddress = reader[3].ToString()
                    };
                    supplierModels.Add(model);
                }
                con.Close();
                return supplierModels;
            }
        }



    }
}
