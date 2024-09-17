using Domain.AggegratesModel.ActivityAggegrate;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> activityConfiguration)
        {
            activityConfiguration.ToTable("activities");

            activityConfiguration.Ignore(b => b.DomainEvents);

            activityConfiguration.Property(b => b.Id).ValueGeneratedOnAdd();

            activityConfiguration.Property(b => b.Id)
                .HasMaxLength(200);

            activityConfiguration.HasIndex("Id").IsUnique(true);

            activityConfiguration
               .HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.NoAction);

            activityConfiguration
             .HasOne<ApplicationUser>()
             .WithMany()
             .HasForeignKey(x => x.CreateByUserId)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
