namespace CodingQuestions.Parallelism.DataParallelism;

[Q(3_04_02, "Parallel Sum: Wrong, Slow and Right", Medium,
"Sum 10 million numbers in parallel. Compare: an unsynchronized shared total (wrong), a lock per item (slow), thread-local subtotals (fast and correct), and PLINQ.")]
public static class ParallelAggregation
{
    public static void Run()
    {
        var data = Enumerable.Range(0, 10_000_000).Select(i => (long)(i % 1000)).ToArray();
        long expected = data.Sum();
        var clock = new Stopwatch();

        // 1) WRONG: `total += x` from many threads loses updates (read-add-write race).
        long racy = 0;
        clock.Restart();
        Parallel.For(0, data.Length, i => racy += data[i]);
        Console.WriteLine($"  1) shared total, no sync : {racy,14:N0}  {(racy == expected ? "(got lucky)" : "WRONG")}  {clock.ElapsedMilliseconds} ms");

        // 2) CORRECT but SLOW: every item fights for the same lock.
        long locked = 0;
        var gate = new Lock();
        clock.Restart();
        Parallel.For(0, data.Length, i => { lock (gate) locked += data[i]; });
        long lockMs = clock.ElapsedMilliseconds;
        Check($"2) lock per item ({lockMs} ms)", locked, expected);

        // 3) RIGHT: each thread sums its own chunk (localInit/body/localFinally), then locks ONCE to merge.
        long merged = 0;
        clock.Restart();
        Parallel.For(0, data.Length,
            localInit: () => 0L,
            body: (i, _, subtotal) => subtotal + data[i],
            localFinally: subtotal => Interlocked.Add(ref merged, subtotal));
        long localMs = clock.ElapsedMilliseconds;
        Check($"3) thread-local subtotals ({localMs} ms)", merged, expected);

        // 4) Simplest: PLINQ does the same partition-and-merge for you.
        clock.Restart();
        long plinq = data.AsParallel().Sum();
        Check($"4) data.AsParallel().Sum() ({clock.ElapsedMilliseconds} ms)", plinq, expected);
    }
}
