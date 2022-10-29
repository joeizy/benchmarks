# Reliable Transport Delay Benchmarks
Benchmarks for services which reliably transfer data from a producer to a consumer such as Queus & Topics but also including unconventional mechanisms such as Mongo DB Change Stream and Cosmos Change Feed. These benchmarks are to **measure the delay** introduced by just the underlying transport service (including any necessary libraries).

Although these types of services are typically regarded as asynchronous, there performance in terms of additional delay they introduce can have an impact on user experience for example in transferring data between two microservices where the receiving service will have stale data until it receives the update.

These tests are not intended to measure the throughput of the transport but the delay the transport introduces assuming the producer, the transport service, and the consumer are appropriately provisioned to handle the load.

# Setup & Running
1. Populate the values for connection strings, db names, etc. in the source code for the projects you want to run.
1. Start the relevent projects.
1. Stop both the producer and consumer at the same time and save the console output to a csv file. (Can do this with copy & paste or redirecting console output.)
1. Use the `ConsoleAppReadOutput` project to read the producer's output and the consumer/watcher output. This will print the stats of the delay to the console. (Don't forget to  update file names in source code for `ConsoleAppReadOutput`.)

# Notes
- Keep note of whether the service you are using has any form of partitioning the data to help scale out which may also have an impact (positive or negative) on latency/delay.
- Different service tiers and skus may have an impact on latency/delay even if the target capacity and/or throughput is the same. Test with the tier that is relevent to your scenario.
- Be cautious of service limits and machine capabilities. Saturating the service (esp. in lower tiers) can happen when the producer is in a tight loop doing no work. If the producer gets too far ahead of the consumer you will be artificially increasing your delay time which has nothing to do with service but is instead a factor of under provisioned capacity. This is especially true when using PAAS services which have documented limits at different SKU or scale out levels but may also happen due to network bandwidth, local machine limits, etc.
- Check your "queue" or "work backlog" throughout the test and after the test is complete. If it grows too big or the remaining work in queue is more than a few items at the time you stop the test, your producer may be getting too far ahead of your consumer and affecting your results.
- Consider slowing down the producer with artificial delays if it getting too far ahead.

## Azure Service Bus
Expect substantially different delays between the Basic & Standard Tiers in Service Bus and the Premium Tier.
