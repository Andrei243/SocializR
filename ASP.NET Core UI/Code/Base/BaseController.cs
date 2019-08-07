using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_UI.Code.Base
{
    public class BaseController : Controller
    {
        protected readonly IMapper mapper;
        public BaseController( IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IActionResult InternalServerErrorView()
        {
            return View("InternalServerError");
        }

        public IActionResult NotFoundView()
        {
            return View("NotFound");
        }

        public IActionResult ForbidView()
        {
            return View("Forbid");
        }

    }
}
