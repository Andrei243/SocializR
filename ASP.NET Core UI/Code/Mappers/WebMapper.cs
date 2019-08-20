using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using ASP.NET_Core_UI.Models;
using Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASP.NET_Core_UI.Models.ProfileModels;
using ASP.NET_Core_UI.Models.FeedModels;
using ASP.NET_Core_UI.Models.GeneralModels;

namespace ASP.NET_Core_UI.Code.Mappers
{
    public class WebMapper:Profile
    {

        public static void Run()
        {
            Mapper.Initialize(
                a =>
                {
                    a.AddProfile<WebMapper>();
                }
                );

        }

        public WebMapper()
        {
            CreateMap<Users, LoginModel>();
            CreateMap<LoginModel, Users>();
            CreateMap<Users, EditUserModel>();
            CreateMap<EditUserModel, Users>();
            CreateMap<Users, PostUserModel>();
            CreateMap<PostUserModel, Users>();
            CreateMap<Users, UserDropdownModel>();
            CreateMap<UserDropdownModel, Users>();
            CreateMap<Users, RegisterModel>();
            CreateMap<RegisterModel, Users>();
            CreateMap<County, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));
            CreateMap<Interest, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));
            CreateMap<Users, ProfileViewerModel>()
                .ForMember(dest => dest.Album, s => s.MapFrom(src => new List<Models.DomainModels.Album>(src.Album.Select(e => new Models.DomainModels.Album()
                {
                    Id = e.Id,
                    Count = e.Photo.Count,
                    Name = e.Name,
                }))))
                .ForMember(dest => dest.Interests, s => s.MapFrom(src => new List<string>(src.InterestsUsers.Select(e => e.Interest.Name).ToList())))
                .ForMember(dest=>dest.Locality,s=>s.MapFrom(src=>src.Locality.Name))
                .ForMember(dest=>dest.County,s=>s.MapFrom(src=>src.Locality.County.Name))
                ;
        }

    }
}
