using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class ScalePointConfiguration : IEntityTypeConfiguration<ScalePoint>
    {
        public void Configure(EntityTypeBuilder<ScalePoint> builder)
        {
            //builder.HasOne(p => p.WorldPosition).WithOne(p => p.ScalePoint).HasForeignKey<Position>(p => p.Id);
        }
    }
}