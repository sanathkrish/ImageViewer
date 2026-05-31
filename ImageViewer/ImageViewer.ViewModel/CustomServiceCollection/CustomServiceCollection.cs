using AutoMapper;
using ImageViewer.Data;
using ImageViewer.Service;
using ImageViewer.Service.BackgroundWorkers;
using ImageViewer.Service.File;
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
            var dbServiceDescription = new SqlLiteSetupService();
            dbServiceDescription.Initilize();
            dbServiceDescription.CreateTables().GetAwaiter().GetResult();
            
            _serviceCollection.AddLogging(builder =>
            {
                //builder.Configure((da)=>);
                builder.SetMinimumLevel(LogLevel.Information);
            });
            _serviceCollection.AddTransient<SqliteConnection>((a)=> dbServiceDescription.CreateConnection());
            _serviceCollection.AddTransient<FileDataService>();

            _serviceCollection.AddSingleton<FileService>();
            _serviceCollection.AddSingleton<ThumbnailService>(new ThumbnailService(new ThumbnailBackgroundWorker()));
            _serviceCollection.AddSingleton<ThumbnailBackgroundWorker>();
            _serviceCollection.AddSingleton<HashService>();
            _serviceCollection.AddTransient<ExtensionService>();
            _serviceCollection.AddTransient<DuplicateImageService>();
            _serviceCollection.AddTransient<DriverDataService>();
            _serviceCollection.AddTransient<FileDataService>();
            _serviceCollection.AddTransient<FileMetaInfoService>();
            _serviceCollection.AddTransient<ScanFileService>();
            _serviceCollection.AddSingleton<XmlConfigService>(provider =>
            {
                return new XmlConfigService("F:\\.thumbnails_1\\config.xml", 300000);
            });

            _serviceCollection.AddTransient<FilesListViewModel>();
            _serviceCollection.AddTransient<BaseFileViewModel>();
            _serviceCollection.AddTransient<DirectoryInfoViewModel>();
            _serviceCollection.AddTransient<FileInfoViewModel>();
            _serviceCollection.AddTransient<ExtensionCollectionViewModel>();
            _serviceCollection.AddTransient<NavigationViewModel>();
            _serviceCollection.AddTransient<NavigationItemViewModel>();
            _serviceCollection.AddTransient<FileClassificationTileCollection>();
            _serviceCollection.AddTransient<FileClassificationTileViewModel>();
            _serviceCollection.AddTransient<MainWindowViewModel>();
            _serviceCollection.AddSingleton<EventAggreator>(EventAggreator.Instance);
            _serviceCollection.AddAutoMapper((cfg) => cfg.AddProfile<AutoMapperProfile>());
            var buildServiceProvider = _serviceCollection.BuildServiceProvider();

            IMapper mapper = buildServiceProvider.GetRequiredService<IMapper>();
            var fileService = buildServiceProvider.GetRequiredService<FileService>();
            var thumbnailService = buildServiceProvider.GetRequiredService<ThumbnailService>();
            var hashService = buildServiceProvider.GetRequiredService<HashService>();
            var xmlService = buildServiceProvider.GetRequiredService<XmlConfigService>();
        }
        public static ServiceCollection GetServiceCollection()
        {
            return _serviceCollection;
            
        }
    }
}
