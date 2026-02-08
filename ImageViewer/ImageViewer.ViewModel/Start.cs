using AutoMapper;
using ImageViewer.Model.Pagination;
using ImageViewer.Service.BackgroundWorkers;
using ImageViewer.Service.File;
using ImageViewer.ViewModel.AutoMapperSetup;
using ImageViewer.ViewModel.Collections;
using ImageViewer.ViewModel.File;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel
{
    public  class Start
    {
        public static void Main()
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

            services.AddTransient<FilesListViewModel>();
            services.AddTransient<BaseFileViewModel>();
            services.AddTransient<DirectoryInfoViewModel>();
            services.AddTransient<FileInfoViewModel>();
            services.AddAutoMapper((cfg)=>cfg.AddProfile<AutoMapperProfile>());
            var buildServiceProvider = services.BuildServiceProvider();

            IMapper mapper = buildServiceProvider.GetRequiredService<IMapper>();
            var fileService = buildServiceProvider.GetRequiredService<FileService>(); 
            var thumbnailService = buildServiceProvider.GetRequiredService<ThumbnailService>();
            var xmlService = new XMLFileService();
            xmlService.CreateXMLFile("F:\\test.xml", "<root><test>Test Content</test></root>");
            //var imageFiles = fileService.GetAllImageFiles("F:\\").ToList();
            //Console.WriteLine("Image files found: " + imageFiles.Count);
            //imageFiles.ForEach(file => {

            //    if (new List<string> { ".thumbnails_1" }.Any(ignore =>
            //    file.Contains(
            //        Path.DirectorySeparatorChar + ignore +
            //        Path.DirectorySeparatorChar)))
            //    {
            //        return;
            //    }
            //    thumbnailService.GetThumbnail(file, (thumbnail) =>
            //    {
            //        // Handle the thumbnail (e.g., update UI)
            //        Console.WriteLine("Thumbnail received for: " + file);
            //        System.IO.File.WriteAllBytes("F:\\.thumbnails_1\\"+Path.GetFileName(file), thumbnail);
            //    });
            //    Task.Delay(100).Wait(); // Simulate some delay in processing each file
            //});
            //var fileService = new Service.File.FileService();
            //var file = fileService.GetFiles(new PaginationDataRequest<string>
            //{
            //    PageNumber = 1,
            //    PageSize = 10,
            //    Filter = "F:\\"
            //}).GetAwaiter().GetResult();
            //var basefiles = mapper.Map<PaginationDataResponse<BaseFileViewModel>>(file);
            //var vm =buildServiceProvider.GetRequiredService<FilesListViewModel>();
            //vm.Initilize<String>("F:\\");
            Console.ReadKey();
            
            // Mapper is now configured and can be used throughout the application 
        }
    }
}
