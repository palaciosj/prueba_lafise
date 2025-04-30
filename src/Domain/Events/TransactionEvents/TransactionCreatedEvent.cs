using Domain.Common;
using Domain.Entities;

namespace Domain.Events.TransactionEvents;

public class TransactionCreatedEvent(Transaction transaction) : BaseEvent
{
    public Transaction Transaction { get; } = transaction;
}
