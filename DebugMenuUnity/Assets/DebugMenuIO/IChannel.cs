#nullable enable
using System;
using Newtonsoft.Json.Linq;

public interface IChannel {
    void SendJson(object payload);
    void SendBinary(ReadOnlyMemory<byte> bytes);

    event Action<JObject> ReceivedJson;
    event Action<ReadOnlyMemory<byte>> ReceivedBytes;
}
