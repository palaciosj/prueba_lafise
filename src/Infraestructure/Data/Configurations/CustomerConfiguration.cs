using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(property => property.Name).IsRequired();
        builder.Property(property => property.BirthDate).IsRequired();
        builder.Property(property => property.Gender).IsRequired();
        builder.Property(property => property.Incomes).IsRequired();
    }
}