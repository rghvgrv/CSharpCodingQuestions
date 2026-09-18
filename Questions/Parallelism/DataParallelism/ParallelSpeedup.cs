namespace CodingQuestions.Parallelism.DataParallelism;

[Q(3_04_04, "Measuring Speedup (Amdahl's Law)", Medium,
"Count primes below 1,000,000 sequentially and in parallel. Measure the speedup and compare it with the number of CPU cores.")]
public static class ParallelSpeedup
{
    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int d = 2; (long)d * d <= n; d++) if (n % d == 0) return false;
        return true;
    }

    // Speedup = T(sequential) / T(parallel). Best case ≈ number of cores.
    // Amdahl's law: if a fraction p of the work can run in parallel, max speedup = 1 / ((1 - p) + p / cores).
    public static void Run()
    {
        const int limit = 1_000_000;

        var clock = Stopwatch.StartNew();
        int sequential = 0;
        for (int n = 0; n < limit; n++) if (IsPrime(n)) sequential++;
        double seqMs = clock.Elapsed.TotalMilliseconds;

        clock.Restart();
        int parallel = 0;
        Parallel.For(0, limit, () => 0, (n, _, local) => IsPrime(n) ? local + 1 : local, local => Interlocked.Add(ref parallel, local));
        double parMs = clock.Elapsed.TotalMilliseconds;

        Check("Sequential count", sequential, 78498);
        Check("Parallel count", parallel, 78498);
        int cores = Environment.ProcessorCount;
        Console.WriteLine($"Sequential {seqMs:0} ms · Parallel {parMs:0} ms · speedup {seqMs / parMs:0.0}× on {cores} cores");

        foreach (double p in new[] { 0.5, 0.9, 0.99 })
            Console.WriteLine($"  Amdahl: {p:P0} parallel work on {cores} cores → max {1 / ((1 - p) + p / cores):0.0}×");
    }
}
