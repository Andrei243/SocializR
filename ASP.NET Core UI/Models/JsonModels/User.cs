﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.JsonModels
{
    public class UserJsonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfilePhoto { get; set; }
        public int IsAdmin { get; set; }
        public bool IsBanned { get; set; }
    }
}
