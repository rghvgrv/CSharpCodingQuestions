namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_02, "async / await Explained", Easy,
"Write an async method that awaits simulated I/O. Show that no thread is blocked while waiting, and that the code after await may continue on a different thread.")]
public static class AsyncAwaitBasics
{
    // await on an incomplete task: the method returns to its caller, and the thread goes back to the pool.
    // When the I/O finishes, the REST of the method is scheduled as a continuation (often on another thread).
    // async is for I/O-bound waiting; for CPU-bound work use Task.Run / Parallel.
    static async Task<string> FetchUserAsync(int id)
    {
        Console.WriteLine($"  FetchUser({id}) starts on thread {Environment.CurrentManagedThreadId}");
        await Task.Delay(100); // stands in for an HTTP call or DB query
        Console.WriteLine($"  FetchUser({id}) resumes on thread {Environment.CurrentManagedThreadId}");
        return $"user-{id}";
    }

    // Run() must be synchronous here, so it blocks once at the top.
    // In a real app, stay async all the way up instead: never call .Result on async code.
    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        Check("await FetchUserAsync(7)", await FetchUserAsync(7), "user-7");

        // Start first, await later: both "requests" are in flight together.
        var clock = Stopwatch.StartNew();
        Task<string> a = FetchUserAsync(1), b = FetchUserAsync(2);
        Check("Two concurrent awaits", $"{await a}, {await b}", "user-1, user-2");
        Console.WriteLine($"  both finished in {clock.ElapsedMilliseconds} ms (not 200 ms)");

        // Proof that waiting doesn't hold threads: 1,000 concurrent 100 ms waits finish in ~100 ms.
        clock.Restart();
        await Task.WhenAll(Enumerable.Range(0, 1000).Select(_ => Task.Delay(100)));
        Console.WriteLine($"  1,000 concurrent awaits took {clock.ElapsedMilliseconds} ms using no blocked threads");
        Check("1,000 awaits finish in well under 1 s", clock.ElapsedMilliseconds < 1000, true);
    }
}
