using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.AdminModels
{
    public class UserIndex
    {
        public int Id { get; set; }
        public int? ProfilePhoto { get; set; }
        public string FullName { get; set; }
    }
}
