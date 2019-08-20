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

        public bool isFriendWith(int with)
        {
            return unitOfWork.Friendships.Query.AsNoTracking().Any(e => e.IdSender == currentUser.Id && e.IdReceiver == with && (e.Accepted??false));
        }
        public bool SendFriendRequest(int to)
        {
            
            if(unitOfWork.Friendships.Query.Any(e=>e.IdReceiver == currentUser.Id && e.IdSender == to))
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
            var friendRequest = unitOfWork.Friendships.Query.Where(e => e.IdSender == from && e.IdReceiver == currentUser.Id).ToList()[0];
            friendRequest.Accepted = false;
            unitOfWork.Friendships.Update(friendRequest);
            return unitOfWork.SaveChanges() != 0;

        }
        public bool AcceptFriendRequest(int from)
        {
            var friendRequest = unitOfWork.Friendships.Query.Where(e => e.IdSender == from && e.IdReceiver == currentUser.Id).ToList()[0];
            friendRequest.Accepted = true;
            var friendRequest2 = new Friendship()
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

        public List<Users> getAllFriends()
        {
            return unitOfWork.Friendships.Query.AsNoTracking().Include(e => e.IdReceiverNavigation).Where(e => e.IdSender == currentUser.Id && (e.Accepted ?? false)).Select(e => e.IdReceiverNavigation).ToList();

        }

        public List<Users> getRequesters()
        {
            return unitOfWork.Friendships.Query.Where(e => e.IdReceiver == currentUser.Id && !e.Accepted.HasValue).Include(e=>e.IdSenderNavigation).Select(e => e.IdSenderNavigation).AsNoTracking().ToList();
        }

        public bool isFriendRequested(int by)
        {
            return unitOfWork.Friendships.Query.AsNoTracking().Any(e => e.IdSender == by && e.IdReceiver == currentUser.Id && !e.Accepted.HasValue);
        }
        public bool canSee(int receiver)
        {
            return unitOfWork.Friendships.Query.Include(e => e.IdReceiverNavigation).Any(
                e => (e.IdSender == currentUser.Id && e.IdReceiver == receiver&&(e.Accepted??false) && !e.IdReceiverNavigation.IsBanned))
                ||unitOfWork.Users.Query.Any(e=>e.Id==receiver&&e.Vizibility=="public"&&!e.IsBanned)
                ;
        }

        public bool canSendRequest(int receiver)
        {
            return !isRefused(receiver) && !isAlreadySent(receiver) && !isFriendWith(receiver)&&currentUser.IsAuthenticated;
        }
        public bool isRefused(int by)
        {
            return unitOfWork.Friendships.Query.AsNoTracking().Any(e => e.IdSender == currentUser.Id && e.IdReceiver == by && e.Accepted.Value == false);
        }

        public bool isAlreadySent(int to)
        {
            return unitOfWork.Friendships.Query.AsNoTracking().Any(e => e.IdSender == currentUser.Id && e.IdReceiver == to && !e.Accepted.HasValue);
        }

    }
}