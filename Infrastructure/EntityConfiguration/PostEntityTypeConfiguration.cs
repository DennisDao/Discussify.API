using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfiguration
{
    internal class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> postConfiguration)
        {
            postConfiguration.ToTable("posts");

            postConfiguration.Ignore(b => b.DomainEvents);

            postConfiguration.Property(b => b.Id)
                .UseHiLo("postseq");

            postConfiguration.Property(b => b.Id)
                .HasMaxLength(200);

            postConfiguration.HasIndex("Id")
                .IsUnique(true);

            postConfiguration.HasMany(b => b.Topics)
                .WithOne();
        }
    }
}
