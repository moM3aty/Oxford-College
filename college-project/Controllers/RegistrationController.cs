using college_project.Data;
using college_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace college_project.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegistrationController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.AgreeToCharter)
                {
                    ModelState.AddModelError("AgreeToCharter", "You must agree to the Transparency Charter.");
                    return View(model);
                }

                var student = new StudentRegistration
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Major = model.Major,
                    IsTransparencyCharterAgreed = true,
                    RegistrationDate = DateTime.Now
                };

                student.QualificationImagePath = await UploadFile(model.QualificationImage);
                student.IdentityImagePath = await UploadFile(model.IdentityImage);
                student.PersonalPhotoPath = await UploadFile(model.PersonalPhoto);

                _context.StudentRegistrations.Add(student);
                await _context.SaveChangesAsync();

                return RedirectToAction("Success", new { id = student.Id });
            }
            return View(model);
        }

        public async Task<IActionResult> Success(int id)
        {
            var student = await _context.StudentRegistrations.FindAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var students = await _context.StudentRegistrations.OrderByDescending(s => s.RegistrationDate).ToListAsync();
            return View(students);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.StudentRegistrations.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.StudentRegistrations.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.StudentRegistrations.FindAsync(id);
            if (student != null)
            {
                DeleteFile(student.QualificationImagePath);
                DeleteFile(student.IdentityImagePath);
                DeleteFile(student.PersonalPhotoPath);
                _context.StudentRegistrations.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> ExportToCSV()
        {
            var students = await _context.StudentRegistrations.ToListAsync();
            var builder = new StringBuilder();
            builder.AppendLine("ID,FullName,PhoneNumber,Major,RegistrationDate");

            foreach (var student in students)
            {
                builder.AppendLine($"{student.Id},{EscapeCsv(student.FullName)},{EscapeCsv(student.PhoneNumber)},{EscapeCsv(student.Major)},{student.RegistrationDate}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Students_List.csv");
        }

        private string EscapeCsv(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }
            return field;
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string uploadPath = Path.Combine(wwwRootPath, "uploads", "students");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"/uploads/students/{fileName}";
        }

        private void DeleteFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                }
                catch { }
            }
        }
    }
}