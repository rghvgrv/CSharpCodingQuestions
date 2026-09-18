# Tasks & async/await

A Task represents work that finishes later and may return a result. async/await lets you wait for that work without blocking a thread, and it's the standard way to do I/O in modern C#.

## Task

```csharp
Task<int> task = Task.Run(() => 6 * 7);   // runs on a thread-pool thread
int answer = await task;                   // 42
```

A `Task` can report its result, report an exception, be cancelled, and be combined with other tasks (`Task.WhenAll`, `Task.WhenAny`).

## async / await

```csharp
async Task<string> DownloadAsync(string url)
{
    using var client = new HttpClient();
    string html = await client.GetStringAsync(url);   // the thread is free while waiting
    return html;
}
```

At `await`, if the work isn't finished, the method **pauses** and gives its thread back. When the work completes, the rest of the method continues, sometimes on a different thread. While 1,000 requests wait on the network, no threads are blocked.

## Rules that save you from bugs

- **Async all the way**: if you call an async method, `await` it. Avoid `.Result` and `.Wait()`, which block a thread and can deadlock.
- Name async methods with `Async` at the end.
- Return `Task` or `Task<T>`, not `void` (except for event handlers).
- Pass a `CancellationToken` into long operations so callers can stop them.
- `async` doesn't make CPU work faster. For CPU-heavy work use `Task.Run` or `Parallel`.

## Combining tasks

| You want | Use |
|---|---|
| Run several at once, wait for all | `await Task.WhenAll(tasks)` |
| Take the first one that finishes | `await Task.WhenAny(tasks)` |
| Give up after a time limit | `await task.WaitAsync(TimeSpan.FromSeconds(5))` |
| Run many, at most N at a time | `Parallel.ForEachAsync` with `MaxDegreeOfParallelism` |
