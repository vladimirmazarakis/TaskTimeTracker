using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Domain.Exceptions.TaskSession;

public class NoStartedTaskSessionWasFoundException : Exception
{
    public NoStartedTaskSessionWasFoundException(int taskId) : base($"No started task session was found for task with ID {taskId}.")
    {
        
    }
}
