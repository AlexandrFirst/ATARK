using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class IoTConfiguration : IEntityTypeConfiguration<IoT>
    {
        public void Configure(EntityTypeBuilder<IoT> builder)
        {
            builder.HasOne(iot => iot.MapPosition)
                .WithOne(pos => pos.IotPostion)
                .HasForeignKey<Position>(pos => pos.IotId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}