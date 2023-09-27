using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirBnB.Data;
using AirBnB.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace AirBnB.Controllers
{
    public class CategoraysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoraysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categorays
        public async Task<IActionResult> Index()
        {
              return View(await _context.Categoraies.ToListAsync());
        }

        public async Task<IActionResult> ShowProp(int id)
        {
            var PropList = _context.Properties.Include(z => z.AppUser).Include(z => z.Area).ThenInclude(z => z.City).Include(z => z.Categoray).Include(z => z.PropertyImgs).Where(a => a.CategorayId == id &&a.Accepted==true);
            var categories = _context.Categoraies;
            ViewBag.cat = categories;
            return View(PropList);
        }

        // GET: Categorays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categoraies == null)
            {
                return NotFound();
            }

            var categoray = await _context.Categoraies.Include(D => D.Properties).ThenInclude(z =>z.Area).ThenInclude(z =>z.City)
                .FirstOrDefaultAsync(m => m.CategorayId == id);
            ViewBag.catphotos = await _context.Categoraies.Include(z => z.Properties).ThenInclude(z => z.PropertyImgs).FirstOrDefaultAsync(m => m.CategorayId == id);
            if (categoray == null)
            {
                return NotFound();
            }

            return View(categoray);
        }

        // GET: Categorays/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }



        // POST: Categorays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("CategorayId,CategorayName")] Categoray categoray, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                
                var lastrow = _context.Categoraies.OrderByDescending(u => u.CategorayId).FirstOrDefault();
                if (lastrow != null)
                {
                    int lastid = lastrow.CategorayId;
                    string fileName = (lastid + 1).ToString() + "." + imgFile.FileName.Split(".").Last();
                    categoray.Categoryphotosrc = fileName;
                    using (var fs = System.IO.File.Create("wwwroot/CategoryImgs/" + fileName))
                    {
                        imgFile.CopyTo(fs);
                    }
                    _context.Add(categoray);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    string fileName = categoray.CategorayId.ToString() + "." + imgFile.FileName.Split(".").Last();
                    categoray.Categoryphotosrc = fileName;
                    using (var fs = System.IO.File.Create("wwwroot/CategoryImgs/" + fileName))
                    {
                        imgFile.CopyTo(fs);
                    }
                    _context.Add(categoray);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(categoray);
        }


        // GET: Categorays/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categoraies == null)
            {
                return NotFound();
            }

            var categoray = await _context.Categoraies.FindAsync(id);
            if (categoray == null)
            {
                return NotFound();
            }
            return View(categoray);
        }

        // POST: Categorays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("CategorayId,CategorayName")] Categoray categoray, IFormFile imgFile)
        {
            if (id != categoray.CategorayId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imgFile != null)
                    {
                        string fileName = categoray.CategorayId.ToString() + "." + imgFile.FileName.Split(".").Last();
                        if (System.IO.File.Exists("wwwroot/CategoryImgs/" + fileName))
                        {
                            System.IO.File.Delete("wwwroot/CategoryImgs/" + fileName);
                        }
                        using (var fs = System.IO.File.Create("wwwroot/CategoryImgs/" + fileName))
                        {
                            imgFile.CopyTo(fs);
                        }
                        categoray.Categoryphotosrc = fileName;
                    }
                    _context.Update(categoray);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorayExists(categoray.CategorayId))
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
            return View(categoray);
        }

        // GET: Categorays/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categoraies == null)
            {
                return NotFound();
            }

            var categoray = await _context.Categoraies
                .FirstOrDefaultAsync(m => m.CategorayId == id);
            if (categoray == null)
            {
                return NotFound();
            }

            return View(categoray);
        }

        // POST: Categorays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categoraies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categoraies'  is null.");
            }
            var categoray = await _context.Categoraies.FindAsync(id);
            if (categoray != null)
            {
                string fileName = categoray.Categoryphotosrc;
                if (System.IO.File.Exists("wwwroot/CategorayImgs/" + fileName))
                {
                    System.IO.File.Delete("wwwroot/CategorayImgs/" + fileName);
                }
                _context.Categoraies.Remove(categoray);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategorayExists(int id)
        {
          return _context.Categoraies.Any(e => e.CategorayId == id);
        }
    }
}
