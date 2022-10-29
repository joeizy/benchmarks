// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Driver;

Console.WriteLine("Hello, World! Enter to Start Writing to Mongo");
Console.ReadLine();
Console.WriteLine("Inserting");

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

var rand = new Random();

while (true)
{
    int id = rand.Next();
    collection.InsertOne(new Account
    {
        account_id = id,
        limit = 99999,
        products = new[] { "apple", "orange" }
    });
    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff},Insert,{id}");
    Thread.Sleep(1);
}

class Account
{
    public ObjectId _id { get; set; }
    public int account_id { get; set; }
    public int limit { get; set; }  
    public string[] products { get; set; }
}
