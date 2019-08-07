using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.User
{
    public class UserAccountService : Base.BaseService
    {
        public static readonly int IDROLPUBLIC = 2;
        public UserAccountService(SocializRUnitOfWork unitOfWork):
            base(unitOfWork)
        { }

        public Domain.Users Get(string email)
        {
            return unitOfWork.Users.Query
                .FirstOrDefault(e => e.Email == email);
        }

        public Domain.Users Login(string email, string password)
        {
            return unitOfWork.Users.Query.Include(e=>e.Role)
                .FirstOrDefault(e => e.Email == email && e.Password == password);
        }

        public bool Register(Domain.Users user)
        {
            user.Vizibility = "friends";
            user.RoleId = IDROLPUBLIC;
            unitOfWork.Users.Add(user);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool EmailExists(string email)
        {
            return unitOfWork.Users.Query.Any(e => e.Email == email);
        }

    }
}
