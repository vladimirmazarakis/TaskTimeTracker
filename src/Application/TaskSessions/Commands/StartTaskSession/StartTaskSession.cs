using Microsoft.EntityFrameworkCore;
using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;
using TaskTimeTracker.Domain.Entities;
using TaskTimeTracker.Domain.Events.TaskSession;
using TaskTimeTracker.Domain.Exceptions.TaskItem;
using TaskTimeTracker.Domain.Exceptions.TaskSession;

namespace TaskTimeTracker.Application.TaskSessions.Commands.StartTaskSession;

[Authorize]
public record StartTaskSessionCommand : IRequest<bool>
{
    public int TaskId { get; set; }
}

public class StartTaskSessionCommandValidator : AbstractValidator<StartTaskSessionCommand>
{
    public StartTaskSessionCommandValidator()
    {
    }
}

public class StartTaskSessionCommandHandler : IRequestHandler<StartTaskSessionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public StartTaskSessionCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<bool> Handle(StartTaskSessionCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId && t.ApplicationUserId == _user.Id);

        if(task is null)
        {
            throw new TaskItemNotFoundException(request.TaskId);
        }

        if (_context.TaskSessions.Any(t => t.TaskId  == task.Id && t.EndDateTime == null))
        {
            throw new StartedTaskSessionAlreadyExistsException(request.TaskId);
        }

        var newTaskSession = new TaskSession
        {
            TaskId = task.Id,
            StartDateTime = DateTimeOffset.UtcNow,
            EndDateTime = null
        };

        _context.TaskSessions.Add(newTaskSession);

        await _context.SaveChangesAsync(cancellationToken);

        newTaskSession.AddDomainEvent(new TaskSessionStartedEvent(newTaskSession.TaskId, newTaskSession.StartDateTime));

        return true;
    }
}
