using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ImageViewer.Service.Models;
using ImageViewer.Data;
using ImageViewer.Model.Data;

namespace ImageViewer.Service
{
    public class ImageAnalysisWorker
    {
        private readonly BlockingCollection<(int fileId, string path)> _queue = new();
        private readonly ImageAnalysisService _analysis;
        private readonly FileDataService _fileData;
        private readonly FileMetaInfoService _metaService;
        private readonly ImageViewer.Service.HashService _hashService;
        private readonly IPublisher _publisher;
        private CancellationTokenSource _cts;

        public ImageAnalysisWorker(ImageAnalysisService analysis, FileDataService fileData, FileMetaInfoService metaService, ImageViewer.Service.HashService hashService, IPublisher publisher)
        {
            _analysis = analysis;
            _fileData = fileData;
            _metaService = metaService;
            _hashService = hashService;
            _publisher = publisher;
        }

        public void Enqueue(int fileId, string path)
        {
            _queue.Add((fileId, path));
        }

        public void Start()
        {
            if (_cts != null) return;
            _cts = new CancellationTokenSource();
            Task.Run(() => ProcessQueue(_cts.Token));
        }

        public void Stop()
        {
            try { _cts?.Cancel(); } catch { }
        }

        private async Task ProcessQueue(CancellationToken token)
        {
            foreach (var item in _queue.GetConsumingEnumerable(token))
            {
                try
                {
                    if (token.IsCancellationRequested) break;
                    var (fileId, path) = item;
                    if (!_analysis.IsImageFile(path)) continue;

                    var meta = new FileMetaInfo { FileId = fileId, FileType = System.IO.Path.GetExtension(path) };

                    // hash
                    string hash = null;
                    try { hash = _hashService.GenerateFileHash(path); } catch { }
                    if (!string.IsNullOrEmpty(hash))
                    {
                        await _fileData.UpdateFileAsync(fileId, System.IO.Path.GetFileName(path), path, new System.IO.FileInfo(path).Length, DateTime.UtcNow, DateTime.UtcNow, hash);
                        var dup = await _fileData.GetFileIdByHashAsync(hash);
                        if (dup > 0 && dup != fileId) meta.Duplicate = dup;
                    }

                    var ph = await _analysis.ComputePerceptualHashAsync(path);
                    if (ph.HasValue) meta.AdditionalMetaInfo = ph.Value.ToString();

                    meta.IsBlurred = await _analysis.IsBlurredAsync(path);
                    meta.IsCorrupted = await _analysis.IsCorruptedAsync(path);

                    await _metaService.AddOrUpdateFileMetaInfo(meta);

                    // Publish analysis result
                    try
                    {
                        var result = new ImageViewer.Service.Models.FileAnalysisResult
                        {
                            FileId = fileId,
                            Path = path,
                            IsDuplicate = meta.Duplicate.HasValue,
                            DuplicateOf = meta.Duplicate,
                            IsBlurred = meta.IsBlurred,
                            IsCorrupted = meta.IsCorrupted,
                            AdditionalInfo = meta.AdditionalMetaInfo
                        };
                        _publisher.Publish("analysis.completed", result);
                    }
                    catch { }
                }
                catch { }
            }
        }
    }
}
