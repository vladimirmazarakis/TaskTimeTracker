using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Domain.Entities;
using TaskTimeTracker.Domain.Enums;

namespace TaskTimeTracker.Application.TaskItems.Commands.UpdateTaskItem;
public class UpdatedTaskItemDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public PriorityLevel Priority { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TaskItem, UpdatedTaskItemDto>();
        }
    }
}
