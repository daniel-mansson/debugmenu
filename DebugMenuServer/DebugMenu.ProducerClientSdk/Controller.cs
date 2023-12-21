using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DebugMenu.ProducerClientSdk;

public class Controller {
    private readonly string _url;
    private readonly string _clientToken;
    private HttpClient _client;
    private string _instanceId;
    private ClientWebSocket _webSocket;

    public Controller(string url, string clientToken) {
        _url = url;
        _clientToken = clientToken;
        _client = new HttpClient();
        //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientToken);
    }

    public async Task<bool> Start(Dictionary<string, string> metadata) {
        _webSocket = new ClientWebSocket();
        await _webSocket.ConnectAsync(new Uri(_url + "/controller"), CancellationToken.None);

        return _webSocket.State == WebSocketState.Open;
    }

    public async Task<byte[]> Receive() {
        var buffer = new byte[4096 * 20];
        var r = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        Console.WriteLine("C Recv " + r.Count); 
        return buffer;
    }

    public record StartInstanceRequestDto(
        Dictionary<string, string> Metadata);

    public record StartInstanceResponseDto(
        string InstanceId,
        string WebsocketUrl);

    public void Send(string text) {
        _webSocket.SendAsync(
            new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(text)),
            WebSocketMessageType.Binary, 
            true,
            CancellationToken.None);
    }
}