using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Domain;
using ASP.NET_Core_UI.Models;
using System.IO;

namespace ASP.NET_Core_UI.Controllers
{
    public class PhotosController : Controller
    {
        private readonly SocializRContext _context;

        public PhotosController(SocializRContext context)
        {
            _context = context;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            var photos = _context.Photo.Include(p => p.Album).Include(p => p.Post).ToList();
            return View(photos);
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .Include(p => p.Album)
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Name");
            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id");
            return View();
        }

        public IActionResult Download(int id)
        {
            var photo = _context.Photo.Find(id);

            if (photo == null) return NotFound();
            return File(photo.Binar,"image/png");

        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( PhotoModel photoModel)
        {
            if (ModelState.IsValid)
            {

                //_context.Add(photo);
                Photo photo = new Photo()
                {
                    AlbumId = photoModel.AlbumId,
                    Position = photoModel.Position,
                    PostId = photoModel.PostId
                };
                using (var memoryStream = new MemoryStream())
                {
                    photoModel.Binar.CopyTo(memoryStream);
                    photo.Binar = memoryStream.ToArray();
                }
                _context.Add(photo);
                    await _context.SaveChangesAsync();
                //using (var memoryStream = new MemoryStream())
                //{
                //    await model.AvatarImage.CopyToAsync(memoryStream);
                //    user.AvatarImage = memoryStream.ToArray();
                //}
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Name", photoModel.AlbumId);
            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id", photoModel.PostId);
            return View(photoModel);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Name", photo.AlbumId);
            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id", photo.PostId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Binar,AlbumId,PostId,Position")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
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
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Name", photo.AlbumId);
            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id", photo.PostId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .Include(p => p.Album)
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photo.FindAsync(id);
            _context.Photo.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.Id == id);
        }
    }
}
