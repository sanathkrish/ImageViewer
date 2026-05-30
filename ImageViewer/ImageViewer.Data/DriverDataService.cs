using ImageViewer.Model.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageViewer.Data
{
    public class DriverDataService
    {
        private SqliteConnection _connection;
        public DriverDataService(SqliteConnection connection)
        {
            this._connection = connection;
        }

        public async Task AddFileDriver(DriverInfo driver)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO DRIVERS (Name, Path, Type, DateAdded, TotalSize, FreeSpace)
                VALUES (@name, @path, @type, @dateAdded, @totalSize, @freeSpace)";
            command.Parameters.AddWithValue("@name", driver.Name);
            command.Parameters.AddWithValue("@path", driver.Path);
            command.Parameters.AddWithValue("@type", driver.Type );
            command.Parameters.AddWithValue("@dateAdded", driver.DateAdded);
            command.Parameters.AddWithValue("@totalSpace", driver.TotalSize);
            command.Parameters.AddWithValue("@freeSpace", driver.FreeSpace);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateFileDeriver(DriverInfo driver) 
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                UPDATE Drivers
                SET Name = @name, Path = @path, type = @type, DateAdded = @dateAdded, FreeSpace = @freeSpace, totalSpace = @totalSpace
                WHERE Id = @id";
            command.Parameters.AddWithValue("@id", driver.Id);
            command.Parameters.AddWithValue("@name", driver.Name);
            command.Parameters.AddWithValue("@path", driver.Path);
            command.Parameters.AddWithValue("@size", driver.Type);
            command.Parameters.AddWithValue("@freeSpace", driver.FreeSpace);
            command.Parameters.AddWithValue("@totalSpace", driver.TotalSize);
            command.Parameters.AddWithValue("@dateAdded", driver.DateAdded);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteFileDriverAsync(int id)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM Drivers WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}
