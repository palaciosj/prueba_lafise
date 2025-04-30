using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Configurations;
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(property => property.Amount).IsRequired();
        builder.Property(property => property.Type).IsRequired();
        builder.HasOne(property => property.BankAccount).WithMany(bankAccount => bankAccount.Transactions).HasForeignKey(property => property.BankAccountId).IsRequired();
    }
}