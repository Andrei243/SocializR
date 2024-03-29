﻿using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.FriendShip
{
    public class FriendshipService : Base.BaseService
    {
        private readonly CurrentUser currentUser;

        public FriendshipService(CurrentUser currentUser, SocializRUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            this.currentUser = currentUser;
        }

        public bool IsFriendWith(int with)
        {
            return unitOfWork.Friendships.Query.AsNoTracking().Any(e => e.IdSender == currentUser.Id && e.IdReceiver == with && (e.Accepted??false));
        }
        public bool SendFriendRequest(int to)
        {
            
            if(unitOfWork.Friendships.Query.Any(e=>e.IdReceiver == currentUser.Id && e.IdSender == to)
                ||(unitOfWork.Users.Query.FirstOrDefault(e=>e.Id==currentUser.Id)?.IsBanned??false)
                )
            {
                return false;
            }
            var friendRequest = new Friendship()
            {
                IdSender = currentUser.Id,
                IdReceiver = to,
                Accepted = null,
                CreatedOn = DateTime.Now
            };
            unitOfWork.Friendships.Add(friendRequest);
            return unitOfWork.SaveChanges() != 0;

        }

       public bool RefuseFriendRequest(int from)
        {
            var friendRequest = unitOfWork.Friendships.Query
                .FirstOrDefault(e => e.IdSender == from && e.IdReceiver == currentUser.Id);
            if (friendRequest == null) return false;
            friendRequest.Accepted = false;
            unitOfWork.Friendships.Update(friendRequest);
            return unitOfWork.SaveChanges() != 0;

        }
        public bool AcceptFriendRequest(int from)
        {
            if (unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false)
            {
                return false;
            }
            var friendRequest = unitOfWork.Friendships.Query
                .FirstOrDefault(e => e.IdSender == from && e.IdReceiver == currentUser.Id);
            if (friendRequest == null) return false;
            if (friendRequest.Accepted != true)
            {
                friendRequest.Accepted = true;
                var friendRequest2 = unitOfWork.Friendships.Query
                .FirstOrDefault(e => e.IdSender == currentUser.Id && e.IdReceiver == from);
                if (friendRequest2 == null)
                {
                    friendRequest2 = new Friendship()
                    {
                        IdReceiver = friendRequest.IdSender,
                        IdSender = friendRequest.IdReceiver,
                        CreatedOn = friendRequest.CreatedOn,
                        Accepted = true
                    };
                    unitOfWork.Friendships.Add(friendRequest2);
                }
                else
                {
                    friendRequest2.Accepted = true;
                    unitOfWork.Friendships.Update(friendRequest2);
                }
                unitOfWork.Friendships.Update(friendRequest);

                return unitOfWork.SaveChanges() != 0;
            }
            else return true;
        }

        public bool IsFriendRequested(int by)
        {
            return unitOfWork.Friendships.Query
                .AsNoTracking()
                .Any(e => e.IdSender == by && e.IdReceiver == currentUser.Id && !e.Accepted.HasValue);
        }
        public bool CanSee(int receiver)
        {
            return currentUser.IsAdmin|| unitOfWork.Friendships.Query
                .Include(e => e.IdReceiverNavigation)
                .Any(e => 
                (e.IdSender == currentUser.Id && e.IdReceiver == receiver&&(e.Accepted??false) && !e.IdReceiverNavigation.IsBanned))
                ||unitOfWork.Users.Query.Any(e=>e.Id==receiver&&e.Confidentiality == "public"&&!e.IsBanned)
                ;
        }

        public bool CanSendRequest(int receiver)
        {
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            return !IsRefused(receiver) && !IsAlreadySent(receiver) && !IsFriendWith(receiver) && currentUser.IsAuthenticated && isBanned;
        }
        public bool IsRefused(int by)
        {
            return unitOfWork.Friendships.Query
                .AsNoTracking()
                .Any(e => e.IdSender == currentUser.Id && e.IdReceiver == by && e.Accepted.Value == false);
        }

        public bool IsAlreadySent(int to)
        {
            return unitOfWork.Friendships.Query
                .AsNoTracking()
                .Any(e => e.IdSender == currentUser.Id && e.IdReceiver == to && !e.Accepted.HasValue);
        }

        public List<Users> GetFriends(int toSkip,int howMany)
        {
            return unitOfWork.Friendships.Query
                .Where(e => e.IdSender == currentUser.Id && (e.Accepted ?? false))
                .Skip(toSkip).Take(howMany)
                .Select(e => e.IdReceiverNavigation)
                .ToList();
        }

        public List<Users> GetRequesters(int toSkip,int howMany)
        {
            return unitOfWork.Friendships.Query
                .Where(e => e.IdReceiver == currentUser.Id && !e.Accepted.HasValue)
                .Skip(toSkip)
                .Take(howMany)
                .Select(e => e.IdSenderNavigation)
                .ToList();
        }

    }
}
