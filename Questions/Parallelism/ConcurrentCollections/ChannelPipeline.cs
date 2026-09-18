namespace CSharpCodingQuestions.Questions.Parallelism.ConcurrentCollections;

[Question(Order = 5, Title = "A Processing Pipeline With Channels", Level = Hard, Problem = """
    Build a 3-stage pipeline, like an assembly line where every stage works at the same time:

    1. **Generate** the numbers 1 to 100.
    2. **Square** them. This stage is slow, so give it 3 workers.
    3. **Add up** the results.
    """)]
public static class ChannelPipeline
{
    [Approach(Name = "Stages Connected by Channels", Idea = """
        Each stage reads from the channel before it and writes to the channel after it. All stages run at the same time,
        so while stage 3 adds up early results, stage 1 is still generating.

        The key detail is **completion**: when a stage's input is finished and all its workers are done,
        it completes its own output channel. "Done" flows down the pipeline like a signal, and every loop ends cleanly.
        """)]
    public static long RunPipeline(int count, int squareWorkers)
    {
        Channel<int> numbers = Channel.CreateBounded<int>(10);
        Channel<long> squares = Channel.CreateBounded<long>(10);

        Task generate = Task.Run(async () =>
        {
            for (int i = 1; i <= count; i++)
            {
                await numbers.Writer.WriteAsync(i);
            }
            numbers.Writer.Complete();
        });

        Task[] squareStage = Enumerable.Range(0, squareWorkers).Select(_ => Task.Run(async () =>
        {
            await foreach (int number in numbers.Reader.ReadAllAsync())
            {
                await Task.Delay(1);   // pretend this stage is slow
                await squares.Writer.WriteAsync((long)number * number);
            }
        })).ToArray();

        Task closeSquares = Task.WhenAll(squareStage).ContinueWith(_ => squares.Writer.Complete());

        Task<long> sumStage = Task.Run(async () =>
        {
            long total = 0;
            await foreach (long square in squares.Reader.ReadAllAsync())
            {
                total += square;
            }
            return total;
        });

        Task.WaitAll(generate, closeSquares);
        return sumStage.Result;
    }

    public static void Demo()
    {
        var clock = Stopwatch.StartNew();
        Print("Sum of squares 1² + 2² + … + 100²", RunPipeline(100, squareWorkers: 3), expected: 338350L);
        Console.WriteLine($"  pipeline finished in {clock.ElapsedMilliseconds} ms");
    }
}
