using DebugMenu.Silo.Web.Applications.Requests.GetAllApplications;
using DebugMenu.Silo.Web.RunningInstances.Requests.GetRunningInstancesByTokenId;
using DebugMenu.Silo.Web.RuntimeTokens.Requests.CreateRuntimeToken;
using DebugMenu.Silo.Web.RuntimeTokens.Requests.DeleteRuntimeToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DebugMenu.Silo.Web.RuntimeTokens; 

public static class RuntimeTokensEndpoints {
    public static WebApplication MapRuntimeTokensEndpoints(this WebApplication app) {
        var root = app.MapGroup("/api/tokens")
            .WithTags("tokens");

        root.MapGet("/", GetAll);
        root.MapGet("/{id}/instances", GetAllInstances);
        root.MapPost("/", Create);
        root.MapDelete("/{id}", Delete);

        return app;
    }

    private static async Task<IResult> GetAllInstances(int id, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetRunningInstancesByTokenIdRequest() {
            Id = id
        }));
    }

    private static async Task<IResult> Delete(int id, IMediator mediator) {
        await mediator.Send(new DeleteRuntimeTokenRequest() {
            Id = id
        });
        return Results.Ok();
    }

    private static async Task<IResult> GetAll(IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetAllApplicationsRequest()));
    }
    
    private static async Task<IResult> Create([FromBody] CreateRuntimeTokenRequest request, IMediator mediator) {
        return Results.Ok(await mediator.Send(request));
    }
}