namespace CodingQuestions.Parallelism.ConcurrentCollections;

[Q(3_05_01, "ConcurrentDictionary: AddOrUpdate, GetOrAdd & the Lazy Trick", Medium,
"Count hits from many threads with a thread-safe dictionary. Show why a normal Dictionary breaks, and why a GetOrAdd factory may run more than once.")]
public static class ConcurrentDictionaryDemo
{
    public static void Run()
    {
        // A plain Dictionary is not thread-safe: concurrent writes can lose data, throw, or corrupt it.
        var plain = new Dictionary<int, int>();
        string plainResult;
        try
        {
            Parallel.For(0, 100_000, i => { plain[i % 100] = plain.GetValueOrDefault(i % 100) + 1; });
            plainResult = $"total {plain.Values.Sum():N0}";
        }
        catch (Exception e) { plainResult = $"threw {e.GetType().Name}"; }
        Console.WriteLine($"  plain Dictionary from many threads: {plainResult} (expected 100,000)");

        // AddOrUpdate is atomic per key: add 1 or apply the update function.
        var hits = new ConcurrentDictionary<int, int>();
        Parallel.For(0, 100_000, i => hits.AddOrUpdate(i % 100, 1, (_, old) => old + 1));
        Check("ConcurrentDictionary total", hits.Values.Sum(), 100_000);
        Check("Each of 100 keys", hits.Values.Distinct(), [1000]);

        // GetOrAdd(key, factory): the factory can run on several threads at once for the same key
        // (only one result is kept). Bad if the factory is expensive or has side effects.
        int factoryCalls = 0;
        var cache = new ConcurrentDictionary<string, string>();
        Parallel.For(0, 32, _ => cache.GetOrAdd("config", k => { Interlocked.Increment(ref factoryCalls); Thread.Sleep(20); return "loaded"; }));
        Console.WriteLine($"  GetOrAdd factory ran {factoryCalls} time(s) for one key");

        // Fix: store Lazy<T>. Many Lazy wrappers may be created, but only the stored one's Value ever runs.
        int lazyCalls = 0;
        var lazyCache = new ConcurrentDictionary<string, Lazy<string>>();
        Parallel.For(0, 32, i => _ = lazyCache.GetOrAdd("config", k => new Lazy<string>(() => { Interlocked.Increment(ref lazyCalls); Thread.Sleep(20); return "loaded"; })).Value);
        Check("Lazy<T> factory runs", lazyCalls, 1);

        Check("TryRemove", hits.TryRemove(5, out int removed) ? removed : -1, 1000);
    }
}
