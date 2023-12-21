using System.Net.WebSockets;

namespace DebugMenu.Silo.Abstractions; 

public interface IDebugInstanceGrain : IGrainWithStringKey {
    public Task<StartInstanceResponseDto> StartInstance(StartInstanceRequestDto request);
    public Task<string> GetWebSocketUrl();

}