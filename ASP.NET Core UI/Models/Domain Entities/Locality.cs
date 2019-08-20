﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.DomainEntities
{
    public class Locality
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int CountyId { get; set; }
        public County County { get; set; }

    }
}