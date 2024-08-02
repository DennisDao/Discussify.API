using Domain.AggegratesModel.PostAggegrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> tagConfiguration)
        {
            tagConfiguration.HasKey(x => x.Id);

            tagConfiguration.ToTable("tags");

            tagConfiguration.Ignore(b => b.DomainEvents);

            tagConfiguration.Property(b => b.Id).UseHiLo("tag_seq");

            tagConfiguration.Property(b => b.Id).HasMaxLength(200);
        }
    }
}
