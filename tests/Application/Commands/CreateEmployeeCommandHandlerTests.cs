using Application.Common.Interfaces;
using Application.Employees.Commands.CreateEmployee;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Application.Commands;

public class CreateEmployeeCommandHandlerTests
{
    private ApplicationDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateEmployee()
    {
        using var context = GetInMemoryContext();
        var mockMediator = new Mock<IMediator>();
        var handler = new CreateEmployeeCommandHandler(context, mockMediator.Object);

        var command = new CreateEmployeeCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            Role = EmployeeRole.Employee,
            HireDate = DateTime.Today,
            Salary = 50000
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        var employee = await context.Employees.FindAsync(result.Value);
        employee.Should().NotBeNull();
        employee!.FirstName.Should().Be("John");
        employee.LastName.Should().Be("Doe");
        employee.Email.Should().Be("john.doe@example.com");
        employee.Role.Should().Be(EmployeeRole.Employee);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ShouldReturnFailure()
    {
        using var context = GetInMemoryContext();
        var mockMediator = new Mock<IMediator>();
        var handler = new CreateEmployeeCommandHandler(context, mockMediator.Object);

        var existingEmployee = new Employee
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "john.doe@example.com",
            PhoneNumber = "987-654-3210",
            Role = EmployeeRole.Manager,
            HireDate = DateTime.Today.AddDays(-30),
            Salary = 60000,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Employees.Add(existingEmployee);
        await context.SaveChangesAsync();

        var command = new CreateEmployeeCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            Role = EmployeeRole.Employee,
            HireDate = DateTime.Today,
            Salary = 50000
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Employee with this email already exists.");
    }
}