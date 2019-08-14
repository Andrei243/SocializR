using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Services.InterestsUsers
{
    public class InterestsUsersService : Base.BaseService
    {
        private readonly CurrentUser CurrentUser;

        public InterestsUsersService(CurrentUser currentUser, SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            CurrentUser = currentUser;
        }

        public List<string> GetAllInterests(int idUser)
        {
            return unitOfWork.InterestsUserss.Query.AsNoTracking().Include(e => e.Interest).AsNoTracking().Where(e => e.UserId == idUser).Select(e=>e.Interest.Name).ToList();

        }

        public bool AddInterest(Domain.Interest interest)
        {
            if (unitOfWork.InterestsUserss.Query.Any(e => e.InterestId == interest.Id && e.UserId == CurrentUser.Id)) return false;
            var legatura = new Domain.InterestsUsers()
            {
                InterestId = interest.Id,
                UserId = CurrentUser.Id
            };
            unitOfWork.InterestsUserss.Add(legatura);
            return unitOfWork.SaveChanges() != 0;

        }

        public bool RemoveInterest(Domain.Interest interest)
        {
            if (!unitOfWork.InterestsUserss.Query.Any(e => e.InterestId == interest.Id && e.UserId == CurrentUser.Id)) return false;
            var legatura = new Domain.InterestsUsers()
            {
                InterestId = interest.Id,
                UserId = CurrentUser.Id
            };
            unitOfWork.InterestsUserss.Remove(legatura);
            return unitOfWork.SaveChanges() != 0;

        }

    }
}
