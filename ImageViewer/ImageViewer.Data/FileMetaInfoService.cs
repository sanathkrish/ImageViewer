using ImageViewer.Model.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Data
{
    public class FileMetaInfoService
    {
        private SqliteConnection _connection;
        public FileMetaInfoService(SqliteConnection sqliteConnection)
        {
            _connection = sqliteConnection;
        }

        public async Task AddFileMetaInfo(FileMetaInfo fileMetaInfo)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO FileMetaInfo (FileType, FileId, Duplicate, IsBlurred, IsCorrupted, Similar, IsEmpty, AdditionalMetaInfo)
                VALUES (@FileType, @FileId, @Duplicate, @IsBlurred, @IsCorrupted, @Similar, @IsEmpty, @AdditionalMetaInfo);";
            command.Parameters.AddWithValue("@FileType", fileMetaInfo.FileType);
            command.Parameters.AddWithValue("@FileId", fileMetaInfo.FileId);
            command.Parameters.AddWithValue("@Duplicate", fileMetaInfo.Duplicate);
            command.Parameters.AddWithValue("@IsBlurred", fileMetaInfo.IsBlurred);
            command.Parameters.AddWithValue("@IsCorrupted", fileMetaInfo.IsCorrupted);
            command.Parameters.AddWithValue("@Similar", fileMetaInfo.Similar);
            command.Parameters.AddWithValue("@IsEmpty", fileMetaInfo.IsEmpty);
            command.Parameters.AddWithValue("@AdditionalMetaInfo", fileMetaInfo.AdditionalMetaInfo);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<FileMetaInfo> GetFileMetaInfoByFileId(int fileId)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT Id, FileType, FileId, Duplicate, IsBlurred, IsCorrupted, Similar, IsEmpty, AdditionalMetaInfo FROM FileMetaInfo WHERE FileId = @fileId";
            command.Parameters.AddWithValue("@fileId", fileId);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new FileMetaInfo
                {
                    Id = reader.GetInt32(0),
                    FileType = reader.GetString(1),
                    FileId = reader.GetInt32(2),
                    Duplicate = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                    IsBlurred = reader.GetBoolean(4),
                    IsCorrupted = reader.GetBoolean(5),
                    Similar = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                    IsEmpty = reader.GetBoolean(7),
                    AdditionalMetaInfo = reader.IsDBNull(8) ? null : reader.GetString(8)
                };
            }
            return null;
        }

        public async Task<bool> UpdateFileMetaInfo(FileMetaInfo fileMetaInfo)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                UPDATE FileMetaInfo
                SET FileType = @FileType, Duplicate = @Duplicate, IsBlurred = @IsBlurred, IsCorrupted = @IsCorrupted, Similar = @Similar, IsEmpty = @IsEmpty, AdditionalMetaInfo = @AdditionalMetaInfo
                WHERE FileId = @FileId";
            command.Parameters.AddWithValue("@FileType", fileMetaInfo.FileType);
            command.Parameters.AddWithValue("@Duplicate", fileMetaInfo.Duplicate);
            command.Parameters.AddWithValue("@IsBlurred", fileMetaInfo.IsBlurred);
            command.Parameters.AddWithValue("@IsCorrupted", fileMetaInfo.IsCorrupted);
            command.Parameters.AddWithValue("@Similar", fileMetaInfo.Similar);
            command.Parameters.AddWithValue("@IsEmpty", fileMetaInfo.IsEmpty);
            command.Parameters.AddWithValue("@AdditionalMetaInfo", fileMetaInfo.AdditionalMetaInfo);
            command.Parameters.AddWithValue("@FileId", fileMetaInfo.FileId);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteFileMetaInfoByFileId(int fileId)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM FileMetaInfo WHERE FileId = @fileId";
            command.Parameters.AddWithValue("@fileId", fileId);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> RecordExistsForFileId(int fileId)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM FileMetaInfo WHERE FileId = @fileId";
            command.Parameters.AddWithValue("@fileId", fileId);
            var count = (long)await command.ExecuteScalarAsync();
            return count > 0;
        }
    }
}
