using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
