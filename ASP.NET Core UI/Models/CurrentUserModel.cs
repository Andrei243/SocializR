using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models
{
    public class CurrentUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int ProfilePhoto { get; set; }
        public bool isAuthenticated { get; set; }
        public bool IsAdmin { get; set; }

    }
}
