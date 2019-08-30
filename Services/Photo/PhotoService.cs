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
        private readonly CurrentUser currentUser;

        public PhotoService(SocializRUnitOfWork unitOfWork,CurrentUser currentUser) : base(unitOfWork)
        {
            this.currentUser = currentUser;
        }

        public bool CanSeePhoto(int photoId)
        {
            if (currentUser.IsAdmin)
            {
                return true;
            }

            var photo = unitOfWork.Photos.Query
                .Include(e=>e.Post)
                .Include(e=>e.Album)
                .First(e => e.Id == photoId);

            Domain.Users user;
            if (photo.Post != null)
            {
                if (photo.Post.UserId == currentUser.Id) return true;
                if (photo.Post.Vizibilitate == "public") return true;
                user = unitOfWork.Posts.Query
                    .Where(e => e.Id == photo.PostId)
                    .Select(e => e.User)
                    .First(e => true);

            }
            else if (photo.Album != null)
            {
                user = unitOfWork.Albums.Query
                    .Where(e => e.Id == photo.AlbumId)
                    .Select(e => e.User)
                    .First(e => true);
            }
            else
            {
                return false;
            }
            if (user.Vizibility == "public") return true;
            else if (unitOfWork.Friendships.Query.Any(e => e.IdReceiver == currentUser.Id && e.IdSender == user.Id && (e.Accepted ?? false)))
            {
                return true;
            }
            return false;

        }
        public void ChangeDescription(int photoId,string description)
        {
            var image = unitOfWork.Photos.Query
                .FirstOrDefault(e => e.Id == photoId);
            image.Description = description;
            unitOfWork.Photos.Update(image);
            unitOfWork.SaveChanges();

        }

        public bool HasThisPhoto(int photoId,int userId)
        {
            var photo = unitOfWork.Photos.Query
                .FirstOrDefault(e => e.Id == photoId);
            var albums = unitOfWork.Albums.Query
                .Where(e => e.UserId == userId);
            return albums.Select(e => e.Id).Contains(photo.AlbumId.Value);

        }
        public void AddPhoto(Domain.Photo photo)
        {
            unitOfWork.Photos.Add(photo);
            if (photo.PostId.HasValue)
            {
                var post = unitOfWork.Posts.Query
                    .Include(e=>e.Photo)
                    .FirstOrDefault(e => e.Id == photo.PostId);
                
                post.Photo = photo;
                unitOfWork.Posts.Update(post);
            }
            else
            {
                photo.Position = unitOfWork.Photos.Query
                    .Where(e => e.AlbumId == photo.AlbumId)
                    .Count() + 1;
            }
            unitOfWork.SaveChanges();
        }

        public bool RemovePhoto(int photoId)
        {
            var photo = unitOfWork.Photos.Query
                .AsNoTracking()
                .First(e => e.Id == photoId);
            if (photo == null) return false;
            int? postId = photo.PostId;
            int? albumId= photo.AlbumId;
            IQueryable<Domain.Photo> listaPoze;
            if (postId != null)
            {
                listaPoze = unitOfWork.Photos.Query
                    .Where(e => e.PostId == postId);
            }
            else
            {
                listaPoze = unitOfWork.Photos.Query
                    .Where(e => e.AlbumId == albumId);
            }
            var poze = listaPoze
                .OrderBy(e => e.Position)
                .ToList();

           

            if(albumId!= null)
            {
                var user = unitOfWork.Albums.Query.Where(e => e.Id == albumId).Select(e => e.User).FirstOrDefault();
                if (user.PhotoId == photo.Id)
                {
                    user.PhotoId = null;
                    unitOfWork.Users.Update(user);
                }
            }

            foreach (var poza in poze)
            {
                if (poza.Id == photoId)
                {
                    unitOfWork.Photos
                        .Remove(poza);
                }
                else if (poza.Position > photo.Position)
                {
                    poza.Position -= 1;
                    unitOfWork.Photos.Update(poza);
                }
            }

            return unitOfWork.SaveChanges() != 0;
        }

        public Domain.Photo GetPhoto(int id)
        {
            return unitOfWork.Photos.Query
                .FirstOrDefault(e => e.Id == id);
        }

        public List<Domain.Photo> GetPhotos(int toSkip,int howMany,int albumId)
        {
            return unitOfWork.Photos.Query
                .Where(e => e.AlbumId == albumId)
                .OrderBy(e => e.Position)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
        }

    }
}
