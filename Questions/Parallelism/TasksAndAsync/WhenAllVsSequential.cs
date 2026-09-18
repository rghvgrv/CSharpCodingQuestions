namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_03, "Task.WhenAll: Sequential vs Concurrent Calls", Easy,
"Fetch 5 prices that each take 150 ms. Compare awaiting them one by one with starting them all and awaiting Task.WhenAll.")]
public static class WhenAllVsSequential
{
    static async Task<decimal> GetPriceAsync(string item)
    {
        await Task.Delay(150); // simulated network call
        return item.Length * 1.5m;
    }

    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        string[] items = ["apple", "banana", "cherry", "date", "elderberry"];

        var clock = Stopwatch.StartNew();
        var sequential = new List<decimal>();
        foreach (var item in items) sequential.Add(await GetPriceAsync(item)); // waits for each before starting the next
        long seqMs = clock.ElapsedMilliseconds;

        clock.Restart();
        decimal[] concurrent = await Task.WhenAll(items.Select(GetPriceAsync)); // all start now, results keep input order
        long allMs = clock.ElapsedMilliseconds;

        Console.WriteLine($"Sequential: {seqMs} ms   WhenAll: {allMs} ms");
        Check("Same results", concurrent, sequential.ToArray());
        Check("WhenAll is faster", allMs < seqMs, true);
    }
}
