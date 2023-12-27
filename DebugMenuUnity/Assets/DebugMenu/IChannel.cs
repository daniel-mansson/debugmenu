#nullable enable
using System;

public interface IChannel {
    void SendJson(object payload);
    void SendBinary(ReadOnlyMemory<byte> bytes);

    event Action<Unity.Plastic.Newtonsoft.Json.Linq.JObject> ReceivedJson;
    event Action<ReadOnlyMemory<byte>> ReceivedBytes;
}