using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.DomainModels
{
    public class LocalityDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountyId { get; set; }
        public CountyDomainModel County { get; set; }

    }
}
