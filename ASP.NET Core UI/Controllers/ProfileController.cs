using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Services;
using AutoMapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASP.NET_Core_UI.Models;

namespace ASP.NET_Core_UI.Controllers
{
    public class ProfileController : Code.Base.BaseController
    {
        //private readonly SocializRUnitOfWork unitOfWork;
        private readonly Services.County.CountyService countyService;
        private readonly Services.User.UserService userService;
        private readonly CurrentUser currentUser;
        private readonly Services.FriendShip.FriendRequest friendService;


        public ProfileController(IMapper mapper,CurrentUser currentUser, Services.FriendShip.FriendRequest friendRequest,Services.User.UserService userService,Services.County.CountyService countyService)
            : base(mapper)
        {
            this.userService = userService;
            this.countyService = countyService;
            this.currentUser = currentUser;
            this.friendService = friendRequest;
        }


        public IActionResult Index()
        {
            //var user = userService.getUserById(currentUser.Id);
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {

            var user = userService.getCurrentUser();
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
                Domain.Users updateUser = new Domain.Users
                {
                    BirthDay = user.BirthDay,
                    Email = currentUser.Email,
                    Id = user.Id,
                    IsBanned = currentUser.IsBanned,
                    LocalityId = user.LocalityId,
                    Name = user.Name,
                    Password = currentUser.Password,
                    PhotoId = currentUser.ProfilePhoto,
                    RoleId = currentUser.IsAdmin ? 1 : 2,
                    SexualIdentity = user.SexualIdentity,
                    Surname = user.Surname,
                    Vizibility = user.Visibility
                };
                userService.Update(updateUser);

                return RedirectToAction("Index");
            }
            return View(user);

        }

        public IActionResult Profile(int? userId)
        {
            if(!userId.HasValue || userId == 0 || userService.getUserById(userId) ==null)
            {
                return NotFoundView();
            }
            else
            {
                if (userId == currentUser.Id)
                {
                    return RedirectToAction("Index", "Profile", null);
                }
                var user = userService.getUserById(userId);
                return View(user);
            }

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


    }
}