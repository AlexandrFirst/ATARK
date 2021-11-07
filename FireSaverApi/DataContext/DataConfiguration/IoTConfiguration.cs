using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class IoTConfiguration : IEntityTypeConfiguration<IoT>
    {
        public void Configure(EntityTypeBuilder<IoT> builder)
        {
         
        }
    }
}