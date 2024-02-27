#nullable enable
using System;
using Newtonsoft.Json.Linq;

public interface IReceiver {
    event Action<(string channel, JObject payload)> ReceivedJson;
    event Action<(string channel, ReadOnlyMemory<byte> payload)> ReceivedBytes;
}
