using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class FloorConfiguration : IEntityTypeConfiguration<Floor>
    {
        public void Configure(EntityTypeBuilder<Floor> builder)
        {
            builder.HasMany(f => f.Rooms).WithOne(r => r.RoomFloor);
            builder.HasOne(f => f.CurrentFloor).WithMany(r => r.NearFloors);

        }
    }
}