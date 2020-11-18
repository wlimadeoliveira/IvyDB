using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using Inventar_bearbeiten;
using System.Collections.ObjectModel;
using System.IO;
using Inventar_bearbeiten.Model;
using Inventar_bearbeiten.Controller;

namespace Ivy_DB.Controller
{
  
    class ExcelEditor
    {
        static _Application excel = new _Excel.Application();
        static Workbook  wb;
        public static Worksheet ws;
        public static ObservableCollection<Inventar> InventarListe = new ObservableCollection<Inventar>();
         
 
        public ExcelEditor()
        {
            

        }

        public static List<ArticleModel> ArticlesFromExcel(string path)
        {
            List<ArticleModel> ArticleFromExcel = new List<ArticleModel>();
            openExcelFile(path);
            Range range = ws.UsedRange;
            object[,] values = range.Value2;
            for (int i = 8; values[i, 2] != null; i++) //Muss noch versuchen letzte zeile zu greifen --> zeilen länge temporär auf 100;
            {
                

                ArticleModel model = new ArticleModel()
                {
                    ArticleID = Convert.ToInt32(values[i, 2]),
                    Description = Convert.ToString(values[i, 3]),
                    Manufacturer = Convert.ToString(values[i, 4]),
                    ManufacturerPartNumber = Convert.ToString(values[i, 5]),
                    Supplier = Convert.ToString(values[i, 6]),
                    SupplierPartNumber = Convert.ToString(values[i, 7]),
                    Pricing = CheckEmptyDoubleValue(values[i, 8]),
                    Quantity = CheckEmptyIntValue(values[i, 9]),                   
                    Project = Convert.ToString(values[i, 10]),
                    Status = Convert.ToString(values[i, 11]),
                    Created = tryDateTimeConvertionFromExcel(values[i, 12]).ToString("dd/MM/yyyy"),                  
                    Location = Convert.ToString(values[i, 14]),
                };

                ArticleFromExcel.Add(model);
            }
            wb.Close();
            excel.Quit();
            return ArticleFromExcel;
           

           
        }

        public static int CheckEmptyIntValue(object value)
        {
            if(Convert.ToString(value) == "" || value == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }
        public static  double CheckEmptyDoubleValue(object value)
        {
            if (Convert.ToString(value) == "" || value == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(value);
            }
        }

        public static void openExcelFile(string pfad)
        {
         
            if (IsClose(pfad))
            {
                wb = excel.Workbooks.Open(pfad);
                ws = wb.Worksheets[1];
         
            }
            else
            {
                MessageBox.Show("File is already in use...Application will be closed");
                System.Windows.Application.Current.Shutdown();
            }
        }

        static bool IsClose(string pfad)
        {
            try
            {
                Stream s = File.Open(pfad, FileMode.Open, FileAccess.Read, FileShare.None);

                s.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ToExcelFile()
        {
            
            string directory = Directory.GetCurrentDirectory();
            string path = directory + @"\StockManagement.xlsx";
            openExcelFile(path);
            List<ArticleModel> myList = ArticleController.GetInventoryList();
            int row = 8;
            SaveFileDialog savePath = new SaveFileDialog();
            savePath.Filter = "Excel File|*.xlsx";
            savePath.Title = "Export list as Excel File";
            savePath.ShowDialog();
            
            
            foreach (ArticleModel article in myList)
            {

                ws.Cells[row, 2].Value2 = article.ArticleID;
                ws.Cells[row, 3].Value2 = article.Description;
                ws.Cells[row, 4].Value2 = article.Manufacturer;
                ws.Cells[row, 5].Value2 = article.ManufacturerPartNumber;
                ws.Cells[row, 6].Value2 = article.Supplier;
                ws.Cells[row, 7].Value2 = article.SupplierPartNumber;
                ws.Cells[row, 8].Value2 = article.Pricing;
                ws.Cells[row, 9].Value2 = article.Quantity;

                ws.Cells[row, 11].Value2 = article.Project;
                ws.Cells[row, 12].Value2 = article.Status;
                ws.Cells[row, 13].Value2 = article.Created;
                
                ws.Cells[row, 15].Value2 = article.Location;
                row++;

            }             

         
            wb.SaveAs(savePath.FileName);
            wb.Close();

            openGeneratedFile(savePath.FileName);

        }
        public static DateTime tryDateTimeConvertionFromExcel(object dateFromExcel)
        {
            try
            {
                string dateString = Convert.ToString(dateFromExcel);
                double dateDouble = Convert.ToDouble(dateString);
                return DateTime.FromOADate(dateDouble).Date;
            }
            catch (Exception e)
            {
                if (dateFromExcel != null)
                {
                    return Convert.ToDateTime(Convert.ToString(dateFromExcel));
                }
                else
                {

                    return DateTime.Today;
                }
            }
        }

        public static  void openGeneratedFile(string path)
        {
            string msgBoxText = "Do you want to open the generated StockManagement File ?";
            MessageBoxButton btnConfirmToOpen = MessageBoxButton.YesNo;
            MessageBoxImage msgImage = MessageBoxImage.Question;
            MessageBoxResult rsltMessageBox = MessageBox.Show(msgBoxText, path, btnConfirmToOpen, msgImage);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    System.Diagnostics.Process.Start(@path);
                    break;
                case MessageBoxResult.No:
                    break;
            }

        }



        public void save(string savePath)
        {
            wb.SaveAs(savePath);
        }

        public void close()
        {
            wb.Close();
            excel.Quit();
        }







    }
}
