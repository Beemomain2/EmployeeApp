using Domain.Common;

namespace Domain.Events;

public record EmployeeCreatedEvent(int EmployeeId, string Email) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}