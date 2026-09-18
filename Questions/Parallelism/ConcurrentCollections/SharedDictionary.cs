namespace CSharpCodingQuestions.Questions.Parallelism.ConcurrentCollections;

[Question(Order = 1, Title = "A Dictionary Shared by Many Threads", Level = Medium, Problem = """
    Many threads count page visits in one shared dictionary (`page → visits`). 100,000 visits spread over 100 pages
    must give exactly 1,000 visits per page.
    """)]
public static class SharedDictionary
{
    [Approach(Name = "Dictionary + lock", Idea = """
        A normal `Dictionary` is **not** safe when several threads change it at once: updates get lost, or it throws.
        Wrapping every access in one `lock` makes it correct, but only one thread can use the dictionary at a time.
        """)]
    public static Dictionary<string, int> CountWithLock(string[] visits)
    {
        var counts = new Dictionary<string, int>();
        object gate = new object();
        Parallel.ForEach(visits, page =>
        {
            lock (gate)
            {
                counts[page] = counts.GetValueOrDefault(page) + 1;
            }
        });
        return counts;
    }

    [Approach(Name = "ConcurrentDictionary", Idea = """
        `ConcurrentDictionary` is built for sharing: different threads can update different keys at the same time.
        Use its **atomic** methods instead of read-then-write:

        - `AddOrUpdate(key, valueIfNew, (key, old) => newValue)`
        - `GetOrAdd(key, key => create())`
        - `TryRemove(key, out value)`

        Watch out: the function you pass to `GetOrAdd` may run **more than once** if two threads ask for the same new key at the same moment.
        If creating the value is expensive, store a `Lazy<T>` instead, so only one gets built.
        """)]
    public static Dictionary<string, int> CountWithConcurrentDictionary(string[] visits)
    {
        var counts = new ConcurrentDictionary<string, int>();
        Parallel.ForEach(visits, page =>
        {
            counts.AddOrUpdate(page, 1, (key, old) => old + 1);
        });
        return new Dictionary<string, int>(counts);
    }

    public static void Demo()
    {
        string[] visits = Enumerable.Range(0, 100_000).Select(i => $"page{i % 100}").ToArray();

        var clock = Stopwatch.StartNew();
        Dictionary<string, int> locked = CountWithLock(visits);
        Print($"Dictionary + lock ({clock.ElapsedMilliseconds} ms): every page has 1,000", locked.Values.All(count => count == 1000), expected: true);

        clock.Restart();
        Dictionary<string, int> concurrent = CountWithConcurrentDictionary(visits);
        Print($"ConcurrentDictionary ({clock.ElapsedMilliseconds} ms): every page has 1,000", concurrent.Values.All(count => count == 1000), expected: true);
        Print("Pages counted", concurrent.Count, expected: 100);
    }
}
