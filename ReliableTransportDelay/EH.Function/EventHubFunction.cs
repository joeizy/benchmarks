using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EH.Function
{
    public class EventHubFunction
    {
        private readonly ILogger _logger;

        public EventHubFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EventHubFunction>();
        }

        [Function("EventHubFunction")]
        public void Run([EventHubTrigger("%Hub_Name%", Connection = "Hub_Namespace_Connection_String")] string[] input, string[] partitionKeyArray, FunctionContext context)
        {
            var now = DateTime.Now;
            for (int i = 0; i < input.Length; i++)
            {
                var sent = DateTime.Parse(input[i]);
                _logger.LogInformation($"Received: {now.ToLongTimeString()}, Sent: {sent}, Delay: {(now - sent).TotalMilliseconds:N2}ms, PK: {partitionKeyArray[i]}");
            }
        }
    }
}
