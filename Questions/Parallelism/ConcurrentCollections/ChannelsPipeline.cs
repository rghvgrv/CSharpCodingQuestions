namespace CodingQuestions.Parallelism.ConcurrentCollections;

[Q(3_05_05, "Multi-Stage Pipeline with Channels", Hard,
"Build a 3-stage pipeline: generate numbers → square them (3 parallel workers) → add up the results. Each stage runs concurrently and passes data through a channel.")]
public static class ChannelsPipeline
{
    // Stage N reads from channel N-1 and writes to channel N. Every stage completes its output when its input is done,
    // so completion flows down the pipeline like a "poison pill".
    static ChannelReader<int> Generate(int count)
    {
        var output = Channel.CreateBounded<int>(10);
        _ = Task.Run(async () =>
        {
            for (int i = 1; i <= count; i++) await output.Writer.WriteAsync(i);
            output.Writer.Complete();
        });
        return output.Reader;
    }

    static ChannelReader<long> Square(ChannelReader<int> input, int workers)
    {
        var output = Channel.CreateBounded<long>(10);
        var tasks = Enumerable.Range(0, workers).Select(_ => Task.Run(async () =>
        {
            await foreach (int x in input.ReadAllAsync())
            {
                await Task.Delay(1); // pretend this stage is slow, so it gets several workers
                await output.Writer.WriteAsync((long)x * x);
            }
        }));
        _ = Task.WhenAll(tasks).ContinueWith(t => output.Writer.Complete(t.Exception));
        return output.Reader;
    }

    static async Task<long> Sum(ChannelReader<long> input)
    {
        long total = 0;
        await foreach (long x in input.ReadAllAsync()) total += x;
        return total;
    }

    public static void Run()
    {
        var clock = Stopwatch.StartNew();
        long total = Sum(Square(Generate(100), workers: 3)).GetAwaiter().GetResult();
        Check("Σ i² for i = 1..100", total, 338350L);
        Console.WriteLine($"  pipeline finished in {clock.ElapsedMilliseconds} ms");
    }
}
