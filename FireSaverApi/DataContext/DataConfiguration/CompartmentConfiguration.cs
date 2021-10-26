using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FireSaverApi.DataContext.DataConfiguration
{
    public class CompartmentConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Compartment
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // builder.HasOne(ev => ev.EvacuationPlan)
            //     .WithOne(cmp => (TEntity)cmp.Compartment)
            //     .HasForeignKey<TEntity>(p => p.Id);
            // .OnDelete(DeleteBehavior.SetNull);

            // builder.HasOne(tst => tst.CompartmentTest)
            //     .WithOne(cmp => (TEntity)cmp.Compartment)
            //     .HasForeignKey<TEntity>(p => p.Id);
            // .OnDelete(DeleteBehavior.SetNull);

            // builder.HasMany(iot => iot.Iots)
            //     .WithOne(cmp => (TEntity)cmp.Compartment);
            // .OnDelete(DeleteBehavior.SetNull);

            // builder.HasMany(rtPnts => rtPnts.RoutePoints)
            //     .WithOne(cmp => (TEntity)cmp.Compartment);
            // .OnDelete(DeleteBehavior.Cascade);

            // builder.HasMany(compartment => compartment.InboundUsers)
            //     .WithOne(user => (TEntity)user.CurrentCompartment);
            // .OnDelete(DeleteBehavior.SetNull);

        }
    }
}