using Application.BankAccounts.Commands.CreateBankAccount;
using Application.Common.Interfaces;
using Domain.Entities;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Tests.BankAccounts;

public class CreateBankAccountCommandTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;

    public CreateBankAccountCommandTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();

        // Simular un customer válido
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Test User", BirthDate = DateTime.Today.AddYears(-30), Gender = Domain.Enums.Gender.Male, Incomes = 50000 }
        }.AsQueryable();

        var mockCustomerDbSet = new Mock<DbSet<Customer>>();
        mockCustomerDbSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.Provider);
        mockCustomerDbSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
        mockCustomerDbSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
        mockCustomerDbSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());

        _mockContext.Setup(c => c.Customers).Returns(mockCustomerDbSet.Object);

        // Simular un DbSet vacío para BankAccounts
        var bankAccounts = new List<BankAccount>().AsQueryable();

        var mockBankDbSet = new Mock<DbSet<BankAccount>>();
        mockBankDbSet.As<IQueryable<BankAccount>>().Setup(m => m.Provider).Returns(bankAccounts.Provider);
        mockBankDbSet.As<IQueryable<BankAccount>>().Setup(m => m.Expression).Returns(bankAccounts.Expression);
        mockBankDbSet.As<IQueryable<BankAccount>>().Setup(m => m.ElementType).Returns(bankAccounts.ElementType);
        mockBankDbSet.As<IQueryable<BankAccount>>().Setup(m => m.GetEnumerator()).Returns(bankAccounts.GetEnumerator());

        _mockContext.Setup(c => c.BankAccounts).Returns(mockBankDbSet.Object);
        _mockContext.Setup(c => c.BankAccounts.Add(It.IsAny<BankAccount>())).Callback<BankAccount>(b => b.Id = 99);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_ShouldCreateBankAccountSuccessfully()
    {
        // Arrange
        var handler = new CreateBankAccountCommandHandler(_mockContext.Object);
        var command = new CreateBankAccountCommand
        {
            AccountNumber = "AC000123",
            Balance = 5000,
            CustomerId = 1
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result > 0);
    }
}
