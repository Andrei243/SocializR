﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.AdminModels
{
    public class EditCountyModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
