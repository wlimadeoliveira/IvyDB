using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar_bearbeiten.Model
{
    class kategorieModel
    {
        public string ArtID { get; set; }
        public string typID { get; set; }
        public string elementID { get; set; }
        public string bezeichnung { get; set; }

    }

    class KategorieSimpleModel{
        public string id { get; set; }
        public string bezeichnung { get; set; }
    }


    class CategoryModel{
        public string CategoryID { get; set; }
        public string Description { get; set; }
    }
}
