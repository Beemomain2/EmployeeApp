using MediatR;
using Application.Common.Models;

namespace Application.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(int Id) : IRequest<Result<bool>>;