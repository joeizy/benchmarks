# benchmarks
Benchmark code &amp; tips for various scenarios. The purpose of these benchmarks is to measure **how they will behave in average conditions with average code** so that the average person can use the results and/or run the code in their environment. The purpose _is not_ to measure the maximum capability of a target service or framework.  

Unless otherwise noted as a Tip or Best Practice, these examples are typically not highly optimized benchmark code and may even be slightly less that production quality code. The average development shop or enterprise will not have the expertise or time that a Facebook, Microsoft, Google, or Amazon has to highly tune their code.

# Reliable Transports
Benchmarks for services which reliably transfer data from a producer to a consumer such as Queus & Topics but also including unconventional options such as Mongo DB Change Stream and Azure Cosmos DB Change Feed.
