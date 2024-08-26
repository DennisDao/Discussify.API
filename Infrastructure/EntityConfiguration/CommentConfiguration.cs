using Domain.AggegratesModel.PostAggegrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.EntityConfiguration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> commentConfiguration)
        {
            commentConfiguration.ToTable("comments");

            commentConfiguration.Ignore(b => b.DomainEvents);

            commentConfiguration.Property(b => b.Id)
                .UseHiLo("comment_seq");

            commentConfiguration.Property(b => b.Id)
                .HasMaxLength(200);

            commentConfiguration.HasIndex("Id")
            .IsUnique(true);
        }
    }
}
