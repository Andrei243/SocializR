﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Domain;

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
            post.Confidentiality = post.Confidentiality ?? Confidentiality.FriendsOnly;
            unitOfWork.Posts.Add(post);
            return unitOfWork.SaveChanges()!=0;

        }

        

        public List<Domain.Post> GetPersonPost(int toSkip,int howMany,int userId)
        {
            return GetFeed()
                .Where(e => e.UserId == userId)
                .OrderByDescending(e=>e.AddingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
        }

        public bool CanDetelePost(int postId)
        {
            var post = unitOfWork.Posts.Query.FirstOrDefault(e => e.Id == postId);
            if (post == null) return false;
            return post.UserId == CurrentUser.Id || CurrentUser.IsAdmin;
        }

        private IEnumerable<Domain.Post> GetFeed()
        {
            return unitOfWork
                            .Posts
                            .Query
                            .AsNoTracking()
                            .Include(e => e.User)
                            .AsNoTracking()
                            .Include(e => e.Photo)
                            .AsNoTracking()
                            .Include(e => e.Reaction)
                            .AsNoTracking()
                            .Where(e=>!e.User.IsBanned)
                            ;
        }

        public List<Domain.Post> GetPublicNewsfeed(int toSkip,int howMany)
        {
            return GetFeed()
                .Where(e=>e.Confidentiality == Confidentiality.Public)
                .OrderByDescending(e => e.AddingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
        }

        public List<Domain.Post> GetNewsfeed(int toSkip,int howMany)
        {
            var friends = unitOfWork.Friendships.Query
                .AsNoTracking()
                .Where(e => e.IdReceiver == CurrentUser.Id && (e.Accepted??false))
                .Select(e=>e.IdSender)
                .ToList();

            
            var posts = GetFeed()
                .Where(e =>
                (e.Confidentiality == Confidentiality.Public) ||
                (e.UserId == CurrentUser.Id) ||
                (friends.Contains(e.UserId) && e.Confidentiality == Confidentiality.FriendsOnly)
                )
                .OrderByDescending(e => e.AddingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
            return posts;
        }
        

        public bool RemovePost(int postId)
        {
            var post = unitOfWork.Posts.Query
                .FirstOrDefault(e => e.Id == postId);
            if (post == null) return false;

            var reactions = unitOfWork.Reactions.Query.Where(e => e.PostId == postId);
            var comments = unitOfWork.Comments.Query.Where(e => e.PostId == postId);
            var photos = unitOfWork.Photos.Query.Where(e => e.PostId == postId);
            unitOfWork.Reactions.RemoveRange(reactions);
            unitOfWork.Comments.RemoveRange(comments);
            unitOfWork.Photos.RemoveRange(photos);
            unitOfWork.Posts.Remove(post);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool CanSeePost(int postId)
        {
            if (CurrentUser.IsAdmin) return true;
            var post = unitOfWork.Posts.Query.Include(e=>e.User).FirstOrDefault(e => e.Id == postId);
            if (post == null || post.User.IsBanned) return false;
            if (post.UserId == CurrentUser.Id || post.Confidentiality == Confidentiality.Public) return true;
            var suntPrieteni = unitOfWork.Friendships.Query
                .Any(e => e.IdReceiver == post.UserId && e.IdSender == CurrentUser.Id && (e.Accepted ?? false));
            if (suntPrieteni) return post.Confidentiality == Confidentiality.FriendsOnly;
            else return false;
        }
    }
}
