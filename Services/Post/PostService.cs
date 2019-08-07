using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Services.Post
{
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

        public List<Domain.Post> GetAllPersonalPost()
        {
            return unitOfWork.Posts.Query.Where(e => e.UserId == CurrentUser.Id).OrderBy(e=>e.AddingMoment).ToList();
        }

        public List<Domain.Post> GetPublicNewsfeed()
        {
            return unitOfWork.Posts.Query.Where(e => e.Vizibilitate == "public").ToList();
        }

        public List<Domain.Post> GetNewsfeed()
        {
            var friends = unitOfWork.Friendships.Query.Where(e => e.IdReceiver == CurrentUser.Id && (e.Accepted??false)).Select(e=>e.IdSender).ToList();

            var posts1 = unitOfWork.Posts.Query.Include(e => e.User).Where(e => friends.Contains(e.Id) && e.Vizibilitate=="friends").ToList();
            var posts2 = GetAllPersonalPost();
            var posts3 = unitOfWork.Posts.Query.Where(e => e.Vizibilitate == "public");
            posts1.AddRange(posts2);
            posts1.AddRange(posts3);
            return posts1.Distinct().OrderBy(e => e.AddingMoment).ToList();

        }

    }
}
