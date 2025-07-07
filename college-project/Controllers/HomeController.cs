using college_project.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace college_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult agencies()
        {
            return View();
        }
        public IActionResult major()
        {
            return View();
        }
        public IActionResult blog()
        {
            return View();
        }
        public IActionResult certificates()
        {
            return View();
        }

        [HttpGet("/Search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string serial)
        {
            if (string.IsNullOrEmpty(serial))
                return Json(new { success = false });

            var certificate = await _context.Certificates
                .Where(c => c.SerialNumber == serial)
                .FirstOrDefaultAsync();

            if (certificate == null)
                return Json(new { success = false });

            return Json(new
            {
                success = true,
                imagePath = certificate.CertificateImagePath 
            });
        }


    }
}
