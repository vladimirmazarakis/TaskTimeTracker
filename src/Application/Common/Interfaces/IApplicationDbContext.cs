using TaskTimeTracker.Domain.Entities;

namespace TaskTimeTracker.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    public DbSet<TaskItem> Tasks { get; }
    public DbSet<TaskSession> TaskSessions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
