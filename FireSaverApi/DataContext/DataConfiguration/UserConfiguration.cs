using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
        
            builder.HasIndex(user => user.Mail).IsUnique();
            builder.HasMany(user => user.RolesList).WithMany(role => role.Users);
        }
    }
}