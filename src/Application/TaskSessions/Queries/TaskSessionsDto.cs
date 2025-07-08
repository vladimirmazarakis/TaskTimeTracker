using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.TaskItems.Queries;
using TaskTimeTracker.Domain.Entities;

namespace TaskTimeTracker.Application.TaskSessions.Queries;

public class TaskSessionsDto
{
    public IReadOnlyCollection<TaskSessionDto> TaskSessions { get; set; } = default!;

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ICollection<TaskSession>, TaskSessionsDto>()
                .ForMember(dest => dest.TaskSessions, opt => opt.MapFrom(src => src.Select(x => new TaskSessionDto
                {
                    StartDate = x.StartDateTime,
                    EndDate = x.EndDateTime ?? DateTimeOffset.Now,
                    TotalDuration = x.DurationInSeconds()  
                })));
        }
    }
}
