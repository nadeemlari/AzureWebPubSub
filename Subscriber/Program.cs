using System.Threading.Channels;
using Azure.Messaging.WebPubSub;
using Websocket.Client;

const string connectionString = "Endpoint=https://izaan.webpubsub.azure.com;AccessKey=7+rZAXhhRR+eXQqZ3Mby1dNsulbY0JMQIeO2NdZnDXE=;Version=1.0;";
const string hub = "izaan.webpubsub.azure.com";
var client = new WebPubSubServiceClient(connectionString,hub);
var url = client.GetClientAccessUri();
using var ws = new WebsocketClient(url);
ws.MessageReceived.Subscribe(msg =>
{
    Console.WriteLine($"Message received: {msg}");
    
});
await ws.Start();
Console.WriteLine("I am connected");
Console.Read();