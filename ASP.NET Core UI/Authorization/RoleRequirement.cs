using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NET_Core_UI.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }
        public RoleRequirement(string role)
        {
            Role = role;
        }

    }
}
