using System.Diagnostics;
using CustomeHangfireClean.Data;
using CustomeHangfireClean.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomeHangfireClean.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var counter = _db.JobCounters.FirstOrDefault();
            ViewData["Counter"] = counter?.Number ?? 0;
            ViewData["Square"] = counter?.Square ?? 0;
            ViewData["NewNumber"] = counter?.NewNumber ?? 0;
            return View();
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
