using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.AdminModels
{
    public class AlbumEditModel
    {
        public int Id { get; set; }
        public List<int> Photos { get; set; }
        public string Name { get; set; }

    }
}
