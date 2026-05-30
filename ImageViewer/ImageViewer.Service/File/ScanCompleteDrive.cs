using System;
using System.Threading.Tasks;
using ImageViewer.Data;
using ImageViewer.Model.Data;
using System.IO;

namespace ImageViewer.Service.File
{
    public class ScanCompleteDrive
    {
        private readonly FileDataService _fileDataService;
        private readonly FileMetaInfoService _fileMetaInfoService;
        private readonly DriverDataService _driverDataService;
        private readonly ImageViewer.Service.HashService _hashService;

        public ScanCompleteDrive(FileDataService fileDataService, FileMetaInfoService fileMetaInfoService, DriverDataService driverDataService, ImageViewer.Service.HashService hashService)
        {
            _fileDataService = fileDataService;
            _fileMetaInfoService = fileMetaInfoService;
            _driverDataService = driverDataService;
            _hashService = hashService;
        }

        public async Task ScanDriverAsync(string driverPath, IProgress<string> progress = null, System.Threading.CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(driverPath) || !Directory.Exists(driverPath))
                throw new ArgumentException("Driver path does not exist", nameof(driverPath));

            // find or create driver
            int driverId = await _driverDataService.GetDriverIdByPathAsync(driverPath);
            if (driverId <= 0)
            {
                var dinfo = new DriverInfo { Name = driverPath, Path = driverPath, Type = "drive", DateAdded = DateTime.UtcNow };
                driverId = await _driverDataService.CreateDriverAsync(dinfo);
            }

            await RecurseScanDirectoryServices(driverId, driverPath, progress, cancellationToken);
        }

        private async Task RecurseScanDirectoryServices(int driverId, string directoryPath, IProgress<string> progress, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                var files = Directory.GetFiles(directoryPath);
                foreach (var file in files)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    try
                    {
                        var fi = new FileInfo(file);
                        var existingId = await _fileDataService.GetFileIdByPathAsync(fi.FullName);
                        if (existingId > 0)
                        {
                            await _fileDataService.UpdateFileAsync(existingId, fi.Name, fi.FullName, fi.Length, fi.CreationTimeUtc, fi.LastWriteTimeUtc, null);
                        }
                        else
                        {
                            existingId = await _fileDataService.AddFileAndGetIdAsync(driverId, fi.Name, fi.FullName, fi.Length, fi.CreationTimeUtc, fi.LastWriteTimeUtc, null);
                        }

                        string hash = null;
                        try { hash = _hashService.GenerateFileHash(fi.FullName); } catch { hash = null; }
                        if (!string.IsNullOrEmpty(hash))
                        {
                            await _fileDataService.UpdateFileAsync(existingId, fi.Name, fi.FullName, fi.Length, fi.CreationTimeUtc, fi.LastWriteTimeUtc, hash);
                            var dupId = await _fileDataService.GetFileIdByHashAsync(hash);
                            if (dupId > 0 && dupId != existingId)
                            {
                                var meta = new FileMetaInfo { FileId = existingId, FileType = Path.GetExtension(fi.Name), Duplicate = dupId, IsEmpty = fi.Length == 0 };
                                await _fileMetaInfoService.AddFileMetaInfo(meta);
                            }
                            else
                            {
                                var meta = new FileMetaInfo { FileId = existingId, FileType = Path.GetExtension(fi.Name), IsEmpty = fi.Length == 0 };
                                await _fileMetaInfoService.AddFileMetaInfo(meta);
                            }
                        }
                        else
                        {
                            var meta = new FileMetaInfo { FileId = existingId, FileType = Path.GetExtension(fi.Name), IsEmpty = fi.Length == 0 };
                            await _fileMetaInfoService.AddFileMetaInfo(meta);
                        }

                        progress?.Report(fi.FullName);
                        if (cancellationToken.IsCancellationRequested)
                            return;
                    }
                    catch { }
                }

                var dirs = Directory.GetDirectories(directoryPath);
                foreach (var dir in dirs)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    try { await RecurseScanDirectoryServices(driverId, dir, progress, cancellationToken); } catch { }
                }
            }
            catch { }
        }
    }
}
