
[GenerateSerializer]
[Serializable]
public record StartInstanceRequestDto(
    string Token,
    Dictionary<string, string> Metadata,
    int ApplicationId);

[GenerateSerializer]
[Serializable]
public record StartInstanceResponseDto(
    string InstanceId,
    string WebsocketUrl);