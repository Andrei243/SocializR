using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;

namespace Services.Locality
{
    public class LocalityService : Base.BaseService
    {
        private readonly Domain.County County;

        public LocalityService(SocializRUnitOfWork unitOfWork, Domain.County county) : base(unitOfWork)
        {
            County = county;
        }

        public bool AddLocality(string denumire,Domain.County County)
        {
            if (unitOfWork.Localities.Query.Any(e =>e.Name==denumire && e.CountyId==County.Id)) return false;
            var locality = new Domain.Locality()
            {
                CountyId = County.Id,
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

        public List<Domain.Locality> getAll()
        {
            return unitOfWork.Localities.Query.Where(e=>e.CountyId==County.Id).ToList();
        }

    }
}
