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

        public List<Domain.Post> getAllPersonalPost()
        {
            return unitOfWork.Posts.Query.Where(e => e.UserId == CurrentUser.Id).OrderBy(e=>e.AddingMoment).ToList();
        }

        public List<Domain.Post> getNewsfeed()
        {
            var friends = unitOfWork.Friendships.Query.Where(e => e.IdReceiver == CurrentUser.Id && (e.Accepted??false)).Select(e=>e.IdSender).ToList();

            var posts1 = unitOfWork.Posts.Query.Include(e => e.User).Where(e => friends.Contains(e.Id)).ToList();
            var posts2 = getAllPersonalPost();
            var posts3 = unitOfWork.Posts.Query.Where(e => e.Vizibilitate == "public");
            posts1.AddRange(posts2);
            posts1.AddRange(posts3);
            return posts1.Distinct().OrderBy(e => e.AddingMoment).ToList();

        }

    }
}
