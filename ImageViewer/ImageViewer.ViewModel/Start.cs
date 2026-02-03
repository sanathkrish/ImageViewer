using AutoMapper;
using ImageViewer.Model.Pagination;
using ImageViewer.ViewModel.Collections;
using ImageViewer.ViewModel.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel
{
    public  class Start
    {
        public static void Main()
        {
           var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperSetup.AutoMapperProfile>();
            },null);
            IMapper mapper = config.CreateMapper();
            var fileService = new Service.File.FileService();
            var file = fileService.GetFiles(new PaginationDataRequest<string>
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = "F:\\"
            }).Result;
            var basefiles = mapper.Map<PaginationDataResponse<BaseFileViewModel>>(file);
            var vm = new FilesListViewModel(fileService);
            
            // Mapper is now configured and can be used throughout the application 
        }
    }
}
