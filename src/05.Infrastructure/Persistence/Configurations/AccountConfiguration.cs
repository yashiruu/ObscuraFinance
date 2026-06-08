using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration :  IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(account => account.Id);

            builder.Property(account => account.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(account => account.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");    // Ensure account names are unique

            builder.Property(account => account.Description)
                .HasMaxLength(500);

            builder.Property(account => account.InitialBalance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(account => account.CurrentBalance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(account => account.Currency)
                .IsRequired()
                .HasMaxLength(3) // ISO currency code
                .HasDefaultValue("IDR");

            builder.Property(account => account.Type)
                .IsRequired();

            builder.Property(account => account.IsActive)
                .HasDefaultValue(true);

            builder.Property(account => account.IsDeleted)
                .HasDefaultValue(false);

            // Optional: global filter (Active it later)
            //builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
