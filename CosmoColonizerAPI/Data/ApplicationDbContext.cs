using CosmoColonizerAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CosmoColonizerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Planet> Planets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for the three planets
            modelBuilder.Entity<Planet>().HasData(
                new Planet
                {
                    Id = 1,
                    Name = "Terra Nova",
                },
                new Planet
                {
                    Id = 2,
                    Name = "Luna Magna",
                },
                new Planet
                {
                    Id = 3,
                    Name = "Solara",
                }
            );
        }
    }
}
