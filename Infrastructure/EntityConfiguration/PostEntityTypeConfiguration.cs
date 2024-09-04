using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
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

            postConfiguration.HasKey(p => p.Id);

            postConfiguration.Property(b => b.Id).ValueGeneratedOnAdd();

            postConfiguration.Property(b => b.Id).HasMaxLength(200);

            // Junction table
            postConfiguration.HasMany(s => s.Tags)
                             .WithMany(s => s.Posts)
                             .UsingEntity(j => j.ToTable("post_tags"));

            postConfiguration.HasMany(s => s.Comments);

            //postConfiguration.HasOne<Category>()
            //                 .WithMany()
            //                 //.HasForeignKey(p => p.CategoryId)
            //                 //.HasConstraintName("CategoryId")
            //                 .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
