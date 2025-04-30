using Domain.Common;

namespace Domain.Entities;

public class BankAccount : BaseAuditableEntity 
{
    public required string AccountNumber { get; set; }
    public double Balance { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = [];
}
