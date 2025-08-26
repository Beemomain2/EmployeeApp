using EmployeeApp.Application.Interfaces;
using EmployeeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
}
