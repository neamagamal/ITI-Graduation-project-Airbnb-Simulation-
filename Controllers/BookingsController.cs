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


namespace AirBnB.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.AppUser).Include(b => b.Property);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.AppUser)
                .Include(b => b.Property)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create(int? id)
        {
            ViewBag.flag = null;
            if (TempData.ContainsKey("f")){
                ViewBag.flag = true;
            }
           
            //code to get the selected property id
            ViewData["SelectedPropertyId"] = id;
            if (TempData.ContainsKey("propid"))
            {
                ViewBag.flag = false;
                ViewData["SelectedPropertyId"] = TempData["propid"];
            }
              
               
            //code to get the current user id
            ViewData["userid"] =  this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var SelectedProp = _context.Properties.FirstOrDefault(x => x.PropertyId == id);
            ViewData["PropertyCapacity"] = SelectedProp.PropertyCapacity;
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "AppUserId");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,AppUserId,PropertyId,CheckInDate,CheckOutDate,BookingCapacity")] Booking booking)
        {

            //var idtest = _context.Bookings.FirstOrDefault(a => a.PropertyId == booking.PropertyId);

            //var NotAvalible = _context.Bookings.Where(a => a.PropertyId == booking.PropertyId).Select(a => (a.CheckInDate <= booking.CheckInDate && a.CheckOutDate >= booking.CheckInDate) && (a.CheckInDate <= booking.CheckOutDate && a.CheckOutDate >= booking.CheckOutDate)).FirstOrDefault();
            var valid=_context.Bookings.ToArray();
            
            Booking book = null;
            for(int i = 0; i < valid.Length ; i++)
            {
                if (booking.PropertyId  == valid[i].PropertyId  && ((booking.CheckInDate >= valid[i].CheckInDate && booking.CheckOutDate <= valid[i].CheckOutDate )|| ( booking.CheckInDate <= valid[i].CheckInDate && booking.CheckOutDate  >= valid[i].CheckInDate && booking.CheckOutDate <= valid[i].CheckOutDate) ||(booking.CheckInDate >= valid[i].CheckInDate && booking.CheckInDate <= valid[i].CheckOutDate && booking.CheckOutDate >= valid[i].CheckOutDate) || (booking.CheckInDate <= valid[i].CheckInDate && booking.CheckOutDate >= valid[i].CheckOutDate) ))
                {
                    book = valid[i];
                    break;
                }

            }
          
            if (ModelState.IsValid  )
            {
                if (book == null)
                {
                    TempData["f"] = true;
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                else {
                   
                    int id = booking.PropertyId;
                    var SelectedProp = _context.Properties.FirstOrDefault(x => x.PropertyId == id);
                    ViewData["PropertyCapacity"] = SelectedProp.PropertyCapacity;
                    TempData["propid"] = id;
                    return RedirectToAction(nameof(Create));
                }
            }
            
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", booking.AppUserId);
            ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "AppUserId", booking.PropertyId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", booking.AppUserId);
            ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "AppUserId", booking.PropertyId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,AppUserId,PropertyId,CheckInDate,CheckOutDate,BookingCapacity")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", booking.AppUserId);
            ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "AppUserId", booking.PropertyId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.AppUser)
                .Include(b => b.Property)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
