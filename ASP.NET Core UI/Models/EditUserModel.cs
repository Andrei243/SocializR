using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models
{
    public class EditUserModel
    {
        public EditUserModel()
        {
            Counties = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public int? LocalityId { get; set; }
        public string SexualIdentity { get; set; }
        public string Visibility { get; set; }
        public int? PhotoId { get; set; }
        public List<SelectListItem> Counties { get; set; }
    }
}
