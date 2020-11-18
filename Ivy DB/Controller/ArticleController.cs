using Inventar_bearbeiten.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using Dapper;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Ivy_DB.Controller;

namespace Inventar_bearbeiten.Controller
{
    class ArticleController
    {


        public static void addHyperLinkAndProjects(List<ArticleModel> inv)
        {
            foreach (ArticleModel article in inv)
            {
                if (article.Supplier == "Mouser")
                {
                    article.SupplierOrGoogleLink = "https://www.mouser.ch/Search/Refine?Keyword=" + article.SupplierPartNumber;
                }
                else if (article.Supplier == "Digi-Key")
                {
                    article.SupplierOrGoogleLink = "https://www.digikey.com/products/en?keywords=" + article.SupplierPartNumber;
                }
                else if (article.Supplier == "Distrelec")
                {
                    article.SupplierOrGoogleLink = "https://www.distrelec.ch/search?q=" + article.SupplierPartNumber;
                }
                else if (article.Manufacturer == "Mini-Circuits")
                {
                    article.SupplierOrGoogleLink = "https://ww2.minicircuits.com/WebStore/modelSearch.html?model=" + article.ManufacturerPartNumber + "&search=1";
                }
                else
                {
                    article.SupplierOrGoogleLink = "https://www.google.com/search?q=" + article.SupplierPartNumber + " " + article.ManufacturerPartNumber + " " + article.Supplier + " " + article.Manufacturer;
                }
                article.Project = Article_ProjectController.getProjects(article.ArticleID.ToString());
            }
            
        }




