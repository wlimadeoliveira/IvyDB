using Inventar_bearbeiten.Controller;
using Inventar_bearbeiten.Model;
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

namespace Inventar_bearbeiten
{
    /// <summary>
    /// Interaktionslogik für AddNewManFactOrSupplier.xaml
    /// </summary>
    public partial class AddNewManFactOrSupplier : Window
    {
        ManufacturerController mc = new ManufacturerController();
        SupplierController sc = new SupplierController();
        string typeOf;
       
        public AddNewManFactOrSupplier()
        {
            InitializeComponent();
        }

        public AddNewManFactOrSupplier(string typeOf)
        {
            InitializeComponent();
            this.typeOf = typeOf;
            this.Title = "Add New" + typeOf;


            if (typeOf == "Project")
            {
                this.Title = "Add new Project";
                lbl1.Content = "Project Name";
                lbl2.Visibility = Visibility.Hidden;
                lbl3.Visibility = Visibility.Hidden;
                txtClientNumber.Visibility = Visibility.Hidden;
                txtWebAddress.Visibility = Visibility.Hidden;

            }

            if (typeOf == "Location")
            {
                this.Title = "Add new Location";
                lbl1.Content = "Location Name";
                lbl2.Visibility = Visibility.Hidden;
                lbl3.Visibility = Visibility.Hidden;
                txtClientNumber.Visibility = Visibility.Hidden;
                txtWebAddress.Visibility = Visibility.Hidden;

            }
            if (typeOf == "Category")
            {
                this.Title = "Add new Category";
                lbl1.Content = "ID";
                lbl2.Content = "Description";
               

                lbl3.Visibility = Visibility.Hidden;

                txtWebAddress.Visibility = Visibility.Hidden;

            }
        }

        private void Button_Click_Add_New(object sender, RoutedEventArgs e)
        {
            if (typeOf == "Manufacturer")
            {
                this.Title = "Add new Manufacturer";
                ManufacturerModel mm = new ManufacturerModel()
                {
                    Name = txtName.Text,
                    ClientNumber = txtClientNumber.Text,
                    WebAddress = txtWebAddress.Text
                };
                ManufacturerController.addManufacturer(mm);
                MessageBox.Show("New " + typeOf + " " + mm.Name + " was added succesfully");
            }
            if (typeOf == "Supplier")
            {
                this.Title = "Add new Supplier";
                SupplierModel sm = new SupplierModel()
                {
                    Name = txtName.Text,
                    clientNumber = txtClientNumber.Text,
                    webAddress = txtWebAddress.Text
                };
                SupplierController.addSupplier(sm);
                MessageBox.Show("New " + typeOf + " " + sm.Name + " was added succesfully");
            }

            if (typeOf == "Project")
            {
                this.Title = "Add new Project";
                lbl1.Content = "Project Name";
                lbl2.Visibility = Visibility.Hidden;
                lbl3.Visibility = Visibility.Hidden;
                ProjectModel pj = new ProjectModel()
                {
                    Name = txtName.Text,
                };
                ProjectController.addProject(pj);
                MessageBox.Show("New " + typeOf + " " + pj.Name + " was added succesfully");
            }

            if (typeOf == "Location")
            {
                this.Title = "Add new Location";
                lbl1.Content = "Location Name";
                lbl2.Visibility = Visibility.Hidden;
                lbl3.Visibility = Visibility.Hidden;
                txtClientNumber.Visibility = Visibility.Hidden;
                txtWebAddress.Visibility = Visibility.Hidden;
                LocationModel lm = new LocationModel()
                {
                    Location = txtName.Text
                };
                LocationController.addLocation(lm);
                MessageBox.Show("New " + typeOf + " " + lm.Location + " was added succesfully");
            }
            if (typeOf == "Category")
            {
                this.Title = "Add new Location";
                lbl1.Content = "ID";
                lbl2.Content = "Description";
               
                lbl3.Visibility = Visibility.Hidden;
                txtWebAddress.Visibility = Visibility.Hidden;
                CategoryModel cm = new CategoryModel()
                {
                    CategoryID = txtName.Text,
                    Description = txtClientNumber.Text
                };
                bool result = CategoryController.addCategory(cm);
                if (result) MessageBox.Show("New " + typeOf + " " + cm.CategoryID + " - " + cm.Description + " was added succesfully");

            }
        }

    }
}


