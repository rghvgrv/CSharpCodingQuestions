using System.Runtime.CompilerServices;

namespace CodingQuestions.Parallelism.TasksAndAsync;

[Q(3_03_08, "Async Streams: IAsyncEnumerable & await foreach", Medium,
"Produce values one at a time as they become available (like pages from an API), consume them with await foreach, and stop early with a cancellation token.")]
public static class AsyncStreams
{
    // async + yield return = a stream whose items arrive over time. The consumer processes item 1
    // while item 2 is still being fetched, instead of waiting for the whole list.
    static async IAsyncEnumerable<int> FetchPages(int pages, [EnumeratorCancellation] CancellationToken ct = default)
    {
        for (int p = 1; p <= pages; p++)
        {
            await Task.Delay(30, ct); // fetch page p
            Console.WriteLine($"  fetched page {p}");
            yield return p;
        }
    }

    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        var got = new List<int>();
        await foreach (int page in FetchPages(5))
        {
            Console.WriteLine($"  processing page {page}");
            got.Add(page);
        }
        Check("All pages", got, [1, 2, 3, 4, 5]);

        // Stop after 100 ms: WithCancellation passes the token into the producer.
        using var cts = new CancellationTokenSource(100);
        var partial = new List<int>();
        try
        {
            await foreach (int page in FetchPages(100).WithCancellation(cts.Token)) partial.Add(page);
        }
        catch (OperationCanceledException) { }
        Console.WriteLine($"  canceled after {partial.Count} pages");
        Check("Stopped early", partial.Count is > 0 and < 100, true);

        // LINQ works on async streams too (System.Linq.AsyncEnumerable, .NET 10)
        Check("Take(3) of a stream", await FetchPages(10).Take(3).ToListAsync(), [1, 2, 3]);
    }
}
