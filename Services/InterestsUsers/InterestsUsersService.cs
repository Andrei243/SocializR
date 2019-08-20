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

        public List<Domain.Interest> GetAllInterests(int idUser)
        {
            return unitOfWork.InterestsUserss.Query.AsNoTracking().Include(e => e.Interest).AsNoTracking().Where(e => e.UserId == idUser).Select(e=>e.Interest).ToList();

        }

        public bool AddInterest(int interestId,int userId)
        {
            if (unitOfWork.InterestsUserss.Query.Any(e => e.InterestId == interestId && e.UserId == userId)) return false;
            var legatura = new Domain.InterestsUsers()
            {
                InterestId = interestId,
                UserId = userId
            };
            unitOfWork.InterestsUserss.Add(legatura);
            return unitOfWork.SaveChanges() != 0;

        }

        public bool RemoveInterest(int interestId,int userId)
        {
            if (!unitOfWork.InterestsUserss.Query.Any(e => e.InterestId == interestId && e.UserId == userId)) return false;
            
            unitOfWork.InterestsUserss.Remove(unitOfWork.InterestsUserss.Query.First(e => e.InterestId == interestId && e.UserId == userId));
            return unitOfWork.SaveChanges() != 0;

        }

    }
}
