using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.Comment
{
    public class CommentService : Base.BaseService
    {
        private readonly Domain.Post Post;
        private readonly CurrentUser CurrentUser;
        public CommentService(Domain.Post post,CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            Post = post;
            CurrentUser = currentUser;
        }

        public List<Domain.Comment> GetAll()
        {
            return unitOfWork.Comments.Query.Include(e=>e.User).Where(e => e.PostId == Post.Id).OrderBy(e => e.AddingMoment).ToList();
        }

        public bool AddComment(string text)
        {
            var comment = new Domain.Comment()
            {
                Content = text,
                PostId = Post.Id,
                UserId = CurrentUser.Id,
                AddingMoment=DateTime.Now
            };
            unitOfWork.Comments.Add(comment);
            return unitOfWork.SaveChanges() != 0;
        }

    }
}
