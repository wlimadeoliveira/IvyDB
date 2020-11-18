using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar_bearbeiten.Model
{
    public class ArticleModel
    {
        public int ArticleID { get; set; }
        public string Description { get; set; }
        public int ID_Category { get; set; }
        public double Pricing { get; set; }
        public int Quantity{ get; set; }
        public int ID_Project { get; set; }
        public string SupplierPartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public int ID_Manufacturer { get; set; }
        public int ID_Supplier { get; set; }
        public int ID_Status { get; set; }
        public string Created { get; set; }
        public string LastUpdate { get; set; }
        public int ID_Location { get; set; }

        
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string Supplier { get; set; }
        public string Project { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string SupplierOrGoogleLink { get; set; }
    }
}
