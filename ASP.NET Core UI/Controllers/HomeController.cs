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

namespace ASP.NET_Core_UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly Services.Post.PostService postService;
        private readonly Services.Comment.CommentService commentService;
        private readonly CurrentUser currentUser;
        private readonly Services.Photo.PhotoService photoService;
        private readonly Services.Reaction.ReactionService reactionService;
        public HomeController(IMapper mapper,
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

        
        [HttpGet]
        public IActionResult Index()
        {
            FeedModel feedModel = new FeedModel();
            if (currentUser.IsAuthenticated)
            {
                feedModel.Posts =
                    postService.GetNewsfeed()
                    .Select(e => new PostModel
                    {   Id=e.Id,
                        Text = e.Text,
                        User = new PostUserModel { Id = e.User.Id, Name = e.User.Name, ProfilePhoto = e.User.PhotoId },
                        Comments = e.Comment.Select(f => new CommentModel { Text = f.Content, User = new PostUserModel { Id = f.User.Id, Name = f.User.Name, ProfilePhoto = f.User.PhotoId } }).ToList(),
                        Reactions=e.Reaction.Select(f=>f.UserId ).ToList(),
                        PhotoId = photoService.getPhotos(e.Id, null).Select(f => f.Id).ToList()
                    }
                    )
                    .ToList();
            }
            else
            {
                feedModel.Posts =
                   postService.GetPublicNewsfeed()
                   .Select(e => new PostModel
                   {   Id=e.Id,
                       Text = e.Text,
                       User = new PostUserModel { Id = e.User.Id, Name = e.User.Name, ProfilePhoto = e.User.PhotoId },
                       Comments = e.Comment.Select(f => new CommentModel { Text = f.Content, User = new PostUserModel { Id = f.User.Id, Name = f.User.Name, ProfilePhoto = f.User.PhotoId } }).ToList(),
                       Reactions = e.Reaction.Select(f => f.UserId ).ToList(),
                       PhotoId = photoService.getPhotos(e.Id, null).Select(f => f.Id).ToList()

                   }
                   )
                   .ToList();

            }
            return View(feedModel);
        }

        [HttpPost]
        public IActionResult Index(FeedModel model)
        {
            if (ModelState.IsValid)
            {
                PostAddModel post = model.PostAdd;
                Domain.Post newPost = new Domain.Post
                {
                    Text = post.Text,
                    Vizibilitate = post.Visibility
                };
                postService.AddPost(newPost);

                if (post.Binar != null)
                {
                    Domain.Photo photo = new Domain.Photo()
                    {
                        Position = 1,
                        PostId=newPost.Id,
                        MIMEType = post.Binar.ContentType
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        post.Binar.CopyTo(memoryStream);
                        photo.Binar = memoryStream.ToArray();
                    }
                    photoService.AddPhoto(photo);
                }

            }
            return View(model);
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
