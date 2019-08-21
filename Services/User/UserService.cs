using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.User
{
    public class UserService : Base.BaseService
    {
        private readonly CurrentUser currentUser;
        public UserService(CurrentUser currentUser, SocializRUnitOfWork unitOfWork):
            base(unitOfWork)
        {
            this.currentUser = currentUser;
        }

       

        public Users getCurrentUser()
        {
            return getUserById(currentUser.Id);
        }
        public Users getUserById(int? id)
        {
            return unitOfWork
                .Users
                .Query
                .AsNoTracking()
                .Include(e=>e.Locality).ThenInclude(e=>e.County)
                .AsNoTracking()
                .Include(e=>e.Role)
                .AsNoTracking()
                .Include(e=>e.InterestsUsers)
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
        }
        public IEnumerable<Users> getAll()
        {
            return unitOfWork.Users.Query.AsNoTracking().Include(e => e.Locality).AsNoTracking().Include(e => e.Role).AsNoTracking();
        }

        public IEnumerable<Users> GetUsersByName(string name)
        {
            return unitOfWork
                .Users
                .Query
                .AsNoTracking()
                .Where(e => (e.Name + e.Surname).Contains(name));
        }
       

        public void Update(Users user)
        {
            Users oldUser = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == user.Id);
            oldUser.LocalityId = user.LocalityId ?? oldUser.LocalityId;
            oldUser.Vizibility = user.Vizibility ?? oldUser.Vizibility;
            oldUser.BirthDay = user.BirthDay;
            oldUser.Name = user.Name;
            oldUser.Surname = user.Surname;
            oldUser.SexualIdentity = user.SexualIdentity;

            unitOfWork.Users.Update(oldUser);
            unitOfWork.SaveChanges();
        }

        public bool HasThisPhoto(int photoId)
        {
            var poza = unitOfWork.Photos.Query.Include(e => e.Album).Include(e => e.Post).FirstOrDefault(e => e.Id == photoId);

            if (poza.Album != null)
            {
                return poza.Album.UserId == currentUser.Id;
            }
            else
            {
                return poza.Post.UserId == currentUser.Id;
            }
        }

        public void UpdateProfilePhoto(int photoId)
        {
            var user = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id);
            user.PhotoId = photoId;
            unitOfWork.Users.Update(user);
            unitOfWork.SaveChanges();
        }

        public void RemoveUser(int userId)
        {
            var albums = unitOfWork.Albums.Query.Where(e => e.UserId == userId);
            var posts = unitOfWork.Posts.Query.Where(e => e.UserId == userId);
            var comments = unitOfWork.Comments.Query.Where(e => e.UserId == userId || posts.Select(f => f.Id).Contains(e.PostId));
            var reactions = unitOfWork.Reactions.Query.Where(e => e.UserId == userId || posts.Select(f => f.Id).Contains(e.PostId));
            var friendships = unitOfWork.Friendships.Query.Where(e => e.IdReceiver == userId || e.IdSender == userId);
            var interestsUsers = unitOfWork.InterestsUserss.Query.Where(e => e.UserId == userId);
            var photos = unitOfWork.Photos.Query.Where(e => posts.Select(f => f.Id).Contains(e.PostId.Value) || albums.Select(f => f.Id).Contains(e.AlbumId.Value));
            unitOfWork.Photos.RemoveRange(photos);
            unitOfWork.InterestsUserss.RemoveRange(interestsUsers);
            unitOfWork.Friendships.RemoveRange(friendships);
            unitOfWork.Reactions.RemoveRange(reactions);
            unitOfWork.Comments.RemoveRange(comments);
            unitOfWork.Posts.RemoveRange(posts);
            unitOfWork.Albums.RemoveRange(albums);
            unitOfWork.Users.Remove(unitOfWork.Users.Query.First(e => e.Id == userId));
            unitOfWork.SaveChanges();
        }
    }
}
