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

        public Domain.County GetCountyById(int? id)
        {
            return unitOfWork.Counties.Query.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        public void Add(Domain.County county)
        {
            unitOfWork.Counties.Add(county);
            unitOfWork.SaveChanges();
        }

        public void Update(Domain.County county)
        {
            unitOfWork.Counties.Update(county);
            unitOfWork.SaveChanges();
        }

        public void Remove(Domain.County county)
        {
            unitOfWork.Counties.Remove(county);
            unitOfWork.Localities.RemoveRange(unitOfWork.Localities.Query.Where(e => e.CountyId == county.Id));
            unitOfWork.SaveChanges();
        }

        public List<Domain.Locality> GetLocalities(int id)
        {
            return unitOfWork.Localities.Query.Where(e => e.CountyId == id).AsNoTracking().ToList();
        }
    }
}
