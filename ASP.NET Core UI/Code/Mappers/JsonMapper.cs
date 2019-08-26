﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Code.Mappers
{
    public class JsonMapper : Profile
    {
        public JsonMapper()
        {
            CreateMap<Domain.County, ASP.NET_Core_UI.Models.JsonModels.County>();
            CreateMap<ASP.NET_Core_UI.Models.JsonModels.County, Domain.County>();
            CreateMap<Domain.Locality, ASP.NET_Core_UI.Models.JsonModels.Locality>()
                .ForMember(dest => dest.County, s => s.MapFrom(src => src.County.Name));
            CreateMap<ASP.NET_Core_UI.Models.JsonModels.Locality, Domain.Locality>();
            CreateMap<Domain.Interest, ASP.NET_Core_UI.Models.JsonModels.Interest>();
            CreateMap<ASP.NET_Core_UI.Models.JsonModels.Interest, Domain.Interest>();
            CreateMap<Domain.Comment, ASP.NET_Core_UI.Models.JsonModels.Comment>()
                .ForMember(dest => dest.UserName, s => s.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.UserId, s => s.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.User.PhotoId))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Content));
            CreateMap<ASP.NET_Core_UI.Models.JsonModels.Comment, Domain.Comment>();
            CreateMap<Domain.Post, ASP.NET_Core_UI.Models.JsonModels.Post>()
                .ForMember(dest => dest.UserId, s => s.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, s => s.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.User.PhotoId))
                .ForMember(dest => dest.NoReactions, s => s.MapFrom(src => src.Reaction.Count))
                .ForMember(dest => dest.PhotoId, s => s.MapFrom(src => new List<int>() { src.Photo.Id }));
                

        }
    }
}
