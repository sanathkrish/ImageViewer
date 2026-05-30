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

        public async Task<List<DriverInfo>> GetAllFileDriversAsync()
        {
            var drivers = new List<DriverInfo>();
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Path, type, DateAdded, totalSpace, freespace FROM drivers";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                drivers.Add(new DriverInfo
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(2),
                    Path = reader.GetString(3),
                    Type = reader.GetString(4),
                    DateAdded = reader.GetDateTime(5),
                    TotalSize = reader.GetInt64(6),
                    FreeSpace = reader.GetInt64(7)
                });
            }
            return drivers;
        }

        public async Task<bool> RecordExistsForPath(string path)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Drivers WHERE lower(Path) = lower(@path)";
            command.Parameters.AddWithValue("@path", path);
            var count = (long)await command.ExecuteScalarAsync();
            return count > 0;
        }
    }
}
