using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.ProfileModels
{
    public class EditUserModel
    {
        public EditUserModel()
        {
            Counties = new List<SelectListItem>();
            InterestsId = new List<int>();
            Interests = new List<SelectListItem>();
            Albume = new List<DomainModels.Album>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public int? LocalityId { get; set; }
        public string SexualIdentity { get; set; }
        public string Visibility { get; set; }
        public int? PhotoId { get; set; }
        public List<int> InterestsId { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<SelectListItem> Interests { get; set; }
        public List<ASP.NET_Core_UI.Models.DomainModels.Album> Albume { get; set; }
    }
}
