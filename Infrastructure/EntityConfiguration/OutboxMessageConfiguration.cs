using Domain.AggegratesModel.PostAggegrate;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Runtime.InteropServices.Marshalling;

namespace Infrastructure.EntityConfiguration
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> outBoxconfiguration)
        {
            {
                outBoxconfiguration.ToTable("outbox_messages");

                outBoxconfiguration.Property(b => b.Id)
                    .UseHiLo("outbox_messages_seq");

                outBoxconfiguration.Property(b => b.Id)
                    .HasMaxLength(200);
                outBoxconfiguration.HasIndex("Id").IsUnique(true);

                outBoxconfiguration.HasIndex(u => u.Type).HasDatabaseName("outbox_messages_IDX1");
            }
        }
    }
}
