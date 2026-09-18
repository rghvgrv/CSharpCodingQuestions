namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_10, "TaskCompletionSource: Wrap a Callback API as a Task", Medium,
"An old library reports results through callbacks (onDone / onError). Wrap it so callers can simply await it.")]
public static class TaskCompletionSourceDemo
{
    // Pretend legacy API: does the work on a background thread and calls back later.
    class LegacyScanner
    {
        public void Scan(string file, Action<int> onDone, Action<Exception> onError)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(50);
                if (file.EndsWith(".exe")) onError(new InvalidDataException($"{file} is infected"));
                else onDone(file.Length * 100);
            });
        }
    }

    // TaskCompletionSource<T> is a Task you complete yourself: SetResult / SetException / SetCanceled.
    // RunContinuationsAsynchronously keeps awaiting code from running inline on the callback thread.
    static Task<int> ScanAsync(this LegacyScanner scanner, string file)
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        scanner.Scan(file, bytes => tcs.TrySetResult(bytes), error => tcs.TrySetException(error));
        return tcs.Task;
    }

    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        var scanner = new LegacyScanner();
        Check("await ScanAsync(\"notes.txt\")", await scanner.ScanAsync("notes.txt"), 900);
        try
        {
            await scanner.ScanAsync("game.exe");
            Check("ScanAsync(\"game.exe\")", "no error", "error");
        }
        catch (InvalidDataException e) { Check("ScanAsync(\"game.exe\") throws", e.Message, "game.exe is infected"); }

        // It composes like any other task:
        var sizes = await Task.WhenAll(new[] { "a.txt", "bb.txt", "ccc.txt" }.Select(scanner.ScanAsync));
        Check("WhenAll over wrapped callbacks", sizes, [500, 600, 700]);
    }
}
