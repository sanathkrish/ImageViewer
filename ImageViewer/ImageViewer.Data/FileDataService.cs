using ImageViewer.Model.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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

        public async Task AddFileAsync(string name, string path, long size, DateTime dateAdded, DateTime? modifiedDate = null, string hash = null)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Files (Name, Path, Size, DateAdded, ModifiedDate, Hash)
                VALUES (@name, @path, @size, @dateAdded, @modifiedDate, @hash)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@path", path);
            command.Parameters.AddWithValue("@size", size);
            command.Parameters.AddWithValue("@dateAdded", dateAdded);
            command.Parameters.AddWithValue("@modifiedDate", modifiedDate.HasValue ? (object)modifiedDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@hash", hash ?? (object)DBNull.Value);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<int> AddFileAndGetIdAsync(int driverId, string name, string path, long size, DateTime dateAdded, DateTime? modifiedDate = null, string hash = null)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Files (DriverId, Name, Path, Size, DateAdded, ModifiedDate, Hash)
                VALUES (@driverId, @name, @path, @size, @dateAdded, @modifiedDate, @hash);";
            command.Parameters.AddWithValue("@driverId", driverId);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@path", path);
            command.Parameters.AddWithValue("@size", size);
            command.Parameters.AddWithValue("@dateAdded", dateAdded);
            command.Parameters.AddWithValue("@modifiedDate", modifiedDate.HasValue ? (object)modifiedDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@hash", hash ?? (object)DBNull.Value);
            await command.ExecuteNonQueryAsync();
            using var idCmd = _connection.CreateCommand();
            idCmd.CommandText = "SELECT last_insert_rowid();";
            var last = await idCmd.ExecuteScalarAsync();
            return Convert.ToInt32(last);
        }

        public async Task<int> GetFileIdByPathAsync(string path)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT Id FROM Files WHERE lower(Path) = lower(@path) LIMIT 1;";
            cmd.Parameters.AddWithValue("@path", path);
            var res = await cmd.ExecuteScalarAsync();
            if (res == null || res == DBNull.Value) return -1;
            return Convert.ToInt32(res);
        }

        public async Task<int> GetFileIdByHashAsync(string hash)
        {
            if (string.IsNullOrEmpty(hash)) return -1;
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT Id FROM Files WHERE Hash = @hash LIMIT 1;";
            cmd.Parameters.AddWithValue("@hash", hash);
            var res = await cmd.ExecuteScalarAsync();
            if (res == null || res == DBNull.Value) return -1;
            return Convert.ToInt32(res);
        }

        public async Task<List<FileRecord>> GetAllFilesAsync(string driverId)
        {
            var files = new List<FileRecord>();
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT Id, DriverId, Name, Path, Size, DateAdded, ModifiedDate, Hash FROM Files WHERE DriverId = @driverId";
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
                    ModifiedDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                    Hash = reader.IsDBNull(7) ? null : reader.GetString(7)
                });
            }
            return files;
        }

        public async Task<bool> UpdateFileAsync(int id, string name, string path, long size, DateTime dateAdded, DateTime? modifiedDate = null, string hash = null)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                UPDATE Files
                SET Name = @name, Path = @path, Size = @size, DateAdded = @dateAdded, ModifiedDate = @modifiedDate, Hash = @hash
                WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@path", path);
            command.Parameters.AddWithValue("@size", size);
            command.Parameters.AddWithValue("@dateAdded", dateAdded);
            command.Parameters.AddWithValue("@modifiedDate", modifiedDate.HasValue ? (object)modifiedDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@hash", hash ?? (object)DBNull.Value);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM Files WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> RecordExistsForPath(string path)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Files WHERE lower(Path) = lower(@path)";
            command.Parameters.AddWithValue("@path", path);
            var count = (long)await command.ExecuteScalarAsync();
            return count > 0;
        }
    }
}
