using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Domain;
using System.Linq;

namespace Services.Album
{
    public class AlbumService : Base.BaseService
    {
        public readonly CurrentUser CurrentUser;

        public AlbumService(CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.CurrentUser = currentUser;
        }

        public List<Domain.Album> getAll()
        {
            return unitOfWork.Albums.Query.Where(e => e.UserId == CurrentUser.Id).OrderBy(e => e.Id).ToList();
        }

        

    }
}
