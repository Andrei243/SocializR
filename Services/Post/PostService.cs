using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Services.Post
{
    internal class PostComparer : IEqualityComparer<Domain.Post>
    {
        bool IEqualityComparer<Domain.Post>.Equals(Domain.Post x, Domain.Post y)
        {
            return x.Id == y.Id;
        }

        int IEqualityComparer<Domain.Post>.GetHashCode(Domain.Post obj)
        {
            return obj.Id;
        }
    }
    public class PostService : Base.BaseService
    {
        private readonly CurrentUser CurrentUser;
        

        public PostService(CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            CurrentUser = currentUser;
        }

        
        public bool AddPost(Domain.Post post)
        {
            post.AddingMoment = DateTime.Now;
            post.UserId = CurrentUser.Id;
            post.Vizibilitate = post.Vizibilitate ?? "friends";
            unitOfWork.Posts.Add(post);
            return unitOfWork.SaveChanges()!=0;

        }

        

        public List<Domain.Post> GetAllPersonalPost(int currentPage)
        {
            return GetFeed()
                .Where(e => e.UserId == CurrentUser.Id)
                            .OrderByDescending(e => e.AddingMoment)
                            .Skip(currentPage * Base.GlobalConstants.PAGESIZE) // from parameter
                            .Take(Base.GlobalConstants.PAGESIZE)
                            .ToList();
        }

        public bool CanDetelePost(int postId)
        {
            return unitOfWork.Posts.Query.First(e => e.Id == postId).UserId == CurrentUser.Id;
        }

        private IEnumerable<Domain.Post> GetFeed()
        {
            return unitOfWork
                            .Posts
                            .Query
                            .AsNoTracking()
                            .Include(e => e.User)
                            .Include(e => e.Comment)
                            .ThenInclude(e => e.User)
                            .AsNoTracking()
                            .Include(e => e.Photo)
                            .AsNoTracking()
                            .Include(e => e.Reaction)
                            .AsNoTracking()
                            ;
        }

        public List<Domain.Post> GetPublicNewsfeed(int currentPage)
        {
            return GetFeed()
                .Where(e=>e.Vizibilitate == "public")
                .OrderByDescending(e => e.AddingMoment)
                .Skip(currentPage*Base.GlobalConstants.PAGESIZE)
                .Take(Base.GlobalConstants.PAGESIZE)
                .ToList();
        }

        public List<Domain.Post> GetNewsfeed(int currentPage)
        {
            var friends = unitOfWork.Friendships.Query.AsNoTracking().Where(e => e.IdReceiver == CurrentUser.Id && (e.Accepted??false)).Select(e=>e.IdSender).ToList();

            
            var posts = GetFeed()
                .Where(e =>
                (e.Vizibilitate == "public") ||
                (e.UserId == CurrentUser.Id) ||
                (friends.Contains(e.UserId) && e.Vizibilitate == "friends")
                )
                .OrderByDescending(e => e.AddingMoment)
                .Skip(currentPage * Base.GlobalConstants.PAGESIZE)
                .Take(Base.GlobalConstants.PAGESIZE)
                .ToList();
            return posts;
        }

        public bool RemovePost(int postId)
        {
            var reactions = unitOfWork.Reactions.Query.Where(e => e.PostId == postId);
            var comments = unitOfWork.Comments.Query.Where(e => e.PostId == postId);
            var photos = unitOfWork.Photos.Query.Where(e => e.PostId == postId);
            unitOfWork.Reactions.RemoveRange(reactions);
            unitOfWork.Comments.RemoveRange(comments);
            unitOfWork.Photos.RemoveRange(photos);
            unitOfWork.Posts.Remove(unitOfWork.Posts.Query.FirstOrDefault(e => e.Id == postId));
            return unitOfWork.SaveChanges() != 0;
        }

        public bool CanSeePost(int postId)
        {
            if (CurrentUser.IsAdmin) return true;
            var post = unitOfWork.Posts.Query.First(e => e.Id == postId);
            if (post.UserId == CurrentUser.Id || post.Vizibilitate == "public") return true;
            var suntPrieteni = unitOfWork.Friendships.Query.Any(e => e.IdReceiver == post.UserId && e.IdSender == CurrentUser.Id && (e.Accepted ?? false));
            if (suntPrieteni) return post.Vizibilitate == "friends";
            else return false;
        }
    }
}
