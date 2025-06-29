namespace TaskTimeTracker.Web.Endpoints;

public class TaskSessions : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app
            .MapGroup(this);
    }
}
