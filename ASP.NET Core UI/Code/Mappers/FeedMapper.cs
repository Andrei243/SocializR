using ASP.NET_Core_UI.Models.AdminModels;
using ASP.NET_Core_UI.Models.DomainModels;
using ASP.NET_Core_UI.Models.FeedModels;
using ASP.NET_Core_UI.Models.ProfileModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Code.Mappers
{
    public class FeedMapper : Profile
    {
        public FeedMapper()
        {
            CreateMap<Domain.County, County>();
            CreateMap<County, Domain.County>();
            CreateMap<DetailsCountyModel, Domain.County>();
            CreateMap<Domain.County, DetailsCountyModel>();
            CreateMap<EditCountyModel, Domain.County>();
            CreateMap<Domain.County, EditCountyModel>();
            CreateMap<Domain.Users, PostUserModel>()
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.PhotoId))
                .ForMember(dest => dest.Name, s => s.MapFrom(src => src.Name + " " + src.Surname));
            //CreateMap<Domain.Post, PostModel>();  nu are sens deoarece ar insemna sa trec inca o data prin tot vectorul de postari sa le adaug individual
            //                                      reactiile, pozele asociate si comentariile
            CreateMap<PostAddModel, Domain.Post>()
                .ForMember(dest => dest.Vizibilitate, s => s.MapFrom(src => src.Visibility));
            CreateMap<Domain.Interest, Interest>();
            CreateMap<County, SelectListItem>()
                .ForMember(e => e.Value, s => s.MapFrom(dest => dest.Id.ToString()))
                .ForMember(e => e.Text, s => s.MapFrom(dest => dest.Name));
            CreateMap<Domain.Locality, EditLocalityModel>();
            CreateMap<Domain.Album, Album>()
                .ForMember(dest => dest.Count, s => s.MapFrom(src => src.Photo.Count))
                .ForMember(dest => dest.CoverPhoto, s => s.MapFrom(
                       src => src.Photo.Count > 0 ?
                         src.Photo.OrderBy(f => f.Position).FirstOrDefault(f => f.AlbumId == src.Id).Id :
                   -1));
            CreateMap<PhotoModel, Domain.Photo>()
                .ForMember(dest => dest.MIMEType, s => s.MapFrom(src => src.Binar.ContentType));
            CreateMap<EditUserModel, Domain.Users>()
                .ForMember(dest => dest.Vizibility, s => s.MapFrom(src => src.Visibility));

            CreateMap<Domain.Users, EditUserModel>()
                .ForMember(dest => dest.Visibility, s => s.MapFrom(src => src.Vizibility))
                .ForMember(dest => dest.Albume, s => s.MapFrom(src => src.Album.Select(e => new Album
                {
                    Count = e.Photo.Count,
                    CoverPhoto = e.Photo.Count > 0 ? e.Photo.First().Id : -1,
                    Id = e.Id,
                    Name = e.Name
                })));

            CreateMap<Domain.Users, UserIndex>()
                .ForMember(dest => dest.FullName, s => s.MapFrom(src => src.Name + " " + src.Surname))
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.PhotoId));

            CreateMap<Domain.Comment, CommentModel>()
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Content))
                .ForMember(dest => dest.User, s => s.MapFrom(
                       src => new PostUserModel
                       {
                           Id = src.User.Id,
                           Name = src.User.Name + " " + src.User.Surname,
                           ProfilePhoto = src.User.PhotoId
                       }
                       ));

            CreateMap<Domain.Locality, Locality>()
                .ForMember(dest => dest.County, s => s.MapFrom(src =>
                     new County()
                     {
                         Id = src.County.Id,
                         Name = src.County.Name
                     }
                   ));
            CreateMap<Domain.Album, AlbumEditModel>()
                .ForMember(dest => dest.Photos, s => s.MapFrom(src => src.Photo.Select(e => e.Id)));

            CreateMap<PhotoModel, Domain.Photo>()
                .ForMember(dest => dest.AlbumId, s => s.MapFrom(src => src.AlbumId))
                .ForMember(dest => dest.PostId, s => s.MapFrom(src => src.PostId))
                .ForMember(dest => dest.Position, s => s.MapFrom(src => src.Position))
                .ForMember(dest => dest.MIMEType, s => s.MapFrom(src => src.Binar.ContentType));
            CreateMap<Domain.Photo, Photo>();
            CreateMap<Photo, Domain.Photo>();

        }
    }
}
