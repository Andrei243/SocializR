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
            var localities = localityService.getAll().Select(e =>mapper.Map<Locality>(e));
            return View(localities);
        }     

        // GET: Localities/Create
        public IActionResult Create()
        {
            var model = new AddLocalityModel();
            model.CountyIds = countyService.GetAll().Select(e => mapper.Map<SelectListItem>(e)).ToList();
            return View(model);
        }

        // POST: Localities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddLocalityModel model)
        {
            if (ModelState.IsValid)
            {
                localityService.AddLocality(model.Name, model.CountyId);
                return RedirectToAction("Index","Localities");
            }

            model.CountyIds = countyService.GetAll().Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();

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
            model.CountyIds = countyService.GetAll().Select(e => { var item = mapper.Map<SelectListItem>(e); item.Selected = e.Id == locality.Id; return item; }).ToList();
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
            model.CountyIds = countyService.GetAll().Select(e => { var item = mapper.Map<SelectListItem>(e); item.Selected = e.Id == model.Id; return item; }).ToList();
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

        public JsonResult GetLocalities(int already)
        {
            var result= localityService.GetLocalities(already, PageSize).Select(e => mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Locality>(e)).ToList();
            return Json(result);
        }

    }
}
