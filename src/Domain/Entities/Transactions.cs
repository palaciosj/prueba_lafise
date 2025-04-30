using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Transaction: BaseAuditableEntity
{
    public double Amount { get; set; }
    public TransactionType Type { get; set; }
    public int BankAccountId { get; set; }
    public required BankAccount BankAccount { get; set; }
}