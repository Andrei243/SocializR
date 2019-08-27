using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_Core_UI.Models;
using AutoMapper;
using Services;
using ASP.NET_Core_UI.Code.Base;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using ASP.NET_Core_UI.Models.FeedModels;
using Domain;
using Newtonsoft.Json;

namespace ASP.NET_Core_UI.Controllers
{
    public class FeedController : BaseController
    {
        private readonly Services.Post.PostService postService;
        private readonly Services.Comment.CommentService commentService;
        private readonly CurrentUser currentUser;
        private readonly Services.Photo.PhotoService photoService;
        private readonly Services.Reaction.ReactionService reactionService;
        public FeedController(IMapper mapper,
            Services.Post.PostService postService,
            Services.Comment.CommentService commentService,
            CurrentUser currentUser,
            Services.Photo.PhotoService photoService,
            Services.Reaction.ReactionService reactionService) : base(mapper)
        {
            this.postService = postService;
            this.commentService = commentService;
            this.currentUser = currentUser;
            this.photoService = photoService;
            this.reactionService = reactionService;
        }
        [Authorize]
        public void RemoveComment(int commentId)
        {
            if (commentService.CanDeleteComment(commentId))
            {
                commentService.RemoveComment(commentId);
            }
        }
        [Authorize]
        public void RemovePost(int postId)
        {
            if (postService.CanDetelePost(postId))
            { 
                postService.RemovePost(postId);
            }
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var postModelAdd = new PostAddModel();
            return View(postModelAdd);
        }

        
       
        [Authorize]
        public JsonResult GetComments(int postId,int already)
        {
            if (postService.CanSeePost(postId))
            {
                var comments = commentService.GetComments(already, PageSize, postId).Select(e =>
                {
                    var comment = mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Comment>(e);
                    comment.IsMine = (currentUser.Id == comment.UserId);
                    return comment;
                }).ToList();
                return Json(comments);
            }
            return Json(new List<int>());
        }

        public JsonResult GetPosts(int already)
        {
            
            if (currentUser.IsAuthenticated)
            {
                var posts = postService.GetNewsfeed(already, PageSize).Select(e =>
                {
                    var post = mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Post>(e);
                    post.Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id);
                    post.IsMine = e.UserId == currentUser.Id;
                    return post;

                }).ToList();
                return Json(posts);
            }
            else
            {
                var posts = postService.GetPublicNewsfeed(already, PageSize).Select(e =>
                {
                    var post = mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Post>(e);
                    post.Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id);
                    post.IsMine = false;
                    return post;

                }).ToList();
                return Json(posts);
            }
        }
        public JsonResult GetPersonPosts(int already,int userId)
        {

                var posts = postService.GetPersonPost(already, PageSize,userId).Select(e =>
                {
                    var post = mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Post>(e);
                    post.Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id);
                    post.IsMine = e.UserId == currentUser.Id;
                    return post;
                }).ToList();
                return Json(posts);
            
        }

        [HttpPost]
        public IActionResult AddPost(PostAddModel post)
        {
            if (ModelState.IsValid)
            {
                
                var newPost = mapper.Map<Domain.Post>(post);
                postService.AddPost(newPost);

                if (post.Binar != null)
                {
                    Domain.Photo photo = new Domain.Photo()
                    {
                        Position = 1,
                        PostId = newPost.Id,
                        MIMEType = post.Binar.ContentType
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        post.Binar.CopyTo(memoryStream);
                        photo.Binar = memoryStream.ToArray();
                    }
                    photoService.AddPhoto(photo);
                }
                return RedirectToAction("Index");
            }
            
            return View("Index", post);
        }

      
        public bool Reaction(int postId)
        {
            if(postService.CanSeePost(postId)) return reactionService.changeReaction(postId);
            return false;
        }

        public void Comment(int postId,string comentariu)
        {
            if (postService.CanSeePost(postId))
            {
                commentService.AddComment(comentariu, postId);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
