using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;
using TaskTimeTracker.Domain.Exceptions.TaskSession;

namespace TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionDuration;

[Authorize]
public record GetTaskSessionDurationQuery : IRequest<int>
{
    public int TaskId { get; set; }
}

public class GetTaskSessionDurationQueryValidator : AbstractValidator<GetTaskSessionDurationQuery>
{
    public GetTaskSessionDurationQueryValidator()
    {
    }
}

public class GetTaskSessionDurationQueryHandler : IRequestHandler<GetTaskSessionDurationQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetTaskSessionDurationQueryHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<int> Handle(GetTaskSessionDurationQuery request, CancellationToken cancellationToken)
    {
        var taskSession = await _context.TaskSessions.FirstOrDefaultAsync(t => t.TaskId == request.TaskId && t.Task.ApplicationUserId == _user.Id && t.EndDateTime == null);
        if (taskSession is null)
        {
            throw new NoStartedTaskSessionWasFoundException(request.TaskId);
        }
        return taskSession.DurationInSeconds();
    }
}
