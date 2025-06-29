using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;
using TaskTimeTracker.Domain.Enums;
using TaskTimeTracker.Domain.Exceptions.TaskItem;

namespace TaskTimeTracker.Application.TaskItems.Commands.UpdateTaskItem;

[Authorize]
public record UpdateTaskItemCommand : IRequest<UpdatedTaskItemDto>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public PriorityLevel Priority { get; set; }
    public bool IsCompleted { get; set; }
}

public class UpdateTaskItemCommandValidator : AbstractValidator<UpdateTaskItemCommand>
{
    public UpdateTaskItemCommandValidator()
    {
    }
}

public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, UpdatedTaskItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;
    private readonly IMapper _mapper;

    public UpdateTaskItemCommandHandler(IApplicationDbContext context, IUser user, IMapper mapper)
    {
        _context = context;
        _user = user;
        _mapper = mapper;
    }

    public async Task<UpdatedTaskItemDto> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var taskItem = _context.Tasks.FirstOrDefault(x => x.Id == request.Id && x.ApplicationUserId == _user.UserId);
        if(taskItem is null) 
        {
            throw new TaskItemNotFoundException(request.Id);
        }

        taskItem.Name = request.Name ?? taskItem.Name;
        taskItem.Description = request.Description ?? taskItem.Description;
        taskItem.Priority = request.Priority != default ? request.Priority : taskItem.Priority;
        taskItem.IsCompleted = request.IsCompleted;

        await _context.SaveChangesAsync(cancellationToken);
        var updatedTaskItemDto = new UpdatedTaskItemDto();
        _mapper.Map(taskItem, updatedTaskItemDto);

        return updatedTaskItemDto;
    }
}
