using Inventar_bearbeiten;
using Inventar_bearbeiten.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ivy_DB.View
{
    /// <summary>
    /// Interaktionslogik für CSVRowsSelection.xaml
    /// </summary>
    public partial class CSVRowsSelection : Window
    {
        List<ArticleModel> csvBasketList = new List<ArticleModel>();
        DataGrid dtg;
        CheckBox[] checkBoxes = new CheckBox[12];
        public CSVRowsSelection(DataGrid dtg)
        {
            InitializeComponent();
            this.dtg = dtg;
           
            checkBoxes[0] = cbxArticleID;
            checkBoxes[1] = cbxDescription;
            checkBoxes[2] = cbxManufacturer;
            checkBoxes[3] = cbxManufacturerPartNumber;
            checkBoxes[4] = cbxSupplier;
            checkBoxes[5] = cbxSupplierPartNumber;
            checkBoxes[6] = cbxPricing;
            checkBoxes[7] = cbxQuantity;
            checkBoxes[8] = cbxProjects;
            checkBoxes[9] = cbxStatus;
            checkBoxes[10] = cbxCreated;
            checkBoxes[11] = cbxLocation;
        }

        

        public void  createCSVFromRows(DataGrid dt)
        {
            var items = dt.SelectedItems;
            csvBasketList.Clear();
            if (items != null)
            {
                foreach (var item in items)
                {
                    csvBasketList.Add((ArticleModel)item);
                }

                if (csvBasketList.Count > 0)
                {
                    SaveFileDialog svd = new SaveFileDialog();
                    svd.Filter = "CSV File|*.csv";
                    svd.Title = "Save CSV File";
                    svd.ShowDialog();

                    List<ArticleModel> myCsvBasket = new List<ArticleModel>(csvBasketList);
                    CSVEditor.createCSVFromInventarList(myCsvBasket, svd.FileName, checkBoxes);
                    openGeneratedFile(svd.FileName);
                }
                else
                {
                    MessageBox.Show("CSV Basket is Empty");
                }

            }
        }
      

           
        
        public void openGeneratedFile(string path)
        {
            string msgBoxText = "Do you want to open the generated CSV File ?";
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            createCSVFromRows(dtg);
        }

        private void allChecked(object sender, RoutedEventArgs e)
        {
            foreach(CheckBox cbx in checkBoxes)
            {
                cbx.IsChecked = true;
            }
        }

        private void allUnchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cbx in checkBoxes)
            {
                cbx.IsChecked = false;
            }
        }
    }
}
