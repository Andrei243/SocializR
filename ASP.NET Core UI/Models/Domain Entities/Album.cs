using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.Domain_Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int? CoverPhoto { get; set; }
    }
}
