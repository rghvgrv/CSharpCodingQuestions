namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_01, "Task Basics: Task.Run, Task<T>, Wait, Status", Easy,
"Start work with Task.Run, get a result from Task<T>, wait for several tasks, and look at task status. Explain Task vs Thread.")]
public static class TaskBasics
{
    // Task = a promise of a future result. It usually runs on a pool thread (unlike Thread, which is always a new OS thread).
    // Tasks compose: results, exceptions, cancellation, continuations, await.
    public static void Run()
    {
        Task<long> sumTask = Task.Run(() =>
        {
            Console.WriteLine($"  computing on pool thread {Environment.CurrentManagedThreadId}");
            return Enumerable.Range(1, 1_000_000).Sum(x => (long)x);
        });
        Check(".Result (blocks until done)", sumTask.Result, 500_000_500_000L);
        Check("Status", sumTask.Status.ToString(), "RanToCompletion");

        var tasks = Enumerable.Range(1, 4).Select(n => Task.Run(() => { Thread.Sleep(20 * n); return n * n; })).ToArray();
        Task.WaitAll(tasks);
        Check("WaitAll → results", tasks.Select(t => t.Result), [1, 4, 9, 16]);

        int firstDone = Task.WaitAny(tasks);
        Console.WriteLine($"  WaitAny returned index {firstDone} (all were already finished)");

        // Long blocking work: hint the scheduler to use a dedicated thread instead of tying up a pool thread.
        var longRunning = Task.Factory.StartNew(() => Thread.CurrentThread.IsThreadPoolThread, TaskCreationOptions.LongRunning);
        Check("LongRunning uses a pool thread?", longRunning.Result, false);

        // An already-finished task, no thread at all (useful for caches and fast paths)
        Task<string> cached = Task.FromResult("from cache");
        Check("Task.FromResult is complete immediately", cached.IsCompleted, true);
    }
}
