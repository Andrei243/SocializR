using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Code.Mappers
{
    public class AlbumMapper : Profile
    {
        public AlbumMapper()
        {
            //CreateMap<Album, AlbumVM>
            //    .ForMember(destinatie => destinatie.username, s => s.MapFrom(sursa => sursa.User.nume))
        }
    }
}
