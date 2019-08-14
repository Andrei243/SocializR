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

namespace ASP.NET_Core_UI.Controllers
{
    public class HomeController : BaseController
    {
        private Services.Post.PostService postService;
        private Services.Comment.CommentService commentService;
        public HomeController(IMapper mapper,Services.Post.PostService postService,Services.Comment.CommentService commentService) : base(mapper)
        {
            this.postService = postService;
            this.commentService = commentService;
        }

        
        
        public IActionResult Index()
        {
            return View();
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
