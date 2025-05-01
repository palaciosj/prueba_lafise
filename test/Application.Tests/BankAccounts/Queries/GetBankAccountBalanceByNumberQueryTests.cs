using Application.BankAccounts.Queries.GetBalanceByNumber;
using Application.Common.Interfaces;
using Domain.Entities;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tests.BankAccounts.Queries;

public class GetBankAccountBalanceByNumberQueryTests
{
    [Fact]
    public async Task Handle_ShouldReturnCorrectBalance()
    {
        var accounts = new List<BankAccount>
        {
            new() { Id = 1, AccountNumber = "AC123456", Balance = 2500, CustomerId = 1 }
        }.AsQueryable();

        var mockDbSet = new Mock<DbSet<BankAccount>>();
        mockDbSet.As<IQueryable<BankAccount>>().Setup(m => m.Provider).Returns(accounts.Provider);
        mockDbSet.As<IQueryable<BankAccount>>().Setup(m => m.Expression).Returns(accounts.Expression);
        mockDbSet.As<IQueryable<BankAccount>>().Setup(m => m.ElementType).Returns(accounts.ElementType);
        mockDbSet.As<IQueryable<BankAccount>>().Setup(m => m.GetEnumerator()).Returns(accounts.GetEnumerator());

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.BankAccounts).Returns(mockDbSet.Object);

        var handler = new GetBankAccountBalanceByNumberQueryHandler(mockContext.Object);
        var query = new GetBankAccountBalanceByNumberQuery("AC123456");

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("AC123456", result.AccountNumber);
        Assert.Equal(2500, result.Balance);
    }
}
