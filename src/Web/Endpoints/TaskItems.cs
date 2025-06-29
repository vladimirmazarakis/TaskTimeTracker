
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;
using TaskTimeTracker.Application.TaskItems.Commands.DeleteTaskItem;
using TaskTimeTracker.Application.TaskItems.Commands.UpdateTaskItem;
using TaskTimeTracker.Application.TaskItems.Queries;
using TaskTimeTracker.Application.TaskItems.Queries.GetAllTaskItems;
using TaskTimeTracker.Domain.Exceptions.TaskItem;

namespace TaskTimeTracker.Web.Endpoints;

public class TaskItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app
            .MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetAllTaskItems)
            .MapPost(CreateTask)
            .MapPut(UpdateTask, "")
            .MapDelete("/{id}", DeleteTask);
    }

    public async Task<Created<int>> CreateTask(ISender sender, [FromBody] CreateTaskItemCommand createTaskItem)
    {
        var result = await sender.Send(createTaskItem);
        return TypedResults.Created<int>(string.Empty, result);
    }

    public async Task<Results<Ok<UpdatedTaskItemDto>, NotFound>> UpdateTask(ISender sender, [FromBody] UpdateTaskItemCommand updateTaskItem)
    {
        try
        {
            var result = await sender.Send(updateTaskItem);
            return TypedResults.Ok<UpdatedTaskItemDto>(result);
        }
        catch(TaskItemNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    public async Task<Results<Ok<int>, NotFound>> DeleteTask(ISender sender, [FromRoute] int id)
    {
        try
        {
            var delCommand = new DeleteTaskItemCommand() 
            {
                Id = id
            };
            var result = await sender.Send(delCommand);
            return TypedResults.Ok<int>(result);
        }
        catch (TaskItemNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    public async Task<TaskItemsDto> GetAllTaskItems(ISender sender)
    {
        var query = new GetAllTaskItemsQuery();
        var result = await sender.Send(query);

        return result;
    }
}
