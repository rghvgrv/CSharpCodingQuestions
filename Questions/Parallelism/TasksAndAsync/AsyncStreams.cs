using System.Runtime.CompilerServices;

namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 8, Title = "Process Items as They Arrive (IAsyncEnumerable)", Level = Medium, Problem = """
    An API returns 5 pages of results, and each page takes 50 ms to load. Start working on page 1 as soon as it arrives,
    instead of waiting for all 5.
    """)]
public static class AsyncStreams
{
    [Approach(Name = "Load Everything, Then Return a List", Idea = """
        Load all pages into a `List`, then return it. The caller can't see **anything** until the last page arrives (about 250 ms),
        and every page sits in memory at the same time.
        """)]
    public static async Task<List<int>> LoadAllPages(int pageCount)
    {
        var pages = new List<int>();
        for (int page = 1; page <= pageCount; page++)
        {
            await Task.Delay(50);   // load one page
            pages.Add(page);
        }
        return pages;
    }

    [Approach(Name = "Async Stream (yield return)", Idea = """
        Return `IAsyncEnumerable<int>` and `yield return` each page as soon as it's loaded.
        The caller loops with `await foreach` and handles page 1 after about 50 ms, while page 2 is still loading.

        `[EnumeratorCancellation]` lets the caller stop the stream with a token (`.WithCancellation(token)`).
        """)]
    public static async IAsyncEnumerable<int> StreamPages(int pageCount, [EnumeratorCancellation] CancellationToken token = default)
    {
        for (int page = 1; page <= pageCount; page++)
        {
            await Task.Delay(50, token);   // load one page
            yield return page;
        }
    }

    public static void Demo()
    {
        DemoAsync().Wait();
    }

    private static async Task DemoAsync()
    {
        var clock = Stopwatch.StartNew();
        List<int> all = await LoadAllPages(5);
        Print("List: first page available after", $"{clock.ElapsedMilliseconds} ms");

        clock.Restart();
        long firstPageMs = -1;
        var streamed = new List<int>();
        await foreach (int page in StreamPages(5))
        {
            if (firstPageMs < 0)
            {
                firstPageMs = clock.ElapsedMilliseconds;
            }
            streamed.Add(page);
        }
        Print("Stream: first page available after", $"{firstPageMs} ms");
        Print("Both gave all pages", all.SequenceEqual(streamed) && streamed.Count == 5, expected: true);
    }
}
