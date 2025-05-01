using Application.Common.Interfaces;
using Application.Transactions.Commands.CreateTransaction;
using Domain.Entities;
using Domain.Enums;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tests.Transactions;

public class CreateTransactionCommandTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly BankAccount _bankAccount;

    public CreateTransactionCommandTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();

        // Cuenta simulada con balance inicial
        _bankAccount = new BankAccount
        {
            Id = 1,
            AccountNumber = "AC999999",
            Balance = 1000,
            CustomerId = 1
        };

        // Simular el DbSet para consultas por LINQ (si se usa)
        var accounts = new List<BankAccount> { _bankAccount }.AsQueryable();
        var mockDbSet = new Mock<DbSet<BankAccount>>();
        mockDbSet.As<IQueryable<BankAccount>>().Setup(m => m.Provider).Returns(accounts.Provider);
        mockDbSet.As<IQueryable<BankAccount>>().Setup(m => m.Expression).Returns(accounts.Expression);
        mockDbSet.As<IQueryable<BankAccount>>().Setup(m => m.ElementType).Returns(accounts.ElementType);
        mockDbSet.As<IQueryable<BankAccount>>().Setup(m => m.GetEnumerator()).Returns(accounts.GetEnumerator());

        _mockContext.Setup(c => c.BankAccounts).Returns(mockDbSet.Object);

        // Simular FindAsync para devolver la cuenta según ID
        _mockContext.Setup(c => c.BankAccounts.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_bankAccount);

        _mockContext.Setup(c => c.Transactions.Add(It.IsAny<Transaction>()));
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_ShouldDepositCorrectly()
    {
        var handler = new CreateTransactionCommandHandler(_mockContext.Object);

        var command = new CreateTransactionCommand
        {
            Amount = 500,
            Type = TransactionType.Deposit,
            BankAccountId = 1
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(1500, _bankAccount.Balance);
    }

    [Fact]
    public async Task Handle_ShouldWithdrawCorrectly()
    {
        var handler = new CreateTransactionCommandHandler(_mockContext.Object);

        var command = new CreateTransactionCommand
        {
            Amount = 300,
            Type = TransactionType.Withdraw,
            BankAccountId = 1
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(700, _bankAccount.Balance);
    }
}
