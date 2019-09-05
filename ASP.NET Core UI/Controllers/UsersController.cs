using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Domain;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ASP.NET_Core_UI.Models.GeneralModels;
using ASP.NET_Core_UI.Models.AdminModels;
using ASP.NET_Core_UI.Models.ProfileModels;
using ASP.NET_Core_UI.Models.JsonModels;

namespace ASP.NET_Core_UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UsersController : Code.Base.BaseController
    {
        private readonly Services.User.UserService userService;
        private readonly Services.CurrentUser currentUser;
        private readonly Services.FriendShip.FriendshipService friendshipService;
        private readonly Services.InterestsUsers.InterestsUsersService interestsUsersService;
        private readonly Services.County.CountyService countyService;
        private readonly Services.Interest.InterestService interestService;
        private readonly Services.Album.AlbumService albumService;
        private readonly Services.Photo.PhotoService photoService;


        public UsersController(IMapper mapper,
            Services.User.UserService userService,
            Services.CurrentUser currentUser,
            Services.FriendShip.FriendshipService friendshipService,
            Services.InterestsUsers.InterestsUsersService interestsUsersService,
            Services.County.CountyService countyService,
            Services.Interest.InterestService interestService,
            Services.Album.AlbumService albumService,
            Services.Photo.PhotoService photoService)
            : base(mapper)
        {
            this.userService = userService;
            this.currentUser = currentUser;
            this.friendshipService = friendshipService;
            this.interestsUsersService = interestsUsersService;
            this.countyService = countyService;
            this.interestService = interestService;
            this.albumService = albumService;
            this.photoService = photoService;
            PageSize = 500;
        }
        [AllowAnonymous]
        [HttpGet]
        public List<UserDropdownModel> GetUsersByName(string name)
        {
            var el = userService
                .GetUsersByName(name)
                .Select(e => new UserDropdownModel() { Id = e.Id, ProfilePhotoId = e.PhotoId, Name = e.Name + " " + e.Surname })
                .OrderBy(e => e.Name)
                .Take(125)
                .ToList();
            return el;
        }

        [HttpGet]
        public JsonResult GetInterests(int? userId)
        {
            if (userId == null)
            {
                return Json(new List<InterestSelectJsonModel>());
            }
            var indexes = interestsUsersService.GetAllInterests(userId.Value)
                .Select(e => e.Id)
                .ToList();
            var interests = interestService.GetAll().Select(e =>
            {
                var item = mapper.Map<InterestSelectJsonModel>(e);
                item.Selected = indexes.Contains(e.Id);
                return item;

            }).ToList();
            return Json(interests);
        }

        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        public IActionResult Details(int? userId)
        {
            if (!userId.HasValue || userId == 0 || userService.GetUserById(userId) == null)
            {
                return NotFoundView();
            }

            if (userId == currentUser.Id)
            {
                return RedirectToAction("Index", "Profile", null);
            }

            var domainUser = userService.GetUserById(userId);
            ProfileViewerModel user = mapper.Map<ProfileViewerModel>(domainUser);
            user.CanSee = friendshipService.CanSee(userId.Value);
            user.CanSendRequest = friendshipService.CanSendRequest(userId.Value);
            user.IsRequested = friendshipService.IsFriendRequested(userId.Value);
            user.Interests = interestsUsersService.GetAllInterests(domainUser.Id)
                .Select(e => e.Name)
                .ToList();
            user.Album = albumService.GetAll(userId.Value).Select(e => mapper.Map<Models.DomainModels.AlbumDomainModel>(e)).ToList();

            return View(user);

        }



        [HttpGet]
        public IActionResult Edit(int? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var user = userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            var model = mapper.Map<EditUserModel>(user);

            var counties = countyService.GetAll();


            model.Counties = counties.Select(c => mapper.Map<SelectListItem>(c)).ToList();
            return View(model);

        }

        [HttpPost]
        public IActionResult Edit(EditUserModel user)
        {
            if (ModelState.IsValid)
            {
                Request.Form.TryGetValue("Interests",
                                         out var raspunsuri);
                interestsUsersService.ChangeInterests(user.Id, raspunsuri.Select(e => int.Parse(e)).ToList());


                var updateUser = mapper.Map<Users>(user);
                userService.Update(updateUser);

                return RedirectToAction("Index");
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult DeleteAlbum(int? userId, int? albumId)
        {
            if (userId == null || albumId == null)
            {
                return NotFound();
            }
            albumService.RemoveAlbum(albumId.Value, userId.Value);


            return RedirectToAction("Details", new { userId });
        }
        [HttpGet]
        public IActionResult Album(int? albumId)
        {
            if (albumId == null)
            {
                return NotFound();
            }

            var album = albumService.GetAlbum(albumId.Value);
            if (album == null)
            {
                return NotFound();
            }

            var model = mapper.Map<AlbumEditModel>(album);

            return View(model);
        }
        [HttpGet]
        public IActionResult DeletePhoto(int? photoId, int? albumId)
        {
            if (photoId == null || albumId == null)
            {
                return NotFound();
            }
            photoService.RemovePhoto(photoId.Value);
            return RedirectToAction("Album", new { albumId });

        }

        [HttpGet]
        public IActionResult Delete(int? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            userService.RemoveUser(userId.Value);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Ban(int? userId)
        {
            if (userId == null) { return NotFound(); }

            userService.BanUser(userId.Value);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Unban(int? userId)
        {
            if (userId == null) { return NotFound(); }

            userService.UnbanUser(userId.Value);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult MakeAdmin(int? userId)
        {
            if (userId == null)
            {
                return NotFoundView();
            }
            userService.MakeAdmin(userId.Value);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult RevokeAdmin(int? userId)
        {
            if (userId == null)
            {
                return NotFoundView();
            }
            userService.RevokeAdmin(userId.Value);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public JsonResult GetUsers(int toSkip)
        {
            var users = userService.GetUsers(toSkip, PageSize).Select(mapper.Map<UserJsonModel>).ToList();
            return Json(users);

        }

    }
}
