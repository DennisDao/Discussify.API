using Domain.AggegratesModel.PostAggegrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfiguration
{
    internal class TopicEntityTypeConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> topicConfiguration)
        {
            topicConfiguration.ToTable("topics");

            topicConfiguration.Ignore(b => b.DomainEvents);

            topicConfiguration.Property(b => b.Id)
                .UseHiLo("topiseq");

            topicConfiguration.Property(b => b.Id)
                .HasMaxLength(200);

            topicConfiguration.HasIndex("Id")
                .IsUnique(true);

            topicConfiguration
                .HasOne(p => p.TopicType)
                .WithMany()
                .HasForeignKey(t => t.TopicTypeId);

        }
    }
}
