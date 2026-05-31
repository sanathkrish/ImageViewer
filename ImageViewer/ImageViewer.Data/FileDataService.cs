using ImageViewer.Model.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Data
{
    public class FileDataService
    {
        private SqliteConnection _connection;
        public FileDataService(SqliteConnection connection)
        {
            this._connection = connection;
        }

        public async Task AddOrUpdateFileAsync(FileRecord record)
        {
            try
            {
                if (record == null) throw new ArgumentNullException(nameof(record));
                if (await RecordExistsForPath(record.Path))
                {
                    await UpdateFileAsync(record.DriverId, record.Id, record.Name, record.Path, record.Size, record.DateAdded, record.ModifiedDate);
                }
                else
                {
                    await AddFileAsync(record.DriverId, record.Name, record.Path, record.Size, record.DateAdded, record.ModifiedDate);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task AddFileAsync(int driverId, string name, string path, long size, DateTime dateAdded, DateTime? modifiedDate = null)
        {
            _connection.Open();

            var command = _connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Files (DriverId, Name, Path, Size, DateAdded, ModifiedDate)
                VALUES (@driverId, @name, @path, @size, @dateAdded, @modifiedDate)";
            command.Parameters.AddWithValue("@driverId", driverId);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@path", path);
            command.Parameters.AddWithValue("@size", size);
            command.Parameters.AddWithValue("@dateAdded", dateAdded);
            command.Parameters.AddWithValue("@modifiedDate", modifiedDate.HasValue ? (object)modifiedDate.Value : DBNull.Value);
            await command.ExecuteNonQueryAsync();

        }

        public async Task<List<FileRecord>> GetAllFilesAsync(string driverId)
        {
            var files = new List<FileRecord>();
            _connection.Open();

            var command = _connection.CreateCommand();
            command.CommandText = "SELECT Id, DriverId, Name, Path, Size, DateAdded, ModifiedDate FROM Files WHERE DriverId = @driverId";
            command.Parameters.AddWithValue("@driverId", driverId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                files.Add(new FileRecord
                {
                    Id = reader.GetInt32(0),
                    DriverId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Path = reader.GetString(3),
                    Size = reader.GetInt64(4),
                    DateAdded = reader.GetDateTime(5),
                    ModifiedDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6)
                });
            }

            return files;
        }

        public async Task<bool> UpdateFileAsync(int driverId, int id, string name, string path, long size, DateTime dateAdded, DateTime? modifiedDate = null)
        {
            _connection.Open();

            var command = _connection.CreateCommand();
            command.CommandText = @"
                UPDATE Files
                SET Name = @name, DriverId = @driverId, Path = @path, Size = @size, DateAdded = @dateAdded, ModifiedDate = @modifiedDate
                WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@driverId", driverId);
            command.Parameters.AddWithValue("@path", path);
            command.Parameters.AddWithValue("@size", size);
            command.Parameters.AddWithValue("@dateAdded", dateAdded);
            command.Parameters.AddWithValue("@modifiedDate", modifiedDate.HasValue ? (object)modifiedDate.Value : DBNull.Value);
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            _connection.Open();

            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM Files WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> RecordExistsForPath(string path)
        {
            _connection.Open();

            var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Files WHERE lower(Path) = lower(@path)";
            command.Parameters.AddWithValue("@path", path);
            var count = await ExeuteScalarAsync(command);

            return count > 0;
        }

        public async Task<long> GetFileCountForDriver(int driverId)
        {
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Files WHERE DriverId = @driverId";
            command.Parameters.AddWithValue("@driverId", driverId);
            var count = await ExeuteScalarAsync(command);
            return count;
        }

        public async Task<long> GetTotalSizeOfFilesForDriver(int driverId)
        {
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT SUM(Size) FROM Files WHERE DriverId = @driverId";
            command.Parameters.AddWithValue("@driverId", driverId);
            var totalSize = await ExeuteScalarAsync(command);
            return totalSize ;
        }

        private async Task<long> ExeuteScalarAsync(SqliteCommand command)
        {
            var response = await command.ExecuteScalarAsync();
            return response == null || response == DBNull.Value ? 0L : (long)response;
        }
    }
}
