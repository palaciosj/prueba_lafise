using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public abstract class Customer : BaseAuditableEntity 
{
    public required string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public double Incomes { get; set; }
    public IList<BankAccount> BankAccounts { get; set; } = [];
}
