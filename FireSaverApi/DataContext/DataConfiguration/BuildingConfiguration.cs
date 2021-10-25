using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.HasMany(u => u.ResponsibleUsers)
                .WithOne(b => b.ResponsibleForBuilding);
                // .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(f => f.Floors)
                .WithOne(b => b.BuildingWithThisFloor);
                // .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.BuildingCenter)
                .WithOne(b => b.Building)
                .HasForeignKey<Position>(b => b.Id);
                // .OnDelete(DeleteBehavior.Cascade);

        }
    }
}