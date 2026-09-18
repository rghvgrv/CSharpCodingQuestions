namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_09, "Parallel.ForEachAsync: Throttled Async Work", Medium,
"Process 20 URLs with async I/O (each 50 ms), with at most 4 in flight at once. Compare with doing them one by one.")]
public static class ParallelForEachAsync
{
    // .NET 6+: Parallel.ForEachAsync = Parallel.ForEach for async bodies, with a built-in concurrency limit.
    static async Task<int> FetchLength(string url, CancellationToken ct)
    {
        await Task.Delay(50, ct);
        return url.Length;
    }

    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        var urls = Enumerable.Range(1, 20).Select(i => $"https://example.com/page/{i}").ToList();

        var clock = Stopwatch.StartNew();
        int sequentialTotal = 0;
        foreach (var url in urls) sequentialTotal += await FetchLength(url, default);
        long seqMs = clock.ElapsedMilliseconds;

        clock.Restart();
        int total = 0, inFlight = 0, maxInFlight = 0;
        var gate = new Lock();
        var options = new ParallelOptions { MaxDegreeOfParallelism = 4 };
        await Parallel.ForEachAsync(urls, options, async (url, ct) =>
        {
            int now = Interlocked.Increment(ref inFlight);
            lock (gate) maxInFlight = Math.Max(maxInFlight, now);
            Interlocked.Add(ref total, await FetchLength(url, ct));
            Interlocked.Decrement(ref inFlight);
        });
        long parMs = clock.ElapsedMilliseconds;

        Console.WriteLine($"One by one: {seqMs} ms   ForEachAsync(max 4): {parMs} ms (≈ 20 / 4 × 50 ms)");
        Check("Same total", total, sequentialTotal);
        Check("Never more than 4 in flight", maxInFlight <= 4, true);
    }
}
