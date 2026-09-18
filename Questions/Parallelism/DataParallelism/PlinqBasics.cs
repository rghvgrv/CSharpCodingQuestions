namespace CodingQuestions.Parallelism.DataParallelism;

[Q(3_04_03, "PLINQ: Parallel LINQ", Easy,
"Turn a LINQ query parallel with AsParallel(). Keep the input order with AsOrdered(), limit threads with WithDegreeOfParallelism, and process results with ForAll.")]
public static class PlinqBasics
{
    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int d = 2; d * d <= n; d++) if (n % d == 0) return false;
        return true;
    }

    public static void Run()
    {
        var numbers = Enumerable.Range(1, 200_000);

        // Same query, one call added. Results come back in whatever order threads finish.
        int count = numbers.AsParallel().Count(IsPrime);
        Check("Primes ≤ 200,000", count, numbers.Count(IsPrime));

        // Unordered: fast, but order is not guaranteed.
        var unordered = numbers.AsParallel().Where(IsPrime).Take(10).ToList();
        Console.WriteLine($"  unordered Take(10): {Fmt(unordered)}");

        // AsOrdered: results keep the source order (costs some buffering).
        var ordered = numbers.AsParallel().AsOrdered().Where(IsPrime).Take(10).ToList();
        Check("AsOrdered Take(10)", ordered, [2, 3, 5, 7, 11, 13, 17, 19, 23, 29]);

        // Limit the number of threads; see which threads ran the query.
        var threads = new ConcurrentDictionary<int, byte>();
        numbers.AsParallel().WithDegreeOfParallelism(2).ForAll(_ => threads.TryAdd(Environment.CurrentManagedThreadId, 0));
        Check("WithDegreeOfParallelism(2) → threads used ≤ 2", threads.Count <= 2, true);

        // PLINQ is for CPU-bound work on in-memory data. For tiny work per item, it can be SLOWER (overhead).
        var clock = Stopwatch.StartNew();
        _ = numbers.Select(x => (long)x + 1).Sum();
        double seqMs = clock.Elapsed.TotalMilliseconds;
        clock.Restart();
        _ = numbers.AsParallel().Select(x => (long)x + 1).Sum();
        Console.WriteLine($"  trivial work x+1: sequential {seqMs:0.00} ms vs parallel {clock.Elapsed.TotalMilliseconds:0.00} ms (overhead can win)");
    }
}
