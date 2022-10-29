using Azure.Messaging.ServiceBus;

Console.WriteLine("Hello, World! Enter to start.");
Console.ReadLine();
Console.WriteLine("Sending Service Bus");

// SB Standard
string connectionString = "";
string queueName = "";

await using var client = new ServiceBusClient(connectionString);
ServiceBusSender sender = client.CreateSender(queueName);

var rand = new Random();
const int numberOfSessionsToRoundRobin = 20;

for (int i = 0; ; i++)
{
    int id = rand.Next();
    string msgText = $@"
{{
    ""_id"": ""{Guid.NewGuid()}"",
    ""account_id"": {id},
    ""limit"": 99999,
    ""products"": [""apple"", ""orange""]
}}
";
    var msg = new ServiceBusMessage(msgText)
    {
        SessionId = $"Session{i % numberOfSessionsToRoundRobin}" // round robin messages across sessions
    };

    sender.SendMessageAsync(msg).Wait();
    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff},Insert,{id}");
}
