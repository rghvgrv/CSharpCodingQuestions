namespace CodingQuestions.Parallelism.ThreadBasics;

[Q(3_01_01, "Creating Threads: Start, Join, Background", Easy,
"Start three threads that finish in different orders, wait for all of them with Join, and return a result computed on another thread.")]
public static class CreatingThreads
{
    // A Thread is an OS thread: its own stack (~1 MB) and scheduling. Start() begins it, Join() waits for it to end.
    // Background threads don't keep the process alive; foreground threads (the default) do.
    public static void Run()
    {
        Console.WriteLine($"Caller runs on thread {Environment.CurrentManagedThreadId}");

        var threads = new List<Thread>();
        for (int i = 1; i <= 3; i++)
        {
            int n = i; // copy: a `for` variable captured by a lambda is shared by all iterations
            var t = new Thread(() =>
            {
                Thread.Sleep(40 * (4 - n)); // worker 3 finishes first
                Console.WriteLine($"  worker {n} finished on thread {Environment.CurrentManagedThreadId} ({Thread.CurrentThread.Name})");
            })
            { Name = $"Worker-{n}", IsBackground = true };
            threads.Add(t);
            t.Start();
        }
        threads.ForEach(t => t.Join());
        Console.WriteLine("All workers joined");

        // Getting a result back: write to a captured variable, then Join before reading it.
        long result = 0;
        var calc = new Thread(() => result = Enumerable.Range(1, 1000).Sum());
        calc.Start();
        calc.Join();
        Check("Sum 1..1000 computed on another thread", result, 500500L);
        Check("ThreadState after Join", calc.ThreadState.ToString(), "Stopped");
    }
}
