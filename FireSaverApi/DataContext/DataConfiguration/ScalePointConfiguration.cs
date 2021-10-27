using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class ScalePointConfiguration : IEntityTypeConfiguration<ScalePoint>
    {
        public void Configure(EntityTypeBuilder<ScalePoint> builder)
        {
            builder.HasOne(scalePoint => scalePoint.WorldPosition)
                    .WithOne(position => position.ScalePoint)
                    .HasForeignKey<ScalePoint>(p => p.WorldPositionId);
        }
    }
}