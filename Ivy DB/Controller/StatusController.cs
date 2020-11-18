using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventar_bearbeiten.Model;

namespace Inventar_bearbeiten.Controller
{
    class StatusController
    {
        public static int getStatusID(string status)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT StatusID FROM Status WHERE Status = '" + status + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            var statusID = cmd.ExecuteScalar();
            con.Close();
            if (statusID != null)
            {
                return Convert.ToInt32(statusID);
            }
            else
            {
                return 0;
            }
        }


        public static List<StatusModel> StatusList()
        {
            List<StatusModel> statusModels = new List<StatusModel>();
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT * FROM Status";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    StatusModel model = new StatusModel()
                    {
                        StatusID = Convert.ToInt32(reader[0]),
                        Status = reader[1].ToString()
                    };
                    statusModels.Add(model);
                }
                con.Close();
                return statusModels;
            }

        }

        internal static void addStatus(StatusModel model)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            string query = "INSERT INTO Status (Status) VALUES(@Status);";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            con.Open();
            cmd.Parameters.AddWithValue("@Status", model.Status);

            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
