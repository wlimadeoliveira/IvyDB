using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using Inventar_bearbeiten.Controller;
using Inventar_bearbeiten.Model;
using Microsoft.Win32;

using System.Windows;
using Ivy_2.View;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Ivy_DB.Controller;
using Ivy_DB.View;
using Ivy_DB.Model;
using Squirrel;


/*using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;*/


namespace Inventar_bearbeiten
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class AdvancedSearch : System.Windows.Window
    {
        public OpenFileDialog op = new OpenFileDialog();
        public AddOrUpdate addupdate;

        public ObservableCollection<ArticleModel> inv;

        CSVEditor csveditor = new CSVEditor();

        ObservableCollection<ArticleModel> csvBasketList = new ObservableCollection<ArticleModel>();
        

        public AdvancedSearch()
        {
            InitializeComponent();
            Process mobj_pro = Process.GetCurrentProcess();
            Process[] mobj_proList = Process.GetProcessesByName(mobj_pro.ProcessName);
            if (mobj_proList.Length > 1)
            {
                MessageBox.Show("Can not open another one ", "Alert", MessageBoxButton.OK);
                this.Close();
                return;
            }

            this.inv = new ObservableCollection<ArticleModel>(ArticleController.GetInventoryList());
            dtGridInventarListe.ItemsSource = null;
            dtGridInventarListe.ItemsSource = inv;
            lblRowsCount.Content = "Articles count: " + dtGridInventarListe.Items.Count;
            AddOrUpdate UCaddupdate = new AddOrUpdate(dtGridInventarListe, inv, this);
            addupdate = UCaddupdate;
            addupdate.Visibility = Visibility.Visible;
            gridAddOrUpdate.Children.Add(addupdate);
            addupdate.HorizontalAlignment = HorizontalAlignment.Center;
            //CheckForUpdates();
            AddVersionNumber();
           
            
            
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Title =  $"Ivy DB v. {versionInfo.FileVersion}";
        }
      /*  private async Task CheckForUpdates()
        {
            using(var manager = new UpdateManager(@"\\192.168.1.10\software\Wagner\IvyUpdate\Releases"))
            {
                await manager.UpdateApp();
            }
        }*/

        //Search button --> will find Manufacturer and Supplier Number  and select it in the DataGrid
        private void BtnSuchen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dtGridInventarListe.ItemsSource = null;
            /*  ObservableCollection<Inventar> FoundList = InventarSearch.AdvancedSearchList(this, inv);
              dtGridInventarListe.ItemsSource = FoundList;*/
            CheckBox[] checkBoxes = new CheckBox[7];
            checkBoxes[0] = cbxSupplier;
            checkBoxes[1] = cbxManufacturer;
            checkBoxes[2] = cbxCategory;
            checkBoxes[3] = cbxDescription;
            checkBoxes[4] = cbxLocation;
            checkBoxes[5] = cbxManufacturerPartNumber;
            checkBoxes[6] = cbxSupplierPartNumber;


            dtGridInventarListe.ItemsSource = null;
            dtGridInventarListe.ItemsSource = ArticleController.GetAdvancedSearchList(checkBoxes, txtSuchfeld.Text); //ArticleController.FoundArticles(txtSuchfeld.Text, checkBoxes);
            lblRowsCount.Content = "Articles count: " + dtGridInventarListe.Items.Count;
            //MessageBox.Show(ExpertSmartSearch.advancedQuery(txtSuchfeld.Text));
        }
        
        //Get path from a file
        private string getFileName()
        {
            op = new OpenFileDialog();
            op.ShowDialog();
            return op.FileName;
        }
        
        //Closes editor
        private void WindowsClosed(object sender, EventArgs e)
        {
            // editor.close();
        }

        //On Datagrid row double click --> new FormAddArticle window will be open with the clicked information to update row on Excel file
        private void UpdateRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ArticleModel inv = (ArticleModel)dtGridInventarListe.SelectedItem;

            addupdate.txtArticleType.Text = inv.ArticleID.ToString();
            addupdate.txtDescription.Text = inv.Description;

            addupdate.txtManufacturer.Text = inv.Manufacturer;
            addupdate.txtManufacturerPartNumber.Text = inv.ManufacturerPartNumber;
            addupdate.txtPricing.Text = inv.Pricing.ToString();
            addupdate.txtLocation.Text = inv.Location;
            addupdate.txtQuantity.Text = inv.Quantity.ToString();
            addupdate.txtProjects.Text = inv.Project;
            addupdate.txtSupplier.Text = inv.Supplier;
            addupdate.txtSupplierPartNumber.Text = inv.SupplierPartNumber;
            addupdate.cbxStatus.SelectedIndex = 0;

            addupdate.btnAddArticle.Content = "Update";
            addupdate.itsAnUpdate = true;

            addupdate.txtArticleType.IsReadOnly = true;
            addupdate.itsAnUpdate = true;
        }

        public void refreshGrid()
        {
            dtGridInventarListe.ItemsSource = null;
            dtGridInventarListe.ItemsSource = ArticleController.GetInventoryList();
            lblRowsCount.Content = "Articles count: " + dtGridInventarListe.Items.Count;
        }



        //On Right Click in a Row/Context Menü -> Show More Info will start browser with Supplier, Manufacturer or Google link 
        //See Excel Editor --> AddHyperLink
        private void startBrowser(object sender, System.Windows.RoutedEventArgs e)
        {
            ArticleModel article = (ArticleModel)dtGridInventarListe.SelectedItem;
            System.Diagnostics.Process.Start(article.SupplierOrGoogleLink);
        }



        private void DtGridInventarListe_Unloaded(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)sender;
            grid.CommitEdit(DataGridEditingUnit.Row, true);
            grid.CancelEdit();
        }



        private void Button_AddCsvBasket(object sender, RoutedEventArgs e)
        {
            var items = dtGridInventarListe.SelectedItems;

            if (items != null)
            {
                foreach (var item in items)
                {
                    csvBasketList.Add((ArticleModel)item);
                }
            }
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /**  var items = dtGridInventarListe.SelectedItems;
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
                      CSVEditor.createCSVFromInventarList(myCsvBasket, svd.FileName);
                      openGeneratedFile(svd.FileName);
                  }
                  else
                  {
                      MessageBox.Show("CSV Basket is Empty");
                  }

              }**/
            CSVRowsSelection cSVRowSelection = new CSVRowsSelection(dtGridInventarListe);
            cSVRowSelection.Show();
        }


        private void DtGridInventarListe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtGridInventarListe.SelectedItem != null)
            {
                ArticleModel inv = (ArticleModel)dtGridInventarListe.SelectedItem;

                addupdate.txtArticleType.Text = inv.ArticleID.ToString();
                addupdate.txtDescription.Text = inv.Description;

                addupdate.txtManufacturer.Text = inv.Manufacturer;
                addupdate.txtManufacturerPartNumber.Text = inv.ManufacturerPartNumber;
                addupdate.txtPricing.Text = inv.Pricing.ToString();
                addupdate.txtLocation.Text = inv.Location;
                addupdate.txtQuantity.Text = inv.Quantity.ToString();
                addupdate.txtProjects.Text = Article_ProjectController.getProjects(inv.ArticleID.ToString());
                addupdate.txtProjects.ToolTip =  Article_ProjectController.getProjects(inv.ArticleID.ToString()).Replace(",","\n");
                addupdate.txtSupplier.Text = inv.Supplier;
                addupdate.txtSupplierPartNumber.Text = inv.SupplierPartNumber;
                addupdate.cbxStatus.SelectedIndex = 0;

                addupdate.btnAddArticle.Content = "Update";
                addupdate.itsAnUpdate = true;

                addupdate.txtArticleType.IsReadOnly = true;
                addupdate.itsAnUpdate = true;
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


        


        private void TxtSuchfeld_KeyUp(object sender, KeyEventArgs e)
        {
            dtGridInventarListe.ItemsSource = null;
            if (e.Key == Key.Enter)
            {
                CheckBox[] checkBoxes = new CheckBox[7];
                checkBoxes[0] = cbxSupplier;
                checkBoxes[1] = cbxManufacturer;
                checkBoxes[2] = cbxCategory;
                checkBoxes[3] = cbxDescription;
                checkBoxes[4] = cbxLocation;
                checkBoxes[5] = cbxManufacturerPartNumber;
                checkBoxes[6] = cbxSupplierPartNumber;


                dtGridInventarListe.ItemsSource = null;
                dtGridInventarListe.ItemsSource = ArticleController.GetAdvancedSearchList(checkBoxes,txtSuchfeld.Text);
                lblRowsCount.Content = "Articles count: " + dtGridInventarListe.Items.Count;
                

            }
            if (e.Key == Key.Escape)
            {
                txtSuchfeld.Text = "";
            }

        }

        private void BtnSuchen_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {

            }
        }

        private void Click_ExportExcelFile(object sender, RoutedEventArgs e)
        {
            ExcelEditor.ToExcelFile();
        }

        private void Click_ImportExcelFile(object sender, RoutedEventArgs e)
        {
            ImportController.ImportFromExcel(getFileName());
        }

        private void BTN_RemoveArticle(object sender, RoutedEventArgs e)
        {
            ArticleModel articletoDelete = (ArticleModel)dtGridInventarListe.SelectedItem;
            string msgBoxText = "Are you sure that you want to remove: "+ articletoDelete.ArticleID + ":  " + articletoDelete.Description +  "? The deleted article will be saved in the Archiv";
            MessageBoxButton btnConfirmToOpen = MessageBoxButton.YesNo;
            MessageBoxImage msgImage = MessageBoxImage.Warning;
            MessageBoxResult rsltMessageBox = MessageBox.Show(msgBoxText,"REMOVE: "+ articletoDelete.ArticleID + ":  " + articletoDelete.Description, btnConfirmToOpen, msgImage);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    ArticleController.deleteArticle(articletoDelete);
                    addupdate.btnAddArticle.IsEnabled = false;
                    refreshGrid();
                    break;
                case MessageBoxResult.No:
                    break;
            }
         
        }

        private void PrintLabel(object sender, RoutedEventArgs e)
        {
            if (dtGridInventarListe.SelectedItem != null)
            {
                foreach (object item in dtGridInventarListe.SelectedItems)
                {
                    ArticleModel article = (ArticleModel)item;
                    PrintModel print = new PrintModel()
                    {
                        ArticleID = article.ArticleID.ToString(),
                        Description = article.Description,
                        Manufacturer = article.Manufacturer,
                        ManufacturerPartNumber = article.ManufacturerPartNumber
                    };

                    PrintController.print(print);
                }
            }
        }

        private void Clone_Article(object sender, RoutedEventArgs e)
        {
            if(dtGridInventarListe.SelectedItem != null)
            {
                ArticleModel article = (ArticleModel) dtGridInventarListe.SelectedItem;
                addupdate.txtArticleType.Text = (article.ArticleID.ToString()[0].ToString() + article.ArticleID.ToString()[1].ToString() + article.ArticleID.ToString()[2].ToString()).ToString();
                addupdate.txtDescription.Text = article.Description;
                addupdate.txtLocation.Text = article.Location;
                addupdate.txtManufacturer.Text = article.Manufacturer;
                addupdate.txtPricing.Text = article.Pricing.ToString();
                addupdate.txtManufacturerPartNumber.Text = article.ManufacturerPartNumber;
                addupdate.txtProjects.Text = article.Project;
                addupdate.txtSupplierPartNumber.Text = article.SupplierPartNumber;
                addupdate.txtQuantity.Text = article.Quantity.ToString();
                addupdate.txtSupplier.Text = article.Supplier;
                addupdate.itsAnUpdate = false;
                addupdate.btnAddArticle.Content = "Add New";

            }
        }
    }
}
