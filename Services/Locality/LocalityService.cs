using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;

namespace Services.Locality
{
    public class LocalityService : Base.BaseService
    {

        public LocalityService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool AddLocality(string denumire,int countyId)
        {
            if (unitOfWork.Localities.Query.Any(e =>e.Name==denumire && e.CountyId==countyId)) return false;
            var locality = new Domain.Locality()
            {
                CountyId = countyId,
                Name = denumire
            };
            unitOfWork.Localities.Add(locality);
            return unitOfWork.SaveChanges() != 0;

        }

        public bool RemoveLocality(Domain.Locality locality)
        {
            if (!unitOfWork.Localities.Query.Any(e => e.Id == locality.Id)) return false;
            unitOfWork.Localities.Remove(locality);
            return unitOfWork.SaveChanges() != 0;

        }

        public List<Domain.Locality> getAll(int countyId)
        {
            return unitOfWork.Localities.Query.Where(e=>e.CountyId==countyId).ToList();
        }

    }
}
