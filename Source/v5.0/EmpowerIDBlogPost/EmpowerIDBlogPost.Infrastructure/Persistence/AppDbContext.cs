using EmpowerIDBlogPost.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpowerIDBlogPost.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define a primary key for the BlogPost entity
            modelBuilder.Entity<BlogPost>().HasKey(b => b.PostId);

            // Add any additional configuration for your entities here
            // Example:
            // modelBuilder.Entity<BlogPost>().Property(b => b.Title).HasMaxLength(255);
        }
    }
}
