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
        public readonly Users Users;

        public AlbumService(Users user,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Users = user;
        }

        public List<Domain.Album> getAll()
        {
            return unitOfWork.Albums.Query.Where(e => e.UserId == Users.Id).OrderBy(e => e.Id).ToList();
        }

        

    }
}
