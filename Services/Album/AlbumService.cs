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

        public void RemoveAlbum(int albumId,int userId)
        {
            var profilePhoto = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == userId).PhotoId;
            var album = unitOfWork.Albums.Query.FirstOrDefault(e => e.Id == albumId);
            if (profilePhoto!= null)
            {
                var photo = unitOfWork.Photos.Query.FirstOrDefault(e => e.Id == profilePhoto);
                if (photo.AlbumId == albumId)
                {
                    var user = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == album.UserId);
                    user.PhotoId = null;
                    unitOfWork.Users.Update(user);
                }
            }
            unitOfWork.Photos.RemoveRange(unitOfWork.Photos.Query.Where(e => e.AlbumId == albumId));
            unitOfWork.Albums.Remove(album);
            unitOfWork.SaveChanges();

        }

        public bool HasThisAlbum(int albumId)
        {
            return unitOfWork.Albums.Query.FirstOrDefault(e => e.Id == albumId && e.UserId == CurrentUser.Id) != null;
        }

    }
}
