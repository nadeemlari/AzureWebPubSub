using Websocket.Client;

Console.WriteLine("Enter user id");
var userid = Console.ReadLine();
var url = await GetWebSocketUrlAsync(userid);
Console.WriteLine($"Url : {url}");
Console.WriteLine("Starting .......");
using var wsc = new WebsocketClient(new Uri(url));
wsc.ReconnectTimeout = null;
wsc.MessageReceived.Subscribe(OnMessageReceive);
await wsc.Start();
Console.WriteLine("Connecting to server");
while (true)
{
    var text = Console.ReadLine();
    wsc.Send(text);
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