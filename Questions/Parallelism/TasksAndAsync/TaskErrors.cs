namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 7, Title = "Handling Errors in Tasks", Level = Medium, Problem = """
    Three background jobs run together and **two of them fail**. Catch the errors and find out everything that went wrong.
    """)]
public static class TaskErrors
{
    private static async Task FailAsync(string message, int delayMs)
    {
        await Task.Delay(delayMs);
        throw new InvalidOperationException(message);
    }

    private static async Task SucceedAsync()
    {
        await Task.Delay(10);
    }

    [Approach(Name = ".Wait() and AggregateException", Idea = """
        Blocking with `.Wait()` or `.Result` wraps errors in an `AggregateException`, so you have to dig into `InnerExceptions`.
        Blocking also wastes a thread and can deadlock in UI apps. Avoid it.
        """)]
    public static List<string> ErrorsWithWait()
    {
        try
        {
            Task.WaitAll(FailAsync("disk full", 30), SucceedAsync(), FailAsync("network down", 10));
            return new List<string>();
        }
        catch (AggregateException error)
        {
            return error.InnerExceptions.Select(inner => inner.Message).Order().ToList();
        }
    }

    [Approach(Name = "await in try/catch (First Error Only)", Idea = """
        `await` throws the **original** exception, so a normal `catch (InvalidOperationException)` works.
        But with `Task.WhenAll`, `await` only throws the **first** error. The others are easy to miss.
        """)]
    public static async Task<List<string>> ErrorsWithAwait()
    {
        try
        {
            await Task.WhenAll(FailAsync("disk full", 30), SucceedAsync(), FailAsync("network down", 10));
            return new List<string>();
        }
        catch (InvalidOperationException error)
        {
            return new List<string> { error.Message };
        }
    }

    [Approach(Name = "await, Then Read All Errors From the Task", Idea = """
        Keep a reference to the `WhenAll` task. After catching, its `Exception.InnerExceptions` holds **every** error.
        This gives you clean `await` code without losing information.
        """)]
    public static async Task<List<string>> AllErrorsWithAwait()
    {
        Task all = Task.WhenAll(FailAsync("disk full", 30), SucceedAsync(), FailAsync("network down", 10));
        try
        {
            await all;
            return new List<string>();
        }
        catch
        {
            return all.Exception!.InnerExceptions.Select(inner => inner.Message).Order().ToList();
        }
    }

    public static void Demo()
    {
        Print(".Wait() → AggregateException", ErrorsWithWait(), expected: new[] { "disk full", "network down" });
        Print("await → first error only", ErrorsWithAwait().Result.Count, expected: 1);
        Print("await + task.Exception → all errors", AllErrorsWithAwait().Result, expected: new[] { "disk full", "network down" });
    }
}
