using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Linq;

namespace Services.County
{
    public class CountyService : Base.BaseService
    {
        public CountyService(SocializRUnitOfWork unitOfWork) : base(unitOfWork) { }

        public List<Domain.County> GetAll()
        {
            return unitOfWork.Counties.Query.ToList();
        }

        public List<Domain.Locality> GetLocalities(int id)
        {
            return unitOfWork.Localities.Query.Where(e => e.CountyId == id).ToList();
        }
    }
}
