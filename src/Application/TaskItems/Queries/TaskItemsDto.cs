using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Domain.Entities;

namespace TaskTimeTracker.Application.TaskItems.Queries;
public class TaskItemsDto
{
    public IReadOnlyCollection<TaskItemDto> TaskItems { get; set; } = default!;

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ICollection<TaskItem>, TaskItemsDto>()
                .ForMember(dest => dest.TaskItems, opt => opt.MapFrom(src => src.Select(x => new TaskItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Priority = x.Priority,
                    Description = x.Description,
                    IsCompleted = x.IsCompleted
                })));
        }
    }
}
