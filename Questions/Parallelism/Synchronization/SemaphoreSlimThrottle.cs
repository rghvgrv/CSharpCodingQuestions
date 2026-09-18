namespace CodingQuestions.Parallelism.Synchronization;

[Q(3_02_04, "SemaphoreSlim: Limit Concurrency (and an Async Lock)", Medium,
"Start 10 async downloads but let at most 3 run at the same time. Then use SemaphoreSlim(1, 1) as a lock that works with await.")]
public static class SemaphoreSlimThrottle
{
    // A semaphore holds N permits. WaitAsync takes one (or waits), Release gives it back.
    // SemaphoreSlim supports await, unlike `lock`, which can't contain an await.
    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        using var throttle = new SemaphoreSlim(3);
        int running = 0, maxRunning = 0;
        var clock = Stopwatch.StartNew();

        async Task<int> Download(int id)
        {
            await throttle.WaitAsync();
            try
            {
                int now = Interlocked.Increment(ref running);
                InterlockedMax(ref maxRunning, now);
                Console.WriteLine($"  [{clock.ElapsedMilliseconds,4} ms] start #{id} (running: {now})");
                await Task.Delay(100); // pretend I/O
                Interlocked.Decrement(ref running);
                return id * 10;
            }
            finally { throttle.Release(); }
        }

        var results = await Task.WhenAll(Enumerable.Range(1, 10).Select(Download));
        Console.WriteLine($"Took ~{clock.ElapsedMilliseconds} ms (10 jobs × 100 ms / 3 at a time ≈ 400 ms)");
        Check("Max running at once ≤ 3", maxRunning <= 3, true);
        Check("All results", results, [10, 20, 30, 40, 50, 60, 70, 80, 90, 100]);

        // Async mutex: protects a critical section that contains an await.
        using var mutex = new SemaphoreSlim(1, 1);
        int counter = 0;
        await Task.WhenAll(Enumerable.Range(0, 20).Select(async _ =>
        {
            await mutex.WaitAsync();
            try { int c = counter; await Task.Yield(); counter = c + 1; } // read-await-write, safe under the mutex
            finally { mutex.Release(); }
        }));
        Check("Async-locked read-await-write ×20", counter, 20);
    }

    static void InterlockedMax(ref int target, int value)
    {
        for (int cur = target; value > cur; cur = target)
            if (Interlocked.CompareExchange(ref target, value, cur) == cur) return;
    }
}
