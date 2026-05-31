using ImageViewer.Data;
using ImageViewer.Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.File
{
    public class ScanFileService
    {
        private FileDataService _fileDataService;
        private DriverDataService _driverDataService;
        public ScanFileService(FileDataService fileDataService,DriverDataService driverDataService)
        {
            _fileDataService = fileDataService;
            _driverDataService = driverDataService;
        }


        public async Task<Tuple<long, long>> GetScanedAllFilesResultsCountAsync(string path)
        {
            var driverInfo = await _driverDataService.GetFileDriversAsync(Path.GetPathRoot(path));
            if (driverInfo == null) return Tuple.Create(0L, 0L);

            var fileCount = await _fileDataService.GetFileCountForDriver(driverInfo.Id);
            var totalSize = await _fileDataService.GetTotalSizeOfFilesForDriver(driverInfo.Id);
            return Tuple.Create(fileCount, totalSize);
        }

        public async Task<bool> ScanSelectedPath(string path)
        {
           var driver = Path.GetPathRoot(path);
            if(string.IsNullOrEmpty(path)) return false;
            var driverInfo = await _driverDataService.GetFileDriversAsync(path);
            if (driverInfo == null)
            {
                var driverDetails = System.IO.DriveInfo.GetDrives().FirstOrDefault(x=>x.Name == Path.GetPathRoot(driver));
                if (driverDetails == null) { return false; }
                driverInfo = new DriverInfo
                {
                    Name = driverDetails.Name,
                    Path = driver,
                    Type = driverDetails.DriveType.ToString(),
                    TotalSize = driverDetails.TotalSize,
                    FreeSpace = driverDetails.TotalFreeSpace,
                    DateAdded = DateTime.UtcNow
                };

               await  _driverDataService.AddFileDriver(driverInfo);
               driverInfo = await _driverDataService.GetFileDriversAsync(path);
            }
            await RecurrentFileScanAsync(path, driverInfo.Id);
            return true;
        }

        public async Task RecurrentFileScanAsync(string path, int driverId)
        {
            var files = await GetFiles(path);
            if(files != null)
            {
                var dataFiles = files.Where(x=>!x.Attributes.HasFlag(FileAttributes.Directory));
                var fileRecords= new List<FileRecord>();
                foreach (var dataFile in dataFiles)
                {
                    var fileInfo = new FileInfo(dataFile.FullName);
                    var recordFile = new FileRecord() 
                    {
                        Name = dataFile.Name,
                        Path = dataFile.FullName,
                        DateAdded = dataFile.CreationTime,
                        DriverId = driverId,
                        ModifiedDate = dataFile.LastWriteTimeUtc,
                        Size = fileInfo.Length
                    };
                    fileRecords.Add(recordFile);
                }
                await AddOrUpdateFileEntry(fileRecords);
                var directories = files.Where(x => x.Attributes.HasFlag(FileAttributes.Directory));
                foreach (var directory in directories) 
                {
                    await RecurrentFileScanAsync(directory.FullName, driverId);
                }
            }
        }

        public string ComputeHash(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                using var stream = System.IO.File.OpenRead(filePath);
                var hash = sha256.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
            catch (Exception ex) {
            return null;
            }
           
        }

        private async Task<IEnumerable<System.IO.FileSystemInfo>> GetFiles(string path) 
        {
            try
            {
                if (Directory.Exists(path))
                {
                    var dirInfo = new DirectoryInfo(path);
                    return dirInfo.GetFileSystemInfos();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            return null;
        }

        public async Task AddOrUpdateFileEntry(List<FileRecord> files)
        {
            if (files == null || files.Count == 0) return;
            foreach (var file in files)
            {
                await _fileDataService.AddOrUpdateFileAsync(file);
            }
        }
    }
}
