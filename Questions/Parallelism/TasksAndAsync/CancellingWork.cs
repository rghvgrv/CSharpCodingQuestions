namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 6, Title = "Stop Work Early (CancellationToken)", Level = Medium, Problem = """
    A long calculation runs in the background. The user clicks "Stop" after 50 ms. Make the work stop quickly and cleanly.
    """)]
public static class CancellingWork
{
    [Approach(Name = "A Shared Stop Flag", Idea = """
        The worker checks a `volatile bool` in its loop, and the caller sets it to stop.
        It works for one loop, but every method you call needs access to that flag, and `Task.Delay`, `HttpClient` and
        other .NET methods have no idea your flag exists, so they can't stop early.
        """)]
    public class StopFlag
    {
        private volatile bool stopRequested;

        public void Stop() => stopRequested = true;

        public long CountUntilStopped()
        {
            long count = 0;
            while (!stopRequested)
            {
                count++;
            }
            return count;
        }
    }

    [Approach(Name = "CancellationToken", Idea = """
        The standard .NET way: a `CancellationTokenSource` makes a **token** that you pass into your methods.

        - The caller calls `source.Cancel()`, or `CancelAfter(time)` to cancel after a timeout.
        - The worker calls `token.ThrowIfCancellationRequested()` now and then, which throws `OperationCanceledException`.
        - Built-in async methods (`Task.Delay`, `HttpClient`, database calls…) accept the same token and stop too.

        Cancellation is **cooperative**: the worker decides where it's safe to stop.
        """)]
    public static long CountUntilCancelled(CancellationToken token)
    {
        long count = 0;
        while (true)
        {
            count++;
            if (count % 1000 == 0)
            {
                token.ThrowIfCancellationRequested();
            }
        }
    }

    public static void Demo()
    {
        var flag = new StopFlag();
        Task<long> flagWork = Task.Run(flag.CountUntilStopped);
        Thread.Sleep(50);
        flag.Stop();
        Print("Stop flag: work stopped", flagWork.Wait(1000), expected: true);

        using var source = new CancellationTokenSource();
        source.CancelAfter(50);
        var clock = Stopwatch.StartNew();
        try
        {
            CountUntilCancelled(source.Token);
        }
        catch (OperationCanceledException)
        {
            Print("CancellationToken: work stopped", $"after about {clock.ElapsedMilliseconds} ms");
        }

        using var another = new CancellationTokenSource();
        Task delay = Task.Delay(10_000, another.Token);
        another.Cancel();
        try
        {
            delay.Wait();
        }
        catch (AggregateException)
        {
        }
        Print("Task.Delay(10 s) with a cancelled token", delay.Status.ToString(), expected: "Canceled");
    }
}
