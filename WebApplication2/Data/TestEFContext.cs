using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Data
{
    public class TestEFContext : DbContext
    {
        public DbSet<WebApplication2.Models.User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }
}
