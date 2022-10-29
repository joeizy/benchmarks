using Microsoft.Azure.Cosmos;

Console.WriteLine("Hello, World! Enter to Start");

using CosmosClient client = new(
    accountEndpoint: "",
    authKeyOrResourceToken: ""
);

var container = client.GetContainer(
    databaseId: "",
    containerId: ""
);

var rand = new Random();

Console.ReadLine();
Console.WriteLine("Insert!");

while (true)
{
    int id = rand.Next();
    await container.CreateItemAsync(new Account
    {
        id = Guid.NewGuid().ToString(),
        account_id = id,
        limit = 99999,
        products = new[] { "apple", "orange" }
    });

    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff},Insert,{id}");
}

class Account
{
    public string id { get; set; }
    public int account_id { get; set; }
    public int limit { get; set; }
    public string[] products { get; set; }
}
