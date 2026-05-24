using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageViewer.Data
{
    public partial class Initilize
    {
        private readonly string _connectionString;

        public Initilize() 
        {
           
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(
                _connectionString);
        }
    }
}
