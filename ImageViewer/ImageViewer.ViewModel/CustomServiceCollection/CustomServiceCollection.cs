using AutoMapper;
using ImageViewer.Service;
using ImageViewer.Service.BackgroundWorkers;
using ImageViewer.Service.File;
using ImageViewer.ViewModel.AutoMapperSetup;
using ImageViewer.ViewModel.Collections;
using ImageViewer.ViewModel.File;
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
        public static ServiceCollection GetServiceCollection()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                //builder.Configure((da)=>);
                builder.SetMinimumLevel(LogLevel.Information);
            });
            services.AddSingleton<FileService>();
            services.AddSingleton<ThumbnailService>();
            services.AddSingleton<ThumbnailBackgroundWorker>();
            services.AddSingleton<HashService>();
            services.AddSingleton<XmlConfigService>(provider =>
            {
                return new XmlConfigService("F:\\.thumbnails_1\\config.xml", 300000);
            });

            services.AddTransient<FilesListViewModel>();
            services.AddTransient<BaseFileViewModel>();
            services.AddTransient<DirectoryInfoViewModel>();
            services.AddTransient<FileInfoViewModel>();
            services.AddAutoMapper((cfg) => cfg.AddProfile<AutoMapperProfile>());
            var buildServiceProvider = services.BuildServiceProvider();

            IMapper mapper = buildServiceProvider.GetRequiredService<IMapper>();
            var fileService = buildServiceProvider.GetRequiredService<FileService>();
            var thumbnailService = buildServiceProvider.GetRequiredService<ThumbnailService>();
            var hashService = buildServiceProvider.GetRequiredService<HashService>();
            var xmlService = buildServiceProvider.GetRequiredService<XmlConfigService>();

            return services;
        }
    }
}
