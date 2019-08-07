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
        private readonly Domain.Post Post;

        public ReactionService(CurrentUser currentUser,Domain.Post post,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            CurrentUser = currentUser;
            Post = post;
        }

        public bool AddReaction()
        {
            if (unitOfWork.Reactions.Query.Any(e => e.PostId == Post.Id && e.UserId == CurrentUser.Id)) return false;
            var reaction = new Domain.Reaction()
            {
                PostId = Post.Id,
                UserId = CurrentUser.Id
            };
            unitOfWork.Reactions.Add(reaction);
            return unitOfWork.SaveChanges() != 0;

        }

        public bool RemoveReaction()
        {
            unitOfWork.Reactions.RemoveRange(unitOfWork.Reactions.Query.Where(e => e.PostId == Post.Id && e.UserId == CurrentUser.Id));
            return unitOfWork.SaveChanges() != 0;
        }

    }
}
