using Domain.Entities;
using Domain.Events.BankAccountEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.BankAccounts.EventHandlers;

public class BankAccountCreatedEventHandler(ILogger<BankAccountCreatedEventHandler> logger) : INotificationHandler<BankAccountCreatedEvent>
{
    private readonly ILogger<BankAccountCreatedEventHandler> _logger = logger;

    public Task Handle(BankAccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PruebaLAFISE Domain Event: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
