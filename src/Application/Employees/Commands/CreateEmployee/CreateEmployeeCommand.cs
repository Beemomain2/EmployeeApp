using Domain.Enums;
using MediatR;
using Application.Common.Models;

namespace Application.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand : IRequest<Result<int>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public EmployeeRole Role { get; init; }
    public DateTime HireDate { get; init; }
    public decimal Salary { get; init; }
}