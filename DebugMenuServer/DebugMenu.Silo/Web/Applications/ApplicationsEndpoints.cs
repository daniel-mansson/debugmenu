using DebugMenu.Silo.Web.Applications.Requests.CreateApplication;
using DebugMenu.Silo.Web.Applications.Requests.DeleteApplication;
using DebugMenu.Silo.Web.Applications.Requests.GetAllApplications;
using DebugMenu.Silo.Web.Applications.Requests.GetApplication;
using DebugMenu.Silo.Web.Applications.Requests.GetApplicationsByUser;
using DebugMenu.Silo.Web.RuntimeTokens;
using DebugMenu.Silo.Web.RuntimeTokens.Requests.CreateRuntimeToken;
using DebugMenu.Silo.Web.RuntimeTokens.Requests.GetRuntimeTokensByApplication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DebugMenu.Silo.Web.Applications;

public static class ApplicationsEndpoints {

    public static WebApplication MapApplicationsEndpoints(this WebApplication app) {
        var root = app.MapGroup("/api/applications")
            .WithTags("applications");

        root.MapGet("/", GetAll);
        root.MapGet("/{id}", Get);
        root.MapGet("/by-team/{id}", GetByTeam);

        root.MapGet("/{id}/tokens", GetTokens);
        root.MapPost("/{id}/tokens", CreateToken);

        root.MapPost("/", Create);

        root.MapPut("/", Update);

        root.MapDelete("/{id}", Delete);

        return app;
    }

    private static async Task<IResult> CreateToken(int id, [FromBody] RuntimeTokenMutableDto initialData, IMediator mediator) {
        return Results.Ok(await mediator.Send(new CreateRuntimeTokenRequest() {
            ApplicationId = id,
            InitialData = initialData
        }));
    }

    private static async Task<IResult> GetTokens(int id, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetRuntimeTokensByApplicationRequest() {
            ApplicationId = id
        }));
    }

    private static async Task<IResult> Get(int id, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetApplicationRequest() {
            Id = id
        }));
    }

    private static async Task<IResult> Delete(int id, IMediator mediator) {
        await mediator.Send(new DeleteApplicationRequest() {
            Id = id
        });
        return Results.Ok();
    }

    private static Task Update(HttpContext context) {
        throw new NotImplementedException();
    }

    private static async Task<IResult> Create([FromBody]CreateApplicationRequest request, IMediator mediator) {
        return Results.Ok(await mediator.Send(request));
    }

    private static async Task<IResult> GetAll(IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetAllApplicationsRequest()));
    }

    private static async Task<IResult> GetByTeam(int id, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetApplicationsByUserRequest() {
            TeamId = id
        }));
    }

}
