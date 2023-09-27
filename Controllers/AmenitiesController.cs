using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirBnB.Data;
using AirBnB.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AirBnB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AmenitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AmenitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Amenities
        public async Task<IActionResult> Index()
        {
              return View(await _context.Amenities.ToListAsync());
        }

        // GET: Amenities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Amenities == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenities
                .FirstOrDefaultAsync(m => m.AmenityId == id);
            if (amenity == null)
            {
                return NotFound();
            }

            return View(amenity);
        }

        // GET: Amenities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Amenities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmenityId,AmenityName,AmenityType")] Amenity amenity , IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                var lastrow = _context.Amenities.OrderByDescending(u => u.AmenityId).FirstOrDefault();
                if (lastrow != null )
                {
                    int lastid = lastrow.AmenityId;
                    string fileName = (lastid + 1).ToString() + "." + imgFile.FileName.Split(".").Last();
                    amenity.AmenityImgSrc = fileName;
                    using (var fs = System.IO.File.Create("wwwroot/AmenityImg/" + fileName))
                    {
                        imgFile.CopyTo(fs);
                    }
                    _context.Add(amenity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    string fileName = amenity.AmenityId.ToString() + "." + imgFile.FileName.Split(".").Last();
                    amenity.AmenityImgSrc = fileName;
                    using (var fs = System.IO.File.Create("wwwroot/AmenityImg/" + fileName))
                    {
                        imgFile.CopyTo(fs);
                    }
                    _context.Add(amenity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(amenity);
        }

        // GET: Amenities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Amenities == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound();
            }
            return View(amenity);
        }

        // POST: Amenities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AmenityId,AmenityName,AmenityType")] Amenity amenity, IFormFile imgFile)
        {
            if (id != amenity.AmenityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imgFile != null)
                    {
                        string fileName = amenity.AmenityId.ToString() + "." + imgFile.FileName.Split(".").Last();
                        if (System.IO.File.Exists("wwwroot/AmenityImg/" + fileName))
                        {
                            System.IO.File.Delete("wwwroot/AmenityImg/" + fileName);
                        }
                        using (var fs = System.IO.File.Create("wwwroot/AmenityImg/" + fileName))
                        {
                            imgFile.CopyTo(fs);
                        }
                        amenity.AmenityImgSrc = fileName;
                    }
                    
                    _context.Update(amenity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmenityExists(amenity.AmenityId))
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
            return View(amenity);
        }

        // GET: Amenities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Amenities == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenities
                .FirstOrDefaultAsync(m => m.AmenityId == id);
            if (amenity == null)
            {
                return NotFound();
            }

            return View(amenity);
        }

        // POST: Amenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Amenities == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Amenities'  is null.");
            }
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity != null)
            {
                string fileName = amenity.AmenityImgSrc;
                if (System.IO.File.Exists("wwwroot/AmenityImg/" + fileName))
                {
                    System.IO.File.Delete("wwwroot/AmenityImg/" + fileName);
                }
                _context.Amenities.Remove(amenity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmenityExists(int id)
        {
          return _context.Amenities.Any(e => e.AmenityId == id);
        }
    }
}
