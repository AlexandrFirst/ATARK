using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.DataContext
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Point> Points { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate();
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}