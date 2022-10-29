int runid = 11;

var insertLines = File.ReadAllLines(@$"C:\temp\insert{runid}.csv").Skip(2).ToArray();
var watchLinesAll = File.ReadAllLines(@$"C:\temp\watch{runid}.csv");
var watchLines = watchLinesAll.Skip(2).ToArray();

// parse from CSV output of console
var inserts = from line in insertLines
              let cols = line.Split(',')
              select new
              {
                  time = DateTime.Parse(cols[0]),
                  operation = cols[1],
                  id = cols[2]
              };

// parse from CSV output of console
var watches = from line in watchLines
              let cols = line.Split(',')
              select new
              {
                  time = DateTime.Parse(cols[0]),
                  operation = cols[1],
                  id = cols[2]
              };

// join watches & inserts to find delta of time
var deltas = from ins in inserts
        join wat in watches on ins.id equals wat.id
        select new
        {
            insert = ins,
            watch = wat,
            delta = wat.time - ins.time
        };

foreach (var item in deltas)
{
    Console.WriteLine(new { item.delta, item.watch.id });
}

Console.WriteLine();
Console.WriteLine();
Console.WriteLine("--- STATS ---");
Console.WriteLine($"Title: {watchLinesAll[1]}");
Console.WriteLine($"Start Time: {inserts.Min(m => m.time)}");
Console.WriteLine($"End Time: {inserts.Max(m => m.time)}");
Console.WriteLine($"Total Time: {inserts.Max(m => m.time) - inserts.Min(m => m.time)}");
Console.WriteLine($"Total Count: inserts = {inserts.Count()}, watches = {watches.Count()}");
Console.WriteLine($"Avg Delta: {deltas.Where(a => a.delta.TotalMilliseconds > -1).Average(a => a.delta.TotalMilliseconds)} (excludes negatives)");
Console.WriteLine($"Max Delta: {deltas.Max(a => a.delta.TotalMilliseconds)}");
Console.WriteLine($"Min Delta: {deltas.Min(a => a.delta.TotalMilliseconds)}");
Console.WriteLine($"Duplicate IDs: {inserts.GroupBy(x => x.id).Where(x => x.Count() > 1).Count()}");
Console.WriteLine($"Total Negative: {deltas.Count(i => i.delta.TotalMilliseconds < 0)}");
