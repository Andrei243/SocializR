using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.Photo
{
    public class PhotoService : Base.BaseService
    {


        public PhotoService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void AddPhoto(Domain.Photo photo)
        {
            unitOfWork.Photos.Add(photo);
            if (photo.PostId.HasValue)
            {
                var post = unitOfWork.Posts.Query.FirstOrDefault(e => e.Id == photo.PostId);
                post.Photo = photo;
                unitOfWork.Posts.Update(post);
            }
            else
            {
                photo.Position = unitOfWork.Photos.Query.Where(e => e.AlbumId == photo.AlbumId).Select(e => e.Position).OrderBy(e => e).LastOrDefault() ?? 0;
                photo.Position++;
            }
            unitOfWork.SaveChanges();
        }

        public List<Domain.Photo> getPhotos(int? postId,int? albumId)
        {
            IQueryable<Domain.Photo> listaPoze ;
            if(postId!= null)
            {
                listaPoze = unitOfWork.Photos.Query.AsNoTracking().Where(e => e.PostId == postId);
            }
            else
            {
                listaPoze = unitOfWork.Photos.Query.AsNoTracking().Where(e => e.AlbumId == albumId);
            }
            return listaPoze.OrderBy(e => e.Position).ToList();
        }

        public bool RemovePhoto(int photoId,int? postId,int? albumId)
        {
            postId = unitOfWork.Photos.Query.AsNoTracking().Where(e => e.Id == photoId).Select(e => e.PostId).FirstOrDefault();
            albumId= unitOfWork.Photos.Query.AsNoTracking().Where(e => e.Id == photoId).Select(e => e.AlbumId).FirstOrDefault();
            IQueryable<Domain.Photo> listaPoze;
            if (postId != null)
            {
                listaPoze = unitOfWork.Photos.Query.Where(e => e.PostId == postId);
            }
            else
            {
                listaPoze = unitOfWork.Photos.Query.Where(e => e.AlbumId == albumId);
            }
            var poze = listaPoze.OrderBy(e => e.Position).ToList();

            foreach(var poza in poze)
            {
                if (poza.Id == photoId) unitOfWork.Photos.Remove(poza);
                else if(poza.Position > photoId)
                {
                    poza.Position -= 1;
                    unitOfWork.Photos.Update(poza);
                }
            }
            return unitOfWork.SaveChanges() != 0;
        }

    }
}
