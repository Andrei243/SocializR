using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Domain;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NET_Core_UI.Controllers
{
    
    [Authorize(Policy = "Admin")]

    public class CountiesController : Controller
    {
        //private readonly SocializRContext _context;
        private readonly Services.County.CountyService countyService;

        public CountiesController(Services.County.CountyService countyService)
        {
            this.countyService = countyService;
        }

        // GET: Counties
        public async Task<IActionResult> Index()
        {
            return View(countyService.GetAll());
        }

        // GET: Counties/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(county);
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
        public async Task<IActionResult> Create([Bind("Id,Name")] County county)
        {
            if (ModelState.IsValid)
            {
                countyService.Add(county);
                return RedirectToAction(nameof(Index));
            }
            return View(county);
        }

        // GET: Counties/Edit/5
        public  IActionResult Edit(int? id)
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
            return View(county);
        }

        // POST: Counties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] County county)
        {
            if (id != county.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    countyService.Update(county);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountyExists(county.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(county);
        }

        // GET: Counties/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(county);
        }

        // POST: Counties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var county = countyService.GetCountyById(id);
            countyService.Remove(county);
            return RedirectToAction(nameof(Index));
        }

        private bool CountyExists(int id)
        {
            return countyService.GetCountyById(id)!=null;
        }
    }
}
