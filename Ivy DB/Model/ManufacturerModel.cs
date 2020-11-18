using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar_bearbeiten.Model
{
    class ManufacturerModel
    {
        public int ManufacturerID { get; set; }
        public string Name { get; set; }
        public string ClientNumber { get; set; }
        public string WebAddress { get; set; }
    }

    class ManufacturerInsertModel
    {
        public string Name { get; set; }
        public string ClientNumber { get; set; }
        public string WebAddress { get; set; }
    }
}
