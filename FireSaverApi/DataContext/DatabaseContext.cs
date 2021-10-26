using FireSaverApi.DataContext.DataConfiguration;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.DataContext
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate();
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BuildingConfiguration());

            modelBuilder.ApplyConfiguration(new CompartmentConfiguration<Floor>());
            modelBuilder.ApplyConfiguration(new CompartmentConfiguration<Room>());
            modelBuilder.ApplyConfiguration(new FloorConfiguration());
        }
    }
}