using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.AdminModels
{
    public class EditLocalityModel
    {
        public List<SelectListItem> CountyIds { get; set; }
        public string Name { get; set; }
        public int CountyId { get; set; }
        public int Id { get; set; }
    }
}
