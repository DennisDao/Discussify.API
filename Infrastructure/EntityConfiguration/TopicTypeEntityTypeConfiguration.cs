using Domain.AggegratesModel.PostAggegrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfiguration
{
    public class TopicTypeEntityTypeConfiguration : IEntityTypeConfiguration<TopicType>
    {
        public void Configure(EntityTypeBuilder<TopicType> topicTypeConfiguration)
        {
            topicTypeConfiguration.ToTable("topic_type");

            topicTypeConfiguration.Ignore(b => b.DomainEvents);

            topicTypeConfiguration.Property(b => b.Id)
                .UseHiLo("topiseq");

            topicTypeConfiguration
                .Property(b => b.Id)
                .HasMaxLength(200);

            topicTypeConfiguration
                .HasIndex("Id")
                .IsUnique(true);
        }
    }
}
