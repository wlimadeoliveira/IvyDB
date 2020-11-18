

using Inventar_bearbeiten.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Inventar_bearbeiten
{
    //Takes data from CSV
    class CSVEditor
    {
        List<Inventar> inv = new List<Inventar>();

        public CSVEditor() { }
        public CSVEditor(string pfad)
        {

        }




        //Holt die Kategorie Liste aus einer CSV und gibt diese als KategorieSimpleModel zurück
         public static List<KategorieSimpleModel> kategorienSimpleListe()
         {
             string directory = Directory.GetCurrentDirectory();
             List<KategorieSimpleModel> kategorieSimpleList = new List<KategorieSimpleModel>();
             string pfad = directory + @"\KategorieCSV.csv";
             StreamReader streamReader = new StreamReader(pfad,  new UTF8Encoding(true));
             String rowValue;
             String[] cellValue;
             if (System.IO.File.Exists(pfad))
             {
                 rowValue = streamReader.ReadLine();
                 cellValue = rowValue.Split(';');
             }

             while (streamReader.Peek() != -1)
             {
                 rowValue = streamReader.ReadLine();
                 cellValue = rowValue.Split(';');
                 KategorieSimpleModel model = new KategorieSimpleModel()
                 {
                     id = cellValue[0] + cellValue[1] + cellValue[2],
                     bezeichnung = cellValue[3]

                 };
                 kategorieSimpleList.Add(model);
             }
             return kategorieSimpleList;
         }

      /*  public class menuItem
        {
            public menuItem()
            {
                this.Items = new ObservableCollection<menuItem>();
            }

            public string id { get; set; }
            public string bezeichnung { get; set; }

            public ObservableCollection<menuItem> Items { get; set; }
        }*/





        //Holt die Kategorie Liste aus einer CSV und gibt diese als TreeView Items zurück
     /*   static public menuItem kts1()
        {
            menuItem root = new menuItem() { id = "", bezeichnung = "Categories" };

            string directory = Directory.GetCurrentDirectory();
            string pfad = directory + @"\KategorieCSV.csv";
            StreamReader streamReader = new StreamReader(pfad);
            string rowValue;
            string[] cellValue;
            if (System.IO.File.Exists(pfad))
            {
                rowValue = streamReader.ReadLine();
                cellValue = rowValue.Split(';');
            }
            while (streamReader.Peek() != -1)
            {

                rowValue = streamReader.ReadLine();
                cellValue = rowValue.Split(';');
                if (Convert.ToInt32(cellValue[1]) == 0)
                {
                    root.Items.Add(new menuItem() { id = cellValue[0] + cellValue[1] + cellValue[2], bezeichnung = cellValue[3] });
                }
                else if (Convert.ToInt32(cellValue[2]) == 0)
                {

                    root.Items[Convert.ToInt32(cellValue[0]) - 1].Items.Add(new menuItem() { id = cellValue[0] + cellValue[1] + cellValue[2], bezeichnung = cellValue[3] });
                }
                else
                {
                    root.Items[Convert.ToInt32(cellValue[0]) - 1].Items[Convert.ToInt32(cellValue[1]) - 1].Items.Add(new menuItem() { id = cellValue[0] + cellValue[1] + cellValue[2], bezeichnung = cellValue[3] });
                }

            }
            return root;
        }*/




        //Lauf beim Start von Programm nachdem mann ein Pfad ausgewählt hat. Holt allen Informationen aus den Inventar CSV..    
        // Und speichert diese in eine Liste als Inventar Objekten


        //Nimmt eine Liste von typ Inventar als Parameter und gibt diese in einer CSV Datei aus.



        
        public static void createCSVFromInventarList(List<ArticleModel> articles, string path, CheckBox[] checkBoxes) // Mit SaveFileDialog ergänzen
        {
            object propertyvalue = new object();
            string line = "";
            if (path != "")
            {
                using (StreamWriter sw = File.AppendText(@path)) //SaveFileDialog hier
                {
                    foreach (CheckBox cbx in checkBoxes)
                    {
                        if (cbx.IsChecked == true)
                        {

                            sw.Write(cbx.Content + ";");

                        }
                    }
                    sw.WriteLine();
                    foreach (ArticleModel article in articles)
                    {
                        foreach (CheckBox cbx in checkBoxes)
                        {
                           
                                
                                if (cbx.IsChecked == true)
                                {
                                //sw.Write(article.)
                                sw.Write(ReflectPropertyValue(article, cbx.Content.ToString()) + ";");
                                }
                        }
                        sw.WriteLine();
                    }
                }

            }



            /*   if (path != "")
               {
                   using (StreamWriter sw = File.AppendText(@path)) //SaveFileDialog hier
                   {
                       sw.WriteLine("Art No" + ";" + "Description" + ";"
                               + "Manufacturer" + ";" + "Manufacturer Part Number" + ";" + "Supplier" + ";" + "Supplier Part Number"
                               + ";" + "Pricing" + ";" + "Quantity" + ";" + "Projects"
                               + ";" + "Status" + ";" + "Created" + ";" + "Location "
                               );

                       foreach (ArticleModel inv in Inventar)
                       {

                           sw.WriteLine(inv.ArticleID + ";" +  inv.Description + ";"
                               + inv.Manufacturer + ";" + inv.ManufacturerPartNumber + ";" + inv.Supplier + ";" + inv.SupplierPartNumber
                               + ";" + inv.Pricing + ";" + inv.Quantity + ";" + inv.Project
                               + ";" + inv.Status + ";" + inv.Created + ";" + inv.Location
                               );
                       }
                   }
               }
               else
               {

               }*/
        }


 


        public static object ReflectPropertyValue(object source, string property)
        {
            try
            {
                return source.GetType().GetProperty(property).GetValue(source, null);
            } catch(Exception e)
            {
                return "";
            }
        }





    }

}
