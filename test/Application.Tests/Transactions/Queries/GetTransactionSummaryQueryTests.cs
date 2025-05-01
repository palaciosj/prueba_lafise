using Application.Common.Interfaces;
using Application.Transactions.Queries.GetTransactionSummary;
using Domain.Entities;
using Domain.Enums;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tests.Transactions.Queries;

public class GetTransactionSummaryQueryTests
{
    [Fact]
    public async Task Handle_ShouldReturnTransactionSummary()
    {
        // Simular transacciones
        var transactions = new List<Transaction>
        {
            new() { Id = 1, Amount = 1000, Type = TransactionType.Deposit, BankAccountId = 1 },
            new() { Id = 2, Amount = 500, Type = TransactionType.Withdraw, BankAccountId = 1 }
        };

        // Simular cuenta bancaria
        var account = new BankAccount
        {
            Id = 1,
            AccountNumber = "AC001",
            Balance = 500, // Resultado esperado luego de aplicar transacciones
            CustomerId = 1,
            Transactions = transactions
        };

        var mockContext = new Mock<IApplicationDbContext>();

        // Simular FindAsync con respuesta esperada
        mockContext.Setup(c =>
            c.BankAccounts.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>())
        ).ReturnsAsync(account);

        var handler = new GetTransactionSummaryQueryHandler(mockContext.Object);
        var query = new GetTransactionSummaryQuery(1);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("AC001", result.AccountNumber);
        Assert.Equal(500, result.Balance);
        Assert.Equal(2, result.Transactions.Count);
        Assert.Contains(result.Transactions, t => t.Type == TransactionType.Deposit);
        Assert.Contains(result.Transactions, t => t.Type == TransactionType.Withdraw);
    }
}
