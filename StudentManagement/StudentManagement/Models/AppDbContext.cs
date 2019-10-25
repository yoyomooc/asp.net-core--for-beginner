using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public  DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(
                e=>e.GetForeignKeys()             
                ))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;

            }

            modelBuilder.Seed();
        }
    }
}
