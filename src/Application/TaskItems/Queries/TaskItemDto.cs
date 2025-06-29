using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Domain.Entities;
using TaskTimeTracker.Domain.Enums;

namespace TaskTimeTracker.Application.TaskItems.Queries;
public class TaskItemDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public PriorityLevel Priority { get; set; } 

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Created))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.LastModified));
        }
    }
}
