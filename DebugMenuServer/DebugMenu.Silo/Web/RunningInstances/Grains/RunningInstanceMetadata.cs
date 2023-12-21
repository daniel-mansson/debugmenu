namespace DebugMenu.Silo.Web.RunningInstances.Grains;

[GenerateSerializer]
[Serializable]
public class RunningInstanceMetadata {
    [Id(0)]
    public bool IsInitialized { get; set; }
    [Id(1)]
    public string Api { get; set; } = string.Empty;
    [Id(2)]
    public Dictionary<string, string> Metadata { get; set; } = new();
}