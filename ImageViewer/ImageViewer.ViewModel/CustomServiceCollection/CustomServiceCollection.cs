using AutoMapper;
using ImageViewer.Data;
using ImageViewer.Service;
using ImageViewer.Service.BackgroundWorkers;
using ImageViewer.Service.File;
using ImageViewer.Service.Interfaces;
using ImageViewer.ViewModel.AutoMapperSetup;
using ImageViewer.ViewModel.Collections;
using ImageViewer.ViewModel.Events;
using ImageViewer.ViewModel.Extensions;
using ImageViewer.ViewModel.File;
using ImageViewer.ViewModel.Views;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.CustomServiceCollection
{
    public class CustomServiceCollection
    {
        private static ServiceProvider _serviceProvider;
        private static ServiceCollection _serviceCollection = new ServiceCollection();
        public static ServiceProvider ServiceProvider 
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = GetServiceCollection().BuildServiceProvider();
                }
                return _serviceProvider;
            }
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
        public static void Initilize()
        {
            // Initialize and prepare SQLite database and tables synchronously on startup.
            var dbServiceDescription = new SqlLiteSetupService();
            dbServiceDescription.Initilize();
            dbServiceDescription.CreateTables().GetAwaiter().GetResult();

            // Logging
            _serviceCollection.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Information));

            // SQLite registration
            _serviceCollection.AddSingleton(dbServiceDescription);
            // Use scoped SqliteConnection so each scope gets its own connection instance
            _serviceCollection.AddScoped<SqliteConnection>(provider => provider.GetRequiredService<SqlLiteSetupService>().CreateConnection());

            // Data layer services
            _serviceCollection.AddTransient<FileDataService>();
            _serviceCollection.AddTransient<DriverDataService>();
            _serviceCollection.AddTransient<FileMetaInfoService>();

            // Core services
            // FileService depends on data access and benefits from scoped lifetime
            _serviceCollection.AddScoped<FileService>();
            _serviceCollection.AddSingleton<HashService>();

            // Background workers and thumbnail service
            _serviceCollection.AddSingleton<ThumbnailBackgroundWorker>();
            _serviceCollection.AddSingleton<ThumbnailService>();

            // File utilities
            _serviceCollection.AddTransient<ExtensionService>();
            _serviceCollection.AddTransient<DuplicateImageService>();

            // Ensure unreferenced / auxiliary classes are also registered so they are retained
            _serviceCollection.AddTransient<DuplicateFileCollection>();
            _serviceCollection.AddTransient<DataCollectionViewModel>();
            _serviceCollection.AddTransient<DuplicateFileInfoViewModel>();
            _serviceCollection.AddTransient<ExtensionInfoViewModel>();
            _serviceCollection.AddTransient<NavigateBackItemViewModel>();
            _serviceCollection.AddTransient<BaseItemViewModel>();

            // Background pipes and scan helpers
            _serviceCollection.AddTransient<FindFacePipe>();
            _serviceCollection.AddTransient<ImageComparisionPipe>();
            _serviceCollection.AddTransient<ScanCompleteDrive>();

            // Scan UI viewmodel
            _serviceCollection.AddTransient<ImageViewer.ViewModel.File.ScanDriveViewModel>();

            // Event aggregator (use existing singleton instance)
            _serviceCollection.AddSingleton<EventAggreator>(provider => EventAggreator.Instance);

            // XML config service - path can be overridden by environment variable IMAGEVIEWER_CONFIG_PATH
            var xmlPath = Environment.GetEnvironmentVariable("IMAGEVIEWER_CONFIG_PATH");
            if (string.IsNullOrEmpty(xmlPath))
            {
                xmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".thumbnails_1", "config.xml");
            }
            _serviceCollection.AddSingleton<XmlConfigService>(provider => new XmlConfigService(xmlPath, 300000));

            // ViewModels and collections
            _serviceCollection.AddTransient<FilesListViewModel>();
            _serviceCollection.AddTransient<BaseFileViewModel>();
            _serviceCollection.AddTransient<DirectoryInfoViewModel>();
            _serviceCollection.AddTransient<FileInfoViewModel>();
            _serviceCollection.AddTransient<ExtensionCollectionViewModel>();
            _serviceCollection.AddTransient<NavigationViewModel>();
            _serviceCollection.AddTransient<NavigationItemViewModel>();
            _serviceCollection.AddTransient<FileClassificationTileCollection>();
            _serviceCollection.AddTransient<FileClassificationTileViewModel>();

            // AutoMapper
            _serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

            // Build provider once after all registrations and perform post-start actions
            _serviceProvider = _serviceCollection.BuildServiceProvider();

            // Start background worker via ThumbnailService
            try
            {
                var thumbnailService = _serviceProvider.GetService<ThumbnailService>();
                thumbnailService?.StartWorker();
            }
            catch { }
        }
        public static ServiceCollection GetServiceCollection()
        {
            return _serviceCollection;
            
        }
    }
}
