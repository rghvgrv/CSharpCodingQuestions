namespace CSharpCodingQuestions.Questions.Parallelism.Synchronization;

[Question(Order = 4, Title = "Limit How Many Run at Once (SemaphoreSlim)", Level = Medium, Problem = """
    You must call a slow web service 10 times, but it allows at most **3 calls at the same time**. Run the calls as fast as allowed.
    """)]
public static class LimitConcurrency
{
    [Approach(Name = "Start Everything (No Limit)", Idea = """
        Start all 10 calls at once with `Task.WhenAll`. Fastest, but it breaks the service's rule: 10 calls run together.
        """)]
    public static async Task<int> CallWithoutLimit(int calls)
    {
        var tracker = new ConcurrencyTracker();
        await Task.WhenAll(Enumerable.Range(0, calls).Select(_ => tracker.SimulatedCallAsync()));
        return tracker.MaxAtOnce;
    }

    [Approach(Name = "SemaphoreSlim(3)", Idea = """
        A semaphore holds a number of **permits**, here 3. Each call waits for a permit (`WaitAsync`), runs, then gives it back (`Release`).
        A 4th call waits until one of the first 3 finishes.

        `WaitAsync` waits **without blocking a thread**, and `finally` makes sure the permit is returned even if the call fails.
        `new SemaphoreSlim(1)` is also the standard way to "lock" code that contains `await`, since a normal `lock` can't.
        """)]
    public static async Task<int> CallWithSemaphore(int calls)
    {
        var tracker = new ConcurrencyTracker();
        using var permits = new SemaphoreSlim(3);

        async Task LimitedCall()
        {
            await permits.WaitAsync();
            try
            {
                await tracker.SimulatedCallAsync();
            }
            finally
            {
                permits.Release();
            }
        }

        await Task.WhenAll(Enumerable.Range(0, calls).Select(_ => LimitedCall()));
        return tracker.MaxAtOnce;
    }

    // Pretends to be a web call (100 ms) and records how many calls were running at the same moment.
    public class ConcurrencyTracker
    {
        private readonly object gate = new object();
        private int running;
        private int maxAtOnce;

        public int MaxAtOnce => maxAtOnce;

        public async Task SimulatedCallAsync()
        {
            int now = Interlocked.Increment(ref running);
            lock (gate)
            {
                maxAtOnce = Math.Max(maxAtOnce, now);
            }
            await Task.Delay(100);
            Interlocked.Decrement(ref running);
        }
    }

    public static void Demo()
    {
        var clock = Stopwatch.StartNew();
        int unlimited = CallWithoutLimit(10).Result;
        Print("No limit: most calls running at once", $"{unlimited} (took {clock.ElapsedMilliseconds} ms)");

        clock.Restart();
        int limited = CallWithSemaphore(10).Result;
        long limitedMs = clock.ElapsedMilliseconds;
        Print("SemaphoreSlim(3): never more than 3 at once", limited <= 3, expected: true);
        Console.WriteLine($"  took {limitedMs} ms (10 calls in groups of 3 ≈ 4 × 100 ms)");
    }
}
