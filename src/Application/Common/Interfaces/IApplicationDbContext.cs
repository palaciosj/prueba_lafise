using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<BankAccount> BankAccounts { get; }
    DbSet<Transaction> Transactions { get; }
}