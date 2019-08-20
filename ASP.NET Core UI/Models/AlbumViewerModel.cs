using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models
{
    public class AlbumViewerModel
    {
        public PhotoModel PhotoModel { get; set; }
        public List<int> poze { get; set; }
        public bool HasThisAlbum { get; set; }
    }
}
