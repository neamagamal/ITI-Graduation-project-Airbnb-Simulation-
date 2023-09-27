using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirBnB.Data;
using AirBnB.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace AirBnB.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PropertiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Properties
        public async Task<IActionResult> Index()
        {
            var PropList = _context.Properties.Include(z => z.AppUser).Include(z => z.Area).ThenInclude(z => z.City).Include(z => z.Categoray).Include(z => z.PropertyImgs).Where(a=>a.Accepted==true);
         
            var categories = _context.Categoraies;
            ViewBag.cat = categories;
            return View(await PropList.ToListAsync());
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Properties == null)
            {
                return NotFound();
            }
            ViewBag.flag = true;
            if (TempData["x"]!=null)
            {
                ViewBag.flag = false;
            }
            //code to get the current user id
            ViewData["userid"] = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //code to get the selected property id
            ViewData["SelectedPropertyId"] = id;
            //code to get the current user id
            ViewData["userid"] = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["r"] = false;
            Booking z = null;
            var b = _context.Bookings.ToArray();
            for(int i = 0; i < b.Length; i++)
            {
                if (b[i].AppUserId == (string)ViewData["userid"] && b[i].PropertyId == (int)ViewData["SelectedPropertyId"] && DateTime.Today > b[i].CheckOutDate)
                {
                    ViewData["r"] = true;
                    z = b[i];
                }
            }
            ViewData["z"] = z;


            var SelectedProp = _context.Properties.FirstOrDefault(x => x.PropertyId == id);
            ViewData["PropertyCapacity"] = SelectedProp.PropertyCapacity;

            var @property = await _context.Properties
                .Include(z => z.AppUser)
                .Include(z => z.Categoray)
                .Include(q => q.Reviews)
                .Include(z => z.PropertyImgs)
                .Include(a => a.Amenities)
                .Include(z => z.Area)
                .ThenInclude(z => z.City)
                .FirstOrDefaultAsync(m => m.PropertyId == id);

            ViewBag.reviews = _context.Reviews;
            ViewBag.areas = _context.Areas;
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // GET: Properties/Create
        public IActionResult Create()
        {
            ViewBag.cat = _context.Categoraies;
            ViewData["userid"] = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["AreaId"] = new SelectList(_context.Areas, "AreaId", "AreaName");
            ViewData["CategorayId"] = new SelectList(_context.Categoraies, "CategorayId", "CategorayName");
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "AmenityId", "AmenityId");
            ViewBag.Amenity = _context.Amenities.ToList();
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PropertyId,PropertyTitle,PropertyDescription,PropertyCapacity,PropertyBedsNum,PropertyBedRooms,PropertyBath,PropertyPricePerNight,PropertyHostInfo,AppUserId,AreaId,CategorayId")] Property @property , IFormFile[] propImg , int[] amenity)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < amenity.Length; i++)
                {
                    var x = _context.Amenities.Find(amenity[i]);
                    @property.Amenities.Add(x);
                }
                _context.Add(@property);
                await _context.SaveChangesAsync();
            }
            if (propImg == null)
            {
                ModelState.AddModelError("", "no image uploaded , plz upload image");
            }
            else
            {

                for (int i = 0,count=0; i < propImg.Length && count<propImg.Length; i++,count++)
                {
                    string filename;
                    PropertyImg p = new PropertyImg();
                    PropertyImg x = _context.PropertyImgs.OrderByDescending(u => u.PropertyId).FirstOrDefault();

                    if (x != null)
                    {
                        filename = @property.PropertyTitle + (x.PropertyId +count).ToString() + "." + propImg[i].FileName.Split(".").Last();

                        p.ImgSrc = filename;
                        p.PropertyId = @property.PropertyId;
                        _context.PropertyImgs.Add(p);
                        await _context.SaveChangesAsync();

                        using (var fs = System.IO.File.Create("wwwroot/Images/" + filename))
                        {
                            propImg[i].CopyTo(fs);
                        }
                        
                    }
                    else
                    {
                        filename = i.ToString() + "." + propImg[i].FileName.Split(".").Last();

                        p.ImgSrc = filename;
                        p.PropertyId = @property.PropertyId;
                        _context.PropertyImgs.Add(p);
                        await _context.SaveChangesAsync();
                        using (var fs = System.IO.File.Create("wwwroot/Images/" + filename))
                        {
                            propImg[i].CopyTo(fs);
                        }
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", @property.AppUserId);
            ViewData["AreaId"] = new SelectList(_context.Areas, "AreaId", "AreaName", @property.AreaId);
            ViewData["CategorayId"] = new SelectList(_context.Categoraies, "CategorayId", "CategorayName", @property.CategorayId);
            return View(@property);
        }

        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Properties == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", @property.AppUserId);
            ViewData["AreaId"] = new SelectList(_context.Areas, "AreaId", "AreaName", @property.AreaId);
            ViewData["CategorayId"] = new SelectList(_context.Categoraies, "CategorayId", "CategorayName", @property.CategorayId);
            return View(@property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PropertyId,PropertyTitle,PropertyDescription,PropertyCapacity,PropertyBedsNum,PropertyBedRooms,PropertyBath,PropertyPricePerNight,PropertyHostInfo,AppUserId,AreaId,CategorayId")] Property @property)
        {
            if (id != @property.PropertyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(@property.PropertyId))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", @property.AppUserId);
            ViewData["AreaId"] = new SelectList(_context.Areas, "AreaId", "AreaName", @property.AreaId);
            ViewData["CategorayId"] = new SelectList(_context.Categoraies, "CategorayId", "CategorayName", @property.CategorayId);
            return View(@property);
        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Properties == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .Include(z => z.AppUser)
                .Include(z => z.Area)
                .Include(z => z.Categoray)
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: AdminProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Properties == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Properties'  is null.");
            }
            var @property = await _context.Properties.FindAsync(id);
            var propImgs = _context.PropertyImgs.Select(a => a).Where(a => a.PropertyId == id);
            if (@property != null && propImgs != null)
            {
                foreach (var item in propImgs)
                {
                    if (System.IO.File.Exists("wwwroot/Images/" + item.ImgSrc))
                    {
                        System.IO.File.Delete("wwwroot/Images/" + item.ImgSrc);
                    }
                    _context.PropertyImgs.Remove(item);
                }
                _context.Properties.Remove(@property);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //to show prop in area
        public IActionResult ShowProp(int id)
        {
            var PropList = _context.Properties.Include(z => z.AppUser).Include(z => z.Area).ThenInclude(z => z.City).Include(z => z.Categoray).Include(z => z.PropertyImgs).Where(a=>a.AreaId==id && a.Accepted==true);

            var categories = _context.Categoraies;
            ViewBag.cat = categories;
            return View(PropList.ToList());
        }
        private bool PropertyExists(int id)
        {
          return _context.Properties.Any(e => e.PropertyId == id);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var applicationDbContext = _context.Properties.Include(a => a.AppUser).Include(a => a.Area).Include(a => a.Categoray).Where(a=>a.Accepted==false);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize (Roles ="Admin")]
        public async Task<IActionResult> AdminDetails(int? id)
        {
            if (id == null || _context.Properties == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .Include(a => a.AppUser)
                .Include(a => a.Area)
                .Include(a => a.Categoray)
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        public IActionResult Confirm(int id)
        {
            var prop = _context.Properties.Find(id);
            prop.Accepted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(AdminIndex));
        }
        public IActionResult Reject(int id)
        {
            var prop = _context.Properties.Find(id);
            _context.Properties.Remove(prop);
            _context.SaveChanges();
            return RedirectToAction(nameof(AdminIndex));
        }
    }
}
