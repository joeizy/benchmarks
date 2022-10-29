using Azure.Messaging.ServiceBus;

Console.WriteLine("Hello, World!");
Console.WriteLine("Receiveing Service Bus");


// SB Premium
string connectionString = "";
string queueName = "";

await using var client = new ServiceBusClient(connectionString);

var options = new ServiceBusSessionProcessorOptions
{
    AutoCompleteMessages = false,
    MaxConcurrentSessions = 20,
    MaxConcurrentCallsPerSession = 1,
};

await using ServiceBusSessionProcessor processor = client.CreateSessionProcessor(queueName, options);
processor.ProcessMessageAsync += MessageHandler;
processor.ProcessErrorAsync += ErrorHandler;

async Task MessageHandler(ProcessSessionMessageEventArgs args)
{
    if (args.Message.SessionId.ToLower() == "warmup")
    {
        Console.WriteLine("Warmup Message Received");
        await args.CompleteMessageAsync(args.Message);
        return;
    }

    var body = args.Message.Body.ToString();
    var acct = System.Text.Json.JsonSerializer.Deserialize<Account>(body);
    await args.CompleteMessageAsync(args.Message);

    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff},Watch,{acct!.account_id}");
}

Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.ErrorSource);
    Console.WriteLine(args.FullyQualifiedNamespace);
    Console.WriteLine(args.EntityPath);
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}

await processor.StartProcessingAsync();
Console.ReadLine();

class Account
{
    public string _id { get; set; }
    public int account_id { get; set; }
    public int limit { get; set; }
    public string[] products { get; set; }
}
