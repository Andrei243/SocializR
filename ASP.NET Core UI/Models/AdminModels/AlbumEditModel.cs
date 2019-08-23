﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.AdminModels
{
    public class AlbumEditModel
    {
        [Required]
        public int Id { get; set; }
        public List<int> Photos { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
