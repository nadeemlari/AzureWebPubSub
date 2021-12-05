using Azure.Messaging.WebPubSub;

const string connectionString = "Endpoint=https://izaan.webpubsub.azure.com;AccessKey=7+rZAXhhRR+eXQqZ3Mby1dNsulbY0JMQIeO2NdZnDXE=;Version=1.0;";
const string hub = "izaan.webpubsub.azure.com";
var client = new WebPubSubServiceClient(connectionString,hub);
while (true)
{
    Console.WriteLine("Enter message");
    var msg = Console.ReadLine();
    await client.SendToAllAsync(msg);
}