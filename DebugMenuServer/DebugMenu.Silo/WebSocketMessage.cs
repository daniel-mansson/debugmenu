[GenerateSerializer]
public class WebSocketMessage {
    [Id(0)] public string Type { get; set; }
    [Id(1)] public byte[]? Payload { get; set; }
}