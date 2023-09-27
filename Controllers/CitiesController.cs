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

// Changes by ahmed
namespace AirBnB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
              return View(await _context.Cities.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.Include(z => z.Areas)
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,CityName")] City city, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                var lastrow = _context.Cities.OrderByDescending(u => u.CityId).FirstOrDefault();
                if (lastrow != null)
                {
                    int lastid = lastrow.CityId;
                    string fileName = (lastid+1).ToString() + "." + imgFile.FileName.Split(".").Last();
                    city.CityImg = fileName;
                    using (var fs = System.IO.File.Create("wwwroot/CityImgs/" + fileName))
                    {
                        imgFile.CopyTo(fs);
                    }
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    string fileName = city.CityId.ToString() + "." + imgFile.FileName.Split(".").Last();
                    city.CityImg = fileName;
                    using (var fs = System.IO.File.Create("wwwroot/CityImgs/" + fileName))
                    {
                        imgFile.CopyTo(fs);
                    }
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CityId,CityName")] City city,IFormFile imgFile)
        {
            if (id != city.CityId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (imgFile != null)
                    {
                        string fileName = city.CityId.ToString() + "." + imgFile.FileName.Split(".").Last();
                        if (System.IO.File.Exists("wwwroot/CityImgs/" + fileName))
                        {
                            System.IO.File.Delete("wwwroot/CityImgs/" + fileName);
                        }
                        using (var fs = System.IO.File.Create("wwwroot/CityImgs/" + fileName))
                        {
                            imgFile.CopyTo(fs);
                        }
                        city.CityImg = fileName;
                    }
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityId))
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
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cities == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cities'  is null.");
            }
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                string fileName = city.CityImg;
                if (System.IO.File.Exists("wwwroot/CityImgs/" + fileName))
                {
                    System.IO.File.Delete("wwwroot/CityImgs/" + fileName);
                }
                _context.Cities.Remove(city);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
          return _context.Cities.Any(e => e.CityId == id);
        }
    }
}
