using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;

namespace TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionsHistory;

[Authorize]
public record GetTaskSessionsHistoryQuery : IRequest<TaskSessionsDto>
{
    public int TaskId { get; set; }
}

public class GetTaskSessionsHistoryQueryValidator : AbstractValidator<GetTaskSessionsHistoryQuery>
{
    public GetTaskSessionsHistoryQueryValidator()
    {
    }
}

public class GetTaskSessionsHistoryQueryHandler : IRequestHandler<GetTaskSessionsHistoryQuery, TaskSessionsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;
    private readonly IMapper _mapper;

    public GetTaskSessionsHistoryQueryHandler(IApplicationDbContext context, IUser user, IMapper mapper)
    {
        _context = context;
        _user = user;
        _mapper = mapper;
    }

    public async Task<TaskSessionsDto> Handle(GetTaskSessionsHistoryQuery request, CancellationToken cancellationToken)
    {
        var taskSessions = await _context.TaskSessions.Where(t => t.TaskId == request.TaskId && t.Task.ApplicationUserId == _user.Id && t.EndDateTime != null).ToListAsync();
        
        TaskSessionsDto taskSessionsDto = new TaskSessionsDto();
        _mapper.Map(taskSessions, taskSessionsDto);

        return taskSessionsDto;
    }
}
