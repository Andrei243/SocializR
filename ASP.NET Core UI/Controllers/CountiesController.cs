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
using ASP.NET_Core_UI.Models.JsonModels;

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

        [HttpGet]
        public IActionResult Index()
        {
            var counties = countyService.GetAll().Select(e => mapper.Map<CountyDomainModel>(e));
            return View(counties);
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public IActionResult Create(CountyDomainModel model)
        {
            if (ModelState.IsValid)
            {
                countyService.Add(model.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
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

        [HttpPost]
        public IActionResult Edit(EditCountyModel model)
        {
            

            if (ModelState.IsValid)
            {
                countyService.Update(model.Id, model.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public bool CanDelete(int? countyId)
        {
            if (countyId == null)
            {
                return true;
            }
            var canBeDeleted = countyService.CanBeDeleted(countyId.Value);
            if (canBeDeleted)
            {
                countyService.Remove(countyId.Value);
            }
            return canBeDeleted;
        }

       
        [HttpGet]
        public JsonResult GetCounties(int toSkip)
        {
            var counties = countyService.GetCounties(toSkip, PageSize).Select(e => mapper.Map<CountyJsonModel>(e)).ToList();

            return Json(counties);

        }

    }
}
