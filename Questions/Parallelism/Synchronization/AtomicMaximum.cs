namespace CSharpCodingQuestions.Questions.Parallelism.Synchronization;

[Question(Order = 2, Title = "Lock-Free Updates With Interlocked", Level = Medium, Problem = """
    Many threads report numbers, and we must keep track of the **largest** number reported, safely and fast.
    """)]
public static class AtomicMaximum
{
    [Approach(Name = "lock", Idea = """
        Take a lock, compare, update. Simple and correct, but every thread must wait its turn, even when there's nothing to update.
        """)]
    public static int MaxWithLock(int[] numbers)
    {
        int max = int.MinValue;
        object gate = new object();
        Parallel.ForEach(numbers, number =>
        {
            lock (gate)
            {
                if (number > max)
                {
                    max = number;
                }
            }
        });
        return max;
    }

    [Approach(Name = "Compare-And-Swap Loop", Idea = """
        `Interlocked.CompareExchange(ref max, newValue, expected)` does this as **one atomic step**:
        "if `max` still equals `expected`, set it to `newValue`". It returns what `max` was.

        1. Read the current max.
        2. If our number isn't bigger, stop.
        3. Try to swap it in. If another thread changed `max` in the meantime, the swap fails; read again and retry.

        No thread ever waits for a lock. This retry loop is the general recipe for lock-free updates.
        """)]
    public static int MaxWithCompareExchange(int[] numbers)
    {
        int max = int.MinValue;
        Parallel.ForEach(numbers, number =>
        {
            int current = Volatile.Read(ref max);
            while (number > current)
            {
                int seen = Interlocked.CompareExchange(ref max, number, current);
                if (seen == current)
                {
                    break;           // our swap worked
                }
                current = seen;      // someone else changed it: try again with the new value
            }
        });
        return max;
    }

    public static void Demo()
    {
        int[] numbers = Enumerable.Range(0, 1_000_000).Select(i => (int)((long)i * 7919 % 1_000_003)).ToArray();
        int expected = numbers.Max();

        var clock = Stopwatch.StartNew();
        int withLock = MaxWithLock(numbers);
        long lockMs = clock.ElapsedMilliseconds;

        clock.Restart();
        int withCas = MaxWithCompareExchange(numbers);
        long casMs = clock.ElapsedMilliseconds;

        Print($"lock ({lockMs} ms)", withLock, expected: expected);
        Print($"Compare-and-swap ({casMs} ms)", withCas, expected: expected);
    }
}
