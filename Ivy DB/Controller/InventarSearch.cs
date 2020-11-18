using Inventar_bearbeiten.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Inventar_bearbeiten
{
    public class InventarSearch
    {
        List<Inventar> inv;
        CSVEditor csvEditor = new CSVEditor();
        List<KategorieSimpleModel> kategorieSimpleList;

        public InventarSearch(List<Inventar> inv)
        {
            this.inv = inv;
            // this.kategorieSimpleList = csvEditor.kategorienSimpleListe();

        }

        public InventarSearch()
        {
            // this.kategorieSimpleList = csvEditor.kategorienSimpleListe();
        }

        //Search only by manufacturer number - method is not in use.. --> see ExcelEditor findArticlebyNumber
        public void searchByManufacturerNumber(string number)
        {
            string suchFeld = number;
            bool found = false;
            foreach (Inventar inventar in inv)
            {
                if (suchFeld == inventar.ManufacturerPartNumber)
                {
                    MessageBox.Show("Found! ArtNo:" + inventar.ArticleID);
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                MessageBox.Show("not found!");
            }
        }


        // At Event Textbox content Changed, fill the small suggestion list
        public List<String> autoComplete(TextBox textBox)
        {
            string text = textBox.Text;
            int txtLength = text.Length;
            string textToSearch = text.Substring(0, txtLength);
            List<string> found = new List<string>();


            if (text != null || text != "")
            {
                if (textBox.Name == "txtManufacturer" || textBox.Name == "txtSupplier")
                    foreach (Inventar model in inv)
                    {
                        switch (textBox.Name)
                        {
                            case "txtSupplier":
                                if (model.Supplier.Length >= txtLength && model.Supplier.Substring(0, txtLength).ToLower() == text.ToLower())
                                {
                                    if (!found.Contains(model.Supplier) && model.Supplier != null)
                                    {
                                        found.Add(model.Supplier);
                                    }
                                }
                                break;
                            case "txtManufacturer":
                                if (model.Manufacturer.Length >= txtLength && model.Manufacturer.Substring(0, txtLength).ToLower() == text.ToLower())
                                {
                                    if (!found.Contains(model.Manufacturer) && model.Manufacturer != null)
                                    {
                                        found.Add(model.Manufacturer);
                                    }
                                }
                                break;
                        }
                    }
                if (textBox.Name == "txtArticleType")
                {
                    foreach (KategorieSimpleModel kategorie in kategorieSimpleList)
                    {
                        if (kategorie.id.Length >= txtLength && kategorie.id.Substring(0, txtLength) == text.ToLower())
                        {
                            if (!found.Contains(kategorie.id) && kategorie.id != null)
                            {
                                found.Add(kategorie.id + " - " + kategorie.bezeichnung);
                            }
                        }
                    }
                }

            }

            return found;
        }


        //Fill the full suggetion list for Manufacturer or Supplier name
        public List<String> suggestionfullList(TextBox textBox)
        {
            string text = textBox.Text;
            int txtLength = text.Length;
            string textToSearch = text.Substring(0, txtLength);
            List<string> found = new List<string>();
            if (text != null || text != "")
            {
                if (textBox.Name == "txtManufacturer" || textBox.Name == "txtSupplier")
                    foreach (Inventar model in inv)
                    {
                        switch (textBox.Name)
                        {
                            case "txtSupplier":
                                if (!found.Contains(model.Supplier) && model.Supplier != null)
                                {
                                    found.Add(model.Supplier);
                                }
                                break;
                            case "txtManufacturer":
                                if (!found.Contains(model.Manufacturer) && model.Manufacturer != null)
                                {
                                    found.Add(model.Manufacturer);
                                }
                                break;
                        }
                    }
                if (textBox.Name == "txtArticleType")
                {
                    foreach (KategorieSimpleModel kategorie in kategorieSimpleList)
                    {
                        if (!found.Contains(kategorie.id) && kategorie.id != null)
                        {
                            found.Add(kategorie.id + " - " + kategorie.bezeichnung);
                        }
                    }
                }
            }
            return found;
        }


    }
}
