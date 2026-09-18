namespace CSharpCodingQuestions.Questions.Parallelism.ThreadBasics;

[Question(Order = 1, Title = "Your First Threads", Level = Easy, Problem = """
    Three jobs each take about 100 ms (simulated with `Thread.Sleep`). Run them so the whole thing finishes as soon as possible,
    and collect each job's result.
    """)]
public static class CreatingThreads
{
    [Approach(Name = "One After Another", Idea = """
        Call the jobs in order on the current thread. Each one has to wait for the previous one: about 3 × 100 ms.
        """)]
    public static int[] RunSequentially()
    {
        int[] results = new int[3];
        for (int job = 0; job < 3; job++)
        {
            Thread.Sleep(100);           // pretend to work
            results[job] = job * 10;
        }
        return results;
    }

    [Approach(Name = "One Thread per Job", Idea = """
        Create a `Thread` for each job and `Start()` them all, so they run at the same time.
        Then `Join()` each thread, which waits until it has finished, before reading the results.

        Each thread writes only to **its own** slot of the array, so they don't interfere with each other.
        Note `int job = i;`: each thread needs its own copy of the loop number.
        """)]
    public static int[] RunOnThreads()
    {
        int[] results = new int[3];
        var threads = new List<Thread>();

        for (int i = 0; i < 3; i++)
        {
            int job = i;
            var thread = new Thread(() =>
            {
                Thread.Sleep(100);       // pretend to work
                results[job] = job * 10;
            });
            threads.Add(thread);
            thread.Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        return results;
    }

    public static void Demo()
    {
        var clock = Stopwatch.StartNew();
        int[] sequential = RunSequentially();
        long sequentialMs = clock.ElapsedMilliseconds;

        clock.Restart();
        int[] parallel = RunOnThreads();
        long parallelMs = clock.ElapsedMilliseconds;

        Print("One after another", $"{Formatter.Format(sequential)} in {sequentialMs} ms");
        Print("One thread per job", $"{Formatter.Format(parallel)} in {parallelMs} ms");
        Print("Same results", sequential.SequenceEqual(parallel), expected: true);
        Print("Threads were faster", parallelMs < sequentialMs, expected: true);
    }
}
