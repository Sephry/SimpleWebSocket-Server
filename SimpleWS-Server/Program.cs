using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:6969");

var app = builder.Build();
app.UseWebSockets();

List<Notificaitons> datas = new List<Notificaitons>();

datas.Add(new Notificaitons
                {
                    Id = "468d9d99-b195-4b6a-944e-a0b468747c91",
                    Status = 0,
                    Type = "excelimportcomplete",
                    WebUrl = "updatedata/ST/",
                    TargetEntityId = "",
                    Description = "•Poyraz RES santraline ait veri -> EAK (MW) - 2023090703 /  Güncel Veri : 25.0\n•Poyraz RES santraline ait veri -> EAK (MW) - 2023090702 /  Güncel Veri : 25.0\n•Poyraz RES santraline ait veri -> EAK (MW) - 2023090701 /  Güncel Veri : 25.0\n•Poyraz RES santraline ait veri -> EAK (MW) - 2023090700 /  Güncel Veri : 25.0\n"
                });
datas.Add(new Notificaitons
            {
                Id = "468d9d99-b195-4b6a-944e-a0b468747c91",
                Status = 0,
                Type = "excelimportcomplete",
                WebUrl = "updatedata/ST/",
                TargetEntityId = "",
                Description = "•Poyraz RES santraline ait veri -> EAK (MW) - 2023090703 /  Güncel Veri : 25.0\n•Poyraz RES santraline ait veri -> EAK (MW) - 2023090702 /  Güncel Veri : 25.0\n•Poyraz RES santraline ait veri -> EAK (MW) - 2023090701 /  Güncel Veri : 25.0\n•Poyraz RES santraline ait veri -> EAK (MW) - 2023090700 /  Güncel Veri : 25.0\n"
            });

var responseNotification = new NotificaitonList
                           {
                               ErrorCode = 0,
                               ResultMessage = "",
                               Success = false,
                               Data = datas
                            };


app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        while (true)
        {
            var message = "The current time is: " + DateTime.Now.ToString("HH:mm:ss");
            var bytes = Encoding.UTF8.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);

            string jsonString = JsonSerializer.Serialize(responseNotification);
            var bytes2 = Encoding.UTF8.GetBytes(jsonString);
            var arraySegment2 = new ArraySegment<byte>(bytes2, 0, bytes2.Length);

            if (ws.State == WebSocketState.Open)
            {
                await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

                await ws.SendAsync(arraySegment2, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
            {
                break;
            }
            Thread.Sleep(5000);
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});



// ÇOKLU WEBSOCKET SERVER

/*
var connections = new List<WebSocket>();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var curName = context.Request.Query["name"];

        using var ws = await context.WebSockets.AcceptWebSocketAsync();

        connections.Add(ws);

        await Broadcast($"{curName} joined the room");
        await Broadcast($"{connections.Count} users connected");
        await ReciveMessage(ws, async (result, buffer) =>
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                await Broadcast(curName + ": " + message);
            }
            else if (result.MessageType == WebSocketMessageType.Close || ws.State == WebSocketState.Aborted)
            {
                connections.Remove(ws);
                await Broadcast($"{curName} left the room");
                await Broadcast($"{connections.Count} users connected");
                await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
        });
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

async Task ReciveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
{
    var buffer = new byte[1024 * 4];
    while (socket.State == WebSocketState.Open)
    {
        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        handleMessage(result, buffer);
    }
}

async Task Broadcast(string message)
{
    var bytes = Encoding.UTF8.GetBytes(message);
    foreach (var socket in connections)
    {
        if (socket.State == WebSocketState.Open) 
        {
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
*/
await app.RunAsync();

public class NotificaitonList
{
    public int ErrorCode { get; set; }
    public bool Success { get; set; }
    public string ResultMessage { get; set; } = "";
    public List<Notificaitons> Data { get; set; }


}
public class Notificaitons
{
    public string Id { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
    public string WebUrl { get; set; }
    public string TargetEntityId { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
}

