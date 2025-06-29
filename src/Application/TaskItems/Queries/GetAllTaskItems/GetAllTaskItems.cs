using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Security;

namespace TaskTimeTracker.Application.TaskItems.Queries.GetAllTaskItems;

[Authorize]
public record GetAllTaskItemsQuery : IRequest<TaskItemsDto>
{
}

public class GetAllTaskItemsQueryValidator : AbstractValidator<GetAllTaskItemsQuery>
{
    public GetAllTaskItemsQueryValidator()
    {
    }
}

public class GetAllTaskItemsQueryHandler : IRequestHandler<GetAllTaskItemsQuery, TaskItemsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;
    private readonly IMapper _mapper; 

    public GetAllTaskItemsQueryHandler(IApplicationDbContext context, IUser user, IMapper mapper)
    {
        _context = context;
        _user = user;
        _mapper = mapper;
    }

    public async Task<TaskItemsDto> Handle(GetAllTaskItemsQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _context.Tasks.Where(x => x.ApplicationUserId == _user.UserId)
            .OrderByDescending(x => x.Created)
            .ToListAsync();

        var taskItemsDto = new TaskItemsDto();
        _mapper.Map(tasks, taskItemsDto);

        return taskItemsDto;
    }
}
