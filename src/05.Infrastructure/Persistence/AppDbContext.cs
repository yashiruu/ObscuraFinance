using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Base.Entities;

namespace Obscura.FinanceTracker.Infrastructure.Persistence
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations from the assembly containing AppDbContext
            // This will automatically apply any IEntityTypeConfiguration<T> implementations
            // register services in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            /// <summary> Global Query Filter </summary>
            modelBuilder.Entity<Category>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Account>()
                .HasQueryFilter(a => !a.IsDeleted);

            modelBuilder.Entity<Transaction>()
                .HasQueryFilter(t => !t.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Applies audit information automatically before entities are persisted.
        ///
        /// Rules:
        /// - Added entities populate CreatedAt and CreatedBy.
        /// - Modified entities populate UpdatedAt and UpdatedBy.
        /// - Soft deleted entities populate DeletedAt and DeletedBy.
        /// - Restored entities clear DeletedAt and DeletedBy.
        /// </summary>
        // Override SaveChanges to automatically set audit information
        private void ApplyAuditInformation()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    // For added entities
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy ??= Guid.Empty;
                        break;

                    // For modified entities
                    case EntityState.Modified:
                        // Skip update audit if soft deleting the entity
                        if (!entry.Entity.IsDeleted)
                        {
                            entry.Entity.UpdatedAt = DateTime.UtcNow;
                            entry.Entity.UpdatedBy ??= Guid.Empty;
                        }

                        break;
                }

                // If the entity is soft deleted
                if (entry.Entity.IsDeleted && entry.Entity.DeletedAt == null)
                {
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.DeletedBy ??= Guid.Empty;
                }

                // If the entity is restored
                if (!entry.Entity.IsDeleted && entry.Entity.DeletedAt != null)
                {
                    entry.Entity.DeletedAt = null;
                    entry.Entity.DeletedBy = null;
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return await base.SaveChangesAsync(cancellationToken);
        }

        // DbSets for the entities
        // Expose DbSet properties for each entity to allow querying and saving instances of these entities
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Account> Accounts => Set<Account>();
    }
}
