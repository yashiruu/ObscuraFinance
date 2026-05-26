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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Type)
                .HasConversion<string>();
        }
    }
}
