using DebugMenu.Silo.Web.Users.Requests;
using MediatR;

namespace DebugMenu.Silo.Web.Users;

public static class UsersEndpoints {
    public static WebApplication MapUsersEndpoints(this WebApplication app) {
        var root = app.MapGroup("/api/users")
            .WithTags("users");
        
        root.MapGet("/", GetAll);

        return app;
    }

    private static async Task<IResult> GetAll(IMediator mediator) {
        return Results.Ok(await mediator.Send(new GetAllUsersRequest()));
    }
}