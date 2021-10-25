using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class PointConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Point
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasOne(p => p.MapPosition)
                .WithOne(pos => (TEntity)pos.PointPostion)
                .HasForeignKey<Position>(p => p.Id);
                //.OnDelete(DeleteBehavior.Cascade);
        }
    }
}