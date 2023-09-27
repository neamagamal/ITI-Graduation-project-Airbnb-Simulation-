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
    public class AreasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AreasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Areas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Areas.Include(a => a.City);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Areas == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .Include(a => a.City)
                .FirstOrDefaultAsync(m => m.AreaId == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // GET: Areas/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AreaName,CityId")] Area area,IFormFile imgFile )
        {
            if (ModelState.IsValid)
            {
                var lastrow = _context.Areas.OrderByDescending(u => u.AreaId).FirstOrDefault();
                if (lastrow != null)
                {
                    int lastid = lastrow.AreaId;
                    string fileName = (lastid + 1).ToString() + "." + imgFile.FileName.Split(".").Last();
                    area.AreaImg = fileName;
                    using (var fs = System.IO.File.Create("wwwroot/AreaImgs/" + fileName))
                    {
                        imgFile.CopyTo(fs);
                    }
                    _context.Add(area);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    string fileName = area.AreaId.ToString() + "." + imgFile.FileName.Split(".").Last();
                    area.AreaImg = fileName;
                    using (var fs = System.IO.File.Create("wwwroot/AreaImgs/" + fileName))
                    {
                        imgFile.CopyTo(fs);
                    }
                    _context.Add(area);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", area.CityId);
            return View(area);
        }

        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Areas == null)
            {
                return NotFound();
            }

            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", area.CityId);
            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AreaId,AreaName,CityId")] Area area,IFormFile imgFile)
        {
            if (id != area.AreaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imgFile != null)
                    {
                        string fileName = area.AreaId.ToString() + "." + imgFile.FileName.Split(".").Last();
                        if (System.IO.File.Exists("wwwroot/AreaImgs/" + fileName))
                        {
                            System.IO.File.Delete("wwwroot/AreaImgs/" + fileName);
                        }
                        using (var fs = System.IO.File.Create("wwwroot/AreaImgs/" + fileName))
                        {
                            imgFile.CopyTo(fs);
                        }
                        area.AreaImg = fileName;
                    }
                    _context.Update(area);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.AreaId))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", area.CityId);
            return View(area);
        }

        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Areas == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .Include(a => a.City)
                .FirstOrDefaultAsync(m => m.AreaId == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Areas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Areas'  is null.");
            }
            var area = await _context.Areas.FindAsync(id);
            if (area != null)
            {
                string fileName = area.AreaImg;
                if (System.IO.File.Exists("wwwroot/AreaImgs/" + fileName))
                {
                    System.IO.File.Delete("wwwroot/AreaImgs/" + fileName);
                }
                _context.Areas.Remove(area);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
          return _context.Areas.Any(e => e.AreaId == id);
        }
    }
}
