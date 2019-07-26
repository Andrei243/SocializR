using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_UI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Domain.Users user)
        {
            var ctx = new DataAccess.SocializRContext();
            var userDupaEmail = ctx.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if(userDupaEmail.Password == user.Password)
            {
                Debug.WriteLine("Succes");
                return Ok();
            }
            else
            {
                Debug.WriteLine("Fail");
                return Ok();
            }

        }
    }
}