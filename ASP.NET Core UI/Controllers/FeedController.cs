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
using ASP.NET_Core_UI.Models.JsonModels;

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
            this.PageSize = 5;
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
        public bool RemovePost(int postId)
        {
            if (postService.CanDetelePost(postId))
            { 
                postService.RemovePost(postId);
                return true;
            }
            return false;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var postModelAdd = new PostAddModel();
            return View(postModelAdd);
        }

        
       
        [AllowAnonymous]
        public JsonResult GetComments(int postId,int toSkip)
        {
            if (postService.CanSeePost(postId))
            {
                var comments = commentService.GetComments(toSkip, PageSize, postId).Select(e =>
                {
                    var comment = mapper.Map<CommentJsonModel>(e);
                    comment.IsMine = (currentUser.Id == comment.UserId);
                    comment.IsAdmin = currentUser.IsAdmin;
                    return comment;
                }).ToList();
                return Json(comments);
            }
            return Json(new List<int>());
        }

        public JsonResult GetPosts(int toSkip)
        {
            
            if (currentUser.IsAuthenticated)
            {
                var posts = postService.GetNewsfeed(toSkip, PageSize).Select(e =>
                {
                    var post = mapper.Map<PostJsonModel>(e);
                    post.Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id);
                    post.IsMine = e.UserId == currentUser.Id;
                    post.IsAdmin = currentUser.IsAdmin;
                    return post;

                }).ToList();
                return Json(posts);
            }
            else
            {
                var posts = postService.GetPublicNewsfeed(toSkip, PageSize).Select(e =>
                {
                    var post = mapper.Map<PostJsonModel>(e);
                    post.Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id);
                    post.IsMine = false;
                    return post;

                }).ToList();
                return Json(posts);
            }
        }
        public JsonResult GetPersonPosts(int toSkip,int userId)
        {

                var posts = postService.GetPersonPost(toSkip, PageSize,userId).Select(e =>
                {
                    var post = mapper.Map<PostJsonModel>(e);
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
                
                var newPost = mapper.Map<Post>(post);
                postService.AddPost(newPost);

                if (post.Binar != null)
                {
                    var photo = new Photo()
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
            if(postService.CanSeePost(postId)) return reactionService.ChangeReaction(postId);
            return false;
        }

        public int Comment(int postId,string comentariu)
        {
            if (postService.CanSeePost(postId)&& !string.IsNullOrEmpty(comentariu))
            {
                int id = commentService.AddComment(comentariu.Trim(), postId);
                return id;

            }
            return 0;
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
