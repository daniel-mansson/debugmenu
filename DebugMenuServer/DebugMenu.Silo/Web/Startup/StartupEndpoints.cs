using DebugMenu.Silo.Web.Startup.Requests.ProcessStartupEvents;
using MediatR;

namespace DebugMenu.Silo.Web.Startup;

public static class StartupEndpoints {
    public static WebApplication MapStartupEndpoints(this WebApplication app) {
        var root = app.MapGroup("/api/startup")
            .WithTags("startup");

        root.MapPost("/process-by-user/{userId}", ProcessStartupEvents);

        return app;
    }

    private static async Task<IResult> ProcessStartupEvents(string userId, IMediator mediator) {
        return Results.Ok(await mediator.Send(new ProcessStartupEventsRequest() {
            UserId = userId
        }));
    }

}
