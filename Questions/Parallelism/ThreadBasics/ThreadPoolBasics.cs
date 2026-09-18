namespace CodingQuestions.Parallelism.ThreadBasics;

[Q(3_01_02, "The Thread Pool", Easy,
"Queue 100 small work items to the .NET thread pool and show that a handful of reused threads runs them all. Compare with Task.Run.")]
public static class ThreadPoolBasics
{
    // Creating a thread per job is expensive. The pool keeps a few threads and feeds them work items.
    // Task.Run, Parallel.For, PLINQ and async continuations all run on the pool.
    public static void Run()
    {
        ThreadPool.GetMinThreads(out int minWorkers, out _);
        Console.WriteLine($"Pool min worker threads: {minWorkers} (usually = CPU cores: {Environment.ProcessorCount})");

        var perThread = new ConcurrentDictionary<int, int>();
        using var done = new CountdownEvent(100); // lets us wait until 100 signals arrive (see Synchronization)
        for (int i = 0; i < 100; i++)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                perThread.AddOrUpdate(Environment.CurrentManagedThreadId, 1, (_, c) => c + 1);
                Thread.SpinWait(20_000); // a little CPU work
                done.Signal();
            });
        }
        done.Wait();
        Console.WriteLine($"100 work items ran on {perThread.Count} distinct pool threads:");
        foreach (var (id, count) in perThread.OrderBy(kv => kv.Key)) Console.WriteLine($"  thread {id,3}: {count} items");
        Check("Total items run", perThread.Values.Sum(), 100);

        Check("Task.Run runs on a pool thread", Task.Run(() => Thread.CurrentThread.IsThreadPoolThread).Result, true);
        Check("new Thread is NOT a pool thread", RunOnNewThread(() => Thread.CurrentThread.IsThreadPoolThread), false);
    }

    static bool RunOnNewThread(Func<bool> f)
    {
        bool r = false;
        var t = new Thread(() => r = f());
        t.Start(); t.Join();
        return r;
    }
}
