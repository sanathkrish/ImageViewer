using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.File
{
    public class ScanCompleteDrive
    {
        SqliteConnection _connection;
        public ScanCompleteDrive(SqliteConnection connection)
        {
            _connection = connection;
        }

    }
}
