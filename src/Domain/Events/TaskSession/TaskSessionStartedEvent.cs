using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Domain.Events.TaskSession;
public class TaskSessionStartedEvent : BaseEvent
{
    public TaskSessionStartedEvent(int taskId, DateTimeOffset startDateTime)
    {
        TaskId = taskId;
        StartDateTime = startDateTime;
    }

    public int TaskId { get; set; }
    public DateTimeOffset StartDateTime { get; set; }
}
