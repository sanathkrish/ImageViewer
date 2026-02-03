using ImageViewer.Model;
using ImageViewer.Model.Pagination;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.File
{
    public class FileService
    {
        public async Task<PaginationDataResponse<BaseFile>> GetFiles(PaginationDataRequest<string> request)
        {
            var files = new PaginationDataResponse<BaseFile>();
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (Directory.Exists(request.Filter))
            {
                var t = new Task<PaginationDataResponse<BaseFile>>(() => {
                    var paginationDataResponse = new PaginationDataResponse<BaseFile>();
                    var directory = new DirectoryInfo(request.Filter);
                    var filesRequest = directory.EnumerateFileSystemInfos();
                    paginationDataResponse.TotalRecords = filesRequest.Count();

                    var fileData = filesRequest.OrderBy(f => f.Name)
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .Select(f =>
                        {
                            if (f is FileInfo fileInfo)
                            {
                                return new Model.File.FileInfo
                                {
                                    Name = fileInfo.Name,
                                    Path = fileInfo.FullName,
                                    FileType = Model.Common.FileType.File,
                                    Created = fileInfo.CreationTime,
                                    LastModified = fileInfo.LastWriteTime
                                } as BaseFile;
                            }
                            else if (f is DirectoryInfo dirInfo)
                            {
                                return new Model.File.DirectoryDetails
                                {
                                    Name = dirInfo.Name,
                                    Path = dirInfo.FullName,
                                    FileType = Model.Common.FileType.Directory,
                                    Created = dirInfo.CreationTime,
                                    LastModified = dirInfo.LastWriteTime
                                } as BaseFile;
                            }
                            else
                            {
                                return null;
                            }
                        })
                        .Where(f => f != null)
                        .ToList();
                    return files;
                }).ConfigureAwait(false);
                files = await t;
            }
            else
            {
                throw new DirectoryNotFoundException($"The directory '{request.Filter}' does not exist.");
            }

            return files;

        }
    }
}
