﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ASP.NET_Core_UI.Models.DomainModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASP.NET_Core_UI.Models.JsonModels;

namespace ASP.NET_Core_UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InterestsController : Code.Base.BaseController
    {

        private readonly Services.Interest.InterestService interestService;
        public InterestsController(IMapper mapper,Services.Interest.InterestService interestService):
            base(mapper)
        {
            this.interestService = interestService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var interese = interestService.GetAll().Select(e =>mapper.Map<InterestDomainModel>(e));

            return View(interese);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Create( InterestDomainModel interest)
        {
            if (ModelState.IsValid)
            {
                interestService.AddInterest(interest.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(interest);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interest = interestService.GetInterest(id.Value);
            if (interest == null)
            {
                return NotFound();
            }
           
            var model = mapper.Map<InterestDomainModel>(interest);
            return View(model);
        }

        
        [HttpPost]
        public IActionResult Edit( InterestDomainModel interest)
        {
            if (ModelState.IsValid)
            {

                interestService.EditInterest(interest.Id, interest.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(interest);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            interestService.RemoveInterest(id.Value);

            return RedirectToAction("Index", "Interests");
        }

        [HttpGet]
       public JsonResult GetInterests(int toSkip)
        {
            var interests = interestService.GetInterests(toSkip, PageSize).Select(e => mapper.Map<InterestJsonModel>(e)).ToList();
            return Json(interests);
        }

    }
}
