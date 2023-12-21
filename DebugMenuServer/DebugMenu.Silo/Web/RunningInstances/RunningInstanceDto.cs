namespace DebugMenu.Silo.Web.RunningInstances;

[GenerateSerializer]
[Serializable]
public class RunningInstanceDto {
    [Id(0)]
    public string Id { get; set; } = string.Empty;
    [Id(1)]
    public string? DeviceId { get; set; }
    [Id(2)]
    public string? WebsocketUrl { get; set; }
    [Id(3)]
    public int ConnectedViewers { get; set; }
    [Id(4)]
    public bool HasConnectedInstance { get; set; }
    [Id(5)]
    public int ApplicationId { get; set; }
}