using Application.Employees.Commands.CreateEmployee;
using Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Tests.Application.Commands;

public class CreateEmployeeCommandValidatorTests
{
    private readonly CreateEmployeeCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveErrors()
    {
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

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyFirstName_ShouldHaveError()
    {
        var command = new CreateEmployeeCommand
        {
            FirstName = "",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            Role = EmployeeRole.Employee,
            HireDate = DateTime.Today,
            Salary = 50000
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_InvalidEmail_ShouldHaveError()
    {
        var command = new CreateEmployeeCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email",
            PhoneNumber = "123-456-7890",
            Role = EmployeeRole.Employee,
            HireDate = DateTime.Today,
            Salary = 50000
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_NegativeSalary_ShouldHaveError()
    {
        var command = new CreateEmployeeCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            Role = EmployeeRole.Employee,
            HireDate = DateTime.Today,
            Salary = -1000
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Salary);
    }

    [Fact]
    public void Validate_FutureHireDate_ShouldHaveError()
    {
        var command = new CreateEmployeeCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            Role = EmployeeRole.Employee,
            HireDate = DateTime.Today.AddDays(1),
            Salary = 50000
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.HireDate);
    }
}