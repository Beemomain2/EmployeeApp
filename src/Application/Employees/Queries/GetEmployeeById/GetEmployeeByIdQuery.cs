using Domain.Entities;
using MediatR;

namespace Application.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(int Id) : IRequest<Employee?>;