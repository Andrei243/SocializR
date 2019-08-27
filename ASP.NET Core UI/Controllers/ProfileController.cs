﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Services;
using AutoMapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASP.NET_Core_UI.Models;
using System.IO;
using ASP.NET_Core_UI.Models.ProfileModels;
using ASP.NET_Core_UI.Models.DomainModels;
using ASP.NET_Core_UI.Models.GeneralModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ASP.NET_Core_UI.Models.JsonModels;

namespace ASP.NET_Core_UI.Controllers
{
    public class ProfileController : Code.Base.BaseController
    {
        private readonly Services.County.CountyService countyService;
        private readonly Services.User.UserService userService;
        private readonly CurrentUser currentUser;
        private readonly Services.FriendShip.FriendshipService friendService;
        private readonly Services.Interest.InterestService interestService;
        private readonly Services.InterestsUsers.InterestsUsersService interestsUsersService;
        private readonly Services.Album.AlbumService albumService;
        private readonly Services.Photo.PhotoService photoService;


        public ProfileController(
            IMapper mapper,
            CurrentUser currentUser, 
            Services.FriendShip.FriendshipService friendshipService,
            Services.User.UserService userService,
            Services.County.CountyService countyService,
            Services.Interest.InterestService interestService,
            Services.InterestsUsers.InterestsUsersService interestsUsersService,
            Services.Album.AlbumService albumService,
            Services.Photo.PhotoService photoService
            )
            : base(mapper)
        {
            this.interestsUsersService = interestsUsersService;
            this.interestService = interestService;
            this.userService = userService;
            this.countyService = countyService;
            this.currentUser = currentUser;
            this.friendService = friendshipService;
            this.albumService = albumService;
            this.photoService = photoService;
        }

        public JsonResult GetInterests()
        {
            var indexi = interestsUsersService.GetAllInterests(currentUser.Id).Select(e=>e.Id).ToList();
            var interests = interestService.getAll().Select(e =>
             {
                 var item = mapper.Map<InterestSelect>(e);
                 item.Selected = indexi.Contains(e.Id);
                 return item;

             }).ToList();
            return Json(interests);
        }

        public bool ChangeDescription(int? photoId,string description)
        {
            if (photoId == null || !photoService.HasThisPhoto(photoId.Value, currentUser.Id))
            {
                return false;
            }
            photoService.ChangeDescription(photoId.Value, description);
            return true;
        }

        public JsonResult GetPhotosJson(int? already,int? albumId)
        {
            if (already == null || albumId == null)
            {
                return Json(new List<int>());
            }
            if (albumService.CanSeeAlbum(albumId.Value))
            {
                var photos = photoService.GetPhotos(already.Value, PageSize, albumId.Value).Select(e => mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Image>(e)).ToList();
                return Json(photos);
            }
            else return Json(new List<int>());

        }

        public IActionResult Index()
        {
            var domainUser = userService.GetUserById(currentUser.Id);
            var user = mapper.Map<ProfileViewerModel>(domainUser);
            user.Interests = interestsUsersService.GetAllInterests(domainUser.Id).Select(e => e.Name).ToList();
            user.Album = albumService.GetAll(currentUser.Id).Select(
                e=>mapper.Map<Album>(e)
                ).ToList();
            return View(user);
        }

        public List<Photo> GetPhotos(int? albumId)
        {
            if (albumId == null)
            {
                return new List<Photo>();
            }
            if (albumService.CanSeeAlbum(albumId.Value))
            {
                return albumService.GetPhotos(albumId.Value).Select(e => mapper.Map<Photo>(e)).ToList();
            }
            return new List<Photo>();
        }

        public IActionResult MakeProfilePhoto(int? photoId)
        {
            if (photoId == null)
            {
                return NotFound();
            }
            if (!userService.HasThisPhoto(photoId.Value))
            {
                return RedirectToAction("Index", "Profile");
            }
            userService.UpdateProfilePhoto(photoId.Value);
            return RedirectToAction("Index", "Profile");

        }

        public IActionResult Album(int? albumId)
        {
            if (albumId == null)
            {
                return NotFound();
            }
            if (!albumService.CanSeeAlbum(albumId.Value))
            {
                return ForbidView();
            }

            AlbumViewerModel albumViewerModel = new AlbumViewerModel()
            {
                poze = photoService.getPhotos(null, albumId).Select(e => mapper.Map<Photo>(e)).ToList(),
                PhotoModel = new PhotoModel() { AlbumId = albumId },
                HasThisAlbum = albumService.HasThisAlbum(albumId.Value),
                Id=albumId.Value
            };

            return View(albumViewerModel);
        }

        public IActionResult RemovePhoto(int? photoId,int? albumId)
        {
            if (photoId == null||albumId==null )
            {
                return NotFound();
            }
            if(!photoService.HasThisPhoto(photoId.Value, currentUser.Id))
            {
                return ForbidView();
            }

            photoService.RemovePhoto(photoId.Value,null,albumId.Value);

            return RedirectToAction("Album", new { albumId = albumId.Value });
        }

