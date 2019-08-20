using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.ProfileModels
{
    public class PhotoModel
    {
        public Microsoft.AspNetCore.Http.IFormFile Binar { get; set; }
        public int? AlbumId { get; set; }
        public int? PostId { get; set; }
        public int? Position { get; set; }
    }
}
