namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_05, "Continuations: ContinueWith vs await", Medium,
"Chain three steps (download → parse → save), first with ContinueWith, then with await. Handle a failing step both ways.")]
public static class TaskContinuations
{
    // ContinueWith was the pre-async way to say "then do this". await compiles to continuations too,
    // but it reads like normal code and unwraps exceptions. Prefer await; know ContinueWith for older code.
    public static void Run()
    {
        var log = new ConcurrentQueue<string>();

        Task<int> chain = Task.Run(() => { log.Enqueue("download"); return "42"; })
            .ContinueWith(t => { log.Enqueue("parse"); return int.Parse(t.Result); })
            .ContinueWith(t => { log.Enqueue("save"); return t.Result * 2; });
        Check("ContinueWith chain result", chain.Result, 84);
        Check("Order", log.ToArray(), ["download", "parse", "save"]);

        // Run different continuations depending on the outcome.
        var failing = Task.Run(() => int.Parse("not a number"));
        var onError = failing.ContinueWith(t => $"handled: {t.Exception!.InnerException!.GetType().Name}", TaskContinuationOptions.OnlyOnFaulted);
        var onSuccess = failing.ContinueWith(_ => "success", TaskContinuationOptions.OnlyOnRanToCompletion);
        Check("OnlyOnFaulted continuation", onError.Result, "handled: FormatException");
        Task.WhenAny(onSuccess).Wait(); // WhenAny never throws, unlike Wait() on a canceled task
        Check("OnlyOnRanToCompletion was skipped", onSuccess.Status.ToString(), "Canceled");

        // The same with await: plain try/catch.
        Check("await version", WithAwait("42").GetAwaiter().GetResult(), "84");
        Check("await version with bad input", WithAwait("x").GetAwaiter().GetResult(), "handled: FormatException");
    }

    static async Task<string> WithAwait(string input)
    {
        try
        {
            string raw = await Task.Run(() => input);
            int parsed = await Task.Run(() => int.Parse(raw));
            return (parsed * 2).ToString();
        }
        catch (FormatException e) { return $"handled: {e.GetType().Name}"; }
    }
}
