﻿using ASP.NET_Core_UI.Models.DomainModels;
using ASP.NET_Core_UI.Models.JsonModels;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.ProfileModels
{
    public class EditUserModel
    {
        public EditUserModel()
        {
            Counties = new List<SelectListItem>();
            Interests = new List<InterestSelectJsonModel>();
            Albume = new List<AlbumDomainModel>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public int? LocalityId { get; set; }
        [Required]
        public string SexualIdentity { get; set; }
        [Required]
        public string Visibility { get; set; }
        public int? PhotoId { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<InterestSelectJsonModel> Interests { get; set; }
        public List<AlbumDomainModel> Albume { get; set; }
        public AddAlbumModel AddAlbumModel { get; set; }
    }

}
