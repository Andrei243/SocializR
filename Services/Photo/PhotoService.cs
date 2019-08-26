﻿using System;
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

        public void ChangeDescription(int photoId,string description)
        {
            var image = unitOfWork.Photos.Query.FirstOrDefault(e => e.Id == photoId);
            image.Description = description;
            unitOfWork.Photos.Update(image);
            unitOfWork.SaveChanges();

        }

        public bool HasThisPhoto(int photoId,int userId)
        {
            var photo = unitOfWork.Photos.Query.FirstOrDefault(e => e.Id == photoId);
            var albums = unitOfWork.Albums.Query.Where(e => e.UserId == userId);
            return albums.Select(e => e.Id).Contains(photo.AlbumId.Value);

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

        public Domain.Photo GetPhoto(int id)
        {
            return unitOfWork.Photos.Query.FirstOrDefault(e => e.Id == id);
        }

        public List<Domain.Photo> GetPhotos(int already,int howMany,int albumId)
        {
            return unitOfWork.Photos.Query.Where(e => e.AlbumId == albumId).OrderBy(e => e.Position).Skip(already).Take(howMany).ToList();
        }

    }
}
