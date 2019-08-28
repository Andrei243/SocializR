using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;

namespace Services.Reaction
{
    public class ReactionService : Base.BaseService
    {
        private readonly CurrentUser CurrentUser;
        //private readonly Domain.Post Post;

        public ReactionService(CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            CurrentUser = currentUser;
        }

        //public bool AddReaction(int postId)
        //{
        //    //if (unitOfWork.Reactions.Query.Any(e => e.PostId == postId && e.UserId == CurrentUser.Id)) return false;
        //    var reaction = new Domain.Reaction()
        //    {
        //        PostId = postId,
        //        UserId = CurrentUser.Id
        //    };
        //    unitOfWork.Reactions.Add(reaction);
        //    unitOfWork.SaveChanges();
        //    return true;

        //}

        //public bool RemoveReaction(int postId)
        //{
        //    unitOfWork.Reactions.RemoveRange(unitOfWork.Reactions.Query.Where(e => e.PostId == postId && e.UserId == CurrentUser.Id));
        //    unitOfWork.SaveChanges();
        //    return false;
        //}

        public bool IsLiked(int postId)
        {
            return unitOfWork.Reactions.Query.Any(e => e.PostId == postId && e.UserId == CurrentUser.Id);
        }

        public bool ChangeReaction(int postId)
        {
            var reaction = unitOfWork.Reactions.Query.FirstOrDefault(e => e.PostId == postId && e.UserId == CurrentUser.Id);
            if (reaction == null)
            {
                var reaction2 = new Domain.Reaction()
                {
                    PostId = postId,
                    UserId = CurrentUser.Id
                };
                unitOfWork.Reactions.Add(reaction2);
                unitOfWork.SaveChanges();
                return true;
            }
            else
            {
                unitOfWork.Reactions.Remove(reaction);
                unitOfWork.SaveChanges();
                return false;
            }

            
        }

       
    }
}
