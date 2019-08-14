using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;

namespace Services.Photo
{
    public class PhotoService : Base.BaseService
    {


        public PhotoService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<Domain.Photo> getPhotos(int? postId,int? albumId)
        {
            IQueryable<Domain.Photo> listaPoze ;
            if(postId!= null)
            {
                listaPoze = unitOfWork.Photos.Query.Where(e => e.PostId == postId);
            }
            else
            {
                listaPoze = unitOfWork.Photos.Query.Where(e => e.AlbumId == albumId);
            }
            return listaPoze.OrderBy(e => e.Position).ToList();
        }

        public bool RemovePhoto(int photoId,int? postId,int? albumId)
        {
            postId = unitOfWork.Photos.Query.Where(e => e.Id == photoId).Select(e => e.PostId).FirstOrDefault();
            albumId= unitOfWork.Photos.Query.Where(e => e.Id == photoId).Select(e => e.AlbumId).FirstOrDefault();
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
