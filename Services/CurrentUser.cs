using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class CurrentUser
    {
        public CurrentUser(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int? ProfilePhoto { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }

    }
}
