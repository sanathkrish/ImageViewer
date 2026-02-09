using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service
{
    public  class HashService
    {
        // Hash from file path
        public string GeneratePathHash(
            string filePath)
        {
            using var sha = SHA256.Create();

            byte[] bytes =
                Encoding.UTF8.GetBytes(filePath);

            byte[] hash = sha.ComputeHash(bytes);

            return ConvertToHex(hash);
        }

        // Hash from file content
        public string GenerateFileHash(
            string filePath)
        {
            using var sha = SHA256.Create();
            using var stream = System.IO.File.OpenRead(filePath);

            byte[] hash =
                sha.ComputeHash(stream);

            return ConvertToHex(hash);
        }

        // Hash from thumbnail bytes
        public string GenerateThumbHash(
            byte[] thumbnailBytes)
        {
            using var sha = SHA256.Create();

            byte[] hash =
                sha.ComputeHash(thumbnailBytes);

            return ConvertToHex(hash);
        }

        private string ConvertToHex(
            byte[] hash)
        {
            var sb = new StringBuilder();

            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}
