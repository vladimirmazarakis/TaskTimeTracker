using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;
using TaskTimeTracker.Domain.Entities;
using TaskTimeTracker.Domain.Enums;

namespace TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;

[Authorize]
public record CreateTaskItemCommand : IRequest<int>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public PriorityLevel Priority { get; set; }
}

public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator()
    {
    }
}

public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreateTaskItemCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<int> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var taskItem = new TaskItem()
        { 
            Name = request.Name,
            Description = request.Description,
            ApplicationUserId = _user.UserId,
            Priority = request.Priority
        };
        _context.Tasks.Add(taskItem);
        await _context.SaveChangesAsync(cancellationToken);
        return taskItem.Id;
    }
}
