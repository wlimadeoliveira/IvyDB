using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using Microsoft.Win32;

namespace Inventar_bearbeiten
{
    public class InventoryDBSqliteConnection
    {
        public static string LoadConnectionString(string id = "SqliteInvetoryDB")
        {
            string directory = Directory.GetCurrentDirectory();
            string directorydown = Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

            //return "Data Source = C:\\Users\\Wagner\\source\\repos\\Ivy DB\\InventarDB.db; Version = 3; New = True; Compress = True; ";
            return "Data Source = InventarDB.db; Version = 3; New = True; Compress = True; ";
            //ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }


    }
}
