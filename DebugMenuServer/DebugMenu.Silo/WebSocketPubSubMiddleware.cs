using System.Net;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using DebugMenu.Silo.Abstractions;
using DebugMenu.Silo.Common;
using DebugMenu.Silo.Grains;
using DebugMenu.Silo.Web.RunningInstances.Grains;
using DebugMenu.Silo.Web.RunningInstances.Requests.ValidateUserAccess;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Orleans.Runtime;
using Orleans.Streams;

public class WebSocketPubSubMiddleware {
    private readonly IClusterClient _clusterClient;
    private readonly ILogger<WebSocketPubSubMiddleware> _logger;
    private readonly IWebsocketManager _websocketManager;
    private readonly IJwtService _jwtService;
    private readonly IMediator _mediator;

    public WebSocketPubSubMiddleware(
        RequestDelegate _,
        IClusterClient clusterClient,
        ILogger<WebSocketPubSubMiddleware> logger,
        IWebsocketManager websocketManager,
        IJwtService jwtService,
        IMediator mediator) {
        _logger = logger;
        _websocketManager = websocketManager;
        _jwtService = jwtService;
        _mediator = mediator;
        _clusterClient = clusterClient;
    }

    public async Task Invoke(HttpContext context) {
        if (!context.WebSockets.IsWebSocketRequest) {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        //JWT if platform websocket
        //secret if debug instance / controller 
        
        // if (context.Request.Headers.TryGetValue("SessionEntity", out var token)) {
        //     
        // }
        // else {
        //     var result = await context.AuthenticateAsync();
        //     if (!result.Succeeded) {
        //         context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //         return;
        //     }
        // }
        //ws://localhost:6666/ws/instance/[id]
        //ws://localhost:6666/ws/controller/[id]

        //ws://localhost:6666/ws/room/[id]/instance
        //ws://localhost:6666/ws/room/[id]/controller

        var path = context.Request.Path.Value!.Split("/");
        if (path[1] != "room") {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }
        
        string id = path[2].ToString();
        string role = path[3].ToString();

        try {
            if (role == "instance") {
                await HandleInstance(context, id);
            }
            else if (role == "controller") {
                await HandleController(context, id);
            }
        }
        finally {
            
        }
        //
        // var subscription = default(StreamSubscriptionHandle<WebSocketMessage>);
        //
        // try {
        //     var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        //
        //     //_websocketManager.Put(id, webSocket);
        //     try {
        //         await _clusterClient.GetGrain<IDebugInstanceGrain>(id).RegisterWebsocket(id);
        //         _logger.LogInformation("[Websocket] opened connection for UserId: {userId}", id);
        //
        //         var buffer = new byte[1024 * 4];
        //
        //         while (webSocket.CloseStatus.HasValue == false) {
        //             var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //
        //             Console.WriteLine($"[Websocket] received {result.Count} bytes");
        //         }
        //
        //         await webSocket.CloseAsync(
        //             webSocket.CloseStatus.Value,
        //             webSocket.CloseStatusDescription, CancellationToken.None);
        //
        //         _logger.LogInformation("[Websocket] closed connection for TraceId: {traceId}", id);
        //     }
        //     finally {
        //         _websocketManager.Remove(id);
        //     }
        // }
        // catch (Exception e) {
        //     _logger.LogError("[Websocket] disconnect error -> {exception}", e);
        // }
        // finally {
        //     if (subscription != null)
        //         await subscription.UnsubscribeAsync();
        // }
    }

    private async Task HandleController(HttpContext context, string id) {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var buffer = new byte[1024 * 4];
        if (!await PerformHandshakeAndValidateControllerConnection(id, webSocket)) {
            await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Failed handshake", new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);
            Console.WriteLine("Handshake failed for controller: " + id);
            return;
        }

        var room = _websocketManager.GetRoom(id);
        try {
            room.AddController(webSocket);

            _logger.LogInformation("[Websocket] opened connection for {id}", id);
        
            while (webSocket.CloseStatus.HasValue == false) {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (webSocket.CloseStatus.HasValue)
                    break;
                
                Console.WriteLine($"[Websocket] received {result.Count} bytes");
                await room.SendToInstance(buffer, result.Count, result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
        
            await webSocket.CloseAsync(
                webSocket.CloseStatus.Value,
                webSocket.CloseStatusDescription, CancellationToken.None);
        
            _logger.LogInformation("[Websocket] closed connection for TraceId: {traceId}", id);
        }
        finally {
            room.RemoveController(webSocket);
        }
    }

    private async Task<bool> PerformHandshakeAndValidateControllerConnection(string instanceId, WebSocket webSocket) {
        CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var buffer = new byte[1024];

        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
        if(result.MessageType != WebSocketMessageType.Text 
           || !result.EndOfMessage
           || result.CloseStatus.HasValue
           || result.Count > 1000) {
            return false;
        }
        
        var userJwt = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(buffer, 0, result.Count));
        var decoded = _jwtService.Decode(userJwt);
        if (decoded == null) {
            return false;
        }
        var userId = int.Parse(decoded.Id);

        var instanceGrain = _clusterClient.GetGrain<IRunningInstanceGrain>(instanceId);
        var metadata = await instanceGrain.GetMetadata();
        if (!metadata.IsInitialized) {
            return false;
        }
        
        var userHasAccess = await _mediator.Send(new ValidateUserAccessRequest() {
            UserId = userId,
            InstanceId = instanceId
        }, cts.Token);
        if (!userHasAccess) {
            return false;
        }

        if (metadata.Api != string.Empty) {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            var channelBytes = Encoding.UTF8.GetBytes("__internal/api");
            writer.Write((byte)channelBytes.Length);
            writer.Write(channelBytes);
            var payloadBytes = Encoding.UTF8.GetBytes(metadata.Api);
            writer.Write((byte)payloadBytes.Length);
            writer.Write(payloadBytes);

            var bytes = stream.ToArray();
            await webSocket.SendAsync(bytes, WebSocketMessageType.Binary, true, cts.Token);
        }
        
        return true;
    }
    private async Task<bool> PerformHandshakeAndValidateInstanceConnection(string instanceId, WebSocket webSocket) {
        CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        var buffer = new byte[2 * 1024];
        var tokenResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
        if(tokenResult.MessageType != WebSocketMessageType.Text 
           || !tokenResult.EndOfMessage
           || tokenResult.CloseStatus.HasValue
           || tokenResult.Count > 1000) {
            return false;
        }
        var token = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(buffer, 0, tokenResult.Count));
        
        var metadataResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),  cts.Token);
        if(metadataResult.MessageType != WebSocketMessageType.Text 
           || !metadataResult.EndOfMessage
           || metadataResult.CloseStatus.HasValue
           || metadataResult.Count > 1000) {
            return false;
        }
        
        var metadataJson = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(buffer, 0, metadataResult.Count));
        var metadata = JsonSerializer.Deserialize<Dictionary<string, string>>(metadataJson);
        
        return true;
    }

    private async Task HandleInstance(HttpContext context, string id) {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();

        if (!await PerformHandshakeAndValidateInstanceConnection(id, webSocket)) {
            await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Failed handshake", new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);
            return;
        }
        
        var room = _websocketManager.GetRoom(id);
        try {
            room.SetInstance(webSocket);

            _logger.LogInformation("[Websocket] opened connection for UserId: {userId}", id);
        
            var buffer = new byte[1024 * 4];
            while (webSocket.CloseStatus.HasValue == false) {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (webSocket.CloseStatus.HasValue)
                    break;
                
                Console.WriteLine($"[Websocket] received {result.Count} bytes");
                await room.BroadcastToControllersAsync(buffer, result.Count, result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
        
            await webSocket.CloseAsync(
                webSocket.CloseStatus.Value,
                webSocket.CloseStatusDescription, CancellationToken.None);
        
            _logger.LogInformation("[Websocket] closed connection for TraceId: {traceId}", id);
        }
        finally {
            room.RemoveInstance(webSocket);
        }
    }
}