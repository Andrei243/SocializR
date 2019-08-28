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

        public AlbumService(CurrentUser currentUser, SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.CurrentUser = currentUser;
        }

        public bool CanSeeAlbum(int albumId)
        {
            if (CurrentUser.IsAdmin) return true;
            var album = unitOfWork.Albums.Query.AsNoTracking().First(e => e.Id == albumId);
            if (album.UserId == CurrentUser.Id) return true;
            return unitOfWork.Friendships.Query.Any(e => e.IdReceiver == CurrentUser.Id && e.IdSender == album.UserId);
        }

        public List<Domain.Album> GetAll(int idUser)
        {
            return unitOfWork.Albums.Query.Where(e => e.UserId == idUser).AsNoTracking().Include(e => e.Photo).AsNoTracking().OrderBy(e => e.Id).ToList();
        }

        public List<Domain.Photo> GetPhotos(int albumId)
        {
            return unitOfWork.Photos.Query.Where(e => e.AlbumId == albumId).ToList();
        }

        public int AddAlbum(string denumire)
        {
            Domain.Album album = new Domain.Album { Name = denumire, UserId = CurrentUser.Id };
            unitOfWork.Albums.Add(album);
            unitOfWork.SaveChanges();
            return album.Id;
        }
        public bool CanDeleteAlbum(int albumId)
        {
            if (CurrentUser.IsAdmin) return true;
            return unitOfWork.Albums.Query.Any(e => e.Id == albumId && e.UserId == CurrentUser.Id);
        }

        public bool RemoveAlbum(int albumId, int userId)
        {
            var album = unitOfWork.Albums.Query.FirstOrDefault(e => e.Id == albumId);
            if (album == null) return false; ;

            var profilePhoto = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == userId).PhotoId;
            if (profilePhoto != null)
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
            return unitOfWork.SaveChanges()!=0;

        }

        public bool HasThisAlbum(int albumId)
        {
            return unitOfWork.Albums.Query.Any(e => e.Id == albumId && e.UserId == CurrentUser.Id);
        }

        public Domain.Album GetAlbum(int albumId)
        {
            return unitOfWork.Albums.Query.Include(e => e.Photo).FirstOrDefault(e => e.Id == albumId);
        }
    }
}
