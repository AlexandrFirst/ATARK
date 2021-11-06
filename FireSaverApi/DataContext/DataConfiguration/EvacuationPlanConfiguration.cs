using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class EvacuationPlanConfiguration : IEntityTypeConfiguration<EvacuationPlan>
    {
        public void Configure(EntityTypeBuilder<EvacuationPlan> builder)
        {
            builder.HasOne(evacuationPlan => evacuationPlan.ScaleModel)
                .WithOne(scaleModel => scaleModel.ApplyingEvacPlans)
                .HasForeignKey<ScaleModel>(scaleModel => scaleModel.ApplyingEvacPlansId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}