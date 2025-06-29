using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Domain.Exceptions.TaskItem;
public class TaskItemNotFoundException : Exception
{
    public TaskItemNotFoundException(int id) : base($"Task item with ID {id} not found.")
    {
    }
}
