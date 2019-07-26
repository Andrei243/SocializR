using System;
using Domain;

namespace Utilities
{
    public class Authenticator
    {
        private Users user;

        public Authenticator(Users _user)
        {
            user = _user;
        }

        public bool isAdmin()
        {
            if (user.Role.Name.Equals("admin")) return true;
            return false;
        }

        public bool isAuthenticated()
        {
            if (user != DefaultUser.getInstance()) return true;
            return false;
        }

    }
}
