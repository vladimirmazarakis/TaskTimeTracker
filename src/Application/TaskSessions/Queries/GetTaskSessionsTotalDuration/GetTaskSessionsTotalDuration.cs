using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;

namespace TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionsTotalDuration;

[Authorize]
public record GetTaskSessionsTotalDurationQuery : IRequest<int>
{
    public int TaskId { get; set; }
}

public class GetTaskSessionsTotalDurationQueryValidator : AbstractValidator<GetTaskSessionsTotalDurationQuery>
{
    public GetTaskSessionsTotalDurationQueryValidator()
    {
    }
}

public class GetTaskSessionsTotalDurationQueryHandler : IRequestHandler<GetTaskSessionsTotalDurationQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetTaskSessionsTotalDurationQueryHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<int> Handle(GetTaskSessionsTotalDurationQuery request, CancellationToken cancellationToken)
    {
        var taskSessions = await _context.TaskSessions.Where(t => t.TaskId == request.TaskId && t.Task.ApplicationUserId == _user.Id && t.EndDateTime != null).ToListAsync();
        int duration = 0;

        foreach(var taskSession in taskSessions)
        {
            duration += taskSession.DurationInSeconds();
        }

        return duration;
    }
}
