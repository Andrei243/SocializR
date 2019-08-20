using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.Album
{
    public class AlbumService : Base.BaseService
    {
        public readonly CurrentUser CurrentUser;

        public AlbumService(CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.CurrentUser = currentUser;
        }

        public List<Domain.Album> GetAll(int idUser)
        {
            return unitOfWork.Albums.Query.Where(e => e.UserId == idUser).AsNoTracking().Include(e=>e.Photo).AsNoTracking().OrderBy(e => e.Id).ToList();
        }

        public void AddAlbum(string denumire)
        {
            Domain.Album album = new Domain.Album { Name = denumire, UserId = CurrentUser.Id };
            unitOfWork.Albums.Add(album);
            unitOfWork.SaveChanges();
        }

        public void RemoveAlbum(int albumId)
        {
            unitOfWork.Photos.RemoveRange(unitOfWork.Photos.Query.Where(e => e.AlbumId == albumId&&CurrentUser.ProfilePhoto!=e.Id));
            unitOfWork.Albums.Remove(unitOfWork.Albums.Query.FirstOrDefault(e => e.Id == albumId));
            unitOfWork.SaveChanges();

        }

    }
}