        [HttpPost]
        public IActionResult AddPhoto(PhotoModel model,int? albumId)
        {
            if (ModelState.IsValid)
            {

                var photo = new Domain.Photo()
                {
                    AlbumId = albumId,
                    PostId = null,
                    MIMEType = model.Binar.ContentType
                };
                using (var memoryStream = new MemoryStream())
                {
                    model.Binar.CopyTo(memoryStream);
                    photo.Binar = memoryStream.ToArray();
                    
                }
                photoService.AddPhoto(photo);

                return RedirectToAction("Album", "Profile", new { albumId = model.AlbumId });
            }
            if (albumId == null)
            {
                return NotFound();
            }
            AlbumViewerModel albumViewerModel = new AlbumViewerModel()
            {
                poze = photoService.getPhotos(null, albumId).Select(e => mapper.Map<Photo>(e)).ToList(),
                PhotoModel = model,
                HasThisAlbum = albumService.HasThisAlbum(albumId.Value),
                Id = albumId.Value
            };
            return View("Album", albumViewerModel);
        }

        [HttpGet]
        public IActionResult Edit()
        {

            var user = userService.GetCurrentUser();
            var model = mapper.Map<EditUserModel>(user);

            var counties = countyService.GetAll();

            var interests = interestService.getAll();

            ///model.Interests = interestService.GetAllSelectListItems(currentUser.Id);
            model.Counties = counties.Select(c => mapper.Map<SelectListItem>(c)).ToList();
            model.Albume = albumService.GetAll(currentUser.Id).Select(e => mapper.Map<Album>(e)).ToList();
            return View(model);

        }

        //refactor
        [HttpPost]
        public IActionResult Edit(EditUserModel user)
        {
            if (ModelState.IsValid)
            {
                Microsoft.Extensions.Primitives.StringValues raspunsuri;
                Request.Form.TryGetValue("Interests",
                                         out raspunsuri);
                interestsUsersService.ChangeInterests(currentUser.Id, raspunsuri.Select(e => int.Parse(e)).ToList());
               

                var updateUser = mapper.Map<Domain.Users>(user);
                userService.Update(updateUser);

                return RedirectToAction("Index");
            }
            user.Albume = albumService.GetAll(user.Id).Select(e => mapper.Map<Album>(e)).ToList();
            return View(user);

        }

        [HttpPost]
        public IActionResult AddAlbum(AddAlbumModel model)
        {
            if (ModelState.IsValid)
            {
                int albumId= albumService.AddAlbum(model.Name);
                return RedirectToAction("Album", "Profile",new {albumId });
            }
            return PartialView("PartialAddAlbum", model);
        }

        public IActionResult Profile(int? userId)
        {
            if(!userId.HasValue || userId == 0 || userService.GetUserById(userId) ==null)
            {
                return NotFoundView();
            }
            else
            {
                if (userId == currentUser.Id)
                {
                    return RedirectToAction("Index", "Profile", null);
                }

                var domainUser = userService.GetUserById(userId);
                ProfileViewerModel user = mapper.Map<ProfileViewerModel>(domainUser);
                user.CanSee = friendService.canSee(userId.Value);
                user.CanSendRequest = friendService.canSendRequest(userId.Value);
                user.IsRequested = friendService.isFriendRequested(userId.Value);
                user.Interests = interestsUsersService.GetAllInterests(domainUser.Id).Select(e => e.Name).ToList();
                user.Album = albumService.GetAll(userId.Value).Select(e => mapper.Map<Album>(e)).ToList();

                return View(user);
            }

        } 

        public IActionResult RemoveAlbum(int? albumId)
        {
            if (albumId == null)
            {
                return NotFound();
            }
            if (albumService.CanDeleteAlbum(albumId.Value))
            {
                albumService.RemoveAlbum(albumId.Value, currentUser.Id);
            }
            return RedirectToAction("Index", "Profile", null);
        }

        public IActionResult FriendRequests()
        {
            return View();
        }

        public IActionResult FriendList()
        {
            return View();
        }

        public IActionResult Accept(int id)
        {
            friendService.AcceptFriendRequest(id);
            return RedirectToAction("Profile", "Profile", new { userId = id });
        }

        public IActionResult Refuse(int id)
        {
            friendService.RefuseFriendRequest(id);
            return RedirectToAction("Index", "Profile", null);
        }

        public IActionResult Send(int id)
        {
            friendService.SendFriendRequest(id);
            return RedirectToAction("Profile", "Profile", new {userId=id });
        }
        [Authorize]
        public JsonResult GetFriends(int already)
        {
            var friends = friendService.GetFriends(already, PageSize).Select(e => mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Friend>(e)).ToList();
            return Json(friends);

        }
        [Authorize]
        public JsonResult GetRequesters(int already)
        {
            var friends = friendService.GetRequesters(already, PageSize).Select(e => mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Friend>(e)).ToList();
            return Json(friends);

        }

    }
}