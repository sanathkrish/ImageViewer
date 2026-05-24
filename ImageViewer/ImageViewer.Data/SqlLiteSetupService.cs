using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Data
{
    public class SqlLiteSetupService
    {
        private string _connectionString;

        public SqliteConnection Initilize()
        {
            string dbPath =
              Path.Combine(
                  AppDomain.CurrentDomain.BaseDirectory,
                  "ImageViewer_Data",
                  "ImageViewer.db");
            if (!File.Exists(dbPath))
            {
                Directory.CreateDirectory(
          Path.GetDirectoryName(dbPath));
            }

            _connectionString =
                $"Data Source={dbPath}";
            return new SqliteConnection(
           _connectionString);
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(
                _connectionString);
        }

        public async Task CreateTables()
        {
            await ExecuteQuery(@"
        PRAGMA journal_mode=WAL;
        PRAGMA synchronous=NORMAL;
        PRAGMA temp_store=MEMORY;
        ");
            await ExecuteQuery(@"
create Table IF NOT EXISTS DRIVERS (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Path TEXT NOT NULL,
    Type TEXT NOT NULL,
    TotalSize INTEGER NOT NULL,
    FreeSpace INTEGER NOT NULL,
    DateAdded DATETIME NOT NULL
);
");
            await ExecuteQuery(@"
      CREATE TABLE IF NOT EXISTS Files (
          Id INTEGER PRIMARY KEY AUTOINCREMENT,
          DriverId INTEGER NOT NULL,
          Name TEXT NOT NULL,
          Path TEXT NOT NULL,
          Size INTEGER NOT NULL,
          DateAdded DATETIME NOT NULL,
          ModifiedDate DATETIME NULL,
          Hash TEXT NULL
      );");
            await ExecuteQuery(@" CREATE INDEX IF NOT EXISTS idx_path
        ON Files(Path);

        CREATE INDEX IF NOT EXISTS idx_size
        ON Files(Size);");

            await ExecuteQuery(@"
CREATE TABLE IF NOT EXISTS DUPLICATE_FILES (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FileId INTEGER NOT NULL,
    DuplicateFileId INTEGER NOT NULL,
    DateAdded DATETIME NOT NULL
);
");
        }

        public async Task ExecuteQuery(string query)
        {
            var connection = CreateConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            // Performance settings
            command.CommandText = query;

            await command.ExecuteNonQueryAsync();
            connection.Close();
        }
    }
}
