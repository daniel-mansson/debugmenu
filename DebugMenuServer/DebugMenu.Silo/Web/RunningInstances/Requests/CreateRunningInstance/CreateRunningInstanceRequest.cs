using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.CreateRunningInstance;

public class CreateRunningInstanceRequest : IRequest<RunningInstanceDto> {
    public string Token { get; set; } = string.Empty;
    public Dictionary<string, string> Metadata { get; set; } = new();
}