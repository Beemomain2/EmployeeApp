using Domain.Enums;
using MediatR;
using Application.Common.Models;

namespace Application.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand : IRequest<Result<bool>>
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public EmployeeRole Role { get; init; }
    public DateTime HireDate { get; init; }
    public decimal Salary { get; init; }
    public bool IsActive { get; init; }
}
