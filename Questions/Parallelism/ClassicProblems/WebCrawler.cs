namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 8, Title = "Concurrent Web Crawler", Level = Medium, Problem = """
    Starting from one URL, visit every page on the **same site** by following links. Loading a page takes 30 ms.
    Never load the same page twice. (LeetCode 1242)
    """)]
public static class WebCrawler
{
    // A tiny fake internet: page → links on that page.
    private static readonly Dictionary<string, string[]> FakeWeb = new()
    {
        ["http://news.site/"] = ["http://news.site/a", "http://news.site/b", "http://other.site/"],
        ["http://news.site/a"] = ["http://news.site/c", "http://news.site/"],
        ["http://news.site/b"] = ["http://news.site/c", "http://news.site/d"],
        ["http://news.site/c"] = ["http://news.site/e"],
        ["http://news.site/d"] = [],
        ["http://news.site/e"] = ["http://news.site/a"],
        ["http://other.site/"] = ["http://other.site/x"],
    };

    private static async Task<string[]> GetLinksAsync(string url)
    {
        await Task.Delay(30);   // network time
        return FakeWeb.GetValueOrDefault(url, []);
    }

    [Approach(Name = "One Page at a Time (BFS)", Idea = """
        A normal breadth-first search: a queue of pages to load and a `HashSet` of pages already seen.
        Only one page is loaded at a time, so the total time is 30 ms × the number of pages.
        """)]
    public static async Task<List<string>> CrawlSequential(string start)
    {
        string host = new Uri(start).Host;
        var seen = new HashSet<string> { start };
        var queue = new Queue<string>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            string url = queue.Dequeue();
            foreach (string link in await GetLinksAsync(url))
            {
                if (new Uri(link).Host == host && seen.Add(link))
                {
                    queue.Enqueue(link);
                }
            }
        }
        return seen.Order().ToList();
    }

    [Approach(Name = "Load Links Concurrently", Idea = """
        When a page is loaded, start loading **all** its new links at once, and wait for them together with `Task.WhenAll`.

        Many tasks now discover links at the same time, so "have I seen this page?" must be **atomic**:
        `ConcurrentDictionary.TryAdd` succeeds for exactly one task per URL, and only that task loads the page.
        """)]
    public static async Task<List<string>> CrawlConcurrent(string start)
    {
        string host = new Uri(start).Host;
        var seen = new ConcurrentDictionary<string, bool>();

        async Task Visit(string url)
        {
            if (new Uri(url).Host != host || !seen.TryAdd(url, true))
            {
                return;
            }
            string[] links = await GetLinksAsync(url);
            await Task.WhenAll(links.Select(Visit));
        }

        await Visit(start);
        return seen.Keys.Order().ToList();
    }

    public static void Demo()
    {
        var expected = new[] { "http://news.site/", "http://news.site/a", "http://news.site/b", "http://news.site/c", "http://news.site/d", "http://news.site/e" };

        var clock = Stopwatch.StartNew();
        Print("One page at a time", CrawlSequential("http://news.site/").Result, expected: expected);
        Console.WriteLine($"  {clock.ElapsedMilliseconds} ms");

        clock.Restart();
        Print("Concurrent", CrawlConcurrent("http://news.site/").Result, expected: expected);
        Console.WriteLine($"  {clock.ElapsedMilliseconds} ms");
    }
}
