using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_UI.Models.JsonModels;

namespace ASP.NET_Core_UI.Code.Mappers
{
    public class JsonMapper : Profile
    {
        public JsonMapper()
        {
            CreateMap<Domain.County, County>();
            CreateMap<County, Domain.County>();
            CreateMap<Domain.Locality,Locality>()
                .ForMember(dest => dest.County, s => s.MapFrom(src => src.County.Name));
            CreateMap<Locality, Domain.Locality>();
            CreateMap<Domain.Interest, Interest>();
            CreateMap<Interest, Domain.Interest>();
            CreateMap<Domain.Comment, Comment>()
                .ForMember(dest => dest.UserName, s => s.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.UserId, s => s.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.User.PhotoId))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Content));
            CreateMap<Comment, Domain.Comment>();
            CreateMap<Domain.Post, Post>()
                .ForMember(dest => dest.UserId, s => s.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, s => s.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.User.PhotoId))
                .ForMember(dest => dest.NoReactions, s => s.MapFrom(src => src.Reaction.Count))
                .ForMember(dest => dest.PhotoId, s => s.MapFrom(src => new List<int>() { src.Photo.Id }));
            CreateMap<Domain.Photo, Image>();
            CreateMap<Domain.Users, Friend>()
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.PhotoId))
                .ForMember(dest => dest.Name, s => s.MapFrom(src => src.Name + " " + src.Surname));
            CreateMap<Domain.Users, User>()
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.PhotoId))
                .ForMember(dest => dest.Name, s => s.MapFrom(src => src.Name + " " + src.Surname))
                .ForMember(dest => dest.IsAdmin, s => s.MapFrom(src => src.RoleId == 2))
                .ForMember(dest => dest.IsBanned, s => s.MapFrom(src => src.IsBanned));
            CreateMap<Domain.Interest,InterestSelect>()
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, s => s.MapFrom(src => src.Id));
                

        }
    }
}
