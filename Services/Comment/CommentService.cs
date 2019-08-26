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

        public List<Domain.Comment> GetComments(int currentPage,int postId)
        {
            return unitOfWork
                .Comments
                .Query
                .AsNoTracking()
                .Where(e => e.PostId == postId)
                .OrderBy(e => e.AddingMoment)
                .Skip(currentPage * Base.GlobalConstants.PAGESIZE)
                .Take(Base.GlobalConstants.PAGESIZE)
                .Include(e => e.User)
                .ToList();
        }
        public List<Domain.Comment> GetComments( int postId)
        {
            return unitOfWork
                .Comments
                .Query
                .AsNoTracking()
                .Where(e => e.PostId == postId)
                .OrderBy(e => e.AddingMoment)
                .Include(e => e.User)
                .ToList();
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

        public bool RemoveComment(int commentId)
        {
            unitOfWork.Comments.Remove(unitOfWork.Comments.Query.FirstOrDefault(e => e.Id == commentId));
            return unitOfWork.SaveChanges() != 0;
        }

        public bool CanDeleteComment(int commentId)
        {
            if (CurrentUser.IsAdmin) return true;
            var bool1 = unitOfWork.Comments.Query.First(e => e.Id == commentId).UserId == CurrentUser.Id;
            if (bool1) return true;
            var postId = unitOfWork.Comments.Query.First(e => e.Id == commentId).PostId;
            return unitOfWork.Posts.Query.First(e => e.Id == postId).UserId == CurrentUser.Id;
        }

        public List<Domain.Comment> GetComments(int already,int howMany,int postId)
        {
            return unitOfWork.Comments.Query.OrderByDescending(e => e.AddingMoment)
                .Where(e=>e.PostId==postId)
                .Skip(already)
                .Take(howMany)
                .Include(e => e.User)
                .AsNoTracking()
                .ToList();
        }
    }
}
