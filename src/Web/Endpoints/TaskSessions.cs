using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskTimeTracker.Application.TaskSessions.Commands.StartTaskSession;
using TaskTimeTracker.Application.TaskSessions.Commands.StopTaskSession;
using TaskTimeTracker.Application.TaskSessions.Queries;
using TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionDuration;
using TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionsHistory;
using TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionsTotalDuration;
using TaskTimeTracker.Domain.Exceptions.TaskItem;
using TaskTimeTracker.Domain.Exceptions.TaskSession;

namespace TaskTimeTracker.Web.Endpoints;

public class TaskSessions : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app
            .MapGroup(this)
            .RequireAuthorization()
            .MapPost(StartTaskSession, "/start/{taskId}")
            .MapPost(StopTaskSession, "/stop/{taskId}")
            .MapGet(GetTaskSessionDuration, "{taskId}")
            .MapGet(GetTaskSessionsTotalDuration, "/total/{taskId}")
            .MapGet(GetTaskSessionsHistory, "/history/{taskId}");
    }

    public async Task<Results<Ok, BadRequest<string>, NotFound<string>>> StartTaskSession(ISender sender, [FromRoute] int taskId)
    {
        var command = new StartTaskSessionCommand
        {
            TaskId = taskId
        };

        try
        {
            await sender.Send(command);
            return TypedResults.Ok();
        }
        catch (TaskItemNotFoundException)
        {
            return TypedResults.NotFound("No task was found.");
        }
        catch (StartedTaskSessionAlreadyExistsException)
        {
            return TypedResults.BadRequest("A task session for this task is already in progress.");
        }
    }

    public async Task<Results<Ok, NotFound<string>>> StopTaskSession(ISender sender, [FromRoute] int taskId)
    {
        var command = new StopTaskSessionCommand
        {
            TaskId = taskId
        };

        try
        {
            await sender.Send(command);
            return TypedResults.Ok();
        }
        catch (TaskItemNotFoundException)
        {
            return TypedResults.NotFound("No task was found.");
        }
        catch (NoStartedTaskSessionWasFoundException)
        {
            return TypedResults.NotFound("No started task session was found for this task.");
        }
    }

    public async Task<Results<Ok<int>, NotFound<string>>> GetTaskSessionDuration(ISender sender, [FromRoute] int taskId)
    {
        var query = new GetTaskSessionDurationQuery
        {
            TaskId = taskId
        };

        try
        {
            var duration = await sender.Send(query);
            return TypedResults.Ok(duration);
        }
        catch (NoStartedTaskSessionWasFoundException)
        {
            return TypedResults.NotFound("No started task session was found exception.");
        }
    }

    public async Task<Ok<int>> GetTaskSessionsTotalDuration(ISender sender, [FromRoute] int taskId)
    {
        var query = new GetTaskSessionsTotalDurationQuery
        {
            TaskId = taskId
        };

        var duration = await sender.Send(query);
        return TypedResults.Ok(duration);
    }

    public async Task<Ok<TaskSessionsDto>> GetTaskSessionsHistory(ISender sender, [FromRoute] int taskId)
    {
        var query = new GetTaskSessionsHistoryQuery
        {
            TaskId = taskId
        };

        var taskSessions = await sender.Send(query);

        return TypedResults.Ok(taskSessions);
    }
}
