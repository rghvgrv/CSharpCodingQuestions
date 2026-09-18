namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_07, "Exceptions in Tasks: AggregateException vs await", Medium,
"Show how exceptions surface from .Wait() / .Result (AggregateException), from await (the original exception), and how to see ALL failures from Task.WhenAll.")]
public static class TaskExceptions
{
    static async Task Fail(string message, int delayMs)
    {
        await Task.Delay(delayMs);
        throw new InvalidOperationException(message);
    }

    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        // .Wait() / .Result wrap the error in an AggregateException.
        try { Fail("boom", 10).Wait(); }
        catch (AggregateException ae) { Check(".Wait() throws", $"{ae.GetType().Name} → {ae.InnerException!.Message}", "AggregateException → boom"); }

        // await unwraps it: you catch the original exception type.
        try { await Fail("boom", 10); }
        catch (InvalidOperationException e) { Check("await throws", e.Message, "boom"); }

        // WhenAll with several failures: await gives only the FIRST; the task holds all of them.
        var all = Task.WhenAll(Fail("a", 30), Fail("b", 10), Fail("c", 20));
        try { await all; }
        catch (Exception first) { Console.WriteLine($"  await caught only: {first.Message}"); }
        Check("all.Exception.InnerExceptions", all.Exception!.InnerExceptions.Select(e => e.Message).Order().ToArray(), ["a", "b", "c"]);

        // An unobserved failed task doesn't crash the process in modern .NET, but the error is silently lost.
        // Always await (or observe) your tasks. "Fire and forget" needs its own try/catch.
        _ = Task.Run(async () =>
        {
            try { await Fail("background", 1); }
            catch (Exception e) { Console.WriteLine($"  fire-and-forget logged: {e.Message}"); }
        });
        await Task.Delay(50);
    }
}
