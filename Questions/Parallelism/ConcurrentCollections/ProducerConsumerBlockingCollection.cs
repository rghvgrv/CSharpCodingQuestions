namespace CodingQuestions.Parallelism.ConcurrentCollections;

[Q(3_05_03, "Producer-Consumer with BlockingCollection", Medium,
"One producer creates 20 jobs; two consumers process them. The buffer holds at most 5 jobs, so a fast producer must wait. Shut down cleanly when the producer is done.")]
public static class ProducerConsumerBlockingCollection
{
    // BlockingCollection wraps a ConcurrentQueue:
    // Add blocks when full (back-pressure), Take blocks when empty, CompleteAdding tells consumers to finish.
    public static void Run()
    {
        using var buffer = new BlockingCollection<int>(boundedCapacity: 5);
        var processed = new ConcurrentBag<(int Job, string Consumer)>();

        var producer = new Thread(() =>
        {
            for (int job = 1; job <= 20; job++)
            {
                buffer.Add(job); // waits while 5 items are already buffered
                if (job % 5 == 0) Console.WriteLine($"  producer added job {job} (buffer: {buffer.Count})");
            }
            buffer.CompleteAdding(); // no more items; consumers' loops end once the buffer is empty
        });

        Thread Consumer(string name) => new(() =>
        {
            foreach (int job in buffer.GetConsumingEnumerable())
            {
                Thread.Sleep(5); // work
                processed.Add((job, name));
            }
        });

        var consumers = new[] { Consumer("A"), Consumer("B") };
        producer.Start();
        foreach (var c in consumers) c.Start();
        producer.Join();
        foreach (var c in consumers) c.Join();

        Check("Every job processed exactly once", processed.Select(p => p.Job).Order().ToArray(), Enumerable.Range(1, 20).ToArray());
        foreach (var g in processed.GroupBy(p => p.Consumer).OrderBy(g => g.Key))
            Console.WriteLine($"  consumer {g.Key} handled {g.Count()} jobs");
    }
}
