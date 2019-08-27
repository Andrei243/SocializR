using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ASP.NET_Core_UI.Models.DomainModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        // GET: Interests
        public IActionResult Index()
        {
            var interese = interestService.getAll().Select(e =>mapper.Map<Interest>(e));

            return View(interese);
        }


        // GET: Interests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Interests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( Interest interest)
        {
            if (ModelState.IsValid)
            {
                interestService.AddInterest(interest.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(interest);
        }

        // GET: Interests/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interest = interestService.GetInterest(id.Value);
           
            var model = mapper.Map<Interest>(interest);
            return View(model);
        }

        // POST: Interests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( Interest interest)
        {
            if (ModelState.IsValid)
            {

                interestService.EditInterest(interest.Id, interest.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(interest);
        }

        // GET: Interests/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            interestService.RemoveInterest(id.Value);

            return RedirectToAction("Index", "Interests");
        }

       public JsonResult GetInterests(int already)
        {
            var interests = interestService.GetInterests(already, PageSize).Select(e => mapper.Map<ASP.NET_Core_UI.Models.JsonModels.Interest>(e)).ToList();
            return Json(interests);
        }

    }
}
