namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_08, "Web Crawler Multithreaded", Medium,
"Starting from a URL, crawl every page on the same host concurrently. Fetching a page is slow (simulated 30 ms). Never fetch a page twice. (LeetCode 1242)")]
public static class WebCrawlerMultithreaded
{
    // Fake web: page → links
    static readonly Dictionary<string, string[]> Web = new()
    {
        ["http://news.site.com"] = ["http://news.site.com/a", "http://news.site.com/b", "http://other.com"],
        ["http://news.site.com/a"] = ["http://news.site.com/c", "http://news.site.com"],
        ["http://news.site.com/b"] = ["http://news.site.com/c", "http://news.site.com/d"],
        ["http://news.site.com/c"] = ["http://news.site.com/e"],
        ["http://news.site.com/d"] = [],
        ["http://news.site.com/e"] = ["http://news.site.com/a"],
        ["http://other.com"] = ["http://other.com/x"],
    };

    static async Task<string[]> GetLinksAsync(string url)
    {
        await Task.Delay(30); // network latency
        return Web.GetValueOrDefault(url, []);
    }

    // `visited.TryAdd` is the atomic "claim": only the thread that adds a URL crawls it.
    // Each page's children are crawled concurrently; we wait for the whole tree with WhenAll.
    public static async Task<List<string>> Crawl(string start)
    {
        string host = new Uri(start).Host;
        var visited = new ConcurrentDictionary<string, byte>();

        async Task Visit(string url)
        {
            if (new Uri(url).Host != host || !visited.TryAdd(url, 0)) return;
            var links = await GetLinksAsync(url);
            await Task.WhenAll(links.Select(Visit));
        }

        await Visit(start);
        return [.. visited.Keys.Order()];
    }

    public static void Run()
    {
        var clock = Stopwatch.StartNew();
        var pages = Crawl("http://news.site.com").GetAwaiter().GetResult();
        Check("Pages on news.site.com", pages,
            ["http://news.site.com", "http://news.site.com/a", "http://news.site.com/b", "http://news.site.com/c", "http://news.site.com/d", "http://news.site.com/e"]);
        Console.WriteLine($"  crawled {pages.Count} pages in {clock.ElapsedMilliseconds} ms (sequentially: ~{pages.Count * 30} ms+)");
    }
}
