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
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Transaction> Transactions => Set<Transaction>();
    }
}
