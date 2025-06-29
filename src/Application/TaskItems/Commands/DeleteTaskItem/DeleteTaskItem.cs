using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;
using TaskTimeTracker.Domain.Exceptions.TaskItem;

namespace TaskTimeTracker.Application.TaskItems.Commands.DeleteTaskItem;

[Authorize]
public record DeleteTaskItemCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteTaskItemCommandValidator : AbstractValidator<DeleteTaskItemCommand>
{
    public DeleteTaskItemCommandValidator()
    {
    }
}

public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public DeleteTaskItemCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<int> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var deleteEntity = _context.Tasks.FirstOrDefault(x => x.Id == request.Id && x.ApplicationUserId == _user.UserId);
        if(deleteEntity is null)
        {
            throw new TaskItemNotFoundException(request.Id); 
        }
        _context.Tasks.Remove(deleteEntity);
        await _context.SaveChangesAsync(cancellationToken);
        return request.Id;
    }
}
