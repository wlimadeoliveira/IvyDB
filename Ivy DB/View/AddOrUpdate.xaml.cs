using Inventar_bearbeiten;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Inventar_bearbeiten.CSVEditor;
using Inventar_bearbeiten.Controller;
using Inventar_bearbeiten.Model;
using static Inventar_bearbeiten.Controller.CategoryController;
using Ivy_DB.Controller;
using Ivy_DB.Model;


namespace Ivy_2.View
{
    /// <summary>
    /// Interaktionslogik für AddOrUpdate.xaml
    /// </summary>
    public partial class AddOrUpdate : UserControl
    {
        

        InventarSearch inventarSearch;
        string itsME = "Articletype";
        DataGrid dtg;
        public bool itsAnUpdate = false;
        public int rowNumber;
        ObservableCollection<ArticleModel> inv;
        AdvancedSearch adv;

        public AddOrUpdate(DataGrid dtg, ObservableCollection<ArticleModel> inv, AdvancedSearch adv)
        {
            InitializeComponent();

            inventarSearch = new InventarSearch();
            this.dtg = dtg;
            this.inv = inv;
            tvrCategory.Items.Add(CategoryController.getCategoryListAsTreeView());
            cbxStatus.DisplayMemberPath = "Status";
            cbxStatus.SelectedValuePath = "StatusID";
            cbxStatus.SelectedIndex = 0;
            cbxStatus.ItemsSource = StatusController.StatusList();
            this.inv = inv;
            this.adv = adv;
            
        }

        public bool idsExist(ArticleModel am)
        {

            var isNumeric = int.TryParse(txtArticleType.Text, out int n);
            if (!isNumeric || txtArticleType.Text == "")
            {
                MessageBox.Show("Please check the Article type");
                return false;
            }
            if (am.ID_Category == 0) { MessageBox.Show("This Categorie isn't regitered in the DB"); return false; };
           // if (am.ID_Project == 0) { MessageBox.Show("This Project isn't regitered in the DB"); return false; };
            if (am.ID_Manufacturer == 0) { MessageBox.Show("This Manufacturer isn't regitered in the DB"); return false; };
            if (am.ID_Supplier == 0) { MessageBox.Show("This Supplier isn't regitered in the DB"); return false; };
            if (am.ID_Status == 0) { MessageBox.Show("This Status isn't regitered in the DB"); return false; };
            if (am.ID_Location == 0) { MessageBox.Show("This Location isn't regitered in the DB"); return false; };
            return true;
        }

        public bool isNumber(dynamic number)
        {
            var isNumeric = double.TryParse(number, out double n);
            if (!isNumeric)
            {

                return false;

            }
            else
            {
                return true;
            }

        }
        public List<Article_ProjectModel> articleprojectList(int articleID)
        {

            List<Article_ProjectModel> articleprojectList = new List<Article_ProjectModel>();
            string[] projs = txtProjects.Text.Split(',');
            foreach (string project in projs)
            {
                Article_ProjectModel articleproject = new Article_ProjectModel()
                {
                    ID_Article = articleID,
                    ID_Project = ProjectController.getProjectID(project)
                };
                articleprojectList.Add(articleproject);
            }
            return articleprojectList;
        }


