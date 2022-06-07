using ExamenAdam.Entities;
using ExamenAdam.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamenAdam.Data
{
    public class ExamenAdamContext : IdentityDbContext<User, Role, long>
    {
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Address> Address { get; set; } = null!;

        public ExamenAdamContext (DbContextOptions<ExamenAdamContext> options) : base(options)
        {
            //
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
