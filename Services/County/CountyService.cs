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
    }
}
