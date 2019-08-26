using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.JsonModels
{
    public class Image
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Description { get; set; }
    }
}
