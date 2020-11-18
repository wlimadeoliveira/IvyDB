using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Inventar_bearbeiten.Model;

namespace Inventar_bearbeiten.Controller
{
    class LocationController
    {
        public static int getLocationID(string Location)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT LocationID FROM Locations WHERE Location = '" + Location + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            var locationID = cmd.ExecuteScalar();
            con.Close();
            if (locationID != null)
            {
                return Convert.ToInt32(locationID);
            }
            else
            {
                return 0;
            }

        }

        public static List<LocationModel> getLocationList()
        {
            List<LocationModel> locationModels = new List<LocationModel>();
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT * FROM Locations";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    LocationModel model = new LocationModel()
                    {
                        LocationID = Convert.ToInt32(reader[0]),
                        Location = reader[1].ToString()
                    };
                    locationModels.Add(model);
                }
                con.Close();
                return locationModels;
            }
        }


        public static void addLocation(LocationModel location)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            string query = "INSERT INTO Locations (Location) VALUES(@Location);";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            con.Open();
            cmd.Parameters.AddWithValue("@Location", location.Location);
        
            cmd.ExecuteNonQuery();
            con.Close();
        }




    }
}
