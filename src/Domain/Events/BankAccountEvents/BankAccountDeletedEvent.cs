using Domain.Common;
using Domain.Entities;

namespace Domain.Events.BankAccountEvents;

public class BankAccountDeletedEvent(BankAccount bankAccount) : BaseEvent 
{
    public BankAccount BankAccount { get; } = bankAccount;
}