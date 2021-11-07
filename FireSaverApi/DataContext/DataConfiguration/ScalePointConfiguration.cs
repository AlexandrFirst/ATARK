using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class ScalePointConfiguration : IEntityTypeConfiguration<ScalePoint>
    {
        public void Configure(EntityTypeBuilder<ScalePoint> builder)
        {
            
        }
    }
}