using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace DataAccess.Base
{
    class BaseUnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly SocializRContext DbContext;
        public BaseUnitOfWork(SocializRContext context)
        {
            DbContext = context;
        }
        void IDisposable.Dispose()
        {
            DbContext.Dispose();
        }

        int IUnitOfWork.SaveChanges()
        {
            return DbContext.SaveChanges();
        }
    }
}
