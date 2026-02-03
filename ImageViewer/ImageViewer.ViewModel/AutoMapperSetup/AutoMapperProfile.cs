using AutoMapper;
using ImageViewer.Model;
using ImageViewer.Model.File;
using ImageViewer.ViewModel.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.AutoMapperSetup
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            // Add your mappings here
            // CreateMap<Source, Destination>();
            this.ConfigureMappings();

        }

        public void ConfigureMappings()
        {
            // Additional configuration if needed

            CreateMap<BaseFile, BaseFileViewModel>().ReverseMap();
            CreateMap<FileInfo, FileInfoViewModel>().ReverseMap();
            CreateMap<DirectoryDetails, DirectoryInfoViewModel>().ReverseMap();
                //.ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files))
                //.ForMember(dest => dest.SubDirectories, opt => opt.MapFrom(src => src.SubDirectories));
        }
    }
}
