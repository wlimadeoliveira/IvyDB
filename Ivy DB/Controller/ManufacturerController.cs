using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using Inventar_bearbeiten.Model;


namespace Inventar_bearbeiten.Controller
{
    class ManufacturerController
    {

        public static List<string> searchManufacturer(string word)
        {
            List<string> result = new List<string>();

            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT name FROM Manufacturers Where name like '%" + word + "%' ";
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

        public static void addManufacturer(ManufacturerModel manufacturer)
        {

            using (SQLiteConnection cnn = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString()))
            {
                cnn.Open();
                SQLiteCommand cmd = new SQLiteCommand(cnn);
                cmd.CommandText = "INSERT INTO Manufacturers (Name, ClientNumber, WebAddress) VALUES (@Name, @ClientNumber, @WebAddress)";
                cmd.Parameters.AddWithValue("@Name", manufacturer.Name);
                cmd.Parameters.AddWithValue("@ClientNumber", manufacturer.ClientNumber);
                cmd.Parameters.AddWithValue("@WebAddress", manufacturer.WebAddress);
                cmd.ExecuteNonQuery();
            }
        }





        public static int getManufacturerID(string manufacturerName)
        {

            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT ManufacturerID FROM Manufacturers WHERE name = '" + manufacturerName + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            var manufacturerID = cmd.ExecuteScalar();
            con.Close();
            if (manufacturerID != null)
            {
                return Convert.ToInt32(manufacturerID);
            }
            else
            {
                return 0;
            }


        }

        public static List<ManufacturerModel> getManufacturerList()
        {
            List<ManufacturerModel> manufacturerModels = new List<ManufacturerModel>();
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT * FROM Manufacturers Order by name asc";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ManufacturerModel model = new ManufacturerModel()
                    {
                        ManufacturerID = Convert.ToInt32(reader[0]),
                        Name = reader[1].ToString(),
                        ClientNumber = reader[2].ToString(),
                        WebAddress = reader[3].ToString()
                    };
                    manufacturerModels.Add(model);
                }
                con.Close();
                return manufacturerModels;


            }
        }











    }
}
