using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace ImageViewer.Service.File
{
    public class XmlConfigService : IDisposable
    {
        private readonly string _xmlPath;

        // Thread-safe buffer
        private readonly ConcurrentDictionary<string,
            ConcurrentDictionary<string, string>> _buffer;

        private readonly System.Timers.Timer _flushTimer;
        private readonly SemaphoreSlim _fileLock = new(1, 1);

        private bool _isFlushing = false;

        public XmlConfigService(
            string xmlPath,
            double flushIntervalMs = 300000) // 5 min
        {
            _xmlPath = xmlPath;
            _buffer = new();

            EnsureXmlExists();

            _flushTimer = new System.Timers.Timer(flushIntervalMs);
            _flushTimer.Elapsed += async (s, e) =>
                await FlushToDiskAsync();

            _flushTimer.Start();
        }

        // Ensure XML exists
        private void EnsureXmlExists()
        {
            if (!System.IO.File.Exists(_xmlPath))
            {
                var doc = new XDocument(
                    new XElement("images",
                        new XElement("thumbnails")
                    )
                );

                doc.Save(_xmlPath);
            }
        }

        // Add / Update → BUFFER ONLY
        public void AddOrUpdateBuffered(
            string collection,
            string key,
            string value)
        {
            var col = _buffer.GetOrAdd(
                collection,
                _ => new ConcurrentDictionary<string, string>());

            col[key] = value;
        }

        // Get value (checks buffer first)
        public async Task<string> GetValueAsync(
            string collection,
            string key)
        {
            // Check buffer first
            if (_buffer.ContainsKey(collection) &&
                _buffer[collection].ContainsKey(key))
            {
                return _buffer[collection][key];
            }

            await _fileLock.WaitAsync();

            try
            {
                var doc = XDocument.Load(_xmlPath);

                return doc.Root
                    .Element("thumbnails")
                    .Elements("thumbnail")
                    .FirstOrDefault(x =>
                        x.Attribute("name")?.Value ==
                        collection)?
                    .Elements("Add")
                    .FirstOrDefault(x =>
                        x.Attribute("key")?.Value == key)?
                    .Attribute("value")?.Value;
            }
            finally
            {
                _fileLock.Release();
            }
        }

        // Flush buffer → XML (ASYNC)
        public async Task FlushToDiskAsync()
        {
            if (_isFlushing || _buffer.IsEmpty)
                return;

            _isFlushing = true;

            await _fileLock.WaitAsync();

            try
            {
                var doc = XDocument.Load(_xmlPath);
                var collectionsNode =
                    doc.Root.Element("thumbnails");

                foreach (var collection in _buffer)
                {
                    var collectionNode =
                        collectionsNode
                        .Elements("thumbnail")
                        .FirstOrDefault(x =>
                            x.Attribute("name")?.Value ==
                            collection.Key);

                    if (collectionNode == null)
                    {
                        collectionNode =
                            new XElement("thumbnail",
                                new XAttribute("name",
                                    collection.Key));

                        collectionsNode.Add(collectionNode);
                    }

                    foreach (var item in collection.Value)
                    {
                        var existing =
                            collectionNode
                            .Elements("Add")
                            .FirstOrDefault(x =>
                                x.Attribute("key")?.Value ==
                                item.Key);

                        if (existing != null)
                        {
                            existing.SetAttributeValue(
                                "value", item.Value);
                        }
                        else
                        {
                            collectionNode.Add(
                                new XElement("Add",
                                    new XAttribute("key",
                                        item.Key),
                                    new XAttribute("value",
                                        item.Value)));
                        }
                    }
                }

                doc.Save(_xmlPath);

                _buffer.Clear();
            }
            finally
            {
                _fileLock.Release();
                _isFlushing = false;
            }
        }

        // Manual flush (sync wrapper)
        public void FlushNow()
        {
            FlushToDiskAsync()
                .GetAwaiter()
                .GetResult();
        }

        // Dispose (App close safe)
        public void Dispose()
        {
            _flushTimer?.Stop();
            _flushTimer?.Dispose();

            // Final flush before exit
            FlushNow();

            _fileLock?.Dispose();
        }
    }
}

