using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public class CourseContext : DbContext
    {
        public CourseContext(DbContextOptions<CourseContext> options) : base(options) { }

        public DbSet<API.Models.Category> Categories { get; set; } = default!;
        public DbSet<API.Models.Role> Roles { get; set; } = default!;
        public DbSet<API.Models.User> Users { get; set; } = default!;
        public DbSet<API.Models.Course> Courses { get; set; } = default!;
        public DbSet<API.Models.Comment> Comment { get; set; } = default!;
        public DbSet<API.Models.Image> Image { get; set; } = default!;
        public DbSet<API.Models.Country> Country { get; set; } = default!;
        public DbSet<API.Models.Favorite> Favorite { get; set; } = default!;

        public CourseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Kyrs;Username=postgres;Password=1234");
        }
    }
}
