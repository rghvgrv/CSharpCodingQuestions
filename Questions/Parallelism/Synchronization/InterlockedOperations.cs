namespace CodingQuestions.Parallelism.Synchronization;

[Q(3_02_02, "Interlocked & Compare-And-Swap", Medium,
"Use lock-free atomic operations: Increment, Add, Exchange, and a CompareExchange loop that tracks a thread-safe maximum.")]
public static class InterlockedOperations
{
    // Interlocked ops are single CPU instructions: faster than a lock for simple updates.
    // CompareExchange(ref x, new, expected) sets x = new only if x still equals expected, and returns the old value.
    // Retry loop = the general recipe for any lock-free update.
    static void UpdateMax(ref int target, int value)
    {
        int current = Volatile.Read(ref target);
        while (value > current)
        {
            int seen = Interlocked.CompareExchange(ref target, value, current);
            if (seen == current) return; // we won
            current = seen;              // someone changed it; retry with the new value
        }
    }

    public static void Run()
    {
        int count = 0, max = int.MinValue;
        long total = 0;
        Parallel.For(0, 100_000, i =>
        {
            Interlocked.Increment(ref count);
            Interlocked.Add(ref total, i);
            UpdateMax(ref max, (i * 7919) % 100_003);
        });
        Check("Increment ×100000", count, 100_000);
        Check("Add 0..99999", total, 4_999_950_000L);
        Check("CAS max of (i·7919) mod 100003", max, Enumerable.Range(0, 100_000).Max(i => (i * 7919) % 100_003));

        // Exchange: atomically swap in a value and get the old one (e.g. "only the first caller runs this").
        int flag = 0;
        int winners = 0;
        Parallel.For(0, 50, _ => { if (Interlocked.Exchange(ref flag, 1) == 0) Interlocked.Increment(ref winners); });
        Check("Only one thread wins Exchange(ref flag, 1)", winners, 1);
    }
}
