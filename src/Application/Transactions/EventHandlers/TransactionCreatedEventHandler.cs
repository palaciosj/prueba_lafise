using Domain.Entities;
using Domain.Events.TransactionEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Transactions.EventHandlers;

public class TransactionCreatedEventHandler(ILogger<TransactionCreatedEventHandler> logger) : INotificationHandler<TransactionCreatedEvent>
{
    private readonly ILogger<TransactionCreatedEventHandler> _logger = logger;

    public Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PruebaLAFISE Domain Event: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
