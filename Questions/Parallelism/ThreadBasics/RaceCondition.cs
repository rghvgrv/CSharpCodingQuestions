namespace CodingQuestions.Parallelism.ThreadBasics;

[Q(3_01_03, "Race Condition", Easy,
"Four threads each increment a shared counter 100,000 times. Show why the result is wrong without synchronization, then fix it.")]
public static class RaceCondition
{
    // counter++ is read → add → write. Two threads can read the same value and one update is lost.
    public static void Run()
    {
        const int threads = 4, perThread = 100_000;

        int unsafeCounter = 0;
        RunThreads(threads, () => { for (int i = 0; i < perThread; i++) unsafeCounter++; });
        Console.WriteLine($"Without sync : {unsafeCounter:N0} (expected {threads * perThread:N0}, updates lost: {threads * perThread - unsafeCounter:N0})");

        int lockedCounter = 0;
        var gate = new object();
        RunThreads(threads, () => { for (int i = 0; i < perThread; i++) lock (gate) lockedCounter++; });
        Check("With lock", lockedCounter, threads * perThread);

        int atomicCounter = 0;
        RunThreads(threads, () => { for (int i = 0; i < perThread; i++) Interlocked.Increment(ref atomicCounter); });
        Check("With Interlocked", atomicCounter, threads * perThread);
    }

    static void RunThreads(int count, Action work)
    {
        var all = Enumerable.Range(0, count).Select(_ => new Thread(() => work())).ToList();
        all.ForEach(t => t.Start());
        all.ForEach(t => t.Join());
    }
}
