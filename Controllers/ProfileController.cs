using AirBnB.Data;
using AirBnB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net;

namespace AirBnB.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index(int? id)
        //{
        //    if (id == null || _context.Properties.FindAsync(id) == null)
        //    {
        //        return Content("user does not exist.");
        //    }

        //    var userData = await _context.Properties.Include(x => x.AppUser).FirstOrDefaultAsync(a => a.PropertyId == id);
        //    return View(userData);
        //}

        public async Task<IActionResult> showProfile(int? id)
        {
            if (id == null ||await _context.Properties.FindAsync(id) == null)
            {
                return NotFound();
            }

            var userData = await _context.Properties.Include(x => x.AppUser).FirstOrDefaultAsync(a => a.PropertyId == id);


            byte[] byteArray = userData.AppUser.ProfilePicture;
            string base64String = Convert.ToBase64String(byteArray);
            ViewBag.proImg = base64String;

            List<Property> p1 = _context.Properties.Include(x => x.PropertyImgs).Select(a=>a).Where(a => a.AppUserId == userData.AppUserId).ToList();    //find the props. of that user
            ViewData["propList"] = p1;      //add list of properties(of this user) to verwData to show them in the view
            return View("Index", userData);
        }
    }
}
