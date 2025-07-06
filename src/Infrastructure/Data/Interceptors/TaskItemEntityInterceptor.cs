using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TaskTimeTracker.Infrastructure.Data.Interceptors;
public class TaskItemEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        foreach (var entry in context.ChangeTracker.Entries<Domain.Entities.TaskItem>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                if (entry.Entity.Priority < Domain.Enums.PriorityLevel.None)
                {
                    entry.Entity.Priority = Domain.Enums.PriorityLevel.None;
                }
                else if (entry.Entity.Priority > Domain.Enums.PriorityLevel.Urgent)
                {
                    entry.Entity.Priority = Domain.Enums.PriorityLevel.Urgent;
                }
            }
        }
    }
}
