// See https://aka.ms/new-console-template for more information

using MongoDB.Bson;
using MongoDB.Driver;

Console.WriteLine("Hello, World!");

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

using (var watch = collection.Watch())
{
    Console.WriteLine("Watching");
    foreach(var item in watch.ToEnumerable())
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff},Watch,{item.FullDocument.account_id}");
    }
}

class Account
{
    public ObjectId _id { get; set; }
    public int account_id { get; set; }
    public int limit { get; set; }
    public string[] products { get; set; }
}
