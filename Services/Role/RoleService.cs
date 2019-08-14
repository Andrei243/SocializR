using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.Role
{
    public class RoleService : Base.BaseService
    {
        private readonly CurrentUser CurrentUser;

        public RoleService(CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            CurrentUser = currentUser;
        }

        protected bool IsAdmin()
        {
            return unitOfWork.Roles.Query.AsNoTracking().Include(e => e.Users).AsNoTracking().Any(e => e.Name == "admin" && e.Users.Select(f => f.Id).Contains(CurrentUser.Id));
        }

    }
}
