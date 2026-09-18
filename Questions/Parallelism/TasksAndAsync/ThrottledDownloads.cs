namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 9, Title = "Many Async Calls, a Few at a Time (Parallel.ForEachAsync)", Level = Medium, Problem = """
    Download 20 pages (each takes 50 ms), but never more than 4 downloads at the same time, so the server isn't overloaded.
    """)]
public static class ThrottledDownloads
{
    private static async Task<int> DownloadAsync(string url, CancellationToken token)
    {
        await Task.Delay(50, token);
        return url.Length;
    }

    [Approach(Name = "One at a Time", Idea = """
        A plain `foreach` with `await`: safe for the server, but slow, about 20 × 50 ms.
        """)]
    public static async Task<int> DownloadOneByOne(List<string> urls)
    {
        int totalBytes = 0;
        foreach (string url in urls)
        {
            totalBytes += await DownloadAsync(url, CancellationToken.None);
        }
        return totalBytes;
    }

    [Approach(Name = "All at Once", Idea = """
        Start all 20 and `await Task.WhenAll`. Fast, but all 20 hit the server at the same moment,
        and with 20,000 URLs that would overload it.
        """)]
    public static async Task<int> DownloadAllAtOnce(List<string> urls)
    {
        int[] sizes = await Task.WhenAll(urls.Select(url => DownloadAsync(url, CancellationToken.None)));
        return sizes.Sum();
    }

    [Approach(Name = "Parallel.ForEachAsync With a Limit", Idea = """
        `Parallel.ForEachAsync` (.NET 6+) runs an async body for every item, with at most `MaxDegreeOfParallelism` running at once.
        Here that's 4 at a time: about 20 / 4 × 50 ms. It's the easy, built-in version of the semaphore pattern.
        """)]
    public static async Task<int> DownloadWithLimit(List<string> urls)
    {
        int totalBytes = 0;
        var options = new ParallelOptions { MaxDegreeOfParallelism = 4 };
        await Parallel.ForEachAsync(urls, options, async (url, token) =>
        {
            int size = await DownloadAsync(url, token);
            Interlocked.Add(ref totalBytes, size);
        });
        return totalBytes;
    }

    public static void Demo()
    {
        List<string> urls = Enumerable.Range(1, 20).Select(i => $"https://example.com/page/{i}").ToList();
        int expected = urls.Sum(url => url.Length);

        var clock = Stopwatch.StartNew();
        Print("One at a time", DownloadOneByOne(urls).Result, expected: expected);
        Console.WriteLine($"  {clock.ElapsedMilliseconds} ms");

        clock.Restart();
        Print("All at once", DownloadAllAtOnce(urls).Result, expected: expected);
        Console.WriteLine($"  {clock.ElapsedMilliseconds} ms, but 20 requests hit the server together");

        clock.Restart();
        Print("ForEachAsync, max 4 at once", DownloadWithLimit(urls).Result, expected: expected);
        Console.WriteLine($"  {clock.ElapsedMilliseconds} ms");
    }
}
