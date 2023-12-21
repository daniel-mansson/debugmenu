using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DebugMenu.ProducerClientSdk;



public class Producer {
    private readonly string _url;
    private readonly string _clientToken;
    private HttpClient _client;
    private string _instanceId;
    private ClientWebSocket _webSocket;

    public string SessionId => _instanceId;

    public Producer(string url, string clientToken) {
        _url = url;
        _clientToken = clientToken;
        _client = new HttpClient();
        //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientToken);
    }

    public async Task<bool> Start(Dictionary<string, string> metadata) {
        var body = JsonContent.Create(new StartInstanceRequestDto(metadata));
        var response = await _client.PostAsync(new Uri(_url + "/instance/start"), body);

        if (response.StatusCode != HttpStatusCode.OK)
            return false;

        var json = await response.Content.ReadAsStringAsync();
        var responseBody = JsonSerializer.Deserialize<StartInstanceResponseDto>(json, new JsonSerializerOptions() {
            PropertyNameCaseInsensitive = true
        });
        if (responseBody == null)
            return false;

        _instanceId = responseBody.InstanceId;

        _webSocket = new ClientWebSocket();
        await _webSocket.ConnectAsync(new Uri(responseBody.WebsocketUrl + "/instance"), CancellationToken.None);

        return _webSocket.State == WebSocketState.Open;
    }

    public async Task<bool> Rejoin(string websocketUrl, string id) {
        _instanceId = id;

        _webSocket = new ClientWebSocket();
        await _webSocket.ConnectAsync(new Uri(websocketUrl + "/instance"), CancellationToken.None);

        return _webSocket.State == WebSocketState.Open;
    }

    public async Task<byte[]> Receive() {
        var buffer = new byte[4096 * 20];
        var r = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        Console.WriteLine("Recv " + r.Count);
        return buffer;
    }

    public void Send(string text) {
        _webSocket.SendAsync(
            new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(text)),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }

    public record StartInstanceRequestDto(
        Dictionary<string, string> Metadata);

    public record StartInstanceResponseDto(
        string InstanceId,
        string WebsocketUrl);
}