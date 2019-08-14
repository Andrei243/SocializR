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
        private readonly CurrentUser CurrentUser;
        public CommentService(CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            CurrentUser = currentUser;
        }

        public List<Domain.Comment> GetAll(int PostId)
        {
            return unitOfWork.Comments.Query.AsNoTracking().Include(e=>e.User).AsNoTracking().Where(e => e.PostId == PostId).OrderBy(e => e.AddingMoment).ToList();
        }

        public bool AddComment(string text,int PostId)
        {
            var comment = new Domain.Comment()
            {
                Content = text,
                PostId = PostId,
                UserId = CurrentUser.Id,
                AddingMoment=DateTime.Now
            };
            unitOfWork.Comments.Add(comment);
            return unitOfWork.SaveChanges() != 0;
        }

    }
}
