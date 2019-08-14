using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Domain;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace ASP.NET_Core_UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InterestsController : Code.Base.BaseController
    {
        //private readonly SocializRContext _context;
        private readonly SocializRUnitOfWork unitOfWork;

        public InterestsController(SocializRContext context,IMapper mapper,SocializRUnitOfWork unitOfWork):
            base(mapper)
        {
            //_context = context;
            this.unitOfWork = unitOfWork;
        }

        // GET: Interests
        public async Task<IActionResult> Index()
        {
            return View(await unitOfWork.Interests.Query.ToListAsync());
        }

        // GET: Interests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interest = await unitOfWork.Interests.Query
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interest == null)
            {
                return NotFound();
            }

            return View(interest);
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
        public IActionResult Create([Bind("Id,Name")] Interest interest)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Interests.Add(interest);
                unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(interest);
        }

        // GET: Interests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interest = unitOfWork.Interests.Query.FirstOrDefault(e=>e.Id==id);
            if (interest == null)
            {
                return NotFound();
            }
            return View(interest);
        }

        // POST: Interests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Interest interest)
        {
            if (id != interest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Interests.Update(interest);
                    unitOfWork.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InterestExists(interest.Id))
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
            return View(interest);
        }

        // GET: Interests/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interest = unitOfWork.Interests.Query
                .FirstOrDefault(m => m.Id == id);
            if (interest == null)
            {
                return NotFound();
            }

            return View(interest);
        }

        // POST: Interests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var interest = unitOfWork.Interests.Query.FirstOrDefault(e=>e.Id==id);
            unitOfWork.Interests.Remove(interest);
            unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool InterestExists(int id)
        {
            return unitOfWork.Interests.Query.Any(e => e.Id == id);
        }
    }
}
