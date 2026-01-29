using college_project.Models;
using Microsoft.EntityFrameworkCore;

namespace college_project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }
    }
}