using CommunityToolkit.Mvvm.Input;
using ImageViewer.Service.File;
using ImageViewer.ViewModel.Common;
using ImageViewer.ViewModel.Events;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Views
{
    public class MainWindowViewModel:BaseViewModel
    {
        EventAggreator aggreator;
        ScanFileService _scanFileService;
        FileSystemWatcher watcher;

        private RelayCommand _startNewScanCommand;
        private RelayCommand _stopNewScanCommand;

        public DispatcherQueue DispatcherQueue { get; } = DispatcherQueue.GetForCurrentThread();

        public RelayCommand StartNewScanCommand => _startNewScanCommand ??= new RelayCommand(() =>
        {
            aggreator.Publish("start_new_scan");
        });

        public RelayCommand StopNewScanCommand => _stopNewScanCommand ??= new RelayCommand(() =>
        {
            aggreator.Publish("stop_new_scan");
        });
        public MainWindowViewModel(EventAggreator eventAggregator,ScanFileService scanFileService)
        {
            aggreator = eventAggregator;
            _scanFileService = scanFileService;
        }

        public void OnLoaded()
        {
            string dbPath =
             Path.Combine(
                 AppDomain.CurrentDomain.BaseDirectory,
                 "ImageViewer_Data");
            watcher = new FileSystemWatcher(dbPath);
            
            watcher.Filter = "*.*";
            watcher.NotifyFilter =
            NotifyFilters.FileName |
            NotifyFilters.DirectoryName |
            NotifyFilters.LastWrite;
            watcher.Created += OnCreated;
            watcher.Changed += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += (s, e) =>
            {
                Debug.WriteLine($"Database file changed: {e.FullPath}");
                Task.Run(() =>
                {
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        var count = _scanFileService.GetScanedAllFilesResultsCountAsync("F:\\").GetAwaiter().GetResult();
                        aggreator.Publish(EventAggregatorConstants.AllFilesCount, count);
                    } );
                });
            };

            aggreator.Publish<Tuple<long, long>>(EventAggregatorConstants.AllFilesCount, _scanFileService.GetScanedAllFilesResultsCountAsync("F:\\").GetAwaiter().GetResult());

            aggreator.Subscribe("start_new_scan", () =>
            {
                Debug.WriteLine("Start new scan event received in MainWindowViewModel");
                Task.Run(async () => {
                        await _scanFileService.ScanSelectedPath("F:\\");
                } );

            });

            aggreator.Subscribe("stop_new_scan", () =>
            {
                Debug.WriteLine("Stop new scan event received in MainWindowViewModel");
            });

            aggreator.Subscribe<String>("initialize_db_watcher", (dbPath) =>
            {

            });
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Created: {e.FullPath}");
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath}");
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed: {e.OldFullPath} -> {e.FullPath}");
        }
    }
}
