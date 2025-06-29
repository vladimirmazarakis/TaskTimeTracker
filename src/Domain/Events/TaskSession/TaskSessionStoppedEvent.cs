using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Domain.Events.TaskSession;

public class TaskSessionStoppedEvent : BaseEvent
{
    public TaskSessionStoppedEvent(int taskId, DateTimeOffset endDateTime)
    {
        TaskId = taskId;
        EndDateTime = endDateTime;
    }

    public int TaskId { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
}
