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
        public void Configure(EntityTypeBuilder<Post> buyerConfiguration)
        {
            buyerConfiguration.ToTable("posts");

            buyerConfiguration.Ignore(b => b.DomainEvents);

            buyerConfiguration.Property(b => b.Id)
                .UseHiLo("postseq");

            buyerConfiguration.Property(b => b.Id)
                .HasMaxLength(200);

            buyerConfiguration.HasIndex("Id")
                .IsUnique(true);

            buyerConfiguration.HasMany(b => b.Topics)
                .WithOne();
        }
    }
}