        private void BtnAddArticle_Click(object sender, RoutedEventArgs e)
        {
            if (!isNumber(txtPricing.Text)) { txtPricing.Text = "1";  }
            if (!isNumber(txtQuantity.Text)) { txtQuantity.Text = "1";  }
            if ( !CategoryController.categoryExist(txtArticleType.Text) && !itsAnUpdate) {MessageBox.Show("Invalid Category" + CategoryController.getCategoryID(txtArticleType.Text));return; }
            ArticleModel am = new ArticleModel()
            {
                ArticleID = ArticleController.generateNewArticleNumber(Convert.ToInt32(txtArticleType.Text)),
                Description = txtDescription.Text,
                ID_Category = Convert.ToInt32(txtArticleType.Text),
                Pricing = Convert.ToDouble(txtPricing.Text),
                Quantity = Convert.ToInt32(txtQuantity.Text),
              //ID_Project = ProjectController.getProjectID(txtProjects.Text),
                SupplierPartNumber = txtSupplierPartNumber.Text,
                ManufacturerPartNumber = txtManufacturerPartNumber.Text,
                ID_Manufacturer = ManufacturerController.getManufacturerID(txtManufacturer.Text),
                ID_Supplier = SupplierController.getSupplierID(txtSupplier.Text),
                ID_Status = Convert.ToInt32(cbxStatus.SelectedValue.ToString()),
                Created = DateTime.Now.ToString("dd/MM/yyyy"),
                LastUpdate = DateTime.Now.ToString("dd/MM/yyyy"),
                ID_Location = LocationController.getLocationID(txtLocation.Text)
            };
            if (!idsExist(am))
            {
                return;
            }
  
            List<ArticleModel> duplicateArticles = ArticleController.checkSupplierAndManufacturerPartNumber(txtManufacturerPartNumber.Text, txtSupplierPartNumber.Text);
            if (duplicateArticles.Count >0 && itsAnUpdate == false)
            {
                MessageBox.Show("Manufacturer or Supplier Part Number Already Exist on ArticleNumber: " + duplicateArticles[0].ArticleID.ToString());
                return;
            }

                //AC.InsertArticle(am);
                if (!itsAnUpdate)
            {
                ArticleController.addNewArticle(am);
                inv.Add(am);
                Article_ProjectController.addArticleProjects(articleprojectList(am.ArticleID));
            }
            else if (itsAnUpdate)
            {
                am.ArticleID = Convert.ToInt32(txtArticleType.Text);
                ArticleController.updateArticle(am);
                Article_ProjectController.addArticleProjects(articleprojectList(am.ArticleID));
            }

            adv.refreshGrid();


            // adv.inv = new ObservableCollection<ArticleModel>(ArticleController.GetInventoryList());

        }
        //Auto Complete --> Supplier
        private void AutoComplete(object sender, KeyEventArgs e)
        {
            if (txtSupplier.Text != "" && SupplierController.searchSupplier(txtSupplier.Text).Count > 0)
            {
                listSuggestions.ItemsSource = SupplierController.searchSupplier(txtSupplier.Text);
                itsME = "Supplier";
                Thickness margin = listSuggestions.Margin;
                margin.Top = 168;
                listSuggestions.Margin = margin;
                listSuggestions.Visibility = Visibility.Visible;
                if (e.Key == Key.Down)
                {
                    listSuggestions.Focus();
                }
            }
            else
            {
                listSuggestions.Visibility = Visibility.Hidden;
            }
        }

        private void TxtSupplier_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        //Erkennt wo die Information, welches in der SuggestionList hin muss
        //Checks the clicked TextBox and fill the Suggestion list
        private void itemSelected(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                if (listSuggestions.SelectedItem != null && itsME == "Supplier") txtSupplier.Text = listSuggestions.SelectedItem.ToString();
                if (listSuggestions.SelectedItem != null && itsME == "Manufacturer") txtManufacturer.Text = listSuggestions.SelectedItem.ToString();
                if (listSuggestions.SelectedItem != null && itsME == "Articletype") txtArticleType.Text = listSuggestions.SelectedItem.ToString().Substring(0, 3);
                listSuggestions.Visibility = Visibility.Hidden;
            }
            if (e.Key == Key.Escape)
            {
                listSuggestions.Visibility = Visibility.Hidden;
            }

            listSuggestions.Visibility = Visibility.Hidden;

        }

        private void itemSelectedByMouse(object sender, MouseEventArgs e)
        {

            if (listSuggestions.SelectedItem != null && itsME == "Supplier") txtSupplier.Text = listSuggestions.SelectedItem.ToString();
            if (listSuggestions.SelectedItem != null && itsME == "Manufacturer") txtManufacturer.Text = listSuggestions.SelectedItem.ToString();
            if (listSuggestions.SelectedItem != null && itsME == "Articletype") txtArticleType.Text = listSuggestions.SelectedItem.ToString().Substring(0, 3);
            listSuggestions.Visibility = Visibility.Hidden;
        }

