using AirBnB.Data;
using AirBnB.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AirBnB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
           List<City> Cities=_context.Cities.ToList();
            return View(Cities);
        }

        [HttpPost]
        public IActionResult ShowArea(int cityid)
        {
            City city = _context.Cities.FirstOrDefault(a => a.CityId == cityid);
            ViewBag.CityName = city.CityName;
            List<Area>Areas=_context.Areas.Select(a=>a).Where(a=>a.CityId==cityid).ToList();
            return View(Areas);
        }
        [HttpGet]
        public IActionResult ShowAreaGet(int id)
        {
            City city = _context.Cities.FirstOrDefault(a =>a.CityId==id);
            ViewBag.CityName = city.CityName;
            List<Area> Areas = _context.Areas.Select(a => a).Where(a => a.CityId == id).ToList();
            return View("ShowArea",Areas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}