namespace CodingQuestions.Parallelism.DataParallelism;

[Q(3_04_07, "Map-Reduce Word Count", Medium,
"Count word frequencies across many documents in parallel: map each document to words, then reduce to counts. Do it with PLINQ and with Parallel.ForEach + ConcurrentDictionary.")]
public static class MapReduceWordCount
{
    public static void Run()
    {
        string[] vocab = ["the", "quick", "brown", "fox", "jumps", "over", "lazy", "dog", "and", "cat"];
        var rnd = new Random(5);
        var documents = Enumerable.Range(0, 2_000)
            .Select(_ => string.Join(' ', Enumerable.Range(0, 200).Select(_ => vocab[rnd.Next(vocab.Length)])))
            .ToArray();

        // Sequential reference answer
        var expected = documents.SelectMany(d => d.Split(' ')).CountBy(w => w).ToDictionary();

        // PLINQ: map (SelectMany) → shuffle (GroupBy) → reduce (Count)
        var plinq = documents.AsParallel()
            .SelectMany(d => d.Split(' '))
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count());

        // Parallel.ForEach: count locally per document, then merge into a shared ConcurrentDictionary.
        var shared = new ConcurrentDictionary<string, int>();
        Parallel.ForEach(documents, doc =>
        {
            foreach (var (word, n) in doc.Split(' ').CountBy(w => w))
                shared.AddOrUpdate(word, n, (_, old) => old + n); // atomic per key
        });

        Check("PLINQ matches sequential", expected.OrderBy(kv => kv.Key).SequenceEqual(plinq.OrderBy(kv => kv.Key)), true);
        Check("ConcurrentDictionary matches sequential", expected.OrderBy(kv => kv.Key).SequenceEqual(shared.OrderBy(kv => kv.Key)), true);
        Check("Total words", shared.Values.Sum(), 2_000 * 200);
        foreach (var (word, n) in shared.OrderByDescending(kv => kv.Value).Take(3)) Console.WriteLine($"  {word,-6} {n:N0}");
    }
}
