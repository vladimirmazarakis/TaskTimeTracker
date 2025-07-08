using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Domain.Entities;

namespace TaskTimeTracker.Application.TaskSessions.Queries;
public class TaskSessionDto
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public int TotalDuration { get; set; }

    public class Mapping : Profile
    {        
        public Mapping()
        {
            CreateMap<TaskSession, TaskSessionDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDateTime))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDateTime ?? DateTimeOffset.Now))
                .ForMember(dest => dest.TotalDuration, opt => opt.MapFrom(src => src.DurationInSeconds()));
        }
    }
}
