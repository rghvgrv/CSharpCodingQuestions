namespace CodingQuestions.Parallelism.ConcurrentCollections;

[Q(3_05_04, "Producer-Consumer with Channels (async)", Medium,
"Rebuild producer-consumer with System.Threading.Channels: async producers and consumers, a bounded buffer, and clean completion. No threads are blocked while waiting.")]
public static class ProducerConsumerChannels
{
    // Channel<T> is the async version of BlockingCollection: WriteAsync waits (without blocking) when full,
    // ReadAllAsync waits when empty, Writer.Complete() ends the consumers' loops.
    public static void Run() => RunAsync().GetAwaiter().GetResult();

    static async Task RunAsync()
    {
        var channel = Channel.CreateBounded<int>(new BoundedChannelOptions(5) { FullMode = BoundedChannelFullMode.Wait });

        async Task Produce(int id, int count)
        {
            for (int i = 0; i < count; i++)
                await channel.Writer.WriteAsync(id * 100 + i);
        }

        async Task<List<int>> Consume()
        {
            var mine = new List<int>();
            await foreach (int item in channel.Reader.ReadAllAsync())
            {
                await Task.Delay(1); // async work
                mine.Add(item);
            }
            return mine;
        }

        var consumers = Enumerable.Range(0, 3).Select(_ => Consume()).ToArray();
        await Task.WhenAll(Produce(1, 10), Produce(2, 10), Produce(3, 10));
        channel.Writer.Complete(); // all producers are done

        var results = await Task.WhenAll(consumers);
        for (int i = 0; i < results.Length; i++) Console.WriteLine($"  consumer {i} got {results[i].Count} items");
        var all = results.SelectMany(r => r).Order().ToArray();
        Check("All 30 items consumed exactly once", all.Length == 30 && all.Distinct().Count() == 30, true);
        Check("Items from producer 2", all.Where(x => x / 100 == 2).ToArray(), Enumerable.Range(200, 10).ToArray());
    }
}
