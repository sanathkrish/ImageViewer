using ImageViewer.Model.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.File
{
    public class DuplicateImageService
    {
        public async Task ScanDuplicateFiles(string driverOrFolder)
        {
            var files = await ScanFiles(driverOrFolder);
            Debug.WriteLine("Total files scanned: " + files.Count);
        }

        public async  Task<List<DuplicateFileInfo>> ScanFiles(string driverOrFolder)
        {
           List<DuplicateFileInfo> files = new List<DuplicateFileInfo>();
            if (!System.IO.Directory.Exists(driverOrFolder)) 
            {
                return files;
            }
            try
            {
            var directories = System.IO.Directory.GetDirectories(driverOrFolder);
            var filesInCurrentDirectory = System.IO.Directory.GetFiles(driverOrFolder);
            foreach ( var file in filesInCurrentDirectory)
            {
                var fileInfo = new System.IO.FileInfo(file);
                var duplicateFileInfo = new DuplicateFileInfo
                {
                    FilePath = file,
                    FileName = Path.GetFileNameWithoutExtension(fileInfo.Name),
                    FileSize = fileInfo.Length,
                    CreationDate = fileInfo.CreationTime,
                    LastModifiedDate = fileInfo.LastWriteTime,
                    Hash = string.Empty, // Hash calculation can be implemented here
                    IsDuplicate = false, // This will be determined later
                    DuplicateOf = string.Empty, // This will be set if it's a duplicate
                    Extension = Path.GetExtension(fileInfo.Name)
                };
                files.Add(duplicateFileInfo);
            }
            foreach (var directory in directories)
            {
                var subFiles = await ScanFiles(directory);
                files.AddRange(subFiles);
            }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., access denied)
                Debug.WriteLine($"Error scanning directory: {ex.Message}");
            }

            return files;
        }
    }
}
