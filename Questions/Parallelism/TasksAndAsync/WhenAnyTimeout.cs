namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_04, "Task.WhenAny: First Response Wins & Timeouts", Medium,
"(1) Query three mirrors and use whichever answers first. (2) Put a timeout on an async call, with WhenAny and with WaitAsync.")]
public static class WhenAnyTimeout
{
    static async Task<string> Mirror(string name, int delayMs, CancellationToken ct)
    {
        await Task.Delay(delayMs, ct);
        return name;
    }

    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        // Hedged request: start all, take the first, cancel the rest.
        using var cts = new CancellationTokenSource();
        Task<string>[] mirrors = [Mirror("eu", 300, cts.Token), Mirror("us", 80, cts.Token), Mirror("asia", 200, cts.Token)];
        Task<string> winner = await Task.WhenAny(mirrors);
        cts.Cancel();
        Check("Fastest mirror", await winner, "us");

        // Timeout with WhenAny: race the work against a delay.
        Task<string> slow = Mirror("slow", 1000, CancellationToken.None);
        bool timedOut = await Task.WhenAny(slow, Task.Delay(100)) != slow;
        Check("WhenAny(work, Delay(100)) timed out", timedOut, true);

        // Timeout with WaitAsync (.NET 6+): throws TimeoutException.
        try
        {
            await Mirror("slow", 1000, CancellationToken.None).WaitAsync(TimeSpan.FromMilliseconds(100));
            Check("WaitAsync timeout", "no exception", "TimeoutException");
        }
        catch (TimeoutException) { Check("WaitAsync(100 ms) threw", "TimeoutException", "TimeoutException"); }

        Check("Fast call inside the timeout", await Mirror("fast", 20, CancellationToken.None).WaitAsync(TimeSpan.FromSeconds(1)), "fast");
    }
}
