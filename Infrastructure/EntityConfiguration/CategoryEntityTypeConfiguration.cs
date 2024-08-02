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
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> categoryConfiguration)
        {
            categoryConfiguration.HasKey(x => x.Id);

            categoryConfiguration.ToTable("categories");

            categoryConfiguration.Ignore(b => b.DomainEvents);

            categoryConfiguration.Property(b => b.Id).UseHiLo("categories_seq");

            categoryConfiguration.Property(b => b.Id).HasMaxLength(200);
        }
    }
}
