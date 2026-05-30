using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer.Service.File;
using ImageViewer.ViewModel.Events;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ImageViewer.ViewModel.File
{
    public class ScanDriveViewModel : ObservableObject
    {
        private readonly ScanCompleteDrive _scanner;
        private readonly EventAggreator _events;

        private System.Threading.CancellationTokenSource _cts;

        public ScanDriveViewModel(ScanCompleteDrive scanner)
        {
            _scanner = scanner;
            _events = EventAggreator.Instance;
            _events.Subscribe<string>("scan.progress", OnProgress);
            _events.Subscribe<ImageViewer.Service.Models.FileAnalysisResult>("analysis.completed", OnAnalysisCompleted);
            StartScanCommand = new AsyncRelayCommand<string>(StartScanAsync);
            StopScanCommand = new RelayCommand(() => { CancelScan(); });
            ProgressItems = new ObservableCollection<string>();
            DuplicateItems = new ObservableCollection<ImageViewer.Service.Models.FileAnalysisResult>();
            BlurredItems = new ObservableCollection<ImageViewer.Service.Models.FileAnalysisResult>();
            CorruptedItems = new ObservableCollection<ImageViewer.Service.Models.FileAnalysisResult>();
        }

        // UI-bound properties
        public ObservableCollection<string> ProgressItems { get; }
        public ObservableCollection<ImageViewer.Service.Models.FileAnalysisResult> DuplicateItems { get; }
        public ObservableCollection<ImageViewer.Service.Models.FileAnalysisResult> BlurredItems { get; }
        public ObservableCollection<ImageViewer.Service.Models.FileAnalysisResult> CorruptedItems { get; }

        private string _currentFile;
        public string CurrentFile { get => _currentFile; set => SetProperty(ref _currentFile, value); }

        private int _filesScanned;
        public int FilesScanned { get => _filesScanned; set => SetProperty(ref _filesScanned, value); }

        private bool _isScanning;
        public bool IsScanning { get => _isScanning; set => SetProperty(ref _isScanning, value); }

        // Stop command (no-op until cancellation implemented)
        public IRelayCommand StopScanCommand { get; }

        private void OnProgress(string path)
        {
            // Update UI-bound properties
            try
            {
                // Ensure updates happen on UI thread — progress callbacks are posted to UI via Progress<T>
                ProgressItems.Add(path);
                FilesScanned = ProgressItems.Count;
                CurrentFile = path;
            }
            catch { }
        }

        private void OnAnalysisCompleted(ImageViewer.Service.Models.FileAnalysisResult result)
        {
            try
            {
                if (result.IsDuplicate)
                    DuplicateItems.Add(result);
                if (result.IsBlurred)
                    BlurredItems.Add(result);
                if (result.IsCorrupted)
                    CorruptedItems.Add(result);
            }
            catch { }
        }

        public IAsyncRelayCommand StartScanCommand { get; }

        private async Task StartScanAsync(string path)
        {
            IsScanning = true;
            ProgressItems.Clear();
            FilesScanned = 0;
            CurrentFile = null;

            _cts?.Cancel();
            _cts = new System.Threading.CancellationTokenSource();

            var progress = new Progress<string>(p => _events.Publish("scan.progress", p));
            try
            {
                await Task.Run(async () => await _scanner.ScanDriverAsync(path, progress, _cts.Token));
            }
            finally
            {
                IsScanning = false;
            }
        }

        private void CancelScan()
        {
            try { _cts?.Cancel(); } catch { }
        }
    }
}
