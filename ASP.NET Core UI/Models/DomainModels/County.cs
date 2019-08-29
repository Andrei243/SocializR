using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.DomainModels
{
    public class CountyDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<LocalityDomainModel> Localities { get; set; }
    }
}
