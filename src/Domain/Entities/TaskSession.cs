using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Domain.Entities;
public class TaskSession : BaseAuditableEntity
{
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset? EndDateTime { get; set; }
    public int TaskId { get; set; }
    public TaskItem Task { get; set; } = default!;

    public int DurationInSeconds()
    {
        double result = 0;
        if(EndDateTime.HasValue)
        {
            result = (EndDateTime.Value - StartDateTime).TotalSeconds;
        }
        else
        {
            result = (DateTime.UtcNow - StartDateTime).TotalSeconds;
        }
        return (int)result;
    }
}
