using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class CompartmentConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Compartment
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasOne(compartment => compartment.EvacuationPlan)
                .WithOne(evacuationPlan => (TEntity)evacuationPlan.Compartment)
                .HasForeignKey<TEntity>(compartment => compartment.EvacuationPlanId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(compartment => compartment.CompartmentTest)
                .WithOne(test => (TEntity)test.Compartment)
                .HasForeignKey<TEntity>(compartment => compartment.CompartmentTestId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(compartment => compartment.Iots)
                .WithOne(iot => (TEntity)iot.Compartment)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(compartment => compartment.RoutePoints)
                .WithOne(iot => (TEntity)iot.Compartment)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(compartment => compartment.InboundUsers)
                .WithOne(iot => (TEntity)iot.CurrentCompartment);
        }
    }
}