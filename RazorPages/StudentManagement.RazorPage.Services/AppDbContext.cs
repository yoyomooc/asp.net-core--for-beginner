using Microsoft.EntityFrameworkCore;
using StudentManagement.RazorPage.Models;

namespace StudentManagement.RazorPage.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

            public DbSet<Student> Students { get; set; }

    }
}