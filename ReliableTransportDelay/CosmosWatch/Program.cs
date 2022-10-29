using Microsoft.Azure.Cosmos;

Console.WriteLine("Hello, World!");

using CosmosClient client = new(
    accountEndpoint: "",
    authKeyOrResourceToken: ""
);

string databaseName = "";
string sourceContainerName = "";
string leaseContainerName = "";

Container leaseContainer = client.GetContainer(databaseName, leaseContainerName);
ChangeFeedProcessor changeFeedProcessor = client.GetContainer(databaseName, sourceContainerName)
    .GetChangeFeedProcessorBuilder<Account>(processorName: "changeFeedSample", onChangesDelegate: HandleChangesAsync)
        .WithInstanceName("consoleHost")
        .WithLeaseContainer(leaseContainer)
        .Build();

await changeFeedProcessor.StartAsync();
Console.WriteLine("Change Feed Processor started.");

Console.ReadLine();
await changeFeedProcessor.StopAsync().WaitAsync(TimeSpan.FromSeconds(5));

static Task HandleChangesAsync(ChangeFeedProcessorContext context, IReadOnlyCollection<Account> changes, CancellationToken cancellationToken)
{
    foreach (Account item in changes)
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff},Watch,{item.account_id}");
    }

    return Task.CompletedTask;
}

class Account
{
    public string id { get; set; }
    public int account_id { get; set; }
    public int limit { get; set; }
    public string[] products { get; set; }
}
