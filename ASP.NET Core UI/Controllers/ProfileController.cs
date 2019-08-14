using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Services;
using AutoMapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NET_Core_UI.Controllers
{
    public class ProfileController : Code.Base.BaseController
    {
        private readonly SocializRUnitOfWork unitOfWork;
        //private readonly SocializRContext _context;
        private readonly CurrentUser currentUser;
        private readonly Services.FriendShip.FriendRequest friendService;


        public ProfileController(IMapper mapper,CurrentUser currentUser,SocializRContext context, Services.FriendShip.FriendRequest friendRequest,SocializRUnitOfWork unitOfWork)
            : base(mapper)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
            this.friendService = friendRequest;
            //_context = context;
        }


        public IActionResult Index()
        {
            var user = unitOfWork.Users.Query.Include(e=>e.Locality).ThenInclude(e=>e.County).Include(e=>e.InterestsUsers).FirstOrDefault(e=>e.Id==currentUser.Id);
            return View(user);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var user = unitOfWork.Users.Query.FirstOrDefault(e=>e.Id==currentUser.Id);
            ViewData["Counties"] = new SelectList(unitOfWork.Counties.Query, "Id", "Name");
            return View(user);

        }

        [HttpPost]
        public IActionResult Edit(Domain.Users user)
        {

            if (ModelState.IsValid)
            {
                unitOfWork.Users.Update(user);
                return RedirectToAction("Index");
            }
            return View(user);

        }

        public IActionResult Profile(int? id)
        {
            if(!id.HasValue || id == 0 || !unitOfWork.Users.Query.Any(e=>e.Id==id))
            {
                return NotFoundView();
            }
            else
            {
                if (id == currentUser.Id)
                {
                    return RedirectToAction("Index", "Profile", null);
                }
                var user = unitOfWork.Users.Query.Include(e => e.Locality).ThenInclude(e => e.County).Include(e => e.InterestsUsers).FirstOrDefault(e => e.Id == id);
                return View(user);
            }

        } 

        public IActionResult Accept(int id)
        {
            friendService.AcceptFriendRequest(id);
            return RedirectToAction("Profile", "Profile", new { id = id });
        }

        public IActionResult Refuse(int id)
        {
            friendService.RefuseFriendRequest(id);
            return RedirectToAction("Index", "Profile", null);
        }

        public IActionResult Send(int id)
        {
            friendService.SendFriendRequest(id);
            return RedirectToAction("Profile", "Profile", new {id=id });
        }


    }
}