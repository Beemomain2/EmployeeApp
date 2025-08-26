using Application.Employees.Queries.GetEmployees;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.Application.Queries;

public class GetEmployeesQueryHandlerTests
{
    private ApplicationDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Handle_WithEmployees_ShouldReturnSortedList()
    {
        using var context = GetInMemoryContext();
        var handler = new GetEmployeesQueryHandler(context);

        var employees = new List<Employee>
        {
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                Role = EmployeeRole.Employee,
                HireDate = DateTime.Today,
                Salary = 50000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "987-654-3210",
                Role = EmployeeRole.Manager,
                HireDate = DateTime.Today.AddDays(-30),
                Salary = 60000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                FirstName = "Bob",
                LastName = "Adams",
                Email = "bob.adams@example.com",
                PhoneNumber = "555-123-4567",
                Role = EmployeeRole.TeamLead,
                HireDate = DateTime.Today.AddDays(-60),
                Salary = 55000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();

        var query = new GetEmployeesQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(3);
        result.Should().BeInAscendingOrder(e => e.LastName);
        result.First().LastName.Should().Be("Adams");
        result.Last().LastName.Should().Be("Smith");
    }

    [Fact]
    public async Task Handle_NoEmployees_ShouldReturnEmptyList()
    {
        using var context = GetInMemoryContext();
        var handler = new GetEmployeesQueryHandler(context);
        var query = new GetEmployeesQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}
