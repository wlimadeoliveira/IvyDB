using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using Dapper;
using Inventar_bearbeiten.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Inventar_bearbeiten.Controller
{
    class CategoryController
    {
        public static int getCategoryID(string description)
        {

            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT CategoryID FROM Categories WHERE Description = '" + description + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            var categoryID = cmd.ExecuteScalar();
            con.Close();
            if (categoryID != null)
            {
                return Convert.ToInt32(categoryID);
            }
            else
            {
                return 0;
            }


        }

        public static bool categoryExist(string categoryID)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT CategoryID FROM Categories WHERE CategoryID = '" + categoryID + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            var result = cmd.ExecuteScalar();
            con.Close();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public static List<string> searchCategory(string word)
        {
            List<string> result = new List<string>();
            List<CategoryModel> models = new List<CategoryModel>();
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT * FROM Categories Where Description like '%" + word + "%' ";
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

                    result.Add(reader[0].ToString() + " - " + reader[1].ToString());
                }
            }
            con.Close();
            return result;
        }

        public static List<CategoryModel> getCategoryList()
        {
            List<CategoryModel> models = new List<CategoryModel>();
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT * FROM Categories";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    CategoryModel model = new CategoryModel()
                    {
                        CategoryID = reader[0].ToString(),
                        Description = reader[1].ToString()
                    };
                    models.Add(model);
                }
            }
            con.Close();
            return models;
        }


        public class menuItem
        {
            public menuItem()
            {
                this.Items = new ObservableCollection<menuItem>();
            }

            public string id { get; set; }
            public string bezeichnung { get; set; }

            public ObservableCollection<menuItem> Items { get; set; }
        }

        static public menuItem getCategoryListAsTreeView()
        {
            menuItem root = new menuItem() { id = "", bezeichnung = "Categories" };
            List<CategoryModel> tmpListe = getCategoryList();
            int charToInt(char c)
            {
                return (int)(c - '0');
            }


            foreach (CategoryModel category in tmpListe)
            {
                string myID = category.CategoryID;
                string myDescription = category.Description;

                if (charToInt(myID[1]) == 0)
                {
                    root.Items.Add(new menuItem() { id = myID, bezeichnung = myDescription });
                }
                else if (charToInt(myID[2]) == 0)
                {

                    root.Items[charToInt(myID[0]) - 1].Items.Add(new menuItem() { id = myID, bezeichnung = myDescription });
                }
                else
                {
                    root.Items[charToInt(myID[0]) - 1].Items[charToInt(myID[1]) - 1].Items.Add(new menuItem() { id = myID, bezeichnung = myDescription });
                }

            }
            return root;
        }


        public static bool addCategory(CategoryModel category)
        {


            using (SQLiteConnection cnn = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString()))
            {
                cnn.Open();
                SQLiteCommand cmd = new SQLiteCommand(cnn);
                try
                {
                    cmd.CommandText = "INSERT INTO Categories (CategoryID, Description) VALUES (@CategoryID, @Description)";
                    cmd.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                    cmd.Parameters.AddWithValue("@Description", category.Description);

                    cmd.ExecuteNonQuery();

                    cnn.Close();
                    return true;
                }
                catch (SQLiteException e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }

            }




            /*  public static void copyList()
              {

                  List < KategorieSimpleModel > diesehier = CSVEditor.kategorienSimpleListe();

                  using (SQLiteConnection cnn = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString()))
                  {
                      cnn.Open();
                      SQLiteCommand cmd = new SQLiteCommand(cnn);
                      foreach (KategorieSimpleModel dies in diesehier) {
                          cmd.CommandText = "INSERT INTO Categories1 (CategoryID, Description) VALUES (@CategoryID, @Description)";
                          cmd.Parameters.AddWithValue("@CategoryID", dies.id);
                          cmd.Parameters.AddWithValue("@Description", dies.bezeichnung);

                          cmd.ExecuteNonQuery();
                      }
                      cnn.Close();

                  }

              }*/


        }
    }
}
