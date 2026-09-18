namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 4, Title = "Give Up After a Time Limit", Level = Medium, Problem = """
    A call to a slow service sometimes takes a whole second. Wait at most 100 ms for it, then move on.
    """)]
public static class Timeouts
{
    private static async Task<string> SlowServiceAsync(int delayMs)
    {
        await Task.Delay(delayMs);
        return "data";
    }

    [Approach(Name = "Just await It", Idea = """
        A plain `await` waits as long as the call takes. If the service hangs, your program hangs with it.
        """)]
    public static async Task<string> CallWithoutLimit(int delayMs)
    {
        return await SlowServiceAsync(delayMs);
    }

    [Approach(Name = "Race Against Task.Delay", Idea = """
        Start the call and a timer (`Task.Delay(limit)`). `Task.WhenAny` finishes as soon as **either** one finishes.
        If the timer won, it's a timeout. This pattern also works for "use whichever server answers first".
        """)]
    public static async Task<string> CallWithWhenAny(int delayMs, int limitMs)
    {
        Task<string> call = SlowServiceAsync(delayMs);
        Task winner = await Task.WhenAny(call, Task.Delay(limitMs));
        return winner == call ? await call : "timed out";
    }

    [Approach(Name = "WaitAsync", Idea = """
        Since .NET 6, `task.WaitAsync(timeout)` does the race for you and throws a `TimeoutException` if time runs out.
        Shortest and clearest. (It stops **waiting**; to also stop the work itself, pass a `CancellationToken`, covered in a later question.)
        """)]
    public static async Task<string> CallWithWaitAsync(int delayMs, int limitMs)
    {
        try
        {
            return await SlowServiceAsync(delayMs).WaitAsync(TimeSpan.FromMilliseconds(limitMs));
        }
        catch (TimeoutException)
        {
            return "timed out";
        }
    }

    public static void Demo()
    {
        var clock = Stopwatch.StartNew();
        string noLimit = CallWithoutLimit(1000).Result;
        Print("No limit", $"\"{noLimit}\" after {clock.ElapsedMilliseconds} ms");

        clock.Restart();
        Print("WhenAny with a 100 ms limit", CallWithWhenAny(1000, 100).Result, expected: "timed out");
        Print("WaitAsync with a 100 ms limit", CallWithWaitAsync(1000, 100).Result, expected: "timed out");
        Console.WriteLine($"  both gave up after about {clock.ElapsedMilliseconds / 2} ms each");
        Print("WaitAsync when the call is fast (20 ms)", CallWithWaitAsync(20, 100).Result, expected: "data");
    }
}
