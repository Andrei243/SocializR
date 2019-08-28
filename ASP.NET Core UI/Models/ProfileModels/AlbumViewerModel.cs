using ASP.NET_Core_UI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.ProfileModels
{
    public class AlbumViewerModel
    {
        
        public int Id { get; set; }
        public PhotoModel PhotoModel { get; set; }
        public bool HasThisAlbum { get; set; }
    }
}
