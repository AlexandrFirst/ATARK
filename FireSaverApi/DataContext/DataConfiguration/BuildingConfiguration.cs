using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.HasMany(building => building.ResponsibleUsers)
                .WithOne(user => user.ResponsibleForBuilding)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(building => building.Floors)
                .WithOne(floor => floor.BuildingWithThisFloor)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(building => building.BuildingCenterPosition)
                .WithOne(postition => postition.Building)
                .HasForeignKey<Building>(building => building.BuildingCenterPositionId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}