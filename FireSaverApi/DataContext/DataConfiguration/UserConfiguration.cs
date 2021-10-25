using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.LastSeenBuildingPosition)
                .WithOne(pos => pos.User)
                .HasForeignKey<Position>(p => p.Id);
                // .OnDelete(DeleteBehavior.SetNull);
        }
    }
}