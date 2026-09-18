namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_06, "Cancellation with CancellationToken", Medium,
"Cancel a long CPU loop and an async wait cooperatively, cancel automatically after a timeout, and link two tokens so either one cancels the work.")]
public static class Cancellation
{
    // Cancellation is cooperative: the caller requests it (cts.Cancel), the worker checks the token and stops.
    static long CountPrimes(int limit, CancellationToken ct)
    {
        long count = 0;
        for (int n = 2; n < limit; n++)
        {
            if ((n & 0x3FF) == 0) ct.ThrowIfCancellationRequested(); // check every 1024 iterations: cheap enough
            bool prime = true;
            for (int d = 2; (long)d * d <= n; d++) if (n % d == 0) { prime = false; break; }
            if (prime) count++;
        }
        return count;
    }

    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        // 1) Cancel after a timeout.
        using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50)))
        {
            var clock = Stopwatch.StartNew();
            try
            {
                await Task.Run(() => CountPrimes(int.MaxValue, cts.Token), cts.Token);
                Check("CPU loop", "finished", "canceled");
            }
            catch (OperationCanceledException) { Check($"CPU loop canceled after ~{clock.ElapsedMilliseconds} ms", "canceled", "canceled"); }
        }

        // 2) Async APIs take a token too.
        using (var cts = new CancellationTokenSource())
        {
            var wait = Task.Delay(5000, cts.Token);
            cts.Cancel();
            try { await wait; }
            catch (TaskCanceledException) { }
            Check("Task.Delay status after Cancel()", wait.Status.ToString(), "Canceled");
        }

        // 3) Linked tokens: cancel if EITHER the user clicks stop OR the timeout hits.
        using var userStop = new CancellationTokenSource();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(userStop.Token, timeout.Token);
        userStop.Cancel();
        Check("Linked token canceled by the user source", linked.IsCancellationRequested, true);

        // 4) Register a callback that runs on cancel (e.g. abort a socket).
        using var cb = new CancellationTokenSource();
        string? note = null;
        cb.Token.Register(() => note = "cleanup ran");
        cb.Cancel();
        Check("Register callback", note, "cleanup ran");
    }
}
