using DebugMenu.Silo.Web.Applications.Requests.CreateApplication;
using DebugMenu.Silo.Web.Applications.Requests.DeleteApplication;
using DebugMenu.Silo.Web.Applications.Requests.GetAllApplications;
using DebugMenu.Silo.Web.Applications.Requests.GetApplication;
using DebugMenu.Silo.Web.Applications.Requests.GetApplicationsByUser;
using DebugMenu.Silo.Web.Applications.Requests.GetUsersInApplication;
using DebugMenu.Silo.Web.Teams.Requests.CreateTeam;
using DebugMenu.Silo.Web.Teams.Requests.DeleteTeam;
using DebugMenu.Silo.Web.Teams.Requests.GetAllTeams;
using DebugMenu.Silo.Web.Teams.Requests.GetTeam;
using DebugMenu.Silo.Web.Teams.Requests.GetTeamsByUser;
using DebugMenu.Silo.Web.Teams.Requests.GetUsersInTeam;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DebugMenu.Silo.Web.Teams;

public static class TeamsEndpoints {
    public static WebApplication MapTeamsEndpoints(this WebApplication app) {
        var root = app.MapGroup("/api/teams")
            .WithTags("teams");

        root.MapGet("/", GetAll);
        root.MapGet("/{id}", Get);
        root.MapGet("/by-user/{id}", GetByUser);
        root.MapGet("/{id}/users", GetUsers);

        root.MapPost("/", Create);

        root.MapPut("/", Update);

        root.MapDelete("/{id}", Delete);

        return app;
    }

    private static async Task<IResult> Get(int id, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetTeamRequest() {
            Id = id
        }));
    }

    private static async Task<IResult> Delete(int id, IMediator mediator) {
        await mediator.Send(new DeleteTeamRequest() {
            Id = id
        });
        return Results.Ok();
    }

    private static Task Update(HttpContext context) {
        throw new NotImplementedException();
    }

    private static async Task<IResult> Create([FromBody]CreateTeamRequest request, IMediator mediator) {
        return Results.Ok(await mediator.Send(request));
    }

    private static async Task<IResult> GetAll(IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetAllTeamsRequest()));
    }

    private static async Task<IResult> GetByUser(string id, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetTeamsByUserRequest() {
            UserId = id
        }));
    }

    private static async Task<IResult> GetUsers(int id, IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetUsersInTeamRequest() {
            TeamId = id
        }));
    }
}
