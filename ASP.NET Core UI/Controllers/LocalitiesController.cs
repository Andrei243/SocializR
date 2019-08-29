using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using ASP.NET_Core_UI.Models;
using ASP.NET_Core_UI.Models.DomainModels;
using ASP.NET_Core_UI.Models.AdminModels;
using ASP.NET_Core_UI.Code.Base;
using AutoMapper;
using ASP.NET_Core_UI.Models.JsonModels;

namespace ASP.NET_Core_UI.Controllers
{
    [Authorize(Policy ="Admin")]
    public class LocalitiesController : BaseController
    {
        private readonly Services.Locality.LocalityService localityService;
        private readonly Services.County.CountyService countyService;
       
        public LocalitiesController(
            Services.Locality.LocalityService localityService,
            Services.County.CountyService countyService,
            IMapper mapper
            ):
            base(mapper)
        {
            this.localityService = localityService;
            this.countyService = countyService;
        }

        // GET: Localities
        public IActionResult Index()
        {
            var localities = localityService.GetAll().Select(e =>mapper.Map<LocalityDomainModel>(e));
            return View(localities);
        }

        // GET: Localities/Create
        public IActionResult Create()
        {
            var model = new AddLocalityModel()
            {
                CountyIds = countyService.GetAll().Select(e => mapper.Map<SelectListItem>(e)).ToList()
            };
            return View(model);
        }

        // POST: Localities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddLocalityModel model)
        {
            if (localityService.CityAlreadyExistsInCounty(model.Name, model.CountyId))
            {
                ModelState.AddModelError(nameof(model.Name), "Locality already exists");
            }
            if (ModelState.IsValid)
            {
                localityService.AddLocality(model.Name, model.CountyId);
                return RedirectToAction("Index","Localities");
            }

            model.CountyIds = countyService.GetAll().Select(e => mapper.Map<SelectListItem>(e)).ToList();

            return View(model);
        }

        // GET: Localities/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locality = localityService.GetLocality(id.Value);
            if (locality == null)
            {
                return NotFound();
            }
            
            var model = mapper.Map<EditLocalityModel>(locality);
            model.CountyIds = countyService.GetAll().Select(e => 
            { var item = mapper.Map<SelectListItem>(e);
                item.Selected = e.Id == locality.Id;
                return item; })
                .ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditLocalityModel model)
        {
            
            if (ModelState.IsValid)
            {
                localityService.EditLocality(model.Id, model.Name, model.CountyId);
                return RedirectToAction(nameof(Index));
            }
            model.CountyIds = countyService.GetAll().Select(e => {
                var item = mapper.Map<SelectListItem>(e);
                item.Selected = e.Id == model.Id;
                return item; })
                .ToList();
            return View(model);
        }

        // GET: Localities/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            localityService.RemoveLocality(id.Value);

            return RedirectToAction("Index");
        }

        public JsonResult GetLocalities(int toSkip)
        {
            var result= localityService
                .GetLocalities(toSkip, PageSize)
                .Select(e => mapper.Map<LocalityJsonModel>(e))
                .ToList();
            return Json(result);
        }

    }
}
