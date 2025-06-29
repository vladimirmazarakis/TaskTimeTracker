using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Domain.Entities;

public class TaskItem : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public PriorityLevel Priority { get; set; }
    public string? Description { get; set; }
    public string ApplicationUserId { get; set; } = default!;
    public bool IsCompleted { get; set; }
}
