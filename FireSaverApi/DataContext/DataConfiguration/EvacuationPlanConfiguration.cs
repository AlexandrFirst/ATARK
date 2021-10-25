using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class EvacuationPlanConfiguration : IEntityTypeConfiguration<EvacuationPlan>
    {
        public void Configure(EntityTypeBuilder<EvacuationPlan> builder)
        {
            builder.HasOne(ev => ev.ScaleModel)
                .WithOne(sm => sm.ApplyingEvacPlans)
                .HasForeignKey<ScaleModel>(s => s.Id);
                //.OnDelete(DeleteBehavior.Cascade);
        }
    }
}