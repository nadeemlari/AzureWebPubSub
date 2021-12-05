using System.Net.WebSockets;
using System.Text.Json;
using Websocket.Client;

Console.WriteLine("Enter user id");
var userid = Console.ReadLine();
var url = await GetWebSocketUrlAsync(userid);
Console.WriteLine($"Url : {url}");
Console.WriteLine("Starting .......");

using var wsc = new WebsocketClient(new Uri(url), () =>
{
    var inner = new ClientWebSocket();
    inner.Options.AddSubProtocol("json.webpubsub.azure.v1");
    return inner;
});
wsc.ReconnectTimeout = null;
wsc.MessageReceived.Subscribe(OnMessageReceive);
await wsc.Start();
Console.WriteLine("Connecting to server");
Console.WriteLine("joining group 'lari' ..");
var ackId = 1;
wsc.Send(JsonSerializer.Serialize(new
{
    type ="joinGroup",
    group="lari",
    ackId=ackId++
}));
while (true)
{
    var text = Console.ReadLine();
    wsc.Send(JsonSerializer.Serialize(new
    {
        type ="sendToGroup",
        group="lari",
        ackId=ackId++,
        dataType="text",
        data=text
    }));
}

static async Task<string> GetWebSocketUrlAsync(string? userid)
{
    using var httpClient = new HttpClient();
    return await httpClient.GetStringAsync($"http://localhost:5000/negotiate?id={userid}");
}

void OnMessageReceive(ResponseMessage message)
{
    Console.WriteLine(message.Text);
}