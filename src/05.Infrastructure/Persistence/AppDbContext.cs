using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Infrastructure.Persistence
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations from the assembly containing AppDbContext
            // This will automatically apply any IEntityTypeConfiguration<T> implementations
            // register services in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Account> Accounts => Set<Account>();
    }
}
