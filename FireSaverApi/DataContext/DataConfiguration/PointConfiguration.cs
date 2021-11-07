using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class PointConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Point
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
           
        }
    }
}