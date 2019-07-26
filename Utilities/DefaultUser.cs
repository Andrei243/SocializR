using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    class DefaultUser : Domain.Users
    {
        private static Domain.Users user=null;
        private DefaultUser()
        {
            Id = -1;
            Name = "default";
            Surname = "default";
            Email = "default";
            Password = "default";
            RoleId = -1;
            BirthDay = DateTime.Now;
            LocalityId = -1;
            SexualIdentity = "default";
            Vizibility = "default";
            Role = new Domain.Role() { Id = -1,Name = "default" };
            Locality = new Domain.Locality() { Id = -1, Name = "default" };
        }

        public static Domain.Users getInstance()
        {
            if (user == null)
            {
                user = new DefaultUser();
            }
            return user;
        }

    }
}
