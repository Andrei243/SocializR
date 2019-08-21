﻿using System;
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

namespace ASP.NET_Core_UI.Controllers
{
    [Authorize(Policy ="Admin")]
    public class UsersController : Code.Base.BaseController
    {
        private readonly Services.User.UserService userService;
        private readonly Services.CurrentUser currentUser;
        private readonly Services.FriendShip.FriendshipService friendshipService;
        private readonly Services.InterestsUsers.InterestsUsersService interestsUsersService;
        private readonly Services.County.CountyService countyService;
        private readonly Services.Locality.LocalityService localityService;
        private readonly Services.Interest.InterestService interestService;
       

        public UsersController(IMapper mapper,
            Services.User.UserService userService,
            Services.CurrentUser currentUser,
            Services.FriendShip.FriendshipService friendshipService,
            Services.InterestsUsers.InterestsUsersService interestsUsersService,
            Services.County.CountyService countyService,
            Services.Locality.LocalityService localityService,
            Services.Interest.InterestService interestService)
            :base(mapper)
        {
            this.userService = userService;
            this.currentUser = currentUser;
            this.friendshipService = friendshipService;
            this.interestsUsersService = interestsUsersService;
            this.countyService = countyService;
            this.localityService = localityService;
            this.interestService = interestService;
        }
        public List<UserDropdownModel> GetUsersByName(string name)
        {
            var el = userService
                .GetUsersByName(name)
                .Select(e => new UserDropdownModel() { Id = e.Id, ProfilePhotoId = e.PhotoId, Name = e.Name + " " + e.Surname })
                .OrderBy(e => e.Name)
                .Take(5)
                .ToList();
            return el;
        }

        // GET: Users
        
        public IActionResult Index()
        {
            var useri = userService.getAll().Select(e=>
            mapper.Map<UserIndex>(e)
            );
            return View(useri);
        }

        // GET: Users/Details/5
        [HttpGet]
        public IActionResult Details(int? userId)
        {
            if (!userId.HasValue || userId == 0 || userService.getUserById(userId) == null)
            {
                return NotFoundView();
            }
            else
            {
                if (userId == currentUser.Id)
                {
                    return RedirectToAction("Index", "Profile", null);
                }

                var domainUser = userService.getUserById(userId);
                ProfileViewerModel user = mapper.Map<ProfileViewerModel>(domainUser);
                user.CanSee = friendshipService.canSee(userId.Value);
                user.CanSendRequest = friendshipService.canSendRequest(userId.Value);
                user.IsRequested = friendshipService.isFriendRequested(userId.Value);
                user.Interests = interestsUsersService.GetAllInterests(domainUser.Id).Select(e => e.Name).ToList();

                return View(user);
            }
        }


        [HttpGet]
        public IActionResult Edit(int? userId)
        {

            var user = userService.getUserById(userId);
            var model = mapper.Map<EditUserModel>(user);

            var counties = countyService.GetAll();

            var interests = interestService.getAll();

            model.InterestsId = interestsUsersService.GetAllInterests(user.Id).Select(e => e.Id).ToList();
            model.Interests = interests.Select(c => mapper.Map<SelectListItem>(c)).ToList();
            model.Counties = counties.Select(c => mapper.Map<SelectListItem>(c)).ToList();
            return View(model);

        }

        [HttpPost]
        public IActionResult Edit(EditUserModel user)
        {
            if (ModelState.IsValid)
            {
                user.InterestsId = interestsUsersService.GetAllInterests(user.Id).Select(e => e.Id).ToList();
                Microsoft.Extensions.Primitives.StringValues raspunsuri;
                Request.Form.TryGetValue("Interests",
                                         out raspunsuri);
                var interese = raspunsuri.Select(e => int.Parse(e));
                foreach (int x in interese)
                {
                    if (!user.InterestsId.Contains(x))
                    {
                        interestsUsersService.AddInterest(x,user.Id);
                    }

                }
                foreach (int x in user.InterestsId)
                {
                    if (!interese.Contains(x))
                    {

                        interestsUsersService.RemoveInterest(x,user.Id);
                    }
                }
                
                var updateUser = mapper.Map<Domain.Users>(user);
                userService.Update(updateUser);

                return RedirectToAction("Index");
            }
            return View(user);
        }
        public IActionResult DeleteAlbum(int? userId,int? albumId)
        {
            if (userId == null||albumId==null)
            {
                return NotFound();
            }


            return RedirectToAction("Edit", new { userId = userId });
        }

        // GET: Users/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //TBC
            return View();
        }

       
    }
}
