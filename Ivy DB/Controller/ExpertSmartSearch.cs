using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ivy_DB.Controller
{
    class ExpertSmartSearch
    {

        public static string[] splitInput(string input)
        {
            string[] keywords;
            keywords = input.ToLower().Split(' ');
            return keywords;
        }
 


        public static string generateQueryPart(CheckBox[] cbxs, string input) 
        {          
            int counter = 0;
            string queryPart = "";
            for (int i = 0; i < cbxs.Length; i++)
            {
                if(cbxs[i].IsChecked == true)
                {
                    if(counter == 0)
                    {
                        queryPart += "(" + cbxs[i].Content.ToString() + " LIKE '%" + input + "%' ";
                        counter++;
                    }
                    else
                    {
                        queryPart += " OR " + cbxs[i].Content.ToString() + " LIKE '%" + input + "%' ";
                    }
                }
            }
            queryPart += ")";
            return queryPart;
        }
        public static string advancedQuery(CheckBox[] cbxs, string input)
        {
            boxesNotChecked(cbxs);
            string[] keywords = splitInput(input);
            string startQuery = "SELECT Article.ArticleID, Article.Description ,  Manufacturer.name AS Manufacturer,Article.ManufacturerPartNumber, Supplier.name AS Supplier, Article.SupplierPartNumber , Location.Location AS Location, Article.Quantity, Article.Created, Article.LastUpdate, Article.Pricing FROM Articles AS Article"
                                                        + "  left JOIN Manufacturers AS Manufacturer ON Article.ID_Manufacturer = Manufacturer.ManufacturerID"
                                                        + "  left JOIN Suppliers AS Supplier ON Article.ID_Supplier = Supplier.SupplierID"
                                                        + "  left JOIN Locations AS Location ON Article.ID_Location = Location.LocationID WHERE ";
            string adhesive = "";
            if (keywords.Length == 1) return startQuery + generateQueryPart(cbxs,keywords[0]);

            for (int i = 0; i < keywords.Length -1; i++)
            {
                if (i < 1)
                {
                    adhesive += generateQueryPart(cbxs, keywords[i]);  //"(ArticleID Like '%" + keywords[i] + "%' OR Description LIKE '%" + keywords[i] + "%') " ;
                }
                else
                {
                    if(keywords[i] != "or" && keywords[i] != "and")
                    {
                        ArrayList tmp = new ArrayList(keywords);
                        tmp.Insert(i, "or");
                        keywords = (string[])tmp.ToArray(typeof(string));
                    } 
                    adhesive += keywords[i] + generateQueryPart(cbxs, keywords[i + 1]); //keywords[i] + " (ArticleID Like '%" + keywords[i + 1] + "%' OR Description LIKE '%" + keywords[i + 1] + "%') ";
                    i = i + 1;
                }          
            }           
            startQuery += adhesive;


            
            return startQuery;           
        }

        public static void boxesNotChecked(CheckBox[] cbxs)
        {
            int counter = 0;
         
            foreach(CheckBox cbx in cbxs)
            {
                if(cbx.IsChecked == false)
                {
                    counter++;
                }
            }
            if(counter == cbxs.Length)
            {
                foreach (CheckBox cbx in cbxs)
                {
                    cbx.IsChecked = true;                  
                }
            }


        }
        
        


    }
}