        //Fill the autocomple suggestion list and fill it with manufacturer
        private void AutoCompleteManufacturer(object sender, KeyEventArgs e)
        {
            if (txtManufacturer.Text != "" && ManufacturerController.searchManufacturer(txtManufacturer.Text).Count > 0)
            {
                listSuggestions.ItemsSource = ManufacturerController.searchManufacturer(txtManufacturer.Text);
                itsME = "Manufacturer";
                Thickness margin = listSuggestions.Margin;
                margin.Top = 95;
                listSuggestions.Margin = margin;
                listSuggestions.Visibility = Visibility.Visible;
                if (e.Key == Key.Down)
                {
                    listSuggestions.Focus();
                }

            }
            else
            {
                listSuggestions.Visibility = Visibility.Hidden;
            }

        }
        //Fill the autocomple suggestion list and fill it with Article type
        private void AutoCompleteArticleType(object sender, KeyEventArgs e)
        {

            if (txtArticleType.Text != "" && CategoryController.searchCategory(txtArticleType.Text).Count > 0)
            {
                //listSuggestions.ItemsSource = inventarSearch.autoComplete(txtArticleType);
                listSuggestions.ItemsSource = CategoryController.searchCategory(txtArticleType.Text);
                itsME = "Articletype";
                Thickness margin = listSuggestions.Margin;
                margin.Top = 25;
                listSuggestions.Margin = margin;
                listSuggestions.Visibility = Visibility.Visible;
                if (e.Key == Key.Down)
                {
                    listSuggestions.Focus();
                }

            }
            else
            {
                listSuggestions.Visibility = Visibility.Hidden;
            }

        }

        //Fill the full suggestion list with Article Type
        private void OnFocus_ArtTypeShowFullSuggestionList(object sender, RoutedEventArgs e)
        {

            tvrCategory.Visibility = Visibility.Visible;
            listfullSuggestions.Visibility = Visibility.Hidden;

            itsME = "Articletype";
        }

        //Fill the full suggestion list with manufacturer names
        private void OnFocus_ManufacturerShowFullSuggestionList(object sender, RoutedEventArgs e)
        {

            tvrCategory.Visibility = Visibility.Hidden;
            listfullSuggestions.Visibility = Visibility.Visible;
            listfullSuggestions.DisplayMemberPath = "Name";
            listfullSuggestions.SelectedValuePath = "Name";
            listfullSuggestions.ItemsSource = ManufacturerController.getManufacturerList();
            itsME = "Manufacturer";

        }

        //Fill the full suggestion list with supplier names
        private void OnFocus_SupplierShowFullSuggestionList(object sender, RoutedEventArgs e)
        {
            tvrCategory.Visibility = Visibility.Hidden;
            listfullSuggestions.Visibility = Visibility.Visible;
            listfullSuggestions.DisplayMemberPath = "Name";
            listfullSuggestions.SelectedValuePath = "Name";
            listfullSuggestions.ItemsSource = SupplierController.getSupplierList();
            itsME = "Supplier";
            //listfullSuggestions.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
        }


