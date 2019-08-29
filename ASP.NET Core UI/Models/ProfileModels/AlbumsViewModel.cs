using ASP.NET_Core_UI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.ProfileModels
{
    public class AlbumsViewModel
    {

        public AddAlbumModel AddAlbumModel { get; set; }

        public List<AlbumDomainModel> Album { get; set; }

        public bool CanEdit { get; set; }

    }
}
