using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.DomainEntities
{
    public class County
    {
        public int Id { get; set; }
        public int Name { get; set; }

        public List<Locality> Localities { get; set; }
    }
}
