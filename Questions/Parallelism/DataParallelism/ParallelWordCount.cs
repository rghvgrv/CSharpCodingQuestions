namespace CSharpCodingQuestions.Questions.Parallelism.DataParallelism;

[Question(Order = 7, Title = "Count Words Across Many Documents (Map-Reduce)", Level = Medium, Problem = """
    Count how often each word appears across 2,000 documents, using all cores.
    The results must match a normal single-threaded count exactly.
    """)]
public static class ParallelWordCount
{
    [Approach(Name = "One Thread, One Dictionary", Idea = """
        Loop over the documents and words, counting in a normal `Dictionary`. Simple and correct, but it uses one core.
        """)]
    public static Dictionary<string, int> CountSequential(string[] documents)
    {
        var counts = new Dictionary<string, int>();
        foreach (string document in documents)
        {
            foreach (string word in document.Split(' '))
            {
                counts[word] = counts.GetValueOrDefault(word) + 1;
            }
        }
        return counts;
    }

    [Approach(Name = "Parallel.ForEach + ConcurrentDictionary", Idea = """
        Process documents in parallel. A normal `Dictionary` would break with many threads writing to it,
        so use `ConcurrentDictionary`. Its `AddOrUpdate` changes one key safely and atomically.
        """)]
    public static Dictionary<string, int> CountWithConcurrentDictionary(string[] documents)
    {
        var counts = new ConcurrentDictionary<string, int>();
        Parallel.ForEach(documents, document =>
        {
            foreach (string word in document.Split(' '))
            {
                counts.AddOrUpdate(word, 1, (key, old) => old + 1);
            }
        });
        return new Dictionary<string, int>(counts);
    }

    [Approach(Name = "PLINQ Map-Reduce", Idea = """
        The classic **map-reduce** shape, written as one parallel query:

        - **Map**: split every document into words (`SelectMany`).
        - **Shuffle**: bring equal words together (`GroupBy`).
        - **Reduce**: count each group (`Count`).

        PLINQ runs each step in parallel and combines the pieces for you.
        """)]
    public static Dictionary<string, int> CountWithPlinq(string[] documents)
    {
        return documents
            .AsParallel()
            .SelectMany(document => document.Split(' '))
            .GroupBy(word => word)
            .ToDictionary(group => group.Key, group => group.Count());
    }

    public static void Demo()
    {
        string[] vocabulary = ["the", "quick", "brown", "fox", "jumps", "over", "lazy", "dog"];
        var random = new Random(1);
        string[] documents = Enumerable.Range(0, 2_000)
            .Select(_ => string.Join(' ', Enumerable.Range(0, 300).Select(_ => vocabulary[random.Next(vocabulary.Length)])))
            .ToArray();

        Dictionary<string, int> expected = CountSequential(documents);
        bool SameAs(Dictionary<string, int> counts) => counts.Count == expected.Count && counts.All(pair => expected[pair.Key] == pair.Value);

        Print("ConcurrentDictionary matches", SameAs(CountWithConcurrentDictionary(documents)), expected: true);
        Print("PLINQ matches", SameAs(CountWithPlinq(documents)), expected: true);
        Print("Total words counted", expected.Values.Sum(), expected: 600_000);
        foreach (var (word, count) in expected.OrderByDescending(pair => pair.Value).Take(3))
        {
            Console.WriteLine($"  {word}: {count:N0}");
        }
    }
}
