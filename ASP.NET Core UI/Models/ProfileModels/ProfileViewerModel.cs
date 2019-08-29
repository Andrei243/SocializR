﻿using ASP.NET_Core_UI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.ProfileModels
{
    public class ProfileViewerModel
    {

        public bool CanSendRequest { get; set; }
        public bool CanSee { get; set; }
        public bool IsRequested { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string County { get; set; }
        public string Birthday { get; set; }
        public string SexualIdentity { get; set; }
        public string Locality { get; set; }
        public List<string> Interests { get; set; }
        public List<AlbumDomainModel> Album { get; set; }
        public int? PhotoId { get; set; }
        public AddAlbumModel AddAlbumModel { get; set; }
    }
}
