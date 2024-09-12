using Domain.AggegratesModel.FollowerAggegrate;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class FollowerConfiguration : IEntityTypeConfiguration<Follower>
    {
        public void Configure(EntityTypeBuilder<Follower> followerConfiguration)
        {
            followerConfiguration.ToTable("followers");

            followerConfiguration.Ignore(b => b.DomainEvents);

            followerConfiguration.Property(b => b.Id).ValueGeneratedOnAdd();

            followerConfiguration.Property(b => b.Id)
                .HasMaxLength(200);

            followerConfiguration.HasIndex("Id").IsUnique(true);

            followerConfiguration
               .HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(x => x.FollowerUserId)
               .OnDelete(DeleteBehavior.NoAction);

              followerConfiguration
               .HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(x => x.FollowingUserId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
