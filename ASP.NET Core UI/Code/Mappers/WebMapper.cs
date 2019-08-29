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
using ASP.NET_Core_UI.Models.DomainModels;

namespace ASP.NET_Core_UI.Code.Mappers
{
    public class WebMapper:Profile
    {

        public WebMapper()
        {
            CreateMap<Users, LoginModel>();
            CreateMap<LoginModel, Users>();

            CreateMap<Users, UserDropdownModel>();
            CreateMap<UserDropdownModel, Users>();

            CreateMap<Users, RegisterModel>();
            CreateMap<RegisterModel, Users>();

            CreateMap<County, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));

            CreateMap<Interest, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));

            CreateMap<Locality, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));

            CreateMap<Users, ProfileViewerModel>()
                .ForMember(dest => dest.Album, s => s.MapFrom(src => new List<AlbumDomainModel>(src.Album.Select(e => new AlbumDomainModel()
                {
                    Id = e.Id,
                    Count = e.Photo.Count,
                    Name = e.Name,
                }))))
                .ForMember(dest => dest.Interests, s => s.MapFrom(src => new List<string>(src.InterestsUsers.Select(e => e.Interest.Name).ToList())))
                .ForMember(dest=>dest.Locality,s=>s.MapFrom(src=>src.Locality.Name))
                .ForMember(dest=>dest.County,s=>s.MapFrom(src=>src.Locality.County.Name))
                .ForMember(dest=>dest.Birthday,s=>s.MapFrom(src=>src.BirthDay.Year+"."+src.BirthDay.Month+"."+src.BirthDay.Day))
                .ForMember(dest=>dest.PhotoId,s=>s.MapFrom(src=>src.PhotoId))
                ;
            
        }

    }
}
