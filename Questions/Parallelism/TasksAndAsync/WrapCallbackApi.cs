namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 10, Title = "Turn a Callback API Into a Task (TaskCompletionSource)", Level = Medium, Problem = """
    An old library reports its result through callbacks: `Scan(file, onDone, onError)`. Wrap it so callers can simply write
    `int size = await ScanAsync(file);`.
    """)]
public static class WrapCallbackApi
{
    // The old library: it works in the background and calls you back later.
    public class OldScanner
    {
        public void Scan(string file, Action<int> onDone, Action<Exception> onError)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(30);
                if (file.EndsWith(".exe"))
                {
                    onError(new InvalidDataException($"{file} is blocked"));
                }
                else
                {
                    onDone(file.Length * 100);
                }
            });
        }
    }

    [Approach(Name = "TaskCompletionSource", Idea = """
        A `TaskCompletionSource<T>` gives you a `Task` that **you** decide when to finish:

        - `SetResult(value)` completes it successfully, and `await` returns the value.
        - `SetException(error)` makes `await` throw that error.

        Create one, start the old operation with callbacks that complete it, and return its `Task`.
        `RunContinuationsAsynchronously` stops the caller's code from running on the library's callback thread.
        """)]
    public static Task<int> ScanAsync(OldScanner scanner, string file)
    {
        var completion = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        scanner.Scan(
            file,
            size => completion.SetResult(size),
            error => completion.SetException(error));
        return completion.Task;
    }

    public static void Demo()
    {
        var scanner = new OldScanner();
        Print("await ScanAsync(\"notes.txt\")", ScanAsync(scanner, "notes.txt").Result, expected: 900);

        try
        {
            ScanAsync(scanner, "game.exe").Wait();
        }
        catch (AggregateException error)
        {
            Print("ScanAsync(\"game.exe\") throws", error.InnerException!.Message, expected: "game.exe is blocked");
        }

        int[] sizes = Task.WhenAll(new[] { "a.txt", "bb.txt" }.Select(file => ScanAsync(scanner, file))).Result;
        Print("Works with Task.WhenAll too", sizes, expected: new[] { 500, 600 });
    }
}
