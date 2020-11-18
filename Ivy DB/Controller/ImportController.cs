using Inventar_bearbeiten;
using Inventar_bearbeiten.Model;
using Inventar_bearbeiten.Controller;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ivy_DB.Controller
{
    class ImportController
    {
        

        public static void ImportFromExcel(string path)
        {
            

            List<ArticleModel> ArticlesFromExcel = ExcelEditor.ArticlesFromExcel(path);
            CheckAndCreateReferences(ArticlesFromExcel);
                SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
                string query = "INSERT INTO Articles (ArticleID, Description, ID_Category, Pricing, Quantity, ID_Project, SupplierPartNumber, ManufacturerPartNumber, ID_Manufacturer, ID_Supplier, ID_Status, Created, LastUpdate, ID_Location) VALUES (@ArticleID, @Description, @ID_Category, @Pricing, @Quantity, @ID_Project, @SupplierPartNumber, @ManufacturerPartNumber, @ID_Manufacturer, @ID_Supplier, @ID_Status, @Created, @LastUpdate, @ID_Location)";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                con.Open();
            foreach (ArticleModel article in ArticlesFromExcel) {
                if (ArticleController.checkSupplierAndManufacturerPartNumber(article.ManufacturerPartNumber, article.SupplierPartNumber).Count == 0)
                {
                    cmd.Parameters.AddWithValue("@ArticleID", article.ArticleID);
                    cmd.Parameters.AddWithValue("@Description", article.Description);
                    cmd.Parameters.AddWithValue("@ID_Category", getThreeDigits(article.ArticleID.ToString()));
                    cmd.Parameters.AddWithValue("@Pricing", article.Pricing);
                    cmd.Parameters.AddWithValue("@Quantity", article.Quantity);
                    cmd.Parameters.AddWithValue("@ID_Project", article.ID_Project);
                    cmd.Parameters.AddWithValue("@SupplierPartNumber", article.SupplierPartNumber);
                    cmd.Parameters.AddWithValue("@ManufacturerPartNumber", article.ManufacturerPartNumber);
                    cmd.Parameters.AddWithValue("@ID_Manufacturer", ManufacturerController.getManufacturerID(article.Manufacturer));
                    cmd.Parameters.AddWithValue("@ID_Supplier", SupplierController.getSupplierID(article.Supplier));
                    cmd.Parameters.AddWithValue("@ID_Status", StatusController.getStatusID(article.Status));
                    cmd.Parameters.AddWithValue("@Created", article.Created);
                    cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("@ID_Location", LocationController.getLocationID(article.Location));
                    cmd.ExecuteNonQuery();
                }
            }
            con.Close();
            MessageBox.Show("Import done succesfully, Please restart the Application");
            //MessageBox.Show(article.ArticleID + ": " + article.Description + " was added successfully");



        }
        public static void CheckAndCreateReferences(List<ArticleModel> ArticlesFromExcel)
        {
            
            foreach(ArticleModel article in ArticlesFromExcel)
            {
                if (ManufacturerController.getManufacturerID(article.Manufacturer) == 0)
                {
                    ManufacturerModel manufacturer = new ManufacturerModel() { Name = article.Manufacturer };
                    ManufacturerController.addManufacturer(manufacturer);
                }
                if(SupplierController.getSupplierID(article.Supplier) == 0)
                {
                    SupplierModel supplier = new SupplierModel() {Name = article.Supplier };
                    SupplierController.addSupplier(supplier);
                }
                
                if(LocationController.getLocationID(article.Location) == 0)
                {
                    LocationModel location = new LocationModel() { Location = article.Location };
                    LocationController.addLocation(location);        
                }
                if (StatusController.getStatusID(article.Status) == 0)
                {
                    StatusModel status = new StatusModel() { Status = article.Status };
                    StatusController.addStatus(status);
                }

            }

        }

        static int getThreeDigits(string number)
        {
            string digione = number[0].ToString();
            string digitwo = number[1].ToString();
            string digitthree = number[2].ToString();
            string threedigits = digione + digitwo + digitthree;
            return Convert.ToInt32(threedigits);
        }
        public void ImportFromExcel()
        {


            

 
        }




    }
}
