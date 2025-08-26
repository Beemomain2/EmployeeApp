using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Tests.Domain.Entities;

public class EmployeeTests
{
    [Fact]
    public void Employee_ShouldHaveCorrectProperties()
    {
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            Role = EmployeeRole.Manager,
            HireDate = DateTime.Today,
            Salary = 75000,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        employee.Id.Should().Be(1);
        employee.FirstName.Should().Be("John");
        employee.LastName.Should().Be("Doe");
        employee.Email.Should().Be("john.doe@example.com");
        employee.PhoneNumber.Should().Be("123-456-7890");
        employee.Role.Should().Be(EmployeeRole.Manager);
        employee.HireDate.Should().Be(DateTime.Today);
        employee.Salary.Should().Be(75000);
        employee.IsActive.Should().BeTrue();
        employee.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        employee.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Employee_IsActive_ShouldDefaultToTrue()
    {
        var employee = new Employee();

        employee.IsActive.Should().BeTrue();
    }
}