using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class ShelterConfiguration : IEntityTypeConfiguration<Shelter>
    {
        public void Configure(EntityTypeBuilder<Shelter> builder)
        {
            builder.HasOne(b => b.Building)
                .WithMany(s => s.Shelters)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.Users)
                .WithOne(s => s.Shelter)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}