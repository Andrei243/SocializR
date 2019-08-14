using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using ASP.NET_Core_UI.Models;
using Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NET_Core_UI.Code.Mappers
{
    public class WebMapper:Profile
    {

        public static void Run()
        {
            AutoMapper.Mapper.Initialize(
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
            CreateMap<Users, UserFriendModel>();
            CreateMap<UserFriendModel, Users>();
            CreateMap<Users, RegisterModel>();
            CreateMap<RegisterModel, Users>();
            CreateMap<County, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));
            CreateMap<Interest, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));
        }

    }
}
