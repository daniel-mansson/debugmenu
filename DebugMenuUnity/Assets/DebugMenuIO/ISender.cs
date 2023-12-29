#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;

public interface ISender {
    Task SendJson(string channel, object payload, CancellationToken cancellationToken);
    Task SendBytes(string channel, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken);
}