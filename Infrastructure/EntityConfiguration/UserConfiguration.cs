
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> userconfiguration)
        {
            userconfiguration.ToTable("users");

            userconfiguration.Ignore(b => b.DomainEvents);

            userconfiguration.HasKey(x => x.Id);

            userconfiguration.Property(b => b.Id)
               .UseHiLo("userseq");

            userconfiguration.Property(b => b.Id)
                .HasMaxLength(200);
        }
    }
}
