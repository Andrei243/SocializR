using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.User
{
    public class UserService : Base.BaseService
    {
        private readonly CurrentUser currentUser;
        public UserService(CurrentUser currentUser, SocializRUnitOfWork unitOfWork):
            base(unitOfWork)
        {
            this.currentUser = currentUser;
        }


        public Users getCurrentUser()
        {
            return unitOfWork
                .Users
                .Query
                .AsNoTracking()
                .Include(e => e.Locality).ThenInclude(e => e.County)
                .AsNoTracking()
                .Include(e=>e.Locality)
                .AsNoTracking()
                .Include(e => e.Role)
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == currentUser.Id);
        }
        public Users getUserById(int? id)
        {
            return unitOfWork
                .Users
                .Query
                .AsNoTracking()
                .Include(e=>e.Locality).ThenInclude(e=>e.County)
                .AsNoTracking()
                .Include(e=>e.Role)
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
        }
        public IEnumerable<Users> getAll()
        {
            return unitOfWork.Users.Query.AsNoTracking().Include(e => e.Locality).AsNoTracking().Include(e => e.Role).AsNoTracking();
        }

        public IEnumerable<Users> GetUsersByName(string name)
        {
            return unitOfWork
                .Users
                .Query
                .AsNoTracking()
                .Where(e => (e.Name + e.Surname).Contains(name));
        }

        public void Update(Users user)
        {
            unitOfWork.Users.Update(user);
            unitOfWork.SaveChanges();
        }
    }
}
