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
using ASP.NET_Core_UI.Models.DomainEntities;

namespace ASP.NET_Core_UI.Controllers
{
    [Authorize(Policy ="Admin")]
    public class LocalitiesController : Controller
    {
        private readonly Services.Locality.LocalityService localityService;
        private readonly Services.County.CountyService countyService;
        public LocalitiesController(
            Services.Locality.LocalityService localityService,
            Services.County.CountyService countyService
            )
        {
            this.localityService = localityService;
            this.countyService = countyService;
        }

        // GET: Localities
        public IActionResult Index()
        {
            var localities = localityService.getAll().Select(e => new Locality
            {
                County = new County { Name = e.County.Name, Id = e.County.Id },
                Id = e.Id,
                Name = e.Name

            });
            return View(localities);
        }     

        // GET: Localities/Create
        public IActionResult Create()
        {
            AddLocalityModel model = new AddLocalityModel();
            model.CountyIds = countyService.GetAll().Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();
            return View(model);
        }

        // POST: Localities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            EditLocalityModel model = new EditLocalityModel()
            {
                CountyId = locality.CountyId,
                CountyIds = countyService.GetAll().Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name,Selected=e.Id==locality.CountyId }).ToList(),
                Id = locality.Id,
                Name = locality.Name
            };
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
            model.CountyIds = countyService.GetAll().Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name, Selected = e.Id == model.CountyId }).ToList();
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


    }
}
