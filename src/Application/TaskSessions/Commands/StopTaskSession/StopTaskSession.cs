using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;
using TaskTimeTracker.Domain.Entities;
using TaskTimeTracker.Domain.Events.TaskSession;
using TaskTimeTracker.Domain.Exceptions.TaskItem;
using TaskTimeTracker.Domain.Exceptions.TaskSession;

namespace TaskTimeTracker.Application.TaskSessions.Commands.StopTaskSession;

[Authorize]
public record StopTaskSessionCommand : IRequest<bool>
{
    public int TaskId { get; set; }
}

public class StopTaskSessionCommandValidator : AbstractValidator<StopTaskSessionCommand>
{
    public StopTaskSessionCommandValidator()
    {
    }
}

public class StopTaskSessionCommandHandler : IRequestHandler<StopTaskSessionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public StopTaskSessionCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<bool> Handle(StopTaskSessionCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId && t.ApplicationUserId == _user.Id);

        if (task is null)
        {
            throw new TaskItemNotFoundException(request.TaskId);
        }

        var taskSession = await _context.TaskSessions
            .FirstOrDefaultAsync(ts => ts.TaskId == task.Id && ts.EndDateTime == null);

        if (taskSession is null)
        {
            throw new NoStartedTaskSessionWasFoundException(request.TaskId);
        }

        taskSession.EndDateTime = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        taskSession.AddDomainEvent(new TaskSessionStoppedEvent(taskSession.TaskId, (DateTimeOffset)taskSession.EndDateTime));

        return true;
    }
}
