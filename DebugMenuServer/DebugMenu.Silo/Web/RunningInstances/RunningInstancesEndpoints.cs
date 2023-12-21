using DebugMenu.Silo.Web.RunningInstances.Requests.CreateRunningInstance;
using DebugMenu.Silo.Web.RunningInstances.Requests.GetRunningInstance;
using DebugMenu.Silo.Web.RuntimeTokens.Requests.GetRuntimeTokensByApplication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DebugMenu.Silo.Web.RunningInstances; 

public static class RunningInstancesEndpoints {
    public static WebApplication MapRunningInstancesEndpoints(this WebApplication app) {
        var root = app.MapGroup("/api/instances")
            .WithTags("instances");

        root.MapGet("/", GetAllByToken);
        root.MapGet("/{id}", Get);
        root.MapPost("/", Create);

        return app;
    }

    private static async Task<IResult> Create([FromBody] CreateRunningInstanceRequest request, IMediator mediator) {
        return Results.Ok(await mediator.Send(request));
    }

    private static async Task<IResult> Get(string id, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetRunningInstanceRequest() {
            Id = id
        }));
    }

    private static async Task<IResult> GetAllByToken([FromQuery] string token, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetRuntimeTokensByApplicationRequest() {
            Token = token
        }));
    }
}