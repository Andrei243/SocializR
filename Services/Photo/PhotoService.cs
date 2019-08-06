using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;

namespace Services.Photo
{
    public class PhotoService : Base.BaseService
    {
        private readonly Domain.Post Post;
        private readonly Domain.Album Album;

        public PhotoService(Domain.Post post,Domain.Album album,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            Post = post;
            Album = album;
        }

        public List<Domain.Photo> getPhotos()
        {
            IQueryable<Domain.Photo> listaPoze ;
            if(Post!= null)
            {
                listaPoze = unitOfWork.Photos.Query.Where(e => e.PostId == Post.Id);
            }
            else
            {
                listaPoze = unitOfWork.Photos.Query.Where(e => e.AlbumId == Album.Id);
            }
            return listaPoze.OrderBy(e => e.Position).ToList();
        }

        public bool RemovePhoto(Domain.Photo photo)
        {
            IQueryable<Domain.Photo> listaPoze;
            if (Post != null)
            {
                listaPoze = unitOfWork.Photos.Query.Where(e => e.PostId == photo.PostId);
            }
            else
            {
                listaPoze = unitOfWork.Photos.Query.Where(e => e.AlbumId == photo.AlbumId);
            }
            var poze = listaPoze.OrderBy(e => e.Position).ToList();

            foreach(var poza in poze)
            {
                if (poza.Id == photo.Id) unitOfWork.Photos.Remove(poza);
                else if(poza.Position > photo.Id)
                {
                    poza.Position -= 1;
                    unitOfWork.Photos.Update(poza);
                }
            }
            return unitOfWork.SaveChanges() != 0;
        }

    }
}
