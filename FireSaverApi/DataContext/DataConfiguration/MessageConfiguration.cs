using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(building => building.Building)
                .WithMany(message => message.Messages)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(user => user.User)
                .WithMany(message => message.Messages)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}