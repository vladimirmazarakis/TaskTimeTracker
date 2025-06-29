using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Domain.Exceptions.TaskItem;

public class TaskItemAlreadyCompletedException : Exception
{
    public TaskItemAlreadyCompletedException(int taskId) 
        : base($"The task item with ID {taskId} has already been completed.")
    {
    }
}
