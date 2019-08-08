using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_UI.Code.Base;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_UI.Controllers
{
    [Authorize(Policy ="Admin")]
    public class AdminController : BaseController
    {
        public AdminController(IMapper mapper)
            :base(mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}