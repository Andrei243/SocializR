using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.County
{
    public class CountyService : Base.BaseService
    {
        public CountyService(SocializRUnitOfWork unitOfWork) : base(unitOfWork) { }

        public List<Domain.County> GetAll()
        {
            return unitOfWork.Counties.Query.AsNoTracking().Include(e=>e.Locality).AsNoTracking().ToList();
        }

        public bool CanBeDeleted(int countyId)
        {
            return !unitOfWork.Localities.Query.Any(e => e.CountyId == countyId);
        }
        public Domain.County GetCountyById(int? id)
        {
            return unitOfWork.Counties.Query.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        public void Add(string name)
        {
            Domain.County county = new Domain.County()
            {
                Name = name
            };
            unitOfWork.Counties.Add(county);
            unitOfWork.SaveChanges();
        }

        public void Update(int id, string name)
        {
            var county = unitOfWork.Counties.Query.FirstOrDefault(e => e.Id == id);
            if (county == null) return;
            county.Name = name;
            unitOfWork.Counties.Update(county);
            unitOfWork.SaveChanges();
        }

        public void Remove(int countyId)
        {
            unitOfWork.Counties.Remove(unitOfWork.Counties.Query.FirstOrDefault(e=>e.Id==countyId));
            var localitiesIds = unitOfWork.Localities.Query.Where(e => e.CountyId == countyId).Select(e => e.Id).ToList();

            unitOfWork.Localities.RemoveRange(unitOfWork.Localities.Query.Where(e => e.CountyId == countyId));
            var users = unitOfWork.Users.Query.Where(e => localitiesIds.Contains(e.Id));
            foreach(var user in users)
            {
                user.LocalityId = null;
                unitOfWork.Users.Update(user);
            }
            unitOfWork.SaveChanges();
        }

        public List<Domain.Locality> GetLocalities(int id)
        {
            return unitOfWork.Localities.Query.Where(e => e.CountyId == id).AsNoTracking().ToList();
        }

        public List<Domain.County> GetCounties(int toSkip,int howMany)
        {
            return unitOfWork.Counties.Query
                .OrderBy(e => e.Name)
                .Skip(toSkip)
                .Take(howMany)
                .AsNoTracking()
                .ToList();

        }
    }
}
