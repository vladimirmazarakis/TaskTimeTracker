using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Domain.Exceptions.TaskSession;
public class StartedTaskSessionAlreadyExistsException : Exception
{
    public StartedTaskSessionAlreadyExistsException(int taskId): base($"A started task session for task with ID {taskId} already exists.")
    {
    }
}
