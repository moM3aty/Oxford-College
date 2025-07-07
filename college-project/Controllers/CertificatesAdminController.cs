using college_project.Data;
using college_project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace college_project.Controllers
{
    [Authorize]
    public class CertificatesAdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CertificatesAdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var certificates = from c in _context.Certificates
                               select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                certificates = certificates.Where(c => c.SerialNumber.Contains(searchString));
            }

            return View(await certificates.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Certificate certificate, IFormFile certificateImage)
        {
            ModelState.Remove(nameof(certificate.CertificateImagePath));

            if (await _context.Certificates.AnyAsync(c => c.SerialNumber == certificate.SerialNumber))
            {
                ModelState.AddModelError("SerialNumber", "This serial number already exists. Please enter a unique one.");
            }

            if (ModelState.IsValid)
            {
                if (certificateImage != null && certificateImage.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(certificateImage.FileName);
                    string certPath = Path.Combine(wwwRootPath, "uploads", "certificates");

                    if (!Directory.Exists(certPath))
                    {
                        Directory.CreateDirectory(certPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(certPath, fileName), FileMode.Create))
                    {
                        await certificateImage.CopyToAsync(fileStream);
                    }

                    certificate.CertificateImagePath = $"/uploads/certificates/{fileName}";
                }

                _context.Certificates.Add(certificate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Certificate added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(certificate);
        }




        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates.FirstOrDefaultAsync(m => m.Id == id);

            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Certificate deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
