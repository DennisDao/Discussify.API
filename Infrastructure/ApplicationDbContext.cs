using Domain.AggegratesModel.NotificationAggegrate;
using Domain.AggegratesModel.PostAggegrate;
using Infrastructure.Entities;
using Infrastructure.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// dotnet ef migrations add update_1 --verbose --project Infrastructure --startup-project Discussify.API
// dotnet ef database update --project Infrastructure --startup-project Discussify.API
namespace Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<OutboxMessage> OutBoxMessages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
