using Ivy_DB.Model;
using System;
using System.IO;
using System.Windows;
using bpac;


namespace Ivy_DB.Controller
{
    class PrintController
    {
        public static void print(PrintModel print) {
            try
            {
                string directory = Directory.GetCurrentDirectory();
                string pfad = directory + @"\ivy_label.LBX";
                
                bpac.DocumentClass doc = new DocumentClass();
                doc.Open(pfad);
                
                    doc.GetObject("ArticleID").Text = print.ArticleID.ToString();
                    doc.GetObject("ArticleBCode").Text = print.ArticleID.ToString();
                    doc.GetObject("Manufacturer").Text = print.Manufacturer.ToString();
                    doc.GetObject("ManufacturerPartNumber").Text = print.ManufacturerPartNumber.ToString();
                    doc.GetObject("ArticleQR").Text = print.ArticleID.ToString();
                    doc.GetObject("Description").Text = print.Description.ToString();
                    doc.SetPrinter("Brother QL-500", true);
                    doc.StartPrint("", PrintOptionConstants.bpoDefault);
                    doc.PrintOut(1, PrintOptionConstants.bpoDefault);
                    doc.EndPrint();
                    doc.Close();




               


               
                    
                
            }catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


    }
}
