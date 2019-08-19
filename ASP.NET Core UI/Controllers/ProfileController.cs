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
        private readonly Services.County.CountyService countyService;
        private readonly Services.User.UserService userService;
        private readonly CurrentUser currentUser;
        private readonly Services.FriendShip.FriendshipService friendService;
        private readonly Services.Interest.InterestService interestService;
        private readonly Services.InterestsUsers.InterestsUsersService interestsUsersService;


        public ProfileController(IMapper mapper,CurrentUser currentUser, Services.FriendShip.FriendshipService friendRequest,Services.User.UserService userService,Services.County.CountyService countyService,
            Services.Interest.InterestService interestService,Services.InterestsUsers.InterestsUsersService interestsUsersService
            )
            : base(mapper)
        {
            this.interestsUsersService = interestsUsersService;
            this.interestService = interestService;
            this.userService = userService;
            this.countyService = countyService;
            this.currentUser = currentUser;
            this.friendService = friendRequest;
        }


        public IActionResult Index()
        {
            var domainUser = userService.getUserById(currentUser.Id);
            ProfileViewerModel user = mapper.Map<ProfileViewerModel>(domainUser);
            user.Interests = interestsUsersService.GetAllInterests(domainUser.Id).Select(e => e.Name).ToList();
            return View(user);
        }

        [HttpGet]
        public IActionResult Edit()
        {

            var user = userService.getCurrentUser();
            var model = mapper.Map<EditUserModel>(user);

            var counties = countyService.GetAll();

            var interests = interestService.getAll();

            model.InterestsId = interestsUsersService.GetAllInterests(currentUser.Id).Select(e => e.Id).ToList();
            model.Interests = interests.Select(c => mapper.Map<SelectListItem>(c)).ToList();
            model.Counties = counties.Select(c => mapper.Map<SelectListItem>(c)).ToList();
            return View(model);

        }

        [HttpPost]
        public IActionResult Edit(EditUserModel user)
        {
            if (ModelState.IsValid)
            {
                user.InterestsId= interestsUsersService.GetAllInterests(currentUser.Id).Select(e => e.Id).ToList();
                Microsoft.Extensions.Primitives.StringValues raspunsuri;
                Request.Form.TryGetValue("Interests",
                                         out raspunsuri);
                var interese = raspunsuri.Select(e => int.Parse(e));
                foreach(int x in interese)
                {
                    if (!user.InterestsId.Contains(x))
                    {
                        interestsUsersService.AddInterest(x);
                    }
                   
                }
                foreach (int x in user.InterestsId)
                {
                    if (!interese.Contains(x))
                    {

                        interestsUsersService.RemoveInterest(x);
                    }
                }     
                Domain.Users updateUser = new Domain.Users
                {
                    BirthDay = user.BirthDay,
                    Id = user.Id,
                    LocalityId = user.LocalityId,
                    Name = user.Name,
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

                var domainUser = userService.getUserById(userId);
                ProfileViewerModel user = mapper.Map<ProfileViewerModel>(domainUser);
                user.CanSee = friendService.canSee(userId.Value);
                user.CanSendRequest = friendService.canSendRequest(userId.Value);
                user.IsRequested = friendService.isFriendRequested(userId.Value);
                user.Interests = interestsUsersService.GetAllInterests(domainUser.Id).Select(e => e.Name).ToList();

                return View(user);
            }

        } 

        public IActionResult FriendList()
        {
            FriendListModel friendListModel=new FriendListModel();
            friendListModel.friends = friendService.getAllFriends().Select(e => new UserFriendModel { Id = e.Id, Name = e.Name + " " + e.Surname, ProfilePhotoId = e.PhotoId }).ToList();
            return View(friendListModel);
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