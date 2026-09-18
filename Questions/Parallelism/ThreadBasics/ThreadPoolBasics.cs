namespace CSharpCodingQuestions.Questions.Parallelism.ThreadBasics;

[Question(Order = 2, Title = "The Thread Pool", Level = Easy, Problem = """
    Run 500 tiny jobs at the same time. Creating a brand-new thread for each one is expensive. Is there a better way?
    """)]
public static class ThreadPoolBasics
{
    [Approach(Name = "A New Thread for Every Job", Idea = """
        Each `new Thread` asks the operating system for a real thread with about 1 MB of memory. For tiny jobs,
        creating and destroying the threads costs far more than the work itself.
        """)]
    public static int RunWithNewThreads(int jobCount)
    {
        int finished = 0;
        var threads = new List<Thread>();
        for (int i = 0; i < jobCount; i++)
        {
            var thread = new Thread(() => Interlocked.Increment(ref finished));
            threads.Add(thread);
            thread.Start();
        }
        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        return finished;
    }

    [Approach(Name = "Thread Pool With Task.Run", Idea = """
        .NET keeps a **pool** of threads that stay alive and pick up jobs from a queue.
        `Task.Run` puts a job in that queue, and `Task.WaitAll` waits for all of them.
        A handful of reused threads run all 500 jobs. `Parallel.For`, PLINQ and `async` code all use this pool.
        """)]
    public static int RunOnThreadPool(int jobCount)
    {
        int finished = 0;
        var tasks = new Task[jobCount];
        for (int i = 0; i < jobCount; i++)
        {
            tasks[i] = Task.Run(() => Interlocked.Increment(ref finished));
        }
        Task.WaitAll(tasks);
        return finished;
    }

    public static void Demo()
    {
        var clock = Stopwatch.StartNew();
        int withThreads = RunWithNewThreads(500);
        long threadsMs = clock.ElapsedMilliseconds;

        clock.Restart();
        int withPool = RunOnThreadPool(500);
        long poolMs = clock.ElapsedMilliseconds;

        Print("New thread per job", $"{withThreads} jobs done in {threadsMs} ms");
        Print("Thread pool", $"{withPool} jobs done in {poolMs} ms");
        Print("All jobs finished both ways", withThreads == 500 && withPool == 500, expected: true);
        Print("Task.Run runs on a pool thread", Task.Run(() => Thread.CurrentThread.IsThreadPoolThread).Result, expected: true);
    }
}