        //Fills the focused text box with the selected information from the Suggestion list
        private void itemSelectedFullList(object sender, SelectionChangedEventArgs e)
        {
            if (listfullSuggestions.SelectedItem != null && itsME == "Supplier") txtSupplier.Text = listfullSuggestions.SelectedValue.ToString();
            if (listfullSuggestions.SelectedItem != null && itsME == "Manufacturer") txtManufacturer.Text = listfullSuggestions.SelectedValue.ToString();
            if (listfullSuggestions.SelectedItem != null && itsME == "Articletype") txtArticleType.Text = listfullSuggestions.SelectedItem.ToString().Substring(0, 3);
            if (listfullSuggestions.SelectedValue != null && itsAnUpdate)
            {
                Article_ProjectModel artproj = new Article_ProjectModel()
                {
                    ID_Article = Convert.ToInt32(txtArticleType.Text),
                    ID_Project = Convert.ToInt32(ProjectController.getProjectID(listfullSuggestions.SelectedValue.ToString()))
                };

                if (!Article_ProjectController.articleprojectExists(artproj) && itsAnUpdate)
                {
                    if (txtProjects.Text != "") if (listfullSuggestions.SelectedItem != null && itsME == "Project") txtProjects.Text += "," + listfullSuggestions.SelectedValue.ToString();
                    if (txtProjects.Text == "") if (listfullSuggestions.SelectedItem != null && itsME == "Project") txtProjects.Text += listfullSuggestions.SelectedValue.ToString();
                }
               
                   
               
            }
            if (!itsAnUpdate && itsME == "Project")
            {
                if (txtProjects.Text != "") txtProjects.Text += "," + listfullSuggestions.SelectedValue.ToString();
                if (txtProjects.Text == "") txtProjects.Text += listfullSuggestions.SelectedValue.ToString();
            }
            if (listfullSuggestions.SelectedItem != null && itsME == "Location") txtLocation.Text = listfullSuggestions.SelectedValue.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtArticleType.Text = "";
            txtDescription.Text = "";

            txtLocation.Text = "";
            txtManufacturer.Text = "";
            txtManufacturerPartNumber.Text = "";
            txtPricing.Text = "";
            txtProjects.Text = "";

            txtSupplier.Text = "";
            txtSupplierPartNumber.Text = "";
            txtQuantity.Text = "";
            cbxStatus.SelectedIndex = 0;
            btnAddArticle.Content = "Add";
            itsME = "Articletype";
            txtArticleType.IsReadOnly = false;
            itsAnUpdate = false;
            txtArticleType.Focus();
            btnAddArticle.IsEnabled = true;
            adv.dtGridInventarListe.SelectedItem = null;
        }

        private void OnFocus_ArticleType(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (itsME == "Articletype")
            {
                if (!itsAnUpdate)
                {
                    menuItem category = (menuItem)tvrCategory.SelectedItem;
                    txtArticleType.Text = category.id;
                }
            }
        }

        private void Choose_ArticleType_DoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnFocus_ProjectFullSuggetionList(object sender, RoutedEventArgs e)
        {
            tvrCategory.Visibility = Visibility.Hidden;
            listfullSuggestions.Visibility = Visibility.Visible;
            listfullSuggestions.DisplayMemberPath = "Name";
            listfullSuggestions.SelectedValuePath = "Name";
            listfullSuggestions.ItemsSource = ProjectController.getProjectList();
            itsME = "Project";

        }

        private void OnFocus_LocationFullSuggetionList(object sender, RoutedEventArgs e)
        {
            tvrCategory.Visibility = Visibility.Hidden;
            listfullSuggestions.Visibility = Visibility.Visible;
            listfullSuggestions.DisplayMemberPath = "Location";
            listfullSuggestions.SelectedValuePath = "Location";
            listfullSuggestions.ItemsSource = LocationController.getLocationList();

            itsME = "Location";

        }

        private void BTN_AddManufacturer(object sender, RoutedEventArgs e)
        {
            AddNewManFactOrSupplier adnm = new AddNewManFactOrSupplier("Manufacturer");
            adnm.Show();
        }

        private void BTN_AddSupplier(object sender, RoutedEventArgs e)
        {
            AddNewManFactOrSupplier adnm = new AddNewManFactOrSupplier("Supplier");
            adnm.Show();
        }

        private void BTN_AddCategory(object sender, RoutedEventArgs e)
        {
            AddNewManFactOrSupplier adnm = new AddNewManFactOrSupplier("Category");
            adnm.Show();
        }

        private void BTN_AddLocation(object sender, RoutedEventArgs e)
        {
            AddNewManFactOrSupplier adnm = new AddNewManFactOrSupplier("Location");
            adnm.Show();

           

        }

        private void BTN_AddProject(object sender, RoutedEventArgs e)
        {
            AddNewManFactOrSupplier adnm = new AddNewManFactOrSupplier("Project");
            adnm.Show();
        }

        private void btnPrintLabel(object sender, RoutedEventArgs e)
        {


            PrintModel print = new PrintModel()
            {
                ArticleID = txtArticleType.Text,
                Manufacturer = txtManufacturer.Text,
                Description = txtDescription.Text,
                ManufacturerPartNumber = txtManufacturerPartNumber.Text
               
            };

            PrintController.print(print);


        }

        

     
    }
}
