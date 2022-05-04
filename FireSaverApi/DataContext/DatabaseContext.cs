using FireSaverApi.DataContext.DataConfiguration;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.DataContext
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<ScalePoint> ScalePoints { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<EvacuationPlan> EvacuationPlans { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<IoT> IoTs { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<ScaleModel> ScaleModels { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Compartment> Compartment { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate();
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuildingConfiguration());
            modelBuilder.ApplyConfiguration(new CompartmentConfiguration<Room>());
            modelBuilder.ApplyConfiguration(new CompartmentConfiguration<Floor>());
            modelBuilder.ApplyConfiguration(new EvacuationPlanConfiguration());
            modelBuilder.ApplyConfiguration(new FloorConfiguration());
            modelBuilder.ApplyConfiguration(new IoTConfiguration());
            modelBuilder.ApplyConfiguration(new PointConfiguration<ScalePoint>());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ScalePointConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
        }
    }
}