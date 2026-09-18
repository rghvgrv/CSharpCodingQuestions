namespace CSharpCodingQuestions.Questions.Parallelism.DataParallelism;

[Question(Order = 2, Title = "Add Up Numbers in Parallel", Level = Medium, Problem = """
    Add up 10 million numbers using all cores. This is harder than it looks: all threads want to update the **same** total.
    """)]
public static class ParallelSum
{
    [Approach(Name = "Shared Total, No Protection (Broken)", Idea = """
        Every thread does `total += value` on one shared variable. That's a read-add-write race, so updates get lost and the sum is wrong.
        """)]
    public static long SumRacy(long[] numbers)
    {
        long total = 0;
        Parallel.For(0, numbers.Length, i =>
        {
            total += numbers[i];
        });
        return total;
    }

    [Approach(Name = "Shared Total With a lock", Idea = """
        A `lock` around each addition makes it correct. But now 10 million tiny additions queue up for **one** lock,
        and the threads spend their time waiting for each other. Often slower than a normal loop.
        """)]
    public static long SumWithLock(long[] numbers)
    {
        long total = 0;
        object gate = new object();
        Parallel.For(0, numbers.Length, i =>
        {
            lock (gate)
            {
                total += numbers[i];
            }
        });
        return total;
    }

    [Approach(Name = "Private Subtotal per Thread", Idea = """
        Give each thread its **own** subtotal, and combine the subtotals only once at the end.
        This `Parallel.For` overload does exactly that:

        - `localInit`: each thread starts its own subtotal at 0
        - `body`: adds to that thread's subtotal, with no sharing and no locking
        - `localFinally`: runs once per thread and adds its subtotal to the shared total (atomically)
        """)]
    public static long SumWithSubtotals(long[] numbers)
    {
        long total = 0;
        Parallel.For(0, numbers.Length,
            localInit: () => 0L,
            body: (i, loopState, subtotal) => subtotal + numbers[i],
            localFinally: subtotal => Interlocked.Add(ref total, subtotal));
        return total;
    }

    [Approach(Name = "PLINQ", Idea = """
        `AsParallel().Sum()` does the same split-and-combine for you, in one line.
        """)]
    public static long SumWithPlinq(long[] numbers)
    {
        return numbers.AsParallel().Sum();
    }

    public static void Demo()
    {
        long[] numbers = Enumerable.Range(0, 10_000_000).Select(i => (long)(i % 1000)).ToArray();
        long expected = numbers.Sum();

        var clock = Stopwatch.StartNew();
        long racy = SumRacy(numbers);
        Print("No protection", $"{racy:N0} {(racy == expected ? "(lucky this time)" : "← WRONG")} in {clock.ElapsedMilliseconds} ms");

        clock.Restart();
        Print("lock per addition", SumWithLock(numbers), expected: expected);
        Console.WriteLine($"  {clock.ElapsedMilliseconds} ms");

        clock.Restart();
        Print("Private subtotals", SumWithSubtotals(numbers), expected: expected);
        Console.WriteLine($"  {clock.ElapsedMilliseconds} ms");

        clock.Restart();
        Print("PLINQ", SumWithPlinq(numbers), expected: expected);
        Console.WriteLine($"  {clock.ElapsedMilliseconds} ms");
    }
}
