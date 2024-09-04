using Domain.AggegratesModel.NotificationAggegrate;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> notificationConfiguration)
        {
            notificationConfiguration.ToTable("notifications");

            notificationConfiguration.Ignore(b => b.DomainEvents);

            notificationConfiguration.Property(b => b.Id).ValueGeneratedOnAdd();

            notificationConfiguration.Property(b => b.Id)
                .HasMaxLength(200);

            notificationConfiguration.HasIndex("Id")
                .IsUnique(true);

            notificationConfiguration
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}

