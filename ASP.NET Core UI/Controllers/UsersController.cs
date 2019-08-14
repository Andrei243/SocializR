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

namespace ASP.NET_Core_UI.Controllers
{
    public class UsersController : Code.Base.BaseController
    {
        private readonly Services.User.UserService userService;
        private readonly SocializRUnitOfWork unitOfWork;

        public List<Models.UserFriendModel> GetUsersByName(string name)
        {
            var el= userService
                .GetUsersByName(name)
                .Select(e => new Models.UserFriendModel() { Id = e.Id, ProfilePhotoId = e.PhotoId, Name = e.Name+" " + e.Surname })
                .OrderBy(e => e.Name)
                .Take(5)
                .ToList();
            return el;
        }

        public UsersController(SocializRUnitOfWork unitOfWork,IMapper mapper,Services.User.UserService userService)
            :base(mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }

        // GET: Users
        //[Authorize(Policy ="Admin")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Index()
        {
            var socializRContext = userService.getAll();
            return View(socializRContext.ToList());
        }

        // GET: Users/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = userService.getUserById(id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["LocalityId"] = new SelectList(unitOfWork.Localities.Query, "Id", "Name");
            ViewData["RoleId"] = new SelectList(unitOfWork.Roles.Query, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email,Password,RoleId,BirthDay,LocalityId,SexualIdentity,Vizibility")] Users users)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Users.Add(users);
                unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalityId"] = new SelectList(unitOfWork.Localities.Query, "Id", "Name", users.LocalityId);
            ViewData["RoleId"] = new SelectList(unitOfWork.Roles.Query, "Id", "Name", users.RoleId);
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = userService.getUserById(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["LocalityId"] = new SelectList(unitOfWork.Localities.Query, "Id", "Name", users.LocalityId);
            ViewData["RoleId"] = new SelectList(unitOfWork.Roles.Query, "Id", "Name", users.RoleId);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email,Password,RoleId,BirthDay,LocalityId,SexualIdentity,Vizibility")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Users.Update(users);
                    unitOfWork.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalityId"] = new SelectList(unitOfWork.Localities.Query, "Id", "Name", users.LocalityId);
            ViewData["RoleId"] = new SelectList(unitOfWork.Roles.Query, "Id", "Name", users.RoleId);
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = userService.getUserById(id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = userService.getUserById(id);
            unitOfWork.Users.Remove(users);
            unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return userService.getUserById(id)!=null;
        }
    }
}
