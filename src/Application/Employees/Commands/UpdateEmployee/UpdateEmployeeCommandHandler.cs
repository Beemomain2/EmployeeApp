using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdateEmployeeCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Result<bool>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            return Result<bool>.Failure("Employee not found.");
        }

        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == request.Email && e.Id != request.Id, cancellationToken);

        if (existingEmployee != null)
        {
            return Result<bool>.Failure("Employee with this email already exists.");
        }

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.PhoneNumber = request.PhoneNumber;
        employee.Role = request.Role;
        employee.HireDate = request.HireDate;
        employee.Salary = request.Salary;
        employee.IsActive = request.IsActive;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new EmployeeUpdatedEvent(employee.Id, employee.Email), cancellationToken);

        return Result<bool>.Success(true);
    }
}