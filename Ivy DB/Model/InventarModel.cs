using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar_bearbeiten
{
    class InventarModel
    {
       

    }
    public class Inventar
    {
        public int ArticleID {get; set;}
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string Supplier { get; set; }
        public string SupplierPartNumber { get; set; }
        public double Pricing { get; set; }
        public int Quantity { get; set; }
        public string quantityAvaible { get; set; }
        public string Project { get; set; }
        public string Status { get; set; }
        public DateTime created { get; set; }
        public string Group { get; set; }
        public string Location { get; set; }
        public int rowNumber { get; set; }
        public string supplierOrGoogleLink { get; set; }
    }

    public class PrecisionWaveLibrary
    {
        
    }


}
