using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Date)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(t => t.Type)
                .IsRequired()
                .HasConversion<string>();

            // Relation
            builder.HasOne(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountId);

            builder.HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId);
        }
    }
}
