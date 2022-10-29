
using Azure.Messaging.ServiceBus;
using MongoDB.Bson;
using MongoDB.Driver;

Console.WriteLine("Hello, World!");

/////////////// Mongo Setup
string user = "";
string password = "";
string server = "";
string dbName = "";
string collectionName = "";

var settings = MongoClientSettings.FromConnectionString($"mongodb+srv://{user}:{password}@{server}/?retryWrites=true&w=majority");
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);
var database = client.GetDatabase(dbName);
var collection = database.GetCollection<Account>(collectionName);

//////////// Service Bus Setup
string connectionString = "";
string queueName = "";

await using var serviceBus = new ServiceBusClient(connectionString);
ServiceBusSender sender = serviceBus.CreateSender(queueName);

////////////// Start Watching
sender.SendMessageAsync(new ServiceBusMessage("Warmup Mesasge") { SessionId = "Warmup" }).Wait();

using (var watch = collection.Watch())
{
    Console.WriteLine("Watching Mongo");
    int i = 0;
    const int numberOfSessionsToRoundRobin = 20;

    //////////// Processing
    foreach (var item in watch.ToEnumerable())
    {
        string msgText = $@"
{{
    ""_id"": ""{Guid.NewGuid()}"",
    ""account_id"": {item.FullDocument.account_id},
    ""limit"": 99999,
    ""products"": [""apple"", ""orange""]
}}
";
        var msg = new ServiceBusMessage(msgText)
        {
            SessionId = $"Session{i % numberOfSessionsToRoundRobin}" // round robin messages across sessions
        };

        sender.SendMessageAsync(msg).Wait();

        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff},MongoWatchSendSb,{item.FullDocument.account_id}");
        i++;
    }
}

class Account
{
    public ObjectId _id { get; set; }
    public int account_id { get; set; }
    public int limit { get; set; }
    public string[] products { get; set; }
}
