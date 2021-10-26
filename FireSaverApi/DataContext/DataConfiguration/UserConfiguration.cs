using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(user => user.LastSeenBuildingPosition)
                .WithOne(position => position.User)
                .HasForeignKey<User>(user => user.LastSeenBuildingPositionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}