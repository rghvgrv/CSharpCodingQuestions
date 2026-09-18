namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 2, Title = "Waiting Without Blocking (async / await)", Level = Easy, Problem = """
    A server handles 40 requests at once. Each request waits 200 ms for a database. How do you wait without wasting threads?
    """)]
public static class AsyncAwaitBasics
{
    [Approach(Name = "Block a Thread While Waiting", Idea = """
        `Thread.Sleep` (like any blocking call) keeps a thread **busy doing nothing** while it waits.
        40 waiting requests need 40 threads. The thread pool starts only a few threads at first and adds more slowly,
        so requests queue up and the whole batch takes much longer than 200 ms.
        """)]
    public static long HandleBlocking(int requests)
    {
        var clock = Stopwatch.StartNew();
        var tasks = new Task[requests];
        for (int i = 0; i < requests; i++)
        {
            tasks[i] = Task.Run(() => Thread.Sleep(200));   // simulated database call that blocks
        }
        Task.WaitAll(tasks);
        return clock.ElapsedMilliseconds;
    }

    [Approach(Name = "await Task.Delay", Idea = """
        `await` on something that isn't finished yet **gives the thread back** and remembers where the method stopped.
        When the wait is over, the rest of the method continues, often on a different thread.
        No thread is held while waiting, so 40 (or 40,000) requests can wait at the same time.

        Use `async/await` for **waiting** (network, disk, database). For heavy **calculation**, use `Task.Run` or `Parallel`.
        """)]
    public static async Task<long> HandleAsync(int requests)
    {
        var clock = Stopwatch.StartNew();
        var tasks = new Task[requests];
        for (int i = 0; i < requests; i++)
        {
            tasks[i] = HandleOneRequestAsync();
        }
        await Task.WhenAll(tasks);
        return clock.ElapsedMilliseconds;
    }

    private static async Task HandleOneRequestAsync()
    {
        await Task.Delay(200);   // simulated database call that doesn't block
    }

    public static void Demo()
    {
        long blockingMs = HandleBlocking(40);
        long asyncMs = HandleAsync(40).Result;
        Print("Blocking: 40 requests of 200 ms", $"{blockingMs} ms");
        Print("async/await: 40 requests of 200 ms", $"{asyncMs} ms");
        Print("async finished in well under a second", asyncMs < 1000, expected: true);
    }
}
