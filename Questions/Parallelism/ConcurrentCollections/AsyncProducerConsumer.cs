namespace CSharpCodingQuestions.Questions.Parallelism.ConcurrentCollections;

[Question(Order = 4, Title = "Async Producer and Consumers (Channels)", Level = Medium, Problem = """
    Same producer/consumer job as before, but the work is **async** (for example, saving to a database).
    3 producers and 3 consumers share a buffer of at most 5 items, and no thread should be blocked while waiting.
    """)]
public static class AsyncProducerConsumer
{
    [Approach(Name = "BlockingCollection", Idea = """
        `BlockingCollection` works, but waiting **blocks a real thread**. With async code, that wastes thread-pool threads
        that other requests could be using.
        """)]
    public static List<int> RunWithBlockingCollection()
    {
        using var buffer = new BlockingCollection<int>(boundedCapacity: 5);
        var consumed = new ConcurrentBag<int>();

        Task[] consumers = Enumerable.Range(0, 3).Select(_ => Task.Run(() =>
        {
            foreach (int item in buffer.GetConsumingEnumerable())
            {
                consumed.Add(item);
            }
        })).ToArray();

        Task[] producers = Enumerable.Range(1, 3).Select(producer => Task.Run(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                buffer.Add(producer * 100 + i);
            }
        })).ToArray();

        Task.WaitAll(producers);
        buffer.CompleteAdding();
        Task.WaitAll(consumers);
        return consumed.Order().ToList();
    }

    [Approach(Name = "Channel", Idea = """
        `System.Threading.Channels` is the async version: `await writer.WriteAsync(item)` and `await foreach (… in reader.ReadAllAsync())`
        wait **without blocking a thread**.

        - `Channel.CreateBounded<T>(5)`: at most 5 items, so writers wait when it's full (back-pressure).
        - `writer.Complete()`: no more items; the readers' loops end once the channel is empty.
        """)]
    public static async Task<List<int>> RunWithChannel()
    {
        Channel<int> channel = Channel.CreateBounded<int>(5);
        var consumed = new ConcurrentBag<int>();

        Task[] consumers = Enumerable.Range(0, 3).Select(async _ =>
        {
            await foreach (int item in channel.Reader.ReadAllAsync())
            {
                await Task.Delay(1);   // pretend async work
                consumed.Add(item);
            }
        }).ToArray();

        Task[] producers = Enumerable.Range(1, 3).Select(async producer =>
        {
            for (int i = 0; i < 10; i++)
            {
                await channel.Writer.WriteAsync(producer * 100 + i);
            }
        }).ToArray();

        await Task.WhenAll(producers);
        channel.Writer.Complete();
        await Task.WhenAll(consumers);
        return consumed.Order().ToList();
    }

    public static void Demo()
    {
        List<int> expected = Enumerable.Range(1, 3).SelectMany(producer => Enumerable.Range(producer * 100, 10)).ToList();
        Print("BlockingCollection: all 30 items consumed once", RunWithBlockingCollection(), expected: expected);
        Print("Channel: all 30 items consumed once", RunWithChannel().Result, expected: expected);
    }
}
