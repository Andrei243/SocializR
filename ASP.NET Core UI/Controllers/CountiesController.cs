using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using ASP.NET_Core_UI.Models.DomainModels;
using ASP.NET_Core_UI.Models.AdminModels;
using ASP.NET_Core_UI.Code.Base;
using AutoMapper;

namespace ASP.NET_Core_UI.Controllers
{
    
    [Authorize(Policy = "Admin")]

    public class CountiesController : BaseController
    {
        private readonly Services.County.CountyService countyService;
        private readonly Services.Locality.LocalityService localityService;
        

        public CountiesController(Services.County.CountyService countyService,Services.Locality.LocalityService localityService,IMapper mapper):
            base(mapper)
        {
            this.countyService = countyService;
            this.localityService = localityService;
        }

        // GET: Counties
        public IActionResult Index()
        {
            return View(countyService.GetAll().Select(e=>new County() {Id=e.Id,Name=e.Name }));
        }

        // GET: Counties/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var county = countyService.GetCountyById(id);
            if (county == null)
            {
                return NotFound();
            }
           
            var model = mapper.Map<DetailsCountyModel>(county);
            model.Localities = localityService.GetAll(county.Id).Select(e => e.Name).ToList();
            return View(model);
        }

        // GET: Counties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Counties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(County model)
        {
            if (ModelState.IsValid)
            {
                countyService.Add(model.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Counties/Edit/5
        public  IActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var county = countyService.GetCountyById(id);
            
            var model = mapper.Map<EditCountyModel>(county);
            if (county == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Counties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCountyModel model)
        {
            

            if (ModelState.IsValid)
            {
                countyService.Update(model.Id, model.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public bool CanDelete(int? countyId)
        {
            if (countyId == null)
            {
                return true;
            }
            bool x = countyService.CanBeDeleted(countyId.Value);
            if (x)
            {
                countyService.Remove(countyId.Value);
            }
            return x;
        }

        // GET: Counties/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //countyService.Remove(id.Value);

            return RedirectToAction("Index", "Counties");
        }

        public JsonResult GetCounties(int toSkip)
        {
            var counties = countyService.GetCounties(toSkip, PageSize).Select(e => mapper.Map<ASP.NET_Core_UI.Models.JsonModels.County>(e)).ToList();

            return Json(counties);

        }

    }
}
