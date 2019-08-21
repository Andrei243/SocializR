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
using ASP.NET_Core_UI.Models.GeneralModels;

namespace ASP.NET_Core_UI.Controllers
{
    public class AccountController : Code.Base.BaseController
    {
        private readonly Services.User.UserAccountService userAccountService;
        private readonly Services.County.CountyService countyService;

        public AccountController(Services.County.CountyService countyService, Services.User.UserAccountService userAccountService,IMapper imapper)
            :base(imapper)
        {
            this.countyService = countyService;
            this.userAccountService = userAccountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
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
            var model = new RegisterModel
            {
                Counties=GetCounties()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var entity = mapper.Map<Domain.Users>(model);
           
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

        public List<SelectListItem> GetCounties()
        {
            return countyService.GetAll().Select(c =>mapper.Map<SelectListItem>(c)).ToList();
        }

        [HttpGet]
        public List<SelectListItem> GetLocalities(int CountyId)
        {
            return countyService.GetLocalities(CountyId).Select(e=> mapper.Map<SelectListItem>(e)).ToList();
        }
    }
}