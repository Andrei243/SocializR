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
            commentService.RemoveComment(commentId);
        }
        [Authorize]
        public void RemovePost(int postId)
        {
            postService.RemovePost(postId);
        }
        
        [HttpGet]
        public IActionResult Index(int? page)
        {
            FeedModel feedModel = new FeedModel();
            feedModel.CurrentPage = page ?? 0;
            var postari = new List<Post>();
            if (currentUser.IsAuthenticated)
            {
                postari = postService.GetNewsfeed(feedModel.CurrentPage);
                    
            }
            else
            {
                postari = postService.GetPublicNewsfeed(feedModel.CurrentPage);

            }
            feedModel.Posts=postari.Select(e => 
            new PostModel()
            {
                Id = e.Id,
                Text = e.Text,
                User = mapper.Map<PostUserModel>(e.User), 
                Comments=commentService.GetComments(e.Id).Select(f=>mapper.Map<CommentModel>(f)).ToList(),
                Reactions = e.Reaction.Select(f => f.UserId).ToList(),
                PhotoId = photoService.getPhotos(e.Id, null).Select(f => f.Id).ToList()

            }
            
            )
            .ToList();
            return View(feedModel);
        }

        [HttpGet]
        public IActionResult MyFeed(int? page)
        {
            FeedModel feedModel = new FeedModel();
            feedModel.CurrentPage = page ?? 0;
            var postari = new List<Post>();
            
                postari = postService.GetAllPersonalPost(feedModel.CurrentPage);

            
            feedModel.Posts = postari.Select(e =>
              new PostModel()
              {
                  Id = e.Id,
                  Text = e.Text,
                  User = mapper.Map<PostUserModel>(e.User),
                  Comments = commentService.GetComments(e.Id).Select(f => mapper.Map<CommentModel>(f)).ToList(),
                  Reactions = e.Reaction.Select(f => f.UserId).ToList(),
                  PhotoId = photoService.getPhotos(e.Id, null).Select(f => f.Id).ToList()

              }

            )
            .ToList();
            return View(feedModel);
        }

        public IActionResult GetComments(int? postId,int page)
        {
            if (postId == null)
            {
                return Json(null);
            }

            //JsonResult result = new JsonResult(commentService.GetComments(page, postId.Value).Select(e => mapper.Map<CommentModel>(e)));
            return Json(commentService.GetComments(page, postId.Value).Select(e => mapper.Map<CommentModel>(e)));

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
            return PartialView("PartialPostAdd", post);
        }

      
        public bool Reaction(int postId)
        {
            return reactionService.changeReaction(postId);
        }

        public void Comment(int postId,string comentariu)
        {
            commentService.AddComment(comentariu, postId);
            
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
