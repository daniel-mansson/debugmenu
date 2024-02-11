// See https://aka.ms/new-console-template for more information

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DebugMenu.Silo.Abstractions;
using DebugMenu.Silo.Common;
using DebugMenu.Silo.Grains;
using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Web.Applications;
using DebugMenu.Silo.Web.Applications.Persistence;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;
using DebugMenu.Silo.Web.RunningInstances;
using DebugMenu.Silo.Web.RuntimeTokens;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Teams;
using DebugMenu.Silo.Web.Teams.Persistence;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Users;
using DebugMenu.Silo.Web.Users.Persistence;
using DebugMenu.Silo.Web.Users.Persistence.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var reference = typeof(DebugInstanceGrain);

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://localhost:8082");

var websocketManager = new WebsocketManager();

// Configure the host
using var host = new HostBuilder()
    .UseOrleans(siloBuilder => {
        siloBuilder
            .UseLocalhostClustering()
            .AddMemoryGrainStorage(Constants.DefaultStore)
            .AddMemoryGrainStorage("PubSubStore")
            .AddMemoryStreams(Constants.InMemoryStream)
            .AddMemoryGrainStorageAsDefault()
            .UseInMemoryReminderService()
            .UseDashboard();

        // siloBuilder.Configure<GrainCollectionOptions>(options => {
        //     options.CollectionAge = TimeSpan.FromSeconds(15);
        //     options.CollectionQuantum = TimeSpan.FromSeconds(5);
        // });
    })
    .ConfigureServices(serviceCollection => serviceCollection
        .AddSingleton<ILocalWebsocketUrlProvider, LocalWebsocketUrlProvider>()
        .AddSingleton<IWebsocketManager>(websocketManager))
    .Build();


// Start the host
await host.StartAsync();

var clusterClient = host.Services.GetService<IClusterClient>()!;


builder.Services
    .AddLogging(e =>
        e.AddConsole()
            .AddFilter(level => level >= LogLevel.Information)
    )
    .AddSingleton(clusterClient)
    .AddSingleton<ILocalWebsocketUrlProvider, LocalWebsocketUrlProvider>()
    .AddSingleton<IWebsocketManager>(websocketManager)
    .AddControllers();

builder.Services
    .AddDbContext<DebugMenuDbContext>(options => {
        // options.UseNpgsql(
        //     builder.Configuration.GetConnectionString(
        //         "Host=localhost;Port=5432;Database=debugmenue;Username=postgres;Password=postgres;Include Error Detail=true;")
        //     !,
        //     serverOptions => serverOptions
        //         .EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null)
        //         .MigrationsAssembly("DebugMenu.Silo")
        // );
    });

string jwtSecretKey = "ed5b824bd6f6a2db591c0273d9fb176decae1c5f6b86e6ab03df741815827e90";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
key.KeyId = "dm";

builder
    .Services
    .AddAuthorization()
    .AddAuthentication(options => {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidIssuer = "debugmenu",
        ValidateAudience = true,
        ValidAudience = "http://debugmenu.io",
        IssuerSigningKey = key,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = false
    });

builder.Services.AddScoped<IJwtService, DebugMenuIoJwtService>();

builder.Services.AddScoped<IApplicationsRepository, ApplicationsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITeamsRepository, TeamsRepository>();
builder.Services.AddScoped<IRuntimeTokenRepository, RuntimeTokenRepository>();

builder.Services.AddAutoMapper(_ => { }, typeof(Program).Assembly);

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

app.UseCors(b => {
    b.WithOrigins("http://localhost:5173",
            "http://localhost:8082",
            "https://localhost:8082",
            "http://192.168.10.180:5173")
        .AllowAnyMethod()
        .AllowCredentials()
        .AllowAnyHeader();
});

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/get-jwt", () => {
    Claim[] claims = new Claim[] {
        new Claim("Id", "1"),
        new Claim("Name", "daniel"),
        new Claim("Email", "d@d.d"),
    };

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));

    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new
        JwtSecurityToken(
            "debugmenu",
            "http://debugmenu.io",
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signingCredentials);

    return Results.Ok(new JwtSecurityTokenHandler().WriteToken(token));
});

app.MapGet("/version", () => "1.0.0")
    .RequireAuthorization();

app.MapApplicationsEndpoints();
app.MapUsersEndpoints();
app.MapRuntimeTokensEndpoints();
app.MapRunningInstancesEndpoints();
app.MapTeamsEndpoints();

app.MapPost("/instance/start", async (StartInstanceRequestDto request, IClusterClient clusterClient) =>
    await clusterClient.GetGrain<IDebugInstanceGrain>(Guid.NewGuid().ToString())
        .StartInstance(request));

app.UseWebSockets();
app.Map("/ws", ws => ws.UseMiddleware<WebSocketPubSubMiddleware>());

app.MapControllers();

await app.StartAsync();


Console.WriteLine("Orleans is running.\nPress Enter to terminate...");

while(!Console.KeyAvailable) {
    await Task.Delay(1000);
}

Console.WriteLine("Orleans is stopping...");

await host.StopAsync();
