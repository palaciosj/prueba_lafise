using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Configurations;
public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.Property(property => property.AccountNumber).IsRequired();
        builder.Property(property => property.Balance).IsRequired();
        builder.HasOne(property => property.Customer).WithMany(customer => customer.BankAccounts).HasForeignKey(property => property.CustomerId).IsRequired();
    }
}