        public static List<ArticleModel> GetInventoryList()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString()))
            {
                 var output = cnn.Query<ArticleModel>("SELECT Article.ArticleID, Article.Description ,  Manufacturer.name AS Manufacturer,Article.ManufacturerPartNumber, Supplier.name AS Supplier, Article.SupplierPartNumber , Location.Location AS Location, Article.Quantity, Article.Created, Article.LastUpdate, Article.Pricing FROM Articles AS Article"
                                                        + "  left JOIN Manufacturers AS Manufacturer ON Article.ID_Manufacturer = Manufacturer.ManufacturerID"
                                                        + "  left JOIN Suppliers AS Supplier ON Article.ID_Supplier = Supplier.SupplierID"
                                                        + "  left JOIN Locations AS Location ON Article.ID_Location = Location.LocationID; ", new DynamicParameters());
                
                cnn.Close();
                addHyperLinkAndProjects(output.ToList());
                return output.ToList();

            }               
        }

        public static List<ArticleModel> GetAdvancedSearchList(CheckBox[] cbxs, string input)
        {
            using (SQLiteConnection cnn = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString()))
            {
                var output = cnn.Query<ArticleModel>(ExpertSmartSearch.advancedQuery(cbxs,input), new DynamicParameters());

                cnn.Close();
                addHyperLinkAndProjects(output.ToList());
                return output.ToList();
            }
        }

        public static dynamic checkIfEmpty(dynamic value)
        {
            if (value is string)
            {
                if (value == "")
                {
                    return "NULL";
                }
                else
                {
                    return value;
                }
            }
            else
            {
                if (value == null)
                {
                    return 0;
                }
                else
                {
                    return value;
                }
            }

        }
        public static int checkIfEmptyNumber(dynamic value)
        {


            if (value == null)
            {
                return 0;
            }
            else
            {
                return value;
            }

        }


        public static void archivInsert(ArticleModel article)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "INSERT INTO Archiv_Article( " +
                "old_ArticleID, old_Description, " +
                "old_Category, old_Pricing, " +
                "old_Projects, " +
                "old_SupplierPartNumber, " +
                "old_ManufacturerPartNumber, " +
                "old_Manufacturer, " +
                "old_Supplier, " +
                "old_Status," +
                "old_Created, " +
                "old_Deleted) " +
                "VALUES(" +
                "@old_ArticleID, @old_Description, " +
                "@old_Category, @old_Pricing, " +
                "@old_Projects, " +
                "@old_SupplierPartNumber, " +
                "@old_ManufacturerPartNumber, " +
                "@old_Manufacturer, " +
                "@old_Supplier, " +
                "@old_Status," +
                "@old_Created, " +
                "@old_Deleted )";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            cmd.Parameters.AddWithValue("@old_ArticleID",article.ArticleID);
            cmd.Parameters.AddWithValue("@old_Description", article.Description);
            cmd.Parameters.AddWithValue("@old_Category", article.Category);
            cmd.Parameters.AddWithValue("@old_Pricing",article.Pricing);
            cmd.Parameters.AddWithValue("@old_Projects",Article_ProjectController.getProjects(article.ArticleID.ToString()));
            cmd.Parameters.AddWithValue("@old_SupplierPartNumber",article.SupplierPartNumber);
            cmd.Parameters.AddWithValue("@old_ManufacturerPartNumber",article.ManufacturerPartNumber);
            cmd.Parameters.AddWithValue("@old_Manufacturer",article.Manufacturer);
            cmd.Parameters.AddWithValue("@old_Supplier",article.Supplier);
            cmd.Parameters.AddWithValue("@old_Status",article.Status);
            cmd.Parameters.AddWithValue("@old_Created",article.Created);
            cmd.Parameters.AddWithValue("@old_Created",article.Created);
            cmd.Parameters.AddWithValue("@old_Deleted",DateTime.Now.ToString("dd/MM/yyyy"));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void deleteArticle(ArticleModel article)
        {
            archivInsert(article);
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "DELETE FROM Articles WHERE ArticleID = @ArticleID ";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            cmd.Parameters.AddWithValue("@ArticleID", article.ArticleID);
            cmd.ExecuteNonQuery();
            con.Close();     
        }


        public static void updateArticle(ArticleModel am)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "UPDATE Articles SET Description = @Description"
                + ", Pricing = @Pricing"
                + ", ID_Project = @ID_Project"
                + ", SupplierPartNumber = @SupplierPartNumber "
                + ", ManufacturerPartNumber = @ManufacturerPartNumber"
                + ", ID_Manufacturer = @ID_Manufacturer"
                + ", ID_Supplier = @ID_Supplier"
                + ", ID_Status = @ID_Status"
                + ", LastUpdate = @LastUpdate"
                + ", ID_Location = @ID_Location"
                + ", Quantity = @Quantity"
                + " WHERE ArticleID = @ArticleID";


            SQLiteCommand cmd = new SQLiteCommand(query, con);
            cmd.Parameters.AddWithValue("@Description", am.Description);
            cmd.Parameters.AddWithValue("@Pricing", am.Pricing);
            cmd.Parameters.AddWithValue("@ID_Project", am.ID_Project);
            cmd.Parameters.AddWithValue("@SupplierPartNumber", am.SupplierPartNumber);
            cmd.Parameters.AddWithValue("@ManufacturerPartNumber", am.ManufacturerPartNumber);
            cmd.Parameters.AddWithValue("@ID_Manufacturer", am.ID_Manufacturer);
            cmd.Parameters.AddWithValue("@ID_Supplier", am.ID_Supplier);
            cmd.Parameters.AddWithValue("@ID_Status", am.ID_Status);
            cmd.Parameters.AddWithValue("@LastUpdate", am.LastUpdate);
            cmd.Parameters.AddWithValue("@ID_Location", am.ID_Location);
            cmd.Parameters.AddWithValue("@Quantity", am.Quantity);
            cmd.Parameters.AddWithValue("@ArticleID", am.ArticleID);

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show(am.ArticleID + ": " + am.Description + " was successfully updated");
        }


        /* public static void addNewArticle(ArticleModel article)
         {
             using (IDbConnection cnn = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString()))
             {
                 cnn.Execute("INSERT INTO Articles (Description, ID_Category, Pricing, Quantity,ID_Project,SupplierPartNumber,ManufacturerPartNumber, ID_Manufacturer, ID_Supplier, ID_Status, Created, LastUpdate, ID_Location) VALUES (@Description, @ID_Category, @Pricing, @Quantity, @ID_Project, @SupplierPartNumber, @ManufacturerPartNumber, @ID_Manufacturer, @ID_Supplier, @ID_Status, @Created, @LastUpdate, @ID_Location)", article);            
             }
         }*/

        public static int generateNewArticleNumber(int categoryNumber)
        {
            List<int> articlesInThisCategory = new List<int>();
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT ArticleID FROM Articles WHERE ArticleID >= " + categoryNumber + "000" + " AND ArticleID < " + categoryNumber + "999" + "  ";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            int missingInSequence = 99999999;
            int newArticleID;
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

                    articlesInThisCategory.Add(Convert.ToInt32(reader[0]));

                }
                
                for (int i = 0; i < articlesInThisCategory.Count; i++)
                {
                    if (articlesInThisCategory.Count < 1) break;
                    if (articlesInThisCategory.Count == 1)
                    {
                        missingInSequence = articlesInThisCategory[i] + 1;
                        break;
                    }
                    if(articlesInThisCategory[i + 1] - articlesInThisCategory[i]   == 2)
                    {
                        missingInSequence = articlesInThisCategory[i] + 1;
                        break;
                    }else if (i == articlesInThisCategory.Count -2)
                    {
                        break;
                    }

                }
                
                
            }
            con.Close();
            if (articlesInThisCategory.Count() >= 1)
            {
                int lastArticleID = articlesInThisCategory.ElementAt(articlesInThisCategory.Count - 1);
                if (missingInSequence < lastArticleID)
                {
                    newArticleID = missingInSequence;
                }
                else{
                    newArticleID = lastArticleID + 1;
                }
            }
            else
            {
                string newCategory = categoryNumber.ToString() + "000";
                newArticleID = Convert.ToInt32(newCategory);
            }
            
            return newArticleID;

        }

        public static void addNewArticle(ArticleModel article)
        {
           
                SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
                string query = "INSERT INTO Articles (ArticleID, Description, ID_Category, Pricing, Quantity, ID_Project, SupplierPartNumber, ManufacturerPartNumber, ID_Manufacturer, ID_Supplier, ID_Status, Created, LastUpdate, ID_Location) VALUES (@ArticleID, @Description, @ID_Category, @Pricing, @Quantity, @ID_Project, @SupplierPartNumber, @ManufacturerPartNumber, @ID_Manufacturer, @ID_Supplier, @ID_Status, @Created, @LastUpdate, @ID_Location)";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@ArticleID", article.ArticleID);
                cmd.Parameters.AddWithValue("@Description", article.Description);
                cmd.Parameters.AddWithValue("@ID_Category", article.ID_Category);
                cmd.Parameters.AddWithValue("@Pricing", article.Pricing);
                cmd.Parameters.AddWithValue("@Quantity", article.Quantity);
                cmd.Parameters.AddWithValue("@ID_Project", article.ID_Project);
                cmd.Parameters.AddWithValue("@SupplierPartNumber", article.SupplierPartNumber);
                cmd.Parameters.AddWithValue("@ManufacturerPartNumber", article.ManufacturerPartNumber);
                cmd.Parameters.AddWithValue("@ID_Manufacturer", article.ID_Manufacturer);
                cmd.Parameters.AddWithValue("@ID_Supplier", article.ID_Supplier);
                cmd.Parameters.AddWithValue("@ID_Status", article.ID_Status);
                cmd.Parameters.AddWithValue("@Created", article.Created);
                cmd.Parameters.AddWithValue("@LastUpdate", article.LastUpdate);
                cmd.Parameters.AddWithValue("@ID_Location", article.ID_Location);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show(article.ArticleID + ": " + article.Description + " was added successfully");
            
           


            /*cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@ArticleID", 9000);
            cmd.Parameters.AddWithValue("@Description", "LALALALA");
            cmd.Parameters.AddWithValue("@ID_Category", 121);
            cmd.Parameters.AddWithValue("@Pricing", 12.2);
            cmd.Parameters.AddWithValue("@Quantity", 12);
            cmd.Parameters.AddWithValue("@ID_Projects", 1);
            cmd.Parameters.AddWithValue("@SupplierPartNumber", "asdasdasdas");
            cmd.Parameters.AddWithValue("@ManufacturerPartNumber", "asdasdasd");
            cmd.Parameters.AddWithValue("@ID_Manufacturer", 1);
            cmd.Parameters.AddWithValue("@ID_Supplier", "asdasdasdasd");
            cmd.Parameters.AddWithValue("@ID_Status", 1);
            cmd.Parameters.AddWithValue("@Created", "12/12/2018");
            cmd.Parameters.AddWithValue("@LastUpdate", "12/12/2018");
            cmd.Parameters.AddWithValue("@ID_Location", 1);*/
           
        }


        public static List<ArticleModel> myFoundArticles(string columnName, string attr)
        {
            using (SQLiteConnection cnn = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString()))
            {
                var output = cnn.Query<ArticleModel>("SELECT Article.ArticleID, Article.Description ,  Manufacturer.name AS Manufacturer,Article.ManufacturerPartNumber, Supplier.name AS Supplier, Article.SupplierPartNumber , Location.Location AS Location, Article.Quantity, Article.Created, Article.LastUpdate, Article.Pricing FROM Articles AS Article"
                                                           + " left JOIN Manufacturers AS Manufacturer ON Article.ID_Manufacturer = Manufacturer.ManufacturerID "
                                                           + " left JOIN Suppliers AS Supplier ON Article.ID_Supplier = Supplier.SupplierID"
                                                           + " left JOIN Locations AS Location ON Article.ID_Location = Location.LocationID WHERE " + columnName + " Like  '%" + attr + "%'", new DynamicParameters());
                cnn.Close();
                return output.ToList();
            }
        }

        public static ObservableCollection<ArticleModel> FoundArticles(string myAttrs, CheckBox[] checkBoxes)
        {
            List<ArticleModel> allFoundArticles = new List<ArticleModel>();
            string[] myAttributes = myAttrs.Split(' ');
            ObservableCollection<ArticleModel> result;
            int counter = 0;
            if (myAttributes.Length > 0)
            {
                foreach (CheckBox cbx in checkBoxes)
                {
                    if (cbx.IsChecked == true)
                    {
                        foreach (string attr in myAttributes)
                        {
                            allFoundArticles = allFoundArticles.Union(myFoundArticles(cbx.Content.ToString(), attr)).ToList();
                        }
                    }
                    else
                    {
                        counter++;
                    }
                }
                result = new ObservableCollection<ArticleModel>(allFoundArticles);
            }
            else
            {
                result = new ObservableCollection<ArticleModel>(GetInventoryList());

            }
            if(myAttributes[0] == "" && counter == 7)
            {
                result = new ObservableCollection<ArticleModel>(GetInventoryList());

            }
            return result;
        }



        public static List<ArticleModel> checkSupplierAndManufacturerPartNumber(string manufacturerPartNumber,string supplierPartNumber)
        {

            List<ArticleModel> duplicateArticles = new List<ArticleModel>();
                using (SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString()))
                {
                con.Open();
                    string query = "SELECT ArticleID, ManufacturerPartNumber, SupplierPartNumber FROM Articles WHERE ManufacturerPartNumber IS @ManufacturerPartNumber OR SupplierPartNumber IS @SupplierPartNumber";
                    SQLiteCommand cmd = new SQLiteCommand(query, con);
                    cmd.Parameters.AddWithValue("@ManufacturerPartNumber", manufacturerPartNumber);
                    cmd.Parameters.AddWithValue("@SupplierPartNumber", supplierPartNumber);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ArticleModel article = new ArticleModel()
                        {
                            ArticleID = Convert.ToInt32(reader[0].ToString()),
                            ManufacturerPartNumber = reader[1].ToString(),
                            SupplierPartNumber = reader[2].ToString()
                        };
                        duplicateArticles.Add(article);
                    }
                }
                con.Close();
                return duplicateArticles;
             
                    
                    
                }       
        }








        /*public void InsertArticle(ArticleModel model)
        {
            MySqlConnection con = new MySqlConnection(cs);
            string query = "INSERT INTO `article`(`Description`, `Pricing`, `Quantity`, `Quantity Avaible`, `Projects`, `Status`, `ItemGroup`, `Location`, `ManufacturerPartNumber`, `SupplierPartNumber`, `ID_Supplier`, `ID_Manufacturer`) VALUES(@Description, @Pricing, @Quantity, @QuantityAvaible, @Projects, @Status, @ItemGroup, @Location, @ManufacturerPartNumber, @SupplierPartNumber, @ID_Supplier, @ID_Manufacturer);";
            //string query = "INSERT INTO 'Article'('Description', 'Pricing', 'Quantity', 'SupplierPartNumber', 'ManufacturerPartNumber', 'Quantity Avaible', 'Projects', 'Status', 'ID_Manufacturer', 'ID_Supplier') VALUES('@Description', @Pricing, @Quantity, '@SupplierPartNumber', '@ManufacturerPartNumber', @QuantityAvaible, '@Projects', '@Status', '@ID_Manufacturer', '@ID_Supplier');";
            MySqlCommand cmd = new MySqlCommand(query, con);
            con.Open();
            cmd.Parameters.AddWithValue("@Description", model.description);
            cmd.Parameters.AddWithValue("@Pricing", model.pricing);
            cmd.Parameters.AddWithValue("@Quantity", model.quantity);
            cmd.Parameters.AddWithValue("@SupplierPartNumber", model.SupplierPartNumber);
            cmd.Parameters.AddWithValue("@ManufacturerPartNumber", model.ManufacturerPartNumber);
            cmd.Parameters.AddWithValue("@QuantityAvaible", model.quantityAvaible);
            cmd.Parameters.AddWithValue("@Projects", model.projects);
            cmd.Parameters.AddWithValue("@Status", model.status);
            cmd.Parameters.AddWithValue("@ItemGroup", model.ItemGroup);
            cmd.Parameters.AddWithValue("@Location", model.location);
            cmd.Parameters.AddWithValue("@ID_Manufacturer", model.ID_Manufacturer);
            cmd.Parameters.AddWithValue("@ID_Supplier", model.ID_Supplier);

            cmd.ExecuteNonQuery();
            con.Close();

        }*/



    }
}
