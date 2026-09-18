namespace CSharpCodingQuestions.Questions.Parallelism.Synchronization;

[Question(Order = 7, Title = "Wait for N Workers (CountdownEvent)", Level = Easy, Problem = """
    Hand out 5 jobs to pool threads and continue only when **all 5** have reported that they're done.
    """)]
public static class WaitForWorkers
{
    [Approach(Name = "Count and Poll", Idea = """
        Each worker adds 1 to a shared counter when done, and the main thread checks the counter in a loop.
        It works, but the main thread keeps waking up just to check.
        """)]
    public static int WaitByPolling(int workerCount)
    {
        int done = 0;
        for (int i = 0; i < workerCount; i++)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(20);
                Interlocked.Increment(ref done);
            });
        }
        while (Volatile.Read(ref done) < workerCount)
        {
            Thread.Sleep(5);
        }
        return done;
    }

    [Approach(Name = "CountdownEvent", Idea = """
        A `CountdownEvent` starts at `N`. Each worker calls `Signal()` to count down by 1, and `Wait()` sleeps until it reaches 0.
        No polling and no wasted wake-ups.

        (With tasks instead of raw threads, `await Task.WhenAll(tasks)` does the same job.)
        """)]
    public static int WaitWithCountdown(int workerCount)
    {
        int done = 0;
        using var remaining = new CountdownEvent(workerCount);
        for (int i = 0; i < workerCount; i++)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(20);
                Interlocked.Increment(ref done);
                remaining.Signal();
            });
        }
        remaining.Wait();
        return done;
    }

    public static void Demo()
    {
        Print("Count and poll: workers finished", WaitByPolling(5), expected: 5);
        Print("CountdownEvent: workers finished", WaitWithCountdown(5), expected: 5);
    }
}
