using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Linq;

namespace Services.FriendShip
{
    public class FriendRequest : Base.BaseService
    {
        private readonly CurrentUser currentUser;

        public FriendRequest(CurrentUser currentUser, SocializRUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            this.currentUser = currentUser;
        }

        public bool isFriendWith(Users users)
        {
            return unitOfWork.Friendships.Query.Any(e => e.IdSender == currentUser.Id && e.IdReceiver == users.Id && (e.Accepted??false));
        }
        public bool SendFriendRequest(Users to)
        {
            
            if(unitOfWork.Friendships.Query.Any(e=>e.IdReceiver == currentUser.Id && e.IdSender == to.Id))
            {
                return false;
            }
            var friendRequest = new Domain.Friendship()
            {
                IdSender = currentUser.Id,
                IdReceiver = to.Id,
                Accepted = null,
                CreatedOn = DateTime.Now
            };
            unitOfWork.Friendships.Add(friendRequest);
            return unitOfWork.SaveChanges() != 0;

        }

       public bool RefuseFriendRequest(Users from)
        {
            var friendRequest = unitOfWork.Friendships.Query.Where(e => e.IdSender == from.Id && e.IdReceiver == currentUser.Id).ToList()[0];
            friendRequest.Accepted = false;
            unitOfWork.Friendships.Update(friendRequest);
            return unitOfWork.SaveChanges() != 0;

        }
        public bool AcceptFriendRequest(Users from)
        {
            var friendRequest = unitOfWork.Friendships.Query.Where(e => e.IdSender == from.Id && e.IdReceiver == currentUser.Id).ToList()[0];
            friendRequest.Accepted = true;
            var friendRequest2 = new Domain.Friendship()
            {
                IdReceiver = friendRequest.IdSender,
                IdSender = friendRequest.IdReceiver,
                CreatedOn = friendRequest.CreatedOn,
                Accepted = true
            };
            unitOfWork.Friendships.Update(friendRequest);
            unitOfWork.Friendships.Add(friendRequest2);
            return unitOfWork.SaveChanges() != 0;
        }

    }
}
