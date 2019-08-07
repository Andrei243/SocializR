using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;

namespace ASP.NET_Core_UI.Controllers
{
    public class AccountController : Code.Base.BaseController
    {
        private readonly Services.User.UserAccountService userAccountService;
        private readonly Services.Locality.LocalityService localityService;

        public AccountController(Services.User.UserAccountService userAccountService, Services.Locality.LocalityService localityService,IMapper imapper)
            :base(imapper)
        {
            
            this.userAccountService = userAccountService;
            this.localityService = localityService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new Models.LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Models.LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            var user = userAccountService.Login(loginModel.Email, loginModel.Password);
            if(user == null)
            {
                loginModel.AreCredentialsInvalid = true;
                return View(loginModel);
            }

            await LogIn(user);
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await LogOut();
            return RedirectToAction("Index", "Home");
        }

        public async Task LogIn(Domain.Users user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(

                scheme: "SocializR",
                principal: principal); 
        }
        public async Task LogOut()
        {
            await HttpContext.SignOutAsync(scheme: "SocializR");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new Models.RegisterModel
            {
                Localities = GetLocalities()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(Models.RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Localities = GetLocalities();
                return View(model);
            }
            //var entity = mapper.Map<Domain.Users>(model);
            var entity = new Domain.Users()
            {
                Name = model.Name,
                Password = model.Password,
                Email = model.Email,
                LocalityId = model.LocalityId
            };
            var result = userAccountService.Register(entity);
            if (!result)
                return InternalServerErrorView();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult IsEmailAvailable(string Email)
        {
            var emailExists = userAccountService.EmailExists(Email);
            return Ok(!emailExists);
        }
        public List<SelectListItem> GetLocalities()
        {
            return localityService.getAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

        }
    }
}