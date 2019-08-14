using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.Interest
{
    public class InterestService : Base.BaseService
    {
        public InterestService(SocializRUnitOfWork unitOfWork) : base(unitOfWork) { }

        public bool AddInterest(string denumire)
        {
            var interest = new Domain.Interest()
            {
                Name = denumire
            };
            unitOfWork.Interests.Add(interest);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool RemoveInterest(Domain.Interest interest)
        {
            unitOfWork.InterestsUserss.RemoveRange(unitOfWork.InterestsUserss.Query.Where(e => e.InterestId == interest.Id).ToList());
            unitOfWork.Interests.Remove(interest);
            return unitOfWork.SaveChanges() != 0;
        }

        public List<Domain.Interest> getAll()
        {
            return unitOfWork.Interests.Query.AsNoTracking().ToList();
        }
    }
}
