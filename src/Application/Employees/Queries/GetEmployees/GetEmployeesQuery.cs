using Domain.Entities;
using MediatR;

namespace Application.Employees.Queries.GetEmployees;

public record GetEmployeesQuery : IRequest<List<Employee>>;
