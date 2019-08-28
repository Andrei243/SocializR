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
       // private readonly CurrentUser CurrentUser;

        public InterestsUsersService( SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            //CurrentUser = currentUser;
        }

        public List<Domain.Interest> GetAllInterests(int idUser)
        {
            return unitOfWork.InterestsUserss.Query.AsNoTracking().Include(e => e.Interest).AsNoTracking().Where(e => e.UserId == idUser).Select(e=>e.Interest).ToList();

        }

        //public bool AddInterest(int interestId,int userId)
        //{
        //    if (unitOfWork.InterestsUserss.Query.Any(e => e.InterestId == interestId && e.UserId == userId)) return false;
        //    var legatura = new Domain.InterestsUsers()
        //    {
        //        InterestId = interestId,
        //        UserId = userId
        //    };
        //    unitOfWork.InterestsUserss.Add(legatura);
        //    return unitOfWork.SaveChanges() != 0;

        //}

        //public bool RemoveInterest(int interestId,int userId)
        //{
        //    var interest = unitOfWork.InterestsUserss.Query.FirstOrDefault(e => e.InterestId == interestId && e.UserId == userId);
        //    if (interest==null) return false;
            
        //    unitOfWork.InterestsUserss.Remove(interest);
        //    return unitOfWork.SaveChanges() != 0;

        //}

        public bool ChangeInterests(int userId,List<int> interestId)
        {
            unitOfWork.InterestsUserss.RemoveRange(unitOfWork.InterestsUserss.Query.Where(e => e.UserId == userId));
            unitOfWork.InterestsUserss.AddRange(interestId.Select(e => new Domain.InterestsUsers()
            {
                UserId = userId,
                InterestId = e
            }));
            return unitOfWork.SaveChanges()!=0;
        }

    }
}
