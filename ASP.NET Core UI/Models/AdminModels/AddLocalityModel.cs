using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.AdminModels
{
    public class AddLocalityModel
    {

        public List<SelectListItem> CountyIds { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CountyId { get; set; }
    }
}
