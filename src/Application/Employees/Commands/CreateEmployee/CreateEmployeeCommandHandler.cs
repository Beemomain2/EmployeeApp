using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public CreateEmployeeCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Result<int>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == request.Email, cancellationToken);

        if (existingEmployee != null)
        {
            return Result<int>.Failure("Employee with this email already exists.");
        }

        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role,
            HireDate = request.HireDate,
            Salary = request.Salary,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new EmployeeCreatedEvent(employee.Id, employee.Email), cancellationToken);

        return Result<int>.Success(employee.Id);
    }
}